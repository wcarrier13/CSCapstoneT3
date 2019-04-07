using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lizst.Models;
using System.Text.RegularExpressions;

namespace Lizst.Controllers
{
    public class CheckInController : Controller
    {
        private readonly LizstContext _context;

        public CheckInController(LizstContext context)
        {
            _context = context;
        }

        // GET: CheckIn
        //Displays all the musicians that have at least one piece checked out.
        public IActionResult Index()
        {
            //Nested query to find any musician that has something checked out.
            IEnumerable<Musician> musicians = from m in _context.Musician
                                              where _context.CheckedOut.Any(e=>e.MusicianId == m.MusicianId)
                                              select m;

            return View(musicians);
        }

        public IActionResult Score()
        {
            //Nested query, select any score with at least on piece checked out.
            IEnumerable<Score> scores = from s in _context.Score
                                        where (from p in _context.Piece
                                              where _context.CheckedOut.Any(e => e.PartId == p.PieceId)
                                              select p).Any(e => e.ScoreId == s.ScoreId)
                                        select s;
            return View(scores);
        }

        public IActionResult ScoreCheck(int id)
        {
            Score s = _context.Score.Find(id);
            CheckInModel check = new CheckInModel(s.Title, id);

            //Select any piece from the desired score that is checked out.
            IEnumerable < Piece > pieces = from p in _context.Piece
                                           where _context.CheckedOut.Any
                                               ( e => e.PartId == p.PieceId) 
                                               && p.ScoreId == id
                                           select p;

            foreach(Piece p in pieces)
            {
                //Select any musician with a piece in the score currently checked out.
                IEnumerable<Musician> musicians = from m in _context.Musician
                                                  where _context.CheckedOut.Any(e => e.MusicianId == m.MusicianId && e.PartId == p.PieceId)
                                                  select m;
                foreach (Musician m in musicians)
                {
                    check.AddMusician(m);
                    check.AddPiece(p, m.MusicianId);
                }
            }

            return View(check);
        }

        public IActionResult CheckInScore(int id)
        {
            IEnumerable<String> keys = Request.Form.Keys;
            foreach(String key in keys)
            {
                Match m = Regex.Match(key, "Checkin [0-9][0-9]* [0-9][0-9]*");
                if (m.Success)
                {
                    String[] parts = key.Split(" ");
                    int pieceId = Convert.ToInt32(parts[1]);
                    int musicianId = Convert.ToInt32(parts[2]);

                    String condition = Request.Form["condition " + parts[1] + " " + parts[2]];
                    Return(pieceId, musicianId, condition);
                }
            }

            return RedirectToAction("ScoreCheck", new { id });
        }

        //GET: CheckIn/CheckIn/1
        //Displays all of the pieces that are checked out by a given musician,
        //and allows the user to select which pieces are being checked in and
        //in what condition they are in.
        public IActionResult CheckIn(int id)
        {
            Musician musician = _context.Musician.Find(id);
            IEnumerable<Piece> pieces = from p in _context.Piece
                                        where _context.CheckedOut.Any(e => e.PartId == p.PieceId && e.MusicianId == musician.MusicianId)
                                        select p;

            MusicianAndPieces mAndPs = new MusicianAndPieces()
            {
                Musician = musician,
                Pieces = pieces.ToArray(),
                Scores = new Score[pieces.Count()]
            };

            for(int i = 0; i < mAndPs.Scores.Length; i++)
            {
                Piece p = pieces.ElementAt(i);
                mAndPs.Scores[i] =  _context.Score.Find(p.ScoreId);
            }



            return View(mAndPs);
        }

        //POST: Checkin/Confirm
        //Does the logic for checking in a piece and updating the condition
        //it is in. Returns a confirmation page.
        public async Task<IActionResult> Confirm()
        {
            //Find the id of the musician whose piece is being checked in.
            int musicianId = Convert.ToInt32(Request.Form["musician"]);

            //Find every piece that is being checked in.
            List<int> toReturn = new List<int>();
            IEnumerable<String> keys = Request.Form.Keys;
            foreach(String k in keys)
            {
                Match m = Regex.Match(k, "Piece [0-9][0-9]*");
                if (m.Success)
                {
                    toReturn.Add(Convert.ToInt32(k.Substring(5)));
                    continue;
                }
            }

            //List<Piece> pieces = new List<Piece>();
            //List<String> condition = new List<string>();

            //For each piece being returned, assess the condition and update the rating.
            foreach (int i in toReturn)
            {
                String cond = Request.Form["condition " + i];
                Return(i, musicianId, cond);
            }
            await _context.SaveChangesAsync();
            return View();
        }

        public void Return(int pieceId, int musicianId, String cond)
        {
            Piece p = _context.Piece.Find(pieceId);
            CheckedOut co = _context.CheckedOut.Find(musicianId, pieceId);
            float rating;
            if (cond.Equals("Excellent"))
            {
                rating = 5;
            }
            else if (cond.Equals("Good"))
            {
                rating = 4;
            }
            else if (cond.Equals("Fair"))
            {
                rating = 3;
            }
            else if (cond.Equals("Poor"))
            {
                rating = 2;
            }
            else if (cond.Equals("Aweful"))
            {
                rating = 1;
            }
            else
            {
                _context.Remove(co);
                _context.SaveChanges();
                return;
            }

            //The new rating will be the weighted average of the old rating with the condition of the returned part.
            float newRating = (p.AggregateRating * (p.NumberofParts - 1) + rating) / p.NumberofParts;
            p.AggregateRating = newRating;
            _context.Update(p);
            _context.Remove(co);
            _context.SaveChanges();
        }
    }
}
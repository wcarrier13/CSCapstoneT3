using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lizst.Models;

namespace Lizst.Controllers
{
    public class CheckedOutController : Controller
    {
        public readonly LizstContext _context;

        public CheckedOutController(LizstContext context)
        {
            _context = context;
        }

        //Finds all musicians that have something checked out.
        public IActionResult Index()
        {
            IEnumerable<Musician> musicians = from m in _context.Musician
                                              where _context.CheckedOut.Any(e => e.MusicianId == m.MusicianId)
                                              select m;
            return View(musicians);
        }

        //Finds all pieces that are checked out by a given musician.
        public IActionResult Musician(int id)
        {
            //Find everything checked out by a given musician.
            CheckedOut[] checkedOut = (from co in _context.CheckedOut
                                       where co.MusicianId == id
                                       select co).ToArray();

            //Find all the piece information.
            Piece[] pieces = new Piece[checkedOut.Count()];
            for (int i = 0; i < checkedOut.Count(); i++)
            {
                pieces[i] = _context.Piece.Find(checkedOut[i].PartId);
            }

            //Find all of the score information.
            Score[] scores = new Score[pieces.Count()];
            for (int i = 0; i < pieces.Count(); i++)
            {
                scores[i] = _context.Score.Find(pieces[i].ScoreId);
            }

            //Create a new object storing all of the information.
            MusicianAndPieces mAndPs = new MusicianAndPieces { Musician = _context.Musician.Find(id), Pieces = pieces, Scores = scores };

            return View(mAndPs);
        }

        public IActionResult CheckedOutScores()
        {
            //Nested query, select any score with at least on piece checked out.
            IEnumerable<Score> scores = from s in _context.Score
                                        where (from p in _context.Piece
                                               where _context.CheckedOut.Any(e => e.PartId == p.PieceId)
                                               select p).Any(e => e.ScoreId == s.ScoreId)
                                        select s;
            return View(scores);
        }

        public IActionResult CheckedOutByScore(int id)
        {
            Score s = _context.Score.Find(id);
            CheckInModel check = new CheckInModel(s.Title, id);

            //Select any piece from the desired score that is checked out.
            IEnumerable<Piece> pieces = from p in _context.Piece
                                        where _context.CheckedOut.Any
                                            (e => e.PartId == p.PieceId)
                                            && p.ScoreId == id
                                        select p;

            foreach (Piece p in pieces)
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
    }
}
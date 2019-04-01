﻿using System;
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

        public IActionResult Index()
        {
            //Nested query to find any musician that has something checked out.
            IEnumerable<Musician> musicians = from m in _context.Musician
                                              where _context.CheckedOut.Any(e=>e.MusicianId == m.MusicianId)
                                              select m;

            return View(musicians);
        }

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

        public async Task<IActionResult> Confirm()

        {
            
            int musicianId = Convert.ToInt32(Request.Form["musician"]);
            List<int> toReturn = new List<int>();
            List<Piece> pieces = new List<Piece>();
            List<String> condition = new List<string>();
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

            //For each piece being returned, assess the condition and update the rating.
            foreach(int i in toReturn)
            {
                Piece p = _context.Piece.Find(i);
                String cond = Request.Form["condition " + i];
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
                    continue;
                }

                //The new rating will be the weighted average of the old rating with the condition of the returned part.
                float newRating = (p.AggregateRating * (p.NumberofParts - 1) + rating)/p.NumberofParts;
                p.AggregateRating = newRating;
                _context.Update(p);

                //Return all of the selected pieces.
                IEnumerable<CheckedOut> checkedOut = from co in _context.CheckedOut
                                                      where co.MusicianId == musicianId && co.PartId == i
                                                      select co;
                _context.CheckedOut.RemoveRange(checkedOut);
            }
            await _context.SaveChangesAsync();
            return View();
        }
    }
}
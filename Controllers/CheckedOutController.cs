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

        //REFACTOR
        public IActionResult Index()
        {
            IEnumerable<Musician> musicians = from m in _context.Musician
                                              where _context.CheckedOut.Any(e => e.MusicianId == m.MusicianId)
                                              select m;
            return View(musicians);
        }

        public IActionResult Musician(int id)
        {
            //Find everything checked out by a given musician.
            CheckedOut[] checkedOut = (from co in _context.CheckedOut
                                                 where co.MusicianId == id
                                                 select co).ToArray();

            //Find all the piece information.
            Piece[] pieces = new Piece[checkedOut.Count()];
            for(int i = 0; i < checkedOut.Count(); i++)
            {
                pieces[i] = _context.Piece.Find(checkedOut[i].PartId);
            }

            //Find all of the score information.
            Score[] scores = new Score[pieces.Count()];
            for(int i = 0; i < pieces.Count(); i++)
            {
                scores[i] = _context.Score.Find(pieces[i].ScoreId);
            }

            //Create a new object storing all of the information.
            MusicianAndPieces mAndPs = new MusicianAndPieces { Musician = _context.Musician.Find(id), Pieces = pieces, Scores = scores };

            return View(mAndPs);
        }
    }
}
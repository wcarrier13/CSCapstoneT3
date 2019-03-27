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
            List<MusicianAndPieces> msAndPs = new List<MusicianAndPieces>();
            IEnumerable<CheckedOut> checkedOut = _context.CheckedOut.ToList();
            foreach(CheckedOut co in checkedOut)
            {
                Musician musician = _context.Musician.Find(co.MusicianId);
                Piece piece = _context.Piece.Find(co.PartId);
                try
                {
                    MusicianAndPieces mAndPs = msAndPs.Find(m => m.Musician == musician);
                    List<Piece> pieces = mAndPs.Pieces.ToList();
                    pieces.Add(piece);
                    mAndPs.Pieces = pieces.ToArray();
                }
                catch
                {
                    MusicianAndPieces mAndPs = new MusicianAndPieces { Musician = musician, Pieces = new Piece[] { piece } };
                    msAndPs.Add(mAndPs);
                }
            }
            return View(msAndPs);
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
                //System.Diagnostics.Debug.WriteLine("\n\n" + scores[i].Title + "\n\n");
            }

            //Create a new object storing all of the information.
            MusicianAndPieces mAndPs = new MusicianAndPieces { Musician = _context.Musician.Find(id), Pieces = pieces, Scores = scores };

            return View(mAndPs);
        }
    }
}
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
    }
}
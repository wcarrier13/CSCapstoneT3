using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lizst.Models;

namespace Lizst.Controllers
{
    public class ScorePiecesController : Controller
    {

        private readonly LizstContext _context;

        public ScorePiecesController(LizstContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(new ScorePieces());
        }

        public async Task<IActionResult> AddTo(int p, int s)
        {
            ScorePieces piece = new ScorePieces { ScoreId = s, PieceId = p };
            await _context.ScorePieces.AddAsync(piece);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = s });
        }

        public IActionResult Submit()
        {
            List<String> test = new List<string>();
            //Finds every name for an input field. In the html we have <input ... name = "results[count]".../>
            IEnumerable<String> keys = Request.Form.Keys;

            //Print everything for verification.
            System.Diagnostics.Debug.WriteLine("\n\n");
            foreach(String k in keys)
            {
                //Key name (like results[1])
                System.Diagnostics.Debug.WriteLine(k);
                //Key value.
                System.Diagnostics.Debug.WriteLine(Request.Form[k]);
            }
            System.Diagnostics.Debug.WriteLine("\n\n");

            return RedirectToAction("Index", "Score");
        }
    }
}
using Lizst.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Submit()
        {
            List<String> test = new List<string>();
            //Finds every name for an input field. In the html we have <input ... name = "results[count]".../>
            IEnumerable<String> keys = Request.Form.Keys;

            //Print everything for verification.
            System.Diagnostics.Debug.WriteLine("\n\n");
            int count = 0;
            string numberOfParts = "";
            string edition = "";
            string rating;
            foreach(String k in keys)
            {
                if (count%3 == 0)
                {
                    
                    numberOfParts = Request.Form[k];
                }
                if (count%3 == 1)
                {
                    edition = Request.Form[k];
                }
                if (count%3 == 2)
                {
                    rating = Request.Form[k];
                    if (Convert.ToInt32(numberOfParts) > 0)
                    {
                        Piece piece = new Piece { Instrument = "Flute", NumberofParts = Convert.ToInt32(numberOfParts), Edition = edition };
                        await _context.Piece.AddAsync(piece);
                        await _context.SaveChangesAsync();
                    }
                }

                //Key name (like results[1])
                System.Diagnostics.Debug.WriteLine(k);
                //Key value.
                System.Diagnostics.Debug.WriteLine(Request.Form[k][0]);
                System.Diagnostics.Debug.WriteLine(Request.Form[k]);
                //System.Diagnostics.Debug.WriteLine(Request.Form[k][1]);
                //System.Diagnostics.Debug.WriteLine(Request.Form[k][2]);
                //if (pos == 0)
                //{
                    //Piece piece = new Piece { Instrument = "Flute", NumberofParts = Convert.ToInt32(Request.Form[k])};
                //}
                count++;
            }
            System.Diagnostics.Debug.WriteLine("\n\n");

               
     
            return RedirectToAction("Index", "Score");
        }
    }
}
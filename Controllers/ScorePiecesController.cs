using Lizst.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lizst.Controllers
{
    public class ScorePiecesController : Controller
    {

        public String[] names = { "Flute 1", "Flute 2", "Flute 3", "Flute 4"
            ,"Piccolo 1", "Piccolo 2" , "Clarinet 1", "Clarinet 2", "Clarinet 3" ,
             "Oboe 1", "Oboe 2", "Oboe 3" , "Horn 1", "Horn 2", "Horn 3", "Horn 4" ,
           "Trumpet 1", "Trumpet 2", "Trumpet 3" ,"Trombone 1", "Trombone 2", "Trombone 3" ,
           "Tuba 1", "Tuba 2","Other Brass", "Timpani 1", "Timpani 2", "Timpani 3", "Timpani 4" , "Timpani 5",
            "Snare Drum", "Tenor Drum", "Bass Drum", "Cymbals", "Triangle", "Tam-Tam", "Tambourine", "Wood Block", "Glockenspiel",
            "Xylophone", "Vibraphone", "Marimba", "Crotales", "Tubular Bells", "Mark Tree", "Drum Kit", "Other Percussion",
             "Piano", "Celesta", "Pipe Organ", "Harpsichord", "Accordion", "Claviharp", "Other Keyboard","Violin 1", "Violin 2", "Violin 3" ,"Harp", "Viola", "Cello", "Double Bass", "Other String"};

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
            string instrument = "";
            foreach (String k in keys)
            {
                if (count%3 == 0)
                {
                    if (count/3 != names.Length)
                    {
                        instrument = names[(count / 3)];
                    }
                    else
                    {
                        instrument = names[count/3 - 1];
                    }
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
                        Piece piece = new Piece { Instrument = instrument, NumberofParts = Convert.ToInt32(numberOfParts), Edition = edition };
                        await _context.Piece.AddAsync(piece);
                        await _context.SaveChangesAsync();
                    }
                }

                //Key name (like results[1])
                System.Diagnostics.Debug.WriteLine(k);
                //Key value.
                System.Diagnostics.Debug.WriteLine(Request.Form[k][0]);
                System.Diagnostics.Debug.WriteLine(Request.Form[k]);
             
                count++;
            }
            System.Diagnostics.Debug.WriteLine("\n\n");

               
     
            return RedirectToAction("Index", "Score");
        }
    }
}
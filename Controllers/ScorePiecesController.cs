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

        public IActionResult Index(string id)
        {
            System.Diagnostics.Debug.WriteLine("SCORE ID IN INDEX");
            System.Diagnostics.Debug.WriteLine(id);
            int sid = Convert.ToInt32(id);
            System.Diagnostics.Debug.WriteLine("SID IN INDEX");
            System.Diagnostics.Debug.WriteLine(sid);


            System.Diagnostics.Debug.WriteLine(sid);
            ScorePieces scorepiece = new ScorePieces() { ScoreId = sid };
            return View(scorepiece);
        }


        public async Task<IActionResult> Submit()
        {


            List<String> test = new List<string>();
            //Finds every name for an input field. In the html we have <input ... name = "results[count]".../>
            IEnumerable<String> keys = Request.Form.Keys;
            int id = Convert.ToInt32(Request.Form["sid"]);


            //Print everything for verification.
            System.Diagnostics.Debug.WriteLine("\n\n");
            int count = 0;
            string numberOfParts = "";
            string edition = "";
            string rating;
            string instrument = "";
            //int scoreid = Convert.ToInt32(sid);
            foreach (String k in keys)
            {
                if (count % 3 == 0)
                {
                    if (count / 3 != names.Length)
                    {
                        instrument = names[(count / 3)];
                    }
                    else
                    {
                        instrument = names[count / 3 - 1];
                    }
                    numberOfParts = Request.Form[k];
                    System.Diagnostics.Debug.WriteLine("numberOfParts from form");
                    System.Diagnostics.Debug.WriteLine(numberOfParts);
                    
                }
                if (count % 3 == 1)
                {
                    edition = Request.Form[k];
                }
                if (count % 3 == 2)
                {
                    rating = Request.Form[k];
                    System.Diagnostics.Debug.WriteLine("numberOfPartsAfterConverted");
                    System.Diagnostics.Debug.WriteLine(Convert.ToInt32(numberOfParts));
                    if (Convert.ToInt32(numberOfParts) > 0)
                    {
                        Piece piece = new Piece { Instrument = instrument, NumberofParts = Convert.ToInt32(numberOfParts), Edition = edition, ScoreId = id };
                        await _context.Piece.AddAsync(piece);
                        await _context.SaveChangesAsync();
                    }
                }

                //Key name (like results[1])
                System.Diagnostics.Debug.WriteLine(k);
                //Key value.

                System.Diagnostics.Debug.WriteLine(Request.Form[k]);


                count++;
            }
            System.Diagnostics.Debug.WriteLine("\n\n");

            System.Diagnostics.Debug.WriteLine("SID IN SUBMIT IS");
            System.Diagnostics.Debug.WriteLine(id);



            return RedirectToAction("Index", "Score");
        }
    }
}
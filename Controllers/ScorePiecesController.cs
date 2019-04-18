using Lizst.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lizst.Controllers
{
    public class ScorePiecesController : Controller
    {
       
        //List flattened array of instrument names
        //Will change this to the ScorePieces names if time
        public  String[] names = { "Flute 1", "Flute 2", "Flute 3", "Flute 4"
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

        //Creates new ScorePiece object to add pieces with the corresponding scoreId
        //Saves to the database
        //Add Model State checking
        public async Task<IActionResult> AddPieces(string id)
        {
            int sid = Convert.ToInt32(id);
            ScorePieces scorepiece = new ScorePieces() { ScoreId = sid};
            _context.ScorePieces.Add(scorepiece);
            await _context.SaveChangesAsync();
            return View(scorepiece);
        }

        //Creates a form to edit the pieces in a score given a scoreid
        public async Task<IActionResult> EditScorePiece(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Find scorepiece with corresponding scoreid
            var scorepiece = await _context.ScorePieces.FindAsync(id);

            if (scorepiece == null)
            {
                return NotFound();
            }

            //Build array of pieces that attach to the score
            IEnumerable<Piece> pieces = from p in _context.Piece where p.ScoreId == scorepiece.ScoreId select p;
            Piece[][] ps = new Piece[ScorePieces.Instruments.Length][];
            
            for (int i =0; i < ps.Length; i++)
            {
                ps[i] = new Piece[ScorePieces.names[i].Length];
                for (int j =0; j < ps[i].Length; j++)
                {
                    //Add any matching piece to the array, or null if it doesn't exist
                    ps[i][j] = pieces.FirstOrDefault(p => p.Instrument.Equals(ScorePieces.names[i][j]));
                }
            }
            return View(ps);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //Scorepiece is posted, update any changes to the record
        public async Task<IActionResult> EditScorePiece(int id, [Bind("ScoreId", "Instruments", "IndexedPieces", "names")] ScorePieces scorepiece)
        {
            if (ModelState.IsValid)
            {
                //Update the record.
                try
                {
                    scorepiece.ScoreId = id;
                    _context.Update(scorepiece);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScorePieceExists(scorepiece.ScoreId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View(scorepiece);
            }
            //Change view to the original score index screen
            return View("Index", "Score");
        }

        //Take all information in from form in a list of keys and submit to database
        //Add model state testing
        public async Task<IActionResult> Submit()
        {
            List<String> test = new List<string>();
            //Finds every name for an input field. In the html we have <input ... name = "results[count]".../>
            IEnumerable<String> keys = Request.Form.Keys;
            int id = Convert.ToInt32(Request.Form["sid"]);
            //ScorePieces spAdd = await _context.ScorePieces.FindAsync(id);
            //IDictionary<string, Piece> IndexedPieces = new Dictionary<string, Piece>();
            //ScorePieces.IndexedSPieces = new Dictionary<string, Piece>();
            //Print everything for verification.
            System.Diagnostics.Debug.WriteLine("\n\n");
            int count = 0;
            string numberOfParts = "";
            string edition = "";
            string rating;
            string instrument = "";
            //int scoreid = Convert.ToInt32(sid);
            var addToTotal = await _context.Score.FindAsync(id);

            //for every instrument inputted, grab the number of parts, edition, and rating
            //The keys come in in groups of 3, so modulus function is used to split
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
                }
                if (count % 3 == 1)
                {
                    edition = Request.Form[k];
                }
                if (count % 3 == 2)
                {
                    rating = Request.Form[k];
                    
                    
                    //If there is one or more parts per piece, create a new piece object
                    if (Convert.ToInt32(numberOfParts) > 0)
                    {
                        Piece piece = new Piece { Instrument = instrument, NumberofParts = Convert.ToInt32(numberOfParts), Edition = edition, ScoreId = id };

                        Score.Pieces.Add(piece);
                        //Add total number of parts per piece to the score
                        if (addToTotal.NumberOfParts != 0)
                        {
                            addToTotal.NumberOfParts += Convert.ToInt32(numberOfParts);
                        }
                        else
                        {
                            addToTotal.NumberOfParts = Convert.ToInt32(numberOfParts);
                        }
                        await _context.Piece.AddAsync(piece);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        //leaving this right now in case I change the add pieces implementation
                        //addToTotal.IndexedPieces.Add(instrument, null);
                    }
                }
   
                //Key name (like results[1])
                System.Diagnostics.Debug.WriteLine(k);
                //Key value.
                System.Diagnostics.Debug.WriteLine(Request.Form[k]);

                count++;
            }
            System.Diagnostics.Debug.WriteLine("\n\n");

            return RedirectToAction("Index", "Score");
        }

        //Check if the score exists in the database
        private bool ScorePieceExists(int id)
        {
            return _context.ScorePieces.Any(e => e.ScoreId == id);
        }


    }
}
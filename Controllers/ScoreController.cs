using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lizst.Models;

namespace Lizst.Controllers
{
    public class ScoreController : Controller
    {
        private readonly LizstContext _context;


        public ScoreController(LizstContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search, string genre)
        {
            IEnumerable<Score> scores;
            //No information passed, return all results.
            if (search == null || search.Equals(""))
            {
                scores =
                    from score in _context.Score
                    where true
                    select score;
            }
            //Perform the search.
            else
            {
                scores = Search.FindRelevant(search, _context);
            }

            //Limit search to genre.
            if (genre != null && !genre.Equals(""))
            {
                scores =
                    from score in scores
                    where score.Genre.Equals(genre)
                    select score;
            }
            SearchModel sm = new SearchModel { Search = search, Genre = genre, Results = scores };
            return View(sm);
        }

        // GET: Score/AddScore
        // Blank template for the addition of a new score.
        public IActionResult AddScore()
        {
            return View();
        }

        // POST: Score/AddScore
        // New score is posted, add it to database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddScore(Score score)
        {
            if (ModelState.IsValid)
            {
                _context.Score.Add(score);
                await _context.SaveChangesAsync();
                return RedirectToAction("AddPieces", "ScorePieces", new { id = score.ScoreId });
            }
            return View(score);
        }

        // GET: Score/EditScore
        // If score is in database, give edit page with relevant details.
        public async Task<IActionResult> EditScore(int? id, string search, string genre)
        {
            if (id == null)
            {
                return NotFound();
            }

            var score = await _context.Score.FindAsync(id);
            if (score == null)
            {
                return NotFound();
            }

            SearchModel sm = new SearchModel { Score = score, Search = search, Genre = genre };

            //return RedirectToAction("EditScorePiece", "ScorePieces");
            return View(sm);
        }

        // POST: Score/EditScore/6
        // Revised version of score has been posted. Update the database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditScore(int id, string search, string genre, string button, [Bind("ScoreId", "Title", "Composer", "Genre", "NumberOfParts", "Notes")] Score score)
        {
            if (ModelState.IsValid)
            {
                if (button != null)
                {
                    //Delete button was selected, attempt to delete the record.
                    if (button.Equals("Delete"))
                    {
                        var toDelete = await _context.Score.FindAsync(id);
                        foreach (Piece piece in _context.Piece)
                        {
                            if (piece.ScoreId == id)
                            {
                                var pieceDelete = await _context.Piece.FindAsync(piece.PieceId);
                                _context.Piece.Remove(pieceDelete);
                            }
                        }
                        _context.Score.Remove(toDelete);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }

                //Update the record.
                try
                {
                    score.ScoreId = id;
                    _context.Update(score);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScoreExists(score.ScoreId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("EditScorePiece", "ScorePieces", new { id = score.ScoreId });
            }      
            return RedirectToAction("EditScorePiece", "ScorePieces", new { id = score.ScoreId });
        }

        //Simply delete a score by id.
        public async Task<IActionResult> Delete(int id)
        {
            foreach (Piece piece in _context.Piece)
            {
                if (piece.ScoreId == id)
                {
                    var pieceDelete = await _context.Piece.FindAsync(piece.PieceId);
                    _context.Piece.Remove(pieceDelete);
                }
            }
            var toDelete = await _context.Score.FindAsync(id);
            _context.Remove(toDelete);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET: Score/Details
        //Returns a page displaying all the information about a score.
        public async Task<IActionResult> Details(int id, string search, string genre)
        {
            Score score = await _context.Score.FindAsync(id);
            
            if (score == null)
            {
                return NotFound();
            }
            SearchModel sm = new SearchModel { Score = score, Search = search, Genre = genre };
         
         
            IEnumerable<Piece> pieces = _context.Piece
        .Where(s => s.ScoreId == id).ToList();
           
                sm.sps = pieces;

            return View(sm);
        }


        //Takes an id, and returns true if any score has that id.
        private bool ScoreExists(int id)
        {
            return _context.Score.Any(e => e.ScoreId == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            if (search == null)
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
                search = search.ToLower();
            }

            //Limit search to genre.
            if (genre != null)
            {
                scores =
                    from score in scores
                    where score.Genre.Equals(genre)
                    select score;
            }

            return View(scores);
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
                return RedirectToAction(nameof(Index));
            }
            return View(score);
        }

        // GET: Score/EditScore
        // If score is in database, give edit page with relevant details.
        public async Task<IActionResult> EditScore(int? id)
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
            return View(score);
        }

        // POST: Score/EditScore/6
        // Revised version of score has been posted. Update the database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditScore(int id, string button, [Bind("ScoreId", "Title", "Composer", "Genre", "NumberOfParts", "Notes")] Score score)
        {
            if (ModelState.IsValid)
            {
                if (button != null)
                {
                    //Delete button was selected, attempt to delete the record.
                    if (button.Equals("Delete"))
                    {
                        var toDelete = await _context.Score.FindAsync(id);

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

                return RedirectToAction(nameof(Index));
            }

            return View(score);
        }

        //Simply delete a score by id.
        public async Task<IActionResult> Delete(int id)
        {
            var toDelete = await _context.Score.FindAsync(id);
            _context.Remove(toDelete);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET: Score/Details
        //Returns a page displaying all the information about a score.
        public async Task<IActionResult> Details(int id)
        {
            Score score = await _context.Score.FindAsync(id);
            if (score == null)
            {
                return NotFound();
            }
            return View(score);
        }


        //Takes an id, and returns true if any score has that id.
        private bool ScoreExists(int id)
        {
            return _context.Score.Any(e => e.ScoreId == id);
        }
    }
}

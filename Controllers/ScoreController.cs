﻿using System;
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

        // GET: Score
        // Index page merely displays all scores.
        public async Task<IActionResult> Index()
        {
            return View(await _context.Score.ToListAsync());
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

        // POST: Tests/Edit/5
        // Revised version of score has been posted. Update the database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditScore(int id, string button, [Bind("ScoreId", "Title", "Composer", "Genre", "NumberOfParts")] Score score)
        {
            if (button != null)
            {
                if (button.Equals("Delete"))
                {
                    //Just want to match ID, incase the fields change.
                    IEnumerable<Score> find =
                        from sco in _context.Score
                        where sco.ScoreId == score.ScoreId
                        select sco;
                    _context.Score.RemoveRange(find);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        private bool ScoreExists(int id)
        {
            return _context.Score.Any(e => e.ScoreId == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lizst.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace Lizst.Controllers
{
    public class PieceController : Controller
    {
        public readonly LizstContext _context;

        public PieceController(LizstContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Piece.ToListAsync());
        }

        // GET: Score/AddScore
        // Blank template for the addition of a new score.
        public IActionResult AddPiece()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPiece(Piece piece)
        {
            if (ModelState.IsValid)
            {
                _context.Piece.Add(piece);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(piece);
        }

        public async Task<IActionResult> EditPiece(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var piece = await _context.Piece.FindAsync(id);
            if (piece == null)
            {
                return NotFound();
            }
            return View(piece);
        }

        public async Task<IActionResult> AddToScore(int p, int s)
        {
            ScorePieces scorePiece = new ScorePieces { ScoreId = s, PieceId = p };
            await _context.ScorePieces.AddAsync(scorePiece);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = s });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPiece(int id, string button, [Bind("ScoreId", "PieceId", "NumberOfParts")] Piece piece)
        {
            if (ModelState.IsValid)
            {
                if (button != null)
                {
                    //Delete button was selected, attempt to delete the record.
                    if (button.Equals("Delete"))
                    {
                        var toDelete = await _context.Piece.FindAsync(id);

                        _context.Piece.Remove(toDelete);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }

                //Update the record.
                try
                {
                    piece.PieceId = id;
                    _context.Update(piece);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PieceExists(piece.PieceId))
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

            return View(piece);
        }

        //Simply delete a score by id.
        public async Task<IActionResult> Delete(int id)
        {
            var toDelete = await _context.Piece.FindAsync(id);
            _context.Remove(toDelete);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET: Score/Details
        //Returns a page displaying all the information about a score.
        public async Task<IActionResult> Details(int id)
        {
            Piece piece = await _context.Piece.FindAsync(id);
            if (piece == null)
            {
                return NotFound();
            }
            return View(piece);
        }


        private bool PieceExists(int id)
        {
            return _context.Piece.Any(e => e.PieceId == id);
        }


    }
}
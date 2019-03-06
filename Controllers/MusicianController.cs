using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lizst.Models;

namespace Lizst.Controllers
{
    public class MusicianController : Controller
    {
        private readonly LizstContext _context;

        public MusicianController(LizstContext context)
        {
            _context = context;
        }

        // GET: Musician
        //Index page merely displays all musicians.
        public async Task<IActionResult> Index()
        {
            return View(await _context.Musician.ToListAsync());
        }

        //GET: Musician/AddMusician
        // Blank template for the addition of a new score.
        public IActionResult AddMusician()
        {
            return View();
        }

        //POST: Musician/AddMusician
        //New musician is posted, add it to the database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMusician(Musician musician)
        {
            if (ModelState.IsValid)
            {
                _context.Musician.Add(musician);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(musician);
        }

        //GET: Musician/EditMusician
        //If musician is in database, return an edit page with the relevant details.
        public async Task<IActionResult> EditMusician(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var musician = await _context.Musician.FindAsync(id);
            if(musician == null)
            {
                return NotFound();
            }
            return View(musician);
        }

        //Post: Musician/EditMusician/
        // Revised version of the musician has be posted. Update the database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMusician(int id, string button, Musician musician)
        {
            if (ModelState.IsValid)
            {
                if(button != null)
                {
                    //Delete button was selected, attempt to delete the record.
                    if (button.Equals("Delete"))
                    {
                        var toDelete = await _context.Musician.FindAsync(id);
                        _context.Musician.Remove(toDelete);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }

                //Attempt to update the record.
                try
                {
                    musician.MusicianId = id;
                    _context.Update(musician);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicianExists(musician.MusicianId))
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

            return View(musician);
        }

        //Simply delete a musician by id.
        public async Task<IActionResult> Delete(int id)
        {
            var toDelete = await _context.Musician.FindAsync(id);
            _context.Remove(toDelete);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET: Musician/Details
        //Returns a page displaying all the information about a musician.
        public async Task<IActionResult> Details(int id)
        {
            Musician musician = await _context.Musician.FindAsync(id);
            if(musician == null)
            {
                return NotFound();
            }
            return View(musician);
        }

        //Takes an id and returns true if any musician has that id.
        private bool MusicianExists(int id)
        {
            return _context.Musician.Any(e => e.MusicianId == id);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lizst.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lizst.Controllers
{
    public class EnsembleController : Controller
    {
        public readonly LizstContext _context;

        public EnsembleController(LizstContext context)
        {
            _context = context;
        }



        // GET: Ensemble/
        // Merely display a list of all ensembles.
        public IActionResult Index()
        {
            return View(_context.Ensemble);
        }

        // GET: Ensemble/AddEnsemble
        // Returns an empty form to add a new ensemble.
        public IActionResult AddEnsemble()
        {
            return View();
        }

        // POST :Ensemble/AddEnsemble
        // Details of new ensemble are given, update the database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEnsemble(string add, Ensemble model)
        {
            if (ModelState.IsValid)
            {
                //User saving Ensemble to database.
                if (add.Equals("Save"))
                {
                    _context.Ensemble.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        // GET: Ensemble/EditEnsemble
        // If ensemble is in the database, return edit page with relevant details.
        public async Task<IActionResult> EditEnsemble(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var ensemble = await _context.Ensemble.FindAsync(id);
            if(ensemble == null)
            {
                return NotFound();
            }

            return View(ensemble);
        }

        // POST: Ensemble/EditEnsemble/5
        // Ensemble has been modified, update the database with the new information.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEnsemble(int id, string button, [Bind("EnsembleId","EnsembleName","Year","Conductor")] Ensemble ensemble )
        {
            if (button != null)
            {
                //Delete the ensemble from the database.
                if (button.Equals("Delete"))
                {
                    var toDelete = await _context.Ensemble.FindAsync(id);
                    _context.Remove(toDelete);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            //Update the ensemble
            if (ModelState.IsValid)
            {
                try
                {
                    ensemble.EnsembleId = id;
                    _context.Update(ensemble);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnsembleExists(ensemble.EnsembleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //Success
                return RedirectToAction(nameof(Index));
            }
            return View(ensemble);
        }

        private bool EnsembleExists(int id)
        {
            return _context.Ensemble.Any(e => e.EnsembleId == id);
        }
    }
}
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
        public IActionResult Index()
        {
            return View(_context.Ensemble);
        }

        // GET: Ensemble/AddEnsemble
        public IActionResult AddEnsemble()
        {
            return View();
        }

        // POST :Ensemble/AddEnsemble
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEnsemble(Ensemble ensemble)
        {
            if (ModelState.IsValid)
            {
                _context.Ensemble.Add(ensemble);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(ensemble);
        }

        // GET: Ensemble/EditEnsemble
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

        //POST: Ensemble/EditEnsemble/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEnsemble(string id, [Bind("EnsembleId","EnsembleName","Year","Conductor")] Ensemble ensemble )
        {
            if (id != null)
            {
                if (id.Equals("Delete"))
                {
                    IEnumerable<Ensemble> find =
                        from ens in _context.Ensemble
                        where ens.EnsembleId == ensemble.EnsembleId
                        select ens;
                    _context.Ensemble.RemoveRange(find);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ensemble);
                    await (_context.SaveChangesAsync());
                } catch (DbUpdateConcurrencyException)
                {
                    if (!EnsembleExists(ensemble.EnsembleId))
                    {
                        return NotFound();
                    } else
                    {
                        throw;
                    }
                }
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
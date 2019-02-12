using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lizst.Models;

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
        public IActionResult EditEnsemble()
        {
            return View();
        }
    }
}
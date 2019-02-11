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



        // GET: /<controller>/
        public IActionResult Index()
        {

            return View(_context.Ensemble);
        }
        public IActionResult AddEnsemble()
        {
            return View();
        }
        public IActionResult EditEnsemble()
        {
            return View();
        }
    }
}
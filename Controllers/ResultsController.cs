using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lizst.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lizst.Controllers
{
    public class ResultsController : Controller
    {
        private readonly LizstContext _context;

        public ResultsController(LizstContext context)
        {
            _context = context;
        }

        // GET: /Results/
        public IActionResult Index(string search)
        {

            IEnumerable<Score> scores;
            //No information passed, return all results.
            if (search == null)
            {
                scores =
                    from score in _context.Score
                    where true
                    select score;

                return View(scores);
            }

            search = search.ToLower();

            //Select relevant scores from the score database context.
            scores =
                from score in _context.Score
                where Search.Relevant(score, search)
                select score;

            return View(scores);
        }
    }
}

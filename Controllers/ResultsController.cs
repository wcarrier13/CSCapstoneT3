using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lizst.Models;

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
            else
            {
                scores = Search.FindRelevant(search, _context);
                search = search.ToLower();
            }

            if (genre != null)
            {
                scores =
                    from score in scores
                    where score.Genre.Equals(genre)
                    select score;
            }

            return View(scores);
        }
    }
}

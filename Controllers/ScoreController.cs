using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LizstMVC.Data;
using Microsoft.AspNetCore.Mvc;
using LizstMVC.Models;

namespace LizstMVC.Controllers
{
    public class ScoreController : Controller
    {
        private LibraryContext dbc = new LibraryContext();

        [HttpPost]
        public IActionResult Add(string title, string composer, string genre, DateTime dateCheckedOut, DateTime dueDate, int numberOfParts)
        {
            Score newScore = new Score
            {
                Title = title,
                Composer = composer,
                Genre = genre,
                DateCheckedOut = dateCheckedOut,
                DueDate = dueDate,
                NumberOfParts = numberOfParts
            };
            dbc.Score.Add(newScore);
            dbc.SaveChanges();

            return View();

        }
        // GET: Score
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Edit(int id)
        //{
            //return View();
       // }
       /*
        public ActionResult Delete(Score score)
        {
            var dbScore = dbc.Score.find(score.id);
            dbc.Scores.remove(dbScore);
            dbc.saveChanges();
            return View();
        }
*/
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static LizstMVC.Models.LibraryModel;

namespace LizstMVC.Controllers
{
    public class ScoreController : Controller
    {
        [HttpPost]
        public IActionResult Add(string title, string composer, string genre, DateTime dateCheckedOut, DateTime dueDate, int numberOfParts)
        {
            Score newScore = new Score
            {
                Title = title,
                Composer = composer,
                genre = genre,
                DateCheckedOut = dateCheckedOut,
                DueDate = dueDate,
                numberOfParts = numberOfParts
            };
            dbc.Score.Add(newScore);
            dbc.SaveChanges();

        }
        // GET: Score
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        public ActionResult Delete(Score score)
        {
            var dbScore = dbc.Scores.find(score.id);
            dbc.Scores.remove(dbScore);
            dbc.saveChanges();
            return View();
        }

    }
}
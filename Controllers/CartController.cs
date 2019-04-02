using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lizst.Models;
using System.Text.RegularExpressions;

namespace Lizst.Controllers
{
    public class CartController : Controller
    {
        LizstContext _context;
        ICollection<Score> ShoppingCart;

        public CartController(LizstContext context)
        {
            _context = context;
            ShoppingCart = Cart.GetCart();
        }

        // GET: Cart/
        // Displays all of the scores that are in the shopping cart.
        public IActionResult Index()
        {
            return View(ShoppingCart);
        }

        // GET: Cart/AddToCart/1
        // Adds a given score to the shopping cart and returns to the cart display.
        public IActionResult AddToCart(int id)
        {
            Score score = _context.Score.Find(id);
            if(score == null)
            {
                return NotFound();
            }
            if (!ShoppingCart.Any(s => s.ScoreId == id))
            {
                ShoppingCart.Add(score);
            }


            return RedirectToAction(nameof(Index));
        }

        // GET: Cart/RemoveFromCart
        // Removes a given score from the shopping cart and returns to the cart display.
        public IActionResult RemoveFromCart(int id)
        {
            Score score = ShoppingCart.Single(s => s.ScoreId == id);
            if(score == null)
            {
                return NotFound();
            }
            ShoppingCart.Remove(score);

            return RedirectToAction(nameof(Index));
        }

        // GET: Cart/SelectEnsemble
        // Displays all ensembles, and allows the user to select which one they wish to check the cart out to.
        public IActionResult SelectEnsemble()
        {
            IEnumerable<Ensemble> ensembles = _context.Ensemble.ToList();
            return View(ensembles);
        }

        // GET: Cart/Select
        //Given an ensemble, this matches every player with the parts they can play.
        public IActionResult Select(int id)
        {
            Ensemble ensemble = _context.Ensemble.Find(id);
            if(ensemble == null)
            {
                return NotFound();
            }

            CheckOutSelect outSelect = new CheckOutSelect();

            //Find all musicians that are in the selected ensemble.
            outSelect.AddAllMusicians(from musician in _context.Musician
                                      where _context.EnsemblePlayers.Any( e=>e.EnsembleId == id && e.MusicianId == musician.MusicianId)
                                      select musician);

            //Find all scores that are being checked out.
            outSelect.AddAllScores(Cart.ShoppingCart);

            //Find all pieces that are in some score being checked out.
            outSelect.AddAllPieces(from piece in _context.Piece
                                   where Cart.ShoppingCart.Any(e => e.ScoreId == piece.ScoreId)
                                   select piece);

            outSelect.MatchMusicians();
            
            return View(outSelect);
        }

        // GET: Cart/Confirm
        // Officially checks out the elements of the cart to the selected ensemble,
        // creating a new record of a piece being checked out.
        public async Task<IActionResult> Confirm()
        {
            IEnumerable<string> keys = Request.Form.Keys;
            foreach(String s in keys)
            {
                string regex = "musician [0-9][0-9]* score [0-9][0-9]*";
                Match m = Regex.Match(s, regex);
                if(m.Success)
                {
                    string[] split = s.Split(" ");
                    int musician = Convert.ToInt32(split[1]);
                    int pieceId = Convert.ToInt32( Request.Form[s]);
                    CheckedOut co = new CheckedOut { MusicianId = musician, PartId = pieceId };
                    _context.CheckedOut.Add(co);
                }
            }
            ShoppingCart.Clear();
            await _context.SaveChangesAsync();
            return View();
        }
    }
}
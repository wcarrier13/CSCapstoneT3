using Lizst.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            if (score == null)
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
            if (score == null)
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
            if (ensemble == null)
            {
                return NotFound();
            }

            CheckOutSelect outSelect = new CheckOutSelect();

            //Find all musicians that are in the selected ensemble.
            outSelect.AddAllMusicians(from musician in _context.Musician
                                      where _context.EnsemblePlayers.Any(e => e.EnsembleId == id && e.MusicianId == musician.MusicianId)
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
        public async Task<IActionResult> CheckOut()
        {
            IEnumerable<string> keys = Request.Form.Keys;
            List<CheckedOut> toCheckOut = new List<CheckedOut>();
            Dictionary<int, int> piecesAvailable = new Dictionary<int, int>();
            Dictionary<int, int> piecesToCheckOut = new Dictionary<int, int>();

            //Go through every musician we are checking out to, and add
            //any piece that we have selected for them to check out.
            //If availablie, check out the piece.
            foreach (String s in keys)
            {
                string regex = "musician [0-9][0-9]* score [0-9][0-9]*";
                Match m = Regex.Match(s, regex);
                //Found a musician, save their piece.
                if (m.Success)
                {
                    string[] split = s.Split(" ");
                    int musician = Convert.ToInt32(split[1]);
                    int pieceId = Convert.ToInt32(Request.Form[s]);

                    //As we find new pieces, add it to the list and calculate the
                    //number still available to check out. Then keep track of the
                    //number we need to perform the check out.
                    if (!piecesAvailable.ContainsKey(pieceId))
                    {
                        Piece p = _context.Piece.Find(pieceId);
                        int available = p.NumberofParts;
                        IEnumerable<CheckedOut> outs = from co in _context.CheckedOut
                                                       where co.PartId == pieceId
                                                       select co;
                        available -= outs.Count();
                        piecesAvailable.Add(pieceId, available);
                        piecesToCheckOut.Add(pieceId, 1);
                    }
                    else
                    {
                        piecesToCheckOut[pieceId]++;
                    }
                    //Store the record.
                    toCheckOut.Add(new CheckedOut { MusicianId = musician, PartId = pieceId });
                }
            }

            //Find any piece that we do not have enough of available to perform check out.
            IEnumerable<int> pieces = piecesToCheckOut.Keys;
            Cart.unavailable = new List<Piece>();
            foreach(int piece in pieces)
            {
                if (piecesToCheckOut[piece] > piecesAvailable[piece])
                {

                    Cart.unavailable.Add(_context.Piece.Find(piece));
                }
            }

            //If there are any pieces we cannot check out, redirect to an
            //error giving a few details.
            if (Cart.unavailable.Any())
            {
                return RedirectToAction("CheckOutError");
            }

            //Everything succeeded, officially perform the check out action.
            foreach(CheckedOut co in toCheckOut)
            {
                if(_context.CheckedOut.Any(e => e.MusicianId == co.MusicianId && e.PartId == co.PartId))
                {
                    continue;
                }
                _context.CheckedOut.Add(co);
            }

            //Empty the cart, save changes.
            ShoppingCart.Clear();
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Confirm));
        }

        public IActionResult CheckOutError()
        {
            //Find the score of any piece that did not have enough to be checked out.
            List<Score> scores = new List<Score>();
            foreach(Piece p in Cart.unavailable)
            {
                scores.Add(_context.Score.Find(p.ScoreId));
            }
            ScoresAndPieces SsAndPs = new ScoresAndPieces { Scores = scores, Pieces = Cart.unavailable };
            return View(SsAndPs);
        }

        //Check out succeeded, let the user know.
        public IActionResult Confirm()
        {
            return View();
        }
    }
}
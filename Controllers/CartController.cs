using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lizst.Models;

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


        public IActionResult Index()
        {
            return View(ShoppingCart);
        }

        public IActionResult AddToCart(int id)
        {
            Score score = _context.Score.Find(id);
            if(score == null)
            {
                return NotFound();
            }
            ShoppingCart.Add(score);


            return RedirectToAction(nameof(Index));
        }

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

        public IActionResult SelectEnsemble()
        {
            IEnumerable<Ensemble> ensembles = _context.Ensemble.ToList();
            return View(ensembles);
        }

        public IActionResult Select(int id)
        {
            Ensemble ensemble = _context.Ensemble.Find(id);
            if(ensemble == null)
            {
                return NotFound();
            }
            Cart.Ensemble = ensemble;
            return RedirectToAction(nameof(Checkout));
        }

        public async Task<IActionResult> Checkout()
        {
            //Nested query, select all musicians that are in the ensemble music is being checked out to.
            IEnumerable<Musician> musicians = from musician in _context.Musician
                                              where (from ensemblePlayer in _context.EnsemblePlayers
                                                     where ensemblePlayer.EnsembleId == Cart.Ensemble.EnsembleId
                                                     select ensemblePlayer).Any(e => e.MusicianId == musician.MusicianId)
                                              select musician;

            //Select any piece that is a part of some score in the cart.
            IEnumerable<Piece> pieces = from piece in _context.Piece
                                        where Cart.ShoppingCart.Any(e => e.ScoreId == piece.ScoreId)
                                        select piece;

            //Create records associating each musician with the relevant pieces that they can play.
            List<MusicianAndPieces> msAndPs = new List<MusicianAndPieces>();
            foreach(Musician musician in musicians)
            {
                IEnumerable<Piece> allowed = from piece in pieces
                                             where musician.Part != null && musician.Part.Equals(piece.Instrument)
                                             select piece;
                IEnumerable<Score> scores = from score in Cart.ShoppingCart
                                            where allowed.Any(e => e.ScoreId == score.ScoreId)
                                            select score;
                MusicianAndPieces mAndPs = new MusicianAndPieces {
                    Musician = musician,
                    Pieces = allowed.ToArray(),
                    Scores = scores.ToArray() };
                msAndPs.Add(mAndPs);
            }
            Cart.MusiciansAndPieces = msAndPs;

            return View(msAndPs);
        }

        public async Task<IActionResult> Confirm()
        {
            foreach(MusicianAndPieces mAndPs in Cart.MusiciansAndPieces)
            {
                foreach(Piece piece in mAndPs.Pieces)
                {
                    CheckedOut checkedOut = new CheckedOut { MusicianId = mAndPs.Musician.MusicianId, PartId = piece.PieceId };
                    _context.CheckedOut.Add(checkedOut);
                }
            }
            await _context.SaveChangesAsync();
            return View();
        }
    }
}
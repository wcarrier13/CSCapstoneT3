﻿using System;
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

        public IActionResult CheckOut()
        {
            return View();
        }
    }
}
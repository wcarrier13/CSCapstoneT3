using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lizst.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace Lizst.Controllers
{
    public class PieceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public readonly LizstContext _context;

        public PieceController(LizstContext context)
        {
            _context = context;
        }


        

    }
}
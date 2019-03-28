using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Lizst.Controllers
{
    public class CheckInController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
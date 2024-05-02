using Microsoft.AspNetCore.Mvc;
using RapidApiProject.Models;
using System.Diagnostics;

namespace RapidApiProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

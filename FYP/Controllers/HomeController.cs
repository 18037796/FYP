using Microsoft.AspNetCore.Mvc;
using FYP.Models;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace FYP.Controllers
{
    public class HomeController : Controller
    {
        
        
        public IActionResult Index()
        {
            return View(); 
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Statistics()
        {
            return View();
        }

        public IActionResult UserAlert()
        {
           
        return View();
            
        }


    }
}

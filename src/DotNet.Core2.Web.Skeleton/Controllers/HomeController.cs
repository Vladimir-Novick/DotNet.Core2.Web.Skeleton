using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNet.Core2.Web.Skeleton.Models;
using Microsoft.AspNetCore.Authorization;

namespace DotNet.Core2.Web.Skeleton.Controllers
{

       public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }




        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

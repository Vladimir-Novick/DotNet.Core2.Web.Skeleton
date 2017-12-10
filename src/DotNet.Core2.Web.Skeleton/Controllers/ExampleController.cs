using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;

using System.Linq;

using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Authorization;


namespace MQS.NetCore2.Server.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]

    public class ExampleController : Controller
    {
        public IActionResult List()
        {
            return View();
        }

    }
}

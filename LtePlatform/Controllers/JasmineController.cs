using System;
using System.Web.Mvc;

namespace LtePlatform.Controllers
{
    public class JasmineController : Controller
    {
        public ViewResult Run()
        {
            return View("SpecRunner");
        }
    }
}
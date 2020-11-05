using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutofacAspNetMvcDemos.Models;

namespace AutofacAspNetMvcDemos.Controllers
{
  public class HomeController : Controller
  {
    private readonly UrlHelper helper;

    public HomeController(UrlHelper helper)
    {
      this.helper = helper;
    }

    [LoggingActionFilter]
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult About()
    {
      ViewBag.Message = "Your application description page.";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }

    public void PostData([ModelBinder(typeof(PersonModelBinder))] Person person)
    {
      // bla bla
    }
  }
}
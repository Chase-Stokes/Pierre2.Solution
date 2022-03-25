using Microsoft.AspNetCore.Mvc;
using Bakery.Models;
using System.Linq;


namespace Bakery.Controllers
{
  public class HomeController : Controller
  {
    public readonly BakeryContext _db;
    public HomeController(BakeryContext db)
    {
      _db = db;
    }

    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }
    
    public ActionResult Contact()
    {
      return View();
    }

    public ActionResult Search(string Search)
    {
      var treats = _db.Treats.Where(t => (t.Name.Contains(Search) || (t.Name == Search))).ToList();
      ViewBag.Treats = treats;
      return View();
    }
  }
}
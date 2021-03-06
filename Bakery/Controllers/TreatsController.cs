using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Bakery.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;

namespace Bakery.Controllers
{
    [Authorize]
    public class TreatsController : Controller
  {
    private readonly BakeryContext _db;

    public TreatsController(BakeryContext db)
    {
      _db = db;
    }

    [AllowAnonymous]
    public ActionResult Index()
    {
      List<Treat> model = _db.Treats.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
        ViewBag.CategoryId = new SelectList(_db.Flavors, "FlavorId", "Description");
        return View();
    }

    [HttpPost]
  public ActionResult Create(Treat treat, int FlavorId)
  {
      // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      // var currentUser = await _userManager.FindByIdAsync(userId);
      // treat.User = currentUser;
      _db.Treats.Add(treat);
      _db.SaveChanges();
      if (FlavorId != 0)
      {
          _db.TreatFlavor.Add(new TreatFlavor() { FlavorId = FlavorId, TreatId = treat.TreatId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
  }

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
        var thisTreat = _db.Treats
        .Include(t => t.JoinEntities)
        .ThenInclude(join => join.Flavor)
        .FirstOrDefault(t => t.TreatId == id);
        return View(thisTreat);
    }

    public ActionResult Edit(int id)
    {
        var thisTreat = _db.Treats.FirstOrDefault(t => t.TreatId == id);
        ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Description");
        return View(thisTreat);
    }

    [HttpPost]
    public ActionResult Edit(Treat treat, int FlavorId)
    {
      if (FlavorId != 0)
      {
        _db.TreatFlavor.Add(new TreatFlavor() { FlavorId = FlavorId, TreatId = treat.TreatId });
      }
      _db.Entry(treat).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddFlavor(int id)
    {
        var thisTreat = _db.Treats.FirstOrDefault(t => t.TreatId == id);
        ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Description");
        return View(thisTreat);
    }

    [HttpPost]
    public ActionResult AddFlavor(Treat treat, int FlavorId)
    {
        if (FlavorId != 0)
        {
          _db.TreatFlavor.Add(new TreatFlavor() { FlavorId = FlavorId, TreatId = treat.TreatId });
          _db.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        var thisTreat = _db.Treats.FirstOrDefault(t => t.TreatId == id);
        return View(thisTreat);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        var thisTreat = _db.Treats.FirstOrDefault(t => t.TreatId == id);
        _db.Treats.Remove(thisTreat);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteFlavor(int joinId)
    {
        var joinEntry = _db.TreatFlavor.FirstOrDefault(e => e.TreatFlavorId == joinId);
        _db.TreatFlavor.Remove(joinEntry);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
  }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NetApp.Models;
using log4net;

namespace NetApp.Controllers
{
    [Authorize]
    public class TactsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private static ILog log = LogManager.GetLogger("ErrorLog");

        // GET: Tacts
        public ActionResult Index()
        {
            try
            {
                log.Debug("Tacts Index()");

                var tacts = db.Tacts.Include(t => t.Game);
                return View(tacts.ToList());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // GET: Tacts/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Tact tact = db.Tacts.Find(id);
        //    if (tact == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tact);
        //}

        // GET: Tacts/Create
        public ActionResult Create()
        {
            try
            {
                log.Debug("Tacts Create()");

                ViewBag.GameId = new SelectList(db.Games, "GameId", "Author");
                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // POST: Tacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TactId,TimeStart,TimeEnd,GameId")] Tact tact)
        {
            try
            {
                log.Debug("Tacts Create()[POST]");

                if (ModelState.IsValid)
                {
                    db.Tacts.Add(tact);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", tact.GameId);
                return View(tact);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // GET: Tacts/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Tact tact = db.Tacts.Find(id);
        //    if (tact == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", tact.GameId);
        //    return View(tact);
        //}

        // POST: Tacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "TactId,TimeStart,TimeEnd,GameId")] Tact tact)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(tact).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", tact.GameId);
        //    return View(tact);
        //}

        //// GET: Tacts/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Tact tact = db.Tacts.Find(id);
        //    if (tact == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tact);
        //}

        //// POST: Tacts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Tact tact = db.Tacts.Find(id);
        //    db.Tacts.Remove(tact);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

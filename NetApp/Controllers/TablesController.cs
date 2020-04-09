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
    public class TablesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private static ILog log = LogManager.GetLogger("ErrorLog");

        // GET: Tables
        public ActionResult Index()
        {
            try
            {
                log.Debug("Tables Index()");

                var tables = db.Tables.Include(t => t.Game);
                return View(tables.ToList());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // GET: Tables/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Table table = db.Tables.Find(id);
        //    if (table == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(table);
        //}

        // GET: Tables/Create
        //public ActionResult Create()
        //{
        //    ViewBag.GameId = new SelectList(db.Games, "GameId", "Author");
        //    return View();
        //}

        // POST: Tables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "TableId,TableNumber,GameId")] Table table)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Tables.Add(table);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", table.GameId);
        //    return View(table);
        //}

        // GET: Tables/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Table table = db.Tables.Find(id);
        //    if (table == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", table.GameId);
        //    return View(table);
        //}

        // POST: Tables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "TableId,TableNumber,GameId")] Table table)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(table).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", table.GameId);
        //    return View(table);
        //}

        // GET: Tables/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Table table = db.Tables.Find(id);
        //    if (table == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(table);
        //}

        // POST: Tables/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Table table = db.Tables.Find(id);
        //    db.Tables.Remove(table);
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

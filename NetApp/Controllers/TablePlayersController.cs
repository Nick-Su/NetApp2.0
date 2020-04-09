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
    public class TablePlayersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private static ILog log = LogManager.GetLogger("ErrorLog");

        // GET: TablePlayers
        //public ActionResult Index()
        //{
        //    var tablePlayers = db.TablePlayers.Include(t => t.Table);
        //    return View(tablePlayers.ToList());
        //}

        // GET: TablePlayers/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TablePlayer tablePlayer = db.TablePlayers.Find(id);
        //    if (tablePlayer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tablePlayer);
        //}

        // GET: TablePlayers/Create
        //public ActionResult Create()
        //{
        //    ViewBag.TableId = new SelectList(db.Tables, "TableId", "TableId");
        //    return View();
        //}

        // POST: TablePlayers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,TableId,ProjectParticipantId")] TablePlayer tablePlayer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.TablePlayers.Add(tablePlayer);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.TableId = new SelectList(db.Tables, "TableId", "TableId", tablePlayer.TableId);
        //    return View(tablePlayer);
        //}

        // GET: TablePlayers/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TablePlayer tablePlayer = db.TablePlayers.Find(id);
        //    if (tablePlayer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.TableId = new SelectList(db.Tables, "TableId", "TableId", tablePlayer.TableId);
        //    return View(tablePlayer);
        //}

        // POST: TablePlayers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,TableId,ProjectParticipantId")] TablePlayer tablePlayer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(tablePlayer).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.TableId = new SelectList(db.Tables, "TableId", "TableId", tablePlayer.TableId);
        //    return View(tablePlayer);
        //}

        // GET: TablePlayers/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TablePlayer tablePlayer = db.TablePlayers.Find(id);
        //    if (tablePlayer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tablePlayer);
        //}

        // POST: TablePlayers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    TablePlayer tablePlayer = db.TablePlayers.Find(id);
        //    db.TablePlayers.Remove(tablePlayer);
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

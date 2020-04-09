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
    public class ConnectionTypesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private static ILog log = LogManager.GetLogger("ErrorLog");

        // GET: ConnectionTypes
        public ActionResult Index(int gid, int gameCode)
        {
            try
            {
                log.Debug("ConnectionTypes Index()");

                var connectionTypes = db.ConnectionTypes.Where(x => x.GameId == gid).Include(c => c.Game);
                ViewBag.gameCode = gameCode;
                return View(connectionTypes.ToList());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        public JsonResult GetConnectionTypes(int gid)
        {
            try
            {
                log.Debug("ConnectionTypes GetConnectionTypes()");

                var connectionTypes = db.ConnectionTypes.Where(x => x.GameId == gid).Include(c => c.Game).ToList<ConnectionType>();

                //var conTypesList = connectionTypes;
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    connectionTypes
                });
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return Json("error", JsonRequestBehavior.AllowGet);

        }

        // GET: ConnectionTypes/Create
        public ActionResult Create()
        {
            ViewBag.GameId = new SelectList(db.Games, "GameId", "Author");
            return View();
        }

        // POST: ConnectionTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ConnectionTypeId,Decription,GameId")] ConnectionType connectionType, int gameCode)
        {
            try
            {
                log.Debug("ConnectionTypes Create()[POST]");

                var intGameCode = Convert.ToInt32(gameCode);
                if (ModelState.IsValid)
                {
                    db.ConnectionTypes.Add(connectionType);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { gid = connectionType.GameId, gameCode = intGameCode });
                }

                ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", connectionType.GameId);
                return View(connectionType);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");

        }

        // GET: ConnectionTypes/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                log.Debug("ConnectionTypes Details()");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ConnectionType connectionType = db.ConnectionTypes.Find(id);
                if (connectionType == null)
                {
                    return HttpNotFound();
                }
                return View(connectionType);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }



        // GET: ConnectionTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                log.Debug("ConnectionTypes Edit()");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ConnectionType connectionType = db.ConnectionTypes.Find(id);
                if (connectionType == null)
                {
                    return HttpNotFound();
                }
                ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", connectionType.GameId);

                return View(connectionType);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // POST: ConnectionTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ConnectionTypeId,Decription,GameId")] ConnectionType connectionType, int gid, int gameCode)
        {
            try
            {
                log.Debug("ConnectionTypes Edit()[POST]");

                if (ModelState.IsValid)
                {
                    db.Entry(connectionType).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { gid, gameCode});
                }
                ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", connectionType.GameId);
                return View(connectionType);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        //// GET: ConnectionTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                log.Debug("ConnectionTypes Delete()");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ConnectionType connectionType = db.ConnectionTypes.Find(id);
                if (connectionType == null)
                {
                    return HttpNotFound();
                }
                return View(connectionType);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        //// POST: ConnectionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int gid, int gameCode)
        {
            try
            {
                log.Debug("ConnectionTypes GetTactTimeEnd()");

                ConnectionType connectionType = db.ConnectionTypes.Find(id);
                db.ConnectionTypes.Remove(connectionType);
                db.SaveChanges();
                return RedirectToAction("Index", new { gid, gameCode });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

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

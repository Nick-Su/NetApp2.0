using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using NetApp.Models;
using log4net;

namespace NetApp.Controllers
{
    [Authorize]
    public class NeedTypesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private static ILog log = LogManager.GetLogger("ErrorLog");

        // GET: NeedTypes
        public ActionResult Index(int gid, int gameCode)
        {
            try
            {
                log.Debug("NeedTypes Index()");

                var needTypes = db.NeedTypes.Where(x => x.GameId == gid).Include(n => n.Game);
                ViewBag.gameCode = gameCode;
                return View(needTypes.ToList());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        public JsonResult GetNeedTypes(int gid)
        {
            try
            {
                log.Debug("NeedTypes GetNeedTypes()");

                var needTypes = db.NeedTypes.Where(x => x.GameId == gid).Include(c => c.Game).ToList<NeedType>();

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    needTypes
                });

                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        //GET: NeedTypes/Create
        public ActionResult Create()
        {
            // ViewBag.GameId = new SelectList(db.Games, "GameId", "Author");
            try
            {
                log.Debug("NeedTypes Create()");
                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // POST: NeedTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NeedTypeId,Description,GameId")] NeedType needType, int gameCode)
        {
            try
            {
                log.Debug("NeedTypes Details()[POST]");

                var intGameCode = Convert.ToInt32(gameCode);
                if (ModelState.IsValid)
                {
                    db.NeedTypes.Add(needType);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { gid = needType.GameId, gameCode = intGameCode });
                }

                ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", needType.GameId);
                return View(needType);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // GET: NeedTypes/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                log.Debug("NeedTypes Details()");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                NeedType needType = db.NeedTypes.Find(id);
                if (needType == null)
                {
                    return HttpNotFound();
                }
                return View(needType);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // GET: NeedTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                log.Debug("NeedTypes Edit()");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                NeedType needType = db.NeedTypes.Find(id);
                if (needType == null)
                {
                    return HttpNotFound();
                }
                ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", needType.GameId);
                return View(needType);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // POST: NeedTypes/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NeedTypeId,Description,GameId")] NeedType needType, int gid, int gameCode)
        {
            try
            {
                log.Debug("NeedTypes Edit()[POST]");

                if (ModelState.IsValid)
                {
                    db.Entry(needType).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { gid, gameCode});
                }
                ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", needType.GameId);

                return View(needType);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // GET: NeedTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                log.Debug("NeedTypes Delete()");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                NeedType needType = db.NeedTypes.Find(id);
                if (needType == null)
                {
                    return HttpNotFound();
                }
                return View(needType);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // POST: NeedTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int gid, int gameCode)
        {
            try
            {
                log.Debug("NeedTypes DeleteConfirmed()");
                NeedType needType = db.NeedTypes.Find(id);
                db.NeedTypes.Remove(needType);
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

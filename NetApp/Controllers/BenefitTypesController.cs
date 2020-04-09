using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using NetApp.Models;
using log4net;
using System;

namespace NetApp.Controllers
{
    [Authorize]
    public class BenefitTypesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private static ILog log = LogManager.GetLogger("ErrorLog");

        // GET: BenefitTypes
        public ActionResult Index(int gid, int gameCode)
        {
            try
            {
                log.Debug("ConnectionRequests Index()");

                var benefitTypes = db.BenefitTypes.Where(x => x.GameId == gid).Include(b => b.Game);
                ViewBag.gameCode = gameCode;
                return View(benefitTypes.ToList());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        public JsonResult GetBenefitTypes(int gid)
        {
            try
            {
                log.Debug("ConnectionRequests GetBenefitTypes()[JSON]");

                var benefitTypes = db.BenefitTypes.Where(x => x.GameId == gid).Include(c => c.Game).ToList<BenefitType>();
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    benefitTypes
                });
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return Json("error", JsonRequestBehavior.AllowGet);
        }

        // GET: BenefitTypes/Create
        public ActionResult Create()
        {
            try
            {
                log.Debug("ConnectionRequests Create()");
                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // POST: BenefitTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BenefitTypeId,Description,GameId")] BenefitType benefitType, int gameCode)
        {
            try
            {
                log.Debug("ConnectionRequests Create()[POST]");

                if (ModelState.IsValid)
                {
                    db.BenefitTypes.Add(benefitType);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { gid = benefitType.GameId, gameCode });
                }

                //ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", benefitType.GameId);
                return View(benefitType);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        // GET: BenefitTypes/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                log.Debug("ConnectionRequests Details()[POST]");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                BenefitType benefitType = db.BenefitTypes.Find(id);
                if (benefitType == null)
                {
                    return HttpNotFound();
                }
                return View(benefitType);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // GET: BenefitTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                log.Debug("ConnectionRequests Edit()");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                BenefitType benefitType = db.BenefitTypes.Find(id);
                if (benefitType == null)
                {
                    return HttpNotFound();
                }

                return View(benefitType);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // POST: BenefitTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BenefitTypeId,Description,GameId")] BenefitType benefitType, int gid, int gameCode)
        {
            try
            {
                log.Debug("ConnectionRequests Edit()[POST]");

                if (ModelState.IsValid)
                {
                    db.Entry(benefitType).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { gid, gameCode });
                }
                ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", benefitType.GameId);
                return View(benefitType);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // GET: BenefitTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                log.Debug("ConnectionRequests Delete()[POST]");
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                BenefitType benefitType = db.BenefitTypes.Find(id);
                if (benefitType == null)
                {
                    return HttpNotFound();
                }
                return View(benefitType);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // POST: BenefitTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int gid, int gameCode)
        {
            try
            {
                log.Debug("ConnectionRequests DeleteConfirmed()[POST]");
                BenefitType benefitType = db.BenefitTypes.Find(id);
                db.BenefitTypes.Remove(benefitType);
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NetApp.Models;
using Newtonsoft.Json;
using log4net;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace NetApp.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private static ILog log = LogManager.GetLogger("ErrorLog");

        // GET: Projects
        public ActionResult Index()
        {
            try
            {
                log.Debug("Project Index()");
                var uid = User.Identity.GetUserId();
                return View(db.Projects.Where(t => t.AuthorId == uid).ToList());
            } catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        public ActionResult CreateProject()
        {
            try
            {
                log.Debug("Projects CreateProject()");
                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        public JsonResult GetMyProjects()
        {
            try
            {
                log.Debug("Project GetMyProjects()");

                var uid = User.Identity.GetUserId();
                var userProjectsList = db.Projects.Where(x => x.AuthorId == uid).ToList<Project>();

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    userProjectsList
                });

                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                log.Debug("Project Details()");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Project project = db.Projects.Find(id);

                if (project == null)
                {
                    return HttpNotFound();
                }

                return View(project);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // GET: Projects/Details/5
        public ActionResult NodeDetails(int? id)
        {
            try
            {
                log.Debug("Project Node Details()");
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Project project = db.Projects.Find(id);
                if (project == null)
                {
                    return HttpNotFound();
                }
                return View(project);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            try
            {
                log.Debug("Project Create()");
                var context = new IdentityDbContext();
             
                var user =  HttpContext.GetOwinContext()
                            .GetUserManager<ApplicationUserManager>()
                            .FindById(User.Identity.GetUserId());

                var result = user.FirstName;
                result += " ";
                result += user.LastName;

                @ViewBag.username = result;
                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Whom,What,Feature,Author,AuthorId,Date, Contact")] Project project)
        {

            try
            {
                log.Debug("Project Create()[POST]");

                project.AuthorId = User.Identity.GetUserId();
                project.Date = DateTime.Now;

                if (ModelState.IsValid)
                {
                    db.Projects.Add(project);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(project);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return View("~/Views/Shared/Error.cshtml");
        }


        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                log.Debug("Project Edit()");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Project project = db.Projects.Find(id);
                if (project == null)
                {
                    return HttpNotFound();
                }
                return View(project);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Whom,What,Feature,Author,AuthorId,Date, Contact")] Project project)
        {
            try
            {
                log.Debug("Project Edit()[POST]");

                project.AuthorId = User.Identity.GetUserId();
                project.Date = DateTime.Now;

                if (ModelState.IsValid)
                {
                    db.Entry(project).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(project);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                log.Debug("Project Delete()");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Project project = db.Projects.Find(id);
                if (project == null)
                {
                    return HttpNotFound();
                }
                return View(project);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return View("~/Views/Shared/Error.cshtml");
}

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                log.Debug("Project Delete()[POST]");
                Project project = db.Projects.Find(id);
                db.Projects.Remove(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        public JsonResult FindProjectNameById(int pid)
        {
            try
            {
                log.Debug("Project FindProjectNameById()[JSON]");

                var record = db.Projects.Where(x => x.Id == pid).FirstOrDefault();
                var result = record != null ? record.Name : "Неизвестно";

                return Json(result.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserProjects()
        {
            try
            {
                log.Debug("Project GetUserProjects()[JSON]");

                var uid = User.Identity.GetUserId();
                var jsonprojects = db.Projects.Where(x => x.AuthorId == uid).ToList<Project>();

                // Serialize to JSON
                string json = JsonConvert.SerializeObject(new
                {
                    jsonprojects
                });

                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json("Error", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllProjects()
        {
            try
            {
                log.Debug("Project GetAllProjects()[JSON]");

                var allprojects = db.Projects.ToList<Project>();
                // Serialize to JSON
                string json = JsonConvert.SerializeObject(new
                {
                    allprojects
                });

                return Json(json, JsonRequestBehavior.AllowGet);

            } catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json("Error", JsonRequestBehavior.AllowGet);
        }

        // no need?
        //public string GetProjectNameById(int pid)
        //{
        //    try
        //    {
        //        log.Debug("Project GetProjectNameById()");
        //        var record = db.Projects.Where(x => x.Id == pid).FirstOrDefault();

        //        return record != null ? record.Name : "Неизвестно";
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //    }

        //    return "Неизвестно";
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

    public class HelperMethods
    {
        private static DatabaseContext db = new DatabaseContext();
        private static ILog log = LogManager.GetLogger("ErrorLog");
        public static string GetProjectNameById(int pid)
        {
            try
            {
                log.Debug("Project GetProjectNameById()");
                var record = db.Projects.Where(x => x.Id == pid).FirstOrDefault();
                return record != null ? record.Name : "Неизвестно";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return "Ошибка";
        }
    }

}

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
using Newtonsoft.Json.Linq;
using static NetApp.Controllers.HelperMethods;
using log4net;


namespace NetApp.Controllers
{
    [Authorize]
    public class ConnectionsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private static ILog log = LogManager.GetLogger("ErrorLog");

        // GET: Connections
        public ActionResult Index()
        {
            try
            {
                log.Debug("Connections index()");

                var connections = db.Connections.Include(c => c.Game);
                return View(connections.ToList());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

       
        // GET: Connections/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                log.Debug("Connections Details()");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Connection connection = db.Connections.Find(id);
                if (connection == null)
                {
                    return HttpNotFound();
                }
                return View(connection);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        
        public ActionResult Create([Bind(Include = "ConnectionId,SenderId,RecieverId,GameId")] Connection connection, int gid, int sid, int rid)
        {
            try
            {
                log.Debug("Connections Create()");

                if(gid == sid)
                {
                    return RedirectToAction("WhereAmI", "Games", new { gameId = gid });
                }

                connection.GameId = gid;
                connection.SenderId = sid;
                connection.RecieverId = rid;

                if (ModelState.IsValid)
                {
                    db.Connections.Add(connection);
                    db.SaveChanges();
                    //return RedirectToAction("Index", "ConnectionRequests");
                    return RedirectToAction("WhereAmI", "Games", new { gameId = gid });
                }

                ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", connection.GameId);
                return View(connection);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

     
        public ActionResult MyConnections()
        {
            try
            {
                log.Debug("Connections MyConnections()");

                var UserId = User.Identity.GetUserId();
                List<Project> allProjects = db.Projects.Where(c => c.AuthorId == UserId).ToList<Project>();
                var allCons = GetConnections();

                ViewBag.source = allCons;
                ViewBag.proj = allProjects;

                var uid = User.Identity.GetUserId();
                var userProjects = db.Projects.Where(x => x.AuthorId == uid).ToList<Project>();

                if (userProjects == null)
                {
                    return Json("Can't build graph. User projects can not be found", JsonRequestBehavior.AllowGet);
                }

                // Get Links From Connections Table
                var connectionList = new List<Connection>();

                foreach (var item in userProjects)
                {
                    var connPartList = db.Connections.Where(x => x.SenderId == item.Id).ToList();
                    connectionList = connectionList.Concat(connPartList).ToList<Connection>();
                }

                string json = JsonConvert.SerializeObject(new
                {
                    connectionList
                });

                // Create JSON
                JArray JsonConnectionRequests = new JArray();
                dynamic record = new JObject();
                foreach (var item in connectionList)
                {
                    record.ConnectionId = item.ConnectionId;
                    record.GameId = item.GameId;
                    record.SenderId = item.SenderId;
                    record.RecieverId = item.RecieverId;

                    record.SenderActiveProjectName = GetProjectNameById(item.SenderId);
                    record.RecieverActiveProjectName = GetProjectNameById(item.RecieverId);

                    JsonConnectionRequests.Add(record);
                    record = new JObject();
                }

                //using System.Text.Json;
                //using System.Text.Json.Serialization;
                // string json2 = JsonConnectionRequests.ToString() ;
                string json2 = JsonConvert.SerializeObject(JsonConnectionRequests);
                // string json2 = JsonSerializer.Serialize(JsonConnectionRequests);

                ViewBag.connections = json2;


                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        public JsonResult GetConnections()
        {
            try
            {
                log.Debug("Connections GetConnections()[JSON]");

                var uid = User.Identity.GetUserId();
                var jsondata = db.Connections.ToList<Connection>();

                return Json(jsondata, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return Json("error", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetConnectionFromAllGames()
        {
            //try
            //{
               // log.Debug("Connections GetConnectionFromAllGames()[JSON]");
                
             
                var uid = User.Identity.GetUserId();
                var userProjects = db.Projects.Where(x => x.AuthorId == uid).ToList<Project>();

                if(userProjects == null)
                {
                    return Json("Can't build graph. User projects can not be found", JsonRequestBehavior.AllowGet);
                }

      
                var connectionList = new List<Connection>();
          
                foreach (var item in userProjects)
                {
                    var connPartList = db.Connections.Where(x => x.SenderId == item.Id || x.RecieverId == item.Id).ToList();
                    connectionList = connectionList.Concat(connPartList).ToList<Connection>(); 
                }

                string json = JsonConvert.SerializeObject(new
                {
                    connectionList
                });

                JArray JsonConnectionRequests = new JArray();
                dynamic record = new JObject();
                foreach (var item in connectionList)
                {
                    record.ConnectionId = item.ConnectionId;
                    record.GameId = item.GameId;
                    record.SenderId = item.SenderId;
                    record.RecieverId = item.RecieverId;

                    record.SenderActiveProjectName = GetProjectNameById(item.SenderId);
                    record.RecieverActiveProjectName = GetProjectNameById(item.RecieverId);

                    JsonConnectionRequests.Add(record);
                    record = new JObject();
                }

                string json2 = JsonConnectionRequests.ToString();

                return Json(json2, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex.Message);
            //}
            //return Json("error", JsonRequestBehavior.AllowGet);
        }

        private List<Connection> FindTargetNode(int targetId)
        {
            try
            {
                log.Debug("Connections FindTargetNode()[JSON]");

                var temp = db.Connections.Where(x => x.SenderId == targetId).ToList();
                return temp;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return null;
        }

        public JsonResult GetConnectionFromCurrentGame(int gid)
        {
           
            var connectionList = db.Connections.Where(x => x.GameId == gid).ToList();

            JArray JsonConnectionRequests = new JArray();
            dynamic record = new JObject();
            foreach (var item in connectionList)
            {
                record.ConnectionId = item.ConnectionId;
                record.GameId = item.GameId;
                record.SenderId = item.SenderId;
                record.RecieverId = item.RecieverId;

                record.SenderActiveProjectName = GetProjectNameById(item.SenderId);
                record.RecieverActiveProjectName = GetProjectNameById(item.RecieverId);

                JsonConnectionRequests.Add(record);
                record = new JObject();
            }

            string result = JsonConnectionRequests.ToString();

            return Json(result, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex.Message);
            //}
            //return Json("error", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GameGraph()
        {
            return View();
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












// GET: Connections/Edit/5
//public ActionResult Edit(int? id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    Connection connection = db.Connections.Find(id);
//    if (connection == null)
//    {
//        return HttpNotFound();
//    }
//    ViewBag.GameId = new SelectList(db.Games1, "GameId", "Author", connection.GameId);
//    return View(connection);
//}

// POST: Connections/Edit/5
//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult Edit([Bind(Include = "ConnectionId,SenderId,RecieverId,GameId")] Connection connection)
//{
//    if (ModelState.IsValid)
//    {
//        db.Entry(connection).State = EntityState.Modified;
//        db.SaveChanges();
//        return RedirectToAction("Index");
//    }
//    ViewBag.GameId = new SelectList(db.Games1, "GameId", "Author", connection.GameId);
//    return View(connection);
//}

// GET: Connections/Delete/5
//public ActionResult Delete(int? id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    Connection connection = db.Connections.Find(id);
//    if (connection == null)
//    {
//        return HttpNotFound();
//    }
//    return View(connection);
//}

// POST: Connections/Delete/5
//[HttpPost, ActionName("Delete")]
//[ValidateAntiForgeryToken]
//public ActionResult DeleteConfirmed(int id)
//{
//    Connection connection = db.Connections.Find(id);
//    db.Connections.Remove(connection);
//    db.SaveChanges();
//    return RedirectToAction("Index");
//}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NetApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using log4net;

namespace NetApp.Controllers
{
    [Authorize]
    public class ConnectionRequestsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private static ILog log = LogManager.GetLogger("ErrorLog");

        // GET: ConnectionRequests
        public ActionResult Index()
        {
            // var connectionRequests = db.ConnectionRequests.Include(c => c.Game);
            // return View(connectionRequests.ToList());
            try
            {
                log.Debug("ConnectionRequests Index()");

                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        public JsonResult GetConnectionRequests()
        {
            try
            {
                log.Debug("ConnectionRequests GetConnectionRequests()[JSON]");

                var uid = User.Identity.GetUserId();
            var userProjects = db.Projects.Where(x => x.AuthorId == uid).ToList<Project>();
        
                var connectionRequestList = new List<ConnectionRequest>();
                foreach(var item in userProjects)
                {
                    var connectionRequests = db.ConnectionRequests.Where(x => x.SenderProjectId == item.Id).OrderByDescending(x => x.ConnectionRequestId).ToList<ConnectionRequest>();
                    connectionRequestList = connectionRequestList.Concat(connectionRequests).ToList<ConnectionRequest>();

                    connectionRequests = db.ConnectionRequests.Where(x => x.RecieverProjectId == item.Id).ToList<ConnectionRequest>();
                    connectionRequestList = connectionRequestList.Concat(connectionRequests).ToList<ConnectionRequest>();
                }

                connectionRequestList = connectionRequestList.OrderByDescending(x => x.ConnectionRequestId).ToList<ConnectionRequest>() ;

                // Serialize to JSON
                string json = JsonConvert.SerializeObject(new
                {
                    connectionRequestList
                });

                // Create JSON
                JArray JsonConnectionRequests = new JArray();
                dynamic record = new JObject();
                foreach (var item in connectionRequestList)
                {
                    record.ConnectionRequestId = item.ConnectionRequestId;
                    record.ConnectionType = item.ConnectionType;
                    record.SenderResourceRequest = item.SenderResourceRequest;
                    record.SenderGivenBenefit = item.SenderGivenBenefit;
                    record.SenderProjectId = item.SenderProjectId;
                    record.RecieverGetResource = item.RecieverGetResource;
                    record.RecieverGetBenefit = item.RecieverGetBenefit;
                    record.RecieverProjectId = item.RecieverProjectId;
                    record.IsApproved = item.IsApproved;
                    record.GameId = item.GameId;

                    record.SenderActiveProjectName = FindProjectNameById(item.SenderProjectId);
                    record.RecieverActiveProjectName = FindProjectNameById(item.RecieverProjectId);

                    JsonConnectionRequests.Add(record);
                    record = new JObject();
                }

                string json2 = JsonConnectionRequests.ToString();

                return Json(json2, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return Json("error", JsonRequestBehavior.AllowGet);
        }

        public string FindProjectNameById(int pid)
        {
            try
            {
                log.Debug("ConnectionRequests FindProjectNameById()");

                var record = db.Projects.Where(x => x.Id == pid).FirstOrDefault();
                return record != null ? record.Name : "Неизвестно";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return "ошибка";
        }

        // GET: ConnectionRequests/Create
        public ActionResult Create(int spid, int rpid, int gid)
        {
            try
            {
                log.Debug("ConnectionRequests Create()");

                int senderProjectId = Convert.ToInt32(spid);
                int recieverProjectId = Convert.ToInt32(rpid);
                int gameId = Convert.ToInt32(gid);

                // SelectList connectionTypes = new SelectList(db.ConnectionTypes.Where(x => x.GameId == gameId), "ConnectionTypeId", "Description");
                ViewBag.ConType = new SelectList(db.ConnectionTypes.Where(x => x.GameId == gameId), "Decription", "Decription");
                ViewBag.NeedTypeList = new SelectList(db.NeedTypes.Where(x => x.GameId == gameId), "Description", "Description");
                ViewBag.BenefitTypeList = new SelectList(db.BenefitTypes.Where(x => x.GameId == gameId), "Description", "Description");
                ViewBag.GameId = new SelectList(db.Games, "GameId", "Author");
                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // POST: ConnectionRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ConnectionRequestId,ConnectionType,SenderResourceRequest,SenderGivenBenefit,SenderProjectId,RecieverGetResource,RecieverGetBenefit,RecieverProjectId,IsApproved,GameId,SenderIsRead,RecieverIsRead")] ConnectionRequest connectionRequest)
        {
            try
            {
                log.Debug("ConnectionRequests Create()[POST]");

                connectionRequest.SenderIsRead = 1;
                connectionRequest.RecieverIsRead = 0;

                if (ModelState.IsValid)
                {
                    db.ConnectionRequests.Add(connectionRequest);
                    db.SaveChanges();
                    return RedirectToAction("SuccessSendRequest", "ConnectionRequests", new { gid = connectionRequest.GameId });
                    //return RedirectToAction("WhereAmI", "Games", new { gameId = connectionRequest.GameId});
                }

                ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", connectionRequest.GameId);
                return View(connectionRequest);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

           // return RedirectToAction("WhereAmI", "Games", new { gameId = connectionRequest.GameId });
            return View("~/Views/Shared/Error.cshtml");
        }

        public ActionResult SuccessSendRequest(int gid)
        {
            return View("~/Views/ConnectionRequests/Success.cshtml");
        }
        // GET: ConnectionRequests/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                log.Debug("ConnectionRequests Details()");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ConnectionRequest connectionRequest = db.ConnectionRequests.Find(id);
                if (connectionRequest == null)
                {
                    return HttpNotFound();
                }
                return View(connectionRequest);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // GET: ConnectionRequests/Edit/5
        // Used as Accept Request
        public ActionResult Edit(int? id, int gid)
        {
            try
            {
                log.Debug("ConnectionRequests Edit()");
                //ViewBag.NeedTypeList = new SelectList(db.NeedTypes.Where(x => x.GameId == gid), "Description", "Description");
                //ViewBag.BenefitTypeList = new SelectList(db.BenefitTypes.Where(x => x.GameId == gid), "Description", "Description");

                // SelectList connectionTypes = new SelectList(db.ConnectionTypes.Where(x => x.GameId == gameId), "ConnectionTypeId", "Description");
                ViewBag.ConType = new SelectList(db.ConnectionTypes.Where(x => x.GameId == gid), "Decription", "Decription");
                ViewBag.NeedTypeList = new SelectList(db.NeedTypes.Where(x => x.GameId == gid), "Description", "Description");
                ViewBag.BenefitTypeList = new SelectList(db.BenefitTypes.Where(x => x.GameId == gid), "Description", "Description");
                ViewBag.GameId = new SelectList(db.Games, "GameId", "Author");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ConnectionRequest connectionRequest = db.ConnectionRequests.Find(id);
                if (connectionRequest == null)
                {
                    return HttpNotFound();
                }
                ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", connectionRequest.GameId);

                return View(connectionRequest);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // POST: ConnectionRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ConnectionRequestId,ConnectionType,SenderResourceRequest,SenderGivenBenefit,SenderProjectId,RecieverGetResource,RecieverGetBenefit,RecieverProjectId,IsApproved,SenderIsRead,RecieverIsRead,GameId")] ConnectionRequest connectionRequest, int? after)
        {
            try
            {
                log.Debug("ConnectionRequests Edit()[POST]");

                //connectionRequest.SenderIsRead = 0;
                //connectionRequest.RecieverIsRead = 1;
               // connectionRequest.Game = db.Games.Find(connectionRequest.GameId);

                if (ModelState.IsValid)
                {
                    db.Entry(connectionRequest).State = EntityState.Modified;
                    db.SaveChanges();

                    // create record in Connections table
                    CreateConnection(connectionRequest.GameId, connectionRequest.SenderProjectId, connectionRequest.RecieverProjectId);

                    if (after != 0)
                    {
                        return RedirectToAction("Index", "ConnectionRequests", new { gid = connectionRequest.GameId, toLeaderBoard = 1, after });
                    }

                    return RedirectToAction("Index", "ConnectionRequests");
                }

                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach(var item in errors)
                {
                    var a = item;
                }

                ViewBag.ConType = new SelectList(db.ConnectionTypes.Where(x => x.GameId == connectionRequest.GameId), "Decription", "Decription");
                ViewBag.NeedTypeList = new SelectList(db.NeedTypes.Where(x => x.GameId == connectionRequest.GameId), "Description", "Description");
                ViewBag.BenefitTypeList = new SelectList(db.BenefitTypes.Where(x => x.GameId == connectionRequest.GameId), "Description", "Description");
                ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", connectionRequest.GameId);

                return View(connectionRequest);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        public bool CreateConnection(int gid, int sid, int rid)
        {
            try
            {
                log.Debug("ConnectionRequests CreateConnection()");

                Connection connection = new Connection
                {
                    GameId = gid,
                    SenderId = sid,
                    RecieverId = rid
                };

                if (ModelState.IsValid)
                {
                    db.Connections.Add(connection);
                    db.SaveChanges();
                    return true;
                }
            } catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

            return false;
        }

        [HttpGet]
        public ActionResult DeclineRequest(int? id, int gid)
        {
            try
            {
                log.Debug("ConnectionRequests DeclineRequest()");

                ViewBag.NeedTypeList = new SelectList(db.NeedTypes.Where(x => x.GameId == gid), "Description", "Description");
                ViewBag.BenefitTypeList = new SelectList(db.BenefitTypes.Where(x => x.GameId == gid), "Description", "Description");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ConnectionRequest connectionRequest = db.ConnectionRequests.Find(id);
                if (connectionRequest == null)
                {
                    return HttpNotFound();
                }
                //ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", connectionRequest.GameId);
                return View(connectionRequest);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeclineRequest([Bind(Include = "ConnectionRequestId,ConnectionType,SenderResourceRequest,SenderGivenBenefit,SenderProjectId,RecieverGetResource,RecieverGetBenefit,RecieverProjectId,IsApproved,GameId")] ConnectionRequest connectionRequest, int? after)
        {
            try
            {
                log.Debug("ConnectionRequests DeclineRequest()[POST]");

                if (ModelState.IsValid)
                {
                    db.Entry(connectionRequest).State = EntityState.Modified;
                    db.SaveChanges();

                    if (after != null)
                    {
                        return RedirectToAction("Index", "ConnectionRequests", new { gid = connectionRequest.GameId, toLeaderBoard = 1, after });
                    }
                    return RedirectToAction("Index");
                }
                //ViewBag.GameId = new SelectList(db.Games, "GameId", "Author", connectionRequest.GameId);
                return View(connectionRequest);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        //// GET: ConnectionRequests/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ConnectionRequest connectionRequest = db.ConnectionRequests.Find(id);
        //    if (connectionRequest == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(connectionRequest);
        //}

        //// POST: ConnectionRequests/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    ConnectionRequest connectionRequest = db.ConnectionRequests.Find(id);
        //    db.ConnectionRequests.Remove(connectionRequest);
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

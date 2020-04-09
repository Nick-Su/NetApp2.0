using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using NetApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using log4net;

namespace NetApp.Controllers
{
    [Authorize]
    public class UserRatingsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private static ILog log = LogManager.GetLogger("ErrorLog");

        // GET: UserRatings
        public ActionResult Index()
        {
            try
            {
                log.Debug("UserRatings Index()");
                return View(db.UserRatings.ToList());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        public JsonResult CalculateUserRating(int gid)
        {
            try
            {
                log.Debug("UserRatings CalculateUserRating()[JSON]");
                // 1. Get user projects
                string uid = User.Identity.GetUserId();
                var userProjectsList = db.Projects.Where(x => x.AuthorId == uid).ToList();
            
                var aprovedRequestsList = new List<ConnectionRequest>();
                var aprovedRequestListForGame = new List<ConnectionRequest>();
                // Списки для расчета общей статистики по запросам
                var senderRequestsList = new List<ConnectionRequest>();
                var recieverRequestsList = new List<ConnectionRequest>();

                // 2.Ищем в тбл ConnectionRequests пересечение userProjectId == SenderProjectId
                foreach (var up in userProjectsList)
                {
                    // Для расчета рейтинга
                    var gameRatingRequests = db.ConnectionRequests.Where(z => z.GameId == gid).Where(x => x.SenderProjectId == up.Id && x.IsApproved == 1).ToList<ConnectionRequest>();
                    var aprovedRequest = db.ConnectionRequests.Where(x => x.SenderProjectId == up.Id && x.IsApproved == 1).ToList<ConnectionRequest>();

                    if (aprovedRequest.Count != 0)
                    {
                        aprovedRequestsList = aprovedRequestsList.Concat(aprovedRequest).ToList<ConnectionRequest>();
                    
                    }

                    if(gameRatingRequests.Count != 0)
                    {
                        aprovedRequestListForGame = aprovedRequestListForGame.Concat(gameRatingRequests).ToList<ConnectionRequest>();
                    }

                    // Для расчета общей статистики по запросам принято\отправлено
                    var sendedRequests = db.ConnectionRequests.Where(z => z.GameId == gid).Where(x => x.SenderProjectId == up.Id).ToList<ConnectionRequest>();
                    var recievedRequests = db.ConnectionRequests.Where(z =>z.GameId == gid).Where(x =>x.RecieverProjectId == up.Id).ToList<ConnectionRequest>();

                    if(sendedRequests.Count != 0)
                    {
                        senderRequestsList = senderRequestsList.Concat(sendedRequests).ToList<ConnectionRequest>();
                    }

                    if(recievedRequests.Count != 0)
                    {
                        recieverRequestsList = recieverRequestsList.Concat(recievedRequests).ToList<ConnectionRequest>();
                    }
                }

                if (aprovedRequestsList.Count == 0)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }

                // Рассчитываем, сколько баллов начислить
                int recalculatedRatingForAllGames = CalculateRatings(aprovedRequestsList);
                int ratingEarnedWhileGame = CalculateRatings(aprovedRequestListForGame);

                // Рассчитываем статистику
                var sendedRequestsListStat = CalculateCommonRequestStats(senderRequestsList);
                var recieverRequestListStat = CalculateCommonRequestStats(recieverRequestsList);

                // Пишем рейтинг в БД
            
                UserRating userRating = db.UserRatings.SingleOrDefault(x => x.UserId == uid);

                if (userRating == null)
                {
                    userRating = new UserRating
                    {
                        Rating = recalculatedRatingForAllGames,
                        UserId = uid
                       
                    };

                    if (ModelState.IsValid)
                    {
                        db.UserRatings.Add(userRating);
                        db.SaveChanges();
                    }
                } else
                {
                    userRating.Rating = recalculatedRatingForAllGames;
                    if (ModelState.IsValid)
                    {
                        db.Entry(userRating).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                   
                }
                            
                // To json
                JArray JsonRatings = new JArray();
                dynamic record = new JObject();
                record.oldRating = recalculatedRatingForAllGames;
                record.ratingAfterGame = ratingEarnedWhileGame;

                if(sendedRequestsListStat.Count != 0)
                {
                    record.totalRequestSend = sendedRequestsListStat[0];
                    record.sendTotalWaitAproval = sendedRequestsListStat[1];
                    record.sendTotalAproved = sendedRequestsListStat[2];
                    record.sendTotalDeclined = sendedRequestsListStat[3];
                }

                if (recieverRequestListStat.Count != 0)
                {
                    record.totalRequestRecieved = recieverRequestListStat[0];
                    record.recievedTotalWaitAproval = recieverRequestListStat[1];
                    record.recievedTotalAproved = recieverRequestListStat[2];
                    record.recievedTotalDeclined = recieverRequestListStat[3];
                }

                JsonRatings.Add(record);

                string json = JsonRatings.ToString();

                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        private List<int> CalculateCommonRequestStats(List<ConnectionRequest> list)
        {
            try
            {
                log.Debug("UserRatings CalculateCommonRequestStats()");

                int totalRequestSend = list.Count;
                int sendTotalWaitAproval = 0;
                int sendTotalAproved = 0;
                int sendTotalDeclined = 0;

                var statList = new List<int>();

                foreach (var item in list)
                {
                    if (item.IsApproved == 0)
                    {
                        sendTotalWaitAproval++;
                    }
                    else if (item.IsApproved == 1)
                    {
                        sendTotalAproved++;
                    }
                    else if (item.IsApproved == 2)
                    {
                        sendTotalDeclined++;
                    }
                }

                statList.Add(totalRequestSend);
                statList.Add(sendTotalWaitAproval);
                statList.Add(sendTotalAproved);
                statList.Add(sendTotalDeclined);

                return statList;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return null;
        }

        private int CalculateRatings(List<ConnectionRequest> list)
        {
            try
            {
                log.Debug("UserRatings CalculateRatings()");

                int rating = 0;
                foreach (var item in list)
                {
                    if (item.SenderGivenBenefit != null && item.SenderResourceRequest != null)
                    {
                        rating += 2;
                    }
                    else
                    {
                        rating++;
                    }
                }
                return rating;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return 0;
        }

        public JsonResult GetLeaderBoard(int gid)
        {
            try
            {
                log.Debug("UserRatings GetLeaderBoard()[JSON]");

                // Получить список id проектов игроков игры
                var projectGame = db.Games.Where(x => x.GameId == gid).Include(z => z.Projects).FirstOrDefault();

                JArray JsonLeaderBoard = new JArray();
            
                foreach (var pg in projectGame.Projects)
                {
                    dynamic record = new JObject();
                    var aprovedRequestListForGame = new List<ConnectionRequest>();

                    var gameRatingRequests = db.ConnectionRequests.Where(z => z.GameId == gid).Where(x => x.SenderProjectId == pg.Id && x.IsApproved == 1).ToList<ConnectionRequest>();

                    if (gameRatingRequests.Count != 0)
                    {
                        aprovedRequestListForGame = aprovedRequestListForGame.Concat(gameRatingRequests).ToList<ConnectionRequest>();
                    }

                    int ratingEarnedWhileGame = CalculateRatings(aprovedRequestListForGame);
               
                    // Ищем автора проекта
                    var user = HttpContext.GetOwinContext()
                                    .GetUserManager<ApplicationUserManager>()
                                    .FindById(pg.AuthorId);

                    var myId = User.Identity.GetUserId();

             
                    record.name = user.FirstName;
                    record.lastName = user.LastName;
                    record.rating = ratingEarnedWhileGame;
                    record.isItMe = myId == user.Id ? 1 : 0;

                    JsonLeaderBoard.Add(record);
                }

            

                string json = JsonLeaderBoard.ToString();
                JArray array = JArray.Parse(json);
                JArray sorted = new JArray(array.OrderByDescending(obj => (int)obj["rating"]));


                return Json(sorted.ToString(Formatting.Indented), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json("error", JsonRequestBehavior.AllowGet);

        }

        //// GET: UserRatings/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: UserRatings/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "UserRatingId,Rating,AuthorId")] UserRating userRating)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.UserRatings.Add(userRating);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(userRating);
        //}
        
        // GET: UserRatings/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserRating userRating = db.UserRatings.Find(id);
        //    if (userRating == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userRating);
        //}

        // POST: UserRatings/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "UserRatingId,Rating,AuthorId")] UserRating userRating)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(userRating).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(userRating);
        //}

        // GET: UserRatings/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserRating userRating = db.UserRatings.Find(id);
        //    if (userRating == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userRating);
        //}

        // GET: UserRatings/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserRating userRating = db.UserRatings.Find(id);
        //    if (userRating == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userRating);
        //}

        // POST: UserRatings/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    UserRating userRating = db.UserRatings.Find(id);
        //    db.UserRatings.Remove(userRating);
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

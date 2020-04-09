using Microsoft.AspNet.Identity;
using NetApp.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace NetApp.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private static ILog log = LogManager.GetLogger("ErrorLog");
        public JsonResult GetNotifications(string isseen)
        {
            try
            {
               // log.Debug("Notifications GetNotifications()");

                string uid = User.Identity.GetUserId();
                var userProjects = db.Projects.Where(x => x.AuthorId == uid).ToList();
                var readedOutcomingNotifications = new List<ConnectionRequest>();
                var readedIncomingNotifications = new List<ConnectionRequest>();
                if (isseen == "seen")
                {
                    foreach (var up in userProjects)
                    {
                        var outcomingRequestsNotification = db.ConnectionRequests.Where(x => x.SenderProjectId == up.Id & x.SenderIsRead == 0).ToList();
                        if (outcomingRequestsNotification.Count != 0)
                        {
                            readedOutcomingNotifications = readedOutcomingNotifications.Concat(outcomingRequestsNotification).ToList<ConnectionRequest>();
                        }

                        var incomingRequests = db.ConnectionRequests.Where(x => x.RecieverProjectId == up.Id & x.RecieverIsRead == 0).ToList();
                        if (incomingRequests.Count != 0)
                        {
                            readedIncomingNotifications = readedIncomingNotifications.Concat(incomingRequests).ToList<ConnectionRequest>();
                        }
                    }

                    foreach (var item in readedOutcomingNotifications)
                    {
                        MarkNotificationsAsRead(item.ConnectionRequestId);
                    }

                    foreach (var item in readedIncomingNotifications)
                    {
                        MarkIncomingNotificationAsRead(item.ConnectionRequestId);
                    }
                }
                var incomingRequestsList = new List<ConnectionRequest>();
                var outcomingRequestsList = new List<ConnectionRequest>();
                foreach (var up in userProjects)
                {
                    var incomingRequests = db.ConnectionRequests.Where(x => x.RecieverProjectId == up.Id & x.RecieverIsRead == 0).ToList();
                    if (incomingRequests.Count != 0)
                    {
                        incomingRequestsList = incomingRequestsList.Concat(incomingRequests).ToList<ConnectionRequest>();
                    }
                    var outcomingRequests = db.ConnectionRequests.Where(x => x.SenderProjectId == up.Id & x.SenderIsRead == 0).ToList();
                    if (outcomingRequests.Count != 0)
                    {
                        outcomingRequestsList = outcomingRequestsList.Concat(outcomingRequests).ToList<ConnectionRequest>();
                    }
                }

                JArray JsonNotifications = new JArray();

                foreach (var item in incomingRequestsList)
                {
                    dynamic record = new JObject();
                    record.type = "incomingRequest";
                    record.title = "Входящий запрос";
                    record.senderProject = FindProjectNameById(item.SenderProjectId);
                    record.recieverProject = FindProjectNameById(item.RecieverProjectId);
                    JsonNotifications.Add(record);
                }

                foreach (var item in outcomingRequestsList)
                {
                    var result = "";
                    if (item.IsApproved == 1)
                    {
                        result = "принят";
                    }
                    else if (item.IsApproved == 2)
                    {
                        result = "отклонен";
                    }
                    dynamic record = new JObject();
                    record.type = "outcomingRequest";
                    record.title = "Исходящий запрос " + result;
                    record.senderProject = FindProjectNameById(item.SenderProjectId);
                    record.recieverProject = FindProjectNameById(item.RecieverProjectId);
                    JsonNotifications.Add(record);
                }

                string json = JsonNotifications.ToString();

                return Json(json, JsonRequestBehavior.AllowGet);
            } catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        private void MarkNotificationsAsRead(int rid)
        {
            try
            {
                log.Debug("Notifications MarkNotificationsAsRead()");

                var connectionRequest = db.ConnectionRequests.Find(rid);
                connectionRequest.SenderIsRead = 1;
                if (ModelState.IsValid)
                {
                    db.Entry(connectionRequest).State = EntityState.Modified;
                    db.SaveChanges();
                }
            } catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void MarkIncomingNotificationAsRead(int rid)
        {
            try
            {
                log.Debug("Notifications MarkIncomingNotificationAsRead()");

                var connectionRequest = db.ConnectionRequests.Find(rid);
                connectionRequest.RecieverIsRead = 1;
                if (ModelState.IsValid)
                {
                    db.Entry(connectionRequest).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        public string FindProjectNameById(int pid)
        {
            try
            {
                log.Debug("Notifications FindProjectNameById()");

                var record = db.Projects.Where(x => x.Id == pid).FirstOrDefault();
                return record != null ? record.Name : "Неизвестно";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return null;

        }
    }
}
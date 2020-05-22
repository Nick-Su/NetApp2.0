using Microsoft.AspNet.Identity;
using NetApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using log4net;
using NetApp.Hubs;

using System.Web.Script.Serialization;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NetApp.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private static ILog log = LogManager.GetLogger("ErrorLog");
        
        public ActionResult Test()
        {
            
            return View();
        }

        // GET: Games
        public ActionResult Index()
        {
            try
            {
                log.Debug("Games Index()");

                return View(db.Games.ToList());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        // GET: Games/Create
        public ActionResult Create()
        {
            try
            {
                log.Debug("Games Create()");
                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Author,NumPlayers,NumTables,NumTacts,TactDuration,CreationTime,Code, CurrentTact,IsOnlyGivenConTypes,IsOnlyGivenNeeds,IsOnlyGivenBenefits, TransitionGroup, PlayerIntroductionTime, CurrentStage, TimeToAproveRequests")] Game game)
        {
            try
            {
                log.Debug("Games Create()[POST]");
                game.Author = User.Identity.GetUserId();
                game.CreationTime = DateTime.Now;
                var gameCode = GenerateRandomNo();
                game.Code = gameCode;
                game.Status = 0;
                game.CurrentTact = 0;
                game.NumPlayers = 0;

                if (ModelState.IsValid)
                {
                    db.Games.Add(game);
                    db.SaveChanges();

                    CreatePredefinedConnectionTypes(game.GameId);
                    CreatePredefinedNeedTypes(game.GameId);
                    CreatePredefinedBenefitTypes(game.GameId);

                    return RedirectToAction("AdminZone", "Games", new { gameCode, gid = game.GameId });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View(game);
        }

        [HttpGet]
        public ActionResult AdminZone(int gameCode, int gid)
        {
            try
            {
                log.Debug("Games AdminZone()");
                ViewBag.gameCode = gameCode;
                ViewBag.gameId = gid;

                return View("AdminZone");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        private List<Game> FindGame(int gid)
        {
            try
            {
                log.Debug("Games FindGame()");
                return db.Games.Where(p => p.GameId == gid).Include(u => u.Table).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return null;
        }

        private List<Table> GetGameTables(List<Game> game)
        {
            try
            {
                log.Debug("Games GetGameTables()");

                var tableList = new List<Table>();
                foreach (var item in game)
                {
                    foreach (var table in item.Table)
                    {
                        tableList.Add(table);
                    }
                }

                return tableList;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return null;
        }

        private bool AssignParticipantToTable(int tableId, Game participants)
        {
            try
            {
                log.Debug("Games AssignParticipantToTable()");

                var tablePlayer = new TablePlayer();
                var participantsList = participants.Projects.ToList();
         
                for (var i = 0; i < participantsList.Count(); i++) {

                    tablePlayer.TableId = tableId;
                    tablePlayer.ProjectParticipantId = participantsList[i].Id;

                    var isExist = db.TablePlayers.Where(t => t.TableId == tablePlayer.TableId).Where(y => y.ProjectParticipantId == tablePlayer.ProjectParticipantId).ToList();
                    if (isExist.Count() == 0)
                    {
                        if (ModelState.IsValid)
                        {
                            db.TablePlayers.Add(tablePlayer);
                            db.SaveChanges();
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return false;
        }
 
        private bool CreateTables(int gid, int tableNum)
        {
            try
            {
                log.Debug("Games CreateTables()");

                var isExist = db.Tables.Where(t => t.GameId == gid).ToList();
                if (isExist.Count() > 0 )
                {
                    return true;
                }

                Table table = new Table();
                for (var i = 1; i <= tableNum; i++)
                {
                    table.GameId = gid;
                    table.TableNumber = i;
                    
                    if (ModelState.IsValid)
                    {
                        db.Tables.Add(table);
                        db.SaveChanges();
                    } else
                        return false; 
                }
           
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return false;
        }

        private bool CreatePredefinedConnectionTypes(int gid)
        {
            try
            {
                log.Debug("Games CreatePredefinedConnectionTypes()");

                List<string> list = new List<string> { "Обмен ресурсами", "Обмен информацией", "Обмен капиталом", "Обмен базами клиентов" };

                foreach (var item in list)
                {
                    var connectionType = new ConnectionType
                    {
                        GameId = gid,
                        Decription = item
                    };

                    if (ModelState.IsValid)
                    {
                        db.ConnectionTypes.Add(connectionType);
                        db.SaveChanges();
                    } else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return false;
        }

        private bool CreatePredefinedNeedTypes(int gid)
        {
            try
            {
                log.Debug("Games CreatePredefinedNeedTypes()");

                List<string> list = new List<string> { "Трудовые ресурсы", "Оборудование", "Сырье", "Информация", "Финансовые ресурсы" };

                foreach (var item in list)
                {
                    var needType = new NeedType
                    {
                        GameId = gid,
                        Description = item
                    };

                    if (ModelState.IsValid)
                    {
                        db.NeedTypes.Add(needType);
                        db.SaveChanges();
                    } else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return false;
        }

        private bool CreatePredefinedBenefitTypes(int gid)
        {
            try
            {
                log.Debug("Games CreatePredefinedBenefitTypes()");

                List<string> list = new List<string> { "Получение ресурса", "Партнерство", "Получение информации" };
                foreach (var item in list)
                {
                    var benefitType = new BenefitType
                    {
                        GameId = gid,
                        Description = item
                    };

                    if (ModelState.IsValid)
                    {
                        db.BenefitTypes.Add(benefitType);
                        db.SaveChanges();
                    } else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return false;
        }

        //[HttpPost]
        private bool ChangeStatus([Bind(Include = "Id,AuthorId,NumPlayers,NumTables,NumTacts,TactDuration,CreationTime,Code, Status, CurrentTact, CurrentStage, TimeToAproveRequests")] Game game, int statusNum)
        {
            try
            {
                log.Debug("Games ChangeStatus()");

                game.Status = statusNum;
                if (ModelState.IsValid)
                {
                    db.Entry(game).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return false;
        }

        private bool SetCurrentTact([Bind(Include = "Id,AuthorId,NumPlayers,NumTables,NumTacts,TactDuration,CreationTime,Code, Status, CurrentTact, CurrentStage, TimeToAproveRequests")] Game game, int TactId)
        {
            try
            {
                log.Debug("Games SetCurrentTact()");

                game.CurrentTact = TactId;
                if (ModelState.IsValid)
                {
                    db.Entry(game).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
            } catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            return false;
        }

        public JsonResult GetConnectedPlayers(int gid)
        {
            try
            {
                log.Debug("Games GetConnectedPlayers()");

                var projectList = db.Games.Where(c => c.GameId == gid).Include(t => t.Projects).ToList();
                var players = 0;

                foreach(var item in projectList)
                {
                    players = item.Projects.Count();
                }

                return Json(players, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json("Error", JsonRequestBehavior.AllowGet);
        }

        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public JsonResult ShufflePlayersByClient(int gid)
        {
            ShufflePlayers(gid);
            return Json("shuffled", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ConnectToGame()
        {
            try
            {
                log.Debug("Games ConnectToGame()");
                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConnectToGame(int? gameCode)
        {
            try
            {
                log.Debug("Games ConnectToGame()");

                Game game = db.Games.FirstOrDefault(p => p.Code == gameCode);
                if (game == null)
                {
                    return View("~/Views/Games/GameNotFound.cshtml");
                }

                if (game.Status == 2)
                {
                    return View("~/Views/Games/GameHasBeenEnded.cshtml");
                }

                return RedirectToAction("ChooseProject", "Games", new { gameCode });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        public ActionResult ChooseProject(int? gameCode)
        {
            try
            {
                log.Debug("Games ChooseProject()");

                var userId = User.Identity.GetUserId();
                var projects = db.Projects.Where(m => m.AuthorId == userId).ToList<Project>();

                if (projects.Count == 0)
                {
                    return View("~/Views/Games/ProjectNotFound.cshtml");
                }

                ViewBag.projects = projects;
                ViewBag.userId = userId;
                ViewBag.gameCode = gameCode;

                return View("~/Views/Games/ChooseProject.cshtml");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddParticipant([Bind(Include = "Id,AuthorId,NumPlayers,NumTables,NumTacts,TactDuration,CreationTime,Code, Status, CurrentTact, CurrentStage, TimeToAproveRequests")] Game game, string assignedProject, int gameCode)
        {
            try
            {
                log.Debug("Games AddParticipant()");

                var gcode = Convert.ToInt32(gameCode);
                Game gameEntity = db.Games.FirstOrDefault(p => p.Code == gcode);

                var gameStatus = gameEntity.Status;
                var gid = gameEntity.GameId;
                var projectId = Convert.ToInt32(assignedProject);

                var ifExist = db.Games.Where(t => t.GameId == gid).Include(t => t.Projects).ToList();
                var project = new List<Project>();
                foreach(var item in ifExist)
                {
                    foreach(var elem in item.Projects)
                    {
                        if (elem.Id == projectId)
                        {
                            project.Add(elem);
                        }
                    }
                }

                Game gameInstance = db.Games.FirstOrDefault(s => s.GameId == gid);
                Project projectInstance = db.Projects.FirstOrDefault(s => s.Id == projectId);
                if (project.Count() == 0)
                {
                    gameInstance.Projects.Add(projectInstance);
                    db.SaveChanges();
                }

                if (gameStatus == 0)
                {
                    return RedirectToAction("WaitRoom", new { gid });
                }
                else if (gameStatus == 1)
                {
                    var tableList = db.Tables.Where(x => x.GameId == gid).ToList();
                    int minplayers = 100000;
                    int tid = 0;
                    foreach (var item in tableList)
                    {
                        var tableplayers = db.TablePlayers.Where(z => z.TableId == item.TableId).ToList().Count();
                        if(minplayers >= tableplayers)
                        {
                            minplayers = tableplayers;
                            tid = item.TableId;
                        }
                    }
                    TablePlayer tablePlayer = new TablePlayer
                    {
                        TableId = tid,
                        ProjectParticipantId = projectInstance.Id
                    };
                    var isExist = db.TablePlayers.Where(t => t.TableId == tablePlayer.TableId).Where(y => y.ProjectParticipantId == tablePlayer.ProjectParticipantId).ToList();
                    if (isExist.Count() == 0)
                    {
                        db.TablePlayers.Add(tablePlayer);
                        db.SaveChanges();
                    }
                    return RedirectToAction("WhereAmI", "Games", new { gid });
                }
                else if (gameStatus == 2)
                {
                    return View("~/Views/Games/GameHasBeenEnded.cshtml");
                }

                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        public ActionResult WaitRoom(int gid)
        {
            try
            {
                log.Debug("Games WaitRoom()");

                return View("~/Views/Games/WaitRoom.cshtml");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        public ActionResult Introduction(int gid)
        {
            try
            {
                log.Debug("Games Introduction()");
                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        [HttpGet]
        public ActionResult GetGameStatus(int gameId)
        {
            try
            {
                log.Debug("Games GetGameStatus()");
           
                Game game = db.Games.FirstOrDefault(r => r.GameId == gameId);
                var status = game.Status;
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error,cshtml");
        }

        private int GetPlayerProjectId(int gid)
        {
            try
            {

                var uid = User.Identity.GetUserId();
                var uprojects = db.Projects.Where(u => u.AuthorId == uid).ToList<Project>();
                var game = db.Games.Find(gid);
                var projectParticipants = game.Projects;
                int result = 0; 
                foreach (var item in projectParticipants)
                {
                    foreach (var up in uprojects)
                    {
                        if (item.Id == up.Id)
                        {
                            result = item.Id;
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return 0;
        }

        public JsonResult GetPlayerActiveProject(int gid)
        {
            try
            {
                var uid = User.Identity.GetUserId();
                var uprojects = db.Projects.Where(u => u.AuthorId == uid).ToList<Project>();
                var game = FindGame(gid);
                var projectParticipants = game[0].Projects;

                var result = new List<Project>();
                foreach (var item in projectParticipants)
                {
                    foreach (var up in uprojects)
                    {
                        if (item.Id == up.Id)
                        {
                            result.Add(up);
                        }
                    }
                }

                string json = JsonConvert.SerializeObject(new
                {
                    result
                });

                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ShowCurrentTact(int gid)
        {
            var game = db.Games.Find(gid);
            var currentTactId = game.CurrentTact;

            var tact = db.Tacts.Where(c => c.TactId == currentTactId).First();
            int tnum = tact.TactNumber;
            return Json(tnum, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ShowTotalTactNum(int gid)
        {
            Game game = db.Games.Find(gid);
            int tactNum = game.NumTacts;
            return Json(tactNum, JsonRequestBehavior.AllowGet);

        }

        public ActionResult WhereAmI(int gid)
        {
            try
            {
                log.Debug("Games WhereAmI()");
                int currentStage = GetCurrentStage(gid);

                switch(currentStage)
                {
                    case 1:
                        return RedirectToAction("WaitUntilRedirectTable", "Games", new { gid });
                    case 2:
                        return RedirectToAction("Introduction", "Games", new { gid });
                    case 3:
                        var uid = User.Identity.GetUserId();
                        var uprojects = db.Projects.Where(u => u.AuthorId == uid).ToList<Project>();
                        var game = FindGame(gid);
                        var tables = game[0].Table;
                        var projects = game[0].Projects.ToList();

                        List<Project> participant = new List<Project>();
                        foreach (var gp in projects)
                        {
                            foreach (var up in uprojects)
                            {
                                var a = projects.Where(x => x.Id == up.Id).FirstOrDefault();
                                if (a != null)
                                {
                                    participant.Add(a);
                                }
                            }
                        }

                        int tid = participant[0].Id;
                        var tabplayer = db.TablePlayers.Where(x => x.ProjectParticipantId == tid).OrderByDescending(x => x.TableId).First();
                        int tableId = 0;
                        int tableNumber = 0;
                        tableId = tabplayer.TableId;
                        var tempNum = db.Tables.Where(x => x.TableId == tableId).ToList();
                        tableNumber = tempNum[0].TableNumber;
                        var tactId = game[0].CurrentTact;
                        return RedirectToAction("ShowPlayRoom", "Games", new { gid, tid = tableId, tnum = tableNumber });
                    case 4:
                        return RedirectToAction("Index", "ConnectionRequests", new { gid });
                    case 5:
                        return RedirectToAction("EndGame", "Games", new { gid });
                    case 0:
                        return RedirectToAction("WaitRoom", new {gid });
                    default:
                        return View("~/Views/Shared/Error.cshtml");
                }  
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        public int GetCurrentStage(int gid)
        {
            Game game = db.Games.Find(gid);
            return game.CurrentStage;
        }

        public ActionResult ShowPlayRoom(int tid, int gid, int tnum)
        {
            try
            {
                log.Debug("Games ShowPlayRoom()");

                return View("~/Views/Games/PlayRoom.cshtml");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        public JsonResult GetTablePlayerList(int tableId, int gameId)
        {
            try
            {
                log.Debug("Games GetTablePlayerList()[JSON]");
                var upid = GetPlayerProjectId(gameId);
                var tablePlayerEntity = db.TablePlayers.Where(x => x.ProjectParticipantId == upid).OrderByDescending(x => x.Id).First();
                var tid = tablePlayerEntity.TableId;
                var tablePlayers = db.TablePlayers.Where(q => q.TableId == tableId).Include(t => t.Table);
                var rawProjects = new List<Project>();
                foreach (Game t in db.Games.Where(y => y.GameId == gameId).Include(u => u.Projects))
                {
                    foreach (Project pr in t.Projects)
                    {
                        rawProjects.Add(pr);
                    }
                }

                JArray finalProjects = new JArray();
                dynamic record = new JObject();

                foreach (var t in db.Tables.Where(q => q.GameId == gameId).Include(w => w.TablePlayer).Where(w => w.TableId == tid))
                {
                    foreach (var y in t.TablePlayer)
                    {
                        foreach (var item in rawProjects)
                        {
                            if (y.ProjectParticipantId == item.Id)
                            {
                                if (item.Id != upid)
                                {
                                    record.Author = item.Author;
                                    record.AuthorId = item.AuthorId;
                                    record.Contact = item.Contact;
                                    record.Date = item.Date;
                                    record.Feature = item.Feature;
                                    record.Id = item.Id;
                                    record.Name = item.Name;
                                    record.What = item.What;
                                    record.Whom = item.Whom;

                                    var checkReq = db.ConnectionRequests.Where(
                                        v => v.RecieverProjectId == item.Id
                                        && v.SenderProjectId == upid
                                        && v.IsApproved == 0
                                        ).ToList();

                                    record.IsRequestExist = checkReq.Count() == 0 ? 0 : 1;
                                    finalProjects.Add(record);
                                    record = new JObject();
                                }
                            }
                        }
                    }
                }
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    finalProjects
                });

                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        public ActionResult WaitUntilRedirectTable(int gid)
        {
            try
            {
                log.Debug("Games WaitUntilRedirectTable()");

                Game game = db.Games.Find(gid);
                var groupTransfer = game.TransitionGroup;
                ViewBag.groupTransfer = groupTransfer;

                return RedirectToAction("RedirectToWaitPage", "Games", new { gid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }

        public ActionResult RedirectToWaitPage()
        {
            try
            {
                log.Debug("Games RedirectToWaitPage()");

                return View("~/Views/Games/WaitUntilRedirectTable.cshtml");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        public ActionResult TimeToAproveRequests(int gid)
        {
            try
            {
                log.Debug("Games TimeToAproveRequests()");

                ViewBag.gid = gid;
                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        public JsonResult BackToGame()
        {
            try
            {
                log.Debug("Games BackToGame()[JSON]");
                
                var uid = User.Identity.GetUserId();
                var userProjects = db.Projects.Where(x => x.AuthorId == uid).ToList();
                int targetGameId = 0;
                foreach(var userProject in userProjects)
                {
                    var games = db.Games.Include(x => x.Projects).ToList();
                    foreach(var game in games)
                    {
                        foreach (var x in game.Projects)
                        {
                           if(x.Id == userProject.Id && game.Status == 1)
                           {
                                targetGameId = game.GameId;
                           }
                        }
                    }
                }

                return Json(targetGameId, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return Json("error", JsonRequestBehavior.AllowGet);
        }

        public JsonResult BackToAdmin()
        {
            try
            {
                log.Debug("Games BackToGame()[JSON]");

                var uid = User.Identity.GetUserId();
                var game = db.Games.Where(x => x.Author == uid && x.Status == 1).FirstOrDefault();

                if (game == null) {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                
                JArray gameInfo = new JArray();
                dynamic record = new JObject();
                record.gameCode = game.Code;
                record.gid = game.GameId;
                gameInfo.Add(record);

                string json = gameInfo.ToString();

                return Json(json, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json("Error", JsonRequestBehavior.AllowGet);

        }

        public void ShufflePlayers(int gameId)
        {
            try
            {
                log.Debug("Games ShufflePlayers()");
                var game = FindGame(gameId);
                List<Project> participants = game[0].Projects.ToList();
                participants.Shuffle();
                var numTables = Convert.ToDecimal(game[0].NumTables);
                var numPlayersPerTable = Convert.ToInt32(Math.Round((participants.Count() / numTables), 2));
                var tables = game[0].Table.ToList();
                for (var i = 0; i < tables.Count(); i++)
                {
                    DeletePlayerShuffle(tables[i].TableId);
                }

                var start = 0;
                var end = numPlayersPerTable--;
                var pointer = tables.Count();
                pointer = tables[pointer - 1].TableId;

                for (var i = 0; i < tables.Count() - 1; i++)
                {
                    var part1 = participants.GetRange(start, end);
                    start = end++;
                    end = end + numPlayersPerTable;
                    AddTablePlayer(tables[i].TableId, part1);
                }
                end = participants.Count();
                var part2 = participants.GetRange(start, end - 2);
                AddTablePlayer(pointer, part2);
                var allTacts = db.Tacts.Where(x => x.GameId == gameId).ToList();
                var notUsedTacts = allTacts.Where(x => x.TactId > game[0].CurrentTact).OrderBy(o => o.TactId).ToList();

                var tactId = notUsedTacts.FirstOrDefault();

                if (tactId != null)
                {
                    var tactDiff = tactId.TactId - game[0].CurrentTact;
                    if (tactDiff == 1)
                    {
                        var currentTact = SetCurrentTact(game[0], tactId.TactId);
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void DeletePlayerShuffle(int id)
        {
            try
            {
                log.Debug("Games DeletePlayerShuffle()");

                var tabPlRecords = db.TablePlayers.Where(t => t.TableId == id).ToList();

                foreach (var item in tabPlRecords)
                {
                    TablePlayer tablePlayer = db.TablePlayers.Find(item.Id);
                    db.TablePlayers.Remove(tablePlayer);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void AddTablePlayer(int tableId, List<Project> participantsList)
        {
            try
            {
                log.Debug("Games AddTablePlayer()");

                TablePlayer tablePlayer = new TablePlayer();

                for (var i = 0; i < participantsList.Count(); i++)
                {
                    tablePlayer.TableId = tableId;
                    tablePlayer.ProjectParticipantId = participantsList[i].Id;

                    var isExist = db.TablePlayers.Where(t => t.TableId == tablePlayer.TableId).Where(y => y.ProjectParticipantId == tablePlayer.ProjectParticipantId).ToList();
                    if (isExist.Count() == 0)
                    {
                        if (ModelState.IsValid)
                        {
                            db.TablePlayers.Add(tablePlayer);
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        public ActionResult EndGame(int gid)
        {
            try
            {
                log.Debug("Games EndGame()");

                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return View("~/Views/Shared/Error.cshtml");
        }
       
        public JsonResult isOnlyGivenConTypes(int gid)
        {
            try
            {
                log.Debug("Games isOnlyGivenConTypes()[JSON]");

                var game = FindGame(gid);
                var result = game[0].IsOnlyGivenConTypes;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsOnlyGivenNeedTypes(int gid)
        {
            try
            {
                log.Debug("Games IsOnlyGivenNeedTypes()[JSON]");

                var game = FindGame(gid);
                var result = game[0].IsOnlyGivenNeeds;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsOnlyGivenBenefitTypes(int gid)
        {
            try
            {
                log.Debug("Games IsOnlyGivenBenefitTypes()[JSON]");

                var game = FindGame(gid);
                var result = game[0].IsOnlyGivenBenefits;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json("error", JsonRequestBehavior.AllowGet);

        }

        public JsonResult WhereIsMyGroup(int gid)
        {
            try
            {
                log.Debug("Games WhereIsMyGroup()[JSON]");
                var uid = User.Identity.GetUserId();
                var uprojects = db.Projects.Where(u => u.AuthorId == uid).ToList<Project>();
                var game = FindGame(gid);
                var tables = game[0].Table;
                var projects = game[0].Projects.ToList();

                List<Project> participant = new List<Project>();
                foreach (var gp in projects)
                {
                    foreach (var up in uprojects)
                    {
                        var a = projects.Where(x => x.Id == up.Id).FirstOrDefault();
                        if (a != null)
                        {
                            participant.Add(a);
                        }
                    }
                }

                int tid = participant[0].Id;
                var tabplayer = db.TablePlayers.Where(x => x.ProjectParticipantId == tid).OrderByDescending(x => x.Id).First();

                int tableId = tabplayer.TableId;
                var tempNum = db.Tables.Where(x => x.TableId == tableId).ToList();
                var tableNumber = tempNum[0].TableNumber;

                return Json(tableNumber, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTimeToIntroduction(int gid)
        {
            try
            {
                log.Debug("Games GetTimeToIntroduction()[JSON]");

                Game game = db.Games.Find(gid);
                int timeToPlayer = game.PlayerIntroductionTime;
               // int timeToGroupIntroduction = (game.NumPlayers / game.NumTables) * game.PlayerIntroductionTime;

               // JArray JsonTimeToIntroduction = new JArray();
              //  dynamic record = new JObject();
               // record.timeToPlayer = timeToPlayer;
               // record.timeToGroupIntroduction = timeToGroupIntroduction;
               // JsonTimeToIntroduction.Add(record);

                string json = timeToPlayer.ToString();

                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json("error", JsonRequestBehavior.AllowGet);
        }

        public JsonResult UserProjects(int gid)
        {
            ApplicationDbContext mycontext = new ApplicationDbContext();
            UserStore<ApplicationUser> mystore = new UserStore<ApplicationUser>(mycontext);
            ApplicationUserManager userMgr = new ApplicationUserManager(mystore);

            var rawProjects = new List<Project>();
            JArray JsonConnectionRequests = new JArray();
            dynamic record = new JObject();

            foreach (Game t in db.Games.Where(y => y.GameId == gid).Include(u => u.Projects))
            {
                foreach (Project pr in t.Projects)
                {
                    var user = userMgr.FindById(pr.AuthorId);

                    record.name = user.FirstName;
                    record.surname = user.LastName;
                    var rawRt = db.UserRatings.Where(x => x.UserId == pr.AuthorId).ToList();
                    record.rating = rawRt.Count == 0 ? 0 : rawRt[0].Rating;
                    record.projectName = pr.Name;
                    record.what = pr.What;
                    record.whom = pr.Whom;
                    record.feature = pr.Feature;
                    record.contact = pr.Contact;
                    var rawAppr = db.Connections.Where(x => x.SenderId == pr.Id || x.RecieverId == pr.Id).ToList();
                    record.approvedConnections = rawAppr.Count();

                    JsonConnectionRequests.Add(record);
                    record = new JObject();
                }
            }

            string json2 = JsonConnectionRequests.ToString();

            return Json(json2, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Pixi example
        public ActionResult pixiShow()
        {
            return View();
        }

        public ActionResult pixiCanvas()
        {
            return View();
        }
    }

    static class MyExtensions
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}





//public ActionResult GameCreatedSuccessful(int gameId)
//{
//    ViewBag.gameId = gameId;
//    return View("GameCreationSuccess");
//}

// GET: Games/Details/5
//public ActionResult Details(int? id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    Game game = db.Games.Find(id);
//    if (game == null)
//    {
//        return HttpNotFound();
//    }
//    return View(game);
//}



// GET: Games/Edit/5
//public ActionResult Edit(int? id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    Game game = db.Games.Find(id);
//    if (game == null)
//    {
//        return HttpNotFound();
//    }
//    return View(game);
//}

// POST: Games/Edit/5
// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult Edit([Bind(Include = "Id,AuthorId,NumPlayers,NumTables,NumTacts,TactDuration,CreationTime,Code")] Game game)
//{
//    if (ModelState.IsValid)
//    {
//        db.Entry(game).State = EntityState.Modified;
//        db.SaveChanges();
//        return RedirectToAction("Index");
//    }
//    return View(game);
//}

// GET: Games/Delete/5
//public ActionResult Delete(int? id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    Game game = db.Games.Find(id);
//    if (game == null)
//    {
//        return HttpNotFound();
//    }
//    return View(game);
//}

// POST: Games/Delete/5
//[HttpPost, ActionName("Delete")]
//[ValidateAntiForgeryToken]
//public ActionResult DeleteConfirmed(int id)
//{
//    Game game = db.Games.Find(id);
//    db.Games.Remove(game);
//    db.SaveChanges();
//    return RedirectToAction("Index");
//}


//private bool DeleteUsedTact(int tactId)
//{
//    try
//    {
//        Tact tact = db.Tacts.Find(tactId);
//        db.Tacts.Remove(tact);
//        db.SaveChanges();
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex);
//    }
//    return false;
//}


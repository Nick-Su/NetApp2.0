using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

using NetApp.Models;
using System.Data;
using System.Data.Entity;
using System.Web.Mvc;
using log4net;

using Microsoft.AspNet.Identity;
using System.Threading;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR.Json;
using Newtonsoft.Json.Linq;

namespace NetApp.Hubs
{
    public class TimerHub : Hub
    {
        private static readonly DatabaseContext db = new DatabaseContext();
        private static ILog log = LogManager.GetLogger("ErrorLog");

        static List<User> Users = new List<User>();

        public Task JoinGroup(string group)
        {
            return Groups.Add(Context.ConnectionId, group);
        }

        public Task LeaveRoom(string roomName)
        {
            return Groups.Remove(Context.ConnectionId, roomName);
        }

        public Task RedirectUser(string group)
        {
            return Clients.Group(group).redirectAlert();
        }

        public Task UpdateTimerToGroup(string group, string message)
        {
            return Clients.Group(group).updateTime(message);
        }

        public async Task Main(string group, int gid)
        {
            try
            {
                log.Debug("TimerHub Main()");

                var game = db.Games.Find(gid);

                if (game.NumTables > game.Projects.Count())
                {
                    await WarnAdminFewPlayers(group);
                }
                else
                {
                    int sec = GetTransitionTime(gid);
                    ActivateGame(gid);
                    ChangeStatus(gid, 1);
                    await ShowGameInfo(group);
                    await RedirectUsersToTransitionPage(group, gid);
                    await ShowCurrentStage(group, gid);
                    SetCurrentStage(gid, 1);
                    await StartTimer(group, gid, sec);
                    
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private async Task StartTimer(string group, int gid, int sec)
        {
            var d = new DateTime(2020, 1, 1, 0, 0, 0);
            d = d.AddSeconds(sec);
            System.Threading.Thread.Sleep(5000);
            for (int i = 0; i <= sec; i++)
            {
                await UpdateTimerToGroup(group, d.ToString("mm:ss"));
                d = d.AddSeconds(-1);
                System.Threading.Thread.Sleep(1000);
            }
            int currentStage = GetCurrentStage(gid);
            switch (currentStage)
            {
                case 1:
                    SetCurrentStage(gid, 2);
                    int introTimeSec = GetIntroductionTime(gid);
                    await RedirectUsersToIntroductionPage(group, gid);
                    await ShowCurrentStage(group, gid);
                    await Task.WhenAll(InitSubgroupTimer(gid), StartTimer(group, gid, introTimeSec)); 
                    break;
                case 2:
                    SetCurrentStage(gid, 3);
                    int minsNetworkingTime = GetTactDuration(gid);
                    await RedirectUsersToPlayRoom(group, gid);
                    await ShowCurrentStage(group, gid);
                    await StartTimer(group, gid, minsNetworkingTime); 
                    break;
                case 3:
                    if (CheckIfLastTact(gid))
                    {
                        SetCurrentStage(gid, 4);
                        int minsAproveRequests = GetAproveTime(gid);
                        await RedirectUsersToAproveRequestsPage(group, gid);
                        await ShowAdminLeaderBoard(group, gid);
                        await ShowCurrentStage(group, gid);

                        await StartTimer(group, gid, minsAproveRequests);
                    } else
                    {
                        ShufflePlayers(gid);
                        int minsTact = GetTransitionTime(gid);
                        SetCurrentStage(gid, 1);
                        await RedirectUsersToTransitionPage(group, gid);
                        await ShowCurrentStage(group, gid);
                        await StartTimer(group, gid, minsTact);
                    }
                    break;
                case 4:
                    ChangeStatus(gid, 2);
                    SetCurrentStage(gid, 5);
                    await RedirectUsersToEndGame(group, gid);
                    await HideDisclaimer(group);
                    await ShowCurrentStage(group, gid);
                    break;
                default:
                    break;
            }
        }

        private async Task InitSubgroupTimer(int gid)
        {
            new Thread(() =>
            {
                DatabaseContext db = new DatabaseContext();
                var tableList = db.Tables.Where(x => x.GameId == gid).ToList();
                var game = db.Games.Find(gid);
                var timeToPlayerIntro = game.PlayerIntroductionTime;
                
                foreach (var item in tableList)
                {
                    var subGroupName = gid.ToString() + item.TableNumber.ToString();
                    var tblPlayers = db.TablePlayers.Where(c => c.TableId == item.TableId).ToList();
                    new Thread(() => {
                        _ = StartIntroductionTimer(subGroupName, timeToPlayerIntro, tblPlayers);
                    }).Start();
                }
                }).Start();
        }

        private async Task StartIntroductionTimer(string subgroup, int sec, List<TablePlayer> tblPlayers)
        {
            ApplicationDbContext mycontext = new ApplicationDbContext();
            UserStore<ApplicationUser> mystore = new UserStore<ApplicationUser>(mycontext);
            ApplicationUserManager userMgr = new ApplicationUserManager(mystore);

            int counter = 0;
            foreach (var player in tblPlayers)
            {
                var project = db.Projects.Where(v => v.Id == player.ProjectParticipantId).FirstOrDefault();

                var user = userMgr.FindById(project.AuthorId);
                var username = user.FirstName + " " + user.LastName;

                var d = new DateTime(2020, 1, 1, 0, 0, 0);
                d = d.AddSeconds(sec);

                AskForSpeakersList(subgroup);
                if (counter == 0)
                {
                    Thread.Sleep(6000);
                }

                for (int i = 0; i <= sec; i++)
                {
                    await UpdateTimerToIntroductionGroup(subgroup, d.ToString("mm:ss"), username);
                    d = d.AddSeconds(-1);
                    Thread.Sleep(1000);
                }
                counter++;
            }
            DeleteCurrentSpeakerInfo(subgroup);
        }

        public async Task AskForSpeakersList(string subgroup)
        {
            await Clients.Group(subgroup).askForSpeakersList();
        }

        public async Task UpdateTimerToIntroductionGroup(string subgroup, string time, string username)
        {
            await Clients.Group(subgroup).updateIntroductionGroupTime(time, username);
        }

        public async Task DeleteCurrentSpeakerInfo(string subgroup)
        {
            await Clients.Group(subgroup).deleteCurrentSpeakerInfo();
        }

        public async Task GetSpeakersList(int tid)
        {
            ApplicationDbContext mycontext = new ApplicationDbContext();
            UserStore<ApplicationUser> mystore = new UserStore<ApplicationUser>(mycontext);
            ApplicationUserManager userMgr = new ApplicationUserManager(mystore);

            var tablePlayers = db.TablePlayers.Where(x => x.TableId == tid).ToList();

            List<string> wplayers = new List<string>();
            foreach (var player in tablePlayers)
            {
                var project = db.Projects.Where(v => v.Id == player.ProjectParticipantId).FirstOrDefault();
                var user = userMgr.FindById(project.AuthorId);
                var username = user.FirstName + " " + user.LastName;

                wplayers.Add(username);
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                wplayers
            });

            await Clients.Caller.showWaitingSpeakerList(json);
        }

        public bool ActivateGame(int gid)
        {
            try
            {
                log.Debug("TimerHub ActivateGame()");

                Game game = db.Games.Find(gid);
                var tableNum = game.NumTables;
                var tactNum = game.NumTacts;

                CreateTables(gid, tableNum);
                CreateTacts(gid, tactNum);
                ShufflePlayers(gid);
                var allTacts = db.Tacts.Where(x => x.GameId == gid).OrderBy(o => o.TactId).ToList<Tact>();
                var tactId = allTacts[0].TactId;
                var currentTact = SetCurrentTact(gid, tactId);
                ShowCurrentTact(gid.ToString(), Convert.ToInt32(gid));
                ShowTotalTactNum(gid.ToString(), Convert.ToInt32(gid));

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return false;
        }

        public void ShufflePlayers(int gid)
        {
            try
            {
                log.Debug("TimerHub ShufflePlayers()");
                var game = FindGame(gid);
                List<Project> participants = game[0].Projects.ToList();
                participants.Shuffle();
                var numTables = Convert.ToDecimal(game[0].NumTables);
                var numPlayersPerTable = Convert.ToInt32(Math.Round((participants.Count() / numTables), 2));
                var estimatedPlayersNum = numPlayersPerTable * numTables;
                if (estimatedPlayersNum > participants.Count())
                {
                    numPlayersPerTable--;
                }
                var tables = game[0].Table.ToList();
                for (var i = 0; i < tables.Count(); i++)
                {
                    DeletePlayerShuffle(tables[i].TableId);
                }

                var start = 0;
                var pointer = 0;

                for (var i = 0; i < tables.Count(); i++)
                {
                    var part1 = participants.GetRange(start, numPlayersPerTable);
                    start += numPlayersPerTable;
                    pointer = tables[i].TableId;
                    AddTablePlayer(tables[i].TableId, part1);
                }

                if (participants.Count() % numTables != 0)
                {
                    var playersAlreadySorted = numPlayersPerTable * tables.Count();
                    var sortedIndex = playersAlreadySorted;

                    var numPlayers = participants.Count() - playersAlreadySorted;
                    var part2 = participants.GetRange(sortedIndex, numPlayers);
                    AddTablePlayer(pointer, part2);
                }

                var allTacts = db.Tacts.Where(x => x.GameId == gid).ToList();
                var notUsedTacts = allTacts.Where(x => x.TactId > game[0].CurrentTact).OrderBy(o => o.TactId).ToList();

                var tactEntity = notUsedTacts.FirstOrDefault();

                if (tactEntity != null)
                {
                    var tactDiff = tactEntity.TactId - game[0].CurrentTact;
                    if (tactDiff == 1)
                    {
                        var currentTact = SetCurrentTact(gid, tactEntity.TactId);
                        ShowCurrentTact(gid.ToString(), Convert.ToInt32(gid));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private bool CreateTables(int gid, int tableNum)
        {
            try
            {
                log.Debug("Games CreateTables()");

                var isExist = db.Tables.Where(t => t.GameId == gid).ToList();
                if (isExist.Count() > 0)
                {
                    return true;
                }

                Table table = new Table();
                for (var i = 1; i <= tableNum; i++)
                {
                    table.GameId = gid;
                    table.TableNumber = i;

                    db.Tables.Add(table);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return false;
        }

        private bool CreateTacts(int gid, int numTacts)
        {
            try
            {
                log.Debug("Games CreateTacts()");

                var isExist = db.Tacts.Where(y => y.GameId == gid).ToList();
                if (isExist.Count() > 0)
                {
                    return false;
                }

                Tact tact = new Tact();

                for (var i = 1; i <= numTacts; i++)
                {
                    tact.GameId = gid;
                    tact.TactNumber = i;

                    db.Tacts.Add(tact);
                    db.SaveChanges();
                    
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return false;
        }

        public Task RedirectUsersToTransitionPage(string group, int gid)
        {
            return Clients.Group(group).redirectToTransition(gid);
        }

        public Task RedirectUsersToIntroductionPage(string group, int gid)
        {
            return Clients.Group(group).redirectToIntroduction(gid);
        }

        public Task RedirectUsersToPlayRoom(string group, int gid)
        {
            return Clients.Group(group).redirectToPlayRoom(gid);
        }

        public Task RedirectUsersToAproveRequestsPage(string group, int gid)
        {
            return Clients.Group(group).redirectToAproveRequests(gid);
        }

        public Task RedirectUsersToEndGame(string group, int gid)
        {
            return Clients.Group(group).redirectUsersToEndGame(gid);
        }

        public Task WarnAdminFewPlayers(string group)
        {
            return Clients.Group(group).warnAdminFewPlayers();
        }

        public Task HideDisclaimer(string group)
        {
            return Clients.Group(group).hideDisclaimer();
        }

        public Task ShowGameInfo(string group)
        {
            return Clients.Group(group).showGameInfo();
        }

        public Task ShowAdminLeaderBoard(string group, int gid)
        {
            return Clients.Group(group).showAdminLeaderBoard();
        }

        public async Task ShowCurrentStage(string group, int gid)
        {
            Game game = db.Games.Find(gid);
            int stage = game.CurrentStage;
            string stageInfo;
            switch (stage)
            {
                case 1:
                    stageInfo = "Перемещение групп";
                    break;
                case 2:
                    stageInfo = "Знакомство участников";
                    break;
                case 3:
                    stageInfo = "Нетворкинг";
                    break;
                case 4:
                    stageInfo = "Рассмотрение запросов";
                    break;
                default:
                    stageInfo = "";
                    break;

            }
            await Clients.Group(group).showCurrentStage(stageInfo);
        }

        public async Task GetStageName(int gid)
        {
            Game game = db.Games.Find(gid);
            int stage = game.CurrentStage;
            string stageInfo;
            switch (stage)
            {
                case 1:
                    stageInfo = "Перемещение групп";
                    break;
                case 2:
                    stageInfo = "Знакомство участников";
                    break;
                case 3:
                    stageInfo = "Нетворкинг";
                    break;
                case 4:
                    stageInfo = "Рассмотрение запросов";
                    break;
                case 5:
                    stageInfo = "Результаты";
                    break;
                default:
                    stageInfo = "";
                    break;

            }
            await Clients.Caller.showStageName(stageInfo);
        }

        public void ShowCurrentTact(string group, int gid)
        {
            try
            {
                log.Debug("TimerHub ShowCurrentTact()");

                var game = db.Games.Find(gid);
                var currentTactId = game.CurrentTact;

                var tact = db.Tacts.Where(c => c.TactId == currentTactId).First();
                int tnum = tact.TactNumber;
                Clients.Group(group).showCurrentTact(tnum);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        public void ShowTotalTactNum(string group, int gid)
        {
            try
            {
                log.Debug("TimerHub ShowTotalTactNum()");

                Game game = db.Games.Find(gid);
                int tactNum = game.NumTacts;
                Clients.Group(group).showTotalTactNum(tactNum);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }

        public async Task GetNetworkingProgress(int gid)
        {
            try
            {
                log.Debug("TimerHub ShowTotalTactNum()");

                Game game = db.Games.Find(gid);
                int totalTactNum = game.NumTacts;

                var currentTactId = game.CurrentTact;
                var tact = db.Tacts.Where(c => c.TactId == currentTactId).First();
                int currTactNum = tact.TactNumber;

                await Clients.Caller.showNetworkingProgress(currTactNum, totalTactNum);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        public async Task ShowMyGroupNumber(int gid)
        {
            var uid = Context.User.Identity.GetUserId();
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
            await Clients.Caller.showMyGroupNumber(tableNumber, tableId);
        }

        public async Task GetGameStage(int gid)
        {
            int currentStage = GetCurrentStage(gid);
            await Clients.Caller.returnPlayerToActualStage(currentStage);
        }

        public int GetTransitionTime(int gid)
        {
            Game game = db.Games.Find(gid);
            return game.TransitionGroup;
        }
        public int GetTactDuration(int gid)
        {
            Game game = db.Games.Find(gid);
            return game.TactDuration;
        }

        public int GetAproveTime(int gid)
        {
            Game game = db.Games.Find(gid);
            return game.TimeToAproveRequests;
        }
        public int GetIntroductionTime(int gid)
        {
            Game game = db.Games.Find(gid);

            int actualNumPlayers = 0;
            var tableList = db.Tables.Where(x => x.GameId == gid).ToList();
            foreach (var item in tableList)
            {
                var tbplayersnum = db.TablePlayers.Where(z => z.TableId == item.TableId).ToList().Count();
                actualNumPlayers += tbplayersnum;
            }
            int playerPerGroup = (int)Math.Ceiling((double)actualNumPlayers / game.NumTables);
            int timeToGroupIntroduction = playerPerGroup * game.PlayerIntroductionTime + 15;
            return timeToGroupIntroduction;
        }

        public int GetCurrentStage(int gid)
        {
            DatabaseContext db = new DatabaseContext();
            Game game = db.Games.Find(gid);
            return game.CurrentStage;
        }

        public bool CheckIfLastTact(int gameId)
        {
            DatabaseContext db = new DatabaseContext();
            try
            {
                log.Debug("Games CheckIfLastTact()");

                Game game = db.Games.Where(u => u.GameId == gameId).Include(e => e.Tact).FirstOrDefault();
                var lastTact = game.Tact.OrderBy(t => t.TactId).ToList().LastOrDefault();

                if (game.CurrentTact == lastTact.TactId)
                {
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

        private void DeletePlayerShuffle(int id)
        {
            DatabaseContext db = new DatabaseContext();
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
            DatabaseContext db = new DatabaseContext();
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
                        db.TablePlayers.Add(tablePlayer);
                        db.SaveChanges();  
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        public void Connect(string userName, string groupName)
        {
            try
            {
                var id = Context.ConnectionId;

            if (!Users.Any(x => x.ConnectionId == id))
            {
                Groups.Add(Context.ConnectionId, groupName);

                Clients.Group(groupName).onConnected(id, userName, Users);
            }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                Users.Remove(item);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.Name);
            }
            
            return base.OnDisconnected(stopCalled);
        }

        private List<Game> FindGame(int gid)
        {
            DatabaseContext db = new DatabaseContext();
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

        private bool SetCurrentTact(int gid, int TactId)
        {
            log.Debug("Games SetCurrentTact()");

            var game = db.Games.Find(gid);
            game.CurrentTact = TactId;
            db.Entry(game).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }

        private bool ChangeStatus(int gid, int statusNum)
        {
            var game = db.Games.Find(gid);
            game.Status = statusNum;
            db.Entry(game).State = EntityState.Modified;
            db.SaveChanges();

            return true;
        }

        public bool SetCurrentStage(int gid, int stageId)
        {
            var game = db.Games.Find(gid);
            game.CurrentStage = stageId;
            db.Entry(game).State = EntityState.Modified;
            db.SaveChanges();
            return true;
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

    public class MyExt : Controller
    {
        private readonly DatabaseContext db = new DatabaseContext();
        private static ILog log = LogManager.GetLogger("ErrorLog");

        public static MyExt C1;


       
    }
}
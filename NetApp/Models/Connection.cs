using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetApp.Models
{
    public class Connection
    {
        public int ConnectionId { get; set; }
        public int SenderId { get; set; }
        public int RecieverId { get; set; }
        public int GameId { get; set; }
        [JsonIgnore]
        public Game Game { get; set; }
    }
}
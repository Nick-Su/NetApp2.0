using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetApp.Models
{
    public class Table
    {
        [Key]
        public int TableId { get; set; }
        public int TableNumber { get; set; }
        // Rels
        public int GameId { get; set; }
        public Game Game { get; set; }
        [JsonIgnore]
        public ICollection<TablePlayer> TablePlayer { get; set; }

       // public ICollection<Game> Game { get; set; }
    }
}
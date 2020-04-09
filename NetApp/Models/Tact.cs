using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetApp.Models
{
    public class Tact
    {
        [Key]
        public int TactId { get; set; }
        public int TactNumber { get; set; }

        //rels
        public int GameId { get; set; }
        [JsonIgnore]
        public Game Game { get; set; }
    }
}
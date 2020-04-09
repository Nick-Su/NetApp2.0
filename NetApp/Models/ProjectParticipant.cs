using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetApp.Models
{
    public class Games
    {
        [Key]
        public int Id { get; set; }
        public int GameId { get; set; }
        public int ProjectId { get; set; }

        // ICollection<Project> Project { get; set; }
        [JsonIgnore]
        public Game Game { get; set; }
    }
}
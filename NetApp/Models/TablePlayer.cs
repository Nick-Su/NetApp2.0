using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetApp.Models
{
    public class TablePlayer
    {
        [Key]
        public int Id { get; set; }
        public int TableId { get; set; }
        public int ProjectParticipantId { get; set; }

        // Rels
        [JsonIgnore]
        public Table Table { get; set; }
    }
}
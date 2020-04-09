using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NetApp.Models
{
    public class ConnectionType
    {
        [Key]
        public int ConnectionTypeId { get; set; }

        [DisplayName("Название типа связи")]
        public string Decription { get; set; }

        public int GameId { get; set; }
        [JsonIgnore]
        public Game Game { get; set; }
    }
}
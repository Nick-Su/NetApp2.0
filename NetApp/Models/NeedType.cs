using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NetApp.Models
{
    public class NeedType
    {
        [Key]
        public int NeedTypeId { get; set; }
        [DisplayName("Название типа запроса")]
        public string Description { get; set; }

        public int GameId { get; set; }
        [JsonIgnore]
        public Game Game { get; set; }
    }
}
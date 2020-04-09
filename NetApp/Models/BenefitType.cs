using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NetApp.Models
{
    public class BenefitType
    {
        [Key]
        public int BenefitTypeId { get; set; }
        [DisplayName("Название типа пользы")]
        public string Description { get; set; }

        public int GameId { get; set; }
        [JsonIgnore]
        public Game Game { get; set; }
    }
}
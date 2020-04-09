using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetApp.Models
{
    public class ConnectionRequest
    {
        [Key]
        public int ConnectionRequestId { get; set; }
        [DisplayName("Тип связи")]
        public string ConnectionType { get; set; }
        [DisplayName("Мне необходимо")]
        public string SenderResourceRequest { get; set; }
        [DisplayName("Я буду полезен тем...")]
        public string SenderGivenBenefit { get; set; }
        public int SenderProjectId { get; set; }
        [DisplayName("Я получу ресурс")]
        public string RecieverGetResource { get; set; }
        [DisplayName("Я получу пользу")]
        public string RecieverGetBenefit { get; set; }
        public int RecieverProjectId { get; set; }
        [DisplayName("Запрос принят")]
        public int IsApproved { get; set; }

        public int SenderIsRead { get; set; }
        public int RecieverIsRead { get; set; }

        public int GameId { get; set; }
        [JsonIgnore]
        public Game Game { get; set; }

    }
}
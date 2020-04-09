using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetApp.Models
{
    public class Game
    {
        public Game()
        {
            this.Projects = new HashSet<Project>();
        }

        [Key]
        public int GameId { get; set; }
        public string Author { get; set; }
        [DisplayName("Количество участников")]
        public int NumPlayers { get; set; }
        [DisplayName("Количество групп")]
        public int NumTables { get; set; }
        [DisplayName("Количество раундов")]
        public int NumTacts { get; set; }
        [DisplayName("Время на такт, мин.")]
        public int TactDuration { get; set; }
        [DisplayName("Время на переход между группами, мин.")]
        public int TransitionGroup { get; set; }
        [DisplayName("Время для представления участинка, мин.")]
        public int PlayerIntroductionTime { get; set; }
        [DisplayName("Время на подтверждение запросов, мин.")]
        public int TimeToAproveRequests { get; set; }
        public DateTime CreationTime { get; set; }
        public int Code { get; set; }
        public int Status { get; set; }
        [ConcurrencyCheck]
        public int CurrentTact { get; set; }
        [DisplayName("Типы связей ограничены списком")]
        public bool IsOnlyGivenConTypes { get; set; }
        [DisplayName("Типы запросов ограничены списком")]
        public bool IsOnlyGivenNeeds { get; set; }
        [DisplayName("Типы польз ограничены списком")]
        public bool IsOnlyGivenBenefits { get; set; }
        public int CurrentStage { get; set; }

        [JsonIgnore]
        public virtual ICollection<Project> Projects { get; set; }
        public ICollection<Tact> Tact { get; set; }
        public ICollection<Table> Table { get; set; }
        public ICollection<ConnectionType> ConnectionType { get; set; }
        public ICollection<NeedType> NeedType { get; set; }
        public ICollection<BenefitType> BenefitType { get; set; }
        public ICollection<ConnectionRequest> ConnectionRequest { get; set; }
        [JsonIgnore]
        public ICollection<Connection> Connection { get; set; }
        public ICollection<Games> ProjectParticipant { get; set; } // delete & crash everything

    }
}

//
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetApp.Models
{
    public class Project
    {
       
        public Project()
        {
            this.Games = new HashSet<Game>();
        }

        [Key]
        public int Id { get; set; }
        
        
        [DisplayName("Проект")]
        public string Name { get; set; }

        
        [DisplayName("Кому помогает?")]
        public string Whom { get; set; }

        
        [DisplayName("Что делает?")]
        public string What { get; set; }

        
        [DisplayName("В чем фишка?")]
        public string Feature { get; set; }

      
        [DisplayName("Представитель проекта")]
        public string Author { get; set; }

       
        [DisplayName("Контакт для связи")]
        public string Contact { get; set; }

        public string AuthorId { get; set; }

        public DateTime Date { get; set; }

        // Relationships
        [JsonIgnore]
        public virtual ICollection<Game> Games { get; set; }
    }
}
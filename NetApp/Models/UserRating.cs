using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetApp.Models
{
    public class UserRating
    {
        [Key]
        public int UserRatingId { get; set; }
        public int Rating { get; set; }
        public string UserId { get; set; }
    }
}
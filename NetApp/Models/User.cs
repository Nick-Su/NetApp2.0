using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetApp.Models
{
    public class User
    {
        public static object Identity { get; internal set; }
        public string ConnectionId { get; set; }
        public string Name { get; set; }
    }
}
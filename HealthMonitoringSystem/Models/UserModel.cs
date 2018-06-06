using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthMonitoringSystem.Models
{
    public class UserModel
    {
        public int userId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public bool result { get; set; }
        public string Message { get; set; }
    }
}
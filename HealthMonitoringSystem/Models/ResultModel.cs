using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthMonitoringSystem.Models
{
    public class ResultModel
    {
        public int userId { get; set; }
        public int resultId {get ; set;}
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public string date { get; set; }
        public string result { get; set; }
        public bool isNull { get; set; }

    }
}
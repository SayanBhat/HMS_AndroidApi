using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthMonitoringSystem.Models
{
    public class DiabeticModel
    {
        public double glucose { get; set; }
        public double bloodPressure { get; set; }

        public double bmi { get; set; }
        public double insulin { get; set; }
        public double age { get; set; }

        public double decisionResult { get; set;  }
        public double logisticProbability { get; set; }
    }
}
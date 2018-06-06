using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthMonitoringSystem.Models
{
    public class LiverAttributesModel
    {
        public double Age { get; set; }
        public double Gender { get; set; } //Male = 1 ,Female = 2 
        public double Total_Bilirubin { get; set; }
        public double Direct_Bilirubin { get; set; }
        public double Alkaline_Phosphotase { get; set; }
        public double Alamine_Aminotransferase { get; set; }
        public double Aspartate_Aminotransferase { get; set; }
        public double Total_Protiens { get; set; }
        public double Albumin { get; set; }
        public double Albumin_and_Globulin_Ratio { get; set; }
        public double Dataset { get; set; }

        public double decisionResult { get; set; }
        public double logisticProbability { get; set; }
    }
}
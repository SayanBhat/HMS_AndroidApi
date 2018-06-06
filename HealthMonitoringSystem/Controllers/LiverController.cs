using HealthMonitoringSystem.Controllers.Classes;
using HealthMonitoringSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HealthMonitoringSystem.Controllers
{
    public class LiverController : ApiController
    {
        public LiverAttributesModel Post([FromBody]LiverAttributesModel model)
        {

            LiverDecisionTree ldt = new LiverDecisionTree(model);
            LiverAttributesModel result = new LiverAttributesModel();

            result.decisionResult = ldt.treeDeciding();
            result.logisticProbability = ldt.regressionProbability();

            return result;
        }
    }
}

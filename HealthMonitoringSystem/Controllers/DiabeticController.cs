using System.Web.Http;
using HealthMonitoringSystem.Controllers.Classes;
using HealthMonitoringSystem.Models;

namespace HealthMonitoringSystem.Controllers
{
    public class DiabeticController : ApiController
    {
        public DiabeticModel Post([FromBody]DiabeticModel dbtMod)
        {

            DiabeticDecisionTree diabeticTree = new DiabeticDecisionTree(dbtMod);
            DiabeticModel returnMod = new DiabeticModel();

            returnMod.decisionResult = diabeticTree.treeDeciding();
            returnMod.logisticProbability = diabeticTree.regressionProbability();
            
            return returnMod;
        }
    }
}

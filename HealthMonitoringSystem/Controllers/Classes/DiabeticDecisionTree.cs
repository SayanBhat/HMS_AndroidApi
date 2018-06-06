using System.IO;
using System.Linq;
using Accord.IO;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Math;
using System.Data;
using System.Web.Hosting;
using HealthMonitoringSystem.Models;
using Accord.Statistics.Models.Regression.Fitting;
using Accord.Statistics.Models.Regression;
using Accord.Statistics.Analysis;

namespace HealthMonitoringSystem.Controllers.Classes
{
    public class DiabeticDecisionTree
    {
        double glucose { get; set; }
        double bloodPressure { get; set; }

        double bmi { get; set; }
        double insulin { get; set; }
        double age { get; set; }

        static DecisionTree tree { get; set; }
        static LogisticRegression regression { get; set; }
        static LogisticRegressionAnalysis lra { get; set; }


        public DiabeticDecisionTree(double glucose, double bloodPressure, double insulin, double bmi, double age ) {
            this.glucose = glucose;
            this.bloodPressure = bloodPressure;
            this.insulin = insulin;
            this.bmi = bmi;
            this.age = age;
        }
        public DiabeticDecisionTree(DiabeticModel dbtMod)
        {
            glucose = dbtMod.glucose;
            bloodPressure = dbtMod.bloodPressure;
            insulin = dbtMod.insulin;
            bmi = dbtMod.bmi;
            age = dbtMod.age;
        }

        public static void training()
        {
            string filepath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath + @"Content\files\diabetes.xls");

            DataTable table = new ExcelReader(filepath).GetWorksheet("diabetes");

            double[][] inputs = table.ToJagged<double>("Glucose", "BloodPressure", "Insulin", "BMI", "Age");
            int[] outputs = table.ToArray<int>("Outcome");

            DecisionVariable[] var =
            {
                new DecisionVariable("G",DecisionVariableKind.Continuous),
                new DecisionVariable("B",DecisionVariableKind.Continuous),
                new DecisionVariable("I",DecisionVariableKind.Continuous),
                new DecisionVariable("BMI",DecisionVariableKind.Continuous),
                new DecisionVariable("A",DecisionVariableKind.Continuous),
            };

            tree= new DecisionTree(var, 2);

            C45Learning teacher = new C45Learning(tree);
            teacher.Learn(inputs, outputs);


            //From Here Trying to find out the probability
            var learner = new IterativeReweightedLeastSquares<LogisticRegression>()
            {
                Tolerance = 1e-6,  // Let's set some convergence parameters
                MaxIterations = 1000,  // maximum number of iterations to perform
                Regularization = 0
            };
            regression = learner.Learn(inputs, outputs);

            //lra = new LogisticRegressionAnalysis()
            //{
            //    Regularization = 0
            //};
            //lra.Learn(inputs, outputs);

        }


        public double regressionProbability() {
            double probablity = regression.Probability(new double[] { glucose, bloodPressure, insulin, bmi, age });
            // return lra.Regression.Probability(new double[] { glucose, bloodPressure, insulin, bmi, age }); 
            return probablity;
        }
        public double  treeDeciding()
        {

            return tree.Decide(new double[] { glucose, bloodPressure, insulin, bmi, age });
        }
    }
}
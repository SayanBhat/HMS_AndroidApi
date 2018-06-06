using Accord.IO;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Math;
using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Fitting;
using HealthMonitoringSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace HealthMonitoringSystem.Controllers.Classes
{
    public class LiverDecisionTree
    {
        private static LogisticRegression regression;
        private static DecisionTree tree;
        LiverAttributesModel lam = new LiverAttributesModel();

        public LiverDecisionTree(LiverAttributesModel obj)
        {
            this.lam = obj;
        }

        public static void training()
        {
            string filepath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath + @"Content\files\indian_liver_patient.xls");

            DataTable table = new ExcelReader(filepath).GetWorksheet("indian_liver_patient");

            double[][] inputs = table.ToJagged<double>("Age", "Gender","Total_Bilirubin", "Direct_Bilirubin", "Alkaline_Phosphotase"
                                                           , "Alamine_Aminotransferase", "Aspartate_Aminotransferase"
                                                           , "Total_Protiens", "Albumin", "Albumin_and_Globulin_Ratio");
            int[] outputs = table.ToArray<int>("Dataset");

            for(int i=0;i<outputs.Length;i++)
            {
                outputs[i]=outputs[i]-1;
            }

            DecisionVariable[] var =
            {
                new DecisionVariable("A",DecisionVariableKind.Continuous),
                new DecisionVariable("G",DecisionVariableKind.Continuous),
                new DecisionVariable("TB",DecisionVariableKind.Continuous),
                new DecisionVariable("DB",DecisionVariableKind.Continuous),
                new DecisionVariable("AP",DecisionVariableKind.Continuous),
                new DecisionVariable("AA",DecisionVariableKind.Continuous),
                new DecisionVariable("AS",DecisionVariableKind.Continuous),
                new DecisionVariable("TP",DecisionVariableKind.Continuous),
                new DecisionVariable("ALB",DecisionVariableKind.Continuous),
                new DecisionVariable("AGR",DecisionVariableKind.Continuous)
            };

            tree = new DecisionTree(var,2);

            C45Learning teacher = new C45Learning(tree);
            teacher.Learn(inputs, outputs);
            var learner = new IterativeReweightedLeastSquares<LogisticRegression>()
            {
                Tolerance = 1e-6,  // Let's set some convergence parameters
                MaxIterations = 1000,  // maximum number of iterations to perform
                Regularization = 0
            };
            regression = learner.Learn(inputs, outputs);

        }


        public double regressionProbability()
        {
            double probablity = regression.Probability(new double[] { lam.Age,lam.Gender,lam.Total_Bilirubin,lam.Direct_Bilirubin
                                                                      ,lam.Alkaline_Phosphotase,lam.Alamine_Aminotransferase
                                                                      ,lam.Aspartate_Aminotransferase,lam.Total_Protiens
                                                                      ,lam.Albumin,lam.Albumin_and_Globulin_Ratio });
           
            return probablity;
        }
        public double treeDeciding()
        {

            double val= tree.Decide(new double[] { lam.Age,lam.Gender,lam.Total_Bilirubin,lam.Direct_Bilirubin
                                                                      ,lam.Alkaline_Phosphotase,lam.Alamine_Aminotransferase
                                                                      ,lam.Aspartate_Aminotransferase,lam.Total_Protiens
                                                                      ,lam.Albumin,lam.Albumin_and_Globulin_Ratio });
            return val;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMatrix;
using SwarmOps;
using SwarmOps.Optimizers;
using SwarmOps.Problems;

namespace NmfAnnClassifier
{
    class Program
    {
        static void Main(string[] args)
        {
            //Execute and create the non-negative matrix factorized values from the unlabeled source data
            //TODO: Replace testing matrix
            const int packedWRows = 3;
            const int packedHRows = 2;
            var packedKnownW = new[] { 1.0, 3.0, 2.0, 5.0, 1.0, 7.0 };
            var packedKnownH = new[] { 3.0, 8.0, 1.0, 3.0, 4.0, 7.0 };
            var w = new GeneralMatrix(packedKnownW, packedWRows);
            var h = new GeneralMatrix(packedKnownH, packedHRows);
            var v = w * h;

            //Get the matrix / array to factor
            
            //Create the problem from the matrix
            var problem = new NonNegativeMatrixFactorization(packedWRows, packedHRows, v.ColumnPackedCopy,false);
            
            //Specify stoping conditions
            int numIterations = 1000000;
            IRunCondition runCondition = new RunConditionFitness(numIterations, problem.AcceptableFitness);
            problem.RunCondition = runCondition;
            
            //Get the optimizer
            SPSO optimizer = new SPSO();
            double[] parameters = optimizer.DefaultParameters;
            
            //Set the optimizer problem
            optimizer.Problem = problem;
            
            //Optimize / Get coffee
            Result result = optimizer.Optimize(parameters);
            var best = result.Parameters;

            Console.WriteLine("Fitness: {0}",result.Fitness);
            Console.Write("Result: ");
            foreach (var val in best)
            {
                Console.Write("{0} ", val);
            }
            Console.ReadKey();
        }
    }
}

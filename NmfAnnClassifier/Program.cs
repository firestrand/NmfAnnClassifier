using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
            const int packedWRows = 1875;
            const int packedHRows = 81;
            //var packedKnownW = new[] { 1.0, 3.0, 2.0, 5.0, 1.0, 7.0 };
            //var packedKnownH = new[] { 3.0, 8.0, 1.0, 3.0, 4.0, 7.0 };
            //var w = new GeneralMatrix(packedKnownW, packedWRows);
            //var h = new GeneralMatrix(packedKnownH, packedHRows);
            //var v = w * h;

            var list = new List<double>(packedWRows * 10000);
            var rows = 0;
            //Get the matrix / array to factor
            using(var reader = File.OpenText("test.csv"))
            {
                string s;
                //Skip first line
                reader.ReadLine();
                while((s = reader.ReadLine()) != null)
                {
                    //Read into memory
                    list.AddRange(s.Split(new[] {','}).Select(val=>Double.Parse(val)));
                    rows++;
                }
            }
            //var matrix = new GeneralMatrix(list.ToArray(), rows);
            var v = list.ToArray();
            //Scale each value from -2,2 to 0, 4

            //Create the problem from the matrix
            var problem = new MatrixFactorization(packedWRows, packedHRows, v ,-1.42,1.42);
            
            //Specify stoping conditions
            int numIterations = 30000;
            IRunCondition runCondition = new RunConditionFitness(numIterations, problem.AcceptableFitness);
            problem.RunCondition = runCondition;
            
            //Get the optimizer
            SPSO optimizer = new SPSO();
            double[] parameters = optimizer.DefaultParameters;
            
            //Set the optimizer problem
            optimizer.Problem = problem;

            var sw = new Stopwatch();
            Console.WriteLine("Starting Factorization");
            sw.Start();
            //Optimize / Get coffee
            Result result = optimizer.Optimize(parameters);
            var best = result.Parameters;
            sw.Stop();
            Console.WriteLine("Elapsed: {0}", sw.ElapsedMilliseconds / 1000);
            Console.WriteLine("Fitness: {0}",result.Fitness);
            Console.Write("Result: ");
            File.WriteAllText("result.csv", String.Join(",",best.Select(dv=>dv.ToString(CultureInfo.InvariantCulture))));
                
            Console.WriteLine("ALL DONE!");
            Console.ReadKey();
        }
    }
}

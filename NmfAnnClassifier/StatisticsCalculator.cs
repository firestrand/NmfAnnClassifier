using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NmfAnnClassifier
{
    public class StatisticsCalculator
    {
        public double[] CalculateStdDeviation(string filename)
        {
            int length = 0;
            int count = 0;
            //Calculate mean of each dimension
            using (var sr = new StreamReader(filename))
            {
                string line;
                double[] mean = null;
                while ((line = sr.ReadLine()) != null)
                {
                    var temp = ParseLine(line);
                    if(mean == null)
                    {
                        length = temp.Length;
                        mean = Enumerable.Repeat(0.0d, length).ToArray();
                    }
                    for (int i = 0; i < length; i++)
                    {
                        mean[i] += temp[i];
                    }    
                    count++;
                }
                for (int i = 0; i < length; i++)
                {
                    mean[i] /= count;
                } 
            }

            //Load the file and create a running tally
            using (var sr = new StreamReader(filename))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
            throw new NotImplementedException();
        }
        public double[] ParseLine(string line)
        {
            var lineValues = line.Split(new[] {','});
            var result = new double[lineValues.Length];
            for (int i = 0; i < lineValues.Length; i++)
            {
                result[i] = Double.Parse(lineValues[i]);
            }
            return result;
        }
    }
}

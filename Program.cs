using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace PrelucrareTraces
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            string final_line;
            string []frame;
            Dictionary<string, double> previousTimeStampById = new Dictionary<string, double>();

            double current_timeStamp = 0.0;
            double deltaT = 0.0;

            double cycle_time_10 = 10.0;
            double cycle_time_20 = 20.0;
            string inputFile = @"D:\Master\an2\disertatie\Traces\Fuzzy + Multiplication\mult25%EEC1_fuzzy100%TCO1.asc";
            string outputFile = @"D:\Master\an2\disertatie\Traces\Fuzzy + Multiplication\mult25%EEC1_fuzzy100%TCO1_Inputs.asc";

            try
            {
                StreamReader sr = new StreamReader(inputFile); // create a StreamRead for Input File(CANoe)
                StreamWriter wrinputs = new StreamWriter(outputFile); // create a StreamWriter for Output File(python input)
                line = sr.ReadLine(); //read line
                while (line != null) // while line is not null
                {
                    int count = line.Split(',').Length - 1; // line.Split(',') returns an array of comma-separated strings; number of elements -> (.length)  
                    if (count > 3) 
                    {
                        frame = line.Split(',');
                        frame[0]=frame[0].Replace('/', ' '); //for each line, "/" character is replaced with space
                 
                        string messageId = frame[1].Trim();

                        current_timeStamp = Convert.ToDouble(frame[0], CultureInfo.InvariantCulture);

                        if (!previousTimeStampById.ContainsKey(messageId))
                        {
                            if (messageId == "218000622")
                                deltaT = cycle_time_20;
                            else if (messageId == "217056256")
                                deltaT = cycle_time_10;
                            else
                                deltaT = 0.0;
                        }
                        else
                        {
                            deltaT = (current_timeStamp - previousTimeStampById[messageId]) * 1000;
                        }

                        if (frame[frame.Length - 1].Trim() == "0")
                        {
                            previousTimeStampById[messageId] = current_timeStamp;
                        }

                        final_line = deltaT.ToString("0.00", CultureInfo.InvariantCulture) + ",";
                        
                        //final_line = "";

                        for (int i = 1; i < frame.Length; i++)
                        {
                            if (i == frame.Length - 1)
                                final_line = final_line + frame[i];
                            else
                                final_line = final_line + frame[i] + ",";
                        }

                        wrinputs.WriteLine(final_line); // write the final line in the output file
                        final_line = ""; //resets the string
                   }
                    line = sr.ReadLine(); // read the next line
                }
                wrinputs.Close(); // close the file 
            }

            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");

            }
        }
    }
}



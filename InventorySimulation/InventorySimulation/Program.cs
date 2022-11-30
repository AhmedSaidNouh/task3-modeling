using InventoryModels;
using InventoryTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventorySimulation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            SimulationSystem sim = new SimulationSystem();
            string path = "../../TestCases/TestCase4.txt";
            sim.readFromFile(path, sim);
           // string test= TestingManager.Test(sim, Constants.FileNames.TestCase1);
           
           /* Console.WriteLine(sim.OrderUpTo);
            Console.WriteLine(sim.ReviewPeriod);
            Console.WriteLine(sim.StartInventoryQuantity);
            Console.WriteLine(sim.StartLeadDays);
            Console.WriteLine(sim.StartOrderQuantity);
            Console.WriteLine(sim.NumberOfDays);*/
            foreach (Distribution row in sim.LeadDaysDistribution)
            {
                
                Console.WriteLine(row.Value + ","+row.Probability+","+row.CummProbability +","+row.MinRange+','+row.MaxRange);
            }
        }
    }
}

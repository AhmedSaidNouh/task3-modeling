using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryModels
{
    public class SimulationSystem
    {
        public SimulationSystem()
        {
            DemandDistribution = new List<Distribution>();
            LeadDaysDistribution = new List<Distribution>();
            SimulationTable = new List<SimulationCase>();
            PerformanceMeasures = new PerformanceMeasures();
        }

        ///////////// INPUTS /////////////
        /// <summary>
        /// 
        /// </summary>
        public void readFromFile(string filePath, SimulationSystem sumlation)
        {

            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            StreamReader reader = new StreamReader(fileStream);
            while (reader.Peek() != -1)
            {
                string lineRead = reader.ReadLine();
                if (lineRead == "OrderUpTo")
                {
                    OrderUpTo= int.Parse(reader.ReadLine());
                }
                else if (lineRead == "ReviewPeriod")
                {
                    ReviewPeriod = int.Parse(reader.ReadLine());
                }
                else if (lineRead == "StartInventoryQuantity")
                {
                    StartInventoryQuantity = int.Parse(reader.ReadLine());
                }
                else if (lineRead == "StartOrderQuantity")
                {

                    StartOrderQuantity = int.Parse(reader.ReadLine());
                }
                else if (lineRead == "StartLeadDays")
                {

                    StartLeadDays = int.Parse(reader.ReadLine());
                }
                else if (lineRead == "NumberOfDays")
                {
                    NumberOfDays = int.Parse(reader.ReadLine());
                }
                else if (lineRead == "DemandDistribution")
                {
                    
                    while (true)
                    {
                        string intervalLine = reader.ReadLine();
                        if (intervalLine == "")
                        {
                            break;
                        }
                        string[] interval_Prob = intervalLine.Split(',');
                            Distribution row = new Distribution();
                            row.Probability = decimal.Parse(interval_Prob[1]);
                        row.Value= int.Parse(interval_Prob[0]);
                       DemandDistribution.Add(row);
                 
                    }
                    calculateCummProb_demaind();
                }
                else if (lineRead == "LeadDaysDistribution")
                {
                    while (true)
                    {
                        string intervalLine = reader.ReadLine();
                        if (intervalLine == null)
                        {
                            break;
                        }
                        string[] interval_Prob = intervalLine.Split(',');

                        Distribution row = new Distribution();
                        row.Probability = decimal.Parse(interval_Prob[1]);
                        row.Value = int.Parse(interval_Prob[0]);
                        LeadDaysDistribution.Add(row);

                    }
                    calculateCummProb_lead();
                }
                //test randoms

            }
            fileStream.Close();
        }
        private void calculateCummProb_demaind()
        {
            decimal sumCProb = 0;
            foreach (Distribution Row in DemandDistribution)
            {
                if (sumCProb * 100 == 100||Row.Probability==0)
                {
                    Row.MinRange = 0;
                    Row.MaxRange = 0;
                }
                else
                {
                   
                    Row.MinRange = (int)(sumCProb * 100) + 1;
                    sumCProb += Row.Probability;
                    Row.CummProbability = sumCProb;
                    Row.MaxRange = (int)(sumCProb * 100);
                }
            }
        }

        private void calculateCummProb_lead()
        {
            decimal sumCProb = 0;
            foreach (Distribution Row in LeadDaysDistribution)
            {
                if (sumCProb * 10 == 10 || Row.Probability == 0)
                {
                    Row.MinRange = 0;
                    Row.MaxRange = 0;
                }
                else
                {
                    
                    Row.MinRange = (int)(sumCProb * 10) + 1;
                    sumCProb += Row.Probability; 
                    Row.CummProbability = sumCProb;
                    Row.MaxRange = (int)(sumCProb * 10);
                    if (Row.MaxRange < Row.MinRange)
                    {
                        Row.MinRange = 0;
                        Row.MaxRange = 0;
                    }
                }
            }
        }
        public int OrderUpTo { get; set; }
        public int ReviewPeriod { get; set; }
        public int NumberOfDays { get; set; }
        public int StartInventoryQuantity { get; set; }
        public int StartLeadDays { get; set; }
        public int StartOrderQuantity { get; set; }
        public List<Distribution> DemandDistribution { get; set; }
        public List<Distribution> LeadDaysDistribution { get; set; }

        ///////////// OUTPUTS /////////////

        public List<SimulationCase> SimulationTable { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }
    }
}

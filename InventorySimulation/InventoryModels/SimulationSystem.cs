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
        /// </summary>read file
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
        //demaind
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
        //leadday
        private void calculateCummProb_lead()
        {
            decimal sumCProb = 0;
            foreach (Distribution Row in LeadDaysDistribution)
            {
                if (sumCProb * 100 == 100 || Row.Probability == 0)
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
                    /*if (Row.MaxRange < Row.MinRange)
                    {
                        Row.MinRange = 0;
                        Row.MaxRange = 0;
                    }*/
                }
            }
        }

        //table

        public void calculateSimulationTable()
        {
            Random rd = new Random();
            var randForDemands = new List<int>();
            //var randForlead = new List<int>();
            //create random values
            for (int i = 0; i < NumberOfDays; i++)
            {
                randForDemands.Add(rd.Next(1, 100));
              
            }
            //values in demain and lead random and cycle
            SimulationCase raw = new SimulationCase();
            int cycle = 1;
            int day_in_cycle = 1;
            int arrivale = StartLeadDays;
            int storge = 0;
            int order = 0;

            for (int i = 0; i < NumberOfDays; i++)
            {
                //demain & random demain & day &  cycle & cycle with day &
                if(i!=0&&i% ReviewPeriod == 0)
                {
                    cycle++;
                }
               
                raw = new SimulationCase();
                raw.Day = i + 1;
                //raw.RandomLeadDays= randForlead[i];
                raw.RandomDemand = randForDemands[i];
                raw.Cycle = cycle;
                raw.DayWithinCycle = day_in_cycle;
                var demand_val= DemandDistribution.Find(x => (randForDemands[i] <= x.MaxRange) && (randForDemands[i] >= x.MinRange));
                raw.Demand = demand_val.Value;
                day_in_cycle++;
                if (ReviewPeriod < day_in_cycle)
                {
                    day_in_cycle = 1;
                }


                if (i == 0)
                {
                    raw.BeginningInventory = StartInventoryQuantity;
                    if (demand_val.Value > raw.BeginningInventory)
                    {
                        storge += demand_val.Value - raw.BeginningInventory;

                        raw.EndingInventory = 0;

                    }
                    else

                    raw.EndingInventory = StartInventoryQuantity - demand_val.Value;
                   

                   


                }
                else
                {
                    if (arrivale == 0 && cycle == 1)
                    {
                        raw.BeginningInventory = SimulationTable[i - 1].EndingInventory + StartOrderQuantity;
                        int test = raw.BeginningInventory - demand_val.Value - storge;
                        // raw.EndingInventory = test;
                         storge = 0;
                        if (test >= 0)
                        {
                            raw.EndingInventory = test;
                            storge = 0;


                        }
                        else
                        {
                            storge += (test * -1);

                            raw.EndingInventory = 0;
                        }

                    }
                    else if (arrivale == 0)
                    {
                        
                        raw.BeginningInventory = SimulationTable[i - 1].EndingInventory + order;
                        int test = raw.BeginningInventory - demand_val.Value-storge;
                       storge = 0;
                        if (test>=0)
                        {
                            raw.EndingInventory = test;
                            storge = 0;
                          

                        }
                        else
                        {
                            storge += (test*-1);
                           
                            raw.EndingInventory = 0;
                        }
                        
                    }
                    else
                    {
                        raw.BeginningInventory = SimulationTable[i-1].EndingInventory;
                        if (demand_val.Value > raw.BeginningInventory)
                        {
                            storge += demand_val.Value - raw.BeginningInventory;

                            raw.EndingInventory = 0;

                        }
                        else
                        {


                                raw.EndingInventory = raw.BeginningInventory - demand_val.Value;

                        }
                    }
                 

                }
                if (arrivale == 0)
                {
                    arrivale = -1;
                }
                else
                {
                    arrivale--;
                }
                if((i+1)% ReviewPeriod == 0)
                {
                    
                    raw.RandomLeadDays = rd.Next(1, 100);       
                    var lead_val = LeadDaysDistribution.Find(x => (raw.RandomLeadDays <= x.MaxRange) && (raw.RandomLeadDays >= x.MinRange));
                    raw.LeadDays = lead_val.Value;
                    arrivale = lead_val.Value;
                    raw.OrderQuantity = OrderUpTo - raw.EndingInventory + storge;
                    order = raw.OrderQuantity;

                }
                raw.ShortageQuantity = storge;
                SimulationTable.Add(raw);
               

            }

        }
        public void calulate_proformance()
        {
            decimal EndingInventoryAverage = 0;
            decimal ShortageQuantityAverage = 0;
            foreach (SimulationCase row in SimulationTable)
            {
                EndingInventoryAverage += row.EndingInventory;
                ShortageQuantityAverage += row.ShortageQuantity;
            }
          
            EndingInventoryAverage = EndingInventoryAverage / NumberOfDays;

            ShortageQuantityAverage = ShortageQuantityAverage / NumberOfDays;
            PerformanceMeasures.EndingInventoryAverage = EndingInventoryAverage;
            PerformanceMeasures.ShortageQuantityAverage = ShortageQuantityAverage;

        }

        //variable
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

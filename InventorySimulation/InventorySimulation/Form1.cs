using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InventoryModels;
using InventoryTesting;

namespace InventorySimulation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void run_Click(object sender, EventArgs e)
        {
            
        }

        private void test_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void run_Click_1(object sender, EventArgs e)
        {
            Console.WriteLine("sss");
            if (test.SelectedItem != null)
            {
                SimulationSystem simulation = new SimulationSystem();
                string path = "../../TestCases/";
                string Test = test.SelectedItem.ToString();
                string final_path = path + Test;
                simulation.readFromFile(final_path, simulation);
                simulation.calculateSimulationTable();
                simulation.calulate_proformance();
                string x = null;
                switch (Test)
                {
                    case "TestCase1.txt":
                        x = TestingManager.Test(simulation, Constants.FileNames.TestCase1);
                        break;
                    case "TestCase2.txt":
                        x = TestingManager.Test(simulation, Constants.FileNames.TestCase2);
                        break;
                    case "TestCase3.txt":
                        x = TestingManager.Test(simulation, Constants.FileNames.TestCase3);
                        break;
                    case "TestCase4.txt":
                        x = TestingManager.Test(simulation, Constants.FileNames.TestCase4);
                        break;

                }

                MessageBox.Show(x);
                 Form2 graphForm = new Form2(simulation);
                graphForm.Show();

            }
            else
                MessageBox.Show("chooce test");


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

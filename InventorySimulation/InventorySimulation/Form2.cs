using InventoryModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventorySimulation
{
    public partial class Form2 : Form
    {
        SimulationSystem sim;
        public Form2(SimulationSystem simulation)
        {
            InitializeComponent();
            sim = simulation;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            var binding = new BindingList<SimulationCase>(sim.SimulationTable);
            var src = new BindingSource(binding, null);
            dataGridView1.DataSource = src;
            textBox1.Text = sim.PerformanceMeasures.EndingInventoryAverage.ToString();
            textBox2.Text = sim.PerformanceMeasures.ShortageQuantityAverage.ToString();
           
        }
    }
}

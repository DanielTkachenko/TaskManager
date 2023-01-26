using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using NLog;

namespace TaskManager_test
{
    public partial class Form2 : Form
    {
        #region class fields and properties

        /// <summary>
        /// keeps information about one process
        /// </summary>
        public Process process { get; set; }

        /// <summary>
        /// logs information
        /// </summary>
        Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region class constructors
        public Form2()
        {
            InitializeComponent();
            logger.Info($"Inforamtion form was initialized");
        }
        #endregion

        #region Windows Forms event handlers

        /// <summary>
        /// Used when the form is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_Load(object sender, EventArgs e)
        {
            if (process != null)
            {
                dataGridView1.Rows.Add("Name", process.ProcessName);
                dataGridView1.Rows.Add("Id", process.Id);
                dataGridView1.Rows.Add("Physical memory", process.WorkingSet64 / 1000000.0 + " Мб");
                dataGridView1.Rows.Add("Paged system memory size", process.PagedSystemMemorySize64 / 1000000.0 + " Мб");
                dataGridView1.Rows.Add("Paged memory size", process.PagedMemorySize64 / 1000000.0 + " Мб");
                dataGridView1.Rows.Add("Threads", process.Threads.Count);
                dataGridView1.Rows.Add("Status", process.Responding ? "Running" : "Not responding");

                logger.Info($"Inforamtion form for process \'{process.ProcessName}\' was loaded");
            }
            else
            {
                logger.Error($"Process property was not set");
            }
        }

        /// <summary>
        /// closes the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            logger.Info($"Inforamtion form for process \'{process.ProcessName}\' was closed");
        }
        #endregion
    }
}

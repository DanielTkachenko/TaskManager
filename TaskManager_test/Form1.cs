using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using NLog;

namespace TaskManager_test
{
    public partial class Form1 : Form
    {
        #region class fields and properties
        /// <summary>
        /// keeps information about received processes
        /// </summary>
        List<Process> processes = new List<Process>();
        List<Process> processes_copy = new List<Process>();

        /// <summary>
        /// logs information
        /// </summary>
        Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region class constructors
        public Form1()
        {
            InitializeComponent();
            logger.Info("Main form was initialized");
        }
        #endregion

        #region Windows Forms event handlers

        /// <summary>
        /// Used when the form is loaded 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            processes = Process.GetProcesses().ToList<Process>();
            RefreshData();
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            logger.Info("Main form was loaded");
            Task.Run(
                () =>
                {
                    lock (processes)
                    {
                        processes.Clear();
                        processes = Process.GetProcesses().ToList<Process>();
                    }
                    Task.Delay(1000);
                });
        }

        /// <summary>
        /// Updates the list of processes and datagridview elements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshData();
            logger.Info("The data about processes was refreshed");
        }

        /// <summary>
        /// Shows the form with additional information about chosen process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form2 infoForm = new Form2();
            Process _process = processes[dataGridView1.SelectedRows[0].Index];
            if(_process != null)
            {
                infoForm.process = _process;
                infoForm.Show();
                logger.Info($"Information form for process \'{_process.ProcessName}\' was shown");
            }
            else
            {
                logger.Error("Process was not found");
            }
            
        }

        /// <summary>
        /// Selects clicked row in table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Selected = true;
        }
        #endregion

        #region User methods
        /// <summary>
        /// Used for updating information about processes
        /// </summary>
        private void RefreshData()
        {
            dataGridView1.Rows.Clear();
            foreach (Process item in processes)
            {
                dataGridView1.Rows.Add(item.ProcessName, item.Id, item.WorkingSet64 / 1000000.0 + " Мб");
            }
            toolStripLabel2.Text = processes.Count.ToString();
        }
        #endregion
    }
}

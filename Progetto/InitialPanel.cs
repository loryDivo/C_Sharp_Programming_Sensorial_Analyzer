using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Progetto
{
    public partial class InitialPanel : Form
    {
        private String selectedFrequency;

        public InitialPanel()
        {
            InitializeComponent();
            freqeuncies.SelectedIndex = 0;
            freqeuncies.DropDownStyle = ComboBoxStyle.DropDownList;
            selectedFrequency = freqeuncies.SelectedItem as String;
            //Some stuff is set in InitialPanel.Designer.cs
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (selectedFrequency.Equals("50 Hz"))
            {
                Program.Settings.Frequency = 50;
            }
            else if (selectedFrequency.Equals("100 Hz"))
            {
                Program.Settings.Frequency = 100;
            }
            else if (selectedFrequency.Equals("200 Hz"))
            {
                Program.Settings.Frequency = 200;
            }

            this.Dispose();
        }

        private void info_Click(object sender, EventArgs e)
        {

        }

        private void InitPanel_Load(object sender, EventArgs e)
        {

        }

        private void freqeuncies_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            selectedFrequency = comboBox.SelectedItem as String;
        }

        private void InitPanel_Closing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.Cancel)
            {
                if (MessageBox.Show("Vuoi veramente uscire dal programma?", "Chiusura programma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Program.CloseApplication();
                }
            }
        }
    }
}

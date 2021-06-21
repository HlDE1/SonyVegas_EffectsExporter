using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SonyVegas_EffectsExporter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (radioButton3.Checked == true)
            {
                Effects.GetNewBlue(listView1, label1, progressBar1);
            }
            else if(radioButton5.Checked == true)
            {
                Effects.GetSony(listView1);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listView1.SelectedItems[0].Text);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            if (radioButton3.Checked == true)
            {
                Effects.GetNewBluePresetName(listView1, listView2);
            }
            else if (radioButton5.Checked == true)
            {
                Effects.GetSonyPresetName(listView1, listView2);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int selectedIndex = 0;
            if (listView1.SelectedIndices.Count > 0)
            {
                selectedIndex = listView1.SelectedIndices[0];
                MessageBox.Show(listView1.SelectedItems[0].Text);
                Effects.ExportReg(@"HKEY_USERS\S-1-5-21-2384987514-954954182-3699566690-1001\SOFTWARE\DXTransform\Presets\{" + Effects.Effect_CodeName[selectedIndex] + "}", listView1.SelectedItems[0].Text.Replace(" ", "") + "_Preset");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            Effects.ExportReg(@"HKEY_USERS\S-1-5-21-2384987514-954954182-3699566690-1001\SOFTWARE\DXTransform\", "All_Effect_Presets");
        }
    }
}

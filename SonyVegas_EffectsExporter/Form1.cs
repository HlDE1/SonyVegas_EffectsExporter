using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;


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
            if (radioButton3.Checked == true)//NewBlue
            {
                Effects.GetNewBlue(listView1, label1, progressBar1);
            }
            else if (radioButton5.Checked == true)//Sony
            {
                Effects.GetSony(listView1);
            }
            else if (radioButton7.Checked == true)//Render Settings
            {
                Effects.GetRenderSettings(listView1, listView2);
            }
            else if (radioButton1.Checked == true)//Sapphire
            {
                Effects.GetSapphire(listView1);

            }
            else if (radioButton2.Checked == true)//Universe
            {
                Effects.GetUniverse(listView1);
            }
            else if (radioButton4.Checked == true)//BCC
            {
                Effects.GetBCC(listView1);
            }
            else
            {
                MessageBox.Show("Please select something", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listView1.SelectedItems[0].Text);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked != true)
                listView2.Items.Clear();

            if (radioButton3.Checked == true)//NewBlue
            {
                Effects.GetNewBluePresetName(listView1, listView2);
            }
            else if (radioButton5.Checked == true)//Sony
            {
                Effects.GetSonyPresetName(listView1, listView2);
            }
            else if (radioButton1.Checked == true)//Sapphire
            {
                Effects.GetSapphirePresetName(listView1, listView2);
            }
            else if (radioButton2.Checked == true)//Universe
            {
                Effects.GetUniversePresetName(listView1, listView2);
            }
            else if (radioButton4.Checked == true)//BCC
            {
                Effects.GetBCCPresetName(listView1, listView2);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int selectedIndex = 0;
            if (listView1.SelectedIndices.Count > 0)
            {
                if (radioButton3.Checked == true)//NewBlue
                {
                    selectedIndex = listView1.SelectedIndices[0];
                    Effects.ExportReg(@"HKEY_USERS\S-1-5-21-2384987514-954954182-3699566690-1001\SOFTWARE\DXTransform\Presets\{" + Effects.Effect_CodeName[selectedIndex] + "}", listView1.SelectedItems[0].Text.Replace(" ", "") + "_Preset");
                }
                else if (radioButton5.Checked == true || radioButton1.Checked == true || radioButton2.Checked == true || radioButton4.Checked == true)
                {
                    Effects.ExportXML(listView1.SelectedItems[0].Text);
                }
                Process.Start(Directory.GetCurrentDirectory());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Effects.ExportReg(@"HKEY_USERS\S-1-5-21-2384987514-954954182-3699566690-1001\SOFTWARE\DXTransform\", "All_Effect_Presets");
            Process.Start(Directory.GetCurrentDirectory());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ImageList imgs = new ImageList();
            imgs.ImageSize = new Size(20, 20);
            imgs.Images.Add(Resource1.notepad);
            listView2.SmallImageList = imgs;
            listView2.Items.Add("Test", 0);
        }
    }
}

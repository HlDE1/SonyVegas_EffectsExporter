using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace SonyVegas_EffectsExporter
{
    public partial class Importer : Form
    {
        public Importer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            string path = Directory.GetCurrentDirectory() + @"\";
            for (int i = 0; i < Directory.GetFiles(path).Length; i++)
            {
                if (!Path.GetFileName(Directory.GetFiles(path)[i]).Contains("exe"))
                {
                    listView1.Items.Add(Path.GetFileName(Directory.GetFiles(path)[i]));
                }
            }
            for (int i = 0; i < Directory.GetDirectories(path).Length; i++)
            {
                listView1.Items.Add(Path.GetFileName(Directory.GetDirectories(path)[i]));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string RenderSettingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Sony";
            string OFX_Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OFX Presets";


            if (listView1.SelectedItems[0].Text == "Render Templates")
            {
                Directory.Move("Render Templates", RenderSettingPath);
            }
            else if (listView1.SelectedItems[0].Text.Contains("com."))
            {
                Directory.Move(listView1.SelectedItems[0].Text, OFX_Path);
            }
            else if (listView1.SelectedItems[0].Text.Contains(".reg"))
            {
                Process.Start(listView1.SelectedItems[0].Text);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string RenderSettingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Sony";
            string OFX_Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OFX Presets";

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Text == "Render Templates")
                {
                    Directory.Move("Render Templates", RenderSettingPath);
                }
                else if (listView1.Items[i].Text.Contains("com."))
                {
                    Directory.Move(listView1.Items[i].Text, OFX_Path);
                }
                else if (listView1.Items[i].Text.Contains(".reg"))
                {
                    Process.Start(listView1.Items[i].Text);
                }
            }
        }
    }
}

﻿using System;
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
using System.Threading;
using Microsoft.Win32;

namespace SonyVegas_EffectsExporter
{
    public partial class Exporter : Form
    {
        public Exporter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView2.Items.Clear();
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
            else if (radioButton6.Checked == true)//Pancrop
            {
                Effects.GetPancrop(listView1);
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
            else if (radioButton6.Checked == true)//Pancrop
            {
                Effects.GetPancropName(listView1, listView2);
            }
        }

        void ExportNewBlue(int selectedIndex)
        {
            string fileName = listView1.SelectedItems[0].Text.Replace(" ", "") + "_Preset";
            if (File.Exists(fileName + ".reg"))
                File.Delete(fileName + ".reg");

            Effects.ExportReg(@"HKEY_USERS\S-1-5-21-2384987514-954954182-3699566690-1001\SOFTWARE\DXTransform\Presets\{" + Effects.Effect_CodeName[selectedIndex] + "}", fileName);
            Thread.Sleep(1000);
            if (listView2.CheckedItems.Count != listView2.Items.Count)
            {
                Effects.RegistryAddSpecificDataToList(fileName + ".reg");
                File.AppendAllText("temp.txt", Effects.NewBlueData[0] + "\n");
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    if (listView2.Items[i].Checked == true)
                    {
                        Effects.RegistryRemoveSpecificData(listView2.Items[i].Text, fileName + ".reg");
                    }
                }
                File.Delete(fileName + ".reg");
                File.Move("temp.txt", fileName + ".reg");
                Effects.NewBlueData.Clear();
            }
        }

        void ExportPancrop(int selectedIndex)
        {
            string fileName = listView1.SelectedItems[0].Text.Replace(" ", "") + "_Preset";
            if (File.Exists(fileName + ".reg"))
                File.Delete(fileName + ".reg");

            Effects.ExportReg(@"HKEY_CURRENT_USER\SOFTWARE\Sony Creative Software\Vegas Pro\13.0\Metrics\Application", fileName);

            Thread.Sleep(1000);
            if (listView2.CheckedItems.Count != listView2.Items.Count)
            {
                Effects.RegistryAddSpecificDataToList(fileName + ".reg");
                File.AppendAllText("temp.txt", Effects.NewBlueData[0] + "\n");
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    if (listView2.Items[i].Checked == true)
                    {
                        Effects.RegistryRemoveSpecificData(listView2.Items[i].Text, fileName + ".reg");
                    }
                }
                File.Delete(fileName + ".reg");
                File.Move("temp.txt", fileName + ".reg");
                Effects.NewBlueData.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string OFX_Presets_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OFX Presets/";
            string AppDataRender_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sony\Render Templates";
            string FavouriteRenderSettings_Path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sony\Render Templates\avc-mc";

            int selectedIndex = 0;
            if (listView1.SelectedIndices.Count > 0)
            {
                if (listView2.CheckedItems.Count > 0)
                {
                    selectedIndex = listView1.SelectedIndices[0];

                    if (radioButton3.Checked == true)//NewBlue
                    {
                        ExportNewBlue(selectedIndex);
                    }
                    else if (radioButton5.Checked == true || radioButton1.Checked == true || radioButton2.Checked == true || radioButton4.Checked == true)
                    {
                        for (int i = 0; i < listView2.Items.Count; i++)
                        {
                            if (listView2.Items[i].Checked == true)
                            {
                                Effects.ExportXML(OFX_Presets_path, listView1.SelectedItems[0].Text, i);
                            }
                        }
                    }
                    else if (radioButton7.Checked == true)
                    {
                        for (int i = 0; i < listView2.Items.Count; i++)
                        {
                            if (listView2.Items[i].Checked == true)
                            {
                                Effects.ExportFavoriteRender(FavouriteRenderSettings_Path, listView2.Items[i].Text, i);
                            }
                        }
                    }
                    else if (radioButton6.Checked == true)
                    {
                        ExportPancrop(selectedIndex);
                    }

                    //Process.Start(Directory.GetCurrentDirectory());
                }
                else
                {
                    MessageBox.Show("Please Select A Preset", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select something", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string OFX_Presets_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OFX Presets/";

            if (radioButton3.Checked == true)//NewBlue
            {
                Effects.ExportReg(@"HKEY_USERS\S-1-5-21-2384987514-954954182-3699566690-1001\SOFTWARE\DXTransform\", "All_Effect_Presets");
            }
            else if (radioButton6.Checked == true) // Pancrop
            {
                Effects.ExportReg(@"HKEY_CURRENT_USER\SOFTWARE\Sony Creative Software\Vegas Pro\13.0\Metrics\Application\", "All_Pancrop_Presets");
            }
            else if (radioButton5.Checked == true || radioButton1.Checked == true || radioButton2.Checked == true || radioButton4.Checked == true)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    for (int j = 0; j < listView2.Items.Count; j++)
                    {
                        Effects.ExportXML(OFX_Presets_path, listView1.Items[i].Text, j);
                    }
                }
            }
            else if (radioButton7.Checked == true)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    //Effects.ExportXML("", listView1.Items[i].Text);
                }
            }
            Process.Start(Directory.GetCurrentDirectory());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //string effect = @"SOFTWARE\Sony Creative Software\Vegas Pro\13.0\Metrics\Application";
            //var effect_key = Registry.CurrentUser.OpenSubKey(effect);
            //var effect_subKeys = effect_key.GetSubKeyNames();
            /*ImageList imgs = new ImageList();
            imgs.ImageSize = new Size(20, 20);
            imgs.Images.Add(Resource1.notepad);
            listView2.SmallImageList = imgs;
            listView2.Items.Add("Test", 0);*/
            //Effects.RegistryRemoveSpecificData("");
            /*  for (int i = 0; i < effect_key.GetValueNames().Length; i++)
              {
                  if (effect_key.GetValueNames()[i].Contains("S44"))
                      listView2.Items.Add(effect_key.GetValue(effect_key.GetValueNames()[i]).ToString());
              }*/

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox1.Text = "Uncheck All";
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    if (!listView2.Items[i].Checked)
                        listView2.Items[i].Checked = true;
                }
            }
            else
            {
                checkBox1.Text = "Check All";
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    if (listView2.Items[i].Checked)
                        listView2.Items[i].Checked = false;
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox2.Text = "Uncheck All";
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (!listView1.Items[i].Checked)
                        listView1.Items[i].Checked = true;
                }
            }
            else
            {
                checkBox2.Text = "Check All";
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].Checked)
                        listView1.Items[i].Checked = false;
                }
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

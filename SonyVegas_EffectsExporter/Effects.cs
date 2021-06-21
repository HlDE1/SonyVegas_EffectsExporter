﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace SonyVegas_EffectsExporter
{
    class Effects
    {
        public static List<string> Effect_CodeName = new List<string>();

        #region NewBlue
        public static void GetNewBlue(ListView listView, Label label, ProgressBar progressBar)
        {


            string effect = @"S-1-5-21-2384987514-954954182-3699566690-1001\SOFTWARE\DXTransform\Presets";
            var effect_key = Microsoft.Win32.Registry.Users.OpenSubKey(effect);
            var effect_subKeys = effect_key.GetSubKeyNames();


            //string effect_names = @"CLSID\{58fa9b1f-2c60-4cdf-b68f-1da10f18e199}";
            string effect_names = "";
            string effect_name_value = "";
            var effect_names_key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(effect_names);

            label.Text = effect_key.SubKeyCount.ToString();
            progressBar.Value = 0;
            progressBar.Maximum = effect_key.SubKeyCount;
            for (int i = 0; i < effect_key.SubKeyCount; i++)
            {

                var text = effect_subKeys[i].Replace("{", "").Replace("}", "");

                effect_names = @"CLSID\{" + text + "}";
                effect_names_key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(effect_names);
                effect_name_value = effect_names_key?.GetValue("").ToString();
                if (!string.IsNullOrEmpty(effect_name_value))
                {
                    listView.Items.Add(effect_name_value);
                    Effect_CodeName.Add(text.ToString());
                }
                progressBar.Value++;
            }

        }


        public static void GetNewBluePresetName(ListView listView1, ListView listView2)
        {
            try
            {

                int selectedIndex = 0;
                if (listView1.SelectedIndices.Count > 0)
                {
                    selectedIndex = listView1.SelectedIndices[0];

                    string effect = @"S-1-5-21-2384987514-954954182-3699566690-1001\SOFTWARE\DXTransform\Presets";
                    var effect_key = Microsoft.Win32.Registry.Users.OpenSubKey(effect);
                    var effect_subKeys = effect_key.GetSubKeyNames();
                    string effect_names = "";


                    var effect_names_key = Microsoft.Win32.Registry.Users.OpenSubKey(effect_names);
                    var text = Effects.Effect_CodeName[selectedIndex];
                    effect_names = @"S-1-5-21-2384987514-954954182-3699566690-1001\SOFTWARE\DXTransform\Presets\{" + text + "}";
                    effect_names_key = Microsoft.Win32.Registry.Users.OpenSubKey(effect_names);

                    for (int i = 0; i <= effect_names_key.ValueCount; i++)
                    {

                        listView2.Items.Add(effect_names_key.GetValueNames()[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString());
            }
        }


        public static void ExportReg(string path, string filename)
        {

            Process.Start("cmd.exe", $@"/c REG EXPORT {path} {filename}.reg");
        }


        #endregion


        public static void GetSony(ListView listView)
        {
            string document_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OFX Presets";
            string text = "";
            for (int i = 0; i < Directory.GetDirectories(document_path).Length; i++)
            {
                if (Directory.GetDirectories(document_path)[i].Contains("com.sonycreativesoftware"))
                {
                    text = Directory.GetDirectories(document_path)[i].Remove(0, 62);
                    listView.Items.Add(text);
                }
            }
        }

        public static void GetSonyPresetName(ListView listView1, ListView listView2)
        {
            try
            {
                string document_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OFX Presets/";
                string filter_path = "";
                int selectedIndex = 0;
                if (listView1.SelectedIndices.Count > 0)
                {
                    selectedIndex = listView1.SelectedIndices[0];

                    filter_path = document_path + "com.sonycreativesoftware_" + listView1.SelectedItems[0].Text + "/Filter/";
                    for (int i = 0; i < Directory.GetFiles(filter_path).Length; i++)
                    {
                        listView2.Items.Add(Directory.GetFiles(filter_path)[i].Replace(filter_path, ""));
                    }
                }
            }
            catch
            {

            }
            
        }
            
    }
}

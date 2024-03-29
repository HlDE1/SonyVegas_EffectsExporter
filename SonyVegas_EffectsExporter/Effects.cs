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

        public static List<string> NewBlueData = new List<string>();

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



        #endregion

        #region Sony
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
        #endregion


        #region RenderSettings


        public static void GetRenderSettings(ListView listView1, ListView listView2)
        {
            string AppData_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sony\Render Templates";
            string FavouriteRenderSettings_Path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sony\Render Templates\avc-mc";

            listView1.Items.Add(Path.GetFileName(Directory.GetFiles(AppData_path)[0]));
            for (int i = 0; i < Directory.GetFiles(FavouriteRenderSettings_Path).Length; i++)
                listView2.Items.Add(Path.GetFileName(Directory.GetFiles(FavouriteRenderSettings_Path)[i]));

            //MessageBox.Show(Directory.GetFiles(AppData_path)[0]);
        }

        #endregion

        #region Pancrop
        /// S44_$s = Pancrop 
        /// S46_%s = Mask
        /// S48_%s = Trackanimation
        /// HKEY_CURRENT_USER\SOFTWARE\Sony Creative Software\Vegas Pro\13.0\Metrics\Application
        public static List<string> PancropHexCode = new List<string>();
        public static List<string> MaskHexCode = new List<string>();
        public static List<string> TrackAnimationHexCode = new List<string>();

        public static void GetPancrop(ListView listView)
        {


            string effect = @"SOFTWARE\Sony Creative Software\Vegas Pro\13.0\Metrics\Application";
            var effect_key = Registry.CurrentUser.OpenSubKey(effect);
            var effect_subKeys = effect_key.GetSubKeyNames();
            listView.Items.Add("Pancrop");
            listView.Items.Add("Mask");
            listView.Items.Add("Trackanimation");
        }

        public static void GetPancropName(ListView listView1, ListView listView2)
        {
            try
            {
                string effect = @"SOFTWARE\Sony Creative Software\Vegas Pro\13.0\Metrics\Application";
                var effect_key = Registry.CurrentUser.OpenSubKey(effect);
                var effect_subKeys = effect_key.GetSubKeyNames();

                if (listView1.SelectedItems[0].Text == "Pancrop")
                {
                    for (int i = 0; i < effect_key.GetValueNames().Length; i++)
                    {
                        if (effect_key.GetValueNames()[i].Contains("S44"))
                        {
                            listView2.Items.Add(effect_key.GetValue(effect_key.GetValueNames()[i]).ToString());
                            PancropHexCode.Add(effect_key.GetValueNames()[i]);
                        }
                    }
                }
                else if (listView1.SelectedItems[0].Text == "Mask")
                {
                    /*for (int i = 0; i < effect_key.GetValueNames().Length; i++)
                    {
                        if (effect_key.GetValueNames()[i].Contains("S46"))
                            listView2.Items.Add(effect_key.GetValue(effect_key.GetValueNames()[i]).ToString());
                    }*/
                }
                else if (listView1.SelectedItems[0].Text == "Trackanimation")
                {
                    /*for (int i = 0; i < effect_key.GetValueNames().Length; i++)
                    {
                        if (effect_key.GetValueNames()[i].Contains("S48"))
                            listView2.Items.Add(effect_key.GetValue(effect_key.GetValueNames()[i]).ToString());
                    }*/
                }
            }
            catch { }
        }

        public static void ExportPancrop(ListView listView2)
        {
            try
            {
                string effect = @"SOFTWARE\Sony Creative Software\Vegas Pro\13.0\Metrics\Application";
                var effect_key = Registry.CurrentUser.OpenSubKey(effect);
                var effect_subKeys = effect_key.GetSubKeyNames();

                string filename = "Pancrop.reg";
                string RegVersion = "Windows Registry Editor Version 5.00\n\n";
                string RegPath = @"[HKEY_CURRENT_USER\SOFTWARE\Sony Creative Software\Vegas Pro\13.0\Metrics\Application]";
                string fileContent = "";
                RegVersion += RegPath;
                RegVersion += "\n\n";
                File.WriteAllText(filename, RegVersion);

                string hexName = PancropHexCode[listView2.SelectedItems[0].Index].Replace('S', 'B');
                var hexCode = (byte[])effect_key.GetValue("B4400");
                string hexPrint = $"\"{hexName}\"" + "=hex:";
                string hexCodeStr = BitConverter.ToString(hexCode).Replace("-", ",").ToString();
                string test = "";

                MessageBox.Show(hexCodeStr.Length.ToString());
                //Clipboard.SetText(BitConverter.ToString(hexCode).Replace("-", ",").ToString());
                int t2 = 0;
                for (int i = 0; i < hexCodeStr.Length; i++)
                {
                    test += BitConverter.ToString(hexCode).Replace("-", ",")[i].ToString().ToLower();
                    if (i == 66 - 1)
                    {
                        test += @"\" + "\n";
                        t2++;
                    }
                    else if (75 - 2 + i == 216)
                    {
                        test += @"\" + "\n";
                        t2++;
                        MessageBox.Show(((hexCodeStr.Length - 66) % 75).ToString());
                    }
                    if (216 + ((hexCodeStr.Length - 66) % 75) == 251 && t2 == 2)
                    {
                        test += @"\" + "\n";
                        t2++;
                    }
                }

                // hexName += hexCode;
                hexPrint += test;
                File.AppendAllText(filename, hexPrint);

            }
            catch { }
        }

        #endregion


        //HKEY_CURRENT_USER\SOFTWARE\Sony Creative Software\Vegas Pro\13.0\Metrics\VideoProProfiles


        #region Sapphire
        public static void GetSapphire(ListView listView)
        {
            string document_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OFX Presets";
            string text = "";
            for (int i = 0; i < Directory.GetDirectories(document_path).Length; i++)
            {
                if (Directory.GetDirectories(document_path)[i].Contains("com.genarts.sapphire"))
                {
                    text = Path.GetFileName(Directory.GetDirectories(document_path)[i]).Replace("com.genarts.sapphire.", "");
                    listView.Items.Add(text);
                }
            }
        }

        public static void GetSapphirePresetName(ListView listView1, ListView listView2)
        {
            try
            {
                string document_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OFX Presets/";
                string filter_path = "";
                int selectedIndex = 0;
                if (listView1.SelectedIndices.Count > 0)
                {
                    selectedIndex = listView1.SelectedIndices[0];

                    filter_path = document_path + "com.genarts.sapphire." + listView1.SelectedItems[0].Text + "/Filter/";
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

        #endregion

        #region BCC

        public static void GetBCC(ListView listView)
        {
            string document_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OFX Presets";
            string text = "";
            for (int i = 0; i < Directory.GetDirectories(document_path).Length; i++)
            {
                if (Directory.GetDirectories(document_path)[i].Contains("com.borisfx"))
                {
                    text = Path.GetFileName(Directory.GetDirectories(document_path)[i]).Replace("com.borisfx_BCC3", "");
                    listView.Items.Add(text);
                }
            }
        }

        public static void GetBCCPresetName(ListView listView1, ListView listView2)
        {
            try
            {
                string document_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OFX Presets/";
                string filter_path = "";
                int selectedIndex = 0;
                if (listView1.SelectedIndices.Count > 0)
                {
                    selectedIndex = listView1.SelectedIndices[0];

                    filter_path = document_path + "com.borisfx_BCC3" + listView1.SelectedItems[0].Text + "/Filter/";
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

        #endregion

        #region Universe
        public static void GetUniverse(ListView listView)
        {
            string document_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OFX Presets";
            string text = "";
            for (int i = 0; i < Directory.GetDirectories(document_path).Length; i++)
            {
                if (Directory.GetDirectories(document_path)[i].Contains("com.redgiantsoftware.Universe_"))
                {
                    text = Path.GetFileName(Directory.GetDirectories(document_path)[i]).Replace("com.redgiantsoftware.Universe_", "");
                    listView.Items.Add(text);
                }
            }
        }

        public static void GetUniversePresetName(ListView listView1, ListView listView2)
        {
            try
            {
                string document_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OFX Presets/";
                string filter_path = "";
                int selectedIndex = 0;
                if (listView1.SelectedIndices.Count > 0)
                {
                    selectedIndex = listView1.SelectedIndices[0];

                    filter_path = document_path + "com.redgiantsoftware.Universe_" + listView1.SelectedItems[0].Text + "/Filter/";
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
        #endregion


        #region Export

        public static string ConvertStringToCmdString(string Text)
        {
            return Text.Replace(" ", "\" \"");
        }
        public static void ExportReg(string path, string filename)
        {

            Process.Start("cmd.exe", $@"/c REG EXPORT {ConvertStringToCmdString(path)} {filename}.reg");
        }

        public static void RegistryAddSpecificDataToList(string path)
        {
            try
            {
                string reg_file = File.ReadAllText(path);
                string[] reg_file_line = File.ReadAllLines(path);
                List<int> Lines = new List<int>();
                string text = "";
                for (int i = 0; i < reg_file.Split('\n').Length; i++)
                {
                    if (reg_file.Split('\n')[i].Contains("=hex:"))
                    {
                        Lines.Add(i);
                    }
                }
                Lines.Add(reg_file_line.Length);
                NewBlueData.Add(reg_file_line[0] + "\n" + reg_file_line[1] + "\n" + reg_file_line[2]);
                for (int a = 1; a < Lines.Count; a++)
                {
                    for (int i = Lines[a - 1]; i < Lines[a]; i++)
                    {
                        text += reg_file_line.Skip(i).Take(1).First() + "\n";

                    }
                    NewBlueData.Add(text);
                    text = "";
                }
            }
            catch (Exception e)
            {

            }
        }

        public static void RegistryRemoveSpecificData(string data, string fileName)
        {

            for (int i = 0; i < NewBlueData.Count; i++)
            {
                if (NewBlueData[i].Contains($"\"{data}\""))
                {
                    //File.AppendAllText(fileName, Data[i] + "\n");
                    File.AppendAllText("temp.txt", NewBlueData[i] + "\n");
                    //MessageBox.Show(NewBlueData[i]);
                }
            }
        }

        public static void ExportXML(string path, string filename, int j)
        {
            try
            {
                //string OFX_Presets_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OFX Presets/";
                for (int i = 0; i < Directory.GetDirectories(path).Length; i++)
                {
                    if (Directory.GetDirectories(path)[i].Contains(filename))
                    {
                        string DirName = Path.GetFileName(Directory.GetDirectories(path)[i]);
                        string DirFullPath = Directory.GetDirectories(path)[i];
                        string Full_FilterPath = $"{DirFullPath}/Filter/";
                        string MoveTo_Full_FilterPath = Directory.GetCurrentDirectory() + $"/{DirName}/Filter";


                        Directory.CreateDirectory(Directory.GetCurrentDirectory() + $"/{DirName}"); //Directory
                        Directory.CreateDirectory(MoveTo_Full_FilterPath);//Directory + Filter
                        /*for (int j = 0; j < Directory.GetFiles(Full_FilterPath).Length; j++)
                        {
                            string xmlFileName = Path.GetFileName(Directory.GetFiles(Full_FilterPath)[j]);
                            File.Copy(Directory.GetFiles(Full_FilterPath)[j], MoveTo_Full_FilterPath + $"/{xmlFileName}");
                        }*/

                        string xmlFileName = Path.GetFileName(Directory.GetFiles(Full_FilterPath)[j]);
                        //MessageBox.Show(Directory.GetFiles(Full_FilterPath)[j] + "\n\n" + MoveTo_Full_FilterPath + $"/{xmlFileName}");
                        File.Copy(Directory.GetFiles(Full_FilterPath)[j], MoveTo_Full_FilterPath + $"/{xmlFileName}");
                    }
                }
            }
            catch { }
        }

        public static void ExportFavoriteRender(string path, string filename, int j)
        {
            string AppDataRender_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sony\Render Templates";
            string renderTemp_path = Directory.GetCurrentDirectory() + $"/Render Templates/";
            string avc_mc = renderTemp_path + "avc-mc";
            for (int i = 0; i < Directory.GetFiles(path).Length; i++)
            {
                if (Directory.GetFiles(path)[i].Contains(filename))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + $"/Render Templates"); //Directory
                    Directory.CreateDirectory(avc_mc);//Directory + avc_mc
                    string renderFileName = Path.GetFileName(Directory.GetFiles(path)[j]);
                    if (File.Exists(renderTemp_path + $"/avc-mc/{renderFileName}"))
                        File.Delete(renderTemp_path + $"/avc-mc/{renderFileName}");
                    File.Copy(Directory.GetFiles(path)[j], renderTemp_path + $"/avc-mc/{renderFileName}");
                }
            }
            if (File.Exists(renderTemp_path + "/Favorites.settings"))
                File.Delete(renderTemp_path + "/Favorites.settings");
            File.Copy(AppDataRender_path + "/Favorites.settings", renderTemp_path + "/Favorites.settings");

        }
    }
    #endregion

}

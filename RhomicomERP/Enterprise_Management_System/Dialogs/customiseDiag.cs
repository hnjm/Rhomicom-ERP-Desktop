using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Enterprise_Management_System.Classes;
using Microsoft.VisualBasic.Devices;
using System.IO;
using Microsoft.Win32;

namespace Enterprise_Management_System.Dialogs
{
    public partial class customiseDiag : Form
    {
        public customiseDiag()
        {
            InitializeComponent();
        }
        public Computer myComputer = new Microsoft.VisualBasic.Devices.Computer();

        private void ChngColor()
        {
            this.bClrLabel.BackColor = Color.FromArgb((int)this.bClrNumUpDwn1.Value, (int)this.bClrNumUpDwn2.Value, (int)this.bClrNumUpDwn3.Value);
            this.Glabel.BackColor = Color.FromArgb((int)this.gClrNumUpDwn1.Value, (int)this.gClrNumUpDwn2.Value, (int)this.gClrNumUpDwn3.Value);
            this.HLabel.BackColor = Color.FromArgb((int)this.hClrNumUpDwn1.Value, (int)this.hClrNumUpDwn2.Value, (int)this.hClrNumUpDwn3.Value);
        }
        private void bClrNumUpDwn1_ValueChanged(object sender, EventArgs e)
        {
            this.ChngColor();
        }

        private void bClrNumUpDwn2_ValueChanged(object sender, EventArgs e)
        {
            this.ChngColor();
        }

        private void bClrNumUpDwn3_ValueChanged(object sender, EventArgs e)
        {
            this.ChngColor();
        }

        private void gClrNumUpDwn1_ValueChanged(object sender, EventArgs e)
        {
            this.ChngColor();
        }

        private void gClrNumUpDwn2_ValueChanged(object sender, EventArgs e)
        {
            this.ChngColor();
        }

        private void gClrNumUpDwn3_ValueChanged(object sender, EventArgs e)
        {
            this.ChngColor();
        }

        private void hClrNumUpDwn1_ValueChanged(object sender, EventArgs e)
        {
            this.ChngColor();
        }

        private void hClrNumUpDwn2_ValueChanged(object sender, EventArgs e)
        {
            this.ChngColor();
        }

        private void hClrNumUpDwn3_ValueChanged(object sender, EventArgs e)
        {
            this.ChngColor();
        }

        private void loadThemeFiles()
        {
            string[] smplFiles = System.IO.Directory.GetFiles(Application.StartupPath + @"\DBInfo\", "*.rtheme", System.IO.SearchOption.TopDirectoryOnly);
            this.themeComboBox.Items.Clear();
            for (int i = 0; i < smplFiles.Length; i++)
            {
                this.themeComboBox.Items.Add(smplFiles[i].Replace(Application.StartupPath + @"\DBInfo\", "").Replace(".rtheme", ""));
            }
            if (this.themeComboBox.Items.Count > 0)
            {
                this.themeComboBox.SelectedIndex = 0;
            }
        }

        private void customiseDiag_Load(object sender, EventArgs e)
        {
            this.loadThemeFiles();
            this.loadConnFiles();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (this.modulesBaughtComboBox.Text == "")
            {
                Global.myNwMainFrm.cmnCdMn.showMsg("Please select the Default Modules needed First!", 0);
                return;
            }
            this.saveCstmsFile("Default.rtheme");
            CommonCode.CommonCodes.ModulesNeeded = this.modulesBaughtComboBox.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void saveCstmsFile(string flNm)
        {
            if (this.modulesBaughtComboBox.Text == "")
            {
                Global.myNwMainFrm.cmnCdMn.showMsg("Please select the Default Modules needed First!", 0);
                return;
            }
            System.IO.StreamWriter fileWriter;
            string fileLoc = "";
            fileLoc = @"DBInfo\" + flNm;
            try
            {
                fileWriter = Global.myNwMainFrm.cmnCdMn.myComputer.FileSystem.OpenTextFileWriter(fileLoc, false);
                fileWriter.WriteLine(this.bClrNumUpDwn1.Value.ToString() + "," + this.bClrNumUpDwn2.Value.ToString() + "," + this.bClrNumUpDwn3.Value.ToString());
                fileWriter.WriteLine(this.gClrNumUpDwn1.Value.ToString() + "," + this.gClrNumUpDwn2.Value.ToString() + "," + this.gClrNumUpDwn3.Value.ToString());
                fileWriter.WriteLine(this.hClrNumUpDwn1.Value.ToString() + "," + this.hClrNumUpDwn2.Value.ToString() + "," + this.hClrNumUpDwn3.Value.ToString());
                fileWriter.WriteLine(Global.myNwMainFrm.cmnCdMn.cnvrtBoolToBitStr(this.autoConnectCheckBox.Checked));
                fileWriter.WriteLine(this.modulesBaughtComboBox.Text);

                fileWriter.Close();
                fileWriter = null;
                if (Global.homeFrm.BackgroundImage != null)
                {
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage = null;
                    Application.DoEvents();
                    Application.DoEvents();
                    Application.DoEvents();
                }
                if (this.pictureBox1.Image != null && this.pictureBox1.Image != this.pictureBox1.ErrorImage)
                {
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image = null;
                    Application.DoEvents();
                    this.pictureBox1.Image = Properties.Resources.blank;
                    this.pictureBox1.Image = Image.FromFile(this.fileLocTextBox.Text);
                    string fileName = fileLoc.Replace(".rtheme", ".jpg");
                    if (this.myComputer.FileSystem.FileExists(fileName) && !this.fileLocTextBox.Text.Contains(fileName))
                    {
                        this.myComputer.FileSystem.DeleteFile(fileName,
                          Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                         Microsoft.VisualBasic.FileIO.RecycleOption.DeletePermanently,
                         Microsoft.VisualBasic.FileIO.UICancelOption.DoNothing);
                    }
                    if (!this.fileLocTextBox.Text.Contains(fileName))
                    {
                        this.pictureBox1.Image.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }
                else
                {
                    string fileName = fileLoc.Replace(".rtheme", ".jpg");
                    if (this.myComputer.FileSystem.FileExists(fileName) && this.fileLocTextBox.Text == "")
                    {
                        this.myComputer.FileSystem.DeleteFile(fileName,
                          Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                         Microsoft.VisualBasic.FileIO.RecycleOption.DeletePermanently,
                         Microsoft.VisualBasic.FileIO.UICancelOption.DoNothing);
                    }
                }
            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg("Error saving file!\n" + ex.Message + "\r\n" + ex.StackTrace, 0);
                this.pictureBox1.Image.Dispose();
                this.pictureBox1.Image.Dispose();
                this.pictureBox1.Image.Dispose();
                this.pictureBox1.Image.Dispose();
                this.pictureBox1.Image.Dispose();
                this.pictureBox1.Image.Dispose();
                this.pictureBox1.Image.Dispose();
                this.pictureBox1.Image.Dispose();
                this.pictureBox1.Image.Dispose();
                this.pictureBox1.Image.Dispose();
                this.pictureBox1.Image = null;
                Application.DoEvents();
                this.pictureBox1.Image = Properties.Resources.blank;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void color1Button_Click(object sender, EventArgs e)
        {
            DialogResult dgres = colorDialog1.ShowDialog();
            //colorDialog1. = this.bClrLabel.BackColor;
            colorDialog1.AllowFullOpen = true;
            colorDialog1.AnyColor = true;
            colorDialog1.FullOpen = true;

            if (dgres == DialogResult.OK)
            {
                Color clr = colorDialog1.Color;
                this.bClrNumUpDwn1.Value = (Decimal)clr.R;
                this.bClrNumUpDwn2.Value = (Decimal)clr.G;
                this.bClrNumUpDwn3.Value = (Decimal)clr.B;

            }
        }

        private void color2Button_Click(object sender, EventArgs e)
        {
            DialogResult dgres = colorDialog1.ShowDialog();
            //colorDialog1.Color = this.Glabel.BackColor;
            colorDialog1.AllowFullOpen = true;
            colorDialog1.AnyColor = true;
            colorDialog1.FullOpen = true;

            if (dgres == DialogResult.OK)
            {
                Color clr = colorDialog1.Color;
                this.gClrNumUpDwn1.Value = (Decimal)clr.R;
                this.gClrNumUpDwn2.Value = (Decimal)clr.G;
                this.gClrNumUpDwn3.Value = (Decimal)clr.B;

            }
        }

        private void color3Button_Click(object sender, EventArgs e)
        {
            DialogResult dgres = colorDialog1.ShowDialog();
            //colorDialog1.Color = this.HLabel.BackColor;
            colorDialog1.AllowFullOpen = true;
            colorDialog1.AnyColor = true;
            colorDialog1.FullOpen = true;

            if (dgres == DialogResult.OK)
            {
                Color clr = colorDialog1.Color;
                this.hClrNumUpDwn1.Value = (Decimal)clr.R;
                this.hClrNumUpDwn2.Value = (Decimal)clr.G;
                this.hClrNumUpDwn3.Value = (Decimal)clr.B;

            }
        }

        private void themeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.IO.StreamReader fileReader;
            //Color[] clrs = { Color.FromArgb(0, 102, 160), Color.FromArgb(0, 129, 206), Color.FromArgb(0, 255, 0) };
            string fileLoc = "";
            fileLoc = @"DBInfo\" + this.themeComboBox.Text + ".rtheme";
            if (Global.myNwMainFrm.cmnCdMn.myComputer.FileSystem.FileExists(fileLoc))
            {
                fileReader = Global.myNwMainFrm.cmnCdMn.myComputer.FileSystem.OpenTextFileReader(fileLoc);
                try
                {
                    char[] cho = { ',' };
                    string[] bck = fileReader.ReadLine().Split(cho, StringSplitOptions.RemoveEmptyEntries);
                    this.bClrNumUpDwn1.Value = Decimal.Parse(bck[0]);
                    this.bClrNumUpDwn2.Value = Decimal.Parse(bck[1]);
                    this.bClrNumUpDwn3.Value = Decimal.Parse(bck[2]);
                    string[] btm = fileReader.ReadLine().Split(cho, StringSplitOptions.RemoveEmptyEntries);
                    this.gClrNumUpDwn1.Value = Decimal.Parse(btm[0]);
                    this.gClrNumUpDwn2.Value = Decimal.Parse(btm[1]);
                    this.gClrNumUpDwn3.Value = Decimal.Parse(btm[2]);
                    string[] btm1 = fileReader.ReadLine().Split(cho, StringSplitOptions.RemoveEmptyEntries);
                    this.hClrNumUpDwn1.Value = Decimal.Parse(btm1[0]);
                    this.hClrNumUpDwn2.Value = Decimal.Parse(btm1[1]);
                    this.hClrNumUpDwn3.Value = Decimal.Parse(btm1[2]);
                    this.autoConnectCheckBox.Checked = Global.myNwMainFrm.cmnCdMn.cnvrtBitStrToBool(fileReader.ReadLine());
                    string mdlsght = fileReader.ReadLine();
                    if (mdlsght == "")
                    {
                        mdlsght = "Person Records Only";
                    }
                    this.modulesBaughtComboBox.SelectedItem = "Person Records Only";
                    fileReader.Close();
                    fileReader = null;
                    string fileName = fileLoc.Replace(".rtheme", ".jpg");
                    if (this.pictureBox1.Image != null && this.pictureBox1.Image != this.pictureBox1.ErrorImage
                        && this.pictureBox1.Image != Properties.Resources.blank)
                    {
                        this.pictureBox1.Image.Dispose();
                        this.pictureBox1.Image.Dispose();
                        this.pictureBox1.Image.Dispose();
                        this.pictureBox1.Image.Dispose();
                        this.pictureBox1.Image.Dispose();
                        this.pictureBox1.Image.Dispose();
                        this.pictureBox1.Image.Dispose();
                        this.pictureBox1.Image.Dispose();
                        this.pictureBox1.Image.Dispose();
                        this.pictureBox1.Image.Dispose();

                        this.pictureBox1.Image = null;
                        Application.DoEvents();
                        Application.DoEvents();
                        Application.DoEvents();
                    }
                    this.fileLocTextBox.Text = "";
                    if (this.myComputer.FileSystem.FileExists(fileName))
                    {
                        this.fileLocTextBox.Text = fileName;
                        this.pictureBox1.Image = Image.FromFile(fileName);
                    }


                }
                catch (Exception ex)
                {
                    Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message + "\r\n" + ex.StackTrace + "\r\n" + ex.InnerException, 0);
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image = null;
                    Application.DoEvents();
                    this.pictureBox1.Image = Properties.Resources.blank;
                    fileReader.Close();
                    fileReader = null;
                }
            }
        }

        private void saveThemeButton_Click(object sender, EventArgs e)
        {
            this.saveCstmsFile(this.themeComboBox.Text + ".rtheme");
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.RestoreDirectory = true;
            this.openFileDialog1.Filter = "All Files|*.*|Image Files|*.bmp;*.gif;*.jpg;*.png|Bitmaps|*.bmp|GIFs|*.gif|JPEGs|*.jpg|PNGs|*.png";
            this.openFileDialog1.FilterIndex = 2;
            this.openFileDialog1.Title = "Select a picture to Load...";
            this.openFileDialog1.FileName = "";
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (this.pictureBox1.Image != null && this.pictureBox1.Image != this.pictureBox1.ErrorImage
                       && this.pictureBox1.Image != Properties.Resources.blank)
                {
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();

                    this.pictureBox1.Image = null;
                    Application.DoEvents();
                    Application.DoEvents();
                    Application.DoEvents();
                }

                System.Drawing.Image img = Image.FromFile(this.openFileDialog1.FileName);
                this.pictureBox1.Image = img;
                this.fileLocTextBox.Text = this.openFileDialog1.FileName;
            }
        }

        private void delButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.homeFrm.BackgroundImage != null)
                {
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage.Dispose();
                    Global.homeFrm.BackgroundImage = null;
                    Application.DoEvents();
                    Application.DoEvents();
                    Application.DoEvents();
                }
                if (this.pictureBox1.Image != null && this.pictureBox1.Image != this.pictureBox1.ErrorImage
                    && this.pictureBox1.Image != Properties.Resources.blank)
                {
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();
                    this.pictureBox1.Image.Dispose();

                    this.pictureBox1.Image = null;
                    Application.DoEvents();
                    Application.DoEvents();
                    Application.DoEvents();
                }
                string fileName = this.fileLocTextBox.Text;
                if (this.myComputer.FileSystem.FileExists(fileName))
                {
                    this.myComputer.FileSystem.DeleteFile(fileName,
                      Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                     Microsoft.VisualBasic.FileIO.RecycleOption.DeletePermanently,
                     Microsoft.VisualBasic.FileIO.UICancelOption.DoNothing);
                }
                this.fileLocTextBox.Text = "";
            }
            catch (Exception ex)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.loadThemeFiles();
        }

        private void loadConnFiles()
        {
            string[] smplFiles = Directory.GetFiles(Application.StartupPath + @"\DBInfo\", "*.rho", SearchOption.TopDirectoryOnly);
            this.storedConnsComboBox.Items.Clear();
            for (int i = 0; i < smplFiles.Length; i++)
            {
                if (!smplFiles[i].Contains("customize.rho"))
                {
                    this.storedConnsComboBox.Items.Add(smplFiles[i].Replace(Application.StartupPath + @"\DBInfo\", "").Replace(".rho", ""));
                }
            }
            if (this.storedConnsComboBox.Items.Count > 0)
            {
                this.storedConnsComboBox.SelectedIndex = 0;
            }
        }
        private void loadConnFiles(string fileNm)
        {
            string[] smplFiles = Directory.GetFiles(Application.StartupPath + @"\DBInfo\", "*.rho", SearchOption.TopDirectoryOnly);
            this.storedConnsComboBox.Items.Clear();
            int slctdIdx = 0;
            for (int i = 0; i < smplFiles.Length; i++)
            {
                if (!smplFiles[i].Contains("customize.rho"))
                {
                    this.storedConnsComboBox.Items.Add(smplFiles[i].Replace(Application.StartupPath + @"\DBInfo\", "").Replace(".rho", ""));
                }
                if (smplFiles[i].Contains(fileNm))
                {
                    slctdIdx = i;
                }
            }
            if (this.storedConnsComboBox.Items.Count > 0)
            {
                this.storedConnsComboBox.SelectedIndex = slctdIdx;
            }
        }
        private void readConnFile()
        {
            StreamReader fileReader = null;
            string fileLoc = "";
            fileLoc = @"DBInfo\" + this.storedConnsComboBox.Text + ".rho";
            if (Global.myNwMainFrm.cmnCdMn.myComputer.FileSystem.FileExists(fileLoc))
            {
                fileReader = Global.myNwMainFrm.cmnCdMn.myComputer.FileSystem.OpenTextFileReader(fileLoc);
                try
                {
                    this.hostTextBox.Text = Global.myNwMainFrm.cmnCdMn.decrypt(fileReader.ReadLine(), CommonCode.CommonCodes.OrgnlAppKey);
                    this.pwdTextBox.Text = Global.myNwMainFrm.cmnCdMn.decrypt(fileReader.ReadLine(), CommonCode.CommonCodes.OrgnlAppKey);
                    this.unameTextBox.Text = Global.myNwMainFrm.cmnCdMn.decrypt(fileReader.ReadLine(), CommonCode.CommonCodes.OrgnlAppKey);
                    this.dbaseTextBox.Text = Global.myNwMainFrm.cmnCdMn.decrypt(fileReader.ReadLine(), CommonCode.CommonCodes.OrgnlAppKey);
                    this.portTextBox.Text = Global.myNwMainFrm.cmnCdMn.decrypt(fileReader.ReadLine(), CommonCode.CommonCodes.OrgnlAppKey);
                    this.pgDirTextBox.Text = Global.myNwMainFrm.cmnCdMn.decrypt(fileReader.ReadLine(), CommonCode.CommonCodes.OrgnlAppKey);
                    this.javaBinDirTextBox.Text = Global.myNwMainFrm.cmnCdMn.decrypt(fileReader.ReadLine(), CommonCode.CommonCodes.OrgnlAppKey);
                    if (this.javaBinDirTextBox.Text == "")
                    {
                        this.javaBinDirTextBox.Text = Global.myNwMainFrm.cmnCdMn.GetJavaInstallationPath();
                        if (!this.javaBinDirTextBox.Text.Contains("\\bin"))
                        {
                            this.javaBinDirTextBox.Text = this.javaBinDirTextBox.Text + "\\bin";
                        }
                    }
                    fileReader.Close();
                    fileReader = null;
                }
                catch (Exception ex)
                {
                    fileReader.Close();
                    fileReader = null;
                }
                finally
                {
                    fileReader = null;
                    Global.myNwMainFrm.cmnCdMn.minimizeMemory();
                    GC.Collect();
                }
            }

            if (this.pgDirTextBox.Text == "")
            {
                if (CommonCode.CommonCodes.is64BitOperatingSystem == true)
                {
                    this.pgDirTextBox.Text = this.getRegistryVal("Base Directory", @"PostgreSQL\Installations\postgresql-x64-9.3");
                    if (this.pgDirTextBox.Text == "")
                    {
                        this.pgDirTextBox.Text = this.get64RegistryVal("Base Directory", @"PostgreSQL\Installations\postgresql-9.3");
                    }
                }
                else
                {
                    this.pgDirTextBox.Text = this.getRegistryVal("Base Directory", @"PostgreSQL\Installations\postgresql-9.3");
                }
                if (this.pgDirTextBox.Text != "")
                {
                    this.pgDirTextBox.Text += @"\bin\";
                }
            }
            else if (!Global.myNwMainFrm.cmnCdMn.myComputer.FileSystem.DirectoryExists(this.pgDirTextBox.Text))
            {
                if (CommonCode.CommonCodes.is64BitOperatingSystem == true)
                {
                    this.pgDirTextBox.Text = this.getRegistryVal("Base Directory", @"PostgreSQL\Installations\postgresql-x64-9.3");
                    if (this.pgDirTextBox.Text == "")
                    {
                        this.pgDirTextBox.Text = this.get64RegistryVal("Base Directory", @"PostgreSQL\Installations\postgresql-9.3");
                    }
                }
                else
                {
                    this.pgDirTextBox.Text = this.getRegistryVal("Base Directory", @"PostgreSQL\Installations\postgresql-9.3");
                }
                if (this.pgDirTextBox.Text != "")
                {
                    this.pgDirTextBox.Text += @"\bin\";
                }
            }
        }

        private void saveConnFile(string flNm)
        {
            StreamWriter fileWriter = null;
            string fileLoc = "";
            if (flNm == "")
            {
                flNm = this.hostTextBox.Text.Replace("\"", "") + "_" +
              this.dbaseTextBox.Text + ".rho";
            }
            fileLoc = @"DBInfo\" + flNm;
            try
            {
                fileWriter = Global.myNwMainFrm.cmnCdMn.myComputer.FileSystem.OpenTextFileWriter(fileLoc, false);
                fileWriter.WriteLine(Global.myNwMainFrm.cmnCdMn.encrypt1(this.hostTextBox.Text, CommonCode.CommonCodes.OrgnlAppKey));
                fileWriter.WriteLine(Global.myNwMainFrm.cmnCdMn.encrypt1(this.pwdTextBox.Text, CommonCode.CommonCodes.OrgnlAppKey));
                fileWriter.WriteLine(Global.myNwMainFrm.cmnCdMn.encrypt1(this.unameTextBox.Text, CommonCode.CommonCodes.OrgnlAppKey));
                fileWriter.WriteLine(Global.myNwMainFrm.cmnCdMn.encrypt1(this.dbaseTextBox.Text, CommonCode.CommonCodes.OrgnlAppKey));
                fileWriter.WriteLine(Global.myNwMainFrm.cmnCdMn.encrypt1(this.portTextBox.Text, CommonCode.CommonCodes.OrgnlAppKey));
                fileWriter.WriteLine(Global.myNwMainFrm.cmnCdMn.encrypt1(this.pgDirTextBox.Text, CommonCode.CommonCodes.OrgnlAppKey));
                if (this.javaBinDirTextBox.Text == "")
                {
                    this.javaBinDirTextBox.Text = Global.myNwMainFrm.cmnCdMn.GetJavaInstallationPath();
                    if (!this.javaBinDirTextBox.Text.Contains("\\bin"))
                    {
                        this.javaBinDirTextBox.Text = this.javaBinDirTextBox.Text + "\\bin";
                    }
                }
                fileWriter.WriteLine(Global.myNwMainFrm.cmnCdMn.encrypt1(this.javaBinDirTextBox.Text, CommonCode.CommonCodes.OrgnlAppKey));
                fileWriter.Close();
                fileWriter = null;
            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg("Error saving file!\n" + ex.Message + "\r\n" + ex.StackTrace + "\r\n" + ex.InnerException, 0);
                fileWriter.Close();
                fileWriter = null;
            }
            finally
            {
                fileWriter = null;
                Global.myNwMainFrm.cmnCdMn.minimizeMemory();
                GC.Collect();
            }
        }

        private void delConnButton_Click(object sender, EventArgs e)
        {
            if (Global.myNwMainFrm.cmnCdMn.showMsg("Are you sure you want to " +
         "delete the Selected Stored Connection?", 1) == DialogResult.No)
            {
                return;
            }
            string fileLoc = "";
            fileLoc = @"DBInfo\" + this.storedConnsComboBox.Text + ".rho";
            try
            {
                Global.myNwMainFrm.cmnCdMn.myComputer.FileSystem.DeleteFile(fileLoc,
                  Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                  Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                this.loadConnFiles();
            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 4);
            }
        }

        private void saveConnctnButton_Click(object sender, EventArgs e)
        {
            if (this.hostTextBox.Text == "" || this.dbaseTextBox.Text == "")
            {
                Global.myNwMainFrm.cmnCdMn.showMsg("Kindly fill all Required Fields!", 0);
            }
            string fileNM = this.hostTextBox.Text.Replace("\"", "") + "_" +
          this.dbaseTextBox.Text;

            this.saveConnFile(fileNM + ".rho");
            MessageBox.Show("Saving Successful!");
            this.loadConnFiles(fileNM + ".rho");
        }

        private void storedConnsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.storedConnsComboBox.SelectedIndex >= 0)
            {
                this.readConnFile();
            }
        }

        private void dfltConnButton_Click(object sender, EventArgs e)
        {
            this.saveConnFile("ActiveDB.rho");
            this.loadConnFiles("ActiveDB.rho");
            MessageBox.Show("Saving Successful!");
            this.restartAppButton.PerformClick();
        }

        private void restartAppButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.myNwMainFrm.cmnCdMn.showMsg("Are you sure you want to Restart the application?", 1) == DialogResult.No)
                {
                    return;
                }
                System.Windows.Forms.Application.Restart();
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.Application.Restart();
            }
        }
        public string getRegistryVal(string keyname, string prdctNm)
        {
            RegistryKey rk = Registry.LocalMachine;
            RegistryKey sk1 = rk.OpenSubKey(@"SOFTWARE\" + prdctNm);
            if (sk1 == null)
            {
                return "";
            }
            else
            {
                try
                {
                    return (string)sk1.GetValue(keyname);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!");
                    return "";
                }
            }
        }

        public string get64RegistryVal(string keyname, string prdctNm)
        {
            RegistryKey rk = Registry.LocalMachine;
            RegistryKey sk1 = rk.OpenSubKey(@"SOFTWARE\Wow6432Node\" + prdctNm);
            if (sk1 == null)
            {
                return "";
            }
            else
            {
                try
                {
                    return (string)sk1.GetValue(keyname);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!");
                    return "";
                }
            }
        }
    }
}
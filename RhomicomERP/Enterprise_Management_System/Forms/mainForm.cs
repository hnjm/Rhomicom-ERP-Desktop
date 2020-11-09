using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Enterprise_Management_System.Classes;
using Enterprise_Management_System.Dialogs;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Enterprise_Management_System.Forms
{
    public partial class mainForm : Form
    {
        #region "GLOBAL VARIABLES..."
        public CommonCode.CommonCodes cmnCdMn = new CommonCode.CommonCodes();
        #endregion

        #region "MAIN FORM & TIMERS EVENT HANDLERS..."
        public mainForm()
        {
            InitializeComponent();
        }
        public void changeBackground()
        {
            Global.myNwMainFrm.statusLoadLabel.Visible = true;
            Global.myNwMainFrm.statusLoadPictureBox.Visible = true;
            System.Windows.Forms.Application.DoEvents();

            Color[] clrs = Global.myNwMainFrm.cmnCdMn.getColors();
            this.bannerGlsLabel.TopFill = clrs[0];
            this.bannerGlsLabel.BottomFill = clrs[1];
            this.mainDockPanel.BackColor = clrs[0];
            this.mainDockPanel.DockBackColor = clrs[0];

            this.mainDockPanel.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.StartColor = clrs[0];
            this.mainDockPanel.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.EndColor = clrs[2];
            this.mainDockPanel.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.TextColor = Color.Black;

            this.mainDockPanel.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.StartColor = clrs[0]; ;
            this.mainDockPanel.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.EndColor = clrs[1];

            this.mainDockPanel.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.StartColor = clrs[0];
            this.mainDockPanel.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.EndColor = clrs[1];
            this.mainDockPanel.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.TextColor = Color.White;
            //this.mainDockPanel.DockBottomPortion
            if (Global.homeFrm != null)
            {
                Global.homeFrm.curRoleLabel.BackColor = clrs[0];
                Global.homeFrm.dbServerDateLabel.ForeColor = clrs[2];
                Global.homeFrm.dbServerTimeLabel.ForeColor = clrs[2];
                Global.homeFrm.userLabel.ForeColor = clrs[2];
                Global.homeFrm.userLogTimeLabel.ForeColor = clrs[2];
                Global.homeFrm.curRoleLabel.ForeColor = clrs[2];
                string fileLoc = "";
                if (CommonCode.CommonCodes.Db_dbase != "")
                {
                    int dbaseLovID = Global.myNwMainFrm.cmnCdMn.getLovID("Per Database Background Themes");
                    string dbaseBackColor = Global.myNwMainFrm.cmnCdMn.getEnbldPssblValDesc(
                      CommonCode.CommonCodes.Db_dbase, dbaseLovID);
                    if (dbaseBackColor != "")
                    {
                        fileLoc = @dbaseBackColor;
                    }
                }
                fileLoc = fileLoc.Replace(".rtheme", ".jpg");
                if (fileLoc == "" || !System.IO.File.Exists(fileLoc))
                {
                    if (CommonCode.CommonCodes.Db_dbase.Contains("test")
              || CommonCode.CommonCodes.Db_dbase.Contains("try")
              || CommonCode.CommonCodes.Db_dbase.Contains("trial")
              || CommonCode.CommonCodes.Db_dbase.Contains("train")
              || CommonCode.CommonCodes.Db_dbase.Contains("sample"))
                    {
                        fileLoc = @"DBInfo\Default_Test.jpg";
                    }
                    else
                    {
                        fileLoc = @"DBInfo\Default.jpg";
                    }
                }
                if (System.IO.File.Exists(fileLoc))
                {
                    Image imgBkg = Image.FromFile(fileLoc);
                    if (Global.homeFrm.BackgroundImage == null)
                    {
                        Global.homeFrm.BackgroundImage = Image.FromFile(fileLoc);
                        Global.homeFrm.BackColor = clrs[1];
                    }
                    else if (!Global.homeFrm.BackgroundImage.Equals(imgBkg))
                    {
                        Global.homeFrm.BackgroundImage = Image.FromFile(fileLoc);
                        Global.homeFrm.BackColor = clrs[1];
                    }
                }
                else
                {
                    Global.homeFrm.BackgroundImage = null;
                    Global.homeFrm.BackColor = clrs[0];
                }
            }
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            string tst = System.Environment.GetEnvironmentVariable("Path");
            CommonCode.CommonCodes.LocalDataPool = new string[100];
            this.appVersionStatusLabel.Text = CommonCode.CommonCodes.AppName + " " + CommonCode.CommonCodes.AppVersion;
            this.Text = CommonCode.CommonCodes.AppName + " " + CommonCode.CommonCodes.AppVersion;

            Global.myNwMainFrm = this;

            Global.refreshRqrdVrbls();
            this.changeBackground();
            CommonCode.CommonCodes.DatabaseNm = "";
            Global.homeFrm = new homePageForm();
            Global.homeFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);

            this.connectToDatabaseToolStripMenuItem_Click(this.connectToDatabaseToolStripMenuItem, e);
            Global.myNwMainFrm.statusLoadLabel.Visible = false;
            Global.myNwMainFrm.statusLoadPictureBox.Visible = false;

            System.Diagnostics.Process jarPrcs = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C java -version";
            jarPrcs.StartInfo = startInfo;
            jarPrcs.Start();
            System.Threading.Thread.Sleep(200);
            jarPrcs.Close();
        }

        private void mainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            /*try
            {
                //CommonCode.CommonCodes.GlobalSQLConn.ConnectionString = CommonCode.CommonCodes.ConnStr;
                //Global.myNwMainFrm.cmnCdMn.adminForceLogoutLgns(Global.myNwMainFrm.cmnCdMn.User_id);
                //CommonCode.CommonCodes.GlobalSQLConn.Close();
            }
            catch (Exception ex)
            {

            }*/
            if (Global.myNwMainFrm.cmnCdMn.showMsg("Are you sure you want to exit the application?", 1) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            //Global.myNwMainFrm.cmnCdMn.minimizeMemory();
            e.Cancel = false;
            //try
            //{
            //  if (Global.myNwMainFrm.cmnCdMn.hsSessionExpired(CommonCode.CommonCodes.GlobalSQLConn))
            //  {
            //    e.Cancel = false;
            //  }
            //  else
            //  {

            //    this.disconnectDB_Actns();
            //    e.Cancel = false;
            //  }
            //  //if (System.IO.Directory.Exists(Application.StartupPath + "\\Images\\" + CommonCode.CommonCodes.DatabaseNm))
            //  //{
            //  //  System.IO.Directory.Delete(Application.StartupPath + "\\Images\\" + CommonCode.CommonCodes.DatabaseNm, true);
            //  //}
            //}
            //catch (Exception ex)
            //{
            //  e.Cancel = false;
            //}
        }

        private void updtLabelsTimer_Tick(object sender, EventArgs e)
        {
            if (Global.myNwMainFrm.cmnCdMn.AutoRfrsh == false)
            {
                this.updtLabelsTimer.Enabled = false;
                return;
            }
            this.updtLabelsTimer.Enabled = false;
            //this.updateDBLabels();
            //this.updateLoginLabels();
            this.updtLabelsTimer.Enabled = true;
        }
        #endregion

        #region "START MENU ITEMS EVENT HANDLERS..."
        public bool isDsconnet = false;
        private void connectToDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.FindDockedFormExistence("Home Page") == false)
            {
                homePageForm nwFrm = new homePageForm();
                Global.homeFrm = nwFrm;
                Global.homeFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                if (Global.login_number <= 0 && CommonCode.CommonCodes.DatabaseNm != "")
                {
                    Global.homeFrm.loadConnectDiag();
                }
            }
            else
            {
                this.FindDockedFormToActivate("Home Page");
            }

            if (this.connectToDatabaseToolStripMenuItem.Text.ToLower().Contains("disconnect"))
            {
                if (Global.myNwMainFrm.cmnCdMn.showMsg("Are you sure you want to disconnect!", 1) == DialogResult.Yes)
                {
                    try
                    {
                        ////System.Windows.Forms.Application.DoEvents();
                        if (Global.homeFrm != null)
                        {
                            this.closeAllDockedFormsExcpt(Global.homeFrm.TabText);
                        }
                        else
                        {
                            this.closeAllDockedFormsExcpt("Home Page");
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    this.disconnectDB_Actns();
                    this.connectDB_Actions();
                }
            }
            else
            {
                this.connectDB_Actions();
                try
                {
                    CommonCode.CommonCodes.GlobalSQLConn.Close();
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.FindDockedFormExistence("Home Page") == false)
            {
                homePageForm nwFrm = new homePageForm();
                Global.homeFrm = nwFrm;
                Global.homeFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                if (Global.login_number <= 0 && CommonCode.CommonCodes.DatabaseNm == "")
                {
                    Global.homeFrm.loadConnectDiag();
                }
            }
            else
            {
                this.FindDockedFormToActivate("Home Page");
            }
            if (this.loginToolStripMenuItem.Text.ToLower().Contains("logout"))
            {
                if (Global.myNwMainFrm.cmnCdMn.showMsg("This will close all open forms!\nAre you sure you want to Logout?", 1)
                  == DialogResult.Yes)
                {
                    this.logoutActions();
                }
            }
            else
            {
                if (!Global.myNwMainFrm.cmnCdMn.get_LastPatchVrsn().Contains(CommonCode.CommonCodes.AppVrsn))
                {
                    this.cmnCdMn.showMsg("Your Version of this Software is not Up to Date!" +
                      "\r\nContact the System Administrator for Assistance!", 4);
                    return;
                }
                if (!Global.myNwMainFrm.cmnCdMn.isThsMchnPrmtd())
                {
                    this.cmnCdMn.showMsg("This Machine is not Permitted to run this software!\r\nContact the Vendor for Assistance!", 4);
                    return;
                }
                else
                {
                    this.loginActions();
                }
            }
        }

        private void changeMyPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //All the actions occur in the dialog box itself
            /* 1. Load user name
             * 2. user types old password and new password
             * 3. if old password is correct then
             *		a. check if new password is not in recent password history based on the password policy
             *		b. check if new password complexity meets current password policy
             *		c. store old password in encrypted format
             *		d. store the new password in encrypted format and update the last pswd chnge date
             */
            if (Global.login_result == "logout"
         || Global.usr_id <= 0 || Global.login_number <= 0)
            {
                this.logoutActions();
                Global.myNwMainFrm.cmnCdMn.showMsg("You are not qualified to log in!", 0);
                return;
            }

            chngPswdDiag nwDiag = new chngPswdDiag();
            DialogResult dgRes = nwDiag.ShowDialog();
            if (dgRes == DialogResult.OK)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg("Password has been changed successfully!", 3);
                if (Global.role_set_id.Length <= 0)
                {
                    Global.login_result = "select role";
                    this.switchRoleSetToolStripMenuItem.PerformClick();
                }
            }
        }

        private void switchRoleSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* 1. Load All Roles of the current user
             * 2. Warn User that all forms will be closed
             * 3. Store the role the user selects
             * 4. Reload modules based on the role the user selected
             * 5. Refresh Permissions based on the role the user selected			 * 
             */
            bool frmLgn = false;
            if (Global.login_result == "change password")
            {
                Global.myNwMainFrm.cmnCdMn.showMsg("Please change your password first!", 0);
                this.changeMyPasswordToolStripMenuItem.PerformClick();
                return;
            }
            else if (Global.login_result == "logout"
            || Global.usr_id <= 0 || Global.login_number <= 0)
            {
                this.logoutActions();
                Global.myNwMainFrm.cmnCdMn.showMsg("You are not qualified to log in!", 0);
                return;
            }
            switchRolesDiag nwDiag = new switchRolesDiag();
            DialogResult dgRes;

            Global.org_id = Global.myNwMainFrm.cmnCdMn.getPrsnOrgID(Global.usr_id);
            //Global.myNwMainFrm.cmnCdMn.showMsg(Global.role_set_id.Length + "-" + Global.org_id + "-" + Global.usr_id, 0);

            if (Global.role_set_id.Length <= 0 && Global.org_id > 0)
            {
                nwDiag.crntOrgIDTextBox.Text = Global.org_id.ToString();
                DataSet dtst = Global.get_AllUsers_Roles();
                nwDiag.selected_role_id = new int[dtst.Tables[0].Rows.Count];
                for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
                {
                    nwDiag.selected_role_id[i] = int.Parse(dtst.Tables[0].Rows[i][0].ToString());
                }
                dgRes = DialogResult.OK;
                frmLgn = true;
            }
            else
            {
                dgRes = nwDiag.ShowDialog();
            }

            if (dgRes == DialogResult.OK)
            {
                if (Global.role_set_id.Length > 0)
                {
                    if (Global.myNwMainFrm.cmnCdMn.showMsg("This will close all open forms!\nAre you sure you want to proceed?", 1)
                      == DialogResult.No)
                    {
                        return;
                    }
                }
                if (Global.homeFrm != null)
                {
                    Global.homeFrm.curRoleLabel.Text = this.statusLoadLabel.Text;
                }
                //Global.myNwMainFrm.basicSetupToolStripMenuItem.DropDownItems.Clear();
                //Global.myNwMainFrm.specializedModulesToolStripMenuItem.DropDownItems.Clear();
                Global.myNwMainFrm.customModulesToolStripMenuItem.DropDownItems.Clear();
                if (frmLgn == false)
                {
                    Global.myNwMainFrm.statusLoadLabel.Visible = true;
                    Global.myNwMainFrm.statusLoadPictureBox.Visible = true;
                    ////System.Windows.Forms.Application.DoEvents();
                }
                /*this.statusLoadLabel.Visible = true;
                this.statusLoadLabel.Location = new Point((Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Width / 2) - (int)((520 - this.statusLoadPictureBox.Width) / 2),
                (Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Height / 2) - (int)(1.25 * this.statusLoadPictureBox.Height));//
                this.statusLoadPictureBox.Visible = true;
                this.statusLoadPictureBox.Location = new Point(this.statusLoadLabel.Location.X - this.statusLoadPictureBox.Width, this.statusLoadLabel.Location.Y);*/
                ////System.Windows.Forms.Application.DoEvents();
                if (Global.homeFrm != null)
                {
                    this.closeAllDockedFormsExcpt(Global.homeFrm.TabText);
                }
                else
                {
                    this.closeAllDockedFormsExcpt("Home Page");
                }
                ////System.Windows.Forms.Application.DoEvents();
                Global.moduleFuncs.CloseModules();
                Global.role_set_id = nwDiag.selected_role_id;
                Global.org_id = int.Parse(nwDiag.crntOrgIDTextBox.Text);
                //this.backgroundWorker1.RunWorkerAsync();
                CommonCode.CommonCodes.lgnNum = Global.login_number;
                CommonCode.CommonCodes.rlSetIDS = Global.role_set_id;
                CommonCode.CommonCodes.uID = Global.usr_id;
                CommonCode.CommonCodes.ogID = Global.org_id;

                ////System.Windows.Forms.Application.DoEvents();
                Global.refreshRqrdVrbls();
                Global.moduleFuncs.FindModules(Application.StartupPath + @"\Plugins");
                //processDB.CloseMainWindow();
                //processDB.Close();
                //processDB.Dispose();
                this.changeBackground();
                Global.myNwMainFrm.updateDBLabels();
                Global.myNwMainFrm.updateLoginLabels();

                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                ////System.Windows.Forms.Application.DoEvents();
                //if (Global.homeFrm != null)
                //{
                //}
                //MessageBox.Show(Global.role_set_id.Length.ToString());
                //if (Global.role_set_id.Length > 0)
                //{
                //  Global.moduleFuncs.CreateMenuItems();
                //}
            }
        }

        private void myInboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Global.login_result == "change password")
            {
                Global.myNwMainFrm.cmnCdMn.showMsg("Please change your password first!", 0);
                this.changeMyPasswordToolStripMenuItem.PerformClick();
                return;
            }
            else if (Global.login_result == "logout"
         || Global.usr_id <= 0 || Global.login_number <= 0)
            {
                this.logoutActions();
                Global.myNwMainFrm.cmnCdMn.showMsg("You are not qualified to log in!", 0);
                return;
            }
            //All the actions occur in the dialog box itself
            inboxDiag nwDiag = new inboxDiag();
            DialogResult dgRes = nwDiag.ShowDialog();
            if (dgRes == DialogResult.OK)
            {

            }
        }

        private void homePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.FindDockedFormExistence("Home Page") == false)
            {
                homePageForm nwFrm = new homePageForm();
                Global.homeFrm = nwFrm;
                Global.homeFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                if (Global.login_number <= 0 && CommonCode.CommonCodes.DatabaseNm == "")
                {
                    Global.homeFrm.loadConnectDiag();
                }
            }
            else
            {
                this.FindDockedFormToActivate("Home Page");
            }
            this.refreshToolStripMenuItem.PerformClick();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region "OTHER MENUS & DOCKED FORM FUNCTIONS..."
        public void loadClickedModule(object sender, EventArgs e)
        {
            if (sender != null)
            {
                System.Windows.Forms.ToolStripMenuItem item1 = (ToolStripMenuItem)sender;
                Enterprise_Management_System.Classes.Types.AvailableModule selectedPlugin = Global.moduleFuncs.AvailableModules.Find(item1.Text.ToString());
                WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = null;
                String strTitle = "";

                if (selectedPlugin != null)
                {
                    //Again, if the plugin is found, do some work...
                    //This part adds the plugin's info to the 'Plugin Information:' Frame
                    nwMainFrm = selectedPlugin.Instance.mainInterface;
                    strTitle = selectedPlugin.Instance.name;
                    if (nwMainFrm == null)
                    {
                        //Remove that plugin and reload it
                        Global.moduleFuncs.AvailableModules.Remove(selectedPlugin);
                        string file_to_load = Application.StartupPath + @"\Plugins\" + strTitle.Replace(" ", "") + ".dll";
                        if (Global.myNwMainFrm.cmnCdMn.myComputer.FileSystem.FileExists(file_to_load) == true)
                        {
                            Global.moduleFuncs.reloadModule(file_to_load);
                            selectedPlugin = Global.moduleFuncs.AvailableModules.Find(item1.Text.ToString());
                            nwMainFrm = selectedPlugin.Instance.mainInterface;
                            strTitle = selectedPlugin.Instance.name;
                        }
                    }
                    else if (nwMainFrm.IsDisposed)
                    {
                        string path_to_load = Application.StartupPath + @"\Plugins";
                        if (Global.myNwMainFrm.cmnCdMn.myComputer.FileSystem.DirectoryExists(path_to_load) == true)
                        {
                            Global.moduleFuncs.reloadModules(path_to_load);
                            selectedPlugin = Global.moduleFuncs.AvailableModules.Find(item1.Text.ToString());
                            nwMainFrm = selectedPlugin.Instance.mainInterface;
                            strTitle = selectedPlugin.Instance.name;
                        }
                    }
                    if (this.FindDockedFormExistence(strTitle) == false)
                    {
                        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        selectedPlugin.Instance.loadMyRolesNMsgtyps();
                    }
                    else
                    {
                        this.FindDockedFormToActivate(strTitle);
                    }
                }
            }
        }

        public void mainDockPanel_ActiveContentChanged(object sender, System.EventArgs e)
        {
            if (this.mainDockPanel.ActiveContent != null)
            {
                string frmName = this.mainDockPanel.ActiveContent.DockHandler.TabText;
                //Global.currentPlugin = Global.moduleFuncs.AvailableModules.Find(frmName);
                ////System.Windows.Forms.Application.DoEvents();
            }
            else
            {
                if (Global.currentPlugin != null)
                {
                    Global.currentPlugin.Instance.Dispose();
                    Global.currentPlugin = null;
                }
            }
            Global.myNwMainFrm.cmnCdMn.minimizeMemory();
            GC.Collect();
        }

        public Boolean FindDockedFormExistence(string frmName)
        {
            int i = 0;

            for (i = 0; i < this.mainDockPanel.Contents.Count; i++)
            {
                if (this.mainDockPanel.Contents[i].DockHandler.TabText == frmName)
                {
                    return true;
                }
                else
                {
                }
            }
            return false;
        }

        public int FindDockedFormToActivate(string frmName)
        {
            int i = 0;

            for (i = 0; i < this.mainDockPanel.Contents.Count; i++)
            {
                if (this.mainDockPanel.Contents[i].DockHandler.TabText == frmName)
                {
                    this.mainDockPanel.Contents[i].DockHandler.Activate();
                    return i;
                }
                else
                {
                }
            }
            return -1;
        }

        public int FindDockedFormToClose(string frmName)
        {
            int i = 0;

            for (i = 0; i < this.mainDockPanel.Contents.Count; i++)
            {
                if (this.mainDockPanel.Contents[i].DockHandler.TabText == frmName)
                {
                    this.mainDockPanel.Contents[i].DockHandler.Close();
                    return i;
                }
                else
                {
                }
            }
            return -1;
        }

        public WeifenLuo.WinFormsUI.Docking.DockContent GetADockedForm(string frmName)
        {
            int i = 0;

            for (i = 0; i < this.mainDockPanel.Contents.Count; i++)
            {
                if (this.mainDockPanel.Contents[i].DockHandler.TabText == frmName)
                {
                    return (WeifenLuo.WinFormsUI.Docking.DockContent)this.mainDockPanel.Contents[i].DockHandler.Content;
                }
                else
                {
                }
            }
            return null;
        }

        public void closeAllDockedFormsExcpt(string frmName)
        {
            int i = 0;
            WeifenLuo.WinFormsUI.Docking.IDockContent[] cntnts = new WeifenLuo.WinFormsUI.Docking.IDockContent[this.mainDockPanel.Contents.Count];
            foreach (WeifenLuo.WinFormsUI.Docking.IDockContent cntnt in this.mainDockPanel.Contents)
            {
                if (cntnt.DockHandler.TabText != frmName)
                {
                    cntnts[i] = cntnt;
                }
                i++;
            }
            for (i = 0; i < cntnts.Length; i++)
            {
                if (cntnts[i] != null)
                {
                    cntnts[i].DockHandler.Close();
                }
            }
        }
        #endregion

        #region "TOOLS MENU ITEMS EVENT HANDLERS..."
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (Global.currentPlugin != null)
            //{
            //  Global.currentPlugin.Instance.refreshData();
            //}
            //else
            //{
            //  Global.myNwMainFrm.cmnCdMn.showMsg("No Active Module Yet!", 3);
            //}
            //this.Refresh();
            if (Global.homeFrm != null)
            {
                Global.homeFrm.refreshButton_Click(Global.homeFrm.refreshButton, e);
            }
        }

        private void viewSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Global.currentPlugin != null)
            {
                Global.currentPlugin.Instance.viewCurSQL();
            }
            else
            {
                Global.myNwMainFrm.cmnCdMn.showMsg("No Active Module Yet!", 3);
            }
        }

        private void createExcelDataImportTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Global.currentPlugin != null)
            {
                Global.currentPlugin.Instance.createExcelTemplate();
            }
            else
            {
                Global.myNwMainFrm.cmnCdMn.showMsg("No Active Module Yet!", 3);
            }
        }

        private void importDataFromExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Global.currentPlugin != null)
            {
                Global.currentPlugin.Instance.importDataFromExcel();
            }
            else
            {
                Global.myNwMainFrm.cmnCdMn.showMsg("No Active Module Yet!", 3);
            }
        }

        private void exportDataToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Global.currentPlugin != null)
            {
                Global.currentPlugin.Instance.exprtDataToExcel();
            }
            else
            {
                Global.myNwMainFrm.cmnCdMn.showMsg("No Active Module Yet!", 3);
            }
        }

        private void viewWordReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Global.currentPlugin != null)
            {
                Global.currentPlugin.Instance.creatWordReport();
            }
            else
            {
                Global.myNwMainFrm.cmnCdMn.showMsg("No Active Module Yet!", 4);
            }
        }
        #endregion

        #region "HELP MENU ITEMS EVENT HANDLERS..."
        private void operationalManualsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Global.myNwMainFrm.cmnCdMn.CheckForInternetConnection())
            {
                string strUrl = @"http://rhomicom.com/manual";
                bool error = false;
                try
                {
                    System.Diagnostics.Process.Start("chrome.exe", strUrl);
                }
                catch (Exception ex)
                {
                    error = true;
                }
                if (error)
                {
                    try
                    {
                        System.Diagnostics.Process.Start("firefox.exe", strUrl);
                    }
                    catch (Exception ex)
                    {
                        error = true;
                    }
                }
                if (error)
                {
                    try
                    {
                        System.Diagnostics.Process.Start("IEXPLORE.EXE", strUrl);
                    }
                    catch (Exception ex)
                    {
                        Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
                    }
                }
            }
            try
            {
                String strTitle = "Operational Manuals";
                if (this.FindDockedFormToActivate(strTitle) < 0)
                {
                    if (this.FindDockedFormExistence(strTitle) == false)
                    {
                        Global.refreshRqrdVrbls();
                        Manuals.Forms.mainForm nwMnFrm = new Manuals.Forms.mainForm();
                        nwMnFrm.lgn_num = Global.login_number;
                        nwMnFrm.role_st_id = Global.role_set_id;
                        nwMnFrm.usr_id = Global.usr_id;
                        nwMnFrm.Og_id = Global.org_id;

                        //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.Role_Set_IDs;
                        WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
                        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        //selectedPlugin.Instance.loadMyRolesNMsgtyps();
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
            }
        }

        private void contentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(System.Windows.Forms.Application.StartupPath + @"\htmls\ROMS SOFTWARE MANUAL.pdf");
            }
            catch
            {
                MessageBox.Show("Could not find the help file!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void aboutRhomicomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process myprcss = System.Diagnostics.Process.Start("http://www.rhomicomgh.com/");
            //myprcss.Kill();
            try
            {
                System.Diagnostics.Process.Start(System.Windows.Forms.Application.StartupPath + @"\htmls\about_us\ABOUT_RHOMICOM.exe");
            }
            catch
            {
                MessageBox.Show("Could not find the about us programme!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region "DB CONNECTION/DISCONNECTION FUNCTIONS..."
        private void connectDB_Actions()
        {
            /* 1. Show the Connect Dialog Box
             * 2. Read in the default parameters from a file into the textboxes in the dialog form
             * 3. Use Data in the Textboxes to form a connection string 
             * 4. Use this conn string to open the connection
             * 5. If Connection is successful, save the connection info in an encrypted file
             * 6. Update the various status bar labels and home Page Labels
             * 7. Enable the Timers that will continue to update the labels every second
             * 8. Start the Login to Database Dialog Form i.e. Call loginMenuitem Click
             */
            //connectDiag nwDiag = new connectDiag();
            ////System.Windows.Forms.Application.DoEvents();
            //DialogResult dgRes = nwDiag.ShowDialog();
            Global.homeFrm.dsplayInfoPanel.Visible = false;
            Global.homeFrm.loginPanel.Visible = false;
            Global.homeFrm.connectDBPanel.Dock = DockStyle.Fill;
            Global.homeFrm.connectDBPanel.Visible = true;
            Global.homeFrm.loadConnectDiag();
            this.timer1.Interval = 1000;
            this.timer1.Enabled = true;
            //if (dgRes == DialogResult.OK)
            //{

            //}
            //else
            //{
            //    this.Close();
            //}
        }

        private void disconnectDB_Actns()
        {
            //string[] srvr = {"","","","","","0" };
            //srvr = Global.myNwMainFrm.cmnCdMn.getFTPServerDet();
            if (CommonCode.CommonCodes.DatabaseNm != "")
            {
                this.logoutActions();
                CommonCode.CommonCodes.GlobalSQLConn.Close();
                CommonCode.CommonCodes.ConnStr = "";
                CommonCode.CommonCodes.DatabaseNm = "";
                Global.db_server = "";
                Global.db_name = "";
                CommonCode.CommonCodes.Db_host = "";
                CommonCode.CommonCodes.Db_port = "";
                CommonCode.CommonCodes.Db_dbase = "";
                CommonCode.CommonCodes.Db_uname = "";
                CommonCode.CommonCodes.Db_pwd = "";
                this.changeBackground();
            }
            else
            {
                CommonCode.CommonCodes.ConnStr = "";
                CommonCode.CommonCodes.DatabaseNm = "";
                Global.db_server = "";
                Global.db_name = "";
                CommonCode.CommonCodes.Db_host = "";
                CommonCode.CommonCodes.Db_port = "";
                CommonCode.CommonCodes.Db_dbase = "";
                CommonCode.CommonCodes.Db_uname = "";
                CommonCode.CommonCodes.Db_pwd = "";
                this.changeBackground();
            }
            this.isDsconnet = true;
            this.updateDBLabels();
            this.updateLoginLabels();
            this.isDsconnet = false;
            Global.myNwMainFrm.statusLoadLabel.Visible = false;
            Global.myNwMainFrm.statusLoadPictureBox.Visible = false;
        }
        public bool connectionFailed = false;

        public void enableTimer()
        {
            this.updtLabelsTimer.Interval = Global.myNwMainFrm.cmnCdMn.AutoRfrshTime;
            this.updtLabelsTimer.Enabled = Global.myNwMainFrm.cmnCdMn.AutoRfrsh;
        }

        public void updateDBLabels()
        {
            try
            {
                if (this.isDsconnet == false && this.connectionFailed == false)
                {
                    CommonCode.CommonCodes.GlobalSQLConn.ConnectionString = CommonCode.CommonCodes.ConnStr;
                    CommonCode.CommonCodes.GlobalSQLConn.Open();
                }
            }
            catch (Exception ex)
            {
            }
            try
            {
                this.appVersionStatusLabel.Text = CommonCode.CommonCodes.AppName + " " + CommonCode.CommonCodes.AppVersion;
                if (CommonCode.CommonCodes.DatabaseNm != "")
                {
                    CommonCode.CommonCodes.Bsc_prsn_name = Global.myNwMainFrm.cmnCdMn.getEnbldPssblValDesc("Basic Person Data",
                        Global.myNwMainFrm.cmnCdMn.getEnbldLovID("Customized Module Names"));

                    if (CommonCode.CommonCodes.Bsc_prsn_name == "")
                    {
                        CommonCode.CommonCodes.Bsc_prsn_name = "Basic Person Data";
                    }

                    CommonCode.CommonCodes.Intrnl_pymnts_name = Global.myNwMainFrm.cmnCdMn.getEnbldPssblValDesc("Internal Payments",
                        Global.myNwMainFrm.cmnCdMn.getEnbldLovID("Customized Module Names"));

                    if (CommonCode.CommonCodes.Intrnl_pymnts_name == "")
                    {
                        CommonCode.CommonCodes.Intrnl_pymnts_name = "Internal Payments";
                    }

                    CommonCode.CommonCodes.Learning_name = Global.myNwMainFrm.cmnCdMn.getEnbldPssblValDesc("Learning/Performance Management",
                        Global.myNwMainFrm.cmnCdMn.getEnbldLovID("Customized Module Names"));

                    if (CommonCode.CommonCodes.Learning_name == "")
                    {
                        CommonCode.CommonCodes.Learning_name = "Learning/Performance Management";
                    }

                    CommonCode.CommonCodes.Hospitality_name = Global.myNwMainFrm.cmnCdMn.getEnbldPssblValDesc("Hospitality Management",
                        Global.myNwMainFrm.cmnCdMn.getEnbldLovID("Customized Module Names"));

                    if (CommonCode.CommonCodes.Hospitality_name == "")
                    {
                        CommonCode.CommonCodes.Hospitality_name = "Hospitality Management";
                    }

                    CommonCode.CommonCodes.Appointments_name = Global.myNwMainFrm.cmnCdMn.getEnbldPssblValDesc("Visits And Appointments",
                        Global.myNwMainFrm.cmnCdMn.getEnbldLovID("Customized Module Names"));

                    if (CommonCode.CommonCodes.Appointments_name == "")
                    {
                        CommonCode.CommonCodes.Appointments_name = "Visits and Appointments";
                    }

                    CommonCode.CommonCodes.Proj_mgmnt_name = Global.myNwMainFrm.cmnCdMn.getEnbldPssblValDesc("Project Management",
                        Global.myNwMainFrm.cmnCdMn.getEnbldLovID("Customized Module Names"));

                    if (CommonCode.CommonCodes.Proj_mgmnt_name == "")
                    {
                        CommonCode.CommonCodes.Proj_mgmnt_name = "Projects Management";
                    }

                    CommonCode.CommonCodes.Events_name = Global.myNwMainFrm.cmnCdMn.getEnbldPssblValDesc("Events and Attendance",
                        Global.myNwMainFrm.cmnCdMn.getEnbldLovID("Customized Module Names"));

                    if (CommonCode.CommonCodes.Events_name == "")
                    {
                        CommonCode.CommonCodes.Events_name = "Events and Attendance";
                    }

                    CommonCode.CommonCodes.Store_inventory = Global.myNwMainFrm.cmnCdMn.getEnbldPssblValDesc("Sales and Inventory",
                        Global.myNwMainFrm.cmnCdMn.getEnbldLovID("Customized Module Names"));

                    if (CommonCode.CommonCodes.Store_inventory == "")
                    {
                        CommonCode.CommonCodes.Store_inventory = "Sales and Inventory";
                    }
                    this.connectStatusLabel.Text = "Connected!";
                    this.dbServerToolStripStatusLabel.Text = Global.db_server;
                    this.dbNameToolStripStatusLabel.Text = Global.db_name +
                      " (" + Global.myNwMainFrm.cmnCdMn.getUsername(Global.usr_id) + ")";
                    if (this.dbNameToolStripStatusLabel.Text.Length > 25)
                    {
                        this.dbNameToolStripStatusLabel.Text = this.dbNameToolStripStatusLabel.Text.Substring(0, 25);
                    }

                    if (Global.db_name.Contains("test")
                  || Global.db_name.Contains("try")
                  || Global.db_name.Contains("trial")
                  || Global.db_name.Contains("train")
                  || Global.db_name.Contains("sample"))
                    {
                        this.dbServerToolStripStatusLabel.BackColor = Color.Red;
                        this.dbNameToolStripStatusLabel.BackColor = Color.Red;
                    }
                    else
                    {
                        this.dbServerToolStripStatusLabel.BackColor = Color.Lime;
                        this.dbNameToolStripStatusLabel.BackColor = Color.Lime;
                    }
                    this.dbTimeToolStripStatusLabel.Text = DateTime.ParseExact(Global.myNwMainFrm.cmnCdMn.getDB_Date_time(),
                      "yyyy-MM-dd HH:mm:ss",
                        System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy HH:mm:ss");//hh:mm:ss tt
                    this.connectToDatabaseToolStripMenuItem.Text = "Disconnect from Database";
                    this.connectToDatabaseToolStripMenuItem.Image = Enterprise_Management_System.Properties.Resources.dscnt;
                    if (Global.homeFrm != null)
                    {
                        /*String neededMdls = "";
                        if (Global.myNwMainFrm.cmnCdMn.User_id > 0)
                        {
                            neededMdls = Global.myNwMainFrm.cmnCdMn.getGnrlRecNm("sec.sec_users", "user_id", "modules_needed", Global.myNwMainFrm.cmnCdMn.User_id);
                            if ((!neededMdls.Contains("Only") && !neededMdls.Contains("Modules")) || neededMdls == "")
                            {
                                int lvid = Global.myNwMainFrm.cmnCdMn.getLovID("Rhomicom Software Licenses");
                                neededMdls = Global.myNwMainFrm.cmnCdMn.decrypt(Global.myNwMainFrm.cmnCdMn.getEnbldPssblValDesc("Modules/Packages Needed", lvid), CommonCode.CommonCodes.AppKey);
                                if (neededMdls.Contains("Only") || neededMdls.Contains("Modules"))
                                {
                                    CommonCode.CommonCodes.ModulesNeeded = neededMdls;
                                }
                                else
                                {
                                    CommonCode.CommonCodes.ModulesNeeded = "Person Records Only";
                                }
                            }
                            else
                            {
                                CommonCode.CommonCodes.ModulesNeeded = neededMdls;
                            }
                        }
                        else
                        {
                            CommonCode.CommonCodes.ModulesNeeded = "Person Records Only";
                        }
                        Global.homeFrm.checkAllwdModules();*/
                        Global.homeFrm.prsnDataButton.Text = CommonCode.CommonCodes.Bsc_prsn_name.ToUpper();
                        Global.homeFrm.paymntButton.Text = CommonCode.CommonCodes.Intrnl_pymnts_name.ToUpper();
                        Global.homeFrm.acadmcsButton.Text = CommonCode.CommonCodes.Learning_name.ToUpper();
                        Global.homeFrm.attndButton.Text = CommonCode.CommonCodes.Events_name.ToUpper();
                        Global.homeFrm.hospitalityButton.Text = CommonCode.CommonCodes.Hospitality_name.ToUpper();
                        Global.homeFrm.invButton.Text = CommonCode.CommonCodes.Store_inventory.ToUpper();
                        Global.homeFrm.projectMgmntButton.Text = CommonCode.CommonCodes.Proj_mgmnt_name.ToUpper();
                        Global.homeFrm.appointmentsButton.Text = CommonCode.CommonCodes.Appointments_name.ToUpper();

                        Global.homeFrm.connectButton.ImageKey = "dscnt.png";
                        Global.homeFrm.connectButton.Text = "DISCONNECT FROM DATABASE";
                        //Global.homeFrm.connectLabel.Text = "Connected!";
                        //Global.homeFrm.hostLabel.Text = Global.db_server;
                        //Global.homeFrm.dbNameLabel.Text = Global.db_name;
                        Global.homeFrm.dbServerDateLabel.Text = DateTime.ParseExact(
                          Global.myNwMainFrm.cmnCdMn.getDB_Date_time(), "yyyy-MM-dd HH:mm:ss",
                          System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy HH:mm:ss");
                        Global.homeFrm.dbServerTimeLabel.Text = DateTime.ParseExact(
                          Global.myNwMainFrm.cmnCdMn.getDB_Date_time(), "yyyy-MM-dd HH:mm:ss",
                          System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm:ss tt");
                    }
                    this.loginToolStripMenuItem.Enabled = true;
                    //CommonCode.CommonCodes.GlobalSQLConn.Close();
                }
                else
                {

                    this.dbServerToolStripStatusLabel.BackColor = Color.LightGray;
                    this.dbNameToolStripStatusLabel.BackColor = Color.LightGray;

                    this.connectStatusLabel.Text = "Disconnected!";
                    this.dbServerToolStripStatusLabel.Text = "Unknown";
                    this.dbNameToolStripStatusLabel.Text = "Unknown";
                    this.dbTimeToolStripStatusLabel.Text = "Unknown";
                    Global.db_server = "";
                    Global.db_name = "";
                    this.connectToDatabaseToolStripMenuItem.Text = "Connect to Database";
                    this.connectToDatabaseToolStripMenuItem.Image = Enterprise_Management_System.Properties.Resources.network_48;
                    CommonCode.CommonCodes.ModulesNeeded = "Person Records Only";
                    if (Global.homeFrm != null)
                    {
                        Global.homeFrm.checkAllwdModules();
                        Global.homeFrm.connectButton.ImageKey = "network_48.png";
                        Global.homeFrm.connectButton.Text = "CONNECT TO DATABASE";
                        Global.homeFrm.userLabel.Text = "";
                        Global.homeFrm.userLogTimeLabel.Text = "";
                        Global.homeFrm.curRoleLabel.Text = "";
                        Global.homeFrm.dbServerDateLabel.Text = "";
                        Global.homeFrm.dbServerTimeLabel.Text = "";
                    }
                    this.loginToolStripMenuItem.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showSQLNoPermsn(ex.Message + "\r\n" + ex.StackTrace);
            }
        }
        #endregion

        #region "DB LOGIN/LOGOUT FUNCTIONS..."
        private void loginActions()
        {
            /* 1. Check if the admin account and admin role set exist
             * 1. Does Admin Account have admin role
             * 2. Create them if they don't
             * 3. Show the Login Dialog Box
             * 4. if Login credentials are correct then update successfullogicvbns, else update failed logins
             * 5. also update homepage labels and menu item texts/icons
             * 6. Call the select role set click procedure
            */

            //loginDiag nwDiag = new loginDiag();
            //DialogResult dgRes = nwDiag.ShowDialog();
            Global.homeFrm.dsplayInfoPanel.Visible = false;
            Global.homeFrm.connectDBPanel.Visible = false;
            Global.homeFrm.loginPanel.Dock = DockStyle.Fill;
            Global.homeFrm.loginPanel.Visible = true;
            Global.homeFrm.uname1TextBox.Focus();
            Global.homeFrm.uname1TextBox.SelectAll();
            Global.myNwMainFrm.statusLoadLabel.Visible = false;
            Global.myNwMainFrm.statusLoadPictureBox.Visible = false;
            /**/
        }

        public void logoutActions()
        {
            /* 1. Close All Open Forms
             * 2. update databse for user's logout time
             * 3. set Global values to 0 or null
             */
            if (Global.homeFrm != null)
            {
                this.closeAllDockedFormsExcpt(Global.homeFrm.TabText);
            }
            else
            {
                this.closeAllDockedFormsExcpt("Home Page");
            }
            Global.storeLogoutTime(Global.login_number);
            Global.usr_id = (-1);
            Global.role_set_id = new int[0];
            Global.login_number = (-1);
            Global.moduleFuncs.CloseModules();
            Global.refreshRqrdVrbls();
            this.updateLoginLabels();
            Global.homeFrm.uname1TextBox.Text = "";
            Global.homeFrm.pwd1TextBox.Text = "";
            Global.myNwMainFrm.statusLoadLabel.Visible = false;
            Global.myNwMainFrm.statusLoadPictureBox.Visible = false;


            Global.homeFrm.dsplayInfoPanel.Visible = false;
            Global.homeFrm.connectDBPanel.Visible = false;
            Global.homeFrm.loginPanel.Dock = DockStyle.Fill;
            Global.homeFrm.loginPanel.Visible = true;
            Global.homeFrm.uname1TextBox.Focus();
            Global.homeFrm.uname1TextBox.SelectAll();
            Global.myNwMainFrm.statusLoadLabel.Visible = false;
            Global.myNwMainFrm.statusLoadPictureBox.Visible = false;
        }

        public void updateLoginLabels()
        {
            try
            {
                if (this.isDsconnet == false && this.connectionFailed == false)
                {
                    CommonCode.CommonCodes.GlobalSQLConn.ConnectionString = CommonCode.CommonCodes.ConnStr;
                    CommonCode.CommonCodes.GlobalSQLConn.Open();
                }
            }
            catch (Exception ex)
            {
            }
            try
            {
                string orgSlogan = "Rhomicom...Building Dreams...";

                if (Global.login_number > 0 && CommonCode.CommonCodes.DatabaseNm != "")
                {
                    this.basicSetupToolStripMenuItem.Enabled = true;
                    this.basicSetupToolStripMenuItem.Visible = true;
                    this.specializedModulesToolStripMenuItem.Enabled = true;
                    this.specializedModulesToolStripMenuItem.Visible = true;
                    this.customModulesToolStripMenuItem.Enabled = true;
                    this.customModulesToolStripMenuItem.Visible = true;
                    this.hospitalityMngmntMenuItem.Visible = true;
                    this.academicsMenuItem.Visible = true;
                    this.visitsAndAppointmentsToolStripMenuItem.Visible = true;
                    this.projectManagementToolStripMenuItem.Visible = true;
                    this.eventsMenuItem.Visible = true;
                    this.accountingToolStripMenuItem.Visible = true;
                    this.basicPersonDataToolStripMenuItem.Visible = true;
                    this.internalPaymentsToolStripMenuItem.Visible = true;
                    this.storesInventoryToolStripMenuItem.Visible = true;
                    String neededMdls = "";
                    if (Global.myNwMainFrm.cmnCdMn.User_id > 0)
                    {
                        neededMdls = Global.myNwMainFrm.cmnCdMn.getGnrlRecNm("sec.sec_users", "user_id", "modules_needed", Global.myNwMainFrm.cmnCdMn.User_id);
                        if ((!neededMdls.Contains("Only") && !neededMdls.Contains("Modules")) || neededMdls == "")
                        {
                            int lvid = Global.myNwMainFrm.cmnCdMn.getLovID("Rhomicom Software Licenses");
                            neededMdls = Global.myNwMainFrm.cmnCdMn.decrypt(Global.myNwMainFrm.cmnCdMn.getEnbldPssblValDesc("Modules/Packages Needed", lvid), CommonCode.CommonCodes.AppKey);
                            if (neededMdls.Contains("Only") || neededMdls.Contains("Modules"))
                            {
                                CommonCode.CommonCodes.ModulesNeeded = neededMdls;
                            }
                            else
                            {
                                CommonCode.CommonCodes.ModulesNeeded = "Person Records Only";
                            }
                        }
                        else
                        {
                            CommonCode.CommonCodes.ModulesNeeded = neededMdls;
                        }
                    }
                    else
                    {
                        CommonCode.CommonCodes.ModulesNeeded = "Person Records Only";
                    }

                    if (CommonCode.CommonCodes.ModulesNeeded != "All Modules")
                    {
                        if (CommonCode.CommonCodes.ModulesNeeded == "Person Records Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = false;
                            this.specializedModulesToolStripMenuItem.Visible = false;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = false;
                            this.accountingToolStripMenuItem.Visible = false;
                            this.basicPersonDataToolStripMenuItem.Visible = true;
                            this.internalPaymentsToolStripMenuItem.Visible = false;
                            this.storesInventoryToolStripMenuItem.Visible = false;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Point of Sale Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = false;
                            this.specializedModulesToolStripMenuItem.Visible = false;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = false;
                            this.accountingToolStripMenuItem.Visible = true;
                            this.basicPersonDataToolStripMenuItem.Visible = true;
                            this.internalPaymentsToolStripMenuItem.Visible = false;
                            this.storesInventoryToolStripMenuItem.Visible = true;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Accounting Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = false;
                            this.specializedModulesToolStripMenuItem.Visible = false;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = false;
                            this.accountingToolStripMenuItem.Visible = true;
                            this.basicPersonDataToolStripMenuItem.Visible = true;
                            this.internalPaymentsToolStripMenuItem.Visible = false;
                            this.storesInventoryToolStripMenuItem.Visible = false;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Person Records with Accounting Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = false;
                            this.specializedModulesToolStripMenuItem.Visible = false;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = false;
                            this.accountingToolStripMenuItem.Visible = true;
                            this.basicPersonDataToolStripMenuItem.Visible = true;
                            this.internalPaymentsToolStripMenuItem.Visible = false;
                            this.storesInventoryToolStripMenuItem.Visible = false;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Person Records + Hospitality Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = false;
                            this.accountingToolStripMenuItem.Visible = false;
                            this.basicPersonDataToolStripMenuItem.Visible = true;
                            this.internalPaymentsToolStripMenuItem.Visible = false;
                            this.storesInventoryToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = true;
                            this.academicsMenuItem.Visible = false;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = false;
                            this.projectManagementToolStripMenuItem.Visible = false;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Person Records + Events Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = true;
                            this.accountingToolStripMenuItem.Visible = false;
                            this.basicPersonDataToolStripMenuItem.Visible = true;
                            this.internalPaymentsToolStripMenuItem.Visible = false;
                            this.storesInventoryToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = false;
                            this.academicsMenuItem.Visible = false;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = false;
                            this.projectManagementToolStripMenuItem.Visible = false;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Sales with Accounting Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = false;
                            this.specializedModulesToolStripMenuItem.Visible = false;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = false;
                            this.accountingToolStripMenuItem.Visible = true;
                            this.basicPersonDataToolStripMenuItem.Visible = true;
                            this.internalPaymentsToolStripMenuItem.Visible = false;
                            this.storesInventoryToolStripMenuItem.Visible = true;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Accounting with Payroll Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = false;
                            this.specializedModulesToolStripMenuItem.Visible = false;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = false;
                            this.accountingToolStripMenuItem.Visible = true;
                            this.basicPersonDataToolStripMenuItem.Visible = true;
                            this.internalPaymentsToolStripMenuItem.Visible = true;
                            this.storesInventoryToolStripMenuItem.Visible = false;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Basic Modules Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = false;
                            this.specializedModulesToolStripMenuItem.Visible = false;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = false;
                            this.accountingToolStripMenuItem.Visible = true;
                            this.basicPersonDataToolStripMenuItem.Visible = true;
                            this.internalPaymentsToolStripMenuItem.Visible = true;
                            this.storesInventoryToolStripMenuItem.Visible = true;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Basic Modules + Hospitality Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = true;
                            this.academicsMenuItem.Visible = false;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = false;
                            this.projectManagementToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = false;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Basic Modules + Events Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = false;
                            this.academicsMenuItem.Visible = false;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = false;
                            this.projectManagementToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = true;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Basic Modules + Projects Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = false;
                            this.academicsMenuItem.Visible = false;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = false;
                            this.projectManagementToolStripMenuItem.Visible = true;
                            this.eventsMenuItem.Visible = false;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Basic Modules + Appointments Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = false;
                            this.academicsMenuItem.Visible = false;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = true;
                            this.projectManagementToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = false;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Basic Modules + PMS Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = false;
                            this.academicsMenuItem.Visible = true;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = false;
                            this.projectManagementToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = false;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Basic Modules + Events + Hospitality Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = true;
                            this.academicsMenuItem.Visible = false;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = false;
                            this.projectManagementToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = true;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Basic Modules - Payroll - Person Records + Events + Hospitality Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = true;
                            this.academicsMenuItem.Visible = false;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = false;
                            this.projectManagementToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = true;
                            this.internalPaymentsToolStripMenuItem.Visible = false;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Basic Modules + Payroll - Person Records + Events + Hospitality Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = true;
                            this.academicsMenuItem.Visible = false;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = false;
                            this.projectManagementToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = true;
                            this.internalPaymentsToolStripMenuItem.Visible = true;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Basic Modules + Events + PMS Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = false;
                            this.academicsMenuItem.Visible = true;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = false;
                            this.projectManagementToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = true;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Basic Modules + Projects + PMS Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = false;
                            this.academicsMenuItem.Visible = true;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = false;
                            this.projectManagementToolStripMenuItem.Visible = true;
                            this.eventsMenuItem.Visible = false;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Basic Modules + Projects + Events Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = false;
                            this.academicsMenuItem.Visible = false;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = false;
                            this.projectManagementToolStripMenuItem.Visible = true;
                            this.eventsMenuItem.Visible = true;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Basic Modules + Projects + Hospitality Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = true;
                            this.academicsMenuItem.Visible = false;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = false;
                            this.projectManagementToolStripMenuItem.Visible = true;
                            this.eventsMenuItem.Visible = false;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Basic Modules + Events + Hospitality + PMS Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = true;
                            this.academicsMenuItem.Visible = true;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = false;
                            this.projectManagementToolStripMenuItem.Visible = false;
                            this.eventsMenuItem.Visible = true;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Basic Modules + Projects + Hospitality + PMS Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = true;
                            this.academicsMenuItem.Visible = true;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = false;
                            this.projectManagementToolStripMenuItem.Visible = true;
                            this.eventsMenuItem.Visible = false;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Basic Modules + Events + Projects + Hospitality Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = true;
                            this.academicsMenuItem.Visible = false;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = false;
                            this.projectManagementToolStripMenuItem.Visible = true;
                            this.eventsMenuItem.Visible = true;
                        }
                        else if (CommonCode.CommonCodes.ModulesNeeded == "Basic Modules + Events + Projects + Hospitality + PMS Only")
                        {
                            this.specializedModulesToolStripMenuItem.Enabled = true;
                            this.specializedModulesToolStripMenuItem.Visible = true;
                            this.customModulesToolStripMenuItem.Enabled = false;
                            this.customModulesToolStripMenuItem.Visible = false;
                            this.hospitalityMngmntMenuItem.Visible = true;
                            this.academicsMenuItem.Visible = true;
                            this.visitsAndAppointmentsToolStripMenuItem.Visible = false;
                            this.projectManagementToolStripMenuItem.Visible = true;
                            this.eventsMenuItem.Visible = true;
                        }
                    }
                    if (Global.myNwMainFrm.cmnCdMn.getModuleID("System Administration") > 0)
                    {
                        this.systemAdministrationToolStripMenuItem.Visible = Global.myNwMainFrm.cmnCdMn.test_prmssns("View System Administration", "System Administration");
                        this.generalSetupToolStripMenuItem.Visible = Global.myNwMainFrm.cmnCdMn.test_prmssns("View General Setup", "General Setup");
                        this.organisationSetupToolStripMenuItem.Visible = Global.myNwMainFrm.cmnCdMn.test_prmssns("View Organization Setup", "Organization Setup");
                        this.reportsAndProcessesToolStripMenuItem.Visible = Global.myNwMainFrm.cmnCdMn.test_prmssns("View Reports And Processes", "Reports And Processes");
                        this.dBConfigToolStripMenuItem.Visible = Global.myNwMainFrm.cmnCdMn.test_prmssns("View System Administration", "System Administration");
                    }
                    else
                    {
                        this.systemAdministrationToolStripMenuItem.Visible = true;
                        this.generalSetupToolStripMenuItem.Visible = true;
                        this.organisationSetupToolStripMenuItem.Visible = true;
                        this.reportsAndProcessesToolStripMenuItem.Visible = true;
                        this.dBConfigToolStripMenuItem.Visible = true;
                    }
                    this.loginToolStripMenuItem.Text = "&Logout";
                    this.loginToolStripMenuItem.Image = Enterprise_Management_System.Properties.Resources._49;
                    this.changeMyPasswordToolStripMenuItem.Enabled = true;
                    this.switchRoleSetToolStripMenuItem.Enabled = true;
                    this.myInboxToolStripMenuItem.Enabled = true;
                    this.localStorageMenuItem.Enabled = true;
                    this.selfServiceMoMenuItem.Enabled = true;
                    this.basicPersonDataToolStripMenuItem.Text = CommonCode.CommonCodes.Bsc_prsn_name;
                    this.internalPaymentsToolStripMenuItem.Text = CommonCode.CommonCodes.Intrnl_pymnts_name;
                    this.academicsMenuItem.Text = CommonCode.CommonCodes.Learning_name;
                    this.eventsMenuItem.Text = CommonCode.CommonCodes.Events_name;
                    this.hospitalityMngmntMenuItem.Text = CommonCode.CommonCodes.Hospitality_name;
                    this.visitsAndAppointmentsToolStripMenuItem.Text = CommonCode.CommonCodes.Appointments_name;
                    this.projectManagementToolStripMenuItem.Text = CommonCode.CommonCodes.Proj_mgmnt_name;
                    this.storesInventoryToolStripMenuItem.Text = CommonCode.CommonCodes.Store_inventory;
                    if (Global.homeFrm != null)
                    {
                        Global.homeFrm.checkAllwdModules();
                        Global.homeFrm.prsnDataButton.Text = CommonCode.CommonCodes.Bsc_prsn_name.ToUpper();
                        Global.homeFrm.paymntButton.Text = CommonCode.CommonCodes.Intrnl_pymnts_name.ToUpper();
                        Global.homeFrm.acadmcsButton.Text = CommonCode.CommonCodes.Learning_name.ToUpper();
                        Global.homeFrm.attndButton.Text = CommonCode.CommonCodes.Events_name.ToUpper();
                        Global.homeFrm.hospitalityButton.Text = CommonCode.CommonCodes.Hospitality_name.ToUpper();
                        Global.homeFrm.appointmentsButton.Text = CommonCode.CommonCodes.Appointments_name.ToUpper();
                        Global.homeFrm.projectMgmntButton.Text = CommonCode.CommonCodes.Proj_mgmnt_name.ToUpper();
                        Global.homeFrm.invButton.Text = CommonCode.CommonCodes.Store_inventory.ToUpper();

                        Global.homeFrm.userLabel.Text = Global.myNwMainFrm.cmnCdMn.get_user_name(Global.usr_id).ToUpper();
                        Global.homeFrm.userLogTimeLabel.Text = DateTime.Parse(
                          Global.get_last_login_time(Global.myNwMainFrm.cmnCdMn.get_user_name(Global.usr_id))).ToString("dd-MMM-yyyy hh:mm:ss tt");
                        Global.homeFrm.curRoleLabel.Text = "";
                        for (int i = 0; i < Global.role_set_id.Length; i++)
                        {
                            Global.homeFrm.curRoleLabel.Text = Global.homeFrm.curRoleLabel.Text +
                              ", " + Global.myNwMainFrm.cmnCdMn.get_role_name(Global.role_set_id[i]);
                        }
                        char[] rmvChr = { ' ', ',' };
                        Global.homeFrm.curRoleLabel.Text = Global.homeFrm.curRoleLabel.Text.Trim(rmvChr);
                        Global.homeFrm.loginButton.Text = "&LOGOUT";
                        Global.homeFrm.loginButton.ImageKey = "49.png";
                        Global.homeFrm.pictureBox3.Image.Dispose();
                        Global.homeFrm.pictureBox3.Image = Enterprise_Management_System.Properties.Resources._1;

                        //Global.org_id = Global.myNwMainFrm.cmnCdMn.getPrsnOrgID(Global.usr_id);
                        //Global.homeFrm.pictureBox1.Image = Global.homeFrm.pictureBox3.Image;


                        if (Global.org_id > 0)
                        {
                            Global.homeFrm.label1.Text = Global.myNwMainFrm.cmnCdMn.getOrgName(Global.org_id);
                            Global.myNwMainFrm.cmnCdMn.getDBImageFile(Global.org_id.ToString() + ".png", 0, ref Global.homeFrm.pictureBox3);

                            string orgType = Global.myNwMainFrm.cmnCdMn.getPssblValNm(
                                int.Parse(Global.myNwMainFrm.cmnCdMn.getGnrlRecNm("org.org_details", "org_id", "org_typ_id", Global.org_id)));
                            if (orgType.ToUpper().Contains("MARKET") || orgType.ToUpper().Contains("MART")
                              || orgType.ToUpper().Contains("STORE") || orgType.ToUpper().Contains("SHOP")
                              || orgType.ToUpper().Contains("BOUTIQUE"))
                            {
                                Global.homeFrm.invButton.Focus();
                            }
                            else if (orgType.ToUpper().Contains("HOTEL") || orgType.ToUpper().Contains("HOSTEL")
                               || orgType.ToUpper().Contains("GUEST") || orgType.ToUpper().Contains("LODGE")
                               || orgType.ToUpper().Contains("HOSPITALITY")
                              || orgType.ToUpper().Contains("RESTAURANT"))
                            {
                                Global.homeFrm.hospitalityButton.Focus();
                            }
                            else
                            {
                                Global.homeFrm.prsnDataButton.Focus();
                            }
                            //else if (orgType.ToUpper().Contains("CLINIC") || orgType.ToUpper().Contains("HOSPITAL")
                            //   || orgType.ToUpper().Contains("MEDICAL"))
                            //{
                            //  Global.homeFrm.clinicButton.Visible = true;
                            //  this.clinicHospitalManagementToolStripMenuItem.Visible = true;
                            //  Global.homeFrm.clinicButton.Focus();
                            //}
                            //else if (orgType.ToUpper().Contains("BANKING") || orgType.ToUpper().Contains("MICROFINANCE")
                            //   || orgType.ToUpper().Contains("SUSU") || orgType.ToUpper().Contains("LOANS"))
                            //{
                            //  Global.homeFrm.bankingButton.Visible = true;
                            //  this.bnkMicroMenuItem.Visible = true;
                            //  Global.homeFrm.bankingButton.Focus();
                            //}
                        }
                        //Global.homeFrm.mdlPanel.Visible = true;
                        //Global.homeFrm.avlbMdlsListView.Visible = true;
                        //Global.homeFrm.avlbMdlsListView.BringToFront();
                        //Global.homeFrm.label4.Text = "Available Modules:";
                        //Global.homeFrm.Label3.Visible = false;
                        //Global.homeFrm.label7.Visible = false;
                        //Global.homeFrm.populateModulesLstVw();

                    }
                    this.Text = Global.myNwMainFrm.cmnCdMn.getOrgName(Global.org_id);
                    this.bannerGlsLabel.Caption = this.Text;
                    orgSlogan = Global.myNwMainFrm.cmnCdMn.getOrgSlogan(Global.org_id);
                    if (orgSlogan == "")
                    {
                        orgSlogan = "Rhomicom...Building Dreams...";
                    }
                    if (Global.homeFrm != null)
                    {
                        Global.homeFrm.sloganLabel.Text = orgSlogan;
                    }
                    if (Global.org_id <= 0)
                    {
                        this.Text = CommonCode.CommonCodes.AppName + " " + CommonCode.CommonCodes.AppVersion;
                        this.bannerGlsLabel.Caption = "Rhomicom Systems Technologies Ltd.".ToUpper();
                        if (Global.homeFrm != null)
                        {
                            Global.homeFrm.pictureBox3.Image.Dispose();
                            Global.homeFrm.pictureBox3.Image = Enterprise_Management_System.Properties.Resources._1;

                            Global.homeFrm.label1.Text = ("WELCOME TO " + CommonCode.CommonCodes.AppName.ToUpper() + " " + CommonCode.CommonCodes.AppVersion);
                            //Global.homeFrm.orgGlsLabel.Caption = "Rhomicom Systems Technologies Ltd.";
                        }
                    }
                    //CommonCode.CommonCodes.GlobalSQLConn.Close();
                }
                else
                {
                    Global.usr_id = (-1);
                    Global.role_set_id = new int[0];
                    Global.login_number = (-1);
                    Global.org_id = -1;
                    this.basicSetupToolStripMenuItem.Enabled = false;
                    this.basicSetupToolStripMenuItem.Visible = false;
                    this.specializedModulesToolStripMenuItem.Enabled = false;
                    this.specializedModulesToolStripMenuItem.Visible = false;
                    this.customModulesToolStripMenuItem.Enabled = false;
                    this.customModulesToolStripMenuItem.Visible = false;
                    this.loginToolStripMenuItem.Text = "&Login";
                    this.loginToolStripMenuItem.Image = Enterprise_Management_System.Properties.Resources._53;
                    this.changeMyPasswordToolStripMenuItem.Enabled = false;
                    this.switchRoleSetToolStripMenuItem.Enabled = false;
                    this.myInboxToolStripMenuItem.Enabled = false;
                    this.localStorageMenuItem.Enabled = false;
                    this.selfServiceMoMenuItem.Enabled = false;
                    this.Text = "" + CommonCode.CommonCodes.AppName + " " + CommonCode.CommonCodes.AppVersion;
                    this.bannerGlsLabel.Caption = "Rhomicom Systems Technologies Ltd.".ToUpper();
                    if (Global.homeFrm != null)
                    {
                        CommonCode.CommonCodes.ModulesNeeded = "Person Records Only";
                        Global.myNwMainFrm.cmnCdMn.Login_number = -1;
                        Global.homeFrm.checkAllwdModules();
                        Global.homeFrm.sloganLabel.Text = orgSlogan;
                        Global.homeFrm.label15.Visible = true;
                        Global.homeFrm.label14.Visible = true;
                        Global.homeFrm.userLabel.Text = "";
                        Global.homeFrm.userLogTimeLabel.Text = "";
                        Global.homeFrm.curRoleLabel.Text = "";
                        Global.homeFrm.loginButton.Text = "&LOGIN";
                        Global.homeFrm.loginButton.ImageKey = "53.png";
                        Global.homeFrm.pictureBox3.Image.Dispose();
                        Global.homeFrm.pictureBox3.Image = Enterprise_Management_System.Properties.Resources._1;
                        //Global.homeFrm.pictureBox1.Image = Global.homeFrm.pictureBox3.Image;

                        Global.homeFrm.label1.Text = ("WELCOME TO " + CommonCode.CommonCodes.AppName.ToUpper() + " " + CommonCode.CommonCodes.AppVersion); //.PadRight(35).PadLeft(10)
                        //Global.homeFrm.orgGlsLabel.Caption = "Rhomicom Systems Technologies Ltd.";
                    }
                }
            }
            catch (Exception ex)
            {

                //Global.myNwMainFrm.statusLoadLabel.Visible = false;
                //Global.myNwMainFrm.statusLoadPictureBox.Visible = false;
                ////System.Windows.Forms.Application.DoEvents();
            }

        }
        #endregion

        private void custmseMenuItem_Click(object sender, EventArgs e)
        {
            customiseDiag nwDiag = new customiseDiag();
            DialogResult dg = nwDiag.ShowDialog();
            if (dg == DialogResult.OK)
            {
                if (Global.myNwMainFrm.cmnCdMn.showMsg("Would you like to Restart the Application\r\n for the Changes to take Effect?", 2) == DialogResult.Yes)
                {
                    this.restartToolStripMenuItem_Click(this.restartToolStripMenuItem, e);
                }
            }
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Application.Restart();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.Application.Restart();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.backgroundWorker1.ReportProgress(10);
            //this.statusLoadLabel.Visible = true;
            //this.statusLoadPictureBox.Visible = true;
            ////System.Windows.Forms.Application.DoEvents();

            //Global.moduleFuncs.reloadModules(Application.StartupPath + @"\Plugins");
            //Global.moduleFuncs.CreateModulePrvldgs();
            string Path = Application.StartupPath + @"\Plugins\Add_Ins";
            //this.basicSetupToolStripMenuItem.Enabled = false;
            //this.specializedModulesToolStripMenuItem.Enabled = false;
            //this.customModulesToolStripMenuItem.Enabled = false;
            //this.basicSetupToolStripMenuItem.Visible = false;
            //this.specializedModulesToolStripMenuItem.Visible = false;
            //this.customModulesToolStripMenuItem.Visible = false;
            if (Global.homeFrm != null)
            {
                Global.homeFrm.loadMdlsLabel.Visible = true;
                Global.homeFrm.progressBar1.Visible = true;
                //Global.homeFrm.avlbMdlsListView.Enabled = false;
            }
            Global.moduleFuncs.AvailableModules.Clear();
            int ttl = Directory.GetFiles(Path).Length;
            int i = 1;
            if (ttl > 0)
            {
                foreach (string fileOn in Directory.GetFiles(Path))
                {
                    i++;
                    FileInfo file = new FileInfo(fileOn);
                    if (file.Extension.Equals(".dll"))
                    {
                        Global.moduleFuncs.AddModule(fileOn);
                        this.backgroundWorker1.ReportProgress((int)((double)i / (double)ttl) * 100);
                    }
                }
            }
            this.backgroundWorker1.ReportProgress(100);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (Global.homeFrm != null)
            {
                Global.homeFrm.progressBar1.Value = e.ProgressPercentage;
            }

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(e.Error.Message + "\r\n" + e.Error.StackTrace + "\r\n" + e.Error.InnerException, 0);
            }
            this.basicSetupToolStripMenuItem.Enabled = true;
            this.specializedModulesToolStripMenuItem.Enabled = true;
            this.customModulesToolStripMenuItem.Enabled = true;
            this.basicSetupToolStripMenuItem.Visible = true;
            this.specializedModulesToolStripMenuItem.Visible = true;
            this.customModulesToolStripMenuItem.Visible = true;
            if (Global.homeFrm != null)
            {
                Global.homeFrm.loadMdlsLabel.Visible = false;
                Global.homeFrm.progressBar1.Visible = false;
                //Global.homeFrm.avlbMdlsListView.Enabled = true;
            }
            //this.statusLoadLabel.Visible = false;
            //this.statusLoadPictureBox.Visible = false;
            ////System.Windows.Forms.Application.DoEvents();

        }

        private void accountingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.myNwMainFrm.cmnCdMn.Login_number <= 0)
                {
                    Global.myNwMainFrm.cmnCdMn.showMsg("Please Login First!", 0);
                    return;
                }
                this.statusLoadLabel.Visible = true;
                this.statusLoadLabel.Location = new Point((Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Width / 2) - (int)((520 - this.statusLoadPictureBox.Width) / 2),
                (Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Height / 2) - (int)(1.25 * this.statusLoadPictureBox.Height));//
                this.statusLoadPictureBox.Visible = true;
                this.statusLoadPictureBox.Location = new Point(this.statusLoadLabel.Location.X - this.statusLoadPictureBox.Width, this.statusLoadLabel.Location.Y);
                //System.Windows.Forms.Application.DoEvents();

                String strTitle = "Accounting";
                if (this.FindDockedFormToActivate(strTitle) < 0)
                {
                    if (this.FindDockedFormExistence(strTitle) == false)
                    {
                        Global.refreshRqrdVrbls();
                        Accounting.Forms.mainForm nwMnFrm = new Accounting.Forms.mainForm();
                        nwMnFrm.lgn_num = Global.login_number;
                        nwMnFrm.role_st_id = Global.role_set_id;
                        nwMnFrm.usr_id = Global.usr_id;
                        nwMnFrm.Og_id = Global.org_id;

                        //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.Role_Set_IDs;
                        WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
                        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        //selectedPlugin.Instance.loadMyRolesNMsgtyps();
                    }
                    else
                    {
                    }
                }
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
        }

        private void basicPersonDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.myNwMainFrm.cmnCdMn.Login_number <= 0)
                {
                    Global.myNwMainFrm.cmnCdMn.showMsg("Please Login First!", 0);
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                this.statusLoadLabel.Visible = true;
                this.statusLoadLabel.Location = new Point((Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Width / 2) - (int)((520 - this.statusLoadPictureBox.Width) / 2),
                (Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Height / 2) - (int)(1.25 * this.statusLoadPictureBox.Height));//
                this.statusLoadPictureBox.Visible = true;
                this.statusLoadPictureBox.Location = new Point(this.statusLoadLabel.Location.X - this.statusLoadPictureBox.Width, this.statusLoadLabel.Location.Y);
                //System.Windows.Forms.Application.DoEvents();

                String strTitle = CommonCode.CommonCodes.Bsc_prsn_name;
                if (this.FindDockedFormToActivate(strTitle) < 0)
                {
                    if (this.FindDockedFormExistence(strTitle) == false)
                    {
                        Global.refreshRqrdVrbls();
                        BasicPersonData.Forms.mainForm nwMnFrm = new BasicPersonData.Forms.mainForm();
                        nwMnFrm.lgn_num = Global.login_number;
                        nwMnFrm.role_st_id = Global.role_set_id;
                        nwMnFrm.usr_id = Global.usr_id;
                        nwMnFrm.Og_id = Global.org_id;

                        //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.Role_Set_IDs;
                        WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
                        nwMainFrm.TabText = CommonCode.CommonCodes.Bsc_prsn_name;
                        nwMainFrm.Text = CommonCode.CommonCodes.Bsc_prsn_name;
                        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        //selectedPlugin.Instance.loadMyRolesNMsgtyps();
                    }
                    else
                    {
                    }
                }
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
                Cursor.Current = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void generalSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.myNwMainFrm.cmnCdMn.Login_number <= 0)
                {
                    Global.myNwMainFrm.cmnCdMn.showMsg("Please Login First!", 0);
                    return;
                }
                this.statusLoadLabel.Visible = true;
                this.statusLoadLabel.Location = new Point((Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Width / 2) - (int)((520 - this.statusLoadPictureBox.Width) / 2),
                (Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Height / 2) - (int)(1.25 * this.statusLoadPictureBox.Height));//
                this.statusLoadPictureBox.Visible = true;
                this.statusLoadPictureBox.Location = new Point(this.statusLoadLabel.Location.X - this.statusLoadPictureBox.Width, this.statusLoadLabel.Location.Y);
                //System.Windows.Forms.Application.DoEvents();

                String strTitle = "General Setup";
                if (this.FindDockedFormToActivate(strTitle) < 0)
                {
                    if (this.FindDockedFormExistence(strTitle) == false)
                    {
                        Global.refreshRqrdVrbls();
                        GeneralSetup.Forms.mainForm nwMnFrm = new GeneralSetup.Forms.mainForm();
                        nwMnFrm.lgn_num = Global.login_number;
                        nwMnFrm.role_st_id = Global.role_set_id;
                        nwMnFrm.usr_id = Global.usr_id;
                        nwMnFrm.Og_id = Global.org_id;

                        //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmmnCodeGstp.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.Role_Set_IDs;
                        WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
                        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        //selectedPlugin.Instance.loadMyRolesNMsgtyps();
                    }
                    else
                    {
                    }
                }
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
        }

        private void internalPaymentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.myNwMainFrm.cmnCdMn.Login_number <= 0)
                {
                    Global.myNwMainFrm.cmnCdMn.showMsg("Please Login First!", 0);
                    return;
                }
                this.statusLoadLabel.Visible = true;
                this.statusLoadLabel.Location = new Point((Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Width / 2) - (int)((520 - this.statusLoadPictureBox.Width) / 2),
                (Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Height / 2) - (int)(1.25 * this.statusLoadPictureBox.Height));//
                this.statusLoadPictureBox.Visible = true;
                this.statusLoadPictureBox.Location = new Point(this.statusLoadLabel.Location.X - this.statusLoadPictureBox.Width, this.statusLoadLabel.Location.Y);
                //System.Windows.Forms.Application.DoEvents();

                String strTitle = CommonCode.CommonCodes.Intrnl_pymnts_name;
                if (this.FindDockedFormToActivate(strTitle) < 0)
                {
                    if (this.FindDockedFormExistence(strTitle) == false)
                    {
                        Global.refreshRqrdVrbls();
                        InternalPayments.Forms.mainForm nwMnFrm = new InternalPayments.Forms.mainForm();
                        nwMnFrm.lgn_num = Global.login_number;
                        nwMnFrm.role_st_id = Global.role_set_id;
                        nwMnFrm.usr_id = Global.usr_id;
                        nwMnFrm.Og_id = Global.org_id;

                        //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.Role_Set_IDs;
                        WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
                        nwMainFrm.TabText = CommonCode.CommonCodes.Intrnl_pymnts_name;
                        nwMainFrm.Text = CommonCode.CommonCodes.Intrnl_pymnts_name;
                        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        //selectedPlugin.Instance.loadMyRolesNMsgtyps();
                    }
                    else
                    {
                    }
                }
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
        }

        private void organisationSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.myNwMainFrm.cmnCdMn.Login_number <= 0)
                {
                    Global.myNwMainFrm.cmnCdMn.showMsg("Please Login First!", 0);
                    return;
                }
                this.statusLoadLabel.Visible = true;
                this.statusLoadLabel.Location = new Point((Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Width / 2) - (int)((520 - this.statusLoadPictureBox.Width) / 2),
                (Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Height / 2) - (int)(1.25 * this.statusLoadPictureBox.Height));//
                this.statusLoadPictureBox.Visible = true;
                this.statusLoadPictureBox.Location = new Point(this.statusLoadLabel.Location.X - this.statusLoadPictureBox.Width, this.statusLoadLabel.Location.Y);
                //System.Windows.Forms.Application.DoEvents();

                String strTitle = "Organization Setup";
                if (this.FindDockedFormToActivate(strTitle) < 0)
                {
                    if (this.FindDockedFormExistence(strTitle) == false)
                    {
                        Global.refreshRqrdVrbls();
                        OrganizationSetup.Forms.mainForm nwMnFrm = new OrganizationSetup.Forms.mainForm();
                        nwMnFrm.lgn_num = Global.login_number;
                        nwMnFrm.role_st_id = Global.role_set_id;
                        nwMnFrm.usr_id = Global.usr_id;
                        nwMnFrm.Og_id = Global.org_id;

                        //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.Role_Set_IDs;
                        WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
                        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        //selectedPlugin.Instance.loadMyRolesNMsgtyps();
                    }
                    else
                    {
                    }
                }
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
        }

        private void reportsAndProcessesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.myNwMainFrm.cmnCdMn.Login_number <= 0)
                {
                    Global.myNwMainFrm.cmnCdMn.showMsg("Please Login First!", 0);
                    return;
                }
                this.statusLoadLabel.Visible = true;
                this.statusLoadLabel.Location = new Point((Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Width / 2) - (int)((520 - this.statusLoadPictureBox.Width) / 2),
                (Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Height / 2) - (int)(1.25 * this.statusLoadPictureBox.Height));//
                this.statusLoadPictureBox.Visible = true;
                this.statusLoadPictureBox.Location = new Point(this.statusLoadLabel.Location.X - this.statusLoadPictureBox.Width, this.statusLoadLabel.Location.Y);
                //System.Windows.Forms.Application.DoEvents();

                String strTitle = "Reports And Processes";
                if (this.FindDockedFormToActivate(strTitle) < 0)
                {
                    if (this.FindDockedFormExistence(strTitle) == false)
                    {
                        Global.refreshRqrdVrbls();
                        ReportsAndProcesses.Forms.mainForm nwMnFrm = new ReportsAndProcesses.Forms.mainForm();
                        nwMnFrm.lgn_num = Global.login_number;
                        nwMnFrm.role_st_id = Global.role_set_id;
                        nwMnFrm.usr_id = Global.usr_id;
                        nwMnFrm.Og_id = Global.org_id;

                        //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.Role_Set_IDs;
                        WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
                        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        //selectedPlugin.Instance.loadMyRolesNMsgtyps();
                    }
                    else
                    {
                    }
                }
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
        }

        private void systemAdministrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.myNwMainFrm.cmnCdMn.Login_number <= 0)
                {
                    Global.myNwMainFrm.cmnCdMn.showMsg("Please Login First!", 0);
                    return;
                }
                this.statusLoadLabel.Visible = true;
                this.statusLoadLabel.Location = new Point((Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Width / 2) - (int)((520 - this.statusLoadPictureBox.Width) / 2),
                (Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Height / 2) - (int)(1.25 * this.statusLoadPictureBox.Height));//
                this.statusLoadPictureBox.Visible = true;
                this.statusLoadPictureBox.Location = new Point(this.statusLoadLabel.Location.X - this.statusLoadPictureBox.Width, this.statusLoadLabel.Location.Y);
                //System.Windows.Forms.Application.DoEvents();

                String strTitle = "System Administration";
                if (this.FindDockedFormToActivate(strTitle) < 0)
                {
                    if (this.FindDockedFormExistence(strTitle) == false)
                    {
                        Global.refreshRqrdVrbls();
                        SystemAdministration.Forms.mainForm nwMnFrm = new SystemAdministration.Forms.mainForm();
                        nwMnFrm.lgn_num = Global.login_number;
                        nwMnFrm.role_st_id = Global.role_set_id;
                        nwMnFrm.usr_id = Global.usr_id;
                        nwMnFrm.Og_id = Global.org_id;

                        //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmmnCode.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.Role_Set_IDs;
                        WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
                        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        //selectedPlugin.Instance.loadMyRolesNMsgtyps();
                    }
                    else
                    {
                    }
                }
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
        }

        private void wrkFlwMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.myNwMainFrm.cmnCdMn.Login_number <= 0)
                {
                    Global.myNwMainFrm.cmnCdMn.showMsg("Please Login First!", 0);
                    return;
                }
                this.statusLoadLabel.Visible = true;
                this.statusLoadLabel.Location = new Point((Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Width / 2) - (int)((520 - this.statusLoadPictureBox.Width) / 2),
                (Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Height / 2) - (int)(1.25 * this.statusLoadPictureBox.Height));//
                this.statusLoadPictureBox.Visible = true;
                this.statusLoadPictureBox.Location = new Point(this.statusLoadLabel.Location.X - this.statusLoadPictureBox.Width, this.statusLoadLabel.Location.Y);
                //System.Windows.Forms.Application.DoEvents();

                String strTitle = "Workflow Manager";
                if (this.FindDockedFormToActivate(strTitle) < 0)
                {
                    if (this.FindDockedFormExistence(strTitle) == false)
                    {
                        Global.refreshRqrdVrbls();
                        WorkflowManager.Forms.mainForm nwMnFrm = new WorkflowManager.Forms.mainForm();
                        nwMnFrm.lgn_num = Global.login_number;
                        nwMnFrm.role_st_id = Global.role_set_id;
                        nwMnFrm.usr_id = Global.usr_id;
                        nwMnFrm.Og_id = Global.org_id;

                        //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.Role_Set_IDs;
                        WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
                        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        //selectedPlugin.Instance.loadMyRolesNMsgtyps();
                    }
                    else
                    {
                    }
                }
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
        }

        private void alertsMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.myNwMainFrm.cmnCdMn.Login_number <= 0)
                {
                    Global.myNwMainFrm.cmnCdMn.showMsg("Please Login First!", 0);
                    return;
                }
                this.statusLoadLabel.Visible = true;
                this.statusLoadLabel.Location = new Point((Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Width / 2) - (int)((520 - this.statusLoadPictureBox.Width) / 2),
                (Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Height / 2) - (int)(1.25 * this.statusLoadPictureBox.Height));//
                this.statusLoadPictureBox.Visible = true;
                this.statusLoadPictureBox.Location = new Point(this.statusLoadLabel.Location.X - this.statusLoadPictureBox.Width, this.statusLoadLabel.Location.Y);
                //System.Windows.Forms.Application.DoEvents();

                String strTitle = "Alerts Manager";
                if (this.FindDockedFormToActivate(strTitle) < 0)
                {
                    if (this.FindDockedFormExistence(strTitle) == false)
                    {
                        Global.refreshRqrdVrbls();
                        AlertsManager.Forms.mainForm nwMnFrm = new AlertsManager.Forms.mainForm();
                        nwMnFrm.lgn_num = Global.login_number;
                        nwMnFrm.role_st_id = Global.role_set_id;
                        nwMnFrm.usr_id = Global.usr_id;
                        nwMnFrm.Og_id = Global.org_id;

                        //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.Role_Set_IDs;
                        WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
                        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        //selectedPlugin.Instance.loadMyRolesNMsgtyps();
                    }
                    else
                    {
                    }
                }
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
        }

        private void eventsMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.myNwMainFrm.cmnCdMn.Login_number <= 0)
                {
                    Global.myNwMainFrm.cmnCdMn.showMsg("Please Login First!", 0);
                    return;
                }
                this.statusLoadLabel.Visible = true;
                this.statusLoadLabel.Location = new Point((Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Width / 2) - (int)((520 - this.statusLoadPictureBox.Width) / 2),
                (Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Height / 2) - (int)(1.25 * this.statusLoadPictureBox.Height));//
                this.statusLoadPictureBox.Visible = true;
                this.statusLoadPictureBox.Location = new Point(this.statusLoadLabel.Location.X - this.statusLoadPictureBox.Width, this.statusLoadLabel.Location.Y);
                //System.Windows.Forms.Application.DoEvents();

                String strTitle = CommonCode.CommonCodes.Events_name;
                if (this.FindDockedFormToActivate(strTitle) < 0)
                {
                    if (this.FindDockedFormExistence(strTitle) == false)
                    {
                        Global.refreshRqrdVrbls();
                        EventsAndAttendance.Forms.mainForm nwMnFrm = new EventsAndAttendance.Forms.mainForm();
                        nwMnFrm.lgn_num = Global.login_number;
                        nwMnFrm.role_st_id = Global.role_set_id;
                        nwMnFrm.usr_id = Global.usr_id;
                        nwMnFrm.Og_id = Global.org_id;

                        //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.Role_Set_IDs;
                        WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
                        nwMainFrm.TabText = CommonCode.CommonCodes.Events_name;
                        nwMainFrm.Text = CommonCode.CommonCodes.Events_name;
                        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        //selectedPlugin.Instance.loadMyRolesNMsgtyps();
                    }
                    else
                    {
                    }
                }
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
        }

        private void localStorageMenuItem_Click(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists(Application.StartupPath + "\\Images\\"))
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Images\\");
            }
        }

        private void selfServiceMoMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //  String strTitle = "Events And Attendance";
            //  if (this.FindDockedFormToActivate(strTitle) < 0)
            //  {
            //    if (this.FindDockedFormExistence(strTitle) == false)
            //    {
            //      Global.refreshRqrdVrbls();
            //      SelfService.Forms.mainForm nwMnFrm = new SelfService.Forms.mainForm();
            //      nwMnFrm.lgn_num = Global.login_number;
            //      nwMnFrm.role_st_id = Global.role_set_id;
            //      nwMnFrm.usr_id = Global.usr_id;
            //      nwMnFrm.Og_id = Global.org_id;

            //      //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
            //      //nwMnFrm.cmCde.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
            //      //nwMnFrm.cmCde.Role_Set_IDs;
            //      WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
            //      nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            //      //selectedPlugin.Instance.loadMyRolesNMsgtyps();
            //    }
            //    else
            //    {
            //    }
            //  }
            //}
            //catch (Exception ex)
            //{
            //  Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
            //}
        }

        private void storesInventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.myNwMainFrm.cmnCdMn.Login_number <= 0)
                {
                    Global.myNwMainFrm.cmnCdMn.showMsg("Please Login First!", 0);
                    return;
                }
                this.statusLoadLabel.Visible = true;
                this.statusLoadLabel.Location = new Point((Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Width / 2) - (int)((520 - this.statusLoadPictureBox.Width) / 2),
                (Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Height / 2) - (int)(1.25 * this.statusLoadPictureBox.Height));//
                this.statusLoadPictureBox.Visible = true;
                this.statusLoadPictureBox.Location = new Point(this.statusLoadLabel.Location.X - this.statusLoadPictureBox.Width, this.statusLoadLabel.Location.Y);
                //System.Windows.Forms.Application.DoEvents();

                String strTitle = CommonCode.CommonCodes.Store_inventory;
                if (this.FindDockedFormToActivate(strTitle) < 0)
                {
                    if (this.FindDockedFormExistence(strTitle) == false)
                    {
                        Global.refreshRqrdVrbls();
                        StoresAndInventoryManager.Forms.mainForm nwMnFrm = new StoresAndInventoryManager.Forms.mainForm();
                        nwMnFrm.lgn_num = Global.login_number;
                        nwMnFrm.role_st_id = Global.role_set_id;
                        nwMnFrm.usr_id = Global.usr_id;
                        nwMnFrm.Og_id = Global.org_id;

                        //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.Role_Set_IDs;
                        WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
                        nwMainFrm.TabText = CommonCode.CommonCodes.Store_inventory;
                        nwMainFrm.Text = CommonCode.CommonCodes.Store_inventory;
                        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        //selectedPlugin.Instance.loadMyRolesNMsgtyps();
                    }
                    else
                    {
                    }
                }
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();

            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            try
            {
                try
                {
                    CommonCode.CommonCodes.GlobalSQLConn.ConnectionString = CommonCode.CommonCodes.ConnStr;
                    CommonCode.CommonCodes.GlobalSQLConn.Open();
                    CommonCode.CommonCodes.DatabaseNm = CommonCode.CommonCodes.GlobalSQLConn.Database;
                    Global.db_server = CommonCode.CommonCodes.GlobalSQLConn.Host;
                    Global.db_name = CommonCode.CommonCodes.GlobalSQLConn.Database;
                }
                catch (Exception ex)
                {

                }
                if (CommonCode.CommonCodes.DatabaseNm != "")
                {
                    int lvid = Global.myNwMainFrm.cmnCdMn.getLovID("Allowed DB Name for Request Listener");
                    if (lvid <= 0)
                    {
                        Global.myNwMainFrm.cmnCdMn.createLovNm("Allowed DB Name for Request Listener", "Allowed DB Name for Request Listener", false, "", "SYS", true);
                        lvid = Global.myNwMainFrm.cmnCdMn.getLovID("Allowed DB Name for Request Listener");
                    }
                    bool rnnrRnng = false;
                    int isIPAllwd = Global.myNwMainFrm.cmnCdMn.getEnbldPssblValID(Global.myNwMainFrm.cmnCdMn.getMachDetails()[2],
                      Global.myNwMainFrm.cmnCdMn.getEnbldLovID("Allowed IP Address for Request Listener"));
                    int isDBAllwd = Global.myNwMainFrm.cmnCdMn.getEnbldPssblValID(Global.db_name, lvid);

                    string tst = System.Environment.GetEnvironmentVariable("Path");
                    if (isIPAllwd > 0 && isDBAllwd > 0)
                    {
                        if (rnnrRnng == false)
                        {
                            Global.updatePrcsRnnrCmd("REQUESTS LISTENER PROGRAM", "0");
                            string[] args = { CommonCode.CommonCodes.Db_host,
                          CommonCode.CommonCodes.Db_port,
                          CommonCode.CommonCodes.Db_uname,
                          CommonCode.CommonCodes.Db_pwd,
                          CommonCode.CommonCodes.Db_dbase,
                          "\"REQUESTS LISTENER PROGRAM\"",
                          (-1).ToString(),
                          "\""+ Application.StartupPath + "\\bin\"",
                          "DESKTOP",
                          "\""+ Application.StartupPath + "\\Images\\"+CommonCode.CommonCodes.DatabaseNm+"\""};

                            System.Diagnostics.Process processDB = new System.Diagnostics.Process();
                            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            startInfo.FileName = Application.StartupPath + @"\bin\REMSProcessRunner.exe";//"cmd.exe";
                            startInfo.Arguments = String.Join(" ", args);//"/C xcopy \"" + srcpath + "\" \"" + destpath + "\" /E /I /Q /Y /C";
                            processDB.StartInfo = startInfo;
                            processDB.Start();
                        }
                    }
                    CommonCode.CommonCodes.GlobalSQLConn.Close();
                }
            }
            catch (Exception ex)
            {
                //Global.myNwMainFrm.cmnCdMn.showSQLNoPermsn(ex.Message+"\r\n"+ex.StackTrace+"\r\n"+ex.InnerException);
            }
        }

        private void mainDockPanel_ContentRemoved(object sender, WeifenLuo.WinFormsUI.Docking.DockContentEventArgs e)
        {
            Global.myNwMainFrm.cmnCdMn.minimizeMemory();
            GC.Collect();
        }

        private void hospitalityMngmntMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.myNwMainFrm.cmnCdMn.Login_number <= 0)
                {
                    Global.myNwMainFrm.cmnCdMn.showMsg("Please Login First!", 0);
                    return;
                }
                this.statusLoadLabel.Visible = true;
                this.statusLoadLabel.Location = new Point((Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Width / 2) - (int)((520 - this.statusLoadPictureBox.Width) / 2),
                (Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Height / 2) - (int)(1.25 * this.statusLoadPictureBox.Height));//
                this.statusLoadPictureBox.Visible = true;
                this.statusLoadPictureBox.Location = new Point(this.statusLoadLabel.Location.X - this.statusLoadPictureBox.Width, this.statusLoadLabel.Location.Y);
                //System.Windows.Forms.Application.DoEvents();

                String strTitle = CommonCode.CommonCodes.Hospitality_name;
                if (this.FindDockedFormToActivate(strTitle) < 0)
                {
                    if (this.FindDockedFormExistence(strTitle) == false)
                    {
                        Global.refreshRqrdVrbls();
                        HospitalityManagement.Forms.mainForm nwMnFrm = new HospitalityManagement.Forms.mainForm();
                        nwMnFrm.lgn_num = Global.login_number;
                        nwMnFrm.role_st_id = Global.role_set_id;
                        nwMnFrm.usr_id = Global.usr_id;
                        nwMnFrm.Og_id = Global.org_id;

                        //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.Role_Set_IDs;
                        WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
                        nwMainFrm.TabText = CommonCode.CommonCodes.Hospitality_name;
                        nwMainFrm.Text = CommonCode.CommonCodes.Hospitality_name;
                        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        //selectedPlugin.Instance.loadMyRolesNMsgtyps();
                    }
                    else
                    {
                    }
                }
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();

            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
        }

        private void bnkMicroMenuItem_Click(object sender, EventArgs e)
        {
            //  try
            //  {
            //    if (Global.myNwMainFrm.cmnCdMn.Login_number <= 0)
            //    {
            //      Global.myNwMainFrm.cmnCdMn.showMsg("Please Login First!", 0);
            //      return;
            //    }
            //    this.statusLoadLabel.Visible = true;
            //    this.statusLoadLabel.Location = new Point((Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Width / 2) - (int)((520 - this.statusLoadPictureBox.Width) / 2),
            //    (Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Height / 2) - (int)(1.25 * this.statusLoadPictureBox.Height));//
            //    this.statusLoadPictureBox.Visible = true;
            //    this.statusLoadPictureBox.Location = new Point(this.statusLoadLabel.Location.X - this.statusLoadPictureBox.Width, this.statusLoadLabel.Location.Y);
            //    //System.Windows.Forms.Application.DoEvents();

            //    String strTitle = "Banking And Microfinance";
            //    if (this.FindDockedFormToActivate(strTitle) < 0)
            //    {
            //      if (this.FindDockedFormExistence(strTitle) == false)
            //      {
            //        Global.refreshRqrdVrbls();
            //        BankingAndMicrofinance.Forms.mainForm nwMnFrm = new BankingAndMicrofinance.Forms.mainForm();
            //        nwMnFrm.lgn_num = Global.login_number;
            //        nwMnFrm.role_st_id = Global.role_set_id;
            //        nwMnFrm.usr_id = Global.usr_id;
            //        nwMnFrm.Og_id = Global.org_id;

            //        //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
            //        //nwMnFrm.cmCde.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
            //        //nwMnFrm.cmCde.Role_Set_IDs;
            //        WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
            //        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            //        //selectedPlugin.Instance.loadMyRolesNMsgtyps();
            //      }
            //      else
            //      {
            //      }
            //    }
            //    this.statusLoadLabel.Visible = false;
            //    this.statusLoadPictureBox.Visible = false;
            //    //System.Windows.Forms.Application.DoEvents();

            //  }
            //  catch (Exception ex)
            //  {
            //    Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
            //    this.statusLoadLabel.Visible = false;
            //    this.statusLoadPictureBox.Visible = false;
            //    //System.Windows.Forms.Application.DoEvents();
            //  }
        }

        private void academicsMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.myNwMainFrm.cmnCdMn.Login_number <= 0)
                {
                    Global.myNwMainFrm.cmnCdMn.showMsg("Please Login First!", 0);
                    return;
                }
                this.statusLoadLabel.Visible = true;
                this.statusLoadLabel.Location = new Point((Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Width / 2) - (int)((520 - this.statusLoadPictureBox.Width) / 2),
                (Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Height / 2) - (int)(1.25 * this.statusLoadPictureBox.Height));//
                this.statusLoadPictureBox.Visible = true;
                this.statusLoadPictureBox.Location = new Point(this.statusLoadLabel.Location.X - this.statusLoadPictureBox.Width, this.statusLoadLabel.Location.Y);
                //System.Windows.Forms.Application.DoEvents();

                String strTitle = CommonCode.CommonCodes.Learning_name;
                if (this.FindDockedFormToActivate(strTitle) < 0)
                {
                    if (this.FindDockedFormExistence(strTitle) == false)
                    {
                        Global.refreshRqrdVrbls();
                        AcademicsManagement.Forms.mainForm nwMnFrm = new AcademicsManagement.Forms.mainForm();
                        nwMnFrm.lgn_num = Global.login_number;
                        nwMnFrm.role_st_id = Global.role_set_id;
                        nwMnFrm.usr_id = Global.usr_id;
                        nwMnFrm.Og_id = Global.org_id;

                        //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.Role_Set_IDs;
                        WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
                        nwMainFrm.TabText = CommonCode.CommonCodes.Learning_name;
                        nwMainFrm.Text = CommonCode.CommonCodes.Learning_name;
                        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        //selectedPlugin.Instance.loadMyRolesNMsgtyps();
                    }
                    else
                    {
                    }
                }
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();

            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
        }

        private void clinicHospitalManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void projectManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.myNwMainFrm.cmnCdMn.Login_number <= 0)
                {
                    Global.myNwMainFrm.cmnCdMn.showMsg("Please Login First!", 0);
                    return;
                }
                this.statusLoadLabel.Visible = true;
                this.statusLoadLabel.Location = new Point((Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Width / 2) - (int)((520 - this.statusLoadPictureBox.Width) / 2),
                (Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Height / 2) - (int)(1.25 * this.statusLoadPictureBox.Height));//
                this.statusLoadPictureBox.Visible = true;
                this.statusLoadPictureBox.Location = new Point(this.statusLoadLabel.Location.X - this.statusLoadPictureBox.Width, this.statusLoadLabel.Location.Y);
                //System.Windows.Forms.Application.DoEvents();

                String strTitle = CommonCode.CommonCodes.Proj_mgmnt_name;
                if (this.FindDockedFormToActivate(strTitle) < 0)
                {
                    if (this.FindDockedFormExistence(strTitle) == false)
                    {
                        Global.refreshRqrdVrbls();
                        ProjectsManagement.Forms.mainForm nwMnFrm = new ProjectsManagement.Forms.mainForm();
                        nwMnFrm.lgn_num = Global.login_number;
                        nwMnFrm.role_st_id = Global.role_set_id;
                        nwMnFrm.usr_id = Global.usr_id;
                        nwMnFrm.Og_id = Global.org_id;

                        //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.Role_Set_IDs;
                        WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
                        nwMainFrm.TabText = CommonCode.CommonCodes.Proj_mgmnt_name;
                        nwMainFrm.Text = CommonCode.CommonCodes.Proj_mgmnt_name;
                        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        //selectedPlugin.Instance.loadMyRolesNMsgtyps();
                    }
                    else
                    {
                    }
                }
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();

            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
        }

        private void visitsAndAppointmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.myNwMainFrm.cmnCdMn.Login_number <= 0)
                {
                    Global.myNwMainFrm.cmnCdMn.showMsg("Please Login First!", 0);
                    return;
                }
                this.statusLoadLabel.Visible = true;
                this.statusLoadLabel.Location = new Point((Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Width / 2) - (int)((520 - this.statusLoadPictureBox.Width) / 2),
                (Global.myNwMainFrm.cmnCdMn.myComputer.Screen.Bounds.Height / 2) - (int)(1.25 * this.statusLoadPictureBox.Height));//
                this.statusLoadPictureBox.Visible = true;
                this.statusLoadPictureBox.Location = new Point(this.statusLoadLabel.Location.X - this.statusLoadPictureBox.Width, this.statusLoadLabel.Location.Y);
                //System.Windows.Forms.Application.DoEvents();

                String strTitle = CommonCode.CommonCodes.Appointments_name;
                if (this.FindDockedFormToActivate(strTitle) < 0)
                {
                    if (this.FindDockedFormExistence(strTitle) == false)
                    {
                        Global.refreshRqrdVrbls();
                        AppointmentsManagement.Forms.mainForm nwMnFrm = new AppointmentsManagement.Forms.mainForm();
                        nwMnFrm.lgn_num = Global.login_number;
                        nwMnFrm.role_st_id = Global.role_set_id;
                        nwMnFrm.usr_id = Global.usr_id;
                        nwMnFrm.Og_id = Global.org_id;

                        //nwMnFrm.gnrlSQLConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.pgSqlConn = CommonCode.CommonCodes.GlobalSQLConn;
                        //nwMnFrm.cmCde.Role_Set_IDs;
                        WeifenLuo.WinFormsUI.Docking.DockContent nwMainFrm = nwMnFrm;
                        nwMainFrm.TabText = CommonCode.CommonCodes.Appointments_name;
                        nwMainFrm.Text = CommonCode.CommonCodes.Appointments_name;
                        nwMainFrm.Show(this.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        //selectedPlugin.Instance.loadMyRolesNMsgtyps();
                    }
                    else
                    {
                    }
                }
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();

            }
            catch (Exception ex)
            {
                Global.myNwMainFrm.cmnCdMn.showMsg(ex.Message, 0);
                this.statusLoadLabel.Visible = false;
                this.statusLoadPictureBox.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
            }
        }


        #region "OTHER FUNCTIONS..."

        #endregion

        private void registerForSupportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Global.myNwMainFrm.cmnCdMn.showSupportDiag(Global.myNwMainFrm.cmnCdMn);
        }

        private void dBConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process processDB = System.Diagnostics.Process.Start(Application.StartupPath + @"\DBConfig.exe");
        }
    }


    internal static class NativeWinAPI
    {
        internal static readonly int GWL_EXSTYLE = -20;
        internal static readonly int WS_EX_COMPOSITED = 0x02000000;
        internal static readonly int WS_CLIPCHILDREN = ~0x02000000;

        [DllImport("user32")]
        internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32")]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    }
}
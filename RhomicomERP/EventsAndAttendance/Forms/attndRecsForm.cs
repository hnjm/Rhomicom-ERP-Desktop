﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EventsAndAttendance.Classes;
using EventsAndAttendance.Dialogs;
using cadmaFunctions;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.VisualBasic;

namespace EventsAndAttendance.Forms
{
    public partial class attndRecsForm : Form
    {
        #region "GLOBAL VARIABLES..."
        //Records;
        cadmaFunctions.NavFuncs myNav = new cadmaFunctions.NavFuncs();
        cadmaFunctions.NavFuncs myNav1 = new cadmaFunctions.NavFuncs();
        long rec_cur_indx = 0;
        bool is_last_rec = false;
        long totl_rec = 0;
        long last_rec_num = 0;
        public string rec_SQL = "";

        long rec_det_cur_indx = 0;
        bool is_last_rec_det = false;
        long totl_rec_det = 0;
        long last_rec_det_num = 0;
        public string rec_det_SQL = "";
        public string rec_mtrc_SQL = "";

        long rec_Cost_cur_indx = 0;
        bool is_last_rec_Cost = false;
        long totl_rec_Cost = 0;
        long last_rec_Cost_num = 0;
        public string rec_Cost_SQL = "";

        bool obey_evnts = false;
        public bool txtChngd = false;
        public string srchWrd = "%";

        bool addRec = false;
        bool editRec = false;
        bool addRecsP = false;
        bool editRecsP = false;
        bool delRecsP = false;
        bool beenToCheckBx = false;
        bool vwCost = false;
        bool editCost = false;
        int curid = -1;
        string curCode = "";
        #endregion

        #region "FORM EVENTS..."
        public attndRecsForm()
        {
            InitializeComponent();
        }

        private void attndRecsForm_Load(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.deleteDataNoParams("DELETE FROM attn.attn_attendance_costs a WHERE((a.recs_hdr_id <= -1))");
            Color[] clrs = Global.mnFrm.cmCde.getColors();
            this.BackColor = clrs[0];
            this.tabPage1.BackColor = clrs[0];
            this.tabPage2.BackColor = clrs[0];
            //this.glsLabel3.TopFill = clrs[0];
            //this.glsLabel3.BottomFill = clrs[1];
            //this.storeIDTextBox.Text = Global.getUserStoreID().ToString();
            this.curid = Global.mnFrm.cmCde.getOrgFuncCurID(Global.mnFrm.cmCde.Org_id);
            this.curCode = Global.mnFrm.cmCde.getPssblValNm(this.curid);
            Global.selectedStoreID = Global.getUserStoreID();
        }

        public void disableFormButtons()
        {
            bool vwSQL = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[6]);
            bool rcHstry = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[7]);
            this.addRecsP = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[8]);
            this.editRecsP = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[9]);
            this.delRecsP = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[10]);
            this.vwCost = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[27]);
            this.editCost = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[29]);

            if (this.vwCost == false)
            {
                this.attendRecsTabControl.TabPages.Remove(this.tabPage3);
            }
            if (this.editRec == false && this.addRec == false)
            {
                this.saveButton.Enabled = false;
            }
            this.addButton.Enabled = this.addRecsP;
            this.editButton.Enabled = this.editRecsP;
            this.delButton.Enabled = this.delRecsP;
            this.vwSQLButton.Enabled = vwSQL;
            this.rcHstryButton.Enabled = rcHstry;

            this.deleteDetButton.Enabled = this.editRecsP;
            this.vwSQLDetButton.Enabled = vwSQL;
            this.rcHstryDetButton.Enabled = rcHstry;

        }

        #endregion

        #region "ATTENDANCE REGISTERS..."
        public void loadPanel()
        {
            if (this.editButton.Text.Contains("STOP"))
            {
                this.editButton.PerformClick();
            }

            this.obey_evnts = false;
            if (this.searchInComboBox.SelectedIndex < 0)
            {
                this.searchInComboBox.SelectedIndex = 0;
            }
            if (this.searchForTextBox.Text.Contains("%") == false)
            {
                this.searchForTextBox.Text = "%" + this.searchForTextBox.Text.Replace(" ", "%") + "%";
            }
            if (this.searchForTextBox.Text == "%%")
            {
                this.searchForTextBox.Text = "%";
            }
            int dsply = 0;
            if (this.dsplySizeComboBox.Text == ""
              || int.TryParse(this.dsplySizeComboBox.Text, out dsply) == false)
            {
                this.dsplySizeComboBox.Text = Global.mnFrm.cmCde.get_CurPlcy_Mx_Dsply_Recs().ToString();
            }
            this.is_last_rec = false;
            this.totl_rec = Global.mnFrm.cmCde.Big_Val;
            this.getPnlData();
            this.obey_evnts = true;
        }

        private void getPnlData()
        {
            this.updtTotals();
            this.populateListVw();
            this.updtNavLabels();
        }

        private void updtTotals()
        {
            Global.mnFrm.cmCde.navFuncts.FindNavigationIndices(
              long.Parse(this.dsplySizeComboBox.Text), this.totl_rec);
            if (this.rec_cur_indx >= Global.mnFrm.cmCde.navFuncts.totalGroups)
            {
                this.rec_cur_indx = Global.mnFrm.cmCde.navFuncts.totalGroups - 1;
            }
            if (this.rec_cur_indx < 0)
            {
                this.rec_cur_indx = 0;
            }
            Global.mnFrm.cmCde.navFuncts.currentNavigationIndex = this.rec_cur_indx;
        }

        private void updtNavLabels()
        {
            this.moveFirstButton.Enabled = Global.mnFrm.cmCde.navFuncts.moveFirstBtnStatus();
            this.movePreviousButton.Enabled = Global.mnFrm.cmCde.navFuncts.movePrevBtnStatus();
            this.moveNextButton.Enabled = Global.mnFrm.cmCde.navFuncts.moveNextBtnStatus();
            this.moveLastButton.Enabled = Global.mnFrm.cmCde.navFuncts.moveLastBtnStatus();
            this.positionTextBox.Text = Global.mnFrm.cmCde.navFuncts.displayedRecordsNumbers();
            if (this.is_last_rec == true ||
              this.totl_rec != Global.mnFrm.cmCde.Big_Val)
            {
                this.totalRecsLabel.Text = Global.mnFrm.cmCde.navFuncts.totalRecordsLabel();
            }
            else
            {
                this.totalRecsLabel.Text = "of Total";
            }
        }

        private void populateListVw()
        {
            this.obey_evnts = false;
            DataSet dtst = Global.get_Basic_AttnRgstr(this.searchForTextBox.Text,
              this.searchInComboBox.Text, this.rec_cur_indx,
              int.Parse(this.dsplySizeComboBox.Text), Global.mnFrm.cmCde.Org_id, false);
            this.attnRgstrListView.Items.Clear();

            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                this.last_rec_num = Global.mnFrm.cmCde.navFuncts.startIndex() + i;
                ListViewItem nwItem = new ListViewItem(new string[] {
    (Global.mnFrm.cmCde.navFuncts.startIndex() + i).ToString(),
    dtst.Tables[0].Rows[i][1].ToString(),
    dtst.Tables[0].Rows[i][0].ToString()});
                this.attnRgstrListView.Items.Add(nwItem);
            }
            this.correctNavLbls(dtst);
            if (this.attnRgstrListView.Items.Count > 0)
            {
                this.obey_evnts = true;
                this.attnRgstrListView.Items[0].Selected = true;
            }
            else
            {
                this.populateDet(-10000);
            }
            this.obey_evnts = true;
        }

        private void populateDet(long rgstrID)
        {
            if (this.editRec == false)
            {
                this.clearDetInfo();
                this.disableDetEdit();
            }
            this.obey_evnts = false;
            DataSet dtst = Global.get_One_AttnRgstrDet(rgstrID);
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                this.rgstrIDTextBox.Text = dtst.Tables[0].Rows[i][0].ToString();
                this.rgstrNmTextBox.Text = dtst.Tables[0].Rows[i][1].ToString();
                this.rgstrDescTextBox.Text = dtst.Tables[0].Rows[i][2].ToString();

                this.tmTblIDTextBox.Text = dtst.Tables[0].Rows[i][3].ToString();
                this.tmTblNmTextBox.Text = dtst.Tables[0].Rows[i][4].ToString();

                this.tmTblDetIDTextBox.Text = dtst.Tables[0].Rows[i][5].ToString();
                this.evntDescTextBox.Text = dtst.Tables[0].Rows[i][6].ToString();

                this.evntStrtDateTextBox.Text = dtst.Tables[0].Rows[i][7].ToString();
                this.evntEndDateTextBox.Text = dtst.Tables[0].Rows[i][8].ToString();
            }
            this.loadRgstrDetLnsPanel();
            this.populateMetricGridVw();
            if (this.vwCost)
            {
                this.loadRgstrCostLnsPanel();
            }
            this.obey_evnts = true;
        }

        private void correctNavLbls(DataSet dtst)
        {
            long totlRecs = dtst.Tables[0].Rows.Count;
            if (this.rec_cur_indx == 0 && totlRecs == 0)
            {
                this.is_last_rec = true;
                this.totl_rec = 0;
                this.last_rec_num = 0;
                this.rec_cur_indx = 0;
                this.updtTotals();
                this.updtNavLabels();
            }
            else if (this.totl_rec == Global.mnFrm.cmCde.Big_Val
           && totlRecs < long.Parse(this.dsplySizeComboBox.Text))
            {
                this.totl_rec = this.last_rec_num;
                if (totlRecs == 0)
                {
                    this.rec_cur_indx -= 1;
                    this.updtTotals();
                    this.populateListVw();
                }
                else
                {
                    this.updtTotals();
                }
            }
        }

        private bool shdObeyEvts()
        {
            return this.obey_evnts;
        }

        private void PnlNavButtons(object sender, System.EventArgs e)
        {
            System.Windows.Forms.ToolStripButton sentObj = (System.Windows.Forms.ToolStripButton)sender;
            this.totalRecsLabel.Text = "";
            if (sentObj.Name.ToLower().Contains("first"))
            {
                this.is_last_rec = false;
                this.rec_cur_indx = 0;
            }
            else if (sentObj.Name.ToLower().Contains("previous"))
            {
                this.is_last_rec = false;
                this.rec_cur_indx -= 1;
            }
            else if (sentObj.Name.ToLower().Contains("next"))
            {
                this.is_last_rec = false;
                this.rec_cur_indx += 1;
            }
            else if (sentObj.Name.ToLower().Contains("last"))
            {
                this.is_last_rec = true;
                this.totl_rec = Global.get_Total_AttnRgstr(this.searchForTextBox.Text,
                  this.searchInComboBox.Text, Global.mnFrm.cmCde.Org_id, false);
                this.updtTotals();
                this.rec_cur_indx = Global.mnFrm.cmCde.navFuncts.totalGroups - 1;
            }
            this.getPnlData();
        }

        private void clearDetInfo()
        {
            this.obey_evnts = false;
            this.saveButton.Enabled = false;
            this.addButton.Enabled = this.addRecsP;
            this.editButton.Enabled = this.editRecsP;
            this.delButton.Enabled = this.delRecsP;
            this.rgstrIDTextBox.Text = "-1";
            this.rgstrNmTextBox.Text = "";
            this.rgstrDescTextBox.Text = "";

            this.tmTblIDTextBox.Text = "-1";
            this.tmTblNmTextBox.Text = "";

            this.tmTblDetIDTextBox.Text = "-1";
            this.evntDescTextBox.Text = "";

            this.evntStrtDateTextBox.Text = "";
            this.evntEndDateTextBox.Text = "";
            this.obey_evnts = true;
        }

        private void prpareForDetEdit()
        {
            this.obey_evnts = false;
            this.saveButton.Enabled = true;
            this.rgstrNmTextBox.ReadOnly = false;
            this.rgstrNmTextBox.BackColor = Color.FromArgb(255, 255, 128);
            this.rgstrDescTextBox.ReadOnly = false;
            this.rgstrDescTextBox.BackColor = Color.FromArgb(255, 255, 128);

            this.tmTblNmTextBox.ReadOnly = false;
            this.tmTblNmTextBox.BackColor = Color.White;

            this.evntDescTextBox.ReadOnly = false;
            this.evntDescTextBox.BackColor = Color.White;

            this.evntStrtDateTextBox.ReadOnly = false;
            this.evntStrtDateTextBox.BackColor = Color.FromArgb(255, 255, 128);

            this.evntEndDateTextBox.ReadOnly = false;
            this.evntEndDateTextBox.BackColor = Color.FromArgb(255, 255, 128);
            this.obey_evnts = true;
        }

        private void disableDetEdit()
        {
            this.obey_evnts = false;
            this.addRec = false;
            this.editRec = false;
            this.rgstrNmTextBox.ReadOnly = true;
            this.rgstrNmTextBox.BackColor = Color.WhiteSmoke;
            this.rgstrDescTextBox.ReadOnly = true;
            this.rgstrDescTextBox.BackColor = Color.WhiteSmoke;

            this.tmTblNmTextBox.ReadOnly = true;
            this.tmTblNmTextBox.BackColor = Color.WhiteSmoke;

            this.evntDescTextBox.ReadOnly = true;
            this.evntDescTextBox.BackColor = Color.WhiteSmoke;

            this.evntStrtDateTextBox.ReadOnly = true;
            this.evntStrtDateTextBox.BackColor = Color.WhiteSmoke;

            this.evntEndDateTextBox.ReadOnly = true;
            this.evntEndDateTextBox.BackColor = Color.WhiteSmoke;
            this.obey_evnts = true;
        }

        private void loadRgstrDetLnsPanel()
        {
            this.obey_evnts = false;
            int dsply = 0;
            if (this.dsplySizeDetComboBox.Text == ""
             || int.TryParse(this.dsplySizeDetComboBox.Text, out dsply) == false)
            {
                this.dsplySizeDetComboBox.Text = "10";
            }
            if (this.searchInDetComboBox.SelectedIndex < 0)
            {
                this.searchInDetComboBox.SelectedIndex = 4;
            }
            if (this.searchForDetTextBox.Text.Contains("%") == false)
            {
                this.searchForDetTextBox.Text = "%" + this.searchForDetTextBox.Text.Replace(" ", "%") + "%";
            }
            if (this.searchForDetTextBox.Text == "%%")
            {
                this.searchForDetTextBox.Text = "%";
            }
            this.rec_det_cur_indx = 0;
            this.is_last_rec_det = false;
            this.last_rec_det_num = 0;
            this.totl_rec_det = Global.mnFrm.cmCde.Big_Val;
            this.getTdetPnlData();
            this.obey_evnts = true;
        }

        private void getTdetPnlData()
        {
            this.updtTdetTotals();
            this.populateTdetGridVw();
            this.updtTdetNavLabels();
        }

        private void updtTdetTotals()
        {
            int dsply = 0;
            if (this.dsplySizeDetComboBox.Text == ""
              || int.TryParse(this.dsplySizeDetComboBox.Text, out dsply) == false)
            {
                this.dsplySizeDetComboBox.Text = "10";
            }
            this.myNav.FindNavigationIndices(
          long.Parse(this.dsplySizeDetComboBox.Text), this.totl_rec_det);
            if (this.rec_det_cur_indx >= this.myNav.totalGroups)
            {
                this.rec_det_cur_indx = this.myNav.totalGroups - 1;
            }
            if (this.rec_det_cur_indx < 0)
            {
                this.rec_det_cur_indx = 0;
            }
            this.myNav.currentNavigationIndex = this.rec_det_cur_indx;
        }

        private void updtTdetNavLabels()
        {
            this.moveFirstDetButton.Enabled = this.myNav.moveFirstBtnStatus();
            this.movePreviousDetButton.Enabled = this.myNav.movePrevBtnStatus();
            this.moveNextDetButton.Enabled = this.myNav.moveNextBtnStatus();
            this.moveLastDetButton.Enabled = this.myNav.moveLastBtnStatus();
            this.positionDetTextBox.Text = this.myNav.displayedRecordsNumbers();
            if (this.is_last_rec_det == true ||
             this.totl_rec_det != Global.mnFrm.cmCde.Big_Val)
            {
                this.totalRecsDetLabel.Text = this.myNav.totalRecordsLabel();
            }
            else
            {
                this.totalRecsDetLabel.Text = "of Total";
            }
        }

        private void populateMetricGridVw()
        {
            this.metricsDataGridView.Rows.Clear();
            if (this.rgstrIDTextBox.Text == ""
              || this.rgstrIDTextBox.Text == "-1"
              || this.tmTblDetIDTextBox.Text == ""
              || this.tmTblDetIDTextBox.Text == "-1")
            {
                return;
            }
            this.obey_evnts = false;
            if (this.editRec == false && this.addRec == false)
            {
                disableMetrcLnsEdit();
            }

            this.obey_evnts = false;
            this.metricsDataGridView.DefaultCellStyle.ForeColor = Color.Black;
            string evntID = Global.mnFrm.cmCde.getGnrlRecNm(
                  "attn.attn_time_table_details", "time_table_det_id",
                  "event_id", long.Parse(this.tmTblDetIDTextBox.Text));
            string lovNm = Global.mnFrm.cmCde.getGnrlRecNm(
                  "attn.attn_attendance_events", "event_id",
                  "attnd_metric_lov_nm", int.Parse(evntID)); ;
            DataSet dtst = Global.get_One_AttnRgstr_MtrcLns(
             long.Parse(this.rgstrIDTextBox.Text), lovNm, this.editRec, int.Parse(evntID));
            this.metricsDataGridView.Rows.Clear();

            int rwcnt = dtst.Tables[0].Rows.Count;
            for (int i = 0; i < rwcnt; i++)
            {
                this.metricsDataGridView.RowCount += 1;//.Insert(this.metricsDataGridView.RowCount - 1, 1);
                int rowIdx = this.metricsDataGridView.RowCount - 1;

                this.metricsDataGridView.Rows[rowIdx].HeaderCell.Value = (i + 1).ToString();
                this.metricsDataGridView.Rows[rowIdx].Cells[0].Value = dtst.Tables[0].Rows[i][1].ToString();
                this.metricsDataGridView.Rows[rowIdx].Cells[1].Value = dtst.Tables[0].Rows[i][4].ToString();
                this.metricsDataGridView.Rows[rowIdx].Cells[2].Value = dtst.Tables[0].Rows[i][2].ToString();
                this.metricsDataGridView.Rows[rowIdx].Cells[3].Value = dtst.Tables[0].Rows[i][3].ToString();

                this.metricsDataGridView.Rows[rowIdx].Cells[4].Value = this.rgstrIDTextBox.Text;
                this.metricsDataGridView.Rows[rowIdx].Cells[5].Value = dtst.Tables[0].Rows[i][0].ToString();

                this.metricsDataGridView.Rows[rowIdx].Cells[6].Value = dtst.Tables[0].Rows[i][5].ToString();
                this.metricsDataGridView.Rows[rowIdx].Cells[7].Value = dtst.Tables[0].Rows[i][6].ToString();
            }
            this.obey_evnts = true;
        }

        private void populateTdetGridVw()
        {
            this.obey_evnts = false;
            this.rgstrDetDataGridView.Rows.Clear();
            if (this.editRec == false && this.addRec == false)
            {
                disableLnsEdit();
            }

            this.obey_evnts = false;
            this.rgstrDetDataGridView.DefaultCellStyle.ForeColor = Color.Black;
            string evntID = Global.mnFrm.cmCde.getGnrlRecNm(
                  "attn.attn_time_table_details", "time_table_det_id",
                  "event_id", long.Parse(this.tmTblDetIDTextBox.Text));
            int eventID = -1;
            int.TryParse(evntID, out eventID);
            long evntPrcsCnt = Global.get_One_EvntTtlPrices(eventID);
            if (long.Parse(this.tmTblDetIDTextBox.Text) > 0 && evntPrcsCnt > 0)
            {
                this.rgstrDetDataGridView.Columns[9].Visible = false;
                this.rgstrDetDataGridView.Columns[10].Visible = true;
                this.rgstrDetDataGridView.Columns[11].Visible = false;
                this.rgstrDetDataGridView.Columns[12].Visible = false;
                this.rgstrDetDataGridView.Columns[17].Visible = true;
                this.rgstrDetDataGridView.Columns[19].Visible = true;
                this.rgstrDetDataGridView.Columns[21].Visible = true;
                this.rgstrDetDataGridView.Columns[22].Visible = true;
                this.rgstrDetDataGridView.Columns[23].Visible = true;
                this.rgstrDetDataGridView.Columns[24].Visible = true;
            }
            else
            {
                this.rgstrDetDataGridView.Columns[9].Visible = false;
                this.rgstrDetDataGridView.Columns[10].Visible = true;
                this.rgstrDetDataGridView.Columns[11].Visible = true;
                this.rgstrDetDataGridView.Columns[12].Visible = true;
                this.rgstrDetDataGridView.Columns[17].Visible = false;
                this.rgstrDetDataGridView.Columns[19].Visible = false;
                this.rgstrDetDataGridView.Columns[21].Visible = false;
                this.rgstrDetDataGridView.Columns[22].Visible = false;
                this.rgstrDetDataGridView.Columns[23].Visible = false;
                this.rgstrDetDataGridView.Columns[24].Visible = false;
            }
            DataSet dtst = Global.get_One_AttnRgstr_DetLns(this.searchForDetTextBox.Text,
              this.searchInDetComboBox.Text,
              this.rec_det_cur_indx,
             int.Parse(this.dsplySizeDetComboBox.Text),
             long.Parse(this.rgstrIDTextBox.Text));
            this.rgstrDetDataGridView.Rows.Clear();

            int rwcnt = dtst.Tables[0].Rows.Count;
            System.Windows.Forms.Application.DoEvents();
            for (int i = 0; i < rwcnt; i++)
            {
                this.last_rec_det_num = this.myNav.startIndex() + i;
                this.rgstrDetDataGridView.RowCount += 1;//.Insert(this.rgstrDetDataGridView.RowCount - 1, 1);
                int rowIdx = this.rgstrDetDataGridView.RowCount - 1;

                this.rgstrDetDataGridView.Rows[rowIdx].HeaderCell.Value = (i + 1).ToString();
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[0].Value = dtst.Tables[0].Rows[i][3].ToString();
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[1].Value = dtst.Tables[0].Rows[i][9].ToString();
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[2].Value = "...";
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[3].Value = dtst.Tables[0].Rows[i][2].ToString();
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[4].Value = dtst.Tables[0].Rows[i][4].ToString();
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[5].Value = "...";

                this.rgstrDetDataGridView.Rows[rowIdx].Cells[6].Value = bool.Parse(dtst.Tables[0].Rows[i][7].ToString());

                this.rgstrDetDataGridView.Rows[rowIdx].Cells[7].Value = dtst.Tables[0].Rows[i][5].ToString();
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[8].Value = "...";
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[9].Value = dtst.Tables[0].Rows[i][6].ToString();
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[10].Value = "Attendance Time Details";
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[11].Value = dtst.Tables[0].Rows[i][8].ToString();
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[12].Value = dtst.Tables[0].Rows[i][10].ToString();
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[13].Value = dtst.Tables[0].Rows[i][11].ToString();

                this.rgstrDetDataGridView.Rows[rowIdx].Cells[14].Value = dtst.Tables[0].Rows[i][1].ToString();
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[15].Value = dtst.Tables[0].Rows[i][0].ToString();

                this.rgstrDetDataGridView.Rows[rowIdx].Cells[16].Value = dtst.Tables[0].Rows[i][12].ToString();
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[17].Value = "Points Scored";
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[18].Value = "Attach Docs.";
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[19].Value = "Invoice";
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[20].Value = dtst.Tables[0].Rows[i][13].ToString();
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[21].Value = dtst.Tables[0].Rows[i][14].ToString();
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[22].Value = "...";
                double amntBlld = double.Parse(dtst.Tables[0].Rows[i][15].ToString());
                double amntPaid = double.Parse(dtst.Tables[0].Rows[i][16].ToString());
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[23].Value = amntBlld.ToString("#,##0.00");
                this.rgstrDetDataGridView.Rows[rowIdx].Cells[24].Value = amntPaid.ToString("#,##0.00");
                if (amntBlld == 0)
                {
                    this.rgstrDetDataGridView.Rows[rowIdx].Cells[23].Style.BackColor = Color.WhiteSmoke;
                    this.rgstrDetDataGridView.Rows[rowIdx].Cells[24].Style.BackColor = Color.WhiteSmoke;
                }
                else if (amntPaid >= amntBlld)
                {
                    this.rgstrDetDataGridView.Rows[rowIdx].Cells[23].Style.BackColor = Color.Lime;
                    this.rgstrDetDataGridView.Rows[rowIdx].Cells[24].Style.BackColor = Color.Lime;
                }
                else
                {
                    this.rgstrDetDataGridView.Rows[rowIdx].Cells[23].Style.BackColor = Color.Red;
                    this.rgstrDetDataGridView.Rows[rowIdx].Cells[24].Style.BackColor = Color.Red;
                }

            }
            this.correctTdetNavLbls(dtst);
            this.obey_evnts = true;
        }

        private void correctTdetNavLbls(DataSet dtst)
        {
            long totlRecs = dtst.Tables[0].Rows.Count;
            if (this.rec_det_cur_indx == 0 && totlRecs == 0)
            {
                this.is_last_rec_det = true;
                this.totl_rec_det = 0;
                this.last_rec_det_num = 0;
                this.rec_det_cur_indx = 0;
                this.updtTdetTotals();
                this.updtTdetNavLabels();
            }
            else if (this.totl_rec_det == Global.mnFrm.cmCde.Big_Val
          && totlRecs < long.Parse(this.dsplySizeDetComboBox.Text))
            {
                this.totl_rec_det = this.last_rec_det_num;
                if (totlRecs == 0)
                {
                    this.rec_det_cur_indx -= 1;
                    this.updtTdetTotals();
                    this.populateTdetGridVw();
                }
                else
                {
                    this.updtTdetTotals();
                }
            }
        }

        private bool shdObeyTdetEvts()
        {
            return this.obey_evnts;
        }

        private void TdetPnlNavButtons(object sender, System.EventArgs e)
        {
            System.Windows.Forms.ToolStripButton sentObj = (System.Windows.Forms.ToolStripButton)sender;
            this.totalRecsDetLabel.Text = "";
            if (sentObj.Name.ToLower().Contains("first"))
            {
                this.is_last_rec_det = false;
                this.rec_det_cur_indx = 0;
            }
            else if (sentObj.Name.ToLower().Contains("previous"))
            {
                this.is_last_rec_det = false;
                this.rec_det_cur_indx -= 1;
            }
            else if (sentObj.Name.ToLower().Contains("next"))
            {
                this.is_last_rec_det = false;
                this.rec_det_cur_indx += 1;
            }
            else if (sentObj.Name.ToLower().Contains("last"))
            {
                this.is_last_rec_det = true;
                this.totl_rec_det = Global.get_Total_AttnRgstr_DetLns(this.searchForDetTextBox.Text,
                this.searchInDetComboBox.Text, long.Parse(this.rgstrIDTextBox.Text));
                this.updtTdetTotals();
                this.rec_det_cur_indx = this.myNav.totalGroups - 1;
            }
            this.getTdetPnlData();
        }

        private void prpareForLnsEdit()
        {
            this.rgstrDetDataGridView.ReadOnly = false;
            this.rgstrDetDataGridView.Columns[1].ReadOnly = true;
            this.rgstrDetDataGridView.Columns[1].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.rgstrDetDataGridView.Columns[6].ReadOnly = false;
            this.rgstrDetDataGridView.Columns[6].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.rgstrDetDataGridView.Columns[4].ReadOnly = true;
            this.rgstrDetDataGridView.Columns[4].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.rgstrDetDataGridView.Columns[7].ReadOnly = false;
            this.rgstrDetDataGridView.Columns[7].DefaultCellStyle.BackColor = Color.White;
            this.rgstrDetDataGridView.Columns[9].ReadOnly = false;
            this.rgstrDetDataGridView.Columns[9].DefaultCellStyle.BackColor = Color.White;

            this.rgstrDetDataGridView.Columns[11].ReadOnly = false;
            this.rgstrDetDataGridView.Columns[11].DefaultCellStyle.BackColor = Color.White;
            this.rgstrDetDataGridView.Columns[12].ReadOnly = false;
            this.rgstrDetDataGridView.Columns[12].DefaultCellStyle.BackColor = Color.White;
            this.rgstrDetDataGridView.Columns[13].ReadOnly = false;
            this.rgstrDetDataGridView.Columns[13].DefaultCellStyle.BackColor = Color.White;
            this.rgstrDetDataGridView.Columns[21].ReadOnly = true;
            this.rgstrDetDataGridView.Columns[21].DefaultCellStyle.BackColor = Color.White;
            this.rgstrDetDataGridView.Columns[23].ReadOnly = true;
            this.rgstrDetDataGridView.Columns[23].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.rgstrDetDataGridView.Columns[24].ReadOnly = true;
            this.rgstrDetDataGridView.Columns[24].DefaultCellStyle.BackColor = Color.WhiteSmoke;

            this.rgstrDetDataGridView.DefaultCellStyle.ForeColor = Color.Black;
        }

        private void prpareForCostLnsEdit()
        {
            this.costingDataGridView.ReadOnly = false;
            this.costingDataGridView.Columns[0].ReadOnly = true;
            this.costingDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.costingDataGridView.Columns[3].ReadOnly = true;
            this.costingDataGridView.Columns[3].DefaultCellStyle.BackColor = Color.WhiteSmoke;

            this.costingDataGridView.Columns[2].ReadOnly = false;
            this.costingDataGridView.Columns[2].DefaultCellStyle.BackColor = Color.White;
            this.costingDataGridView.Columns[5].ReadOnly = false;
            this.costingDataGridView.Columns[5].DefaultCellStyle.BackColor = Color.White;
            this.costingDataGridView.Columns[6].ReadOnly = false;
            this.costingDataGridView.Columns[6].DefaultCellStyle.BackColor = Color.White;
            this.costingDataGridView.Columns[7].ReadOnly = false;
            this.costingDataGridView.Columns[7].DefaultCellStyle.BackColor = Color.White;

            this.costingDataGridView.Columns[8].ReadOnly = true;
            this.costingDataGridView.Columns[8].DefaultCellStyle.BackColor = Color.WhiteSmoke;

            this.costingDataGridView.Columns[9].ReadOnly = true;
            this.costingDataGridView.Columns[9].DefaultCellStyle.BackColor = Color.WhiteSmoke;

            this.costingDataGridView.Columns[10].ReadOnly = true;
            this.costingDataGridView.Columns[10].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.costingDataGridView.Columns[11].ReadOnly = true;
            this.costingDataGridView.Columns[11].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.costingDataGridView.Columns[12].ReadOnly = true;
            this.costingDataGridView.Columns[12].DefaultCellStyle.BackColor = Color.WhiteSmoke;

            this.costingDataGridView.DefaultCellStyle.ForeColor = Color.Black;
        }

        private void prpareForMetricLnsEdit()
        {
            this.saveMtrcsButton.Enabled = true;
            this.metricsDataGridView.ReadOnly = false;
            this.metricsDataGridView.Columns[0].ReadOnly = true;
            this.metricsDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.metricsDataGridView.Columns[2].ReadOnly = false;
            this.metricsDataGridView.Columns[2].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.metricsDataGridView.Columns[3].ReadOnly = false;
            this.metricsDataGridView.Columns[3].DefaultCellStyle.BackColor = Color.White;

            this.rgstrDetDataGridView.DefaultCellStyle.ForeColor = Color.Black;
        }

        private void disableLnsEdit()
        {
            this.rgstrDetDataGridView.DefaultCellStyle.ForeColor = Color.Black;

            this.rgstrDetDataGridView.ReadOnly = true;
            this.rgstrDetDataGridView.Columns[1].ReadOnly = true;
            this.rgstrDetDataGridView.Columns[1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.rgstrDetDataGridView.Columns[6].ReadOnly = true;
            this.rgstrDetDataGridView.Columns[6].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.rgstrDetDataGridView.Columns[4].ReadOnly = true;
            this.rgstrDetDataGridView.Columns[4].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.rgstrDetDataGridView.Columns[7].ReadOnly = true;
            this.rgstrDetDataGridView.Columns[7].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.rgstrDetDataGridView.Columns[9].ReadOnly = true;
            this.rgstrDetDataGridView.Columns[9].DefaultCellStyle.BackColor = Color.WhiteSmoke;

            this.rgstrDetDataGridView.Columns[11].ReadOnly = true;
            this.rgstrDetDataGridView.Columns[11].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.rgstrDetDataGridView.Columns[12].ReadOnly = true;
            this.rgstrDetDataGridView.Columns[12].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.rgstrDetDataGridView.Columns[13].ReadOnly = true;
            this.rgstrDetDataGridView.Columns[13].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.rgstrDetDataGridView.Columns[21].ReadOnly = true;
            this.rgstrDetDataGridView.Columns[21].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.rgstrDetDataGridView.Columns[23].ReadOnly = true;
            this.rgstrDetDataGridView.Columns[23].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.rgstrDetDataGridView.Columns[24].ReadOnly = true;
            this.rgstrDetDataGridView.Columns[24].DefaultCellStyle.BackColor = Color.WhiteSmoke;
        }

        private void disableCostLnsEdit()
        {
            this.costingDataGridView.ReadOnly = true;
            this.costingDataGridView.Columns[0].ReadOnly = true;
            this.costingDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.costingDataGridView.Columns[3].ReadOnly = true;
            this.costingDataGridView.Columns[3].DefaultCellStyle.BackColor = Color.WhiteSmoke;

            this.costingDataGridView.Columns[2].ReadOnly = true;
            this.costingDataGridView.Columns[2].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.costingDataGridView.Columns[5].ReadOnly = true;
            this.costingDataGridView.Columns[5].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.costingDataGridView.Columns[6].ReadOnly = true;
            this.costingDataGridView.Columns[6].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.costingDataGridView.Columns[7].ReadOnly = true;
            this.costingDataGridView.Columns[7].DefaultCellStyle.BackColor = Color.WhiteSmoke;

            this.costingDataGridView.Columns[8].ReadOnly = true;
            this.costingDataGridView.Columns[8].DefaultCellStyle.BackColor = Color.WhiteSmoke;

            this.costingDataGridView.Columns[9].ReadOnly = true;
            this.costingDataGridView.Columns[9].DefaultCellStyle.BackColor = Color.WhiteSmoke;

            this.costingDataGridView.Columns[10].ReadOnly = true;
            this.costingDataGridView.Columns[10].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.costingDataGridView.Columns[11].ReadOnly = true;
            this.costingDataGridView.Columns[11].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.costingDataGridView.Columns[12].ReadOnly = true;
            this.costingDataGridView.Columns[12].DefaultCellStyle.BackColor = Color.WhiteSmoke;

            this.costingDataGridView.DefaultCellStyle.ForeColor = Color.Black;

        }

        private void disableMetrcLnsEdit()
        {
            this.saveMtrcsButton.Enabled = false;
            this.metricsDataGridView.DefaultCellStyle.ForeColor = Color.Black;

            this.metricsDataGridView.ReadOnly = true;
            this.metricsDataGridView.Columns[0].ReadOnly = true;
            this.metricsDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.metricsDataGridView.Columns[2].ReadOnly = true;
            this.metricsDataGridView.Columns[2].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.metricsDataGridView.Columns[3].ReadOnly = true;
            this.metricsDataGridView.Columns[3].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.metricsDataGridView.Columns[1].ReadOnly = true;
            this.metricsDataGridView.Columns[1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.metricsDataGridView.Columns[4].ReadOnly = true;
            this.metricsDataGridView.Columns[4].DefaultCellStyle.BackColor = Color.WhiteSmoke;

            this.metricsDataGridView.Columns[5].ReadOnly = true;
            this.metricsDataGridView.Columns[5].DefaultCellStyle.BackColor = Color.WhiteSmoke;

        }
        #endregion

        #region "EVENT COSTS..."
        private void loadRgstrCostLnsPanel()
        {
            this.obey_evnts = false;
            int dsply = 0;
            if (this.dsplySizeCostComboBox.Text == ""
             || int.TryParse(this.dsplySizeCostComboBox.Text, out dsply) == false)
            {
                this.dsplySizeCostComboBox.Text = "10";
            }
            if (this.searchInCostComboBox.SelectedIndex < 0)
            {
                this.searchInCostComboBox.SelectedIndex = 1;
            }
            if (this.searchForCostTextBox.Text.Contains("%") == false)
            {
                this.searchForCostTextBox.Text = "%" + this.searchForCostTextBox.Text.Replace(" ", "%") + "%";
            }
            if (this.searchForCostTextBox.Text == "%%")
            {
                this.searchForCostTextBox.Text = "%";
            }
            this.rec_Cost_cur_indx = 0;
            this.is_last_rec_Cost = false;
            this.last_rec_Cost_num = 0;
            this.totl_rec_Cost = Global.mnFrm.cmCde.Big_Val;
            this.getCostPnlData();
            this.obey_evnts = true;
        }

        private void getCostPnlData()
        {
            this.updtCostTotals();
            this.populateCostGridVw();
            this.updtCostNavLabels();
        }

        private void updtCostTotals()
        {
            int dsply = 0;
            if (this.dsplySizeCostComboBox.Text == ""
              || int.TryParse(this.dsplySizeCostComboBox.Text, out dsply) == false)
            {
                this.dsplySizeCostComboBox.Text = Global.mnFrm.cmCde.get_CurPlcy_Mx_Dsply_Recs().ToString();
            }
            this.myNav1.FindNavigationIndices(
          long.Parse(this.dsplySizeCostComboBox.Text), this.totl_rec_Cost);
            if (this.rec_Cost_cur_indx >= this.myNav1.totalGroups)
            {
                this.rec_Cost_cur_indx = this.myNav1.totalGroups - 1;
            }
            if (this.rec_Cost_cur_indx < 0)
            {
                this.rec_Cost_cur_indx = 0;
            }
            this.myNav1.currentNavigationIndex = this.rec_Cost_cur_indx;
        }

        private void updtCostNavLabels()
        {
            this.moveFirstCostButton.Enabled = this.myNav1.moveFirstBtnStatus();
            this.movePreviousCostButton.Enabled = this.myNav1.movePrevBtnStatus();
            this.moveNextCostButton.Enabled = this.myNav1.moveNextBtnStatus();
            this.moveLastCostButton.Enabled = this.myNav1.moveLastBtnStatus();
            this.positionCostTextBox.Text = this.myNav1.displayedRecordsNumbers();
            if (this.is_last_rec_Cost == true ||
             this.totl_rec_Cost != Global.mnFrm.cmCde.Big_Val)
            {
                this.totalRecsCostLabel.Text = this.myNav1.totalRecordsLabel();
            }
            else
            {
                this.totalRecsCostLabel.Text = "of Total";
            }
        }

        private void populateCostGridVw()
        {
            this.obey_evnts = false;
            this.costingDataGridView.Rows.Clear();
            if (this.editRec == false && this.addRec == false)
            {
                disableCostLnsEdit();
            }

            this.obey_evnts = false;
            this.costingDataGridView.DefaultCellStyle.ForeColor = Color.Black;
            DataSet dtst = Global.get_One_AttnCostLns(this.searchForCostTextBox.Text,
              this.searchInCostComboBox.Text,
              this.rec_Cost_cur_indx,
             int.Parse(this.dsplySizeCostComboBox.Text),
             long.Parse(this.rgstrIDTextBox.Text));
            this.costingDataGridView.Rows.Clear();

            int rwcnt = dtst.Tables[0].Rows.Count;
            for (int i = 0; i < rwcnt; i++)
            {
                this.last_rec_Cost_num = this.myNav1.startIndex() + i;
                this.costingDataGridView.RowCount += 1;//.Insert(this.rgstrDetDataGridView.RowCount - 1, 1);
                int rowIdx = this.costingDataGridView.RowCount - 1;

                this.costingDataGridView.Rows[rowIdx].HeaderCell.Value = (i + 1).ToString();
                this.costingDataGridView.Rows[rowIdx].Cells[0].Value = dtst.Tables[0].Rows[i][10].ToString();
                this.costingDataGridView.Rows[rowIdx].Cells[1].Value = "...";
                this.costingDataGridView.Rows[rowIdx].Cells[2].Value = dtst.Tables[0].Rows[i][5].ToString();
                this.costingDataGridView.Rows[rowIdx].Cells[3].Value = dtst.Tables[0].Rows[i][2].ToString();
                this.costingDataGridView.Rows[rowIdx].Cells[4].Value = "...";
                this.costingDataGridView.Rows[rowIdx].Cells[5].Value = double.Parse(dtst.Tables[0].Rows[i][7].ToString()).ToString("#,##0.00");

                this.costingDataGridView.Rows[rowIdx].Cells[6].Value = double.Parse(dtst.Tables[0].Rows[i][6].ToString()).ToString("#,##0.00");

                this.costingDataGridView.Rows[rowIdx].Cells[7].Value = double.Parse(dtst.Tables[0].Rows[i][8].ToString()).ToString("#,##0.00");
                this.costingDataGridView.Rows[rowIdx].Cells[8].Value = double.Parse(dtst.Tables[0].Rows[i][9].ToString()).ToString("#,##0.00");

                this.costingDataGridView.Rows[rowIdx].Cells[9].Value = dtst.Tables[0].Rows[i][4].ToString();
                this.costingDataGridView.Rows[rowIdx].Cells[10].Value = dtst.Tables[0].Rows[i][1].ToString();
                this.costingDataGridView.Rows[rowIdx].Cells[11].Value = dtst.Tables[0].Rows[i][3].ToString();
                this.costingDataGridView.Rows[rowIdx].Cells[12].Value = dtst.Tables[0].Rows[i][0].ToString();
                this.costingDataGridView.Rows[rowIdx].Cells[13].Value = "Create Accounting";
                this.costingDataGridView.Rows[rowIdx].Cells[14].Value = "Reverse Accounting";
                this.costingDataGridView.Rows[rowIdx].Cells[15].Value = Global.getBatchNm(long.Parse(dtst.Tables[0].Rows[i][11].ToString()));
                this.costingDataGridView.Rows[rowIdx].Cells[16].Value = dtst.Tables[0].Rows[i][12].ToString();
                this.costingDataGridView.Rows[rowIdx].Cells[17].Value = dtst.Tables[0].Rows[i][13].ToString();
                this.costingDataGridView.Rows[rowIdx].Cells[18].Value = dtst.Tables[0].Rows[i][14].ToString();
                this.costingDataGridView.Rows[rowIdx].Cells[19].Value = dtst.Tables[0].Rows[i][15].ToString();
                if (int.Parse(dtst.Tables[0].Rows[i][16].ToString()) > 0)
                {
                    this.costingDataGridView.Rows[rowIdx].Cells[3].Style.BackColor = Color.Red;
                }
            }
            this.correctCostNavLbls(dtst);
            this.obey_evnts = true;
        }

        private void correctCostNavLbls(DataSet dtst)
        {
            long totlRecs = dtst.Tables[0].Rows.Count;
            if (this.rec_Cost_cur_indx == 0 && totlRecs == 0)
            {
                this.is_last_rec_Cost = true;
                this.totl_rec_Cost = 0;
                this.last_rec_Cost_num = 0;
                this.rec_Cost_cur_indx = 0;
                this.updtCostTotals();
                this.updtCostNavLabels();
            }
            else if (this.totl_rec_Cost == Global.mnFrm.cmCde.Big_Val
          && totlRecs < long.Parse(this.dsplySizeCostComboBox.Text))
            {
                this.totl_rec_Cost = this.last_rec_Cost_num;
                if (totlRecs == 0)
                {
                    this.rec_Cost_cur_indx -= 1;
                    this.updtCostTotals();
                    this.populateCostGridVw();
                }
                else
                {
                    this.updtCostTotals();
                }
            }
        }

        private bool shdObeyCostEvts()
        {
            return this.obey_evnts;
        }

        private void CostPnlNavButtons(object sender, System.EventArgs e)
        {
            System.Windows.Forms.ToolStripButton sentObj = (System.Windows.Forms.ToolStripButton)sender;
            this.totalRecsCostLabel.Text = "";
            if (sentObj.Name.ToLower().Contains("first"))
            {
                this.is_last_rec_Cost = false;
                this.rec_Cost_cur_indx = 0;
            }
            else if (sentObj.Name.ToLower().Contains("previous"))
            {
                this.is_last_rec_Cost = false;
                this.rec_Cost_cur_indx -= 1;
            }
            else if (sentObj.Name.ToLower().Contains("next"))
            {
                this.is_last_rec_Cost = false;
                this.rec_Cost_cur_indx += 1;
            }
            else if (sentObj.Name.ToLower().Contains("last"))
            {
                this.is_last_rec_Cost = true;
                this.totl_rec_Cost = Global.get_Total_AttnCostLns(this.searchForCostTextBox.Text,
                this.searchInCostComboBox.Text, long.Parse(this.rgstrIDTextBox.Text));
                this.updtCostTotals();
                this.rec_Cost_cur_indx = this.myNav.totalGroups - 1;
            }
            this.getCostPnlData();
        }
        #endregion

        private void evntDateButton_Click(object sender, EventArgs e)
        {
            if (this.addRec == false && this.editRec == false)
            {
                this.editButton.PerformClick();
            }
            if (this.addRec == false && this.editRec == false)
            {
                Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }
            Global.mnFrm.cmCde.selectDate(ref this.evntStrtDateTextBox);
        }

        private void tmTblButton_Click(object sender, EventArgs e)
        {

            if (this.addRec == false && this.editRec == false)
            {
                this.editButton.PerformClick();
            }
            if (this.addRec == false && this.editRec == false)
            {
                Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }
            string[] selVals = new string[1];
            selVals[0] = this.tmTblIDTextBox.Text;
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
             Global.mnFrm.cmCde.getLovID("Time Tables"), ref selVals,
             true, false, Global.mnFrm.cmCde.Org_id,
             this.srchWrd, "Both", true);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.tmTblIDTextBox.Text = selVals[i];
                    this.tmTblNmTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                      "attn.attn_time_table_hdrs", "time_table_id", "time_table_name", long.Parse(selVals[i]));
                }
            }
        }

        private void evntDescButton_Click(object sender, EventArgs e)
        {

            if (this.addRec == false && this.editRec == false)
            {
                this.editButton.PerformClick();
            }
            if (this.addRec == false && this.editRec == false)
            {
                Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }
            if (this.tmTblNmTextBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Please select a Time Table First!", 0);
                return;
            }
            string[] selVals = new string[1];
            selVals[0] = this.tmTblDetIDTextBox.Text;
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
             Global.mnFrm.cmCde.getLovID("Time Table Event Lines"), ref selVals,
             true, false, 1, this.tmTblIDTextBox.Text, "",
             this.srchWrd, "Both", true);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.tmTblDetIDTextBox.Text = selVals[i];
                    this.evntDescTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm("attn.attn_time_table_details", "time_table_det_id",
                      @"'EVENT: ' || COALESCE(attn.get_event_name(event_id),'') || 
              ' VENUE: ' || COALESCE(attn.get_venue_name(assgnd_venue_id),'') || 
              ' HOST: ' || COALESCE(prs.get_prsn_name(assgnd_host_id),'') ",
                      long.Parse(selVals[i]));
                }
                if (long.Parse(this.tmTblDetIDTextBox.Text) > 0)
                {
                    this.rgstrDetDataGridView.Columns[9].Visible = false;
                    this.rgstrDetDataGridView.Columns[10].Visible = false;
                    this.rgstrDetDataGridView.Columns[11].Visible = false;
                    this.rgstrDetDataGridView.Columns[12].Visible = false;
                    this.rgstrDetDataGridView.Columns[19].Visible = true;
                    this.rgstrDetDataGridView.Columns[21].Visible = true;
                    this.rgstrDetDataGridView.Columns[22].Visible = true;
                    this.rgstrDetDataGridView.Columns[23].Visible = true;
                }
                else
                {
                    this.rgstrDetDataGridView.Columns[9].Visible = true;
                    this.rgstrDetDataGridView.Columns[10].Visible = true;
                    this.rgstrDetDataGridView.Columns[11].Visible = true;
                    this.rgstrDetDataGridView.Columns[12].Visible = true;
                    this.rgstrDetDataGridView.Columns[17].Visible = true;
                    this.rgstrDetDataGridView.Columns[19].Visible = false;
                    this.rgstrDetDataGridView.Columns[21].Visible = false;
                    this.rgstrDetDataGridView.Columns[22].Visible = false;
                    this.rgstrDetDataGridView.Columns[23].Visible = false;
                }
                this.populateMetricGridVw();
            }
        }

        private void attnRgstrListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.shdObeyEvts() == false)
            {
                return;
            }
            if (this.attnRgstrListView.SelectedItems.Count > 0)
            {
                this.populateDet(int.Parse(this.attnRgstrListView.SelectedItems[0].SubItems[2].Text));
            }
            else
            {
                this.populateDet(-100000);
            }
        }

        private void positionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            EventArgs ex = new EventArgs();
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up)
            {
                this.PnlNavButtons(this.movePreviousButton, ex);
            }
            else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Down)
            {
                this.PnlNavButtons(this.moveNextButton, ex);
            }
        }

        private void searchForTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            EventArgs ex = new EventArgs();
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                this.goButton_Click(this.goButton, ex);
            }
        }

        private void rfrshButton_Click(object sender, EventArgs e)
        {
            this.loadPanel();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            this.loadPanel();
        }

        private void positionDetTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            EventArgs ex = new EventArgs();
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up)
            {
                this.TdetPnlNavButtons(this.movePreviousDetButton, ex);
            }
            else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Down)
            {
                this.TdetPnlNavButtons(this.moveNextDetButton, ex);
            }
        }

        private void searchForDetTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            EventArgs ex = new EventArgs();
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                this.rfrshDetButton_Click(this.rfrshDetButton, ex);
            }
        }

        private void rfrshDetButton_Click(object sender, EventArgs e)
        {
            this.loadRgstrDetLnsPanel();
        }

        private void rcHstryButton_Click(object sender, EventArgs e)
        {
            if (this.attnRgstrListView.SelectedItems.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select a Record First!", 0);
                return;
            }
            Global.mnFrm.cmCde.showRecHstry(
              Global.mnFrm.cmCde.get_Gnrl_Rec_Hstry(long.Parse(
              this.attnRgstrListView.SelectedItems[0].SubItems[2].Text),
              "attn.attn_attendance_recs_hdr", "recs_hdr_id"), 7);
        }

        private void vwSQLButton_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.showSQL(this.rec_SQL, 6);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[8]) == false)
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }
            this.clearDetInfo();
            this.rgstrDetDataGridView.Rows.Clear();
            this.addRec = true;
            this.editRec = false;
            this.prpareForDetEdit();
            this.prpareForLnsEdit();
            this.addButton.Enabled = false;
            this.editButton.Enabled = false;
            this.rgstrNmTextBox.Text = "EVNT".ToUpper()
            + "-" + Global.mnFrm.cmCde.getDB_Date_time().Substring(0, 11).Replace("-", "") + "-" +
        Global.getNewRgstrID().ToString().PadLeft(4, '0');
            this.evntStrtDateTextBox.Text = Global.mnFrm.cmCde.getFrmtdDB_Date_time().Substring(0, 11) + " 00:00:00";
            this.evntEndDateTextBox.Text = Global.mnFrm.cmCde.getFrmtdDB_Date_time().Substring(0, 11) + " 23:59:59";
            //this.saveButton_Click(this.saveButton, e);
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (this.editButton.Text == "EDIT")
            {
                if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[9]) == false)
                {
                    Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                        " this action!\nContact your System Administrator!", 0);
                    return;
                }
                if (this.rgstrIDTextBox.Text == "" || this.rgstrIDTextBox.Text == "-1")
                {
                    Global.mnFrm.cmCde.showMsg("No record to Edit!", 0);
                    return;
                }
                this.addRec = false;
                this.editRec = true;
                this.prpareForDetEdit();
                this.prpareForLnsEdit();
                this.populateMetricGridVw();
                this.prpareForMetricLnsEdit();
                if (this.editCost)
                {
                    this.prpareForCostLnsEdit();
                }
                this.addButton.Enabled = false;
                this.editButton.Text = "STOP";
                //this.editMenuItem.Text = "STOP EDITING";
            }
            else
            {
                this.saveButton.Enabled = false;
                this.addRec = false;
                this.editRec = false;
                this.editButton.Enabled = this.addRecsP;
                this.addButton.Enabled = this.editRecsP;
                this.editButton.Text = "EDIT";
                //this.editMenuItem.Text = "Edit Item";
                this.disableDetEdit();
                this.disableLnsEdit();
                this.disableMetrcLnsEdit();
                System.Windows.Forms.Application.DoEvents();
                this.loadPanel();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            this.rgstrDetDataGridView.EndEdit();
            this.rgstrDetDataGridView.EndEdit();
            System.Windows.Forms.Application.DoEvents();
            this.rgstrNmTextBox.Focus();
            System.Windows.Forms.Application.DoEvents();
            if (this.addRec == true)
            {
                if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[8]) == false)
                {
                    Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                     " this action!\nContact your System Administrator!", 0);
                    return;
                }
            }
            else
            {
                if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[9]) == false)
                {
                    Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                     " this action!\nContact your System Administrator!", 0);
                    return;
                }
            }
            if (this.rgstrNmTextBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Please enter a Register name!", 0);
                return;
            }
            if (this.rgstrDescTextBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Please provide a Description for the Register!", 0);
                return;
            }
            long oldRecID = Global.getAttnRgstrID(this.rgstrNmTextBox.Text,
                Global.mnFrm.cmCde.Org_id);
            if (oldRecID > 0
             && this.addRec == true)
            {
                Global.mnFrm.cmCde.showMsg("Register Name is already in use in this Organisation!", 0);
                return;
            }
            if (oldRecID > 0
             && this.editRec == true
             && oldRecID.ToString() !=
             this.rgstrIDTextBox.Text)
            {
                Global.mnFrm.cmCde.showMsg("New Register Name is already in use in this Organisation!", 0);
                return;
            }

            if (this.evntStrtDateTextBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Start Date cannot be Empty!", 0);
                return;
            }
            if (this.evntEndDateTextBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("End Date cannot be Empty!", 0);
                return;
            }

            if (this.addRec == true)
            {
                Global.createAttnRgstr(Global.mnFrm.cmCde.Org_id, this.rgstrNmTextBox.Text,
                  this.rgstrDescTextBox.Text, int.Parse(this.tmTblIDTextBox.Text),
                  long.Parse(this.tmTblDetIDTextBox.Text), this.evntStrtDateTextBox.Text, this.evntEndDateTextBox.Text);

                this.saveButton.Enabled = false;
                this.addRec = false;
                this.editRec = false;
                this.editButton.Enabled = this.addRecsP;
                this.addButton.Enabled = this.editRecsP;

                //Global.mnFrm.cmCde.showMsg("Record Saved!", 3);
                System.Windows.Forms.Application.DoEvents();
                this.rgstrIDTextBox.Text = Global.getAttnRgstrID(this.rgstrNmTextBox.Text,
                  Global.mnFrm.cmCde.Org_id).ToString();
                this.searchInComboBox.SelectedIndex = 3;
                this.searchForTextBox.Text = this.rgstrIDTextBox.Text;
                //this.saveGridView(int.Parse(this.tmetblIDTextBox.Text));
                this.loadPanel();
                this.editButton_Click(this.editButton, e);
            }
            else if (this.editRec == true)
            {
                Global.updateAttnRgstr(long.Parse(this.rgstrIDTextBox.Text), this.rgstrNmTextBox.Text,
                  this.rgstrDescTextBox.Text, int.Parse(this.tmTblIDTextBox.Text),
                  long.Parse(this.tmTblDetIDTextBox.Text), this.evntStrtDateTextBox.Text, this.evntEndDateTextBox.Text);

                //this.saveGridView(int.Parse(this.tmetblIDTextBox.Text));

                if (this.attnRgstrListView.SelectedItems.Count > 0)
                {
                    this.attnRgstrListView.SelectedItems[0].SubItems[1].Text = this.rgstrNmTextBox.Text;
                }
                if (this.attendRecsTabControl.SelectedTab.Equals(this.tabPage2))
                {
                    this.saveMtrcsButton_Click(this.saveMtrcsButton, e);
                }
                else
                {
                    Global.mnFrm.cmCde.showMsg("Record Saved!", 3);
                }
            }


        }

        private void delButton_Click(object sender, EventArgs e)
        {
            if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[10]) == false)
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                 " this action!\nContact your System Administrator!", 0);
                return;
            }

            if (this.rgstrIDTextBox.Text == "" || this.rgstrIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("Please select the Record to DELETE!", 0);
                return;
            }

            if (Global.isAttnRgstrInUse(long.Parse(this.rgstrIDTextBox.Text)) == true)
            {
                Global.mnFrm.cmCde.showMsg("This Register is in Use!", 0);
                return;
            }
            if (Global.mnFrm.cmCde.showMsg("Are you sure you want to DELETE the selected Register?" +
       "\r\nThis action cannot be undone!", 1) == DialogResult.No)
            {
                //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                return;
            }
            Global.deleteAttnRgstr(int.Parse(this.rgstrIDTextBox.Text), this.rgstrNmTextBox.Text);
            this.loadPanel();
        }

        private void loadRgstrPrsnsButton_Click(object sender, EventArgs e)
        {
            if (this.addRec == false && this.editRec == false)
            {
                this.editButton.PerformClick();
            }
            if (this.addRec == false && this.editRec == false)
            {
                Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            if (this.tmTblDetIDTextBox.Text == "-1"
        || this.tmTblDetIDTextBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Please indicate the Event First!", 0);
                return;
            }
            long evntID = long.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
              "attn.attn_time_table_details", "time_table_det_id", "event_id",
              long.Parse(this.tmTblDetIDTextBox.Text)));
            string grpTyp = Global.mnFrm.cmCde.getGnrlRecNm(
              "attn.attn_attendance_events", "event_id", "allwd_grp_typ",
              evntID);
            string grpNm = Global.mnFrm.cmCde.getGnrlRecNm(
             "attn.attn_attendance_events", "event_id", "allwd_group_nm",
             evntID);
            int grpID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
              "attn.attn_attendance_events", "event_id", "allwd_grp_id",
              evntID));
            string grpSQL = "";
            if (grpTyp == "Divisions/Groups")
            {
                grpSQL = "Select distinct a.person_id From pasn.prsn_divs_groups a Where ((a.div_id = " +
                  grpID + ") and (to_timestamp('" + dateStr +
                  "','YYYY-MM-DD HH24:MI:SS') between to_timestamp(a.valid_start_date|| ' 00:00:00','YYYY-MM-DD HH24:MI:SS') " +
                    "AND to_timestamp(a.valid_end_date || ' 23:59:59','YYYY-MM-DD HH24:MI:SS'))) ORDER BY a.person_id";
            }
            else if (grpTyp == "Grade")
            {
                grpSQL = "Select distinct a.person_id From pasn.prsn_grades a Where ((a.grade_id = " +
                  grpID + ") and (to_timestamp('" + dateStr +
                  "','YYYY-MM-DD HH24:MI:SS') between to_timestamp(a.valid_start_date|| ' 00:00:00','YYYY-MM-DD HH24:MI:SS') " +
                    "AND to_timestamp(a.valid_end_date || ' 23:59:59','YYYY-MM-DD HH24:MI:SS'))) ORDER BY a.person_id";
            }
            else if (grpTyp == "Job")
            {
                grpSQL = "Select distinct a.person_id From pasn.prsn_jobs a Where ((a.job_id = " +
                  grpID + ") and (to_timestamp('" + dateStr +
                  "','YYYY-MM-DD HH24:MI:SS') between to_timestamp(a.valid_start_date|| ' 00:00:00','YYYY-MM-DD HH24:MI:SS') " +
                    "AND to_timestamp(a.valid_end_date || ' 23:59:59','YYYY-MM-DD HH24:MI:SS'))) ORDER BY a.person_id";
            }
            else if (grpTyp == "Position")
            {
                grpSQL = "Select distinct a.person_id From pasn.prsn_positions a Where ((a.position_id = " +
                  grpID + ") and (to_timestamp('" + dateStr +
                  "','YYYY-MM-DD HH24:MI:SS') between to_timestamp(a.valid_start_date|| ' 00:00:00','YYYY-MM-DD HH24:MI:SS') " +
                    "AND to_timestamp(a.valid_end_date || ' 23:59:59','YYYY-MM-DD HH24:MI:SS'))) ORDER BY a.person_id";
            }
            else if (grpTyp == "Site/Location")
            {
                grpSQL = "Select distinct a.person_id From pasn.prsn_locations a Where ((a.location_id = " +
                  grpID + ") and (to_timestamp('" + dateStr +
                  "','YYYY-MM-DD HH24:MI:SS') between to_timestamp(a.valid_start_date|| ' 00:00:00','YYYY-MM-DD HH24:MI:SS') " +
                    "AND to_timestamp(a.valid_end_date || ' 23:59:59','YYYY-MM-DD HH24:MI:SS'))) ORDER BY a.person_id";
            }
            else if (grpTyp == "Person Type")
            {
                grpSQL = "Select distinct a.person_id From pasn.prsn_prsntyps a, prs.prsn_names_nos b " +
                  "Where ((a.person_id = b.person_id) and (b.org_id = " + Global.mnFrm.cmCde.Org_id + ") and (a.prsn_type = '" +
                  grpNm.Replace("'", "''") + "') and (to_timestamp('" + dateStr +
                  "','YYYY-MM-DD HH24:MI:SS') between to_timestamp(a.valid_start_date|| ' 00:00:00','YYYY-MM-DD HH24:MI:SS') " +
                    "AND to_timestamp(a.valid_end_date || ' 23:59:59','YYYY-MM-DD HH24:MI:SS'))) ORDER BY a.person_id";
            }
            else if (grpTyp == "Working Hour Type")
            {
                grpSQL = "Select distinct a.person_id From pasn.prsn_work_id a Where ((a.work_hour_id = " +
                  grpID + ") and (to_timestamp('" + dateStr +
                  "','YYYY-MM-DD HH24:MI:SS') between to_timestamp(a.valid_start_date|| ' 00:00:00','YYYY-MM-DD HH24:MI:SS') " +
                    "AND to_timestamp(a.valid_end_date || ' 23:59:59','YYYY-MM-DD HH24:MI:SS'))) ORDER BY a.person_id";
            }
            else if (grpTyp == "Gathering Type")
            {
                grpSQL = "Select distinct a.person_id From pasn.prsn_gathering_typs a Where ((a.gatherng_typ_id = " +
                  grpID + ") and (to_timestamp('" + dateStr +
                  "','YYYY-MM-DD HH24:MI:SS') between to_timestamp(a.valid_start_date|| ' 00:00:00','YYYY-MM-DD HH24:MI:SS') " +
                    "AND to_timestamp(a.valid_end_date || ' 23:59:59','YYYY-MM-DD HH24:MI:SS'))) ORDER BY a.person_id";
            }
            else
            {
                grpSQL = "Select distinct a.person_id From prs.prsn_names_nos a Where ((a.org_id = " + Global.mnFrm.cmCde.Org_id + ")) ORDER BY a.person_id";
            }
            //Global.mnFrm.cmCde.showSQLNoPermsn(grpSQL);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(grpSQL);
            long[] prsnIDs = new long[dtst.Tables[0].Rows.Count];
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                prsnIDs[i] = long.Parse(dtst.Tables[0].Rows[i][0].ToString());
            }
            if (Global.mnFrm.cmCde.showMsg("Are you sure you want to Load the Person(s) in the Selected Group?"
        + "\r\nThere are " + prsnIDs.Length + " Person(s) involved!\r\n", 1) == DialogResult.No)
            {
                //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                return;
            }
            for (int a = 0; a < prsnIDs.Length; a++)
            {
                //Person Pay Items
                long prsitmid = Global.doesRgstrHvPrsn(prsnIDs[a],
                  long.Parse(this.rgstrIDTextBox.Text));
                if (prsitmid <= 0)
                {
                    long cstmrID = this.checkNCreateCstmr(prsnIDs[a]);

                    Global.createAttnRgstrDetLn(long.Parse(this.rgstrIDTextBox.Text), prsnIDs[a]
                      , "", "", false, "", Global.mnFrm.cmCde.getPrsnSurNameFrst(prsnIDs[a]), 1, cstmrID, "Existing Person", -1);
                }
                else
                {
                }
            }

            Global.mnFrm.cmCde.showMsg("Successfully Loaded the Allowed Persons!", 3);
            this.rfrshDetButton_Click(this.rfrshDetButton, e);
        }

        private void deleteDetButton_Click(object sender, EventArgs e)
        {

            if (this.addRec == false && this.editRec == false)
            {
                this.editButton.PerformClick();
            }
            if (this.addRec == false && this.editRec == false)
            {
                Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }
            if (this.rgstrDetDataGridView.CurrentCell != null && this.rgstrDetDataGridView.SelectedRows.Count <= 0)
            {
                this.rgstrDetDataGridView.Rows[this.rgstrDetDataGridView.CurrentCell.RowIndex].Selected = true;
            }

            if (this.rgstrDetDataGridView.SelectedRows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select the Record(s) to Delete!", 0);
                return;
            }

            if (Global.mnFrm.cmCde.showMsg("Are you sure you want to DELETE the selected Item?" +
      "\r\nThis action cannot be undone!", 1) == DialogResult.No)
            {
                //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                return;
            }
            for (int i = 0; i < this.rgstrDetDataGridView.SelectedRows.Count; i++)
            {
                long lnID = -1;
                long.TryParse(this.rgstrDetDataGridView.SelectedRows[i].Cells[15].Value.ToString(), out lnID);
                if (this.rgstrDetDataGridView.SelectedRows[i].Cells[2].Value == null)
                {
                    this.rgstrDetDataGridView.SelectedRows[i].Cells[2].Value = string.Empty;
                }
                if (Global.isAttnRgstrLnInUse(lnID) == false)
                {
                    Global.deleteAttnRgstrDLn(lnID, this.rgstrDetDataGridView.SelectedRows[i].Cells[2].Value.ToString());
                }
            }
            this.rfrshDetButton.PerformClick();
        }

        private void vwSQLDetButton_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.showSQL(this.rec_det_SQL, 6);
        }

        private void rcHstryDetButton_Click(object sender, EventArgs e)
        {
            if (this.rgstrDetDataGridView.CurrentCell != null && this.rgstrDetDataGridView.SelectedRows.Count <= 0)
            {
                this.rgstrDetDataGridView.Rows[this.rgstrDetDataGridView.CurrentCell.RowIndex].Selected = true;
            }

            if (this.rgstrDetDataGridView.SelectedRows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select a Record First!", 0);
                return;
            }
            Global.mnFrm.cmCde.showRecHstry(
              Global.mnFrm.cmCde.get_Gnrl_Rec_Hstry(long.Parse(
              this.rgstrDetDataGridView.SelectedRows[0].Cells[15].Value.ToString()),
              "attn.attn_attendance_recs", "attnd_rec_id"), 7);
        }

        private long checkNCreateCstmr(long prsnID)
        {
            long cstmrID = -1;
            long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
      "scm.scm_cstmr_suplr", "lnkd_prsn_id", "cust_sup_id",
      prsnID), out cstmrID);
            if (cstmrID <= 0)
            {
                DataSet prsDtst = Global.get_PrsnCstmrDet(prsnID);
                if (prsDtst.Tables[0].Rows.Count > 0)
                {
                    string fllnm = prsDtst.Tables[0].Rows[0][0].ToString();
                    string gndr = prsDtst.Tables[0].Rows[0][1].ToString();

                    string dob = prsDtst.Tables[0].Rows[0][2].ToString();

                    string telNos = prsDtst.Tables[0].Rows[0][3].ToString();
                    string eml = prsDtst.Tables[0].Rows[0][4].ToString();
                    string siteNm = "OFFICE";// Global.mnFrm.cmCde.getOrgName(Global.mnFrm.cmCde.Org_id);
                    string bllng = prsDtst.Tables[0].Rows[0][5].ToString();
                    string shpAdrs = prsDtst.Tables[0].Rows[0][6].ToString();

                    string ntnlty = prsDtst.Tables[0].Rows[0][7].ToString();

                    Global.createCstSplrRec(Global.mnFrm.cmCde.Org_id, fllnm, fllnm, "Customer", "Individual",
                      Global.get_DfltSalesLbltyAcnt(Global.mnFrm.cmCde.Org_id),
                      Global.get_DfltRcvblAcnt(Global.mnFrm.cmCde.Org_id), prsnID, gndr, dob);
                    long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
          "scm.scm_cstmr_suplr", "lnkd_prsn_id", "cust_sup_id",
          prsnID), out cstmrID);
                    if (cstmrID > 0)
                    {
                        Global.createCstSplrSiteRec(cstmrID, siteNm, siteNm, fllnm, telNos, eml, "", "", "", bllng, shpAdrs, -1,
                          -1, "", ntnlty, "", "", "", "", "");
                    }
                }

            }
            return cstmrID;
        }

        private void rgstrDetDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e == null || this.obey_evnts == false)
            {
                return;
            }
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            bool prv = this.obey_evnts;
            this.obey_evnts = false;


            this.dfltFill(e.RowIndex);
            if (e.ColumnIndex == 8
              || e.ColumnIndex == 10
              || e.ColumnIndex == 2
              || e.ColumnIndex == 5)
            {

                if (this.addRec == false && this.editRec == false)
                {
                    this.editButton.PerformClick();
                }
                if (this.addRec == false && this.editRec == false)
                {
                    Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                    this.obey_evnts = true;
                    return;
                }
            }
            if (e.ColumnIndex == 2)
            {
                int[] selVals = new int[1];
                selVals[0] = Global.mnFrm.cmCde.getPssblValID(
                  this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString(),
                  Global.mnFrm.cmCde.getLovID("Visitor Classifications"));
                DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                    Global.mnFrm.cmCde.getLovID("Visitor Classifications"), ref selVals,
                    true, false,
                 this.srchWrd, "Both", true);
                if (dgRes == DialogResult.OK)
                {
                    for (int i = 0; i < selVals.Length; i++)
                    {
                        this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[1].Value = Global.mnFrm.cmCde.getPssblValNm(
                          selVals[i]);
                    }
                    this.obey_evnts = true;
                    DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(1, e.RowIndex);
                    this.rgstrDetDataGridView_CellValueChanged(this.rgstrDetDataGridView, ex);

                }
            }
            else if (e.ColumnIndex == 5)
            {
                string lovNm = "Active Persons";
                string csfctn = this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                string[] selVals = new string[1];
                int[] selVals1 = new int[1];
                int idxWrk = 4;
                if (csfctn == "Existing Person")
                {
                    idxWrk = 3;
                    selVals[0] = this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[idxWrk].Value.ToString();
                    DialogResult dgRes1 = Global.mnFrm.cmCde.showPssblValDiag(
                        Global.mnFrm.cmCde.getLovID(lovNm), ref selVals,
                        true, false, Global.mnFrm.cmCde.Org_id,
                     this.srchWrd, "Both", true);
                    if (dgRes1 == DialogResult.OK)
                    {
                        for (int i = 0; i < selVals.Length; i++)
                        {
                            long prsnID = -1;
                            long cstmrID = -1;
                            string fullNm = "";
                            long spnsrID = -1;
                            string spnsrNm = "";

                            if (csfctn == "Existing Person")
                            {
                                cstmrID = -1;
                                prsnID = Global.mnFrm.cmCde.getPrsnID(selVals[i]);
                                fullNm = Global.mnFrm.cmCde.getPrsnSurNameFrst(prsnID) + " (" + Global.mnFrm.cmCde.getPrsnLocID(prsnID) + ")";
                                spnsrID = Global.mnFrm.cmCde.getPrsnLnkdFirmID(prsnID);
                                spnsrNm = Global.mnFrm.cmCde.getCstmrSpplrName(spnsrID);

                                long prsitmid = Global.doesRgstrHvPrsn(prsnID,
                    long.Parse(this.rgstrIDTextBox.Text));
                                if (prsitmid > 0 &&
                                  prsnID != long.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[idxWrk].Value.ToString()))
                                {
                                    Global.mnFrm.cmCde.showMsg("Person already exists in this Register!", 0);
                                    this.obey_evnts = true;
                                    return;
                                }
                                long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
                                "scm.scm_cstmr_suplr", "lnkd_prsn_id", "cust_sup_id",
                                prsnID), out cstmrID);
                                if (cstmrID <= 0)
                                {
                                    cstmrID = this.checkNCreateCstmr(prsnID);
                                }
                                this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[16].Value = cstmrID;
                                this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[3].Value = prsnID.ToString();
                                this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[20].Value = spnsrID;
                                this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[21].Value = spnsrNm;
                            }
                            this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[4].Value = fullNm;
                        }

                        this.obey_evnts = true;
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(4, e.RowIndex);
                        this.rgstrDetDataGridView_CellValueChanged(this.rgstrDetDataGridView, ex);

                    }
                }
                else if (csfctn == "Customer")
                {
                    idxWrk = 16;
                    lovNm = "Customers";
                    long prsnID = -1;
                    long cstspplID = long.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[idxWrk].Value.ToString());
                    long siteID = -1;
                    bool isReadOnly = true;
                    if (this.addRec || this.editRec)
                    {
                        isReadOnly = false;
                    }
                    Global.mnFrm.cmCde.showCstSpplrDiag(ref cstspplID, ref siteID, true, false, "%",
                      "Customer/Supplier Name", false, isReadOnly, Global.mnFrm.cmCde, "Customer");

                    string fullNm = Global.get_One_CstmrNm(cstspplID);

                    long cstmritmid = Global.doesRgstrHvCstmr(cstspplID,
          long.Parse(this.rgstrIDTextBox.Text));
                    if (cstmritmid > 0
                      && cstspplID != int.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[idxWrk].Value.ToString()))
                    {
                        Global.mnFrm.cmCde.showMsg("Customer already exists in this Register!", 0);
                        this.obey_evnts = true;
                        return;
                    }

                    long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
          "scm.scm_cstmr_suplr", "cust_sup_id", "lnkd_prsn_id",
          cstspplID), out prsnID);

                    this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[3].Value = prsnID;
                    this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[16].Value = cstspplID.ToString();
                    this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[4].Value = fullNm;


                    this.obey_evnts = true;
                    DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(4, e.RowIndex);
                    this.rgstrDetDataGridView_CellValueChanged(this.rgstrDetDataGridView, ex);
                    //this.sponsorIDTextBox.Text = cstspplID.ToString();
                    //this.sponsorSiteIDTextBox.Text = siteID.ToString();
                    //this.sponsorNmTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                    //    "scm.scm_cstmr_suplr", "cust_sup_id", "cust_sup_name",
                    //    cstspplID);
                }
                else
                {
                    lovNm = "Ad hoc Visitors";
                    idxWrk = 4;
                    selVals1[0] = Global.mnFrm.cmCde.getPssblValID(
                      this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString(),
                      Global.mnFrm.cmCde.getLovID(lovNm));
                    DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                        Global.mnFrm.cmCde.getLovID(lovNm), ref selVals1,
                        true, false,
                     this.srchWrd, "Both", false);
                    if (dgRes == DialogResult.OK)
                    {
                        for (int i = 0; i < selVals1.Length; i++)
                        {
                            this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[4].Value = Global.mnFrm.cmCde.getPssblValNm(
                              selVals1[i]);
                            this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[16].Value = "-1";
                            this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[3].Value = "-1";
                        }
                        this.obey_evnts = true;
                        DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(4, e.RowIndex);
                        this.rgstrDetDataGridView_CellValueChanged(this.rgstrDetDataGridView, ex);
                    }
                    this.obey_evnts = true;
                    return;
                }
            }
            else if (e.ColumnIndex == 8)
            {
                this.textBox1.Text = this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[7].Value.ToString();
                Global.mnFrm.cmCde.selectDate(ref this.textBox1);
                this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[7].Value = this.textBox1.Text;
                this.rgstrDetDataGridView.EndEdit();

                this.obey_evnts = true;
                DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(7, e.RowIndex);
                this.rgstrDetDataGridView_CellValueChanged(this.rgstrDetDataGridView, ex);
            }
            else if (e.ColumnIndex == 10)
            {
                long attrecid = long.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[15].Value.ToString());
                if (attrecid <= 0)
                {
                    Global.mnFrm.cmCde.showMsg("Please save this Attendance Record First!", 0);
                }
                timeDetailsDiag nwDiag = new timeDetailsDiag();
                nwDiag.attnRecID = attrecid;
                nwDiag.Text = "Attendance Time Details for " + this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
                Global.attndDiag = nwDiag;
                if (nwDiag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                }
                Global.attndDiag = null;
                this.rfrshDetButton.PerformClick();
                //this.textBox2.Text = this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[9].Value.ToString();
                //Global.mnFrm.cmCde.selectDate(ref this.textBox2);
                //this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[9].Value = this.textBox2.Text;
                //this.rgstrDetDataGridView.EndEdit();

                //this.obey_evnts = true;
                //DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(9, e.RowIndex);
                //this.rgstrDetDataGridView_CellValueChanged(this.rgstrDetDataGridView, ex);
            }
            else if (e.ColumnIndex == 17)
            {
                attnScoresDiag nwDiag = new attnScoresDiag();
                nwDiag.recLineID = long.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[15].Value.ToString());
                nwDiag.rdOnly = !this.editRecsP;
                nwDiag.tmtblDetID = long.Parse(this.tmTblDetIDTextBox.Text);
                nwDiag.ShowDialog();
            }
            else if (e.ColumnIndex == 18)
            {
                if (this.rgstrIDTextBox.Text == "" ||
            this.rgstrIDTextBox.Text == "-1")
                {
                    Global.mnFrm.cmCde.showMsg("Please select a saved Register First!", 0);
                    this.obey_evnts = true;
                    return;
                }

                attchmntsDiag nwDiag = new attchmntsDiag();
                if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[9]) == false)
                {
                    nwDiag.addButton.Enabled = false;
                    nwDiag.addButton.Visible = false;
                    nwDiag.editButton.Enabled = false;
                    nwDiag.editButton.Visible = false;
                    nwDiag.delButton.Enabled = false;
                    nwDiag.delButton.Visible = false;
                }
                //Global.mnFrm.cmCde.showMsg("Cannot add Transactions to already Posted Batch of Transactions!", 0);
                //return;
                nwDiag.batchid = long.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[15].Value.ToString());
                nwDiag.batchHdrID = long.Parse(this.rgstrIDTextBox.Text);
                DialogResult dgres = nwDiag.ShowDialog();
                if (dgres == DialogResult.OK)
                {
                }
                //this.textBox2.Text = this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[9].Value.ToString();
                //Global.mnFrm.cmCde.selectDate(ref this.textBox2);
                //this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[9].Value = this.textBox2.Text;
                //this.rgstrDetDataGridView.EndEdit();

                //this.obey_evnts = true;
                //DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(9, e.RowIndex);
                //this.rgstrDetDataGridView_CellValueChanged(this.rgstrDetDataGridView, ex);
            }
            else if (e.ColumnIndex == 19)
            {
                if (this.tmTblDetIDTextBox.Text == "" ||
            this.tmTblDetIDTextBox.Text == "-1")
                {
                    Global.mnFrm.cmCde.showMsg("Please select an Event First!", 0);
                    this.obey_evnts = true;
                    return;
                }
                if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[28]) == false)
                {
                    Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                        " this action!\nContact your System Administrator!", 0);
                    this.obey_evnts = true;
                    return;
                }
                checkinsForm nwDiag = new checkinsForm();
                nwDiag.BackColor = this.BackColor;
                Global.wfnCheckinsDiag = nwDiag;
                nwDiag.strdDte = this.evntStrtDateTextBox.Text;
                nwDiag.endDte = this.evntEndDateTextBox.Text;
                nwDiag.evntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("attn.attn_time_table_details",
                  "time_table_det_id", "event_id", long.Parse(this.tmTblDetIDTextBox.Text)));
                nwDiag.registerID = long.Parse(this.rgstrIDTextBox.Text);
                nwDiag.tmTblID = long.Parse(this.tmTblIDTextBox.Text);
                nwDiag.tmTblDetID = long.Parse(this.tmTblDetIDTextBox.Text);
                long csmrID = long.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[16].Value.ToString());
                long prsnID = long.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[3].Value.ToString());
                long spnsrID = long.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[20].Value.ToString());
                if (csmrID <= 0)
                {
                    Global.mnFrm.cmCde.showMsg("Please select a Customer or an Existing Person First!", 0);
                    this.obey_evnts = true;
                    return;
                }
                nwDiag.inptCstmrID = csmrID;
                nwDiag.showAll = false;
                nwDiag.inptSpnsrID = spnsrID;

                DialogResult dgres = nwDiag.ShowDialog();
                if (dgres == DialogResult.OK)
                {

                }
                this.populateTdetGridVw();
            }
            else if (e.ColumnIndex == 22)
            {
                string[] selVals = new string[1];
                int[] selVals1 = new int[1];
                int idxWrk = 0;
                idxWrk = 20;
                string lovNm = "Customers";
                long prsnID = -1;
                long cstspplID = long.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[idxWrk].Value.ToString());
                long siteID = -1;
                bool isReadOnly = true;
                if (this.addRec || this.editRec)
                {
                    isReadOnly = false;
                }
                Global.mnFrm.cmCde.showCstSpplrDiag(ref cstspplID, ref siteID, true, false, "%",
                  "Customer/Supplier Name", false, isReadOnly, Global.mnFrm.cmCde, "Customer");

                string fullNm = Global.get_One_CstmrNm(cstspplID);

                //        long cstmritmid = Global.doesRgstrHvCstmr(cstspplID,
                //long.Parse(this.rgstrIDTextBox.Text));
                //        if (cstmritmid > 0
                //          && cstspplID != int.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[idxWrk].Value.ToString()))
                //        {
                //          Global.mnFrm.cmCde.showMsg("Customer already exists in this Register!", 0);
                //          this.obey_evnts = true;
                //          return;
                //        }

                //        long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
                //"scm.scm_cstmr_suplr", "cust_sup_id", "lnkd_prsn_id",
                //cstspplID), out prsnID);

                //        this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[3].Value = prsnID;
                this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[20].Value = cstspplID.ToString();
                this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[21].Value = fullNm;


                this.obey_evnts = true;
                DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(4, e.RowIndex);
                this.rgstrDetDataGridView_CellValueChanged(this.rgstrDetDataGridView, ex);
            }

            this.obey_evnts = true;
        }

        private void dfltFill(int rwIdx)
        {
            if (this.rgstrDetDataGridView.Rows[rwIdx].Cells[6].Value == null)
            {
                this.rgstrDetDataGridView.Rows[rwIdx].Cells[6].Value = false;
            }
            if (this.rgstrDetDataGridView.Rows[rwIdx].Cells[10].Value == null)
            {
                this.rgstrDetDataGridView.Rows[rwIdx].Cells[10].Value = "";
            }

            if (this.rgstrDetDataGridView.Rows[rwIdx].Cells[7].Value == null)
            {
                this.rgstrDetDataGridView.Rows[rwIdx].Cells[7].Value = "";
            }
            if (this.rgstrDetDataGridView.Rows[rwIdx].Cells[9].Value == null)
            {
                this.rgstrDetDataGridView.Rows[rwIdx].Cells[9].Value = "";
            }
            if (this.rgstrDetDataGridView.Rows[rwIdx].Cells[3].Value == null)
            {
                this.rgstrDetDataGridView.Rows[rwIdx].Cells[3].Value = "-1";
            }
            if (this.rgstrDetDataGridView.Rows[rwIdx].Cells[14].Value == null)
            {
                this.rgstrDetDataGridView.Rows[rwIdx].Cells[14].Value = "-1";
            }
            if (this.rgstrDetDataGridView.Rows[rwIdx].Cells[15].Value == null)
            {
                this.rgstrDetDataGridView.Rows[rwIdx].Cells[15].Value = "-1";
            }
            if (this.rgstrDetDataGridView.Rows[rwIdx].Cells[16].Value == null)
            {
                this.rgstrDetDataGridView.Rows[rwIdx].Cells[16].Value = "-1";
            }
        }

        private void dfltFill1(int rwIdx)
        {
            if (this.costingDataGridView.Rows[rwIdx].Cells[0].Value == null)
            {
                this.costingDataGridView.Rows[rwIdx].Cells[0].Value = string.Empty;
            }
            if (this.costingDataGridView.Rows[rwIdx].Cells[2].Value == null)
            {
                this.costingDataGridView.Rows[rwIdx].Cells[2].Value = "";
            }

            if (this.costingDataGridView.Rows[rwIdx].Cells[3].Value == null)
            {
                this.costingDataGridView.Rows[rwIdx].Cells[3].Value = "";
            }
            if (this.costingDataGridView.Rows[rwIdx].Cells[5].Value == null)
            {
                this.costingDataGridView.Rows[rwIdx].Cells[5].Value = "0";
            }
            if (this.costingDataGridView.Rows[rwIdx].Cells[6].Value == null)
            {
                this.costingDataGridView.Rows[rwIdx].Cells[6].Value = "0";
            }
            if (this.costingDataGridView.Rows[rwIdx].Cells[7].Value == null)
            {
                this.costingDataGridView.Rows[rwIdx].Cells[7].Value = "0";
            }
            if (this.costingDataGridView.Rows[rwIdx].Cells[8].Value == null)
            {
                this.costingDataGridView.Rows[rwIdx].Cells[8].Value = "0";
            }
            if (this.costingDataGridView.Rows[rwIdx].Cells[9].Value == null)
            {
                this.costingDataGridView.Rows[rwIdx].Cells[9].Value = "";
            }
            if (this.costingDataGridView.Rows[rwIdx].Cells[10].Value == null)
            {
                this.costingDataGridView.Rows[rwIdx].Cells[10].Value = "-1";
            }
            if (this.costingDataGridView.Rows[rwIdx].Cells[11].Value == null)
            {
                this.costingDataGridView.Rows[rwIdx].Cells[11].Value = "-1";
            }
            if (this.costingDataGridView.Rows[rwIdx].Cells[12].Value == null)
            {
                this.costingDataGridView.Rows[rwIdx].Cells[12].Value = "-1";
            }
            if (this.costingDataGridView.Rows[rwIdx].Cells[16].Value == null)
            {
                this.costingDataGridView.Rows[rwIdx].Cells[16].Value = "";
            }
            if (this.costingDataGridView.Rows[rwIdx].Cells[17].Value == null)
            {
                this.costingDataGridView.Rows[rwIdx].Cells[17].Value = "-1";
            }
            if (this.costingDataGridView.Rows[rwIdx].Cells[18].Value == null)
            {
                this.costingDataGridView.Rows[rwIdx].Cells[18].Value = "";
            }
            if (this.costingDataGridView.Rows[rwIdx].Cells[19].Value == null)
            {
                this.costingDataGridView.Rows[rwIdx].Cells[19].Value = "-1";
            }
            if (this.costingDataGridView.Rows[rwIdx].Cells[15].Value == null)
            {
                this.costingDataGridView.Rows[rwIdx].Cells[15].Value = "";
            }
        }

        private void rgstrDetDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e == null || this.obey_evnts == false || (this.addRec == false && this.editRec == false))
            {
                return;
            }
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            bool prv = this.obey_evnts;
            this.obey_evnts = false;

            this.dfltFill(e.RowIndex);

            if (e.ColumnIndex >= 0 && e.ColumnIndex <= 11)
            {
                this.rgstrDetDataGridView.EndEdit();
                System.Windows.Forms.Application.DoEvents();

                string dtetmin = this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[7].Value.ToString();
                string dtetmout = this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[9].Value.ToString();
                if (e.ColumnIndex == 7 && dtetmin != "")
                {
                    dtetmin = Global.mnFrm.cmCde.checkNFormatDate(dtetmin);
                    this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[7].Value = dtetmin;
                    this.rgstrDetDataGridView.EndEdit();
                    System.Windows.Forms.Application.DoEvents();
                }
                if (e.ColumnIndex == 9 && dtetmout != "")
                {
                    dtetmout = Global.mnFrm.cmCde.checkNFormatDate(dtetmout);
                    this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[9].Value = dtetmout;
                    this.rgstrDetDataGridView.EndEdit();
                    System.Windows.Forms.Application.DoEvents();
                }
                long row_id = long.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[15].Value.ToString());
                long rgstrid = long.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[14].Value.ToString());
                long prsnid = long.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[3].Value.ToString());
                bool isprsnt = (bool)(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[6].Value);
                string attncmnts = this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[11].Value.ToString();

                string name_desc = this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
                int noAdlts = int.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString());
                int noChdn = 0;// int.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[13].Value.ToString());
                long cstmrID = long.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[16].Value.ToString());
                long sponsor_id = long.Parse(this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[20].Value.ToString());

                string vstrClsf = this.rgstrDetDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();

                Global.updtAttnRgstrDetLn(row_id, rgstrid, prsnid, dtetmin, dtetmout,
                  isprsnt, attncmnts, name_desc, noAdlts, cstmrID, vstrClsf, sponsor_id);
                this.rgstrDetDataGridView.EndEdit();
            }
            this.obey_evnts = true;
        }

        private void attndRecsForm_KeyDown(object sender, KeyEventArgs e)
        {
            EventArgs ex = new EventArgs();
            if (e.Control && e.KeyCode == Keys.S)       // Ctrl-S Save
            {
                // do what you want here
                if (this.rgstrDetDataGridView.Focused)
                {
                    this.saveButton.PerformClick();
                }
                else if (this.metricsDataGridView.Focused)
                {
                    this.saveButton.PerformClick();
                    //this.saveMtrcsButton.PerformClick();
                }
                else
                {
                    this.saveButton.PerformClick();
                }
                e.SuppressKeyPress = true;  // stops bing! also sets handeled which stop event bubbling
            }
            else if (e.Control && e.KeyCode == Keys.N)       // Ctrl-S Save
            {
                // do what you want here
                if (this.rgstrDetDataGridView.Focused)
                {
                    this.loadRgstrPrsnsButton.PerformClick();
                }
                else if (this.metricsDataGridView.Focused)
                {
                    this.autoCalcMtrcValsButton.PerformClick();
                    //this.saveMtrcsButton.PerformClick();
                }
                else
                {
                    this.addButton.PerformClick();
                }
                e.SuppressKeyPress = true;  // stops bing! also sets handeled which stop event bubbling
            }
            else if (e.Control && e.KeyCode == Keys.E)       // Ctrl-S Save
            {
                // do what you want here
                this.editButton.PerformClick();
                e.SuppressKeyPress = true;  // stops bing! also sets handeled which stop event bubbling
            }
            else if (e.Control && e.KeyCode == Keys.R)
            {
                this.resetButton.PerformClick();
            }
            else if ((e.Control && e.KeyCode == Keys.F) || e.KeyCode == Keys.F5)      // Ctrl-S Save
            {
                // do what you want here
                if (this.rgstrDetDataGridView.Focused)
                {
                    this.rfrshDetButton.PerformClick();
                }
                else if (this.metricsDataGridView.Focused)
                {
                    this.rfrshMtrcButton.PerformClick();
                    //this.saveMtrcsButton.PerformClick();
                }
                else
                {
                    this.rfrshButton.PerformClick();
                }
                e.SuppressKeyPress = true;  // stops bing! also sets handeled which stop event bubbling
            }
            else if (e.Control && e.KeyCode == Keys.Delete)
            {
                if (this.rgstrDetDataGridView.Focused)
                {
                    this.deleteDetButton.PerformClick();
                }
                else if (this.metricsDataGridView.Focused)
                {
                    this.delMtrcButton.PerformClick();
                    //this.saveMtrcsButton.PerformClick();
                }
                else
                {
                    if (this.delButton.Enabled == true)
                    {
                        this.delButton_Click(this.delButton, ex);
                    }
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else
            {
                e.Handled = false;
                e.SuppressKeyPress = false;  // stops bing! also sets handeled which stop event bubbling
            }
        }

        private void exprtAttndncTmp(int exprtTyp, long regstrID_in)
        {
            System.Windows.Forms.Application.DoEvents();
            Global.mnFrm.cmCde.clearPrvExclFiles();
            Global.mnFrm.cmCde.exclApp = new Microsoft.Office.Interop.Excel.Application();
            Global.mnFrm.cmCde.exclApp.WindowState = Excel.XlWindowState.xlNormal;
            Global.mnFrm.cmCde.exclApp.Visible = true;
            CommonCode.CommonCodes.SetWindowPos((IntPtr)Global.mnFrm.cmCde.exclApp.Hwnd, CommonCode.CommonCodes.HWND_TOP, 0, 0, 0, 0, CommonCode.CommonCodes.SWP_NOMOVE | CommonCode.CommonCodes.SWP_NOSIZE | CommonCode.CommonCodes.SWP_SHOWWINDOW);

            Global.mnFrm.cmCde.nwWrkBk = Global.mnFrm.cmCde.exclApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Global.mnFrm.cmCde.nwWrkBk.Worksheets.Add(Type.Missing, Type.Missing, 1, Type.Missing);
            Global.mnFrm.cmCde.trgtSheets = new Excel.Worksheet[1];

            Global.mnFrm.cmCde.trgtSheets[0] = (Excel.Worksheet)Global.mnFrm.cmCde.nwWrkBk.Worksheets[1];

            Global.mnFrm.cmCde.trgtSheets[0].get_Range("B2:C3", Type.Missing).MergeCells = true;
            Global.mnFrm.cmCde.trgtSheets[0].get_Range("B2:C3", Type.Missing).Value2 = Global.mnFrm.cmCde.getOrgName(Global.mnFrm.cmCde.Org_id).ToUpper();
            Global.mnFrm.cmCde.trgtSheets[0].get_Range("B2:C3", Type.Missing).Font.Bold = true;
            Global.mnFrm.cmCde.trgtSheets[0].get_Range("B2:C3", Type.Missing).Font.Size = 13;
            Global.mnFrm.cmCde.trgtSheets[0].get_Range("B2:C3", Type.Missing).WrapText = true;
            Global.mnFrm.cmCde.trgtSheets[0].Shapes.AddPicture(Global.mnFrm.cmCde.getOrgImgsDrctry() + @"\" + Global.mnFrm.cmCde.Org_id + ".png",
                Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, 1, 1, 50, 50);

            ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[5, 1]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 162, 192));
            ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[5, 1]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(255, 255, 255));
            ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[5, 1]).Font.Bold = true;
            ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[5, 1]).Value2 = "No.";
            //string[] hdngs = { "Person's ID No.**", "Full Name", "Present? (YES/NO)**", "Date/Time In", "Date/Time Out", "Comments" };
            string[] hdngs = { "Person's ID No.*", "Name/Description of Visitor/Attendee*", "Present? (YES/NO)**",
                             "Date/Time In", "Date/Time Out", "Comments","No. of Persons",
                             "Visitor/Attendee Classification","IS CUSTOMER? (YES/NO)","Linked Sponsor/Firm" };

            for (int a = 0; a < hdngs.Length; a++)
            {
                ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[5, (a + 2)]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 162, 192));
                ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[5, (a + 2)]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(255, 255, 255));
                ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[5, (a + 2)]).Font.Bold = true;
                ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[5, (a + 2)]).Value2 = hdngs[a].ToUpper();
            }
            if (exprtTyp == 2)
            {
                DataSet dtst = Global.get_One_AttnRgstr_DetLns("%", "Person Name/ID", 0, 10000000, regstrID_in);
                for (int a = 0; a < dtst.Tables[0].Rows.Count; a++)
                {
                    long cstmrID = long.Parse(dtst.Tables[0].Rows[a][12].ToString());
                    string isCstmr = "NO";
                    if (cstmrID > 0)
                    {
                        isCstmr = "YES";
                    }
                    string prsnt = dtst.Tables[0].Rows[a][7].ToString();
                    if (prsnt == "TRUE")
                    {
                        prsnt = "YES";
                    }
                    else
                    {
                        prsnt = "NO";
                    }
                  ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 1]).Value2 = a + 1;
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 2]).Value2 = dtst.Tables[0].Rows[a][3].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 3]).Value2 = dtst.Tables[0].Rows[a][4].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 4]).Value2 = prsnt;
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 5]).Value2 = dtst.Tables[0].Rows[a][5].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 6]).Value2 = dtst.Tables[0].Rows[a][6].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 7]).Value2 = dtst.Tables[0].Rows[a][8].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 8]).Value2 = dtst.Tables[0].Rows[a][10].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 9]).Value2 = dtst.Tables[0].Rows[a][9].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 10]).Value2 = isCstmr;
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 11]).Value2 = dtst.Tables[0].Rows[a][14].ToString();
                }
            }
            else if (exprtTyp >= 3)
            {
                DataSet dtst = Global.get_One_AttnRgstr_DetLns("%", "Person Name/ID", 0, exprtTyp, regstrID_in);
                for (int a = 0; a < dtst.Tables[0].Rows.Count; a++)
                {
                    long cstmrID = long.Parse(dtst.Tables[0].Rows[a][12].ToString());
                    string isCstmr = "NO";
                    if (cstmrID > 0)
                    {
                        isCstmr = "YES";
                    }

                    string prsnt = dtst.Tables[0].Rows[a][7].ToString();
                    if (prsnt == "TRUE")
                    {
                        prsnt = "YES";
                    }
                    else
                    {
                        prsnt = "NO";
                    }
                  ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 1]).Value2 = a + 1;
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 2]).Value2 = dtst.Tables[0].Rows[a][3].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 3]).Value2 = dtst.Tables[0].Rows[a][4].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 4]).Value2 = prsnt;
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 5]).Value2 = dtst.Tables[0].Rows[a][5].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 6]).Value2 = dtst.Tables[0].Rows[a][6].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 7]).Value2 = dtst.Tables[0].Rows[a][8].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 8]).Value2 = dtst.Tables[0].Rows[a][10].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 9]).Value2 = dtst.Tables[0].Rows[a][9].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 10]).Value2 = isCstmr;
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 11]).Value2 = dtst.Tables[0].Rows[a][14].ToString();
                }
            }
            else
            {
            }

            Global.mnFrm.cmCde.trgtSheets[0].get_Range("A1:A65535", Type.Missing).ColumnWidth = 10;
            Global.mnFrm.cmCde.trgtSheets[0].get_Range("A1:A65535", Type.Missing).WrapText = true;

            Global.mnFrm.cmCde.trgtSheets[0].get_Range("B1:Z65535", Type.Missing).Columns.AutoFit();
            Global.mnFrm.cmCde.trgtSheets[0].get_Range("B1:Z65535", Type.Missing).Rows.AutoFit();
        }

        private void exptAttndncTmpButton_Click(object sender, EventArgs e)
        {
            string rspnse = Interaction.InputBox("How many Attendance Records will you like to Export?" +
              "\r\n1=No Attendance Records(Empty Template)" +
              "\r\n2=All Attendance Records" +
              "\r\n3-Infinity=Specify the exact number of Attendance Records to Export\r\n",
              "Rhomicom", "1", (Global.mnFrm.cmCde.myComputer.Screen.Bounds.Width / 2) - 170,
              (Global.mnFrm.cmCde.myComputer.Screen.Bounds.Height / 2) - 100);
            if (rspnse.Equals(string.Empty) || rspnse.Equals(null))
            {
                //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                return;
            }
            int rsponse = 0;
            bool rsps = int.TryParse(rspnse, out rsponse);
            if (rsps == false)
            {
                Global.mnFrm.cmCde.showMsg("Invalid Option! Expecting a Number Above Zero", 4);
                return;
            }
            if (rsponse < 1)
            {
                Global.mnFrm.cmCde.showMsg("Invalid Option! Expecting a Number Above Zero", 4);
                return;
            }
            this.exprtAttndncTmp(rsponse, long.Parse(this.rgstrIDTextBox.Text));
        }

        private void imprtAttndncTmp(string filename, long regstrID_in)
        {
            System.Windows.Forms.Application.DoEvents();
            Global.mnFrm.cmCde.clearPrvExclFiles();
            Global.mnFrm.cmCde.exclApp = new Microsoft.Office.Interop.Excel.Application();
            Global.mnFrm.cmCde.exclApp.WindowState = Excel.XlWindowState.xlNormal;
            Global.mnFrm.cmCde.exclApp.Visible = true;
            CommonCode.CommonCodes.SetWindowPos((IntPtr)Global.mnFrm.cmCde.exclApp.Hwnd, CommonCode.CommonCodes.HWND_TOP, 0, 0, 0, 0, CommonCode.CommonCodes.SWP_NOMOVE | CommonCode.CommonCodes.SWP_NOSIZE | CommonCode.CommonCodes.SWP_SHOWWINDOW);

            Global.mnFrm.cmCde.nwWrkBk = Global.mnFrm.cmCde.exclApp.Workbooks.Open(filename, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);

            Global.mnFrm.cmCde.trgtSheets = new Excel.Worksheet[1];

            Global.mnFrm.cmCde.trgtSheets[0] = (Excel.Worksheet)Global.mnFrm.cmCde.nwWrkBk.Worksheets[1];
            string prsnsIDNo = "";
            string fullNm = "";
            string isPrsnt = "";
            string dtetmenum1 = "";
            string dtetmenum2 = "";
            string cmmnts = "";
            //string name_desc = "";
            string noAdlts = "1";
            int noPrsns = -1;
            long cstmrID = -1;
            string vstrClsf = "Existing Person";
            string isCstmr = "";
            string spnsorNm = "";
            int rownum = 5;
            do
            {
                try
                {
                    prsnsIDNo = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 2]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    prsnsIDNo = "";
                }
                try
                {
                    fullNm = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 3]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    fullNm = "";
                }
                try
                {
                    isPrsnt = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 4]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    isPrsnt = "";
                }
                try
                {
                    dtetmenum1 = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 5]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    dtetmenum1 = "";
                }
                try
                {
                    dtetmenum2 = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 6]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    dtetmenum2 = "";
                }
                try
                {
                    cmmnts = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 7]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    cmmnts = "";
                }
                try
                {
                    noAdlts = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 8]).Value2.ToString();
                    if (noAdlts == "")
                    {
                        noAdlts = "1";
                    }
                }
                catch (Exception ex)
                {
                    noAdlts = "1";
                }
                try
                {
                    vstrClsf = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 9]).Value2.ToString();
                    if (vstrClsf == "")
                    {
                        vstrClsf = "Existing Person";
                    }
                }
                catch (Exception ex)
                {
                    vstrClsf = "Existing Person";
                }
                try
                {
                    isCstmr = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 10]).Value2.ToString();
                    if (isCstmr == "")
                    {
                        isCstmr = "NO";
                    }
                }
                catch (Exception ex)
                {
                    isCstmr = "NO";
                }
                try
                {
                    spnsorNm = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 11]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    spnsorNm = "";
                }

                if (rownum == 5)
                {
                    string[] hdngs = { "Person's ID No.*", "Name/Description of Visitor/Attendee*", "Present? (YES/NO)**",
                             "Date/Time In", "Date/Time Out", "Comments","No. of Persons",
                             "Visitor/Attendee Classification","IS CUSTOMER? (YES/NO)","Linked Sponsor/Firm" };

                    if (prsnsIDNo != hdngs[0].ToUpper()
                      || fullNm != hdngs[1].ToUpper()
                      || isPrsnt != hdngs[2].ToUpper()
                      || dtetmenum1 != hdngs[3].ToUpper()
                      || dtetmenum2 != hdngs[4].ToUpper()
                      || cmmnts != hdngs[5].ToUpper()
                      || noAdlts != hdngs[6].ToUpper()
                      || vstrClsf != hdngs[7].ToUpper()
                      || isCstmr != hdngs[8].ToUpper()
                      || spnsorNm != hdngs[9].ToUpper())
                    {
                        Global.mnFrm.cmCde.showMsg("The Excel File you Selected is not a Valid Template\r\nfor importing records here.", 0);
                        return;
                    }
                    rownum++;
                    continue;
                }
                if ((prsnsIDNo != "" || fullNm != "") && isPrsnt != "")
                {
                    if (int.TryParse(noAdlts, out noPrsns) == false)
                    {
                        noPrsns = 1;
                    }
                    double numFrm = 0;
                    bool isdbl = false;
                    isdbl = double.TryParse(dtetmenum1, out numFrm);
                    string DteFrm;
                    if (isdbl)
                    {
                        DteFrm = DateTime.FromOADate(numFrm).ToString("dd-MMM-yyyy HH:mm:ss");
                    }
                    else
                    {
                        DteFrm = "";
                    }

                    numFrm = 0;
                    isdbl = false;
                    isdbl = double.TryParse(dtetmenum2, out numFrm);
                    string DteTo;
                    if (isdbl)
                    {
                        DteTo = DateTime.FromOADate(numFrm).ToString("dd-MMM-yyyy HH:mm:ss");
                    }
                    else
                    {
                        DteTo = "";
                    }

                    long prsn_id_in = Global.mnFrm.cmCde.getPrsnID(prsnsIDNo);
                    if (isCstmr == "YES")
                    {
                        cstmrID = Global.get_One_CstmrID(fullNm);
                    }
                    long sponsor_id = Global.get_One_CstmrID(spnsorNm);
                    long attnRecID = Global.doesRgstrHvPrsn(prsn_id_in,
            regstrID_in);
                    if (attnRecID <= 0 && prsn_id_in > 0)
                    {
                        Global.createAttnRgstrDetLn(regstrID_in, prsn_id_in
                          , DteFrm, DteTo, Global.mnFrm.cmCde.cnvrtYNToBool(isPrsnt), cmmnts,
                          fullNm, noPrsns, cstmrID, vstrClsf, sponsor_id);
                        Global.mnFrm.cmCde.trgtSheets[0].get_Range("A" + rownum + ":G" + rownum + "", Type.Missing).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 255, 0));
                    }
                    else if (prsn_id_in > 0)
                    {
                        Global.updtAttnRgstrDetLn(attnRecID, regstrID_in, prsn_id_in
                 , DteFrm, DteTo, Global.mnFrm.cmCde.cnvrtYNToBool(isPrsnt), cmmnts,
                          fullNm, noPrsns, cstmrID, vstrClsf, sponsor_id);
                        Global.mnFrm.cmCde.trgtSheets[0].get_Range("A" + rownum + ":G" + rownum + "", Type.Missing).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                    }
                    else
                    {
                        Global.mnFrm.cmCde.trgtSheets[0].get_Range("A" + rownum + ":G" + rownum + "", Type.Missing).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(255, 0, 0));
                        //this.trgtSheets[0].get_Range("M" + rownum + ":M" + rownum + "", Type.Missing).Value2 = errMsg;
                    }
                }
                rownum++;
            }
            while (prsnsIDNo != "");
        }

        private void imptAttndncTmpButton_Click(object sender, EventArgs e)
        {
            if (Global.mnFrm.cmCde.showMsg("Are you sure you want to Import Attendance Records\r\n to Overwrite the existing Field Labels shown here?", 1) == DialogResult.No)
            {
                //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                return;
            }

            this.openFileDialog1.RestoreDirectory = true;
            this.openFileDialog1.Filter = "All Files|*.*|Excel Files|*.xls;*.xlsx";
            this.openFileDialog1.FilterIndex = 2;
            this.openFileDialog1.Title = "Select an Excel File to Upload...";
            this.openFileDialog1.FileName = "";
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.imprtAttndncTmp(this.openFileDialog1.FileName, long.Parse(this.rgstrIDTextBox.Text));
            }
            this.loadRgstrDetLnsPanel();
        }

        private void attnRgstrListView_KeyDown(object sender, KeyEventArgs e)
        {
            Global.mnFrm.cmCde.listViewKeyDown(this.attnRgstrListView, e);
        }

        private void searchForTextBox_Click(object sender, EventArgs e)
        {
            this.searchForTextBox.SelectAll();
        }

        private void searchForDetTextBox_Click(object sender, EventArgs e)
        {
            this.searchForDetTextBox.SelectAll();
        }

        private void tmTblNmTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!this.obey_evnts)
            {
                return;
            }
            this.txtChngd = true;
        }

        private void tmTblNmTextBox_Leave(object sender, EventArgs e)
        {
            if (this.txtChngd == false)
            {
                return;
            }
            this.txtChngd = false;
            TextBox mytxt = (TextBox)sender;
            this.obey_evnts = false;
            this.srchWrd = mytxt.Text;
            if (!mytxt.Text.Contains("%"))
            {
                this.srchWrd = "%" + this.srchWrd.Replace(" ", "%") + "%";
            }

            if (mytxt.Name == "tmTblNmTextBox")
            {
                this.tmTblNmTextBox.Text = "";
                this.tmTblIDTextBox.Text = "-1";
                this.tmTblButton_Click(this.tmTblButton, e);
            }
            else if (mytxt.Name == "evntDescTextBox")
            {
                this.evntDescTextBox.Text = "";
                this.tmTblDetIDTextBox.Text = "-1";
                this.evntDescButton_Click(this.evntDescButton, e);
            }
            else if (mytxt.Name == "evntDateTextBox")
            {
                this.evntStrtDateTextBox.Text = Global.mnFrm.cmCde.checkNFormatDate(this.evntStrtDateTextBox.Text);
            }
            else if (mytxt.Name == "evntEndDateTextBox")
            {
                this.evntEndDateTextBox.Text = Global.mnFrm.cmCde.checkNFormatDate(this.evntEndDateTextBox.Text);
            }
            this.srchWrd = "%";
            this.obey_evnts = true;
            this.txtChngd = false;
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.minimizeMemory();
            this.searchInComboBox.SelectedIndex = 0;
            this.searchForTextBox.Text = "%";

            this.searchInDetComboBox.SelectedIndex = 4;
            this.searchForDetTextBox.Text = "%";

            this.dsplySizeComboBox.Text = Global.mnFrm.cmCde.get_CurPlcy_Mx_Dsply_Recs().ToString();
            this.dsplySizeDetComboBox.Text = "10";
            this.disableDetEdit();
            this.disableLnsEdit();
            this.disableMetrcLnsEdit();
            this.rec_cur_indx = 0;
            this.rfrshButton_Click(this.rfrshButton, e);
        }

        private void delMtrcButton_Click(object sender, EventArgs e)
        {

            if (this.addRec == false && this.editRec == false)
            {
                this.editButton.PerformClick();
            }
            if (this.addRec == false && this.editRec == false)
            {
                Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }

            if (this.metricsDataGridView.CurrentCell != null)
            {
                this.metricsDataGridView.Rows[this.metricsDataGridView.CurrentCell.RowIndex].Selected = true;
            }
            if (this.metricsDataGridView.SelectedRows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select the lines to be Deleted First!", 0);
                return;
            }
            int slctdrows = this.metricsDataGridView.SelectedRows.Count;
            for (int i = 0; i < slctdrows; i++)
            {
                long rwID = long.Parse(this.metricsDataGridView.SelectedRows[0].Cells[5].Value.ToString());
                long mtrcID = long.Parse(this.metricsDataGridView.SelectedRows[0].Cells[7].Value.ToString());
                long pssblValID = long.Parse(this.metricsDataGridView.SelectedRows[0].Cells[1].Value.ToString());
                if (rwID > 0)
                {
                    if (mtrcID > 0)
                    {
                        Global.deleteActvtyRslt(rwID);
                    }
                    else if (pssblValID > 0)
                    {
                        Global.deleteAtndncMtrc(rwID);
                    }
                }
                this.metricsDataGridView.Rows.RemoveAt(this.metricsDataGridView.SelectedRows[0].Index);
            }
        }

        private void rfrshMtrcButton_Click(object sender, EventArgs e)
        {
            this.populateMetricGridVw();
        }

        private void rcHstryMtrcButton_Click(object sender, EventArgs e)
        {
            if (this.metricsDataGridView.CurrentCell != null)
            {
                this.metricsDataGridView.Rows[this.metricsDataGridView.CurrentCell.RowIndex].Selected = true;
            }

            if (this.metricsDataGridView.SelectedRows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select a Record First!", 0);
                return;
            }
            long mtrcID = long.Parse(this.metricsDataGridView.SelectedRows[0].Cells[7].Value.ToString());
            long pssblValID = long.Parse(this.metricsDataGridView.SelectedRows[0].Cells[1].Value.ToString());
            string tblNm = "attn.attn_attendance_recs_cntr";
            string pkColNm = "cntr_id";
            if (mtrcID > 0)
            {
                tblNm = "attn.attn_attendance_events_rslts";
                pkColNm = "evnt_rslt_id";
            }
            Global.mnFrm.cmCde.showRecHstry(
              Global.mnFrm.cmCde.get_Gnrl_Rec_Hstry(long.Parse(
              this.metricsDataGridView.SelectedRows[0].Cells[5].Value.ToString()),
              tblNm, pkColNm), 7);
        }

        private void vwSQLMtrcButton_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.showSQL(this.rec_mtrc_SQL, 6);
        }

        private void autoCalcMtrcValsButton_Click(object sender, EventArgs e)
        {

            if (this.addRec == false && this.editRec == false)
            {
                this.editButton.PerformClick();
            }
            if (this.addRec == false && this.editRec == false)
            {
                Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            if (this.tmTblDetIDTextBox.Text == "-1"
        || this.tmTblDetIDTextBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Please indicate the Event First!", 0);
                return;
            }
            long rgstrID = long.Parse(this.rgstrIDTextBox.Text);

            for (int i = 0; i < this.metricsDataGridView.Rows.Count; i++)
            {
                string mtrcNm = this.metricsDataGridView.Rows[i].Cells[0].Value.ToString();
                int pssblValID = int.Parse(this.metricsDataGridView.Rows[i].Cells[1].Value.ToString());
                int rsltMtrcID = int.Parse(this.metricsDataGridView.Rows[i].Cells[7].Value.ToString());

                if ((mtrcNm == "Male Attendance"
                  || mtrcNm == "Female Attendance"
                  || mtrcNm == "Total Attendance") && pssblValID > 0)
                {
                    this.metricsDataGridView.Rows[i].Cells[2].Value = Global.getAtndncMtrcCnt(mtrcNm, rgstrID);
                    this.metricsDataGridView.Rows[i].Cells[3].Value = "Auto Count of " + mtrcNm + " in this Register";
                }
                else if (rsltMtrcID > 0)
                {
                    string evntID = Global.mnFrm.cmCde.getGnrlRecNm(
                      "attn.attn_time_table_details", "time_table_det_id",
                      "event_id", long.Parse(this.tmTblDetIDTextBox.Text));

                    string dte1 = this.evntStrtDateTextBox.Text;
                    string dte2 = this.evntEndDateTextBox.Text;

                    string mtrcSQL = Global.getMtrcSQL(rsltMtrcID);
                    this.metricsDataGridView.Rows[i].Cells[2].Value = Global.computMtrcSQL(mtrcSQL, int.Parse(evntID), dte1, dte2);
                }
            }
        }

        private void saveMtrcsButton_Click(object sender, EventArgs e)
        {
            int svd = 0;
            this.metricsDataGridView.EndEdit();
            System.Windows.Forms.Application.DoEvents();
            string evntID = Global.mnFrm.cmCde.getGnrlRecNm(
      "attn.attn_time_table_details", "time_table_det_id",
      "event_id", long.Parse(this.tmTblDetIDTextBox.Text));

            string dte1 = this.evntStrtDateTextBox.Text;
            string dte2 = this.evntEndDateTextBox.Text;

            dte1 = DateTime.ParseExact(
              dte1, "dd-MMM-yyyy HH:mm:ss",
              System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            dte2 = DateTime.ParseExact(
          dte2, "dd-MMM-yyyy HH:mm:ss",
          System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            bool autoCalc = false;
            for (int i = 0; i < this.metricsDataGridView.Rows.Count; i++)
            {
                long cntrID = long.Parse(this.metricsDataGridView.Rows[i].Cells[5].Value.ToString());
                long rgstrID = long.Parse(this.rgstrIDTextBox.Text);
                int rsltMtrcID = int.Parse(this.metricsDataGridView.Rows[i].Cells[7].Value.ToString());
                int pssblValID = int.Parse(this.metricsDataGridView.Rows[i].Cells[1].Value.ToString());
                string cmmntDesc = this.metricsDataGridView.Rows[i].Cells[3].Value.ToString();
                string mtrcNm = this.metricsDataGridView.Rows[i].Cells[0].Value.ToString();

                long rsltVal = 0;
                string rsltVal1 = this.metricsDataGridView.Rows[i].Cells[2].Value.ToString();
                if (rsltMtrcID <= 0)
                {
                    bool sccs = long.TryParse(this.metricsDataGridView.Rows[i].Cells[2].Value.ToString(), out rsltVal);
                    if (pssblValID > 0 && mtrcNm != "" && sccs)
                    {
                        if (cntrID <= 0)
                        {
                            cntrID = Global.getNewMtrcCntLnID();
                            Global.createAttnMtrcCnt(cntrID, rgstrID, mtrcNm, cmmntDesc, rsltVal, pssblValID);
                            this.metricsDataGridView.Rows[i].Cells[5].Value = cntrID;
                        }
                        else
                        {
                            Global.updateAttnMtrcCnt(cntrID, mtrcNm, cmmntDesc, rsltVal, pssblValID);
                        }
                        this.metricsDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                        svd++;
                    }
                    else
                    {
                        this.metricsDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 100, 100);
                    }
                }
                else
                {
                    if (dte1 != "" && dte2 != "" && rsltVal1 != "")
                    {
                        if (cntrID <= 0)
                        {
                            cntrID = Global.getNewRsltLnID();
                            Global.createActvtyRslt(cntrID, int.Parse(evntID), rsltMtrcID, cmmntDesc, rsltVal1, dte1, dte2, autoCalc, rgstrID);
                            this.metricsDataGridView.Rows[i].Cells[5].Value = cntrID;
                        }
                        else
                        {
                            Global.updateActvtyRslt(cntrID, int.Parse(evntID), rsltMtrcID, cmmntDesc, rsltVal1, dte1, dte2, autoCalc, rgstrID);
                        }

                        this.metricsDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                        svd++;
                    }
                    else
                    {
                        this.metricsDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 100, 100);
                    }
                }
            }
            Global.mnFrm.cmCde.showMsg(svd + " Record(s) Saved!", 3);
            //this.populateMetricGridVw();

        }

        private void evntEndDateButton_Click(object sender, EventArgs e)
        {

            if (this.addRec == false && this.editRec == false)
            {
                this.editButton.PerformClick();
            }
            if (this.addRec == false && this.editRec == false)
            {
                Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }
            Global.mnFrm.cmCde.selectDate(ref this.evntEndDateTextBox);
        }

        private void tmTblNmTextBox_Click(object sender, EventArgs e)
        {
            TextBox mytxt = (TextBox)sender;
            //mytxt.SelectAll();

            if (mytxt.Name == "tmTblNmTextBox")
            {
                this.tmTblNmTextBox.SelectAll();
            }
            else if (mytxt.Name == "evntStrtDateTextBox")
            {
                this.evntStrtDateTextBox.SelectAll();
            }
            else if (mytxt.Name == "evntEndDateTextBox")
            {
                this.evntEndDateTextBox.SelectAll();
            }
            else if (mytxt.Name == "evntDescTextBox")
            {
                this.evntDescTextBox.SelectAll();
            }

        }

        private void vwAttchmntsButton_Click(object sender, EventArgs e)
        {
            if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[9]) == false)
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }

            if (this.rgstrIDTextBox.Text == "" ||
          this.rgstrIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("Please select a saved Register First!", 0);
                return;
            }

            attchmntsDiag nwDiag = new attchmntsDiag();
            if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[9]) == false)
            {
                nwDiag.addButton.Enabled = false;
                nwDiag.addButton.Visible = false;
                nwDiag.editButton.Enabled = false;
                nwDiag.editButton.Visible = false;
                nwDiag.delButton.Enabled = false;
                nwDiag.delButton.Visible = false;
            }
            //Global.mnFrm.cmCde.showMsg("Cannot add Transactions to already Posted Batch of Transactions!", 0);
            //return;
            nwDiag.batchid = -1;
            nwDiag.batchHdrID = long.Parse(this.rgstrIDTextBox.Text);
            DialogResult dgres = nwDiag.ShowDialog();
            if (dgres == DialogResult.OK)
            {
            }
        }

        private void addVisitorButton_Click(object sender, EventArgs e)
        {
            if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[9]) == false)
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }
            if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[24]) == false)
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }
            if (this.rgstrIDTextBox.Text == "" ||
      this.rgstrIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("Please select a saved Register First!", 0);
                return;
            }
            if (this.editButton.Text == "EDIT")
            {
                this.editButton_Click(this.editButton, e);
            }
            if (long.Parse(this.tmTblDetIDTextBox.Text) > 0)
            {
                Global.createAttnRgstrDetLn(long.Parse(this.rgstrIDTextBox.Text), -1
               , "", "", false, "", "", 1, -1, "Customer", -1);
            }
            else
            {
                Global.createAttnRgstrDetLn(long.Parse(this.rgstrIDTextBox.Text), -1
            , "", "", false, "", "", 1, -1, "Visitor", -1);
            }
            //this.prpareForLnsEdit();
            this.searchForDetTextBox.Text = "%";
            this.rfrshDetButton.PerformClick();
        }

        private void refreshCostButton_Click(object sender, EventArgs e)
        {
            this.loadRgstrCostLnsPanel();
        }

        private void vwSQLCostButton_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.showSQL(this.rec_Cost_SQL, 6);
        }

        private void rcHstryCostButton_Click(object sender, EventArgs e)
        {
            if (this.costingDataGridView.CurrentCell != null && this.costingDataGridView.SelectedRows.Count <= 0)
            {
                this.costingDataGridView.Rows[this.costingDataGridView.CurrentCell.RowIndex].Selected = true;
            }

            if (this.costingDataGridView.SelectedRows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select a Record First!", 0);
                return;
            }
            Global.mnFrm.cmCde.showRecHstry(
              Global.mnFrm.cmCde.get_Gnrl_Rec_Hstry(long.Parse(
              this.costingDataGridView.SelectedRows[0].Cells[12].Value.ToString()),
              "attn.attn_attendance_costs", "attnd_cost_id"), 7);
        }

        private void positionCostTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            EventArgs ex = new EventArgs();
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up)
            {
                this.CostPnlNavButtons(this.movePreviousCostButton, ex);
            }
            else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Down)
            {
                this.CostPnlNavButtons(this.moveNextCostButton, ex);
            }
        }

        private void searchForCostTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            EventArgs ex = new EventArgs();
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                this.refreshCostButton_Click(this.refreshCostButton, ex);
            }
        }

        private void searchForCostTextBox_Click(object sender, EventArgs e)
        {
            this.searchForCostTextBox.SelectAll();
        }

        private void delCostButton_Click(object sender, EventArgs e)
        {
            if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[30]) == false)
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }

            //if (this.addRec == false && this.editRec == false)
            //{
            //  Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
            //  return;
            //}
            if (this.costingDataGridView.CurrentCell != null && this.costingDataGridView.SelectedRows.Count <= 0)
            {
                this.costingDataGridView.Rows[this.costingDataGridView.CurrentCell.RowIndex].Selected = true;
            }

            if (this.costingDataGridView.SelectedRows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select the Record(s) to Delete!", 0);
                return;
            }

            if (Global.mnFrm.cmCde.showMsg("Are you sure you want to DELETE the selected Item?" +
      "\r\nThis action cannot be undone!", 1) == DialogResult.No)
            {
                // //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                return;
            }
            for (int i = 0; i < this.costingDataGridView.SelectedRows.Count; i++)
            {
                long lnID = -1;
                long.TryParse(this.costingDataGridView.SelectedRows[i].Cells[12].Value.ToString(), out lnID);
                if (this.costingDataGridView.SelectedRows[i].Cells[2].Value == null)
                {
                    this.costingDataGridView.SelectedRows[i].Cells[2].Value = string.Empty;
                }
                if (Global.isAttnCostLnInUse(lnID) == false)
                {
                    Global.deleteAttnCostLn(lnID, this.costingDataGridView.SelectedRows[i].Cells[2].Value.ToString());
                }
            }
            this.refreshCostButton.PerformClick();
        }

        private void newCostButton_Click(object sender, EventArgs e)
        {
            if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[9]) == false)
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }

            if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[28]) == false)
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }
            if (long.Parse(this.rgstrIDTextBox.Text) <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select a Register First!", 0);
                return;
            }

            if (this.rgstrIDTextBox.Text == "" ||
      this.rgstrIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("Please select a saved Register First!", 0);
                return;
            }

            if (this.editButton.Text == "EDIT")
            {
                this.editButton_Click(this.editButton, e);
            }

            Global.createAttnCostLn(long.Parse(this.rgstrIDTextBox.Text), -1, ""
              , "", 1, 1, 0, "Uncategorized");
            //this.prpareForLnsEdit();cost_comments
            this.refreshCostButton.PerformClick();
        }

        private void costingDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e == null || this.obey_evnts == false)
            {
                return;
            }
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            bool prv = this.obey_evnts;
            this.obey_evnts = false;


            this.dfltFill1(e.RowIndex);
            if (e.ColumnIndex == 13
              || e.ColumnIndex == 14
              || e.ColumnIndex == 1)
            {

                if (this.addRec == false && this.editRec == false)
                {
                    this.editButton.PerformClick();
                }
                if (this.addRec == false && this.editRec == false)
                {
                    Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                    this.obey_evnts = true;
                    return;
                }
            }
            if (e.ColumnIndex == 1)
            {
                int[] selVals = new int[1];
                selVals[0] = Global.mnFrm.cmCde.getPssblValID(
                  this.costingDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString(),
                  Global.mnFrm.cmCde.getLovID("Event Cost Categories"));

                DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                    Global.mnFrm.cmCde.getLovID("Event Cost Categories"), ref selVals,
                    true, false,
                 this.srchWrd, "Both", true);

                if (dgRes == DialogResult.OK)
                {
                    for (int i = 0; i < selVals.Length; i++)
                    {
                        this.costingDataGridView.Rows[e.RowIndex].Cells[0].Value = Global.mnFrm.cmCde.getPssblValNm(
                          selVals[i]);
                    }
                    this.obey_evnts = true;
                    DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(0, e.RowIndex);
                    this.costingDataGridView_CellValueChanged(this.costingDataGridView, ex);
                }
            }
            else if (e.ColumnIndex == 13)
            {
                if (this.costingDataGridView.CurrentCell != null && this.costingDataGridView.SelectedRows.Count <= 0)
                {
                    this.costingDataGridView.Rows[this.costingDataGridView.CurrentCell.RowIndex].Selected = true;
                }

                long srcDocID = long.Parse(this.costingDataGridView.Rows[e.RowIndex].Cells[11].Value.ToString());
                if (srcDocID > 0)
                {
                    Global.mnFrm.cmCde.showMsg("Accounting for such lines are usually done in the Source Document", 0);
                }
                else
                {
                    long glbatchID = -1;
                    long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
           "attn.attn_attendance_costs", "attnd_cost_id", "gl_batch_id",
           long.Parse(this.costingDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString())), out glbatchID);
                    if (glbatchID > 0)
                    {
                        Global.mnFrm.cmCde.showMsg("Accounting already Created!", 0);
                        int rwIdx = e.RowIndex;
                        addTrnsTmpltDiag nwDiag = new addTrnsTmpltDiag();
                        nwDiag.incrsDcrs1ComboBox.SelectedItem = this.costingDataGridView.Rows[rwIdx].Cells[16].Value.ToString();
                        nwDiag.accntID1TextBox.Text = this.costingDataGridView.Rows[rwIdx].Cells[17].Value.ToString();
                        nwDiag.accntName1TextBox.Text = Global.mnFrm.cmCde.getAccntName(int.Parse(nwDiag.accntID1TextBox.Text));
                        nwDiag.accntNum1TextBox.Text = Global.mnFrm.cmCde.getAccntNum(int.Parse(nwDiag.accntID1TextBox.Text));

                        nwDiag.incrsDcrs2ComboBox.SelectedItem = this.costingDataGridView.Rows[rwIdx].Cells[18].Value.ToString();
                        nwDiag.accntID2TextBox.Text = this.costingDataGridView.Rows[rwIdx].Cells[19].Value.ToString();
                        nwDiag.accntName2TextBox.Text = Global.mnFrm.cmCde.getAccntName(int.Parse(nwDiag.accntID2TextBox.Text));
                        nwDiag.accntNum2TextBox.Text = Global.mnFrm.cmCde.getAccntNum(int.Parse(nwDiag.accntID2TextBox.Text));

                        nwDiag.accntNum1Button.Enabled = false;
                        nwDiag.accntNum2Button.Enabled = false;
                        nwDiag.accntNum1Button.Visible = false;
                        nwDiag.accntNum2Button.Visible = false;

                        nwDiag.OKButton.Enabled = false;
                        nwDiag.OKButton.Visible = false;

                        nwDiag.OKButton.Enabled = false;
                        nwDiag.OKButton.Visible = false;
                        nwDiag.accntName2TextBox.ReadOnly = true;
                        nwDiag.accntName1TextBox.ReadOnly = true;
                        DialogResult dgres = nwDiag.ShowDialog();
                        this.obey_evnts = true;
                        return;
                    }

                    if (Global.mnFrm.cmCde.showMsg("Are you sure you want to Create Accounting for the selected Item?" +
                 "\r\nThis action cannot be undone!", 1) == DialogResult.No)
                    {
                        ////Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                        this.obey_evnts = true;
                        return;
                    }
                    if (this.createEventAccntng(
                      long.Parse(this.costingDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString()),
                      e.RowIndex))
                    {
                        Global.mnFrm.cmCde.showMsg("Create Accounting Successful!", 3);
                        if (this.saveButton.Enabled == true)
                        {
                            this.populateDet(long.Parse(this.rgstrIDTextBox.Text));
                        }
                        this.populateCostGridVw();
                    }
                }
            }
            else if (e.ColumnIndex == 14)
            {
                if (this.costingDataGridView.CurrentCell != null && this.costingDataGridView.SelectedRows.Count <= 0)
                {
                    this.costingDataGridView.Rows[this.costingDataGridView.CurrentCell.RowIndex].Selected = true;
                }

                long srcDocID = long.Parse(this.costingDataGridView.Rows[e.RowIndex].Cells[11].Value.ToString());
                if (srcDocID > 0)
                {
                    Global.mnFrm.cmCde.showMsg("Accounting for such lines are usually done in the Source Document", 0);
                }
                else
                {
                    long glbatchID = -1;
                    long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
           "attn.attn_attendance_costs", "attnd_cost_id", "gl_batch_id",
           long.Parse(this.costingDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString())), out glbatchID);
                    if (glbatchID <= 0)
                    {
                        Global.mnFrm.cmCde.showMsg("No Accounting to Reverse!", 0);
                        this.obey_evnts = true;
                        return;
                    }

                    if (Global.mnFrm.cmCde.showMsg("Are you sure you want to Reverse Accounting for the selected Record?" +
                 "\r\nThis action cannot be undone!", 1) == DialogResult.No)
                    {
                        ////Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                        this.obey_evnts = true;
                        return;
                    }
                    if (this.voidAttachedBatch(long.Parse(this.costingDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString())))
                    {
                        Global.mnFrm.cmCde.showMsg("Reverse Accounting Completed Successfully!", 3);
                        if (this.saveButton.Enabled == true)
                        {
                            this.populateDet(long.Parse(this.rgstrIDTextBox.Text));
                        }
                        this.populateCostGridVw();
                    }
                }
            }
            this.obey_evnts = true;

        }

        public bool createEventAccntng(long eventCostID, int rwIdx)
        {
            /* 1. Create a GL Batch and get all doc lines
             * 2. for each line create costing account transaction
             * 3. create one balancing account transaction using the grand total amount
             * 4. Check if created gl_batch is balanced.
             * 5. if balanced update docHdr else delete the gl batch created and throw error message
             */
            try
            {
                long glbatchID = long.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
        "attn.attn_attendance_costs", "attnd_cost_id", "gl_batch_id", eventCostID));
                if (glbatchID > 0)
                {
                    Global.mnFrm.cmCde.showMsg("Accounting Created Already!", 0);
                    return false;
                }
                string costCtgry = this.costingDataGridView.Rows[rwIdx].Cells[0].Value.ToString();
                string evntIDStr = Global.mnFrm.cmCde.getGnrlRecNm(
                   "attn.attn_time_table_details", "time_table_det_id",
                   "event_id", long.Parse(this.tmTblDetIDTextBox.Text));
                addTrnsTmpltDiag nwDiag = new addTrnsTmpltDiag();
                int evntID = -1;
                int.TryParse(evntIDStr, out evntID);
                string[] evntCstAccnts = Global.get_CostAcntInfo(costCtgry, evntID);
                nwDiag.incrsDcrs1ComboBox.SelectedItem = evntCstAccnts[0];
                nwDiag.accntID1TextBox.Text = evntCstAccnts[1];
                nwDiag.accntName1TextBox.Text = Global.mnFrm.cmCde.getAccntName(int.Parse(nwDiag.accntID1TextBox.Text));
                nwDiag.accntNum1TextBox.Text = Global.mnFrm.cmCde.getAccntNum(int.Parse(nwDiag.accntID1TextBox.Text));

                nwDiag.incrsDcrs2ComboBox.SelectedItem = evntCstAccnts[3];
                nwDiag.accntID2TextBox.Text = evntCstAccnts[4];
                nwDiag.accntName2TextBox.Text = Global.mnFrm.cmCde.getAccntName(int.Parse(nwDiag.accntID2TextBox.Text));
                nwDiag.accntNum2TextBox.Text = Global.mnFrm.cmCde.getAccntNum(int.Parse(nwDiag.accntID2TextBox.Text));

                DialogResult dgres = nwDiag.ShowDialog();
                if (dgres == DialogResult.Cancel)
                {
                    return false;
                }
                string glBatchName = "ACC_EVENT-" +
                 DateTime.Parse(Global.mnFrm.cmCde.getFrmtdDB_Date_time()).ToString("yyMMdd-HHmmss")
                          + "-" + Global.mnFrm.cmCde.getRandomInt(10, 100);

                /*Global.mnFrm.cmCde.getDB_Date_time().Substring(11, 8).Replace(":", "").Replace("-", "").Replace(" ", "") + "-" +
          Global.getNewBatchID().ToString().PadLeft(4, '0');*/
                long glBatchID = Global.mnFrm.cmCde.getGnrlRecID("accb.accb_trnsctn_batches",
                  "batch_name", "batch_id", glBatchName, Global.mnFrm.cmCde.Org_id);

                if (glBatchID <= 0)
                {
                    Global.createBatch(Global.mnFrm.cmCde.Org_id, glBatchName,
                      this.rgstrDescTextBox.Text + " (" + this.rgstrNmTextBox.Text + ")",
                      "Event Costing", "VALID", -1, "0");
                }
                else
                {
                    Global.mnFrm.cmCde.showMsg("GL Batch Could not be Created!\r\n Try Again Later!", 0);
                    return false;
                }

                glBatchID = Global.mnFrm.cmCde.getGnrlRecID("accb.accb_trnsctn_batches",
                  "batch_name", "batch_id", glBatchName, Global.mnFrm.cmCde.Org_id);
                int blncngAccntID = -1;
                string lnDte = this.evntStrtDateTextBox.Text;
                this.dfltFill1(rwIdx);
                string lineTypeNm = this.costingDataGridView.Rows[rwIdx].Cells[0].Value.ToString();

                string incrDcrs1 = nwDiag.incrsDcrs1ComboBox.Text.Substring(0, 1);
                int accntID1 = -1;
                int.TryParse(nwDiag.accntID1TextBox.Text, out accntID1);
                string isdbtCrdt1 = Global.mnFrm.cmCde.dbtOrCrdtAccnt(accntID1, incrDcrs1.Substring(0, 1));

                string incrDcrs2 = nwDiag.incrsDcrs2ComboBox.Text.Substring(0, 1);
                int accntID2 = -1;
                int.TryParse(nwDiag.accntID2TextBox.Text, out accntID2);
                blncngAccntID = accntID2;
                string isdbtCrdt2 = Global.mnFrm.cmCde.dbtOrCrdtAccnt(accntID2, incrDcrs2.Substring(0, 1));

                double lnAmnt = double.Parse(this.costingDataGridView.Rows[rwIdx].Cells[8].Value.ToString());

                System.Windows.Forms.Application.DoEvents();

                double acntAmnt = lnAmnt;
                double entrdAmnt = lnAmnt;

                string lneDesc = this.rgstrDescTextBox.Text;
                int entrdCurrID = this.curid;
                int funcCurrID = this.curid;
                int accntCurrID = entrdCurrID;
                double funcCurrRate = 1;
                double funcCurrAmnt = lnAmnt;
                double accntCurrRate = 1;

                if (accntID1 > 0 && (lnAmnt != 0 || funcCurrAmnt != 0) && incrDcrs1 != "" && lneDesc != "")
                {
                    double netAmnt = (double)Global.dbtOrCrdtAccntMultiplier(accntID1,
              incrDcrs1) * (double)funcCurrAmnt;

                    if (!Global.mnFrm.cmCde.isTransPrmttd(accntID1, lnDte, netAmnt))
                    {
                        return false;
                    }
                    if (Global.getTrnsID(lneDesc, accntID1, entrdAmnt, entrdCurrID,
                      DateTime.ParseExact(
             lnDte, "dd-MMM-yyyy HH:mm:ss",
             System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss")) > 0)
                    {
                        Global.mnFrm.cmCde.showMsg("Same Transaction has been created Already!\r\nConsider changing the Date or Time and Try Again!", 0);
                        Global.deleteBatchTrns(glBatchID);
                        Global.deleteBatch(glBatchID, glBatchName);
                        return false;
                    }

                    if (Global.dbtOrCrdtAccnt(accntID1,
                      incrDcrs1) == "Debit")
                    {

                        Global.createTransaction(accntID1,
                          lneDesc, funcCurrAmnt,
                          lnDte, funcCurrID, glBatchID, 0.00,
                          netAmnt, entrdAmnt, entrdCurrID, acntAmnt, accntCurrID, funcCurrRate, accntCurrRate, "D");
                    }
                    else
                    {
                        Global.createTransaction(accntID1,
                          lneDesc, 0.00,
                          lnDte, funcCurrID,
                          glBatchID, funcCurrAmnt, netAmnt,
                  entrdAmnt, entrdCurrID, acntAmnt, accntCurrID, funcCurrRate, accntCurrRate, "C");
                    }
                }
                //Receivable Balancing Leg

                int accntCurrID1 = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
          "accb.accb_chart_of_accnts", "accnt_id", "crncy_id", blncngAccntID));

                string slctdCurrID = entrdCurrID.ToString();
                double funcCurrRate1 = Math.Round(
            Global.get_LtstExchRate(int.Parse(slctdCurrID), this.curid, lnDte), 15);
                double accntCurrRate1 = Math.Round(
                  Global.get_LtstExchRate(int.Parse(slctdCurrID), accntCurrID1, lnDte), 15);
                System.Windows.Forms.Application.DoEvents();

                double grndAmnt = lnAmnt;

                funcCurrAmnt = (funcCurrRate1 * grndAmnt);
                double accntCurrAmnt = (accntCurrRate1 * grndAmnt);
                System.Windows.Forms.Application.DoEvents();

                double netAmnt1 = (double)Global.dbtOrCrdtAccntMultiplier(blncngAccntID,
            incrDcrs2) * (double)funcCurrAmnt;


                if (!Global.mnFrm.cmCde.isTransPrmttd(blncngAccntID, lnDte, netAmnt1))
                {
                    return false;
                }

                if (Global.getTrnsID(lneDesc, blncngAccntID, grndAmnt, entrdCurrID,
                    DateTime.ParseExact(
           lnDte, "dd-MMM-yyyy HH:mm:ss",
           System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss")) > 0)
                {
                    Global.mnFrm.cmCde.showMsg("Same Transaction has been created Already!\r\nConsider changing the Date or Time and Try Again!", 0);
                    Global.deleteBatchTrns(glBatchID);
                    Global.deleteBatch(glBatchID, glBatchName);
                    return false;
                }

                if (Global.dbtOrCrdtAccnt(blncngAccntID,
                  incrDcrs2) == "Debit")
                {
                    Global.createTransaction(blncngAccntID,
                      lneDesc +
                      " (Balacing Leg for Asset Trns:-" +
                      this.rgstrNmTextBox.Text + ")", funcCurrAmnt,
                      lnDte, this.curid, glBatchID, 0.00,
                      netAmnt1, grndAmnt, entrdCurrID,
                      accntCurrAmnt, accntCurrID1, funcCurrRate1, accntCurrRate1, "D");
                }
                else
                {
                    Global.createTransaction(blncngAccntID,
                      lneDesc +
                      " (Balancing Leg for Asset Trns:-" +
                      this.rgstrNmTextBox.Text + ")", 0.00,
                      lnDte, this.curid,
                      glBatchID, funcCurrAmnt, netAmnt1,
               grndAmnt, entrdCurrID, accntCurrAmnt,
               accntCurrID1, funcCurrRate1, accntCurrRate1, "C");
                }

                if (Global.get_Batch_CrdtSum(glBatchID) == Global.get_Batch_DbtSum(glBatchID))
                {
                    Global.updtEventCostGLBatch(eventCostID, glBatchID);
                    Global.updateBatchAvlblty(glBatchID, "1");
                    return true;
                }
                else
                {
                    Global.mnFrm.cmCde.showMsg("The GL Batch created is not Balanced!\r\nTransactions created will be reversed and deleted!", 0);
                    Global.deleteBatchTrns(glBatchID);
                    Global.deleteBatch(glBatchID, glBatchName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Global.mnFrm.cmCde.showMsg("Create Accounting Failed!\r\n" + ex.Message+"\r\n"+ex.StackTrace, 0);
                return false;
            }
        }

        private bool voidAttachedBatch(long eventCostID)
        {
            try
            {
                long glbatchID = long.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
            "attn.attn_attendance_costs", "attnd_cost_id", "gl_batch_id", eventCostID));

                string glbatchNm = Global.mnFrm.cmCde.getGnrlRecNm(
            "accb.accb_trnsctn_batches", "batch_id", "batch_name", glbatchID);

                string glbatchDesc = Global.mnFrm.cmCde.getGnrlRecNm(
            "accb.accb_trnsctn_batches", "batch_id", "batch_description", glbatchID);
                //Void Batch
                string dateStr = Global.mnFrm.cmCde.getFrmtdDB_Date_time();
                //Begin Process of voiding
                long beenPstdB4 = Global.getSimlrPstdBatchID(
                 glbatchID, glbatchNm, Global.mnFrm.cmCde.Org_id);
                if (beenPstdB4 > 0)
                {
                    {
                        Global.mnFrm.cmCde.showMsg("This batch has been reversed before\r\n Operation Cancelled!", 0);
                        return false;
                    }
                }
                string glNwBatchName = glbatchNm + " (Event Costing Cancellation@" + dateStr + ")";
                long nwbatchid = Global.mnFrm.cmCde.getGnrlRecID("accb.accb_trnsctn_batches",
                  "batch_name", "batch_id", glNwBatchName, Global.mnFrm.cmCde.Org_id);

                if (nwbatchid <= 0)
                {
                    Global.createBatch(Global.mnFrm.cmCde.Org_id,
                     glNwBatchName,
                     glbatchDesc + " (Event Costing Cancellation@" + dateStr + ")",
                     "Event Costing",
                     "VALID", glbatchID, "0");
                    Global.updateBatchVldtyStatus(glbatchID, "VOID");
                    nwbatchid = Global.mnFrm.cmCde.getGnrlRecID("accb.accb_trnsctn_batches",
                    "batch_name", "batch_id", glNwBatchName, Global.mnFrm.cmCde.Org_id);
                }
                //Get All Posted/Unposted Transactions in current batch
                DataSet dtst = Global.get_Batch_Trns_NoStatus(glbatchID);
                long ttltrns = dtst.Tables[0].Rows.Count;
                for (int i = 0; i < ttltrns; i++)
                {
                    if (Global.getTrnsID(dtst.Tables[0].Rows[i][3].ToString() + " (Event Costing Cancellation)"
                      , int.Parse(dtst.Tables[0].Rows[i][9].ToString())
                      , -1 * double.Parse(dtst.Tables[0].Rows[i][12].ToString()),
                      int.Parse(dtst.Tables[0].Rows[i][13].ToString()),
                      DateTime.ParseExact(
             dtst.Tables[0].Rows[i][6].ToString(), "dd-MMM-yyyy HH:mm:ss",
             System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss")) > 0)
                    {
                        Global.mnFrm.cmCde.showMsg("Same Transaction has been created Already!\r\nConsider changing the Date or Time and Try Again!", 0);
                        Global.deleteBatchTrns(nwbatchid);
                        Global.deleteBatch(nwbatchid, glNwBatchName);
                        return false;
                    }

                    Global.createTransaction(int.Parse(dtst.Tables[0].Rows[i][9].ToString()),
                    dtst.Tables[0].Rows[i][3].ToString() + " (Event Costing Cancellation)",
                    -1 * double.Parse(dtst.Tables[0].Rows[i][4].ToString()),
                    dtst.Tables[0].Rows[i][6].ToString(),
                    int.Parse(dtst.Tables[0].Rows[i][7].ToString()),
                    nwbatchid, -1 * double.Parse(dtst.Tables[0].Rows[i][5].ToString()),
                    -1 * double.Parse(dtst.Tables[0].Rows[i][10].ToString()),
               -1 * double.Parse(dtst.Tables[0].Rows[i][12].ToString()),
               int.Parse(dtst.Tables[0].Rows[i][13].ToString()),
               -1 * double.Parse(dtst.Tables[0].Rows[i][14].ToString()),
               int.Parse(dtst.Tables[0].Rows[i][15].ToString()),
               double.Parse(dtst.Tables[0].Rows[i][16].ToString()),
               double.Parse(dtst.Tables[0].Rows[i][17].ToString()),
               dtst.Tables[0].Rows[i][18].ToString());
                }
                //}
                Global.updateBatchAvlblty(nwbatchid, "1");
                Global.updtEventCostGLBatch(eventCostID, -1);
                //this.rvrsAppldPrepayHdrs();
                return true;
            }
            catch (Exception ex)
            {
                Global.mnFrm.cmCde.showMsg(ex.Message, 0);
                return false;
            }
        }

        private void costingDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e == null || this.obey_evnts == false ||
              (this.addRec == false && this.editRec == false))
            {
                return;
            }
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            bool prv = this.obey_evnts;
            this.obey_evnts = false;

            this.dfltFill1(e.RowIndex);


            if (e.ColumnIndex >= 0 && e.ColumnIndex <= 9)
            {
                this.costingDataGridView.EndEdit();
                System.Windows.Forms.Application.DoEvents();

                long row_id = long.Parse(this.costingDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString());
                long rgstrid = long.Parse(this.costingDataGridView.Rows[e.RowIndex].Cells[10].Value.ToString());
                long srcDocID = long.Parse(this.costingDataGridView.Rows[e.RowIndex].Cells[11].Value.ToString());
                string costcmnts = this.costingDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                string srcDocType = this.costingDataGridView.Rows[e.RowIndex].Cells[9].Value.ToString();

                string costCtgry = this.costingDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                int noPrsns = (int)double.Parse(this.costingDataGridView.Rows[e.RowIndex].Cells[6].Value.ToString());
                int noDays = (int)double.Parse(this.costingDataGridView.Rows[e.RowIndex].Cells[5].Value.ToString());

                double lnAmnt = 0;

                string orgnlAmnt = this.costingDataGridView.Rows[e.RowIndex].Cells[7].Value.ToString();
                bool isno = double.TryParse(orgnlAmnt, out lnAmnt);
                if (isno == false)
                {
                    lnAmnt = Math.Abs(Math.Round(Global.computeMathExprsn(orgnlAmnt), 2));
                }
                this.costingDataGridView.Rows[e.RowIndex].Cells[7].Value = lnAmnt.ToString("#,##0.00");

                double unitCst = lnAmnt;

                this.costingDataGridView.EndEdit();
                System.Windows.Forms.Application.DoEvents();

                this.costingDataGridView.Rows[e.RowIndex].Cells[8].Value = (double)noPrsns * (double)noDays * unitCst;

                Global.updtAttnCostLn(row_id, rgstrid, srcDocID, srcDocType,
                  costcmnts, noPrsns, noDays, unitCst, costCtgry);

                this.costingDataGridView.EndEdit();
                System.Windows.Forms.Application.DoEvents();
            }
            this.obey_evnts = true;
        }

        private void metricsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void autoLoadCostButton_Click(object sender, EventArgs e)
        {
            if (long.Parse(this.rgstrIDTextBox.Text) <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select a Register First!", 0);
                return;
            }
            DataSet dtst = Global.getEvntInvoices(long.Parse(this.rgstrIDTextBox.Text));
            int noDys = (int)Math.Ceiling((DateTime.Parse(this.evntEndDateTextBox.Text) - DateTime.Parse(this.evntStrtDateTextBox.Text)).TotalDays);
            int recs = 0;
            for (int a = 0; a < dtst.Tables[0].Rows.Count; a++)
            {
                long evntCostID = Global.getEventCostID(long.Parse(this.rgstrIDTextBox.Text),
                    long.Parse(dtst.Tables[0].Rows[a][0].ToString()),
                    dtst.Tables[0].Rows[a][2].ToString());

                if (evntCostID <= 0)
                {
                    Global.createAttnCostLn(long.Parse(this.rgstrIDTextBox.Text),
                      long.Parse(dtst.Tables[0].Rows[a][0].ToString()),
                      dtst.Tables[0].Rows[a][2].ToString(),
                      dtst.Tables[0].Rows[a][3].ToString(),
                      int.Parse(dtst.Tables[0].Rows[a][6].ToString()), noDys,
                      double.Parse(dtst.Tables[0].Rows[a][4].ToString()) / double.Parse(dtst.Tables[0].Rows[a][6].ToString()),
                      dtst.Tables[0].Rows[a][5].ToString());
                    recs++;
                }
                else
                {
                    Global.updtAttnCostLn(evntCostID, long.Parse(this.rgstrIDTextBox.Text),
              long.Parse(dtst.Tables[0].Rows[a][0].ToString()),
              dtst.Tables[0].Rows[a][2].ToString(),
              dtst.Tables[0].Rows[a][3].ToString(),
              int.Parse(dtst.Tables[0].Rows[a][6].ToString()), noDys, double.Parse(dtst.Tables[0].Rows[a][4].ToString()) / double.Parse(dtst.Tables[0].Rows[a][6].ToString()),
              dtst.Tables[0].Rows[a][5].ToString());
                }
            }

            DataSet pybldtst = Global.getEvntPayables(long.Parse(this.rgstrIDTextBox.Text));

            for (int a = 0; a < pybldtst.Tables[0].Rows.Count; a++)
            {
                long evntCostID = Global.getEventCostID(long.Parse(this.rgstrIDTextBox.Text),
                    long.Parse(pybldtst.Tables[0].Rows[a][0].ToString()),
                    pybldtst.Tables[0].Rows[a][2].ToString());

                if (evntCostID <= 0)
                {
                    Global.createAttnCostLn(long.Parse(this.rgstrIDTextBox.Text),
                      long.Parse(pybldtst.Tables[0].Rows[a][0].ToString()),
                      pybldtst.Tables[0].Rows[a][2].ToString(),
                      pybldtst.Tables[0].Rows[a][3].ToString(),
                      1, noDys, double.Parse(pybldtst.Tables[0].Rows[a][4].ToString()) / ((double)(1)),
                      pybldtst.Tables[0].Rows[a][5].ToString());
                    recs++;
                }
                else
                {
                    Global.updtAttnCostLn(evntCostID, long.Parse(this.rgstrIDTextBox.Text),
              long.Parse(pybldtst.Tables[0].Rows[a][0].ToString()),
              pybldtst.Tables[0].Rows[a][2].ToString(),
              pybldtst.Tables[0].Rows[a][3].ToString(),
              1, noDys, double.Parse(pybldtst.Tables[0].Rows[a][4].ToString()) / ((double)(1)),
              pybldtst.Tables[0].Rows[a][5].ToString());
                }
            }

            DataSet rcvbldtst = Global.getEvntReceivables(long.Parse(this.rgstrIDTextBox.Text));

            for (int a = 0; a < rcvbldtst.Tables[0].Rows.Count; a++)
            {
                long evntCostID = Global.getEventCostID(long.Parse(this.rgstrIDTextBox.Text),
                    long.Parse(rcvbldtst.Tables[0].Rows[a][0].ToString()),
                    rcvbldtst.Tables[0].Rows[a][2].ToString());

                if (evntCostID <= 0)
                {
                    Global.createAttnCostLn(long.Parse(this.rgstrIDTextBox.Text),
                      long.Parse(rcvbldtst.Tables[0].Rows[a][0].ToString()),
                      rcvbldtst.Tables[0].Rows[a][2].ToString(),
                      rcvbldtst.Tables[0].Rows[a][3].ToString(),
                      1, noDys, double.Parse(rcvbldtst.Tables[0].Rows[a][4].ToString()) / ((double)(1)),
                      rcvbldtst.Tables[0].Rows[a][5].ToString());
                    recs++;
                }
                else
                {
                    Global.updtAttnCostLn(evntCostID, long.Parse(this.rgstrIDTextBox.Text),
              long.Parse(rcvbldtst.Tables[0].Rows[a][0].ToString()),
              rcvbldtst.Tables[0].Rows[a][2].ToString(),
              rcvbldtst.Tables[0].Rows[a][3].ToString(),
              1, noDys, double.Parse(rcvbldtst.Tables[0].Rows[a][4].ToString()) / ((double)(1)),
              rcvbldtst.Tables[0].Rows[a][5].ToString());
                }
            }
            Global.deleteCancelled(long.Parse(this.rgstrIDTextBox.Text));
            Global.mnFrm.cmCde.showMsg(recs + "New Record(s) Loaded Successfully!", 3);
            this.refreshCostButton.PerformClick();
        }

        private void exportCostsToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridView tstGrid = (DataGridView)this.contextMenuStrip1.SourceControl;
                string rptTitle = "ATTENDANCE REGISTER FOR " + this.rgstrDescTextBox.Text +
                  " (" + this.evntStrtDateTextBox.Text + " TO " + this.evntEndDateTextBox.Text + ")";
                if (tstGrid.Name == this.costingDataGridView.Name)
                {
                    rptTitle = "INCOME/EXPENDITURE REPORT IRO " + this.rgstrDescTextBox.Text +
                    " (" + this.evntStrtDateTextBox.Text + " TO " + this.evntEndDateTextBox.Text + ")";
                }
                Global.mnFrm.cmCde.exprtToExcelSelective(tstGrid, rptTitle);
            }
            catch (Exception ex)
            {
            }
        }

        private void duplicateRgstrButton_Click(object sender, EventArgs e)
        {
            if (this.rgstrIDTextBox.Text == "" || this.rgstrIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("No record to Duplicate!", 0);
                return;
            }
            if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[8]) == false)
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                 " this action!\nContact your System Administrator!", 0);
                return;
            }
            long oldRgstrID = Global.getAttnRgstrID("(Duplicate) " + this.rgstrNmTextBox.Text,
                Global.mnFrm.cmCde.Org_id);
            if (oldRgstrID > 0)
            {
                Global.mnFrm.cmCde.showMsg("A duplicate Attendance Register with the name " +
                  "(Duplicate) " + this.rgstrNmTextBox.Text + " already Exists!", 0);
                return;
            }
            else if (oldRgstrID <= 0)
            {
                if (Global.mnFrm.cmCde.showMsg("Are you sure you want to DUPLICATE the selected Register?", 1) == DialogResult.No)
                {
                    //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                    return;
                }
                string dtes = Global.mnFrm.cmCde.getFrmtdDB_Date_time();
                Global.createAttnRgstr(Global.mnFrm.cmCde.Org_id, "(Duplicate) " + this.rgstrNmTextBox.Text,
                  this.rgstrDescTextBox.Text, int.Parse(this.tmTblIDTextBox.Text),
                  long.Parse(this.tmTblDetIDTextBox.Text), dtes, dtes);

                oldRgstrID = Global.getAttnRgstrID("(Duplicate) " + this.rgstrNmTextBox.Text,
                  Global.mnFrm.cmCde.Org_id);
                DataSet oldDtSt = Global.get_One_AttnRgstr_DetLns("%",
                "Person Name/ID", 0, 1000000000, long.Parse(this.rgstrIDTextBox.Text));

                for (int i = 0; i < oldDtSt.Tables[0].Rows.Count; i++)
                {
                    Global.createAttnRgstrDetLn(oldRgstrID, long.Parse(oldDtSt.Tables[0].Rows[i][2].ToString()),
          "", "", false, "", oldDtSt.Tables[0].Rows[i][12].ToString(), 1,
          long.Parse(oldDtSt.Tables[0].Rows[i][12].ToString()),
          oldDtSt.Tables[0].Rows[i][9].ToString(),
          long.Parse(oldDtSt.Tables[0].Rows[i][13].ToString()));
                }
            }
            string oldNm = "(Duplicate) " + this.rgstrNmTextBox.Text;
            this.loadPanel();
            System.Threading.Thread.Sleep(1000);
            if (this.rgstrNmTextBox.Text == oldNm)
            {
                this.editButton_Click(this.editButton, e);
            }
        }

        private void resetDetButton_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.minimizeMemory();

            this.searchInDetComboBox.SelectedIndex = 4;
            this.searchForDetTextBox.Text = "%";
            this.dsplySizeDetComboBox.Text = "10";
            this.disableLnsEdit();
            this.rec_det_cur_indx = 0;
            this.loadRgstrDetLnsPanel();
        }
    }
}

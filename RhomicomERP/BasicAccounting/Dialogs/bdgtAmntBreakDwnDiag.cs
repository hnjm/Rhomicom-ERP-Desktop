using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Accounting.Classes;
using Microsoft.VisualBasic;
using Excel = Microsoft.Office.Interop.Excel;

namespace Accounting.Dialogs
{
    public partial class bdgtAmntBreakDwnDiag : Form
    {
        public bdgtAmntBreakDwnDiag()
        {
            InitializeComponent();
        }

        public long trnsaction_id = -1;
        public int inptAccountID = -1;
        public long budgetID = -1;
        public string inStartDate = "";
        public string inEndDate = "";
        public bool obey_evnts = false;
        public bool editMode = false;
        bool addRec = false;
        bool editRec = false;
        bool addDtRec = false;
        bool editDtRec = false;
        bool isClosing = false;
        bool addRecsP = false;
        bool editRecsP = false;
        bool delRecsP = false;
        public string chrt_SQL = "";
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.saveTrnsBatchButton.Enabled = false;
            this.OKButton.Enabled = false;
            int accntID = int.Parse(this.accntIDTextBox.Text);
            double ttlAmnt1 = 0;
            this.trnsDataGridView.EndEdit();
            System.Windows.Forms.Application.DoEvents();
            for (int i = 0; i < this.trnsDataGridView.Rows.Count; i++)
            {
                this.dfltFill(i);
                long bdgtDetID = -1;
                long.TryParse(this.trnsDataGridView.Rows[i].Cells[11].Value.ToString(), out bdgtDetID);
                if (bdgtDetID == this.trnsaction_id)
                {
                    ttlAmnt1 += double.Parse(this.trnsDataGridView.Rows[i].Cells[7].Value.ToString());
                }
            }
            this.ttlNumUpDwn.Value = (decimal)ttlAmnt1;
            for (int i = 0; i < this.trnsDataGridView.Rows.Count; i++)
            {
                int bdgtItmID = -1;
                int.TryParse(this.trnsDataGridView.Rows[i].Cells[1].Value.ToString(), out bdgtItmID);

                long bdgtBrkDwnID = -1;
                long.TryParse(this.trnsDataGridView.Rows[i].Cells[9].Value.ToString(), out bdgtBrkDwnID);
                long bdgtDetID = -1;
                long.TryParse(this.trnsDataGridView.Rows[i].Cells[11].Value.ToString(), out bdgtDetID);
                double mltplr1 = 0;
                double.TryParse(this.trnsDataGridView.Rows[i].Cells[4].Value.ToString(), out mltplr1);
                double mltplr2 = 0;
                double.TryParse(this.trnsDataGridView.Rows[i].Cells[5].Value.ToString(), out mltplr2);
                double unitAmnt = 0;
                double.TryParse(this.trnsDataGridView.Rows[i].Cells[6].Value.ToString(), out unitAmnt);
                double ttlAmnt = 0;
                double.TryParse(this.trnsDataGridView.Rows[i].Cells[7].Value.ToString(), out ttlAmnt);

                string bdgtDetType = this.trnsDataGridView.Rows[i].Cells[3].Value.ToString().Trim();
                string lneDesc = this.trnsDataGridView.Rows[i].Cells[0].Value.ToString().Trim();
                this.inStartDate = this.trnsDataGridView.Rows[i].Cells[12].Value.ToString().Trim();
                this.inEndDate = this.trnsDataGridView.Rows[i].Cells[14].Value.ToString().Trim();
                if (ttlAmnt != 0 && bdgtDetType != "" && bdgtItmID > 0 && bdgtDetID == this.trnsaction_id)
                {
                    if (bdgtBrkDwnID <= 0)
                    {
                        bdgtBrkDwnID = Global.getNewBrkDwnLnID();
                        Global.createBdgtBrkDwn(bdgtBrkDwnID, accntID, bdgtItmID, bdgtDetType, lneDesc, mltplr1, mltplr2, unitAmnt, this.trnsaction_id, this.inStartDate, this.inEndDate);
                        this.trnsDataGridView.Rows[i].Cells[9].Value = bdgtBrkDwnID;
                    }
                    else
                    {
                        Global.updateBdgtBrkDwn(bdgtBrkDwnID, accntID, bdgtItmID, bdgtDetType, lneDesc, mltplr1, mltplr2, unitAmnt, this.trnsaction_id, this.inStartDate, this.inEndDate);
                    }
                }
            }
            this.populateBdgtBrkDwn(accntID);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void bdgtAmntBreakDwnDiag_Load(object sender, EventArgs e)
        {
            Color[] clrs = Global.mnFrm.cmCde.getColors();
            this.BackColor = clrs[0];
            this.disableFormButtons();
            this.accntIDTextBox.Text = this.inptAccountID.ToString();
            this.accntNameTextBox.Text = Global.mnFrm.cmCde.getAccntName(this.inptAccountID);
            this.accntNumTextBox.Text = Global.mnFrm.cmCde.getAccntNum(this.inptAccountID);
            this.populateBdgtBrkDwn(this.inptAccountID);
        }

        public void disableFormButtons()
        {
            bool vwSQL = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[10]);
            bool rcHstry = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[9]);
            //this.addRecsP = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[11]);
            //this.editRecsP = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[12]);
            //this.delRecsP = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[13]);

            this.addTrnsLineButton.Enabled = this.editMode;
            this.delLineButton.Enabled = this.editMode;
            this.OKButton.Enabled = this.editMode;
            this.saveTrnsBatchButton.Enabled = this.editMode;
            this.vwSQLChrtButton.Enabled = vwSQL;
            this.recHstryChrtButton.Enabled = rcHstry;
            this.importTrnsButton.Enabled = this.editMode;
        }

        private void populateBdgtBrkDwn(int accountID)
        {
            if (this.editMode)
            {
                this.prprForBrkDwnEdit();
            }
            else
            {
                this.disableBrkDwnEdit();
            }
            this.obey_evnts = false;
            DataSet dtst = null;
            if (this.shwAllPrdsCheckBox.Checked == true)
            {
                dtst = Global.get_Bdgt_AmntBrkdwn(accountID, -1, ref chrt_SQL);
            }
            else
            {
                dtst = Global.get_Bdgt_AmntBrkdwn(accountID, this.trnsaction_id, ref chrt_SQL);
            }
            this.trnsDataGridView.Rows.Clear();

            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                this.obey_evnts = false;
                this.trnsDataGridView.RowCount += 1;
                int rowIdx = this.trnsDataGridView.RowCount - 1;
                this.trnsDataGridView.Rows[rowIdx].HeaderCell.Value = this.trnsDataGridView.RowCount.ToString();
                this.trnsDataGridView.Rows[rowIdx].Cells[0].Value = dtst.Tables[0].Rows[i][3].ToString();
                this.trnsDataGridView.Rows[rowIdx].Cells[1].Value = dtst.Tables[0].Rows[i][2].ToString();
                this.trnsDataGridView.Rows[rowIdx].Cells[2].Value = "...";
                this.trnsDataGridView.Rows[rowIdx].Cells[3].Value = dtst.Tables[0].Rows[i][4].ToString();
                this.trnsDataGridView.Rows[rowIdx].Cells[4].Value = double.Parse(dtst.Tables[0].Rows[i][5].ToString()).ToString();
                this.trnsDataGridView.Rows[rowIdx].Cells[5].Value = double.Parse(dtst.Tables[0].Rows[i][6].ToString()).ToString();
                this.trnsDataGridView.Rows[rowIdx].Cells[6].Value = double.Parse(dtst.Tables[0].Rows[i][7].ToString()).ToString("#,##0.00");
                this.trnsDataGridView.Rows[rowIdx].Cells[7].Value = double.Parse(dtst.Tables[0].Rows[i][8].ToString()).ToString("#,##0.00");
                this.trnsDataGridView.Rows[rowIdx].Cells[8].Value = dtst.Tables[0].Rows[i][9].ToString();
                this.trnsDataGridView.Rows[rowIdx].Cells[9].Value = dtst.Tables[0].Rows[i][0].ToString();
                this.trnsDataGridView.Rows[rowIdx].Cells[10].Value = dtst.Tables[0].Rows[i][1].ToString();
                this.trnsDataGridView.Rows[rowIdx].Cells[11].Value = dtst.Tables[0].Rows[i][10].ToString();
                this.trnsDataGridView.Rows[rowIdx].Cells[12].Value = dtst.Tables[0].Rows[i][11].ToString();
                this.trnsDataGridView.Rows[rowIdx].Cells[13].Value = "...";
                this.trnsDataGridView.Rows[rowIdx].Cells[14].Value = dtst.Tables[0].Rows[i][12].ToString();
                this.trnsDataGridView.Rows[rowIdx].Cells[15].Value = "...";
            }
            if (this.trnsDataGridView.Rows.Count > 0)
            {
                this.trnsDataGridView.CurrentCell = this.trnsDataGridView.Rows[this.trnsDataGridView.Rows.Count - 1].Cells[0];
            }
            this.obey_evnts = true;
            double ttlAmnt = 0;
            this.trnsDataGridView.EndEdit();
            System.Windows.Forms.Application.DoEvents();
            for (int i = 0; i < this.trnsDataGridView.Rows.Count; i++)
            {
                this.dfltFill(i);
                ttlAmnt += double.Parse(this.trnsDataGridView.Rows[i].Cells[7].Value.ToString());
            }
            this.ttlNumUpDwn.Value = (decimal)ttlAmnt;
        }

        private void disableBrkDwnEdit()
        {
            this.trnsDataGridView.ReadOnly = true;
            this.trnsDataGridView.Columns[0].ReadOnly = true;
            this.trnsDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.Gainsboro;

            this.trnsDataGridView.Columns[3].ReadOnly = true;
            this.trnsDataGridView.Columns[3].DefaultCellStyle.BackColor = Color.Gainsboro;

            this.trnsDataGridView.Columns[4].ReadOnly = true;
            this.trnsDataGridView.Columns[4].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.trnsDataGridView.Columns[5].ReadOnly = true;
            this.trnsDataGridView.Columns[5].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.trnsDataGridView.Columns[6].ReadOnly = true;
            this.trnsDataGridView.Columns[6].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.trnsDataGridView.Columns[7].ReadOnly = true;
            this.trnsDataGridView.Columns[7].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.trnsDataGridView.Columns[8].ReadOnly = true;
            this.trnsDataGridView.Columns[8].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.trnsDataGridView.Columns[12].ReadOnly = true;
            this.trnsDataGridView.Columns[12].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.trnsDataGridView.Columns[14].ReadOnly = true;
            this.trnsDataGridView.Columns[14].DefaultCellStyle.BackColor = Color.Gainsboro;
        }

        private void prprForBrkDwnEdit()
        {
            this.trnsDataGridView.ReadOnly = false;
            this.trnsDataGridView.Columns[0].ReadOnly = true;
            this.trnsDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);

            this.trnsDataGridView.Columns[3].ReadOnly = false;
            this.trnsDataGridView.Columns[3].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);

            this.trnsDataGridView.Columns[4].ReadOnly = false;
            this.trnsDataGridView.Columns[4].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.trnsDataGridView.Columns[5].ReadOnly = false;
            this.trnsDataGridView.Columns[5].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.trnsDataGridView.Columns[6].ReadOnly = false;
            this.trnsDataGridView.Columns[6].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.trnsDataGridView.Columns[7].ReadOnly = true;
            this.trnsDataGridView.Columns[7].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.trnsDataGridView.Columns[8].ReadOnly = false;
            this.trnsDataGridView.Columns[8].DefaultCellStyle.BackColor = Color.White;
            this.trnsDataGridView.Columns[12].ReadOnly = true;
            this.trnsDataGridView.Columns[12].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.trnsDataGridView.Columns[14].ReadOnly = true;
            this.trnsDataGridView.Columns[14].DefaultCellStyle.BackColor = Color.WhiteSmoke;
        }

        private void trnsDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e == null || this.obey_evnts == false)
            {
                return;
            }
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            this.obey_evnts = false;
            if (e.ColumnIndex == 4)
            {
                this.dfltFill(e.RowIndex);
                double lnAmnt = 0;
                string orgnlAmnt = this.trnsDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
                bool isno = double.TryParse(orgnlAmnt, out lnAmnt);
                if (isno == false)
                {
                    lnAmnt = Math.Round(Global.computeMathExprsn(orgnlAmnt), 15);
                }
                this.trnsDataGridView.Rows[e.RowIndex].Cells[4].Value = Math.Round(lnAmnt, 15);
                double mltplr2 = 0;
                double unitAmnt = 0;
                double.TryParse(this.trnsDataGridView.Rows[e.RowIndex].Cells[6].Value.ToString(), out unitAmnt);
                double.TryParse(this.trnsDataGridView.Rows[e.RowIndex].Cells[5].Value.ToString(), out mltplr2);
                this.trnsDataGridView.Rows[e.RowIndex].Cells[7].Value = (unitAmnt * lnAmnt * mltplr2).ToString("#,##0.00");
            }
            else if (e.ColumnIndex == 5)
            {
                this.dfltFill(e.RowIndex);
                double lnAmnt = 0;
                string orgnlAmnt = this.trnsDataGridView.Rows[e.RowIndex].Cells[5].Value.ToString();
                bool isno = double.TryParse(orgnlAmnt, out lnAmnt);
                if (isno == false)
                {
                    lnAmnt = Math.Round(Global.computeMathExprsn(orgnlAmnt), 15);
                }
                this.trnsDataGridView.Rows[e.RowIndex].Cells[5].Value = Math.Round(lnAmnt, 15);
                double qty = 0;
                double mltplr2 = 0;
                double unitAmnt = 0;
                double.TryParse(this.trnsDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString(), out qty);
                double.TryParse(this.trnsDataGridView.Rows[e.RowIndex].Cells[6].Value.ToString(), out unitAmnt);
                this.trnsDataGridView.Rows[e.RowIndex].Cells[7].Value = (qty * lnAmnt * unitAmnt).ToString("#,##0.00");
                if (e.RowIndex == this.trnsDataGridView.Rows.Count - 1)
                {
                    this.addTrnsLineButton.PerformClick();
                }
            }
            else if (e.ColumnIndex == 6)
            {
                this.dfltFill(e.RowIndex);
                double lnAmnt = 0;
                string orgnlAmnt = this.trnsDataGridView.Rows[e.RowIndex].Cells[6].Value.ToString();
                bool isno = double.TryParse(orgnlAmnt, out lnAmnt);
                if (isno == false)
                {
                    lnAmnt = Math.Round(Global.computeMathExprsn(orgnlAmnt), 15);
                }
                this.trnsDataGridView.Rows[e.RowIndex].Cells[6].Value = Math.Round(lnAmnt, 15);
                double qty = 0;
                double mltplr2 = 0;
                double.TryParse(this.trnsDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString(), out qty);
                double.TryParse(this.trnsDataGridView.Rows[e.RowIndex].Cells[5].Value.ToString(), out mltplr2);
                this.trnsDataGridView.Rows[e.RowIndex].Cells[7].Value = (qty * lnAmnt * mltplr2).ToString("#,##0.00");
                if (e.RowIndex == this.trnsDataGridView.Rows.Count - 1)
                {
                    this.addTrnsLineButton.PerformClick();
                }
            }
            else if (e.ColumnIndex == 12)
            {
                DateTime dte1 = DateTime.Now;
                bool sccs = DateTime.TryParse(this.trnsDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString(), out dte1);
                if (!sccs)
                {
                    dte1 = DateTime.Now;
                }
                this.trnsDataGridView.EndEdit();
                this.trnsDataGridView.Rows[e.RowIndex].Cells[12].Value = dte1.ToString("dd-MMM-yyyy HH:mm:ss");
                System.Windows.Forms.Application.DoEvents();
            }
            else if (e.ColumnIndex == 14)
            {
                DateTime dte1 = DateTime.Now;
                bool sccs = DateTime.TryParse(this.trnsDataGridView.Rows[e.RowIndex].Cells[14].Value.ToString(), out dte1);
                if (!sccs)
                {
                    dte1 = DateTime.Now;
                }
                this.trnsDataGridView.EndEdit();
                this.trnsDataGridView.Rows[e.RowIndex].Cells[14].Value = dte1.ToString("dd-MMM-yyyy HH:mm:ss");
                System.Windows.Forms.Application.DoEvents();
            }
            this.obey_evnts = true;

        }

        private void addTrnsLineButton_Click(object sender, EventArgs e)
        {
            this.createTrnsRows(1);
        }

        public void createTrnsRows(int num)
        {
            this.obey_evnts = false;
            for (int i = 0; i < num; i++)
            {
                int rowIdx = this.trnsDataGridView.RowCount;
                if (this.trnsDataGridView.CurrentCell != null)
                {
                    rowIdx = this.trnsDataGridView.CurrentCell.RowIndex + 1;
                }
                this.trnsDataGridView.Rows.Insert(rowIdx, 1);
                this.trnsDataGridView.Rows[rowIdx].Cells[0].Value = "";
                this.trnsDataGridView.Rows[rowIdx].Cells[1].Value = "-1";
                this.trnsDataGridView.Rows[rowIdx].Cells[2].Value = "...";
                this.trnsDataGridView.Rows[rowIdx].Cells[3].Value = "Item Quantity";
                this.trnsDataGridView.Rows[rowIdx].Cells[4].Value = "0.00";
                this.trnsDataGridView.Rows[rowIdx].Cells[5].Value = "1.00";
                this.trnsDataGridView.Rows[rowIdx].Cells[6].Value = "0.00";
                this.trnsDataGridView.Rows[rowIdx].Cells[7].Value = "0.00";
                this.trnsDataGridView.Rows[rowIdx].Cells[8].Value = "";
                this.trnsDataGridView.Rows[rowIdx].Cells[9].Value = "-1";
                this.trnsDataGridView.Rows[rowIdx].Cells[10].Value = this.accntIDTextBox.Text;
                this.trnsDataGridView.Rows[rowIdx].Cells[11].Value = this.trnsaction_id;
                this.trnsDataGridView.Rows[rowIdx].Cells[12].Value = this.inStartDate;
                this.trnsDataGridView.Rows[rowIdx].Cells[13].Value = "...";
                this.trnsDataGridView.Rows[rowIdx].Cells[14].Value = this.inEndDate;
                this.trnsDataGridView.Rows[rowIdx].Cells[15].Value = "...";
            }
            for (int i = 0; i < this.trnsDataGridView.Rows.Count; i++)
            {
                this.trnsDataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
            this.obey_evnts = true;
        }

        private void delLineButton_Click(object sender, EventArgs e)
        {
            if (this.trnsDataGridView.CurrentCell != null)
            {
                this.trnsDataGridView.Rows[this.trnsDataGridView.CurrentCell.RowIndex].Selected = true;
            }
            if (this.trnsDataGridView.SelectedRows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select the lines to be Deleted First!", 0);
                return;
            }
            if (Global.mnFrm.cmCde.showMsg("Are you sure you want to DELETE the selected Record?" +
             "\r\nThis action cannot be undone!", 1) == DialogResult.No)
            {
                //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                return;
            }
            int slctdrows = this.trnsDataGridView.SelectedRows.Count;
            for (int i = 0; i < slctdrows; i++)
            {
                long trnsDetID = long.Parse(this.trnsDataGridView.Rows[this.trnsDataGridView.SelectedRows[0].Index].Cells[9].Value.ToString());
                Global.deleteBdgtBrkDwn(trnsDetID);
                this.trnsDataGridView.Rows.RemoveAt(this.trnsDataGridView.SelectedRows[0].Index);
            }
        }

        private void dfltFill(int idx)
        {
            if (this.trnsDataGridView.Rows[idx].Cells[0].Value == null)
            {
                this.trnsDataGridView.Rows[idx].Cells[0].Value = "";
            }
            if (this.trnsDataGridView.Rows[idx].Cells[1].Value == null)
            {
                this.trnsDataGridView.Rows[idx].Cells[1].Value = "-1";
            }
            if (this.trnsDataGridView.Rows[idx].Cells[3].Value == null)
            {
                this.trnsDataGridView.Rows[idx].Cells[3].Value = "Item Quantity";
            }
            if (this.trnsDataGridView.Rows[idx].Cells[4].Value == null)
            {
                this.trnsDataGridView.Rows[idx].Cells[4].Value = "0";
            }
            if (this.trnsDataGridView.Rows[idx].Cells[5].Value == null)
            {
                this.trnsDataGridView.Rows[idx].Cells[5].Value = "1";
            }
            if (this.trnsDataGridView.Rows[idx].Cells[6].Value == null)
            {
                this.trnsDataGridView.Rows[idx].Cells[6].Value = "0.00";
            }
            if (this.trnsDataGridView.Rows[idx].Cells[7].Value == null)
            {
                this.trnsDataGridView.Rows[idx].Cells[7].Value = "0.00";
            }
            if (this.trnsDataGridView.Rows[idx].Cells[8].Value == null)
            {
                this.trnsDataGridView.Rows[idx].Cells[8].Value = "";
            }
            if (this.trnsDataGridView.Rows[idx].Cells[9].Value == null)
            {
                this.trnsDataGridView.Rows[idx].Cells[9].Value = "-1";
            }
            if (this.trnsDataGridView.Rows[idx].Cells[10].Value == null)
            {
                this.trnsDataGridView.Rows[idx].Cells[10].Value = "-1";
            }
            if (this.trnsDataGridView.Rows[idx].Cells[11].Value == null)
            {
                this.trnsDataGridView.Rows[idx].Cells[11].Value = "-1";
            }
            if (this.trnsDataGridView.Rows[idx].Cells[12].Value == null)
            {
                this.trnsDataGridView.Rows[idx].Cells[12].Value = "";
            }
            if (this.trnsDataGridView.Rows[idx].Cells[14].Value == null)
            {
                this.trnsDataGridView.Rows[idx].Cells[14].Value = "";
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            this.populateBdgtBrkDwn(int.Parse(this.accntIDTextBox.Text));
        }

        private void saveTrnsBatchButton_Click(object sender, EventArgs e)
        {
            this.OKButton_Click(this.OKButton, e);
        }

        private void trnsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

            if (e.ColumnIndex == 2)
            {
                if (this.editMode == false)
                {
                    Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                    this.obey_evnts = true;
                    return;
                }
                string[] selVals = new string[1];
                selVals[0] = this.trnsDataGridView.Rows[e.RowIndex].Cells[7].Value.ToString();
                DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                    Global.mnFrm.cmCde.getLovID("Inventory Items"), ref selVals,
                    true, false, Global.mnFrm.cmCde.Org_id,
                 "%", "Both", true);
                if (dgRes == DialogResult.OK)
                {
                    for (int i = 0; i < selVals.Length; i++)
                    {
                        this.trnsDataGridView.Rows[e.RowIndex].Cells[1].Value = selVals[i];
                        this.trnsDataGridView.Rows[e.RowIndex].Cells[0].Value = Global.get_InvItemNm(
                         int.Parse(selVals[i]));
                        this.trnsDataGridView.Rows[e.RowIndex].Cells[6].Value = Global.get_InvItemPrice(int.Parse(selVals[i])).ToString("#,##0.00");
                        this.trnsDataGridView.Rows[e.RowIndex].Cells[7].Value = (double.Parse(this.trnsDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString())
                                                                                * double.Parse(this.trnsDataGridView.Rows[e.RowIndex].Cells[5].Value.ToString())
                                                                                * double.Parse(this.trnsDataGridView.Rows[e.RowIndex].Cells[6].Value.ToString())).ToString("#,##0.00");
                    }
                }
            }
            else if (e.ColumnIndex == 13)
            {
                this.textBox1.Text = this.trnsDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString();
                Global.mnFrm.cmCde.selectDate(ref this.textBox1);
                this.trnsDataGridView.Rows[e.RowIndex].Cells[12].Value = this.textBox1.Text;
                this.trnsDataGridView.EndEdit();
                System.Windows.Forms.Application.DoEvents();

            }
            else if (e.ColumnIndex == 15)
            {
                this.textBox1.Text = this.trnsDataGridView.Rows[e.RowIndex].Cells[14].Value.ToString();
                Global.mnFrm.cmCde.selectDate(ref this.textBox1);
                this.trnsDataGridView.Rows[e.RowIndex].Cells[14].Value = this.textBox1.Text;
                this.trnsDataGridView.EndEdit();
                System.Windows.Forms.Application.DoEvents();

            }
            this.obey_evnts = true;
        }

        private void accntNumButton_Click(object sender, EventArgs e)
        {
            this.accntNmLOVSearch();
        }

        private void accntNmLOVSearch()
        {
            if (!this.accntNumTextBox.Text.Contains("%"))
            {
                this.accntNumTextBox.Text = "%" + this.accntNumTextBox.Text.Replace(" ", "%") + "%";
                this.accntIDTextBox.Text = "-1";
            }
            int accntID = int.Parse(this.accntIDTextBox.Text);
            string[] selVals = new string[1];
            selVals[0] = accntID.ToString();
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
              Global.mnFrm.cmCde.getLovID("All Accounts"),
              ref selVals, true, true, Global.mnFrm.cmCde.Org_id,
              this.accntNumTextBox.Text, "Both", false);

            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    accntID = int.Parse(selVals[i]);
                    this.accntIDTextBox.Text = accntID.ToString();
                    this.accntNameTextBox.Text = Global.mnFrm.cmCde.getAccntName(accntID);
                    this.accntNumTextBox.Text = Global.mnFrm.cmCde.getAccntNum(accntID);
                    this.populateBdgtBrkDwn(accntID);
                }
            }
            /*bool isReadOnly = false;
             Global.mnFrm.cmCde.showAcntsDiag(ref accntID, true, true, this.accntNumTextBox.Text, "Account Details", true, isReadOnly, Global.mnFrm.cmCde);
             this.accntIDTextBox.Text = accntID.ToString();
             this.accntNameTextBox.Text = Global.mnFrm.cmCde.getAccntName(accntID);
             this.accntNumTextBox.Text = Global.mnFrm.cmCde.getAccntNum(accntID);*/
        }

        private void cmpTtlButton_Click(object sender, EventArgs e)
        {
            double ttlAmnt = 0;
            for (int i = 0; i < this.trnsDataGridView.Rows.Count; i++)
            {
                this.dfltFill(i);
                ttlAmnt += double.Parse(this.trnsDataGridView.Rows[i].Cells[7].Value.ToString());
            }
            this.ttlNumUpDwn.Value = (decimal)ttlAmnt;
        }

        private void vwSQLChrtButton_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.showSQL(this.chrt_SQL, 10);
        }

        private void recHstryChrtButton_Click(object sender, EventArgs e)
        {
            if (this.trnsDataGridView.CurrentCell != null && this.trnsDataGridView.SelectedRows.Count <= 0)
            {
                this.trnsDataGridView.Rows[this.trnsDataGridView.CurrentCell.RowIndex].Selected = true;
            }
            if (this.trnsDataGridView.SelectedRows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select a Record First!", 0);
                return;
            }
            Global.mnFrm.cmCde.showRecHstry(
              Global.mnFrm.cmCde.get_Gnrl_Rec_Hstry(
              long.Parse(this.trnsDataGridView.SelectedRows[0].Cells[9].Value.ToString()),
              "accb.accb_bdgt_amnt_brkdwn", "bdgt_amnt_brkdwn_id"), 9);
        }

        private void exportTrnsButton_Click(object sender, EventArgs e)
        {
            string rspnse = Interaction.InputBox("How many Budgeting Lines will you like to Export?" +
              "\r\n1=No Budgeting Lines(Empty Template)" +
              "\r\n2=All Budgeting Lines" +
              "\r\n3-Infinity=Specify the exact number of Budgeting Lines to Export\r\n",
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
            this.exprtBdgtLinesTmp(rsponse);
        }

        private void exprtBdgtLinesTmp(int exprtTyp)
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
            string[] hdngs = {"Account Number**", "Account Description**", "Budget Item Code**", "Budget Item Description**", "Budget Detail Type**",
                "Multiplier 1 (Qty or No. of Persons)**", "Multiplier 2 (E.g. No. of Days | Type 1 if N/A)**",
                "Unit Amount**", "Total Amount", "Remarks/Justifications", "Period Start Date**", "Period End Date**"};

            for (int a = 0; a < hdngs.Length; a++)
            {
                ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[5, (a + 2)]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 162, 192));
                ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[5, (a + 2)]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(255, 255, 255));
                ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[5, (a + 2)]).Font.Bold = true;
                ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[5, (a + 2)]).Value2 = hdngs[a].ToUpper();
            }

            if (exprtTyp >= 2)
            {
                DataSet dtst = new DataSet();
                if (exprtTyp == 2)
                {
                    dtst = Global.get_Bdgt_DetBrkdwns(this.budgetID, 0, 1000000000);
                }
                else if (exprtTyp >= 3)
                {
                    dtst = Global.get_Bdgt_DetBrkdwns(this.budgetID, 0, exprtTyp);
                }
                for (int a = 0; a < dtst.Tables[0].Rows.Count; a++)
                {
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 1]).Value2 = a + 1;
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 2]).Value2 = "'" + dtst.Tables[0].Rows[a][14].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 3]).Value2 = dtst.Tables[0].Rows[a][15].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 4]).Value2 = "'" + dtst.Tables[0].Rows[a][3].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 5]).Value2 = dtst.Tables[0].Rows[a][4].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 6]).Value2 = dtst.Tables[0].Rows[a][5].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 7]).Value2 = dtst.Tables[0].Rows[a][6].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 8]).Value2 = dtst.Tables[0].Rows[a][7].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 9]).Value2 = dtst.Tables[0].Rows[a][8].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 10]).Value2 = dtst.Tables[0].Rows[a][9].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 11]).Value2 = dtst.Tables[0].Rows[a][10].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 12]).Value2 = "'" + dtst.Tables[0].Rows[a][12].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[(a + 6), 13]).Value2 = "'" + dtst.Tables[0].Rows[a][13].ToString();
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

        private void importTrnsButton_Click(object sender, EventArgs e)
        {
            if (this.budgetID <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select a Saved Budget First!", 4);
                return;
            }
            if (Global.mnFrm.cmCde.showMsg("Are you sure you want to Import Budget Lines\r\n to Overwrite the existing Field Labels shown here?", 1) == DialogResult.No)
            {
                return;
            }
            this.openFileDialog1.RestoreDirectory = true;
            this.openFileDialog1.Filter = "All Files|*.*|Excel Files|*.xls;*.xlsx";
            this.openFileDialog1.FilterIndex = 2;
            this.openFileDialog1.Title = "Select an Excel File to Upload...";
            this.openFileDialog1.FileName = "";
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.imprttBdgtLinesTmp(this.openFileDialog1.FileName);
            }
            this.populateBdgtBrkDwn(int.Parse(this.accntIDTextBox.Text));
        }

        private void imprttBdgtLinesTmp(string filename)
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
            string accntNum = "";
            string accntDesc = "";
            string itemCode = "";
            string itemDesc = "";
            string bdgtDetType = "";
            string mltplr1 = "";
            string mltplr2 = "";
            string unitAmnt = "";
            string ttlAmnt = "";
            string rmrksJstfctn = "";
            string prdStrtDte = "";
            string prdEndDte = "";
            int rownum = 5;
            char[] w = { '\'' };
            do
            {
                try
                {
                    accntNum = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 2]).Value2.ToString().Trim(w);
                }
                catch (Exception ex)
                {
                    accntNum = "";
                }
                try
                {
                    accntDesc = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 3]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    accntDesc = "";
                }
                try
                {
                    itemCode = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 4]).Value2.ToString().Trim(w);
                }
                catch (Exception ex)
                {
                    itemCode = "";
                }
                try
                {
                    itemDesc = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 5]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    itemDesc = "";
                }
                try
                {
                    bdgtDetType = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 6]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    bdgtDetType = "";
                }
                try
                {
                    mltplr1 = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 7]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    mltplr1 = "";
                }
                try
                {
                    mltplr2 = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 8]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    mltplr2 = "";
                }
                try
                {
                    unitAmnt = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 9]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    unitAmnt = "";
                }
                try
                {
                    ttlAmnt = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 10]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    ttlAmnt = "";
                }
                try
                {
                    rmrksJstfctn = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 11]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    rmrksJstfctn = "";
                }
                try
                {
                    prdStrtDte = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 12]).Value2.ToString().Trim(w);
                }
                catch (Exception ex)
                {
                    prdStrtDte = "";
                }
                try
                {
                    prdEndDte = ((Microsoft.Office.Interop.Excel.Range)Global.mnFrm.cmCde.trgtSheets[0].Cells[rownum, 13]).Value2.ToString().Trim(w);
                }
                catch (Exception ex)
                {
                    prdEndDte = "";
                }
                if (rownum == 5)
                {
                    string[] hdngs = {"Account Number**", "Account Description**", "Budget Item Code**", "Budget Item Description**", "Budget Detail Type**",
                "Multiplier 1 (Qty or No. of Persons)**", "Multiplier 2 (E.g. No. of Days | Type 1 if N/A)**",
                "Unit Amount**", "Total Amount", "Remarks/Justifications", "Period Start Date**", "Period End Date**"};
                    if (accntNum != hdngs[0].ToUpper()
                      || accntDesc != hdngs[1].ToUpper()
                      || itemCode != hdngs[2].ToUpper()
                      || itemDesc != hdngs[3].ToUpper()
                      || bdgtDetType != hdngs[4].ToUpper()
                      || mltplr1 != hdngs[5].ToUpper()
                      || mltplr2 != hdngs[6].ToUpper()
                      || unitAmnt != hdngs[7].ToUpper()
                      || ttlAmnt != hdngs[8].ToUpper()
                      || rmrksJstfctn != hdngs[9].ToUpper()
                      || prdStrtDte != hdngs[10].ToUpper()
                      || prdEndDte != hdngs[11].ToUpper())
                    {
                        Global.mnFrm.cmCde.showMsg("The Excel File you Selected is not a Valid Template\r\nfor importing records here.", 0);
                        return;
                    }
                    rownum++;
                    continue;
                }

                if (accntNum != "" && itemCode != "" && this.budgetID > 0)
                {
                    int accntID = Global.mnFrm.cmCde.getAccntID(accntNum, Global.mnFrm.cmCde.Org_id);
                    int bdgtItmID = (int)Global.mnFrm.cmCde.getInvItmID(itemCode, Global.mnFrm.cmCde.Org_id);
                    double mltplr1d = 0;
                    double.TryParse(mltplr1.Replace(",", ""), out mltplr1d);
                    double mltplr2d = 1;
                    double.TryParse(mltplr2.Replace(",", ""), out mltplr2d);
                    double unitAmntd = 0;
                    double.TryParse(unitAmnt.Replace(",", ""), out unitAmntd);
                    //double tstDte = 0;
                    //bool isdate = double.TryParse(prdStrtDte, out tstDte);
                    bool isdate = true;
                    string prdStrtDteF = prdStrtDte;
                    string prdEndDteF = prdEndDte;
                    try
                    {
                        string strtDte = DateTime.ParseExact(
          prdStrtDte, "dd-MMM-yyyy HH:mm:ss",
          System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                        string endDte = DateTime.ParseExact(
                     prdEndDte, "dd-MMM-yyyy HH:mm:ss",
                     System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch (Exception ex)
                    {
                        isdate = false;
                    }
                    /*Global.mnFrm.cmCde.showSQLNoPermsn("accntID:"+ accntID + ":bdgtItmID:"+ bdgtItmID + 
                        ":mltplr1d:"+ mltplr1d + ":mltplr2d:"+ mltplr2d + ":unitAmntd:"+ unitAmntd + 
                        ":prdStrtDteF:"+ prdStrtDteF + ":prdEndDteF:"+ prdEndDteF);*/
                    /*if (isdate)
                    {
                        prdStrtDteF = DateTime.FromOADate(tstDte).ToString("dd-MMM-yyyy HH:mm:ss");
                    }
                    isdate = double.TryParse(prdEndDte, out tstDte);
                    if (isdate)
                    {
                        prdEndDteF = DateTime.FromOADate(tstDte).ToString("dd-MMM-yyyy HH:mm:ss");
                    }*/

                    string errMsg = "";
                    long bdgtDetID = Global.get_BdgtDetID(prdStrtDte, prdEndDte, this.budgetID, accntID);
                    long oldBdgtBrkDwnID = Global.get_BrkDwnLnID(bdgtDetID, bdgtItmID, accntID);
                    if (oldBdgtBrkDwnID <= 0 && accntID > 0 && bdgtDetID > 0 && bdgtItmID > 0 && isdate == true)
                    {
                        oldBdgtBrkDwnID = Global.getNewBrkDwnLnID();
                        Global.createBdgtBrkDwn(oldBdgtBrkDwnID, accntID, bdgtItmID, bdgtDetType, rmrksJstfctn, mltplr1d, mltplr2d, unitAmntd, bdgtDetID, prdStrtDteF, prdEndDteF);
                        if (oldBdgtBrkDwnID > 0)
                        {
                            Global.updateBdgtDetAmnt(bdgtDetID, accntID);
                        }
                        Global.mnFrm.cmCde.trgtSheets[0].get_Range("A" + rownum + ":M" + rownum + "", Type.Missing).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 255, 0));
                    }
                    else if (oldBdgtBrkDwnID > 0 && accntID > 0 && bdgtDetID > 0 && bdgtItmID > 0 && isdate == true)
                    {
                        Global.updateBdgtBrkDwn(oldBdgtBrkDwnID, accntID, bdgtItmID, bdgtDetType, rmrksJstfctn, mltplr1d, mltplr2d, unitAmntd, bdgtDetID, prdStrtDteF, prdEndDteF);
                        if (oldBdgtBrkDwnID > 0)
                        {
                            Global.updateBdgtDetAmnt(bdgtDetID, accntID);
                        }
                        Global.mnFrm.cmCde.trgtSheets[0].get_Range("A" + rownum + ":M" + rownum + "", Type.Missing).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                    }
                    else
                    {
                        Global.mnFrm.cmCde.trgtSheets[0].get_Range("A" + rownum + ":M" + rownum + "", Type.Missing).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightPink);
                    }
                }
                else
                {
                    Global.mnFrm.cmCde.trgtSheets[0].get_Range("A" + rownum + ":M" + rownum + "", Type.Missing).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                }
                rownum++;
            }
            while (accntNum != "");
            System.Windows.Forms.Application.DoEvents();
        }

        private void shwAllPrdsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.populateBdgtBrkDwn(int.Parse(this.accntIDTextBox.Text));
        }
    }
}

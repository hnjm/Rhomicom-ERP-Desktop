using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonCode
{
    public partial class sgmtValClsfctnsDiag : Form
    {
        public sgmtValClsfctnsDiag()
        {
            InitializeComponent();
        }
        #region "GLOBAL VARIABLES..."
        //Records;
        public long accntID = -1;
        long rec_cur_indx = 0;
        bool is_last_rec = false;
        long totl_rec = 0;
        long last_rec_num = 0;
        public string rec_SQL = "";
        public string recDt_SQL = "";
        bool obey_evnts = false;
        public bool txtChngd = false;
        public bool autoLoad = false;
        public bool isReadOnly = false;
        public bool shdSelOne = false;
        public bool mustSelctSth = false;
        string srchWrd = "%";
        string recClsfctn_SQL = "";
        private string selItemTxt = "";

        bool addRec = false;
        bool editRec = false;
        bool addDtRec = false;
        bool editDtRec = false;
        bool isClosing = false;
        bool addRecsP = false;
        bool editRecsP = false;
        bool delRecsP = false;
        //bool beenToCheckBx = false;
        public CommonCodes cmnCde = new CommonCodes();
        public  string[] dfltPrvldgs = { "View Organization Setup",
  "View Org Details", "View Divisions/Groups", "View Sites/Locations", 
    /*4*/"View Jobs", "View Grades", "View Positions", "View Benefits", 
  /*8*/"View Pay Items", "View Remunerations", "View Working Hours", 
    /*11*/"View Gathering Types", "View SQL", "View Record History",
  /*14*/"Add Org Details","Edit Org Details",
  /*16*/"Add Divisions/Groups","Edit Divisions/Groups","Delete Divisions/Groups",
  /*19*/"Add Sites/Locations","Edit Sites/Locations","Delete Sites/Locations",
  /*22*/"Add Jobs","Edit Jobs","Delete Jobs",
  /*25*/"Add Grades","Edit Grades","Delete Grades",
  /*28*/"Add Positions","Edit Positions","Delete Positions",
  /*31*/"Add Pay Items","Edit Pay Items","Delete Pay Items",
  /*34*/"Add Working Hours","Edit Working Hours","Delete Working Hours",
  /*37*/"Add Gathering Types","Edit Gathering Types","Delete Gathering Types"};
        #endregion
        private void sgmtValClsfctnsDiag_Load(object sender, EventArgs e)
        {
            Color[] clrs = cmnCde.getColors();
            this.BackColor = clrs[0];
            cmnCde.DefaultPrvldgs = this.dfltPrvldgs;

            //this.disableFormButtons();
            this.populateRptClsfctn((int)this.accntID);
            this.accntIDTextBox.Text = this.accntID.ToString();
            this.accntNameTextBox.Text = cmnCde.getSegmentValDesc((int)this.accntID);
            this.accntNumTextBox.Text = cmnCde.getSegmentVal((int)accntID);
        }

        private void disableRptClsfctnEdit()
        {
            this.addRec = false;
            this.editRec = false;
            this.rptCtgrysDataGridView.ReadOnly = true;
            this.rptCtgrysDataGridView.Columns[0].ReadOnly = true;
            this.rptCtgrysDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.Gainsboro;

            this.rptCtgrysDataGridView.Columns[2].ReadOnly = true;
            this.rptCtgrysDataGridView.Columns[2].DefaultCellStyle.BackColor = Color.Gainsboro;
        }

        private void prprForRptClsfctnEdit()
        {
            this.addRec = false;
            this.editRec = true;
            this.rptCtgrysDataGridView.ReadOnly = false;
            this.rptCtgrysDataGridView.Columns[0].ReadOnly = true;
            this.rptCtgrysDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);

            this.rptCtgrysDataGridView.Columns[2].ReadOnly = true;
            this.rptCtgrysDataGridView.Columns[2].DefaultCellStyle.BackColor = Color.White;
        }

        private void populateRptClsfctn(int accntID)
        {
            this.rptCtgrysDataGridView.Rows.Clear();
            if (accntID > 0 && this.addRec == false && this.editRec == false)
            {
                this.disableRptClsfctnEdit();
            }
            this.obey_evnts = false;
            //System.Windows.Forms.Application.DoEvents();
            DataSet dtst = this.get_One_RptClsfctns(accntID);
            this.rptCtgrysDataGridView.Rows.Clear();
            // this.rptCtgrysDataGridView.RowCount = dtst.Tables[0].Rows.Count;
            int rwcnt = dtst.Tables[0].Rows.Count;
            //System.Windows.Forms.Application.DoEvents();

            for (int i = 0; i < rwcnt; i++)
            {
                //System.Windows.Forms.Application.DoEvents();
                this.rptCtgrysDataGridView.RowCount += 1;
                int rowIdx = this.rptCtgrysDataGridView.RowCount - 1;

                this.rptCtgrysDataGridView.Rows[rowIdx].HeaderCell.Value = (i + 1).ToString();
                this.rptCtgrysDataGridView.Rows[rowIdx].Cells[0].Value = dtst.Tables[0].Rows[i][1].ToString();
                this.rptCtgrysDataGridView.Rows[rowIdx].Cells[1].Value = "...";

                this.rptCtgrysDataGridView.Rows[rowIdx].Cells[2].Value = dtst.Tables[0].Rows[i][2].ToString();
                this.rptCtgrysDataGridView.Rows[rowIdx].Cells[3].Value = "...";
                this.rptCtgrysDataGridView.Rows[rowIdx].Cells[4].Value = accntID;
                this.rptCtgrysDataGridView.Rows[rowIdx].Cells[5].Value = dtst.Tables[0].Rows[i][0].ToString();
            }
            this.obey_evnts = true;
            System.Windows.Forms.Application.DoEvents();
            SendKeys.Send("{TAB}");
            SendKeys.Send("{HOME}");
        }

        public void createRptClsfctnRows(int num)
        {
            bool prv = this.obey_evnts;
            this.obey_evnts = false;
            int rowIdx = 0;
            for (int i = 0; i < num; i++)
            {
                this.rptCtgrysDataGridView.RowCount += 1;
                rowIdx = this.rptCtgrysDataGridView.RowCount - 1;
                this.rptCtgrysDataGridView.Rows[rowIdx].HeaderCell.Value = "***";
                this.rptCtgrysDataGridView.Rows[rowIdx].Cells[0].Value = "";
                this.rptCtgrysDataGridView.Rows[rowIdx].Cells[1].Value = "...";
                this.rptCtgrysDataGridView.Rows[rowIdx].Cells[2].Value = "";
                this.rptCtgrysDataGridView.Rows[rowIdx].Cells[3].Value = "...";
                this.rptCtgrysDataGridView.Rows[rowIdx].Cells[4].Value = int.Parse(this.accntIDTextBox.Text); ;
                this.rptCtgrysDataGridView.Rows[rowIdx].Cells[5].Value = "-1";
            }
            this.obey_evnts = prv;
            this.rptCtgrysDataGridView.ClearSelection();
            this.rptCtgrysDataGridView.Focus();
            //System.Windows.Forms.Application.DoEvents();
            this.rptCtgrysDataGridView.CurrentCell = this.rptCtgrysDataGridView.Rows[rowIdx].Cells[0];
            //System.Windows.Forms.Application.DoEvents();
            this.rptCtgrysDataGridView.BeginEdit(true);
            //System.Windows.Forms.Application.DoEvents();
            //SendKeys.Send("{TAB}");
            SendKeys.Send("{HOME}");

            //this.rptCtgrysDataGridView.CurrentCell = this.rptCtgrysDataGridView.Rows[rowIdx].Cells[0];
            //System.Windows.Forms.Application.DoEvents();
            //this.rptCtgrysDataGridView.BeginEdit(true);

        }

        private bool checkRptClsfctnRqrmnts(int rwIdx)
        {
            if (this.rptCtgrysDataGridView.Rows[rwIdx].Cells[0].Value == null)
            {
                return false;
            }
            if (this.rptCtgrysDataGridView.Rows[rwIdx].Cells[0].Value.ToString() == "")
            {
                return false;
            }
            return true;
        }

        private int saveRptClsfctnGridView()
        {
            this.rptCtgrysDataGridView.EndEdit();
            System.Windows.Forms.Application.DoEvents();
            int svd = 0;
            for (int i = 0; i < this.rptCtgrysDataGridView.Rows.Count; i++)
            {
                if (!this.checkRptClsfctnRqrmnts(i))
                {
                    this.rptCtgrysDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 100, 100);
                    continue;
                }
                else
                {
                    //Check if Doc Ln Rec Exists
                    //Create if not else update
                    long lineid = long.Parse(this.rptCtgrysDataGridView.Rows[i].Cells[5].Value.ToString());
                    int accntID = int.Parse(this.accntIDTextBox.Text);
                    string majCatgry = this.rptCtgrysDataGridView.Rows[i].Cells[0].Value.ToString();
                    string minCatgry = this.rptCtgrysDataGridView.Rows[i].Cells[2].Value.ToString();

                    int oldClsfctnID = this.get_RptClsfctnID(majCatgry, minCatgry, accntID);

                    if (oldClsfctnID > 0 && oldClsfctnID != lineid)
                    {
                        cmnCde.showMsg("New Report Classification Name already exists for this Account!", 0);
                        return svd;
                    }
                    if (lineid <= 0)
                    {
                        lineid = this.getNewRptClsfLnID();
                        this.createRptClsfctn(lineid, majCatgry, minCatgry, accntID);
                        this.rptCtgrysDataGridView.Rows[i].Cells[5].Value = lineid;
                    }
                    else
                    {
                        this.updateRptClsfctn(lineid, majCatgry, minCatgry, accntID);
                    }
                    svd++;
                    this.rptCtgrysDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                }
            }
            cmnCde.showMsg(svd + " Classification(s) Saved!", 3);
            //this.populateEvntPrices(int.Parse(this.eventIDTextBox.Text));
            return svd;
        }

        public int get_RptClsfctnID(string majCtgrName, string minCtgrName, int accntID)
        {
            string strSql = @"SELECT account_clsfctn_id from org.org_account_clsfctns where account_id=" + accntID +
              " and lower(maj_rpt_ctgry)='" + majCtgrName.Replace("'", "''").ToLower() +
              "' and lower(min_rpt_ctgry)='" + minCtgrName.Replace("'", "''").ToLower() + "'";

            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            //Global.taxFrm.rec_SQL = strSql;
            return -1;
        }

        public long getNewRptClsfLnID()
        {
            string strSql = "select nextval('org.org_account_clsfctns_account_clsfctn_id_seq')";
            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public void createRptClsfctn(long clsfctnID, string majCtgrName, string minCtgrName, int accntID)
        {
            string dateStr = cmnCde.getDB_Date_time();
            string insSQL = @"INSERT INTO org.org_account_clsfctns(
            account_clsfctn_id, account_id, maj_rpt_ctgry, min_rpt_ctgry, 
            created_by, creation_date, last_update_by, last_update_date) " +
                  "VALUES (" + clsfctnID + ", " + accntID + ", '" + majCtgrName.Replace("'", "''") +
                  "', '" + minCtgrName.Replace("'", "''") +
                  "', " + cmnCde.User_id + ", '" + dateStr +
                  "', " + cmnCde.User_id + ", '" + dateStr +
                  "')";
            cmnCde.insertDataNoParams(insSQL);
        }

        public void updateRptClsfctn(long clsfctnID, string majCtgrName, string minCtgrName, int accntID)
        {
            cmnCde.Extra_Adt_Trl_Info = "";
            string dateStr = cmnCde.getDB_Date_time();
            string updtSQL = "UPDATE org.org_account_clsfctns SET " +
                  "maj_rpt_ctgry='" + majCtgrName.Replace("'", "''") +
                  "', min_rpt_ctgry='" + minCtgrName.Replace("'", "''") +
                  "',account_id=" + accntID +
                  ", last_update_by = " + cmnCde.User_id + ", " +
                  "last_update_date = '" + dateStr +
                  "' WHERE (account_clsfctn_id =" + clsfctnID + ")";
            cmnCde.updateDataNoParams(updtSQL);
        }

        public void deleteRptClsfctn(long lnID)
        {
            cmnCde.Extra_Adt_Trl_Info = "";
            string delSQL = "DELETE FROM org.org_account_clsfctns WHERE account_clsfctn_id = " +
              lnID + "";
            cmnCde.deleteDataNoParams(delSQL);
        }

        public DataSet get_One_RptClsfctns(int accntid)
        {
            string strSql = @"SELECT account_clsfctn_id, maj_rpt_ctgry, min_rpt_ctgry, 
       created_by, creation_date, last_update_by, last_update_date
  FROM org.org_account_clsfctns a WHERE(a.account_id = " + accntid + ") ORDER BY 1";

            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            this.recClsfctn_SQL = strSql;
            return dtst;
        }

        private void dfltFill1(int idx)
        {
            if (this.rptCtgrysDataGridView.Rows[idx].Cells[0].Value == null)
            {
                this.rptCtgrysDataGridView.Rows[idx].Cells[0].Value = string.Empty;
            }
            if (this.rptCtgrysDataGridView.Rows[idx].Cells[2].Value == null)
            {
                this.rptCtgrysDataGridView.Rows[idx].Cells[2].Value = string.Empty;
            }
            if (this.rptCtgrysDataGridView.Rows[idx].Cells[4].Value == null)
            {
                this.rptCtgrysDataGridView.Rows[idx].Cells[4].Value = "-1";
            }
            if (this.rptCtgrysDataGridView.Rows[idx].Cells[5].Value == null)
            {
                this.rptCtgrysDataGridView.Rows[idx].Cells[5].Value = "-1";
            }
        }

        private void rptCtgrysDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

            if (e.ColumnIndex == 1)
            {
                if (this.addRec == false && this.editRec == false)
                {
                    cmnCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                    this.obey_evnts = true;
                    return;
                }
                int[] selVals = new int[1];
                int lovID = cmnCde.getLovID("Account Classifications");
                selVals[0] = cmnCde.getPssblValID(this.rptCtgrysDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString(), lovID);
                this.srchWrd = "%";
                DialogResult dgRes = cmnCde.showPssblValDiag(
                    lovID, ref selVals,
                    true, false, this.srchWrd, "Both", true);
                if (dgRes == DialogResult.OK)
                {
                    for (int i = 0; i < selVals.Length; i++)
                    {
                        this.rptCtgrysDataGridView.Rows[e.RowIndex].Cells[0].Value = cmnCde.getPssblValNm(
                          selVals[i]);
                    }
                }
            }
            else if (e.ColumnIndex == 3)
            {
                if (this.addRec == false && this.editRec == false)
                {
                    cmnCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                    this.obey_evnts = true;
                    return;
                }
                int[] selVals = new int[1];
                int lovID = cmnCde.getLovID("Account Classifications");
                selVals[0] = cmnCde.getPssblValID(this.rptCtgrysDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString(), lovID);
                this.srchWrd = "%";
                DialogResult dgRes = cmnCde.showPssblValDiag(
                    lovID, ref selVals,
                    true, false, this.srchWrd, "Both", true);
                if (dgRes == DialogResult.OK)
                {
                    for (int i = 0; i < selVals.Length; i++)
                    {
                        this.rptCtgrysDataGridView.Rows[e.RowIndex].Cells[2].Value = cmnCde.getPssblValNm(
                          selVals[i]);
                    }
                }
            }
            this.obey_evnts = true;
        }

        private void refreshRptCtgryButton_Click(object sender, EventArgs e)
        {
            if (int.Parse(this.accntIDTextBox.Text) <= 0)
            {
                cmnCde.showMsg("Please select a Record First!", 0);
                return;
            }
            this.populateRptClsfctn(int.Parse(this.accntIDTextBox.Text));
        }

        private void addRptCtgryButton_Click(object sender, EventArgs e)
        {
            if (cmnCde.test_prmssns(this.dfltPrvldgs[15]) == false)
            {
                cmnCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }
            if (this.accntIDTextBox.Text == "" || this.accntIDTextBox.Text == "-1")
            {
                cmnCde.showMsg("Please select an Account First!", 0);
                return;
            }
            this.createRptClsfctnRows(1);
            this.prprForRptClsfctnEdit();
        }

        private void delRptCtgryButton_Click(object sender, EventArgs e)
        {
            if (cmnCde.test_prmssns(this.dfltPrvldgs[15]) == false)
            {
                cmnCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }
            if (this.rptCtgrysDataGridView.CurrentCell != null
                && this.rptCtgrysDataGridView.SelectedRows.Count <= 0)
            {
                this.rptCtgrysDataGridView.Rows[this.rptCtgrysDataGridView.CurrentCell.RowIndex].Selected = true;
            }
            if (this.rptCtgrysDataGridView.SelectedRows.Count <= 0)
            {
                cmnCde.showMsg("Please select the record to Delete!", 0);
                return;
            }
            if (cmnCde.showMsg("Are you sure you want to DELETE the selected Item?" +
       "\r\nThis action cannot be undone!", 1) == DialogResult.No)
            {
                //cmnCde.showMsg("Operation Cancelled!", 4);
                return;
            }
            bool prv = this.obey_evnts;
            this.obey_evnts = false;
            for (int i = 0; i < this.rptCtgrysDataGridView.SelectedRows.Count;)
            {
                long lnID = -1;
                long.TryParse(this.rptCtgrysDataGridView.SelectedRows[0].Cells[5].Value.ToString(), out lnID);
                if (lnID > 0)
                {
                    this.deleteRptClsfctn(lnID);
                }
                this.rptCtgrysDataGridView.Rows.RemoveAt(this.rptCtgrysDataGridView.SelectedRows[0].Index);
            }
            this.obey_evnts = prv;
        }

        private void vwSQLRptCtgryButton_Click(object sender, EventArgs e)
        {
            cmnCde.showSQL(this.recClsfctn_SQL, 10);
        }

        private void rcHstryRptCtgryButton_Click(object sender, EventArgs e)
        {
            if (this.rptCtgrysDataGridView.CurrentCell != null
       && this.rptCtgrysDataGridView.SelectedRows.Count <= 0)
            {
                this.rptCtgrysDataGridView.Rows[this.rptCtgrysDataGridView.CurrentCell.RowIndex].Selected = true;
            }
            if (this.rptCtgrysDataGridView.SelectedRows.Count <= 0)
            {
                cmnCde.showMsg("Please select a Record First!", 0);
                return;
            }
            cmnCde.showRecHstry(
              cmnCde.get_Gnrl_Rec_Hstry(
              long.Parse(this.rptCtgrysDataGridView.SelectedRows[0].Cells[5].Value.ToString()),
              "org.org_account_clsfctns", "account_clsfctn_id"), 9);
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
            int sgmntID = cmnCde.getSegmentID("NaturalAccount", cmnCde.Org_id);
            DialogResult dgRes = cmnCde.showPssblValDiag(
              cmnCde.getLovID("Account Segment Values"),
              ref selVals, true, true, sgmntID,
              this.srchWrd, "Both", true);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    accntID = int.Parse(selVals[i]);
                    this.accntIDTextBox.Text = accntID.ToString();
                    this.accntNameTextBox.Text = cmnCde.getSegmentValDesc(accntID);
                    this.accntNumTextBox.Text = cmnCde.getSegmentVal(accntID);
                    this.populateRptClsfctn(accntID);
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            int svd = this.saveRptClsfctnGridView();
            if (svd == this.rptCtgrysDataGridView.Rows.Count)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

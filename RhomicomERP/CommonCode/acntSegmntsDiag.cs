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
    public partial class acntSegmntsDiag : Form
    {
        public acntSegmntsDiag()
        {
            InitializeComponent();
        }
        public int accountID = -1;
        public bool canEdit = false;
        public bool obey_evnts = false;
        public bool isForRpt = false;
        public string nwAcctNum = "";
        public string nwAcctName = "";
        public int accntSgmnt1 = -1;
        public int accntSgmnt2 = -1;
        public int accntSgmnt3 = -1;
        public int accntSgmnt4 = -1;
        public int accntSgmnt5 = -1;
        public int accntSgmnt6 = -1;
        public int accntSgmnt7 = -1;
        public int accntSgmnt8 = -1;
        public int accntSgmnt9 = -1;
        public int accntSgmnt10 = -1;
        public int ntrlAcntSgmtNum = -1;
        public int ntrlAcntSgmtVal = -1;
        public int crncySgmtNum = -1;
        public int crncySgmtID = -1;
        public bool allwNtrlAcntEdit = false;
        public CommonCodes cmnCde = new CommonCodes();
        private void acntSegmntsDiag_Load(object sender, EventArgs e)
        {
            this.obey_evnts = false;
            Color[] clrs = cmnCde.getColors();
            this.BackColor = clrs[0];
            this.disableLnsEdit();
            this.populateSegments();
            this.obey_evnts = true;
        }

        private void disableLnsEdit()
        {
            this.accntSgmntsDataGridView.DefaultCellStyle.ForeColor = Color.Black;
            this.accntSgmntsDataGridView.Columns[0].ReadOnly = true;
            this.accntSgmntsDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.accntSgmntsDataGridView.Columns[1].ReadOnly = true;
            this.accntSgmntsDataGridView.Columns[1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.accntSgmntsDataGridView.Columns[2].ReadOnly = true;
            this.accntSgmntsDataGridView.Columns[2].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.accntSgmntsDataGridView.Columns[3].ReadOnly = true;
            this.accntSgmntsDataGridView.Columns[3].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.accntSgmntsDataGridView.Columns[8].ReadOnly = true;
            this.accntSgmntsDataGridView.Columns[8].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            if (canEdit == false)
            {
                this.accntSgmntsDataGridView.Columns[6].ReadOnly = true;
                this.accntSgmntsDataGridView.Columns[6].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                //this.accntSgmntsDataGridView.Columns[12].Visible = false;
            }
            else
            {
                this.accntSgmntsDataGridView.Columns[6].ReadOnly = true;
                this.accntSgmntsDataGridView.Columns[6].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
                // this.accntSgmntsDataGridView.Columns[12].Visible = true;
            }
        }

        private void populateSegments()
        {
            this.obey_evnts = false;
            this.accntSgmntsDataGridView.Rows.Clear();
            int rwcnt = this.get_SegmnetsTtl(cmnCde.Org_id);
            this.accntSgmntsDataGridView.ForeColor = Color.Black;
            this.accntSgmntsDataGridView.DefaultCellStyle.ForeColor = Color.Black;
            for (int i = 0; i < rwcnt; i++)
            {
                this.accntSgmntsDataGridView.RowCount += 1;
                int rowIdx = this.accntSgmntsDataGridView.RowCount - 1;
                DataSet dtst = this.get_One_SegmentDet((i + 1), cmnCde.Org_id);
                this.accntSgmntsDataGridView.Rows[rowIdx].HeaderCell.Value = (i + 1).ToString();
                this.accntSgmntsDataGridView.Rows[rowIdx].Cells[0].Value = (i + 1).ToString();
                if (dtst.Tables[0].Rows.Count > 0)
                {
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[1].Value = dtst.Tables[0].Rows[0][1].ToString();
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[2].Value = (dtst.Tables[0].Rows[0][2].ToString() == "NaturalAccount") ? true : false;
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[3].Value = dtst.Tables[0].Rows[0][2].ToString();
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[4].Value = "Attached Values";
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[5].Value = dtst.Tables[0].Rows[0][0].ToString();
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[7].Value = "...";
                    int segNum = i + 1;
                    int segVID = -1;
                    switch (segNum)
                    {
                        case 1:
                            segVID = this.accntSgmnt1;
                            break;
                        case 2:
                            segVID = this.accntSgmnt2;
                            break;
                        case 3:
                            segVID = this.accntSgmnt3;
                            break;
                        case 4:
                            segVID = this.accntSgmnt4;
                            break;
                        case 5:
                            segVID = this.accntSgmnt5;
                            break;
                        case 6:
                            segVID = this.accntSgmnt6;
                            break;
                        case 7:
                            segVID = this.accntSgmnt7;
                            break;
                        case 8:
                            segVID = this.accntSgmnt8;
                            break;
                        case 9:
                            segVID = this.accntSgmnt9;
                            break;
                        case 10:
                            segVID = this.accntSgmnt10;
                            break;
                    }

                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[6].Value = cmnCde.getSegmentVal(segVID);
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[8].Value = cmnCde.getSegmentValDesc(segVID);
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[9].Value = segVID;
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[10].Value = dtst.Tables[0].Rows[0][3].ToString();
                    //MessageBox.Show(dtst.Tables[0].Rows[0][3].ToString());
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[11].Value = cmnCde.getSegmentDpndntValID(segVID);
                    //this.accntSgmntsDataGridView.Rows[rowIdx].Cells[12].Value = "New Value";

                    string sysClsf = dtst.Tables[0].Rows[0][2].ToString();
                    if (sysClsf == "NaturalAccount")
                    {
                        this.ntrlAcntSgmtNum = segNum;
                        this.ntrlAcntSgmtVal = segVID;
                    }
                    if (sysClsf == "Currency")
                    {
                        this.crncySgmtNum = segNum;
                        this.crncySgmtID = segVID;
                    }
                }
                else
                {
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[1].Value = "";
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[2].Value = false;
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[3].Value = "Other";
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[4].Value = "Attached Values";
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[5].Value = -1;
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[6].Value = "";
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[7].Value = "...";
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[8].Value = "";
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[9].Value = "-1";
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[10].Value = "-1";
                    this.accntSgmntsDataGridView.Rows[rowIdx].Cells[11].Value = "-1";
                    //this.accntSgmntsDataGridView.Rows[rowIdx].Cells[12].Value = "New Value";
                }
            }

            System.Windows.Forms.Application.DoEvents();
            this.obey_evnts = true;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.nwAcctNum = "";
            this.nwAcctName = "";
            string dlmtr = this.get_SegmnetsDlmtr(cmnCde.Org_id);
            string dlmtr1 = "";
            switch (dlmtr)
            {
                case "Period (.)":
                    dlmtr = ".";
                    dlmtr1 = dlmtr;
                    break;
                case "hiphen(-)":
                    dlmtr = "-";
                    dlmtr1 = dlmtr;
                    break;
                case "Space ( )":
                    dlmtr = " ";
                    dlmtr1 = " ";
                    break;
                case "None":
                    dlmtr = "";
                    dlmtr1 = " ";
                    break;
            }
            int cntr = this.accntSgmntsDataGridView.Rows.Count;
            for (int i = 0; i < cntr; i++)
            {
                if (i == 0)
                {
                    this.nwAcctNum = this.accntSgmntsDataGridView.Rows[i].Cells[6].Value.ToString();
                    this.nwAcctName = this.accntSgmntsDataGridView.Rows[i].Cells[8].Value.ToString();
                }
                else
                {
                    this.nwAcctNum = this.nwAcctNum + dlmtr + this.accntSgmntsDataGridView.Rows[i].Cells[6].Value.ToString();
                    this.nwAcctName = this.nwAcctName + dlmtr1 + this.accntSgmntsDataGridView.Rows[i].Cells[8].Value.ToString();
                }
                int segNum = int.Parse(this.accntSgmntsDataGridView.Rows[i].Cells[0].Value.ToString());
                int segVID = int.Parse(this.accntSgmntsDataGridView.Rows[i].Cells[9].Value.ToString());
                /*if (segVID <= 0 && this.isForRpt == false)
                {
                    cmnCde.showMsg("Segment Value Cannot be Empty!", 0);
                    return;
                }*/
                switch (segNum)
                {
                    case 1:
                        this.accntSgmnt1 = segVID;
                        break;
                    case 2:
                        this.accntSgmnt2 = segVID;
                        break;
                    case 3:
                        this.accntSgmnt3 = segVID;
                        break;
                    case 4:
                        this.accntSgmnt4 = segVID;
                        break;
                    case 5:
                        this.accntSgmnt5 = segVID;
                        break;
                    case 6:
                        this.accntSgmnt6 = segVID;
                        break;
                    case 7:
                        this.accntSgmnt7 = segVID;
                        break;
                    case 8:
                        this.accntSgmnt8 = segVID;
                        break;
                    case 9:
                        this.accntSgmnt9 = segVID;
                        break;
                    case 10:
                        this.accntSgmnt10 = segVID;
                        break;
                }
                string sysClsf = this.accntSgmntsDataGridView.Rows[i].Cells[3].Value.ToString();
                if (sysClsf == "NaturalAccount")
                {
                    this.ntrlAcntSgmtNum = segNum;
                    this.ntrlAcntSgmtVal = segVID;
                }
                if (sysClsf == "Currency")
                {
                    this.crncySgmtNum = segNum;
                    this.crncySgmtID = segVID;
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void accntSgmntsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
            if (this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[0].Value == null)
            {
                this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[0].Value = "-1";
            }
            if (this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[1].Value == null)
            {
                this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[1].Value = "";
            }
            if (this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[2].Value == null)
            {
                this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[2].Value = false;
            }
            if (this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[3].Value == null)
            {
                this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[3].Value = "Other";
            }
            if (this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[5].Value == null)
            {
                this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[5].Value = -1;
            }
            if (this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[6].Value == null)
            {
                this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[6].Value = "";
            }
            if (this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[8].Value == null)
            {
                this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[8].Value = "";
            }
            if (this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[9].Value == null)
            {
                this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[9].Value = -1;
            }
            if (this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[10].Value == null)
            {
                this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[10].Value = -1;
            }
            if (this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[11].Value == null)
            {
                this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[11].Value = -1;
            }
            if (e.ColumnIndex == 7)
            {
                string[] selVals = new string[1];
                int segmentID = int.Parse(this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[5].Value.ToString());
                string srchWrd = this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[6].Value.ToString();
                string sysClsf = this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
                /*if (this.allwNtrlAcntEdit == false && sysClsf == "NaturalAccount" && this.isForRpt == false)
                {
                    cmnCde.showMsg("This segment cannot be EDITED for Accounts with posted transactions!", 0);
                    this.accntSgmntsDataGridView.EndEdit();
                    this.obey_evnts = true;
                    return;
                }
                else if (this.allwNtrlAcntEdit == false && sysClsf == "Currency" && this.isForRpt == false)
                {
                    cmnCde.showMsg("This segment cannot be EDITED for Accounts with posted transactions!", 0);
                    this.accntSgmntsDataGridView.EndEdit();
                    this.obey_evnts = true;
                    return;
                }*/
                selVals[0] = this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[9].Value.ToString();
                int dpndntSegID = int.Parse(this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[10].Value.ToString());
                int dpnSgValID = this.getSgmntsChsnDpndntValID(dpndntSegID);

                if (this.isForRpt)
                {
                    DialogResult dgRes = cmnCde.showPssblValDiag(cmnCde.getLovID("Account Segment Values"), ref selVals, true, false, segmentID, "1", dpnSgValID.ToString(),
                     srchWrd, "Both", false);
                    if (dgRes == DialogResult.OK)
                    {
                        for (int i = 0; i < selVals.Length; i++)
                        {
                            int sgmntValID = this.getSgmntValID(selVals[i], segmentID);
                            this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[9].Value = sgmntValID.ToString();
                            this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[6].Value = cmnCde.getSegmentVal(sgmntValID);
                            this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[8].Value = cmnCde.getSegmentValDesc(sgmntValID);
                            this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[11].Value = cmnCde.getSegmentDpndntValID(sgmntValID);
                        }
                    }
                }
                else
                {
                    int sgmntValID = int.Parse(this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[9].Value.ToString());
                    int nD_segmentNum = cmnCde.getSegmentNum(segmentID, cmnCde.Org_id);
                    string nD_sysClsfctn = cmnCde.getSegmentClsfctn(segmentID, cmnCde.Org_id);
                    string srchin = "All Fields";
                    if (dpnSgValID > 0)
                    {
                        string dpnSgVal = this.getSgmntsChsnDpndntValue(dpndntSegID);
                        srchin = "Dependent Value";
                        srchWrd = dpnSgVal;
                    }
                    DialogResult dgrs = cmnCde.showSgmntValuesDiag(ref sgmntValID, segmentID, nD_segmentNum, dpndntSegID, nD_sysClsfctn, this.cmnCde.Org_id,
            true, false, srchWrd, srchin, false, this.cmnCde);
                    if (dgrs == DialogResult.OK)
                    {
                        this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[9].Value = sgmntValID.ToString();
                        this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[6].Value = cmnCde.getSegmentVal(sgmntValID);
                        this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[8].Value = cmnCde.getSegmentValDesc(sgmntValID);
                        this.accntSgmntsDataGridView.Rows[e.RowIndex].Cells[11].Value = cmnCde.getSegmentDpndntValID(sgmntValID);
                    }
                }
            }
            this.accntSgmntsDataGridView.EndEdit();
            this.obey_evnts = true;
        }

        private int getSgmntsChsnDpndntValID(int dpndntSgmntID)
        {
            int cntr = this.accntSgmntsDataGridView.Rows.Count;
            for (int i = 0; i < cntr; i++)
            {
                int segNum = int.Parse(this.accntSgmntsDataGridView.Rows[i].Cells[0].Value.ToString());
                int segID = int.Parse(this.accntSgmntsDataGridView.Rows[i].Cells[5].Value.ToString());
                int segVID = int.Parse(this.accntSgmntsDataGridView.Rows[i].Cells[9].Value.ToString());
                if (segID == dpndntSgmntID)
                {
                    //cmnCde.showMsg("Segment Value Cannot be Empty!", 0);
                    return segVID;
                }
            }
            return -1;
        }

        private string getSgmntsChsnDpndntValue(int dpndntSgmntID)
        {
            int cntr = this.accntSgmntsDataGridView.Rows.Count;
            for (int i = 0; i < cntr; i++)
            {
                int segNum = int.Parse(this.accntSgmntsDataGridView.Rows[i].Cells[0].Value.ToString());
                int segID = int.Parse(this.accntSgmntsDataGridView.Rows[i].Cells[5].Value.ToString());
                string segVal = this.accntSgmntsDataGridView.Rows[i].Cells[6].Value.ToString();
                if (segID == dpndntSgmntID)
                {
                    return segVal;
                }
            }
            return "";
        }

        public DataSet get_One_SegmentDet(int segNum, int orgid)
        {
            string strSql = "";
            strSql = @"SELECT a.segment_id, a.segment_name_prompt, a.system_clsfctn, org.get_sgmnt_id(a.prnt_sgmnt_number)  
        FROM org.org_acnt_sgmnts a WHERE((a.org_id = " + orgid + " and a.segment_number = " + segNum + "))";
            //Global.mnFrm.orgDet_SQL = strSql;
            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            return dtst;
        }

        public DataSet get_One_SegmentAcntVal(int segNum, int accountID)
        {
            string strSql = "";
            strSql = @"SELECT a.accnt_seg" + segNum + @"_val_id, org.get_sgmnt_val(a.accnt_seg" + segNum +
                @"_val_id), org.get_sgmnt_val_desc(a.accnt_seg" + segNum + @"_val_id)  
        FROM accb.accb_chart_of_accnts a  WHERE((a.accnt_id = " + accountID + "))";
            //Global.mnFrm.orgDet_SQL = strSql;
            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            return dtst;
        }

        public int get_SegmnetsTtl(long orgid)
        {
            string strSql = @"SELECT no_of_accnt_sgmnts FROM org.org_details a  " +
             " WHERE((a.org_id = " + orgid + "))";

            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public DataSet get_One_SgmntValDet(int segmentValID)
        {
            string strSql = @"SELECT a.segment_value_id, a.segment_id, a.segment_value, a.segment_description, 
       a.allwd_group_type, a.allwd_group_value, a.is_enabled, a.prnt_segment_value_id, 
       a.created_by, a.creation_date, a.last_update_by, a.last_update_date, 
       a.org_id, a.is_contra, a.accnt_type, a.is_prnt_accnt, a.is_retained_earnings, 
       a.is_net_income, a.accnt_typ_id, a.report_line_no, a.has_sub_ledgers, 
       a.control_account_id, a.crncy_id, a.is_suspens_accnt, a.account_clsfctn, 
       a.mapped_grp_accnt_id, b.segment_number
  FROM org.org_segment_values a, org.org_acnt_sgmnts b " +
             "WHERE(a.segment_id = b.segment_id and a.segment_value_id = " + segmentValID + ")";

            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            //Global.taxFrm.rec_SQL = strSql;
            return dtst;
        }

        public string get_SegmnetsDlmtr(long orgid)
        {
            string strSql = @"SELECT segment_delimiter FROM org.org_details a  " +
             " WHERE((a.org_id = " + orgid + "))";

            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            return "";
        }

        public int getSgmntValID(string segmentVal, int segmentID)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "select segment_value_id from org.org_segment_values where lower(segment_value) = '" +
             segmentVal.Replace("'", "''").ToLower() + "' and segment_id = " + segmentID;
            dtSt = cmnCde.selectDataNoParams(sqlStr);
            if (dtSt.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtSt.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }
    }
}

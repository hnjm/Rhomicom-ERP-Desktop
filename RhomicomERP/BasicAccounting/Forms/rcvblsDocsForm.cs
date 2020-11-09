﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Accounting.Classes;
using Accounting.Dialogs;

namespace Accounting.Forms
{
    public partial class rcvblsDocsForm : Form
    {
        #region "GLOBAL VARIABLES..."
        //Records;
        long rec_cur_indx = 0;
        bool is_last_rec = false;
        long totl_rec = 0;
        long last_rec_num = 0;
        public string rec_SQL = "";
        public string recDt_SQL = "";
        public string smmry_SQL = "";
        public string docTmplt_SQL = "";

        public bool txtChngd = false;
        bool obey_evnts = false;
        bool autoLoad = false;

        bool addRec = false;
        bool editRec = false;

        bool vwRecsCSP = false;
        bool addRecsCSP = false;
        bool editRecsCSP = false;
        bool delRecsCSP = false;

        bool vwRecsCAP = false;
        bool addRecsCAP = false;
        bool editRecsCAP = false;
        bool delRecsCAP = false;

        bool vwRecsDRTC = false;
        bool addRecsDRTC = false;
        bool editRecsDRTC = false;
        bool delRecsDRTC = false;

        bool vwRecsCCMIT = false;
        bool addRecsCCMIT = false;
        bool editRecsCCMIT = false;
        bool delRecsCCMIT = false;

        bool vwRecsDTFC = false;
        bool addRecsDTFC = false;
        bool editRecsDTFC = false;
        bool delRecsDTFC = false;

        bool vwRecsCDMIR = false;
        bool addRecsCDMIR = false;
        bool editRecsCDMIR = false;
        bool delRecsCDMIR = false;

        bool rvwApprvDocs = false;
        bool payDocs = false;
        //bool beenToCheckBx = false;

        public int curid = -1;
        public string curCode = "";

        #endregion

        #region "FORM EVENTS..."
        public rcvblsDocsForm()
        {
            InitializeComponent();
        }

        private void rcvblsDocsForm_Load(object sender, EventArgs e)
        {
            Color[] clrs = Global.mnFrm.cmCde.getColors();
            this.BackColor = clrs[0];
            //this.glsLabel3.TopFill = clrs[0];
            //this.glsLabel3.BottomFill = clrs[1];
            this.curid = Global.mnFrm.cmCde.getOrgFuncCurID(Global.mnFrm.cmCde.Org_id);
            this.curCode = Global.mnFrm.cmCde.getPssblValNm(this.curid);
            //this.timer1.Interval = 100;
            //this.timer1.Enabled = true;
            Global.mnFrm.cmCde.updateDataNoParams(@"UPDATE accb.accb_rcvbl_amnt_smmrys
                                                   SET line_qty=1 WHERE line_qty=0");
            Global.mnFrm.cmCde.updateDataNoParams(@"UPDATE accb.accb_rcvbl_amnt_smmrys
                                                   SET unit_price=(rcvbl_smmry_amnt/line_qty)
                                                 WHERE unit_price=0");
        }

        public void loadPrvldgs()
        {
            bool vwSQL = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[10]);
            bool rcHstry = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[9]);

            this.vwRecsCSP = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[32]);
            this.addRecsCSP = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[73]);
            this.editRecsCSP = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[74]);
            this.delRecsCSP = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[75]);

            this.vwRecsCAP = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[32]);
            this.addRecsCAP = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[76]);
            this.editRecsCAP = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[77]);
            this.delRecsCAP = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[78]);

            this.vwRecsDRTC = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[32]);
            this.addRecsDRTC = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[79]);
            this.editRecsDRTC = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[80]);
            this.delRecsDRTC = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[81]);

            this.vwRecsCCMIT = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[32]);
            this.addRecsCCMIT = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[82]);
            this.editRecsCCMIT = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[83]);
            this.delRecsCCMIT = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[84]);

            this.vwRecsDTFC = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[32]);
            this.addRecsDTFC = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[85]);
            this.editRecsDTFC = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[86]);
            this.delRecsDTFC = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[87]);

            this.vwRecsCDMIR = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[32]);
            this.addRecsCDMIR = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[88]);
            this.editRecsCDMIR = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[89]);
            this.delRecsCDMIR = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[90]);

            this.rvwApprvDocs = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[54]);
            this.payDocs = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[72]);
            this.vwSQLButton.Enabled = vwSQL;
            this.rcHstryButton.Enabled = rcHstry;
            this.vwSmrySQLButton.Enabled = vwSQL;
            this.rcHstrySmryButton.Enabled = rcHstry;
        }

        public void disableFormButtons()
        {
            this.saveButton.Enabled = false;
            if (this.docTypeComboBox.Text == "Customer Standard Payment")
            {
                this.addButton.Enabled = this.addRecsCSP;
                this.editButton.Enabled = this.editRecsCSP;
                this.delButton.Enabled = this.delRecsCSP;
                this.addLineButton.Enabled = this.editRecsCSP;
                this.delLineButton.Enabled = this.editRecsCSP;
                this.addTaxButton.Enabled = this.editRecsCSP;
                this.addDscntButton.Enabled = this.editRecsCSP;
                this.addChrgButton.Enabled = this.editRecsCSP;
                this.applyPrpymntButton.Enabled = editRecsCSP;
            }
            else if (this.docTypeComboBox.Text == "Customer Advance Payment")
            {
                this.addButton.Enabled = this.addRecsCAP;
                this.editButton.Enabled = this.editRecsCAP;
                this.delButton.Enabled = this.delRecsCAP;
                this.addLineButton.Enabled = this.editRecsCAP;
                this.delLineButton.Enabled = this.editRecsCAP;
                this.addTaxButton.Enabled = false;
                this.addDscntButton.Enabled = false;
                this.addChrgButton.Enabled = false;
                this.applyPrpymntButton.Enabled = false;
            }
            else if (this.docTypeComboBox.Text == "Direct Refund to Customer")
            {
                this.addButton.Enabled = this.addRecsDRTC;
                this.editButton.Enabled = this.editRecsDRTC;
                this.delButton.Enabled = this.delRecsDRTC;
                this.addLineButton.Enabled = false;
                this.delLineButton.Enabled = false;
                this.addTaxButton.Enabled = false;
                this.addDscntButton.Enabled = false;
                this.addChrgButton.Enabled = false;
                this.applyPrpymntButton.Enabled = editRecsDRTC;
            }
            else if (this.docTypeComboBox.Text == "Customer Credit Memo (InDirect Topup)")
            {
                this.addButton.Enabled = this.addRecsCCMIT;
                this.editButton.Enabled = this.editRecsCCMIT;
                this.delButton.Enabled = this.delRecsCCMIT;
                this.addLineButton.Enabled = false;
                this.delLineButton.Enabled = false;
                this.addTaxButton.Enabled = false;
                this.addDscntButton.Enabled = false;
                this.addChrgButton.Enabled = false;
                this.applyPrpymntButton.Enabled = false;
            }
            else if (this.docTypeComboBox.Text == "Direct Topup from Customer")
            {
                this.addButton.Enabled = this.addRecsDTFC;
                this.editButton.Enabled = this.editRecsDTFC;
                this.delButton.Enabled = this.delRecsDTFC;
                this.addLineButton.Enabled = false;
                this.delLineButton.Enabled = false;
                this.addTaxButton.Enabled = false;
                this.addDscntButton.Enabled = false;
                this.addChrgButton.Enabled = false;
                this.applyPrpymntButton.Enabled = editRecsDTFC;
            }
            else if (this.docTypeComboBox.Text == "Customer Debit Memo (InDirect Refund)")
            {
                this.addButton.Enabled = this.addRecsCDMIR;
                this.editButton.Enabled = this.editRecsCDMIR;
                this.delButton.Enabled = this.delRecsCDMIR;
                this.applyPrpymntButton.Enabled = false;
                this.addLineButton.Enabled = false;
                this.delLineButton.Enabled = false;
                this.addTaxButton.Enabled = false;
                this.addDscntButton.Enabled = false;
                this.addChrgButton.Enabled = false;
            }
            else
            {
                this.addButton.Enabled = this.addRecsCSP || this.addRecsCAP || this.addRecsCCMIT || this.addRecsCDMIR || this.addRecsDRTC || this.addRecsDTFC;
                this.editButton.Enabled = this.editRecsCSP || this.editRecsCAP || this.editRecsCCMIT || this.editRecsCDMIR || this.editRecsDRTC || this.editRecsDTFC;
                this.delButton.Enabled = this.delRecsCSP || this.delRecsCAP || this.delRecsCCMIT || this.delRecsCDMIR || this.delRecsDRTC || this.delRecsDTFC;
                this.addLineButton.Enabled = this.addButton.Enabled;
                this.delLineButton.Enabled = this.editButton.Enabled;
                this.addTaxButton.Enabled = false;
                this.addDscntButton.Enabled = false;
                this.addChrgButton.Enabled = false;
                this.applyPrpymntButton.Enabled = false;
            }
        }

        #endregion

        #region "RECEIVABLES DOCUMENTS..."
        public void loadPanel()
        {
            //this.saveLabel.Visible = false;
            this.obey_evnts = false;
            if (this.searchInComboBox.SelectedIndex < 0)
            {
                this.searchInComboBox.SelectedIndex = 3;
            }
            if (searchForTextBox.Text.Contains("%") == false)
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
            DataSet dtst = Global.get_RcvblsDocHdr(this.searchForTextBox.Text,
              this.searchInComboBox.Text, this.rec_cur_indx,
              int.Parse(this.dsplySizeComboBox.Text), Global.mnFrm.cmCde.Org_id,
              this.showUnpaidCheckBox.Checked);
            this.rcvblsDocListView.Items.Clear();

            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                this.last_rec_num = Global.mnFrm.cmCde.navFuncts.startIndex() + i;
                ListViewItem nwItem = new ListViewItem(new string[] {
    (Global.mnFrm.cmCde.navFuncts.startIndex() + i).ToString(),
    dtst.Tables[0].Rows[i][1].ToString(),
    dtst.Tables[0].Rows[i][0].ToString(),
    dtst.Tables[0].Rows[i][2].ToString()});
                if (dtst.Tables[0].Rows[i][4].ToString() == "Cancelled")
                {
                    nwItem.BackColor = Color.Gainsboro;
                }
                else if (dtst.Tables[0].Rows[i][4].ToString() != "Approved")
                {
                    nwItem.BackColor = Color.Orange;
                }
                else if (double.Parse(dtst.Tables[0].Rows[i][3].ToString()) <= 0)
                {
                    nwItem.BackColor = Color.Lime;
                }
                else
                {
                    nwItem.BackColor = Color.FromArgb(255, 100, 100);
                }
                this.rcvblsDocListView.Items.Add(nwItem);
            }
            this.correctNavLbls(dtst);
            if (this.rcvblsDocListView.Items.Count > 0)
            {
                this.obey_evnts = true;
                this.rcvblsDocListView.Items[0].Selected = true;
            }
            else
            {
                this.populateDet(-10000);
                this.populateLines(-100000, "");
            }
            this.obey_evnts = true;
        }

        private void populateDet(long docHdrID)
        {
            this.clearDetInfo();
            this.disableDetEdit();
            this.obey_evnts = false;
            DataSet dtst = Global.get_One_RcvblsDocHdr(docHdrID);
            double invAmnt = 0;
            double amntPaid = 0;

            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                this.docIDTextBox.Text = dtst.Tables[0].Rows[i][0].ToString();
                this.docIDNumTextBox.Text = dtst.Tables[0].Rows[i][4].ToString();
                this.docTypeComboBox.Items.Clear();
                this.docTypeComboBox.Items.Add(dtst.Tables[0].Rows[i][5].ToString());
                if (this.editRec == false && this.addRec == false)
                {
                }
                this.docTypeComboBox.SelectedItem = dtst.Tables[0].Rows[i][5].ToString();//;
                this.srcDocIDTextBox.Text = dtst.Tables[0].Rows[i][7].ToString();
                this.srcDocTypeTextBox.Text = dtst.Tables[0].Rows[i][16].ToString();
                if (this.srcDocIDTextBox.Text == "-1" || this.srcDocIDTextBox.Text == "")
                {
                    this.srcDocNumTextBox.Text = "";
                }
                else if (this.srcDocTypeTextBox.Text.Contains("Customer"))
                {
                    this.srcDocNumTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
               "accb.accb_rcvbls_invc_hdr", "rcvbls_invc_hdr_id", "rcvbls_invc_number",
               long.Parse(this.srcDocIDTextBox.Text));
                }
                else if (this.srcDocTypeTextBox.Text == "Sales Invoice" || this.srcDocTypeTextBox.Text == "Sales Return")
                {
                    this.srcDocNumTextBox.Text = dtst.Tables[0].Rows[i][26].ToString();
                }
                else
                {
                    this.srcDocNumTextBox.Text = dtst.Tables[0].Rows[i][7].ToString();
                }

                this.docDteTextBox.Text = dtst.Tables[0].Rows[i][1].ToString();
                this.docClsfctnTextBox.Text = dtst.Tables[0].Rows[i][23].ToString();
                this.cstmrDocNumTextBox.Text = dtst.Tables[0].Rows[i][22].ToString();

                this.pymntMthdIDTextBox.Text = dtst.Tables[0].Rows[i][17].ToString();
                this.pymntMthdTextBox.Text = dtst.Tables[0].Rows[i][18].ToString();

                this.cstmrIDTextBox.Text = dtst.Tables[0].Rows[i][8].ToString();
                this.cstmrNmTextBox.Text = dtst.Tables[0].Rows[i][9].ToString();

                this.glBatchIDTextBox.Text = dtst.Tables[0].Rows[i][20].ToString();
                this.glBatchNmTextBox.Text = dtst.Tables[0].Rows[i][21].ToString();

                this.rgstrIDTextBox.Text = dtst.Tables[0].Rows[i][27].ToString();
                if (dtst.Tables[0].Rows[i][29].ToString() == "Attendance Register")
                {
                    this.rgstrNumTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                      "attn.attn_attendance_recs_hdr", "recs_hdr_id", "recs_hdr_name",
                      long.Parse(dtst.Tables[0].Rows[i][27].ToString()));
                }
                //   else if (dtst.Tables[0].Rows[i][29].ToString() == "Production Process Run")
                //   {
                //     this.rgstrNumTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                //"scm.scm_process_run", "process_run_id", "batch_code_num",
                //long.Parse(dtst.Tables[0].Rows[i][27].ToString()));
                //   }
                else
                {
                    this.rgstrNumTextBox.Text = "";
                }
                this.costCtgrTextBox.Text = dtst.Tables[0].Rows[i][28].ToString();

                double.TryParse(dtst.Tables[0].Rows[i][14].ToString(), out invAmnt);//.ToString("#,##0.00");
                double.TryParse(dtst.Tables[0].Rows[i][19].ToString(), out amntPaid);
                this.invcAmntTextBox.Text = invAmnt.ToString("#,##0.00");
                this.amntPaidTextBox.Text = amntPaid.ToString("#,##0.00");
                this.outstndngBalsTextBox.Text = (invAmnt - amntPaid).ToString("#,##0.00");
                this.invcCurrIDTextBox.Text = dtst.Tables[0].Rows[i][24].ToString();
                this.invcCurrTextBox.Text = dtst.Tables[0].Rows[i][25].ToString();

                this.cstmrSiteIDTextBox.Text = dtst.Tables[0].Rows[i][10].ToString();
                this.cstmrSiteTextBox.Text = dtst.Tables[0].Rows[i][11].ToString();

                this.docCommentsTextBox.Text = dtst.Tables[0].Rows[i][6].ToString();
                this.pymntTermsTextBox.Text = dtst.Tables[0].Rows[i][15].ToString();

                this.apprvlStatusTextBox.Text = dtst.Tables[0].Rows[i][12].ToString();
                this.nxtApprvlStatusButton.Text = dtst.Tables[0].Rows[i][13].ToString();
                if ((invAmnt - amntPaid) <= 0 && amntPaid > 0)
                {
                    this.outstndngBalsTextBox.BackColor = Color.Lime;
                }
                else if (invAmnt > 0 && this.apprvlStatusTextBox.Text != "Not Validated")
                {
                    this.outstndngBalsTextBox.BackColor = Color.FromArgb(255, 100, 100);
                }

                if (this.nxtApprvlStatusButton.Text == "Cancel")
                {
                    this.nxtApprvlStatusButton.ImageKey = "90.png";
                }
                else
                {
                    this.nxtApprvlStatusButton.ImageKey = "tick_32.png";
                }
                if (this.nxtApprvlStatusButton.Text == "None")
                {
                    this.nxtApprvlStatusButton.Enabled = false;
                }
                else
                {
                    this.nxtApprvlStatusButton.Enabled = true;
                }

                if (this.apprvlStatusTextBox.Text != "Not Validated"
                  && this.apprvlStatusTextBox.Text != "Cancelled"
                  && this.apprvlStatusTextBox.Text != "Approved")
                {
                    this.rejectDocButton.Enabled = true;
                }
                else
                {
                    this.rejectDocButton.Enabled = false;
                }
                this.createdByIDTextBox.Text = dtst.Tables[0].Rows[i][2].ToString();
                this.createdByTextBox.Text = dtst.Tables[0].Rows[i][3].ToString().ToUpper();
            }
            this.grndTotalTextBox.Text = Global.getRcvblsDocGrndAmnt(long.Parse(this.docIDTextBox.Text)).ToString("#,##0.00");
            if (this.docTypeComboBox.Text == "Customer Advance Payment"
              || this.docTypeComboBox.Text == "Customer Credit Memo (InDirect Topup)"
              || this.docTypeComboBox.Text == "Customer Debit Memo (InDirect Refund)")
            {
                this.availblePrepayAmntTextBox.Text = Global.get_RcvblPrepayDocAvlblAmnt(long.Parse(this.docIDTextBox.Text)).ToString("#,##0.00");
                double avlblPrepay = 0;
                double.TryParse(this.availblePrepayAmntTextBox.Text, out avlblPrepay);
                if ((avlblPrepay) <= 0 && amntPaid > 0)
                {
                    this.availblePrepayAmntTextBox.BackColor = Color.Lime;
                }
                else if (invAmnt > 0 && this.apprvlStatusTextBox.Text != "Not Validated")
                {
                    this.availblePrepayAmntTextBox.BackColor = Color.FromArgb(255, 100, 100);
                }
            }
            else
            {
                this.availblePrepayAmntTextBox.BackColor = Color.WhiteSmoke;
            }

            this.obey_evnts = true;
        }

        private void populateLines(long docHdrID, string docTyp)
        {
            this.clearLnsInfo();
            this.disableLnsEdit();
            //if (this.editRec == false)
            //{
            //}
            this.obey_evnts = false;

            DataSet dtst = Global.get_RcvblsDocDet(docHdrID);
            this.smmryDataGridView.DefaultCellStyle.ForeColor = Color.Black;

            this.smmryDataGridView.Rows.Clear();

            int rwcnt = dtst.Tables[0].Rows.Count;
            for (int i = 0; i < rwcnt; i++)
            {
                this.smmryDataGridView.RowCount += 1;//, this.apprvlStatusTextBox.Text.Insert(this.rgstrDetDataGridView.RowCount - 1, 1);
                int rowIdx = this.smmryDataGridView.RowCount - 1;

                this.smmryDataGridView.Rows[rowIdx].HeaderCell.Value = (i + 1).ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[0].Value = dtst.Tables[0].Rows[i][1].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[1].Value = dtst.Tables[0].Rows[i][2].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[2].Value = double.Parse(dtst.Tables[0].Rows[i][3].ToString()).ToString("#,##0.00");
                this.smmryDataGridView.Rows[rowIdx].Cells[3].Value = dtst.Tables[0].Rows[i][12].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[4].Value = dtst.Tables[0].Rows[i][11].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[5].Value = dtst.Tables[0].Rows[i][0].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[6].Value = dtst.Tables[0].Rows[i][4].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[7].Value = Global.mnFrm.cmCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][5].ToString());
                this.smmryDataGridView.Rows[rowIdx].Cells[8].Value = dtst.Tables[0].Rows[i][6].ToString();

                int chrgAcntID = int.Parse(dtst.Tables[0].Rows[i][7].ToString());
                this.smmryDataGridView.Rows[rowIdx].Cells[9].Value = Global.mnFrm.cmCde.getAccntNum(chrgAcntID) + "." +
                  Global.mnFrm.cmCde.getAccntName(chrgAcntID);
                this.smmryDataGridView.Rows[rowIdx].Cells[10].Value = chrgAcntID.ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[11].Value = "...";

                this.smmryDataGridView.Rows[rowIdx].Cells[12].Value = dtst.Tables[0].Rows[i][8].ToString();
                int balsAcntID = int.Parse(dtst.Tables[0].Rows[i][9].ToString());
                this.smmryDataGridView.Rows[rowIdx].Cells[13].Value = Global.mnFrm.cmCde.getAccntNum(balsAcntID) + "." +
                  Global.mnFrm.cmCde.getAccntName(balsAcntID);
                this.smmryDataGridView.Rows[rowIdx].Cells[14].Value = balsAcntID.ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[15].Value = "...";

                long prepyDocID = long.Parse(dtst.Tables[0].Rows[i][10].ToString());
                this.smmryDataGridView.Rows[rowIdx].Cells[16].Value = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
                  "rcvbls_invc_hdr_id", "rcvbls_invc_number",
                  prepyDocID);
                this.smmryDataGridView.Rows[rowIdx].Cells[17].Value = prepyDocID.ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[18].Value = "...";

                this.smmryDataGridView.Rows[rowIdx].Cells[19].Value = dtst.Tables[0].Rows[i][17].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[20].Value = dtst.Tables[0].Rows[i][18].ToString();

                this.smmryDataGridView.Rows[rowIdx].Cells[21].Value = double.Parse(dtst.Tables[0].Rows[i][19].ToString()).ToString("#,##0.00");
                this.smmryDataGridView.Rows[rowIdx].Cells[22].Value = dtst.Tables[0].Rows[i][14].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[23].Value = dtst.Tables[0].Rows[i][13].ToString();

                this.smmryDataGridView.Rows[rowIdx].Cells[24].Value = double.Parse(dtst.Tables[0].Rows[i][20].ToString()).ToString("#,##0.00");
                this.smmryDataGridView.Rows[rowIdx].Cells[25].Value = dtst.Tables[0].Rows[i][16].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[26].Value = dtst.Tables[0].Rows[i][15].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[27].Value = dtst.Tables[0].Rows[i][21].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[28].Value = dtst.Tables[0].Rows[i][23].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[29].Value = double.Parse(dtst.Tables[0].Rows[i][24].ToString()).ToString("#,##0.00");
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
                this.totl_rec = Global.get_Total_RcvblsDoc(this.searchForTextBox.Text,
                  this.searchInComboBox.Text, Global.mnFrm.cmCde.Org_id,
                this.showUnpaidCheckBox.Checked);
                this.updtTotals();
                this.rec_cur_indx = Global.mnFrm.cmCde.navFuncts.totalGroups - 1;
            }
            this.getPnlData();
        }

        private void clearDetInfo()
        {
            this.obey_evnts = false;
            this.docIDTextBox.Text = "-1";
            this.docIDNumTextBox.Text = "";
            this.docTypeComboBox.Items.Clear();
            this.docIDPrfxComboBox.Items.Clear();
            this.docCommentsTextBox.Text = "";
            this.pymntTermsTextBox.Text = "";
            this.cstmrDocNumTextBox.Text = "";

            this.pymntMthdIDTextBox.Text = "-1";
            this.pymntMthdTextBox.Text = "";
            this.rgstrIDTextBox.Text = "-1";
            this.rgstrNumTextBox.Text = "";
            this.costCtgrTextBox.Text = "";

            this.glBatchIDTextBox.Text = "-1";
            this.glBatchNmTextBox.Text = "";

            this.invcAmntTextBox.Text = "0.00";
            this.amntPaidTextBox.Text = "0.00";
            this.outstndngBalsTextBox.Text = "0.00";
            this.availblePrepayAmntTextBox.Text = "0.00";

            this.invcCurrIDTextBox.Text = "-1";
            this.invcCurrTextBox.Text = "";

            this.srcDocIDTextBox.Text = "-1";
            this.srcDocNumTextBox.Text = "";
            this.srcDocTypeTextBox.Text = "";

            this.createdByIDTextBox.Text = "-1";
            this.createdByTextBox.Text = "";

            this.cstmrIDTextBox.Text = "-1";
            this.cstmrNmTextBox.Text = "";
            this.cstmrSiteIDTextBox.Text = "-1";
            this.cstmrSiteTextBox.Text = "";
            this.docDteTextBox.Text = "";
            this.docClsfctnTextBox.Text = "";
            this.apprvlStatusTextBox.Text = "Not Validated";
            this.nxtApprvlStatusButton.Text = "Approve";
            this.nxtApprvlStatusButton.ImageKey = "tick_32.png";
            this.grndTotalTextBox.Text = "0.00";

            this.obey_evnts = true;
        }

        private void prpareForDetEdit()
        {
            bool prv = this.obey_evnts;
            this.disableFormButtons();
            this.obey_evnts = false;
            this.saveButton.Enabled = true;
            this.docIDNumTextBox.ReadOnly = false;
            this.docIDNumTextBox.BackColor = Color.FromArgb(255, 255, 128);
            this.docCommentsTextBox.ReadOnly = false;
            this.docCommentsTextBox.BackColor = Color.FromArgb(255, 255, 128);

            this.pymntTermsTextBox.ReadOnly = false;
            this.pymntTermsTextBox.BackColor = Color.White;

            this.rgstrNumTextBox.ReadOnly = true;
            this.rgstrNumTextBox.BackColor = Color.White;

            this.costCtgrTextBox.ReadOnly = true;
            this.costCtgrTextBox.BackColor = Color.White;

            this.docDteTextBox.ReadOnly = false;
            this.docDteTextBox.BackColor = Color.FromArgb(255, 255, 128);
            this.invcAmntTextBox.ReadOnly = false;
            this.invcAmntTextBox.BackColor = Color.FromArgb(255, 255, 128);

            this.docClsfctnTextBox.ReadOnly = false;
            this.docClsfctnTextBox.BackColor = Color.White;

            this.cstmrDocNumTextBox.ReadOnly = false;
            this.cstmrDocNumTextBox.BackColor = Color.FromArgb(255, 255, 128);

            this.cstmrNmTextBox.ReadOnly = false;
            this.cstmrNmTextBox.BackColor = Color.FromArgb(255, 255, 128);

            this.cstmrSiteTextBox.ReadOnly = false;
            this.cstmrSiteTextBox.BackColor = Color.FromArgb(255, 255, 128);

            this.srcDocNumTextBox.ReadOnly = false;
            this.srcDocNumTextBox.BackColor = Color.White;
            this.srcDocTypeTextBox.ReadOnly = true;
            this.srcDocTypeTextBox.BackColor = Color.WhiteSmoke;

            this.glBatchNmTextBox.ReadOnly = true;
            this.glBatchNmTextBox.BackColor = Color.WhiteSmoke;
            this.createdByTextBox.ReadOnly = true;
            this.createdByTextBox.BackColor = Color.WhiteSmoke;
            this.amntPaidTextBox.ReadOnly = true;
            this.amntPaidTextBox.BackColor = Color.WhiteSmoke;
            this.outstndngBalsTextBox.ReadOnly = true;
            this.outstndngBalsTextBox.BackColor = Color.WhiteSmoke;

            this.pymntMthdTextBox.ReadOnly = false;
            this.pymntMthdTextBox.BackColor = Color.FromArgb(255, 255, 128);
            this.invcCurrTextBox.ReadOnly = false;
            this.invcCurrTextBox.BackColor = Color.FromArgb(255, 255, 128);

            string selItm = this.docTypeComboBox.Text;
            this.docTypeComboBox.Items.Clear();
            this.docIDPrfxComboBox.Items.Clear();

            if (this.addRec == true)
            {
                if (this.addRecsCSP == true || this.editRecsCSP == true)
                {
                    this.docTypeComboBox.Items.Add("Customer Standard Payment");
                }
                if (this.addRecsCAP == true || this.editRecsCAP == true)
                {
                    this.docTypeComboBox.Items.Add("Customer Advance Payment");
                }
                if (this.addRecsDRTC == true || this.editRecsDRTC == true)
                {
                    this.docTypeComboBox.Items.Add("Direct Refund to Customer");
                }
                if (this.addRecsCCMIT == true || this.editRecsCCMIT == true)
                {
                    this.docTypeComboBox.Items.Add("Customer Credit Memo (InDirect Topup)");
                }
                if (this.addRecsDTFC == true || this.editRecsDTFC == true)
                {
                    this.docTypeComboBox.Items.Add("Direct Topup from Customer");
                }
                if (this.addRecsCDMIR == true || this.editRecsCDMIR == true)
                {
                    this.docTypeComboBox.Items.Add("Customer Debit Memo (InDirect Refund)");
                }
            }
            if (this.editRec == true)
            {
                this.docTypeComboBox.Items.Add(selItm);
                this.docTypeComboBox.SelectedItem = selItm;
            }
            this.obey_evnts = prv;
        }

        private void disableDetEdit()
        {
            this.addRec = false;
            this.editRec = false;
            this.saveButton.Enabled = false;
            this.disableFormButtons();
            this.docIDNumTextBox.ReadOnly = true;
            this.docIDNumTextBox.BackColor = Color.WhiteSmoke;
            this.docCommentsTextBox.ReadOnly = true;
            this.docCommentsTextBox.BackColor = Color.WhiteSmoke;

            this.pymntTermsTextBox.ReadOnly = true;
            this.pymntTermsTextBox.BackColor = Color.WhiteSmoke;
            this.rgstrNumTextBox.ReadOnly = true;
            this.rgstrNumTextBox.BackColor = Color.WhiteSmoke;

            this.costCtgrTextBox.ReadOnly = true;
            this.costCtgrTextBox.BackColor = Color.WhiteSmoke;

            this.docDteTextBox.ReadOnly = true;
            this.docDteTextBox.BackColor = Color.WhiteSmoke;
            this.invcAmntTextBox.ReadOnly = true;
            this.invcAmntTextBox.BackColor = Color.WhiteSmoke;

            this.docClsfctnTextBox.ReadOnly = true;
            this.docClsfctnTextBox.BackColor = Color.WhiteSmoke;

            this.cstmrDocNumTextBox.ReadOnly = true;
            this.cstmrDocNumTextBox.BackColor = Color.WhiteSmoke;

            this.cstmrNmTextBox.ReadOnly = true;
            this.cstmrNmTextBox.BackColor = Color.WhiteSmoke;

            this.cstmrSiteTextBox.ReadOnly = true;
            this.cstmrSiteTextBox.BackColor = Color.WhiteSmoke;

            this.srcDocNumTextBox.ReadOnly = true;
            this.srcDocNumTextBox.BackColor = Color.WhiteSmoke;
            this.srcDocTypeTextBox.ReadOnly = true;
            this.srcDocTypeTextBox.BackColor = Color.WhiteSmoke;
            this.glBatchNmTextBox.ReadOnly = true;
            this.glBatchNmTextBox.BackColor = Color.WhiteSmoke;
            this.createdByTextBox.ReadOnly = true;
            this.createdByTextBox.BackColor = Color.WhiteSmoke;
            this.amntPaidTextBox.ReadOnly = true;
            this.amntPaidTextBox.BackColor = Color.WhiteSmoke;
            this.outstndngBalsTextBox.ReadOnly = true;
            this.outstndngBalsTextBox.BackColor = Color.WhiteSmoke;

            this.availblePrepayAmntTextBox.ReadOnly = true;
            this.availblePrepayAmntTextBox.BackColor = Color.WhiteSmoke;

            this.pymntMthdTextBox.ReadOnly = true;
            this.pymntMthdTextBox.BackColor = Color.WhiteSmoke;
            this.invcCurrTextBox.ReadOnly = true;
            this.invcCurrTextBox.BackColor = Color.WhiteSmoke;
        }

        private void clearLnsInfo()
        {
            this.obey_evnts = false;
            this.smmryDataGridView.Rows.Clear();
            this.smmryDataGridView.DefaultCellStyle.ForeColor = Color.Black;
            //this.grndTotalTextBox.Text = "0.00";
            this.obey_evnts = true;
        }

        private void prpareForLnsEdit()
        {
            this.saveButton.Enabled = true;
            //this.addLineButton.Enabled = this.addRecsCSP == true ? this.addRecsCSP : this.addRecsCAP;
            //this.delLineButton.Enabled = this.addRecsCSP == true ? this.addRecsCSP : this.addRecsCAP;
            this.smmryDataGridView.ReadOnly = false;
            this.smmryDataGridView.Columns[0].ReadOnly = true;
            this.smmryDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[1].ReadOnly = false;
            this.smmryDataGridView.Columns[1].DefaultCellStyle.BackColor = Color.White;
            this.smmryDataGridView.Columns[2].ReadOnly = false;
            this.smmryDataGridView.Columns[2].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.smmryDataGridView.Columns[3].ReadOnly = true;
            this.smmryDataGridView.Columns[3].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[7].ReadOnly = false;
            this.smmryDataGridView.Columns[7].DefaultCellStyle.BackColor = Color.White;
            this.smmryDataGridView.Columns[8].ReadOnly = false;
            this.smmryDataGridView.Columns[8].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.smmryDataGridView.Columns[9].ReadOnly = false;
            this.smmryDataGridView.Columns[9].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.smmryDataGridView.Columns[12].ReadOnly = true;
            this.smmryDataGridView.Columns[12].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[13].ReadOnly = true;
            this.smmryDataGridView.Columns[13].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[16].ReadOnly = true;
            this.smmryDataGridView.Columns[16].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[19].ReadOnly = false;
            this.smmryDataGridView.Columns[19].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.smmryDataGridView.Columns[20].ReadOnly = false;
            this.smmryDataGridView.Columns[20].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.smmryDataGridView.Columns[21].ReadOnly = true;
            this.smmryDataGridView.Columns[21].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[22].ReadOnly = true;
            this.smmryDataGridView.Columns[22].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[24].ReadOnly = true;
            this.smmryDataGridView.Columns[24].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[25].ReadOnly = true;
            this.smmryDataGridView.Columns[25].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[28].ReadOnly = false;
            this.smmryDataGridView.Columns[28].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.smmryDataGridView.Columns[29].ReadOnly = false;
            this.smmryDataGridView.Columns[29].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
        }

        private void disableLnsEdit()
        {
            this.addRec = false;
            this.editRec = false;
            this.smmryDataGridView.ReadOnly = true;
            this.smmryDataGridView.Columns[0].ReadOnly = true;
            this.smmryDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[1].ReadOnly = true;
            this.smmryDataGridView.Columns[1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[2].ReadOnly = true;
            this.smmryDataGridView.Columns[2].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[3].ReadOnly = true;
            this.smmryDataGridView.Columns[3].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[7].ReadOnly = true;
            this.smmryDataGridView.Columns[7].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[8].ReadOnly = true;
            this.smmryDataGridView.Columns[8].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[9].ReadOnly = true;
            this.smmryDataGridView.Columns[9].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[12].ReadOnly = true;
            this.smmryDataGridView.Columns[12].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[13].ReadOnly = true;
            this.smmryDataGridView.Columns[13].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[16].ReadOnly = true;
            this.smmryDataGridView.Columns[16].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[19].ReadOnly = true;
            this.smmryDataGridView.Columns[19].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[20].ReadOnly = true;
            this.smmryDataGridView.Columns[20].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[21].ReadOnly = true;
            this.smmryDataGridView.Columns[21].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[22].ReadOnly = true;
            this.smmryDataGridView.Columns[22].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[24].ReadOnly = true;
            this.smmryDataGridView.Columns[24].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[25].ReadOnly = true;
            this.smmryDataGridView.Columns[25].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[28].ReadOnly = true;
            this.smmryDataGridView.Columns[28].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            this.smmryDataGridView.Columns[29].ReadOnly = true;
            this.smmryDataGridView.Columns[29].DefaultCellStyle.BackColor = Color.WhiteSmoke;
        }

        private void searchForTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            EventArgs ex = new EventArgs();
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                this.goButton_Click(this.rfrshButton, ex);
            }
        }

        private void positionTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

        private void docTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.smmryDataGridView.Columns[11].Visible = true;
            this.disableFormButtons();
            //this.clearDetInfo();
            if (this.addRec == true || this.editRec == true)
            {
                this.saveButton.Enabled = true;
                this.addButton.Enabled = false;
                this.editButton.Enabled = false;
            }
            if (this.obey_evnts == false)
            {
                return;
            }
            this.obey_evnts = false;

            if (this.docTypeComboBox.Text == "Customer Advance Payment")
            {
                this.docIDPrfxComboBox.Items.Clear();
                if (this.addRec == true || this.editRec == true)
                {
                    this.docIDPrfxComboBox.Items.Add("CAP");
                    this.docIDPrfxComboBox.SelectedIndex = 0;
                    this.srcDocTypeTextBox.Text = "";
                    this.srcDocNumTextBox.Text = "";
                    this.srcDocIDTextBox.Text = "-1";
                    this.srcDocButton.Enabled = false;
                    this.srcDocNumTextBox.ReadOnly = true;
                    this.srcDocNumTextBox.BackColor = Color.WhiteSmoke;
                    this.docClsfctnTextBox.Text = "";
                    this.docClsfctnTextBox.ReadOnly = false;
                    this.docClsfctnTextBox.BackColor = Color.White;
                    this.docClsfctnButton.Enabled = true;
                }
            }
            else if (this.docTypeComboBox.Text == "Customer Standard Payment")
            {
                this.docIDPrfxComboBox.Items.Clear();
                if (this.addRec == true || this.editRec == true)
                {
                    this.docIDPrfxComboBox.Items.Add("CSP");
                    this.docIDPrfxComboBox.SelectedIndex = 0;
                    this.srcDocTypeTextBox.Text = "";
                    this.srcDocNumTextBox.Text = "";
                    this.srcDocIDTextBox.Text = "-1";
                    this.srcDocButton.Enabled = false;
                    this.srcDocNumTextBox.ReadOnly = true;
                    this.srcDocNumTextBox.BackColor = Color.WhiteSmoke;
                    this.docClsfctnTextBox.Text = "";
                    this.docClsfctnTextBox.ReadOnly = false;
                    this.docClsfctnTextBox.BackColor = Color.White;
                    this.docClsfctnButton.Enabled = true;
                }
            }
            else if (this.docTypeComboBox.Text == "Direct Refund to Customer")
            {
                this.docIDPrfxComboBox.Items.Clear();
                if (this.addRec == true || this.editRec == true)
                {
                    this.docIDPrfxComboBox.Items.Add("DRTC");
                    this.docIDPrfxComboBox.SelectedIndex = 0;
                    this.srcDocTypeTextBox.Text = "Customer Standard Payment";
                    this.srcDocNumTextBox.Text = "";
                    this.srcDocIDTextBox.Text = "-1";
                    this.srcDocButton.Enabled = true;
                    this.srcDocNumTextBox.ReadOnly = false;
                    this.srcDocNumTextBox.BackColor = Color.FromArgb(255, 255, 128);
                    this.docClsfctnTextBox.Text = "";
                    this.docClsfctnTextBox.ReadOnly = true;
                    this.docClsfctnTextBox.BackColor = Color.White;
                    this.docClsfctnButton.Enabled = false;
                }
            }
            else if (this.docTypeComboBox.Text == "Customer Credit Memo (InDirect Topup)")
            {
                this.docIDPrfxComboBox.Items.Clear();
                if (this.addRec == true || this.editRec == true)
                {
                    this.docIDPrfxComboBox.Items.Add("CCM-IT");
                    this.docIDPrfxComboBox.SelectedIndex = 0;
                    this.srcDocTypeTextBox.Text = "Customer Standard Payment";
                    this.srcDocNumTextBox.Text = "";
                    this.srcDocIDTextBox.Text = "-1";
                    this.srcDocButton.Enabled = true;
                    this.srcDocNumTextBox.ReadOnly = false;
                    this.srcDocNumTextBox.BackColor = Color.FromArgb(255, 255, 128);
                    this.docClsfctnTextBox.Text = "";
                    this.docClsfctnTextBox.ReadOnly = true;
                    this.docClsfctnTextBox.BackColor = Color.White;
                    this.docClsfctnButton.Enabled = false;
                }
            }
            else if (this.docTypeComboBox.Text == "Direct Topup from Customer")
            {
                this.docIDPrfxComboBox.Items.Clear();
                if (this.addRec == true || this.editRec == true)
                {
                    this.docIDPrfxComboBox.Items.Add("DTFC");
                    this.docIDPrfxComboBox.SelectedIndex = 0;
                    this.srcDocTypeTextBox.Text = "Customer Standard Payment";
                    this.srcDocNumTextBox.Text = "";
                    this.srcDocIDTextBox.Text = "-1";
                    this.srcDocButton.Enabled = true;
                    this.srcDocNumTextBox.ReadOnly = false;
                    this.srcDocNumTextBox.BackColor = Color.FromArgb(255, 255, 128);
                    this.docClsfctnTextBox.Text = "";
                    this.docClsfctnTextBox.ReadOnly = true;
                    this.docClsfctnTextBox.BackColor = Color.White;
                    this.docClsfctnButton.Enabled = false;
                }
            }
            else if (this.docTypeComboBox.Text == "Customer Debit Memo (InDirect Refund)")
            {
                this.docIDPrfxComboBox.Items.Clear();
                if (this.addRec == true || this.editRec == true)
                {
                    this.docIDPrfxComboBox.Items.Add("CDM-IR");
                    this.docIDPrfxComboBox.SelectedIndex = 0;
                    this.srcDocTypeTextBox.Text = "Customer Standard Payment";
                    this.srcDocNumTextBox.Text = "";
                    this.srcDocIDTextBox.Text = "-1";
                    this.srcDocButton.Enabled = true;
                    this.srcDocNumTextBox.ReadOnly = false;
                    this.srcDocNumTextBox.BackColor = Color.FromArgb(255, 255, 128);
                    this.docClsfctnTextBox.Text = "";
                    this.docClsfctnTextBox.ReadOnly = true;
                    this.docClsfctnTextBox.BackColor = Color.White;
                    this.docClsfctnButton.Enabled = false;
                }
            }
            this.clearLnsInfo();
            this.obey_evnts = true;
        }

        private void docIDPrfxComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.docIDNumTextBox.Text.Contains(this.docIDPrfxComboBox.Text))
            {
                string dte = DateTime.Parse(Global.mnFrm.cmCde.getFrmtdDB_Date_time()).ToString("yyMMdd");
                this.docIDNumTextBox.Text = this.docIDPrfxComboBox.Text + dte
                          + "-" + Global.mnFrm.cmCde.getRandomInt(100, 1000)
                          + "-" + (Global.mnFrm.cmCde.getRecCount("accb.accb_rcvbls_invc_hdr", "rcvbls_invc_number",
                          "rcvbls_invc_hdr_id", this.docIDPrfxComboBox.Text + dte + "-%") + 1).ToString().PadLeft(3, '0');

                //this.docIDNumTextBox.Text = this.docIDPrfxComboBox.Text + "-" +
                // DateTime.Parse(Global.mnFrm.cmCde.getFrmtdDB_Date_time()).ToString("yyMMdd-HHmmss")
                //          + "-" + Global.mnFrm.cmCde.getRandomInt(10, 100);
                /*Global.getLtstRcvblsIDNoInPrfx(this.docIDPrfxComboBox.Text) + "-" +
          Global.mnFrm.cmCde.getFrmtdDB_Date_time().Substring(12, 8).Replace(":", "") + "-" +
              Global.getLtstRecPkID("accb.accb_rcvbls_invc_hdr",
              "rcvbls_invc_hdr_id");*/
            }
        }

        private void docDteButton_Click(object sender, EventArgs e)
        {
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("You must be in ADD/EDIT mode first!", 0);
                return;
            }
            Global.mnFrm.cmCde.selectDate(ref this.docDteTextBox);
            if (this.docDteTextBox.Text.Length > 11)
            {
                this.docDteTextBox.Text = this.docDteTextBox.Text.Substring(0, 11);
            }
        }

        private void cstmrButton_Click(object sender, EventArgs e)
        {
            this.cstmrNmLOVSearch("%");
        }

        private void cstmrSiteButton_Click(object sender, EventArgs e)
        {
            this.cstmrSiteLOVSearch("%");
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            this.loadPanel();
        }

        private void rfrshButton_Click(object sender, EventArgs e)
        {
            this.loadPanel();
        }

        private void rcvblsDocListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.shdObeyEvts() == false)
            {
                return;
            }
            if (this.rcvblsDocListView.SelectedItems.Count > 0)
            {
                this.populateDet(long.Parse(this.rcvblsDocListView.SelectedItems[0].SubItems[2].Text));
                this.populateLines(long.Parse(this.rcvblsDocListView.SelectedItems[0].SubItems[2].Text),
                    this.rcvblsDocListView.SelectedItems[0].SubItems[3].Text);
            }
            else
            {
                this.populateDet(-100000);
                this.populateLines(-100000, "");
            }
        }

        private void rcvblsDocListView_ItemSelectionChanged(object sender,
          System.Windows.Forms.ListViewItemSelectionChangedEventArgs e)
        {
            if (this.shdObeyEvts() == false)
            {
                return;
            }
            if (e.IsSelected)
            {
                e.Item.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            }
            else
            {
                e.Item.Font = new Font("Tahoma", 8.25f, FontStyle.Regular);
            }
        }


        private void setupDocTmpltsButton_Click(object sender, EventArgs e)
        {
            if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[52]) == false)
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                  " this action!\nContact your System Administrator!", 0);
                return;
            }
            docTmpltsDiag nwdiag = new docTmpltsDiag();
            DialogResult dgres = nwdiag.ShowDialog();
            if (dgres == DialogResult.OK)
            {
            }
        }

        private void vwSQLButton_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.showSQL(this.rec_SQL, 10);
        }

        private void rcHstryButton_Click(object sender, EventArgs e)
        {
            if (this.rcvblsDocListView.SelectedItems.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select a Record First!", 0);
                return;//cstmr
            }
            Global.mnFrm.cmCde.showRecHstry(
              Global.mnFrm.cmCde.get_Gnrl_Rec_Hstry(long.Parse(this.rcvblsDocListView.SelectedItems[0].SubItems[2].Text),
              "accb.accb_rcvbls_invc_hdr", "rcvbls_invc_hdr_id"), 9);
        }

        private void vwSmrySQLButton_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.showSQL(this.recDt_SQL, 10);
        }

        private void rcHstrySmryButton_Click(object sender, EventArgs e)
        {
            if (this.smmryDataGridView.SelectedRows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select a Record First!", 0);
                return;
            }
            Global.mnFrm.cmCde.showRecHstry(
              Global.mnFrm.cmCde.get_Gnrl_Rec_Hstry(long.Parse(
              this.smmryDataGridView.SelectedRows[0].Cells[5].Value.ToString()),
              "accb.accb_rcvbl_amnt_smmrys", "rcvbl_smmry_id"), 9);
        }

        private void docDteTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!this.obey_evnts)
            {
                this.txtChngd = false;
                return;
            }
            this.txtChngd = true;
        }

        private void docDteTextBox_Leave(object sender, EventArgs e)
        {
            if (this.txtChngd == false)
            {
                return;
            }
            this.txtChngd = false;
            TextBox mytxt = (TextBox)sender;
            this.obey_evnts = false;
            string srchWrd = mytxt.Text;
            if (!mytxt.Text.Contains("%"))
            {
                srchWrd = "%" + srchWrd.Replace(" ", "%") + "%";
            }

            if (mytxt.Name == "invcCurrTextBox")
            {
                this.crncyNmLOVSearch(srchWrd);
            }
            else if (mytxt.Name == "docClsfctnTextBox")
            {
                this.docClsfctnLOVSearch(srchWrd);
            }
            else if (mytxt.Name == "cstmrNmTextBox")
            {
                this.cstmrNmLOVSearch(srchWrd);
            }
            else if (mytxt.Name == "cstmrSiteTextBox")
            {
                this.cstmrSiteLOVSearch(srchWrd);
            }
            else if (mytxt.Name == "pymntMthdTextBox")
            {
                this.pymntMthdLOVSearch(srchWrd);
            }
            else if (mytxt.Name == "docDteTextBox")
            {
                this.trnsDteLOVSrch();
            }
            else if (mytxt.Name == "invcAmntTextBox")
            {
                this.amntLOVSrch();
            }
            else if (mytxt.Name == "srcDocNumTextBox")
            {
                this.srcDocNumLOVSrch(srchWrd);
            }
            this.obey_evnts = true;
            this.txtChngd = false;
        }

        private void srcDocNumLOVSrch(string srchWrd)
        {
            this.txtChngd = false;
            if (this.addRec == false && this.editRec == false)
            {
                Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }
            if (this.docTypeComboBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Please pick a Document Type First!", 0);
                return;
            }
            if (this.cstmrIDTextBox.Text == "" || this.cstmrIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("Please pick a Customer First!", 0);
                return;
            }
            if (this.docTypeComboBox.Text == "Customer Advance Payment"
         || this.docTypeComboBox.Text == "Customer Standard Payment")
            {
                Global.mnFrm.cmCde.showMsg("Cannot use this to Select a Source Document for this Document Type!", 0);
                return;
            }

            string[] selVals = new string[1];
            selVals[0] = this.srcDocIDTextBox.Text;
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
              Global.mnFrm.cmCde.getLovID("Customer Standard Payments"), ref selVals,
              true, true, Global.mnFrm.cmCde.Org_id, this.cstmrIDTextBox.Text, this.invcCurrIDTextBox.Text,
             srchWrd, "Both", true);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.srcDocIDTextBox.Text = selVals[i];
                    this.srcDocNumTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                      "accb.accb_rcvbls_invc_hdr", "rcvbls_invc_hdr_id", "rcvbls_invc_number",
                      long.Parse(selVals[i]));
                    bool prv = this.obey_evnts;
                    this.obey_evnts = false;
                    this.docClsfctnTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                      "accb.accb_rcvbls_invc_hdr", "rcvbls_invc_hdr_id", "doc_tmplt_clsfctn",
                      long.Parse(selVals[i]));
                    this.obey_evnts = true;
                    if (long.Parse(selVals[i]) > 0)
                    {
                        //Load Content of Source Doc
                        this.populateSourceDocDet(long.Parse(selVals[i]));
                        return;
                    }
                }
            }
            this.txtChngd = false;
        }

        private void docClsfctnLOVSearch(string srchWrd)
        {
            this.txtChngd = false;
            if (this.addRec == false && this.editRec == false)
            {
                Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }
            if (this.docTypeComboBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Please pick a Document Type First!", 0);
                return;
            }

            string[] selVals = new string[1];
            selVals[0] = Global.mnFrm.cmCde.getGnrlRecID(
                  "accb.accb_doc_tmplts_hdr", "doc_tmplt_name", "doc_tmplts_hdr_id",
                  this.docClsfctnTextBox.Text).ToString();
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
              Global.mnFrm.cmCde.getLovID("Payment Document Templates"), ref selVals,
              true, true, Global.mnFrm.cmCde.Org_id, this.docTypeComboBox.Text, "",
             srchWrd, "Both", true);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    //this.accntIDTextBox.Text = selVals[i];
                    this.docClsfctnTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                      "accb.accb_doc_tmplts_hdr", "doc_tmplts_hdr_id", "doc_tmplt_name",
                      int.Parse(selVals[i]));
                    if (int.Parse(selVals[i]) > 0)
                    {
                        //Load Content of Doc Template
                        if (Global.mnFrm.cmCde.showMsg("Do you want to OVERWRITE all lines \r\n in this Document with the Content of this Template?" +
                  "\r\nThis action cannot be undone!", 1) == DialogResult.No)
                        {
                            //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                            return;
                        }
                        this.populateTmpltDet(int.Parse(selVals[i]));
                        return;
                    }
                }
            }
            this.txtChngd = false;
        }

        private void populateTmpltDet(int tmpltID)
        {
            this.obey_evnts = false;
            this.txtChngd = false;
            DataSet dtst = Global.get_DocTmpltsDet(tmpltID);
            this.smmryDataGridView.DefaultCellStyle.ForeColor = Color.Black;

            this.smmryDataGridView.Rows.Clear();

            int rwcnt = dtst.Tables[0].Rows.Count;
            for (int i = 0; i < rwcnt; i++)
            {
                this.smmryDataGridView.RowCount += 1;//.Insert(this.rgstrDetDataGridView.RowCount - 1, 1);
                int rowIdx = this.smmryDataGridView.RowCount - 1;

                this.smmryDataGridView.Rows[rowIdx].HeaderCell.Value = (i + 1).ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[0].Value = dtst.Tables[0].Rows[i][1].ToString();
                if (dtst.Tables[0].Rows[i][1].ToString() == "1Initial Amount")
                {
                    this.smmryDataGridView.Rows[rowIdx].Cells[1].Value = this.docCommentsTextBox.Text;
                }
                else
                {
                    this.smmryDataGridView.Rows[rowIdx].Cells[1].Value = dtst.Tables[0].Rows[i][2].ToString();
                }
                this.smmryDataGridView.Rows[rowIdx].Cells[2].Value = "0.00";
                this.smmryDataGridView.Rows[rowIdx].Cells[3].Value = this.invcCurrTextBox.Text;
                this.smmryDataGridView.Rows[rowIdx].Cells[4].Value = this.invcCurrIDTextBox.Text;
                this.smmryDataGridView.Rows[rowIdx].Cells[5].Value = "-1";
                this.smmryDataGridView.Rows[rowIdx].Cells[6].Value = dtst.Tables[0].Rows[i][6].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[7].Value = Global.mnFrm.cmCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][5].ToString());
                this.smmryDataGridView.Rows[rowIdx].Cells[8].Value = dtst.Tables[0].Rows[i][3].ToString();

                int chrgAcntID = int.Parse(dtst.Tables[0].Rows[i][4].ToString());
                this.smmryDataGridView.Rows[rowIdx].Cells[9].Value = Global.mnFrm.cmCde.getAccntNum(chrgAcntID) + "." +
                  Global.mnFrm.cmCde.getAccntName(chrgAcntID);
                this.smmryDataGridView.Rows[rowIdx].Cells[10].Value = chrgAcntID.ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[11].Value = "...";

                string lnType = dtst.Tables[0].Rows[i][1].ToString();
                string[] acnts = this.getRcvblBalncnAccnt(lnType,
                  int.Parse(dtst.Tables[0].Rows[i][6].ToString()),
                  int.Parse(this.cstmrIDTextBox.Text), -1, this.docTypeComboBox.Text);
                this.smmryDataGridView.Rows[rowIdx].Cells[12].Value = acnts[0];
                this.smmryDataGridView.Rows[rowIdx].Cells[13].Value = Global.mnFrm.cmCde.getAccntNum(int.Parse(acnts[1])) + "." +
                  Global.mnFrm.cmCde.getAccntName(int.Parse(acnts[1]));
                this.smmryDataGridView.Rows[rowIdx].Cells[14].Value = acnts[1];
                this.smmryDataGridView.Rows[rowIdx].Cells[15].Value = "...";

                this.smmryDataGridView.Rows[rowIdx].Cells[16].Value = "";
                this.smmryDataGridView.Rows[rowIdx].Cells[17].Value = "-1";
                this.smmryDataGridView.Rows[rowIdx].Cells[18].Value = "...";

                this.smmryDataGridView.Rows[rowIdx].Cells[19].Value = "0.00";
                this.smmryDataGridView.Rows[rowIdx].Cells[20].Value = "0.00";

                this.smmryDataGridView.Rows[rowIdx].Cells[21].Value = "0.00";
                this.smmryDataGridView.Rows[rowIdx].Cells[22].Value = this.curCode;
                this.smmryDataGridView.Rows[rowIdx].Cells[23].Value = this.curid;

                this.smmryDataGridView.Rows[rowIdx].Cells[24].Value = "0.00";
                this.smmryDataGridView.Rows[rowIdx].Cells[25].Value = this.curCode;
                this.smmryDataGridView.Rows[rowIdx].Cells[26].Value = this.curid;
            }
            this.obey_evnts = true;
            this.txtChngd = false;
        }

        private void populateSourceDocDet(long srcDocID)
        {
            this.obey_evnts = false;
            this.txtChngd = false;
            DataSet dtst = Global.get_RcvblsDocDet(srcDocID);
            this.smmryDataGridView.DefaultCellStyle.ForeColor = Color.Black;

            this.smmryDataGridView.Rows.Clear();

            int rwcnt = dtst.Tables[0].Rows.Count;
            //MessageBox.Show(srcDocID.ToString() + "-" + rwcnt.ToString());
            for (int i = 0; i < rwcnt; i++)
            {
                if (dtst.Tables[0].Rows[i][1].ToString() == "5Applied Prepayment")
                {
                    continue;
                }
                this.smmryDataGridView.RowCount += 1;//.Insert(this.rgstrDetDataGridView.RowCount - 1, 1);
                int rowIdx = this.smmryDataGridView.RowCount - 1;

                this.smmryDataGridView.Rows[rowIdx].HeaderCell.Value = (i + 1).ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[0].Value = dtst.Tables[0].Rows[i][1].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[1].Value = dtst.Tables[0].Rows[i][2].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[2].Value = double.Parse(dtst.Tables[0].Rows[i][3].ToString()).ToString("#,##0.00");
                this.smmryDataGridView.Rows[rowIdx].Cells[3].Value = dtst.Tables[0].Rows[i][12].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[4].Value = dtst.Tables[0].Rows[i][11].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[5].Value = "-1"; //dtst.Tables[0].Rows[i][0].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[6].Value = dtst.Tables[0].Rows[i][4].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[7].Value = Global.mnFrm.cmCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][5].ToString());
                this.smmryDataGridView.Rows[rowIdx].Cells[8].Value = dtst.Tables[0].Rows[i][6].ToString();

                int chrgAcntID = int.Parse(dtst.Tables[0].Rows[i][7].ToString());
                this.smmryDataGridView.Rows[rowIdx].Cells[9].Value = Global.mnFrm.cmCde.getAccntNum(chrgAcntID) + "." +
                  Global.mnFrm.cmCde.getAccntName(chrgAcntID);
                this.smmryDataGridView.Rows[rowIdx].Cells[10].Value = chrgAcntID.ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[11].Value = "...";

                this.smmryDataGridView.Rows[rowIdx].Cells[12].Value = dtst.Tables[0].Rows[i][8].ToString();
                int balsAcntID = int.Parse(dtst.Tables[0].Rows[i][9].ToString());
                this.smmryDataGridView.Rows[rowIdx].Cells[13].Value = Global.mnFrm.cmCde.getAccntNum(balsAcntID) + "." +
                  Global.mnFrm.cmCde.getAccntName(balsAcntID);
                this.smmryDataGridView.Rows[rowIdx].Cells[14].Value = balsAcntID.ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[15].Value = "...";

                long prepyDocID = long.Parse(dtst.Tables[0].Rows[i][10].ToString());
                this.smmryDataGridView.Rows[rowIdx].Cells[16].Value = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
                  "rcvbls_invc_hdr_id", "rcvbls_invc_number",
                  prepyDocID);
                this.smmryDataGridView.Rows[rowIdx].Cells[17].Value = prepyDocID.ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[18].Value = "...";

                this.smmryDataGridView.Rows[rowIdx].Cells[19].Value = dtst.Tables[0].Rows[i][17].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[20].Value = dtst.Tables[0].Rows[i][18].ToString();

                this.smmryDataGridView.Rows[rowIdx].Cells[21].Value = double.Parse(dtst.Tables[0].Rows[i][19].ToString()).ToString("#,##0.00");
                this.smmryDataGridView.Rows[rowIdx].Cells[22].Value = dtst.Tables[0].Rows[i][14].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[23].Value = dtst.Tables[0].Rows[i][13].ToString();

                this.smmryDataGridView.Rows[rowIdx].Cells[24].Value = double.Parse(dtst.Tables[0].Rows[i][20].ToString()).ToString("#,##0.00");
                this.smmryDataGridView.Rows[rowIdx].Cells[25].Value = dtst.Tables[0].Rows[i][16].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[26].Value = dtst.Tables[0].Rows[i][15].ToString();



                string lnType = dtst.Tables[0].Rows[i][1].ToString();
                string[] acnts = this.getRcvblBalncnAccnt(lnType,
                  int.Parse(dtst.Tables[0].Rows[i][4].ToString()),
                  int.Parse(this.cstmrIDTextBox.Text), -1, this.docTypeComboBox.Text);
                this.smmryDataGridView.Rows[rowIdx].Cells[12].Value = acnts[0];
                this.smmryDataGridView.Rows[rowIdx].Cells[13].Value = Global.mnFrm.cmCde.getAccntNum(int.Parse(acnts[1])) + "." +
                  Global.mnFrm.cmCde.getAccntName(int.Parse(acnts[1]));
                this.smmryDataGridView.Rows[rowIdx].Cells[14].Value = acnts[1];
                this.smmryDataGridView.Rows[rowIdx].Cells[15].Value = "...";
                this.smmryDataGridView.Rows[rowIdx].Cells[8].Value = acnts[2];
            }
            this.obey_evnts = true;
            this.txtChngd = false;
        }

        private void cstmrNmLOVSearch(string srchWrd)
        {
            this.txtChngd = false;
            if (this.addRec == false && this.editRec == false)
            {
                Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }

            if (!this.cstmrNmTextBox.Text.Contains("%"))
            {
                this.cstmrIDTextBox.Text = "-1";
            }

            long cstspplID = long.Parse(this.cstmrIDTextBox.Text);
            long siteID = long.Parse(this.cstmrSiteIDTextBox.Text);
            bool isReadOnly = true;
            if (this.addRec || this.editRec)
            {
                isReadOnly = false;
            }
            Global.mnFrm.cmCde.showCstSpplrDiag(ref cstspplID, ref siteID, true, false, srchWrd,
              "Customer/Supplier Name", false, isReadOnly, Global.mnFrm.cmCde, "Customer");
            this.cstmrIDTextBox.Text = cstspplID.ToString();
            this.cstmrSiteIDTextBox.Text = siteID.ToString();
            this.cstmrNmTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                "scm.scm_cstmr_suplr", "cust_sup_id", "cust_sup_name",
                cstspplID);
            this.cstmrSiteTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                  "scm.scm_cstmr_suplr_sites", "cust_sup_site_id", "site_name",
                  siteID);
            //string[] selVals = new string[1];
            //selVals[0] = this.cstmrIDTextBox.Text;
            //DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
            //  Global.mnFrm.cmCde.getLovID("Customers"), ref selVals,
            //  true, true, Global.mnFrm.cmCde.Org_id,
            // srchWrd, "Both", true);
            //if (dgRes == DialogResult.OK)
            //{
            //  for (int i = 0; i < selVals.Length; i++)
            //  {
            //    this.cstmrIDTextBox.Text = selVals[i];
            //    this.cstmrNmTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
            //      "scm.scm_cstmr_suplr", "cust_sup_id", "cust_sup_name",
            //      long.Parse(selVals[i]));
            //    this.cstmrSiteIDTextBox.Text = "-1";
            //    this.cstmrSiteTextBox.Text = "";

            //  }
            //}
            this.txtChngd = false;
        }

        private void cstmrSiteLOVSearch(string srchWrd)
        {
            this.txtChngd = false;
            if (this.addRec == false && this.editRec == false)
            {
                Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }
            if (this.cstmrIDTextBox.Text == "" || this.cstmrIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("Please pick a Customer Name First!", 0);
                return;
            }
            if (!this.cstmrSiteTextBox.Text.Contains("%"))
            {
                this.cstmrSiteIDTextBox.Text = "-1";
            }

            string[] selVals = new string[1];
            selVals[0] = this.cstmrSiteIDTextBox.Text;
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
              Global.mnFrm.cmCde.getLovID("Customer/Supplier Sites"), ref selVals,
              true, true, int.Parse(this.cstmrIDTextBox.Text),
             srchWrd, "Both", true);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.cstmrSiteIDTextBox.Text = selVals[i];
                    this.cstmrSiteTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                      "scm.scm_cstmr_suplr_sites", "cust_sup_site_id", "site_name",
                      long.Parse(selVals[i]));
                }
            }
            this.txtChngd = false;
        }

        private void pymntMthdLOVSearch(string srchWrd)
        {
            this.txtChngd = false;
            if (this.addRec == false && this.editRec == false)
            {
                Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }

            if (!this.pymntMthdTextBox.Text.Contains("%"))
            {
                this.pymntMthdIDTextBox.Text = "-1";
            }

            string[] selVals = new string[1];
            selVals[0] = this.pymntMthdIDTextBox.Text;
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
              Global.mnFrm.cmCde.getLovID("Payment Methods"), ref selVals,
              true, true, Global.mnFrm.cmCde.Org_id, "Customer Payments", "",
             srchWrd, "Both", true);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.pymntMthdIDTextBox.Text = selVals[i];
                    this.pymntMthdTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                      "accb.accb_paymnt_mthds", "paymnt_mthd_id", "pymnt_mthd_name",
                      int.Parse(selVals[i]));
                }
            }
            this.txtChngd = false;
        }

        private void trnsDteLOVSrch()
        {
            this.txtChngd = false;
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("You must be in ADD/EDIT mode first!", 0);
                return;
            }
            DateTime dte1 = DateTime.Now;
            bool sccs = DateTime.TryParse(this.docDteTextBox.Text, out dte1);
            if (!sccs)
            {
                dte1 = DateTime.Now;
            }
            this.docDteTextBox.Text = dte1.ToString("dd-MMM-yyyy");
            this.txtChngd = false;
        }

        private void amntLOVSrch()
        {
            this.txtChngd = false;
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("You must be in ADD/EDIT mode first!", 0);
                return;
            }
            double lnAmnt = 0;
            double paidAmnt = 0;
            double balsAmnt = 0;

            string orgnlAmnt = this.invcAmntTextBox.Text;
            bool isno = double.TryParse(orgnlAmnt, out lnAmnt);
            if (isno == false)
            {
                lnAmnt = Math.Round(Global.computeMathExprsn(orgnlAmnt), 2);
            }
            double.TryParse(this.amntPaidTextBox.Text, out paidAmnt);
            balsAmnt = lnAmnt - paidAmnt;
            this.invcAmntTextBox.Text = lnAmnt.ToString("#,##0.00");
            this.outstndngBalsTextBox.Text = balsAmnt.ToString("#,##0.00");

            this.txtChngd = false;
            EventArgs e = new EventArgs();
            this.calcSmryButton_Click(this.calcSmryButton, e);
        }

        private void crncyNmLOVSearch(string srchWrd)
        {
            this.txtChngd = false;
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("You must be in ADD/EDIT mode first!", 0);
                return;
            }
            if (this.invcCurrTextBox.Text == "")
            {
                this.invcCurrIDTextBox.Text = this.curid.ToString();
                this.invcCurrTextBox.Text = this.curCode;
                this.txtChngd = false;
                return;
            }
            if (!this.invcCurrTextBox.Text.Contains("%"))
            {
                this.invcCurrIDTextBox.Text = "-1";
            }

            int[] selVals = new int[1];
            selVals[0] = int.Parse(this.invcCurrIDTextBox.Text);
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
             Global.mnFrm.cmCde.getLovID("Currencies"), ref selVals,
             true, true, srchWrd, "Both", true);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.invcCurrIDTextBox.Text = selVals[i].ToString();
                    this.invcCurrTextBox.Text = Global.mnFrm.cmCde.getPssblValNm(selVals[i]);
                }
                this.clearLnsInfo();
            }
            this.txtChngd = false;
        }

        private string[] getRcvblBalncnAccnt(string lineType, int codebhndID, int cstmrID, long prepayDocID, string docType)
        {
            string[] res = { "Increase", /*Balancing Account*/"-1",
                       "Increase", /*Charge Account*/"-1" };

            string cstmrAccntID = "-1";

            if (docType == "Customer Standard Payment"
              || docType == "Customer Advance Payment"
              || docType == "Direct Topup from Customer"
              || docType == "Customer Credit Memo (InDirect Topup)")
            {
                cstmrAccntID = Global.mnFrm.cmCde.getGnrlRecNm(
                   "scm.scm_cstmr_suplr", "cust_sup_id", "dflt_rcvbl_accnt_id",
                    cstmrID);
            }
            else //if (docType == "Direct Refund to Customer")
            {
                cstmrAccntID = Global.mnFrm.cmCde.getGnrlRecNm(
             "scm.scm_cstmr_suplr", "cust_sup_id", "dflt_pybl_accnt_id",
              cstmrID);
            }

            int accntID = -1;
            int.TryParse(cstmrAccntID, out accntID);
            if (accntID <= 0)
            {
                if (docType == "Customer Standard Payment"
                || docType == "Customer Advance Payment"
                || docType == "Direct Topup from Customer"
                || docType == "Customer Credit Memo (InDirect Topup)")
                {
                    int dflACntID = Global.get_DfltRcvblAcnt(Global.mnFrm.cmCde.Org_id);
                    accntID = dflACntID;
                }
                else
                {
                    int dflACntID = Global.get_DfltPyblAcnt(Global.mnFrm.cmCde.Org_id);
                    accntID = dflACntID;
                }
            }
            res[1] = accntID.ToString();
            if (docType == "Customer Standard Payment"
              || docType == "Customer Advance Payment"
              || docType == "Direct Topup from Customer"
              || docType == "Customer Credit Memo (InDirect Topup)")
            {
                if (lineType == "1Initial Amount")
                {
                    res[0] = "Increase";
                    res[2] = "Increase";
                    res[3] = "-1";
                    //res[3] = Global.get_DfltExpnsAcnt(Global.mnFrm.cmCde.Org_id).ToString();
                    return res;
                }
                if (lineType == "2Tax")
                {
                    string taxAccntID = Global.mnFrm.cmCde.getGnrlRecNm(
                  "scm.scm_tax_codes", "code_id", "taxes_payables_accnt_id",
                  codebhndID);
                    string taxExpAccntID = Global.mnFrm.cmCde.getGnrlRecNm(
                  "scm.scm_tax_codes", "code_id", "tax_expense_accnt_id",
                  codebhndID);
                    string isRcvrbl = Global.mnFrm.cmCde.getGnrlRecNm(
                 "scm.scm_tax_codes", "code_id", "is_recovrbl_tax",
                 codebhndID);
                    string isWthHldng = Global.mnFrm.cmCde.getGnrlRecNm(
               "scm.scm_tax_codes", "code_id", "is_withldng_tax",
               codebhndID);
                    res[0] = "Increase";
                    if (isRcvrbl == "1")
                    {
                        res[2] = "Increase";
                        res[3] = taxAccntID;
                    }
                    else if (isWthHldng == "1")
                    {
                        res[0] = "Decrease";
                        res[2] = "Increase";
                        res[3] = taxExpAccntID;
                    }
                    else
                    {
                        //    string taxExpnsAccntID = Global.mnFrm.cmCde.getGnrlRecNm(
                        //"scm.scm_tax_codes", "code_id", "tax_expense_accnt_id",
                        //codebhndID);
                        res[2] = "Increase";
                        res[3] = taxAccntID;
                    }
                    return res;
                }
                if (lineType == "3Discount")
                {
                    //string taxAccntID = Global.mnFrm.cmCde.getGnrlRecNm(
                    // "scm.scm_tax_codes", "code_id", "dscount_expns_accnt_id",
                    // codebhndID);
                    res[0] = "Decrease";
                    res[2] = "Increase";
                    string salesDscntAccntID = Global.mnFrm.cmCde.getGnrlRecNm(
                  "scm.scm_tax_codes", "code_id", "dscount_expns_accnt_id",
                  codebhndID);
                    res[2] = "Increase";
                    res[3] = salesDscntAccntID;

                    return res;
                }
                if (lineType == "4Extra Charge")
                {
                    res[0] = "Increase";
                    string chrgeRvnuAccntID = Global.mnFrm.cmCde.getGnrlRecNm(
                  "scm.scm_tax_codes", "code_id", "chrge_revnu_accnt_id",
                  codebhndID);
                    res[2] = "Increase";
                    res[3] = chrgeRvnuAccntID;
                }
                if (docType == "Customer Standard Payment"
                  || docType == "Direct Topup from Customer")
                {
                    if (lineType == "5Applied Prepayment")
                    {
                        int prepayAccntID = -1;
                        string prepayDocType = Global.mnFrm.cmCde.getGnrlRecNm(
                  "accb.accb_rcvbls_invc_hdr", "rcvbls_invc_hdr_id", "rcvbls_invc_type",
                  prepayDocID);

                        res[0] = "Decrease";
                        res[2] = "Decrease";
                        if (prepayDocType == "Customer Credit Memo (InDirect Topup)")
                        {
                            prepayAccntID = Global.get_RcvblPrepayDocRcvblAcntID(prepayDocID);
                        }
                        else
                        {
                            prepayAccntID = Global.get_RcvblPrepayDocRvnuAcntID(prepayDocID);
                        }
                        res[3] = prepayAccntID.ToString();
                    }
                }
            }
            else
            {
                if (lineType == "1Initial Amount")
                {
                    res[0] = "Increase";
                    res[2] = "Decrease";
                    res[3] = "-1";
                    //res[3] = Global.get_DfltExpnsAcnt(Global.mnFrm.cmCde.Org_id).ToString();
                    return res;
                }
                if (lineType == "2Tax")
                {
                    string taxAccntID = Global.mnFrm.cmCde.getGnrlRecNm(
                  "scm.scm_tax_codes", "code_id", "taxes_payables_accnt_id",
                  codebhndID);
                    string isRcvrbl = Global.mnFrm.cmCde.getGnrlRecNm(
                 "scm.scm_tax_codes", "code_id", "is_recovrbl_tax",
                 codebhndID);
                    string isWthHldng = Global.mnFrm.cmCde.getGnrlRecNm(
               "scm.scm_tax_codes", "code_id", "is_withldng_tax",
               codebhndID);
                    res[0] = "Increase";
                    if (isRcvrbl == "1")
                    {
                        res[2] = "Decrease";
                        res[3] = taxAccntID;
                    }
                    else if (isWthHldng == "1")
                    {
                        res[0] = "Decrease";
                        res[2] = "Increase";
                        res[3] = taxAccntID;
                    }
                    else
                    {
                        //    string taxExpnsAccntID = Global.mnFrm.cmCde.getGnrlRecNm(
                        //"scm.scm_tax_codes", "code_id", "tax_expense_accnt_id",
                        //codebhndID);
                        res[2] = "Decrease";
                        res[3] = taxAccntID;
                    }
                    return res;
                }
                if (lineType == "3Discount")
                {
                    //string taxAccntID = Global.mnFrm.cmCde.getGnrlRecNm(
                    // "scm.scm_tax_codes", "code_id", "dscount_expns_accnt_id",
                    // codebhndID);
                    res[0] = "Decrease";
                    res[2] = "Decrease";
                    string prchsDscntAccntID = Global.mnFrm.cmCde.getGnrlRecNm(
                  "scm.scm_tax_codes", "code_id", "prchs_dscnt_accnt_id",
                  codebhndID);
                    res[2] = "Decrease";
                    res[3] = prchsDscntAccntID;

                    return res;
                }
                if (lineType == "4Extra Charge")
                {
                    res[0] = "Increase";
                    string chrgeRvnuAccntID = Global.mnFrm.cmCde.getGnrlRecNm(
                  "scm.scm_tax_codes", "code_id", "chrge_revnu_accnt_id",
                  codebhndID);
                    res[2] = "Decrease";
                    res[3] = chrgeRvnuAccntID;
                }
                if (docType == "Direct Refund to Customer")
                {
                    if (lineType == "5Applied Prepayment")
                    {
                        int prepayAccntID = Global.get_RcvblPrepayDocRcvblAcntID(prepayDocID);
                        res[0] = "Decrease";
                        res[2] = "Decrease";
                        res[3] = prepayAccntID.ToString();
                    }
                }
            }
            return res;
        }

        private void docClsfctnButton_Click(object sender, EventArgs e)
        {
            this.docClsfctnLOVSearch("%");
        }

        private void pymntMthdButton_Click(object sender, EventArgs e)
        {
            this.pymntMthdLOVSearch("%");
        }

        private void invcCurrButton_Click(object sender, EventArgs e)
        {
            this.crncyNmLOVSearch("%");
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            this.curid = Global.mnFrm.cmCde.getOrgFuncCurID(Global.mnFrm.cmCde.Org_id);
            this.curCode = Global.mnFrm.cmCde.getPssblValNm(this.curid);
            if (this.addRecsCSP == false
               && this.addRecsCAP == false
              && this.addRecsDRTC == false
              && this.addRecsDTFC == false
              && this.addRecsCCMIT == false
              && this.addRecsCDMIR == false)
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }
            string dateStr = Global.mnFrm.cmCde.getFrmtdDB_Date_time();

            double invcAmnt = 1;
            int smplAcntID = Global.get_DfltRcvblAcnt(Global.mnFrm.cmCde.Org_id);
            if (smplAcntID <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Default Receivables Account has not been defined!", 0);
                return;
            }
            else if (!this.isPayTrnsValid(smplAcntID, "I", invcAmnt, dateStr))
            {
                this.rfrshButton_Click(this.rfrshButton, e);
                return;
            }
            this.clearDetInfo();
            this.clearLnsInfo();
            this.addRec = true;
            this.editRec = false;
            this.obey_evnts = false;
            this.docDteTextBox.Text = DateTime.ParseExact(
         Global.mnFrm.cmCde.getDB_Date_time().Substring(0, 10), "yyyy-MM-dd",
         System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
            this.invcCurrTextBox.Text = this.curCode;
            this.invcCurrIDTextBox.Text = this.curid.ToString();
            if (this.invcCurrTextBox.Text == "")
            {
            }
            this.prpareForDetEdit();
            this.addButton.Enabled = false;
            this.editButton.Enabled = false;
            this.prpareForLnsEdit();
            this.obey_evnts = true;
        }

        private bool isPayTrnsValid(int accntID, string incrsDcrs, double amnt, string date1)
        {
            double netamnt = 0;

            netamnt = Global.mnFrm.cmCde.dbtOrCrdtAccntMultiplier(accntID,
         incrsDcrs) * amnt;

            if (!Global.mnFrm.cmCde.isTransPrmttd(
         accntID, date1, netamnt))
            {
                return false;
            }
            return true;
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if ((this.editRecsCSP == false
               && this.docTypeComboBox.Text == "Customer Standard Payment")
               || (this.editRecsCAP == false
               && this.docTypeComboBox.Text == "Customer Advance Payment")
              || (this.editRecsDRTC == false
               && this.docTypeComboBox.Text == "Direct Refund to Customer")
              || (this.editRecsDTFC == false
               && this.docTypeComboBox.Text == "Direct Topup from Customer")
              || (this.editRecsCCMIT == false
               && this.docTypeComboBox.Text == "Customer Credit Memo (InDirect Topup)")
               || (this.editRecsCDMIR == false
               && this.docTypeComboBox.Text == "Customer Debit Memo (InDirect Refund)"))
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }
            if (this.docIDTextBox.Text == "" || this.docIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("No record to Edit!", 0);
                return;
            }
            if (this.apprvlStatusTextBox.Text == "Approved"
              || this.apprvlStatusTextBox.Text == "Initiated"
               || this.apprvlStatusTextBox.Text == "Validated"
              || this.apprvlStatusTextBox.Text == "Cancelled"
              || this.apprvlStatusTextBox.Text.Contains("Reviewed")
              || this.srcDocTypeTextBox.Text.Contains("Sales"))
            {
                Global.mnFrm.cmCde.showMsg("Cannot EDIT Approved, Initiated, " +
                  "Reviewed, Validated and Cancelled Documents \r\n as well as Documents that were created from Other Modules!", 0);
                return;
            }
            this.addRec = false;
            this.editRec = true;
            this.prpareForDetEdit();
            this.editButton.Enabled = false;
            this.addButton.Enabled = false;
            this.prpareForLnsEdit();
        }

        private void delButton_Click(object sender, EventArgs e)
        {
            if ((this.delRecsCSP == false
               && this.docTypeComboBox.Text == "Customer Standard Payment")
               || (this.delRecsCAP == false
               && this.docTypeComboBox.Text == "Customer Advance Payment")
              || (this.delRecsDRTC == false
               && this.docTypeComboBox.Text == "Direct Refund to Customer")
              || (this.delRecsDTFC == false
               && this.docTypeComboBox.Text == "Direct Topup from Customer")
              || (this.delRecsCCMIT == false
               && this.docTypeComboBox.Text == "Customer Credit Memo (InDirect Topup)")
               || (this.delRecsCDMIR == false
               && this.docTypeComboBox.Text == "Customer Debit Memo (InDirect Refund)"))
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }
            if (this.rcvblsDocListView.SelectedItems.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select the Record to Delete!", 0);
                return;
            }
            if (this.apprvlStatusTextBox.Text == "Approved"
              || this.apprvlStatusTextBox.Text == "Initiated"
               || this.apprvlStatusTextBox.Text == "Validated"
              || this.apprvlStatusTextBox.Text == "Cancelled"
              || this.apprvlStatusTextBox.Text.Contains("Reviewed")
              || this.srcDocTypeTextBox.Text.Contains("Sales"))
            {
                Global.mnFrm.cmCde.showMsg("Cannot EDIT Approved, Initiated, " +
                  "Reviewed, Validated and Cancelled Documents \r\n as well as Documents that were created from Other Modules!", 0);
                return;
            }
            if (Global.mnFrm.cmCde.showMsg("Are you sure you want to DELETE the selected Document?" +
           "\r\nThis action cannot be undone!", 1) == DialogResult.No)
            {
                Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                return;
            }
            Global.deleteRcvblsDocHdrNDet(long.Parse(this.docIDTextBox.Text), this.docIDNumTextBox.Text);
            this.rfrshButton_Click(this.rfrshButton, e);
        }

        private void addLineButton_Click(object sender, EventArgs e)
        {
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("You must be in ADD/EDIT mode first!", 0);
                return;
            }
            if ((this.docIDTextBox.Text == "" ||
              this.docIDTextBox.Text == "-1") &&
              this.saveButton.Enabled == false)
            {
                Global.mnFrm.cmCde.showMsg("Please select saved Document First!", 0);
                return;
            }
            if (this.apprvlStatusTextBox.Text == "Approved"
              || this.apprvlStatusTextBox.Text == "Initiated"
               || this.apprvlStatusTextBox.Text == "Validated"
              || this.apprvlStatusTextBox.Text == "Cancelled"
              || this.apprvlStatusTextBox.Text.Contains("Reviewed")
              || this.srcDocTypeTextBox.Text.Contains("Sales"))
            {
                Global.mnFrm.cmCde.showMsg("Cannot EDIT Approved, Initiated, " +
                  "Reviewed, Validated and Cancelled Documents \r\n as well as Documents that were created from Other Modules!", 0);
                return;
            }
            int rndmNum = -1 * Global.mnFrm.cmCde.getRandomInt(99999, 99999999);
            this.createRcvblsDocRows(1, "1Initial Amount", this.docCommentsTextBox.Text, -1, -1, rndmNum);
            this.prpareForLnsEdit();
        }

        public void createRcvblsDocRows(int num, string lnTyp, string lnDesc, int cdeBhnd, long prpayDocID, long initialAmntLineID)
        {
            this.obey_evnts = false;
            int nwIdx = 0;

            for (int i = 0; i < num; i++)
            {
                //this.smmryDataGridView.RowCount += 1;
                //int rowIdx = this.smmryDataGridView.RowCount - 1;
                int rowIdx = this.smmryDataGridView.RowCount;
                if (this.smmryDataGridView.CurrentCell != null)
                {
                    rowIdx = this.smmryDataGridView.CurrentCell.RowIndex + 1;
                }
                this.smmryDataGridView.Rows.Insert(rowIdx, 1);
                this.smmryDataGridView.Rows[rowIdx].Cells[0].Value = lnTyp;// ;
                this.smmryDataGridView.Rows[rowIdx].Cells[1].Value = lnDesc;
                if (prpayDocID > 0)
                {
                    this.smmryDataGridView.Rows[rowIdx].Cells[2].Value = Global.get_RcvblPrepayDocAvlblAmnt(prpayDocID);
                }
                else
                {
                    this.smmryDataGridView.Rows[rowIdx].Cells[2].Value = "0.00";
                }
                this.smmryDataGridView.Rows[rowIdx].Cells[3].Value = this.invcCurrTextBox.Text;
                this.smmryDataGridView.Rows[rowIdx].Cells[4].Value = this.invcCurrIDTextBox.Text;
                if (lnTyp == "1Initial Amount")
                {
                    this.smmryDataGridView.Rows[rowIdx].Cells[5].Value = initialAmntLineID;
                }
                else
                {
                    this.smmryDataGridView.Rows[rowIdx].Cells[5].Value = "-1";
                }
                this.smmryDataGridView.Rows[rowIdx].Cells[6].Value = cdeBhnd;
                if (cdeBhnd > 0 && lnTyp == "2Tax")
                {
                    double dscnt = 0;
                    dscnt = this.sumGridEntrdAmnts("3Discount");
                    double lnAmnt = 0;

                    if (initialAmntLineID == -1)
                    {
                        double grndAmnt = this.sumGridEntrdAmnts("1Initial Amount");
                        lnAmnt = Global.getCodeAmnt(cdeBhnd, grndAmnt - dscnt);
                    }
                    else
                    {
                        double slctdAmnt = this.getGridEntrdAmnts(initialAmntLineID);
                        lnAmnt = Global.getCodeAmnt(cdeBhnd, slctdAmnt - dscnt);
                    }
                    this.smmryDataGridView.Rows[rowIdx].Cells[2].Value = lnAmnt.ToString("#,##0.00");
                    //this.obey_evnts = true;
                    this.smmryDataGridView.Rows[rowIdx].Cells[7].Value = true;
                }
                else
                {
                    this.smmryDataGridView.Rows[rowIdx].Cells[7].Value = false;
                }


                //string lnType = "1Initial Amount";
                string[] acnts = this.getRcvblBalncnAccnt(lnTyp,
                  cdeBhnd,
                  int.Parse(this.cstmrIDTextBox.Text), prpayDocID, this.docTypeComboBox.Text);

                this.smmryDataGridView.Rows[rowIdx].Cells[8].Value = acnts[2];
                this.smmryDataGridView.Rows[rowIdx].Cells[9].Value = Global.mnFrm.cmCde.getAccntNum(int.Parse(acnts[3])) + "." +
                  Global.mnFrm.cmCde.getAccntName(int.Parse(acnts[3]));
                this.smmryDataGridView.Rows[rowIdx].Cells[10].Value = acnts[3];
                this.smmryDataGridView.Rows[rowIdx].Cells[11].Value = "...";

                this.smmryDataGridView.Rows[rowIdx].Cells[12].Value = acnts[0];
                this.smmryDataGridView.Rows[rowIdx].Cells[13].Value = Global.mnFrm.cmCde.getAccntNum(int.Parse(acnts[1])) + "." +
                  Global.mnFrm.cmCde.getAccntName(int.Parse(acnts[1]));
                this.smmryDataGridView.Rows[rowIdx].Cells[14].Value = acnts[1];
                this.smmryDataGridView.Rows[rowIdx].Cells[15].Value = "...";

                this.smmryDataGridView.Rows[rowIdx].Cells[16].Value = Global.mnFrm.cmCde.getGnrlRecNm(
                  "accb.accb_rcvbls_invc_hdr", "rcvbls_invc_hdr_id", "rcvbls_invc_number", prpayDocID);
                this.smmryDataGridView.Rows[rowIdx].Cells[17].Value = prpayDocID;
                this.smmryDataGridView.Rows[rowIdx].Cells[18].Value = "...";

                this.smmryDataGridView.Rows[rowIdx].Cells[19].Value = "0.00";
                this.smmryDataGridView.Rows[rowIdx].Cells[20].Value = "0.00";

                this.smmryDataGridView.Rows[rowIdx].Cells[21].Value = "0.00";
                this.smmryDataGridView.Rows[rowIdx].Cells[22].Value = this.curCode;
                this.smmryDataGridView.Rows[rowIdx].Cells[23].Value = this.curid;

                this.smmryDataGridView.Rows[rowIdx].Cells[24].Value = "0.00";
                this.smmryDataGridView.Rows[rowIdx].Cells[25].Value = this.curCode;
                this.smmryDataGridView.Rows[rowIdx].Cells[26].Value = this.curid;
                if (lnTyp == "2Tax")
                {
                    this.smmryDataGridView.Rows[rowIdx].Cells[27].Value = initialAmntLineID;
                }
                else
                {
                    this.smmryDataGridView.Rows[rowIdx].Cells[27].Value = "-1";
                }
                this.smmryDataGridView.Rows[rowIdx].Cells[28].Value = "1";
                this.smmryDataGridView.Rows[rowIdx].Cells[29].Value = "0";
                this.smmryDataGridView.EndEdit();
                nwIdx = rowIdx;
            }

            for (int i = 0; i < this.smmryDataGridView.Rows.Count; i++)
            {
                this.smmryDataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
            //this.smmryDataGridView.BeginEdit(false);
            this.obey_evnts = true;
            DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(2, nwIdx);
            this.smmryDataGridView_CellValueChanged(this.smmryDataGridView, ex);
            this.smmryDataGridView.ClearSelection();
            this.smmryDataGridView.Focus();
            //System.Windows.Forms.Application.DoEvents();
            this.smmryDataGridView.CurrentCell = this.smmryDataGridView.Rows[nwIdx].Cells[1];
            //System.Windows.Forms.Application.DoEvents();
            this.smmryDataGridView.BeginEdit(true);
            //System.Windows.Forms.Application.DoEvents();
            //SendKeys.Send("{TAB}");
            SendKeys.Send("{HOME}");
            //System.Windows.Forms.Application.DoEvents();
        }

        private void dfltFill(int rwIdx)
        {
            if (this.smmryDataGridView.Rows[rwIdx].Cells[0].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[0].Value = "1Initial Amount";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[1].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[1].Value = string.Empty;
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[2].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[2].Value = "0.00";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[3].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[3].Value = string.Empty;
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[4].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[4].Value = "-1";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[5].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[5].Value = "-1";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[6].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[6].Value = "-1";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[7].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[7].Value = false;
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[8].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[8].Value = "Increase";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[9].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[9].Value = string.Empty;
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[10].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[10].Value = "-1";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[12].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[12].Value = "Increase";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[13].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[13].Value = string.Empty;
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[14].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[14].Value = "-1";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[16].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[16].Value = string.Empty;
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[17].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[17].Value = "-1";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[19].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[19].Value = "0.00";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[20].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[20].Value = "0.00";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[21].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[21].Value = "0.00";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[22].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[22].Value = string.Empty;
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[23].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[23].Value = "-1";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[24].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[24].Value = "0.00";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[25].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[25].Value = string.Empty;
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[26].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[26].Value = "-1";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[27].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[27].Value = "-1";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[28].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[28].Value = "1";
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[29].Value == null)
            {
                this.smmryDataGridView.Rows[rwIdx].Cells[29].Value = "0";
            }
        }

        private void delLineButton_Click(object sender, EventArgs e)
        {
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("You must be in ADD/EDIT mode first!", 0);
                return;
            }

            if (this.smmryDataGridView.CurrentCell != null
         && this.smmryDataGridView.SelectedRows.Count <= 0)
            {
                this.smmryDataGridView.Rows[this.smmryDataGridView.CurrentCell.RowIndex].Selected = true;
            }

            if (this.smmryDataGridView.SelectedRows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select the record to Delete!", 0);
                return;
            }
            if (this.apprvlStatusTextBox.Text == "Approved"
              || this.apprvlStatusTextBox.Text == "Initiated"
               || this.apprvlStatusTextBox.Text == "Validated"
              || this.apprvlStatusTextBox.Text == "Cancelled"
              || this.apprvlStatusTextBox.Text.Contains("Reviewed")
              || this.srcDocTypeTextBox.Text.Contains("Sales"))
            {
                Global.mnFrm.cmCde.showMsg("Cannot EDIT Approved, Initiated, " +
                  "Reviewed, Validated and Cancelled Documents \r\n as well as Documents that were created from Other Modules!", 0);
                return;
            }
            if (Global.mnFrm.cmCde.showMsg("Are you sure you want to DELETE the selected Item?" +
         "\r\nThis action cannot be undone!", 1) == DialogResult.No)
            {
                Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                return;
            }
            bool prv = this.obey_evnts;
            this.obey_evnts = false;
            for (int i = 0; i < this.smmryDataGridView.SelectedRows.Count;)
            {
                long lnID = -1;
                long.TryParse(this.smmryDataGridView.SelectedRows[0].Cells[5].Value.ToString(), out lnID);
                if (lnID > 0)
                {
                    Global.deleteRcvblsDocDet(lnID);
                }
                this.smmryDataGridView.Rows.RemoveAt(this.smmryDataGridView.SelectedRows[0].Index);
            }
            this.obey_evnts = prv;
        }

        private void addTaxButton_Click(object sender, EventArgs e)
        {
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("You must be in ADD/EDIT mode first!", 0);
                return;
            }
            if (this.docTypeComboBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Please select a Document Type First!", 0);
                return;
            }

            if (this.smmryDataGridView.CurrentCell != null
           && this.smmryDataGridView.SelectedRows.Count <= 0)
            {
                this.smmryDataGridView.Rows[this.smmryDataGridView.CurrentCell.RowIndex].Selected = true;
            }

            if (this.smmryDataGridView.SelectedRows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select the record to Apply Tax On!", 0);
                return;
            }
            string lnTyp = this.smmryDataGridView.SelectedRows[0].Cells[0].Value.ToString();
            if (lnTyp != "1Initial Amount")
            {
                Global.mnFrm.cmCde.showMsg("Please select the Appropriate Record to apply this Tax on!", 0);
                return;
            }
            string[] selVals = new string[1];
            selVals[0] = "-1";
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                Global.mnFrm.cmCde.getLovID("Tax Codes"), ref selVals,
                false, true, Global.mnFrm.cmCde.Org_id);
            if (dgRes == DialogResult.OK)
            {
                //Global.deleteSmmryItm(long.Parse(this.docIDTextBox.Text), 
                //  this.docTypeComboBox.Text, "2Tax");
                //getSmmryItemID First
                //function to calc code Amnt &  grand total and basic amnt
                for (int i = 0; i < selVals.Length; i++)
                {
                    string smmryNm = Global.mnFrm.cmCde.getGnrlRecNm(
                      "scm.scm_tax_codes", "code_id", "code_name",
                      int.Parse(selVals[i]));
                    int rndmNum = int.Parse(this.smmryDataGridView.SelectedRows[0].Cells[5].Value.ToString());
                    string oldDesc = this.smmryDataGridView.SelectedRows[0].Cells[1].Value.ToString();
                    string oldAmnt = this.smmryDataGridView.SelectedRows[0].Cells[2].Value.ToString();
                    this.createRcvblsDocRows(1, "2Tax", smmryNm, int.Parse(selVals[i]), -1, rndmNum);
                }
            }
        }

        private void addDscntButton_Click(object sender, EventArgs e)
        {
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("You must be in ADD/EDIT mode first!", 0);
                return;
            }
            if (this.docTypeComboBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Please select a Document Type First!", 0);
                return;
            }

            //int dscntCodeID = -1;
            ////int idx = this.itemsDataGridView.SelectedRows[0].Index;
            //double untPrce = double.Parse(this.itemsDataGridView.SelectedRows[0].Cells[7].Value.ToString());
            //DialogResult dgres = Global.mnFrm.cmCde.showDscntDiag(ref dscntCodeID, untPrce, Global.mnFrm.cmCde);
            ////dscntCodeID > 0 && 
            string[] selVals = new string[1];
            selVals[0] = "-1";
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                Global.mnFrm.cmCde.getLovID("Discount Codes"), ref selVals,
                false, true, Global.mnFrm.cmCde.Org_id);
            if (dgRes == DialogResult.OK)
            {
                //Global.deleteSmmryItm(long.Parse(this.docIDTextBox.Text), 
                //  this.docTypeComboBox.Text, "2Tax");
                //getSmmryItemID First
                //function to calc code Amnt &  grand total and basic amnt
                for (int i = 0; i < selVals.Length; i++)
                {
                    string smmryNm = Global.mnFrm.cmCde.getGnrlRecNm(
                      "scm.scm_tax_codes", "code_id", "code_name",
                      int.Parse(selVals[i]));
                    this.createRcvblsDocRows(1, "3Discount", smmryNm, int.Parse(selVals[i]), -1, -1);
                }
            }
        }

        private void addChrgButton_Click(object sender, EventArgs e)
        {
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("You must be in ADD/EDIT mode first!", 0);
                return;
            }

            if (this.docTypeComboBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Please select a Document Type First!", 0);
                return;
            }
            string[] selVals = new string[1];
            selVals[0] = "-1";
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                Global.mnFrm.cmCde.getLovID("Extra Charges"), ref selVals,
                false, true, Global.mnFrm.cmCde.Org_id);
            if (dgRes == DialogResult.OK)
            {
                //Global.deleteSmmryItm(long.Parse(this.docIDTextBox.Text), 
                //  this.docTypeComboBox.Text, "2Tax");
                //getSmmryItemID First
                //function to calc code Amnt &  grand total and basic amnt
                for (int i = 0; i < selVals.Length; i++)
                {
                    string smmryNm = Global.mnFrm.cmCde.getGnrlRecNm(
                      "scm.scm_tax_codes", "code_id", "code_name",
                      int.Parse(selVals[i]));
                    this.createRcvblsDocRows(1, "4Extra Charge", smmryNm, int.Parse(selVals[i]), -1, -1);
                }
            }
        }

        private void calcSmryButton_Click(object sender, EventArgs e)
        {
            if (this.docIDTextBox.Text != "" && this.docIDTextBox.Text != "-1"
              && this.editRec == false && this.addRec == false)
            {
                this.updateGridCodeAmnts();
                this.reCalcSmmrys(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
                //this.populateSmmry(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
            }
            else
            {
                this.smmryDataGridView.EndEdit();
                System.Windows.Forms.Application.DoEvents();
                this.updateGridCodeAmnts();
            }
        }

        private void smmryDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
            if (e.ColumnIndex == 11)
            {
                string srchWrd = this.smmryDataGridView.Rows[e.RowIndex].Cells[9].Value.ToString();
                if (!srchWrd.Contains("%"))
                {
                    srchWrd = "%" + srchWrd + "%";
                    //this.smmryDataGridView.Rows[e.RowIndex].Cells[4].Value = "-1";
                }

                int lnAccntID = int.Parse(this.smmryDataGridView.Rows[e.RowIndex].Cells[10].Value.ToString());
                bool isReadOnly = false;
                DialogResult dgRes = Global.mnFrm.cmCde.showAcntsDiag(ref lnAccntID, true,
              true, srchWrd, "Account Details", this.autoLoad, isReadOnly, Global.mnFrm.cmCde);
                this.autoLoad = false;
                if (dgRes == DialogResult.OK)
                {
                    this.obey_evnts = false;
                    this.smmryDataGridView.Rows[e.RowIndex].Cells[10].Value = lnAccntID.ToString();
                    //this.smmryDataGridView.Rows[e.RowIndex].Cells[6].Value = 

                    this.smmryDataGridView.Rows[e.RowIndex].Cells[9].Value = Global.mnFrm.cmCde.getAccntNum(lnAccntID) +
              "." + Global.mnFrm.cmCde.getAccntName(lnAccntID);
                    System.Windows.Forms.Application.DoEvents();

                    int accntCurrID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
                    "accb.accb_chart_of_accnts", "accnt_id", "crncy_id", lnAccntID));
                    this.smmryDataGridView.Rows[e.RowIndex].Cells[25].Value = Global.mnFrm.cmCde.getPssblValNm(accntCurrID);
                    this.smmryDataGridView.Rows[e.RowIndex].Cells[26].Value = accntCurrID;

                    string slctdCurrID = this.smmryDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
                    this.smmryDataGridView.Rows[e.RowIndex].Cells[19].Value = Math.Round(
                        Global.get_LtstExchRate(int.Parse(slctdCurrID), this.curid,
                        this.docDteTextBox.Text + " 00:00:00"), 15);
                    this.smmryDataGridView.Rows[e.RowIndex].Cells[20].Value = Math.Round(
                      Global.get_LtstExchRate(int.Parse(slctdCurrID), accntCurrID,
                      this.docDteTextBox.Text + " 00:00:00"), 15);
                    System.Windows.Forms.Application.DoEvents();

                    double funcCurrRate = 0;
                    double accntCurrRate = 0;
                    double entrdAmnt = 0;
                    double.TryParse(this.smmryDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString(), out entrdAmnt);
                    double.TryParse(this.smmryDataGridView.Rows[e.RowIndex].Cells[19].Value.ToString(), out funcCurrRate);
                    double.TryParse(this.smmryDataGridView.Rows[e.RowIndex].Cells[20].Value.ToString(), out accntCurrRate);
                    this.smmryDataGridView.Rows[e.RowIndex].Cells[21].Value = (funcCurrRate * entrdAmnt).ToString("#,##0.00");
                    this.smmryDataGridView.Rows[e.RowIndex].Cells[24].Value = (accntCurrRate * entrdAmnt).ToString("#,##0.00");
                    System.Windows.Forms.Application.DoEvents();
                }
                /*string[] selVals = new string[1];
                selVals[0] = this.smmryDataGridView.Rows[e.RowIndex].Cells[10].Value.ToString();
                DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                  Global.mnFrm.cmCde.getLovID("Transaction Accounts"),
                  ref selVals, true, true, Global.mnFrm.cmCde.Org_id,
                  srchWrd, "Both", this.autoLoad);
                this.autoLoad = false;
                if (dgRes == DialogResult.OK)
                {
                    for (int i = 0; i < selVals.Length; i++)
                    {
                        this.obey_evnts = false;
                        this.smmryDataGridView.Rows[e.RowIndex].Cells[10].Value = selVals[i];
                        //this.smmryDataGridView.Rows[e.RowIndex].Cells[6].Value = 

                        this.smmryDataGridView.Rows[e.RowIndex].Cells[9].Value = Global.mnFrm.cmCde.getAccntNum(int.Parse(selVals[i])) +
                  "." + Global.mnFrm.cmCde.getAccntName(int.Parse(selVals[i]));
                        System.Windows.Forms.Application.DoEvents();

                        int accntCurrID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
                        "accb.accb_chart_of_accnts", "accnt_id", "crncy_id", long.Parse(selVals[i])));
                        this.smmryDataGridView.Rows[e.RowIndex].Cells[25].Value = Global.mnFrm.cmCde.getPssblValNm(accntCurrID);
                        this.smmryDataGridView.Rows[e.RowIndex].Cells[26].Value = accntCurrID;

                        string slctdCurrID = this.smmryDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
                        this.smmryDataGridView.Rows[e.RowIndex].Cells[19].Value = Math.Round(
                            Global.get_LtstExchRate(int.Parse(slctdCurrID), this.curid,
                            this.docDteTextBox.Text + " 00:00:00"), 15);
                        this.smmryDataGridView.Rows[e.RowIndex].Cells[20].Value = Math.Round(
                          Global.get_LtstExchRate(int.Parse(slctdCurrID), accntCurrID,
                          this.docDteTextBox.Text + " 00:00:00"), 15);
                        System.Windows.Forms.Application.DoEvents();

                        double funcCurrRate = 0;
                        double accntCurrRate = 0;
                        double entrdAmnt = 0;
                        double.TryParse(this.smmryDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString(), out entrdAmnt);
                        double.TryParse(this.smmryDataGridView.Rows[e.RowIndex].Cells[19].Value.ToString(), out funcCurrRate);
                        double.TryParse(this.smmryDataGridView.Rows[e.RowIndex].Cells[20].Value.ToString(), out accntCurrRate);
                        this.smmryDataGridView.Rows[e.RowIndex].Cells[21].Value = (funcCurrRate * entrdAmnt).ToString("#,##0.00");
                        this.smmryDataGridView.Rows[e.RowIndex].Cells[24].Value = (accntCurrRate * entrdAmnt).ToString("#,##0.00");
                        System.Windows.Forms.Application.DoEvents();

                    }
                }*/
                //SendKeys.Send("{Tab}"); 
                //SendKeys.Send("{Tab}"); 
                this.smmryDataGridView.EndEdit();
                this.obey_evnts = true;
                this.smmryDataGridView.CurrentCell = this.smmryDataGridView.Rows[e.RowIndex].Cells[2];
            }
            else if (e.ColumnIndex == 18)
            {

            }


            this.obey_evnts = true;
        }

        private void updateExchRates(int rwindx)
        {
            this.obey_evnts = false;
            double funcCurrRate = 0;
            double accntCurrRate = 0;
            double.TryParse(this.smmryDataGridView.Rows[rwindx].Cells[19].Value.ToString(), out funcCurrRate);
            double.TryParse(this.smmryDataGridView.Rows[rwindx].Cells[20].Value.ToString(), out accntCurrRate);

            funcCurrRate = Math.Abs(funcCurrRate);
            accntCurrRate = Math.Abs(accntCurrRate);

            int accntCurrID = int.Parse(this.smmryDataGridView.Rows[rwindx].Cells[26].Value.ToString());
            string slctdCurrID = this.smmryDataGridView.Rows[rwindx].Cells[4].Value.ToString();
            if (funcCurrRate == 0 || (funcCurrRate == 1 && int.Parse(slctdCurrID) != this.curid))
            {
                this.smmryDataGridView.Rows[rwindx].Cells[19].Value = Math.Abs(Math.Round(
                    Global.get_LtstExchRate(int.Parse(slctdCurrID), this.curid,
                    this.docDteTextBox.Text + " 00:00:00"), 15));
            }
            if (accntCurrRate == 0 || (accntCurrRate == 1 && int.Parse(slctdCurrID) != accntCurrID))
            {
                this.smmryDataGridView.Rows[rwindx].Cells[20].Value = Math.Abs(Math.Round(
                  Global.get_LtstExchRate(int.Parse(slctdCurrID), accntCurrID,
                  this.docDteTextBox.Text + " 00:00:00"), 15));
            }
            System.Windows.Forms.Application.DoEvents();

            funcCurrRate = 0;
            accntCurrRate = 0;
            double entrdAmnt = 0;
            double.TryParse(this.smmryDataGridView.Rows[rwindx].Cells[2].Value.ToString(), out entrdAmnt);
            double.TryParse(this.smmryDataGridView.Rows[rwindx].Cells[19].Value.ToString(), out funcCurrRate);
            double.TryParse(this.smmryDataGridView.Rows[rwindx].Cells[20].Value.ToString(), out accntCurrRate);

            funcCurrRate = Math.Abs(funcCurrRate);
            accntCurrRate = Math.Abs(accntCurrRate);
            entrdAmnt = Math.Abs(entrdAmnt);

            this.smmryDataGridView.Rows[rwindx].Cells[21].Value = (funcCurrRate * entrdAmnt).ToString("#,##0.00");
            this.smmryDataGridView.Rows[rwindx].Cells[24].Value = (accntCurrRate * entrdAmnt).ToString("#,##0.00");
            this.smmryDataGridView.EndEdit();
            System.Windows.Forms.Application.DoEvents();
            this.obey_evnts = true;
        }

        private void smmryDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
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
            this.smmryDataGridView.EndEdit();
            if (e.ColumnIndex == 9)
            {
                this.obey_evnts = true;
                DataGridViewCellEventArgs e1 = new DataGridViewCellEventArgs(11, e.RowIndex);
                this.smmryDataGridView.EndEdit();
                this.autoLoad = true;
                this.smmryDataGridView_CellContentClick(this.smmryDataGridView, e1);
                this.autoLoad = false;
            }
            else if (e.ColumnIndex == 7)
            {
                string lineType = this.smmryDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                bool autoCalc = (bool)this.smmryDataGridView.Rows[e.RowIndex].Cells[7].Value;
                int cdeBhnd = int.Parse(this.smmryDataGridView.Rows[e.RowIndex].Cells[6].Value.ToString());
                long srcInitAmnt = long.Parse(this.smmryDataGridView.Rows[e.RowIndex].Cells[27].Value.ToString());
                if (lineType == "2Tax" || lineType == "3Discount" || lineType == "4Extra Charge")
                {
                    if (autoCalc)
                    {
                        double grndAmnt = this.sumGridEntrdAmnts("1Initial Amount");
                        double dscnt = 0;
                        if (lineType == "2Tax")
                        {
                            dscnt = this.sumGridEntrdAmnts("3Discount");
                        }
                        if (srcInitAmnt == -1)
                        {
                            this.smmryDataGridView.Rows[e.RowIndex].Cells[2].Value = Global.getCodeAmnt(cdeBhnd, grndAmnt - dscnt).ToString("#,##0.00");
                        }
                        else
                        {
                            double slctdAmnt = this.getGridEntrdAmnts(srcInitAmnt);
                            this.smmryDataGridView.Rows[e.RowIndex].Cells[2].Value = Global.getCodeAmnt(cdeBhnd, slctdAmnt - dscnt).ToString("#,##0.00");
                        }
                    }
                    else
                    {
                        this.smmryDataGridView.Rows[e.RowIndex].Cells[27].Value = "-1";
                        if (lineType == "2Tax")
                        {
                            this.smmryDataGridView.Rows[e.RowIndex].Cells[2].Value = "0.00";
                        }
                    }
                    this.obey_evnts = false;
                    this.updateExchRates(e.RowIndex);
                }
                else if (lineType == "1Initial Amount" && autoCalc)
                {
                    double grndAmnt = double.Parse(this.invcAmntTextBox.Text) - this.sumGridEntrdAmnts("2Tax") +
                      this.sumGridEntrdAmnts("3Discount") - this.sumGridEntrdAmnts("4Extra Charge");
                    this.smmryDataGridView.Rows[e.RowIndex].Cells[2].Value = grndAmnt.ToString("#,##0.00");

                    this.obey_evnts = false;
                    this.updateExchRates(e.RowIndex);
                }
            }
            else if (e.ColumnIndex == 19)
            {
                double lnAmnt = 0;
                string orgnlAmnt = this.smmryDataGridView.Rows[e.RowIndex].Cells[19].Value.ToString();
                bool isno = double.TryParse(orgnlAmnt, out lnAmnt);
                if (isno == false)
                {
                    lnAmnt = Math.Abs(Math.Round(Global.computeMathExprsn(orgnlAmnt), 15));
                }
                this.smmryDataGridView.Rows[e.RowIndex].Cells[19].Value = Math.Round(lnAmnt, 15);
                double entrdAmnt = 0;
                double.TryParse(this.smmryDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString(), out entrdAmnt);
                this.smmryDataGridView.Rows[e.RowIndex].Cells[21].Value = Math.Abs(entrdAmnt * lnAmnt).ToString("#,##0.00");
                this.obey_evnts = false;
                this.updateExchRates(e.RowIndex);
            }
            else if (e.ColumnIndex == 20)
            {
                double lnAmnt = 0;
                string orgnlAmnt = this.smmryDataGridView.Rows[e.RowIndex].Cells[20].Value.ToString();
                bool isno = double.TryParse(orgnlAmnt, out lnAmnt);
                if (isno == false)
                {
                    lnAmnt = Math.Abs(Math.Round(Global.computeMathExprsn(orgnlAmnt), 15));
                }
                this.smmryDataGridView.Rows[e.RowIndex].Cells[20].Value = Math.Round(lnAmnt, 15);

                double entrdAmnt = 0;
                double.TryParse(this.smmryDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString(), out entrdAmnt);
                this.smmryDataGridView.Rows[e.RowIndex].Cells[24].Value = Math.Abs(entrdAmnt * lnAmnt).ToString("#,##0.00");
                this.obey_evnts = false;
                this.updateExchRates(e.RowIndex);

            }
            else if (e.ColumnIndex == 2)
            {
                double lnAmnt = 0;

                string orgnlAmnt = this.smmryDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                bool isno = double.TryParse(orgnlAmnt, out lnAmnt);
                if (isno == false)
                {
                    lnAmnt = Math.Abs(Math.Round(Global.computeMathExprsn(orgnlAmnt), 2));
                }
                this.smmryDataGridView.Rows[e.RowIndex].Cells[2].Value = lnAmnt.ToString("#,##0.00");


                this.smmryDataGridView.EndEdit();
                System.Windows.Forms.Application.DoEvents();
                this.obey_evnts = false;
                this.updateExchRates(e.RowIndex);
                //this.updateGridCodeAmnts();
            }

            this.obey_evnts = true;
        }

        private double sumGridEntrdAmnts(string lineType)
        {
            double rslt = 0;
            for (int i = 0; i < this.smmryDataGridView.Rows.Count; i++)
            {
                this.dfltFill(i);
                if (lineType == this.smmryDataGridView.Rows[i].Cells[0].Value.ToString())
                {
                    rslt += double.Parse(this.smmryDataGridView.Rows[i].Cells[2].Value.ToString());
                }
            }

            return Math.Round(rslt, 2);
        }

        private double getGridEntrdAmnts(long nwNumID)
        {
            double rslt = 0;
            for (int i = 0; i < this.smmryDataGridView.Rows.Count; i++)
            {
                this.dfltFill(i);
                if (nwNumID == long.Parse(this.smmryDataGridView.Rows[i].Cells[5].Value.ToString()))
                {
                    return double.Parse(this.smmryDataGridView.Rows[i].Cells[2].Value.ToString());
                }
            }

            return rslt;
        }
        private void changeGridInitAmntIDs(long rndmNum, long nwNumID)
        {
            for (int i = 0; i < this.smmryDataGridView.Rows.Count; i++)
            {
                this.dfltFill(i);
                if (rndmNum == int.Parse(this.smmryDataGridView.Rows[i].Cells[27].Value.ToString()))
                {
                    this.smmryDataGridView.Rows[i].Cells[27].Value = nwNumID;
                }
            }
        }

        private double sumGridEntrdAmnts()
        {
            double rslt = 0;
            string lineType = "";
            int cdeBhnd = -1;

            for (int i = 0; i < this.smmryDataGridView.Rows.Count; i++)
            {
                this.dfltFill(i);
                lineType = this.smmryDataGridView.Rows[i].Cells[0].Value.ToString();
                cdeBhnd = int.Parse(this.smmryDataGridView.Rows[i].Cells[6].Value.ToString());
                if (lineType == "3Discount" || Global.isTaxWthHldng(cdeBhnd)
                 || lineType == "5Applied Prepayment")
                {
                    rslt -= double.Parse(this.smmryDataGridView.Rows[i].Cells[2].Value.ToString());
                }
                else
                {
                    rslt += double.Parse(this.smmryDataGridView.Rows[i].Cells[2].Value.ToString());
                }
            }

            return Math.Round(rslt, 2);
        }

        private void updateGridCodeAmnts()
        {
            this.obey_evnts = false;
            this.smmryDataGridView.EndEdit();
            double nwgrndAmnt = 0;
            double grndAmnt = double.Parse(this.invcAmntTextBox.Text);
            int cnt = 0;
            do
            {
                if (cnt > 0)
                {
                    grndAmnt = Math.Round(this.sumGridEntrdAmnts("1Initial Amount"), 2);
                }
                cnt++;
                int rcCntr = this.smmryDataGridView.Rows.Count;
                for (int i = rcCntr - 1; i >= 0; i--)
                {
                    this.dfltFill(i);
                    long curLnID = long.Parse(this.smmryDataGridView.Rows[i].Cells[5].Value.ToString());
                    string lineType = this.smmryDataGridView.Rows[i].Cells[0].Value.ToString();
                    bool autoCalc = (bool)this.smmryDataGridView.Rows[i].Cells[7].Value;
                    int cdeBhnd = int.Parse(this.smmryDataGridView.Rows[i].Cells[6].Value.ToString());

                    long srcInitAmnt = long.Parse(this.smmryDataGridView.Rows[i].Cells[27].Value.ToString());

                    if (lineType == "2Tax" || lineType == "3Discount" || lineType == "4Extra Charge")
                    {
                        if (autoCalc)
                        {
                            double dscnt = 0;
                            if (lineType == "2Tax")
                            {
                                dscnt = Math.Round(this.sumGridEntrdAmnts("3Discount"), 2);
                            }
                            double lnAmnt = 0;
                            if (srcInitAmnt == -1)
                            {
                                lnAmnt = Global.getCodeAmnt(cdeBhnd, grndAmnt - dscnt);
                            }
                            else
                            {
                                double slctdAmnt = this.getGridEntrdAmnts(srcInitAmnt);
                                lnAmnt = Global.getCodeAmnt(cdeBhnd, slctdAmnt - dscnt);
                            }

                            this.smmryDataGridView.Rows[i].Cells[2].Value = lnAmnt.ToString("#,##0.00");

                            double funcCurrRate = 0;
                            double accntCurrRate = 0;
                            double.TryParse(this.smmryDataGridView.Rows[i].Cells[19].Value.ToString(), out funcCurrRate);
                            double.TryParse(this.smmryDataGridView.Rows[i].Cells[20].Value.ToString(), out accntCurrRate);
                            this.smmryDataGridView.Rows[i].Cells[21].Value = (funcCurrRate * lnAmnt).ToString("#,##0.00");
                            this.smmryDataGridView.Rows[i].Cells[24].Value = (accntCurrRate * lnAmnt).ToString("#,##0.00");
                            this.smmryDataGridView.EndEdit();
                            System.Windows.Forms.Application.DoEvents();
                            this.updateExchRates(i);

                            if (this.editRec == true && curLnID > 0)
                            {
                                string lineDesc = this.smmryDataGridView.Rows[i].Cells[1].Value.ToString();
                                double entrdAmnt = double.Parse(this.smmryDataGridView.Rows[i].Cells[2].Value.ToString());
                                int entrdCurrID = int.Parse(this.smmryDataGridView.Rows[i].Cells[4].Value.ToString());
                                int codeBhnd = int.Parse(this.smmryDataGridView.Rows[i].Cells[6].Value.ToString());
                                string docType = this.docTypeComboBox.Text;
                                string incrDcrs1 = this.smmryDataGridView.Rows[i].Cells[8].Value.ToString();
                                int costngID = int.Parse(this.smmryDataGridView.Rows[i].Cells[10].Value.ToString());
                                string incrDcrs2 = this.smmryDataGridView.Rows[i].Cells[12].Value.ToString();
                                int blncgAccntID = int.Parse(this.smmryDataGridView.Rows[i].Cells[14].Value.ToString());
                                long prepayDocHdrID = long.Parse(this.smmryDataGridView.Rows[i].Cells[17].Value.ToString());
                                string vldyStatus = "VALID";
                                long orgnlLnID = -1;
                                int funcCurrID = int.Parse(this.smmryDataGridView.Rows[i].Cells[23].Value.ToString());
                                int accntCurrID = int.Parse(this.smmryDataGridView.Rows[i].Cells[26].Value.ToString());
                                double funcCurrAmnt = double.Parse(this.smmryDataGridView.Rows[i].Cells[21].Value.ToString());
                                double accntCurrAmnt = double.Parse(this.smmryDataGridView.Rows[i].Cells[24].Value.ToString());
                                long initAmntID = long.Parse(this.smmryDataGridView.Rows[i].Cells[27].Value.ToString());
                                double lineQty = double.Parse(this.smmryDataGridView.Rows[i].Cells[28].Value.ToString());
                                double unitPrice = double.Parse(this.smmryDataGridView.Rows[i].Cells[29].Value.ToString());
                                Global.updtRcvblsDocDet(curLnID, long.Parse(this.docIDTextBox.Text), lineType,
                                  lineDesc, entrdAmnt, entrdCurrID, codeBhnd, docType, autoCalc, incrDcrs1,
                                  costngID, incrDcrs2, blncgAccntID, prepayDocHdrID, vldyStatus, orgnlLnID, funcCurrID,
                                  accntCurrID, funcCurrRate, accntCurrRate, funcCurrAmnt, accntCurrAmnt, initAmntID, lineQty, unitPrice);

                            }
                        }
                    }
                    else
                    {
                        if (lineType == "1Initial Amount" && autoCalc)
                        {
                            this.smmryDataGridView.EndEdit();
                            System.Windows.Forms.Application.DoEvents();
                            double initAmnt = 0;
                            if (cnt > 50)
                            {
                                initAmnt = this.sumGridEntrdAmnts() - this.sumGridEntrdAmnts("2Tax") +
                this.sumGridEntrdAmnts("3Discount") - this.sumGridEntrdAmnts("4Extra Charge");
                            }
                            else
                            {
                                initAmnt = double.Parse(this.invcAmntTextBox.Text) - this.sumGridEntrdAmnts("2Tax") +
                        this.sumGridEntrdAmnts("3Discount") - this.sumGridEntrdAmnts("4Extra Charge");
                            }

                            this.smmryDataGridView.Rows[i].Cells[2].Value = initAmnt.ToString("#,##0.00");
                            this.smmryDataGridView.EndEdit();
                            System.Windows.Forms.Application.DoEvents();
                        }

                        this.updateExchRates(i);
                    }
                }
                this.smmryDataGridView.EndEdit();
                if (this.smmryDataGridView.CurrentCell != null)
                {
                    this.smmryDataGridView.CurrentCell = this.smmryDataGridView.Rows[this.smmryDataGridView.CurrentCell.RowIndex].Cells[0];
                }
                System.Windows.Forms.Application.DoEvents();
                this.grndTotalTextBox.Text = this.sumGridEntrdAmnts().ToString("#,##0.00");
                nwgrndAmnt = Math.Round(this.sumGridEntrdAmnts("1Initial Amount"), 2);
            }
            while (Math.Round(Math.Abs(grndAmnt - nwgrndAmnt), 2) > 0.01 && cnt <= 100);
            this.obey_evnts = true;
        }

        private void smmryDataGridView_CurrentCellChanged(object sender, EventArgs e)
        {
            if (this.smmryDataGridView.CurrentCell == null || this.obey_evnts == false)
            {
                return;
            }
            int rwidx = this.smmryDataGridView.CurrentCell.RowIndex;
            int colidx = this.smmryDataGridView.CurrentCell.ColumnIndex;

            if (rwidx < 0 || colidx < 0)
            {
                return;
            }
            bool prv = this.obey_evnts;
            this.obey_evnts = false;
            this.dfltFill(rwidx);
            if (colidx >= 0)
            {
                int acntID = int.Parse(this.smmryDataGridView.Rows[rwidx].Cells[10].Value.ToString());
                this.smmryDataGridView.Rows[rwidx].Cells[9].Value = Global.mnFrm.cmCde.getAccntNum(acntID) +
                "." + Global.mnFrm.cmCde.getAccntName(acntID);

                long prepayID = long.Parse(this.smmryDataGridView.Rows[rwidx].Cells[17].Value.ToString());
                this.smmryDataGridView.Rows[rwidx].Cells[16].Value = Global.mnFrm.cmCde.getGnrlRecNm(
                "accb.accb_rcvbls_invc_hdr", "rcvbls_invc_hdr_id", "rcvbls_invc_number", prepayID);

            }
            this.obey_evnts = true;
        }

        private void applyPrpymntButton_Click(object sender, EventArgs e)
        {
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("You must be in ADD/EDIT mode first!", 0);
                return;
            }
            if (this.docTypeComboBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Please select a Document Type First!", 0);
                return;
            }
            if (this.cstmrIDTextBox.Text == "" || this.cstmrIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("Please select a Customer First!", 0);
                return;
            }
            if (this.docTypeComboBox.Text == "Customer Advance Payment"
              || this.docTypeComboBox.Text == "Customer Credit Memo (InDirect Topup)"
               || this.docTypeComboBox.Text == "Customer Debit Memo (InDirect Refund)")
            {
                Global.mnFrm.cmCde.showMsg("Cannot Apply Prepayments to this Document Type!", 0);
                return;
            }
            string[] selVals = new string[1];
            selVals[0] = "-1";
            string lovNm = "Customer Prepayments";
            if (this.docTypeComboBox.Text == "Direct Refund to Customer")
            {
                lovNm = "Customer Credit Memos";
            }
            string extrWhere = " and (chartonumeric(tbl1.a) NOT IN (Select appld_prepymnt_doc_id FROM accb.accb_rcvbl_amnt_smmrys WHERE src_rcvbl_hdr_id =" + this.docIDTextBox.Text + "))";
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                Global.mnFrm.cmCde.getLovID(lovNm), ref selVals,
                false, true, Global.mnFrm.cmCde.Org_id,
                this.cstmrIDTextBox.Text, this.invcCurrIDTextBox.Text, "%", "Both", false, extrWhere);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    string smmryNm = "Applied Prepayment";
                    this.createRcvblsDocRows(1, "5Applied Prepayment", smmryNm, -1, long.Parse(selVals[i]), -1);
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (!this.checkRqrmnts())
            {
                return;
            }
            this.calcSmryButton_Click(this.calcSmryButton, e);

            if (this.addRec == true)
            {
                Global.createRcvblsDocHdr(Global.mnFrm.cmCde.Org_id, this.docDteTextBox.Text,
                  this.docIDNumTextBox.Text, this.docTypeComboBox.Text,
                  this.docCommentsTextBox.Text, long.Parse(this.srcDocIDTextBox.Text),
                  int.Parse(this.cstmrIDTextBox.Text), int.Parse(this.cstmrSiteIDTextBox.Text),
                  "Not Validated", "Approve", double.Parse(this.invcAmntTextBox.Text),
                  this.pymntTermsTextBox.Text, this.srcDocTypeTextBox.Text,
                  int.Parse(this.pymntMthdIDTextBox.Text), 0, -1,
                  this.cstmrDocNumTextBox.Text, this.docClsfctnTextBox.Text,
                  int.Parse(this.invcCurrIDTextBox.Text), 0,
                  long.Parse(this.rgstrIDTextBox.Text), this.costCtgrTextBox.Text, "Attendance Register");

                this.saveButton.Enabled = false;
                this.addRec = false;
                this.editRec = true;

                System.Windows.Forms.Application.DoEvents();
                this.docIDTextBox.Text = Global.mnFrm.cmCde.getGnrlRecID(
                  "accb.accb_rcvbls_invc_hdr",
                  "rcvbls_invc_number", "rcvbls_invc_hdr_id",
                  this.docIDNumTextBox.Text, Global.mnFrm.cmCde.Org_id).ToString();
                bool prv = this.obey_evnts;
                this.obey_evnts = false;
                ListViewItem nwItem = new ListViewItem(new string[] {
    "New",
    this.docIDNumTextBox.Text,
    this.docIDTextBox.Text,
    this.docTypeComboBox.Text});
                this.rcvblsDocListView.Items.Insert(0, nwItem);
                for (int i = 0; i < this.rcvblsDocListView.SelectedItems.Count; i++)
                {
                    this.rcvblsDocListView.SelectedItems[i].Font = new Font("Tahoma", 8.25f, FontStyle.Regular);
                    this.rcvblsDocListView.SelectedItems[i].Selected = false;
                }
                this.rcvblsDocListView.Items[0].Selected = true;
                this.rcvblsDocListView.Items[0].Font = new Font("Tahoma", 8.25f, FontStyle.Bold); this.rcvblsDocListView.Items[0].Selected = true;

                this.saveGridView();
                this.saveButton.Enabled = true;
                this.editRec = true;
                this.prpareForDetEdit();
                this.prpareForLnsEdit();
                this.obey_evnts = prv;

            }
            else if (this.editRec == true)
            {
                Global.updtRcvblsDocHdr(long.Parse(this.docIDTextBox.Text), this.docDteTextBox.Text,
                  this.docIDNumTextBox.Text, this.docTypeComboBox.Text,
                  this.docCommentsTextBox.Text, long.Parse(this.srcDocIDTextBox.Text),
                  int.Parse(this.cstmrIDTextBox.Text), int.Parse(this.cstmrSiteIDTextBox.Text),
                  "Not Validated", "Approve", double.Parse(this.invcAmntTextBox.Text),
                  this.pymntTermsTextBox.Text, this.srcDocTypeTextBox.Text,
                  int.Parse(this.pymntMthdIDTextBox.Text), 0, -1,
                  this.cstmrDocNumTextBox.Text, this.docClsfctnTextBox.Text,
                  int.Parse(this.invcCurrIDTextBox.Text), 0,
                  long.Parse(this.rgstrIDTextBox.Text), this.costCtgrTextBox.Text, "Attendance Register");

                this.saveButton.Enabled = false;
                this.addRec = false;
                //this.editRec = false;
                System.Windows.Forms.Application.DoEvents();
                this.saveGridView();
                this.saveButton.Enabled = true;
                this.editRec = true;
            }
            //this.rfrshButton_Click(this.rfrshButton, e);
            this.grndTotalTextBox.Text = "0.00";
            this.grndTotalTextBox.Text = Global.getRcvblsDocGrndAmnt(long.Parse(this.docIDTextBox.Text)).ToString("#,##0.00");
        }

        private bool checkRqrmnts()
        {
            if (this.docIDNumTextBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Please enter a Document Number!", 0);
                return false;
            }
            long oldRecID = Global.mnFrm.cmCde.getGnrlRecID(
              "accb.accb_rcvbls_invc_hdr", "rcvbls_invc_number",
              "rcvbls_invc_hdr_id", this.docIDNumTextBox.Text,
                Global.mnFrm.cmCde.Org_id);
            if (oldRecID > 0
             && this.addRec == true)
            {
                Global.mnFrm.cmCde.showMsg("Document Number is already in use in this Organisation!", 0);
                return false;
            }

            if (oldRecID > 0
             && this.editRec == true
             && oldRecID.ToString() !=
             this.docIDTextBox.Text)
            {
                Global.mnFrm.cmCde.showMsg("New Document Number is already in use in this Organisation!", 0);
                return false;
            }
            if (this.cstmrDocNumTextBox.Text == "")
            {
                this.cstmrDocNumTextBox.Text = this.docIDNumTextBox.Text;
            }
            if (this.docTypeComboBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Document Type cannot be empty!", 0);
                return false;
            }
            if (this.docCommentsTextBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Document Description cannot be empty!", 0);
                return false;
            }

            if (this.docDteTextBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Document Date cannot be empty!", 0);
                return false;
            }


            if (this.cstmrIDTextBox.Text == "" || this.cstmrIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("Customer Name cannot be empty!", 0);
                return false;
            }

            if (this.cstmrSiteIDTextBox.Text == "" || this.cstmrSiteIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("Customer Site cannot be empty!", 0);
                return false;
            }

            if (this.pymntMthdTextBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Payment Method cannot be empty!", 0);
                return false;
            }

            if (this.invcAmntTextBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Invoice AMount cannot be empty!", 0);
                return false;
            }

            /*if (this.docClsfctnTextBox.Text == "")
            {
              Global.mnFrm.cmCde.showMsg("Document Classification cannot be empty!", 0);
              return false;
            }*/
            if (this.invcCurrIDTextBox.Text == "" || this.invcCurrIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("Invoice Currency cannot be empty!", 0);
                return false;
            }
            return true;
        }

        private bool checkDtRqrmnts(int rwIdx)
        {
            if (this.smmryDataGridView.Rows[rwIdx].Cells[1].Value == null)
            {
                //Global.mnFrm.cmCde.showMsg("Please select an Item for Row " + (rwIdx + 1), 0);
                return false;
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[1].Value.ToString() == "")
            {
                //Global.mnFrm.cmCde.showMsg("Please select an Item for Row " + (rwIdx + 1), 0);
                return false;
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[14].Value == null)
            {
                //Global.mnFrm.cmCde.showMsg("Please select an Item for Row " + (rwIdx + 1), 0);
                return false;
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[14].Value.ToString() == "-1")
            {
                //Global.mnFrm.cmCde.showMsg("Please select an Item for Row " + (rwIdx + 1), 0);
                return false;
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[10].Value == null)
            {
                //Global.mnFrm.cmCde.showMsg("Please indicate Item Quantity for Row " + (rwIdx + 1), 0);
                return false;
            }
            if (this.smmryDataGridView.Rows[rwIdx].Cells[10].Value.ToString() == "-1")
            {
                //Global.mnFrm.cmCde.showMsg("Please indicate Item Price for Row " + (rwIdx + 1), 0);
                return false;
            }
            double tst = 0;
            double.TryParse(this.smmryDataGridView.Rows[rwIdx].Cells[2].Value.ToString(), out tst);
            if (tst == 0)
            {
                //Global.mnFrm.cmCde.showMsg("Please indicate Item Quantity(above zero) for Row " + (rwIdx + 1), 0);
                return false;
            }
            tst = 0;
            double.TryParse(this.smmryDataGridView.Rows[rwIdx].Cells[19].Value.ToString(), out tst);
            if (tst == 0)
            {
                //Global.mnFrm.cmCde.showMsg("Please indicate Item Price(above zero) for Row " + (rwIdx + 1), 0);
                return false;
            }
            tst = 0;
            double.TryParse(this.smmryDataGridView.Rows[rwIdx].Cells[20].Value.ToString(), out tst);
            if (tst == 0)
            {
                //Global.mnFrm.cmCde.showMsg("Please indicate Item Price(above zero) for Row " + (rwIdx + 1), 0);
                return false;
            }
            return true;
        }

        private void saveGridView()
        {
            int svd = 0;
            //this.saveLabel.Visible = true;
            for (int i = 0; i < this.smmryDataGridView.Rows.Count; i++)
            {
                if (!this.checkDtRqrmnts(i))
                {
                    this.smmryDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 100, 100);
                    continue;
                }
                else
                {
                    //Check if Doc Ln Rec Exists
                    //Create if not else update
                    long curlnID = long.Parse(this.smmryDataGridView.Rows[i].Cells[5].Value.ToString());
                    string lineType = this.smmryDataGridView.Rows[i].Cells[0].Value.ToString();
                    string lineDesc = this.smmryDataGridView.Rows[i].Cells[1].Value.ToString();
                    double entrdAmnt = double.Parse(this.smmryDataGridView.Rows[i].Cells[2].Value.ToString());
                    int entrdCurrID = int.Parse(this.smmryDataGridView.Rows[i].Cells[4].Value.ToString());
                    int codeBhnd = int.Parse(this.smmryDataGridView.Rows[i].Cells[6].Value.ToString());
                    string docType = this.docTypeComboBox.Text;
                    bool autoCalc = (bool)this.smmryDataGridView.Rows[i].Cells[7].Value;
                    string incrDcrs1 = this.smmryDataGridView.Rows[i].Cells[8].Value.ToString();
                    int costngID = int.Parse(this.smmryDataGridView.Rows[i].Cells[10].Value.ToString());
                    string incrDcrs2 = this.smmryDataGridView.Rows[i].Cells[12].Value.ToString();
                    int blncgAccntID = int.Parse(this.smmryDataGridView.Rows[i].Cells[14].Value.ToString());
                    long prepayDocHdrID = long.Parse(this.smmryDataGridView.Rows[i].Cells[17].Value.ToString());
                    string vldyStatus = "VALID";
                    long orgnlLnID = -1;
                    int funcCurrID = int.Parse(this.smmryDataGridView.Rows[i].Cells[23].Value.ToString());
                    int accntCurrID = int.Parse(this.smmryDataGridView.Rows[i].Cells[26].Value.ToString());
                    double funcCurrRate = double.Parse(this.smmryDataGridView.Rows[i].Cells[19].Value.ToString());
                    double accntCurrRate = double.Parse(this.smmryDataGridView.Rows[i].Cells[20].Value.ToString());
                    double funcCurrAmnt = double.Parse(this.smmryDataGridView.Rows[i].Cells[21].Value.ToString());
                    double accntCurrAmnt = double.Parse(this.smmryDataGridView.Rows[i].Cells[24].Value.ToString());
                    long rndmNum = -1;
                    if (lineType == "1Initial Amount")
                    {
                        rndmNum = long.Parse(this.smmryDataGridView.Rows[i].Cells[5].Value.ToString());
                    }
                    else
                    {
                        rndmNum = long.Parse(this.smmryDataGridView.Rows[i].Cells[27].Value.ToString());
                    }
                    double lineQty = double.Parse(this.smmryDataGridView.Rows[i].Cells[28].Value.ToString());
                    double unitPrice = double.Parse(this.smmryDataGridView.Rows[i].Cells[29].Value.ToString());
                    if (curlnID <= 0)
                    {

                        curlnID = Global.getNewRcvblsLnID();
                        if (lineType == "1Initial Amount")
                        {
                            Global.createRcvblsDocDet(curlnID, long.Parse(this.docIDTextBox.Text), lineType,
                          lineDesc, entrdAmnt, entrdCurrID, codeBhnd, docType, autoCalc, incrDcrs1,
                          costngID, incrDcrs2, blncgAccntID, prepayDocHdrID, vldyStatus, orgnlLnID, funcCurrID,
                          accntCurrID, funcCurrRate, accntCurrRate, funcCurrAmnt, accntCurrAmnt, -1, lineQty, unitPrice);
                            this.smmryDataGridView.Rows[i].Cells[5].Value = curlnID;
                            this.smmryDataGridView.EndEdit();
                            if (rndmNum != -1)
                            {
                                this.changeGridInitAmntIDs(rndmNum, curlnID);
                            }
                        }
                        else
                        {
                            Global.createRcvblsDocDet(curlnID, long.Parse(this.docIDTextBox.Text), lineType,
                                                      lineDesc, entrdAmnt, entrdCurrID, codeBhnd, docType, autoCalc, incrDcrs1,
                                                      costngID, incrDcrs2, blncgAccntID, prepayDocHdrID, vldyStatus, orgnlLnID, funcCurrID,
                                                      accntCurrID, funcCurrRate, accntCurrRate, funcCurrAmnt, accntCurrAmnt, rndmNum, lineQty, unitPrice);
                            this.smmryDataGridView.Rows[i].Cells[5].Value = curlnID;
                            this.smmryDataGridView.EndEdit();
                        }
                    }
                    else
                    {
                        if (lineType == "1Initial Amount")
                        {
                            Global.updtRcvblsDocDet(curlnID, long.Parse(this.docIDTextBox.Text), lineType,
                          lineDesc, entrdAmnt, entrdCurrID, codeBhnd, docType, autoCalc, incrDcrs1,
                          costngID, incrDcrs2, blncgAccntID, prepayDocHdrID, vldyStatus, orgnlLnID, funcCurrID,
                          accntCurrID, funcCurrRate, accntCurrRate, funcCurrAmnt, accntCurrAmnt, -1, lineQty, unitPrice);
                        }
                        else
                        {
                            Global.updtRcvblsDocDet(curlnID, long.Parse(this.docIDTextBox.Text), lineType,
                          lineDesc, entrdAmnt, entrdCurrID, codeBhnd, docType, autoCalc, incrDcrs1,
                          costngID, incrDcrs2, blncgAccntID, prepayDocHdrID, vldyStatus, orgnlLnID, funcCurrID,
                          accntCurrID, funcCurrRate, accntCurrRate, funcCurrAmnt, accntCurrAmnt, rndmNum, lineQty, unitPrice);

                        }
                    }
                    svd++;
                    this.smmryDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                    this.smmryDataGridView.EndEdit();
                }
            }
            Global.mnFrm.cmCde.updateDataNoParams(@"UPDATE accb.accb_rcvbl_amnt_smmrys
                                                   SET line_qty=1 WHERE line_qty=0");
            Global.mnFrm.cmCde.updateDataNoParams(@"UPDATE accb.accb_rcvbl_amnt_smmrys
                                                   SET unit_price=(rcvbl_smmry_amnt/line_qty)
                                                 WHERE unit_price=0");
            this.reCalcSmmrys(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
            //this.saveLabel.Visible = false;
            Global.mnFrm.cmCde.showMsg(svd + " Record(s) Saved!", 3);
        }

        public void reCalcSmmrys(long srcDocID, string srcDocType)
        {
            double grndAmnt = Global.getRcvblsDocGrndAmnt(srcDocID);
            //Grand Total
            string smmryNm = "Grand Total";
            long smmryID = Global.getRcvblsSmmryItmID("6Grand Total", -1,
              srcDocID, srcDocType, smmryNm);
            if (smmryID <= 0)
            {
                long curlnID = Global.getNewRcvblsLnID();
                Global.createRcvblsDocDet(curlnID, srcDocID, "6Grand Total",
                  smmryNm, grndAmnt, int.Parse(this.invcCurrIDTextBox.Text),
                  -1, srcDocType, true, "Increase",
                  -1, "Increase", -1, -1, "VALID", -1, -1,
                  -1, 0, 0, 0, 0, -1,1,0);
            }
            else
            {
                Global.updtRcvblsDocDet(smmryID, srcDocID, "6Grand Total",
                  smmryNm, grndAmnt, int.Parse(this.invcCurrIDTextBox.Text),
                  -1, srcDocType, true, "Increase",
                  -1, "Increase", -1, -1, "VALID", -1, -1,
                  -1, 0, 0, 0, 0, -1,1,0);
            }

            //7Total Payments Received
            smmryNm = "Total Payments Made";
            smmryID = Global.getRcvblsSmmryItmID("7Total Payments Made", -1,
              srcDocID, srcDocType, smmryNm);
            double pymntsAmnt = Global.getRcvblsDocTtlPymnts(srcDocID, srcDocType);

            if (smmryID <= 0)
            {
                long curlnID = Global.getNewRcvblsLnID();
                Global.createRcvblsDocDet(curlnID, srcDocID, "7Total Payments Made",
                  smmryNm, pymntsAmnt, int.Parse(this.invcCurrIDTextBox.Text),
                  -1, srcDocType, true, "Increase",
                  -1, "Increase", -1, -1, "VALID", -1, -1,
                  -1, 0, 0, 0, 0, -1,1,0);
            }
            else
            {
                Global.updtRcvblsDocDet(smmryID, srcDocID, "7Total Payments Made",
                  smmryNm, pymntsAmnt, int.Parse(this.invcCurrIDTextBox.Text),
                  -1, srcDocType, true, "Increase",
                  -1, "Increase", -1, -1, "VALID", -1, -1,
                  -1, 0, 0, 0, 0, -1,1,0);
            }
            //MessageBox.Show(pymntsAmnt.ToString("#,##0.00") +"|"+ this.amntPaidTextBox.Text);
            if (pymntsAmnt.ToString("#,##0.00") != this.amntPaidTextBox.Text)
            {
                Global.updtRcvblsHdrAmntPaid(srcDocID, pymntsAmnt);
            }
            //7Total Payments Received
            smmryNm = "Outstanding Balance";
            smmryID = Global.getRcvblsSmmryItmID("8Outstanding Balance", -1,
              srcDocID, srcDocType, smmryNm);
            double outstndngAmnt = grndAmnt - pymntsAmnt;
            if (smmryID <= 0)
            {
                long curlnID = Global.getNewRcvblsLnID();
                Global.createRcvblsDocDet(curlnID, srcDocID, "8Outstanding Balance",
                  smmryNm, outstndngAmnt, int.Parse(this.invcCurrIDTextBox.Text),
                  -1, srcDocType, true, "Increase",
                  -1, "Increase", -1, -1, "VALID", -1, -1,
                  -1, 0, 0, 0, 0, -1,1,0);
            }
            else
            {
                Global.updtRcvblsDocDet(smmryID, srcDocID, "8Outstanding Balance",
                  smmryNm, outstndngAmnt, int.Parse(this.invcCurrIDTextBox.Text),
                  -1, srcDocType, true, "Increase",
                  -1, "Increase", -1, -1, "VALID", -1, -1,
                  -1, 0, 0, 0, 0, -1,1,0);
            }
            //if (long.Parse(this.srcDocIDTextBox.Text) > 0)
            //{
            //  string salesDoctype = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr",
            //    "invc_hdr_id", "invc_type", long.Parse(this.srcDocIDTextBox.Text));
            //  this.reCalcSalesDocSmmrys(long.Parse(this.srcDocIDTextBox.Text), salesDoctype, srcDocID, srcDocType);
            //}
        }

        public bool validateLns(long docHdrID, string docType)
        {
            double invcAmnt = double.Parse(this.invcAmntTextBox.Text);
            int cstmrID = int.Parse(this.cstmrIDTextBox.Text);
            int invcCurrID = int.Parse(this.invcCurrIDTextBox.Text);

            double grndAmnt = Global.getRcvblsDocGrndAmnt(docHdrID);
            int sameprepayCnt = Global.getRcvblsPrepayDocCnt(docHdrID);
            if (Math.Round(invcAmnt, 2) != Math.Round(grndAmnt, 2))
            {
                Global.mnFrm.cmCde.showMsg("Total Invoice Amount must be the Same as the Invoice Grand Total!", 0);
                return false;
            }
            if (sameprepayCnt > 1)
            {
                Global.mnFrm.cmCde.showMsg("Same Prepayment Cannot be Applied More than Once!", 0);
                return false;
            }
            int blcngAccntID = -1;
            int costAccntID = -1;
            for (int i = 0; i < this.smmryDataGridView.Rows.Count; i++)
            {
                this.dfltFill(i);
                string lineTypeNm = this.smmryDataGridView.Rows[i].Cells[0].Value.ToString();

                int codeBhndID = -1;
                int.TryParse(this.smmryDataGridView.Rows[i].Cells[10].Value.ToString(), out codeBhndID);

                long prepayDocID = -1;
                long.TryParse(this.smmryDataGridView.Rows[i].Cells[17].Value.ToString(), out prepayDocID);

                double prepayLnAmnt = -1;
                double.TryParse(this.smmryDataGridView.Rows[i].Cells[2].Value.ToString(), out prepayLnAmnt);
                if (prepayDocID > 0)
                {
                    if (!Global.isRcvblPrepayDocValid(prepayDocID, invcCurrID, cstmrID))
                    {
                        Global.mnFrm.cmCde.showMsg("An Invalid Prepayment has been Applied!", 0);
                        return false;
                    }
                }
                if (lineTypeNm == "5Applied Prepayment")
                {
                    if ((docType == "Customer Advance Payment"
               || docType == "Customer Credit Memo (InDirect Topup)"
               || docType == "Customer Debit Memo (InDirect Refund)"))
                    {
                        Global.mnFrm.cmCde.showMsg("Cannot Apply Prepayments to this Document Type!", 0);
                        return false;
                    }
                    else
                    {
                        double prepayAvlblAmnt = Global.get_RcvblPrepayDocAvlblAmnt(prepayDocID);
                        if (prepayLnAmnt > prepayAvlblAmnt)
                        {
                            Global.mnFrm.cmCde.showMsg("Applied Prepayment Amount Exceeds the \r\nAvailable Amount on the Source Document!", 0);
                            return false;
                        }
                    }
                }

                string incrDcrs1 = this.smmryDataGridView.Rows[i].Cells[8].Value.ToString();
                int accntID1 = -1;
                int.TryParse(this.smmryDataGridView.Rows[i].Cells[10].Value.ToString(), out accntID1);
                string isdbtCrdt1 = Global.mnFrm.cmCde.dbtOrCrdtAccnt(accntID1, incrDcrs1.Substring(0, 1));

                string incrDcrs2 = this.smmryDataGridView.Rows[i].Cells[12].Value.ToString();
                int accntID2 = -1;
                int.TryParse(this.smmryDataGridView.Rows[i].Cells[14].Value.ToString(), out accntID2);

                double lnAmnt = 0;
                double.TryParse(this.smmryDataGridView.Rows[i].Cells[21].Value.ToString(), out lnAmnt);
                if (lnAmnt == 0)
                {
                    Global.mnFrm.cmCde.showMsg("Please Enter an Amount Other than Zero for all Lines!", 0);
                    return false;
                }
                if (accntID1 <= 0 || accntID2 <= 0)
                {
                    Global.mnFrm.cmCde.showMsg("Please provide the Costing and Balancing Account for all Lines!", 0);
                    return false;
                }

                string isdbtCrdt2 = Global.mnFrm.cmCde.dbtOrCrdtAccnt(accntID2, incrDcrs2.Substring(0, 1));
                if (i == 0)
                {
                    blcngAccntID = accntID2;
                    costAccntID = accntID1;
                }

                if (blcngAccntID != accntID2)
                {
                    Global.mnFrm.cmCde.showMsg("Balancing Account must be the Same for all Lines!", 0);
                    return false;
                }

                if (docType == "Customer Advance Payment"
                  && costAccntID != accntID1)
                {
                    Global.mnFrm.cmCde.showMsg("Costing Account must be the Same for all " +
                      "\r\nLines in a Customer Advance Payment Document!", 0);
                    return false;
                }

                string acntType = Global.mnFrm.cmCde.getAccntType(accntID1);

                if (docType == "Customer Advance Payment"
                  && acntType != "L")
                {
                    Global.mnFrm.cmCde.showMsg("Must Increase an Account Payable(Customer Advance Payments Account) for all " +
                      "\r\nLines in a Customer Advance Payment Document!", 0);
                    return false;
                }

                if (isdbtCrdt1.ToUpper() == isdbtCrdt2.ToUpper())
                {
                    if (docType == "Customer Standard Payment"
                      || docType == "Customer Advance Payment"
                      || docType == "Direct Topup from Customer"
                      || docType == "Customer Debit Memo (InDirect Refund)")
                    {
                        if (lineTypeNm == "1Initial Amount")
                        {
                            Global.mnFrm.cmCde.showMsg("Row " + (i + 1).ToString() +
                              ":- Must Increase Revenue or Customer Advance Payment Account!", 0);
                            return false;
                        }
                        if (lineTypeNm == "2Tax")
                        {
                            Global.mnFrm.cmCde.showMsg("Row " + (i + 1).ToString() +
                              ":- Must Increase Sales Tax Expense or Increase/Decrease Taxes Payable Account!", 0);
                            return false;
                        }
                        if (lineTypeNm == "3Discount")
                        {
                            Global.mnFrm.cmCde.showMsg("Row " + (i + 1).ToString() +
                              ":- Must Increase Sales Discounts (Contra Revenue) Account!", 0);
                            return false;
                        }
                        if (lineTypeNm == "4Extra Charge")
                        {
                            Global.mnFrm.cmCde.showMsg("Row " + (i + 1).ToString() +
                              ":- Must Increase Revenue Account!", 0);
                            return false;
                        }
                        if (docType == "Customer Standard Payment"
                  || docType == "Direct Topup from Customer")
                        {
                            if (lineTypeNm == "5Applied Prepayment")
                            {
                                Global.mnFrm.cmCde.showMsg("Row " + (i + 1).ToString() +
                                  ":- Must Decrease Customer Advance Payment Account or Liability Account!", 0);
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (lineTypeNm == "1Initial Amount")
                        {
                            Global.mnFrm.cmCde.showMsg("Row " + (i + 1).ToString() +
                              ":- Must Decrease a Revenue or Customer Advance Payment Account!", 0);
                            return false;
                        }
                        if (lineTypeNm == "2Tax")
                        {
                            Global.mnFrm.cmCde.showMsg("Row " + (i + 1).ToString() +
                              ":- Must Decrease a Sales Tax Expense or Increase/Decrease a Taxes Payable Account!", 0);
                            return false;
                        }
                        if (lineTypeNm == "3Discount")
                        {
                            Global.mnFrm.cmCde.showMsg("Row " + (i + 1).ToString() +
                              ":- Must Increase Sales Discounts (Contra Revenue) Account!", 0);
                            return false;
                        }
                        if (lineTypeNm == "4Extra Charge")
                        {
                            Global.mnFrm.cmCde.showMsg("Row " + (i + 1).ToString() +
                              ":- Must Decrease Revenue Account!", 0);
                            return false;
                        }
                        if (docType == "Direct Refund to Customer")
                        {
                            if (lineTypeNm == "5Applied Prepayment")
                            {
                                Global.mnFrm.cmCde.showMsg("Row " + (i + 1).ToString() +
                                  ":- Must Decrease a Liability Account!", 0);
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool approveRcvblsDoc(long docHdrID)
        {
            /* 1. Create a GL Batch and get all doc lines
             * 2. for each line create costing account transaction
             * 3. create one balancing account transaction using the grand total amount
             * 4. Check if created gl_batch is balanced.
             * 5. if balanced update docHdr else delete the gl batch created and throw error message
             */
            try
            {
                string glBatchName = "ACC_RCVBL-" +
                 DateTime.Parse(Global.mnFrm.cmCde.getFrmtdDB_Date_time()).ToString("yyMMdd-HHmmss")
                          + "-" + Global.mnFrm.cmCde.getRandomInt(10, 100);

                /*Global.mnFrm.cmCde.getDB_Date_time().Substring(11, 8).Replace(":", "").Replace("-", "").Replace(" ", "") + "-" +
          Global.getNewBatchID().ToString().PadLeft(4, '0');*/
                long glBatchID = Global.mnFrm.cmCde.getGnrlRecID("accb.accb_trnsctn_batches",
                  "batch_name", "batch_id", glBatchName, Global.mnFrm.cmCde.Org_id);

                if (glBatchID <= 0)
                {
                    Global.createBatch(Global.mnFrm.cmCde.Org_id, glBatchName,
                      this.docCommentsTextBox.Text + " (" + this.docIDNumTextBox.Text + ")",
                      "Receivables Invoice Document", "VALID", -1, "0");
                }
                else
                {
                    Global.mnFrm.cmCde.showMsg("GL Batch Could not be Created!\r\n Try Again Later!", 0);
                    return false;
                }

                glBatchID = Global.mnFrm.cmCde.getGnrlRecID("accb.accb_trnsctn_batches",
                  "batch_name", "batch_id", glBatchName, Global.mnFrm.cmCde.Org_id);
                int rcvblAccntID = -1;
                string lnDte = this.docDteTextBox.Text + Global.mnFrm.cmCde.getDB_Date_time().Substring(10);
                for (int i = 0; i < this.smmryDataGridView.Rows.Count; i++)
                {
                    this.dfltFill(i);
                    string lineTypeNm = this.smmryDataGridView.Rows[i].Cells[0].Value.ToString();
                    int codeBhndID = -1;
                    int.TryParse(this.smmryDataGridView.Rows[i].Cells[10].Value.ToString(), out codeBhndID);

                    string incrDcrs1 = this.smmryDataGridView.Rows[i].Cells[8].Value.ToString().Substring(0, 1);
                    int accntID1 = -1;
                    int.TryParse(this.smmryDataGridView.Rows[i].Cells[10].Value.ToString(), out accntID1);
                    string isdbtCrdt1 = Global.mnFrm.cmCde.dbtOrCrdtAccnt(accntID1, incrDcrs1.Substring(0, 1));

                    string incrDcrs2 = this.smmryDataGridView.Rows[i].Cells[12].Value.ToString().Substring(0, 1);
                    int accntID2 = -1;
                    int.TryParse(this.smmryDataGridView.Rows[i].Cells[14].Value.ToString(), out accntID2);
                    rcvblAccntID = accntID2;
                    string isdbtCrdt2 = Global.mnFrm.cmCde.dbtOrCrdtAccnt(accntID2, incrDcrs2.Substring(0, 1));

                    double lnAmnt = double.Parse(this.smmryDataGridView.Rows[i].Cells[21].Value.ToString());

                    System.Windows.Forms.Application.DoEvents();

                    double acntAmnt = 0;
                    double.TryParse(this.smmryDataGridView.Rows[i].Cells[24].Value.ToString(), out acntAmnt);
                    double entrdAmnt = 0;
                    double.TryParse(this.smmryDataGridView.Rows[i].Cells[2].Value.ToString(), out entrdAmnt);

                    string lneDesc = (this.smmryDataGridView.Rows[i].Cells[1].Value.ToString() + " (" + this.cstmrNmTextBox.Text + ")").Replace(" ()", "");
                    int entrdCurrID = int.Parse(this.smmryDataGridView.Rows[i].Cells[4].Value.ToString());
                    int funcCurrID = int.Parse(this.smmryDataGridView.Rows[i].Cells[23].Value.ToString());
                    int accntCurrID = int.Parse(this.smmryDataGridView.Rows[i].Cells[26].Value.ToString());
                    double funcCurrRate = double.Parse(this.smmryDataGridView.Rows[i].Cells[19].Value.ToString());
                    double accntCurrRate = double.Parse(this.smmryDataGridView.Rows[i].Cells[20].Value.ToString());

                    if (accntID1 > 0 && (lnAmnt != 0 || acntAmnt != 0) && incrDcrs1 != "" && lneDesc != "")
                    {
                        double netAmnt = (double)Global.dbtOrCrdtAccntMultiplier(accntID1,
                  incrDcrs1) * (double)lnAmnt;


                        if (!Global.mnFrm.cmCde.isTransPrmttd(accntID1, lnDte, netAmnt))
                        {
                            return false;
                        }

                        if (Global.dbtOrCrdtAccnt(accntID1,
                          incrDcrs1) == "Debit")
                        {
                            Global.createTransaction(accntID1,
                              lneDesc, lnAmnt,
                              lnDte, funcCurrID, glBatchID, 0.00,
                              netAmnt, entrdAmnt, entrdCurrID, acntAmnt, accntCurrID, funcCurrRate, accntCurrRate, "D", "");
                        }
                        else
                        {
                            Global.createTransaction(accntID1,
                              lneDesc, 0.00,
                              lnDte, funcCurrID,
                              glBatchID, lnAmnt, netAmnt,
                      entrdAmnt, entrdCurrID, acntAmnt, accntCurrID, funcCurrRate, accntCurrRate, "C", "");
                        }
                    }
                }
                //Receivable Balancing Leg

                int accntCurrID1 = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
            "accb.accb_chart_of_accnts", "accnt_id", "crncy_id", rcvblAccntID));

                string slctdCurrID = this.invcCurrIDTextBox.Text;
                double funcCurrRate1 = Math.Round(
            Global.get_LtstExchRate(int.Parse(slctdCurrID), this.curid, lnDte), 15);
                double accntCurrRate1 = Math.Round(
                  Global.get_LtstExchRate(int.Parse(slctdCurrID), accntCurrID1, lnDte), 15);
                System.Windows.Forms.Application.DoEvents();

                double grndAmnt = Global.getRcvblsDocGrndAmnt(docHdrID);

                double funcCurrAmnt = Global.getRcvblsDocFuncAmnt(docHdrID);// (funcCurrRate1 * grndAmnt);
                double accntCurrAmnt = (accntCurrRate1 * grndAmnt);
                System.Windows.Forms.Application.DoEvents();

                double netAmnt1 = (double)Global.dbtOrCrdtAccntMultiplier(rcvblAccntID,
            "I") * (double)funcCurrAmnt;


                if (!Global.mnFrm.cmCde.isTransPrmttd(rcvblAccntID, lnDte, netAmnt1))
                {
                    return false;
                }

                if (Global.dbtOrCrdtAccnt(rcvblAccntID,
                  "I") == "Debit")
                {
                    Global.createTransaction(rcvblAccntID,
                      (this.docCommentsTextBox.Text +
                      " (Balacing Leg for Receivables Doc:-" +
                      this.docIDNumTextBox.Text + ")" + " (" + this.cstmrNmTextBox.Text + ")").Replace(" ()", ""), funcCurrAmnt,
                      lnDte, this.curid, glBatchID, 0.00,
                      netAmnt1, grndAmnt, int.Parse(this.invcCurrIDTextBox.Text),
                      accntCurrAmnt, accntCurrID1, funcCurrRate1, accntCurrRate1, "D", "");
                }
                else
                {
                    Global.createTransaction(rcvblAccntID,
                      (this.docCommentsTextBox.Text +
                      " (Balacing Leg for Receivables Doc:-" +
                      this.docIDNumTextBox.Text + ")" + " (" + this.cstmrNmTextBox.Text + ")").Replace(" ()", ""), 0.00,
                      lnDte, this.curid,
                      glBatchID, funcCurrAmnt, netAmnt1,
               grndAmnt, int.Parse(this.invcCurrIDTextBox.Text), accntCurrAmnt,
               accntCurrID1, funcCurrRate1, accntCurrRate1, "C", "");
                }
                if (Global.get_Batch_CrdtSum(glBatchID) == Global.get_Batch_DbtSum(glBatchID))
                {
                    Global.updtRcvblsDocGLBatch(docHdrID, glBatchID);
                    this.updateAppldPrepayHdrs();
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
                Global.mnFrm.cmCde.showMsg("Document Approval Failed!\r\n" + ex.Message, 0);
                return false;
            }
        }

        private void updateAppldPrepayHdrs()
        {
            for (int i = 0; i < this.smmryDataGridView.Rows.Count; i++)
            {
                this.dfltFill(i);
                string lineTypeNm = this.smmryDataGridView.Rows[i].Cells[0].Value.ToString();
                long prepayDocID = -1;
                long.TryParse(this.smmryDataGridView.Rows[i].Cells[17].Value.ToString(), out prepayDocID);

                double lnAmnt = double.Parse(this.smmryDataGridView.Rows[i].Cells[2].Value.ToString());
                if (prepayDocID > 0 && lineTypeNm == "5Applied Prepayment")
                {
                    Global.updtRcvblsDocAmntAppld(prepayDocID, lnAmnt);
                }
                string pepyDocType = Global.mnFrm.cmCde.getGnrlRecNm(
            "accb.accb_rcvbls_invc_hdr", "rcvbls_invc_hdr_id", "rcvbls_invc_type",
            prepayDocID);
                if (pepyDocType == "Customer Credit Memo (InDirect Topup)"
                  || pepyDocType == "Customer Debit Memo (InDirect Refund)")
                {
                    Global.updtRcvblsDocAmntPaid(prepayDocID, lnAmnt);
                }
            }
        }

        private void nxtApprvlStatusButton_Click(object sender, EventArgs e)
        {
            if (this.docIDTextBox.Text == "" || this.docIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("Please select a Saved Document First!", 0);
                return;
            }
            if (this.srcDocTypeTextBox.Text.Contains("Sales"))
            {
                Global.mnFrm.cmCde.showMsg("Documents from Other Modules cannot be Approved/Cancelled from Here!", 0);
                return;
            }
            if (this.smmryDataGridView.Rows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("The Document has no Lines hence cannot be Validated!", 0);
                return;
            }
            if (!Global.mnFrm.cmCde.isTransPrmttd(
                    Global.mnFrm.cmCde.get_DfltCashAcnt(Global.mnFrm.cmCde.Org_id),
                    this.docDteTextBox.Text + " 00:00:00", 200))
            {
                return;
            }
            if (this.nxtApprvlStatusButton.Text == "Approve")
            {
                double invcAmnt = double.Parse(this.invcAmntTextBox.Text);
                double grndAmnt = Global.getRcvblsDocGrndAmnt(long.Parse(this.docIDTextBox.Text));
                if (invcAmnt != grndAmnt)
                {
                    if (Global.mnFrm.cmCde.showMsg("Total Invoice Amount must be the Same as the Invoice Grand Total!\r\n\r\n" +
                      "Do you want to Overwrite the Current Total Invoice Amount (" + this.invcAmntTextBox.Text +
                      ")\r\n with the System GrandTotal (" + grndAmnt.ToString("#,##0.00") + ")", 1) == DialogResult.No)
                    {
                        return;
                    }
                    if ((this.addRec == false && this.editRec == false)
                      && this.editRecsCSP)
                    {
                        this.editButton.PerformClick();
                    }
                    Global.updtRcvblsDocAmnt(long.Parse(this.docIDTextBox.Text), grndAmnt);
                    this.invcAmntTextBox.Text = grndAmnt.ToString("#,##0.00");
                    if ((this.addRec == false && this.editRec == false)
                     && this.editRecsCSP)
                    {
                        this.txtChngd = true;
                        this.docDteTextBox_Leave(this.invcAmntTextBox, e);
                        this.txtChngd = false;
                    }
                }
                this.disableDetEdit();
                this.disableLnsEdit();
                this.populateDet(long.Parse(this.docIDTextBox.Text));
                this.populateLines(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
            }
            if (this.nxtApprvlStatusButton.Text == "Approve")
            {
                if (this.rvwApprvDocs == false)
                {
                    Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                        " this action!\nContact your System Administrator!", 0);
                    return;
                }
                if (!this.validateLns(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text))
                {
                    return;
                }
                if (Global.mnFrm.cmCde.showMsg("Are you sure you want to APPROVE the selected Document?" +
                "\r\nThis action cannot be undone!", 1) == DialogResult.No)
                {
                    // Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                    return;
                }
                //Do Accounting Transactions
                this.waitLabel.Visible = true;
                System.Windows.Forms.Application.DoEvents();

                if (this.validateLns(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text))
                {
                    if (this.approveRcvblsDoc(long.Parse(this.docIDTextBox.Text)))
                    {
                        Global.updtRcvblsDocApprvl(long.Parse(this.docIDTextBox.Text), "Approved", "Cancel");
                    }
                }
                this.waitLabel.Visible = false;
                System.Windows.Forms.Application.DoEvents();
            }
            else if (this.nxtApprvlStatusButton.Text == "Cancel")
            {
                //Global.mnFrm.cmCde.showMsg("Not Yet Implemented !", 3);
                //return;
                if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[67]) == false)
                {
                    Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                      " this action!\nContact your System Administrator!", 0);
                    //this.saveLabel.Visible = false;
                    return;
                }
                if (this.srcDocIDTextBox.Text != "" && this.srcDocIDTextBox.Text != "-1")
                {
                    Global.mnFrm.cmCde.showMsg("Please cancel this document from " +
                      "\r\nthe linked Source Document (" + this.srcDocNumTextBox.Text + ") instead!", 0);
                    //this.saveLabel.Visible = false;
                    return;
                }
                //if (Global.mnFrm.cmCde.showMsg("Are you sure you want to CANCEL the selected Document?" +
                //"\r\nThis action cannot be undone!", 1) == DialogResult.No)
                //{
                // //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                // return;
                //}
                //Check if unreversed Payments Exists then disallow else allow
                //and reverse accounting Transactions
                long rcvblHdrID = long.Parse(this.docIDTextBox.Text);
                string rcvblDoctype = this.docTypeComboBox.Text;
                double pymntsAmnt = Math.Round(Global.getRcvblsDocTtlPymnts(rcvblHdrID, rcvblDoctype), 2);
                double amntAppldEslwhr = Global.get_RcvblPrepayDocAppldAmnt(rcvblHdrID);
                long getPrpyDcUsgsCnt = Global.get_RcvblPrepayDocUsages(rcvblHdrID, rcvblDoctype);
                //double amntAppldEslwhr = 0;//invc_amnt_appld_elswhr
                if (pymntsAmnt != 0)
                {
                    Global.mnFrm.cmCde.showMsg("Please Reverse all Payments on this Document First!" +
                     "\r\n(TOTAL AMOUNT PAID=" + pymntsAmnt.ToString("#,##0.00") + ")", 0);
                    return;
                }
                if (getPrpyDcUsgsCnt > 0)
                {
                    Global.mnFrm.cmCde.showMsg("Please Release this Document from all Other Documents it has been applied to First!", 0);
                    return;
                }
                if (Global.mnFrm.cmCde.showMsg("Are you sure you want to CANCEL the selected Document?" +
                "\r\nThis action cannot be undone!", 1) == DialogResult.No)
                {
                    //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                    //this.saveLabel.Visible = false;
                    Cursor.Current = Cursors.Default;
                    return;
                }
                //this.saveLabel.Text = "CANCELLING DOCUMENT....PLEASE WAIT....";
                //this.saveLabel.Visible = true;
                Cursor.Current = Cursors.WaitCursor;

                System.Windows.Forms.Application.DoEvents();

                this.nxtApprvlStatusButton.Enabled = false;
                System.Windows.Forms.Application.DoEvents();

                string dateStr = Global.mnFrm.cmCde.getFrmtdDB_Date_time();
                bool sccs = true;
                if (sccs)
                {
                    sccs = this.voidAttachedBatch(rcvblHdrID, rcvblDoctype);
                }
                if (sccs)
                {
                    Global.updtRcvblsDocApprvl(long.Parse(this.docIDTextBox.Text), "Cancelled", "None");
                }
            }
            this.populateDet(long.Parse(this.docIDTextBox.Text));
            //this.rfrshDtButton_Click(this.rfrshDtButton, e);
        }

        private void rvrsAppldPrepayHdrs()
        {
            for (int i = 0; i < this.smmryDataGridView.Rows.Count; i++)
            {
                this.dfltFill(i);
                string lineTypeNm = this.smmryDataGridView.Rows[i].Cells[0].Value.ToString();
                long prepayDocID = -1;
                long.TryParse(this.smmryDataGridView.Rows[i].Cells[17].Value.ToString(), out prepayDocID);

                double lnAmnt = double.Parse(this.smmryDataGridView.Rows[i].Cells[2].Value.ToString());
                if (prepayDocID > 0 && lineTypeNm == "5Applied Prepayment")
                {
                    Global.updtRcvblsDocAmntAppld(prepayDocID, -1 * lnAmnt);
                }
                string pepyDocType = Global.mnFrm.cmCde.getGnrlRecNm(
            "accb.accb_rcvbls_invc_hdr", "rcvbls_invc_hdr_id", "rcvbls_invc_type",
            prepayDocID);
                if (pepyDocType == "Customer Credit Memo (InDirect Topup)"
                  || pepyDocType == "Customer Debit Memo (InDirect Refund)")
                {
                    Global.updtRcvblsDocAmntPaid(prepayDocID, -1 * lnAmnt);
                }
            }
        }

        private bool voidAttachedBatch(long rcvblHdrID, string rcvblDocType)
        {
            try
            {
                long glbatchID = long.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
            "accb.accb_rcvbls_invc_hdr", "rcvbls_invc_hdr_id", "gl_batch_id", rcvblHdrID));
                //     string glbatchstatus = Global.mnFrm.cmCde.getGnrlRecNm(
                //"accb.accb_trnsctn_batches", "batch_id", "batch_status", glbatchID);
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
                        //Global.mnFrm.cmCde.showMsg("This batch has been reversed before\r\n Operation Cancelled!", 4);
                        return true;
                    }
                }
                string glNwBatchName = glbatchNm + " (Receivables Document Cancellation@" + dateStr + ")";
                long nwbatchid = Global.mnFrm.cmCde.getGnrlRecID("accb.accb_trnsctn_batches",
                  "batch_name", "batch_id", glNwBatchName, Global.mnFrm.cmCde.Org_id);

                if (nwbatchid <= 0)
                {
                    Global.createBatch(Global.mnFrm.cmCde.Org_id,
                     glNwBatchName,
                     glbatchDesc + " (Receivables Document Cancellation@" + dateStr + ")",
                     "Receivables Invoice",
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
                    Global.createTransaction(int.Parse(dtst.Tables[0].Rows[i][9].ToString()),
                    dtst.Tables[0].Rows[i][3].ToString() + " (Receivables Document Cancellation)", -1 * double.Parse(dtst.Tables[0].Rows[i][4].ToString()),
                    dtst.Tables[0].Rows[i][6].ToString(), int.Parse(dtst.Tables[0].Rows[i][7].ToString()),
                    nwbatchid, -1 * double.Parse(dtst.Tables[0].Rows[i][5].ToString()),
                    -1 * double.Parse(dtst.Tables[0].Rows[i][10].ToString()),
               -1 * double.Parse(dtst.Tables[0].Rows[i][12].ToString()),
               int.Parse(dtst.Tables[0].Rows[i][13].ToString()),
               -1 * double.Parse(dtst.Tables[0].Rows[i][14].ToString()),
               int.Parse(dtst.Tables[0].Rows[i][15].ToString()),
               double.Parse(dtst.Tables[0].Rows[i][16].ToString()),
               double.Parse(dtst.Tables[0].Rows[i][17].ToString()),
               dtst.Tables[0].Rows[i][18].ToString(), "");
                }
                //}
                Global.updateBatchAvlblty(nwbatchid, "1");
                this.rvrsAppldPrepayHdrs();
                return true;
            }
            catch (Exception ex)
            {
                Global.mnFrm.cmCde.showMsg(ex.InnerException.ToString(), 0);
                return false;
            }
        }

        private void srcDocButton_Click(object sender, EventArgs e)
        {
            this.srcDocNumLOVSrch("%");
        }

        private void makePaymentButton_Click(object sender, EventArgs e)
        {
            bool dsablPayments = false;
            bool createPrepay = false;
            if (this.apprvlStatusTextBox.Text == "Cancelled")
            {
                Global.mnFrm.cmCde.showMsg("Cannot Take Deposits on a Cancelled Document!", 0);
                return;
            }
            if (this.apprvlStatusTextBox.Text != "Approved")
            {
                createPrepay = true;
                //Global.mnFrm.cmCde.showMsg("Only Approved documents can be Paid for!", 0);
                //return;
            }
            if (double.Parse(this.outstndngBalsTextBox.Text) == 0)
            {
                dsablPayments = true;
            }
            if (this.payDocs == false)
            {
                dsablPayments = true;
            }
            DialogResult dgres = Global.mnFrm.cmCde.showPymntDiag(
              createPrepay, dsablPayments, this.makePaymentButton.Location.X - 10,
             this.makePaymentButton.Location.Y - 10,
             double.Parse(this.outstndngBalsTextBox.Text), int.Parse(this.invcCurrIDTextBox.Text),
             int.Parse(this.pymntMthdIDTextBox.Text), "Customer Payments",
             int.Parse(this.cstmrIDTextBox.Text), int.Parse(this.cstmrSiteIDTextBox.Text), long.Parse(this.docIDTextBox.Text),
             this.docTypeComboBox.Text, Global.mnFrm.cmCde);

            this.calcSmryButton_Click(this.calcSmryButton, e);
            this.populateDet(long.Parse(this.docIDTextBox.Text));
            this.populateLines(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);

            if (createPrepay)
            {
                this.rfrshButton.PerformClick();
            }
        }

        private void pymntHstryButton_Click(object sender, EventArgs e)
        {
            DialogResult dgres = Global.mnFrm.cmCde.showPymntDiag(false, true, this.makePaymentButton.Location.X - 10,
             this.makePaymentButton.Location.Y - 10,
             double.Parse(this.outstndngBalsTextBox.Text), int.Parse(this.invcCurrIDTextBox.Text),
             int.Parse(this.pymntMthdIDTextBox.Text), "Customer Payments",
             int.Parse(this.cstmrIDTextBox.Text), int.Parse(this.cstmrSiteIDTextBox.Text), long.Parse(this.docIDTextBox.Text),
             this.docTypeComboBox.Text, Global.mnFrm.cmCde);

            this.calcSmryButton_Click(this.calcSmryButton, e);
            this.populateDet(long.Parse(this.docIDTextBox.Text));
            this.populateLines(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
        }

        private void resetTrnsButton_Click(object sender, EventArgs e)
        {
            this.searchInComboBox.SelectedIndex = 3;
            this.searchForTextBox.Text = "%";
            this.dsplySizeComboBox.Text = Global.mnFrm.cmCde.get_CurPlcy_Mx_Dsply_Recs().ToString();

            this.rec_cur_indx = 0;
            this.obey_evnts = false;
            this.showUnpaidCheckBox.Checked = false;
            this.obey_evnts = true;
            this.rfrshButton_Click(this.rfrshButton, e);
        }

        private void searchForTextBox_Click(object sender, EventArgs e)
        {
            this.searchForTextBox.SelectAll();
        }

        private void searchForTextBox_Enter(object sender, EventArgs e)
        {
            this.searchForTextBox.SelectAll();
        }

        private void rcvblsDocsForm_KeyDown(object sender, KeyEventArgs e)
        {
            EventArgs ex = new EventArgs();

            if (e.Control && e.KeyCode == Keys.S)
            {
                if (this.saveButton.Enabled == true)
                {
                    this.saveButton_Click(this.saveButton, ex);
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.N)
            {
                if (this.addButton.Enabled == true)
                {
                    this.addButton_Click(this.addButton, ex);
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.E)
            {
                if (this.editButton.Enabled == true)
                {
                    this.editButton_Click(this.editButton, ex);
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.R)
            {
                this.resetTrnsButton.PerformClick();
            }
            else if ((e.Control && e.KeyCode == Keys.F) || e.KeyCode == Keys.F5)
            {
                if (this.rfrshButton.Enabled == true)
                {
                    this.rfrshButton_Click(this.rfrshButton, ex);
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.Delete)
            {
                if (this.delButton.Enabled == true)
                {
                    this.delButton_Click(this.delButton, ex);
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else
            {
                e.Handled = false;
                e.SuppressKeyPress = false;
                if (this.rcvblsDocListView.Focused)
                {
                    Global.mnFrm.cmCde.listViewKeyDown(this.rcvblsDocListView, e);
                }
            }
        }

        private void showUnapprvdCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.shdObeyEvts())
            {
                this.rfrshButton_Click(this.rfrshButton, e);
            }
        }
        #endregion

        private void docsUsngThisButton_Click(object sender, EventArgs e)
        {
            string[] selVals = new string[1];
            selVals[0] = "-1";
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
              Global.mnFrm.cmCde.getLovID("Receivables Docs. with Prepayments Applied"), ref selVals,
              true, false, 1, this.docIDTextBox.Text, this.docTypeComboBox.Text,
             "%", "Both", false);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    if (selVals[i] != "-1")
                    {
                        this.searchInComboBox.SelectedItem = "Document Number";
                        this.searchForTextBox.Text = selVals[i];
                        this.loadPanel();
                    }
                }
            }
        }

        private void rejectDocButton_Click(object sender, EventArgs e)
        {
            if (this.saveButton.Enabled == true)
            {
                Global.mnFrm.cmCde.showMsg("Please disable Editing of the Document First!", 0);
                return;
            }
            if (this.srcDocTypeTextBox.Text.Contains("Sales"))
            {
                Global.mnFrm.cmCde.showMsg("Cannot Work on Documents that were created from Other Modules From Here!", 0);
                return;
            }
            if (Global.mnFrm.cmCde.showMsg("Are you sure you want to REJECT the selected Document?" +
         "\r\nThis action cannot be undone!", 1) == DialogResult.No)
            {
                //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                return;
            }
            this.rejectDocButton.Enabled = false;
            System.Windows.Forms.Application.DoEvents();

            //Global.updtActnPrcss(7);//Invetory Import Process

            bool sccs = true;// this.rvrsApprval(Global.mnFrm.cmCde.getFrmtdDB_Date_time());
            if (sccs)
            {
                Global.updtRcvblsDocApprvl(long.Parse(this.docIDTextBox.Text), "Not Validated", "Approve");
            }
            this.rejectDocButton.Enabled = true;
            System.Windows.Forms.Application.DoEvents();
            if (this.rcvblsDocListView.SelectedItems.Count > 0)
            {
                this.populateDet(long.Parse(this.rcvblsDocListView.SelectedItems[0].SubItems[2].Text));
                this.populateLines(long.Parse(this.rcvblsDocListView.SelectedItems[0].SubItems[2].Text),
                    this.rcvblsDocListView.SelectedItems[0].SubItems[3].Text);
            }
        }

        private void exptExclBlsMenuItem_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.exprtToExcel(this.rcvblsDocListView);
        }

        private void vwSQLBlsMenuItem_Click(object sender, EventArgs e)
        {
            this.vwSQLButton_Click(this.vwSQLButton, e);
        }

        private void viewAtchmntsButton_Click(object sender, EventArgs e)
        {
            if (this.docIDTextBox.Text == "" ||
         this.docIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("Please select a saved Firm First!", 0);
                return;
            }
            attchmntsDiag nwDiag = new attchmntsDiag();
            if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[15]) == false)
            {
                nwDiag.addButton.Enabled = false;
                nwDiag.addButton.Visible = false;
                nwDiag.editButton.Enabled = false;
                nwDiag.editButton.Visible = false;
                nwDiag.delButton.Enabled = false;
                nwDiag.delButton.Visible = false;
            }
            nwDiag.prmKeyID = long.Parse(this.docIDTextBox.Text);
            nwDiag.fldrNm = Global.mnFrm.cmCde.getRcvblsImgsDrctry();
            nwDiag.fldrTyp = 13;
            nwDiag.attchCtgry = 4;
            DialogResult dgres = nwDiag.ShowDialog();
            if (dgres == DialogResult.OK)
            {
            }
        }

        private void prvwInvoiceButton_Click(object sender, EventArgs e)
        {
            if (long.Parse(this.docIDTextBox.Text) <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select a Record First!", 0);
                return;
            }
            this.pageNo = 1;
            this.prntIdx = 0;
            this.prntIdx1 = 0;
            this.prntIdx2 = 0;
            this.ght = 0;
            this.prcWdth = 0;
            this.qntyWdth = 0;
            this.itmWdth = 0;
            this.qntyStartX = 0;
            this.prcStartX = 0;
            this.amntStartX = 0;
            this.amntWdth = 0;
            this.printPreviewDialog1 = new PrintPreviewDialog();

            this.printPreviewDialog1.Document = printDocument1;
            this.printPreviewDialog1.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.printPreviewDialog1.PrintPreviewControl.Zoom = 1;

            this.printPreviewDialog1.PrintPreviewControl.FindForm().ShowIcon = false;
            this.printPreviewDialog1.PrintPreviewControl.FindForm().ShowInTaskbar = false;
            ((ToolStripButton)((ToolStrip)this.printPreviewDialog1.Controls[1]).Items[0]).Enabled = false;
            ((ToolStripButton)((ToolStrip)this.printPreviewDialog1.Controls[1]).Items[0]).Visible = false;
            //((ToolStripButton)((ToolStrip)this.printPreviewDialog1.Controls[1]).Items[0]).Click += new EventHandler(this.printRcptButton_Click);
            //this.printPreviewDialog1.MainMenuStrip = menuStrip1;
            //this.printPreviewDialog1.MainMenuStrip.Visible = true;
            this.printRcptButton1.Visible = true;
            ((ToolStrip)this.printPreviewDialog1.Controls[1]).Items.Add(this.printRcptButton1);

            this.printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4", 850, 1100);
            //this.printPreviewDialog1.FindForm().Height = Global.mnFrm.Height;
            //this.printPreviewDialog1.FindForm().StartPosition = FormStartPosition.Manual;
            this.printPreviewDialog1.FindForm().WindowState = FormWindowState.Maximized;
            this.printPreviewDialog1.ShowDialog();
        }

        int pageNo = 1;
        int prntIdx = 0;
        int prntIdx1 = 0;
        int prntIdx2 = 0;
        float ght = 0;
        int prcWdth = 0;
        int qntyWdth = 0;
        int itmWdth = 0;
        int qntyStartX = 0;
        int prcStartX = 0;
        int amntWdth = 0;
        int amntStartX = 0;

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            long rcvblHdrID = long.Parse(this.docIDTextBox.Text);
            string rcvblDoctype = this.docTypeComboBox.Text;

            Graphics g = e.Graphics;
            Pen aPen = new Pen(Brushes.Black, 1);
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4", 850, 1100);
            //e.PageSettings.
            Font font1 = new Font("Times New Roman", 12.25f, FontStyle.Underline | FontStyle.Bold);
            Font font11 = new Font("Times New Roman", 12.25f, FontStyle.Bold);
            Font font2 = new Font("Times New Roman", 12.25f, FontStyle.Bold);
            Font font4 = new Font("Times New Roman", 12.0f, FontStyle.Bold);
            Font font41 = new Font("Times New Roman", 12.0f);
            Font font3 = new Font("Tahoma", 11.0f);
            Font font311 = new Font("Lucida Console", 10.0f);
            Font font31 = new Font("Lucida Console", 12.5f, FontStyle.Bold);
            Font font5 = new Font("Times New Roman", 6.0f, FontStyle.Italic);

            int font1Hght = font1.Height;
            int font2Hght = font2.Height;
            int font3Hght = font3.Height;
            int font31Hght = font31.Height;
            int font311Hght = font311.Height;
            int font4Hght = font4.Height;
            int font5Hght = font5.Height;

            float pageWidth = e.PageSettings.PaperSize.Width - 40;//e.PageSettings.PrintableArea.Width;
            float pageHeight = e.PageSettings.PaperSize.Height - 40;// e.PageSettings.PrintableArea.Height;
                                                                    //Global.mnFrm.cmCde.showMsg(pageWidth.ToString(), 0);
            int startX = 60;
            int startY = 20;
            int offsetY = 0;
            int lnLength = 730;
            //StringBuilder strPrnt = new StringBuilder();
            //strPrnt.AppendLine("Received From");
            string[] nwLn;
            string drfPrnt = "";
            if (this.apprvlStatusTextBox.Text != "Approved")
            {
                //Global.mnFrm.cmCde.showMsg("Only Approved Documents Can be Printed!", 0);
                //return;
                drfPrnt = " ";//(DRAFT INVOICE HENCE MAY CHANGE)
            }

            if (this.pageNo == 1)
            {
                //Image img = Global.mnFrm.cmCde.getDBImageFile(Global.mnFrm.cmCde.Org_id.ToString() + ".png", 0);
                Image img = global::Accounting.Properties.Resources.actions_document_preview;
                string folderNm = Global.mnFrm.cmCde.getOrgImgsDrctry();
                string storeFileNm = Global.mnFrm.cmCde.Org_id.ToString() + ".png";
                if (Global.mnFrm.cmCde.myComputer.FileSystem.FileExists(folderNm + @"\" + storeFileNm))
                {
                    System.IO.FileStream rs = new System.IO.FileStream(folderNm + @"\" + storeFileNm,
                   System.IO.FileMode.OpenOrCreate,
                   System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
                    Byte[] imgRead = new Byte[rs.Length];
                    rs.Read(imgRead, 0, Convert.ToInt32(rs.Length));
                    img = Image.FromStream(rs);
                    rs.Close();
                }
                else
                {
                    img = Global.mnFrm.cmCde.getDBImageFile(Global.mnFrm.cmCde.Org_id.ToString() + ".png", 0);
                }

                float picWdth = 100.00F;
                float picHght = (float)(picWdth / img.Width) * (float)img.Height;

                g.DrawImage(img, startX, startY + offsetY, picWdth, picHght);
                //g.DrawImage(this.LargerImage, destRect, srcRect, GraphicsUnit.Pixel);

                //Org Name
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
                  Global.mnFrm.cmCde.getOrgName(Global.mnFrm.cmCde.Org_id),
                  pageWidth + 85, font2, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i]
                    , font2, Brushes.Black, startX + picWdth, startY + offsetY);
                    offsetY += font2Hght;
                }

                //Pstal Address
                g.DrawString(Global.mnFrm.cmCde.getOrgPstlAddrs(Global.mnFrm.cmCde.Org_id).Trim(),
                font2, Brushes.Black, startX + picWdth, startY + offsetY);
                //offsetY += font2Hght;

                ght = g.MeasureString(
                  Global.mnFrm.cmCde.getOrgPstlAddrs(Global.mnFrm.cmCde.Org_id).Trim(), font2).Height;
                offsetY = offsetY + (int)ght;
                //Contacts Nos
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
            Global.mnFrm.cmCde.getOrgContactNos(Global.mnFrm.cmCde.Org_id),
            pageWidth - 85, font2, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i]
                    , font2, Brushes.Black, startX + picWdth, startY + offsetY);
                    offsetY += font2Hght;
                }
                //Email Address
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
            Global.mnFrm.cmCde.getOrgEmailAddrs(Global.mnFrm.cmCde.Org_id),
            pageWidth, font2, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i]
                    , font2, Brushes.Black, startX + picWdth, startY + offsetY);
                    offsetY += font2Hght;
                }
                offsetY += font2Hght;
                if (offsetY < (int)picHght)
                {
                    offsetY = font2Hght + (int)picHght;
                }

                g.DrawLine(aPen, startX, startY + offsetY, startX + lnLength,
                  startY + offsetY);
                g.DrawString(this.docTypeComboBox.Text.Replace("Customer Standard Payment", "Customer Bill/Invoice").ToUpper() + drfPrnt, font2, Brushes.Black, startX, startY + offsetY);

                g.DrawLine(aPen, startX, startY + offsetY, startX,
        startY + offsetY + font2Hght);
                g.DrawLine(aPen, startX + lnLength, startY + offsetY, startX + lnLength,
        startY + offsetY + font2Hght);
                offsetY += font2Hght;
                g.DrawLine(aPen, startX, startY + offsetY, startX + lnLength,
                startY + offsetY);


                offsetY += 7;
                g.DrawString("Document No: ", font4, Brushes.Black, startX, startY + offsetY);
                ght = g.MeasureString("Document No: ", font4).Width;
                //Receipt No: 
                g.DrawString(this.docIDNumTextBox.Text,
            font3, Brushes.Black, startX + ght, startY + offsetY);
                float nwght = g.MeasureString(this.docIDNumTextBox.Text, font3).Width;
                g.DrawString("Document Date: ", font4, Brushes.Black, startX + ght + nwght + 10, startY + offsetY);
                ght += g.MeasureString("Document Date: ", font4).Width;
                //Receipt No: 
                g.DrawString(this.docDteTextBox.Text,
            font3, Brushes.Black, startX + ght + nwght + 10, startY + offsetY);

                offsetY += font4Hght;
                g.DrawString(this.label2.Text + " ", font4, Brushes.Black, startX, startY + offsetY);
                //offsetY += font4Hght;
                ght = g.MeasureString(this.label2.Text + " ", font4).Width;
                //Get Last Payment
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
            this.cstmrNmTextBox.Text,
            startX + ght + pageWidth - 350, font3, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i]
                    , font3, Brushes.Black, startX + ght, startY + offsetY);
                    if (i < nwLn.Length - 1)
                    {
                        offsetY += font4Hght;
                    }
                }
                offsetY += font4Hght;
                string bllto = Global.mnFrm.cmCde.getGnrlRecNm(
                  "scm.scm_cstmr_suplr_sites", "cust_sup_site_id",
                  "billing_address", long.Parse(this.cstmrSiteIDTextBox.Text));
                string shipto = Global.mnFrm.cmCde.getGnrlRecNm(
                 "scm.scm_cstmr_suplr_sites", "cust_sup_site_id",
                 "ship_to_address", long.Parse(this.cstmrSiteIDTextBox.Text));
                g.DrawString("Bill To: ", font4, Brushes.Black, startX, startY + offsetY);
                //offsetY += font4Hght;
                ght = g.MeasureString("Bill To: ", font4).Width;
                //Get Last Payment
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
            bllto,
            startX + ght + pageWidth - 350, font3, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i]
                    , font3, Brushes.Black, startX + ght, startY + offsetY);
                    if (i < nwLn.Length - 1)
                    {
                        offsetY += font4Hght;
                    }
                }
                offsetY += font4Hght;
                g.DrawString("Ship To: ", font4, Brushes.Black, startX, startY + offsetY);
                //offsetY += font4Hght;
                ght = g.MeasureString("Ship To: ", font4).Width;
                //Get Last Payment
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
            shipto,
            startX + ght + pageWidth - 350, font3, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i]
                    , font3, Brushes.Black, startX + ght, startY + offsetY);
                    if (i < nwLn.Length - 1)
                    {
                        offsetY += font4Hght;
                    }
                }
                offsetY += font4Hght;

                g.DrawString("Description: ", font4, Brushes.Black, startX, startY + offsetY);
                //offsetY += font4Hght;
                ght = g.MeasureString("Description: ", font4).Width;
                //Get Last Payment
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
            this.docCommentsTextBox.Text,
            startX + ght + pageWidth - 350, font3, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i]
                    , font3, Brushes.Black, startX + ght, startY + offsetY);
                    if (i < nwLn.Length - 1)
                    {
                        offsetY += font4Hght;
                    }
                }
                offsetY += font4Hght + 7;
                //offsetY += font4Hght;

                g.DrawLine(aPen, startX, startY + offsetY, startX + lnLength,
             startY + offsetY);
                g.DrawString("Item Description".ToUpper(), font11, Brushes.Black, startX, startY + offsetY);
                //offsetY += font4Hght;
                g.DrawLine(aPen, startX, startY + offsetY, startX,
        startY + offsetY + (int)font11.Height);

                ght = g.MeasureString("Item Description_____________", font11).Width;
                itmWdth = (int)ght + 40;
                qntyStartX = startX + (int)ght;
                //g.DrawString("Quantity".PadLeft(21, ' ').ToUpper(), font11, Brushes.Black, qntyStartX, startY + offsetY);
                //offsetY += font4Hght;
                //        g.DrawLine(aPen, qntyStartX + 27, startY + offsetY, qntyStartX + 27,
                //startY + offsetY + (int)font11.Height);

                ght += g.MeasureString("Quantity".PadLeft(26, ' '), font11).Width;
                qntyWdth = (int)g.MeasureString("Quantity".PadLeft(26, ' '), font11).Width; ;
                prcStartX = startX + (int)ght;

                //g.DrawString("Unit Price".PadLeft(21, ' ').ToUpper(), font11, Brushes.Black, prcStartX, startY + offsetY);
                //        g.DrawLine(aPen, prcStartX + 5, startY + offsetY, prcStartX + 5,
                //startY + offsetY + (int)font11.Height);

                ght += g.MeasureString("Unit Price".PadLeft(26, ' '), font11).Width;
                prcWdth = (int)g.MeasureString("Unit Price".PadLeft(26, ' '), font11).Width;
                amntStartX = startX + (int)ght;
                g.DrawString(("Amount (" + this.invcCurrTextBox.Text + ")").PadLeft(22, ' ').ToUpper(),
                  font11, Brushes.Black, amntStartX, startY + offsetY);
                g.DrawLine(aPen, amntStartX + 5, startY + offsetY, amntStartX + 5,
        startY + offsetY + (int)font11.Height);

                ght = g.MeasureString(("Amount (" + this.invcCurrTextBox.Text + ")").PadLeft(27, ' '), font11).Width;
                amntWdth = (int)ght;
                g.DrawLine(aPen, startX + lnLength, startY + offsetY, startX + lnLength,
        startY + offsetY + (int)font11.Height);

                offsetY += font1Hght;
                g.DrawLine(aPen, startX, startY + offsetY, startX + lnLength,
            startY + offsetY);

            }
            offsetY += 5;
            //DataSet lndtst = Global.get_One_SalesDcLines(long.Parse(this.docIDTextBox.Text));
            DataSet lndtst;
            if (this.docTypeComboBox.Text.Contains("Customer"))
            {
                lndtst = Global.get_RcvblDocSmryLns(rcvblHdrID,
                rcvblDoctype);
            }
            else
            {
                lndtst = Global.get_PyblsDocSmryLns(rcvblHdrID,
                rcvblDoctype);
            }
            //Line Items
            int orgOffstY = 0;
            int hgstOffst = offsetY;
            int y2 = 0;
            int itmCnt = lndtst.Tables[0].Rows.Count;
            if (itmCnt <= 0)
            {
                orgOffstY = hgstOffst;
                offsetY = orgOffstY;
                y2 = hgstOffst;
                ght = 0;
            }
            for (int a = this.prntIdx; a < itmCnt; a++)
            {
                orgOffstY = hgstOffst;
                offsetY = orgOffstY;
                ght = 0;
                nwLn = Global.mnFrm.cmCde.breakTxtDown(lndtst.Tables[0].Rows[a][1].ToString(),
            itmWdth + 30, font3, g);

                float itmHght = 0;
                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i]
                    , font3, Brushes.Black, startX, startY + offsetY);
                    ght += g.MeasureString(nwLn[i], font3).Width;
                    itmHght += g.MeasureString(nwLn[i], font3).Height;
                    offsetY += font3Hght;
                    if (i == nwLn.Length - 1)
                    {
                        g.DrawLine(aPen, startX, startY + orgOffstY - 5, startX,
                startY + orgOffstY + (int)itmHght + 5);
                        if (a == itmCnt - 1)
                        {
                            y2 = orgOffstY + (int)itmHght + 5;
                        }
                    }
                }

                if (offsetY > hgstOffst)
                {
                    hgstOffst = offsetY;
                }
                offsetY = orgOffstY;

                //        nwLn = Global.mnFrm.cmCde.breakTxtDown(
                //          double.Parse(lndtst.Tables[0].Rows[a][2].ToString()).ToString("#,##0.00"),
                //    qntyWdth, font311, g);
                //        for (int i = 0; i < nwLn.Length; i++)
                //        {
                //          if (i == 0)
                //          {
                //            ght = g.MeasureString(nwLn[i], font311).Width;
                //            g.DrawLine(aPen, qntyStartX + 27, startY + offsetY - 5, qntyStartX + 27,
                //startY + offsetY + (int)itmHght + 5);
                //          }
                //          g.DrawString(nwLn[i].PadLeft(19, ' ')
                //          , font311, Brushes.Black, qntyStartX - 5, startY + offsetY);
                //          offsetY += font311Hght;
                //        }
                //        if (offsetY > hgstOffst)
                //        {
                //          hgstOffst = offsetY;
                //        }
                //        offsetY = orgOffstY;

                //        nwLn = Global.mnFrm.cmCde.breakTxtDown("1",
                //    prcWdth, font311, g);
                //        for (int i = 0; i < nwLn.Length; i++)
                //        {
                //          if (i == 0)
                //          {
                //            ght = g.MeasureString(nwLn[i], font311).Width;
                //            g.DrawLine(aPen, prcStartX + 5, startY + offsetY - 5, prcStartX + 5,
                //startY + offsetY + (int)itmHght + 5);
                //          }
                //          //g.DrawString(nwLn[i].PadLeft(19, ' ')
                //          //, font311, Brushes.Black, prcStartX - 5, startY + offsetY);
                //          offsetY += font311Hght;
                //        }
                //        if (offsetY > hgstOffst)
                //        {
                //          hgstOffst = offsetY;
                //        }
                //        offsetY = orgOffstY;

                nwLn = Global.mnFrm.cmCde.breakTxtDown(
                  (double.Parse(lndtst.Tables[0].Rows[a][2].ToString())).ToString("#,##0.00"),
            prcWdth, font311, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    if (i == 0)
                    {
                        ght = g.MeasureString(nwLn[i], font311).Width;
                        g.DrawLine(aPen, amntStartX + 5, startY + offsetY - 5, amntStartX + 5,
            startY + offsetY + (int)itmHght + 5);
                        g.DrawLine(aPen, startX + lnLength, startY + offsetY - 5, startX + lnLength,
            startY + offsetY + (int)itmHght + 5);
                    }
                    g.DrawString(nwLn[i].PadLeft(21, ' ')
                    , font311, Brushes.Black, amntStartX, startY + offsetY);
                    offsetY += font311Hght;
                }
                if (offsetY > hgstOffst)
                {
                    hgstOffst = offsetY;
                }
                hgstOffst += 8;

                this.prntIdx++;

                if (hgstOffst >= pageHeight - 30)
                {
                    e.HasMorePages = true;
                    offsetY = 0;
                    this.pageNo++;
                    return;
                }
                //else
                //{
                //  e.HasMorePages = false;
                //}

            }

            if (this.prntIdx1 == 0)
            {
                offsetY = y2;//hgstOffst + font3Hght - 8;
                             //y2;
                g.DrawLine(aPen, startX, startY + offsetY, startX + lnLength,
                     startY + offsetY);

                g.DrawLine(aPen, startX, startY + offsetY, startX,
        startY + offsetY + 5);
                g.DrawLine(aPen, startX + lnLength, startY + offsetY, startX + lnLength,
        startY + offsetY + 5);


                g.DrawLine(aPen, startX, startY + offsetY + 5, startX + lnLength,
            startY + offsetY + 5);
            }
            offsetY += 10;
            DataSet smmryDtSt;
            if (this.docTypeComboBox.Text.Contains("Customer"))
            {
                smmryDtSt = Global.get_RcvblDocEndLns(rcvblHdrID,
                rcvblDoctype);
            }
            else
            {
                smmryDtSt = Global.get_PyblsDocEndLns(rcvblHdrID,
                rcvblDoctype);
            }
            orgOffstY = 0;
            hgstOffst = offsetY;

            for (int b = this.prntIdx1; b < smmryDtSt.Tables[0].Rows.Count; b++)
            {
                orgOffstY = hgstOffst;
                offsetY = orgOffstY;
                ght = 0;
                if (hgstOffst >= pageHeight - 30)
                {
                    e.HasMorePages = true;
                    offsetY = 0;
                    this.pageNo++;
                    return;
                }
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
                  (smmryDtSt.Tables[0].Rows[b][4].ToString()
                  + " (" + this.invcCurrTextBox.Text + ")").PadLeft(35, ' ').PadRight(36, ' '),
            1.77F * qntyWdth, font311, g);
                float itmHght = 0;
                //float smrWdth = 0;
                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i].PadLeft(35, ' ').PadRight(36, ' ')
                    , font311, Brushes.Black, prcStartX - 145, startY + offsetY + 1);
                    offsetY += font311Hght;
                    //smrWdth += g.MeasureString(nwLn[i], font3).Width;
                    itmHght += g.MeasureString(nwLn[i], font311).Height;
                    //if (i > 0)
                    //{
                    //  itmHght -= 3.5F;
                    //}
                    if (i == nwLn.Length - 1)
                    {
                        g.DrawLine(aPen, qntyStartX + 27, startY + orgOffstY - 5, qntyStartX + 27,
                startY + orgOffstY + (int)itmHght);
                        g.DrawLine(aPen, qntyStartX + 27, startY + orgOffstY + (int)itmHght, qntyStartX + 39 + lnLength - itmWdth,
            startY + orgOffstY + (int)itmHght);
                        offsetY += 5;
                    }

                }
                if (offsetY > hgstOffst)
                {
                    hgstOffst = offsetY;
                }
                offsetY = orgOffstY;

                nwLn = Global.mnFrm.cmCde.breakTxtDown(
                  double.Parse(smmryDtSt.Tables[0].Rows[b][2].ToString()).ToString("#,##0.00"),
            prcWdth, font311, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    if (i == 0)
                    {
                        ght = g.MeasureString(nwLn[i], font311).Width;
                        g.DrawLine(aPen, amntStartX + 5, startY + offsetY - 5, amntStartX + 5,
            startY + offsetY + (int)itmHght);
                        g.DrawLine(aPen, startX + lnLength, startY + offsetY - 5, startX + lnLength,
            startY + offsetY + (int)itmHght);
                    }
                    g.DrawString(nwLn[i].PadLeft(21, ' ')
                    , font311, Brushes.Black, amntStartX, startY + offsetY + 1);
                    offsetY += font311Hght + 5;
                    //          if (i == nwLn.Length - 1 && hgstOffst <= offsetY)
                    //          {
                    //            g.DrawLine(aPen, qntyStartX + 27, startY + offsetY - 3, qntyStartX + 39 + lnLength - itmWdth,
                    //startY + offsetY - 3);
                    //          }
                }
                //        g.DrawLine(aPen, qntyStartX + 27, startY + offsetY, qntyStartX + 27 + lnLength - itmWdth,
                //startY + offsetY);

                if (offsetY > hgstOffst)
                {
                    hgstOffst = offsetY;
                }
                this.prntIdx1++;
            }
            offsetY = hgstOffst;
            offsetY += font2Hght + 5;
            //  //offsetY += font2Hght;
            //  if (this.pymntTermsTextBox.Text != "")
            //  {
            //      if (offsetY >= pageHeight - 30)
            //      {
            //          e.HasMorePages = true;
            //          offsetY = 0;
            //          this.pageNo++;
            //          return;
            //      }
            //      g.DrawLine(aPen, startX, startY + offsetY, startX + lnLength,
            //  startY + offsetY);
            //      g.DrawString("TERMS", font2, Brushes.Black, startX, startY + offsetY);
            //      g.DrawLine(aPen, startX, startY + offsetY, startX,
            //startY + offsetY + font2Hght);
            //      g.DrawLine(aPen, startX + lnLength, startY + offsetY, startX + lnLength,
            //startY + offsetY + font2Hght);
            //      offsetY += font2Hght;
            //      g.DrawLine(aPen, startX, startY + offsetY, startX + lnLength,
            //startY + offsetY);

            //      float trmHgth = 0;
            //      nwLn = Global.mnFrm.cmCde.breakTxtDown(
            //    this.pymntTermsTextBox.Text,
            //    startX + pageWidth - 150, font3, g);
            //      orgOffstY = offsetY;
            //      offsetY += 5;
            //      for (int i = 0; i < nwLn.Length; i++)
            //      {
            //          //if (i == 0)
            //          //{
            //          //}
            //          g.DrawString(nwLn[i]
            //          , font3, Brushes.Black, startX, startY + offsetY);
            //          trmHgth += g.MeasureString(nwLn[i], font3).Height + 5;
            //          offsetY += font3Hght;
            //          if (hgstOffst <= offsetY)
            //          {
            //              hgstOffst = offsetY;
            //          }
            //          if (i == nwLn.Length - 1)
            //          {
            //              g.DrawLine(aPen, startX, startY + orgOffstY, startX,
            //    startY + orgOffstY + trmHgth);
            //              g.DrawLine(aPen, startX + lnLength, startY + orgOffstY, startX + lnLength,
            //    startY + orgOffstY + trmHgth);
            //              g.DrawLine(aPen, startX, startY + orgOffstY + trmHgth, startX + lnLength,
            //    startY + orgOffstY + trmHgth);
            //          }
            //      }
            //  }
            //  //offsetY += font4Hght;
            //  if (this.pymntTermsTextBox.Text != "")
            //  {
            //      offsetY = hgstOffst;
            //      offsetY += font2Hght + 5;
            //  }
            if (this.pymntTermsTextBox.Text != "")
            {
                if (offsetY >= pageHeight - 30)
                {
                    e.HasMorePages = true;
                    offsetY = 0;
                    this.pageNo++;
                    return;
                }
                g.DrawLine(aPen, startX, startY + offsetY, startX + lnLength,
            startY + offsetY);
                g.DrawString("TERMS", font2, Brushes.Black, startX, startY + offsetY);
                g.DrawLine(aPen, startX, startY + offsetY, startX,
          startY + offsetY + font2Hght);
                g.DrawLine(aPen, startX + lnLength, startY + offsetY, startX + lnLength,
          startY + offsetY + font2Hght);
                offsetY += font2Hght;
                g.DrawLine(aPen, startX, startY + offsetY, startX + lnLength,
          startY + offsetY);

                float trmHgth = 0;
                nwLn = Global.mnFrm.cmCde.breakTxtDownML(
              this.pymntTermsTextBox.Text,
              startX + pageWidth - 150, font3, g);
                orgOffstY = offsetY;
                offsetY += 5;
                for (int i = 0; i < nwLn.Length; i++)
                {
                    //if (i == 0)
                    //{
                    //}
                    g.DrawString(nwLn[i]
                    , font3, Brushes.Black, startX, startY + offsetY);
                    trmHgth += g.MeasureString(nwLn[i], font3).Height + 0.0F;
                    offsetY += font3Hght;
                    if (hgstOffst <= offsetY)
                    {
                        hgstOffst = offsetY;
                    }
                    if (i == nwLn.Length - 1)
                    {
                        trmHgth += 5;
                        g.DrawLine(aPen, startX, startY + orgOffstY, startX,
              startY + orgOffstY + trmHgth);
                        g.DrawLine(aPen, startX + lnLength, startY + orgOffstY, startX + lnLength,
              startY + orgOffstY + trmHgth);
                        g.DrawLine(aPen, startX, startY + orgOffstY + trmHgth, startX + lnLength,
              startY + orgOffstY + trmHgth);
                    }
                }
            }
            //offsetY += font4Hght;
            if (this.pymntTermsTextBox.Text != "")
            {
                offsetY = hgstOffst;
                offsetY += font2Hght + 5;
                offsetY += 40;
            }
            //offsetY += font2Hght;
            string sgntryCols = Global.getDocSgntryCols("Invoices Signatories");
            if (sgntryCols != "")
            {
                if (offsetY >= pageHeight - 30)
                {
                    e.HasMorePages = true;
                    offsetY = 0;
                    this.pageNo++;
                    return;
                }
                //      g.DrawLine(aPen, startX, startY + offsetY, startX + lnLength,
                //  startY + offsetY);
                //      g.DrawString("", font2, Brushes.Black, startX, startY + offsetY);
                //      g.DrawLine(aPen, startX, startY + offsetY, startX,
                //startY + offsetY + 40);
                //      g.DrawLine(aPen, startX + lnLength, startY + offsetY, startX + lnLength,
                //startY + offsetY + 40);
                offsetY += 40;
                g.DrawLine(aPen, startX, startY + offsetY, startX + lnLength,
          startY + offsetY);

                float trmHgth = 0;

                orgOffstY = offsetY;
                offsetY += 5;
                g.DrawString(sgntryCols
          , font4, Brushes.Black, startX, startY + offsetY);

                //g.DrawString("                    " + sgntryCols.Replace(",", "                    ").ToUpper()
                //  , font4, Brushes.Black, startX, startY + offsetY);
                trmHgth += font4Hght + 5;
                //offsetY += font3Hght;
                if (hgstOffst <= orgOffstY + trmHgth)
                {
                    hgstOffst = (int)orgOffstY + (int)trmHgth;
                }
                //        g.DrawLine(aPen, startX, startY + orgOffstY, startX,
                //startY + orgOffstY + trmHgth);
                //        g.DrawLine(aPen, startX + lnLength, startY + orgOffstY, startX + lnLength,
                //startY + orgOffstY + trmHgth);
                //        g.DrawLine(aPen, startX, startY + orgOffstY + trmHgth, startX + lnLength,
                //startY + orgOffstY + trmHgth);
            }
            //Slogan: 
            offsetY = (int)pageHeight - 30;
            //hgstOffst = offsetY;
            if (hgstOffst >= pageHeight - 20)
            {
                e.HasMorePages = true;
                offsetY = 0;
                this.pageNo++;
                return;
            }
            g.DrawLine(aPen, startX, startY + offsetY, startX + lnLength,
         startY + offsetY);
            offsetY += font5Hght;
            g.DrawString(Global.mnFrm.cmCde.getOrgSlogan(Global.mnFrm.cmCde.Org_id) +
            "    Software Developed by Rhomicom Systems Technologies Ltd."
            + "   Website:www.rhomicomgh.com Mobile: 0544709501/0266245395"
            , font5, Brushes.Black, startX, startY + offsetY);
            offsetY += font5Hght;
        }

        private void printRcptButton_Click(object sender, EventArgs e)
        {
            this.pageNo = 1;
            this.prntIdx = 0;
            this.prntIdx1 = 0;
            this.prntIdx2 = 0;
            this.ght = 0;
            this.prcWdth = 0;
            this.qntyWdth = 0;
            this.itmWdth = 0;
            this.qntyStartX = 0;
            this.prcStartX = 0;
            this.amntStartX = 0;
            this.amntWdth = 0;

            this.printDialog1 = new PrintDialog();
            this.printDialog1.UseEXDialog = true;
            this.printDialog1.ShowNetwork = true;
            this.printDialog1.AllowCurrentPage = true;
            this.printDialog1.AllowPrintToFile = true;
            this.printDialog1.AllowSelection = true;
            this.printDialog1.AllowSomePages = true;
            this.printDialog1.PrinterSettings.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4", 850, 1100);
            this.printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4", 850, 1100);
            this.printDialog1.PrinterSettings.DefaultPageSettings.PaperSize.PaperName = "A4";
            this.printDialog1.PrinterSettings.DefaultPageSettings.PaperSize.Height = 1100;
            this.printDialog1.PrinterSettings.DefaultPageSettings.PaperSize.Width = 850;

            printDialog1.Document = this.printDocument1;
            DialogResult res = printDialog1.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void rgstrButton_Click(object sender, EventArgs e)
        {
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("You must be in ADD/EDIT mode first!", 0);
                return;
            }
            string lnkdEventComboBox = "Attendance Register";
            if (lnkdEventComboBox == "None")
            {
                Global.mnFrm.cmCde.showMsg("You must indicate Event Type first!", 0);
                return;
            }
            else if (lnkdEventComboBox == "Attendance Register")
            {
                string[] selVals = new string[1];
                selVals[0] = this.rgstrIDTextBox.Text;
                DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                  Global.mnFrm.cmCde.getLovID("Attendance Registers"), ref selVals,
                  true, false, Global.mnFrm.cmCde.Org_id, "", "",
                 "%", "Both", true);
                if (dgRes == DialogResult.OK)
                {
                    for (int i = 0; i < selVals.Length; i++)
                    {
                        this.rgstrIDTextBox.Text = selVals[i];
                        this.rgstrNumTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                          "attn.attn_attendance_recs_hdr", "recs_hdr_id", "recs_hdr_name",
                          long.Parse(selVals[i]));
                    }
                }
            }
            else
            {
                string[] selVals = new string[1];
                selVals[0] = this.rgstrIDTextBox.Text;
                DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                  Global.mnFrm.cmCde.getLovID("Production Process Runs"), ref selVals,
                  true, false, Global.mnFrm.cmCde.Org_id, "", "",
                 "%", "Both", true);
                if (dgRes == DialogResult.OK)
                {
                    for (int i = 0; i < selVals.Length; i++)
                    {
                        this.rgstrIDTextBox.Text = selVals[i];
                        this.rgstrNumTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                          "scm.scm_process_run", "process_run_id", "batch_code_num",
                          long.Parse(selVals[i]));
                    }
                }
            }
        }

        private void costCtgrButton_Click(object sender, EventArgs e)
        {
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("You must be in ADD/EDIT mode first!", 0);
                return;//
            }

            if (this.rgstrIDTextBox.Text == ""
              || this.rgstrIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("You must select an Event Number first!", 0);
                return;
            }
            string lnkdEventComboBox = "Attendance Register";

            if (lnkdEventComboBox == "None")
            {
                Global.mnFrm.cmCde.showMsg("You must indicate Event Type first!", 0);
                return;
            }
            else if (lnkdEventComboBox == "Attendance Register")
            {
                int[] selVals = new int[1];
                selVals[0] = Global.mnFrm.cmCde.getPssblValID(this.costCtgrTextBox.Text,
                  Global.mnFrm.cmCde.getLovID("Event Cost Categories"));
                DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                    Global.mnFrm.cmCde.getLovID("Event Cost Categories"), ref selVals,
                    true, false,
                 "%", "Both", true);
                if (dgRes == DialogResult.OK)
                {
                    for (int i = 0; i < selVals.Length; i++)
                    {
                        this.costCtgrTextBox.Text = Global.mnFrm.cmCde.getPssblValNm(selVals[i]);
                    }
                    //this.obey_evnts = true;
                    //DataGridViewCellEventArgs ex = new DataGridViewCellEventArgs(0, e.RowIndex);
                    //this.costingDataGridView_CellValueChanged(this.costingDataGridView, ex);
                }
            }
            else
            {
                string[] selVals = new string[1];
                selVals[0] = this.costCtgrTextBox.Text;
                DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                  Global.mnFrm.cmCde.getLovID("Production Process Run Stages"), ref selVals,
                  true, false, Global.mnFrm.cmCde.Org_id, "", "",
                 "%", "Both", true);
                if (dgRes == DialogResult.OK)
                {
                    for (int i = 0; i < selVals.Length; i++)
                    {
                        this.costCtgrTextBox.Text = selVals[i];
                    }
                }
            }
        }

        private void customInvoiceButton_Click(object sender, EventArgs e)
        {
            string reportName = "";
            string reportTitle = this.docTypeComboBox.Text.Replace("Customer Standard Payment", "Customer Bill/Invoice").ToUpper();

            reportName = Global.mnFrm.cmCde.getEnbldPssblValDesc("Receivables Invoice",
            Global.mnFrm.cmCde.getLovID("Document Custom Print Process Names"));

            string paramRepsNVals = "{:invoice_id}~" + this.docIDTextBox.Text + "|{:documentTitle}~" + reportTitle;
            //Global.mnFrm.cmCde.showSQLNoPermsn(reportName + "\r\n" + paramRepsNVals);
            Global.mnFrm.cmCde.showRptParamsDiag(Global.mnFrm.cmCde.getRptID(reportName), Global.mnFrm.cmCde, paramRepsNVals, reportTitle);

        }

        private void pymntTermsButton_Click(object sender, EventArgs e)
        {
            string txtStr = this.pymntTermsTextBox.Text;
            if (this.editRec || this.addRec)
            {
                Global.mnFrm.cmCde.showTxtNoPermsn(ref txtStr);
                this.pymntTermsTextBox.Text = txtStr;
            }
            else
            {
                Global.mnFrm.cmCde.showSQLNoPermsn(txtStr);
            }
        }

        private void openBatchButton_Click(object sender, EventArgs e)
        {
            if (this.glBatchNmTextBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Please select a Transaction Batch First!", 0);
                return;
            }
            string btchN = this.glBatchNmTextBox.Text;
            Global.mnFrm.searchForTrnsTextBox.Text = btchN;
            Global.mnFrm.searchInTrnsComboBox.SelectedItem = "Batch Name";
            Global.mnFrm.loadCorrectPanel("Journal Entries");
            Global.mnFrm.showUnpostedCheckBox.Checked = false;
            if (Global.mnFrm.shwMyBatchesCheckBox.Enabled == true)
            {
                Global.mnFrm.shwMyBatchesCheckBox.Checked = false;
            }
            Global.mnFrm.rfrshTrnsButton.PerformClick();
        }

        private void docIDNumTextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.cstmrDocNumTextBox.Text == "")
            {
                this.cstmrDocNumTextBox.Text = this.docIDNumTextBox.Text;
            }
        }

        private void extraInfoButton_Click(object sender, EventArgs e)
        {
            if (this.docIDTextBox.Text == "" ||
             this.docIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("No record to View!", 0);
                return;
            }
            bool canEdt = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[12]);
            DialogResult dgres = Global.mnFrm.cmCde.showRowsExtInfDiag(Global.mnFrm.cmCde.getMdlGrpID("Receivable Invoices"),
             long.Parse(this.docIDTextBox.Text), "accb.accb_all_other_info_table", this.docIDNumTextBox.Text, canEdt, 10, 9,
                "accb.accb_all_other_info_table_dflt_row_id_seq");
            if (dgres == DialogResult.OK)
            {
            }
        }
    }
}

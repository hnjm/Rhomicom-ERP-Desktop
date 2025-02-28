using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using StoresAndInventoryManager.Forms;
using StoresAndInventoryManager.Classes;
using StoresAndInventoryManager.Dialogs;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing.Layout;

namespace StoresAndInventoryManager.Forms
{
    public partial class invoiceForm : Form
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
        bool obey_evnts = false;
        public string srchWrd = "%";
        public bool txtChngd = false;
        long[] prsnIDs = new long[1];
        long PrsnID = -1;

        bool addRec = false;
        bool editRec = false;
        bool addDtRec = false;
        bool editDtRec = false;
        bool payDocs = false;
        bool canEditPrice = false;
        //Pro-Forma Invoice
        bool vwRecsPF = false;
        bool addRecsPF = false;
        bool editRecsPF = false;
        bool delRecsPF = false;
        //Sales Order
        bool vwRecsSO = false;
        bool addRecsSO = false;
        bool editRecsSO = false;
        bool delRecsSO = false;
        bool beenToCheckBx = false;
        //Sales Invoice
        bool vwRecsSI = false;
        bool addRecsSI = false;
        bool editRecsSI = false;
        bool delRecsSI = false;
        //Internal Item Request
        bool vwRecsIR = false;
        bool addRecsIR = false;
        bool editRecsIR = false;
        bool delRecsIR = false;
        //Item Issue-Unbilled
        bool vwRecsII = false;
        bool addRecsII = false;
        bool editRecsII = false;
        bool delRecsII = false;
        //Sales Return
        bool vwRecsSR = false;
        bool addRecsSR = false;
        bool editRecsSR = false;
        bool delRecsSR = false;

        bool docSaved = true;
        bool autoLoad = false;

        bool qtyChnged = false;
        bool itmChnged = false;
        bool rowCreated = false;

        public int curid = -1;
        public string curCode = "";

        #endregion

        #region "FORM EVENTS..."
        public invoiceForm()
        {
            InitializeComponent();
        }

        int dfltInvAcntID = -1;
        int dfltCGSAcntID = -1;
        int dfltExpnsAcntID = -1;
        int dfltRvnuAcntID = -1;

        int dfltSRAcntID = -1;
        int dfltCashAcntID = -1;
        int dfltCheckAcntID = -1;
        int dfltRcvblAcntID = -1;
        int dfltLbltyAccnt = -1;
        int dfltBadDbtAcntID = -1;

        private void invoiceForm_Load(object sender, EventArgs e)
        {
            this.txtChngd = false;
            this.obey_evnts = false;
            Color[] clrs = Global.mnFrm.cmCde.getColors();
            this.BackColor = clrs[0];
            //this.glsLabel3.TopFill = clrs[0];
            //this.glsLabel3.BottomFill = clrs[1];
            this.curid = Global.mnFrm.cmCde.getOrgFuncCurID(Global.mnFrm.cmCde.Org_id);
            this.curCode = Global.mnFrm.cmCde.getPssblValNm(this.curid);

            //this.smmryDataGridView.RowCount = 7;
            this.invcListView.Focus();
            this.srcDocButton.Enabled = false;

            this.dfltRcvblAcntID = Global.get_DfltRcvblAcnt(Global.mnFrm.cmCde.Org_id);
            this.dfltBadDbtAcntID = Global.get_DfltBadDbtAcnt(Global.mnFrm.cmCde.Org_id);
            this.dfltLbltyAccnt = Global.get_DfltAccPyblAcnt(Global.mnFrm.cmCde.Org_id);
            this.dfltInvAcntID = Global.get_DfltInvAcnt(Global.mnFrm.cmCde.Org_id);
            this.dfltCGSAcntID = Global.get_DfltCSGAcnt(Global.mnFrm.cmCde.Org_id);
            this.dfltExpnsAcntID = Global.get_DfltExpnsAcnt(Global.mnFrm.cmCde.Org_id);
            this.dfltRvnuAcntID = Global.get_DfltRvnuAcnt(Global.mnFrm.cmCde.Org_id);
            this.dfltSRAcntID = Global.get_DfltSRAcnt(Global.mnFrm.cmCde.Org_id);
            this.dfltCashAcntID = Global.get_DfltCashAcnt(Global.mnFrm.cmCde.Org_id);
            this.dfltCheckAcntID = Global.get_DfltCheckAcnt(Global.mnFrm.cmCde.Org_id);

            this.txtChngd = false;

            this.timer1.Interval = 100;
            this.timer1.Enabled = true;
            this.obey_evnts = true;
            this.txtChngd = false;
        }

        public void loadPrvldgs()
        {
            bool vwSQL = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[31]);
            bool rcHstry = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[32]);
            this.payDocs = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[77]);
            this.canEditPrice = Global.mnFrm.cmCde.test_prmssns
                (Global.dfltPrvldgs[103]);
            //Pro-Forma Invoice
            this.vwRecsPF = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[46]);
            this.addRecsPF = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[47]);
            this.editRecsPF = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[48]);
            this.delRecsPF = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[49]);

            //Sales Order
            this.vwRecsSO = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[50]);
            this.addRecsSO = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[51]);
            this.editRecsSO = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[52]);
            this.delRecsSO = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[53]);
            //Sales Invoice
            this.vwRecsSI = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[54]);
            this.addRecsSI = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[55]);
            this.editRecsSI = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[56]);
            this.delRecsSI = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[57]);
            //Internal Item Request
            this.vwRecsIR = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[58]);
            this.addRecsIR = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[59]);
            this.editRecsIR = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[60]);
            this.delRecsIR = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[61]);
            //Item Issue-Unbilled
            this.vwRecsII = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[62]);
            this.addRecsII = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[63]);
            this.editRecsII = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[64]);
            this.delRecsII = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[65]);
            //Sales Return
            this.vwRecsSR = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[66]);
            this.addRecsSR = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[67]);
            this.editRecsSR = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[68]);
            this.delRecsSR = Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[69]);

            this.vwSQLButton.Enabled = vwSQL;
            this.rcHstryButton.Enabled = rcHstry;
        }

        public void disableFormButtons()
        {
            this.saveButton.Enabled = false;
            this.saveDtButton.Enabled = false;
            this.addPRFButton.Enabled = this.addRecsPF;
            this.addSOButton.Enabled = this.addRecsSO;
            this.addSIButton.Enabled = this.addRecsSI;
            this.addIRButton.Enabled = this.addRecsIR;
            this.addUIIButton.Enabled = this.addRecsII;
            this.addSRButton.Enabled = this.addRecsSR;
            if (this.docTypeComboBox.Text == "Pro-Forma Invoice")
            {
                this.editButton.Enabled = this.editRecsPF;
                this.delButton.Enabled = this.delRecsPF;
                this.addDtButton.Enabled = this.addRecsPF;
                this.editDtButton.Enabled = this.editRecsPF;
                this.delDtButton.Enabled = this.delRecsPF;
            }
            else if (this.docTypeComboBox.Text == "Sales Order")
            {
                this.editButton.Enabled = this.editRecsSO;
                this.delButton.Enabled = this.delRecsSO;
                this.addDtButton.Enabled = this.addRecsSO;
                this.editDtButton.Enabled = this.editRecsSO;
                this.delDtButton.Enabled = this.delRecsSO;
            }
            else if (this.docTypeComboBox.Text == "Sales Invoice")
            {
                this.editButton.Enabled = this.editRecsSI;
                this.delButton.Enabled = this.delRecsSI;
                this.addDtButton.Enabled = this.addRecsSI;
                this.editDtButton.Enabled = this.editRecsSI;
                this.delDtButton.Enabled = this.delRecsSI;
            }
            else if (this.docTypeComboBox.Text == "Internal Item Request")
            {
                this.editButton.Enabled = this.editRecsIR;
                this.delButton.Enabled = this.delRecsIR;
                this.addDtButton.Enabled = this.addRecsIR;
                this.editDtButton.Enabled = this.editRecsIR;
                this.delDtButton.Enabled = this.delRecsIR;
            }
            else if (this.docTypeComboBox.Text == "Item Issue-Unbilled")
            {
                this.editButton.Enabled = this.editRecsII;
                this.delButton.Enabled = this.delRecsII;
                this.addDtButton.Enabled = this.addRecsII;
                this.editDtButton.Enabled = this.editRecsII;
                this.delDtButton.Enabled = this.delRecsII;
            }
            else if (this.docTypeComboBox.Text == "Sales Return")
            {
                this.editButton.Enabled = this.editRecsSR;
                this.delButton.Enabled = this.delRecsSR;
                this.addDtButton.Enabled = this.addRecsSR;
                this.editDtButton.Enabled = this.editRecsSR;
                this.delDtButton.Enabled = this.delRecsSR;
            }
        }
        #endregion

        #region "SALES/ITEM ISSUE DOCUMENTS..."
        public void loadPanel()
        {
            this.saveLabel.Visible = false;
            Cursor.Current = Cursors.Default;

            this.obey_evnts = false;
            if (this.searchInComboBox.SelectedIndex < 0)
            {
                this.searchInComboBox.SelectedIndex = 4;
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
            DataSet dtst = Global.get_Basic_SalesDoc(this.searchForTextBox.Text,
              this.searchInComboBox.Text, this.rec_cur_indx,
              int.Parse(this.dsplySizeComboBox.Text), Global.mnFrm.cmCde.Org_id,
              Global.wfnLftMnu.vwSelfCheckBox.Checked,
              this.showUnpaidCheckBox.Checked);
            this.invcListView.Items.Clear();
            //System.Windows.Forms.Application.DoEvents();
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                this.last_rec_num = Global.mnFrm.cmCde.navFuncts.startIndex() + i;
                ListViewItem nwItem = new ListViewItem(new string[] {
    (Global.mnFrm.cmCde.navFuncts.startIndex() + i).ToString(),
    dtst.Tables[0].Rows[i][1].ToString(),
    dtst.Tables[0].Rows[i][0].ToString(),
    dtst.Tables[0].Rows[i][2].ToString()});
                this.invcListView.Items.Add(nwItem);
            }
            this.correctNavLbls(dtst);
            if (this.invcListView.Items.Count > 0)
            {
                this.obey_evnts = true;
                try
                {
                    this.invcListView.Items[0].Selected = true;
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                this.populateDet(-10000);
                this.populateLines(-100000, "");
                this.populateSmmry(-100000, "");
            }
            this.obey_evnts = true;
        }

        private void populateDet(long docHdrID)
        {
            //Global.mnFrm.cmCde.minimizeMemory();
            this.clearDetInfo();
            this.disableDetEdit();
            //System.Windows.Forms.Application.DoEvents();
            if (this.editRec == false)
            {
            }
            this.obey_evnts = false;
            DataSet dtst = Global.get_One_SalesDcDt(docHdrID);
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                this.docIDTextBox.Text = dtst.Tables[0].Rows[i][0].ToString();
                this.docIDNumTextBox.Text = dtst.Tables[0].Rows[i][1].ToString();
                if (this.editRec == false && this.addRec == false)
                {
                    this.docTypeComboBox.Items.Clear();
                    this.docTypeComboBox.Items.Add(dtst.Tables[0].Rows[i][2].ToString());
                }
                this.docTypeComboBox.SelectedItem = dtst.Tables[0].Rows[i][2].ToString();
                this.srcDocIDTextBox.Text = dtst.Tables[0].Rows[i][3].ToString();
                this.srcDocNumTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr",
                  "invc_hdr_id", "invc_number",
                  long.Parse(dtst.Tables[0].Rows[i][3].ToString()));

                long SIDocID = long.Parse(this.srcDocIDTextBox.Text);
                string strSrcDocType = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr",
                  "invc_hdr_id", "invc_type", SIDocID);

                if ((this.docTypeComboBox.Text != "Sales Invoice"
                  && this.docTypeComboBox.Text != "Sales Return")
                  || (this.docTypeComboBox.Text == "Sales Return"
                  && strSrcDocType != "Sales Invoice"))
                {
                    this.groupBox4.Visible = false;
                    this.groupBox4.Enabled = false;
                }
                else
                {
                    this.groupBox4.Enabled = true;
                    this.groupBox4.Visible = true;
                }
                //System.Windows.Forms.Application.DoEvents();
                this.extAppDocIDTextBox.Text = dtst.Tables[0].Rows[i][17].ToString();
                this.extAppDocNoTextBox.Text = dtst.Tables[0].Rows[i][18].ToString();
                this.extAppDocTypTextBox.Text = dtst.Tables[0].Rows[i][19].ToString();
                this.autoBalscheckBox.Checked = Global.mnFrm.cmCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][20].ToString());
                this.allowDuesCheckBox.Checked = Global.mnFrm.cmCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][23].ToString());

                this.docDteTextBox.Text = dtst.Tables[0].Rows[i][4].ToString();

                this.cstmrIDTextBox.Text = dtst.Tables[0].Rows[i][5].ToString();
                this.cstmrNmTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                  "scm.scm_cstmr_suplr", "cust_sup_id", "cust_sup_name",
                  long.Parse(dtst.Tables[0].Rows[i][5].ToString()));

                this.siteIDTextBox.Text = dtst.Tables[0].Rows[i][6].ToString();
                this.siteNumTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                  "scm.scm_cstmr_suplr_sites", "cust_sup_site_id", "site_name",
                  long.Parse(dtst.Tables[0].Rows[i][6].ToString()));

                this.rgstrIDTextBox.Text = dtst.Tables[0].Rows[i][21].ToString();
                this.costCtgrTextBox.Text = dtst.Tables[0].Rows[i][22].ToString();

                if (dtst.Tables[0].Rows[i][24].ToString() == "Attendance Register")
                {
                    this.rgstrNumTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                      "attn.attn_attendance_recs_hdr", "recs_hdr_id", "recs_hdr_name",
                      long.Parse(dtst.Tables[0].Rows[i][21].ToString()));
                }
                else if (dtst.Tables[0].Rows[i][24].ToString() == "Production Process Run")
                {
                    this.rgstrNumTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
               "scm.scm_process_run", "process_run_id", "batch_code_num",
               long.Parse(dtst.Tables[0].Rows[i][21].ToString()));
                }
                else
                {
                    this.rgstrNumTextBox.Text = "";
                }

                this.lnkdEventComboBox.Items.Clear();
                this.lnkdEventComboBox.Items.Add(dtst.Tables[0].Rows[i][24].ToString());
                this.lnkdEventComboBox.SelectedItem = dtst.Tables[0].Rows[i][24].ToString();//;


                this.docCommentsTextBox.Text = dtst.Tables[0].Rows[i][7].ToString();
                this.payTermsTextBox.Text = dtst.Tables[0].Rows[i][8].ToString();
                this.apprvlStatusTextBox.Text = dtst.Tables[0].Rows[i][9].ToString();
                this.nxtApprvlStatusButton.Text = dtst.Tables[0].Rows[i][10].ToString();

                this.pymntMthdIDTextBox.Text = dtst.Tables[0].Rows[i][12].ToString();
                this.pymntMthdTextBox.Text = dtst.Tables[0].Rows[i][13].ToString();
                this.invcCurrIDTextBox.Text = dtst.Tables[0].Rows[i][14].ToString();
                this.invcCurrTextBox.Text = dtst.Tables[0].Rows[i][15].ToString();
                this.exchRateLabel.Text = "(" + this.curCode + "-" + this.invcCurrTextBox.Text + "):";
                this.exchRateNumUpDwn.Value = decimal.Parse(dtst.Tables[0].Rows[i][16].ToString());

                if (this.nxtApprvlStatusButton.Text == "Cancel")
                {
                    this.nxtApprvlStatusButton.ImageKey = "90.png";
                }
                else
                {
                    this.nxtApprvlStatusButton.ImageKey = "tick_64.png";
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
                  //&& this.nxtApprvlStatusButton.Text != "Initiate"
                  && this.nxtApprvlStatusButton.Text != "Cancel"
                  && this.nxtApprvlStatusButton.Text != "None")
                {
                    this.rejectDocButton.Enabled = true;
                }
                else
                {
                    this.rejectDocButton.Enabled = false;
                }
                if (this.apprvlStatusTextBox.Text == "Approved"
                  || this.apprvlStatusTextBox.Text == "Declared Bad Debt")
                {
                    this.badDebtButton.Enabled = true;
                }
                else
                {
                    this.badDebtButton.Enabled = false;
                }
                if (this.apprvlStatusTextBox.Text == "Declared Bad Debt")
                {
                    this.badDebtButton.Text = "Reverse Bad Debt";
                    this.badDebtButton.ImageKey = "undo_256.png";
                }
                else
                {
                    this.badDebtButton.Text = "Declare as Bad Debt";
                    this.badDebtButton.ImageKey = "blocked.png";
                }
                this.createdByIDTextBox.Text = dtst.Tables[0].Rows[i][11].ToString();
                this.createdByTextBox.Text = Global.mnFrm.cmCde.get_user_name(
                  long.Parse(dtst.Tables[0].Rows[i][11].ToString())).ToUpper();
            }
            if (this.apprvlStatusTextBox.Text == "Approved")
            {
                this.itemsDataGridView.Columns[30].Visible = false;
            }
            else
            {
                this.itemsDataGridView.Columns[30].Visible = true;
            }
            this.obey_evnts = true;
        }

        private void populateLines(long docHdrID, string docTyp)
        {
            this.saveLabel.Text = "Loading Lines...Please Wait...";
            this.saveLabel.Visible = true;
            System.Windows.Forms.Application.DoEvents();
            this.clearLnsInfo();
            if (docHdrID > 0 && this.addRec == false && this.editRec == false)
            {
                this.disableLnsEdit();
            }
            else if (this.addRec == true || this.editRec == true)
            {
                this.saveDtButton.Enabled = true;
                this.editDtButton.Enabled = false;
            }
            this.obey_evnts = false;
            //System.Windows.Forms.Application.DoEvents();
            string curnm = this.invcCurrTextBox.Text;
            this.itemsDataGridView.Columns[7].HeaderText = "Unit Price (" + curnm + ")";
            this.itemsDataGridView.Columns[8].HeaderText = "Amount (" + curnm + ")";
            //System.Windows.Forms.Application.DoEvents();

            DataSet dtst = Global.get_One_SalesDcLines(docHdrID);
            this.itemsDataGridView.Rows.Clear();
            // this.itemsDataGridView.RowCount = dtst.Tables[0].Rows.Count;
            long srcDocID = -1;
            long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr",
              "invc_hdr_id", "src_doc_hdr_id", docHdrID), out srcDocID);
            int rwcnt = dtst.Tables[0].Rows.Count;
            //System.Windows.Forms.Application.DoEvents();

            for (int i = 0; i < rwcnt; i++)
            {
                //System.Windows.Forms.Application.DoEvents();
                this.itemsDataGridView.RowCount += 1;//, this.apprvlStatusTextBox.Text.Insert(this.rgstrDetDataGridView.RowCount - 1, 1);
                int rowIdx = this.itemsDataGridView.RowCount - 1;

                this.itemsDataGridView.Rows[rowIdx].HeaderCell.Value = (i + 1).ToString();
                //Object[] cellDesc = new Object[27];
                this.itemsDataGridView.Rows[rowIdx].Cells[0].Value = dtst.Tables[0].Rows[i][16].ToString();/*Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_itm_list",
          "item_id", "item_code", long.Parse(dtst.Tables[0].Rows[i][1].ToString()));*/
                this.itemsDataGridView.Rows[rowIdx].Cells[1].Value = "...";
                this.itemsDataGridView.Rows[rowIdx].Cells[2].Value = dtst.Tables[0].Rows[i][17].ToString();/*Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_itm_list",
          "item_id", "item_desc", long.Parse(dtst.Tables[0].Rows[i][1].ToString()));*/
                this.itemsDataGridView.Rows[rowIdx].Cells[3].Value = "...";
                this.itemsDataGridView.Rows[rowIdx].Cells[4].Value = dtst.Tables[0].Rows[i][2].ToString();
                int uomid = -1;//uom_name
                int.TryParse(dtst.Tables[0].Rows[i][15].ToString(), out uomid);
                this.itemsDataGridView.Rows[rowIdx].Cells[5].Value = dtst.Tables[0].Rows[i][18].ToString();/*Global.mnFrm.cmCde.getGnrlRecNm("inv.unit_of_measure",
          "uom_id", "uom_name", uomid);*/
                this.itemsDataGridView.Rows[rowIdx].Cells[6].Value = "...";
                this.itemsDataGridView.Rows[rowIdx].Cells[7].Value = dtst.Tables[0].Rows[i][3].ToString();
                this.itemsDataGridView.Rows[rowIdx].Cells[8].Value = double.Parse(dtst.Tables[0].Rows[i][4].ToString()).ToString("#,##0.00");
                if (docTyp == "Pro-Forma Invoice"
                  || docTyp == "Internal Item Request")
                {
                    this.itemsDataGridView.Rows[rowIdx].Cells[9].Value = Global.get_One_LnTrnsctdQty(docHdrID
                      , long.Parse(dtst.Tables[0].Rows[i][0].ToString()));
                }
                else
                {
                    this.itemsDataGridView.Rows[rowIdx].Cells[9].Value = Global.get_One_AvlblSrcLnQty(
                      long.Parse(dtst.Tables[0].Rows[i][8].ToString()));
                }
                this.itemsDataGridView.Rows[rowIdx].Cells[10].Value = dtst.Tables[0].Rows[i][13].ToString();
                this.itemsDataGridView.Rows[rowIdx].Cells[11].Value = "...";
                this.itemsDataGridView.Rows[rowIdx].Cells[12].Value = dtst.Tables[0].Rows[i][1].ToString();
                this.itemsDataGridView.Rows[rowIdx].Cells[13].Value = dtst.Tables[0].Rows[i][5].ToString();
                this.itemsDataGridView.Rows[rowIdx].Cells[14].Value = dtst.Tables[0].Rows[i][6].ToString();
                this.itemsDataGridView.Rows[rowIdx].Cells[15].Value = dtst.Tables[0].Rows[i][0].ToString();
                this.itemsDataGridView.Rows[rowIdx].Cells[16].Value = dtst.Tables[0].Rows[i][8].ToString();
                //Tax
                this.itemsDataGridView.Rows[rowIdx].Cells[17].Value = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes",
                  "code_id", "code_name", long.Parse(dtst.Tables[0].Rows[i][9].ToString()));
                this.itemsDataGridView.Rows[rowIdx].Cells[18].Value = "...";
                this.itemsDataGridView.Rows[rowIdx].Cells[19].Value = dtst.Tables[0].Rows[i][9].ToString();
                //Discount
                this.itemsDataGridView.Rows[rowIdx].Cells[20].Value = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes",
                  "code_id", "code_name", long.Parse(dtst.Tables[0].Rows[i][10].ToString()));

                this.itemsDataGridView.Rows[rowIdx].Cells[21].Value = "...";
                this.itemsDataGridView.Rows[rowIdx].Cells[22].Value = dtst.Tables[0].Rows[i][10].ToString();
                //Extra Charge
                this.itemsDataGridView.Rows[rowIdx].Cells[23].Value = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes",
                  "code_id", "code_name", long.Parse(dtst.Tables[0].Rows[i][11].ToString()));
                this.itemsDataGridView.Rows[rowIdx].Cells[24].Value = "...";
                this.itemsDataGridView.Rows[rowIdx].Cells[25].Value = dtst.Tables[0].Rows[i][11].ToString();
                this.itemsDataGridView.Rows[rowIdx].Cells[26].Value = dtst.Tables[0].Rows[i][12].ToString();
                //this.itemsDataGridView.Rows[i].SetValues(cellDesc);
                this.itemsDataGridView.Rows[rowIdx].Cells[27].Value = dtst.Tables[0].Rows[i][24].ToString();
                this.itemsDataGridView.Rows[rowIdx].Cells[28].Value = dtst.Tables[0].Rows[i][23].ToString();
                this.itemsDataGridView.Rows[rowIdx].Cells[29].Value = "...";//dtst.Tables[0].Rows[i][12].ToString();
                if (this.apprvlStatusTextBox.Text == "Approved")
                {
                    this.itemsDataGridView.Rows[rowIdx].Cells[30].Value = "Dues Payments";//dtst.Tables[0].Rows[i][12].ToString();
                }
                else
                {
                    this.itemsDataGridView.Rows[rowIdx].Cells[30].Value = "Linked Person";//dtst.Tables[0].Rows[i][12].ToString();
                }
                this.itemsDataGridView.Rows[rowIdx].Cells[31].Value = dtst.Tables[0].Rows[i][25].ToString();
                this.itemsDataGridView.Rows[rowIdx].Cells[32].Value = "Change Accounts";
                this.itemsDataGridView.Rows[rowIdx].Cells[33].Value = dtst.Tables[0].Rows[i][27].ToString();
            }
            this.obey_evnts = true;
            this.saveLabel.Visible = false;
            //System.Windows.Forms.Application.DoEvents();
        }

        public int isItemThere(int itmID)
        {
            //, int storeID
            for (int i = 0; i < this.itemsDataGridView.RowCount; i++)
            {
                if (this.itemsDataGridView.Rows[i].Cells[12].Value == null)
                {
                    this.itemsDataGridView.Rows[i].Cells[12].Value = "-1";
                }
                //if (this.itemsDataGridView.Rows[i].Cells[9].Value == null)
                //{
                //  this.itemsDataGridView.Rows[i].Cells[9].Value = string.Empty;
                //}
                //  && this.itemsDataGridView.Rows[i].Cells[9].Value.ToString() == storeID.ToString()
                if (this.itemsDataGridView.Rows[i].Cells[12].Value.ToString() == itmID.ToString())
                {
                    return i;
                }
            }
            return -1;
        }

        public int getFreeRowIdx()
        {
            //, int storeID
            for (int i = 0; i < this.itemsDataGridView.RowCount; i++)
            {
                int itmid = 0;
                if (this.itemsDataGridView.Rows[i].Cells[12].Value == null)
                {
                    this.itemsDataGridView.Rows[i].Cells[12].Value = string.Empty;
                }
                int.TryParse(this.itemsDataGridView.Rows[i].Cells[12].Value.ToString(), out itmid);

                if (itmid <= 0)
                {
                    return i;
                }
            }
            return -1;
        }

        private void populateSrcDocLines(long docHdrID, string docTyp)
        {
            this.obey_evnts = false;
            string curnm = this.invcCurrTextBox.Text;

            //Global.mnFrm.cmCde.getPssblValNm(
            //Global.mnFrm.cmCde.getOrgFuncCurID(
            //Global.mnFrm.cmCde.Org_id));
            this.itemsDataGridView.Columns[7].HeaderText = "Unit Price (" + curnm + ")";
            this.itemsDataGridView.Columns[8].HeaderText = "Amount (" + curnm + ")";

            DataSet dtst = Global.get_One_SalesDcLines(docHdrID);
            //this.itemsDataGridView.Rows.Clear();
            //int prvCnt = this.itemsDataGridView.RowCount;
            //this.createPrchsDocRows(dtst.Tables[0].Rows.Count);
            //MessageBox.Show(dtst.Tables[0].Rows.Count.ToString());
            //double tst = 0;
            //System.Windows.Forms.Application.DoEvents();
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                if (this.isItemThere(int.Parse(dtst.Tables[0].Rows[i][1].ToString())) >= 0)
                {
                    continue;
                }
                //double.TryParse(dtst.Tables[0].Rows[i][9].ToString(), out tst);
                //if (tst <= 0)
                //{
                //  continue;
                //}
                int idx = this.getFreeRowIdx();
                if (idx < 0)
                {
                    this.itemsDataGridView.RowCount += 1;
                    idx = this.itemsDataGridView.RowCount - 1;
                }
                this.itemsDataGridView.Rows[idx].HeaderCell.Value = (i + 1).ToString();
                this.itemsDataGridView.Rows[idx].Cells[0].Value = Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_itm_list",
                  "item_id", "item_code", long.Parse(dtst.Tables[0].Rows[i][1].ToString()));
                this.itemsDataGridView.Rows[idx].Cells[1].Value = "...";
                this.itemsDataGridView.Rows[idx].Cells[2].Value = Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_itm_list",
                  "item_id", "item_desc", long.Parse(dtst.Tables[0].Rows[i][1].ToString()));
                this.itemsDataGridView.Rows[idx].Cells[3].Value = "...";
                this.itemsDataGridView.Rows[idx].Cells[4].Value = "0.00";
                int uomid = -1;//uom_name
                int.TryParse(dtst.Tables[0].Rows[i][15].ToString(), out uomid);
                this.itemsDataGridView.Rows[idx].Cells[5].Value = Global.mnFrm.cmCde.getGnrlRecNm("inv.unit_of_measure",
                  "uom_id", "uom_name", uomid);
                this.itemsDataGridView.Rows[idx].Cells[6].Value = "...";
                this.itemsDataGridView.Rows[idx].Cells[7].Value = dtst.Tables[0].Rows[i][3].ToString();
                this.itemsDataGridView.Rows[idx].Cells[8].Value = double.Parse(dtst.Tables[0].Rows[i][4].ToString()).ToString("#,##0.00");
                //          || docTyp == "Sales Order"

                if (docTyp == "Pro-Forma Invoice"
                  || docTyp == "Internal Item Request")
                {
                    this.itemsDataGridView.Rows[idx].Cells[9].Value = Global.get_One_LnTrnsctdQty(docHdrID
                      , long.Parse(dtst.Tables[0].Rows[i][0].ToString()));
                }
                else
                {
                    this.itemsDataGridView.Rows[idx].Cells[9].Value = Global.get_One_AvlblSrcLnQty(
                      long.Parse(dtst.Tables[0].Rows[i][0].ToString()));
                }
                this.itemsDataGridView.Rows[idx].Cells[10].Value = dtst.Tables[0].Rows[i][13].ToString();
                this.itemsDataGridView.Rows[idx].Cells[11].Value = "...";
                this.itemsDataGridView.Rows[idx].Cells[12].Value = dtst.Tables[0].Rows[i][1].ToString();
                this.itemsDataGridView.Rows[idx].Cells[13].Value = dtst.Tables[0].Rows[i][5].ToString();
                this.itemsDataGridView.Rows[idx].Cells[14].Value = dtst.Tables[0].Rows[i][6].ToString();
                this.itemsDataGridView.Rows[idx].Cells[15].Value = "-1";
                this.itemsDataGridView.Rows[idx].Cells[16].Value = dtst.Tables[0].Rows[i][0].ToString();
                //Tax
                this.itemsDataGridView.Rows[idx].Cells[17].Value = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes",
                  "code_id", "code_name", long.Parse(dtst.Tables[0].Rows[i][9].ToString()));
                this.itemsDataGridView.Rows[idx].Cells[18].Value = "...";
                this.itemsDataGridView.Rows[idx].Cells[19].Value = dtst.Tables[0].Rows[i][9].ToString();
                //Discount
                this.itemsDataGridView.Rows[idx].Cells[20].Value = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes",
                  "code_id", "code_name", long.Parse(dtst.Tables[0].Rows[i][10].ToString()));
                this.itemsDataGridView.Rows[idx].Cells[21].Value = "...";
                this.itemsDataGridView.Rows[idx].Cells[22].Value = dtst.Tables[0].Rows[i][10].ToString();
                //Extra Charge
                this.itemsDataGridView.Rows[idx].Cells[23].Value = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes",
                  "code_id", "code_name", long.Parse(dtst.Tables[0].Rows[i][11].ToString()));
                this.itemsDataGridView.Rows[idx].Cells[24].Value = "...";
                this.itemsDataGridView.Rows[idx].Cells[25].Value = dtst.Tables[0].Rows[i][11].ToString();
                this.itemsDataGridView.Rows[idx].Cells[26].Value = dtst.Tables[0].Rows[i][12].ToString();
                //this.itemsDataGridView.Rows[idx].Cells[27].Value = dtst.Tables[0].Rows[i][12].ToString();
            }

            long SIDocID = long.Parse(this.srcDocIDTextBox.Text);
            string strSrcDocType = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr",
              "invc_hdr_id", "invc_type", SIDocID);

            if ((this.docTypeComboBox.Text != "Sales Invoice"
              && this.docTypeComboBox.Text != "Sales Return")
              || (this.docTypeComboBox.Text == "Sales Return"
              && strSrcDocType != "Sales Invoice"))
            {
                this.groupBox4.Visible = false;
                //System.Windows.Forms.Application.DoEvents();
                this.groupBox4.Enabled = false;
            }
            else
            {
                this.groupBox4.Enabled = true;
                this.groupBox4.Visible = true;
            }
            this.obey_evnts = true;
            //System.Windows.Forms.Application.DoEvents();
        }

        private void populateSmmry(long docHdrID, string docTyp)
        {
            EventArgs e1 = new EventArgs();
            //if (this.editRec == false && this.addRec == false)
            //{
            //  this.docTypeComboBox_SelectedIndexChanged(this.docTypeComboBox, e1);
            //}
            //System.Windows.Forms.Application.DoEvents();
            string curnm = this.invcCurrTextBox.Text;
            DataSet dtst = Global.get_DocSmryLns(docHdrID, docTyp);
            this.smmryDataGridView.Rows.Clear();

            //this.smmryDataGridView.RowCount = dtst.Tables[0].Rows.Count;
            this.smmryDataGridView.Columns[1].HeaderText = "Amount (" + curnm + ")";
            this.obey_evnts = true;
            //      this.dteRcvdTextBox.Text = DateTime.ParseExact(
            //Global.mnFrm.cmCde.getDB_Date_time(), "yyyy-MM-dd HH:mm:ss",
            //System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy HH:mm:ss");
            //      this.pymntTypeComboBox.SelectedItem = "Cash";

            if (docHdrID < 0)
            {
                this.obey_evnts = true;
                return;
            }
            int rwcnt = dtst.Tables[0].Rows.Count;
            //System.Windows.Forms.Application.DoEvents();

            for (int i = 0; i < rwcnt; i++)
            {
                //System.Windows.Forms.Application.DoEvents();
                //Object[] cellDesc = new Object[6];
                this.smmryDataGridView.RowCount += 1;//, this.apprvlStatusTextBox.Text.Insert(this.rgstrDetDataGridView.RowCount - 1, 1);
                int rowIdx = this.smmryDataGridView.RowCount - 1;

                this.smmryDataGridView.Rows[rowIdx].HeaderCell.Value = (i + 1).ToString();

                this.smmryDataGridView.Rows[rowIdx].Cells[0].Value = dtst.Tables[0].Rows[i][1].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[1].Value = double.Parse(dtst.Tables[0].Rows[i][2].ToString()).ToString("#,##0.00");
                this.smmryDataGridView.Rows[rowIdx].Cells[2].Value = dtst.Tables[0].Rows[i][0].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[3].Value = dtst.Tables[0].Rows[i][3].ToString();
                this.smmryDataGridView.Rows[rowIdx].Cells[4].Value = Global.mnFrm.cmCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][5].ToString());
                this.smmryDataGridView.Rows[rowIdx].Cells[5].Value = dtst.Tables[0].Rows[i][4].ToString();
                // }
                //this.smmryDataGridView.Rows[i].SetValues(cellDesc);
                if (dtst.Tables[0].Rows[i][4].ToString() == "7Change/Balance")
                {
                    if (double.Parse(dtst.Tables[0].Rows[i][2].ToString()) > 0)
                    {
                        this.smmryDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 100, 100);
                    }
                    else
                    {
                        this.smmryDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                    }
                }
            }
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
                this.totl_rec = Global.get_Total_SalesDoc(this.searchForTextBox.Text,
                  this.searchInComboBox.Text, Global.mnFrm.cmCde.Org_id,
                  Global.wfnLftMnu.vwSelfCheckBox.Checked, this.showUnpaidCheckBox.Checked);
                this.updtTotals();
                this.rec_cur_indx = Global.mnFrm.cmCde.navFuncts.totalGroups - 1;
            }
            this.getPnlData();
        }

        private void clearDetInfo()
        {
            bool prv = this.obey_evnts;
            this.obey_evnts = false;

            this.saveButton.Enabled = false;
            this.docSaved = true;
            this.disableFormButtons();
            this.docIDTextBox.Text = "-1";
            this.docIDNumTextBox.Text = "";
            this.docCommentsTextBox.Text = "";
            this.docTypeComboBox.Items.Clear();
            this.docIDPrfxComboBox.Items.Clear();
            this.lnkdEventComboBox.Items.Clear();
            this.autoBalscheckBox.Checked = true;
            this.srcDocIDTextBox.Text = "-1";
            this.srcDocNumTextBox.Text = "";

            this.extAppDocIDTextBox.Text = "-1";
            this.extAppDocNoTextBox.Text = "";
            this.extAppDocTypTextBox.Text = "";
            this.allowDuesCheckBox.Checked = false;
            this.srcDocButton.Enabled = false;
            this.exchRateLabel.Text = "(" + this.curCode + "-" + this.curCode + "):";
            this.exchRateNumUpDwn.Value = 1;
            this.exchRateNumUpDwn.Increment = 0.1M;
            this.invcCurrIDTextBox.Text = "-1";
            this.invcCurrTextBox.Text = "";

            this.docCommentsTextBox.Text = "";

            this.srcDocIDTextBox.Text = "-1";
            this.srcDocNumTextBox.Text = "";
            this.pymntMthdIDTextBox.Text = "-1";
            this.pymntMthdTextBox.Text = "";

            this.createdByIDTextBox.Text = "-1";
            this.createdByTextBox.Text = "";

            this.cstmrIDTextBox.Text = "-1";
            this.cstmrNmTextBox.Text = "";

            this.rgstrIDTextBox.Text = "-1";
            this.rgstrNumTextBox.Text = "";
            this.costCtgrTextBox.Text = "";

            this.siteIDTextBox.Text = "-1";
            this.siteNumTextBox.Text = "";
            this.docDteTextBox.Text = "";
            this.payTermsTextBox.Text = "";
            this.apprvlStatusTextBox.Text = "Not Validated";
            this.nxtApprvlStatusButton.Text = "Approve";
            this.nxtApprvlStatusButton.ImageKey = "tick_64.png";
            //this.dteRcvdTextBox.Text = "";
            //this.pymntCmmntsTextBox.Text = "";
            //this.pymntTypeComboBox.SelectedIndex = -1;
            //this.amntRcvdNumUpDown.Value = 0;
            //this.changeNumUpDown.Value = 0;

            this.obey_evnts = prv;
        }

        private void prpareForDetEdit()
        {
            bool prv = this.obey_evnts;
            this.obey_evnts = false;
            this.saveButton.Enabled = true;
            this.docSaved = false;
            this.docIDNumTextBox.ReadOnly = false;
            this.docIDNumTextBox.BackColor = Color.FromArgb(255, 255, 128);
            this.docCommentsTextBox.ReadOnly = false;
            this.docCommentsTextBox.BackColor = Color.White;

            this.payTermsTextBox.ReadOnly = false;
            this.payTermsTextBox.BackColor = Color.White;

            this.docDteTextBox.ReadOnly = false;
            this.docDteTextBox.BackColor = Color.FromArgb(255, 255, 128);

            this.cstmrNmTextBox.ReadOnly = false;
            this.cstmrNmTextBox.BackColor = Color.White;

            this.rgstrNumTextBox.ReadOnly = true;
            this.rgstrNumTextBox.BackColor = Color.White;

            this.costCtgrTextBox.ReadOnly = true;
            this.costCtgrTextBox.BackColor = Color.White;

            string orgnlItm = this.lnkdEventComboBox.Text;
            this.lnkdEventComboBox.Items.Clear();
            this.lnkdEventComboBox.Items.Add("None");
            this.lnkdEventComboBox.Items.Add("Attendance Register");
            this.lnkdEventComboBox.Items.Add("Production Process Run");

            this.siteNumTextBox.ReadOnly = true;
            this.siteNumTextBox.BackColor = Color.WhiteSmoke;

            this.pymntMthdTextBox.ReadOnly = false;
            this.pymntMthdTextBox.BackColor = Color.FromArgb(255, 255, 128);

            this.invcCurrTextBox.ReadOnly = false;
            this.invcCurrTextBox.BackColor = Color.FromArgb(255, 255, 128);

            this.exchRateNumUpDwn.Increment = (decimal)1.1;
            this.exchRateNumUpDwn.ReadOnly = false;
            this.exchRateNumUpDwn.BackColor = Color.FromArgb(255, 255, 128);

            string selItm = this.docTypeComboBox.Text;
            this.docTypeComboBox.Items.Clear();
            this.docIDPrfxComboBox.Items.Clear();
            if (this.addRec == true)
            {
                if (this.addRecsPF == true || this.editRecsPF == true
                  || this.delRecsPF == true || this.vwRecsPF == true)
                {
                    this.docTypeComboBox.Items.Add("Pro-Forma Invoice");
                }
                if (this.addRecsSO == true || this.editRecsSO == true
                  || this.delRecsSO == true || this.vwRecsSO == true)
                {
                    this.docTypeComboBox.Items.Add("Sales Order");
                }
                if (this.addRecsSI == true || this.editRecsSI == true
                  || this.delRecsSI == true || this.vwRecsSI == true)
                {
                    this.docTypeComboBox.Items.Add("Sales Invoice");
                }
                if (this.addRecsIR == true || this.editRecsIR == true
                  || this.delRecsIR == true || this.vwRecsIR == true)
                {
                    this.docTypeComboBox.Items.Add("Internal Item Request");
                }
                if (this.addRecsII == true || this.editRecsII == true
                  || this.delRecsII == true || this.vwRecsII == true)
                {
                    this.docTypeComboBox.Items.Add("Item Issue-Unbilled");
                }
                if (this.addRecsSR == true || this.editRecsSR == true
                  || this.delRecsSR == true || this.vwRecsSR == true)
                {
                    this.docTypeComboBox.Items.Add("Sales Return");
                }
            }
            if (this.editRec == true)
            {
                this.docTypeComboBox.Items.Add(selItm);
                this.docTypeComboBox.SelectedItem = selItm;
                this.lnkdEventComboBox.SelectedItem = orgnlItm;
            }
            this.obey_evnts = prv;
        }

        private void disableDetEdit()
        {
            this.addRec = false;
            this.editRec = false;
            this.saveButton.Enabled = false;
            this.docSaved = true;
            this.docIDNumTextBox.ReadOnly = true;
            this.docIDNumTextBox.BackColor = Color.WhiteSmoke;
            this.docCommentsTextBox.ReadOnly = true;
            this.docCommentsTextBox.BackColor = Color.WhiteSmoke;

            this.payTermsTextBox.ReadOnly = true;
            this.payTermsTextBox.BackColor = Color.WhiteSmoke;

            this.extAppDocIDTextBox.ReadOnly = true;
            this.extAppDocNoTextBox.ReadOnly = true;
            this.extAppDocTypTextBox.ReadOnly = true;

            this.extAppDocIDTextBox.BackColor = Color.WhiteSmoke;
            this.extAppDocNoTextBox.BackColor = Color.WhiteSmoke;
            this.extAppDocTypTextBox.BackColor = Color.WhiteSmoke;

            this.rgstrNumTextBox.ReadOnly = true;
            this.rgstrNumTextBox.BackColor = Color.WhiteSmoke;

            this.costCtgrTextBox.ReadOnly = true;
            this.costCtgrTextBox.BackColor = Color.WhiteSmoke;

            this.docDteTextBox.ReadOnly = true;
            this.docDteTextBox.BackColor = Color.WhiteSmoke;

            this.cstmrNmTextBox.ReadOnly = true;
            this.cstmrNmTextBox.BackColor = Color.WhiteSmoke;

            this.srcDocNumTextBox.ReadOnly = true;
            this.srcDocNumTextBox.BackColor = Color.WhiteSmoke;

            this.siteNumTextBox.ReadOnly = true;
            this.siteNumTextBox.BackColor = Color.WhiteSmoke;
            this.pymntMthdTextBox.ReadOnly = true;
            this.pymntMthdTextBox.BackColor = Color.WhiteSmoke;

            this.invcCurrTextBox.ReadOnly = true;
            this.invcCurrTextBox.BackColor = Color.WhiteSmoke;
            this.exchRateNumUpDwn.Increment = (decimal)0;
            this.exchRateNumUpDwn.ReadOnly = true;
            this.exchRateNumUpDwn.BackColor = Color.WhiteSmoke;

            this.addPRFButton.Enabled = this.addRecsPF;
            this.addSOButton.Enabled = this.addRecsSO;
            this.addSIButton.Enabled = this.addRecsSI;
            this.addIRButton.Enabled = this.addRecsIR;
            this.addUIIButton.Enabled = this.addRecsII;
            this.addSRButton.Enabled = this.addRecsSR;

            if (this.docTypeComboBox.Text == "Pro-Forma Invoice")
            {
                this.editButton.Enabled = this.editRecsPF;
                //this.addSIButton.Enabled = this.addRecsPF;
            }
            else if (this.docTypeComboBox.Text == "Sales Order")
            {
                this.editButton.Enabled = this.editRecsSO;
                //this.addSIButton.Enabled = this.addRecsSO;
            }
            else if (this.docTypeComboBox.Text == "Sales Invoice")
            {
                this.editButton.Enabled = this.editRecsSI;
                //this.addSIButton.Enabled = this.addRecsSI;
            }
            else if (this.docTypeComboBox.Text == "Internal Item Request")
            {
                this.editButton.Enabled = this.editRecsIR;
                //this.addSIButton.Enabled = this.addRecsIR;
            }
            else if (this.docTypeComboBox.Text == "Item Issue-Unbilled")
            {
                this.editButton.Enabled = this.editRecsII;
                //this.addSIButton.Enabled = this.addRecsII;
            }
            else if (this.docTypeComboBox.Text == "Sales Return")
            {
                this.editButton.Enabled = this.editRecsSR;
                //this.addSIButton.Enabled = this.addRecsSR;
            }
        }

        private void clearLnsInfo()
        {
            bool prv = this.obey_evnts;
            this.obey_evnts = false;

            this.saveDtButton.Enabled = false;
            this.docSaved = true;
            this.itemsDataGridView.Rows.Clear();
            this.smmryDataGridView.Rows.Clear();
            this.itemsDataGridView.Columns[7].HeaderText = "Unit Price";
            this.itemsDataGridView.Columns[8].HeaderText = "Amount";
            this.itemsDataGridView.DefaultCellStyle.ForeColor = Color.Black;
            this.smmryDataGridView.DefaultCellStyle.ForeColor = Color.Black;
            this.obey_evnts = prv;
        }

        private void prpareForLnsEdit()
        {
            this.saveDtButton.Enabled = true;
            this.docSaved = false;
            this.itemsDataGridView.ReadOnly = false;
            this.itemsDataGridView.Columns[0].ReadOnly = false;
            this.itemsDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.itemsDataGridView.Columns[2].ReadOnly = false;
            this.itemsDataGridView.Columns[2].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.itemsDataGridView.Columns[4].ReadOnly = false;
            this.itemsDataGridView.Columns[4].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.itemsDataGridView.Columns[5].ReadOnly = true;
            this.itemsDataGridView.Columns[5].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.itemsDataGridView.Columns[7].ReadOnly = false;
            this.itemsDataGridView.Columns[7].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);

            this.itemsDataGridView.Columns[8].ReadOnly = true;
            this.itemsDataGridView.Columns[8].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.itemsDataGridView.Columns[9].ReadOnly = true;
            this.itemsDataGridView.Columns[9].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.itemsDataGridView.Columns[10].ReadOnly = false;
            this.itemsDataGridView.Columns[10].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            this.itemsDataGridView.Columns[17].ReadOnly = false;
            this.itemsDataGridView.Columns[17].DefaultCellStyle.BackColor = Color.White;
            this.itemsDataGridView.Columns[20].ReadOnly = false;
            this.itemsDataGridView.Columns[20].DefaultCellStyle.BackColor = Color.White;
            this.itemsDataGridView.Columns[23].ReadOnly = false;
            this.itemsDataGridView.Columns[23].DefaultCellStyle.BackColor = Color.White;
            this.itemsDataGridView.Columns[27].ReadOnly = true;
            this.itemsDataGridView.Columns[27].DefaultCellStyle.BackColor = Color.White;

            this.itemsDataGridView.Columns[26].ReadOnly = false;
            this.itemsDataGridView.Columns[26].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            if (this.docTypeComboBox.Text == "Sales Return")
            {
                this.itemsDataGridView.Columns[0].ReadOnly = true;
                this.itemsDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.Gainsboro;
                this.itemsDataGridView.Columns[2].ReadOnly = true;
                this.itemsDataGridView.Columns[2].DefaultCellStyle.BackColor = Color.Gainsboro;
                this.itemsDataGridView.Columns[1].Visible = false;
                this.itemsDataGridView.Columns[3].Visible = false;
            }
        }

        private void disableLnsEdit()
        {
            this.addDtRec = false;
            this.editDtRec = false;
            this.saveDtButton.Enabled = false;
            this.docSaved = true;
            this.itemsDataGridView.ReadOnly = true;
            this.itemsDataGridView.Columns[0].ReadOnly = true;
            this.itemsDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.itemsDataGridView.Columns[2].ReadOnly = true;
            this.itemsDataGridView.Columns[2].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.itemsDataGridView.Columns[4].ReadOnly = true;
            this.itemsDataGridView.Columns[4].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.itemsDataGridView.Columns[5].ReadOnly = true;
            this.itemsDataGridView.Columns[5].DefaultCellStyle.BackColor = Color.Gainsboro;

            this.itemsDataGridView.Columns[7].ReadOnly = true;
            this.itemsDataGridView.Columns[7].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.itemsDataGridView.Columns[8].ReadOnly = true;
            this.itemsDataGridView.Columns[8].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.itemsDataGridView.Columns[9].ReadOnly = true;
            this.itemsDataGridView.Columns[9].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.itemsDataGridView.Columns[10].ReadOnly = true;
            this.itemsDataGridView.Columns[10].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.itemsDataGridView.Columns[17].ReadOnly = true;
            this.itemsDataGridView.Columns[17].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.itemsDataGridView.Columns[20].ReadOnly = true;
            this.itemsDataGridView.Columns[20].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.itemsDataGridView.Columns[23].ReadOnly = true;
            this.itemsDataGridView.Columns[23].DefaultCellStyle.BackColor = Color.Gainsboro;

            this.itemsDataGridView.Columns[27].ReadOnly = true;
            this.itemsDataGridView.Columns[27].DefaultCellStyle.BackColor = Color.Gainsboro;

            this.itemsDataGridView.Columns[26].ReadOnly = true;
            this.itemsDataGridView.Columns[26].DefaultCellStyle.BackColor = Color.Gainsboro;
            this.itemsDataGridView.ReadOnly = true;
            //this.itemsDataGridView.Columns[0].ReadOnly = true;
            //this.itemsDataGridView.Columns[1].DefaultCellStyle.BackColor = Color.Gainsboro;
            if (this.docTypeComboBox.Text == "Pro-Forma Invoice")
            {
                this.editDtButton.Enabled = this.editRecsPF;
                this.addDtButton.Enabled = this.addRecsPF;
            }
            else if (this.docTypeComboBox.Text == "Sales Order")
            {
                this.editDtButton.Enabled = this.editRecsSO;
                this.addDtButton.Enabled = this.addRecsSO;
            }
            else if (this.docTypeComboBox.Text == "Sales Invoice")
            {
                this.editDtButton.Enabled = this.editRecsSI;
                this.addDtButton.Enabled = this.addRecsSI;
            }
            else if (this.docTypeComboBox.Text == "Internal Item Request")
            {
                this.editDtButton.Enabled = this.editRecsIR;
                this.addDtButton.Enabled = this.addRecsIR;
            }
            else if (this.docTypeComboBox.Text == "Item Issue-Unbilled")
            {
                this.editDtButton.Enabled = this.editRecsII;
                this.addDtButton.Enabled = this.addRecsII;
            }
            else if (this.docTypeComboBox.Text == "Sales Return")
            {
                this.editDtButton.Enabled = this.editRecsSR;
                this.addDtButton.Enabled = this.addRecsSR;
            }
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

        #endregion

        #region "EVENT HANDLERS..."
        private void goButton_Click(object sender, EventArgs e)
        {
            this.loadPanel();
        }

        private void rfrshButton_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.minimizeMemory();
            this.loadPanel();
        }

        private void rfrshDtButton_Click(object sender, EventArgs e)
        {
            this.saveLabel.Visible = false;
            Cursor.Current = Cursors.Default;

            if (this.docIDTextBox.Text != "")
            {
                this.populateLines(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
                this.populateSmmry(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
            }
            else
            {
                this.populateLines(-1000, "");
                this.populateSmmry(-1000, "");
            }
            if (this.editRec == true || this.addRec == true)
            {
                this.saveDtButton.Enabled = true;
                this.editDtButton.Enabled = false;
                SendKeys.Send("{TAB}");
                SendKeys.Send("{HOME}");
            }
        }

        private void docTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            long SIDocID = long.Parse(this.srcDocIDTextBox.Text);
            string strSrcDocType = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr",
              "invc_hdr_id", "invc_type", SIDocID);

            if ((this.docTypeComboBox.Text != "Sales Invoice"
              && this.docTypeComboBox.Text != "Sales Return")
              || (this.docTypeComboBox.Text == "Sales Return"
              && strSrcDocType != "Sales Invoice"))
            {
                this.groupBox4.Visible = false;
                this.groupBox4.Enabled = false;
            }
            else
            {
                this.groupBox4.Enabled = true;
                this.groupBox4.Visible = true;
            }
            //System.Windows.Forms.Application.DoEvents();
            if (this.shdObeyEvts() == false)
            {
                return;
            }


            this.changeGridVw();

            if (this.docTypeComboBox.Text == "Pro-Forma Invoice"
              || this.docTypeComboBox.Text == "Internal Item Request")
            {
                this.srcDocIDTextBox.Text = "-1";
                this.srcDocNumTextBox.Text = "";
                this.srcDocButton.Enabled = false;
                this.srcDocNumTextBox.ReadOnly = true;
                this.srcDocNumTextBox.BackColor = Color.WhiteSmoke;

                this.docIDPrfxComboBox.Items.Clear();
                if (this.addRec == true || this.editRec == true)
                {
                    if (this.docTypeComboBox.Text == "Pro-Forma Invoice")
                    {
                        this.docIDPrfxComboBox.Items.Add("PFI");
                    }
                    else
                    {
                        this.docIDPrfxComboBox.Items.Add("IIR");
                    }
                    this.docIDPrfxComboBox.SelectedIndex = 0;
                }
            }
            else if (this.docTypeComboBox.Text == "Sales Order"
              || this.docTypeComboBox.Text == "Sales Invoice"
              || this.docTypeComboBox.Text == "Item Issue-Unbilled"
              || this.docTypeComboBox.Text == "Sales Return")
            {
                this.srcDocButton.Enabled = true;

                this.docIDPrfxComboBox.Items.Clear();
                if (this.addRec == true || this.editRec == true)
                {
                    if (this.docTypeComboBox.Text == "Sales Invoice")
                    {
                        this.docIDPrfxComboBox.Items.Add("SI");
                    }
                    else if (this.docTypeComboBox.Text == "Sales Order")
                    {
                        this.docIDPrfxComboBox.Items.Add("SO");
                    }
                    else if (this.docTypeComboBox.Text == "Item Issue-Unbilled")
                    {
                        this.docIDPrfxComboBox.Items.Add("IIU");
                    }
                    else if (this.docTypeComboBox.Text == "Sales Return")
                    {
                        this.docIDPrfxComboBox.Items.Add("SR");
                    }
                    this.docIDPrfxComboBox.SelectedIndex = 0;
                }
            }
            if (this.editRec == true || this.addRec == true)
            {
                this.srcDocIDTextBox.Text = "-1";
                this.srcDocNumTextBox.Text = "";
                if (this.srcDocButton.Enabled == true)
                {
                    this.srcDocNumTextBox.ReadOnly = false;
                    this.srcDocNumTextBox.BackColor = Color.White;
                }
                this.itemsDataGridView.Rows.Clear();
                this.createSalesDocRows(1);
            }
        }

        private void changeGridVw()
        {
            this.itemsSoldPdfButton.Visible = true;
            this.itemsDataGridView.Columns[27].Visible = false;
            this.itemsDataGridView.Columns[29].Visible = false;
            this.itemsDataGridView.Columns[30].Visible = false;
            this.itemsDataGridView.Columns[32].Visible = false;
            this.itemsDataGridView.Columns[5].Visible = true;
            this.itemsDataGridView.Columns[0].Width = 170;
            this.itemsDataGridView.Columns[31].Width = 170;
            //this.itemsDataGridView.Columns[18].HeaderText = "...";
            //this.itemsDataGridView.Columns[21].HeaderText = "...";
            this.itemsDataGridView.Columns[17].Visible = false;
            this.itemsDataGridView.Columns[18].HeaderText = "TX";
            this.itemsDataGridView.Columns[21].HeaderText = "DC";
            this.itemsDataGridView.Columns[24].HeaderText = "EX";
            this.itemsDataGridView.Columns[20].Visible = false;
            this.itemsDataGridView.Columns[23].Visible = false;
            this.itemsDataGridView.Columns[18].Visible = true;
            this.itemsDataGridView.Columns[21].Visible = true;
            if ((this.addRec || this.editRec))
            {
                this.itemsDataGridView.Columns[7].ReadOnly = false;
                this.itemsDataGridView.Columns[7].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
            }

            if (this.docTypeComboBox.Text == "Sales Return")
            {
                this.itemsDataGridView.Columns[11].Visible = false;
                this.itemsDataGridView.Columns[17].Visible = false;
                this.itemsDataGridView.Columns[18].Visible = false;
                this.itemsDataGridView.Columns[19].Visible = false;
                this.itemsDataGridView.Columns[20].Visible = false;
                this.itemsDataGridView.Columns[21].Visible = false;
                this.itemsDataGridView.Columns[22].Visible = false;
                this.dscntButton.Enabled = false;
                this.itemsDataGridView.Columns[24].Visible = false;
                this.itemsDataGridView.Columns[25].Visible = false;
                this.itemsDataGridView.Columns[26].Visible = true;
                this.itemsDataGridView.Columns[0].ReadOnly = true;
                this.itemsDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.Gainsboro;
                this.itemsDataGridView.Columns[2].ReadOnly = true;
                this.itemsDataGridView.Columns[2].DefaultCellStyle.BackColor = Color.Gainsboro;
                this.itemsDataGridView.Columns[1].Visible = false;
                this.itemsDataGridView.Columns[3].Visible = false;
                if (this.editDtRec == true
                  || this.addDtRec == true)
                {
                    this.itemsDataGridView.Columns[26].ReadOnly = false;
                    this.itemsDataGridView.Columns[26].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
                }
                else
                {
                    this.itemsDataGridView.Columns[26].ReadOnly = true;
                    this.itemsDataGridView.Columns[26].DefaultCellStyle.BackColor = Color.Gainsboro;
                }
            }
            else
            {
                this.itemsDataGridView.Columns[11].Visible = true;
                this.itemsDataGridView.Columns[17].Visible = false;
                this.itemsDataGridView.Columns[18].Visible = true;
                this.itemsDataGridView.Columns[19].Visible = false;
                this.itemsDataGridView.Columns[20].Visible = false;
                this.itemsDataGridView.Columns[21].Visible = true;
                this.itemsDataGridView.Columns[22].Visible = false;
                this.dscntButton.Enabled = true;
                this.itemsDataGridView.Columns[23].Visible = false;
                this.itemsDataGridView.Columns[24].Visible = false;
                this.itemsDataGridView.Columns[25].Visible = false;
                this.itemsDataGridView.Columns[26].Visible = false;
                if (this.editDtRec == true
                  || this.addDtRec == true)
                {
                    this.itemsDataGridView.Columns[0].ReadOnly = false;
                    this.itemsDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
                    this.itemsDataGridView.Columns[2].ReadOnly = false;
                    this.itemsDataGridView.Columns[2].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
                }
                else
                {
                    this.itemsDataGridView.Columns[0].ReadOnly = true;
                    this.itemsDataGridView.Columns[0].DefaultCellStyle.BackColor = Color.Gainsboro;
                    this.itemsDataGridView.Columns[2].ReadOnly = true;
                    this.itemsDataGridView.Columns[2].DefaultCellStyle.BackColor = Color.Gainsboro;
                }
                this.itemsDataGridView.Columns[1].Visible = true;
                this.itemsDataGridView.Columns[3].Visible = false;
                if (this.allowDuesCheckBox.Checked)
                {
                    this.itemsDataGridView.Columns[27].Visible = true;
                    this.itemsDataGridView.Columns[29].Visible = false;
                    if (this.apprvlStatusTextBox.Text == "Approved")
                    {
                        this.itemsDataGridView.Columns[30].Visible = false;
                    }
                    else
                    {
                        this.itemsDataGridView.Columns[30].Visible = true;
                    }
                    this.itemsSoldPdfButton.Visible = true;
                }
                if (this.docTypeComboBox.Text == "Item Issue-Unbilled")
                {
                    this.itemsDataGridView.Columns[32].Visible = true;
                    this.itemsDataGridView.Columns[18].Visible = false;
                    this.itemsDataGridView.Columns[21].Visible = false;
                }
            }

            if (this.docTypeComboBox.Text == "Pro-Forma Invoice"
              || this.docTypeComboBox.Text == "Internal Item Request")
            {
                this.itemsDataGridView.Columns[9].HeaderText = "Qty Used in Other Docs.";
                this.itemsDataGridView.Columns[9].Visible = true;
            }
            else
            {
                this.itemsDataGridView.Columns[9].Visible = false;
                this.itemsDataGridView.Columns[9].HeaderText = "Avlbl Qty (Source Doc.)";
            }
        }

        private void docIDPrfxComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.shdObeyEvts() == false)
            {
                return;
            }
            if (!this.docIDNumTextBox.Text.Contains(this.docIDPrfxComboBox.Text))
            {
                string dte = DateTime.Parse(Global.mnFrm.cmCde.getFrmtdDB_Date_time()).ToString("yyMMdd");
                this.docIDNumTextBox.Text = this.docIDPrfxComboBox.Text + dte
                          + "-" + (Global.mnFrm.cmCde.getRecCount("scm.scm_sales_invc_hdr", "invc_number",
                          "invc_hdr_id", this.docIDPrfxComboBox.Text + dte + "-%") + 1).ToString().PadLeft(3, '0')
                          + "-" + Global.mnFrm.cmCde.getRandomInt(100, 1000);
                //.Substring(2, 17).Replace(":", "").Replace("-", "").Replace(" ", "")

                /*this.docIDNumTextBox.Text = this.docIDPrfxComboBox.Text +
            Global.getLtstInvcIDNoInPrfx(this.docIDPrfxComboBox.Text) + "-" +
            Global.mnFrm.cmCde.getFrmtdDB_Date_time().Substring(12, 8).Replace(":", "") + "-" + Global.getLtstRecPkID("scm.scm_sales_invc_hdr",
            "invc_hdr_id");*/

                //this.docIDNumTextBox.Text = this.docIDPrfxComboBox.Text +
                //Global.getLtstRecPkID("scm.scm_sales_invc_hdr",
                //"invc_hdr_id");
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
            //this.exchRateNumUpDwn.Value = 0;
            this.updtRates();
        }

        private void srcDocButton_Click(object sender, EventArgs e)
        {
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("You must be in ADD/EDIT mode first!", 0);
                return;
            }
            this.srcDocLOVSrch();
        }

        private void sponsorLOVSrch(bool autoLoad)
        {
            this.txtChngd = false;
            long cstspplID = long.Parse(this.cstmrIDTextBox.Text);
            long siteID = long.Parse(this.siteIDTextBox.Text);
            bool isReadOnly = true;
            if (this.addRec || this.editRec)
            {
                isReadOnly = false;
            }
            Global.mnFrm.cmCde.showCstSpplrDiag(ref cstspplID, ref siteID, true, false, this.srchWrd,
              "Customer/Supplier Name", autoLoad, isReadOnly, Global.mnFrm.cmCde, "Customer");

            this.cstmrIDTextBox.Text = cstspplID.ToString();
            this.siteIDTextBox.Text = siteID.ToString();
            this.cstmrNmTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                "scm.scm_cstmr_suplr", "cust_sup_id", "cust_sup_name",
                cstspplID);
            this.siteNumTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
        "scm.scm_cstmr_suplr_sites", "cust_sup_site_id", "site_name",
        siteID);

            this.txtChngd = false;
        }

        private void cstmrButton_Click(object sender, EventArgs e)
        {
            this.sponsorLOVSrch(false);
            //if (this.addRec == false && this.editRec == false)
            //{
            //  Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
            //  return;
            //}
            //string[] selVals = new string[1];
            //selVals[0] = this.cstmrIDTextBox.Text;
            //DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
            //    Global.mnFrm.cmCde.getLovID("Customers"), ref selVals,
            //    true, false, Global.mnFrm.cmCde.Org_id);
            //if (dgRes == DialogResult.OK)
            //{
            //  for (int i = 0; i < selVals.Length; i++)
            //  {
            //    this.cstmrIDTextBox.Text = selVals[i];
            //    this.cstmrNmTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
            //      "scm.scm_cstmr_suplr", "cust_sup_id", "cust_sup_name",
            //      long.Parse(selVals[i]));
            //  }
            //}
        }

        private void siteButton_Click(object sender, EventArgs e)
        {
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
            string[] selVals = new string[1];
            selVals[0] = this.siteIDTextBox.Text;
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                Global.mnFrm.cmCde.getLovID("Customer/Supplier Sites"), ref selVals,
                true, false, int.Parse(this.cstmrIDTextBox.Text));
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.siteIDTextBox.Text = selVals[i];
                    this.siteNumTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                      "scm.scm_cstmr_suplr_sites", "cust_sup_site_id", "site_name",
                      long.Parse(selVals[i]));
                }
            }
        }

        private double sumGridStckQtys(long itmID, long storeID, ref string cnsIDs)
        {
            double rslt = 0;
            cnsIDs = "";
            string nwCsgID = "";
            for (int i = 0; i < this.itemsDataGridView.Rows.Count; i++)
            {
                this.dfltFill(i);
                if (itmID == int.Parse(this.itemsDataGridView.Rows[i].Cells[12].Value.ToString())
                  && storeID == int.Parse(this.itemsDataGridView.Rows[i].Cells[13].Value.ToString()))
                {
                    rslt += double.Parse(this.itemsDataGridView.Rows[i].Cells[4].Value.ToString());
                    if (this.itemsDataGridView.Rows[i].Cells[10].Value.ToString() == "")
                    {
                        nwCsgID = Global.getOldstItmCnsgmts(itmID, rslt);
                        this.itemsDataGridView.Rows[i].Cells[10].Value = nwCsgID;
                        cnsIDs += nwCsgID + ",";
                    }
                    else
                    {
                        if (Global.getCnsgmtsQtySum(cnsIDs) < rslt)
                        {
                            nwCsgID = Global.getOldstItmCnsgmts(itmID, rslt);
                            this.itemsDataGridView.Rows[i].Cells[10].Value = nwCsgID;
                            cnsIDs += nwCsgID + ",";
                        }
                        else
                        {
                            cnsIDs += this.itemsDataGridView.Rows[i].Cells[10].Value.ToString() + ",";
                        }
                    }
                }
            }
            return Math.Round(rslt, 2);
        }

        private void sumGridAmounts()
        {
            double rslt = 0;
            for (int i = 0; i < this.itemsDataGridView.Rows.Count; i++)
            {
                this.dfltFill(i);
                rslt += double.Parse(this.itemsDataGridView.Rows[i].Cells[8].Value.ToString());
            }
            this.smmryDataGridView.Rows.Clear();
            this.smmryDataGridView.RowCount = 1;
            this.smmryDataGridView.Rows[0].Cells[0].Value = "Grand Total";
            this.smmryDataGridView.Rows[0].Cells[1].Value = Math.Round(rslt, 2).ToString("#,##0.00");
            this.smmryDataGridView.Rows[0].Cells[2].Value = -1;
            this.smmryDataGridView.Rows[0].Cells[3].Value = -1;
            this.smmryDataGridView.Rows[0].Cells[4].Value = false;
            this.smmryDataGridView.Rows[0].Cells[5].Value = "";
        }

        //private double sumConsgnQtys(long itmID, ref string cnsIDs)
        //{
        //  double rslt = 0;
        //  for (int i = 0; i < this.itemsDataGridView.Rows.Count; i++)
        //  {
        //    this.dfltFill(i);
        //    if (itmID == int.Parse(this.itemsDataGridView.Rows[i].Cells[12].Value.ToString())
        //      && cnsIDs.Contains(this.itemsDataGridView.Rows[i].Cells[10].Value.ToString()))
        //    {
        //      rslt += double.Parse(this.itemsDataGridView.Rows[i].Cells[4].Value.ToString());
        //      cnsIDs += this.itemsDataGridView.Rows[i].Cells[10].Value.ToString() + ",";
        //    }
        //  }
        //  return Math.Round(rslt, 2);
        //}

        public bool validateLns(string srcDocType)
        {
            if (this.itemsDataGridView.Rows.Count <= 0)
            {
                //Global.mnFrm.cmCde.showMsg("The Document has no Items hence cannot be Validated!", 0);
                return true;
            }

            for (int i = 0; i < this.itemsDataGridView.Rows.Count; i++)
            {
                string dateStr = DateTime.ParseExact(
            Global.mnFrm.cmCde.getDB_Date_time(), "yyyy-MM-dd HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy HH:mm:ss");
                long itmID = -1;
                long storeID = -1;
                long lineid = long.Parse(this.itemsDataGridView.Rows[i].Cells[15].Value.ToString());
                long srclineID = long.Parse(this.itemsDataGridView.Rows[i].Cells[16].Value.ToString());
                long.TryParse(this.itemsDataGridView.Rows[i].Cells[12].Value.ToString(), out itmID);
                string itmType = Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_itm_list", "item_id", "item_type", itmID);
                long.TryParse(this.itemsDataGridView.Rows[i].Cells[13].Value.ToString(), out storeID);
                long stckID = Global.getItemStockID(itmID, storeID);
                string cnsgmntIDs = this.itemsDataGridView.Rows[i].Cells[10].Value.ToString();
                double tst1 = 0;
                double tst2 = 0;

                double.TryParse(this.itemsDataGridView.Rows[i].Cells[4].Value.ToString(), out tst1);
                double.TryParse(this.itemsDataGridView.Rows[i].Cells[9].Value.ToString(), out tst2);
                if (this.itemsDataGridView.Rows[i].Cells[16].Value.ToString() != "-1")
                {
                    if (tst1 > tst2 && itmType != "Services")
                    {
                        Global.mnFrm.cmCde.showMsg("Document Quantity in Row(" + (i + 1).ToString() +
                          ") cannot EXCEED Available Source Doc. Quantity!", 0);
                        return false;
                    }
                }

                if (tst1 > Global.getCnsgmtsQtySum(cnsgmntIDs))
                {
                    if (this.itemsDataGridView.Rows[i].Cells[16].Value.ToString() == "-1")
                    {
                        cnsgmntIDs = Global.getOldstItmCnsgmts(
                          long.Parse(this.itemsDataGridView.Rows[i].Cells[12].Value.ToString()), tst1);

                        this.itemsDataGridView.Rows[i].Cells[10].Value = cnsgmntIDs;
                        Global.updateSalesLnCsgmtIDs(lineid, cnsgmntIDs);
                    }
                }

                bool isPrevdlvrd = Global.mnFrm.cmCde.cnvrtBitStrToBool(
        Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_det", "invc_det_ln_id", "is_itm_delivered", lineid));

                if (isPrevdlvrd == false)
                {
                    string nwCnsgIDs = cnsgmntIDs;
                    double ttlItmStckQty = this.sumGridStckQtys(itmID, storeID, ref nwCnsgIDs);
                    double ttlItmCnsgQty = ttlItmStckQty;// this.sumConsgnQtys(itmID, ref nwCnsgIDs);

                    if (this.docTypeComboBox.Text != "Sales Return"
                      && this.docTypeComboBox.Text != "Internal Item Request"
                      && itmType != "Services"
                      && srcDocType != "Sales Order")
                    {
                        double kk1 = Global.getStockLstAvlblBls(stckID, dateStr);
                        if (tst1 > kk1
                          || ttlItmStckQty > kk1)
                        {
                            Global.mnFrm.cmCde.showMsg("Quantity in Row(" + (i + 1).ToString() +
                         ") cannot EXCEED Available Stock[" + Global.getStoreNm(storeID) +
                       "] Quantity[" + kk1 + "] hence cannot be delivered!!", 0);
                            return false;
                        }
                        kk1 = Global.getCnsgmtsQtySum(nwCnsgIDs);
                        if (tst1 > kk1
                          || ttlItmCnsgQty > kk1)
                        {
                            Global.mnFrm.cmCde.showMsg("Quantity in Row(" + (i + 1).ToString() +
                           ") cannot EXCEED Available Quantity[" + kk1 + "] in the Selected Consignments["
                         + nwCnsgIDs + "] hence cannot be delivered!!", 0);
                            return false;
                        }
                    }
                    else if (srcDocType == "Sales Order" && srclineID > 0)
                    {
                        double kk1 = Global.getStockLstRsvdBls(stckID, dateStr);
                        if (tst1 > kk1)
                        {
                            Global.mnFrm.cmCde.showMsg("Quantity in Row(" + (i + 1).ToString() +
                         ") cannot EXCEED Reserved Stock Quantity[" + kk1 + "] hence cannot be delivered!!", 0);
                            return false;
                        }
                        kk1 = Global.getCnsgmtsRsvdSum(cnsgmntIDs);
                        if (tst1 > kk1)
                        {
                            Global.mnFrm.cmCde.showMsg("Quantity in Row(" + (i + 1).ToString() +
                           ") cannot EXCEED Reserved Quantity[" + kk1 + "] in the Selected Consignments hence cannot be delivered["
                         + cnsgmntIDs + "]!", 0);
                            return false;
                        }
                    }
                }

                long prsn_id = -1;
                long.TryParse(this.itemsDataGridView.Rows[i].Cells[28].Value.ToString(), out prsn_id);
                if (this.allowDuesCheckBox.Checked)
                {
                    long pay_itm_id = -1;
                    long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
              "org.org_pay_items", "inv_item_id", "item_id", itmID), out pay_itm_id);
                    if (pay_itm_id > 0 && prsn_id <= 0)
                    {
                        Global.mnFrm.cmCde.showMsg("Row(" + (i + 1).ToString() +
") must have a linked Person!", 0);
                        return false;
                    }
                }
            }
            return true;
        }

        //    public bool nxtLvlValidateLns(string srcDocType)
        //    {
        //      for (int i = 0; i < this.itemsDataGridView.Rows.Count; i++)
        //      {
        //        if (this.itemsDataGridView.Rows[i].Cells[4].Value == null)
        //        {
        //          this.itemsDataGridView.Rows[i].Cells[4].Value = string.Empty;
        //        }
        //        if (this.itemsDataGridView.Rows[i].Cells[9].Value == null)
        //        {
        //          this.itemsDataGridView.Rows[i].Cells[9].Value = string.Empty;
        //        }
        //        if (this.itemsDataGridView.Rows[i].Cells[10].Value == null)
        //        {
        //          this.itemsDataGridView.Rows[i].Cells[10].Value = string.Empty;
        //        }
        //        if (this.itemsDataGridView.Rows[i].Cells[12].Value == null)
        //        {
        //          this.itemsDataGridView.Rows[i].Cells[12].Value = string.Empty;
        //        }
        //        if (this.itemsDataGridView.Rows[i].Cells[13].Value == null)
        //        {
        //          this.itemsDataGridView.Rows[i].Cells[13].Value = string.Empty;
        //        }
        //        if (this.itemsDataGridView.Rows[i].Cells[16].Value == null)
        //        {
        //          this.itemsDataGridView.Rows[i].Cells[16].Value = "-1";
        //        }
        //        double tst1 = 0;
        //        double tst2 = 0;
        //        double.TryParse(this.itemsDataGridView.Rows[i].Cells[4].Value.ToString(), out tst1);
        //        string dateStr = DateTime.ParseExact(Global.mnFrm.cmCde.getDB_Date_time(), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy HH:mm:ss");
        //        long itmID = -1;
        //        long storeID = -1;
        //        long.TryParse(this.itemsDataGridView.Rows[i].Cells[12].Value.ToString(), out itmID);
        //        long.TryParse(this.itemsDataGridView.Rows[i].Cells[13].Value.ToString(), out storeID);
        //        long stckID = Global.getItemStockID(itmID, storeID);
        //        string itmType = Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_itm_list", "item_id", "item_type", itmID);
        //        string cnsgmntIDs = this.itemsDataGridView.Rows[i].Cells[10].Value.ToString();
        //        if (this.docTypeComboBox.Text != "Sales Return" && itmType != "Services"
        //          && srcDocType != "Sales Order")
        //        {
        //          if (tst1 > Global.getStockLstAvlblBls(stckID, dateStr))
        //          {
        //            Global.mnFrm.cmCde.showMsg("Document Quantity in Row(" + (i + 1).ToString() +
        //         ") cannot EXCEED Available Stock Quantity!", 0);
        //            return false;
        //          }
        //          if (tst1 > Global.getCnsgmtsQtySum(cnsgmntIDs))
        //          {
        //            Global.mnFrm.cmCde.showMsg("Document Quantity in Row(" + (i + 1).ToString() +
        //           ") cannot EXCEED Available Quantity in the Selected Consignments!", 0);
        //            return false;
        //          }
        //        }
        //        else if (srcDocType == "Sales Order")
        //        {
        //          if (tst1 > Global.getStockLstRsvdBls(stckID, dateStr))
        //          {
        //            Global.mnFrm.cmCde.showMsg("Document Quantity in Row(" + (i + 1).ToString() +
        //         ") cannot EXCEED Reserved Stock Quantity!", 0);
        //            return false;
        //          }
        //          if (tst1 > Global.getCnsgmtsRsvdSum(cnsgmntIDs))
        //          {
        //            Global.mnFrm.cmCde.showMsg("Document Quantity in Row(" + (i + 1).ToString() +
        //           ") cannot EXCEED Available Quantity in the Selected Consignments!", 0);
        //            return false;
        //          }
        //        }
        //      }
        //      return true;
        //    }

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

        public bool sendToGLInterfaceMnl(int accntID,
          string incrsDcrs, double amount,
      string trns_date, string trns_desc,
      int crncy_id, string dateStr, string srcDocTyp,
          long srcDocID, long srcDocLnID)
        {
            try
            {
                double netamnt = 0;

                netamnt = Global.mnFrm.cmCde.dbtOrCrdtAccntMultiplier(
                  accntID,
                  incrsDcrs) * amount;

                long py_dbt_ln = -1;// Global.getIntFcTrnsDbtLn(srcDocLnID, srcDocTyp, amount, accntID, trns_desc);
                long py_crdt_ln = -1;// Global.getIntFcTrnsCrdtLn(srcDocLnID, srcDocTyp, amount, accntID, trns_desc);
                if (Global.mnFrm.cmCde.dbtOrCrdtAccnt(accntID,
                  incrsDcrs) == "Debit")
                {
                    if (py_dbt_ln <= 0)
                    {
                        Global.createPymntGLIntFcLn(accntID,
                          trns_desc,
                              amount, trns_date,
                              crncy_id, 0,
                              netamnt, srcDocTyp, srcDocID, srcDocLnID, dateStr);
                    }
                }
                else
                {
                    if (py_crdt_ln <= 0)
                    {
                        Global.createPymntGLIntFcLn(accntID,
                        trns_desc,
                  0, trns_date,
                  crncy_id, amount,
                  netamnt, srcDocTyp, srcDocID, srcDocLnID, dateStr);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Global.mnFrm.cmCde.showMsg("Error Sending Payment to GL Interface" +
                  " " + ex.Message, 0);
                return false;
            }
        }

        private bool generateItmAccntng(long itmID, double qnty, string cnsgmntIDs,
       int txCodeID, int dscntCodeID, int chrgCodeID,
       string docTyp, long docID, long srcDocID, int dfltRcvblAcntID,
       int dfltInvAcntID, int dfltCGSAcntID, int dfltExpnsAcntID, int dfltRvnuAcntID,
       long stckID, double unitSllgPrc, int crncyID, long docLnID,
       int dfltSRAcntID, int dfltCashAcntID, int dfltCheckAcntID, long srcDocLnID,
       string dateStr, string docIDNum, int entrdCurrID,
         decimal exchngRate, int dfltLbltyAccnt, string strSrcDocType,
         string cstmrNm, string docDesc, string itmDesc, int storeID, string itmType,
         double orgnlSllngPrce)
        {
            try
            {
                if (cstmrNm == "")
                {
                    cstmrNm = "Unspecified Customer";
                }
                if (docDesc == "")
                {
                    docDesc = "Unstated Purpose";
                }
                bool succs = true;
                /*For each Item in a Sales Invoice
                 * 1. Get Items Consgnmnt Cost Prices using all selected consignments and their used qtys
                 * 2. Decrease Inv Account by Cost Price --0Inventory
                 * 3. Increase Cost of Goods Sold by Cost Price --0Inventory
                 * 4. Get Selling Price, Taxes, Extra Charges, Discounts
                 * 5. Get Net Selling Price = (Selling Price - Taxes - Extra Charges + Discounts)*Qty
                 * 6. Increase Revenue Account by Net Selling Price --1Initial Amount
                 * 7. Increase Receivables account by Net Selling price --1Initial Amount
                 * 8. Increase Taxes Payable by Taxes  --2Tax
                 * 9. Increase Receivables account by Taxes --2Tax
                 * 10.Increase Extra Charges Revenue by Extra Charges --4Extra Charge
                 * 11.Increase Receivables account by Extra Charges --4Extra Charge
                 * 12.Increase Sales Discounts by Discounts --3Discount
                 * 13.Decrease Receivables by Discounts --3Discount
                 */
                int txPyblAcntID = -1;
                int chrgRvnuAcntID = -1;
                int salesDscntAcntID = -1;
                double funcCurrrate = Math.Round((double)1 / (double)exchngRate, 15);
                double ttlSllngPrc = Math.Round(qnty * unitSllgPrc, 2);
                //Get Net Selling Price = Selling Price - Taxes
                double ttlRvnuAmnt = ttlSllngPrc;
                //For Sales Invoice, Sales Return, Item Issues-Unbilled Docs get the ff
                if (dfltRcvblAcntID <= 0
            || dfltInvAcntID <= 0
            || dfltCGSAcntID <= 0
            || dfltExpnsAcntID <= 0
            || dfltRvnuAcntID <= 0)
                {
                    Global.mnFrm.cmCde.showMsg("You must first Setup all Default " +
                      "Accounts before Accounting can be Created!\r\n\r\n" +
                      dfltRcvblAcntID + "," + dfltInvAcntID + "," + dfltCGSAcntID + ","
                      + dfltExpnsAcntID + "," + dfltRvnuAcntID, 0);
                    return false;
                }

                //Global.mnFrm.cmCde.showMsg("Type:" + itmType, 0);
                if (itmType.Contains("Inventory")
                  || itmType.Contains("Fixed Assets"))
                {
                    List<string[]> csngmtData;
                    if (docTyp != "Sales Return")
                    {
                        csngmtData = Global.getItmCnsgmtVals(qnty, cnsgmntIDs);
                    }
                    else
                    {
                        csngmtData = Global.getSRItmCnsgmtVals(
                          docLnID, qnty, cnsgmntIDs, srcDocLnID);
                    }
                    //From the List get Total Cost Price of the Item

                    double ttlCstPrice = 0;
                    for (int i = 0; i < csngmtData.Count; i++)
                    {
                        string[] ary = csngmtData[i];
                        double fig1Qty = 0;
                        double fig2Prc = 0;
                        double.TryParse(ary[1], out fig1Qty);
                        double.TryParse(ary[2], out fig2Prc);
                        ttlCstPrice += fig1Qty * fig2Prc;
                    }
                    if (dfltInvAcntID > 0 && dfltCGSAcntID > 0 && docTyp == "Sales Invoice")
                    {
                        succs = this.sendToGLInterfaceMnl(
                          dfltInvAcntID, "D", ttlCstPrice, dateStr,
                           "Sale of " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")", crncyID, dateStr,
                           docTyp, docID, docLnID);
                        if (!succs)
                        {
                            return succs;
                        }
                        succs = this.sendToGLInterfaceMnl(dfltCGSAcntID, "I", ttlCstPrice, dateStr,
                            "Sale of " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")", crncyID, dateStr,
                            docTyp, docID, docLnID);
                        if (!succs)
                        {
                            return succs;
                        }
                    }
                    else if (dfltInvAcntID > 0 && dfltCGSAcntID > 0 && docTyp == "Sales Return" && strSrcDocType == "Sales Invoice")
                    {
                        succs = this.sendToGLInterfaceMnl(dfltInvAcntID, "I", ttlCstPrice, dateStr,
                          "Return of Sold " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")", crncyID, dateStr,
                          docTyp, docID, docLnID);
                        if (!succs)
                        {
                            return succs;
                        }
                        succs = this.sendToGLInterfaceMnl(dfltCGSAcntID, "D", ttlCstPrice, dateStr,
                          "Return of Sold " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")", crncyID, dateStr,
                          docTyp, docID, docLnID);
                        if (!succs)
                        {
                            return succs;
                        }
                    }
                    else if (docTyp == "Item Issue-Unbilled")
                    {
                        if (dfltInvAcntID > 0 && dfltExpnsAcntID > 0)
                        {
                            succs = this.sendToGLInterfaceMnl(dfltInvAcntID, "D", ttlCstPrice, dateStr,
                              "Issue Out of " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")", crncyID, dateStr,
                              docTyp, docID, docLnID);
                            if (!succs)
                            {
                                return succs;
                            }
                            succs = this.sendToGLInterfaceMnl(dfltExpnsAcntID, "I", ttlCstPrice, dateStr,
                              "Issue Out of " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")", crncyID, dateStr,
                              docTyp, docID, docLnID);
                            if (!succs)
                            {
                                return succs;
                            }
                        }
                    }
                    else if (docTyp == "Sales Return" && strSrcDocType == "Item Issue-Unbilled")
                    {
                        if (dfltInvAcntID > 0 && dfltExpnsAcntID > 0)
                        {
                            succs = this.sendToGLInterfaceMnl(dfltInvAcntID, "I", ttlCstPrice, dateStr,
                              "Return of " + itmDesc + " Issued Out to " + cstmrNm + " (" + docDesc + ")", crncyID, dateStr,
                              docTyp, docID, docLnID);
                            if (!succs)
                            {
                                return succs;
                            }
                            succs = this.sendToGLInterfaceMnl(dfltExpnsAcntID, "D", ttlCstPrice, dateStr,
                              "Return of " + itmDesc + " Issued Out to " + cstmrNm + " (" + docDesc + ")", crncyID, dateStr,
                              docTyp, docID, docLnID);
                            if (!succs)
                            {
                                return succs;
                            }
                        }
                    }
                }
                char[] w = { ',' };
                double snglDscnt = 0;
                string isParnt = "";
                int accntCurrID = this.curid;
                double accntCurrRate = funcCurrrate;

                if (docTyp == "Sales Invoice")
                {
                    snglDscnt = 0;
                    if (dscntCodeID > 0)
                    {
                        isParnt = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "is_parent", dscntCodeID);
                        if (isParnt == "1")
                        {
                            string[] codeIDs = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "child_code_ids", dscntCodeID).Split(w, StringSplitOptions.RemoveEmptyEntries);
                            snglDscnt = 0;
                            for (int j = 0; j < codeIDs.Length; j++)
                            {
                                if (int.Parse(codeIDs[j]) > 0)
                                {
                                    salesDscntAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "dscount_expns_accnt_id", int.Parse(codeIDs[j])));
                                    if (salesDscntAcntID > 0 && dfltRcvblAcntID > 0)
                                    {
                                        string dscntCodeNm = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "code_name", int.Parse(codeIDs[j]));
                                        double ttlDsctAmnt = Math.Round(Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), orgnlSllngPrce, qnty), 2);
                                        snglDscnt += Math.Round(Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), orgnlSllngPrce, 1), 2);

                                        Global.createScmRcvblsDocDet(docID, "3Discount",
                                  "Discounts (" + dscntCodeNm + ") on Sales Invoice (" + docIDNum + ") IRO " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")",
                                  ttlDsctAmnt, entrdCurrID, int.Parse(codeIDs[j]), docTyp, false, "Increase", salesDscntAcntID,
                                  "Decrease", dfltRcvblAcntID, -1, "VALID", -1, this.curid, accntCurrID,
                                  funcCurrrate, accntCurrRate, Math.Round(funcCurrrate * ttlDsctAmnt, 2),
                                  Math.Round(accntCurrRate * ttlDsctAmnt, 2));
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (dscntCodeID > 0)
                            {
                                salesDscntAcntID = -1;
                                int.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "dscount_expns_accnt_id", dscntCodeID), out salesDscntAcntID);
                                if (salesDscntAcntID > 0 && dfltRcvblAcntID > 0)
                                {
                                    string dscntCodeNm = Global.mnFrm.cmCde.getGnrlRecNm(
                             "scm.scm_tax_codes", "code_id", "code_name",
                             dscntCodeID);
                                    double ttlDsctAmnt = Math.Round(Global.getSalesDocCodesAmnt(
                                dscntCodeID, orgnlSllngPrce, qnty), 2);
                                    snglDscnt = Math.Round(Global.getSalesDocCodesAmnt(dscntCodeID, orgnlSllngPrce, 1), 2);

                                    Global.createScmRcvblsDocDet(docID, "3Discount",
                              "Discounts (" + dscntCodeNm + ") on Sales Invoice (" + docIDNum + ") IRO " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")",
                              ttlDsctAmnt, entrdCurrID, dscntCodeID, docTyp, false, "Increase", salesDscntAcntID,
                              "Decrease", dfltRcvblAcntID, -1, "VALID", -1, this.curid, accntCurrID,
                              funcCurrrate, accntCurrRate, Math.Round(funcCurrrate * ttlDsctAmnt, 2),
                              Math.Round(accntCurrRate * ttlDsctAmnt, 2));
                                }
                            }
                        }
                    }

                    if (txCodeID > 0)
                    {
                        isParnt = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "is_parent", txCodeID);
                        if (isParnt == "1")
                        {
                            string[] codeIDs = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "child_code_ids", txCodeID).Split(w, StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < codeIDs.Length; j++)
                            {
                                if (int.Parse(codeIDs[j]) > 0)
                                {
                                    double ttlTxAmnt = Math.Round(Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), orgnlSllngPrce - snglDscnt, qnty), 2);
                                    string txCodeNm = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "code_name", int.Parse(codeIDs[j]));
                                    txPyblAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "taxes_payables_accnt_id", int.Parse(codeIDs[j])));
                                    if (txPyblAcntID > 0 && dfltRcvblAcntID > 0)
                                    {
                                        Global.createScmRcvblsDocDet(docID, "2Tax",
                                        "Taxes (" + txCodeNm + ") on Sales Invoice (" + docIDNum + ") IRO " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")",
                                        ttlTxAmnt, entrdCurrID, int.Parse(codeIDs[j]), docTyp, false, "Increase", txPyblAcntID,
                                        "Increase", dfltRcvblAcntID, -1, "VALID", -1, this.curid, accntCurrID,
                                        funcCurrrate, accntCurrRate, Math.Round(funcCurrrate * ttlTxAmnt, 2),
                                        Math.Round(accntCurrRate * ttlTxAmnt, 2));
                                        ttlRvnuAmnt -= ttlTxAmnt;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (txCodeID > 0)
                            {
                                txPyblAcntID = -1;
                                int.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "taxes_payables_accnt_id", txCodeID), out txPyblAcntID);
                                if (txPyblAcntID > 0 && dfltRcvblAcntID > 0)
                                {
                                    double ttlTxAmnt = Math.Round(Global.getSalesDocCodesAmnt(txCodeID, orgnlSllngPrce - snglDscnt, qnty), 2);
                                    string txCodeNm = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "code_name", txCodeID);
                                    Global.createScmRcvblsDocDet(docID, "2Tax",
                            "Taxes (" + txCodeNm + ") on Sales Invoice (" + docIDNum + ") IRO " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")",
                            ttlTxAmnt, entrdCurrID, txCodeID, docTyp, false, "Increase", txPyblAcntID,
                            "Increase", dfltRcvblAcntID, -1, "VALID", -1, this.curid, accntCurrID,
                            funcCurrrate, accntCurrRate, Math.Round(funcCurrrate * ttlTxAmnt, 2),
                            Math.Round(accntCurrRate * ttlTxAmnt, 2));
                                    ttlRvnuAmnt -= ttlTxAmnt;
                                }
                            }
                        }
                    }

                    if (chrgCodeID > 0)
                    {
                        isParnt = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "is_parent", chrgCodeID);
                        if (isParnt == "1")
                        {
                            string[] codeIDs = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id",
                              "child_code_ids", chrgCodeID).Split(w, StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < codeIDs.Length; j++)
                            {
                                if (int.Parse(codeIDs[j]) > 0)
                                {
                                    double ttlChrgAmnt = Math.Round(Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), orgnlSllngPrce, qnty), 2);
                                    string chrgCodeNm = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "code_name", int.Parse(codeIDs[j]));
                                    chrgRvnuAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "chrge_revnu_accnt_id", int.Parse(codeIDs[j])));

                                    if (chrgRvnuAcntID > 0 && dfltRcvblAcntID > 0)
                                    {
                                        Global.createScmRcvblsDocDet(docID, "4Extra Charge",
                                  "Extra Charges (" + chrgCodeNm + ") on Sales Invoice (" + docIDNum + ") IRO " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")",
                                  ttlChrgAmnt, entrdCurrID, int.Parse(codeIDs[j]), docTyp, false, "Increase", chrgRvnuAcntID,
                                  "Increase", dfltRcvblAcntID, -1, "VALID", -1, this.curid, accntCurrID,
                                  funcCurrrate, accntCurrRate, Math.Round(funcCurrrate * ttlChrgAmnt, 2),
                                  Math.Round(accntCurrRate * ttlChrgAmnt, 2));
                                    }
                                }
                            }
                        }
                        else
                        {
                            chrgRvnuAcntID = -1;
                            int.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "chrge_revnu_accnt_id", chrgCodeID), out chrgRvnuAcntID);
                            if (chrgRvnuAcntID > 0 && dfltRcvblAcntID > 0)
                            {
                                double ttlChrgAmnt = Math.Round(Global.getSalesDocCodesAmnt(chrgCodeID, orgnlSllngPrce, qnty), 2);
                                string chrgCodeNm = Global.mnFrm.cmCde.getGnrlRecNm(
                            "scm.scm_tax_codes", "code_id", "code_name",
                            chrgCodeID);

                                Global.createScmRcvblsDocDet(docID, "4Extra Charge",
                          "Extra Charges (" + chrgCodeNm + ") on Sales Invoice (" + docIDNum + ") IRO " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")",
                          ttlChrgAmnt, entrdCurrID, chrgCodeID, docTyp, false, "Increase", chrgRvnuAcntID,
                          "Increase", dfltRcvblAcntID, -1, "VALID", -1, this.curid, accntCurrID,
                          funcCurrrate, accntCurrRate, Math.Round(funcCurrrate * ttlChrgAmnt, 2),
                          Math.Round(accntCurrRate * ttlChrgAmnt, 2));
                            }
                        }
                    }

                    if (dfltRvnuAcntID > 0 && dfltRcvblAcntID > 0)
                    {
                        Global.createScmRcvblsDocDet(docID, "1Initial Amount",
                  "Revenue from Sales Invoice (" + docIDNum + ") IRO " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")",
                  ttlRvnuAmnt, entrdCurrID, -1, docTyp, false, "Increase", dfltRvnuAcntID,
                  "Increase", dfltRcvblAcntID, -1, "VALID", -1, this.curid, accntCurrID,
                  funcCurrrate, accntCurrRate, Math.Round(funcCurrrate * ttlRvnuAmnt, 2),
                  Math.Round(accntCurrRate * ttlRvnuAmnt, 2));
                    }
                }
                else if (docTyp == "Sales Return" && strSrcDocType == "Sales Invoice")
                {
                    if (dscntCodeID > 0)
                    {
                        isParnt = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "is_parent", dscntCodeID);
                        if (isParnt == "1")
                        {
                            string[] codeIDs = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "child_code_ids", dscntCodeID).Split(w, StringSplitOptions.RemoveEmptyEntries);
                            snglDscnt = 0;
                            for (int j = 0; j < codeIDs.Length; j++)
                            {
                                if (int.Parse(codeIDs[j]) > 0)
                                {
                                    salesDscntAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "dscount_expns_accnt_id", int.Parse(codeIDs[j])));
                                    if (salesDscntAcntID > 0 && dfltLbltyAccnt > 0)
                                    {
                                        string dscntCodeNm = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "code_name", int.Parse(codeIDs[j]));
                                        double ttlDsctAmnt = Math.Round(Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), orgnlSllngPrce, qnty), 2);
                                        snglDscnt += Math.Round(Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), orgnlSllngPrce, 1), 2);

                                        Global.createScmRcvblsDocDet(docID, "3Discount",
                          "Take Back Discounts (" + dscntCodeNm + ") on Sales Return (" + docIDNum + ") IRO " + itmDesc + " by " + cstmrNm + " (" + docDesc + ")",
                          ttlDsctAmnt, entrdCurrID, int.Parse(codeIDs[j]), docTyp, false, "Decrease", salesDscntAcntID,
                          "Decrease", dfltLbltyAccnt, -1, "VALID", -1, this.curid, accntCurrID,
                          funcCurrrate, accntCurrRate, Math.Round(funcCurrrate * ttlDsctAmnt, 2),
                          Math.Round(accntCurrRate * ttlDsctAmnt, 2));
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (dscntCodeID > 0)
                            {
                                salesDscntAcntID = -1;
                                int.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "dscount_expns_accnt_id", dscntCodeID), out salesDscntAcntID);
                                if (salesDscntAcntID > 0 && dfltLbltyAccnt > 0)
                                {
                                    string dscntCodeNm = Global.mnFrm.cmCde.getGnrlRecNm(
                             "scm.scm_tax_codes", "code_id", "code_name",
                             dscntCodeID);
                                    double ttlDsctAmnt = Math.Round(Global.getSalesDocCodesAmnt(
                                dscntCodeID, orgnlSllngPrce, qnty), 2);
                                    snglDscnt = Math.Round(Global.getSalesDocCodesAmnt(dscntCodeID, orgnlSllngPrce, 1), 2);

                                    Global.createScmRcvblsDocDet(docID, "3Discount",
                          "Take Back Discounts (" + dscntCodeNm + ") on Sales Return (" + docIDNum + ") IRO " + itmDesc + " by " + cstmrNm + " (" + docDesc + ")",
                          ttlDsctAmnt, entrdCurrID, dscntCodeID, docTyp, false, "Decrease", salesDscntAcntID,
                          "Decrease", dfltLbltyAccnt, -1, "VALID", -1, this.curid, accntCurrID,
                          funcCurrrate, accntCurrRate, Math.Round(funcCurrrate * ttlDsctAmnt, 2),
                          Math.Round(accntCurrRate * ttlDsctAmnt, 2));
                                }
                            }
                        }
                    }

                    if (txCodeID > 0)
                    {
                        isParnt = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "is_parent", txCodeID);
                        if (isParnt == "1")
                        {
                            string[] codeIDs = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "child_code_ids", txCodeID).Split(w, StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < codeIDs.Length; j++)
                            {
                                if (int.Parse(codeIDs[j]) > 0)
                                {
                                    double ttlTxAmnt = Math.Round(Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), orgnlSllngPrce - snglDscnt, qnty), 2);
                                    string txCodeNm = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "code_name", int.Parse(codeIDs[j]));
                                    txPyblAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "taxes_payables_accnt_id", int.Parse(codeIDs[j])));
                                    if (txPyblAcntID > 0 && dfltLbltyAccnt > 0)
                                    {
                                        Global.createScmRcvblsDocDet(docID, "2Tax",
                          "Refund Taxes (" + txCodeNm + ") on Sales Return (" + docIDNum + ") IRO " + itmDesc + " by " + cstmrNm + " (" + docDesc + ")",
                          ttlTxAmnt, entrdCurrID, int.Parse(codeIDs[j]), docTyp, false, "Decrease", txPyblAcntID,
                          "Increase", dfltLbltyAccnt, -1, "VALID", -1, this.curid, accntCurrID,
                          funcCurrrate, accntCurrRate, Math.Round(funcCurrrate * ttlTxAmnt, 2),
                          Math.Round(accntCurrRate * ttlTxAmnt, 2));
                                        ttlRvnuAmnt -= ttlTxAmnt;
                                    }
                                }
                            }
                        }
                        else
                        {
                            txPyblAcntID = -1;
                            int.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "taxes_payables_accnt_id", txCodeID), out txPyblAcntID);
                            if (txPyblAcntID > 0 && dfltLbltyAccnt > 0)
                            {
                                double ttlTxAmnt = Math.Round(Global.getSalesDocCodesAmnt(txCodeID, orgnlSllngPrce - snglDscnt, qnty), 2);
                                string txCodeNm = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "code_name", txCodeID);
                                Global.createScmRcvblsDocDet(docID, "2Tax",
                      "Refund Taxes (" + txCodeNm + ") on Sales Return (" + docIDNum + ") IRO " + itmDesc + " by " + cstmrNm + " (" + docDesc + ")",
                      ttlTxAmnt, entrdCurrID, txCodeID, docTyp, false, "Decrease", txPyblAcntID,
                      "Increase", dfltLbltyAccnt, -1, "VALID", -1, this.curid, accntCurrID,
                      funcCurrrate, accntCurrRate, Math.Round(funcCurrrate * ttlTxAmnt, 2),
                      Math.Round(accntCurrRate * ttlTxAmnt, 2));
                                ttlRvnuAmnt -= ttlTxAmnt;
                            }
                        }
                    }

                    if (chrgCodeID > 0)
                    {
                        isParnt = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "is_parent", chrgCodeID);
                        if (isParnt == "1")
                        {
                            string[] codeIDs = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id",
                              "child_code_ids", chrgCodeID).Split(w, StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < codeIDs.Length; j++)
                            {
                                if (int.Parse(codeIDs[j]) > 0)
                                {
                                    double ttlChrgAmnt = Math.Round(Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), orgnlSllngPrce, qnty), 2);
                                    string chrgCodeNm = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "code_name", int.Parse(codeIDs[j]));
                                    chrgRvnuAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "chrge_revnu_accnt_id", int.Parse(codeIDs[j])));

                                    if (chrgRvnuAcntID > 0 && dfltLbltyAccnt > 0)
                                    {
                                        Global.createScmRcvblsDocDet(docID, "4Extra Charge",
                          "Refund Extra Charges (" + chrgCodeNm + ") on Sales Return (" + docIDNum + ") IRO " + itmDesc + " by " + cstmrNm + " (" + docDesc + ")",
                          ttlChrgAmnt, entrdCurrID, int.Parse(codeIDs[j]), docTyp, false, "Decrease", chrgRvnuAcntID,
                          "Increase", dfltLbltyAccnt, -1, "VALID", -1, this.curid, accntCurrID,
                          funcCurrrate, accntCurrRate, Math.Round(funcCurrrate * ttlChrgAmnt, 2),
                          Math.Round(accntCurrRate * ttlChrgAmnt, 2));
                                    }
                                }
                            }
                        }
                        else
                        {
                            chrgRvnuAcntID = -1;
                            int.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "chrge_revnu_accnt_id", chrgCodeID), out chrgRvnuAcntID);
                            if (chrgRvnuAcntID > 0 && dfltLbltyAccnt > 0)
                            {
                                double ttlChrgAmnt = Math.Round(Global.getSalesDocCodesAmnt(chrgCodeID, orgnlSllngPrce, qnty), 2);
                                string chrgCodeNm = Global.mnFrm.cmCde.getGnrlRecNm(
                            "scm.scm_tax_codes", "code_id", "code_name",
                            chrgCodeID);

                                Global.createScmRcvblsDocDet(docID, "4Extra Charge",
                      "Refund Extra Charges (" + chrgCodeNm + ") on Sales Return (" + docIDNum + ") IRO " + itmDesc + " by " + cstmrNm + " (" + docDesc + ")",
                      ttlChrgAmnt, entrdCurrID, chrgCodeID, docTyp, false, "Decrease", chrgRvnuAcntID,
                      "Increase", dfltLbltyAccnt, -1, "VALID", -1, this.curid, accntCurrID,
                      funcCurrrate, accntCurrRate, Math.Round(funcCurrrate * ttlChrgAmnt, 2),
                      Math.Round(accntCurrRate * ttlChrgAmnt, 2));
                            }
                        }
                    }
                    if (dfltRvnuAcntID > 0 && dfltLbltyAccnt > 0)
                    {
                        Global.createScmRcvblsDocDet(docID, "1Initial Amount",
                  "Refund from Sales Return (" + docIDNum + ") IRO " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")",
                  ttlRvnuAmnt, entrdCurrID, -1, docTyp, false, "Decrease", dfltRvnuAcntID,
                  "Increase", dfltLbltyAccnt, -1, "VALID", -1, this.curid, accntCurrID,
                  funcCurrrate, accntCurrRate, Math.Round(funcCurrrate * ttlRvnuAmnt, 2),
                  Math.Round(accntCurrRate * ttlRvnuAmnt, 2));
                    }
                }
                return succs;
            }
            catch (Exception ex)
            {
                Global.mnFrm.cmCde.showMsg(ex.InnerException + "\r\n" + ex.StackTrace + "\r\n" + ex.Message, 0);
                return false;
            }
        }

        /* private bool generateItmAccntng(long itmID, double qnty, string cnsgmntIDs,
        int txCodeID, int dscntCodeID, int chrgCodeID,
        string docTyp, long docID, long srcDocID, int dfltRcvblAcntID,
        int dfltInvAcntID, int dfltCGSAcntID, int dfltExpnsAcntID, int dfltRvnuAcntID,
        long stckID, double unitSllgPrc, int crncyID, long docLnID,
        int dfltSRAcntID, int dfltCashAcntID, int dfltCheckAcntID, long srcDocLnID,
        string dateStr, string docIDNum, int entrdCurrID,
          decimal exchngRate, int dfltLbltyAccnt, string strSrcDocType,
          string cstmrNm, string docDesc, string itmDesc, int storeID)
         {
             try
             {
                 if (cstmrNm == "")
                 {
                     cstmrNm = "Unspecified Customer";
                 }

                 if (docDesc == "")
                 {
                     docDesc = "Unstated Purpose";
                 }

                 bool succs = true;
                 int itmInvAcntID = -1;
                 int itmCGSAcntID = -1;
                 //For Sales Return, Item Issues-Unbilled Docs get the ff
                 int itmExpnsAcntID = -1;
                 //For Sales Invoice, Sales Return get the ff
                 int itmRvnuAcntID = -1;
                 //Genral
                 int txPyblAcntID = -1;
                 int chrgRvnuAcntID = -1;
                 int salesDscntAcntID = -1;

                 int.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_itm_subinventories", "subinv_id", "inv_asset_acct_id", storeID), out itmInvAcntID);
                 //int.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_itm_list", "item_id", "inv_asset_acct_id", itmID), out itmInvAcntID);

                 int.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_itm_list", "item_id", "cogs_acct_id", itmID), out itmCGSAcntID);
                 //For Sales Return, Item Issues-Unbilled Docs get the ff
                 int.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_itm_list", "item_id", "expense_accnt_id", itmID), out itmExpnsAcntID);
                 //For Sales Invoice, Sales Return get the ff
                 int.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_itm_list", "item_id", "sales_rev_accnt_id", itmID), out itmRvnuAcntID);
                 //Genral
                 //int.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "taxes_payables_accnt_id", txCodeID), out txPyblAcntID);
                 //int.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "chrge_revnu_accnt_id", chrgCodeID), out chrgRvnuAcntID);
                 //int.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "dscount_expns_accnt_id", dscntCodeID), out salesDscntAcntID);
                 if (itmInvAcntID > 0)
                 {
                     dfltInvAcntID = itmInvAcntID;
                 }
                 if (itmCGSAcntID > 0)
                 {
                     dfltCGSAcntID = itmCGSAcntID;
                 }
                 if (itmExpnsAcntID > 0)
                 {
                     dfltExpnsAcntID = itmExpnsAcntID;
                 }
                 if (itmRvnuAcntID > 0)
                 {
                     dfltRvnuAcntID = itmRvnuAcntID;
                 }

                 if (dfltRcvblAcntID <= 0
             || dfltInvAcntID <= 0
             || dfltCGSAcntID <= 0
             || dfltExpnsAcntID <= 0
             || dfltRvnuAcntID <= 0)
                 {
                     Global.mnFrm.cmCde.showMsg("You must first Setup all Default " +
                       "Accounts before Accounting can be Created!", 0);
                     return false;
                 }

                 string itmType = Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_itm_list", "item_id", "item_type", itmID);
                 //        string dateStr = DateTime.ParseExact(
                 //Global.mnFrm.cmCde.getDB_Date_time(), "yyyy-MM-dd HH:mm:ss",
                 //System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy HH:mm:ss");
                 //     long SIDocID = -1;
                 //     long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr",
                 //"invc_hdr_id", "src_doc_hdr_id", docID),out SIDocID);
                 //Create a List of Consignment IDs, Quantity Used in this doc, Cost Price
                 //Get ttlSllngPrc, ttlTxAmnt, ttlChrgAmnt, ttlDsctAmnt for this item only

                 double funcCurrrate = Math.Round((double)1 / (double)exchngRate, 15);

                 double orgnlSllngPrce = double.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
                   "scm.scm_sales_invc_det", "invc_det_ln_id", "orgnl_selling_price", docLnID));
                 double sllngPrce = double.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
                   "scm.scm_sales_invc_det", "invc_det_ln_id", "unit_selling_price", docLnID));
                 double ttlSllngPrc = (qnty * sllngPrce);


                 //Get Net Selling Price = Selling Price - Taxes
                 double ttlRvnuAmnt = ttlSllngPrc;

                 if (itmType.Contains("Inventory")
                   || itmType.Contains("Fixed Assets"))
                 {
                     List<string[]> csngmtData;
                     if (docTyp != "Sales Return")
                     {
                         csngmtData = Global.getItmCnsgmtVals(qnty, cnsgmntIDs);
                     }
                     else
                     {
                         csngmtData = Global.getSRItmCnsgmtVals(
                           docLnID, qnty, cnsgmntIDs, srcDocLnID);
                     }
                     //From the List get Total Cost Price of the Item

                     double ttlCstPrice = 0;
                     for (int i = 0; i < csngmtData.Count; i++)
                     {
                         string[] ary = csngmtData[i];
                         double fig1Qty = 0;
                         double fig2Prc = 0;
                         double.TryParse(ary[1], out fig1Qty);
                         double.TryParse(ary[2], out fig2Prc);
                         ttlCstPrice += fig1Qty * fig2Prc;
                     }
                     if (dfltInvAcntID > 0 && dfltCGSAcntID > 0 && docTyp == "Sales Invoice")
                     {
                         succs = this.sendToGLInterfaceMnl(
                           dfltInvAcntID, "D", ttlCstPrice, dateStr,
                            "Sale of " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")", crncyID, dateStr,
                            docTyp, docID, docLnID);
                         if (!succs)
                         {
                             return succs;
                         }
                         succs = this.sendToGLInterfaceMnl(dfltCGSAcntID, "I", ttlCstPrice, dateStr,
                             "Sale of " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")", crncyID, dateStr,
                             docTyp, docID, docLnID);
                         if (!succs)
                         {
                             return succs;
                         }
                     }
                     else if (dfltInvAcntID > 0 && dfltCGSAcntID > 0 && docTyp == "Sales Return" && strSrcDocType == "Sales Invoice")
                     {
                         succs = this.sendToGLInterfaceMnl(dfltInvAcntID, "I", ttlCstPrice, dateStr,
                           "Return of Sold " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")", crncyID, dateStr,
                           docTyp, docID, docLnID);
                         if (!succs)
                         {
                             return succs;
                         }
                         succs = this.sendToGLInterfaceMnl(dfltCGSAcntID, "D", ttlCstPrice, dateStr,
                           "Return of Sold " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")", crncyID, dateStr,
                           docTyp, docID, docLnID);
                         if (!succs)
                         {
                             return succs;
                         }
                     }
                     else if (docTyp == "Item Issue-Unbilled")
                     {
                         if (dfltInvAcntID > 0 && dfltExpnsAcntID > 0)
                         {
                             succs = this.sendToGLInterfaceMnl(dfltInvAcntID, "D", ttlCstPrice, dateStr,
                               "Issue Out of " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")", crncyID, dateStr,
                               docTyp, docID, docLnID);
                             if (!succs)
                             {
                                 return succs;
                             }
                             succs = this.sendToGLInterfaceMnl(dfltExpnsAcntID, "I", ttlCstPrice, dateStr,
                               "Issue Out of " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")", crncyID, dateStr,
                               docTyp, docID, docLnID);
                             if (!succs)
                             {
                                 return succs;
                             }
                         }
                     }
                     else if (docTyp == "Sales Return" && strSrcDocType == "Item Issue-Unbilled")
                     {
                         if (dfltInvAcntID > 0 && dfltExpnsAcntID > 0)
                         {
                             succs = this.sendToGLInterfaceMnl(dfltInvAcntID, "I", ttlCstPrice, dateStr,
                               "Return of " + itmDesc + " Issued Out to " + cstmrNm + " (" + docDesc + ")", crncyID, dateStr,
                               docTyp, docID, docLnID);
                             if (!succs)
                             {
                                 return succs;
                             }
                             succs = this.sendToGLInterfaceMnl(dfltExpnsAcntID, "D", ttlCstPrice, dateStr,
                               "Return of " + itmDesc + " Issued Out to " + cstmrNm + " (" + docDesc + ")", crncyID, dateStr,
                               docTyp, docID, docLnID);
                             if (!succs)
                             {
                                 return succs;
                             }
                         }
                     }
                 }

                 char[] w = { ',' };
                 double snglDscnt = 0;
                 double initialDscnt = 0;
                 double ttlDscntTax = 0;
                 string isParnt = "";
                 int accntCurrID = this.curid;
                 double accntCurrRate = funcCurrrate;

                 if (docTyp == "Sales Invoice")
                 {
                     snglDscnt = 0;
                     if (dscntCodeID > 0)
                     {
                         isParnt = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "is_parent", dscntCodeID);
                         if (isParnt == "1")
                         {
                             string[] codeIDs = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "child_code_ids", dscntCodeID).Split(w, StringSplitOptions.RemoveEmptyEntries);
                             snglDscnt = 0;
                             for (int j = 0; j < codeIDs.Length; j++)
                             {
                                 if (int.Parse(codeIDs[j]) > 0)
                                 {
                                     salesDscntAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "dscount_expns_accnt_id", int.Parse(codeIDs[j])));
                                     if (salesDscntAcntID > 0 && dfltRcvblAcntID > 0)
                                     {
                                         string dscntCodeNm = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "code_name", int.Parse(codeIDs[j]));
                                         initialDscnt = Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), orgnlSllngPrce, qnty);
                                         double ttlDsctAmnt = this.getDscntLessTax(txCodeID, initialDscnt);
                                         snglDscnt += this.getDscntLessTax(txCodeID, Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), orgnlSllngPrce, 1));
                                         ttlDscntTax = initialDscnt - ttlDsctAmnt;

                                         Global.createScmRcvblsDocDet(docID, "3Discount",
                                           "Discounts (" + dscntCodeNm + ") on Sales Invoice (" + docIDNum + ") IRO " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")",
                                           ttlDsctAmnt, entrdCurrID, int.Parse(codeIDs[j]), docTyp, false, "Increase", salesDscntAcntID,
                                           "Decrease", dfltRcvblAcntID, -1, "VALID", -1, this.curid, accntCurrID,
                                           funcCurrrate, accntCurrRate, funcCurrrate * ttlDsctAmnt,
                                           accntCurrRate * ttlDsctAmnt);
                                         ttlRvnuAmnt -= ttlDscntTax;
                                     }
                                 }
                             }
                         }
                         else
                         {
                             salesDscntAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "dscount_expns_accnt_id", dscntCodeID));
                             if (salesDscntAcntID > 0 && dfltRcvblAcntID > 0)
                             {
                                 string dscntCodeNm = Global.mnFrm.cmCde.getGnrlRecNm(
                          "scm.scm_tax_codes", "code_id", "code_name",
                          dscntCodeID);
                                 initialDscnt = Global.getSalesDocCodesAmnt(dscntCodeID, orgnlSllngPrce, qnty);
                                 double ttlDsctAmnt = this.getDscntLessTax(txCodeID, initialDscnt);

                                 snglDscnt = this.getDscntLessTax(txCodeID, Global.getSalesDocCodesAmnt(dscntCodeID, orgnlSllngPrce, 1));
                                 ttlDscntTax = initialDscnt - ttlDsctAmnt;

                                 Global.createScmRcvblsDocDet(docID, "3Discount",
                           "Discounts (" + dscntCodeNm + ") on Sales Invoice (" + docIDNum + ") IRO " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")",
                           ttlDsctAmnt, entrdCurrID, dscntCodeID, docTyp, false, "Increase", salesDscntAcntID,
                           "Decrease", dfltRcvblAcntID, -1, "VALID", -1, this.curid, accntCurrID,
                           funcCurrrate, accntCurrRate, funcCurrrate * ttlDsctAmnt,
                           accntCurrRate * ttlDsctAmnt);
                                 ttlRvnuAmnt -= ttlDscntTax;
                             }
                         }
                     }

                     if (txCodeID > 0)
                     {
                         isParnt = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "is_parent", txCodeID);
                         if (isParnt == "1")
                         {
                             string[] codeIDs = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "child_code_ids", txCodeID).Split(w, StringSplitOptions.RemoveEmptyEntries);
                             for (int j = 0; j < codeIDs.Length; j++)
                             {
                                 if (int.Parse(codeIDs[j]) > 0)
                                 {
                                     double ttlTxAmnt = Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), orgnlSllngPrce - snglDscnt, qnty);
                                     string txCodeNm = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "code_name", int.Parse(codeIDs[j]));
                                     txPyblAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "taxes_payables_accnt_id", int.Parse(codeIDs[j])));
                                     if (txPyblAcntID > 0 && dfltRcvblAcntID > 0)
                                     {
                                         Global.createScmRcvblsDocDet(docID, "2Tax",
                                         "Taxes (" + txCodeNm + ") on Sales Invoice (" + docIDNum + ") IRO " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")",
                                         ttlTxAmnt, entrdCurrID, int.Parse(codeIDs[j]), docTyp, false, "Increase", txPyblAcntID,
                                         "Increase", dfltRcvblAcntID, -1, "VALID", -1, this.curid, accntCurrID,
                                         funcCurrrate, accntCurrRate, funcCurrrate * ttlTxAmnt,
                                         accntCurrRate * ttlTxAmnt);
                                         ttlRvnuAmnt -= ttlTxAmnt;
                                     }
                                 }
                             }
                         }
                         else
                         {
                             txPyblAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "taxes_payables_accnt_id", txCodeID));
                             if (txPyblAcntID > 0 && dfltRcvblAcntID > 0)
                             {
                                 double ttlTxAmnt = Global.getSalesDocCodesAmnt(txCodeID, orgnlSllngPrce - snglDscnt, qnty);
                                 string txCodeNm = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "code_name", txCodeID);
                                 Global.createScmRcvblsDocDet(docID, "2Tax",
                         "Taxes (" + txCodeNm + ") on Sales Invoice (" + docIDNum + ") IRO " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")",
                         ttlTxAmnt, entrdCurrID, txCodeID, docTyp, false, "Increase", txPyblAcntID,
                         "Increase", dfltRcvblAcntID, -1, "VALID", -1, this.curid, accntCurrID,
                         funcCurrrate, accntCurrRate, funcCurrrate * ttlTxAmnt,
                        accntCurrRate * ttlTxAmnt);
                                 ttlRvnuAmnt -= ttlTxAmnt;
                             }
                         }
                     }

                     if (chrgCodeID > 0)
                     {
                         isParnt = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "is_parent", chrgCodeID);
                         if (isParnt == "1")
                         {
                             string[] codeIDs = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id",
                               "child_code_ids", chrgCodeID).Split(w, StringSplitOptions.RemoveEmptyEntries);
                             for (int j = 0; j < codeIDs.Length; j++)
                             {
                                 if (int.Parse(codeIDs[j]) > 0)
                                 {
                                     double ttlChrgAmnt = Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), orgnlSllngPrce, qnty);
                                     string chrgCodeNm = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "code_name", int.Parse(codeIDs[j]));
                                     chrgRvnuAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "chrge_revnu_accnt_id", int.Parse(codeIDs[j])));

                                     if (chrgRvnuAcntID > 0 && dfltRcvblAcntID > 0)
                                     {
                                         Global.createScmRcvblsDocDet(docID, "4Extra Charge",
                                   "Extra Charges (" + chrgCodeNm + ") on Sales Invoice (" + docIDNum + ") IRO " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")",
                                   ttlChrgAmnt, entrdCurrID, int.Parse(codeIDs[j]), docTyp, false, "Increase", chrgRvnuAcntID,
                                   "Increase", dfltRcvblAcntID, -1, "VALID", -1, this.curid, accntCurrID,
                                   funcCurrrate, accntCurrRate, funcCurrrate * ttlChrgAmnt,
                                   accntCurrRate * ttlChrgAmnt);
                                     }
                                 }
                             }
                         }
                         else
                         {
                             chrgRvnuAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "chrge_revnu_accnt_id", chrgCodeID));
                             if (chrgRvnuAcntID > 0 && dfltRcvblAcntID > 0)
                             {
                                 double ttlChrgAmnt = Global.getSalesDocCodesAmnt(chrgCodeID, orgnlSllngPrce, qnty);
                                 string chrgCodeNm = Global.mnFrm.cmCde.getGnrlRecNm(
                             "scm.scm_tax_codes", "code_id", "code_name",
                             chrgCodeID);

                                 Global.createScmRcvblsDocDet(docID, "4Extra Charge",
                           "Extra Charges (" + chrgCodeNm + ") on Sales Invoice (" + docIDNum + ") IRO " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")",
                           ttlChrgAmnt, entrdCurrID, chrgCodeID, docTyp, false, "Increase", chrgRvnuAcntID,
                           "Increase", dfltRcvblAcntID, -1, "VALID", -1, this.curid, accntCurrID,
                           funcCurrrate, accntCurrRate, funcCurrrate * ttlChrgAmnt,
                          accntCurrRate * ttlChrgAmnt);
                             }
                         }
                     }
                     if (dfltRvnuAcntID > 0 && dfltRcvblAcntID > 0)
                     {
                         Global.createScmRcvblsDocDet(docID, "1Initial Amount",
                   "Revenue from Sales Invoice (" + docIDNum + ") IRO " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")",
                   ttlRvnuAmnt, entrdCurrID, -1, docTyp, false, "Increase", dfltRvnuAcntID,
                   "Increase", dfltRcvblAcntID, -1, "VALID", -1, this.curid, accntCurrID,
                   funcCurrrate, accntCurrRate, funcCurrrate * ttlRvnuAmnt,
                   accntCurrRate * ttlRvnuAmnt);
                     }
                 }
                 else if (docTyp == "Sales Return" && strSrcDocType == "Sales Invoice")
                 {
                     if (dscntCodeID > 0)
                     {
                         isParnt = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "is_parent", dscntCodeID);
                         if (isParnt == "1")
                         {
                             string[] codeIDs = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "child_code_ids", dscntCodeID).Split(w, StringSplitOptions.RemoveEmptyEntries);
                             snglDscnt = 0;
                             for (int j = 0; j < codeIDs.Length; j++)
                             {
                                 if (int.Parse(codeIDs[j]) > 0)
                                 {
                                     salesDscntAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "dscount_expns_accnt_id", int.Parse(codeIDs[j])));
                                     if (salesDscntAcntID > 0 && dfltLbltyAccnt > 0)
                                     {
                                         string dscntCodeNm = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "code_name", int.Parse(codeIDs[j]));
                                         initialDscnt = Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), orgnlSllngPrce, qnty);
                                         double ttlDsctAmnt = this.getDscntLessTax(txCodeID, initialDscnt);
                                         snglDscnt += this.getDscntLessTax(txCodeID, Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), orgnlSllngPrce, 1));
                                         ttlDscntTax = initialDscnt - ttlDsctAmnt;

                                         Global.createScmRcvblsDocDet(docID, "3Discount",
                                       "Take Back Discounts (" + dscntCodeNm + ") on Sales Return (" + docIDNum + ") IRO " + itmDesc +
                                       " by " + cstmrNm + " (" + docDesc + ")",
                                       ttlDsctAmnt, entrdCurrID, int.Parse(codeIDs[j]), docTyp, false, "Decrease", salesDscntAcntID,
                                       "Decrease", dfltLbltyAccnt, -1, "VALID", -1, this.curid, accntCurrID,
                                       funcCurrrate, accntCurrRate, (funcCurrrate * ttlDsctAmnt),
                                       (accntCurrRate * ttlDsctAmnt));
                                         ttlRvnuAmnt -= ttlDscntTax;
                                     }
                                 }
                             }
                         }
                         else
                         {
                             salesDscntAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "dscount_expns_accnt_id", dscntCodeID));
                             if (salesDscntAcntID > 0 && dfltLbltyAccnt > 0)
                             {
                                 string dscntCodeNm = Global.mnFrm.cmCde.getGnrlRecNm(
                          "scm.scm_tax_codes", "code_id", "code_name",
                          dscntCodeID);
                                 initialDscnt = Global.getSalesDocCodesAmnt(dscntCodeID, orgnlSllngPrce, qnty);
                                 double ttlDsctAmnt = this.getDscntLessTax(txCodeID, initialDscnt);
                                 snglDscnt = this.getDscntLessTax(txCodeID, Global.getSalesDocCodesAmnt(dscntCodeID, orgnlSllngPrce, 1));
                                 ttlDscntTax = initialDscnt - ttlDsctAmnt;

                                 Global.createScmRcvblsDocDet(docID, "3Discount",
                       "Take Back Discounts (" + dscntCodeNm + ") on Sales Return (" + docIDNum + ") IRO " + itmDesc + " by " + cstmrNm + " (" + docDesc + ")",
                       ttlDsctAmnt, entrdCurrID, dscntCodeID, docTyp, false, "Decrease", salesDscntAcntID,
                       "Decrease", dfltLbltyAccnt, -1, "VALID", -1, this.curid, accntCurrID,
                       funcCurrrate, accntCurrRate, (funcCurrrate * ttlDsctAmnt),
                       (accntCurrRate * ttlDsctAmnt));
                                 ttlRvnuAmnt -= ttlDscntTax;
                             }
                         }
                     }

                     if (txCodeID > 0)
                     {
                         isParnt = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "is_parent", txCodeID);
                         if (isParnt == "1")
                         {
                             string[] codeIDs = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "child_code_ids", txCodeID).Split(w, StringSplitOptions.RemoveEmptyEntries);
                             for (int j = 0; j < codeIDs.Length; j++)
                             {
                                 if (int.Parse(codeIDs[j]) > 0)
                                 {
                                     double ttlTxAmnt = Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), orgnlSllngPrce - snglDscnt, qnty);
                                     string txCodeNm = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "code_name", int.Parse(codeIDs[j]));
                                     txPyblAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "taxes_payables_accnt_id", int.Parse(codeIDs[j])));
                                     if (txPyblAcntID > 0 && dfltLbltyAccnt > 0)
                                     {
                                         Global.createScmRcvblsDocDet(docID, "2Tax",
                           "Refund Taxes (" + txCodeNm + ") on Sales Return (" + docIDNum + ") IRO " + itmDesc + " by " + cstmrNm + " (" + docDesc + ")",
                           ttlTxAmnt, entrdCurrID, int.Parse(codeIDs[j]), docTyp, false, "Decrease", txPyblAcntID,
                           "Increase", dfltLbltyAccnt, -1, "VALID", -1, this.curid, accntCurrID,
                           funcCurrrate, accntCurrRate, (funcCurrrate * ttlTxAmnt),
                           (accntCurrRate * ttlTxAmnt));
                                         ttlRvnuAmnt -= ttlTxAmnt;
                                     }
                                 }
                             }
                         }
                         else
                         {
                             txPyblAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "taxes_payables_accnt_id", txCodeID));
                             if (txPyblAcntID > 0 && dfltLbltyAccnt > 0)
                             {
                                 double ttlTxAmnt = Global.getSalesDocCodesAmnt(txCodeID, orgnlSllngPrce - snglDscnt, qnty);
                                 string txCodeNm = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "code_name", txCodeID);
                                 Global.createScmRcvblsDocDet(docID, "2Tax",
                       "Refund Taxes (" + txCodeNm + ") on Sales Return (" + docIDNum + ") IRO " + itmDesc + " by " + cstmrNm + " (" + docDesc + ")",
                       ttlTxAmnt, entrdCurrID, txCodeID, docTyp, false, "Decrease", txPyblAcntID,
                       "Increase", dfltLbltyAccnt, -1, "VALID", -1, this.curid, accntCurrID,
                       funcCurrrate, accntCurrRate, (funcCurrrate * ttlTxAmnt),
                       (accntCurrRate * ttlTxAmnt));
                                 ttlRvnuAmnt -= ttlTxAmnt;
                             }
                         }
                     }

                     if (chrgCodeID > 0)
                     {
                         isParnt = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "is_parent", chrgCodeID);
                         if (isParnt == "1")
                         {
                             string[] codeIDs = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id",
                               "child_code_ids", chrgCodeID).Split(w, StringSplitOptions.RemoveEmptyEntries);
                             for (int j = 0; j < codeIDs.Length; j++)
                             {
                                 if (int.Parse(codeIDs[j]) > 0)
                                 {
                                     double ttlChrgAmnt = Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), orgnlSllngPrce, qnty);
                                     string chrgCodeNm = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "code_name", int.Parse(codeIDs[j]));
                                     chrgRvnuAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "chrge_revnu_accnt_id", int.Parse(codeIDs[j])));

                                     if (chrgRvnuAcntID > 0 && dfltLbltyAccnt > 0)
                                     {
                                         Global.createScmRcvblsDocDet(docID, "4Extra Charge",
                           "Refund Extra Charges (" + chrgCodeNm + ") on Sales Return (" + docIDNum + ") IRO " + itmDesc + " by " + cstmrNm + " (" + docDesc + ")",
                           ttlChrgAmnt, entrdCurrID, int.Parse(codeIDs[j]), docTyp, false, "Decrease", chrgRvnuAcntID,
                           "Increase", dfltLbltyAccnt, -1, "VALID", -1, this.curid, accntCurrID,
                           funcCurrrate, accntCurrRate, (funcCurrrate * ttlChrgAmnt),
                           (accntCurrRate * ttlChrgAmnt));
                                     }
                                 }
                             }
                         }
                         else
                         {
                             chrgRvnuAcntID = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "chrge_revnu_accnt_id", chrgCodeID));
                             if (chrgRvnuAcntID > 0 && dfltLbltyAccnt > 0)
                             {
                                 double ttlChrgAmnt = Global.getSalesDocCodesAmnt(chrgCodeID, orgnlSllngPrce, qnty);
                                 string chrgCodeNm = Global.mnFrm.cmCde.getGnrlRecNm(
                             "scm.scm_tax_codes", "code_id", "code_name",
                             chrgCodeID);

                                 Global.createScmRcvblsDocDet(docID, "4Extra Charge",
                       "Refund Extra Charges (" + chrgCodeNm + ") on Sales Return (" + docIDNum + ") IRO " + itmDesc + " by " + cstmrNm + " (" + docDesc + ")",
                       ttlChrgAmnt, entrdCurrID, chrgCodeID, docTyp, false, "Decrease", chrgRvnuAcntID,
                       "Increase", dfltLbltyAccnt, -1, "VALID", -1, this.curid, accntCurrID,
                       funcCurrrate, accntCurrRate, (funcCurrrate * ttlChrgAmnt),
                       (accntCurrRate * ttlChrgAmnt));
                             }
                         }
                     }
                     if (dfltRvnuAcntID > 0 && dfltLbltyAccnt > 0)
                     {
                         Global.createScmRcvblsDocDet(docID, "1Initial Amount",
                   "Refund from Sales Return (" + docIDNum + ") IRO " + itmDesc + " to " + cstmrNm + " (" + docDesc + ")",
                   ttlRvnuAmnt, entrdCurrID, -1, docTyp, false, "Decrease", dfltRvnuAcntID,
                   "Increase", dfltLbltyAccnt, -1, "VALID", -1, this.curid, accntCurrID,
                   funcCurrrate, accntCurrRate, (funcCurrrate * ttlRvnuAmnt),
                   (accntCurrRate * ttlRvnuAmnt));
                     }
                 }
                 Global.roundScmRcvblsDocAmnts(docID, docTyp);
                 return succs;
             }
             catch (Exception ex)
             {
                 Global.mnFrm.cmCde.showMsg(ex.InnerException + "\r\n" + ex.StackTrace + "\r\n" + ex.Message, 0);
                 return false;
             }
         }
         */

        private bool udateItemBalances(long itmID, double qnty, string cnsgmntIDs,
          int txCodeID, int dscntCodeID, int chrgCodeID,
          string docTyp, long docID, long srcDocID, int dfltRcvblAcntID,
          int dfltInvAcntID, int dfltCGSAcntID, int dfltExpnsAcntID, int dfltRvnuAcntID,
          long stckID, double unitSllgPrc, int crncyID, long docLnID,
          int dfltSRAcntID, int dfltCashAcntID, int dfltCheckAcntID, long srcDocLnID,
          string dateStr, string docIDNum, int entrdCurrID, decimal exchngRate, int dfltLbltyAccnt, string strSrcDocType)
        {
            try
            {
                bool succs = true;
                /*For each Item in a Sales Invoice
                 * 1. Get Items Consgnmnt Cost Prices using all selected consignments and their used qtys
                 * 2. Decrease Inv Account by Cost Price --0Inventory
                 * 3. Increase Cost of Goods Sold by Cost Price --0Inventory
                 * 4. Get Selling Price, Taxes, Extra Charges, Discounts
                 * 5. Get Net Selling Price = (Selling Price - Taxes - Extra Charges + Discounts)*Qty
                 * 6. Increase Revenue Account by Net Selling Price --1Initial Amount
                 * 7. Increase Receivables account by Net Selling price --1Initial Amount
                 * 8. Increase Taxes Payable by Taxes  --2Tax
                 * 9. Increase Receivables account by Taxes --2Tax
                 * 10.Increase Extra Charges Revenue by Extra Charges --4Extra Charge
                 * 11.Increase Receivables account by Extra Charges --4Extra Charge
                 * 12.Increase Sales Discounts by Discounts --3Discount
                 * 13.Decrease Receivables by Discounts --3Discount
                 */
                string itmType = Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_itm_list", "item_id", "item_type", itmID);
                //        string dateStr = DateTime.ParseExact(
                //Global.mnFrm.cmCde.getDB_Date_time(), "yyyy-MM-dd HH:mm:ss",
                //System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy HH:mm:ss");
                //     long SIDocID = -1;
                //     long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr",
                //"invc_hdr_id", "src_doc_hdr_id", docID),out SIDocID);
                //Create a List of Consignment IDs, Quantity Used in this doc, Cost Price
                //Get ttlSllngPrc, ttlTxAmnt, ttlChrgAmnt, ttlDsctAmnt for this item only

                //For Sales Invoice, Sales Return, Item Issues-Unbilled Docs get the ff
                if (itmType.Contains("Inventory")
                  || itmType.Contains("Fixed Assets"))
                {
                    List<string[]> csngmtData;
                    if (docTyp != "Sales Return")
                    {
                        csngmtData = Global.getItmCnsgmtVals(qnty, cnsgmntIDs);
                    }
                    else
                    {
                        csngmtData = Global.getSRItmCnsgmtVals(
                          docLnID, qnty, cnsgmntIDs, srcDocLnID);
                    }
                    //From the List get Total Cost Price of the Item
                    string csgmtQtyDists = ",";
                    for (int i = 0; i < csngmtData.Count; i++)
                    {
                        string[] ary = csngmtData[i];
                        long figID = 0;
                        long.TryParse(ary[0], out figID);
                        double fig1Qty = 0;
                        double fig2Prc = 0;
                        double.TryParse(ary[1], out fig1Qty);
                        double.TryParse(ary[2], out fig2Prc);
                        csgmtQtyDists = csgmtQtyDists + fig1Qty.ToString() + ",";
                        if (docTyp == "Sales Order")
                        {
                            Global.postCnsgnmntQty(figID, 0, fig1Qty, -1 * fig1Qty, dateStr, "SO" + docLnID.ToString());
                            Global.postStockQty(stckID, 0, fig1Qty, -1 * fig1Qty, dateStr, "SO" + docLnID.ToString());
                        }
                        else if (docTyp == "Sales Invoice")
                        {
                            if (strSrcDocType == "Sales Order")
                            {
                                Global.postCnsgnmntQty(figID, -1 * fig1Qty, -1 * fig1Qty, 0, dateStr, "SI" + docLnID.ToString());
                                Global.postStockQty(stckID, -1 * fig1Qty, -1 * fig1Qty, 0, dateStr, "SI" + docLnID.ToString());
                            }
                            else
                            {
                                Global.postCnsgnmntQty(figID, -1 * fig1Qty, 0, -1 * fig1Qty, dateStr, "SI" + docLnID.ToString());
                                Global.postStockQty(stckID, -1 * fig1Qty, 0, -1 * fig1Qty, dateStr, "SI" + docLnID.ToString());
                            }
                        }
                        else if (docTyp == "Item Issue-Unbilled")
                        {
                            Global.postCnsgnmntQty(figID, -1 * fig1Qty, 0, -1 * fig1Qty, dateStr, "IU" + docLnID.ToString());
                            Global.postStockQty(stckID, -1 * fig1Qty, 0, -1 * fig1Qty, dateStr, "IU" + docLnID.ToString());
                        }
                        else if (docTyp == "Sales Return")
                        {
                            Global.postCnsgnmntQty(figID, fig1Qty, 0, fig1Qty, dateStr, "SR" + docLnID.ToString());
                            Global.postStockQty(stckID, fig1Qty, 0, fig1Qty, dateStr, "SR" + docLnID.ToString());
                        }
                    }
                    Global.updateSalesLnCsgmtDist(docLnID, csgmtQtyDists.Trim(','));
                }

                return succs;
            }
            catch (Exception ex)
            {
                Global.mnFrm.cmCde.showMsg(ex.InnerException + "\r\n" + ex.StackTrace + "\r\n" + ex.Message, 0);
                return false;
            }
        }

        public void reCalcRcvblsSmmrys(long srcDocID, string srcDocType)
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
                  -1, 0, 0, 0, 0);
            }
            else
            {
                Global.updtRcvblsDocDet(smmryID, srcDocID, "6Grand Total",
                  smmryNm, grndAmnt, int.Parse(this.invcCurrIDTextBox.Text),
                  -1, srcDocType, true, "Increase",
                  -1, "Increase", -1, -1, "VALID", -1, -1,
                  -1, 0, 0, 0, 0);
            }

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
                  -1, 0, 0, 0, 0);
            }
            else
            {
                Global.updtRcvblsDocDet(smmryID, srcDocID, "7Total Payments Made",
                  smmryNm, pymntsAmnt, int.Parse(this.invcCurrIDTextBox.Text),
                  -1, srcDocType, true, "Increase",
                  -1, "Increase", -1, -1, "VALID", -1, -1,
                  -1, 0, 0, 0, 0);
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
                  -1, 0, 0, 0, 0);
            }
            else
            {
                Global.updtRcvblsDocDet(smmryID, srcDocID, "8Outstanding Balance",
                  smmryNm, outstndngAmnt, int.Parse(this.invcCurrIDTextBox.Text),
                  -1, srcDocType, true, "Increase",
                  -1, "Increase", -1, -1, "VALID", -1, -1,
                  -1, 0, 0, 0, 0);
            }
        }

        public bool approveRcvblsDoc(long docHdrID, string docNum)
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

                /*+Global.mnFrm.cmCde.getDB_Date_time().Substring(11, 8).Replace(":", "").Replace("-", "").Replace(" ", "") + "-" +
            Global.getNewBatchID().ToString().PadLeft(4, '0');*/
                long glBatchID = Global.mnFrm.cmCde.getGnrlRecID("accb.accb_trnsctn_batches",
                  "batch_name", "batch_id", glBatchName, Global.mnFrm.cmCde.Org_id);

                if (glBatchID <= 0)
                {
                    Global.createBatch(Global.mnFrm.cmCde.Org_id, glBatchName,
                      this.docCommentsTextBox.Text + " (" + docNum + ")",
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
                string lnDte = this.docDteTextBox.Text + " 00:00:00";
                DataSet dtst = Global.get_RcvblsDocDet(docHdrID);
                for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
                {
                    string lineTypeNm = dtst.Tables[0].Rows[i][1].ToString();
                    int codeBhndID = -1;
                    int.TryParse(dtst.Tables[0].Rows[i][4].ToString(), out codeBhndID);

                    string incrDcrs1 = dtst.Tables[0].Rows[i][6].ToString().Substring(0, 1);
                    int accntID1 = -1;
                    int.TryParse(dtst.Tables[0].Rows[i][7].ToString(), out accntID1);
                    string isdbtCrdt1 = Global.mnFrm.cmCde.dbtOrCrdtAccnt(accntID1, incrDcrs1.Substring(0, 1));

                    string incrDcrs2 = dtst.Tables[0].Rows[i][8].ToString().Substring(0, 1);
                    int accntID2 = -1;
                    int.TryParse(dtst.Tables[0].Rows[i][9].ToString(), out accntID2);
                    rcvblAccntID = accntID2;
                    string isdbtCrdt2 = Global.mnFrm.cmCde.dbtOrCrdtAccnt(accntID2, incrDcrs2.Substring(0, 1));

                    double lnAmnt = double.Parse(dtst.Tables[0].Rows[i][19].ToString());

                    System.Windows.Forms.Application.DoEvents();

                    double acntAmnt = 0;
                    double.TryParse(dtst.Tables[0].Rows[i][20].ToString(), out acntAmnt);
                    double entrdAmnt = 0;
                    double.TryParse(dtst.Tables[0].Rows[i][3].ToString(), out entrdAmnt);

                    string lneDesc = dtst.Tables[0].Rows[i][2].ToString();
                    int entrdCurrID = int.Parse(dtst.Tables[0].Rows[i][11].ToString());
                    int funcCurrID = int.Parse(dtst.Tables[0].Rows[i][13].ToString());
                    int accntCurrID = int.Parse(dtst.Tables[0].Rows[i][15].ToString());
                    double funcCurrRate = double.Parse(dtst.Tables[0].Rows[i][17].ToString());
                    double accntCurrRate = double.Parse(dtst.Tables[0].Rows[i][18].ToString());

                    if (accntID1 > 0 && (lnAmnt != 0 || acntAmnt != 0) && incrDcrs1 != "" && lneDesc != "")
                    {
                        double netAmnt = (double)Global.dbtOrCrdtAccntMultiplier(accntID1,
                  incrDcrs1) * (double)lnAmnt;


                        //if (!Global.mnFrm.cmCde.isTransPrmttd(accntID1, lnDte, netAmnt))
                        //{
                        //  return false;
                        //}

                        if (Global.dbtOrCrdtAccnt(accntID1,
                          incrDcrs1) == "Debit")
                        {
                            Global.createTransaction(accntID1,
                              lneDesc, lnAmnt,
                              lnDte, funcCurrID, glBatchID, 0.00,
                              netAmnt, entrdAmnt, entrdCurrID, acntAmnt, accntCurrID, funcCurrRate, accntCurrRate, "D");
                        }
                        else
                        {
                            Global.createTransaction(accntID1,
                              lneDesc, 0.00,
                              lnDte, funcCurrID,
                              glBatchID, lnAmnt, netAmnt,
                      entrdAmnt, entrdCurrID, acntAmnt, accntCurrID, funcCurrRate, accntCurrRate, "C");
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


                //if (!Global.mnFrm.cmCde.isTransPrmttd(rcvblAccntID, lnDte, netAmnt1))
                //{
                //  return false;
                //}

                if (Global.dbtOrCrdtAccnt(rcvblAccntID,
                  "I") == "Debit")
                {
                    Global.createTransaction(rcvblAccntID,
                      this.docCommentsTextBox.Text +
                      " (Balacing Leg for Receivables Doc:-" +
                      this.docIDNumTextBox.Text + ")", funcCurrAmnt,
                      lnDte, this.curid, glBatchID, 0.00,
                      netAmnt1, grndAmnt, int.Parse(this.invcCurrIDTextBox.Text),
                      accntCurrAmnt, accntCurrID1, funcCurrRate1, accntCurrRate1, "D");
                }
                else
                {
                    Global.createTransaction(rcvblAccntID,
                      this.docCommentsTextBox.Text +
                      " (Balacing Leg for Receivables Doc:-" +
                      this.docIDNumTextBox.Text + ")", 0.00,
                      lnDte, this.curid,
                      glBatchID, funcCurrAmnt, netAmnt1,
               grndAmnt, int.Parse(this.invcCurrIDTextBox.Text), accntCurrAmnt,
               accntCurrID1, funcCurrRate1, accntCurrRate1, "C");
                }
                if (Global.get_Batch_CrdtSum(glBatchID) == Global.get_Batch_DbtSum(glBatchID))
                {
                    Global.updtRcvblsDocGLBatch(docHdrID, glBatchID);
                    //this.updateAppldPrepayHdrs();
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
                Global.mnFrm.cmCde.showMsg("Receivables Document Approval Failed!", 0);
                return false;
            }
        }

        private bool rvrsQtyPostngs(long lnID, string cnsgmntIDs, string dateStr, long stckID, string strSrcDocType, string docTyp)
        {
            List<string[]> csngmtData = Global.getCsgmtsDist(lnID, cnsgmntIDs);

            foreach (string[] ary in csngmtData)
            {
                //string[] ary = csngmtData[a];
                long figID = 0;
                long.TryParse(ary[0], out figID);
                double fig1Qty = double.Parse(ary[1]);
                double fig2Prc = double.Parse(ary[2]);
                //Global.mnFrm.cmCde.showMsg(cnsgmntIDs + "/" + ary[0], 0);

                //double.TryParse(ary[1], out fig1Qty);
                //double.TryParse(ary[2], out fig2Prc);
                //                string docTyp = this.docTypeComboBox.Text;
                if (docTyp == "Sales Order")
                {
                    dateStr = Global.getCsgmntBlsTrnsDte("SO" + lnID.ToString(), dateStr, figID);
                    if (dateStr != "")
                    {
                        Global.undoPostCnsgnmntQty(figID, 0, fig1Qty, -1 * fig1Qty, dateStr, "SO" + lnID.ToString());
                        dateStr = Global.getStockBlsTrnsDte("SO" + lnID.ToString(), dateStr, stckID);
                        Global.undoPostStockQty(stckID, 0, fig1Qty, -1 * fig1Qty, dateStr, "SO" + lnID.ToString());
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (docTyp == "Sales Invoice")
                {
                    if (strSrcDocType == "Sales Order")
                    {
                        dateStr = Global.getCsgmntBlsTrnsDte("SI" + lnID.ToString(), dateStr, figID);
                        if (dateStr != "")
                        {
                            Global.undoPostCnsgnmntQty(figID, -1 * fig1Qty, -1 * fig1Qty, 0, dateStr, "SI" + lnID.ToString());
                            dateStr = Global.getStockBlsTrnsDte("SI" + lnID.ToString(), dateStr, stckID);
                            Global.undoPostStockQty(stckID, -1 * fig1Qty, -1 * fig1Qty, 0, dateStr, "SI" + lnID.ToString());
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        dateStr = Global.getCsgmntBlsTrnsDte("SI" + lnID.ToString(), dateStr, figID);
                        //Global.mnFrm.cmCde.showMsg("SI" + lnID.ToString() + "/" + dateStr + "/" + figID + "/" + fig1Qty, 0);
                        if (dateStr != "")
                        {
                            Global.undoPostCnsgnmntQty(figID, -1 * fig1Qty, 0, -1 * fig1Qty, dateStr, "SI" + lnID.ToString());
                            dateStr = Global.getStockBlsTrnsDte("SI" + lnID.ToString(), dateStr, stckID);
                            Global.undoPostStockQty(stckID, -1 * fig1Qty, 0, -1 * fig1Qty, dateStr, "SI" + lnID.ToString());
                        }
                        else
                        {
                            return false;
                        }
                    }

                }
                else if (docTyp == "Item Issue-Unbilled")
                {
                    dateStr = Global.getCsgmntBlsTrnsDte("IU" + lnID.ToString(), dateStr, figID);
                    if (dateStr != "")
                    {
                        Global.undoPostCnsgnmntQty(figID, -1 * fig1Qty, 0, -1 * fig1Qty, dateStr, "IU" + lnID.ToString());
                        dateStr = Global.getStockBlsTrnsDte("IU" + lnID.ToString(), dateStr, stckID);
                        Global.undoPostStockQty(stckID, -1 * fig1Qty, 0, -1 * fig1Qty, dateStr, "IU" + lnID.ToString());
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (docTyp == "Sales Return")
                {
                    dateStr = Global.getCsgmntBlsTrnsDte("SR" + lnID.ToString(), dateStr, figID);
                    if (dateStr != "")
                    {
                        Global.undoPostCnsgnmntQty(figID, fig1Qty, 0, fig1Qty, dateStr, "SR" + lnID.ToString());
                        dateStr = Global.getStockBlsTrnsDte("SR" + lnID.ToString(), dateStr, stckID);
                        Global.undoPostStockQty(stckID, fig1Qty, 0, fig1Qty, dateStr, "SR" + lnID.ToString());
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            Global.updateSalesLnDlvrd(lnID, false);
            return true;
            //Global.deleteDocGLInfcLns(long.Parse(this.docIDTextBox.Text), "Restaurant Order");
            //Global.deleteScmRcvblsDocDets(long.Parse(this.docIDTextBox.Text), this.docIDNumTextBox.Text);
        }

        private void nxtApprvlStatusButton_Click(object sender, EventArgs e)
        {
            if (this.docIDTextBox.Text == "" || this.docIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("Please select a Saved Document First!", 0);
                this.saveLabel.Visible = false;
                return;
            }

            if (this.itemsDataGridView.Rows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("The Document has no Items hence cannot be Validated!", 0);
                this.saveLabel.Visible = false;
                return;
            }

            if (this.docSaved == false)
            {
                Global.mnFrm.cmCde.showMsg("Please Save the Document First!", 0);
                this.saveLabel.Visible = false;
                return;
            }
            if ((this.extAppDocIDTextBox.Text != "" && this.extAppDocIDTextBox.Text != "-1"))
            {
                Global.mnFrm.cmCde.showMsg("Cannot Work on Documents Created from other Modules!", 0);
                this.saveLabel.Visible = false;
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
                if (MessageBox.Show("Are you sure you want to APPROVE the selected Document?" +
            "\r\nThis action cannot be undone!", "Rhomicom Message",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                    this.saveLabel.Visible = false;
                    Cursor.Current = Cursors.Default;

                    System.Windows.Forms.Application.DoEvents();
                    //System.Windows.Forms.Application.DoEvents();
                    return;
                }

                this.disableDetEdit();
                this.disableLnsEdit();
                this.populateDet(long.Parse(this.docIDTextBox.Text));
                this.populateLines(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
                this.populateSmmry(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);

                //Do Accounting Transactions
                //string srcDocType = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr", "invc_hdr_id", "invc_type", long.Parse(this.srcDocIDTextBox.Text));
                this.saveLabel.Text = "VALIDATING DOCUMENT....PLEASE WAIT....";
                this.saveLabel.Visible = true;
                Cursor.Current = Cursors.WaitCursor;
                System.Windows.Forms.Application.DoEvents();
                //System.Windows.Forms.Application.DoEvents();
                //System.Windows.Forms.Application.DoEvents();
                //System.Windows.Forms.Application.DoEvents();
                //System.Windows.Forms.Application.DoEvents();
                //System.Windows.Forms.Application.DoEvents();

                string srcDocType = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr", "invc_hdr_id", "invc_type", long.Parse(this.srcDocIDTextBox.Text));
                string apprvlStatus = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr", "invc_hdr_id", "approval_status", long.Parse(this.docIDTextBox.Text));
                bool isvald = false;
                if (apprvlStatus == "Not Validated")
                {
                    isvald = this.validateLns(srcDocType);
                    if (isvald)
                    {
                        for (int i = 0; i < this.itemsDataGridView.Rows.Count; i++)
                        {
                            if (this.itemsDataGridView.Rows[i].Cells[16].Value.ToString() != "-1")
                            {
                                Global.updtSrcDocTrnsctdQty(long.Parse(this.itemsDataGridView.Rows[i].Cells[16].Value.ToString()),
                                  double.Parse(this.itemsDataGridView.Rows[i].Cells[4].Value.ToString()));
                            }
                        }
                        Global.updtSalesDocApprvl(long.Parse(this.docIDTextBox.Text), "Validated", "Approve");
                    }
                    else
                    {
                        //if invalid disallow
                        this.saveLabel.Visible = false;
                        Cursor.Current = Cursors.Default;
                        System.Windows.Forms.Application.DoEvents();
                        return;
                    }
                }
                else
                {
                    //if validated users must reject and redo validation and approval
                    this.rejectDocButton_Click(this.rejectDocButton, e);
                    Global.mnFrm.cmCde.showMsg("Please try Submitting this Document for Approval Again!", 0);
                    this.saveLabel.Visible = false;
                    Cursor.Current = Cursors.Default;
                    this.populateDet(long.Parse(this.docIDTextBox.Text));
                    System.Windows.Forms.Application.DoEvents();
                    return;
                }
                this.saveLabel.Text = "UPDATING ITEM BALANCES....PLEASE WAIT....";
                this.saveLabel.Visible = true;
                Cursor.Current = Cursors.WaitCursor;
                System.Windows.Forms.Application.DoEvents();
                Cursor.Current = Cursors.WaitCursor;

                double invcAmnt = 0;
                string dateStr = Global.mnFrm.cmCde.getFrmtdDB_Date_time();

                this.backgroundWorker2.WorkerReportsProgress = true;
                this.backgroundWorker2.WorkerSupportsCancellation = true;


                Object[] args = {this.docIDTextBox.Text, dateStr, this.docTypeComboBox.Text,
                        this.docIDNumTextBox.Text, this.srcDocIDTextBox.Text,
                        this.invcCurrIDTextBox.Text,this.exchRateNumUpDwn.Value.ToString(), srcDocType};

                this.backgroundWorker2.RunWorkerAsync(args);

                int cntrWait = 0;
                do
                {
                    //Nothing
                    System.Windows.Forms.Application.DoEvents();
                    Cursor.Current = Cursors.WaitCursor;
                    cntrWait++;
                    System.Threading.Thread.Sleep(200);
                }
                while (this.backgroundWorker1.IsBusy == true && cntrWait < 20);


                this.saveLabel.Text = "CREATING ACCOUNTING FOR DOCUMENT....PLEASE WAIT....";
                this.saveLabel.Visible = true;
                System.Windows.Forms.Application.DoEvents();
                Cursor.Current = Cursors.WaitCursor;

                if (true)
                {
                    bool apprvlSccs = true;

                    long rcvblDocID = Global.get_ScmRcvblsDocHdrID(long.Parse(this.docIDTextBox.Text),
               this.docTypeComboBox.Text, Global.mnFrm.cmCde.Org_id);
                    string rcvblDocNum = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
                      "rcvbls_invc_hdr_id", "rcvbls_invc_number", rcvblDocID);
                    string rcvblDocType = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
                      "rcvbls_invc_hdr_id", "rcvbls_invc_type", rcvblDocID);

                    if (rcvblDocID > 0)
                    {
                        apprvlSccs = this.approveRcvblsDoc(rcvblDocID, rcvblDocNum);
                    }
                    if (apprvlSccs)
                    {
                        invcAmnt = Global.getRcvblsDocGrndAmnt(rcvblDocID);
                        Global.updtRcvblsDocApprvl(rcvblDocID, "Approved", "Cancel", invcAmnt);
                        Global.updtSalesDocApprvl(long.Parse(this.docIDTextBox.Text), "Approved", "Cancel");
                        this.apprvlStatusTextBox.Text = "Approved";
                        this.nxtApprvlStatusButton.Text = "Cancel";
                        this.nxtApprvlStatusButton.ImageKey = "90.png";
                        this.disableDetEdit();
                        this.disableLnsEdit();
                        this.populateDet(long.Parse(this.docIDTextBox.Text));
                        this.populateLines(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
                        this.populateSmmry(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
                    }
                    else
                    {
                        this.rvrsApprval(dateStr, this.docTypeComboBox.Text);
                        Global.deleteRcvblsDocHdrNDet(rcvblDocID, rcvblDocNum);
                        this.saveLabel.Visible = false;
                        Cursor.Current = Cursors.Default;
                        System.Windows.Forms.Application.DoEvents();
                        return;
                    }
                }
                else
                {
                    this.rvrsApprval(dateStr, this.docTypeComboBox.Text);
                    this.saveLabel.Visible = false;
                    Cursor.Current = Cursors.Default;
                    System.Windows.Forms.Application.DoEvents();
                    return;
                }

                this.saveLabel.Visible = false;
                Cursor.Current = Cursors.Default;
                System.Windows.Forms.Application.DoEvents();
                if (this.payDocs && this.docTypeComboBox.Text == "Sales Invoice")
                {
                    /*
                    && this.allowDuesCheckBox.Checked == false*/
                    this.processPayButton_Click(this.processPayButton, e);
                }
            }
            else if (this.nxtApprvlStatusButton.Text.Contains("Review"))
            {
                if (Global.mnFrm.cmCde.showMsg("Are you sure you want to FORWARD the selected Document?" +
            "\r\nThis action cannot be undone!", 1) == DialogResult.No)
                {
                    Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                    this.saveLabel.Visible = false;
                    Cursor.Current = Cursors.Default;
                    return;
                }
                //Check Approval Hierarchy

                if (true)
                {
                    Global.updtSalesDocApprvl(long.Parse(this.docIDTextBox.Text), "Reviewed 1", "Review 2");
                    this.apprvlStatusTextBox.Text = "Reviewed 1";
                    this.nxtApprvlStatusButton.Text = "Review 2";
                    this.nxtApprvlStatusButton.ImageKey = "tick_64.png";
                }
            }
            else if (this.nxtApprvlStatusButton.Text == "Cancel")
            {
                //Global.mnFrm.cmCde.showMsg("Not Yet Implemented !", 3);
                //return;
                //Will do what rejection does and the reversal of what approve did
                if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[71]) == false)
                {
                    Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                      " this action!\nContact your System Administrator!", 0);
                    this.saveLabel.Visible = false;
                    Cursor.Current = Cursors.Default;
                    return;
                }
                //Check if Unreversed Payments Exists then disallow else allow
                //and reverse accounting Transactions
                long rcvblHdrID = Global.get_ScmRcvblsDocHdrID(long.Parse(this.docIDTextBox.Text),
                  this.docTypeComboBox.Text, Global.mnFrm.cmCde.Org_id);
                string rcvblDoctype = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
                  "rcvbls_invc_hdr_id", "rcvbls_invc_type", rcvblHdrID);
                double pymntsAmnt = Math.Round(Global.getRcvblsDocTtlPymnts(rcvblHdrID, rcvblDoctype), 2);
                if (pymntsAmnt != 0)
                {
                    Global.mnFrm.cmCde.showMsg("Please Reverse all Payments on this Document First!\r\n(TOTAL AMOUNT PAID=" + pymntsAmnt.ToString("#,##0.00") + ")", 0);
                    this.saveLabel.Visible = false;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (Global.mnFrm.cmCde.showMsg("Are you sure you want to CANCEL the selected Document?" +
                "\r\nThis action cannot be undone!", 1) == DialogResult.No)
                {
                    //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                    this.saveLabel.Visible = false;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                this.saveLabel.Text = "CANCELLING DOCUMENT....PLEASE WAIT....";
                this.saveLabel.Visible = true;
                Cursor.Current = Cursors.WaitCursor;

                System.Windows.Forms.Application.DoEvents();

                this.nxtApprvlStatusButton.Enabled = false;
                System.Windows.Forms.Application.DoEvents();
                /*bool isAnyRnng = true;
                int witcntr = 0;
                do
                {
                    witcntr++;
                    isAnyRnng = Global.isThereANActvActnPrcss("7", "10 second");//Invetory Import Process
                    System.Windows.Forms.Application.DoEvents();
                }
                while (isAnyRnng == true);*/

                string dateStr = Global.mnFrm.cmCde.getFrmtdDB_Date_time();
                bool sccs = this.rvrsApprval(dateStr, this.docTypeComboBox.Text);
                if (sccs)
                {
                    sccs = this.rvrsImprtdIntrfcTrns(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
                }
                if (sccs)
                {
                    sccs = this.voidAttachedBatch(rcvblHdrID, rcvblDoctype);
                }
                if (sccs)
                {
                    Global.updtSalesDocApprvl(long.Parse(this.docIDTextBox.Text), "Cancelled", "None");
                    Global.updtRcvblsDocApprvl(rcvblHdrID, "Cancelled", "None");
                    this.apprvlStatusTextBox.Text = "Cancelled";
                    this.nxtApprvlStatusButton.Text = "None";
                    this.nxtApprvlStatusButton.ImageKey = "tick_64.png";
                    this.populateDet(long.Parse(this.docIDTextBox.Text));
                    this.rfrshDtButton_Click(this.rfrshDtButton, e);
                }
            }
            this.saveLabel.Visible = false;
            Cursor.Current = Cursors.Default;
        }

        private void checkNCreateRcvblLines(long invcDocHdrID, long rcvblDocID, string rcvblDocNum, string rcvblDocType)
        {

            if (rcvblDocID > 0 && rcvblDocType != "")
            {
                DataSet dtstSmmry = Global.get_ScmRcvblsDocDets(invcDocHdrID);
                for (int i = 0; i < dtstSmmry.Tables[0].Rows.Count; i++)
                {
                    long curlnID = Global.getNewRcvblsLnID();
                    string lineType = dtstSmmry.Tables[0].Rows[i][0].ToString();
                    string lineDesc = dtstSmmry.Tables[0].Rows[i][1].ToString();
                    double entrdAmnt = double.Parse(dtstSmmry.Tables[0].Rows[i][2].ToString());
                    int entrdCurrID = int.Parse(dtstSmmry.Tables[0].Rows[i][10].ToString());
                    int codeBhnd = int.Parse(dtstSmmry.Tables[0].Rows[i][3].ToString());
                    string docType = rcvblDocType;
                    bool autoCalc = Global.mnFrm.cmCde.cnvrtBitStrToBool(dtstSmmry.Tables[0].Rows[i][4].ToString());
                    string incrDcrs1 = dtstSmmry.Tables[0].Rows[i][5].ToString();
                    int costngID = int.Parse(dtstSmmry.Tables[0].Rows[i][6].ToString());
                    string incrDcrs2 = dtstSmmry.Tables[0].Rows[i][7].ToString();
                    int blncgAccntID = int.Parse(dtstSmmry.Tables[0].Rows[i][8].ToString());
                    long prepayDocHdrID = long.Parse(dtstSmmry.Tables[0].Rows[i][9].ToString());
                    string vldyStatus = "VALID";
                    long orgnlLnID = -1;
                    int funcCurrID = int.Parse(dtstSmmry.Tables[0].Rows[i][11].ToString());
                    int accntCurrID = int.Parse(dtstSmmry.Tables[0].Rows[i][12].ToString());
                    double funcCurrRate = double.Parse(dtstSmmry.Tables[0].Rows[i][13].ToString());
                    double accntCurrRate = double.Parse(dtstSmmry.Tables[0].Rows[i][14].ToString());
                    double funcCurrAmnt = double.Parse(dtstSmmry.Tables[0].Rows[i][15].ToString());
                    double accntCurrAmnt = double.Parse(dtstSmmry.Tables[0].Rows[i][16].ToString());
                    Global.createRcvblsDocDet(curlnID, rcvblDocID, lineType,
                                  lineDesc, entrdAmnt, entrdCurrID, codeBhnd, docType, autoCalc, incrDcrs1,
                                  costngID, incrDcrs2, blncgAccntID, prepayDocHdrID, vldyStatus, orgnlLnID, funcCurrID,
                                  accntCurrID, funcCurrRate, accntCurrRate, funcCurrAmnt, accntCurrAmnt);
                }
                this.reCalcRcvblsSmmrys(rcvblDocID, rcvblDocType);
            }
        }

        private void checkNCreateRcvblsHdr(double invcAmnt, string srcDocType, string docTyp)
        {
            //Global.mnFrm.cmCde.showMsg("Inside Rcvbl Hdr", 0);
            long cstmrID = long.Parse(this.cstmrIDTextBox.Text);
            int cstmLblty = -1;
            int cstmRcvbl = -1;
            if (cstmrID > 0)
            {
                cstmLblty = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
            "scm.scm_cstmr_suplr", "cust_sup_id", "dflt_pybl_accnt_id",
            cstmrID));
                cstmRcvbl = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
            "scm.scm_cstmr_suplr", "cust_sup_id", "dflt_rcvbl_accnt_id",
            cstmrID));
            }

            if (cstmLblty > 0)
            {
                this.dfltLbltyAccnt = cstmLblty;
            }

            if (cstmRcvbl > 0)
            {
                this.dfltRcvblAcntID = cstmRcvbl;
            }
            //Global.mnFrm.cmCde.showMsg("Inside Rcvbl Hdr " + dfltRcvblAcntID, 0);

            //int curid = -1;

            string rcvblDocNum = "";
            string rcvblDocType = "";
            //string srcDocType = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr", "invc_hdr_id", "invc_type", long.Parse(this.srcDocIDTextBox.Text));

            long rcvblHdrID = Global.get_ScmRcvblsDocHdrID(long.Parse(this.docIDTextBox.Text),
         docTyp, Global.mnFrm.cmCde.Org_id);

            //Global.mnFrm.cmCde.showMsg("Inside Rcvbl Hdr " + rcvblHdrID, 0);

            if (docTyp == "Sales Invoice")
            {
                if (rcvblHdrID <= 0)
                {
                    rcvblDocNum = "CSP-" +
                    DateTime.Parse(Global.mnFrm.cmCde.getFrmtdDB_Date_time()).ToString("yyMMdd-HHmmss")
                             + "-" + Global.mnFrm.cmCde.getRandomInt(10, 100);


                    /*+"-" +
               Global.mnFrm.cmCde.getFrmtdDB_Date_time().Substring(12, 8).Replace(":", "") + "-" +
                Global.getLtstRecPkID("accb.accb_rcvbls_invc_hdr",
                "rcvbls_invc_hdr_id");*/
                    rcvblDocType = "Customer Standard Payment";
                    Global.createRcvblsDocHdr(Global.mnFrm.cmCde.Org_id, this.docDteTextBox.Text,
                      rcvblDocNum, rcvblDocType, this.docCommentsTextBox.Text,
                      long.Parse(this.docIDTextBox.Text), int.Parse(this.cstmrIDTextBox.Text),
                      int.Parse(this.siteIDTextBox.Text), "Not Validated", "Approve",
                      invcAmnt, this.payTermsTextBox.Text, docTyp,
                      int.Parse(this.pymntMthdIDTextBox.Text), 0, -1, "",
                      "Payment of Customer Goods Delivered", int.Parse(this.invcCurrIDTextBox.Text), 0, dfltRcvblAcntID);
                }
                else
                {
                    rcvblDocNum = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
                  "rcvbls_invc_hdr_id", "rcvbls_invc_number", rcvblHdrID);
                    rcvblDocType = "Customer Standard Payment";
                    Global.updtRcvblsDocHdr(rcvblHdrID, this.docDteTextBox.Text,
                      rcvblDocNum, rcvblDocType, this.docCommentsTextBox.Text,
                      long.Parse(this.docIDTextBox.Text), int.Parse(this.cstmrIDTextBox.Text),
                      int.Parse(this.siteIDTextBox.Text), "Not Validated", "Approve",
                      invcAmnt, this.payTermsTextBox.Text, docTyp,
                      int.Parse(this.pymntMthdIDTextBox.Text), 0, -1, "",
                      "Payment of Customer Goods Delivered", int.Parse(this.invcCurrIDTextBox.Text), 0, dfltRcvblAcntID);
                }
            }
            else if (docTyp == "Sales Return" && srcDocType == "Sales Invoice")
            {
                if (rcvblHdrID <= 0)
                {
                    rcvblDocNum = "CDM-IR" + "-" +
                    DateTime.Parse(Global.mnFrm.cmCde.getFrmtdDB_Date_time()).ToString("yyMMdd-HHmmss")
                             + "-" + Global.mnFrm.cmCde.getRandomInt(10, 100);

                    /*+
                   Global.getLtstRcvblsIDNoInPrfx("CDM-IR") + "-" +
                   Global.mnFrm.cmCde.getFrmtdDB_Date_time().Substring(12, 8).Replace(":", "") + "-" +
                   Global.getLtstRecPkID("accb.accb_rcvbls_invc_hdr",
                   "rcvbls_invc_hdr_id");*/
                    rcvblDocType = "Customer Debit Memo (InDirect Refund)";

                    Global.createRcvblsDocHdr(Global.mnFrm.cmCde.Org_id, this.docDteTextBox.Text,
                      rcvblDocNum, rcvblDocType, this.docCommentsTextBox.Text,
                      long.Parse(this.docIDTextBox.Text), int.Parse(this.cstmrIDTextBox.Text),
                      int.Parse(this.siteIDTextBox.Text), "Not Validated", "Approve",
                      invcAmnt, this.payTermsTextBox.Text, docTyp,
                      int.Parse(this.pymntMthdIDTextBox.Text), 0, -1, "",
                      "Refund-Return of Goods Delivered", int.Parse(this.invcCurrIDTextBox.Text), 0, dfltLbltyAccnt);
                }
                else
                {
                    rcvblDocNum = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
               "rcvbls_invc_hdr_id", "rcvbls_invc_number", rcvblHdrID);
                    rcvblDocType = "Customer Standard Payment";
                    Global.updtRcvblsDocHdr(rcvblHdrID, this.docDteTextBox.Text,
                      rcvblDocNum, rcvblDocType, this.docCommentsTextBox.Text,
                      long.Parse(this.docIDTextBox.Text), int.Parse(this.cstmrIDTextBox.Text),
                      int.Parse(this.siteIDTextBox.Text), "Not Validated", "Approve",
                      invcAmnt, this.payTermsTextBox.Text, docTyp,
                      int.Parse(this.pymntMthdIDTextBox.Text), 0, -1, "",
                      "Refund-Return of Goods Delivered", int.Parse(this.invcCurrIDTextBox.Text), 0, dfltLbltyAccnt);
                }
            }
            //Global.mnFrm.cmCde.showMsg("Inside Rcvbl Hdr " + rcvblDocNum, 0);
        }

        private bool rvrsImprtdIntrfcTrns(long docID, string doctype)
        {
            //try
            //{
            DataSet dtst = Global.getDocGLInfcLns(docID, doctype);
            string dateStr = Global.mnFrm.cmCde.getFrmtdDB_Date_time();
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                int accntID = -1;
                double dbtamount = 0;
                double crdtamount = 0;
                int crncy_id = -1;
                double netamnt = 0;
                long srcDocID = -1;
                long srcDocLnID = -1;

                int.TryParse(dtst.Tables[0].Rows[i][1].ToString(), out accntID);
                double.TryParse(dtst.Tables[0].Rows[i][3].ToString(), out dbtamount);
                double.TryParse(dtst.Tables[0].Rows[i][8].ToString(), out crdtamount);
                int.TryParse(dtst.Tables[0].Rows[i][5].ToString(), out crncy_id);
                double.TryParse(dtst.Tables[0].Rows[i][11].ToString(), out netamnt);
                long.TryParse(dtst.Tables[0].Rows[i][14].ToString(), out srcDocID);
                long.TryParse(dtst.Tables[0].Rows[i][15].ToString(), out srcDocLnID);

                string trnsdte = DateTime.ParseExact(
            dtst.Tables[0].Rows[i][4].ToString(), "yyyy-MM-dd HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy HH:mm:ss");

                Global.createPymntGLIntFcLn(accntID,
            "(Cancellation)" + dtst.Tables[0].Rows[i][2].ToString(),
            -1 * dbtamount, trnsdte,
            crncy_id, -1 * crdtamount,
            -1 * netamnt, dtst.Tables[0].Rows[i][13].ToString(), srcDocID, srcDocLnID, dateStr);

            }
            return true;
            //}
            //catch (Exception ex)
            //{
            //  Global.mnFrm.cmCde.showMsg(ex.InnerException.ToString(), 0);
            //  return false;
            //}
        }

        private bool voidAttachedBatch(long rcvblHdrID, string rcvblDocType)
        {
            try
            {
                long glbatchID = -1;

                if (long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
          "accb.accb_rcvbls_invc_hdr", "rcvbls_invc_hdr_id", "gl_batch_id", rcvblHdrID), out glbatchID) == false)
                {
                    return true;
                }
                //    long glbatchID = long.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
                //"accb.accb_rcvbls_invc_hdr", "rcvbls_invc_hdr_id", "gl_batch_id", rcvblHdrID));
                //     string glbatchstatus = Global.mnFrm.cmCde.getGnrlRecNm(
                //"accb.accb_trnsctn_batches", "batch_id", "batch_status", glbatchID);
                string glbatchNm = Global.mnFrm.cmCde.getGnrlRecNm(
            "accb.accb_trnsctn_batches", "batch_id", "batch_name", glbatchID);
                string glbatchDesc = Global.mnFrm.cmCde.getGnrlRecNm(
            "accb.accb_trnsctn_batches", "batch_id", "batch_description", glbatchID);
                //        if (glbatchstatus == "0")
                //        {
                //          //Delete Batch
                //          bool dltd = true;
                //          DataSet dtst1 = Global.get_Batch_Attachments(glbatchID);

                //          for (int i = 0; i < dtst1.Tables[0].Rows.Count; i++)
                //          {
                //            if (Global.mnFrm.cmCde.deleteAFile(
                //              Global.mnFrm.cmCde.getAcctngImgsDrctry() +
                //@"\" + dtst1.Tables[0].Rows[i][3].ToString()) == true)
                //            {
                //              Global.deleteAttchmnt(long.Parse(dtst1.Tables[0].Rows[i][0].ToString()),
                //                dtst1.Tables[0].Rows[i][2].ToString());
                //            }
                //            else
                //            {
                //              Global.mnFrm.cmCde.showMsg("Could not delete File: " +
                //              Global.mnFrm.cmCde.getAcctngImgsDrctry() +
                //@"\" + dtst1.Tables[0].Rows[i][3].ToString(), 0);
                //              dltd = false;
                //              break;
                //            }
                //          }
                //          if (dltd == true)
                //          {
                //            Global.deleteBatchTrns(glbatchID);
                //            Global.deleteBatch(glbatchID, glbatchNm);
                //          }
                //        }
                //        else
                //        {
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
               dtst.Tables[0].Rows[i][18].ToString());
                }
                //}
                Global.updateBatchAvlblty(nwbatchid, "1");

                return true;
            }
            catch (Exception ex)
            {
                Global.mnFrm.cmCde.showMsg(ex.InnerException.ToString(), 0);
                return false;
            }
        }

        private bool rvrsApprval(string dateStr, string docTyp)
        {
            try
            {
                string srcDocType = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr", "invc_hdr_id", "invc_type", long.Parse(this.srcDocIDTextBox.Text));

                for (int i = 0; i < this.itemsDataGridView.Rows.Count; i++)
                {
                    //Global.updtActnPrcss(7);//Invetory Import Process
                    System.Windows.Forms.Application.DoEvents();
                    long itmID = -1;
                    long storeID = -1;
                    long lnID = -1;
                    long.TryParse(this.itemsDataGridView.Rows[i].Cells[12].Value.ToString(), out itmID);
                    long.TryParse(this.itemsDataGridView.Rows[i].Cells[13].Value.ToString(), out storeID);
                    int.TryParse(this.itemsDataGridView.Rows[i].Cells[14].Value.ToString(), out curid);
                    long.TryParse(this.itemsDataGridView.Rows[i].Cells[15].Value.ToString(), out lnID);
                    long stckID = Global.getItemStockID(itmID, storeID);
                    string cnsgmntIDs = this.itemsDataGridView.Rows[i].Cells[10].Value.ToString();
                    if (this.itemsDataGridView.Rows[i].Cells[16].Value.ToString() != "-1")
                    {
                        Global.updtSrcDocTrnsctdQty(long.Parse(this.itemsDataGridView.Rows[i].Cells[16].Value.ToString()),
                          -1 * double.Parse(this.itemsDataGridView.Rows[i].Cells[4].Value.ToString()));
                    }
                    this.rvrsQtyPostngs(lnID, cnsgmntIDs, dateStr, stckID, srcDocType, docTyp);
                }
                //Global.updtActnPrcss(7);//Invetory Import Process
                Global.deleteScmRcvblsDocDet(long.Parse(this.docIDTextBox.Text));
                Global.deleteDocGLInfcLns(long.Parse(this.docIDTextBox.Text), docTyp);
                return true;
            }
            catch (Exception ex)
            {
                Global.mnFrm.cmCde.showMsg(ex.StackTrace, 0);
                return false;
            }
        }

        private bool voidBadDebtBatch(long rcvblHdrID, string rcvblDocType)
        {
            try
            {
                long glbatchID = -1;

                if (long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
          "accb.accb_rcvbls_invc_hdr", "rcvbls_invc_hdr_id", "debt_gl_batch_id", rcvblHdrID), out glbatchID) == false)
                {
                    return true;
                }
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
                string glNwBatchName = glbatchNm + " (Receivables Document Bad Debt Reversal@" + dateStr + ")";
                long nwbatchid = Global.mnFrm.cmCde.getGnrlRecID("accb.accb_trnsctn_batches",
                  "batch_name", "batch_id", glNwBatchName, Global.mnFrm.cmCde.Org_id);

                if (nwbatchid <= 0)
                {
                    Global.createBatch(Global.mnFrm.cmCde.Org_id,
                     glNwBatchName,
                     glbatchDesc + " (Receivables Document Bad Debt Reversal@" + dateStr + ")",
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
                    dtst.Tables[0].Rows[i][3].ToString() + " (Receivables Document Bad Debt Reversal)", -1 * double.Parse(dtst.Tables[0].Rows[i][4].ToString()),
                    dtst.Tables[0].Rows[i][6].ToString(), int.Parse(dtst.Tables[0].Rows[i][7].ToString()),
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
                Global.updtRcvblsDocBadDbtGLBatch(rcvblHdrID, -1);
                Global.updateBatchAvlblty(nwbatchid, "1");

                return true;
            }
            catch (Exception ex)
            {
                //Global.mnFrm.cmCde.showMsg(ex.InnerException.ToString(), 0);
                return false;
            }
        }

        public bool declareBadDebt(long docHdrID, string docNum)
        {
            /* 1. Create a GL Batch and get all doc lines
             * 2. for each line create costing account transaction
             * 3. create one balancing account transaction using the grand total amount
             * 4. Check if created gl_batch is balanced.
             * 5. if balanced update docHdr else delete the gl batch created and throw error message
             */
            try
            {
                if (this.dfltBadDbtAcntID <= 0)
                {
                    Global.mnFrm.cmCde.showMsg("Bad Debt Account not Defined!\r\n Try Again Later!", 0);
                    return false;
                }
                string glBatchName = "ACC_RCVBL-" +
                 DateTime.Parse(Global.mnFrm.cmCde.getFrmtdDB_Date_time()).ToString("yyMMdd-HHmmss")
                      + "-" + Global.mnFrm.cmCde.getRandomInt(10, 100);
                /*+Global.mnFrm.cmCde.getDB_Date_time().Substring(11, 8).Replace(":", "").Replace("-", "").Replace(" ", "") + "-" +
            Global.getNewBatchID().ToString().PadLeft(4, '0');*/
                long glBatchID = Global.mnFrm.cmCde.getGnrlRecID("accb.accb_trnsctn_batches",
                  "batch_name", "batch_id", glBatchName, Global.mnFrm.cmCde.Org_id);

                if (glBatchID <= 0)
                {
                    Global.createBatch(Global.mnFrm.cmCde.Org_id, glBatchName,
                      "(Declared Bad Debt) " + this.docCommentsTextBox.Text,
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
                string lnDte = this.docDteTextBox.Text;
                DataSet dtst = Global.get_RcvblsDocDet(docHdrID);
                double ttlTaxes = 0;
                for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
                {
                    string lineTypeNm = dtst.Tables[0].Rows[i][1].ToString();
                    if (lineTypeNm == "2Tax")
                    {
                        int codeBhndID = -1;
                        int.TryParse(dtst.Tables[0].Rows[i][4].ToString(), out codeBhndID);

                        string incrDcrs1 = dtst.Tables[0].Rows[i][6].ToString().Substring(0, 1);
                        if (incrDcrs1 == "I")
                        {
                            incrDcrs1 = "D";
                        }
                        else
                        {
                            incrDcrs1 = "I";
                        }
                        int accntID1 = -1;
                        int.TryParse(dtst.Tables[0].Rows[i][7].ToString(), out accntID1);
                        string isdbtCrdt1 = Global.mnFrm.cmCde.dbtOrCrdtAccnt(accntID1, incrDcrs1.Substring(0, 1));

                        //string incrDcrs2 = dtst.Tables[0].Rows[i][8].ToString().Substring(0, 1);
                        int accntID2 = -1;
                        int.TryParse(dtst.Tables[0].Rows[i][9].ToString(), out accntID2);
                        rcvblAccntID = accntID2;
                        //string isdbtCrdt2 = Global.mnFrm.cmCde.dbtOrCrdtAccnt(accntID2, incrDcrs2.Substring(0, 1));

                        double lnAmnt = double.Parse(dtst.Tables[0].Rows[i][19].ToString());
                        ttlTaxes += lnAmnt;

                        System.Windows.Forms.Application.DoEvents();

                        double acntAmnt = 0;
                        double.TryParse(dtst.Tables[0].Rows[i][20].ToString(), out acntAmnt);
                        double entrdAmnt = 0;
                        double.TryParse(dtst.Tables[0].Rows[i][3].ToString(), out entrdAmnt);

                        string lneDesc = "(Declared Bad Debt) " + dtst.Tables[0].Rows[i][2].ToString();
                        int entrdCurrID = int.Parse(dtst.Tables[0].Rows[i][11].ToString());
                        int funcCurrID = int.Parse(dtst.Tables[0].Rows[i][13].ToString());
                        int accntCurrID = int.Parse(dtst.Tables[0].Rows[i][15].ToString());
                        double funcCurrRate = double.Parse(dtst.Tables[0].Rows[i][17].ToString());
                        double accntCurrRate = double.Parse(dtst.Tables[0].Rows[i][18].ToString());

                        if (accntID1 > 0 && (lnAmnt != 0 || acntAmnt != 0) && incrDcrs1 != "" && lneDesc != "")
                        {
                            double netAmnt = (double)Global.dbtOrCrdtAccntMultiplier(accntID1,
                      incrDcrs1) * (double)lnAmnt;

                            if (Global.dbtOrCrdtAccnt(accntID1,
                              incrDcrs1) == "Debit")
                            {
                                Global.createTransaction(accntID1,
                                  lneDesc, lnAmnt,
                                  lnDte, funcCurrID, glBatchID, 0.00,
                                  netAmnt, entrdAmnt, entrdCurrID, acntAmnt, accntCurrID, funcCurrRate, accntCurrRate, "D");
                            }
                            else
                            {
                                Global.createTransaction(accntID1,
                                  lneDesc, 0.00,
                                  lnDte, funcCurrID,
                                  glBatchID, lnAmnt, netAmnt,
                          entrdAmnt, entrdCurrID, acntAmnt, accntCurrID, funcCurrRate, accntCurrRate, "C");
                            }
                        }
                    }
                }
                //Receivable Balancing Leg
                if (rcvblAccntID <= 0)
                {
                    rcvblAccntID = this.dfltRcvblAcntID;
                }
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
            "D") * (double)funcCurrAmnt;

                if (Global.dbtOrCrdtAccnt(rcvblAccntID,
                  "D") == "Debit")
                {
                    Global.createTransaction(rcvblAccntID,
                      "(Declared Bad Debt) " + this.docCommentsTextBox.Text +
                      " (Balacing Leg for Receivables Doc:-" +
                      this.docIDNumTextBox.Text + ")", funcCurrAmnt,
                      lnDte, this.curid, glBatchID, 0.00,
                      netAmnt1, grndAmnt, int.Parse(this.invcCurrIDTextBox.Text),
                      accntCurrAmnt, accntCurrID1, funcCurrRate1, accntCurrRate1, "D");
                }
                else
                {
                    Global.createTransaction(rcvblAccntID,
                      "(Declared Bad Debt) " + this.docCommentsTextBox.Text +
                      " (Balancing Leg for Receivables Doc:-" +
                      this.docIDNumTextBox.Text + ")", 0.00,
                      lnDte, this.curid,
                      glBatchID, funcCurrAmnt, netAmnt1,
               grndAmnt, int.Parse(this.invcCurrIDTextBox.Text), accntCurrAmnt,
               accntCurrID1, funcCurrRate1, accntCurrRate1, "C");
                }

                //Bad Debt Balancing Leg
                accntCurrID1 = int.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
           "accb.accb_chart_of_accnts", "accnt_id", "crncy_id", this.dfltBadDbtAcntID));

                slctdCurrID = this.invcCurrIDTextBox.Text;
                funcCurrRate1 = Math.Round(
            Global.get_LtstExchRate(int.Parse(slctdCurrID), this.curid, lnDte), 15);
                accntCurrRate1 = Math.Round(
                  Global.get_LtstExchRate(int.Parse(slctdCurrID), accntCurrID1, lnDte), 15);
                System.Windows.Forms.Application.DoEvents();

                grndAmnt = grndAmnt - ttlTaxes;

                funcCurrAmnt = Global.getRcvblsDocFuncAmnt(docHdrID) - ttlTaxes;// (funcCurrRate1 * grndAmnt);
                accntCurrAmnt = (accntCurrRate1 * grndAmnt);
                System.Windows.Forms.Application.DoEvents();

                netAmnt1 = (double)Global.dbtOrCrdtAccntMultiplier(this.dfltBadDbtAcntID,
           "I") * (double)funcCurrAmnt;

                if (Global.dbtOrCrdtAccnt(this.dfltBadDbtAcntID,
                  "I") == "Debit")
                {
                    Global.createTransaction(this.dfltBadDbtAcntID,
                      "(Declared Bad Debt) " + this.docCommentsTextBox.Text +
                      " (Balacing Leg Less Taxes for Receivables Doc:-" +
                      this.docIDNumTextBox.Text + ")", funcCurrAmnt,
                      lnDte, this.curid, glBatchID, 0.00,
                      netAmnt1, grndAmnt, int.Parse(this.invcCurrIDTextBox.Text),
                      accntCurrAmnt, accntCurrID1, funcCurrRate1, accntCurrRate1, "D");
                }
                else
                {
                    Global.createTransaction(this.dfltBadDbtAcntID,
                      "(Declared Bad Debt) " + this.docCommentsTextBox.Text +
                      " (Balancing Leg Less Taxes for Receivables Doc:-" +
                      this.docIDNumTextBox.Text + ")", 0.00,
                      lnDte, this.curid,
                      glBatchID, funcCurrAmnt, netAmnt1,
               grndAmnt, int.Parse(this.invcCurrIDTextBox.Text), accntCurrAmnt,
               accntCurrID1, funcCurrRate1, accntCurrRate1, "C");
                }

                if (Global.get_Batch_CrdtSum(glBatchID) == Global.get_Batch_DbtSum(glBatchID))
                {
                    Global.updtRcvblsDocBadDbtGLBatch(docHdrID, glBatchID);
                    //this.updateAppldPrepayHdrs();
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
                Global.mnFrm.cmCde.showMsg("Receivables Document Bad Debt Declaration Failed!", 0);
                return false;
            }
        }

        private void rejectDocButton_Click(object sender, EventArgs e)
        {
            //if (this.saveButton.Enabled == true
            //  || this.saveDtButton.Enabled == true)
            //{
            //  Global.mnFrm.cmCde.showMsg("Please Save the Document First!", 0);
            //  return;
            //}
            //if ((this.extAppDocIDTextBox.Text != "" && this.extAppDocIDTextBox.Text != "-1"))
            //{
            //  Global.mnFrm.cmCde.showMsg("Cannot Work on Documents Created from other Modules!", 0);
            //  return;
            //}
            //   if (Global.mnFrm.cmCde.showMsg("Are you sure you want to REJECT the selected Document?" +
            //"\r\nThis action cannot be undone!", 1) == DialogResult.No)
            //   {
            //     Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
            //     return;
            //   }
            this.rejectDocButton.Enabled = false;
            System.Windows.Forms.Application.DoEvents();
            /*bool isAnyRnng = true;
            int witcntr = 0;
            do
            {
                witcntr++;
                isAnyRnng = Global.isThereANActvActnPrcss("7", "10 second");//Invetory Import Process
                System.Windows.Forms.Application.DoEvents();
            }
            while (isAnyRnng == true);*/

            //Global.updtActnPrcss(7);//Invetory Import Process

            bool sccs = this.rvrsApprval(Global.mnFrm.cmCde.getFrmtdDB_Date_time(), this.docTypeComboBox.Text);
            if (sccs)
            {
                Global.updtSalesDocApprvl(long.Parse(this.docIDTextBox.Text), "Not Validated", "Approve");
            }
            this.rejectDocButton.Enabled = true;
            //System.Windows.Forms.Application.DoEvents();
            this.populateDet(long.Parse(this.docIDTextBox.Text));
            //this.rfrshDtButton_Click(this.rfrshDtButton, e);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if ((this.addRecsPF == false
              && this.addRecsSO == false
             && this.addRecsSI == false
              && this.addRecsIR == false
              && this.addRecsII == false
              && this.addRecsSR == false))
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }

            if (this.dfltRcvblAcntID <= 0
              || this.dfltLbltyAccnt <= 0
              || this.dfltInvAcntID <= 0
              || this.dfltCGSAcntID <= 0
              || this.dfltExpnsAcntID <= 0
              || this.dfltRvnuAcntID <= 0)
            {
                Global.mnFrm.cmCde.showMsg("You must first Setup all Default " +
                  "Accounts before Accounting can be Created!", 0);
                this.saveLabel.Visible = false;
                Cursor.Current = Cursors.Default;
                System.Windows.Forms.Application.DoEvents();
                return;
            }

            string dateStr = Global.mnFrm.cmCde.getFrmtdDB_Date_time();

            double invcAmnt = 20000;
            if (this.isPayTrnsValid(this.dfltRcvblAcntID, "I", invcAmnt, dateStr))
            {
            }
            else
            {
                this.rfrshButton_Click(this.rfrshButton, e);
                return;
            }

            this.clearDetInfo();
            this.clearLnsInfo();
            this.addRec = true;
            this.editRec = false;
            if (Global.mnFrm.cmCde.getEnbldPssblValID("YES", Global.mnFrm.cmCde.getLovID("Allow Dues on Invoices")) > 0
        && this.docTypeComboBox.Text == "Sales Invoice")
            {
                this.allowDuesCheckBox.Checked = true;
            }
            this.apprvlStatusTextBox.Text = "Not Validated";
            this.nxtApprvlStatusButton.Text = "Approve";
            this.nxtApprvlStatusButton.ImageKey = "tick_64.png";
            this.docDteTextBox.Text = DateTime.ParseExact(
         Global.mnFrm.cmCde.getDB_Date_time().Substring(0, 10), "yyyy-MM-dd",
         System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
            if (this.invcCurrTextBox.Text == "")
            {
                this.invcCurrTextBox.Text = this.curCode;
                this.invcCurrIDTextBox.Text = this.curid.ToString();
                string curnm = this.invcCurrTextBox.Text;
                this.itemsDataGridView.Columns[7].HeaderText = "Unit Price (" + curnm + ")";
                this.itemsDataGridView.Columns[8].HeaderText = "Amount (" + curnm + ")";
                this.smmryDataGridView.Columns[1].HeaderText = "Amount (" + curnm + ")";
            }
            long pymntID = Global.mnFrm.cmCde.getGnrlRecID("accb.accb_paymnt_mthds", "pymnt_mthd_name",
         "paymnt_mthd_id", "Customer Cash", Global.mnFrm.cmCde.Org_id);
            this.pymntMthdIDTextBox.Text = pymntID.ToString();
            this.pymntMthdTextBox.Text = "Customer Cash";

            this.prpareForDetEdit();
            this.lnkdEventComboBox.SelectedItem = "None";

            this.addPRFButton.Enabled = false;
            this.addSOButton.Enabled = false;
            this.addSIButton.Enabled = false;
            this.addIRButton.Enabled = false;
            this.addUIIButton.Enabled = false;
            this.addSRButton.Enabled = false;

            this.editButton.Enabled = false;
            this.editDtButton.Enabled = false;
            ToolStripButton mybtn = (ToolStripButton)sender;

            if (mybtn.Text.Contains("SI"))
            {
                this.docTypeComboBox.SelectedItem = "Sales Invoice";
                if (this.allowDuesCheckBox.Checked)
                {
                    this.payTermsTextBox.Text = Global.mnFrm.cmCde.getEnbldPssblValDesc("Sales Invoice - Dues",
                   Global.mnFrm.cmCde.getLovID("Default Document Notes"));
                }
                else
                {
                    this.payTermsTextBox.Text = Global.mnFrm.cmCde.getEnbldPssblValDesc("Sales Invoice",
                      Global.mnFrm.cmCde.getLovID("Default Document Notes"));
                }
            }
            else if (mybtn.Text.Contains("SR"))
            {
                this.docTypeComboBox.SelectedItem = "Sales Return";
            }
            else if (mybtn.Text.Contains("SO"))
            {
                this.docTypeComboBox.SelectedItem = "Sales Order";
                this.payTermsTextBox.Text = Global.mnFrm.cmCde.getEnbldPssblValDesc("Sales Invoice",
            Global.mnFrm.cmCde.getLovID("Default Document Notes"));
            }
            else if (mybtn.Text.Contains("IR"))
            {
                this.docTypeComboBox.SelectedItem = "Internal Item Request";
                this.payTermsTextBox.Text = Global.mnFrm.cmCde.getEnbldPssblValDesc("Internal Item Request",
            Global.mnFrm.cmCde.getLovID("Default Document Notes"));
            }
            else if (mybtn.Text.Contains("ISSUE"))
            {
                this.docTypeComboBox.SelectedItem = "Item Issue-Unbilled";
                this.payTermsTextBox.Text = Global.mnFrm.cmCde.getEnbldPssblValDesc("Item Issues",
             Global.mnFrm.cmCde.getLovID("Default Document Notes"));
            }
            else if (mybtn.Text.Contains("PRF"))
            {
                this.docTypeComboBox.SelectedItem = "Pro-Forma Invoice";
                this.payTermsTextBox.Text = Global.mnFrm.cmCde.getEnbldPssblValDesc("Sales Invoice",
             Global.mnFrm.cmCde.getLovID("Default Document Notes"));
            }
            this.addDtRec = true;
            this.editDtRec = false;
            //this.createSalesDocRows(1);
            this.prpareForLnsEdit();
            //this.addDtButton_Click(this.addDtButton, e);
            this.txtChngd = false;
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if ((this.editRecsPF == false
              && this.docTypeComboBox.Text == "Pro-Forma Invoice")
              || (this.editRecsSO == false
             && this.docTypeComboBox.Text == "Sales Order")
              || (this.editRecsSI == false
              && this.docTypeComboBox.Text == "Sales Invoice")
              || (this.editRecsIR == false
              && this.docTypeComboBox.Text == "Internal Item Request")
              || (this.editRecsII == false
              && this.docTypeComboBox.Text == "Item Issue-Unbilled")
              || (this.editRecsSR == false
              && this.docTypeComboBox.Text == "Sales Return"))
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
            if (this.apprvlStatusTextBox.Text == "Validated")
            {
                this.rejectDocButton_Click(this.rejectDocButton, e);
            }
            if (this.apprvlStatusTextBox.Text == "Approved"
              || this.apprvlStatusTextBox.Text == "Initiated"
               || this.apprvlStatusTextBox.Text == "Validated"
              || this.apprvlStatusTextBox.Text == "Cancelled" || this.apprvlStatusTextBox.Text == "Declared Bad Debt"
              || this.apprvlStatusTextBox.Text.Contains("Reviewed")
              || (this.extAppDocIDTextBox.Text != "" && this.extAppDocIDTextBox.Text != "-1"))
            {
                Global.mnFrm.cmCde.showMsg("Cannot EDIT Approved, Initiated, " +
                  "Reviewed, Validated and Cancelled Documents\r\n as well as Documents Created from other Modules!", 0);
                return;
            }
            this.addRec = false;
            this.editRec = true;
            this.prpareForDetEdit();
            this.editButton.Enabled = false;
            this.addPRFButton.Enabled = false;
            this.addSOButton.Enabled = false;
            this.addSIButton.Enabled = false;
            this.addIRButton.Enabled = false;
            this.addUIIButton.Enabled = false;
            this.addSRButton.Enabled = false;
            if (this.itemsDataGridView.Rows.Count > 0
              && this.editDtButton.Enabled == true)
            {
                //this.invcCurrButton.Enabled = false;
                //this.invcCurrTextBox.Enabled = false;
                this.editDtButton_Click(this.editDtButton, e);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (this.addRec == true)
            {
                if ((this.addRecsPF == false
                  && this.docTypeComboBox.Text == "Pro-Forma Invoice")
                  || (this.addRecsSO == false
                 && this.docTypeComboBox.Text == "Sales Order")
                  || (this.addRecsSI == false
                  && this.docTypeComboBox.Text == "Sales Invoice")
                  || (this.addRecsIR == false
                  && this.docTypeComboBox.Text == "Internal Item Request")
                  || (this.addRecsII == false
                  && this.docTypeComboBox.Text == "Item Issue-Unbilled")
                  || (this.addRecsSR == false
                  && this.docTypeComboBox.Text == "Sales Return"))
                {
                    Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                        " this action!\nContact your System Administrator!", 0);
                    return;
                }
            }
            else
            {
                if ((this.editRecsPF == false
                  && this.docTypeComboBox.Text == "Pro-Forma Invoice")
                  || (this.editRecsSO == false
                 && this.docTypeComboBox.Text == "Sales Order")
                  || (this.editRecsSI == false
                  && this.docTypeComboBox.Text == "Sales Invoice")
                  || (this.editRecsIR == false
                  && this.docTypeComboBox.Text == "Internal Item Request")
                  || (this.editRecsII == false
                  && this.docTypeComboBox.Text == "Item Issue-Unbilled")
                  || (this.editRecsSR == false
                  && this.docTypeComboBox.Text == "Sales Return"))
                {
                    Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                        " this action!\nContact your System Administrator!", 0);
                    return;
                }
            }
            if (!this.checkRqrmnts())
            {
                return;
            }
            if (this.addRec == true)
            {
                Global.createSalesDocHdr(Global.mnFrm.cmCde.Org_id, this.docIDNumTextBox.Text,
                  this.docCommentsTextBox.Text, this.docTypeComboBox.Text, this.docDteTextBox.Text
                  , this.payTermsTextBox.Text, int.Parse(this.cstmrIDTextBox.Text),
                  int.Parse(this.siteIDTextBox.Text), "Not Validated",
                  "Approve", long.Parse(this.srcDocIDTextBox.Text),
                  Global.get_DfltRcvblAcnt(Global.mnFrm.cmCde.Org_id),
                  int.Parse(this.pymntMthdIDTextBox.Text), int.Parse(this.invcCurrIDTextBox.Text),
                  (double)this.exchRateNumUpDwn.Value, -1, "",
                    this.autoBalscheckBox.Checked, long.Parse(this.rgstrIDTextBox.Text),
                    this.costCtgrTextBox.Text, this.allowDuesCheckBox.Checked, this.lnkdEventComboBox.Text);

                this.saveButton.Enabled = false;
                this.addRec = false;
                this.editRec = false;
                System.Windows.Forms.Application.DoEvents();
                this.docIDTextBox.Text = Global.mnFrm.cmCde.getGnrlRecID(
                  "scm.scm_sales_invc_hdr",
                  "invc_number", "invc_hdr_id",
                  this.docIDNumTextBox.Text, Global.mnFrm.cmCde.Org_id).ToString();

                string srcDocType = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr", "invc_hdr_id", "invc_type", long.Parse(this.srcDocIDTextBox.Text));
                this.checkNCreateRcvblsHdr(0, srcDocType, this.docTypeComboBox.Text);

                bool prv = this.obey_evnts;
                this.obey_evnts = false;
                ListViewItem nwItem = new ListViewItem(new string[] {
    "New",
    this.docIDNumTextBox.Text,
    this.docIDTextBox.Text,
    this.docTypeComboBox.Text});
                this.invcListView.Items.Insert(0, nwItem);
                for (int i = 0; i < this.invcListView.SelectedItems.Count; i++)
                {
                    this.invcListView.SelectedItems[i].Font = new Font("Tahoma", 8.25f, FontStyle.Regular);
                    this.invcListView.SelectedItems[i].Selected = false;
                }
                this.invcListView.Items[0].Selected = true;
                this.invcListView.Items[0].Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
                this.obey_evnts = prv;


                if (this.saveDtButton.Enabled == true)
                {
                    this.saveDtButton_Click(this.saveDtButton, e);
                }
                if (this.nxtApprvlStatusButton.Text == "Approve")
                {
                    this.saveButton.Enabled = true;
                    this.editRec = true;
                    this.prpareForDetEdit();
                    this.prpareForLnsEdit();
                }
                //this.loadPanel();
            }
            else if (this.editRec == true)
            {
                Global.updtSalesDocHdr(long.Parse(this.docIDTextBox.Text), this.docIDNumTextBox.Text,
                  this.docCommentsTextBox.Text, this.docTypeComboBox.Text, this.docDteTextBox.Text
                  , this.payTermsTextBox.Text, int.Parse(this.cstmrIDTextBox.Text),
                  int.Parse(this.siteIDTextBox.Text), "Not Validated",
                  "Approve", long.Parse(this.srcDocIDTextBox.Text),
                  int.Parse(this.pymntMthdIDTextBox.Text), int.Parse(this.invcCurrIDTextBox.Text),
                  (double)this.exchRateNumUpDwn.Value, -1, "",
                    this.autoBalscheckBox.Checked, long.Parse(this.rgstrIDTextBox.Text),
                    this.costCtgrTextBox.Text, this.allowDuesCheckBox.Checked, this.lnkdEventComboBox.Text);

                this.saveButton.Enabled = false;
                this.addRec = false;
                this.editRec = false;

                // System.Windows.Forms.Application.DoEvents();
                string srcDocType = Global.mnFrm.cmCde.getGnrlRecNm(
                  "scm.scm_sales_invc_hdr", "invc_hdr_id", "invc_type",
                  long.Parse(this.srcDocIDTextBox.Text));
                this.checkNCreateRcvblsHdr(0, srcDocType, this.docTypeComboBox.Text);

                if (this.saveDtButton.Enabled == true)
                {
                    this.saveDtButton_Click(this.saveDtButton, e);
                }
                if (this.nxtApprvlStatusButton.Text == "Approve")
                {
                    this.saveButton.Enabled = true;
                    this.editRec = true;
                    //this.loadPanel();
                }
            }
            this.docSaved = true;
        }

        private bool checkRqrmnts()
        {
            if (this.docIDNumTextBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Please enter a Document Number!", 0);
                return false;
            }
            long oldRecID = Global.mnFrm.cmCde.getGnrlRecID("scm.scm_sales_invc_hdr", "invc_number", "invc_hdr_id", this.docIDNumTextBox.Text,
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
            if (this.docTypeComboBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Document Type cannot be empty!", 0);
                return false;
            }

            if (this.docDteTextBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Document Date cannot be empty!", 0);
                return false;
            }
            if (this.docTypeComboBox.Text == "Item Issue-Unbilled"
              && this.docCommentsTextBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Document Description cannot be empty for Unbilled Item Issues!", 0);
                return false;
            }
            if ((this.srcDocIDTextBox.Text == "" || this.srcDocIDTextBox.Text == "-1")
              && this.docTypeComboBox.Text == "Sales Return")
            {
                Global.mnFrm.cmCde.showMsg("For a Sales Return Document the Source Document cannot be empty!", 0);
                return false;
            }
            return true;
        }

        private bool checkDtRqrmnts(int rwIdx)
        {
            if (this.itemsDataGridView.Rows[rwIdx].Cells[12].Value == null)
            {
                return false;
            }
            if (this.itemsDataGridView.Rows[rwIdx].Cells[12].Value.ToString() == "-1")
            {
                return false;
            }
            long itmID = -1;
            double qty = 0;
            int storeID = -1;
            long.TryParse(this.itemsDataGridView.Rows[rwIdx].Cells[12].Value.ToString(), out itmID);
            double.TryParse(this.itemsDataGridView.Rows[rwIdx].Cells[4].Value.ToString(), out qty);
            int.TryParse(this.itemsDataGridView.Rows[rwIdx].Cells[13].Value.ToString(), out storeID);
            string itmType = Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_itm_list", "item_id", "item_type", itmID);

            if (itmType != "Services")
            {
                string cnsgmntIDs = Global.getOldstItmCnsgmtsForStock(itmID, qty, storeID);
                if (this.itemsDataGridView.Rows[rwIdx].Cells[10].Value == null)
                {
                    this.itemsDataGridView.EndEdit();
                    System.Windows.Forms.Application.DoEvents();
                    this.itemsDataGridView.Rows[rwIdx].Cells[10].Value = cnsgmntIDs;
                }

                if (this.itemsDataGridView.Rows[rwIdx].Cells[10].Value.ToString() == "")
                {
                    this.itemsDataGridView.Rows[rwIdx].Cells[10].Value = cnsgmntIDs;
                    this.itemsDataGridView.EndEdit();
                    System.Windows.Forms.Application.DoEvents();
                }

                if (this.docTypeComboBox.Text != "Internal Item Request"
                  && this.docTypeComboBox.Text != "Pro-Forma Invoice")
                {
                    if (this.itemsDataGridView.Rows[rwIdx].Cells[10].Value == null)
                    {
                        MessageBox.Show("Please check Consignments on Row(" + (rwIdx + 1).ToString() + ")!");
                        return false;
                    }

                    if (this.itemsDataGridView.Rows[rwIdx].Cells[10].Value.ToString() == "")
                    {
                        MessageBox.Show("Please check Consignments on Row(" + (rwIdx + 1).ToString() + ")!");
                        return false;
                    }
                }

                if (this.itemsDataGridView.Rows[rwIdx].Cells[13].Value == null)
                {
                    MessageBox.Show("Please check Stores on Row(" + (rwIdx + 1).ToString() + ")!");
                    return false;
                }

                if (this.itemsDataGridView.Rows[rwIdx].Cells[13].Value.ToString() == "-1")
                {
                    MessageBox.Show("Please check Stores on Row(" + (rwIdx + 1).ToString() + ")!");
                    return false;
                }
            }

            if (this.docTypeComboBox.Text == "Sales Return")
            {
                if (this.itemsDataGridView.Rows[rwIdx].Cells[26].Value == null)
                {
                    return false;
                }
                if (this.itemsDataGridView.Rows[rwIdx].Cells[26].Value.ToString().Trim() == "")
                {
                    return false;
                }
            }
            if (this.itemsDataGridView.Rows[rwIdx].Cells[4].Value == null)
            {
                return false;
            }
            if (this.itemsDataGridView.Rows[rwIdx].Cells[7].Value == null)
            {
                return false;
            }
            double tst = 0;
            double.TryParse(this.itemsDataGridView.Rows[rwIdx].Cells[4].Value.ToString(), out tst);
            if (tst <= 0)
            {
                return false;
            }
            tst = 0;
            double.TryParse(this.itemsDataGridView.Rows[rwIdx].Cells[7].Value.ToString(), out tst);
            if (tst <= 0)
            {
                return false;
            }
            long prsn_id = -1;
            long.TryParse(this.itemsDataGridView.Rows[rwIdx].Cells[28].Value.ToString(), out prsn_id);
            if (this.allowDuesCheckBox.Checked)
            {
                if (prsn_id <= 0)
                {
                    long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
            "scm.scm_cstmr_suplr", "cust_sup_id", "lnkd_prsn_id", long.Parse(this.cstmrIDTextBox.Text)), out prsn_id);
                }
                this.itemsDataGridView.Rows[rwIdx].Cells[27].Value = Global.mnFrm.cmCde.getPrsnSurNameFrst(prsn_id);
                this.itemsDataGridView.Rows[rwIdx].Cells[28].Value = prsn_id;
                long pay_itm_id = -1;
                long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
          "org.org_pay_items", "inv_item_id", "item_id", itmID), out pay_itm_id);
                if (pay_itm_id > 0 && prsn_id <= 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void delButton_Click(object sender, EventArgs e)
        {
            if ((this.delRecsPF == false
              && this.docTypeComboBox.Text == "Pro-Forma Invoice")
              || (this.delRecsSO == false
             && this.docTypeComboBox.Text == "Sales Order")
              || (this.delRecsSI == false
              && this.docTypeComboBox.Text == "Sales Invoice")
              || (this.delRecsIR == false
              && this.docTypeComboBox.Text == "Internal Item Request")
              || (this.delRecsII == false
              && this.docTypeComboBox.Text == "Item Issue-Unbilled")
              || (this.delRecsSR == false
              && this.docTypeComboBox.Text == "Sales Return"))
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }
            if (this.invcListView.Items.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select the Record to Delete!", 0);
                return;
            }
            if (this.apprvlStatusTextBox.Text == "Approved"
              || this.apprvlStatusTextBox.Text == "Initiated"
              || this.apprvlStatusTextBox.Text == "Validated"
              || this.apprvlStatusTextBox.Text == "Cancelled"
              || this.apprvlStatusTextBox.Text == "Declared Bad Debt"
              || this.apprvlStatusTextBox.Text.Contains("Reviewed")
              || (this.extAppDocIDTextBox.Text != "" && this.extAppDocIDTextBox.Text != "-1"))
            {
                Global.mnFrm.cmCde.showMsg("Cannot DELETE Approved, Initiated, " +
                  "Reviewed, Validated and Cancelled Documents\r\n as well as Documents Created from other Modules!", 0);
                return;
            }
            if (Global.mnFrm.cmCde.showMsg("Are you sure you want to DELETE the selected Document?" +
           "\r\nThis action cannot be undone!", 1) == DialogResult.No)
            {
                //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                return;
            }
            long rcvblHdrID = Global.get_ScmRcvblsDocHdrID(long.Parse(this.docIDTextBox.Text),
          this.docTypeComboBox.Text, Global.mnFrm.cmCde.Org_id);
            string rcvblDocNum = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
             "rcvbls_invc_hdr_id", "rcvbls_invc_number", rcvblHdrID);

            Global.deleteRcvblsDocHdrNDet(rcvblHdrID, rcvblDocNum);
            Global.deleteSalesDoc(long.Parse(this.docIDTextBox.Text));
            Global.deleteDocSmmryItms(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
            Global.deleteScmRcvblsDocDet(long.Parse(this.docIDTextBox.Text));
            Global.deleteDocGLInfcLns(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
            this.rfrshButton_Click(this.rfrshButton, e);
        }

        private void vwSQLButton_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.showSQL(this.rec_SQL, 31);
        }

        private void rcHstryButton_Click(object sender, EventArgs e)
        {
            if (this.invcListView.SelectedItems.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select a Record First!", 0);
                return;
            }
            Global.mnFrm.cmCde.showRecHstry(
              Global.mnFrm.cmCde.get_Gnrl_Rec_Hstry(long.Parse(
              this.invcListView.SelectedItems[0].SubItems[2].Text),
              "scm.scm_sales_invc_hdr", "invc_hdr_id"), 32);
        }

        private void invcListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.shdObeyEvts() == false)
            {
                return;
            }
            if (this.invcListView.SelectedItems.Count == 1)
            {
                this.populateDet(long.Parse(this.invcListView.SelectedItems[0].SubItems[2].Text));
                this.changeGridVw();
                this.populateLines(long.Parse(this.invcListView.SelectedItems[0].SubItems[2].Text),
                    this.invcListView.SelectedItems[0].SubItems[3].Text);
                this.populateSmmry(long.Parse(this.invcListView.SelectedItems[0].SubItems[2].Text),
                  this.invcListView.SelectedItems[0].SubItems[3].Text);
            }
            //else
            //{
            //  this.clearDetInfo();
            //  this.clearLnsInfo();
            //  this.smmryDataGridView.Rows.Clear();
            //  //this.disableDetEdit();
            //  //this.disableLnsEdit();
            //  //this.populateDet(-100000);
            //  //this.populateLines(-100000, "");
            //  //this.populateSmmry(-100000, "");
            //}
        }

        private void invcListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                e.Item.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            }
            else
            {
                e.Item.Font = new Font("Tahoma", 8.25f, FontStyle.Regular);
            }
        }

        private void addDtButton_Click(object sender, EventArgs e)
        {
            if ((this.editRecsPF == false
              && this.docTypeComboBox.Text == "Pro-Forma Invoice")
              || (this.editRecsSO == false
             && this.docTypeComboBox.Text == "Sales Order")
              || (this.editRecsSI == false
              && this.docTypeComboBox.Text == "Sales Invoice")
              || (this.editRecsIR == false
              && this.docTypeComboBox.Text == "Internal Item Request")
              || (this.editRecsII == false
              && this.docTypeComboBox.Text == "Item Issue-Unbilled")
              || (this.editRecsSR == false
              && this.docTypeComboBox.Text == "Sales Return"))
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
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
              || this.apprvlStatusTextBox.Text == "Cancelled" || this.apprvlStatusTextBox.Text == "Declared Bad Debt"
              || this.apprvlStatusTextBox.Text.Contains("Reviewed")
              || (this.extAppDocIDTextBox.Text != "" && this.extAppDocIDTextBox.Text != "-1"))
            {
                Global.mnFrm.cmCde.showMsg("Cannot EDIT Approved, Initiated, " +
                  "Reviewed, Validated and Cancelled Documents\r\n as well as Documents Created from other Modules!", 0);
                return;
            }
            this.addDtRec = true;
            this.editDtRec = true;
            this.createSalesDocRows(1);
            this.prpareForLnsEdit();
        }

        public void createSalesDocRows(int num)
        {
            bool prv = this.obey_evnts;
            this.obey_evnts = false;
            string curid = this.invcCurrIDTextBox.Text;//Global.mnFrm.cmCde.getOrgFuncCurID(Global.mnFrm.cmCde.Org_id).ToString();
            int rowIdx = 0;
            for (int i = 0; i < num; i++)
            {
                this.itemsDataGridView.RowCount += 1;
                rowIdx = this.itemsDataGridView.RowCount - 1;
                this.itemsDataGridView.Rows[rowIdx].Cells[0].Value = "";
                this.itemsDataGridView.Rows[rowIdx].Cells[1].Value = "...";
                this.itemsDataGridView.Rows[rowIdx].Cells[2].Value = "";
                this.itemsDataGridView.Rows[rowIdx].Cells[3].Value = "...";
                this.itemsDataGridView.Rows[rowIdx].Cells[4].Value = "0.00";
                this.itemsDataGridView.Rows[rowIdx].Cells[5].Value = "Pcs";
                this.itemsDataGridView.Rows[rowIdx].Cells[6].Value = "...";
                this.itemsDataGridView.Rows[rowIdx].Cells[7].Value = "0.00";
                this.itemsDataGridView.Rows[rowIdx].Cells[8].Value = "0.00";
                this.itemsDataGridView.Rows[rowIdx].Cells[9].Value = "0.00";
                this.itemsDataGridView.Rows[rowIdx].Cells[10].Value = "";
                this.itemsDataGridView.Rows[rowIdx].Cells[11].Value = "...";
                this.itemsDataGridView.Rows[rowIdx].Cells[12].Value = "-1";
                this.itemsDataGridView.Rows[rowIdx].Cells[13].Value = "-1";
                this.itemsDataGridView.Rows[rowIdx].Cells[14].Value = curid;
                this.itemsDataGridView.Rows[rowIdx].Cells[15].Value = "-1";
                this.itemsDataGridView.Rows[rowIdx].Cells[16].Value = "-1";
                this.itemsDataGridView.Rows[rowIdx].Cells[17].Value = "";
                this.itemsDataGridView.Rows[rowIdx].Cells[18].Value = "...";
                this.itemsDataGridView.Rows[rowIdx].Cells[19].Value = "-1";
                this.itemsDataGridView.Rows[rowIdx].Cells[20].Value = "";
                this.itemsDataGridView.Rows[rowIdx].Cells[21].Value = "...";
                this.itemsDataGridView.Rows[rowIdx].Cells[22].Value = "-1";
                this.itemsDataGridView.Rows[rowIdx].Cells[23].Value = "";
                this.itemsDataGridView.Rows[rowIdx].Cells[24].Value = "...";
                this.itemsDataGridView.Rows[rowIdx].Cells[25].Value = "-1";
                this.itemsDataGridView.Rows[rowIdx].Cells[26].Value = "";
                this.itemsDataGridView.Rows[rowIdx].Cells[27].Value = "";
                this.itemsDataGridView.Rows[rowIdx].Cells[28].Value = "-1";
                this.itemsDataGridView.Rows[rowIdx].Cells[29].Value = "...";
                this.itemsDataGridView.Rows[rowIdx].Cells[30].Value = "Linked Person";
                this.itemsDataGridView.Rows[rowIdx].Cells[31].Value = "";
                this.itemsDataGridView.Rows[rowIdx].Cells[32].Value = "Change Accounts";
                this.itemsDataGridView.Rows[rowIdx].Cells[33].Value = "-1,-1,-1,-1,-1";
            }
            this.obey_evnts = prv;
            this.itemsDataGridView.ClearSelection();
            this.itemsDataGridView.Focus();
            //System.Windows.Forms.Application.DoEvents();
            this.itemsDataGridView.CurrentCell = this.itemsDataGridView.Rows[rowIdx].Cells[0];
            //System.Windows.Forms.Application.DoEvents();
            this.itemsDataGridView.BeginEdit(true);
            //System.Windows.Forms.Application.DoEvents();
            //SendKeys.Send("{TAB}");
            SendKeys.Send("{HOME}");

            //this.itemsDataGridView.CurrentCell = this.itemsDataGridView.Rows[rowIdx].Cells[0];
            //System.Windows.Forms.Application.DoEvents();
            //this.itemsDataGridView.BeginEdit(true);

        }

        private void editDtButton_Click(object sender, EventArgs e)
        {
            if ((this.editRecsPF == false
              && this.docTypeComboBox.Text == "Pro-Forma Invoice")
              || (this.editRecsSO == false
             && this.docTypeComboBox.Text == "Sales Order")
              || (this.editRecsSI == false
              && this.docTypeComboBox.Text == "Sales Invoice")
              || (this.editRecsIR == false
              && this.docTypeComboBox.Text == "Internal Item Request")
              || (this.editRecsII == false
              && this.docTypeComboBox.Text == "Item Issue-Unbilled")
              || (this.editRecsSR == false
              && this.docTypeComboBox.Text == "Sales Return"))
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }
            if (this.itemsDataGridView.RowCount <= 0)
            {
                Global.mnFrm.cmCde.showMsg("No record to Edit!", 0);
                return;
            }
            if (this.apprvlStatusTextBox.Text == "Approved"
               || this.apprvlStatusTextBox.Text == "Initiated"
                || this.apprvlStatusTextBox.Text == "Validated"
               || this.apprvlStatusTextBox.Text == "Cancelled" || this.apprvlStatusTextBox.Text == "Declared Bad Debt"
               || this.apprvlStatusTextBox.Text.Contains("Reviewed")
               || (this.extAppDocIDTextBox.Text != "" && this.extAppDocIDTextBox.Text != "-1"))
            {
                Global.mnFrm.cmCde.showMsg("Cannot EDIT Approved, Initiated, " +
                  "Reviewed, Validated and Cancelled Documents\r\n as well as Documents Created from other Modules!", 0);
                return;
            }
            this.addDtRec = false;
            this.editDtRec = true;
            this.prpareForLnsEdit();
            if (this.itemsDataGridView.Rows.Count > 0
         && this.editButton.Enabled == true)
            {
                this.editButton_Click(this.editButton, e);
            }
        }

        private void vwSQLDtButton_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.showSQL(this.recDt_SQL, 31);
        }

        private void rcHstryDtButton_Click(object sender, EventArgs e)
        {
            if (this.itemsDataGridView.CurrentCell != null
        && this.itemsDataGridView.SelectedRows.Count <= 0)
            {
                this.itemsDataGridView.Rows[this.itemsDataGridView.CurrentCell.RowIndex].Selected = true;
            }

            if (this.itemsDataGridView.SelectedRows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select a Record First!", 0);
                return;
            }
            Global.mnFrm.cmCde.showRecHstry(
              Global.mnFrm.cmCde.get_Gnrl_Rec_Hstry(long.Parse(
              this.itemsDataGridView.SelectedRows[0].Cells[13].Value.ToString()),
              "scm.scm_sales_invc_det", "invc_det_ln_id"), 32);
        }

        private void rcHstrySmryButton_Click(object sender, EventArgs e)
        {
            if (this.smmryDataGridView.CurrentCell != null
        && this.smmryDataGridView.SelectedRows.Count <= 0)
            {
                this.smmryDataGridView.Rows[this.smmryDataGridView.CurrentCell.RowIndex].Selected = true;
            }
            if (this.smmryDataGridView.SelectedRows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select a Record First!", 0);
                return;
            }
            Global.mnFrm.cmCde.showRecHstry(
              Global.mnFrm.cmCde.get_Gnrl_Rec_Hstry(long.Parse(
              this.smmryDataGridView.SelectedRows[0].Cells[2].Value.ToString()),
              "scm.scm_doc_amnt_smmrys", "smmry_id"), 32);
        }

        private void vwSmrySQLButton_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.showSQL(this.smmry_SQL, 31);
        }

        private void calcSmryButton_Click(object sender, EventArgs e)
        {
            if (this.docIDTextBox.Text != "" && this.docIDTextBox.Text != "-1")
            {
                this.reCalcSmmrys(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text,
                  int.Parse(this.cstmrIDTextBox.Text), int.Parse(this.invcCurrIDTextBox.Text), this.apprvlStatusTextBox.Text);
                this.populateSmmry(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
            }
            else
            {
                //this.populateSmmry(-1000, "");
                this.sumGridAmounts();
            }
        }

        public void reCalcSmmrys(long srcDocID, string srcDocType, int cstmrID, int invCurID, string docStatus)
        {
            long rcvblHdrID = Global.get_ScmRcvblsDocHdrID(srcDocID, srcDocType, Global.mnFrm.cmCde.Org_id);
            string rcvblDoctype = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
              "rcvbls_invc_hdr_id", "rcvbls_invc_type", rcvblHdrID);

            /*if (docStatus == "Approved" && Global.mnFrm.cmCde.doesDteTmeExceedIntrvl(Global.getRcvblsDocLastUpdate(rcvblHdrID, rcvblDoctype), "1 day"))
            {
                return;
            }*/
            DataSet dtst = Global.get_One_SalesDcLines(srcDocID);
            double grndAmnt = Global.getSalesDocGrndAmnt(srcDocID);
            // Grand Total
            string smmryNm = "Grand Total";
            long smmryID = Global.getSalesSmmryItmID("5Grand Total", -1,
              srcDocID, srcDocType);
            if (smmryID <= 0)
            {
                Global.createSmmryItm("5Grand Total", smmryNm, grndAmnt, -1,
                  srcDocType, srcDocID, true);
            }
            else
            {
                Global.updateSmmryItm(smmryID, "5Grand Total", grndAmnt, true, smmryNm);
            }

            //Total Payments
            double blsAmnt = 0;
            double pymntsAmnt = 0;
            long SIDocID = -1;
            long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr",
               "invc_hdr_id", "src_doc_hdr_id", srcDocID), out SIDocID);
            string strSrcDocType = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr",
              "invc_hdr_id", "invc_type", SIDocID);

            if (srcDocType == "Sales Invoice")
            {

                pymntsAmnt = Global.getRcvblsDocTtlPymnts(rcvblHdrID, rcvblDoctype);
                //pymntsAmnt = Global.getSalesDocRcvdPymnts(srcDocID, srcDocType);
                smmryNm = "Total Payments Received";
                smmryID = Global.getSalesSmmryItmID("6Total Payments Received", -1,
                  srcDocID, srcDocType);
                if (smmryID <= 0)
                {
                    Global.createSmmryItm("6Total Payments Received", smmryNm, pymntsAmnt, -1,
                      srcDocType, srcDocID, true);
                }
                else
                {
                    Global.updateSmmryItm(smmryID, "6Total Payments Received", pymntsAmnt, true, smmryNm);
                }
            }
            else if (srcDocType == "Sales Return" && strSrcDocType == "Sales Invoice")
            {
                pymntsAmnt = Global.getRcvblsDocTtlPymnts(rcvblHdrID, rcvblDoctype);
                //pymntsAmnt = Global.getSalesDocRcvdPymnts(srcDocID, srcDocType);
                smmryNm = "Total Amount Refunded";
                smmryID = Global.getSalesSmmryItmID("6Total Payments Received", -1,
                  srcDocID, srcDocType);
                if (smmryID <= 0)
                {
                    Global.createSmmryItm("6Total Payments Received", smmryNm, pymntsAmnt, -1,
                      srcDocType, srcDocID, true);
                }
                else
                {
                    Global.updateSmmryItm(smmryID, "6Total Payments Received", pymntsAmnt, true, smmryNm);
                }
            }
            int codeCntr = 0;
            //Tax Codes
            double txAmnts = 0;
            double dscntAmnts = 0;
            double extrChrgAmnts = 0;

            double txAmnts1 = 0;
            double dscntAmnts1 = 0;
            double extrChrgAmnts1 = 0;

            //string txSmmryNm = "";
            //string dscntSmmryNm = "";
            //string chrgSmmryNm = "";
            char[] w = { ',' };
            Global.updateResetSmmryItm(srcDocID, srcDocType);
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                int txID = int.Parse(dtst.Tables[0].Rows[i][9].ToString());
                int dscntID = int.Parse(dtst.Tables[0].Rows[i][10].ToString());
                int chrgID = int.Parse(dtst.Tables[0].Rows[i][11].ToString());
                double unitAmnt = double.Parse(dtst.Tables[0].Rows[i][14].ToString());
                double qnty = double.Parse(dtst.Tables[0].Rows[i][2].ToString());
                string tmp = "";
                double snglDscnt = 0;
                if (dscntID > 0)
                {
                    string isParnt = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "is_parent", dscntID);
                    if (isParnt == "1")
                    {
                        string[] codeIDs = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "child_code_ids", dscntID).Split(w, StringSplitOptions.RemoveEmptyEntries);
                        snglDscnt = 0;
                        for (int j = 0; j < codeIDs.Length; j++)
                        {
                            if (int.Parse(codeIDs[j]) > 0)
                            {
                                snglDscnt += this.getDscntLessTax(txID, Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), unitAmnt, 1));
                                dscntAmnts1 = this.getDscntLessTax(txID, Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), unitAmnt, qnty));
                                dscntAmnts += dscntAmnts1;
                                tmp = Global.mnFrm.cmCde.getGnrlRecNm(
                           "scm.scm_tax_codes", "code_id", "code_name", int.Parse(codeIDs[j]));
                                smmryID = Global.getSalesSmmryItmID("3Discount", int.Parse(codeIDs[j]),
                       srcDocID, srcDocType);
                                if (smmryID <= 0 && dscntAmnts1 > 0)
                                {
                                    Global.createSmmryItm("3Discount", tmp, dscntAmnts1, int.Parse(codeIDs[j]), srcDocType, srcDocID, true);
                                }
                                else if (dscntAmnts1 > 0)
                                {
                                    Global.updateSmmryItmAddOn(smmryID, "3Discount", dscntAmnts1, true, tmp);
                                }
                                codeCntr++;
                            }
                        }
                    }
                    else
                    {
                        snglDscnt = this.getDscntLessTax(txID, Global.getSalesDocCodesAmnt(dscntID, unitAmnt, 1));
                        dscntAmnts1 = this.getDscntLessTax(txID, Global.getSalesDocCodesAmnt(dscntID, unitAmnt, qnty));
                        dscntAmnts += dscntAmnts1;
                        tmp = Global.mnFrm.cmCde.getGnrlRecNm(
                   "scm.scm_tax_codes", "code_id", "code_name", dscntID);
                        smmryID = Global.getSalesSmmryItmID("3Discount", dscntID,
               srcDocID, srcDocType);
                        if (smmryID <= 0 && dscntAmnts1 > 0)
                        {
                            Global.createSmmryItm("3Discount", tmp, dscntAmnts1, dscntID, srcDocType, srcDocID, true);
                        }
                        else if (dscntAmnts1 > 0)
                        {
                            Global.updateSmmryItmAddOn(smmryID, "3Discount", dscntAmnts1, true, tmp);
                        }
                        codeCntr++;
                    }
                    //codeCntr++;
                }

                if (txID > 0)
                {
                    string isParnt = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "is_parent", txID);
                    if (isParnt == "1")
                    {
                        string[] codeIDs = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "child_code_ids", txID).Split(w, StringSplitOptions.RemoveEmptyEntries);
                        //snglDscnt = 0;
                        for (int j = 0; j < codeIDs.Length; j++)
                        {
                            if (int.Parse(codeIDs[j]) > 0)
                            {
                                txAmnts1 = Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), unitAmnt - snglDscnt, qnty);
                                txAmnts += txAmnts1;
                                tmp = Global.mnFrm.cmCde.getGnrlRecNm(
                           "scm.scm_tax_codes", "code_id", "code_name", int.Parse(codeIDs[j]));
                                smmryID = Global.getSalesSmmryItmID("2Tax", int.Parse(codeIDs[j]),
                       srcDocID, srcDocType);
                                if (smmryID <= 0 && txAmnts1 > 0)
                                {
                                    Global.createSmmryItm("2Tax", tmp, txAmnts1, int.Parse(codeIDs[j]),
                                      srcDocType, srcDocID, true);
                                }
                                else if (txAmnts1 > 0)
                                {
                                    Global.updateSmmryItmAddOn(smmryID, "2Tax", txAmnts1, true, tmp);
                                }
                                codeCntr++;
                            }
                        }
                    }
                    else
                    {
                        txAmnts1 = Global.getSalesDocCodesAmnt(txID, unitAmnt - snglDscnt, qnty);
                        txAmnts += txAmnts1;
                        tmp = Global.mnFrm.cmCde.getGnrlRecNm(
                    "scm.scm_tax_codes", "code_id", "code_name", txID);

                        smmryID = Global.getSalesSmmryItmID("2Tax", txID,
                       srcDocID, srcDocType);
                        if (smmryID <= 0 && txAmnts1 > 0)
                        {
                            Global.createSmmryItm("2Tax", tmp, txAmnts1, txID,
                              srcDocType, srcDocID, true);
                        }
                        else if (txAmnts1 > 0)
                        {
                            Global.updateSmmryItmAddOn(smmryID, "2Tax", txAmnts1, true, tmp);
                        }
                        codeCntr++;
                    }
                }

                if (chrgID > 0)
                {
                    string isParnt = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "is_parent", chrgID);
                    if (isParnt == "1")
                    {
                        string[] codeIDs = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "child_code_ids", chrgID).Split(w, StringSplitOptions.RemoveEmptyEntries);
                        //snglDscnt = 0;
                        for (int j = 0; j < codeIDs.Length; j++)
                        {
                            if (int.Parse(codeIDs[j]) > 0)
                            {
                                extrChrgAmnts1 = Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), unitAmnt, qnty);
                                extrChrgAmnts += extrChrgAmnts1;
                                tmp = Global.mnFrm.cmCde.getGnrlRecNm(
                           "scm.scm_tax_codes", "code_id", "code_name", int.Parse(codeIDs[j]));
                                smmryID = Global.getSalesSmmryItmID("4Extra Charge", int.Parse(codeIDs[j]),
                       srcDocID, srcDocType);
                                if (smmryID <= 0 && extrChrgAmnts1 > 0)
                                {
                                    Global.createSmmryItm("4Extra Charge", tmp, extrChrgAmnts1, int.Parse(codeIDs[j]),
                                      srcDocType, srcDocID, true);
                                }
                                else if (extrChrgAmnts1 > 0)
                                {
                                    Global.updateSmmryItmAddOn(smmryID, "4Extra Charge", extrChrgAmnts1, true, tmp);
                                }
                                codeCntr++;
                            }
                        }
                    }
                    else
                    {
                        extrChrgAmnts1 = Global.getSalesDocCodesAmnt(chrgID, unitAmnt, qnty);
                        extrChrgAmnts += extrChrgAmnts1;
                        tmp = Global.mnFrm.cmCde.getGnrlRecNm(
                   "scm.scm_tax_codes", "code_id", "code_name", chrgID);

                        smmryID = Global.getSalesSmmryItmID("4Extra Charge", chrgID,
                       srcDocID, srcDocType);
                        if (smmryID <= 0 && extrChrgAmnts1 > 0)
                        {
                            Global.createSmmryItm("4Extra Charge", tmp, extrChrgAmnts1, chrgID,
                              srcDocType, srcDocID, true);
                        }
                        else if (extrChrgAmnts1 > 0)
                        {
                            Global.updateSmmryItmAddOn(smmryID, "4Extra Charge", extrChrgAmnts1, true, tmp);
                        }
                        codeCntr++;
                    }
                }
            }
            //char[] trm = { '+' };
            //txSmmryNm = txSmmryNm.Trim().Trim(trm).Trim();
            //dscntSmmryNm = dscntSmmryNm.Trim().Trim(trm).Trim();
            //chrgSmmryNm = chrgSmmryNm.Trim().Trim(trm).Trim();

            if (txAmnts <= 0)
            {
                Global.deleteSalesSmmryItm(srcDocID, srcDocType, "2Tax");
            }

            if (dscntAmnts <= 0)
            {
                Global.deleteSalesSmmryItm(srcDocID, srcDocType, "3Discount");
            }

            if (extrChrgAmnts <= 0)
            {
                Global.deleteSalesSmmryItm(srcDocID, srcDocType, "4Extra Charge");
            }
            Global.deleteZeroSmmryItms(srcDocID, srcDocType);
            //Initial Amount
            double initAmnt = 0;
            if (txAmnts <= 0 && dscntAmnts <= 0 && extrChrgAmnts <= 0)
            {
                Global.deleteSalesSmmryItm(srcDocID, srcDocType, "1Initial Amount");
            }
            else if (codeCntr > 0)
            {
                smmryNm = "Initial Amount";
                smmryID = Global.getSalesSmmryItmID("1Initial Amount", -1,
                  srcDocID, srcDocType);
                initAmnt = grndAmnt;
                if (smmryID <= 0)
                {
                    Global.createSmmryItm("1Initial Amount", smmryNm, initAmnt, -1,
                      srcDocType, srcDocID, true);
                }
                else
                {
                    Global.updateSmmryItm(smmryID, "1Initial Amount", initAmnt, true, smmryNm);
                }
            }

            // Grand Total
            grndAmnt = grndAmnt + txAmnts + extrChrgAmnts - dscntAmnts;
            smmryNm = "Grand Total";
            smmryID = Global.getSalesSmmryItmID("5Grand Total", -1,
              srcDocID, srcDocType);
            if (smmryID <= 0)
            {
                Global.createSmmryItm("5Grand Total", smmryNm, grndAmnt, -1,
                  srcDocType, srcDocID, true);
            }
            else
            {
                Global.updateSmmryItm(smmryID, "5Grand Total", grndAmnt, true, smmryNm);
            }
            //Total Payments     
            if (srcDocType == "Sales Invoice")
            {
                //Change Given/Outstanding Balance
                blsAmnt = grndAmnt - pymntsAmnt;
                if (Math.Round(blsAmnt, 2) >= 0.00)
                {
                    smmryNm = "Outstanding Balance";
                }
                else
                {
                    smmryNm = "Change Given to Customer";
                }
                smmryID = Global.getSalesSmmryItmID("7Change/Balance", -1,
                  srcDocID, srcDocType);
                if (smmryID <= 0)
                {
                    Global.createSmmryItm("7Change/Balance", smmryNm, blsAmnt, -1,
                      srcDocType, srcDocID, true);
                }
                else
                {
                    Global.updateSmmryItm(smmryID, "7Change/Balance", blsAmnt, true, smmryNm);
                }
                //Customer's Total Deposits
                double ttlDpsts = Global.getCstmrDpsts(cstmrID, invCurID);
                smmryNm = "Total Deposits";
                smmryID = Global.getSalesSmmryItmID("8Deposits", -1,
                  srcDocID, srcDocType);
                if (smmryID <= 0)
                {
                    Global.createSmmryItm("8Deposits", smmryNm, ttlDpsts, -1,
                      srcDocType, srcDocID, true);
                }
                else
                {
                    Global.updateSmmryItm(smmryID, "8Deposits", ttlDpsts, true, smmryNm);
                }

                //Actual Change or Balance
                double actlblsAmnt = blsAmnt - ttlDpsts;
                if (Math.Round(actlblsAmnt, 2) >= 0.00)
                {
                    smmryNm = "Actual Outstanding Balance";
                }
                else
                {
                    smmryNm = "Amount to be Refunded to Customer";
                }
                smmryID = Global.getSalesSmmryItmID("9Actual_Change/Balance", -1,
                  srcDocID, srcDocType);
                if (smmryID <= 0)
                {
                    Global.createSmmryItm("9Actual_Change/Balance", smmryNm, actlblsAmnt, -1,
                      srcDocType, srcDocID, true);
                }
                else
                {
                    Global.updateSmmryItm(smmryID, "9Actual_Change/Balance", actlblsAmnt, true, smmryNm);
                }
            }
            else if (srcDocType == "Sales Return" && strSrcDocType == "Sales Invoice")
            {
                //Change Given/Outstanding Balance
                blsAmnt = grndAmnt - pymntsAmnt;
                if (Math.Round(blsAmnt, 2) >= 0.00)
                {
                    smmryNm = "Outstanding Balance";
                }
                else
                {
                    smmryNm = "Change Received from Customer";
                }
                smmryID = Global.getSalesSmmryItmID("7Change/Balance", -1,
                  srcDocID, srcDocType);
                if (smmryID <= 0)
                {
                    Global.createSmmryItm("7Change/Balance", smmryNm, blsAmnt, -1,
                      srcDocType, srcDocID, true);
                }
                else
                {
                    Global.updateSmmryItm(smmryID, "7Change/Balance", blsAmnt, true, smmryNm);
                }
            }
            Global.roundSmmryItms(srcDocID, srcDocType);
            if (this.autoBalscheckBox.Checked)
            {
                this.autoBals(this.docTypeComboBox.Text);
            }

        }

        private double getDscntLessTax(int txID, double orgnlDscnt)
        {
            char[] w = { ',' };
            double txAmnts = 0;
            double txAmnts1 = 0;
            if (txID > 0)
            {
                string isParnt = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "is_parent", txID);
                if (isParnt == "1")
                {
                    string[] codeIDs = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "child_code_ids", txID).Split(w, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < codeIDs.Length; j++)
                    {
                        if (int.Parse(codeIDs[j]) > 0)
                        {
                            txAmnts1 += Global.getSalesDocCodesAmnt(int.Parse(codeIDs[j]), 1, 1);
                        }
                    }
                    txAmnts1 = orgnlDscnt / (1.0 + txAmnts1);
                    txAmnts += txAmnts1;
                }
                else
                {
                    txAmnts1 = Global.getSalesDocCodesAmnt(txID, 1, 1);
                    txAmnts1 = orgnlDscnt / (1.0 + txAmnts1);
                    txAmnts += txAmnts1;
                }
            }
            else
            {
                txAmnts = orgnlDscnt;
            }
            return txAmnts;
        }

        private void autoBals(string srcDocType)
        {
            return;
            //DataSet dtst = Global.get_DocSmryLns(docHdrID, docTyp);
            //for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            //{

            //}
            long srcDocID = long.Parse(this.docIDTextBox.Text);
            /*,
              int.Parse(this.sponsorIDTextBox.Text), int.Parse(this.invcCurrIDTextBox.Text)*/
            if (this.editRecsSI == false)
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }
            if (this.docIDTextBox.Text == "" ||
              this.docIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("Please select a Document First!", 0);
                return;
            }
            //string[] selVals = new string[1];
            //for (int i = 0; i < this.smmryDataGridView.Rows.Count; i++)
            //{
            //  if (this.smmryDataGridView.Rows[i].Cells[5].Value.ToString() == "4Extra Charge")
            //  {
            //    selVals[0] = this.smmryDataGridView.Rows[i].Cells[3].Value.ToString();
            //  }
            //}
            DialogResult dgRes = DialogResult.OK; /*Global.mnFrm.cmCde.showPssblValDiag(
          Global.mnFrm.cmCde.getLovID("Extra Charges"), ref selVals,
          true, false, Global.mnFrm.cmCde.Org_id);*/
            if (dgRes == DialogResult.OK)
            {
                long mscChrgID = Global.mnFrm.cmCde.getGnrlRecID("scm.scm_tax_codes", "code_name",
          "code_id", "Miscellaneous Charges", Global.mnFrm.cmCde.Org_id);
                /*long.Parse(selVals[0]);
                */
                double msChrgAmnts = 0;// Global.getSalesSmmryItmAmnt("4Extra Charge", mscChrgID, srcDocID, srcDocType);
                double grndAmnt = Global.getSalesSmmryItmAmnt("5Grand Total", -1, srcDocID, srcDocType);
                double dscntAmnts = -1 * Global.getSalesSmmryItmAmnt("3Discount", -1, srcDocID, srcDocType);
                double pymntsAmnt = Global.getSalesSmmryItmAmnt("6Total Payments Received", -1, srcDocID, srcDocType); ;
                if (mscChrgID > 0)
                {
                    msChrgAmnts = Math.Round(Global.getSalesDocTtlAmnt(srcDocID), 2) - dscntAmnts - Math.Round(grndAmnt, 2);
                    //Global.mnFrm.cmCde.showSQLNoPermsn(msChrgAmnts + "/" + grndAmnt + "/" + dscntAmnts);
                    string chrgSmmryNm = Global.mnFrm.cmCde.getGnrlRecNm(
            "scm.scm_tax_codes", "code_id", "code_name", mscChrgID);
                    //Global.mnFrm.cmCde.showMsg(chrgSmmryNm + "/" + msChrgAmnts.ToString(), 0);
                    long smmryID = -1;
                    if (msChrgAmnts > 0.05)
                    {
                        smmryID = Global.getSalesSmmryItmID("4Extra Charge", mscChrgID,
                    srcDocID, srcDocType);
                        if (smmryID <= 0 && msChrgAmnts > 0)
                        {
                            Global.createSmmryItm("4Extra Charge", chrgSmmryNm, msChrgAmnts, mscChrgID,
                              srcDocType, srcDocID, true);
                        }
                        else if (msChrgAmnts > 0)
                        {
                            Global.updateSmmryItm(smmryID, "4Extra Charge", msChrgAmnts, true, chrgSmmryNm);
                        }
                        //else if (msChrgAmnts <= 0)
                        //{
                        //  //Global.deleteSalesSmmryItm(srcDocID, srcDocType, "4Extra Charge", mscChrgID);
                        //}

                        int accntCurrID = this.curid;
                        double funcCurrrate = Math.Round((double)1 / (double)this.exchRateNumUpDwn.Value, 15);
                        double accntCurrRate = funcCurrrate;
                        int chrgRvnuAcntID = -1;
                        int.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes", "code_id", "chrge_revnu_accnt_id", mscChrgID), out chrgRvnuAcntID);

                        if (msChrgAmnts != 0)
                        {
                            Global.deleteScmRcvblsDocDets(srcDocID, (int)mscChrgID);
                        }
                        //System.Windows.Forms.Application.DoEvents();
                        //System.Threading.Thread.Sleep(500);
                        //Global.mnFrm.cmCde.showMsg(msChrgAmnts.ToString(), 0);
                        if (Global.getScmRcvblsSmmryItmID("4Extra Charge", mscChrgID, srcDocID, srcDocType) <= 0
                          && msChrgAmnts != 0)
                        {
                            Global.createScmRcvblsDocDet(srcDocID, "4Extra Charge",
                    "Extra Charges (" + chrgSmmryNm + ") on Sales Invoice (" + this.docIDNumTextBox.Text + ")",
                    msChrgAmnts, int.Parse(this.invcCurrIDTextBox.Text), (int)mscChrgID, srcDocType
                    , false, "Increase", chrgRvnuAcntID,
                    "Increase", this.dfltRcvblAcntID, -1, "VALID", -1, this.curid, accntCurrID,
                    funcCurrrate, accntCurrRate, Math.Round(funcCurrrate * msChrgAmnts, 2),
                    Math.Round(accntCurrRate * msChrgAmnts, 2));
                        }

                        smmryID = Global.getSalesSmmryItmID("5Grand Total", -1,
                    srcDocID, srcDocType);
                        chrgSmmryNm = "Grand Total";
                        if (smmryID > 0)
                        {
                            Global.updateSmmryItm(smmryID, "5Grand Total", Math.Round(grndAmnt + msChrgAmnts, 2), true, chrgSmmryNm);
                        }
                    }
                    else
                    {
                        double initAmnt = Global.getSalesSmmryItmAmnt("1Initial Amount", -1, srcDocID, srcDocType);
                        smmryID = Global.getSalesSmmryItmID("1Initial Amount", -1,
            srcDocID, srcDocType);
                        chrgSmmryNm = "Initial Amount";
                        if (smmryID > 0)
                        {
                            Global.updateSmmryItm(smmryID, "1Initial Amount", Math.Round(initAmnt + msChrgAmnts, 2), true, chrgSmmryNm);
                        }

                        smmryID = Global.getSalesSmmryItmID("5Grand Total", -1,
            srcDocID, srcDocType);
                        chrgSmmryNm = "Grand Total";
                        if (smmryID > 0)
                        {
                            Global.updateSmmryItm(smmryID, "5Grand Total", Math.Round(grndAmnt + msChrgAmnts, 2), true, chrgSmmryNm);
                        }

                    }
                    //Total Payments    
                    grndAmnt = grndAmnt + msChrgAmnts;
                    double blsAmnt = 0;
                    string smmryNm = "";
                    if (srcDocType == "Sales Invoice")
                    {
                        //Change Given/Outstanding Balance
                        blsAmnt = Math.Round(grndAmnt - pymntsAmnt, 2);
                        if (blsAmnt < 0)
                        {
                            smmryNm = "Change Given to Customer";
                        }
                        else
                        {
                            smmryNm = "Outstanding Balance";
                        }
                        smmryID = Global.getSalesSmmryItmID("7Change/Balance", -1,
                          srcDocID, srcDocType);
                        if (smmryID <= 0)
                        {
                            Global.createSmmryItm("7Change/Balance", smmryNm, blsAmnt, -1,
                              srcDocType, srcDocID, true);
                        }
                        else
                        {
                            Global.updateSmmryItm(smmryID, "7Change/Balance", blsAmnt, true, smmryNm);
                        }
                        //Customer's Total Deposits
                        double ttlDpsts = Global.getCstmrDpsts(int.Parse(this.cstmrIDTextBox.Text),
                          int.Parse(this.invcCurrIDTextBox.Text));
                        smmryNm = "Total Deposits";
                        smmryID = Global.getSalesSmmryItmID("8Deposits", -1,
                          srcDocID, srcDocType);
                        if (smmryID <= 0)
                        {
                            Global.createSmmryItm("8Deposits", smmryNm, ttlDpsts, -1,
                              srcDocType, srcDocID, true);
                        }
                        else
                        {
                            Global.updateSmmryItm(smmryID, "8Deposits", ttlDpsts, true, smmryNm);
                        }

                        //Actual Change or Balance
                        double actlblsAmnt = Math.Round(blsAmnt - ttlDpsts, 2);
                        if (actlblsAmnt < 0)
                        {
                            smmryNm = "Amount to be Refunded to Customer";
                        }
                        else
                        {
                            smmryNm = "Actual Outstanding Balance";
                        }
                        smmryID = Global.getSalesSmmryItmID("9Actual_Change/Balance", -1,
                          srcDocID, srcDocType);
                        if (smmryID <= 0)
                        {
                            Global.createSmmryItm("9Actual_Change/Balance", smmryNm, actlblsAmnt, -1,
                              srcDocType, srcDocID, true);
                        }
                        else
                        {
                            Global.updateSmmryItm(smmryID, "9Actual_Change/Balance", actlblsAmnt, true, smmryNm);
                        }
                    }

                }
                //this.reCalcSmmrys(long.Parse(this.salesDocIDTextBox.Text), this.salesDocTypeTextBox.Text,
                //int.Parse(this.sponsorIDTextBox.Text), int.Parse(this.invcCurrIDTextBox.Text));
            }
        }

        private void saveDtButton_Click(object sender, EventArgs e)
        {
            if (this.backgroundWorker1.IsBusy)
            {
                return;
            }
            if (this.itemsDataGridView.Rows.Count > 0)
            {
                this.itemsDataGridView.EndEdit();
                //this.itemsDataGridView.Rows[0].Cells[1].Selected = true;
                System.Windows.Forms.Application.DoEvents();
            }
            if (this.saveButton.Enabled == true)
            {
                //this.saveDtButton.Enabled = true;
                this.saveButton_Click(this.saveButton, e);
                return;
            }
            if (this.addRec == true)
            {
                if ((this.editRecsPF == false
                  && this.docTypeComboBox.Text == "Pro-Forma Invoice")
                  || (this.editRecsSO == false
                 && this.docTypeComboBox.Text == "Sales Order")
                  || (this.editRecsSI == false
                  && this.docTypeComboBox.Text == "Sales Invoice")
                  || (this.editRecsIR == false
                  && this.docTypeComboBox.Text == "Internal Item Request")
                  || (this.editRecsII == false
                  && this.docTypeComboBox.Text == "Item Issue-Unbilled")
                  || (this.editRecsSR == false
                  && this.docTypeComboBox.Text == "Sales Return"))
                {
                    Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                        " this action!\nContact your System Administrator!", 0);
                    return;
                }
            }
            else
            {
                if ((this.editRecsPF == false
                  && this.docTypeComboBox.Text == "Pro-Forma Invoice")
                  || (this.editRecsSO == false
                 && this.docTypeComboBox.Text == "Sales Order")
                  || (this.editRecsSI == false
                  && this.docTypeComboBox.Text == "Sales Invoice")
                  || (this.editRecsIR == false
                  && this.docTypeComboBox.Text == "Internal Item Request")
                  || (this.editRecsII == false
                  && this.docTypeComboBox.Text == "Item Issue-Unbilled")
                  || (this.editRecsSR == false
                  && this.docTypeComboBox.Text == "Sales Return"))
                {
                    Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                        " this action!\nContact your System Administrator!", 0);
                    return;
                }
            }
            int svd = 0;
            this.saveLabel.Text = "SAVING DOCUMENT....PLEASE WAIT....";
            this.saveLabel.Visible = true;
            Cursor.Current = Cursors.WaitCursor;
            System.Windows.Forms.Application.DoEvents();
            string dateStr = Global.mnFrm.cmCde.getFrmtdDB_Date_time();
            string srcDocType = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr",
              "invc_hdr_id", "invc_type", long.Parse(this.srcDocIDTextBox.Text));

            for (int i = 0; i < this.itemsDataGridView.Rows.Count; i++)
            {
                if (!this.checkDtRqrmnts(i))
                {
                    this.itemsDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 100, 100);
                    continue;
                }
                else
                {
                    //Check if Doc Ln Rec Exists
                    //Create if not else update
                    long prsnID = long.Parse(this.itemsDataGridView.Rows[i].Cells[28].Value.ToString());
                    int itmID = int.Parse(this.itemsDataGridView.Rows[i].Cells[12].Value.ToString());
                    int storeID = int.Parse(this.itemsDataGridView.Rows[i].Cells[13].Value.ToString());
                    int crncyID = int.Parse(this.itemsDataGridView.Rows[i].Cells[14].Value.ToString());
                    long srclnID = long.Parse(this.itemsDataGridView.Rows[i].Cells[16].Value.ToString());
                    double qty = double.Parse(this.itemsDataGridView.Rows[i].Cells[4].Value.ToString());
                    double price = double.Parse(this.itemsDataGridView.Rows[i].Cells[7].Value.ToString());
                    long lineid = long.Parse(this.itemsDataGridView.Rows[i].Cells[15].Value.ToString());
                    int taxID = int.Parse(this.itemsDataGridView.Rows[i].Cells[19].Value.ToString());
                    int dscntID = int.Parse(this.itemsDataGridView.Rows[i].Cells[22].Value.ToString());
                    int chrgeID = int.Parse(this.itemsDataGridView.Rows[i].Cells[25].Value.ToString());
                    string slctdAcntIDs = this.itemsDataGridView.Rows[i].Cells[33].Value.ToString();
                    char[] w = { ',' };
                    string[] inbrghtIDs = slctdAcntIDs.Split(w);
                    int cogsID = -1;
                    int salesRevID = -1;
                    int salesRetID = -1;
                    int purcRetID = -1;
                    int expnsID = -1;
                    for (int z = 0; z < inbrghtIDs.Length; z++)
                    {
                        switch (z)
                        {
                            case 0:
                                cogsID = int.Parse(inbrghtIDs[z]);
                                break;
                            case 1:
                                salesRevID = int.Parse(inbrghtIDs[z]);
                                break;
                            case 2:
                                salesRetID = int.Parse(inbrghtIDs[z]);
                                break;
                            case 3:
                                purcRetID = int.Parse(inbrghtIDs[z]);
                                break;
                            case 4:
                                expnsID = int.Parse(inbrghtIDs[z]);
                                break;
                        }
                    }
                    double orgnlSllngPrce = 0;
                    orgnlSllngPrce = price;
                    if (taxID > 0)
                    {
                        decimal snglTax = (decimal)Global.getSalesDocCodesAmnt(taxID, (double)(1), 1);
                        orgnlSllngPrce = (double)Math.Round(this.exchRateNumUpDwn.Value * ((decimal)orgnlSllngPrce / (1 + snglTax)), 6);
                    }
                    if (lineid <= 0)
                    {
                        lineid = Global.getNewInvcLnID();
                        Global.createSalesDocLn(lineid, long.Parse(this.docIDTextBox.Text),
                          itmID, qty, price, storeID, crncyID, srclnID, taxID,
                          dscntID, chrgeID, this.itemsDataGridView.Rows[i].Cells[26].Value.ToString()
                          , this.itemsDataGridView.Rows[i].Cells[10].Value.ToString(), orgnlSllngPrce, false, prsnID,
                          this.itemsDataGridView.Rows[i].Cells[31].Value.ToString(),
                          cogsID, salesRevID, salesRetID, purcRetID, expnsID);
                        this.itemsDataGridView.Rows[i].Cells[15].Value = lineid;
                    }
                    else
                    {
                        Global.updateSalesDocLn(lineid,
                  itmID, qty, price, storeID, crncyID, srclnID,
                  taxID, dscntID, chrgeID,
                  this.itemsDataGridView.Rows[i].Cells[26].Value.ToString()
                  , this.itemsDataGridView.Rows[i].Cells[10].Value.ToString(), orgnlSllngPrce, false, prsnID,
                          this.itemsDataGridView.Rows[i].Cells[31].Value.ToString(),
                          cogsID, salesRevID, salesRetID, purcRetID, expnsID);
                    }
                    svd++;
                    this.itemsDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                }
            }
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;

            Object[] args = {this.docIDTextBox.Text, dateStr, this.docTypeComboBox.Text,
                        this.docIDNumTextBox.Text, this.srcDocIDTextBox.Text,
                        this.invcCurrIDTextBox.Text,this.exchRateNumUpDwn.Value.ToString(), srcDocType,
                      this.cstmrNmTextBox.Text,this.docCommentsTextBox.Text};

            this.backgroundWorker1.RunWorkerAsync(args);

            this.reCalcSmmrys(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text,
                int.Parse(this.cstmrIDTextBox.Text), int.Parse(this.invcCurrIDTextBox.Text), this.apprvlStatusTextBox.Text);
            this.populateSmmry(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
            this.docSaved = true;
            //System.Windows.Forms.Application.DoEvents();
            this.nxtApprvlStatusButton_Click(this.nxtApprvlStatusButton, e);
            //System.Windows.Forms.Application.DoEvents();
        }

        private void delDtButton_Click(object sender, EventArgs e)
        {
            if ((this.editRecsPF == false
              && this.docTypeComboBox.Text == "Pro-Forma Invoice")
              || (this.editRecsSO == false
             && this.docTypeComboBox.Text == "Sales Order")
              || (this.editRecsSI == false
              && this.docTypeComboBox.Text == "Sales Invoice")
              || (this.editRecsIR == false
              && this.docTypeComboBox.Text == "Internal Item Request")
              || (this.editRecsII == false
              && this.docTypeComboBox.Text == "Item Issue-Unbilled")
              || (this.editRecsSR == false
              && this.docTypeComboBox.Text == "Sales Return"))
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }
            if (this.itemsDataGridView.CurrentCell != null
         && this.itemsDataGridView.SelectedRows.Count <= 0)
            {
                this.itemsDataGridView.Rows[this.itemsDataGridView.CurrentCell.RowIndex].Selected = true;
            }

            if (this.itemsDataGridView.SelectedRows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select the record to Delete!", 0);
                return;
            }
            if (this.apprvlStatusTextBox.Text == "Approved"
              || this.apprvlStatusTextBox.Text == "Initiated"
               || this.apprvlStatusTextBox.Text == "Validated"
              || this.apprvlStatusTextBox.Text == "Cancelled" || this.apprvlStatusTextBox.Text == "Declared Bad Debt"
              || this.apprvlStatusTextBox.Text.Contains("Reviewed")
              || (this.extAppDocIDTextBox.Text != "" && this.extAppDocIDTextBox.Text != "-1"))
            {
                Global.mnFrm.cmCde.showMsg("Cannot EDIT Approved, Initiated, " +
                  "Reviewed, Validated and Cancelled Documents\r\n as well as Documents Created from other Modules!", 0);
                return;
            }
            if (Global.mnFrm.cmCde.showMsg("Are you sure you want to DELETE the selected Item?" +
         "\r\nThis action cannot be undone!", 1) == DialogResult.No)
            {
                //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                return;
            }

            bool prv = this.obey_evnts;
            this.obey_evnts = false;
            for (int i = 0; i < this.itemsDataGridView.SelectedRows.Count;)
            {
                long lnID = -1;
                long.TryParse(this.itemsDataGridView.SelectedRows[0].Cells[15].Value.ToString(), out lnID);
                if (lnID > 0)
                {
                    Global.deleteSalesLnItm(lnID);
                }
                this.itemsDataGridView.Rows.RemoveAt(this.itemsDataGridView.SelectedRows[0].Index);
            }

            Global.deleteScmRcvblsDocDet(long.Parse(this.docIDTextBox.Text));
            Global.deleteDocGLInfcLns(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);

            this.reCalcSmmrys(long.Parse(this.docIDTextBox.Text),
              this.docTypeComboBox.Text,
                int.Parse(this.cstmrIDTextBox.Text), int.Parse(this.invcCurrIDTextBox.Text), this.apprvlStatusTextBox.Text);
            this.populateSmmry(long.Parse(this.docIDTextBox.Text),
              this.docTypeComboBox.Text);
            //bool prv = this.obey_evnts;
            //this.obey_evnts = false;

            //if (this.addDtRec == false && this.editDtRec == false)
            //{
            //  this.populateLines(long.Parse(this.docIDTextBox.Text),
            //    this.docTypeComboBox.Text);
            //}
            //else if (this.itemsDataGridView.SelectedRows.Count > 0)
            //{
            //  string curid = Global.mnFrm.cmCde.getOrgFuncCurID(Global.mnFrm.cmCde.Org_id).ToString();
            //  this.itemsDataGridView.SelectedRows[0].Cells[0].Value = "";
            //  this.itemsDataGridView.SelectedRows[0].Cells[1].Value = "...";
            //  this.itemsDataGridView.SelectedRows[0].Cells[2].Value = "";
            //  this.itemsDataGridView.SelectedRows[0].Cells[3].Value = "...";
            //  this.itemsDataGridView.SelectedRows[0].Cells[4].Value = "0.00";
            //  this.itemsDataGridView.SelectedRows[0].Cells[5].Value = "Pcs";
            //  this.itemsDataGridView.SelectedRows[0].Cells[6].Value = "...";
            //  this.itemsDataGridView.SelectedRows[0].Cells[7].Value = "0.00";
            //  this.itemsDataGridView.SelectedRows[0].Cells[8].Value = "0.00";
            //  this.itemsDataGridView.SelectedRows[0].Cells[9].Value = "0.00";
            //  this.itemsDataGridView.SelectedRows[0].Cells[10].Value = "";
            //  this.itemsDataGridView.SelectedRows[0].Cells[11].Value = "...";
            //  this.itemsDataGridView.SelectedRows[0].Cells[12].Value = "-1";
            //  this.itemsDataGridView.SelectedRows[0].Cells[13].Value = "-1";
            //  this.itemsDataGridView.SelectedRows[0].Cells[14].Value = curid;
            //  this.itemsDataGridView.SelectedRows[0].Cells[15].Value = "-1";
            //  this.itemsDataGridView.SelectedRows[0].Cells[16].Value = "-1";
            //  this.itemsDataGridView.SelectedRows[0].Cells[17].Value = "";
            //  this.itemsDataGridView.SelectedRows[0].Cells[18].Value = "...";
            //  this.itemsDataGridView.SelectedRows[0].Cells[19].Value = "-1";
            //  this.itemsDataGridView.SelectedRows[0].Cells[20].Value = "";
            //  this.itemsDataGridView.SelectedRows[0].Cells[21].Value = "...";
            //  this.itemsDataGridView.SelectedRows[0].Cells[22].Value = "-1";
            //  this.itemsDataGridView.SelectedRows[0].Cells[23].Value = "";
            //  this.itemsDataGridView.SelectedRows[0].Cells[24].Value = "...";
            //  this.itemsDataGridView.SelectedRows[0].Cells[25].Value = "-1";
            //  this.itemsDataGridView.SelectedRows[0].Cells[26].Value = "";
            //}
            this.obey_evnts = prv;

        }

        private void delSmryButton_Click(object sender, EventArgs e)
        {
            if ((this.editRecsPF == false
              && this.docTypeComboBox.Text == "Pro-Forma Invoice")
              || (this.editRecsSO == false
             && this.docTypeComboBox.Text == "Sales Order")
              || (this.editRecsSI == false
              && this.docTypeComboBox.Text == "Sales Invoice")
              || (this.editRecsIR == false
              && this.docTypeComboBox.Text == "Internal Item Request")
              || (this.editRecsII == false
              && this.docTypeComboBox.Text == "Item Issue-Unbilled")
              || (this.editRecsSR == false
              && this.docTypeComboBox.Text == "Sales Return"))
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
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
              || this.apprvlStatusTextBox.Text == "Cancelled" || this.apprvlStatusTextBox.Text == "Declared Bad Debt"
              || this.apprvlStatusTextBox.Text.Contains("Reviewed")
              || (this.extAppDocIDTextBox.Text != "" && this.extAppDocIDTextBox.Text != "-1"))
            {
                Global.mnFrm.cmCde.showMsg("Cannot EDIT Approved, Initiated, " +
                  "Reviewed, Validated and Cancelled Documents\r\n as well as Documents Created from other Modules!", 0);
                return;
            }
            if (Global.mnFrm.cmCde.showMsg("Are you sure you want to DELETE the selected Item?" +
         "\r\nThis action cannot be undone!", 1) == DialogResult.No)
            {
                Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                return;
            }
            //Global.deleteSalesSmmryItm(long.Parse(this.docIDTextBox.Text),
            //  this.docTypeComboBox.Text,
            //  this.smmryDataGridView.SelectedRows[0].Cells[5].Value.ToString());

            Global.deleteSalesSmmryItm(long.Parse(this.docIDTextBox.Text),
        this.docTypeComboBox.Text,
        this.smmryDataGridView.SelectedRows[0].Cells[5].Value.ToString(),
        long.Parse(this.smmryDataGridView.SelectedRows[0].Cells[3].Value.ToString()));

            this.reCalcSmmrys(long.Parse(this.docIDTextBox.Text),
              this.docTypeComboBox.Text,
                int.Parse(this.cstmrIDTextBox.Text), int.Parse(this.invcCurrIDTextBox.Text), this.apprvlStatusTextBox.Text);
            this.populateSmmry(long.Parse(this.docIDTextBox.Text),
              this.docTypeComboBox.Text);
        }

        private void processPayButton_Click(object sender, EventArgs e)
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
                if (this.allowDuesCheckBox.Checked)
                {
                    Global.mnFrm.cmCde.showMsg("Only Approved documents can be Paid for!", 0);
                    return;
                }
            }

            if (this.payDocs == false)
            {
                dsablPayments = true;
                //Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                //    " this action!\nContact your System Administrator!", 0);
                //return;
            }
            long SIDocID = long.Parse(this.srcDocIDTextBox.Text);
            string strSrcDocType = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_sales_invc_hdr",
              "invc_hdr_id", "invc_type", SIDocID);

            if (this.docTypeComboBox.Text != "Sales Invoice"
              && this.docTypeComboBox.Text != "Sales Return"
              || (this.docTypeComboBox.Text == "Sales Return"
              && strSrcDocType != "Sales Invoice"))
            {
                Global.mnFrm.cmCde.showMsg("Only Sales Invoices & Sales Returns whose Source\r\n Document is a Sales Invoice can be paid for!", 0);
                return;
            }

            double outsBals = Global.get_DocSmryOutsbls(
              long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
            double SIDocBlsAmnt = Math.Round(Global.get_DocSmryOutsbls(SIDocID, "Sales Invoice"), 2);
            if (this.docTypeComboBox.Text == "Sales Return"
              && strSrcDocType == "Sales Invoice")
            {
                if (SIDocBlsAmnt > 0)
                {
                    Global.mnFrm.cmCde.showMsg(
                      "Cannot Pay this Document because the Customer\r\n " +
                      "has an Outstanding Balance of " + SIDocBlsAmnt + " \r\non the Source Sales Invoice!", 0);
                    return;
                }
            }
            if (outsBals > 0)
            {
            }
            else
            {
                dsablPayments = true;
                // Global.mnFrm.cmCde.showMsg("Cannot Repay a Fully Paid Document!", 0);
                //return;
            }

            double ttlDues = 0;
            bool settledDues = true;
            if (this.allowDuesCheckBox.Checked && dsablPayments == false)
            {
                settledDues = this.autoPayDuesItems(outsBals, ref ttlDues);
                this.saveLabel.Text = "Processing...Please Wait...";
                this.saveLabel.Visible = false;
                if (settledDues)
                {
                    return;
                }
            }
            else
            {
                long rcvblHdrID = Global.get_ScmRcvblsDocHdrID(
                  long.Parse(this.docIDTextBox.Text),
                  this.docTypeComboBox.Text,
                  Global.mnFrm.cmCde.Org_id);
                string rcvblDoctype = Global.mnFrm.cmCde.getGnrlRecNm(
                  "accb.accb_rcvbls_invc_hdr",
                  "rcvbls_invc_hdr_id", "rcvbls_invc_type", rcvblHdrID);

                DialogResult dgres = Global.mnFrm.cmCde.showPymntDiag(
                 createPrepay, dsablPayments,
                 this.groupBox2.Location.X - 85,
                 180,
                 outsBals, int.Parse(this.invcCurrIDTextBox.Text),
                 int.Parse(this.pymntMthdIDTextBox.Text),
                 "Customer Payments",
                 int.Parse(this.cstmrIDTextBox.Text),
                 int.Parse(this.siteIDTextBox.Text),
                 rcvblHdrID,
                 rcvblDoctype, Global.mnFrm.cmCde);

                /*addPymntDiag nwdiag = new addPymntDiag();
                nwdiag.amntToPay = outsBals;
                nwdiag.orgid = Global.mnFrm.cmCde.Org_id;
                nwdiag.entrdCurrID = int.Parse(this.invcCurrIDTextBox.Text);
                nwdiag.pymntMthdID = int.Parse(this.pymntMthdIDTextBox.Text);
                nwdiag.docTypes = "Customer Payments";


                nwdiag.srcDocID = rcvblHdrID;
                nwdiag.srcDocType = rcvblDoctype;
                nwdiag.spplrID = int.Parse(this.cstmrIDTextBox.Text);

                nwdiag.StartPosition = FormStartPosition.Manual;

                nwdiag.Location = new Point(this.groupBox2.Location.X - 85, 180);*/
                if (dgres == DialogResult.OK)
                {
                    this.reCalcRcvblsSmmrys(rcvblHdrID, rcvblDoctype);
                    this.populateDet(long.Parse(this.docIDTextBox.Text));
                    this.populateLines(long.Parse(this.docIDTextBox.Text),
                      this.docTypeComboBox.Text);
                    this.calcSmryButton_Click(this.calcSmryButton, e);
                    this.printRcptButton_Click(this.printRcptButton, e);
                }
                else
                {
                    this.calcSmryButton_Click(this.calcSmryButton, e);
                }
            }
        }

        public void undoPayment(long pymntID)
        {
            Global.deletePymntLn(pymntID);
            Global.deletePymtGLInfcLns(long.Parse(this.docIDTextBox.Text),
              this.docTypeComboBox.Text, pymntID);
            EventArgs e = new EventArgs();
            this.calcSmryButton_Click(this.calcSmryButton, e);
        }

        //private void amntRcvdNumUpDown_ValueChanged(object sender, EventArgs e)
        //{
        //  if (this.apprvlStatusTextBox.Text != "Approved")
        //  {
        //    //Global.mnFrm.cmCde.showMsg("Please Approve this document First!", 0);
        //    return;
        //  }
        //  this.changeNumUpDown.Value = (Decimal)Global.get_DocSmryOutsbls(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text)
        //  - this.amntRcvdNumUpDown.Value;

        //}

        //private void dteRcvdButton_Click(object sender, EventArgs e)
        //{
        //  Global.mnFrm.cmCde.selectDate(ref this.dteRcvdTextBox);
        //  //if (this.dteRcvdTextBox.Text.Length > 11)
        //  //{
        //  //  this.dteRcvdTextBox.Text = this.dteRcvdTextBox.Text.Substring(0, 11);
        //  //}
        //}

        private void printPrvwRcptButton_Click(object sender, EventArgs e)
        {
            //    DataSet dtst = Global.get_LastScmPay_Trns(
            //long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text, Global.mnFrm.cmCde.Org_id);
            this.calcSmryButton.PerformClick();
            long rcvblHdrID = Global.get_ScmRcvblsDocHdrID(long.Parse(this.docIDTextBox.Text),
              this.docTypeComboBox.Text, Global.mnFrm.cmCde.Org_id);
            string rcvblDoctype = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
              "rcvbls_invc_hdr_id", "rcvbls_invc_type", rcvblHdrID);

            DataSet dtst = Global.get_LastRcvblPay_Trns(rcvblHdrID,
              rcvblDoctype, Global.mnFrm.cmCde.Org_id);

            if (dtst.Tables[0].Rows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Cannot Print a Receipt when no Payment has been made!", 0);
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
            int lvid = Global.mnFrm.cmCde.getLovID("Default POS Paper Size");
            int isSmall = Global.mnFrm.cmCde.getEnbldPssblValID(
              "58mm", lvid);
            if (isSmall > 0)
            {
                this.printPreviewDialog1.Document = printDocument3;
                this.printDocument3.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Pos", 199, 3300);
            }
            else
            {
                this.printPreviewDialog1.Document = printDocument1;
                this.printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Pos", 283, 3300);
            }
            this.printPreviewDialog1.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.printPreviewDialog1.PrintPreviewControl.Zoom = 1;
            ((ToolStripButton)((ToolStrip)this.printPreviewDialog1.Controls[1]).Items[0]).Enabled = false;
            ((ToolStripButton)((ToolStrip)this.printPreviewDialog1.Controls[1]).Items[0]).Visible = false;
            this.printRcptButton1.Visible = true;
            ((ToolStrip)this.printPreviewDialog1.Controls[1]).Items.Add(this.printRcptButton1);
            this.printPreviewDialog1.FindForm().ShowIcon = false;
            this.printPreviewDialog1.FindForm().Height = Global.mnFrm.Height;
            this.printPreviewDialog1.FindForm().StartPosition = FormStartPosition.Manual;
            this.printPreviewDialog1.FindForm().Location = new Point(this.groupBox2.Location.X - 85, 20);
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

        private void printRcptButton_Click(object sender, EventArgs e)
        {
            long rcvblHdrID = Global.get_ScmRcvblsDocHdrID(long.Parse(this.docIDTextBox.Text),
          this.docTypeComboBox.Text, Global.mnFrm.cmCde.Org_id);
            string rcvblDoctype = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
              "rcvbls_invc_hdr_id", "rcvbls_invc_type", rcvblHdrID);

            DataSet dtst = Global.get_LastRcvblPay_Trns(
              rcvblHdrID, rcvblDoctype, Global.mnFrm.cmCde.Org_id);

            if (dtst.Tables[0].Rows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Cannot Print a Receipt when no Payment has been made!", 0);
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

            this.printDialog1 = new PrintDialog();
            this.printDialog1.UseEXDialog = true;
            this.printDialog1.ShowNetwork = true;
            this.printDialog1.AllowCurrentPage = false;
            this.printDialog1.AllowPrintToFile = false;
            this.printDialog1.AllowSelection = false;
            this.printDialog1.AllowSomePages = false;
            int lvid = Global.mnFrm.cmCde.getLovID("Default POS Paper Size");
            int isSmall = Global.mnFrm.cmCde.getEnbldPssblValID(
              "58mm", lvid);
            if (isSmall > 0)
            {
                this.printDialog1.PrinterSettings.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Pos", 199, 3300);
                this.printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Pos", 199, 3300);
                this.printDialog1.PrinterSettings.DefaultPageSettings.PaperSize.PaperName = "Pos";
                this.printDialog1.PrinterSettings.DefaultPageSettings.PaperSize.Height = 3300;
                this.printDialog1.PrinterSettings.DefaultPageSettings.PaperSize.Width = 199;
                printDialog1.Document = this.printDocument3;
                DialogResult res = printDialog1.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    printDocument3.Print();
                }
            }
            else
            {
                this.printDialog1.PrinterSettings.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Pos", 283, 3300);
                this.printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Pos", 283, 3300);
                this.printDialog1.PrinterSettings.DefaultPageSettings.PaperSize.PaperName = "Pos";
                this.printDialog1.PrinterSettings.DefaultPageSettings.PaperSize.Height = 3300;
                this.printDialog1.PrinterSettings.DefaultPageSettings.PaperSize.Width = 283;
                printDialog1.Document = this.printDocument1;
                DialogResult res = printDialog1.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    printDocument1.Print();
                }
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Pen aPen = new Pen(Brushes.Black, 1);
            Graphics g = e.Graphics;
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Pos", 283, 3300);
            Font font1 = new Font("Tahoma", 8.25f, FontStyle.Bold);
            Font font2 = new Font("Tahoma", 8.25f, FontStyle.Bold);
            Font font4 = new Font("Tahoma", 8.25f, FontStyle.Bold);
            Font font3 = new Font("Lucida Console", 8.25f, FontStyle.Regular);
            Font font5 = new Font("Times New Roman", 6.0f, FontStyle.Italic);

            int font1Hght = font1.Height;
            int font2Hght = font2.Height;
            int font3Hght = font3.Height;
            int font4Hght = font4.Height;
            int font5Hght = font5.Height;

            float pageWidth = e.PageSettings.PaperSize.Width - 40;//e.PageSettings.PrintableArea.Width;
            float pageHeight = e.PageSettings.PaperSize.Height - 40;// e.PageSettings.PrintableArea.Height;
                                                                    //Global.mnFrm.cmCde.showMsg(pageWidth.ToString(), 0);
            int startX = 10;
            int startY = 20;
            int offsetY = 0;
            //StringBuilder strPrnt = new StringBuilder();
            //strPrnt.AppendLine("Received From");
            string[] nwLn;
            //DataSet dtst = Global.get_LastScmPay_Trns(
            //  long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text, Global.mnFrm.cmCde.Org_id);
            long rcvblHdrID = Global.get_ScmRcvblsDocHdrID(long.Parse(this.docIDTextBox.Text),
         this.docTypeComboBox.Text, Global.mnFrm.cmCde.Org_id);
            string rcvblDoctype = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
              "rcvbls_invc_hdr_id", "rcvbls_invc_type", rcvblHdrID);

            DataSet dtst = Global.get_LastRcvblPay_Trns(
              rcvblHdrID, rcvblDoctype, Global.mnFrm.cmCde.Org_id);

            if (dtst.Tables[0].Rows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Cannot Print a Receipt when no Payment has been made!", 0);
                return;
            }
            string rcptNo = "";

            if (this.pageNo == 1)
            {
                //Org Name
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
                  Global.mnFrm.cmCde.getOrgName(Global.mnFrm.cmCde.Org_id),
                  pageWidth + 85, font2, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i]
                    , font2, Brushes.Black, startX, startY + offsetY);
                    offsetY += font2Hght;
                }

                //Pstal Address
                g.DrawString(Global.mnFrm.cmCde.getOrgPstlAddrs(Global.mnFrm.cmCde.Org_id).Trim(),
                font2, Brushes.Black, startX, startY + offsetY);
                //offsetY += font2Hght;

                ght = g.MeasureString(
                 Global.mnFrm.cmCde.getOrgPstlAddrs(Global.mnFrm.cmCde.Org_id).Trim(), font2).Height;
                offsetY = offsetY + (int)ght;
                //Contacts Nos
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
            Global.mnFrm.cmCde.getOrgContactNos(Global.mnFrm.cmCde.Org_id),
            pageWidth, font2, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i]
                    , font2, Brushes.Black, startX, startY + offsetY);
                    offsetY += font2Hght;
                }
                //Email Address
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
            Global.mnFrm.cmCde.getOrgEmailAddrs(Global.mnFrm.cmCde.Org_id),
            pageWidth, font2, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i]
                    , font2, Brushes.Black, startX, startY + offsetY);
                    offsetY += font2Hght;
                }

                offsetY += 3;
                g.DrawLine(aPen, startX, startY + offsetY, startX + pageWidth + 25,
                  startY + offsetY);
                g.DrawString("Payment Receipt", font2, Brushes.Black, startX, startY + offsetY);
                offsetY += font2Hght;
                g.DrawLine(aPen, startX, startY + offsetY, startX + pageWidth + 25,
                startY + offsetY);
                offsetY += 3;
                g.DrawString("Doc. No: ", font4, Brushes.Black, startX, startY + offsetY);
                ght = g.MeasureString("Doc. No: ", font4).Width;
                //Receipt No: 
                g.DrawString(this.docIDNumTextBox.Text,
            font3, Brushes.Black, startX + ght, startY + offsetY + 2);
                offsetY += font4Hght;

                g.DrawString("Payment Receipt No: ", font4, Brushes.Black, startX, startY + offsetY);
                //offsetY += font4Hght;
                ght = g.MeasureString("Payment Receipt No: ", font4).Width;
                //Get Last Payment
                if (dtst.Tables[0].Rows.Count > 0)
                {
                    rcptNo = dtst.Tables[0].Rows[0][0].ToString();
                }
                if (rcptNo.Length < 4)
                {
                    rcptNo = rcptNo.PadLeft(4, '0');
                }
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
            rcptNo,
            startX + ght, font3, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i]
                    , font3, Brushes.Black, startX + ght, startY + offsetY + 2);
                    offsetY += font3Hght;
                }
                offsetY += 2;

                string curcy = this.invcCurrTextBox.Text;// Global.mnFrm.cmCde.getPssblValNm(Global.mnFrm.cmCde.getOrgFuncCurID(Global.mnFrm.cmCde.Org_id));
                g.DrawString("Date Received: ", font4, Brushes.Black, startX, startY + offsetY);
                ght = g.MeasureString("Date Received: ", font4).Width;
                //Receipt No: 
                g.DrawString(dtst.Tables[0].Rows[0][8].ToString().ToUpper(),
            font3, Brushes.Black, startX + ght, startY + offsetY + 3);
                offsetY += font4Hght;
                g.DrawString("Currency: ", font4, Brushes.Black, startX, startY + offsetY);
                ght = g.MeasureString("Currency: ", font4).Width;
                //Receipt No: 
                g.DrawString(curcy,
            font3, Brushes.Black, startX + ght, startY + offsetY + 3);
                offsetY += font4Hght;
                g.DrawString("Cashier: ", font4, Brushes.Black, startX, startY + offsetY);
                ght = g.MeasureString("Cashier: ", font4).Width;
                //Receipt No: 
                g.DrawString(dtst.Tables[0].Rows[0][10].ToString().ToUpper(),
            font3, Brushes.Black, startX + ght, startY + offsetY + 2);

                if (this.cstmrNmTextBox.Text != "")
                {
                    offsetY += font4Hght;
                    g.DrawString("Customer: ", font4, Brushes.Black, startX, startY + offsetY);
                    //offsetY += font4Hght;
                    ght = g.MeasureString("Customer: ", font4).Width;
                    //Get Last Payment
                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                this.cstmrNmTextBox.Text,
                pageWidth - startX - ght - 5, font3, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        g.DrawString(nwLn[i]
                        , font3, Brushes.Black, startX + ght, startY + offsetY + 2);
                        if (i < nwLn.Length - 1)
                        {
                            offsetY += font4Hght;
                        }
                    }
                }

                offsetY += 3;
                offsetY += font3Hght;
                g.DrawLine(aPen, startX, startY + offsetY, startX + pageWidth + 25,
            startY + offsetY);
                offsetY += 3;
                g.DrawString("Item Description", font1, Brushes.Black, startX, startY + offsetY);
                //offsetY += font4Hght;
                ght = g.MeasureString("Item Description", font1).Width;
                itmWdth = (int)ght;
                qntyStartX = startX + (int)ght;
                if (this.allowDuesCheckBox.Checked == false)
                {
                    g.DrawString("Quantity".PadLeft(15, ' '), font1, Brushes.Black, qntyStartX, startY + offsetY);
                    //offsetY += font4Hght;
                }
                ght += g.MeasureString("Quantity".PadLeft(15, ' '), font1).Width;
                qntyWdth = (int)g.MeasureString("Quantity".PadLeft(15, ' '), font1).Width; ;
                prcStartX = startX + (int)ght;
                if (this.allowDuesCheckBox.Checked == true)
                {
                    itmWdth = (int)ght;
                }

                g.DrawString("Amount".PadLeft(15, ' '), font1, Brushes.Black, prcStartX, startY + offsetY);
                ght = g.MeasureString("Amount".PadLeft(15, ' '), font1).Width;
                prcWdth = (int)ght;
                offsetY += font1Hght;
                g.DrawLine(aPen, startX, startY + offsetY, startX + pageWidth + 25,
             startY + offsetY);
                offsetY += 3;
            }
            DataSet lndtst = Global.get_One_SalesDcLines(long.Parse(this.docIDTextBox.Text));
            //Line Items
            int orgOffstY = 0;
            int hgstOffst = offsetY;
            for (int a = this.prntIdx; a < lndtst.Tables[0].Rows.Count; a++)
            {
                orgOffstY = hgstOffst;
                offsetY = orgOffstY;
                ght = 0;
                if (this.allowDuesCheckBox.Checked == true)
                {
                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                    (lndtst.Tables[0].Rows[a][25].ToString().Trim()
                    + " for " + lndtst.Tables[0].Rows[a][24].ToString().Replace("-", " ").Trim()).Trim() + "@"
                + double.Parse(lndtst.Tables[0].Rows[a][3].ToString()).ToString("#,##0.00"),
                itmWdth, font3, g);
                }
                else
                {
                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                    (lndtst.Tables[0].Rows[a][25].ToString().Trim()).Trim() + "@"
                + double.Parse(lndtst.Tables[0].Rows[a][3].ToString()).ToString("#,##0.00"),
                itmWdth, font3, g);
                }

                for (int i = 0; i < nwLn.Length; i++)
                {
                    //breakPOSTxtDown
                    if (g.MeasureString(nwLn[i], font3).Width > itmWdth)
                    {
                        string[] nwnwLn;
                        nwnwLn = Global.mnFrm.cmCde.breakPOSTxtDown(nwLn[i],
                  itmWdth, font3, g, 14);
                        for (int j = 0; j < nwnwLn.Length; j++)
                        {
                            g.DrawString(nwnwLn[j]
                     , font3, Brushes.Black, startX, startY + offsetY);
                            offsetY += font3Hght;
                            ght += g.MeasureString(nwnwLn[j], font3).Width;
                        }
                    }
                    else
                    {
                        g.DrawString(nwLn[i]
                        , font3, Brushes.Black, startX, startY + offsetY);
                        offsetY += font3Hght;
                        ght += g.MeasureString(nwLn[i], font3).Width;
                    }
                }
                if (offsetY > hgstOffst)
                {
                    hgstOffst = offsetY;
                }
                offsetY = orgOffstY;

                if (this.allowDuesCheckBox.Checked == false)
                {
                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                              double.Parse(lndtst.Tables[0].Rows[a][2].ToString()).ToString("#,##0.00"),
                        qntyWdth, font3, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        if (i == 0)
                        {
                            ght = g.MeasureString(nwLn[i], font3).Width;
                        }
                        g.DrawString(nwLn[i].PadLeft(15, ' ')
                        , font3, Brushes.Black, qntyStartX - 22, startY + offsetY);
                        offsetY += font3Hght;
                    }
                    if (offsetY > hgstOffst)
                    {
                        hgstOffst = offsetY;
                    }
                    offsetY = orgOffstY;
                }
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
                  double.Parse(lndtst.Tables[0].Rows[a][4].ToString()).ToString("#,##0.00"),
            prcWdth, font3, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    if (i == 0)
                    {
                        ght = g.MeasureString(nwLn[i], font3).Width;
                    }
                    g.DrawString(nwLn[i].PadLeft(15, ' ')
                    , font3, Brushes.Black, prcStartX - 22, startY + offsetY);
                    offsetY += font3Hght;
                }
                if (offsetY > hgstOffst)
                {
                    hgstOffst = offsetY;
                }
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
                offsetY = hgstOffst + font3Hght;
                g.DrawLine(aPen, startX, startY + offsetY, startX + pageWidth + 25,
                     startY + offsetY);
                offsetY += 3;
            }
            DataSet smmryDtSt = Global.get_DocSmryLns(long.Parse(this.docIDTextBox.Text),
              this.docTypeComboBox.Text);
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
                  smmryDtSt.Tables[0].Rows[b][1].ToString().PadLeft(30, ' '),
            2 * qntyWdth, font3, g);

                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i].PadLeft(30, ' ')
                    , font3, Brushes.Black, qntyStartX - 122, startY + offsetY);
                    offsetY += font3Hght;
                    ght += g.MeasureString(nwLn[i], font3).Width;
                }
                if (offsetY > hgstOffst)
                {
                    hgstOffst = offsetY;
                }
                offsetY = orgOffstY;

                nwLn = Global.mnFrm.cmCde.breakTxtDown(
                  double.Parse(smmryDtSt.Tables[0].Rows[b][2].ToString()).ToString("#,##0.00"),
            prcWdth, font3, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    if (i == 0)
                    {
                        ght = g.MeasureString(nwLn[i], font3).Width;
                    }
                    g.DrawString(nwLn[i].PadLeft(15, ' ')
                    , font3, Brushes.Black, prcStartX - 22, startY + offsetY);
                    offsetY += font3Hght;
                }
                if (offsetY > hgstOffst)
                {
                    hgstOffst = offsetY;
                }
                this.prntIdx1++;
            }
            if (this.prntIdx2 == 0)
            {
                offsetY = hgstOffst + font3Hght;
                g.DrawLine(aPen, startX, startY + offsetY, startX + pageWidth + 25,
              startY + offsetY);
                offsetY += 3;
            }
            orgOffstY = 0;
            hgstOffst = offsetY;

            for (int c = this.prntIdx2; c < 4; c++)
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
                if (c == 0)
                {
                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                      "Receipt Amount:".PadLeft(30, ' '),
               2 * qntyWdth, font3, g);

                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        g.DrawString(nwLn[i].PadLeft(30, ' ')
                        , font3, Brushes.Black, qntyStartX - 122, startY + offsetY);
                        offsetY += font3Hght;
                        ght += g.MeasureString(nwLn[i], font3).Width;
                    }
                    if (offsetY > hgstOffst)
                    {
                        hgstOffst = offsetY;
                    }
                    offsetY = orgOffstY;

                    string amntRcvd = "0.00";
                    if (double.Parse(dtst.Tables[0].Rows[0][2].ToString()) > 0
                      && double.Parse(dtst.Tables[0].Rows[0][3].ToString()) <= 0)
                    {
                        amntRcvd = (Math.Abs(double.Parse(dtst.Tables[0].Rows[0][2].ToString())) -
                        double.Parse(dtst.Tables[0].Rows[0][3].ToString())).ToString("#,##0.00");
                    }
                    else if (double.Parse(dtst.Tables[0].Rows[0][2].ToString()) > 0
                      && double.Parse(dtst.Tables[0].Rows[0][3].ToString()) > 0)
                    {
                        amntRcvd = double.Parse(dtst.Tables[0].Rows[0][2].ToString()).ToString("#,##0.00");
                    }

                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                      double.Parse(amntRcvd).ToString("#,##0.00"),
               prcWdth, font3, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        if (i == 0)
                        {
                            ght = g.MeasureString(nwLn[i], font3).Width;
                        }
                        g.DrawString(nwLn[i].PadLeft(15, ' ')
                        , font3, Brushes.Black, prcStartX - 22, startY + offsetY);
                        offsetY += font3Hght;
                    }
                    if (offsetY > hgstOffst)
                    {
                        hgstOffst = offsetY;
                    }
                    this.prntIdx2++;
                }
                else if (c == 1)
                {
                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                      "Description:".PadLeft(30, ' '),
               2 * qntyWdth, font3, g);

                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        g.DrawString(nwLn[i].PadLeft(30, ' ')
                        , font3, Brushes.Black, qntyStartX - 122, startY + offsetY);
                        offsetY += font3Hght;
                        ght += g.MeasureString(nwLn[i], font3).Width;
                    }
                    if (offsetY > hgstOffst)
                    {
                        hgstOffst = offsetY;
                    }
                    offsetY = orgOffstY;
                    string payDesc = "-Part Payment";
                    if (double.Parse(dtst.Tables[0].Rows[0][3].ToString()) <= 0)
                    {
                        payDesc = "-Full Payment";
                    }
                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                      dtst.Tables[0].Rows[0][1].ToString() + payDesc,
               prcWdth, font3, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        if (i == 0)
                        {
                            ght = g.MeasureString(nwLn[i], font3).Width;
                        }
                        g.DrawString(nwLn[i]//.PadRight(25, ' ')
                        , font3, Brushes.Black, prcStartX + 3, startY + offsetY);
                        offsetY += font3Hght;
                    }

                    if (offsetY > hgstOffst)
                    {
                        hgstOffst = offsetY;
                    }
                    this.prntIdx2++;
                }
                else if (c == 2)
                {
                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                      "Change/Balance:".PadLeft(30, ' '),
               2 * qntyWdth, font3, g);

                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        g.DrawString(nwLn[i].PadLeft(30, ' ')
                        , font3, Brushes.Black, qntyStartX - 122, startY + offsetY);
                        offsetY += font3Hght;
                        ght += g.MeasureString(nwLn[i], font3).Width;
                    }
                    if (offsetY > hgstOffst)
                    {
                        hgstOffst = offsetY;
                    }
                    offsetY = orgOffstY;

                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                      double.Parse(dtst.Tables[0].Rows[0][3].ToString()).ToString("#,##0.00"),
               prcWdth, font3, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        if (i == 0)
                        {
                            ght = g.MeasureString(nwLn[i], font3).Width;
                        }
                        g.DrawString(nwLn[i].PadLeft(15, ' ')
                        , font3, Brushes.Black, prcStartX - 22, startY + offsetY);
                        offsetY += font3Hght;
                    }
                    if (offsetY > hgstOffst)
                    {
                        hgstOffst = offsetY;
                    }
                    this.prntIdx2++;
                }
                //      else if (c == 3)
                //      {
                //        nwLn = Global.mnFrm.cmCde.breakTxtDown(
                //          "Cashier:".PadLeft(30, ' '),
                //2 * qntyWdth, font3, g);

                //        for (int i = 0; i < nwLn.Length; i++)
                //        {
                //          g.DrawString(nwLn[i].PadLeft(30, ' ')
                //          , font3, Brushes.Black, qntyStartX - 122, startY + offsetY);
                //          offsetY += font3Hght;
                //          ght += g.MeasureString(nwLn[i], font3).Width;
                //        }
                //        if (offsetY > hgstOffst)
                //        {
                //          hgstOffst = offsetY;
                //        }
                //        offsetY = orgOffstY;
                //        nwLn = Global.mnFrm.cmCde.breakTxtDown(
                //          dtst.Tables[0].Rows[0][10].ToString().ToUpper(),
                //  prcWdth, font3, g);
                //        for (int i = 0; i < nwLn.Length; i++)
                //        {
                //          if (i == 0)
                //          {
                //            ght = g.MeasureString(nwLn[i], font3).Width;
                //          }
                //          g.DrawString(nwLn[i]//.PadRight(25, ' ')
                //          , font3, Brushes.Black, prcStartX, startY + offsetY);
                //          offsetY += font3Hght;
                //        }
                //        if (offsetY > hgstOffst)
                //        {
                //          hgstOffst = offsetY;
                //        }
                //        this.prntIdx2++;
                //      }
            }

            //Slogan: 
            offsetY += font3Hght;
            offsetY += font3Hght;
            if (hgstOffst >= pageHeight - 30)
            {
                e.HasMorePages = true;
                offsetY = 0;
                this.pageNo++;
                return;
            }
            g.DrawLine(aPen, startX, startY + offsetY, startX + pageWidth + 25,
         startY + offsetY);
            nwLn = Global.mnFrm.cmCde.breakTxtDown(
              Global.mnFrm.cmCde.getOrgSlogan(Global.mnFrm.cmCde.Org_id),
         pageWidth - ght, font5, g);
            for (int i = 0; i < nwLn.Length; i++)
            {
                g.DrawString(nwLn[i]
                , font5, Brushes.Black, startX, startY + offsetY);
                offsetY += font5Hght;
            }
            offsetY += font5Hght;
            nwLn = Global.mnFrm.cmCde.breakTxtDown(
             "Software Developed by Rhomicom Systems Technologies Ltd.",
         pageWidth + 40, font5, g);
            for (int i = 0; i < nwLn.Length; i++)
            {
                g.DrawString(nwLn[i]
                , font5, Brushes.Black, startX, startY + offsetY);
                offsetY += font5Hght;
            }
            nwLn = Global.mnFrm.cmCde.breakTxtDown(
         "Website:www.rhomicomgh.com Mobile: 0544709501/0266245395",
         pageWidth + 40, font5, g);
            for (int i = 0; i < nwLn.Length; i++)
            {
                g.DrawString(nwLn[i]
                , font5, Brushes.Black, startX, startY + offsetY);
                offsetY += font5Hght;
            }
        }

        private void printDocument2_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
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
                drfPrnt = " ";//(DRAFT INVOICE HENCE INVALID)
            }

            if (this.pageNo == 1)
            {
                //Image img = Global.mnFrm.cmCde.getDBImageFile(Global.mnFrm.cmCde.Org_id.ToString() + ".png", 0);
                Image img = global::StoresAndInventoryManager.Properties.Resources.actions_document_preview;
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
                string docHdr = this.docTypeComboBox.Text.ToUpper();
                if (this.allowDuesCheckBox.Checked)
                {
                    docHdr = "DUES PAYMENT DOCUMENT";
                }
                g.DrawString(docHdr + drfPrnt, font2, Brushes.Black, startX, startY + offsetY);

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
                g.DrawString("Customer Name: ", font4, Brushes.Black, startX, startY + offsetY);
                //offsetY += font4Hght;
                ght = g.MeasureString("Customer Name: ", font4).Width;
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
                  "billing_address", long.Parse(this.siteIDTextBox.Text));
                string shipto = Global.mnFrm.cmCde.getGnrlRecNm(
                 "scm.scm_cstmr_suplr_sites", "cust_sup_site_id",
                 "ship_to_address", long.Parse(this.siteIDTextBox.Text));
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
                if (this.allowDuesCheckBox.Checked == false)
                {
                    g.DrawString("Quantity".PadLeft(21, ' ').ToUpper(), font11, Brushes.Black, qntyStartX, startY + offsetY);
                    //offsetY += font4Hght;
                    g.DrawLine(aPen, qntyStartX + 27, startY + offsetY, qntyStartX + 27,
            startY + offsetY + (int)font11.Height);
                }
                ght += g.MeasureString("Quantity".PadLeft(26, ' '), font11).Width;
                qntyWdth = (int)g.MeasureString("Quantity".PadLeft(26, ' '), font11).Width; ;
                prcStartX = startX + (int)ght;

                if (this.allowDuesCheckBox.Checked == false)
                {
                    g.DrawString("Unit Price".PadLeft(21, ' ').ToUpper(), font11, Brushes.Black, prcStartX, startY + offsetY);
                    g.DrawLine(aPen, prcStartX + 5, startY + offsetY, prcStartX + 5,
            startY + offsetY + (int)font11.Height);
                }

                ght += g.MeasureString("Unit Price".PadLeft(26, ' '), font11).Width;
                prcWdth = (int)g.MeasureString("Unit Price".PadLeft(26, ' '), font11).Width;
                if (this.allowDuesCheckBox.Checked == true)
                {
                    itmWdth = (int)ght + 40;
                }
                amntStartX = startX + (int)ght;
                g.DrawString(this.itemsDataGridView.Columns[8].HeaderText.PadLeft(22, ' ').ToUpper(), font11, Brushes.Black, amntStartX, startY + offsetY);
                g.DrawLine(aPen, amntStartX + 5, startY + offsetY, amntStartX + 5,
        startY + offsetY + (int)font11.Height);

                ght = g.MeasureString(this.itemsDataGridView.Columns[8].HeaderText.PadLeft(25, ' '), font11).Width;
                amntWdth = (int)ght;
                g.DrawLine(aPen, startX + lnLength, startY + offsetY, startX + lnLength,
        startY + offsetY + (int)font11.Height);

                offsetY += font1Hght;
                g.DrawLine(aPen, startX, startY + offsetY, startX + lnLength,
            startY + offsetY);

            }

            offsetY += 5;
            DataSet lndtst;
            if (this.docTypeComboBox.Text == "Internal Item Request")
            {
                lndtst = Global.get_One_SalesDcLinesReq(long.Parse(this.docIDTextBox.Text));
            }
            else
            {
                lndtst = Global.get_One_SalesDcLines(long.Parse(this.docIDTextBox.Text));
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

            string ctgrNm = "";
            double ttlCtrgryAmnt = 0;
            for (int a = this.prntIdx; a < itmCnt; a++)
            {
                orgOffstY = hgstOffst;
                offsetY = orgOffstY;
                if (a != this.prntIdx)
                {
                    orgOffstY += 2;
                    offsetY += 2;
                }
                ght = 0;
                float itmHght = 0;
                if (this.docTypeComboBox.Text == "Internal Item Request")
                {
                    if (a == 0)
                    {
                        nwLn = Global.mnFrm.cmCde.breakTxtDown(lndtst.Tables[0].Rows[a][26].ToString().ToUpper(),
                 itmWdth - 30, font4, g);
                    }
                    else if (lndtst.Tables[0].Rows[a - 1][26].ToString() != lndtst.Tables[0].Rows[a][26].ToString())
                    {
                        nwLn = Global.mnFrm.cmCde.breakTxtDown(lndtst.Tables[0].Rows[a][26].ToString().ToUpper(),
                 itmWdth - 30, font4, g);
                    }
                    else
                    {
                        nwLn = new string[] { "" };
                    }
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        if (nwLn[i] != "")
                        {
                            if (i == 0)
                            {
                                offsetY = orgOffstY;
                            }
                            g.DrawString(nwLn[i]
                                         , font4, Brushes.Black, startX, startY + offsetY);
                            ght += g.MeasureString(nwLn[i], font4).Width;
                            itmHght += g.MeasureString(nwLn[i], font4).Height;
                            offsetY += font4Hght;
                            if (i == nwLn.Length - 1)
                            {
                                offsetY += 5;
                                g.DrawLine(aPen, startX, startY + orgOffstY - 15, startX,
                        startY + orgOffstY + (int)itmHght + 10);
                                g.DrawLine(aPen, prcStartX + 5, startY + orgOffstY - 15, prcStartX + 5,
                     startY + orgOffstY + (int)itmHght + 10);
                                g.DrawLine(aPen, qntyStartX + 27, startY + orgOffstY - 15, qntyStartX + 27,
                  startY + orgOffstY + (int)itmHght + 10);
                                g.DrawLine(aPen, amntStartX + 5, startY + orgOffstY - 15, amntStartX + 5,
                  startY + orgOffstY + (int)itmHght + 10);
                                g.DrawLine(aPen, startX + lnLength, startY + orgOffstY - 15, startX + lnLength,
                    startY + orgOffstY + (int)itmHght + 10);
                                if (a == itmCnt - 1)
                                {
                                    y2 = orgOffstY + (int)itmHght + 5;
                                }
                            }
                        }
                    }
                    //offsetY += 5;
                    //if (offsetY > hgstOffst)
                    //{
                    //  hgstOffst = offsetY;
                    //}
                    //offsetY = orgOffstY;
                }
                orgOffstY = offsetY;
                if (this.allowDuesCheckBox.Checked)
                {
                    nwLn = Global.mnFrm.cmCde.breakTxtDown((lndtst.Tables[0].Rows[a][25].ToString().Trim()
                       + " for " + lndtst.Tables[0].Rows[a][24].ToString().Replace("-", " ").Trim()).Trim(),
                   itmWdth - 30, font3, g);
                }
                else
                {
                    nwLn = Global.mnFrm.cmCde.breakTxtDown((
                      lndtst.Tables[0].Rows[a][25].ToString().Trim()
              + " (uom: " + lndtst.Tables[0].Rows[a][18].ToString() + ")").Trim(),
            itmWdth - 30, font3, g);
                }

                itmHght = 0;
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
                        offsetY += 2;
                    }
                }

                if (offsetY > hgstOffst)
                {
                    hgstOffst = offsetY;
                }
                offsetY = orgOffstY;

                if (this.allowDuesCheckBox.Checked == false)
                {
                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                    double.Parse(lndtst.Tables[0].Rows[a][2].ToString()).ToString("#,##0"),
              qntyWdth, font311, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        if (i == 0)
                        {
                            ght = g.MeasureString(nwLn[i], font311).Width;
                            g.DrawLine(aPen, qntyStartX + 27, startY + offsetY - 5, qntyStartX + 27,
                startY + offsetY + (int)itmHght + 5);
                        }
                        g.DrawString(nwLn[i].PadLeft(19, ' ')
                        , font311, Brushes.Black, qntyStartX - 5, startY + offsetY);
                        offsetY += font311Hght;
                    }
                    if (offsetY > hgstOffst)
                    {
                        hgstOffst = offsetY;
                    }
                    offsetY = orgOffstY;

                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                     double.Parse(lndtst.Tables[0].Rows[a][3].ToString()).ToString("#,##0.00"),
                      prcWdth, font311, g);

                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        if (i == 0)
                        {
                            ght = g.MeasureString(nwLn[i], font311).Width;
                            g.DrawLine(aPen, prcStartX + 5, startY + offsetY - 5, prcStartX + 5,
                startY + offsetY + (int)itmHght + 5);
                        }
                        g.DrawString(nwLn[i].PadLeft(19, ' ')
                        , font311, Brushes.Black, prcStartX - 5, startY + offsetY);
                        offsetY += font311Hght;
                    }
                    if (offsetY > hgstOffst)
                    {
                        hgstOffst = offsetY;
                    }
                    offsetY = orgOffstY;
                }

                ttlCtrgryAmnt += double.Parse(lndtst.Tables[0].Rows[a][4].ToString());
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
                  (double.Parse(lndtst.Tables[0].Rows[a][4].ToString())).ToString("#,##0.00"),
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
                    g.DrawString(nwLn[i].PadLeft(20, ' ')
                    , font311, Brushes.Black, amntStartX, startY + offsetY);
                    offsetY += font311Hght;
                }
                if (offsetY > hgstOffst)
                {
                    hgstOffst = offsetY;
                }

                if (this.docTypeComboBox.Text == "Internal Item Request")
                {
                    if (a == itmCnt - 1)
                    {
                        nwLn = Global.mnFrm.cmCde.breakTxtDown("TOTAL"
                          + " = " + ttlCtrgryAmnt.ToString("#,##0.00"),
                 itmWdth - 30, font4, g);
                        ttlCtrgryAmnt = 0;
                    }
                    else if (lndtst.Tables[0].Rows[a][26].ToString() != lndtst.Tables[0].Rows[a + 1][26].ToString())
                    {
                        nwLn = Global.mnFrm.cmCde.breakTxtDown("TOTAL"
                          + " = " + ttlCtrgryAmnt.ToString("#,##0.00"),
                 itmWdth - 30, font4, g);
                        ttlCtrgryAmnt = 0;
                    }
                    else
                    {
                        nwLn = new string[] { "" };
                    }
                    if (nwLn.Length > 0)
                    {
                        orgOffstY = hgstOffst;
                        offsetY = orgOffstY;
                    }
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        if (nwLn[i] != "")
                        {
                            if (i == 0)
                            {
                                itmHght = 0;
                                orgOffstY += 5;
                                offsetY = orgOffstY;
                            }
                            g.DrawString(nwLn[i]
                            , font4, Brushes.Black, startX, startY + offsetY);
                            ght += g.MeasureString(nwLn[i], font4).Width;
                            itmHght += g.MeasureString(nwLn[i], font4).Height;
                            offsetY += font4Hght;
                            if (i == nwLn.Length - 1)
                            {
                                //offsetY += 5;              
                                g.DrawLine(aPen, startX, startY + orgOffstY - 5, startX,
                        startY + orgOffstY + (int)itmHght + 5);
                                g.DrawLine(aPen, prcStartX + 5, startY + orgOffstY - 5, prcStartX + 5,
                     startY + orgOffstY + (int)itmHght + 5);
                                g.DrawLine(aPen, qntyStartX + 27, startY + orgOffstY - 5, qntyStartX + 27,
                  startY + orgOffstY + (int)itmHght + 5);
                                g.DrawLine(aPen, amntStartX + 5, startY + orgOffstY - 5, amntStartX + 5,
                  startY + orgOffstY + (int)itmHght + 5);
                                g.DrawLine(aPen, startX + lnLength, startY + orgOffstY - 5, startX + lnLength,
                    startY + orgOffstY + (int)itmHght + 5);
                                if (a == itmCnt - 1)
                                {
                                    y2 = orgOffstY + (int)itmHght + 5;
                                }
                                else
                                {
                                    g.DrawLine(aPen, startX, startY + orgOffstY + (int)itmHght - 1, startX + lnLength,
                               startY + orgOffstY + (int)itmHght - 1);
                                }
                                offsetY += 20;
                            }
                        }
                    }
                    if (offsetY > hgstOffst)
                    {
                        hgstOffst = offsetY;
                        orgOffstY = offsetY;
                    }
                    //offsetY += 15;
                }
                //hgstOffst += 8;

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
            DataSet smmryDtSt = Global.get_DocSmryLns(long.Parse(this.docIDTextBox.Text),
              this.docTypeComboBox.Text);
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
                  (smmryDtSt.Tables[0].Rows[b][1].ToString()
                  + this.itemsDataGridView.Columns[8].HeaderText.Replace("Amount", "")).PadLeft(35, ' ').PadRight(36, ' '),
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
                        if (this.allowDuesCheckBox.Checked == true)
                        {
                            g.DrawLine(aPen, qntyStartX + 27, startY + orgOffstY + (int)itmHght, qntyStartX + 15 + prcWdth + amntWdth + lnLength - itmWdth,
              startY + orgOffstY + (int)itmHght);
                        }
                        else
                        {
                            g.DrawLine(aPen, qntyStartX + 27, startY + orgOffstY + (int)itmHght, qntyStartX + 39 + lnLength - itmWdth,
                startY + orgOffstY + (int)itmHght);
                        }
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
                        g.DrawLine(aPen, amntStartX + 5, startY + offsetY - 5, amntStartX + 5, startY + offsetY + (int)itmHght);
                        g.DrawLine(aPen, startX + lnLength, startY + offsetY - 5, startX + lnLength, startY + offsetY + (int)itmHght);
                    }
                    g.DrawString(nwLn[i].PadLeft(20, ' ')
                    , font311, Brushes.Black, amntStartX, startY + offsetY + 1);
                    offsetY += font311Hght + 5;
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
            //offsetY += font2Hght;
            if (this.payTermsTextBox.Text != "")
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
              this.payTermsTextBox.Text,
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
            if (this.payTermsTextBox.Text != "")
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

        private void prvwInvoiceButton_Click(object sender, EventArgs e)
        {
            //if (this.apprvlStatusTextBox.Text != "Approved")
            //{
            //  Global.mnFrm.cmCde.showMsg("Only Approved Documents Can be Printed!", 0);
            //  return;
            //}
            this.calcSmryButton.PerformClick();
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

            this.printPreviewDialog1.Document = printDocument2;
            this.printPreviewDialog1.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.printPreviewDialog1.PrintPreviewControl.Zoom = 1;

            this.printPreviewDialog1.PrintPreviewControl.FindForm().ShowIcon = false;
            this.printPreviewDialog1.PrintPreviewControl.FindForm().ShowInTaskbar = false;
            ((ToolStripButton)((ToolStrip)this.printPreviewDialog1.Controls[1]).Items[0]).Enabled = false;
            ((ToolStripButton)((ToolStrip)this.printPreviewDialog1.Controls[1]).Items[0]).Visible = false;
            this.printInvcButton1.Visible = true;
            ((ToolStrip)this.printPreviewDialog1.Controls[1]).Items.Add(this.printInvcButton1);

            this.printDocument2.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4", 850, 1100);
            this.printPreviewDialog1.FindForm().WindowState = FormWindowState.Maximized;
            this.printPreviewDialog1.ShowDialog();
        }

        private void printInvoiceButton_Click(object sender, EventArgs e)
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
            this.printDocument2.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4", 850, 1100);
            this.printDialog1.PrinterSettings.DefaultPageSettings.PaperSize.PaperName = "A4";
            this.printDialog1.PrinterSettings.DefaultPageSettings.PaperSize.Height = 1100;
            this.printDialog1.PrinterSettings.DefaultPageSettings.PaperSize.Width = 850;

            printDialog1.Document = this.printDocument2;
            DialogResult res = printDialog1.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                printDocument2.Print();
            }
        }


        private void addMenuItem_Click(object sender, EventArgs e)
        {
            this.addButton_Click(this.addSIButton, e);
        }

        private void editMenuItem_Click(object sender, EventArgs e)
        {
            this.editButton_Click(this.editButton, e);
        }

        private void delMenuItem_Click(object sender, EventArgs e)
        {
            this.delButton_Click(this.delButton, e);
        }

        private void exptExMenuItem_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.exprtToExcel(this.invcListView);
        }

        private void rfrshMenuItem_Click(object sender, EventArgs e)
        {
            this.goButton_Click(this.goButton, e);
        }

        private void vwSQLMenuItem_Click(object sender, EventArgs e)
        {
            this.vwSQLButton_Click(this.vwSQLButton, e);
        }

        private void rcHstryMenuItem_Click(object sender, EventArgs e)
        {
            this.rcHstryButton_Click(this.rcHstryButton, e);
        }

        private void addDtMenuItem_Click(object sender, EventArgs e)
        {
            this.addDtButton_Click(this.addDtButton, e);
        }

        private void editDtMenuItem_Click(object sender, EventArgs e)
        {
            this.editDtButton_Click(this.editDtButton, e);
        }

        private void delDtMenuItem_Click(object sender, EventArgs e)
        {
            this.delDtButton_Click(this.delDtButton, e);
        }

        private void exptExDtMenuItem_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.exprtToExcel(this.itemsDataGridView);
        }

        private void rfrshDtMenuItem_Click(object sender, EventArgs e)
        {
            this.rfrshDtButton_Click(this.rfrshDtButton, e);
        }

        private void vwSQLDtMenuItem_Click(object sender, EventArgs e)
        {
            this.vwSQLDtButton_Click(this.vwSQLDtButton, e);
        }

        private void rcHstryDtMenuItem_Click(object sender, EventArgs e)
        {
            this.rcHstryDtButton_Click(this.rcHstryDtButton, e);
        }

        private void exptExSmryMenuItem_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.exprtToExcel(this.smmryDataGridView);
        }

        private void rfrshSmryMenuItem_Click(object sender, EventArgs e)
        {
            this.calcSmryButton_Click(this.calcSmryButton, e);
        }

        private void vwSQLSmryMenuItem_Click(object sender, EventArgs e)
        {
            this.vwSmrySQLButton_Click(this.vwSmrySQLButton, e);
        }

        private void rcHstrySmryMenuItem_Click(object sender, EventArgs e)
        {
            this.rcHstrySmryButton_Click(this.rcHstrySmryButton, e);
        }

        private void itemsDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            //{
            //  SendKeys.Send("{Tab}");
            //}
            this.invoiceForm_KeyDown(this, e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            //Global.invcFrm.loadPanel();
            this.loadPrvldgs();
            this.disableFormButtons();
            this.loadPanel();
        }

        private void vwExtraInfoMenuItem_Click(object sender, EventArgs e)
        {
            this.vwExtraInfoButton_Click(this.vwAttchmntsButton, e);
        }

        private void vwExtraInfoButton_Click(object sender, EventArgs e)
        {
            if (this.itemsDataGridView.CurrentCell != null
              && this.itemsDataGridView.SelectedRows.Count <= 0)
            {
                this.itemsDataGridView.Rows[this.itemsDataGridView.CurrentCell.RowIndex].Selected = true;
            }
            extraInfoDiag nwDiag = new extraInfoDiag();
            if (this.itemsDataGridView.SelectedRows[0].Cells[12].Value == null)
            {
                this.itemsDataGridView.SelectedRows[0].Cells[12].Value = "-1";
            }
            long itmID = -1;
            long.TryParse(this.itemsDataGridView.SelectedRows[0].Cells[12].Value.ToString(), out itmID);
            nwDiag.itmID = itmID;
            DialogResult dgres = nwDiag.ShowDialog();
            if (dgres == DialogResult.OK)
            {
            }
        }

        private void vwAttchmntsButton_Click(object sender, EventArgs e)
        {
            if (this.docIDTextBox.Text == "" ||
          this.docIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("Please select a saved Document First!", 0);
                return;
            }
            attchmntsDiag nwDiag = new attchmntsDiag();
            if ((this.editRecsPF == false
                    && this.docTypeComboBox.Text == "Pro-Forma Invoice")
                    || (this.editRecsSO == false
                   && this.docTypeComboBox.Text == "Sales Order")
                    || (this.editRecsSI == false
                    && this.docTypeComboBox.Text == "Sales Invoice")
                    || (this.editRecsIR == false
                    && this.docTypeComboBox.Text == "Internal Item Request")
                    || (this.editRecsII == false
                    && this.docTypeComboBox.Text == "Item Issue-Unbilled")
                    || (this.editRecsSR == false
                    && this.docTypeComboBox.Text == "Sales Return"))
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
            nwDiag.prmKeyID = long.Parse(this.docIDTextBox.Text);
            DialogResult dgres = nwDiag.ShowDialog();
            if (dgres == DialogResult.OK)
            {
            }
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
            if (this.txtChngd == false || this.obey_evnts == false)
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

            if (mytxt.Name == "invcCurrTextBox")
            {
                this.crncyNmLOVSearch();
            }
            else if (mytxt.Name == "cstmrNmTextBox")
            {
                this.sponsorLOVSrch(true);
            }
            else if (mytxt.Name == "siteNumTextBox")
            {
                //this.cstmrSiteLOVSearch();
            }
            else if (mytxt.Name == "pymntMthdTextBox")
            {
                this.pymntMthdLOVSearch();
            }
            else if (mytxt.Name == "docDteTextBox")
            {
                this.trnsDteLOVSrch();
            }
            else if (mytxt.Name == "srcDocNumTextBox")
            {
                this.srcDocLOVSrch();
            }
            this.srchWrd = "%";
            this.obey_evnts = true;
            this.txtChngd = false;
        }

        private void srcDocLOVSrch()
        {
            this.txtChngd = false;
            if (this.docTypeComboBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Please select a Document Type First!", 0);
                return;
            }
            this.srcDocNumTextBox.Text = "";
            this.srcDocIDTextBox.Text = "-1";

            string[] selVals = new string[1];
            selVals[0] = this.srcDocIDTextBox.Text;
            string lovNm = "";
            if (this.docTypeComboBox.Text == "Sales Order")
            {
                lovNm = "Approved Pro-Forma Invoices";
            }
            else if (this.docTypeComboBox.Text == "Sales Invoice")
            {
                lovNm = "Approved Sales Orders";
            }
            else if (this.docTypeComboBox.Text == "Item Issue-Unbilled")
            {
                lovNm = "Approved Internal Item Requests";
            }
            else if (this.docTypeComboBox.Text == "Sales Return")
            {
                lovNm = "Approved Sales Invoices/Item Issues";
            }
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                Global.mnFrm.cmCde.getLovID(lovNm), ref selVals,
                true, false, Global.mnFrm.cmCde.Org_id,
             this.srchWrd, "Both", true);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.srcDocIDTextBox.Text = selVals[i];
                    this.srcDocNumTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                      "scm.scm_sales_invc_hdr", "invc_hdr_id", "invc_number",
                      long.Parse(selVals[i]));
                }
                this.txtChngd = false;

                DataSet dtst = Global.get_One_SalesDcDt(long.Parse(this.srcDocIDTextBox.Text));
                for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
                {
                    this.cstmrIDTextBox.Text = dtst.Tables[0].Rows[i][5].ToString();
                    this.cstmrNmTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                      "scm.scm_cstmr_suplr", "cust_sup_id", "cust_sup_name",
                      long.Parse(dtst.Tables[0].Rows[i][5].ToString()));

                    this.siteIDTextBox.Text = dtst.Tables[0].Rows[i][6].ToString();
                    this.siteNumTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                      "scm.scm_cstmr_suplr_sites", "cust_sup_site_id", "site_name",
                      long.Parse(dtst.Tables[0].Rows[i][6].ToString()));
                    //if (this.cstmrIDTextBox.Text == "-1")
                    //{
                    //}
                    //if (this.siteIDTextBox.Text == "-1")
                    //{
                    //}
                    if (this.docCommentsTextBox.Text == "")
                    {
                        this.docCommentsTextBox.Text = dtst.Tables[0].Rows[i][7].ToString();
                    }
                    if (this.payTermsTextBox.Text == "")
                    {
                        this.payTermsTextBox.Text = dtst.Tables[0].Rows[i][8].ToString();
                    }
                }

                this.populateSrcDocLines(long.Parse(this.srcDocIDTextBox.Text), this.docTypeComboBox.Text);
                if (this.itemsDataGridView.Rows.Count > 0)
                {
                    EventArgs e = new EventArgs();
                    this.editDtButton_Click(this.editDtButton, e);
                }
            }
        }

        private void cstmrNmLOVSearch()
        {
            this.txtChngd = false;
            this.cstmrNmTextBox.Text = "";
            this.cstmrIDTextBox.Text = "-1";


            string[] selVals = new string[1];
            selVals[0] = this.cstmrIDTextBox.Text;
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
              Global.mnFrm.cmCde.getLovID("Customers"), ref selVals,
              true, false, Global.mnFrm.cmCde.Org_id,
             this.srchWrd, "Both", true);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.cstmrIDTextBox.Text = selVals[i];
                    this.cstmrNmTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                      "scm.scm_cstmr_suplr", "cust_sup_id", "cust_sup_name",
                      long.Parse(selVals[i]));
                }
            }
            this.txtChngd = false;
        }

        private void cstmrSiteLOVSearch()
        {
            this.txtChngd = false;
            if (this.cstmrIDTextBox.Text == "" || this.cstmrIDTextBox.Text == "-1")
            {
                Global.mnFrm.cmCde.showMsg("Please pick a Customer Name First!", 0);
                return;
            }
            this.siteNumTextBox.Text = "";
            this.siteIDTextBox.Text = "-1";


            string[] selVals = new string[1];
            selVals[0] = this.siteIDTextBox.Text;
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
              Global.mnFrm.cmCde.getLovID("Customer/Supplier Sites"), ref selVals,
              true, false, int.Parse(this.cstmrIDTextBox.Text),
             this.srchWrd, "Both", true);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.siteIDTextBox.Text = selVals[i];
                    this.siteNumTextBox.Text = Global.mnFrm.cmCde.getGnrlRecNm(
                      "scm.scm_cstmr_suplr_sites", "cust_sup_site_id", "site_name",
                      long.Parse(selVals[i]));
                }
            }
            this.txtChngd = false;
        }

        private void pymntMthdLOVSearch()
        {
            this.txtChngd = false;

            this.pymntMthdTextBox.Text = "";
            this.pymntMthdIDTextBox.Text = "-1";

            string[] selVals = new string[1];
            selVals[0] = this.pymntMthdIDTextBox.Text;
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
              Global.mnFrm.cmCde.getLovID("Payment Methods"), ref selVals,
              true, true, Global.mnFrm.cmCde.Org_id, "Customer Payments", "",
             this.srchWrd, "Both", true);
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
            DateTime dte1 = DateTime.Now;
            bool sccs = DateTime.TryParse(this.docDteTextBox.Text, out dte1);
            if (!sccs)
            {
                dte1 = DateTime.Now;
            }
            this.docDteTextBox.Text = dte1.ToString("dd-MMM-yyyy");
            //this.exchRateNumUpDwn.Value = 0;
            this.updtRates();
            this.txtChngd = false;
        }

        private void crncyNmLOVSearch()
        {
            this.txtChngd = false;
            if (this.invcCurrTextBox.Text == "")
            {
                this.invcCurrIDTextBox.Text = this.curid.ToString();
                this.invcCurrTextBox.Text = this.curCode;
                this.updtRates();
                this.txtChngd = false;
                return;
            }
            //this.invcCurrTextBox.Text = "";
            //this.invcCurrIDTextBox.Text = "-1";

            int[] selVals = new int[1];
            selVals[0] = int.Parse(this.invcCurrIDTextBox.Text);
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
             Global.mnFrm.cmCde.getLovID("Currencies"), ref selVals,
             true, true, this.srchWrd, "Both", true);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.invcCurrIDTextBox.Text = selVals[i].ToString();
                    this.invcCurrTextBox.Text = Global.mnFrm.cmCde.getPssblValNm(selVals[i]);
                }
                //this.exchRateNumUpDwn.Value = 0;
                this.updtRates();
                //this.clearLnsInfo();
            }
            this.txtChngd = false;
        }

        private void updtRates()
        {
            string slctdCurrID = this.invcCurrIDTextBox.Text;
            string curnm = this.invcCurrTextBox.Text;
            decimal strdRate = (decimal)Math.Round(
                    Global.get_LtstExchRate(this.curid, int.Parse(slctdCurrID),
              this.docDteTextBox.Text), 15);
            this.exchRateNumUpDwn.Value = strdRate;
            if (this.exchRateNumUpDwn.Value == 0)
            {
                this.exchRateNumUpDwn.Value = 1;
            }
            this.exchRateLabel.Text = "(" + this.curCode + "-" + this.invcCurrTextBox.Text + "):";
            this.itemsDataGridView.Columns[7].HeaderText = "Unit Price (" + curnm + ")";
            this.itemsDataGridView.Columns[8].HeaderText = "Amount (" + curnm + ")";
            this.smmryDataGridView.Columns[1].HeaderText = "Amount (" + curnm + ")";
            this.obey_evnts = false;
            for (int i = 0; i < this.itemsDataGridView.Rows.Count; i++)
            {
                int itmID = int.Parse(this.itemsDataGridView.Rows[i].Cells[12].Value.ToString());
                if (itmID > 0)
                {
                    double qty = 0;
                    double.TryParse(this.itemsDataGridView.Rows[i].Cells[4].Value.ToString(), out qty);
                    if (qty == 0)
                    {
                        continue;
                    }
                    decimal sllprce = (decimal)Global.getUOMSllngPrice(itmID, qty); /*decimal.Parse(Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_itm_list", "item_id",
  "selling_price", itmID));*/

                    this.itemsDataGridView.Rows[i].Cells[14].Value = slctdCurrID;
                    this.itemsDataGridView.Rows[i].Cells[7].Value = (this.exchRateNumUpDwn.Value * sllprce).ToString("#,##0.00");
                    this.itemsDataGridView.EndEdit();
                    System.Windows.Forms.Application.DoEvents();
                    this.itemsDataGridView.Rows[i].Cells[8].Value = (decimal.Parse(this.itemsDataGridView.Rows[i].Cells[4].Value.ToString()) * decimal.Parse(this.itemsDataGridView.Rows[i].Cells[7].Value.ToString())).ToString("#,##0.00");
                }
            }
            this.itemsDataGridView.EndEdit();
            System.Windows.Forms.Application.DoEvents();
            this.smmryDataGridView.Rows.Clear();
            this.obey_evnts = true;
        }

        private void invcCurrButton_Click(object sender, EventArgs e)
        {
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("You must be in ADD/EDIT mode first!", 0);
                return;
            }
            this.crncyNmLOVSearch();
        }

        private void pymntMthdButton_Click(object sender, EventArgs e)
        {
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("You must be in ADD/EDIT mode first!", 0);
                return;
            }
            this.pymntMthdLOVSearch();
        }

        private void exchRateNumUpDwn_ValueChanged(object sender, EventArgs e)
        {
            if (this.shdObeyEvts() == false)
            {
                return;
            }
            string slctdCurrID = this.invcCurrIDTextBox.Text;
            string curnm = this.invcCurrTextBox.Text;
            this.exchRateLabel.Text = "(" + this.curCode + "-" + this.invcCurrTextBox.Text + "):";
            this.itemsDataGridView.Columns[7].HeaderText = "Unit Price (" + curnm + ")";
            this.itemsDataGridView.Columns[8].HeaderText = "Amount (" + curnm + ")";
            this.smmryDataGridView.Columns[1].HeaderText = "Amount (" + curnm + ")";
            this.obey_evnts = false;
            for (int i = 0; i < this.itemsDataGridView.Rows.Count; i++)
            {
                int itmID = int.Parse(this.itemsDataGridView.Rows[i].Cells[12].Value.ToString());
                if (itmID > 0)
                {
                    double qty = 0;
                    double.TryParse(this.itemsDataGridView.Rows[i].Cells[4].Value.ToString(), out qty);
                    decimal sllprce = (decimal)Global.getUOMSllngPrice(itmID, qty); /*decimal.Parse(Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_itm_list", "item_id",
  "selling_price", itmID));*/

                    this.itemsDataGridView.Rows[i].Cells[14].Value = slctdCurrID;
                    this.itemsDataGridView.Rows[i].Cells[7].Value = (this.exchRateNumUpDwn.Value * sllprce).ToString("#,##0.00");
                    this.itemsDataGridView.EndEdit();
                    System.Windows.Forms.Application.DoEvents();
                    this.itemsDataGridView.Rows[i].Cells[8].Value = (decimal.Parse(this.itemsDataGridView.Rows[i].Cells[4].Value.ToString()) * decimal.Parse(this.itemsDataGridView.Rows[i].Cells[7].Value.ToString())).ToString("#,##0.00");
                }
            }
            this.itemsDataGridView.EndEdit();
            System.Windows.Forms.Application.DoEvents();
            this.smmryDataGridView.Rows.Clear();
            this.obey_evnts = true;
        }

        private void dfltFill(int idx)
        {
            if (this.itemsDataGridView.Rows[idx].Cells[0].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[0].Value = string.Empty;
            }
            if (this.itemsDataGridView.Rows[idx].Cells[2].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[2].Value = string.Empty;
            }
            if (this.itemsDataGridView.Rows[idx].Cells[4].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[4].Value = "0.00";
            }
            if (this.itemsDataGridView.Rows[idx].Cells[5].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[5].Value = string.Empty;
            }
            if (this.itemsDataGridView.Rows[idx].Cells[7].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[7].Value = "0.00";
            }
            if (this.itemsDataGridView.Rows[idx].Cells[8].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[8].Value = "0.00";
            }
            if (this.itemsDataGridView.Rows[idx].Cells[9].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[9].Value = "0.00";
            }
            if (this.itemsDataGridView.Rows[idx].Cells[10].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[10].Value = string.Empty;
            }
            if (this.itemsDataGridView.Rows[idx].Cells[12].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[12].Value = "-1";
            }
            if (this.itemsDataGridView.Rows[idx].Cells[13].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[13].Value = "-1";
            }
            if (this.itemsDataGridView.Rows[idx].Cells[14].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[14].Value = "-1";
            }
            if (this.itemsDataGridView.Rows[idx].Cells[15].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[15].Value = "-1";
            }
            if (this.itemsDataGridView.Rows[idx].Cells[16].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[16].Value = "-1";
            }
            if (this.itemsDataGridView.Rows[idx].Cells[17].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[17].Value = string.Empty;
            }
            if (this.itemsDataGridView.Rows[idx].Cells[19].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[19].Value = "-1";
            }
            if (this.itemsDataGridView.Rows[idx].Cells[20].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[20].Value = string.Empty;
            }
            if (this.itemsDataGridView.Rows[idx].Cells[22].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[22].Value = "-1";
            }
            if (this.itemsDataGridView.Rows[idx].Cells[23].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[23].Value = string.Empty;
            }
            if (this.itemsDataGridView.Rows[idx].Cells[25].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[25].Value = "-1";
            }
            if (this.itemsDataGridView.Rows[idx].Cells[26].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[26].Value = string.Empty;
            }
            if (this.itemsDataGridView.Rows[idx].Cells[28].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[28].Value = "-1";
            }
            if (this.itemsDataGridView.Rows[idx].Cells[27].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[27].Value = string.Empty;
            }
            if (this.itemsDataGridView.Rows[idx].Cells[31].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[31].Value = string.Empty;
            }
            if (this.itemsDataGridView.Rows[idx].Cells[33].Value == null)
            {
                this.itemsDataGridView.Rows[idx].Cells[33].Value = "-1,-1,-1,-1,-1";
            }
        }

        private void enblPriceEdit(int idx)
        {
            if (this.canEditPrice == true)
            {
                this.itemsDataGridView.Rows[idx].Cells[7].ReadOnly = true;
                if (this.addDtRec || this.editDtRec)
                {
                    this.itemsDataGridView.Columns[7].ReadOnly = false;
                    this.itemsDataGridView.Rows[idx].Cells[7].ReadOnly = false;
                    this.itemsDataGridView.Rows[idx].Cells[7].Style.BackColor = Color.FromArgb(255, 255, 128);
                    //long itmID = long.Parse(this.itemsDataGridView.Rows[idx].Cells[12].Value.ToString());
                    //string itmTyp = Global.mnFrm.cmCde.getGnrlRecNm(
                    //"inv.inv_itm_list", "item_id", "item_type", itmID);
                    //if (itmTyp == "Services" || this.allowDuesCheckBox.Checked)
                    //{
                    //}
                    //else
                    //{
                    //  this.itemsDataGridView.Rows[idx].Cells[7].Style.BackColor = Color.Gainsboro;
                    //}
                }
                else
                {
                    this.itemsDataGridView.Rows[idx].Cells[7].Style.BackColor = Color.Gainsboro;
                }
            }
            else
            {
                this.itemsDataGridView.Rows[idx].Cells[7].ReadOnly = true;
                this.itemsDataGridView.Rows[idx].Cells[7].Style.BackColor = Color.Gainsboro;
                this.itemsDataGridView.Columns[7].ReadOnly = true;
            }
        }

        private double getPayItmAmount(int invItmID, long cstmrID)
        {
            long pay_itm_id = -1;
            long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
        "org.org_pay_items", "inv_item_id", "item_id", invItmID), out pay_itm_id);
            //Global.mnFrm.cmCde.showSQLNoPermsn(prsn_id + "/" + pay_itm_id + "/" + trnsDte);

            if (pay_itm_id > 0)
            {
                //Global.mnFrm.cmCde.showSQLNoPermsn(-1 + "/" + pay_itm_id + "/" + "");
                long prsn_id = -1;
                long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
        "scm.scm_cstmr_suplr", "cust_sup_id", "lnkd_prsn_id", cstmrID), out prsn_id);

                if (prsn_id > 0)
                {
                    string trnsDte = Global.mnFrm.cmCde.getFrmtdDB_Date_time();
                    /*Global.mnFrm.cmCde.getOneExtInfosNVals(
                      Global.mnFrm.cmCde.getMdlGrpTblID("Pay Items",
                      Global.mnFrm.cmCde.getModuleID("Internal Payments")), pay_itm_id,
                      "pay.pay_all_other_info_table", "Start Date")*/
                    ;
                    DateTime trnDte;

                    if (DateTime.TryParseExact(trnsDte, "dd-MMM-yyyy HH:mm:ss",
          System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out trnDte))
                    {
                        return this.getPayItmAmount(prsn_id, pay_itm_id, trnsDte);
                        //Global.mnFrm.cmCde.showMsg(sellingPrcs[i].ToString(), 0);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private double getPrsPayItmAmount(int invItmID, long prsn_id)
        {
            long pay_itm_id = -1;
            long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
        "org.org_pay_items", "inv_item_id", "item_id", invItmID), out pay_itm_id);

            if (pay_itm_id > 0)
            {
                if (prsn_id > 0)
                {
                    string trnsDte = Global.mnFrm.cmCde.getFrmtdDB_Date_time();
                    /*Global.mnFrm.cmCde.getOneExtInfosNVals(
                      Global.mnFrm.cmCde.getMdlGrpTblID("Pay Items",
                      Global.mnFrm.cmCde.getModuleID("Internal Payments")), pay_itm_id,
                      "pay.pay_all_other_info_table", "Start Date")*/
                    ;

                    DateTime trnDte;

                    if (DateTime.TryParseExact(trnsDte, "dd-MMM-yyyy HH:mm:ss",
          System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out trnDte))
                    {
                        return this.getPayItmAmount(prsn_id, pay_itm_id, trnsDte);
                        //Global.mnFrm.cmCde.showMsg(sellingPrcs[i].ToString(), 0);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private double getPayItmAmount(long prsn_id, long pay_itm_id, string trns_date)
        {
            double pay_amount = 0;
            long prs_itm_val_id = Global.getPrsnItmVlID(prsn_id, pay_itm_id, trns_date);
            if (prs_itm_val_id <= 0)
            {
                prs_itm_val_id = this.getFirstItmValID(pay_itm_id);
            }
            int crncy_id = -1;
            int org_id = Global.mnFrm.cmCde.Org_id;

            //string crncy_cde = itm_uom;
            //if (itm_uom == "Money")
            //{
            //  crncy_id = Global.mnFrm.cmCde.getOrgFuncCurID(org_id);
            //  crncy_cde = Global.mnFrm.cmCde.getPssblValNm(crncy_id);
            //}
            string valSQL = Global.mnFrm.cmCde.getItmValSQL(prs_itm_val_id);
            if (valSQL == "")
            {
                pay_amount = Global.mnFrm.cmCde.getItmValueAmnt(prs_itm_val_id);
                //pay_amount = Global.getAtchdValPrsnAmnt(prsn_id, mspy_id, itm_id);
                //if (pay_amount == 0)
                //{
                //}
            }
            else
            {
                pay_amount = Global.mnFrm.cmCde.exctItmValSQL(valSQL, prsn_id,
                  org_id, trns_date);
            }

            return pay_amount;
        }

        private void itemsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e == null || this.shdObeyEvts() == false)
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
            double payItmAmnt = 0;

            if (e.ColumnIndex == 1)
            {
                if (this.addDtRec == false && this.editDtRec == false)
                {
                    Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                    this.obey_evnts = prv;
                    return;
                }
                if (this.docTypeComboBox.Text == "")
                {
                    Global.mnFrm.cmCde.showMsg("Please select a Document Type First!", 0);
                    this.obey_evnts = prv;
                    return;
                }
                itmSearchDiag nwDiag = new itmSearchDiag();
                nwDiag.my_org_id = Global.mnFrm.cmCde.Org_id;
                nwDiag.cstmrSiteID = long.Parse(this.siteIDTextBox.Text);
                nwDiag.srchIn = 0;
                nwDiag.cnsgmntsOnly = false;
                nwDiag.srchWrd = this.itemsDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                nwDiag.docType = this.docTypeComboBox.Text;
                nwDiag.itmID = int.Parse(this.itemsDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString());
                nwDiag.storeid = int.Parse(this.itemsDataGridView.Rows[e.RowIndex].Cells[13].Value.ToString());
                nwDiag.srchWrd = "%" + nwDiag.srchWrd.Replace(" ", "%") + "%";
                if (nwDiag.itmID > 0)
                {
                    nwDiag.canLoad1stOne = false;
                }
                else
                {
                    nwDiag.canLoad1stOne = this.autoLoad;
                }
                if (nwDiag.storeid <= 0)
                {
                    nwDiag.storeid = Global.selectedStoreID;
                }
                if (nwDiag.srchWrd == "" || nwDiag.srchWrd == "%%")
                {
                    nwDiag.srchWrd = "%";
                }
                int rwidx = 0;
                DialogResult dgRes = nwDiag.ShowDialog();
                if (dgRes == DialogResult.OK)
                {
                    int slctdItmsCnt = nwDiag.res.Count;
                    int[] itmIDs = new int[slctdItmsCnt];
                    int[] storeids = new int[slctdItmsCnt];
                    string[] itmNms = new string[slctdItmsCnt];
                    string[] itmDescs = new string[slctdItmsCnt];
                    double[] sellingPrcs = new double[slctdItmsCnt];
                    string[] taxNms = new string[slctdItmsCnt];
                    int[] taxIDs = new int[slctdItmsCnt];
                    string[] dscntNms = new string[slctdItmsCnt];
                    int[] dscntIDs = new int[slctdItmsCnt];
                    string[] chrgeNms = new string[slctdItmsCnt];
                    int[] chrgeIDs = new int[slctdItmsCnt];

                    int i = 0;
                    foreach (string[] lstArr in nwDiag.res)
                    {
                        itmIDs[i] = int.Parse(lstArr[0]);
                        storeids[i] = int.Parse(lstArr[1]);
                        itmNms[i] = lstArr[2];
                        itmDescs[i] = lstArr[3];
                        double.TryParse(lstArr[4], out sellingPrcs[i]);
                        taxNms[i] = lstArr[8];
                        int.TryParse(lstArr[5], out taxIDs[i]);
                        dscntNms[i] = lstArr[9];
                        int.TryParse(lstArr[6], out dscntIDs[i]);
                        chrgeNms[i] = lstArr[10];
                        int.TryParse(lstArr[7], out chrgeIDs[i]);

                        long prsn_id = -1;
                        if (this.allowDuesCheckBox.Checked)
                        {
                            long.TryParse(this.itemsDataGridView.Rows[rwidx].Cells[28].Value.ToString(), out prsn_id);
                            if (prsn_id <= 0)
                            {
                                long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
                        "scm.scm_cstmr_suplr", "cust_sup_id", "lnkd_prsn_id", long.Parse(this.cstmrIDTextBox.Text)), out prsn_id);
                            }
                            this.itemsDataGridView.Rows[rwidx].Cells[27].Value = Global.mnFrm.cmCde.getPrsnSurNameFrst(prsn_id);
                            this.itemsDataGridView.Rows[rwidx].Cells[28].Value = prsn_id;

                            payItmAmnt = this.getPrsPayItmAmount(itmIDs[i], prsn_id);

                            if (payItmAmnt != 0)
                            {
                                sellingPrcs[i] = payItmAmnt;
                            }
                        }
                        else
                        {
                            this.itemsDataGridView.Rows[rwidx].Cells[27].Value = "";
                            this.itemsDataGridView.Rows[rwidx].Cells[28].Value = prsn_id;
                        }

                        int idx = -1;// this.isItemThere(itmIDs[i]);
                        if (idx <= 0)
                        {
                            if (i == 0)
                            {
                                rwidx = e.RowIndex;
                            }
                            else
                            {
                                rwidx++;
                                if (rwidx >= this.itemsDataGridView.Rows.Count)
                                {
                                    this.createSalesDocRows(1);
                                }
                            }
                        }
                        else
                        {
                            rwidx = idx;
                        }
                        this.obey_evnts = false;
                        this.itemsDataGridView.EndEdit();
                        this.itemsDataGridView.EndEdit();
                        System.Windows.Forms.Application.DoEvents();
                        System.Windows.Forms.Application.DoEvents();
                        this.itemsDataGridView.Rows[rwidx].Cells[12].Value = itmIDs[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[13].Value = storeids[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[0].Value = itmNms[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[31].Value = itmDescs[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[2].Value = itmDescs[i];
                        long pay_itm_id = -1;
                        long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
                  "org.org_pay_items", "inv_item_id", "item_id", itmIDs[i]), out pay_itm_id);

                        if (pay_itm_id > 0)
                        {
                            this.itemsDataGridView.Rows[rwidx].Cells[4].Value = 1.00;
                        }
                        this.itemsDataGridView.Rows[rwidx].Cells[5].Value = Global.getItmUOM(itmNms[i]);
                        this.itemsDataGridView.Rows[rwidx].Cells[7].Value = Math.Round((double)this.exchRateNumUpDwn.Value * sellingPrcs[i], 2);
                        if (payItmAmnt > 0)
                        {
                            this.itemsDataGridView.Rows[rwidx].Cells[8].Value = payItmAmnt;
                        }
                        this.itemsDataGridView.Rows[rwidx].Cells[17].Value = taxNms[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[19].Value = taxIDs[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[20].Value = dscntNms[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[22].Value = dscntIDs[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[23].Value = chrgeNms[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[25].Value = chrgeIDs[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[33].Value = Global.get_One_ItmAccnts(itmIDs[i]);
                        i++;
                    }
                }
                this.itemsDataGridView.EndEdit();
                this.itemsDataGridView.EndEdit();
                System.Windows.Forms.Application.DoEvents();
                System.Windows.Forms.Application.DoEvents();
                //SendKeys.Send("{Tab}");
                //SendKeys.Send("{Tab}");
                //SendKeys.Send("{Tab}");
                this.obey_evnts = true;
                this.itemsDataGridView.CurrentCell = this.itemsDataGridView.Rows[rwidx].Cells[4];
                System.Windows.Forms.Application.DoEvents();
                this.itmChnged = true;
                this.rowCreated = false;
                nwDiag.Dispose();
                nwDiag = null;
                System.Windows.Forms.Application.DoEvents();

                //Global.mnFrm.cmCde.minimizeMemory();
            }
            else if (e.ColumnIndex == 3)
            {
                if (this.addDtRec == false && this.editDtRec == false)
                {
                    Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                    this.obey_evnts = prv;
                    return;
                }
                if (this.docTypeComboBox.Text == "")
                {
                    Global.mnFrm.cmCde.showMsg("Please select a Document Type First!", 0);
                    this.obey_evnts = prv;
                    return;
                }
                itmSearchDiag nwDiag = new itmSearchDiag();
                nwDiag.my_org_id = Global.mnFrm.cmCde.Org_id;
                nwDiag.cstmrSiteID = long.Parse(this.siteIDTextBox.Text);
                nwDiag.srchIn = 1;
                nwDiag.srchWrd = this.itemsDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                nwDiag.cnsgmntsOnly = false;
                nwDiag.docType = this.docTypeComboBox.Text;
                nwDiag.itmID = int.Parse(this.itemsDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString());
                nwDiag.storeid = int.Parse(this.itemsDataGridView.Rows[e.RowIndex].Cells[13].Value.ToString());
                nwDiag.srchWrd = "%" + nwDiag.srchWrd.Replace(" ", "%") + "%";
                if (nwDiag.itmID > 0)
                {
                    nwDiag.canLoad1stOne = false;
                }
                else
                {
                    nwDiag.canLoad1stOne = this.autoLoad;
                }
                if (nwDiag.storeid <= 0)
                {
                    nwDiag.storeid = Global.selectedStoreID;
                }
                if (nwDiag.srchWrd == "" || nwDiag.srchWrd == "%%")
                {
                    nwDiag.srchWrd = "%";
                }
                int rwidx = 0;
                DialogResult dgRes = nwDiag.ShowDialog();
                if (dgRes == DialogResult.OK)
                {
                    int slctdItmsCnt = nwDiag.res.Count;
                    int[] itmIDs = new int[slctdItmsCnt];
                    int[] storeids = new int[slctdItmsCnt];
                    string[] itmNms = new string[slctdItmsCnt];
                    string[] itmDescs = new string[slctdItmsCnt];
                    double[] sellingPrcs = new double[slctdItmsCnt];
                    string[] taxNms = new string[slctdItmsCnt];
                    int[] taxIDs = new int[slctdItmsCnt];
                    string[] dscntNms = new string[slctdItmsCnt];
                    int[] dscntIDs = new int[slctdItmsCnt];
                    string[] chrgeNms = new string[slctdItmsCnt];
                    int[] chrgeIDs = new int[slctdItmsCnt];

                    int i = 0;
                    foreach (string[] lstArr in nwDiag.res)
                    {
                        itmIDs[i] = int.Parse(lstArr[0]);
                        storeids[i] = int.Parse(lstArr[1]);
                        itmNms[i] = lstArr[2];
                        itmDescs[i] = lstArr[3];
                        double.TryParse(lstArr[4], out sellingPrcs[i]);
                        taxNms[i] = lstArr[8];
                        int.TryParse(lstArr[5], out taxIDs[i]);
                        dscntNms[i] = lstArr[9];
                        int.TryParse(lstArr[6], out dscntIDs[i]);
                        chrgeNms[i] = lstArr[10];
                        int.TryParse(lstArr[7], out chrgeIDs[i]);

                        //this.itemsDataGridView.CurrentCell = this.itemsDataGridView.Rows[idx].Cells[4];

                        long prsn_id = -1;
                        if (this.allowDuesCheckBox.Checked)
                        {
                            long.TryParse(this.itemsDataGridView.Rows[rwidx].Cells[28].Value.ToString(), out prsn_id);
                            if (prsn_id <= 0)
                            {
                                long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
                        "scm.scm_cstmr_suplr", "cust_sup_id", "lnkd_prsn_id", long.Parse(this.cstmrIDTextBox.Text)), out prsn_id);
                            }
                            this.itemsDataGridView.Rows[rwidx].Cells[27].Value = Global.mnFrm.cmCde.getPrsnSurNameFrst(prsn_id);
                            this.itemsDataGridView.Rows[rwidx].Cells[28].Value = prsn_id;

                            payItmAmnt = this.getPrsPayItmAmount(itmIDs[i], prsn_id);

                            if (payItmAmnt != 0)
                            {
                                sellingPrcs[i] = payItmAmnt;
                            }
                        }
                        else
                        {
                            this.itemsDataGridView.Rows[rwidx].Cells[27].Value = "";
                            this.itemsDataGridView.Rows[rwidx].Cells[28].Value = prsn_id;
                        }
                        int idx = -1;// this.isItemThere(itmIDs[i]);
                        if (idx <= 0)
                        {
                            if (i == 0)
                            {
                                rwidx = e.RowIndex;
                            }
                            else
                            {
                                rwidx++;
                                if (rwidx >= this.itemsDataGridView.Rows.Count)
                                {
                                    this.createSalesDocRows(1);
                                }
                            }
                        }
                        else
                        {
                            rwidx = idx;
                        }
                        this.obey_evnts = false;
                        this.itemsDataGridView.EndEdit();
                        this.itemsDataGridView.EndEdit();
                        System.Windows.Forms.Application.DoEvents();
                        System.Windows.Forms.Application.DoEvents();
                        this.itemsDataGridView.Rows[rwidx].Cells[12].Value = itmIDs[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[13].Value = storeids[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[0].Value = itmNms[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[31].Value = itmDescs[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[2].Value = itmDescs[i];
                        long pay_itm_id = -1;
                        long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
                  "org.org_pay_items", "inv_item_id", "item_id", itmIDs[i]), out pay_itm_id);

                        if (pay_itm_id > 0)
                        {
                            this.itemsDataGridView.Rows[rwidx].Cells[4].Value = 1.00;
                        }
                        this.itemsDataGridView.Rows[rwidx].Cells[5].Value = Global.getItmUOM(itmNms[i]);
                        this.itemsDataGridView.Rows[rwidx].Cells[7].Value = Math.Round((double)this.exchRateNumUpDwn.Value * sellingPrcs[i], 2);
                        if (payItmAmnt > 0)
                        {
                            this.itemsDataGridView.Rows[rwidx].Cells[8].Value = payItmAmnt;
                        }
                        //            string itmTyp = Global.mnFrm.cmCde.getGnrlRecNm(
                        //"inv.inv_itm_list", "item_id", "item_type", itmIDs[i]);
                        //            if (itmTyp == "Services")
                        //            {
                        //              this.itemsDataGridView.Rows[rwidx].Cells[7].ReadOnly = false;
                        //              this.itemsDataGridView.Rows[rwidx].Cells[7].Style.BackColor = Color.FromArgb(255, 255, 128);
                        //            }
                        //            else
                        //            {
                        //              this.itemsDataGridView.Rows[rwidx].Cells[7].ReadOnly = true;
                        //              this.itemsDataGridView.Rows[rwidx].Cells[7].Style.BackColor = Color.Gainsboro;
                        //            }

                        this.itemsDataGridView.Rows[rwidx].Cells[17].Value = taxNms[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[19].Value = taxIDs[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[20].Value = dscntNms[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[22].Value = dscntIDs[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[23].Value = chrgeNms[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[25].Value = chrgeIDs[i];
                        this.itemsDataGridView.Rows[rwidx].Cells[33].Value = Global.get_One_ItmAccnts(itmIDs[i]);

                        i++;
                    }
                }
                this.itemsDataGridView.EndEdit();
                this.itemsDataGridView.EndEdit();
                System.Windows.Forms.Application.DoEvents();
                System.Windows.Forms.Application.DoEvents();
                //SendKeys.Send("{Tab}");
                //SendKeys.Send("{Tab}");
                //SendKeys.Send("{Tab}");
                this.obey_evnts = true;
                this.itemsDataGridView.CurrentCell = this.itemsDataGridView.Rows[rwidx].Cells[4];
                System.Windows.Forms.Application.DoEvents();
                this.itmChnged = true;
                this.rowCreated = false;
                nwDiag.Dispose();
                nwDiag = null;
                System.Windows.Forms.Application.DoEvents();
                //Global.mnFrm.cmCde.minimizeMemory();
                if (payItmAmnt > 0)
                {
                    this.obey_evnts = true;
                    this.itemsDataGridView.Rows[rwidx].Cells[8].Value = payItmAmnt;
                    this.itemsDataGridView.CurrentCell = this.itemsDataGridView.Rows[e.RowIndex].Cells[7];
                }
            }
            else if (e.ColumnIndex == 6)
            {
                long itmID = int.Parse(this.itemsDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString());
                if (itmID <= 0)
                {
                    Global.mnFrm.cmCde.showMsg("Please pick an Item First!", 0);
                    this.obey_evnts = true;
                    return;
                }

                string cellLbl = "Column4";
                string mode = "Read/Write";

                if (this.addRec == false && this.editRec == false)
                {
                    mode = "Read";
                }
                string ttlQty = "0";

                if (!(itemsDataGridView.Rows[e.RowIndex].Cells[cellLbl].Value == null ||
                    itemsDataGridView.Rows[e.RowIndex].Cells[cellLbl].Value == (object)"" ||
                    itemsDataGridView.Rows[e.RowIndex].Cells[cellLbl].Value == (object)"-1"))
                {
                    ttlQty = itemsDataGridView.Rows[e.RowIndex].Cells[cellLbl].Value.ToString();
                }

                uomConversion.varUomQtyRcvd = ttlQty;

                uomConversion uomCnvs = new uomConversion();
                DialogResult dr = new DialogResult();
                string itmCode = itemsDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();

                uomCnvs.populateViewUomConversionGridView(itmCode, ttlQty, mode);
                uomCnvs.ttlTxt = ttlQty;
                uomCnvs.cntrlTxt = "0";

                dr = uomCnvs.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    itemsDataGridView.Rows[e.RowIndex].Cells[cellLbl].Value = uomConversion.varUomQtyRcvd;
                }
                this.obey_evnts = true;
                uomCnvs.Dispose();
                uomCnvs = null;
                this.itemsDataGridView.EndEdit();
                System.Windows.Forms.Application.DoEvents();
                //Global.mnFrm.cmCde.minimizeMemory();
                DataGridViewCellEventArgs e1 = new DataGridViewCellEventArgs(4, e.RowIndex);
                this.itemsDataGridView_CellValueChanged(this.itemsDataGridView, e1);
                this.docSaved = false;
            }
            else if (e.ColumnIndex == 11)
            {
                if (this.addDtRec == false && this.editDtRec == false)
                {
                    Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                    this.obey_evnts = prv;
                    return;
                }
                if (this.docTypeComboBox.Text == "")
                {
                    Global.mnFrm.cmCde.showMsg("Please select a Document Type First!", 0);
                    this.obey_evnts = prv;
                    return;
                }

                if (this.itemsDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString() == "-1")
                {
                    Global.mnFrm.cmCde.showMsg("Please select an Item First!", 0);
                    this.obey_evnts = prv;
                    return;
                }
                itmSearchDiag nwDiag = new itmSearchDiag();
                nwDiag.my_org_id = Global.mnFrm.cmCde.Org_id;
                nwDiag.srchIn = 1;
                nwDiag.srchWrd = this.itemsDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                nwDiag.cnsgmtIDs = this.itemsDataGridView.Rows[e.RowIndex].Cells[10].Value.ToString();
                nwDiag.cnsgmntsOnly = true;
                nwDiag.docType = this.docTypeComboBox.Text;
                nwDiag.itmID = int.Parse(this.itemsDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString());
                nwDiag.storeid = int.Parse(this.itemsDataGridView.Rows[e.RowIndex].Cells[13].Value.ToString());
                nwDiag.canLoad1stOne = false;

                if (nwDiag.storeid <= 0)
                {
                    nwDiag.storeid = Global.selectedStoreID;
                }
                if (nwDiag.srchWrd == "" || nwDiag.srchWrd == "%%")
                {
                    nwDiag.srchWrd = "%";
                }
                DialogResult dgRes = nwDiag.ShowDialog();
                if (dgRes == DialogResult.OK)
                {
                    this.itemsDataGridView.Rows[e.RowIndex].Cells[10].Value = nwDiag.cnsgmtIDs;
                }
                nwDiag.Dispose();
                nwDiag = null;
                System.Windows.Forms.Application.DoEvents();
            }
            else if (e.ColumnIndex == 18)
            {
                if (this.addDtRec == false && this.editDtRec == false)
                {
                    Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                    this.obey_evnts = prv;
                    return;
                }
                if (this.docTypeComboBox.Text == "")
                {
                    Global.mnFrm.cmCde.showMsg("Please select a Document Type First!", 0);
                    this.obey_evnts = prv;
                    return;
                }
                string[] selVals = new string[1];
                selVals[0] = this.itemsDataGridView.Rows[e.RowIndex].Cells[19].Value.ToString();
                DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                    Global.mnFrm.cmCde.getLovID("Tax Codes"), ref selVals,
                    true, false, Global.mnFrm.cmCde.Org_id,
               this.srchWrd, "Both", this.autoLoad);
                if (dgRes == DialogResult.OK)
                {
                    for (int i = 0; i < selVals.Length; i++)
                    {
                        this.itemsDataGridView.Rows[e.RowIndex].Cells[17].Value = Global.mnFrm.cmCde.getGnrlRecNm(
                          "scm.scm_tax_codes", "code_id", "code_name",
                          long.Parse(selVals[i]));
                        this.itemsDataGridView.Rows[e.RowIndex].Cells[19].Value = selVals[i];
                    }
                }
            }
            else if (e.ColumnIndex == 21)
            {
                if (this.addDtRec == false && this.editDtRec == false)
                {
                    Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                    this.obey_evnts = prv;
                    return;
                }
                if (this.docTypeComboBox.Text == "")
                {
                    Global.mnFrm.cmCde.showMsg("Please select a Document Type First!", 0);
                    this.obey_evnts = prv;
                    return;
                }
                string[] selVals = new string[1];
                selVals[0] = this.itemsDataGridView.Rows[e.RowIndex].Cells[22].Value.ToString();
                DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                    Global.mnFrm.cmCde.getLovID("Discount Codes"), ref selVals,
                    true, false, Global.mnFrm.cmCde.Org_id,
               this.srchWrd, "Both", this.autoLoad);
                if (dgRes == DialogResult.OK)
                {
                    for (int i = 0; i < selVals.Length; i++)
                    {
                        this.itemsDataGridView.Rows[e.RowIndex].Cells[20].Value = Global.mnFrm.cmCde.getGnrlRecNm(
                          "scm.scm_tax_codes", "code_id", "code_name",
                          long.Parse(selVals[i]));
                        this.itemsDataGridView.Rows[e.RowIndex].Cells[22].Value = selVals[i];
                    }
                    //this.reCalcSmmrys(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
                    //this.populateSmmry(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
                }
            }
            else if (e.ColumnIndex == 24)
            {
                if (this.addDtRec == false && this.editDtRec == false)
                {
                    Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                    this.obey_evnts = prv;
                    return;
                }
                if (this.docTypeComboBox.Text == "")
                {
                    Global.mnFrm.cmCde.showMsg("Please select a Document Type First!", 0);
                    this.obey_evnts = prv;
                    return;
                }
                string[] selVals = new string[1];
                selVals[0] = this.itemsDataGridView.Rows[e.RowIndex].Cells[25].Value.ToString();
                DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                    Global.mnFrm.cmCde.getLovID("Extra Charges"), ref selVals,
                    true, false, Global.mnFrm.cmCde.Org_id,
               this.srchWrd, "Both", this.autoLoad);
                if (dgRes == DialogResult.OK)
                {
                    for (int i = 0; i < selVals.Length; i++)
                    {
                        this.itemsDataGridView.Rows[e.RowIndex].Cells[23].Value = Global.mnFrm.cmCde.getGnrlRecNm(
                          "scm.scm_tax_codes", "code_id", "code_name",
                          long.Parse(selVals[i]));
                        this.itemsDataGridView.Rows[e.RowIndex].Cells[25].Value = selVals[i];
                    }
                    //this.reCalcSmmrys(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
                    //this.populateSmmry(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
                }
            }
            else if (e.ColumnIndex == 30)
            {
                if (this.addDtRec == false && this.editDtRec == false
                  && this.apprvlStatusTextBox.Text != "Approved")
                {
                    Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                    this.obey_evnts = prv;
                    return;
                }
                if (this.docTypeComboBox.Text == "")
                {
                    Global.mnFrm.cmCde.showMsg("Please select a Document Type First!", 0);
                    this.obey_evnts = prv;
                    return;
                }
                if (this.apprvlStatusTextBox.Text != "Approved")
                {
                    string lovNm = "Active Persons";
                    string[] selVals = new string[1];
                    selVals[0] = this.itemsDataGridView.Rows[e.RowIndex].Cells[28].Value.ToString();
                    DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                        Global.mnFrm.cmCde.getLovID(lovNm), ref selVals,
                        true, false, Global.mnFrm.cmCde.Org_id,
                   this.srchWrd, "Both", this.autoLoad);
                    if (dgRes == DialogResult.OK)
                    {
                        for (int i = 0; i < selVals.Length; i++)
                        {
                            this.itemsDataGridView.Rows[e.RowIndex].Cells[27].Value = Global.mnFrm.cmCde.getPrsnSurNameFrst(
                              Global.mnFrm.cmCde.getPrsnID(selVals[i])) + " (" + selVals[i] + ")";
                            this.itemsDataGridView.Rows[e.RowIndex].Cells[28].Value = Global.mnFrm.cmCde.getPrsnID(selVals[i]);

                            payItmAmnt = this.getPrsPayItmAmount(
                int.Parse(this.itemsDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString())
                , Global.mnFrm.cmCde.getPrsnID(selVals[i]));

                            if (payItmAmnt == 0)
                            {
                                //Global.mnFrm.cmCde.showMsg("The feature only works for Dues Payment Items!", 0);
                                this.obey_evnts = prv;
                                return;
                            }
                            else
                            {
                                this.itemsDataGridView.Rows[e.RowIndex].Cells[4].Value = 1.00;
                                this.itemsDataGridView.Rows[e.RowIndex].Cells[7].Value = payItmAmnt;
                                this.itemsDataGridView.Rows[e.RowIndex].Cells[8].Value = payItmAmnt;
                                this.itemsDataGridView.EndEdit();
                            }
                            this.obey_evnts = prv;
                            DataGridViewCellEventArgs e1 = new DataGridViewCellEventArgs(4, e.RowIndex);
                            this.itemsDataGridView_CellValueChanged(this.itemsDataGridView, e1);
                            this.obey_evnts = true;
                            this.itemsDataGridView.CurrentCell = this.itemsDataGridView.Rows[e.RowIndex].Cells[7];

                        }
                    }
                    //this.reCalcSmmrys(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
                    //this.populateSmmry(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
                }
                else if (this.apprvlStatusTextBox.Text == "Approved")
                {
                    long intnlPayTrnsID = -1;
                    long pay_itm_id = -1;
                    long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
              "org.org_pay_items", "inv_item_id", "item_id",
              long.Parse(this.itemsDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString())), out pay_itm_id);

                    string trnsDte = Global.mnFrm.cmCde.getFrmtdDB_Date_time();

                    DateTime trnDte;

                    if (DateTime.TryParse(trnsDte, out trnDte))
                    {
                        DialogResult dgres = Global.mnFrm.cmCde.showIntnlPymntDiag(ref intnlPayTrnsID,
                          pay_itm_id,
                          long.Parse(this.itemsDataGridView.Rows[e.RowIndex].Cells[28].Value.ToString()),
                          long.Parse(this.docIDTextBox.Text), trnsDte, Global.mnFrm.cmCde,
                          double.Parse(this.itemsDataGridView.Rows[e.RowIndex].Cells[8].Value.ToString()));

                        if (dgres == DialogResult.OK)
                        {
                            long rcvblHdrID = Global.get_ScmRcvblsDocHdrID(long.Parse(this.docIDTextBox.Text)
                              , this.docTypeComboBox.Text, Global.mnFrm.cmCde.Org_id);
                            string rcvblDoctype = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
                              "rcvbls_invc_hdr_id", "rcvbls_invc_type", rcvblHdrID);

                            this.reCalcRcvblsSmmrys(rcvblHdrID, rcvblDoctype);
                            this.populateDet(long.Parse(this.docIDTextBox.Text));
                            this.populateLines(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
                            this.calcSmryButton_Click(this.calcSmryButton, e);
                            this.printRcptButton_Click(this.printRcptButton, e);
                        }
                        else
                        {
                            this.calcSmryButton_Click(this.calcSmryButton, e);
                        }
                    }

                }
            }
            else if (e.ColumnIndex == 32)
            {
                changeAccountsDiag nwDiag = new changeAccountsDiag();
                nwDiag.slctdAcntIDs = this.itemsDataGridView.Rows[e.RowIndex].Cells[33].Value.ToString();
                nwDiag.editMode = this.editRec || this.addRec;
                if (nwDiag.ShowDialog() == DialogResult.OK)
                {
                    this.itemsDataGridView.Rows[e.RowIndex].Cells[33].Value = nwDiag.cogsIDtextBox.Text + "," +
                        nwDiag.salesRevIDtextBox.Text + "," +
                        nwDiag.salesRetIDtextBox.Text + "," +
                        nwDiag.purcRetIDtextBox.Text + "," +
                        nwDiag.expnsIDtextBox.Text;
                }
            }
            this.obey_evnts = prv;
        }

        private void itemsDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e == null || this.shdObeyEvts() == false)
            {
                return;
            }
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            this.dfltFill(e.RowIndex);

            if (e.ColumnIndex == 0)
            {
                this.autoLoad = true;
                DataGridViewCellEventArgs e1 = new DataGridViewCellEventArgs(1, e.RowIndex);
                this.itemsDataGridView_CellContentClick(this.itemsDataGridView, e1);
                this.docSaved = false;
                this.autoLoad = false;
            }
            else if (e.ColumnIndex == 2)
            {
                this.autoLoad = true;
                DataGridViewCellEventArgs e1 = new DataGridViewCellEventArgs(3, e.RowIndex);
                this.itemsDataGridView_CellContentClick(this.itemsDataGridView, e1);
                this.docSaved = false;
                this.autoLoad = false;
            }
            else if (e.ColumnIndex == 4)
            {
                double qty = 0;
                string orgnlAmnt = this.itemsDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
                bool isno = double.TryParse(orgnlAmnt, out qty);
                if (isno == false)
                {
                    qty = Math.Round(Global.computeMathExprsn(orgnlAmnt), 2);
                }

                double price = 0;
                long itmID = -1;
                long.TryParse(this.itemsDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString(), out itmID);
                long prsnID = -1;
                long.TryParse(this.itemsDataGridView.Rows[e.RowIndex].Cells[28].Value.ToString(), out prsnID);
                double nwprce = 0;
                if (this.allowDuesCheckBox.Checked)
                {
                    nwprce = this.getPrsPayItmAmount((int)itmID, prsnID);
                }
                if (nwprce == 0)
                {
                    nwprce = Global.getUOMSllngPrice(itmID, qty);
                }
                this.itemsDataGridView.Rows[e.RowIndex].Cells[7].Value = nwprce;
                price = nwprce;
                //if (qty > 1)
                //{
                //}
                //else
                //{
                //  double.TryParse(this.itemsDataGridView.Rows[e.RowIndex].Cells[7].Value.ToString(), out price);
                //}

                //this.obey_evnts = false;
                this.itemsDataGridView.Rows[e.RowIndex].Cells[4].Value = qty.ToString("#,##0.00");
                //this.itemsDataGridView.EndEdit();
                //System.Windows.Forms.Application.DoEvents();
                //System.Windows.Forms.Application.DoEvents();
                //this.itemsDataGridView.BeginEdit(false);
                //this.obey_evnts = true;

                this.itemsDataGridView.Rows[e.RowIndex].Cells[8].Value = (qty * price).ToString("#,##0.00");
                if (this.itemsDataGridView.Rows[e.RowIndex].Cells[16].Value.ToString() == "-1")
                {
                    this.itemsDataGridView.Rows[e.RowIndex].Cells[10].Value = Global.getOldstItmCnsgmts(
                      long.Parse(this.itemsDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString()), qty);
                }
                this.docSaved = false;
                this.qtyChnged = true;
                //this.itemsDataGridView.EndEdit();
                //System.Windows.Forms.Application.DoEvents();
                this.sumGridAmounts();
                if (e.RowIndex == this.itemsDataGridView.Rows.Count - 1 && this.rowCreated == false)
                {
                    this.rowCreated = true;
                    this.docIDNumTextBox.Focus();
                    System.Windows.Forms.Application.DoEvents();
                    EventArgs ex = new EventArgs();
                    this.addDtButton_Click(this.addDtButton, ex);
                }
                this.obey_evnts = true;
            }
            else if (e.ColumnIndex == 7)
            {
                //this.obey_evnts = false;
                double qty = 0;
                double price = 0;
                string orgnlAmnt = this.itemsDataGridView.Rows[e.RowIndex].Cells[7].Value.ToString();
                bool isno = double.TryParse(orgnlAmnt, out price);
                if (isno == false)
                {
                    price = Math.Round(Global.computeMathExprsn(orgnlAmnt), 2);
                }

                double.TryParse(this.itemsDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString(), out qty);
                this.itemsDataGridView.Rows[e.RowIndex].Cells[7].Value = (price).ToString("#,##0.00");
                this.itemsDataGridView.Rows[e.RowIndex].Cells[8].Value = (qty * price).ToString("#,##0.00");
                this.docSaved = false;
                //this.itemsDataGridView.EndEdit();
                //System.Windows.Forms.Application.DoEvents();
                this.sumGridAmounts();
                this.obey_evnts = true;
            }
            else if (e.ColumnIndex == 17
              || e.ColumnIndex == 20
              || e.ColumnIndex == 23)
            {
                this.srchWrd = this.itemsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (this.srchWrd == "")
                {
                    this.itemsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                    this.itemsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Value = "-1";
                    return;
                }
                if (!this.srchWrd.Contains("%"))
                {
                    this.srchWrd = "%" + this.srchWrd.Replace(" ", "%") + "%";
                }
                //this.itemsDataGridView.EndEdit();
                //System.Windows.Forms.Application.DoEvents();
                //this.obey_evnts = false;

                this.autoLoad = true;
                DataGridViewCellEventArgs e1 = new DataGridViewCellEventArgs(e.ColumnIndex + 1, e.RowIndex);
                this.itemsDataGridView_CellContentClick(this.itemsDataGridView, e1);
                this.docSaved = false;
                this.srchWrd = "";
                this.autoLoad = false;
                //this.obey_evnts = true;
                //this.itemsDataGridView.EndEdit();
                // System.Windows.Forms.Application.DoEvents();
            }
            this.srchWrd = "";
            this.autoLoad = false;

            System.Windows.Forms.Application.DoEvents();
        }

        private void itemsDataGridView_CurrentCellChanged(object sender, EventArgs e)
        {
            if (this.itemsDataGridView.CurrentCell == null
              || this.obey_evnts == false)
            {
                return;
            }

            if (this.itemsDataGridView.CurrentCell.RowIndex < 0
              || this.itemsDataGridView.CurrentCell.ColumnIndex < 0)
            {
                return;
            }

            if (this.itemsDataGridView.CurrentCell != null && this.shdObeyEvts() == true
              && (this.addRec == true || this.editRec == true))
            {
                this.obey_evnts = false;
                if (this.itemsDataGridView.CurrentCell.ColumnIndex == 5 && this.qtyChnged == true)
                {
                    this.qtyChnged = false;
                    int rwidx = this.itemsDataGridView.CurrentCell.RowIndex;
                    double qty = 0;
                    //double qty1 = 0;
                    double price = 0;
                    double.TryParse(this.itemsDataGridView.Rows[rwidx].Cells[4].Value.ToString(), out qty);
                    long itmID = -1;
                    long.TryParse(this.itemsDataGridView.Rows[rwidx].Cells[12].Value.ToString(), out itmID);
                    long prsnID = -1;
                    long.TryParse(this.itemsDataGridView.Rows[rwidx].Cells[28].Value.ToString(), out prsnID);
                    double nwprce = 0;
                    if (this.allowDuesCheckBox.Checked)
                    {
                        nwprce = this.getPrsPayItmAmount((int)itmID, prsnID);
                    }
                    if (nwprce == 0)
                    {
                        nwprce = Global.getUOMSllngPrice(itmID, qty);
                    }
                    this.itemsDataGridView.Rows[rwidx].Cells[7].Value = nwprce;
                    price = nwprce;
                    //if (qty > 1)
                    //{
                    //  //this.itemsDataGridView.EndEdit();
                    //}
                    //else
                    //{
                    //  double.TryParse(this.itemsDataGridView.Rows[rwidx].Cells[7].Value.ToString(), out price);
                    //}
                    this.itemsDataGridView.Rows[rwidx].Cells[8].Value = (qty * price).ToString("#,##0.00");
                    if (this.itemsDataGridView.Rows[rwidx].Cells[16].Value.ToString() == "-1")
                    {
                        this.itemsDataGridView.Rows[rwidx].Cells[10].Value = Global.getOldstItmCnsgmts(
                          long.Parse(this.itemsDataGridView.Rows[rwidx].Cells[12].Value.ToString()), qty);
                    }

                    SendKeys.Send("{DOWN}");
                    SendKeys.Send("{HOME}");
                }
                else if (this.itemsDataGridView.CurrentCell.ColumnIndex == 1 && this.itmChnged == true)
                {
                    //this.itmChnged = false;
                    SendKeys.Send("{TAB}");
                    //SendKeys.Send("{TAB}");
                    //SendKeys.Send("{TAB}");
                }
                else if (this.itemsDataGridView.CurrentCell.ColumnIndex == 3 && this.itmChnged == true)
                {
                    //this.itmChnged = false;
                    SendKeys.Send("{TAB}");
                    //SendKeys.Send("{TAB}");
                }
                this.obey_evnts = true;
            }
        }


        private void invoiceForm_KeyDown(object sender, KeyEventArgs e)
        {
            EventArgs ex = new EventArgs();
            if (e.Control && e.KeyCode == Keys.S)       // Ctrl-S Save
            {
                // do what you want here
                this.saveButton.PerformClick();
                e.SuppressKeyPress = true;  // stops bing! also sets handeled which stop event bubbling
            }
            else if (e.Control && e.KeyCode == Keys.N)       // Ctrl-S Save
            {
                // do what you want here
                this.addSIButton.PerformClick();
                e.SuppressKeyPress = true;  // stops bing! also sets handeled which stop event bubbling
            }
            else if (e.Control && e.KeyCode == Keys.E)       // Ctrl-S Save
            {
                // do what you want here
                this.editButton.PerformClick();
                e.SuppressKeyPress = true;  // stops bing! also sets handeled which stop event bubbling
            }
            else if ((e.Control && e.KeyCode == Keys.F) || e.KeyCode == Keys.F5)       // Ctrl-S Save
            {
                // do what you want here
                this.rfrshButton.PerformClick();
                e.SuppressKeyPress = true;  // stops bing! also sets handeled which stop event bubbling
            }
            else if (e.Control && e.KeyCode == Keys.R)
            {
                this.resetTrnsButton.PerformClick();
                e.SuppressKeyPress = true;  // stops bing! also sets handeled which stop event bubbling
            }
            else if (e.Control && e.KeyCode == Keys.Delete)
            {
                if (this.itemsDataGridView.Focused)
                {
                    if (this.delDtButton.Enabled == true)
                    {
                        this.delDtButton_Click(this.delDtButton, ex);
                    }
                }
                else if (this.smmryDataGridView.Focused)
                {
                    if (this.delSmryButton.Enabled == true)
                    {
                        this.delSmryButton_Click(this.delSmryButton, ex);
                    }
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

        private void invcListView_KeyDown(object sender, KeyEventArgs e)
        {
            Global.mnFrm.cmCde.listViewKeyDown(this.invcListView, e);
        }

        private void pdfRptButton_Click(rptParamsDiag nwDiag)
        {
            try
            {
                // Create a new PDF document
                Graphics g = Graphics.FromHwnd(this.Handle);
                XPen aPen = new XPen(XColor.FromArgb(Color.Black), 1);
                PdfDocument document = new PdfDocument();
                document.Info.Title = "MONEY RECEIVED REPORT (DOCUMENTS CREATED)";
                // Create first page for basic person details
                PdfPage page0 = document.AddPage();
                page0.Orientation = PageOrientation.Landscape;
                page0.Height = XUnit.FromInch(8.5);
                page0.Width = XUnit.FromInch(11);
                XGraphics gfx0 = XGraphics.FromPdfPage(page0);
                XFont xfont0 = new XFont("Verdana", 20, XFontStyle.BoldItalic);
                //gfx0.DrawString("Hello, World!" + this.locIDTextBox.Text, xfont0, XBrushes.Black,
                //new XRect(0, 0, page0.Width, page0.Height),
                //  XStringFormats.TopLeft);

                XFont xfont1 = new XFont("Times New Roman", 10.25f, XFontStyle.Underline | XFontStyle.Bold);
                XFont xfont11 = new XFont("Times New Roman", 10.25f, XFontStyle.Bold);
                XFont xfont2 = new XFont("Times New Roman", 10.25f, XFontStyle.Bold);
                XFont xfont4 = new XFont("Times New Roman", 10.0f, XFontStyle.Bold);
                XFont xfont41 = new XFont("Lucida Console", 10.0f);
                XFont xfont3 = new XFont("Lucida Console", 8.25f);
                XFont xfont31 = new XFont("Lucida Console", 10.5f, XFontStyle.Bold);
                XFont xfont5 = new XFont("Times New Roman", 6.0f, XFontStyle.Italic);

                Font font1 = new Font("Times New Roman", 10.25f, FontStyle.Underline | FontStyle.Bold);
                Font font11 = new Font("Times New Roman", 10.25f, FontStyle.Bold);
                Font font2 = new Font("Times New Roman", 10.25f, FontStyle.Bold);
                Font font4 = new Font("Times New Roman", 10.0f, FontStyle.Bold);
                Font font41 = new Font("Lucida Console", 10.0f);
                Font font3 = new Font("Lucida Console", 8.25f);
                Font font31 = new Font("Lucida Console", 10.5f, FontStyle.Bold);
                Font font5 = new Font("Times New Roman", 6.0f, FontStyle.Italic);

                float font1Hght = (float)gfx0.MeasureString("ABCDEFGHIJKLMNOP", xfont1).Height;
                float font2Hght = (float)gfx0.MeasureString("ABCDEFGHIJKLMNOP", xfont2).Height;
                float font3Hght = (float)gfx0.MeasureString("ABCDEFGHIJKLMNOP", xfont3).Height;
                float font4Hght = (float)gfx0.MeasureString("ABCDEFGHIJKLMNOP", xfont41).Height;
                float font5Hght = (float)gfx0.MeasureString("ABCDEFGHIJKLMNOP", xfont5).Height;

                float startX = 40;
                float startXNw = 40;
                float endX = 680;
                float startY = 40;
                float offsetY = 0;
                float ght = 0;

                float pageWidth = 760 - startX;
                float txtwdth = pageWidth - startX;
                float gwdth = 0;
                string[] nwLn;
                int pageNo = 1;
                XImage img = (XImage)Global.mnFrm.cmCde.getDBImageFile(Global.mnFrm.cmCde.Org_id.ToString() + ".png", 0);
                float picWdth = 80.00F;
                float picHght = (float)(picWdth / img.PixelWidth) * (float)img.PixelHeight;
                if (pageNo == 1)
                {
                    gfx0.DrawImage(img, startX - 10, startY + offsetY - 15, picWdth, picHght);
                    nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
                      Global.mnFrm.cmCde.getOrgName(Global.mnFrm.cmCde.Org_id),
                      pageWidth + 85, font2, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        gfx0.DrawString(nwLn[i]
                        , xfont2, XBrushes.Black, startX + picWdth + 5, startY + offsetY);
                        offsetY += font2Hght;
                    }
                    ght = (float)gfx0.MeasureString(
                      Global.mnFrm.cmCde.getOrgPstlAddrs(Global.mnFrm.cmCde.Org_id).Replace("\r\n", " ").Trim(), xfont2).Height;
                    //offsetY = offsetY + (int)ght;
                    //Pstal Address
                    XTextFormatter tf = new XTextFormatter(gfx0);
                    XRect rect = new XRect(startX + picWdth + 5, startY + offsetY - 7, pageWidth, ght);
                    gfx0.DrawRectangle(XBrushes.White, rect);
                    tf.DrawString(Global.mnFrm.cmCde.getOrgPstlAddrs(Global.mnFrm.cmCde.Org_id).Replace("\r\n", " ").Trim()
                      , xfont2, XBrushes.Black, rect, XStringFormats.TopLeft);
                    //gfx0.DrawString(,
                    //xfont2, XBrushes.Black, startX + picWdth, startY + offsetY);
                    offsetY += ght + 5;
                    //Contacts Nos
                    nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
               Global.mnFrm.cmCde.getOrgContactNos(Global.mnFrm.cmCde.Org_id),
               pageWidth, font2, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        gfx0.DrawString(nwLn[i]
                        , xfont2, XBrushes.Black, startX + picWdth + 5, startY + offsetY);
                        offsetY += font2Hght;
                    }
                    //Email Address
                    nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
               Global.mnFrm.cmCde.getOrgEmailAddrs(Global.mnFrm.cmCde.Org_id),
               pageWidth, font2, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        gfx0.DrawString(nwLn[i]
                        , xfont2, XBrushes.Black, startX + picWdth + 5, startY + offsetY);
                        offsetY += font2Hght;
                    }
                    offsetY += font2Hght;
                    if (offsetY < picHght)
                    {
                        offsetY = picHght;
                    }
                    gfx0.DrawLine(aPen, startX, startY + offsetY - 8, startX + endX,
               startY + offsetY - 8);
                }
                string orgType = Global.mnFrm.cmCde.getPssblValNm(int.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
            "org.org_details", "org_id", "org_typ_id", Global.mnFrm.cmCde.Org_id)));

                //Person Types
                float oldoffsetY = offsetY;
                float hgstOffsetY = 0;
                float hghstght = 0;

                DataSet dtst = Global.get_SalesMoneyRcvd(long.Parse(nwDiag.createdByIDTextBox.Text),
                  nwDiag.docTypComboBox.Text, nwDiag.startDteTextBox.Text, nwDiag.endDteTextBox.Text,
                  Global.mnFrm.cmCde.Org_id, nwDiag.sortByComboBox.Text, nwDiag.useCreationDateCheckBox.Checked);

                oldoffsetY = offsetY;
                offsetY = oldoffsetY + 5;

                double invcAmnt = 0;
                double dscntAmnt = 0;
                double amntRcvd = 0;
                double outstndngAmnt = 0;

                startX = startXNw;
                string usrNm = "ALL AGENTS";
                if (long.Parse(nwDiag.createdByIDTextBox.Text) > 0)
                {
                    usrNm = Global.mnFrm.cmCde.getUsername(long.Parse(nwDiag.createdByIDTextBox.Text));
                }
                for (int a = 0; a < dtst.Tables[0].Rows.Count; a++)
                {
                    startX = startXNw;
                    if (a == 0)
                    {
                        hgstOffsetY = 0;
                        ght = (float)gfx0.MeasureString(
                        ("SALES MONEY RECEIVED BY " + usrNm + " (" + nwDiag.startDteTextBox.Text + " to " + nwDiag.endDteTextBox.Text + ")").ToUpper(), xfont2).Height;
                        //lblght = ght;
                        XTextFormatter tf = new XTextFormatter(gfx0);
                        XRect rect = new XRect(startX, startY + offsetY, pageWidth - startX, ght);
                        gfx0.DrawRectangle(XBrushes.LightGray, rect);
                        tf.DrawString(("SALES MONEY RECEIVED BY " + usrNm + " (" + nwDiag.startDteTextBox.Text + " to " + nwDiag.endDteTextBox.Text + ")").ToUpper()
                          , xfont2, XBrushes.Black, rect, XStringFormats.TopLeft);
                        offsetY += (int)ght + 5;
                        for (int j = 0; j < dtst.Tables[0].Columns.Count; j++)
                        {
                            if (dtst.Tables[0].Columns[j].Caption.StartsWith("mt") == false)
                            {
                                XSize sze = gfx0.MeasureString(
                             dtst.Tables[0].Columns[j].Caption, xfont2);
                                ght = (float)sze.Height;
                                float wdth = (float)sze.Width;
                                if (wdth < (float)(dtst.Tables[0].Columns[j].Caption.Length * 5))
                                {
                                    wdth = (float)(dtst.Tables[0].Columns[j].Caption.Length * 5);
                                }
                                tf = new XTextFormatter(gfx0);
                                rect = new XRect(startX, startY + offsetY, wdth + 5, ght);
                                gfx0.DrawRectangle(XBrushes.LightGray, rect);
                                tf.DrawString(" " + dtst.Tables[0].Columns[j].Caption
                                  , xfont2, XBrushes.Black, rect, XStringFormats.TopLeft);
                                startX += wdth + 10;
                            }
                        }
                        offsetY += (int)ght + 5;
                        startX = startXNw;
                    }
                    hghstght = 0;
                    for (int j = 0; j < dtst.Tables[0].Columns.Count; j++)
                    {
                        if (dtst.Tables[0].Columns[j].Caption.StartsWith("mt") == false)
                        {
                            XSize sze = gfx0.MeasureString(dtst.Tables[0].Columns[j].Caption, xfont2);
                            ght = (float)sze.Height;
                            float wdth = (float)(sze.Width);
                            if (wdth < (float)(dtst.Tables[0].Columns[j].Caption.Length * 5))
                            {
                                wdth = (float)(dtst.Tables[0].Columns[j].Caption.Length * 5);
                            }
                            string strToBreak = dtst.Tables[0].Rows[a][j].ToString();

                            if (j >= 2 && j <= 5)
                            {
                                double tst = 0;
                                if (double.TryParse(strToBreak, out tst) == false)
                                {
                                    strToBreak = "0";
                                }
                                strToBreak = double.Parse(strToBreak).ToString("#,##0.00");
                                if (j == 2)
                                {
                                    invcAmnt += double.Parse(strToBreak);
                                }
                                else if (j == 3)
                                {
                                    dscntAmnt += double.Parse(strToBreak);
                                }
                                else if (j == 4)
                                {
                                    amntRcvd += double.Parse(strToBreak);
                                }
                                else if (j == 5)
                                {
                                    outstndngAmnt += double.Parse(strToBreak);
                                }
                            }
                            nwLn = Global.mnFrm.cmCde.breakTxtDown(
                              strToBreak,
                              (int)(wdth * 1.64), font41, g);

                            string finlStr = "";
                            if (j >= 2 && j <= 5)
                            {
                                finlStr = string.Join("\n", nwLn).PadLeft(15);
                            }
                            else
                            {
                                finlStr = string.Join("\n", nwLn);
                            }
                            ght = (float)gfx0.MeasureString(
                           finlStr, xfont41).Height * 1.2F;

                            XTextFormatter tf = new XTextFormatter(gfx0);
                            XRect rect = new XRect(startX, startY + offsetY, wdth + 5, ght);
                            gfx0.DrawRectangle(XBrushes.White, rect);


                            tf.DrawString(finlStr
                              , xfont41, XBrushes.Black, rect, XStringFormats.TopLeft);

                            startX += wdth + 10;
                            if (hghstght < ght)
                            {
                                hghstght = ght;
                            }
                        }
                    }
                    if (hghstght < 10)
                    {
                        hghstght = 10;
                    }
                    offsetY += hghstght + 5;
                    if (hgstOffsetY < offsetY)
                    {
                        hgstOffsetY = offsetY;
                    }
                    if ((startY + offsetY) >= 580)
                    {
                        page0 = document.AddPage();
                        page0.Orientation = PageOrientation.Portrait;
                        page0.Height = XUnit.FromInch(8.5);
                        page0.Width = XUnit.FromInch(11);
                        gfx0 = XGraphics.FromPdfPage(page0);
                        offsetY = 0;
                        hgstOffsetY = 0;
                    }
                }

                //offsetY += hghstght + 5;
                offsetY += 5;
                hghstght = 0;
                startX = startXNw;
                for (int j = 0; j < dtst.Tables[0].Columns.Count; j++)
                {
                    if (dtst.Tables[0].Columns[j].Caption.StartsWith("mt") == false)
                    {
                        XSize sze = gfx0.MeasureString(dtst.Tables[0].Columns[j].Caption, xfont2);
                        ght = (float)sze.Height;
                        float wdth = (float)(sze.Width);
                        if (wdth < (float)(dtst.Tables[0].Columns[j].Caption.Length * 5))
                        {
                            wdth = (float)(dtst.Tables[0].Columns[j].Caption.Length * 5);
                        }
                        string strToBreak = " ";
                        if (j == 1)
                        {
                            strToBreak = "TOTALS = ";
                        }
                        if (j >= 2 && j <= 5)
                        {
                            if (j == 2)
                            {
                                strToBreak = (invcAmnt).ToString("#,##0.00");
                            }
                            else if (j == 3)
                            {
                                strToBreak = (dscntAmnt).ToString("#,##0.00");
                            }
                            else if (j == 4)
                            {
                                strToBreak = (amntRcvd).ToString("#,##0.00");
                            }
                            else if (j == 5)
                            {
                                strToBreak = (outstndngAmnt).ToString("#,##0.00");
                            }
                        }
                        nwLn = Global.mnFrm.cmCde.breakTxtDown(
                          strToBreak,
                          (int)(wdth * 1.5), font31, g);

                        string finlStr = "";
                        if (j >= 2 && j <= 5)
                        {
                            finlStr = string.Join("\n", nwLn).PadLeft(15);
                        }
                        else
                        {
                            finlStr = string.Join("\n", nwLn);
                        }
                        ght = (float)gfx0.MeasureString(
                       finlStr, xfont31).Height;

                        XTextFormatter tf = new XTextFormatter(gfx0);
                        XRect rect = new XRect(startX, startY + offsetY, wdth + 5, ght);
                        gfx0.DrawRectangle(XBrushes.White, rect);


                        tf.DrawString(finlStr
                          , xfont31, XBrushes.Black, rect, XStringFormats.TopLeft);

                        startX += wdth + 10;
                        if (hghstght < ght)
                        {
                            hghstght = ght;
                        }
                    }
                }
                offsetY += hghstght + 5;
                //Slogan: 
                startX = startXNw;
                offsetY = 535;
                gfx0.DrawLine(aPen, startX, startY + offsetY, startX + endX,
            startY + offsetY);
                offsetY += font3Hght;
                nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
                  Global.mnFrm.cmCde.getOrgName(Global.mnFrm.cmCde.Org_id) + "..." +
                  Global.mnFrm.cmCde.getOrgSlogan(Global.mnFrm.cmCde.Org_id),
            pageWidth - ght, font5, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    gfx0.DrawString(nwLn[i]
                    , xfont5, XBrushes.Black, startX, startY + offsetY);
                    offsetY += font5Hght;
                }
                offsetY += font5Hght;
                nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
                 "Software Developed by Rhomicom Systems Technologies Ltd.",
            pageWidth + 40, font5, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    gfx0.DrawString(nwLn[i]
                    , xfont5, XBrushes.Black, startX, startY + offsetY);
                    offsetY += font5Hght;
                }
                nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
            "Website:www.rhomicomgh.com",
            pageWidth + 40, font5, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    gfx0.DrawString(nwLn[i]
                    , xfont5, XBrushes.Black, startX, startY + offsetY);
                    offsetY += font5Hght;
                }
                // Create second page for additional person details
                /*PdfPage page1 = document.AddPage();
                XGraphics gfx1 = XGraphics.FromPdfPage(page1);
                XFont xfont1 = new XFont("Verdana", 20, XFontStyle.BoldItalic);
                gfx0.DrawString("Page 2!" + this.locIDTextBox.Text, xfont1, XBrushes.Black,
                  new XRect(100, 100, page1.Width, page1.Height),
                  XStringFormats.TopLeft);*/



                // Save the document...
                string filename = Global.mnFrm.cmCde.getRptDrctry() + @"\SalesMoneyRcvd_" + Global.mnFrm.cmCde.getDB_Date_time().Replace(" ", "").Replace(":", "").Replace("-", "") + ".pdf";
                document.Save(filename);
                // ...and start a viewer.
                System.Diagnostics.Process.Start(filename);
                this.moneyRcvdRptButton.Enabled = true;
                //Global.mnFrm.cmCde.upldImgsFTP(9, Global.mnFrm.cmCde.getRptDrctry(), @"\SalesMoneyRcvd_" + Global.mnFrm.cmCde.getDB_Date_time().Replace(" ", "").Replace(":", "").Replace("-", "") + ".pdf");
                System.Windows.Forms.Application.DoEvents();
            }
            catch (Exception ex)
            {
                this.moneyRcvdRptButton.Enabled = true;
                System.Windows.Forms.Application.DoEvents();
                Global.mnFrm.cmCde.showMsg(ex.Message + "\r\n\r\n" + ex.InnerException + "\r\n\r\n" + ex.StackTrace, 0);
            }
        }

        private void pymtsRcvdRptButton_Click(rptParamsDiag nwDiag)
        {
            try
            {

                // Create a new PDF document
                Graphics g = Graphics.FromHwnd(this.Handle);
                XPen aPen = new XPen(XColor.FromArgb(Color.Black), 1);
                PdfDocument document = new PdfDocument();
                document.Info.Title = "MONEY RECEIVED REPORT (PAYMENTS RECEIVED)";
                // Create first page for basic person details
                PdfPage page0 = document.AddPage();
                page0.Orientation = PageOrientation.Landscape;
                page0.Height = XUnit.FromInch(8.5);
                page0.Width = XUnit.FromInch(11);
                XGraphics gfx0 = XGraphics.FromPdfPage(page0);
                XFont xfont0 = new XFont("Verdana", 20, XFontStyle.BoldItalic);
                //gfx0.DrawString("Hello, World!" + this.locIDTextBox.Text, xfont0, XBrushes.Black,
                //new XRect(0, 0, page0.Width, page0.Height),
                //  XStringFormats.TopLeft);

                XFont xfont1 = new XFont("Times New Roman", 10.25f, XFontStyle.Underline | XFontStyle.Bold);
                XFont xfont11 = new XFont("Times New Roman", 10.25f, XFontStyle.Bold);
                XFont xfont2 = new XFont("Times New Roman", 10.25f, XFontStyle.Bold);
                XFont xfont4 = new XFont("Times New Roman", 10.0f, XFontStyle.Bold);
                XFont xfont41 = new XFont("Lucida Console", 10.0f);
                XFont xfont3 = new XFont("Lucida Console", 8.25f);
                XFont xfont31 = new XFont("Lucida Console", 10.5f, XFontStyle.Bold);
                XFont xfont5 = new XFont("Times New Roman", 6.0f, XFontStyle.Italic);

                Font font1 = new Font("Times New Roman", 10.25f, FontStyle.Underline | FontStyle.Bold);
                Font font11 = new Font("Times New Roman", 10.25f, FontStyle.Bold);
                Font font2 = new Font("Times New Roman", 10.25f, FontStyle.Bold);
                Font font4 = new Font("Times New Roman", 10.0f, FontStyle.Bold);
                Font font41 = new Font("Lucida Console", 10.0f);
                Font font3 = new Font("Lucida Console", 8.25f);
                Font font31 = new Font("Lucida Console", 10.5f, FontStyle.Bold);
                Font font5 = new Font("Times New Roman", 6.0f, FontStyle.Italic);

                float font1Hght = (float)gfx0.MeasureString("ABCDEFGHIJKLMNOP", xfont1).Height;
                float font2Hght = (float)gfx0.MeasureString("ABCDEFGHIJKLMNOP", xfont2).Height;
                float font3Hght = (float)gfx0.MeasureString("ABCDEFGHIJKLMNOP", xfont3).Height;
                float font4Hght = (float)gfx0.MeasureString("ABCDEFGHIJKLMNOP", xfont41).Height;
                float font5Hght = (float)gfx0.MeasureString("ABCDEFGHIJKLMNOP", xfont5).Height;

                float startX = 40;
                float startXNw = 40;
                float endX = 680;
                float startY = 40;
                float offsetY = 0;
                float ght = 0;

                float pageWidth = 760 - startX;//e.PageSettings.PrintableArea.Width;
                                               //float pageHeight = 590 - startX;// e.PageSettings.PrintableArea.Height;
                float txtwdth = pageWidth - startX;
                //Global.mnFrm.cmCde.showMsg(pageWidth.ToString(), 0);
                float gwdth = 0;
                //StringBuilder strPrnt = new StringBuilder();
                //strPrnt.AppendLine("Received From");
                string[] nwLn;
                int pageNo = 1;
                XImage img = (XImage)Global.mnFrm.cmCde.getDBImageFile(Global.mnFrm.cmCde.Org_id.ToString() + ".png", 0);
                float picWdth = 80.00F;
                float picHght = (float)(picWdth / img.PixelWidth) * (float)img.PixelHeight;
                if (pageNo == 1)
                { //Org Logo
                  //RectangleF srcRect = new Rectangle(0, 0, this.BackgroundImage.Width,
                  //BackgroundImage.Height);
                  //RectangleF destRect = new Rectangle(0, 0, nWidth, nHeight);
                  //Rectangle destRect = new Rectangle(0, 0, nWidth, nHeight);


                    gfx0.DrawImage(img, startX - 10, startY + offsetY - 15, picWdth, picHght);
                    //g.DrawImage(this.LargerImage, destRect, srcRect, GraphicsUnit.Pixel);

                    //Org Name
                    nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
                      Global.mnFrm.cmCde.getOrgName(Global.mnFrm.cmCde.Org_id),
                      pageWidth + 85, font2, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        gfx0.DrawString(nwLn[i]
                        , xfont2, XBrushes.Black, startX + picWdth + 5, startY + offsetY);
                        offsetY += font2Hght;
                    }

                    ght = (float)gfx0.MeasureString(
                      Global.mnFrm.cmCde.getOrgPstlAddrs(Global.mnFrm.cmCde.Org_id).Replace("\r\n", " ").Trim(), xfont2).Height;
                    //offsetY = offsetY + (int)ght;

                    //Pstal Address
                    XTextFormatter tf = new XTextFormatter(gfx0);
                    XRect rect = new XRect(startX + picWdth + 5, startY + offsetY - 7, pageWidth, ght);
                    gfx0.DrawRectangle(XBrushes.White, rect);
                    tf.DrawString(Global.mnFrm.cmCde.getOrgPstlAddrs(Global.mnFrm.cmCde.Org_id).Replace("\r\n", " ").Trim()
                      , xfont2, XBrushes.Black, rect, XStringFormats.TopLeft);
                    //gfx0.DrawString(,
                    //xfont2, XBrushes.Black, startX + picWdth, startY + offsetY);
                    offsetY += ght + 5;

                    //Contacts Nos
                    nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
               Global.mnFrm.cmCde.getOrgContactNos(Global.mnFrm.cmCde.Org_id),
               pageWidth, font2, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        gfx0.DrawString(nwLn[i]
                        , xfont2, XBrushes.Black, startX + picWdth + 5, startY + offsetY);
                        offsetY += font2Hght;
                    }
                    //Email Address
                    nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
               Global.mnFrm.cmCde.getOrgEmailAddrs(Global.mnFrm.cmCde.Org_id),
               pageWidth, font2, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        gfx0.DrawString(nwLn[i]
                        , xfont2, XBrushes.Black, startX + picWdth + 5, startY + offsetY);
                        offsetY += font2Hght;
                    }
                    offsetY += font2Hght;
                    if (offsetY < picHght)
                    {
                        offsetY = picHght;
                    }
                    gfx0.DrawLine(aPen, startX, startY + offsetY - 8, startX + endX,
               startY + offsetY - 8);

                }
                string orgType = Global.mnFrm.cmCde.getPssblValNm(int.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
            "org.org_details", "org_id", "org_typ_id", Global.mnFrm.cmCde.Org_id)));

                //Person Types
                float oldoffsetY = offsetY;
                float hgstOffsetY = 0;
                float hghstght = 0;

                DataSet dtst = Global.get_PymtsMoneyRcvd(long.Parse(nwDiag.createdByIDTextBox.Text),
                  nwDiag.docTypComboBox.Text, nwDiag.startDteTextBox.Text, nwDiag.endDteTextBox.Text,
                  Global.mnFrm.cmCde.Org_id, nwDiag.sortByComboBox.Text, nwDiag.useCreationDateCheckBox.Checked);

                oldoffsetY = offsetY;
                offsetY = oldoffsetY + 5;

                double invcAmnt = 0;
                double dscntAmnt = 0;
                double amntRcvd = 0;
                double outstndngAmnt = 0;

                startX = startXNw;
                string usrNm = "ALL AGENTS";
                if (long.Parse(nwDiag.createdByIDTextBox.Text) > 0)
                {
                    usrNm = Global.mnFrm.cmCde.getUsername(long.Parse(nwDiag.createdByIDTextBox.Text));
                }
                for (int a = 0; a < dtst.Tables[0].Rows.Count; a++)
                {
                    startX = startXNw;
                    if (a == 0)
                    {
                        hgstOffsetY = 0;
                        ght = (float)gfx0.MeasureString(
                        ("PAYMENTS RECEIVED BY " + usrNm + " (" + nwDiag.startDteTextBox.Text + " to " + nwDiag.endDteTextBox.Text + ")").ToUpper(), xfont2).Height;
                        //lblght = ght;
                        XTextFormatter tf = new XTextFormatter(gfx0);
                        XRect rect = new XRect(startX, startY + offsetY, pageWidth - startX, ght);
                        gfx0.DrawRectangle(XBrushes.LightGray, rect);
                        tf.DrawString(("PAYMENTS RECEIVED BY " + usrNm + " (" + nwDiag.startDteTextBox.Text + " to " + nwDiag.endDteTextBox.Text + ")").ToUpper()
                          , xfont2, XBrushes.Black, rect, XStringFormats.TopLeft);
                        offsetY += (int)ght + 5;
                        for (int j = 0; j < dtst.Tables[0].Columns.Count; j++)
                        {
                            if (dtst.Tables[0].Columns[j].Caption.StartsWith("mt") == false)
                            {
                                XSize sze = gfx0.MeasureString(
                             dtst.Tables[0].Columns[j].Caption, xfont2);
                                ght = (float)sze.Height;
                                float wdth = (float)sze.Width;
                                if (wdth < (float)(dtst.Tables[0].Columns[j].Caption.Length * 5))
                                {
                                    wdth = (float)(dtst.Tables[0].Columns[j].Caption.Length * 5);
                                }
                                tf = new XTextFormatter(gfx0);
                                rect = new XRect(startX, startY + offsetY, wdth + 5, ght);
                                gfx0.DrawRectangle(XBrushes.LightGray, rect);
                                tf.DrawString(" " + dtst.Tables[0].Columns[j].Caption
                                  , xfont2, XBrushes.Black, rect, XStringFormats.TopLeft);
                                startX += wdth + 10;
                            }
                        }
                        offsetY += (int)ght + 5;
                        startX = startXNw;
                    }
                    hghstght = 0;
                    for (int j = 0; j < dtst.Tables[0].Columns.Count; j++)
                    {
                        if (dtst.Tables[0].Columns[j].Caption.StartsWith("mt") == false)
                        {
                            XSize sze = gfx0.MeasureString(dtst.Tables[0].Columns[j].Caption, xfont2);
                            ght = (float)sze.Height;
                            float wdth = (float)(sze.Width);
                            if (wdth < (float)(dtst.Tables[0].Columns[j].Caption.Length * 5))
                            {
                                wdth = (float)(dtst.Tables[0].Columns[j].Caption.Length * 5);
                            }
                            string strToBreak = dtst.Tables[0].Rows[a][j].ToString();

                            if (j >= 2 && j <= 5)
                            {
                                double tst = 0;
                                if (double.TryParse(strToBreak, out tst) == false)
                                {
                                    strToBreak = "0";
                                }
                                strToBreak = double.Parse(strToBreak).ToString("#,##0.00");
                                if (j == 2)
                                {
                                    invcAmnt += double.Parse(strToBreak);
                                }
                                else if (j == 3)
                                {
                                    dscntAmnt += double.Parse(strToBreak);
                                }
                                else if (j == 4)
                                {
                                    amntRcvd += double.Parse(strToBreak);
                                }
                                else if (j == 5)
                                {
                                    outstndngAmnt += double.Parse(strToBreak);
                                }
                            }
                            nwLn = Global.mnFrm.cmCde.breakTxtDown(
                              strToBreak,
                              (int)(wdth * 1.64), font41, g);

                            string finlStr = "";
                            if (j >= 2 && j <= 5)
                            {
                                finlStr = string.Join("\n", nwLn).PadLeft(15);
                            }
                            else
                            {
                                finlStr = string.Join("\n", nwLn);
                            }
                            ght = (float)gfx0.MeasureString(
                           finlStr, xfont41).Height * 1.2F;

                            XTextFormatter tf = new XTextFormatter(gfx0);
                            XRect rect = new XRect(startX, startY + offsetY, wdth + 5, ght);
                            gfx0.DrawRectangle(XBrushes.White, rect);


                            tf.DrawString(finlStr
                              , xfont41, XBrushes.Black, rect, XStringFormats.TopLeft);

                            startX += wdth + 10;
                            if (hghstght < ght)
                            {
                                hghstght = ght;
                            }
                        }
                    }
                    if (hghstght < 10)
                    {
                        hghstght = 10;
                    }
                    offsetY += hghstght + 5;
                    if (hgstOffsetY < offsetY)
                    {
                        hgstOffsetY = offsetY;
                    }
                    if ((startY + offsetY) >= 580)
                    {
                        page0 = document.AddPage();
                        page0.Orientation = PageOrientation.Portrait;
                        page0.Height = XUnit.FromInch(8.5);
                        page0.Width = XUnit.FromInch(11);
                        gfx0 = XGraphics.FromPdfPage(page0);
                        offsetY = 0;
                        hgstOffsetY = 0;
                    }
                }

                //offsetY += hghstght + 5;
                offsetY += 5;
                hghstght = 0;
                startX = startXNw;
                for (int j = 0; j < dtst.Tables[0].Columns.Count; j++)
                {
                    if (dtst.Tables[0].Columns[j].Caption.StartsWith("mt") == false)
                    {
                        XSize sze = gfx0.MeasureString(dtst.Tables[0].Columns[j].Caption, xfont2);
                        ght = (float)sze.Height;
                        float wdth = (float)(sze.Width);
                        if (wdth < (float)(dtst.Tables[0].Columns[j].Caption.Length * 5))
                        {
                            wdth = (float)(dtst.Tables[0].Columns[j].Caption.Length * 5);
                        }
                        string strToBreak = " ";
                        if (j == 1)
                        {
                            strToBreak = "TOTALS = ";
                        }
                        if (j >= 2 && j <= 5)
                        {
                            if (j == 2)
                            {
                                strToBreak = "";// (invcAmnt).ToString("#,##0.00");
                            }
                            else if (j == 3)
                            {
                                strToBreak = "";// (dscntAmnt).ToString("#,##0.00");
                            }
                            else if (j == 4)
                            {
                                strToBreak = (amntRcvd).ToString("#,##0.00");
                            }
                            else if (j == 5)
                            {
                                strToBreak = "";// (outstndngAmnt).ToString("#,##0.00");
                            }
                        }
                        nwLn = Global.mnFrm.cmCde.breakTxtDown(
                          strToBreak,
                          (int)(wdth * 1.5), font31, g);

                        string finlStr = "";
                        if (j >= 2 && j <= 5)
                        {
                            finlStr = string.Join("\n", nwLn).PadLeft(15);
                        }
                        else
                        {
                            finlStr = string.Join("\n", nwLn);
                        }
                        ght = (float)gfx0.MeasureString(
                       finlStr, xfont31).Height;

                        XTextFormatter tf = new XTextFormatter(gfx0);
                        XRect rect = new XRect(startX, startY + offsetY, wdth + 5, ght);
                        gfx0.DrawRectangle(XBrushes.White, rect);


                        tf.DrawString(finlStr
                          , xfont31, XBrushes.Black, rect, XStringFormats.TopLeft);

                        startX += wdth + 10;
                        if (hghstght < ght)
                        {
                            hghstght = ght;
                        }
                    }
                }
                offsetY += hghstght + 5;
                //Slogan: 
                startX = startXNw;
                offsetY = 535;
                gfx0.DrawLine(aPen, startX, startY + offsetY, startX + endX,
            startY + offsetY);
                offsetY += font3Hght;
                nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
                  Global.mnFrm.cmCde.getOrgName(Global.mnFrm.cmCde.Org_id) + "..." +
                  Global.mnFrm.cmCde.getOrgSlogan(Global.mnFrm.cmCde.Org_id),
            pageWidth - ght, font5, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    gfx0.DrawString(nwLn[i]
                    , xfont5, XBrushes.Black, startX, startY + offsetY);
                    offsetY += font5Hght;
                }
                offsetY += font5Hght;
                nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
                 "Software Developed by Rhomicom Systems Technologies Ltd.",
            pageWidth + 40, font5, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    gfx0.DrawString(nwLn[i]
                    , xfont5, XBrushes.Black, startX, startY + offsetY);
                    offsetY += font5Hght;
                }
                nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
            "Website:www.rhomicomgh.com",
            pageWidth + 40, font5, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    gfx0.DrawString(nwLn[i]
                    , xfont5, XBrushes.Black, startX, startY + offsetY);
                    offsetY += font5Hght;
                }
                // Create second page for additional person details
                /*PdfPage page1 = document.AddPage();
                XGraphics gfx1 = XGraphics.FromPdfPage(page1);
                XFont xfont1 = new XFont("Verdana", 20, XFontStyle.BoldItalic);
                gfx0.DrawString("Page 2!" + this.locIDTextBox.Text, xfont1, XBrushes.Black,
                  new XRect(100, 100, page1.Width, page1.Height),
                  XStringFormats.TopLeft);*/



                // Save the document...
                string filename = Global.mnFrm.cmCde.getRptDrctry() + @"\SalesMoneyRcvd_" + Global.mnFrm.cmCde.getDB_Date_time().Replace(" ", "").Replace(":", "").Replace("-", "") + ".pdf";
                document.Save(filename);
                // ...and start a viewer.
                System.Diagnostics.Process.Start(filename);
                this.itemsSoldPdfButton.Enabled = true;
                //Global.mnFrm.cmCde.upldImgsFTP(9, Global.mnFrm.cmCde.getRptDrctry(), @"\SalesMoneyRcvd_" + Global.mnFrm.cmCde.getDB_Date_time().Replace(" ", "").Replace(":", "").Replace("-", "") + ".pdf");
                System.Windows.Forms.Application.DoEvents();
            }
            catch (Exception ex)
            {
                this.itemsSoldPdfButton.Enabled = true;
                System.Windows.Forms.Application.DoEvents();
                Global.mnFrm.cmCde.showMsg(ex.Message + "\r\n\r\n" + ex.InnerException + "\r\n\r\n" + ex.StackTrace, 0);
            }
        }

        private void itemsSoldPdfButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.itemsSoldPdfButton.Enabled = false;
                System.Windows.Forms.Application.DoEvents();
                rptParamsDiag nwDiag = new rptParamsDiag();
                nwDiag.startDteTextBox.Text = Global.mnFrm.cmCde.getFrmtdDB_Date_time().Substring(0, 11) + " 00:00:00";
                nwDiag.endDteTextBox.Text = Global.mnFrm.cmCde.getFrmtdDB_Date_time().Substring(0, 11) + " 23:59:59";
                nwDiag.sortByComboBox.Items.Clear();

                nwDiag.sortByComboBox.Items.Add("None");
                //nwDiag.sortByComboBox.Items.Add("QTY");
                nwDiag.sortByComboBox.Items.Add("TOTAL AMOUNT");
                nwDiag.sortByComboBox.Items.Add("OUTSTANDING AMOUNT");
                nwDiag.sortByComboBox.SelectedItem = "TOTAL AMOUNT";
                nwDiag.rptComboBox.SelectedIndex = 0;

                if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[72]) == true)
                {
                    nwDiag.createdByTextBox.Text = Global.mnFrm.cmCde.getUsername(Global.mnFrm.cmCde.User_id);
                    nwDiag.createdByIDTextBox.Text = Global.mnFrm.cmCde.User_id.ToString();
                    nwDiag.createdByTextBox.Enabled = false;
                    nwDiag.createdByIDTextBox.Enabled = false;
                    nwDiag.createdByButton.Enabled = false;
                    nwDiag.useCreationDateCheckBox.Checked = true;
                    nwDiag.useCreationDateCheckBox.Enabled = false;
                }
                else
                {
                    nwDiag.createdByTextBox.Text = "";
                    nwDiag.createdByIDTextBox.Text = "-1";
                    nwDiag.useCreationDateCheckBox.Checked = true;
                    nwDiag.useCreationDateCheckBox.Enabled = true;
                }

                if (nwDiag.ShowDialog() == DialogResult.Cancel)
                {
                    this.itemsSoldPdfButton.Enabled = true;
                    System.Windows.Forms.Application.DoEvents();
                    return;
                }

                if (nwDiag.rptComboBox.Text == "Money Received Report (Documents Created)")
                {
                    this.pdfRptButton_Click(nwDiag);
                    this.itemsSoldPdfButton.Enabled = true;
                    System.Windows.Forms.Application.DoEvents();
                    return;
                }
                else if (nwDiag.rptComboBox.Text == "Money Received Report (Payments Received)")
                {
                    this.pymtsRcvdRptButton_Click(nwDiag);
                    this.itemsSoldPdfButton.Enabled = true;
                    System.Windows.Forms.Application.DoEvents();
                    return;
                }

                // Create a new PDF document
                Graphics g = Graphics.FromHwnd(this.Handle);
                XPen aPen = new XPen(XColor.FromArgb(Color.Black), 1);
                PdfDocument document = new PdfDocument();
                document.Info.Title = "ITEM ISSUES/SALES REPORT";
                // Create first page for basic person details
                PdfPage page0 = document.AddPage();
                page0.Orientation = PageOrientation.Portrait;
                page0.Height = XUnit.FromInch(11);
                page0.Width = XUnit.FromInch(8.5);
                XGraphics gfx0 = XGraphics.FromPdfPage(page0);
                XFont xfont0 = new XFont("Verdana", 20, XFontStyle.BoldItalic);
                //gfx0.DrawString("Hello, World!" + this.locIDTextBox.Text, xfont0, XBrushes.Black,
                //new XRect(0, 0, page0.Width, page0.Height),
                //  XStringFormats.TopLeft);

                XFont xfont1 = new XFont("Times New Roman", 10.25f, XFontStyle.Underline | XFontStyle.Bold);
                XFont xfont11 = new XFont("Times New Roman", 10.25f, XFontStyle.Bold);
                XFont xfont2 = new XFont("Times New Roman", 10.25f, XFontStyle.Bold);
                XFont xfont4 = new XFont("Times New Roman", 10.0f, XFontStyle.Bold);
                XFont xfont41 = new XFont("Lucida Console", 10.0f);
                XFont xfont3 = new XFont("Lucida Console", 8.25f);
                XFont xfont31 = new XFont("Lucida Console", 10.5f, XFontStyle.Bold);
                XFont xfont5 = new XFont("Times New Roman", 6.0f, XFontStyle.Italic);

                Font font1 = new Font("Times New Roman", 10.25f, FontStyle.Underline | FontStyle.Bold);
                Font font11 = new Font("Times New Roman", 10.25f, FontStyle.Bold);
                Font font2 = new Font("Times New Roman", 10.25f, FontStyle.Bold);
                Font font4 = new Font("Times New Roman", 10.0f, FontStyle.Bold);
                Font font41 = new Font("Lucida Console", 10.0f);
                Font font3 = new Font("Lucida Console", 8.25f);
                Font font31 = new Font("Lucida Console", 10.5f, FontStyle.Bold);
                Font font5 = new Font("Times New Roman", 6.0f, FontStyle.Italic);

                float font1Hght = (float)gfx0.MeasureString("ABCDEFGHIJKLMNOP", xfont1).Height;
                float font2Hght = (float)gfx0.MeasureString("ABCDEFGHIJKLMNOP", xfont2).Height;
                float font3Hght = (float)gfx0.MeasureString("ABCDEFGHIJKLMNOP", xfont3).Height;
                float font4Hght = (float)gfx0.MeasureString("ABCDEFGHIJKLMNOP", xfont41).Height;
                float font5Hght = (float)gfx0.MeasureString("ABCDEFGHIJKLMNOP", xfont5).Height;

                float startX = 25;
                float startXNw = 25;
                float endX = 560;
                float startY = 40;
                float offsetY = 0;
                float ght = 0;

                float pageWidth = 590 - startX;//e.PageSettings.PrintableArea.Width;
                                               //float pageHeight = 760 - 40;// e.PageSettings.PrintableArea.Height;
                float txtwdth = pageWidth - startX;
                //Global.mnFrm.cmCde.showMsg(pageWidth.ToString(), 0);
                //float gwdth = 0;
                //StringBuilder strPrnt = new StringBuilder();
                //strPrnt.AppendLine("Received From");
                string[] nwLn;
                int pageNo = 1;
                XImage img = (XImage)Global.mnFrm.cmCde.getDBImageFile(Global.mnFrm.cmCde.Org_id.ToString() + ".png", 0);
                float picWdth = 80.00F;
                float picHght = (float)(picWdth / img.PixelWidth) * (float)img.PixelHeight;
                if (pageNo == 1)
                { //Org Logo
                  //RectangleF srcRect = new Rectangle(0, 0, this.BackgroundImage.Width,
                  //BackgroundImage.Height);
                  //RectangleF destRect = new Rectangle(0, 0, nWidth, nHeight);
                  //Rectangle destRect = new Rectangle(0, 0, nWidth, nHeight);
                    gfx0.DrawImage(img, startX - 10, startY + offsetY - 15, picWdth, picHght);
                    //g.DrawImage(this.LargerImage, destRect, srcRect, GraphicsUnit.Pixel);
                    //Org Name
                    nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
                      Global.mnFrm.cmCde.getOrgName(Global.mnFrm.cmCde.Org_id),
                      pageWidth, font2, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        gfx0.DrawString(nwLn[i]
                        , xfont2, XBrushes.Black, startX + picWdth + 5, startY + offsetY);
                        offsetY += font2Hght;
                    }
                    ght = (float)gfx0.MeasureString(
                      Global.mnFrm.cmCde.getOrgPstlAddrs(Global.mnFrm.cmCde.Org_id).Replace("\r\n", " ").Trim(), xfont2).Height;
                    //offsetY = offsetY + (int)ght;
                    //Pstal Address
                    XTextFormatter tf = new XTextFormatter(gfx0);
                    XRect rect = new XRect(startX + picWdth + 5, startY + offsetY - 7, pageWidth, ght);
                    gfx0.DrawRectangle(XBrushes.White, rect);
                    tf.DrawString(Global.mnFrm.cmCde.getOrgPstlAddrs(Global.mnFrm.cmCde.Org_id).Replace("\r\n", " ").Trim()
                      , xfont2, XBrushes.Black, rect, XStringFormats.TopLeft);
                    //gfx0.DrawString(,
                    //xfont2, XBrushes.Black, startX + picWdth, startY + offsetY);
                    offsetY += ght + 5;

                    //Contacts Nos
                    nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
               Global.mnFrm.cmCde.getOrgContactNos(Global.mnFrm.cmCde.Org_id),
               pageWidth, font2, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        gfx0.DrawString(nwLn[i]
                        , xfont2, XBrushes.Black, startX + picWdth + 5, startY + offsetY);
                        offsetY += font2Hght;
                    }
                    //Email Address
                    nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
               Global.mnFrm.cmCde.getOrgEmailAddrs(Global.mnFrm.cmCde.Org_id),
               pageWidth, font2, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        gfx0.DrawString(nwLn[i]
                        , xfont2, XBrushes.Black, startX + picWdth + 5, startY + offsetY);
                        offsetY += font2Hght;
                    }
                    offsetY += font2Hght;
                    if (offsetY < picHght)
                    {
                        offsetY = picHght;
                    }
                    gfx0.DrawLine(aPen, startX, startY + offsetY - 8, startX + endX,
               startY + offsetY - 8);

                }
                string orgType = Global.mnFrm.cmCde.getPssblValNm(int.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
            "org.org_details", "org_id", "org_typ_id", Global.mnFrm.cmCde.Org_id)));

                //Person Types
                float oldoffsetY = offsetY;
                float hgstOffsetY = 0;
                float hghstght = 0;

                DataSet dtst = Global.get_ItemsSold(long.Parse(nwDiag.createdByIDTextBox.Text),
                  nwDiag.docTypComboBox.Text, nwDiag.startDteTextBox.Text, nwDiag.endDteTextBox.Text,
                  Global.mnFrm.cmCde.Org_id, nwDiag.sortByComboBox.Text);

                oldoffsetY = offsetY;
                offsetY = oldoffsetY + 5;

                double ttlAmnt = 0;

                startX = startXNw;
                string usrNm = "ALL AGENTS";
                if (long.Parse(nwDiag.createdByIDTextBox.Text) > 0)
                {
                    usrNm = Global.mnFrm.cmCde.getUsername(long.Parse(nwDiag.createdByIDTextBox.Text));
                }

                for (int a = 0; a < dtst.Tables[0].Rows.Count; a++)
                {
                    startX = startXNw;
                    if (a == 0)
                    {
                        hgstOffsetY = 0;
                        ght = (float)gfx0.MeasureString(
                        (nwDiag.docTypComboBox.Text + " BY " + usrNm +
                        " (" + nwDiag.startDteTextBox.Text + " to " + nwDiag.endDteTextBox.Text + ")").ToUpper(), xfont2).Height;
                        //lblght = ght;
                        XTextFormatter tf = new XTextFormatter(gfx0);
                        XRect rect = new XRect(startX, startY + offsetY, pageWidth, ght);
                        gfx0.DrawRectangle(XBrushes.LightGray, rect);
                        tf.DrawString((nwDiag.docTypComboBox.Text + " BY " + usrNm +
                        " (" + nwDiag.startDteTextBox.Text + " to " + nwDiag.endDteTextBox.Text + ")").ToUpper()
                          , xfont2, XBrushes.Black, rect, XStringFormats.TopLeft);
                        offsetY += (int)ght + 5;
                        for (int j = 0; j < dtst.Tables[0].Columns.Count; j++)
                        {
                            if (dtst.Tables[0].Columns[j].Caption.StartsWith("mt") == false)
                            {
                                XSize sze = gfx0.MeasureString(
                             dtst.Tables[0].Columns[j].Caption, xfont2);
                                ght = (float)sze.Height;
                                float wdth = (float)sze.Width;
                                if (wdth < (float)(dtst.Tables[0].Columns[j].Caption.Length * 5))
                                {
                                    wdth = (float)(dtst.Tables[0].Columns[j].Caption.Length * 5);
                                }
                                tf = new XTextFormatter(gfx0);
                                rect = new XRect(startX, startY + offsetY, wdth + 5, ght);
                                gfx0.DrawRectangle(XBrushes.LightGray, rect);
                                tf.DrawString(" " + dtst.Tables[0].Columns[j].Caption
                                  , xfont2, XBrushes.Black, rect, XStringFormats.TopLeft);
                                startX += wdth + 10;
                            }
                        }
                        offsetY += (int)ght + 5;
                        startX = startXNw;
                    }
                    hghstght = 0;
                    for (int j = 0; j < dtst.Tables[0].Columns.Count; j++)
                    {
                        if (dtst.Tables[0].Columns[j].Caption.StartsWith("mt") == false)
                        {
                            XSize sze = gfx0.MeasureString(dtst.Tables[0].Columns[j].Caption, xfont2);
                            ght = (float)sze.Height;
                            float wdth = (float)(sze.Width);
                            if (wdth < (float)(dtst.Tables[0].Columns[j].Caption.Length * 5))
                            {
                                wdth = (float)(dtst.Tables[0].Columns[j].Caption.Length * 5);
                            }
                            string strToBreak = dtst.Tables[0].Rows[a][j].ToString();

                            if (j == 6 || j == 5 || j == 3)
                            {
                                double tst = 0;
                                if (double.TryParse(strToBreak, out tst) == false)
                                {
                                    strToBreak = "0";
                                }
                                strToBreak = double.Parse(strToBreak).ToString("#,##0.00");
                                if (j == 6)
                                {
                                    ttlAmnt += double.Parse(strToBreak);
                                }
                            }

                            nwLn = Global.mnFrm.cmCde.breakTxtDown(
                     strToBreak, (int)(wdth * 1.3), font41, g);
                            //    if (j == 1 || j == 2)
                            //    {
                            //      nwLn = Global.mnFrm.cmCde.breakPOSTxtDown(strToBreak,
                            //(int)(wdth * 1.2), font41, g, 14);
                            //    }
                            //    else
                            //    {
                            //    }
                            string finlStr = "";
                            if (j == 6 || j == 5 || j == 3)
                            {
                                if (j == 3)
                                {
                                    finlStr = string.Join("\n", nwLn).PadLeft(8);
                                }
                                else if (j == 6)
                                {
                                    finlStr = string.Join("\n", nwLn).PadLeft(13);
                                }
                                else
                                {
                                    finlStr = string.Join("\n", nwLn).PadLeft(12);
                                }
                            }
                            else
                            {
                                finlStr = string.Join("\n", nwLn);
                            }
                            ght = (float)gfx0.MeasureString(
                           finlStr, xfont41).Height;

                            XTextFormatter tf = new XTextFormatter(gfx0);
                            XRect rect = new XRect(startX, startY + offsetY, wdth + 5, ght);
                            gfx0.DrawRectangle(XBrushes.White, rect);


                            tf.DrawString(finlStr
                              , xfont41, XBrushes.Black, rect, XStringFormats.TopLeft);

                            startX += wdth + 10;
                            if (hghstght < ght)
                            {
                                hghstght = ght;
                            }
                        }
                    }
                    if (hghstght < 10)
                    {
                        hghstght = 10;
                    }
                    offsetY += hghstght + 5;
                    if (hgstOffsetY < offsetY)
                    {
                        hgstOffsetY = offsetY;
                    }
                    if ((startY + offsetY) >= 750)
                    {
                        page0 = document.AddPage();
                        page0.Orientation = PageOrientation.Portrait;
                        page0.Height = XUnit.FromInch(11);
                        page0.Width = XUnit.FromInch(8.5);
                        gfx0 = XGraphics.FromPdfPage(page0);
                        offsetY = 0;
                        hgstOffsetY = 0;
                    }
                }

                //offsetY += hghstght + 5;
                offsetY += 5;

                hghstght = 0;
                startX = startXNw;
                for (int j = 0; j < dtst.Tables[0].Columns.Count; j++)
                {
                    if (dtst.Tables[0].Columns[j].Caption.StartsWith("mt") == false)
                    {
                        XSize sze = gfx0.MeasureString(dtst.Tables[0].Columns[j].Caption, xfont2);
                        ght = (float)sze.Height;
                        float wdth = (float)(sze.Width);
                        if (wdth < (float)(dtst.Tables[0].Columns[j].Caption.Length * 5))
                        {
                            wdth = (float)(dtst.Tables[0].Columns[j].Caption.Length * 5);
                        }
                        string strToBreak = " ";
                        if (j == 5)
                        {
                            strToBreak = "TOTALS = ";
                        }
                        if (j == 6 || j == 5 || j == 3)
                        {
                            if (j == 6)
                            {
                                strToBreak = (ttlAmnt).ToString("#,##0.00");
                            }
                        }
                        nwLn = Global.mnFrm.cmCde.breakPDFTxtDown(
                          strToBreak,
                          (int)(wdth * 1.8), font31, g);

                        string finlStr = "";
                        if (j == 6 || j == 5 || j == 3)
                        {
                            if (j == 3)
                            {
                                finlStr = string.Join("\n", nwLn).PadLeft(5);
                            }
                            else if (j == 6)
                            {
                                finlStr = string.Join("\n", nwLn).PadLeft(15);
                            }
                            else
                            {
                                finlStr = string.Join("\n", nwLn).PadLeft(12);
                            }
                        }
                        else
                        {
                            finlStr = string.Join("\n", nwLn);
                        }
                        ght = (float)gfx0.MeasureString(
                       finlStr, xfont31).Height;

                        XTextFormatter tf = new XTextFormatter(gfx0);
                        XRect rect = new XRect(startX, startY + offsetY, wdth + 5, ght);
                        gfx0.DrawRectangle(XBrushes.White, rect);


                        tf.DrawString(finlStr
                          , xfont31, XBrushes.Black, rect, XStringFormats.TopLeft);

                        startX += wdth + 10;
                        if (hghstght < ght)
                        {
                            hghstght = ght;
                        }
                    }
                }
                offsetY += hghstght + 5;
                //Slogan: 
                startX = startXNw;
                offsetY = 705;
                gfx0.DrawLine(aPen, startX, startY + offsetY, startX + endX,
            startY + offsetY);
                offsetY += font3Hght;
                nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
                  Global.mnFrm.cmCde.getOrgName(Global.mnFrm.cmCde.Org_id) + "..." +
                  Global.mnFrm.cmCde.getOrgSlogan(Global.mnFrm.cmCde.Org_id),
            pageWidth - ght, font5, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    gfx0.DrawString(nwLn[i]
                    , xfont5, XBrushes.Black, startX, startY + offsetY);
                    offsetY += font5Hght;
                }
                offsetY += font5Hght;
                nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
                 "Software Developed by Rhomicom Systems Technologies Ltd.",
            pageWidth + 40, font5, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    gfx0.DrawString(nwLn[i]
                    , xfont5, XBrushes.Black, startX, startY + offsetY);
                    offsetY += font5Hght;
                }
                nwLn = Global.mnFrm.cmCde.breakRptTxtDown(
            "Website:www.rhomicomgh.com",
            pageWidth + 40, font5, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    gfx0.DrawString(nwLn[i]
                    , xfont5, XBrushes.Black, startX, startY + offsetY);
                    offsetY += font5Hght;
                }
                // Create second page for additional person details
                /*PdfPage page1 = document.AddPage();
                XGraphics gfx1 = XGraphics.FromPdfPage(page1);
                XFont xfont1 = new XFont("Verdana", 20, XFontStyle.BoldItalic);
                gfx0.DrawString("Page 2!" + this.locIDTextBox.Text, xfont1, XBrushes.Black,
                  new XRect(100, 100, page1.Width, page1.Height),
                  XStringFormats.TopLeft);*/
                // Save the document...
                string filename = Global.mnFrm.cmCde.getRptDrctry() + @"\ItemsSold_" + Global.mnFrm.cmCde.getDB_Date_time().Replace(" ", "").Replace(":", "").Replace("-", "") + ".pdf";
                document.Save(filename);
                // ...and start a viewer.
                System.Diagnostics.Process.Start(filename);
                this.itemsSoldPdfButton.Enabled = true;
                //Global.mnFrm.cmCde.upldImgsFTP(9, Global.mnFrm.cmCde.getRptDrctry(), @"\ItemsSold_" + Global.mnFrm.cmCde.getDB_Date_time().Replace(" ", "").Replace(":", "").Replace("-", "") + ".pdf");
                System.Windows.Forms.Application.DoEvents();
            }
            catch (Exception ex)
            {
                this.itemsSoldPdfButton.Enabled = true;
                System.Windows.Forms.Application.DoEvents();
                Global.mnFrm.cmCde.showMsg(ex.Message + "\r\n\r\n" + ex.InnerException + "\r\n\r\n" + ex.StackTrace, 0);
            }
        }

        private void searchForTextBox_Click(object sender, EventArgs e)
        {
            this.searchForTextBox.SelectAll();
        }

        private void searchForTextBox_Enter(object sender, EventArgs e)
        {
            this.searchForTextBox.SelectAll();
        }

        private void resetTrnsButton_Click(object sender, EventArgs e)
        {
            Global.mnFrm.cmCde.minimizeMemory();
            this.searchInComboBox.SelectedIndex = 4;
            this.searchForTextBox.Text = "%";
            this.dsplySizeComboBox.Text = Global.mnFrm.cmCde.get_CurPlcy_Mx_Dsply_Recs().ToString();
            this.rec_cur_indx = 0;
            this.obey_evnts = false;
            this.showUnpaidCheckBox.Checked = false;
            this.obey_evnts = true;

            this.rfrshButton_Click(this.rfrshButton, e);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                Object[] myargs = (Object[])e.Argument;
                worker.ReportProgress(10);

                long docHdrID = long.Parse((string)myargs[0]);
                string dateStr = (string)myargs[1];
                string doctype = (string)myargs[2];
                string docNum = (string)myargs[3];
                long srcDocID = long.Parse((string)myargs[4]);
                int invcCurrID = int.Parse((string)myargs[5]);
                decimal exchRate = decimal.Parse((string)myargs[6]);
                string srcDocType = (string)myargs[7];
                string cstmrNm = (string)myargs[8];
                string docDesc = (string)myargs[9];
                //string dateStr = Global.mnFrm.cmCde.getFrmtdDB_Date_time();
                DataSet dtst = Global.get_One_SalesDcLines(docHdrID);
                int ttl = dtst.Tables[0].Rows.Count;

                Global.deleteScmRcvblsDocDet(docHdrID);
                Global.deleteDocGLInfcLns(docHdrID, doctype);
                this.rvrsImprtdIntrfcTrns(docHdrID, doctype);

                for (int i = 0; i < ttl; i++)
                {
                    //Check if Doc Ln Rec Exists
                    //Create if not else update
                    int itmID = int.Parse(dtst.Tables[0].Rows[i][1].ToString());
                    string itmDesc = dtst.Tables[0].Rows[i][17].ToString() + " (" + dtst.Tables[0].Rows[i][2].ToString() + " " +
                      dtst.Tables[0].Rows[i][18].ToString() + ")";
                    int storeID = int.Parse(dtst.Tables[0].Rows[i][5].ToString());
                    int crncyID = int.Parse(dtst.Tables[0].Rows[i][6].ToString());
                    long srclnID = long.Parse(dtst.Tables[0].Rows[i][8].ToString());
                    double qty = double.Parse(dtst.Tables[0].Rows[i][2].ToString());
                    double price = double.Parse(dtst.Tables[0].Rows[i][3].ToString());
                    long lineid = long.Parse(dtst.Tables[0].Rows[i][0].ToString());
                    int taxID = int.Parse(dtst.Tables[0].Rows[i][9].ToString());
                    int dscntID = int.Parse(dtst.Tables[0].Rows[i][10].ToString());
                    int chrgeID = int.Parse(dtst.Tables[0].Rows[i][11].ToString());
                    string slctdAccntIDs = dtst.Tables[0].Rows[i][27].ToString();
                    char[] w = { ',' };
                    string[] inbrghtIDs = slctdAccntIDs.Split(w);
                    int itmInvAcntID = -1;
                    int.TryParse(Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_itm_subinventories", "subinv_id", "inv_asset_acct_id", storeID), out itmInvAcntID);
                    int cogsID = -1;
                    int salesRevID = -1;
                    int salesRetID = -1;
                    int purcRetID = -1;
                    int expnsID = -1;
                    for (int z = 0; z < inbrghtIDs.Length; z++)
                    {
                        switch (z)
                        {
                            case 0:
                                cogsID = int.Parse(inbrghtIDs[z]);
                                break;
                            case 1:
                                salesRevID = int.Parse(inbrghtIDs[z]);
                                break;
                            case 2:
                                salesRetID = int.Parse(inbrghtIDs[z]);
                                break;
                            case 3:
                                purcRetID = int.Parse(inbrghtIDs[z]);
                                break;
                            case 4:
                                expnsID = int.Parse(inbrghtIDs[z]);
                                break;
                        }
                    }
                    if (itmInvAcntID <= 0)
                    {
                        itmInvAcntID = this.dfltInvAcntID;
                    }
                    if (cogsID <= 0)
                    {
                        cogsID = this.dfltCGSAcntID;
                    }
                    if (salesRevID <= 0)
                    {
                        salesRevID = this.dfltRvnuAcntID;
                    }
                    if (salesRetID <= 0)
                    {
                        salesRetID = this.dfltSRAcntID;
                    }
                    if (expnsID <= 0)
                    {
                        expnsID = this.dfltExpnsAcntID;
                    }
                    //double orgnlSllngPrce = Math.Round((double)exchRate * Global.getUOMPriceLsTx(itmID, qty), 4);
                    double orgnlSllngPrce = double.Parse(dtst.Tables[0].Rows[i][14].ToString());
                    string itmType = dtst.Tables[0].Rows[i][28].ToString();
                    long stckID = Global.getItemStockID(itmID, storeID);
                    string cnsgmntIDs = dtst.Tables[0].Rows[i][13].ToString();
                    //MessageBox.Show(itmID + "|" + slctdAccntIDs);
                    if (itmID > 0)
                    {
                        this.generateItmAccntng(itmID, qty, cnsgmntIDs, taxID, dscntID, chrgeID,
                            doctype, docHdrID,
                            srcDocID, this.dfltRcvblAcntID, itmInvAcntID,
                            cogsID, expnsID, salesRevID, stckID,
                            price, crncyID, lineid, salesRetID, this.dfltCashAcntID,
                            this.dfltCheckAcntID, srclnID, dateStr, docNum,
                            invcCurrID, exchRate, this.dfltLbltyAccnt, srcDocType, cstmrNm,
                            docDesc, itmDesc, storeID, itmType, orgnlSllngPrce);
                    }
                }
                if (this.autoBalscheckBox.Checked)
                {
                    this.autoBals(doctype);
                }

                worker.ReportProgress(70);

                long rcvblDocID = Global.get_ScmRcvblsDocHdrID(docHdrID,
            doctype, Global.mnFrm.cmCde.Org_id);

                string rcvblDocNum = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
                  "rcvbls_invc_hdr_id", "rcvbls_invc_number", rcvblDocID);

                string rcvblDocType = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
                  "rcvbls_invc_hdr_id", "rcvbls_invc_type", rcvblDocID);

                Global.deleteRcvblsDocDetails(rcvblDocID, rcvblDocNum);

                this.checkNCreateRcvblLines(docHdrID, rcvblDocID, rcvblDocNum, rcvblDocType);

                worker.ReportProgress(100);
            }
            catch (Exception ex)
            {
                Global.mnFrm.cmCde.showMsg(ex.Message + "\r\n\r\n" + ex.InnerException + "\r\n\r\n" + ex.StackTrace, 4);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            System.Windows.Forms.Application.DoEvents();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
            }
            else if (e.Error != null)
            {
                Global.mnFrm.cmCde.showMsg("Error: " + e.Error.Message, 4);
            }

            System.Windows.Forms.Application.DoEvents();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            //try
            //{
            BackgroundWorker worker = sender as BackgroundWorker;
            Object[] myargs = (Object[])e.Argument;
            worker.ReportProgress(10);

            long docHdrID = long.Parse((string)myargs[0]);
            string dateStr = (string)myargs[1];
            string doctype = (string)myargs[2];
            string docNum = (string)myargs[3];
            long srcDocID = long.Parse((string)myargs[4]);
            int invcCurrID = int.Parse((string)myargs[5]);
            decimal exchRate = decimal.Parse((string)myargs[6]);
            string srcDocType = (string)myargs[7];

            //string dateStr = Global.mnFrm.cmCde.getFrmtdDB_Date_time();
            DataSet dtst = Global.get_One_SalesDcLines(docHdrID);
            int ttl = dtst.Tables[0].Rows.Count;
            worker.ReportProgress(10);

            for (int i = 0; i < ttl; i++)
            {
                //System.Windows.Forms.Application.DoEvents();

                bool isdlvrd = Global.mnFrm.cmCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][19].ToString());
                int itmID = int.Parse(dtst.Tables[0].Rows[i][1].ToString());
                int storeID = int.Parse(dtst.Tables[0].Rows[i][5].ToString());
                int crncyID = int.Parse(dtst.Tables[0].Rows[i][6].ToString());
                long srclnID = long.Parse(dtst.Tables[0].Rows[i][8].ToString());
                double qty = double.Parse(dtst.Tables[0].Rows[i][2].ToString());
                double price = double.Parse(dtst.Tables[0].Rows[i][3].ToString());
                long lineid = long.Parse(dtst.Tables[0].Rows[i][0].ToString());
                int taxID = int.Parse(dtst.Tables[0].Rows[i][9].ToString());
                int dscntID = int.Parse(dtst.Tables[0].Rows[i][10].ToString());
                int chrgeID = int.Parse(dtst.Tables[0].Rows[i][11].ToString());
                /*double.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
                  "inv.inv_itm_list", "item_id", "orgnl_selling_price", itmID))*/
                double orgnlSllngPrce = Math.Round((double)exchRate * Global.getUOMPriceLsTx(itmID, qty), 4);

                long stckID = Global.getItemStockID(itmID, storeID);
                string cnsgmntIDs = dtst.Tables[0].Rows[i][13].ToString();
                if (itmID > 0 && storeID > 0 && isdlvrd == false)
                {
                    this.udateItemBalances(itmID, qty, cnsgmntIDs, taxID, dscntID, chrgeID,
                        doctype, docHdrID,
                       srcDocID, dfltRcvblAcntID, dfltInvAcntID,
                        dfltCGSAcntID, dfltExpnsAcntID, dfltRvnuAcntID, stckID,
                        price, curid, lineid, dfltSRAcntID, dfltCashAcntID,
                        dfltCheckAcntID, srclnID, dateStr, docNum,
                        invcCurrID, exchRate, dfltLbltyAccnt, srcDocType);
                    Global.updateSalesLnDlvrd(lineid, true);
                }
                else if (isdlvrd == false && lineid > 0)
                {
                    Global.updateSalesLnDlvrd(lineid, true);
                }
            }
            worker.ReportProgress(100);
            //}
            //catch (Exception ex)
            //{
            //    Global.mnFrm.cmCde.showMsg(ex.Message + "\r\n\r\n" + ex.InnerException + "\r\n\r\n" + ex.StackTrace, 4);
            //}

        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            System.Windows.Forms.Application.DoEvents();
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
            }
            else if (e.Error != null)
            {
                Global.mnFrm.cmCde.showMsg("Error: " + e.Error.Message, 4);
            }

            System.Windows.Forms.Application.DoEvents();
        }

        private void itemsDataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e == null
              || this.obey_evnts == false)
            {
                return;
            }

            if (e.RowIndex < 0
              || e.ColumnIndex < 0)
            {
                return;
            }
            if (this.addRec == false && this.editRec == false)
            {
                return;
            }
            //this.obey_evnts = false;

            if (e.ColumnIndex == 5 && this.qtyChnged == true)
            {
                this.qtyChnged = false;
                SendKeys.Send("{DOWN}");
                SendKeys.Send("{HOME}");
                //System.Windows.Forms.Application.DoEvents();
                //this.itemsDataGridView.BeginEdit(true);
            }
            else if (e.ColumnIndex == 1 && this.itmChnged == true)
            {
                this.itmChnged = false;
                SendKeys.Send("{TAB}");
                //SendKeys.Send("{TAB}");
                //SendKeys.Send("{TAB}");
                //System.Windows.Forms.Application.DoEvents();
                //this.itemsDataGridView.BeginEdit(true);
            }
            else if (e.ColumnIndex == 3 && this.itmChnged == true)
            {
                this.itmChnged = false;
                SendKeys.Send("{TAB}");
            }
            else if (e.ColumnIndex == 7)
            {
                this.enblPriceEdit(e.RowIndex);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.timer2.Enabled = false;
            System.Windows.Forms.Application.DoEvents();
            SendKeys.Send("{DOWN}");
            SendKeys.Send("{HOME}");
            //SendKeys.Send("{HOME}");
            if (this.itemsDataGridView.CurrentCell != null)
            {
                int rwidx = this.itemsDataGridView.CurrentCell.RowIndex;
                if (this.itemsDataGridView.Rows.Count > this.itemsDataGridView.CurrentCell.RowIndex + 1)
                {
                    this.itemsDataGridView.CurrentCell = this.itemsDataGridView.Rows[rwidx + 1].Cells[0];
                }
                else
                {
                    this.itemsDataGridView.CurrentCell = this.itemsDataGridView.Rows[rwidx].Cells[0];
                }
            }
            System.Windows.Forms.Application.DoEvents();

            this.itemsDataGridView.BeginEdit(false);
        }

        private void showUnapprvdCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.shdObeyEvts())
            {
                this.rfrshButton_Click(this.rfrshButton, e);
            }
        }

        private void docDteTextBox_Enter(object sender, EventArgs e)
        {
            if (!this.obey_evnts)
            {
                return;
            }

            TextBox mytxt = (TextBox)sender;
            mytxt.SelectAll();
        }

        private void dscntButton_Click(object sender, EventArgs e)
        {
            if ((Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[98]) == false))
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                    " this action!\nContact your System Administrator!", 0);
                return;
            }
            if (this.apprvlStatusTextBox.Text == "Approved"
              || this.apprvlStatusTextBox.Text == "Initiated"
               || this.apprvlStatusTextBox.Text == "Validated"
              || this.apprvlStatusTextBox.Text == "Cancelled" || this.apprvlStatusTextBox.Text == "Declared Bad Debt"
              || this.apprvlStatusTextBox.Text.Contains("Reviewed")
              || (this.extAppDocIDTextBox.Text != "" && this.extAppDocIDTextBox.Text != "-1"))
            {
                Global.mnFrm.cmCde.showMsg("Cannot EDIT Approved, Initiated, " +
                  "Reviewed, Validated and Cancelled Documents\r\n as well as Documents Created from other Modules!", 0);
                return;
            }

            if (this.editRec == false && this.addRec == false)
            {
                EventArgs e1 = new EventArgs();
                this.editButton_Click(this.editButton, e1);
            }
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT MODE First!", 0);
                return;
            }
            if (this.itemsDataGridView.CurrentCell != null
        && this.itemsDataGridView.SelectedRows.Count <= 0)
            {
                this.itemsDataGridView.Rows[this.itemsDataGridView.CurrentCell.RowIndex].Selected = true;
            }

            if (this.itemsDataGridView.SelectedRows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please select the record to Apply the Discount!", 0);
                return;
            }
            int dscntCodeID = -1;
            //int idx = this.itemsDataGridView.SelectedRows[0].Index;
            double untPrce = double.Parse(this.itemsDataGridView.SelectedRows[0].Cells[7].Value.ToString());
            DialogResult dgres = Global.mnFrm.cmCde.showDscntDiag(ref dscntCodeID, untPrce, Global.mnFrm.cmCde);
            if (dscntCodeID > 0 && dgres == DialogResult.OK)
            {
                this.itemsDataGridView.SelectedRows[0].Cells[22].Value = dscntCodeID.ToString();
                this.itemsDataGridView.SelectedRows[0].Cells[20].Value = Global.mnFrm.cmCde.getGnrlRecNm(
                    "scm.scm_tax_codes", "code_id", "code_name",
                    dscntCodeID);
                this.Refresh();
                System.Windows.Forms.Application.DoEvents();
                this.saveButton.PerformClick();
            }
        }

        private void autoBalscheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.shdObeyEvts() == false
                || beenToCheckBx == true)
            {
                beenToCheckBx = false;
                return;
            }
            beenToCheckBx = true;
            if (this.addRec == false && this.editRec == false)
            {
                this.autoBalscheckBox.Checked = !this.autoBalscheckBox.Checked;
            }
        }

        private void badDebtButton_Click(object sender, EventArgs e)
        {
            if (this.docTypeComboBox.Text != "Sales Invoice")
            {
                Global.mnFrm.cmCde.showMsg("This feature works with Sales Invoices Only!", 0);
                this.saveLabel.Visible = false;
                Cursor.Current = Cursors.Default;
                return;
            }
            if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[71]) == false)
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                  " this action!\nContact your System Administrator!", 0);
                this.saveLabel.Visible = false;
                Cursor.Current = Cursors.Default;
                return;
            }
            if ((this.extAppDocIDTextBox.Text != "" && this.extAppDocIDTextBox.Text != "-1"))
            {
                Global.mnFrm.cmCde.showMsg("Cannot Work on Documents Created from other Modules!", 0);
                this.saveLabel.Visible = false;
                Cursor.Current = Cursors.Default;
                return;
            }
            //Check if Unreversed Payments Exists then disallow else allow
            //and reverse accounting Transactions
            if (this.apprvlStatusTextBox.Text != "Approved"
              && this.badDebtButton.Text == "Declare as Bad Debt")
            {
                Global.mnFrm.cmCde.showMsg("Only Approved Documents can be DECLARED BAD DEBT!", 0);
                return;
            }

            if (this.apprvlStatusTextBox.Text != "Declared Bad Debt"
             && this.badDebtButton.Text == "Reverse Bad Debt")
            {
                Global.mnFrm.cmCde.showMsg("Only Documents Declared as Bad Debt can have this action!", 0);
                return;
            }
            long rcvblHdrID = Global.get_ScmRcvblsDocHdrID(long.Parse(this.docIDTextBox.Text),
              this.docTypeComboBox.Text, Global.mnFrm.cmCde.Org_id);
            string rcvblDoctype = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
              "rcvbls_invc_hdr_id", "rcvbls_invc_type", rcvblHdrID);

            string rcvblDocNum = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
        "rcvbls_invc_hdr_id", "rcvbls_invc_number", rcvblHdrID);

            double pymntsAmnt = Math.Round(Global.getRcvblsDocTtlPymnts(rcvblHdrID, rcvblDoctype), 2);
            if (pymntsAmnt != 0)
            {
                Global.mnFrm.cmCde.showMsg("Please Reverse all Payments on this Document First!\r\n(TOTAL AMOUNT PAID=" + pymntsAmnt.ToString("#,##0.00") + ")", 0);
                this.saveLabel.Visible = false;
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Global.mnFrm.cmCde.showMsg("Are you sure you want to PERFORM this ACTION the selected Document (" + this.badDebtButton.Text.ToUpper() + ")?" +
            "!", 1) == DialogResult.No)
            {
                //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                this.saveLabel.Visible = false;
                Cursor.Current = Cursors.Default;
                return;
            }

            this.saveLabel.Text = "PERFORMING ACTION SELECTED....PLEASE WAIT....";
            this.saveLabel.Visible = true;
            Cursor.Current = Cursors.WaitCursor;

            System.Windows.Forms.Application.DoEvents();

            this.badDebtButton.Enabled = false;
            System.Windows.Forms.Application.DoEvents();
            /*bool isAnyRnng = true;
            int witcntr = 0;
            do
            {
                witcntr++;
                isAnyRnng = Global.isThereANActvActnPrcss("7", "10 second");//Invetory Import Process
                System.Windows.Forms.Application.DoEvents();
            }
            while (isAnyRnng == true);*/

            string dateStr = Global.mnFrm.cmCde.getFrmtdDB_Date_time();
            bool sccs = true;// this.rvrsApprval(dateStr);
                             //if (sccs)
                             //{
                             //  sccs = this.rvrsImprtdIntrfcTrns(long.Parse(this.salesDocIDTextBox.Text), this.salesDocTypeTextBox.Text);
                             //}
            if (sccs)
            {
                if (this.badDebtButton.Text == "Declare as Bad Debt")
                {
                    sccs = this.declareBadDebt(rcvblHdrID, rcvblDocNum);
                }
                else
                {
                    sccs = this.voidBadDebtBatch(rcvblHdrID, rcvblDoctype);
                }
            }

            if (sccs)
            {
                string nwState = "Declared Bad Debt";
                string nxtState = "None";
                string chkIndocState = "Declared Bad Debt";
                string btnText = "Reverse Bad Debt";
                string btnKey = "undo_256.png";
                if (this.badDebtButton.Text == "Reverse Bad Debt")
                {
                    nwState = "Approved";
                    nxtState = "Cancel";
                    chkIndocState = "Checked-Out";
                    btnText = "Declare as Bad Debt";
                    btnKey = "blocked.png";
                }
                //Global.updtCheckInStatus(long.Parse(this.docIDTextBox.Text), chkIndocState);
                Global.updtSalesDocApprvl(long.Parse(this.docIDTextBox.Text), nwState, "None");
                Global.updtRcvblsDocApprvl(rcvblHdrID, nwState, "None");
                this.apprvlStatusTextBox.Text = nwState;
                // this.docStatusTextBox.Text = chkIndocState;
                this.badDebtButton.Text = btnText;
                this.badDebtButton.ImageKey = btnKey;
                this.populateDet(long.Parse(this.docIDTextBox.Text));
                //this.rfrshDtButton_Click(this.rfrshDtButton, e);
            }
            this.saveLabel.Visible = false;
            Cursor.Current = Cursors.Default;

        }

        private void rgstrButton_Click(object sender, EventArgs e)
        {
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("You must be in ADD/EDIT mode first!", 0);
                return;
            }
            if (this.lnkdEventComboBox.Text == "None")
            {
                Global.mnFrm.cmCde.showMsg("You must indicate Event Type first!", 0);
                return;
            }
            else if (this.lnkdEventComboBox.Text == "Attendance Register")
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

            if (this.lnkdEventComboBox.Text == "None")
            {
                Global.mnFrm.cmCde.showMsg("You must indicate Event Type first!", 0);
                return;
            }
            else if (this.lnkdEventComboBox.Text == "Attendance Register")
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

        private void allowDuesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.shdObeyEvts() == false
                || beenToCheckBx == true)
            {
                beenToCheckBx = false;
                if (this.addRec || this.editRec)
                {
                    this.docTypeComboBox_SelectedIndexChanged(this.docTypeComboBox, e);
                    if (this.allowDuesCheckBox.Checked)
                    {
                        if (long.Parse(this.cstmrIDTextBox.Text) <= 0)
                        {
                            this.cstmrButton.PerformClick();
                        }
                        if (long.Parse(this.cstmrIDTextBox.Text) <= 0)
                        {
                            Global.mnFrm.cmCde.showMsg("Please pick a Customer First!", 0);
                            return;
                        }
                        if (this.beenToLoadPerson == false)
                        {
                            this.loadPersonsButton.PerformClick();
                        }
                    }
                }
                return;
            }
            beenToCheckBx = true;
            if (this.addRec == false && this.editRec == false)
            {
                this.allowDuesCheckBox.Checked = !this.allowDuesCheckBox.Checked;
            }
            else
            {
                this.docTypeComboBox_SelectedIndexChanged(this.docTypeComboBox, e);
                if (this.allowDuesCheckBox.Checked)
                {
                    if (long.Parse(this.cstmrIDTextBox.Text) <= 0)
                    {
                        this.cstmrButton.PerformClick();
                    }
                    if (long.Parse(this.cstmrIDTextBox.Text) <= 0)
                    {
                        Global.mnFrm.cmCde.showMsg("Please pick a Customer First!", 0);
                        return;
                    }
                    if (this.beenToLoadPerson == false)
                    {
                        this.loadPersonsButton.PerformClick();
                    }
                }
            }
        }

        private void lnkdEventComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.shdObeyEvts() == false)
            {
                return;
            }
            this.rgstrIDTextBox.Text = "-1";
            this.rgstrNumTextBox.Text = "";
            this.costCtgrTextBox.Text = "";
        }

        private long[] getPrsnsInvolved()
        {
            string dateStr = DateTime.Parse(this.docDteTextBox.Text + " 00:00:00").ToString("yyyy-MM-dd HH:mm:ss");
            string extrWhr = "";
            if (long.Parse(this.cstmrIDTextBox.Text) > 0)
            {
                extrWhr += " and (Select distinct z.lnkd_firm_org_id From prs.prsn_names_nos z where z.person_id=a.person_id)=" + this.cstmrIDTextBox.Text;
            }
            //if (long.Parse(this.siteIDTextBox.Text) > 0)
            //{
            //  extrWhr += " and (Select distinct z.lnkd_firm_site_id From prs.prsn_names_nos z where z.person_id=a.person_id)=" + this.siteIDTextBox.Text;
            //}

            string grpSQL = "";
            string cstmrType = Global.mnFrm.cmCde.getGnrlRecNm(
              "scm.scm_cstmr_suplr", "cust_sup_id",
              "cust_sup_clssfctn", long.Parse(this.cstmrIDTextBox.Text));

            long prsn_id = -1;
            long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
        "scm.scm_cstmr_suplr", "cust_sup_id", "lnkd_prsn_id",
        long.Parse(this.cstmrIDTextBox.Text)), out prsn_id);

            if (prsn_id > 0)
            {
                extrWhr = "";
                grpSQL = "Select distinct a.person_id From prs.prsn_names_nos a Where ((a.person_id = "
                       + prsn_id + ")" + extrWhr + ") ORDER BY a.person_id";
            }
            else
            {
                grpSQL = "Select distinct a.person_id From prs.prsn_names_nos a Where ((a.org_id = "
                 + Global.mnFrm.cmCde.Org_id + ")" + extrWhr + ") ORDER BY a.person_id";
            }

            //Global.mnFrm.cmCde.showSQLNoPermsn(grpSQL);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(grpSQL);
            this.prsnIDs = new long[dtst.Tables[0].Rows.Count];
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                this.prsnIDs[i] = long.Parse(dtst.Tables[0].Rows[i][0].ToString());
            }
            return this.prsnIDs;
        }
        bool beenToLoadPerson = false;
        private void loadPersonsButton_Click(object sender, EventArgs e)
        {
            this.beenToLoadPerson = true;
            if (this.editRec == false && this.addRec == false)
            {
                this.editButton.PerformClick();
            }
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("Must be in ADD/EDIT mode!", 0);
                this.beenToLoadPerson = false;
                return;
            }
            this.allowDuesCheckBox.Checked = true;
            this.obey_evnts = false;

            if (!this.allowDuesCheckBox.Checked)
            {
                Global.mnFrm.cmCde.showMsg("Please allow Dues First!", 0);
                return;
            }
            if (long.Parse(this.cstmrIDTextBox.Text) <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please pick a Customer First!", 0);
                return;
            }
            string rspnse = Microsoft.VisualBasic.Interaction.InputBox(
              "What Total Amount are you Receiving?" +
              "\r\n0   = Unlimited" +
              "\r\n1-∞ = Exact Total Amount\r\n",
              "Rhomicom", "0", (Global.mnFrm.cmCde.myComputer.Screen.Bounds.Width / 2) - 170,
              (Global.mnFrm.cmCde.myComputer.Screen.Bounds.Height / 2) - 100);
            if (rspnse.Equals(string.Empty) || rspnse.Equals(null))
            {
                //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                this.obey_evnts = true;
                this.beenToLoadPerson = false;
                return;
            }
            double rsponse = 0;
            bool rsps = double.TryParse(rspnse, out rsponse);
            if (rsps == false)
            {
                Global.mnFrm.cmCde.showMsg("Invalid Option! Expecting 0-Infinity", 0);
                this.obey_evnts = true;
                this.beenToLoadPerson = false;
                return;
            }
            if (rsponse < 0)
            {
                Global.mnFrm.cmCde.showMsg("Invalid Option! Expecting 0-Infinity", 4);
                this.beenToLoadPerson = false;
                this.obey_evnts = true;
                return;
            }
            string[] selVals = new string[1];
            selVals[0] = "-1";
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                Global.mnFrm.cmCde.getLovID("Item Sets for Payments(Enabled)"), ref selVals,
                true, true, Global.mnFrm.cmCde.Org_id, "", "",
             this.srchWrd, "Both", true, " and (tbl1.g IN (" + Global.concatCurRoleIDs() + "))");
            if (dgRes == DialogResult.OK)
            {
                if (selVals.Length >= 1)
                {
                    this.prsnIDs = this.getPrsnsInvolved();
                    this.itemsDataGridView.Rows.Clear();
                    int rowIdx = 0;
                    double payItmAmnt = 0;
                    if (Global.mnFrm.cmCde.showMsg("This will automatically load all the various \r\nPersons (" + this.prsnIDs.Length +
                        ") Linked to the Selected Customer which can take some time.\r\nAre you sure you want to proceed with this Loading?", 1) == DialogResult.No)
                    {
                        //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                        this.obey_evnts = true;
                        this.beenToLoadPerson = false;
                        return;
                    }
                    this.saveLabel.Text = "Auto-Loading Persons/Items involved...Please Wait...";
                    this.saveLabel.Visible = true;
                    System.Windows.Forms.Application.DoEvents();
                    double ttlAmntLoaded = 0;
                    double diffrnc = 0;
                    decimal outstandgAdvcAmnt = 0;
                    List<Object[]> advncsToApply = new List<Object[]>();
                    List<Object[]> advncsToBeKept = new List<Object[]>();

                    long advBlsItmID = Global.mnFrm.cmCde.getItmID("Total Advance Payments Balance", Global.mnFrm.cmCde.Org_id);
                    long advApplyItmID = Global.mnFrm.cmCde.getItmID("Advance Payments Amount Applied", Global.mnFrm.cmCde.Org_id);
                    long advKeptItmID = Global.mnFrm.cmCde.getItmID("Advance Payments Amount Kept", Global.mnFrm.cmCde.Org_id);

                    long advApplyItmValID = Global.getFirstItmValID(advApplyItmID);
                    long advKeptItmValID = Global.getFirstItmValID(advKeptItmID);
                    String prsnName = "";
                    DataSet advApldDtSt = Global.get_One_AdvcItmDet("Advance Payments Amount Applied");
                    int advApldDtStCntr = advApldDtSt.Tables[0].Rows.Count;
                    for (int a = 0; a < this.prsnIDs.Length; a++)
                    {
                        if (ttlAmntLoaded >= rsponse
                          && rsponse > 0)
                        {
                            break;
                        }
                        this.PrsnID = this.prsnIDs[a];

                        outstandgAdvcAmnt = (decimal)Global.getBlsItmLtstDailyBals(advBlsItmID,
                             this.PrsnID, this.docDteTextBox.Text + " 23:59:59");
                        advncsToApply = new List<Object[]>();
                        prsnName = Global.mnFrm.cmCde.getPrsnName(this.PrsnID) + " (" + Global.mnFrm.cmCde.getPrsnLocID(this.PrsnID) + ")";

                        DataSet dtst = Global.get_One_ItmStDet(int.Parse(selVals[0]));
                        for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
                        {
                            if (this.editRec == true)
                            {
                                if (Global.getIvcDetID(long.Parse(this.docIDTextBox.Text), this.PrsnID,
                                  int.Parse(dtst.Tables[0].Rows[i][8].ToString())) > 0)
                                {
                                    continue;
                                }
                            }
                            this.itemsDataGridView.RowCount += 1;
                            rowIdx = this.itemsDataGridView.RowCount - 1;
                            this.itemsDataGridView.Rows[rowIdx].Cells[0].Value = dtst.Tables[0].Rows[i][9].ToString();
                            this.itemsDataGridView.Rows[rowIdx].Cells[1].Value = "...";
                            this.itemsDataGridView.Rows[rowIdx].Cells[2].Value = dtst.Tables[0].Rows[i][9].ToString();
                            this.itemsDataGridView.Rows[rowIdx].Cells[3].Value = "...";
                            this.itemsDataGridView.Rows[rowIdx].Cells[4].Value = "1.00";
                            this.itemsDataGridView.Rows[rowIdx].Cells[5].Value = dtst.Tables[0].Rows[i][10].ToString();
                            this.itemsDataGridView.Rows[rowIdx].Cells[6].Value = "...";

                            payItmAmnt = this.getPrsPayItmAmount(int.Parse(dtst.Tables[0].Rows[i][8].ToString()), this.PrsnID);

                            if ((ttlAmntLoaded + payItmAmnt) > rsponse && rsponse > 0)
                            {
                                payItmAmnt = rsponse - ttlAmntLoaded;
                            }

                            if (payItmAmnt <= 0)
                            {
                                this.itemsDataGridView.Rows.RemoveAt(rowIdx);
                                continue;
                            }
                            this.itemsDataGridView.Rows[rowIdx].Cells[7].Value = payItmAmnt.ToString("#,##0.00");
                            this.itemsDataGridView.Rows[rowIdx].Cells[8].Value = payItmAmnt.ToString("#,##0.00");
                            ttlAmntLoaded += payItmAmnt;
                            if (outstandgAdvcAmnt > 0)
                            {
                                Global.mnFrm.cmCde.showMsg("Person: " + prsnName + " has Advance Amount of " + outstandgAdvcAmnt.ToString("#,##0.00") + "\r\n\r\nKindly Use Basic Person or Internal Payments to process this payment rather. \r\nThank You!", 0);
                            }
                            if (outstandgAdvcAmnt > 0 && advApldDtStCntr > 0 && 1 == 2)
                            {
                                //To be re-visited at a later time
                                Object[] testArry = new Object[34];
                                decimal advPymnt = 0;
                                testArry[0] = advApldDtSt.Tables[0].Rows[0][9].ToString();
                                testArry[1] = "...";
                                testArry[2] = advApldDtSt.Tables[0].Rows[0][9].ToString();
                                testArry[3] = "...";
                                testArry[4] = "1.00";
                                testArry[5] = advApldDtSt.Tables[0].Rows[0][10].ToString();
                                testArry[6] = "...";
                                if (payItmAmnt > (double)outstandgAdvcAmnt)
                                {
                                    testArry[7] = Math.Round(outstandgAdvcAmnt, 4).ToString();
                                    testArry[8] = Math.Round(outstandgAdvcAmnt, 4).ToString();
                                    advPymnt = Math.Round(outstandgAdvcAmnt, 4);
                                    ttlAmntLoaded -= Math.Round((double)outstandgAdvcAmnt, 4);
                                    outstandgAdvcAmnt = 0;
                                }
                                else
                                {
                                    testArry[7] = payItmAmnt.ToString();
                                    testArry[8] = payItmAmnt.ToString();
                                    advPymnt = (decimal)payItmAmnt;
                                    ttlAmntLoaded -= payItmAmnt;
                                    outstandgAdvcAmnt -= (decimal)payItmAmnt;
                                }
                                testArry[9] = "0.00";
                                testArry[10] = "";
                                testArry[11] = "...";
                                testArry[12] = advApldDtSt.Tables[0].Rows[0][8].ToString();
                                if (advApldDtSt.Tables[0].Rows[0][11].ToString() == "Services")
                                {
                                    testArry[13] = "-1";
                                }
                                else
                                {
                                    testArry[13] = Global.selectedStoreID.ToString();
                                }
                                testArry[14] = this.curid;
                                testArry[15] = "-1";
                                testArry[16] = "-1";
                                testArry[17] = "";
                                testArry[18] = "...";
                                testArry[19] = "-1";
                                testArry[20] = "";
                                testArry[21] = "...";
                                testArry[22] = "-1";
                                testArry[23] = "";
                                testArry[24] = "...";
                                testArry[25] = "-1";
                                testArry[26] = "";
                                testArry[27] = Global.mnFrm.cmCde.getPrsnName(this.PrsnID) + " (" + Global.mnFrm.cmCde.getPrsnLocID(this.PrsnID) + ")";
                                testArry[28] = this.PrsnID.ToString();
                                testArry[29] = "...";
                                testArry[30] = "Linked Person";
                                testArry[31] = advApldDtSt.Tables[0].Rows[0][9].ToString();
                                testArry[32] = "Change Accounts";
                                testArry[33] = "-1,-1,-1,-1,-1";
                                advncsToApply.Add(testArry);

                                testArry = new Object[34];

                                testArry[0] = dtst.Tables[0].Rows[0][9].ToString();
                                testArry[1] = "...";
                                testArry[2] = dtst.Tables[0].Rows[0][9].ToString();
                                testArry[3] = "...";
                                testArry[4] = "1.00";
                                testArry[5] = dtst.Tables[0].Rows[0][10].ToString();
                                testArry[6] = "...";
                                testArry[7] = Math.Round(-1 * advPymnt, 2).ToString();
                                testArry[8] = Math.Round(-1 * advPymnt, 2).ToString();
                                testArry[9] = "0.00";
                                testArry[10] = "";
                                testArry[11] = "...";
                                testArry[12] = dtst.Tables[0].Rows[0][8].ToString();
                                if (dtst.Tables[0].Rows[0][11].ToString() == "Services")
                                {
                                    testArry[13] = "-1";
                                }
                                else
                                {
                                    testArry[13] = Global.selectedStoreID.ToString();
                                }
                                testArry[14] = this.curid;
                                testArry[15] = "-1";
                                testArry[16] = "-1";
                                testArry[17] = "";
                                testArry[18] = "...";
                                testArry[19] = "-1";
                                testArry[20] = "";
                                testArry[21] = "...";
                                testArry[22] = "-1";
                                testArry[23] = "";
                                testArry[24] = "...";
                                testArry[25] = "-1";
                                testArry[26] = "";
                                testArry[27] = Global.mnFrm.cmCde.getPrsnName(this.PrsnID) + " (" + Global.mnFrm.cmCde.getPrsnLocID(this.PrsnID) + ")";
                                testArry[28] = this.PrsnID.ToString();
                                testArry[29] = "...";
                                testArry[30] = "Linked Person";
                                testArry[31] = dtst.Tables[0].Rows[0][9].ToString();
                                testArry[32] = "Change Accounts";
                                testArry[33] = "-1,-1,-1,-1,-1";
                                advncsToApply.Add(testArry);
                            }
                            this.itemsDataGridView.Rows[rowIdx].Cells[9].Value = "0.00";
                            this.itemsDataGridView.Rows[rowIdx].Cells[10].Value = "";
                            this.itemsDataGridView.Rows[rowIdx].Cells[11].Value = "...";
                            this.itemsDataGridView.Rows[rowIdx].Cells[12].Value = dtst.Tables[0].Rows[i][8].ToString();
                            if (dtst.Tables[0].Rows[i][11].ToString() == "Services")
                            {
                                this.itemsDataGridView.Rows[rowIdx].Cells[13].Value = "-1";
                            }
                            else
                            {
                                this.itemsDataGridView.Rows[rowIdx].Cells[13].Value = Global.selectedStoreID.ToString();
                            }
                            this.itemsDataGridView.Rows[rowIdx].Cells[14].Value = this.curid;
                            this.itemsDataGridView.Rows[rowIdx].Cells[15].Value = "-1";
                            this.itemsDataGridView.Rows[rowIdx].Cells[16].Value = "-1";
                            this.itemsDataGridView.Rows[rowIdx].Cells[17].Value = "";
                            this.itemsDataGridView.Rows[rowIdx].Cells[18].Value = "...";
                            this.itemsDataGridView.Rows[rowIdx].Cells[19].Value = "-1";
                            this.itemsDataGridView.Rows[rowIdx].Cells[20].Value = "";
                            this.itemsDataGridView.Rows[rowIdx].Cells[21].Value = "...";
                            this.itemsDataGridView.Rows[rowIdx].Cells[22].Value = "-1";
                            this.itemsDataGridView.Rows[rowIdx].Cells[23].Value = "";
                            this.itemsDataGridView.Rows[rowIdx].Cells[24].Value = "...";
                            this.itemsDataGridView.Rows[rowIdx].Cells[25].Value = "-1";
                            this.itemsDataGridView.Rows[rowIdx].Cells[26].Value = "";
                            this.itemsDataGridView.Rows[rowIdx].Cells[27].Value = Global.mnFrm.cmCde.getPrsnName(this.PrsnID)
                              + " (" + Global.mnFrm.cmCde.getPrsnLocID(this.PrsnID) + ")";
                            this.itemsDataGridView.Rows[rowIdx].Cells[28].Value = this.PrsnID.ToString();
                            this.itemsDataGridView.Rows[rowIdx].Cells[29].Value = "...";
                            this.itemsDataGridView.Rows[rowIdx].Cells[30].Value = "Linked Person";
                            this.itemsDataGridView.Rows[rowIdx].Cells[31].Value = dtst.Tables[0].Rows[i][9].ToString();
                            this.itemsDataGridView.Rows[rowIdx].Cells[32].Value = "Change Accounts";
                            this.itemsDataGridView.Rows[rowIdx].Cells[33].Value = "-1,-1,-1,-1,-1";

                            this.saveLabel.Text = "Loading the Persons involved (" + (a + 1).ToString() + "/" + this.prsnIDs.Length + ") and their Items...Please Wait...";
                            System.Windows.Forms.Application.DoEvents();
                        }
                    }
                    int v = 0;
                    foreach (Object[] lstArr in advncsToApply)
                    {
                        this.itemsDataGridView.RowCount += 1;
                        rowIdx = this.itemsDataGridView.RowCount - 1;
                        if (double.Parse(lstArr[7].ToString()) == 0)
                        {
                            this.itemsDataGridView.Rows.RemoveAt(rowIdx);
                            continue;
                        }
                        else
                        {
                            this.itemsDataGridView.Rows[rowIdx].SetValues(lstArr);
                        }
                        this.saveLabel.Text = "Applying Advance Payments (" + (v + 1).ToString() + "/" + advncsToApply.Count + ")...Please Wait...";
                        System.Windows.Forms.Application.DoEvents();
                        v++;
                    }
                    diffrnc = (rsponse - ttlAmntLoaded);
                    DataSet advDtSt = Global.get_One_AdvcItmDet("Advance Payments Amount Kept");
                    long advcItmID = Global.mnFrm.cmCde.getInvItmID("Advance Payments Amount Kept", Global.mnFrm.cmCde.Org_id);

                    if (diffrnc > 0 && ttlAmntLoaded >= 0 && advcItmID > 0)
                    {
                        if (Global.mnFrm.cmCde.showMsg(
                          "Do you want to keep the Excess Amount (" + diffrnc.ToString("#,##0.00") + ") as Advance Payment?", 2) == DialogResult.Yes)
                        {
                            double amntPerPerson = Math.Round((diffrnc / (double)this.prsnIDs.Length), 2);
                            advncsToBeKept = new List<Object[]>();
                            for (int a = 0; a < this.prsnIDs.Length; a++)
                            {
                                this.PrsnID = this.prsnIDs[a];
                                this.itemsDataGridView.RowCount += 1;
                                rowIdx = this.itemsDataGridView.RowCount - 1;
                                this.itemsDataGridView.Rows[rowIdx].Cells[0].Value = advDtSt.Tables[0].Rows[0][9].ToString();
                                this.itemsDataGridView.Rows[rowIdx].Cells[1].Value = "...";
                                this.itemsDataGridView.Rows[rowIdx].Cells[2].Value = advDtSt.Tables[0].Rows[0][9].ToString();
                                this.itemsDataGridView.Rows[rowIdx].Cells[3].Value = "...";
                                this.itemsDataGridView.Rows[rowIdx].Cells[4].Value = "1.00";
                                this.itemsDataGridView.Rows[rowIdx].Cells[5].Value = advDtSt.Tables[0].Rows[0][10].ToString();
                                this.itemsDataGridView.Rows[rowIdx].Cells[6].Value = "...";
                                if (amntPerPerson > diffrnc || a == this.prsnIDs.Length - 1)
                                {
                                    this.itemsDataGridView.Rows[rowIdx].Cells[7].Value = diffrnc.ToString("#,##0.00");
                                    this.itemsDataGridView.Rows[rowIdx].Cells[8].Value = diffrnc.ToString("#,##0.00");
                                    ttlAmntLoaded += diffrnc;
                                    diffrnc = 0;
                                }
                                else
                                {
                                    this.itemsDataGridView.Rows[rowIdx].Cells[7].Value = amntPerPerson.ToString("#,##0.00");
                                    this.itemsDataGridView.Rows[rowIdx].Cells[8].Value = amntPerPerson.ToString("#,##0.00");
                                    ttlAmntLoaded += amntPerPerson;
                                    diffrnc -= amntPerPerson;
                                }

                                this.itemsDataGridView.Rows[rowIdx].Cells[9].Value = "0.00";
                                this.itemsDataGridView.Rows[rowIdx].Cells[10].Value = "";
                                this.itemsDataGridView.Rows[rowIdx].Cells[11].Value = "...";
                                this.itemsDataGridView.Rows[rowIdx].Cells[12].Value = advDtSt.Tables[0].Rows[0][8].ToString();
                                if (advDtSt.Tables[0].Rows[0][11].ToString() == "Services")
                                {
                                    this.itemsDataGridView.Rows[rowIdx].Cells[13].Value = "-1";
                                }
                                else
                                {
                                    this.itemsDataGridView.Rows[rowIdx].Cells[13].Value = Global.selectedStoreID.ToString();
                                }
                                this.itemsDataGridView.Rows[rowIdx].Cells[14].Value = this.curid;
                                this.itemsDataGridView.Rows[rowIdx].Cells[15].Value = "-1";
                                this.itemsDataGridView.Rows[rowIdx].Cells[16].Value = "-1";
                                this.itemsDataGridView.Rows[rowIdx].Cells[17].Value = "";
                                this.itemsDataGridView.Rows[rowIdx].Cells[18].Value = "...";
                                this.itemsDataGridView.Rows[rowIdx].Cells[19].Value = "-1";
                                this.itemsDataGridView.Rows[rowIdx].Cells[20].Value = "";
                                this.itemsDataGridView.Rows[rowIdx].Cells[21].Value = "...";
                                this.itemsDataGridView.Rows[rowIdx].Cells[22].Value = "-1";
                                this.itemsDataGridView.Rows[rowIdx].Cells[23].Value = "";
                                this.itemsDataGridView.Rows[rowIdx].Cells[24].Value = "...";
                                this.itemsDataGridView.Rows[rowIdx].Cells[25].Value = "-1";
                                this.itemsDataGridView.Rows[rowIdx].Cells[26].Value = "";
                                this.itemsDataGridView.Rows[rowIdx].Cells[27].Value = Global.mnFrm.cmCde.getPrsnName(this.PrsnID)
                                  + " (" + Global.mnFrm.cmCde.getPrsnLocID(this.PrsnID) + ")";
                                this.itemsDataGridView.Rows[rowIdx].Cells[28].Value = this.PrsnID.ToString();//dtst.Tables[0].Rows[i][12].ToString();
                                this.itemsDataGridView.Rows[rowIdx].Cells[29].Value = "...";//dtst.Tables[0].Rows[i][12].ToString();
                                this.itemsDataGridView.Rows[rowIdx].Cells[30].Value = "Linked Person";//dtst.Tables[0].Rows[i][12].ToString();
                                this.itemsDataGridView.Rows[rowIdx].Cells[31].Value = advDtSt.Tables[0].Rows[0][9].ToString();
                                this.itemsDataGridView.Rows[rowIdx].Cells[32].Value = "Change Accounts";
                                this.itemsDataGridView.Rows[rowIdx].Cells[33].Value = "-1,-1,-1,-1,-1";

                                this.saveLabel.Text = "Loading the Persons involved (" + (a + 1).ToString() + "/" + this.prsnIDs.Length + ") and their Advance Payments...Please Wait...";
                                System.Windows.Forms.Application.DoEvents();
                            }
                        }
                    }
                }
                this.saveLabel.Visible = false;
                this.obey_evnts = true;

                this.prpareForLnsEdit();
                this.obey_evnts = true;
                this.beenToLoadPerson = false;
            }
        }

        private bool autoPayDuesItems(double outsBals, ref double ttlAmnt)
        {
            ttlAmnt = 0;
            if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[77]) == false)
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                  " this action!\nContact your System Administrator!", 0);
                return false;
            }

            long payItmID = -1;
            //long payTrnsID = -1;
            long prsnID = -1;
            string itmMajType = "";
            string itmMinType = "";
            string trnsTypComboBox = "";
            decimal amntNumericUpDown = 0;
            string glDateTextBox = this.docDteTextBox.Text + " 00:00:00";
            string docDteTextBox = this.docDteTextBox.Text + " 00:00:00";
            string paymntDescTextBox = "";
            string errMsg = "";
            if (Global.mnFrm.cmCde.showMsg("This will automatically settle the Dues Amounts Entered " +
              "for the various Persons.\r\nAre you sure you want to proceed with this payment?", 1)
        == DialogResult.No)
            {
                //Global.mnFrm.cmCde.showMsg("Operation Cancelled!", 4);
                return false;
            }
            this.saveLabel.Text = "Auto-Processing Dues Payments...Please Wait...";
            this.saveLabel.Visible = true;
            System.Windows.Forms.Application.DoEvents();

            long mspID = Global.get_InvoiceMsPyID(long.Parse(this.docIDTextBox.Text));
            if (mspID <= 0)
            {
                string runFor = "";
                string rnDte = Global.mnFrm.cmCde.getFrmtdDB_Date_time().Replace("-", "").Replace(":", "").Replace(" ", "");
                if (this.cstmrNmTextBox.Text != "")
                {
                    runFor += " (" + this.cstmrNmTextBox.Text + "-" + this.siteNumTextBox.Text + "-Sales Doc. " + this.docIDNumTextBox.Text + ")";
                }
                string tstmspyNm = "Quick Pay Run for " +
                     runFor + " on (" + rnDte + ")";
                mspID = Global.mnFrm.cmCde.getMsPyID(tstmspyNm,
                    Global.mnFrm.cmCde.Org_id);
                if (mspID <= 0)
                {
                    Global.createMsPy(Global.mnFrm.cmCde.Org_id,
                      tstmspyNm, tstmspyNm,
                   docDteTextBox, -1000010,
                   -1000010, glDateTextBox);
                }

                mspID = Global.mnFrm.cmCde.getMsPyID(tstmspyNm,
                  Global.mnFrm.cmCde.Org_id);
            }
            if (mspID <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Failed To Generate Mass Pay Run!", 0);
                return false;
            }
            else
            {
                string dateStr = DateTime.ParseExact(
        Global.mnFrm.cmCde.getDB_Date_time(), "yyyy-MM-dd HH:mm:ss",
        System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy HH:mm:ss");
                long msg_id = Global.mnFrm.cmCde.getLogMsgID(
                      "pay.pay_mass_pay_run_msgs", "Mass Pay Run", mspID);

                if (msg_id <= 0)
                {
                    Global.mnFrm.cmCde.createLogMsg(dateStr + " .... Mass Pay Run through Quick Pay is about to Start...\r\n\r\n",
                "pay.pay_mass_pay_run_msgs", "Mass Pay Run", mspID, dateStr);

                }

                msg_id = Global.mnFrm.cmCde.getLogMsgID("pay.pay_mass_pay_run_msgs", "Mass Pay Run", mspID);
                long invItmID = -1;
                string prsnNameNo = "";
                string itmName = "";
                for (int i = 0; i < this.itemsDataGridView.Rows.Count; i++)
                {
                    invItmID = long.Parse(this.itemsDataGridView.Rows[i].Cells[12].Value.ToString());
                    payItmID = -1;
                    long.TryParse(Global.mnFrm.cmCde.getGnrlRecNm(
            "org.org_pay_items", "inv_item_id", "item_id", invItmID), out payItmID);

                    DataSet pyDtSt = Global.getPayItemDet(payItmID);

                    if (payItmID <= 0 || pyDtSt.Tables[0].Rows.Count <= 0)
                    {
                        errMsg += "\r\n" + "Row (" + (i + 1).ToString() + ") Please select a Pay Item First!";
                        continue;
                    }
                    prsnID = long.Parse(this.itemsDataGridView.Rows[i].Cells[28].Value.ToString());
                    prsnNameNo = this.itemsDataGridView.Rows[i].Cells[27].Value.ToString();
                    itmMajType = pyDtSt.Tables[0].Rows[0][2].ToString();
                    itmMinType = pyDtSt.Tables[0].Rows[0][3].ToString();
                    itmName = pyDtSt.Tables[0].Rows[0][1].ToString();
                    if (itmMinType == "Earnings"
                    || itmMinType == "Employer Charges")
                    {
                        trnsTypComboBox = "Payment by Organisation";
                        paymntDescTextBox = "Payment of " +
                    itmName +
                    " for " + prsnNameNo;
                    }
                    else if (itmMinType == "Bills/Charges"
                    || itmMinType == "Deductions")
                    {
                        trnsTypComboBox = "Payment by Person";
                        paymntDescTextBox = "Payment of " +
                    itmName + " by " + prsnNameNo;
                    }
                    else if (itmMajType.ToUpper() == "Balance Item".ToUpper())
                    {
                        trnsTypComboBox = "";
                    }
                    else
                    {
                        trnsTypComboBox = "Purely Informational";
                        paymntDescTextBox = "Running of Purely Informational Item " +
                    itmName + " for " + prsnNameNo;
                    }

                    amntNumericUpDown = decimal.Parse(this.itemsDataGridView.Rows[i].Cells[8].Value.ToString());


                    if (prsnID <= 0)
                    {
                        errMsg += "\r\n" + "Row (" + (i + 1).ToString() + ") Please select a Person First!";
                        continue;
                    }
                    if (itmMajType.ToUpper() == "Balance Item".ToUpper())
                    {
                        errMsg += "\r\n" + "Row (" + (i + 1).ToString() + ") Cannot run Payment for Balance Items!";
                        continue;
                    }
                    long prsnItmRwID = this.doesPrsnHvItmPrs(prsnID,
                      payItmID);
                    if (prsnItmRwID <= 0)
                    {
                        long dfltVal = this.getFirstItmValID(payItmID);
                        if (dfltVal > 0)
                        {
                            this.createBnftsPrs(prsnID,
                      payItmID
                        , dfltVal
                        , "01-Jan-1900", "31-Dec-4000");
                        }
                    }
                    else if (this.doesPrsnHvItm(prsnID,
             payItmID, docDteTextBox) == false)
                    {
                        errMsg += "\r\n" + "Row (" + (i + 1).ToString() + ") cannot be processed because Person does not have Item for the Transaction Date Used!\r\n";
                        continue;
                    }
                    if (trnsTypComboBox == "")
                    {
                        errMsg += "\r\n" + "Row (" + (i + 1).ToString() + ") Transaction Type cannot be empty!";
                        continue;
                    }
                    if (amntNumericUpDown == 0)
                    {
                        errMsg += "\r\n" + "Row (" + (i + 1).ToString() + ") Amount cannot be zero!";
                        continue;
                    }
                    if (docDteTextBox == "")
                    {
                        errMsg += "\r\n" + "Row (" + (i + 1).ToString() + ") Payment Date cannot be empty!";
                        continue;
                    }
                    if (glDateTextBox == "")
                    {
                        errMsg += "\r\n" + "Row (" + (i + 1).ToString() + ") GL Date cannot be empty!";
                        continue;
                    }

                    if (paymntDescTextBox == "")
                    {
                        errMsg += "\r\n" + "Row (" + (i + 1).ToString() + ") Payment Description cannot be empty!";
                        continue;
                    }

                    /* Processing a Payment
                    * 1. Create Payment line pay.pay_itm_trnsctns for Pay Value Items
                    * 2. Update Daily BalsItms for all balance items this Pay value Item feeds into
                    * 3. Create Tmp GL Lines in a temp GL interface Table 
                    * 4. Need to check whether any of its Balance Items disallows negative balance. 
                    * If Not disallow this trans if it will lead to a negative balance on a Balance Item
                    */
                    if (this.doesPymntDteViolateFreq(prsnID
                      , payItmID
                      , docDteTextBox) == true)
                    {
                        errMsg += "\r\n" + "Row (" + (i + 1).ToString() + ") The Payment Date violates the Item's Defined Pay Frequency!";
                        continue;
                    }

                    if (this.hsPrsnBnPaidItmMnl(prsnID
                 , payItmID
                 , docDteTextBox, (double)amntNumericUpDown) == true)
                    {
                        errMsg += "\r\n" + "Row (" + (i + 1).ToString() + ") Same Payment has been made for this Person on the same Date Already!";
                        continue;
                    }

                    //if (!this.isPayTrnsValid())
                    //{
                    //  continue;
                    //}

                    //   dateStr = DateTime.ParseExact(
                    //Global.mnFrm.cmCde.getDB_Date_time(), "yyyy-MM-dd HH:mm:ss",
                    //System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy HH:mm:ss");
                    double nwAmnt = this.willItmBlsBeNgtv(
                      prsnID
                      , payItmID
                      , (double)amntNumericUpDown, docDteTextBox);
                    if (nwAmnt < 0)
                    {
                        errMsg += "\r\n" + "Row (" + (i + 1).ToString() + ") This transaction will cause a Balance Item " +
                          "to Have Negative Balance and hence cannot be allowed!";
                        continue;
                    }


                    bool res = false;
                    if (1 == 2)
                    {

                    }
                    else
                    {
                        this.createPaymntLine(prsnID,
                      payItmID,
                      (double)amntNumericUpDown, docDteTextBox,
                      "Manual", trnsTypComboBox, mspID, paymntDescTextBox,
                      int.Parse(this.invcCurrIDTextBox.Text), dateStr
                      , "VALID", -1, glDateTextBox, long.Parse(this.docIDTextBox.Text));
                        //if (i == this.itemsDataGridView.Rows.Count - 1)
                        //{
                        //  payTrnsID = this.getPaymntTrnsID(prsnID,
                        //payItmID,
                        //(double)amntNumericUpDown,
                        //docDteTextBox, -1);
                        //}

                        //Update Balance Items
                        this.updtBlsItms(prsnID
                          , payItmID
                          , (double)amntNumericUpDown
                          , docDteTextBox, "Mass Pay Run", -1);
                        ttlAmnt += (double)amntNumericUpDown;
                    }
                    res = true;
                    if (res)
                    {
                    }
                    else
                    {
                        errMsg += "\r\n" + "Row (" + (i + 1).ToString() + ") Processing Payment Failed!";
                        continue;
                    }
                }
                Global.mnFrm.cmCde.updateLogMsg(msg_id, errMsg, "pay.pay_mass_pay_run_msgs", dateStr);
                this.saveLabel.Visible = false;
                System.Windows.Forms.Application.DoEvents();
                if (ttlAmnt <= 0)
                {
                    ttlAmnt = outsBals;
                }
                if (mspID > 0)
                {
                    long rcvblHdrID = Global.get_ScmRcvblsDocHdrID(long.Parse(this.docIDTextBox.Text), "Sales Invoice",
                Global.mnFrm.cmCde.Org_id);

                    string rcvblDoctype = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
                   "rcvbls_invc_hdr_id", "rcvbls_invc_type", rcvblHdrID);

                    DialogResult dgres = Global.mnFrm.cmCde.showPymntDiag(
                     false, false,
                   this.groupBox2.Location.X - 85,
                     180,
                     ttlAmnt, this.curid,
                     int.Parse(this.pymntMthdIDTextBox.Text), "Customer Payments",
                     long.Parse(this.cstmrIDTextBox.Text),
                     long.Parse(this.siteIDTextBox.Text),
                     rcvblHdrID,
                     rcvblDoctype, Global.mnFrm.cmCde, mspID);
                    EventArgs e = new EventArgs();
                    if (dgres == DialogResult.OK)
                    {
                        Global.updateMsPyStatus(mspID, "1", "1");
                        Global.mnFrm.cmCde.updateLogMsg(msg_id, "Payment Successfully Processed", "pay.pay_mass_pay_run_msgs", dateStr);
                        //Global.mnFrm.cmCde.showMsg("Payment Successfully Processed! \r\nMessages Logged! You can go to the Quick Pay Run to Check!", 3);
                        this.reCalcRcvblsSmmrys(rcvblHdrID, rcvblDoctype);
                        this.populateDet(long.Parse(this.docIDTextBox.Text));
                        this.populateLines(long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text);
                        this.calcSmryButton_Click(this.calcSmryButton, e);
                        if (ttlAmnt > 0)
                        {
                            this.printRcptButton_Click(this.printRcptButton, e);
                        }
                    }
                    else
                    {
                        this.calcSmryButton_Click(this.calcSmryButton, e);
                    }
                }
                return true;
            }
        }

        #endregion

        #region "INTERNAL/RECEIVABLES PAYMENTS..."
        public DataSet getItmVal1(long itmid)
        {
            string selSQL = "SELECT pssbl_value_id, pssbl_value_code_name, pssbl_amount, pssbl_value_sql, item_id " +
            "FROM org.org_pay_items_values WHERE ((item_id = " + itmid + ")) ORDER BY pssbl_value_id DESC";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(selSQL);
            return dtst;
        }

        public DataSet getAllItmFeeds1(long itmid)
        {
            string selSQL = "SELECT a.balance_item_id, a.adds_subtracts, b.balance_type, a.scale_factor, c.pssbl_value_id " +
            "FROM org.org_pay_itm_feeds a LEFT OUTER JOIN org.org_pay_items b " +
            "ON a.balance_item_id = b.item_id LEFT OUTER JOIN org.org_pay_items_values c " +
            "ON c.item_id = a.balance_item_id WHERE ((a.fed_by_itm_id = " + itmid +
            ")) ORDER BY a.feed_id ";
            //Global.mnFrm.cmCde.showSQLNoPermsn(selSQL);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(selSQL);
            return dtst;
        }
        public void createBnftsPrs(long prsnid, long itmid, long itm_val_id,
        string strtdte, string enddte)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            strtdte = DateTime.ParseExact(
         strtdte, "dd-MMM-yyyy",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            enddte = DateTime.ParseExact(
         enddte, "dd-MMM-yyyy",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string insSQL = "INSERT INTO pasn.prsn_bnfts_cntrbtns(" +
                     "person_id, item_id, item_pssbl_value_id, valid_start_date, valid_end_date, " +
                     "created_by, creation_date, last_update_by, last_update_date) " +
             "VALUES (" + prsnid + ", " + itmid +
             ", " + itm_val_id + ", '" + strtdte.Replace("'", "''") + "', '" + enddte.Replace("'", "''") +
             "', " + Global.mnFrm.cmCde.User_id + ", '" + dateStr + "', " +
                     Global.mnFrm.cmCde.User_id + ", '" + dateStr + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public void updateBnftsPrs(long prsnid, long rowid, long itm_val_id,
        string strtdte, string enddte)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            strtdte = DateTime.ParseExact(
         strtdte, "dd-MMM-yyyy",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            enddte = DateTime.ParseExact(
         enddte, "dd-MMM-yyyy",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string updtSQL = "UPDATE pasn.prsn_bnfts_cntrbtns " +
                "SET person_id=" + prsnid + ", item_pssbl_value_id=" + itm_val_id +
             ", valid_start_date='" + strtdte.Replace("'", "''") +
             "', valid_end_date='" + enddte.Replace("'", "''") + "', " +
                "last_update_by=" +
                     Global.mnFrm.cmCde.User_id + ", last_update_date='" + dateStr + "' " +
             "WHERE row_id=" + rowid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }


        public void updtBlsItms(long prsn_id, long itm_id,
          double pay_amount, string trns_date, string trns_src, long orgnlTrnsID)
        {
            DataSet dtst = this.getAllItmFeeds1(itm_id);
            double nwAmnt = 0;
            for (int a = 0; a < dtst.Tables[0].Rows.Count; a++)
            {
                double lstBals = 0;
                double scaleFctr = 1;
                double.TryParse(dtst.Tables[0].Rows[a][3].ToString(), out scaleFctr);
                if (dtst.Tables[0].Rows[a][2].ToString() == "Cumulative")
                {
                    lstBals = this.getBlsItmLtstDailyBals(
                      long.Parse(dtst.Tables[0].Rows[a][0].ToString()),
                    prsn_id, trns_date);
                    if (dtst.Tables[0].Rows[a][1].ToString() == "Subtracts")
                    {
                        nwAmnt = -1 * pay_amount * scaleFctr;
                    }
                    else
                    {
                        nwAmnt = pay_amount * scaleFctr;
                    }
                }
                else
                {
                    lstBals = this.getBlsItmDailyBals(long.Parse(dtst.Tables[0].Rows[a][0].ToString()),
               prsn_id, trns_date);
                    if (dtst.Tables[0].Rows[a][1].ToString() == "Subtracts")
                    {
                        nwAmnt = -1 * pay_amount * scaleFctr;
                    }
                    else
                    {
                        nwAmnt = pay_amount * scaleFctr;
                    }
                }
                //Check if prsn's balance has not been updated already
                long paytrnsid = this.getPaymntTrnsID(
                prsn_id, itm_id,
                pay_amount, trns_date, orgnlTrnsID);

                bool hsBlsBnUpdtd = this.hsPrsItmBlsBnUptd(paytrnsid,
                  trns_date, long.Parse(dtst.Tables[0].Rows[a][0].ToString()),
                  prsn_id);
                long dailybalID = this.getItmDailyBalsID(
                  long.Parse(dtst.Tables[0].Rows[a][0].ToString()),
                  trns_date, prsn_id);

                if (hsBlsBnUpdtd == false)
                {
                    if (dailybalID <= 0)
                    {
                        this.createItmBals(long.Parse(dtst.Tables[0].Rows[a][0].ToString()),
                          lstBals, prsn_id, trns_date, -1);

                        if (dtst.Tables[0].Rows[a][2].ToString() == "Cumulative")
                        {
                            this.updtItmDailyBalsCum(trns_date,
                            long.Parse(dtst.Tables[0].Rows[a][0].ToString()),
                           prsn_id,
                           nwAmnt, paytrnsid);
                        }
                        else
                        {
                            this.updtItmDailyBalsNonCum(trns_date,
                            long.Parse(dtst.Tables[0].Rows[a][0].ToString()),
                           prsn_id,
                           nwAmnt, paytrnsid);
                        }

                    }
                    else
                    {
                        if (dtst.Tables[0].Rows[a][2].ToString() == "Cumulative")
                        {
                            this.updtItmDailyBalsCum(trns_date,
                            long.Parse(dtst.Tables[0].Rows[a][0].ToString()),
                           prsn_id,
                           nwAmnt, paytrnsid);
                        }
                        else
                        {
                            this.updtItmDailyBalsNonCum(trns_date,
                            long.Parse(dtst.Tables[0].Rows[a][0].ToString()),
                           prsn_id,
                           nwAmnt, paytrnsid);
                        }
                    }
                }
            }
        }

        public double willItmBlsBeNgtv(long prsn_id, long itm_id,
          double pay_amount, string trns_date)
        {
            DataSet dtst = this.getAllItmFeeds1(itm_id);
            double nwAmnt = 0;
            for (int a = 0; a < dtst.Tables[0].Rows.Count; a++)
            {
                //if (this.doesPrsnHvItm(prsn_id, long.Parse(dtst.Tables[0].Rows[a][0].ToString()), trns_date) == false)
                //{
                //  string tstDte = "";
                //  this.doesPrsnHvItm(prsn_id, itm_id, trns_date, ref tstDte);
                //  if (tstDte == "")
                //  {
                //    tstDte = "01-Jan-1900 00:00:00";
                //  }
                //  this.createBnftsPrs(prsn_id,
                //    long.Parse(dtst.Tables[0].Rows[a][0].ToString())
                //      , long.Parse(dtst.Tables[0].Rows[a][4].ToString())
                //      , "01-" + tstDte.Substring(3, 8), "31-Dec-4000");
                //}
                if (this.doesPrsnHvItmPrs(prsn_id,
                  long.Parse(dtst.Tables[0].Rows[a][0].ToString())) <= 0)
                {
                    string tstDte = "";
                    this.doesPrsnHvItm(prsn_id, itm_id, trns_date, ref tstDte);
                    if (tstDte == "")
                    {
                        tstDte = "01-Jan-1900 00:00:00";
                    }
                    this.createBnftsPrs(prsn_id,
                      long.Parse(dtst.Tables[0].Rows[a][0].ToString())
                        , long.Parse(dtst.Tables[0].Rows[a][4].ToString())
                        , "01-" + tstDte.Substring(3, 8), "31-Dec-4000");
                    //Global.createBnftsPrs(prsn_id,
                    //  long.Parse(dtst.Tables[0].Rows[a][0].ToString())
                    //    , long.Parse(dtst.Tables[0].Rows[a][0].ToString())
                    //    , "01-" + trns_date.Substring(3, 8), "31-Dec-4000");
                }
                double scaleFctr = 1;
                double.TryParse(dtst.Tables[0].Rows[a][3].ToString(), out scaleFctr);
                if (dtst.Tables[0].Rows[a][2].ToString() == "Cumulative")
                {
                    if (dtst.Tables[0].Rows[a][1].ToString() == "Subtracts")
                    {
                        nwAmnt = this.getBlsItmLtstDailyBals(
                          long.Parse(dtst.Tables[0].Rows[a][0].ToString()),
                          prsn_id, trns_date) - (pay_amount * scaleFctr);
                        //Global.mnFrm.cmCde.showMsg(nwAmnt.ToString() + "/" + Global.mnFrm.cmCde.getPrsnLocID(prsn_id), 0);
                    }
                    else
                    {
                        nwAmnt = (pay_amount * scaleFctr)
                  + this.getBlsItmLtstDailyBals(long.Parse(dtst.Tables[0].Rows[a][0].ToString()),
                  prsn_id, trns_date);
                    }
                }
                else
                {
                    if (dtst.Tables[0].Rows[a][1].ToString() == "Subtracts")
                    {
                        nwAmnt = this.getBlsItmDailyBals(long.Parse(dtst.Tables[0].Rows[a][0].ToString()),
                          prsn_id, trns_date) - (pay_amount * scaleFctr);
                    }
                    else
                    {
                        nwAmnt = (pay_amount * scaleFctr)
                  + this.getBlsItmDailyBals(long.Parse(dtst.Tables[0].Rows[a][0].ToString()),
                  prsn_id, trns_date);
                    }
                }

                if (nwAmnt < 0)
                {
                    return nwAmnt;
                }
            }
            return nwAmnt;
        }

        public void updtItmDailyBalsCum(string balsDate, long blsItmID,
        long prsn_id, double netAmnt, long py_trns_id)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE pay.pay_balsitm_bals " +
            "SET last_update_by = " + Global.mnFrm.cmCde.User_id +
            ", last_update_date = '" + dateStr +
            "', bals_amount = bals_amount +" + netAmnt +
            ", source_trns_ids = source_trns_ids || '" + py_trns_id +
          ",' WHERE (to_timestamp(bals_date,'YYYY-MM-DD') >= to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and bals_itm_id = " + blsItmID + " and person_id = " + prsn_id + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public void updtItmDailyBalsNonCum(string balsDate, long blsItmID,
        long prsn_id, double netAmnt, long py_trns_id)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE pay.pay_balsitm_bals " +
            "SET last_update_by = " + Global.mnFrm.cmCde.User_id +
            ", last_update_date = '" + dateStr +
            "', bals_amount = bals_amount +" + netAmnt +
            ", source_trns_ids = source_trns_ids || '" + py_trns_id +
            ",' WHERE (to_timestamp(bals_date,'YYYY-MM-DD') = to_timestamp('" + balsDate +
            "','YYYY-MM-DD') and bals_itm_id = " + blsItmID + " and person_id = " + prsn_id + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public void createItmBals(long blsitmid, double netbals,
        long prsn_id,
        string balsDate, long py_trns_id)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            if (balsDate.Length > 10)
            {
                balsDate = balsDate.Substring(0, 10);
            }
            string src_trns = ",";
            if (py_trns_id > 0)
            {
                src_trns = "," + py_trns_id + ",";
            }
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO pay.pay_balsitm_bals(" +
                  "bals_itm_id, bals_amount, person_id, bals_date, created_by, " +
                  "creation_date, last_update_by, last_update_date, source_trns_ids) " +
              "VALUES (" + blsitmid +
              ", " + netbals + ", " + prsn_id + ", '" + balsDate + "', " +
              Global.mnFrm.cmCde.User_id + ", '" + dateStr +
                              "', " + Global.mnFrm.cmCde.User_id + ", '" + dateStr + "', '" + src_trns.Replace("'", "''") + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public void createPaymntLine(long prsnid, long itmid, double amnt, string paydate,
        string paysource, string trnsType, long msspyid, string paydesc, int crncyid, string dateStr,
          string pymt_vldty, long src_trns_id, string glDate, long invcID)
        {
            paydate = DateTime.ParseExact(
         paydate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            dateStr = DateTime.ParseExact(
         dateStr, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string insSQL = "INSERT INTO pay.pay_itm_trnsctns(" +
                     "person_id, item_id, amount_paid, paymnt_date, paymnt_source, " +
                     "pay_trns_type, created_by, creation_date, last_update_by, last_update_date, " +
                     "mass_pay_id, pymnt_desc, crncy_id, pymnt_vldty_status, src_py_trns_id, gl_date, sales_invoice_id) " +
             "VALUES (" + prsnid + ", " + itmid + ", " + amnt +
             ", '" + paydate.Replace("'", "''") + "', '" + paysource.Replace("'", "''") +
             "', '" + trnsType.Replace("'", "''") + "', " + Global.mnFrm.cmCde.User_id + ", '" + dateStr + "', " +
                     Global.mnFrm.cmCde.User_id + ", '" + dateStr + "', " + msspyid +
                     ", '" + paydesc.Replace("'", "''") + "', " + crncyid +
                     ", '" + pymt_vldty.Replace("'", "''") + "', " + src_trns_id + ", '" + glDate + "', " + invcID + ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public string get_InvItemNm(int itmID)
        {
            string strSql = "SELECT REPLACE(item_desc || ' (' || REPLACE(item_code,item_desc,'') || ')', ' ()','') " +
         "FROM inv.inv_itm_list a " +
         "WHERE item_id =" + itmID + "";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            return "";
        }

        public string get_PayItemNm(int itmID)
        {
            string strSql = "SELECT item_code_name " +
         "FROM org.org_pay_items a " +
         "WHERE item_id =" + itmID + "";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            return "";
        }

        public string[] get_ItmAccntInfo(long itmID)
        {
            string[] retSql = { "Q", "-123", "Q", "-123" };
            string strSql = "SELECT a.incrs_dcrs_cost_acnt, a.cost_accnt_id, a.incrs_dcrs_bals_acnt, a.bals_accnt_id " +
         "FROM org.org_pay_items a " +
         "WHERE(a.item_id = " + itmID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                retSql[0] = dtst.Tables[0].Rows[0][0].ToString();
                retSql[1] = dtst.Tables[0].Rows[0][1].ToString();
                retSql[2] = dtst.Tables[0].Rows[0][2].ToString();
                retSql[3] = dtst.Tables[0].Rows[0][3].ToString();
            }
            return retSql;
        }

        public long getItmDailyBalsID(long balsItmID, string balsDate, long prsn_id)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);
            string strSql = "";
            strSql = "SELECT a.bals_id " +
         "FROM pay.pay_balsitm_bals a " +
         "WHERE(to_timestamp(a.bals_date,'YYYY-MM-DD') =  to_timestamp('" + balsDate +
         "','YYYY-MM-DD') and a.bals_itm_id = " + balsItmID +
         " and a.person_id = " + prsn_id + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        public double getBlsItmDailyBals(long balsItmID, long prsn_id, string balsDate)
        {
            string orgnlDte = balsDate;
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);
            double res = 0;
            string strSql = "";
            string usesSQL = Global.mnFrm.cmCde.getGnrlRecNm("org.org_pay_items",
              "item_id", "uses_sql_formulas", balsItmID);
            if (usesSQL != "1")
            {
                strSql = "SELECT a.bals_amount " +
              "FROM pay.pay_balsitm_bals a " +
              "WHERE(to_timestamp(a.bals_date,'YYYY-MM-DD') =  to_timestamp('" + balsDate +
              "','YYYY-MM-DD') and a.bals_itm_id = " + balsItmID + " and a.person_id = " + prsn_id + ")";

                DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
                if (dtst.Tables[0].Rows.Count > 0)
                {
                    double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out res);
                }
            }
            else
            {
                string valSQL = Global.mnFrm.cmCde.getItmValSQL(this.getPrsnItmVlID(prsn_id, balsItmID, orgnlDte));
                if (valSQL == "")
                {
                }
                else
                {
                    try
                    {
                        res = Global.mnFrm.cmCde.exctItmValSQL(
                          valSQL, prsn_id,
                          Global.mnFrm.cmCde.Org_id, balsDate);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return res;
        }

        public double getBlsItmLtstDailyBals(long balsItmID, long prsn_id, string balsDate)
        {
            string orgnlDte = balsDate;
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            double res = 0;
            string strSql = "";
            string usesSQL = Global.mnFrm.cmCde.getGnrlRecNm("org.org_pay_items",
         "item_id", "uses_sql_formulas", balsItmID);
            if (usesSQL != "1")
            {
                strSql = "SELECT a.bals_amount " +
                   "FROM pay.pay_balsitm_bals a " +
                   "WHERE(to_timestamp(a.bals_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
                   "','YYYY-MM-DD') and a.bals_itm_id = " + balsItmID + " and a.person_id = " + prsn_id +
                   ") ORDER BY to_timestamp(a.bals_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";

                DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
                if (dtst.Tables[0].Rows.Count > 0)
                {
                    double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out res);
                }
            }
            else
            {
                string valSQL = Global.mnFrm.cmCde.getItmValSQL(this.getPrsnItmVlID(prsn_id, balsItmID, orgnlDte));
                if (valSQL == "")
                {
                }
                else
                {
                    try
                    {
                        res = Global.mnFrm.cmCde.exctItmValSQL(
                          valSQL, prsn_id,
                          Global.mnFrm.cmCde.Org_id, balsDate);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return res;
        }

        public DataSet getPstPayDet(long paytrnsid)
        {
            string strSql = @"SELECT a.amount_paid, 
to_char(to_timestamp(a.paymnt_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS')
, a.pay_trns_type, a.crncy_id, a.pymnt_desc " +
             "FROM pay.pay_itm_trnsctns a " +
             "WHERE ((a.pay_trns_id = " + paytrnsid + "))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public long getPymntRvrslTrnsID(long paytrnsid)
        {
            string strSql = @"SELECT a.pay_trns_id " +
              "FROM pay.pay_itm_trnsctns a " +
              "WHERE ((a.src_py_trns_id = "
              + paytrnsid + ") or (a.pay_trns_id = "
              + paytrnsid + " AND a.src_py_trns_id>0))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        public void updateTrnsVldtyStatus(long paytrnsid, string vldty)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE pay.pay_itm_trnsctns " +
            "SET pymnt_vldty_status='" + vldty.Replace("'", "''") +
            "', last_update_by=" + Global.mnFrm.cmCde.User_id +
            ", last_update_date='" + dateStr +
            "' WHERE pay_trns_id = " + paytrnsid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public long getPaymntTrnsID(long prsnid, long itmid,
          double amnt, string paydate, long orgnlTrnsID)
        {
            //, string vldty, long srcTrnsID
            paydate = DateTime.ParseExact(
         paydate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "SELECT pay_trns_id FROM pay.pay_itm_trnsctns WHERE (person_id = " +
                prsnid + " and item_id = " + itmid + " and amount_paid = " + amnt +
                " and paymnt_date = '" + paydate.Replace("'", "''") +
                "' and pymnt_vldty_status='VALID' and src_py_trns_id=" + orgnlTrnsID + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        public decimal get_ttl_paymnts(long prsnID, long itmID, string whopays)
        {
            string colnm = "ttl_amnt_given_prsn";
            if (whopays == "Person")
            {
                colnm = "ttl_amnt_prsn_hs_paid";
            }
            /*string strSql = "Select SUM(a.amount_paid) FROM pay.pay_itm_trnsctns a where a.person_id = " + 
             prsnID + " and a.item_id = " + itmID + " and a.pay_trns_type like '%Payment%'";*/
            string strSql = "Select " + colnm + " FROM pasn.prsn_bnfts_cntrbtns a where a.person_id = " +
                prsnID + " and a.item_id = " + itmID + "";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Decimal fnl_val = 0;
            bool res = false;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                res = decimal.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out fnl_val);
                if (res)
                {
                    return fnl_val;
                }
            }
            return fnl_val;
        }

        public decimal get_ttl_withdrwls(long prsnID, long itmID)
        {
            /*string strSql = "Select SUM(a.amount_paid) FROM pay.pay_itm_trnsctns a where a.person_id = " +
             prsnID + " and a.item_id = " + itmID + " and a.pay_trns_type like '%Withdrawal%'";*/
            string strSql = "Select ttl_amnt_wthdrwn FROM pasn.prsn_bnfts_cntrbtns a where a.person_id = " +
         prsnID + " and a.item_id = " + itmID + "";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Decimal fnl_val = 0;
            bool res = false;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                res = decimal.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out fnl_val);
                if (res)
                {
                    return fnl_val;
                }
            }
            return fnl_val;
        }

        public string getPymntTyp(long py_trns_id)
        {
            string strSql = "SELECT a.paymnt_source FROM pay.pay_itm_trnsctns a WHERE a.pay_trns_id = " + py_trns_id;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            return "";
        }

        public bool hsMsPyBnRun(long mspyid)
        {
            string strSql = "SELECT a.run_status FROM pay.pay_mass_pay_run_hdr a WHERE a.mass_pay_id = " + mspyid;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return Global.mnFrm.cmCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[0][0].ToString());
            }
            return false;
        }

        public bool hsMsPyGoneToGL(long mspyid)
        {
            string strSql = "SELECT a.sent_to_gl FROM pay.pay_mass_pay_run_hdr a WHERE a.mass_pay_id = " + mspyid;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return Global.mnFrm.cmCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[0][0].ToString());
            }
            return false;
        }

        public bool hsPrsItmBlsBnUptd(long pytrnsid,
          string trnsdate, long bals_itm_id, long prsn_id)
        {
            trnsdate = DateTime.ParseExact(
         trnsdate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            if (trnsdate.Length > 10)
            {
                trnsdate = trnsdate.Substring(0, 10);
            }

            string strSql = "SELECT a.bals_id FROM pay.pay_balsitm_bals a WHERE a.bals_itm_id = " + bals_itm_id +
              " and a.person_id = " + prsn_id + " and a.bals_date = '" + trnsdate + "' and a.source_trns_ids like '%," + pytrnsid + ",%'";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void deletePymntGLInfcLns(long pyTrnsID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSQL = "DELETE FROM pay.pay_gl_interface WHERE source_trns_id = " +
              pyTrnsID + " and gl_batch_id = -1";
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public long getIntFcTrnsDbtLn(long pytrnsid, double pay_amnt)
        {
            string strSql = "SELECT a.interface_id FROM pay.pay_gl_interface a " +
                    "WHERE a.source_trns_id = " + pytrnsid +
              " and a.dbt_amount = " + pay_amnt + " ";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public long getIntFcTrnsCrdtLn(long pytrnsid, double pay_amnt)
        {
            string strSql = "SELECT a.interface_id FROM pay.pay_gl_interface a " +
                    "WHERE a.source_trns_id = " + pytrnsid +
              " and a.crdt_amount = " + pay_amnt + " ";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public double getMsPyAmntSum(long mspyid)
        {
            string strSql = "SELECT SUM(a.amount_paid) FROM pay.pay_itm_trnsctns a " +
              "WHERE a.pay_trns_type !='Purely Informational' and a.crncy_id > 0 and a.mass_pay_id = " + mspyid;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public double getMsPyIntfcDbtSum(long mspyid)
        {
            string strSql = "SELECT SUM(a.dbt_amount) FROM pay.pay_gl_interface a " +
              "WHERE a.source_trns_id IN (select b.pay_trns_id from pay.pay_itm_trnsctns b WHERE b.mass_pay_id = " + mspyid + ") ";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public double getMsPyIntfcCrdtSum(long mspyid)
        {
            string strSql = "SELECT SUM(a.crdt_amount) FROM pay.pay_gl_interface a " +
              "WHERE a.source_trns_id IN (select b.pay_trns_id from pay.pay_itm_trnsctns b WHERE b.mass_pay_id = " + mspyid + ") ";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public long getFirstItmValID(long itmID)
        {
            string strSql = @"Select a.pssbl_value_id FROM org.org_pay_items_values a 
      where((a.item_id = " + itmID + ")) ORDER BY 1 LIMIT 1 OFFSET 0";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public bool doesPrsnHvItm(long prsnID, long itmID, string dateStr)
        {
            dateStr = DateTime.ParseExact(
         dateStr, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "Select a.row_id FROM pasn.prsn_bnfts_cntrbtns a where((a.person_id = " +
          prsnID + ") and (a.item_id = " + itmID + ") and (to_timestamp('" + dateStr + "'," +
          "'YYYY-MM-DD HH24:MI:SS') between to_timestamp(valid_start_date|| ' 00:00:00','YYYY-MM-DD HH24:MI:SS') " +
                  "AND to_timestamp(valid_end_date || ' 23:59:59','YYYY-MM-DD HH24:MI:SS')))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool doesPrsnHvItm(long prsnID, long itmID, string dateStr, ref string strtDte)
        {
            dateStr = DateTime.ParseExact(
         dateStr, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = @"Select a.row_id, to_char(to_timestamp(valid_start_date|| ' 00:00:00','YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS')
      FROM pasn.prsn_bnfts_cntrbtns a where((a.person_id = " +
          prsnID + ") and (a.item_id = " + itmID + ") and (to_timestamp('" + dateStr + "'," +
          "'YYYY-MM-DD HH24:MI:SS') between to_timestamp(valid_start_date|| ' 00:00:00','YYYY-MM-DD HH24:MI:SS') " +
                  "AND to_timestamp(valid_end_date || ' 23:59:59','YYYY-MM-DD HH24:MI:SS')))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                strtDte = dtst.Tables[0].Rows[0][1].ToString();
                return true;
            }
            strtDte = "";
            return false;
        }

        public long doesPrsnHvItmPrs(long prsnid, long itmid)
        {
            string selSQL = "SELECT row_id " +
                        "FROM pasn.prsn_bnfts_cntrbtns WHERE ((person_id = " + prsnid +
                        ") and (item_id = " + itmid + "))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(selSQL);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        public long hsPrsnBnPaidItmMsPy(long prsnID, long itmID,
          string trns_date, double amnt)
        {
            trns_date = DateTime.ParseExact(
            trns_date, "dd-MMM-yyyy HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            //if (trns_date.Length > 10)
            //{
            //  trns_date = trns_date.Substring(0, 10);
            //}
            string strSql = "Select a.pay_trns_id FROM pay.pay_itm_trnsctns a where((a.person_id = " +
          prsnID + ") and (a.item_id = " + itmID + ") and (paymnt_date ilike '%" + trns_date +
          "%') and (amount_paid=" + amnt + ") and (a.pymnt_vldty_status='VALID' and a.src_py_trns_id < 0))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public bool hsPrsnBnPaidItmMnl(long prsnID, long itmID,
          string trns_date, double amnt)
        {
            trns_date = DateTime.ParseExact(
            trns_date, "dd-MMM-yyyy HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            //if (trns_date.Length > 10)
            //{
            //  trns_date = trns_date.Substring(0, 10);
            //}
            string strSql = "Select a.pay_trns_id FROM pay.pay_itm_trnsctns a where((a.person_id = " +
          prsnID + ") and (a.item_id = " + itmID + ") and (paymnt_date like '%" + trns_date +
          "%') and (amount_paid=" + amnt + ") and (a.pymnt_vldty_status='VALID' and a.src_py_trns_id < 0))";
            // and (paymnt_source = '" + py_src.Replace("'", "''") + "')
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool doesPymntDteViolateFreq(long prsnID, long itmID,
          string trns_date)
        {
            /*Daily
         Weekly
         Fortnightly
         Semi-Monthly
         Monthly
         Quarterly
         Half-Yearly
         Annually
         Adhoc
         None*/
            trns_date = DateTime.ParseExact(
            trns_date, "dd-MMM-yyyy HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string pyFreq = Global.mnFrm.cmCde.getGnrlRecNm("org.org_pay_items", "item_id", "pay_frequency", itmID);
            string intrvlCls = "";
            string whrCls = "";
            if (pyFreq == "Daily")
            {
                intrvlCls = "1 day";
            }
            else if (pyFreq == "Weekly")
            {
                intrvlCls = "7 day";
            }
            else if (pyFreq == "Fortnightly")
            {
                intrvlCls = "14 day";
            }
            else if (pyFreq == "Semi-Monthly")
            {
                intrvlCls = "14 day";
            }
            else if (pyFreq == "Monthly")
            {
                intrvlCls = "28 day";
            }
            else if (pyFreq == "Quarterly")
            {
                intrvlCls = "90 day";
            }
            else if (pyFreq == "Half-Yearly")
            {
                intrvlCls = "182 day";
            }
            else if (pyFreq == "Annually")
            {
                intrvlCls = "365 day";
            }
            else if (pyFreq == "Adhoc")
            {
                intrvlCls = "1 second";
                return false;
            }
            else if (pyFreq == "None")
            {
                intrvlCls = "1 second";
                return false;
            }
            else
            {
                intrvlCls = "1 second";
                if (pyFreq == "Once a Month" || pyFreq == "Twice a Month")
                {
                    whrCls = @" and (substr(a.paymnt_date,1,7) = substr('" + trns_date +
              "',1,7))";
                }
            }
            if (whrCls == "")
            {
                whrCls = " and (age(GREATEST(paymnt_date::TIMESTAMP,'" + trns_date +
            "'::TIMESTAMP),LEAST(paymnt_date::TIMESTAMP, '" + trns_date +
            "'::TIMESTAMP)) < interval '" + intrvlCls + "')";
            }

            string strSql = "Select count(1) FROM pay.pay_itm_trnsctns a where((a.person_id = " +
          prsnID + ") and (a.item_id = " + itmID + @") and (a.pymnt_vldty_status='VALID' and 
      a.src_py_trns_id <= 0)" + whrCls + ")";
            // and (paymnt_source = '" + py_src.Replace("'", "''") + "')
            /*a.pay_trns_id, a.paymnt_date*/
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.mnFrm.cmCde.showSQLNoPermsn(pyFreq + "/" + strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                if (pyFreq == "Once a Month" && long.Parse(dtst.Tables[0].Rows[0][0].ToString()) >= 1)
                {
                    return true;
                }
                else if (pyFreq == "Twice a Month" && long.Parse(dtst.Tables[0].Rows[0][0].ToString()) >= 2)
                {
                    return true;
                }
                else if (!(pyFreq == "Once a Month" || pyFreq == "Twice a Month")
                  && (long.Parse(dtst.Tables[0].Rows[0][0].ToString()) > 0))
                {
                    return true;
                }
            }
            return false;
        }

        public bool doesItmStHvItm(long hdrID, long itmID)
        {
            string strSql = "Select a.det_id FROM pay.pay_itm_sets_det a where((a.hdr_id = " +
          hdrID + ") and (a.item_id = " + itmID + "))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool doesPrsStHvPrs(long hdrID, long prsnID)
        {
            string strSql = "Select a.prsn_set_det_id FROM pay.pay_prsn_sets_det a where((a.prsn_set_hdr_id = " +
          hdrID + ") and (a.person_id = " + prsnID + "))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public long getPrsnItmVlID(long prsnID, long itmID, string trnsdte)
        {
            trnsdte = DateTime.ParseExact(trnsdte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            //string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string strSql = "Select a.item_pssbl_value_id FROM pasn.prsn_bnfts_cntrbtns a where((a.person_id = " +
          prsnID + ") and (a.item_id = " + itmID + ") and (to_timestamp('" + trnsdte + "'," +
          "'YYYY-MM-DD HH24:MI:SS') between to_timestamp(valid_start_date|| ' 00:00:00','YYYY-MM-DD HH24:MI:SS') " +
                  "AND to_timestamp(valid_end_date || ' 23:59:59','YYYY-MM-DD HH24:MI:SS')))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -100000;
        }

        #endregion

        private void printDocument3_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Pen aPen = new Pen(Brushes.Black, 1);
            Graphics g = e.Graphics;
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Pos", 199, 3300);
            Font font1 = new Font("Tahoma", 7.25f, FontStyle.Bold);
            Font font2 = new Font("Tahoma", 7.25f, FontStyle.Bold);
            Font font4 = new Font("Tahoma", 7.25f, FontStyle.Bold);
            Font font3 = new Font("Lucida Console", 7.25f, FontStyle.Regular);
            Font font5 = new Font("Times New Roman", 6.0f, FontStyle.Italic);

            int font1Hght = font1.Height;
            int font2Hght = font2.Height;
            int font3Hght = font3.Height;
            int font4Hght = font4.Height;
            int font5Hght = font5.Height;

            float pageWidth = e.PageSettings.PaperSize.Width - 40;//e.PageSettings.PrintableArea.Width;
            float pageHeight = e.PageSettings.PaperSize.Height - 40;// e.PageSettings.PrintableArea.Height;
                                                                    //Global.mnFrm.cmCde.showMsg(pageWidth.ToString(), 0);
            int startX = 5;
            int startY = 10;
            int offsetY = 0;
            //StringBuilder strPrnt = new StringBuilder();
            //strPrnt.AppendLine("Received From");
            string[] nwLn;
            //DataSet dtst = Global.get_LastScmPay_Trns(
            //  long.Parse(this.docIDTextBox.Text), this.docTypeComboBox.Text, Global.mnFrm.cmCde.Org_id);
            long rcvblHdrID = Global.get_ScmRcvblsDocHdrID(long.Parse(this.docIDTextBox.Text),
         this.docTypeComboBox.Text, Global.mnFrm.cmCde.Org_id);
            string rcvblDoctype = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_rcvbls_invc_hdr",
              "rcvbls_invc_hdr_id", "rcvbls_invc_type", rcvblHdrID);

            DataSet dtst = Global.get_LastRcvblPay_Trns(
              rcvblHdrID, rcvblDoctype, Global.mnFrm.cmCde.Org_id);

            if (dtst.Tables[0].Rows.Count <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Cannot Print a Receipt when no Payment has been made!", 0);
                return;
            }
            string rcptNo = "";

            if (this.pageNo == 1)
            {
                //Org Name
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
                  Global.mnFrm.cmCde.getOrgName(Global.mnFrm.cmCde.Org_id),
                  pageWidth + 65, font2, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i]
                    , font2, Brushes.Black, startX, startY + offsetY);
                    offsetY += font2Hght;
                }

                //Pstal Address
                g.DrawString(Global.mnFrm.cmCde.getOrgPstlAddrs(Global.mnFrm.cmCde.Org_id).Trim(),
                font2, Brushes.Black, startX, startY + offsetY);
                //offsetY += font2Hght;

                ght = g.MeasureString(
                 Global.mnFrm.cmCde.getOrgPstlAddrs(Global.mnFrm.cmCde.Org_id).Trim(), font2).Height;
                offsetY = offsetY + (int)ght;
                //Contacts Nos
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
            Global.mnFrm.cmCde.getOrgContactNos(Global.mnFrm.cmCde.Org_id),
            pageWidth, font2, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i]
                    , font2, Brushes.Black, startX, startY + offsetY);
                    offsetY += font2Hght;
                }
                //Email Address
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
            Global.mnFrm.cmCde.getOrgEmailAddrs(Global.mnFrm.cmCde.Org_id),
            pageWidth, font2, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i]
                    , font2, Brushes.Black, startX, startY + offsetY);
                    offsetY += font2Hght;
                }

                offsetY += 3;
                g.DrawLine(aPen, startX, startY + offsetY, startX + pageWidth + 25,
                  startY + offsetY);
                g.DrawString("Payment Receipt", font2, Brushes.Black, startX, startY + offsetY);
                offsetY += font2Hght;
                g.DrawLine(aPen, startX, startY + offsetY, startX + pageWidth + 25,
                startY + offsetY);
                offsetY += 3;
                g.DrawString("Doc. No: ", font4, Brushes.Black, startX, startY + offsetY);
                ght = g.MeasureString("Doc. No: ", font4).Width;
                //Receipt No: 
                g.DrawString(this.docIDNumTextBox.Text,
            font3, Brushes.Black, startX + ght, startY + offsetY + 2);
                offsetY += font4Hght;

                g.DrawString("Payment Receipt No: ", font4, Brushes.Black, startX, startY + offsetY);
                //offsetY += font4Hght;
                ght = g.MeasureString("Payment Receipt No: ", font4).Width;
                //Get Last Payment
                if (dtst.Tables[0].Rows.Count > 0)
                {
                    rcptNo = dtst.Tables[0].Rows[0][0].ToString();
                }
                if (rcptNo.Length < 4)
                {
                    rcptNo = rcptNo.PadLeft(4, '0');
                }
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
            rcptNo,
            startX + ght, font3, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i]
                    , font3, Brushes.Black, startX + ght, startY + offsetY + 2);
                    offsetY += font3Hght;
                }
                offsetY += 2;

                string curcy = this.invcCurrTextBox.Text;// Global.mnFrm.cmCde.getPssblValNm(Global.mnFrm.cmCde.getOrgFuncCurID(Global.mnFrm.cmCde.Org_id));
                g.DrawString("Date Rcvd: ", font4, Brushes.Black, startX, startY + offsetY);
                ght = g.MeasureString("Date Rcvd: ", font4).Width;
                //Receipt No: 
                g.DrawString(dtst.Tables[0].Rows[0][8].ToString(),
            font3, Brushes.Black, startX + ght, startY + offsetY + 3);
                offsetY += font4Hght;
                g.DrawString("Currency: ", font4, Brushes.Black, startX, startY + offsetY);
                ght = g.MeasureString("Currency: ", font4).Width;
                //Receipt No: 
                g.DrawString(curcy,
            font3, Brushes.Black, startX + ght, startY + offsetY + 3);
                offsetY += font4Hght;
                g.DrawString("Cashier: ", font4, Brushes.Black, startX, startY + offsetY);
                ght = g.MeasureString("Cashier: ", font4).Width;
                //Receipt No: 
                g.DrawString(dtst.Tables[0].Rows[0][10].ToString().ToUpper(),
            font3, Brushes.Black, startX + ght, startY + offsetY + 2);

                if (this.cstmrNmTextBox.Text != "")
                {
                    offsetY += font4Hght;
                    g.DrawString("Customer: ", font4, Brushes.Black, startX, startY + offsetY);
                    //offsetY += font4Hght;
                    ght = g.MeasureString("Customer: ", font4).Width;
                    //Get Last Payment
                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                this.cstmrNmTextBox.Text,
                pageWidth - startX - ght - 5, font3, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        g.DrawString(nwLn[i]
                        , font3, Brushes.Black, startX + ght, startY + offsetY + 2);
                        if (i < nwLn.Length - 1)
                        {
                            offsetY += font4Hght;
                        }
                    }
                }

                offsetY += 3;
                offsetY += font3Hght;
                g.DrawLine(aPen, startX, startY + offsetY, startX + pageWidth + 25,
            startY + offsetY);
                offsetY += 3;
                g.DrawString("Item Description", font1, Brushes.Black, startX, startY + offsetY);
                //offsetY += font4Hght;
                ght = g.MeasureString("Item Description", font1).Width;
                itmWdth = (int)ght;
                qntyStartX = startX + (int)ght;
                if (this.allowDuesCheckBox.Checked == false)
                {
                    g.DrawString(" Qty".PadLeft(7, ' '), font1, Brushes.Black, qntyStartX, startY + offsetY);
                    //offsetY += font4Hght;
                }
                ght += g.MeasureString(" Qty".PadLeft(7, ' '), font1).Width;
                qntyWdth = (int)g.MeasureString(" Qty".PadLeft(7, ' '), font1).Width; ;
                prcStartX = startX + (int)ght;
                if (this.allowDuesCheckBox.Checked == true)
                {
                    itmWdth = (int)ght;
                }

                g.DrawString("Amount".PadLeft(11, ' '), font1, Brushes.Black, prcStartX, startY + offsetY);
                ght = g.MeasureString("Amount".PadLeft(11, ' '), font1).Width;
                prcWdth = (int)ght;
                offsetY += font1Hght;
                g.DrawLine(aPen, startX, startY + offsetY, startX + pageWidth + 25,
             startY + offsetY);
                offsetY += 3;
            }
            DataSet lndtst = Global.get_One_SalesDcLines(long.Parse(this.docIDTextBox.Text));
            //Line Items
            int orgOffstY = 0;
            int hgstOffst = offsetY;
            for (int a = this.prntIdx; a < lndtst.Tables[0].Rows.Count; a++)
            {
                orgOffstY = hgstOffst;
                offsetY = orgOffstY;
                ght = 0;
                if (this.allowDuesCheckBox.Checked == true)
                {
                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                    (lndtst.Tables[0].Rows[a][25].ToString().Trim()
                    + " for " + lndtst.Tables[0].Rows[a][24].ToString().Replace("-", " ").Trim()).Trim() + "@"
                + double.Parse(lndtst.Tables[0].Rows[a][3].ToString()).ToString("#,##0.00"),
                itmWdth, font3, g);
                }
                else
                {
                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                    (lndtst.Tables[0].Rows[a][25].ToString().Trim()).Trim() + "@"
                + double.Parse(lndtst.Tables[0].Rows[a][3].ToString()).ToString("#,##0.00"),
                itmWdth, font3, g);
                }

                for (int i = 0; i < nwLn.Length; i++)
                {
                    //breakPOSTxtDown
                    if (g.MeasureString(nwLn[i], font3).Width > itmWdth)
                    {
                        string[] nwnwLn;
                        nwnwLn = Global.mnFrm.cmCde.breakPOSTxtDown(nwLn[i],
                  itmWdth, font3, g, 14);
                        for (int j = 0; j < nwnwLn.Length; j++)
                        {
                            g.DrawString(nwnwLn[j]
                     , font3, Brushes.Black, startX, startY + offsetY);
                            offsetY += font3Hght;
                            ght += g.MeasureString(nwnwLn[j], font3).Width;
                        }
                    }
                    else
                    {
                        g.DrawString(nwLn[i]
                        , font3, Brushes.Black, startX, startY + offsetY);
                        offsetY += font3Hght;
                        ght += g.MeasureString(nwLn[i], font3).Width;
                    }
                }
                if (offsetY > hgstOffst)
                {
                    hgstOffst = offsetY;
                }
                offsetY = orgOffstY;

                if (this.allowDuesCheckBox.Checked == false)
                {
                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                              double.Parse(lndtst.Tables[0].Rows[a][2].ToString()).ToString(),
                        qntyWdth, font3, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        if (i == 0)
                        {
                            ght = g.MeasureString(nwLn[i], font3).Width;
                        }
                        g.DrawString(nwLn[i].PadLeft(7, ' ')
                        , font3, Brushes.Black, qntyStartX - 12, startY + offsetY);
                        offsetY += font3Hght;
                    }
                    if (offsetY > hgstOffst)
                    {
                        hgstOffst = offsetY;
                    }
                    offsetY = orgOffstY;
                }
                nwLn = Global.mnFrm.cmCde.breakTxtDown(
                  double.Parse(lndtst.Tables[0].Rows[a][4].ToString()).ToString("#,##0"),
            prcWdth, font3, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    if (i == 0)
                    {
                        ght = g.MeasureString(nwLn[i], font3).Width;
                    }
                    g.DrawString(nwLn[i].PadLeft(13, ' ')
                    , font3, Brushes.Black, prcStartX - 20, startY + offsetY);
                    offsetY += font3Hght;
                }
                if (offsetY > hgstOffst)
                {
                    hgstOffst = offsetY;
                }
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
                offsetY = hgstOffst + font3Hght;
                g.DrawLine(aPen, startX, startY + offsetY, startX + pageWidth + 25,
                     startY + offsetY);
                offsetY += 3;
            }
            DataSet smmryDtSt = Global.get_DocSmryLns(long.Parse(this.docIDTextBox.Text),
              this.docTypeComboBox.Text);
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
                  smmryDtSt.Tables[0].Rows[b][1].ToString().PadRight(30, ' '),
            itmWdth, font3, g);

                for (int i = 0; i < nwLn.Length; i++)
                {
                    g.DrawString(nwLn[i].PadRight(30, ' ')
                    , font3, Brushes.Black, startX + 5, startY + offsetY);
                    offsetY += font3Hght;
                    ght += g.MeasureString(nwLn[i], font3).Width;
                }
                if (offsetY > hgstOffst)
                {
                    hgstOffst = offsetY;
                }
                offsetY = orgOffstY;

                nwLn = Global.mnFrm.cmCde.breakTxtDown(
                  double.Parse(smmryDtSt.Tables[0].Rows[b][2].ToString()).ToString("#,##0.00"),
            prcWdth, font3, g);
                for (int i = 0; i < nwLn.Length; i++)
                {
                    if (i == 0)
                    {
                        ght = g.MeasureString(nwLn[i], font3).Width;
                    }
                    g.DrawString(nwLn[i].PadLeft(13, ' ')
                    , font3, Brushes.Black, prcStartX - 20, startY + offsetY);
                    offsetY += font3Hght;
                }
                if (offsetY > hgstOffst)
                {
                    hgstOffst = offsetY;
                }
                this.prntIdx1++;
            }
            if (this.prntIdx2 == 0)
            {
                offsetY = hgstOffst + font3Hght;
                g.DrawLine(aPen, startX, startY + offsetY, startX + pageWidth + 25,
              startY + offsetY);
                offsetY += 3;
            }
            orgOffstY = 0;
            hgstOffst = offsetY;

            for (int c = this.prntIdx2; c < 4; c++)
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
                if (c == 0)
                {
                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                      "Receipt Amount:".PadRight(30, ' '),
               itmWdth, font3, g);

                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        g.DrawString(nwLn[i].PadRight(30, ' ')
                        , font3, Brushes.Black, startX + 5, startY + offsetY);
                        offsetY += font3Hght;
                        ght += g.MeasureString(nwLn[i], font3).Width;
                    }
                    if (offsetY > hgstOffst)
                    {
                        hgstOffst = offsetY;
                    }
                    offsetY = orgOffstY;

                    string amntRcvd = "0.00";
                    if (double.Parse(dtst.Tables[0].Rows[0][2].ToString()) > 0
                      && double.Parse(dtst.Tables[0].Rows[0][3].ToString()) <= 0)
                    {
                        amntRcvd = (Math.Abs(double.Parse(dtst.Tables[0].Rows[0][2].ToString())) -
                        double.Parse(dtst.Tables[0].Rows[0][3].ToString())).ToString("#,##0.00");
                    }
                    else if (double.Parse(dtst.Tables[0].Rows[0][2].ToString()) > 0
                      && double.Parse(dtst.Tables[0].Rows[0][3].ToString()) > 0)
                    {
                        amntRcvd = double.Parse(dtst.Tables[0].Rows[0][2].ToString()).ToString("#,##0.00");
                    }

                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                      double.Parse(amntRcvd).ToString("#,##0.00"),
               prcWdth, font3, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        if (i == 0)
                        {
                            ght = g.MeasureString(nwLn[i], font3).Width;
                        }
                        g.DrawString(nwLn[i].PadLeft(13, ' ')
                        , font3, Brushes.Black, prcStartX - 20, startY + offsetY);
                        offsetY += font3Hght;
                    }
                    if (offsetY > hgstOffst)
                    {
                        hgstOffst = offsetY;
                    }
                    this.prntIdx2++;
                }
                else if (c == 1)
                {
                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                      "Description:".PadRight(30, ' '),
              itmWdth, font3, g);

                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        g.DrawString(nwLn[i].PadRight(30, ' ')
                        , font3, Brushes.Black, startX + 5, startY + offsetY);
                        offsetY += font3Hght;
                        ght += g.MeasureString(nwLn[i], font3).Width;
                    }
                    if (offsetY > hgstOffst)
                    {
                        hgstOffst = offsetY;
                    }
                    offsetY = orgOffstY;
                    string payDesc = "-Part Payment";
                    if (double.Parse(dtst.Tables[0].Rows[0][3].ToString()) <= 0)
                    {
                        payDesc = "-Full Payment";
                    }
                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                      dtst.Tables[0].Rows[0][1].ToString() + payDesc,
               prcWdth, font3, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        if (i == 0)
                        {
                            ght = g.MeasureString(nwLn[i], font3).Width;
                        }
                        g.DrawString(nwLn[i]//.PadRight(30, ' ')
                        , font3, Brushes.Black, prcStartX + 3, startY + offsetY);
                        offsetY += font3Hght;
                    }

                    if (offsetY > hgstOffst)
                    {
                        hgstOffst = offsetY;
                    }
                    this.prntIdx2++;
                }
                else if (c == 2)
                {
                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                      "Change/Balance:".PadRight(30, ' '),
               itmWdth, font3, g);

                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        g.DrawString(nwLn[i].PadRight(30, ' ')
                        , font3, Brushes.Black, startX + 5, startY + offsetY);
                        offsetY += font3Hght;
                        ght += g.MeasureString(nwLn[i], font3).Width;
                    }
                    if (offsetY > hgstOffst)
                    {
                        hgstOffst = offsetY;
                    }
                    offsetY = orgOffstY;

                    nwLn = Global.mnFrm.cmCde.breakTxtDown(
                      double.Parse(dtst.Tables[0].Rows[0][3].ToString()).ToString("#,##0.00"),
               prcWdth, font3, g);
                    for (int i = 0; i < nwLn.Length; i++)
                    {
                        if (i == 0)
                        {
                            ght = g.MeasureString(nwLn[i], font3).Width;
                        }
                        g.DrawString(nwLn[i].PadLeft(13, ' ')
                        , font3, Brushes.Black, prcStartX - 20, startY + offsetY);
                        offsetY += font3Hght;
                    }
                    if (offsetY > hgstOffst)
                    {
                        hgstOffst = offsetY;
                    }
                    this.prntIdx2++;
                }
            }

            //Slogan: 
            offsetY += font3Hght;
            offsetY += font3Hght;
            if (hgstOffst >= pageHeight - 30)
            {
                e.HasMorePages = true;
                offsetY = 0;
                this.pageNo++;
                return;
            }
            g.DrawLine(aPen, startX, startY + offsetY, startX + pageWidth + 25,
         startY + offsetY);
            nwLn = Global.mnFrm.cmCde.breakTxtDown(
              Global.mnFrm.cmCde.getOrgSlogan(Global.mnFrm.cmCde.Org_id),
         pageWidth - ght, font5, g);
            for (int i = 0; i < nwLn.Length; i++)
            {
                g.DrawString(nwLn[i]
                , font5, Brushes.Black, startX, startY + offsetY);
                offsetY += font5Hght;
            }
            offsetY += font5Hght;
            nwLn = Global.mnFrm.cmCde.breakTxtDown(
             "Software Developed by Rhomicom Systems Technologies Ltd.",
         pageWidth + 40, font5, g);
            for (int i = 0; i < nwLn.Length; i++)
            {
                g.DrawString(nwLn[i]
                , font5, Brushes.Black, startX, startY + offsetY);
                offsetY += font5Hght;
            }
            nwLn = Global.mnFrm.cmCde.breakTxtDown(
         "Website:www.rhomicomgh.com Mobile: 0544709501/0266245395",
         pageWidth + 40, font5, g);
            for (int i = 0; i < nwLn.Length; i++)
            {
                g.DrawString(nwLn[i]
                , font5, Brushes.Black, startX, startY + offsetY);
                offsetY += font5Hght;
            }
        }

        private void customInvoiceButton_Click(object sender, EventArgs e)
        {
            this.calcSmryButton.PerformClick();
            string reportName = "";
            string reportTitle = this.docTypeComboBox.Text;
            if (this.docTypeComboBox.Text == "Sales Invoice"
              || this.docTypeComboBox.Text == "Sales Return"
              || this.docTypeComboBox.Text == "Sales Order"
              || this.docTypeComboBox.Text == "Pro-Forma Invoice")
            {
                if (this.allowDuesCheckBox.Checked)
                {
                    reportName = Global.mnFrm.cmCde.getEnbldPssblValDesc("Sales Invoice - Dues",
             Global.mnFrm.cmCde.getLovID("Document Custom Print Process Names"));
                    reportTitle = "Dues Payment Document";
                }
                else
                {
                    reportName = Global.mnFrm.cmCde.getEnbldPssblValDesc("Sales Invoice",
                    Global.mnFrm.cmCde.getLovID("Document Custom Print Process Names"));
                }
            }
            else if (this.docTypeComboBox.Text == "Item Issue-Unbilled")
            {
                reportName = Global.mnFrm.cmCde.getEnbldPssblValDesc("Item Issues",
                Global.mnFrm.cmCde.getLovID("Document Custom Print Process Names"));
            }
            else
            {
                reportName = Global.mnFrm.cmCde.getEnbldPssblValDesc("Internal Item Request",
                Global.mnFrm.cmCde.getLovID("Document Custom Print Process Names"));
            }
            string paramRepsNVals = "{:invoice_id}~" + this.docIDTextBox.Text + "|{:documentTitle}~" + reportTitle;
            //Global.mnFrm.cmCde.showSQLNoPermsn(reportName + "\r\n" + paramRepsNVals);
            Global.mnFrm.cmCde.showRptParamsDiag(Global.mnFrm.cmCde.getRptID(reportName), Global.mnFrm.cmCde, paramRepsNVals, reportTitle);
        }

        private void pymntTermsButton_Click(object sender, EventArgs e)
        {
            string txtStr = this.payTermsTextBox.Text;
            if (this.editRec || this.addRec)
            {
                Global.mnFrm.cmCde.showTxtNoPermsn(ref txtStr);
                this.payTermsTextBox.Text = txtStr;
            }
            else
            {
                Global.mnFrm.cmCde.showSQLNoPermsn(txtStr);
            }
        }

        private void vwDuesLogsButton_Click(object sender, EventArgs e)
        {
            if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[7]) == false)
            {
                Global.mnFrm.cmCde.showMsg("You don't have permission to perform" +
                 " this action!\nContact your System Administrator!", 0);
                return;
            }
            long docID = long.Parse(this.docIDTextBox.Text);
            long mspyID = Global.get_InvoiceMsPyID(docID);
            if (docID <= 0 || mspyID <= 0)
            {
                Global.mnFrm.cmCde.showMsg("No Logs Found!", 0);
                return;
            }
            string mspyNm = Global.mnFrm.cmCde.getMsPyName(mspyID);
            if (mspyNm.Contains("Reversal"))
            {
                Global.mnFrm.cmCde.showLogMsg(
                  Global.mnFrm.cmCde.getLogMsgID("pay.pay_mass_pay_run_msgs",
                  "Mass Pay Run Reversal", mspyID), "pay.pay_mass_pay_run_msgs");
            }
            else
            {
                Global.mnFrm.cmCde.showLogMsg(
            Global.mnFrm.cmCde.getLogMsgID("pay.pay_mass_pay_run_msgs",
            "Mass Pay Run", mspyID), "pay.pay_mass_pay_run_msgs");
            }
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void invcCurrIDTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void descLovButton_Click(object sender, EventArgs e)
        {
            if (this.editRec == false && this.addRec == false)
            {
                Global.mnFrm.cmCde.showMsg("You must be in ADD/EDIT mode first!", 0);
                return;//
            }
            int[] selVals = new int[1];
            selVals[0] = Global.mnFrm.cmCde.getPssblValID(this.docCommentsTextBox.Text,
              Global.mnFrm.cmCde.getLovID("Sample Sales Narrations"));
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                Global.mnFrm.cmCde.getLovID("Sample Sales Narrations"), ref selVals,
                true, false,
             "%", "Both", true);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.docCommentsTextBox.Text = Global.mnFrm.cmCde.getPssblValNm(selVals[i]);
                }
            }
        }
    }
}
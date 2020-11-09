using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cadmaFunctions;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.VisualBasic;

namespace CommonCode
{
    public partial class segmentValuesDiag : Form
    {
        #region "GLOBAL VARIABLES..."
        //Records;
        private string selItemTxt = "";
        public bool autoLoad = false;
        public bool isReadOnly = false;
        public bool shdSelOne = false;
        public bool mustSelctSth = false;
        bool errorOcrd = false;
        long rec_cur_indx = 0;
        bool is_last_rec = false;
        long totl_rec = 0;
        long last_rec_num = 0;
        public string rec_SQL = "";
        bool obey_evnts = false;
        public bool txtChngd = false;
        public string srchWrd = "%";
        public long[] prsnIDs = new long[1];
        bool addRec = false;
        bool editRec = false;
        bool addRecsP = false;
        bool editRecsP = false;
        bool delRecsP = false;
        bool beenToCheckBx = false;

        public int segmentID = -1;
        public int sgmntValID = -1;
        public int segmentNum = -1;
        public int dpndntSegmentID = -1;
        public int orgID = -1;
        public string sysClsfctn = "";
        string segmentValsSQL = "";
        string recClsfctn_SQL = "";
        public CommonCodes cmnCde = new CommonCodes();
        public string[] dfltPrvldgs = { "View Organization Setup",
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
        public string[] cashFlowClsfctns ={"Cash and Cash Equivalents",
"Operating Activities.Sale of Goods",
"Operating Activities.Sale of Services",
"Operating Activities.Other Income Sources",
"Operating Activities.Cost of Sales",
"Operating Activities.Net Income",
"Operating Activities.Depreciation Expense",
"Operating Activities.Amortization Expense",
"Operating Activities.Gain on Sale of Asset"/*NEGATE*/,
"Operating Activities.Loss on Sale of Asset",
"Operating Activities.Other Non-Cash Expense",
"Operating Activities.Accounts Receivable"/*NEGATE*/,
"Operating Activities.Bad Debt Expense"/*NEGATE*/,
"Operating Activities.Prepaid Expenses"/*NEGATE*/,
"Operating Activities.Inventory"/*NEGATE*/,
"Operating Activities.Accounts Payable",
"Operating Activities.Accrued Expenses",
"Operating Activities.Taxes Payable",
"Operating Activities.Operating Expense"/*NEGATE*/,
"Operating Activities.General and Administrative Expense"/*NEGATE*/,
"Investing Activities.Asset Sales/Purchases"/*NEGATE*/,
"Investing Activities.Equipment Sales/Purchases"/*NEGATE*/,
"Financing Activities.Capital/Stock",
"Financing Activities.Long Term Debts",
"Financing Activities.Short Term Debts",
"Financing Activities.Equity Securities",
"Financing Activities.Dividends Declared"/*NEGATE*/,
""
};
        #endregion

        #region "FORM EVENTS..."
        public segmentValuesDiag()
        {
            InitializeComponent();
        }

        private void segmentValuesDiag_Load(object sender, EventArgs e)
        {
            Color[] clrs = cmnCde.getColors();
            this.BackColor = clrs[0];
            cmnCde.DefaultPrvldgs = this.dfltPrvldgs;
            if (this.sysClsfctn == "NaturalAccount")
            {
                this.ntrlAccntGroupBox.Visible = true;
                this.sysCrncyLabel.Visible = false;
                this.accntCrncyNmTextBox.Visible = false;
                this.accntCurrButton.Visible = false;
                this.accntCurrIDTextBox.Visible = false;
            }
            else if (this.sysClsfctn == "Currency")
            {
                this.ntrlAccntGroupBox.Visible = false;
                this.sysCrncyLabel.Visible = true;
                this.accntCrncyNmTextBox.Visible = true;
                this.accntCurrButton.Visible = true;
                this.accntCurrIDTextBox.Visible = true;
            }
            else
            {
                this.ntrlAccntGroupBox.Visible = false;
                this.sysCrncyLabel.Visible = false;
                this.accntCrncyNmTextBox.Visible = false;
                this.accntCurrButton.Visible = false;
                this.accntCurrIDTextBox.Visible = false;
            }
            this.disableFormButtons();
            this.loadPanel();
        }

        public void disableFormButtons()
        {
            bool vwSQL = cmnCde.test_prmssns(this.dfltPrvldgs[12]);
            bool rcHstry = cmnCde.test_prmssns(this.dfltPrvldgs[13]);
            this.addRecsP = cmnCde.test_prmssns(this.dfltPrvldgs[14]);
            this.editRecsP = cmnCde.test_prmssns(this.dfltPrvldgs[15]);
            this.delRecsP = this.editRecsP;

            if (this.editRec == false && this.addRec == false)
            {
                this.saveButton.Enabled = false;
            }
            this.addButton.Enabled = this.addRecsP;
            this.editButton.Enabled = this.editRecsP;
            this.delButton.Enabled = this.delRecsP;
            this.vwSQLButton.Enabled = vwSQL;
            this.rcHstryButton.Enabled = rcHstry;
        }
        #endregion

        #region "SEGMENT VALUES..."
        public void loadPanel()
        {
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
                this.dsplySizeComboBox.Text = cmnCde.get_CurPlcy_Mx_Dsply_Recs().ToString();
            }
            this.is_last_rec = false;
            this.totl_rec = cmnCde.Big_Val;
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
            cmnCde.navFuncts.FindNavigationIndices(
              long.Parse(this.dsplySizeComboBox.Text), this.totl_rec);
            if (this.rec_cur_indx >= cmnCde.navFuncts.totalGroups)
            {
                this.rec_cur_indx = cmnCde.navFuncts.totalGroups - 1;
            }
            if (this.rec_cur_indx < 0)
            {
                this.rec_cur_indx = 0;
            }
            cmnCde.navFuncts.currentNavigationIndex = this.rec_cur_indx;
        }

        private void updtNavLabels()
        {
            this.moveFirstButton.Enabled = cmnCde.navFuncts.moveFirstBtnStatus();
            this.movePreviousButton.Enabled = cmnCde.navFuncts.movePrevBtnStatus();
            this.moveNextButton.Enabled = cmnCde.navFuncts.moveNextBtnStatus();
            this.moveLastButton.Enabled = cmnCde.navFuncts.moveLastBtnStatus();
            this.positionTextBox.Text = cmnCde.navFuncts.displayedRecordsNumbers();
            if (this.is_last_rec == true ||
              this.totl_rec != cmnCde.Big_Val)
            {
                this.totalRecsLabel.Text = cmnCde.navFuncts.totalRecordsLabel();
            }
            else
            {
                this.totalRecsLabel.Text = "of Total";
            }
        }

        private void populateListVw()
        {
            this.obey_evnts = false;
            DataSet dtst = this.get_Basic_SgmntVals(this.searchForTextBox.Text,
              this.searchInComboBox.Text, this.rec_cur_indx,
              int.Parse(this.dsplySizeComboBox.Text), this.segmentID);
            this.segmentValsListView.Items.Clear();
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                this.last_rec_num = cmnCde.navFuncts.startIndex() + i;
                ListViewItem nwItem = new ListViewItem(new string[] {
    (cmnCde.navFuncts.startIndex() + i).ToString(),
    dtst.Tables[0].Rows[i][3].ToString(),
    dtst.Tables[0].Rows[i][1].ToString(),
    dtst.Tables[0].Rows[i][2].ToString(),
    dtst.Tables[0].Rows[i][0].ToString()});
                this.segmentValsListView.Items.Add(nwItem);
            }
            this.correctNavLbls(dtst);
            if (this.segmentValsListView.Items.Count > 0)
            {
                this.obey_evnts = true;
                this.segmentValsListView.Items[0].Selected = true;
            }
            else
            {
                this.populateDet(-10000);
            }
            this.obey_evnts = true;
        }

        private void populateDet(int segmentValID)
        {
            if (this.editRec == false)
            {
                this.clearDetInfo();
                this.disableDetEdit();
            }
            this.obey_evnts = false;
            DataSet dtst = this.get_One_SgmntValDet(segmentValID);
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                this.segmentIDTextBox.Text = dtst.Tables[0].Rows[i][1].ToString();
                this.segmentValIDTextBox.Text = dtst.Tables[0].Rows[i][0].ToString();
                this.segmentValTextBox.Text = dtst.Tables[0].Rows[i][2].ToString();
                this.segmentValDescTextBox.Text = dtst.Tables[0].Rows[i][3].ToString();
                this.prntValIDTextBox.Text = dtst.Tables[0].Rows[i][7].ToString();
                this.prntValTextBox.Text = cmnCde.getSegmentVal(int.Parse(this.prntValIDTextBox.Text)) + "." +
                        cmnCde.getSegmentValDesc(int.Parse(this.prntValIDTextBox.Text));

                this.dpndntSgmntValIDTextBox.Text = dtst.Tables[0].Rows[i][29].ToString();
                this.dpndntSgmntIDTextBox.Text = dtst.Tables[0].Rows[i][28].ToString();
                this.dpndntSgmntValTextBox.Text = cmnCde.getSegmentVal(int.Parse(this.dpndntSgmntValIDTextBox.Text)) + "." +
                        cmnCde.getSegmentValDesc(int.Parse(this.dpndntSgmntValIDTextBox.Text));

                this.isValEnbldCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][6].ToString());
                if (editRec == false)
                {
                    this.grpComboBox.Items.Clear();
                    this.grpComboBox.Items.Add(dtst.Tables[0].Rows[i][4].ToString());
                    this.accClsfctnComboBox.Items.Clear();
                    this.accClsfctnComboBox.Items.Add(dtst.Tables[0].Rows[i][24].ToString());
                }
                this.grpComboBox.SelectedItem = dtst.Tables[0].Rows[i][4].ToString();
                this.accClsfctnComboBox.SelectedItem = dtst.Tables[0].Rows[i][24].ToString();
                this.grpNmIDTextBox.Text = dtst.Tables[0].Rows[i][5].ToString();
                if (this.grpComboBox.Text == "Divisions/Groups")
                {
                    this.grpNmTextBox.Text = cmnCde.getDivName(int.Parse(dtst.Tables[0].Rows[i][5].ToString()));
                }
                else if (this.grpComboBox.Text == "Grade")
                {
                    this.grpNmTextBox.Text = cmnCde.getGrdName(int.Parse(dtst.Tables[0].Rows[i][5].ToString()));
                }
                else if (this.grpComboBox.Text == "Job")
                {
                    this.grpNmTextBox.Text = cmnCde.getJobName(int.Parse(dtst.Tables[0].Rows[i][5].ToString()));
                }
                else if (this.grpComboBox.Text == "Position")
                {
                    this.grpNmTextBox.Text = cmnCde.getPosName(int.Parse(dtst.Tables[0].Rows[i][5].ToString()));
                }
                else if (this.grpComboBox.Text == "Site/Location")
                {
                    this.grpNmTextBox.Text = cmnCde.getSiteNameDesc(int.Parse(dtst.Tables[0].Rows[i][5].ToString()));
                }
                else if (this.grpComboBox.Text == "Person Type")
                {
                    this.grpNmTextBox.Text = cmnCde.getPssblValNm(int.Parse(dtst.Tables[0].Rows[i][5].ToString()));
                }
                else if (this.grpComboBox.Text == "Working Hour Type")
                {
                    this.grpNmTextBox.Text = cmnCde.getWkhName(int.Parse(dtst.Tables[0].Rows[i][5].ToString()));
                }
                else if (this.grpComboBox.Text == "Gathering Type")
                {
                    this.grpNmTextBox.Text = cmnCde.getGathName(int.Parse(dtst.Tables[0].Rows[i][5].ToString()));
                }
                else if (this.grpComboBox.Text == "Single Person")
                {
                    this.grpNmTextBox.Text = cmnCde.getPrsnName(long.Parse(dtst.Tables[0].Rows[i][5].ToString()));
                }

                this.accntTypeComboBox.Items.Clear();
                if (dtst.Tables[0].Rows[i][14].ToString() == "A")
                {
                    this.accntTypeComboBox.Items.Add("A -ASSET");
                }
                else if (dtst.Tables[0].Rows[i][14].ToString() == "EQ")
                {
                    this.accntTypeComboBox.Items.Add("EQ-EQUITY");
                }
                else if (dtst.Tables[0].Rows[i][14].ToString() == "L")
                {
                    this.accntTypeComboBox.Items.Add("L -LIABILITY");
                }
                else if (dtst.Tables[0].Rows[i][14].ToString() == "R")
                {
                    this.accntTypeComboBox.Items.Add("R -REVENUE");
                }
                else if (dtst.Tables[0].Rows[i][14].ToString() == "EX")
                {
                    this.accntTypeComboBox.Items.Add("EX-EXPENSE");
                }
                if (this.accntTypeComboBox.Items.Count > 0)
                {
                    this.accntTypeComboBox.SelectedIndex = 0;
                }

                this.isPrntAccntsCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][15].ToString());
                this.isRetEarnsCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][16].ToString());
                this.isNetIncmCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][17].ToString());
                this.isContraCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][13].ToString());
                this.rptLnNoUpDown.Value = Decimal.Parse(dtst.Tables[0].Rows[i][19].ToString());
                this.hasSubldgrCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][20].ToString());
                this.isSuspensCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][23].ToString());
                this.enblCmbtnsCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][27].ToString());
                this.cntrlAccntIDTextBox.Text = dtst.Tables[0].Rows[i][21].ToString();
                this.cntrlAccntTextBox.Text = cmnCde.getSegmentVal(int.Parse(dtst.Tables[0].Rows[i][21].ToString())) + "." +
                        cmnCde.getSegmentValDesc(int.Parse(dtst.Tables[0].Rows[i][21].ToString()));
                this.accntCurrIDTextBox.Text = dtst.Tables[0].Rows[i][22].ToString();
                this.accntCrncyNmTextBox.Text = cmnCde.getPssblValNm(int.Parse(dtst.Tables[0].Rows[i][22].ToString()));
                this.parentAccntIDTextBox.Text = dtst.Tables[0].Rows[i][25].ToString();
                this.parentAccntTextBox.Text = cmnCde.getAccntNum(int.Parse(dtst.Tables[0].Rows[i][25].ToString()))
                    + "." + cmnCde.getAccntName(int.Parse(dtst.Tables[0].Rows[i][25].ToString()));

                if (this.editRec == true)
                {
                    if (this.isSgmntValInUse(int.Parse(dtst.Tables[0].Rows[i][0].ToString()), int.Parse(dtst.Tables[0].Rows[i][26].ToString())))
                    {
                        this.isPrntAccntsCheckBox.Enabled = false;
                        this.isContraCheckBox.Enabled = false;
                        this.isRetEarnsCheckBox.Enabled = false;
                        this.isNetIncmCheckBox.Enabled = false;
                        this.accntTypeComboBox.Enabled = false;
                        this.hasSubldgrCheckBox.Enabled = false;
                        this.cntrlAccntTextBox.Enabled = false;
                        this.cntrlAccntButton.Enabled = false;
                        this.accntCrncyNmTextBox.Enabled = false;
                        this.accntCurrButton.Enabled = false;
                        //this.enblCmbtnsCheckBox.Enabled = false;
                    }
                    else
                    {
                        this.isPrntAccntsCheckBox.Enabled = true;
                        this.isContraCheckBox.Enabled = true;
                        this.isRetEarnsCheckBox.Enabled = true;
                        this.isNetIncmCheckBox.Enabled = true;
                        this.accntTypeComboBox.Enabled = true;
                        this.hasSubldgrCheckBox.Enabled = true;
                        this.cntrlAccntTextBox.Enabled = true;
                        this.cntrlAccntButton.Enabled = true;
                        this.accntCrncyNmTextBox.Enabled = true;
                        this.accntCurrButton.Enabled = true;
                        //this.enblCmbtnsCheckBox.Enabled = true;
                    }
                }
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
            else if (this.totl_rec == cmnCde.Big_Val
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
                this.totl_rec = this.get_Total_SgmntVals(this.searchForTextBox.Text,
                  this.searchInComboBox.Text, this.segmentID);
                this.updtTotals();
                this.rec_cur_indx = cmnCde.navFuncts.totalGroups - 1;
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
            this.segmentIDTextBox.Text = "-1";
            this.segmentValIDTextBox.Text = "-1";
            this.segmentValTextBox.Text = "";
            this.segmentValDescTextBox.Text = "";
            this.prntValIDTextBox.Text = "-1";
            this.prntValTextBox.Text = "";
            this.dpndntSgmntValIDTextBox.Text = "-1";
            this.dpndntSgmntIDTextBox.Text = "-1";
            this.dpndntSgmntValTextBox.Text = "";
            this.isValEnbldCheckBox.Checked = true;
            this.grpComboBox.Items.Clear();
            this.accClsfctnComboBox.Items.Clear();
            this.grpNmTextBox.Text = "";
            this.grpNmIDTextBox.Text = "-1";

            this.accntTypeComboBox.Items.Clear();

            this.isPrntAccntsCheckBox.Checked = false;
            this.isRetEarnsCheckBox.Checked = false;
            this.isNetIncmCheckBox.Checked = false;
            this.isContraCheckBox.Checked = false;
            this.rptLnNoUpDown.Value = 100;
            this.hasSubldgrCheckBox.Checked = false;
            this.isSuspensCheckBox.Checked = false;
            this.enblCmbtnsCheckBox.Checked = true;
            this.cntrlAccntIDTextBox.Text = "-1";
            this.cntrlAccntTextBox.Text = "";
            this.accntCurrIDTextBox.Text = "-1";
            this.accntCrncyNmTextBox.Text = "";
            this.parentAccntIDTextBox.Text = "-1";
            this.parentAccntTextBox.Text = "";

            this.isPrntAccntsCheckBox.Enabled = true;
            this.isContraCheckBox.Enabled = true;
            this.isRetEarnsCheckBox.Enabled = true;
            this.isNetIncmCheckBox.Enabled = true;
            this.accntTypeComboBox.Enabled = true;
            this.hasSubldgrCheckBox.Enabled = true;
            this.cntrlAccntButton.Enabled = true;
            this.cntrlAccntTextBox.Enabled = true;
            this.accntCurrButton.Enabled = true;
            this.accntCrncyNmTextBox.Enabled = true;
            this.enblCmbtnsCheckBox.Enabled = true;

            this.obey_evnts = true;
        }

        private void prpareForDetEdit()
        {
            this.obey_evnts = false;
            this.saveButton.Enabled = true;
            this.segmentValTextBox.ReadOnly = false;
            this.segmentValTextBox.BackColor = Color.FromArgb(255, 255, 118);
            this.segmentValDescTextBox.ReadOnly = false;
            this.segmentValDescTextBox.BackColor = Color.FromArgb(255, 255, 118);
            this.prntValTextBox.ReadOnly = false;
            this.prntValTextBox.BackColor = Color.White;
            this.dpndntSgmntValTextBox.ReadOnly = false;
            this.dpndntSgmntValTextBox.BackColor = Color.White;
            this.grpNmTextBox.ReadOnly = false;
            this.grpNmTextBox.BackColor = Color.White;

            this.grpComboBox.BackColor = Color.FromArgb(255, 255, 118);
            string orgItm = this.grpComboBox.Text;
            this.grpComboBox.Items.Clear();
            this.grpComboBox.Items.Add("Everyone");
            this.grpComboBox.Items.Add("Divisions/Groups");
            this.grpComboBox.Items.Add("Grade");
            this.grpComboBox.Items.Add("Job");
            this.grpComboBox.Items.Add("Position");
            this.grpComboBox.Items.Add("Site/Location");
            this.grpComboBox.Items.Add("Person Type");
            this.grpComboBox.Items.Add("Single Person");
            if (this.editRec == true)
            {
                this.grpComboBox.SelectedItem = orgItm;
            }
            this.accClsfctnComboBox.BackColor = Color.White;

            this.accntCrncyNmTextBox.ReadOnly = false;
            this.accntCrncyNmTextBox.BackColor = Color.FromArgb(255, 255, 118);

            this.rptLnNoUpDown.Increment = 1;
            this.rptLnNoUpDown.ReadOnly = false;
            this.rptLnNoUpDown.BackColor = Color.White;
            orgItm = this.accntTypeComboBox.Text;
            this.accntTypeComboBox.Items.Clear();
            this.accntTypeComboBox.Items.Add("A -ASSET");
            this.accntTypeComboBox.Items.Add("EQ-EQUITY");
            this.accntTypeComboBox.Items.Add("L -LIABILITY");
            this.accntTypeComboBox.Items.Add("R -REVENUE");
            this.accntTypeComboBox.Items.Add("EX-EXPENSE");
            if (this.editRec == true)
            {
                this.accntTypeComboBox.SelectedItem = orgItm;
            }

            orgItm = this.accClsfctnComboBox.Text;
            this.accClsfctnComboBox.Items.Clear();
            for (int a = 0; a < this.cashFlowClsfctns.Length; a++)
            {
                this.accClsfctnComboBox.Items.Add(this.cashFlowClsfctns[a]);
            }
            if (this.editRec == true)
            {
                this.accClsfctnComboBox.SelectedItem = orgItm;
            }
            this.obey_evnts = true;
        }

        private void disableDetEdit()
        {
            this.addRec = false;
            this.editRec = false;
            this.saveButton.Enabled = false;
            this.editButton.Enabled = this.addRecsP;
            this.addButton.Enabled = this.editRecsP;
            this.delButton.Enabled = this.delRecsP;
            this.editButton.Text = "EDIT";
            this.segmentValTextBox.ReadOnly = true;
            this.segmentValTextBox.BackColor = Color.WhiteSmoke;
            this.segmentValDescTextBox.ReadOnly = true;
            this.segmentValDescTextBox.BackColor = Color.WhiteSmoke;
            this.prntValTextBox.ReadOnly = true;
            this.prntValTextBox.BackColor = Color.WhiteSmoke;
            this.dpndntSgmntValTextBox.ReadOnly = true;
            this.dpndntSgmntValTextBox.BackColor = Color.WhiteSmoke;
            this.accClsfctnComboBox.BackColor = Color.WhiteSmoke;
            this.grpComboBox.BackColor = Color.WhiteSmoke;
            this.grpNmTextBox.ReadOnly = true;
            this.grpNmTextBox.BackColor = Color.WhiteSmoke;
            this.parentAccntTextBox.ReadOnly = true;
            this.parentAccntTextBox.BackColor = Color.WhiteSmoke;
            this.cntrlAccntTextBox.ReadOnly = true;
            this.cntrlAccntTextBox.BackColor = Color.WhiteSmoke;
            this.accntCrncyNmTextBox.ReadOnly = true;
            this.accntCrncyNmTextBox.BackColor = Color.WhiteSmoke;

            this.rptLnNoUpDown.Increment = 0;
            this.rptLnNoUpDown.ReadOnly = true;
            this.rptLnNoUpDown.BackColor = Color.WhiteSmoke;
        }
        #endregion

        private void prntValButton_Click(object sender, EventArgs e)
        {
            if (cmnCde.test_prmssns(this.dfltPrvldgs[15]) == false)
            {
                cmnCde.showMsg("You don't have permission to perform" +
                 " this action!\nContact your System Administrator!", 0);
                return;
            }
            if (this.addRec == false && this.editRec == false)
            {
                this.editButton.PerformClick();
            }
            if (this.addRec == false && this.editRec == false)
            {
                cmnCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }
            string[] selVals = new string[1];
            selVals[0] = this.prntValIDTextBox.Text;
            DialogResult dgRes = cmnCde.showPssblValDiag(
                cmnCde.getLovID("Account Segment Values"), ref selVals, true, false, this.segmentID,
             this.srchWrd, "Both", false);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.prntValIDTextBox.Text = this.getSgmntValID(selVals[i], this.segmentID).ToString();
                    this.prntValTextBox.Text = cmnCde.getSegmentVal(int.Parse(this.prntValIDTextBox.Text)) + "." +
                        cmnCde.getSegmentValDesc(int.Parse(this.prntValIDTextBox.Text));
                }
            }
        }

        private void accntCurrButton_Click(object sender, EventArgs e)
        {
            if (this.addRec == false && this.editRec == false)
            {
                this.editButton.PerformClick();
            }
            if (this.addRec == false && this.editRec == false)
            {
                cmnCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }

            int[] selVals = new int[1];
            selVals[0] = int.Parse(this.accntCurrIDTextBox.Text);
            DialogResult dgRes = cmnCde.showPssblValDiag(
             cmnCde.getLovID("Currencies"), ref selVals,
             true, false);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.accntCurrIDTextBox.Text = selVals[i].ToString();
                    this.accntCrncyNmTextBox.Text = cmnCde.getPssblValNm(selVals[i]);
                }
            }
        }

        private void grpComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.shdObeyEvts() == false)
            {
                return;
            }
            this.grpNmIDTextBox.Text = "-1";
            this.grpNmTextBox.Text = "";
            if (this.grpComboBox.Text == "Everyone")
            {
                this.grpNmTextBox.BackColor = Color.WhiteSmoke;
                this.grpNmTextBox.Enabled = false;
                this.grpNmButton.Enabled = false;
            }
            else
            {
                this.grpNmTextBox.BackColor = Color.FromArgb(255, 255, 118);
                this.grpNmTextBox.Enabled = true;
                this.grpNmButton.Enabled = true;
            }
            if (this.prsnIDs[0] > 0 && this.grpComboBox.Text == "Single Person")
            {
                this.grpComboBox.SelectedItem = "Single Person";
                this.grpNmTextBox.Text = cmnCde.getPrsnName(this.prsnIDs[0]);
            }
        }

        private void grpNmButton_Click(object sender, EventArgs e)
        {
            if (this.addRec == false && this.editRec == false)
            {
                this.editButton.PerformClick();
            }
            if (this.addRec == false && this.editRec == false)
            {
                cmnCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }
            if (this.grpComboBox.Text == "")
            {
                cmnCde.showMsg("Please select a Group Type!", 0);
                return;
            }
            string[] selVals = new string[1];
            selVals[0] = this.grpNmIDTextBox.Text;
            string grpCmbo = "";
            if (this.grpComboBox.Text == "Divisions/Groups")
            {
                grpCmbo = "Divisions/Groups";
            }
            else if (this.grpComboBox.Text == "Grade")
            {
                grpCmbo = "Grades";
            }
            else if (this.grpComboBox.Text == "Job")
            {
                grpCmbo = "Jobs";
            }
            else if (this.grpComboBox.Text == "Position")
            {
                grpCmbo = "Positions";
            }
            else if (this.grpComboBox.Text == "Site/Location")
            {
                grpCmbo = "Sites/Locations";
            }
            else if (this.grpComboBox.Text == "Person Type")
            {
                grpCmbo = "Person Types";
            }
            else if (this.grpComboBox.Text == "Working Hour Type")
            {
                grpCmbo = "Working Hours";
            }
            else if (this.grpComboBox.Text == "Gathering Type")
            {
                grpCmbo = "Gathering Types";
            }
            else if (this.grpComboBox.Text == "Single Person")
            {
                grpCmbo = "Active Persons";
            }
            int[] selVal1s = new int[1];

            DialogResult dgRes;
            if (this.grpComboBox.Text != "Person Type")
            {
                dgRes = cmnCde.showPssblValDiag(
                cmnCde.getLovID(grpCmbo), ref selVals, true, true, this.orgID,
               this.srchWrd, "Both", true);
            }
            else
            {
                dgRes = cmnCde.showPssblValDiag(
                cmnCde.getLovID("Person Types"), ref selVal1s, true, true,
               this.srchWrd, "Both", true);
            }
            int slctn = 0;
            if (this.grpComboBox.Text != "Person Type")
            {
                slctn = selVals.Length;
            }
            else
            {
                slctn = selVal1s.Length;
            }
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < slctn; i++)
                {
                    this.grpNmIDTextBox.Text = selVals[i];
                    if (this.grpComboBox.Text == "Divisions/Groups")
                    {
                        this.grpNmTextBox.Text = cmnCde.getDivName(int.Parse(selVals[i]));
                    }
                    else if (this.grpComboBox.Text == "Grade")
                    {
                        this.grpNmTextBox.Text = cmnCde.getGrdName(int.Parse(selVals[i]));
                    }
                    else if (this.grpComboBox.Text == "Job")
                    {
                        this.grpNmTextBox.Text = cmnCde.getJobName(int.Parse(selVals[i]));
                    }
                    else if (this.grpComboBox.Text == "Position")
                    {
                        this.grpNmTextBox.Text = cmnCde.getPosName(int.Parse(selVals[i]));
                    }
                    else if (this.grpComboBox.Text == "Site/Location")
                    {
                        this.grpNmTextBox.Text = cmnCde.getSiteNameDesc(int.Parse(selVals[i]));
                    }
                    else if (this.grpComboBox.Text == "Person Type")
                    {
                        this.grpNmIDTextBox.Text = selVal1s[i].ToString();
                        this.grpNmTextBox.Text = cmnCde.getPssblValNm(selVal1s[i]);
                    }
                    else if (this.grpComboBox.Text == "Working Hour Type")
                    {
                        this.grpNmTextBox.Text = cmnCde.getWkhName(int.Parse(selVals[i]));
                    }
                    else if (this.grpComboBox.Text == "Gathering Type")
                    {
                        this.grpNmTextBox.Text = cmnCde.getGathName(int.Parse(selVals[i]));
                    }
                    else if (this.grpComboBox.Text == "Single Person")
                    {
                        this.grpNmIDTextBox.Text = cmnCde.getPrsnID(selVals[i]).ToString();
                        this.grpNmTextBox.Text = cmnCde.getPrsnName(selVals[i]);
                    }
                }
            }
        }

        private void cntrlAccntButton_Click(object sender, EventArgs e)
        {
            if (cmnCde.test_prmssns(this.dfltPrvldgs[15]) == false)
            {
                cmnCde.showMsg("You don't have permission to perform" +
                 " this action!\nContact your System Administrator!", 0);
                return;
            }
            if (this.addRec == false && this.editRec == false)
            {
                this.editButton.PerformClick();
            }
            if (this.addRec == false && this.editRec == false)
            {
                cmnCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }
            string[] selVals = new string[1];
            selVals[0] = this.cntrlAccntIDTextBox.Text;
            DialogResult dgRes = cmnCde.showPssblValDiag(
                cmnCde.getLovID("Control Account Segment Values"), ref selVals, true, false, this.segmentID,
             this.srchWrd, "Both", false);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.cntrlAccntIDTextBox.Text = selVals[i];
                    this.cntrlAccntTextBox.Text = cmnCde.getSegmentVal(int.Parse(selVals[i])) + "." +
                        cmnCde.getSegmentValDesc(int.Parse(selVals[i]));
                }
            }
        }

        private void mappedAcntLOVSearch()
        {
            if (!this.parentAccntTextBox.Text.Contains("%"))
            {
                this.parentAccntTextBox.Text = "%" + this.parentAccntTextBox.Text.Replace(" ", "%") + "%";
                this.parentAccntIDTextBox.Text = "-1";
            }
            int grpOrgID = cmnCde.getGrpOrgID();
            string[] selVals = new string[1];
            selVals[0] = this.parentAccntIDTextBox.Text;
            DialogResult dgRes = cmnCde.showPssblValDiag(
              cmnCde.getLovID("Transaction Accounts"), ref selVals,
              true, true, grpOrgID,
             this.parentAccntTextBox.Text, "Both", true);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.parentAccntIDTextBox.Text = selVals[i];
                    this.parentAccntTextBox.Text = cmnCde.getAccntNum(int.Parse(selVals[i]))
                    + "." + cmnCde.getAccntName(int.Parse(selVals[i]));
                }
            }
        }

        private void parntAccntButton_Click(object sender, EventArgs e)
        {
            if (this.addRec == false && this.editRec == false)
            {
                this.editButton.PerformClick();
            }
            if (this.addRec == false && this.editRec == false)
            {
                cmnCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }
            this.mappedAcntLOVSearch();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            this.disableFormButtons();
            this.loadPanel();
            this.Refresh();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            this.searchInComboBox.SelectedIndex = 0;
            this.searchForTextBox.Text = "%";
            this.dsplySizeComboBox.Text = cmnCde.get_CurPlcy_Mx_Dsply_Recs().ToString();
            this.rec_cur_indx = 0;
            if (this.editButton.Text == "STOP")
            {
                this.editButton.PerformClick();
            }
            this.goButton_Click(this.goButton, e);
        }

        private void isValEnbldCheckBox_CheckedChanged(object sender, EventArgs e)
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
                this.isValEnbldCheckBox.Checked = !this.isValEnbldCheckBox.Checked;
            }
        }

        private void isPrntAccntsCheckBox_CheckedChanged(object sender, EventArgs e)
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
                this.isPrntAccntsCheckBox.Checked = !this.isPrntAccntsCheckBox.Checked;
            }
        }

        private void isRetEarnsCheckBox_CheckedChanged(object sender, EventArgs e)
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
                this.isRetEarnsCheckBox.Checked = !this.isRetEarnsCheckBox.Checked;
            }
        }

        private void isSuspensCheckBox_CheckedChanged(object sender, EventArgs e)
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
                this.isSuspensCheckBox.Checked = !this.isSuspensCheckBox.Checked;
            }
        }

        private void isContraCheckBox_CheckedChanged(object sender, EventArgs e)
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
                this.isContraCheckBox.Checked = !this.isContraCheckBox.Checked;
            }
        }

        private void isNetIncmCheckBox_CheckedChanged(object sender, EventArgs e)
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
                this.isNetIncmCheckBox.Checked = !this.isNetIncmCheckBox.Checked;
            }
        }

        private void hasSubldgrCheckBox_CheckedChanged(object sender, EventArgs e)
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
                this.hasSubldgrCheckBox.Checked = !this.hasSubldgrCheckBox.Checked;
            }
        }

        private void segmentValsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.shdObeyEvts() == false || this.segmentValsListView.SelectedItems.Count > 1)
            {
                return;
            }
            if (this.segmentValsListView.SelectedItems.Count > 0)
            {
                this.populateDet(int.Parse(this.segmentValsListView.SelectedItems[0].SubItems[4].Text));
            }
            else
            {
                this.populateDet(-12345);
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

        private void rcHstryButton_Click(object sender, EventArgs e)
        {
            if (this.segmentValIDTextBox.Text == "-1"
         || this.segmentValIDTextBox.Text == "")
            {
                cmnCde.showMsg("Please select an Account First!", 0);
                return;
            }
            cmnCde.showRecHstry(cmnCde.get_Gnrl_Rec_Hstry(int.Parse(this.segmentValIDTextBox.Text), "org.org_segment_values", "segment_value_id"), 13);
        }

        private void vwSQLButton_Click(object sender, EventArgs e)
        {
            cmnCde.showSQL(this.segmentValsSQL, 12);
        }

        private void delButton_Click(object sender, EventArgs e)
        {
            if (cmnCde.test_prmssns(this.dfltPrvldgs[15]) == false)
            {
                cmnCde.showMsg("You don't have permission to perform" +
                 " this action!\nContact your System Administrator!", 0);
                return;
            }
            if (this.segmentValsListView.SelectedItems.Count <= 0)
            {
                cmnCde.showMsg("Please select the record to delete!", 0);
                return;
            }
            int segmentvalid = int.Parse(this.segmentValsListView.SelectedItems[0].SubItems[4].Text);
            if (this.isSgmntValInUse(segmentvalid, this.segmentNum))
            {
                cmnCde.showMsg("Cannot delete Segment Values that have been used to Create Accounts!", 0);
                return;
            }

            if (cmnCde.showMsg("Are you sure you want to DELETE the selected Value?" +
             "\r\nThis action cannot be undone!", 1) == DialogResult.No)
            {
                //cmnCde.showMsg("Operation Cancelled!", 4);
                return;
            }
            this.deleteSgmntVal(segmentvalid, this.segmentValsListView.SelectedItems[0].SubItems[1].Text);
            this.loadPanel();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (this.addRec == true)
            {
                if (cmnCde.test_prmssns(this.dfltPrvldgs[15]) == false)
                {
                    cmnCde.showMsg("You don't have permission to perform" +
                     " this action!\nContact your System Administrator!", 0);
                    return;
                }
            }
            else
            {
                if (cmnCde.test_prmssns(this.dfltPrvldgs[15]) == false)
                {
                    cmnCde.showMsg("You don't have permission to perform" +
                     " this action!\nContact your System Administrator!", 0);
                    return;
                }
            }
            if (this.segmentValTextBox.Text == "")
            {
                cmnCde.showMsg("Please enter Segment Value!", 0);
                return;
            }
            if (this.segmentValDescTextBox.Text == "")
            {
                cmnCde.showMsg("Please enter Value Description!", 0);
                return;
            }
            if (this.sysClsfctn == "NaturalAccount")
            {
                if (this.accntTypeComboBox.Text == "")
                {
                    cmnCde.showMsg("Please select an account Type!", 0);
                    return;
                }
                if (this.isRetEarnsCheckBox.Checked == true && this.isPrntAccntsCheckBox.Checked == true)
                {
                    cmnCde.showMsg("A Parent account cannot be used as Retained Earnings Account!", 0);
                    return;
                }
                if (this.isRetEarnsCheckBox.Checked == true && this.isContraCheckBox.Checked == true)
                {
                    cmnCde.showMsg("A contra account cannot be used as Retained Earnings Account!", 0);
                    return;
                }
                if (this.isRetEarnsCheckBox.Checked == true && this.isValEnbldCheckBox.Checked == false)
                {
                    cmnCde.showMsg("A Retained Earnings Account cannot be disabled!", 0);
                    return;
                }

                if (this.isSuspensCheckBox.Checked == true && this.accntTypeComboBox.Text != "A -ASSET")
                {
                    cmnCde.showMsg("The account type of the Suspense Account must be ASSET", 0);
                    return;
                }

                if (this.isRetEarnsCheckBox.Checked == true && this.accntTypeComboBox.Text != "EQ-EQUITY")
                {
                    cmnCde.showMsg("The account type of a Retained Earnings Account must be NET WORTH", 0);
                    return;
                }
                if (this.isNetIncmCheckBox.Checked == true && this.isPrntAccntsCheckBox.Checked == true)
                {
                    cmnCde.showMsg("A Parent account cannot be used as Net Income Account!", 0);
                    return;
                }
                if (this.isNetIncmCheckBox.Checked == true && this.isContraCheckBox.Checked == true)
                {
                    cmnCde.showMsg("A contra account cannot be used as Net Income Account!", 0);
                    return;
                }
                if (this.isNetIncmCheckBox.Checked == true && this.isValEnbldCheckBox.Checked == false)
                {
                    cmnCde.showMsg("A Net Income Account cannot be disabled!", 0);
                    return;
                }
                if (this.isNetIncmCheckBox.Checked == true && this.accntTypeComboBox.Text != "EQ-EQUITY")
                {
                    cmnCde.showMsg("The account type of a Net Income Account must be NET WORTH", 0);
                    return;
                }
                if (this.isRetEarnsCheckBox.Checked == true && this.isNetIncmCheckBox.Checked == true)
                {
                    cmnCde.showMsg("Same Account cannot be Retained Earnings and Net Income at same time!", 0);
                    return;
                }
                if (this.isRetEarnsCheckBox.Checked == true && this.hasSubldgrCheckBox.Checked == true)
                {
                    cmnCde.showMsg("Retained Earnings account cannot have sub-ledgers!", 0);
                    return;
                }
                if (this.isNetIncmCheckBox.Checked == true && this.hasSubldgrCheckBox.Checked == true)
                {
                    cmnCde.showMsg("Net Income account cannot have sub-ledgers!", 0);
                    return;
                }
                if (this.isContraCheckBox.Checked == true && this.hasSubldgrCheckBox.Checked == true)
                {
                    cmnCde.showMsg("The system does not support Sub-Ledgers on Contra-Accounts!", 0);
                    return;
                }
                if (this.isPrntAccntsCheckBox.Checked == true && this.hasSubldgrCheckBox.Checked == true)
                {
                    cmnCde.showMsg("Parent Account cannot have sub-ledgers!", 0);
                    return;
                }
                if (this.cntrlAccntIDTextBox.Text != "-1" && this.hasSubldgrCheckBox.Checked == true)
                {
                    cmnCde.showMsg("The system does not support Control Accounts reporting to other Control Account!", 0);
                    return;
                }
                if (this.cntrlAccntIDTextBox.Text != "-1" && this.prntValIDTextBox.Text != "-1")
                {
                    cmnCde.showMsg("An Account with a Control Account cannot have a Parent Account as well!", 0);
                    return;
                }
                if (this.parentAccntIDTextBox.Text != "-1")
                {
                    if (cmnCde.getAccntType(int.Parse(parentAccntIDTextBox.Text)) !=
                     this.accntTypeComboBox.Text.Substring(0, 2).Trim())
                    {
                        cmnCde.showMsg("Account Type does not match that of the Mapped Account", 0);
                        return;
                    }
                }
            }
            else if (this.sysClsfctn == "Currency")
            {
                if (this.accntCurrIDTextBox.Text == "-1" || this.accntCurrIDTextBox.Text == "")
                {
                    cmnCde.showMsg("System Currency Cannot be Empty!", 0);
                    return;
                }
            }

            int oldSgmntValID = this.getSgmntValID(this.segmentValTextBox.Text, this.segmentID);
            if (oldSgmntValID > 0 && this.addRec == true)
            {
                cmnCde.showMsg("Segment Value is already in available in this Segment!", 0);
                return;
            }
            if (oldSgmntValID > 0
             && this.editRec == true
             && oldSgmntValID.ToString() != this.segmentValIDTextBox.Text)
            {
                cmnCde.showMsg("New Segment Value is already in use in this Organization!", 0);
                return;
            }

            int oldSgmntNmID = this.getSgmntValDescID(this.segmentValDescTextBox.Text, this.segmentID);
            if (oldSgmntNmID > 0
             && this.addRec == true)
            {
                cmnCde.showMsg("Segment Description is already in use in this Segment!", 0);
                return;
            }
            if (oldSgmntNmID > 0
             && this.editRec == true
             && oldSgmntNmID.ToString() != this.segmentValIDTextBox.Text)
            {
                cmnCde.showMsg("New Segment Description is already in use in this Segment!", 0);
                return;
            }
            string accntType = "";
            if (this.accntTypeComboBox.Text != "")
            {
                accntType = this.accntTypeComboBox.Text.Substring(0, 2).Trim();
            }
            if (this.addRec == true)
            {
                this.createSgmntVal(this.orgID, this.segmentID, this.segmentValTextBox.Text, this.segmentValDescTextBox.Text
                    , this.grpComboBox.Text, this.grpNmIDTextBox.Text, this.isValEnbldCheckBox.Checked, int.Parse(this.prntValIDTextBox.Text), this.isContraCheckBox.Checked,
                    accntType, this.isPrntAccntsCheckBox.Checked, this.isRetEarnsCheckBox.Checked, this.isNetIncmCheckBox.Checked,
                    this.getAcctTypID(accntType), (int)this.rptLnNoUpDown.Value, this.hasSubldgrCheckBox.Checked, int.Parse(this.cntrlAccntIDTextBox.Text),
                    int.Parse(this.accntCurrIDTextBox.Text), this.isSuspensCheckBox.Checked, this.accClsfctnComboBox.Text,
                    int.Parse(this.parentAccntIDTextBox.Text), this.enblCmbtnsCheckBox.Checked, int.Parse(this.dpndntSgmntValIDTextBox.Text));
                this.saveButton.Enabled = false;
                this.addRec = false;
                this.editRec = false;
                this.editButton.Enabled = this.addRecsP;
                this.addButton.Enabled = this.editRecsP;
                this.delButton.Enabled = this.delRecsP;
                System.Windows.Forms.Application.DoEvents();
                this.loadPanel();
            }
            else if (this.editRec == true)
            {
                this.updateSgmntVal(int.Parse(this.segmentValIDTextBox.Text),
                this.segmentValTextBox.Text, this.segmentValDescTextBox.Text
                    , this.grpComboBox.Text, this.grpNmIDTextBox.Text, this.isValEnbldCheckBox.Checked
                    , int.Parse(this.prntValIDTextBox.Text), this.isContraCheckBox.Checked,
                    accntType, this.isPrntAccntsCheckBox.Checked, this.isRetEarnsCheckBox.Checked
                    , this.isNetIncmCheckBox.Checked, this.getAcctTypID(accntType),
                    (int)this.rptLnNoUpDown.Value, this.hasSubldgrCheckBox.Checked, int.Parse(this.cntrlAccntIDTextBox.Text),
                    int.Parse(this.accntCurrIDTextBox.Text), this.isSuspensCheckBox.Checked, this.accClsfctnComboBox.Text,
                    int.Parse(this.parentAccntIDTextBox.Text), this.segmentID, this.enblCmbtnsCheckBox.Checked, int.Parse(this.dpndntSgmntValIDTextBox.Text));
                if (this.segmentValsListView.SelectedItems.Count > 0)
                {
                    this.segmentValsListView.SelectedItems[0].SubItems[1].Text = this.segmentValTextBox.Text + "." + this.segmentValDescTextBox.Text;
                    this.segmentValsListView.SelectedItems[0].SubItems[2].Text = this.segmentValTextBox.Text;
                    this.segmentValsListView.SelectedItems[0].SubItems[3].Text = this.segmentValDescTextBox.Text;
                }
                cmnCde.showMsg("Record Saved!", 3);
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (this.editButton.Text == "EDIT")
            {
                if (cmnCde.test_prmssns(this.dfltPrvldgs[15]) == false)
                {
                    cmnCde.showMsg("You don't have permission to perform" +
                     " this action!\nContact your System Administrator!", 0);
                    return;
                }
                if (this.segmentValIDTextBox.Text == "" || this.segmentValIDTextBox.Text == "-1")
                {
                    cmnCde.showMsg("No record to Edit!", 0);
                    return;
                }
                if (this.isSgmntValInUse(int.Parse(this.segmentValIDTextBox.Text), this.segmentNum))
                {
                    this.isPrntAccntsCheckBox.Enabled = false;
                    this.isContraCheckBox.Enabled = false;
                    this.isRetEarnsCheckBox.Enabled = false;
                    this.isNetIncmCheckBox.Enabled = false;
                    this.hasSubldgrCheckBox.Enabled = false;
                    this.isSuspensCheckBox.Enabled = false;
                    this.accntTypeComboBox.Enabled = false;
                    this.cntrlAccntButton.Enabled = false;
                    this.cntrlAccntTextBox.Enabled = false;
                    this.accntCrncyNmTextBox.Enabled = false;
                    this.accntCurrButton.Enabled = false;
                }
                this.addRec = false;
                this.editRec = true;
                this.prpareForDetEdit();
                this.addButton.Enabled = false;
                this.editButton.Enabled = false;
                this.delButton.Enabled = false;
                this.editButton.Text = "STOP";
            }
            else
            {
                this.disableDetEdit();
                System.Windows.Forms.Application.DoEvents();
                this.loadPanel();
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (cmnCde.test_prmssns(this.dfltPrvldgs[15]) == false)
            {
                cmnCde.showMsg("You don't have permission to perform" +
                 " this action!\nContact your System Administrator!", 0);
                return;
            }
            this.clearDetInfo();
            this.addRec = true;
            this.editRec = false;
            this.prpareForDetEdit();
            this.addButton.Enabled = false;
            this.editButton.Enabled = false;
            this.delButton.Enabled = false;
            this.txtChngd = false;
            this.accntCurrIDTextBox.Text = cmnCde.getOrgFuncCurID(this.orgID).ToString();
            this.accntCrncyNmTextBox.Text = cmnCde.getPssblValNm(int.Parse(this.accntCurrIDTextBox.Text)) +
                  " - " + cmnCde.getPssblValDesc(int.Parse(this.accntCurrIDTextBox.Text));
            this.txtChngd = false;
        }

        private void exptChartButton_Click(object sender, EventArgs e)
        {
            string rspnse = Interaction.InputBox("How many Segment Values will you like to Export?" +
              "\r\n1=No Segment Value(Empty Template)" +
              "\r\n2=All Segment Values" +
              "\r\n3-Infinity=Specify the exact number of Segment Values to Export\r\n",
              "Rhomicom", "1", (cmnCde.myComputer.Screen.Bounds.Width / 2) - 170,
              (cmnCde.myComputer.Screen.Bounds.Height / 2) - 100);
            if (rspnse.Equals(string.Empty) || rspnse.Equals(null))
            {
                //cmnCde.showMsg("Operation Cancelled!", 4);
                return;
            }
            int rsponse = 0;
            bool rsps = int.TryParse(rspnse, out rsponse);
            if (rsps == false)
            {
                cmnCde.showMsg("Invalid Option! Expecting a Number Above Zero", 4);
                return;
            }
            if (rsponse < 1)
            {
                cmnCde.showMsg("Invalid Option! Expecting a Number Above Zero", 4);
                return;
            }
            this.exprtSgmntValTmp(rsponse, this.segmentID);
        }

        private void exprtSgmntValTmp(int exprtTyp, long sgmntID)
        {
            System.Windows.Forms.Application.DoEvents();
            cmnCde.clearPrvExclFiles();
            cmnCde.exclApp = new Microsoft.Office.Interop.Excel.Application();
            cmnCde.exclApp.WindowState = Excel.XlWindowState.xlNormal;
            cmnCde.exclApp.Visible = true;
            CommonCode.CommonCodes.SetWindowPos((IntPtr)cmnCde.exclApp.Hwnd, CommonCode.CommonCodes.HWND_TOP, 0, 0, 0, 0, CommonCode.CommonCodes.SWP_NOMOVE | CommonCode.CommonCodes.SWP_NOSIZE | CommonCode.CommonCodes.SWP_SHOWWINDOW);

            cmnCde.nwWrkBk = cmnCde.exclApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            cmnCde.nwWrkBk.Worksheets.Add(Type.Missing, Type.Missing, 1, Type.Missing);
            cmnCde.trgtSheets = new Excel.Worksheet[1];

            cmnCde.trgtSheets[0] = (Excel.Worksheet)cmnCde.nwWrkBk.Worksheets[1];

            cmnCde.trgtSheets[0].get_Range("B2:C3", Type.Missing).MergeCells = true;
            cmnCde.trgtSheets[0].get_Range("B2:C3", Type.Missing).Value2 = cmnCde.getOrgName(cmnCde.Org_id).ToUpper();
            cmnCde.trgtSheets[0].get_Range("B2:C3", Type.Missing).Font.Bold = true;
            cmnCde.trgtSheets[0].get_Range("B2:C3", Type.Missing).Font.Size = 13;
            cmnCde.trgtSheets[0].get_Range("B2:C3", Type.Missing).WrapText = true;
            cmnCde.trgtSheets[0].Shapes.AddPicture(cmnCde.getOrgImgsDrctry() + @"\" + cmnCde.Org_id + ".png",
                Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, 1, 1, 50, 50);

            ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[5, 1]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 162, 192));
            ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[5, 1]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(255, 255, 255));
            ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[5, 1]).Font.Bold = true;
            ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[5, 1]).Value2 = "No.";
            string[] hdngs = {"Segment Value**", "Value Description**", "Allowed Group Type", "Allowed Group Name",
                "Parent Segment Value Description", "Combinations Enabled?", "Dependent Segment Value",
                "System Currency Code*", "Account Type**", "Is Parent?(YES/NO)","Is Retained Earnings?(YES/NO)", "Is Net Income Account?(YES/NO)",
                "Is Contra Account?(YES/NO)", "Is Suspense Account?(YES/NO)", "Has SubLedgers?(YES/NO)", "Control Account Value",
                "Account Classification", "Mapped Group Account Number"};

            for (int a = 0; a < hdngs.Length; a++)
            {
                if (this.sysClsfctn == "Currency" && a > 7)
                {
                    continue;
                }
                else if (this.sysClsfctn != "NaturalAccount" && a > 6)
                {
                    continue;
                }
                ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[5, (a + 2)]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 162, 192));
                ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[5, (a + 2)]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(255, 255, 255));
                ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[5, (a + 2)]).Font.Bold = true;
                ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[5, (a + 2)]).Value2 = hdngs[a].ToUpper();
            }

            if (exprtTyp >= 2)
            {
                DataSet dtst = new DataSet();
                if (exprtTyp == 2)
                {
                    dtst = this.get_One_SgmntVals("%", "Segment Value", 0, 10000000, this.segmentID);
                }
                else if (exprtTyp >= 3)
                {
                    dtst = this.get_One_SgmntVals("%", "Segment Value", 0, exprtTyp, this.segmentID);
                }
                for (int a = 0; a < dtst.Tables[0].Rows.Count; a++)
                {
                    ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 1]).Value2 = a + 1;
                    ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 2]).Value2 = "'" + dtst.Tables[0].Rows[a][2].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 3]).Value2 = dtst.Tables[0].Rows[a][3].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 4]).Value2 = dtst.Tables[0].Rows[a][4].ToString();
                    string grpName = "";
                    string grpType = dtst.Tables[0].Rows[a][4].ToString();
                    if (grpType == "Divisions/Groups")
                    {
                        grpName = cmnCde.getDivName(int.Parse(dtst.Tables[0].Rows[a][5].ToString()));
                    }
                    else if (grpType == "Grade")
                    {
                        grpName = cmnCde.getGrdName(int.Parse(dtst.Tables[0].Rows[a][5].ToString()));
                    }
                    else if (grpType == "Job")
                    {
                        grpName = cmnCde.getJobName(int.Parse(dtst.Tables[0].Rows[a][5].ToString()));
                    }
                    else if (grpType == "Position")
                    {
                        grpName = cmnCde.getPosName(int.Parse(dtst.Tables[0].Rows[a][5].ToString()));
                    }
                    else if (grpType == "Site/Location")
                    {
                        grpName = cmnCde.getSiteName(int.Parse(dtst.Tables[0].Rows[a][5].ToString()));
                    }
                    else if (grpType == "Person Type")
                    {
                        grpName = cmnCde.getPssblValNm(int.Parse(dtst.Tables[0].Rows[a][5].ToString()));
                    }
                    else if (grpType == "Working Hour Type")
                    {
                        grpName = cmnCde.getWkhName(int.Parse(dtst.Tables[0].Rows[a][5].ToString()));
                    }
                    else if (grpType == "Gathering Type")
                    {
                        grpName = cmnCde.getGathName(int.Parse(dtst.Tables[0].Rows[a][5].ToString()));
                    }
                    else if (grpType == "Single Person")
                    {
                        grpName = cmnCde.getPrsnName(dtst.Tables[0].Rows[a][5].ToString());
                    }
                   ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 5]).Value2 = grpName;
                    ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 6]).Value2 =
                        cmnCde.getSegmentValDesc(int.Parse(dtst.Tables[0].Rows[a][7].ToString()));
                    ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 7]).Value2 = (dtst.Tables[0].Rows[a][27].ToString() == "1") ? "YES" : "NO";
                    ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 8]).Value2 = dtst.Tables[0].Rows[a][28].ToString();
                    if (this.sysClsfctn == "Currency")
                    {
                        ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 9]).Value2 = cmnCde.getPssblValNm(int.Parse(dtst.Tables[0].Rows[a][22].ToString()));
                    }
                    else if (this.sysClsfctn == "NaturalAccount")
                    {
                        string accntType = "";
                        if (dtst.Tables[0].Rows[a][14].ToString() == "A")
                        {
                            accntType = "A -ASSET";
                        }
                        else if (dtst.Tables[0].Rows[a][14].ToString() == "EQ")
                        {
                            accntType = "EQ-EQUITY";
                        }
                        else if (dtst.Tables[0].Rows[a][14].ToString() == "L")
                        {
                            accntType = "L -LIABILITY";
                        }
                        else if (dtst.Tables[0].Rows[a][14].ToString() == "R")
                        {
                            accntType = "R -REVENUE";
                        }
                        else if (dtst.Tables[0].Rows[a][14].ToString() == "EX")
                        {
                            accntType = "EX-EXPENSE";
                        }
                        //cmnCde.showSQLNoPermsn(dtst.Tables[0].Rows[a][28].ToString());
                        ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 9]).Value2 = cmnCde.getPssblValNm(int.Parse(dtst.Tables[0].Rows[a][22].ToString()));
                        ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 10]).Value2 = accntType;
                        ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 11]).Value2 = dtst.Tables[0].Rows[a][15].ToString() == "1" ? "YES" : "NO";
                        ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 12]).Value2 = dtst.Tables[0].Rows[a][16].ToString() == "1" ? "YES" : "NO";
                        ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 13]).Value2 = dtst.Tables[0].Rows[a][17].ToString() == "1" ? "YES" : "NO";
                        ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 14]).Value2 = dtst.Tables[0].Rows[a][13].ToString() == "1" ? "YES" : "NO";
                        ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 15]).Value2 = dtst.Tables[0].Rows[a][23].ToString() == "1" ? "YES" : "NO";
                        ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 16]).Value2 = dtst.Tables[0].Rows[a][20].ToString() == "1" ? "YES" : "NO";
                        ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 17]).Value2 = cmnCde.getSegmentValDesc(int.Parse(dtst.Tables[0].Rows[a][21].ToString()));
                        ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 18]).Value2 = dtst.Tables[0].Rows[a][24].ToString();
                        ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 19]).Value2 = cmnCde.getAccntNum(int.Parse(dtst.Tables[0].Rows[a][25].ToString()));
                    }
                }
            }
            else
            {
            }

            cmnCde.trgtSheets[0].get_Range("A1:A65535", Type.Missing).ColumnWidth = 10;
            cmnCde.trgtSheets[0].get_Range("A1:A65535", Type.Missing).WrapText = true;

            cmnCde.trgtSheets[0].get_Range("B1:Z65535", Type.Missing).Columns.AutoFit();
            cmnCde.trgtSheets[0].get_Range("B1:Z65535", Type.Missing).Rows.AutoFit();
        }

        private void importChartButton_Click(object sender, EventArgs e)
        {
            if (cmnCde.showMsg("Are you sure you want to Import Segment Values\r\n to Overwrite the existing Field Labels shown here?", 1) == DialogResult.No)
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
                this.imprtSgmntValTmp(this.openFileDialog1.FileName, this.segmentID);
            }
            this.loadPanel();
        }

        private void imprtSgmntValTmp(string filename, long sgmntID)
        {
            System.Windows.Forms.Application.DoEvents();
            cmnCde.clearPrvExclFiles();
            cmnCde.exclApp = new Microsoft.Office.Interop.Excel.Application();
            cmnCde.exclApp.WindowState = Excel.XlWindowState.xlNormal;
            cmnCde.exclApp.Visible = true;
            CommonCode.CommonCodes.SetWindowPos((IntPtr)cmnCde.exclApp.Hwnd, CommonCode.CommonCodes.HWND_TOP, 0, 0, 0, 0, CommonCode.CommonCodes.SWP_NOMOVE | CommonCode.CommonCodes.SWP_NOSIZE | CommonCode.CommonCodes.SWP_SHOWWINDOW);

            cmnCde.nwWrkBk = cmnCde.exclApp.Workbooks.Open(filename, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);

            cmnCde.trgtSheets = new Excel.Worksheet[1];

            cmnCde.trgtSheets[0] = (Excel.Worksheet)cmnCde.nwWrkBk.Worksheets[1];
            string segmentVal = "";
            string segmentDesc = "";
            string allwdGrpTyp = "";
            string allwdGrpNm = "";
            string prntSgmntVal = "";
            string accntCrncyCode = "";
            string accntType = "";
            string isParent = "";
            string isRetndErngs = "";
            string isNetIncome = "";
            string isContra = "";
            string isSuspense = "";
            string hsSubldgrs = "";
            string controlAcntVal = "";
            string accntClsfctn = "";
            string mpdAcntNum = "";
            string enblCmbtns = "";
            string dpndntSgmntVal = "";
            int rownum = 5;
            char[] w = { '\'' };
            do
            {
                try
                {
                    segmentVal = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 2]).Value2.ToString().Trim(w);
                }
                catch (Exception ex)
                {
                    segmentVal = "";
                }
                try
                {
                    segmentDesc = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 3]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    segmentDesc = "";
                }
                try
                {
                    allwdGrpTyp = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 4]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    allwdGrpTyp = "";
                }
                try
                {
                    allwdGrpNm = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 5]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    allwdGrpNm = "";
                }
                try
                {
                    prntSgmntVal = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 6]).Value2.ToString().Trim(w);
                }
                catch (Exception ex)
                {
                    prntSgmntVal = "";
                }

                try
                {
                    enblCmbtns = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 7]).Value2.ToString().Trim(w);
                }
                catch (Exception ex)
                {
                    enblCmbtns = "NO";
                }
                try
                {
                    dpndntSgmntVal = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 8]).Value2.ToString().Trim(w);
                }
                catch (Exception ex)
                {
                    dpndntSgmntVal = "";
                }
                try
                {
                    accntCrncyCode = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 9]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    accntCrncyCode = "";
                }
                try
                {
                    accntType = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 10]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    accntType = "";
                }
                try
                {
                    isParent = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 11]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    isParent = "NO";
                }
                try
                {
                    isRetndErngs = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 12]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    isRetndErngs = "NO";
                }
                try
                {
                    isNetIncome = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 13]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    isNetIncome = "NO";
                }
                try
                {
                    isContra = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 14]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    isContra = "NO";
                }
                try
                {
                    isSuspense = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 15]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    isSuspense = "NO";
                }
                try
                {
                    hsSubldgrs = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 16]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    hsSubldgrs = "NO";
                }
                try
                {
                    controlAcntVal = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 17]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    controlAcntVal = "";
                }
                try
                {
                    accntClsfctn = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 18]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    accntClsfctn = "";
                }
                try
                {
                    mpdAcntNum = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 19]).Value2.ToString().Trim(w);
                }
                catch (Exception ex)
                {
                    mpdAcntNum = "NO";
                }
                if (rownum == 5)
                {
                    string[] hdngs ={"Segment Value**","Value Description**","Allowed Group Type","Allowed Group Name",
                        "Parent Segment Value Description","Combinations Enabled?","Dependent Segment Value",
                "System Currency Code*", "Account Type**", "Is Parent?(YES/NO)","Is Retained Earnings?(YES/NO)","Is Net Income Account?(YES/NO)",
                "Is Contra Account?(YES/NO)", "Is Suspense Account?(YES/NO)", "Has SubLedgers?(YES/NO)", "Control Account Value",
                        "Account Classification","Mapped Group Account Number" };

                    if (this.sysClsfctn == "Currency")
                    {
                        if (segmentVal != hdngs[0].ToUpper()
                       || segmentDesc != hdngs[1].ToUpper()
                       || allwdGrpTyp != hdngs[2].ToUpper()
                       || allwdGrpNm != hdngs[3].ToUpper()
                       || prntSgmntVal != hdngs[4].ToUpper()
                      || enblCmbtns != hdngs[5].ToUpper()
                      || dpndntSgmntVal != hdngs[6].ToUpper()
                            || accntCrncyCode != hdngs[7].ToUpper())
                        {
                            cmnCde.showMsg("The Excel File you Selected is not a Valid Template\r\nfor importing records here.", 0);
                            return;
                        }
                    }
                    else if (this.sysClsfctn == "NaturalAccount")
                    {
                        if (segmentVal != hdngs[0].ToUpper()
                       || segmentDesc != hdngs[1].ToUpper()
                       || allwdGrpTyp != hdngs[2].ToUpper()
                       || allwdGrpNm != hdngs[3].ToUpper()
                       || prntSgmntVal != hdngs[4].ToUpper()
                      || enblCmbtns != hdngs[5].ToUpper()
                      || dpndntSgmntVal != hdngs[6].ToUpper()
                            || accntCrncyCode != hdngs[7].ToUpper()
                            || accntType != hdngs[8].ToUpper()
                     || isParent != hdngs[9].ToUpper()
                     || isRetndErngs != hdngs[10].ToUpper()
                     || isNetIncome != hdngs[11].ToUpper()
                     || isContra != hdngs[12].ToUpper()
                     || isSuspense != hdngs[13].ToUpper()
                     || hsSubldgrs != hdngs[14].ToUpper()
                     || controlAcntVal != hdngs[15].ToUpper()
                     || accntClsfctn != hdngs[16].ToUpper()
                     || mpdAcntNum != hdngs[17].ToUpper())
                        {
                            cmnCde.showMsg("The Excel File you Selected is not a Valid Template\r\nfor importing records here.", 0);
                            return;
                        }
                    }
                    else
                    {
                        if (segmentVal != hdngs[0].ToUpper()
                       || segmentDesc != hdngs[1].ToUpper()
                       || allwdGrpTyp != hdngs[2].ToUpper()
                       || allwdGrpNm != hdngs[3].ToUpper()
                       || prntSgmntVal != hdngs[4].ToUpper()
                      || enblCmbtns != hdngs[5].ToUpper()
                      || dpndntSgmntVal != hdngs[6].ToUpper())
                        {
                            cmnCde.showMsg("The Excel File you Selected is not a Valid Template\r\nfor importing records here.", 0);
                            return;
                        }
                    }
                    rownum++;
                    continue;
                }
                bool isParentBool = isParent == "YES" ? true : false;
                bool isRetndErngsBool = isRetndErngs == "YES" ? true : false;
                bool isNetIncomeBool = isNetIncome == "YES" ? true : false;
                bool isContraBool = isContra == "YES" ? true : false;
                bool isSuspenseBool = isSuspense == "YES" ? true : false;
                bool hsSubldgrsBool = hsSubldgrs == "YES" ? true : false;
                bool enblCmbtnsBool = enblCmbtns == "YES" ? true : false;
                int cntrlAccntIDTextBox = this.getSgmntValDescID(controlAcntVal, this.segmentID);
                int prntValIDTextBox = this.getSgmntValDescID(prntSgmntVal, this.segmentID);
                int dpndntSgmntValID = this.getSgmntValDescID(dpndntSgmntVal, this.dpndntSegmentID);
                int parentAccntIDTextBox = cmnCde.getAccntID(mpdAcntNum, cmnCde.getGrpOrgID());
                int accntCurrIDTextBox = cmnCde.getPssblValID(accntCrncyCode, cmnCde.getLovID("Currencies"));
                string grpNmIDTextBox = "-1";
                if (allwdGrpTyp == "Divisions/Groups")
                {
                    grpNmIDTextBox = cmnCde.getDivID(allwdGrpNm, this.orgID).ToString();
                }
                else if (allwdGrpTyp == "Grade")
                {
                    grpNmIDTextBox = cmnCde.getGrdID(allwdGrpNm, this.orgID).ToString();
                }
                else if (allwdGrpTyp == "Job")
                {
                    grpNmIDTextBox = cmnCde.getJobID(allwdGrpNm, this.orgID).ToString();
                }
                else if (allwdGrpTyp == "Position")
                {
                    grpNmIDTextBox = cmnCde.getPosID(allwdGrpNm, this.orgID).ToString();
                }
                else if (allwdGrpTyp == "Site/Location")
                {
                    grpNmIDTextBox = cmnCde.getSiteID(allwdGrpNm, this.orgID).ToString();
                }
                else if (allwdGrpTyp == "Person Type")
                {
                    grpNmIDTextBox = cmnCde.getPssblValID(allwdGrpNm, cmnCde.getLovID("Person Types")).ToString();
                }
                else if (allwdGrpTyp == "Single Person")
                {
                    grpNmIDTextBox = allwdGrpNm;
                }
                if (grpNmIDTextBox == "-1" || grpNmIDTextBox == "")
                {
                    allwdGrpTyp = "Everyone";
                }
                if ((segmentVal != "" || segmentDesc != "") && allwdGrpTyp != "")
                {
                    string errMsg = "";
                    if (this.validateSegmntVals(segmentVal,
                                                 segmentDesc,
                                                 allwdGrpTyp,
                                                 allwdGrpNm,
                                                 prntValIDTextBox,
                                                 accntCurrIDTextBox,
                                                 accntType,
                                                 isParentBool,
                                                 isRetndErngsBool,
                                                 isNetIncomeBool,
                                                 isContraBool,
                                                 isSuspenseBool,
                                                 hsSubldgrsBool,
                                                 cntrlAccntIDTextBox,
                                                 accntClsfctn,
                                                 parentAccntIDTextBox,
                                                 ref errMsg) == false)
                    {
                        //Do Nothing;
                        ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 18]).Value2 = errMsg;
                        cmnCde.trgtSheets[0].get_Range("A" + rownum + ":Q" + rownum + "", Type.Missing).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(255, 0, 0));
                    }
                    else
                    {
                        int oldSgmntValID = this.getSgmntValID(segmentVal, this.segmentID);
                        int oldSgmntNmID = this.getSgmntValDescID(segmentDesc, this.segmentID);
                        if (accntType != "")
                        {
                            accntType = accntType.Substring(0, 2).Trim();
                        }
                        if (oldSgmntValID <= 0)
                        {
                            this.createSgmntVal(this.orgID, this.segmentID, segmentVal, segmentDesc
                                , allwdGrpTyp, grpNmIDTextBox, true, prntValIDTextBox, isContraBool,
                                accntType, isParentBool, isRetndErngsBool, isNetIncomeBool,
                                this.getAcctTypID(accntType), 100, hsSubldgrsBool, cntrlAccntIDTextBox,
                                accntCurrIDTextBox, isSuspenseBool, accntClsfctn,
                                parentAccntIDTextBox, enblCmbtnsBool, dpndntSgmntValID);
                            cmnCde.trgtSheets[0].get_Range("A" + rownum + ":Q" + rownum + "", Type.Missing).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 255, 0));

                        }
                        else if (oldSgmntValID > 0)
                        {
                            this.updateSgmntVal(oldSgmntValID,
                            segmentVal, segmentDesc
                                , allwdGrpTyp, grpNmIDTextBox, true
                                , prntValIDTextBox, isContraBool,
                                accntType, isParentBool, isRetndErngsBool
                                , isNetIncomeBool, this.getAcctTypID(accntType),
                                100, hsSubldgrsBool, cntrlAccntIDTextBox,
                                accntCurrIDTextBox, isSuspenseBool, accntClsfctn,
                                parentAccntIDTextBox, this.segmentID, enblCmbtnsBool, dpndntSgmntValID);
                            cmnCde.trgtSheets[0].get_Range("A" + rownum + ":Q" + rownum + "", Type.Missing).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);

                        }
                    }
                }
                rownum++;
            }
            while (segmentVal != "");
            System.Windows.Forms.Application.DoEvents();
            this.loadPanel();
        }

        private bool validateSegmntVals(string segmentVal,
        string segmentDesc,
        string allwdGrpTyp,
        string allwdGrpNm,
        int prntValIDTextBox,
        int accntCurrIDTextBox,
        string accntType,
        bool isParentBool,
        bool isRetndErngsBool,
        bool isNetIncomeBool,
        bool isContraBool,
        bool isSuspenseBool,
        bool hsSubldgrsBool,
        int cntrlAccntIDTextBox,
        string accntClsfctn,
        int parentAccntIDTextBox,
        ref string errMsg)
        {
            if (allwdGrpTyp == "Single Person")
            {
                long prsnID = cmnCde.getPrsnID(allwdGrpNm);
                if (prsnID <= 0)
                {
                    errMsg = "The Person Local ID No. provided doesn't exist!";
                    return false;
                }
            }
            if (this.sysClsfctn == "NaturalAccount")
            {
                if (accntType == "")
                {
                    errMsg = "Please select an account Type!";
                    return false;
                }
                if (isRetndErngsBool == true && isParentBool == true)
                {
                    errMsg = "A Parent account cannot be used as Retained Earnings Account!";
                    return false;
                }
                if (isRetndErngsBool == true && isContraBool == true)
                {
                    errMsg = "A contra account cannot be used as Retained Earnings Account!";
                    return false;
                }

                if (isSuspenseBool == true && accntType != "A -ASSET")
                {
                    errMsg = "The account type of the Suspense Account must be ASSET";
                    return false;
                }

                if (isRetndErngsBool == true && accntType != "EQ-EQUITY")
                {
                    errMsg = "The account type of a Retained Earnings Account must be NET WORTH";
                    return false;
                }
                if (isNetIncomeBool == true && isParentBool == true)
                {
                    errMsg = "A Parent account cannot be used as Net Income Account!";
                    return false;
                }
                if (isNetIncomeBool == true && isContraBool == true)
                {
                    errMsg = "A contra account cannot be used as Net Income Account!";
                    return false;
                }
                if (isNetIncomeBool == true && accntType != "EQ-EQUITY")
                {
                    errMsg = "The account type of a Net Income Account must be NET WORTH";
                    return false;
                }
                if (isRetndErngsBool == true && isNetIncomeBool == true)
                {
                    errMsg = "Same Account cannot be Retained Earnings and Net Income at same time!";
                    return false;
                }
                if (isRetndErngsBool == true && hsSubldgrsBool)
                {
                    errMsg = "Retained Earnings account cannot have sub-ledgers!";
                    return false;
                }
                if (isNetIncomeBool == true && hsSubldgrsBool)
                {
                    errMsg = "Net Income account cannot have sub-ledgers!";
                    return false;
                }
                if (isContraBool == true && hsSubldgrsBool)
                {
                    errMsg = "The system does not support Sub-Ledgers on Contra-Accounts!";
                    return false;
                }
                if (isParentBool == true && hsSubldgrsBool)
                {
                    errMsg = "Parent Account cannot have sub-ledgers!";
                    return false;
                }
                if (cntrlAccntIDTextBox > 0 && hsSubldgrsBool)
                {
                    errMsg = "The system does not support Control Accounts reporting to other Control Account!";
                    return false;
                }
                if (cntrlAccntIDTextBox > 0 && prntValIDTextBox > 0)
                {
                    errMsg = "An Account with a Control Account cannot have a Parent Account as well!";
                    return false;
                }
                if (prntValIDTextBox > 0)
                {
                    if (cmnCde.getSgmntValAccntType(prntValIDTextBox) !=
                     accntType.Substring(0, 2).Trim())
                    {
                        errMsg = "Account Type does not match that of the Parent Account";
                        return false;
                    }
                }
                if (parentAccntIDTextBox > 0)
                {
                    if (cmnCde.getAccntType(parentAccntIDTextBox) !=
                     accntType.Substring(0, 2).Trim())
                    {
                        errMsg = "Account Type does not match that of the Mapped Group Account";
                        return false;
                    }
                }
            }
            else if (this.sysClsfctn == "Currency")
            {
                if (accntCurrIDTextBox <= 0)
                {
                    errMsg = "System Currency Cannot be Empty!";
                    return false;
                }
            }
            return true;
        }

        private void enblCmbtnsCheckBox_CheckedChanged(object sender, EventArgs e)
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
                this.enblCmbtnsCheckBox.Checked = !this.enblCmbtnsCheckBox.Checked;
            }
        }

        private void dpndntSgmntValButton_Click(object sender, EventArgs e)
        {
            if (cmnCde.test_prmssns(this.dfltPrvldgs[15]) == false)
            {
                cmnCde.showMsg("You don't have permission to perform" +
                 " this action!\nContact your System Administrator!", 0);
                return;
            }
            if (this.addRec == false && this.editRec == false)
            {
                this.editButton.PerformClick();
            }
            if (this.addRec == false && this.editRec == false)
            {
                cmnCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }

            string[] selVals = new string[1];
            selVals[0] = this.dpndntSgmntValIDTextBox.Text;
            DialogResult dgRes = cmnCde.showPssblValDiag(
                cmnCde.getLovID("Account Segment Values"), ref selVals, true, false, this.dpndntSegmentID,
             this.srchWrd, "Both", false);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.dpndntSgmntValIDTextBox.Text = this.getSgmntValID(selVals[i], this.dpndntSegmentID).ToString();
                    this.dpndntSgmntValTextBox.Text = cmnCde.getSegmentVal(int.Parse(this.dpndntSgmntValIDTextBox.Text)) + "." +
                        cmnCde.getSegmentValDesc(int.Parse(this.dpndntSgmntValIDTextBox.Text));
                }
            }
        }

        private void rprtClsfctnsButton_Click(object sender, EventArgs e)
        {
            if (this.editRec && this.editButton.Text == "EDIT")
            {
                this.editButton.PerformClick();
            }
            if (int.Parse(this.segmentValIDTextBox.Text) <= 0 && this.addRec == false)
            {
                cmnCde.showMsg("Please select an Account First!", 0);
                return;
            }
            long accntID = long.Parse(this.segmentValIDTextBox.Text);
            cmnCde.showSgmntClsfctnsDiag(ref accntID, this.editRec, this.cmnCde);
        }

        private void exprtRprtClsfctnsButton_Click(object sender, EventArgs e)
        {
            string rspnse = Interaction.InputBox("How many Segment Value Classifications will you like to Export?" +
              "\r\n1=No Segment Value(Empty Template)" +
              "\r\n2=All Segment Values" +
              "\r\n3-Infinity=Specify the exact number of Segment Value Classifications to Export\r\n",
              "Rhomicom", "1", (cmnCde.myComputer.Screen.Bounds.Width / 2) - 170,
              (cmnCde.myComputer.Screen.Bounds.Height / 2) - 100);
            if (rspnse.Equals(string.Empty) || rspnse.Equals(null))
            {
                //cmnCde.showMsg("Operation Cancelled!", 4);
                return;
            }
            int rsponse = 0;
            bool rsps = int.TryParse(rspnse, out rsponse);
            if (rsps == false)
            {
                cmnCde.showMsg("Invalid Option! Expecting a Number Above Zero", 4);
                return;
            }
            if (rsponse < 1)
            {
                cmnCde.showMsg("Invalid Option! Expecting a Number Above Zero", 4);
                return;
            }
            this.exprtSgmntValClsfctnsTmp(rsponse, this.segmentID);
        }
        private void exprtSgmntValClsfctnsTmp(int exprtTyp, long sgmntID)
        {
            System.Windows.Forms.Application.DoEvents();
            cmnCde.clearPrvExclFiles();
            cmnCde.exclApp = new Microsoft.Office.Interop.Excel.Application();
            cmnCde.exclApp.WindowState = Excel.XlWindowState.xlNormal;
            cmnCde.exclApp.Visible = true;
            CommonCode.CommonCodes.SetWindowPos((IntPtr)cmnCde.exclApp.Hwnd, CommonCode.CommonCodes.HWND_TOP, 0, 0, 0, 0, CommonCode.CommonCodes.SWP_NOMOVE | CommonCode.CommonCodes.SWP_NOSIZE | CommonCode.CommonCodes.SWP_SHOWWINDOW);

            cmnCde.nwWrkBk = cmnCde.exclApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            cmnCde.nwWrkBk.Worksheets.Add(Type.Missing, Type.Missing, 1, Type.Missing);
            cmnCde.trgtSheets = new Excel.Worksheet[1];

            cmnCde.trgtSheets[0] = (Excel.Worksheet)cmnCde.nwWrkBk.Worksheets[1];

            cmnCde.trgtSheets[0].get_Range("B2:C3", Type.Missing).MergeCells = true;
            cmnCde.trgtSheets[0].get_Range("B2:C3", Type.Missing).Value2 = cmnCde.getOrgName(cmnCde.Org_id).ToUpper();
            cmnCde.trgtSheets[0].get_Range("B2:C3", Type.Missing).Font.Bold = true;
            cmnCde.trgtSheets[0].get_Range("B2:C3", Type.Missing).Font.Size = 13;
            cmnCde.trgtSheets[0].get_Range("B2:C3", Type.Missing).WrapText = true;
            cmnCde.trgtSheets[0].Shapes.AddPicture(cmnCde.getOrgImgsDrctry() + @"\" + cmnCde.Org_id + ".png",
                Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, 1, 1, 50, 50);

            ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[5, 1]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 162, 192));
            ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[5, 1]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(255, 255, 255));
            ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[5, 1]).Font.Bold = true;
            ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[5, 1]).Value2 = "No.";
            string[] hdngs = { "Segment Value**", "Value Description**", "Main Report Classification", "Sub-Report Classification" };

            for (int a = 0; a < hdngs.Length; a++)
            {
                ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[5, (a + 2)]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 162, 192));
                ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[5, (a + 2)]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(255, 255, 255));
                ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[5, (a + 2)]).Font.Bold = true;
                ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[5, (a + 2)]).Value2 = hdngs[a].ToUpper();
            }

            if (exprtTyp >= 2)
            {
                DataSet dtst = new DataSet();
                if (exprtTyp == 2)
                {
                    dtst = this.get_One_RptClsfctns(10000000, 0);
                }
                else if (exprtTyp >= 3)
                {
                    dtst = this.get_One_RptClsfctns(exprtTyp, 0);
                }
                for (int a = 0; a < dtst.Tables[0].Rows.Count; a++)
                {
                    ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 1]).Value2 = a + 1;
                    ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 2]).Value2 = "'" + dtst.Tables[0].Rows[a][0].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 3]).Value2 = dtst.Tables[0].Rows[a][1].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 4]).Value2 = dtst.Tables[0].Rows[a][2].ToString();
                    ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[(a + 6), 5]).Value2 = dtst.Tables[0].Rows[a][3].ToString();
                }
            }
            else
            {
            }

            cmnCde.trgtSheets[0].get_Range("A1:A65535", Type.Missing).ColumnWidth = 10;
            cmnCde.trgtSheets[0].get_Range("A1:A65535", Type.Missing).WrapText = true;

            cmnCde.trgtSheets[0].get_Range("B1:Z65535", Type.Missing).Columns.AutoFit();
            cmnCde.trgtSheets[0].get_Range("B1:Z65535", Type.Missing).Rows.AutoFit();
        }

        private void imprtRprtClsfctnsButton_Click(object sender, EventArgs e)
        {
            if (cmnCde.showMsg("Are you sure you want to Import Segment Values\r\n to Overwrite the existing Field Labels shown here?", 1) == DialogResult.No)
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
                this.imprtSgmntValClsfctnsTmp(this.openFileDialog1.FileName, this.segmentID);
            }
        }

        private void imprtSgmntValClsfctnsTmp(string filename, long sgmntID)
        {
            System.Windows.Forms.Application.DoEvents();
            cmnCde.clearPrvExclFiles();
            cmnCde.exclApp = new Microsoft.Office.Interop.Excel.Application();
            cmnCde.exclApp.WindowState = Excel.XlWindowState.xlNormal;
            cmnCde.exclApp.Visible = true;
            CommonCode.CommonCodes.SetWindowPos((IntPtr)cmnCde.exclApp.Hwnd, CommonCode.CommonCodes.HWND_TOP, 0, 0, 0, 0, CommonCode.CommonCodes.SWP_NOMOVE | CommonCode.CommonCodes.SWP_NOSIZE | CommonCode.CommonCodes.SWP_SHOWWINDOW);

            cmnCde.nwWrkBk = cmnCde.exclApp.Workbooks.Open(filename, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);

            cmnCde.trgtSheets = new Excel.Worksheet[1];

            cmnCde.trgtSheets[0] = (Excel.Worksheet)cmnCde.nwWrkBk.Worksheets[1];
            string segmentVal = "";
            string segmentDesc = "";
            string majCtgry = "";
            string MinCtgry = "";
            int rownum = 5;
            char[] w = { '\'' };
            do
            {
                try
                {
                    segmentVal = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 2]).Value2.ToString().Trim(w);
                }
                catch (Exception ex)
                {
                    segmentVal = "";
                }
                try
                {
                    segmentDesc = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 3]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    segmentDesc = "";
                }
                try
                {
                    majCtgry = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 4]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    majCtgry = "";
                }
                try
                {
                    MinCtgry = ((Microsoft.Office.Interop.Excel.Range)cmnCde.trgtSheets[0].Cells[rownum, 5]).Value2.ToString();
                }
                catch (Exception ex)
                {
                    MinCtgry = "";
                }
                if (rownum == 5)
                {
                    string[] hdngs = { "Segment Value**", "Value Description**", "Main Report Classification", "Sub-Report Classification" };

                    if (segmentVal != hdngs[0].ToUpper()
                      || segmentDesc != hdngs[1].ToUpper()
                      || majCtgry != hdngs[2].ToUpper()
                      || MinCtgry != hdngs[3].ToUpper())
                    {
                        cmnCde.showMsg("The Excel File you Selected is not a Valid Template\r\nfor importing records here.", 0);
                        return;
                    }
                    rownum++;
                    continue;
                }
                if (segmentVal != "" && majCtgry != "")
                {
                    string errMsg = "";
                    int oldSgmntValID = this.getSgmntValID(segmentVal, this.segmentID);
                    long oldClsfctnID = this.get_RptClsfctnID(majCtgry, MinCtgry, oldSgmntValID);
                    if (oldClsfctnID <= 0 && oldSgmntValID > 0)
                    {
                        oldClsfctnID = this.getNewRptClsfLnID();
                        this.createRptClsfctn(oldClsfctnID, majCtgry, MinCtgry, oldSgmntValID);
                        cmnCde.trgtSheets[0].get_Range("A" + rownum + ":Q" + rownum + "", Type.Missing).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(0, 255, 0));
                    }
                    else if (oldSgmntValID > 0)
                    {
                        this.updateRptClsfctn(oldClsfctnID, majCtgry, MinCtgry, oldSgmntValID);
                        cmnCde.trgtSheets[0].get_Range("A" + rownum + ":Q" + rownum + "", Type.Missing).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                    }
                    else
                    {
                        cmnCde.trgtSheets[0].get_Range("A" + rownum + ":Q" + rownum + "", Type.Missing).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightPink);
                    }
                }
                else
                {
                    cmnCde.trgtSheets[0].get_Range("A" + rownum + ":Q" + rownum + "", Type.Missing).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                }
                rownum++;
            }
            while (segmentVal != "");
            System.Windows.Forms.Application.DoEvents();
        }

        public DataSet get_Basic_SgmntVals(string searchWord, string searchIn, Int64 offset, int limit_size, int segmentID)
        {
            string strSql = "";
            string whrcls = "";
            if (searchIn == "Dependent Value")
            {
                whrcls = " AND ((COALESCE(org.get_sgmnt_val(dpndnt_sgmnt_val_id),'')||'.'||COALESCE(org.get_sgmnt_val_desc(dpndnt_sgmnt_val_id),'')) ilike '" + searchWord.Replace("'", "''") +
                               "')";
            }
            else if (searchIn == "Value/Description")
            {
                whrcls = " AND (segment_value ilike '" + searchWord.Replace("'", "''") +
               "' or segment_description ilike '" + searchWord.Replace("'", "''") +
               "')";
            }
            else
            {
                whrcls = " AND (segment_value ilike '" + searchWord.Replace("'", "''") +
               "' or segment_description ilike '" + searchWord.Replace("'", "''") +
               "' or (COALESCE(org.get_sgmnt_val(dpndnt_sgmnt_val_id),'')||'.'||COALESCE(org.get_sgmnt_val_desc(dpndnt_sgmnt_val_id),'')) ilike '" + searchWord.Replace("'", "''") +
                               "')";
            }
            string subSql = @"SELECT segment_value_id,segment_value,segment_description,space||segment_value||'.'||segment_description account_number_name, is_prnt_accnt, accnt_type,accnt_typ_id, prnt_segment_value_id, control_account_id,dpndnt_sgmnt_val_id, depth, path, cycle 
      FROM suborg WHERE 1=1 " + whrcls + @" ORDER BY accnt_typ_id, path";

            strSql = @"WITH RECURSIVE suborg(segment_value_id, segment_value, segment_description, is_prnt_accnt, accnt_type, accnt_typ_id, prnt_segment_value_id, control_account_id,dpndnt_sgmnt_val_id, depth, path, cycle, space) AS 
      ( 
      SELECT a.segment_value_id, a.segment_value, a.segment_description, a.is_prnt_accnt, a.accnt_type,a.accnt_typ_id, a.prnt_segment_value_id, a.control_account_id,a.dpndnt_sgmnt_val_id, 1, ARRAY[a.segment_value||'']::character varying[], false, '' opad 
      FROM org.org_segment_values a 
        WHERE ((CASE WHEN a.prnt_segment_value_id<=0 THEN a.control_account_id ELSE a.prnt_segment_value_id END)=-1 AND (a.segment_id = " + segmentID + @")) 
      UNION ALL        
      SELECT a.segment_value_id, a.segment_value, a.segment_description, a.is_prnt_accnt, a.accnt_type,a.accnt_typ_id, a.prnt_segment_value_id, a.control_account_id,a.dpndnt_sgmnt_val_id, sd.depth + 1, 
      path || a.segment_value, 
      a.segment_value = ANY(path), space || '      '
      FROM org.org_segment_values a, suborg AS sd 
      WHERE (((CASE WHEN a.prnt_segment_value_id<=0 THEN a.control_account_id ELSE a.prnt_segment_value_id END)=sd.segment_value_id AND NOT cycle) 
       AND (a.segment_id = " + segmentID + @"))) 
       " + subSql + " LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            this.segmentValsSQL = strSql;
            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            return dtst;
        }

        public long get_Total_SgmntVals(string searchWord, string searchIn, int segmentID)
        {
            string strSql = "";
            string whrcls = "";
            if (searchIn == "Dependent Value")
            {
                whrcls = " AND ((COALESCE(org.get_sgmnt_val(dpndnt_sgmnt_val_id),'')||'.'||COALESCE(org.get_sgmnt_val_desc(dpndnt_sgmnt_val_id),'')) ilike '" + searchWord.Replace("'", "''") +
                               "')";
            }
            else if (searchIn == "Value/Description")
            {
                whrcls = " AND (segment_value ilike '" + searchWord.Replace("'", "''") +
               "' or segment_description ilike '" + searchWord.Replace("'", "''") +
               "')";
            }
            else
            {
                whrcls = " AND (segment_value ilike '" + searchWord.Replace("'", "''") +
               "' or segment_description ilike '" + searchWord.Replace("'", "''") +
               "' or (COALESCE(org.get_sgmnt_val(dpndnt_sgmnt_val_id),'')||'.'||COALESCE(org.get_sgmnt_val_desc(dpndnt_sgmnt_val_id),'')) ilike '" + searchWord.Replace("'", "''") +
                               "')";
            }
            string subSql = @"SELECT count(segment_value_id) 
      FROM suborg WHERE 1=1" + whrcls + @"";

            strSql = @"WITH RECURSIVE suborg(segment_value_id, segment_value, segment_description, is_prnt_accnt, accnt_type, accnt_typ_id, prnt_segment_value_id, control_account_id, dpndnt_sgmnt_val_id, depth, path, cycle, space) AS 
      ( 
      SELECT a.segment_value_id, a.segment_value, a.segment_description, a.is_prnt_accnt, a.accnt_type,a.accnt_typ_id, a.prnt_segment_value_id, a.control_account_id,a.dpndnt_sgmnt_val_id, 1, ARRAY[a.segment_value||'']::character varying[], false, '' opad 
      FROM org.org_segment_values a 
        WHERE ((CASE WHEN a.prnt_segment_value_id<=0 THEN a.control_account_id ELSE a.prnt_segment_value_id END)=-1 AND (a.segment_id = " + segmentID + @")) 
      UNION ALL        
      SELECT a.segment_value_id, a.segment_value, a.segment_description, a.is_prnt_accnt, a.accnt_type,a.accnt_typ_id, a.prnt_segment_value_id, a.control_account_id,a.dpndnt_sgmnt_val_id, sd.depth + 1, 
      path || a.segment_value, 
      a.segment_value = ANY(path), space || '      '
      FROM org.org_segment_values a, suborg AS sd 
      WHERE (((CASE WHEN a.prnt_segment_value_id<=0 THEN a.control_account_id ELSE a.prnt_segment_value_id END)=sd.segment_value_id AND NOT cycle) 
       AND (a.segment_id = " + segmentID + @"))) 
       " + subSql;

            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public double get_SgmntValAcntBals(int segmentValID, int segmentNum)
        {
            string strSql = "SELECT sum(net_balance) " +
      "FROM accb.accb_chart_of_accnts a " +
      "WHERE (a.accnt_seg" + segmentNum + "_val_id = " + segmentValID + ")";
            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public int getAcctTypID(string accntTyp)
        {
            if (accntTyp == "A")
            {
                return 1;
            }
            else if (accntTyp == "L")
            {
                return 2;
            }
            else if (accntTyp == "EQ")
            {
                return 3;
            }
            else if (accntTyp == "R")
            {
                return 4;
            }
            else if (accntTyp == "EX")
            {
                return 5;
            }
            return -1;
        }


        public void clearChrtRetEarns(int segmentID)
        {
            cmnCde.Extra_Adt_Trl_Info = "";
            string updtSQL = "UPDATE org.org_segment_values " +
            "SET is_retained_earnings='0' WHERE segment_id = " + segmentID;
            cmnCde.updateDataNoParams(updtSQL);
        }

        public void clearChrtNetIncome(int segmntID)
        {
            cmnCde.Extra_Adt_Trl_Info = "";
            string updtSQL = "UPDATE org.org_segment_values " +
            "SET is_net_income='0' WHERE segment_id = " + segmntID;
            cmnCde.updateDataNoParams(updtSQL);
        }

        public void clearChrtSuspns(int segmntID)
        {
            cmnCde.Extra_Adt_Trl_Info = "";
            string updtSQL = "UPDATE org.org_segment_values " +
            "SET is_suspens_accnt='0' WHERE segment_id = " + segmntID;
            cmnCde.updateDataNoParams(updtSQL);
        }

        public void createSgmntVal(int orgid, int segmentID, string segmentVal, string segmentDesc,
    string allwdGrpTyp, string allwdGrpVal, bool isEnbld, int prntSegmentID,
    bool isContra, string accntType, bool isParent, bool isRetainedErngs,
    bool isNetIncome, int accntTypID, int reportLineNo, bool hsSubLdgrs,
    int contrlAcntID, int crncyID, bool isSuspenseAcnt, string acntClsfctn,
    int mappedAcntID, bool enblCmbtnsCheckBox, int dpndntValId)
        {
            if (isRetainedErngs == true)
            {
                this.clearChrtRetEarns(segmentID);
            }
            if (isNetIncome == true)
            {
                this.clearChrtNetIncome(segmentID);
            }
            if (isSuspenseAcnt == true)
            {
                this.clearChrtSuspns(segmentID);
            }
            string dateStr = cmnCde.getDB_Date_time();
            string insSQL = @"INSERT INTO org.org_segment_values(
            segment_id, segment_value, segment_description, 
            allwd_group_type, allwd_group_value, is_enabled, prnt_segment_value_id, 
            created_by, creation_date, last_update_by, last_update_date, 
            org_id, is_contra, accnt_type, is_prnt_accnt, is_retained_earnings, 
            is_net_income, accnt_typ_id, report_line_no, has_sub_ledgers, 
            control_account_id, crncy_id, is_suspens_accnt, account_clsfctn, 
            mapped_grp_accnt_id, enable_cmbntns, dpndnt_sgmnt_val_id) " +
                  "VALUES (" + segmentID +
                  ",'" + segmentVal.Replace("'", "''") +
                  "', '" + segmentDesc.Replace("'", "''") +
                  "', '" + allwdGrpTyp.Replace("'", "''") +
                  "', '" + allwdGrpVal.Replace("'", "''") +
                  "', '" + cmnCde.cnvrtBoolToBitStr(isEnbld) +
                  "', " + prntSegmentID +
                  ", " + this.cmnCde.User_id +
                  ", '" + dateStr +
                  "', " + this.cmnCde.User_id +
                  ", '" + dateStr +
                  "', " + orgid +
                  ", '" + cmnCde.cnvrtBoolToBitStr(isContra) +
                  "', '" + accntType.Replace("'", "''") +
                  "', '" + cmnCde.cnvrtBoolToBitStr(isParent) +
                  "', '" + cmnCde.cnvrtBoolToBitStr(isRetainedErngs) +
                  "', '" + cmnCde.cnvrtBoolToBitStr(isNetIncome) +
                  "', " + accntTypID +
                  ", " + reportLineNo +
                  ", '" + cmnCde.cnvrtBoolToBitStr(hsSubLdgrs) +
                  "', " + contrlAcntID +
                  ", " + crncyID +
                  ", '" + cmnCde.cnvrtBoolToBitStr(isSuspenseAcnt) +
                  "', '" + acntClsfctn.Replace("'", "''") +
                  "', " + mappedAcntID +
                  ", '" + cmnCde.cnvrtBoolToBitStr(enblCmbtnsCheckBox) +
                  "', " + dpndntValId + ")";
            cmnCde.insertDataNoParams(insSQL);
        }

        public void updateSgmntVal(int segmentValID, string segmentVal, string segmentDesc,
    string allwdGrpTyp, string allwdGrpVal, bool isEnbld, int prntSegmentID,
    bool isContra, string accntType, bool isParent, bool isRetainedErngs,
    bool isNetIncome, int accntTypID, int reportLineNo, bool hsSubLdgrs,
    int contrlAcntID, int crncyID, bool isSuspenseAcnt, string acntClsfctn, int mappedAcntID, int segmnetID, bool enblCmbtnsCheckBox, int dpndntValId)
        {
            if (isRetainedErngs == true)
            {
                this.clearChrtRetEarns(segmnetID);
            }
            if (isNetIncome == true)
            {
                this.clearChrtNetIncome(segmnetID);
            }
            if (isSuspenseAcnt == true)
            {
                this.clearChrtSuspns(segmnetID);
            }
            cmnCde.Extra_Adt_Trl_Info = "";
            string dateStr = cmnCde.getDB_Date_time();
            string updtSQL = @"UPDATE org.org_segment_values
       SET segment_value ='" + segmentVal.Replace("'", "''") +
       "', segment_description ='" + segmentDesc.Replace("'", "''") +
       "', allwd_group_type ='" + allwdGrpTyp.Replace("'", "''") +
       "', allwd_group_value ='" + allwdGrpVal.Replace("'", "''") +
       "', is_enabled ='" + cmnCde.cnvrtBoolToBitStr(isEnbld) +
       "', prnt_segment_value_id =" + prntSegmentID +
       ", created_by =" + this.cmnCde.User_id +
       ", creation_date ='" + dateStr +
       "', last_update_by =" + this.cmnCde.User_id +
       ", last_update_date ='" + dateStr +
       "', is_contra ='" + cmnCde.cnvrtBoolToBitStr(isContra) +
       "', accnt_type ='" + accntType.Replace("'", "''") +
       "', is_prnt_accnt ='" + cmnCde.cnvrtBoolToBitStr(isParent) +
       "', is_retained_earnings ='" + cmnCde.cnvrtBoolToBitStr(isRetainedErngs) +
       "', is_net_income ='" + cmnCde.cnvrtBoolToBitStr(isNetIncome) +
       "', accnt_typ_id =" + accntTypID +
       ", report_line_no =" + reportLineNo +
       ", has_sub_ledgers ='" + cmnCde.cnvrtBoolToBitStr(hsSubLdgrs) +
       "', control_account_id =" + contrlAcntID +
       ", crncy_id =" + crncyID +
       ", is_suspens_accnt ='" + cmnCde.cnvrtBoolToBitStr(isSuspenseAcnt) +
       "', account_clsfctn ='" + acntClsfctn.Replace("'", "''") +
       "', mapped_grp_accnt_id =" + mappedAcntID +
       ", enable_cmbntns = '" + cmnCde.cnvrtBoolToBitStr(enblCmbtnsCheckBox) +
       "', dpndnt_sgmnt_val_id=" + dpndntValId + " WHERE (segment_value_id =" + segmentValID + ")";
            cmnCde.updateDataNoParams(updtSQL);
        }

        public void deleteSgmntVal(long segmentValID, string segmentValDesc)
        {
            cmnCde.Extra_Adt_Trl_Info = "Segment Value = " + segmentValDesc;
            string delSQL = "DELETE FROM org.org_segment_values WHERE segment_value_id = " + segmentValID;
            cmnCde.deleteDataNoParams(delSQL);
        }

        public void updateSegmentVal(int segmentValID, int segmentNum, int accntID)
        {
            cmnCde.Extra_Adt_Trl_Info = "";
            string dateStr = cmnCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_chart_of_accnts SET accnt_seg" + segmentNum + "_val_id = " + segmentValID +
                ", last_update_by =" + this.cmnCde.User_id +
                   ", last_update_date ='" + dateStr +
                   "' WHERE accnt_id = " + accntID;
            cmnCde.updateDataNoParams(updtSQL);
        }

        public bool isSgmntValInUse(int segmentValID, int segmentNum)
        {
            string strSql = "SELECT a.accnt_id " +
             "FROM accb.accb_chart_of_accnts a " +
             "WHERE(a.accnt_seg" + segmentNum + "_val_id = " + segmentValID + ")";
            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            strSql = "SELECT a.segment_value_id " +
             "FROM org.org_segment_values a " +
             "WHERE(a.prnt_segment_value_id= " + segmentValID + " or a.control_account_id= " + segmentValID + ")";
            dtst = cmnCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
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

        public int getSgmntValDescID(string segmentVal, int segmentID)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "select segment_value_id from org.org_segment_values where lower(segment_description) = '" +
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

        public DataSet get_One_SgmntVals(string searchWord, string searchIn,
     Int64 offset, int limit_size, int segmentID)
        {
            string strSql = @"SELECT a.segment_value_id, a.segment_id, a.segment_value, a.segment_description, 
       a.allwd_group_type, a.allwd_group_value, a.is_enabled, a.prnt_segment_value_id, 
       a.created_by, a.creation_date, a.last_update_by, a.last_update_date, 
       a.org_id, a.is_contra, a.accnt_type, a.is_prnt_accnt, a.is_retained_earnings, 
       a.is_net_income, a.accnt_typ_id, a.report_line_no, a.has_sub_ledgers, 
       a.control_account_id, a.crncy_id, a.is_suspens_accnt, a.account_clsfctn, 
       a.mapped_grp_accnt_id, b.segment_number, a.enable_cmbntns, 
       org.get_sgmnt_val_desc(a.dpndnt_sgmnt_val_id), a.dpndnt_sgmnt_val_id
  FROM org.org_segment_values a, org.org_acnt_sgmnts b " +
             "WHERE(a.segment_id = b.segment_id and b.segment_id = " + segmentID + ") ORDER BY a.segment_value LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            //this.taxFrm.rec_SQL = strSql;
            return dtst;
        }

        public DataSet get_One_SgmntValDet(int segmentValID)
        {
            string strSql = @"SELECT a.segment_value_id, a.segment_id, a.segment_value, a.segment_description, 
       a.allwd_group_type, a.allwd_group_value, a.is_enabled, a.prnt_segment_value_id, 
       a.created_by, a.creation_date, a.last_update_by, a.last_update_date, 
       a.org_id, a.is_contra, a.accnt_type, a.is_prnt_accnt, a.is_retained_earnings, 
       a.is_net_income, a.accnt_typ_id, a.report_line_no, a.has_sub_ledgers, 
       a.control_account_id, a.crncy_id, a.is_suspens_accnt, a.account_clsfctn, 
       a.mapped_grp_accnt_id, b.segment_number, a.enable_cmbntns, 
       org.get_sgmnt_id(b.prnt_sgmnt_number), a.dpndnt_sgmnt_val_id  
  FROM org.org_segment_values a, org.org_acnt_sgmnts b " +
             "WHERE(a.segment_id = b.segment_id and a.segment_value_id = " + segmentValID + ")";
            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            //this.taxFrm.rec_SQL = strSql;
            return dtst;
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
        public DataSet get_One_RptClsfctns(int limit_size, int offset)
        {
            string strSql = @"SELECT b.segment_value, b.segment_description, a.maj_rpt_ctgry, a.min_rpt_ctgry 
  FROM org.org_account_clsfctns a, org.org_segment_values b WHERE(a.account_id= b.segment_value_id)  
ORDER BY b.segment_value, a.maj_rpt_ctgry, a.min_rpt_ctgry LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            return dtst;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (this.saveButton.Enabled == true)
            {
                this.saveButton.PerformClick();
            }
            if (this.segmentValsListView.Items.Count > 0 && this.segmentValsListView.CheckedItems.Count <= 0
                && this.segmentValsListView.SelectedItems.Count > 0)
            {
                this.segmentValsListView.SelectedItems[0].Checked = true;
            }
            if (this.segmentValsListView.CheckedItems.Count > 0)
            {
                //this.idTextBox.Text = this.cstSplrListView.CheckedItems[0].SubItems[2].Text;
            }
            else
            {
                this.segmentValIDTextBox.Text = "-1";
            }
            if (int.Parse(this.segmentValIDTextBox.Text) <= 0 && this.mustSelctSth == true)
            {
                cmnCde.showMsg("Must Select A Value First!", 0);
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void segmentValsListView_DoubleClick(object sender, EventArgs e)
        {
            this.segmentValsListView.SelectedItems[0].Checked = true;
            if (this.isReadOnly)
            {
                return;
            }
            this.okButton_Click(this.okButton, e);
        }

        private void segmentValsListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (this.shdObeyEvts() == false)
            {
                return;
            }
            if (e != null)
            {
                this.selItemTxt = "";
                if (e.Item.Checked == true)
                {
                    this.selItemTxt = e.Item.Text;
                    e.Item.Selected = true;
                }
            }
            this.uncheckAllBtOne();
        }

        private void uncheckAllBtOne()
        {
            this.obey_evnts = false;
            for (int i = 0; i < this.segmentValsListView.Items.Count; i++)
            {
                if (this.segmentValsListView.Items[i].Text != this.selItemTxt)
                {
                    this.segmentValsListView.Items[i].Checked = false;
                }
            }
            this.obey_evnts = true;

        }

        private void segmentValsListView_KeyDown(object sender, KeyEventArgs e)
        {
            EventArgs ex = new EventArgs();
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                //this.vwAccntTrnsctnsButton_Click(this.vwAccntTrnsctnsButton, ex);
            }
            else if (e.Control && e.KeyCode == Keys.S)
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
            else if ((e.Control && e.KeyCode == Keys.F) || e.KeyCode == Keys.F5)
            {
                if (this.goButton.Enabled == true)
                {
                    this.goButton_Click(this.goButton, ex);
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.R)
            {
                this.resetButton.PerformClick();
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
                cmnCde.listViewKeyDown(this.segmentValsListView, e);
            }
        }
    }
}

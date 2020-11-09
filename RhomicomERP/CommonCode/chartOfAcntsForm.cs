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
    public partial class chartOfAcntsForm : Form
    {
        public chartOfAcntsForm()
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
        bool errorOcrd = false;
        string srchWrd = "%";
        private string selItemTxt = "";
        public string lovName = "";
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
        public string[] dfltPrvldgs = { "View Accounting","View Chart of Accounts", 
    /*2*/"View Account Transactions", "View Transactions Search",
    /*4*/"View/Generate Trial Balance", "View/Generate Profit & Loss Statement", 
    /*6*/"View/Generate Balance Sheet","View Budgets",
		/*8*/"View Transaction Templates", "View Record History", "View SQL",
    /*11*/"Add Chart of Accounts", "Edit Chart of Accounts", "Delete Chart of Accounts",
    /*14*/"Add Batch for Transactions","Edit Batch for Transactions","Void/Delete Batch for Transactions",
    /*17*/"Add Transactions Directly", "Edit Transactions","Delete Transactions",
    /*20*/"Add Transactions Using Template","Post Transactions",
    /*22*/"Add Budgets","Edit Budgets","Delete Budgets",
    /*25*/"Add Transaction Templates","Edit Transaction Templates","Delete Transaction Templates",
    /*28*/"View Only Self-Created Transaction Batches",
    /*29*/"View Financial Statements","View Accounting Periods","View Payables",
    /*32*/"View Receivables","View Customers/Suppliers","View Tax Codes",
    /*35*/"View Default Accounts","View Account Reconciliation",
    /*37*/"Add Accounting Periods","Edit Accounting Periods", "Delete Accounting Periods",
    /*40*/"View Fixed Assets","View Payments",
    /*42*/"Add Payment Methods", "Edit Payment Methods","Delete Payment Methods",
    /*45*/"Add Supplier Standard Payments", "Edit Supplier Standard Payments","Delete Supplier Standard Payments",
    /*48*/"Add Supplier Advance Payments", "Edit Supplier Advance Payments","Delete Supplier Advance Payments", 
    /*51*/"Setup Exchange Rates", "Setup Document Templates","Review/Approve Payables Documents","Review/Approve Receivables Documents",
    /*55*/"Add Direct Refund from Supplier", "Edit Direct Refund from Supplier","Delete Direct Refund from Supplier",
    /*58*/"Add Supplier Credit Memo (InDirect Refund)", "Edit Supplier Credit Memo (InDirect Refund)","Delete Supplier Credit Memo (InDirect Refund)",
    /*61*/"Add Direct Topup for Supplier", "Edit Direct Topup for Supplier","Delete Direct Topup for Supplier",
    /*64*/"Add Supplier Debit Memo (InDirect Topup)", "Edit Supplier Debit Memo (InDirect Topup)", "Delete Supplier Debit Memo (InDirect Topup)",
    /*67*/"Cancel Payables Documents", "Cancel Receivables Documents",
    /*69*/"Reject Payables Documents", "Reject Receivables Documents",
    /*71*/"Pay Payables Documents", "Pay Receivables Documents",
    /*73*/"Add Customer Standard Payments", "Edit Customer Standard Payments","Delete Customer Standard Payments",
    /*76*/"Add Customer Advance Payments", "Edit Customer Advance Payments","Delete Customer Advance Payments", 
    /*79*/"Add Direct Refund to Customer", "Edit Direct Refund to Customer","Delete Direct Refund to Customer",
    /*82*/"Add Customer Credit Memo (InDirect Topup)", "Edit Customer Credit Memo (InDirect Topup)","Delete Customer Credit Memo (InDirect Topup)",
    /*85*/"Add Direct Topup from Customer", "Edit Direct Topup from Customer","Delete Direct Topup from Customer",
    /*88*/"Add Customer Debit Memo (InDirect Refund)", "Edit Customer Debit Memo (InDirect Refund)", "Delete Customer Debit Memo (InDirect Refund)",
    /*91*/"Add Customers/Suppliers", "Edit Customers/Suppliers", "Delete Customers/Suppliers"
    };

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
        //Chart of Accounts Panel Variables;
        Int64 chrt_cur_indx = 0;
        bool is_last_chrt = false;
        Int64 totl_chrt = 0;
        long last_chrt_num = 0;
        public string chrt_SQL = "";
        public string chrtDet_SQL = "";
        public string rates_SQL = "";
        bool obey_chrt_evnts = false;
        bool addChrt = false;
        bool editChrt = false;
        bool beenToIsprntfunc = false;
        bool beenToIsContra = false;
        bool beenToIsRetEarns = false;
        bool beenToIsNetInc = false;
        bool beenToIsEnabled = false;
        bool beenClicked = false;
        bool addAccounts = false;
        bool editAccounts = false;
        bool delAccounts = false;
        public int funCurID = -1;
        public string funcCurCode = "";
        public int trnsAcntLovID = -1;
        #endregion
        private void chartOfAcntsForm_Load(object sender, EventArgs e)
        {
            cmnCde.Prsn_id = cmnCde.getUserPrsnID(cmnCde.User_id);
            Color[] clrs = cmnCde.getColors();
            this.BackColor = clrs[0];
            cmnCde.DefaultPrvldgs = this.dfltPrvldgs;
            if (this.lovName == "")
            {
                this.lovName = "Transaction Accounts";
            }
            this.trnsAcntLovID = cmnCde.getLovID(this.lovName);
            this.disableFormButtons();
            this.loadAccntChrtPanel();
            //this.disableChrtEdit();
            System.Windows.Forms.Application.DoEvents();
            this.searchForChrtTextBox.Select();
            System.Windows.Forms.Application.DoEvents();
            this.searchForChrtTextBox.Focus();
            this.searchForChrtTextBox.SelectAll();
            if (this.accntsChrtListView.Items.Count == 1 && this.accntsChrtListView.CheckedItems.Count <= 0 && this.autoLoad)
            {
                this.accntsChrtListView.Items[0].Checked = true;
                this.okButton.PerformClick();
            }
        }

        public void disableFormButtons()
        {
            bool vwSQL = cmnCde.test_prmssns(this.dfltPrvldgs[10]);
            bool rcHstry = cmnCde.test_prmssns(this.dfltPrvldgs[9]);
            this.addRecsP = cmnCde.test_prmssns(this.dfltPrvldgs[11]);
            this.editRecsP = cmnCde.test_prmssns(this.dfltPrvldgs[12]);
            this.delRecsP = cmnCde.test_prmssns(this.dfltPrvldgs[13]);
            this.saveChrtButton.Enabled = false;
            if (this.isReadOnly == false)
            {
                this.addChrtButton.Enabled = this.addRecsP;
                this.editChrtButton.Enabled = this.editRecsP;
                this.deleteChrtButton.Enabled = this.delRecsP;
                this.addAccounts = this.addRecsP;
                this.editAccounts = this.editRecsP;
                this.delAccounts = this.delRecsP;
                //this.addChrt = this.addRecsP;
                //this.editChrt = this.editRecsP;
            }
            else
            {
                this.addChrtButton.Enabled = false;
                this.editChrtButton.Enabled = false;
                this.deleteChrtButton.Enabled = false;
                this.okButton.Enabled = false;
                this.addAccounts = false;
                this.editAccounts = false;
                this.delAccounts = false;
                this.addChrt = false;
                this.editChrt = false;
            }
            if (this.editRecsP)
            {
                this.groupBox1.Visible = true;
            }
            else
            {
                this.groupBox1.Visible = false;
            }
        }
        #region "CHART OF ACCOUNTS..."
        private void loadAccntChrtPanel()
        {
            System.Windows.Forms.Application.DoEvents();

            this.obey_chrt_evnts = false;
            if (this.searchInChrtComboBox.SelectedIndex < 0)
            {
                this.searchInChrtComboBox.SelectedIndex = 0;
            }
            if (searchForChrtTextBox.Text.Contains("%") == false)
            {
                this.searchForChrtTextBox.Text = "%" + this.searchForChrtTextBox.Text.Replace(" ", "%") + "%";
            }
            if (this.searchForChrtTextBox.Text == "%%")
            {
                this.searchForChrtTextBox.Text = "%";
            }
            int dsply = 0;
            if (this.dsplySizeChrtComboBox.Text == ""
             || int.TryParse(this.dsplySizeChrtComboBox.Text, out dsply) == false)
            {
                this.dsplySizeChrtComboBox.Text = cmnCde.get_CurPlcy_Mx_Dsply_Recs().ToString();
            }
            this.funcCurrLabel.Text = "FUNCTIONAL CURRENCY BALANCE (" + funcCurCode + ")";
            this.accntCurrLabel.Text = "ACCOUNT CURRENCY BALANCE (" + funcCurCode + ")";
            this.is_last_chrt = false;
            this.chrt_cur_indx = 0;
            this.totl_chrt = cmnCde.Big_Val;
            this.getChrtPnlData();
            this.obey_chrt_evnts = true;
        }

        private void getChrtPnlData()
        {
            this.updtChrtTotals();
            this.populateChrt();
            this.updtChrtNavLabels();
        }

        private void updtChrtTotals()
        {
            cmnCde.navFuncts.FindNavigationIndices(int.Parse(this.dsplySizeChrtComboBox.Text), this.totl_chrt);
            if (this.chrt_cur_indx >= cmnCde.navFuncts.totalGroups)
            {
                this.chrt_cur_indx = cmnCde.navFuncts.totalGroups - 1;
            }
            if (this.chrt_cur_indx < 0)
            {
                this.chrt_cur_indx = 0;
            }
            cmnCde.navFuncts.currentNavigationIndex = this.chrt_cur_indx;
        }

        private void updtChrtNavLabels()
        {
            this.moveFirstChrtButton.Enabled = cmnCde.navFuncts.moveFirstBtnStatus();
            this.movePreviousChrtButton.Enabled = cmnCde.navFuncts.movePrevBtnStatus();
            this.moveNextChrtButton.Enabled = cmnCde.navFuncts.moveNextBtnStatus();
            this.moveLastChrtButton.Enabled = cmnCde.navFuncts.moveLastBtnStatus();
            this.positionChrtTextBox.Text = cmnCde.navFuncts.displayedRecordsNumbers();
            if (this.is_last_chrt == true ||
             this.totl_chrt != cmnCde.Big_Val)
            {
                this.totalRecChrtLabel.Text = cmnCde.navFuncts.totalRecordsLabel();
            }
            else
            {
                this.totalRecChrtLabel.Text = "of Total";
            }
        }

        private void populateChrtDet(int accntID)
        {
            if (this.addChrt == true && this.benhr == 0)
            {
                if (cmnCde.showMsg("Are you sure you want to Navigate away \r\n from this Record without Saving?", 1) == DialogResult.No)
                {
                    this.benhr++;
                    return;
                }
            }
            else if (this.benhr > 0)
            {
                this.benhr = 0;
                return;
            }
            this.obey_chrt_evnts = false;
            /*if (this.editChrtButton.Text == "STOP")
            {
                this.editChrtButton.Text = "EDIT";
                this.editChrtButton_Click(this.editChrtButton, e);
            }*/
            if (this.editChrt == false)
            {
                this.clearChrtInfo();
                this.disableChrtEdit();
            }

            this.obey_chrt_evnts = false;
            DataSet dtst = this.get_One_Chrt_Det(accntID);
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                this.accntIDTextBox.Text = dtst.Tables[0].Rows[i][0].ToString();
                this.accntNumTextBox.Text = dtst.Tables[0].Rows[i][1].ToString();
                this.accntNameTextBox.Text = dtst.Tables[0].Rows[i][2].ToString();
                this.accntDescTextBox.Text = dtst.Tables[0].Rows[i][3].ToString();
                if (editChrt == false)
                {
                    this.accClsfctnComboBox.Items.Clear();
                    this.accClsfctnComboBox.Items.Add(dtst.Tables[0].Rows[i][26].ToString());
                }
                this.accClsfctnComboBox.SelectedItem = dtst.Tables[0].Rows[i][26].ToString();
                this.isContraCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][4].ToString());
                this.parentAccntIDTextBox.Text = dtst.Tables[0].Rows[i][5].ToString();
                this.parentAccntTextBox.Text = cmnCde.getAccntName(int.Parse(dtst.Tables[0].Rows[i][5].ToString()));
                this.balsDateTextBox.Text = dtst.Tables[0].Rows[i][6].ToString();
                this.accntTypeComboBox.Items.Clear();
                if (dtst.Tables[0].Rows[i][12].ToString() == "A")
                {
                    this.accntTypeComboBox.Items.Add("A -ASSET");
                }
                else if (dtst.Tables[0].Rows[i][12].ToString() == "EQ")
                {
                    this.accntTypeComboBox.Items.Add("EQ-EQUITY");
                }
                else if (dtst.Tables[0].Rows[i][12].ToString() == "L")
                {
                    this.accntTypeComboBox.Items.Add("L -LIABILITY");
                }
                else if (dtst.Tables[0].Rows[i][12].ToString() == "R")
                {
                    this.accntTypeComboBox.Items.Add("R -REVENUE");
                }
                else if (dtst.Tables[0].Rows[i][12].ToString() == "EX")
                {
                    this.accntTypeComboBox.Items.Add("EX-EXPENSE");
                }
                if (this.accntTypeComboBox.Items.Count > 0)
                {
                    this.accntTypeComboBox.SelectedIndex = 0;
                }

                this.isPrntAccntsCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][13].ToString());
                if (this.isPrntAccntsCheckBox.Checked == true)
                {
                    DataSet dtst1 = this.get_Bals_Prnt_Accnts(int.Parse(this.accntIDTextBox.Text));
                    if (dtst1.Tables[0].Rows.Count > 0)
                    {
                        float a = 0;
                        float b = 0;
                        float c = 0;
                        float.TryParse(dtst1.Tables[0].Rows[0][0].ToString(), out a);
                        float.TryParse(dtst1.Tables[0].Rows[0][1].ToString(), out b);
                        float.TryParse(dtst1.Tables[0].Rows[0][2].ToString(), out c);

                        this.dbtBalNumericUpDown.Value = (Decimal)Math.Round(a, 2);
                        this.crdtBalNumericUpDown.Value = (Decimal)Math.Round(b, 2);
                        this.netBalNumericUpDown.Value = (Decimal)Math.Round(c, 2);
                        this.balsDateTextBox.Text = cmnCde.getFrmtdDB_Date_time().Substring(0, 11);
                    }
                }
                else
                {
                    this.dbtBalNumericUpDown.Value = (Decimal)float.Parse(dtst.Tables[0].Rows[i][14].ToString());
                    this.crdtBalNumericUpDown.Value = (Decimal)float.Parse(dtst.Tables[0].Rows[i][15].ToString());
                    this.netBalNumericUpDown.Value = (Decimal)float.Parse(dtst.Tables[0].Rows[i][17].ToString());
                }
                if (this.crdtBalNumericUpDown.Value > this.dbtBalNumericUpDown.Value)
                {
                    this.netBalTypeLabel.Text = "CREDIT";
                }
                else if (this.crdtBalNumericUpDown.Value < this.dbtBalNumericUpDown.Value)
                {
                    this.netBalTypeLabel.Text = "DEBIT";
                }
                else
                {
                    this.netBalTypeLabel.Text = "";
                }

                this.isEnabledAccntsCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][16].ToString());
                this.isRetEarnsCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][18].ToString());
                this.isNetIncmCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][19].ToString());
                this.rptLnNoUpDown.Value = Decimal.Parse(dtst.Tables[0].Rows[i][21].ToString());

                this.hasSubldgrCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][22].ToString());
                this.isSuspensCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][25].ToString());
                this.cntrlAccntIDTextBox.Text = dtst.Tables[0].Rows[i][23].ToString();
                this.cntrlAccntTextBox.Text = cmnCde.getAccntName(int.Parse(dtst.Tables[0].Rows[i][23].ToString()));
                this.accntCurrIDTextBox.Text = dtst.Tables[0].Rows[i][24].ToString();
                this.accntCrncyNmTextBox.Text = cmnCde.getPssblValNm(int.Parse(dtst.Tables[0].Rows[i][24].ToString()))
                  + " - " + cmnCde.getPssblValDesc(int.Parse(dtst.Tables[0].Rows[i][24].ToString()));

                this.accntSgmnt1TextBox.Text = dtst.Tables[0].Rows[i][27].ToString();
                this.accntSgmnt2TextBox.Text = dtst.Tables[0].Rows[i][28].ToString();
                this.accntSgmnt3TextBox.Text = dtst.Tables[0].Rows[i][29].ToString();
                this.accntSgmnt4TextBox.Text = dtst.Tables[0].Rows[i][30].ToString();
                this.accntSgmnt5TextBox.Text = dtst.Tables[0].Rows[i][31].ToString();
                this.accntSgmnt6TextBox.Text = dtst.Tables[0].Rows[i][32].ToString();
                this.accntSgmnt7TextBox.Text = dtst.Tables[0].Rows[i][33].ToString();
                this.accntSgmnt8TextBox.Text = dtst.Tables[0].Rows[i][34].ToString();
                this.accntSgmnt9TextBox.Text = dtst.Tables[0].Rows[i][35].ToString();
                this.accntSgmnt10TextBox.Text = dtst.Tables[0].Rows[i][36].ToString();
                this.mappedAccntIDTextBox.Text = dtst.Tables[0].Rows[i][37].ToString();
                this.mappedAccntTextBox.Text = cmnCde.getAccntNum(int.Parse(dtst.Tables[0].Rows[i][37].ToString()))
                + "." + cmnCde.getAccntName(int.Parse(dtst.Tables[0].Rows[i][37].ToString()));

                this.accntCurrLabel.Text = "ACCOUNT CURRENCY BALANCE (" +
                  cmnCde.getPssblValNm(int.Parse(dtst.Tables[0].Rows[i][24].ToString())) + ")";

                if (this.accntCurrIDTextBox.Text == this.funCurID.ToString())
                {
                    this.bals2DteTextBox.Text = this.balsDateTextBox.Text;
                    this.dbtBal2NumericUpDown.Value = this.dbtBalNumericUpDown.Value;
                    this.crdtBal2NumericUpDown.Value = this.crdtBalNumericUpDown.Value;
                    this.netBal2NumericUpDown.Value = this.netBalNumericUpDown.Value;
                }
                else
                {
                    if (this.isPrntAccntsCheckBox.Checked == true)
                    {
                        DataSet dtst1 = this.get_CurrBals_Prnt_Accnts(
                          int.Parse(this.accntIDTextBox.Text), int.Parse(this.accntCurrIDTextBox.Text));
                        if (dtst1.Tables[0].Rows.Count > 0)
                        {
                            float a = 0;
                            float b = 0;
                            float c = 0;
                            float.TryParse(dtst1.Tables[0].Rows[0][0].ToString(), out a);
                            float.TryParse(dtst1.Tables[0].Rows[0][1].ToString(), out b);
                            float.TryParse(dtst1.Tables[0].Rows[0][2].ToString(), out c);

                            this.bals2DteTextBox.Text = cmnCde.getFrmtdDB_Date_time().Substring(0, 11);
                            this.dbtBal2NumericUpDown.Value = (Decimal)Math.Round(a, 2);
                            this.crdtBal2NumericUpDown.Value = (Decimal)Math.Round(b, 2);
                            this.netBal2NumericUpDown.Value = (Decimal)Math.Round(c, 2);
                        }
                        else
                        {
                            this.bals2DteTextBox.Text = "";
                            this.dbtBal2NumericUpDown.Value = 0;
                            this.crdtBal2NumericUpDown.Value = 0;
                            this.netBal2NumericUpDown.Value = 0;
                        }
                    }
                    else if (this.hasSubldgrCheckBox.Checked == true)
                    {
                        DataSet dtst1 = this.get_CurrBals_Cntrl_Accnts(
                           int.Parse(this.accntIDTextBox.Text), int.Parse(this.accntCurrIDTextBox.Text));
                        if (dtst1.Tables[0].Rows.Count > 0)
                        {
                            float a = 0;
                            float b = 0;
                            float c = 0;
                            float.TryParse(dtst1.Tables[0].Rows[0][0].ToString(), out a);
                            float.TryParse(dtst1.Tables[0].Rows[0][1].ToString(), out b);
                            float.TryParse(dtst1.Tables[0].Rows[0][2].ToString(), out c);

                            this.bals2DteTextBox.Text = cmnCde.getFrmtdDB_Date_time().Substring(0, 11);
                            this.dbtBal2NumericUpDown.Value = (Decimal)Math.Round(a, 2);
                            this.crdtBal2NumericUpDown.Value = (Decimal)Math.Round(b, 2);
                            this.netBal2NumericUpDown.Value = (Decimal)Math.Round(c, 2);
                        }
                        else
                        {
                            this.bals2DteTextBox.Text = "";
                            this.dbtBal2NumericUpDown.Value = 0;
                            this.crdtBal2NumericUpDown.Value = 0;
                            this.netBal2NumericUpDown.Value = 0;
                        }
                    }
                    else
                    {
                        DataSet dtst1 = this.get_CurrBals_Accnts(int.Parse(this.accntIDTextBox.Text));
                        if (dtst1.Tables[0].Rows.Count > 0)
                        {
                            float a = 0;
                            float b = 0;
                            float c = 0;
                            float.TryParse(dtst1.Tables[0].Rows[0][0].ToString(), out a);
                            float.TryParse(dtst1.Tables[0].Rows[0][1].ToString(), out b);
                            float.TryParse(dtst1.Tables[0].Rows[0][2].ToString(), out c);

                            this.bals2DteTextBox.Text = dtst1.Tables[0].Rows[0][3].ToString();
                            this.dbtBal2NumericUpDown.Value = (Decimal)Math.Round(a, 2);
                            this.crdtBal2NumericUpDown.Value = (Decimal)Math.Round(b, 2);
                            this.netBal2NumericUpDown.Value = (Decimal)Math.Round(c, 2);
                        }
                        else
                        {
                            this.bals2DteTextBox.Text = "";
                            this.dbtBal2NumericUpDown.Value = 0;
                            this.crdtBal2NumericUpDown.Value = 0;
                            this.netBal2NumericUpDown.Value = 0;
                        }

                    }
                }
            }
            if (this.editChrt == true)
            {
                if (this.netBalNumericUpDown.Value != 0
                  || this.crdtBalNumericUpDown.Value != 0
                  || this.dbtBalNumericUpDown.Value != 0
                  || this.accntSgmnt1TextBox.Text != "-1")
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
                }
                this.accntNumTextBox.ReadOnly = false;
                this.accntNumTextBox.BackColor = Color.FromArgb(255, 255, 118);
                this.accntNameTextBox.ReadOnly = false;
                this.accntNameTextBox.BackColor = Color.FromArgb(255, 255, 118);
                this.accntDescTextBox.ReadOnly = false;
                this.accntDescTextBox.BackColor = Color.FromArgb(255, 255, 118);
                /*if (this.accntSgmnt1TextBox.Text != "-1")
                {
                    this.accntNumTextBox.ReadOnly = true;
                    this.accntNumTextBox.BackColor = Color.WhiteSmoke;
                    this.accntNameTextBox.ReadOnly = true;
                    this.accntNameTextBox.BackColor = Color.WhiteSmoke;
                    this.accntDescTextBox.ReadOnly = true;
                    this.accntDescTextBox.BackColor = Color.WhiteSmoke;
                }
                else
                {
                    
                }*/
            }
            this.obey_chrt_evnts = true;
        }

        private void populateChrt()
        {
            this.obey_chrt_evnts = false;
            if (this.editChrt == false)
            {
                this.clearChrtInfo();
                this.disableChrtEdit();
            }
            this.accntsChrtListView.Items.Clear();
            DataSet dtst = this.get_Basic_ChrtDet(this.searchForChrtTextBox.Text,
             this.searchInChrtComboBox.Text, this.chrt_cur_indx, int.Parse(this.dsplySizeChrtComboBox.Text)
             , cmnCde.Org_id, this.trnsAcntLovID);
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                this.last_chrt_num = cmnCde.navFuncts.startIndex() + i;
                ListViewItem nwItem = new ListViewItem(new string[] {
    (cmnCde.navFuncts.startIndex() + i).ToString(),
    dtst.Tables[0].Rows[i][1].ToString(),
    dtst.Tables[0].Rows[i][2].ToString(),
    dtst.Tables[0].Rows[i][0].ToString(),
    dtst.Tables[0].Rows[i][3].ToString()});
                this.accntsChrtListView.Items.Add(nwItem);
            }
            this.correctChrtNavLbls(dtst);
            if (this.accntsChrtListView.Items.Count > 0)
            {
                this.obey_chrt_evnts = true;
                this.accntsChrtListView.Items[0].Selected = true;
            }
            this.obey_chrt_evnts = true;
        }

        private void correctChrtNavLbls(DataSet dtst)
        {
            long totlRecs = dtst.Tables[0].Rows.Count;
            if (this.chrt_cur_indx == 0 && totlRecs == 0)
            {
                this.is_last_chrt = true;
                this.totl_chrt = 0;
                this.last_chrt_num = 0;
                this.chrt_cur_indx = 0;
                this.updtChrtTotals();
                this.updtChrtNavLabels();
            }
            else if (this.totl_chrt == cmnCde.Big_Val
          && totlRecs < int.Parse(this.dsplySizeChrtComboBox.Text))
            {
                this.totl_chrt = this.last_chrt_num;
                if (totlRecs == 0)
                {
                    this.chrt_cur_indx -= 1;
                    this.updtChrtTotals();
                    this.populateChrt();
                }
                else
                {
                    this.updtChrtTotals();
                }
            }
        }

        private void clearChrtInfo()
        {
            this.obey_chrt_evnts = false;
            this.beenClicked = false;
            this.saveChrtButton.Enabled = false;
            this.addChrtButton.Enabled = this.addAccounts;
            this.editChrtButton.Enabled = this.editAccounts;
            this.deleteChrtButton.Enabled = this.delAccounts;

            this.accntIDTextBox.Text = "-1";
            this.accntNumTextBox.Text = "";
            this.accntNameTextBox.Text = "";
            this.accntDescTextBox.Text = "";
            this.accClsfctnComboBox.SelectedIndex = -1;
            this.isEnabledAccntsCheckBox.Checked = true;
            this.isPrntAccntsCheckBox.Checked = false;
            this.isContraCheckBox.Checked = false;
            this.isRetEarnsCheckBox.Checked = false;
            this.isNetIncmCheckBox.Checked = false;
            this.hasSubldgrCheckBox.Checked = false;
            this.isSuspensCheckBox.Checked = false;

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

            this.parentAccntIDTextBox.Text = "-1";
            this.parentAccntTextBox.Text = "";
            this.cntrlAccntIDTextBox.Text = "-1";
            this.cntrlAccntTextBox.Text = "";
            this.accntCurrIDTextBox.Text = "-1";
            this.accntCrncyNmTextBox.Text = "";

            this.accntSgmnt1TextBox.Text = "-1";
            this.accntSgmnt2TextBox.Text = "-1";
            this.accntSgmnt3TextBox.Text = "-1";
            this.accntSgmnt4TextBox.Text = "-1";
            this.accntSgmnt5TextBox.Text = "-1";
            this.accntSgmnt6TextBox.Text = "-1";
            this.accntSgmnt7TextBox.Text = "-1";
            this.accntSgmnt8TextBox.Text = "-1";
            this.accntSgmnt9TextBox.Text = "-1";
            this.accntSgmnt10TextBox.Text = "-1";

            this.accntTypeComboBox.Items.Clear();
            this.rptLnNoUpDown.Value = 100;
            this.balsDateTextBox.Text = "";
            this.dbtBalNumericUpDown.Value = 0;
            this.crdtBalNumericUpDown.Value = 0;
            this.netBalNumericUpDown.Value = 0;
            this.dbtBalNumericUpDown.BackColor = Color.Green;
            this.crdtBalNumericUpDown.BackColor = Color.Green;
            this.netBalNumericUpDown.BackColor = Color.Green;

            this.bals2DteTextBox.Text = "";
            this.dbtBal2NumericUpDown.Value = 0;
            this.crdtBal2NumericUpDown.Value = 0;
            this.netBal2NumericUpDown.Value = 0;
            this.dbtBal2NumericUpDown.BackColor = Color.Green;
            this.crdtBal2NumericUpDown.BackColor = Color.Green;
            this.netBal2NumericUpDown.BackColor = Color.Green;

            this.netBalTypeLabel.Text = "";
            //this.disableFormButtons(Global.currentPanel);
            this.obey_chrt_evnts = true;
        }

        private void prpareForChrtEdit()
        {
            this.saveChrtButton.Enabled = true;
            this.accntNumTextBox.ReadOnly = false;
            this.accntNumTextBox.BackColor = Color.FromArgb(255, 255, 118);
            this.accntNameTextBox.ReadOnly = false;
            this.accntNameTextBox.BackColor = Color.FromArgb(255, 255, 118);
            this.accntDescTextBox.ReadOnly = false;
            this.accntDescTextBox.BackColor = Color.FromArgb(255, 255, 118);
            /*if (this.accntSgmnt1TextBox.Text != "-1")
            {
                this.accntNumTextBox.ReadOnly = true;
                this.accntNumTextBox.BackColor = Color.WhiteSmoke;
                this.accntNameTextBox.ReadOnly = true;
                this.accntNameTextBox.BackColor = Color.WhiteSmoke;
                this.accntDescTextBox.ReadOnly = true;
                this.accntDescTextBox.BackColor = Color.WhiteSmoke;
            }
            else
            {
                this.accntNumTextBox.ReadOnly = false;
                this.accntNumTextBox.BackColor = Color.FromArgb(255, 255, 118);
                this.accntNameTextBox.ReadOnly = false;
                this.accntNameTextBox.BackColor = Color.FromArgb(255, 255, 118);
                this.accntDescTextBox.ReadOnly = false;
                this.accntDescTextBox.BackColor = Color.FromArgb(255, 255, 118);
            }*/
            this.accClsfctnComboBox.BackColor = Color.White;

            this.accntCrncyNmTextBox.ReadOnly = false;
            this.accntCrncyNmTextBox.BackColor = Color.FromArgb(255, 255, 118);



            this.rptLnNoUpDown.Increment = 1;
            this.rptLnNoUpDown.ReadOnly = false;
            this.rptLnNoUpDown.BackColor = Color.White;
            string orgItm = this.accntTypeComboBox.Text;
            this.accntTypeComboBox.Items.Clear();
            this.accntTypeComboBox.Items.Add("A -ASSET");
            this.accntTypeComboBox.Items.Add("EQ-EQUITY");
            this.accntTypeComboBox.Items.Add("L -LIABILITY");
            this.accntTypeComboBox.Items.Add("R -REVENUE");
            this.accntTypeComboBox.Items.Add("EX-EXPENSE");
            if (this.editChrt == true)
            {
                this.accntTypeComboBox.SelectedItem = orgItm;
            }

            orgItm = this.accClsfctnComboBox.Text;
            this.accClsfctnComboBox.Items.Clear();
            for (int a = 0; a < this.cashFlowClsfctns.Length; a++)
            {
                this.accClsfctnComboBox.Items.Add(this.cashFlowClsfctns[a]);
            }
            if (this.editChrt == true)
            {
                this.accClsfctnComboBox.SelectedItem = orgItm;
            }
        }

        private void disableChrtEdit()
        {
            this.addChrt = false;
            this.editChrt = false;
            this.saveChrtButton.Enabled = false;
            this.editChrtButton.Enabled = this.addAccounts;
            this.addChrtButton.Enabled = this.editAccounts;
            this.deleteChrtButton.Enabled = this.delAccounts;
            this.editChrtButton.Text = "EDIT";
            this.editAcntMenuItem.Text = "&Edit Account";
            this.accntNumTextBox.ReadOnly = true;
            this.accntNumTextBox.BackColor = Color.WhiteSmoke;
            this.accntNameTextBox.ReadOnly = true;
            this.accntNameTextBox.BackColor = Color.WhiteSmoke;
            this.accntDescTextBox.ReadOnly = true;
            this.accntDescTextBox.BackColor = Color.WhiteSmoke;
            this.accClsfctnComboBox.BackColor = Color.WhiteSmoke;

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

        private bool shdObeyChrtEvts()
        {
            return this.obey_chrt_evnts;
        }

        private void ChrtPnlNavButtons(object sender, System.EventArgs e)
        {
            System.Windows.Forms.ToolStripButton sentObj = (System.Windows.Forms.ToolStripButton)sender;
            this.totalRecChrtLabel.Text = "";
            if (sentObj.Name.ToLower().Contains("first"))
            {
                this.is_last_chrt = false;
                this.chrt_cur_indx = 0;
            }
            else if (sentObj.Name.ToLower().Contains("previous"))
            {
                this.is_last_chrt = false;
                this.chrt_cur_indx -= 1;
            }
            else if (sentObj.Name.ToLower().Contains("next"))
            {
                this.is_last_chrt = false;
                this.chrt_cur_indx += 1;
            }
            else if (sentObj.Name.ToLower().Contains("last"))
            {
                this.is_last_chrt = true;
                this.totl_chrt = this.get_Total_Chrts(this.searchForChrtTextBox.Text,
                 this.searchInChrtComboBox.Text, cmnCde.Org_id, this.trnsAcntLovID);
                this.updtChrtTotals();
                this.chrt_cur_indx = cmnCde.navFuncts.totalGroups - 1;
            }
            this.getChrtPnlData();
        }

        private void parntAccntButton_Click(object sender, EventArgs e)
        {
            if (this.addChrt == false && this.editChrt == false)
            {
                cmnCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }
            if (this.accntTypeComboBox.Text == "")
            {
                cmnCde.showMsg("Please select an Account Type First!", 0);
                return;
            }

            string[] selVals = new string[1];
            selVals[0] = this.parentAccntIDTextBox.Text;
            DialogResult dgRes = cmnCde.showPssblValDiag(
             cmnCde.getLovID("Parent Accounts"), ref selVals,
             true, false, cmnCde.Org_id,
             this.accntTypeComboBox.Text.Substring(0, 2).Trim(), "");
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.parentAccntIDTextBox.Text = selVals[i];
                    this.parentAccntTextBox.Text = cmnCde.getAccntName(int.Parse(selVals[i]));
                }
            }
            //if (int.Parse(this.accntIDTextBox.Text) > 0)
            //{
            //  Global.updtAccntPrntID(int.Parse(this.accntIDTextBox.Text),
            //    int.Parse(this.parentAccntIDTextBox.Text));
            //}
        }

        private void accntsExtraInfoButton_Click(object sender, EventArgs e)
        {
            if (this.accntIDTextBox.Text == "" ||
                         this.accntIDTextBox.Text == "-1")
            {
                cmnCde.showMsg("No record to View!", 0);
                return;
            }
            bool canEdt = cmnCde.test_prmssns(this.dfltPrvldgs[12]);
            DialogResult dgres = this.cmnCde.showRowsExtInfDiag(this.cmnCde.getMdlGrpID("Chart of Accounts"),
             long.Parse(this.accntIDTextBox.Text), "accb.accb_all_other_info_table", this.accntNameTextBox.Text, canEdt, 10, 9,
                "accb.accb_all_other_info_table_dflt_row_id_seq");
            if (dgres == DialogResult.OK)
            {
            }
        }

        private void goChrtButton_Click(object sender, EventArgs e)
        {
            this.disableChrtEdit();
            System.Windows.Forms.Application.DoEvents();
            this.loadAccntChrtPanel();
        }

        private void addChrtButton_Click(object sender, EventArgs e)
        {
            if (cmnCde.test_prmssns(this.dfltPrvldgs[11]) == false)
            {
                cmnCde.showMsg("You don't have permission to perform" +
                 " this action!\nContact your System Administrator!", 0);
                return;
            }
            this.clearChrtInfo();
            this.addChrt = true;
            this.editChrt = false;
            this.prpareForChrtEdit();
            this.addChrtButton.Enabled = false;
            this.editChrtButton.Enabled = false;
            this.deleteChrtButton.Enabled = false;
            this.txtChngd = false;
            this.accntCurrIDTextBox.Text = cmnCde.getOrgFuncCurID(cmnCde.Org_id).ToString();
            this.accntCrncyNmTextBox.Text = cmnCde.getPssblValNm(int.Parse(this.accntCurrIDTextBox.Text)) +
                  " - " + cmnCde.getPssblValDesc(int.Parse(this.accntCurrIDTextBox.Text));
            this.txtChngd = false;
        }

        private void editChrtButton_Click(object sender, EventArgs e)
        {
            if (this.editChrtButton.Text == "EDIT")
            {
                if (cmnCde.test_prmssns(this.dfltPrvldgs[12]) == false)
                {
                    cmnCde.showMsg("You don't have permission to perform" +
                     " this action!\nContact your System Administrator!", 0);
                    return;
                }
                if (this.accntIDTextBox.Text == "" || this.accntIDTextBox.Text == "-1")
                {
                    cmnCde.showMsg("No record to Edit!", 0);
                    return;
                }
                if (this.netBalNumericUpDown.Value != 0
                  || this.dbtBalNumericUpDown.Value != 0
                  || this.crdtBalNumericUpDown.Value != 0
                  || this.accntSgmnt1TextBox.Text != "-1")
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
                this.addChrt = false;
                this.editChrt = true;
                this.prpareForChrtEdit();
                this.addChrtButton.Enabled = false;
                this.editChrtButton.Enabled = true;
                this.deleteChrtButton.Enabled = this.delAccounts;
                this.editChrtButton.Text = "STOP";
                this.editAcntMenuItem.Text = "STOP EDITING";
            }
            else
            {
                this.disableChrtEdit();
                System.Windows.Forms.Application.DoEvents();
                this.loadAccntChrtPanel();
            }
        }

        private void deleteChrtButton_Click(object sender, EventArgs e)
        {
            if (cmnCde.test_prmssns(this.dfltPrvldgs[13]) == false)
            {
                cmnCde.showMsg("You don't have permission to perform" +
                 " this action!\nContact your System Administrator!", 0);
                return;
            }
            if (this.accntsChrtListView.SelectedItems.Count <= 0)
            {
                cmnCde.showMsg("Please select the record to delete!", 0);
                return;
            }
            long accntid = long.Parse(this.accntsChrtListView.SelectedItems[0].SubItems[3].Text);
            if (this.get_Accnt_Tot_Trns(accntid) > 0)
            {
                cmnCde.showMsg("Cannot delete accounts with Transactions in their Name!", 0);
                return;
            }

            if (this.get_Accnt_Tot_Chldrn(accntid) > 0)
            {
                cmnCde.showMsg("Cannot delete Parent Accounts with Child Accounts!", 0);
                return;
            }
            if (this.get_Accnt_Tot_Mappngs(accntid) > 0)
            {
                cmnCde.showMsg("Cannot delete Accounts with Subsidiary Account Mappings!", 0);
                return;
            }
            if (this.get_Accnt_Tot_Pymnts(accntid) > 0)
            {
                cmnCde.showMsg("Cannot delete accounts with Personnel Payments in their Name!", 0);
                return;
            }
            if (this.get_Accnt_Tot_PyItms(accntid) > 0)
            {
                cmnCde.showMsg("Cannot delete accounts with Pay Items in their Name!", 0);
                return;
            }

            if (cmnCde.showMsg("Are you sure you want to DELETE the selected Account?" +
             "\r\nThis action cannot be undone!", 1) == DialogResult.No)
            {
                cmnCde.showMsg("Operation Cancelled!", 4);
                return;
            }
            this.deleteAccount(accntid, this.accntsChrtListView.SelectedItems[0].SubItems[1].Text
              , this.accntsChrtListView.SelectedItems[0].SubItems[2].Text);
            this.loadAccntChrtPanel();
        }

        private void saveChrtButton_Click(object sender, EventArgs e)
        {
            if (this.addChrt == true)
            {
                if (cmnCde.test_prmssns(this.dfltPrvldgs[11]) == false)
                {
                    cmnCde.showMsg("You don't have permission to perform" +
                     " this action!\nContact your System Administrator!", 0);
                    return;
                }
            }
            else
            {
                if (cmnCde.test_prmssns(this.dfltPrvldgs[12]) == false)
                {
                    cmnCde.showMsg("You don't have permission to perform" +
                     " this action!\nContact your System Administrator!", 0);
                    return;
                }
            }
            char[] w = { '.' };
            this.accntNumTextBox.Text = this.accntNumTextBox.Text.Trim(w);
            this.accntNameTextBox.Text = this.accntNameTextBox.Text.Trim(w);
            if (this.accntNameTextBox.Text == "")
            {
                cmnCde.showMsg("Please enter an Account Name!", 0);
                return;
            }
            if (this.accntNumTextBox.Text == "")
            {
                cmnCde.showMsg("Please enter an Account Number!", 0);
                return;
            }
            if (this.accntTypeComboBox.Text == "")
            {
                cmnCde.showMsg("Please select an account Type!", 0);
                return;
            }
            if (this.isRetEarnsCheckBox.Checked == true && this.isPrntAccntsCheckBox.Checked == true)
            {
                cmnCde.showMsg("A Parent account cannot be used as Retained Earinings Account!", 0);
                return;
            }
            if (this.isRetEarnsCheckBox.Checked == true && this.isContraCheckBox.Checked == true)
            {
                cmnCde.showMsg("A contra account cannot be used as Retained Earinings Account!", 0);
                return;
            }
            if (this.isRetEarnsCheckBox.Checked == true && this.isEnabledAccntsCheckBox.Checked == false)
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
            if (this.isNetIncmCheckBox.Checked == true && this.isEnabledAccntsCheckBox.Checked == false)
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
            if (this.cntrlAccntIDTextBox.Text != "-1" && this.parentAccntIDTextBox.Text != "-1")
            {
                cmnCde.showMsg("An Account with a Control Account cannot have a Parent Account as well!", 0);
                return;
            }
            if (this.parentAccntIDTextBox.Text != "-1")
            {
                if (cmnCde.getAccntType(int.Parse(parentAccntIDTextBox.Text)) !=
                 this.accntTypeComboBox.Text.Substring(0, 2).Trim())
                {
                    cmnCde.showMsg("Account Type does not match that of the Parent Account", 0);
                    return;
                }
            }
            if (this.accntCurrIDTextBox.Text == "-1" || this.accntCurrIDTextBox.Text == "")
            {
                cmnCde.showMsg("Account Currency Cannot be Empty!", 0);
                return;
            }
            int oldAccntNosID = cmnCde.getAccntID(this.accntNumTextBox.Text, cmnCde.Org_id);
            if (oldAccntNosID > 0
             && this.addChrt == true)
            {
                cmnCde.showMsg("Account Number is already in use in this Organization!", 0);
                return;
            }
            if (oldAccntNosID > 0
             && this.editChrt == true
             && oldAccntNosID.ToString() != this.accntIDTextBox.Text)
            {
                cmnCde.showMsg("New Account Number is already in use in this Organization!", 0);
                return;
            }

            int oldAccntNmID = cmnCde.getAccntID(this.accntNameTextBox.Text, cmnCde.Org_id);
            if (oldAccntNmID > 0
             && this.addChrt == true)
            {
                cmnCde.showMsg("Account Name is already in use in this Organization!", 0);
                return;
            }
            if (oldAccntNmID > 0
             && this.editChrt == true
             && oldAccntNmID.ToString() != this.accntIDTextBox.Text)
            {
                cmnCde.showMsg("New Account Name is already in use in this Organization!", 0);
                return;
            }

            int oldCmbntnID = cmnCde.getAccountCmbntnID(cmnCde.Org_id,
                 int.Parse(this.accntSgmnt1TextBox.Text),
                 int.Parse(this.accntSgmnt2TextBox.Text),
                 int.Parse(this.accntSgmnt3TextBox.Text),
                 int.Parse(this.accntSgmnt4TextBox.Text),
                 int.Parse(this.accntSgmnt5TextBox.Text),
                 int.Parse(this.accntSgmnt6TextBox.Text),
                 int.Parse(this.accntSgmnt7TextBox.Text),
                 int.Parse(this.accntSgmnt8TextBox.Text),
                 int.Parse(this.accntSgmnt9TextBox.Text),
                 int.Parse(this.accntSgmnt10TextBox.Text));
            if (oldCmbntnID > 0 && oldCmbntnID != int.Parse(this.accntIDTextBox.Text))
            {
                cmnCde.showMsg("This combination of Segment Values is already present in this Organization!", 0);
                return;
            }
            if (this.addChrt == true)
            {
                this.createChrt(cmnCde.Org_id,
                 this.accntNumTextBox.Text, this.accntNameTextBox.Text, this.accntDescTextBox.Text,
                 this.isContraCheckBox.Checked, int.Parse(this.parentAccntIDTextBox.Text),
                 this.accntTypeComboBox.Text.Substring(0, 2).Trim(),
                 this.isPrntAccntsCheckBox.Checked, this.isEnabledAccntsCheckBox.Checked,
                 this.isRetEarnsCheckBox.Checked, this.isNetIncmCheckBox.Checked,
                 (int)this.rptLnNoUpDown.Value, this.hasSubldgrCheckBox.Checked,
                 int.Parse(this.cntrlAccntIDTextBox.Text),
                 int.Parse(this.accntCurrIDTextBox.Text), this.isSuspensCheckBox.Checked,
                 this.accClsfctnComboBox.Text,
                 int.Parse(this.accntSgmnt1TextBox.Text),
                 int.Parse(this.accntSgmnt2TextBox.Text),
                 int.Parse(this.accntSgmnt3TextBox.Text),
                 int.Parse(this.accntSgmnt4TextBox.Text),
                 int.Parse(this.accntSgmnt5TextBox.Text),
                 int.Parse(this.accntSgmnt6TextBox.Text),
                 int.Parse(this.accntSgmnt7TextBox.Text),
                 int.Parse(this.accntSgmnt8TextBox.Text),
                 int.Parse(this.accntSgmnt9TextBox.Text),
                 int.Parse(this.accntSgmnt10TextBox.Text),
                 int.Parse(this.mappedAccntIDTextBox.Text));

                oldAccntNosID = cmnCde.getAccntID(this.accntNumTextBox.Text, cmnCde.Org_id);

                this.saveChrtButton.Enabled = false;
                this.addChrt = false;
                this.editChrt = false;
                this.editChrtButton.Enabled = this.addAccounts;
                this.addChrtButton.Enabled = this.editAccounts;
                this.deleteChrtButton.Enabled = this.delAccounts;
                System.Windows.Forms.Application.DoEvents();
                this.accntIDTextBox.Text = oldAccntNosID.ToString();
                //this.loadAccntChrtPanel();

                System.Windows.Forms.Application.DoEvents();
                ListViewItem nwItem = new ListViewItem(new string[] {
                            "New",
                            this.accntNumTextBox.Text,
                            this.accntNameTextBox.Text,
                            this.accntIDTextBox.Text,
                            this.accntNumTextBox.Text +"."+this.accntNameTextBox.Text});
                this.accntsChrtListView.Items.Insert(0, nwItem);
                int slctnCnt = this.accntsChrtListView.SelectedItems.Count;
                for (int i = 0; i < slctnCnt; i++)
                {
                    this.accntsChrtListView.SelectedItems[i].Font = new Font("Tahoma", 8.25f, FontStyle.Regular);
                    this.accntsChrtListView.SelectedItems[i].Checked = false;
                    this.accntsChrtListView.SelectedItems[i].Selected = false;
                }
                this.accntsChrtListView.Items[0].Selected = true;
                this.accntsChrtListView.Items[0].Checked = true;
                this.accntsChrtListView.Items[0].Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
                System.Windows.Forms.Application.DoEvents();
            }
            else if (this.editChrt == true)
            {
                this.updateChrtDet(cmnCde.Org_id, int.Parse(this.accntIDTextBox.Text),
                 this.accntNumTextBox.Text, this.accntNameTextBox.Text, this.accntDescTextBox.Text,
                 this.isContraCheckBox.Checked, int.Parse(this.parentAccntIDTextBox.Text),
                 this.accntTypeComboBox.Text.Substring(0, 2).Trim(),
                 this.isPrntAccntsCheckBox.Checked, this.isEnabledAccntsCheckBox.Checked,
                 this.isRetEarnsCheckBox.Checked, this.isNetIncmCheckBox.Checked,
                 (int)this.rptLnNoUpDown.Value, this.hasSubldgrCheckBox.Checked,
                 int.Parse(this.cntrlAccntIDTextBox.Text),
                 int.Parse(this.accntCurrIDTextBox.Text), this.isSuspensCheckBox.Checked,
                 this.accClsfctnComboBox.Text,
                 int.Parse(this.accntSgmnt1TextBox.Text),
                 int.Parse(this.accntSgmnt2TextBox.Text),
                 int.Parse(this.accntSgmnt3TextBox.Text),
                 int.Parse(this.accntSgmnt4TextBox.Text),
                 int.Parse(this.accntSgmnt5TextBox.Text),
                 int.Parse(this.accntSgmnt6TextBox.Text),
                 int.Parse(this.accntSgmnt7TextBox.Text),
                 int.Parse(this.accntSgmnt8TextBox.Text),
                 int.Parse(this.accntSgmnt9TextBox.Text),
                 int.Parse(this.accntSgmnt10TextBox.Text),
                 int.Parse(this.mappedAccntIDTextBox.Text));
                if (this.accntsChrtListView.SelectedItems.Count > 0)
                {
                    this.accntsChrtListView.SelectedItems[0].SubItems[1].Text = this.accntNumTextBox.Text;
                    this.accntsChrtListView.SelectedItems[0].SubItems[2].Text = this.accntNameTextBox.Text;
                    this.accntsChrtListView.SelectedItems[0].Checked = true;
                }
                //cmnCde.showMsg("Record Saved!", 3);
            }
            if (oldAccntNosID > 0)
            {
                //Get Report Classifications on the Natural Acount Segment and Save
                int sgmntID = cmnCde.getSegmentID("NaturalAccount", cmnCde.Org_id);
                if (sgmntID > 0)
                {
                    int sgmntNum = -1;
                    int.TryParse(cmnCde.getGnrlRecNm("org.org_acnt_sgmnts", "segment_id", "segment_number", sgmntID), out sgmntNum);
                    int ntrlSgmntValID = -1;
                    switch (sgmntNum)
                    {
                        case 1:
                            ntrlSgmntValID = int.Parse(this.accntSgmnt1TextBox.Text);
                            break;
                        case 2:
                            ntrlSgmntValID = int.Parse(this.accntSgmnt2TextBox.Text);
                            break;
                        case 3:
                            ntrlSgmntValID = int.Parse(this.accntSgmnt3TextBox.Text);
                            break;
                        case 4:
                            ntrlSgmntValID = int.Parse(this.accntSgmnt4TextBox.Text);
                            break;
                        case 5:
                            ntrlSgmntValID = int.Parse(this.accntSgmnt5TextBox.Text);
                            break;
                        case 6:
                            ntrlSgmntValID = int.Parse(this.accntSgmnt6TextBox.Text);
                            break;
                        case 7:
                            ntrlSgmntValID = int.Parse(this.accntSgmnt7TextBox.Text);
                            break;
                        case 8:
                            ntrlSgmntValID = int.Parse(this.accntSgmnt8TextBox.Text);
                            break;
                        case 9:
                            ntrlSgmntValID = int.Parse(this.accntSgmnt9TextBox.Text);
                            break;
                        case 10:
                            ntrlSgmntValID = int.Parse(this.accntSgmnt10TextBox.Text);
                            break;
                        default:
                            break;
                    }
                    if (sgmntNum > 0 && ntrlSgmntValID > 0)
                    {
                        DataSet rptClsDtSt = this.get_SgmntVal_RptClsfctns(ntrlSgmntValID);
                        for (int h = 0; h < rptClsDtSt.Tables[0].Rows.Count; h++)
                        {
                            long rptClsfID = this.get_RptClsfctnID(rptClsDtSt.Tables[0].Rows[h][1].ToString(),
                                rptClsDtSt.Tables[0].Rows[h][2].ToString(), oldAccntNosID);
                            if (rptClsfID <= 0)
                            {
                                rptClsfID = this.getNewRptClsfLnID();
                                this.createRptClsfctn(rptClsfID, rptClsDtSt.Tables[0].Rows[h][1].ToString(),
                                    rptClsDtSt.Tables[0].Rows[h][2].ToString(), oldAccntNosID);
                            }
                            else
                            {
                                this.updateRptClsfctn(rptClsfID, rptClsDtSt.Tables[0].Rows[h][1].ToString(),
                                    rptClsDtSt.Tables[0].Rows[h][2].ToString(), oldAccntNosID);
                            }
                        }
                    }
                }
            }
            if (this.editChrt == true)
            {
                cmnCde.showMsg("Record Saved!", 3);
            }
        }

        int benhr = 0;

        private void accntsChrtListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.shdObeyChrtEvts() == false || this.accntsChrtListView.SelectedItems.Count > 1)
            {
                return;
            }
            if (this.accntsChrtListView.SelectedItems.Count > 0)
            {
                this.populateChrtDet(int.Parse(this.accntsChrtListView.SelectedItems[0].SubItems[3].Text));
            }
            else
            {
                this.populateChrtDet(-12345);
            }
        }

        private void accntsChrtListView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            EventArgs ex = new EventArgs();
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                //this.vwAccntTrnsctnsButton_Click(this.vwAccntTrnsctnsButton, ex);
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (this.saveChrtButton.Enabled == true)
                {
                    this.saveChrtButton_Click(this.saveChrtButton, ex);
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.N)
            {
                if (this.addChrtButton.Enabled == true)
                {
                    this.addChrtButton_Click(this.addChrtButton, ex);
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.E)
            {
                if (this.editChrtButton.Enabled == true)
                {
                    this.editChrtButton_Click(this.editChrtButton, ex);
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if ((e.Control && e.KeyCode == Keys.F) || e.KeyCode == Keys.F5)
            {
                if (this.rfrshChrtButton.Enabled == true)
                {
                    this.rfrshChrtButton_Click(this.rfrshChrtButton, ex);
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.R)
            {
                this.resetChrtButton.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.Delete)
            {
                if (this.deleteChrtButton.Enabled == true)
                {
                    this.deleteChrtButton_Click(this.deleteChrtButton, ex);
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else
            {
                cmnCde.listViewKeyDown(this.accntsChrtListView, e);
            }
        }

        private void exprtExclMenuItem_Click(object sender, EventArgs e)
        {
            cmnCde.exprtToExcel(this.accntsChrtListView);
        }

        private void rfrshChrtButton_Click(object sender, EventArgs e)
        {
            this.disableChrtEdit();
            System.Windows.Forms.Application.DoEvents();
            this.loadAccntChrtPanel();
        }

        private void vwSQLChrtButton_Click(object sender, EventArgs e)
        {
            cmnCde.showSQL(this.chrt_SQL, 10);
        }

        private void recHstryChrtButton_Click(object sender, EventArgs e)
        {
            if (this.accntIDTextBox.Text == "-1"
         || this.accntIDTextBox.Text == "")
            {
                cmnCde.showMsg("Please select an Account First!", 0);
                return;
            }
            cmnCde.showRecHstry(this.get_Chrt_Rec_Hstry(int.Parse(this.accntIDTextBox.Text)), 9);
        }

        private void addAcntMenuItem_Click(object sender, EventArgs e)
        {
            this.addChrtButton_Click(this.addChrtButton, e);
        }

        private void editAcntMenuItem_Click(object sender, EventArgs e)
        {
            this.editChrtButton_Click(this.editChrtButton, e);
        }

        private void delAcntMenuItem_Click(object sender, EventArgs e)
        {
            this.deleteChrtButton_Click(this.deleteChrtButton, e);
        }

        private void rfrshAcntMenuItem_Click(object sender, EventArgs e)
        {
            this.goChrtButton_Click(this.goChrtButton, e);
        }

        private void rcHstryAcntMenuItem_Click(object sender, EventArgs e)
        {
            this.recHstryChrtButton_Click(this.recHstryChrtButton, e);
        }

        private void vwSQLAcntMenuItem_Click(object sender, EventArgs e)
        {
            this.vwSQLChrtButton_Click(this.vwSQLChrtButton, e);
        }

        private void searchForChrtTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            EventArgs ex = new EventArgs();
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                this.goChrtButton_Click(this.goChrtButton, ex);
            }
        }

        private void positionChrtTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            EventArgs ex = new EventArgs();
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up)
            {
                this.ChrtPnlNavButtons(this.movePreviousChrtButton, ex);
            }
            else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Down)
            {
                this.ChrtPnlNavButtons(this.moveNextChrtButton, ex);
            }
        }

        private void isPrntAccntsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.shdObeyChrtEvts() == false
             || beenToIsprntfunc == true)
            {
                beenToIsprntfunc = false;
                return;
            }
            beenToIsprntfunc = true;
            if (this.addChrt == false && this.editChrt == false)
            {
                this.isPrntAccntsCheckBox.Checked = !this.isPrntAccntsCheckBox.Checked;
            }
        }

        private void isContraCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.shdObeyChrtEvts() == false
             || beenToIsContra == true)
            {
                beenToIsContra = false;
                return;
            }
            beenToIsContra = true;
            if (this.addChrt == false && this.editChrt == false)
            {
                this.isContraCheckBox.Checked = !this.isContraCheckBox.Checked;
            }
        }

        private void isRetEarnsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.shdObeyChrtEvts() == false
             || beenToIsRetEarns == true)
            {
                beenToIsRetEarns = false;
                return;
            }
            beenToIsRetEarns = true;
            if (this.addChrt == false && this.editChrt == false)
            {
                this.isRetEarnsCheckBox.Checked = !this.isRetEarnsCheckBox.Checked;
            }
        }

        private void isNetIncmCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.shdObeyChrtEvts() == false
             || beenToIsNetInc == true)
            {
                beenToIsNetInc = false;
                return;
            }
            beenToIsNetInc = true;
            if (this.addChrt == false && this.editChrt == false)
            {
                this.isNetIncmCheckBox.Checked = !this.isNetIncmCheckBox.Checked;
            }
        }

        private void isEnabledAccntsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.shdObeyChrtEvts() == false
             || beenToIsEnabled == true)
            {
                beenToIsEnabled = false;
                return;
            }
            beenToIsEnabled = true;
            if (this.addChrt == false && this.editChrt == false)
            {
                this.isEnabledAccntsCheckBox.Checked = !this.isEnabledAccntsCheckBox.Checked;
            }
        }
        #endregion

        public DataSet get_One_Chrt_Det(int chrtID)
        {
            string strSql = "";
            strSql = @"SELECT accnt_id, accnt_num, accnt_name, accnt_desc, is_contra, prnt_accnt_id, 
       CASE WHEN balance_date ='' THEN '' ELSE to_char(to_timestamp(balance_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') END bsldte, 
created_by, creation_date, last_update_by, last_update_date, 
       org_id, accnt_type, is_prnt_accnt, debit_balance, credit_balance, 
       is_enabled, net_balance, is_retained_earnings, is_net_income, 
       accnt_typ_id, report_line_no, has_sub_ledgers, control_account_id, crncy_id, is_suspens_accnt, account_clsfctn, accnt_seg1_val_id, 
       accnt_seg2_val_id, accnt_seg3_val_id, accnt_seg4_val_id, accnt_seg5_val_id, 
       accnt_seg6_val_id, accnt_seg7_val_id, accnt_seg8_val_id, accnt_seg9_val_id, 
       accnt_seg10_val_id, mapped_grp_accnt_id " +
              "FROM accb.accb_chart_of_accnts a " +
          "WHERE (a.accnt_id = " + chrtID + ") ORDER BY a.accnt_typ_id, a.report_line_no, a.accnt_num";
            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            return dtst;
        }
        public DataSet get_Bals_Prnt_Accnts(int prntAccntID)
        {

            string strSql = "";
            strSql = "WITH RECURSIVE subaccnt(accnt_id, prnt_accnt_id, accnt_num, accnt_name, debit_balance, credit_balance, net_balance, depth, path, cycle, space) AS " +
         "( " +
         "   SELECT e.accnt_id, e.prnt_accnt_id, e.accnt_num, e.accnt_name, e.debit_balance, e.credit_balance, e.net_balance, 1, ARRAY[e.accnt_id], false, '' FROM accb.accb_chart_of_accnts e WHERE e.prnt_accnt_id = " + prntAccntID +
         "   UNION ALL " +
          "  SELECT d.accnt_id, d.prnt_accnt_id, d.accnt_num, d.accnt_name, d.debit_balance, d.credit_balance, d.net_balance, sd.depth + 1, " +
          "        path || d.accnt_id, " +
          "        d.accnt_id = ANY(path), space || '.' " +
            " FROM " +
              "    accb.accb_chart_of_accnts AS d, " +
                "   subaccnt AS sd " +
                  "  WHERE d.prnt_accnt_id = sd.accnt_id AND NOT cycle " +
         ") " +
         "SELECT SUM(debit_balance), SUM(credit_balance), SUM(net_balance) " +
         "FROM subaccnt " +
         "WHERE accnt_num ilike '%'";
            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            return dtst;
        }

        public DataSet get_CurrBals_Prnt_Accnts(int prntAccntID, int CurrID)
        {
            string dtestr = cmnCde.getDB_Date_time();
            string strSql = "";
            strSql = @"select SUM(g.dbt_bal), SUM(g.crdt_bal), SUM(g.net_balance)
      from accb.accb_accnt_crncy_daily_bals g, accb.accb_chart_of_accnts h,
      (SELECT  MAX(a.as_at_date) dte1, a.accnt_id accnt1
          from accb.accb_accnt_crncy_daily_bals a, accb.accb_chart_of_accnts b 
          where a.accnt_id=b.accnt_id 
          and a.crncy_id = " + CurrID +
                @" and b.prnt_accnt_id = " + prntAccntID + @"
          and to_timestamp(a.as_at_date,'YYYY-MM-DD') <= to_timestamp('" +
                dtestr.Substring(0, 10) + @"','YYYY-MM-DD') 
          GROUP BY a.accnt_id) tbl1           
          where g.accnt_id=h.accnt_id 
          and g.crncy_id = " + CurrID +
                @" and h.prnt_accnt_id = " + prntAccntID + @"
          and g.as_at_date =tbl1.dte1 
          and g.accnt_id =tbl1.accnt1";
            //      strSql = @"select  SUM(a.dbt_bal), SUM(a.crdt_bal), SUM(a.net_balance), to_timestamp(a.as_at_date,'YYYY-MM-DD')
            //          from accb.accb_accnt_crncy_daily_bals a, accb.accb_chart_of_accnts b 
            //          where a.accnt_id=b.accnt_id and a.crncy_id = " + CurrID + 
            //          " and b.prnt_accnt_id = " + prntAccntID + @"
            //          and to_timestamp(a.as_at_date,'YYYY-MM-DD') <= to_timestamp('" +
            //          dtestr.Substring(0, 10) + @"','YYYY-MM-DD') GROUP BY to_timestamp(a.as_at_date,'YYYY-MM-DD')
            //          ORDER BY to_timestamp(a.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0;";
            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            return dtst;
        }

        public DataSet get_CurrBals_Cntrl_Accnts(int cntrlAccntID, int CurrID)
        {
            string dtestr = cmnCde.getDB_Date_time();
            string strSql = "";
            strSql = @"select SUM(g.dbt_bal), SUM(g.crdt_bal), SUM(g.net_balance)
      from accb.accb_accnt_crncy_daily_bals g, accb.accb_chart_of_accnts h,
      (SELECT  MAX(a.as_at_date) dte1, a.accnt_id accnt1
          from accb.accb_accnt_crncy_daily_bals a, accb.accb_chart_of_accnts b 
          where a.accnt_id=b.accnt_id 
          and a.crncy_id = " + CurrID +
                @" and b.control_account_id = " + cntrlAccntID + @"
          and to_timestamp(a.as_at_date,'YYYY-MM-DD') <= to_timestamp('" +
                dtestr.Substring(0, 10) + @"','YYYY-MM-DD') 
          GROUP BY a.accnt_id) tbl1           
          where g.accnt_id=h.accnt_id 
          and g.crncy_id = " + CurrID +
                @" and h.control_account_id = " + cntrlAccntID + @"
          and g.as_at_date =tbl1.dte1 
          and g.accnt_id =tbl1.accnt1";
            //      strSql = @"select  SUM(a.dbt_bal), SUM(a.crdt_bal), SUM(a.net_balance), to_timestamp(a.as_at_date,'YYYY-MM-DD')
            //          from accb.accb_accnt_crncy_daily_bals a, accb.accb_chart_of_accnts b 
            //          where a.accnt_id=b.accnt_id and a.crncy_id = " + CurrID + 
            //          " and b.prnt_accnt_id = " + prntAccntID + @"
            //          and to_timestamp(a.as_at_date,'YYYY-MM-DD') <= to_timestamp('" +
            //          dtestr.Substring(0, 10) + @"','YYYY-MM-DD') GROUP BY to_timestamp(a.as_at_date,'YYYY-MM-DD')
            //          ORDER BY to_timestamp(a.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0;";
            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            return dtst;
        }

        public DataSet get_CurrBals_Accnts(int accntID)
        {
            string dtestr = cmnCde.getDB_Date_time();
            string strSql = "";
            strSql = @"select  a.dbt_bal, a.crdt_bal, a.net_balance, to_char(to_timestamp(a.as_at_date,'YYYY-MM-DD'),'DD-Mon-YYYY') 
          from accb.accb_accnt_crncy_daily_bals a
          where a.accnt_id= " + accntID +
                @" and to_timestamp(a.as_at_date,'YYYY-MM-DD') <= to_timestamp('" + dtestr.Substring(0, 10) + @"','YYYY-MM-DD') 
          ORDER BY to_timestamp(a.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0;";
            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            return dtst;
        }
        public DataSet get_Basic_ChrtDet(string searchWord, string searchIn,
          Int64 offset, int limit_size, int orgID, int lovID)
        {
            searchWord = searchWord.Replace(".", "%");
            string lovQry = cmnCde.getGnrlRecNm("gst.gen_stp_lov_names", "value_list_id", "sqlquery_if_dyn", lovID);
            lovQry = "(" + lovQry.Replace("{:prsn_id}", cmnCde.Prsn_id.ToString()) + ") xxtbl1 ";
            string strSql = "";
            string whereCls = " and (accnt_num ilike '" + searchWord.Replace("'", "''") +
           "' or accnt_name ilike '" + searchWord.Replace("'", "''") +
           "' or accnt_num||'%.%'||accnt_name ilike '" + searchWord.Replace("'", "''") +
           "')";
            if (lovQry != "")
            {
                whereCls = whereCls + " and accnt_id IN (select xxtbl1.a::integer from " + lovQry + ")";
            }
            string subSql = @"SELECT accnt_id,accnt_num,accnt_name,space||accnt_num||'.'||accnt_name account_number_name, is_prnt_accnt, accnt_type,accnt_typ_id, prnt_accnt_id, control_account_id, depth, path, cycle 
      FROM suborg WHERE 1=1 " + whereCls + @"  
      ORDER BY accnt_typ_id, path";

            if (searchIn != "Parent Account Details"
              || searchWord.Length <= 3)
            {
                strSql = @"WITH RECURSIVE suborg(accnt_id, accnt_num, accnt_name, is_prnt_accnt, accnt_type, accnt_typ_id, prnt_accnt_id, control_account_id, depth, path, cycle, space) AS 
      ( 
      SELECT a.accnt_id, a.accnt_num, a.accnt_name, a.is_prnt_accnt, a.accnt_type,a.accnt_typ_id, a.prnt_accnt_id, a.control_account_id, 1, ARRAY[a.accnt_num||'']::character varying[], false, '' opad 
      FROM accb.accb_chart_of_accnts a 
        WHERE ((CASE WHEN a.prnt_accnt_id<=0 THEN a.control_account_id ELSE a.prnt_accnt_id END)=-1 AND (a.org_id = " + orgID + @")) 
      UNION ALL        
      SELECT a.accnt_id, a.accnt_num, a.accnt_name, a.is_prnt_accnt, a.accnt_type,a.accnt_typ_id, a.prnt_accnt_id, a.control_account_id, sd.depth + 1, 
      path || a.accnt_num, 
      a.accnt_num = ANY(path), space || '      '
      FROM 
      accb.accb_chart_of_accnts a, suborg AS sd 
      WHERE (((CASE WHEN a.prnt_accnt_id<=0 THEN a.control_account_id ELSE a.prnt_accnt_id END)=sd.accnt_id AND NOT cycle) 
       AND (a.org_id = " + orgID + @"))) 
       " + subSql + " LIMIT " + limit_size +
                    " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }
            else
            {
                subSql = @"SELECT accnt_id,accnt_num,accnt_name,space||accnt_num||'.'||accnt_name account_number_name, is_prnt_accnt, accnt_type,accnt_typ_id, prnt_accnt_id, control_account_id, depth, path, cycle 
      FROM suborg WHERE 1=1 ORDER BY accnt_typ_id, path";

                strSql = @"WITH RECURSIVE suborg(accnt_id, accnt_num, accnt_name, is_prnt_accnt, accnt_type, accnt_typ_id, prnt_accnt_id, control_account_id, depth, path, cycle, space) AS 
      ( 
      SELECT a.accnt_id, a.accnt_num, a.accnt_name, a.is_prnt_accnt, a.accnt_type,a.accnt_typ_id, a.prnt_accnt_id, a.control_account_id, 1, ARRAY[a.accnt_num||'']::character varying[], false, '' opad 
      FROM accb.accb_chart_of_accnts a 
        WHERE ((a.accnt_name ilike '" + searchWord.Replace("'", "''") +
             @"' or a.accnt_num ilike '" + searchWord.Replace("'", "''") +
             @"') AND (a.org_id = " + orgID + @")) 
      UNION ALL        
      SELECT a.accnt_id, a.accnt_num, a.accnt_name, a.is_prnt_accnt, a.accnt_type,a.accnt_typ_id, a.prnt_accnt_id, a.control_account_id, sd.depth + 1, 
      path || a.accnt_num, 
      a.accnt_num = ANY(path), space || '      '
      FROM 
      accb.accb_chart_of_accnts a, suborg AS sd 
      WHERE (((CASE WHEN a.prnt_accnt_id<=0 THEN a.control_account_id ELSE a.prnt_accnt_id END)=sd.accnt_id AND NOT cycle) 
       AND (a.org_id = " + orgID + @"))) 
       " + subSql + " LIMIT " + limit_size +
                  " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }

            this.chrt_SQL = strSql;
            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            return dtst;
        }

        public long get_Total_Chrts(string searchWord, string searchIn, int orgID, int lovID)
        {
            searchWord = searchWord.Replace(".", "%");
            string lovQry = cmnCde.getGnrlRecNm("gst.gen_stp_lov_names", "value_list_id", "sqlquery_if_dyn", lovID);
            lovQry = "(" + lovQry.Replace("{:prsn_id}", cmnCde.Prsn_id.ToString()) + ") xxtbl1 ";
            string strSql = "";
            string whereCls = " and (accnt_num ilike '" + searchWord.Replace("'", "''") +
           "' or accnt_name ilike '" + searchWord.Replace("'", "''") +
           "' or accnt_num||'%.%'||accnt_name ilike '" + searchWord.Replace("'", "''") +
           "')";
            if (lovQry != "")
            {
                whereCls = whereCls + " and accnt_id IN (select xxtbl1.a::integer from " + lovQry + ")";
            }
            string subSql = @"SELECT count(1) 
      FROM suborg WHERE 1=1 " + whereCls + @"";

            if (searchIn != "Parent Account Details"
              || searchWord.Length <= 3)
            {
                strSql = @"WITH RECURSIVE suborg(accnt_id, accnt_num, accnt_name, is_prnt_accnt, accnt_type, accnt_typ_id, prnt_accnt_id, control_account_id, depth, path, cycle, space) AS 
      ( 
      SELECT a.accnt_id, a.accnt_num, a.accnt_name, a.is_prnt_accnt, a.accnt_type,a.accnt_typ_id, a.prnt_accnt_id, a.control_account_id, 1, ARRAY[a.accnt_num||'']::character varying[], false, '' opad 
      FROM accb.accb_chart_of_accnts a 
        WHERE ((CASE WHEN a.prnt_accnt_id<=0 THEN a.control_account_id ELSE a.prnt_accnt_id END)=-1 AND (a.org_id = " + orgID + @")) 
      UNION ALL        
      SELECT a.accnt_id, a.accnt_num, a.accnt_name, a.is_prnt_accnt, a.accnt_type,a.accnt_typ_id, a.prnt_accnt_id, a.control_account_id, sd.depth + 1, 
      path || a.accnt_num, 
      a.accnt_num = ANY(path), space || '      '
      FROM 
      accb.accb_chart_of_accnts a, suborg AS sd 
      WHERE (((CASE WHEN a.prnt_accnt_id<=0 THEN a.control_account_id ELSE a.prnt_accnt_id END)=sd.accnt_id AND NOT cycle) 
       AND (a.org_id = " + orgID + @"))) 
       " + subSql + "";
            }
            else
            {
                subSql = @"SELECT count(1) 
      FROM suborg WHERE 1=1";

                strSql = @"WITH RECURSIVE suborg(accnt_id, accnt_num, accnt_name, is_prnt_accnt, accnt_type, accnt_typ_id, prnt_accnt_id, control_account_id, depth, path, cycle, space) AS 
      ( 
      SELECT a.accnt_id, a.accnt_num, a.accnt_name, a.is_prnt_accnt, a.accnt_type,a.accnt_typ_id, a.prnt_accnt_id, a.control_account_id, 1, ARRAY[a.accnt_num||'']::character varying[], false, '' opad 
      FROM accb.accb_chart_of_accnts a 
        WHERE ((a.accnt_name ilike '" + searchWord.Replace("'", "''") +
             @"' or a.accnt_num ilike '" + searchWord.Replace("'", "''") +
             @"') AND (a.org_id = " + orgID + @")) 
      UNION ALL        
      SELECT a.accnt_id, a.accnt_num, a.accnt_name, a.is_prnt_accnt, a.accnt_type,a.accnt_typ_id, a.prnt_accnt_id, a.control_account_id, sd.depth + 1, 
      path || a.accnt_num, 
      a.accnt_num = ANY(path), space || '      '
      FROM 
      accb.accb_chart_of_accnts a, suborg AS sd 
      WHERE (((CASE WHEN a.prnt_accnt_id<=0 THEN a.control_account_id ELSE a.prnt_accnt_id END)=sd.accnt_id AND NOT cycle) 
       AND (a.org_id = " + orgID + @"))) 
       " + subSql + "";
            }

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

        public string get_Chrt_Rec_Hstry(int chrtID)
        {
            string strSQL = @"SELECT a.created_by, 
to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
a.last_update_by, 
to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM accb.accb_chart_of_accnts a WHERE(a.accnt_id  = " + chrtID + ")";
            string fnl_str = "";
            DataSet dtst = cmnCde.selectDataNoParams(strSQL);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                fnl_str = "CREATED BY: " + cmnCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][0].ToString())) +
                  "\r\nCREATION DATE: " + dtst.Tables[0].Rows[0][1].ToString() + "\r\nLAST UPDATE BY:" +
                  cmnCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][2].ToString())) +
                  "\r\nLAST UPDATE DATE: " + dtst.Tables[0].Rows[0][3].ToString();
                return fnl_str;
            }
            else
            {
                return "";
            }
        }
        public long get_Accnt_Tot_Trns(long accntID)
        {
            string strSql = "";
            strSql = "SELECT count(1) " +
             "FROM accb.accb_trnsctn_details a " +
             "WHERE(a.accnt_id = " + accntID + ")";
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

        public long get_Accnt_Tot_Chldrn(long accntID)
        {
            string strSql = "";
            strSql = "SELECT count(1) " +
             "FROM accb.accb_chart_of_accnts a " +
             "WHERE(a.prnt_accnt_id = " + accntID + ")";
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
        public long get_Accnt_Tot_Mappngs(long accntID)
        {
            string strSql = "";
            strSql = "SELECT count(1) " +
             "FROM org.org_segment_values a " +
             "WHERE(a.mapped_grp_accnt_id = " + accntID + ")";
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
        public string getIsParentOrHsLedger(long accntID)
        {
            string strSql = "";
            strSql = "SELECT CASE WHEN a.is_prnt_accnt='1' THEN a.is_prnt_accnt ELSE a.has_sub_ledgers END " +
             "FROM accb.accb_chart_of_accnts a " +
             "WHERE(a.accnt_id = " + accntID + " and (a.is_prnt_accnt='1' or a.has_sub_ledgers='1'))";
            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public long get_Accnt_Tot_Pymnts(long accntID)
        {
            string strSql = "";
            strSql = "SELECT count(1) " +
             "FROM pay.pay_gl_interface a " +
             "WHERE(a.accnt_id = " + accntID + ")";
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

        public long get_Accnt_Tot_PyItms(long accntID)
        {
            string strSql = "";
            strSql = "SELECT count(1) " +
             "FROM org.org_pay_items a " +
             "WHERE(a.cost_accnt_id = " + accntID + " or a.bals_accnt_id = " + accntID + ")";
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

        public DataSet get_All_Chrt_Det(int orgid)
        {
            string strSql = "";
            strSql = @"SELECT a.accnt_id, a.debit_balance , a.credit_balance , a.net_balance ,
to_char(to_timestamp(a.balance_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') bsldte " +
              "FROM accb.accb_chart_of_accnts a WHERE a.org_id = " + orgid + " ORDER BY a.accnt_typ_id, a.report_line_no, a.accnt_num";
            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            return dtst;
        }
        public void deleteAccount(long accntid, string accntNm, string accntNo)
        {
            cmnCde.Extra_Adt_Trl_Info = "Account Name = " + accntNm + " Account No. = " + accntNo;
            string delSql = "DELETE FROM accb.accb_chart_of_accnts WHERE (accnt_id = " + accntid + ")";
            cmnCde.deleteDataNoParams(delSql);
        }

        public void createChrt(int orgid, string accntnum, string accntname,
          string accntdesc, bool isContra, int prntAccntID, string accntTyp,
          bool isparent, bool isenbld, bool isretearngs, bool isnetincome, int rpt_ln,
          bool hasSbLdgrs, int cntrlAccntID, int currID, bool isSuspns, string accClsftn,
          int accntSegmnt1, int accntSegmnt2, int accntSegmnt3, int accntSegmnt4, int accntSegmnt5,
          int accntSegmnt6, int accntSegmnt7, int accntSegmnt8, int accntSegmnt9, int accntSegmnt10,
          int mappedAcntID)
        {
            string dateStr = cmnCde.getDB_Date_time();
            if (isretearngs == true)
            {
                this.clearChrtRetEarns(orgid);
            }
            if (isnetincome == true)
            {
                this.clearChrtNetIncome(orgid);
            }
            if (isSuspns == true)
            {
                this.clearChrtSuspns(orgid);
            }
            string insSQL = "INSERT INTO accb.accb_chart_of_accnts(" +
                     "accnt_num, accnt_name, accnt_desc, is_contra, " +
                     "prnt_accnt_id, balance_date, created_by, creation_date, last_update_by, " +
                     "last_update_date, org_id, accnt_type, is_prnt_accnt, debit_balance, " +
                     "credit_balance, is_enabled, net_balance, is_retained_earnings, " +
                     "is_net_income, accnt_typ_id, report_line_no, has_sub_ledgers, " +
                     @"control_account_id, crncy_id, is_suspens_accnt,account_clsfctn, accnt_seg1_val_id, 
       accnt_seg2_val_id, accnt_seg3_val_id, accnt_seg4_val_id, accnt_seg5_val_id, 
       accnt_seg6_val_id, accnt_seg7_val_id, accnt_seg8_val_id, accnt_seg9_val_id, 
       accnt_seg10_val_id, mapped_grp_accnt_id)" +
             "VALUES ('" + accntnum.Replace("'", "''") + "', '" + accntname.Replace("'", "''") +
             "', '" + accntdesc.Replace("'", "''") + "', '" + cmnCde.cnvrtBoolToBitStr(isContra) +
             "', " + prntAccntID + ", '" + dateStr + "', " + cmnCde.User_id + ", '" + dateStr +
                     "', " + cmnCde.User_id + ", '" + dateStr + "', " +
                     orgid + ", '" + accntTyp.Replace("'", "''") +
             "', '" + cmnCde.cnvrtBoolToBitStr(isparent) + "', 0, 0, '" +
             cmnCde.cnvrtBoolToBitStr(isenbld) + "', 0, '" +
             cmnCde.cnvrtBoolToBitStr(isretearngs) + "', '" +
             cmnCde.cnvrtBoolToBitStr(isnetincome) + "', " +
             this.getAcctTypID(accntTyp) +
             ", " + rpt_ln + ", '" + cmnCde.cnvrtBoolToBitStr(hasSbLdgrs) +
             "', " + cntrlAccntID + ", " + currID + ", '" + cmnCde.cnvrtBoolToBitStr(isSuspns) +
             "','" + accClsftn.Replace("'", "''") + "', " + accntSegmnt1 + ", " + accntSegmnt2 + ", " + accntSegmnt3 +
             ", " + accntSegmnt4 + ", " + accntSegmnt5 + ", " + accntSegmnt6 + ", " + accntSegmnt7 + ", " + accntSegmnt8 +
             ", " + accntSegmnt9 + ", " + accntSegmnt10 + ", " + mappedAcntID + ")";
            cmnCde.insertDataNoParams(insSQL);
        }

        public void updateChrtDet(int orgid, int accntid, string accntnum, string accntname,
          string accntdesc, bool isContra, int prntAccntID, string accntTyp,
          bool isparent, bool isenbld, bool isretearngs, bool isnetincome, int rpt_ln,
          bool hasSbLdgrs, int cntrlAccntID, int currID, bool isSuspns, string accClsftn,
          int accntSegmnt1, int accntSegmnt2, int accntSegmnt3, int accntSegmnt4, int accntSegmnt5,
          int accntSegmnt6, int accntSegmnt7, int accntSegmnt8, int accntSegmnt9, int accntSegmnt10,
          int mappedAcntID)
        {
            cmnCde.Extra_Adt_Trl_Info = "";
            string dateStr = cmnCde.getDB_Date_time();
            if (isretearngs == true)
            {
                this.clearChrtRetEarns(orgid);
            }
            if (isnetincome == true)
            {
                this.clearChrtNetIncome(orgid);
            }
            if (isSuspns == true)
            {
                this.clearChrtSuspns(orgid);
            }

            string updtSQL = "UPDATE accb.accb_chart_of_accnts " +
            "SET accnt_num='" + accntnum.Replace("'", "''") + "', accnt_name='" + accntname.Replace("'", "''") +
            "', accnt_desc='" + accntdesc.Replace("'", "''") + "', is_contra='" + cmnCde.cnvrtBoolToBitStr(isContra) + "', " +
                "prnt_accnt_id=" + prntAccntID + ", " +
                "last_update_by=" + cmnCde.User_id + ", last_update_date='" + dateStr +
                "', accnt_type='" + accntTyp.Replace("'", "''") + "', " +
                "is_prnt_accnt='" + cmnCde.cnvrtBoolToBitStr(isparent) +
                "', is_enabled='" + cmnCde.cnvrtBoolToBitStr(isenbld) + "', " +
                "is_retained_earnings='" + cmnCde.cnvrtBoolToBitStr(isretearngs) +
                "', is_net_income='" + cmnCde.cnvrtBoolToBitStr(isnetincome) +
                "', accnt_typ_id = " + this.getAcctTypID(accntTyp) +
                ", report_line_no = " + rpt_ln +
                ", has_sub_ledgers = '" + cmnCde.cnvrtBoolToBitStr(hasSbLdgrs) +
                "', control_account_id = " + cntrlAccntID +
                ", crncy_id = " + currID +
                ", is_suspens_accnt = '" + cmnCde.cnvrtBoolToBitStr(isSuspns) +
                "', account_clsfctn = '" + accClsftn.Replace("'", "''") +
                "', accnt_seg1_val_id=" + accntSegmnt1 +
                ", accnt_seg2_val_id=" + accntSegmnt2 +
                ", accnt_seg3_val_id =" + accntSegmnt3 +
                ", accnt_seg4_val_id =" + accntSegmnt4 +
                ", accnt_seg5_val_id =" + accntSegmnt5 +
                ", accnt_seg6_val_id =" + accntSegmnt6 +
                ", accnt_seg7_val_id =" + accntSegmnt7 +
                ", accnt_seg8_val_id =" + accntSegmnt8 +
                ",  accnt_seg9_val_id =" + accntSegmnt9 +
                ", accnt_seg10_val_id =" + accntSegmnt10 +
                ", mapped_grp_accnt_id =" + mappedAcntID +
                " WHERE accnt_id = " + accntid;
            cmnCde.updateDataNoParams(updtSQL);
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
        public void clearChrtRetEarns(int orgid)
        {
            cmnCde.Extra_Adt_Trl_Info = "";
            string updtSQL = "UPDATE accb.accb_chart_of_accnts " +
            "SET is_retained_earnings='0' WHERE org_id = " + orgid;
            cmnCde.updateDataNoParams(updtSQL);
        }

        public void clearChrtNetIncome(int orgid)
        {
            cmnCde.Extra_Adt_Trl_Info = "";
            string updtSQL = "UPDATE accb.accb_chart_of_accnts " +
            "SET is_net_income='0' WHERE org_id = " + orgid;
            cmnCde.updateDataNoParams(updtSQL);
        }

        public void clearChrtSuspns(int orgid)
        {
            cmnCde.Extra_Adt_Trl_Info = "";
            string updtSQL = "UPDATE accb.accb_chart_of_accnts " +
            "SET is_suspens_accnt='0' WHERE org_id = " + orgid;
            cmnCde.updateDataNoParams(updtSQL);
        }

        public void updtAccntPrntID(int accntID, int prntID)
        {
            cmnCde.Extra_Adt_Trl_Info = "";
            string dateStr = cmnCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_chart_of_accnts SET prnt_accnt_id = " + prntID +
                              ", last_update_by = " + cmnCde.User_id + ", " +
                              "last_update_date = '" + dateStr + "' " +
              "WHERE (accnt_id = " + accntID + ")";
            cmnCde.updateDataNoParams(updtSQL);
        }

        public void updtAccntCurrID(int accntID, int crncyID)
        {
            cmnCde.Extra_Adt_Trl_Info = "";
            string dateStr = cmnCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_chart_of_accnts SET crncy_id = " + crncyID +
                              ", last_update_by = " + cmnCde.User_id + ", " +
                              "last_update_date = '" + dateStr + "' " +
              "WHERE (accnt_id = " + accntID + ")";
            cmnCde.updateDataNoParams(updtSQL);
        }

        public void updtOrgAccntCurrID(int orgID, int crncyID)
        {
            cmnCde.Extra_Adt_Trl_Info = "";
            string dateStr = cmnCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_chart_of_accnts SET crncy_id = " + crncyID +
                              ", last_update_by = " + cmnCde.User_id + ", " +
                              "last_update_date = '" + dateStr + "' " +
              "WHERE (org_id = " + orgID + " and crncy_id<=0)";
            cmnCde.updateDataNoParams(updtSQL);
            updtSQL = @"UPDATE accb.accb_trnsctn_details SET dbt_or_crdt='C' WHERE dbt_or_crdt='U' and dbt_amount=0 and crdt_amount !=0;
UPDATE accb.accb_trnsctn_details SET dbt_or_crdt='D' WHERE dbt_or_crdt='U' and dbt_amount!=0 and crdt_amount =0;";
            cmnCde.updateDataNoParams(updtSQL);
            updtSQL = @"UPDATE accb.accb_trnsctn_details SET entered_amnt=dbt_amount, accnt_crncy_amnt=dbt_amount WHERE dbt_amount!=0 and crdt_amount =0 and entered_amnt=0 and accnt_crncy_amnt=0;
UPDATE accb.accb_trnsctn_details SET entered_amnt=crdt_amount, accnt_crncy_amnt=crdt_amount WHERE dbt_amount=0 and crdt_amount!=0 and entered_amnt=0 and accnt_crncy_amnt=0";
            cmnCde.updateDataNoParams(updtSQL);
            updtSQL = @"UPDATE accb.accb_trnsctn_details SET entered_amt_crncy_id=func_cur_id WHERE entered_amt_crncy_id=-1;
UPDATE accb.accb_trnsctn_details SET accnt_crncy_id=func_cur_id WHERE accnt_crncy_id=-1";
            cmnCde.updateDataNoParams(updtSQL);

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
        public DataSet get_One_RptClsfctns(int limit_size, int offset)
        {
            string strSql = @"SELECT b.accnt_num, b.accnt_name, a.maj_rpt_ctgry, a.min_rpt_ctgry 
  FROM accb.accb_account_clsfctns a, accb.accb_chart_of_accnts b WHERE(a.account_id= b.accnt_id)  
ORDER BY b.accnt_num, a.maj_rpt_ctgry, a.min_rpt_ctgry LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            return dtst;
        }
        public DataSet get_SgmntVal_RptClsfctns(int sgmntValid)
        {
            string strSql = @"SELECT account_clsfctn_id, maj_rpt_ctgry, min_rpt_ctgry, 
       created_by, creation_date, last_update_by, last_update_date
  FROM org.org_account_clsfctns a WHERE(a.account_id = " + sgmntValid + ") ORDER BY 1";

            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            return dtst;
        }
        public long get_RptClsfctnID(string majCtgrName, string minCtgrName, int accntID)
        {
            string strSql = @"SELECT account_clsfctn_id from accb.accb_account_clsfctns where account_id=" + accntID +
              " and lower(maj_rpt_ctgry)='" + majCtgrName.Replace("'", "''").ToLower() +
              "' and lower(min_rpt_ctgry)='" + minCtgrName.Replace("'", "''").ToLower() + "'";

            DataSet dtst = cmnCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            //Global.taxFrm.rec_SQL = strSql;
            return -1;
        }

        public long getNewRptClsfLnID()
        {
            string strSql = "select nextval('accb.accb_account_clsfctns_account_clsfctn_id_seq')";
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
            string insSQL = @"INSERT INTO accb.accb_account_clsfctns(
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
            string updtSQL = "UPDATE accb.accb_account_clsfctns SET " +
                  "maj_rpt_ctgry='" + majCtgrName.Replace("'", "''") +
                  "', min_rpt_ctgry='" + minCtgrName.Replace("'", "''") +
                  "',account_id=" + accntID +
                  ", last_update_by = " + cmnCde.User_id + ", " +
                  "last_update_date = '" + dateStr +
                  "' WHERE (account_clsfctn_id =" + clsfctnID + ")";
            cmnCde.updateDataNoParams(updtSQL);
        }

        private void isSuspensCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.shdObeyChrtEvts() == false
         || beenToIsEnabled == true)
            {
                beenToIsEnabled = false;
                return;
            }
            beenToIsEnabled = true;
            if (this.addChrt == false && this.editChrt == false)
            {
                this.isSuspensCheckBox.Checked = !this.isSuspensCheckBox.Checked;
            }

        }

        private void hasSubldgrCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.shdObeyChrtEvts() == false
             || beenToIsEnabled == true)
            {
                beenToIsEnabled = false;
                return;
            }
            beenToIsEnabled = true;
            if (this.addChrt == false && this.editChrt == false)
            {
                this.hasSubldgrCheckBox.Checked = !this.hasSubldgrCheckBox.Checked;
            }
        }

        private void resetChrtButton_Click(object sender, EventArgs e)
        {
            this.searchInChrtComboBox.SelectedIndex = 0;
            this.searchForChrtTextBox.Text = "%";
            this.dsplySizeChrtComboBox.Text = cmnCde.get_CurPlcy_Mx_Dsply_Recs().ToString();
            this.chrt_cur_indx = 0;
            this.rfrshChrtButton_Click(this.rfrshChrtButton, e);
        }

        private void cntrlAccntButton_Click(object sender, EventArgs e)
        {
            if (this.addChrt == false && this.editChrt == false)
            {
                cmnCde.showMsg("Must be in ADD/EDIT mode First!", 0);
                return;
            }
            if (this.accntTypeComboBox.Text == "")
            {
                cmnCde.showMsg("Please select an Account Type First!", 0);
                return;
            }

            string[] selVals = new string[1];
            selVals[0] = this.cntrlAccntIDTextBox.Text;
            DialogResult dgRes = cmnCde.showPssblValDiag(
             cmnCde.getLovID("Control Accounts"), ref selVals,
             true, false, cmnCde.Org_id,
             this.accntTypeComboBox.Text.Substring(0, 2).Trim(), "");
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.cntrlAccntIDTextBox.Text = selVals[i];
                    this.cntrlAccntTextBox.Text = cmnCde.getAccntName(int.Parse(selVals[i]));
                }
            }
            if (int.Parse(this.accntIDTextBox.Text) > 0)
            {
                this.updtAccntPrntID(int.Parse(this.accntIDTextBox.Text),
                  int.Parse(this.cntrlAccntIDTextBox.Text));
            }
        }

        private void accntCurrButton_Click(object sender, EventArgs e)
        {

            if (this.addChrt == false && this.editChrt == false)
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
                    this.accntCrncyNmTextBox.Text = cmnCde.getPssblValNm(selVals[i]) +
                      " - " + cmnCde.getPssblValDesc(selVals[i]);
                }
            }
            //if (int.Parse(this.accntIDTextBox.Text) > 0)
            //{
            //  Global.updtAccntCurrID(int.Parse(this.accntIDTextBox.Text),
            //    int.Parse(this.accntCurrIDTextBox.Text));
            //}
        }

        private void mappedAccntButton_Click(object sender, EventArgs e)
        {
            this.mappedAcntLOVSearch();
        }

        private void mappedAcntLOVSearch()
        {
            if (!this.mappedAccntTextBox.Text.Contains("%"))
            {
                this.mappedAccntTextBox.Text = "%" + this.mappedAccntTextBox.Text.Replace(" ", "%") + "%";
                this.mappedAccntIDTextBox.Text = "-1";
            }
            int grpOrgID = cmnCde.getGrpOrgID();
            string[] selVals = new string[1];
            selVals[0] = this.mappedAccntIDTextBox.Text;
            DialogResult dgRes = cmnCde.showPssblValDiag(
              cmnCde.getLovID("Transaction Accounts"), ref selVals,
              true, true, grpOrgID,
             this.parentAccntTextBox.Text, "Both", true);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.mappedAccntIDTextBox.Text = selVals[i];
                    this.mappedAccntTextBox.Text = cmnCde.getAccntNum(int.Parse(selVals[i]))
                    + "." + cmnCde.getAccntName(int.Parse(selVals[i]));
                }
            }
        }

        private void segmentsButton_Click(object sender, EventArgs e)
        {
            if (this.editAccounts && this.editChrtButton.Text == "EDIT")
            {
                this.editChrtButton.PerformClick();
            }
            if (int.Parse(this.accntIDTextBox.Text) <= 0 && this.addChrt == false)
            {
                cmnCde.showMsg("Please select an Account First!", 0);
                return;
            }
            acntSegmntsDiag nwDiag = new acntSegmntsDiag();
            nwDiag.accountID = int.Parse(this.accntIDTextBox.Text);
            nwDiag.canEdit = this.editAccounts;
            nwDiag.cmnCde = cmnCde;
            nwDiag.allwNtrlAcntEdit = this.editAccounts;
            if (this.editAccounts == true)
            {
                if (this.netBalNumericUpDown.Value != 0
                  || this.crdtBalNumericUpDown.Value != 0
                  || this.dbtBalNumericUpDown.Value != 0)
                {
                    nwDiag.allwNtrlAcntEdit = false;
                }
            }
            nwDiag.accntSgmnt1 = int.Parse(this.accntSgmnt1TextBox.Text);
            nwDiag.accntSgmnt2 = int.Parse(this.accntSgmnt2TextBox.Text);
            nwDiag.accntSgmnt3 = int.Parse(this.accntSgmnt3TextBox.Text);
            nwDiag.accntSgmnt4 = int.Parse(this.accntSgmnt4TextBox.Text);
            nwDiag.accntSgmnt5 = int.Parse(this.accntSgmnt5TextBox.Text);
            nwDiag.accntSgmnt6 = int.Parse(this.accntSgmnt6TextBox.Text);
            nwDiag.accntSgmnt7 = int.Parse(this.accntSgmnt7TextBox.Text);
            nwDiag.accntSgmnt8 = int.Parse(this.accntSgmnt8TextBox.Text);
            nwDiag.accntSgmnt9 = int.Parse(this.accntSgmnt9TextBox.Text);
            nwDiag.accntSgmnt10 = int.Parse(this.accntSgmnt10TextBox.Text);

            if (nwDiag.ShowDialog() == DialogResult.OK)
            {
                this.accntNumTextBox.Text = nwDiag.nwAcctNum;
                this.accntNameTextBox.Text = nwDiag.nwAcctName;

                this.accntSgmnt1TextBox.Text = nwDiag.accntSgmnt1.ToString();
                this.accntSgmnt2TextBox.Text = nwDiag.accntSgmnt2.ToString();
                this.accntSgmnt3TextBox.Text = nwDiag.accntSgmnt3.ToString();
                this.accntSgmnt4TextBox.Text = nwDiag.accntSgmnt4.ToString();
                this.accntSgmnt5TextBox.Text = nwDiag.accntSgmnt5.ToString();
                this.accntSgmnt6TextBox.Text = nwDiag.accntSgmnt6.ToString();
                this.accntSgmnt7TextBox.Text = nwDiag.accntSgmnt7.ToString();
                this.accntSgmnt8TextBox.Text = nwDiag.accntSgmnt8.ToString();
                this.accntSgmnt9TextBox.Text = nwDiag.accntSgmnt9.ToString();
                this.accntSgmnt10TextBox.Text = nwDiag.accntSgmnt10.ToString();

                if (nwDiag.allwNtrlAcntEdit == true)
                {
                    //Get Natural Account Details and Change Corresponding Fields
                    DataSet dtst = this.get_One_SgmntValDet(nwDiag.ntrlAcntSgmtVal);
                    for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
                    {
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

                        /*this.parentAccntIDTextBox.Text = dtst.Tables[0].Rows[i][7].ToString();
                        this.parentAccntTextBox.Text = cmnCde.getSegmentVal(int.Parse(this.prntValIDTextBox.Text)) + "." +
                                cmnCde.getSegmentValDesc(int.Parse(this.prntValIDTextBox.Text));*/
                        this.isEnabledAccntsCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][6].ToString());

                        this.isPrntAccntsCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][15].ToString());
                        this.isRetEarnsCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][16].ToString());
                        this.isNetIncmCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][17].ToString());
                        this.isContraCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][13].ToString());
                        this.rptLnNoUpDown.Value = Decimal.Parse(dtst.Tables[0].Rows[i][19].ToString());
                        this.hasSubldgrCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][20].ToString());
                        this.isSuspensCheckBox.Checked = cmnCde.cnvrtBitStrToBool(dtst.Tables[0].Rows[i][23].ToString());
                        this.cntrlAccntIDTextBox.Text = dtst.Tables[0].Rows[i][21].ToString();
                        this.cntrlAccntTextBox.Text = cmnCde.getSegmentVal(int.Parse(dtst.Tables[0].Rows[i][21].ToString())) + "." +
                                cmnCde.getSegmentValDesc(int.Parse(dtst.Tables[0].Rows[i][21].ToString()));
                        this.accntCurrIDTextBox.Text = dtst.Tables[0].Rows[i][22].ToString();
                        this.accntCrncyNmTextBox.Text = cmnCde.getPssblValNm(int.Parse(dtst.Tables[0].Rows[i][22].ToString()));
                        this.mappedAccntIDTextBox.Text = dtst.Tables[0].Rows[i][25].ToString();
                        this.accClsfctnComboBox.SelectedItem = dtst.Tables[0].Rows[i][24].ToString();
                        this.mappedAccntTextBox.Text = cmnCde.getAccntNum(int.Parse(dtst.Tables[0].Rows[i][25].ToString()))
                            + "." + cmnCde.getAccntName(int.Parse(dtst.Tables[0].Rows[i][25].ToString()));
                        this.parntAccntButton.PerformClick();
                    }
                }
            }
        }

        private void rprtClsfctnsButton_Click(object sender, EventArgs e)
        {
            if (this.editAccounts && this.editChrtButton.Text == "EDIT")
            {
                this.editChrtButton.PerformClick();
            }
            if (int.Parse(this.accntIDTextBox.Text) <= 0 && this.addChrt == false)
            {
                cmnCde.showMsg("Please select an Account First!", 0);
                return;
            }
            long accntID = long.Parse(this.accntIDTextBox.Text);
            cmnCde.showAcntClsfctnsDiag(ref accntID, this.editChrt, cmnCde);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (this.isReadOnly == true)
            {
                cmnCde.showMsg("Must be ADD/EDIT mode First!", 0);
                return;
            }
            if (this.saveChrtButton.Enabled == true)
            {
                isClosing = true;
                this.saveChrtButton.PerformClick();
                isClosing = false;
            }
            if (this.errorOcrd == true)
            {
                return;
            }
            if (this.accntsChrtListView.Items.Count > 0 && this.accntsChrtListView.CheckedItems.Count <= 0)
            {
                this.accntsChrtListView.Items[0].Checked = true;
            }
            if (this.accntsChrtListView.CheckedItems.Count > 0)
            {
                //this.idTextBox.Text = this.cstSplrListView.CheckedItems[0].SubItems[2].Text;
            }
            else
            {
                this.accntIDTextBox.Text = "-1";
            }
            if (int.Parse(this.accntIDTextBox.Text) <= 0 && this.mustSelctSth == true)
            {
                cmnCde.showMsg("Must Select An Account First!", 0);
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void accntsChrtListView_DoubleClick(object sender, EventArgs e)
        {
            this.accntsChrtListView.SelectedItems[0].Checked = true;
            if (this.isReadOnly)
            {
                return;
            }
            this.okButton_Click(this.okButton, e);
        }

        private void accntsChrtListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (this.shdObeyChrtEvts() == false)
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
            for (int i = 0; i < this.accntsChrtListView.Items.Count; i++)
            {
                if (this.accntsChrtListView.Items[i].Text != this.selItemTxt)
                {
                    this.accntsChrtListView.Items[i].Checked = false;
                }
            }
            this.obey_evnts = true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void searchForChrtTextBox_Click(object sender, EventArgs e)
        {
            this.searchForChrtTextBox.SelectAll();
        }
    }
}

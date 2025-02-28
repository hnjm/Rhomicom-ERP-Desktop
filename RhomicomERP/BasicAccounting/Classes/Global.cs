using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Accounting.Forms;
using System.Windows.Forms;
using CommonCode;

namespace Accounting.Classes
{
    /// <summary>
    /// A  class containing variables and 
    /// functions we will like to call directly from 
    /// anywhere in the project without creating an instance first
    /// </summary>
    class Global
    {
        #region "CONSTRUCTOR..."
        public Global() { }
        #endregion

        #region "GLOBAL DECLARATION..."
        public static Accounting myBscActn = new Accounting();
        public static mainForm mnFrm = null;
        public static acctnPrdsForm actnPrdFrm = null;
        public static pyblsDocsForm pyblsFrm = null;
        public static pettyCashDocsForm ptycshFrm = null;
        public static rcvblsDocsForm rcvblsFrm = null;
        public static fxdAsstsForm fxdAstsFrm = null;
        public static reconcileForm rcncileFrm = null;
        public static pymntsForm pymntFrm = null;
        public static custSpplrForm custFrm = null;
        public static accntsSetupForm accntFrm = null;
        public static taxNDscntsForm taxFrm = null;
        public static string[] cashFlowClsfctns ={"Cash and Cash Equivalents",
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
        public static string[] dfltPrvldgs = { "View Accounting","View Chart of Accounts", 
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
    /*91*/"Add Customers/Suppliers", "Edit Customers/Suppliers", "Delete Customers/Suppliers",
    /*94*/"Add Fixed Assets","Edit Fixed Assets", "Delete Fixed Assets"
    /*97*/,"View Petty Cash Vouchers", "View Petty Cash Payments","Add Petty Cash Payments","Edit Petty Cash Payments","Delete Petty Cash Payments"
    /*102*/,"View Petty Cash Re-imbursements","Add Petty Cash Re-imbursements","Edit Petty Cash Re-imbursements","Delete Petty Cash Re-imbursements"};

        public static string[] subGrpNames = { "Chart of Accounts" };//, "Accounting Transactions"
        public static string[] mainTableNames = { "accb.accb_chart_of_accnts" };//, "accb.accb_trnsctn_details"
        public static string[] keyColumnNames = { "accnt_id" };//, "transctn_id" 
        public static string currentPanel = "";
        public static string[] sysParaIDs = { "-130", "-140", "-150", "-160", "-170", "-180", "-190", "-200" };
        public static string[] sysParaNames = { "Report Title:", "Cols Nos To Group or Width & Height (Px) for Charts:",
                                          "Cols Nos To Count or Use in Charts:", "Columns To Sum:", "Columns To Average:",
                                          "Columns To Format Numerically:", "Report Output Formats", "Report Orientations" };

        #endregion

        #region "DATA MANIPULATION FUNCTIONS..."
        #region "INSERT STATEMENTS..."
        public static void createBdgtLn(long bdgtID, int accntid,
      double amntLmt, string strtDate, string endDate, string action)
        {
            strtDate = DateTime.ParseExact(
         strtDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            endDate = DateTime.ParseExact(
         endDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO accb.accb_budget_details(" +
                  "budget_id, accnt_id, limit_amount, start_date, " +
                  "end_date, created_by, creation_date, last_update_by, last_update_date, " +
                  "action_if_limit_excded) " +
                              "VALUES (" + bdgtID + "," + accntid + ", " + amntLmt +
                              ", '" + strtDate + "', '" + endDate + "', " +
                              Global.myBscActn.user_id + ", '" + dateStr +
                              "', " + Global.myBscActn.user_id +
                              ", '" + dateStr + "', '" + action + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void createBudget(int orgid, string bdgtname,
      string bdgtdesc, bool isactive, string strtDate, string endDate, string prdType)
        {
            strtDate = DateTime.ParseExact(
         strtDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            endDate = DateTime.ParseExact(
         endDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO accb.accb_budget_header(" +
                              "budget_name, budget_desc, is_the_active_one, created_by, " +
                              "creation_date, last_update_by, last_update_date, org_id, start_date, end_date, period_type) " +
                              "VALUES ('" + bdgtname.Replace("'", "''") + "', '" + bdgtdesc.Replace("'", "''") +
                              "', '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isactive) + "', " + Global.myBscActn.user_id + ", '" + dateStr +
                              "', " + Global.myBscActn.user_id + ", '" + dateStr + "', " + orgid + ", '" + strtDate.Replace("'", "''") +
                              "', '" + endDate.Replace("'", "''") +
                              "', '" + prdType.Replace("'", "''") +
                              "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void createDailyBals(int accntid, double netbals,
          double dbtbals, double crdtbals, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO accb.accb_accnt_daily_bals(" +
                              "accnt_id, net_balance, dbt_bal, crdt_bal, as_at_date, " +
                              "created_by, creation_date, last_update_by, last_update_date, src_trns_ids) " +
              "VALUES (" + accntid +
              ", " + netbals + ", " + dbtbals + ", " + crdtbals + ", '" + balsDate + "', " + Global.myBscActn.user_id + ", '" + dateStr +
                              "', " + Global.myBscActn.user_id + ", '" + dateStr + "', ',')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void createDailyAccntCurrBals(int accntid, double netbals,
          double dbtbals, double crdtbals, string balsDate, int currID)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO accb.accb_accnt_crncy_daily_bals(" +
                              "accnt_id, net_balance, dbt_bal, crdt_bal, as_at_date, " +
                              "created_by, creation_date, last_update_by, last_update_date, src_trns_ids, crncy_id) " +
              "VALUES (" + accntid +
              ", " + netbals + ", " + dbtbals + ", " + crdtbals + ", '" + balsDate + "', " + Global.myBscActn.user_id + ", '" + dateStr +
                              "', " + Global.myBscActn.user_id + ", '" + dateStr + "', ',', " + currID + ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void createChrt(int orgid, string accntnum, string accntname,
          string accntdesc, bool isContra, int prntAccntID, string accntTyp,
          bool isparent, bool isenbld, bool isretearngs, bool isnetincome, int rpt_ln,
          bool hasSbLdgrs, int cntrlAccntID, int currID, bool isSuspns, string accClsftn,
          int accntSegmnt1, int accntSegmnt2, int accntSegmnt3, int accntSegmnt4, int accntSegmnt5,
          int accntSegmnt6, int accntSegmnt7, int accntSegmnt8, int accntSegmnt9, int accntSegmnt10,
          int mappedAcntID)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            if (isretearngs == true)
            {
                Global.clearChrtRetEarns(orgid);
            }
            if (isnetincome == true)
            {
                Global.clearChrtNetIncome(orgid);
            }
            if (isSuspns == true)
            {
                Global.clearChrtSuspns(orgid);
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
             "', '" + accntdesc.Replace("'", "''") + "', '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isContra) +
             "', " + prntAccntID + ", '" + dateStr + "', " + Global.myBscActn.user_id + ", '" + dateStr +
                     "', " + Global.myBscActn.user_id + ", '" + dateStr + "', " +
                     orgid + ", '" + accntTyp.Replace("'", "''") +
             "', '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isparent) + "', 0, 0, '" +
             Global.mnFrm.cmCde.cnvrtBoolToBitStr(isenbld) + "', 0, '" +
             Global.mnFrm.cmCde.cnvrtBoolToBitStr(isretearngs) + "', '" +
             Global.mnFrm.cmCde.cnvrtBoolToBitStr(isnetincome) + "', " +
             Global.getAcctTypID(accntTyp) +
             ", " + rpt_ln + ", '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(hasSbLdgrs) +
             "', " + cntrlAccntID + ", " + currID + ", '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isSuspns) +
             "','" + accClsftn.Replace("'", "''") + "', " + accntSegmnt1 + ", " + accntSegmnt2 + ", " + accntSegmnt3 +
             ", " + accntSegmnt4 + ", " + accntSegmnt5 + ", " + accntSegmnt6 + ", " + accntSegmnt7 + ", " + accntSegmnt8 +
             ", " + accntSegmnt9 + ", " + accntSegmnt10 + ", " + mappedAcntID + ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static DataSet get_One_RptClsfctns(int limit_size, int offset)
        {
            string strSql = @"SELECT b.accnt_num, b.accnt_name, a.maj_rpt_ctgry, a.min_rpt_ctgry, b.account_clsfctn 
  FROM accb.accb_chart_of_accnts b LEFT OUTER JOIN accb.accb_account_clsfctns a ON (a.account_id= b.accnt_id)   
ORDER BY b.accnt_num, a.maj_rpt_ctgry, a.min_rpt_ctgry LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_SgmntVal_RptClsfctns(int sgmntValid)
        {
            string strSql = @"SELECT account_clsfctn_id, maj_rpt_ctgry, min_rpt_ctgry, 
       created_by, creation_date, last_update_by, last_update_date
  FROM org.org_account_clsfctns a WHERE(a.account_id = " + sgmntValid + ") ORDER BY 1";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_RptClsfctnID(string majCtgrName, string minCtgrName, int accntID)
        {
            string strSql = @"SELECT account_clsfctn_id from accb.accb_account_clsfctns where account_id=" + accntID +
              " and lower(maj_rpt_ctgry)='" + majCtgrName.Replace("'", "''").ToLower() +
              "' and lower(min_rpt_ctgry)='" + minCtgrName.Replace("'", "''").ToLower() + "'";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            //Global.taxFrm.rec_SQL = strSql;
            return -1;
        }

        public static long getNewRptClsfLnID()
        {
            string strSql = "select nextval('accb.accb_account_clsfctns_account_clsfctn_id_seq')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static void createRptClsfctn(long clsfctnID, string majCtgrName, string minCtgrName, int accntID)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_account_clsfctns(
            account_clsfctn_id, account_id, maj_rpt_ctgry, min_rpt_ctgry, 
            created_by, creation_date, last_update_by, last_update_date) " +
                  "VALUES (" + clsfctnID + ", " + accntID + ", '" + majCtgrName.Replace("'", "''") +
                  "', '" + minCtgrName.Replace("'", "''") +
                  "', " + Global.mnFrm.cmCde.User_id + ", '" + dateStr +
                  "', " + Global.mnFrm.cmCde.User_id + ", '" + dateStr +
                  "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updateRptClsfctn(long clsfctnID, string majCtgrName, string minCtgrName, int accntID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_account_clsfctns SET " +
                  "maj_rpt_ctgry='" + majCtgrName.Replace("'", "''") +
                  "', min_rpt_ctgry='" + minCtgrName.Replace("'", "''") +
                  "',account_id=" + accntID +
                  ", last_update_by = " + Global.mnFrm.cmCde.User_id + ", " +
                  "last_update_date = '" + dateStr +
                  "' WHERE (account_clsfctn_id =" + clsfctnID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void createRptClsfctnImprt(long clsfctnID, string majCtgrName, string minCtgrName, int accntID, string cshFlwClsfctn)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_account_clsfctns(
            account_clsfctn_id, account_id, maj_rpt_ctgry, min_rpt_ctgry, 
            created_by, creation_date, last_update_by, last_update_date) " +
                  "VALUES (" + clsfctnID + ", " + accntID + ", '" + majCtgrName.Replace("'", "''") +
                  "', '" + minCtgrName.Replace("'", "''") +
                  "', " + Global.mnFrm.cmCde.User_id + ", '" + dateStr +
                  "', " + Global.mnFrm.cmCde.User_id + ", '" + dateStr +
                  "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
            if (cshFlwClsfctn != "")
            {
                insSQL = "UPDATE accb.accb_chart_of_accnts SET " +
                      "account_clsfctn='" + cshFlwClsfctn.Replace("'", "''") +
                      "', last_update_by = " + Global.mnFrm.cmCde.User_id + ", " +
                      "last_update_date = '" + dateStr +
                      "' WHERE (accnt_id=" + accntID + ")";
                Global.mnFrm.cmCde.updateDataNoParams(insSQL);
            }
        }

        public static void updateRptClsfctnImprt(long clsfctnID, string majCtgrName, string minCtgrName, int accntID, string cshFlwClsfctn)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_account_clsfctns SET " +
                  "maj_rpt_ctgry='" + majCtgrName.Replace("'", "''") +
                  "', min_rpt_ctgry='" + minCtgrName.Replace("'", "''") +
                  "',account_id=" + accntID +
                  ", last_update_by = " + Global.mnFrm.cmCde.User_id + ", " +
                  "last_update_date = '" + dateStr +
                  "' WHERE (account_clsfctn_id =" + clsfctnID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
            if (cshFlwClsfctn != "")
            {
                updtSQL = "UPDATE accb.accb_chart_of_accnts SET " +
                      "account_clsfctn='" + cshFlwClsfctn.Replace("'", "''") +
                      "', last_update_by = " + Global.mnFrm.cmCde.User_id + ", " +
                      "last_update_date = '" + dateStr +
                      "' WHERE (accnt_id=" + accntID + ")";
                Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
            }
        }
        public static void createBatch(int orgid, string batchname,
          string batchdesc, string btchsrc, string batchvldty, long srcbatchid, string avlblforPpstng)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO accb.accb_trnsctn_batches(" +
                              "batch_name, batch_description, created_by, creation_date, " +
                              "org_id, batch_status, last_update_by, last_update_date, " +
            "batch_source, batch_vldty_status, src_batch_id, avlbl_for_postng) " +
                              "VALUES ('" + batchname.Replace("'", "''") + "', '" + batchdesc.Replace("'", "''") +
                              "', " + Global.myBscActn.user_id + ", '" + dateStr +
                              "', " + orgid + ", '0', " + Global.myBscActn.user_id + ", '" + dateStr +
                              "', '" + btchsrc.Replace("'", "''") +
                              "', '" + batchvldty.Replace("'", "''") +
                              "', " + srcbatchid +
                              ",'" + avlblforPpstng + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void createAttachment(long batchid, string attchDesc,
         string filNm, string tblNm, string pkNm)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO " + tblNm + "(" +
                  pkNm + ", attchmnt_desc, file_name, created_by, " +
                  "creation_date, last_update_by, last_update_date) " +
                              "VALUES (" + batchid +
                              ", '" + attchDesc.Replace("'", "''") +
                              "', '" + filNm.Replace("'", "''") +
                              "', " + Global.myBscActn.user_id + ", '" + dateStr +
                              "', " + Global.myBscActn.user_id + ", '" + dateStr + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static long getTrnsID(string trsDesc, int accntID, double entrdAmnt, int entrdCurID, string trnsDate)
        {
            string selSql = @"Select transctn_id from accb.accb_trnsctn_details
   where accnt_id=" + accntID + " and transaction_desc='" + trsDesc.Replace("'", "''") +
                             "' and entered_amnt =" + entrdAmnt + " and " +
            "entered_amt_crncy_id=" + entrdCurID + " and trnsctn_date = '" + trnsDate.Replace("'", "''") + "'";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(selSql);

            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static void createTransaction(int accntid, string trnsDesc,
      double dbtAmnt, string trnsDate, int crncyid,
          long batchid, double crdtamnt, double netAmnt,
          double entrdAmt, int entrdCurrID, double acntAmnt, int acntCurrID,
          double funcExchRate, double acntExchRate, string dbtOrCrdt, string refDocNum)
        {
            trnsDate = DateTime.ParseExact(
         trnsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            if (trnsDesc.Length > 500)
            {
                trnsDesc = trnsDesc.Substring(0, 500);
            }
            if (Global.getTrnsID(trnsDesc, accntid, entrdAmt, entrdCurrID, trnsDate) > 0)
            {
                Global.mnFrm.cmCde.showMsg("Same Transaction has been created Already!\r\nConsider changing the Date or Time and Try Again!", 0);
                return;
            }
            double tstVal = 0;
            if (double.TryParse(refDocNum, out tstVal))
            {
                refDocNum = "Ref.:" + refDocNum;
            }
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO accb.accb_trnsctn_details(" +
                              "accnt_id, transaction_desc, dbt_amount, trnsctn_date, " +
                              "func_cur_id, created_by, creation_date, batch_id, crdt_amount, " +
                              @"last_update_by, last_update_date, net_amount, 
            entered_amnt, entered_amt_crncy_id, accnt_crncy_amnt, accnt_crncy_id, 
            func_cur_exchng_rate, accnt_cur_exchng_rate, dbt_or_crdt, ref_doc_number) " +
                              "VALUES (" + accntid + ", '" + trnsDesc.Replace("'", "''") + "', " + dbtAmnt +
                              ", '" + trnsDate + "', " + crncyid + ", " + Global.myBscActn.user_id + ", '" + dateStr +
                              "', " + batchid + ", " + crdtamnt + ", " + Global.myBscActn.user_id +
                              ", '" + dateStr + "'," + netAmnt + ", " + entrdAmt +
                              ", " + entrdCurrID + ", " + acntAmnt +
                              ", " + acntCurrID + ", " + funcExchRate +
                              ", " + acntExchRate + ", '" + dbtOrCrdt + "', '" + refDocNum.Replace("'", "''") + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static bool createTransaction(long trnsID, int accntid, string trnsDesc,
      double dbtAmnt, string trnsDate, int crncyid,
          long batchid, double crdtamnt, double netAmnt,
          double entrdAmt, int entrdCurrID, double acntAmnt, int acntCurrID,
          double funcExchRate, double acntExchRate, string dbtOrCrdt, string refDocNum)
        {
            trnsDate = DateTime.ParseExact(
         trnsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            if (trnsDesc.Length > 500)
            {
                trnsDesc = trnsDesc.Substring(0, 500);
            }
            if (Global.getTrnsID(trnsDesc, accntid, entrdAmt, entrdCurrID, trnsDate) > 0)
            {
                Global.mnFrm.cmCde.showMsg("Same Transaction has been created Already!\r\nConsider changing the Date or Time and Try Again!", 0);
                return false;
            }
            double tstVal = 0;
            if (double.TryParse(refDocNum, out tstVal))
            {
                refDocNum = "Ref.:" + refDocNum;
            }

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO accb.accb_trnsctn_details(" +
                              "transctn_id, accnt_id, transaction_desc, dbt_amount, trnsctn_date, " +
                              "func_cur_id, created_by, creation_date, batch_id, crdt_amount, " +
                              @"last_update_by, last_update_date, net_amount, 
            entered_amnt, entered_amt_crncy_id, accnt_crncy_amnt, accnt_crncy_id, 
            func_cur_exchng_rate, accnt_cur_exchng_rate, dbt_or_crdt, ref_doc_number) " +
                              "VALUES (" + trnsID + "," + accntid + ", '" + trnsDesc.Replace("'", "''") + "', " + dbtAmnt +
                              ", '" + trnsDate + "', " + crncyid + ", " + Global.myBscActn.user_id + ", '" + dateStr +
                              "', " + batchid + ", " + crdtamnt + ", " + Global.myBscActn.user_id +
                              ", '" + dateStr + "'," + netAmnt + ", " + entrdAmt +
                              ", " + entrdCurrID + ", " + acntAmnt +
                              ", " + acntCurrID + ", " + funcExchRate +
                              ", " + acntExchRate + ", '" + dbtOrCrdt + "', '" + refDocNum.Replace("'", "''") + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
            return true;
        }

        public static bool createTransaction(long trnsID, int accntid, string trnsDesc,
      double dbtAmnt, string trnsDate, int crncyid,
          long batchid, double crdtamnt, double netAmnt,
          double entrdAmt, int entrdCurrID, double acntAmnt, int acntCurrID,
          double funcExchRate, double acntExchRate, string dbtOrCrdt, string refDocNum,
          long srcTrnsID)
        {
            trnsDate = DateTime.ParseExact(
         trnsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            if (trnsDesc.Length > 500)
            {
                trnsDesc = trnsDesc.Substring(0, 500);
            }
            if (Global.getTrnsID(trnsDesc, accntid, entrdAmt, entrdCurrID, trnsDate) > 0)
            {
                Global.mnFrm.cmCde.showMsg("Same Transaction has been created Already!\r\nConsider changing the Date or Time and Try Again!", 0);
                return false; ;
            }
            double tstVal = 0;
            if (double.TryParse(refDocNum, out tstVal))
            {
                refDocNum = "Ref.:" + refDocNum;
            }

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO accb.accb_trnsctn_details(" +
                              "transctn_id, accnt_id, transaction_desc, dbt_amount, trnsctn_date, " +
                              "func_cur_id, created_by, creation_date, batch_id, crdt_amount, " +
                              @"last_update_by, last_update_date, net_amount, 
            entered_amnt, entered_amt_crncy_id, accnt_crncy_amnt, accnt_crncy_id, 
            func_cur_exchng_rate, accnt_cur_exchng_rate, dbt_or_crdt, ref_doc_number, src_trns_id_reconciled) " +
                              "VALUES (" + trnsID + "," + accntid + ", '" + trnsDesc.Replace("'", "''") + "', " + dbtAmnt +
                              ", '" + trnsDate + "', " + crncyid + ", " + Global.myBscActn.user_id + ", '" + dateStr +
                              "', " + batchid + ", " + crdtamnt + ", " + Global.myBscActn.user_id +
                              ", '" + dateStr + "'," + netAmnt + ", " + entrdAmt +
                              ", " + entrdCurrID + ", " + acntAmnt +
                              ", " + acntCurrID + ", " + funcExchRate +
                              ", " + acntExchRate + ", '" + dbtOrCrdt + "', '" +
                              refDocNum.Replace("'", "''") + "', " + srcTrnsID + ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
            if (srcTrnsID > 0)
            {
                Global.changeReconciledStatus(srcTrnsID, "1");
            }
            return true;
        }

        public static void createAmntBrkDwn(long trnsID, long trnsdetid, int pssblvalid, string trnsDesc,
  double qty, double unitAmnt, double ttlAmnt)
        {
            if (trnsDesc.Length > 290)
            {
                trnsDesc = trnsDesc.Substring(0, 290);
            }
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_trnsctn_amnt_breakdown(
            transaction_id, description, quantity, unit_amnt, ttl_amnt, created_by, 
            creation_date, last_update_by, last_update_date, trns_amnt_det_id, 
            lnkd_pssbl_val_id) " +
                              "VALUES (" + trnsID + ", '" + trnsDesc.Replace("'", "''") + "', " + qty + ", " +
                               +unitAmnt + ", " + ttlAmnt + ", " + Global.myBscActn.user_id +
                              ", '" + dateStr + "', " + Global.myBscActn.user_id +
                              ", '" + dateStr + "', " + trnsdetid + ", " + pssblvalid + ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updateAmntBrkDwn(long trnsID, long trnsdetid, int pssblvalid, string trnsDesc,
         double qty, double unitAmnt, double ttlAmnt)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string insSQL = @"UPDATE accb.accb_trnsctn_amnt_breakdown SET 
            transaction_id=" + trnsID + ", description='" + trnsDesc.Replace("'", "''") +
                                   "', quantity=" + qty + ", unit_amnt=" +
                               +unitAmnt + ", ttl_amnt=" + ttlAmnt + ", last_update_by=" + Global.myBscActn.user_id +
                              ", last_update_date='" + dateStr + "', lnkd_pssbl_val_id=" + pssblvalid + " " +
                              "WHERE (trns_amnt_det_id= " + trnsdetid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void updateAmntBrkDwn(long oldtrnsID, long nwtrnsID)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string insSQL = @"UPDATE accb.accb_trnsctn_amnt_breakdown SET 
            transaction_id=" + nwtrnsID + ", last_update_by=" + Global.myBscActn.user_id +
                              ", last_update_date='" + dateStr + "' " +
                              "WHERE (transaction_id= " + oldtrnsID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static long getNewBrkDwnLnID()
        {
            string strSql = "select nextval('accb.accb_bdgt_amnt_brkdwn_bdgt_amnt_brkdwn_id_seq')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static long get_BrkDwnLnID(long bdgtDetID, int itmID, int accntID)
        {
            string strSql = @"SELECT bdgt_amnt_brkdwn_id from accb.accb_bdgt_amnt_brkdwn where account_id=" + accntID +
              " and bdgt_item_id=" + itmID +
              " and budget_det_id=" + bdgtDetID + "";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            //Global.taxFrm.rec_SQL = strSql;
            return -1;
        }
        public static long get_BdgtDetID(string strtDte, string endDte, long bdgtID, int accntID)
        {
            strtDte = DateTime.ParseExact(
         strtDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            endDte = DateTime.ParseExact(
         endDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = @"SELECT budget_det_id from accb.accb_budget_details where accnt_id=" + accntID +
              " and budget_id=" + bdgtID +
              " and start_date='" + strtDte + "'" +
              " and end_date='" + endDte + "'";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            //Global.taxFrm.rec_SQL = strSql;
            return -1;
        }

        public static void createBdgtBrkDwn(long brkDwnID, long accntID, int itemID, string bdgtDetType, string lineDesc,
          double qty, double mltplr2, double unitAmnt, long bdgtDetID, string strtDte, string endDte)
        {
            if (lineDesc.Length > 299)
            {
                lineDesc = lineDesc.Substring(0, 299);
            }
            if (bdgtDetType.Length > 299)
            {
                bdgtDetType = bdgtDetType.Substring(0, 299);
            }
            strtDte = DateTime.ParseExact(
         strtDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            endDte = DateTime.ParseExact(
         endDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_bdgt_amnt_brkdwn(
            bdgt_amnt_brkdwn_id, account_id, bdgt_item_id, bdgt_detail_type, 
            item_quantity1, item_quantity2, unit_price_or_rate, remarks_desc, 
            created_by, creation_date, last_update_by, last_update_date, budget_det_id, start_date, end_date) " +
                              "VALUES (" + brkDwnID +
                              ", " + accntID +
                              ", " + itemID +
                              ", '" + bdgtDetType.Replace("'", "''") +
                              "', " + qty +
                              ", " + mltplr2 +
                               ", " + unitAmnt +
                               ",'" + lineDesc.Replace("'", "''") +
                               "', " + Global.myBscActn.user_id +
                              ", '" + dateStr + "', " + Global.myBscActn.user_id +
                              ", '" + dateStr + "', " + bdgtDetID +
                              ", '" + strtDte + "', '" + endDte + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updateBdgtBrkDwn(long brkDwnID, long accntID, int itemID, string bdgtDetType, string lineDesc,
          double qty, double mltplr2, double unitAmnt, long bdgtDetID, string strtDte, string endDte)
        {
            strtDte = DateTime.ParseExact(
         strtDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            endDte = DateTime.ParseExact(
         endDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string insSQL = @"UPDATE accb.accb_bdgt_amnt_brkdwn SET 
                                account_id=" + accntID +
                                ", bdgt_item_id = " + itemID +
                                ", bdgt_detail_type ='" + bdgtDetType.Replace("'", "''") +
                                "', item_quantity1=" + qty +
                                ", unit_price_or_rate=" + unitAmnt +
                                ", item_quantity2=" + mltplr2 +
                                ", remarks_desc='" + lineDesc.Replace("'", "''") +
                                "', last_update_by=" + Global.myBscActn.user_id +
                              ", last_update_date='" + dateStr +
                              "', budget_det_id = " + bdgtDetID +
                                ", start_date='" + strtDte +
                              "', end_date='" + endDte +
                              "' WHERE (bdgt_amnt_brkdwn_id= " + brkDwnID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void updateBdgtDetAmnt(long bdgtDetID, long accntID, double ttlAmount)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string insSQL = @"UPDATE accb.accb_budget_details SET 
                               limit_amount=" + ttlAmount +
                                ", last_update_by=" + Global.myBscActn.user_id +
                              ", last_update_date='" + dateStr +
                              "' WHERE (budget_det_id= " + bdgtDetID + " and accnt_id = " + accntID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void updateBdgtDetAmnt(long bdgtDetID, long accntID)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string insSQL = @"UPDATE accb.accb_budget_details SET 
                               limit_amount=(SELECT SUM(z.item_quantity1*z.item_quantity2*z.unit_price_or_rate) " +
                               "FROM accb.accb_bdgt_amnt_brkdwn z WHERE z.budget_det_id=" + bdgtDetID +
                               " and z.account_id = " + accntID + "), last_update_by=" + Global.myBscActn.user_id +
                              ", last_update_date='" + dateStr +
                              "' WHERE (budget_det_id= " + bdgtDetID + " and accnt_id = " + accntID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void updateBdgtBrkDwnDates(long bdgtDetID, string strtDte, string endDte)
        {
            strtDte = DateTime.ParseExact(
         strtDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            endDte = DateTime.ParseExact(
         endDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string insSQL = @"UPDATE accb.accb_bdgt_amnt_brkdwn SET 
                                 last_update_by=" + Global.myBscActn.user_id +
                              ", last_update_date='" + dateStr +
                              "', start_date='" + strtDte +
                              "', end_date='" + endDte +
                              "' WHERE (budget_det_id = " + bdgtDetID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void createTmplt(int orgid, string tmpltname,
          string tmpltdesc)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO accb.accb_trnsctn_templates_hdr(" +
                              "template_name, template_description, created_by, " +
                              "creation_date, last_update_by, last_update_date, org_id) " +
                              "VALUES ('" + tmpltname.Replace("'", "''") + "', '" + tmpltdesc.Replace("'", "''") +
                              "', " + Global.myBscActn.user_id + ", '" + dateStr +
                              "', " + Global.myBscActn.user_id + ", '" + dateStr + "', " + orgid + ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void createTmpltTrns(int accntid, string trnsDesc,
      long tmpltid, string incrsDcrs)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO accb.accb_trnsctn_templates_det(" +
                              "template_id, accnt_id, increase_decrease, trnstn_desc, " +
                              "created_by, creation_date, last_update_by, last_update_date) " +
                              "VALUES (" + tmpltid + ", " + accntid + ", '" + incrsDcrs + "', '" +
                              trnsDesc.Replace("'", "''") + "', " + Global.myBscActn.user_id +
                              ", '" + dateStr + "', " + Global.myBscActn.user_id +
                              ", '" + dateStr + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void createTmpltUsr(long usrid, long tmpltid)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO accb.accb_trnsctn_templates_usrs(" +
                              "template_id, user_id, valid_start_date, valid_end_date, " +
                              "created_by, creation_date, last_update_by, last_update_date)" +
                              "VALUES (" + tmpltid + ", " + usrid + ", '" + dateStr +
                              "', '4000-12-31 00:00:00', " + Global.myBscActn.user_id +
                              ", '" + dateStr + "', " + Global.myBscActn.user_id +
                              ", '" + dateStr + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void createProcess()
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO accb.accb_running_prcses(" +
                          "process_id, which_process_is_rnng) " +
                          "VALUES (1, -1)";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void createTodaysGLBatch(int orgid, string batchnm,
          string batchdesc, string batchsource)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO accb.accb_trnsctn_batches(" +
                              "batch_name, batch_description, created_by, creation_date, " +
                              "org_id, batch_status, last_update_by, last_update_date, batch_source) " +
              "VALUES ('" + batchnm.Replace("'", "''") + "', '" + batchdesc.Replace("'", "''") +
              "', " + Global.myBscActn.user_id + ", '" + dateStr + "', " + orgid + ", '0', " +
                              Global.myBscActn.user_id + ", '" + dateStr + "', '" +
                              batchsource.Replace("'", "''") + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }
        #endregion

        #region "UPDATE STATEMENTS..."
        public static void updateBdgtLn(long bdgtDtID, int accntid,
      double amntLmt, string strtDate, string endDate, string action)
        {
            strtDate = DateTime.ParseExact(
         strtDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            endDate = DateTime.ParseExact(
         endDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_budget_details SET " +
                              "accnt_id = " + accntid + ", limit_amount = " + amntLmt +
                              ", start_date = '" + strtDate + "', " +
                              "end_date = '" + endDate + "', last_update_by = " +
                              Global.myBscActn.user_id + ", last_update_date = '" + dateStr +
                              "', " +
                              "action_if_limit_excded = '" + action + "' " +
                              "WHERE budget_det_id = " + bdgtDtID;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void setAllBdgtInActive()
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_budget_header SET " +
                              "is_the_active_one = '0', last_update_by = " + Global.myBscActn.user_id +
                              ", last_update_date = '" + dateStr + "' " +
                              "WHERE is_the_active_one = '1'";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updateBudget(long budgtID, string bdgtname,
      string bdgtdesc, bool isactive, string strtDate, string endDate, string prdType)
        {
            strtDate = DateTime.ParseExact(strtDate, "dd-MMM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            endDate = DateTime.ParseExact(endDate, "dd-MMM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_budget_header SET " +
                              "budget_name = '" + bdgtname.Replace("'", "''") +
                              "', budget_desc = '" + bdgtdesc.Replace("'", "''") +
                              "', is_the_active_one = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isactive) +
                              "', last_update_by = " + Global.myBscActn.user_id +
                              ", last_update_date = '" + dateStr + "' " +
                              ", start_date = '" + strtDate + "' " +
                              ", end_date = '" + endDate + "' " +
                              ", period_type = '" + prdType + "' " +
                              "WHERE budget_id = " + budgtID;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updateTmpltTrns(int accntid, string trnsDesc,
      long tmpltid, string incrsDcrs, long detid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_trnsctn_templates_det " +
            "SET accnt_id=" + accntid + ", increase_decrease = '" + incrsDcrs +
            "', trnstn_desc='" + trnsDesc.Replace("'", "''") +
            "', template_id=" + tmpltid + ", last_update_by=" + Global.myBscActn.user_id +
            ", last_update_date='" + dateStr + "' " +
            " WHERE detail_id=" + detid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updateTransaction(int accntid, string trnsDesc,
      double dbtAmnt, string trnsDate, int crncyid, long batchid,
          double crdtamnt, double netAmnt, long trnsid,
          double entrdAmt, int entrdCurrID, double acntAmnt, int acntCurrID,
          double funcExchRate, double acntExchRate, string dbtOrCrdt, string refDocNum)
        {
            double tstVal = 0;
            if (double.TryParse(refDocNum, out tstVal))
            {
                refDocNum = "Ref.:" + refDocNum;
            }

            trnsDate = DateTime.ParseExact(
         trnsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_trnsctn_details " +
            "SET accnt_id=" + accntid + ", transaction_desc='" + trnsDesc.Replace("'", "''") +
            "', dbt_amount=" + dbtAmnt + ", trnsctn_date='" + trnsDate + "', func_cur_id=" + crncyid +
            ", batch_id=" + batchid + ", crdt_amount=" + crdtamnt + ", last_update_by=" + Global.myBscActn.user_id +
            ", last_update_date='" + dateStr + "', net_amount=" + netAmnt +
            ", entered_amnt=" + entrdAmt + ", entered_amt_crncy_id=" + entrdCurrID +
            ", accnt_crncy_amnt=" + acntAmnt + ", accnt_crncy_id=" + acntCurrID +
            ", func_cur_exchng_rate=" + funcExchRate + ", accnt_cur_exchng_rate=" + acntExchRate +
            ", dbt_or_crdt='" + dbtOrCrdt +
            "', ref_doc_number='" + refDocNum.Replace("'", "''") +
            "' WHERE transctn_id=" + trnsid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updateTransaction(int accntid, string trnsDesc,
      double dbtAmnt, string trnsDate, int crncyid, long batchid,
          double crdtamnt, double netAmnt, long trnsid,
          double entrdAmt, int entrdCurrID, double acntAmnt, int acntCurrID,
          double funcExchRate, double acntExchRate, string dbtOrCrdt, string refDocNum, long srcTrnsID)
        {
            double tstVal = 0;
            if (double.TryParse(refDocNum, out tstVal))
            {
                refDocNum = "Ref.:" + refDocNum;
            }

            trnsDate = DateTime.ParseExact(
         trnsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_trnsctn_details " +
            "SET accnt_id=" + accntid + ", transaction_desc='" + trnsDesc.Replace("'", "''") +
            "', dbt_amount=" + dbtAmnt + ", trnsctn_date='" + trnsDate + "', func_cur_id=" + crncyid +
            ", batch_id=" + batchid + ", crdt_amount=" + crdtamnt + ", last_update_by=" + Global.myBscActn.user_id +
            ", last_update_date='" + dateStr + "', net_amount=" + netAmnt +
            ", entered_amnt=" + entrdAmt + ", entered_amt_crncy_id=" + entrdCurrID +
            ", accnt_crncy_amnt=" + acntAmnt + ", accnt_crncy_id=" + acntCurrID +
            ", func_cur_exchng_rate=" + funcExchRate + ", accnt_cur_exchng_rate=" + acntExchRate +
            ", dbt_or_crdt='" + dbtOrCrdt +
            "', ref_doc_number='" + refDocNum.Replace("'", "''") +
            "', src_trns_id_reconciled = " + srcTrnsID + " WHERE transctn_id=" + trnsid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
            if (srcTrnsID > 0)
            {
                Global.changeReconciledStatus(srcTrnsID, "1");
            }
        }

        public static void updateChrtDet(int orgid, int accntid, string accntnum, string accntname,
          string accntdesc, bool isContra, int prntAccntID, string accntTyp,
          bool isparent, bool isenbld, bool isretearngs, bool isnetincome, int rpt_ln,
          bool hasSbLdgrs, int cntrlAccntID, int currID, bool isSuspns, string accClsftn,
          int accntSegmnt1, int accntSegmnt2, int accntSegmnt3, int accntSegmnt4, int accntSegmnt5,
          int accntSegmnt6, int accntSegmnt7, int accntSegmnt8, int accntSegmnt9, int accntSegmnt10,
          int mappedAcntID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            if (isretearngs == true)
            {
                Global.clearChrtRetEarns(orgid);
            }
            if (isnetincome == true)
            {
                Global.clearChrtNetIncome(orgid);
            }
            if (isSuspns == true)
            {
                Global.clearChrtSuspns(orgid);
            }

            string updtSQL = "UPDATE accb.accb_chart_of_accnts " +
            "SET accnt_num='" + accntnum.Replace("'", "''") + "', accnt_name='" + accntname.Replace("'", "''") +
            "', accnt_desc='" + accntdesc.Replace("'", "''") + "', is_contra='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isContra) + "', " +
                "prnt_accnt_id=" + prntAccntID + ", " +
                "last_update_by=" + Global.myBscActn.user_id + ", last_update_date='" + dateStr +
                "', accnt_type='" + accntTyp.Replace("'", "''") + "', " +
                "is_prnt_accnt='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isparent) +
                "', is_enabled='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isenbld) + "', " +
                "is_retained_earnings='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isretearngs) +
                "', is_net_income='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isnetincome) +
                "', accnt_typ_id = " + Global.getAcctTypID(accntTyp) +
                ", report_line_no = " + rpt_ln +
                ", has_sub_ledgers = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(hasSbLdgrs) +
                "', control_account_id = " + cntrlAccntID +
                ", crncy_id = " + currID +
                ", is_suspens_accnt = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isSuspns) +
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
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void clearChrtRetEarns(int orgid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string updtSQL = "UPDATE accb.accb_chart_of_accnts " +
            "SET is_retained_earnings='0' WHERE org_id = " + orgid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void clearChrtNetIncome(int orgid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string updtSQL = "UPDATE accb.accb_chart_of_accnts " +
            "SET is_net_income='0' WHERE org_id = " + orgid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void clearChrtSuspns(int orgid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string updtSQL = "UPDATE accb.accb_chart_of_accnts " +
            "SET is_suspens_accnt='0' WHERE org_id = " + orgid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtAcntChrtBals(int accntid,
          double dbtAmnt, double crdtAmnt, double netAmnt, string trnsDate)
        {
            trnsDate = DateTime.ParseExact(
         trnsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            trnsDate = trnsDate.Substring(0, 10);

            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_chart_of_accnts " +
                  "SET last_update_by = " + Global.myBscActn.user_id +
                  ", last_update_date = '" + dateStr +
                          "', balance_date = '" + trnsDate + "', " +
                          "debit_balance = " + dbtAmnt +
                          ", credit_balance = " + crdtAmnt +
                          ", net_balance = " + netAmnt +
              " WHERE accnt_id = " + accntid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static double getSign(double inptAMnt)
        {
            if (inptAMnt != 0)
            {
                return inptAMnt / Math.Abs(inptAMnt);
            }
            return 0;
        }

        public static void postTransaction(int accntid,
          double dbtAmnt, double crdtAmnt, double netAmnt,
          string trnsDate, long src_trsID)
        {
            long dailybalID = Global.getAccntDailyBalsID(accntid, trnsDate);
            //Get dailybalid for accnt on this date
            //if doesn't exist get last accnt bals be4 this date
            //add new amount to it and insert record
            if (dailybalID <= 0)
            {
                double lstNetBals = Global.getAccntLstDailyNetBals(accntid, trnsDate);
                double lstDbtBals = Global.getAccntLstDailyDbtBals(accntid, trnsDate);
                double lstCrdtBals = Global.getAccntLstDailyCrdtBals(accntid, trnsDate);
                Global.createDailyBals(accntid, lstNetBals, lstDbtBals, lstCrdtBals, trnsDate);
                Global.updtAccntDailyBals(trnsDate, accntid, dbtAmnt,
                  crdtAmnt, netAmnt, src_trsID, "Do");
            }
            else
            {
                Global.updtAccntDailyBals(trnsDate, accntid, dbtAmnt,
                  crdtAmnt, netAmnt, src_trsID, "Do");
            }
        }

        public static void updtAccntDailyBals(string balsDate, int accntID,
      double dbtAmnt, double crdtAmnt, double netAmnt, long src_trnsID,
          string act_typ)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "";
            if (act_typ == "Undo")
            {
                updtSQL = "UPDATE accb.accb_accnt_daily_bals " +
            "SET last_update_by = " + Global.myBscActn.user_id +
            ", last_update_date = '" + dateStr +
                  "', dbt_bal = dbt_bal - " + dbtAmnt +
                  ", crdt_bal = crdt_bal - " + crdtAmnt +
                  ", net_balance = net_balance - " + netAmnt +
                  ", src_trns_ids = replace(src_trns_ids, '," + src_trnsID + ",', ',')" +
            " WHERE (to_timestamp(as_at_date,'YYYY-MM-DD') >=  to_timestamp('" + balsDate +
            "','YYYY-MM-DD') and accnt_id = " + accntID + ")";
            }
            else
            {
                updtSQL = "UPDATE accb.accb_accnt_daily_bals " +
            "SET last_update_by = " + Global.myBscActn.user_id +
            ", last_update_date = '" + dateStr +
                  "', dbt_bal = dbt_bal + " + dbtAmnt +
                  ", crdt_bal = crdt_bal + " + crdtAmnt +
                  ", net_balance = net_balance +" + netAmnt +
                  ", src_trns_ids = src_trns_ids || '" + src_trnsID + ",'" +
            " WHERE (to_timestamp(as_at_date,'YYYY-MM-DD') >=  to_timestamp('" + balsDate +
            "','YYYY-MM-DD') and accnt_id = " + accntID + ")";
            }
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void undoPostTransaction(int accntid, double dbtAmnt,
          double crdtAmnt, double netAmnt, string trnsDate, long src_trsID)
        {
            long dailybalID = Global.getAccntDailyBalsID(accntid, trnsDate);
            //Get dailybalid for accnt on this date
            //if doesn't exist get last accnt bals be4 this date
            //subtract new amount from it and insert record
            if (dailybalID <= 0)
            {
                //double lstNetBals = Global.getAccntLstDailyNetBals(accntid, trnsDate);
                //double lstDbtBals = Global.getAccntLstDailyDbtBals(accntid, trnsDate);
                //double lstCrdtBals = Global.getAccntLstDailyCrdtBals(accntid, trnsDate);
                //Global.createDailyBals(accntid, lstNetBals, lstDbtBals, lstCrdtBals, trnsDate);
                //Global.updtAccntDailyBals(trnsDate, accntid, dbtAmnt,
                //  crdtAmnt, netAmnt, src_trsID, "Undo");
            }
            else
            {
                Global.updtAccntDailyBals(trnsDate, accntid, dbtAmnt,
                  crdtAmnt, netAmnt, src_trsID, "Undo");
            }
        }


        public static void postAccntCurrTransaction(int accntid,
         double dbtAmnt, double crdtAmnt, double netAmnt,
         string trnsDate, long src_trsID, int currID)
        {
            if (dbtAmnt == 0 && crdtAmnt == 0 && netAmnt == 0)
            {
                double acntCurrAmnt = double.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
            "accb.accb_trnsctn_details", "transctn_id", "accnt_crncy_amnt", src_trsID));
                string dbtCrdt = Global.mnFrm.cmCde.getGnrlRecNm(
            "accb.accb_trnsctn_details", "transctn_id", "dbt_or_crdt", src_trsID);
                string incrdcrs = "";
                if (dbtCrdt == "C")
                {
                    incrdcrs = Global.incrsOrDcrsAccnt(accntid, "Credit");
                    dbtAmnt = 0;
                    crdtAmnt = acntCurrAmnt;
                    netAmnt = (double)Global.dbtOrCrdtAccntMultiplier(accntid,
               incrdcrs.Substring(0, 1)) * acntCurrAmnt;

                }
                else
                {
                    incrdcrs = Global.incrsOrDcrsAccnt(accntid, "Debit");
                    dbtAmnt = acntCurrAmnt;
                    crdtAmnt = 0;
                    netAmnt = (double)Global.dbtOrCrdtAccntMultiplier(accntid,
               incrdcrs.Substring(0, 1)) * acntCurrAmnt;
                }
            }
            long dailybalID = Global.getAccntDailyCurrBalsID(accntid, trnsDate);
            //Get dailybalid for accnt on this date
            //if doesn't exist get last accnt bals be4 this date
            //add new amount to it and insert record
            if (dailybalID <= 0)
            {
                double lstNetBals = Global.getAccntLstDailyNetCurrBals(accntid, trnsDate);
                double lstDbtBals = Global.getAccntLstDailyDbtCurrBals(accntid, trnsDate);
                double lstCrdtBals = Global.getAccntLstDailyCrdtCurrBals(accntid, trnsDate);
                Global.createDailyAccntCurrBals(accntid, lstNetBals, lstDbtBals, lstCrdtBals, trnsDate, currID);
                Global.updtAccntDailyCurrBals(trnsDate, accntid, dbtAmnt,
                  crdtAmnt, netAmnt, src_trsID, "Do", currID);
            }
            else
            {
                Global.updtAccntDailyCurrBals(trnsDate, accntid, dbtAmnt,
                  crdtAmnt, netAmnt, src_trsID, "Do", currID);
            }
        }

        public static void updtAccntDailyCurrBals(string balsDate, int accntID,
      double dbtAmnt, double crdtAmnt, double netAmnt, long src_trnsID,
          string act_typ, int currID)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "";
            if (act_typ == "Undo")
            {
                updtSQL = "UPDATE accb.accb_accnt_crncy_daily_bals " +
            "SET last_update_by = " + Global.myBscActn.user_id +
            ", last_update_date = '" + dateStr +
                  "', dbt_bal = dbt_bal - " + dbtAmnt +
                  ", crdt_bal = crdt_bal - " + crdtAmnt +
                  ", net_balance = net_balance - " + netAmnt +
                  ", src_trns_ids = replace(src_trns_ids, '," + src_trnsID + ",', ',')" +
                  ", crncy_id = " + currID + " " +
            " WHERE (to_timestamp(as_at_date,'YYYY-MM-DD') >=  to_timestamp('" + balsDate +
            "','YYYY-MM-DD') and accnt_id = " + accntID + ")";
            }
            else
            {
                updtSQL = "UPDATE accb.accb_accnt_crncy_daily_bals " +
            "SET last_update_by = " + Global.myBscActn.user_id +
            ", last_update_date = '" + dateStr +
                  "', dbt_bal = dbt_bal + " + dbtAmnt +
                  ", crdt_bal = crdt_bal + " + crdtAmnt +
                  ", net_balance = net_balance +" + netAmnt +
                  ", src_trns_ids = src_trns_ids || '" + src_trnsID + ",'" +
                  ", crncy_id = " + currID + " " +
            " WHERE (to_timestamp(as_at_date,'YYYY-MM-DD') >=  to_timestamp('" + balsDate +
            "','YYYY-MM-DD') and accnt_id = " + accntID + ")";
            }
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void undoPostAccntCurrTransaction(int accntid, double dbtAmnt,
          double crdtAmnt, double netAmnt, string trnsDate, long src_trsID, int currID)
        {
            if (dbtAmnt == 0 && crdtAmnt == 0 && netAmnt == 0)
            {
                double acntCurrAmnt = double.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
            "accb.accb_trnsctn_details", "transctn_id", "accnt_crncy_amnt", src_trsID));
                string dbtCrdt = Global.mnFrm.cmCde.getGnrlRecNm(
            "accb.accb_trnsctn_details", "transctn_id", "dbt_or_crdt", src_trsID);
                string incrdcrs = "";
                if (dbtCrdt == "C")
                {
                    incrdcrs = Global.incrsOrDcrsAccnt(accntid, "Credit");
                    dbtAmnt = 0;
                    crdtAmnt = acntCurrAmnt;
                    netAmnt = (double)Global.dbtOrCrdtAccntMultiplier(accntid,
               incrdcrs.Substring(0, 1)) * acntCurrAmnt;

                }
                else
                {
                    incrdcrs = Global.incrsOrDcrsAccnt(accntid, "Debit");
                    dbtAmnt = acntCurrAmnt;
                    crdtAmnt = 0;
                    netAmnt = (double)Global.dbtOrCrdtAccntMultiplier(accntid,
               incrdcrs.Substring(0, 1)) * acntCurrAmnt;
                }
            }
            long dailybalID = Global.getAccntDailyCurrBalsID(accntid, trnsDate);
            //Get dailybalid for accnt on this date
            //if doesn't exist get last accnt bals be4 this date
            //subtract new amount from it and insert record
            if (dailybalID <= 0)
            {
                //double lstNetBals = Global.getAccntLstDailyNetBals(accntid, trnsDate);
                //double lstDbtBals = Global.getAccntLstDailyDbtBals(accntid, trnsDate);
                //double lstCrdtBals = Global.getAccntLstDailyCrdtBals(accntid, trnsDate);
                //Global.createDailyBals(accntid, lstNetBals, lstDbtBals, lstCrdtBals, trnsDate);
                //Global.updtAccntDailyBals(trnsDate, accntid, dbtAmnt,
                //  crdtAmnt, netAmnt, src_trsID, "Undo");
            }
            else
            {
                Global.updtAccntDailyCurrBals(trnsDate, accntid, dbtAmnt,
                  crdtAmnt, netAmnt, src_trsID, "Undo", currID);
            }
        }

        public static void chngeTrnsStatus(long trnsid, string status)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_trnsctn_details " +
            "SET last_update_by = " + Global.myBscActn.user_id + ", last_update_date = '" + dateStr +
                    "', trns_status = '" + status + "'" +
         " WHERE transctn_id = " + trnsid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }


        public static void updtAccntPrntID(int accntID, int prntID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_chart_of_accnts SET prnt_accnt_id = " + prntID +
                              ", last_update_by = " + Global.myBscActn.user_id + ", " +
                              "last_update_date = '" + dateStr + "' " +
              "WHERE (accnt_id = " + accntID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtAccntCurrID(int accntID, int crncyID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_chart_of_accnts SET crncy_id = " + crncyID +
                              ", last_update_by = " + Global.myBscActn.user_id + ", " +
                              "last_update_date = '" + dateStr + "' " +
              "WHERE (accnt_id = " + accntID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtOrgAccntCurrID(int orgID, int crncyID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_chart_of_accnts SET crncy_id = " + crncyID +
                              ", last_update_by = " + Global.myBscActn.user_id + ", " +
                              "last_update_date = '" + dateStr + "' " +
              "WHERE (org_id = " + orgID + " and crncy_id<=0)";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
            updtSQL = @"UPDATE accb.accb_trnsctn_details SET dbt_or_crdt='C' WHERE dbt_or_crdt='U' and dbt_amount=0 and crdt_amount !=0;
UPDATE accb.accb_trnsctn_details SET dbt_or_crdt='D' WHERE dbt_or_crdt='U' and dbt_amount!=0 and crdt_amount =0;";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
            updtSQL = @"UPDATE accb.accb_trnsctn_details SET entered_amnt=dbt_amount, accnt_crncy_amnt=dbt_amount WHERE dbt_amount!=0 and crdt_amount =0 and entered_amnt=0 and accnt_crncy_amnt=0;
UPDATE accb.accb_trnsctn_details SET entered_amnt=crdt_amount, accnt_crncy_amnt=crdt_amount WHERE dbt_amount=0 and crdt_amount!=0 and entered_amnt=0 and accnt_crncy_amnt=0";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
            updtSQL = @"UPDATE accb.accb_trnsctn_details SET entered_amt_crncy_id=func_cur_id WHERE entered_amt_crncy_id=-1;
UPDATE accb.accb_trnsctn_details SET accnt_crncy_id=func_cur_id WHERE accnt_crncy_id=-1";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);

        }

        public static void updateBatch(long batchid, string batchname,
          string batchdesc)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_trnsctn_batches " +
            "SET batch_name='" + batchname.Replace("'", "''") + "', batch_description='" + batchdesc.Replace("'", "''") +
            "', last_update_by=" + Global.myBscActn.user_id + ", last_update_date='" + dateStr +
            "' WHERE batch_id = " + batchid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updateAttachment(long attchID, long batchid, string attchDesc,
        string filNm, string tblNm, string pkNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE " + tblNm + " SET " +
                  pkNm + "=" + batchid +
                              ", attchmnt_desc='" + attchDesc.Replace("'", "''") +
                              "', file_name='" + filNm.Replace("'", "''") +
                              "', last_update_by=" + Global.myBscActn.user_id +
                              ", last_update_date='" + dateStr + "' " +
                               "WHERE attchmnt_id = " + attchID;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updateBatchStatus(long batchid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_trnsctn_batches " +
            "SET batch_status='1', avlbl_for_postng='0', last_update_by=" + Global.myBscActn.user_id + ", last_update_date='" + dateStr +
            "' WHERE batch_id = " + batchid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updateBatchAvlblty(long batchid, string avlblty)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_trnsctn_batches " +
            "SET avlbl_for_postng='" + avlblty.Replace("'", "''") +
            "', last_update_by=" + Global.myBscActn.user_id +
            ", last_update_date='" + dateStr +
            "' WHERE batch_id = " + batchid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updateBatchVldtyStatus(long batchid, string vldty)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_trnsctn_batches " +
            "SET batch_vldty_status='" + vldty.Replace("'", "''") +
            "', last_update_by=" + Global.myBscActn.user_id +
            ", last_update_date='" + dateStr +
            "' WHERE batch_id = " + batchid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updatePrdCloseStatus(long batchid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_period_close_dates " +
            "SET is_posted='1' WHERE gl_batch_id = " + batchid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updateTmplt(long tmpltid, string tmpltname,
      string tmpltdesc)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_trnsctn_templates_hdr " +
            "SET template_name='" + tmpltname.Replace("'", "''") + "', template_description='" + tmpltdesc.Replace("'", "''") +
            "', last_update_by=" + Global.myBscActn.user_id + ", last_update_date='" + dateStr +
            "' WHERE template_id = " + tmpltid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void changeTmpltUsrVldStrDate(long rowid, string inpt_date)
        {
            inpt_date = DateTime.ParseExact(
         inpt_date, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            //Changes a user's account's valid start date
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string sqlStr = "UPDATE accb.accb_trnsctn_templates_usrs SET valid_start_date = '" + inpt_date + "', last_update_by = " +
              Global.myBscActn.user_id + ", last_update_date = '" + dateStr + "' WHERE (row_id = " +
              rowid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(sqlStr);
        }

        public static void changeTmpltUsrVldEndDate(long rowid, string inpt_date)
        {
            inpt_date = DateTime.ParseExact(
         inpt_date, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            //Changes a user's account's valid start date
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string sqlStr = "UPDATE accb.accb_trnsctn_templates_usrs SET valid_end_date = '" +
              inpt_date + "', last_update_by = " +
              Global.myBscActn.user_id + ", last_update_date = '" + dateStr + "' WHERE (row_id = " +
              rowid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(sqlStr);
        }

        public static void updateProcess(int processtyp)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_running_prcses " +
            "SET which_process_is_rnng=" + processtyp +
            " WHERE process_id = 1";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }
        #endregion

        #region "DELETE STATEMENTS..."
        public static void deleteTmplt(long tmpltid, string tmpltNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Template Name = " + tmpltNm;
            string delSql = "DELETE FROM accb.accb_trnsctn_templates_hdr WHERE(template_id = " + tmpltid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }

        public static void deleteTmpltTrns(long tmpltid, string tmpltNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Template Name = " + tmpltNm;
            string delSql = "DELETE FROM accb.accb_trnsctn_templates_det WHERE(template_id = " + tmpltid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }

        public static void deleteTmpltUsrs(long tmpltid, string tmpltNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Template Name = " + tmpltNm;
            string delSql = "DELETE FROM accb.accb_trnsctn_templates_usrs WHERE(template_id = " + tmpltid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }

        public static void deleteOneTmpltTrns(long tmpltdtid, string tmpltNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Template Name = " + tmpltNm;
            string delSql = "DELETE FROM accb.accb_trnsctn_templates_det WHERE(detail_id = " + tmpltdtid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }

        public static void deleteBdgt(long bdgtid, string bdgtNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Budget Name = " + bdgtNm;
            string delSql = "DELETE FROM accb.accb_budget_header WHERE(budget_id = " + bdgtid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }

        public static void deleteBdgtDet(long bdgtid, string bdgtNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Budget Name = " + bdgtNm;
            string delSql = "DELETE FROM accb.accb_bdgt_amnt_brkdwn WHERE(budget_det_id IN (Select budget_det_id from accb.accb_budget_details WHERE budget_id = " + bdgtid + "))";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
            delSql = "DELETE FROM accb.accb_budget_details WHERE(budget_id = " + bdgtid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }

        public static void deleteOneBdgtDet(long bdgtdtid, string bdgtNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Budget Name = " + bdgtNm;
            string delSql = "DELETE FROM accb.accb_budget_details WHERE(budget_det_id = " + bdgtdtid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }

        public static void deleteAccount(long accntid, string accntNm, string accntNo)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Account Name = " + accntNm + " Account No. = " + accntNo;
            string delSql = "DELETE FROM accb.accb_chart_of_accnts WHERE (accnt_id = " + accntid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }

        public static void deleteAttchmnt(long attchid, string attchNm, string tblNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Attachment Name = " + attchNm;
            string delSql = "DELETE FROM " + tblNm + " WHERE(attchmnt_id = " + attchid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }

        public static void deleteBatch(long batchid, string batchNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Batch Name = " + batchNm;
            string delSql = "DELETE FROM accb.accb_trnsctn_batches WHERE(batch_id = " + batchid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
            string updtSQL = @"UPDATE accb.accb_trnsctn_batches SET batch_vldty_status='VALID' WHERE batch_id IN (SELECT h.batch_id
  FROM accb.accb_trnsctn_batches h where batch_vldty_status='VOID'
AND NOT EXISTS(Select g.batch_id from accb.accb_trnsctn_batches g where h.batch_id=g.src_batch_id))";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void deleteBatchTrns(long batchid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSql = "DELETE FROM accb.accb_trnsctn_details WHERE(batch_id = " + batchid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }

        public static void deleteTransaction(long trnsid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSql = "DELETE FROM accb.accb_trnsctn_details WHERE(transctn_id = " + trnsid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
            delSql = "DELETE FROM accb.accb_trnsctn_amnt_breakdown WHERE(transaction_id = " + trnsid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }

        public static void deleteTransBrkDwn(long trnsdetid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSql = "DELETE FROM accb.accb_trnsctn_amnt_breakdown WHERE(trns_amnt_det_id = " + trnsdetid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }

        public static void deleteBdgtBrkDwn(long trnsdetid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSql = "DELETE FROM accb.accb_bdgt_amnt_brkdwn WHERE(bdgt_amnt_brkdwn_id = " + trnsdetid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }

        public static void deleteTmpltTransaction(long dettrnsid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSql = "DELETE FROM accb.accb_trnsctn_templates_det WHERE(detail_id = " + dettrnsid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }
        #endregion

        #region "SELECT STATEMENTS..."
        #region "COA DETAILS..."
        public static string getLastPeriodEndDate(string tstDate)
        {
            string strSql = "";
            strSql = @"SELECT to_char(to_timestamp(period_end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') 
                          FROM accb.accb_periods_det
                          Where period_end_date ilike '%" + tstDate + @"%'
                          and period_status = 'Open'
                          ORDER BY period_end_date DESC LIMIT 1 OFFSET 0";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count == 1)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
            }
        }
        public static int get_Rtnd_Erngs_Accnt(int orgid)
        {
            string strSql = "";
            strSql = "SELECT a.accnt_id " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE(a.is_retained_earnings = '1' and a.org_id = " + orgid + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count == 1)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        public static int get_Net_Income_Accnt(int orgid)
        {
            string strSql = "";
            strSql = "SELECT a.accnt_id " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE(a.is_net_income = '1' and a.org_id = " + orgid + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count == 1)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        public static int get_Suspns_Accnt(int orgid)
        {
            string strSql = "";
            strSql = "SELECT a.accnt_id " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE(a.is_suspens_accnt = '1' and a.org_id = " + orgid + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count == 1)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        public static int get_RetEarn_Accnt(int orgid)
        {
            string strSql = "";
            strSql = "SELECT a.accnt_id " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE(a.is_retained_earnings = '1' and a.org_id = " + orgid + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count == 1)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        public static double get_Accnt_Net_Bals(int accntID)
        {
            string strSql = "";
            strSql = "SELECT a.net_balance " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE(a.accnt_id = " + accntID + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count == 1)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0.00;
            }
        }

        public static double get_Accnt_Bls_Bals(int accntID, long blsID)
        {
            string strSql = "";
            strSql = "SELECT a.net_balance " +
              "FROM accb.accb_balsheet_details a " +
              "WHERE(a.accnt_id = " + accntID + " and a.balsheet_header_id = " + blsID + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count == 1)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0.00;
            }
        }

        public static long getTodaysGLBatchID(string batchnm, int orgid)
        {
            string strSql = "";
            strSql = "SELECT a.batch_id " +
          "FROM accb.accb_trnsctn_batches a " +
          "WHERE(a.batch_name ilike '%" + batchnm.Replace("'", "''") +
          "%' and org_id = " + orgid + " and batch_status = '0')";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        public static double getAccntDailyNetBals(int accntID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            string strSql = "";
            strSql = "SELECT a.net_balance " +
          "FROM accb.accb_accnt_daily_bals a " +
          "WHERE(to_timestamp(a.as_at_date,'YYYY-MM-DD') =  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.accnt_id = " + accntID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0.00;
            }
        }

        public static string[] getAccntLstDailyBalsInfo(int accntID, string balsDate)
        {
            string dateStr = balsDate;
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            string strSql = "";
            strSql = @"SELECT a.dbt_bal, a.crdt_bal, a.net_balance, 
to_char(to_timestamp(a.as_at_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
          "FROM accb.accb_accnt_daily_bals a " +
          "WHERE(to_timestamp(a.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.accnt_id = " + accntID +
          ") ORDER BY to_timestamp(a.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            string[] rslt = { "0", "0", "0", dateStr };
            if (dtst.Tables[0].Rows.Count > 0)
            {
                rslt[0] = dtst.Tables[0].Rows[0][0].ToString();
                rslt[1] = dtst.Tables[0].Rows[0][1].ToString();
                rslt[2] = dtst.Tables[0].Rows[0][2].ToString();
                rslt[3] = dtst.Tables[0].Rows[0][3].ToString();
                return rslt;
            }
            else
            {
                return rslt;
            }
        }

        public static double getAccntLstDailyNetBals(int accntID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            string strSql = "";
            strSql = "SELECT a.net_balance " +
          "FROM accb.accb_accnt_daily_bals a " +
          "WHERE(to_timestamp(a.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.accnt_id = " + accntID +
          ") ORDER BY to_timestamp(a.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0.00;
            }
        }

        public static long getAccntDailyBalsID(int accntID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            string strSql = "";
            strSql = "SELECT a.daily_bals_id " +
          "FROM accb.accb_accnt_daily_bals a " +
          "WHERE(to_timestamp(a.as_at_date,'YYYY-MM-DD') =  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.accnt_id = " + accntID + ")";

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

        public static double getAccntLstDailyCrdtBals(int accntID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            string strSql = "";
            strSql = "SELECT a.crdt_bal " +
          "FROM accb.accb_accnt_daily_bals a " +
          "WHERE(to_timestamp(a.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.accnt_id = " + accntID + ") ORDER BY to_timestamp(a.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0.00;
            }
        }

        public static double getAccntLstDailyDbtBals(int accntID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            string strSql = "";
            strSql = "SELECT a.dbt_bal " +
          "FROM accb.accb_accnt_daily_bals a " +
          "WHERE(to_timestamp(a.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.accnt_id = " + accntID + ") ORDER BY to_timestamp(a.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0.00;
            }
        }

        public static double getAccntDailyDbtBals(int accntID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            string strSql = "";
            strSql = "SELECT a.dbt_bal " +
          "FROM accb.accb_accnt_daily_bals a " +
          "WHERE(to_timestamp(a.as_at_date,'YYYY-MM-DD') =  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.accnt_id = " + accntID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0.00;
            }
        }

        public static double getAccntDailyCrdtBals(int accntID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            string strSql = "";
            strSql = "SELECT a.crdt_bal " +
          "FROM accb.accb_accnt_daily_bals a " +
          "WHERE(to_timestamp(a.as_at_date,'YYYY-MM-DD') =  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.accnt_id = " + accntID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0.00;
            }
        }

        public static double getPrntAccntDailyBals(int accntID, string balsDate, string balsTyp)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            string strSql = "";
            strSql = "SELECT accb.get_ltst_prnt_accnt_bals(" + accntID + ", '" + balsDate +
          "','" + balsTyp +
          "') ";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0.00;
            }
        }

        public static DataSet get_Bals_Prnt_Accnts(int prntAccntID)
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
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_Bals_Prnt_Accnts(int prntAccntID, string balsDate, bool useNetPostns,
            int rptSgmt1, int rptSgmt2, int rptSgmt3,
            int rptSgmt4, int rptSgmt5, int rptSgmt6,
            int rptSgmt7, int rptSgmt8, int rptSgmt9, int rptSgmt10)
        {
            balsDate = DateTime.ParseExact(
      balsDate, "dd-MMM-yyyy HH:mm:ss",
      System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            /**/
            string strSql = "";
            if (useNetPostns)
            {
                strSql = "Select accb.get_ltst_prnt_accnt_bals2(" + prntAccntID + ",'" + balsDate +
          "','dbt_amount', " + rptSgmt1.ToString() +
          "," + rptSgmt2.ToString() +
          "," + rptSgmt3.ToString() +
          "," + rptSgmt4.ToString() +
          "," + rptSgmt5.ToString() +
          "," + rptSgmt6.ToString() +
          "," + rptSgmt7.ToString() +
          "," + rptSgmt8.ToString() +
          "," + rptSgmt9.ToString() +
          "," + rptSgmt10.ToString() + "), accb.get_ltst_prnt_accnt_bals2(" + prntAccntID + ",'" + balsDate +
          "','crdt_amount', " + rptSgmt1.ToString() +
          "," + rptSgmt2.ToString() +
          "," + rptSgmt3.ToString() +
          "," + rptSgmt4.ToString() +
          "," + rptSgmt5.ToString() +
          "," + rptSgmt6.ToString() +
          "," + rptSgmt7.ToString() +
          "," + rptSgmt8.ToString() +
          "," + rptSgmt9.ToString() +
          "," + rptSgmt10.ToString() + "), accb.get_ltst_prnt_accnt_bals2(" + prntAccntID + ",'" + balsDate +
          "','net_amount', " + rptSgmt1.ToString() +
          "," + rptSgmt2.ToString() +
          "," + rptSgmt3.ToString() +
          "," + rptSgmt4.ToString() +
          "," + rptSgmt5.ToString() +
          "," + rptSgmt6.ToString() +
          "," + rptSgmt7.ToString() +
          "," + rptSgmt8.ToString() +
          "," + rptSgmt9.ToString() +
          "," + rptSgmt10.ToString() + ")";
            }
            else
            {
                strSql = "Select accb.get_ltst_prnt_accnt_bals3(" + prntAccntID + ",'" + balsDate +
           "','dbt_amount', " + rptSgmt1.ToString() +
          "," + rptSgmt2.ToString() +
          "," + rptSgmt3.ToString() +
          "," + rptSgmt4.ToString() +
          "," + rptSgmt5.ToString() +
          "," + rptSgmt6.ToString() +
          "," + rptSgmt7.ToString() +
          "," + rptSgmt8.ToString() +
          "," + rptSgmt9.ToString() +
          "," + rptSgmt10.ToString() + "), accb.get_ltst_prnt_accnt_bals3(" + prntAccntID + ",'" + balsDate +
           "','crdt_amount', " + rptSgmt1.ToString() +
          "," + rptSgmt2.ToString() +
          "," + rptSgmt3.ToString() +
          "," + rptSgmt4.ToString() +
          "," + rptSgmt5.ToString() +
          "," + rptSgmt6.ToString() +
          "," + rptSgmt7.ToString() +
          "," + rptSgmt8.ToString() +
          "," + rptSgmt9.ToString() +
          "," + rptSgmt10.ToString() + "),accb.get_ltst_prnt_accnt_bals3(" + prntAccntID + ",'" + balsDate +
           "','net_amount', " + rptSgmt1.ToString() +
          "," + rptSgmt2.ToString() +
          "," + rptSgmt3.ToString() +
          "," + rptSgmt4.ToString() +
          "," + rptSgmt5.ToString() +
          "," + rptSgmt6.ToString() +
          "," + rptSgmt7.ToString() +
          "," + rptSgmt8.ToString() +
          "," + rptSgmt9.ToString() +
          "," + rptSgmt10.ToString() + ")";
            }
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_TBals_Prnt_Accnts(int prntAccntID, string balsDate, bool useNetPostns,
            int rptSgmt1, int rptSgmt2, int rptSgmt3,
            int rptSgmt4, int rptSgmt5, int rptSgmt6,
            int rptSgmt7, int rptSgmt8, int rptSgmt9, int rptSgmt10)
        {
            balsDate = DateTime.ParseExact(balsDate, "dd-MMM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);
            /**/
            string strSql = "";
            if (useNetPostns)
            {
                strSql = "Select accb.get_ltst_prnt_accnt_tbals2(" + prntAccntID + ",'" + balsDate +
              "','dbt_amount', " + rptSgmt1.ToString() +
          "," + rptSgmt2.ToString() +
          "," + rptSgmt3.ToString() +
          "," + rptSgmt4.ToString() +
          "," + rptSgmt5.ToString() +
          "," + rptSgmt6.ToString() +
          "," + rptSgmt7.ToString() +
          "," + rptSgmt8.ToString() +
          "," + rptSgmt9.ToString() +
          "," + rptSgmt10.ToString() + "), accb.get_ltst_prnt_accnt_tbals2(" + prntAccntID + ",'" + balsDate +
              "','crdt_amount', " + rptSgmt1.ToString() +
          "," + rptSgmt2.ToString() +
          "," + rptSgmt3.ToString() +
          "," + rptSgmt4.ToString() +
          "," + rptSgmt5.ToString() +
          "," + rptSgmt6.ToString() +
          "," + rptSgmt7.ToString() +
          "," + rptSgmt8.ToString() +
          "," + rptSgmt9.ToString() +
          "," + rptSgmt10.ToString() + "), accb.get_ltst_prnt_accnt_tbals2(" + prntAccntID + ",'" + balsDate +
              "','net_amount', " + rptSgmt1.ToString() +
          "," + rptSgmt2.ToString() +
          "," + rptSgmt3.ToString() +
          "," + rptSgmt4.ToString() +
          "," + rptSgmt5.ToString() +
          "," + rptSgmt6.ToString() +
          "," + rptSgmt7.ToString() +
          "," + rptSgmt8.ToString() +
          "," + rptSgmt9.ToString() +
          "," + rptSgmt10.ToString() + ")";
            }
            else
            {
                strSql = "Select accb.get_ltst_prnt_accnt_tbals3(" + prntAccntID + ",'" + balsDate +
              "','dbt_amount', " + rptSgmt1.ToString() +
          "," + rptSgmt2.ToString() +
          "," + rptSgmt3.ToString() +
          "," + rptSgmt4.ToString() +
          "," + rptSgmt5.ToString() +
          "," + rptSgmt6.ToString() +
          "," + rptSgmt7.ToString() +
          "," + rptSgmt8.ToString() +
          "," + rptSgmt9.ToString() +
          "," + rptSgmt10.ToString() + "), accb.get_ltst_prnt_accnt_tbals3(" + prntAccntID + ",'" + balsDate +
              "','crdt_amount', " + rptSgmt1.ToString() +
          "," + rptSgmt2.ToString() +
          "," + rptSgmt3.ToString() +
          "," + rptSgmt4.ToString() +
          "," + rptSgmt5.ToString() +
          "," + rptSgmt6.ToString() +
          "," + rptSgmt7.ToString() +
          "," + rptSgmt8.ToString() +
          "," + rptSgmt9.ToString() +
          "," + rptSgmt10.ToString() + "), accb.get_ltst_prnt_accnt_tbals3(" + prntAccntID + ",'" + balsDate +
              "','net_amount', " + rptSgmt1.ToString() +
          "," + rptSgmt2.ToString() +
          "," + rptSgmt3.ToString() +
          "," + rptSgmt4.ToString() +
          "," + rptSgmt5.ToString() +
          "," + rptSgmt6.ToString() +
          "," + rptSgmt7.ToString() +
          "," + rptSgmt8.ToString() +
          "," + rptSgmt9.ToString() +
          "," + rptSgmt10.ToString() + ")";
            }
            //Global.mnFrm.cmCde.showSQLNoPermsn(strSql);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long getAccntDailyCurrBalsID(int accntID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            string strSql = "";
            strSql = "SELECT a.daily_cbals_id " +
          "FROM accb.accb_accnt_crncy_daily_bals a " +
          "WHERE(to_timestamp(a.as_at_date,'YYYY-MM-DD') =  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.accnt_id = " + accntID + ")";

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

        public static double getAccntLstDailyNetCurrBals(int accntID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            string strSql = "";
            strSql = "SELECT a.net_balance " +
          "FROM accb.accb_accnt_crncy_daily_bals a " +
          "WHERE(to_timestamp(a.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.accnt_id = " + accntID +
          ") ORDER BY to_timestamp(a.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";
            //Global.mnFrm.cmCde.showSQLNoPermsn(strSql);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0.00;
            }
        }

        public static double getAccntLstDailyCrdtCurrBals(int accntID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            string strSql = "";
            strSql = "SELECT a.crdt_bal " +
          "FROM accb.accb_accnt_crncy_daily_bals a " +
          "WHERE(to_timestamp(a.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.accnt_id = " + accntID + ") ORDER BY to_timestamp(a.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0.00;
            }
        }

        public static double getAccntLstDailyDbtCurrBals(int accntID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            string strSql = "";
            strSql = "SELECT a.dbt_bal " +
          "FROM accb.accb_accnt_crncy_daily_bals a " +
          "WHERE(to_timestamp(a.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.accnt_id = " + accntID + ") ORDER BY to_timestamp(a.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0.00;
            }
        }

        public static double getAccntDailyDbtCurrBals(int accntID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            string strSql = "";
            strSql = "SELECT a.dbt_bal " +
          "FROM accb.accb_accnt_crncy_daily_bals a " +
          "WHERE(to_timestamp(a.as_at_date,'YYYY-MM-DD') =  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.accnt_id = " + accntID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0.00;
            }
        }

        public static DataSet get_CurrBals_Prnt_Accnts(int prntAccntID, int CurrID)
        {
            string dtestr = Global.mnFrm.cmCde.getDB_Date_time();
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
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_CurrBals_Cntrl_Accnts(int cntrlAccntID, int CurrID)
        {
            string dtestr = Global.mnFrm.cmCde.getDB_Date_time();
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
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_CurrBals_Accnts(int accntID)
        {
            string dtestr = Global.mnFrm.cmCde.getDB_Date_time();
            string strSql = "";
            strSql = @"select  a.dbt_bal, a.crdt_bal, a.net_balance, to_char(to_timestamp(a.as_at_date,'YYYY-MM-DD'),'DD-Mon-YYYY') 
          from accb.accb_accnt_crncy_daily_bals a
          where a.accnt_id= " + accntID +
                @" and to_timestamp(a.as_at_date,'YYYY-MM-DD') <= to_timestamp('" + dtestr.Substring(0, 10) + @"','YYYY-MM-DD') 
          ORDER BY to_timestamp(a.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0;";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_One_Chrt_Det(int chrtID)
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
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_Accnt_Tot_Trns(long accntID)
        {
            string strSql = "";
            strSql = "SELECT count(1) " +
             "FROM accb.accb_trnsctn_details a " +
             "WHERE(a.accnt_id = " + accntID + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static long get_Accnt_Tot_Chldrn(long accntID)
        {
            string strSql = "";
            strSql = "SELECT count(1) " +
             "FROM accb.accb_chart_of_accnts a " +
             "WHERE(a.prnt_accnt_id = " + accntID + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }
        public static long get_Accnt_Tot_Mappngs(long accntID)
        {
            string strSql = "";
            strSql = "SELECT count(1) " +
             "FROM org.org_segment_values a " +
             "WHERE(a.mapped_grp_accnt_id = " + accntID + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }
        public static string getIsParentOrHsLedger(long accntID)
        {
            string strSql = "";
            strSql = "SELECT CASE WHEN a.is_prnt_accnt='1' THEN a.is_prnt_accnt ELSE a.has_sub_ledgers END " +
             "FROM accb.accb_chart_of_accnts a " +
             "WHERE(a.accnt_id = " + accntID + " and (a.is_prnt_accnt='1' or a.has_sub_ledgers='1'))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public static long get_Accnt_Tot_Pymnts(long accntID)
        {
            string strSql = "";
            strSql = "SELECT count(1) " +
             "FROM pay.pay_gl_interface a " +
             "WHERE(a.accnt_id = " + accntID + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static long get_Accnt_Tot_PyItms(long accntID)
        {
            string strSql = "";
            strSql = "SELECT count(1) " +
             "FROM org.org_pay_items a " +
             "WHERE(a.cost_accnt_id = " + accntID + " or a.bals_accnt_id = " + accntID + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static DataSet get_All_Chrt_Det(int orgid)
        {
            string strSql = "";
            strSql = @"SELECT a.accnt_id, a.debit_balance , a.credit_balance , a.net_balance ,
to_char(to_timestamp(a.balance_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') bsldte " +
              "FROM accb.accb_chart_of_accnts a WHERE a.org_id = " + orgid + " ORDER BY a.accnt_typ_id, a.report_line_no, a.accnt_num";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_One_SegmentDet(int segNum, int orgid)
        {
            string strSql = "";
            strSql = @"SELECT a.segment_id, a.segment_name_prompt, a.system_clsfctn, org.get_sgmnt_id(a.prnt_sgmnt_number)  
        FROM org.org_acnt_sgmnts a WHERE((a.org_id = " + orgid + " and a.segment_number = " + segNum + "))";
            //Global.mnFrm.orgDet_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_One_SegmentAcntVal(int segNum, int accountID)
        {
            string strSql = "";
            strSql = @"SELECT a.accnt_seg" + segNum + @"_val_id, org.get_sgmnt_val(a.accnt_seg" + segNum +
                @"_val_id), org.get_sgmnt_val_desc(a.accnt_seg" + segNum + @"_val_id)  
        FROM accb.accb_chart_of_accnts a  WHERE((a.accnt_id = " + accountID + "))";
            //Global.mnFrm.orgDet_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static int get_SegmnetsTtl(long orgid)
        {
            string strSql = @"SELECT no_of_accnt_sgmnts FROM org.org_details a  " +
             " WHERE((a.org_id = " + orgid + "))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static DataSet get_One_SgmntValDet(int segmentValID)
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

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.taxFrm.rec_SQL = strSql;
            return dtst;
        }

        public static string get_SegmnetsDlmtr(long orgid)
        {
            string strSql = @"SELECT segment_delimiter FROM org.org_details a  " +
             " WHERE((a.org_id = " + orgid + "))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            return "";
        }

        public static int getSgmntValID(string segmentVal, int segmentID)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "select segment_value_id from org.org_segment_values where lower(segment_value) = '" +
             segmentVal.Replace("'", "''").ToLower() + "' and segment_id = " + segmentID;
            dtSt = Global.mnFrm.cmCde.selectDataNoParams(sqlStr);
            if (dtSt.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtSt.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        public static int getAcctTypID(string accntTyp)
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

        public static DataSet get_Basic_ChrtDet(string searchWord, string searchIn,
          Int64 offset, int limit_size, int orgID, string mjrClsfctn, string mnrClsfctn)
        {
            string strSql = "";
            string whereCls = " and (accnt_num ilike '" + searchWord.Replace("'", "''") +
           "' or accnt_name ilike '" + searchWord.Replace("'", "''") +
           "')";
            string extrWhr = "";
            if (mnrClsfctn != "")
            {
                extrWhr += @" and z1.accnt_id IN (SELECT w.account_id
                       FROM accb.accb_account_clsfctns w
                       WHERE lower(w.maj_rpt_ctgry) = lower('" + mnrClsfctn + @"')
                             OR lower(w.min_rpt_ctgry) = lower('" + mnrClsfctn + @"'))";
            }
            if (mjrClsfctn != "")
            {
                extrWhr += @" and (select y.account_clsfctn from accb.accb_chart_of_accnts y where y.accnt_id=z1.accnt_id) ilike '" + mjrClsfctn + @"'";
            }
            string subSql = @"SELECT accnt_id,accnt_num,accnt_name,space||accnt_num||'.'||accnt_name account_number_name, is_prnt_accnt, accnt_type,accnt_typ_id, prnt_accnt_id, control_account_id, depth, path, cycle 
      FROM suborg z1 WHERE 1=1 " + whereCls + extrWhr + @" ORDER BY accnt_typ_id, path";

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
      FROM suborg z1 WHERE 1=1 " + extrWhr + @" ORDER BY accnt_typ_id, path";

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

            Global.mnFrm.chrt_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_Bdgt_ChrtDet(string searchWord, string searchIn,
          Int64 offset, int limit_size, int orgID, int lovID)
        {
            searchWord = searchWord.Replace(".", "%");
            string lovQry = Global.mnFrm.cmCde.getGnrlRecNm("gst.gen_stp_lov_names", "value_list_id", "sqlquery_if_dyn", lovID);
            lovQry = "(" + lovQry.Replace("{:prsn_id}", Global.mnFrm.cmCde.Prsn_id.ToString()) + ") xxtbl1 ";
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
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static double get_COA_dbtSum(int orgID)
        {
            string strSql = "";
            strSql = "SELECT SUM(a.debit_balance) " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE ((a.org_id = " + orgID + ") and (a.is_retained_earnings = '0') and (a.is_net_income = '0') and (a.control_account_id <=0))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return Math.Round(sumRes, 2);
        }

        public static double get_COA_crdtSum(int orgID)
        {
            string strSql = "";
            strSql = "SELECT SUM(a.credit_balance) " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE ((a.org_id = " + orgID + ") and (a.is_retained_earnings = '0') and (a.is_net_income = '0') and (a.control_account_id <=0) )";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return Math.Round(sumRes, 2);
        }

        public static double get_COA_NetSum(int orgID)
        {
            string strSql = "";
            strSql = "SELECT SUM(a.net_balance) " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE ((a.org_id = " + orgID + ") and (a.is_retained_earnings = '0') and (a.is_net_income = '0') and (a.control_account_id <=0) )";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return Math.Round(sumRes, 2);
        }

        public static double get_COA_CRLSum(int orgID)
        {
            string strSql = "";
            strSql = "SELECT SUM(a.net_balance) " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE ((a.org_id = " + orgID + ") and " +
              "(a.is_net_income = '0') and (a.control_account_id <=0) " +
              "and (a.accnt_type IN ('EQ','R', 'L')))";
            //(a.is_retained_earnings = '0') and 
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return Math.Round(sumRes, 2);
        }

        public static double get_COA_CLSum(int orgID)
        {
            string strSql = "";
            strSql = "SELECT SUM(a.net_balance) " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE ((a.org_id = " + orgID + ") and (a.control_account_id <=0) " +
              "and (a.accnt_type IN ('EQ', 'L')))";
            //(a.is_retained_earnings = '0') and and " +
            // "(a.is_net_income = '0')  
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return Math.Round(sumRes, 2);
        }

        public static double get_COA_AESum(int orgID)
        {
            string strSql = "";
            strSql = "SELECT SUM(a.net_balance) " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE ((a.org_id = " + orgID + ") and " +
              "(a.is_net_income = '0') and (a.has_sub_ledgers !='1') " +
              "and (a.accnt_type IN ('A','EX')))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return Math.Round(sumRes, 2);
        }

        public static double get_COA_ASum(int orgID)
        {
            string strSql = "";
            strSql = "SELECT SUM(a.net_balance) " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE ((a.org_id = " + orgID + ") and (a.has_sub_ledgers !='1') " +
              "and (a.accnt_type IN ('A')))";
            //(a.is_retained_earnings = '0') 
            /*and " +
              "(a.is_net_income = '0') */
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return Math.Round(sumRes, 2);
        }

        public static string getMinUnpstdTrnsDte(int orgid)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "select to_char(to_timestamp(min(a.trnsctn_date),'YYYY-MM-DD HH24:MI:SS')-interval '5 day','DD-Mon-YYYY 00:00:00') " +
                "from accb.accb_trnsctn_details a, accb.accb_trnsctn_batches b " +
                "where a.batch_id = b.batch_id and (b.creation_date >= to_char(now()-interval '5 day','YYYY-MM-DD HH24:MI:SS') or a.trns_status = '0') and b.org_id=" + orgid;

            dtSt = Global.mnFrm.cmCde.selectDataNoParams(sqlStr);
            if (dtSt.Tables[0].Rows.Count > 0)
            {
                return dtSt.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        public static DataSet pg_Post_Gl_Trns(long gl_btchid, long usrID, string run_date, int orgID, int p_msgid, string p_is_bulk_run)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            //System.Threading.Thread.Sleep(2000);
            string selSQL = "select accb.post_gl_trns(" + gl_btchid + "," + usrID +
                ", to_char(now(), 'YYYY-MM-DD HH24:MI:SS')," + orgID + ", " + p_msgid + ", '" + p_is_bulk_run + "')";
            return Global.mnFrm.cmCde.selectDataNoParams(selSQL);
        }

        public static DataSet pg_CorrectImblnsProcess(string asatdate, int orgID, long usrID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            //System.Threading.Thread.Sleep(2000);
            string selSQL = "select accb.correctimblnsprocess('" + asatdate + "'," + orgID + "," + usrID + ")";
            return Global.mnFrm.cmCde.selectDataNoParams(selSQL);
        }
        public static DataSet get_WrongNetBalncs(int orgID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            //System.Threading.Thread.Sleep(2000);
            string selSQL = @"select a.transctn_id, a.accnt_id, b.accnt_type, 
                a.transaction_desc, a.trnsctn_date, 
                a.dbt_amount, a.crdt_amount, a.net_amount, CASE WHEN b.accnt_type='A' or b.accnt_type='EX'  
               THEN (dbt_amount-crdt_amount)  
               ELSE (crdt_amount-dbt_amount) END actual_net 
               from accb.accb_trnsctn_details a, accb.accb_chart_of_accnts b
            where a.accnt_id=b.accnt_id and a.trns_status='1' and b.org_id=" + orgID + @"
            and CASE WHEN b.accnt_type='A' or b.accnt_type='EX'  
               THEN (dbt_amount-crdt_amount)  
               ELSE (crdt_amount-dbt_amount) END <> (net_amount)";
            return Global.mnFrm.cmCde.selectDataNoParams(selSQL);
        }

        public static DataSet get_WrongBalncs(int orgID, string trnsWthDateAfta)
        {
            string selSQL = @"SELECT * FROM (SELECT a.daily_bals_id, a.accnt_id, b.accnt_name, b.accnt_type, 
    round(accb.get_accnt_trnsSum(a.accnt_id,'dbt_amount',as_at_date||' 23:59:59'),2)-a.dbt_bal nw_dbbt_diff, 
    round(accb.get_accnt_trnsSum(a.accnt_id,'crdt_amount',as_at_date||' 23:59:59'),2)-a.crdt_bal nw_crdt_diff,
    round(accb.get_accnt_trnsSum(a.accnt_id,'net_amount',as_at_date||' 23:59:59'),2)-a.net_balance nw_net_diff, 
    to_char(to_timestamp(a.as_at_date||' 23:59:00','YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') trns_date
    FROM accb.accb_accnt_daily_bals a, accb.accb_chart_of_accnts b 
      where a.accnt_id=b.accnt_id and b.org_id=" + orgID + @" and b.is_net_income!='1' and b.has_sub_ledgers!='1'  
      and a.as_at_date ='" + trnsWthDateAfta.Replace("'", "''") +
      @"' ORDER BY a.as_at_date ASC) tbl1 WHERE tbl1.nw_dbbt_diff !=0 or tbl1.nw_crdt_diff !=0 or tbl1.nw_net_diff !=0";
            //  and b.is_retained_earnings!='1'
            /*a.as_at_date=(SELECT MAX(as_at_date)
      FROM accb.accb_accnt_daily_bals d
      where d.accnt_id=a.accnt_id)*/
            return Global.mnFrm.cmCde.selectDataNoParams(selSQL);
        }

        public static DataSet get_WrongHsSubLdgrBalncs(int orgID, string trnsWthDateAfta)
        {
            string selSQL = @"SELECT * FROM (SELECT a.daily_bals_id, a.accnt_id, b.accnt_name, b.accnt_type, 
    round(accb.get_accnt_trnsSum(a.accnt_id,'dbt_amount',as_at_date||' 23:59:59'),2)-a.dbt_bal nw_dbbt_diff, 
    round(accb.get_accnt_trnsSum(a.accnt_id,'crdt_amount',as_at_date||' 23:59:59'),2)-a.crdt_bal nw_crdt_diff,
    round(accb.get_accnt_trnsSum(a.accnt_id,'net_amount',as_at_date||' 23:59:59'),2)-a.net_balance nw_net_diff, 
    to_char(to_timestamp(a.as_at_date||' 23:59:00','YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') trns_date
    FROM accb.accb_accnt_daily_bals a, accb.accb_chart_of_accnts b 
      where a.accnt_id=b.accnt_id and b.org_id=" + orgID + @" and b.is_net_income!='1' and b.has_sub_ledgers ='1'  
      and a.as_at_date=(SELECT MAX(as_at_date)
      FROM accb.accb_accnt_daily_bals d
      where d.accnt_id=a.accnt_id) ORDER BY a.as_at_date ASC) tbl1 WHERE tbl1.nw_dbbt_diff !=0 or tbl1.nw_crdt_diff !=0 or tbl1.nw_net_diff !=0";
            //  and b.is_retained_earnings!='1'
            /*and a.as_at_date >='" + trnsWthDateAfta.Replace("'", "''") +
      @"'*/
            return Global.mnFrm.cmCde.selectDataNoParams(selSQL);
        }

        public static DataSet get_WrongNetIncmBalncs(int orgID, string trnsWthDateAfta)
        {
            string selSQL = @"SELECT a.daily_bals_id, a.accnt_id, b.accnt_name, b.accnt_type, 
round(accb.get_accnttype_trnsSum(" + orgID + @",'R','dbt_amount',as_at_date||' 23:59:59'),2)+round(accb.get_accnttype_trnsSum(" + orgID + @",'EX','dbt_amount',as_at_date||' 23:59:59'),2)-a.dbt_bal nw_dbbt_diff, 
round(accb.get_accnttype_trnsSum(" + orgID + @",'R','crdt_amount',as_at_date||' 23:59:59'),2)+round(accb.get_accnttype_trnsSum(" + orgID + @",'EX','crdt_amount',as_at_date||' 23:59:59'),2)-a.crdt_bal nw_crdt_diff,
round(accb.get_accnttype_trnsSum(" + orgID + @",'R','net_amount',as_at_date||' 23:59:59'),2)-round(accb.get_accnttype_trnsSum(" + orgID + @",'EX','net_amount',as_at_date||' 23:59:59'),2)-a.net_balance nw_net_diff, 
to_char(to_timestamp(a.as_at_date||' 23:59:00','YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') trns_date 
FROM accb.accb_accnt_daily_bals a, accb.accb_chart_of_accnts b 
  where a.accnt_id=b.accnt_id and b.org_id=" + orgID + @" and b.is_net_income='1' and b.has_sub_ledgers!='1'
  and a.as_at_date ='" + trnsWthDateAfta.Replace("'", "''") + @"' ORDER BY a.as_at_date ASC";
            //  and b.is_retained_earnings!='1'
            /*(SELECT MAX(as_at_date)
  FROM accb.accb_accnt_daily_bals d
  where d.accnt_id=a.accnt_id)*/
            return Global.mnFrm.cmCde.selectDataNoParams(selSQL);
        }

        public static long get_Total_Chrts(string searchWord, string searchIn, int orgID, string mjrClsfctn, string mnrClsfctn)
        {
            string strSql = "";
            string whereCls = " and (accnt_num ilike '" + searchWord.Replace("'", "''") +
           "' or accnt_name ilike '" + searchWord.Replace("'", "''") +
           "')";
            string extrWhr = "";
            if (mnrClsfctn != "")
            {
                extrWhr += @" and z1.accnt_id IN (SELECT w.account_id
                       FROM accb.accb_account_clsfctns w
                       WHERE lower(w.maj_rpt_ctgry) = lower('" + mnrClsfctn + @"')
                             OR lower(w.min_rpt_ctgry) = lower('" + mnrClsfctn + @"'))";
            }
            if (mjrClsfctn != "")
            {
                extrWhr += @" and (select y.account_clsfctn from accb.accb_chart_of_accnts y where y.accnt_id=z1.accnt_id) ilike '" + mjrClsfctn + @"'";
            }
            string subSql = @"SELECT count(1) 
      FROM suborg z1 WHERE 1=1 " + whereCls + extrWhr + @"";

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
      FROM suborg z1 WHERE 1=1" + extrWhr;

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

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static string get_Chrt_Rec_Hstry(int chrtID)
        {
            string strSQL = @"SELECT a.created_by, 
to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
a.last_update_by, 
to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM accb.accb_chart_of_accnts a WHERE(a.accnt_id  = " + chrtID + ")";
            string fnl_str = "";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSQL);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                fnl_str = "CREATED BY: " + Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][0].ToString())) +
                  "\r\nCREATION DATE: " + dtst.Tables[0].Rows[0][1].ToString() + "\r\nLAST UPDATE BY:" +
                  Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][2].ToString())) +
                  "\r\nLAST UPDATE DATE: " + dtst.Tables[0].Rows[0][3].ToString();
                return fnl_str;
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region "INFORMATIONAL/MEMO ACCOUNTS..."
        public static DataSet get_IMA_Trns(string searchWord, string searchIn,
       Int64 offset, int limit_size, string dte1, string dte2,
         decimal lowVal, decimal highVal)
        {
            //, int accntID
            dte1 = DateTime.ParseExact(
         dte1, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            dte2 = DateTime.ParseExact(
         dte2, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";
            string whereCls = "";
            if (searchIn == "Account Number")
            {
                whereCls = " AND (b.memo_accnt_num ilike '" + searchWord.Replace("'", "''") +
              "')";
            }
            else if (searchIn == "Account Name")
            {
                whereCls = " AND (b.memo_accnt_name ilike '" + searchWord.Replace("'", "''") +
              "')";
            }
            else if (searchIn == "Transaction Description")
            {
                whereCls = " AND (a.trns_desc ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Transaction Date")
            {
                whereCls = " AND (to_char(to_timestamp(a.trns_date,'YYYY-MM-DD HH24:MI:SS'), " +
                  "'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") + "')";
            }

            string amntCls = "";
            if (lowVal != 0 || highVal != 0)
            {
                amntCls = " and ((dbt_amount !=0 and dbt_amount between " + lowVal + " and " + highVal +
                  ") or (crdt_amount !=0 and crdt_amount between " + lowVal + " and " + highVal + "))";
            }

            strSql = @"SELECT a.memo_trns_id, b.memo_accnt_num, b.memo_accnt_name, a.trns_desc, a.dbt_amount, 
a.crdt_amount, to_char(to_timestamp(a.trns_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
a.crncy_id, a.memo_accnt_id, a.net_amount, a.trns_status,  " +
         " a.entered_amnt, gst.get_pssbl_val(a.crncy_id) " +
            "FROM accb.accb_memo_accnt_trns a LEFT OUTER JOIN " +
            "accb.accb_memo_accounts b on a.memo_accnt_id = b.memo_accnt_id " +
            "WHERE((to_timestamp(a.trns_date,'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + dte1 +
            "','YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + dte2 + "','YYYY-MM-DD HH24:MI:SS'))" + whereCls + amntCls + ") " +
            "ORDER BY to_timestamp(a.trns_date,'YYYY-MM-DD HH24:MI:SS') DESC LIMIT " + limit_size +
                " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            //(b.memo_accnt_id = " + accntID + ") and 
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.imadt_SQL = strSql;
            return dtst;
        }

        public static long get_Total_IMA_Trns(string searchWord, string searchIn
         , string dte1, string dte2,
         decimal lowVal, decimal highVal)
        {
            //, int accntID
            dte1 = DateTime.ParseExact(
         dte1, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            dte2 = DateTime.ParseExact(
         dte2, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";
            string whereCls = "";
            if (searchIn == "Account Number")
            {
                whereCls = " AND (b.memo_accnt_num ilike '" + searchWord.Replace("'", "''") +
              "')";
            }
            else if (searchIn == "Account Name")
            {
                whereCls = " AND (b.memo_accnt_name ilike '" + searchWord.Replace("'", "''") +
              "')";
            }
            else if (searchIn == "Transaction Description")
            {
                whereCls = " AND (a.trns_desc ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Transaction Date")
            {
                whereCls = " AND (to_char(to_timestamp(a.trns_date,'YYYY-MM-DD HH24:MI:SS'), " +
                  "'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") + "')";
            }


            string amntCls = "";
            if (lowVal != 0 || highVal != 0)
            {
                amntCls = " and ((dbt_amount !=0 and dbt_amount between " + lowVal + " and " + highVal +
                  ") or (crdt_amount !=0 and crdt_amount between " + lowVal + " and " + highVal + "))";
            }

            strSql = @"SELECT count(1) " +
            "FROM accb.accb_memo_accnt_trns a LEFT OUTER JOIN " +
            "accb.accb_memo_accounts b on a.memo_accnt_id = b.memo_accnt_id " +
            "WHERE((to_timestamp(a.trns_date,'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + dte1 +
            "','YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + dte2 + "','YYYY-MM-DD HH24:MI:SS'))" + whereCls + amntCls + ") "; ;
            //(b.memo_accnt_id = " + accntID + ") and 
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            long sumRes = 0;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                long.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static DataSet get_Basic_IMADet(int accntID)
        {
            string strSql = "";

            strSql = @"SELECT a.memo_accnt_id, a.memo_accnt_num, a.memo_accnt_name, a.memo_accnt_desc, a.is_enabled, a.accnt_type,
a.accnt_type_id, a.crncy_id, a.dflt_cost_accnt_id, a.dflt_bals_accnt_id, a.uses_sql, a.sql_formular, a.is_contra
       FROM accb.accb_memo_accounts a " +
         "WHERE ((a.memo_accnt_id=" + accntID + "))";

            //Global.mnFrm.ima_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static string[] getIMALstDailyBalsInfo(int accntID, string balsDate)
        {
            string dateStr = balsDate;
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);

            string strSql = "";
            strSql = @"SELECT a.dbt_bal, a.crdt_bal, a.net_balance, 
to_char(to_timestamp(a.as_at_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
          "FROM accb.accb_memo_accnt_daily_bals a " +
          "WHERE(to_timestamp(a.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.accnt_id = " + accntID +
          ") ORDER BY to_timestamp(a.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            string[] rslt = { "0", "0", "0", dateStr };
            if (dtst.Tables[0].Rows.Count > 0)
            {
                rslt[0] = dtst.Tables[0].Rows[0][0].ToString();
                rslt[1] = dtst.Tables[0].Rows[0][1].ToString();
                rslt[2] = dtst.Tables[0].Rows[0][2].ToString();
                rslt[3] = dtst.Tables[0].Rows[0][3].ToString();
                return rslt;
            }
            else
            {
                return rslt;
            }
        }

        public static DataSet get_Basic_IMA(string searchWord, string searchIn,
      Int64 offset, int limit_size, int orgID)
        {
            string strSql = "";
            string wherCls = "";
            /*Account Description
         Account Name
         Account Number*/
            if (searchIn == "Account Description")
            {
                wherCls = "(a.memo_accnt_desc ilike '" + searchWord.Replace("'", "''") +
            "') AND ";
            }
            else if (searchIn == "Account Name")
            {
                wherCls = "(a.memo_accnt_name ilike '" + searchWord.Replace("'", "''") +
            "') AND ";
            }
            else if (searchIn == "Account Number")
            {
                wherCls = "(a.memo_accnt_num ilike '" + searchWord.Replace("'", "''") +
            "') AND ";
            }
            strSql = @"SELECT a.memo_accnt_id, a.memo_accnt_num || '.' || a.memo_accnt_name
       FROM accb.accb_memo_accounts a " +
         "WHERE (" + wherCls + "(org_id = " + orgID + ")) ORDER BY a.accnt_type_id,a.memo_accnt_num LIMIT " + limit_size +
         " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            Global.mnFrm.ima_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_Total_IMA(string searchWord, string searchIn, int orgID)
        {
            string strSql = "";
            string wherCls = "";
            if (searchIn == "Account Description")
            {
                wherCls = "(a.memo_accnt_desc ilike '" + searchWord.Replace("'", "''") +
            "') AND ";
            }
            else if (searchIn == "Account Name")
            {
                wherCls = "(a.memo_accnt_name ilike '" + searchWord.Replace("'", "''") +
            "') AND ";
            }
            else if (searchIn == "Account Number")
            {
                wherCls = "(a.memo_accnt_num ilike '" + searchWord.Replace("'", "''") +
            "') AND ";
            }
            strSql = @"SELECT count(1)
       FROM accb.accb_memo_accounts a " +
         "WHERE (" + wherCls + "(org_id = " + orgID + "))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static string get_IMA_Rec_Hstry(int hdrID)
        {
            string strSQL = @"SELECT a.created_by, 
to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'), 'DD-Mon-YYYY  HH24:MI:SS'), 
      a.last_update_by, 
      to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY  HH24:MI:SS') " +
            "FROM accb.accb_memo_accounts a WHERE(a.memo_accnt_id = " + hdrID + ")";
            string fnl_str = "";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSQL);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                fnl_str = "CREATED BY: " + Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][0].ToString())) +
                 "\r\nCREATION DATE: " + dtst.Tables[0].Rows[0][1].ToString() + "\r\nLAST UPDATE BY:" +
                 Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][2].ToString())) +
                 "\r\nLAST UPDATE DATE: " + dtst.Tables[0].Rows[0][3].ToString();
                return fnl_str;
            }
            else
            {
                return "";
            }
        }

        public static string get_IMADT_Rec_Hstry(int dteID)
        {
            string strSQL = @"SELECT a.created_by, 
to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY  HH24:MI:SS'), 
      a.last_update_by, 
      to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY  HH24:MI:SS') " +
            "FROM accb.accb_memo_accnt_trns a WHERE(a.memo_trns_id = " + dteID + ")";
            string fnl_str = "";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSQL);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                fnl_str = "CREATED BY: " + Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][0].ToString())) +
                 "\r\nCREATION DATE: " + dtst.Tables[0].Rows[0][1].ToString() + "\r\nLAST UPDATE BY:" +
                 Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][2].ToString())) +
                 "\r\nLAST UPDATE DATE: " + dtst.Tables[0].Rows[0][3].ToString();
                return fnl_str;
            }
            else
            {
                return "";
            }
        }

        public static void deleteIMA(long hdrID, string imaNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Memo Account Name = " + imaNm;
            string delSQL = "DELETE FROM accb.accb_memo_accnt_trns WHERE memo_accnt_id = " + hdrID;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);

            delSQL = "DELETE FROM accb.accb_memo_accounts WHERE memo_accnt_id = " + hdrID;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteIMALn(long imaLnid, string trsDesc)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "IMA Line Description = " + trsDesc;
            string delSQL = "DELETE FROM accb.accb_memo_accnt_trns WHERE memo_trns_id = " + imaLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static int getIMAAccntID(string imaname, int orgid)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "select memo_accnt_id from accb.accb_memo_accounts where (lower(memo_accnt_name) = '" +
             imaname.Replace("'", "''").ToLower() + "' or lower(memo_accnt_num) = '" +
             imaname.Replace("'", "''").ToLower() + "') and org_id = " + orgid;
            dtSt = Global.mnFrm.cmCde.selectDataNoParams(sqlStr);
            if (dtSt.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtSt.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        public static void createIMAHdr(int orgid, string imanum, string imaname,
      string imadesc, string accntType, int acntTypID, int crncyID, int dfltCostAcntID, int dfltBalsAcntID,
         bool usSQL, string sqlFrmlr, bool isEnbld, bool isCntra)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_memo_accounts(
            memo_accnt_num, memo_accnt_name, memo_accnt_desc, 
            is_enabled, org_id, accnt_type, accnt_type_id, crncy_id, dflt_cost_accnt_id, 
            dflt_bals_accnt_id, uses_sql, sql_formular, created_by, creation_date, 
            last_update_by, last_update_date, is_contra) " +
                  "VALUES ('" + imanum.Replace("'", "''") +
                  "','" + imaname.Replace("'", "''") +
                  "', '" + imadesc.Replace("'", "''") +
                  "', '" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(isEnbld) +
                  "', " + orgid + ", '" + accntType.Replace("'", "''") +
                  "', " + acntTypID + ", " + crncyID + ", " + dfltCostAcntID +
                  ", " + dfltBalsAcntID + ", '" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(usSQL) +
                  "','" + sqlFrmlr.Replace("'", "''") +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', '" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(isCntra) +
                  "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updateIMAHdr(int acntID, string imanum, string imaname,
      string imadesc, string accntType, int acntTypID, int crncyID, int dfltCostAcntID, int dfltBalsAcntID,
         bool usSQL, string sqlFrmlr, bool isEnbld, bool isCntra)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_memo_accounts SET " +
                  "memo_accnt_num='" + imanum.Replace("'", "''") +
                  "', memo_accnt_name='" + imaname.Replace("'", "''") +
                  "', memo_accnt_desc='" + imadesc.Replace("'", "''") +
                  "', accnt_type='" + accntType.Replace("'", "''") +
                  "', accnt_type_id=" + acntTypID + ", crncy_id=" + crncyID + ", " +
                "dflt_cost_accnt_id=" + dfltCostAcntID + ", dflt_bals_accnt_id=" + dfltBalsAcntID +
                ", last_update_by=" + Global.myBscActn.user_id + ", " +
                  "last_update_date='" + dateStr +
                  "', is_enabled='" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(isEnbld) +
                  "', uses_sql='" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(usSQL) +
                  "', sql_formular='" + sqlFrmlr.Replace("'", "''") +
                  "', is_contra='" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(isCntra) +
                  "' " +
                  "WHERE (memo_accnt_id =" + acntID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void createIMALn(int acntID, int crncyID,
      string trsnDec, string trnsDte, bool trnsStatus, double entrdamnt,
         double dbtamnt, double crdtamnt, double netamnt, string dbtCrdt)
        {
            trnsDte = DateTime.ParseExact(
         trnsDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_memo_accnt_trns(
            memo_accnt_id, trns_desc, dbt_amount, crdt_amount, 
            net_amount, crncy_id, trns_status, entered_amnt, dbt_or_crdt, 
            created_by, creation_date, last_update_by, last_update_date, 
            trns_date) " +
                  "VALUES (" + acntID + ", '" + trsnDec.Replace("'", "''") +
                  "', " + dbtamnt + ", " + crdtamnt + ", " + netamnt + ", " + crncyID +
                  ", '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(trnsStatus) +
                  "', " + entrdamnt + ", '" + dbtCrdt.Replace("'", "''") +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', '" + trnsDte.Replace("'", "''") +
                  "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updateIMALn(int trnsID, int accntID, int crncyID,
      string trsnDec, string trnsDte, bool trnsStatus, double entrdamnt,
         double dbtamnt, double crdtamnt, double netamnt, string dbtCrdt)
        {
            trnsDte = DateTime.ParseExact(
         trnsDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_memo_accnt_trns SET 
            memo_accnt_id=" + accntID + ", trns_desc= " + trsnDec +
                  ",dbt_amount=" + dbtamnt +
                  ",crdt_amount=" + crdtamnt +
                  ",net_amount=" + netamnt +
                  ",entered_amnt=" + entrdamnt +
                  ", trns_date='" + trnsDte.Replace("'", "''") +
                  "', trns_status='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(trnsStatus) +
                  "', dbt_or_crdt='" + dbtCrdt.Replace("'", "''") + "', last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "' " +
                  "WHERE memo_trns_id=" + trnsID + " ";
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        #endregion

        #region "ACCOUNT TRANSACTIONS DETAILS..."
        public static double get_LtstExchRate(string fromCurr, string toCurr, string asAtDte)
        {
            int funccurid = Global.mnFrm.cmCde.getOrgFuncCurID(Global.mnFrm.cmCde.Org_id);
            string funccurCode = Global.mnFrm.cmCde.getPssblValNm(funccurid);
            string strSql = "";
            strSql = @"SELECT CASE WHEN a.currency_from='" + fromCurr.Replace("'", "''") +
              @"' THEN a.multiply_from_by ELSE (1/a.multiply_from_by) END
      FROM accb.accb_exchange_rates a WHERE ((a.currency_from='" + fromCurr.Replace("'", "''") +
              @"' and a.currency_to='" + toCurr.Replace("'", "''") +
              @"') or (a.currency_to='" + fromCurr.Replace("'", "''") +
              @"' and a.currency_from='" + toCurr.Replace("'", "''") +
              @"')) and to_timestamp(a.conversion_date,'YYYY-MM-DD') <= to_timestamp('" + asAtDte +
              "','DD-Mon-YYYY HH24:MI:SS') ORDER BY to_timestamp(a.conversion_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            if (fromCurr == toCurr)
            {
                return 1;
            }
            else if (fromCurr != funccurCode && toCurr != funccurCode)
            {
                double a = Global.get_LtstExchRate(fromCurr, funccurCode, asAtDte);
                double b = Global.get_LtstExchRate(toCurr, funccurCode, asAtDte);
                if (a != 0 && b != 0)
                {
                    return a / b;
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

        public static double get_LtstExchRate(int fromCurrID, int toCurrID, string asAtDte)
        {
            int fnccurid = Global.mnFrm.cmCde.getOrgFuncCurID(Global.mnFrm.cmCde.Org_id);
            //this.curCode = Global.mnFrm.cmCde.getPssblValNm(this.curid);

            string strSql = "";
            strSql = @"SELECT CASE WHEN a.currency_from_id=" + fromCurrID +
              @" THEN a.multiply_from_by ELSE (1/a.multiply_from_by) END
      FROM accb.accb_exchange_rates a WHERE ((a.currency_from_id=" + fromCurrID +
              @" and a.currency_to_id=" + toCurrID +
              @") or (a.currency_to_id=" + fromCurrID +
              @" and a.currency_from_id=" + toCurrID +
              @")) and to_timestamp(a.conversion_date,'YYYY-MM-DD') <= to_timestamp('" + asAtDte +
              "','DD-Mon-YYYY HH24:MI:SS') ORDER BY to_timestamp(a.conversion_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";
            //MessageBox.Show(strSql);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            if (fromCurrID == toCurrID)
            {
                return 1;
            }
            else if (fromCurrID != fnccurid && toCurrID != fnccurid)
            {
                double a = Global.get_LtstExchRate(fromCurrID, fnccurid, asAtDte);
                double b = Global.get_LtstExchRate(toCurrID, fnccurid, asAtDte);
                if (a != 0 && b != 0)
                {
                    return a / b;
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

        public static long getNewBatchID()
        {
            //string strSql = "select nextval('accb.accb_trnsctn_batches_batch_id_seq'::regclass);";
            string strSql = "select  last_value from accb.accb_trnsctn_batches_batch_id_seq";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString()) + 1;
            }
            return -1;
        }

        public static long get_TrnsCntBtwnDtes(int orgID, string dte1, string dte2)
        {
            string strSql = "";
            strSql = @"SELECT count(a.transctn_id)
      FROM (accb.accb_trnsctn_details a LEFT OUTER JOIN accb.accb_trnsctn_batches c ON a.batch_id = c.batch_id) " +
            "LEFT OUTER JOIN accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
            "WHERE((b.org_id = " + orgID + ") and (to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') <= to_timestamp('" + dte2 +
            "','YYYY-MM-DD HH24:MI:SS')) and (to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') >= to_timestamp('" + dte1 +
            "','YYYY-MM-DD HH24:MI:SS')))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }

        public static long get_TrnsCntB4Dte(int orgID, string dte1, bool isposted)
        {
            string strSql = "";
            strSql = @"SELECT count(a.transctn_id)
      FROM (accb.accb_trnsctn_details a LEFT OUTER JOIN accb.accb_trnsctn_batches c ON a.batch_id = c.batch_id) " +
            "LEFT OUTER JOIN accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
            "WHERE((b.org_id = " + orgID + ") and (a.trns_status = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isposted) +
            "') and (to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') <= to_timestamp('" + dte1 +
            "','YYYY-MM-DD HH24:MI:SS'))) ";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }

        public static long get_UnImprtdPayTrns(int orgID, string dte1)
        {
            string strSql = @"select count(a.interface_id) from pay.pay_gl_interface a, accb.accb_chart_of_accnts b 
      where ((a.accnt_id = b.accnt_id) and (a.gl_batch_id <=0) and (b.org_id = " + orgID +
            ") and (to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') <= to_timestamp('" + dte1 +
            "','YYYY-MM-DD HH24:MI:SS')))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }

        public static DataSet get_AccntStmntTransactions(int accntID, string dte1, string dte2,
          bool isposted, decimal lowVal, decimal highVal, bool showMnl, bool showUnbalcd, bool shwIntrfc)
        {
            dte1 = DateTime.ParseExact(
         dte1, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            dte2 = DateTime.ParseExact(
         dte2, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";
            string whereCls = "";
            if (showUnbalcd == true)
            {
                whereCls = @" AND (select ','||string_agg(tbl1.trnsids,',')||','
    FROM(SELECT abs(a.net_amount), count(a.transctn_id), string_agg('' || a.transctn_id, ',') trnsids, sum(a.net_amount)
    FROM(accb.accb_trnsctn_details a LEFT OUTER JOIN accb.accb_trnsctn_batches c ON a.batch_id = c.batch_id)
    LEFT OUTER JOIN accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id
    WHERE((b.accnt_id = " + accntID + " or b.prnt_accnt_id = " + accntID + " or b.control_account_id = " + accntID +
    @") and(trns_status = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isposted) +
            @"') and(to_timestamp(a.trnsctn_date, 'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + dte1 +
                "','YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + dte2 + @"','YYYY-MM-DD HH24:MI:SS')))
    GROUP BY 1
    HAVING sum(a.net_amount)!= 0
    ORDER BY abs(a.net_amount) DESC) tbl1) like '%,' || a.transctn_id || ',%'";
            }
            /* mod(count(a.transctn_id), 2) != 0*/
            if (showMnl == true)
            {
                whereCls = @" AND COALESCE((select min(z.transctn_id) from accb.accb_trnsctn_details z 
where a.net_amount=-1*z.net_amount and a.trnsctn_date<=z.trnsctn_date and a.transctn_id<z.transctn_id 
 and a.accnt_id=z.accnt_id and z.trns_status = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isposted) +
            @"' and a.net_amount>=0 and (to_timestamp(z.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') between 
to_timestamp('" + dte1 + "','YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + dte2 + @"','YYYY-MM-DD HH24:MI:SS'))),-1)<=0
  AND COALESCE((select min(z.transctn_id) from accb.accb_trnsctn_details z 
where a.net_amount=-1*z.net_amount and a.trnsctn_date>=z.trnsctn_date and a.transctn_id>z.transctn_id 
 and a.accnt_id=z.accnt_id and z.trns_status = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isposted) +
            @"' and a.net_amount<0 and (to_timestamp(z.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + dte1 +
            "','YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + dte2 + @"','YYYY-MM-DD HH24:MI:SS'))),-1)<=0";
            }
            string amntCls = "";
            if (lowVal != 0 || highVal != 0)
            {
                amntCls = " and ((dbt_amount !=0 and dbt_amount between " + lowVal + " and " + highVal +
                  ") or (crdt_amount !=0 and crdt_amount between " + lowVal + " and " + highVal + "))";
            }
            if (shwIntrfc)
            {
                strSql = @"SELECT a.transctn_id, b.accnt_num, b.accnt_name, 
COALESCE(y.transaction_desc, COALESCE(y1.transaction_desc, COALESCE(y2.transaction_desc, COALESCE(y3.transaction_desc, a.transaction_desc)))), 
COALESCE(y.dbt_amount, COALESCE(y1.dbt_amount, COALESCE(y2.dbt_amount, COALESCE(y3.dbt_amount, a.dbt_amount)))), 
COALESCE(y.crdt_amount, COALESCE(y1.crdt_amount, COALESCE(y2.crdt_amount, COALESCE(y3.crdt_amount, a.crdt_amount)))), 
to_char(to_timestamp(COALESCE(y.trnsctn_date, COALESCE(y1.trnsctn_date, COALESCE(y2.trnsctn_date, COALESCE(y3.trnsctn_date, a.trnsctn_date)))),'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
a.func_cur_id, a.batch_id, a.accnt_id,
COALESCE(y.net_amount, COALESCE(y1.net_amount, COALESCE(y2.net_amount, COALESCE(y3.net_amount, a.net_amount)))), 
c.batch_name, a.trns_status, c.batch_source, " +
             " a.entered_amnt, gst.get_pssbl_val(a.entered_amt_crncy_id), a.entered_amt_crncy_id, " +
                        @"a.accnt_crncy_amnt, gst.get_pssbl_val(a.accnt_crncy_id), a.accnt_crncy_id, a.func_cur_exchng_rate, a.accnt_cur_exchng_rate, 
c.batch_name btch_nm, a.ref_doc_number, a.is_reconciled, a.dbt_or_crdt, " +
          "c.batch_vldty_status, c.src_batch_id " +
                "FROM (accb.accb_trnsctn_details a " +
                "LEFT OUTER JOIN accb.accb_trnsctn_batches c ON a.batch_id = c.batch_id) " +
                "LEFT OUTER JOIN accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
                "LEFT OUTER JOIN mcf.mcf_gl_interface y on (a.accnt_id = y.accnt_id and y.gl_batch_id=a.batch_id and (c.batch_source ilike '%Banking%') and a.source_trns_ids like '%,' || y.interface_id || ',%') " +
                "LEFT OUTER JOIN vms.vms_gl_interface y1 on (a.accnt_id = y1.accnt_id and y1.gl_batch_id=a.batch_id and c.batch_source ilike '%Vault Management%' and a.source_trns_ids like '%,' || y1.interface_id || ',%') " +
                "LEFT OUTER JOIN pay.pay_gl_interface y2 on (a.accnt_id = y2.accnt_id and y2.gl_batch_id=a.batch_id and c.batch_source ilike '%Internal Payments%' and a.source_trns_ids like '%,' || y2.interface_id || ',%') " +
                "LEFT OUTER JOIN scm.scm_gl_interface y3 on (a.accnt_id = y3.accnt_id and y3.gl_batch_id=a.batch_id and c.batch_source ilike '%Inventory%' and a.source_trns_ids like '%,' || y3.interface_id || ',%') " +
                "WHERE((b.accnt_id = " + accntID + " or b.prnt_accnt_id=" + accntID + " or b.control_account_id=" + accntID + ") and (trns_status = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isposted) +
                "') and (to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + dte1 +
                "','YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + dte2 + "','YYYY-MM-DD HH24:MI:SS'))" + whereCls + amntCls + ") " +
                "ORDER BY to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') ASC, a.transctn_id ";
            }
            else
            {
                strSql = @"SELECT a.transctn_id, b.accnt_num, b.accnt_name, a.transaction_desc, a.dbt_amount, 
a.crdt_amount, to_char(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
a.func_cur_id, a.batch_id, a.accnt_id, a.net_amount, c.batch_name, a.trns_status, c.batch_source, " +
             " a.entered_amnt, gst.get_pssbl_val(a.entered_amt_crncy_id), a.entered_amt_crncy_id, " +
                        @"a.accnt_crncy_amnt, gst.get_pssbl_val(a.accnt_crncy_id), a.accnt_crncy_id, a.func_cur_exchng_rate, a.accnt_cur_exchng_rate, 
c.batch_name btch_nm, a.ref_doc_number, a.is_reconciled, a.dbt_or_crdt, " +
          "c.batch_vldty_status, c.src_batch_id " +
                "FROM (accb.accb_trnsctn_details a LEFT OUTER JOIN accb.accb_trnsctn_batches c ON a.batch_id = c.batch_id) " +
                "LEFT OUTER JOIN accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
                "WHERE((b.accnt_id = " + accntID + " or b.prnt_accnt_id=" + accntID + " or b.control_account_id=" + accntID + ") and (trns_status = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isposted) +
                "') and (to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + dte1 +
                "','YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + dte2 + "','YYYY-MM-DD HH24:MI:SS'))" + whereCls + amntCls + ") " +
                "ORDER BY to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') ASC, a.transctn_id ";
            }
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.mnFrm.vwsrchSQLStmnt = strSql;
            //Global.mnFrm.bls_SQL = strSql;
            Global.mnFrm.accntStmntSQL = strSql;
            return dtst;
        }

        public static DataSet get_AccntClassStmntTrns(string accntClass, string dte1, string dte2,
          bool isposted, decimal lowVal, decimal highVal)
        {
            dte1 = DateTime.ParseExact(dte1, "dd-MMM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            dte2 = DateTime.ParseExact(
         dte2, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";
            string whereCls = "";

            string amntCls = "";
            if (lowVal != 0 || highVal != 0)
            {
                amntCls = " and ((dbt_amount !=0 and dbt_amount between " + lowVal + " and " + highVal +
                  ") or (crdt_amount !=0 and crdt_amount between " + lowVal + " and " + highVal + "))";
            }

            strSql = @"SELECT distinct a.transctn_id, b.accnt_num, b.accnt_name, a.transaction_desc, a.dbt_amount, 
        a.crdt_amount, to_char(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
        a.func_cur_id, a.batch_id, a.accnt_id, a.net_amount, c.batch_name, a.trns_status, c.batch_source, " +
         " a.entered_amnt, gst.get_pssbl_val(a.entered_amt_crncy_id), a.entered_amt_crncy_id, " +
                    @"a.accnt_crncy_amnt, gst.get_pssbl_val(a.accnt_crncy_id), a.accnt_crncy_id, a.func_cur_exchng_rate, a.accnt_cur_exchng_rate, 
        c.batch_name btch_nm, a.ref_doc_number, a.is_reconciled, a.dbt_or_crdt, " +
      "c.batch_vldty_status, c.src_batch_id, a.trnsctn_date " +
            "FROM (accb.accb_trnsctn_details a LEFT OUTER JOIN accb.accb_trnsctn_batches c ON a.batch_id = c.batch_id) " +
            "LEFT OUTER JOIN accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
            "WHERE((b.account_clsfctn = '" + accntClass.Replace("'", "''") + "' and b.is_prnt_accnt='0' and b.has_sub_ledgers='0') " +
            "and (trns_status = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isposted) +
            "') and (to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + dte1 +
            "','YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + dte2 + "','YYYY-MM-DD HH24:MI:SS'))" + whereCls + amntCls + ") " +
            "ORDER BY a.trnsctn_date ASC, a.transctn_id ";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.accntCshBkStmntSQL = strSql;
            return dtst;
        }

        public static DataSet get_AccntClassAccounts(string accntClass)
        {
            string strSql = "";
            string whereCls = "";

            string amntCls = "";
            strSql = @"SELECT b.accnt_id, b.accnt_num, b.accnt_name " +
            "FROM accb.accb_chart_of_accnts b " +
            "WHERE((b.account_clsfctn = '" + accntClass.Replace("'", "''") + "' and b.is_prnt_accnt='0' and b.has_sub_ledgers='0') " +
            "" + whereCls + amntCls + ") " +
            "ORDER BY b.accnt_num ";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static void changeReconciledStatus(long trnsID, string nwStatus)
        {
            if (trnsID <= 0)
            {
                return;
            }
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string updtSQL = "UPDATE accb.accb_trnsctn_details SET is_reconciled = '" +
              nwStatus.Replace("'", "''") + "' WHERE transctn_id=" + trnsID + " or src_trns_id_reconciled = " + trnsID;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static DataSet get_Transactions(string searchWord, string searchIn,
        Int64 offset, int limit_size, int orgID, string dte1, string dte2,
          bool isposted, decimal lowVal, decimal highVal, string orderBy)
        {
            /*Date (ASC)
      Date (DESC)*/
            string ordrCls = "ORDER BY to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') DESC";
            if (orderBy == "Date (ASC)")
            {
                ordrCls = "ORDER BY to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS')";
            }

            dte1 = DateTime.ParseExact(
         dte1, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            dte2 = DateTime.ParseExact(
         dte2, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";
            string whereCls = "";
            if (searchIn == "Account Number")
            {
                whereCls = " AND (b.accnt_num ilike '" + searchWord.Replace("'", "''") +
              "' or accb.get_accnt_num(b.control_account_id) ilike '" + searchWord.Replace("'", "''") +
              "' or b.accnt_name ilike '" + searchWord.Replace("'", "''") +
             "' or accb.get_accnt_name(b.control_account_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Account Name")
            {
                whereCls = " AND (b.accnt_num ilike '" + searchWord.Replace("'", "''") +
              "' or accb.get_accnt_num(b.control_account_id) ilike '" + searchWord.Replace("'", "''") +
              "' or b.accnt_name ilike '" + searchWord.Replace("'", "''") +
             "' or accb.get_accnt_name(b.control_account_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Transaction Description")
            {
                whereCls = " AND (a.transaction_desc ilike '" + searchWord.Replace("'", "''") +
                  "' or a.ref_doc_number ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Transaction Date")
            {
                whereCls = " AND (to_char(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS'), " +
                  "'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Batch Name")
            {
                whereCls = " AND (c.batch_name ilike '" + searchWord.Replace("'", "''") +
              "')";
            }

            string amntCls = "";
            if (lowVal != 0 || highVal != 0)
            {
                amntCls = " and ((dbt_amount !=0 and dbt_amount between " + lowVal + " and " + highVal +
                  ") or (crdt_amount !=0 and crdt_amount between " + lowVal + " and " + highVal + "))";
            }

            strSql = @"SELECT a.transctn_id, b.accnt_num, b.accnt_name, a.transaction_desc, a.dbt_amount, 
a.crdt_amount, to_char(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
a.func_cur_id, a.batch_id, a.accnt_id, a.net_amount, c.batch_name, a.trns_status, c.batch_source, " +
         " a.entered_amnt, gst.get_pssbl_val(a.entered_amt_crncy_id), a.entered_amt_crncy_id, " +
                    @"a.accnt_crncy_amnt, gst.get_pssbl_val(a.accnt_crncy_id), a.accnt_crncy_id, a.func_cur_exchng_rate, a.accnt_cur_exchng_rate, 
(select d.batch_name from accb.accb_trnsctn_batches d where d.batch_id = a.batch_id) btch_nm, a.ref_doc_number " +
            "FROM (accb.accb_trnsctn_details a LEFT OUTER JOIN accb.accb_trnsctn_batches c ON a.batch_id = c.batch_id) " +
            "LEFT OUTER JOIN accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
            "WHERE((b.org_id = " + orgID + ") and (trns_status = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isposted) +
            "') and (to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + dte1 +
            "','YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + dte2 + "','YYYY-MM-DD HH24:MI:SS'))" + whereCls + amntCls + ") " +
            ordrCls + " LIMIT " + limit_size +
                " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.vwsrchSQLStmnt = strSql;
            Global.mnFrm.bls_SQL = strSql;
            Global.mnFrm.chrtDet_SQL = strSql;
            return dtst;
        }

        public static long get_Total_Transactions(string searchWord, string searchIn,
        int orgID, string dte1, string dte2, bool isposted, decimal lowVal, decimal highVal)
        {
            dte1 = DateTime.ParseExact(
         dte1, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            dte2 = DateTime.ParseExact(
         dte2, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string strSql = "";
            string whereCls = "";
            if (searchIn == "Account Number")
            {
                whereCls = " AND (b.accnt_num ilike '" + searchWord.Replace("'", "''") +
              "' or accb.get_accnt_num(b.control_account_id) ilike '" + searchWord.Replace("'", "''") +
              "' or b.accnt_name ilike '" + searchWord.Replace("'", "''") +
             "' or accb.get_accnt_name(b.control_account_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Account Name")
            {
                whereCls = " AND (b.accnt_num ilike '" + searchWord.Replace("'", "''") +
              "' or accb.get_accnt_num(b.control_account_id) ilike '" + searchWord.Replace("'", "''") +
              "' or b.accnt_name ilike '" + searchWord.Replace("'", "''") +
             "' or accb.get_accnt_name(b.control_account_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Transaction Description")
            {
                whereCls = " AND (a.transaction_desc ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Transaction Date")
            {
                whereCls = " AND (to_char(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS'), " +
                  "'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Batch Name")
            {
                whereCls = " AND (c.batch_name ilike '" + searchWord.Replace("'", "''") +
              "')";
            }
            string amntCls = "";
            if (lowVal != 0 || highVal != 0)
            {
                amntCls = " and ((dbt_amount !=0 and dbt_amount between " + lowVal + " and " + highVal +
                  ") or (crdt_amount !=0 and crdt_amount between " + lowVal + " and " + highVal + "))";
            }
            strSql = "SELECT count(1) " +
            "FROM (accb.accb_trnsctn_details a LEFT OUTER JOIN accb.accb_trnsctn_batches c ON a.batch_id = c.batch_id) " +
            "LEFT OUTER JOIN accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
            "WHERE((b.org_id = " + orgID + ") and (trns_status = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isposted) +
            "') and (to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + dte1 +
            "','YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + dte2 + "','YYYY-MM-DD HH24:MI:SS'))" + whereCls + amntCls + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            long sumRes = 0;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                long.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static DataSet get_Batch_Attachments(long batchID)
        {
            string strSql = "";

            strSql = "SELECT a.attchmnt_id, a.batch_id, a.attchmnt_desc, a.file_name " +
          "FROM accb.accb_batch_trns_attchmnts a " +
          "WHERE(a.batch_id = " + batchID + ") ORDER BY a.attchmnt_id";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_Attachments(string searchWord, string searchIn,
       Int64 offset, int limit_size, long batchID, ref string attchSQL)
        {
            string strSql = "";
            if (searchIn == "Attachment Name/Description")
            {
                strSql = "SELECT a.attchmnt_id, a.batch_id, a.attchmnt_desc, a.file_name " +
              "FROM accb.accb_batch_trns_attchmnts a " +
              "WHERE(a.attchmnt_desc ilike '" + searchWord.Replace("'", "''") +
              "' and a.batch_id = " + batchID + ") ORDER BY a.attchmnt_id LIMIT " + limit_size +
                  " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            attchSQL = strSql;
            return dtst;
        }

        public static long get_Total_Attachments(string searchWord,
          string searchIn, long batchID)
        {
            string strSql = "";
            if (searchIn == "Attachment Name/Description")
            {
                strSql = "SELECT COUNT(1) " +
              "FROM accb.accb_batch_trns_attchmnts a " +
              "WHERE(a.attchmnt_desc ilike '" + searchWord.Replace("'", "''") +
              "' and a.batch_id = " + batchID + ")";
            }
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            long sumRes = 0;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                long.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static DataSet get_Asset_Attachments(string searchWord, string searchIn,
      Int64 offset, int limit_size, long batchID, ref string attchSQL)
        {
            string strSql = "";
            if (searchIn == "Attachment Name/Description")
            {
                strSql = "SELECT a.attchmnt_id, a.asset_id, a.attchmnt_desc, a.file_name " +
              "FROM accb.accb_asset_doc_attchmnts a " +
              "WHERE(a.attchmnt_desc ilike '" + searchWord.Replace("'", "''") +
              "' and a.asset_id = " + batchID + ") ORDER BY a.attchmnt_id LIMIT " + limit_size +
                  " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            attchSQL = strSql;
            return dtst;
        }

        public static long get_Total_Asset_Attachments(string searchWord,
          string searchIn, long batchID)
        {
            string strSql = "";
            if (searchIn == "Attachment Name/Description")
            {
                strSql = "SELECT COUNT(1) " +
              "FROM accb.accb_asset_doc_attchmnts a " +
              "WHERE(a.attchmnt_desc ilike '" + searchWord.Replace("'", "''") +
              "' and a.asset_id = " + batchID + ")";
            }
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            long sumRes = 0;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                long.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static DataSet get_Rcvbls_Attachments(string searchWord, string searchIn,
      Int64 offset, int limit_size, long batchID, ref string attchSQL)
        {
            string strSql = "";
            if (searchIn == "Attachment Name/Description")
            {
                strSql = "SELECT a.attchmnt_id, a.doc_hdr_id, a.attchmnt_desc, a.file_name " +
              "FROM accb.accb_rcvbl_doc_attchmnts a " +
              "WHERE(a.attchmnt_desc ilike '" + searchWord.Replace("'", "''") +
              "' and a.doc_hdr_id = " + batchID + ") ORDER BY a.attchmnt_id LIMIT " + limit_size +
                  " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            attchSQL = strSql;
            return dtst;
        }

        public static long get_Total_Rcvbls_Attachments(string searchWord,
          string searchIn, long batchID)
        {
            string strSql = "";
            if (searchIn == "Attachment Name/Description")
            {
                strSql = "SELECT COUNT(1) " +
              "FROM accb.accb_rcvbl_doc_attchmnts a " +
              "WHERE(a.attchmnt_desc ilike '" + searchWord.Replace("'", "''") +
              "' and a.doc_hdr_id = " + batchID + ")";
            }
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            long sumRes = 0;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                long.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static DataSet get_Cstmr_Attachments(string searchWord, string searchIn,
      Int64 offset, int limit_size, long batchID, ref string attchSQL)
        {
            string strSql = "";
            if (searchIn == "Attachment Name/Description")
            {
                strSql = "SELECT a.attchmnt_id, a.firms_id, a.attchmnt_desc, a.file_name " +
              "FROM accb.accb_firms_doc_attchmnts a " +
              "WHERE(a.attchmnt_desc ilike '" + searchWord.Replace("'", "''") +
              "' and a.firms_id = " + batchID + ") ORDER BY a.attchmnt_id LIMIT " + limit_size +
                  " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            attchSQL = strSql;
            return dtst;
        }

        public static long get_Total_Cstmr_Attachments(string searchWord,
          string searchIn, long batchID)
        {
            string strSql = "";
            if (searchIn == "Attachment Name/Description")
            {
                strSql = "SELECT COUNT(1) " +
              "FROM accb.accb_firms_doc_attchmnts a " +
              "WHERE(a.attchmnt_desc ilike '" + searchWord.Replace("'", "''") +
              "' and a.firms_id = " + batchID + ")";
            }
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            long sumRes = 0;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                long.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static DataSet get_Transactions(string searchWord, string searchIn,
        Int64 offset, int limit_size, int orgID, decimal lowVal, decimal highVal, string orderBy)
        {
            /*Date (ASC)
      Date (DESC)*/
            string ordrCls = "ORDER BY to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') DESC";
            if (orderBy == "Date (ASC)")
            {
                ordrCls = "ORDER BY to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS')";
            }
            string strSql = "";
            string whereCls = "";
            if (searchIn == "Account Number")
            {
                whereCls = " AND (b.accnt_num ilike '" + searchWord.Replace("'", "''") +
              "' or accb.get_accnt_num(b.control_account_id) ilike '" + searchWord.Replace("'", "''") +
              "' or b.accnt_name ilike '" + searchWord.Replace("'", "''") +
             "' or accb.get_accnt_name(b.control_account_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Account Name")
            {
                whereCls = " AND (b.accnt_num ilike '" + searchWord.Replace("'", "''") +
              "' or accb.get_accnt_num(b.control_account_id) ilike '" + searchWord.Replace("'", "''") +
              "' or b.accnt_name ilike '" + searchWord.Replace("'", "''") +
             "' or accb.get_accnt_name(b.control_account_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Transaction Description")
            {
                whereCls = " AND (a.transaction_desc || a.ref_doc_number ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Transaction Date")
            {
                whereCls = " AND (to_char(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS'), " +
                  "'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") + "')";
            }

            string amntCls = "";
            if (lowVal != 0 || highVal != 0)
            {
                amntCls = " and ((a.dbt_amount !=0 and a.dbt_amount between " + lowVal + " and " + highVal +
                  ") or (a.crdt_amount !=0 and a.crdt_amount between " + lowVal + " and " + highVal + "))";
            }

            strSql = "SELECT a.transctn_id, b.accnt_num, b.accnt_name, a.transaction_desc, a.dbt_amount, a.crdt_amount, " +
              "to_char(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), a.func_cur_id, " +
              "a.batch_id, a.accnt_id, a.net_amount, a.entered_amnt, gst.get_pssbl_val(a.entered_amt_crncy_id), a.entered_amt_crncy_id, " +
                    @"a.accnt_crncy_amnt, gst.get_pssbl_val(a.accnt_crncy_id), a.accnt_crncy_id, a.func_cur_exchng_rate, a.accnt_cur_exchng_rate, 
(select d.batch_name from accb.accb_trnsctn_batches d where d.batch_id = a.batch_id) btch_nm, a.ref_doc_number " +
         "FROM accb.accb_trnsctn_details a LEFT OUTER JOIN " +
         "accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
         "WHERE(b.org_id = " + orgID + " and trns_status = '1'" + whereCls + amntCls + ") " + ordrCls + " LIMIT " + limit_size +
         " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.chrtDet_SQL = strSql;
            Global.mnFrm.bls_SQL = strSql;
            return dtst;
        }

        public static long get_Total_Transactions(string searchWord, string searchIn, int orgID, decimal lowVal, decimal highVal)
        {
            string strSql = "";
            string whereCls = "";
            if (searchIn == "Account Number")
            {
                whereCls = " AND (b.accnt_num ilike '" + searchWord.Replace("'", "''") +
              "' or accb.get_accnt_num(b.control_account_id) ilike '" + searchWord.Replace("'", "''") +
              "' or b.accnt_name ilike '" + searchWord.Replace("'", "''") +
             "' or accb.get_accnt_name(b.control_account_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Account Name")
            {
                whereCls = " AND (b.accnt_num ilike '" + searchWord.Replace("'", "''") +
              "' or accb.get_accnt_num(b.control_account_id) ilike '" + searchWord.Replace("'", "''") +
              "' or b.accnt_name ilike '" + searchWord.Replace("'", "''") +
             "' or accb.get_accnt_name(b.control_account_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Transaction Description")
            {
                whereCls = " AND (a.transaction_desc ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Transaction Date")
            {
                whereCls = " AND (to_char(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS'), " +
                  "'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") + "')";
            }

            string amntCls = "";
            if (lowVal != 0 || highVal != 0)
            {
                amntCls = " and ((a.dbt_amount !=0 and a.dbt_amount between " + lowVal + " and " + highVal +
                  ") or (a.crdt_amount !=0 and a.crdt_amount between " + lowVal + " and " + highVal + "))";
            }

            strSql = "SELECT count(1) " +
            "FROM accb.accb_trnsctn_details a LEFT OUTER JOIN " +
            "accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
            "WHERE(b.org_id = " + orgID + " and trns_status = '1'" + whereCls + amntCls + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            long sumRes = 0;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                long.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static DataSet get_Transactions(string searchWord, string searchIn,
      Int64 offset, int limit_size, int orgID, int accntid, decimal lowVal, decimal highVal, string orderBy)
        {
            /*Date (ASC)
      Date (DESC)*/
            string ordrCls = "ORDER BY to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') DESC";
            if (orderBy == "Date (ASC)")
            {
                ordrCls = "ORDER BY to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS')";
            }
            string strSql = "";
            string whereCls = "";
            if (searchIn == "Account Number")
            {
                whereCls = " AND (b.accnt_num ilike '" + searchWord.Replace("'", "''") +
              "' or accb.get_accnt_num(b.control_account_id) ilike '" + searchWord.Replace("'", "''") +
              "' or b.accnt_name ilike '" + searchWord.Replace("'", "''") +
             "' or accb.get_accnt_name(b.control_account_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Account Name")
            {
                whereCls = " AND (b.accnt_num ilike '" + searchWord.Replace("'", "''") +
              "' or accb.get_accnt_num(b.control_account_id) ilike '" + searchWord.Replace("'", "''") +
              "' or b.accnt_name ilike '" + searchWord.Replace("'", "''") +
             "' or accb.get_accnt_name(b.control_account_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Transaction Description")
            {
                whereCls = " AND (a.transaction_desc || a.ref_doc_number ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Transaction Date")
            {
                whereCls = " AND (to_char(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS'), " +
                  "'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") + "')";
            }

            string amntCls = "";
            if (lowVal != 0 || highVal != 0)
            {
                amntCls = " and ((a.dbt_amount !=0 and a.dbt_amount between " + lowVal + " and " + highVal +
                  ") or (a.crdt_amount !=0 and a.crdt_amount between " + lowVal + " and " + highVal + "))";
            }

            strSql = "SELECT a.transctn_id, b.accnt_num, b.accnt_name, a.transaction_desc, a.dbt_amount, a.crdt_amount, " +
              "to_char(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), a.func_cur_id, " +
              "a.batch_id, a.accnt_id, a.net_amount, a.entered_amnt, gst.get_pssbl_val(a.entered_amt_crncy_id), a.entered_amt_crncy_id, " +
                    @"a.accnt_crncy_amnt, gst.get_pssbl_val(a.accnt_crncy_id), a.accnt_crncy_id, a.func_cur_exchng_rate, a.accnt_cur_exchng_rate, 
(select d.batch_name from accb.accb_trnsctn_batches d where d.batch_id = a.batch_id) btch_nm, a.ref_doc_number " +
         "FROM accb.accb_trnsctn_details a LEFT OUTER JOIN " +
         "accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
         "WHERE(b.org_id = " + orgID + " and trns_status = '1' and (a.accnt_id = " +
            accntid + " or b.control_account_id = " +
            accntid + ")" + whereCls + amntCls + ") " + ordrCls + " LIMIT " + limit_size +
         " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.chrtDet_SQL = strSql;
            Global.mnFrm.bls_SQL = strSql;
            return dtst;
        }

        public static long get_Total_Transactions(string searchWord, string searchIn,
        int orgID, int accntid, decimal lowVal, decimal highVal)
        {
            string strSql = "";
            string whereCls = "";
            if (searchIn == "Account Number")
            {
                whereCls = " AND (b.accnt_num ilike '" + searchWord.Replace("'", "''") +
              "' or accb.get_accnt_num(b.control_account_id) ilike '" + searchWord.Replace("'", "''") +
              "' or b.accnt_name ilike '" + searchWord.Replace("'", "''") +
             "' or accb.get_accnt_name(b.control_account_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Account Name")
            {
                whereCls = " AND (b.accnt_num ilike '" + searchWord.Replace("'", "''") +
              "' or accb.get_accnt_num(b.control_account_id) ilike '" + searchWord.Replace("'", "''") +
              "' or b.accnt_name ilike '" + searchWord.Replace("'", "''") +
             "' or accb.get_accnt_name(b.control_account_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Transaction Description")
            {
                whereCls = " AND (a.transaction_desc ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Transaction Date")
            {
                whereCls = " AND (to_char(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS'), " +
                  "'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") + "')";
            }

            string amntCls = "";
            if (lowVal != 0 || highVal != 0)
            {
                amntCls = " and ((a.dbt_amount !=0 and a.dbt_amount between " + lowVal + " and " + highVal +
                  ") or (a.crdt_amount !=0 and a.crdt_amount between " + lowVal + " and " + highVal + "))";
            }

            strSql = "SELECT count(1) " +
            "FROM accb.accb_trnsctn_details a LEFT OUTER JOIN " +
            "accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
            "WHERE(b.org_id = " + orgID + " and trns_status = '1' and (a.accnt_id = " +
            accntid + " or b.control_account_id = " +
            accntid + ")" + whereCls + amntCls + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            long sumRes = 0;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                long.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static DataSet get_IntfcTrns(string searchWord, string searchIn,
       Int64 offset, int limit_size, int orgID, long trnsID, decimal lowVal, decimal highVal, string orderBy)
        {
            /*Date (ASC)
      Date (DESC)*/
            string ordrCls = "ORDER BY to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') DESC";
            if (orderBy == "Date (ASC)")
            {
                ordrCls = "ORDER BY to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS')";
            }
            string strSql = "";
            string intfcTblNm = "scm.scm_gl_interface";
            string extrDesc = "scm.get_src_doc_num(a.src_doc_id,a.src_doc_typ)";
            long batchID = long.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
         "accb.accb_trnsctn_details", "transctn_id", "batch_id", trnsID));

            string batchSrc = Global.mnFrm.cmCde.getGnrlRecNm(
         "accb.accb_trnsctn_batches", "batch_id", "batch_source", batchID);
            if (batchSrc.Contains("Internal Payments"))
            {
                intfcTblNm = "pay.pay_gl_interface";
                extrDesc = "a.source_trns_id";
            }
            else if (batchSrc.Contains("Banking"))
            {
                intfcTblNm = "mcf.mcf_gl_interface";
                extrDesc = "a.src_doc_typ";
            }
            else if (batchSrc.Contains("Vault"))
            {
                intfcTblNm = "vms.vms_gl_interface";
                extrDesc = "a.src_doc_typ";
            }

            string amntCls = "";
            if (lowVal != 0 || highVal != 0)
            {
                amntCls = " and ((a.dbt_amount !=0 and a.dbt_amount between " + lowVal + " and " + highVal +
                  ") or (a.crdt_amount !=0 and a.crdt_amount between " + lowVal + " and " + highVal + "))";
            }
            strSql = @"SELECT c.transctn_id, b.accnt_num, b.accnt_name, 
a.transaction_desc || ' Source Doc: ' ||" + extrDesc + @" description, 
a.dbt_amount, a.crdt_amount,
to_char(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), c.func_cur_id,
c.batch_id,c.accnt_id, a.net_amount, c.entered_amnt, gst.get_pssbl_val(c.entered_amt_crncy_id), c.entered_amt_crncy_id, 
c.accnt_crncy_amnt, gst.get_pssbl_val(c.accnt_crncy_id), c.accnt_crncy_id, c.func_cur_exchng_rate, c.accnt_cur_exchng_rate, 
(select d.batch_name from accb.accb_trnsctn_batches d where d.batch_id = c.batch_id) btch_nm, '' refdoc
FROM " + intfcTblNm + @" a, accb.accb_chart_of_accnts b, accb.accb_trnsctn_details c 
WHERE ((a.accnt_id = b.accnt_id and c.batch_id=a.gl_batch_id) and (b.accnt_num ilike '%')" + amntCls + " and (b.org_id = " + orgID + @") 
and c.transctn_id=" + trnsID + @" and c.source_trns_ids like '%,' || a.interface_id || ',%') " +
      ordrCls + " LIMIT " + limit_size +
         " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.chrtDet_SQL = strSql;
            Global.mnFrm.bls_SQL = strSql;
            return dtst;
        }

        public static long get_Total_IntfcTrns(string searchWord, string searchIn,
          int orgID, long trnsID, decimal lowVal, decimal highVal)
        {
            string strSql = "";
            string intfcTblNm = "scm.scm_gl_interface";

            long batchID = long.Parse(Global.mnFrm.cmCde.getGnrlRecNm(
         "accb.accb_trnsctn_details", "transctn_id", "batch_id", trnsID));

            string batchSrc = Global.mnFrm.cmCde.getGnrlRecNm(
         "accb.accb_trnsctn_batches", "batch_id", "batch_source", batchID);
            if (batchSrc.Contains("Internal Payments"))
            {
                intfcTblNm = "pay.pay_gl_interface";
            }
            else if (batchSrc.Contains("Banking"))
            {
                intfcTblNm = "mcf.mcf_gl_interface";
            }
            else if (batchSrc.Contains("Vault"))
            {
                intfcTblNm = "vms.vms_gl_interface";
            }

            string amntCls = "";
            if (lowVal != 0 || highVal != 0)
            {
                amntCls = " and ((a.dbt_amount !=0 and a.dbt_amount between " + lowVal + " and " + highVal +
                  ") or (a.crdt_amount !=0 and a.crdt_amount between " + lowVal + " and " + highVal + "))";
            }
            strSql = "SELECT count(1) " +
            "FROM " + intfcTblNm + @" a, accb.accb_chart_of_accnts b, accb.accb_trnsctn_details c 
WHERE ((a.accnt_id = b.accnt_id and c.batch_id=a.gl_batch_id) and (b.accnt_num ilike '%')" + amntCls + " and (b.org_id = " + orgID + @") 
and c.transctn_id=" + trnsID + @" and c.source_trns_ids like '%,' || a.interface_id || ',%') ";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            long sumRes = 0;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                long.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static double computeMathExprsn(string exprSn)
        {
            string strSql = "";
            strSql = "SELECT " + exprSn.Replace("/", "::float/").Replace("=", "").Replace(",", "").Replace("'", "''");

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams1(strSql);
            if (dtst.Tables.Count <= 0)
            {
                return 0;
            }
            else if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }

        public static DataSet get_One_Batch_Trns(long offset, int limit_size, long batchID)
        {
            string strSql = "";
            strSql = "SELECT a.transctn_id, b.accnt_num, b.accnt_name, " +
              "a.transaction_desc, a.dbt_amount, a.crdt_amount, " +
                    "to_char(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), a.func_cur_id, " +
                    "a.batch_id, a.accnt_id, a.net_amount, a.trns_status, a.entered_amnt, gst.get_pssbl_val(a.entered_amt_crncy_id), a.entered_amt_crncy_id, " +
                    "a.accnt_crncy_amnt, gst.get_pssbl_val(a.accnt_crncy_id), a.accnt_crncy_id, a.func_cur_exchng_rate, a.accnt_cur_exchng_rate, a.ref_doc_number " +
          "FROM accb.accb_trnsctn_details a LEFT OUTER JOIN " +
          "accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
          "WHERE(a.batch_id = " + batchID + ") ORDER BY a.transctn_id ASC LIMIT " + limit_size +
                " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.trnsDet_SQL = strSql;
            return dtst;
        }

        public static long get_Total_BatchTrns(long batchID)
        {
            string strSql = "";
            strSql = "SELECT count(1) " +
         "FROM accb.accb_trnsctn_details a LEFT OUTER JOIN " +
         "accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
         "WHERE(a.batch_id = " + batchID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static bool hsTrnsUptdAcntBls(long actrnsid,
      string trnsdate, int accnt_id)
        {
            trnsdate = DateTime.ParseExact(
         trnsdate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            trnsdate = trnsdate.Substring(0, 10);

            string strSql = "SELECT a.daily_bals_id FROM accb.accb_accnt_daily_bals a " +
              "WHERE a.accnt_id = " + accnt_id +
              " and a.as_at_date = '" + trnsdate + "' and a.src_trns_ids like '%," + actrnsid + ",%'";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static bool hsTrnsUptdAcntCurrBls(long actrnsid,
      string trnsdate, int accnt_id)
        {
            trnsdate = DateTime.ParseExact(
         trnsdate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            trnsdate = trnsdate.Substring(0, 10);

            string strSql = "SELECT a.daily_cbals_id FROM accb.accb_accnt_crncy_daily_bals a " +
              "WHERE a.accnt_id = " + accnt_id +
              " and a.as_at_date = '" + trnsdate + "' and a.src_trns_ids like '%," + actrnsid + ",%'";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static DataSet get_Batch_Trns(long batchID)
        {
            string strSql = "";
            strSql = "SELECT a.transctn_id, b.accnt_num, b.accnt_name, " +
            "a.transaction_desc, a.dbt_amount, a.crdt_amount, " +
            "to_char(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), a.func_cur_id, " +
            "a.batch_id, a.accnt_id, a.net_amount, a.trns_status, a.entered_amnt, gst.get_pssbl_val(a.entered_amt_crncy_id), a.entered_amt_crncy_id, " +
            "a.accnt_crncy_amnt, gst.get_pssbl_val(a.accnt_crncy_id), a.accnt_crncy_id, a.func_cur_exchng_rate, a.accnt_cur_exchng_rate, a.src_trns_id_reconciled " +
            "FROM accb.accb_trnsctn_details a LEFT OUTER JOIN " +
            "accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
            "WHERE(a.batch_id = " + batchID + " and a.trns_status='0') ORDER BY a.transctn_id";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.mnFrm.trnsDet_SQL = strSql;
            return dtst;
        }

        public static DataSet get_Batch_Accnts(long batchID)
        {
            string strSql = "";
            strSql = "SELECT a.accnt_id " +
          "FROM accb.accb_trnsctn_details a LEFT OUTER JOIN " +
          "accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
          "WHERE(a.batch_id = " + batchID + ") ORDER BY a.transctn_id";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.trnsDet_SQL = strSql;
            return dtst;
        }

        public static long get_ScmIntrfcTrnsCnt(long batchID)
        {
            string strSql = "";
            strSql = "SELECT count(1) " +
          "FROM scm.scm_gl_interface a " +
          "WHERE(a.gl_batch_id = " + batchID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }

        public static long get_PayIntrfcTrnsCnt(long batchID)
        {
            string strSql = "";
            strSql = "SELECT count(1) " +
          "FROM pay.pay_gl_interface a " +
          "WHERE(a.gl_batch_id = " + batchID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }

        public static DataSet get_Batch_Trns_NoStatus(long batchID)
        {
            string strSql = "";
            strSql = "SELECT a.transctn_id, b.accnt_num, b.accnt_name, " +
              "a.transaction_desc, a.dbt_amount, a.crdt_amount, " +
                    "to_char(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), a.func_cur_id, " +
                    "a.batch_id, a.accnt_id, a.net_amount, a.trns_status, a.entered_amnt, a.entered_amt_crncy_id, " +
                    "a.accnt_crncy_amnt, a.accnt_crncy_id, a.func_cur_exchng_rate, a.accnt_cur_exchng_rate, a.dbt_or_crdt " +
          "FROM accb.accb_trnsctn_details a LEFT OUTER JOIN " +
          "accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
          "WHERE(a.batch_id = " + batchID + ") ORDER BY a.transctn_id";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.trnsDet_SQL = strSql;
            return dtst;
        }

        public static DataSet get_Trns_AmntBrkdwn(long trnsID, int lovID)
        {
            string strSql = "";
            strSql = @"  SELECT   *
    FROM   (SELECT   a.trns_amnt_det_id,
                     a.description,
                     a.quantity,
                     a.unit_amnt,
                     a.ttl_amnt,
                     a.lnkd_pssbl_val_id
              FROM   accb.accb_trnsctn_amnt_breakdown a
             WHERE   (a.transaction_id = " + trnsID + @")
            UNION
            SELECT   -1,
                     b.pssbl_value,
                     0,
                     0,
                     0,
                     b.pssbl_value_id
              FROM   gst.gen_stp_lov_values b
             WHERE   b.value_list_id = " + lovID + @"
                     AND b.is_enabled='1'
                     AND b.pssbl_value_id NOT IN
                              (SELECT   c.lnkd_pssbl_val_id
                                 FROM   accb.accb_trnsctn_amnt_breakdown c
                                WHERE   (c.transaction_id = " + trnsID + @"))) tbl1
ORDER BY   1, 6";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.mnFrm.trnsDet_SQL = strSql;
            return dtst;
        }

        public static DataSet get_Trns_AmntBrkdwn1(long trnsID, int lovID)
        {
            string strSql = "";
            strSql = @"  SELECT   *
    FROM   (SELECT   a.trns_amnt_det_id,
                     a.description,
                     a.quantity,
                     a.unit_amnt,
                     a.ttl_amnt,
                     a.lnkd_pssbl_val_id
              FROM   accb.accb_trnsctn_amnt_breakdown a
             WHERE   (a.transaction_id = " + trnsID + @")) tbl1
ORDER BY   1, 6";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.mnFrm.trnsDet_SQL = strSql;
            return dtst;
        }

        public static DataSet get_Bdgt_AmntBrkdwn(long acntID, long bdgtDtID, ref string chrt_SQL)
        {
            string strSql = "";
            string extrWhere = "";
            if (bdgtDtID > 0)
            {
                extrWhere = " and a.budget_det_id=" + bdgtDtID;
            }
            strSql = @"  SELECT a.bdgt_amnt_brkdwn_id, a.account_id, 
        a.bdgt_item_id, inv.get_invitm_name(a.bdgt_item_id), a.bdgt_detail_type, 
       a.item_quantity1, a.item_quantity2, a.unit_price_or_rate,
        (a.item_quantity1*a.item_quantity2*a.unit_price_or_rate), a.remarks_desc, a.budget_det_id, 
        to_char(to_timestamp(b.start_date,'YYYY-MM-DD HH24:MI:SS'), 'DD-Mon-YYYY HH24:MI:SS')  start_date, 
        to_char(to_timestamp(b.end_date,'YYYY-MM-DD HH24:MI:SS'), 'DD-Mon-YYYY HH24:MI:SS') end_date
        FROM accb.accb_bdgt_amnt_brkdwn a, accb.accb_budget_details b
             WHERE   (a.account_id = " + acntID + @" and a.budget_det_id=b.budget_det_id" + extrWhere + @")
        ORDER BY   4, 1";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            chrt_SQL = strSql;
            return dtst;
        }

        public static DataSet get_Bdgt_DetBrkdwns(long bdgtID, long offset, int limit_size)
        {
            string strSql = "";
            strSql = @" SELECT a.bdgt_amnt_brkdwn_id, a.account_id, 
        a.bdgt_item_id, inv.get_invitm_code(a.bdgt_item_id), inv.get_invitm_name(a.bdgt_item_id), a.bdgt_detail_type, 
       a.item_quantity1, a.item_quantity2, a.unit_price_or_rate,
        (a.item_quantity1*a.item_quantity2*a.unit_price_or_rate), a.remarks_desc, a.budget_det_id, 
        to_char(to_timestamp(b.start_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS')  start_date, 
        to_char(to_timestamp(b.end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') end_date, 
        accb.get_accnt_num(a.account_id), accb.get_accnt_name(a.account_id) 
        FROM accb.accb_bdgt_amnt_brkdwn a, accb.accb_budget_details b 
             WHERE (a.budget_det_id IN (select z.budget_det_id from accb.accb_budget_details z where z.budget_id=" + bdgtID + @") and a.budget_det_id=b.budget_det_id)
        ORDER BY   15, 4 LIMIT " + limit_size +
                " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //chrt_SQL = strSql;
            return dtst;
        }

        public static double get_InvItemPrice(int itmID)
        {
            string strSql = "SELECT selling_price " +
         "FROM inv.inv_itm_list a " +
         "WHERE item_id =" + itmID + "";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0.00;
        }

        public static DataSet get_Batch_dateSums(long batchID)
        {
            string strSql = "";
            strSql = @"SELECT substring(a.trnsctn_date from 1 for 10), round(SUM(a.dbt_amount),4), round(SUM(a.crdt_amount),4) 
    FROM accb.accb_trnsctn_details a
    WHERE(a.batch_id = " + batchID + @") 
    GROUP BY substring(a.trnsctn_date from 1 for 10) 
    HAVING round(SUM(a.dbt_amount),2) != round(SUM(a.crdt_amount),2)
    ORDER BY 1";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.mnFrm.trnsDet_SQL = strSql;
            return dtst;
        }

        public static double get_Batch_DbtSum(long batchID)
        {
            string strSql = "";
            double sumRes = 0.00;
            strSql = "SELECT SUM(a.dbt_amount)" +
          "FROM accb.accb_trnsctn_details a " +
          "WHERE(a.batch_id = " + batchID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return Math.Round(sumRes, 2);
        }

        public static double get_Batch_CrdtSum(long batchID)
        {
            string strSql = "";
            strSql = "SELECT SUM(a.crdt_amount)" +
          "FROM accb.accb_trnsctn_details a " +
          "WHERE(a.batch_id = " + batchID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return Math.Round(sumRes, 2);
        }

        public static long getBatchID(string batchname, int orgid)
        {
            string strSql = "";
            strSql = "SELECT a.batch_id " +
         "FROM accb.accb_trnsctn_batches a " +
            "WHERE ((a.batch_name ilike '" + batchname.Replace("'", "''") +
              "') AND (a.org_id = " + orgid + "))";

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

        public static string getBatchNm(long batchid)
        {
            string strSql = "";
            strSql = "SELECT a.batch_name " +
         "FROM accb.accb_trnsctn_batches a " +
            "WHERE ((a.batch_id = " + batchid + "))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        public static long getNewTrnsID()
        {
            //string strSql = "select nextval('accb.accb_trnsctn_batches_batch_id_seq'::regclass);";
            string strSql = "select nextval('accb.accb_trnsctn_details_transctn_id_seq')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static long getNewAmntBrkDwnID()
        {
            //string strSql = "select nextval('accb.accb_trnsctn_batches_batch_id_seq'::regclass);";
            string strSql = "select nextval('accb.accb_trnsctn_amnt_breakdown_trns_amnt_det_id_seq')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static string getTemplateNm(long templtID)
        {
            string strSql = "";
            strSql = "SELECT a.template_name " +
         "FROM accb.accb_trnsctn_templates_hdr a " +
            "WHERE ((a.template_id = " + templtID + "))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        public static long getAttchmntID(string attchname, long batchID, string tblNm, string pkName)
        {
            string strSql = "";
            strSql = "SELECT a.attchmnt_id " +
         "FROM " + tblNm + " a " +
            "WHERE ((a.attchmnt_desc = '" + attchname.Replace("'", "''") +
              "') AND (a." + pkName + " = " + batchID + "))";

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

        public static long getSimlrPstdBatchID(long srcbatchid, string orgnlbatchname, int orgid)
        {
            string strSql = "";
            strSql = "SELECT a.batch_id " +
         "FROM accb.accb_trnsctn_batches a " +
            "WHERE (((a.src_batch_id = " + srcbatchid.ToString() +
              ") or (a.batch_name ilike '" + orgnlbatchname.Replace("'", "''") +
              "' AND a.batch_vldty_status = 'VOID')) AND (a.org_id = " + orgid + "))";// AND (a.batch_status='1')

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

        public static long getSimlrPstdBatchID(string orgnlbatchname, int orgid)
        {
            long srcbatchid = getBatchID(orgnlbatchname, orgid);
            string strSql = "";
            strSql = "SELECT a.batch_id " +
         "FROM accb.accb_trnsctn_batches a " +
            "WHERE (((a.src_batch_id = " + srcbatchid.ToString() +
              ") or (a.batch_name ilike '" + orgnlbatchname.Replace("'", "''") +
              "' AND a.batch_vldty_status = 'VOID')) AND (a.org_id = " + orgid + "))";// AND (a.batch_status='1')

            Global.mnFrm.trns_SQL = strSql;
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

        public static DataSet get_Basic_BatchDet(string searchWord, string searchIn,
        Int64 offset, int limit_size, int orgID, bool shwUsrOnly, bool shwUnpstdOnly)
        {
            string whercls = "";

            if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[28]) == true
              || shwUsrOnly == true)
            {
                whercls = " AND (a.created_by=" + Global.mnFrm.cmCde.User_id + ")";
            }
            string unpstdCls = "";
            if (shwUnpstdOnly)
            {
                unpstdCls = " AND (a.batch_status!='1')";
            }
            string strSql = "";
            string whercls1 = "";

            if (searchIn == "Batch Name")
            {
                whercls1 = " AND (a.batch_name ilike '" + searchWord.Replace("'", "''") +
                  "' or trim(to_char(a.batch_id, '99999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            else if (searchIn == "Batch Description")
            {
                whercls1 = " AND (a.batch_description ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            else if (searchIn == "Batch Status")
            {
                whercls1 = " AND ((CASE WHEN a.batch_status='1' THEN 'Posted' ELSE 'Not Posted' END) ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            else if (searchIn == "Batch Number")
            {
                whercls1 = " AND (trim(to_char(a.batch_id, '99999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            else if (searchIn == "Batch Date")
            {
                whercls1 = " AND (to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            strSql = "SELECT a.batch_id, a.batch_name, a.batch_description, " +
                    @"a.batch_status, to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
      a.batch_source, a.batch_vldty_status, CASE WHEN a.avlbl_for_postng='1' THEN 'Pending Auto-Post' ELSE 'Not Monitored' END " +
          "FROM accb.accb_trnsctn_batches a " +
              "WHERE ((a.org_id = " + orgID + ")" + whercls1 + "" + whercls + unpstdCls + ") ORDER BY a.batch_id DESC LIMIT " + limit_size +
                " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            Global.mnFrm.trns_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_Total_Batches(string searchWord, string searchIn, int orgID, bool shwUsrOnly, bool shwUnpstdOnly)
        {
            string whercls = "";
            string unpstdCls = "";
            if (Global.mnFrm.cmCde.test_prmssns(Global.dfltPrvldgs[28]) == true
              || shwUsrOnly == true)
            {
                whercls = " AND (a.created_by=" + Global.mnFrm.cmCde.User_id + ")";
            }
            if (shwUnpstdOnly)
            {
                unpstdCls = " AND (a.batch_status!='1')";
            }
            string strSql = "";
            string whercls1 = "";

            if (searchIn == "Batch Name")
            {
                whercls1 = " AND (a.batch_name ilike '" + searchWord.Replace("'", "''") +
                  "' or trim(to_char(a.batch_id, '99999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            else if (searchIn == "Batch Description")
            {
                whercls1 = " AND (a.batch_description ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            else if (searchIn == "Batch Status")
            {
                whercls1 = " AND ((CASE WHEN a.batch_status='1' THEN 'Posted' ELSE 'Not Posted' END) ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            else if (searchIn == "Batch Number")
            {
                whercls1 = " AND (trim(to_char(a.batch_id, '99999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            else if (searchIn == "Batch Date")
            {
                whercls1 = " AND (to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            strSql = "SELECT count(1) FROM accb.accb_trnsctn_batches a " +
              "WHERE ((a.org_id = " + orgID + ")" + whercls1 + "" + whercls + unpstdCls + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static string get_Batch_Rec_Hstry(long batchID)
        {
            string strSQL = "SELECT a.created_by, to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), a.last_update_by, to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM accb.accb_trnsctn_batches a WHERE(a.batch_id = " + batchID + ")";
            string fnl_str = "";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSQL);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                fnl_str = "CREATED BY: " + Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][0].ToString())) +
                  "\r\nCREATION DATE: " + dtst.Tables[0].Rows[0][1].ToString() + "\r\nLAST UPDATE BY: " +
                  Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][2].ToString())) +
                  "\r\nLAST UPDATE DATE: " + dtst.Tables[0].Rows[0][3].ToString();
                return fnl_str;
            }
            else
            {
                return "";
            }
        }

        public static string get_TrnsDet_Rec_Hstry(long trnsID)
        {
            string strSQL = "SELECT a.created_by, to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), a.last_update_by, to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM accb.accb_trnsctn_details a WHERE(a.transctn_id = " + trnsID + ")";
            string fnl_str = "";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSQL);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                fnl_str = "CREATED BY: " + Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][0].ToString())) +
                  "\r\nCREATION DATE: " + dtst.Tables[0].Rows[0][1].ToString() + "\r\nLAST UPDATE BY: " +
                  Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][2].ToString())) +
                  "\r\nLAST UPDATE DATE: " + dtst.Tables[0].Rows[0][3].ToString();
                return fnl_str;
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region "TRANSACTIONS TEMPLATE..."
        public static DataSet get_One_Tmplt_Trns(long tmpltID, int curID)
        {
            string strSql = "";
            strSql = "SELECT a.detail_id, a.increase_decrease, b.accnt_num, b.accnt_name, a.trnstn_desc, " +
                     "a.accnt_id " +
                     "FROM accb.accb_trnsctn_templates_det a LEFT OUTER JOIN " +
                     "accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
                     "WHERE(a.template_id = " + tmpltID + " and b.crncy_id = " + curID + ") ORDER BY a.detail_id";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.tmpltDet_SQL = strSql;
            return dtst;
        }

        public static DataSet get_One_Tmplt_Trns(long tmpltID)
        {
            string strSql = "";
            strSql = "SELECT a.detail_id, a.increase_decrease, b.accnt_num, b.accnt_name, a.trnstn_desc, " +
                     "a.accnt_id " +
                     "FROM accb.accb_trnsctn_templates_det a LEFT OUTER JOIN " +
                     "accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
                     "WHERE(a.template_id = " + tmpltID + ") ORDER BY a.detail_id";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.tmpltDet_SQL = strSql;
            return dtst;
        }

        public static DataSet get_One_Tmplt_Usrs(long tmpltID)
        {
            string strSql = "";
            strSql = "SELECT b.user_name, trim(c.title || ' ' || c.sur_name || ', ' || c.first_name " +
                "|| ' ' || c.other_names) fullname, a.user_id, b.person_id, a.row_id, to_char(to_timestamp(a.valid_start_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), " +
                    "to_char(to_timestamp(a.valid_end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
          "FROM (accb.accb_trnsctn_templates_usrs a LEFT OUTER JOIN " +
          "sec.sec_users b ON a.user_id = b.user_id) LEFT OUTER JOIN prs.prsn_names_nos c " +
         "ON b.person_id = c.person_id " +
          "WHERE(a.template_id = " + tmpltID + ") ORDER BY a.row_id";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.tmpltUsrs_SQL = strSql;
            return dtst;
        }

        public static long get_Tmplt_Usr(long tmpltID, long usrid)
        {
            string strSql = "";
            strSql = "SELECT a.row_id " +
          "FROM accb.accb_trnsctn_templates_usrs a " +
          "WHERE((a.template_id = " + tmpltID + ") and (a.user_id = " + usrid + "))";
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

        public static DataSet get_Basic_Tmplt(string searchWord, string searchIn,
        Int64 offset, int limit_size, int orgID)
        {
            string strSql = "";
            if (searchIn == "Template Name")
            {
                strSql = "SELECT a.template_id, a.template_name, a.template_description " +
            "FROM accb.accb_trnsctn_templates_hdr a " +
                "WHERE ((a.template_name ilike '" + searchWord.Replace("'", "''") +
                  "') AND (a.org_id = " + orgID + ")) ORDER BY a.template_id DESC LIMIT " + limit_size +
                  " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }
            else if (searchIn == "Template Description")
            {
                strSql = "SELECT a.template_id, a.template_name, a.template_description " +
            "FROM accb.accb_trnsctn_templates_hdr a " +
                "WHERE ((a.template_description ilike '" + searchWord.Replace("'", "''") +
                  "') AND (a.org_id = " + orgID + ")) ORDER BY a.template_id DESC LIMIT " + limit_size +
                  " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }
            Global.mnFrm.tmplt_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_Total_Tmplts(string searchWord, string searchIn, int orgID)
        {
            string strSql = "";
            if (searchIn == "Template Name")
            {
                strSql = "SELECT count(1) " +
            "FROM accb.accb_trnsctn_templates_hdr a " +
                "WHERE ((a.template_name ilike '" + searchWord.Replace("'", "''") +
                  "') AND (a.org_id = " + orgID + "))";
            }
            else if (searchIn == "Template Description")
            {
                strSql = "SELECT count(1) " +
            "FROM accb.accb_trnsctn_templates_hdr a " +
                "WHERE ((a.template_description ilike '" + searchWord.Replace("'", "''") +
                  "') AND (a.org_id = " + orgID + "))";
            }

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static DataSet get_Usrs_Tmplt(string searchWord, string searchIn,
      Int64 offset, int limit_size, int orgID)
        {
            string curid = Global.mnFrm.cmCde.getOrgFuncCurID(orgID).ToString();

            string strSql = "";
            if (searchIn == "Template Name")
            {
                strSql = "SELECT a.template_id, a.template_name, a.template_description " +
            "FROM accb.accb_trnsctn_templates_hdr a " +
                "WHERE (((Select count(y.detail_id) from accb.accb_trnsctn_templates_det y, " +
                "accb.accb_chart_of_accnts c where y.accnt_id = c.accnt_id and " +
                "y.template_id=a.template_id and c.crncy_id = " + curid + ")=2) and (a.template_name ilike '" + searchWord.Replace("'", "''") +
                  "') AND (a.org_id = " + orgID +
                  ") and (a.template_id IN (select b.template_id from accb.accb_trnsctn_templates_usrs b " +
                  "where ((b.user_id = " + Global.myBscActn.user_id +
                  ") and (now() between to_timestamp(valid_start_date,'YYYY-MM-DD HH24:MI:SS') " +
                  "AND to_timestamp(valid_end_date,'YYYY-MM-DD HH24:MI:SS')))))) " +
                  "ORDER BY a.template_name LIMIT " + limit_size +
                  " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }
            else if (searchIn == "Template Description")
            {
                strSql = "SELECT a.template_id, a.template_name, a.template_description " +
            "FROM accb.accb_trnsctn_templates_hdr a " +
                "WHERE (((Select count(y.detail_id) from accb.accb_trnsctn_templates_det y, " +
                "accb.accb_chart_of_accnts c where y.accnt_id = c.accnt_id and " +
                "y.template_id=a.template_id and c.crncy_id = " + curid + ")=2) and (a.template_description ilike '" + searchWord.Replace("'", "''") +
                  "') AND (a.org_id = " + orgID +
                  ") and (a.template_id IN (select b.template_id from accb.accb_trnsctn_templates_usrs b " +
                  "where ((b.user_id = " + Global.myBscActn.user_id +
                  ") and (now() between to_timestamp(valid_start_date,'YYYY-MM-DD HH24:MI:SS') " +
                  "AND to_timestamp(valid_end_date,'YYYY-MM-DD HH24:MI:SS')))))) " +
                  "ORDER BY a.template_name LIMIT " + limit_size +
                  " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }
            Global.mnFrm.tmpltDiag_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_Total_Usr_Tmplts(string searchWord, string searchIn, int orgID)
        {
            string curid = Global.mnFrm.cmCde.getOrgFuncCurID(orgID).ToString();
            string strSql = "";
            if (searchIn == "Template Name")
            {
                strSql = "SELECT count(1) " +
            "FROM accb.accb_trnsctn_templates_hdr a " +
                "WHERE (((Select count(y.detail_id) from accb.accb_trnsctn_templates_det y, " +
                "accb.accb_chart_of_accnts c where y.accnt_id = c.accnt_id and " +
                "y.template_id=a.template_id and c.crncy_id = " + curid + ")=2) and (a.template_name ilike '" + searchWord.Replace("'", "''") +
                  "') AND (a.org_id = " + orgID +
                  ") and (a.template_id IN (select b.template_id from accb.accb_trnsctn_templates_usrs b " +
                  "where ((b.user_id = " + Global.myBscActn.user_id +
                  ") and (now() between to_timestamp(valid_start_date,'YYYY-MM-DD HH24:MI:SS') " +
                  "AND to_timestamp(valid_end_date,'YYYY-MM-DD HH24:MI:SS')))))) ";
            }
            else if (searchIn == "Template Description")
            {
                strSql = "SELECT count(1) " +
            "FROM accb.accb_trnsctn_templates_hdr a " +
                        "WHERE (((Select count(y.detail_id) from accb.accb_trnsctn_templates_det y, " +
                "accb.accb_chart_of_accnts c where y.accnt_id = c.accnt_id and " +
                "y.template_id=a.template_id and c.crncy_id = " + curid + ")=2) and (a.template_description ilike '" + searchWord.Replace("'", "''") +
                  "') AND (a.org_id = " + orgID +
                  ") and (a.template_id IN (select b.template_id from accb.accb_trnsctn_templates_usrs b " +
                  "where ((b.user_id = " + Global.myBscActn.user_id +
                  ") and (now() between to_timestamp(valid_start_date,'YYYY-MM-DD HH24:MI:SS') " +
                  "AND to_timestamp(valid_end_date,'YYYY-MM-DD HH24:MI:SS')))))) ";
            }

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static string get_Tmplt_Rec_Hstry(int tmpltID)
        {
            string strSQL = "SELECT a.created_by, to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), a.last_update_by, to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM accb.accb_trnsctn_templates_hdr a WHERE(a.template_id = " + tmpltID + ")";
            string fnl_str = "";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSQL);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                fnl_str = "CREATED BY: " + Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][0].ToString())) +
                  "\r\nCREATION DATE: " + dtst.Tables[0].Rows[0][1].ToString() + "\r\nLAST UPDATE BY: " +
                  Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][2].ToString())) +
                  "\r\nLAST UPDATE DATE: " + dtst.Tables[0].Rows[0][3].ToString();
                return fnl_str;
            }
            else
            {
                return "";
            }
        }

        public static string get_TmpltTrns_Rec_Hstry(int tmpltDtID)
        {
            string strSQL = "SELECT a.created_by, to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), a.last_update_by, to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM accb.accb_trnsctn_templates_det a WHERE(a.detail_id = " + tmpltDtID + ")";
            string fnl_str = "";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSQL);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                fnl_str = "CREATED BY: " + Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][0].ToString())) +
                  "\r\nCREATION DATE: " + dtst.Tables[0].Rows[0][1].ToString() + "\r\nLAST UPDATE BY: " +
                  Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][2].ToString())) +
                  "\r\nLAST UPDATE DATE: " + dtst.Tables[0].Rows[0][3].ToString();
                return fnl_str;
            }
            else
            {
                return "";
            }
        }

        public static string get_TmpltTUsr_Rec_Hstry(long rowID)
        {
            string strSQL = "SELECT a.created_by, to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), a.last_update_by, to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM accb.accb_trnsctn_templates_usrs a WHERE(a.row_id = " + rowID + ")";
            string fnl_str = "";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSQL);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                fnl_str = "CREATED BY: " + Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][0].ToString())) +
                  "\r\nCREATION DATE: " + dtst.Tables[0].Rows[0][1].ToString() + "\r\nLAST UPDATE BY: " +
                  Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][2].ToString())) +
                  "\r\nLAST UPDATE DATE: " + dtst.Tables[0].Rows[0][3].ToString();
                return fnl_str;
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region "TRIAL BALANCE..."

        public static DataSet get_TrialBalance(int orgID, string trnsDate, int acntLvl,
            int rptSgmt1, int rptSgmt2, int rptSgmt3,
            int rptSgmt4, int rptSgmt5, int rptSgmt6,
            int rptSgmt7, int rptSgmt8, int rptSgmt9, int rptSgmt10)
        {
            String extrWhr = " WHERE depth<=" + acntLvl;
            string ttlSgmnts = rptSgmt1.ToString() + rptSgmt2.ToString() + rptSgmt3.ToString() + rptSgmt4.ToString() + rptSgmt5.ToString() + rptSgmt6.ToString() + rptSgmt7.ToString() + rptSgmt8.ToString() + rptSgmt9.ToString() + rptSgmt10.ToString();
            string sgmntWhere = "";

            if (ttlSgmnts != "-1-1-1-1-1-1-1-1-1-1")
            {
                sgmntWhere += " and ((a.is_prnt_accnt='1') or (1=1";
                if (rptSgmt1 > 0)
                {
                    sgmntWhere += " and a.accnt_seg1_val_id=" + rptSgmt1.ToString() + "";
                }
                if (rptSgmt2 > 0)
                {
                    sgmntWhere += " and a.accnt_seg2_val_id=" + rptSgmt2.ToString() + "";
                }
                if (rptSgmt3 > 0)
                {
                    sgmntWhere += " and a.accnt_seg3_val_id=" + rptSgmt3.ToString() + "";
                }
                if (rptSgmt4 > 0)
                {
                    sgmntWhere += " and a.accnt_seg4_val_id=" + rptSgmt4.ToString() + "";
                }
                if (rptSgmt5 > 0)
                {
                    sgmntWhere += " and a.accnt_seg5_val_id=" + rptSgmt5.ToString() + "";
                }
                if (rptSgmt6 > 0)
                {
                    sgmntWhere += " and a.accnt_seg6_val_id=" + rptSgmt6.ToString() + "";
                }
                if (rptSgmt7 > 0)
                {
                    sgmntWhere += " and a.accnt_seg7_val_id=" + rptSgmt7.ToString() + "";
                }
                if (rptSgmt8 > 0)
                {
                    sgmntWhere += " and a.accnt_seg8_val_id=" + rptSgmt8.ToString() + "";
                }
                if (rptSgmt9 > 0)
                {
                    sgmntWhere += " and a.accnt_seg9_val_id=" + rptSgmt9.ToString() + "";
                }
                if (rptSgmt10 > 0)
                {
                    sgmntWhere += " and a.accnt_seg10_val_id=" + rptSgmt10.ToString() + "";
                }
                sgmntWhere += "))";
            }

            trnsDate = DateTime.ParseExact(
         trnsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string strSql = "";//DISTINCT
            strSql = @"WITH RECURSIVE suborg(accnt_id, accnt_num, accnt_name, dbt_bal, crdt_bal, net_balance, 
as_at_date, is_prnt_accnt, accnt_type, accnt_typ_id, depth, path, cycle, space) AS 
      ( 
      SELECT a.accnt_id, a.accnt_num, a.accnt_name, (SELECT c.dbt_bal " +
          "FROM accb.accb_accnt_daily_bals c " +
          "WHERE(to_timestamp(c.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
          "','YYYY-MM-DD') and a.accnt_id = c.accnt_id)  ORDER BY to_timestamp(c.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), " +
              "(SELECT d.crdt_bal " +
          "FROM accb.accb_accnt_daily_bals d " +
          "WHERE(to_timestamp(d.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
          "','YYYY-MM-DD') and a.accnt_id = d.accnt_id)  ORDER BY to_timestamp(d.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), " +
          "(SELECT e.net_balance " +
          "FROM accb.accb_accnt_daily_bals e " +
          "WHERE(to_timestamp(e.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
          "','YYYY-MM-DD') and a.accnt_id = e.accnt_id)  ORDER BY to_timestamp(e.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), " +
          @"to_timestamp(b.as_at_date,'YYYY-MM-DD'), a.is_prnt_accnt, a.accnt_type,a.accnt_typ_id, 1, ARRAY[a.accnt_num||'']::character varying[], false, '' opad 
      FROM accb.accb_chart_of_accnts a LEFT OUTER JOIN  accb.accb_accnt_daily_bals b ON (a.accnt_id = b.accnt_id) " +
              "WHERE ((CASE WHEN a.prnt_accnt_id<=0 THEN a.control_account_id ELSE a.prnt_accnt_id END)=-1 AND (a.org_id = " + orgID + ") and " +
              "(a.control_account_id <= 0) and " +
              "(a.is_net_income = '0') " +
              "and (a.is_prnt_accnt='1' or (to_timestamp(b.as_at_date,'YYYY-MM-DD')=(SELECT " +
              "MAX(to_timestamp(f.as_at_date,'YYYY-MM-DD')) from " +
              "accb.accb_accnt_daily_bals f where f.accnt_id = a.accnt_id " +
              "and to_timestamp(f.as_at_date,'YYYY-MM-DD')<=to_timestamp('" + trnsDate +
         @"','YYYY-MM-DD'))))) 
      UNION ALL        
      SELECT a.accnt_id, a.accnt_num, a.accnt_name, (SELECT c.dbt_bal " +
          "FROM accb.accb_accnt_daily_bals c " +
          "WHERE(to_timestamp(c.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
          "','YYYY-MM-DD') and a.accnt_id = c.accnt_id)  ORDER BY to_timestamp(c.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), " +
              "(SELECT d.crdt_bal " +
          "FROM accb.accb_accnt_daily_bals d " +
          "WHERE(to_timestamp(d.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
          "','YYYY-MM-DD') and a.accnt_id = d.accnt_id)  ORDER BY to_timestamp(d.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), " +
          "(SELECT e.net_balance " +
          "FROM accb.accb_accnt_daily_bals e " +
          "WHERE(to_timestamp(e.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
          "','YYYY-MM-DD') and a.accnt_id = e.accnt_id)  ORDER BY to_timestamp(e.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), " +
          @"to_timestamp((SELECT MAX(d.as_at_date) " +
          "FROM accb.accb_accnt_daily_bals d " +
          "WHERE(to_timestamp(d.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
          @"','YYYY-MM-DD') and a.accnt_id = d.accnt_id)),'YYYY-MM-DD'), a.is_prnt_accnt, a.accnt_type,a.accnt_typ_id, sd.depth + 1, 
      path || a.accnt_num, 
      a.accnt_num = ANY(path), space || '           ' 
      FROM 
      accb.accb_chart_of_accnts a 
 , suborg AS sd 
      WHERE ((CASE WHEN a.prnt_accnt_id<=0 THEN a.control_account_id ELSE a.prnt_accnt_id END)=sd.accnt_id AND NOT cycle) 
       AND ((a.org_id = " + orgID + ") and " +
              "(a.control_account_id <= 0) and " +
              "(a.is_net_income = '0') " +
              ")" + sgmntWhere + @") 
      SELECT accnt_id, space||accnt_num, accnt_name, dbt_bal, crdt_bal, net_balance, as_at_date, is_prnt_accnt, accnt_type,accnt_typ_id, depth, path, cycle 
      FROM suborg " + extrWhr + " ORDER BY accnt_typ_id, path";

            /* and (a.is_prnt_accnt='1' or (to_timestamp(b.as_at_date,'YYYY-MM-DD')=(SELECT " +
              "MAX(to_timestamp(f.as_at_date,'YYYY-MM-DD')) from " +
              "accb.accb_accnt_daily_bals f where f.accnt_id = a.accnt_id " +
              "and to_timestamp(f.as_at_date,'YYYY-MM-DD')<=to_timestamp('" + trnsDate +
         @"','YYYY-MM-DD'))))
         
            strSql = "SELECT a.accnt_id, a.accnt_num, a.accnt_name, (SELECT c.dbt_bal " +
           "FROM accb.accb_accnt_daily_bals c " +
           "WHERE(to_timestamp(c.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
           "','YYYY-MM-DD') and a.accnt_id = c.accnt_id)  ORDER BY to_timestamp(c.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), " +
               "(SELECT d.crdt_bal " +
           "FROM accb.accb_accnt_daily_bals d " +
           "WHERE(to_timestamp(d.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
           "','YYYY-MM-DD') and a.accnt_id = d.accnt_id)  ORDER BY to_timestamp(d.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), "+
           "(SELECT e.net_balance " +
           "FROM accb.accb_accnt_daily_bals e " +
           "WHERE(to_timestamp(e.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
           "','YYYY-MM-DD') and a.accnt_id = e.accnt_id)  ORDER BY to_timestamp(e.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), "+
           "to_timestamp(b.as_at_date,'YYYY-MM-DD'), a.is_prnt_accnt, a.accnt_type " +
               "FROM accb.accb_chart_of_accnts a LEFT OUTER JOIN  accb.accb_accnt_daily_bals b ON (a.accnt_id = b.accnt_id) " +
               "WHERE ((a.org_id = " + orgID + ") and " +
               "(a.control_account_id <= 0) and " +
               "(a.is_net_income = '0') " +
               "and (a.is_prnt_accnt='1' or (to_timestamp(b.as_at_date,'YYYY-MM-DD')=(SELECT " +
               "MAX(to_timestamp(f.as_at_date,'YYYY-MM-DD')) from " +
               "accb.accb_accnt_daily_bals f where f.accnt_id = a.accnt_id " +
               "and to_timestamp(f.as_at_date,'YYYY-MM-DD')<=to_timestamp('" + trnsDate +
          "','YYYY-MM-DD'))))) ORDER BY a.accnt_typ_id, a.accnt_num";*/
            //Global.mnFrm.cmCde.showSQLNoPermsn(strSql);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.tbalsSQLStmnt = strSql;
            return dtst;
        }


        public static DataSet get_TrialBalance1(int orgID, string trnsDate, int strAcctID,
            int rptSgmt1, int rptSgmt2, int rptSgmt3,
            int rptSgmt4, int rptSgmt5, int rptSgmt6,
            int rptSgmt7, int rptSgmt8, int rptSgmt9, int rptSgmt10)
        {
            //String extrWhr = " WHERE depth<=" + acntLvl;
            string ttlSgmnts = rptSgmt1.ToString() + rptSgmt2.ToString() + rptSgmt3.ToString() + rptSgmt4.ToString() + rptSgmt5.ToString() + rptSgmt6.ToString() + rptSgmt7.ToString() + rptSgmt8.ToString() + rptSgmt9.ToString() + rptSgmt10.ToString();
            string sgmntWhere = "";
            if (ttlSgmnts != "-1-1-1-1-1-1-1-1-1-1")
            {
                sgmntWhere += " and ((a.is_prnt_accnt='1') or (1=1";
                if (rptSgmt1 > 0)
                {
                    sgmntWhere += " and a.accnt_seg1_val_id=" + rptSgmt1.ToString() + "";
                }
                if (rptSgmt2 > 0)
                {
                    sgmntWhere += " and a.accnt_seg2_val_id=" + rptSgmt2.ToString() + "";
                }
                if (rptSgmt3 > 0)
                {
                    sgmntWhere += " and a.accnt_seg3_val_id=" + rptSgmt3.ToString() + "";
                }
                if (rptSgmt4 > 0)
                {
                    sgmntWhere += " and a.accnt_seg4_val_id=" + rptSgmt4.ToString() + "";
                }
                if (rptSgmt5 > 0)
                {
                    sgmntWhere += " and a.accnt_seg5_val_id=" + rptSgmt5.ToString() + "";
                }
                if (rptSgmt6 > 0)
                {
                    sgmntWhere += " and a.accnt_seg6_val_id=" + rptSgmt6.ToString() + "";
                }
                if (rptSgmt7 > 0)
                {
                    sgmntWhere += " and a.accnt_seg7_val_id=" + rptSgmt7.ToString() + "";
                }
                if (rptSgmt8 > 0)
                {
                    sgmntWhere += " and a.accnt_seg8_val_id=" + rptSgmt8.ToString() + "";
                }
                if (rptSgmt9 > 0)
                {
                    sgmntWhere += " and a.accnt_seg9_val_id=" + rptSgmt9.ToString() + "";
                }
                if (rptSgmt10 > 0)
                {
                    sgmntWhere += " and a.accnt_seg10_val_id=" + rptSgmt10.ToString() + "";
                }
                sgmntWhere += "))";
            }
            trnsDate = DateTime.ParseExact(
         trnsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string strSql = "";//DISTINCT
            strSql = @"WITH RECURSIVE suborg(accnt_id, accnt_num, accnt_name, dbt_bal, crdt_bal, net_balance, 
as_at_date, is_prnt_accnt, accnt_type, accnt_typ_id, depth, path, cycle, space) AS 
      ( 
      SELECT a.accnt_id, a.accnt_num, a.accnt_name, (SELECT c.dbt_bal " +
          "FROM accb.accb_accnt_daily_bals c " +
          "WHERE(to_timestamp(c.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
          "','YYYY-MM-DD') and a.accnt_id = c.accnt_id)  ORDER BY to_timestamp(c.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), " +
              "(SELECT d.crdt_bal " +
          "FROM accb.accb_accnt_daily_bals d " +
          "WHERE(to_timestamp(d.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
          "','YYYY-MM-DD') and a.accnt_id = d.accnt_id)  ORDER BY to_timestamp(d.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), " +
          "(SELECT e.net_balance " +
          "FROM accb.accb_accnt_daily_bals e " +
          "WHERE(to_timestamp(e.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
          "','YYYY-MM-DD') and a.accnt_id = e.accnt_id)  ORDER BY to_timestamp(e.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), " +
          @"to_timestamp(b.as_at_date,'YYYY-MM-DD'), a.is_prnt_accnt, a.accnt_type,a.accnt_typ_id, 1, ARRAY[a.accnt_num||'']::character varying[], false, '' opad 
      FROM accb.accb_chart_of_accnts a LEFT OUTER JOIN  accb.accb_accnt_daily_bals b ON (a.accnt_id = b.accnt_id) " +
              "WHERE (a.accnt_id=" + strAcctID + " AND (a.org_id = " + orgID + ") and " +
              "(a.is_prnt_accnt='1' or (to_timestamp(b.as_at_date,'YYYY-MM-DD')=(SELECT " +
              "MAX(to_timestamp(f.as_at_date,'YYYY-MM-DD')) from " +
              "accb.accb_accnt_daily_bals f where f.accnt_id = a.accnt_id " +
              "and to_timestamp(f.as_at_date,'YYYY-MM-DD')<=to_timestamp('" + trnsDate +
         @"','YYYY-MM-DD'))))) 
      UNION ALL        
      SELECT a.accnt_id, a.accnt_num, a.accnt_name, (SELECT c.dbt_bal " +
          "FROM accb.accb_accnt_daily_bals c " +
          "WHERE(to_timestamp(c.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
          "','YYYY-MM-DD') and a.accnt_id = c.accnt_id)  ORDER BY to_timestamp(c.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), " +
              "(SELECT d.crdt_bal " +
          "FROM accb.accb_accnt_daily_bals d " +
          "WHERE(to_timestamp(d.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
          "','YYYY-MM-DD') and a.accnt_id = d.accnt_id)  ORDER BY to_timestamp(d.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), " +
          "(SELECT e.net_balance " +
          "FROM accb.accb_accnt_daily_bals e " +
          "WHERE(to_timestamp(e.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
          "','YYYY-MM-DD') and a.accnt_id = e.accnt_id)  ORDER BY to_timestamp(e.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), " +
          @"to_timestamp((SELECT MAX(d.as_at_date) " +
          "FROM accb.accb_accnt_daily_bals d " +
          "WHERE(to_timestamp(d.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
          @"','YYYY-MM-DD') and a.accnt_id = d.accnt_id)),'YYYY-MM-DD'), a.is_prnt_accnt, a.accnt_type,a.accnt_typ_id, sd.depth + 1, 
      path || a.accnt_num, 
      a.accnt_num = ANY(path), space || '           ' 
      FROM 
      accb.accb_chart_of_accnts a
 , suborg AS sd 
      WHERE ((CASE WHEN a.prnt_accnt_id<=0 THEN a.control_account_id ELSE a.prnt_accnt_id END)=sd.accnt_id AND NOT cycle) 
       AND ((a.org_id = " + orgID + ") )" + sgmntWhere + @") 
      SELECT accnt_id, space||accnt_num, accnt_name, dbt_bal, crdt_bal, net_balance, as_at_date, is_prnt_accnt, accnt_type,accnt_typ_id, depth, path, cycle 
      FROM suborg ORDER BY accnt_typ_id, path";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.tbalsSQLStmnt = strSql;
            return dtst;
            /*and " +
              "(a.is_prnt_accnt='1' or (to_timestamp(b.as_at_date,'YYYY-MM-DD')=(SELECT " +
              "MAX(to_timestamp(f.as_at_date,'YYYY-MM-DD')) from " +
              "accb.accb_accnt_daily_bals f where f.accnt_id = a.accnt_id " +
              "and to_timestamp(f.as_at_date,'YYYY-MM-DD')<=to_timestamp('" + trnsDate +
         @"','YYYY-MM-DD'))))*/
        }

        public static DataSet get_SubLdgrBalance(int orgID, string trnsDate)
        {
            trnsDate = DateTime.ParseExact(
         trnsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";//DISTINCT
            strSql = "SELECT a.accnt_id, a.accnt_num ||'.'||a.accnt_name, (SELECT c.dbt_bal " +
          "FROM accb.accb_accnt_daily_bals c " +
          "WHERE(to_timestamp(c.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
          "','YYYY-MM-DD') and a.accnt_id = c.accnt_id)  ORDER BY to_timestamp(c.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), " +
              "(SELECT d.crdt_bal " +
          "FROM accb.accb_accnt_daily_bals d " +
          "WHERE(to_timestamp(d.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
          "','YYYY-MM-DD') and a.accnt_id = d.accnt_id)  ORDER BY to_timestamp(d.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), (SELECT e.net_balance " +
          "FROM accb.accb_accnt_daily_bals e " +
          "WHERE(to_timestamp(e.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + trnsDate +
          "','YYYY-MM-DD') and a.accnt_id = e.accnt_id)  ORDER BY to_timestamp(e.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), to_timestamp(b.as_at_date,'YYYY-MM-DD'), a.has_sub_ledgers " +
              "FROM accb.accb_chart_of_accnts a LEFT OUTER JOIN  accb.accb_accnt_daily_bals b ON (a.accnt_id = b.accnt_id) " +
              "WHERE ((a.org_id = " + orgID + ") and " +
              "(a.has_sub_ledgers='1' or a.control_account_id > 0) and " +
              "(a.is_net_income = '0') " +
              "and ((to_timestamp(b.as_at_date,'YYYY-MM-DD')=(SELECT " +
              "MAX(to_timestamp(f.as_at_date,'YYYY-MM-DD')) from " +
              "accb.accb_accnt_daily_bals f where f.accnt_id = a.accnt_id " +
              "and to_timestamp(f.as_at_date,'YYYY-MM-DD')<=to_timestamp('" + trnsDate +
         "','YYYY-MM-DD'))))) ORDER BY a.accnt_typ_id, a.accnt_num";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.subldgr_SQL = strSql;
            return dtst;
        }
        #endregion

        #region "PROFIT & LOSS..."
        public static double get_Accnt_BalsTrnsSum(int accntID, string amntCol, string balsDte)
        {
            balsDte = DateTime.ParseExact(
         balsDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string strSql = "";
            strSql = "SELECT SUM(a." + amntCol + ") " +
              "FROM accb.accb_trnsctn_details a, accb.accb_chart_of_accnts b " +
              "WHERE ((a.accnt_id=b.accnt_id) and (a.accnt_id = " + accntID + " or b.control_account_id=" + accntID + ") and (to_timestamp(a.trnsctn_date, " +
              "'YYYY-MM-DD HH24:MI:SS') <= to_timestamp('" + balsDte +
              "', 'YYYY-MM-DD HH24:MI:SS')) and " +
              "(a.trns_status = '1'))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        /*accb.prnt_usr_trns_sum_rcsv(e.accnt_id, '{:fromDate} 
    00:00:00', '{:toDate} 23:59:59')*/

        public static double get_Accnt_Usr_TrnsSumRcsv(int accntID, string date1, string date2)
        {
            date1 = DateTime.ParseExact(
         date1, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            date2 = DateTime.ParseExact(
         date2, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";
            strSql = "SELECT accb.prnt_usr_trns_sum_rcsv(" + accntID + ",'" + date1 +
              "','" + date2 +
              "')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }
        public static double get_Accnt_Usr_TrnsSumRcsv2(int accntID, string date1, string date2,
            int rptSgmt1, int rptSgmt2, int rptSgmt3,
            int rptSgmt4, int rptSgmt5, int rptSgmt6,
            int rptSgmt7, int rptSgmt8, int rptSgmt9, int rptSgmt10)
        {
            date1 = DateTime.ParseExact(
         date1, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            date2 = DateTime.ParseExact(
         date2, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";
            strSql = "SELECT accb.prnt_usr_trns_sum_rcsv2(" + accntID + ",'" + date1 +
              "','" + date2 +
              "', " + rptSgmt1.ToString() +
          "," + rptSgmt2.ToString() +
          "," + rptSgmt3.ToString() +
          "," + rptSgmt4.ToString() +
          "," + rptSgmt5.ToString() +
          "," + rptSgmt6.ToString() +
          "," + rptSgmt7.ToString() +
          "," + rptSgmt8.ToString() +
          "," + rptSgmt9.ToString() +
          "," + rptSgmt10.ToString() + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static double get_Accnt_BalsSumRcsv(int accntID, string date1)
        {
            date1 = DateTime.ParseExact(
         date1, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string strSql = "";
            strSql = "SELECT accb.get_rcsv_prnt_accnt_bals(" + accntID + ",'" + date1 +
              "')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }
        public static double get_Accnt_BalsSumRcsv2(int accntID, string date1,
            int rptSgmt1, int rptSgmt2, int rptSgmt3,
            int rptSgmt4, int rptSgmt5, int rptSgmt6,
            int rptSgmt7, int rptSgmt8, int rptSgmt9, int rptSgmt10)
        {
            date1 = DateTime.ParseExact(
         date1, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string strSql = "";
            strSql = "SELECT accb.get_rcsv_prnt_accnt_bals2(" + accntID + ",'" + date1 +
              "', " + rptSgmt1.ToString() +
          "," + rptSgmt2.ToString() +
          "," + rptSgmt3.ToString() +
          "," + rptSgmt4.ToString() +
          "," + rptSgmt5.ToString() +
          "," + rptSgmt6.ToString() +
          "," + rptSgmt7.ToString() +
          "," + rptSgmt8.ToString() +
          "," + rptSgmt9.ToString() +
          "," + rptSgmt10.ToString() + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static double get_Accnt_Usr_TrnsSum(int accntID, string date1, string date2)
        {
            date1 = DateTime.ParseExact(
         date1, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            date2 = DateTime.ParseExact(
         date2, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string isNetIncme = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_chart_of_accnts", "accnt_id", "is_net_income", accntID);
            string strSql = "";
            if (isNetIncme == "1")
            {

                strSql = "SELECT SUM((CASE WHEN b.accnt_type='EX' THEN -1* a.net_amount ELSE a.net_amount END)) " +
             "FROM accb.accb_trnsctn_details a, accb.accb_chart_of_accnts b " +
             "WHERE ((a.accnt_id = b.accnt_id and b.org_id = " + Global.mnFrm.cmCde.Org_id + " and b.accnt_type IN ('R','EX')) and " +
             "(a.trns_status = '1') and (to_timestamp(a.trnsctn_date, " +
             "'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + date1 +
             "', 'YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + date2 +
             "', 'YYYY-MM-DD HH24:MI:SS')) and a.transctn_id NOT IN (select " +
             "b.transctn_id from accb.accb_trnsctn_details b where b.batch_id " +
             "IN (select c.batch_id from accb.accb_trnsctn_batches c where " +
             "c.batch_name like 'Period Close Process%' and c.batch_source = 'Period Close Process')))";
            }
            else
            {
                strSql = "SELECT SUM(a.net_amount) " +
                  "FROM accb.accb_trnsctn_details a " +
                  "WHERE ((a.accnt_id = " + accntID + ") and " +
                  "(a.trns_status = '1') and (to_timestamp(a.trnsctn_date, " +
                  "'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + date1 +
                  "', 'YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + date2 +
                  "', 'YYYY-MM-DD HH24:MI:SS')) and a.transctn_id NOT IN (select " +
                  "b.transctn_id from accb.accb_trnsctn_details b where b.batch_id " +
                  "IN (select c.batch_id from accb.accb_trnsctn_batches c where " +
                  "c.batch_name like 'Period Close Process%' and c.batch_source = 'Period Close Process')))";
            }
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static double get_CashFlow_Usr_TrnsSum(string accntClsfctn, string date1, string date2)
        {
            date1 = DateTime.ParseExact(
         date1, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            date2 = DateTime.ParseExact(
         date2, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";

            if (accntClsfctn.Contains("Net Income"))
            {
                // or c.is_retained_earnings='1'
                strSql = "SELECT SUM((CASE WHEN c.accnt_type='EX' THEN -1* a.net_amount ELSE a.net_amount END)) " +
          "FROM accb.accb_trnsctn_details a, accb.accb_chart_of_accnts c " +
        "WHERE ((a.accnt_id = c.accnt_id and c.org_id = " + Global.mnFrm.cmCde.Org_id +
        " and (c.accnt_type IN ('R','EX')) and c.has_sub_ledgers='0' and c.is_prnt_accnt='0') and " +
          "(a.trns_status = '1') and (to_timestamp(a.trnsctn_date, " +
          "'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + date1 +
          "', 'YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + date2 +
          "', 'YYYY-MM-DD HH24:MI:SS')) and a.transctn_id NOT IN (select " +
          "b.transctn_id from accb.accb_trnsctn_details b where b.batch_id " +
          "IN (select c.batch_id from accb.accb_trnsctn_batches c where " +
          "c.batch_name like 'Period Close Process%' and c.batch_source = 'Period Close Process')))";

            }
            else
            {
                strSql = "SELECT SUM(a.net_amount) " +
                  "FROM accb.accb_trnsctn_details a, accb.accb_chart_of_accnts c " +
               "WHERE ((a.accnt_id = c.accnt_id and c.org_id = " + Global.mnFrm.cmCde.Org_id +
               " and c.account_clsfctn IN ('" + accntClsfctn.Replace("'", "''") + "') and c.has_sub_ledgers='0' and c.is_prnt_accnt='0') and " +
                  "(a.trns_status = '1') and (to_timestamp(a.trnsctn_date, " +
                  "'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + date1 +
                  "', 'YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + date2 +
                  "', 'YYYY-MM-DD HH24:MI:SS')) and a.transctn_id NOT IN (select " +
                  "b.transctn_id from accb.accb_trnsctn_details b where b.batch_id " +
                  "IN (select c.batch_id from accb.accb_trnsctn_batches c where " +
                  "c.batch_name like 'Period Close Process%' and c.batch_source = 'Period Close Process')))";
            }
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static double get_CashFlow_Usr_TrnsSum2(string accntClsfctn, string date1, string date2,
            int rptSgmt1, int rptSgmt2, int rptSgmt3,
            int rptSgmt4, int rptSgmt5, int rptSgmt6,
            int rptSgmt7, int rptSgmt8, int rptSgmt9, int rptSgmt10)
        {
            //String extrWhr = " WHERE depth<=" + acntLvl;
            string ttlSgmnts = rptSgmt1.ToString() + rptSgmt2.ToString() + rptSgmt3.ToString() + rptSgmt4.ToString() + rptSgmt5.ToString() + rptSgmt6.ToString() + rptSgmt7.ToString() + rptSgmt8.ToString() + rptSgmt9.ToString() + rptSgmt10.ToString();
            string sgmntWhere = "";
            date1 = DateTime.ParseExact(
         date1, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            date2 = DateTime.ParseExact(
         date2, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";

            if (accntClsfctn.Contains("Net Income"))
            {
                // or c.is_retained_earnings='1'
                strSql = "SELECT SUM((CASE WHEN c.accnt_type='EX' THEN -1* a.net_amount ELSE a.net_amount END)) " +
          "FROM accb.accb_trnsctn_details a, accb.accb_chart_of_accnts c " +
        "WHERE ((a.accnt_id = c.accnt_id and c.org_id = " + Global.mnFrm.cmCde.Org_id +
        " and (c.accnt_type IN ('R','EX')) and c.has_sub_ledgers='0' and c.is_prnt_accnt='0') AND ('" + ttlSgmnts + "' = '-1-1-1-1-1-1-1-1-1-1' " +
               "or c.is_prnt_accnt = '1' or c.has_sub_ledgers='1' " +
               "or (c.accnt_seg1_val_id = " + rptSgmt1 + " and " + rptSgmt1 + " > 0) " +
               "or (c.accnt_seg2_val_id = " + rptSgmt2 + " and " + rptSgmt2 + " > 0) " +
               "or (c.accnt_seg3_val_id = " + rptSgmt3 + " and " + rptSgmt3 + " > 0) " +
               "or (c.accnt_seg4_val_id = " + rptSgmt4 + " and " + rptSgmt4 + " > 0) " +
               "or (c.accnt_seg5_val_id = " + rptSgmt5 + " and " + rptSgmt5 + " > 0) " +
               "or (c.accnt_seg6_val_id = " + rptSgmt6 + " and " + rptSgmt6 + " > 0) " +
               "or (c.accnt_seg7_val_id = " + rptSgmt7 + " and " + rptSgmt7 + " > 0) " +
               "or (c.accnt_seg8_val_id = " + rptSgmt8 + " and " + rptSgmt8 + " > 0) " +
               "or (c.accnt_seg9_val_id = " + rptSgmt9 + " and " + rptSgmt9 + " > 0) " +
               "or (c.accnt_seg10_val_id = " + rptSgmt10 + " and " + rptSgmt10 + " > 0)) and " +
          "(a.trns_status = '1') and (to_timestamp(a.trnsctn_date, " +
          "'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + date1 +
          "', 'YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + date2 +
          "', 'YYYY-MM-DD HH24:MI:SS')) and a.transctn_id NOT IN (select " +
          "b.transctn_id from accb.accb_trnsctn_details b where b.batch_id " +
          "IN (select c.batch_id from accb.accb_trnsctn_batches c where " +
          "c.batch_name like 'Period Close Process%' and c.batch_source = 'Period Close Process')))";

            }
            else
            {
                strSql = "SELECT SUM(a.net_amount) " +
                  "FROM accb.accb_trnsctn_details a, accb.accb_chart_of_accnts c " +
               "WHERE ((a.accnt_id = c.accnt_id and c.org_id = " + Global.mnFrm.cmCde.Org_id +
               " and c.account_clsfctn IN ('" + accntClsfctn.Replace("'", "''") + "') and c.has_sub_ledgers='0' and c.is_prnt_accnt='0') AND ('" + ttlSgmnts + "' = '-1-1-1-1-1-1-1-1-1-1' " +
               "or c.is_prnt_accnt = '1' or c.has_sub_ledgers='1' " +
               "or (c.accnt_seg1_val_id = " + rptSgmt1 + " and " + rptSgmt1 + " > 0) " +
               "or (c.accnt_seg2_val_id = " + rptSgmt2 + " and " + rptSgmt2 + " > 0) " +
               "or (c.accnt_seg3_val_id = " + rptSgmt3 + " and " + rptSgmt3 + " > 0) " +
               "or (c.accnt_seg4_val_id = " + rptSgmt4 + " and " + rptSgmt4 + " > 0) " +
               "or (c.accnt_seg5_val_id = " + rptSgmt5 + " and " + rptSgmt5 + " > 0) " +
               "or (c.accnt_seg6_val_id = " + rptSgmt6 + " and " + rptSgmt6 + " > 0) " +
               "or (c.accnt_seg7_val_id = " + rptSgmt7 + " and " + rptSgmt7 + " > 0) " +
               "or (c.accnt_seg8_val_id = " + rptSgmt8 + " and " + rptSgmt8 + " > 0) " +
               "or (c.accnt_seg9_val_id = " + rptSgmt9 + " and " + rptSgmt9 + " > 0) " +
               "or (c.accnt_seg10_val_id = " + rptSgmt10 + " and " + rptSgmt10 + " > 0)) and " +
                  "(a.trns_status = '1') and (to_timestamp(a.trnsctn_date, " +
                  "'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + date1 +
                  "', 'YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + date2 +
                  "', 'YYYY-MM-DD HH24:MI:SS')) and a.transctn_id NOT IN (select " +
                  "b.transctn_id from accb.accb_trnsctn_details b where b.batch_id " +
                  "IN (select c.batch_id from accb.accb_trnsctn_batches c where " +
                  "c.batch_name like 'Period Close Process%' and c.batch_source = 'Period Close Process')))";
            }
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static double get_Accnt_TrnsSum(int accntID, string date1, string date2)
        {
            date1 = DateTime.ParseExact(
         date1, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            date2 = DateTime.ParseExact(
         date2, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";
            strSql = "SELECT SUM(a.net_amount) " +
              "FROM accb.accb_trnsctn_details a " +
              "WHERE ((a.accnt_id = " + accntID + ") and " +
              "(a.trns_status = '1') and (to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + date1 +
              "','YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + date2 +
              "','YYYY-MM-DD HH24:MI:SS')))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static double get_AccntType_TrnsSum(int orgid, string acctype, string date1, string date2)
        {
            date1 = DateTime.ParseExact(
         date1, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            date2 = DateTime.ParseExact(
         date2, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";
            strSql = "SELECT SUM(a.net_amount) " +
              "FROM accb.accb_trnsctn_details a , accb.accb_chart_of_accnts b " +
          "WHERE ((a.accnt_id = b.accnt_id) and (b.org_id = " + orgid + ") and (b.accnt_type = '" + acctype + "') and " +
              "(a.trns_status = '1') and (to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + date1 +
              "','YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + date2 +
              "','YYYY-MM-DD HH24:MI:SS')))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static DataSet get_PrftNLoss_Accnts(int orgid, int acntLvl,
            int rptSgmt1, int rptSgmt2, int rptSgmt3,
            int rptSgmt4, int rptSgmt5, int rptSgmt6,
            int rptSgmt7, int rptSgmt8, int rptSgmt9, int rptSgmt10)
        {
            String extrWhr = " WHERE depth<=" + acntLvl;
            string ttlSgmnts = rptSgmt1.ToString() + rptSgmt2.ToString() + rptSgmt3.ToString() + rptSgmt4.ToString() + rptSgmt5.ToString() + rptSgmt6.ToString() + rptSgmt7.ToString() + rptSgmt8.ToString() + rptSgmt9.ToString() + rptSgmt10.ToString();
            string sgmntWhere = "";
            if (ttlSgmnts != "-1-1-1-1-1-1-1-1-1-1")
            {
                sgmntWhere += " and ((d.is_prnt_accnt='1') or (1=1";
                if (rptSgmt1 > 0)
                {
                    sgmntWhere += " and d.accnt_seg1_val_id=" + rptSgmt1.ToString() + "";
                }
                if (rptSgmt2 > 0)
                {
                    sgmntWhere += " and d.accnt_seg2_val_id=" + rptSgmt2.ToString() + "";
                }
                if (rptSgmt3 > 0)
                {
                    sgmntWhere += " and d.accnt_seg3_val_id=" + rptSgmt3.ToString() + "";
                }
                if (rptSgmt4 > 0)
                {
                    sgmntWhere += " and d.accnt_seg4_val_id=" + rptSgmt4.ToString() + "";
                }
                if (rptSgmt5 > 0)
                {
                    sgmntWhere += " and d.accnt_seg5_val_id=" + rptSgmt5.ToString() + "";
                }
                if (rptSgmt6 > 0)
                {
                    sgmntWhere += " and d.accnt_seg6_val_id=" + rptSgmt6.ToString() + "";
                }
                if (rptSgmt7 > 0)
                {
                    sgmntWhere += " and d.accnt_seg7_val_id=" + rptSgmt7.ToString() + "";
                }
                if (rptSgmt8 > 0)
                {
                    sgmntWhere += " and d.accnt_seg8_val_id=" + rptSgmt8.ToString() + "";
                }
                if (rptSgmt9 > 0)
                {
                    sgmntWhere += " and d.accnt_seg9_val_id=" + rptSgmt9.ToString() + "";
                }
                if (rptSgmt10 > 0)
                {
                    sgmntWhere += " and d.accnt_seg10_val_id=" + rptSgmt10.ToString() + "";
                }
                sgmntWhere += "))";
            }
            string strSql = "";
            strSql = @"WITH RECURSIVE suborg(accnt_id, accnt_num, accnt_name, is_prnt_accnt, accnt_type, control_account_id, has_sub_ledgers, accnt_typ_id, depth, path, cycle, space) AS 
      ( 
      SELECT e.accnt_id, e.accnt_num, e.accnt_name, e.is_prnt_accnt, e.accnt_type, 
      (CASE WHEN e.prnt_accnt_id<=0 THEN e.control_account_id ELSE e.prnt_accnt_id END) control_account_id, e.has_sub_ledgers,e.accnt_typ_id,1, ARRAY[e.accnt_num||'']::character varying[], false, '' opad 
      FROM accb.accb_chart_of_accnts e 
      WHERE (CASE WHEN e.prnt_accnt_id<=0 THEN e.control_account_id ELSE e.prnt_accnt_id END) = -1 
      and (e.org_id = " + orgid + @") and (e.accnt_type = 'R' or e.accnt_type = 'EX')
      UNION ALL        
      SELECT d.accnt_id, d.accnt_num, d.accnt_name, d.is_prnt_accnt, d.accnt_type, 
      (CASE WHEN d.prnt_accnt_id<=0 THEN d.control_account_id ELSE d.prnt_accnt_id END) control_account_id, d.has_sub_ledgers, d.accnt_typ_id, sd.depth + 1, 
      path || d.accnt_num, 
      d.accnt_num = ANY(path), space || '           ' 
      FROM 
      accb.accb_chart_of_accnts AS d, 
      suborg AS sd 
      WHERE (CASE WHEN d.prnt_accnt_id<=0 THEN d.control_account_id ELSE d.prnt_accnt_id END) = sd.accnt_id AND NOT cycle
       and (d.org_id = " + orgid + @") and (d.accnt_type = 'R' or d.accnt_type = 'EX')" + sgmntWhere + @") 
      SELECT accnt_id, space||accnt_num, accnt_name,is_prnt_accnt, accnt_type, has_sub_ledgers, accnt_typ_id, depth, path, cycle 
      FROM suborg " + extrWhr + " ORDER BY accnt_typ_id, path";

            /*strSql = "SELECT a.accnt_id, a.accnt_num, a.accnt_name, a.is_prnt_accnt, a.accnt_type, a.has_sub_ledgers " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE ((a.org_id = " + orgid + ") and " +
          "(a.accnt_type = 'R' or a.accnt_type = 'EX')) ORDER BY a.accnt_typ_id, a.prnt_accnt_id, a.control_account_id, a.accnt_num";
            */
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.pnlSQLStmnt = strSql;
            return dtst;
            /*a.control_account_id <= 0 and (a.has_sub_ledgers<=0)*/
        }

        public static DataSet get_PrftNLoss_Accnts1(int orgid, int accntID,
            int rptSgmt1, int rptSgmt2, int rptSgmt3,
            int rptSgmt4, int rptSgmt5, int rptSgmt6,
            int rptSgmt7, int rptSgmt8, int rptSgmt9, int rptSgmt10)
        {
            string ttlSgmnts = rptSgmt1.ToString() + rptSgmt2.ToString() + rptSgmt3.ToString() + rptSgmt4.ToString() + rptSgmt5.ToString() + rptSgmt6.ToString() + rptSgmt7.ToString() + rptSgmt8.ToString() + rptSgmt9.ToString() + rptSgmt10.ToString();
            string sgmntWhere = "";
            if (ttlSgmnts != "-1-1-1-1-1-1-1-1-1-1")
            {
                sgmntWhere += " and ((d.is_prnt_accnt='1') or (1=1";
                if (rptSgmt1 > 0)
                {
                    sgmntWhere += " and d.accnt_seg1_val_id=" + rptSgmt1.ToString() + "";
                }
                if (rptSgmt2 > 0)
                {
                    sgmntWhere += " and d.accnt_seg2_val_id=" + rptSgmt2.ToString() + "";
                }
                if (rptSgmt3 > 0)
                {
                    sgmntWhere += " and d.accnt_seg3_val_id=" + rptSgmt3.ToString() + "";
                }
                if (rptSgmt4 > 0)
                {
                    sgmntWhere += " and d.accnt_seg4_val_id=" + rptSgmt4.ToString() + "";
                }
                if (rptSgmt5 > 0)
                {
                    sgmntWhere += " and d.accnt_seg5_val_id=" + rptSgmt5.ToString() + "";
                }
                if (rptSgmt6 > 0)
                {
                    sgmntWhere += " and d.accnt_seg6_val_id=" + rptSgmt6.ToString() + "";
                }
                if (rptSgmt7 > 0)
                {
                    sgmntWhere += " and d.accnt_seg7_val_id=" + rptSgmt7.ToString() + "";
                }
                if (rptSgmt8 > 0)
                {
                    sgmntWhere += " and d.accnt_seg8_val_id=" + rptSgmt8.ToString() + "";
                }
                if (rptSgmt9 > 0)
                {
                    sgmntWhere += " and d.accnt_seg9_val_id=" + rptSgmt9.ToString() + "";
                }
                if (rptSgmt10 > 0)
                {
                    sgmntWhere += " and d.accnt_seg10_val_id=" + rptSgmt10.ToString() + "";
                }
                sgmntWhere += "))";
            }
            string strSql = "";
            strSql = @"WITH RECURSIVE suborg(accnt_id, accnt_num, accnt_name, is_prnt_accnt, accnt_type, control_account_id, has_sub_ledgers, accnt_typ_id, depth, path, cycle, space) AS 
      ( 
      SELECT e.accnt_id, e.accnt_num, e.accnt_name, e.is_prnt_accnt, e.accnt_type, 
      (CASE WHEN e.prnt_accnt_id<=0 THEN e.control_account_id ELSE e.prnt_accnt_id END) control_account_id, e.has_sub_ledgers,e.accnt_typ_id,1, ARRAY[e.accnt_num||'']::character varying[], false, '' opad 
      FROM accb.accb_chart_of_accnts e 
      WHERE e.accnt_id = " + accntID + @" 
      and (e.org_id = " + orgid + @") and (e.accnt_type = 'R' or e.accnt_type = 'EX')
      UNION ALL        
      SELECT d.accnt_id, d.accnt_num, d.accnt_name, d.is_prnt_accnt, d.accnt_type, 
      (CASE WHEN d.prnt_accnt_id<=0 THEN d.control_account_id ELSE d.prnt_accnt_id END) control_account_id, d.has_sub_ledgers, d.accnt_typ_id, sd.depth + 1, 
      path || d.accnt_num, 
      d.accnt_num = ANY(path), space || '           ' 
      FROM 
      accb.accb_chart_of_accnts AS d, 
      suborg AS sd 
      WHERE (CASE WHEN d.prnt_accnt_id<=0 THEN d.control_account_id ELSE d.prnt_accnt_id END) = sd.accnt_id AND NOT cycle
       and (d.org_id = " + orgid + @") and (d.accnt_type = 'R' or d.accnt_type = 'EX')" + sgmntWhere + @") 
      SELECT accnt_id, space||accnt_num, accnt_name,is_prnt_accnt, accnt_type, has_sub_ledgers, accnt_typ_id, depth, path, cycle 
      FROM suborg 
      ORDER BY accnt_typ_id, path";

            /*strSql = "SELECT a.accnt_id, a.accnt_num, a.accnt_name, a.is_prnt_accnt, a.accnt_type, a.has_sub_ledgers " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE ((a.org_id = " + orgid + ") and " +
          "(a.accnt_type = 'R' or a.accnt_type = 'EX')) ORDER BY a.accnt_typ_id, a.prnt_accnt_id, a.control_account_id, a.accnt_num";
            */
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.pnlSQLStmnt = strSql;
            return dtst;
            /*a.control_account_id <= 0 and (a.has_sub_ledgers<=0)*/
        }

        public static DataSet get_Type_Accnts(int orgid, string accType,
            int rptSgmt1, int rptSgmt2, int rptSgmt3,
            int rptSgmt4, int rptSgmt5, int rptSgmt6,
            int rptSgmt7, int rptSgmt8, int rptSgmt9, int rptSgmt10)
        {
            //String extrWhr = " WHERE depth<=" + acntLvl;
            string ttlSgmnts = rptSgmt1.ToString() + rptSgmt2.ToString() + rptSgmt3.ToString() + rptSgmt4.ToString() + rptSgmt5.ToString() + rptSgmt6.ToString() + rptSgmt7.ToString() + rptSgmt8.ToString() + rptSgmt9.ToString() + rptSgmt10.ToString();
            string sgmntWhere = "";
            string strSql = "";
            strSql = @"WITH RECURSIVE suborg(accnt_id, accnt_num, accnt_name, is_prnt_accnt, accnt_type, control_account_id, has_sub_ledgers, accnt_typ_id, depth, path, cycle, space) AS 
      ( 
      SELECT e.accnt_id, e.accnt_num, e.accnt_name, e.is_prnt_accnt, e.accnt_type, 
      (CASE WHEN e.prnt_accnt_id<=0 THEN e.control_account_id ELSE e.prnt_accnt_id END) control_account_id, e.has_sub_ledgers,e.accnt_typ_id, 1, ARRAY[e.accnt_num||'']::character varying[], false, '' opad 
      FROM accb.accb_chart_of_accnts e 
      WHERE (CASE WHEN e.prnt_accnt_id<=0 THEN e.control_account_id ELSE e.prnt_accnt_id END) = -1 
      and (e.org_id = " + orgid + @") and (e.accnt_type IN ('" + accType/*Don't Add Replace("'", "''")*/ + @"'))
      UNION ALL        
      SELECT d.accnt_id, d.accnt_num, d.accnt_name, d.is_prnt_accnt, d.accnt_type, 
      (CASE WHEN d.prnt_accnt_id<=0 THEN d.control_account_id ELSE d.prnt_accnt_id END) control_account_id, d.has_sub_ledgers, d.accnt_typ_id, sd.depth + 1, 
      path || d.accnt_num, 
      d.accnt_num = ANY(path), space || '           ' 
      FROM 
      accb.accb_chart_of_accnts AS d, 
      suborg AS sd 
      WHERE (CASE WHEN d.prnt_accnt_id<=0 THEN d.control_account_id ELSE d.prnt_accnt_id END) = sd.accnt_id AND NOT cycle
       and (d.org_id = " + orgid + @") and (d.accnt_type IN ('" + accType/*Don't Add Replace("'", "''")*/ + @"')) AND ('" + ttlSgmnts + "' = '-1-1-1-1-1-1-1-1-1-1' " +
               "or d.is_prnt_accnt = '1' or d.has_sub_ledgers='1' " +
               "or (d.accnt_seg1_val_id = " + rptSgmt1 + " and " + rptSgmt1 + " > 0) " +
               "or (d.accnt_seg2_val_id = " + rptSgmt2 + " and " + rptSgmt2 + " > 0) " +
               "or (d.accnt_seg3_val_id = " + rptSgmt3 + " and " + rptSgmt3 + " > 0) " +
               "or (d.accnt_seg4_val_id = " + rptSgmt4 + " and " + rptSgmt4 + " > 0) " +
               "or (d.accnt_seg5_val_id = " + rptSgmt5 + " and " + rptSgmt5 + " > 0) " +
               "or (d.accnt_seg6_val_id = " + rptSgmt6 + " and " + rptSgmt6 + " > 0) " +
               "or (d.accnt_seg7_val_id = " + rptSgmt7 + " and " + rptSgmt7 + " > 0) " +
               "or (d.accnt_seg8_val_id = " + rptSgmt8 + " and " + rptSgmt8 + " > 0) " +
               "or (d.accnt_seg9_val_id = " + rptSgmt9 + " and " + rptSgmt9 + " > 0) " +
               "or (d.accnt_seg10_val_id = " + rptSgmt10 + " and " + rptSgmt10 + " > 0)))" +
      @"SELECT accnt_id, space||accnt_num, accnt_name,is_prnt_accnt, accnt_type, has_sub_ledgers, accnt_typ_id, depth, path, cycle 
      FROM suborg 
      ORDER BY accnt_typ_id, path";

            /*strSql = "SELECT a.accnt_id, a.accnt_num, a.accnt_name, a.is_prnt_accnt, a.accnt_type, a.has_sub_ledgers " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE ((a.org_id = " + orgid + ") and " +
          "(a.accnt_type = 'R' or a.accnt_type = 'EX')) ORDER BY a.accnt_typ_id, a.prnt_accnt_id, a.control_account_id, a.accnt_num";
            */
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.periodSQLStmnt = strSql;
            return dtst;
            /*a.control_account_id <= 0 and (a.has_sub_ledgers<=0)*/
        }

        public static DataSet get_Clsfctn_Accnts(int orgid, string accClsfctn)
        {
            string strSql = "";
            // or e.has_sub_ledgers='1'
            /*and (e.account_clsfctn IN ('" + accClsfctn.Replace("'", "''") + @"'))*/
            strSql = @"Select accnt_id, substr(MIN(acc_num),12), accnt_name,is_prnt_accnt, accnt_type, has_sub_ledgers,accnt_typ_id, MAX(depth), MIN(path), control_account_id, trim(acc_num) 
      FROM (WITH RECURSIVE suborg(accnt_id, accnt_num, accnt_name, is_prnt_accnt, accnt_type, control_account_id, has_sub_ledgers, accnt_typ_id,depth, path, cycle, space) AS 
      (
      SELECT e.accnt_id, e.accnt_num, e.accnt_name, e.is_prnt_accnt, e.accnt_type, 
      (CASE WHEN e.prnt_accnt_id<=0 THEN e.control_account_id ELSE e.prnt_accnt_id END) control_account_id, e.has_sub_ledgers,e.accnt_typ_id,1, ARRAY[e.accnt_num||'']::character varying[], false, '' opad 
      FROM accb.accb_chart_of_accnts e 
      WHERE (e.has_sub_ledgers ='1' or e.is_prnt_accnt ='1') 
       and (e.org_id = " + orgid + @") 
      UNION ALL        
      SELECT d.accnt_id, d.accnt_num, d.accnt_name, d.is_prnt_accnt, d.accnt_type, 
      (CASE WHEN d.prnt_accnt_id<=0 THEN d.control_account_id ELSE d.prnt_accnt_id END) control_account_id, d.has_sub_ledgers, d.accnt_typ_id, sd.depth + 1, 
      path || d.accnt_num, 
      d.accnt_num = ANY(path), space || '           ' 
      FROM 
      accb.accb_chart_of_accnts AS d, 
      suborg AS sd 
      WHERE (CASE WHEN d.prnt_accnt_id<=0 THEN d.control_account_id ELSE d.prnt_accnt_id END) = sd.accnt_id AND NOT cycle
       and (d.org_id = " + orgid + @") and (d.account_clsfctn IN ('" + accClsfctn.Replace("'", "''") + @"')))  
      SELECT accnt_id, space||accnt_num acc_num, accnt_name,is_prnt_accnt, accnt_type, has_sub_ledgers, accnt_typ_id, depth, path, cycle, control_account_id   
      FROM suborg 
      ORDER BY path) tbl1 WHERE tbl1.depth>1 GROUP BY 1,3,4,5,6,7,10,11 ORDER BY 7,9,10,11,8";

            /*strSql = "SELECT a.accnt_id, a.accnt_num, a.accnt_name, a.is_prnt_accnt, a.accnt_type, a.has_sub_ledgers " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE ((a.org_id = " + orgid + ") and " +
          "(a.accnt_type = 'R' or a.accnt_type = 'EX')) ORDER BY a.accnt_typ_id, a.prnt_accnt_id, a.control_account_id, a.accnt_num";
            */
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.cashFlowSQLStmnt = strSql;
            return dtst;
            /*a.control_account_id <= 0 and (a.has_sub_ledgers<=0)*/
        }

        public static DataSet get_Clsfctn_Accnts2(int orgid, string accClsfctn,
            int rptSgmt1, int rptSgmt2, int rptSgmt3,
            int rptSgmt4, int rptSgmt5, int rptSgmt6,
            int rptSgmt7, int rptSgmt8, int rptSgmt9, int rptSgmt10)
        {
            //String extrWhr = " WHERE depth<=" + acntLvl;
            string ttlSgmnts = rptSgmt1.ToString() + rptSgmt2.ToString() + rptSgmt3.ToString() + rptSgmt4.ToString() + rptSgmt5.ToString() + rptSgmt6.ToString() + rptSgmt7.ToString() + rptSgmt8.ToString() + rptSgmt9.ToString() + rptSgmt10.ToString();
            string sgmntWhere = "";
            string strSql = "";
            // or e.has_sub_ledgers='1'
            /*and (e.account_clsfctn IN ('" + accClsfctn.Replace("'", "''") + @"'))*/
            strSql = @"Select accnt_id, substr(MIN(acc_num),12), accnt_name,is_prnt_accnt, accnt_type, has_sub_ledgers,accnt_typ_id, MAX(depth), MIN(path), control_account_id, trim(acc_num) 
      FROM (WITH RECURSIVE suborg(accnt_id, accnt_num, accnt_name, is_prnt_accnt, accnt_type, control_account_id, has_sub_ledgers, accnt_typ_id,depth, path, cycle, space) AS 
      (
      SELECT e.accnt_id, e.accnt_num, e.accnt_name, e.is_prnt_accnt, e.accnt_type, 
      (CASE WHEN e.prnt_accnt_id<=0 THEN e.control_account_id ELSE e.prnt_accnt_id END) control_account_id, e.has_sub_ledgers,e.accnt_typ_id,1, ARRAY[e.accnt_num||'']::character varying[], false, '' opad 
      FROM accb.accb_chart_of_accnts e 
      WHERE (e.has_sub_ledgers ='1' or e.is_prnt_accnt ='1') 
       and (e.org_id = " + orgid + @") 
      UNION ALL        
      SELECT d.accnt_id, d.accnt_num, d.accnt_name, d.is_prnt_accnt, d.accnt_type, 
      (CASE WHEN d.prnt_accnt_id<=0 THEN d.control_account_id ELSE d.prnt_accnt_id END) control_account_id, d.has_sub_ledgers, d.accnt_typ_id, sd.depth + 1, 
      path || d.accnt_num, 
      d.accnt_num = ANY(path), space || '           ' 
      FROM 
      accb.accb_chart_of_accnts AS d, 
      suborg AS sd 
      WHERE (CASE WHEN d.prnt_accnt_id<=0 THEN d.control_account_id ELSE d.prnt_accnt_id END) = sd.accnt_id AND NOT cycle
       and (d.org_id = " + orgid + @") and (d.account_clsfctn IN ('" + accClsfctn.Replace("'", "''") + @"')) AND ('" + ttlSgmnts + "' = '-1-1-1-1-1-1-1-1-1-1' " +
               "or d.is_prnt_accnt = '1' or d.has_sub_ledgers='1' " +
               "or (d.accnt_seg1_val_id = " + rptSgmt1 + " and " + rptSgmt1 + " > 0) " +
               "or (d.accnt_seg2_val_id = " + rptSgmt2 + " and " + rptSgmt2 + " > 0) " +
               "or (d.accnt_seg3_val_id = " + rptSgmt3 + " and " + rptSgmt3 + " > 0) " +
               "or (d.accnt_seg4_val_id = " + rptSgmt4 + " and " + rptSgmt4 + " > 0) " +
               "or (d.accnt_seg5_val_id = " + rptSgmt5 + " and " + rptSgmt5 + " > 0) " +
               "or (d.accnt_seg6_val_id = " + rptSgmt6 + " and " + rptSgmt6 + " > 0) " +
               "or (d.accnt_seg7_val_id = " + rptSgmt7 + " and " + rptSgmt7 + " > 0) " +
               "or (d.accnt_seg8_val_id = " + rptSgmt8 + " and " + rptSgmt8 + " > 0) " +
               "or (d.accnt_seg9_val_id = " + rptSgmt9 + " and " + rptSgmt9 + " > 0) " +
               "or (d.accnt_seg10_val_id = " + rptSgmt10 + " and " + rptSgmt10 + " > 0))) " +
      @"SELECT accnt_id, space||accnt_num acc_num, accnt_name,is_prnt_accnt, accnt_type, has_sub_ledgers, accnt_typ_id, depth, path, cycle, control_account_id   
      FROM suborg 
      ORDER BY path) tbl1 WHERE tbl1.depth>1 GROUP BY 1,3,4,5,6,7,10,11 ORDER BY 7,9,10,11,8";

            /*strSql = "SELECT a.accnt_id, a.accnt_num, a.accnt_name, a.is_prnt_accnt, a.accnt_type, a.has_sub_ledgers " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE ((a.org_id = " + orgid + ") and " +
          "(a.accnt_type = 'R' or a.accnt_type = 'EX')) ORDER BY a.accnt_typ_id, a.prnt_accnt_id, a.control_account_id, a.accnt_num";
            */
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.cashFlowSQLStmnt = strSql;
            return dtst;
            /*a.control_account_id <= 0 and (a.has_sub_ledgers<=0)*/
        }

        public static List<string> getBdgtDates(
        string startDte, string endDte, string periodTyp)
        {
            DateTime dte1 = DateTime.Parse(DateTime.Parse(startDte).ToString("dd-MMM-yyyy 00:00:00"));
            DateTime dte2 = DateTime.Parse(DateTime.Parse(endDte).ToString("dd-MMM-yyyy 23:59:59"));
            List<string> resArray = new List<string>();
            string nwstr = dte1.ToString("dd-MMM-yyyy 00:00:00");
            resArray.Add(nwstr);
            bool evenOdd = false;//false-begin date true - end date
            if (periodTyp == "Yearly")
            {
                do
                {
                    evenOdd = !evenOdd;
                    if (evenOdd)
                    {
                        nwstr = DateTime.Parse(dte1.AddMonths(12).AddDays(-1).ToString("dd-MMM-yyyy 23:59:59")).ToString("dd-MMM-yyyy 23:59:59");
                        dte1 = DateTime.Parse(DateTime.Parse(nwstr).AddDays(1).ToString("dd-MMM-yyyy 00:00:00"));
                    }
                    else
                    {
                        nwstr = dte1.ToString("dd-MMM-yyyy 00:00:00");
                    }
                    if (DateTime.Parse(nwstr) < dte2)
                    {
                        resArray.Add(nwstr);
                    }
                    else
                    {
                        nwstr = dte2.ToString("dd-MMM-yyyy 23:59:59");
                        resArray.Add(nwstr);
                    }
                }
                while (DateTime.Parse(nwstr) < dte2);
            }
            else if (periodTyp == "Half Yearly")
            {
                do
                {
                    evenOdd = !evenOdd;
                    if (evenOdd)
                    {
                        nwstr = DateTime.Parse(dte1.AddMonths(6).AddDays(-1).ToString("dd-MMM-yyyy 23:59:59")).ToString("dd-MMM-yyyy 23:59:59");
                        dte1 = DateTime.Parse(DateTime.Parse(nwstr).AddDays(1).ToString("dd-MMM-yyyy 00:00:00"));
                    }
                    else
                    {
                        nwstr = dte1.ToString("dd-MMM-yyyy 00:00:00");
                    }
                    if (DateTime.Parse(nwstr) < dte2)
                    {
                        resArray.Add(nwstr);
                    }
                    else
                    {
                        nwstr = dte2.ToString("dd-MMM-yyyy 23:59:59");
                        resArray.Add(nwstr);
                    }
                }
                while (DateTime.Parse(nwstr) < dte2);
            }
            else if (periodTyp == "Quarterly")
            {
                do
                {
                    evenOdd = !evenOdd;
                    if (evenOdd)
                    {
                        nwstr = DateTime.Parse(dte1.AddMonths(3).AddDays(-1).ToString("dd-MMM-yyyy 23:59:59")).ToString("dd-MMM-yyyy 23:59:59");
                        dte1 = DateTime.Parse(DateTime.Parse(nwstr).AddDays(1).ToString("dd-MMM-yyyy 00:00:00"));
                    }
                    else
                    {
                        nwstr = dte1.ToString("dd-MMM-yyyy 00:00:00");
                    }
                    if (DateTime.Parse(nwstr) < dte2)
                    {
                        resArray.Add(nwstr);
                    }
                    else
                    {
                        nwstr = dte2.ToString("dd-MMM-yyyy 23:59:59");
                        resArray.Add(nwstr);
                    }
                }
                while (DateTime.Parse(nwstr) < dte2);
            }
            else if (periodTyp == "Monthly")
            {
                do
                {
                    evenOdd = !evenOdd;
                    if (evenOdd)
                    {
                        nwstr = DateTime.Parse(dte1.AddMonths(1).AddDays(-1).ToString("dd-MMM-yyyy 23:59:59")).ToString("dd-MMM-yyyy 23:59:59");
                        dte1 = DateTime.Parse(DateTime.Parse(nwstr).AddDays(1).ToString("dd-MMM-yyyy 00:00:00"));
                    }
                    else
                    {
                        nwstr = dte1.ToString("dd-MMM-yyyy 00:00:00");
                    }
                    if (DateTime.Parse(nwstr) < dte2)
                    {
                        resArray.Add(nwstr);
                    }
                    else
                    {
                        nwstr = dte2.ToString("dd-MMM-yyyy 23:59:59");
                        resArray.Add(nwstr);
                    }
                }
                while (DateTime.Parse(nwstr) < dte2);
            }
            else if (periodTyp == "Fortnightly")
            {
                do
                {
                    evenOdd = !evenOdd;
                    if (evenOdd)
                    {
                        nwstr = DateTime.Parse(dte1.AddDays(14).AddDays(-1).ToString("dd-MMM-yyyy 23:59:59")).ToString("dd-MMM-yyyy 23:59:59");
                        dte1 = DateTime.Parse(DateTime.Parse(nwstr).AddDays(1).ToString("dd-MMM-yyyy 00:00:00"));
                    }
                    else
                    {
                        nwstr = dte1.ToString("dd-MMM-yyyy 00:00:00");
                    }
                    if (DateTime.Parse(nwstr) < dte2)
                    {
                        resArray.Add(nwstr);
                    }
                    else
                    {
                        nwstr = dte2.ToString("dd-MMM-yyyy 23:59:59");
                        resArray.Add(nwstr);
                    }
                }
                while (DateTime.Parse(nwstr) < dte2);
            }
            else if (periodTyp == "Weekly")
            {
                do
                {
                    evenOdd = !evenOdd;
                    if (evenOdd)
                    {
                        nwstr = DateTime.Parse(dte1.AddDays(7).AddDays(-1).ToString("dd-MMM-yyyy 23:59:59")).ToString("dd-MMM-yyyy 23:59:59");
                        dte1 = DateTime.Parse(DateTime.Parse(nwstr).AddDays(1).ToString("dd-MMM-yyyy 00:00:00"));
                    }
                    else
                    {
                        nwstr = dte1.ToString("dd-MMM-yyyy 00:00:00");
                    }
                    if (DateTime.Parse(nwstr) < dte2)
                    {
                        resArray.Add(nwstr);
                    }
                    else
                    {
                        nwstr = dte2.ToString("dd-MMM-yyyy 23:59:59");
                        resArray.Add(nwstr);
                    }
                }
                while (DateTime.Parse(nwstr) < dte2);
            }
            return resArray;
        }
        #endregion

        #region "Balance Sheet..."
        public static DataSet get_BalSheet_Accnts(int orgid)
        {
            string strSql = "";
            strSql = "SELECT a.accnt_id, a.accnt_num, a.accnt_name, a.net_balance, a.is_prnt_accnt " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE ((a.org_id = " + orgid + ") and " +
          "(a.accnt_type = 'A' or a.accnt_type = 'EQ' or a.accnt_type = 'L')) ORDER BY a.accnt_typ_id, a.accnt_num";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static double get_AccntType_BlsSum(int orgid, string acctype)
        {
            string strSql = "";
            strSql = "SELECT SUM(a.net_balance) " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE ((a.accnt_type = '" + acctype + "') and " +
              "(a.org_id = " + orgid + "))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static double getCashFlowAccBlsSum(string acctClsfctn, string balsDate, int orgid)
        {
            balsDate = DateTime.ParseExact(
        balsDate, "dd-MMM-yyyy HH:mm:ss",
        System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);
            string strSql = "";
            strSql = "SELECT SUM(accb.get_ltst_accnt_bals(a.accnt_id,'" + balsDate + "')) " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE ((a.account_clsfctn ilike '%" + acctClsfctn.Replace("'", "''") + "%') and " +
              "(a.org_id = " + orgid + ") and (a.is_prnt_accnt='0' and a.has_sub_ledgers='0'))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static double getCashFlowAccBlsSum2(string acctClsfctn, string balsDate, int orgid,
            int rptSgmt1, int rptSgmt2, int rptSgmt3,
            int rptSgmt4, int rptSgmt5, int rptSgmt6,
            int rptSgmt7, int rptSgmt8, int rptSgmt9, int rptSgmt10)
        {
            //String extrWhr = " WHERE depth<=" + acntLvl;
            string ttlSgmnts = rptSgmt1.ToString() + rptSgmt2.ToString() + rptSgmt3.ToString() + rptSgmt4.ToString() + rptSgmt5.ToString() + rptSgmt6.ToString() + rptSgmt7.ToString() + rptSgmt8.ToString() + rptSgmt9.ToString() + rptSgmt10.ToString();
            string sgmntWhere = "";
            balsDate = DateTime.ParseExact(
        balsDate, "dd-MMM-yyyy HH:mm:ss",
        System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);
            string strSql = "";
            strSql = "SELECT SUM(accb.get_ltst_accnt_bals(a.accnt_id,'" + balsDate + "')) " +
              "FROM accb.accb_chart_of_accnts a " +
              "WHERE ((a.account_clsfctn ilike '%" + acctClsfctn.Replace("'", "''") + "%') and " +
              "(a.org_id = " + orgid + ") and (a.is_prnt_accnt='0' and a.has_sub_ledgers='0') AND ('" + ttlSgmnts + "' = '-1-1-1-1-1-1-1-1-1-1' " +
               "or (a.accnt_seg1_val_id = " + rptSgmt1 + " and " + rptSgmt1 + " > 0) " +
               "or (a.accnt_seg2_val_id = " + rptSgmt2 + " and " + rptSgmt2 + " > 0) " +
               "or (a.accnt_seg3_val_id = " + rptSgmt3 + " and " + rptSgmt3 + " > 0) " +
               "or (a.accnt_seg4_val_id = " + rptSgmt4 + " and " + rptSgmt4 + " > 0) " +
               "or (a.accnt_seg5_val_id = " + rptSgmt5 + " and " + rptSgmt5 + " > 0) " +
               "or (a.accnt_seg6_val_id = " + rptSgmt6 + " and " + rptSgmt6 + " > 0) " +
               "or (a.accnt_seg7_val_id = " + rptSgmt7 + " and " + rptSgmt7 + " > 0) " +
               "or (a.accnt_seg8_val_id = " + rptSgmt8 + " and " + rptSgmt8 + " > 0) " +
               "or (a.accnt_seg9_val_id = " + rptSgmt9 + " and " + rptSgmt9 + " > 0) " +
               "or (a.accnt_seg10_val_id = " + rptSgmt10 + " and " + rptSgmt10 + " > 0)))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double sumRes = 0.00;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        public static DataSet get_Bls_Det(int orgID, string balsDate, int acntLvl,
            int rptSgmt1, int rptSgmt2, int rptSgmt3,
            int rptSgmt4, int rptSgmt5, int rptSgmt6,
            int rptSgmt7, int rptSgmt8, int rptSgmt9, int rptSgmt10)
        {
            String extrWhr = " WHERE depth<=" + acntLvl;
            string ttlSgmnts = rptSgmt1.ToString() + rptSgmt2.ToString() + rptSgmt3.ToString() + rptSgmt4.ToString() + rptSgmt5.ToString() + rptSgmt6.ToString() + rptSgmt7.ToString() + rptSgmt8.ToString() + rptSgmt9.ToString() + rptSgmt10.ToString();
            string sgmntWhere = "";
            balsDate = DateTime.ParseExact(balsDate, "dd-MMM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";
            strSql = @"WITH RECURSIVE suborg(daily_bals_id, accnt_id, accnt_num, accnt_name, net_balance, as_at_date, is_prnt_accnt, accnt_type, accnt_typ_id, depth, path, cycle, space) AS 
      (SELECT b.daily_bals_id, a.accnt_id, a.accnt_num, a.accnt_name, (SELECT e.net_balance " +
         "FROM accb.accb_accnt_daily_bals e " +
         "WHERE(to_timestamp(e.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
         "','YYYY-MM-DD') and a.accnt_id = e.accnt_id)  ORDER BY to_timestamp(e.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), " +
         @"to_timestamp(b.as_at_date,'YYYY-MM-DD'), a.is_prnt_accnt, a.accnt_type,a.accnt_typ_id, 1, ARRAY[a.accnt_num||'']::character varying[], false, '' opad 
      FROM accb.accb_chart_of_accnts a LEFT OUTER JOIN  accb.accb_accnt_daily_bals b ON (a.accnt_id = b.accnt_id) " +
             "WHERE ((CASE WHEN a.prnt_accnt_id<=0 THEN a.control_account_id ELSE a.prnt_accnt_id END)=-1 AND (a.org_id = " + orgID + ") and " +
             "(a.control_account_id <= 0) and (a.accnt_type != 'R') and " +
          "(a.accnt_type != 'EX') " +
             "and (a.is_prnt_accnt='1' or (to_timestamp(b.as_at_date,'YYYY-MM-DD')=(SELECT " +
             "MAX(to_timestamp(f.as_at_date,'YYYY-MM-DD')) from " +
             "accb.accb_accnt_daily_bals f where f.accnt_id = a.accnt_id " +
             "and to_timestamp(f.as_at_date,'YYYY-MM-DD')<=to_timestamp('" + balsDate +
        @"','YYYY-MM-DD'))))) 
      UNION ALL        
      SELECT (SELECT MAX(d.daily_bals_id) FROM accb.accb_accnt_daily_bals d WHERE(to_timestamp(d.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
         "','YYYY-MM-DD') and a.accnt_id = d.accnt_id)), a.accnt_id, a.accnt_num, a.accnt_name, (SELECT e.net_balance " +
         "FROM accb.accb_accnt_daily_bals e " +
         "WHERE(to_timestamp(e.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
         "','YYYY-MM-DD') and a.accnt_id = e.accnt_id)  ORDER BY to_timestamp(e.as_at_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0), " +
         @"to_timestamp((SELECT MAX(d.as_at_date) " +
          "FROM accb.accb_accnt_daily_bals d " +
          "WHERE(to_timestamp(d.as_at_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
         @"','YYYY-MM-DD') and a.accnt_id = d.accnt_id)),'YYYY-MM-DD'), a.is_prnt_accnt, a.accnt_type, a.accnt_typ_id, sd.depth + 1, 
      path || a.accnt_num, 
      a.accnt_num = ANY(path), space || '           ' 
      FROM 
      accb.accb_chart_of_accnts a
 , suborg AS sd 
      WHERE ((CASE WHEN a.prnt_accnt_id<=0 THEN a.control_account_id ELSE a.prnt_accnt_id END)=sd.accnt_id AND NOT cycle) 
       AND ((a.org_id = " + orgID + ") and " +
             "(a.control_account_id <= 0) and (a.accnt_type != 'R') and " +
          "(a.accnt_type != 'EX') AND ('" + ttlSgmnts + "' = '-1-1-1-1-1-1-1-1-1-1' " +
               "or a.is_prnt_accnt = '1' or a.has_sub_ledgers = '1' " +
               "or (a.accnt_seg1_val_id = " + rptSgmt1 + " and " + rptSgmt1 + " > 0) " +
               "or (a.accnt_seg2_val_id = " + rptSgmt2 + " and " + rptSgmt2 + " > 0) " +
               "or (a.accnt_seg3_val_id = " + rptSgmt3 + " and " + rptSgmt3 + " > 0) " +
               "or (a.accnt_seg4_val_id = " + rptSgmt4 + " and " + rptSgmt4 + " > 0) " +
               "or (a.accnt_seg5_val_id = " + rptSgmt5 + " and " + rptSgmt5 + " > 0) " +
               "or (a.accnt_seg6_val_id = " + rptSgmt6 + " and " + rptSgmt6 + " > 0) " +
               "or (a.accnt_seg7_val_id = " + rptSgmt7 + " and " + rptSgmt7 + " > 0) " +
               "or (a.accnt_seg8_val_id = " + rptSgmt8 + " and " + rptSgmt8 + " > 0) " +
               "or (a.accnt_seg9_val_id = " + rptSgmt9 + " and " + rptSgmt9 + " > 0) " +
               "or (a.accnt_seg10_val_id = " + rptSgmt10 + " and " + rptSgmt10 + " > 0))" +
             @")) 
      SELECT daily_bals_id, accnt_id, space||accnt_num, accnt_name, net_balance, is_prnt_accnt, accnt_type, accnt_typ_id, depth, path, cycle 
      FROM suborg " + extrWhr + @"
      ORDER BY accnt_typ_id, path";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.blshtSQLStmnt = strSql;
            return dtst;
            /*and (a.is_prnt_accnt='1' or (to_timestamp(b.as_at_date,'YYYY-MM-DD')=(SELECT " +
             "MAX(to_timestamp(f.as_at_date,'YYYY-MM-DD')) from " +
             "accb.accb_accnt_daily_bals f where f.accnt_id = a.accnt_id " +
             "and to_timestamp(f.as_at_date,'YYYY-MM-DD')<=to_timestamp('" + balsDate +
        @"','YYYY-MM-DD'))))*/
        }

        public static bool isThereProcess1()
        {
            string strSql = "";
            strSql = "SELECT a.process_id " +
            "FROM accb.accb_running_prcses a " +
            "WHERE (a.process_id=1)";

            //Global.mnFrm.trns_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region "ORG BUDGET..."
        public static long doesBdgtDteOvrlap(long bdgtid, int accntid, string bdgtDte)
        {
            string strSql = "";
            strSql = @"SELECT a.budget_det_id
    FROM accb.accb_budget_details a 
    WHERE(a.budget_id = " + bdgtid.ToString() + " and a.accnt_id = " + accntid +
          " and to_timestamp('" + bdgtDte + "','DD-Mon-YYYY HH24:MI:SS') >= to_timestamp(a.start_date,'YYYY-MM-DD HH24:MI:SS')" +
          " and to_timestamp('" + bdgtDte + "','DD-Mon-YYYY HH24:MI:SS') <= to_timestamp(a.end_date,'YYYY-MM-DD HH24:MI:SS')) ";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }
        public static long getBdgtDteID(long bdgtid, int accntid, string strtbdgtDte, string endBdgtDte)
        {
            string strSql = "";
            strSql = @"SELECT a.budget_det_id
    FROM accb.accb_budget_details a 
    WHERE(a.budget_id = " + bdgtid.ToString() + " and a.accnt_id = " + accntid +
          " and to_timestamp('" + strtbdgtDte + "','DD-Mon-YYYY HH24:MI:SS') = to_timestamp(a.start_date,'YYYY-MM-DD HH24:MI:SS')" +
          " and to_timestamp('" + endBdgtDte + "','DD-Mon-YYYY HH24:MI:SS') = to_timestamp(a.end_date,'YYYY-MM-DD HH24:MI:SS')) ";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static DataSet get_One_BdgtDt(string searchWord, string searchIn, long offset, int limit_size, long bdgtID)
        {
            /*Account Number
         Account Name
         Period Start Date
         Period End Date*/
            string whrcls = "";
            if (searchIn == "Account Number")
            {
                whrcls = " and (b.accnt_num ilike '" + searchWord.Replace("'", "''") + "' or b.accnt_name ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Account Name")
            {
                whrcls = " and (b.accnt_num ilike '" + searchWord.Replace("'", "''") + "' or b.accnt_name ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Period Start Date")
            {
                whrcls = " and (to_char(to_timestamp(a.start_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Period End Date")
            {
                whrcls = " and (to_char(to_timestamp(a.end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") + "')";
            }
            string strSql = "";
            strSql = @"SELECT a.budget_det_id, b.accnt_num, b.accnt_name, " +
              "COALESCE(a.limit_amount,0), COALESCE(accb.get_prd_usr_trns_sum(a.accnt_id, a.start_date, a.end_date),0) usd_amnt, " +
                    @"to_char(to_timestamp(a.start_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
      to_char(to_timestamp(a.end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), " +
                    "a.action_if_limit_excded, a.accnt_id " +
          "FROM accb.accb_budget_details a LEFT OUTER JOIN " +
          "accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
          "WHERE(a.budget_id = " + bdgtID.ToString() + whrcls + ") ORDER BY b.accnt_typ_id, b.accnt_num, " +
          "to_timestamp(a.start_date,'YYYY-MM-DD HH24:MI:SS') LIMIT " + limit_size +
          " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.bdgtDet_SQL = strSql;
            return dtst;
        }

        public static long get_Total_BdgtDt(string searchWord, string searchIn, long bdgtID)
        {
            string whrcls = "";
            if (searchIn == "Account Number")
            {
                whrcls = " and (b.accnt_num ilike '" + searchWord.Replace("'", "''") + "' or b.accnt_name ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Account Name")
            {
                whrcls = " and (b.accnt_num ilike '" + searchWord.Replace("'", "''") + "' or b.accnt_name ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Period Start Date")
            {
                whrcls = " and (to_char(to_timestamp(a.start_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Period End Date")
            {
                whrcls = " and (to_char(to_timestamp(a.end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") + "')";
            }
            string strSql = "";
            strSql = "SELECT count(1) " +
         "FROM accb.accb_budget_details a LEFT OUTER JOIN " +
         "accb.accb_chart_of_accnts b on a.accnt_id = b.accnt_id " +
         "WHERE(a.budget_id = " + bdgtID.ToString() + whrcls + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static DataSet get_Basic_Bdgt(string searchWord, string searchIn,
      Int64 offset, int limit_size, int orgID)
        {
            string strSql = "";
            if (searchIn == "Budget Name")
            {
                strSql = "SELECT a.budget_id, a.budget_name, a.budget_desc, " +
                      "a.is_the_active_one, " +
                      "CASE WHEN a.start_date='' THEN '' ELSE to_char(to_timestamp(a.start_date, 'YYYY-MM-DD HH24:MI:SS'), 'DD-Mon-YYYY HH24:MI:SS') END, " +
                      "CASE WHEN a.end_date='' THEN '' ELSE to_char(to_timestamp(a.end_date, 'YYYY-MM-DD HH24:MI:SS'), 'DD-Mon-YYYY HH24:MI:SS') END, a.period_type " +
            "FROM accb.accb_budget_header a " +
                "WHERE ((a.budget_name ilike '" + searchWord.Replace("'", "''") +
                  "') AND (a.org_id = " + orgID + ")) ORDER BY a.budget_id DESC LIMIT " + limit_size +
                  " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }
            else if (searchIn == "Budget Description")
            {
                strSql = "SELECT a.budget_id, a.budget_name, a.budget_desc, " +
                      "a.is_the_active_one, " +
                      "CASE WHEN a.start_date='' THEN '' ELSE to_char(to_timestamp(a.start_date, 'YYYY-MM-DD HH24:MI:SS'), 'DD-Mon-YYYY HH24:MI:SS') END, " +
                      "CASE WHEN a.end_date='' THEN '' ELSE to_char(to_timestamp(a.end_date, 'YYYY-MM-DD HH24:MI:SS'), 'DD-Mon-YYYY HH24:MI:SS') END, a.period_type " +
            "FROM accb.accb_budget_header a " +
                "WHERE ((a.budget_desc ilike '" + searchWord.Replace("'", "''") +
                  "') AND (a.org_id = " + orgID + ")) ORDER BY a.budget_id DESC LIMIT " + limit_size +
                  " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.bdgt_SQL = strSql;
            return dtst;
        }

        public static long get_Total_Bdgt(string searchWord, string searchIn, int orgID)
        {
            string strSql = "";
            if (searchIn == "Budget Name")
            {
                strSql = "SELECT count(1) " +
            "FROM accb.accb_budget_header a " +
                "WHERE ((a.budget_name ilike '" + searchWord.Replace("'", "''") +
                  "') AND (a.org_id = " + orgID + "))";
            }
            else if (searchIn == "Budget Description")
            {
                strSql = "SELECT count(1) " +
            "FROM accb.accb_budget_header a " +
                "WHERE ((a.budget_desc ilike '" + searchWord.Replace("'", "''") +
                  "') AND (a.org_id = " + orgID + "))";
            }

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static string get_Bdgt_Rec_Hstry(long bdgtID)
        {
            string strSQL = "SELECT a.created_by, to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), a.last_update_by, to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM accb.accb_budget_header a WHERE(a.budget_id = " + bdgtID + ")";
            string fnl_str = "";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSQL);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                fnl_str = "CREATED BY: " + Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][0].ToString())) +
                  "\r\nCREATION DATE: " + dtst.Tables[0].Rows[0][1].ToString() + "\r\nLAST UPDATE BY: " +
                  Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][2].ToString())) +
                  "\r\nLAST UPDATE DATE: " + dtst.Tables[0].Rows[0][3].ToString();
                return fnl_str;
            }
            else
            {
                return "";
            }
        }

        public static string get_BdgtDt_Rec_Hstry(long bdgtDtID)
        {
            string strSQL = @"SELECT a.created_by, 
      to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
a.last_update_by, 
to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM accb.accb_budget_details a WHERE(a.budget_det_id = " + bdgtDtID + ")";
            string fnl_str = "";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSQL);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                fnl_str = "CREATED BY: " + Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][0].ToString())) +
                  "\r\nCREATION DATE: " + dtst.Tables[0].Rows[0][1].ToString() + "\r\nLAST UPDATE BY: " +
                  Global.mnFrm.cmCde.get_user_name(long.Parse(dtst.Tables[0].Rows[0][2].ToString())) +
                  "\r\nLAST UPDATE DATE: " + dtst.Tables[0].Rows[0][3].ToString();
                return fnl_str;
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region "ACCOUNTING PERIODS..."
        public static void createPeriodsHdr(int orgid, string hdrname,
      string hdrdesc, string prdtyp, bool usePerds, string noTrnsDysLOV
          , string noTrnsDatesLOV)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_periods_hdr(
            period_hdr_name, period_hdr_desc, period_type, 
            created_by, creation_date, last_update_by, last_update_date, 
            use_periods_for_org, no_trns_wk_days_lov_nm, no_trns_dates_lov_nm, 
            org_id) " +
                  "VALUES ('" + hdrname.Replace("'", "''") +
                  "', '" + hdrdesc.Replace("'", "''") +
                  "', '" + prdtyp.Replace("'", "''") +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(usePerds) +
                  "', '" + noTrnsDysLOV.Replace("'", "''") +
                  "', '" + noTrnsDatesLOV.Replace("'", "''") +
                  "', " + orgid + ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updatePeriodsHdr(long hdrid, string hdrname,
      string hdrdesc, string prdtyp, bool usePerds, string noTrnsDysLOV
          , string noTrnsDatesLOV)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_periods_hdr SET " +
                  "period_hdr_name='" + hdrname.Replace("'", "''") +
                  "', period_hdr_desc='" + hdrdesc.Replace("'", "''") +
                  "', period_type='" + prdtyp.Replace("'", "''") +
                  "', use_periods_for_org='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(usePerds) +
                  "', last_update_by=" + Global.myBscActn.user_id + ", " +
                  "last_update_date='" + dateStr +
                  "', no_trns_wk_days_lov_nm='" + noTrnsDysLOV.Replace("'", "''") +
                  "', no_trns_dates_lov_nm='" + noTrnsDatesLOV.Replace("'", "''") + "' " +
                  "WHERE (periods_hdr_id =" + hdrid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void createPeriodsDetLn(long hdrid,
      string start_date, string end_date, string prdStatus, string prdNm)
        {
            if (start_date != "")
            {
                start_date = DateTime.ParseExact(
            start_date, "dd-MMM-yyyy HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (end_date != "")
            {
                end_date = DateTime.ParseExact(
            end_date, "dd-MMM-yyyy HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            }
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_periods_det(
            period_hdr_id, period_start_date, period_end_date, 
            created_by, creation_date, last_update_by, last_update_date, 
            period_det_name, period_status) " +
                  "VALUES (" + hdrid + ", '" + start_date + "', '" + end_date.Replace("'", "''") +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', '" + prdNm.Replace("'", "''") +
                  "', '" + prdStatus.Replace("'", "''") +
                  "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtPeriodsDetLn(long prdDetLnid,
      string start_date, string end_date, string prdStatus, string prdNm)
        {
            if (start_date != "")
            {
                start_date = DateTime.ParseExact(
            start_date, "dd-MMM-yyyy HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (end_date != "")
            {
                end_date = DateTime.ParseExact(
            end_date, "dd-MMM-yyyy HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            }
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_periods_det SET 
             period_start_date='" + start_date.Replace("'", "''") +
                  "', period_end_date='" + end_date.Replace("'", "''") +
                  "', last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', period_det_name='" + prdNm.Replace("'", "''") +
                  "', period_status='" + prdStatus.Replace("'", "''") +
                  "' " +
                  "WHERE period_det_id=" + prdDetLnid + " ";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtPeriodsDetLnStatus(long prdDetLnid, string prdStatus)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_periods_det SET 
             last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', period_status='" + prdStatus.Replace("'", "''") +
                  "' " +
                  "WHERE period_det_id=" + prdDetLnid + " ";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static long getRptRnID(long rptID, long runBy, string runDate)
        {
            runDate = DateTime.ParseExact(
         runDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            DataSet dtSt = new DataSet();
            string sqlStr = "select rpt_run_id from rpt.rpt_report_runs where run_by = " +
              runBy + " and report_id = " + rptID + " and run_date = '" +
             runDate + "' order by rpt_run_id DESC";
            dtSt = Global.mnFrm.cmCde.selectDataNoParams(sqlStr);
            if (dtSt.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtSt.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        public static DataSet get_AllParams(long rptID)
        {
            string strSql = "SELECT parameter_id, parameter_name, paramtr_rprstn_nm_in_query, default_value, " +
         "is_required, lov_name_id FROM rpt.rpt_report_parameters WHERE report_id = " + rptID + " ORDER BY parameter_name";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static void createRptRn(long runBy, string runDate,
      long rptID, string paramIDs, string paramVals,
      string outptUsd, string orntUsd)
        {
            TimeSpan tm = new TimeSpan(0, 5, 0);
            runDate = (DateTime.ParseExact(
         runDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture) - tm).ToString("yyyy-MM-dd HH:mm:ss");
            string insSQL = @"INSERT INTO rpt.rpt_report_runs(
            run_by, run_date, rpt_run_output, run_status_txt, 
            run_status_prct, report_id, rpt_rn_param_ids, rpt_rn_param_vals, 
            output_used, orntn_used, last_actv_date_tme, is_this_from_schdler) " +
                  "VALUES (" + runBy + ", '" + runDate +
                  "', '', 'Not Started!', 0, " + rptID + ", '" + paramIDs.Replace("'", "''") +
                  "', '" + paramVals.Replace("'", "''") +
                  "', '" + outptUsd.Replace("'", "''") +
                  "', '" + orntUsd.Replace("'", "''") +
                  "', '" + runDate + "', '0')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void deletePeriodsDLn(long prdLnid, string PrdNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Period Name = " + PrdNm;
            string delSQL = "DELETE FROM accb.accb_periods_det WHERE period_det_id = " + prdLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static bool isPeriodsLnInUse(long prdLnid)
        {
            //1. Only periods with status never opened can be deleted
            //2. 
            string prdStatus = Global.mnFrm.cmCde.getGnrlRecNm("accb.accb_periods_det", "period_det_id", "period_status", prdLnid);
            if (prdStatus != "Never Opened")
            {
                return true;
            }
            return false;
        }

        public static DataSet get_One_CaldrDet(long OrgID)
        {
            string strSql = @"SELECT periods_hdr_id, period_hdr_name, period_hdr_desc, period_type, 
       use_periods_for_org, no_trns_wk_days_lov_nm, no_trns_dates_lov_nm, 
       org_id
  FROM accb.accb_periods_hdr a " +
             "WHERE(a.org_id = " + OrgID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.actnPrdFrm.rec_SQL = strSql;
            return dtst;
        }

        public static bool isNwPrdDatesInUse(string strDte, string endDte)
        {
            string strSql = @"SELECT period_det_id FROM accb.accb_periods_det a, accb.accb_periods_hdr b " +
              "WHERE((a.period_hdr_id=b.periods_hdr_id) and (b.org_id=" + Global.mnFrm.cmCde.Org_id + ") and (to_timestamp('" + strDte.Replace("'", "''") +
              @"','DD-Mon-YYYY HH24:MI:SS') between to_timestamp(a.period_start_date,'YYYY-MM-DD HH24:MI:SS')
       and to_timestamp(a.period_end_date,'YYYY-MM-DD HH24:MI:SS')
       or to_timestamp('" + endDte.Replace("'", "''") +
              @"','DD-Mon-YYYY HH24:MI:SS') between to_timestamp(a.period_start_date,'YYYY-MM-DD HH24:MI:SS')
       and to_timestamp(a.period_end_date,'YYYY-MM-DD HH24:MI:SS')))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static bool isNwPrdDatesInUse(string strDte, string endDte, long prdLnID)
        {
            string strSql = @"SELECT period_det_id FROM accb.accb_periods_det a, accb.accb_periods_hdr b  " +
             "WHERE((a.period_hdr_id=b.periods_hdr_id) and (b.org_id=" + Global.mnFrm.cmCde.Org_id + ") and (to_timestamp('" + strDte.Replace("'", "''") +
             @"','DD-Mon-YYYY HH24:MI:SS') between to_timestamp(a.period_start_date,'YYYY-MM-DD HH24:MI:SS')
       and to_timestamp(a.period_end_date,'YYYY-MM-DD HH24:MI:SS')
       or to_timestamp('" + endDte.Replace("'", "''") +
             @"','DD-Mon-YYYY HH24:MI:SS') between to_timestamp(a.period_start_date,'YYYY-MM-DD HH24:MI:SS')
       and to_timestamp(a.period_end_date,'YYYY-MM-DD HH24:MI:SS')) and (a.period_det_id != " + prdLnID + "))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static bool doesNwPrdDatesMeetPrdTyp(string strDte, string endDte, string prdIntrvlTyp)
        {
            string strSql = @"SELECT age(to_timestamp('" + endDte.Replace("'", "''") +
             @"','DD-Mon-YYYY HH24:MI:SS') + interval '10 second', to_timestamp('" + strDte.Replace("'", "''") +
             @"','DD-Mon-YYYY HH24:MI:SS')) = interval '" + prdIntrvlTyp + "'";

            //Global.mnFrm.cmCde.showMsg(strSql, 0);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return bool.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return false;
        }

        public static bool isThereANActvActnPrcss(string prcsIDs, string prcsIntrvl)
        {
            string strSql = "SELECT age(now(), to_timestamp(last_active_time,'YYYY-MM-DD HH24:MI:SS')) <= interval '" + prcsIntrvl +
              "' FROM accb.accb_running_prcses WHERE which_process_is_rnng IN (" + prcsIDs +
              ") and age(now(), to_timestamp(last_active_time,'YYYY-MM-DD HH24:MI:SS')) <= interval '" + prcsIntrvl +
              "'";

            //Global.mnFrm.cmCde.showMsg(strSql, 0);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return bool.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return false;
        }

        public static int getActnPrcssID(string rnngprcsID)
        {
            string strSql = @"SELECT process_id FROM accb.accb_running_prcses WHERE which_process_is_rnng = " + rnngprcsID + "";

            //Global.mnFrm.cmCde.showMsg(strSql, 0);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static void createActnPrcss(int prcsID, string process_type)
        {
            string dtestr = Global.mnFrm.cmCde.getDB_Date_time();
            string strSql = @"INSERT INTO accb.accb_running_prcses(
            which_process_is_rnng, last_active_time, process_type)
    VALUES (" + prcsID + ", '" + dtestr + "','" + process_type.Replace("'", "''") + "')";
            Global.mnFrm.cmCde.insertDataNoParams(strSql);
        }

        public static void updtActnPrcss(int prcsID, string process_type)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            Global.mnFrm.cmCde.ignorAdtTrail = true;
            string dtestr = Global.mnFrm.cmCde.getDB_Date_time();
            string strSql = @"UPDATE accb.accb_running_prcses SET
            last_active_time='" + dtestr + "', process_type='" + process_type.Replace("'", "''") + "' " +
                  "WHERE which_process_is_rnng = " + prcsID + " ";
            Global.mnFrm.cmCde.updateDataNoParams(strSql);
            Global.mnFrm.cmCde.ignorAdtTrail = false;
        }

        /*public static void updtActnPrcss(int prcsID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            Global.mnFrm.cmCde.ignorAdtTrail = true;
            string dtestr = Global.mnFrm.cmCde.getDB_Date_time();
            string strSql = @"UPDATE accb.accb_running_prcses SET
            last_active_time='" + dtestr + "' " +
                  "WHERE which_process_is_rnng = " + prcsID + " ";
            Global.mnFrm.cmCde.updateDataNoParams(strSql);
            Global.mnFrm.cmCde.ignorAdtTrail = false;
        }

        public static void updtActnPrcss(int prcsID, int secondsAhead)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            Global.mnFrm.cmCde.ignorAdtTrail = true;
            string dtestr = Global.mnFrm.cmCde.getDB_Date_time();
            string strSql = @"UPDATE accb.accb_running_prcses SET
            last_active_time=to_char(to_timestamp('" + dtestr + "','YYYY-MM-DD HH24:MI:SS') + interval '" + secondsAhead + " second','YYYY-MM-DD HH24:MI:SS') " +
                  "WHERE which_process_is_rnng = " + prcsID + " ";
            Global.mnFrm.cmCde.updateDataNoParams(strSql);
            Global.mnFrm.cmCde.ignorAdtTrail = false;
        }*/

        public static bool areTherePrvsUnclsdPrds(long hdrID, string curprdStrtDte)
        {
            string strSql = @"SELECT a.period_det_id 
       FROM accb.accb_periods_det a 
       WHERE((a.period_hdr_id = " + hdrID + ") and (to_timestamp('" + curprdStrtDte +
              @"','YYYY-MM-DD HH24:MI:SS') 
        > to_timestamp(a.period_end_date,'YYYY-MM-DD HH24:MI:SS')) and (a.period_status !='Closed'))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static long get_PrdDetID(long hdrID, string prdNm)
        {
            string strSql = @"SELECT period_det_id FROM accb.accb_periods_det a, accb.accb_periods_hdr b " +
             "WHERE(a.period_hdr_id=b.periods_hdr_id and b.org_id=" + Global.mnFrm.cmCde.Org_id + " and a.period_hdr_id = " + hdrID + " and a.period_det_name = '" + prdNm.Replace("'", "''") + "')";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static long get_TotlPeriods(long hdrID)
        {
            string strSql = @"SELECT count(period_det_id) 
  FROM accb.accb_periods_det a " +
             "WHERE(a.period_hdr_id = " + hdrID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static DataSet get_One_Period_DetLns(string searchWord, string searchIn, long offset,
         int limit_size, long hdrID)
        {
            string strSql = "";
            string whrcls = "";
            /*
             *  End Date
         Period Name
         Start Date
         Status
             * 
             */
            if (searchIn == "Period Name")
            {
                whrcls = " AND (period_det_name ilike '" + searchWord.Replace("'", "''") +
               "')";
            }
            else if (searchIn == "Start Date")
            {
                whrcls = " AND (to_char(to_timestamp(a.period_start_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") +
               "')";
            }
            else if (searchIn == "End Date")
            {
                whrcls = " AND (to_char(to_timestamp(a.period_end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") +
               "')";
            }
            else if (searchIn == "Status")
            {
                whrcls = " AND (a.period_status ilike '" + searchWord.Replace("'", "''") +
               "')";
            }

            strSql = @"SELECT period_det_id, period_hdr_id, period_det_name, 
to_char(to_timestamp(a.period_start_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
to_char(to_timestamp(a.period_end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
        period_status
        FROM accb.accb_periods_det a " +
              "WHERE((a.period_hdr_id = " + hdrID + ")" + whrcls +
              ") ORDER BY a.period_start_date DESC LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.actnPrdFrm.rec_det_SQL = strSql;
            return dtst;
        }

        public static long get_Total_Period_DetLns(string searchWord, string searchIn, long hdrID)
        {
            string strSql = "";
            string whrcls = "";
            /*
             *  End Date
         Period Name
         Start Date
         Status
             * 
             */
            if (searchIn == "Period Name")
            {
                whrcls = " AND (period_det_name ilike '" + searchWord.Replace("'", "''") +
               "')";
            }
            else if (searchIn == "Start Date")
            {
                whrcls = " AND (to_char(to_timestamp(a.period_start_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") +
               "')";
            }
            else if (searchIn == "End Date")
            {
                whrcls = " AND (to_char(to_timestamp(a.period_end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") +
               "')";
            }
            else if (searchIn == "Status")
            {
                whrcls = " AND (a.period_status ilike '" + searchWord.Replace("'", "''") +
               "')";
            }

            strSql = @"SELECT count(1) 
        FROM accb.accb_periods_det a " +
              "WHERE((a.period_hdr_id = " + hdrID + ")" + whrcls + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region "TAX CODES..."
        public static void createDfltAcnts(int orgid)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO scm.scm_dflt_accnts(" +
                  "itm_inv_asst_acnt_id, cost_of_goods_acnt_id, expense_acnt_id, " +
                  "prchs_rtrns_acnt_id, rvnu_acnt_id, sales_rtrns_acnt_id, sales_cash_acnt_id, " +
                  "sales_check_acnt_id, sales_rcvbl_acnt_id, rcpt_cash_acnt_id, " +
                  "rcpt_lblty_acnt_id, rho_name, org_id, created_by, creation_date, " +
                  "last_update_by, last_update_date) " +
                  "VALUES (-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,'Default Accounts', " +
                  orgid + ", " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void createTaxRec(int orgid, string codename,
      string codedesc, string itmTyp, bool isEnbld, int taxAcntID
          , int expnsAcntID,
          int rvnuAcntID, string sqlFormular, bool isTxRcvrbl, int txExpAccID,
         int prchDscAccID, int chrgExpAccID, bool isWthHldng, bool isParnt, string codeIDs)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO scm.scm_tax_codes(" +
                  "code_name, code_desc, created_by, creation_date, last_update_by, " +
                  "last_update_date, itm_type, is_enabled, taxes_payables_accnt_id, " +
                  "dscount_expns_accnt_id, " +
                  "chrge_revnu_accnt_id, " +
                  @"org_id, sql_formular, 
            is_recovrbl_tax, tax_expense_accnt_id, prchs_dscnt_accnt_id, 
            chrge_expns_accnt_id, is_withldng_tax, is_parent, child_code_ids) " +
                  "VALUES ('" + codename.Replace("'", "''") +
                  "', '" + codedesc.Replace("'", "''") +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', '" + itmTyp.Replace("'", "''") + "', '" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(isEnbld) + "', " + taxAcntID + ", " +
                  expnsAcntID + ", " + rvnuAcntID +
                  ", " + orgid + ", '" + sqlFormular.Replace("'", "''") +
                  "', '" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(isTxRcvrbl) + "', " + txExpAccID + ", " +
                  prchDscAccID + ", " + chrgExpAccID +
                  ", '" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(isWthHldng) + "', '" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(isParnt) + "', '" +
                 codeIDs + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void createCstSplrRec(int orgid, string cstmrname,
      string cstmrdesc, string cstmrTyp, string clssfctn,
         int pyblAccntID, int rcvblAccntID, long prsnID, string gender, string dob,
          bool isEnbld, string brndNm, string typeOfOrg, string regNum,
          string dteIncp, string typeOfIncp, string vatNum, string tinNum,
          string ssnitNum, int noEmps, string descSrvcs, string lstSrvcs)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            dob = DateTime.ParseExact(
                dob, "dd-MMM-yyyy",
                System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string insSQL = "INSERT INTO scm.scm_cstmr_suplr(" +
                  "cust_sup_name, cust_sup_desc, created_by, creation_date, last_update_by, last_update_date, " +
                  "cust_sup_clssfctn, cust_or_sup, org_id, dflt_pybl_accnt_id, dflt_rcvbl_accnt_id, " +
                  @"lnkd_prsn_id,person_gender,dob_estblshmnt, is_enabled, firm_brand_name, type_of_organisation, 
            company_reg_num, date_of_incorptn, type_of_incorporation, vat_number, 
            tin_number, ssnit_reg_number, no_of_emplyees, description_of_services, 
            list_of_services) " +
                  "VALUES ('" + cstmrname.Replace("'", "''") +
                  "', '" + cstmrdesc.Replace("'", "''") +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', '" + clssfctn.Replace("'", "''") +
                  "', '" + cstmrTyp.Replace("'", "''") + "', " +
                  orgid + ", " +
                  pyblAccntID + ", " +
                  rcvblAccntID + ", " + prsnID + ",'" + gender.Replace("'", "''") + "','" + dob.Replace("'", "''") +
                  "', '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isEnbld) + "','" + brndNm.Replace("'", "''") +
                  "','" + typeOfOrg.Replace("'", "''") +
                  "','" + regNum.Replace("'", "''") +
                  "','" + dteIncp.Replace("'", "''") +
                  "','" + typeOfIncp.Replace("'", "''") +
                  "','" + vatNum.Replace("'", "''") +
                  "','" + tinNum.Replace("'", "''") +
                  "','" + ssnitNum.Replace("'", "''") +
                  "'," + noEmps +
                  ",'" + descSrvcs.Replace("'", "''") +
                  "','" + lstSrvcs.Replace("'", "''") +
                  "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static DataSet getUnlinkedPrsns(string prsnType)
        {
            string selSQL = @"SELECT a.person_id
          FROM prs.prsn_names_nos a
          WHERE a.org_id = " + Global.mnFrm.cmCde.Org_id + @" 
          and pasn.get_prsn_typid(a.person_id) = gst.get_pssbl_val_id('" + prsnType.Replace("'", "''")
                + @"', gst.get_lov_id('Person Types')) 
          and a.person_id NOT IN (SELECT b.lnkd_prsn_id FROM scm.scm_cstmr_suplr b)";
            return Global.mnFrm.cmCde.selectDataNoParams(selSQL);
        }

        public static void createCstSplrSiteRec(long cstmrID, string sitename,
      string sitedesc, string cntctPrsn, string cntctNos, string email,
          string bankNm, string bnkBrnch, string accNum, string blngAddrs,
          string shpngAddrs, int taxCode, int dscntCode, string swift_code,
                 string nationality, string national_id_typ,
         string id_number, string date_issued, string expiry_date,
                 string other_info, bool isEnbld, string iban_number, int accCurID)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO scm.scm_cstmr_suplr_sites(" +
                  "cust_supplier_id, contact_person_name, contact_nos, email, created_by, " +
                  "creation_date, last_update_by, last_update_date, site_name, site_desc, " +
                  "bank_name, bank_branch, bank_accnt_number, wth_tax_code_id, discount_code_id, " +
                  @"billing_address, ship_to_address, swift_code, 
            nationality, national_id_typ, id_number, date_issued, expiry_date, 
            other_info, is_enabled, iban_number, accnt_cur_id) " +
                  "VALUES (" + cstmrID + ", '" + cntctPrsn.Replace("'", "''") +
                  "', '" + cntctNos.Replace("'", "''") +
                  "', '" + email.Replace("'", "''") +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', '" + sitename.Replace("'", "''") +
                  "', '" + sitedesc.Replace("'", "''") + "', '" +
                  bankNm.Replace("'", "''") + "', '" + bnkBrnch.Replace("'", "''") +
                  "', '" + accNum.Replace("'", "''") + "', " + taxCode + ", " + dscntCode +
                  ", '" + blngAddrs.Replace("'", "''") + "', '" + shpngAddrs.Replace("'", "''") +
                  "', '" + swift_code.Replace("'", "''") + "', '" + nationality.Replace("'", "''") +
                  "', '" + national_id_typ.Replace("'", "''") + "', '" + id_number.Replace("'", "''") +
                  "', '" + date_issued.Replace("'", "''") + "', '" + expiry_date.Replace("'", "''") +
                  "', '" + other_info.Replace("'", "''") +
                  "', '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isEnbld) + "', '" +
                  iban_number.Replace("'", "''") + "', " + accCurID + ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updateDfltAcnt(int rowid, string colNm, int colVal)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE scm.scm_dflt_accnts SET " +
                  colNm + "= " + colVal + ", last_update_by=" + Global.myBscActn.user_id + ", " +
                  "last_update_date='" + dateStr +
                  "' WHERE (row_id =" + rowid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updateTaxRec(int codeid, string codename,
      string codedesc, string itmTyp, bool isEnbld, int taxAcntID
          , int expnsAcntID,
          int rvnuAcntID, string sqlFormular, bool isTxRcvrbl, int txExpAccID,
         int prchDscAccID, int chrgExpAccID, bool isWthHldng, bool is_parent, string chldCodeIDs)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE scm.scm_tax_codes SET " +
                  "code_name='" + codename.Replace("'", "''") +
                  "', code_desc='" + codedesc.Replace("'", "''") +
                  "', last_update_by=" + Global.myBscActn.user_id + ", " +
                  "last_update_date='" + dateStr +
                  "', itm_type='" + itmTyp.Replace("'", "''") + "', is_enabled='" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(isEnbld) +
                  "', taxes_payables_accnt_id=" + taxAcntID + ", " +
                  "dscount_expns_accnt_id=" + expnsAcntID + ", " +
                  "chrge_revnu_accnt_id=" + rvnuAcntID +
                  ", sql_formular='" + sqlFormular.Replace("'", "''") + "', is_recovrbl_tax='" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(isTxRcvrbl) +
                  "', tax_expense_accnt_id=" + txExpAccID +
                  ", prchs_dscnt_accnt_id=" + prchDscAccID +
                  ", chrge_expns_accnt_id=" + chrgExpAccID +
                  ", is_withldng_tax='" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(isWthHldng) +
                  "', is_parent='" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(is_parent) +
                  "', child_code_ids='" + chldCodeIDs +
                  "' " +
                  "WHERE (code_id =" + codeid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtCstSplrRec(long spplrid, string cstmrname,
      string cstmrdesc, string cstmrTyp, string clssfctn, int pyblAccntID,
          int rcvblAccntID, long prsnID, string gender, string dob, bool isEnbld,
          string brndNm, string typeOfOrg, string regNum,
          string dteIncp, string typeOfIncp, string vatNum, string tinNum,
          string ssnitNum, int noEmps, string descSrvcs, string lstSrvcs)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            dob = DateTime.ParseExact(
                dob, "dd-MMM-yyyy",
                System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            string updtSQL = "UPDATE scm.scm_cstmr_suplr SET " +
                  "cust_sup_name = '" + cstmrname.Replace("'", "''") +
                  "', cust_sup_desc = '" + cstmrdesc.Replace("'", "''") +
                  "', last_update_by = " + Global.myBscActn.user_id +
                  ", last_update_date = '" + dateStr +
                  "', cust_sup_clssfctn='" + clssfctn.Replace("'", "''") +
                  "', cust_or_sup='" + cstmrTyp.Replace("'", "''") +
                  "', dflt_pybl_accnt_id=" + pyblAccntID +
                  ", dflt_rcvbl_accnt_id=" + rcvblAccntID +
                  ", lnkd_prsn_id=" + prsnID +
                  ", person_gender='" + gender.Replace("'", "''") +
                  "', dob_estblshmnt='" + dob.Replace("'", "''") +
                  "', is_enabled='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isEnbld) +
                  "', firm_brand_name = '" + brndNm.Replace("'", "''") +
                  "', type_of_organisation = '" + typeOfOrg.Replace("'", "''") +
                  "', company_reg_num ='" + regNum.Replace("'", "''") +
                  "', date_of_incorptn = '" + dteIncp.Replace("'", "''") +
                  "', type_of_incorporation = '" + typeOfIncp.Replace("'", "''") +
                  "', vat_number = '" + vatNum.Replace("'", "''") +
                  "', tin_number = '" + tinNum.Replace("'", "''") +
                  "', ssnit_reg_number = '" + ssnitNum.Replace("'", "''") +
                  "', no_of_emplyees = " + noEmps +
                  ", description_of_services = '" + descSrvcs.Replace("'", "''") +
                  "', list_of_services = '" + lstSrvcs.Replace("'", "''") +
                  "' WHERE (cust_sup_id = " + spplrid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }
        public static void updtCstmrImg(int cstmrID)
        {
            if (Global.mnFrm.cmCde.myComputer.FileSystem.FileExists(Global.mnFrm.cmCde.getCstmrImgsDrctry() + @"\" + cstmrID.ToString() + ".png"))
            {
                Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
                string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
                string sqlStr = "UPDATE scm.scm_cstmr_suplr SET " +
                "cstmr_image = '" + cstmrID.ToString() + ".png', " +
                "last_update_by = " + Global.myBscActn.user_id +
                ", last_update_date = '" + dateStr + "' " +
                "WHERE(cust_sup_id = " + cstmrID + ")";
                Global.mnFrm.cmCde.updateDataNoParams(sqlStr);
            }
        }
        public static void updtCstSplrRecExcl(long spplrid, string cstmrname,
      string cstmrdesc, string cstmrTyp, string clssfctn, int pyblAccntID,
          int rcvblAccntID, string dob, bool isEnbld,
          string brndNm, string typeOfOrg, string regNum,
          string dteIncp, string typeOfIncp, string vatNum, string tinNum,
          string ssnitNum, int noEmps, string descSrvcs, string lstSrvcs)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            dob = DateTime.ParseExact(
                dob, "dd-MMM-yyyy",
                System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            string updtSQL = "UPDATE scm.scm_cstmr_suplr SET " +
                  "cust_sup_name = '" + cstmrname.Replace("'", "''") +
                  "', cust_sup_desc = '" + cstmrdesc.Replace("'", "''") +
                  "', last_update_by = " + Global.myBscActn.user_id +
                  ", last_update_date = '" + dateStr +
                  "', cust_sup_clssfctn='" + clssfctn.Replace("'", "''") +
                  "', cust_or_sup='" + cstmrTyp.Replace("'", "''") +
                  "', dflt_pybl_accnt_id=" + pyblAccntID +
                  ", dflt_rcvbl_accnt_id=" + rcvblAccntID +
                  ", dob_estblshmnt='" + dob.Replace("'", "''") +
                  "', is_enabled='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isEnbld) +
                  "', firm_brand_name = '" + brndNm.Replace("'", "''") +
                  "', type_of_organisation = '" + typeOfOrg.Replace("'", "''") +
                  "', company_reg_num ='" + regNum.Replace("'", "''") +
                  "', date_of_incorptn = '" + dteIncp.Replace("'", "''") +
                  "', type_of_incorporation = '" + typeOfIncp.Replace("'", "''") +
                  "', vat_number = '" + vatNum.Replace("'", "''") +
                  "', tin_number = '" + tinNum.Replace("'", "''") +
                  "', ssnit_reg_number = '" + ssnitNum.Replace("'", "''") +
                  "', no_of_emplyees = " + noEmps +
                  ", description_of_services = '" + descSrvcs.Replace("'", "''") +
                  "', list_of_services = '" + lstSrvcs.Replace("'", "''") +
                  "' WHERE (cust_sup_id = " + spplrid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtCstSplrSiteRec(long siteID, string sitename,
      string sitedesc, string cntctPrsn, string cntctNos, string email,
          string bankNm, string bnkBrnch, string accNum, string blngAddrs,
          string shpngAddrs, int taxCode, int dscntCode, string swift_code,
                 string nationality, string national_id_typ,
         string id_number, string date_issued, string expiry_date,
                 string other_info, bool isEnbld, string iban_number, int accCurID)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE scm.scm_cstmr_suplr_sites " +
         "SET contact_person_name='" + cntctPrsn.Replace("'", "''") +
                  "', contact_nos='" + cntctNos.Replace("'", "''") +
                  "', email='" + email.Replace("'", "''") +
                  "', last_update_by=" + Global.myBscActn.user_id + ", last_update_date='" + dateStr +
                  "', site_name='" + sitename.Replace("'", "''") +
                  "', site_desc='" + sitedesc.Replace("'", "''") + "', bank_name='" +
                  bankNm.Replace("'", "''") + "', bank_branch='" + bnkBrnch.Replace("'", "''") +
                  "', bank_accnt_number='" + accNum.Replace("'", "''") +
                  "', wth_tax_code_id=" + taxCode + ", discount_code_id=" + dscntCode +
                  ", billing_address='" + blngAddrs.Replace("'", "''") + "', " +
             "ship_to_address='" + shpngAddrs.Replace("'", "''") + "', " +
             "swift_code='" + swift_code.Replace("'", "''") + "', " +
             "nationality='" + nationality.Replace("'", "''") + "', " +
             "national_id_typ='" + national_id_typ.Replace("'", "''") + "', " +
             "id_number='" + id_number.Replace("'", "''") + "', " +
             "date_issued='" + date_issued.Replace("'", "''") + "', " +
             "expiry_date='" + expiry_date.Replace("'", "''") + "', " +
             "other_info='" + other_info.Replace("'", "''") +
             "', is_enabled='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isEnbld) + "', " +
             "iban_number='" + iban_number.Replace("'", "''") +
             "', accnt_cur_id=" + accCurID +
             " WHERE cust_sup_site_id = " + siteID + "";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static bool isCstSplrSiteInUse(int recID)
        {
            string strSql = "SELECT a.supplier_site_id " +
             "FROM scm.scm_prchs_docs_hdr a " +
             "WHERE(a.supplier_site_id = " + recID + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            strSql = "SELECT a.customer_site_id " +
             "FROM scm.scm_sales_invc_hdr a " +
             "WHERE(a.customer_site_id = " + recID + ")";
            dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            strSql = "SELECT a.customer_site_id " +
            "FROM accb.accb_rcvbls_invc_hdr a " +
            "WHERE(a.customer_site_id = " + recID + ")";
            dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            strSql = "SELECT a.supplier_site_id " +
            "FROM accb.accb_pybls_invc_hdr a " +
            "WHERE(a.supplier_site_id = " + recID + ")";
            dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            strSql = "SELECT a.supplier_site_id " +
            "FROM accb.accb_ptycsh_vchr_hdr a " +
            "WHERE(a.supplier_site_id = " + recID + ")";
            dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static bool isCstSplrInUse(int recID)
        {
            //string strSql = "SELECT a.cust_sup_site_id " +
            // "FROM scm.scm_cstmr_suplr_sites a " +
            // "WHERE(a.cust_supplier_id = " + recID + ")";
            //DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //if (dtst.Tables[0].Rows.Count > 0)
            //{
            //  return true;
            //}
            string strSql = "SELECT a.supplier_id " +
             "FROM scm.scm_prchs_docs_hdr a " +
             "WHERE(a.supplier_id = " + recID + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            strSql = "SELECT a.customer_id " +
             "FROM scm.scm_sales_invc_hdr a " +
             "WHERE(a.customer_id = " + recID + ")";
            dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            strSql = "SELECT a.customer_id " +
            "FROM accb.accb_rcvbls_invc_hdr a " +
            "WHERE(a.customer_id = " + recID + ")";
            dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            strSql = "SELECT a.supplier_id " +
            "FROM accb.accb_pybls_invc_hdr a " +
            "WHERE(a.supplier_id = " + recID + ")";
            dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            strSql = "SELECT a.supplier_id " +
            "FROM accb.accb_ptycsh_vchr_hdr a " +
            "WHERE(a.supplier_id = " + recID + ")";
            dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static bool isTaxItmInUse(int itmID)
        {
            if (itmID <= 0)
            {
                return false;
            }
            string strSql = "SELECT a.tax_code_id " +
             "FROM scm.scm_sales_invc_det a " +
             "WHERE(a.tax_code_id = " + itmID + ") LIMIT 1 OFFSET 0";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            strSql = "SELECT a.code_id_behind " +
             "FROM scm.scm_doc_amnt_smmrys a " +
             "WHERE(a.code_id_behind = " + itmID + " and a.code_id_behind>0) LIMIT 1 OFFSET 0";
            dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            strSql = "SELECT a.code_id_behind " +
             "FROM accb.accb_pybls_amnt_smmrys a " +
             "WHERE(a.code_id_behind = " + itmID + " and a.code_id_behind>0) LIMIT 1 OFFSET 0";
            dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            strSql = "SELECT a.code_id_behind " +
             "FROM accb.accb_rcvbl_amnt_smmrys a " +
             "WHERE(a.code_id_behind = " + itmID + " and a.code_id_behind>0) LIMIT 1 OFFSET 0";
            dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            strSql = "SELECT a.wth_tax_code_id " +
             "FROM scm.scm_cstmr_suplr_sites a " +
             "WHERE(a.wth_tax_code_id = " + itmID + " or a.discount_code_id = " + itmID + ") LIMIT 1 OFFSET 0";
            dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            strSql = "SELECT a.item_id " +
             "FROM inv.inv_itm_list a " +
             "WHERE(a.tax_code_id = " + itmID + " or a.dscnt_code_id = " +
             itmID + " or a.extr_chrg_id = " + itmID + ") LIMIT 1 OFFSET 0";
            dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static void deleteTaxItm(long itmid, string itmNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Item Name = " + itmNm;
            string delSQL = "DELETE FROM scm.scm_tax_codes WHERE code_id = " + itmid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static int getChargeItmID(string itmname, int orgid)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "select code_id from scm.scm_tax_codes where lower(code_name) = '" +
             itmname.Replace("'", "''").ToLower() + "' and org_id = " + orgid;
            dtSt = Global.mnFrm.cmCde.selectDataNoParams(sqlStr);
            if (dtSt.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtSt.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        public static DataSet get_One_TaxDet(int codeID)
        {
            string strSql = "SELECT a.code_id, a.code_name, a.code_desc, " +
             "a.itm_type, a.is_enabled, a.taxes_payables_accnt_id, " +
             "a.dscount_expns_accnt_id, " +
             @"a.chrge_revnu_accnt_id, a.sql_formular, a.is_recovrbl_tax, a.is_withldng_tax, 
        a.tax_expense_accnt_id, a.prchs_dscnt_accnt_id, a.chrge_expns_accnt_id, a.is_parent, a.child_code_ids " +
             "FROM scm.scm_tax_codes a " +
             "WHERE(a.code_id = " + codeID + ") ORDER BY a.itm_type, a.code_name";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.taxFrm.rec_SQL = strSql;
            return dtst;
        }

        public static string getTaxNm(int codeID)
        {
            string strSql = "SELECT a.code_name FROM scm.scm_tax_codes a " +
             "WHERE(a.code_id = " + codeID + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            return "";
        }

        public static int getTaxID(string codeNm)
        {
            string strSql = "SELECT a.code_id FROM scm.scm_tax_codes a " +
             "WHERE(a.code_name = '" + codeNm.Replace("'", "''") +
             "' and a.org_id = " + Global.mnFrm.cmCde.Org_id + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static DataSet get_Basic_Tax(string searchWord, string searchIn,
      Int64 offset, int limit_size, int orgID)
        {
            string strSql = "";
            if (searchIn == "Item Name")
            {
                strSql = "SELECT a.code_id, a.code_name, a.itm_type " +
               "FROM scm.scm_tax_codes a " +
               "WHERE ((a.code_name ilike '" + searchWord.Replace("'", "''") +
               "') AND (a.org_id = " + orgID + ")) ORDER BY a.itm_type, a.code_name LIMIT " + limit_size +
               " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }
            else if (searchIn == "Item Description")
            {
                strSql = "SELECT a.code_id, a.code_name, a.itm_type " +
               "FROM scm.scm_tax_codes a " +
              "WHERE ((a.code_desc ilike '" + searchWord.Replace("'", "''") +
               "') AND (a.org_id = " + orgID + ")) ORDER BY a.itm_type, a.code_name LIMIT " + limit_size +
               " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }
            else if (searchIn == "Item Type")
            {
                strSql = "SELECT a.code_id, a.code_name, a.itm_type " +
               "FROM scm.scm_tax_codes a " +
              "WHERE ((a.itm_type ilike '" + searchWord.Replace("'", "''") +
               "') AND (a.org_id = " + orgID + ")) ORDER BY a.itm_type, a.code_name LIMIT " + limit_size +
               " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }
            Global.taxFrm.rec_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_Total_Tax(string searchWord, string searchIn, int orgID)
        {
            string strSql = "";
            if (searchIn == "Item Name")
            {
                strSql = "SELECT count(1) " +
                "FROM scm.scm_tax_codes a " +
               "WHERE ((a.code_name ilike '" + searchWord.Replace("'", "''") +
               "') AND (a.org_id = " + orgID + "))";
            }
            else if (searchIn == "Item Description")
            {
                strSql = "SELECT count(1)  " +
                "FROM scm.scm_tax_codes a " +
              "WHERE ((a.code_desc ilike '" + searchWord.Replace("'", "''") +
               "') AND (a.org_id = " + orgID + "))";
            }
            else if (searchIn == "Item Type")
            {
                strSql = "SELECT count(1)  " +
                "FROM scm.scm_tax_codes a " +
              "WHERE ((a.itm_type ilike '" + searchWord.Replace("'", "''") +
               "') AND (a.org_id = " + orgID + "))";
            }
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region "CUSTOMERS & SUPPLIERS..."
        public static long getCstmrSplrID(string cstmrname, int orgid)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "select cust_sup_id from scm.scm_cstmr_suplr where lower(cust_sup_name) = '" +
             cstmrname.Replace("'", "''").ToLower() + "' and org_id = " + orgid;
            dtSt = Global.mnFrm.cmCde.selectDataNoParams(sqlStr);
            if (dtSt.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtSt.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        public static long getCstmrSplrSiteID(string cstmrsitename, long cstmrid)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "select cust_sup_site_id from scm.scm_cstmr_suplr_sites where lower(site_name) = '" +
             cstmrsitename.Replace("'", "''").ToLower() + "' and cust_supplier_id = " + cstmrid;
            dtSt = Global.mnFrm.cmCde.selectDataNoParams(sqlStr);
            if (dtSt.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtSt.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        public static void updtDOBs()
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE scm.scm_cstmr_suplr SET dob_estblshmnt=substr(creation_date,1,10), " +
             "last_update_by=" + Global.myBscActn.user_id + ", last_update_date='" + dateStr +
                  "'  WHERE dob_estblshmnt='' or dob_estblshmnt IS NULL";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static DataSet get_One_CstmrDetNSites(int lmit)
        {
            string extrWhr = "";

            if (lmit >= 0)
            {
                extrWhr = " LIMIT " + lmit + @" OFFSET 0";
            }
            else if (lmit < 0)
            {
                extrWhr = "";
            }

            string strSql = "SELECT a.cust_sup_id, a.cust_or_sup, a.cust_sup_name, a.cust_sup_desc, " +
            @"a.cust_sup_clssfctn, a.dflt_pybl_accnt_id, a.dflt_rcvbl_accnt_id, a.lnkd_prsn_id, 
       a.person_gender, to_char(to_timestamp(a.dob_estblshmnt,'YYYY-MM-DD'),'DD-Mon-YYYY'), 
       a.is_enabled, a.firm_brand_name, a.type_of_organisation, 
       a.company_reg_num, a.date_of_incorptn, a.type_of_incorporation, a.vat_number, 
       a.tin_number, a.ssnit_reg_number, a.no_of_emplyees, a.description_of_services, 
       a.list_of_services, b.cust_sup_site_id, b.site_name, b.site_desc, " +
             "b.bank_name, b.bank_branch, b.bank_accnt_number, b.wth_tax_code_id, " +
             "b.discount_code_id, b.billing_address, b.ship_to_address, " +
             @"b.contact_person_name, b.contact_nos, b.email, b.swift_code, 
       b.nationality, b.national_id_typ, b.id_number, b.date_issued, b.expiry_date, 
       b.other_info, b.is_enabled, b.iban_number, b.accnt_cur_id, gst.get_pssbl_val(b.accnt_cur_id) " +
             "FROM scm.scm_cstmr_suplr a, scm.scm_cstmr_suplr_sites b " +
             @"WHERE(a.cust_sup_id = b.cust_supplier_id) 
      ORDER BY a.cust_or_sup, a.cust_sup_name" + extrWhr;

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.taxFrm.rec_SQL = strSql;
            return dtst;
        }

        public static DataSet get_One_CstmrDet(int cstmrID)
        {
            string strSql = "SELECT a.cust_sup_id, a.cust_or_sup, a.cust_sup_name, a.cust_sup_desc, " +
            @"a.cust_sup_clssfctn, a.dflt_pybl_accnt_id, a.dflt_rcvbl_accnt_id, a.lnkd_prsn_id, 
       a.person_gender, to_char(to_timestamp(a.dob_estblshmnt,'YYYY-MM-DD'),'DD-Mon-YYYY'), 
       is_enabled, firm_brand_name, type_of_organisation, 
       company_reg_num, date_of_incorptn, type_of_incorporation, vat_number, 
       tin_number, ssnit_reg_number, no_of_emplyees, description_of_services, 
       list_of_services, cstmr_image " +
             "FROM scm.scm_cstmr_suplr a " +
             "WHERE(a.cust_sup_id = " + cstmrID + ") ORDER BY a.cust_or_sup, a.cust_sup_name";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.taxFrm.rec_SQL = strSql;
            return dtst;
        }

        public static DataSet get_One_CstmrSitesDt(int cstmrSiteID)
        {
            string strSql = "SELECT a.cust_sup_site_id, a.site_name, a.site_desc, " +
             "a.bank_name, a.bank_branch, a.bank_accnt_number, a.wth_tax_code_id, " +
             "a.discount_code_id, a.billing_address, a.ship_to_address, " +
             @"a.contact_person_name, a.contact_nos, a.email, a.swift_code, 
       a.nationality, a.national_id_typ, a.id_number, a.date_issued, a.expiry_date, 
       a.other_info, is_enabled, a.iban_number, a.accnt_cur_id, gst.get_pssbl_val(a.accnt_cur_id) " +
             "FROM scm.scm_cstmr_suplr_sites a " +
             "WHERE(a.cust_sup_site_id = " + cstmrSiteID +
             ") ORDER BY a.site_name";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.taxFrm.rec_SQL = strSql;
            return dtst;
        }

        public static DataSet get_One_CstmrBscSites(int cstmrID)
        {
            string strSql = "SELECT a.cust_sup_site_id, a.site_name " +
             "FROM scm.scm_cstmr_suplr_sites a " +
             "WHERE(a.cust_supplier_id = " + cstmrID +
             ") ORDER BY a.cust_sup_site_id DESC";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.custFrm.recDt_SQL = strSql;
            return dtst;
        }

        public static DataSet get_Basic_Cstmr(string searchWord, string searchIn,
      Int64 offset, int limit_size, int orgID)
        {
            string strSql = "";
            if (searchIn == "Customer/Supplier Name")
            {
                strSql = "SELECT a.cust_sup_id, a.cust_sup_name, a.cust_or_sup " +
               "FROM scm.scm_cstmr_suplr a " +
               "WHERE ((a.cust_sup_name ilike '" + searchWord.Replace("'", "''") +
               "') AND (a.org_id = " + orgID + ")) ORDER BY a.cust_sup_id DESC LIMIT " + limit_size +
               " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }
            else if (searchIn == "Customer/Supplier Description")
            {
                strSql = "SELECT a.cust_sup_id, a.cust_sup_name, a.cust_or_sup " +
               "FROM scm.scm_cstmr_suplr a " +
              "WHERE ((a.cust_sup_desc ilike '" + searchWord.Replace("'", "''") +
               "') AND (a.org_id = " + orgID + ")) ORDER BY a.cust_sup_id DESC LIMIT " + limit_size +
               " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }
            else if (searchIn == "Customer/Supplier Type")
            {
                strSql = "SELECT a.cust_sup_id, a.cust_sup_name, a.cust_or_sup " +
               "FROM scm.scm_cstmr_suplr a " +
              "WHERE ((a.cust_or_sup ilike '" + searchWord.Replace("'", "''") +
               "') AND (a.org_id = " + orgID + ")) ORDER BY a.cust_sup_id DESC LIMIT " + limit_size +
               " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }
            else if (searchIn == "Linked Person")
            {
                strSql = "SELECT a.cust_sup_id, a.cust_sup_name, a.cust_or_sup " +
               "FROM scm.scm_cstmr_suplr a " +
              "WHERE (((prs.get_prsn_name(a.lnkd_prsn_id) || ' (' || prs.get_prsn_loc_id(a.lnkd_prsn_id) || ')') ilike '" + searchWord.Replace("'", "''") +
               "') AND (a.org_id = " + orgID + ")) ORDER BY a.cust_sup_id DESC LIMIT " + limit_size +
               " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }
            Global.custFrm.rec_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_Total_Cstmr(string searchWord, string searchIn, int orgID)
        {
            string strSql = "";
            if (searchIn == "Customer/Supplier Name")
            {
                strSql = "SELECT count(1) " +
                "FROM scm.scm_cstmr_suplr a " +
               "WHERE ((a.cust_sup_name ilike '" + searchWord.Replace("'", "''") +
               "') AND (a.org_id = " + orgID + "))";
            }
            else if (searchIn == "Customer/Supplier Description")
            {
                strSql = "SELECT count(1)  " +
                "FROM scm.scm_cstmr_suplr a " +
              "WHERE ((a.cust_sup_desc ilike '" + searchWord.Replace("'", "''") +
               "') AND (a.org_id = " + orgID + "))";
            }
            else if (searchIn == "Customer/Supplier Type")
            {
                strSql = "SELECT count(1)  " +
                "FROM scm.scm_cstmr_suplr a " +
              "WHERE ((a.cust_or_sup ilike '" + searchWord.Replace("'", "''") +
               "') AND (a.org_id = " + orgID + "))";
            }
            else if (searchIn == "Linked Person")
            {
                strSql = "SELECT count(1) " +
               "FROM scm.scm_cstmr_suplr a " +
              "WHERE (((prs.get_prsn_name(a.lnkd_prsn_id) || ' (' || prs.get_prsn_loc_id(a.lnkd_prsn_id) || ')') ilike '" + searchWord.Replace("'", "''") +
               "') AND (a.org_id = " + orgID + ")) ";
            }
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region "DEFAULT ACCOUNTS..."
        public static DataSet get_One_DfltAcnt(int orgID)
        {
            string strSql = "SELECT row_id, itm_inv_asst_acnt_id, cost_of_goods_acnt_id, expense_acnt_id, " +
                  "prchs_rtrns_acnt_id, rvnu_acnt_id, sales_rtrns_acnt_id, sales_cash_acnt_id, " +
                  "sales_check_acnt_id, sales_rcvbl_acnt_id, rcpt_cash_acnt_id, " +
                  "rcpt_lblty_acnt_id, inv_adjstmnts_lblty_acnt_id, ttl_caa, ttl_cla, " +
                  @"ttl_aa, ttl_la, ttl_oea, ttl_ra, ttl_cgsa, ttl_ia, ttl_pea,
      sales_dscnt_accnt, prchs_dscnt_accnt, sales_lblty_acnt_id, bad_debt_acnt_id, rcpt_rcvbl_acnt_id, petty_cash_acnt_id " +
             "FROM scm.scm_dflt_accnts a " +
             "WHERE(a.org_id = " + orgID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static int get_DfltRcvblAcnt(int orgID)
        {
            string strSql = "SELECT org.get_dflt_accnt_id(" + Global.mnFrm.prsn_id + ", sales_rcvbl_acnt_id) " +
             "FROM scm.scm_dflt_accnts a " +
             "WHERE(a.org_id = " + orgID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static int get_DfltRcptRcvblAcnt(int orgID)
        {
            string strSql = "SELECT org.get_dflt_accnt_id(" + Global.mnFrm.prsn_id + ", rcpt_rcvbl_acnt_id) " +
             "FROM scm.scm_dflt_accnts a " +
             "WHERE(a.org_id = " + orgID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static int get_DfltPtyCshAcnt(int orgID)
        {
            string strSql = "SELECT org.get_dflt_accnt_id(" + Global.mnFrm.prsn_id + ", petty_cash_acnt_id) " +
             "FROM scm.scm_dflt_accnts a " +
             "WHERE(a.org_id = " + orgID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static int get_PyblPrepayDocAcntID(long prepayDocID)
        {
            string strSql = "SELECT asset_expns_acnt_id, pybls_smmry_id " +
              "FROM accb.accb_pybls_amnt_smmrys a " +
              "WHERE(a.src_pybls_hdr_id = " + prepayDocID +
              " and pybls_smmry_type = '1Initial Amount') ORDER BY pybls_smmry_id ASC LIMIT 1 OFFSET 0";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static int get_PyblPrepayDocLbltyAcntID(long prepayDocID)
        {
            string strSql = "SELECT liability_acnt_id, pybls_smmry_id " +
              "FROM accb.accb_pybls_amnt_smmrys a " +
              "WHERE(a.src_pybls_hdr_id = " + prepayDocID +
              " and pybls_smmry_type = '1Initial Amount') ORDER BY pybls_smmry_id ASC LIMIT 1 OFFSET 0";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static double get_PyblPrepayDocAvlblAmnt(long prepayDocID)
        {
            string strSql = "SELECT invoice_amount-invc_amnt_appld_elswhr " +
              "FROM accb.accb_pybls_invc_hdr a " +
              "WHERE(a.pybls_invc_hdr_id = " + prepayDocID +
              " and (invoice_amount-invc_amnt_appld_elswhr)>0)";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }

        public static int get_RcvblPrepayDocRvnuAcntID(long prepayDocID)
        {
            string strSql = "SELECT rvnu_acnt_id, rcvbl_smmry_id " +
              "FROM accb.accb_rcvbl_amnt_smmrys a " +
              "WHERE(a.src_rcvbl_hdr_id = " + prepayDocID +
              " and rcvbl_smmry_type = '1Initial Amount') ORDER BY rcvbl_smmry_id ASC LIMIT 1 OFFSET 0";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static int get_RcvblPrepayDocRcvblAcntID(long prepayDocID)
        {
            string strSql = "SELECT rcvbl_acnt_id, rcvbl_smmry_id " +
              "FROM accb.accb_rcvbl_amnt_smmrys a " +
              "WHERE(a.src_rcvbl_hdr_id = " + prepayDocID +
              " and rcvbl_smmry_type = '1Initial Amount') ORDER BY rcvbl_smmry_id ASC LIMIT 1 OFFSET 0";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static double get_RcvblPrepayDocAvlblAmnt(long prepayDocID)
        {
            string strSql = "SELECT invoice_amount-invc_amnt_appld_elswhr " +
              "FROM accb.accb_rcvbls_invc_hdr a " +
              "WHERE(a.rcvbls_invc_hdr_id = " + prepayDocID +
              " and (invoice_amount-invc_amnt_appld_elswhr)>0)";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }

        public static double get_RcvblPrepayDocAppldAmnt(long prepayDocID)
        {
            string strSql = "SELECT invc_amnt_appld_elswhr " +
              "FROM accb.accb_rcvbls_invc_hdr a " +
              "WHERE(a.rcvbls_invc_hdr_id = " + prepayDocID +
              " and (invc_amnt_appld_elswhr)>0)";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }

        public static long get_RcvblPrepayDocUsages(long prepayDocID, string rcvblDoctype)
        {
            string strSql = @"SELECT count(1) FROM (SELECT y.rcvbls_invc_number a, z.rcvbl_smmry_amnt || ' (' || y.approval_status || ')' b, '' c, 1 d, 
                z.appld_prepymnt_doc_id||'' e, accb.get_src_doc_type(z.appld_prepymnt_doc_id,'Customer') f 
            FROM accb.accb_rcvbls_invc_hdr y,accb.accb_rcvbl_amnt_smmrys z 
                WHERE y.rcvbls_invc_hdr_id =z.src_rcvbl_hdr_id and z.appld_prepymnt_doc_id > 0 
                    UNION 
            Select accb.get_src_doc_num(w.src_doc_id, w.src_doc_typ) a, 
            CASE WHEN (w.amount_paid>0 and w.change_or_balance <=0) or (w.amount_paid < 0 and w.change_or_balance >= 0) THEN 
                Round(((w.amount_paid/abs(w.amount_paid))*w.amount_paid)-w.change_or_balance,2)|| ' (' || w.pymnt_vldty_status || ')' 
                ELSE w.amount_paid || ' (' || w.pymnt_vldty_status || ')' END b, '' c, 1 d, 
            w.prepay_doc_id||'' e, prepay_doc_type f FROM accb.accb_payments w WHERE w.prepay_doc_id>0 and prepay_doc_type ilike '%Customer%' and pymnt_vldty_status='VALID') tbl1 " +
              "WHERE(tbl1.e = '' || " + prepayDocID + " and tbl1.f = '" + rcvblDoctype.Replace("'", "''") + "')";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }

        public static long get_PyblPrepayDocUsages(long prepayDocID, string rcvblDoctype)
        {
            string strSql = @"SELECT count(1) FROM (SELECT y.pybls_invc_number a, z.pybls_smmry_amnt || ' (' || y.approval_status || ')' b, '' c, 1 d, 
                z.appld_prepymnt_doc_id||'' e, accb.get_src_doc_type(z.appld_prepymnt_doc_id,'Supplier') f 
            FROM accb.accb_pybls_invc_hdr y,accb.accb_pybls_amnt_smmrys z 
                WHERE y.pybls_invc_hdr_id =z.src_pybls_hdr_id and z.appld_prepymnt_doc_id > 0 
                    UNION 
            Select accb.get_src_doc_num(w.src_doc_id, w.src_doc_typ) a, 
            CASE WHEN (w.amount_paid>0 and w.change_or_balance <=0) or (w.amount_paid < 0 and w.change_or_balance >= 0) THEN 
                Round(((w.amount_paid/abs(w.amount_paid))*w.amount_paid)-w.change_or_balance,2) || ' (' || w.pymnt_vldty_status || ')' 
                ELSE w.amount_paid || ' (' || w.pymnt_vldty_status || ')' END b, '' c, 1 d, 
            w.prepay_doc_id || '' e, prepay_doc_type f FROM accb.accb_payments w WHERE w.prepay_doc_id>0 and prepay_doc_type ilike '%Supplier%' and pymnt_vldty_status='VALID') tbl1 " +
              "WHERE(tbl1.e = '' || " + prepayDocID + " and tbl1.f = '" + rcvblDoctype.Replace("'", "''") + "')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }

        public static double get_PyblPrepayDocAppldAmnt(long prepayDocID)
        {
            string strSql = "SELECT invc_amnt_appld_elswhr " +
              "FROM accb.accb_pybls_invc_hdr a " +
              "WHERE(a.pybls_invc_hdr_id = " + prepayDocID +
              " and (invc_amnt_appld_elswhr)>0)";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }

        public static int get_DfltSalesLbltyAcnt(int orgID)
        {
            string strSql = "SELECT org.get_dflt_accnt_id(" + Global.mnFrm.prsn_id + ", sales_lblty_acnt_id) " +
             "FROM scm.scm_dflt_accnts a " +
             "WHERE(a.org_id = " + orgID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static DataSet get_PrsnCstmrDet(long prsnID)
        {
            string strSql = @"SELECT sur_name || ' ' || first_name || ' ' || other_names 
|| ' (' || title || ') (' || local_id_no || ') ' fullname,
       gender, to_char(to_timestamp(date_of_birth,'YYYY-MM-DD'),'DD-Mon-YYYY') dob,
       cntct_no_mobl, email, pstl_addrs, res_address, nationality 
       FROM prs.prsn_names_nos a
       WHERE(a.person_id = " + prsnID + ")";

            return Global.mnFrm.cmCde.selectDataNoParams(strSql);

        }

        public static int get_DfltPyblAcnt(int orgID)
        {
            string strSql = "SELECT org.get_dflt_accnt_id(" + Global.mnFrm.prsn_id + ", rcpt_lblty_acnt_id) " +
             "FROM scm.scm_dflt_accnts a " +
             "WHERE(a.org_id = " + orgID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static int get_DfltInvAcnt(int orgID)
        {
            string strSql = "SELECT org.get_dflt_accnt_id(" + Global.mnFrm.prsn_id + ", itm_inv_asst_acnt_id) " +
             "FROM scm.scm_dflt_accnts a " +
             "WHERE(a.org_id = " + orgID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static int get_DfltCSGAcnt(int orgID)
        {
            string strSql = "SELECT org.get_dflt_accnt_id(" + Global.mnFrm.prsn_id + ", cost_of_goods_acnt_id) " +
             "FROM scm.scm_dflt_accnts a " +
             "WHERE(a.org_id = " + orgID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static int get_DfltExpnsAcnt(int orgID)
        {
            string strSql = "SELECT org.get_dflt_accnt_id(" + Global.mnFrm.prsn_id + ", expense_acnt_id) " +
             "FROM scm.scm_dflt_accnts a " +
             "WHERE(a.org_id = " + orgID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static int get_DfltRvnuAcnt(int orgID)
        {
            string strSql = "SELECT org.get_dflt_accnt_id(" + Global.mnFrm.prsn_id + ", rvnu_acnt_id) " +
             "FROM scm.scm_dflt_accnts a " +
             "WHERE(a.org_id = " + orgID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static int get_DfltSRAcnt(int orgID)
        {
            string strSql = "SELECT org.get_dflt_accnt_id(" + Global.mnFrm.prsn_id + ", sales_rtrns_acnt_id) " +
             "FROM scm.scm_dflt_accnts a " +
             "WHERE(a.org_id = " + orgID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static int get_DfltCashAcnt(int orgID)
        {
            string strSql = "SELECT org.get_dflt_accnt_id(" + Global.mnFrm.prsn_id + ", sales_cash_acnt_id) " +
             "FROM scm.scm_dflt_accnts a " +
             "WHERE(a.org_id = " + orgID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static int get_DfltCheckAcnt(int orgID)
        {
            string strSql = "SELECT org.get_dflt_accnt_id(" + Global.mnFrm.prsn_id + ", sales_check_acnt_id) " +
             "FROM scm.scm_dflt_accnts a " +
             "WHERE(a.org_id = " + orgID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }
        #endregion

        #region "PAYMENT METHODS..."
        public static void createPymntMthd(int orgid, string mthdNm, string mthdDesc,
          int accntID, string docType, string bckgrndPrcss, bool isenbld)
        {

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_paymnt_mthds(
            pymnt_mthd_name, pymnt_mthd_desc, current_asst_acnt_id, 
            created_by, creation_date, last_update_by, last_update_date, 
            supported_doc_type, bckgrnd_process_name, org_id, is_enabled) " +
                  "VALUES ('" + mthdNm.Replace("'", "''") +
                  "', '" + mthdDesc.Replace("'", "''") +
                  "', " + accntID +
                  ", " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', '" + docType.Replace("'", "''") +
                  "', '" + bckgrndPrcss.Replace("'", "''") + "', " + orgid +
                  ",'" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isenbld) +
                  "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtPymntMthd(long mthdID, string mthdNm, string mthdDesc,
          int accntID, string docType, string bckgrndPrcss, bool isenbld)
        {

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_paymnt_mthds SET 
            pymnt_mthd_name='" + mthdNm.Replace("'", "''") +
                  "', pymnt_mthd_desc='" + mthdDesc.Replace("'", "''") +
                  "', current_asst_acnt_id=" + accntID +
                  ", last_update_by=" + Global.myBscActn.user_id + ", last_update_date='" + dateStr +
                  "', supported_doc_type='" + docType.Replace("'", "''") +
                  "', bckgrnd_process_name='" + bckgrndPrcss.Replace("'", "''") +
                  "', is_enabled = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isenbld) +
                  "' WHERE paymnt_mthd_id = " + mthdID;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void deletePymntMthd(long valLnid, string mthdNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Payment Method Name = " + mthdNm;
            string delSQL = "DELETE FROM accb.accb_paymnt_mthds WHERE paymnt_mthd_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static DataSet get_PymntMthds(long offset,
          int limit_size, long orgID)
        {
            string strSql = "";

            strSql = @"SELECT paymnt_mthd_id, pymnt_mthd_name, pymnt_mthd_desc, current_asst_acnt_id,         
       supported_doc_type, bckgrnd_process_name, is_enabled FROM accb.accb_paymnt_mthds a " +
              "WHERE((a.org_id = " + orgID + ")) ORDER BY 2, 5 LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (Global.pymntFrm != null)
            {
                Global.pymntFrm.pymntMthdSQL = strSql;
            }
            return dtst;
        }

        public static long get_Total_PymntMthds(long orgID)
        {
            string strSql = "";

            strSql = @"SELECT count(1) FROM accb.accb_paymnt_mthds a " +
              "WHERE((a.org_id = " + orgID + "))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region "DOCUMENT TEMPLATES..."
        public static void createDocTmpltHdr(int orgid, string tmpltNm, string tmpltDesc,
          string docType, bool isenbld)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_doc_tmplts_hdr(
            doc_tmplt_name, doc_tmplt_desc, created_by, 
            creation_date, last_update_by, last_update_date, 
            is_enabled, org_id, doc_type) " +
                  "VALUES ('" + tmpltNm.Replace("'", "''") +
                  "', '" + tmpltDesc.Replace("'", "''") +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "','" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isenbld) +
                  "', " + orgid +
                  ", '" + docType.Replace("'", "''") +
                  "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void createDocTmpltDet(long hdrID, string lineType, string lineDesc,
          bool autoCalc, string incrDcrs, int accntID, int codeBhnd)
        {

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_doc_tmplts_det(
            doc_tmplts_hdr_id, line_item_type, line_description, 
            auto_calc, incrs_dcrs, costing_accnt_id, created_by, creation_date, 
            last_update_by, last_update_date, code_behind_id) " +
                  "VALUES (" + hdrID +
                  ", '" + lineType.Replace("'", "''") +
                  "', '" + lineDesc.Replace("'", "''") +
                  "', '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(autoCalc) +
                  "', '" + incrDcrs.Replace("'", "''") +
                  "', " + accntID +
                  ", " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + codeBhnd +
                  ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtDocTmpltDet(long tmpltDetID, string lineType, string lineDesc,
          bool autoCalc, string incrDcrs, int accntID, int codeBhnd)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_doc_tmplts_det SET 
            line_item_type='" + lineType.Replace("'", "''") +
                  "', line_description='" + lineDesc.Replace("'", "''") +
                  "', last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', incrs_dcrs='" + incrDcrs.Replace("'", "''") +
                  "', auto_calc = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(autoCalc) +
                  "', costing_accnt_id = " + accntID +
                  ", code_behind_id = " + codeBhnd +
                  " WHERE doc_tmplt_det_id = " + tmpltDetID;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void updtDocTmpltHdr(long tmpltHdrID, string tmpltNm,
          string tmpltDesc, string docType, bool isenbld)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_doc_tmplts_hdr SET 
            doc_tmplt_name='" + tmpltNm.Replace("'", "''") +
                  "', doc_tmplt_desc='" + tmpltDesc.Replace("'", "''") +
                  "', last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', doc_type='" + docType.Replace("'", "''") +
                  "', is_enabled = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isenbld) +
                  "' WHERE doc_tmplts_hdr_id = " + tmpltHdrID;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void deleteTmpltHdrNDet(long valLnid, string tmpltNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Document Template Name = " + tmpltNm;
            string delSQL = "DELETE FROM accb.accb_doc_tmplts_det WHERE doc_tmplts_hdr_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
            delSQL = "DELETE FROM accb.accb_doc_tmplts_hdr WHERE doc_tmplts_hdr_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteTmpltDet(long valLnid, string tmpltNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Document Template Name/Line Type = " + tmpltNm;
            string delSQL = "DELETE FROM accb.accb_doc_tmplts_det WHERE doc_tmplt_det_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static DataSet get_DocTmpltsHdr(string searchWord, string searchIn, long offset,
          int limit_size, long orgID)
        {
            string strSql = "";
            string whrcls = "";

            if (searchIn == "Template Name")
            {
                whrcls = " and (a.doc_tmplt_name ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Template Description")
            {
                whrcls = " and (a.doc_tmplt_desc ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Document Type")
            {
                whrcls = " and (a.doc_type ilike '" + searchWord.Replace("'", "''") + "')";
            }
            strSql = @"SELECT doc_tmplts_hdr_id, doc_tmplt_name, doc_tmplt_desc, doc_type, is_enabled
        FROM accb.accb_doc_tmplts_hdr a " +
              "WHERE((a.org_id = " + orgID + ")" + whrcls +
              ") ORDER BY doc_tmplt_name LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (Global.pyblsFrm != null)
            {
                Global.pyblsFrm.docTmplt_SQL = strSql;
            }
            if (Global.rcvblsFrm != null)
            {
                Global.rcvblsFrm.docTmplt_SQL = strSql;
            }
            return dtst;
        }

        public static long get_Total_DocTmpltsHdr(string searchWord, string searchIn, long orgID)
        {
            string strSql = "";
            string whrcls = "";

            if (searchIn == "Template Name")
            {
                whrcls = " and (a.doc_tmplt_name ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Template Description")
            {
                whrcls = " and (a.doc_tmplt_desc ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Document Type")
            {
                whrcls = " and (a.doc_type ilike '" + searchWord.Replace("'", "''") + "')";
            }
            strSql = @"SELECT count(1) FROM accb.accb_doc_tmplts_hdr a " +
              "WHERE((a.org_id = " + orgID + ")" + whrcls +
              ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static DataSet get_DocTmpltsDet(long tmpltHdrID)
        {
            string strSql = "";

            strSql = @"SELECT doc_tmplt_det_id, line_item_type, line_description, 
incrs_dcrs, costing_accnt_id, auto_calc, code_behind_id
  FROM accb.accb_doc_tmplts_det a " +
              "WHERE((a.doc_tmplts_hdr_id = " + tmpltHdrID + ")) ORDER BY line_item_type ASC ";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_Total_DocTmpltsDet(long tmpltHdrID)
        {
            string strSql = "";
            strSql = @"SELECT count(1)
  FROM accb.accb_doc_tmplts_det a " +
         "WHERE((a.doc_tmplts_hdr_id = " + tmpltHdrID + "))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region "EXCHANGE RATES..."
        public static void createRate(string rate_dte, string curFrom,
          int curFrmID, string curTo, int curToID, double scalefactor)
        {
            rate_dte = DateTime.ParseExact(rate_dte, "dd-MMM-yyyy",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_exchange_rates(
            conversion_date, currency_from, currency_from_id, currency_to, 
            currency_to_id, multiply_from_by, created_by, creation_date, 
            last_update_by, last_update_date) " +
                  "VALUES ('" + rate_dte.Replace("'", "''") +
                  "', '" + curFrom.Replace("'", "''") +
                  "', " + curFrmID +
                  ", '" + curTo.Replace("'", "''") +
                  "', " + curToID +
                  ", " + scalefactor +
                  ", " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtRate(long rateID, string rate_dte, string curFrom,
          int curFrmID, string curTo, int curToID, double scalefactor)
        {
            rate_dte = DateTime.ParseExact(rate_dte, "dd-MMM-yyyy",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_exchange_rates SET 
            conversion_date='" + rate_dte.Replace("'", "''") +
                  "', currency_from='" + curFrom.Replace("'", "''") +
                  "', currency_from_id=" + curFrmID +
                  ", last_update_by=" + Global.myBscActn.user_id + ", last_update_date='" + dateStr +
                  "', currency_to='" + curTo.Replace("'", "''") +
                  "', currency_to_id=" + curToID +
                  ", multiply_from_by = " + scalefactor +
                  " WHERE rate_id = " + rateID;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void updtRateValue(long rateID, double scalefactor)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_exchange_rates SET 
            last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', multiply_from_by = " + scalefactor +
                  " WHERE rate_id = " + rateID;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void deleteRate(long valLnid, string rateDesc)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Rate Description = " + rateDesc;
            string delSQL = "DELETE FROM accb.accb_exchange_rates WHERE rate_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static bool doesRateExst(string rateDte, string fromCur, string toCur)
        {
            rateDte = DateTime.ParseExact(rateDte, "dd-MMM-yyyy",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string strSql = "";
            strSql = @"SELECT rate_id 
  FROM accb.accb_exchange_rates WHERE currency_from='" + fromCur.Replace("'", "''") +
                  "' and currency_to='" + toCur.Replace("'", "''") +
                  "' and conversion_date='" + rateDte.Replace("'", "''") +
                  "'";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static long doesRateExst1(string rateDte, string fromCur, string toCur)
        {
            rateDte = DateTime.ParseExact(rateDte, "dd-MMM-yyyy",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string strSql = "";
            strSql = @"SELECT rate_id 
  FROM accb.accb_exchange_rates WHERE currency_from='" + fromCur.Replace("'", "''") +
                  "' and currency_to='" + toCur.Replace("'", "''") +
                  "' and conversion_date='" + rateDte.Replace("'", "''") +
                  "'";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static DataSet get_Currencies(string funcCurCode)
        {
            string strSql = "";
            strSql = @"SELECT pssbl_value_id, pssbl_value, pssbl_value_desc,
       is_enabled, allowed_org_ids
  FROM gst.gen_stp_lov_values WHERE pssbl_value != '" +
         funcCurCode.Replace("'", "''") + "' and is_enabled='1' and value_list_id=" + Global.mnFrm.cmCde.getLovID("Currencies");

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_Rates(string searchWord, string searchIn,
          string dte1, string dte2, long offset,
          int limit_size)
        {
            string strSql = "";
            string whrcls = "";

            if (searchIn == "CURRENCY FROM")
            {
                whrcls = " AND (gst.get_pssbl_val_desc(a.currency_from_id) ilike '" + searchWord.Replace("'", "''") +
               "' or a.currency_from ilike '" + searchWord.Replace("'", "''") +
               "')";
            }
            else if (searchIn == "CURRENCY TO")
            {
                whrcls = " AND (gst.get_pssbl_val_desc(a.currency_to_id) ilike '" + searchWord.Replace("'", "''") +
               "' or a.currency_to ilike '" + searchWord.Replace("'", "''") +
               "')";
            }
            else if (searchIn == "MULTIPLY BY")
            {
                whrcls = " AND (trim(to_char(a.multiply_from_by, '9999999999999999999999999D9999S')) ilike '" + searchWord.Replace("'", "''") +
               "')";
            }

            strSql = @"SELECT rate_id, to_char(to_timestamp(conversion_date,'YYYY-MM-DD'),'DD-Mon-YYYY'), 
        currency_from, currency_from_id, gst.get_pssbl_val_desc(a.currency_from_id), 
        currency_to, currency_to_id, gst.get_pssbl_val_desc(a.currency_to_id), 
        multiply_from_by, conversion_date 
        FROM accb.accb_exchange_rates a " +
              "WHERE((to_timestamp(conversion_date,'YYYY-MM-DD') >= to_timestamp('" +
              dte1 + "' ,'YYYY-MM-DD HH24:MI:SS') AND to_timestamp(conversion_date,'YYYY-MM-DD') <=to_timestamp('" +
              dte2 + "' ,'YYYY-MM-DD HH24:MI:SS'))" + whrcls + ") ORDER BY conversion_date DESC, currency_from ASC LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.rates_SQL = strSql;
            return dtst;
        }

        public static long get_Total_Rates(string searchWord, string searchIn, string dte1, string dte2)
        {
            string strSql = "";
            string whrcls = "";

            if (searchIn == "CURRENCY FROM")
            {
                whrcls = " AND (gst.get_pssbl_val_desc(a.currency_from_id) ilike '" + searchWord.Replace("'", "''") +
               "' or a.currency_from ilike '" + searchWord.Replace("'", "''") +
               "')";
            }
            else if (searchIn == "CURRENCY TO")
            {
                whrcls = " AND (gst.get_pssbl_val_desc(a.currency_to_id) ilike '" + searchWord.Replace("'", "''") +
               "' or a.currency_to ilike '" + searchWord.Replace("'", "''") +
               "')";
            }
            else if (searchIn == "MULTIPLY BY")
            {
                whrcls = " AND (trim(to_char(a.multiply_from_by, '9999999999999999999999999D9999S')) ilike '" +
                  searchWord.Replace("'", "''") +
               "')";
            }

            strSql = @"SELECT count(1) FROM accb.accb_exchange_rates a " +
              "WHERE((to_timestamp(conversion_date,'YYYY-MM-DD HH24:MI:SS') >= to_timestamp('" +
              dte1 + "' ,'YYYY-MM-DD HH24:MI:SS') AND to_timestamp(conversion_date,'YYYY-MM-DD HH24:MI:SS') <=to_timestamp('" +
              dte2 + "' ,'YYYY-MM-DD HH24:MI:SS'))" + whrcls + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region "PAYABLES..."
        public static long getNewPyblsLnID()
        {
            //string strSql = "select nextval('accb.accb_trnsctn_batches_batch_id_seq'::regclass);";
            string strSql = "select nextval('accb.accb_pybls_amnt_smmrys_pybls_smmry_id_seq')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static string getLtstPyblsIDNoInPrfx(string prfxTxt)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "select count(pybls_invc_hdr_id) from accb.accb_pybls_invc_hdr WHERE org_id=" +
              Global.mnFrm.cmCde.Org_id + " and pybls_invc_number ilike '" + prfxTxt.Replace("'", "''") + "%'";
            dtSt = Global.mnFrm.cmCde.selectDataNoParams(sqlStr);
            if (dtSt.Tables[0].Rows.Count > 0)
            {
                return (long.Parse(dtSt.Tables[0].Rows[0][0].ToString()) + 1).ToString().PadLeft(4, '0');
            }
            else
            {
                return "0001";
            }
        }

        public static void createPyblsDocHdr(int orgid, string docDte, string docNum,
        string docType, string docDesc, long srcDocHdrID, int spplrID, int spplrSiteID,
          string apprvlStatus, string nxtApprvlActn, double invcAmnt, string pymntTrms,
          string srcDocType, int pymntMthdID, double amntPaid, long glBtchID,
          string spplrInvcNum, string docTmpltClsftn, int currID, double amntAppld,
          long rgstrID, string costCtgry, string evntType, string chequeNum)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            docDte = DateTime.ParseExact(docDte, "dd-MMM-yyyy",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string insSQL = @"INSERT INTO accb.accb_pybls_invc_hdr(
            pybls_invc_date, created_by, creation_date, 
            last_update_by, last_update_date, pybls_invc_number, pybls_invc_type, 
            comments_desc, src_doc_hdr_id, supplier_id, supplier_site_id, 
            approval_status, next_aproval_action, org_id, invoice_amount, 
            payment_terms, src_doc_type, pymny_method_id, amnt_paid, gl_batch_id, 
            spplrs_invc_num, doc_tmplt_clsfctn, invc_curr_id, invc_amnt_appld_elswhr,
            event_rgstr_id, evnt_cost_category, event_doc_type, firts_cheque_num) " +
                  "VALUES ('" + docDte.Replace("'", "''") +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', '" + docNum.Replace("'", "''") +
                  "', '" + docType.Replace("'", "''") +
                  "', '" + docDesc.Replace("'", "''") +
                  "', " + srcDocHdrID +
                  ", " + spplrID +
                  ", " + spplrSiteID +
                  ", '" + apprvlStatus.Replace("'", "''") +
                  "', '" + nxtApprvlActn.Replace("'", "''") +
                  "', " + orgid +
                  ", " + invcAmnt +
                  ", '" + pymntTrms.Replace("'", "''") +
                  "', '" + srcDocType.Replace("'", "''") +
                  "', " + pymntMthdID +
                  ", " + amntPaid +
                  ", " + glBtchID +
                  ", '" + spplrInvcNum.Replace("'", "''") +
                  "', '" + docTmpltClsftn.Replace("'", "''") +
                  "', " + currID + ", " + amntAppld + ", " + rgstrID +
                  ", '" + costCtgry.Replace("'", "''") +
                  "', '" + evntType.Replace("'", "''") + "', '" + chequeNum.Replace("'", "''") + "')";
            //Global.mnFrm.cmCde.showSQLNoPermsn(insSQL);
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtPyblsDocHdr(long hdrID, string docDte, string docNum,
        string docType, string docDesc, long srcDocHdrID, int spplrID, int spplrSiteID,
          string apprvlStatus, string nxtApprvlActn, double invcAmnt, string pymntTrms,
          string srcDocType, int pymntMthdID, double amntPaid, long glBtchID,
          string spplrInvcNum, string docTmpltClsftn, int currID, double amntAppld,
          long rgstrID, string costCtgry, string evntType, string chequeNum)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            docDte = DateTime.ParseExact(docDte, "dd-MMM-yyyy",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_pybls_invc_hdr
       SET pybls_invc_date='" + docDte.Replace("'", "''") +
                  "', last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', pybls_invc_number='" + docNum.Replace("'", "''") +
                  "', pybls_invc_type='" + docType.Replace("'", "''") +
                  "', comments_desc='" + docDesc.Replace("'", "''") +
                  "', src_doc_hdr_id=" + srcDocHdrID +
                  ", supplier_id=" + spplrID +
                  ", supplier_site_id=" + spplrSiteID +
                  ", approval_status='" + apprvlStatus.Replace("'", "''") +
                  "', next_aproval_action='" + nxtApprvlActn.Replace("'", "''") +
                  "', invoice_amount=" + invcAmnt +
                  ", payment_terms='" + pymntTrms.Replace("'", "''") +
                  "', src_doc_type='" + srcDocType.Replace("'", "''") +
                  "', pymny_method_id=" + pymntMthdID +
                  ", amnt_paid=" + amntPaid +
                  ", gl_batch_id=" + glBtchID +
                  ", spplrs_invc_num='" + spplrInvcNum.Replace("'", "''") +
                  "', doc_tmplt_clsfctn='" + docTmpltClsftn.Replace("'", "''") +
                  "', invc_curr_id=" + currID +
                  ", invc_amnt_appld_elswhr=" + amntAppld +
                     ", event_rgstr_id=" + rgstrID +
                  ", evnt_cost_category='" + costCtgry.Replace("'", "''") +
                  "', event_doc_type='" + evntType.Replace("'", "''") +
                  "', firts_cheque_num='" + chequeNum.Replace("'", "''") +
               "' WHERE pybls_invc_hdr_id = " + hdrID;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void createPyblsDocDet(long smmryID, long hdrID, string lineType, string lineDesc,
          double entrdAmnt, int entrdCurrID, int codeBhnd, string docType,
          bool autoCalc, string incrDcrs1, int costngID, string incrDcrs2, int blncgAccntID,
          long prepayDocHdrID, string vldyStatus, long orgnlLnID,
          int funcCurrID, int accntCurrID, double funcCurrRate, double accntCurrRate,
          double funcCurrAmnt, double accntCurrAmnt, long initAmntID)
        {

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_pybls_amnt_smmrys(
            pybls_smmry_id, pybls_smmry_type, pybls_smmry_desc, pybls_smmry_amnt, 
            code_id_behind, src_pybls_type, src_pybls_hdr_id, created_by, 
            creation_date, last_update_by, last_update_date, auto_calc, incrs_dcrs1, 
            asset_expns_acnt_id, incrs_dcrs2, liability_acnt_id, appld_prepymnt_doc_id, 
            validty_status, orgnl_line_id, entrd_curr_id, 
            func_curr_id, accnt_curr_id, func_curr_rate, accnt_curr_rate, 
            func_curr_amount, accnt_curr_amnt, initial_amnt_line_id) " +
                  "VALUES (" + smmryID + ", '" + lineType.Replace("'", "''") +
                  "', '" + lineDesc.Replace("'", "''") +
                  "', " + entrdAmnt +
                  ", " + codeBhnd +
                  ", '" + docType.Replace("'", "''") +
                  "', " + hdrID +
                  ", " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(autoCalc) +
                  "', '" + incrDcrs1.Replace("'", "''") +
                  "', " + costngID +
                  ", '" + incrDcrs2.Replace("'", "''") +
                  "', " + blncgAccntID +
                  ", " + prepayDocHdrID +
                  ", '" + vldyStatus.Replace("'", "''") +
                  "', " + orgnlLnID +
                  ", " + entrdCurrID +
                  ", " + funcCurrID +
                  ", " + accntCurrID +
                  ", " + funcCurrRate +
                  ", " + accntCurrRate +
                  ", " + funcCurrAmnt +
                  ", " + accntCurrAmnt +
                  ", " + initAmntID +
                  ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtPyblsDocDet(long docDetID, long hdrID, string lineType, string lineDesc,
          double entrdAmnt, int entrdCurrID, int codeBhnd, string docType,
          bool autoCalc, string incrDcrs1, int costngID, string incrDcrs2, int blncgAccntID,
          long prepayDocHdrID, string vldyStatus, long orgnlLnID,
          int funcCurrID, int accntCurrID, double funcCurrRate, double accntCurrRate,
          double funcCurrAmnt, double accntCurrAmnt, long initAmntID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_pybls_amnt_smmrys
   SET pybls_smmry_type='" + lineType.Replace("'", "''") +
                  "', pybls_smmry_desc='" + lineDesc.Replace("'", "''") +
                  "', pybls_smmry_amnt=" + entrdAmnt +
                  ", code_id_behind=" + codeBhnd +
                  ", src_pybls_type='" + docType.Replace("'", "''") +
                  "', src_pybls_hdr_id=" + hdrID +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', auto_calc='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(autoCalc) +
                  "', incrs_dcrs1='" + incrDcrs1.Replace("'", "''") +
                  "', asset_expns_acnt_id=" + costngID +
                  ", incrs_dcrs2='" + incrDcrs2.Replace("'", "''") +
                  "', liability_acnt_id=" + blncgAccntID +
                  ", appld_prepymnt_doc_id=" + prepayDocHdrID +
                  ", validty_status='" + vldyStatus.Replace("'", "''") +
                  "', orgnl_line_id=" + orgnlLnID +
                  ", entrd_curr_id=" + entrdCurrID +
                  ", func_curr_id=" + funcCurrID +
                  ", accnt_curr_id=" + accntCurrID +
                  ", func_curr_rate=" + funcCurrRate +
                  ", accnt_curr_rate=" + accntCurrRate +
                  ", func_curr_amount=" + funcCurrAmnt +
                  ", accnt_curr_amnt=" + accntCurrAmnt +
                  ", initial_amnt_line_id=" + initAmntID +
                  " WHERE pybls_smmry_id = " + docDetID;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void deletePyblsDocHdrNDet(long valLnid, string docNum)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Document Number = " + docNum;
            string delSQL = "DELETE FROM accb.accb_pybls_amnt_smmrys WHERE src_pybls_hdr_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
            delSQL = "DELETE FROM accb.accb_pybls_invc_hdr WHERE pybls_invc_hdr_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deletePyblsDocDet(long valLnid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSQL = "DELETE FROM accb.accb_pybls_amnt_smmrys WHERE pybls_smmry_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static DataSet get_One_PyblsDocHdr(long hdrID)
        {
            string strSql = "";

            strSql = @"SELECT pybls_invc_hdr_id, to_char(to_timestamp(pybls_invc_date,'YYYY-MM-DD'),'DD-Mon-YYYY'), 
       created_by, sec.get_usr_name(a.created_by), pybls_invc_number, pybls_invc_type, 
       comments_desc, src_doc_hdr_id, supplier_id, scm.get_cstmr_splr_name(a.supplier_id),
       supplier_site_id, scm.get_cstmr_splr_site_name(a.supplier_site_id), 
       approval_status, next_aproval_action, invoice_amount, 
       payment_terms, src_doc_type, pymny_method_id, accb.get_pymnt_mthd_name(a.pymny_method_id), 
       amnt_paid, gl_batch_id, accb.get_gl_batch_name(a.gl_batch_id),
       spplrs_invc_num, doc_tmplt_clsfctn, invc_curr_id, gst.get_pssbl_val(a.invc_curr_id),
        event_rgstr_id, evnt_cost_category, event_doc_type, next_part_payment, firts_cheque_num   
  FROM accb.accb_pybls_invc_hdr a " +
              "WHERE((a.pybls_invc_hdr_id = " + hdrID + "))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.pyblsFrm.docTmplt_SQL = strSql;
            return dtst;
        }

        public static double getCodeAmnt(int codeID, double grndAmnt)
        {
            string codeSQL = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes",
              "code_id", "sql_formular", codeID);
            codeSQL = codeSQL.Replace("{:qty}", "1").Replace("{:unit_price}", grndAmnt.ToString());
            if (codeSQL != "")
            {
                DataSet d1 = Global.mnFrm.cmCde.selectDataNoParams(codeSQL);
                double rs1 = 0;

                if (d1.Tables[0].Rows.Count > 0)
                {
                    double.TryParse(d1.Tables[0].Rows[0][0].ToString(), out rs1);
                }
                return rs1;
            }
            else
            {
                return 0.00;
            }
        }

        public static DataSet get_PyblsDocHdr(string searchWord, string searchIn, long offset,
          int limit_size, long orgID, bool shwUnpstdOnly)
        {
            string strSql = "";
            string whrcls = "";
            /*Document Number
         Document Description
         Document Classification
         Supplier Name
         Supplier's Invoice Number
         Source Doc Number
         Approval Status
         Created By*/
            string unpstdCls = "";
            if (shwUnpstdOnly)
            {
                unpstdCls = " AND (round(a.invoice_amount-a.amnt_paid,2)>0 or a.approval_status IN ('Not Validated','Validated','Reviewed'))";
                // AND (a.approval_status='Approved')
                //        unpstdCls = @" AND EXISTS (SELECT f.src_pybls_hdr_id 
                //FROM accb.accb_pybls_amnt_smmrys f WHERE f.pybls_smmry_type='8Outstanding Balance' 
                //and round(f.pybls_smmry_amnt,2)>0 and a.pybls_invc_hdr_id=f.src_pybls_hdr_id and f.src_pybls_type=a.pybls_invc_type)";
                //        //unpstdCls = " AND (a.approval_status!='Approved')";
            }
            if (searchIn == "Document Number")
            {
                whrcls = " and (a.pybls_invc_number ilike '" + searchWord.Replace("'", "''") +
                  "' or trim(to_char(a.pybls_invc_hdr_id, '99999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            else if (searchIn == "Document Description")
            {
                whrcls = " and (a.comments_desc ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Document Classification")
            {
                whrcls = " and (a.doc_tmplt_clsfctn ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Supplier Name")
            {
                whrcls = @" and (a.supplier_id IN (select c.cust_sup_id from 
scm.scm_cstmr_suplr c where c.cust_sup_name ilike '" + searchWord.Replace("'", "''") +
            "'))";
            }
            else if (searchIn == "Supplier's Invoice Number")
            {
                whrcls = " and (a.spplrs_invc_num ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Source Doc Number")
            {
                whrcls = @" and (trim(to_char(a.src_doc_hdr_id, '9999999999999999999999999')) 
IN (select trim(to_char(d.rcpt_id, '9999999999999999999999999')) from inv.inv_consgmt_rcpt_hdr d 
where trim(to_char(d.rcpt_id, '9999999999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
            @"') or trim(to_char(a.src_doc_hdr_id, '9999999999999999999999999')) 
IN (select trim(to_char(e.rcpt_rtns_id, '9999999999999999999999999')) from inv.inv_consgmt_rcpt_rtns_hdr e 
where trim(to_char(e.rcpt_rtns_id, '9999999999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
            @"') or a.src_doc_hdr_id IN (select f.pybls_invc_hdr_id from accb.accb_pybls_invc_hdr f
where f.pybls_invc_number ilike '" + searchWord.Replace("'", "''") +
            @"'))";
            }
            else if (searchIn == "Approval Status")
            {
                whrcls = " and (a.approval_status ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Created By")
            {
                whrcls = " and (sec.get_usr_name(a.created_by) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Currency")
            {
                whrcls = " and (gst.get_pssbl_val(a.invc_curr_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            strSql = @"SELECT pybls_invc_hdr_id, pybls_invc_number, pybls_invc_type
, round(a.invoice_amount-a.amnt_paid,2),
 a.approval_status 
        FROM accb.accb_pybls_invc_hdr a 
        WHERE((a.org_id = " + orgID + ")" + whrcls + unpstdCls +
              ") ORDER BY pybls_invc_hdr_id DESC LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.pyblsFrm.rec_SQL = strSql;
            return dtst;
        }

        public static long get_Total_PyblsDoc(string searchWord, string searchIn, long orgID, bool shwUnpstdOnly)
        {
            string strSql = "";
            string whrcls = "";
            /*Document Number
         Document Description
         Document Classification
         Supplier Name
         Supplier's Invoice Number
         Source Doc Number
         Approval Status
         Created By*/
            string unpstdCls = "";
            if (shwUnpstdOnly)
            {
                // AND (a.approval_status='Approved')
                //        unpstdCls = @" AND EXISTS (SELECT f.src_pybls_hdr_id 
                //FROM accb.accb_pybls_amnt_smmrys f WHERE f.pybls_smmry_type='8Outstanding Balance' 
                //and round(f.pybls_smmry_amnt,2)>0 and a.pybls_invc_hdr_id=f.src_pybls_hdr_id and f.src_pybls_type=a.pybls_invc_type)";
                unpstdCls = " AND (round(a.invoice_amount-a.amnt_paid,2)>0 or a.approval_status IN ('Not Validated','Validated','Reviewed'))";
                //unpstdCls = " AND (a.approval_status!='Approved')";
            }
            if (searchIn == "Document Number")
            {
                whrcls = " and (a.pybls_invc_number ilike '" + searchWord.Replace("'", "''") + "' or trim(to_char(a.pybls_invc_hdr_id, '99999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            else if (searchIn == "Document Description")
            {
                whrcls = " and (a.comments_desc ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Document Classification")
            {
                whrcls = " and (a.doc_tmplt_clsfctn ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Supplier Name")
            {
                whrcls = @" and (a.supplier_id IN (select c.cust_sup_id from 
scm.scm_cstmr_suplr c where c.cust_sup_name ilike '" + searchWord.Replace("'", "''") +
            "'))";
            }
            else if (searchIn == "Supplier's Invoice Number")
            {
                whrcls = " and (a.spplrs_invc_num ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Source Doc Number")
            {
                whrcls = @" and (trim(to_char(a.src_doc_hdr_id, '9999999999999999999999999')) 
IN (select trim(to_char(d.rcpt_id, '9999999999999999999999999')) from inv.inv_consgmt_rcpt_hdr d 
where trim(to_char(d.rcpt_id, '9999999999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
            @"') or trim(to_char(a.src_doc_hdr_id, '9999999999999999999999999')) 
IN (select trim(to_char(e.rcpt_rtns_id, '9999999999999999999999999')) from inv.inv_consgmt_rcpt_rtns_hdr e 
where trim(to_char(e.rcpt_rtns_id, '9999999999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
            @"') or a.src_doc_hdr_id IN (select f.pybls_invc_hdr_id from accb.accb_pybls_invc_hdr f
where f.pybls_invc_number ilike '" + searchWord.Replace("'", "''") +
            @"'))";
            }
            else if (searchIn == "Approval Status")
            {
                whrcls = " and (a.approval_status ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Created By")
            {
                whrcls = " and (sec.get_usr_name(a.created_by) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Currency")
            {
                whrcls = " and (gst.get_pssbl_val(a.invc_curr_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            strSql = @"SELECT count(1) FROM accb.accb_pybls_invc_hdr a  
        WHERE((a.org_id = " + orgID + ")" + whrcls + unpstdCls +
              ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static DataSet get_PyblsDocDet(long docHdrID)
        {
            string strSql = "";
            string whrcls = @" and (a.pybls_smmry_type !='6Grand Total' and 
a.pybls_smmry_type !='7Total Payments Made' and a.pybls_smmry_type !='8Outstanding Balance')";
            //if (aprvlStatus != "Not Validated")
            //{
            //  //whrcls = "";, string aprvlStatus
            //}
            strSql = @"SELECT pybls_smmry_id, pybls_smmry_type, pybls_smmry_desc, pybls_smmry_amnt, 
       code_id_behind, auto_calc, incrs_dcrs1, 
       asset_expns_acnt_id, incrs_dcrs2, liability_acnt_id, appld_prepymnt_doc_id, 
       entrd_curr_id, gst.get_pssbl_val(a.entrd_curr_id), 
       func_curr_id, gst.get_pssbl_val(a.func_curr_id), 
      accnt_curr_id, gst.get_pssbl_val(a.accnt_curr_id), 
      func_curr_rate, accnt_curr_rate, 
       func_curr_amount, accnt_curr_amnt, initial_amnt_line_id, REPLACE(REPLACE(a.pybls_smmry_type,'2Tax','3Tax'),'3Discount','2Discount') smtyp 
  FROM accb.accb_pybls_amnt_smmrys a " +
              "WHERE((a.src_pybls_hdr_id = " + docHdrID + ")" + whrcls + ") ORDER BY 23 ASC ";

            //MessageBox.Show(strSql);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.pyblsFrm.recDt_SQL = strSql;
            return dtst;
        }

        public static DataSet get_RcvblDocSmryLns(long dochdrID, string docTyp)
        {
            string strSql = "SELECT a.rcvbl_smmry_id, a.rcvbl_smmry_desc, " +
             "CASE WHEN substr(a.rcvbl_smmry_type,1,1) IN ('3','5') THEN -1 * a.rcvbl_smmry_amnt ELSE a.rcvbl_smmry_amnt END, " +
             "a.code_id_behind, a.rcvbl_smmry_type, a.auto_calc, REPLACE(REPLACE(a.rcvbl_smmry_type,'2Tax','3Tax'),'3Discount','2Discount') smtyp  " +
             "FROM accb.accb_rcvbl_amnt_smmrys a " +
             "WHERE((a.src_rcvbl_hdr_id = " + dochdrID +
             ") and (a.src_rcvbl_type='" + docTyp + "') and (substr(a.rcvbl_smmry_type,1,1) NOT IN ('6','7','8'))) ORDER BY 7";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);

            return dtst;
        }

        public static DataSet get_RcvblDocEndLns(long dochdrID, string docTyp)
        {
            string strSql = "SELECT a.rcvbl_smmry_id, a.rcvbl_smmry_desc, " +
             "a.rcvbl_smmry_amnt, a.code_id_behind, substr(a.rcvbl_smmry_type,2), a.auto_calc " +
             "FROM accb.accb_rcvbl_amnt_smmrys a " +
             "WHERE((a.src_rcvbl_hdr_id = " + dochdrID +
             ") and (a.src_rcvbl_type='" + docTyp + "') and (substr(a.rcvbl_smmry_type,1,1) IN ('6','7','8'))) ORDER BY a.rcvbl_smmry_type";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);

            return dtst;
        }
        public static DataSet get_PyblsDocSmryLns(long dochdrID, string docTyp)
        {
            string strSql = "SELECT a.pybls_smmry_id, a.pybls_smmry_desc, " +
             "CASE WHEN substr(a.pybls_smmry_type,1,1) IN ('3','5') THEN -1 * a.pybls_smmry_amnt ELSE a.pybls_smmry_amnt END, " +
             "a.code_id_behind, a.pybls_smmry_type, a.auto_calc, REPLACE(REPLACE(a.pybls_smmry_type,'2Tax','3Tax'),'3Discount','2Discount') smtyp " +
             "FROM accb.accb_pybls_amnt_smmrys a " +
             "WHERE((a.src_pybls_hdr_id = " + dochdrID +
             ") and (a.src_pybls_type='" + docTyp +
             "') and (substr(a.pybls_smmry_type,1,1) NOT IN ('6','7','8'))) ORDER BY 7";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);

            return dtst;
        }

        public static string getDocSgntryCols(string doctype)
        {
            string selSQL = @"select a.pssbl_value_desc from gst.gen_stp_lov_values a, gst.gen_stp_lov_names b
WHERE a.value_list_id = b.value_list_id and a.pssbl_value = '" + doctype.Replace("'", "''") + @"' 
and b.value_list_name = 'Document Signatory Columns'
and a.is_enabled='1' ORDER BY a.pssbl_value_id LIMIT 1 OFFSET 0";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(selSQL);
            if (dtst.Tables.Count <= 0)
            {
                return "";
            }
            else if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            return "";
        }

        public static DataSet get_PyblsDocEndLns(long dochdrID, string docTyp)
        {
            string strSql = @"SELECT * FROM (SELECT a.pybls_smmry_id, a.pybls_smmry_desc, 
             a.pybls_smmry_amnt, a.code_id_behind, substr(a.pybls_smmry_type,2), a.auto_calc, a.pybls_smmry_type
                          FROM accb.accb_pybls_amnt_smmrys a 
             WHERE((a.src_pybls_hdr_id = " + dochdrID +
             ") and (a.src_pybls_type='" + docTyp + @"') and (substr(a.pybls_smmry_type,1,1) IN ('6','7','8'))) 
UNION
SELECT 9999999, 'Amount Being Paid',next_part_payment, -1, 'Amount Being Paid','0', '9Amount Being Paid' 
FROM accb.accb_pybls_invc_hdr  a
WHERE((a.pybls_invc_hdr_id = " + dochdrID +
             ") and (a.pybls_invc_type='" + docTyp + @"'))) tbl1
             ORDER BY 7";

            /* "SELECT a.pybls_smmry_id, a.pybls_smmry_desc, " +
          "a.pybls_smmry_amnt, a.code_id_behind, substr(a.pybls_smmry_type,2), a.auto_calc " +
          "FROM accb.accb_pybls_amnt_smmrys a " +
          "WHERE((a.src_pybls_hdr_id = " + dochdrID +
          ") and (a.src_pybls_type='" + docTyp + "') and (substr(a.pybls_smmry_type,1,1) IN ('6','7','8'))) ORDER BY a.pybls_smmry_type";*/
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);

            return dtst;
        }

        public static void updateNextPayment(long dochdrID, decimal amount)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Update Next Part Payment with " + amount;
            string updtSQL = @"UPDATE accb.accb_pybls_invc_hdr SET next_part_payment=" + amount +
                "WHERE(pybls_invc_hdr_id = " + dochdrID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }
        public static string getLtstRecPkID(string tblNm, string pkeyCol)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "select " + pkeyCol + " from " + tblNm + " ORDER BY 1 DESC LIMIT 1 OFFSET 0";
            dtSt = Global.mnFrm.cmCde.selectDataNoParams(sqlStr);
            if (dtSt.Tables[0].Rows.Count > 0)
            {
                long num = long.Parse(dtSt.Tables[0].Rows[0][0].ToString()) + 1;
                if (num.ToString().Length < 4)
                {
                    return num.ToString().PadLeft(4, '0');
                }
                else
                {
                    return num.ToString();
                }
            }
            else
            {
                return "0001";
            }
        }

        public static double getPyblsDocGrndAmnt(long dochdrID)
        {
            string strSql = @"select SUM(CASE WHEN y.pybls_smmry_type='3Discount' 
or scm.istaxwthhldng(y.code_id_behind)='1' or y.pybls_smmry_type='5Applied Prepayment'
      THEN -1*y.pybls_smmry_amnt ELSE y.pybls_smmry_amnt END) amnt " +
              "from accb.accb_pybls_amnt_smmrys y " +
              "where y.src_pybls_hdr_id=" + dochdrID +
              " and y.pybls_smmry_type IN ('1Initial Amount','2Tax','3Discount','4Extra Charge','5Applied Prepayment')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double rs = 0;

            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
            }
            return rs;
        }

        public static double getPyblsDocFuncAmnt(long dochdrID)
        {
            string strSql = @"select SUM(CASE WHEN y.pybls_smmry_type='3Discount' 
or scm.istaxwthhldng(y.code_id_behind)='1' or y.pybls_smmry_type='5Applied Prepayment'
      THEN -1*y.func_curr_amount ELSE y.func_curr_amount END) amnt " +
              "from accb.accb_pybls_amnt_smmrys y " +
              "where y.src_pybls_hdr_id=" + dochdrID +
              " and y.pybls_smmry_type IN ('1Initial Amount','2Tax','3Discount','4Extra Charge','5Applied Prepayment')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double rs = 0;

            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
            }
            return rs;
        }

        public static double getPyblsDocAccntAmnt(long dochdrID)
        {
            string strSql = @"select SUM(CASE WHEN y.pybls_smmry_type='3Discount' 
or scm.istaxwthhldng(y.code_id_behind)='1' or y.pybls_smmry_type='5Applied Prepayment'
      THEN -1*y.accnt_curr_amnt ELSE y.accnt_curr_amnt END) amnt " +
              "from accb.accb_pybls_amnt_smmrys y " +
              "where y.src_pybls_hdr_id=" + dochdrID +
              " and y.pybls_smmry_type IN ('1Initial Amount','2Tax','3Discount','4Extra Charge','5Applied Prepayment')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double rs = 0;

            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
            }
            return rs;
        }

        public static long getPyblsSmmryItmID(string smmryType, int codeBhnd,
          long srcDocID, string srcDocTyp, string smmryNm)
        {
            string strSql = "select y.pybls_smmry_id " +
              "from accb.accb_pybls_amnt_smmrys y " +
              "where y.pybls_smmry_type= '" + smmryType + "' and y.pybls_smmry_desc = '" + smmryNm +
              "' and y.code_id_behind= " + codeBhnd +
              " and y.src_pybls_type='" + srcDocTyp.Replace("'", "''") +
              "' and y.src_pybls_hdr_id=" + srcDocID + " ";
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

        public static void updtPyblsDocApprvl(long docid,
      string apprvlSts, string nxtApprvl)
        {
            string extrCls = "";
            if (apprvlSts == "Cancelled")
            {
                extrCls = ", invoice_amount=0, invc_amnt_appld_elswhr=0";
            }
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_pybls_invc_hdr SET " +
                  "approval_status='" + apprvlSts.Replace("'", "''") +
                  "', last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', next_aproval_action='" + nxtApprvl.Replace("'", "''") +
                  "'" + extrCls + " WHERE (pybls_invc_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtPyblsDocAmnt(long docid, double invAmnt)
        {
            string extrCls = ", invoice_amount=" + invAmnt + "";

            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_pybls_invc_hdr SET " +
                  "last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "'" + extrCls + " WHERE (pybls_invc_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtPyblsDocGLBatch(long docid,
      long glBatchID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_pybls_invc_hdr SET " +
                  "gl_batch_id=" + glBatchID +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (pybls_invc_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtPyblsDocAmntPaid(long docid,
      double amntPaid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_pybls_invc_hdr SET " +
                  "amnt_paid=amnt_paid + " + amntPaid +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (pybls_invc_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtPyblsDocAmntAppld(long docid,
      double amntAppld)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_pybls_invc_hdr SET " +
                  "invc_amnt_appld_elswhr=invc_amnt_appld_elswhr + " + amntAppld +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (pybls_invc_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static double getPyblsDocTtlPymnts(long dochdrID, string docType)
        {
            string strSql = "select SUM(y.amount_paid) amnt " +
              "from accb.accb_payments y " +
              "where y.src_doc_id = " + dochdrID + " and y.src_doc_typ = '" + docType.Replace("'", "''") + "'";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double rs = 0;

            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
            }
            return rs;
        }

        public static int getPyblsDocBlncngAccnt(long srcDocID, string docType)
        {
            string whrcls = @" and (a.pybls_smmry_type !='6Grand Total' and 
a.pybls_smmry_type !='7Total Payments Made' and a.pybls_smmry_type !='8Outstanding Balance')";

            string selSQL = @"select 
        distinct liability_acnt_id, pybls_smmry_id 
        from accb.accb_pybls_amnt_smmrys a 
        where src_pybls_hdr_id = " + srcDocID +
              " and src_pybls_type = '" + docType.Replace("'", "''") +
              "'" + whrcls + " order by pybls_smmry_id LIMIT 1 OFFSET 0";
            //Global.mnFrm.cmCde.showSQLNoPermsn(selSQL);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(selSQL);

            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }
        public static int getPyblsPrepayDocCnt(long dochdrID)
        {
            string strSql = @"select count(appld_prepymnt_doc_id) " +
              "from accb.accb_pybls_amnt_smmrys y " +
              "where y.src_pybls_hdr_id = " + dochdrID + " and y.appld_prepymnt_doc_id >0 " +
              "Group by y.appld_prepymnt_doc_id having count(y.appld_prepymnt_doc_id)>1";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            int rs = 0;

            if (dtst.Tables[0].Rows.Count > 0)
            {
                int.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
                return rs;
            }
            return 0;
        }
        public static DataSet get_Pybls_Attachments(string searchWord, string searchIn,
      Int64 offset, int limit_size, long batchID, ref string attchSQL)
        {
            string strSql = "";
            if (searchIn == "Attachment Name/Description")
            {
                strSql = "SELECT a.attchmnt_id, a.doc_hdr_id, a.attchmnt_desc, a.file_name " +
              "FROM accb.accb_pybl_doc_attchmnts a " +
              "WHERE(a.attchmnt_desc ilike '" + searchWord.Replace("'", "''") +
              "' and a.doc_hdr_id = " + batchID + ") ORDER BY a.attchmnt_id LIMIT " + limit_size +
                  " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            attchSQL = strSql;
            return dtst;
        }

        public static long get_Total_Pybls_Attachments(string searchWord,
          string searchIn, long batchID)
        {
            string strSql = "";
            if (searchIn == "Attachment Name/Description")
            {
                strSql = "SELECT COUNT(1) " +
              "FROM accb.accb_pybl_doc_attchmnts a " +
              "WHERE(a.attchmnt_desc ilike '" + searchWord.Replace("'", "''") +
              "' and a.doc_hdr_id = " + batchID + ")";
            }
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            long sumRes = 0;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                long.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        #endregion

        #region "PETTY CASH..."
        public static long getNewPtycshLnID()
        {
            //string strSql = "select nextval('accb.accb_trnsctn_batches_batch_id_seq'::regclass);";
            string strSql = "select nextval('accb.accb_ptycsh_amnt_smmrys_ptycsh_smmry_id_seq')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static string getLtstPtycshIDNoInPrfx(string prfxTxt)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "select count(ptycsh_vchr_hdr_id) from accb.accb_ptycsh_vchr_hdr WHERE org_id=" +
              Global.mnFrm.cmCde.Org_id + " and ptycsh_vchr_number ilike '" + prfxTxt.Replace("'", "''") + "%'";
            dtSt = Global.mnFrm.cmCde.selectDataNoParams(sqlStr);
            if (dtSt.Tables[0].Rows.Count > 0)
            {
                return (long.Parse(dtSt.Tables[0].Rows[0][0].ToString()) + 1).ToString().PadLeft(4, '0');
            }
            else
            {
                return "0001";
            }
        }

        public static void createPtycshDocHdr(int orgid, string docDte, string docNum,
        string docType, string docDesc, long srcDocHdrID, int spplrID, int spplrSiteID,
          string apprvlStatus, string nxtApprvlActn, double invcAmnt, string pymntTrms,
          string srcDocType, int pymntMthdID, double amntPaid, long glBtchID,
          string spplrInvcNum, string docTmpltClsftn, int currID, double amntAppld,
          long rgstrID, string costCtgry, string evntType, int blcngAccntID)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            docDte = DateTime.ParseExact(docDte, "dd-MMM-yyyy",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string insSQL = @"INSERT INTO accb.accb_ptycsh_vchr_hdr(
            ptycsh_vchr_date, created_by, creation_date, 
            last_update_by, last_update_date, ptycsh_vchr_number, ptycsh_vchr_type, 
            comments_desc, src_doc_hdr_id, supplier_id, supplier_site_id, 
            approval_status, next_aproval_action, org_id, invoice_amount, 
            payment_terms, src_doc_type, pymny_method_id, amnt_paid, gl_batch_id, 
            spplrs_invc_num, doc_tmplt_clsfctn, invc_curr_id, invc_amnt_appld_elswhr,
            event_rgstr_id, evnt_cost_category, event_doc_type, balancing_accnt_id) " +
                  "VALUES ('" + docDte.Replace("'", "''") +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', '" + docNum.Replace("'", "''") +
                  "', '" + docType.Replace("'", "''") +
                  "', '" + docDesc.Replace("'", "''") +
                  "', " + srcDocHdrID +
                  ", " + spplrID +
                  ", " + spplrSiteID +
                  ", '" + apprvlStatus.Replace("'", "''") +
                  "', '" + nxtApprvlActn.Replace("'", "''") +
                  "', " + orgid +
                  ", " + invcAmnt +
                  ", '" + pymntTrms.Replace("'", "''") +
                  "', '" + srcDocType.Replace("'", "''") +
                  "', " + pymntMthdID +
                  ", " + amntPaid +
                  ", " + glBtchID +
                  ", '" + spplrInvcNum.Replace("'", "''") +
                  "', '" + docTmpltClsftn.Replace("'", "''") +
                  "', " + currID + ", " + amntAppld + ", " + rgstrID +
                  ", '" + costCtgry.Replace("'", "''") + "', '" + evntType.Replace("'", "''") + "', " + blcngAccntID + ")";
            //Global.mnFrm.cmCde.showSQLNoPermsn(insSQL);
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtPtycshDocHdr(long hdrID, string docDte, string docNum,
        string docType, string docDesc, long srcDocHdrID, int spplrID, int spplrSiteID,
          string apprvlStatus, string nxtApprvlActn, double invcAmnt, string pymntTrms,
          string srcDocType, int pymntMthdID, double amntPaid, long glBtchID,
          string spplrInvcNum, string docTmpltClsftn, int currID, double amntAppld,
          long rgstrID, string costCtgry, string evntType, int blcngAccntID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            docDte = DateTime.ParseExact(docDte, "dd-MMM-yyyy",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_ptycsh_vchr_hdr
       SET ptycsh_vchr_date='" + docDte.Replace("'", "''") +
                  "', last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', ptycsh_vchr_number='" + docNum.Replace("'", "''") +
                  "', ptycsh_vchr_type='" + docType.Replace("'", "''") +
                  "', comments_desc='" + docDesc.Replace("'", "''") +
                  "', src_doc_hdr_id=" + srcDocHdrID +
                  ", supplier_id=" + spplrID +
                  ", supplier_site_id=" + spplrSiteID +
                  ", approval_status='" + apprvlStatus.Replace("'", "''") +
                  "', next_aproval_action='" + nxtApprvlActn.Replace("'", "''") +
                  "', invoice_amount=" + invcAmnt +
                  ", payment_terms='" + pymntTrms.Replace("'", "''") +
                  "', src_doc_type='" + srcDocType.Replace("'", "''") +
                  "', pymny_method_id=" + pymntMthdID +
                  ", amnt_paid=" + amntPaid +
                  ", gl_batch_id=" + glBtchID +
                  ", spplrs_invc_num='" + spplrInvcNum.Replace("'", "''") +
                  "', doc_tmplt_clsfctn='" + docTmpltClsftn.Replace("'", "''") +
                  "', invc_curr_id=" + currID +
                  ", invc_amnt_appld_elswhr=" + amntAppld +
                     ", event_rgstr_id=" + rgstrID +
                  ", evnt_cost_category='" + costCtgry.Replace("'", "''") +
                  "', event_doc_type='" + evntType.Replace("'", "''") +
               "', balancing_accnt_id=" + blcngAccntID +
               " WHERE ptycsh_vchr_hdr_id = " + hdrID;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void createPtycshDocDet(long smmryID, long hdrID, string lineType, string lineDesc,
          double entrdAmnt, int entrdCurrID, int codeBhnd, string docType,
          bool autoCalc, string incrDcrs1, int costngID, string incrDcrs2, int blncgAccntID,
          long prepayDocHdrID, string vldyStatus, long orgnlLnID,
          int funcCurrID, int accntCurrID, double funcCurrRate, double accntCurrRate,
          double funcCurrAmnt, double accntCurrAmnt, long initAmntID)
        {

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_ptycsh_amnt_smmrys(
            ptycsh_smmry_id, ptycsh_smmry_type, ptycsh_smmry_desc, ptycsh_smmry_amnt, 
            code_id_behind, src_ptycsh_type, src_ptycsh_hdr_id, created_by, 
            creation_date, last_update_by, last_update_date, auto_calc, incrs_dcrs1, 
            asset_expns_acnt_id, incrs_dcrs2, liability_acnt_id, appld_prepymnt_doc_id, 
            validty_status, orgnl_line_id, entrd_curr_id, 
            func_curr_id, accnt_curr_id, func_curr_rate, accnt_curr_rate, 
            func_curr_amount, accnt_curr_amnt, initial_amnt_line_id) " +
                  "VALUES (" + smmryID + ", '" + lineType.Replace("'", "''") +
                  "', '" + lineDesc.Replace("'", "''") +
                  "', " + entrdAmnt +
                  ", " + codeBhnd +
                  ", '" + docType.Replace("'", "''") +
                  "', " + hdrID +
                  ", " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(autoCalc) +
                  "', '" + incrDcrs1.Replace("'", "''") +
                  "', " + costngID +
                  ", '" + incrDcrs2.Replace("'", "''") +
                  "', " + blncgAccntID +
                  ", " + prepayDocHdrID +
                  ", '" + vldyStatus.Replace("'", "''") +
                  "', " + orgnlLnID +
                  ", " + entrdCurrID +
                  ", " + funcCurrID +
                  ", " + accntCurrID +
                  ", " + funcCurrRate +
                  ", " + accntCurrRate +
                  ", " + funcCurrAmnt +
                  ", " + accntCurrAmnt +
                  ", " + initAmntID +
                  ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtPtycshDocDet(long docDetID, long hdrID, string lineType, string lineDesc,
          double entrdAmnt, int entrdCurrID, int codeBhnd, string docType,
          bool autoCalc, string incrDcrs1, int costngID, string incrDcrs2, int blncgAccntID,
          long prepayDocHdrID, string vldyStatus, long orgnlLnID,
          int funcCurrID, int accntCurrID, double funcCurrRate, double accntCurrRate,
          double funcCurrAmnt, double accntCurrAmnt, long initAmntID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_ptycsh_amnt_smmrys
   SET ptycsh_smmry_type='" + lineType.Replace("'", "''") +
                  "', ptycsh_smmry_desc='" + lineDesc.Replace("'", "''") +
                  "', ptycsh_smmry_amnt=" + entrdAmnt +
                  ", code_id_behind=" + codeBhnd +
                  ", src_ptycsh_type='" + docType.Replace("'", "''") +
                  "', src_ptycsh_hdr_id=" + hdrID +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', auto_calc='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(autoCalc) +
                  "', incrs_dcrs1='" + incrDcrs1.Replace("'", "''") +
                  "', asset_expns_acnt_id=" + costngID +
                  ", incrs_dcrs2='" + incrDcrs2.Replace("'", "''") +
                  "', liability_acnt_id=" + blncgAccntID +
                  ", appld_prepymnt_doc_id=" + prepayDocHdrID +
                  ", validty_status='" + vldyStatus.Replace("'", "''") +
                  "', orgnl_line_id=" + orgnlLnID +
                  ", entrd_curr_id=" + entrdCurrID +
                  ", func_curr_id=" + funcCurrID +
                  ", accnt_curr_id=" + accntCurrID +
                  ", func_curr_rate=" + funcCurrRate +
                  ", accnt_curr_rate=" + accntCurrRate +
                  ", func_curr_amount=" + funcCurrAmnt +
                  ", accnt_curr_amnt=" + accntCurrAmnt +
                  ", initial_amnt_line_id=" + initAmntID +
                  " WHERE ptycsh_smmry_id = " + docDetID;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void deletePtycshDocHdrNDet(long valLnid, string docNum)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Document Number = " + docNum;
            string delSQL = "DELETE FROM accb.accb_ptycsh_amnt_smmrys WHERE src_ptycsh_hdr_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
            delSQL = "DELETE FROM accb.accb_ptycsh_vchr_hdr WHERE ptycsh_vchr_hdr_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deletePtycshDocDet(long valLnid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSQL = "DELETE FROM accb.accb_ptycsh_amnt_smmrys WHERE ptycsh_smmry_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static DataSet get_One_PtycshDocHdr(long hdrID)
        {
            string strSql = "";

            strSql = @"SELECT ptycsh_vchr_hdr_id, to_char(to_timestamp(ptycsh_vchr_date,'YYYY-MM-DD'),'DD-Mon-YYYY'), 
       created_by, sec.get_usr_name(a.created_by), ptycsh_vchr_number, ptycsh_vchr_type, 
       comments_desc, src_doc_hdr_id, supplier_id, scm.get_cstmr_splr_name(a.supplier_id),
       supplier_site_id, scm.get_cstmr_splr_site_name(a.supplier_site_id), 
       approval_status, next_aproval_action, invoice_amount, 
       payment_terms, src_doc_type, pymny_method_id, accb.get_pymnt_mthd_name(a.pymny_method_id), 
       amnt_paid, gl_batch_id, accb.get_gl_batch_name(a.gl_batch_id),
       spplrs_invc_num, doc_tmplt_clsfctn, invc_curr_id, gst.get_pssbl_val(a.invc_curr_id),
        event_rgstr_id, evnt_cost_category, event_doc_type, balancing_accnt_id   
  FROM accb.accb_ptycsh_vchr_hdr a " +
              "WHERE((a.ptycsh_vchr_hdr_id = " + hdrID + "))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.ptycshFrm.docTmplt_SQL = strSql;
            return dtst;
        }

        public static DataSet get_PtycshDocHdr(string searchWord, string searchIn, long offset,
          int limit_size, long orgID, bool shwUnpstdOnly)
        {
            string strSql = "";
            string whrcls = "";
            /*Document Number
         Document Description
         Document Classification
         Supplier Name
         Supplier's Invoice Number
         Source Doc Number
         Approval Status
         Created By*/
            string unpstdCls = "";
            if (shwUnpstdOnly)
            {
                unpstdCls = " AND (round(a.invoice_amount-a.amnt_paid,2)>0 or a.approval_status IN ('Not Validated','Validated','Reviewed') " +
                    "or accb.is_gl_batch_pstd(a.gl_batch_id)='0' )";
            }
            if (searchIn == "Document Number")
            {
                whrcls = " and (a.ptycsh_vchr_number ilike '" + searchWord.Replace("'", "''") +
                  "' or trim(to_char(a.ptycsh_vchr_hdr_id, '99999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            else if (searchIn == "Document Description")
            {
                whrcls = " and (a.comments_desc ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Document Classification")
            {
                whrcls = " and (a.doc_tmplt_clsfctn ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Supplier Name")
            {
                whrcls = @" and (a.supplier_id IN (select c.cust_sup_id from 
scm.scm_cstmr_suplr c where c.cust_sup_name ilike '" + searchWord.Replace("'", "''") +
            "'))";
            }
            else if (searchIn == "Supplier's Invoice Number")
            {
                whrcls = " and (a.spplrs_invc_num ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Source Doc Number")
            {
                whrcls = @" and (trim(to_char(a.src_doc_hdr_id, '9999999999999999999999999')) 
IN (select trim(to_char(d.rcpt_id, '9999999999999999999999999')) from inv.inv_consgmt_rcpt_hdr d 
where trim(to_char(d.rcpt_id, '9999999999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
            @"') or trim(to_char(a.src_doc_hdr_id, '9999999999999999999999999')) 
IN (select trim(to_char(e.rcpt_rtns_id, '9999999999999999999999999')) from inv.inv_consgmt_rcpt_rtns_hdr e 
where trim(to_char(e.rcpt_rtns_id, '9999999999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
            @"') or a.src_doc_hdr_id IN (select f.ptycsh_vchr_hdr_id from accb.accb_ptycsh_vchr_hdr f
where f.ptycsh_vchr_number ilike '" + searchWord.Replace("'", "''") +
            @"'))";
            }
            else if (searchIn == "Approval Status")
            {
                whrcls = " and (a.approval_status ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Created By")
            {
                whrcls = " and (sec.get_usr_name(a.created_by) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Currency")
            {
                whrcls = " and (gst.get_pssbl_val(a.invc_curr_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            strSql = @"SELECT ptycsh_vchr_hdr_id, ptycsh_vchr_number, ptycsh_vchr_type
, round(a.invoice_amount-a.amnt_paid,2),
 a.approval_status, a.gl_batch_id 
        FROM accb.accb_ptycsh_vchr_hdr a 
        WHERE((a.org_id = " + orgID + ")" + whrcls + unpstdCls +
              ") ORDER BY ptycsh_vchr_hdr_id DESC LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.ptycshFrm.rec_SQL = strSql;
            return dtst;
        }

        public static long get_Total_PtycshDoc(string searchWord, string searchIn, long orgID, bool shwUnpstdOnly)
        {
            string strSql = "";
            string whrcls = "";
            /*Document Number
         Document Description
         Document Classification
         Supplier Name
         Supplier's Invoice Number
         Source Doc Number
         Approval Status
         Created By*/
            string unpstdCls = "";
            if (shwUnpstdOnly)
            {
                unpstdCls = " AND (round(a.invoice_amount-a.amnt_paid,2)>0 or a.approval_status IN ('Not Validated','Validated','Reviewed') " +
                     "or accb.is_gl_batch_pstd(a.gl_batch_id)='0' )";
            }
            if (searchIn == "Document Number")
            {
                whrcls = " and (a.ptycsh_vchr_number ilike '" + searchWord.Replace("'", "''") + "' or trim(to_char(a.ptycsh_vchr_hdr_id, '99999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            else if (searchIn == "Document Description")
            {
                whrcls = " and (a.comments_desc ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Document Classification")
            {
                whrcls = " and (a.doc_tmplt_clsfctn ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Supplier Name")
            {
                whrcls = @" and (a.supplier_id IN (select c.cust_sup_id from 
scm.scm_cstmr_suplr c where c.cust_sup_name ilike '" + searchWord.Replace("'", "''") +
            "'))";
            }
            else if (searchIn == "Supplier's Invoice Number")
            {
                whrcls = " and (a.spplrs_invc_num ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Source Doc Number")
            {
                whrcls = @" and (trim(to_char(a.src_doc_hdr_id, '9999999999999999999999999')) 
IN (select trim(to_char(d.rcpt_id, '9999999999999999999999999')) from inv.inv_consgmt_rcpt_hdr d 
where trim(to_char(d.rcpt_id, '9999999999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
            @"') or trim(to_char(a.src_doc_hdr_id, '9999999999999999999999999')) 
IN (select trim(to_char(e.rcpt_rtns_id, '9999999999999999999999999')) from inv.inv_consgmt_rcpt_rtns_hdr e 
where trim(to_char(e.rcpt_rtns_id, '9999999999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
            @"') or a.src_doc_hdr_id IN (select f.ptycsh_vchr_hdr_id from accb.accb_ptycsh_vchr_hdr f
where f.ptycsh_vchr_number ilike '" + searchWord.Replace("'", "''") +
            @"'))";
            }
            else if (searchIn == "Approval Status")
            {
                whrcls = " and (a.approval_status ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Created By")
            {
                whrcls = " and (sec.get_usr_name(a.created_by) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Currency")
            {
                whrcls = " and (gst.get_pssbl_val(a.invc_curr_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            strSql = @"SELECT count(1) FROM accb.accb_ptycsh_vchr_hdr a  
        WHERE((a.org_id = " + orgID + ")" + whrcls + unpstdCls +
              ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static DataSet get_PtycshDocDet(long docHdrID)
        {
            string strSql = "";
            string whrcls = @" and (a.ptycsh_smmry_type !='6Grand Total' and 
a.ptycsh_smmry_type !='7Total Payments Made' and a.ptycsh_smmry_type !='8Outstanding Balance')";
            //if (aprvlStatus != "Not Validated")
            //{
            //  //whrcls = "";, string aprvlStatus
            //}
            strSql = @"SELECT ptycsh_smmry_id, ptycsh_smmry_type, ptycsh_smmry_desc, ptycsh_smmry_amnt, 
       code_id_behind, auto_calc, incrs_dcrs1, 
       asset_expns_acnt_id, incrs_dcrs2, liability_acnt_id, appld_prepymnt_doc_id, 
       entrd_curr_id, gst.get_pssbl_val(a.entrd_curr_id), 
       func_curr_id, gst.get_pssbl_val(a.func_curr_id), 
      accnt_curr_id, gst.get_pssbl_val(a.accnt_curr_id), 
      func_curr_rate, accnt_curr_rate, 
       func_curr_amount, accnt_curr_amnt, initial_amnt_line_id
  FROM accb.accb_ptycsh_amnt_smmrys a " +
              "WHERE((a.src_ptycsh_hdr_id = " + docHdrID + ")" + whrcls + ") ORDER BY ptycsh_smmry_type ASC ";

            //MessageBox.Show(strSql);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.ptycshFrm.recDt_SQL = strSql;
            return dtst;
        }

        public static DataSet get_PtycshDocSmryLns(long dochdrID, string docTyp)
        {
            string strSql = "SELECT a.ptycsh_smmry_id, a.ptycsh_smmry_desc, " +
             "CASE WHEN substr(a.ptycsh_smmry_type,1,1) IN ('3','5') THEN -1 * a.ptycsh_smmry_amnt ELSE a.ptycsh_smmry_amnt END, a.code_id_behind, a.ptycsh_smmry_type, a.auto_calc " +
             "FROM accb.accb_ptycsh_amnt_smmrys a " +
             "WHERE((a.src_ptycsh_hdr_id = " + dochdrID +
             ") and (a.src_ptycsh_type='" + docTyp +
             "') and (substr(a.ptycsh_smmry_type,1,1) NOT IN ('6','7','8'))) ORDER BY a.ptycsh_smmry_type";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);

            return dtst;
        }

        public static DataSet get_PtycshDocEndLns(long dochdrID, string docTyp)
        {
            string strSql = "SELECT a.ptycsh_smmry_id, a.ptycsh_smmry_desc, " +
             "a.ptycsh_smmry_amnt, a.code_id_behind, substr(a.ptycsh_smmry_type,2), a.auto_calc " +
             "FROM accb.accb_ptycsh_amnt_smmrys a " +
             "WHERE((a.src_ptycsh_hdr_id = " + dochdrID +
             ") and (a.src_ptycsh_type='" + docTyp + "') and (substr(a.ptycsh_smmry_type,1,1) IN ('6','7'))) ORDER BY a.ptycsh_smmry_type";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);

            return dtst;
        }

        public static double getPtycshDocGrndAmnt(long dochdrID)
        {
            string strSql = @"select SUM(CASE WHEN y.ptycsh_smmry_type='3Discount' 
or scm.istaxwthhldng(y.code_id_behind)='1' or y.ptycsh_smmry_type='5Applied Prepayment'
      THEN -1*y.ptycsh_smmry_amnt ELSE y.ptycsh_smmry_amnt END) amnt " +
              "from accb.accb_ptycsh_amnt_smmrys y " +
              "where y.src_ptycsh_hdr_id=" + dochdrID +
              " and y.src_ptycsh_hdr_id>0 and y.ptycsh_smmry_type IN ('1Initial Amount','2Tax','3Discount','4Extra Charge','5Applied Prepayment')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double rs = 0;

            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
            }
            return rs;
        }

        public static double getPtycshDocFuncAmnt(long dochdrID)
        {
            string strSql = @"select SUM(CASE WHEN y.ptycsh_smmry_type='3Discount' 
or scm.istaxwthhldng(y.code_id_behind)='1' or y.ptycsh_smmry_type='5Applied Prepayment'
      THEN -1*y.func_curr_amount ELSE y.func_curr_amount END) amnt " +
              "from accb.accb_ptycsh_amnt_smmrys y " +
              "where y.src_ptycsh_hdr_id=" + dochdrID +
              " and y.ptycsh_smmry_type IN ('1Initial Amount','2Tax','3Discount','4Extra Charge','5Applied Prepayment')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double rs = 0;

            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
            }
            return rs;
        }

        public static double getPtycshDocAccntAmnt(long dochdrID)
        {
            string strSql = @"select SUM(CASE WHEN y.ptycsh_smmry_type='3Discount' 
or scm.istaxwthhldng(y.code_id_behind)='1' or y.ptycsh_smmry_type='5Applied Prepayment'
      THEN -1*y.accnt_curr_amnt ELSE y.accnt_curr_amnt END) amnt " +
              "from accb.accb_ptycsh_amnt_smmrys y " +
              "where y.src_ptycsh_hdr_id=" + dochdrID +
              " and y.ptycsh_smmry_type IN ('1Initial Amount','2Tax','3Discount','4Extra Charge','5Applied Prepayment')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double rs = 0;

            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
            }
            return rs;
        }

        public static long getPtycshSmmryItmID(string smmryType, int codeBhnd,
          long srcDocID, string srcDocTyp, string smmryNm)
        {
            string strSql = "select y.ptycsh_smmry_id " +
              "from accb.accb_ptycsh_amnt_smmrys y " +
              "where y.ptycsh_smmry_type= '" + smmryType + "' and y.ptycsh_smmry_desc = '" + smmryNm +
              "' and y.code_id_behind= " + codeBhnd +
              " and y.src_ptycsh_type='" + srcDocTyp.Replace("'", "''") +
              "' and y.src_ptycsh_hdr_id=" + srcDocID + " ";
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

        public static void updtPtycshDocApprvl(long docid,
      string apprvlSts, string nxtApprvl)
        {
            string extrCls = "";
            if (apprvlSts == "Cancelled")
            {
                extrCls = ", invoice_amount=0, invc_amnt_appld_elswhr=0";
            }
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_ptycsh_vchr_hdr SET " +
                  "approval_status='" + apprvlSts.Replace("'", "''") +
                  "', last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', next_aproval_action='" + nxtApprvl.Replace("'", "''") +
                  "'" + extrCls + " WHERE (ptycsh_vchr_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtPtycshDocAmnt(long docid, double invAmnt)
        {
            string extrCls = ", invoice_amount=" + invAmnt + "";

            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_ptycsh_vchr_hdr SET " +
                  "last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "'" + extrCls + " WHERE (ptycsh_vchr_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtPtycshDocGLBatch(long docid,
      long glBatchID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_ptycsh_vchr_hdr SET " +
                  "gl_batch_id=" + glBatchID +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (ptycsh_vchr_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtPtycshDocAmntPaid(long docid,
      double amntPaid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_ptycsh_vchr_hdr SET " +
                  "amnt_paid=amnt_paid + " + amntPaid +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (ptycsh_vchr_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtPtycshDocAmntAppld(long docid,
      double amntAppld)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_ptycsh_vchr_hdr SET " +
                  "invc_amnt_appld_elswhr=invc_amnt_appld_elswhr + " + amntAppld +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (ptycsh_vchr_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static double getPtycshDocTtlPymnts(long dochdrID, string docType)
        {
            string strSql = "select SUM(y.amount_paid) amnt " +
              "from accb.accb_payments y " +
              "where y.src_doc_id = " + dochdrID + " and y.src_doc_typ = '" + docType.Replace("'", "''") + "'";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double rs = 0;

            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
            }
            return rs;
        }

        public static int getPtycshDocBlncngAccnt(long srcDocID, string docType)
        {
            string whrcls = @" and (a.ptycsh_smmry_type !='6Grand Total' and 
a.ptycsh_smmry_type !='7Total Payments Made' and a.ptycsh_smmry_type !='8Outstanding Balance')";

            string selSQL = @"select 
        distinct liability_acnt_id, ptycsh_smmry_id 
        from accb.accb_ptycsh_amnt_smmrys a 
        where src_ptycsh_hdr_id = " + srcDocID +
              " and src_ptycsh_type = '" + docType.Replace("'", "''") +
              "'" + whrcls + " order by ptycsh_smmry_id LIMIT 1 OFFSET 0";
            //Global.mnFrm.cmCde.showSQLNoPermsn(selSQL);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(selSQL);

            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }
        public static int getPtycshPrepayDocCnt(long dochdrID)
        {
            string strSql = @"select count(appld_prepymnt_doc_id) " +
              "from accb.accb_ptycsh_amnt_smmrys y " +
              "where y.src_ptycsh_hdr_id = " + dochdrID + " and y.appld_prepymnt_doc_id >0 " +
              "Group by y.appld_prepymnt_doc_id having count(y.appld_prepymnt_doc_id)>1";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            int rs = 0;

            if (dtst.Tables[0].Rows.Count > 0)
            {
                int.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
                return rs;
            }
            return 0;
        }
        public static DataSet get_Ptycsh_Attachments(string searchWord, string searchIn,
      Int64 offset, int limit_size, long batchID, ref string attchSQL)
        {
            string strSql = "";
            if (searchIn == "Attachment Name/Description")
            {
                strSql = "SELECT a.attchmnt_id, a.doc_hdr_id, a.attchmnt_desc, a.file_name " +
              "FROM accb.accb_ptycsh_doc_attchmnts a " +
              "WHERE(a.attchmnt_desc ilike '" + searchWord.Replace("'", "''") +
              "' and a.doc_hdr_id = " + batchID + ") ORDER BY a.attchmnt_id LIMIT " + limit_size +
                  " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            attchSQL = strSql;
            return dtst;
        }

        public static long get_Total_Ptycsh_Attachments(string searchWord,
          string searchIn, long batchID)
        {
            string strSql = "";
            if (searchIn == "Attachment Name/Description")
            {
                strSql = "SELECT COUNT(1) " +
              "FROM accb.accb_ptycsh_doc_attchmnts a " +
              "WHERE(a.attchmnt_desc ilike '" + searchWord.Replace("'", "''") +
              "' and a.doc_hdr_id = " + batchID + ")";
            }
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            long sumRes = 0;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                long.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }

        #endregion

        #region "PAYMENTS..."
        public static long getNewPymntBatchID()
        {
            //string strSql = "select nextval('accb.accb_trnsctn_batches_batch_id_seq'::regclass);";
            string strSql = "select  last_value from accb.accb_payments_batches_pymnt_batch_id_seq";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString()) + 1;
            }
            return -1;
        }

        public static long getNewPymntLnID()
        {
            //string strSql = "select nextval('accb.accb_trnsctn_batches_batch_id_seq'::regclass);";
            string strSql = "select nextval('accb.accb_payments_pymnt_id_seq')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static void createPymntsBatch(int orgid, string strtDte,
          string endDte, string docType,
        string batchName, string batchDesc, int spplrID, int pymntMthdID,
          string batchSource, long orgnlBtchID,
          string vldtyStatus, string docTmpltClsftn, string batchStatus)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            strtDte = DateTime.ParseExact(strtDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            endDte = DateTime.ParseExact(endDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string insSQL = @"INSERT INTO accb.accb_payments_batches(
            pymnt_batch_name, pymnt_batch_desc, pymnt_mthd_id, 
            doc_type, doc_clsfctn, docs_start_date, docs_end_date, batch_status, 
            batch_source, created_by, creation_date, last_update_by, last_update_date, 
            batch_vldty_status, orgnl_batch_id, org_id, cust_spplr_id) " +
                  "VALUES ('" + batchName.Replace("'", "''") +
                  "', '" + batchDesc.Replace("'", "''") +
                  "', " + pymntMthdID +
                  ", '" + docType.Replace("'", "''") +
                  "', '" + docTmpltClsftn.Replace("'", "''") +
                  "', '" + strtDte.Replace("'", "''") +
                  "', '" + endDte.Replace("'", "''") +
                  "', '" + batchStatus.Replace("'", "''") +
                  "', '" + batchSource.Replace("'", "''") +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', '" + vldtyStatus.Replace("'", "''") +
                  "', " + orgnlBtchID +
                  ", " + orgid + ", " + spplrID +
                  ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtPymntsBatchVldty(long batchID, string vldtyStatus)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_payments_batches SET 
            last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', batch_vldty_status='" + vldtyStatus.Replace("'", "''") +
                  "' WHERE pymnt_batch_id = " + batchID;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void updtPymntsLnVldty(long pymtLnID, string vldtyStatus)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_payments SET 
            last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', pymnt_vldty_status='" + vldtyStatus.Replace("'", "''") +
                  "' WHERE pymnt_id = " + pymtLnID;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void updtPymntsBatch(long batchID, string strtDte,
          string endDte, string docType,
        string batchName, string batchDesc, int spplrID, int pymntMthdID,
          string batchSource, long orgnlBtchID,
          string vldtyStatus, string docTmpltClsftn, string batchStatus)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            strtDte = DateTime.ParseExact(strtDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            endDte = DateTime.ParseExact(endDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string insSQL = @"UPDATE accb.accb_payments_batches SET 
            pymnt_batch_name='" + batchName.Replace("'", "''") +
                  "', pymnt_batch_desc='" + batchDesc.Replace("'", "''") +
                  "', pymnt_mthd_id=" + pymntMthdID +
                  ", doc_type='" + docType.Replace("'", "''") +
                  "', doc_clsfctn='" + docTmpltClsftn.Replace("'", "''") +
                  "', docs_start_date='" + strtDte.Replace("'", "''") +
                  "', docs_end_date='" + endDte.Replace("'", "''") +
                  "', batch_status='" + batchStatus.Replace("'", "''") +
                  "', batch_source='" + batchSource.Replace("'", "''") +
                  "', last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', batch_vldty_status='" + vldtyStatus.Replace("'", "''") +
                  "', orgnl_batch_id=" + orgnlBtchID +
                  ", cust_spplr_id=" + spplrID +
                  " WHERE pymnt_batch_id = " + batchID;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void createPymntDet(long pymntID, long pymntBatchID, int pymntMthdID,
          double amntPaid, int entrdCurrID, double chnge_bals, string pymntRemark,
          string srcDocType, long srcDocID, string pymntDte,
          string incrDcrs1, int blncgAccntID, string incrDcrs2, int chrgAccntID,
          long glBatchID, string vldyStatus, long orgnlLnID,
          int funcCurrID, int accntCurrID, double funcCurrRate, double accntCurrRate,
          double funcCurrAmnt, double accntCurrAmnt, long prepayDocID, string prepayDocType,
         string otherinfo, string cardNm, string expryDte, string cardNum, string sgnCode, string actvtyStatus,
         string actvtyDocName)
        {
            pymntDte = DateTime.ParseExact(pymntDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_payments(
            pymnt_id, pymnt_mthd_id, amount_paid, change_or_balance, pymnt_remark, 
            src_doc_typ, src_doc_id, created_by, creation_date, last_update_by, 
            last_update_date, pymnt_date, incrs_dcrs1, rcvbl_lblty_accnt_id, 
            incrs_dcrs2, cash_or_suspns_acnt_id, gl_batch_id, orgnl_pymnt_id, 
            pymnt_vldty_status, entrd_curr_id, func_curr_id, accnt_curr_id, 
            func_curr_rate, accnt_curr_rate, func_curr_amount, accnt_curr_amnt, 
            pymnt_batch_id, prepay_doc_id, prepay_doc_type, pay_means_other_info, cheque_card_name, 
            expiry_date, cheque_card_num, sign_code, bkgrd_actvty_status, 
            bkgrd_actvty_gen_doc_name) " +
                  "VALUES (" + pymntID + ", " + pymntMthdID + "," + amntPaid + "," + chnge_bals +
                  ",'" + pymntRemark.Replace("'", "''") +
                  "', '" + srcDocType.Replace("'", "''") +
                  "', " + srcDocID +
                  ", " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "','" + pymntDte.Replace("'", "''") +
                  "', '" + incrDcrs1.Replace("'", "''") +
                  "', " + blncgAccntID +
                  ", '" + incrDcrs2.Replace("'", "''") +
                  "', " + chrgAccntID +
                  ", " + glBatchID +
                  ", " + orgnlLnID +
                  ", '" + vldyStatus.Replace("'", "''") +
                  "', " + entrdCurrID +
                  ", " + funcCurrID +
                  ", " + accntCurrID +
                  ", " + funcCurrRate +
                  ", " + accntCurrRate +
                  ", " + funcCurrAmnt +
                  ", " + accntCurrAmnt +
                  ", " + pymntBatchID +
                  ", " + prepayDocID +
                  ", '" + prepayDocType.Replace("'", "''") +
                  "', '" + otherinfo.Replace("'", "''") +
                  "', '" + cardNm.Replace("'", "''") +
                  "', '" + expryDte.Replace("'", "''") +
                  "', '" + cardNum.Replace("'", "''") +
                  "','" + Global.mnFrm.cmCde.encrypt1(sgnCode, CommonCode.CommonCodes.AppKey).Replace("'", "''") +
                  "', '" + actvtyStatus.Replace("'", "''") +
                  "', '" + actvtyDocName.Replace("'", "''") +
                  "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        /* public static void updtPymntDet(long pymntID, long pymntBatchID, int pymntMthdID,
           double amntPaid, int entrdCurrID, double chnge_bals, string pymntRemark,
           string srcDocType, long srcDocID, string pymntDte,
           string incrDcrs1, int blncgAccntID, string incrDcrs2, int chrgAccntID,
           long glBatchID, string vldyStatus, long orgnlLnID,
           int funcCurrID, int accntCurrID, double funcCurrRate, double accntCurrRate,
           double funcCurrAmnt, double accntCurrAmnt)
         {
           Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
           pymntDte = DateTime.ParseExact(pymntDte, "dd-MMM-yyyy HH:mm:ss",
        System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

           string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
           string insSQL = @"UPDATE accb.accb_payments SET 
                 pymnt_mthd_id=" + pymntMthdID + ", amount_paid=" + amntPaid +
                 ", change_or_balance=" + chnge_bals +
                 ", pymnt_remark='" + pymntRemark.Replace("'", "''") +
                 "', src_doc_typ='" + srcDocType.Replace("'", "''") +
                 "', src_doc_id=" + srcDocID +
                 ", last_update_by=" + Global.myBscActn.user_id +
                 ", last_update_date='" + dateStr +
                 "', pymnt_date='" + pymntDte.Replace("'", "''") +
                 "', incrs_dcrs1='" + incrDcrs1.Replace("'", "''") +
                 "', rcvbl_lblty_accnt_id=" + blncgAccntID +
                 ", incrs_dcrs2='" + incrDcrs2.Replace("'", "''") +
                 "', cash_or_suspns_acnt_id=" + chrgAccntID +
                 ", gl_batch_id=" + glBatchID +
                 ", orgnl_pymnt_id=" + orgnlLnID +
                 ", pymnt_vldty_status='" + vldyStatus.Replace("'", "''") +
                 "', entrd_curr_id=" + entrdCurrID +
                 ", func_curr_id=" + funcCurrID +
                 ", accnt_curr_id=" + accntCurrID +
                 ", func_curr_rate=" + funcCurrRate +
                 ", accnt_curr_rate=" + accntCurrRate +
                 ", func_curr_amount=" + funcCurrAmnt +
                 ", accnt_curr_amnt=" + accntCurrAmnt +
                 ", pymnt_batch_id=" + pymntBatchID +
                 " WHERE pymnt_id = " + pymntID;
           Global.mnFrm.cmCde.updateDataNoParams(insSQL);
         }*/

        public static void deletePymntsBatchNDet(long valLnid, string batchName)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Batch Name = " + batchName;
            string delSQL = "DELETE FROM accb.accb_payments WHERE pymnt_batch_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
            delSQL = "DELETE FROM accb.accb_payments_batches WHERE pymnt_batch_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deletePymntsDet(long valLnid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSQL = "DELETE FROM accb.accb_payments WHERE pymnt_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static DataSet get_One_PymntBatchHdr(long hdrID)
        {
            string strSql = "";

            strSql = @"SELECT pymnt_batch_id, pymnt_batch_name, pymnt_batch_desc, 
      pymnt_mthd_id, accb.get_pymnt_mthd_name(a.pymnt_mthd_id), 
       doc_type, doc_clsfctn, to_char(to_timestamp(docs_start_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
to_char(to_timestamp(docs_end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), batch_status, 
       batch_source, cust_spplr_id, scm.get_cstmr_splr_name(cust_spplr_id),
       batch_vldty_status, orgnl_batch_id, org_id
      FROM accb.accb_payments_batches a " +
              "WHERE((a.pymnt_batch_id = " + hdrID + "))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_PymntBatch(string searchWord, string searchIn, long offset,
          int limit_size, long orgID, string startDte, string endDte)
        {
            startDte = DateTime.ParseExact(startDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            endDte = DateTime.ParseExact(endDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";
            string whrcls = "";
            string dteCls = @" and (a.pymnt_batch_id IN (select f.pymnt_batch_id from accb.accb_payments f where 
to_timestamp(f.pymnt_date,'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + startDte + @"','YYYY-MM-DD HH24:MI:SS') 
and to_timestamp('" + endDte + "','YYYY-MM-DD HH24:MI:SS')))";
            /*Batch Name
         Batch Description
         Payment Method
         Document Type
         Document Classification
         Supplier Name
         Batch Source
         Batch Status*/
            if (searchIn == "Batch Name")
            {
                whrcls = " and (a.pymnt_batch_name ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Batch Description")
            {
                whrcls = " and (a.pymnt_batch_desc ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Document Classification")
            {
                whrcls = " and (a.doc_clsfctn ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Supplier Name")
            {
                whrcls = @" and (a.supplier_id IN (select c.cust_sup_id from 
scm.scm_cstmr_suplr c where c.cust_sup_name ilike '" + searchWord.Replace("'", "''") +
            "'))";
            }
            else if (searchIn == "Payment Method")
            {
                whrcls = " and (accb.get_pymnt_mthd_name(a.pymnt_mthd_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Source Doc Number")
            {
                whrcls = @" and (a.pymnt_batch_id IN (select y.pymnt_batch_id from accb.accb_payments y where accb.get_src_doc_num(y.src_doc_id,y.src_doc_typ) ilike '" + searchWord.Replace("'", "''") +
            "'))";
            }
            else if (searchIn == "Document Type")
            {
                whrcls = " and (a.doc_type ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Batch Source")
            {
                whrcls = " and a.batch_source ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Batch Status")
            {
                whrcls = " and a.batch_status ilike '" + searchWord.Replace("'", "''") + "')";
            }
            strSql = @"SELECT pymnt_batch_id, pymnt_batch_name, pymnt_batch_desc 
        FROM accb.accb_payments_batches a 
        WHERE((a.org_id = " + orgID + ")" + whrcls + dteCls +
              ") ORDER BY pymnt_batch_id DESC LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.pymntFrm.rec_SQL = strSql;
            return dtst;
        }

        public static long get_Total_PymntBatch(string searchWord, string searchIn, long orgID, string startDte, string endDte)
        {
            startDte = DateTime.ParseExact(startDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            endDte = DateTime.ParseExact(endDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";
            string whrcls = "";
            string dteCls = @" and (a.pymnt_batch_id IN (select f.pymnt_batch_id from accb.accb_payments f where 
to_timestamp(f.pymnt_date,'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + startDte + @"','YYYY-MM-DD HH24:MI:SS') 
and to_timestamp('" + endDte + "','YYYY-MM-DD HH24:MI:SS')))";
            /*Batch Name
         Batch Description
         Payment Method
         Document Type
         Document Classification
         Supplier Name
         Batch Source
         Batch Status*/
            if (searchIn == "Batch Name")
            {
                whrcls = " and (a.pymnt_batch_name ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Batch Description")
            {
                whrcls = " and (a.pymnt_batch_desc ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Document Classification")
            {
                whrcls = " and (a.doc_clsfctn ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Supplier Name")
            {
                whrcls = @" and (a.supplier_id IN (select c.cust_sup_id from 
scm.scm_cstmr_suplr c where c.cust_sup_name ilike '" + searchWord.Replace("'", "''") +
            "'))";
            }
            else if (searchIn == "Payment Method")
            {
                whrcls = " and (accb.get_pymnt_mthd_name(a.pymnt_mthd_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Source Doc Number")
            {
                whrcls = @" and (a.pymnt_batch_id IN (select y.pymnt_batch_id from accb.accb_payments y where accb.get_src_doc_num(y.src_doc_id,y.src_doc_typ) ilike '" + searchWord.Replace("'", "''") +
            "'))";
            }
            else if (searchIn == "Document Type")
            {
                whrcls = " and (a.doc_type ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Batch Source")
            {
                whrcls = " and a.batch_source ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Batch Status")
            {
                whrcls = " and a.batch_status ilike '" + searchWord.Replace("'", "''") + "')";
            }
            strSql = @"SELECT count(1) 
        FROM accb.accb_payments_batches a 
        WHERE((a.org_id = " + orgID + ")" + whrcls + dteCls +
              ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static DataSet get_PymntBatchLns(long offset,
          int limit_size, long docHdrID)
        {
            string strSql = "";

            strSql = @"SELECT pymnt_id, pymnt_mthd_id, amount_paid, change_or_balance, pymnt_remark, 
       src_doc_typ, src_doc_id, accb.get_src_doc_num(a.src_doc_id, a.src_doc_typ), 
       to_char(to_timestamp(pymnt_date, 'YYYY-MM-DD HH24:MI:SS'), 'DD-Mon-YYYY HH24:MI:SS'), 
       incrs_dcrs1, rcvbl_lblty_accnt_id, 
       incrs_dcrs2, cash_or_suspns_acnt_id, 
       gl_batch_id, accb.get_gl_batch_name(gl_batch_id), 
       orgnl_pymnt_id, pymnt_vldty_status, 
       entrd_curr_id, gst.get_pssbl_val(a.entrd_curr_id), 
       func_curr_id, gst.get_pssbl_val(a.func_curr_id), 
       accnt_curr_id, gst.get_pssbl_val(a.accnt_curr_id), 
       func_curr_rate, accnt_curr_rate, func_curr_amount, accnt_curr_amnt, 
       pymnt_batch_id
       FROM accb.accb_payments a " +
              "WHERE((a.pymnt_batch_id = " + docHdrID + ")) ORDER BY pymnt_id ASC LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.pymntFrm.recDt_SQL = strSql;
            // Global.mnFrm.cmCde.showSQLNoPermsn(strSql);
            return dtst;
        }

        public static void updtPymntBatchStatus(long docid,
      string batchStatus)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_payments_batches SET " +
                  "batch_status='" + batchStatus.Replace("'", "''") +
                  "', last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (pymnt_batch_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtPymntLnGLBatch(long docid,
      long glBatchID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_payments SET " +
                  "gl_batch_id=" + glBatchID +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (pymnt_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static DataSet getPymntMthds(int orgID, string docType)
        {
            string selSQL = @"select 
        distinct trim(to_char(paymnt_mthd_id,'999999999999999999999999999999')) a, 
        pymnt_mthd_name b, '' c, org_id d, supported_doc_type e 
        from accb.accb_paymnt_mthds 
        where is_enabled = '1' and org_id = " + orgID +
              " and supported_doc_type = '" + docType.Replace("'", "''") +
              "' order by pymnt_mthd_name LIMIT 30 OFFSET 0";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(selSQL);
            return dtst;
        }

        public static int getRcvblsDocBlncngAccnt(long srcDocID, string docType)
        {
            string whrcls = @" and (a.rcvbl_smmry_type !='6Grand Total' and 
a.rcvbl_smmry_type !='7Total Payments Made' and a.rcvbl_smmry_type !='8Outstanding Balance')";

            string selSQL = @"select 
        distinct rcvbl_acnt_id, rcvbl_smmry_id 
        from accb.accb_rcvbl_amnt_smmrys a 
        where src_rcvbl_hdr_id = " + srcDocID +
              " and src_rcvbl_type = '" + docType.Replace("'", "''") +
              "'" + whrcls + " order by rcvbl_smmry_id LIMIT 1 OFFSET 0";
            //Global.mnFrm.cmCde.showSQLNoPermsn(selSQL);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(selSQL);

            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static int getPymntMthdChrgAccnt(long pymntMthdID)
        {
            string selSQL = @"select 
        distinct current_asst_acnt_id, paymnt_mthd_id 
        from accb.accb_paymnt_mthds 
        where paymnt_mthd_id = " + pymntMthdID +
              " order by paymnt_mthd_id LIMIT 1 OFFSET 0";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(selSQL);

            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }
        #endregion

        #region "PAYMENTS DONE..."
        public static bool isPymntRvrsdB4(long orgnlPymntID)
        {
            string strSql = "";
            strSql = "SELECT a.pymnt_id FROM accb.accb_payments a " +
             "WHERE(a.orgnl_pymnt_id = " + orgnlPymntID + ") " +
             "ORDER BY a.pymnt_id LIMIT 1 " +
               " OFFSET 0";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static DataSet get_Pay_Trns(string searchWord, string searchIn,
      Int64 offset, int limit_size, string dte1, string dte2)
        {
            dte1 = DateTime.ParseExact(
         dte1, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            dte2 = DateTime.ParseExact(
         dte2, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";
            string whereCls = "";

            if (searchIn == "Source Document No.")
            {
                whereCls = " and (accb.get_src_doc_num(a.src_doc_id, a.src_doc_typ) ilike '" + searchWord.Replace("'", "''") +
               "')";
            }
            else if (searchIn == "Source Document Type")
            {
                whereCls = " and (a.src_doc_typ ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Payment Method")
            {
                whereCls = " and (accb.get_pymnt_mthd_name(a.pymnt_mthd_id) ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Cashier")
            {
                whereCls = " and (sec.get_usr_name(a.created_by) ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Payment Description")
            {
                whereCls = " and (a.pymnt_remark ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            strSql = @"SELECT a.pymnt_id, a.pymnt_mthd_id, accb.get_pymnt_mthd_name(a.pymnt_mthd_id), 
      a.amount_paid, a.change_or_balance, a.pymnt_remark, 
      a.src_doc_typ, a.src_doc_id, accb.get_src_doc_num(a.src_doc_id, a.src_doc_typ), 
      a.created_by, to_char(to_timestamp(a.pymnt_date, 'YYYY-MM-DD HH24:MI:SS'), 'DD-Mon-YYYY HH24:MI:SS'), 
      sec.get_usr_name(a.created_by), gl_batch_id, accb.get_gl_batch_name(gl_batch_id), 
b.pymnt_batch_name, a.pymnt_batch_id,a.prepay_doc_id, accb.get_src_doc_num(a.prepay_doc_id, a.prepay_doc_type),
a.pay_means_other_info, a.cheque_card_name, a.expiry_date, a.cheque_card_num, a.sign_code, a.bkgrd_actvty_status, a.bkgrd_actvty_gen_doc_name " +
             "FROM accb.accb_payments a, accb.accb_payments_batches b " +
             "WHERE((a.pymnt_batch_id = b.pymnt_batch_id)" + whereCls +
             " and (to_timestamp(a.pymnt_date,'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + dte1 +
             "','YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + dte2 + "','YYYY-MM-DD HH24:MI:SS'))) " +
             "ORDER BY a.pymnt_id DESC LIMIT " + limit_size +
               " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.mnFrm.pymntsGvn_SQL = strSql;
            return dtst;
        }

        public static long get_Total_Trns(string searchWord, string searchIn,
         string dte1, string dte2)
        {
            dte1 = DateTime.ParseExact(
         dte1, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            dte2 = DateTime.ParseExact(
         dte2, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";
            string whereCls = "";

            if (searchIn == "Source Document No.")
            {
                whereCls = " and (accb.get_src_doc_num(a.src_doc_id, a.src_doc_typ) ilike '" + searchWord.Replace("'", "''") +
               "')";
            }
            else if (searchIn == "Source Document Type")
            {
                whereCls = " and (a.src_doc_typ ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Payment Method")
            {
                whereCls = " and (accb.get_pymnt_mthd_name(a.pymnt_mthd_id) ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Cashier")
            {
                whereCls = " and (sec.get_usr_name(a.created_by) ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Payment Description")
            {
                whereCls = " and (a.pymnt_remark ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            strSql = @"SELECT count(1) " +
             "FROM accb.accb_payments a, accb.accb_payments_batches b " +
             "WHERE((a.pymnt_batch_id = b.pymnt_batch_id)" + whereCls +
             " and (to_timestamp(a.pymnt_date,'YYYY-MM-DD HH24:MI:SS') between to_timestamp('" + dte1 +
             "','YYYY-MM-DD HH24:MI:SS') AND to_timestamp('" + dte2 + "','YYYY-MM-DD HH24:MI:SS'))) ";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            long sumRes = 0;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                long.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
        }
        #endregion

        #region "RECEIVABLES..."
        public static long getNewRcvblsLnID()
        {
            //string strSql = "select nextval('accb.accb_trnsctn_batches_batch_id_seq'::regclass);";
            string strSql = "select nextval('accb.accb_rcvbl_amnt_smmrys_rcvbl_smmry_id_seq')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static string getLtstRcvblsIDNoInPrfx(string prfxTxt)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "select count(rcvbls_invc_hdr_id) from accb.accb_rcvbls_invc_hdr WHERE org_id=" +
              Global.mnFrm.cmCde.Org_id + " and rcvbls_invc_number ilike '" + prfxTxt.Replace("'", "''") + "%'";
            dtSt = Global.mnFrm.cmCde.selectDataNoParams(sqlStr);
            if (dtSt.Tables[0].Rows.Count > 0)
            {
                return (long.Parse(dtSt.Tables[0].Rows[0][0].ToString()) + 1).ToString().PadLeft(4, '0');
            }
            else
            {
                return "0001";
            }
        }

        public static void createRcvblsDocHdr(int orgid, string docDte, string docNum,
        string docType, string docDesc, long srcDocHdrID, int cstmrID, int cstmrSiteID,
          string apprvlStatus, string nxtApprvlActn, double invcAmnt, string pymntTrms,
          string srcDocType, int pymntMthdID, double amntPaid, long glBtchID,
          string cstmrDocNum, string docTmpltClsftn, int currID, double amntAppld,
          long rgstrID, string costCtgry, string evntType)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            docDte = DateTime.ParseExact(docDte, "dd-MMM-yyyy",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string insSQL = @"INSERT INTO accb.accb_rcvbls_invc_hdr(
            rcvbls_invc_date, created_by, creation_date, 
            last_update_by, last_update_date, rcvbls_invc_number, rcvbls_invc_type, 
            comments_desc, src_doc_hdr_id, customer_id, customer_site_id, 
            approval_status, next_aproval_action, org_id, invoice_amount, 
            payment_terms, src_doc_type, pymny_method_id, amnt_paid, gl_batch_id, 
            cstmrs_doc_num, doc_tmplt_clsfctn, invc_curr_id, invc_amnt_appld_elswhr,
            event_rgstr_id, evnt_cost_category, event_doc_type) " +
                  "VALUES ('" + docDte.Replace("'", "''") +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', '" + docNum.Replace("'", "''") +
                  "', '" + docType.Replace("'", "''") +
                  "', '" + docDesc.Replace("'", "''") +
                  "', " + srcDocHdrID +
                  ", " + cstmrID +
                  ", " + cstmrSiteID +
                  ", '" + apprvlStatus.Replace("'", "''") +
                  "', '" + nxtApprvlActn.Replace("'", "''") +
                  "', " + orgid +
                  ", " + invcAmnt +
                  ", '" + pymntTrms.Replace("'", "''") +
                  "', '" + srcDocType.Replace("'", "''") +
                  "', " + pymntMthdID +
                  ", " + amntPaid +
                  ", " + glBtchID +
                  ", '" + cstmrDocNum.Replace("'", "''") +
                  "', '" + docTmpltClsftn.Replace("'", "''") +
                  "', " + currID + ", " + amntAppld + ", " + rgstrID +
                  ", '" + costCtgry.Replace("'", "''") + "', '" + evntType.Replace("'", "''") + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtRcvblsDocHdr(long hdrID, string docDte, string docNum,
        string docType, string docDesc, long srcDocHdrID, int spplrID, int spplrSiteID,
          string apprvlStatus, string nxtApprvlActn, double invcAmnt, string pymntTrms,
          string srcDocType, int pymntMthdID, double amntPaid, long glBtchID,
          string spplrInvcNum, string docTmpltClsftn, int currID, double amntAppld,
          long rgstrID, string costCtgry, string evntType)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            docDte = DateTime.ParseExact(docDte, "dd-MMM-yyyy",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string insSQL = @"UPDATE accb.accb_rcvbls_invc_hdr
       SET rcvbls_invc_date='" + docDte.Replace("'", "''") +
                  "', last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', rcvbls_invc_number='" + docNum.Replace("'", "''") +
                  "', rcvbls_invc_type='" + docType.Replace("'", "''") +
                  "', comments_desc='" + docDesc.Replace("'", "''") +
                  "', src_doc_hdr_id=" + srcDocHdrID +
                  ", customer_id=" + spplrID +
                  ", customer_site_id=" + spplrSiteID +
                  ", approval_status='" + apprvlStatus.Replace("'", "''") +
                  "', next_aproval_action='" + nxtApprvlActn.Replace("'", "''") +
                  "', invoice_amount=" + invcAmnt +
                  ", payment_terms='" + pymntTrms.Replace("'", "''") +
                  "', src_doc_type='" + srcDocType.Replace("'", "''") +
                  "', pymny_method_id=" + pymntMthdID +
                  ", amnt_paid=" + amntPaid +
                  ", gl_batch_id=" + glBtchID +
                  ", cstmrs_doc_num='" + spplrInvcNum.Replace("'", "''") +
                  "', doc_tmplt_clsfctn='" + docTmpltClsftn.Replace("'", "''") +
                  "', invc_curr_id=" + currID +
                  ", event_rgstr_id=" + rgstrID +
                  ", evnt_cost_category='" + costCtgry.Replace("'", "''") +
                  "', event_doc_type='" + evntType.Replace("'", "''") +
               "' WHERE rcvbls_invc_hdr_id = " + hdrID;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void createRcvblsDocDet(long smmryID, long hdrID, string lineType, string lineDesc,
          double entrdAmnt, int entrdCurrID, int codeBhnd, string docType,
          bool autoCalc, string incrDcrs1, int costngID, string incrDcrs2, int blncgAccntID,
          long prepayDocHdrID, string vldyStatus, long orgnlLnID,
          int funcCurrID, int accntCurrID, double funcCurrRate, double accntCurrRate,
          double funcCurrAmnt, double accntCurrAmnt, long initAmntLineID, double lineQty, double unitPrice)
        {

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_rcvbl_amnt_smmrys(
            rcvbl_smmry_id, rcvbl_smmry_type, rcvbl_smmry_desc, rcvbl_smmry_amnt, 
            code_id_behind, src_rcvbl_type, src_rcvbl_hdr_id, created_by, 
            creation_date, last_update_by, last_update_date, auto_calc, incrs_dcrs1, 
            rvnu_acnt_id, incrs_dcrs2, rcvbl_acnt_id, appld_prepymnt_doc_id, 
            orgnl_line_id, validty_status, entrd_curr_id, func_curr_id, accnt_curr_id, 
            func_curr_rate, accnt_curr_rate, func_curr_amount, accnt_curr_amnt, initial_amnt_line_id, line_qty, unit_price) " +
                  "VALUES (" + smmryID + ", '" + lineType.Replace("'", "''") +
                  "', '" + lineDesc.Replace("'", "''") +
                  "', " + entrdAmnt +
                  ", " + codeBhnd +
                  ", '" + docType.Replace("'", "''") +
                  "', " + hdrID +
                  ", " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(autoCalc) +
                  "', '" + incrDcrs1.Replace("'", "''") +
                  "', " + costngID +
                  ", '" + incrDcrs2.Replace("'", "''") +
                  "', " + blncgAccntID +
                  ", " + prepayDocHdrID +
                  ", " + orgnlLnID +
                  ", '" + vldyStatus.Replace("'", "''") +
                  "', " + entrdCurrID +
                  ", " + funcCurrID +
                  ", " + accntCurrID +
                  ", " + funcCurrRate +
                  ", " + accntCurrRate +
                  ", " + funcCurrAmnt +
                  ", " + accntCurrAmnt +
                  ", " + initAmntLineID +
                  ", " + lineQty +
                  ", " + unitPrice + ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtRcvblsDocDet(long docDetID, long hdrID, string lineType, string lineDesc,
          double entrdAmnt, int entrdCurrID, int codeBhnd, string docType,
          bool autoCalc, string incrDcrs1, int costngID, string incrDcrs2, int blncgAccntID,
          long prepayDocHdrID, string vldyStatus, long orgnlLnID,
          int funcCurrID, int accntCurrID, double funcCurrRate, double accntCurrRate,
          double funcCurrAmnt, double accntCurrAmnt, long initAmntLineID, double lineQty, double unitPrice)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_rcvbl_amnt_smmrys
   SET rcvbl_smmry_type='" + lineType.Replace("'", "''") +
                  "', rcvbl_smmry_desc='" + lineDesc.Replace("'", "''") +
                  "', rcvbl_smmry_amnt=" + entrdAmnt +
                  ", code_id_behind=" + codeBhnd +
                  ", src_rcvbl_type='" + docType.Replace("'", "''") +
                  "', src_rcvbl_hdr_id=" + hdrID +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', auto_calc='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(autoCalc) +
                  "', incrs_dcrs1='" + incrDcrs1.Replace("'", "''") +
                  "', rvnu_acnt_id=" + costngID +
                  ", incrs_dcrs2='" + incrDcrs2.Replace("'", "''") +
                  "', rcvbl_acnt_id=" + blncgAccntID +
                  ", appld_prepymnt_doc_id=" + prepayDocHdrID +
                  ", validty_status='" + vldyStatus.Replace("'", "''") +
                  "', orgnl_line_id=" + orgnlLnID +
                  ", entrd_curr_id=" + entrdCurrID +
                  ", func_curr_id=" + funcCurrID +
                  ", accnt_curr_id=" + accntCurrID +
                  ", func_curr_rate=" + funcCurrRate +
                  ", accnt_curr_rate=" + accntCurrRate +
                  ", func_curr_amount=" + funcCurrAmnt +
                  ", accnt_curr_amnt=" + accntCurrAmnt +
                  ", initial_amnt_line_id=" + initAmntLineID +
                  ", line_qty=" + lineQty +
                  ", unit_price=" + unitPrice +
                  " WHERE rcvbl_smmry_id = " + docDetID;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void deleteRcvblsDocHdrNDet(long valLnid, string docNum)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Document Number = " + docNum;
            string delSQL = "DELETE FROM accb.accb_rcvbl_amnt_smmrys WHERE src_rcvbl_hdr_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
            delSQL = "DELETE FROM accb.accb_rcvbls_invc_hdr WHERE rcvbls_invc_hdr_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteRcvblsDocDet(long valLnid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSQL = "DELETE FROM accb.accb_rcvbl_amnt_smmrys WHERE rcvbl_smmry_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static DataSet get_One_RcvblsDocHdr(long hdrID)
        {
            string strSql = "";

            strSql = @"SELECT rcvbls_invc_hdr_id, to_char(to_timestamp(rcvbls_invc_date,'YYYY-MM-DD'),'DD-Mon-YYYY'), 
       created_by, sec.get_usr_name(a.created_by), rcvbls_invc_number, rcvbls_invc_type, 
       comments_desc, src_doc_hdr_id, customer_id, scm.get_cstmr_splr_name(a.customer_id),
       customer_site_id, scm.get_cstmr_splr_site_name(a.customer_site_id), 
       approval_status, next_aproval_action, invoice_amount, 
       payment_terms, src_doc_type, pymny_method_id, accb.get_pymnt_mthd_name(a.pymny_method_id), 
       amnt_paid, gl_batch_id, accb.get_gl_batch_name(a.gl_batch_id),
       cstmrs_doc_num, doc_tmplt_clsfctn, invc_curr_id, gst.get_pssbl_val(a.invc_curr_id), 
  scm.get_src_doc_num(a.src_doc_hdr_id, a.src_doc_type),
        event_rgstr_id, evnt_cost_category, event_doc_type   
  FROM accb.accb_rcvbls_invc_hdr a " +
              "WHERE((a.rcvbls_invc_hdr_id = " + hdrID + "))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_RcvblsDocHdr(string searchWord, string searchIn, long offset,
          int limit_size, long orgID, bool shwUnpstdOnly)
        {
            string strSql = "";
            string whrcls = "";
            /*Document Number
         Document Description
         Document Classification
         Customer Name
         Customer's Doc. Number
         Source Doc Number
         Approval Status
         Created By
         Currency*/
            string unpstdCls = "";
            if (shwUnpstdOnly)
            {
                unpstdCls = " AND (round(a.invoice_amount-a.amnt_paid,2)>0 or a.approval_status IN ('Not Validated','Validated','Reviewed'))";
                // AND (a.approval_status='Approved')
                //        unpstdCls = @" AND EXISTS (SELECT f.src_rcvbl_hdr_id 
                //FROM accb.accb_rcvbl_amnt_smmrys f WHERE f.rcvbl_smmry_type='8Outstanding Balance' 
                //and round(f.rcvbl_smmry_amnt,2)>0 and a.rcvbls_invc_hdr_id=f.src_rcvbl_hdr_id and f.src_rcvbl_type=a.rcvbls_invc_type)";
                //unpstdCls = " AND (a.approval_status!='Approved')";
            }
            if (searchIn == "Document Number")
            {
                whrcls = " and (a.rcvbls_invc_number ilike '" + searchWord.Replace("'", "''") + "' or trim(to_char(a.rcvbls_invc_hdr_id, '99999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            else if (searchIn == "Document Description")
            {
                whrcls = " and (a.comments_desc ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Document Classification")
            {
                whrcls = " and (a.doc_tmplt_clsfctn ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Customer Name")
            {
                whrcls = @" and (a.customer_id IN (select c.cust_sup_id from 
scm.scm_cstmr_suplr c where c.cust_sup_name ilike '" + searchWord.Replace("'", "''") +
            "'))";
            }
            else if (searchIn == "Customer's Doc. Number")
            {
                whrcls = " and (a.cstmrs_doc_num ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Source Doc Number")
            {
                whrcls = @" and (a.src_doc_hdr_id IN (select d.invc_hdr_id from scm.scm_sales_invc_hdr d 
where d.invc_number ilike '" + searchWord.Replace("'", "''") +
            @"') or a.src_doc_hdr_id IN (select f.rcvbls_invc_hdr_id from accb.accb_rcvbls_invc_hdr f
where f.rcvbls_invc_number ilike '" + searchWord.Replace("'", "''") +
            @"'))";
            }
            else if (searchIn == "Approval Status")
            {
                whrcls = " and (a.approval_status ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Created By")
            {
                whrcls = " and (sec.get_usr_name(a.created_by) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Currency")
            {
                whrcls = " and (gst.get_pssbl_val(a.invc_curr_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            strSql = @"SELECT rcvbls_invc_hdr_id, rcvbls_invc_number, 
rcvbls_invc_type, round(a.invoice_amount-a.amnt_paid,2),
 a.approval_status
        FROM accb.accb_rcvbls_invc_hdr a 
        WHERE((a.org_id = " + orgID + ")" + whrcls + unpstdCls +
              ") ORDER BY rcvbls_invc_hdr_id DESC LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.rcvblsFrm.rec_SQL = strSql;
            return dtst;
        }

        public static long get_Total_RcvblsDoc(string searchWord, string searchIn, long orgID, bool shwUnpstdOnly)
        {
            string strSql = "";
            string whrcls = "";
            /*Document Number
         Document Description
         Document Classification
         Customer Name
         Customer's Doc. Number
         Source Doc Number
         Approval Status
         Created By
         Currency*/
            string unpstdCls = "";
            if (shwUnpstdOnly)
            {
                unpstdCls = " AND (round(a.invoice_amount-a.amnt_paid,2)>0 or a.approval_status IN ('Not Validated','Validated','Reviewed'))";
            }
            if (searchIn == "Document Number")
            {
                whrcls = " and (a.rcvbls_invc_number ilike '" + searchWord.Replace("'", "''") + "' or trim(to_char(a.rcvbls_invc_hdr_id, '99999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            else if (searchIn == "Document Description")
            {
                whrcls = " and (a.comments_desc ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Document Classification")
            {
                whrcls = " and (a.doc_tmplt_clsfctn ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Customer Name")
            {
                whrcls = @" and (a.customer_id IN (select c.cust_sup_id from 
scm.scm_cstmr_suplr c where c.cust_sup_name ilike '" + searchWord.Replace("'", "''") +
            "'))";
            }
            else if (searchIn == "Customer's Doc. Number")
            {
                whrcls = " and (a.cstmrs_doc_num ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Source Doc Number")
            {
                whrcls = @" and (a.src_doc_hdr_id IN (select d.invc_hdr_id from scm.scm_sales_invc_hdr d 
where d.invc_number ilike '" + searchWord.Replace("'", "''") +
            @"') or a.src_doc_hdr_id IN (select f.rcvbls_invc_hdr_id from accb.accb_rcvbls_invc_hdr f
where f.rcvbls_invc_number ilike '" + searchWord.Replace("'", "''") +
            @"'))";
            }
            else if (searchIn == "Approval Status")
            {
                whrcls = " and (a.approval_status ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Created By")
            {
                whrcls = " and (sec.get_usr_name(a.created_by) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Currency")
            {
                whrcls = " and (gst.get_pssbl_val(a.invc_curr_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            strSql = @"SELECT count(1) 
        FROM accb.accb_rcvbls_invc_hdr a 
        WHERE((a.org_id = " + orgID + ")" + whrcls + unpstdCls + ")";


            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static DataSet get_RcvblsDocDet(long docHdrID)
        {
            string strSql = "";
            string whrcls = @" and (a.rcvbl_smmry_type !='6Grand Total' and 
a.rcvbl_smmry_type !='7Total Payments Made' and a.rcvbl_smmry_type !='8Outstanding Balance')";
            //if (aprvlStatus != "Not Validated")
            //{
            //  //whrcls = "";, string aprvlStatus
            //}
            strSql = @"SELECT rcvbl_smmry_id, rcvbl_smmry_type, rcvbl_smmry_desc, rcvbl_smmry_amnt, 
       code_id_behind, auto_calc, incrs_dcrs1, 
       rvnu_acnt_id, incrs_dcrs2, rcvbl_acnt_id, appld_prepymnt_doc_id, 
       entrd_curr_id, gst.get_pssbl_val(a.entrd_curr_id), 
       func_curr_id, gst.get_pssbl_val(a.func_curr_id), 
      accnt_curr_id, gst.get_pssbl_val(a.accnt_curr_id), 
      func_curr_rate, accnt_curr_rate, 
       func_curr_amount, accnt_curr_amnt, initial_amnt_line_id, 
        REPLACE(REPLACE(a.rcvbl_smmry_type,'2Tax','3Tax'),'3Discount','2Discount') smtyp,
        line_qty, unit_price  
  FROM accb.accb_rcvbl_amnt_smmrys a " +
              "WHERE((a.src_rcvbl_hdr_id = " + docHdrID + ")" + whrcls + ") ORDER BY 23 ASC ";

            //MessageBox.Show(strSql);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.rcvblsFrm.recDt_SQL = strSql;
            return dtst;
        }

        public static bool isTaxWthHldng(int codeID)
        {
            string strSql = "Select scm.istaxwthhldng(" + codeID + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);

            if (dtst.Tables[0].Rows.Count > 0)
            {
                if (dtst.Tables[0].Rows[0][0].ToString() == "1")
                {
                    return true;
                }
            }
            return false;
        }

        public static double getRcvblsDocGrndAmnt(long dochdrID)
        {
            string strSql = @"select SUM(CASE WHEN y.rcvbl_smmry_type = '3Discount' 
or scm.istaxwthhldng(y.code_id_behind)='1' or y.rcvbl_smmry_type='5Applied Prepayment'
      THEN -1*y.rcvbl_smmry_amnt ELSE y.rcvbl_smmry_amnt END) amnt " +
              "from accb.accb_rcvbl_amnt_smmrys y " +
              "where y.src_rcvbl_hdr_id = " + dochdrID +
              " and y.rcvbl_smmry_type IN ('1Initial Amount','2Tax','3Discount','4Extra Charge','5Applied Prepayment')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double rs = 0;

            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
            }
            return rs;
        }

        public static int getRcvblsPrepayDocCnt(long dochdrID)
        {
            string strSql = @"select count(appld_prepymnt_doc_id) " +
              "from accb.accb_rcvbl_amnt_smmrys y " +
              "where y.src_rcvbl_hdr_id = " + dochdrID + " and y.appld_prepymnt_doc_id >0 " +
              "Group by y.appld_prepymnt_doc_id having count(y.appld_prepymnt_doc_id)>1";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            int rs = 0;

            if (dtst.Tables[0].Rows.Count > 0)
            {
                int.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
                return rs;
            }
            return 0;
        }



        public static bool isRcvblPrepayDocValid(long dochdrID, int crncyID, long cstmrID)
        {
            string strSql = @"select rcvbls_invc_hdr_id " +
              "from accb.accb_rcvbls_invc_hdr y " +
              "where y.rcvbls_invc_hdr_id = " + dochdrID +
              " and y.customer_id =" + cstmrID +
              " and y.invc_curr_id = " + crncyID;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);

            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static double getRcvblsDocFuncAmnt(long dochdrID)
        {
            string strSql = @"select SUM(CASE WHEN y.rcvbl_smmry_type='3Discount' 
or scm.istaxwthhldng(y.code_id_behind)='1' or y.rcvbl_smmry_type='5Applied Prepayment'
      THEN -1*y.func_curr_amount ELSE y.func_curr_amount END) amnt " +
              "from accb.accb_rcvbl_amnt_smmrys y " +
              "where y.src_rcvbl_hdr_id=" + dochdrID +
              " and y.rcvbl_smmry_type IN ('1Initial Amount','2Tax','3Discount','4Extra Charge','5Applied Prepayment')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double rs = 0;

            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
            }
            return rs;
        }

        public static double getRcvblsDocAccntAmnt(long dochdrID)
        {
            string strSql = @"select SUM(CASE WHEN y.rcvbl_smmry_type='3Discount' 
or scm.istaxwthhldng(y.code_id_behind)='1' or y.rcvbl_smmry_type='5Applied Prepayment'
      THEN -1*y.accnt_curr_amnt ELSE y.accnt_curr_amnt END) amnt " +
              "from accb.accb_rcvbl_amnt_smmrys y " +
              "where y.src_rcvbl_hdr_id=" + dochdrID +
              " and y.rcvbl_smmry_type IN ('1Initial Amount','2Tax','3Discount','4Extra Charge','5Applied Prepayment')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double rs = 0;

            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
            }
            return rs;
        }

        public static long getRcvblsSmmryItmID(string smmryType, int codeBhnd,
          long srcDocID, string srcDocTyp, string smmryNm)
        {
            string strSql = "select y.rcvbl_smmry_id " +
              "from accb.accb_rcvbl_amnt_smmrys y " +
              "where y.rcvbl_smmry_type= '" + smmryType + "' and y.rcvbl_smmry_desc = '" + smmryNm +
              "' and y.code_id_behind= " + codeBhnd +
              " and y.src_rcvbl_type='" + srcDocTyp.Replace("'", "''") +
              "' and y.src_rcvbl_hdr_id=" + srcDocID + " ";
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

        public static void updtRcvblsDocApprvl(long docid,
      string apprvlSts, string nxtApprvl)
        {
            string extrCls = "";

            if (apprvlSts == "Cancelled")
            {
                extrCls = ", invoice_amount=0, invc_amnt_appld_elswhr=0";
            }
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_rcvbls_invc_hdr SET " +
                  "approval_status='" + apprvlSts.Replace("'", "''") +
                  "', last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', next_aproval_action='" + nxtApprvl.Replace("'", "''") +
                  "'" + extrCls + " WHERE (rcvbls_invc_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtRcvblsDocGLBatch(long docid,
      long glBatchID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_rcvbls_invc_hdr SET " +
                  "gl_batch_id=" + glBatchID +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (rcvbls_invc_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtRcvblsDocAmntPaid(long docid,
      double amntPaid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_rcvbls_invc_hdr SET " +
                  "amnt_paid=amnt_paid + " + amntPaid +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (rcvbls_invc_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }
        public static void updtRcvblsHdrAmntPaid(long docid,
     double amntPaid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_rcvbls_invc_hdr SET " +
                  "amnt_paid= + " + amntPaid +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (rcvbls_invc_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtRcvblsDocAmnt(long docid, double invAmnt)
        {
            string extrCls = ", invoice_amount=" + invAmnt + "";

            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_rcvbls_invc_hdr SET " +
                  "last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "'" + extrCls + " WHERE (rcvbls_invc_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }
        public static void updtRcvblsDocAmntAppld(long docid,
      double amntAppld)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_rcvbls_invc_hdr SET " +
                  "invc_amnt_appld_elswhr=invc_amnt_appld_elswhr + " + amntAppld +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (rcvbls_invc_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static double getRcvblsDocTtlPymnts(long dochdrID, string docType)
        {
            string strSql = "select SUM(y.amount_paid) amnt " +
              "from accb.accb_payments y " +
              "where y.src_doc_id = " + dochdrID + " and y.src_doc_typ = '" + docType.Replace("'", "''") + "'";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double rs = 0;

            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
            }
            return rs;
        }

        #endregion

        #region "SALES"
        public static void createSmmryItm(string smmryTyp,
         string smmryNm, double amnt, int codeBehind, string srcDocTyp,
         long srcDocHdrID, bool autoCalc)
        {
            if (smmryTyp == "3Discount")
            {
                amnt = -1 * Math.Abs(amnt);
            }
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO scm.scm_doc_amnt_smmrys(" +
                  "smmry_type, smmry_name, smmry_amnt, code_id_behind, " +
                  "src_doc_type, src_doc_hdr_id, created_by, creation_date, last_update_by, " +
                  "last_update_date, auto_calc) " +
                  "VALUES ('" + smmryTyp.Replace("'", "''") +
                  "', '" + smmryNm.Replace("'", "''") +
                  "', " + amnt + ", " + codeBehind + ", '" + srcDocTyp.Replace("'", "''") +
                  "', " + srcDocHdrID + ", " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr + "', '" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(autoCalc) + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updateSmmryItm(long smmryID, string smmryTyp,
         double amnt, bool autoCalc, string smmryNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            if (smmryTyp == "3Discount")
            {
                amnt = -1 * Math.Abs(amnt);
            }
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE scm.scm_doc_amnt_smmrys SET " +
                  "smmry_amnt = " + amnt +
                  ", last_update_by = " + Global.myBscActn.user_id + ", " +
                  "auto_calc = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(autoCalc) +
                  "', last_update_date = '" + dateStr +
                  "', smmry_name='" + smmryNm.Replace("'", "''") + "' WHERE (smmry_id = " + smmryID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static long getSalesDocLnID(int itmID,
          int storeID, long srcDocID)
        {
            string strSql = "select y.invc_det_ln_id " +
              "from scm.scm_sales_invc_det y " +
              "where y.itm_id= " + itmID +
              " and y.store_id=" + storeID +
              " and y.invc_hdr_id=" + srcDocID + " ";
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

        public static long getSalesSmmryItmID(string smmryType, int codeBhnd,
         long srcDocID, string srcDocTyp)
        {
            string strSql = "select y.smmry_id " +
              "from scm.scm_doc_amnt_smmrys y " +
              "where y.smmry_type= '" + smmryType + "' and y.code_id_behind= " + codeBhnd +
              " and y.src_doc_type='" + srcDocTyp +
              "' and y.src_doc_hdr_id=" + srcDocID + " ";
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

        //public static double getSalesDocFnlGrndAmnt(long dochdrID, string docTyp)
        //{
        //  string strSql = "select SUM(y.smmry_amnt) amnt " +
        //    "from scm.scm_doc_amnt_smmrys y " +
        //    "where y.src_doc_hdr_id=" + dochdrID +
        //    " and y.src_doc_type='" + docTyp + "' and y.smmry_type != '1Initial Amount' " +
        //    " and y.smmry_type != '6Total Payments Received' and y.smmry_type != " +
        //    "'7Change/Balance' and smmry_type!='4Extra Charge' and smmry_type!='2Tax'";
        //  DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
        //  double rs = 0;

        //  if (dtst.Tables[0].Rows.Count > 0)
        //  {
        //    double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
        //  }
        //  return rs;
        //}

        //public static double getSalesDocBscAmnt(long dochdrID, string docTyp)
        //{
        //  string strSql = "select SUM(CASE WHEN (smmry_type='2Tax') THEN -1*y.smmry_amnt ELSE y.smmry_amnt END) amnt " +
        //    "from scm.scm_doc_amnt_smmrys y " +
        //    "where y.src_doc_hdr_id=" + dochdrID +
        //    " and y.src_doc_type='" + docTyp + "' and substr(y.smmry_type,1,1) IN ('2','5')";
        //  DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
        //  double rs = 0;

        //  if (dtst.Tables[0].Rows.Count > 0)
        //  {
        //    double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
        //  }
        //  return rs;
        //}

        public static double getSalesDocCodesAmnt(int codeID, double unitAmnt, double qnty)
        {
            string codeSQL = Global.mnFrm.cmCde.getGnrlRecNm("scm.scm_tax_codes",
              "code_id", "sql_formular", codeID);
            codeSQL = codeSQL.Replace("{:qty}", qnty.ToString()).Replace("{:unit_price}", unitAmnt.ToString());
            if (codeSQL != "")
            {
                DataSet d1 = Global.mnFrm.cmCde.selectDataNoParams(codeSQL);
                double rs1 = 0;

                if (d1.Tables[0].Rows.Count > 0)
                {
                    double.TryParse(d1.Tables[0].Rows[0][0].ToString(), out rs1);
                }
                return rs1 * qnty;
            }
            else
            {
                return 0.00;
            }
        }

        public static double getSalesDocGrndAmnt(long dochdrID)
        {
            string strSql = "select SUM(y.doc_qty*orgnl_selling_price) amnt " +
             "from scm.scm_sales_invc_det y " +
             "where y.invc_hdr_id=" + dochdrID + " ";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double rs = 0;

            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
            }
            return rs;
        }

        public static double getSalesDocRcvdPymnts(long dochdrID, string docType)
        {
            string strSql = "select SUM(y.amount_paid) amnt " +
              "from scm.scm_payments y " +
              "where y.src_doc_id=" + dochdrID + " and y.src_doc_typ = '" + docType.Replace("'", "''") + "'";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double rs = 0;

            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
            }
            return rs;
        }

        public static double get_One_AvlblSrcLnQty(long srcLnID)
        {
            string strSql = "SELECT (a.doc_qty - a.qty_trnsctd_in_dest_doc) avlbl_qty " +
             "FROM scm.scm_sales_invc_det a " +
             "WHERE(a.invc_det_ln_id = " + srcLnID +
             ") ORDER BY a.invc_det_ln_id";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double rs = 0;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
            }
            return rs;
        }

        public static double get_One_LnTrnsctdQty(long dochdrID, long srcLnID)
        {
            string strSql = "SELECT SUM(a.doc_qty) trnsctd_qty " +
             "FROM scm.scm_sales_invc_det a " +
             "WHERE(a.invc_hdr_id IN(select b.invc_hdr_id " +
             "from scm.scm_sales_invc_hdr b where b.src_doc_hdr_id = " + dochdrID +
             " and b.src_doc_hdr_id>0) and a.src_line_id = "
             + srcLnID + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double rs = 0;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out rs);
            }
            return rs;
        }

        public static DataSet get_One_SalesDcLines(long dochdrID)
        {
            string strSql = "SELECT a.invc_det_ln_id, a.itm_id, " +
              "a.doc_qty, a.unit_selling_price, (a.doc_qty * a.unit_selling_price) amnt, " +
              "a.store_id, a.crncy_id, (a.doc_qty - a.qty_trnsctd_in_dest_doc) avlbl_qty, " +
              "a.src_line_id, a.tax_code_id, a.dscnt_code_id, a.chrg_code_id, a.rtrn_reason, " +
              "a.consgmnt_ids, a.orgnl_selling_price, b.base_uom_id " +
             "FROM scm.scm_sales_invc_det a, inv.inv_itm_list b " +
             "WHERE(a.invc_hdr_id = " + dochdrID +
             " and a.invc_hdr_id >0 and a.itm_id = b.item_id) ORDER BY a.invc_det_ln_id";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static void deleteSalesSmmryItm(long docID, string docType, string smmryTyp)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSQL = "DELETE FROM scm.scm_doc_amnt_smmrys WHERE src_doc_hdr_id = " +
              docID + " and src_doc_type = '" + docType + "' and smmry_type = '" + smmryTyp + "'";
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        #endregion

        #region "FIXED ASSETS..."
        public static long getNewAssetLnID()
        {
            //string strSql = "select nextval('accb.accb_trnsctn_batches_batch_id_seq'::regclass);";
            string strSql = "select nextval('accb.accb_fa_asset_trns_asset_trns_id_seq')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static long getNewAssetPMStpID()
        {
            //string strSql = "select nextval('accb.accb_trnsctn_batches_batch_id_seq'::regclass);";
            string strSql = "select nextval('accb.accb_fa_assets_pm_stps_asset_pm_stp_id_seq')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static long getNewAssetPMID()
        {
            //string strSql = "select nextval('accb.accb_trnsctn_batches_batch_id_seq'::regclass);";
            string strSql = "select nextval('accb.accb_fa_assets_pm_recs_asset_pm_rec_id_seq')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static long getAssetTrnsID(string trnsType, string trnsDte, string trnsDesc)
        {
            trnsDte = DateTime.ParseExact(trnsDte, "dd-MMM-yyyy HH:mm:ss",
      System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss").Substring(0, 10);

            string strSql = "select asset_trns_id from accb.accb_fa_asset_trns where trns_type='"
              + trnsType.Replace("'", "''") + "' and trns_date like '"
              + trnsDte.Replace("'", "''") + "%' and line_desc ilike '" + trnsDesc.Replace("'", "''") + "'";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static void createPM(long pmID, string measmtTyp,
    string uom, string recDate, double strtFig, double endFig,
          bool isPmDone, string pmActnTkn, string cmmnts, long assetID)
        {
            recDate = DateTime.ParseExact(recDate, "dd-MMM-yyyy HH:mm:ss",
      System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_fa_assets_pm_recs(
            asset_pm_rec_id, measurement_type, uom, record_date, starting_fig, 
            ending_fig, is_pm_done, exact_pm_action_done, comments_remarks, 
            created_by, creation_date, last_update_by, last_update_date, 
            asset_id) " +
                  "VALUES (" + pmID +
                  ", '" + measmtTyp.Replace("'", "''") +
                  "', '" + uom.Replace("'", "''") +
                  "', '" + recDate.Replace("'", "''") +
                  "', " + strtFig +
                  ", " + endFig +
                  ", '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isPmDone) +
                  "', '" + pmActnTkn.Replace("'", "''") +
                  "', '" + cmmnts.Replace("'", "''") +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + assetID + ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updatePM(long pmID, string measmtTyp,
    string uom, string recDate, double strtFig, double endFig,
          bool isPmDone, string pmActnTkn, string cmmnts, long assetID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            recDate = DateTime.ParseExact(recDate, "dd-MMM-yyyy HH:mm:ss",
      System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = @"UPDATE accb.accb_fa_assets_pm_recs
   SET measurement_type='" + measmtTyp.Replace("'", "''") +
                  "', uom='" + uom.Replace("'", "''") +
                  "', record_date='" + recDate.Replace("'", "''") +
                  "', starting_fig=" + strtFig +
                  ", ending_fig=" + endFig +
                  ", is_pm_done='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isPmDone) +
                  "', exact_pm_action_done='" + pmActnTkn.Replace("'", "''") +
                  "', comments_remarks='" + cmmnts.Replace("'", "''") +
                  "', created_by=" + Global.myBscActn.user_id +
                  ", creation_date='" + dateStr +
                  "', last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date=" + Global.myBscActn.user_id +
                  ", asset_id=" + assetID +
                  " WHERE asset_pm_rec_id = " + pmID;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void createPMStp(long pmStpID, string measmtTyp,
     string uom, double mxFigAllwd, double cumFigForPM, long assetID)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_fa_assets_pm_stps(
            asset_pm_stp_id, measurement_type, uom, max_daily_net_fig_allwd, 
            cmltv_fig_for_srvcng, created_by, creation_date, last_update_by, 
            last_update_date, asset_id) " +
                  "VALUES (" + pmStpID + ", '" + measmtTyp.Replace("'", "''") +
                  "', '" + uom.Replace("'", "''") +
                  "', " + mxFigAllwd +
                  ", " + cumFigForPM +
                  ", " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + assetID + ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updatePMStp(long pmStpID, string measmtTyp,
     string uom, double mxFigAllwd, double cumFigForPM, long assetID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = @"UPDATE accb.accb_fa_assets_pm_stps
            SET measurement_type='" + measmtTyp.Replace("'", "''") +
                  "', uom='" + uom.Replace("'", "''") +
                  "', max_daily_net_fig_allwd=" + mxFigAllwd +
                  ", cmltv_fig_for_srvcng=" + cumFigForPM +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE asset_pm_stp_id = " + pmStpID;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void createAssetHdr(int orgid, string strtDte, string enddte, string assetNum,
         string assetClsf, string assetDesc, string assetCtgry, int divGrpID, int siteID,
           string bldngLoc, string roomNum, long assetPrsn, string tagNum,
           string serialNum, string barCode, int assetAccnt, int deprAccnt,
           int expnsAccnt, int invItmID, string sqlFormula, double salvageVal, bool autoDprct)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            //Global.mnFrm.cmCde.showSQLNoPermsn(strtDte + "/" + enddte);
            strtDte = DateTime.ParseExact(strtDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            if (enddte == "")
            {
                enddte = "31-Dec-4000 23:59:59";
            }
            enddte = DateTime.ParseExact(enddte, "dd-MMM-yyyy HH:mm:ss",
      System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string insSQL = @"INSERT INTO accb.accb_fa_assets_rgstr(
            asset_code_name, asset_desc, asset_classification, 
            asset_category, asset_div_grp_id, asset_site_id, asset_building_loc, 
            asset_room_no, asset_caretaker, tag_number, serial_number, barcode, 
            asset_life_start_date, asset_life_end_date, asset_accnt_id, dpr_aprc_accnt_id, 
            expns_rvnu_accnt_id, created_by, creation_date, last_update_by, 
            last_update_date, inv_item_id, sql_formula, asset_salvage_value, 
            org_id, enbl_auto_dprctn) " +
                  "VALUES ('" + assetNum.Replace("'", "''") +
                  "', '" + assetDesc.Replace("'", "''") +
                  "', '" + assetClsf.Replace("'", "''") +
                  "', '" + assetCtgry.Replace("'", "''") +
                  "', " + divGrpID +
                  ", " + siteID +
                  ", '" + bldngLoc.Replace("'", "''") +
                  "', '" + roomNum.Replace("'", "''") +
                  "', " + assetPrsn +
                  ", '" + tagNum.Replace("'", "''") +
                  "', '" + serialNum.Replace("'", "''") +
                  "', '" + barCode.Replace("'", "''") +
                  "', '" + strtDte.Replace("'", "''") +
                  "', '" + enddte.Replace("'", "''") +
                  "', " + assetAccnt +
                  ", " + deprAccnt +
                  ", " + expnsAccnt +
                  ", " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + invItmID +
                  ", '" + sqlFormula.Replace("'", "''") +
                  "', " + salvageVal +
                  ", " + orgid + ", '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(autoDprct) + "')";
            //Global.mnFrm.cmCde.showSQLNoPermsn(insSQL);
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtAssetHdr(long hdrID, string strtDte, string enddte, string assetNum,
         string assetClsf, string assetDesc, string assetCtgry, int divGrpID, int siteID,
           string bldngLoc, string roomNum, long assetPrsn, string tagNum,
           string serialNum, string barCode, int assetAccnt, int deprAccnt,
           int expnsAccnt, int invItmID, string sqlFormula, double salvageVal, bool autoDprct)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            strtDte = DateTime.ParseExact(strtDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            if (enddte == "")
            {
                enddte = "31-Dec-4000 23:59:59";
            }
            enddte = DateTime.ParseExact(enddte, "dd-MMM-yyyy HH:mm:ss",
      System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_fa_assets_rgstr 
            SET asset_code_name = '" + assetNum.Replace("'", "''") +
                  "', asset_desc='" + assetDesc.Replace("'", "''") +
                  "', asset_classification='" + assetClsf.Replace("'", "''") +
                  "', asset_category='" + assetCtgry.Replace("'", "''") +
                  "', asset_div_grp_id=" + divGrpID +
                  ", asset_site_id=" + siteID +
                  ", asset_building_loc='" + bldngLoc.Replace("'", "''") +
                  "', asset_room_no='" + roomNum.Replace("'", "''") +
                  "', asset_caretaker=" + assetPrsn +
                  ", tag_number='" + tagNum.Replace("'", "''") +
                  "', serial_number='" + serialNum.Replace("'", "''") +
                  "', barcode='" + barCode.Replace("'", "''") +
                  "', asset_life_start_date='" + strtDte.Replace("'", "''") +
                  "', asset_life_end_date='" + enddte.Replace("'", "''") +
                  "', asset_accnt_id=" + assetAccnt +
                  ", dpr_aprc_accnt_id=" + deprAccnt +
                  ", expns_rvnu_accnt_id=" + expnsAccnt +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', inv_item_id=" + invItmID +
                  ", sql_formula='" + sqlFormula.Replace("'", "''") +
                  "', asset_salvage_value = " + salvageVal +
                  ", enbl_auto_dprctn = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(autoDprct) +
                  "' WHERE asset_id = " + hdrID;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void createAssetTrns(long assetTrnsID, long hdrID, string lineType, string lineDesc,
          double entrdAmnt, int entrdCurrID, string incrDcrs1, int costngID, string incrDcrs2, int blncgAccntID,
          int funcCurrID, double funcCurrRate, double funcCurrAmnt, string trnsDte)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            trnsDte = DateTime.ParseExact(trnsDte, "dd-MMM-yyyy HH:mm:ss",
      System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string insSQL = @"INSERT INTO accb.accb_fa_asset_trns(
            asset_trns_id, trns_type, incrs_dcrs1, cost_accnt_id, incrs_dcrs2, 
            bals_leg_accnt_id, created_by, creation_date, last_update_by, 
            last_update_date, asset_id, gl_batch_id, trns_date, trns_amount, 
            entrd_curr_id, func_curr_id, accnt_curr_id, func_curr_rate, accnt_curr_rate, 
            func_curr_amount, accnt_curr_amnt, line_desc) " +
                  "VALUES (" + assetTrnsID + ", '" + lineType.Replace("'", "''") +
                  "', '" + incrDcrs1.Replace("'", "''") +
                  "', " + costngID +
                  ", '" + incrDcrs2.Replace("'", "''") +
                  "', " + blncgAccntID +
                  ", " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + Global.myBscActn.user_id + ", '" + dateStr +
                  "', " + hdrID +
                  ", -1" +
                  ", '" + trnsDte.Replace("'", "''") +
                  "', " + entrdAmnt +
                  ", " + entrdCurrID +
                  ", " + funcCurrID +
                  ", " + entrdCurrID +
                  ", " + funcCurrRate +
                  ", 1" +
                  ", " + funcCurrAmnt +
                  ", " + entrdAmnt +
                  ", '" + lineDesc.Replace("'", "''") +
                  "')";
            //Global.mnFrm.cmCde.showSQLNoPermsn(insSQL);
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtAssetTrns(long assetTrnsID, long hdrID, string lineType, string lineDesc,
          double entrdAmnt, int entrdCurrID, string incrDcrs1, int costngID, string incrDcrs2, int blncgAccntID,
          int funcCurrID, double funcCurrRate, double funcCurrAmnt, string trnsDte)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            trnsDte = DateTime.ParseExact(trnsDte, "dd-MMM-yyyy HH:mm:ss",
      System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string insSQL = @"UPDATE accb.accb_fa_asset_trns SET 
              trns_type='" + lineType.Replace("'", "''") +
                  "', incrs_dcrs1='" + incrDcrs1.Replace("'", "''") +
                  "', cost_accnt_id=" + costngID +
                  ", incrs_dcrs2='" + incrDcrs2.Replace("'", "''") +
                  "', bals_leg_accnt_id=" + blncgAccntID +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "', trns_date='" + trnsDte.Replace("'", "''") +
                  "', trns_amount=" + entrdAmnt +
                  ", entrd_curr_id=" + entrdCurrID +
                  ", func_curr_id=" + funcCurrID +
                  ", accnt_curr_id=" + entrdCurrID +
                  ", func_curr_rate=" + funcCurrRate +
                  ", func_curr_amount=" + funcCurrAmnt +
                  ", accnt_curr_amnt=" + entrdAmnt +
                  ", line_desc='" + lineDesc.Replace("'", "''") +
                  "' WHERE asset_trns_id = " + assetTrnsID + " and gl_batch_id <= 0";

            //Global.mnFrm.cmCde.showSQLNoPermsn(insSQL);
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void deleteAssetHdrNDet(long valLnid, string docNum)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Asset Number = " + docNum;
            string delSQL = "DELETE FROM accb.accb_fa_asset_trns WHERE asset_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);

            delSQL = "DELETE FROM accb.accb_fa_assets_pm_stps WHERE asset_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);

            delSQL = "DELETE FROM accb.accb_fa_assets_pm_recs WHERE asset_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);

            delSQL = "DELETE FROM accb.accb_fa_assets_rgstr WHERE asset_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteAssetDet(long valLnid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSQL = "DELETE FROM accb.accb_fa_asset_trns WHERE asset_trns_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteAssetPMStp(long pmstpid, string assetNum)
        {
            //
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Asset Number = " + assetNum;
            string delSQL = "DELETE FROM accb.accb_fa_assets_pm_stps WHERE asset_pm_stp_id = " + pmstpid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteAssetPMRecs(long pmid, string assetNum)
        {
            //
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Asset Number = " + assetNum;
            string delSQL = "DELETE FROM accb.accb_fa_assets_pm_recs WHERE asset_pm_rec_id = " + pmid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static DataSet get_AssetPMRecs(string searchWord, string searchIn, long offset,
          int limit_size, long hdrID)
        {
            /*
             * Record Date
               Measurement Type/UOM
               PM Action Taken
             * Comments/Remarks
             */
            string strSql = "";
            string whrcls = "";
            if (searchIn == "Measurement Type/UOM")
            {
                whrcls = " and (a.measurement_type ilike '" + searchWord.Replace("'", "''") +
                  "' or a.uom ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            else if (searchIn == "PM Action Taken")
            {
                whrcls = " and (a.exact_pm_action_done ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Comments/Remarks")
            {
                whrcls = " and (a.comments_remarks ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Record Date")
            {
                whrcls = " and (to_char(to_timestamp(a.record_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") + "')";
            }

            strSql = @"SELECT asset_pm_rec_id, measurement_type, uom, 
to_char(to_timestamp(record_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
starting_fig, ending_fig, is_pm_done, 
exact_pm_action_done, comments_remarks, 
       asset_id 
  FROM accb.accb_fa_assets_pm_recs a " +
              "WHERE((a.asset_id = " + hdrID + ")" + whrcls +
              ") ORDER BY record_date DESC LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            //MessageBox.Show(strSql);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.fxdAstsFrm.pm_SQL = strSql;
            return dtst;
        }

        public static long get_TtlAssetPMRecs(string searchWord, string searchIn, long hdrID)
        {
            /*
             * Record Date
               Measurement Type/UOM
               PM Action Taken
             * Comments/Remarks
             */
            string strSql = "";
            string whrcls = "";
            if (searchIn == "Measurement Type/UOM")
            {
                whrcls = " and (a.measurement_type ilike '" + searchWord.Replace("'", "''") +
                  "' or a.uom ilike '" + searchWord.Replace("'", "''") +
                  "')";
            }
            else if (searchIn == "PM Action Taken")
            {
                whrcls = " and (a.exact_pm_action_done ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Comments/Remarks")
            {
                whrcls = " and (a.comments_remarks ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Record Date")
            {
                whrcls = " and (to_char(to_timestamp(a.record_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") + "')";
            }

            strSql = @"SELECT count(1) 
  FROM accb.accb_fa_assets_pm_recs a " +
              "WHERE((a.asset_id = " + hdrID + ")" + whrcls +
              ")";

            //MessageBox.Show(strSql);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static double getMxAllwdDailyFig(long asstID, string measTyp, string uom)
        {
            string strSql = @"SELECT max_daily_net_fig_allwd 
  FROM accb.accb_fa_assets_pm_stps a " +
              "WHERE((a.asset_id = " + asstID +
              ") and a.measurement_type = '" + measTyp.Replace("'", "''") +
              "' and a.uom = '" + uom.Replace("'", "''") +
              "') ORDER BY asset_pm_stp_id DESC LIMIT 1 OFFSET 0";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static double getCumFigForPM(long asstID, string measTyp, string uom)
        {
            string strSql = @"SELECT cmltv_fig_for_srvcng 
  FROM accb.accb_fa_assets_pm_stps a " +
              "WHERE((a.asset_id = " + asstID +
              ") and a.measurement_type = '" + measTyp.Replace("'", "''") +
              "' and a.uom = '" + uom.Replace("'", "''") +
              "') ORDER BY asset_pm_stp_id DESC LIMIT 1 OFFSET 0";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static double getSumPrevPMNetFigs(long asstID, string measTyp, string uom, string recDate)
        {
            recDate = DateTime.ParseExact(recDate, "dd-MMM-yyyy HH:mm:ss",
      System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string strSql = @"SELECT COALESCE(SUM(ending_fig - starting_fig),0) 
  FROM accb.accb_fa_assets_pm_recs a " +
              "WHERE((a.asset_id = " + asstID +
              ") and a.measurement_type = '" + measTyp.Replace("'", "''") +
              "' and a.uom = '" + uom.Replace("'", "''") +
              @"' and a.record_date>COALESCE((SELECT MAX(b.record_date) from accb.accb_fa_assets_pm_recs b where b.is_pm_done='1'),'0001-01-01 00:00:00')
            and a.record_date<'" + recDate + @"')";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static DataSet get_AssetTrns(string searchWord, string searchIn, long offset,
          int limit_size, long hdrID)
        {
            /*
             * Account Number/Description
      Transaction Description
      Transaction Date
             */
            string strSql = "";
            string whrcls = "";
            if (searchIn == "Account Number/Description")
            {
                whrcls = " and (accb.get_accnt_num(a.cost_accnt_id) ilike '" + searchWord.Replace("'", "''") +
                  "' or accb.get_accnt_num(a.bals_leg_accnt_id) ilike '" + searchWord.Replace("'", "''") +
                  "' or accb.get_accnt_name(a.cost_accnt_id) ilike '" + searchWord.Replace("'", "''") +
                  "' or accb.get_accnt_name(a.bals_leg_accnt_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Transaction Description")
            {
                whrcls = " and (a.line_desc ilike '" + searchWord.Replace("'", "''") +
                  "' or a.trns_type ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Transaction Date")
            {
                whrcls = " and (to_char(to_timestamp(trns_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") + "')";
            }

            strSql = @"SELECT asset_trns_id, trns_type, line_desc, trns_amount,
       entrd_curr_id, gst.get_pssbl_val(a.entrd_curr_id), 
       incrs_dcrs1, cost_accnt_id, 
       incrs_dcrs2, bals_leg_accnt_id, 
       gl_batch_id, to_char(to_timestamp(trns_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'),        
       func_curr_id, gst.get_pssbl_val(a.func_curr_id), 
       accnt_curr_id, gst.get_pssbl_val(a.accnt_curr_id), 
       func_curr_rate, accnt_curr_rate, 
       func_curr_amount, accnt_curr_amnt
  FROM accb.accb_fa_asset_trns a " +
              "WHERE((a.asset_id = " + hdrID + ")" + whrcls +
              ") ORDER BY trns_type ASC, trns_date ASC LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            //MessageBox.Show(strSql);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.fxdAstsFrm.recDt_SQL = strSql;
            return dtst;
        }

        public static long get_TtlAssetTrns(string searchWord, string searchIn, long hdrID)
        {
            /*
             * Account Number/Description
      Transaction Description
      Transaction Date
             */
            string strSql = "";
            string whrcls = "";
            if (searchIn == "Account Number/Description")
            {
                whrcls = " and (accb.get_accnt_num(a.cost_accnt_id) ilike '" + searchWord.Replace("'", "''") +
                  "' or accb.get_accnt_num(a.bals_leg_accnt_id) ilike '" + searchWord.Replace("'", "''") +
                  "' or accb.get_accnt_name(a.cost_accnt_id) ilike '" + searchWord.Replace("'", "''") +
                  "' or accb.get_accnt_name(a.bals_leg_accnt_id) ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Transaction Description")
            {
                whrcls = " and (a.line_desc ilike '" + searchWord.Replace("'", "''") +
                  "' or a.trns_type ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Transaction Date")
            {
                whrcls = " and (to_char(to_timestamp(trns_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") + "')";
            }

            strSql = @"SELECT count(1) 
  FROM accb.accb_fa_asset_trns a " +
              "WHERE((a.asset_id = " + hdrID + ")" + whrcls +
              ")";

            //MessageBox.Show(strSql);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static DataSet get_AssetsHdr(string searchWord, string searchIn, long offset,
          int limit_size, long orgID, bool shwNonZeroOnly)
        {
            string strSql = "";
            string whrcls = "";
            /*Asset Code/Tag/Serial
      Asset Description
      Classification/Category
      Location
      Caretaker*/
            string nonZeroCls = "";
            if (shwNonZeroOnly)
            {
                nonZeroCls = @" AND (round(accb.get_asset_trns_typ_amnt(a.asset_id,'1Initial Value')
+ accb.get_asset_trns_typ_amnt(a.asset_id,'3Appreciate Asset')
- accb.get_asset_trns_typ_amnt(a.asset_id,'2Depreciate Asset')
- accb.get_asset_trns_typ_amnt(a.asset_id,'4Retire Asset'),2)>0)";
            }
            if (searchIn == "Asset Code/Tag/Serial")
            {
                whrcls = " and (a.asset_code_name ilike '" + searchWord.Replace("'", "''") +
                  "' or a.tag_number ilike '" + searchWord.Replace("'", "''") +
                  "' or a.serial_number ilike '" + searchWord.Replace("'", "''") +
                  "' or a.barcode ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Asset Description")
            {
                whrcls = " and (a.asset_desc ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Classification/Category")
            {
                whrcls = " and (a.asset_classification ilike '" + searchWord.Replace("'", "''") +
                  "' or a.asset_category ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Location")
            {
                whrcls = @" and (org.get_div_name(a.asset_div_grp_id) ilike '" + searchWord.Replace("'", "''") +
                  "' or org.get_site_name(a.asset_site_id) ilike '" + searchWord.Replace("'", "''") +
                  "' or a.asset_building_loc ilike '" + searchWord.Replace("'", "''") +
                  "' or a.asset_room_no ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Caretaker")
            {
                whrcls = " and (prs.get_prsn_name(a.asset_caretaker) || ' ' || prs.get_prsn_loc_id(a.asset_caretaker) ilike '" + searchWord.Replace("'", "''") + "')";
            }

            strSql = @"SELECT a.asset_id, a.asset_code_name, 
        a.asset_desc, a.asset_classification 
        FROM accb.accb_fa_assets_rgstr a 
        WHERE((a.org_id = " + orgID + ")" + whrcls + nonZeroCls +
              ") ORDER BY asset_id DESC LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            Global.fxdAstsFrm.rec_SQL = strSql;
            return dtst;
        }

        public static long get_Total_AssetsHdr(string searchWord, string searchIn, long orgID, bool shwNonZeroOnly)
        {
            string strSql = "";
            string whrcls = "";
            /*Asset Code/Tag/Serial
      Asset Description
      Classification/Category
      Location
      Caretaker*/
            string nonZeroCls = "";
            if (shwNonZeroOnly)
            {
                nonZeroCls = @" AND (round(accb.get_asset_trns_typ_amnt(a.asset_id,'1Initial Value')
+ accb.get_asset_trns_typ_amnt(a.asset_id,'3Appreciate Asset')
- accb.get_asset_trns_typ_amnt(a.asset_id,'2Depreciate Asset')
- accb.get_asset_trns_typ_amnt(a.asset_id,'4Retire Asset'),2)>0)";
            }
            if (searchIn == "Asset Code/Tag/Serial")
            {
                whrcls = " and (a.asset_code_name ilike '" + searchWord.Replace("'", "''") +
                  "' or a.tag_number ilike '" + searchWord.Replace("'", "''") +
                  "' or a.serial_number ilike '" + searchWord.Replace("'", "''") +
                  "' or a.barcode ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Asset Description")
            {
                whrcls = " and (a.asset_desc ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Classification/Category")
            {
                whrcls = " and (a.asset_classification ilike '" + searchWord.Replace("'", "''") +
                  "' or a.asset_category ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Location")
            {
                whrcls = @" and (org.get_div_name(a.asset_div_grp_id) ilike '" + searchWord.Replace("'", "''") +
                  "' or org.get_site_name(a.asset_site_id) ilike '" + searchWord.Replace("'", "''") +
                  "' or a.asset_building_loc ilike '" + searchWord.Replace("'", "''") +
                  "' or a.asset_room_no ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Caretaker")
            {
                whrcls = " and (prs.get_prsn_name(a.asset_caretaker) || ' ' || prs.get_prsn_loc_id(a.asset_caretaker) ilike '" + searchWord.Replace("'", "''") + "')";
            }

            strSql = @"SELECT count(1) 
        FROM accb.accb_fa_assets_rgstr a 
        WHERE((a.org_id = " + orgID + ")" + whrcls + nonZeroCls + ")";


            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static DataSet get_One_AssetPMStps(long hdrID)
        {
            string strSql = "";

            strSql = @"SELECT asset_pm_stp_id, measurement_type, uom, max_daily_net_fig_allwd, 
       cmltv_fig_for_srvcng 
  FROM accb.accb_fa_assets_pm_stps a " +
              "WHERE((a.asset_id = " + hdrID + "))";

            Global.fxdAstsFrm.pmStps_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_One_AssetHdr(long hdrID)
        {
            string strSql = "";
            strSql = @"SELECT asset_id, asset_code_name, asset_desc, asset_classification, 
       asset_category, asset_div_grp_id, org.get_div_name(asset_div_grp_id), 
       asset_site_id, org.get_site_name(asset_site_id), asset_building_loc, 
       asset_room_no, asset_caretaker, 
       prs.get_prsn_name(asset_caretaker) || ' (' || prs.get_prsn_loc_id(asset_caretaker) || ')' fullnm, 
       tag_number, serial_number, barcode, 
       to_char(to_timestamp(asset_life_start_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') startdte, 
       to_char(to_timestamp(asset_life_end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') enddte, 
       asset_accnt_id, accb.get_accnt_num(asset_accnt_id) || '.' || accb.get_accnt_name(asset_accnt_id) assetacc,
       dpr_aprc_accnt_id, accb.get_accnt_num(dpr_aprc_accnt_id) || '.' || accb.get_accnt_name(dpr_aprc_accnt_id) depreacc,
       expns_rvnu_accnt_id, accb.get_accnt_num(expns_rvnu_accnt_id) || '.' || accb.get_accnt_name(expns_rvnu_accnt_id) expnsacc,
       inv_item_id, inv.get_invitm_name(inv_item_id), 
       sql_formula, asset_salvage_value, enbl_auto_dprctn
  FROM accb.accb_fa_assets_rgstr a " +
              "WHERE((a.asset_id = " + hdrID + "))";

            Global.fxdAstsFrm.recDt_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_One_AssetHdrNTrns(int lmit)
        {
            string extrWhr = "";

            if (lmit >= 0)
            {
                extrWhr = " LIMIT " + lmit + @" OFFSET 0";
            }
            else if (lmit < 0)
            {
                extrWhr = "";
            }

            string strSql = @"SELECT a.asset_id, a.asset_code_name, a.asset_desc, a.asset_classification, 
       a.asset_category, a.asset_div_grp_id, org.get_div_name(a.asset_div_grp_id), 
       a.asset_site_id, org.get_site_name(a.asset_site_id), a.asset_building_loc, 
       a.asset_room_no, a.asset_caretaker, 
       prs.get_prsn_loc_id(a.asset_caretaker), 
       a.tag_number, a.serial_number, a.barcode, 
       to_char(to_timestamp(a.asset_life_start_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') startdte, 
       to_char(to_timestamp(a.asset_life_end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') enddte, 
       a.asset_accnt_id, accb.get_accnt_num(a.asset_accnt_id) assetacc,
       dpr_aprc_accnt_id, accb.get_accnt_num(a.dpr_aprc_accnt_id) depreacc,
       expns_rvnu_accnt_id, accb.get_accnt_num(a.expns_rvnu_accnt_id) expnsacc,
       a.inv_item_id, inv.get_invitm_name(a.inv_item_id), 
       a.sql_formula, a.asset_salvage_value, 
       CASE WHEN a.enbl_auto_dprctn = '1' THEN 'YES' ELSE 'NO' END, 
       b.trns_type, b.line_desc, b.incrs_dcrs1, accb.get_accnt_num(b.cost_accnt_id), b.incrs_dcrs2, 
       accb.get_accnt_num(b.bals_leg_accnt_id), 
       to_char(to_timestamp(b.trns_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
       b.trns_amount, gst.get_pssbl_val(b.entrd_curr_id), b.func_curr_rate 
       FROM accb.accb_fa_assets_rgstr a, accb.accb_fa_asset_trns b " +
             @"WHERE(a.asset_id = b.asset_id) 
      ORDER BY a.asset_code_name, b.trns_type" + extrWhr;

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.taxFrm.rec_SQL = strSql;
            return dtst;
        }

        public static string get_InvItemNm(int itmID)
        {
            string strSql = "SELECT item_desc || ' (' || item_code || ')' " +
         "FROM inv.inv_itm_list a " +
         "WHERE item_id =" + itmID + "";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            return "";
        }

        public static int get_InvItemID(string itmNm)
        {
            string strSql = "SELECT item_id " +
         "FROM inv.inv_itm_list a " +
         "WHERE item_code ='" + itmNm.Replace("'", "''") + "' and org_id = " + Global.mnFrm.cmCde.Org_id;

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static double getPtyCshTrnsSumUsngStatus(long accntID, string trnsStatus)
        {
            string strSql = "";
            strSql = @"select COALESCE(SUM(a.dbt_amount-a.crdt_amount),0) from accb.accb_trnsctn_details a, 
accb.accb_chart_of_accnts b where a.accnt_id = b.accnt_id and a.trns_status = '" + trnsStatus.Replace("'", "''") + @"' and a.accnt_id = " + accntID;

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams1(strSql);
            if (dtst.Tables.Count <= 0)
            {
                return 0;
            }
            else if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }

        public static double getAssetTrnsTypeSum(long assetID, string trnsType)
        {
            string strSql = "";
            strSql = "SELECT accb.get_asset_trns_typ_amnt(" + assetID + ",'" + trnsType.Replace("'", "''") + "')";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams1(strSql);
            if (dtst.Tables.Count <= 0)
            {
                return 0;
            }
            else if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }

        public static void updtAssetTrnsGLBatch(long asstTrnsID,
     long glBatchID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_fa_asset_trns SET " +
                  "gl_batch_id=" + glBatchID +
                  ", last_update_by=" + Global.myBscActn.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (asset_trns_id = " +
                  asstTrnsID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static string computeCrrntAge(string dateOB)
        {
            if (dateOB == "")
            {
                return "";
            }

            string strSql = "";
            strSql = "SELECT extract('years' from age(now(), to_timestamp('" + dateOB + "', 'DD-Mon-YYYY'))) || ' yr(s) ' " +
              "|| extract('months' from age(now(), to_timestamp('" + dateOB + "', 'DD-Mon-YYYY'))) || ' mon(s) ' " +
              "|| extract('days' from age(now(), to_timestamp('" + dateOB + "', 'DD-Mon-YYYY'))) || ' day(s) ' ";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams1(strSql);
            if (dtst.Tables.Count <= 0)
            {
                return "";
            }
            else if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            return "";
        }

        public static string computeLifeSpan(string strtDte, string endDte)
        {
            if (endDte == "")
            {
                endDte = "31-Dec-4000 23:59:59";
            }
            if (strtDte == "" || endDte == "")
            {
                return "";
            }
            string strSql = "";
            strSql = "SELECT extract('years' from age(to_timestamp('" + endDte + "', 'DD-Mon-YYYY HH24:MI:SS'), to_timestamp('" + strtDte + "', 'DD-Mon-YYYY HH24:MI:SS'))) || ' yr(s) ' " +
              "|| extract('months' from age(to_timestamp('" + endDte + "', 'DD-Mon-YYYY HH24:MI:SS'), to_timestamp('" + strtDte + "', 'DD-Mon-YYYY HH24:MI:SS'))) || ' mon(s) ' " +
              "|| extract('days' from age(to_timestamp('" + endDte + "', 'DD-Mon-YYYY HH24:MI:SS'), to_timestamp('" + strtDte + "', 'DD-Mon-YYYY HH24:MI:SS'))) || ' day(s) ' ";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams1(strSql);
            if (dtst.Tables.Count <= 0)
            {
                return "";
            }
            else if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            return "";
        }

        #endregion
        #endregion
        #endregion

        #region "CUSTOM FUNCTIONS..."
        public static void createRqrdLOVs()
        {
            string[] sysLovs = { "Control Accounts", "Transactions not Allowed Days",
                           "Transactions not Allowed Dates", "Account Transaction Templates",
                           "Currencies","Payment Document Templates","Payment Methods",
                           "Supplier Prepayments","Supplier Debit Memos","Supplier Standard Payments",
                           "Customer Prepayments","Customer Credit Memos","Customer Standard Payments" };
            string[] sysLovsDesc = { "Control Accounts", "Transactions not Allowed Days",
                               "Transactions not Allowed Dates", "Account Transaction Templates",
                               "Currencies", "Payment Document Templates", "Payment Methods",
                               "Supplier Prepayments","Supplier Debit Memos","Supplier Standard Payments",
                           "Customer Prepayments","Customer Credit Memos","Customer Standard Payments" };
            string[] sysLovsDynQrys = { "select distinct trim(to_char(accnt_id,'999999999999999999999999999999')) a, accnt_num || '.' || accnt_name b, '' c, org_id d, accnt_type e, accnt_num f from accb.accb_chart_of_accnts where (has_sub_ledgers = '1' and is_enabled = '1') order by accnt_num",
                                "","",
                            @"SELECT distinct trim(to_char(z.template_id,'999999999999999999999999999999')) a, z.template_name b,'' c, z.org_id d, trim(to_char(w.user_id,'999999999999999999999999999999')) e
                            FROM accb.accb_trnsctn_templates_hdr z 
                            LEFT OUTER JOIN accb.accb_trnsctn_templates_usrs w
                            ON ((z.template_id=w.template_id) and (now() between to_timestamp(w.valid_start_date,'YYYY-MM-DD HH24:MI:SS')
                            AND to_timestamp(w.valid_end_date,'YYYY-MM-DD HH24:MI:SS')))
                            ORDER BY z.template_name","",
                            "select distinct trim(to_char(doc_tmplts_hdr_id,'999999999999999999999999999999')) a, doc_tmplt_name b, '' c, org_id d, doc_type e from accb.accb_doc_tmplts_hdr where (is_enabled = '1') order by doc_tmplt_name",
                            "select distinct trim(to_char(paymnt_mthd_id,'999999999999999999999999999999')) a, pymnt_mthd_name b, '' c, org_id d, supported_doc_type e from accb.accb_paymnt_mthds where (is_enabled = '1') order by pymnt_mthd_name",
                            "select distinct trim(to_char(pybls_invc_hdr_id,'999999999999999999999999999999')) a, pybls_invc_number b, '' c, org_id d, trim(to_char(supplier_id,'999999999999999999999999999999')) e, trim(to_char(invc_curr_id,'999999999999999999999999999999')) f, pybls_invc_hdr_id g from accb.accb_pybls_invc_hdr where (((pybls_invc_type = 'Supplier Advance Payment' and (invoice_amount-amnt_paid)<=0) or pybls_invc_type = 'Supplier Credit Memo (InDirect Refund)') and approval_status='Approved' and (invoice_amount-invc_amnt_appld_elswhr)>0) order by pybls_invc_hdr_id DESC",
                            "select distinct trim(to_char(pybls_invc_hdr_id,'999999999999999999999999999999')) a, pybls_invc_number b, '' c, org_id d, trim(to_char(supplier_id,'999999999999999999999999999999')) e, trim(to_char(invc_curr_id,'999999999999999999999999999999')) f, pybls_invc_hdr_id g from accb.accb_pybls_invc_hdr where ((pybls_invc_type = 'Supplier Debit Memo (InDirect Topup)') and approval_status='Approved' and (invoice_amount-invc_amnt_appld_elswhr)>0) order by pybls_invc_hdr_id DESC",
                            "select distinct trim(to_char(pybls_invc_hdr_id,'999999999999999999999999999999')) a, pybls_invc_number b, '' c, org_id d, trim(to_char(supplier_id,'999999999999999999999999999999')) e, trim(to_char(invc_curr_id,'999999999999999999999999999999')) f, pybls_invc_hdr_id g from accb.accb_pybls_invc_hdr where ((pybls_invc_type = 'Supplier Standard Payment') and approval_status='Approved' and (invoice_amount-amnt_paid)<=0) order by pybls_invc_hdr_id DESC",
                            "select distinct trim(to_char(rcvbls_invc_hdr_id,'999999999999999999999999999999')) a, rcvbls_invc_number b, '' c, org_id d, trim(to_char(customer_id,'999999999999999999999999999999')) e, trim(to_char(invc_curr_id,'999999999999999999999999999999')) f, rcvbls_invc_hdr_id g from accb.accb_rcvbls_invc_hdr where (((rcvbls_invc_type = 'Customer Advance Payment' and (invoice_amount-amnt_paid)<=0) or rcvbls_invc_type = 'Customer Debit Memo (InDirect Refund)') and approval_status='Approved' and (invoice_amount-invc_amnt_appld_elswhr)>0) order by rcvbls_invc_hdr_id DESC",
                            "select distinct trim(to_char(rcvbls_invc_hdr_id,'999999999999999999999999999999')) a, rcvbls_invc_number b, '' c, org_id d, trim(to_char(customer_id,'999999999999999999999999999999')) e, trim(to_char(invc_curr_id,'999999999999999999999999999999')) f, rcvbls_invc_hdr_id g from accb.accb_rcvbls_invc_hdr where ((rcvbls_invc_type = 'Customer Credit Memo (InDirect Topup)') and approval_status='Approved' and (invoice_amount-invc_amnt_appld_elswhr)>0) order by rcvbls_invc_hdr_id DESC",
                            "select distinct trim(to_char(rcvbls_invc_hdr_id,'999999999999999999999999999999')) a, rcvbls_invc_number b, '' c, org_id d, trim(to_char(customer_id,'999999999999999999999999999999')) e, trim(to_char(invc_curr_id,'999999999999999999999999999999')) f, rcvbls_invc_hdr_id g from accb.accb_rcvbls_invc_hdr where ((rcvbls_invc_type = 'Customer Standard Payment') and approval_status='Approved' and (invoice_amount-amnt_paid)<=0) order by rcvbls_invc_hdr_id DESC"};
            string[] pssblVals = { "2", "01-JAN-1901", "Sample Holiday Date Disallowed",
                             "2", "01-JAN-2014", "Sample Holiday Date Disallowed",
                           "1", "SUNDAY", "No Weekend Transactions",
                           "1", "SATURDAY", "No Weekend Transactions",
                           "4", "EUR", "European Euro",
                           "4", "CNY", "Chinese Yuan",
                           "4", "ZAR", "South African Rand",
                           "4", "XAF", "CFA Franc (BEAC)",
                           "4", "XOF", "CFA Franc (BCEAO)",
                           "4", "NGN", "Nigerian Naira"};

            Global.mnFrm.cmCde.createSysLovs(sysLovs, sysLovsDynQrys, sysLovsDesc);
            Global.mnFrm.cmCde.createSysLovsPssblVals(sysLovs, pssblVals);
            string[] prcsstyps = { "Trial Balance Report", "Profit and Loss Report",
                             "Balance Sheet Report", "Subledger Balance Report",
                             "Post GL Batch", "Open/Close Periods",
                             "Inventory Journal Import", "Internal Payments Journal Import" };
            for (int i = 1; i < 9; i++)
            {
                if (Global.getActnPrcssID(i.ToString()) <= 0)
                {
                    Global.createActnPrcss(i, prcsstyps[i - 1]);
                }
                else
                {
                    Global.updtActnPrcss(i, prcsstyps[i - 1]);
                }
            }
        }

        //public static void createRqrdLOVs1()
        //{
        //  string[] sysLovs = { "Cash Accounts", "Inventory/Asset Accounts", "Contra Expense Accounts",
        //  "Contra Revenue Accounts","Customer Classifications","Supplier Classifications",
        //    "Tax Codes","Discount Codes", "Extra Charges", "Approved Requisitions",
        //    "Suppliers", "Customer/Supplier Sites","Users' Sales Stores","Approved Pro-Forma Invoices",
        //    "Approved Sales Orders","Approved Internal Item Requests",
        //    "Customers","Approved Sales Invoices/Item Issues"};
        //  string[] sysLovsDesc = { "Cash Accounts", "Inventory/Asset Accounts", "Contra Expense Accounts",
        //  "Contra Revenue Accounts","Customer Classifications","Supplier Classifications",
        //    "Tax Codes","Discount Codes","Extra Charges","Approved Requisitions",
        //    "Suppliers", "Customer/Supplier Sites", "Users' Sales Stores","Approved Pro-Forma Invoices",
        //    "Approved Sales Orders","Approved Internal Item Requests",
        //    "Customers", "Approved Sales Invoices/Item Issues" };
        //  string[] sysLovsDynQrys = { "", "", 
        //    "select distinct trim(to_char(accnt_id,'999999999999999999999999999999')) a, accnt_name b, '' c, org_id d, accnt_num e from accb.accb_chart_of_accnts where (accnt_type = 'EX' and is_prnt_accnt = '0' and is_enabled = '1' and is_contra = '1') order by accnt_num", 
        //    "select distinct trim(to_char(accnt_id,'999999999999999999999999999999')) a, accnt_name b, '' c, org_id d, accnt_num e from accb.accb_chart_of_accnts where (accnt_type = 'R' and is_prnt_accnt = '0' and is_enabled = '1' and is_contra = '1') order by accnt_num", 
        //    "", "", 
        //    "select distinct trim(to_char(code_id,'999999999999999999999999999999')) a, code_name b, '' c, org_id d from scm.scm_tax_codes where (itm_type = 'Tax' and is_enabled = '1') order by code_name", 
        //    "select distinct trim(to_char(code_id,'999999999999999999999999999999')) a, code_name b, '' c, org_id d from scm.scm_tax_codes where (itm_type = 'Discount' and is_enabled = '1') order by code_name",
        //    "select distinct trim(to_char(code_id,'999999999999999999999999999999')) a, code_name b, '' c, org_id d from scm.scm_tax_codes where (itm_type = 'Extra Charge' and is_enabled = '1') order by code_name",
        //    "select distinct trim(to_char(y.prchs_doc_hdr_id,'999999999999999999999999999999')) a, y.purchase_doc_num b, '' c, y.org_id d, y.prchs_doc_hdr_id g " +
        //    "from scm.scm_prchs_docs_hdr y, scm.scm_prchs_docs_det z " +
        //    "where (y.purchase_doc_type = 'Purchase Requisition' " +
        //    "and y.approval_status = 'Approved' " +
        //    "and z.prchs_doc_hdr_id = y.prchs_doc_hdr_id and (z.quantity - z.rqstd_qty_ordrd)>0) order by y.prchs_doc_hdr_id DESC",
        //    "select distinct trim(to_char(cust_sup_id,'999999999999999999999999999999')) a, cust_sup_name b, '' c, org_id d from scm.scm_cstmr_suplr where (cust_or_sup ilike '%Supplier%') order by 2",
        //    "select distinct trim(to_char(cust_sup_site_id,'999999999999999999999999999999')) a, site_name b, '' c, cust_supplier_id d from scm.scm_cstmr_suplr_sites order by 2",
        //    "select distinct trim(to_char(y.subinv_id,'999999999999999999999999999999')) a, y.subinv_name b, '' c, y.org_id d, trim(to_char(z.user_id,'999999999999999999999999999999')) e from inv.inv_itm_subinventories y, inv.inv_user_subinventories z where y.subinv_id=z.subinv_id and y.allow_sales = '1' order by 2",
        //    "select distinct trim(to_char(y.invc_hdr_id,'999999999999999999999999999999')) a, y.invc_number b, '' c, y.org_id d, y.invc_hdr_id g " +
        //    "from scm.scm_sales_invc_hdr y, scm.scm_sales_invc_det z " +
        //    "where (y.invc_type = 'Pro-Forma Invoice' " +
        //    "and y.approval_status = 'Approved' " +
        //    "and z.invc_hdr_id = y.invc_hdr_id and (z.doc_qty - z.qty_trnsctd_in_dest_doc)>0) order by y.invc_hdr_id DESC",
        //    "select distinct trim(to_char(y.invc_hdr_id,'999999999999999999999999999999')) a, y.invc_number b, '' c, y.org_id d, y.invc_hdr_id g " +
        //    "from scm.scm_sales_invc_hdr y, scm.scm_sales_invc_det z " +
        //    "where (y.invc_type = 'Sales Order' " +
        //    "and y.approval_status = 'Approved' " +
        //    "and z.invc_hdr_id = y.invc_hdr_id and (z.doc_qty - z.qty_trnsctd_in_dest_doc)>0) order by y.invc_hdr_id DESC",
        //    "select distinct trim(to_char(y.invc_hdr_id,'999999999999999999999999999999')) a, y.invc_number b, '' c, y.org_id d, y.invc_hdr_id g " +
        //    "from scm.scm_sales_invc_hdr y, scm.scm_sales_invc_det z " +
        //    "where (y.invc_type = 'Internal Item Request' " +
        //    "and y.approval_status = 'Approved' " +
        //    "and z.invc_hdr_id = y.invc_hdr_id and (z.doc_qty - z.qty_trnsctd_in_dest_doc)>0) order by y.invc_hdr_id DESC",
        //    "select distinct trim(to_char(cust_sup_id,'999999999999999999999999999999')) a, cust_sup_name b, '' c, org_id d from scm.scm_cstmr_suplr where (cust_or_sup ilike '%Customer%') order by 2",
        //    "select distinct trim(to_char(y.invc_hdr_id,'999999999999999999999999999999')) a, y.invc_number b, '' c, y.org_id d, y.invc_hdr_id g " +
        //    "from scm.scm_sales_invc_hdr y, scm.scm_sales_invc_det z " +
        //    "where ((y.invc_type = 'Item Issue-Unbilled' or y.invc_type = 'Sales Invoice') " +
        //    "and (y.approval_status = 'Approved') " +
        //    "and (z.invc_hdr_id = y.invc_hdr_id) and ((z.doc_qty - z.qty_trnsctd_in_dest_doc)>0)) order by y.invc_hdr_id DESC"
        //    };
        //  string[] pssblVals = { 
        //    "4", "Retail Customer", "Retail Customer"
        //   ,"4", "Wholesale customer", "Wholesale customer",
        //    "4", "Individual", "Individual Person"
        //   ,"4", "Organisation", "Company/Organisation",
        //    "5", "Service Provider", "Service Provider"
        //   ,"5", "Goods Provider", "Goods Provider",
        //    "5", "Service and Goods Provider", "Service and Goods Provider"
        //   ,"5", "Consultant", "Consultant"
        //  ,"5", "Training Provider", "Training Provider"};

        //  Global.mnFrm.cmCde.createSysLovs(sysLovs, sysLovsDynQrys, sysLovsDesc);
        //  Global.mnFrm.cmCde.createSysLovsPssblVals(sysLovs, pssblVals);
        //}

        public static void refreshRqrdVrbls()
        {
            //""
            //Global.myBscActn.login_number = Global.mnFrm.lgn_num;
            //Global.myBscActn.role_set_id = Global.mnFrm.role_st_id;
            //Global.myBscActn.user_id = Global.mnFrm.usr_id;
            //Global.myBscActn.org_id = Global.mnFrm.Og_id;

            Global.mnFrm.cmCde.DefaultPrvldgs = Global.dfltPrvldgs;
            Global.mnFrm.cmCde.SubGrpNames = Global.subGrpNames;
            Global.mnFrm.cmCde.MainTableNames = Global.mainTableNames;
            Global.mnFrm.cmCde.KeyColumnNames = Global.keyColumnNames;
            Global.mnFrm.cmCde.Login_number = Global.mnFrm.lgn_num;
            Global.mnFrm.cmCde.ModuleAdtTbl = Global.myBscActn.full_audit_trail_tbl_name;
            Global.mnFrm.cmCde.ModuleDesc = Global.myBscActn.mdl_description;
            Global.mnFrm.cmCde.ModuleName = Global.myBscActn.name;
            //Global.mnFrm.cmCde.Role_Set_IDs = Global.mnFrm.role_st_id;
            //Global.mnFrm.cmCde.pgSqlConn = Global.mnFrm.gnrlSQLConn;
            Global.mnFrm.cmCde.SampleRole = "Accounting Administrator";
            //Global.mnFrm.cmCde.User_id = Global.mnFrm.usr_id;
            //Global.mnFrm.cmCde.Org_id = Global.mnFrm.Og_id;
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            Global.myBscActn.user_id = Global.mnFrm.usr_id;
            Global.myBscActn.login_number = Global.mnFrm.lgn_num;
            Global.myBscActn.role_set_id = Global.mnFrm.role_st_id;
            Global.myBscActn.org_id = Global.mnFrm.Og_id;
        }

        public static Form isFormAlreadyOpen(Type formType)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm.GetType() == formType)
                {
                    //openForm.Close();
                    //openForm.Dispose();
                    return openForm;
                }
            }
            return null;
        }

        public static string dbtOrCrdtAccnt(int accntid, string incrsDcrse)
        {
            string accntType = Global.mnFrm.cmCde.getAccntType(accntid);
            string isContra = Global.mnFrm.cmCde.isAccntContra(accntid);
            if (isContra == "0")
            {
                if ((accntType == "A" || accntType == "EX") && incrsDcrse == "I")
                {
                    return "Debit";
                }
                else if ((accntType == "A" || accntType == "EX") && incrsDcrse == "D")
                {
                    return "Credit";
                }
                else if ((accntType == "EQ" || accntType == "R" || accntType == "L") && incrsDcrse == "I")
                {
                    return "Credit";
                }
                else if ((accntType == "EQ" || accntType == "R" || accntType == "L") && incrsDcrse == "D")
                {
                    return "Debit";
                }
            }
            else
            {
                if ((accntType == "A" || accntType == "EX") && incrsDcrse == "I")
                {
                    return "Credit";
                }
                else if ((accntType == "A" || accntType == "EX") && incrsDcrse == "D")
                {
                    return "Debit";
                }
                else if ((accntType == "EQ" || accntType == "R" || accntType == "L") && incrsDcrse == "I")
                {
                    return "Debit";
                }
                else if ((accntType == "EQ" || accntType == "R" || accntType == "L") && incrsDcrse == "D")
                {
                    return "Credit";
                }
            }
            return "";
        }

        public static int dbtOrCrdtAccntMultiplier(int accntid, string incrsDcrse)
        {
            string accntType = Global.mnFrm.cmCde.getAccntType(accntid);
            string isContra = Global.mnFrm.cmCde.isAccntContra(accntid);
            if (isContra == "0")
            {
                if ((accntType == "A" || accntType == "EX") && incrsDcrse == "I")
                {
                    return 1;
                }
                else if ((accntType == "A" || accntType == "EX") && incrsDcrse == "D")
                {
                    return -1;
                }
                else if ((accntType == "EQ" || accntType == "R" || accntType == "L") && incrsDcrse == "I")
                {
                    return 1;
                }
                else if ((accntType == "EQ" || accntType == "R" || accntType == "L") && incrsDcrse == "D")
                {
                    return -1;
                }
            }
            else
            {
                if ((accntType == "A" || accntType == "EX") && incrsDcrse == "I")
                {
                    return -1;
                }
                else if ((accntType == "A" || accntType == "EX") && incrsDcrse == "D")
                {
                    return 1;
                }
                else if ((accntType == "EQ" || accntType == "R" || accntType == "L") && incrsDcrse == "I")
                {
                    return -1;
                }
                else if ((accntType == "EQ" || accntType == "R" || accntType == "L") && incrsDcrse == "D")
                {
                    return 1;
                }
            }
            return 1;
        }

        public static string incrsOrDcrsAccnt(int accntid, string dbtOrCrdt)
        {
            string accntType = Global.mnFrm.cmCde.getAccntType(accntid);
            string isContra = Global.mnFrm.cmCde.isAccntContra(accntid);
            if (isContra == "0")
            {
                if ((accntType == "A" || accntType == "EX") && dbtOrCrdt == "Debit")
                {
                    return "INCREASE";
                }
                else if ((accntType == "A" || accntType == "EX") && dbtOrCrdt == "Credit")
                {
                    return "DECREASE";
                }
                else if ((accntType == "EQ" || accntType == "R" || accntType == "L") && dbtOrCrdt == "Credit")
                {
                    return "INCREASE";
                }
                else if ((accntType == "EQ" || accntType == "R" || accntType == "L") && dbtOrCrdt == "Debit")
                {
                    return "DECREASE";
                }
            }
            else
            {
                if ((accntType == "A" || accntType == "EX") && dbtOrCrdt == "Debit")
                {
                    return "DECREASE";
                }
                else if ((accntType == "A" || accntType == "EX") && dbtOrCrdt == "Credit")
                {
                    return "INCREASE";
                }
                else if ((accntType == "EQ" || accntType == "R" || accntType == "L") && dbtOrCrdt == "Credit")
                {
                    return "DECREASE";
                }
                else if ((accntType == "EQ" || accntType == "R" || accntType == "L") && dbtOrCrdt == "Debit")
                {
                    return "INCREASE";
                }
            }
            return "";
        }
        #endregion
    }
}
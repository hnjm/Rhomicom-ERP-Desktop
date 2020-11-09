using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using AppointmentsManagement.Forms;
using System.Windows.Forms;
using CommonCode;
using Npgsql;

namespace AppointmentsManagement.Classes
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
        public static AppointmentsManagement myVst = new AppointmentsManagement();
        public static mainForm mnFrm = null;
        public static wfnVstApntmntForm wfnVstFrm = null;
        public static wfnApntMntsDataForm wfnApntmtFrm = null;
        public static wfnApntMntsDataForm wfnApntmtFrmDiag = null;
        public static wfnSrvcPrvdrsForm wfnSrvcPrvdFrm = null;
        public static wfnSrvcOffrdForm wfnSrvcOfrdFrm = null;
        public static smmryDshBrdForm wfnSmmryDshForm = null;

        public static leftMenuForm wfnLftMnu = null;
        public static string[] dfltPrvldgs = { 
         /*0*/ "View Visits and Appointments", 
         /*1*/ "View Visits/Appointments", "View Appointments Data", "View Service Providers", "View Services Offered",
         /*5*/ "View SQL","View Record History",
         /*7*/ "Add Visits/Appointments","Edit Visits/Appointments","Delete Visits/Appointments",
         /*10*/ "Add Services Offered","Edit Services Offered","Delete Services Offered",
         /*13*/"Add Service Providers","Edit Service Providers","Delete Service Providers",
        /*16*/"View only Self-Created Sales","Cancel Documents","Take Payments",
        /*19*/"Apply Adhoc Discounts", "Apply Pre-defined Discounts", 
        /*21*/"Can Edit Unit Price","View Other Provider's Data"};

        public static string currentPanel = "";
        public static string itms_SQL = "";
        public static int selectedStoreID = -1;
        public static string intFcSql = string.Empty;
        #endregion

        #region "DATA MANIPULATION FUNCTIONS..."
        #region "VISITS/APPOINTMENTS..."
        public static double get_LtstExchRate(int fromCurrID, int toCurrID, string asAtDte)
        {
            int fnccurid = Global.mnFrm.cmCde.getOrgFuncCurID(Global.mnFrm.cmCde.Org_id);
            //this.curCode = Global.mnFrm.cmCde.getPssblValNm(this.curid);
            if (fromCurrID == toCurrID)
            {
                return 1;
            }

            string strSql = "";
            strSql = @"SELECT CASE WHEN a.currency_from_id=" + fromCurrID +
              @" THEN a.multiply_from_by ELSE (1/a.multiply_from_by) END
      FROM accb.accb_exchange_rates a WHERE ((a.currency_from_id=" + fromCurrID +
              @" and a.currency_to_id=" + toCurrID +
              @") or (a.currency_to_id=" + fromCurrID +
              @" and a.currency_from_id=" + toCurrID +
              @")) and to_timestamp(a.conversion_date,'YYYY-MM-DD') <= to_timestamp('" + asAtDte +
              "','DD-Mon-YYYY HH24:MI:SS') ORDER BY to_timestamp(a.conversion_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            if (fromCurrID != fnccurid && toCurrID != fnccurid)
            {
                double a = Global.get_LtstExchRate(fromCurrID, fnccurid, asAtDte);
                double b = Global.get_LtstExchRate(toCurrID, fnccurid, asAtDte);
                if (a != 0 && b != 0)
                {
                    return a / b;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
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

        public static DataSet get_Visits(
       string searchWord, string searchIn,
      Int64 offset, int limit_size, int orgID
         , bool shwActive, bool shwUnsettled, string extrWhere)
        {
            /*Created By
      Client/Visitor
      Doc. Status
      Document Number
      Visit Purpose
      Start Date
      Service Provider
      Provider Group
             */
            string strSql = "";
            string whereClause = "";
            string activeDocClause = "";
            string unstldBillClause = "";
            if (shwUnsettled)
            {
                unstldBillClause = @" AND EXISTS (SELECT f.src_doc_hdr_id 
  FROM scm.scm_doc_amnt_smmrys f WHERE f.smmry_type='7Change/Balance' 
  and round(f.smmry_amnt,2)>0 and y.invc_hdr_id=f.src_doc_hdr_id and f.src_doc_type=y.invc_type
 and y.approval_status != 'Cancelled')";
                //unpstdCls = " AND (a.approval_status != 'Approved')";
            }
            if (shwActive)
            {
                activeDocClause = " AND (a.vst_status='Open')";
            }

            if (searchIn == "Doc. Status")
            {
                whereClause = "(a.vst_status ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Service Provider")
            {
                whereClause = @"((Select array_to_string(array_agg(distinct (CASE WHEN d.prsn_id > 0 THEN prs.get_prsn_name(d.prsn_id) ||' (' || prs.get_prsn_loc_id(d.prsn_id) || ')' 
ELSE scm.get_cstmr_splr_name(d.cstmr_id) END)),', ') 
From hosp.appntmnt k
        LEFT OUTER JOIN hosp.srvs_prvdrs d ON (k.srvs_prvdr_prsn_id = d.prvdr_id and k.vst_id = a.vst_id)) ilike '" + searchWord.Replace("'", "''") +
             "')";
            }
            else if (searchIn == "Provider Group")
            {
                whereClause = @"((Select array_to_string(array_agg(distinct c.prvdr_grp_name),', ') 
        FROM hosp.appntmnt k 
        LEFT OUTER JOIN hosp.prvdr_grps c ON (k.prvdr_grp_id = c.prvdr_grp_id and k.vst_id = a.vst_id)) ilike '" + searchWord.Replace("'", "''") +
             "')";
            }
            else if (searchIn == "Created By")
            {
                whereClause = "(a.created_by IN (select c.user_id from sec.sec_users c where c.user_name ilike '" + searchWord.Replace("'", "''") +
             "'))";
            }
            else if (searchIn == "Client/Visitor")
            {
                whereClause = "(a.cstmr_id IN (select c.cust_sup_id from scm.scm_cstmr_suplr c where c.cust_sup_name ilike '" + searchWord.Replace("'", "''") +
            "') or (prs.get_prsn_name(a.prsn_id)|| prs.get_prsn_loc_id(a.prsn_id)) ilike '" + searchWord.Replace("'", "''") +
            "' or a.vstr_name_desc ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Visit Purpose")
            {
                whereClause = "(a.cmnts ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Document Number")
            {
                whereClause = "('' || a.vst_id ilike '" + searchWord.Replace("'", "''") +
            "' or y.invc_number ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Start Date")
            {
                whereClause = "(to_char(to_timestamp(a.vst_end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            strSql = @"SELECT a.vst_id, ''||a.vst_id, COALESCE(y.invc_number,'')  
FROM hosp.visit a 
LEFT OUTER JOIN scm.scm_sales_invc_hdr y ON ((a.vst_id = y.other_mdls_doc_id or y.other_mdls_doc_id IS NULL)
and ('Visit'=y.other_mdls_doc_type or y.other_mdls_doc_type IS NULL)) " +
         "WHERE " + whereClause + activeDocClause + unstldBillClause + " and a.org_id=" + orgID +
         @" and (y.invc_hdr_id>0 or y.invc_hdr_id IS NULL)" + extrWhere +
         " ORDER BY a.vst_id DESC LIMIT " + limit_size +
         " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            if (Global.wfnVstFrm != null)
            {
                Global.wfnVstFrm.rec_SQL = strSql;
            }
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_Ttl_Visits(string searchWord, string searchIn,
         int orgID, bool shwActive, bool shwUnsettled, string extrWhere)
        {
            /*Created By
      Client/Visitor
      Doc. Status
      Document Number
      Visit Purpose
      Start Date
      Service Provider
      Provider Group
             */
            string strSql = "";
            string whereClause = "";
            string activeDocClause = "";
            string unstldBillClause = "";
            if (shwUnsettled)
            {
                unstldBillClause = @" AND EXISTS (SELECT f.src_doc_hdr_id 
  FROM scm.scm_doc_amnt_smmrys f WHERE f.smmry_type='7Change/Balance' 
  and round(f.smmry_amnt,2)>0 and y.invc_hdr_id=f.src_doc_hdr_id and f.src_doc_type=y.invc_type
 and y.approval_status != 'Cancelled')";
                //unpstdCls = " AND (a.approval_status != 'Approved')";
            }
            if (shwActive)
            {
                activeDocClause = " AND (a.vst_status='Open')";
            }

            if (searchIn == "Doc. Status")
            {
                whereClause = "(a.vst_status ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Service Provider")
            {
                whereClause = @"((Select array_to_string(array_agg(distinct (CASE WHEN d.prsn_id > 0 THEN prs.get_prsn_name(d.prsn_id) ||' (' || prs.get_prsn_loc_id(d.prsn_id) || ')' 
ELSE scm.get_cstmr_splr_name(d.cstmr_id) END)),', ') 
From hosp.appntmnt k
        LEFT OUTER JOIN hosp.srvs_prvdrs d ON (k.srvs_prvdr_prsn_id = d.prvdr_id and k.vst_id = a.vst_id)) ilike '" + searchWord.Replace("'", "''") +
             "')";
            }
            else if (searchIn == "Provider Group")
            {
                whereClause = @"((Select array_to_string(array_agg(distinct c.prvdr_grp_name),', ') 
        FROM hosp.appntmnt k 
        LEFT OUTER JOIN hosp.prvdr_grps c ON (k.prvdr_grp_id = c.prvdr_grp_id and k.vst_id = a.vst_id)) ilike '" + searchWord.Replace("'", "''") +
             "')";
            }
            else if (searchIn == "Created By")
            {
                whereClause = "(a.created_by IN (select c.user_id from sec.sec_users c where c.user_name ilike '" + searchWord.Replace("'", "''") +
             "'))";
            }
            else if (searchIn == "Client/Visitor")
            {
                whereClause = "(a.cstmr_id IN (select c.cust_sup_id from scm.scm_cstmr_suplr c where c.cust_sup_name ilike '" + searchWord.Replace("'", "''") +
            "') or (prs.get_prsn_name(a.prsn_id)|| prs.get_prsn_loc_id(a.prsn_id)) ilike '" + searchWord.Replace("'", "''") +
            "' or a.vstr_name_desc ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Visit Purpose")
            {
                whereClause = "(a.cmnts ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Document Number")
            {
                whereClause = "('' || a.vst_id ilike '" + searchWord.Replace("'", "''") +
            "' or y.invc_number ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Start Date")
            {
                whereClause = "(to_char(to_timestamp(a.vst_end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            strSql = @"SELECT count(1)  
FROM hosp.visit a 
LEFT OUTER JOIN scm.scm_sales_invc_hdr y ON ((a.vst_id = y.other_mdls_doc_id or y.other_mdls_doc_id IS NULL)
and ('Visit'=y.other_mdls_doc_type or y.other_mdls_doc_type IS NULL)) " +
         "WHERE " + whereClause + activeDocClause + unstldBillClause + " and a.org_id=" + orgID +
         @" and (y.invc_hdr_id>0 or y.invc_hdr_id IS NULL)" + extrWhere;

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

        public static DataSet get_One_VisitDt(long vstID)
        {
            string strSql = @"Select a.vst_id, a.vstr_name_desc, a.cmnts,  
      a.prsn_id, prs.get_prsn_name(a.prsn_id) ||' (' || prs.get_prsn_loc_id(a.prsn_id) || ')', 
      a.cstmr_id, scm.get_cstmr_splr_name(a.cstmr_id), a.created_by, a.vst_status, a.vstr_type, 
      to_char(to_timestamp(a.vst_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
      to_char(to_timestamp(a.vst_end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
      a.bill_this_visit, COALESCE(y.invc_hdr_id,-1), COALESCE(y.invc_number,''), 
      COALESCE(y.pymny_method_id,-1), accb.get_pymnt_mthd_name(y.pymny_method_id), 
      COALESCE(y.invc_curr_id, -1), 
      gst.get_pssbl_val(COALESCE(y.invc_curr_id,-1)), COALESCE(y.exchng_rate,1), y.approval_status, 
      y.invc_type, COALESCE(y.enbl_auto_misc_chrges,'0'), a.cstmr_site_id, y.payment_terms 
          FROM hosp.visit a 
      LEFT OUTER JOIN scm.scm_sales_invc_hdr y ON ((a.vst_id = y.other_mdls_doc_id)
      and ('Visit'=y.other_mdls_doc_type)) " +
         "WHERE a.vst_id=" + vstID + @" ";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_One_VisitAppnts(long visitID)
        {
            string strSql = @"Select a.appntmnt_id, '' || a.appntmnt_id, a.prvdr_type, 
      to_char(to_timestamp(a.appntmnt_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
      to_char(to_timestamp(a.appntmnt_end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
      a.srvs_type_id, c.type_name, 
      a.prvdr_grp_id, d.prvdr_grp_name,
             a.srvs_prvdr_prsn_id, 
      CASE WHEN b.prsn_id > 0 THEN prs.get_prsn_name(b.prsn_id) ||' (' || prs.get_prsn_loc_id(b.prsn_id) || ')' 
      ELSE scm.get_cstmr_splr_name(b.cstmr_id) END, 
      a.created_by, a.appntmnt_status, a.cmnts " +
               @"FROM hosp.appntmnt a 
      LEFT OUTER JOIN hosp.srvs_types c ON (a.srvs_type_id = c.type_id)
      LEFT OUTER JOIN hosp.prvdr_grps d ON (a.prvdr_grp_id = d.prvdr_grp_id)
      LEFT OUTER JOIN hosp.srvs_prvdrs b ON (a.srvs_prvdr_prsn_id = b.prvdr_id) " +
           "WHERE a.vst_id=" + visitID +
          @" ORDER BY a.appntmnt_date ASC";

            if (Global.wfnVstFrm != null)
            {
                Global.wfnVstFrm.fclty_SQL = strSql;
            }

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long getNewAppntMntID()
        {
            string strSql = "select nextval('hosp.appntmnt_appntmnt_id_seq')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static long getNewAppntDataLnID()
        {
            string strSql = "select nextval('hosp.appntmnt_data_captured_data_capture_id_seq')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static string get_VisitRec_Hstry(long hdrID)
        {
            string strSQL = @"SELECT a.created_by, 
to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
      a.last_update_by, 
      to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM hosp.visit a WHERE(a.vst_id = " + hdrID + ")";
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

        public static string get_AppntmtRec_Hstry(long hdrID)
        {
            string strSQL = @"SELECT a.created_by, 
to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
      a.last_update_by, 
      to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM hosp.appntmnt a WHERE(a.appntmnt_id = " + hdrID + ")";
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

        public static string get_SalesDT_Rec_Hstry(long dteID)
        {
            string strSQL = @"SELECT a.created_by, 
to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
      a.last_update_by, 
      to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM scm.scm_sales_invc_det a WHERE(a.invc_det_ln_id = " + dteID + ")";
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

        public static void deleteScmRcvblsDocDet(long valLnid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSQL = "DELETE FROM scm.scm_rcvbl_amnt_smmrys WHERE src_rcvbl_hdr_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteRcvblsDocHdrNDet(long valLnid, string docNum)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Document Number = " + docNum;
            string delSQL = "DELETE FROM accb.accb_rcvbl_amnt_smmrys WHERE src_rcvbl_hdr_id = " + valLnid;
            //Global.mnFrm.cmCde.showSQLNoPermsn(delSQL);
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
            delSQL = "DELETE FROM accb.accb_rcvbls_invc_hdr WHERE rcvbls_invc_hdr_id = " + valLnid;
            //Global.mnFrm.cmCde.showSQLNoPermsn(delSQL);
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteRcvblsDocDetails(long valLnid, string docNum)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Document Number = " + docNum;
            string delSQL = "DELETE FROM accb.accb_rcvbl_amnt_smmrys WHERE src_rcvbl_hdr_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }
        public static void deleteRcvblsDocDet(long valLnid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSQL = "DELETE FROM accb.accb_rcvbl_amnt_smmrys WHERE rcvbl_smmry_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteDocSmmryItms(long docID, string docType)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSQL = "DELETE FROM scm.scm_doc_amnt_smmrys WHERE src_doc_hdr_id = " +
              docID + " and src_doc_type = '" + docType + "'";
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteSalesLnItm(long lnID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSQL = "DELETE FROM scm.scm_sales_invc_det WHERE invc_det_ln_id = " +
              lnID + "";
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteSalesDoc(long docID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Deleting a Sales Document and all its Lines";
            string delSQL = "DELETE FROM scm.scm_sales_invc_det WHERE invc_hdr_id = " +
              docID + "";
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
            delSQL = "DELETE FROM scm.scm_sales_invc_hdr WHERE invc_hdr_id = " +
           docID + "";
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteSalesDocLns(long docID, long othMdlID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Deleting Lines from a Sales Doc.";
            string delSQL = "DELETE FROM scm.scm_sales_invc_det WHERE invc_hdr_id = " +
              docID + " and other_mdls_doc_id = " + othMdlID +
              " and other_mdls_doc_type IN ('Restaurant Order','Pool Subscription','Gym Subscription')";
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteDocGLInfcLns(long docID, string srcDocType)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSQL = "DELETE FROM scm.scm_gl_interface WHERE src_doc_id = " +
              docID + " and src_doc_typ ilike '%" + srcDocType.Replace("'", "''") + "%' and gl_batch_id = -1";
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteVisit(long visitID, string salesNum)
        {
            //
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Sales Number = " + salesNum;
            string delSQL = @"DELETE FROM hosp.appntmnt_data_captured 
       WHERE appntmnt_id IN (SELECT appntmnt_id FROM hosp.appntmnt WHERE vst_id = " + visitID + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);

            delSQL = "DELETE FROM hosp.appntmnt WHERE vst_id = " + visitID;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);

            delSQL = "DELETE FROM hosp.visit WHERE vst_id = " + visitID;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);

        }

        public static void deleteSalesLn(long Lnid, string lnDesc)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Line Desc = " + lnDesc;
            string delSQL = "DELETE FROM scm.scm_sales_invc_det WHERE invc_det_ln_id = " + Lnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static long getVisitID(string vstrName, string startDte, string endDte)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = @"select vst_id from hosp.visit 
      where vstr_name_desc ilike '" +
             vstrName.Replace("'", "''") + "' and (to_timestamp('" + startDte + "','DD-Mon-YYYY HH24:MI:SS') " +
             "between to_timestamp(vst_date,'YYYY-MM-DD HH24:MI:SS') and to_timestamp(vst_end_date,'YYYY-MM-DD HH24:MI:SS') or to_timestamp('"
             + endDte + "','DD-Mon-YYYY HH24:MI:SS') " +
             "between to_timestamp(vst_date,'YYYY-MM-DD HH24:MI:SS') and to_timestamp(vst_end_date,'YYYY-MM-DD HH24:MI:SS'))";
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

        public static long getVstInvcID(int visitID)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = @"select a.invc_hdr_id from scm.scm_sales_invc_hdr a, hosp.visit b 
      where a.other_mdls_doc_id = b.vst_id and a.other_mdls_doc_type = 'Visit' 
      and (b.vst_id = " + visitID + ")";
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

        public static void createVisit(string vstorTyp, string strtDte, string endDte,
          string vstrNameDesc, string vstPurpose, long prsnID, long cstmrID,
          string vstStatus, bool billVst, int OrgID, long cstmrSiteID)
        {
            strtDte = DateTime.ParseExact(
         strtDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            endDte = DateTime.ParseExact(
         endDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO hosp.visit(
            prsn_id, vst_date, cmnts, created_by, creation_date, 
            last_update_by, last_update_date, vst_status, vstr_type, cstmr_id, 
            vstr_name_desc, vst_end_date, bill_this_visit, org_id, cstmr_site_id) " +
                  "VALUES (" + prsnID + ", '" + strtDte.Replace("'", "''") +
                  "', '" + vstPurpose.Replace("'", "''") +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', '" + vstStatus.Replace("'", "''") +
                  "', '" + vstorTyp.Replace("'", "''") +
                  "', " + cstmrID + ", '" + vstrNameDesc.Replace("'", "''") +
                  "', '" + endDte.Replace("'", "''") +
                  "', '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(billVst) + "', " + OrgID + ", " + cstmrSiteID + ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updateVisit(long visitID, string vstorTyp, string strtDte, string endDte,
          string vstrNameDesc, string vstPurpose, long prsnID, long cstmrID,
          string vstStatus, bool billVst, int OrgID, long cstmrSiteID)
        {
            strtDte = DateTime.ParseExact(
         strtDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            endDte = DateTime.ParseExact(
         endDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = @"UPDATE hosp.visit 
            SET prsn_id = " + prsnID + ", vst_date = '" + strtDte.Replace("'", "''") +
                  "', cmnts = '" + vstPurpose.Replace("'", "''") +
                  "', last_update_by = " + Global.myVst.user_id +
                  ", last_update_date = '" + dateStr +
                  "', vst_status = '" + vstStatus.Replace("'", "''") +
                  "', vstr_type = '" + vstorTyp.Replace("'", "''") +
                  "', cstmr_id = " + cstmrID + ", vstr_name_desc = '" + vstrNameDesc.Replace("'", "''") +
                  "', vst_end_date = '" + endDte.Replace("'", "''") +
                  "', bill_this_visit = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(billVst) +
                  "', cstmr_site_id = " + cstmrSiteID + " WHERE vst_id = " + visitID + " ";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void createAppntmnt(long appntmntID, long vstID, string strtDte,
          string endDte, string appntStatus, string rmrksCmnts, string prvdrTyp,
          int srvsTypID, long prvdrID, int prvdrGrpID)
        {
            strtDte = DateTime.ParseExact(
         strtDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            endDte = DateTime.ParseExact(
         endDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO hosp.appntmnt(
            appntmnt_id, vst_id, appntmnt_date, appntmnt_status, cmnts, created_by, 
            creation_date, last_update_by, last_update_date, prvdr_type, 
            srvs_type_id, srvs_prvdr_prsn_id, prvdr_grp_id, appntmnt_end_date) " +
                  "VALUES (" + appntmntID + "," + vstID +
                  ", '" + strtDte.Replace("'", "''") +
                  "', '" + appntStatus.Replace("'", "''") +
                  "', '" + rmrksCmnts.Replace("'", "''") +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', '" + prvdrTyp.Replace("'", "''") +
                  "', " + srvsTypID + ", " + prvdrID +
                  ", " + prvdrGrpID +
                  ", '" + endDte.Replace("'", "''") +
                  "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updateAppntmnt(long appntmntID, long vstID, string strtDte,
          string endDte, string appntStatus, string rmrksCmnts, string prvdrTyp,
          int srvsTypID, long prvdrID, int prvdrGrpID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";

            strtDte = DateTime.ParseExact(
         strtDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            endDte = DateTime.ParseExact(
         endDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE hosp.appntmnt 
            SET appntmnt_date = '" + strtDte.Replace("'", "''") +
                  "', appntmnt_status = '" + appntStatus.Replace("'", "''") +
                  "', cmnts = '" + rmrksCmnts.Replace("'", "''") +
                  "', last_update_by = " + Global.myVst.user_id + ", last_update_date = '" + dateStr +
                  "', prvdr_type = '" + prvdrTyp.Replace("'", "''") +
                  "', srvs_type_id = " + srvsTypID + ", srvs_prvdr_prsn_id = " + prvdrID +
                  ", prvdr_grp_id = " + prvdrGrpID +
                  ", appntmnt_end_date = '" + endDte.Replace("'", "''") +
                  "' WHERE (appntmnt_id = " + appntmntID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void deleteAppntmnt(long lnID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSQL = "DELETE FROM hosp.appntmnt_data_captured WHERE appntmnt_id = " + lnID + "";
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);

            delSQL = "DELETE FROM hosp.appntmnt WHERE appntmnt_id = " + lnID + "";
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        #endregion

        #region "RECEIVABLES..."
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

        public static int get_DfltBadDbtAcnt(int orgID)
        {
            string strSql = "SELECT org.get_dflt_accnt_id(" + Global.mnFrm.prsn_id + ", bad_debt_acnt_id) " +
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
        public static int get_DfltAdjstLbltyAcnt(int orgID)
        {
            string strSql = "SELECT org.get_dflt_accnt_id(" + Global.mnFrm.prsn_id + ", inv_adjstmnts_lblty_acnt_id) " +
             "FROM scm.scm_dflt_accnts a " +
             "WHERE(a.org_id = " + orgID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static int get_DfltAccPyblAcnt(int orgID)
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

        public static int get_DfltPurchRtrnAcnt(int orgID)
        {
            string strSql = "SELECT org.get_dflt_accnt_id("+Global.mnFrm.prsn_id + ", prchs_rtrns_acnt_id) " +
             "FROM scm.scm_dflt_accnts a " +
             "WHERE(a.org_id = " + orgID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }
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

        public static long getNewInvcLnID()
        {
            //string strSql = "select nextval('accb.accb_trnsctn_batches_batch_id_seq'::regclass);";
            string strSql = "select nextval('scm.scm_itm_sales_ordrs_det_trnstn_id_seq')";
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

        public static string getLtstInvcIDNoInPrfx(string prfxTxt)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "select count(invc_hdr_id) from scm.scm_sales_invc_hdr WHERE org_id=" +
              Global.mnFrm.cmCde.Org_id + " and invc_number ilike '" + prfxTxt.Replace("'", "''") + "%'";
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

        public static long getNewDataDefID()
        {
            //string strSql = "select nextval('accb.accb_trnsctn_batches_batch_id_seq'::regclass);";
            string strSql = "select nextval('hosp.srvc_typ_data_to_capture_srvc_data_capture_id_seq')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static void createScmRcvblsDocDet(long hdrID, string lineType, string lineDesc,
          double entrdAmnt, int entrdCurrID, int codeBhnd, string docType,
          bool autoCalc, string incrDcrs1, int costngID, string incrDcrs2, int blncgAccntID,
          long prepayDocHdrID, string vldyStatus, long orgnlLnID,
          int funcCurrID, int accntCurrID, double funcCurrRate, double accntCurrRate,
          double funcCurrAmnt, double accntCurrAmnt)
        {

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            //rcvbl_smmry_id, " + smmryID + ", 
            string insSQL = @"INSERT INTO scm.scm_rcvbl_amnt_smmrys(
            rcvbl_smmry_type, rcvbl_smmry_desc, rcvbl_smmry_amnt, 
            code_id_behind, src_rcvbl_type, src_rcvbl_hdr_id, created_by, 
            creation_date, last_update_by, last_update_date, auto_calc, incrs_dcrs1, 
            rvnu_acnt_id, incrs_dcrs2, rcvbl_acnt_id, appld_prepymnt_doc_id, 
            orgnl_line_id, validty_status, entrd_curr_id, func_curr_id, accnt_curr_id, 
            func_curr_rate, accnt_curr_rate, func_curr_amount, accnt_curr_amnt) " +
                  "VALUES ('" + lineType.Replace("'", "''") +
                  "', '" + lineDesc.Replace("'", "''") +
                  "', " + entrdAmnt +
                  ", " + codeBhnd +
                  ", '" + docType.Replace("'", "''") +
                  "', " + hdrID +
                  ", " + Global.myVst.user_id + ", '" + dateStr +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
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
                  ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtScmRcvblsDocDet(long docDetID, long hdrID, string lineType, string lineDesc,
          double entrdAmnt, int entrdCurrID, int codeBhnd, string docType,
          bool autoCalc, string incrDcrs1, int costngID, string incrDcrs2, int blncgAccntID,
          long prepayDocHdrID, string vldyStatus, long orgnlLnID,
          int funcCurrID, int accntCurrID, double funcCurrRate, double accntCurrRate,
          double funcCurrAmnt, double accntCurrAmnt)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE scm.scm_rcvbl_amnt_smmrys
   SET rcvbl_smmry_type='" + lineType.Replace("'", "''") +
                  "', rcvbl_smmry_desc='" + lineDesc.Replace("'", "''") +
                  "', rcvbl_smmry_amnt=" + entrdAmnt +
                  ", code_id_behind=" + codeBhnd +
                  ", src_rcvbl_type='" + docType.Replace("'", "''") +
                  "', src_rcvbl_hdr_id=" + hdrID +
                  ", last_update_by=" + Global.myVst.user_id +
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
                  " WHERE rcvbl_smmry_id = " + docDetID;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void deleteScmRcvblsDocDets(long valLnid, int cdeBhnd)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";

            string delSQL = "DELETE FROM scm.scm_rcvbl_amnt_smmrys WHERE src_rcvbl_hdr_id = " + valLnid +
              " and code_id_behind = " + cdeBhnd;

            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteScmRcvblsDocDets(long valLnid, string docNum)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Document Number = " + docNum;
            string delSQL = "DELETE FROM scm.scm_rcvbl_amnt_smmrys WHERE src_rcvbl_hdr_id = " + valLnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void createRcvblsDocHdr(int orgid, string docDte, string docNum,
        string docType, string docDesc, long srcDocHdrID, int cstmrID, int cstmrSiteID,
          string apprvlStatus, string nxtApprvlActn, double invcAmnt, string pymntTrms,
          string srcDocType, int pymntMthdID, double amntPaid, long glBtchID,
          string cstmrDocNum, string docTmpltClsftn, int currID, double amntAppld, int blcngAccntID)
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
            cstmrs_doc_num, doc_tmplt_clsfctn, invc_curr_id, invc_amnt_appld_elswhr, balancing_accnt_id) " +
                  "VALUES ('" + docDte.Replace("'", "''") +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
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
                  "', " + currID + ", " + amntAppld + ", " + blcngAccntID + ")";
            //Global.mnFrm.cmCde.showSQLNoPermsn(insSQL);
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtRcvblsDocHdr(long hdrID, string docDte, string docNum,
        string docType, string docDesc, long srcDocHdrID, int spplrID, int spplrSiteID,
          string apprvlStatus, string nxtApprvlActn, double invcAmnt, string pymntTrms,
          string srcDocType, int pymntMthdID, double amntPaid, long glBtchID,
          string spplrInvcNum, string docTmpltClsftn, int currID, double amntAppld, int blcngAccntID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            docDte = DateTime.ParseExact(docDte, "dd-MMM-yyyy",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_rcvbls_invc_hdr
       SET rcvbls_invc_date='" + docDte.Replace("'", "''") +
                  "', last_update_by=" + Global.myVst.user_id +
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
                  ", balancing_accnt_id=" + blcngAccntID +
                  " WHERE rcvbls_invc_hdr_id = " + hdrID;
            //Global.mnFrm.cmCde.showSQLNoPermsn(insSQL);
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void createRcvblsDocDet(long smmryID, long hdrID, string lineType, string lineDesc,
          double entrdAmnt, int entrdCurrID, int codeBhnd, string docType,
          bool autoCalc, string incrDcrs1, int costngID, string incrDcrs2, int blncgAccntID,
          long prepayDocHdrID, string vldyStatus, long orgnlLnID,
          int funcCurrID, int accntCurrID, double funcCurrRate, double accntCurrRate,
          double funcCurrAmnt, double accntCurrAmnt)
        {

            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO accb.accb_rcvbl_amnt_smmrys(
            rcvbl_smmry_id, rcvbl_smmry_type, rcvbl_smmry_desc, rcvbl_smmry_amnt, 
            code_id_behind, src_rcvbl_type, src_rcvbl_hdr_id, created_by, 
            creation_date, last_update_by, last_update_date, auto_calc, incrs_dcrs1, 
            rvnu_acnt_id, incrs_dcrs2, rcvbl_acnt_id, appld_prepymnt_doc_id, 
            orgnl_line_id, validty_status, entrd_curr_id, func_curr_id, accnt_curr_id, 
            func_curr_rate, accnt_curr_rate, func_curr_amount, accnt_curr_amnt) " +
                  "VALUES (" + smmryID + ", '" + lineType.Replace("'", "''") +
                  "', '" + lineDesc.Replace("'", "''") +
                  "', " + entrdAmnt +
                  ", " + codeBhnd +
                  ", '" + docType.Replace("'", "''") +
                  "', " + hdrID +
                  ", " + Global.myVst.user_id + ", '" + dateStr +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
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
                  ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtRcvblsDocDet(long docDetID, long hdrID, string lineType, string lineDesc,
          double entrdAmnt, int entrdCurrID, int codeBhnd, string docType,
          bool autoCalc, string incrDcrs1, int costngID, string incrDcrs2, int blncgAccntID,
          long prepayDocHdrID, string vldyStatus, long orgnlLnID,
          int funcCurrID, int accntCurrID, double funcCurrRate, double accntCurrRate,
          double funcCurrAmnt, double accntCurrAmnt)
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
                  ", last_update_by=" + Global.myVst.user_id +
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
                  " WHERE rcvbl_smmry_id = " + docDetID;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static DataSet get_LastRcvblPay_Trns(long docID, string docType, int orgID)
        {
            string strSql = "";
            strSql = "SELECT a.pymnt_id, accb.get_pymnt_mthd_name(a.pymnt_mthd_id), a.amount_paid, a.change_or_balance, a.pymnt_remark, " +
                  "a.src_doc_typ, a.src_doc_id, a.created_by, to_char(to_timestamp(a.pymnt_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), b.rcvbls_invc_number, c.user_name " +
             "FROM accb.accb_payments a, accb.accb_rcvbls_invc_hdr b, sec.sec_users c " +
             "WHERE(a.src_doc_id = " + docID +
             " and a.src_doc_typ = '" + docType.Replace("'", "''") +
             "') and (a.src_doc_id = b.rcvbls_invc_hdr_id and b.org_id = " + orgID +
             " and a.created_by = c.user_id) " +
             "ORDER BY to_timestamp(a.pymnt_date,'YYYY-MM-DD HH24:MI:SS') DESC, a.pymnt_id DESC LIMIT 1 " +
               " OFFSET 0";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
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
       cstmrs_doc_num, doc_tmplt_clsfctn, invc_curr_id, gst.get_pssbl_val(a.invc_curr_id)
  FROM accb.accb_rcvbls_invc_hdr a " +
              "WHERE((a.rcvbls_invc_hdr_id = " + hdrID + "))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.pyblsFrm.docTmplt_SQL = strSql;
            return dtst;
        }

        public static string get_ScmRcvblsDocHdrNum(long srchdrID, string srcHdrType, int orgID)
        {
            string strSql = "";

            strSql = @"SELECT rcvbls_invc_number
  FROM accb.accb_rcvbls_invc_hdr a " +
              "WHERE((a.src_doc_hdr_id = " + srchdrID +
              " and a.src_doc_type='" + srcHdrType.Replace("'", "''") + "' and a.org_id=" + orgID + "))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            return "";
        }

        public static long get_ScmRcvblsDocHdrID(long srchdrID, string srcHdrType, int orgID)
        {
            string strSql = "";

            strSql = @"SELECT rcvbls_invc_hdr_id
  FROM accb.accb_rcvbls_invc_hdr a " +
              "WHERE((a.src_doc_hdr_id = " + srchdrID +
              " and a.src_doc_type='" + srcHdrType.Replace("'", "''") + "' and a.org_id=" + orgID + "))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static DataSet get_RcvblsDocHdr(string searchWord, string searchIn, long offset,
          int limit_size, long orgID)
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
            if (searchIn == "Document Number")
            {
                whrcls = " and (a.rcvbls_invc_number ilike '" + searchWord.Replace("'", "''") + "')";
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
                whrcls = " and (a.cstmrs_doc_num b ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Source Doc Number")
            {
                whrcls = @" and (a.src_doc_hdr_id IN (select d.invc_hdr_id from scm.scm_sales_invc_hdr d 
where trim(to_char(d.invc_hdr_id, '9999999999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
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
            strSql = @"SELECT rcvbls_invc_hdr_id, rcvbls_invc_number, rcvbls_invc_type 
        FROM accb.accb_rcvbls_invc_hdr a 
        WHERE((a.org_id = " + orgID + ")" + whrcls +
              ") ORDER BY rcvbls_invc_hdr_id DESC LIMIT " + limit_size +
              " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.rcvblsFrm.rec_SQL = strSql;
            return dtst;
        }

        public static long get_Total_RcvblsDoc(string searchWord, string searchIn, long orgID)
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
            if (searchIn == "Document Number")
            {
                whrcls = " and (a.rcvbls_invc_number ilike '" + searchWord.Replace("'", "''") + "')";
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
                whrcls = " and (a.cstmrs_doc_num b ilike '" + searchWord.Replace("'", "''") + "')";
            }
            else if (searchIn == "Source Doc Number")
            {
                whrcls = @" and (a.src_doc_hdr_id IN (select d.invc_hdr_id from scm.scm_sales_invc_hdr d 
where trim(to_char(d.invc_hdr_id, '9999999999999999999999999')) ilike '" + searchWord.Replace("'", "''") +
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
        WHERE((a.org_id = " + orgID + ")" + whrcls + ")";


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
       func_curr_amount, accnt_curr_amnt
  FROM accb.accb_rcvbl_amnt_smmrys a " +
              "WHERE((a.src_rcvbl_hdr_id = " + docHdrID + ")" + whrcls + ") ORDER BY rcvbl_smmry_type ASC ";

            //MessageBox.Show(strSql);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.rcvblsFrm.recDt_SQL = strSql;
            return dtst;
        }

        public static DataSet get_ScmRcvblsDocDets(long docHdrID)
        {
            string strSql = "";
            strSql = @"SELECT rcvbl_smmry_type, rcvbl_smmry_desc, SUM(rcvbl_smmry_amnt), 
       code_id_behind, auto_calc, incrs_dcrs1, 
       rvnu_acnt_id, incrs_dcrs2, rcvbl_acnt_id, appld_prepymnt_doc_id, 
       entrd_curr_id, func_curr_id,accnt_curr_id, func_curr_rate, accnt_curr_rate, 
       SUM(func_curr_amount), SUM(accnt_curr_amnt)
  FROM scm.scm_rcvbl_amnt_smmrys a " +
              "WHERE((a.src_rcvbl_hdr_id = " + docHdrID +
              @")) GROUP BY rcvbl_smmry_type, rcvbl_smmry_desc, 
       code_id_behind, auto_calc, incrs_dcrs1, 
       rvnu_acnt_id, incrs_dcrs2, rcvbl_acnt_id, appld_prepymnt_doc_id, 
       entrd_curr_id, func_curr_id,accnt_curr_id, func_curr_rate, accnt_curr_rate 
      ORDER BY rcvbl_smmry_type ASC ";

            //MessageBox.Show(strSql);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.rcvblsFrm.recDt_SQL = strSql;
            return dtst;
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

        public static double getScmRcvblsDocGrndAmnt(long dochdrID)
        {
            string strSql = @"select SUM(CASE WHEN y.rcvbl_smmry_type = '3Discount' 
or scm.istaxwthhldng(y.code_id_behind)='1' or y.rcvbl_smmry_type='5Applied Prepayment'
      THEN -1*y.rcvbl_smmry_amnt ELSE y.rcvbl_smmry_amnt END) amnt " +
              "from scm.scm_rcvbl_amnt_smmrys y " +
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

        public static double getScmRcvblsDocFuncAmnt(long dochdrID)
        {
            string strSql = @"select SUM(CASE WHEN y.rcvbl_smmry_type='3Discount' 
or scm.istaxwthhldng(y.code_id_behind)='1' or y.rcvbl_smmry_type='5Applied Prepayment'
      THEN -1*y.func_curr_amount ELSE y.func_curr_amount END) amnt " +
              "from scm.scm_rcvbl_amnt_smmrys y " +
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
      string apprvlSts, string nxtApprvl, double invcAmnt)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_rcvbls_invc_hdr SET " +
                  "approval_status='" + apprvlSts.Replace("'", "''") +
                  "', invoice_amount=" + invcAmnt + ", last_update_by=" + Global.myVst.user_id +
                  ", last_update_date='" + dateStr +
                  "', next_aproval_action='" + nxtApprvl.Replace("'", "''") +
                  "' WHERE (rcvbls_invc_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtRcvblsDocApprvl(long docid,
      string apprvlSts, string nxtApprvl)
        {
            string extrCls = "";

            if (apprvlSts == "Cancelled" || apprvlSts == "Declared Bad Debt")
            {
                extrCls = ", invoice_amount=0, invc_amnt_appld_elswhr=0";
            }
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_rcvbls_invc_hdr SET " +
                  "approval_status='" + apprvlSts.Replace("'", "''") +
                  "', last_update_by=" + Global.myVst.user_id +
                  ", last_update_date='" + dateStr +
                  "', next_aproval_action='" + nxtApprvl.Replace("'", "''") +
                  "'" + extrCls + " WHERE (rcvbls_invc_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        //  public static void updtRcvblsDocApprvl(long docid,
        //string apprvlSts, string nxtApprvl)
        //  {
        //   Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
        //   string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
        //   string updtSQL = "UPDATE accb.accb_rcvbls_invc_hdr SET " +
        //         "approval_status='" + apprvlSts.Replace("'", "''") +
        //         "', last_update_by=" + Global.myVst.user_id +
        //         ", last_update_date='" + dateStr +
        //         "', next_aproval_action='" + nxtApprvl.Replace("'", "''") +
        //         "' WHERE (rcvbls_invc_hdr_id = " +
        //         docid + ")";
        //   Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        //  }

        public static void updtRcvblsDocGLBatch(long docid,
      long glBatchID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_rcvbls_invc_hdr SET " +
                  "gl_batch_id=" + glBatchID +
                  ", last_update_by=" + Global.myVst.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (rcvbls_invc_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtRcvblsDocBadDbtGLBatch(long docid,
     long glBatchID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_rcvbls_invc_hdr SET " +
                  "debt_gl_batch_id=" + glBatchID +
                  ", last_update_by=" + Global.myVst.user_id +
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
                  ", last_update_by=" + Global.myVst.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (rcvbls_invc_hdr_id = " +
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
                  ", last_update_by=" + Global.myVst.user_id +
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

        public static DataSet get_Batch_Attachments(long batchID)
        {
            string strSql = "";

            strSql = "SELECT a.attchmnt_id, a.batch_id, a.attchmnt_desc, a.file_name " +
          "FROM accb.accb_batch_trns_attchmnts a " +
          "WHERE(a.batch_id = " + batchID + ") ORDER BY a.attchmnt_id";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
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

        public static long getSimlrPstdBatchID(string orgnlbatchname, int orgid)
        {
            long srcbatchid = Global.getBatchID(orgnlbatchname, orgid);
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

        public static DataSet get_WrongGLBatches(int orgID)
        {
            string strSql = "";
            strSql = @"select distinct batch_name, batch_id from (
SELECT b.transctn_id, c.batch_name, c.batch_id, b.trnsctn_date, b.source_trns_ids, 
d.accnt_id, d.accnt_name, b.dbt_amount, b.crdt_amount, COALESCE(round(SUM(a.dbt_amount),2),0), 
COALESCE(round(SUM(a.crdt_amount),2),0)
FROM scm.scm_gl_interface a, accb.accb_trnsctn_details b, accb.accb_trnsctn_batches c, accb.accb_chart_of_accnts d
WHERE (a.accnt_id = d.accnt_id and a.accnt_id = b.accnt_id and b.batch_id=c.batch_id and 
d.org_id=" + orgID + @" and c.batch_source ilike 'Inventory%'
and b.source_trns_ids like '%,' || a.interface_id || ',%') 
GROUP BY b.transctn_id, c.batch_name, c.batch_id, b.trnsctn_date, 
d.accnt_id, d.accnt_name, b.dbt_amount, b.crdt_amount
HAVING b.dbt_amount <> COALESCE(round(SUM(a.dbt_amount),2),0) or COALESCE(round(SUM(a.crdt_amount),2),0) <>  b.crdt_amount
) tbl1";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
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
            return dtst;
        }

        #endregion

        #region "SALES DOCUMENTS..."
        public static void deleteSalesSmmryItm(long docID, string docType, string smmryTyp)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSQL = "DELETE FROM scm.scm_doc_amnt_smmrys WHERE src_doc_hdr_id = " +
              docID + " and src_doc_type = '" + docType + "' and smmry_type = '" + smmryTyp +
              "' and code_id_behind = -1";
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteSalesSmmryItm(long docID, string docType, string smmryTyp, long codBhnd)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSQL = "DELETE FROM scm.scm_doc_amnt_smmrys WHERE src_doc_hdr_id = " +
              docID + " and src_doc_type = '" + docType + "' and smmry_type = '" + smmryTyp + "' and  code_id_behind= " + codBhnd;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
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
                  ", last_update_by = " + Global.myVst.user_id + ", " +
                  "auto_calc = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(autoCalc) +
                  "', last_update_date = '" + dateStr +
                  "', smmry_name='" + smmryNm.Replace("'", "''") + "' WHERE (smmry_id = " + smmryID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void deleteZeroSmmryItms(long docID, string docType)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSQL = "DELETE FROM scm.scm_doc_amnt_smmrys WHERE src_doc_hdr_id = " +
              docID + " and src_doc_type = '" + docType +
              "' and round(smmry_amnt,2) = 0 and (code_id_behind>0 or substr(smmry_type,1,1) IN ('2','3','4'))";
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void updateResetSmmryItm(long docID, string docType)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string updtSQL = "UPDATE scm.scm_doc_amnt_smmrys SET " +
                  "smmry_amnt = 0 WHERE (src_doc_type = '" + docType.Replace("'", "''") +
                  "' and src_doc_hdr_id = " + docID + " and (code_id_behind>0 or substr(smmry_type,1,1) IN ('2','3','4')))";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updateSmmryItmAddOn(long smmryID, string smmryTyp,
          double amnt, bool autoCalc, string smmryNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            if (smmryTyp == "3Discount")
            {
                amnt = -1 * Math.Abs(amnt);
            }
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE scm.scm_doc_amnt_smmrys SET " +
                  "smmry_amnt = COALESCE(smmry_amnt,0) + " + amnt +
                  ", last_update_by = " + Global.myVst.user_id + ", " +
                  "auto_calc = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(autoCalc) +
                  "', last_update_date = '" + dateStr +
                  "', smmry_name='" + smmryNm.Replace("'", "''") + "' WHERE (smmry_id = " + smmryID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static int getUserStoreID()
        {
            string strSql = "select y.subinv_id " +
              "from inv.inv_itm_subinventories y, inv.inv_user_subinventories z " +
              "where y.subinv_id=z.subinv_id and " +
              "y.allow_sales = '1' and z.user_id = " + Global.myVst.user_id +
              " and y.org_id= " + Global.mnFrm.cmCde.Org_id + " order by 1 LIMIT 1 OFFSET 0 ";
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

        public static void roundSmmryItms(long docHdrID, string docType)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string updtSQL = "UPDATE scm.scm_doc_amnt_smmrys SET " +
                  "smmry_amnt = ROUND(smmry_amnt,2) WHERE (src_doc_hdr_id = " + docHdrID +
                  " and src_doc_type='" + docType.Replace("'", "''") + "')";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void roundScmRcvblsDocAmnts(long hdrID, string docType)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE scm.scm_rcvbl_amnt_smmrys
   SET rcvbl_smmry_amnt = ROUND(rcvbl_smmry_amnt, 2), func_curr_amount=ROUND(func_curr_amount,2), accnt_curr_amnt=ROUND(func_curr_amount,2) " +
                  " WHERE src_rcvbl_hdr_id = " + hdrID + " and src_rcvbl_type='" + docType.Replace("'", "''") + "'";
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static DataSet get_DocSmryLns(long dochdrID, string docTyp)
        {
            string strSql = "SELECT a.smmry_id, CASE WHEN a.smmry_type='3Discount' THEN 'Discount' ELSE a.smmry_name END, " +
             "a.smmry_amnt, a.code_id_behind, a.smmry_type, a.auto_calc, REPLACE(REPLACE(a.smmry_type,'2Tax','3Tax'),'3Discount','2Discount') smtyp " +
             "FROM scm.scm_doc_amnt_smmrys a " +
             "WHERE((a.src_doc_hdr_id = " + dochdrID +
             ") and (a.src_doc_type='" + docTyp + "')) ORDER BY 7";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (Global.wfnVstFrm != null)
            {
                Global.wfnVstFrm.smmry_SQL = strSql;
            }
            return dtst;
        }

        public static string getRcvblsDocLastUpdate(long dochdrID, string docType)
        {
            string strSql = "select to_char(to_timestamp(MAX(y.last_update_date),'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') dte " +
              "from accb.accb_payments y " +
              "where y.src_doc_id = " + dochdrID + " and y.src_doc_typ = '" + docType.Replace("'", "''") + "'";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);

            if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            return Global.mnFrm.cmCde.getFrmtdDB_Date_time();
        }

        public static double get_DocSmryGrndTtl(long dochdrID, string docTyp)
        {
            string strSql = "SELECT a.smmry_amnt " +
             "FROM scm.scm_doc_amnt_smmrys a " +
             "WHERE((a.src_doc_hdr_id = " + dochdrID +
             ") and (a.src_doc_type='" + docTyp +
             "') and (a.smmry_type='5Grand Total'))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double res = 0;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out res);
            }
            return res;
        }

        public static double get_DocSmryOutsbls(long dochdrID, string docTyp)
        {
            string strSql = "SELECT a.smmry_amnt " +
             "FROM scm.scm_doc_amnt_smmrys a " +
             "WHERE((a.src_doc_hdr_id = " + dochdrID +
             ") and (a.src_doc_type='" + docTyp +
             "') and (a.smmry_type='7Change/Balance'))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double res = 0;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                double.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out res);
            }
            return res;
        }

        public static long getAttchmntID(string attchname, long hdrID)
        {
            string strSql = "";
            strSql = "SELECT a.attchmnt_id " +
         "FROM scm.scm_sales_doc_attchmnts a " +
            "WHERE ((a.attchmnt_desc = '" + attchname.Replace("'", "''") +
              "') AND (a.doc_hdr_id = " + hdrID + "))";

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

        public static void createAttachment(long hdrID, string attchDesc,
         string filNm)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO scm.scm_sales_doc_attchmnts(" +
                  "doc_hdr_id, attchmnt_desc, file_name, created_by, " +
                  "creation_date, last_update_by, last_update_date) " +
                              "VALUES (" + hdrID +
                              ", '" + attchDesc.Replace("'", "''") +
                              "', '" + filNm.Replace("'", "''") +
                              "', " + Global.myVst.user_id + ", '" + dateStr +
                              "', " + Global.myVst.user_id + ", '" + dateStr + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updateAttachment(long attchID, long hdrID, string attchDesc,
       string filNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE scm.scm_sales_doc_attchmnts SET " +
                  "doc_hdr_id=" + hdrID +
                              ", attchmnt_desc='" + attchDesc.Replace("'", "''") +
                              "', file_name='" + filNm.Replace("'", "''") +
                              "', last_update_by=" + Global.myVst.user_id +
                              ", last_update_date='" + dateStr + "' " +
                               "WHERE attchmnt_id = " + attchID;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void deleteAttchmnt(long attchid, string attchNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Attachment Name = " + attchNm;
            string delSql = "DELETE FROM scm.scm_sales_doc_attchmnts WHERE(attchmnt_id = " + attchid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }

        public static DataSet get_Attachments(string searchWord, string searchIn,
       Int64 offset, int limit_size, long hdrID, ref string attchSQL)
        {
            string strSql = "";
            if (searchIn == "Attachment Name/Description")
            {
                strSql = "SELECT a.attchmnt_id, a.doc_hdr_id, a.attchmnt_desc, a.file_name " +
              "FROM scm.scm_sales_doc_attchmnts a " +
              "WHERE(a.attchmnt_desc ilike '" + searchWord.Replace("'", "''") +
              "' and a.doc_hdr_id = " + hdrID + ") ORDER BY a.attchmnt_id LIMIT " + limit_size +
                  " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            attchSQL = strSql;
            return dtst;
        }

        public static long get_Total_Attachments(string searchWord,
          string searchIn, long hdrID)
        {
            string strSql = "";
            if (searchIn == "Attachment Name/Description")
            {
                strSql = "SELECT COUNT(1) " +
              "FROM scm.scm_sales_doc_attchmnts a " +
              "WHERE(a.attchmnt_desc ilike '" + searchWord.Replace("'", "''") +
              "' and a.doc_hdr_id = " + hdrID + ")";
            }
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            long sumRes = 0;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                long.TryParse(dtst.Tables[0].Rows[0][0].ToString(), out sumRes);
            }
            return sumRes;
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

        public static long getSalesSmmryItmID(string smmryType, long codeBhnd,
         long srcDocID, string srcDocTyp)
        {
            string strSql = "select y.smmry_id " +
              "from scm.scm_doc_amnt_smmrys y " +
              "where y.smmry_type= '" + smmryType + "' and y.code_id_behind = " + codeBhnd +
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

        public static long getScmRcvblsSmmryItmID(string smmryType, long codeBhnd,
        long srcDocID, string srcDocTyp)
        {
            string strSql = "select y.rcvbl_smmry_id " +
              "from scm.scm_rcvbl_amnt_smmrys y " +
              "where y.rcvbl_smmry_type= '" + smmryType + "' and y.code_id_behind = " + codeBhnd +
              " and y.src_rcvbl_type='" + srcDocTyp +
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

        public static double getSalesSmmryItmAmnt(string smmryType, long codeBhnd,
       long srcDocID, string srcDocTyp)
        {
            //" + codeBhnd +"
            string strSql = "select COALESCE(SUM(y.smmry_amnt),0) " +
              "from scm.scm_doc_amnt_smmrys y " +
              "where y.smmry_type= '" + smmryType +
              "' and y.code_id_behind= y.code_id_behind and y.src_doc_type='" + srcDocTyp +
              "' and y.src_doc_hdr_id=" + srcDocID + " ";
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

        public static double getSalesChrgsSum(
       long srcDocID, string srcDocTyp)
        {
            string strSql = "select COALESCE(SUM(y.smmry_amnt),0) " +
              "from scm.scm_doc_amnt_smmrys y " +
              "where y.smmry_type= '4Extra Charge' and y.code_id_behind >0 and y.src_doc_type='" + srcDocTyp +
              "' and y.src_doc_hdr_id=" + srcDocID + " ";
            Global.mnFrm.cmCde.showSQLNoPermsn(strSql);
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
        //  //CASE WHEN (smmry_type='2Tax') THEN -1*y.smmry_amnt ELSE 
        //  string strSql = "select SUM(y.smmry_amnt) amnt " +
        //    "from scm.scm_doc_amnt_smmrys y " +
        //    "where y.src_doc_hdr_id=" + dochdrID +
        //    " and y.src_doc_type='" + docTyp + "' and substr(y.smmry_type,1,1) IN ('2','5')";
        //  /* != '1Initial Amount' " +
        //    " and y.smmry_type != '6Total Payments Received' and y.smmry_type != " +
        //    "'7Change/Balance' and smmry_type!='3Discount' and smmry_type!='4Extra Charge'*/
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
            string strSql = "select SUM(y.rented_itm_qty * y.doc_qty*orgnl_selling_price) amnt " +
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

        public static double getSalesDocTtlAmnt(long dochdrID)
        {
            string strSql = "select SUM(y.rented_itm_qty * y.doc_qty*unit_selling_price) amnt " +
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
            //      string extrWhere = "";
            //      if (orgnlChckInID > 0)
            //      {
            //        extrWhere = @"and 
            //   (a.other_mdls_doc_id =" + orgnlChckInID + " and a.other_mdls_doc_type='" + orgnDocType.Replace("'", "''") + "')";
            //      }
            string strSql = "SELECT a.invc_det_ln_id, a.itm_id, " +
              "a.doc_qty, a.unit_selling_price, (a.rented_itm_qty * a.doc_qty * a.unit_selling_price) amnt, " +
              "a.store_id, a.crncy_id, (a.doc_qty - a.qty_trnsctd_in_dest_doc) avlbl_qty, " +
              "a.src_line_id, a.tax_code_id, a.dscnt_code_id, a.chrg_code_id, a.rtrn_reason, " +
              @"a.consgmnt_ids, a.orgnl_selling_price, b.base_uom_id, b.item_code, 
      CASE WHEN a.alternate_item_name='' THEN b.item_desc ELSE a.alternate_item_name END, " +
              @"c.uom_name, a.is_itm_delivered, REPLACE(a.extra_desc || ' (' || a.other_mdls_doc_type || ') " +
              @"(' || scm.get_src_doc_num(a.other_mdls_doc_id, a.other_mdls_doc_type) || ')',' ()','')
        , a.other_mdls_doc_id, a.other_mdls_doc_type, scm.get_src_doc_num(a.other_mdls_doc_id, a.other_mdls_doc_type), a.rented_itm_qty, a.alternate_item_name  " +
             "FROM scm.scm_sales_invc_det a, inv.inv_itm_list b, inv.unit_of_measure c  " +
             "WHERE(a.invc_hdr_id = " + dochdrID +
             " and a.invc_hdr_id >0 and a.itm_id = b.item_id and b.base_uom_id=c.uom_id) ORDER BY a.invc_det_ln_id";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (Global.wfnVstFrm != null)
            {
                Global.wfnVstFrm.recDt_SQL = strSql;
            }

            return dtst;
        }

        public static void updtOrgInvoiceCurrID(int orgID, int crncyID, long pymtMthdID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE scm.scm_sales_invc_hdr SET invc_curr_id = " + crncyID +
                              ", last_update_by = " + Global.myVst.user_id + ", " +
                              "last_update_date = '" + dateStr + "' " +
              "WHERE (org_id = " + orgID + " and invc_curr_id<=0)";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
            updtSQL = "UPDATE scm.scm_sales_invc_hdr SET pymny_method_id = " + pymtMthdID +
                              ", last_update_by = " + Global.myVst.user_id + ", " +
                              "last_update_date = '" + dateStr + "' " +
              "WHERE (org_id = " + orgID + " and pymny_method_id<=0)";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);

        }

        //    public static DataSet get_RoomsToClean(int orgID)
        //    {
        //      Global.updatePrvdrGrpApptmtCnt();
        //      string strSql = @"SELECT row_number() OVER (ORDER BY tbl1.col2, tbl1.col1) AS ""No.  "", 
        //                          tbl1.col1 ""Room/Facility No. "", 
        //                          tbl1.col2 ""Facility Type  "", 
        //                          tbl1.col3 ""Name of Occupant (Sponsor)                 "", 
        //                          COALESCE(tbl1.col4,0) ""No. of Occupants ""
        //                          FROM (Select a.room_name col1, b.service_type_name col2, 
        //                          (Select scm.get_cstmr_splr_name(y.customer_id) 
        //                          || ' (' || scm.get_cstmr_splr_name(y.sponsor_id) || ')'
        //                          FROM hotl.checkins_hdr y Where y.service_det_id=a.room_id 
        //                          and y.doc_status = 'Checked-In') col3, 
        //                           (Select SUM(no_of_adults+no_of_children) 
        //                          FROM hotl.checkins_hdr y Where y.service_det_id=a.room_id 
        //                          and y.doc_status = 'Checked-In') col4 
        //                          FROM hotl.rooms a, hotl.service_types b 
        //                          where a.service_type_id = b.service_type_id and b.org_id = " + orgID + @" and 
        //                          (a.needs_hse_keeping='1' or a.crnt_no_occpnts>0) and b.type_of_facility NOT IN ('Restaurant Table')) tbl1 
        //                          ORDER BY tbl1.col2, tbl1.col1";

        //      DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
        //      return dtst;
        //    }

        public static long get_OccpntRoomsCnt(int cstmrID, ref string cstmrNm)
        {
            //Global.updateRoomOccpntCnt();
            string whrcls = "";
            if (cstmrID > 0)
            {
                whrcls = " and y.customer_id=" + cstmrID;
            }
            string strSql = @"SELECT Count(a.room_name), scm.get_cstmr_splr_name(y.customer_id)
                          FROM hotl.rooms a, hotl.service_types b, hotl.checkins_hdr y 
                          where a.service_type_id = b.service_type_id  and 
                          y.service_det_id=a.room_id and y.doc_status = 'Checked-In' 
                          and b.type_of_facility IN ('Room/Hall') and (now() between to_timestamp(start_date,'YYYY-MM-DD HH24:MI:SS') 
                          and to_timestamp(end_date,'YYYY-MM-DD HH24:MI:SS'))" + whrcls + @"
                          GROUP BY y.customer_id
                          HAVING Count(a.room_name)>1";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                cstmrNm = dtst.Tables[0].Rows[0][1].ToString();
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }


        public static DataSet get_SalesMoneyRcvd(long UsrID, string doctype,
       string strtDte, string endDte, int orgID, string ordrBy, bool useCreatnDte)
        {
            /*y.user_name ""Sales Agent"",*/
            string usrCls = "";
            string usrNmSect = " ||' ('||y.user_name||')'";
            if (UsrID > 0)
            {
                usrCls = " and (y.user_id = " + UsrID + ")";
                usrNmSect = "";
            }
            if (ordrBy == "OUTSTANDING AMOUNT")
            {
                ordrBy = @"tbl1.col5 DESC, tbl1.col7, tbl1.col1 ASC";
            }
            else if (ordrBy == "TOTAL AMOUNT")
            {
                ordrBy = @"tbl1.col2 DESC, tbl1.col7, tbl1.col1 ASC";
            }
            else
            {
                ordrBy = "tbl1.col7, tbl1.col1 ASC";
            }

            string strSql = "";
            if (useCreatnDte)
            {
                strSql = @"SELECT row_number() OVER (ORDER BY " + ordrBy + @") AS ""No.  ""
, tbl1.col1 ""Document No.                     "", tbl1.col2 ""  Invoice Amount"", tbl1.col3 "" Discount Amount"",
tbl1.col4 ""     Amount Paid"", tbl1.col5 ""Outstanding Amt."", tbl1.col6 ""Creation Date           "", tbl1.col7 ""mt""
FROM (SELECT REPLACE(a.invc_number || ' (' || COALESCE(scm.get_cstmr_splr_name(a.customer_id),'Unspecified') 
|| ')' || ' (' || hotl.get_invc_room_num(a.invc_hdr_id) || ')-' || gst.get_pssbl_val(a.invc_curr_id),' ()','') col1, 
scm.get_doc_smry_typ_amnt(a.invc_hdr_id, a.invc_type, '5Grand Total') + 
abs(scm.get_doc_smry_typ_amnt(a.invc_hdr_id, a.invc_type, '3Discount')) col2, 
scm.get_doc_smry_typ_amnt(a.invc_hdr_id, a.invc_type, '3Discount') col3,
scm.get_doc_smry_typ_amnt(a.invc_hdr_id, a.invc_type, '6Total Payments Received') col4, 
scm.get_doc_smry_typ_amnt(a.invc_hdr_id, a.invc_type, '7Change/Balance') col5, 
to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),
'DD-Mon-YYYY HH24:MI:SS')" + usrNmSect + @" col6,
a.creation_date col7 
FROM scm.scm_sales_invc_hdr a, 
sec.sec_users y WHERE ((a.approval_status ilike 'Approved' or 
(Select count(q.invc_det_ln_id) from scm.scm_sales_invc_det q 
where q.invc_hdr_id = a.invc_hdr_id and q.is_itm_delivered='1')>0) AND (a.org_id = " + orgID + @") AND 
(a.created_by=y.user_id)" + usrCls + " and (a.invc_type ilike '" + doctype.Replace("'", "''") + @"') 
and (to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS') between 
to_timestamp('" + strtDte + @"','DD-Mon-YYYY HH24:MI:SS') AND 
to_timestamp('" + endDte + @"','DD-Mon-YYYY HH24:MI:SS'))) 
UNION
SELECT a.rcvbls_invc_number  || ' (' || COALESCE(scm.get_cstmr_splr_name(a.customer_id),'Unspecified') || ')-' || gst.get_pssbl_val(a.invc_curr_id) col1, 
CASE WHEN a.advc_pay_ifo_doc_id<=0 THEN accb.get_rcvbl_smry_typ_amnt(a.rcvbls_invc_hdr_id, a.rcvbls_invc_type, '6Grand Total') + 
abs(accb.get_rcvbl_smry_typ_amnt(a.rcvbls_invc_hdr_id, a.rcvbls_invc_type, '3Discount')) ELSE 0 END col2, 
accb.get_rcvbl_smry_typ_amnt(a.rcvbls_invc_hdr_id, a.rcvbls_invc_type, '3Discount') col3,
accb.get_rcvbl_smry_typ_amnt(a.rcvbls_invc_hdr_id, a.rcvbls_invc_type, '7Total Payments Made') col4, 
accb.get_rcvbl_smry_typ_amnt(a.rcvbls_invc_hdr_id, a.rcvbls_invc_type, '8Outstanding Balance') col5, 
to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),
'DD-Mon-YYYY HH24:MI:SS')" + usrNmSect + @" col6, a.creation_date col7 
FROM accb.accb_rcvbls_invc_hdr a, 
sec.sec_users y WHERE ((a.approval_status ilike 'Approved') AND (a.org_id = " + orgID + @") AND 
(a.created_by=y.user_id)" + usrCls + @" and ((a.src_doc_hdr_id||'.'||a.src_doc_type) " +
        "NOT IN (Select v.invc_hdr_id||'.'||v.invc_type from scm.scm_sales_invc_hdr v where v.org_id = " + orgID +
        @" and v.invc_type ilike '" + doctype.Replace("'", "''") + @"')) 
and a.invc_amnt_appld_elswhr <= 0 
/*(a.rcvbls_invc_type ilike '%Advance%Payment%')*/ 
and (to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS') between 
to_timestamp('" + strtDte + @"','DD-Mon-YYYY HH24:MI:SS') AND 
to_timestamp('" + endDte + @"','DD-Mon-YYYY HH24:MI:SS')))
UNION
SELECT a.mass_pay_name col1, 
pay.get_intrnlpay_salesamnt(a.mass_pay_id) col2, 
0 col3,
pay.get_intrnlpay_salesamnt(a.mass_pay_id) col4, 
0 col5, 
to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'), 'DD-Mon-YYYY HH24:MI:SS')" + usrNmSect + @" col6, 
a.creation_date col7 
FROM pay.pay_mass_pay_run_hdr a, 
sec.sec_users y WHERE ((a.run_status = '1' and a.sent_to_gl = '1') AND (a.org_id = " + orgID + @") AND 
(a.created_by=y.user_id)" + usrCls + @" and pay.get_intrnlpay_salesamnt(a.mass_pay_id)!=0 
and (to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS') between 
to_timestamp('" + strtDte + @"','DD-Mon-YYYY HH24:MI:SS') AND 
to_timestamp('" + endDte + @"','DD-Mon-YYYY HH24:MI:SS')))) tbl1 
ORDER BY " + ordrBy + @"";
            }
            else
            {
                strSql = @"SELECT row_number() OVER (ORDER BY " + ordrBy + @") AS ""No.  ""
, tbl1.col1 ""Document No.                     "", tbl1.col2 ""  Invoice Amount"", tbl1.col3 "" Discount Amount"",
tbl1.col4 ""     Amount Paid"", tbl1.col5 ""Outstanding Amt."", tbl1.col6 ""Document Date           "", tbl1.col7 ""mt""
FROM (SELECT REPLACE(a.invc_number || ' (' || COALESCE(scm.get_cstmr_splr_name(a.customer_id),'Unspecified') 
|| ')' || ' (' || hotl.get_invc_room_num(a.invc_hdr_id) || ')-' || gst.get_pssbl_val(a.invc_curr_id),' ()','') col1, 
scm.get_doc_smry_typ_amnt(a.invc_hdr_id, a.invc_type, '5Grand Total') + 
abs(scm.get_doc_smry_typ_amnt(a.invc_hdr_id, a.invc_type, '3Discount')) col2, 
scm.get_doc_smry_typ_amnt(a.invc_hdr_id, a.invc_type, '3Discount') col3,
scm.get_doc_smry_typ_amnt(a.invc_hdr_id, a.invc_type, '6Total Payments Received') col4, 
scm.get_doc_smry_typ_amnt(a.invc_hdr_id, a.invc_type, '7Change/Balance') col5, 
to_char(to_timestamp(a.invc_date || ' 00:00:00','YYYY-MM-DD HH24:MI:SS'),
'DD-Mon-YYYY HH24:MI:SS')" + usrNmSect + @" col6,
a.invc_date || ' 00:00:00' col7 
FROM scm.scm_sales_invc_hdr a, 
sec.sec_users y WHERE ((a.approval_status ilike 'Approved' or 
(Select count(q.invc_det_ln_id) from scm.scm_sales_invc_det q 
where q.invc_hdr_id = a.invc_hdr_id and q.is_itm_delivered='1')>0) AND (a.org_id = " + orgID + @") AND 
(a.created_by=y.user_id)" + usrCls + " and (a.invc_type ilike '" + doctype.Replace("'", "''") + @"') 
and (to_timestamp(a.invc_date|| ' 00:00:00','YYYY-MM-DD HH24:MI:SS') between 
to_timestamp('" + strtDte + @"','DD-Mon-YYYY HH24:MI:SS') AND 
to_timestamp('" + endDte + @"','DD-Mon-YYYY HH24:MI:SS'))) 
UNION
SELECT a.rcvbls_invc_number  || ' (' || COALESCE(scm.get_cstmr_splr_name(a.customer_id),'Unspecified') || ')-' || gst.get_pssbl_val(a.invc_curr_id) col1, 
CASE WHEN a.advc_pay_ifo_doc_id<=0 THEN accb.get_rcvbl_smry_typ_amnt(a.rcvbls_invc_hdr_id, a.rcvbls_invc_type, '6Grand Total') + 
abs(accb.get_rcvbl_smry_typ_amnt(a.rcvbls_invc_hdr_id, a.rcvbls_invc_type, '3Discount')) ELSE 0 END col2, 
accb.get_rcvbl_smry_typ_amnt(a.rcvbls_invc_hdr_id, a.rcvbls_invc_type, '3Discount') col3,
accb.get_rcvbl_smry_typ_amnt(a.rcvbls_invc_hdr_id, a.rcvbls_invc_type, '7Total Payments Made') col4, 
accb.get_rcvbl_smry_typ_amnt(a.rcvbls_invc_hdr_id, a.rcvbls_invc_type, '8Outstanding Balance') col5, 
to_char(to_timestamp(a.rcvbls_invc_date || ' 00:00:00','YYYY-MM-DD HH24:MI:SS'),
'DD-Mon-YYYY HH24:MI:SS')||' ('||y.user_name||')' col6, a.rcvbls_invc_date || ' 00:00:00' col7 
FROM accb.accb_rcvbls_invc_hdr a, 
sec.sec_users y WHERE ((a.approval_status ilike 'Approved') AND (a.org_id = " + orgID + @") AND 
(a.created_by=y.user_id)" + usrCls + @" and ((a.src_doc_hdr_id||'.'||a.src_doc_type) " +
        "NOT IN (Select v.invc_hdr_id||'.'||v.invc_type from scm.scm_sales_invc_hdr v where v.org_id = " + orgID +
        @" and v.invc_type ilike '" + doctype.Replace("'", "''") + @"')) 
and a.invc_amnt_appld_elswhr <= 0 
/*(a.rcvbls_invc_type ilike '%Advance%Payment%')*/ 
and (to_timestamp(a.rcvbls_invc_date || ' 00:00:00','YYYY-MM-DD HH24:MI:SS') between 
to_timestamp('" + strtDte + @"','DD-Mon-YYYY HH24:MI:SS') AND 
to_timestamp('" + endDte + @"','DD-Mon-YYYY HH24:MI:SS')))
UNION
SELECT a.mass_pay_name col1, 
pay.get_intrnlpay_salesamnt(a.mass_pay_id) col2, 
0 col3,
pay.get_intrnlpay_salesamnt(a.mass_pay_id) col4, 
0 col5, 
to_char(to_timestamp(a.mass_pay_trns_date,'YYYY-MM-DD HH24:MI:SS'), 'DD-Mon-YYYY HH24:MI:SS')" + usrNmSect + @" col6, 
a.mass_pay_trns_date col7 
FROM pay.pay_mass_pay_run_hdr a, 
sec.sec_users y WHERE ((a.run_status = '1' and a.sent_to_gl = '1') AND (a.org_id = " + orgID + @") AND 
(a.created_by=y.user_id)" + usrCls + @" and pay.get_intrnlpay_salesamnt(a.mass_pay_id)!=0 
and (to_timestamp(a.mass_pay_trns_date,'YYYY-MM-DD HH24:MI:SS') between 
to_timestamp('" + strtDte + @"','DD-Mon-YYYY HH24:MI:SS') AND 
to_timestamp('" + endDte + @"','DD-Mon-YYYY HH24:MI:SS')))) tbl1 
ORDER BY " + ordrBy + @"";
            }

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }


        public static DataSet get_PymtsMoneyRcvd(long UsrID, string doctype,
       string strtDte, string endDte, int orgID, string ordrBy, bool useCreatnDte)
        {
            /*y.user_name ""Sales Agent"",*/
            string usrCls = "";
            string usrNmSect = " ||' ('||y.user_name||')'";
            if (UsrID > 0)
            {
                usrCls = " and (y.user_id = " + UsrID + ")";
                usrNmSect = "";
            }
            if (ordrBy == "OUTSTANDING AMOUNT")
            {
                ordrBy = @"tbl1.col5 DESC, tbl1.col7, tbl1.col1 ASC";
            }
            else if (ordrBy == "TOTAL AMOUNT")
            {
                ordrBy = @"tbl1.col2 DESC, tbl1.col7, tbl1.col1 ASC";
            }
            else
            {
                ordrBy = "tbl1.col7, tbl1.col1 ASC";
            }

            string strSql = "";
            string dateClause = "";
            string dateClauseR = "";
            string dateClauseM = "";
            if (useCreatnDte)
            {
                dateClause = "(CASE WHEN z.creation_date IS NULL THEN a.creation_date ELSE z.creation_date END)";
                dateClauseR = "(CASE WHEN z.creation_date IS NULL THEN a.creation_date ELSE z.creation_date END)";
                dateClauseM = "a.creation_date";
            }
            else
            {
                dateClause = "(CASE WHEN z.pymnt_date IS NULL THEN a.invc_date || ' 00:00:00' ELSE z.pymnt_date END)";
                dateClauseR = "(CASE WHEN z.pymnt_date IS NULL THEN a.rcvbls_invc_date || ' 00:00:00' ELSE z.pymnt_date END)";
                dateClauseM = "a.mass_pay_trns_date";
            }

            strSql = @"SELECT row_number() OVER (ORDER BY " + ordrBy + @") AS ""No.  ""
, tbl1.col1 ""Document No.                     "", tbl1.col2 ""  Invoice Amount"", tbl1.col3 "" Discount Amount"",
tbl1.col4 ""     Amount Paid"", tbl1.col5 ""Outstanding Amt."", tbl1.col6 "" Date                      "", tbl1.col7 ""mt""
FROM (SELECT REPLACE(a.invc_number || ' (' || COALESCE(scm.get_cstmr_splr_name(a.customer_id),'Unspecified') 
|| ')' || ' (' || hotl.get_invc_room_num(a.invc_hdr_id) || ')-' || gst.get_pssbl_val(a.invc_curr_id),' ()','') col1, 
scm.get_doc_smry_typ_amnt(a.invc_hdr_id, a.invc_type, '5Grand Total') + 
abs(scm.get_doc_smry_typ_amnt(a.invc_hdr_id, a.invc_type, '3Discount')) col2, 
scm.get_doc_smry_typ_amnt(a.invc_hdr_id, a.invc_type, '3Discount') col3,
COALESCE(z.amount_paid,0) col4, 
scm.get_doc_smry_typ_amnt(a.invc_hdr_id, a.invc_type, '7Change/Balance') col5, 
to_char(to_timestamp(" + dateClause + @",'YYYY-MM-DD HH24:MI:SS'),
'DD-Mon-YYYY HH24:MI:SS')" + usrNmSect + @" col6, " + dateClause + @" col7 
FROM scm.scm_sales_invc_hdr a 
LEFT OUTER JOIN accb.accb_rcvbls_invc_hdr x ON (x.src_doc_type=a.invc_type and x.src_doc_hdr_id = a.invc_hdr_id)
LEFT OUTER JOIN accb.accb_payments z ON (z.src_doc_typ=x.rcvbls_invc_type and z.src_doc_id=x.rcvbls_invc_hdr_id and z.orgnl_pymnt_id<=0 and z.pymnt_vldty_status='VALID')
LEFT OUTER JOIN sec.sec_users y ON (z.created_by=y.user_id)
WHERE ((a.approval_status ilike 'Approved' or 
(Select count(q.invc_det_ln_id) from scm.scm_sales_invc_det q 
where q.invc_hdr_id = a.invc_hdr_id and q.is_itm_delivered='1') > 0) AND (a.org_id = " + orgID + @") " + usrCls + " and (a.invc_type ilike '" + doctype.Replace("'", "''") + @"') 
and (to_timestamp(" + dateClause + @", 'YYYY-MM-DD HH24:MI:SS') between 
to_timestamp('" + strtDte + @"', 'DD-Mon-YYYY HH24:MI:SS') AND 
to_timestamp('" + endDte + @"', 'DD-Mon-YYYY HH24:MI:SS')) AND COALESCE(z.created_by,-123)=y.user_id 
AND COALESCE(z.prepay_doc_id, -123)<0) 
UNION
SELECT a.rcvbls_invc_number  || ' (' || COALESCE(scm.get_cstmr_splr_name(a.customer_id),'Unspecified') || ')-' || gst.get_pssbl_val(a.invc_curr_id) col1, 
CASE WHEN a.advc_pay_ifo_doc_id<=0 THEN accb.get_rcvbl_smry_typ_amnt(a.rcvbls_invc_hdr_id, a.rcvbls_invc_type, '6Grand Total') + 
abs(accb.get_rcvbl_smry_typ_amnt(a.rcvbls_invc_hdr_id, a.rcvbls_invc_type, '3Discount')) ELSE 0 END col2, 
accb.get_rcvbl_smry_typ_amnt(a.rcvbls_invc_hdr_id, a.rcvbls_invc_type, '3Discount') col3,
COALESCE(z.amount_paid,0) col4, 
accb.get_rcvbl_smry_typ_amnt(a.rcvbls_invc_hdr_id, a.rcvbls_invc_type, '8Outstanding Balance') col5, 
to_char(to_timestamp(" + dateClauseR + @",'YYYY-MM-DD HH24:MI:SS'),
'DD-Mon-YYYY HH24:MI:SS')" + usrNmSect + @" col6, " + dateClauseR + @" col7 
FROM accb.accb_rcvbls_invc_hdr a
LEFT OUTER JOIN accb.accb_payments z ON (z.src_doc_typ=a.rcvbls_invc_type and z.src_doc_id=a.rcvbls_invc_hdr_id and z.orgnl_pymnt_id<=0 and z.pymnt_vldty_status='VALID') 
LEFT OUTER JOIN sec.sec_users y ON (z.created_by=y.user_id) 
WHERE ((a.approval_status ilike 'Approved') AND (a.org_id = " + orgID + @") " + usrCls + @" and ((a.src_doc_hdr_id||'.'||a.src_doc_type) " +
"NOT IN (Select v.invc_hdr_id||'.'||v.invc_type from scm.scm_sales_invc_hdr v where v.org_id = " + orgID +
@" and v.invc_type ilike '" + doctype.Replace("'", "''") + @"')) 
/*and a.invc_amnt_appld_elswhr <= 0*/ 
and (to_timestamp(" + dateClauseR + @",'YYYY-MM-DD HH24:MI:SS') between 
to_timestamp('" + strtDte + @"','DD-Mon-YYYY HH24:MI:SS') AND 
to_timestamp('" + endDte + @"','DD-Mon-YYYY HH24:MI:SS')) AND COALESCE(z.created_by,-123)=y.user_id 
AND COALESCE(z.prepay_doc_id, -123)<0)
UNION
SELECT a.mass_pay_name col1, 
pay.get_intrnlpay_salesamnt(a.mass_pay_id) col2, 
0 col3,
pay.get_intrnlpay_salesamnt(a.mass_pay_id) col4, 
0 col5, 
to_char(to_timestamp(" + dateClauseM + @",'YYYY-MM-DD HH24:MI:SS'), 'DD-Mon-YYYY HH24:MI:SS')" + usrNmSect + @" col6, 
" + dateClauseM + @" col7 
FROM pay.pay_mass_pay_run_hdr a, 
sec.sec_users y WHERE ((a.run_status = '1' and a.sent_to_gl = '1') AND (a.org_id = " + orgID + @") AND 
(a.created_by=y.user_id)" + usrCls + @" and pay.get_intrnlpay_salesamnt(a.mass_pay_id)!=0 
and (to_timestamp(" + dateClauseM + @",'YYYY-MM-DD HH24:MI:SS') between 
to_timestamp('" + strtDte + @"','DD-Mon-YYYY HH24:MI:SS') AND 
to_timestamp('" + endDte + @"','DD-Mon-YYYY HH24:MI:SS')))) tbl1 
ORDER BY " + ordrBy + @"";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_ItemsSold(long UsrID, string doctype, string strtDte, string endDte, int orgID, string ordrBy)
        {
            /*
         y.user_name ""Sales Agent"",*/
            string usrCls = "";

            if (UsrID > 0)
            {
                usrCls = " and (y.user_id = " + UsrID + ")";
            }
            if (ordrBy == "QTY")
            {
                ordrBy = "SUM(b.doc_qty)  DESC,";
            }
            else if (ordrBy == "TOTAL AMOUNT")
            {
                ordrBy = "SUM(b.doc_qty * b.unit_selling_price)  DESC,";
            }
            else
            {
                ordrBy = "";
            }
            //trim(REPLACE(a.invc_number || ' (' || a.comments_desc || ')','()','')) 
            //, a.invc_number, a.comments_desc a.approval_status ilike 'Approved' or 
            string strSql = @"SELECT row_number() OVER (ORDER BY " + ordrBy + @" c.item_desc ASC) AS ""No.  ""
        , CASE WHEN b.alternate_item_name = '' THEN 
          trim(c.item_code || ' ' || REPLACE(c.item_desc,c.item_code,'')) ELSE b.alternate_item_name END ""Item Code/Desc.  "", 
        array_to_string(array_agg(distinct REPLACE(a.invc_number || ' (' || hotl.get_invc_room_num(a.invc_hdr_id) || ')','()','')),', ') ""Document Numbers           "", 
        SUM(b.doc_qty) ""QTY      "", 
        d.uom_name ""UOM     "", 
        b.unit_selling_price ""Sales Price "", 
        SUM(b.doc_qty * b.unit_selling_price) ""Total Amount  "",
        c.item_desc mt,
        gst.get_pssbl_val(b.crncy_id) ""Curr. ""
        FROM scm.scm_sales_invc_hdr a, sec.sec_users y, scm.scm_sales_invc_det b, inv.inv_itm_list c, inv.unit_of_measure d
        WHERE ((a.invc_hdr_id = b.invc_hdr_id AND b.itm_id = c.item_id AND c.base_uom_id = d.uom_id) 
        AND (b.is_itm_delivered ='1') AND (a.org_id = " + orgID + @") AND 
        (b.created_by=y.user_id)" + usrCls + " and (a.invc_type ilike '" + doctype.Replace("'", "''") + @"') 
        and (to_timestamp(b.creation_date,'YYYY-MM-DD HH24:MI:SS') between 
        to_timestamp('" + strtDte + @"','DD-Mon-YYYY HH24:MI:SS') AND 
        to_timestamp('" + endDte + @"','DD-Mon-YYYY HH24:MI:SS'))) 
        GROUP BY b.alternate_item_name, c.item_desc, b.itm_id, c.item_code, d.uom_name, b.unit_selling_price, b.crncy_id
        ORDER BY " + ordrBy + @" c.item_desc ASC, b.alternate_item_name ASC";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_One_SalesDcDt(long dochdrID)
        {
            string strSql = "SELECT a.invc_hdr_id, a.invc_number, " +
              @"a.invc_type, a.src_doc_hdr_id, 
      to_char(to_timestamp(a.invc_date,'YYYY-MM-DD'),'DD-Mon-YYYY'), " +
             "a.customer_id, a.customer_site_id, a.comments_desc, a.payment_terms, " +
             "a.approval_status, a.next_aproval_action, " +
             "a.created_by, a.pymny_method_id, accb.get_pymnt_mthd_name(a.pymny_method_id), " +
             "a.invc_curr_id, gst.get_pssbl_val(a.invc_curr_id), a.exchng_rate, " +
             "a.other_mdls_doc_id,scm.get_src_doc_num(a.other_mdls_doc_id,a.other_mdls_doc_type) doc_no, a.other_mdls_doc_type " +
             "FROM scm.scm_sales_invc_hdr a " +
             "WHERE(a.invc_hdr_id = " + dochdrID +
             ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_Basic_SalesDoc(
         string searchWord, string searchIn,
      Int64 offset, int limit_size, int orgID, bool onlySelf, bool shwUnpstdOnly)
        {
            string strSql = "";
            string whereClause = "";
            string crtdByClause = "";
            string unpstdCls = "";
            if (shwUnpstdOnly)
            {
                unpstdCls = @" AND EXISTS (SELECT f.src_doc_hdr_id 
FROM scm.scm_doc_amnt_smmrys f WHERE f.smmry_type='7Change/Balance' 
and round(f.smmry_amnt,2)>0 and a.invc_hdr_id=f.src_doc_hdr_id and f.src_doc_type=a.invc_type)";
                //unpstdCls = " AND (a.approval_status!='Approved')";
            }
            if (onlySelf == true)
            {
                crtdByClause = " AND (created_by=" + Global.mnFrm.cmCde.User_id + ")";
            }
            if (searchIn == "Document Number")
            {
                whereClause = "(a.invc_number ilike '" + searchWord.Replace("'", "''") +
              "') AND ";
            }
            else if (searchIn == "Document Description")
            {
                whereClause = "(a.comments_desc ilike '" + searchWord.Replace("'", "''") +
            "') AND ";
            }
            else if (searchIn == "Customer Name")
            {
                whereClause = "(a.customer_id IN (select c.cust_sup_id from scm.scm_cstmr_suplr c where c.cust_sup_name ilike '" + searchWord.Replace("'", "''") +
            "')) AND ";
            }
            else if (searchIn == "Source Doc. Number")
            {
                whereClause = "(a.src_doc_hdr_id IN (select c.invc_hdr_id from scm.scm_sales_invc_hdr c where c.invc_number ilike '" + searchWord.Replace("'", "''") +
            "')) AND ";
            }
            else if (searchIn == "Approval Status")
            {
                whereClause = "(a.approval_status ilike '" + searchWord.Replace("'", "''") +
            "') AND ";
            }
            else if (searchIn == "Created By")
            {
                whereClause = "(a.created_by IN (select c.user_id from sec.sec_users c where c.user_name ilike '" + searchWord.Replace("'", "''") +
            "')) AND ";
            }

            strSql = "SELECT a.invc_hdr_id, a.invc_number, a.invc_type " +
         "FROM scm.scm_sales_invc_hdr a " +
         "WHERE (" + whereClause + "(a.org_id = " + orgID +
         ")" + crtdByClause + unpstdCls + ") ORDER BY a.invc_hdr_id DESC LIMIT " + limit_size +
         " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            //Global.invcFrm.rec_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_Total_SalesDoc(string searchWord, string searchIn, int orgID, bool onlySelf, bool shwUnpstdOnly)
        {
            string strSql = "";
            string whereClause = "";
            string crtdByClause = "";
            string unpstdCls = "";
            if (shwUnpstdOnly)
            {
                unpstdCls = @" AND EXISTS (SELECT f.src_doc_hdr_id 
FROM scm.scm_doc_amnt_smmrys f WHERE f.smmry_type='7Change/Balance' 
and round(f.smmry_amnt,2)>0 and a.invc_hdr_id=f.src_doc_hdr_id and f.src_doc_type=a.invc_type) ";
                //unpstdCls = " AND (a.approval_status!='Approved')";
            }
            if (onlySelf == true)
            {
                crtdByClause = " AND (created_by=" + Global.mnFrm.cmCde.User_id + ")";
            }
            if (searchIn == "Document Number")
            {
                whereClause = "(a.invc_number ilike '" + searchWord.Replace("'", "''") +
              "') AND ";
            }
            else if (searchIn == "Document Description")
            {
                whereClause = "(a.comments_desc ilike '" + searchWord.Replace("'", "''") +
            "') AND ";
            }
            else if (searchIn == "Customer Name")
            {
                whereClause = "(a.customer_id IN (select c.cust_sup_id from scm.scm_cstmr_suplr c where c.cust_sup_name ilike '" + searchWord.Replace("'", "''") +
            "')) AND ";
            }
            else if (searchIn == "Source Doc. Number")
            {
                whereClause = "(a.src_doc_hdr_id IN (select c.invc_hdr_id from scm.scm_sales_invc_hdr c where c.invc_number ilike '" + searchWord.Replace("'", "''") +
            "')) AND ";
            }
            else if (searchIn == "Approval Status")
            {
                whereClause = "(a.approval_status ilike '" + searchWord.Replace("'", "''") +
            "') AND ";
            }
            else if (searchIn == "Created By")
            {
                whereClause = "(a.created_by IN (select c.user_id from sec.sec_users c where c.user_name ilike '" + searchWord.Replace("'", "''") +
            "')) AND ";
            }
            strSql = "SELECT count(1) " +
            "FROM scm.scm_sales_invc_hdr a " +
          "WHERE (" + whereClause + "(a.org_id = " + orgID + ")" + crtdByClause + unpstdCls + ")";
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

        public static void createPymntGLLine(int accntid, string trnsdesc, double dbtamnt,
        string trnsdte, int crncyid, long batchid, double crdtamnt, double netamnt,
          string srcids, string dateStr,
          double entrdAmt, int entrdCurrID, double acntAmnt, int acntCurrID,
          double funcExchRate, double acntExchRate, string dbtOrCrdt)
        {
            if (accntid <= 0)
            {
                return;
            }
            trnsdte = DateTime.ParseExact(
         trnsdte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            dateStr = DateTime.ParseExact(
         dateStr, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string insSQL = "INSERT INTO accb.accb_trnsctn_details(" +
                     "accnt_id, transaction_desc, dbt_amount, trnsctn_date, " +
                     "func_cur_id, created_by, creation_date, batch_id, crdt_amount, " +
                     @"last_update_by, last_update_date, net_amount, trns_status, source_trns_ids, 
            entered_amnt, entered_amt_crncy_id, accnt_crncy_amnt, accnt_crncy_id, 
            func_cur_exchng_rate, accnt_cur_exchng_rate, dbt_or_crdt) " +
                     "VALUES (" + accntid + ", '" + trnsdesc.Replace("'", "''") + "', " + dbtamnt +
                     ", '" + trnsdte.Replace("'", "''") + "', " + crncyid + ", " + Global.myVst.user_id +
                     ", '" + dateStr + "', " + batchid + ", " + crdtamnt + ", " +
                     Global.myVst.user_id + ", '" + dateStr + "', " + netamnt +
                     ", '0', '" + srcids + "', " + entrdAmt +
                              ", " + entrdCurrID + ", " + acntAmnt +
                              ", " + acntCurrID + ", " + funcExchRate +
                              ", " + acntExchRate + ", '" + dbtOrCrdt + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void createPymntGLIntFcLn(int accntid, string trnsdesc, double dbtamnt,
      string trnsdte, int crncyid, double crdtamnt, double netamnt, string srcDocTyp,
          long srcDocID, long srcDocLnID, string dateStr)
        {
            if (accntid <= 0)
            {
                return;
            }
            trnsdte = DateTime.ParseExact(
         trnsdte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            dateStr = DateTime.ParseExact(
         dateStr, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string insSQL = "INSERT INTO scm.scm_gl_interface(" +
                  "accnt_id, transaction_desc, dbt_amount, trnsctn_date, " +
                  "func_cur_id, created_by, creation_date, crdt_amount, last_update_by, " +
                  "last_update_date, net_amount, gl_batch_id, src_doc_typ, src_doc_id, " +
                  "src_doc_line_id) " +
                     "VALUES (" + accntid + ", '" + trnsdesc.Replace("'", "''") + "', " + dbtamnt +
                     ", '" + trnsdte.Replace("'", "''") + "', " + crncyid + ", " + Global.myVst.user_id +
                     ", '" + dateStr + "', " + crdtamnt + ", " +
                     Global.myVst.user_id + ", '" + dateStr + "', " + netamnt +
                     ", -1, '" + srcDocTyp.Replace("'", "''") + "', " + srcDocID + ", " + srcDocLnID + ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void createPymntGLIntFcLn(int accntid, string trnsdesc, double dbtamnt,
      string trnsdte, int crncyid, double crdtamnt, double netamnt, string srcDocTyp,
      long srcDocID, long srcDocLnID, string dateStr, string trnsSrc)
        {
            if (accntid <= 0)
            {
                return;
            }
            trnsdte = DateTime.ParseExact(
         trnsdte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            dateStr = DateTime.ParseExact(
         dateStr, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string insSQL = "INSERT INTO scm.scm_gl_interface(" +
                  "accnt_id, transaction_desc, dbt_amount, trnsctn_date, " +
                  "func_cur_id, created_by, creation_date, crdt_amount, last_update_by, " +
                  "last_update_date, net_amount, gl_batch_id, src_doc_typ, src_doc_id, " +
                  "src_doc_line_id, trns_source) " +
                     "VALUES (" + accntid + ", '" + trnsdesc.Replace("'", "''") + "', " + dbtamnt +
                     ", '" + trnsdte.Replace("'", "''") + "', " + crncyid + ", " + Global.myVst.user_id +
                     ", '" + dateStr + "', " + crdtamnt + ", " +
                     Global.myVst.user_id + ", '" + dateStr + "', " + netamnt +
                     ", -1, '" + srcDocTyp.Replace("'", "''") + "', " +
                     srcDocID + ", " + srcDocLnID + ", '" + trnsSrc + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }
        #endregion

        #region "ITEMS..."
        public static long getPrsnItmVlID(long prsnID, long itmID, string trnsdte)
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

        public static double getStockLstAvlblBls(long stockID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);
            string strSql = "";
            strSql = "SELECT COALESCE(a.available_balance,0) " +
          "FROM inv.inv_stock_daily_bals a " +
          "WHERE(to_timestamp(a.bals_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.stock_id = " + stockID +
          ") ORDER BY to_timestamp(a.bals_date, 'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";

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

        public static double getStoreLstTotBls(long itmID, long storID, string balsDate)
        {
            //    balsDate = DateTime.ParseExact(
            //balsDate, "dd-MMM-yyyy HH:mm:ss",
            //System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);
            string strSql = "";
            strSql = @"SELECT scm.get_ltst_stock_bals(a.stock_id, '" + balsDate + @"')
 FROM inv.inv_stock a 
 WHERE(a.itm_id = " + itmID + " and a.subinv_id = " + storID + @")";

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

        public static double getStoreLstTotBls(long itmID, long storID)
        {
            //    balsDate = DateTime.ParseExact(
            //balsDate, "dd-MMM-yyyy HH:mm:ss",
            //System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";//
            strSql = @"SELECT scm.get_ltst_stock_bals(a.stock_id)
 FROM inv.inv_stock a 
 WHERE(a.itm_id = " + itmID + " and a.subinv_id = " + storID + @")";

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

        public static double getStockLstTotBls(long stockID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);
            string strSql = "";
            strSql = "SELECT COALESCE(a.stock_tot_qty,0) " +
          "FROM inv.inv_stock_daily_bals a " +
          "WHERE(to_timestamp(a.bals_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.stock_id = " + stockID +
          ") ORDER BY to_timestamp(a.bals_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";

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

        public static double getStockLstRsvdBls(long stockID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);
            string strSql = "";
            strSql = "SELECT COALESCE(a.reservations,0) " +
          "FROM inv.inv_stock_daily_bals a " +
          "WHERE(to_timestamp(a.bals_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.stock_id = " + stockID +
          ") ORDER BY to_timestamp(a.bals_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";

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

        public static double getCsgmtLstAvlblBls(long csgmtID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);
            string strSql = "";
            strSql = "SELECT COALESCE(a.available_balance,0) " +
          "FROM inv.inv_consgmt_daily_bals a " +
          "WHERE(to_timestamp(a.bals_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.consgmt_id = " + csgmtID +
          ") ORDER BY to_timestamp(a.bals_date, 'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";

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

        public static double getCsgmtLstTotBls(long csgmtID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);
            string strSql = "";
            strSql = "SELECT COALESCE(a.consgmt_tot_qty,0) " +
          "FROM inv.inv_consgmt_daily_bals a " +
          "WHERE(to_timestamp(a.bals_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.consgmt_id = " + csgmtID +
          ") ORDER BY to_timestamp(a.bals_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";

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

        public static double getCsgmtLstRsvdBls(long csgmtID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);
            string strSql = "";
            strSql = "SELECT COALESCE(a.reservations,0) " +
          "FROM inv.inv_consgmt_daily_bals a " +
          "WHERE(to_timestamp(a.bals_date,'YYYY-MM-DD') <=  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.consgmt_id = " + csgmtID +
          ") ORDER BY to_timestamp(a.bals_date,'YYYY-MM-DD') DESC LIMIT 1 OFFSET 0";

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

        public static long getCsgmtDailyBalsID(long csgmtID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);
            string strSql = "";
            strSql = "SELECT a.bal_id " +
          "FROM inv.inv_consgmt_daily_bals a " +
          "WHERE(to_timestamp(a.bals_date,'YYYY-MM-DD') =  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.consgmt_id = " + csgmtID + ")";
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

        public static long getStockDailyBalsID(long stockID, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            balsDate = balsDate.Substring(0, 10);
            string strSql = "";
            strSql = "SELECT a.bal_id " +
          "FROM inv.inv_stock_daily_bals a " +
          "WHERE(to_timestamp(a.bals_date,'YYYY-MM-DD') =  to_timestamp('" + balsDate +
          "','YYYY-MM-DD') and a.stock_id = " + stockID + ")";
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

        public static long getItemStockID(long itmID, long storeID)
        {
            string strSql = "";
            strSql = "SELECT a.stock_id " +
          "FROM inv.inv_stock a " +
          "WHERE(a.itm_id = " + itmID + " and a.subinv_id = " + storeID + ")";
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

        public static string getStoreNm(long storeID)
        {
            string strSql = "";
            strSql = "SELECT a.subinv_name " +
          "FROM inv.inv_itm_subinventories a " +
          "WHERE(a.subinv_id = " + storeID + ")";
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

        public static void postCnsgnmntQty(long csgmtID,
        double totQty, double rsvdQty, double avblQty,
        string trnsDate, string src_trsID)
        {
            long dailybalID = Global.getCsgmtDailyBalsID(csgmtID, trnsDate);
            //Get dailybalid for accnt on this date
            //if doesn't exist get last accnt bals be4 this date
            //add new amount to it and insert record
            if (dailybalID <= 0)
            {
                double lstTotBals = Global.getCsgmtLstTotBls(csgmtID, trnsDate);
                double lstRsvdBals = Global.getCsgmtLstRsvdBls(csgmtID, trnsDate);
                double lstAvblBals = Global.getCsgmtLstAvlblBls(csgmtID, trnsDate);
                Global.createCnsgmtDailyBals(csgmtID, lstTotBals, lstRsvdBals, lstAvblBals, trnsDate);
                Global.updtCnsgmtDailyBals(csgmtID, totQty,
                  rsvdQty, avblQty, trnsDate, "Do", src_trsID);
            }
            else
            {
                Global.updtCnsgmtDailyBals(csgmtID, totQty,
                  rsvdQty, avblQty, trnsDate, "Do", src_trsID);
            }

            //Global.updateItemBalances(getItmCodeFrmCnsgmntID(csgmtID), totQty);
        }

        public static void postStockQty(long stockID,
      double totQty, double rsvdQty, double avblQty,
      string trnsDate, string src_trsID)
        {
            long dailybalID = Global.getStockDailyBalsID(stockID, trnsDate);
            //Get dailybalid for accnt on this date
            //if doesn't exist get last accnt bals be4 this date
            //add new amount to it and insert record
            if (dailybalID <= 0)
            {
                double lstTotBals = Global.getStockLstTotBls(stockID, trnsDate);
                double lstRsvdBals = Global.getStockLstRsvdBls(stockID, trnsDate);
                double lstAvblBals = Global.getStockLstAvlblBls(stockID, trnsDate);
                Global.createStckDailyBals(stockID, lstTotBals, lstRsvdBals, lstAvblBals, trnsDate);
                Global.updtStckDailyBals(stockID, totQty,
                  rsvdQty, avblQty, trnsDate, "Do", src_trsID);
            }
            else
            {
                Global.updtStckDailyBals(stockID, totQty,
                  rsvdQty, avblQty, trnsDate, "Do", src_trsID);
            }

            //Global.updateItemBalances(getItmCodeFrmStckID(stockID), totQty);
        }

        public static void createSmmryItm(string smmryTyp,
           string smmryNm, double amnt, long codeBehind, string srcDocTyp,
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
                  "', " + srcDocHdrID + ", " + Global.myVst.user_id + ", '" + dateStr +
                  "', " + Global.myVst.user_id + ", '" + dateStr + "', '" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(autoCalc) + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void createSalesDocHdr(int orgid, string docNum,
      string desc, string docTyp, string docdte, string pymntTrms,
      int cstmrID, int siteID, string apprvlSts,
      string nxtApprvl, long srcDocID, int rcvblAcntID,
      int pymntID, int invcCurrID, double exchRate,
      long chckInID, string chckInType, bool enblAutoChrg,
      long event_rgstr_id, string evntCtgry)
        {
            docdte = DateTime.ParseExact(
         docdte, "dd-MMM-yyyy",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO scm.scm_sales_invc_hdr(" +
                  "invc_date, payment_terms, customer_id, " +
                  "customer_site_id, comments_desc, approval_status, created_by, " +
                  "creation_date, last_update_by, last_update_date, next_aproval_action, " +
                  "invc_number, invc_type, src_doc_hdr_id, org_id, receivables_accnt_id, " +
                  "pymny_method_id, invc_curr_id, exchng_rate, " +
                  "other_mdls_doc_id, other_mdls_doc_type, enbl_auto_misc_chrges, " +
                  "event_rgstr_id, evnt_cost_category) " +
                  "VALUES ('" + docdte.Replace("'", "''") +
                  "', '" + pymntTrms.Replace("'", "''") +
                  "', " + cstmrID + ", " + siteID + ", '" + desc.Replace("'", "''") +
                  "', '" + apprvlSts.Replace("'", "''") + "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', '" + nxtApprvl.Replace("'", "''") +
                  "', '" + docNum.Replace("'", "''") + "', '" +
                  docTyp.Replace("'", "''") + "', " + srcDocID + ", " +
                  orgid + ", " + rcvblAcntID + ", " + pymntID + ", "
                  + invcCurrID + ", " + exchRate + "," + chckInID + ",'" + chckInType +
                  "','" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(enblAutoChrg) +
                  "'," + event_rgstr_id + ", '" + evntCtgry.Replace("'", "''") + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        // public static void createSalesDocHdr(int orgid, string docNum,
        //   string desc, string docTyp, string docdte, string pymntTrms,
        //   int cstmrID, int siteID, string apprvlSts,
        //   string nxtApprvl, long srcDocID, int rcvblAcntID,
        //   int pymntID, int invcCurrID, double exchRate,
        //   long chckInID, string chckInType, bool enblAutoChrg)
        // {
        //   docdte = DateTime.ParseExact(
        //docdte, "dd-MMM-yyyy",
        //System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
        //   string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
        //   string insSQL = "INSERT INTO scm.scm_sales_invc_hdr(" +
        //         "invc_date, payment_terms, customer_id, " +
        //         "customer_site_id, comments_desc, approval_status, created_by, " +
        //         "creation_date, last_update_by, last_update_date, next_aproval_action, " +
        //         "invc_number, invc_type, src_doc_hdr_id, org_id, receivables_accnt_id, " +
        //         "pymny_method_id, invc_curr_id, exchng_rate, " +
        //         "other_mdls_doc_id, other_mdls_doc_type, enbl_auto_misc_chrges) " +
        //         "VALUES ('" + docdte.Replace("'", "''") +
        //         "', '" + pymntTrms.Replace("'", "''") +
        //         "', " + cstmrID + ", " + siteID + ", '" + desc.Replace("'", "''") +
        //         "', '" + apprvlSts.Replace("'", "''") + "', " + Global.myVst.user_id + ", '" + dateStr +
        //         "', " + Global.myVst.user_id + ", '" + dateStr +
        //         "', '" + nxtApprvl.Replace("'", "''") +
        //         "', '" + docNum.Replace("'", "''") + "', '" +
        //         docTyp.Replace("'", "''") + "', " + srcDocID + ", " +
        //         orgid + ", " + rcvblAcntID + ", " + pymntID + ", "
        //         + invcCurrID + ", " + exchRate + "," + chckInID + ",'" + chckInType +
        //         "','" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(enblAutoChrg) + "')";
        //   Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        // }

        public static void createSalesDocLn(long lineid, long docID, int itmID,
          double qty, double untPrice, int storeID,
          int crncyID, long srclnID, int txCode, int dscntCde,
          int chrgeCde, string rtrnRsn, string cnsgmntIDs, double orgnlPrice,
          bool isDlvrd, long otherMdlID, string otherMdlType, string extrDesc,
          double rntdQty, string altrntName)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO scm.scm_sales_invc_det(invc_det_ln_id, " +
                  "invc_hdr_id, itm_id, doc_qty, unit_selling_price, " +
                  "created_by, creation_date, last_update_by, last_update_date, " +
                  "store_id, crncy_id, src_line_id, tax_code_id, " +
                  "dscnt_code_id, chrg_code_id, qty_trnsctd_in_dest_doc, " +
                  "rtrn_reason, consgmnt_ids, orgnl_selling_price, is_itm_delivered, " +
                  "other_mdls_doc_id, other_mdls_doc_type, extra_desc, rented_itm_qty, alternate_item_name) " +
                  "VALUES (" + lineid +
                  "," + docID +
                  ", " + itmID +
                  ", " + qty + ", " + untPrice + ", " + Global.myVst.user_id + ", '" + dateStr +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', " + storeID +
                  ", " + crncyID + ", " + srclnID + ", " + txCode +
                  ", " + dscntCde + ", " + chrgeCde + ", 0, '" +
                  rtrnRsn.Replace("'", "''") + "', '" + cnsgmntIDs.Replace("'", "''") +
                  "', " + orgnlPrice + ", " + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isDlvrd) +
                  ", " + otherMdlID + ", '" + otherMdlType.Replace("'", "''") +
                  "', '" + extrDesc.Replace("'", "''") +
                  "'," + rntdQty + ",'" + altrntName.Replace("'", "''") + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }


        public static void createStckDailyBals(long skckId, double totQty,
         double rsvdQty, double avlblQty, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            if (balsDate.Length > 10)
            {
                balsDate = balsDate.Substring(0, 10);
            }
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO inv.inv_stock_daily_bals(" +
                              "stock_id, stock_tot_qty, reservations, available_balance, bals_date, " +
                              "created_by, creation_date, last_update_by, last_update_date, source_trns_ids) " +
              "VALUES (" + skckId +
              ", " + totQty + ", " + rsvdQty + ", " + avlblQty + ", '" + balsDate + "', " + Global.myVst.user_id + ", '" + dateStr +
                              "', " + Global.myVst.user_id + ", '" + dateStr + "', ',')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void createCnsgmtDailyBals(long CnsgmId, double totQty,
         double rsvdQty, double avlblQty, string balsDate)
        {
            balsDate = DateTime.ParseExact(
         balsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            if (balsDate.Length > 10)
            {
                balsDate = balsDate.Substring(0, 10);
            }
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO inv.inv_consgmt_daily_bals(" +
                              "consgmt_id, consgmt_tot_qty, reservations, available_balance, bals_date, " +
                              "created_by, creation_date, last_update_by, last_update_date, source_trns_ids) " +
              "VALUES (" + CnsgmId +
              ", " + totQty + ", " + rsvdQty + ", " + avlblQty + ", '" + balsDate +
              "', " + Global.myVst.user_id + ", '" + dateStr +
                              "', " + Global.myVst.user_id + ", '" + dateStr + "', ',')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }


        public static void createPaymntLine(string pymtTyp, double amnt, double curBals,
          string payRmrk, string srcDocTyp, long srcDocID, string dateStr, string dateRcvd)
        {
            dateRcvd = DateTime.ParseExact(
         dateRcvd, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            dateStr = DateTime.ParseExact(
         dateStr, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string insSQL = "INSERT INTO scm.scm_payments(" +
                  "pymnt_type, amount_paid, custmrs_balance, pymnt_remark, " +
                  "src_doc_typ, src_doc_id, created_by, creation_date, last_update_by, " +
                  "last_update_date, date_rcvd) " +
             "VALUES ('" + pymtTyp.Replace("'", "''") + "', " + amnt + ", " + curBals +
             ", '" + payRmrk.Replace("'", "''") + "', '" + srcDocTyp.Replace("'", "''") +
             "', " + srcDocID + ", " + Global.myVst.user_id + ", '" + dateStr + "', " +
                     Global.myVst.user_id + ", '" + dateStr + "', '" + dateRcvd + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void createTodaysGLBatch(int orgid, string batchnm,
        string batchdesc, string batchsource)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO accb.accb_trnsctn_batches(" +
                     "batch_name, batch_description, created_by, creation_date, " +
                     "org_id, batch_status, last_update_by, last_update_date, batch_source, avlbl_for_postng) " +
             "VALUES ('" + batchnm.Replace("'", "''") + "', '" + batchdesc.Replace("'", "''") +
             "', " + Global.myVst.user_id + ", '" + dateStr + "', " + orgid + ", '0', " +
                     Global.myVst.user_id + ", '" + dateStr + "', '" +
                     batchsource.Replace("'", "''") + "', '0')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtTodaysGLBatchPstngAvlblty(long batchid, string avlblty)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string insSQL = "UPDATE accb.accb_trnsctn_batches SET avlbl_for_postng='" + avlblty +
              "', last_update_by=" + Global.myVst.user_id +
              ", last_update_date='" + dateStr +
              "' WHERE batch_id = " + batchid;
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
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
            return sumRes;
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
            return sumRes;
        }

        public static void deleteBatch(long batchid, string batchNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Batch Name = " + batchNm;
            string delSql = "DELETE FROM accb.accb_trnsctn_batches WHERE(batch_id = " + batchid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }

        public static void deleteBatchTrns(long batchid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string delSql = "DELETE FROM accb.accb_trnsctn_details WHERE(batch_id = " + batchid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }

        public static void createTransaction(int accntid, string trnsDesc,
      double dbtAmnt, string trnsDate, int crncyid,
          long batchid, double crdtamnt, double netAmnt,
          double entrdAmt, int entrdCurrID, double acntAmnt, int acntCurrID,
          double funcExchRate, double acntExchRate, string dbtOrCrdt)
        {
            trnsDate = DateTime.ParseExact(
         trnsDate, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            if (trnsDesc.Length > 500)
            {
                trnsDesc = trnsDesc.Substring(0, 500);
            }
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO accb.accb_trnsctn_details(" +
                              "accnt_id, transaction_desc, dbt_amount, trnsctn_date, " +
                              "func_cur_id, created_by, creation_date, batch_id, crdt_amount, " +
                              @"last_update_by, last_update_date, net_amount, 
            entered_amnt, entered_amt_crncy_id, accnt_crncy_amnt, accnt_crncy_id, 
            func_cur_exchng_rate, accnt_cur_exchng_rate, dbt_or_crdt) " +
                              "VALUES (" + accntid + ", '" + trnsDesc.Replace("'", "''") + "', " + dbtAmnt +
                              ", '" + trnsDate + "', " + crncyid + ", " + Global.myVst.user_id + ", '" + dateStr +
                              "', " + batchid + ", " + crdtamnt + ", " + Global.myVst.user_id +
                              ", '" + dateStr + "'," + netAmnt + ", " + entrdAmt +
                              ", " + entrdCurrID + ", " + acntAmnt +
                              ", " + acntCurrID + ", " + funcExchRate +
                              ", " + acntExchRate + ", '" + dbtOrCrdt + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtBatchTrnsSrcIDs(long batchID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string updtSQL = "UPDATE accb.accb_trnsctn_details SET source_trns_ids='' WHERE batch_id=" + batchID;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtIntrfcTrnsSrcBatchIDs(long batchID)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string updtSQL = "UPDATE scm.scm_gl_interface SET gl_batch_id=-1 WHERE gl_batch_id=" + batchID;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
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

        public static void createBatch(int orgid, string batchname,
         string batchdesc, string btchsrc, string batchvldty, long srcbatchid, string avlblforPpstng)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO accb.accb_trnsctn_batches(" +
                              "batch_name, batch_description, created_by, creation_date, " +
                              "org_id, batch_status, last_update_by, last_update_date, " +
            "batch_source, batch_vldty_status, src_batch_id, avlbl_for_postng) " +
                              "VALUES ('" + batchname.Replace("'", "''") + "', '" + batchdesc.Replace("'", "''") +
                              "', " + Global.myVst.user_id + ", '" + dateStr +
                              "', " + orgid + ", '0', " + Global.myVst.user_id + ", '" + dateStr +
                              "', '" + btchsrc.Replace("'", "''") +
                              "', '" + batchvldty.Replace("'", "''") +
                              "', " + srcbatchid +
                              ",'" + avlblforPpstng + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
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

        public static void updtCnsgmtDailyBals(long CnsgmId, double totQty,
         double rsvdQty, double avlblQty, string balsDate,
          string act_typ, string src_trnsID)
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
                updtSQL = "UPDATE inv.inv_consgmt_daily_bals " +
            "SET last_update_by = " + Global.myVst.user_id +
            ", last_update_date = '" + dateStr +
                  "', consgmt_tot_qty = COALESCE(consgmt_tot_qty,0) - " + totQty +
                  ", reservations = COALESCE(reservations,0) - " + rsvdQty +
                  ", available_balance = COALESCE(available_balance,0) - " + avlblQty +
                  ", source_trns_ids = COALESCE(replace(source_trns_ids, '," + src_trnsID + ",', ','),',')" +
            " WHERE (to_timestamp(bals_date,'YYYY-MM-DD') >=  to_timestamp('" + balsDate +
            "','YYYY-MM-DD') and consgmt_id = " + CnsgmId + ")";
            }
            else
            {
                updtSQL = "UPDATE inv.inv_consgmt_daily_bals " +
            "SET last_update_by = " + Global.myVst.user_id +
            ", last_update_date = '" + dateStr +
                  "', consgmt_tot_qty = COALESCE(consgmt_tot_qty,0) + " + totQty +
                  ", reservations = COALESCE(reservations,0) + " + rsvdQty +
                  ", available_balance = COALESCE(available_balance,0) + " + avlblQty +
                  ", source_trns_ids = COALESCE(source_trns_ids,',') || '" + src_trnsID + ",'" +
            " WHERE (to_timestamp(bals_date,'YYYY-MM-DD') >=  to_timestamp('" + balsDate +
            "','YYYY-MM-DD') and consgmt_id = " + CnsgmId + ")";
            }
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtStckDailyBals(long skckId, double totQty,
         double rsvdQty, double avlblQty, string balsDate,
          string act_typ, string src_trnsID)
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
                updtSQL = "UPDATE inv.inv_stock_daily_bals " +
            "SET last_update_by = " + Global.myVst.user_id +
            ", last_update_date = '" + dateStr +
                  "', stock_tot_qty = COALESCE(stock_tot_qty,0) - " + totQty +
                  ", reservations = COALESCE(reservations,0) - " + rsvdQty +
                  ", available_balance = COALESCE(available_balance,0) - " + avlblQty +
                  ", source_trns_ids = COALESCE(replace(source_trns_ids, '," + src_trnsID + ",', ','),',')" +
            " WHERE (to_timestamp(bals_date,'YYYY-MM-DD') >=  to_timestamp('" + balsDate +
            "','YYYY-MM-DD') and stock_id = " + skckId + ")";
            }
            else
            {
                updtSQL = "UPDATE inv.inv_stock_daily_bals " +
            "SET last_update_by = " + Global.myVst.user_id +
            ", last_update_date = '" + dateStr +
                  "', stock_tot_qty = COALESCE(stock_tot_qty,0) + " + totQty +
                  ", reservations = COALESCE(reservations,0) + " + rsvdQty +
                  ", available_balance = COALESCE(available_balance,0) + " + avlblQty +
                  ", source_trns_ids = COALESCE(source_trns_ids,',') || '" + src_trnsID + ",'" +
            " WHERE (to_timestamp(bals_date,'YYYY-MM-DD') >=  to_timestamp('" + balsDate +
            "','YYYY-MM-DD') and stock_id = " + skckId + ")";
            }
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtSrcDocTrnsctdQty(long src_lnid,
         double qty)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE scm.scm_sales_invc_det SET " +
                  "qty_trnsctd_in_dest_doc=qty_trnsctd_in_dest_doc+" + qty +
                  ", last_update_by=" + Global.myVst.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (invc_det_ln_id = " +
                  src_lnid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtSalesDocApprvl(long docid,
          string apprvlSts, string nxtApprvl)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE scm.scm_sales_invc_hdr SET " +
                  "approval_status='" + apprvlSts + "', last_update_by=" + Global.myVst.user_id +
                  ", last_update_date='" + dateStr +
                  "', next_aproval_action='" + nxtApprvl.Replace("'", "''") +
                  "' WHERE (invc_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtAppntmntStatus(long docid,
          string apprvlSts)
        {
            if (docid <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please Select a Document First!", 0);
                return;
            }
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE hosp.appntmnt SET " +
                  "appntmnt_status='" + apprvlSts + "', last_update_by=" + Global.myVst.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (appntmnt_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtVisitStatus(long docid,
           string apprvlSts)
        {
            if (docid <= 0)
            {
                Global.mnFrm.cmCde.showMsg("Please Select a Document First!", 0);
                return;
            }
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE hosp.visit SET " +
                  "vst_status='" + apprvlSts + "', last_update_by=" + Global.myVst.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (vst_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updateSalesDocLn(long lnID, long nwSalesDocID)
        {
            //long othrMdlID, string othMdlType,
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE scm.scm_sales_invc_det SET " +
                  "last_update_by = " + Global.myVst.user_id +
                  ", last_update_date= '" + dateStr +
                  "', invc_hdr_id = " + nwSalesDocID +
                  " WHERE (invc_det_ln_id = " + lnID + ")";
            /*,             "', other_mdls_doc_id = " + otherMdlID +
      other_mdls_doc_type = '" + otherMdlType.Replace("'", "''") +
                  "' */
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updateSalesDocLn(long lnID, int itmID,
          double qty, double untPrice, int storeID,
          int crncyID, long srclnID, int txCode, int dscntCde,
          int chrgeCde, string rtrnRsn, string cnsgmntIDs,
          double orgnlPrice, long otherMdlID,
          string otherMdlType, string extrDesc,
          double rntdQty, string altrntName)
        {
            /* long otherMdlID,
            string otherMdlType, */
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE scm.scm_sales_invc_det SET " +
                  "itm_id=" + itmID +
                  ", doc_qty =" + qty +
                  ", unit_selling_price= " + untPrice +
                  ", orgnl_selling_price= " + orgnlPrice + ", " +
                  "last_update_by = " + Global.myVst.user_id +
                  ", last_update_date= '" + dateStr + "', " +
                  "store_id=" + storeID +
                  ", crncy_id =" + crncyID + ", src_line_id = " + srclnID +
                  ", tax_code_id = " + txCode +
                  ", dscnt_code_id = " + dscntCde +
                  ", chrg_code_id = " + chrgeCde +
                  ", rtrn_reason = '" + rtrnRsn.Replace("'", "''") +
                  "', consgmnt_ids ='" + cnsgmntIDs.Replace("'", "''") +
              "', other_mdls_doc_id = " + otherMdlID +
              ", other_mdls_doc_type = '" + otherMdlType.Replace("'", "''") +
                  "', extra_desc = '" + extrDesc.Replace("'", "''") +
                  "', rented_itm_qty = " + rntdQty +
                  ", alternate_item_name = '" + altrntName.Replace("'", "''") +
                  "' WHERE (invc_det_ln_id = " + lnID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updateSalesLnCsgmtDist(long lnID, string cnsgmntQtys)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            //string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE scm.scm_sales_invc_det SET " +
                  "cnsgmnt_qty_dist ='" + cnsgmntQtys.Replace("'", "''") +
                  "', is_itm_delivered='1' WHERE (invc_det_ln_id = " + lnID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updateSalesLnDlvrd(long lnID, bool dlvrd)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            //string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE scm.scm_sales_invc_det SET " +
                  "is_itm_delivered='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(dlvrd) +
                  "' WHERE (invc_det_ln_id = " + lnID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static long getSalesLnsDlvrd(long docID)
        {
            string updtSQL = "SELECT count(1) from scm.scm_sales_invc_det " +
                  " WHERE (is_itm_delivered = '1' and invc_hdr_id = " + docID + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(updtSQL);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }

        public static void updateSalesLnCsgmtIDs(long lnID, string cnsgmntIDs)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE scm.scm_sales_invc_det SET " +
                  "consgmnt_ids ='" + cnsgmntIDs.Replace("'", "''") +
                  "' WHERE (invc_det_ln_id = " + lnID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtSalesDocHdr(long docid, string docNum,
         string desc, string docTyp, string docdte, string pymntTerms,
         int spplrID, int spplrSiteID, string apprvlSts,
         string nxtApprvl, long srcDocID,
         int pymntID, int invcCurrID, double exchRate, long chckInID,
         string chckInType, bool enblAutoChrg,
         long event_rgstr_id, string evntCtgry)
        {
            docdte = DateTime.ParseExact(docdte, "dd-MMM-yyyy",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE scm.scm_sales_invc_hdr SET " +
                  "invc_date='" + docdte.Replace("'", "''") +
                  "', payment_terms='" + pymntTerms.Replace("'", "''") +
                  "', customer_id=" + spplrID + ", " +
                  "customer_site_id=" + spplrSiteID + ", comments_desc='" + desc.Replace("'", "''") +
                  "', approval_status='" + apprvlSts.Replace("'", "''") + "', last_update_by=" + Global.myVst.user_id +
                  ", last_update_date='" + dateStr +
                  "', next_aproval_action='" + nxtApprvl.Replace("'", "''") +
                  "', invc_number='" + docNum.Replace("'", "''") + "', invc_type='" +
                  docTyp.Replace("'", "''") + "', src_doc_hdr_id=" + srcDocID +
                  ", pymny_method_id=" + pymntID + ", invc_curr_id=" + invcCurrID +
                  ", exchng_rate=" + exchRate +
                  ", other_mdls_doc_id=" + chckInID +
                  ", other_mdls_doc_type='" + chckInType.Replace("'", "''") + "' " +
                  ", enbl_auto_misc_chrges='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(enblAutoChrg) + "' " +
                  ", event_rgstr_id=" + event_rgstr_id +
                  ", evnt_cost_category='" + evntCtgry.Replace("'", "''") + "' " +
                  "WHERE (invc_hdr_id = " + docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        // public static void updtSalesDocHdr(long docid, string docNum,
        //   string desc, string docTyp, string docdte, string pymntTerms,
        //   int spplrID, int spplrSiteID, string apprvlSts,
        //   string nxtApprvl, long srcDocID,
        //   int pymntID, int invcCurrID, double exchRate, long chckInID,
        //   string chckInType, bool enblAutoChrg)
        // {
        //   docdte = DateTime.ParseExact(
        //docdte, "dd-MMM-yyyy",
        //System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
        //   Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
        //   string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
        //   string updtSQL = "UPDATE scm.scm_sales_invc_hdr SET " +
        //         "invc_date='" + docdte.Replace("'", "''") +
        //         "', payment_terms='" + pymntTerms.Replace("'", "''") +
        //         "', customer_id=" + spplrID + ", " +
        //         "customer_site_id=" + spplrSiteID + ", comments_desc='" + desc.Replace("'", "''") +
        //         "', approval_status='" + apprvlSts.Replace("'", "''") + "', last_update_by=" + Global.myVst.user_id +
        //         ", last_update_date='" + dateStr +
        //         "', next_aproval_action='" + nxtApprvl.Replace("'", "''") +
        //         "', invc_number='" + docNum.Replace("'", "''") + "', invc_type='" +
        //         docTyp.Replace("'", "''") + "', src_doc_hdr_id=" + srcDocID +
        //         ", pymny_method_id=" + pymntID + ", invc_curr_id=" + invcCurrID +
        //         ", exchng_rate=" + exchRate +
        //         ", other_mdls_doc_id=" + chckInID +
        //         ", other_mdls_doc_type='" + chckInType.Replace("'", "''") + "' " +
        //         ", enbl_auto_misc_chrges='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(enblAutoChrg) + "' " +
        //         "WHERE (invc_hdr_id = " +
        //         docid + ")";
        //   Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        // }

        public static bool hsTrnsUptdStockBls(string srctrnsid,
      string trnsdate, long stockID)
        {
            trnsdate = DateTime.ParseExact(
              trnsdate, "dd-MMM-yyyy HH:mm:ss",
              System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            if (trnsdate.Length > 10)
            {
                trnsdate = trnsdate.Substring(0, 10);
            }

            string strSql = "SELECT a.bal_id FROM inv.inv_stock_daily_bals a " +
              "WHERE a.stock_id = " + stockID +
              " and a.bals_date = '" + trnsdate + "' and a.source_trns_ids like '%," + srctrnsid + ",%'";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static string getStockBlsTrnsDte(string srctrnsid,
      string trnsdate, long stockID)
        {
            //trnsdate = DateTime.ParseExact(
            //  trnsdate, "dd-MMM-yyyy HH:mm:ss",
            //  System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            //if (trnsdate.Length > 10)
            //{
            //  trnsdate = trnsdate.Substring(0, 10);
            //}

            string strSql = "SELECT to_char(to_timestamp(a.bals_date,'YYYY-MM-DD'),'DD-Mon-YYYY 00:00:00') FROM inv.inv_stock_daily_bals a " +
              "WHERE a.stock_id = " + stockID +
              " and a.source_trns_ids like '%," + srctrnsid + ",%' ORDER BY a.bals_date DESC";
            // and a.bals_date = '" + trnsdate + "' 
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            return "";
        }

        public static string getCsgmntBlsTrnsDte(string srctrnsid,
      string trnsdate, long csgnmtID)
        {
            //trnsdate = DateTime.ParseExact(
            //       trnsdate, "dd-MMM-yyyy HH:mm:ss",
            //       System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            //if (trnsdate.Length > 10)
            //{
            //  trnsdate = trnsdate.Substring(0, 10);
            //}

            string strSql = "SELECT to_char(to_timestamp(a.bals_date,'YYYY-MM-DD'),'DD-Mon-YYYY 00:00:00') FROM inv.inv_consgmt_daily_bals a " +
              "WHERE a.consgmt_id = " + csgnmtID +
              " and a.source_trns_ids like '%," + srctrnsid + ",%' ORDER BY a.bals_date DESC";
            //and a.bals_date = '" + trnsdate + "'
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            return "";
        }

        public static bool hsTrnsUptdCsgmntBls(string srctrnsid,
      string trnsdate, long csgnmtID)
        {
            trnsdate = DateTime.ParseExact(
                   trnsdate, "dd-MMM-yyyy HH:mm:ss",
                   System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            if (trnsdate.Length > 10)
            {
                trnsdate = trnsdate.Substring(0, 10);
            }

            string strSql = "SELECT a.bal_id FROM inv.inv_consgmt_daily_bals a " +
              "WHERE a.consgmt_id = " + csgnmtID +
              " and a.bals_date = '" + trnsdate + "' and a.source_trns_ids like '%," + srctrnsid + ",%'";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static void undoPostCnsgnmntQty(long csgmtID,
       double totQty, double rsvdQty, double avblQty,
       string trnsDate, string src_trsID)
        {
            long dailybalID = Global.getCsgmtDailyBalsID(csgmtID, trnsDate);
            //Get dailybalid for accnt on this date
            //if doesn't exist get last accnt bals be4 this date
            //add new amount to it and insert record
            //Global.mnFrm.cmCde.showMsg(dailybalID + "/" + csgmtID + "/" + trnsDate, 0);
            if (dailybalID <= 0)
            {
                //double lstTotBals = Global.getCsgmtLstTotBls(csgmtID, trnsDate);
                //double lstRsvdBals = Global.getCsgmtLstRsvdBls(csgmtID, trnsDate);
                //double lstAvblBals = Global.getCsgmtLstAvlblBls(csgmtID, trnsDate);
                //Global.createCnsgmtDailyBals(csgmtID, lstTotBals, lstRsvdBals, lstAvblBals, trnsDate);
                //Global.updtCnsgmtDailyBals(csgmtID, totQty,
                //  rsvdQty, avblQty, trnsDate, "Do", src_trsID);
            }
            else
            {
                Global.updtCnsgmtDailyBals(csgmtID, totQty,
                  rsvdQty, avblQty, trnsDate, "Undo", src_trsID);
            }

        }

        public static void undoPostStockQty(long stockID,
      double totQty, double rsvdQty, double avblQty,
      string trnsDate, string src_trsID)
        {
            long dailybalID = Global.getStockDailyBalsID(stockID, trnsDate);
            //Get dailybalid for accnt on this date
            //if doesn't exist get last accnt bals be4 this date
            //add new amount to it and insert record
            if (dailybalID <= 0)
            {
                //double lstTotBals = Global.getStockLstTotBls(stockID, trnsDate);
                //double lstRsvdBals = Global.getStockLstRsvdBls(stockID, trnsDate);
                //double lstAvblBals = Global.getStockLstAvlblBls(stockID, trnsDate);
                //Global.createStckDailyBals(stockID, lstTotBals, lstRsvdBals, lstAvblBals, trnsDate);
                //Global.updtStckDailyBals(stockID, totQty,
                //  rsvdQty, avblQty, trnsDate, "Do", src_trsID);
            }
            else
            {
                Global.updtStckDailyBals(stockID, totQty,
                  rsvdQty, avblQty, trnsDate, "Undo", src_trsID);
            }
        }

        public static double getUOMPriceLsTx(long itmID, double qty)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string strSql = "SELECT scm.get_item_unit_price_ls_tx(" + itmID + ", " + qty + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return Math.Round(double.Parse(dtst.Tables[0].Rows[0][0].ToString()), 4);
            }
            return 0;
        }

        public static double getUOMSllngPrice(long itmID, double qty)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string strSql = "SELECT scm.get_item_unit_sllng_price(" + itmID + ", " + qty + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return Math.Round(double.Parse(dtst.Tables[0].Rows[0][0].ToString()), 4);
            }
            return 0;
        }
        public static string getItmUOM(string parItmCode)
        {
            string qryItmUOM = "SELECT uom_name FROM inv.unit_of_measure WHERE uom_id = " +
                " (SELECT base_uom_id FROM inv.inv_itm_list WHERE item_code = '" + parItmCode.Replace("'", "''")
                + "' AND org_id = " + Global.mnFrm.cmCde.Org_id + ")";

            DataSet ds = new DataSet();
            ds.Reset();
            ds = Global.fillDataSetFxn(qryItmUOM);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        public static DataSet get_ItemExtInf(long itmID)
        {
            string strSql = "";

            strSql = @"SELECT a.image, a.extra_info, a.other_desc, generic_name, trade_name, drug_usual_dsge, drug_max_dsge, 
       contraindications, food_interactions " +
          "FROM inv.inv_itm_list a WHERE a.item_id = " + itmID;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_ItemInf(long itmID, long cstmrSiteID)
        {
            string strSql = "";

            strSql = @"SELECT a.item_code, a.item_desc, 
a.selling_price, a.tax_code_id, CASE WHEN scm.get_cstmr_splr_dscntid("
            + cstmrSiteID + ") != -1 THEN scm.get_cstmr_splr_dscntid("
            + cstmrSiteID + @") ELSE a.dscnt_code_id END, a.extr_chrg_id, 
       a.item_type, a.base_uom_id, a.orgnl_selling_price " +
          "FROM inv.inv_itm_list a WHERE a.item_id = " + itmID;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet fillDataSetFxn(string selSQL)
        {
            return Global.mnFrm.cmCde.selectDataNoParams(selSQL);
        }
        public static DataSet get_AllConsignments(string searchWord, string searchIn,
        Int64 offset, int limit_size, int orgID, long cstmrSiteID)
        {
            string strSql = "";
            string wherecls = "";
            string invCls = "";
            string extInvCls = "";
            string itmTyp = " AND (a.item_type != 'Expense Item') AND (a.item_type != 'Services')";

            if (searchIn == "Item Code/Name")
            {
                wherecls = "(a.item_code ilike '" + searchWord.Replace("'", "''") +
               "') AND ";
            }
            else if (searchIn == "Item Description")
            {
                wherecls = "(a.item_desc ilike '" + searchWord.Replace("'", "''") +
               "') AND ";
            }

            strSql = "SELECT distinct a.item_id, a.item_code, a.item_desc, " +
              "a.selling_price, a.category_id, b.stock_id, b.subinv_id, b.shelves, " +
              "a.tax_code_id, CASE WHEN scm.get_cstmr_splr_dscntid("
            + cstmrSiteID + ") != -1 THEN scm.get_cstmr_splr_dscntid("
            + cstmrSiteID + ") ELSE a.dscnt_code_id END , a.extr_chrg_id, c.consgmt_id, c.cost_price, c.expiry_date " +
            "FROM inv.inv_itm_list a, inv.inv_stock b, inv.inv_consgmt_rcpt_det c " +
            "WHERE (" + wherecls + "(a.item_id = b.itm_id and b.stock_id = c.stock_id " +
            "and a.item_id = c.itm_id and b.subinv_id = c.subinv_id and a.enabled_flag='1')" + invCls +
            " AND (a.org_id = " + orgID +
            ")" + extInvCls + itmTyp + ") ORDER BY c.consgmt_id ASC, a.item_code LIMIT " + limit_size +
            " OFFSET " + (Math.Abs(offset * limit_size)).ToString();


            Global.itms_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_StoreItems(string searchWord, string searchIn,
        Int64 offset, int limit_size, int orgID, int storeID, string docTyp,
          bool cnsgmtsOnly, long itmID, long cstmrSiteID)
        {
            string strSql = "";
            string wherecls = "";
            string invCls = "";
            string extInvCls = "";
            string itmTyp = "";
            if (docTyp == "Sales Invoice"
              || docTyp == "Pro-Forma Invoice"
              || docTyp == "Sales Order")
            {
                itmTyp = " AND ((a.item_type = 'Merchandise Inventory' AND b.subinv_id = " + storeID + ") OR a.item_type = 'Services')";
                invCls = "";
                extInvCls = " AND (now() between to_timestamp(b.start_date, " +
              "'YYYY-MM-DD HH24:MI:SS') AND to_timestamp(CASE WHEN b.end_date='' " +
              "THEN '4000-12-31 23:59:59' ELSE b.end_date END, " +
              "'YYYY-MM-DD HH24:MI:SS'))";
            }
            else if (docTyp == "Internal Item Request")
            {
                itmTyp = " AND (a.item_type != 'Expense Item') AND (a.item_type != 'Services')";
            }
            else if (docTyp == "Item Issue-Unbilled")
            {
                itmTyp = " AND (a.item_type != 'Expense Item') AND (a.item_type != 'Services')";
                invCls = " AND (b.subinv_id = " + storeID + ")";
                extInvCls = " AND (now() between to_timestamp(b.start_date, " +
                "'YYYY-MM-DD HH24:MI:SS') AND to_timestamp(CASE WHEN b.end_date='' THEN '4000-12-31 23:59:59' ELSE b.end_date END, " +
                "'YYYY-MM-DD HH24:MI:SS'))";
            }
            else if (docTyp == "")
            {
                invCls = " AND (b.subinv_id = " + storeID + ")";
            }
            if (searchIn == "Item Code/Name")
            {
                wherecls = "(a.item_code ilike '" + searchWord.Replace("'", "''") +
               "' or a.item_desc ilike '" + searchWord.Replace("'", "''") +
               "') AND ";
            }
            else if (searchIn == "Item Description")
            {
                wherecls = "(a.item_code ilike '" + searchWord.Replace("'", "''") +
               "' or a.item_desc ilike '" + searchWord.Replace("'", "''") +
               "') AND ";
            }
            if (cnsgmtsOnly == true)
            {
                strSql = "SELECT distinct a.item_id, a.item_code, a.item_desc, " +
                  "a.selling_price, a.category_id, b.stock_id, b.subinv_id, b.shelves, " +
                  "a.tax_code_id, CASE WHEN scm.get_cstmr_splr_dscntid("
              + cstmrSiteID + ") != -1 THEN scm.get_cstmr_splr_dscntid("
              + cstmrSiteID + ") ELSE a.dscnt_code_id END , a.extr_chrg_id, c.consgmt_id, c.cost_price, c.expiry_date " +
                "FROM inv.inv_itm_list a, inv.inv_stock b, inv.inv_consgmt_rcpt_det c " +
                "WHERE (" + wherecls + "(a.item_id = b.itm_id and b.stock_id = c.stock_id " +
                "and a.item_id = c.itm_id and b.subinv_id = c.subinv_id and a.enabled_flag='1' and a.item_id=" + itmID + ")" + invCls +
                " AND (a.org_id = " + orgID +
                ")" + extInvCls + itmTyp + ") ORDER BY c.consgmt_id ASC, a.item_code LIMIT " + limit_size +
                " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }
            else
            {
                strSql = "SELECT distinct a.item_id, a.item_code, a.item_desc, " +
              "a.selling_price, a.category_id, COALESCE(b.stock_id,-1), COALESCE(b.subinv_id,-1), b.shelves, " +
              "a.tax_code_id, CASE WHEN scm.get_cstmr_splr_dscntid("
              + cstmrSiteID + ") != -1 THEN scm.get_cstmr_splr_dscntid("
              + cstmrSiteID + ") ELSE a.dscnt_code_id END, a.extr_chrg_id " +
            "FROM inv.inv_itm_list a LEFT OUTER JOIN inv.inv_stock b ON a.item_id = b.itm_id " + extInvCls +
            " WHERE (" + wherecls + "(a.enabled_flag='1')" + invCls +
            " AND (a.org_id = " + orgID +
            ")" + itmTyp + ") ORDER BY a.item_code LIMIT " + limit_size +
            " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }
            Global.itms_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_Total_StoreItms(
          string searchWord, string searchIn,
          int orgID, int storeID, string docTyp,
          bool cnsgmtsOnly, long itmID)
        {
            string strSql = "";
            string wherecls = "";
            string invCls = "";
            string extInvCls = "";
            string itmTyp = "";
            if (docTyp == "Sales Invoice"
              || docTyp == "Pro-Forma Invoice"
              || docTyp == "Sales Order")
            {
                itmTyp = " AND ((a.item_type = 'Merchandise Inventory' AND b.subinv_id = " + storeID + ") OR a.item_type = 'Services')";
                invCls = "";
                extInvCls = " AND (now() between to_timestamp(b.start_date, " +
              "'YYYY-MM-DD HH24:MI:SS') AND to_timestamp(CASE WHEN b.end_date='' " +
              "THEN '4000-12-31 23:59:59' ELSE b.end_date END, " +
              "'YYYY-MM-DD HH24:MI:SS'))";
            }
            else if (docTyp == "Internal Item Request")
            {
                itmTyp = " AND (a.item_type != 'Expense Item') AND (a.item_type != 'Services')";
            }
            else if (docTyp == "Item Issue-Unbilled")
            {
                itmTyp = " AND (a.item_type != 'Expense Item') AND (a.item_type != 'Services')";
                invCls = " AND (b.subinv_id = " + storeID + ")";
                extInvCls = " AND (now() between to_timestamp(b.start_date, " +
                "'YYYY-MM-DD HH24:MI:SS') AND to_timestamp(CASE WHEN b.end_date='' THEN '4000-12-31 23:59:59' ELSE b.end_date END, " +
                "'YYYY-MM-DD HH24:MI:SS'))";
            }
            if (searchIn == "Item Code/Name")
            {
                wherecls = "(a.item_code ilike '" + searchWord.Replace("'", "''") +
               "') AND ";
            }
            else if (searchIn == "Item Description")
            {
                wherecls = "(a.item_desc ilike '" + searchWord.Replace("'", "''") +
               "') AND ";
            }
            if (cnsgmtsOnly == true)
            {
                strSql = "SELECT count(distinct c.consgmt_id) " +
                "FROM inv.inv_itm_list a, inv.inv_stock b, inv.inv_consgmt_rcpt_det c " +
                "WHERE (" + wherecls + "(a.item_id = b.itm_id and b.stock_id = c.stock_id " +
                "and a.item_id = c.itm_id and b.subinv_id = c.subinv_id and a.enabled_flag='1' and a.item_id=" + itmID + ")" + invCls +
                " AND (a.org_id = " + orgID +
                ")" + extInvCls + itmTyp + ")";
            }
            else
            {
                strSql = "SELECT count(1)" +
            "FROM inv.inv_itm_list a LEFT OUTER JOIN inv.inv_stock b ON a.item_id = b.itm_id " + extInvCls +
            " WHERE (" + wherecls + "(a.enabled_flag='1')" + invCls +
            " AND (a.org_id = " + orgID +
            ")" + itmTyp + ")";
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

        public static string getOldstItmCnsgmts(long itmID, double qnty)
        {
            string res = ",";
            string strSql = "SELECT distinct c.consgmt_id, inv.get_csgmt_lst_avlbl_bls(c.consgmt_id) " +
              "FROM inv.inv_consgmt_rcpt_det c " +
              "WHERE ((c.itm_id=" + itmID + ") and (c.subinv_id =" + Global.selectedStoreID +
              ") and  (inv.get_csgmt_lst_avlbl_bls(c.consgmt_id)>0)) ORDER BY c.consgmt_id ASC";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double curAvlbQty = 0;
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                if (curAvlbQty < qnty)
                {
                    res = res + dtst.Tables[0].Rows[i][0].ToString() + ",";
                    curAvlbQty = curAvlbQty + double.Parse(dtst.Tables[0].Rows[i][1].ToString());
                }
                else
                {
                    return res.Trim(',');
                }
            }
            return res.Trim(',');
        }

        public static string getOldstItmCnsgmtsForStock(long itmID, double qnty, int storeID)
        {
            string res = ",";
            string strSql = "SELECT distinct c.consgmt_id, inv.get_csgmt_lst_avlbl_bls(c.consgmt_id) " +
              "FROM inv.inv_consgmt_rcpt_det c " +
              "WHERE ((c.itm_id=" + itmID + ") and (c.subinv_id =" + storeID +
              ") and (inv.get_csgmt_lst_avlbl_bls(c.consgmt_id)>0)) ORDER BY c.consgmt_id ASC";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double curAvlbQty = 0;
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                if (curAvlbQty < qnty)
                {
                    res = res + dtst.Tables[0].Rows[i][0].ToString() + ",";
                    curAvlbQty = curAvlbQty + double.Parse(dtst.Tables[0].Rows[i][1].ToString());
                }
                else
                {
                    return res.Trim(',');
                }
            }
            return res.Trim(',');
        }

        public static List<string> getOldstItmCnsgmtsNCstPrcLstForStock(long itmID, double qnty, int storeID)
        {
            List<string> result = new List<string>();
            string resCnsgmntIDs = ",";
            string resCnsgmntIDCstPrce = ",";
            string strSql = "SELECT distinct c.consgmt_id, cost_price, inv.get_csgmt_lst_avlbl_bls(c.consgmt_id) " +
              "FROM inv.inv_consgmt_rcpt_det c " +
              "WHERE ((c.itm_id=" + itmID + ") and (c.subinv_id =" + storeID +
              ") and (inv.get_csgmt_lst_avlbl_bls(c.consgmt_id)>0)) ORDER BY c.consgmt_id ASC";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double curAvlbQty = 0;
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                if (curAvlbQty < qnty)
                {
                    resCnsgmntIDs = resCnsgmntIDs + dtst.Tables[0].Rows[i][0].ToString() + ",";
                    resCnsgmntIDCstPrce = resCnsgmntIDCstPrce + dtst.Tables[0].Rows[i][1].ToString() + ",";
                    curAvlbQty = curAvlbQty + double.Parse(dtst.Tables[0].Rows[i][2].ToString());
                }
                else
                {
                    result.Add(resCnsgmntIDs.Trim(','));
                    result.Add(resCnsgmntIDCstPrce.Trim(','));
                    return result;
                }
            }
            result.Add(resCnsgmntIDs.Trim(','));
            result.Add(resCnsgmntIDCstPrce.Trim(','));
            return result;
        }

        public static double getCnsgmtsQtySum(string cnsgmtIDs)
        {
            if (cnsgmtIDs == "")
            {
                cnsgmtIDs = "-123412";
            }
            string strSql = "SELECT distinct c.consgmt_id, inv.get_csgmt_lst_avlbl_bls(c.consgmt_id) " +
              "FROM inv.inv_consgmt_rcpt_det c " +
              "WHERE ((c.consgmt_id IN (" + cnsgmtIDs.Trim(',') + ")) and (inv.get_csgmt_lst_avlbl_bls(c.consgmt_id)>0)) ORDER BY c.consgmt_id ASC";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double ttlQty = 0;
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                ttlQty = ttlQty + double.Parse(dtst.Tables[0].Rows[i][1].ToString());
            }
            return ttlQty;
        }

        public static double getCnsgmtsRsvdSum(string cnsgmtIDs)
        {
            if (cnsgmtIDs == "")
            {
                cnsgmtIDs = "-123412";
            }
            string strSql = "SELECT distinct c.consgmt_id, inv.get_csgmt_lst_rsvd_bls(c.consgmt_id) " +
              "FROM inv.inv_consgmt_rcpt_det c " +
              "WHERE ((c.consgmt_id IN (" + cnsgmtIDs.Trim(',') + ")) and (inv.get_csgmt_lst_rsvd_bls(c.consgmt_id)>0)) ORDER BY c.consgmt_id ASC";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double ttlQty = 0;
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                ttlQty = ttlQty + double.Parse(dtst.Tables[0].Rows[i][1].ToString());
            }
            return ttlQty;
        }

        public static double getHgstUnitCostPrice(int itmID)
        {
            string strSql = "SELECT c.cost_price " +
         "FROM inv.inv_consgmt_rcpt_det c " +
         "WHERE (c.itm_id =" + itmID + ") ORDER BY c.consgmt_id DESC LIMIT 1 OFFSET 0";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }


        public static List<string[]> getItmCnsgmtVals(long itmID)
        {
            List<string[]> res = new List<string[]>();

            string strSql = "SELECT distinct c.consgmt_id, inv.get_csgmt_lst_tot_bls(c.consgmt_id), c.cost_price " +
              "FROM inv.inv_consgmt_rcpt_det c " +
              "WHERE ((c.itm_id=" + itmID + ") and (c.subinv_id =" + Global.selectedStoreID +
              ") and  (inv.get_csgmt_lst_tot_bls(c.consgmt_id)>0)) ORDER BY c.consgmt_id ASC";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);

            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                string[] rec = new string[3];

                rec[0] = dtst.Tables[0].Rows[i][0].ToString();
                rec[1] = dtst.Tables[0].Rows[i][1].ToString();
                rec[2] = dtst.Tables[0].Rows[i][2].ToString();
                res.Add(rec);
            }
            return res;
        }

        public static List<string[]> getItmCnsgmtVals(double qnty, string cnsgmtIDs)
        {
            List<string[]> res = new List<string[]>();
            if (cnsgmtIDs == "")
            {
                cnsgmtIDs = "-123412";
            }

            string strSql = "SELECT distinct c.consgmt_id, inv.get_csgmt_lst_avlbl_bls(c.consgmt_id), c.cost_price " +
              "FROM inv.inv_consgmt_rcpt_det c " +
              "WHERE ((c.consgmt_id IN (" + cnsgmtIDs.Trim(',') + "))) ORDER BY c.consgmt_id ASC";
            // and (inv.get_csgmt_lst_avlbl_bls(c.consgmt_id)>0)
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double remQty = qnty;
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                double crQty = double.Parse(dtst.Tables[0].Rows[i][1].ToString());
                string[] rec = new string[3];
                if (crQty <= remQty)
                {
                    rec[0] = dtst.Tables[0].Rows[i][0].ToString();
                    rec[1] = dtst.Tables[0].Rows[i][1].ToString();
                    rec[2] = dtst.Tables[0].Rows[i][2].ToString();
                    remQty -= crQty;
                    res.Add(rec);
                }
                else if (remQty > 0)
                {
                    rec[0] = dtst.Tables[0].Rows[i][0].ToString();
                    rec[1] = remQty.ToString();
                    rec[2] = dtst.Tables[0].Rows[i][2].ToString();
                    //remQty -= crQty;
                    res.Add(rec);
                    return res;
                }
                else
                {
                    return res;
                }
            }
            return res;
        }

        public static List<string[]> getSRItmCnsgmtVals(long lnID, double qnty, string cnsgmtIDs, long srcDocLnID)
        {
            List<string[]> res = new List<string[]>();
            List<string[]> oldres = Global.getCsgmtsDist(srcDocLnID, cnsgmtIDs);
            double remQty = qnty;
            for (int i = oldres.Count - 1; i >= 0; i--)
            {
                string[] ary = oldres[i];
                long figID = 0;
                long.TryParse(ary[0], out figID);
                double fig1Qty = 0;
                double fig2Prc = 0;
                double.TryParse(ary[1], out fig1Qty);
                double.TryParse(ary[2], out fig2Prc);
                double crQty = fig1Qty;
                string[] rec = new string[3];
                //Global.mnFrm.cmCde.showMsg(ary[0] + ary[1] + ary[2], 0);
                if (crQty <= remQty)
                {
                    rec[0] = figID.ToString();
                    rec[1] = fig1Qty.ToString();
                    rec[2] = fig2Prc.ToString();
                    remQty -= crQty;
                    res.Add(rec);
                }
                else if (remQty > 0)
                {
                    rec[0] = figID.ToString();
                    rec[1] = remQty.ToString();
                    rec[2] = fig2Prc.ToString();
                    //remQty -= crQty;
                    res.Add(rec);
                    return res;
                }
                else
                {
                    return res;
                }
            }
            return res;
        }

        public static List<string[]> getCsgmtsDist(long lnID, string cnsgmtIDs)
        {
            List<string[]> res = new List<string[]>();
            string strSql = "SELECT distinct c.cnsgmnt_qty_dist " +
         "FROM scm.scm_sales_invc_det c " +
         "WHERE ((c.invc_det_ln_id =" + lnID + ") and (consgmnt_ids='" + cnsgmtIDs + "'))";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                char[] mw = { ',' };
                string[] ary = dtst.Tables[0].Rows[0][0].ToString().Split(mw, StringSplitOptions.RemoveEmptyEntries);
                string[] ary1 = cnsgmtIDs.Split(mw, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < ary1.Length; i++)
                {
                    string[] rec = new string[3];//Very very important to avoid same values entering List several times
                    try
                    {
                        rec[0] = ary1[i];
                        rec[1] = ary[i];
                        rec[2] = Global.mnFrm.cmCde.getGnrlRecNm("inv.inv_consgmt_rcpt_det", "consgmt_id", "cost_price", long.Parse(ary1[i]));
                        res.Add(rec);
                        //Global.mnFrm.cmCde.showMsg(rec[0] + "/" + rec[1] + "/" + rec[2], 0);

                    }
                    catch (Exception ex)
                    {
                        rec[0] = ary1[i];
                        rec[1] = "0";
                        rec[2] = "0";
                        res.Add(rec);
                        //Global.mnFrm.cmCde.showMsg(rec[0] + "/" + rec[1] + "/" + rec[2], 0);
                    }
                }
            }
            //Global.mnFrm.cmCde.showMsg(res[0][0] + "/" + res[0][1] + "/" + res[0][2], 0);
            //if (res.Count == 2)
            //{
            //  Global.mnFrm.cmCde.showMsg(res[1][0] + "/" + res[1][1] + "/" + res[1][2], 0);
            //}
            return res;
        }

        public static double getItmTrnsfTtlCost(double qnty, string cnsgmtIDs)
        {
            if (cnsgmtIDs == "")
            {
                cnsgmtIDs = "-123412";
            }

            string strSql = "SELECT distinct c.consgmt_id, inv.get_csgmt_lst_avlbl_bls(c.consgmt_id), c.cost_price " +
              "FROM inv.inv_consgmt_rcpt_det c " +
              "WHERE ((c.consgmt_id IN (" + cnsgmtIDs.Trim(',') + ")) and (inv.get_csgmt_lst_avlbl_bls(c.consgmt_id)>0)) ORDER BY c.consgmt_id ASC";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            double ttlCst = 0;
            double remQty = qnty;
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                double crQty = double.Parse(dtst.Tables[0].Rows[i][1].ToString());
                //string[] rec = new string[3];
                if (crQty <= remQty)
                {
                    ttlCst += double.Parse(dtst.Tables[0].Rows[i][1].ToString()) * double.Parse(dtst.Tables[0].Rows[i][2].ToString());
                    remQty -= crQty;
                }
                else if (remQty > 0)
                {
                    ttlCst += remQty * double.Parse(dtst.Tables[0].Rows[i][2].ToString());
                    return ttlCst;
                }
                else
                {
                    return ttlCst;
                }
            }
            return ttlCst;
        }

        #endregion

        #region "SERVICE TYPES..."
        public static DataSet get_SrvcTyps(
       string searchWord, string searchIn,
      Int64 offset, int limit_size, int orgID)
        {
            string strSql = "";
            string whereClause = "";

            if (searchIn == "Service Name")
            {
                whereClause = "(a.type_name ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Service Description")/*Decription have you seen  why it is not working?*/
            {
                whereClause = "(a.type_desc ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else /*if (searchIn == "Type of Service")*/
            {
                whereClause = "(a.sys_code ilike '" + searchWord.Replace("'", "''") +
            "')";
            }

            strSql = "SELECT a.type_id, a.type_name,a.sys_code " +
         "FROM hosp.srvs_types a " +
         "WHERE " + whereClause +
         " and org_id = " + orgID + " ORDER BY a.type_id DESC LIMIT " + limit_size +
         " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            Global.wfnSrvcOfrdFrm.rec_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_Ttl_SrvsTyps(string searchWord, string searchIn, int orgID)
        {
            string strSql = "";
            string whereClause = "";

            if (searchIn == "Service Name")
            {
                whereClause = "(a.type_name ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Service Description")/*Decription have you seen  why it is not working?*/
            {
                whereClause = "(a.type_desc ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else /*if (searchIn == "Type of Service")*/
            {
                whereClause = "(a.sys_code ilike '" + searchWord.Replace("'", "''") +
            "')";
            }

            strSql = "SELECT count(1) " +
         "FROM hosp.srvs_types a " +
         "WHERE " + whereClause + " and org_id = " + orgID;
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

        public static DataSet get_One_ServTypeDt(int serv_type_hdrID)
        {
            string strSql = @"SELECT a.type_id, a.type_name, a.type_desc, a.is_enabled, a.itm_id, a.sys_code " +
         "FROM hosp.srvs_types a " +
         "WHERE a.type_id ='" + serv_type_hdrID + "'";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
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

        public static double get_InvItemPriceLsTx(int itmID)
        {
            string strSql = "SELECT orgnl_selling_price " +
         "FROM inv.inv_itm_list a " +
         "WHERE item_id =" + itmID + "";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0.00;
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

        public static DataSet getAllAppntedDays()
        {
            string strSql = @"SELECT DISTINCT substring(a.appntmnt_date from 1 for 10) 
      FROM hosp.appntmnt a WHERE a.appntmnt_status != 'Closed' ORDER BY 1 ASC";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_dshbrd_items(
       string searchWord, string searchIn,
      Int64 offset, int limit_size, int orgID,
          string dsplyType)
        {
            string strSql = "";
            string whereClause = "";
            /*Service Provider Groups
      Individual Service Providers

             For Groups-Get all Groups a.cnrt_days_appntmnts and a.mx_no_daily_appntmnts 
             For Providers-Get all Providers a.crnt_days_appntmnts and mx_daily_appntmnts for the group
             * For Days- Get all Days with Unclosed Appointments +/- 1 day
             */
            if (dsplyType == "Service Provider Groups")
            {
                whereClause = "(a.prvdr_grp_name ilike '" + searchWord.Replace("'", "''") + "')";
                strSql = @"SELECT a.prvdr_grp_id, a.prvdr_grp_name, 
         a.prvdr_grp_desc, a.enabled_flag,
         CASE WHEN a.enabled_flag != '1' THEN 'BLOCKED' 
        WHEN a.cur_days_appntmnts = a.max_daily_appntmnts AND a.cur_days_appntmnts>0 THEN 'FULLY BOOKED' 
        WHEN a.cur_days_appntmnts < a.max_daily_appntmnts AND a.cur_days_appntmnts>0 THEN 'PARTIALLY BOOKED' 
        WHEN a.cur_days_appntmnts > a.max_daily_appntmnts THEN 'OVERLOADED'
        ELSE 'AVAILABLE' END status,
a.max_daily_appntmnts,a.cur_days_appntmnts, b.type_name,
COALESCE((Select to_char(to_timestamp(MIN(y.appntmnt_date),'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY')||' to '||
to_char(to_timestamp(MAX(y.appntmnt_end_date),'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY') 
from hosp.appntmnt y 
where (y.appntmnt_status !='Closed') and y.prvdr_grp_id = a.prvdr_grp_id), '') period_rsvrd, 
'' cstmr_nm 
    FROM hosp.prvdr_grps a, hosp.srvs_types b " +
                   "WHERE " + whereClause + " and a.main_srvc_type_id=b.type_id and b.org_id=" +
                   Global.mnFrm.cmCde.Org_id + " ORDER BY a.prvdr_grp_name ASC LIMIT " + limit_size +
                   " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }
            else if (dsplyType == "Individual Service Providers")
            {
                whereClause = @"((CASE WHEN a.prsn_id > 0 THEN prs.get_prsn_name(a.prsn_id) ||' (' || prs.get_prsn_loc_id(a.prsn_id) || ')' 
ELSE scm.get_cstmr_splr_name(a.cstmr_id) END) ilike '" + searchWord.Replace("'", "''") + "')";
                strSql = @"SELECT a.prvdr_id, CASE WHEN a.prsn_id > 0 THEN prs.get_prsn_name(a.prsn_id) ||' (' || prs.get_prsn_loc_id(a.prsn_id) || ')' 
ELSE scm.get_cstmr_splr_name(a.cstmr_id) END , a.comments_rmrks, 
        (CASE WHEN now() between to_timestamp(a.start_date,'YYYY-MM-DD HH24:MI:SS') 
        and to_timestamp(a.end_date,'YYYY-MM-DD HH24:MI:SS') THEN '1' ELSE '0' END) enabled_flag,
        CASE WHEN (CASE WHEN now() between to_timestamp(a.start_date,'YYYY-MM-DD HH24:MI:SS') 
        and to_timestamp(a.end_date,'YYYY-MM-DD HH24:MI:SS') THEN '1' ELSE '0' END) != '1' THEN 'BLOCKED' 
        WHEN a.cur_days_appntmnts = a.max_daily_appntmnts AND a.cur_days_appntmnts>0 THEN 'FULLY BOOKED' 
        WHEN a.cur_days_appntmnts < a.max_daily_appntmnts AND a.cur_days_appntmnts>0 THEN 'PARTIALLY BOOKED' 
        WHEN a.cur_days_appntmnts > a.max_daily_appntmnts THEN 'OVERLOADED'
        ELSE 'AVAILABLE' END status,
a.max_daily_appntmnts,a.cur_days_appntmnts, b.type_name,
COALESCE((Select to_char(to_timestamp(MIN(y.appntmnt_date),'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY')||' to '||
to_char(to_timestamp(MAX(y.appntmnt_end_date),'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY') 
from hosp.appntmnt y 
where (y.appntmnt_status !='Closed') and y.srvs_prvdr_prsn_id = a.prvdr_id), '') period_rsvrd, '' cstmr_nm
    FROM hosp.srvs_prvdrs a, hosp.srvs_types b " +
             "WHERE " + whereClause + " and a.srvs_type_id=b.type_id and b.org_id=" +
             Global.mnFrm.cmCde.Org_id + " ORDER BY 2 ASC LIMIT " + limit_size +
             " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }

            Global.wfnSmmryDshForm.rec_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_Ttl_dshbrd_items(
      string searchWord, string searchIn, int orgID, string dsplyType)
        {
            string strSql = "";
            string whereClause = "";
            /*Service Provider Groups
      Individual Service Providers

             For Groups-Get all Groups a.cnrt_days_appntmnts and a.mx_no_daily_appntmnts 
             For Providers-Get all Providers a.crnt_days_appntmnts and mx_daily_appntmnts for the group
             * For Days- Get all Days with Unclosed Appointments +/- 1 day
             */
            if (dsplyType == "Service Provider Groups")
            {
                whereClause = "(a.prvdr_grp_name ilike '" + searchWord.Replace("'", "''") + "')";
                strSql = @"SELECT count(1) 
    FROM hosp.prvdr_grps a, hosp.srvs_types b " +
                   "WHERE " + whereClause + " and a.main_srvc_type_id=b.type_id and b.org_id=" +
                   Global.mnFrm.cmCde.Org_id;
            }
            else if (dsplyType == "Individual Service Providers")
            {
                whereClause = @"((CASE WHEN a.prsn_id > 0 THEN prs.get_prsn_name(a.prsn_id) ||' (' || prs.get_prsn_loc_id(a.prsn_id) || ')' 
ELSE scm.get_cstmr_splr_name(d.cstmr_id) END) ilike '" + searchWord.Replace("'", "''") + "')";
                strSql = @"SELECT count(1)
    FROM hosp.srvs_prvdrs a, hosp.srvs_types b " +
             "WHERE " + whereClause + " and a.srvs_type_id=b.type_id and b.org_id=" +
             Global.mnFrm.cmCde.Org_id;
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

        public static DataSet get_room_prices(int serv_type_hdrID)
        {
            string whereClause = "";

            string strSql = @"SELECT special_price_id, 
to_char(to_timestamp(a.start_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
to_char(to_timestamp(a.end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'),
price_less_tx, is_enabled, selling_price
  FROM hotl.service_type_prices a " +
         "WHERE service_type_id = " + serv_type_hdrID + whereClause + " ORDER BY start_date";

            //Global.wfnSrvTypeFrm.prices_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_datadfntns(int serv_type_hdrID,
       string searchWord, string searchIn,
      Int64 offset, int limit_size)
        {
            string whereClause = "";
            /*
              Data Category
              Default Label
              Data Type*/

            if (searchIn == "Data Category")
            {
                whereClause = " and (a.data_catgry ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Default Label")
            {
                whereClause = " and (a.data_label ilike '" + searchWord.Replace("'", "''") +
              "')";
            }
            else if (searchIn == "Data Type")
            {
                whereClause = " and (a.data_type ilike '" + searchWord.Replace("'", "''") +
              "')";
            }

            string strSql = @"SELECT srvc_data_capture_id, srvc_type_id, data_catgry, data_label, 
       data_type, data_values_lov, data_value_desc_lov, is_enabled
    FROM hosp.srvc_typ_data_to_capture a " +
         "WHERE srvc_type_id = " + serv_type_hdrID + whereClause + " ORDER BY data_catgry, data_label  LIMIT " + limit_size +
         " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            Global.wfnSrvcOfrdFrm.recDt_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_ttl_datadfntns(int serv_type_hdrID,
       string searchWord, string searchIn)
        {
            string whereClause = "";
            /*
              Data Category
              Default Label
              Data Type*/

            if (searchIn == "Data Category")
            {
                whereClause = " and (a.data_catgry ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Default Label")
            {
                whereClause = " and (a.data_label ilike '" + searchWord.Replace("'", "''") +
              "')";
            }
            else if (searchIn == "Data Type")
            {
                whereClause = " and (a.data_type ilike '" + searchWord.Replace("'", "''") +
              "')";
            }

            string strSql = @"SELECT count(1)
    FROM hosp.srvc_typ_data_to_capture a " +
         "WHERE srvc_type_id = " + serv_type_hdrID + whereClause;

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

        public static string get_Rec_Hstry(int hdrID)
        {
            string strSQL = @"SELECT a.created_by, 
to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
      a.last_update_by, 
      to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM hotl.service_types a WHERE(a.service_type_id = " + hdrID + ")";
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

        public static string get_DT_Rec_Hstry(int dteID)
        {
            string strSQL = @"SELECT a.created_by, 
to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
      a.last_update_by, 
      to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM hosp.srvc_typ_data_to_capture a WHERE(a.srvc_data_capture_id = " + dteID + ")";
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

        public static void deleteSrvsTyp(long hdrID, string srvsNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Service Name = " + srvsNm;
            string delSQL = "DELETE FROM hosp.srvc_typ_data_to_capture WHERE srvc_type_id = " + hdrID;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);

            delSQL = "DELETE FROM hosp.srvs_types WHERE type_id = " + hdrID;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteSrvsTypLn(long Lnid, string critrNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Data Label = " + critrNm;
            string delSQL = "DELETE FROM hosp.srvc_typ_data_to_capture WHERE srvc_data_capture_id = " + Lnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static bool isSrvsTypInUse(long hdrid)
        {
            string strSql = "SELECT a.appntmnt_id " +
             "FROM hosp.appntmnt a " +
             "WHERE(a.srvs_type_id = " + hdrid + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }

            strSql = "SELECT a.prvdr_id " +
            "FROM hosp.srvs_prvdrs a " +
            "WHERE(a.srvs_type_id = " + hdrid + ")";
            dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }

            strSql = "SELECT a.prvdr_grp_id " +
            "FROM hosp.prvdr_grps a " +
            "WHERE(a.main_srvc_type_id = " + hdrid + ")";
            dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static bool isSrvcDataCaptureInUse(long srvcDataCptrid)
        {
            string strSql = "SELECT a.data_capture_id " +
             "FROM hosp.appntmnt_data_captured a " +
             "WHERE(a.srvc_data_capture_id = " + srvcDataCptrid + ")";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static int getSrvsTypID(string srvstypname, int orgid)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "select type_id from hosp.srvs_types where lower(type_name) = '" +
             srvstypname.Replace("'", "''").ToLower() + "' and org_id = " + orgid;
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

        public static long getSrvcsDataDefID(string dfltLabelName, string dataCtgry, int srvTypID)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = @"select a.srvc_data_capture_id from hosp.srvc_typ_data_to_capture a 
         where lower(a.data_label) = '" + dfltLabelName.Replace("'", "''").ToLower() +
            "' and lower(a.data_catgry) ='" + dataCtgry.Replace("'", "''").ToLower() +
            "' and a.srvc_type_id = " + srvTypID;
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

        public static int getPriceID(string strtDate, string endDte, int srvsTypID)
        {
            strtDate = DateTime.ParseExact(
      strtDate, "dd-MMM-yyyy HH:mm:ss",
      System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            endDte = DateTime.ParseExact(
         endDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            DataSet dtSt = new DataSet();
            string sqlStr = @"select a.special_price_id from hotl.service_type_prices a where start_date = '" +
             strtDate.Replace("'", "''") + "' and end_date = '" +
             endDte.Replace("'", "''") + "' and a.service_type_id = " + srvsTypID;
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

        public static void createSrvsTyp(string srvsTypname,
      string srvsTypdesc, int itmId, bool isEnbld, string srvcType, int orgID)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO hosp.srvs_types(
            type_name, type_desc, created_by, creation_date, last_update_by, 
            last_update_date, sys_code, itm_id, is_enabled, org_id) " +
                  "VALUES ('" + srvsTypname.Replace("'", "''") +
                  "', '" + srvsTypdesc.Replace("'", "''") +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', '" + srvcType.Replace("'", "''") +
                  "', " + itmId +
                  ", '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isEnbld) +
                  "', " + orgID + ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updateSrvsTyp(int srvcTypID, string srvsTypname,
      string srvsTypdesc, int itmId, bool isEnbld, string srvcType)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE hosp.srvs_types SET " +
                  "type_name='" + srvsTypname.Replace("'", "''") +
                  "', type_desc='" + srvsTypdesc.Replace("'", "''") +
                  "', itm_id=" + itmId +
                  ", last_update_by=" + Global.myVst.user_id + ", " +
                  "last_update_date='" + dateStr +
                  "', is_enabled='" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(isEnbld) +
                  "', sys_code = '" + srvcType.Replace("'", "''") +
                  "' WHERE (type_id =" + srvcTypID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void createDataDefntn(int srvcTypID,
      string dataCtgry, string dataLabel, bool isEnbld, string dataType,
          string dataValsLov, string dataValsDescLov)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO hosp.srvc_typ_data_to_capture(
            srvc_type_id, data_catgry, data_label, 
            data_type, created_by, creation_date, last_update_by, last_update_date, 
            data_value_desc_lov, data_values_lov, is_enabled) " +
                  "VALUES (" + srvcTypID + ", '" + dataCtgry.Replace("'", "''") +
                  "', '" + dataLabel.Replace("'", "''") +
                  "', '" + dataType.Replace("'", "''") +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', '" + dataValsDescLov.Replace("'", "''") +
                  "', '" + dataValsLov.Replace("'", "''") +
                  "', '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isEnbld) +
                  "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updateDataDefntn(long dataDefID,
      string dataCtgry, string dataLabel, bool isEnbld, string dataType,
          string dataValsLov, string dataValsDescLov)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE hosp.srvc_typ_data_to_capture SET 
             data_catgry = '" + dataCtgry.Replace("'", "''") +
                  "', data_label = '" + dataLabel.Replace("'", "''") +
                  "', data_type = '" + dataType.Replace("'", "''") +
                  "', is_enabled = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isEnbld) +
                  "', last_update_by = " + Global.myVst.user_id +
                  ",  last_update_date = '" + dateStr +
                  "', data_value_desc_lov = '" + dataValsDescLov.Replace("'", "''") +
                  "', data_values_lov = '" + dataValsLov.Replace("'", "''") +
                  "' WHERE srvc_data_capture_id=" + dataDefID + "";
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void updatePrvdrGrpBlckdStatus(long grpID, bool isblckd)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE hosp.prvdr_grps SET 
             last_update_by=" + Global.myVst.user_id +
                  ", last_update_date='" + dateStr +
                  "', enabled_flag='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isblckd) +
                  "' WHERE prvdr_grp_id=" + grpID + " ";
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        //    public static void updatePrvdrBlckdStatus(long prvdrID, bool isblckd)
        //    {
        //      Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
        //      string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
        //      string insSQL = @"UPDATE hosp.srvs_prvdrs SET 
        //             last_update_by=" + Global.myVst.user_id +
        //            ", last_update_date='" + dateStr +
        //            "', end_date='" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isblckd) +
        //            "' WHERE prvdr_grp_id=" + grpID + " ";
        //      Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        //    }

        public static double getCstmrDpsts(int cstmrID, int invcurID)
        {
            string selSQL = @"select SUM(invoice_amount-invc_amnt_appld_elswhr) c, customer_id e, 
invc_curr_id f from accb.accb_rcvbls_invc_hdr where (((rcvbls_invc_type = 'Customer Advance Payment' and (invoice_amount-amnt_paid)<=0) 
or rcvbls_invc_type = 'Customer Debit Memo (InDirect Refund)') 
and approval_status='Approved' and (invoice_amount-invc_amnt_appld_elswhr)>0 and customer_id = " + cstmrID + " and customer_id>0 and invc_curr_id = " + invcurID + @") 
GROUP BY customer_id,invc_curr_id";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(selSQL);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return double.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }

        public static bool isPrvdrFree(long prvdrID, long prvdrGrpID, string strtDte, string endDte, long curAppntID)
        {
            strtDte = DateTime.ParseExact(
      strtDte, "dd-MMM-yyyy HH:mm:ss",
      System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            endDte = DateTime.ParseExact(
         endDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "";
            long prvdrGrpAppntmntsCnt = 0;
            strSql = "SELECT count(a.appntmnt_id) " +
             "FROM hosp.appntmnt a " +
             "WHERE(a.prvdr_grp_id = " + prvdrGrpID +
             " and (a.appntmnt_status != 'Closed') and a.appntmnt_id != " + curAppntID + " and (to_timestamp('" + strtDte + @"','YYYY-MM-DD HH24:MI:SS') between 
to_timestamp(a.appntmnt_date,'YYYY-MM-DD HH24:MI:SS') 
AND to_timestamp(a.appntmnt_end_date,'YYYY-MM-DD HH24:MI:SS') or to_timestamp('" + endDte +
      @"','YYYY-MM-DD HH24:MI:SS') between to_timestamp(a.appntmnt_date,'YYYY-MM-DD HH24:MI:SS') 
AND to_timestamp(a.appntmnt_end_date,'YYYY-MM-DD HH24:MI:SS')))";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count == 1)
            {
                prvdrGrpAppntmntsCnt = long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            strSql = "SELECT count(a.prvdr_id) " +
             "FROM hosp.srvs_prvdrs a " +
             "WHERE(a.prvdr_grp_id = " + prvdrGrpID +
             " and (to_timestamp('" + strtDte + @"','YYYY-MM-DD HH24:MI:SS') between 
to_timestamp(a.start_date,'YYYY-MM-DD HH24:MI:SS') 
AND to_timestamp(a.end_date,'YYYY-MM-DD HH24:MI:SS') or to_timestamp('" + endDte +
      @"','YYYY-MM-DD HH24:MI:SS') between to_timestamp(a.start_date,'YYYY-MM-DD HH24:MI:SS') 
AND to_timestamp(a.end_date,'YYYY-MM-DD HH24:MI:SS')))";
            dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count == 1)
            {
                if (long.Parse(dtst.Tables[0].Rows[0][0].ToString()) <= prvdrGrpAppntmntsCnt
                  && prvdrGrpAppntmntsCnt > 0)
                {
                    return false;
                }
                else
                {
                    if (prvdrID > 0)
                    {
                        strSql = "SELECT count(a.appntmnt_id) " +
                         "FROM hosp.appntmnt a " +
                         "WHERE(a.srvs_prvdr_prsn_id = " + prvdrID +
                         " and a.appntmnt_id != " + curAppntID + " and a.srvs_prvdr_prsn_id>0 and (a.appntmnt_status != 'Closed') and (to_timestamp('" + strtDte + @"','YYYY-MM-DD HH24:MI:SS') between 
to_timestamp(a.appntmnt_date,'YYYY-MM-DD HH24:MI:SS') 
AND to_timestamp(a.appntmnt_end_date,'YYYY-MM-DD HH24:MI:SS') or to_timestamp('" + endDte +
                  @"','YYYY-MM-DD HH24:MI:SS') between to_timestamp(a.appntmnt_date,'YYYY-MM-DD HH24:MI:SS') 
AND to_timestamp(a.appntmnt_end_date,'YYYY-MM-DD HH24:MI:SS')))";
                        dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
                        if (dtst.Tables[0].Rows.Count == 1)
                        {
                            return long.Parse(dtst.Tables[0].Rows[0][0].ToString()) <= 0;
                        }
                    }
                }
            }
            return true;
        }

        public static void updatePrvdrGrpApptmtCnt(DateTime dteToCheck)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            //string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE hosp.prvdr_grps a 
SET cur_days_appntmnts = COALESCE((SELECT count(b.appntmnt_id) 
  FROM hosp.appntmnt b
  WHERE (b.appntmnt_status != 'Closed') and a.prvdr_grp_id = b.prvdr_grp_id  
and (substring(b.appntmnt_date from 1 for 10) = '" + dteToCheck.ToString("yyyy-MM-dd") + @"' 
or substring(b.appntmnt_end_date from 1 for 10) = '" + dteToCheck.ToString("yyyy-MM-dd") + @"')),0)";
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void updatePrvdrGrpApptmtCnt()
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            //string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE hosp.prvdr_grps a 
SET cur_days_appntmnts = COALESCE((SELECT count(b.appntmnt_id) 
  FROM hosp.appntmnt b
  WHERE (b.appntmnt_status != 'Closed') and a.prvdr_grp_id = b.prvdr_grp_id  
and (substring(b.appntmnt_date from 1 for 10) = to_char(now(), 'YYYY-MM-dd') 
or substring(b.appntmnt_end_date from 1 for 10) = to_char(now(), 'YYYY-MM-dd'))),0)";
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void updatePrvdrApptmtCnt(DateTime dteToCheck)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            //string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE hosp.srvs_prvdrs a 
SET cur_days_appntmnts = COALESCE((SELECT count(b.appntmnt_id) 
  FROM hosp.appntmnt b
  WHERE (b.appntmnt_status != 'Closed') and a.prvdr_id = b.srvs_prvdr_prsn_id  
and (substring(b.appntmnt_date from 1 for 10) = '" + dteToCheck.ToString("yyyy-MM-dd") + @"' 
or substring(b.appntmnt_end_date from 1 for 10) = '" + dteToCheck.ToString("yyyy-MM-dd") + @"')),0)";
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        public static void updatePrvdrApptmtCnt()
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            //string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE hosp.srvs_prvdrs a 
SET cur_days_appntmnts = COALESCE((SELECT count(b.appntmnt_id) 
  FROM hosp.appntmnt b
  WHERE (b.appntmnt_status != 'Closed') and a.prvdr_id = b.srvs_prvdr_prsn_id  
and (substring(b.appntmnt_date from 1 for 10) = to_char(now(), 'YYYY-MM-dd') 
or substring(b.appntmnt_end_date from 1 for 10) = to_char(now(), 'YYYY-MM-dd'))),0)";
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        #endregion

        #region "APPOINTMENT DATA..."
        public static DataSet get_Appntmts(
       string searchWord, string searchIn,
      Int64 offset, int limit_size, int orgID,
          long vstID, long appntmtID)
        {
            /*Appointment Date
      Appointment Description
      Appointment Number
      Service Requested*/
            string strSql = "";
            string whereClause = "";
            string extrWhere = " and 1=1";
            if (vstID > 0)
            {
                extrWhere = " and a.vst_id=" + vstID;
            }
            else if (appntmtID > 0)
            {
                extrWhere = " and a.appntmnt_id=" + appntmtID;
            }
            if (searchIn == "Appointment Date")
            {
                whereClause = "(to_char(to_timestamp(a.appntmnt_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") +
            "' or to_char(to_timestamp(a.appntmnt_end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Appointment Description")/*Decription have you seen  why it is not working?*/
            {
                whereClause = "(a.cmnts ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Appointment Number")/**/
            {
                whereClause = "(('' || a.appntmnt_id) ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else /*if (searchIn == "Service Requested")*/
            {
                whereClause = "(b.type_name ilike '" + searchWord.Replace("'", "''") +
            "')";
            }

            strSql = @"SELECT a.appntmnt_id, 
        to_char(to_timestamp(a.appntmnt_date, 'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
        to_char(to_timestamp(a.appntmnt_end_date, 'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
        b.type_name, a.appntmnt_status, a.cmnts, a.prvdr_type, a.srvs_type_id, 
        a.srvs_prvdr_prsn_id, a.prvdr_grp_id, c.prvdr_grp_name,
CASE WHEN d.prsn_id > 0 THEN prs.get_prsn_name(d.prsn_id) ||' (' || prs.get_prsn_loc_id(d.prsn_id) || ')' 
ELSE scm.get_cstmr_splr_name(d.cstmr_id) END 
        FROM hosp.appntmnt a
        LEFT OUTER JOIN hosp.srvs_types b ON (b.type_id = a.srvs_type_id)
        LEFT OUTER JOIN hosp.prvdr_grps c ON (a.prvdr_grp_id = c.prvdr_grp_id)
        LEFT OUTER JOIN hosp.srvs_prvdrs d ON (a.srvs_prvdr_prsn_id = d.prvdr_id) " +
         "WHERE " + whereClause + extrWhere +
         " and b.org_id = " + orgID + " ORDER BY a.appntmnt_date DESC LIMIT " + limit_size +
         " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            if (Global.wfnApntmtFrm != null)
            {
                Global.wfnApntmtFrm.rec_SQL = strSql;
            }
            if (Global.wfnApntmtFrmDiag != null)
            {
                Global.wfnApntmtFrmDiag.rec_SQL = strSql;
            }
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_Ttl_Appntmts(string searchWord, string searchIn, int orgID,
          long vstID, long appntmtID)
        {
            /*Appointment Date
      Appointment Description
      Appointment Number
      Service Requested*/
            string strSql = "";
            string whereClause = "";
            string extrWhere = " and 1=1";
            if (vstID > 0)
            {
                extrWhere = " and a.vst_id=" + vstID;
            }
            else if (appntmtID > 0)
            {
                extrWhere = " and a.appntmnt_id=" + appntmtID;
            }
            if (searchIn == "Appointment Date")
            {
                whereClause = "(to_char(to_timestamp(a.appntmnt_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") +
            "' or to_char(to_timestamp(a.appntmnt_end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Appointment Description")/*Decription have you seen  why it is not working?*/
            {
                whereClause = "(a.cmnts ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Appointment Number")/**/
            {
                whereClause = "(('' || a.appntmnt_id) ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else /*if (searchIn == "Service Requested")*/
            {
                whereClause = "(b.type_name ilike '" + searchWord.Replace("'", "''") +
            "')";
            }

            strSql = @"SELECT count(1)  
        FROM hosp.appntmnt a
        LEFT OUTER JOIN hosp.srvs_types b ON (b.type_id = a.srvs_type_id)
        LEFT OUTER JOIN hosp.prvdr_grps c ON (a.prvdr_grp_id = c.prvdr_grp_id)
        LEFT OUTER JOIN hosp.srvs_prvdrs d ON (a.srvs_prvdr_prsn_id = d.prvdr_id) " +
         "WHERE " + whereClause + extrWhere +
         " and b.org_id = " + orgID;
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

        public static bool isPrsnInPrvdrGrp(long prvdrGrpID, long prsnID)
        {
            string selSQL = "Select count(1) from hosp.srvs_prvdrs where prsn_id=" + prsnID + " and prvdr_grp_id = " + prvdrGrpID;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(selSQL);
            if (dtst.Tables[0].Rows.Count == 1)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString()) > 0;
            }
            return false;
        }

        public static DataSet get_AppntmtData(long appntmntID,
          long srvsTypID,
       string searchWord, string searchIn,
      Int64 offset, int limit_size)
        {
            string whereClause = "";
            /*
              Data Captured
      Data Category
      Data Label*/

            if (searchIn == "Data Category")
            {
                whereClause = " and ((CASE WHEN b.data_catgry IS NULL THEN a.data_catgry ELSE b.data_catgry END) ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Data Label")
            {
                whereClause = " and ((CASE WHEN b.data_label IS NULL THEN a.data_label ELSE b.data_label END) ilike '" + searchWord.Replace("'", "''") +
              "')";
            }
            else if (searchIn == "Data Captured")
            {
                whereClause = " and (COALESCE(b.data_value, '') ilike '" + searchWord.Replace("'", "''") +
              "')";
            }

            string strSql = @"SELECT COALESCE(b.data_capture_id, -1), a.srvc_data_capture_id, a.srvc_type_id, 
    CASE WHEN b.data_catgry IS NULL THEN a.data_catgry ELSE b.data_catgry END, 
    CASE WHEN b.data_label IS NULL THEN a.data_label ELSE b.data_label END, 
    COALESCE(b.data_value, ''), a.data_type, a.data_values_lov, 
COALESCE(b.data_val_desc,''), a.data_value_desc_lov 
    FROM hosp.srvc_typ_data_to_capture a 
    LEFT OUTER JOIN hosp.appntmnt_data_captured b 
ON (a.srvc_data_capture_id = b.srvc_data_capture_id and b.appntmnt_id = " + appntmntID +
         ") " +
         "WHERE a.srvc_type_id = " + srvsTypID + " and (b.appntmnt_id = " + appntmntID +
         " or b.appntmnt_id IS NULL) and (a.is_enabled='1' or b.srvc_data_capture_id IS NOT NULL)" + whereClause + " ORDER BY 4, 5  LIMIT " + limit_size +
         " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            if (Global.wfnApntmtFrm != null)
            {
                Global.wfnApntmtFrm.recDt_SQL = strSql;
            }
            if (Global.wfnApntmtFrmDiag != null)
            {
                Global.wfnApntmtFrmDiag.recDt_SQL = strSql;
            }

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_ttl_AppntmtData(long appntmntID,
          long srvsTypID,
       string searchWord, string searchIn)
        {
            string whereClause = "";
            /*
              Data Captured
      Data Category
      Data Label*/

            if (searchIn == "Data Category")
            {
                whereClause = " and ((CASE WHEN b.data_catgry IS NULL THEN a.data_catgry ELSE b.data_catgry END) ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Data Label")
            {
                whereClause = " and ((CASE WHEN b.data_label IS NULL THEN a.data_label ELSE b.data_label END) ilike '" + searchWord.Replace("'", "''") +
              "')";
            }
            else if (searchIn == "Data Captured")
            {
                whereClause = " and (COALESCE(b.data_value, '') ilike '" + searchWord.Replace("'", "''") +
              "')";
            }

            string strSql = @"SELECT count(1) 
    FROM hosp.srvc_typ_data_to_capture a 
    LEFT OUTER JOIN hosp.appntmnt_data_captured b ON (a.srvc_data_capture_id = b.srvc_data_capture_id and b.appntmnt_id = " + appntmntID +
         ") " +
         "WHERE a.srvc_type_id = " + srvsTypID + " and (b.appntmnt_id = " + appntmntID +
         " or b.appntmnt_id IS NULL) and (a.is_enabled='1' or b.srvc_data_capture_id IS NOT NULL)" + whereClause;

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

        public static string get_AppntmtDT_Rec_Hstry(long dteID)
        {
            string strSQL = @"SELECT a.created_by, 
to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
      a.last_update_by, 
      to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM hosp.appntmnt_data_captured a WHERE(a.data_capture_id = " + dteID + ")";
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

        public static void deleteAppntmt(long hdrID, string srvsNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Service Name = " + srvsNm;
            string delSQL = "DELETE FROM hosp.appntmnt_data_captured WHERE appntmnt_id = " + hdrID;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);

            delSQL = "DELETE FROM hosp.appntmnt WHERE appntmnt_id = " + hdrID;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteAppntmtDtLn(long Lnid, string critrNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Data Label = " + critrNm;
            string delSQL = "DELETE FROM hosp.appntmnt_data_captured WHERE data_capture_id = " + Lnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }


        public static long getAppntmtDataID(string dfltLabelName, string dataCtgry, long appntmtID)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = @"select a.data_capture_id from hosp.appntmnt_data_captured a 
         where lower(a.data_label) = '" + dfltLabelName.Replace("'", "''").ToLower() +
            "' and lower(a.data_catgry) ='" + dataCtgry.Replace("'", "''").ToLower() +
            "' and a.appntmnt_id = " + appntmtID;
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

        public static void createAppntmtData(long dataCaptureID, long appntmtID, long srvcTypDCID,
      string dataCtgry, string dataLabel, string dataVal, string dataType, string dataValDesc)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO hosp.appntmnt_data_captured(
            data_capture_id, appntmnt_id, srvc_data_capture_id, data_catgry, 
            data_label, data_value, data_type, data_val_desc, created_by, 
            creation_date, last_update_by, last_update_date) " +
                  "VALUES (" + dataCaptureID + ", " + appntmtID + ", " + srvcTypDCID +
                  ", '" + dataCtgry.Replace("'", "''") +
                  "', '" + dataLabel.Replace("'", "''") +
                  "', '" + dataVal.Replace("'", "''") +
                  "', '" + dataType.Replace("'", "''") +
                  "', '" + dataValDesc.Replace("'", "''") +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updateAppntmtData(long dataCaptureID, long appntmtID, long srvcTypDCID,
      string dataCtgry, string dataLabel, string dataVal, string dataType, string dataValDesc)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE hosp.appntmnt_data_captured SET 
             data_catgry = '" + dataCtgry.Replace("'", "''") +
                  "', data_label = '" + dataLabel.Replace("'", "''") +
                  "', data_type = '" + dataType.Replace("'", "''") +
                  "', last_update_by = " + Global.myVst.user_id +
                  ",  last_update_date = '" + dateStr +
                  "', data_value = '" + dataVal.Replace("'", "''") +
                  "', data_val_desc = '" + dataValDesc.Replace("'", "''") +
                  "' WHERE data_capture_id=" + dataCaptureID + "";
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
        }

        #endregion

        #region "SERVICE PROVIDERS..."
        public static DataSet get_SrvcPrvdrGrps(
       string searchWord, string searchIn,
      Int64 offset, int limit_size, int orgID)
        {/*Group Description
Group Name
Main Service Offered*/
            string strSql = "";
            string whereClause = "";

            if (searchIn == "Group Name")
            {
                whereClause = "(a.prvdr_grp_name ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Group Description")/*Decription have you seen  why it is not working?*/
            {
                whereClause = "(a.prvdr_grp_desc ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else /*if (searchIn == "Main Service Offered")*/
            {
                whereClause = "(b.type_name ilike '" + searchWord.Replace("'", "''") +
            "')";
            }

            strSql = "SELECT a.prvdr_grp_id, a.prvdr_grp_name, a.prvdr_grp_desc " +
         "FROM hosp.prvdr_grps a LEFT OUTER JOIN hosp.srvs_types b ON (a.main_srvc_type_id = b.type_id) " +
         "WHERE " + whereClause + " and a.org_id = " + orgID + " ORDER BY a.prvdr_grp_id DESC LIMIT " + limit_size +
         " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            Global.wfnSrvcPrvdFrm.rec_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_Ttl_SrvsPrvdrGrps(string searchWord, string searchIn, int orgID)
        {
            /*Group Description
      Group Name
      Main Service Offered*/
            string strSql = "";
            string whereClause = "";

            if (searchIn == "Group Name")
            {
                whereClause = "(a.prvdr_grp_name ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Group Description")/*Decription have you seen  why it is not working?*/
            {
                whereClause = "(a.prvdr_grp_desc ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else /*if (searchIn == "Main Service Offered")*/
            {
                whereClause = "(b.type_name ilike '" + searchWord.Replace("'", "''") +
            "')";
            }

            strSql = "SELECT count(1) " +
         "FROM hosp.prvdr_grps a LEFT OUTER JOIN hosp.srvs_types b ON (a.main_srvc_type_id = b.type_id) " +
         "WHERE " + whereClause + " and a.org_id = " + orgID;
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

        public static DataSet get_One_PrvdrGrpDt(int grphdrID)
        {
            string strSql = @"SELECT a.prvdr_grp_id, a.prvdr_grp_name, a.prvdr_grp_desc, 
    a.enabled_flag, a.main_srvc_type_id, b.type_name, a.max_daily_appntmnts, a.cur_days_appntmnts " +
         "FROM hosp.prvdr_grps a LEFT OUTER JOIN hosp.srvs_types b ON (a.main_srvc_type_id = b.type_id) " +
         "WHERE a.prvdr_grp_id =" + grphdrID + "";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_srvs_prvdrs(int grphdrID,
       string searchWord, string searchIn,
      Int64 offset, int limit_size)
        {
            string whereClause = "";
            /*
      Comments/Remarks
      Provider Name
      Provider Type*/
            if (searchIn == "Comments/Remarks")
            {
                whereClause = " and (a.comments_rmrks ilike '" + searchWord.Replace("'", "''") +
              "')";
            }
            else if (searchIn == "Provider Name")
            {
                whereClause = " and (prs.get_prsn_name(a.prsn_id) ilike '" + searchWord.Replace("'", "''") +
              "' or scm.get_cstmr_splr_name(a.cstmr_id) ilike '" + searchWord.Replace("'", "''") +
              "')";
            }
            else if (searchIn == "Provider Type")
            {
                whereClause = " and (a.provider_type ilike '" + searchWord.Replace("'", "''") +
              "')";
            }

            string strSql = @"SELECT provider_type, prvdr_id, prsn_id, cstmr_id, 
CASE WHEN a.prsn_id>0 THEN prs.get_prsn_name(a.prsn_id) 
ELSE scm.get_cstmr_splr_name(a.cstmr_id) END prvdr_name, srvs_type_id, 
to_char(to_timestamp(start_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
to_char(to_timestamp(end_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
comments_rmrks, max_daily_appntmnts, cur_days_appntmnts  
  FROM hosp.srvs_prvdrs a " +
         "WHERE prvdr_grp_id = " + grphdrID + whereClause + " ORDER BY prvdr_id  LIMIT " + limit_size +
         " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            Global.wfnSrvcPrvdFrm.recDt_SQL = strSql;
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_ttl_srvcprvdrs(int hdrID,
       string searchWord, string searchIn)
        {
            string whereClause = "";
            /*
      Comments/Remarks
      Provider Name
      Provider Type*/

            if (searchIn == "Comments/Remarks")
            {
                whereClause = " and (a.comments_rmrks ilike '" + searchWord.Replace("'", "''") +
            "')";
            }
            else if (searchIn == "Provider Name")
            {
                whereClause = " and (prs.get_prsn_name(a.prsn_id) ilike '" + searchWord.Replace("'", "''") +
              "' or scm.get_cstmr_splr_name(a.cstmr_id) ilike '" + searchWord.Replace("'", "''") +
              "')";
            }
            else if (searchIn == "Provider Type")
            {
                whereClause = " and (a.provider_type ilike '" + searchWord.Replace("'", "''") +
              "')";
            }

            string strSql = @"SELECT count(1) 
  FROM hosp.srvs_prvdrs a " +
         "WHERE prvdr_grp_id = " + hdrID + whereClause;

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

        public static string get_PrvdrGrps_Rec_Hstry(int hdrID)
        {
            string strSQL = @"SELECT a.created_by, 
to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
      a.last_update_by, 
      to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM hosp.prvdr_grps a WHERE(a.prvdr_grp_id = " + hdrID + ")";
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

        public static string get_SrvcPrvdrs_Rec_Hstry(int dteID)
        {
            string strSQL = @"SELECT a.created_by, 
to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
      a.last_update_by, 
      to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM hosp.srvs_prvdrs a WHERE(a.prvdr_id = " + dteID + ")";
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

        public static void deletePrvdrGrp(long hdrID, string grpNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Group Name = " + grpNm;
            string delSQL = "DELETE FROM hosp.srvs_prvdrs WHERE prvdr_grp_id = " + hdrID;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);

            delSQL = "DELETE FROM hosp.prvdr_grps WHERE prvdr_grp_id = " + hdrID;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static void deleteSrvsPrvdrLn(long Lnid, string critrNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Provider Name = " + critrNm;
            string delSQL = "DELETE FROM hosp.srvs_prvdrs WHERE prvdr_id = " + Lnid;
            Global.mnFrm.cmCde.deleteDataNoParams(delSQL);
        }

        public static bool isPrvdrGrpInUse(long hdrid)
        {
            string strSql = "SELECT a.appntmnt_id " +
             "FROM hosp.appntmnt a " +
             "WHERE(a.prvdr_grp_id = " + hdrid + ") ORDER BY 1 LIMIT 1 OFFSET 0";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        public static bool isSrvcPrvdrInUse(long srvcPrvdrid)
        {
            string strSql = "SELECT a.appntmnt_id " +
             "FROM hosp.appntmnt a " +
             "WHERE(a.srvs_prvdr_prsn_id = " + srvcPrvdrid + ") ORDER BY 1 LIMIT 1 OFFSET 0";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static int getPrvdrGrpID(string grpname, int orgid)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "select prvdr_grp_id from hosp.prvdr_grps where lower(prvdr_grp_name) = '" +
             grpname.Replace("'", "''").ToLower() + "' and org_id = " + orgid;
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

        public static string get_One_CstmrNm(long cstmrID)
        {
            string strSql = "SELECT a.cust_sup_name " +
             "FROM scm.scm_cstmr_suplr a " +
             "WHERE(a.cust_sup_id = " + cstmrID + ")";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            //Global.taxFrm.rec_SQL = strSql;
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return dtst.Tables[0].Rows[0][0].ToString();
            }
            return "";
        }

        public static long getSrvcsPrvdrID(long prsnID, long cstmrID, int prvdrGrpID)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = @"select a.prvdr_id from hosp.srvs_prvdrs a 
         where a.prsn_id = " + prsnID +
              " and a.cstmr_id = " + cstmrID +
              " and a.prvdr_grp_id = " + prvdrGrpID;
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

        public static void createSrvsPrvdrGrp(string grpName,
      string grpDesc, int srvsTypId, bool isEnbld, int orgID,
          int maxAllwdApntmnts)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"INSERT INTO hosp.prvdr_grps(
            prvdr_grp_name, prvdr_grp_desc, created_by, creation_date, last_update_by, 
            last_update_date, enabled_flag, main_srvc_type_id, 
            org_id, max_daily_appntmnts) " +
                  "VALUES ('" + grpName.Replace("'", "''") +
                  "', '" + grpDesc.Replace("'", "''") +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(isEnbld) +
                  "', " + srvsTypId +
                  ", " + orgID +
                  ", " + maxAllwdApntmnts +
                  ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updateSrvsPrvdrGrp(int prvdrGrpID, string grpName,
      string grpDesc, int srvsTypId, bool isEnbld,
          int maxAllwdApntmnts)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE hosp.prvdr_grps SET " +
                  "prvdr_grp_name='" + grpName.Replace("'", "''") +
                  "', prvdr_grp_desc ='" + grpDesc.Replace("'", "''") +
                  "', main_srvc_type_id = " + srvsTypId +
                  ", max_daily_appntmnts = " + maxAllwdApntmnts +
                  ", last_update_by=" + Global.myVst.user_id + ", " +
                  "last_update_date='" + dateStr +
                  "', enabled_flag='" +
                  Global.mnFrm.cmCde.cnvrtBoolToBitStr(isEnbld) +
                  "' WHERE (prvdr_grp_id =" + prvdrGrpID + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static long getNewSrvsPrvdrID()
        {
            //string strSql = "select nextval('accb.accb_trnsctn_batches_batch_id_seq'::regclass);";
            string strSql = "select nextval('hosp.srvs_prvdrs_prvdr_id_seq')";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static void createSrvsPrvdr(long prsnID,
          int srvsTypID, string strtDte, string endDte, int grpID,
          string prvdrTyp, long cstmrID, string rmrks, int mxAppnts)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            strtDte = DateTime.ParseExact(strtDte, "dd-MMM-yyyy HH:mm:ss",
      System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            endDte = DateTime.ParseExact(endDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string insSQL = @"INSERT INTO hosp.srvs_prvdrs(
            prsn_id, srvs_type_id, start_date, end_date, created_by, 
            creation_date, last_update_by, last_update_date, prvdr_grp_id, 
            provider_type, cstmr_id, comments_rmrks, max_daily_appntmnts) " +
                  "VALUES (" + prsnID + ", " + srvsTypID + ", '" + strtDte.Replace("'", "''") +
                  "', '" + endDte.Replace("'", "''") +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', " + grpID + ", '" + prvdrTyp.Replace("'", "''") +
                  "', " + cstmrID + ", '" + rmrks.Replace("'", "''") +
                  "'," + mxAppnts + ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updateSrvsPrvdr(long prvdrID, long prsnID,
          int srvsTypID, string strtDte, string endDte, int grpID,
          string prvdrTyp, long cstmrID, string rmrks, int mxAppnts)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            strtDte = DateTime.ParseExact(strtDte, "dd-MMM-yyyy HH:mm:ss",
      System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            endDte = DateTime.ParseExact(endDte, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");

            string insSQL = @"UPDATE hosp.srvs_prvdrs
       SET prsn_id=" + prsnID + ", srvs_type_id=" + srvsTypID +
                           ", start_date='" + strtDte.Replace("'", "''") +
                  "', end_date='" + endDte.Replace("'", "''") +
                  "', last_update_by=" + Global.myVst.user_id + ", last_update_date='" + dateStr +
                  "', prvdr_grp_id= " + grpID + ", provider_type='" + prvdrTyp.Replace("'", "''") +
                  "', cstmr_id=" + cstmrID + ", comments_rmrks='" + rmrks.Replace("'", "''") +
                  "', max_daily_appntmnts=" + mxAppnts + " WHERE prvdr_id=" + prvdrID + "";
            Global.mnFrm.cmCde.updateDataNoParams(insSQL);
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
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
                  "', '" + vldtyStatus.Replace("'", "''") +
                  "', " + orgnlBtchID +
                  ", " + orgid + ", " + spplrID +
                  ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updateBatchVldtyStatus(long batchid, string vldty)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_trnsctn_batches " +
            "SET batch_vldty_status='" + vldty.Replace("'", "''") +
            "', last_update_by=" + Global.myVst.user_id +
            ", last_update_date='" + dateStr +
            "' WHERE batch_id = " + batchid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updateBatchAvlblty(long batchid, string avlblty)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_trnsctn_batches " +
            "SET avlbl_for_postng='" + avlblty.Replace("'", "''") +
            "', last_update_by=" + Global.myVst.user_id +
            ", last_update_date='" + dateStr +
            "' WHERE batch_id = " + batchid;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void updtPymntsBatchVldty(long batchID, string vldtyStatus)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = @"UPDATE accb.accb_payments_batches SET 
            last_update_by=" + Global.myVst.user_id +
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
            last_update_by=" + Global.myVst.user_id +
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
                  "', last_update_by=" + Global.myVst.user_id +
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
          double funcCurrAmnt, double accntCurrAmnt)
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
            pymnt_batch_id) " +
                  "VALUES (" + pymntID + ", " + pymntMthdID + "," + amntPaid + "," + chnge_bals +
                  ",'" + pymntRemark.Replace("'", "''") +
                  "', '" + srcDocType.Replace("'", "''") +
                  "', " + srcDocID +
                  ", " + Global.myVst.user_id + ", '" + dateStr +
                  "', " + Global.myVst.user_id + ", '" + dateStr +
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
                  ")";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updtPymntDet(long pymntID, long pymntBatchID, int pymntMthdID,
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
                  ", last_update_by=" + Global.myVst.user_id +
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
        }

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
            //Global.pyblsFrm.docTmplt_SQL = strSql;
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
            //Global.pymntFrm.rec_SQL = strSql;
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
            //Global.pymntFrm.recDt_SQL = strSql;
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
                  "', last_update_by=" + Global.myVst.user_id +
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
                  ", last_update_by=" + Global.myVst.user_id +
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
        public static void updtPyblsDocAmntPaid(long docid,
      double amntPaid)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE accb.accb_pybls_invc_hdr SET " +
                  "amnt_paid=amnt_paid + " + amntPaid +
                  ", last_update_by=" + Global.myVst.user_id +
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
                  ", last_update_by=" + Global.myVst.user_id +
                  ", last_update_date='" + dateStr +
                  "' WHERE (pybls_invc_hdr_id = " +
                  docid + ")";
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        #endregion

        #region "GL INTERFACE..."
        public static DataSet getDocGLInfcLns(long docID, string srcDocType)
        {
            string strSql = "SELECT * FROM scm.scm_gl_interface WHERE src_doc_id = " +
              docID + " and src_doc_typ ilike '%" + srcDocType.Replace("'", "''") + "%' and gl_batch_id != -1";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet getDocGLInfcLns(long intrfcID)
        {
            string strSql = "SELECT * FROM scm.scm_gl_interface WHERE interface_id = " +
              intrfcID + "  and gl_batch_id != -1";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static long getIntFcTrnsDbtLn(long srcLnID, string srcDocType,
          double amount, int accntID, string trns_desc)
        {
            string strSql = "SELECT a.interface_id FROM scm.scm_gl_interface a " +
                    "WHERE a.src_doc_line_id = " + srcLnID +
              " and a.src_doc_typ = '" + srcDocType.Replace("'", "''") +
              "' and a.dbt_amount = " + amount + " and a.accnt_id = " + accntID +
              " and a.transaction_desc = '" + trns_desc.Replace("'", "''") + "'";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static long getIntFcTrnsCrdtLn(long srcLnID, string srcDocType,
          double amount, int accntID, string trns_desc)
        {
            string strSql = "SELECT a.interface_id FROM scm.scm_gl_interface a " +
               "WHERE a.src_doc_line_id = " + srcLnID +
         " and a.src_doc_typ = '" + srcDocType.Replace("'", "''") +
         "' and a.crdt_amount = " + amount + " and a.accnt_id = " + accntID +
         " and a.transaction_desc = '" + trns_desc.Replace("'", "''") + "'";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
        }

        public static long getPymntRcvdID(long srcDocID, string srcDocType, string dteRcvd, double amount)
        {
            //if (dteRcvd.Length > 11)
            //{
            //  dteRcvd = dteRcvd.Substring(0, 11);
            //}
            string strSql = "SELECT a.pymnt_id FROM scm.scm_payments a " +
               "WHERE a.src_doc_id = " + srcDocID +
         " and a.src_doc_typ = '" + srcDocType.Replace("'", "''") +
         "' and a.amount_paid = " + amount +
         " and to_char(to_timestamp(date_rcvd,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') ilike '%" + dteRcvd.Replace("'", "''") +
         "%'";
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return -1;
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

        public static double[] getGLIntrfcIDAmntSum(string intrfcids, int accntID)
        {
            double[] res = { 0, 0 };
            string strSql = @"SELECT COALESCE(SUM(a.dbt_amount),0), COALESCE(SUM(a.crdt_amount),0)
FROM scm.scm_gl_interface a
WHERE (a.accnt_id = " + accntID + @"
and '" + intrfcids + "' like '%,' || a.interface_id || ',%') ";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);

            if (dtst.Tables[0].Rows.Count > 0)
            {
                res[0] = double.Parse(dtst.Tables[0].Rows[0][0].ToString());
                res[1] = double.Parse(dtst.Tables[0].Rows[0][1].ToString());
            }
            return res;
        }

        public static bool isGLIntrfcBlcdOrg(int orgID, ref double dffrce)
        {
            string strSql = @"SELECT COALESCE(SUM(a.dbt_amount),0) dbt_sum, 
COALESCE(SUM(a.crdt_amount),0) crdt_sum 
FROM scm.scm_gl_interface a, accb.accb_chart_of_accnts b 
WHERE a.gl_batch_id = -1 and a.accnt_id = b.accnt_id and b.org_id=" + orgID +
            " ";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                dffrce = double.Parse(dtst.Tables[0].Rows[0][0].ToString()) -
            double.Parse(dtst.Tables[0].Rows[0][1].ToString());
                try
                {
                    if (dffrce == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public static void updtActnPrcss(int prcsID)
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

        public static bool isThereANActvActnPrcss(string prcsIDs, string prcsIntrvl)
        {
            string strSql = @"SELECT age(now(), to_timestamp(last_active_time,'YYYY-MM-DD HH24:MI:SS')) <= interval '" + prcsIntrvl +
              "' FROM accb.accb_running_prcses WHERE which_process_is_rnng IN (" + prcsIDs + ")";

            //Global.mnFrm.cmCde.showMsg(strSql, 0);
            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return bool.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            return false;
        }

        public static DataSet getAllInGLIntrfcOrg(int orgID)
        {
            string strSql = @"SELECT a.accnt_id, 
to_char(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS')
, SUM(a.dbt_amount) dbt_sum, " +
            "SUM(a.crdt_amount) crdt_sum, SUM(a.net_amount) net_sum, a.func_cur_id " +
            "FROM scm.scm_gl_interface a, accb.accb_chart_of_accnts b " +
            "WHERE a.gl_batch_id = -1 and a.accnt_id = b.accnt_id and b.org_id=" + orgID +
            " and NOT EXISTS(select f.transctn_id from accb.accb_trnsctn_details f " +
            "where f.batch_id IN (select g.batch_id from accb.accb_trnsctn_batches g " +
            "where g.batch_name ilike '%Inventory%' and " +
            "to_timestamp(g.creation_date,'YYYY-MM-DD HH24:MI:SS') between " +
            "(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') - interval '6 months') " +
            "and (to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') + interval '6 months')) " +
            "and f.source_trns_ids like '%,' || a.interface_id || ',%' and " +
            "f.trnsctn_date=a.trnsctn_date and f.accnt_id= a.accnt_id) " +
            "GROUP BY a.accnt_id, a.trnsctn_date, func_cur_id " +
            "ORDER BY to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS')";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            return dtst;
        }

        public static string getGLIntrfcIDs(int accntid, string trns_date, int crncy_id)
        {
            trns_date = DateTime.ParseExact(
         trns_date, "dd-MMM-yyyy HH:mm:ss",
         System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "select distinct a.interface_id from scm.scm_gl_interface a " +
                 "where a.accnt_id = " + accntid + " and a.trnsctn_date = '" + trns_date +
                 "' and a.func_cur_id = " + crncy_id + " and a.gl_batch_id = -1 and NOT EXISTS(select f.transctn_id from accb.accb_trnsctn_details f " +
                 "where f.batch_id IN (select g.batch_id from accb.accb_trnsctn_batches g " +
                 "where g.batch_name ilike '%Sales & Purchasing%' and " +
                 "to_timestamp(g.creation_date,'YYYY-MM-DD HH24:MI:SS') between " +
                 "(to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') - interval '6 months') " +
                 "and (to_timestamp(a.trnsctn_date,'YYYY-MM-DD HH24:MI:SS') + interval '6 months')) " +
                 "and f.source_trns_ids like '%,' || a.interface_id || ',%' and " +
                 "f.trnsctn_date=a.trnsctn_date and f.accnt_id= a.accnt_id) " +
                 "ORDER BY a.interface_id";

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            string infc_ids = ",";
            for (int a = 0; a < dtst.Tables[0].Rows.Count; a++)
            {
                infc_ids = infc_ids + dtst.Tables[0].Rows[a][0].ToString() + ",";
            }
            return infc_ids;
        }

        public static string get_GLBatch_Nm(long batchID)
        {
            string strSql = "";
            strSql = "SELECT a.batch_name " +
           "FROM accb.accb_trnsctn_batches a " +
           "WHERE(a.batch_id = " + batchID + ")";

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

        #endregion

        #region "ATTACHMENT DOCUMENTS..."
        public static long getP_AttchmntID(string attchname, long hdrID)
        {
            string strSql = "";
            strSql = "SELECT a.attchmnt_id " +
         "FROM scm.scm_prchs_doc_attchmnts a " +
            "WHERE ((a.attchmnt_desc = '" + attchname.Replace("'", "''") +
              "') AND (a.doc_hdr_id = " + hdrID + "))";

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

        public static void createP_Attachment(long hdrID, string attchDesc,
         string filNm)
        {
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string insSQL = "INSERT INTO scm.scm_prchs_doc_attchmnts(" +
                  "doc_hdr_id, attchmnt_desc, file_name, created_by, " +
                  "creation_date, last_update_by, last_update_date) " +
                              "VALUES (" + hdrID +
                              ", '" + attchDesc.Replace("'", "''") +
                              "', '" + filNm.Replace("'", "''") +
                              "', " + Global.myVst.user_id + ", '" + dateStr +
                              "', " + Global.myVst.user_id + ", '" + dateStr + "')";
            Global.mnFrm.cmCde.insertDataNoParams(insSQL);
        }

        public static void updateP_Attachment(long attchID, long hdrID, string attchDesc,
       string filNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
            string updtSQL = "UPDATE scm.scm_prchs_doc_attchmnts SET " +
                  "doc_hdr_id=" + hdrID +
                              ", attchmnt_desc='" + attchDesc.Replace("'", "''") +
                              "', file_name='" + filNm.Replace("'", "''") +
                              "', last_update_by=" + Global.myVst.user_id +
                              ", last_update_date='" + dateStr + "' " +
                               "WHERE attchmnt_id = " + attchID;
            Global.mnFrm.cmCde.updateDataNoParams(updtSQL);
        }

        public static void deleteP_Attchmnt(long attchid, string attchNm)
        {
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "Attachment Name = " + attchNm;
            string delSql = "DELETE FROM scm.scm_prchs_doc_attchmnts WHERE(attchmnt_id = " + attchid + ")";
            Global.mnFrm.cmCde.deleteDataNoParams(delSql);
        }


        public static DataSet get_P_Attachments(string searchWord, string searchIn,
       Int64 offset, int limit_size, long hdrID, ref string attchSQL)
        {
            string strSql = "";
            if (searchIn == "Attachment Name/Description")
            {
                strSql = "SELECT a.attchmnt_id, a.doc_hdr_id, a.attchmnt_desc, a.file_name " +
              "FROM scm.scm_prchs_doc_attchmnts a " +
              "WHERE(a.attchmnt_desc ilike '" + searchWord.Replace("'", "''") +
              "' and a.doc_hdr_id = " + hdrID + ") ORDER BY a.attchmnt_id LIMIT " + limit_size +
                  " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            }

            DataSet dtst = Global.mnFrm.cmCde.selectDataNoParams(strSql);
            attchSQL = strSql;
            return dtst;
        }

        public static long get_Total_P_Attachments(string searchWord,
          string searchIn, long hdrID)
        {
            string strSql = "";
            if (searchIn == "Attachment Name/Description")
            {
                strSql = "SELECT COUNT(1) " +
              "FROM scm.scm_prchs_doc_attchmnts a " +
              "WHERE(a.attchmnt_desc ilike '" + searchWord.Replace("'", "''") +
              "' and a.doc_hdr_id = " + hdrID + ")";
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

        #endregion

        #region "CUSTOM FUNCTIONS..."
        #region "MISC..."
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

        public static void refreshRqrdVrbls()
        {
            Global.mnFrm.cmCde.DefaultPrvldgs = Global.dfltPrvldgs;
            //Global.mnFrm.cmCde.Login_number = Global.myInv.login_number;
            Global.mnFrm.cmCde.ModuleAdtTbl = Global.myVst.full_audit_trail_tbl_name;
            Global.mnFrm.cmCde.ModuleDesc = Global.myVst.mdl_description;
            Global.mnFrm.cmCde.ModuleName = Global.myVst.name;
            //Global.mnFrm.cmCde.pgSqlConn = Global.myInv.Host.globalSQLConn;
            //Global.mnFrm.cmCde.Role_Set_IDs = Global.myInv.role_set_id;
            Global.mnFrm.cmCde.SampleRole = "Visits and Appointments Administrator";
            //Global.mnFrm.cmCde.User_id = Global.myInv.user_id;
            //Global.mnFrm.cmCde.Org_id = Global.myInv.org_id;
            Global.mnFrm.cmCde.Extra_Adt_Trl_Info = "";
            Global.myVst.user_id = Global.mnFrm.usr_id;
            Global.myVst.login_number = Global.mnFrm.lgn_num;
            Global.myVst.role_set_id = Global.mnFrm.role_st_id;
            Global.myVst.org_id = Global.mnFrm.Og_id;
        }

        public static void createRqrdLOVs()
        {
            string[] sysLovs = { "System Codes for Appointment Services", "Appointment Data Capture Category",
                         "Appointment Services Offered", "Available Provider Groups", "Available Service Providers",
                             "Non-Dynamic LOV Names"};
            string[] sysLovsDesc = { "System Codes for Appointment Services", "Appointment Data Capture Category",
                         "Appointment Services Offered", "Available Provider Groups", "Available Service Providers",
                             "Non-Dynamic LOV Names"};
            string[] sysLovsDynQrys = { "", "",
                                  "select distinct '' || type_id a, type_name b, '' c, org_id d, sys_code e from hosp.srvs_types where (is_enabled = '1') order by type_name",
                                  "select distinct '' || prvdr_grp_id a, prvdr_grp_name b, '' c, org_id d from hosp.prvdr_grps where (enabled_flag = '1') order by prvdr_grp_name",
                                  "select distinct '' || prvdr_id a, (CASE WHEN prsn_id>0 THEN prs.get_prsn_name(prsn_id) ELSE scm.get_cstmr_splr_name(cstmr_id) END) b, '' c, prvdr_grp_id d, start_date e, end_date f from hosp.srvs_prvdrs order by 2",
                                  "select distinct '' || value_list_id a, value_list_name b, '' c from gst.gen_stp_lov_names where is_list_dynamic != '1' order by value_list_name"};
            string[] pssblVals = {
        "0","01-Consultation","01-Consultation",
        "0","02-Investigations","02-Investigations",
        "0","03-Vitals","03-Vitals",
        "0","04-Pharmacy","04-Pharmacy",
        "0","05-Front Desk","05-Front Desk",
        "0","07-Injection","07-Injection",
        "0","06-Dressing Room","06-Dressing Room",
        "0","08-In-Patient Service","08-In-Patient Service",
        "0","09-Physiotherapy","09-Physiotherapy",
        "0","10-Pre-Sewing Measurements","10-Pre-Sewing Measurements",
        "0","11-Post-Sewing Fitting","11-Post-Sewing Fitting",
        "0","12-Item Pick-Up","12-Item Pick-Up",
        "1","Vital Statistics","Vital Statistics"};

            Global.mnFrm.cmCde.createSysLovs(sysLovs, sysLovsDynQrys, sysLovsDesc);
            Global.mnFrm.cmCde.createSysLovsPssblVals(sysLovs, pssblVals);
        }

        #endregion
        #endregion
    }
}

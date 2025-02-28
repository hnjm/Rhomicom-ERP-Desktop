using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GeneralSetup.Forms;
using System.Windows.Forms;
using CommonCode;
namespace GeneralSetup.Classes
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
        public static GeneralSetup myGenStp = new GeneralSetup();
        public static mainForm myNwMainFrm = null;
        public static string[] dfltPrvldgs = { "View General Setup", "View Value List Names"
        , "View possible values", /*3*/"Add Value List Names", "Edit Value List Names"
        , "Delete Value List Names", /*6*/"Add Possible Values", "Edit Possible Values"
        , "Delete Possible Values", "View Record History", "View SQL"};
        public static string[] sysLovs = { "Benefits Types", "Relationship Types"
        , "Person Types-Further Details", "Countries", "Currencies", "Organisation Types"
        , "Divisions or Group Types", "Person Type Change Reasons", "Person Types"
        , "Qualification Types", "National ID Types", "Pay Frequencies",
    "Benefits & Dues/Contributions Value Types", "Extra Information Labels",
    "Divisions Images Directory","Organization Images Directory","Person Images Directory"
   ,"Organisations","Divisions/Groups","Jobs","Chart of Accounts",
        "Transaction Accounts","Parent Accounts","Active Users","Person Titles",
        "Gender","Marital Status", "Nationalities", "Active Persons","Sites/Locations",
        "Grades","Positions","Asset Accounts","Expense Accounts","Revenue Accounts",
        "Liability Accounts","Equity Accounts","Pay Items","Pay Item Values",
        "Working Hours","Gathering Types","Organisational Pay Scale",
        "Transactions Date Limit 1","Transactions Date Limit 2",
    "Budget Accounts","Banks","Bank Branches","Bank Account Types","Balance Items",
      "Non-Balance Items","Person Sets for Payments","Item Sets for Payments",
    "Audit Logs Directory",/*53*/"Reports Directory","System Modules",
      "LOV Names","User Roles","Pay Item Classifications"};
        public static string[] sysLovsDesc = {"Benefits Types", "Relationship Types"
        , "Further Details about the available person types", "Countries", "Currencies", "Organisation Types"
        , "Divisions or Group Types", "Person Type Change Reasons", "Person Types"
        , "Qualification Types", "National ID Types", "Pay Frequencies",
  "Benefits & Dues/Contributions Value Types", "Extra Information Labels",
  "Directory for keeping images from the div_groups_table",
  "Directory for keeping images coming from the org_details_table",
  "Directory for Storing Person's Images",
  "List of all organizations stored in the system",
  "List of all divisions/groups stored in the system",
        "List of all Jobs stored in the system",
        "List of all Accounts stored in the system",
        "List of all accounts transactions can be posted into",
        "List of all Parent Accounts in the system",
        "List of all users in the system",
        "Name Titles of Organization Persons", "Gender",
  "Marital Status","Nationalities","Active Persons",
  "List of all Sites/Locations","List of all Grades",
        "List of all Positions","Asset Accounts","Expense Accounts",
        "Revenue Accounts","Liability Accounts","Equity Accounts",
        "Pay Items","Pay Item Values","Working Hours","Gathering Types",
        "Organisational Pay Scale","Transactions Date Limit 1" ,
        "Transactions Date Limit 2","Budget Accounts","Banks",
            "Bank Branches","Bank Account Types","Balance Items",
      "Non-Balance Items","Person Sets for Payments",
      "Item Sets for Payments",
    "Audit Logs Directory","Reports Directory","System Modules","LOV Names","User Roles","Pay Item Classifications"};
        public static string[] sysLovsDynQrys = {"", ""
        , "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
  "select distinct trim(to_char(org_id,'999999999999999999999999999999')) a, org_name b, '' c from org.org_details order by 2",
  "select distinct trim(to_char(div_id,'999999999999999999999999999999')) a, div_code_name b, '' c, org_id d from org.org_divs_groups order by 2",
        "select distinct trim(to_char(job_id,'999999999999999999999999999999')) a, job_code_name b, '' c, org_id d from org.org_jobs order by 2",
        "select distinct trim(to_char(accnt_id,'999999999999999999999999999999')) a, accnt_num || '.' || accnt_name b, '' c, org_id d, accnt_num e from accb.accb_chart_of_accnts order by accnt_num",
        "select distinct trim(to_char(accnt_id,'999999999999999999999999999999')) a, accnt_num || '.' || accnt_name b, '' c, org_id d, accnt_num e from accb.accb_chart_of_accnts where (is_prnt_accnt = '0' and is_enabled = '1' and  is_retained_earnings= '0' and is_net_income = '0' and has_sub_ledgers = '0') order by accnt_num",
        "select distinct trim(to_char(accnt_id,'999999999999999999999999999999')) a, accnt_num || '.' || accnt_name b, '' c, org_id d, accnt_type e, accnt_num f from accb.accb_chart_of_accnts where (is_prnt_accnt = '1') order by accnt_num",
        "select distinct trim(to_char(user_id,'999999999999999999999999999999')) a, user_name b, '' c FROM sec.sec_users WHERE (now() between to_timestamp(valid_start_date,'YYYY-MM-DD HH24:MI:SS') AND "+
  "to_timestamp(valid_end_date,'YYYY-MM-DD HH24:MI:SS')) order by 1","","","","",
  "SELECT distinct local_id_no a, trim(title || ' ' || sur_name || "+
        "', ' || first_name || ' ' || other_names) b, '' c, org_id d " +
        "FROM prs.prsn_names_nos a order by local_id_no DESC",
  "select distinct trim(to_char(location_id,'999999999999999999999999999999')) a, location_code_name b, '' c, org_id d from org.org_sites_locations order by 2",
  "select distinct trim(to_char(grade_id,'999999999999999999999999999999')) a, grade_code_name b, '' c, org_id d from org.org_grades order by 2",
        "select distinct trim(to_char(position_id,'999999999999999999999999999999')) a, position_code_name b, '' c, org_id d from org.org_positions order by 2",
  "select distinct trim(to_char(accnt_id,'999999999999999999999999999999')) a, accnt_num || '.' || accnt_name b, '' c, org_id d, accnt_num e from accb.accb_chart_of_accnts where (accnt_type = 'A' and is_prnt_accnt = '0' and is_enabled = '1' and  is_retained_earnings= '0' and is_net_income = '0' and has_sub_ledgers = '0') order by accnt_num",
  "select distinct trim(to_char(accnt_id,'999999999999999999999999999999')) a, accnt_num || '.' || accnt_name b, '' c, org_id d, accnt_num e from accb.accb_chart_of_accnts where (accnt_type = 'EX' and is_prnt_accnt = '0' and is_enabled = '1' and  is_retained_earnings= '0' and is_net_income = '0' and has_sub_ledgers = '0') order by accnt_num",
  "select distinct trim(to_char(accnt_id,'999999999999999999999999999999')) a, accnt_num || '.' || accnt_name b, '' c, org_id d, accnt_num e from accb.accb_chart_of_accnts where (accnt_type = 'R' and is_prnt_accnt = '0' and is_enabled = '1' and  is_retained_earnings= '0' and is_net_income = '0' and has_sub_ledgers = '0') order by accnt_num",
  "select distinct trim(to_char(accnt_id,'999999999999999999999999999999')) a, accnt_num || '.' || accnt_name b, '' c, org_id d, accnt_num e from accb.accb_chart_of_accnts where (accnt_type = 'L' and is_prnt_accnt = '0' and is_enabled = '1' and  is_retained_earnings= '0' and is_net_income = '0' and has_sub_ledgers = '0') order by accnt_num",
  "select distinct trim(to_char(accnt_id,'999999999999999999999999999999')) a, accnt_num || '.' || accnt_name b, '' c, org_id d, accnt_num e from accb.accb_chart_of_accnts where (accnt_type = 'EQ' and is_prnt_accnt = '0' and is_enabled = '1' and  is_retained_earnings= '0' and is_net_income = '0' and has_sub_ledgers = '0') order by accnt_num",
  "select distinct trim(to_char(item_id,'999999999999999999999999999999')) a, item_code_name b, '' c, org_id d from org.org_pay_items order by 2",
  "select distinct trim(to_char(pssbl_value_id,'999999999999999999999999999999')) a, pssbl_value_code_name b, '' c, item_id d from org.org_pay_items_values order by 2",
        "select distinct trim(to_char(work_hours_id,'999999999999999999999999999999')) a, work_hours_name b, '' c, org_id d from org.org_wrkn_hrs order by 2",
        "select distinct trim(to_char(gthrng_typ_id,'999999999999999999999999999999')) a, gthrng_typ_name b, '' c, org_id d from org.org_gthrng_types order by 2",
  "","","",
        "select distinct trim(to_char(accnt_id,'999999999999999999999999999999')) a, accnt_num || '.' || accnt_name b, '' c, org_id d, accnt_num e from accb.accb_chart_of_accnts where ((accnt_type = 'R' or accnt_type = 'EX') and is_prnt_accnt = '0' and is_enabled = '1' and has_sub_ledgers = '0') order by accnt_num","","","",
  "select distinct trim(to_char(item_id,'999999999999999999999999999999')) a, item_code_name b, '' c, org_id d from org.org_pay_items where item_maj_type = 'Balance Item' order by item_code_name",
  "select distinct trim(to_char(item_id,'999999999999999999999999999999')) a, item_code_name b, '' c, org_id d, pay_run_priority e from org.org_pay_items where item_maj_type = 'Pay Value Item' order by pay_run_priority",
  "select distinct trim(to_char(prsn_set_hdr_id,'999999999999999999999999999999')) a, prsn_set_hdr_name b, '' c, org_id d from pay.pay_prsn_sets_hdr order by prsn_set_hdr_name",
  "select distinct trim(to_char(hdr_id,'999999999999999999999999999999')) a, itm_set_name b, '' c, org_id d from pay.pay_itm_sets_hdr order by itm_set_name"
  ,"","", "select distinct trim(to_char(module_id,'999999999999999999999999999999')) a, module_name b, '' c from sec.sec_modules order by module_name"
  , "select distinct trim(to_char(value_list_id,'999999999999999999999999999999')) a, value_list_name b, '' c from gst.gen_stp_lov_names order by value_list_name"
  , "select distinct trim(to_char(role_id,'999999999999999999999999999999')) a, role_name b, '' c from sec.sec_roles order by role_name",""};

        public static string[] pssblVals = { "0", "Loans",
        "Money amounts granted to staff to be paid later"
        ,"0", "Allowances", "Money amounts granted to staff"
        ,"0", "Leave", "Vacation Days allowed for employees"
        ,"1", "Father", "Biological Male Parent"
        ,"1", "Mother", "Biological Female Parent"
        ,"1", "Spouse", "Husband or Wife"
        ,"1", "Ex-Spouse", "Former Husband or wife"
        ,"1", "Son", "Biological Male Child"
        ,"1", "Daughter", "Biological Female Child"
        ,"1", "Uncle", "Uncle"
        ,"1", "Aunt", "Aunt"
        ,"1", "Nephew", "Nephew"
        ,"1", "Niece", "Niece"
        ,"1", "In-Law", "In-Law"
        ,"1", "Cousin", "Cousin"
        ,"1", "Friend", "Friend"
        ,"1", "Guardian", "Guardian"
        ,"1", "Grand-Father", "Grand-Father"
        ,"1", "Grand-Mother", "Grand-Mother"
        ,"1", "Step-Father", "Step-Father"
        ,"1", "Step-Mother", "Step-Mother"
        ,"1", "Step-Son", "Step-Son"
        ,"1", "Step-Daughter", "Step-Daughter"
        ,"2", "Permanent-Full Time", "Full Time permanent staff"
        ,"2", "Permanent-Part Time", "Part Time permanent staff"
        ,"2", "Contract-Full Time", "Full Time contract staff"
        ,"2", "Contract-Part Time", "Part Time contract staff"
        ,"3", "Ghana", "GH"
        ,"3", "South Africa", "SA"
        ,"3", "United States of America", "USA"
        ,"3", "United Kingdom", "UK"
        ,"4", "GHS", "Ghana Cedis ₵"
        ,"4", "JPY", "Japanese Yen ¥"
        ,"4", "USD", "US Dollars $"
        ,"4", "GBP", "British Pound £"
        ,"5", "School", "Place of tution and learning"
        ,"5", "Hotel", "Place where rooms are hired out to the public"
        ,"5", "Church", "Place of Worship"
        ,"5", "NGO", "Non-Governmental Organization"
        ,"5", "Company", "Company"
     ,"6", "Office", "Major Division under Department"
        ,"6", "Unit", "Division under Office"
        ,"6", "Department", "Major Division in an Organization"
        ,"6", "Wing", "Typically in churchs"
        ,"6", "Club", "Association"
        ,"6", "Association", "Welfare Group"
        ,"6", "Religious Denomination", "Religious group"
        ,"6", "Team", "Group for competitions"
    ,"6", "Shareholders", "Group for Shareholders"
    ,"6", "Board of Directors", "Group for Board of Directors"
    ,"6", "Pay/Remuneration", "Group for Workers' Salaries/Wages"
    ,"6", "Top Management", "Group for Top Management"
        ,"7", "New Shareholder", "New Shareholder"
        ,"7", "Starting Director/Shareholder", "Starting Director/Shareholder"
        ,"7", "New Recruitment", "New staff"
        ,"7", "Re-Employment", "Old staff coming back"
        ,"7", "New Enrolment", "New Member"
        ,"7", "Re-Enrolment", "Old Member coming back"
        ,"7", "End of Contract", "Contract has ended duely"
        ,"7", "Appointment as Board Member", "Appointment as Board Member"
        ,"7", "Termination of Appointment", "Appointment Terminated"
        ,"7", "Dismissal", "Sacked"
        ,"7", "Compulsory Retirement", "Reached age Limit"
        ,"7", "Voluntary Retirement", "Decided to retire early"
        ,"7", "Retirement on Medical Grounds", "Retiring due to Ailment"
        ,"7", "Change of Membership Terms", "Change of Membership Terms"
        ,"7", "Change of Employement Terms", "Change of Employement Terms"
        ,"8", "Shareholder", "Owner of Shares in the Company"
        ,"8", "Board Member", "Member of Board of Directors"
        ,"8", "Contact Person", "Relative or Friend"
        ,"8", "Ex-Contact Person", "Former Relative or Friend"
        ,"8", "Customer", "Client"
        ,"8", "Ex-Customer", "Former Client"
        ,"8", "Supplier", "Supplier of goods and services"
        ,"8", "Ex-Supplier", "Former Supplier of goods and services"
        ,"8", "Ex-Customer", "Former Client"
        ,"8", "Student", "Currently a Student"
        ,"8", "Old Student", "Former Student"
        ,"8", "Employee", "Currently a worker"
        ,"8", "Ex-Employee", "Former Worker"
        ,"8", "Member", "Currently a Member of the group"
        ,"8", "Ex-Member", "A Former Member of the group"
        ,"9", "1st Degree", "First Degree University"
        ,"9", "2nd Degree", "Second Degree University"
        ,"9", "WASSCE", "Senior High School Cert."
        ,"9", "BECE", "Junior High School Cert"
        ,"9", "Phd", "Doctor of Philosophy"
        ,"10", "NHIS ID", "Health Insurance"
        ,"10", "Voter's ID", "Voter's ID Card"
        ,"10", "Driving License", "Driver's License"
        ,"10", "Passport", "Passport"
        ,"10", "SSNIT", "SSNIT"//, , , , , , , , 
	 ,"11", "fixed", "for payments at end of contracts"
     ,"11", "hourly", "hourly"
     ,"11", "daily", "daily"
     ,"11", "weekly", "weekly"
     ,"11", "fortnightly", "fortnightly"
     ,"11", "semi-month", "semi-month"
     ,"11", "month", "month"
     ,"11", "yearly", "yearly"
        ,"11", "quaterly", "quaterly"
        ,"12", "Money", "Money"
     ,"12", "Items", "Items"
     ,"12", "Service", "Service"
     ,"12", "Working Days", "Working Days"
     ,"13", "Motto", "Motto of a Group/Division"
     ,"13", "Mission", "Mission of a Group/Division"
     ,"13", "Vision", "Vision of a Group/Division"
   //,"14", Application.StartupPath + @"\Images\Divs", "Divisions Logos Directory"
   //,"15", Application.StartupPath + @"\Images\Org", "Organizations Logos Directory"
   //,"16", Application.StartupPath + @"\Images\Person", "Persons Images Directory"
		,"24","Mr.","Mr."
        ,"24","Mrs.","Mrs."
        ,"24","Master","Master"
        ,"24","Ms.","Ms."
        ,"24","Miss.","Miss."
        ,"24","Dr.","Dr."
        ,"24","Prof.","Prof."
        ,"25","Male","Male"
        ,"25","Female","Female"
          ,"25","Not Applicable","Not Applicable"
,"26","Single","Single"
        ,"26","Married","Married"
        ,"26","Divorced","Divorced"
        ,"26","Separated","Separated"
    ,"26","Widow","Widow"
    ,"26","Widower","Widower"
        ,"27","Ghanaian","Ghanaian"
        ,"27","American","American"
        ,"27","British","British"
        ,"27","Togolese","Togolese"
    ,"41", "2400","9999.Rhomicom Basic Worker Grade.P1"
    ,"41", "3000","9999.Rhomicom Basic Worker Grade.P2"
        ,"42", "01-JAN-1900 00:00:00","01-JAN-1900"
        ,"43","31-DEC-4000 23:59:59","31-DEC-4000"
  ,"45","Bank of Ghana","Bank of Ghana"
  ,"45","Barclays Bank","Barclays Bank"
  ,"45","Standard Chartered Bank","Standard Chartered Bank"
  ,"45","Ghana Commercial Bank","Ghana Commercial Bank"
  ,"45","Prudential Bank","Prudential Bank"
  ,"46","Accra Branch","Accra Branch"
  ,"46","Makola Branch","Makola Branch"
  ,"46","Ring Road Branch","Ring Road Branch"
  ,"46","Kaneshie Branch","Kaneshie Branch"
  ,"46","KNUST Branch","KNUST Branch"
  ,"47","Current Account","Kaneshie Branch"
  ,"47","Savings Account","KNUST Branch"
  //,"52", Application.StartupPath + @"\Images\Logs", "Audit Logs Directory"
  //,"53", Application.StartupPath + @"\Images\Rpts", "Reports Directory"
  ,"57","Payslip Item","Payslip Items-Items that appear on Payslip after during payroll run"
  ,"57","Payroll Item","Payroll Items-Items Run during payroll run but don't appear on Payslip"
  ,"57","Bill Item","Bill Items Eg. School Fees Bill Items"
};//"",""
        #endregion

        #region "DATA MANIPULATION FUNCTIONS..."
        #region "INSERT STATEMENTS..."
        public static void createLovNm(string lovNm, string lovDesc, bool isDyn
          , string sqlQry, string dfndBy, bool isEnbld, string ordrBy)
        {
            string dateStr = Global.myNwMainFrm.cmmnCodeGstp.getDB_Date_time();
            string sqlStr = "INSERT INTO gst.gen_stp_lov_names(" +
                  "value_list_name, value_list_desc, is_list_dynamic, " +
                  "sqlquery_if_dyn, defined_by, created_by, creation_date, last_update_by, " +
                  "last_update_date, is_enabled, dflt_order_by) " +
              "VALUES ('" + lovNm.Replace("'", "''") + "', '" + lovDesc.Replace("'", "''") +
          "', '" + Global.myNwMainFrm.cmmnCodeGstp.cnvrtBoolToBitStr(isDyn) + "', '" + sqlQry.Replace("'", "''") + "', '" + dfndBy.Replace("'", "''") +
              "', " + Global.myGenStp.user_id + ", '" + dateStr + "', " + Global.myGenStp.user_id +
              ", '" + dateStr + "', '" + Global.myNwMainFrm.cmmnCodeGstp.cnvrtBoolToBitStr(isEnbld) + "', '" + ordrBy.Replace("'", "''") + "')";
            Global.myNwMainFrm.cmmnCodeGstp.insertDataNoParams(sqlStr);
        }

        public static void createPssblValsForLov(int lovID, string pssblVal,
          string pssblValDesc, bool isEnbld, string allwd)
        {
            string dateStr = Global.myNwMainFrm.cmmnCodeGstp.getDB_Date_time();
            string sqlStr = "INSERT INTO gst.gen_stp_lov_values(" +
                  "value_list_id, pssbl_value, pssbl_value_desc, " +
                              "created_by, creation_date, last_update_by, last_update_date, is_enabled, allowed_org_ids) " +
              "VALUES (" + lovID + ", '" + pssblVal.Replace("'", "''") + "', '" + pssblValDesc.Replace("'", "''") +
              "', " + Global.myGenStp.user_id + ", '" + dateStr + "', " + Global.myGenStp.user_id +
              ", '" + dateStr + "', '" +
              Global.myNwMainFrm.cmmnCodeGstp.cnvrtBoolToBitStr(isEnbld) +
              "', '" + allwd.Replace("'", "''") + "')";
            Global.myNwMainFrm.cmmnCodeGstp.insertDataNoParams(sqlStr);
        }
        #endregion

        #region "UPDATE STATEMENTS..."
        public static void updateLovNm(int lovID, string lovNm, string lovDesc, bool isDyn
      , string sqlQry, string dfndBy, bool isEnbld, string ordrBy)
        {
            Global.myNwMainFrm.cmmnCodeGstp.Extra_Adt_Trl_Info = "";
            string dateStr = Global.myNwMainFrm.cmmnCodeGstp.getDB_Date_time();
            string sqlStr = "UPDATE gst.gen_stp_lov_names SET " +
            "value_list_name = '" + lovNm.Replace("'", "''") + "', value_list_desc = '" + lovDesc.Replace("'", "''") +
            "', is_list_dynamic = '" + Global.myNwMainFrm.cmmnCodeGstp.cnvrtBoolToBitStr(isDyn) + "', sqlquery_if_dyn = '" + sqlQry.Replace("'", "''") +
            "', defined_by = '" + dfndBy.Replace("'", "''") + "', last_update_by = " + Global.myGenStp.user_id + ", " +
            "last_update_date = '" + dateStr + "', is_enabled = '" + Global.myNwMainFrm.cmmnCodeGstp.cnvrtBoolToBitStr(isEnbld) +
            "', dflt_order_by='" + ordrBy.Replace("'", "''") + "' WHERE(value_list_id = " + lovID + ")";
            Global.myNwMainFrm.cmmnCodeGstp.updateDataNoParams(sqlStr);
        }

        public static void updatePssblValsForLov(int pssblVlID, string pssblVal,
      string pssblValDesc, bool isEnbld, string allwd)
        {
            Global.myNwMainFrm.cmmnCodeGstp.Extra_Adt_Trl_Info = "";
            string dateStr = Global.myNwMainFrm.cmmnCodeGstp.getDB_Date_time();
            string sqlStr = "UPDATE gst.gen_stp_lov_values SET " +
            "pssbl_value = '" + pssblVal.Replace("'", "''") +
            "', pssbl_value_desc = '" + pssblValDesc.Replace("'", "''") + "', " +
            "last_update_by = " + Global.myGenStp.user_id +
            ", last_update_date = '" + dateStr +
            "', is_enabled = '" + Global.myNwMainFrm.cmmnCodeGstp.cnvrtBoolToBitStr(isEnbld) + "', " +
            "allowed_org_ids ='" + allwd.Replace("'", "''") + "' " +
            "WHERE(pssbl_value_id = " + pssblVlID + ")";
            Global.myNwMainFrm.cmmnCodeGstp.updateDataNoParams(sqlStr);
        }

        #endregion

        #region "DELETE STATEMENTS..."
        public static void deleteLovNm(int lovID)
        {
            Global.myNwMainFrm.cmmnCodeGstp.Extra_Adt_Trl_Info = "--LOV name was " + Global.getLovName(lovID) + "--\r\n";
            string sqlStr = "DELETE FROM gst.gen_stp_lov_names WHERE(value_list_id = " + lovID + ")";
            Global.myNwMainFrm.cmmnCodeGstp.deleteDataNoParams(sqlStr);
            sqlStr = "DELETE FROM gst.gen_stp_lov_values WHERE(value_list_id = " + lovID + ")";
            Global.myNwMainFrm.cmmnCodeGstp.deleteDataNoParams(sqlStr);
        }

        public static void deleteLPssblVl(int pssblVlID)
        {
            Global.myNwMainFrm.cmmnCodeGstp.Extra_Adt_Trl_Info = "--Possible Value was " + Global.getPssblValNm(pssblVlID) + "--\r\n";
            string sqlStr = "DELETE FROM gst.gen_stp_lov_values WHERE(pssbl_value_id = " + pssblVlID + ")";
            Global.myNwMainFrm.cmmnCodeGstp.deleteDataNoParams(sqlStr);
        }
        #endregion

        #region "SELECT STATEMENTS..."
        #region "GENERAL..."
        #endregion

        #region "VALUE LIST NAMES..."
        public static DataSet get_Basic_VlNmInfo(string searchWord, string searchIn,
          Int64 offset, int limit_size)
        {
            string[] whereClsFrmts = { "defined_by", "is_list_dynamic", "is_enabled",
            "sqlquery_if_dyn", "value_list_desc","value_list_name"};
            string[] ordrByClsFrmts = { "defined_by", "is_list_dynamic", "is_enabled",
            "sqlquery_if_dyn", "value_list_desc", "value_list_name" };
            string[] sortOrder = { "ASC", "ASC", "ASC", "ASC", "ASC", "ASC" };
            int frmt_to_use = 0;
            string strSql = "";
            string optional_str = "";
            if (searchIn == "Defined By")
            {
                frmt_to_use = 0;
            }
            else if (searchIn == "Is Dynamic")
            {
                frmt_to_use = 1;
            }
            else if (searchIn == "Is Enabled")
            {
                frmt_to_use = 2;
            }
            else if (searchIn == "SQL Query")
            {
                frmt_to_use = 3;
            }
            else if (searchIn == "Value List Description")
            {
                frmt_to_use = 4;
            }
            else if (searchIn == "Value List Name")
            {
                frmt_to_use = 5;
            }

            if (searchWord == "")
            {
                optional_str = " OR (" + ordrByClsFrmts[frmt_to_use] + " IS NULL)";
            }

            strSql = "SELECT value_list_name, value_list_id FROM gst.gen_stp_lov_names " +
         "WHERE ((" + whereClsFrmts[frmt_to_use] + " ilike '" + searchWord.Replace("'", "''") +
         "')" + optional_str + ") ORDER BY value_list_id DESC" + " LIMIT " + limit_size +
         " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            /*+ ordrByClsFrmts[frmt_to_use] + " " +
         sortOrder[frmt_to_use]*/
            Global.myNwMainFrm.VlNm_SQL = strSql;
            DataSet dtst = Global.myNwMainFrm.cmmnCodeGstp.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_total_VlNm(string searchWord, string searchIn)
        {
            string[] whereClsFrmts = { "defined_by", "is_list_dynamic", "is_enabled",
            "sqlquery_if_dyn", "value_list_desc","value_list_name"};
            string[] ordrByClsFrmts = { "defined_by", "is_list_dynamic", "is_enabled",
            "sqlquery_if_dyn", "value_list_desc", "value_list_name" };
            string[] sortOrder = { "ASC", "ASC", "ASC", "ASC", "ASC", "ASC" };
            int frmt_to_use = 0;
            string strSql = "";
            string optional_str = "";
            if (searchIn == "Defined By")
            {
                frmt_to_use = 0;
            }
            else if (searchIn == "Is Dynamic")
            {
                frmt_to_use = 1;
            }
            else if (searchIn == "Is Enabled")
            {
                frmt_to_use = 2;
            }
            else if (searchIn == "SQL Query")
            {
                frmt_to_use = 3;
            }
            else if (searchIn == "Value List Description")
            {
                frmt_to_use = 4;
            }
            else if (searchIn == "Value List Name")
            {
                frmt_to_use = 5;
            }

            if (searchWord == "")
            {
                optional_str = " OR (" + ordrByClsFrmts[frmt_to_use] + " IS NULL)";
            }

            strSql = "SELECT count(value_list_id) FROM gst.gen_stp_lov_names " +
         "WHERE ((" + whereClsFrmts[frmt_to_use] + " ilike '" + searchWord.Replace("'", "''") +
         "')" + optional_str + ")";

            DataSet dtst = Global.myNwMainFrm.cmmnCodeGstp.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return Int64.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static DataSet get_VlNmInfo(int lovID)
        {
            string strSql = "";
            strSql = "SELECT is_enabled, is_list_dynamic, defined_by, " +
            "value_list_desc, sqlquery_if_dyn, dflt_order_by FROM gst.gen_stp_lov_names " +
            "WHERE ((value_list_id = " + lovID + "))";
            DataSet dtst = Global.myNwMainFrm.cmmnCodeGstp.selectDataNoParams(strSql);
            return dtst;
        }

        public static string get_VlNm_Rec_Hstry(int lovID)
        {
            string strSQL = @"SELECT a.created_by, 
      to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
a.last_update_by, 
to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
            "FROM gst.gen_stp_lov_names a WHERE(a.value_list_id = " + lovID + ")";
            string fnl_str = "";
            DataSet dtst = Global.myNwMainFrm.cmmnCodeGstp.selectDataNoParams(strSQL);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                fnl_str = "CREATED BY: " + Global.myNwMainFrm.cmmnCodeGstp.get_user_name(long.Parse(dtst.Tables[0].Rows[0][0].ToString())) +
                  "\r\nCREATION DATE: " + dtst.Tables[0].Rows[0][1].ToString() + "\r\nLAST UPDATE BY:" +
                  Global.myNwMainFrm.cmmnCodeGstp.get_user_name(long.Parse(dtst.Tables[0].Rows[0][2].ToString())) +
                  "\r\nLAST UPDATE DATE: " + dtst.Tables[0].Rows[0][3].ToString();
                return fnl_str;
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region "POSSIBLE VALUES..."
        public static string get_all_OrgIDs()
        {
            string strSql = "";
            strSql = "SELECT distinct org_id FROM org.org_details";
            DataSet dtst = Global.myNwMainFrm.cmmnCodeGstp.selectDataNoParams(strSql);
            string allwd = ",";
            for (int i = 0; i < dtst.Tables[0].Rows.Count; i++)
            {
                allwd += dtst.Tables[0].Rows[i][0].ToString() + ",";
            }
            return allwd;
        }

        public static long get_total_Pvl(string inptSQL)
        {
            string strSql = "";
            strSql = "SELECT count(1) FROM (" + inptSQL.Replace("{:prsn_id}", Global.myNwMainFrm.cmmnCodeGstp.getUserPrsnID(Global.myNwMainFrm.cmmnCodeGstp.User_id).ToString()) +
          ") tbl1";
            DataSet dtst = Global.myNwMainFrm.cmmnCodeGstp.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return long.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static DataSet get_Pssbl_Vals(string inptSQL, long offset, int limit_size, int lovID, string searchWord, string searchIn)
        {
            string strSql = "";
            string extrWhere = "";
            if (searchIn == "Possible Value")
            {
                extrWhere = "and tbl1.a ilike '" + searchWord.Replace("'", "''") + "'";
            }
            else
            {
                extrWhere = "and tbl1.b ilike '" + searchWord.Replace("'", "''") + "'";
            }

            strSql = "SELECT * FROM (" + inptSQL.Replace("{:prsn_id}", Global.myNwMainFrm.cmmnCodeGstp.getUserPrsnID(Global.myNwMainFrm.cmmnCodeGstp.User_id).ToString()) +
          ") tbl1 WHERE 1=1 " + extrWhere + " ORDER BY tbl1.a LIMIT " + limit_size +
          " OFFSET " + (Math.Abs(offset * limit_size)).ToString();
            Global.myNwMainFrm.pvl_SQL = strSql;
            DataSet dtst = Global.myNwMainFrm.cmmnCodeGstp.selectDataNoParams(strSql);
            return dtst;
        }

        public static DataSet get_Pssbl_Vals(string searchWord, string searchIn,
          Int64 offset, int limit_size, int lovID)
        {
            string[] whereClsFrmts = { "pssbl_value", "pssbl_value_desc" };
            int frmt_to_use = 0;
            string strSql = "";
            string optional_str = "";
            if (searchIn == "Possible Value")
            {
                frmt_to_use = 0;
            }
            else if (searchIn == "Possible Value Description")
            {
                frmt_to_use = 1;
            }
            if (searchWord == "")
            {
                optional_str = " OR (" + whereClsFrmts[frmt_to_use] + " IS NULL)";
            }

            strSql = "SELECT pssbl_value, pssbl_value_desc, is_enabled, pssbl_value_id, allowed_org_ids FROM gst.gen_stp_lov_values " +
         "WHERE ((" + whereClsFrmts[frmt_to_use] + " ilike '" + searchWord.Replace("'", "''") +
         "') AND (value_list_id = " + lovID + ")" + optional_str + ") ORDER BY " + whereClsFrmts[frmt_to_use] + " ASC " +
         " LIMIT " + limit_size +
         " OFFSET " + (Math.Abs(offset * limit_size)).ToString();

            Global.myNwMainFrm.pvl_SQL = strSql;
            DataSet dtst = Global.myNwMainFrm.cmmnCodeGstp.selectDataNoParams(strSql);
            return dtst;
        }

        public static long get_total_Pvl(string searchWord, string searchIn, int lovID)
        {
            string[] whereClsFrmts = { "pssbl_value", "pssbl_value_desc" };
            int frmt_to_use = 0;
            string strSql = "";
            string optional_str = "";
            if (searchIn == "Possible Value")
            {
                frmt_to_use = 0;
            }
            else if (searchIn == "Possible Value Description")
            {
                frmt_to_use = 1;
            }
            if (searchWord == "")
            {
                optional_str = " OR (" + whereClsFrmts[frmt_to_use] + " IS NULL)";
            }

            strSql = "SELECT count(pssbl_value_id) FROM gst.gen_stp_lov_values " +
         "WHERE ((" + whereClsFrmts[frmt_to_use] + " ilike '" + searchWord.Replace("'", "''") +
         "') AND (value_list_id = " + lovID + ")" + optional_str + ")";
            DataSet dtst = Global.myNwMainFrm.cmmnCodeGstp.selectDataNoParams(strSql);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                return Int64.Parse(dtst.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static string get_Pvl_Rec_Hstry(int pssblID)
        {
            string strSQL = @"SELECT a.created_by, 
      to_char(to_timestamp(a.creation_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS'), 
a.last_update_by, 
to_char(to_timestamp(a.last_update_date,'YYYY-MM-DD HH24:MI:SS'),'DD-Mon-YYYY HH24:MI:SS') " +
          "FROM gst.gen_stp_lov_values a WHERE(a.pssbl_value_id = " + pssblID + ")";
            string fnl_str = "";
            DataSet dtst = Global.myNwMainFrm.cmmnCodeGstp.selectDataNoParams(strSQL);
            if (dtst.Tables[0].Rows.Count > 0)
            {
                fnl_str = "CREATED BY: " + Global.myNwMainFrm.cmmnCodeGstp.get_user_name(long.Parse(dtst.Tables[0].Rows[0][0].ToString())) +
                  "\r\nCREATION DATE: " + dtst.Tables[0].Rows[0][1].ToString() + "\r\nLAST UPDATE BY:" +
                  Global.myNwMainFrm.cmmnCodeGstp.get_user_name(long.Parse(dtst.Tables[0].Rows[0][2].ToString())) +
                  "\r\nLAST UPDATE DATE: " + dtst.Tables[0].Rows[0][3].ToString();
                return fnl_str;
            }
            else
            {
                return "";
            }
        }
        #endregion

        #endregion

        #region "VERIFICATION STATEMENTS..."
        public static int getLovID(string lovName)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "SELECT value_list_id from gst.gen_stp_lov_names where (value_list_name = '" +
              lovName.Replace("'", "''") + "')";
            dtSt = Global.myNwMainFrm.cmmnCodeGstp.selectDataNoParams(sqlStr);
            if (dtSt.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtSt.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        public static string getLovName(int lovID)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "SELECT value_list_name from gst.gen_stp_lov_names where (value_list_id = " +
              lovID + ")";
            dtSt = Global.myNwMainFrm.cmmnCodeGstp.selectDataNoParams(sqlStr);
            if (dtSt.Tables[0].Rows.Count > 0)
            {
                return dtSt.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        public static int getPssblValID(string pssblVal, int lovID, string pssblValDesc)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "SELECT pssbl_value_id from gst.gen_stp_lov_values " +
              "where ((pssbl_value = '" +
              pssblVal.Replace("'", "''") + "') AND (pssbl_value_desc = '" +
              pssblValDesc.Replace("'", "''") + "') AND (value_list_id = " + lovID + "))";
            dtSt = Global.myNwMainFrm.cmmnCodeGstp.selectDataNoParams(sqlStr);
            if (dtSt.Tables[0].Rows.Count > 0)
            {
                return int.Parse(dtSt.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        public static string getPssblValNm(int pssblVlID)
        {
            DataSet dtSt = new DataSet();
            string sqlStr = "SELECT pssbl_value from gst.gen_stp_lov_values " +
              "where ((pssbl_value_id = " + pssblVlID + "))";
            dtSt = Global.myNwMainFrm.cmmnCodeGstp.selectDataNoParams(sqlStr);
            if (dtSt.Tables[0].Rows.Count > 0)
            {
                return dtSt.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }
        #endregion
        #endregion

        #region "CUSTOM FUNCTIONS..."
        public static void refreshRqrdVrbls()
        {
            Global.myNwMainFrm.cmmnCodeGstp.DefaultPrvldgs = Global.dfltPrvldgs;
            //Global.myNwMainFrm.cmmnCodeGstp.Login_number = Global.myGenStp.login_number;
            Global.myNwMainFrm.cmmnCodeGstp.ModuleAdtTbl = Global.myGenStp.full_audit_trail_tbl_name;
            Global.myNwMainFrm.cmmnCodeGstp.ModuleDesc = Global.myGenStp.mdl_description;
            Global.myNwMainFrm.cmmnCodeGstp.ModuleName = Global.myGenStp.name;
            //Global.myNwMainFrm.cmmnCodeGstp.pgSqlConn = Global.myGenStp.Host.globalSQLConn;
            //Global.myNwMainFrm.cmmnCodeGstp.Role_Set_IDs = Global.myGenStp.role_set_id;
            //Global.myNwMainFrm.cmmnCodeGstp.Org_id = Global.myGenStp.org_id;
            Global.myNwMainFrm.cmmnCodeGstp.SampleRole = "General Setup Administrator";
            //Global.myNwMainFrm.cmmnCodeGstp.User_id = Global.myGenStp.user_id;
            Global.myNwMainFrm.cmmnCodeGstp.Extra_Adt_Trl_Info = "";
            Global.myGenStp.user_id = Global.myNwMainFrm.usr_id;
            Global.myGenStp.login_number = Global.myNwMainFrm.lgn_num;
            Global.myGenStp.role_set_id = Global.myNwMainFrm.role_st_id;
            Global.myGenStp.org_id = Global.myNwMainFrm.Og_id;

        }

        public static void createSysLovs()
        {
            for (int i = 0; i < Global.sysLovs.Length; i++)
            {
                int lovID = Global.getLovID(sysLovs[i]);
                if (lovID <= 0)
                {
                    if (sysLovsDynQrys[i] == "")
                    {
                        Global.createLovNm(sysLovs[i],
                         sysLovsDesc[i], false, "", "SYS", true, "ORDER BY 1");
                    }
                    else
                    {
                        Global.createLovNm(sysLovs[i],
                   sysLovsDesc[i], true, sysLovsDynQrys[i], "SYS", true, "ORDER BY 1");
                    }
                }
                else
                {
                    if (sysLovsDynQrys[i] != "")
                    {
                        Global.updateLovNm(lovID, true, sysLovsDynQrys[i], "SYS", true);
                    }
                }
            }
        }

        public static void updateLovNm(int lovID, bool isDyn
          , string sqlQry, string dfndBy, bool isEnbld)
        {
            string dateStr = Global.myNwMainFrm.cmmnCodeGstp.getDB_Date_time();
            string sqlStr = "UPDATE gst.gen_stp_lov_names SET " +
                  "is_list_dynamic='" + Global.myNwMainFrm.cmmnCodeGstp.cnvrtBoolToBitStr(isDyn) + "', " +
                  "sqlquery_if_dyn='" + sqlQry.Replace("'", "''") +
          "', defined_by='" + dfndBy.Replace("'", "''") +
              "', last_update_by=" + Global.myGenStp.user_id + ", " +
                  "last_update_date='" + dateStr +
                  "', is_enabled='" + Global.myNwMainFrm.cmmnCodeGstp.cnvrtBoolToBitStr(isEnbld) + "' WHERE value_list_id = " + lovID;
            Global.myNwMainFrm.cmmnCodeGstp.updateDataNoParams(sqlStr);
        }

        public static void createSysLovsPssblVals()
        {
            string allwd = Global.get_all_OrgIDs();
            for (int i = 0; i < Global.pssblVals.Length; i += 3)
            {
                if (Global.getPssblValID(Global.pssblVals[i + 1],
                  Global.getLovID(Global.sysLovs[int.Parse(Global.pssblVals[i])]), Global.pssblVals[i + 2]) <= 0)
                {
                    Global.createPssblValsForLov(Global.getLovID(Global.sysLovs[int.Parse(Global.pssblVals[i])]),
                      Global.pssblVals[i + 1], Global.pssblVals[i + 2], true, allwd);
                }
            }
        }
        #endregion
    }
}

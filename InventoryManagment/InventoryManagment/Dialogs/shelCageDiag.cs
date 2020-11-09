using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StoresAndInventoryManager.Classes;

namespace StoresAndInventoryManager.Dialogs
{
    public partial class shelCageDiag : Form
    {
        public shelCageDiag()
        {
            InitializeComponent();
        }
        public string storeName = "";
        private void invAccbutton_Click(object sender, EventArgs e)
        {
            try
            {
                string[] selVals = new string[1];
                selVals[0] = this.invAccIDtextBox.Text;
                DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                    Global.mnFrm.cmCde.getLovID("Asset Accounts"), ref selVals,
                    true, false, Global.mnFrm.cmCde.Org_id, "%", "Both", true);
                if (dgRes == DialogResult.OK)
                {
                    for (int i = 0; i < selVals.Length; i++)
                    {
                        this.invAccIDtextBox.Text = selVals[i];
                        this.invAcctextBox.Text = Global.mnFrm.cmCde.getAccntName(int.Parse(selVals[i]));
                    }
                }
            }
            catch (Exception ex)
            {
                Global.mnFrm.cmCde.showMsg(ex.Message, 0);
                return;
            }
        }

        private void shelCageDiag_Load(object sender, EventArgs e)
        {
            Color[] clrs = Global.mnFrm.cmCde.getColors();
            this.BackColor = clrs[0];
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool checkExistenceOfShelf(int parShelfID, string parStoreName)
        {
            bool found = false;
            DataSet ds = new DataSet();

            string qryCheckExistenceOfShelf = "SELECT COUNT(*) FROM inv.inv_shelf WHERE shelf_id = " + parShelfID
                + " and store_id = (select b.subinv_id from inv.inv_itm_subinventories b where b.subinv_name = '"
                + parStoreName.Replace("'", "''") + "' AND b.org_id = " + Global.mnFrm.cmCde.Org_id + ") AND org_id = " + Global.mnFrm.cmCde.Org_id;

            ds.Reset();

            ds = Global.fillDataSetFxn(qryCheckExistenceOfShelf);

            string results = ds.Tables[0].Rows[0][0].ToString();

            if (results == "0")
            {
                return found;
            }
            else
            {
                return true;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (int.Parse(this.lineIDTextBox.Text) > 0 && int.Parse(this.shelveIDtextBox.Text) > 0)
            {
                string dateStr = Global.mnFrm.cmCde.getDB_Date_time();
                string qrySaveStoreShelf = @"UPDATE inv.inv_shelf
                   SET shelf_id =" + int.Parse(this.shelveIDtextBox.Text) +
                       ", store_id =" + int.Parse(this.storeIDTextBox.Text) +
                       ", last_update_by = " + Global.myInv.user_id +
                       ", last_update_date = '" + dateStr +
                       "', org_id = " + Global.mnFrm.cmCde.Org_id +
                       ", shelve_name = '" + this.shelveNameTextBox.Text.Replace("'", "''") +
                       "', shelve_desc ='" + this.shelveDesctextBox.Text.Replace("'", "''") +
                       "', lnkd_cstmr_id = " + int.Parse(this.lnkdCstmrIDTextBox.Text) +
                       ", allwd_group_type = '" + this.grpComboBox.Text.Replace("'", "''") +
                       "', allwd_group_value = '" + this.grpNmIDTextBox.Text.Replace("'", "''") +
                       "', enabled_flag = '" + Global.mnFrm.cmCde.cnvrtBoolToBitStr(this.isShelfEnabledcheckBox.Checked) +
                       "', inv_asset_acct_id = " + int.Parse(this.invAccIDtextBox.Text) +
                       ", cage_shelve_mngr_id = " + int.Parse(this.shelveMngrIDTextBox.Text) +
                       ", dflt_item_state = '" + this.dfltItmStateTextBox.Text.Replace("'", "''") +
                       "', managers_wthdrwl_limit = " + this.wthdrwLmtNumUpDown.Value +
                       ", managers_deposit_limit = " + this.depLmtNumUpDwn.Value +
                       ", dflt_item_type = '" + this.itemTypecomboBox.Text.Replace("'", "''") +
                       "' WHERE line_id = " + int.Parse(this.lineIDTextBox.Text);

                Global.mnFrm.cmCde.updateDataNoParams(qrySaveStoreShelf);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void shelvebutton_Click(object sender, EventArgs e)
        {
            try
            {
                string[] selVals = new string[1];
                selVals[0] = this.invAccIDtextBox.Text;
                DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                    Global.mnFrm.cmCde.getLovID("Shelves"), ref selVals,
                    true, false, Global.mnFrm.cmCde.Org_id,
               "%", "Both", true);
                if (dgRes == DialogResult.OK)
                {
                    for (int i = 0; i < selVals.Length; i++)
                    {
                        this.invAccIDtextBox.Text = selVals[i];
                        this.invAcctextBox.Text = Global.mnFrm.cmCde.getAccntName(int.Parse(selVals[i]));
                    }
                }
            }
            catch (Exception ex)
            {
                Global.mnFrm.cmCde.showMsg(ex.Message, 0);
                return;
            }
        }

        private void lnkdCstmrbutton_Click(object sender, EventArgs e)
        {
            try
            {
                string[] selVals = new string[1];
                selVals[0] = this.lnkdCstmrIDTextBox.Text;
                DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                    Global.mnFrm.cmCde.getLovID("All Customers and Suppliers"), ref selVals,
                    true, false, Global.mnFrm.cmCde.Org_id,
               "%", "Both", true);
                if (dgRes == DialogResult.OK)
                {
                    for (int i = 0; i < selVals.Length; i++)
                    {
                        this.lnkdCstmrIDTextBox.Text = selVals[i];
                        this.lnkdCstmrTextBox.Text = Global.mnFrm.cmCde.getCstmrSpplrName(long.Parse(selVals[i]));
                    }
                }
            }
            catch (Exception ex)
            {
                Global.mnFrm.cmCde.showMsg(ex.Message, 0);
                return;
            }
        }

        private void grpComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
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
        }

        private void grpNmButton_Click(object sender, EventArgs e)
        {
            if (this.grpComboBox.Text == "")
            {
                Global.mnFrm.cmCde.showMsg("Please select a Group Type!", 0);
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
                dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                Global.mnFrm.cmCde.getLovID(grpCmbo), ref selVals, true, true, Global.mnFrm.cmCde.Org_id,
               "%", "Both", true);
            }
            else
            {
                dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                Global.mnFrm.cmCde.getLovID("Person Types"), ref selVal1s, true, true,
               "%", "Both", true);
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
                        this.grpNmTextBox.Text = Global.mnFrm.cmCde.getDivName(int.Parse(selVals[i]));
                    }
                    else if (this.grpComboBox.Text == "Grade")
                    {
                        this.grpNmTextBox.Text = Global.mnFrm.cmCde.getGrdName(int.Parse(selVals[i]));
                    }
                    else if (this.grpComboBox.Text == "Job")
                    {
                        this.grpNmTextBox.Text = Global.mnFrm.cmCde.getJobName(int.Parse(selVals[i]));
                    }
                    else if (this.grpComboBox.Text == "Position")
                    {
                        this.grpNmTextBox.Text = Global.mnFrm.cmCde.getPosName(int.Parse(selVals[i]));
                    }
                    else if (this.grpComboBox.Text == "Site/Location")
                    {
                        this.grpNmTextBox.Text = Global.mnFrm.cmCde.getSiteNameDesc(int.Parse(selVals[i]));
                    }
                    else if (this.grpComboBox.Text == "Person Type")
                    {
                        this.grpNmIDTextBox.Text = selVal1s[i].ToString();
                        this.grpNmTextBox.Text = Global.mnFrm.cmCde.getPssblValNm(selVal1s[i]);
                    }
                    else if (this.grpComboBox.Text == "Working Hour Type")
                    {
                        this.grpNmTextBox.Text = Global.mnFrm.cmCde.getWkhName(int.Parse(selVals[i]));
                    }
                    else if (this.grpComboBox.Text == "Gathering Type")
                    {
                        this.grpNmTextBox.Text = Global.mnFrm.cmCde.getGathName(int.Parse(selVals[i]));
                    }
                    else if (this.grpComboBox.Text == "Single Person")
                    {
                        this.grpNmIDTextBox.Text = Global.mnFrm.cmCde.getPrsnID(selVals[i]).ToString();
                        this.grpNmTextBox.Text = Global.mnFrm.cmCde.getPrsnName(selVals[i]);
                    }
                }
            }
        }

        private void dfltItemStateButton_Click(object sender, EventArgs e)
        {
            try
            {
                int[] selVals = new int[1];
                selVals[0] = -1;
                DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                    Global.mnFrm.cmCde.getLovID("Vault Item States"), ref selVals,
                    true, false);
                if (dgRes == DialogResult.OK)
                {
                    for (int i = 0; i < selVals.Length; i++)
                    {
                        this.dfltItmStateTextBox.Text = Global.mnFrm.cmCde.getPssblValNm(selVals[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Global.mnFrm.cmCde.showMsg(ex.Message, 0);
                return;
            }
        }

        private void shelveMngrButton_Click(object sender, EventArgs e)
        {
            string[] selVals = new string[1];
            selVals[0] = this.shelveMngrIDTextBox.Text;
            DialogResult dgRes = Global.mnFrm.cmCde.showPssblValDiag(
                Global.mnFrm.cmCde.getLovID("Active Persons"), ref selVals,
                true, false, "%", "Both", true);
            if (dgRes == DialogResult.OK)
            {
                for (int i = 0; i < selVals.Length; i++)
                {
                    this.shelveMngrIDTextBox.Text = Global.mnFrm.cmCde.getPrsnID(selVals[i]).ToString();
                    this.shelveMngrTextBox.Text = Global.mnFrm.cmCde.getPrsnName(selVals[i]);
                }
            }
        }
    }
}

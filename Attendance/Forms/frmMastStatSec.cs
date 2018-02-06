﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace Attendance.Forms
{
    public partial class frmMastStatSec : Form
    {
        public string mode = "NEW";
        public string GRights = "XXXV";
        public string oldCode = "";

        public frmMastStatSec()
        {
            InitializeComponent();
        }

        private void frmMastStatSec_Load(object sender, EventArgs e)
        {
            ResetCtrl();
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            SetRights();
            
        }

        private string DataValidate()
        {
            string err = string.Empty;

            if (string.IsNullOrEmpty(txtCompCode.Text))
            {
                err = err + "Please Enter CompCode " + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtCompName.Text))
            {
                err = err + "Please Enter CompName..." + Environment.NewLine;
            }


            if (string.IsNullOrEmpty(txtWrkGrpCode.Text))
            {
                err = err + "Please Enter WrkGrpCode " + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtWrkGrpDesc.Text))
            {
                err = err + "Please Enter WrkGrp Description" + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtUnitCode.Text))
            {
                err = err + "Please Enter Unit Code" + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtUnitDesc.Text))
            {
                err = err + "Please Enter Unit Name" + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtDeptCode.Text))
            {
                err = err + "Please Enter Dept Code" + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtStatCode.Text))
            {
                err = err + "Please Enter Stat Code" + Environment.NewLine;
            }


            if (string.IsNullOrEmpty(txtSecCode.Text.Trim()))
            {
                err = err + "Please Enter Section Code" + Environment.NewLine;
            }

            else
            {
                string input = txtSecCode.Text.Trim().ToString();
                bool t = Regex.IsMatch(input, @"^\d+$");
                if (!t)
                {
                    err = err + "Please Enter Sec Code in Numeric Format..(001,123,012) " + Environment.NewLine;
                }
                

            }
            if (string.IsNullOrEmpty(txtSecDesc.Text))
            {
                err = err + "Please Enter Section Name" + Environment.NewLine;
            }
            


            return err;
        }

       
        private void ResetCtrl()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            object s = new object();
            EventArgs e = new EventArgs();
            txtCompCode.Text = "01";
            txtCompName.Text = "";
            txtCompCode_Validated(s, e);
           
            //txtWrkGrpCode.Text = "";
            //txtWrkGrpDesc.Text = "";
            //txtUnitCode.Text = "";
            //txtUnitDesc.Text = "";
            //txtDeptCode.Text = "";
            //txtDeptDesc.Text = "";
            //txtStatCode.Text = "";
            //txtStatDesc.Text = "";

            txtSecCode.Text = "";
            txtSecDesc.Text = "";

            oldCode = "";
        }

        private void SetRights()
        {
            if ( txtSecCode.Text.Trim() != "" && mode == "NEW" && GRights.Contains("A") )
            {
                btnAdd.Enabled = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                
            }
            else if (txtSecCode.Text.Trim() != "" && mode == "OLD")
            {
                btnAdd.Enabled = false;
                if (GRights.Contains("U"))
                {
                    btnUpdate.Enabled = true;
                }

                if (GRights.Contains("D"))
                {
                    btnDelete.Enabled = true;
                }                    
            }

            if (GRights.Contains("XXXV"))
            {
                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                
            }
        }

       

        private void txtWrkGrpCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtCompCode.Text.Trim() == "")
                return;
            
            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";


                sql = "Select WrkGrp,WrkGrpDesc From MastWorkGrp Where CompCode ='" + txtCompCode.Text.Trim() + "'";
                if (e.KeyCode == Keys.F1)
                {

                    obj = (List<string>)hlp.Show(sql, "WrkGrp", "WrkGrp", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }
                
                if (obj.Count == 0)
                {
                   
                    return;
                }
                else if (obj.ElementAt(0).ToString() == "0")
                {
                    return;
                }
                else if (obj.ElementAt(0).ToString() == "")
                {
                    return;
                }
                else
                {

                    txtWrkGrpCode.Text = obj.ElementAt(0).ToString();
                    txtWrkGrpDesc.Text = obj.ElementAt(1).ToString();
                    
                    
                }
            }
        }

        private void txtWrkGrpCode_Validated(object sender, EventArgs e)
        {
            if (txtCompCode.Text.Trim() == "" || txtCompName.Text.Trim() == "" )
            {
                
                return;
            }

            DataSet ds = new DataSet();
            string sql = "select * From MastWorkGrp where CompCode ='" + txtCompCode.Text.Trim() + "' and WrkGrp='" + txtWrkGrpCode.Text.Trim() + "'";
            
            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtCompCode.Text = dr["CompCode"].ToString();
                    txtWrkGrpCode.Text = dr["WrkGrp"].ToString();
                    txtWrkGrpDesc.Text = dr["WrkGrpDesc"].ToString();
                    txtCompCode_Validated(sender,e);
                    
                }
            }

            
        }

        private void txtCompCode_Validated(object sender, EventArgs e)
        {
            if (txtCompCode.Text.Trim() == "")
            {   
                return;
            }

            DataSet ds = new DataSet();
            string sql = "select * from MastComp where CompCode ='" + txtCompCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtCompCode.Text = dr["CompCode"].ToString();
                    txtCompName.Text = dr["CompName"].ToString();        

                }
            }
            else
            {
                txtCompName.Text = "";
            }
            
        }

        private void txtCompCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1 )
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";


                sql = "Select CompCode,CompName From MastComp Where 1 = 1";
                if (e.KeyCode == Keys.F1)
                {

                    obj = (List<string>)hlp.Show(sql, "CompCode", "CompCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }

                if (obj.Count == 0)
                {

                    return;
                }
                else if (obj.ElementAt(0).ToString() == "0")
                {
                    return;
                }
                else if (obj.ElementAt(0).ToString() == "")
                {
                    return;
                }
                else
                {

                    txtCompCode.Text = obj.ElementAt(0).ToString();
                    txtCompName.Text = obj.ElementAt(1).ToString();

                }
            }
        }

       
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string err = DataValidate();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        string sql = "Insert into MastStatSec (CompCode,WrkGrp,UnitCode,DeptCode,StatCode,SecCode,SecDesc,AddDt,AddID) Values ('{0}','{1}','{2}','{3}','{4}','{5}',GetDate(),'{6}')";
                        sql = string.Format(sql, txtCompCode.Text.Trim().ToString(),
                            txtWrkGrpCode.Text.Trim().ToString(),
                            txtUnitCode.Text.Trim().ToString(),
                            txtDeptCode.Text.Trim().ToString(),
                            txtStatCode.Text.Trim().ToString(),
                            txtSecCode.Text.Trim().ToString(),
                            txtSecDesc.Text.Trim().ToString(),
                            Utils.User.GUserID);

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record saved...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetCtrl();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }
        
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string err = DataValidate();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        string sql = "Update MastStatSec Set SecDesc = '{0}', UpdDt = GetDate(), UpdID = '{1}' " +
                            " Where CompCode = '{2}' and WrkGrp = '{3}' and UnitCode = '{4}' and DeptCode = '{5}'  and StatCode = '{6}' and SecCode = '{7}'";

                        sql = string.Format(sql, txtSecDesc.Text.ToString(),
                             Utils.User.GUserID, txtCompCode.Text.Trim().ToString(), txtWrkGrpCode.Text.Trim(),
                             txtUnitCode.Text.Trim(), txtDeptCode.Text.Trim(),txtStatCode.Text.Trim(),
                             txtSecCode.Text.Trim()
                           );

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record Updated...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetCtrl();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string err = DataValidate();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Not Implemented...", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetCtrl();
            
            txtWrkGrpCode.Text = "";
            txtWrkGrpDesc.Text = "";
            txtUnitCode.Text = "";
            txtUnitDesc.Text = "";
            txtDeptCode.Text = "";
            txtDeptDesc.Text = "";
            txtStatCode.Text = "";
            txtStatDesc.Text = "";

            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            SetRights();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUnitCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtCompCode.Text.Trim() == "" || txtWrkGrpCode.Text.Trim() == "")
                return;

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";


                sql = "Select UnitCode,UnitName From MastUnit Where CompCode ='" + txtCompCode.Text.Trim() + "' and WrkGrp = '" + txtWrkGrpCode.Text.Trim() + "' ";
                if (e.KeyCode == Keys.F1)
                {

                    obj = (List<string>)hlp.Show(sql, "UnitCode", "UnitCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }

                if (obj.Count == 0)
                {

                    return;
                }
                else if (obj.ElementAt(0).ToString() == "0")
                {
                    return;
                }
                else if (obj.ElementAt(0).ToString() == "")
                {
                    return;
                }
                else
                {

                    txtUnitCode.Text = obj.ElementAt(0).ToString();
                    txtUnitDesc.Text = obj.ElementAt(1).ToString();
                   
                }
            }
        }

        private void txtUnitCode_Validated(object sender, EventArgs e)
        {
            if (txtCompCode.Text.Trim() == "" || txtCompName.Text.Trim() == "" || txtWrkGrpCode.Text.Trim() == "" || txtWrkGrpDesc.Text.Trim() == "")
            {

                return;
            }

            txtUnitCode.Text = txtUnitCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastUnit where CompCode ='" + txtCompCode.Text.Trim() + "' " +
                    " and WrkGrp='" + txtWrkGrpCode.Text.Trim() + "' and UnitCode ='" + txtUnitCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtCompCode.Text = dr["CompCode"].ToString();
                    txtWrkGrpCode.Text = dr["WrkGrp"].ToString();
                    txtUnitCode.Text = dr["UnitCode"].ToString();
                    txtUnitDesc.Text = dr["UnitName"].ToString();
                    txtCompCode_Validated(sender, e);
                    txtWrkGrpCode_Validated(sender, e);
                    
                }
            }
        }

        private void txtDeptCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtCompCode.Text.Trim() == "" || txtWrkGrpCode.Text.Trim() == "" || txtUnitCode.Text.Trim() == "")
                return;

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";

                sql = "Select DeptCode,DeptDesc From MastDept Where CompCode ='" + txtCompCode.Text.Trim() + "' " +
                    " and WrkGrp = '" + txtWrkGrpCode.Text.Trim() + "' and UnitCode ='" + txtUnitCode.Text.Trim() + "'";
                
                if (e.KeyCode == Keys.F1)
                {
                    
                    obj = (List<string>)hlp.Show(sql, "DeptCode", "DeptCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }
                else
                {
                   
                    obj = (List<string>)hlp.Show(sql, "DeptDesc", "DeptDesc", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }

                if (obj.Count == 0)
                {

                    return;
                }
                else if (obj.ElementAt(0).ToString() == "0")
                {
                    return;
                }
                else if (obj.ElementAt(0).ToString() == "")
                {
                    return;
                }
                else
                {

                    txtDeptCode.Text = obj.ElementAt(0).ToString();
                    txtDeptDesc.Text = obj.ElementAt(1).ToString();

                }
            }
        }
        
        private void txtDeptCode_Validated(object sender, EventArgs e)
        {
            if (txtCompCode.Text.Trim() == "" || txtWrkGrpCode.Text.Trim() == "" 
                || txtUnitCode.Text.Trim() == "" )
            {

                return;
            }

            txtDeptCode.Text = txtDeptCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastDept where CompCode ='" + txtCompCode.Text.Trim() + "' " +
                    " and WrkGrp='" + txtWrkGrpCode.Text.Trim() + "' " + 
                    " and UnitCode ='" + txtUnitCode.Text.Trim() + "' " + 
                    " and DeptCode ='" + txtDeptCode.Text.Trim() + "' ";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtCompCode.Text = dr["CompCode"].ToString();
                    txtWrkGrpCode.Text = dr["WrkGrp"].ToString();
                    txtUnitCode.Text = dr["UnitCode"].ToString();
                    txtDeptCode.Text = dr["DeptCode"].ToString();
                    txtDeptDesc.Text = dr["DeptDesc"].ToString();
                    txtCompCode_Validated(sender, e);
                    txtWrkGrpCode_Validated(sender, e);
                    txtUnitCode_Validated(sender, e);                    
                }
            }
            
        }

        private void txtStatCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtCompCode.Text.Trim() == "" || txtWrkGrpCode.Text.Trim() == "" 
                || txtUnitCode.Text.Trim() == "" || txtDeptCode.Text.Trim() == "" )
                return;

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";

                sql = "Select StatCode,StatDesc From MastStat Where CompCode ='" + txtCompCode.Text.Trim() + "' " +
                   " and WrkGrp = '" + txtWrkGrpCode.Text.Trim() + "' and UnitCode ='" + txtUnitCode.Text.Trim() + "' " +
                   " and DeptCode='" + txtDeptCode.Text.Trim() + "'";

                if (e.KeyCode == Keys.F1)
                {
                   
                    obj = (List<string>)hlp.Show(sql, "StatCode", "StatCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }
                else
                {  
                    obj = (List<string>)hlp.Show(sql, "StatDesc", "StatDesc", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }

                if (obj.Count == 0)
                {

                    return;
                }
                else if (obj.ElementAt(0).ToString() == "0")
                {
                    return;
                }
                else if (obj.ElementAt(0).ToString() == "")
                {
                    return;
                }
                else
                {
                    txtStatCode.Text = obj.ElementAt(0).ToString();
                    txtStatDesc.Text = obj.ElementAt(1).ToString();
                }
            }
        }

        private void txtStatCode_Validated(object sender, EventArgs e)
        {
            if (txtCompCode.Text.Trim() == "" ||  txtWrkGrpCode.Text.Trim() == "" 
                || txtUnitCode.Text.Trim() == "" || txtDeptCode.Text.Trim() == "" 
                || txtStatCode.Text.Trim() == "" )
            {

                return;
            }

            txtStatCode.Text = txtStatCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastStat where CompCode ='" + txtCompCode.Text.Trim() + "' " +
                    " and WrkGrp='" + txtWrkGrpCode.Text.Trim() + "' " +
                    " and UnitCode ='" + txtUnitCode.Text.Trim() + "' " +
                    " and DeptCode ='" + txtDeptCode.Text.Trim() + "' " +
                    " and StatCode ='" + txtStatCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtCompCode.Text = dr["CompCode"].ToString();
                    txtWrkGrpCode.Text = dr["WrkGrp"].ToString();
                    txtUnitCode.Text = dr["UnitCode"].ToString();
                    txtDeptCode.Text = dr["DeptCode"].ToString();
                    
                    txtStatCode.Text = dr["StatCode"].ToString();
                    txtStatDesc.Text = dr["StatDesc"].ToString();
                    txtCompCode_Validated(sender, e);
                    txtWrkGrpCode_Validated(sender, e);
                    txtUnitCode_Validated(sender, e);
                    txtDeptCode_Validated(sender, e);
                    
                }
            }
            else
            {
                mode = "NEW";
                oldCode = "";
            }
                        
            SetRights();
        }

        private void txtSecCode_Validated(object sender, EventArgs e)
        {
            if (txtCompCode.Text.Trim() == "" || txtWrkGrpCode.Text.Trim() == ""
                || txtUnitCode.Text.Trim() == "" || txtDeptCode.Text.Trim() == ""
                || txtStatCode.Text.Trim() == "" || txtSecCode.Text.Trim() == "")
            {

                return;
            }

            txtSecCode.Text = txtSecCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastStatSec where CompCode ='" + txtCompCode.Text.Trim() + "' " +
                    " and WrkGrp='" + txtWrkGrpCode.Text.Trim() + "' " +
                    " and UnitCode ='" + txtUnitCode.Text.Trim() + "' " +
                    " and DeptCode ='" + txtDeptCode.Text.Trim() + "' " +
                    " and StatCode ='" + txtStatCode.Text.Trim() + "' " +
                    " and SecCode ='" + txtSecCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtCompCode.Text = dr["CompCode"].ToString();
                    txtWrkGrpCode.Text = dr["WrkGrp"].ToString();
                    txtUnitCode.Text = dr["UnitCode"].ToString();
                    txtDeptCode.Text = dr["DeptCode"].ToString();
                    txtStatCode.Text = dr["StatCode"].ToString();
                    txtSecCode.Text = dr["SecCode"].ToString();
                    txtSecDesc.Text = dr["SecDesc"].ToString();
                    
                    txtCompCode_Validated(sender, e);
                    txtWrkGrpCode_Validated(sender, e);
                    txtUnitCode_Validated(sender, e);
                    txtDeptCode_Validated(sender, e);
                    txtStatCode_Validated(sender, e);

                    mode = "OLD";
                    oldCode = dr["SecCode"].ToString();

                }
            }
            else
            {
                mode = "NEW";
                oldCode = "";
            }

            SetRights();
        }

        private void txtSecCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtCompCode.Text.Trim() == "" || txtWrkGrpCode.Text.Trim() == ""
                || txtUnitCode.Text.Trim() == "" || txtDeptCode.Text.Trim() == "" 
                || txtStatCode.Text.Trim() == ""
                )
                return;

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";

                sql = "Select SecCode,SecDesc From MastStatSec Where CompCode ='" + txtCompCode.Text.Trim() + "' " +
                   " and WrkGrp = '" + txtWrkGrpCode.Text.Trim() + "' and UnitCode ='" + txtUnitCode.Text.Trim() + "' " +
                   " and DeptCode='" + txtDeptCode.Text.Trim() + "' and StatCode ='" + txtStatCode.Text.Trim() + "'";

                if (e.KeyCode == Keys.F1)
                {

                    obj = (List<string>)hlp.Show(sql, "SecCode", "SecCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }
                else
                {
                    obj = (List<string>)hlp.Show(sql, "SecDesc", "SecDesc", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }

                if (obj.Count == 0)
                {

                    return;
                }
                else if (obj.ElementAt(0).ToString() == "0")
                {
                    return;
                }
                else if (obj.ElementAt(0).ToString() == "")
                {
                    return;
                }
                else
                {
                    txtSecCode.Text = obj.ElementAt(0).ToString();
                    txtSecDesc.Text = obj.ElementAt(1).ToString();
                }
            }
        }
    }
}

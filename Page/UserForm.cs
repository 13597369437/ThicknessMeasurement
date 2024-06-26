using MathNet.Numerics.LinearAlgebra.Factorization;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Utilities.Encoders;
using Sunny.UI;
using Sunny.UI.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThicknessMeasurement
{
    public partial class UserForm : UIPage
    {
        IFreeSql fsql = DB.MySQL;

      
        public UserForm()
        {
            InitializeComponent();

            Main.Denglu += Main_Denglu;

        }

        private void Main_Denglu(bool obj)
        {
            if (DataClass.Power >= 3)
            {
                uiButton3.Enabled = true;
                uiButton4.Enabled = true;
                uiButton5.Enabled = true;
                uiButton1.Enabled = true;
            }
            else if(DataClass.Power >0)
            {
                uiButton3.Enabled = true;
            }
            else
            {
                uiButton3.Enabled = false;
                uiButton4.Enabled = false;
                uiButton5.Enabled = false;
                uiButton1.Enabled = false;
            }
        }

        //修改密码按钮
        private void uiButton3_Click(object sender, EventArgs e)
        {
            xiugai();

        }

        //新增用户
        private void uiButton4_Click(object sender, EventArgs e)
        {
            adduser();
        }

        //删除用户
        private void uiButton5_Click(object sender, EventArgs e)
        {
            deleteuser();
        }

        void xiugai()
        {
            UIEditOption option = new UIEditOption();
            option.AddText("user", "用户名", DataClass.User, true);
            option.AddText("name", "姓名", DataClass.Name, true);
            option.AddPassword("password", "密码", null, true);
            option.AddPassword("password1", "新密码", null, true);
            option.AddPassword("password2", "确认密码", null, true);

            UIEditForm frm = new UIEditForm(option);
            frm.Render();
            frm.CheckedData += Frm_CheckedData;
            frm.ShowDialog();

            if (frm.IsOK)
            {
                
                fsql.Update<用户管理>()
                    .Set(a => a.密码, frm["password1"])
                    .Set(a => a.姓名, frm["name"])
                    .Where(w => w.用户名 == DataClass.User).ExecuteAffrows();
                
                ShowSuccessTip("修改密码成功！");
               
            }

            frm.Dispose();
        }

        private bool Frm_CheckedData(object sender, UIEditForm.EditFormEventArgs e)
        {
            var users=fsql.Select< 用户管理 >().Where(w=>w.用户名== DataClass.User).ToList();
            if (e.Form["password"].ToString() != users[0].密码)
            {
                e.Form.SetEditorFocus("password");
                ShowWarningTip("密码错误");
                return false;
            }
            if (e.Form["password1"].ToString() != e.Form["password2"].ToString())
            {
                e.Form.SetEditorFocus("password1");
                ShowWarningTip("新密码不一致");
                return false;
            }

            return true;
        }
   
        void adduser()
        {
            string[] powers = new[] { "员工", "主管" ,"管理员"};
            UIEditOption option = new UIEditOption();
            option.AddText("user", "用户名", null, true);
            option.AddText("name", "姓名", null, true);
            option.AddPassword("password", "密码", null, true);
            option.AddPassword("password1", "确认密码", null, true);
            option.AddCombobox("Power", "权限", powers, 0, true, true);
            

            UIEditForm frm = new UIEditForm(option);
            frm.Render();
            frm.CheckedData += Frm_addData;
            frm.ShowDialog();
            
            if (frm.IsOK)
            {
                int power = int.Parse(frm["Power"].ToString())+1;
                用户管理 user = new 用户管理()
                {
                    用户名 = frm["user"].ToString(),
                    姓名 = frm["name"].ToString(),
                    密码 = frm["password"].ToString(),
                    权限 = power.ToString()

                };
                fsql.Insert(user).ExecuteAffrows();
                ShowSuccessTip("新增用户成功！");
                updatedgv();
            }

            frm.Dispose();
        }

        private bool Frm_addData(object sender, UIEditForm.EditFormEventArgs e)
        {
         
            if (e.Form["password1"].ToString() != e.Form["password"].ToString())
            {
                e.Form.SetEditorFocus("password1");
                ShowWarningTip("密码不一致");
                return false;
            }

            return true;
        }


        void deleteuser()
        {
            string s = uiDataGridView1.Rows[uiDataGridView1.SelectedIndex].Cells[1].Value.ToString();
            if (ShowAskDialog("是否删除用户：" + s))
            {
                fsql.Delete<用户管理>().Where(d => d.姓名 == s).ExecuteAffrows();
               
                ShowSuccessTip($"用户:{s}已删除！");
            }
            updatedgv();
        }

        private void UserForm_Initialize(object sender, EventArgs e)
        {
            updatedgv();
        }

        void updatedgv()
        {
            var usertable = fsql.Select<用户管理>().Where(w => w.用户名 != "Admin").ToDataTable();
            usertable.Columns.Remove(usertable.Columns["密码"]);
            uiDataGridView1.DataSource = null;
            uiDataGridView1.DataSource = usertable;
            uiDataGridView1.Columns[0].Visible = false;


            for (int i = 0; i < usertable.Rows.Count; i++)
            {
                if (uiDataGridView1.Rows[i].Cells[3].Value.ToString() =="1")
                {
                    uiDataGridView1.Rows[i].Cells[3].Value = "员工";
                }
                else if (uiDataGridView1.Rows[i].Cells[3].Value.ToString() == "2")
                {
                    uiDataGridView1.Rows[i].Cells[3].Value = "主管";
                }
                else if (uiDataGridView1.Rows[i].Cells[3].Value.ToString() == "3")
                {
                    uiDataGridView1.Rows[i].Cells[3].Value = "管理员";
                }
            }


        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            string user = uiDataGridView1.Rows[uiDataGridView1.SelectedIndex].Cells[1].Value.ToString();
            string name = uiDataGridView1.Rows[uiDataGridView1.SelectedIndex].Cells[2].Value.ToString();
            var yhgl = fsql.Select<用户管理>().Where(w => w.用户名 == user).ToList();
            int pow = int.Parse(yhgl[0].权限) - 1;

            string[] powers = new[] { "员工", "主管", "管理员" };
            UIEditOption option = new UIEditOption();
            option.AddText("user", "用户名", user, true);
            option.AddText("name", "姓名", name, true);
            option.AddPassword("password", "密码", yhgl[0].密码, true);
            option.AddPassword("password1", "确认密码", yhgl[0].密码, true);
            option.AddCombobox("Power", "权限", powers, pow, true, true);


            UIEditForm frm = new UIEditForm(option);
            frm.Render();
            frm.CheckedData += Frm_CheckedData1;
            frm.ShowDialog();

            if (frm.IsOK)
            {
                int power = int.Parse(frm["Power"].ToString()) + 1;
                fsql.Update<用户管理>()
                    .Set(a => a.密码, frm["password1"])
                    .Set(a => a.姓名, frm["name"])
                    .Set(a => a.权限, power.ToString())
                    .Where(w => w.用户名 == user).ExecuteAffrows();

                ShowSuccessTip("权限变更成功！");
                updatedgv();
            }

            frm.Dispose();
        }

        private bool Frm_CheckedData1(object sender, UIEditForm.EditFormEventArgs e)
        {
            if (e.Form["password1"].ToString() != e.Form["password"].ToString())
            {
                e.Form.SetEditorFocus("password");
                ShowWarningTip("新密码不一致");
                return false;
            }
            return true;
        }


        int a = 5;int b = 0;
        private void uiButton2_Click(object sender, EventArgs e)
        {
   
          
        }

        int i = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (i > 8)
            //    i = 0;

            //b = a << i;
            //uiLight1.State = (b & 1 << 1) > 0 ? UILightState.On : UILightState.Off;
            //uiLight2.State = (b & 1 << 2) > 0 ? UILightState.On : UILightState.Off;
            //uiLight3.State = (b & 1 << 3) > 0 ? UILightState.On : UILightState.Off;
            //uiLight4.State = (b & 1 << 4) > 0 ? UILightState.On : UILightState.Off;
            //uiLight5.State = (b & 1 << 5) > 0 ? UILightState.On : UILightState.Off;
            //uiLight6.State = (b & 1 << 6) > 0 ? UILightState.On : UILightState.Off;
            //uiLight7.State = (b & 1 << 7) > 0 ? UILightState.On : UILightState.Off;
            //uiLight8.State = (b & 1 << 8) > 0 ? UILightState.On : UILightState.Off;

            //i++;
        }


    }
}

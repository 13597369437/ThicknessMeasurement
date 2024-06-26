using Sunny.UI;
using Sunny.UI.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThicknessMeasurement
{
    public partial class 配置界面 : UIPage
    {
        IFreeSql fsql = DB.MySQL;
        DataTable dt=new DataTable();
        List<用户管理> users = new List<用户管理>();
        public 配置界面()
        {
            InitializeComponent();
        }

        //保存参数按钮
        private void uiButton1_Click(object sender, EventArgs e)
        {
            if(DataClass.Power!=3)
            {
                ShowWarningTip("权限不足！");
            }

            funt2();

            //UILoginForm frm = new UILoginForm
            //{
            //    ShowInTaskbar = true,
            //    Text = "Login",
            //    Title = "登录以获取权限进行修改",
            //    SubText = "",
            //    UserName = "Admin"
            //};

            ////绑定确定按钮事件
            //frm.OnLogin += Frm_OnLogin;
            ////设置主题
            //frm.LoginImage = UILoginForm.UILoginImage.Login4;
            //frm.ShowDialog();
            //if (frm.IsLogin)
            //{
            //funt2();

            //}

            //frm.Dispose();



        }


        void funt2()
        {
            bool ok = true;
            int csindex = 0;
            List<配置> pztab = new List<配置>();
            for (int i = 0; i < uiDataGridView1.Rows.Count; i++)
            {
                int id = int.Parse(uiDataGridView1.Rows[i].Cells[0].Value.ToString());
                string name = uiDataGridView1.Rows[i].Cells[1].Value.ToString();
                string value = uiDataGridView1.Rows[i].Cells[2].Value.ToString();
                string beizhu = uiDataGridView1.Rows[i].Cells[3].Value.ToString();
                if (value == "")
                {
                    ok = false;
                    csindex = i;
                    break;
                }

                pztab.Add(new 配置() { id = id, 项目 = name, 值 = value, 备注 = beizhu });
            }

            if (!ok)
            {
                ShowWarningTip($"第{csindex}行的值为空");
                return;
            }

            foreach (var pf in pztab)
            {
                fsql.Update<配置>()
                    .Set(s => s.值 == pf.值)
                    .Where(w => w.id == pf.id).ExecuteAffrowsAsync();
            }

            ShowSuccessTip("配置保存成功");
        }

        private bool Frm_OnLogin(string userName, string password)
        {
          if(userName == "Admin" && password == "123")
            {
                return true;
            }
          else
            {
                ShowWarningTip("用户名或密码错误");
                return false;
            }
        }

        private void 配置界面_Initialize(object sender, EventArgs e)
        {

            func1(0,50);
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < uiDataGridView1.Rows.Count; i++)
            {
                uiDataGridView1.Rows[i].Cells[2].Value = uiDataGridView1.Rows[i].Cells[4].Value.ToString();
            }

        }

        //系统参数
        private void 系统参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            func1(0, 50);
        }

        //测厚仪参数
        private void 测厚仪参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (DataClass.peizhivalues[0])
            {
                case "0":
                    func1(50, 100);
                    break;
                case "1":
                    func1(100, 150);
                    break;
                case "2":
                    func1(200, 250);
                    break;
            }
            
        }


        //PLC参数
        private void pLCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            func1(150, 200);
        }

        //参数读取
        void func1(int a,int b)
        {
            dt = fsql.Select<配置>().Where(w => w.id >= a && w.id < b && w.项目 != "null").ToDataTable();
            uiDataGridView1.DataSource = dt;


            uiDataGridView1.Columns[0].ReadOnly = true;
            uiDataGridView1.Columns[1].ReadOnly = true;
            uiDataGridView1.Columns[3].ReadOnly = true;
            uiDataGridView1.Columns[4].ReadOnly = true;

            uiDataGridView1.Columns[0].Width = 100;
            uiDataGridView1.Columns[1].Width = 210;
            uiDataGridView1.Columns[2].Width = 200;
            uiDataGridView1.Columns[3].Width = 700;
            uiDataGridView1.Columns[4].Width = 210;

        }

    }
}

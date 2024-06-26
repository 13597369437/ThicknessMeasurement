using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny.UI;
using System.IO;
using Sunny;
using CF_Library;
using ThicknessMeasurement.Page;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace ThicknessMeasurement
{
	public partial class Main : UIForm
    {
        TreeNode parent;
        public static DataTable dtConfig = new DataTable();
        IFreeSql fsql = DB.MySQL;

		public static Action<bool> Denglu;
        public Main()
        {
            DataClass.readpeizhi(fsql);
            DataClass.readpeifang(fsql);
            InitializeComponent();
		
			int pageIndex = 100;
         
            Aside.CreateNode(AddPage(new Auto(), ++pageIndex));

			if (DataClass.peizhivalues[1] == "1" && DataClass.peizhivalues[3] == "1")
				Aside.CreateNode(AddPage(new 寿命管控(), ++pageIndex));

            Aside.CreateNode(AddPage(new History(), ++pageIndex));
            Aside.CreateNode(AddPage(new 测量参数设置1(), ++pageIndex));


            if (DataClass.peizhivalues[2] == "1")
                Aside.CreateNode(AddPage(new 钢板参数设置(), ++pageIndex));

			if (DataClass.peizhivalues[0] == "1")
			{
				Aside.CreateNode(AddPage(new Calibration(), ++pageIndex));
			}
			else if (DataClass.peizhivalues[0] == "0")
			{
				Aside.CreateNode(AddPage(new 标定(), ++pageIndex));
			}
			else 
			{
                Aside.CreateNode(AddPage(new ss标定(), ++pageIndex));
            }


            Aside.CreateNode(AddPage(new UserForm(), ++pageIndex));


            
        }
		private void Main_Load(object sender, EventArgs e)
		{
			//加载配置文件
			//dtConfig = configGet();
			//Aside.SelectPage(103);
			//Delay(50);
			//Aside.SelectPage(101);

		}
		/// <summary>
		/// 加载配置文件
		/// </summary>
		/// <returns></returns>
		public DataTable configGet()
		{
			DataTable locDt = new DataTable();
			string filePath = $@".\Config\Config.cfg";
			try
			{
				using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
				{
					string line;
					while ((line = sr.ReadLine()) != null)
					{
						if (line.IndexOf(",") < 0) break;
						if (line.IndexOf("index") >= 0)
						{
							string[] s1 = line.Split(',');
							foreach (string s2 in s1)
							{
								locDt.Columns.Add(new DataColumn(s2));
							}
						}
						else
						{
							string[] s1 = line.Split(',');
							DataRow row = locDt.Rows.Add();
							for (int i = 0; i < s1.Length; i++)
							{
								row[i] = s1[i];
							}
						}
					}
				}
			}
			catch
			{
				ShowErrorDialog("配置文件缺失，软件无法使用");
				return null;
			}
			return locDt;
		}
		
		public object parameterGet(string pName)
		{
			object parameter = null;
			if (dtConfig == null || dtConfig.Rows.Count < 1) return parameter;
			string sUnit = pName.ToString().Substring(0, pName.ToString().IndexOf("_"));
			string sItem = pName.ToString().Substring(pName.ToString().IndexOf("_") + 1);
			DataRow[] row = dtConfig.Select($"unit ='{sUnit}' and item='{sItem}'");
			parameter = row[0]["value"];
			return parameter;
		}

	
		private void Main_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (DataClass.peizhivalues[0] == "1")
			{
                CF_UserInterface.HPS_CF_CloseDevice(Auto.deviceHandle1);
                CF_UserInterface.HPS_CF_CloseDevice(Auto.deviceHandle2);
                CF_UserInterface.HPS_CF_CloseDevice(Auto.deviceHandle3);
            }


			System.Diagnostics.Process.GetCurrentProcess().Kill();
		}

        private void Main_HotKeyEventHandler(object sender, HotKeyEventArgs e)
        {
			if (e.hotKey.Key == Keys.F8)
			{
				TreeNode parent = Aside.CreateNode("手动", 61451, 24, 1000);
            }

        }
      
		public static void Delay(int mm)
        {
            DateTime current = DateTime.Now;
            while (current.AddMilliseconds(mm) > DateTime.Now)
            {
                Application.DoEvents();
            }
            return;
        }


		private void timer1_Tick(object sender, EventArgs e)
		{
			if (Auto.measuringStatus == "正在测厚……")
			{
				if (Aside.GetPageIndex(parent) != 101)
				{
					Aside.SelectPage(101);
					//ShowInfoTip("正在测厚中，自动切回测厚页面");
				}
				Aside.Enabled = false;
			}
			else { Aside.Enabled = true; }
		}

		private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
		{
            DataClass.readpeizhi(fsql);
            DataClass.readpeifang(fsql);
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
           
            if (uiButton1.Text == "登录")
			{
				UILoginForm frm = new UILoginForm
				{
					ShowInTaskbar = true,
					Text = "Login",
					Title = "登录以获取权限进行修改",
					SubText = "",
					UserName = "Admin"
				};
				//绑定确定按钮事件
				frm.OnLogin += Frm_OnLogin;
				//设置主题
				frm.LoginImage = UILoginForm.UILoginImage.Login4;
				frm.ShowDialog();
				if (frm.IsLogin)
				{
					uiButton1.Text = "注销";

					if(DataClass.Power==3)
					{
                        Aside.CreateNode(AddPage(new 配置界面()));
                    }

                    Denglu(true);
				}

				frm.Dispose();

			}
			else
			{
				uiButton1.Text = "登录";
                DataClass.Power = 0;
                DataClass.User = "";
                Denglu(false);

				foreach(TreeNode n in Aside.Nodes)
				{
					if (n.Text == "配置")
						Aside.Nodes.Remove(n);
                }

				Aside.SelectPage(101);
				
			}
        }

        private bool Frm_OnLogin(string userName, string password)
        {
            var users = fsql.Select<用户管理>().Where(w => w.用户名 == userName).ToList();
            if ((users.Count==0))
            {
                ShowWarningTip("用户名不存在");
                return false;
            }
            if (password != users[0].密码)
            {
                ShowWarningTip("密码错误");
                return false;
            }
            else
            {
				DataClass.Power = int.Parse(users[0].权限);
				DataClass.User = userName;
				DataClass.Name = users[0].姓名;
                return true;
            }
        }
    }
}

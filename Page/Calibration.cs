using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlX.XDevAPI.Common;
using Sunny.UI;
using System.Configuration;
using CF_Library;

namespace ThicknessMeasurement
{
	public partial class Calibration : UIPage
	{
		public Calibration()
		{
			InitializeComponent();
		}

        private void Calibration_Load(object sender, EventArgs e)
        {
            // 读取设定
            using (StreamReader sr = new StreamReader($@".\Config\config.cfg"))
            {
                mode = Convert.ToInt32(sr.ReadLine());
                if (mode == 0)
                {
                    uiComboBox1.SelectedIndex = 0;
                    uiDoubleUpDown2.Enabled = false;
                }
                else if (mode == 1)
                {
                    uiComboBox1.SelectedIndex = 1;
                    uiDoubleUpDown1.Enabled = false;
                }
                setThickness = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown16.Value = setThickness;
                setMax = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown2.Value = setMax;
                setMin = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown3.Value = setMin;
                setMax_Min = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown1.Value = setMax_Min;
                setLocation1 = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown4.Value = setLocation1;
                setLocation2 = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown5.Value = setLocation2;
                setLocation3 = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown7.Value = setLocation3;
                setLocation4 = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown6.Value = setLocation4;
                lotNum = sr.ReadLine(); uiTextBox8.Text = lotNum;
                batchNum = sr.ReadLine(); uiTextBox6.Text = batchNum;
                points = Convert.ToInt32(sr.ReadLine());
                if (points == 9)
                {
                    uiComboBox2.SelectedIndex = 0;
                    uiDoubleUpDown6.Enabled = false;

                }
                else if (points == 12)
                {
                    uiComboBox2.SelectedIndex = 1;
                    uiDoubleUpDown6.Enabled = true;
                }
                lineChartRange = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown8.Value = lineChartRange;
                barChartRange = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown9.Value = barChartRange;
            }
        }
        private void uiGroupBox1_Click(object sender, EventArgs e)
        {

        }
        public static int  mode = 0;
        public static double setThickness = 1;
        public static double setMax = 0;
        public static double setMin = 0;
        public static double setMax_Min = 0;
        public static double setLocation1 = 20;
        public static double setLocation2 = 40;
        public static double setLocation3 = 60;
        public static double setLocation4 = 80;
        public static string lotNum = "";
        public static string batchNum = "";
        public static int points = 0;
        public static double lineChartRange = 0.5;
        public static double barChartRange = 0.2;

        private void Calibration_Initialize(object sender, EventArgs e)
        {
            
        }

        private void uiDoubleUpDown1_ValueChanged(object sender, double value)
        {

        }

        private void uiGroupBox2_Click(object sender, EventArgs e)
        {

        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["Password"] == "")
			{

			}
            else
			{
                string inputPassword = "";
                if (this.InputPasswordDialog(ref inputPassword))
                {
                    if (inputPassword == "ACCUTECH123")
                    {
                        uiPanel1.Visible = true;
                        return;
                    }
                    if (inputPassword != ConfigurationManager.AppSettings["Password"])
                    {
                        ShowErrorTip("密码错误!");
                        return;
                    }

                }
                else { ShowWarningTip("取消!"); return; }
            }
            
            using (StreamWriter sw = new StreamWriter($@".\Config\config.cfg"))
            {
                sw.WriteLine(uiComboBox1.SelectedIndex == 0 ? "0" : "1");
                sw.WriteLine(uiDoubleUpDown16.Value.ToString());
                sw.WriteLine(uiDoubleUpDown2.Value.ToString());
                sw.WriteLine(uiDoubleUpDown3.Value.ToString());
                sw.WriteLine(uiDoubleUpDown1.Value.ToString());
                sw.WriteLine(uiDoubleUpDown4.Value.ToString());
                sw.WriteLine(uiDoubleUpDown5.Value.ToString());
                sw.WriteLine(uiDoubleUpDown7.Value.ToString());
                sw.WriteLine(uiDoubleUpDown6.Value.ToString());
                sw.WriteLine(uiTextBox8.Text);
                sw.WriteLine(uiTextBox6.Text);
                sw.WriteLine(uiComboBox2.SelectedIndex == 0 ? "9" : "12");
                sw.WriteLine(uiDoubleUpDown8.Value.ToString());
                sw.WriteLine(uiDoubleUpDown9.Value.ToString());
            }
            using (StreamReader sr = new StreamReader($@".\Config\config.cfg"))
            {
                mode = Convert.ToInt32(sr.ReadLine());
                if (mode == 0)
                {
                    uiComboBox1.SelectedIndex = 0;
                    uiDoubleUpDown2.Enabled = false;
                }
                else if (mode == 1)
                {
                    uiComboBox1.SelectedIndex = 1;
                    uiDoubleUpDown1.Enabled = false;
                }
                setThickness = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown16.Value = setThickness;
                setMax = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown2.Value = setMax;
                setMin = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown3.Value = setMin;
                setMax_Min = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown1.Value = setMax_Min;
                setLocation1 = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown4.Value = setLocation1;
                setLocation2 = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown5.Value = setLocation2;
                setLocation3 = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown7.Value = setLocation3;
                setLocation4 = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown6.Value = setLocation4;
                lotNum = sr.ReadLine(); uiTextBox8.Text = lotNum;
                batchNum = sr.ReadLine(); uiTextBox6.Text = batchNum;
                points = Convert.ToInt32(sr.ReadLine());
                if (points == 9)
                {
                    uiComboBox2.SelectedIndex = 0;
                    uiDoubleUpDown6.Enabled = false;

                }
                else if (points == 12)
                {
                    uiComboBox2.SelectedIndex = 1;
                    uiDoubleUpDown6.Enabled = true;
                }
                lineChartRange = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown8.Value = lineChartRange;
                barChartRange = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown9.Value = barChartRange;
            }
            ShowSuccessTip("保存设定成功！");
            isSaved = true;
        }
        public static bool isSaved = false;

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uiComboBox1.SelectedIndex == 0)
            {
                uiDoubleUpDown1.Enabled = true;
                uiDoubleUpDown2.Enabled = false;
            }
            else
            {
                uiDoubleUpDown1.Enabled = false;
                uiDoubleUpDown2.Enabled = true;
            }
        }
        StatusTypeDef ret = StatusTypeDef.Status_Succeed;

        private void uiButton2_Click(object sender, EventArgs e)
        {
            if (Auto.measuringStatus == "正在测厚……") return;
            string lb = lb_calibthicknesspointnum.Text;
            //写入真实厚度值，同时记录测量值
            ret=CF_UserInterface.hps_setDoubleChannelThicknessSamplePoint(Auto.deviceHandle1, 0, (float)nud_calibthickness.Value);      //此处直接写了一个设备g_handle1,以及一个设备的第一组双通道0


            //更新当前写入点数
            lb_calibthicknesspointnum.Text = CF_UserInterface.hps_getDoubleChannelThicknessSamplePoint(Auto.deviceHandle1, 0).ToString();
            if (lb != lb_calibthicknesspointnum.Text)
            {
                ShowSuccessTip("点数添加成功！");
            }
        }
        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (Convert.ToDouble(lb_calibthicknesspointnum.Text) < 1)
            {
                ShowErrorTip("请先添加点数后，再保存设定");
                return;
            }
            string k0 = nud_k0.Text;string k1 = nud_k1.Text;
            int groupIndex = 0;
            //标定模式，K0为常数项（offset），K1为一次项系数，K2为二次项系数
            //写入点数为1时，为offset补偿
            //写入点数为2时，拟合直线
            //写入点数为3或以上时，拟合二元多次曲线
            double[] coef = new double[3];
            CF_UserInterface.hps_doDoubleChannelThicknessCal(Auto.deviceHandle1, groupIndex, coef);
            nud_k0.Value = coef[0];
            nud_k1.Value = coef[1];
            nud_k2.Value = coef[2];
            CF_UserInterface.HPS_CF_SaveSetting(Auto.deviceHandle1);
            Delay(1500);
            ShowSuccessTip("保存标定参数成功！");
            

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
        private void uiButton4_Click(object sender, EventArgs e)
        {
            string inputPassword = "";
            if (this.InputPasswordDialog(ref inputPassword))
            {
                if (inputPassword != ConfigurationManager.AppSettings["Password"])
                {
                    ShowErrorTip("密码错误!");
                    return;
                }
            }
            else { ShowWarningTip("取消!"); return; }
            if (ShowAskDialog("是否将以上设定内容恢复出厂设置？"))
            {
                using (StreamReader sr = new StreamReader($@".\Config\configInitial.cfg"))
                {
                    mode = Convert.ToInt32(sr.ReadLine());
                    setThickness = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown16.Value = setThickness;
                    setMax = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown2.Value = setMax;
                    setMin = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown3.Value = setMin;
                    setMax_Min = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown1.Value = setMax_Min;
                    setLocation1 = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown4.Value = setLocation1;
                    setLocation2 = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown5.Value = setLocation2;
                    setLocation3 = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown7.Value = setLocation3;
                    setLocation4 = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown6.Value = setLocation4;
                    lotNum = sr.ReadLine(); uiTextBox8.Text = lotNum;
                    batchNum = sr.ReadLine(); uiTextBox6.Text = batchNum;
                    if (mode == 0)
                    {
                        uiComboBox1.SelectedIndex = 0;
                        uiDoubleUpDown2.Enabled = false;
                    }
                    else if (mode == 1)
                    {
                        uiComboBox1.SelectedIndex = 1;
                        uiDoubleUpDown1.Enabled = false;
                    }
                    points = Convert.ToInt32(sr.ReadLine());
                    if (points == 9)
                    {
                        uiComboBox2.SelectedIndex = 0;
                        uiDoubleUpDown6.Enabled = false;

                    }
                    else if (points == 12)
                    {
                        uiComboBox2.SelectedIndex = 1;
                        uiDoubleUpDown6.Enabled = true;
                    }
                    lineChartRange = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown8.Value = lineChartRange;
                     barChartRange = Convert.ToDouble(sr.ReadLine()); uiDoubleUpDown9.Value = barChartRange;
                }
                using (StreamWriter sw = new StreamWriter($@".\Config\config.cfg"))
                {
                    sw.WriteLine(uiComboBox1.SelectedIndex == 0 ? "0" : "1");
                    sw.WriteLine(uiDoubleUpDown16.Value.ToString());
                    sw.WriteLine(uiDoubleUpDown2.Value.ToString());
                    sw.WriteLine(uiDoubleUpDown3.Value.ToString());
                    sw.WriteLine(uiDoubleUpDown1.Value.ToString());
                    sw.WriteLine(uiDoubleUpDown4.Value.ToString());
                    sw.WriteLine(uiDoubleUpDown5.Value.ToString());
                    sw.WriteLine(uiDoubleUpDown7.Value.ToString());
                    sw.WriteLine(uiDoubleUpDown6.Value.ToString());
                    sw.WriteLine(uiTextBox8.Text);
                    sw.WriteLine(uiTextBox6.Text);
                    sw.WriteLine(uiComboBox2.SelectedIndex == 0 ? "9" : "12");
                    sw.WriteLine(uiDoubleUpDown8.Value.ToString());
                    sw.WriteLine(uiDoubleUpDown9.Value.ToString());
                }
                ShowSuccessTip("恢复出厂设置成功!");
            }
            else
            {
            }
            
        }

		private void uiButton5_Click(object sender, EventArgs e)
		{
            if (Convert.ToDouble(lb_calibthicknesspointnum2.Text) < 1)
            {
                ShowErrorTip("请先添加有效点数，再保存设定");
                return;
            }
            string k0 = nud_k02.Text; string k1 = nud_k12.Text;

            int groupIndex = 0;
            //标定模式，K0为常数项（offset），K1为一次项系数，K2为二次项系数
            //写入点数为1时，为offset补偿
            //写入点数为2时，拟合直线
            //写入点数为3或以上时，拟合二元多次曲线
            double[] coef = new double[3];
            CF_UserInterface.hps_doDoubleChannelThicknessCal(Auto.deviceHandle2, groupIndex, coef);
            nud_k02.Value = coef[0];
            nud_k12.Value = coef[1];
            nud_k22.Value = coef[2];
            CF_UserInterface.HPS_CF_SaveSetting(Auto.deviceHandle2);
            Delay(1500);
            ShowSuccessTip("保存标定参数成功！");
        }

		private void uiButton6_Click(object sender, EventArgs e)
		{
            if (Auto.measuringStatus == "正在测厚……") return;
            int groupIndex = 0;
            string lb = lb_calibthicknesspointnum2.Text;
            //写入真实厚度值，同时记录测量值
            CF_UserInterface.hps_setDoubleChannelThicknessSamplePoint(Auto.deviceHandle2, groupIndex, (float)nud_calibthickness2.Value);
            //更新当前写入点数
            lb_calibthicknesspointnum2.Text = CF_UserInterface.hps_getDoubleChannelThicknessSamplePoint(Auto.deviceHandle2, groupIndex).ToString();
            if (lb != lb_calibthicknesspointnum2.Text)
            {
                ShowSuccessTip("点数添加成功！");
            }
        }

        private void uiButton8_Click(object sender, EventArgs e)
        {
            if (Auto.measuringStatus == "正在测厚……") return;
            int groupIndex = 0;
            string lb = lb_calibthicknesspointnum3.Text;
            //写入真实厚度值，同时记录测量值
            CF_UserInterface.hps_setDoubleChannelThicknessSamplePoint(Auto.deviceHandle3, groupIndex, (float)nud_calibthickness3.Value);
            //更新当前写入点数
            lb_calibthicknesspointnum3.Text = CF_UserInterface.hps_getDoubleChannelThicknessSamplePoint(Auto.deviceHandle3, groupIndex).ToString();
            if (lb != lb_calibthicknesspointnum3.Text)
            {
                ShowSuccessTip("点数添加成功！");
            }
        }

        private void uiButton7_Click(object sender, EventArgs e)
        {
            if (Convert.ToDouble(lb_calibthicknesspointnum3.Text) < 1)
            {
                ShowErrorTip("请先添加有效点数，再保存设定");
                return;
            }
            string k0 = nud_k03.Text; string k1 = nud_k13.Text;
            int groupIndex = 0;
            //标定模式，K0为常数项（offset），K1为一次项系数，K2为二次项系数
            //写入点数为1时，为offset补偿
            //写入点数为2时，拟合直线
            //写入点数为3或以上时，拟合二元多次曲线
            double[] coef = new double[3];
            CF_UserInterface.hps_doDoubleChannelThicknessCal(Auto.deviceHandle3, groupIndex, coef);
            nud_k03.Value = coef[0];
            nud_k13.Value = coef[1];
            nud_k23.Value = coef[2];

            CF_UserInterface.HPS_CF_SaveSetting(Auto.deviceHandle3);
            Delay(1500);
            ShowSuccessTip("保存标定参数成功！");
            //获取标定系数
            ret = CF_UserInterface.hps_getDoubleChannelThicknessK(Auto.deviceHandle1, 0, ThicnessK_1);
            ret = CF_UserInterface.hps_getDoubleChannelThicknessK(Auto.deviceHandle2, 0, ThicnessK_2);
            ret = CF_UserInterface.hps_getDoubleChannelThicknessK(Auto.deviceHandle3, 0, ThicnessK_3);
            try
            {
                using (StreamWriter sw = new StreamWriter($@".\data\" + "保存后标定系数.txt"))
                {
                    sw.WriteLine("保存后标定系数");
                    sw.WriteLine("1号，K0=" + ThicnessK_1[0].ToString() + ",K1=" + ThicnessK_1[1].ToString() + ",K2=" + ThicnessK_1[2].ToString());
                    sw.WriteLine("2号，K0=" + ThicnessK_2[0].ToString() + ",K1=" + ThicnessK_2[1].ToString() + ",K2=" + ThicnessK_2[2].ToString());
                    sw.WriteLine("3号，K0=" + ThicnessK_3[0].ToString() + ",K1=" + ThicnessK_3[1].ToString() + ",K2=" + ThicnessK_3[2].ToString());
                }
            }
            catch { }
        }
        public static double[] ThicnessK_1 = new double[3]; public static double[] ThicnessK_2 = new double[3]; public static double[] ThicnessK_3 = new double[3];

        private void uiButton9_Click(object sender, EventArgs e)
        {
            if (Auto.measuringStatus == "正在测厚……") return;
            uiButton9.Enabled = false;
            CF_UserInterface.HPS_CF_StartSample(Auto.deviceHandle1, true);
            Delay(1000);
            uiButton9.Enabled =true;
            MC_ResultDataTypeDef_t[] doubleResult = new MC_ResultDataTypeDef_t[2];
            int len;

            StatusTypeDef ret = CF_UserInterface.HPS_CF_GetLatestResult_MC(Auto.deviceHandle1, out doubleResult, out len);
            ret = CF_UserInterface.HPS_CF_StartSample(Auto.deviceHandle1, false);
            if (ret != StatusTypeDef.Status_Succeed)
            {
                uiTextBox1.Text = "Failed: " + ret;
                return;
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    uiTextBox1.Text = doubleResult[i].thickness.ToString("0.000");
                }
            }
        }

        private void uiButton10_Click(object sender, EventArgs e)
        {
            if (Auto.measuringStatus == "正在测厚……") return;
            uiButton10.Enabled= false;
            CF_UserInterface.HPS_CF_StartSample(Auto.deviceHandle2, true);
            Delay(1000);
            uiButton10.Enabled = true;
            MC_ResultDataTypeDef_t[] doubleResult = new MC_ResultDataTypeDef_t[2];
            int len;

            StatusTypeDef ret = CF_UserInterface.HPS_CF_GetLatestResult_MC(Auto.deviceHandle2, out doubleResult, out len);
            CF_UserInterface.HPS_CF_StartSample(Auto.deviceHandle2, false);

            if (ret != StatusTypeDef.Status_Succeed)
            {
                uiTextBox2.Text = "Failed: " + ret;
                return;
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    uiTextBox2.Text = doubleResult[i].thickness.ToString("0.000");
                }
            }
        }

        private void uiButton11_Click(object sender, EventArgs e)
        {
            if (Auto.measuringStatus == "正在测厚……") return;
            uiButton11.Enabled= false;
            CF_UserInterface.HPS_CF_StartSample(Auto.deviceHandle3, true);
            Delay(1000);
            uiButton11.Enabled = true;
            MC_ResultDataTypeDef_t[] doubleResult = new MC_ResultDataTypeDef_t[2];
            int len;

            StatusTypeDef ret = CF_UserInterface.HPS_CF_GetLatestResult_MC(Auto.deviceHandle3, out doubleResult, out len);
            CF_UserInterface.HPS_CF_StartSample(Auto.deviceHandle3, false);
            if (ret != StatusTypeDef.Status_Succeed)
            {
                uiTextBox3.Text = "Failed: " + ret;
                return;
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    uiTextBox3.Text = doubleResult[i].thickness.ToString("0.000");
                }
            }
        }
        
		private void uiButton12_Click(object sender, EventArgs e)
		{
            int groupIndex = 0;
            //复位双头测厚校准系数（0，1，0）,同时清空写入的点数
            CF_UserInterface.hps_clearDoubleChannelThicknessSamplePoint(Auto.deviceHandle1, groupIndex);
            nud_k0.Value = 0;
            nud_k1.Value = 1;
            nud_k2.Value = 0;
            lb_calibthicknesspointnum.Text = CF_UserInterface.hps_getDoubleChannelThicknessSamplePoint(Auto.deviceHandle1, groupIndex).ToString();
            if (lb_calibthicknesspointnum.Text == "0")
			{
                ShowSuccessTip("复位成功");
            }
        }

		private void uiButton13_Click(object sender, EventArgs e)
		{
            int groupIndex = 0;
            //复位双头测厚校准系数（0，1，0）,同时清空写入的点数
            CF_UserInterface.hps_clearDoubleChannelThicknessSamplePoint(Auto.deviceHandle2, groupIndex);
            nud_k02.Value = 0;
            nud_k12.Value = 1;
            nud_k22.Value = 0;
            lb_calibthicknesspointnum2.Text = CF_UserInterface.hps_getDoubleChannelThicknessSamplePoint(Auto.deviceHandle2, groupIndex).ToString();
            if (lb_calibthicknesspointnum2.Text == "0")
            {
                ShowSuccessTip("复位成功");
            }
        }

		private void uiButton14_Click(object sender, EventArgs e)
		{
            int groupIndex = 0;
            //复位双头测厚校准系数（0，1，0）,同时清空写入的点数
            CF_UserInterface.hps_clearDoubleChannelThicknessSamplePoint(Auto.deviceHandle3, groupIndex);
            nud_k03.Value = 0;
            nud_k13.Value = 1;
            nud_k23.Value = 0;
            lb_calibthicknesspointnum3.Text = CF_UserInterface.hps_getDoubleChannelThicknessSamplePoint(Auto.deviceHandle3, groupIndex).ToString();
            if (lb_calibthicknesspointnum3.Text == "0")
            {
                ShowSuccessTip("复位成功");
            }
        }

		private void uiPanel1_Click(object sender, EventArgs e)
		{

		}

		private void uiButton15_Click(object sender, EventArgs e)
		{
            uiPanel1.Visible = false;
		}
	}
}

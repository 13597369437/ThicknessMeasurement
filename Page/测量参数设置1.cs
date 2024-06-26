using Sunny.UI;
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
    public partial class 测量参数设置1 : UIPage
    {
        IFreeSql fsql = DB.MySQL;
        public 测量参数设置1()
        {
            InitializeComponent();
        }

        private void uiTrackBar1_ValueChanged(object sender, EventArgs e)
        {
            uiLabel1.Text = uiTrackBar1.Value + " %";
        }

        private void uiTrackBar2_ValueChanged(object sender, EventArgs e)
        {
            uiLabel2.Text = uiTrackBar2.Value + " %";
        }

        private void uiTrackBar3_ValueChanged(object sender, EventArgs e)
        {
            uiLabel4.Text = uiTrackBar3.Value + " %";
        }

        private void uiTrackBar4_ValueChanged(object sender, EventArgs e)
        {
            uiLabel6.Text = uiTrackBar4.Value + " %";
        }

        private void 测量参数设置1_Initialize(object sender, EventArgs e)
        {
            if (DataClass.peizhivalues[11] == "9")
            {
                uiPanel1.Visible = false;
                uiIntegerUpDown1.Maximum = 9;
            }
            else 
            {
                uiPanel1.Visible = true;
                uiPanel1.RectColor = Color.White;
                uiIntegerUpDown1.Maximum = 12;
            }

            if(DataClass.peizhivalues[10] == "1")
            {
                uiDoubleUpDown1.Enabled = false;
                uiDoubleUpDown2.Enabled = true;
                uiDoubleUpDown3.Enabled = true;
                uiTrackBar5.Enabled = true;
            }
            else
            {
                uiDoubleUpDown1.Enabled = true;
                uiDoubleUpDown2.Enabled = false;
                uiDoubleUpDown3.Enabled = false;
                uiTrackBar5.Enabled = false;
            }

            uiGroupBox3.Visible = DataClass.peizhivalues[10] == "1" && DataClass.peizhivalues[13] == "1";

            if (DataClass.peizhivalues[2] == "1")
            {
                uiPanel2.Visible = true;
                uiPanel2.RectColor = Color.White;
            }
            else
            {
                uiPanel2.Visible = false;
            }

           

            var clcs = fsql.Select<测量参数>().ToList(t => t.值);
            uiDoubleUpDown16.Value = double.Parse(clcs[0]);//产品厚度
            uiDoubleUpDown1.Value = double.Parse(clcs[1]);//极差
            uiDoubleUpDown2.Value = double.Parse(clcs[2]);//上限
            uiDoubleUpDown3.Value = double.Parse(clcs[3]);//下限
            uiTrackBar1.Value = int.Parse(clcs[5]);//取样点1
            uiTrackBar2.Value = int.Parse(clcs[6]);//取样点2
            uiTrackBar3.Value = int.Parse(clcs[7]);//取样点3
            uiTrackBar4.Value = int.Parse(clcs[8]);//取样点4
            uiIntegerUpDown1.Value = int.Parse(clcs[9]);//NG点数
            uiDoubleUpDown7.Value = double.Parse(clcs[10]);//折线图精度
            uiSwitch1.Active = clcs[11] == "1" ? true : false;//整板取样
            uiDoubleUpDown4.Value = double.Parse(clcs[12]);//NG百分比
            uiDoubleUpDown5.Value = double.Parse(clcs[13]);//固定差值
            uiTrackBar5.Value = int.Parse(clcs[4]);//上下限百分比
        }

        List<string> data = new List<string>();
        private void uiButton1_Click(object sender, EventArgs e)
        {
            data.Clear();
            data.Add(uiDoubleUpDown16.Value.ToString());//产品厚度
            data.Add(uiDoubleUpDown1.Value.ToString());//极差
            data.Add(uiDoubleUpDown2.Value.ToString());//上限
            data.Add(uiDoubleUpDown3.Value.ToString());//下限
            data.Add(uiTrackBar5.Value.ToString());//上下限百分比
            data.Add(uiTrackBar1.Value.ToString());//取样点1
            data.Add(uiTrackBar2.Value.ToString());//取样点2
            data.Add(uiTrackBar3.Value.ToString());//取样点3
            data.Add(uiTrackBar4.Value.ToString());//取样点4
            data.Add(uiIntegerUpDown1.Value.ToString());//NG点数
            data.Add(uiDoubleUpDown7.Value.ToString());//折线图精度
            data.Add(uiSwitch1.Active ? "1" : "0");//识别中间点
            data.Add(uiDoubleUpDown4.Value.ToString());//NG百分比

            for (int i=0; i<data.Count; i++)
            {
                fsql.Update<测量参数>().Set(s => s.值 == data[i]).Where(w => w.id == i).ExecuteAffrowsAsync();
            }
            
          

        }

        private void uiTrackBar5_ValueChanged(object sender, EventArgs e)
        {
            uiDoubleUpDown2.Value = uiDoubleUpDown16.Value + uiDoubleUpDown16.Value * uiTrackBar5.Value / 100;
            uiDoubleUpDown3.Value = uiDoubleUpDown16.Value - uiDoubleUpDown16.Value * uiTrackBar5.Value / 100;
            uiLabel12.Text = uiTrackBar5.Value + " %";
        }

        private void uiDoubleUpDown5_ValueChanged(object sender, double value)
        {
            uiTrackBar5.Value = (int)(uiDoubleUpDown5.Value / uiDoubleUpDown16.Value * 100);
        }
    }
}

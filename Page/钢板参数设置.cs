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
    public partial class 钢板参数设置 : UIPage
    {
        IFreeSql fsql = DB.MySQL;
        public 钢板参数设置()
        {
            InitializeComponent();
        }

        void func1()
        {
            var gbids = fsql.Select<钢板参数>().ToList(t=>t.钢板ID).ToArray();

            uiComboBox1.Items.Clear();
            //uiComboBox1.Items.Add("");
            uiComboBox1.Items.AddRange(gbids);

        }

        private void 测量参数设置2_Initialize(object sender, EventArgs e)
        {
            func1();
            if (uiComboBox1.Items.Count > 0)
                uiComboBox1.SelectedIndex = 0;

        }

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uiComboBox1.Text == "") return;

            var gbcs = fsql.Select<钢板参数>().Where(w => w.钢板ID == uiComboBox1.Text).ToList();

            uiTextBox1.Text = gbcs[0].钢板名称;
            uiDoubleUpDown16.Value = gbcs[0].产品厚度;
            uiTrackBar1.Value = gbcs[0].自动识别范围;

        }

        //删除
        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (uiComboBox1.Text == "") return;
            fsql.Delete<钢板参数>().Where(w => w.钢板ID == uiComboBox1.Text).ExecuteAffrowsAsync();
            ShowSuccessTip($"删除：{uiComboBox1.Text} 成功");
        }

        private void uiButton10_Click(object sender, EventArgs e)
        {
            //查询当前配方名称是否存在
            if (uiComboBox1.Items.Contains(uiComboBox1.Text))
            {
                //更新
                fsql.Update<钢板参数>()
                    .Set(s => s.钢板名称 == uiTextBox1.Text)
                    .Set(s => s.产品厚度 == uiDoubleUpDown16.Value)
                    .Set(s => s.自动识别范围 == uiTrackBar1.Value)
                    .Where(w => w.钢板ID == uiComboBox1.Text).ExecuteAffrowsAsync();
            }
            else
            {
                //新增
                var gb = new 钢板参数
                {
                    钢板ID = uiComboBox1.Text,
                    钢板名称 = uiTextBox1.Text,
                    产品厚度 = uiDoubleUpDown16.Value,
                    自动识别范围 = uiTrackBar1.Value
                };

                fsql.Insert(gb).ExecuteAffrowsAsync();
            }
        }


        private void uiTrackBar1_ValueChanged(object sender, EventArgs e)
        {
            uiLabel3.Text = uiTrackBar1.Value + " %";
        }
    }
}

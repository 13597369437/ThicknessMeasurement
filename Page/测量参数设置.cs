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

namespace ThicknessMeasurement.Page
{
    public partial class 测量参数设置 : UIPage
    {
        IFreeSql fsql = DB.MySQL;
        public 测量参数设置()
        {
            InitializeComponent();
        }

        void func1()
        {
            var penames = fsql.Select<测量参数>().ToList();
            var pfids = penames.GroupBy(g => g.值).Select(s => s.Key).Where(w => w != null).ToList();

            uiComboBox1.Items.Clear();
            //uiComboBox1.Items.Add("");
            uiComboBox1.Items.AddRange(pfids.ToArray());

        }

        private void uiButton10_Click(object sender, EventArgs e)
        {
            if (DataClass.Power < 2)
            {
                ShowWarningTip("权限不足！");
                return;
            }

            ////查询当前配方名称是否存在
            //if (uiComboBox1.Items.Contains(uiComboBox1.Text))
            //{
            //    //更新
            //    for (int i = 0; i < uiDataGridView1.Rows.Count; i++)
            //    {
            //        string value = uiDataGridView1.Rows[i].Cells[2].Value.ToString();
            //        int id = int.Parse(uiDataGridView1.Rows[i].Cells[0].Value.ToString());
            //        fsql.Update<测量参数>().Set(s => s.值 == value).Where(w => w.id == id).ExecuteAffrowsAsync();
            //    }
            //}
            //else
            //{
            //    //新增
            //    for (int i = 0; i < uiDataGridView1.Rows.Count; i++)
            //    {
            //        var pf = new 测量参数
            //        {
            //            配方ID = uiComboBox1.Text,
            //            配方名称 = uiTextBox1.Text,
            //            项目 = uiDataGridView1.Rows[i].Cells[1].Value.ToString(),
            //            值 = uiDataGridView1.Rows[i].Cells[2].Value.ToString(),
            //            备注 = uiDataGridView1.Rows[i].Cells[3].Value.ToString()
            //        };
            //        fsql.Insert(pf).ExecuteAffrowsAsync();

            //    }
            //}

            //更新
            for (int i = 0; i < uiDataGridView1.Rows.Count; i++)
            {
                string value = uiDataGridView1.Rows[i].Cells[2].Value.ToString();
                int id = int.Parse(uiDataGridView1.Rows[i].Cells[0].Value.ToString());
                string name = uiTextBox1.Text;
                fsql.Update<测量参数>()
                    .Set(s => s.值 == value)
                    .Where(w => w.id == id).ExecuteAffrowsAsync();
            }

            func1();

            DataClass.readpeifang(fsql);
        }

        private void 测量参数设置_Initialize(object sender, EventArgs e)
        {
            func1();
            if (uiComboBox1.Items.Count > 0)
                uiComboBox1.SelectedIndex = 0;
        }

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uiComboBox1.Text == "") return;

            var dt = fsql.Select<测量参数>().Where(w => w.值== uiComboBox1.Text).ToDataTable();

            uiTextBox1.Text = dt.Rows[0][2].ToString();

            dt.Columns.RemoveAt(1);
            dt.Columns.RemoveAt(1);

            uiDataGridView1.DataSource = dt;


            uiDataGridView1.Columns[0].Width = 100;
            uiDataGridView1.Columns[1].Width = 220;
            uiDataGridView1.Columns[2].Width = 120;
            uiDataGridView1.Columns[3].Width = 420;
        }
    }
}

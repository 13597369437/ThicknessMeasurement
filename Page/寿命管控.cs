using NPOI.SS.Formula.Functions;
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
    public partial class 寿命管控 : UIPage
    {
        IFreeSql fsql = DB.MySQL;
        public 寿命管控()
        {
            InitializeComponent();
        }

        private void 钢板使用记录_Initialize(object sender, EventArgs e)
        {
            funt1();

            if (tabPage2.Parent != null)
                tabPage2.Parent = null;
            else
                tabPage2.Parent = uiTabControl1;
        }

        private void uiDataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            var dgv = sender as UIDataGridView;

            if (e.RowIndex > -1)
            {
                if (dgv.Rows[e.RowIndex].Cells["剩余次数"].Value != null)
                {

                    if (int.Parse(dgv.Rows[e.RowIndex].Cells["剩余次数"].Value.ToString()) == 0)
                    {
                        dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                    }
                    else if (int.Parse(dgv.Rows[e.RowIndex].Cells["剩余次数"].Value.ToString()) <= 3)
                    {
                        dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                    }
                    else
                    {
                        dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            if(uiTextBox1.Text=="")
            {
                ShowWarningTip("钢板ID不能为空！");
                return;
            }
            var gb = new 钢板记录()
            {
                钢板ID = uiTextBox1.Text,
                设定次数 = uiIntegerUpDown1.Value,
                生产次数 = 0
            };

            fsql.Insert(gb).ExecuteAffrowsAsync();

            funt1();
        }

        void funt1()
        {
            var dt = fsql.Select<钢板记录>().ToDataTable();
            uiDataGridView1.DataSource = null;
            uiDataGridView1.DataSource = dt;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][4] = int.Parse(dt.Rows[i][2].ToString()) - int.Parse(dt.Rows[i][3].ToString());
            }
        }
    }
}

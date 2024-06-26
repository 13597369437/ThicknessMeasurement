using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Sunny.UI;

namespace ThicknessMeasurement
{
	public partial class History : UIPage
	{
        IFreeSql fsql = DB.MySQL;

        public History()
		{
			InitializeComponent();

          
        }

        DataTable ProductionLog = new DataTable();
        private void History_Load(object sender, EventArgs e)
		{

            
        }

        //导出到csv文件
        public void DataGridViewToExcel(DataGridView dgv)
        {
            //程序实例化SaveFileDialog控件，并对该控件相关参数进行设置
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Execl files (*.csv)|*.csv";
            dlg.FilterIndex = 0;
            dlg.RestoreDirectory = true;
            dlg.CreatePrompt = true;
            dlg.Title = "保存为csv文件";
            //以上过程也可以通过添加控件，再设置控件属性完成，此处用程序编写出来了，在移植时就可摆脱控件的限制

            if (dlg.ShowDialog() == DialogResult.OK)//打开SaveFileDialog控件，判断返回值结果
            {
                Stream myStream;//流变量
                myStream = dlg.OpenFile();//返回SaveFileDialog控件打开的文件，并将所选择的文件转化成流
                StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));//将选择的文件流生成写入流
                string columnTitle = "";
                try
                {
                    //写入列标题    
                    for (int i = 0; i < dgv.ColumnCount; i++)
                    {
                        if (i > 0)
                        {
                            columnTitle += ",";
                        }
                        columnTitle += dgv.Columns[i].HeaderText;//符号 ， 的添加，在保存为Excel时就以 ， 分成不同的列了
                    }

                    sw.WriteLine(columnTitle);//将内容写入文件流中

                    //写入列内容    
                    for (int j = 0; j < dgv.Rows.Count; j++)
                    {
                        string columnValue = "";
                        for (int k = 0; k < dgv.Columns.Count; k++)
                        {
                            if (k > 0)
                            {
                                columnValue += ",";
                            }
                            if (dgv.Rows[j].Cells[k].Value == null)
                                columnValue += "";
                            else if (dgv.Rows[j].Cells[k].Value.ToString().Contains(","))
                            {
                                columnValue += "\"" + dgv.Rows[j].Cells[k].Value.ToString().Trim() + "\"";//将单元格中的，号转义成文本
                            }
                            else
                            {
                                columnValue += dgv.Rows[j].Cells[k].Value.ToString().Trim() + "\t";//\t 横向跳格
                            }
                        }//获得写入到列中的值
                        sw.WriteLine(columnValue);//将内容写入文件流中
                    }
                    sw.Close();//关闭写入流
                    myStream.Close();//关闭流变量
                    ShowSuccessTip("导出表格成功！");
                }
                catch (Exception e)
                {
                    ShowErrorTip("导出表格失败！");
                }
                finally
                {
                    sw.Close();
                    myStream.Close();
                }
            }
            else
            {
                ShowInfoTip("取消导出表格操作!");
            }
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            ProductionLog = fsql.Select<productionlog>().Where(w =>
            (w.MaterialNum == textBox3.Text || textBox3.Text == "")
            && (w.钢板ID == uiTextBox1.Text || uiTextBox1.Text == "")
            &&!(textBox3.Text == "" && uiTextBox1.Text == "")
            || (textBox3.Text == "" && uiTextBox1.Text == ""
            && Convert.ToDateTime(w.datee) >= textBox1.Value
            && Convert.ToDateTime(w.datee) <= textBox2.Value)).ToDataTable();

            dataGridView1.DataSource = ProductionLog;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            updatedgv();
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {

            textBox1.Text = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");

            ProductionLog = fsql.Select<productionlog>().Where(w =>
           Convert.ToDateTime(w.datee) >= textBox1.Value
           && Convert.ToDateTime(w.datee) <= textBox2.Value).ToDataTable();

            dataGridView1.DataSource = ProductionLog;

            updatedgv();

        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            DataGridViewToExcel(dataGridView1);
        }

        private void History_Initialize(object sender, EventArgs e)
        {
            textBox1.Text = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");

            ProductionLog = fsql.Select<productionlog>().Where(w =>
           Convert.ToDateTime(w.datee) >= textBox1.Value
           && Convert.ToDateTime(w.datee) <= textBox2.Value).ToDataTable();

            dataGridView1.DataSource = ProductionLog;
            dataGridView1.Columns[0].HeaderText = "序号";
            dataGridView1.Columns[1].HeaderText = "日期";
            dataGridView1.Columns[2].HeaderText = "料号";
            dataGridView1.Columns[3].HeaderText = "批次号";
            dataGridView1.Columns[4].HeaderText = "钢板ID";
            dataGridView1.Columns[5].HeaderText = "设定厚度";
            dataGridView1.Columns[6].HeaderText = "设定上限";
            dataGridView1.Columns[7].HeaderText = "设定下限";
            dataGridView1.Columns[8].HeaderText = "设定极差";
            dataGridView1.Columns[9].HeaderText = "测厚结果";
            dataGridView1.Columns[10].HeaderText = "最大值";
            dataGridView1.Columns[11].HeaderText = "最小值";
            dataGridView1.Columns[12].HeaderText = "极差";
            dataGridView1.Columns[13].HeaderText = "平均值";
            dataGridView1.Columns[14].HeaderText = "总数";
            dataGridView1.Columns[15].HeaderText = "1号头取样点1";
            dataGridView1.Columns[16].HeaderText = "1号头取样点2";
            dataGridView1.Columns[17].HeaderText = "1号头取样点3";
            dataGridView1.Columns[18].HeaderText = "1号头取样点4";
            dataGridView1.Columns[19].HeaderText = "2号头取样点1";
            dataGridView1.Columns[20].HeaderText = "2号头取样点2";
            dataGridView1.Columns[21].HeaderText = "2号头取样点3";
            dataGridView1.Columns[22].HeaderText = "2号头取样点4";
            dataGridView1.Columns[23].HeaderText = "3号头取样点1";
            dataGridView1.Columns[24].HeaderText = "3号头取样点2";
            dataGridView1.Columns[25].HeaderText = "3号头取样点3";
            dataGridView1.Columns[26].HeaderText = "3号头取样点4";

            dataGridView1.Columns[14].Visible = false;


            if(DataClass.peizhivalues[11] == "9")
            {
                dataGridView1.Columns[18].Visible = false;
                dataGridView1.Columns[22].Visible = false;
                dataGridView1.Columns[26].Visible = false;
            }
            else
            {
                dataGridView1.Columns[18].Visible = true;
                dataGridView1.Columns[22].Visible = true;
                dataGridView1.Columns[26].Visible = true;
            }

            updatedgv();


            textBox1.Value = DateTime.Now.AddMonths(-3);
            textBox2.Value = DateTime.Now; 

        }

        void updatedgv()
        {
            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Descending);

            double yield = 0;
            for (int i = 0; i < ProductionLog.Rows.Count; i++)
            {
                if (ProductionLog.Rows[i]["result"].ToString().Trim() == "OK")
                {
                    yield++;
                }
            }
            yields.Text = ((yield / ProductionLog.Rows.Count) * 100).ToString("0.00") + "%";

            uiTextBox2.Text = ProductionLog.Rows.Count.ToString();//总数
            uiTextBox3.Text = (ProductionLog.Rows.Count - yield).ToString();//NG数
        }
    }
}

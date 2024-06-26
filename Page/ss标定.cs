using NPOI.SS.UserModel;
using Org.BouncyCastle.Asn1.Pkcs;
using SG;
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
using static ThicknessMeasurement.LKIF2;

namespace ThicknessMeasurement.Page
{
    public partial class ss标定 : UIPage
    {
        IFreeSql fsql = DB.MySQL;
        public ss标定()
        {
            InitializeComponent();
        }

        //标定读取测量值
        private void uiButton1_Click(object sender, EventArgs e)
        {
            UIButton btn = (UIButton)sender;
            SG.RC rc = new SG.RC();
            SGIF_FLOATVALUE_OUT mData;
            switch (btn.Tag.ToString())
            {
                case "1":
                    rc = SGLinkFuc.SGIF_GetCalcDataSingle(0, 5, out mData);
                    uiDoubleUpDown1.Value =mData.Value;
                    break;
                case "2":
                    rc = SGLinkFuc.SGIF_GetCalcDataSingle(0, 6, out mData);
                    uiDoubleUpDown5.Value = mData.Value;
                    break;
                case "3":
                    rc = SGLinkFuc.SGIF_GetCalcDataSingle(1, 3, out mData);
                    uiDoubleUpDown9.Value = mData.Value;
                    break;
                default:
                    break;
            }
            if (rc == SG.RC.RC_OK)
            {
                ShowSuccessTip("测量成功！");
            }
            else
            {
                ShowErrorTip("测量失败！");
            }
        }

        private string FloatResultValueOutToText(LKIF2.LKIF_FLOATVALUE_OUT FloatValue)
        {
            if (FloatValue.FloatResult == LKIF2.LKIF_FLOATRESULT.LKIF_FLOATRESULT_VALID)
            {
                return FloatValue.value.ToString();
            }
            else if (FloatValue.FloatResult == LKIF2.LKIF_FLOATRESULT.LKIF_FLOATRESULT_RANGEOVER_P)
            {
                return "+FFFFFFF";
            }
            else if (FloatValue.FloatResult == LKIF2.LKIF_FLOATRESULT.LKIF_FLOATRESULT_RANGEOVER_N)
            {
                return "-FFFFFFF";
            }
            else if (FloatValue.FloatResult == LKIF2.LKIF_FLOATRESULT.LKIF_FLOATRESULT_WAITING)
            {
                return "--------";
            }
            else if (FloatValue.FloatResult == LKIF2.LKIF_FLOATRESULT.LKIF_FLOATRESULT_ALARM)
            {
                return "alarm";
            }
            else
            {
                return "Invalid";
            }
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            if (DataClass.Power < 2)
            {
                ShowWarningTip("权限不足！");
                return;
            }
            SGLinkFuc.SGIF_SetZeroSingle(0, 1,true);
            SGLinkFuc.SGIF_SetZeroSingle(0, 2, true);
            int offset = (int)((double)uiDoubleUpDown2.Value * 10000);
            SG.RC rc = new SG.RC();
            rc =SGLinkFuc.SGIF_SetOffset(0, 5, offset);
            if (rc == SG.RC.RC_OK)
            {
                ShowSuccessTip("标定成功！");
            }
            else
            {
                ShowErrorTip("标定失败！");
            }

        }

        private bool GetTextData(UIDoubleUpDown dud, ref int value)
        {
            bool result = false;
            try
            {
                result = true;
                value = (int)(dud.Value * 1000);
                return result;
            }
            catch
            {
                MessageBox.Show("illegal input", Application.ProductName);
                return false;
            }
        }

        private void uiButton15_Click(object sender, EventArgs e)
        {

        }

 

        private void 基恩士配置_Initialize(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                foreach (Control control2 in control.Controls)
                {
                    if (control2 is UIDoubleUpDown)
                    {
                        var dud = (UIDoubleUpDown)control2;
                        dud.DecimalPlaces = int.Parse(DataClass.peizhivalues[9]);
                    }
                }
            }
            
        }

        private void uiButton11_Click(object sender, EventArgs e)
        {

        }

        private void uiButton10_Click(object sender, EventArgs e)
        {
            int offset = (int)((double)uiDoubleUpDown2.Value * 10000);
            SGLinkFuc.SGIF_SetOffset(0, 6,offset);
        }

        private void uiButton11_Click_1(object sender, EventArgs e)
        {
            SGLinkFuc.SGIF_SetResetSingle(0, 3);
            SGLinkFuc.SGIF_SetResetSingle(0, 4);
            int offset = (int)((double)uiDoubleUpDown2.Value * 10000);
            SGLinkFuc.SGIF_SetOffset(0, 6, offset);
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {
            int outNo = 6;
            int Offset =1;
            SGLinkFuc.SGIF_GetOffset(0, out outNo, out  Offset);
            if (DataClass.Power < 2)
            {
                ShowWarningTip("权限不足！");
                return;
            }
            SGLinkFuc.SGIF_SetZeroSingle(0, 3,true);
            SGLinkFuc.SGIF_SetZeroSingle(0, 4, true);
            int offset = (int)((double)uiDoubleUpDown6.Value * 10000);

            //int offset =(int)uiDoubleUpDown2.Value;
            SG.RC rc = new SG.RC();
            rc = SG.RC.RC_ERR_DEVID_OVER;
            rc = SGLinkFuc.SGIF_SetOffset(0, 6, offset);
            
            if (rc == SG.RC.RC_OK)
            {
                ShowSuccessTip("标定成功！");
            }
            else
            {
                ShowErrorTip("标定失败！");
            }
        }

        private void uiButton5_Click(object sender, EventArgs e)
        {
            if (DataClass.Power < 2)
            {
                ShowWarningTip("权限不足！");
                return;
            }
            SGLinkFuc.SGIF_SetZeroSingle(1, 1, true);
            SGLinkFuc.SGIF_SetZeroSingle(1, 2, true);
            int offset = (int)((double)uiDoubleUpDown10.Value * 10000);
            SG.RC rc = new SG.RC();
            rc = SGLinkFuc.SGIF_SetOffset(1, 3, offset);
            if (rc == SG.RC.RC_OK)
            {
                ShowSuccessTip("标定成功！");
            }
            else
            {
                ShowErrorTip("标定失败！");
            }
        }

        private void uiButton9_Click(object sender, EventArgs e)
        {

        }
    }
}
 
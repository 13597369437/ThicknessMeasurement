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

namespace ThicknessMeasurement.Page
{
    public partial class 标定 : UIPage
    {
        IFreeSql fsql = DB.MySQL;
        public 标定()
        {
            InitializeComponent();
        }

        int[] inputValues = new int[6];
        //标定读取测量值
        private void uiButton1_Click(object sender, EventArgs e)
        {
            UIButton btn= (UIButton)sender;

            int outNo=int.Parse(btn.TagString);
            LKIF2.RC result = (LKIF2.RC)0;
            LKIF2.LKIF_FLOATVALUE_OUT calcData = new LKIF2.LKIF_FLOATVALUE_OUT();
            result = LKIF2.LKIF2_GetCalcDataSingle(outNo, ref calcData);

            if (DataClass.peizhivalues[0] == "0")
            {
                try
                {
                    switch (btn.Tag.ToString())
                    {
                        case "1":
                            uiDoubleUpDown1.Value = double.Parse(FloatResultValueOutToText(calcData));
                            inputValues[0] = 计算测厚头测量值(0, 1);
                            break;
                        case "2":
                            uiDoubleUpDown3.Value = double.Parse(FloatResultValueOutToText(calcData));
                            inputValues[1] = 计算测厚头测量值(0, 1);
                            break;
                        case "3":
                            uiDoubleUpDown5.Value = double.Parse(FloatResultValueOutToText(calcData));
                            inputValues[2] = 计算测厚头测量值(2, 3);
                            break;
                        case "4":
                            uiDoubleUpDown7.Value = double.Parse(FloatResultValueOutToText(calcData));
                            inputValues[3] = 计算测厚头测量值(2, 3);
                            break;
                        case "5":
                            uiDoubleUpDown9.Value = double.Parse(FloatResultValueOutToText(calcData));
                            inputValues[4] = 计算测厚头测量值(4, 5);
                            break;
                        case "6":
                            uiDoubleUpDown11.Value =  double.Parse(FloatResultValueOutToText(calcData));
                            inputValues[5] = 计算测厚头测量值(4, 5);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorTip(ex.Message);
                }
            }
            else if (DataClass.peizhivalues[0] == "2")
            {
                RC rc = new RC();
                SGIF_FLOATVALUE_OUT mData;
                switch (btn.Tag.ToString())
                {
                    case "1":
                        rc = SGLinkFuc.SGIF_GetCalcDataSingle(0, 5, out mData);
                        uiDoubleUpDown1.Value = mData.Value;
                        break;
                    case "2":
                        rc = SGLinkFuc.SGIF_GetCalcDataSingle(0, 5, out mData);
                        uiDoubleUpDown3.Value = mData.Value;
                        break;
                    case "3":
                        rc = SGLinkFuc.SGIF_GetCalcDataSingle(0, 6, out mData);
                        uiDoubleUpDown5.Value = mData.Value;
                        break;
                    case "4":
                        rc = SGLinkFuc.SGIF_GetCalcDataSingle(0, 6, out mData);
                        uiDoubleUpDown7.Value = mData.Value;
                        break;
                    case "5":
                        rc = SGLinkFuc.SGIF_GetCalcDataSingle(1, 3, out mData);
                        uiDoubleUpDown9.Value = mData.Value;
                        break;
                    case "6":
                        rc = SGLinkFuc.SGIF_GetCalcDataSingle(1, 3, out mData);
                        uiDoubleUpDown11.Value = mData.Value;
                        break;
                    default:
                        break;


                }
                if (rc == RC.RC_OK)
                {
                    ShowSuccessTip("测量成功！");
                }
                else
                {
                    ShowErrorTip("测量失败！");
                }
            }
        }

        int 计算测厚头测量值(int outNo1, int outNo2)
        {
            LKIF2.RC result = (LKIF2.RC)0;
            LKIF2.LKIF_FLOATVALUE_OUT calcData1 = new LKIF2.LKIF_FLOATVALUE_OUT();
            LKIF2.LKIF_FLOATVALUE_OUT calcData2 = new LKIF2.LKIF_FLOATVALUE_OUT();

            result = LKIF2.LKIF2_GetCalcDataSingle(outNo1, ref calcData1);
            result = LKIF2.LKIF2_GetCalcDataSingle(outNo2, ref calcData2);

            int out1 = GetTextData(double.Parse(FloatResultValueOutToText(calcData1)));
            int out2= GetTextData(double.Parse(FloatResultValueOutToText(calcData2)));

            return out1 + out2;

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


            UIButton btn = (UIButton)sender;
            int outNo = int.Parse(btn.TagString);
            int inputValue1 = 0;
            int outputValue1 = 0;
            int inputValue2 = 0;
            int outputValue2 = 0;


          

            if (DataClass.peizhivalues[0] == "2")//深视
            {
                RC rc = new RC();
                //停止测厚
                SGLinkFuc.SGIF_DataStorageStop(0);
                switch (btn.TagString)
                {
                    case "6":
                        inputValue1 = Convert.ToInt32(uiDoubleUpDown1.Value * 10000);
                        outputValue1 = Convert.ToInt32(uiDoubleUpDown2.Value * 10000);
                        inputValue2 = Convert.ToInt32(uiDoubleUpDown3.Value * 10000);
                        outputValue2 = Convert.ToInt32(uiDoubleUpDown4.Value * 10000);
                        rc = SGLinkFuc.SGIF_SetScaling(0, 5, inputValue1, outputValue1, inputValue2, outputValue2);
                        break;
                    case "7":
                        inputValue1 = Convert.ToInt32(uiDoubleUpDown5.Value * 10000);
                        outputValue1 = Convert.ToInt32(uiDoubleUpDown6.Value * 10000);
                        inputValue2 = Convert.ToInt32(uiDoubleUpDown7.Value * 10000);
                        outputValue2 = Convert.ToInt32(uiDoubleUpDown8.Value * 10000);
                        rc = SGLinkFuc.SGIF_SetScaling(0, 6, inputValue1, outputValue1, inputValue2, outputValue2);
                        break;
                    case "8":
                        inputValue1 = Convert.ToInt32(uiDoubleUpDown9.Value * 10000);
                        outputValue1 = Convert.ToInt32(uiDoubleUpDown10.Value * 10000);
                        inputValue2 = Convert.ToInt32(uiDoubleUpDown11.Value * 10000);
                        outputValue2 = Convert.ToInt32(uiDoubleUpDown12.Value * 10000);
                        rc = SGLinkFuc.SGIF_SetScaling(1, 3, inputValue1, outputValue1, inputValue2, outputValue2);
                        break;
                }
                if (rc == RC.RC_OK)
                {
                    ShowSuccessTip("标定成功！");
                }
                else
                {
                    ShowErrorTip("标定失败！");
                }
            }
            else if (DataClass.peizhivalues[0] == "0")//基恩士
            {
                LKIF2.RC result = (LKIF2.RC)1;

                //停止测量才能标定
                result = LKIF2.LKIF2_StopMeasure();
                if ((int)result != 0)
                {
                    ShowErrorTip("标定失败！");
                    return;
                }


                switch (btn.TagString)
                {
                    case "6":
                        if (inputValues[0]!=0 && GetTextData(uiDoubleUpDown2, ref outputValue1) &&
                             inputValues[1] != 0 && GetTextData(uiDoubleUpDown4, ref outputValue2))
                        {
                            result = LKIF2.LKIF2_SetScaling(outNo, inputValues[0], outputValue1, inputValues[1], outputValue2);
                        }
                        break;
                    case "7":
                        if (inputValues[2] != 0 && GetTextData(uiDoubleUpDown6, ref outputValue1) &&
                             inputValues[3] != 0 && GetTextData(uiDoubleUpDown8, ref outputValue2))
                        {
                            result = LKIF2.LKIF2_SetScaling(outNo, inputValues[2], outputValue1, inputValues[3], outputValue2);
                        }
                        break;
                    case "8":
                        if (inputValues[4] != 0 && GetTextData(uiDoubleUpDown10, ref outputValue1) &&
                            inputValues[5] != 0 && GetTextData(uiDoubleUpDown12, ref outputValue2))
                        {
                            result = LKIF2.LKIF2_SetScaling(outNo, inputValues[4], outputValue1, inputValues[5], outputValue2);
                        }
                        break;
                }

                if ((int)result == 0)
                {
                    ShowSuccessTip("标定成功！");
                }
                else
                {
                    ShowErrorTip("标定失败！");
                }

                //标定结束后恢复测量
                LKIF2.LKIF2_StartMeasure();

            }



        }

        private bool GetTextData(UIDoubleUpDown dud, ref int value)
        {
            bool result = false;
            try
            {
                result = true;
                value = (int)(dud.Value * 10000);
                return result;
            }
            catch
            {
                MessageBox.Show("illegal input", Application.ProductName);
                return false;
            }
        }


        private int GetTextData(double dud)
        {
            try
            {
                return (int)(dud * 10000);
            }
            catch
            {
                MessageBox.Show("illegal input", Application.ProductName);
                return 0;
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

   
    }
}
 
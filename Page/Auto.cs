using MySql.Data.MySqlClient;
using Sunny.UI;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ActUtlTypeLib;
using System.Configuration;
using Utils;
using System.Collections.Generic;
using CF_Library;
using NPOI.Util;
using NPOI.SS.Formula.Functions;
using System.Runtime.InteropServices;
using MathNet.Numerics.LinearAlgebra.Factorization;
using Google.Protobuf.WellKnownTypes;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using SixLabors.Fonts;
using MySqlX.XDevAPI.Common;
using static NPOI.POIFS.Crypt.CryptoFunctions;
using MathNet.Numerics;
using static System.Net.WebRequestMethods;
using System.Diagnostics;
using Org.BouncyCastle.Tls;
using static ThicknessMeasurement.LKIF2;
using SG;
using SG_Demo.SG;


namespace ThicknessMeasurement
{
    public partial class Auto : UIPage
    {
        #region 全局变量

        ActUtlType PLC = new ActUtlType();
        IFreeSql fsql = DB.MySQL;
        //测厚硬件声明
        public delegate void showInfoDelegate(string info, bool display_time = true);
        DeviceInfo_t[] deviceList;
        int deviceNumber = 0;
        public static int deviceHandle1 = -1; public static int deviceHandle2 = -1; public static int deviceHandle3 = -1;

        // float[] g_measureData = new float[4];
        List<float> g_measureData_1 = new List<float>();
        List<float> g_measureData_2 = new List<float>();
        List<float> g_measureData_3 = new List<float>();
        bool isDoubleChannel_1 = false;
        bool isDoubleChannel_2 = false;
        bool isDoubleChannel_3 = false; 

        float[] g_saturation = new float[4];
        int g_signalNumber = 0;
        int g_channel = 0;
        int[][] signal = new int[2][];
        int channelIndex = 0;
        int[] triggerCount = new int[] { 0, 0, 0 };
        float[] distance = new float[] { 0, 0, 0, 0, };
        int distanceLen = 0;
        bool isTriggerPass = false;

        int plcStart = 0;
        int plcStop = 0;
        int plcCancel = 0;

        //线程委托更新
        private delegate void updateuiDelegate(int handle, EventCallbackArgs_t arg, IntPtr userPara);

        bool isInDistanceMode = true;
        StatusTypeDef ret;
        //测厚硬件声明

        int PLCReturnCode = 1;


        DataTable dtMsg = new DataTable();

        public delegate void connectDelegate();

        [DllImport("wsock32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        extern public static int inet_addr([MarshalAs(UnmanagedType.VBByRefStr)] ref string cp);


        DataTable ProductionLog = new DataTable();
        double avg1 = 0; double avg2 = 0; double avg3 = 0; double avg123;
        MC_ResultDataTypeDef_t[] res = new MC_ResultDataTypeDef_t[20000];//这个大小是cache阈值
        double d11 = 0; double d12 = 0; double d13 = 0; double d14 = 0;
        double d21 = 0; double d22 = 0; double d23 = 0; double d24 = 0;
        double d31 = 0; double d32 = 0; double d33 = 0; double d34 = 0;

        double avg = 0;
        public static string measuringStatus = "等待产品进入";
        double[] dataFromSsr1 = new double[] { };
        double[] dataFromSsr2 = new double[] { };
        double[] dataFromSsr3 = new double[] { };
        DataTable DtChart1 = new DataTable();
        DataTable DtChart2 = new DataTable();
        DataTable DtChart3 = new DataTable();
        DateTime dateTimeStart;
        DateTime dateTimeEnd;
        DateTime measureStart;
        string[] r = new string[] {
            ConfigurationManager.AppSettings["r1"],
            ConfigurationManager.AppSettings["r2"],
            ConfigurationManager.AppSettings["r3"],
            ConfigurationManager.AppSettings["r4"],
            ConfigurationManager.AppSettings["r5"],
            ConfigurationManager.AppSettings["r6"],
            ConfigurationManager.AppSettings["r7"],
            ConfigurationManager.AppSettings["r8"],
            ConfigurationManager.AppSettings["r9"],
            ConfigurationManager.AppSettings["r10"],
            ConfigurationManager.AppSettings["r11"],
            ConfigurationManager.AppSettings["r12"], };


        string MesStaus = "";
        string Thickness = "";
        int BatchID = 7060;
        int MaterialID = 7010;
        string SetMax = "";
        string SetMin = "";
        string SetMax_Min = "";
        string PCHeartBeat = "";
        string PLCHeartBeat = "";
        string PLCStart = "";
        string PCResult = "";
        string PCReady = "";
        string PLCCancel = "";

        string precision = "0.000";//显示精度
        int iprecision = 1000;


        #endregion

        public Auto()
        {
            InitializeComponent();

        }

        /// <summary>
        /// 初始化，启动所有连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Auto_Shown(object sender, EventArgs e)
        {

            //连接PLC
            PLC.ActLogicalStationNumber = int.Parse(DataClass.peizhivalues[150]);
            PLC.ActPassword = "";
            PLCReturnCode = PLC.Open();
            if (PLCReturnCode == 0)
            {
                toolStripStatusLabel4.Text = "在线";
                toolStripStatusLabel4.BackColor = Color.Green;
                showMessage("状态", "PLC连接成功");
            }
            else
            {
                toolStripStatusLabel4.Text = "离线";
                toolStripStatusLabel4.BackColor = Color.Red;
                showMessage("状态", "PLC连接失败");
            }
            timerPLC.Enabled = true;

            //连接测厚仪
            if (DataClass.peizhivalues[0] == "1")
            {
                //连接海博森传感器
                Task.Run(() =>
                {
                    connectSensor();
                });

                uiTextBox8.Text = Calibration.lotNum;
                uiTextBox6.Text = Calibration.batchNum;
            }
            else if (DataClass.peizhivalues[0] == "0")
            {
                lianjie1();
            }
            else if (DataClass.peizhivalues[0] == "2") //深视智能传感器
            {
                connectSensorSszn();
                Task.Run(measuringSszn);
            }



            //连接读码器
            if (DataClass.peizhivalues[1] == "1" && DataClass.peizhivalues[3] == "1")
            {
                lianjieccd();
            }

        }


        private void Form2_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            {
                dgvMsg.DataSource = dtMsg;
                dtMsg.Columns.Add(new DataColumn("   类型", typeof(string)));
                dtMsg.Columns.Add(new DataColumn("   内容", typeof(string)));
                dtMsg.Columns.Add(new DataColumn("   时间", typeof(string)));
                dgvMsg.Init();//SunnyUI的dgv初始化功能
                dgvMsg.ReadOnly = true;
                dgvMsg.Columns[0].Width = 100;
                dgvMsg.Columns[1].Width = 436;
                dgvMsg.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            //图表初始化
            setChartOption1();
            setChartOption2(); 
            setChartOption3();
            showMessage("状态", "图表初始化完成");


            Calibration.isSaved = true;


            toolStripStatusLabel5.Visible = DataClass.peizhivalues[8] == "1";
            toolStripStatusLabel6.Visible = DataClass.peizhivalues[8] == "1";

            toolStripStatusLabel7.Visible = DataClass.peizhivalues[1] == "1" && DataClass.peizhivalues[3] == "1";
            toolStripStatusLabel8.Visible = DataClass.peizhivalues[1] == "1" && DataClass.peizhivalues[3] == "1";
        }

        //界面切换事件
        private void Form2_Initialize(object sender, EventArgs e)
        {
            switch(DataClass.peizhivalues[9])
            {
                case "1":
                    precision = "0.0";
                    iprecision = 10;
                    break;
                case "2":
                    precision = "0.00";
                    iprecision = 100;
                    break;
                case "3":
                    precision = "0.000";
                    iprecision = 1000;
                    break;
                case "4":
                    precision = "0.0000";
                    iprecision = 10000;
                    break;
            }

            if (DataClass.peizhivalues[0] == "1")
            {
             
                funt6(Calibration.points != 9);

            }
            else
            {
                funt6(DataClass.peizhivalues[11] != "9");
                
            }

            if (Calibration.isSaved)
            {
                setChartOption1();
                setChartOption2();
                setChartOption3();
                setChartOption4(1.0, 0.0);
                Calibration.isSaved = false;
            }


            PLCCancel = ConfigurationManager.AppSettings["PLCCancel"];
            MesStaus = ConfigurationManager.AppSettings["MESStatus"];
            SetMax = ConfigurationManager.AppSettings["SetMax"];
            SetMin = ConfigurationManager.AppSettings["SetMin"];
            SetMax_Min = ConfigurationManager.AppSettings["SetMax_Min"];
            Thickness = ConfigurationManager.AppSettings["Thickness"];
            BatchID = Convert.ToInt32(ConfigurationManager.AppSettings["BatchID"]);
            MaterialID = Convert.ToInt32(ConfigurationManager.AppSettings["MaterialID"]);

            PCHeartBeat = DataClass.peizhivalues[151];//PC心跳地址
            PLCHeartBeat = DataClass.peizhivalues[152];//PLC心跳地址
            PCResult = DataClass.peizhivalues[154];//PLC测量结果地址
            PLCStart = DataClass.peizhivalues[155];//开始测量地址
            PCReady = DataClass.peizhivalues[156];//测厚仪连接状态地址

        }


        #region 基恩士测厚仪连接及数据读取

        //连接测厚仪
        void lianjie1()
        {
            //开始测量
            LKIF2.LKIF2_StartMeasure();
            Task.Run(() =>
            {
                while (true)
                {
                    //连接基恩士传感器
                    int ljzt = 0;
                    if (DataClass.peizhivalues[50] == "0")
                    {
                        //以太网连接
                        LKIF2.LKIF_OPENPARAM_ETHERNET openParam = new LKIF2.LKIF_OPENPARAM_ETHERNET();
                        string ipAddres = DataClass.peizhivalues[51];
                        openParam.IPAddress.S_addr = inet_addr(ref ipAddres);

                        ljzt = (int)LKIF2.LKIF2_OpenDeviceETHER(ref openParam);

                    }
                    else
                    {
                        //USB连接
                        ljzt = (int)LKIF2.LKIF2_OpenDeviceUsb();
                    }

                    if (ljzt == 0)
                    {
                        测厚仪连接状态显示(true);
                        showMessage("状态", "测厚仪连接成功");
                        PLC.SetDevice(DataClass.peizhivalues[156], 1);
                        break;
                    }
                    else
                    {
                        PLC.SetDevice(DataClass.peizhivalues[156], 2);
                    }

                    Task.Delay(1000).Wait();
                }
                rw_cehou();

            });
        }

        bool startread = true;
       
        void rw_cehou()
        {
            Task.Run(() =>
            {
                while (true)
                {
                   
                    //if (startread)
                    //{ 
                    //    //开始读取测厚数据
                    //    if (!GetCalcDataMulti1())
                    //        break;
                    //}
                    //else
                    //{
                    //    //对测厚数据进行处理
                    //    keyence_funt4_2();

                    //    keyence_funt4_3();

                    //    keyence_funt4_4();

                    //    out7.Clear();
                    //    out8.Clear();
                    //    out9.Clear();
                    //    startread = true;

                    //}

                    if (!GetCalcDataMulti2())
                        break;

                }

                LKIF2.LKIF2_CloseDevice();
                showMessage("状态", "测厚仪断开连接");
                测厚仪连接状态显示(false);
                PLC.SetDevice(DataClass.peizhivalues[156], 2);
                Task.Delay(1000).Wait();
                lianjie1();
            });
        } 

        List<double> out7 = new List<double>();
        List<double> out8 = new List<double>();
        List<double> out9 = new List<double>();

        List<double> ys1 = new List<double>();
        List<double> ys2 = new List<double>();
        List<double> ys3 = new List<double>();

        int pcresult;
        bool readjes = false;
        bool readjes1 = false;
        DateTime startdt;
        LKIF2.LKIF_FLOATVALUE_OUT[] calcData = new LKIF2.LKIF_FLOATVALUE_OUT[13];
        LKIF2.RC result = 0;

        enum Branch
        {
            开始存储,
            存储中,
            停止存储,
            数据处理,
            等待开始
        }

        Branch branch;

        //多通道读取
        bool GetCalcDataMulti()
        {
            LKIF2.LKIF_OUTNO outNo = MakeOutNoFromCheckBoxes();
            result = LKIF2.LKIF2_GetCalcDataMulti(outNo, ref calcData[0]);

            if(FloatResultValueOutToText(calcData[0]) != "+FFFFFFF" && FloatResultValueOutToText(calcData[0]) != "-FFFFFFF"
                && FloatResultValueOutToText(calcData[0]) != "--------" && FloatResultValueOutToText(calcData[0]) != "alarm"
                && FloatResultValueOutToText(calcData[0]) != "Invalid")
            {
                try
                {
                    PLC.GetDevice(PCResult, out pcresult);

                    if (pcresult != 0 && !readjes1)
                    {

                        writePLCValue(PCResult, 0);
                    }

                    if (DateTime.Now >= startdt.AddSeconds(int.Parse(DataClass.peizhivalues[7])) && !readjes1)
                    {
                        showMessage("错误", "测量超时请检查是否发生机械故障");
                        writePLCValue(PCResult, 2);
                        readjes1 = true;
                    }
                    else
                    {
                        out7.Add(double.Parse(FloatResultValueOutToText(calcData[0])));
                        out8.Add(double.Parse(FloatResultValueOutToText(calcData[1])));
                        out9.Add(double.Parse(FloatResultValueOutToText(calcData[2])));
                        readjes = true;
                    }


                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
            else if(readjes)
            {

                startdt=DateTime.Now;
                readjes = false;
                readjes1 = false;
                startread = false;

            }

            return result == 0;
        }

        //从数据存储器读取
        int stratch;
        bool GetCalcDataMulti2()
        {
            PLC.GetDevice(PLCStart, out stratch);

            switch (branch)
            {
                case Branch.开始存储:

                    startdt = DateTime.Now;
                    //初始化数据存储
                    result = LKIF2.LKIF2_DataStorageInit();
                    //开始数据存储
                    result = LKIF2.LKIF2_DataStorageStart();
                    writePLCValue(PCResult, 0);
                    branch = Branch.存储中;

                    break;

                case Branch.存储中:

                    if (DateTime.Now >= startdt.AddSeconds(int.Parse(DataClass.peizhivalues[7])))
                    {
                        //showMessage("错误", "测量超时请检查是否发生机械故障");
                        //writePLCValue(PCResult, 2);
                        //停止数据存储
                        result = LKIF2.LKIF2_DataStorageStop();
                        branch = Branch.等待开始;
                        break;
                    }

                    if (stratch == 2)
                    {
                        branch = Branch.停止存储;
                        break;
                    }

                    break;

                case Branch.停止存储:

                    //停止数据存储
                    result = LKIF2.LKIF2_DataStorageStop();

                    //读取存储的数据
                    LKIF2.LKIF_FLOATVALUE[] OutBuffer1 = new LKIF2.LKIF_FLOATVALUE[1200001];
                    LKIF2.LKIF_FLOATVALUE[] OutBuffer2 = new LKIF2.LKIF_FLOATVALUE[1200001];
                    LKIF2.LKIF_FLOATVALUE[] OutBuffer3 = new LKIF2.LKIF_FLOATVALUE[1200001];
                    int numReceived = 0;
                    result = LKIF2.LKIF2_DataStorageGetData(6, 100000, ref OutBuffer1[0], ref numReceived);
                    result = LKIF2.LKIF2_DataStorageGetData(7, 100000, ref OutBuffer2[0], ref numReceived);
                    result = LKIF2.LKIF2_DataStorageGetData(8, 100000, ref OutBuffer3[0], ref numReceived);

                    out7 = OutBuffer1.Select(s => (double)s.value).ToList();
                    out8 = OutBuffer2.Select(s => (double)s.value).ToList();
                    out9 = OutBuffer3.Select(s => (double)s.value).ToList();

                    OutBuffer3.Where(w => w.value != 0).Select(s => s.value).ToList();

                    PLC.SetDevice(PLCStart, 3);
                    Task.Delay(100).Wait();

                    branch = Branch.数据处理;

                    break;

                case Branch.数据处理:

                    //对测厚数据进行处理
                    keyence_funt4_2();

                    keyence_funt4_3();

                    keyence_funt4_4();

                    out7.Clear();
                    out8.Clear();
                    out9.Clear();

                    branch = Branch.等待开始;
            
                    break;

                case Branch.等待开始:
                    if (stratch == 1)
                    {
                        branch = Branch.开始存储;
                    }
                    LKIF2.LKIF_FLOATVALUE_OUT calcData1 = new LKIF2.LKIF_FLOATVALUE_OUT();
                    result = LKIF2.LKIF2_GetCalcDataSingle(6, ref calcData1);
                   
                    break;

                default:
                    branch = Branch.等待开始;
                    break;

            }


            return result == 0;

        }


        private LKIF2.LKIF_OUTNO MakeOutNoFromCheckBoxes()
        {
            LKIF2.LKIF_OUTNO result = (LKIF2.LKIF_OUTNO)0;
            LKIF2.LKIF_OUTNO[] OutNoTable = new LKIF2.LKIF_OUTNO[] 
            { 
                LKIF2.LKIF_OUTNO.LKIF_OUTNO_01,
                LKIF2.LKIF_OUTNO.LKIF_OUTNO_02, 
                LKIF2.LKIF_OUTNO.LKIF_OUTNO_03,
                LKIF2.LKIF_OUTNO.LKIF_OUTNO_04,
                LKIF2.LKIF_OUTNO.LKIF_OUTNO_05,
                LKIF2.LKIF_OUTNO.LKIF_OUTNO_06,
                LKIF2.LKIF_OUTNO.LKIF_OUTNO_07,
                LKIF2.LKIF_OUTNO.LKIF_OUTNO_08,
                LKIF2.LKIF_OUTNO.LKIF_OUTNO_09,
                LKIF2.LKIF_OUTNO.LKIF_OUTNO_10,
                LKIF2.LKIF_OUTNO.LKIF_OUTNO_11, 
                LKIF2.LKIF_OUTNO.LKIF_OUTNO_12 
            };

         
            result = result | OutNoTable[6];
            result = result | OutNoTable[7];
            result = result | OutNoTable[8];

            return result;
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

        void keyence_funt1()
        {
            //判断是否由PLC下发判定，还是本地
            PLC.GetDevice(MesStaus, out int mesStatus);
            if (mesStatus == 1)
            {
                //订单号
                uiTextBox6.Text = plcGetString(Convert.ToInt32(BatchID));
                //批号
                uiTextBox8.Text = plcGetString(Convert.ToInt32(MaterialID)).Trim();

                PLC.GetDevice(Thickness, out int thickness); 
                Calibration.setThickness = ((double)thickness) / iprecision;
                PLC.GetDevice(SetMax, out int setMax); 
                Calibration.setMax = ((double)setMax) / iprecision;
                PLC.GetDevice(SetMin, out int setMin); 
                Calibration.setMin = ((double)setMin) / iprecision;
                PLC.GetDevice(SetMax_Min, out int setMax_Min); 
                Calibration.setMax_Min = ((double)setMax_Min) / iprecision;

            }

            showMessage("状态", measuringStatus);
        }

        //测厚数据处理
        void keyence_funt4_2()
        {
            try
            {  
                //中值滤波宽度
                int degree = int.Parse(DataClass.peizhivalues[52]);

                //中值滤波
                ys1 = medianfilter(out7, degree);
                ys2 = medianfilter(out8, degree);
                ys3 = medianfilter(out9, degree);


                avg1 = ys1.Average();
                avg2 = ys2.Average();
                avg3 = ys3.Average();
                avg123 = (avg1 + avg2 + avg3) / 3;


                //自动识别配方
                if (DataClass.peizhivalues[2] == "1")
                    DataClass.pfid = funt3(avg123);
                //是否显示取样点4
                funt6(DataClass.peizhivalues[11]!= "9");
            }
            catch { }
            
        }

        //基恩士测厚数据显示
        void keyence_funt4_3()
        {
            //数据分配
            d11 = 0; d12 = 0; d13 = 0; d14 = 0; d21 = 0; d22 = 0; d23 = 0; d24 = 0; d31 = 0; d32 = 0; d33 = 0; d34 = 0;

            double[] dys = new double[4];
            dys[0] = double.Parse(DataClass.peifangvalues[5]);//配方取样点1
            dys[1] = double.Parse(DataClass.peifangvalues[6]);
            dys[2] = double.Parse(DataClass.peifangvalues[7]);
            dys[3] = double.Parse(DataClass.peifangvalues[8]);

            //曲线显示
            xianshi(ys1, dys, lineChart1, setChartOption);
            xianshi(ys2, dys, lineChart2, setChartOption);
            xianshi(ys3, dys, lineChart3, setChartOption);

            //正态分布曲线显示
            xianshi1(uiLineChart1, setChartOption4);


            //获取取样点数据
            var dy1 = funt5(ys1, dys, 1);
            var dy2 = funt5(ys2, dys, 2);
            var dy3 = funt5(ys3, dys, 3);

            d11 = dy1[0]; d12 = dy1[1]; d13 = dy1[2]; d14 = dy1[3];
            txb1.Text = d11.ToString(precision)+ "mm";
            txb2.Text = d12.ToString(precision)+ "mm";
            txb3.Text = d13.ToString(precision)+ "mm";
            txb4.Text = d14.ToString(precision)+ "mm";

            d21 = dy2[0]; d22 = dy2[1]; d23 = dy2[2]; d24 = dy2[3];
            txb5.Text = d21.ToString(precision)+ "mm";
            txb6.Text = d22.ToString(precision)+ "mm";
            txb7.Text = d23.ToString(precision)+ "mm";
            txb8.Text = d24.ToString(precision)+ "mm";

            d31 = dy3[0]; d32 = dy3[1]; d33 = dy3[2]; d34 = dy3[3];
            txb9.Text = d31.ToString(precision)+ "mm";
            txb10.Text = d32.ToString(precision)+ "mm";
            txb11.Text = d33.ToString(precision)+ "mm";
            txb12.Text = d34.ToString(precision)+ "mm";

            avg = avg123;

            //平均值
            uiTextBox5.Text = avg.ToString(precision)+ "mm";

        }

        //测厚结果判断、记录
        void keyence_funt4_4()
        {
            try
            {
                //测量结束后，给plc写入测量结果
                //writePLCValue(ConfigurationManager.AppSettings["PCResultAvg"], (int)(avg * iprecision));

                double[] doubles1 = { d11, d12, d13, d14, d21, d22, d23, d24, d31, d32, d33, d34 };

                var ds = doubles1.Select(s => (int)s * iprecision).ToArray();

                PLC.WriteDeviceBlock(r[0], 12, ref ds[0]);

            }
            catch(Exception ex1) 
            {
                showMessage("错误", "函数keyence_funt4_4，ex1: " + ex1.Message);
            }

            double max; double min; double max_min;
            double[] doubles = null;

            if (DataClass.peizhivalues[11] == "9")
            {
                doubles = new double[] { d11, d12, d13, d21, d22, d23, d31, d32, d33 };
            }
            else
            {
                doubles = new double[] { d11, d12, d13, d14, d21, d22, d23, d24, d31, d32, d33, d34 };
            }

            max = doubles.Max();
            min = doubles.Min();
            max_min = max - min;
            string result = "";

            uiTextBox2.Text = max.ToString(precision) + "mm";
            uiTextBox3.Text = min.ToString(precision) + "mm";
            uiTextBox4.Text = max_min.ToString(precision) + "mm";

            //plc写入判断结果
            if (DataClass.peizhivalues[10] == "0")
            {
                //极差法判定
                double sv_max_min = double.Parse(DataClass.peifangvalues[1]);
                if (max_min > sv_max_min)
                {
                    ShowErrorNotifier("产品极差超出设定值，请检查产品是否异常", false, 900000);
                    showMessage("错误", "产品极差超出设定值");
                    writePLCValue(PCResult, 2);
                    result = "NG";
                }
                else
                {
                    writePLCValue(PCResult, 1);
                    result = "OK";
                    showMessage("状态", "产品OK");

                }
            }
            else if (DataClass.peizhivalues[10] == "1")
            {
                //上下限法判定
                //判定是否整板取样
                if (DataClass.peifangvalues[11] == "1")
                {
                    if (funt7(ys1) || funt7(ys2) || funt7(ys3))
                    {
                        ShowErrorNotifier("产品的最大值或最小值超出设定值，请检查产品是否异常", false, 900000);
                        showMessage("错误", "产品的最大值或最小值超出设定值，请检查产品是否异常");
                        writePLCValue(PCResult, 2);
                    }
                    else
                    {
                        writePLCValue(PCResult, 1);
                        showMessage("状态", "产品OK");
                    }
                }
                else
                {
                    result = ngcount(doubles);

                    if (result == "NG")
                    {
                        ShowErrorNotifier("产品的最大值或最小值超出设定值，请检查产品是否异常", false, 900000);
                        showMessage("错误", "产品的最大值或最小值超出设定值，请检查产品是否异常");
                        writePLCValue(PCResult, 2);
                    }
                    else
                    {
                        writePLCValue(PCResult, 1);
                        showMessage("状态", "产品OK");
                    }
                }
            }


            if (max == 0 || min == 0)
            {
                result = "NG";
                writePLCValue(PCResult, 2);
            }

            //生产记录
            productionrecords(result, max, min);
        }


        #endregion

        #region 读码器

        Socket socketSend;
        string code = "";//读取到的二维码

        //连接相机
        void lianjieccd()
        {
            Task.Run(new Action(() =>
            {
                while (true)
                {
                    try
                    {
                        //创建负责通信的Socket
                        socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        //客户端要连结服务器
                        IPAddress ip = IPAddress.Parse(DataClass.peizhivalues[4]);
                        IPEndPoint point = new IPEndPoint(ip, int.Parse(DataClass.peizhivalues[5]));
                        //获得要连结的远程服务器应用程序的IP地址和端口号
                        socketSend.Connect(point);

                        Invoke(new Action(() =>
                        {
                            showMessage("状态", "读码器连接成功");
                        }));

                        break;

                    }
                    catch (Exception ex)
                    {

                        Invoke(new Action(() =>
                        {
                            showMessage("状态", "测厚仪断开连接");
                        }));
                        socketSend.Close();

                        Task.Delay(2000).Wait();
                    }
                }

                Task.Run(new Action(Receive));
            }));
        }


        void Receive()
        {
            while (true)
            {
                try
                {
                    //开始识别
                    if (!test2()) break;
                }
                catch (Exception ex)
                {
                    ShowSuccessTip("接收数据异常：" + ex.Message);
                    break;
                }
            }

            Invoke(new Action(() =>
            {
                //toolStripStatusLabel5.Text = "断开连接";
                //toolStripStatusLabel5.BackColor = Color.Red;
            }));

            socketSend.Close();

            Task.Delay(2000).Wait();

            lianjieccd();
        }

        //扫码器触发
        void test1()
        {

            byte[] buffer = new byte[] { 84, 13, 10 };
            socketSend.Send(buffer);

        }

        //解析扫码器字符
        bool test2()
        {
            Task.Delay(int.Parse(DataClass.peizhivalues[6]));

            test1();

            byte[] buffer = new byte[1024 * 1024 * 2];
            int r = socketSend.Receive(buffer);
            if (r != 0)
            {
                try
                {
                  string  code1 = Encoding.UTF8.GetString(buffer, 0, r);
                 
                    if(code1 !="NG")
                    {
                        code = code1;
                    }

                    Invoke(new Action(() =>
                    {

                    }));

                    return true;
                }
                catch (Exception ex)
                {
                    ShowWarningTip("二维码解析异常：" + ex.Message);
                    return false;
                }
            }
            else
                return false;

        }


        #endregion

        #region 连接海博森测厚仪

        bool is_connectedSensor = true;

        /// <summary>
        /// 连接测厚传感器
        /// </summary>
        void connectSensor()
        {
            CF_UserInterface.HPS_CF_RegisterEventCallback(new UserEventCallbackHandleDelegate(UserEventCallbackHandle), IntPtr.Zero);
            CF_Library.StatusTypeDef ret = CF_Library.StatusTypeDef.Status_Offline;
            ControllerGEPara_t controllerPara = new ControllerGEPara_t();
            controllerPara.controllerIp = null;

            if (ConfigurationManager.AppSettings["HPS"] == "CF4000")
            {
                ret = CF_UserInterface.HPS_CF_ScanDeviceList(out deviceList, out deviceNumber);//搜索cf4000设备
                if (ret != StatusTypeDef.Status_Succeed)
                {
                    this.showMessage("错误", "错误代码:" + ret);

                }
                if (deviceNumber == 0)
                {
                    this.showMessage("错误", "未寻找到测厚仪器");
                }
                ret = CF_UserInterface.HPS_CF_OpenDevice(deviceList[0], ref deviceHandle1, (int)DeviceType_t.HPS_CF4000);

            }
            else
            {
                ret = CF_UserInterface.HPS_CF_GE_OpenDevice(controllerPara, "192.168.0.250", ref deviceHandle1, (int)DeviceType_t.HPS_CF2000);

            }

            if (ret == StatusTypeDef.Status_Succeed)
            {
                this.showMessage("状态", "1号测厚仪器连接完成");
                ret = StatusTypeDef.Status_Offline;
            }
            else
            {
                this.showMessage("状态", "1号测厚仪器连接失败");
                is_connectedSensor = false;

            }

            if (ConfigurationManager.AppSettings["HPS"] == "CF4000")
            {
                ret = CF_UserInterface.HPS_CF_OpenDevice(deviceList[1], ref deviceHandle2, (int)DeviceType_t.HPS_CF4000);
            }
            else
            {
                ret = CF_UserInterface.HPS_CF_GE_OpenDevice(controllerPara, "192.168.0.251", ref deviceHandle2, (int)DeviceType_t.HPS_CF2000);
            }

            if (ret == StatusTypeDef.Status_Succeed)
            {
                this.showMessage("状态", "2号测厚仪器连接完成");
                ret = StatusTypeDef.Status_Offline;
            }
            else
            {
                this.showMessage("状态", "2号测厚仪器连接失败");
                is_connectedSensor = false;
            }

            if (ConfigurationManager.AppSettings["HPS"] == "CF4000")
            {
                ret = CF_UserInterface.HPS_CF_OpenDevice(deviceList[2], ref deviceHandle3, (int)DeviceType_t.HPS_CF4000);
            }
            else
            {
                ret = CF_UserInterface.HPS_CF_GE_OpenDevice(controllerPara, "192.168.0.252", ref deviceHandle3, (int)DeviceType_t.HPS_CF2000);
            }

            if (ret == StatusTypeDef.Status_Succeed)
            {
                this.showMessage("状态", "3号测厚仪器连接完成");
                ret = StatusTypeDef.Status_Offline;
            }
            else
            {
                this.showMessage("状态", "3号测厚仪器连接失败");
                is_connectedSensor = false;
            }

            writePLCValue(PCReady, is_connectedSensor ? 1 : 2);

            if(!is_connectedSensor)
            {
                测厚仪连接状态显示(false);
            }
            else
            {
                //测厚线程
                Task.Run(measuring);

                测厚仪连接状态显示(true);
                is_connectedSensor = true;
            }

            //获取标定系数
            //ret = CF_UserInterface.hps_getDoubleChannelThicknessK(deviceHandle1, 0, ThicnessK_1);
            //ret = CF_UserInterface.hps_getDoubleChannelThicknessK(deviceHandle2, 0, ThicnessK_2);
            //ret = CF_UserInterface.hps_getDoubleChannelThicknessK(deviceHandle3, 0, ThicnessK_3); 
            //try
            //{
            //    using (StreamWriter sw = new StreamWriter($@".\data\" + "开启软件时标定系数.txt"))
            //    {
            //        sw.WriteLine("开启软件时标定系数");
            //        sw.WriteLine("1号，K0=" + ThicnessK_1[0].ToString() + ",K1=" + ThicnessK_1[1].ToString() + ",K2=" + ThicnessK_1[2].ToString());
            //        sw.WriteLine("2号，K0=" + ThicnessK_2[0].ToString() + ",K1=" + ThicnessK_2[1].ToString() + ",K2=" + ThicnessK_2[2].ToString());
            //        sw.WriteLine("3号，K0=" + ThicnessK_3[0].ToString() + ",K1=" + ThicnessK_3[1].ToString() + ",K2=" + ThicnessK_3[2].ToString());
            //    }
            //}
            //catch { }


            //双头测厚相关设置
            ret = CF_UserInterface.HPS_CF_SetIntParam(deviceHandle1, CF_ParameterDefine.PARAM_DOUBLE_CHANNEL_MODE, 0, 1);
            ret = CF_UserInterface.HPS_CF_SetIntParam(deviceHandle2, CF_ParameterDefine.PARAM_DOUBLE_CHANNEL_MODE, 0, 1);
            ret = CF_UserInterface.HPS_CF_SetIntParam(deviceHandle3, CF_ParameterDefine.PARAM_DOUBLE_CHANNEL_MODE, 0, 1);
            isDoubleChannel_1 = true;
            isDoubleChannel_2 = true;
            isDoubleChannel_3 = true;


        }

        public static double[] ThicnessK_1 = new double[3]; 
        public static double[] ThicnessK_2 = new double[3];
        public static double[] ThicnessK_3 = new double[3];

        /// <summary>
        /// 调用回调函数
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="arg"></param>
        /// <param name="userPara"></param>
        public void UserEventCallbackHandle(int handle, EventCallbackArgs_t arg, IntPtr userPara)
        {

            //光谱共焦数据回调函数，这里不宜做复杂的操作，应该只是将数据拷贝走
            //接收到数据
            if (handle == deviceHandle1)
            {
                if (arg.eventType == EventTypeDef.EventType_DataRecv)
                {
                    if (isDoubleChannel_1)    //设置一个标志位，在设备启动的时，开双头模式的时候设true
                    {
                        doubleChannelDataProcess(arg.data, arg.dataLen);
                    }
                }
            }
            else if (handle == Auto.deviceHandle2)
            {
                if (arg.eventType == EventTypeDef.EventType_DataRecv)
                {

                    if (isDoubleChannel_2)
                        doubleChannelDataProcess2(arg.data, arg.dataLen);
                }
            }
            else if (handle == Auto.deviceHandle3)
            {
                if (arg.eventType == EventTypeDef.EventType_DataRecv)
                {

                    if (isDoubleChannel_3)
                        doubleChannelDataProcess3(arg.data, arg.dataLen);
                }
            }
        }

        /// <summary>
        /// 1号双头测厚获取结果
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataLen"></param>
        void doubleChannelDataProcess(IntPtr data, int dataLen)
        {
            //获取测量结果，数组存储每个通道的计算结果，如果只有一个通道，则数组长度为1
            MC_ResultDataTypeDef_t[] result = Util.IntPtrToStructArray<MC_ResultDataTypeDef_t>(data, dataLen);

            if (dataLen > 0)
            {
                g_measureData_1.Add(result[0].thickness);  //测量结果,默认没有使能多距离测量模式只有resul[0]数据是有效的
            }
        }

        void doubleChannelDataProcess2(IntPtr data, int dataLen)
        {
            //获取测量结果，数组存储每个通道的计算结果，如果只有一个通道，则数组长度为1
            MC_ResultDataTypeDef_t[] result = Util.IntPtrToStructArray<MC_ResultDataTypeDef_t>(data, dataLen);

            if (dataLen > 0)
            {
                g_measureData_2.Add(result[0].thickness);  //测量结果,默认没有使能多距离测量模式只有resul[0]数据是有效的
            }
        }

        void doubleChannelDataProcess3(IntPtr data, int dataLen)
        {
            //获取测量结果，数组存储每个通道的计算结果，如果只有一个通道，则数组长度为1
            MC_ResultDataTypeDef_t[] result = Util.IntPtrToStructArray<MC_ResultDataTypeDef_t>(data, dataLen);

            if (dataLen > 0)
            {
                g_measureData_3.Add(result[0].thickness);  //测量结果,默认没有使能多距离测量模式只有resul[0]数据是有效的
            }
        }

        int dataHead = 0; int dataTail = 0;
        /// <summary>
        /// 减小校准误差
        /// </summary>
        /// <param name="avg"></param>
        /// <param name="avg123"></param>
        /// <param name="DtChart"></param>
        /// <param name="lineChart"></param>
        /// <param name="dt"></param>
        void avgData(double avg, double avg123, DataTable DtChart, UILineChart lineChart, out DataTable dt)
        {
            dt = new DataTable();
            dt.Columns.Add("Data");
            if (avg - avg123 > 0.005)
            {
                for (int i = 0; i < DtChart.Rows.Count; i++)
                {
                    dt.Rows.Add(Convert.ToDouble(DtChart.Rows[i][0]) - 0.005);
                }
            }
            else if (avg123 - avg > 0.005)
            {
                for (int i = 0; i < DtChart.Rows.Count; i++)
                {
                    dt.Rows.Add(Convert.ToDouble(DtChart.Rows[i][0]) + 0.005);
                }
            }
            else
            {
                for (int i = 0; i < DtChart.Rows.Count; i++)
                {
                    dt.Rows.Add(Convert.ToDouble(DtChart.Rows[i][0]));
                }
            }

            try
            {
                for (int i = 0; i < DtChart.Rows.Count; i++)
                {
                    lineChart.Option.AddData("厚度曲线", i, Convert.ToDouble(DtChart.Rows[i][0]));
                }
                lineChart.Refresh();

            }
            catch
            {

            }
        }

        /// <summary>
        /// 去除头尾震荡数据，并轻微做平滑处理
        /// </summary>
        /// <param name="dataFromSensor"></param>
        /// <param name="lineChart"></param>
        /// <param name="DtChart"></param>
        /// <param name="avg"></param>
        void convertData(double[] dataFromSensor, UILineChart lineChart, out DataTable DtChart, out double avg)
        {

            dataHead = 0;
            dataTail = 0;
            DtChart = new DataTable();
            DtChart.Columns.Add("Data");
            int a = 0;
            for (int i = 0; i < dataFromSensor.Length; i++)
            {
                if (dataFromSensor[i].ToString() != "" && dataFromSensor[i] <= 3.2 && dataFromSensor[i] >= 0.06)
                {
                    dataHead = i + 1;
                    break;
                }
            }
            for (int i = dataFromSensor.Length - 1; i >= 0; i--)
            {
                if (dataFromSensor[i].ToString() != "" && dataFromSensor[i] <= 3.2 && dataFromSensor[i] >= 0.06)
                {
                    dataTail = i + 1;
                    break;
                }
            }
            double totalCount = Convert.ToDouble(dataTail - dataHead + 1);
            double NGCount = 0;

            int removeHead = Convert.ToInt32(ConfigurationManager.AppSettings["RemoveHaed"]);//去头n个
            int removeTail = Convert.ToInt32(ConfigurationManager.AppSettings["RemoveTail"]);//去尾m个
            try
            {
                for (int i = 0; i < dataFromSensor.Length; i++)
                {
                    ////取得数据并去头
                    if (dataFromSensor[i + removeHead].ToString() != "" && dataFromSensor[i + removeHead] <= 3.2 && dataFromSensor[i + removeHead] > 0.06)
                    {
                        DtChart.Rows.Add(Math.Round(dataFromSensor[i + removeHead], 4));
                    }
                    ////不去头100个
                    //if (dataFromSensor[i].ToString() != "" && dataFromSensor[i] <= 3.2 && dataFromSensor[i] > 0.3)
                    //{
                    //    //DtChart.Rows.Add(DtFromSSR[i]);
                    //    DtChart.Rows.Add(Math.Round(dataFromSensor[i], 4));//保留4位小数
                    //}
                }
            }
            catch { }
            try
            {
                //去尾
                for (int i = 0; i < removeTail; i++)
                {
                    DtChart.Rows.Remove(DtChart.Rows[DtChart.Rows.Count - removeTail + i]);
                }
            }
            catch { }
            //硬件补偿
            decimal offset = 0;
            if (dataFromSensor == dataFromSsr1)
            { offset = Convert.ToDecimal(ConfigurationManager.AppSettings["Offset_1"]); }
            else if (dataFromSensor == dataFromSsr2)
            { offset = Convert.ToDecimal(ConfigurationManager.AppSettings["Offset_2"]); }
            else if (dataFromSensor == dataFromSsr3)
            { offset = Convert.ToDecimal(ConfigurationManager.AppSettings["Offset_3"]); }

            foreach (DataRow row in DtChart.Rows)
            {
                decimal value = Convert.ToDecimal(row[0]); // 获取第1列的值并转换为decimal类型
                value += offset; // 增加0.005
                row[0] = value; // 更新第1列的值
            }

            double sum = 0;
            for (int i = 0; i < DtChart.Rows.Count; i++)
            {

                sum += Convert.ToDouble(DtChart.Rows[i][0]);
            }
            avg = sum / DtChart.Rows.Count;//第一次取平均
            sum = 0;
            for (int i = 0; i < DtChart.Rows.Count; i++)
            {
                if (avg > Convert.ToDouble(DtChart.Rows[i][0]) + 0.15 || avg < Convert.ToDouble(DtChart.Rows[i][0]) - 0.15)
                {
                    sum += avg;
                }
                else
                {
                    sum += Convert.ToDouble(DtChart.Rows[i][0]);
                }
            }
            avg = sum / DtChart.Rows.Count;//第二次取平均，去除过大过小值
            DataTable dt = new DataTable();
            dt.Columns.Add("Data");
            int badData = 0;
            double level = Convert.ToDouble(ConfigurationManager.AppSettings["Level"]);
            for (int i = 0; i < DtChart.Rows.Count; i++)//数值平滑处理
            {
                if (Convert.ToDouble(DtChart.Rows[i][0]) >= avg + 0.1)
                {
                    badData++;
                    if (badData < 3)
                    {
                        dt.Rows.Add(avg);
                    }
                    else
                    {
                        dt.Rows.Add(Convert.ToDouble(DtChart.Rows[i][0]) - 0.004 * level);
                    }
                }
                else if (Convert.ToDouble(DtChart.Rows[i][0]) >= avg + 0.03)
                {
                    dt.Rows.Add(Convert.ToDouble(DtChart.Rows[i][0]) - 0.003 * level);
                }
                else if (Convert.ToDouble(DtChart.Rows[i][0]) >= avg + 0.02)
                {
                    dt.Rows.Add(Convert.ToDouble(DtChart.Rows[i][0]) - 0.002 * level);
                }
                else if (Convert.ToDouble(DtChart.Rows[i][0]) >= avg + 0.01)
                {
                    dt.Rows.Add(Convert.ToDouble(DtChart.Rows[i][0]) - 0.001 * level);
                }
                else if (Convert.ToDouble(DtChart.Rows[i][0]) <= avg - 0.1)
                {
                    badData++;
                    if (badData < 5)
                    {
                        dt.Rows.Add(avg);
                    }
                    else
                    {
                        dt.Rows.Add(Convert.ToDouble(DtChart.Rows[i][0]) + 0.004 * level);
                    }
                }
                else if (Convert.ToDouble(DtChart.Rows[i][0]) <= avg - 0.03)
                {
                    dt.Rows.Add(Convert.ToDouble(DtChart.Rows[i][0]) + 0.003 * level);
                }
                else if (Convert.ToDouble(DtChart.Rows[i][0]) <= avg - 0.02)
                {
                    dt.Rows.Add(Convert.ToDouble(DtChart.Rows[i][0]) + 0.002 * level);
                }
                else if (Convert.ToDouble(DtChart.Rows[i][0]) <= avg - 0.01)
                {
                    dt.Rows.Add(Convert.ToDouble(DtChart.Rows[i][0]) + 0.001 * level);
                }
                else
                {
                    dt.Rows.Add(Convert.ToDouble(DtChart.Rows[i][0]));
                }
            }
            DtChart.Clear();
            DtChart = dt;
            sum = 0;
            for (int i = 0; i < DtChart.Rows.Count; i++)
            {
                if (avg > Convert.ToDouble(DtChart.Rows[i][0]) + 0.15 || avg < Convert.ToDouble(DtChart.Rows[i][0]) - 0.15)
                {
                    sum += avg;
                }
                else
                {
                    sum += Convert.ToDouble(DtChart.Rows[i][0]);
                }
            }
            avg = sum / DtChart.Rows.Count;


        }
        double NGRange = 0.01;



        void hbs_funt1()
        {
            //判断是否由PLC下发判定，还是本地
            PLC.GetDevice(MesStaus, out int mesStatus);
            if (mesStatus == 1)
            {
                uiTextBox6.Text = plcGetString(Convert.ToInt32(BatchID));
                uiTextBox8.Text = plcGetString(Convert.ToInt32(MaterialID)).Trim();
                PLC.GetDevice(Thickness, out int thickness); Calibration.setThickness = ((double)thickness) / iprecision;
                PLC.GetDevice(SetMax, out int setMax); Calibration.setMax = ((double)setMax) / iprecision;
                PLC.GetDevice(SetMin, out int setMin); Calibration.setMin = ((double)setMin) / iprecision;
                PLC.GetDevice(SetMax_Min, out int setMax_Min); Calibration.setMax_Min = ((double)setMax_Min) / iprecision;

            }

            showMessage("状态", measuringStatus);
        }

        //开始测量
        void strstch_hbs()
        {
            //启动测量
            CF_UserInterface.HPS_CF_StartSample(deviceHandle1, true);
            CF_UserInterface.HPS_CF_StartSample(deviceHandle2, true);
            CF_UserInterface.HPS_CF_StartSample(deviceHandle3, true);
            dateTimeStart = DateTime.Now;
            DateTime dateTime = DateTime.Now;

            if (ConfigurationManager.AppSettings["StopMode"] == "PLC")
            {
                PLC.GetDevice(PLCStart, out plcStart);//PLC给出Stop测厚信号后，停止测厚
                PLC.GetDevice(PLCCancel, out plcCancel);//PLC给出Cancel信号后，取消测厚
                while (plcStart != 2 || plcCancel == 1) /*&& overTime == false)*/
                {

                    PLC.GetDevice(PLCCancel, out plcCancel);
                    PLC.GetDevice(PLCStart, out plcStart);
                    Delay(50);
                }
            }
            else
            {
                Delay(Convert.ToInt32(ConfigurationManager.AppSettings["Delay"]));
            }

            //停止测厚
            measureStart = DateTime.Now;
            ret = CF_UserInterface.HPS_CF_StartSample(deviceHandle1, false);
            ret = CF_UserInterface.HPS_CF_StartSample(deviceHandle2, false);
            ret = CF_UserInterface.HPS_CF_StartSample(deviceHandle3, false);
        }

        //获取数据
        void read_hbs()
        {
            //获取数据
            dataFromSsr1 = new double[g_measureData_1.Count];
            for (int i = 0; i < g_measureData_1.Count; i++)
            {
                dataFromSsr1[i] = g_measureData_1[i];
            }
            dataFromSsr2 = new double[g_measureData_2.Count];
            for (int i = 0; i < g_measureData_2.Count; i++)
            {
                dataFromSsr2[i] = g_measureData_2[i];
            }
            dataFromSsr3 = new double[g_measureData_3.Count];
            for (int i = 0; i < g_measureData_3.Count; i++)
            {
                dataFromSsr3[i] = g_measureData_3[i];
            }
            g_measureData_1.Clear();
            g_measureData_2.Clear();
            g_measureData_3.Clear();

            Task.Run(() =>
            {
                writecsv("1", dataFromSsr1);
                writecsv("2", dataFromSsr2);
                writecsv("3", dataFromSsr3);

            });
        }

        //测厚流程
        void funt4_1()
        {
            PLCReturnCode = PLC.GetDevice(PLCStart, out plcStart);//PLC给出开始测厚信号后，开始测厚
            if (plcStart == 1)//plc给出开始测量信号
            {
                writePLCValue(PCResult, 0);

                writePLCValue(ConfigurationManager.AppSettings["PCResultAvg"], 0);//清之前的测量结果

                int[] d7040 = new int[12];
                PLC.WriteDeviceBlock(r[0], 12, ref d7040[0]);

                //for (int i = 0; i < 12; i++)
                //{
                //    writePLCValue(r[i], 0);
                //}
                measuringStatus = "正在测厚……";
            }
            measuringStatus = "正在测厚……";
        }


        void funt4_2()
        {
            hbs_funt1();
            strstch_hbs();

            if (plcCancel == 0)
            {
                read_hbs();

                lineChart1.Option.Clear();
                setChartOption1();
                convertData(dataFromSsr1, lineChart1, out DtChart1, out avg1);

                lineChart2.Option.Clear();
                setChartOption2();
                convertData(dataFromSsr2, lineChart2, out DtChart2, out avg2);

                lineChart3.Option.Clear();
                setChartOption3();
                convertData(dataFromSsr3, lineChart3, out DtChart3, out avg3);

                avg123 = (avg1 + avg2 + avg3) / 3;

                DataClass.pfid = funt3(avg123);

                avgData(avg1, avg123, DtChart1, lineChart1, out DtChart1);
                avgData(avg2, avg123, DtChart2, lineChart2, out DtChart2);
                avgData(avg3, avg123, DtChart3, lineChart3, out DtChart3);


                measuringStatus = "测厚已完成";

            }
            else
            {
                ShowInfoNotifier("本次测厚已取消");
                measuringStatus = "等待产品进入";
            }
        }


        //海博森测厚数据处理
        void funt4_3()
        {
            //数据分配
            d11 = 0; d12 = 0; d13 = 0; d14 = 0; d21 = 0; d22 = 0; d23 = 0; d24 = 0; d31 = 0; d32 = 0; d33 = 0; d34 = 0;
            try
            {
                d11 = Convert.ToDouble(DtChart1.Rows[(int)(DtChart1.Rows.Count * (Calibration.setLocation1 / 100))][0]);
                d12 = Convert.ToDouble(DtChart1.Rows[(int)(DtChart1.Rows.Count * (Calibration.setLocation2 / 100))][0]);
                d13 = Convert.ToDouble(DtChart1.Rows[(int)(DtChart1.Rows.Count * (Calibration.setLocation3 / 100))][0]);
                d14 = Convert.ToDouble(DtChart1.Rows[(int)(DtChart1.Rows.Count * (Calibration.setLocation4 / 100))][0]);
                txb1.Text = d11.ToString(precision)+ "mm";
                txb2.Text = d12.ToString(precision)+ "mm";
                txb3.Text = d13.ToString(precision)+ "mm";
                txb4.Text = d14.ToString(precision)+ "mm";
            }
            catch
            {
                ShowErrorNotifier("测厚头1号数据异常，请检查", false, 900000);
                showMessage("错误", "测厚头1号数据异常，请检查");
            }
            try
            {
                d21 = Convert.ToDouble(DtChart2.Rows[(int)(DtChart2.Rows.Count * (Calibration.setLocation1 / 100))][0]);
                d22 = Convert.ToDouble(DtChart2.Rows[(int)(DtChart2.Rows.Count * (Calibration.setLocation2 / 100))][0]);
                d23 = Convert.ToDouble(DtChart2.Rows[(int)(DtChart2.Rows.Count * (Calibration.setLocation3 / 100))][0]);
                d24 = Convert.ToDouble(DtChart2.Rows[(int)(DtChart2.Rows.Count * (Calibration.setLocation4 / 100))][0]);
                txb5.Text = d21.ToString(precision)+ "mm";
                txb6.Text = d22.ToString(precision)+ "mm";
                txb7.Text = d23.ToString(precision)+ "mm";
                txb8.Text = d24.ToString(precision)+ "mm";
            }
            catch
            {
                ShowErrorNotifier("测厚头2号数据异常，请检查", false, 900000);
                showMessage("错误", "测厚头2号数据异常，请检查");
            }
            try
            {
                d31 = Convert.ToDouble(DtChart3.Rows[(int)(DtChart3.Rows.Count * (Calibration.setLocation1 / 100))][0]);
                d32 = Convert.ToDouble(DtChart3.Rows[(int)(DtChart3.Rows.Count * (Calibration.setLocation2 / 100))][0]);
                d33 = Convert.ToDouble(DtChart3.Rows[(int)(DtChart3.Rows.Count * (Calibration.setLocation3 / 100))][0]);
                d34 = Convert.ToDouble(DtChart3.Rows[(int)(DtChart3.Rows.Count * (Calibration.setLocation4 / 100))][0]);
                txb9.Text = d31.ToString(precision)+ "mm";
                txb10.Text = d32.ToString(precision)+ "mm";
                txb11.Text = d33.ToString(precision)+ "mm";
                txb12.Text = d34.ToString(precision)+ "mm";
            }
            catch
            {
                ShowErrorNotifier("测厚头3号数据异常，请检查", false, 900000);
                showMessage("错误", "测厚头3号数据异常，请检查");
            }
        
            //求和取平均
            double sum = 0;

            avg = avg123;

            sum = d11 + d12 + d13 + d14 + d21 + d22 + d23 + d24 + d31 + d32 + d33 + d34;
            //avg = sum / 12;
            uiTextBox5.Text = avg.ToString(precision)+ "mm";
            try
            {
                //测量结束后，给plc写入测量结果
                writePLCValue(ConfigurationManager.AppSettings["PCResultAvg"], (int)(avg * iprecision));
                double[] doubles = { d11, d12, d13, d14, d21, d22, d23, d24, d31, d32, d33, d34 };
                for (int i = 0; i < 12; i++)
                {
                    writePLCValue(r[i], (int)(doubles[i] * iprecision));
                }
            }
            catch { }
            double max; double min;
            if (Calibration.points == 9)
            {
                double[] doubles = { d11, d12, d13, d21, d22, d23, d31, d32, d33 };
                max = doubles.Max();
                min = doubles.Min();
            }
            else
            {

                double[] doubles = { d11, d12, d13, d14, d21, d22, d23, d24, d31, d32, d33, d34 };
                max = doubles.Max();
                min = doubles.Min();
            }


            double max_min = max - min;
            string result = "";

            uiTextBox2.Text = max.ToString(precision)+ "mm";
            uiTextBox3.Text = min.ToString(precision)+ "mm";
            uiTextBox4.Text = max_min.ToString(precision)+ "mm";

            //plc写入判断结果
            if (Calibration.mode == 0)
            {
                if (max_min > Calibration.setMax_Min)
                {
                    ShowErrorNotifier("产品极差超出设定值，请检查产品是否异常", false, 900000);
                    showMessage("错误", "产品极差超出设定值");
                    writePLCValue(PCResult, 2);
                    result = "NG";
                }
                else if (min < Calibration.setMin)
                {
                    writePLCValue(PCResult, 3);
                    result = "NG";
                    showMessage("错误", "产品的最小值低于设定值，请检查产品是否异常");
                }
                else
                {
                    writePLCValue(PCResult, 1);
                    result = "OK";
                    showMessage("状态", measuringStatus + "，产品OK");

                }
            }
            else if (Calibration.mode == 1)
            {
                if (min < Calibration.setMin || max > Calibration.setMax)
                {
                    ShowErrorNotifier("产品的最大值或最小值超出设定值，请检查产品是否异常", false, 900000);
                    showMessage("错误", "产品的最大值或最小值超出设定值，请检查产品是否异常");
                    writePLCValue(PCResult, 2);
                    result = "NG";
                }
                else
                {
                    writePLCValue(PCResult, 1);
                    result = "OK";
                    showMessage("状态", measuringStatus + "，产品OK");
                }
            }
            if (max == 0 || min == 0)
            {
                result = "NG";
                writePLCValue(PCResult, 2);
            }


            funt4(result, max, min);
            measuringStatus = "等待产品进入";
        }

        /// <summary>
        /// 测厚线程
        /// </summary>
        void measuring()
        {

            while (true)
            {
                switch (measuringStatus)
                {
                    case "等待产品进入":
                        funt4_1();
                        break;

                    case "正在测厚……":
                        funt4_2();
                        break;

                    case "测厚已完成":

                        funt4_3();
                        break;

                    case "自动测厚停止":
                        //手动模式开启采样后，自动测厚暂停
                        measuringStatus = "等待产品进入";
                        //ShowErrorTip("自动测厚停止");
                        break;

                    default:
                        break;
                }


                Delay(100);
            }
        }

        //生产记录
        void funt4(string result, double max, double min)
        {
            try
            {
                var prod = new productionlog
                {
                    datee = DateTime.Now.ToString("G"),
                    MaterialNum = uiTextBox8.Text,
                    LotNum = uiTextBox6.Text,
                    settingThickness = Calibration.setThickness.ToString(),
                    settingMax = Calibration.setMax.ToString(),
                    settingMin = Calibration.setMin.ToString(),
                    settingMax_min = Calibration.setMax_Min.ToString(),
                    result = result,
                    max = max.ToString(precision),
                    min = min.ToString(precision),
                    max_min = (max - min).ToString(precision),
                    avg = avg.ToString(precision),
                    totalcount = DtChart1.Rows.Count,
                    d11 = d11.ToString(precision),
                    d12 = d12.ToString(precision),
                    d13 = d13.ToString(precision),
                    d14 = d14.ToString(precision),
                    d21 = d21.ToString(precision),
                    d22 = d22.ToString(precision),
                    d23 = d23.ToString(precision),
                    d24 = d24.ToString(precision),
                    d31 = d31.ToString(precision),
                    d32 = d32.ToString(precision),
                    d33 = d33.ToString(precision),
                    d34 = d34.ToString(precision),
                    钢板ID = code
                };

                fsql.Insert(prod).ExecuteIdentity();

                if (DataClass.peizhivalues[1] == "1" && DataClass.peizhivalues[3] == "1")
                {
                    var gbjl = fsql.Select<钢板记录>().Where(w => w.钢板ID == code).ToList();
                    if (gbjl.Count > 0)
                    {
                        int num = gbjl[0].生产次数 + 1;
                        fsql.Update<钢板记录>().Set(s => s.生产次数 == num).Where(w => w.钢板ID == code).ExecuteAffrowsAsync();
                    }
                    else
                    {
                        var gb = new 钢板记录()
                        {
                            钢板ID = code,
                            生产次数 = 1
                        };
                        fsql.Insert(gb).ExecuteAffrowsAsync();
                    }
                }

            }
            catch (Exception ex1)
            {
                showMessage("错误", "funt4,ex1:" + ex1.Message);
            }

            //显示今日NG次数及良率
            DateTime dt;
            dt = DateTime.Now;
            try
            {
                ProductionLog = fsql.Select<productionlog>()
                    .Where(a => Convert.ToDateTime(a.datee) > dt.AddDays(-1)
                    && Convert.ToDateTime(a.datee) <= dt).ToDataTable();

            }
            catch { }
            double yield = 0;
            for (int i = 0; i < ProductionLog.Rows.Count; i++)
            {
                if (ProductionLog.Rows[i][8].ToString().Trim() == "NG")
                {
                    yield++;
                }
            }
            uiTextBox7.Text = yield.ToString() + "次`";
            try { uiTextBox9.Text = (((1 - yield / ProductionLog.Rows.Count)) * 100).ToString("0.00") + "%"; }
            catch (Exception ex2)
            {
                showMessage("错误", "funt4,ex2:" + ex2.Message);
            }


        }


        #endregion

        #region 深视测厚仪

        //读取到的数据
        List<double> sszn_out1 = new List<double>();
        List<double> sszn_out2 = new List<double>();
        List<double> sszn_out3 = new List<double>();

        //滤波后的数据
        List<double> sszn_ys1 = new List<double>();
        List<double> sszn_ys2 = new List<double>();
        List<double> sszn_ys3 = new List<double>();
        void connectSensorSszn()
        {
            //指定IP地址连接控制器1、2
            string mIpStr = DataClass.peizhivalues[200];
            string mIpStr2 = DataClass.peizhivalues[201];
            SGIF_OPENPARAM_ETHERNET mIp = new SGIF_OPENPARAM_ETHERNET(mIpStr);
            SGIF_OPENPARAM_ETHERNET mIp2 = new SGIF_OPENPARAM_ETHERNET(mIpStr2);
            bool isConnected1 = false; bool isConnected2 = false;
            Task.Run(() =>
            {
                while (true)
                {
                    SG.RC mRc = SGLinkFuc.SGIF_OpenDeviceETHER(0, out mIp); bool Shown = false;
                    if (mRc == SG.RC.RC_OK)
                    {
                        showMessage("状态", "1号控制器连接成功");
                        isConnected1 = true;
                        break;
                    }
                    else if (!Shown)
                    {
                        showMessage("错误", "1号控制器连接失败，错误代码:" + mRc.ToString());
                        Shown = true;
                    }
                    Task.Delay(1000).Wait();

                }
            });
            Task.Run(() =>
            {
                while (true)
                {
                    SG.RC mRc2 = SGLinkFuc.SGIF_OpenDeviceETHER(1, out mIp2); bool Shown = false;
                    if (mRc2 == SG.RC.RC_OK)
                    {
                        showMessage("状态", "2号控制器连接成功");
                        isConnected2 = true;
                        break;
                    }
                    else if (!Shown)
                    {
                        Shown = true;
                        showMessage("错误", "2号控制器连接失败，错误代码:" + mRc2.ToString());
                    }
                    Task.Delay(1000).Wait();
                }
            });
            Task.Run(() =>
            {
                while (true)
                {
                    if (isConnected1 && isConnected2)
                    {
                        writePLCValue(PCReady, 1);
                        测厚仪连接状态显示(true);
                        break;
                    }
                    Task.Delay(1000).Wait();
                }
            });

        }
        enum status
        {
            开始,
            停止,
            滤波,

        }
        status s = status.开始;
        bool startMeasure = false;
        void measuringSszn()
        {
            
            DateTime dtStart = DateTime.Now;
            while (true)
            {
                Delay(10);
                switch (s)
                {
                    case status.开始:
                        PLCReturnCode = PLC.GetDevice(PLCStart, out plcStart);//PLC给出开始测厚信号后，开始测厚
                        if (plcStart == 1||startMeasure)//plc给出开始测量信号
                        {
                            startMeasure = false;
                            //清之前的测量结果
                            writePLCValue(PCResult, 0);
                            writePLCValue(ConfigurationManager.AppSettings["PCResultAvg"], 0);
                            int[] d7040 = new int[12];
                            PLC.WriteDeviceBlock(r[0], 12, ref d7040[0]);
                            SGLinkFuc.SGIF_DataStorageInit(0);
                            SGLinkFuc.SGIF_DataStorageInit(1);
                            SGLinkFuc.SGIF_DataStorageStart(0);
                            SGLinkFuc.SGIF_DataStorageStart(1);
                            dtStart = DateTime.Now;
                            s = status.停止;
                        }
                        break;

                    case status.停止:
                        PLCReturnCode = PLC.GetDevice(PLCStart, out plcStart);
                        //plc给出停止信号 或 设定n秒后自动停止
                        if (plcStart == 2 || (DateTime.Now - dtStart).TotalSeconds > Convert.ToInt32(DataClass.peizhivalues[203]))
                        {
                            SGLinkFuc.SGIF_DataStorageStop(0);
                            SGLinkFuc.SGIF_DataStorageStop(1);
                            float[] mData = new float[10000];
                            int receiveNum = 0;
                            using (Pt mPt = new Pt(mData))
                            {
                                SGLinkFuc.SGIF_DataStorageGetData(0, 5, mData.Length, mPt.Ptr, out receiveNum);
                                for (int i = 0; i < receiveNum; i++)
                                {
                                    sszn_out1.Add(mData[i]);
                                }
                            }
                            mData = new float[10000];
                            receiveNum = 0;
                            using (Pt mPt = new Pt(mData))
                            {
                                SGLinkFuc.SGIF_DataStorageGetData(0, 6, mData.Length, mPt.Ptr, out receiveNum);
                                for (int i = 0; i < receiveNum; i++)
                                {
                                    sszn_out2.Add(mData[i]);
                                }
                            }
                            mData = new float[10000];
                            receiveNum = 0;
                            using (Pt mPt = new Pt(mData))
                            {
                                SGLinkFuc.SGIF_DataStorageGetData(1, 3, mData.Length, mPt.Ptr, out receiveNum);
                                for (int i = 0; i < receiveNum; i++)
                                {
                                    sszn_out3.Add(mData[i]);
                                }
                            }

                            s = status.滤波;
                        }
                        break;

                    case status.滤波:

                        sszn_funt1_1();

                        sszn_funt1_2();

                        sszn_funt1_3();

                        sszn_out1.Clear();
                        sszn_out2.Clear();
                        sszn_out3.Clear();

                        s = status.开始;
                        break;

                    default:
                        s = status.开始;
                        break;

                }
            }


        }

        //数据处理
        void sszn_funt1_1()
        {
            try
            {
                //中值滤波宽度
                int degree = int.Parse(DataClass.peizhivalues[202]);


                //中值滤波
                ys1 = medianfilter(sszn_out1, degree);
                ys2 = medianfilter(sszn_out2, degree);
                ys3 = medianfilter(sszn_out3, degree);


                avg1 = ys1.Average();
                avg2 = ys2.Average();
                avg3 = ys3.Average();
                avg123 = (avg1 + avg2 + avg3) / 3;

                //自动识别配方
                if (DataClass.peizhivalues[2] == "1")
                    DataClass.pfid = funt3(avg123);
                //是否显示取样点4
                funt6(DataClass.peizhivalues[11] != "9");

            }
            catch { }
        }

        //数据曲线显示
        void sszn_funt1_2()
        {
            //数据分配
            d11 = 0; d12 = 0; d13 = 0; d14 = 0; d21 = 0; d22 = 0; d23 = 0; d24 = 0; d31 = 0; d32 = 0; d33 = 0; d34 = 0;

            double[] dys = new double[4];
            dys[0] = double.Parse(DataClass.peifangvalues[5]);//配方取样点1
            dys[1] = double.Parse(DataClass.peifangvalues[6]);
            dys[2] = double.Parse(DataClass.peifangvalues[7]);
            dys[3] = double.Parse(DataClass.peifangvalues[8]);

            //曲线显示
            xianshi(ys1, dys, lineChart1, setChartOption);
            xianshi(ys2, dys, lineChart2, setChartOption);
            xianshi(ys3, dys, lineChart3, setChartOption);

            //正态分布曲线显示
            xianshi1(uiLineChart1, setChartOption4);


            //获取取样点数据
            var dy1 = funt5(ys1, dys, 1);
            var dy2 = funt5(ys2, dys, 2);
            var dy3 = funt5(ys3, dys, 3);

            d11 = dy1[0]; d12 = dy1[1]; d13 = dy1[2]; d14 = dy1[3];
            txb1.Text = d11.ToString(precision) + "mm";
            txb2.Text = d12.ToString(precision) + "mm";
            txb3.Text = d13.ToString(precision) + "mm";
            txb4.Text = d14.ToString(precision) + "mm";

            d21 = dy2[0]; d22 = dy2[1]; d23 = dy2[2]; d24 = dy2[3];
            txb5.Text = d21.ToString(precision) + "mm";
            txb6.Text = d22.ToString(precision) + "mm";
            txb7.Text = d23.ToString(precision) + "mm";
            txb8.Text = d24.ToString(precision) + "mm";

            d31 = dy3[0]; d32 = dy3[1]; d33 = dy3[2]; d34 = dy3[3];
            txb9.Text = d31.ToString(precision) + "mm";
            txb10.Text = d32.ToString(precision) + "mm";
            txb11.Text = d33.ToString(precision) + "mm";
            txb12.Text = d34.ToString(precision) + "mm";

            avg = avg123;

            //平均值
            uiTextBox5.Text = avg.ToString(precision) + "mm";
        }

        //结果判定、记录
        void sszn_funt1_3()
        {
            try
            {
                //测量结束后，给plc写入测量结果
                //writePLCValue(ConfigurationManager.AppSettings["PCResultAvg"], (int)(avg * iprecision));

                double[] doubles1 = { d11, d12, d13, d14, d21, d22, d23, d24, d31, d32, d33, d34 };

                var ds = doubles1.Select(s => (int)s * iprecision).ToArray();

                PLC.WriteDeviceBlock(r[0], 12, ref ds[0]);

            }
            catch (Exception ex1)
            {
                showMessage("错误", "函数sszn_funt1_3，ex1: " + ex1.Message);
            }

            double max; double min; double max_min;
            double[] doubles = null;

            if (DataClass.peizhivalues[11] == "9")
            {
                doubles = new double[] { d11, d12, d13, d21, d22, d23, d31, d32, d33 };
            }
            else
            {
                doubles = new double[] { d11, d12, d13, d14, d21, d22, d23, d24, d31, d32, d33, d34 };
            }

            max = doubles.Max();
            min = doubles.Min();
            max_min = max - min;
            string result = "";

            uiTextBox2.Text = max.ToString(precision) + "mm";
            uiTextBox3.Text = min.ToString(precision) + "mm";
            uiTextBox4.Text = max_min.ToString(precision) + "mm";

            //plc写入判断结果
            if (DataClass.peizhivalues[10] == "0")
            {
                //极差法判定
                double sv_max_min = double.Parse(DataClass.peifangvalues[1]);
                if (max_min > sv_max_min)
                {
                    ShowErrorNotifier("产品极差超出设定值，请检查产品是否异常", false, 900000);
                    showMessage("错误", "产品极差超出设定值");
                    writePLCValue(PCResult, 2);
                    result = "NG";
                }
                else
                {
                    writePLCValue(PCResult, 1);
                    result = "OK";
                    showMessage("状态", "产品OK");

                }
            }
            else if (DataClass.peizhivalues[10] == "1")
            {
                //上下限法判定
                //判定是否整板取样
                if (DataClass.peifangvalues[11] == "1")
                {
                    if (funt7(ys1) || funt7(ys2) || funt7(ys3))
                    {
                        ShowErrorNotifier("产品的最大值或最小值超出设定值，请检查产品是否异常", false, 900000);
                        showMessage("错误", "产品的最大值或最小值超出设定值，请检查产品是否异常");
                        writePLCValue(PCResult, 2);
                    }
                    else
                    {
                        writePLCValue(PCResult, 1);
                        showMessage("状态", "产品OK");
                    }
                }
                else
                {
                    result = ngcount(doubles);

                    if (result == "NG")
                    {
                        ShowErrorNotifier("产品的最大值或最小值超出设定值，请检查产品是否异常", false, 900000);
                        showMessage("错误", "产品的最大值或最小值超出设定值，请检查产品是否异常");
                        writePLCValue(PCResult, 2);
                    }
                    else
                    {
                        writePLCValue(PCResult, 1);
                        showMessage("状态", "产品OK");
                    }
                }
            }


            if (max == 0 || min == 0)
            {
                result = "NG";
                writePLCValue(PCResult, 2);
            }

            //生产记录
            productionrecords(result, max, min);
        }


        #endregion


        #region 统计图表参数设置

        /// <summary>
        /// 设定折线图表1
        /// </summary>
        void setChartOption1()
        {
            double svhd = double.Parse(DataClass.peifangvalues[0]);
            double y1 = double.Parse(DataClass.peifangvalues[10]);

            UILineOption option = new UILineOption();
            option.ToolTip.Visible = true;
            option.Title = new UITitle();
            option.Title.Text = "测厚数据曲线";
            option.Title.SubText = "1号测量头";
            option.YAxis.SetRange(svhd-y1/2, svhd + y1 / 2);
            option.XAxis.AxisLabel.DecimalPlaces = 0;//坐标轴显示小数位数
            option.YAxis.AxisLabel.DecimalPlaces = 3;
            option.XAxis.Name = "采样点数/个";
            option.YAxis.Name = "厚度/mm";
            var series = option.AddSeries(new UILineSeries("厚度曲线"));

            series.YAxisDecimalPlaces = 3;
            lineChart1.SetOption(option);
            lineChart1.FillColor = Color.FromArgb(243, 249, 255);
        }

        /// <summary>
        /// 设定折线图表2
        /// </summary>
        void setChartOption2()
        {
            double svhd = double.Parse(DataClass.peifangvalues[0]);
            double y1 = double.Parse(DataClass.peifangvalues[10]);

            UILineOption option = new UILineOption();
            option.ToolTip.Visible = true;
            option.Title = new UITitle();
            option.Title.Text = "测厚数据曲线";
            option.Title.SubText = "2号测量头";
            option.YAxis.SetRange(svhd - y1 / 2, svhd + y1 / 2);
            option.XAxis.AxisLabel.DecimalPlaces = 0;//坐标轴显示小数位数
            option.YAxis.AxisLabel.DecimalPlaces = 3;
            option.XAxis.Name = "采样点数/个";
            option.YAxis.Name = "厚度/mm";
            var series = option.AddSeries(new UILineSeries("厚度曲线"));
            series.YAxisDecimalPlaces = 3;
            lineChart2.SetOption(option);
            lineChart2.FillColor = Color.FromArgb(243, 249, 255);
        }

        /// <summary>
        /// 设定折线图表3
        /// </summary>
        void setChartOption3()
        {
            double svhd = double.Parse(DataClass.peifangvalues[0]);
            double y1 = double.Parse(DataClass.peifangvalues[10]);

            UILineOption option = new UILineOption();
            option.ToolTip.Visible = true;
            option.Title = new UITitle();
            option.Title.Text = "测厚数据曲线";
            option.Title.SubText = "3号测量头";
            option.YAxis.SetRange(svhd - y1 / 2, svhd + y1 / 2);
            option.XAxis.AxisLabel.DecimalPlaces = 0;//坐标轴显示小数位数
            option.YAxis.AxisLabel.DecimalPlaces = 3;
            option.XAxis.Name = "采样点数/个";
            option.YAxis.Name = "厚度/mm";
            var series = option.AddSeries(new UILineSeries("厚度曲线", Color.Green));
            series.YAxisDecimalPlaces = 3;
            lineChart3.SetOption(option);
            lineChart3.FillColor = Color.FromArgb(243, 249, 255);

        }

        /// <summary>
        /// 设定正态分布图
        /// </summary>
        void setChartOption4(double h,double stddev)
        {
            double svhd = double.Parse(DataClass.peifangvalues[0]);
            double y1 = double.Parse(DataClass.peifangvalues[10]);

            UILineOption option = new UILineOption();
            option.ToolTip.Visible = true;
            option.Title = new UITitle();
            option.Title.Text = "厚度正态分布曲线";
            option.Title.SubText = "";
            option.YAxis.SetRange(0, h);
            option.XAxis.AxisLabel.DecimalPlaces = 3;//坐标轴显示小数位数
            option.YAxis.AxisLabel.DecimalPlaces = 2;
            option.XAxis.Name = "厚度/mm";
            option.YAxis.Name = "概率分布";
            var series = option.AddSeries(new UILineSeries("正态分布曲线"));

            //绘制竖线
            option.XAxisScaleLines.Add(new UIScaleLine() { Color = Color.Lime, Name = "-3σ", Value = avg123 - 3 * stddev });
            option.XAxisScaleLines.Add(new UIScaleLine() { Color = Color.Gold, Name = avg123.ToString("0.000"), Value = avg123 });
            option.XAxisScaleLines.Add(new UIScaleLine() { Color = Color.Lime, Name = "+3σ", Value = avg123 + 3 * stddev });

            series.YAxisDecimalPlaces = 3;
            uiLineChart1.SetOption(option);
            uiLineChart1.FillColor = Color.FromArgb(243, 249, 255);

          
        }

        //设置折线图
        void setChartOption(int[] ix, UILineChart lineChart)
        {
            double svhd = double.Parse(DataClass.peifangvalues[0]);
            double y1 = double.Parse(DataClass.peifangvalues[10]);
            double up = double.Parse(DataClass.peifangvalues[2]);
            double down = double.Parse(DataClass.peifangvalues[3]);

            UILineOption option = new UILineOption();
            option.ToolTip.Visible = true;
            option.Title = new UITitle();
            option.Title.Text = "测厚数据曲线";
            option.Title.SubText = $"{lineChart.TagString}号测量头";
            option.YAxis.SetRange(svhd - y1 / 2, svhd + y1 / 2);
            option.XAxis.AxisLabel.DecimalPlaces = 0;//坐标轴显示小数位数
            option.YAxis.AxisLabel.DecimalPlaces = 3;
            option.XAxis.Name = "采样点数/个";
            option.YAxis.Name = "厚度/mm";
            var series = option.AddSeries(new UILineSeries("厚度曲线"));

            //绘制横线, DashDot = true
            option.YAxisScaleLines.Add(new UIScaleLine() { Color = Color.Red, Name = "上限", Value = up });
            option.YAxisScaleLines.Add(new UIScaleLine() { Color = Color.Red, Name = "下限", Value = down });

            //绘制竖线
            option.XAxisScaleLines.Add(new UIScaleLine() { Color = Color.Lime, Name = "取样点1", Value = ix[0] });
            option.XAxisScaleLines.Add(new UIScaleLine() { Color = Color.Gold, Name = "取样点2", Value = ix[1] });
            option.XAxisScaleLines.Add(new UIScaleLine() { Color = Color.Lime, Name = "取样点3", Value = ix[2] });

            if (DataClass.peizhivalues[11] != "9")
                option.XAxisScaleLines.Add(new UIScaleLine() { Color = Color.Gold, Name = "取样点4", Value = ix[3] });

            series.YAxisDecimalPlaces = 3;
            lineChart.SetOption(option);
            lineChart.FillColor = Color.FromArgb(243, 249, 255);
        }

        #endregion

        #region PLC

        /// <summary>
        /// 写入plc
        /// </summary>
        /// <param name="Device"></param>
        /// <param name="Data"></param>
        void writePLCValue(string Device, int Data)
        {
            PLCReturnCode = PLC.SetDevice(Device, Data);
        }

        int plcHeartbeat = 0; int pcHeartbeat = 0; DateTime LatestHeartbeat = DateTime.Now; bool PLCDisconnected = true;
        /// <summary>
        /// PLC断线重连
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void timer1_Tick(object sender, EventArgs e)
        {
            PLC.GetDevice(PLCHeartBeat, out plcHeartbeat);
            PLC.GetDevice(PCHeartBeat, out pcHeartbeat);
            if (plcHeartbeat == pcHeartbeat && plcHeartbeat != 0)//心跳
            {
                pcHeartbeat = plcHeartbeat == 1 ? 2 : 1;
                writePLCValue(PCHeartBeat, pcHeartbeat);
                LatestHeartbeat = DateTime.Now;
                if (PLCDisconnected == false)
                {
                    toolStripStatusLabel4.Text = "在线";
                    toolStripStatusLabel4.BackColor = Color.Green;
                    showMessage("状态", "PLC连接成功");
                    PLCDisconnected = true;
                }
            }

            if ((DateTime.Now - LatestHeartbeat).TotalSeconds > 10)//PLC自重连
            {
                PLCReturnCode = PLC.Open();
                if (PLCReturnCode == 0)
                {
                    toolStripStatusLabel4.Text = "在线";
                    toolStripStatusLabel4.BackColor = Color.Green;
                    showMessage("状态", "PLC连接成功");
                }
                else
                {
                    if (PLCDisconnected)
                    {

                        toolStripStatusLabel4.Text = "离线";
                        toolStripStatusLabel4.BackColor = Color.Red;
                        showMessage("状态", "PLC心跳断开连接");
                        PLCDisconnected = false;
                    }
                }
            }
        }

        /// <summary>
        /// 获取PLC字符型数据
        /// </summary>
        /// <param name="plcDevice"></param>
        /// <returns></returns>
        string plcGetString(int plcDevice)
        {

            int[] values = new int[10];
            string dizhi = $"D{plcDevice}";

            PLC.ReadDeviceBlock(dizhi, 10, out values[0]);

            string asciiString = "";
            string output = "";
            try
            {
                for (int j = 0; j < 10; j++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        char c = (char)((values[j] >> (8 * i)) & 0xFF); // 获取每个字节，并转换为char类型
                        asciiString += c.ToString(); // 拼接成字符串

                    }
                    output += asciiString;
                    asciiString = "";
                }
            }
            catch { }
            return output;
        }


        #endregion

        #region 公用方法

        //判断是否有区域超出设定范围
        bool funt7(List<double> ys)
        {
            bool ok = false;

            //采用距离判定
            //double length = double.Parse(DataClass.peifangvalues[DataClass.pfid * DataClass.pfcscount + 14]);//钢板长度mm
            //double dnum = ys.Count;//测量到的点数
            //double dx = length / dnum;//两点之间的长度

            //double dy1 = double.Parse(DataClass.peifangvalues[DataClass.pfid * DataClass.pfcscount + 15]);//报警的长度mm

            //int k = (int)(dy1 / dx);//计算出要报警的点数
      
            //int count = 0;//记录异常的区域数量
            //List<double> ly = new List<double>();//记录异常区域的长度

            //采用百分比判定
            double dnum = ys.Count;//测量到的点数
            double ng = double.Parse(DataClass.peifangvalues[12]);//NG百分比

            double up = double.Parse(DataClass.peifangvalues[2]);//上限
            double down = double.Parse(DataClass.peifangvalues[3]);//下限

            int i = 0;//记录连续异常的数量
            int k = (int)(dnum * ng);//计算出要报警的点数

            foreach (var y in ys)
            {
                if (y < down || y > up)
                    i++;
                else
                {
                    ////采用距离判定
                    //if (ok)
                    //{
                    //    ly.Add(i * dx);
                    //    count++;
                    //}
                    //i = 0;
                    //ok = false;
                }

                if (i >= k)
                {
                    ok = true;
                }

            }
            return ok;

            //return count > 0;
        }

        //获取取样点数据
        double[] funt5(List<double> ys, double[] dys, int num)
        {
            try
            {

                int y1 = (int)(ys.Count * (dys[0] / 100));//取样点数据1
                int y2 = (int)(ys.Count * (dys[1] / 100));
                int y3 = (int)(ys.Count * (dys[2] / 100));
                int y4 = (int)(ys.Count * (dys[3] / 100));

                return new double[] { ys[y1], ys[y2], ys[y3], ys[y4] };

            }
            catch
            {
                ShowErrorNotifier($"测厚头{num}号数据异常，请检查", false, 900000);
                showMessage("错误", $"测厚头{num}号数据异常，请检查");
                return new double[] { 0.0, 0.0, 0.0, 0.0 };
            }
        }

        //
        void 测厚仪连接状态显示(bool zt)
        {
            if (zt)
            {
                toolStripStatusLabel2.Text = "在线";
                toolStripStatusLabel2.BackColor = Color.Green;
            }
            else
            {
                toolStripStatusLabel2.Text = "离线";
                toolStripStatusLabel2.BackColor = Color.Red;
            }
        }


        //曲线显示
        void xianshi(List<double> dy, double[] dys, UILineChart lineChart, Action<int[], UILineChart> action)
        {
            double[] dx = new double[dy.Count];
            for (int i = 0; i < dy.Count; i++)
            {
                dx[i] = (double)i;
            }

            //取样点位置
            int[] iy = { (int)(dy.Count * (dys[0] / 100)), (int)(dy.Count * (dys[1] / 100)), (int)(dy.Count * (dys[2] / 100)), (int)(dy.Count * (dys[3] / 100)) };


            lineChart.Option.Clear();
            action(iy, lineChart);
            lineChart.Option.AddData("厚度曲线", dx, dy.ToArray());
            lineChart.Refresh();

        }

        //正态曲线显示
        void xianshi1(UILineChart lineChart, Action<double, double> action)
        {
            if (ys1.Count == 0 && ys2.Count == 0 && ys3.Count == 0)
                return;


            double[] dy = new double[100];
            double[] dx = new double[100];


            List<double> doubles = ys1.Copy();
            doubles.AddRange(ys2);
            doubles.AddRange(ys3);

            double stdDev = Get_stdDev(doubles);

            double up = double.Parse(DataClass.peifangvalues[2]);
            double down = double.Parse(DataClass.peifangvalues[3]);

            double cpk = (up - avg123) / (3 * stdDev) > (avg123 - down) / (3 * stdDev) ? (avg123 - down) / (3 * stdDev) : (up - avg123) / (3 * stdDev);
            uiTextBox1.Text = cpk.ToString("0.000");

            for (int i = 0; i < dy.Length; i++)
            {
                double x = avg123 + (i - dx.Length / 2.0) * stdDev / 4;

                dy[i] = NormalDistribution(x, avg123, stdDev);
                dx[i] = x;
            }



            action(dy.Max() + 0.5, stdDev);
            lineChart.Option.AddData("正态分布曲线", dx, dy);
            lineChart.Refresh();

        }

        //中值滤波
        List<double> medianfilter(List<double> ld1, int degree)
        {
            List<double> ly = new List<double>();
            List<double> ly1 = new List<double>();
            List<double> ly2 = new List<double>();

            var selectld = ld1.Where(w => w > 0 && w < 3).ToList();

            if (selectld.Count == 0)
            {
                for (int i = 0; i < 100; i++)
                {
                    selectld.Add(0);
                }
            }


            for (int i = 0; i < selectld.Count; i++)
            {
                ly.Add(selectld[i]);
                if (i >= degree - 1)
                {
                    ly1 = ly.Copy();
                    ly1.Sort();
                    ly2.Add(ly1[degree / 2 + 1]);
                    ly.RemoveAt(0);
                }
            }

            return ly2;
        }

        //生产记录
        void productionrecords(string result, double max, double min)
        {
            try
            {
                var prod = new productionlog
                {
                    datee = DateTime.Now.ToString("G"),
                    MaterialNum = uiTextBox8.Text,
                    LotNum = uiTextBox6.Text,
                    settingThickness = DataClass.peifangvalues[0],
                    settingMax = DataClass.peifangvalues[2],
                    settingMin = DataClass.peifangvalues[3],
                    settingMax_min = DataClass.peifangvalues[1],
                    result = result,
                    max = max.ToString(precision),
                    min = min.ToString(precision),
                    max_min = (max - min).ToString(precision),
                    avg = avg.ToString(precision),
                    totalcount = DtChart1.Rows.Count,
                    d11 = d11.ToString(precision),
                    d12 = d12.ToString(precision),
                    d13 = d13.ToString(precision),
                    d14 = d14.ToString(precision),
                    d21 = d21.ToString(precision),
                    d22 = d22.ToString(precision),
                    d23 = d23.ToString(precision),
                    d24 = d24.ToString(precision),
                    d31 = d31.ToString(precision),
                    d32 = d32.ToString(precision),
                    d33 = d33.ToString(precision),
                    d34 = d34.ToString(precision),
                    钢板ID = code
                };

                fsql.Insert(prod).ExecuteIdentity();

                if (DataClass.peizhivalues[1] == "1" && DataClass.peizhivalues[3] == "1")
                {
                    var gbjl = fsql.Select<钢板记录>().Where(w => w.钢板ID == code).ToList();
                    if (gbjl.Count > 0)
                    {
                        int num = gbjl[0].生产次数 + 1;
                        fsql.Update<钢板记录>().Set(s => s.生产次数 == num).Where(w => w.钢板ID == code).ExecuteAffrowsAsync();
                    }
                    else
                    {
                        var gb = new 钢板记录()
                        {
                            钢板ID = code,
                            设定次数 = 100,
                            生产次数 = 1
                        };
                        fsql.Insert(gb).ExecuteAffrowsAsync();
                    }
                }

            }
            catch (Exception ex1)
            {
                showMessage("错误", "funt4,ex1:" + ex1.Message);
            }

            //显示今日NG次数及良率
            DateTime dt;
            dt = DateTime.Now;
            try
            {
                ProductionLog = fsql.Select<productionlog>()
                    .Where(a => Convert.ToDateTime(a.datee) > dt.AddDays(-1)
                    && Convert.ToDateTime(a.datee) <= dt).ToDataTable();

            }
            catch { }
            double yield = 0;
            for (int i = 0; i < ProductionLog.Rows.Count; i++)
            {
                if (ProductionLog.Rows[i][8].ToString().Trim() == "NG")
                {
                    yield++;
                }
            }
            uiTextBox7.Text = yield.ToString() + "次`";
            try { uiTextBox9.Text = (((1 - yield / ProductionLog.Rows.Count)) * 100).ToString("0.00") + "%"; }
            catch (Exception ex2)
            {
                showMessage("错误", "funt4,ex2:" + ex2.Message);
            }


        }


        // 正态分布的概率密度函数  
        double NormalDistribution(double x, double mean, double stdDev)
        {
            return Math.Exp(-(x - mean) * (x - mean) / (2 * stdDev * stdDev)) / (Math.Sqrt(2 * Math.PI) * stdDev);
        }

        //计算标准差
        double Get_stdDev(List<double> doubles)
        {
            double stdDev2 = 0;
            foreach (double d in doubles)
            {
                stdDev2 = stdDev2 + (d - avg123) * (d - avg123);
            }

            return Math.Sqrt(stdDev2 / doubles.Count);
        }

        //自动识别配方
        int funt3(double thickness)
        {
            int index = -1;


            for (int i = 0; i < DataClass.pfnum; i++)
            {
                double d2 = DataClass.gangbans[i].产品厚度 + DataClass.gangbans[i].产品厚度 * DataClass.gangbans[i].自动识别范围 / 100;
                double d3 = DataClass.gangbans[i].产品厚度 - DataClass.gangbans[i].产品厚度 * DataClass.gangbans[i].自动识别范围 / 100;

                if (d3 < thickness && thickness < d2)
                {
                    index = i;
                    break;
                }
            }

            if (index < 0)
            {
                showMessage("warning", "没有对应测量参数");
                index = 0;
            }


            return index;
        }

        //取样点4是否显示
        void funt6(bool zt)
        {
            Invoke(new Action(() =>
            {
                txb4.Visible = zt;
                txb8.Visible = zt;
                txb12.Visible = zt;
                uiLine4.Visible = zt;
                uiLine6.Visible = zt;
                uiLine11.Visible = zt;
            }));


        }


        //取样点NG数判定
        string ngcount(double[] points)
        {
            int num = 0;
            for (int i = 0; i < points.Length; i++)
            {
                if (DataClass.peizhivalues[2] == "0")
                {
                    if (points[i] > double.Parse(DataClass.peifangvalues[2]) || points[i] < double.Parse(DataClass.peifangvalues[3]))
                    {
                        num++;
                    }
                }
                else
                {
                    //自动判定钢板
                    double up = 0;
                    double down = 0;
                    if (DataClass.peizhivalues[12]=="0")
                    {
                        //百分比法
                        up = DataClass.gangbans[DataClass.pfid].产品厚度 + DataClass.gangbans[DataClass.pfid].产品厚度 * double.Parse(DataClass.peifangvalues[4]) / 100;
                        down = DataClass.gangbans[DataClass.pfid].产品厚度 - DataClass.gangbans[DataClass.pfid].产品厚度 * double.Parse(DataClass.peifangvalues[4]) / 100;
                    }
                    else
                    {
                        //固定差值法
                        up = DataClass.gangbans[DataClass.pfid].产品厚度 + double.Parse(DataClass.peifangvalues[13]);
                        down = DataClass.gangbans[DataClass.pfid].产品厚度 - double.Parse(DataClass.peifangvalues[13]);
                    }

                    if (points[i] < down || points[i] > up)
                    {
                        num++;
                    }
                }
            }

            return num >= int.Parse(DataClass.peifangvalues[9]) ? "NG" : "OK";
        }


        #endregion


        /// <summary>
        /// 右下角消息记录插入一条消息
        /// </summary>
        /// <param name="msgType">status、info、warning、error</param>
        /// <param name="msg">消息内容</param>
        void showMessage(string msgType, string msg)
        {
            this.Invoke(new Action(delegate
            {
                dtMsg.Rows.Add(msgType, msg, DateTime.Now.ToString("HH:mm:ss"));
                dgvMsg.FirstDisplayedScrollingRowIndex = this.dgvMsg.Rows.Count - 1;//自动下拉到最后一行
                                                                                    //消息超过一定数量就删除一部分
                if (dgvMsg.Rows.Count > 50)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        dtMsg.Rows[0].Delete();
                    }
                }
            }));

        }

        /// <summary>
        /// 手动测厚按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            showMessage("状态", "点击按钮");
            measuringStatus = "正在测厚……";
        }


        void writecsv(string filemane, double[] data)
        {
            string filePath = $@".\data\{filemane}\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                try
                {
                    for (int j = 0; j < data.Length; j++)
                    {
                        sw.WriteLine(data[j].ToString("0.000"));
                    }
                }
                catch
                {

                } 
            }
        }

        void writecsv1(string filemane, DataTable dt)
        {
            using (StreamWriter sw = new StreamWriter($"E:\\测厚数据\\{filemane}号测量头测厚数据\\" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".csv"))
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    sw.WriteLine(Convert.ToDouble(dt.Rows[j][0]).ToString("0.000"));
                }
            }
        }



        /// <summary>
        /// 延时
        /// </summary>
        /// <param name="mm"></param>
        public static void Delay(int mm)
        {
            DateTime current = DateTime.Now;
            while (current.AddMilliseconds(mm) > DateTime.Now)
            {
                Application.DoEvents();
            }
            return;
        }


        bool stop = false;

        private void uiButton3_Click(object sender, EventArgs e)
        {
            stop = true;
        }


        /// <summary>
        /// 测试用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void uiButton2_Click(object sender, EventArgs e)
        {
            PLC.SetDevice("w110", 1);
            //ProductionLog = fsql.Select<productionlog>().ToDataTable();
        }



        private void uiGroupBox1_Click(object sender, EventArgs e)
        {
          
        }

        private void uiButton5_Click(object sender, EventArgs e)
        {
            if (DataClass.peizhivalues[0] == "2")
            {
                startMeasure = true;
            }
           else if (DataClass.peizhivalues[0] == "0")
            {
                startdt = DateTime.Now;
                //初始化数据存储
                result = LKIF2.LKIF2_DataStorageInit();
                //开始数据存储
                result = LKIF2.LKIF2_DataStorageStart();

                Task.Run(() =>
                {
                    while (true)
                    {

                        uiButton5.Enabled = false;
                        if (DateTime.Now >= startdt.AddSeconds(int.Parse(DataClass.peizhivalues[7])))
                        {
                            //停止数据存储
                            result = LKIF2.LKIF2_DataStorageStop();

                            //读取存储的数据
                            LKIF2.LKIF_FLOATVALUE[] OutBuffer1 = new LKIF2.LKIF_FLOATVALUE[1200001];
                            LKIF2.LKIF_FLOATVALUE[] OutBuffer2 = new LKIF2.LKIF_FLOATVALUE[1200001];
                            LKIF2.LKIF_FLOATVALUE[] OutBuffer3 = new LKIF2.LKIF_FLOATVALUE[1200001];
                            int numReceived = 0;
                            result = LKIF2.LKIF2_DataStorageGetData(6, 100000, ref OutBuffer1[0], ref numReceived);
                            result = LKIF2.LKIF2_DataStorageGetData(7, 100000, ref OutBuffer2[0], ref numReceived);
                            result = LKIF2.LKIF2_DataStorageGetData(8, 100000, ref OutBuffer3[0], ref numReceived);

                            out7 = OutBuffer1.Select(s => (double)s.value).ToList();
                            out8 = OutBuffer2.Select(s => (double)s.value).ToList();
                            out9 = OutBuffer3.Select(s => (double)s.value).ToList();

                            //对测厚数据进行处理
                            keyence_funt4_2();

                            keyence_funt4_3();

                            keyence_funt4_4();

                            out7.Clear();
                            out8.Clear();
                            out9.Clear();
                            break;
                        }
                    }
                    uiButton5.Enabled = true;
                });

            }

        }
    }
}

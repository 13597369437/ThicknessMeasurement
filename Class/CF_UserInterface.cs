using CF_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace CF_Library
{
    //异步通知事件类型
    public enum EventTypeDef
    {
        EventType_DataRecv = 0              //事件类型:接收数据
    }

    //返回过程数据的RID
    public enum ConfocalDataRid_t
    {
        RID_RESULT = 0,                     //测量结果
        RID_IO_BOARD_TEMP_OVERLOAD,         //温度异常
        RID_IO_BOARD_FAN_ERROR,             //风扇停转
        RID_TOLERANCE_ERROR,                //公差异常
        RID_SIGNAL_ERROR,                   //信号异常
        RID_DEVICE_DISCONNECT,              //设备断开连接
        RID_API_CALL_EXCEPTION,             //API调用异常
        RID_ENCODER_COUNT,                  //编码器计数值
        RID_CACHE_REACH_THRES,              //Cache缓存数据达到阈值
        RID_IO_ASYNC_EVENT                  //控制器IO口异步事件
    }

    //错误码定义
    public enum StatusTypeDef
    {
        Status_Succeed = 0,
        Status_Others = -1,
        Status_Offline = -2,
        Status_NoDevice = -3,
        Status_DeviceAlreadyOpen = -4,
        Status_DeviceNumberExceedLimit = -5,
        Status_OpenDeviceFailed = -6,
        Status_InvalidPara = -7,
        Status_Timeout = -8,
        Status_DeviceNotFound = -9,
        Status_NotStart = -10,
        Status_InvalidState = -11,
        Status_OutOfRange = -12,
        Status_ParaNotExist = -13,
        Status_NoSignal = -14,
        Status_FileNotFound = -15,
        Status_NoLicense = -16,
        Status_LicenseExpired = -17,
        Status_LoadLibFailed = -18,
        Status_EnvCheckError = -19,
        Status_ErrorSDKVersion = -20,
        Status_NoParaMatch = -21,
        Status_ReadOnlyParam = -22
    }

    //预设的积分时间,单位us
    public enum PresetExposureTime_t
    {
        ExposureTime_20 = 20,
        ExposureTime_50 = 50,
        ExposureTime_100 = 100,
        ExposureTime_200 = 200,
        ExposureTime_400 = 400,
        ExposureTime_700 = 700,
        ExposureTime_1000 = 1000,
        ExposureTime_1500 = 1500
    }

    //触发模式选择
    public enum Confocal_TriggerMode_t
    {
        Trigger_Internal = 0,           //内部连续触发
        Trigger_Reserve,
        Trigger_Encoder,                //编码器触发
        Trigger_Timing,                 //内部定时触发
        Trigger_SingleShot              //内部单次触发
    }

    //编码器输入模式
    public enum Confocal_EncoderInputMode_t
    {
        Mode_1_INC_1 = 0,               //一相一递增	
        Mode_2_INC_1 = 1,               //两相一递增	
        Mode_2_INC_2 = 2,               //两相两递增	
        Mode_2_INC_4 = 3                //两相四递增	
    }

    //编码器工作模式
    public enum Confocal_EncoderWorkingMode_t
    {
        Mode_Three_Signal_End = 0,      //三个单端
        Mode_Diff_One_Signal_End = 1    //一个差分一个单端
    }

    //外部触发源
    public enum Confocal_ExternTriggerSource_t
    {
        Sync_In_0 = 0                   //外部触发源0
    }

    //外部触发功能
    public enum Confocal_ExtTriggerFunc_t
    {
        Trigger_DirectCapture = 0,      //直接触发
        Trigger_CacheCapture,           //缓存触发
        Trigger_Zero                    //测量值清零
    }

    //自动调光模式
    public enum Confocal_AutoLightMode_t
    {
        AutoLight_MaxIntensity = 0,     //按最强的波峰调光
        AutoLight_WeakIntensity         //按最弱的波峰进行调光
    }

    //警报类型
    public enum AlarmType_t
    {
        AlarmType_None,                 //无报警
        AlarmType_UpperLimit,           //上公差报警
        AlarmType_LowerLimit,           //下公差报警
        AlarmTyp_DeviceDisconnect,      //设备断开报警
        AlarmType_SignalWeak,           //信号弱报警
        AlarmType_SignalSaturated,      //信号饱和报警
        AlarmType_TempError,            //温度异常报警
        AlarmType_FanError              //风扇停转报警
    }

    //输入口触发功能
    public enum Confocal_InputPortFunc_t
    {
        InputPort_None = 0,
        InputPort_ExtTrigger,           //外部触发
        InputPort_ExtTriggerCache,      //外部触发,使用Cache数据
        InputPort_Zero,                 //对选定通道进行zero
        InputPort_StartSample,          //启动采样
        InputPort_StopSample,           //停止采样
        InputPort_SampleToggle,         //采样状态翻转
        InputPort_ClearCache,           //清空内部缓存
        InputPort_EnableCache,          //使能Cache缓存
        InputPort_DisableCache,         //关闭Cache缓存
        InputPort_R_Start_F_Stop,       //上升沿启动采集，下降沿停止采集
        InputPort_R_Stop_F_Start,       //上升沿停止采集，下降沿开始采集
        InputPort_RF_Async_Notice,      //IO口双边沿回调函数异步通知，SDK内部不做任何处理
        InputPort_Async_Notice          //IO口上升沿回调函数异步通知，SDK内部不做任何处理
    }

    //错误警报时IO开关状态
    public enum IoPortState_t
    {
        PortState_Off = 0,              //IO断开
        PortState_On = 1                //IO闭合
    }

    //测量单位
    public enum Confocal_MeasuretUnit_t
    {
        MeasuretUnit_mm = 0,            //毫米
        MeasuretUnit_um,                //微妙
        MeasuretUnit_inch               //英寸
    }

    //通道测量模式
    public enum Confocal_MeasureMode_t
    {
        MeasureMode_Distance = 0,       //距离模式
        MeasureMode_Thickness           //厚度模式
    }

    //模拟增益
    public enum Confocal_Gain_t
    {
        Gain_1 = 1,
        Gain_2,
        Gain_3,
        Gain_4
    }

    // 多通道协同测量下的测量模式
    public enum Confocal_CooperationMeasureMode_t
    {
        CM_Thickness = 0,               //双头测厚
    }

    //单距离模式下，选择哪个信号用于计算
    public enum Confocal_SignalSelect_t
    {
        Signal_MaxIntensity,            //光强最强的信号
        Signal_NearEnd,                 //最近端信号
        Signal_FarEnd                   //最远端信号
    }

    //多波峰模式下，信号排序
    public enum Confocal_SignalSort_t
    {
        Signal_Sort_Index = 0,           //按CMOS索引，从左到右排序
        Signal_Sort_Near_To_Far,         //按近端到远端排序
        Signal_Sort_Far_To_Near          //按远端到近端排序
    }

    //通讯协议控制权
    public enum COMM_Protocol_Control_Enum_t
    {
        Hardware = 0,                   //默认模式
        Software = 1
    }

    //选择通讯协议
    public enum COMM_Protocol_Enum_t
    {
        RS422_COMM = 0,                 //默认模式
        RS485_COMM = 1,
        RS232_COMM = 3                  //2和3都是RS232通讯
    }

    //通讯波特率
    public enum COMM_BaudRate_Enum_t
    {
        BaudRate_9600 = 0,
        BaudRate_19200 = 1,
        BaudRate_38400 = 2,
        BaudRate_57600 = 3,
        BaudRate_115200 = 4,          //默认波特率
        BaudRate_230400 = 5,
        BaudRate_460800 = 6,
        BaudRate_921600 = 7,
        BaudRate_Max_Num              //多少种波特率选择
    }

    //通讯的校验位,  数据格式：1bit起始位 + 8bit数据位 + 【校验位】 + 1bit停止位
    public enum COMM_Parity_Enum_t
    {
        Even = 0, //设置奇偶校验位，以便设置了位的计数为偶数
        Odd = 1,  //设置奇偶校验位，以便设置了位的计数为奇数
        Mark = 2, //将奇偶校验位设置为 1
        Space = 3,//将奇偶校验位设置为 0
        None = 7  //4~7：没有奇偶校验检查时发生
    }

    //通讯的数据格式
    public enum COMM_Data_Format_Enum_t
    {
        ASCII = 0,
        Hexadecimal = 1 //默认数据格式
    }

    //数据集的属性
    public enum DataSetAttribute_t
    {
        Attribute_MinMax,  //最大最小值
        Attribute_Avg,     //平均值
        Attribute_PtP,     //峰峰值
        Attribute_STD      //标准差
    }

    //网口控制器型号
    public enum DeviceType_t
    {
        HPS_CF2000,
        HPS_CF3000,
        HPS_CF4000,
        HPS_CF3000Lite
    }

    //独立模式下返回的结果
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SC_ResultDataTypeDef_t
    {
        public int channelIndex;                              //独立模式下该结果对应的通道
        public float saturation;                                //信号饱和度
        public int resultLen;                                 //结果的个数，若没使能多距离测量或侧厚度模式，则长度为1，结果存放在result[0]中
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public float[] result;                                    //最多存放10个结果
        public int distanceNumber;                            //厚度模式下波峰的个数
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public float[] distance;                                   //存放厚度模式下每个波峰对应的距离值
        public Int64 signal;                                    //返回信号，仅内部调试有用
        public Int32 signalLength;                              //信号长度
        public Int32 triggerCount;                              //编码器通道0触发计数,只适用于CF2000控制器的编码器触发模式
        public Int32 triggerCount1;                             //编码器通道1触发计数,只适用于CF2000控制器的编码器触发模式
        public Int32 triggerCount2;                            //编码器通道2触发计数,只适用于CF2000控制器的编码器触发模式
        public Int32 bTriggerPass;                             //用于指示是否tirgger pass，必须开启trigger pass 调试功能该变量才有效
    }

    //多传感头协同工作下返回的结果
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MC_ResultDataTypeDef_t
    {
        public int      groupIndex;                //组索引
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public SC_ResultDataTypeDef_t[] channelResult;             //每个通道单独的计算结果
        public float    thickness;                 //双头测厚模式下的计算结果
        public int      resultLen;
        public Int32    triggerCount;              //编码器通道0触发计数,只适用于CF2000控制器的编码器触发模式
        public Int32    triggerCount1;             //编码器通道1触发计数,只适用于CF2000控制器的编码器触发模式
        public Int32    triggerCount2;             //编码器通道2触发计数,只适用于CF2000控制器的编码器触发模式
        public Int32    bTriggerPass;              //用于指示是否tirgger pass，必须开启trigger pass 调试功能该变量才有效
    }

    //异步事件参数
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct EventCallbackArgs_t
    {

        public EventTypeDef eventType;       //事件类型
        public IntPtr data;            //数据
        public int dataLen;         //数据个数
        public int rid;          //数据RID
    }

    //版本信息
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct Version_t
    {
        public byte year;
        public byte month;
        public byte day;
        public byte major;
        public byte minor;
        public byte rev;
    }

    //卡尔曼滤波参数
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct KalmanFilterPara_t
    {
        public float kalman_k;
        public float kalman_threshold;
        public UInt32 num_check;
    }

    //设备描述信息
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DeviceInfo_t
    {
        public int serverIndex;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string descriptor;
    }

    //控制器以太网连接参数
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ControllerGEPara_t
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string controllerIp;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string controllerMAC;
        public UInt16 controllerPort;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ControllerGEParaUnsafe_t
    {
        public IntPtr controllerIp;
        public IntPtr controllerMAC;
        public ushort controllerPort;
    }

    //异步通知回调函数
    // 事件委托类型
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void UserEventCallbackHandleDelegate(int handle, EventCallbackArgs_t arg, IntPtr userPara);


    class CF_UserInterface
    {
        //超出量程或没信号时输出的无效值
        public const double INVALID_VALUE = 888888;
        public const int    CM_MAX_GROUP  = 2;
        static UserEventCallbackHandleDelegate userEventHandle;


        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_ScanDeviceList(ref IntPtr devList, ref int deviceNumber);

        
        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_OpenDevice(IntPtr device, ref int deviceHandler, int deviceMode);

        
        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_GE_OpenDevice(ref ControllerGEParaUnsafe_t controllerPara, IntPtr localIP, ref int deviceHandler, int deviceMode);


        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static void CF_SetFactoryFilePath(byte[] path);


        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static void CF_CloseDevice(int handle);

        
        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_StartSample(int handle, bool en);

       
        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_Zero(int handle, int channelIndex);

        
        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static void CF_RegisterEventCallback(UserEventCallbackHandleDelegate eventHandler, IntPtr userPara);

        
        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_GetLatestResult(int handle, byte[] res, ref int len);

        
        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_GetLatestResult_MC(int handle, byte[] res, ref int len);

        
        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_DarkSignal(int handle, int channel, bool presetExpTime);

       
        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_ExportCacheData(int handle, int cacheIndex, double[] retData, int maxCount, ref Int32 dataCount);

        
        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_SaveSetting(int handle);

       
        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_RestoreFactorySetting(int handle);


        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_ExportUserSetting(int handle, byte[] path);

        
        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_ImportUserSetting(int handle, byte[] pathName);


        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_SetIntParam(int handle,byte[] paramNamePtr,int channelIndex,int value);


        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_SetFloatParam(int handle, byte[] paramNamePtr, int channelIndex, float value);


        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_SetStringParam(int handle, byte[] paramNamePtr, int channelIndex, IntPtr value);


        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_GetIntParam(int handle, byte[] paramName, int channelIndex, ref int value);

 
        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_GetFloatParam(int handle, byte[] paramNamePtr, int channelIndex, ref float value);

 
        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_GetStringParam(int handle, byte[] paramNamePtr, int channelIndex, byte[] value);


        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_BindInputPort(int handle, int Channel, Confocal_InputPortFunc_t func, int inputPort);


        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef CF_UnbindInputPort(int handle, int inputPort);


        //标定功能
        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef setDoubleChannelThicknessK(int handle, int groupIndex, double[] k);

        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef clearDoubleChannelThicknessSamplePoint(int handle, int groupIndex);

        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef setDoubleChannelThicknessSamplePoint(int handle, int groupIndex, float std_thickness);

        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef doDoubleChannelThicknessCal(int handle, int groupIndex, double[] k);

        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static int getDoubleChannelThicknessSamplePoint(int handle, int groupIndex);

        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef getDoubleChannelThicknessK(int handle, int groupIndex, double[] k);

        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef setThicknessRefractivePara(int handle, int channel, int signalIndex1, int signalIndex2, float Nc, float Nd, float Nf);

        [DllImport("hps_cfxxxx_sdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static StatusTypeDef getThicknessRefractivePara(int handle, int channel, int signalIndex1, int signalIndex2, ref float Nc, ref float Nd, ref float Nf);








        //非托管数据和托管数据转换

        /**********************************************************************************
        * HPS_CF_ScanDeviceList
        *  扫描传感器设备列表,适用于USB3.0系列控制器
        * INPUT:
        *	devList:		返回当前所有设备
        *	deviceNumber:	返回设备个数
        *	RETURN:			返回错误码
        **********************************************************************************/
        public static StatusTypeDef HPS_CF_ScanDeviceList(out DeviceInfo_t[] deviceList, out int deviceNumber)
        {
            IntPtr devListPt = IntPtr.Zero;
            deviceNumber = 0;
            deviceList = new DeviceInfo_t[0];

            //扫描设备列表
            StatusTypeDef ret = CF_ScanDeviceList(ref devListPt, ref deviceNumber); 
            if (ret == StatusTypeDef.Status_Succeed && deviceNumber > 0)
            {
                deviceList = new DeviceInfo_t[deviceNumber];

                byte[] deviceListByteArry = new byte[deviceNumber * Marshal.SizeOf<DeviceInfo_t>()];
                Marshal.Copy(devListPt, deviceListByteArry, 0, deviceListByteArry.Length);
                //将byte 数组转换成结构体
                for (int i = 0; i < deviceNumber; i++)
                {
                    deviceList[i] = (DeviceInfo_t)Util.BytesToStuct(deviceListByteArry, i * Marshal.SizeOf<DeviceInfo_t>(), typeof(DeviceInfo_t));
                    Console.WriteLine(deviceList[i].descriptor);
                }
            }

            return ret;
        }


        /**********************************************************************************
        * HPS_CF_OpenDevice
        *	打开指定的光谱仪设备,适用于USB3.0系列控制器
        * INPUT:
        *	deviceList:	    用户指定的传感器设备
        *	handle:	        返回该设备的句柄
        *	RETURN:			返回错误码	
        **********************************************************************************/
        public static StatusTypeDef HPS_CF_OpenDevice(DeviceInfo_t deviceList, ref int handle , int deviceMode)
        {
            StatusTypeDef ret = StatusTypeDef.Status_Succeed;
            IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(DeviceInfo_t)));
            Marshal.StructureToPtr(deviceList, ptr, true);  //false容易造成内存泄漏
            ret = CF_OpenDevice(ptr, ref handle, deviceMode);
            Marshal.FreeHGlobal(ptr);    //free the memory

            return ret;
        }


        /**********************************************************************************
        * HPS_CF_GE_openDevice
        *	打开指定的光谱仪设备,适用于以太网系列控制器
        * INPUT:
        *	controllerPara: 控制器以太网通信参数，如果设置为NULL则使用控制器固定的通信参数进行连接
        *   localIP:        地IP，如果设置为NULL则使用INADDR_ANY绑定本机的所有IP
        *	handle:        返回该设备的句柄
        *	deviceMode:     设备类型选择，DeviceType_t
        *   RETURN:         返回错误码	
        **********************************************************************************/
        public static StatusTypeDef HPS_CF_GE_OpenDevice(ControllerGEPara_t controllerPara, string localIP, ref int handle, int deviceMode)
        {
            StatusTypeDef ret = StatusTypeDef.Status_Succeed;
            ControllerGEParaUnsafe_t para = new ControllerGEParaUnsafe_t();
            para.controllerIp = Marshal.StringToHGlobalAnsi(controllerPara.controllerIp);   //controllerPara.controllerIp、localIP设为null，设备使用绑定的IP
            para.controllerMAC = Marshal.StringToHGlobalAnsi(controllerPara.controllerMAC);
            para.controllerPort = controllerPara.controllerPort;

            IntPtr localIP_ptr = Marshal.StringToHGlobalAnsi(localIP);  //防止乱码
            ret = CF_GE_OpenDevice(ref para, localIP_ptr, ref handle, deviceMode);

            Marshal.FreeHGlobal(localIP_ptr);   //释放分配的非托管内存。 
            return ret;
        }


        /**********************************************************************************
        * HPS_CF_SetFactoryFilePath
        *	设置出厂配置文件搜索路径，适用于CF3000Lite版本控制器;不设置默认在程序运行目录下搜索
        * INPUT:
        *	path:出厂配置文件搜索路径
        **********************************************************************************/
        public static void HPS_CF_SetFactoryFilePath(string path )
        {
            byte[] pathPtr = System.Text.Encoding.ASCII.GetBytes(path);
            CF_SetFactoryFilePath(pathPtr);
        }


        /**********************************************************************************
        * HPS_CF_CloseDevice
        *	关闭指定的传感器设备
        * INPUT:
        *	handle:	用户指定的传感器设备handle
        *   RETURN: 返回错误码
        **********************************************************************************/
        public static void HPS_CF_CloseDevice(int handle)
        {
            CF_CloseDevice(handle);
        }


        /**********************************************************************************
        * HPS_CF_StartSample
        *	启动传感器采集,采集的测量结果通过回调函数返回或通过调用CF_GetLatestResult/CF_GetLatestResult_MC获取最新的测量结果
        * INPUT:
        *	handle:		  用户指定的传感器设备handle
        *   en：	      true:启动采集      false:停止采集
        *	RETURN:		  返回错误码
        ***********************************************************************************/
        public static StatusTypeDef HPS_CF_StartSample(int handle, bool en)
        {
            return CF_StartSample(handle, en);
        }


        /**********************************************************************************
        * HPS_CF_Zero
        *	对传感器测量值进行归零
        * INPUT:
        *	handle:		  用户指定的传感器设备handle
        *   channelIndex：通道索引。单头模式下，该索引取值为0~3，对应4个通道传感头；在双头测厚度非标定模式下，该索引取值为0~1，对应两组双头测量
        *	RETURN:		  返回错误码
        ***********************************************************************************/
        public static StatusTypeDef HPS_CF_Zero(int handle, int channelIndex)
        {
            return CF_Zero(handle, channelIndex);
        }


        /**********************************************************************************
        * CF_RegisterEventCallback
        *	注册总的事件回调函数，连续采集的测量结果/函数调用异常信息通过回调函数通知
        * INPUT:
        *	eventHandle :回调函数
        *	userPara    :用户数据
        ***********************************************************************************/
        public static void HPS_CF_RegisterEventCallback(UserEventCallbackHandleDelegate eventHandle, IntPtr userPara)
        {
            userEventHandle = eventHandle;
            CF_RegisterEventCallback(userEventHandle, userPara);
        }


        /**********************************************************************************
        * HPS_CF_GetLatestResult
        *	获取一帧最新的测量值
        * INPUT:
        *	handle:		用户指定的传感器设备handle
        *	result:		返回所有已激活通达的测量值
        *	len:		返回测量结果个数
        *   RETURN:		返回错误码
        ***********************************************************************************/
        public static StatusTypeDef HPS_CF_GetLatestResult(int handle, out SC_ResultDataTypeDef_t[] result, out int len)
        {
            result = new SC_ResultDataTypeDef_t[4];
            len = 0;
            try
            {
                byte[] bytesArray = new byte[Marshal.SizeOf<SC_ResultDataTypeDef_t>() * 4];
                StatusTypeDef ret = CF_GetLatestResult(handle, bytesArray, ref len);
                if (ret == StatusTypeDef.Status_Succeed)
                {
                    for(int i = 0; i < 4; i++)
                    {
                        result[i] = (SC_ResultDataTypeDef_t)Util.BytesToStuct(bytesArray, i * Marshal.SizeOf<SC_ResultDataTypeDef_t>(), typeof(SC_ResultDataTypeDef_t));
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                return StatusTypeDef.Status_ErrorSDKVersion;
            }
        }


        /**********************************************************************************
        * HPS_CF_GetLatestResult_MC
        *	双头测厚模式下,获取一帧最新的测量值
        * INPUT:
        *	handle:		用户指定的传感器设备handle
        *	result:		返回所有已激活通达的测量值
        *	len:		返回测量结果个数
        *   RETURN:		返回错误码
        ***********************************************************************************/
        public static StatusTypeDef HPS_CF_GetLatestResult_MC(int handle, out MC_ResultDataTypeDef_t[] result, out int len)
        {
            result = new MC_ResultDataTypeDef_t[4];
            len = 0;
            try
            {
                byte[] bytesArray = new byte[Marshal.SizeOf<MC_ResultDataTypeDef_t>() * 2];
                StatusTypeDef ret = CF_GetLatestResult_MC(handle, bytesArray, ref len);
                if (ret == StatusTypeDef.Status_Succeed)
                {
                    for(int i = 0; i < 2; i++)
                    {
                        result[i] = (MC_ResultDataTypeDef_t)Util.BytesToStuct(bytesArray, i * Marshal.SizeOf<MC_ResultDataTypeDef_t>(), typeof(MC_ResultDataTypeDef_t));
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                return StatusTypeDef.Status_ErrorSDKVersion;
            }
        }


        /**********************************************************************************
        * HPS_CF_DarkSignal
        *	消除传感器背景光信号,传感器启动测量前需要先消除背景光信号（将传感器移动到量程外，执行dark操作）。默认出厂已经对传感器执行dark操作，并保存到控制器中。
        * INPUT:
        *	handle:        用户指定的传感器设备handle
        *	channel:       通道,小于0则消除所有通道的dark信号
        *   presetExpTime: true:消除所有预设曝光时间的dark信号,并将数据保存到运行目录,下次运行程序无需进行dark操作；
        *                  false:采集当前曝光时间的dark信号，dark数据在SDK断开连接后失效，下次连接SDK需要重新进行dark
        *   RETURN: 返回错误码
        **********************************************************************************/
        public static StatusTypeDef HPS_CF_DarkSignal(int handle, int channel, bool presetExpTime)
        {
            return CF_DarkSignal(handle, channel, presetExpTime);
        }


        /**********************************************************************************
        * HPS_CF_ExportCacheData
        *	将Cache里面的所有数据都获取出来，用户通过参数PARAM_CACHE_DATA_CNT获取当前Cache里面有多少个数据，在开辟好内存空间将数据获取出去;
        *   通过参数 PARAM_CACHE_CLEAR 可以将Cacnhe内部数据清空
        * INPUT:
        *	handle		:用户指定的传感器设备handle
        *	cacheIndex: ：缓存的索引(0~3)对应4个通道
        *	data		: 返回的数据
        *   maxDataCount: 读取的最大长度
        *	dataCount	：返回的实际读取的数据长度
        *   RETURN: 返回错误码
        **********************************************************************************/
        public static StatusTypeDef HPS_CF_ExportCacheData(int handle, int cacheIndex, double[] retData, int maxCount, ref Int32 dataCount)
        {
            return CF_ExportCacheData(handle, cacheIndex, retData, maxCount, ref dataCount);
        }


        /**********************************************************************************
        * HPS_CF_SaveSetting
        *	保存当前用户配置到控制器中
        * INPUT:
        *	handle:		  用户指定的传感器设备handle
        * RETURN:		  返回错误码
        ***********************************************************************************/
        public static StatusTypeDef HPS_CF_SaveSetting(int handle)
        {
            return CF_SaveSetting(handle);
        }


        /**********************************************************************************
        * HPS_CF_RestoreFactorySetting
        *	恢复出厂配置
        * INPUT:
        *	handle:		  用户指定的传感器设备handle
        *  RETURN:		  返回错误码
        *
        **********************************************************************************/
        public static StatusTypeDef HPS_CF_RestoreFactorySetting(int handle)
        {
            return CF_RestoreFactorySetting(handle);
        }


        /**********************************************************************************
        * HPS_CF_ExportUserSetting
        *	导出传感器配置文件
        * INPUT:
        *	handle:		  用户指定的传感器设备handle
        *	path：		  导出路径
        *   RETURN:		  返回错误码
        *
        **********************************************************************************/
        public static StatusTypeDef HPS_CF_ExportUserSetting(int handle, string path)
        {
            byte[] pathPtr = System.Text.Encoding.ASCII.GetBytes(path);
            return CF_ExportUserSetting(handle, pathPtr);
        }


        /**********************************************************************************
        * HPS_CF_ImportUserSetting
        *	导入传感器配置文件
        * INPUT:
        *	handle:		  用户指定的传感器设备handle
        *	pathName：		  配置文件名
        *   RETURN:		  返回错误码
        *
        **********************************************************************************/
        public static StatusTypeDef HPS_CF_ImportUserSetting(int handle, string pathName)
        {
            byte[] pathNamePtr = System.Text.Encoding.ASCII.GetBytes(pathName);
            return CF_ImportUserSetting(handle, pathNamePtr);
        }


        /**********************************************************************************
        * HPS_CF_SetIntParam
        *	设置Int类型参数
        * INPUT:
        *	handle:		  用户指定的传感器设备handle
        *	paramName：	  参数名， 参数名，包含的参数可以参考CF_ParamterDefine.h文件
        *   channelIndex：通道索引，若是全局参数则该值填0即可，内部不做判断
        *   RETURN:		  返回错误码
        *
        **********************************************************************************/
        public static StatusTypeDef HPS_CF_SetIntParam(int handle, string paramName, int channelIndex, int value)
        {
            byte[] paramNamePtr = System.Text.Encoding.ASCII.GetBytes(paramName);
            return CF_SetIntParam(handle, paramNamePtr, channelIndex, value);
        }


        /**********************************************************************************
        * HPS_CF_SetFloatParam
        *	设置Float类型参数
        * INPUT:
        *	handle:		  用户指定的传感器设备handle
        *	paramName：	  参数名， 参数名，包含的参数可以参考CF_ParamterDefine.h文件
        *   channelIndex：通道索引，若是全局参数则该值填0即可，内部不做判断
        *   RETURN:		  返回错误码
        *
        **********************************************************************************/
        public static StatusTypeDef HPS_CF_SetFloatParam(int handle, string paramName, int channelIndex, float value)
        {
            byte[] paramNamePtr = System.Text.Encoding.ASCII.GetBytes(paramName);
            return CF_SetFloatParam(handle, paramNamePtr, channelIndex, value);
        }


        /**********************************************************************************
        * HPS_CF_SetStringParam
        *	设置String类型参数
        * INPUT:
        *	handle:		  用户指定的传感器设备handle
        *	paramName：	  参数名， 参数名，包含的参数可以参考CF_ParamterDefine.h文件
        *   channelIndex：通道索引，若是全局参数则该值填0即可，内部不做判断
        *	RETURN:		  返回错误码
        *
        **********************************************************************************/
        public static StatusTypeDef HPS_CF_SetStringParam(int handle, string paramName, int channelIndex, string value)
        {
            StatusTypeDef ret = StatusTypeDef.Status_Succeed;
            IntPtr valuePtr = Marshal.StringToHGlobalAnsi(value);
            byte[] paramNamePtr = System.Text.Encoding.ASCII.GetBytes(paramName);
            ret = CF_SetStringParam(handle, paramNamePtr, channelIndex, valuePtr);

            Marshal.FreeHGlobal(valuePtr);  //释放分配的非托管内存。

            return ret;
        }


        /**********************************************************************************
        * HPS_CF_GetIntParam
        *	获取Int类型参数值
        * INPUT:
        *	handle:		  用户指定的传感器设备handle
        *	paramName：	  参数名， 参数名，包含的参数可以参考CF_ParamterDefine.h文件
        *   channelIndex：通道索引，若是全局参数则该值填0即可，内部不做判断
        *	RETURN:		  返回错误码
        *
        **********************************************************************************/
        public static StatusTypeDef HPS_CF_GetIntParam(int handle, string paramName, int channelIndex, ref int value)
        {
            byte[] paramNamePtr = System.Text.Encoding.ASCII.GetBytes(paramName);
            return CF_GetIntParam(handle, paramNamePtr, channelIndex, ref value);           
        }


        /**********************************************************************************
        * HPS_CF_GetFloatParam
        *	获取Float类型参数值
        * INPUT:
        *	handle:		  用户指定的传感器设备handle
        *	paramName：	  参数名， 参数名，包含的参数可以参考CF_ParamterDefine.h文件
        *   channelIndex：通道索引，若是全局参数则该值填0即可，内部不做判断
        *   RETURN: 返回错误码
        **********************************************************************************/
        public static StatusTypeDef HPS_CF_GetFloatParam(int handle, string paramName, int channelIndex, ref float value)
        {
            byte[] paramNamePtr = System.Text.Encoding.ASCII.GetBytes(paramName);
            return CF_GetFloatParam(handle, paramNamePtr, channelIndex, ref value);
        }


        /**********************************************************************************
        * HPS_CF_GetStringParam
        *	获取String类型参数值
        * INPUT:
        *	handle:		  用户指定的传感器设备handle
        *	paramName：	  参数名， 参数名，包含的参数可以参考CF_ParamterDefine.h文件
        *   channelIndex：通道索引，若是全局参数则该值填0即可，内部不做判断
        *   RETURN:		  返回错误码
        *
        **********************************************************************************/
        public static StatusTypeDef HPS_CF_GetStringParam(int handle, string paramName, int channelIndex, byte[] value)
        {
            byte[] paramNamePtr = System.Text.Encoding.ASCII.GetBytes(paramName);
            return CF_GetStringParam(handle, paramNamePtr, channelIndex, value);
        }


        /**********************************************************************************
        * HPS_CF_BindInputPort
        *	设置外部触发IO的功能
        * INPUT:
        *	handle:						用户指定的传感器设备handle
        *	Confocal_InputPortFunc_t:	用户指定触发功能
        *	Channel：					该IO口关联的通道
        *   inputPort:					输入口
        *   RETURN:						返回错误码
        *	
        **********************************************************************************/
        public static StatusTypeDef HPS_CF_BindInputPort(int handle, int Channel, Confocal_InputPortFunc_t func, int inputPort)
        {
            return CF_BindInputPort(handle, Channel, func, inputPort);
        }


        /**********************************************************************************
        * HPS_CF_UnbindInputPort
        *	将指定输入IO口解绑
        * INPUT:
        *  handle:				用户指定的传感器设备handle
        *  inputPort:			输入口
        *  RETURN:			    返回错误码
        *		
        **********************************************************************************/
        public static StatusTypeDef HPS_CF_UnbindInputPort(int handle, int inputPort)
        {
            return CF_UnbindInputPort(handle, inputPort);
        }



     //标定功能
        /**********************************************************************************
        * setDoubleChannelThicknessK
        *   设置双头测厚校准系数
        * INPUT:
        *   handle:用户指定的传感器设备handle
        *   groupIndex: 组选择
        *   K: 校准系数 长度3
        * RETURN:
        * 返回错误描述信息
*       *********************************************************************************/
        public static StatusTypeDef hps_setDoubleChannelThicknessK(int handle, int groupIndex, double[] k)
        {
            try
            {
                return setDoubleChannelThicknessK(handle, groupIndex, k);
            }
            catch (Exception ex)
            {
                return StatusTypeDef.Status_ErrorSDKVersion;
            }
        }

        /**********************************************************************************
        * getDoubleChannelThicknessK
        *	获取双头测厚系数
        * INPUT:
        *   handle:     用户指定的传感器设备handle
        *   groupIndex: 双头测厚通道索引值
        * OUTOPT:
        *	k:     曲线系数，长度为3
        * RETURN:
        *	返回错误描述信息
        **********************************************************************************/
        public static StatusTypeDef hps_getDoubleChannelThicknessK(int handle, int groupIndex, double[] k)
        {
            try
            {
                return getDoubleChannelThicknessK(handle, groupIndex, k);
            }
            catch (Exception ex)
            {
                return StatusTypeDef.Status_ErrorSDKVersion;
            }
        }


        /**********************************************************************************
        * setDoubleChannelThicknessSamplePoint
        *	设置双头测厚采样点
        * INPUT:
        *   handle:     用户指定的传感器设备handle
        *   groupIndex: 双头测厚通道索引值
        *   std_thickness: 真实的厚度值
        * RETURN:
        *	返回错误描述信息
        **********************************************************************************/
        public static StatusTypeDef hps_setDoubleChannelThicknessSamplePoint(int handle, int groupIndex, float std_thickness)
        {
            try
            {
                return setDoubleChannelThicknessSamplePoint(handle, groupIndex, std_thickness);
            }
            catch (Exception ex)
            {
                return StatusTypeDef.Status_ErrorSDKVersion;
            }
        }

        /**********************************************************************************
        * getDoubleChannelThicknessSamplePoint
        *	获取双头测厚采样点个数
        * INPUT:
        *   handle:     用户指定的传感器设备handle
        *   groupIndex: 双头测厚通道索引值
        * RETURN:
        *	返回错误描述信息
        **********************************************************************************/
        public static int hps_getDoubleChannelThicknessSamplePoint(int handle, int groupIndex)
        {
            try
            {
                return getDoubleChannelThicknessSamplePoint(handle, groupIndex);
            }
            catch (Exception ex)
            {
                return (int)StatusTypeDef.Status_ErrorSDKVersion;
            }
        }

        /**********************************************************************************
        * doDoubleChannelThicknessCal
        *	双头测厚校准计算
        * INPUT:
        *   handle:     用户指定的传感器设备handle
        *   groupIndex: 双头测厚通道索引值
        *	k:     曲线系数，长度为3
        * RETURN:
        *	返回错误描述信息
        **********************************************************************************/
        public static StatusTypeDef hps_doDoubleChannelThicknessCal(int handle, int groupIndex, double[] k)
        {
            try
            {
                return doDoubleChannelThicknessCal(handle, groupIndex, k);
            }
            catch (Exception ex)
            {
                return StatusTypeDef.Status_ErrorSDKVersion;
            }
        }

        /**********************************************************************************
        * clearDoubleChannelThicknessSamplePoint
        *	清除双头测厚校准（复位）
        * INPUT:
        *   handle:用户指定的传感器设备handle
        *   groupIndex: 双头测厚通道索引值
        * RETURN:
        *	返回错误描述信息
        **********************************************************************************/
        public static StatusTypeDef hps_clearDoubleChannelThicknessSamplePoint(int handle, int groupIndex)
        {
            try
            {
                return clearDoubleChannelThicknessSamplePoint(handle, groupIndex);
            }
            catch (Exception ex)
            {
                return StatusTypeDef.Status_ErrorSDKVersion;
            }
        }

        

        


        /**********************************************************************************
        * setThicknessRefractivePara
        *	设置折射率标定参数
        * INPUT:
        *	handle:用户指定的传感器设备handle
        *	channel：通道
        *	signalIndex1：信号1索引
        *	signalIndex2：信号2索引
        *	Nc/Nd/Nf ： 3个折射率参数
        * RETURN:
        *	返回错误描述信息
        **********************************************************************************/
        public static StatusTypeDef hps_setThicknessRefractivePara(int handle, int channel, int signalIndex1, int signalIndex2, float Nc, float Nd, float Nf)
        {
            try
            {
                return setThicknessRefractivePara(handle, channel, signalIndex1, signalIndex2, Nc, Nd, Nf);
            }
            catch (Exception ex)
            {
                return StatusTypeDef.Status_ErrorSDKVersion;
            }
        }


        /**********************************************************************************
        * getThicknessRefractivePara
        *	获取折射率标定参数
        * INPUT:
        *	handle:用户指定的传感器设备handle
        *	channel：通道
        *	signalIndex1：信号1索引
        *	signalIndex2：信号2索引Mac
        *	Nc/Nd/Nf ： 返回3个折射率参数
        * RETURN:
        *	返回错误描述信息
        **********************************************************************************/
        public static StatusTypeDef hps_getThicknessRefractivePara(int handle, int channel, int signalIndex1, int signalIndex2, out float Nc, out float Nd, out float Nf)
        {
            Nc = 0;
            Nd = 0;
            Nf = 0;
            try
            {
                return getThicknessRefractivePara(handle, channel, signalIndex1, signalIndex2, ref Nc, ref Nd, ref Nf);
            }
            catch (Exception ex)
            {
                return StatusTypeDef.Status_ErrorSDKVersion;
            }
        }








    }
}

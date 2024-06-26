using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThicknessMeasurement
{
    //全局变量
    internal class DataClass
    {
        public static string User = "";//当前登录的ID
        public static string Name = "";//当前登录的用户名
        public static int Power = 0;//当前登录的用户权限
        public static int value_D7702 = 0;//设备待料状态


        public static int[] data_D7700 = new int[10];//设备状态信息
        public static int[] data_D7700_jl = new int[10];//记录设备状态信息

        public static int pfid = 0;//测量参数的配方编号
        public static int pfnum = 0;//配方数量

        #region 弃用
        public static string[] errorname = new string[1000];//异常内容
        public static string[] Alarm()
        {
            string[] alarm = new string[1000];
            alarm[0] = "F0";
            alarm[1] = "紧急报警F1";
            alarm[2] = "气压不足F2";
            alarm[3] = "F3";
            alarm[4] = "F4";
            alarm[5] = "F5";
            alarm[6] = "F6";
            alarm[7] = "F7";
            alarm[8] = "F8";
            alarm[9] = "F9";
            alarm[10] = "气缸未归位平台禁止移动F10";
            alarm[11] = "光栅感应禁止台面移动F11";
            alarm[12] = "后安全门打开F12";
            alarm[13] = "左侧安全门打开F13";
            alarm[14] = "右安全门打开F14";
            alarm[15] = "熔头位置更新失败F15";
            alarm[16] = "F16";
            alarm[17] = "F17";
            alarm[18] = "F18";
            alarm[19] = "F19";
            alarm[20] = "初始化超时F20";
            alarm[21] = "熔头对位超时F21";
            alarm[22] = "一次加工超时F22";
            alarm[23] = "熔头张开超时F23";
            alarm[24] = "台面切换超时F24";
            alarm[25] = "熔头压合超时F25";
            alarm[26] = "脱PIN超时F26";
            alarm[27] = "F27";
            alarm[28] = "F28";
            alarm[29] = "F29";
            alarm[30] = "F30";
            alarm[31] = "F31";
            alarm[32] = "F32";
            alarm[33] = "F33";
            alarm[34] = "F34";
            alarm[35] = "F35";
            alarm[36] = "F36";
            alarm[37] = "F37";
            alarm[38] = "F38";
            alarm[39] = "F39";
            alarm[40] = "加热超时F40";
            alarm[41] = "检测到高频子站数量不正确F41";
            alarm[42] = "高频指令响应超时F42";
            alarm[43] = "F43";
            alarm[44] = "F44";
            alarm[45] = "F45";
            alarm[46] = "F46";
            alarm[47] = "F47";
            alarm[48] = "F48";
            alarm[49] = "F49";
            alarm[50] = "F50";
            alarm[51] = "F51";
            alarm[52] = "F52";
            alarm[53] = "F53";
            alarm[54] = "F54";
            alarm[55] = "F55";
            alarm[56] = "F56";
            alarm[57] = "F57";
            alarm[58] = "F58";
            alarm[59] = "F59";
            alarm[60] = "F60";
            alarm[61] = "F61";
            alarm[62] = "F62";
            alarm[63] = "F63";
            alarm[64] = "F64";
            alarm[65] = "F65";
            alarm[66] = "F66";
            alarm[67] = "F67";
            alarm[68] = "F68";
            alarm[69] = "F69";
            alarm[70] = "F70";
            alarm[71] = "F71";
            alarm[72] = "F72";
            alarm[73] = "F73";
            alarm[74] = "F74";
            alarm[75] = "F75";
            alarm[76] = "F76";
            alarm[77] = "F77";
            alarm[78] = "F78";
            alarm[79] = "F79";
            alarm[80] = "F80";
            alarm[81] = "F81";
            alarm[82] = "F82";
            alarm[83] = "F83";
            alarm[84] = "F84";
            alarm[85] = "F85";
            alarm[86] = "F86";
            alarm[87] = "F87";
            alarm[88] = "F88";
            alarm[89] = "F89";
            alarm[90] = "F90";
            alarm[91] = "F91";
            alarm[92] = "F92";
            alarm[93] = "F93";
            alarm[94] = "F94";
            alarm[95] = "F95";
            alarm[96] = "F96";
            alarm[97] = "F97";
            alarm[98] = "F98";
            alarm[99] = "F99";
            alarm[100] = "A01上模组伸出超时F100";
            alarm[101] = "A01上模组缩回超时F101";
            alarm[102] = "A01上模组感应异常F102";
            alarm[103] = "SP";
            alarm[104] = "SP";
            alarm[105] = "A02上熔头伸出超时F105";
            alarm[106] = "A02上熔头缩回超时F106";
            alarm[107] = "A02上熔头感应异常F107";
            alarm[108] = "SP";
            alarm[109] = "SP";
            alarm[110] = "A03压板上段伸出超时F110";
            alarm[111] = "A03压板上段缩回超时F111";
            alarm[112] = "A03压板上段感应异常F112";
            alarm[113] = "SP";
            alarm[114] = "SP";
            alarm[115] = "A04压板下段伸出超时F115";
            alarm[116] = "A04压板下段缩回超时F116";
            alarm[117] = "A04压板下段感应异常F117";
            alarm[118] = "SP";
            alarm[119] = "SP";
            alarm[120] = "A05下熔头伸出超时F120";
            alarm[121] = "A05下熔头缩回超时F121";
            alarm[122] = "A05下熔头感应异常F122";
            alarm[123] = "SP";
            alarm[124] = "SP";
            alarm[125] = "A06下模组伸出超时F125";
            alarm[126] = "A06下模组缩回超时F126";
            alarm[127] = "A06下模组感应异常F127";
            alarm[128] = "SP";
            alarm[129] = "SP";
            alarm[130] = "A07上板脱PIN伸出超时F130";
            alarm[131] = "A07上板脱PIN缩回超时F131";
            alarm[132] = "A07上板脱PIN感应异常F132";
            alarm[133] = "SP";
            alarm[134] = "SP";
            alarm[135] = "A08下板脱PIN伸出超时F135";
            alarm[136] = "A08下板脱PIN缩回超时F136";
            alarm[137] = "A08下板脱PIN感应异常F137";
            alarm[138] = "SP";
            alarm[139] = "SP";
            alarm[140] = "A09吹气ON超时F140";
            alarm[141] = "A09吹气OFF超时F141";
            alarm[142] = "A09吹气感应异常F142";
            alarm[143] = "SP";
            alarm[144] = "SP";
            alarm[145] = "F145";
            alarm[146] = "F146";
            alarm[147] = "F147";
            alarm[148] = "F148";
            alarm[149] = "F149";
            alarm[150] = "平台移栽伺服_正限位F150";
            alarm[151] = "平台移栽伺服_负限位F151";
            alarm[152] = "平台移栽伺服_已STOP中F152";
            alarm[153] = "平台移栽伺服_未回原F153";
            alarm[154] = "平台移栽伺服_报警F154";
            alarm[155] = "平台移栽伺服_警告F155";
            alarm[156] = "平台移栽伺服_定位超时F156";
            alarm[157] = "平台移栽伺服_定位完成感应器未感应F157";
            alarm[158] = "平台移栽伺服_驱动器报警F158";
            alarm[159] = "F159";
            alarm[160] = "L1伺服_正限位F160";
            alarm[161] = "L1伺服_负限位F161";
            alarm[162] = "L1伺服_停止中F162";
            alarm[163] = "L1伺服_未回原F163";
            alarm[164] = "L1伺服_报警F164";
            alarm[165] = "L1伺服_警告F165";
            alarm[166] = "L1伺服_定位超时F166";
            alarm[167] = "F167";
            alarm[168] = "L1_驱动器报警F168";
            alarm[169] = "F169";
            alarm[170] = "L2伺服_正限位F170";
            alarm[171] = "L2伺服_负限位F171";
            alarm[172] = "L2伺服_停止中F172";
            alarm[173] = "L2伺服_未回原F173";
            alarm[174] = "L2伺服_报警F174";
            alarm[175] = "L2伺服_警告F175";
            alarm[176] = "L2伺服_定位超时F176";
            alarm[177] = "F177";
            alarm[178] = "L2_驱动器报警F178";
            alarm[179] = "F179";
            alarm[180] = "SP伺服_正限位F180";
            alarm[181] = "SP伺服_负限位F181";
            alarm[182] = "SP伺服_停止中F182";
            alarm[183] = "SP伺服_未回原F183";
            alarm[184] = "SP伺服_报警F184";
            alarm[185] = "SP伺服_警告F185";
            alarm[186] = "SP伺服_定位超时F186";
            alarm[187] = "F187";
            alarm[188] = "SP_驱动器报警F188";
            alarm[189] = "F189";
            alarm[190] = "W1伺服_正限位F190";
            alarm[191] = "W1伺服_负限位F191";
            alarm[192] = "W1伺服_停止中F192";
            alarm[193] = "W1伺服_未回原F193";
            alarm[194] = "W1伺服_报警F194";
            alarm[195] = "W1伺服_警告F195";
            alarm[196] = "W1伺服_定位超时F196";
            alarm[197] = "F197";
            alarm[198] = "W1_驱动器报警F198";
            alarm[199] = "F199";
            alarm[200] = "W2伺服_正限位F198";
            alarm[201] = "W2伺服_负限位F199";
            alarm[202] = "W2伺服_停止中F202";
            alarm[203] = "W2伺服_未回原F203";
            alarm[204] = "W2伺服_报警F204";
            alarm[205] = "W2伺服_警告F205";
            alarm[206] = "W2伺服_定位超时F206";
            alarm[207] = "F207";
            alarm[208] = "W2_驱动器报警F208";
            alarm[209] = "F209";
            alarm[210] = "上部L1长边模组下降位未感应F210";
            alarm[211] = "上部L2长边模组下降位未感应F211";
            alarm[212] = "上部W1宽边模组下降位未感应F212";
            alarm[213] = "上部W2宽边模组下降位未感应F213";
            alarm[214] = "上部L1长边模组下降位未缩回F214";
            alarm[215] = "上部L2长边模组下降位未缩回F215";
            alarm[216] = "上部W1宽边模组下降位未缩回F216";
            alarm[217] = "上部W2宽边模组下降位未缩回F217";
            alarm[218] = "上部L1长边熔头1/2未伸出F218";
            alarm[219] = "上部L1长边熔头3/4未伸出F219";
            alarm[220] = "上部L1长边熔头5/6未伸出F220";
            alarm[221] = "上部L1长边熔头7/8未伸出F221";
            alarm[222] = "上部W1宽边熔头1/2未伸出F222";
            alarm[223] = "上部W2宽边熔头1/2未伸出F223";
            alarm[224] = "上部L1长边熔头1/2未缩回F224";
            alarm[225] = "上部L1长边熔头3/4未缩回F225";
            alarm[226] = "上部L1长边熔头5/6未缩回F226";
            alarm[227] = "上部L1长边熔头7/8未缩回F227";
            alarm[228] = "上部W1宽边熔头1/2未缩回F228";
            alarm[229] = "上部W2宽边熔头1/2未缩回F229";
            alarm[230] = "下部L1长边模组上升位未感应F230";
            alarm[231] = "下部L2长边模组上升位未感应F231";
            alarm[232] = "下部W1宽边模组上升位未感应F232";
            alarm[233] = "下部W2宽边模组上升位未感应F233";
            alarm[234] = "下部L1长边模组下降位未感应F234";
            alarm[235] = "下部L2长边模组下降位未感应F235";
            alarm[236] = "下部W1宽边模组下降位未感应F236";
            alarm[237] = "下部W2宽边模组下降位未感应F237";
            alarm[238] = "下部L1长边熔头未伸出F238";
            alarm[239] = "下部L2长边熔头未伸出F239";
            alarm[240] = "下部W1宽边熔头未伸出F240";
            alarm[241] = "下部W2宽边熔头未伸出F241";
            alarm[242] = "下部L1长边熔头未缩回F242";
            alarm[243] = "下部L2长边熔头未缩回F243";
            alarm[244] = "下部W1宽边熔头未缩回F244";
            alarm[245] = "下部W2宽边熔头未缩回F245";
            alarm[246] = "F246";
            alarm[247] = "F247";
            alarm[248] = "F248";
            alarm[249] = "F249";
            alarm[250] = "F250";
            alarm[251] = "F251";
            alarm[252] = "F252";
            alarm[253] = "F253";
            alarm[254] = "F254";
            alarm[255] = "F255";
            alarm[256] = "F256";
            alarm[257] = "F257";
            alarm[258] = "F258";
            alarm[259] = "F259";
            alarm[260] = "F260";
            alarm[261] = "F261";
            alarm[262] = "F262";
            alarm[263] = "F263";
            alarm[264] = "F264";
            alarm[265] = "F265";
            alarm[266] = "F266";
            alarm[267] = "F267";
            alarm[268] = "F268";
            alarm[269] = "F269";
            alarm[270] = "F270";
            alarm[271] = "F271";
            alarm[272] = "F272";
            alarm[273] = "F273";
            alarm[274] = "F274";
            alarm[275] = "F275";
            alarm[276] = "F276";
            alarm[277] = "F277";
            alarm[278] = "F278";
            alarm[279] = "F279";
            alarm[280] = "F280";
            alarm[281] = "F281";
            alarm[282] = "F282";
            alarm[283] = "F283";
            alarm[284] = "F284";
            alarm[285] = "F285";
            alarm[286] = "F286";
            alarm[287] = "F287";
            alarm[288] = "F288";
            alarm[289] = "F289";
            alarm[290] = "F290";
            alarm[291] = "F291";
            alarm[292] = "F292";
            alarm[293] = "F293";
            alarm[294] = "F294";
            alarm[295] = "F295";
            alarm[296] = "F296";
            alarm[297] = "F297";
            alarm[298] = "F298";
            alarm[299] = "F299";
            alarm[300] = "请设置熔头数量F300";
            alarm[301] = "请设置启用温控段F301";
            alarm[302] = "请设置扫码判断模式F302";
            alarm[303] = "请选用一个MES功能F303";
            alarm[304] = "F304";
            alarm[305] = "计划数量完成F305";
            alarm[306] = "F306";
            alarm[307] = "F307";
            alarm[308] = "F308";
            alarm[309] = "F309";
            alarm[310] = "F310";
            alarm[311] = "F311";
            alarm[312] = "F312";
            alarm[313] = "F313";
            alarm[314] = "F314";
            alarm[315] = "F315";
            alarm[316] = "F316";
            alarm[317] = "F317";
            alarm[318] = "F318";
            alarm[319] = "F319";
            alarm[320] = "熔头1寿命接近F320";
            alarm[321] = "熔头2寿命接近F321";
            alarm[322] = "熔头3寿命接近F322";
            alarm[323] = "熔头4寿命接近F323";
            alarm[324] = "熔头5寿命接近F324";
            alarm[325] = "熔头6寿命接近F325";
            alarm[326] = "熔头7寿命接近F326";
            alarm[327] = "熔头8寿命接近F327";
            alarm[328] = "熔头9寿命接近F328";
            alarm[329] = "熔头10寿命接近F329";
            alarm[330] = "熔头11寿命接近F330";
            alarm[331] = "熔头12寿命接近F331";
            alarm[332] = "熔头13寿命接近F332";
            alarm[333] = "熔头14寿命接近F333";
            alarm[334] = "熔头15寿命接近F334";
            alarm[335] = "熔头16寿命接近F335";
            alarm[336] = "熔头17寿命接近F336";
            alarm[337] = "熔头18寿命接近F337";
            alarm[338] = "熔头19寿命接近F338";
            alarm[339] = "熔头20寿命接近F339";
            alarm[340] = "熔头1寿命用尽F340";
            alarm[341] = "熔头2寿命用尽F341";
            alarm[342] = "熔头3寿命用尽F342";
            alarm[343] = "熔头4寿命用尽F343";
            alarm[344] = "熔头5寿命用尽F344";
            alarm[345] = "熔头6寿命用尽F345";
            alarm[346] = "熔头7寿命用尽F346";
            alarm[347] = "熔头8寿命用尽F347";
            alarm[348] = "熔头9寿命用尽F348";
            alarm[349] = "熔头10寿命用尽F349";
            alarm[350] = "熔头11寿命用尽F350";
            alarm[351] = "熔头12寿命用尽F351";
            alarm[352] = "熔头13寿命用尽F352";
            alarm[353] = "熔头14寿命用尽F353";
            alarm[354] = "熔头15寿命用尽F354";
            alarm[355] = "熔头16寿命用尽F355";
            alarm[356] = "熔头17寿命用尽F356";
            alarm[357] = "熔头18寿命用尽F357";
            alarm[358] = "熔头19寿命用尽F358";
            alarm[359] = "熔头20寿命用尽F359";
            alarm[360] = "高频报警发生F360";
            alarm[361] = "指令响应超时F361";
            alarm[362] = "指令重试失败F362";
            alarm[363] = "发送数据与返回数据异常F363";
            alarm[364] = "高频反馈指令错误F364";
            alarm[365] = "F365";
            alarm[366] = "F366";
            alarm[367] = "F367";
            alarm[368] = "F368";
            alarm[369] = "F369";
            alarm[370] = "F370";
            alarm[371] = "F371";
            alarm[372] = "F372";
            alarm[373] = "F373";
            alarm[374] = "F374";
            alarm[375] = "F375";
            alarm[376] = "F376";
            alarm[377] = "F377";
            alarm[378] = "F378";
            alarm[379] = "F379";
            alarm[380] = "高频警告发生F380";
            alarm[381] = "检索ID失败F381";
            alarm[382] = "加热启动失败F382";
            alarm[383] = "参数读取失败F383";
            alarm[384] = "配方校验失败F384";
            alarm[385] = "配方发送失败F385";
            alarm[386] = "时间设置失败F386";
            alarm[387] = "LED显示失败F387";
            alarm[388] = "站号设置失败F388";
            alarm[389] = "F389";
            alarm[390] = "F390";
            alarm[391] = "F391";
            alarm[392] = "F392";
            alarm[393] = "F393";
            alarm[394] = "F394";
            alarm[395] = "F395";
            alarm[396] = "F396";
            alarm[397] = "F397";
            alarm[398] = "F398";
            alarm[399] = "F399";
            alarm[400] = "当前芯板不正确F400";
            alarm[401] = "未叠PP F401";
            alarm[402] = "非码库二维码F402";
            alarm[403] = "叠构完成请勿多放板F403";
            alarm[404] = "F404";
            alarm[405] = "叠构未完成禁止进板加热F405";

            return alarm;
        }
        public static string AlarmLevel(int alarmID)
        {
            if (alarmID < 300)
            {
                return "警报";
            }
            else if (alarmID < 500)
            {
                return "警告";
            }
            return "未知";
        }
        #endregion

        public static List<string> peizhivalues = new List<string>();//配置参数
        public static List<string> peifangvalues = new List<string>();//配方参数
        public static List<钢板参数> gangbans = new List<钢板参数>();


        //防呆相关变量
        public static int Alarmcode = 0;
        public static int plcdbbtn = 0;//叠板按钮
        public static bool finish = false;//是否叠板完成
        public static bool finish1 = false;//是否叠板完成(用于给PLC写一次信号)
        public static bool tuopinok = false;//托pin完成信号
        public static bool startdb2 = false;//开始叠板判断台面

        //读取配置信息
        public static void readpeizhi(IFreeSql sql)
        {
            peizhivalues = sql.Select<配置>().ToList(t => t.值);
           
        }


        //读取配置信息 
        public static void readpeifang(IFreeSql sql)
        {
            peifangvalues = sql.Select<测量参数>().ToList(s => s.值);

            gangbans = sql.Select<钢板参数>().ToList();

            pfnum = gangbans.Count();

        }

    }
}

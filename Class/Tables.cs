using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;

namespace ThicknessMeasurement
{
    public class DB
    {
        //static Lazy<IFreeSql> sqliteLazy = new Lazy<IFreeSql>(() => new FreeSql.FreeSqlBuilder()
        //      .UseMonitorCommand(cmd => Trace.WriteLine($"Sql：{cmd.CommandText}"))//监听SQL语句,Trace在输出选项卡中查看
        //      .UseConnectionString(FreeSql.DataType.MySql, $"server=127.0.0.1;Uid=root;password=root;Database=fusemachine;Charset=utf8")
        //      .UseAutoSyncStructure(true) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
        //      .Build());
        //public static IFreeSql MySQL => sqliteLazy.Value;

       
        public static IFreeSql MySQL = new FreeSql.FreeSqlBuilder()
            .UseConnectionString(FreeSql.DataType.MySql, $"server=127.0.0.1;Uid=root;password=root;Database=thicknessmeasurement;Charset=utf8")
            .UseAutoSyncStructure(true) //自动同步实体结构到数据库
            .Build();
    }

   
    public class productionlog
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int ID { get; set; }
        public string datee { get; set; }
        public string MaterialNum { get; set; }
        public string LotNum { get; set; }
        public string 钢板ID { get; set; }
        public string settingThickness { get; set; }
        public string settingMax { get; set; }
        public string settingMin { get; set; }
        public string settingMax_min { get; set; }
        public string result { get; set; }
        public string max { get; set; }
        public string min { get; set; }
        public string max_min { get; set; }
        public string avg { get; set; }
        public int totalcount { get; set; }
        public string d11 { get; set; }
        public string d12 { get; set; }
        public string d13 { get; set; }
        public string d14 { get; set; }
        public string d21 { get; set; }
        public string d22 { get; set; }
        public string d23 { get; set; }
        public string d24 { get; set; }
        public string d31 { get; set; }
        public string d32 { get; set; }
        public string d33 { get; set; }
        public string d34 { get; set; }
    }

    public class 钢板记录
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int ID { get; set; }
        public string 钢板ID { get; set; }
        public int 设定次数 { get; set; }
        public int 生产次数 { get; set; }
        public int 剩余次数 { get; set; }

    }


    public class 用户管理
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int ID { get; set; }
        public string 用户名 { get; set; }
        public string 姓名 { get; set; }
        public string 密码 { get; set; }
        public string 权限 { get; set; }
    }

    public class 当前用户
    {
        [Column(IsPrimary = true)]
        public int ID { get; set; }
        public string 机台编号 { get; set; }
        public string 用户名 { get; set; }
        public string 姓名 { get; set; }
        public int 权限 { get; set; }
    }

    public class 配置
    {
        [Column(IsIdentity = false, IsPrimary = true)]
        public int id { get; set; }
        public string 项目 { get; set; }
        public string 值 { get; set; }
        public string 备注 { get; set; }
        public string 默认值 { get; set; }

        //public string 显示等级 { get; set; }
        //public string 修改权限 { get; set; }
        //public string 分类 { get; set; }
    }

    public class 异常记录
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long ID { get; set; }
        public string 设备ID { get; set; }
        public string 工单号 { get; set; }
        public string 人员ID { get; set; }
        public string 异常ID { get; set; }
        public string 异常等级 { get; set; }
        public string 异常内容 { get; set; }
        public string 发生时间 { get; set; }
        public string 结束时间 { get; set; }
        public int 异常时长 { get; set; }
    }

    public class 报警清单
    {
        [Column(IsIdentity = false, IsPrimary = true)]
        public int ID { get; set; }
        public string 设备ID { get; set; }
        public int 报警ID { get; set; }
        public string 内容 { get; set; }
        public string 备注 { get; set; }
        public string 英文 { get; set; }
    }


    public class 测量参数
    {
        [Column(IsIdentity = false, IsPrimary = true)]
        public int id { get; set; }
        public string 项目 { get; set; }
        public string 值 { get; set; }
        public string 备注 { get; set; }

    }

    public class 钢板参数
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int id { get; set; }
        public string 钢板ID { get; set; }
        public string 钢板名称 { get; set; }
        public double 产品厚度 { get; set; }
        public int 自动识别范围 { get; set; }

    }

}

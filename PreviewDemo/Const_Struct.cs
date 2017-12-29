using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Flash_Packing_Client
{
    class Const_Struct
    {
         public const int MSG_PLATE_INFO = 0x2000;//当识别到车牌时，接受摄像机发回来的消息
         public const int MSG_WU_TINGCHE_JILU = 0x2001;//当找不到某车牌的停车记录时发送该消息
         public const int MSG_JIFEI_CELUE_XIUGAI = 0x1001;//刷新JIFEICELUE_FRM中的datagridview1数据
         public const int MSG_BAOCUN_TUPIAN_SHIBAI = 0x2002;//当保存图片失败是发送该消息
         public const int MSG_MAIFEICHE_LURU_BIANJI = 0x2003;//免费车牌录入/编辑结束后，刷新查询界面
         public const int MSG_YUEKACHE_LURU_BIANJI = 0x2004;//免费车牌录入/编辑结束后，刷新查询界面
         public const int MSG_CAM_ZHUAPAI = 0x2005; //执行抓拍时，发送该消息
         public const int MSG_CAM_ZHUAPAI_FALSE = 0x2006; //抓拍完成时，发送该消息
         public const int MSG_APP_CLOSE = 0x2007;//退出系统时发送该消息
         public const int MSG_APP_DENGLU = 0x2008;//登陆系统后发送该消息
         public const int MSG_YONGHU_LIEBIAO = 0x2009;//刷新用户列表
         public const int MSG_SHOUFEI_CELUE = 0x2010;//刷新策略列表
         public const int MSG_CHONGXIN_DENGLU = 0x2011;//重新登陆
         public const int MSG_CHARU_CHEPAI = 0x2012;//手动插入车牌号码
         public const int MSG_SHOUDONG_CHUCHANG = 0x2013;//手动插入车牌后，请求再次出场操作调用
         public const int MSG_CHUCHANG_TAIZHA = 0x2014; //出场扣费时调用
         public const int MSG_XIANSHANG_FANHUI = 0x2015; //线上返回信息

         public const int MSG_DISABLE_MENU_CHECHANG_JIANGKONG = 0x2003;//disable车场监控菜单

         public const int XINTIAO_JIANGE = 240000;//每隔4分钟向数据库发送一次在线心跳
         public const int PAIZHAO_JIANGE = 3;//同一摄像机两次有效拍照间隔15秒


        public const int JIEKOU_RUCHANG = 1;
        public const int JIEKOU_CHAXUN_CHEFEI = 2;
        public const int JIEKOU_KOUFEI = 3;
        public const int JIEKOU_LICHANG = 4;
        public const int JIEKOU_GENGZHENG_CHEPAI = 5;

        public const int JIEKOU_YUEKA_JIAOFEI = 6;
        public const int JIEKOU_YUEKA_TUIFEI = 7;
        public const int JIEKOU_MIANFEI_TIANJIA = 8;
        public const int JIEKOU_MIANFEI_SHANCHU = 9;
        public const int JIEKOU_MIANFEI_BIANJI = 10;
        public const int JIEKOU_CHAXUN_CHEPAI_XINGZHI = 11;
        public const int JIEKOU_CHEPAI_LEIBIE_DAORU = 12;
        public const int JIEKOU_YUEKA_BIANJI = 13;
        public const int JIEKOU_CHEPAI_GENGZHENG = 14;

        public static  string WAI_GUA;//是否是外挂系统
         public static string TINGCHECHANG_ID=""; //停车场ID
        public struct koufei_info
        {
            public string chepai;
            public string ruchangshijian;
            public string ru_kou;
            public string shoufei_biaoji;
            public string chepaixingzhi;
            public string chepai_yanse;
            public string cheliang_leixing;
            public string chuchang_shijian;
            public string damen_chuchang;
            public string tingchefei;
            public string tingche_shichang ;
            public string menwei;
            public int cam_num;
        }

        public struct USER_INFO
        {
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
            public string xingming;
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
            public string zhanghao;
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
            public string mima;
        }

        public struct xianshang_fanhui_struct
        {
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 1024)]
             public string xianshang_fanhui_str;
        }

        public struct get_sj_xz
        {
            public string ruchangshijian;
            public string chepaixingzhi;
        }

        public struct sj_xz_cx_rk_ys
        {
            public string ruchangshijian;
            public string ru_kou;
            public string chepaixingzhi;
            public string qiche_leixing;
            public string chepai_yanse;
        }

        public struct ccsj_tcfy
        {
            public string chuchang_shijian;
            public string tingchefei;
        }

        public struct bitmap_chepai
        {
            public Bitmap tupian;
            public string chepai_haoma;
            public string cunru_shijian;
        }

        public static void set_waigua(string wg)
        {
            WAI_GUA = wg;         
        }
        public static void set_tingchechang_id(string id)
        {
            TINGCHECHANG_ID = id;
        }
    }
}

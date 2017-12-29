using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace Flash_Packing_Client
{

    class ZS_camera
    {

        ZHENSHI_SDK.VZLPRC_PLATE_INFO_CALLBACK ZS_PlateResultCB ;
        ZHENSHI_SDK.VZLPRC_FIND_DEVICE_CALLBACK find_DeviceCB;       
                                        //车牌类型
        private const int LT_UNKNOWN = 0;  //未知车牌
        private const int LT_BLUE = 1;  //蓝牌小汽车
        private const int LT_BLACK = 2;  //黑牌小汽车
        private const int LT_YELLOW = 3;  //单排黄牌
        private const int LT_YELLOW2 = 4;  //双排黄牌（大车尾牌，农用车）
        private const int LT_POLICE = 5;  //警车车牌
        private const int LT_ARMPOL = 6;  //武警车牌
        private const int LT_INDIVI = 7;  //个性化车牌
        private const int LT_ARMY = 8; //单排军车牌
        private const int LT_ARMY2 = 9;  //双排军车牌
        private const int LT_EMBASSY = 10; //使馆车牌
        private const int LT_HONGKONG = 11; //香港进出中国大陆车牌
        private const int LT_TRACTOR = 12; //农用车牌
        private const int LT_COACH = 13; //教练车牌
        private const int LT_MACAO = 14; //澳门进出中国大陆车牌
        private const int LT_ARMPOL2 = 15; //双层武警车牌
        private const int LT_ARMPOL_ZONGDUI = 16; // 武警总队车牌
        private const int LT_ARMPOL2_ZONGDUI = 17;// 双层武警总队车牌
        private const int LI_AVIATION = 18; //民航
        private const int LI_ENERGY = 19; //新能源

        //ZS车牌颜色
        private const int LC_UNKNOWN = 0; //未知
        private const int LC_BLUE = 1;  //蓝色
        private const int LC_YELLOW = 2; //黄色 
        private const int LC_WHITE = 3; //白色
        private const int LC_BLACK = 4; //黑色
        private const int LC_GREEN = 5; //绿色


        public string rev_chepai = "";
        public string rev_qiche_leixing = "";
        public string rev_image_path_name = "";
        // public int rev_chepai_yanse;

        private bool cam_biechu_zaixian_biaoji = false;
        private System.Timers.Timer xintiao_timer = null;
        private bool xintiao_timer_qidong_bool = false;

        private const int TAIZHA_ZHENSHI = 0; //电路板上out1;控制抬闸
        private const int LUOZHA_ZHENSHI = 1;  //电路板上out2;控制抬闸

        private DateTime shangci_paizhao_shijian;

        private int m_nPlayHandle = 0;
        private ComboBox comboxIP;
        private static int lprHandle=0;//nCamId = -1;

        private Label rec_chepai_xianshi_label = null;
        private IntPtr parent_Frm_handle;
        private string camIP;
        private TabPage tabPageVideo=null;

        private static string strImageDir = "";
       
        public bool zhuapai_bool =false;//无牌车入场/出场时需要抓拍，将图片保存
        public string zhuapai_chepai = "";//传入一个手动生成的车牌号码

        private static Byte cam_num;//该相机在窗体上是第 cam_num 个

        public void cam_init(string camIP_,
                              ref Label rec_chepai_xianshi_label_,
                              ref TabPage tabPageVideo_,
                              Byte cam_num_,
                              IntPtr parent_Frm_handle_)
              {
                    camIP = camIP_;
                    rec_chepai_xianshi_label = rec_chepai_xianshi_label_;
                    cam_num = cam_num_;
                    parent_Frm_handle = parent_Frm_handle_;
                    tabPageVideo = tabPageVideo_;

                   // shangci_paizhao_shijian = new DateTime();
                    shangci_paizhao_shijian = DateTime.Now;
                    imagepath_init();
                    cam_biechu_zaixian_biaoji = xiangji_shifou_zaixian();                  
        }

        public void find_cam(ComboBox comboBoxIP)
        {
            comboxIP = comboBoxIP;
            Cursor.Current = Cursors.WaitCursor;
            comboBoxIP.Items.Clear();
            comboBoxIP.Sorted = true;
            find_DeviceCB = new ZHENSHI_SDK.VZLPRC_FIND_DEVICE_CALLBACK(FIND_DEVICE_CALLBACK);
            ZHENSHI_SDK.VZLPRClient_StartFindDevice(find_DeviceCB, IntPtr.Zero);
            comboBoxIP.DroppedDown = true;
            Cursor.Current = Cursors.Default;
        }

        private void FIND_DEVICE_CALLBACK(string pStrDevName, string pStrIPAddr, ushort usPort1, ushort usPort2, uint SL, uint SH, IntPtr pUserData)
        {
            comboxIP.Items.Add(pStrIPAddr.ToString());
        }

        public bool camera_connect()
        {        
             lprHandle = ZHENSHI_SDK.VzLPRClient_Open(camIP, 80, "admin", "admin");      
            if (lprHandle == 0)
            {
                MessageBox.Show("打开设备失败ZS_"+cam_num.ToString());
                return false ;
            }

            if (m_nPlayHandle > 0)
            {
                ZHENSHI_SDK.VzLPRClient_StopRealPlay(m_nPlayHandle);
                m_nPlayHandle = 0;
            }

           IntPtr hWnd;
           hWnd = tabPageVideo.Handle;

            if((int)hWnd > 0)
            {
                ZHENSHI_SDK.VzLPRClient_SetPlateInfoCallBack((int)hWnd, null, IntPtr.Zero, 0);
            }
                m_nPlayHandle = ZHENSHI_SDK.VzLPRClient_StartRealPlay(lprHandle, hWnd);
        
            if (cam_biechu_zaixian_biaoji == false)
             {
                ZS_PlateResultCB = new ZHENSHI_SDK.VZLPRC_PLATE_INFO_CALLBACK(ZS_OnPlateResult);
                ZHENSHI_SDK.VzLPRClient_SetPlateInfoCallBack(lprHandle, ZS_PlateResultCB, IntPtr.Zero, 1);

                DB_Class db = new DB_Class();
                db.update_xiangji_zaixiang_biaoji(camIP, true);

                //到此，相机连接成功，需要每过4分钟给数据库发送一次心跳，表示在线
                xintiao_timer = new System.Timers.Timer(Const_Struct.XINTIAO_JIANGE);   //实例化Timer类，设置间隔时间为10000毫秒；   
                xintiao_timer.Elapsed += new System.Timers.ElapsedEventHandler(fasong_xintiao); //到达时间的时候执行事件；   
                xintiao_timer.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
                xintiao_timer.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件； 
                xintiao_timer_qidong_bool = true;  
             }
            else
                MessageBox.Show("该相机已经在别处被连接，本地只能浏览视频。", "提示");
            return true;               
        }
        public void camera_disconnect()
        {
            Cursor.Current = Cursors.WaitCursor;
            if (m_nPlayHandle > 0)
            {
                int ret = ZHENSHI_SDK.VzLPRClient_StopRealPlay(m_nPlayHandle);
                m_nPlayHandle = 0;
            }
           // int lprHandle =(int)tabPageVideo.Handle;
            if (lprHandle > 0)
            {
                ZHENSHI_SDK.VzLPRClient_SetPlateInfoCallBack((int)tabPageVideo.Handle, null, IntPtr.Zero, 0);
                ZHENSHI_SDK.VzLPRClient_SetPlateInfoCallBack(lprHandle, null, IntPtr.Zero, 0);
            }
            

            ZHENSHI_SDK.VzLPRClient_Close(lprHandle);

            lprHandle = 0;

            tabPageVideo.Refresh();
            rec_chepai_xianshi_label.Text = "";
            if (xintiao_timer_qidong_bool)
            {
                xintiao_timer.Enabled = false;
                fasong_duanxian_biaoji();//向数据库更改相机在线标记
                xintiao_timer.Dispose();
                xintiao_timer_qidong_bool = false;
            }
            Cursor.Current = Cursors.Default;
        }

        public void zhuapai()
        {
            if (cam_biechu_zaixian_biaoji)
            {
                MessageBox.Show("相机在别处已经被连接，本地只能查看，拒绝抓拍", "提示");
                return; //如果相机在别处已经登陆，在本地只能看视频，抓拍功能拒绝使用
            }
            if (lprHandle > 0)
            {
                ZHENSHI_SDK.VzLPRClient_ForceTrigger(lprHandle);
            }
        }

        private void chepai_yanse(int chepai_yansei)
        {   
                switch (chepai_yansei)//ZS车牌颜色
                {
                    case LC_BLUE:
                    rec_chepai_xianshi_label.BackColor = Color.Blue;
                    rec_chepai_xianshi_label.ForeColor = Color.White;
                        break;
                    case LC_YELLOW:
                    rec_chepai_xianshi_label.BackColor = Color.Yellow;
                    rec_chepai_xianshi_label.ForeColor = Color.Black;
                        break;
                    case LC_WHITE:
                    rec_chepai_xianshi_label.BackColor = Color.White;
                    rec_chepai_xianshi_label.ForeColor = Color.Black;
                        break;
                    case LC_BLACK:
                    rec_chepai_xianshi_label.BackColor = Color.Black;
                    rec_chepai_xianshi_label.ForeColor = Color.White;
                        break;
                    case LC_GREEN:
                    rec_chepai_xianshi_label.BackColor = Color.Green;
                    rec_chepai_xianshi_label.ForeColor = Color.White;
                        break;
                    default:
                    rec_chepai_xianshi_label.Text = "未识别";
                    rec_chepai_xianshi_label.BackColor = Color.Blue;
                        break;
                }
           
            }


        private int ZS_OnPlateResult(int handle,
                                    IntPtr pUserData,
                                    IntPtr pResult,
                                    uint uNumPlates,
                                    ZHENSHI_SDK.VZ_LPRC_RESULT_TYPE eResultType,
                                    IntPtr pImgFull,
                                    IntPtr pImgPlateClip
                                    )
        {
           
            if (eResultType != ZHENSHI_SDK.VZ_LPRC_RESULT_TYPE.VZ_LPRC_RESULT_REALTIME)
            {
                TimeSpan timeSpan = DateTime.Now - shangci_paizhao_shijian;
                if (timeSpan.TotalSeconds < Const_Struct.PAIZHAO_JIANGE) return 0;
                shangci_paizhao_shijian = DateTime.Now;
               

                ZHENSHI_SDK.TH_PlateResult result = (ZHENSHI_SDK.TH_PlateResult)Marshal.PtrToStructure(pResult, typeof(ZHENSHI_SDK.TH_PlateResult));
                ZHENSHI_SDK.VZ_LPR_MSG_PLATE_INFO plateInfo = new ZHENSHI_SDK.VZ_LPR_MSG_PLATE_INFO();
                string strLicense = new string(result.license);
                if (strLicense.Substring(0,1)== "_" && !zhuapai_bool) return 0;
                if (strLicense.Trim() == "" && !zhuapai_bool) return 0;
                if (zhuapai_bool)
                { //当无牌车入场/出场时，默认为蓝牌小汽车
                    strLicense = zhuapai_chepai;
                    result.nType = LT_BLUE;
                    result.nColor = LC_BLUE;
                }
                plateInfo.plate = strLicense;
                
                rev_chepai = plateInfo.plate;
                rec_chepai_xianshi_label.Text = rev_chepai;
               // rev_chepai_yanse = result.nColor;

                //chepai_lexing(result.nType,
                //             ref rev_qiche_leixing);
                chepai_lexing(result.nType);
                chepai_yanse(result.nColor); 
                WIN32_API_FUN.MessageBeep(100);
               
            if (baocun_recimage(plateInfo, pImgFull) == -1)
                {
                    WIN32_API_FUN.PostMessage(parent_Frm_handle, Const_Struct.MSG_BAOCUN_TUPIAN_SHIBAI, cam_num, handle);
                }
                taizha();
                WIN32_API_FUN.PostMessage(parent_Frm_handle, Const_Struct.MSG_PLATE_INFO, cam_num, handle);
            }
            return 0;
       }

        private int baocun_recimage(ZHENSHI_SDK.VZ_LPR_MSG_PLATE_INFO plateInfo, IntPtr pImgFull)
        {
            //在pictureBox控件上显示图片；
            //在指定路径上保存图片
            DateTime now = DateTime.Now;
            string sTime = string.Format("{0:yyyyMMddHHmmssffff}", now);

           // string path = strImageDir + sTime + ".jpg";
            string path = String.Format("{0}\\{1}.jpg", strImageDir, sTime);
            if (ZHENSHI_SDK.VzLPRClient_ImageSaveToJpeg(pImgFull, path, 80) == 0)
            {
                rev_image_path_name = path;
                return 0;
            }        
            else
                return -1;
        }

        public void taizha()
        {
            ZHENSHI_SDK.VzLPRClient_SetIOOutputAuto(lprHandle, TAIZHA_ZHENSHI, 500);
        }
        public void luozha()
        {
            ZHENSHI_SDK.VzLPRClient_SetIOOutputAuto(lprHandle, LUOZHA_ZHENSHI, 500);
        }
        private void chepai_lexing(int shibie_chexing//,
                                   //ref string qiche_leixing
                                  )
        {
            switch (shibie_chexing)//车型
            {
                case 0:
                    {
                        rev_qiche_leixing = "未知车型";
                        break;
                    }

                case LT_YELLOW2:
                    {
                        rev_qiche_leixing = "大型车";
                        break;
                    }
                case LT_YELLOW| LT_TRACTOR:
                    {
                        rev_qiche_leixing = "中型车";
                        break;
                    }
                case LT_BLUE| LT_BLACK:
                    {
                        rev_qiche_leixing = "小型车";
                        break;
                    }                             
                default:
                    {
                        rev_qiche_leixing = "小型车";
                        break;
                    }
            }
          
        }
        //生成保存文件的路径，并初始化sdk
        //调用一次就可以了。
        public void imagepath_init()
        {
            strImageDir = Application.StartupPath + "\\image";
            if (!Directory.Exists(strImageDir))
            {
                Directory.CreateDirectory(strImageDir);
            }
          //  ZHENSHI_SDK.VzLPRClient_Setup();
        }
        //关闭所有相机后，调用一次就可以了。
        public void all_exit()
        {
            // ZHENSHI_SDK.VzLPRClient_Cleanup();
            //WIN32_API_FUN.PostMessage(parent_Frm_handle, MSG_PLATE_INFO_ZS, cam_num, lprHandle);
        }

        private bool xiangji_shifou_zaixian()
        {
            DB_Class db = new DB_Class();
            return db.xiangji_shifou_zaixian(camIP);
        }
        private void fasong_xintiao(object source, System.Timers.ElapsedEventArgs e)
        {
            DB_Class db = new DB_Class();
            db.update_xiangji_zaixiang_biaoji(camIP, true);
        }
        private void fasong_duanxian_biaoji()
        {
            DB_Class db = new DB_Class();
            db.update_xiangji_zaixiang_biaoji(camIP, false);
        }
    }
}

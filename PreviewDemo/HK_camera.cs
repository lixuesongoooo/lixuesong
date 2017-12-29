using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using Flash_Packing_Client;

namespace PreviewDemo
{
    class HK_camera
    {
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

       
        private ComboBox comboxIP;

        //海康特有变量
        public Int32 m_lUserID = -1; //-- private static int lprHandle = 0;//nCamId = -1;
        private bool logoutRtn = true;
        public CHCNetSDK.NET_DVR_DEVICEINFO_V30 m_struDeviceInfo;
        private int m_lRealHandle = -1;//m_nPlayHandle
        private bool m_bGuard = false; //相机是否启动了车牌输出功能
        private Int32 m_lFortifyHandle = -1;
        private CHCNetSDK.MSGCallBack m_falarmData = null;
        private bool m_bJpegCapture = false;


        //海康特有变量//
        private Label rec_chepai_xianshi_label = null;
        private IntPtr parent_Frm_handle;
        private string camIP;
        private TabPage tabPageVideo = null;

        private static string strImageDir = "";

        public bool zhuapai_bool = false;//无牌车入场/出场时需要抓拍，将图片保存
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

        public bool camera_connect()
        {    
            //连接海康相机       
            m_lUserID = CHCNetSDK.NET_DVR_Login_V30(camIP, 80, "admin", "admin",ref m_struDeviceInfo);
            if (m_lUserID == 0)
            {
                MessageBox.Show("连接设备失败HK_" + cam_num.ToString());
                return false;
            }
            else
            {
                logoutRtn = false;
            }
            //如果之前已经在显示实时图像，先停止显示
            if (m_lRealHandle != -1) 
            {
                if (!stop_shishi_yulan()) return false;
            }
            //显示实时图像
            CHCNetSDK.NET_DVR_CLIENTINFO lpClientInfo = new CHCNetSDK.NET_DVR_CLIENTINFO();
            lpClientInfo.lChannel = 1;
            lpClientInfo.lLinkMode = 0x0000;
            lpClientInfo.sMultiCastIP = "";
            lpClientInfo.hPlayWnd = tabPageVideo.Handle;
          //  m_ptrRealHandle = tabPageVideo.Handle;
          //  m_fRealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);
            IntPtr hWnd;
            hWnd = tabPageVideo.Handle;
            if ((int)hWnd > 0)
            {
                IntPtr pUser = new IntPtr();
                m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V30(m_lUserID, ref lpClientInfo, null/*m_fRealData*/, pUser, 1);
                if (m_lRealHandle == -1)
                {
                    uint nError = CHCNetSDK.NET_DVR_GetLastError();
                    // DebugInfo("HK_实时播放_fail %d!");
                    return false;
                }
            }
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

        private bool stop_shishi_yulan()
        {
            bool bStopReal = CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
            if (!bStopReal)
            {
               // uint nError = CHCNetSDK.NET_DVR_GetLastError();
                // DebugInfo("NET_DVR_StopRealPlay fail!");
                return false;
            }
            else
            {
                tabPageVideo.Refresh();
                return bStopReal;
            }
            
        }

        private bool qidong_chepai_shuchu_gongneng()
        {
            if (m_bGuard)
            {
                if (m_lFortifyHandle > -1)
                {
                    if (CHCNetSDK.NET_DVR_CloseAlarmChan_V30(m_lFortifyHandle))
                    {
                       // DebugInfo("NET_DVR_CloseAlarmChan_V30 Succ");
                       // btnFortify.Text = "Alarm Guard";
                        m_bGuard = !m_bGuard;
                    }
                    else
                    {
                        //  uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        //  string str = "NET_DVR_CloseAlarmChan_V30 failed, error code= " + iLastErr;
                        //  DebugInfo(str);
                        return false;
                    }
                }

            }
            else
            {
                CHCNetSDK.NET_DVR_SETUPALARM_PARAM m_struSetupParam = new CHCNetSDK.NET_DVR_SETUPALARM_PARAM();
                m_struSetupParam.dwSize = (uint)Marshal.SizeOf(m_struSetupParam);
                m_struSetupParam.byLevel = 1;
                m_struSetupParam.byAlarmInfoType = 1;

                m_lFortifyHandle = CHCNetSDK.NET_DVR_SetupAlarmChan_V41(m_lUserID, ref m_struSetupParam);
                if (m_lFortifyHandle != -1)
                {
                   // btnFortify.Text = "Unguard";
                    m_bGuard = !m_bGuard;
                  //  DebugInfo("NET_DVR_SetupAlarmChan_V30 Succeed");
                }
                else
                {
                    // uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    //  string str = "NET_DVR_SetupAlarmChan_V30 failed, error code= " + iLastErr;
                    //  DebugInfo(str);
                    return false;
                }
                m_falarmData = new CHCNetSDK.MSGCallBack(MsgCallback);
                if (CHCNetSDK.NET_DVR_SetDVRMessageCallBack_V30(m_falarmData, IntPtr.Zero))
                {
                    // DebugInfo("NET_DVR_SetDVRMessageCallBack_V30 Succeed");
                    return true;
                }
                else
                {
                    // uint i = CHCNetSDK.NET_DVR_GetLastError();
                    // DebugInfo("NET_DVR_SetDVRMessageCallBack_V30 Failed");
                    return false;
                }
            }
            return false;
        }

        private void MsgCallback(int lCommand, ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
          //  MyDebugInfo AlarmInfo = new MyDebugInfo(DebugInfo);
            switch (lCommand)
            {
                case CHCNetSDK.COMM_ALARM:
                    ProcessCommAlarm(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_ALARM_V30:
                    ProcessCommAlarm_V30(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_UPLOAD_PLATE_RESULT:
                    ProcessCommAlarm_Plate(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_ITS_PLATE_RESULT:
                    ProcessCommAlarm_ITSPlate(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                //         case CHCNetSDK.COMM_ALARM_RULE:
                //             this.BeginInvoke(AlarmInfo, "COMM_ALARM_RULE");
                //             break;
                //         case CHCNetSDK.COMM_TRADEINFO:
                //             this.BeginInvoke(AlarmInfo, "COMM_TRADEINFO");
                //             break;
                //         case CHCNetSDK.COMM_IPCCFG:
                //             this.BeginInvoke(AlarmInfo, "COMM_IPCCFG");
                //             break;
                //         case CHCNetSDK.COMM_IPCCFG_V31:
                //             this.BeginInvoke(AlarmInfo, "COMM_IPCCFG_V31");
                //             break;
                default:
                    break;
            }
        }


        public void ProcessCommAlarm(ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
           // MyDebugInfo AlarmInfo = new MyDebugInfo(DebugInfo);
            CHCNetSDK.NET_DVR_ALARMINFO struAlarmInfo = new CHCNetSDK.NET_DVR_ALARMINFO();

            struAlarmInfo = (CHCNetSDK.NET_DVR_ALARMINFO)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_DVR_ALARMINFO));
         //   string str;
            switch (struAlarmInfo.dwAlarmType)
            {
                case 0:
                    //this.BeginInvoke(AlarmInfo, "sensor alarm");
                    break;
                case 1:
                  //  this.BeginInvoke(AlarmInfo, "hard disk full");
                    break;
                case 2:
                   // this.BeginInvoke(AlarmInfo, "video lost");
                    break;
                case 3:
                   // str = "";
                  //  str += pAlarmer.sDeviceIP;
                  //  str += " motion detection";
                   // this.BeginInvoke(AlarmInfo, str);
                    m_bJpegCapture = true;
                    break;
                case 4:
                  //  this.BeginInvoke(AlarmInfo, "hard disk unformatted");
                    break;
                case 5:
                 //   this.BeginInvoke(AlarmInfo, "hard disk error");
                    break;
                case 6:
                  //  this.BeginInvoke(AlarmInfo, "tampering detection");
                    break;
                case 7:
                   // this.BeginInvoke(AlarmInfo, "unmatched video output standard");
                    break;
                case 8:
                  //  this.BeginInvoke(AlarmInfo, "illegal operation");
                    break;
                default:
                  //  this.BeginInvoke(AlarmInfo, "Unknow alarm");
                    break;
            }
        }

        private void ProcessCommAlarm_V30(ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
          //  MyDebugInfo AlarmInfo = new MyDebugInfo(DebugInfo);
            CHCNetSDK.NET_DVR_ALARMINFO_V30 struAlarmInfoV30 = new CHCNetSDK.NET_DVR_ALARMINFO_V30();

            struAlarmInfoV30 = (CHCNetSDK.NET_DVR_ALARMINFO_V30)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_DVR_ALARMINFO_V30));

            string str;
            switch (struAlarmInfoV30.dwAlarmType)
            {
                case 0:
                   // this.BeginInvoke(AlarmInfo, "sensor alarm");
                    break;
                case 1:
                   // this.BeginInvoke(AlarmInfo, "hard disk full");
                    break;
                case 2:
                  //  this.BeginInvoke(AlarmInfo, "video lost");
                    break;
                case 3:
                  //  str = "";
                 //   str += pAlarmer.sDeviceIP;
                 //   str += " motion detection";
                //    this.BeginInvoke(AlarmInfo, str);
                    break;
                case 4:
                  //  this.BeginInvoke(AlarmInfo, "hard disk unformatted");
                    break;
                case 5:
                  //  this.BeginInvoke(AlarmInfo, "hard disk error");
                    break;
                case 6:
                  //  this.BeginInvoke(AlarmInfo, "tampering detection");
                    break;
                case 7:
                   // this.BeginInvoke(AlarmInfo, "unmatched video output standard");
                    break;
                case 8:
                   // this.BeginInvoke(AlarmInfo, "illegal operation");
                    break;
                case 9:
                  //  this.BeginInvoke(AlarmInfo, "videl Signal abnormal");
                    break;
                case 10:
                  //  this.BeginInvoke(AlarmInfo, "record abnormal");
                    break;
                default:
                   // this.BeginInvoke(AlarmInfo, "Unknow alarm");
                    break;
            }

        }

        private void ProcessCommAlarm_Plate(ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
         //   MyDebugInfo AlarmInfo = new MyDebugInfo(DebugInfo);
            CHCNetSDK.NET_DVR_PLATE_RESULT struAlarmInfoV30 = new CHCNetSDK.NET_DVR_PLATE_RESULT();
            uint dwSize = (uint)Marshal.SizeOf(struAlarmInfoV30);

            struAlarmInfoV30 = (CHCNetSDK.NET_DVR_PLATE_RESULT)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_DVR_PLATE_RESULT));
            //struAlarmInfoV30.struPlateInfo.sLicense
            CHCNetSDK.NET_ITS_PICTURE_INFO[] PicInfo;
           
            baocun_jinjing_tupian();
        }

        private void ProcessCommAlarm_ITSPlate(ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
         //   MyDebugInfo AlarmInfo = new MyDebugInfo(DebugInfo);

            CHCNetSDK.NET_ITS_PLATE_RESULT struAlarmInfoV30 = new CHCNetSDK.NET_ITS_PLATE_RESULT();
            uint dwSize = (uint)Marshal.SizeOf(struAlarmInfoV30);
            struAlarmInfoV30 = (CHCNetSDK.NET_ITS_PLATE_RESULT)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_ITS_PLATE_RESULT));
            for (int i = 0; i < struAlarmInfoV30.dwPicNum; i++)
            {
                if (struAlarmInfoV30.struPicInfo[i].dwDataLen != 0)
                {
                    string str = "D:/pic_type_" + struAlarmInfoV30.struPicInfo[i].byType + "_Num" + (i + 1) + ".jpg";
                    FileStream fs = new FileStream(str, FileMode.Create);
                    int iLen = (int)struAlarmInfoV30.struPicInfo[i].dwDataLen;
                    byte[] by = new byte[iLen];
                    Marshal.Copy(struAlarmInfoV30.struPicInfo[i].pBuffer, by, 0, iLen);
                    fs.Write(by, 0, iLen);
                    fs.Close();
                }
            }

        }
        private void songchu_chepai(string chepai_haoma,
                                   int cp_yanse,
                                   int cheliang_leixing ,
                                   CHCNetSDK.NET_DVR_PLATE_RESULT chepai_info_result
                                   )
        {
                TimeSpan timeSpan = DateTime.Now - shangci_paizhao_shijian;
                if (timeSpan.TotalSeconds < Const_Struct.PAIZHAO_JIANGE) return ;
                shangci_paizhao_shijian = DateTime.Now;
                if (chepai_haoma.Substring(0, 1) == "_" && !zhuapai_bool) return ;
                if (chepai_haoma.Trim() == "" && !zhuapai_bool) return ;
                if (zhuapai_bool)
                { //当无牌车入场/出场时，默认为蓝牌小汽车
                    chepai_haoma = zhuapai_chepai;
                    cheliang_leixing = LT_BLUE;
                    cp_yanse = LC_BLUE;
                }
                rev_chepai = chepai_haoma;
                rec_chepai_xianshi_label.Text = rev_chepai;
               
                chepai_lexing(cheliang_leixing);
                chepai_yanse(cp_yanse);
                WIN32_API_FUN.MessageBeep(100);

                if (baocun_recimage(chepai_info_result) == -1)
                {
                    WIN32_API_FUN.PostMessage(parent_Frm_handle, Const_Struct.MSG_BAOCUN_TUPIAN_SHIBAI, cam_num, 0);
                }
                taizha();
                WIN32_API_FUN.PostMessage(parent_Frm_handle, Const_Struct.MSG_PLATE_INFO, cam_num, 0);

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

        private void chepai_lexing(int shibie_chexing)
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
                case LT_YELLOW | LT_TRACTOR:
                    {
                        rev_qiche_leixing = "中型车";
                        break;
                    }
                case LT_BLUE | LT_BLACK:
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

        private int baocun_recimage(CHCNetSDK.NET_DVR_PLATE_RESULT chepai_info_result)
        {
            //在pictureBox控件上显示图片；
            //在指定路径上保存图片
            DateTime now = DateTime.Now;
            string sTime = string.Format("{0:yyyyMMddHHmmssffff}", now);
            // string path = strImageDir + sTime + ".jpg";
            string path = String.Format("{0}\\{1}.jpg", strImageDir, sTime);
            if (baocun_jinjing_tupian(path,chepai_info_result) == 0)
            {
                rev_image_path_name = path;
                return 0;
            }
            else
                return -1;
        }
        private int baocun_jinjing_tupian(string path ,CHCNetSDK.NET_DVR_PLATE_RESULT chepai_info_result)
        {
            if (chepai_info_result.byResultType == 1 && chepai_info_result.dwPicLen != 0)
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                int iLen = (int)chepai_info_result.dwPicLen;
                byte[] by = new byte[iLen];
                Marshal.Copy(chepai_info_result.pBuffer1, by, 0, iLen);
                fs.Write(by, 0, iLen);
                fs.Close();
                return 0;
            }
            else
                return -1;
            //if (chepai_info_result.dwPicPlateLen != 0)
            //{
            //    FileStream fs = new FileStream("D:/车牌图.jpg", FileMode.Create);
            //    int iLen = (int)chepai_info_result.dwPicPlateLen;
            //    byte[] by = new byte[iLen];
            //    Marshal.Copy(chepai_info_result.pBuffer2, by, 0, iLen);
            //    fs.Write(by, 0, iLen);
            //    fs.Close();
            //}
            //if (chepai_info_result.dwFarCarPicLen != 0)
            //{
            //    FileStream fs = new FileStream("D:/远景图.jpg", FileMode.Create);
            //    int iLen = (int)chepai_info_result.dwFarCarPicLen;
            //    byte[] by = new byte[iLen];
            //    Marshal.Copy(chepai_info_result.pBuffer5, by, 0, iLen);
            //    fs.Write(by, 0, iLen);
            //    fs.Close();
            //}
          //  return 1;
        }
        public void taizha()
        {
            CHCNetSDK.NET_DVR_BARRIERGATE_CFG m_struControlCond = new CHCNetSDK.NET_DVR_BARRIERGATE_CFG();
            Int32 dwSize = Marshal.SizeOf(m_struControlCond);
            m_struControlCond.dwSize = (uint)dwSize;
            m_struControlCond.dwChannel = 1;
            m_struControlCond.byLaneNo = 1;
            m_struControlCond.byBarrierGateCtrl = 1;

            IntPtr ptrControlCfg = Marshal.AllocHGlobal(dwSize);
            Marshal.StructureToPtr(m_struControlCond, ptrControlCfg, false);

            if (!CHCNetSDK.NET_DVR_RemoteControl(m_lUserID, CHCNetSDK.NET_DVR_BARRIERGATE_CTRL, ptrControlCfg, (UInt32)dwSize))
            {
                uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                string str = "NET_DVR_BARRIERGATE_CTRL failed, error code= " + iLastErr;
               // DebugInfo(str);
            }
            Marshal.FreeHGlobal(ptrControlCfg);
            return;
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

        private void fasong_duanxian_biaojitest()
        {
            DB_Class db = new DB_Class();
            db.update_xiangji_zaixiang_biaoji(camIP, false);
        }


    }
}

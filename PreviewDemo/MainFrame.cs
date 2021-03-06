using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;

using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace PreviewDemo
{
    /// <summary>
    /// Form1 的摘要说明。
    /// </summary>
    public class MainFrame : System.Windows.Forms.Form
    {
        private CHCNetSDK.REALDATACALLBACK m_fRealData = null;
        private CHCNetSDK.MSGCallBack m_falarmData = null;
        public CHCNetSDK.NET_DVR_DEVICEINFO_V30 m_struDeviceInfo;
        public CHCNetSDK.NET_DVR_SNAPCFG struSnapCfg;
        public CHCNetSDK.NET_ITS_OVERLAPCFG_COND m_struOverCond;
        public CHCNetSDK.NET_ITS_OVERLAP_CFG m_struOverCfg;

        public CHCNetSDK.NET_ITC_TRIGGERCFG struTriggerCfg = new CHCNetSDK.NET_ITC_TRIGGERCFG();
        public CHCNetSDK.NET_ITC_POST_RS485_RADAR_PARAM stru485RadarParam = new CHCNetSDK.NET_ITC_POST_RS485_RADAR_PARAM();
        public CHCNetSDK.NET_ITC_EPOLICE_RS485_PARAM struPERs485 = new CHCNetSDK.NET_ITC_EPOLICE_RS485_PARAM();
        public CHCNetSDK.NET_DVR_CURTRIGGERMODE struCurrentMode = new CHCNetSDK.NET_DVR_CURTRIGGERMODE();
        public CHCNetSDK.NET_ITC_POST_SINGLEIO_PARAM struSingleIO = new CHCNetSDK.NET_ITC_POST_SINGLEIO_PARAM();
        public CHCNetSDK.NET_ITC_POST_HVT_PARAM_V50 struHvtParam = new CHCNetSDK.NET_ITC_POST_HVT_PARAM_V50();

        public Int32 m_lUserID = -1;
        private bool m_bInitSDK = false;
        private Int32 m_lRealHandle = -1;
        private Int32 m_lPort = -1;
        private bool m_bJpegCapture = false;
        private bool m_bGuard = false;
        private bool m_bOpenSound = false;
        private Int32 m_lFortifyHandle = -1;
        private bool logoutRtn = true;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.PictureBox RealPlayWnd;
        private Button btnJpegCapture;
        private IntPtr m_ptrRealHandle;
        private Button btnFortify;
        private RichTextBox TextBoxInfo;
        private Label label5;
        private Button btnOpenSound;
        private Button Logout;
        private Label label6;
        private Button btnStopPre;
        private Button Btn_CANCEL;
        private Button SetOverlayConfig;
        private Button btnControl;
        private Button btnEntranceCfg;
        private Button btnManual;
        private Button btnTriggerCfg;
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;

        public MainFrame()
        {
            //
            // Windows 窗体设计器支持所必需的
            //
            InitializeComponent();

            m_bInitSDK = CHCNetSDK.NET_DVR_Init();
            if (m_bInitSDK == false)
            {
                MessageBox.Show("NET_DVR_Init error!");
                return;
            }
            else
            {
                CHCNetSDK.NET_DVR_SetLogToFile(3, "C:\\SdkLog\\", true);
            }
            //
            // TODO: 在 InitializeComponent 调用后添加任何构造函数代码
            //
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
            }
            if (m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout_V30(m_lUserID);
            }
            if (m_bInitSDK == true)
            {
                CHCNetSDK.NET_DVR_Cleanup();
            }
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrame));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.RealPlayWnd = new System.Windows.Forms.PictureBox();
            this.btnJpegCapture = new System.Windows.Forms.Button();
            this.btnFortify = new System.Windows.Forms.Button();
            this.TextBoxInfo = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnOpenSound = new System.Windows.Forms.Button();
            this.Logout = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnStopPre = new System.Windows.Forms.Button();
            this.Btn_CANCEL = new System.Windows.Forms.Button();
            this.SetOverlayConfig = new System.Windows.Forms.Button();
            this.btnControl = new System.Windows.Forms.Button();
            this.btnEntranceCfg = new System.Windows.Forms.Button();
            this.btnManual = new System.Windows.Forms.Button();
            this.btnTriggerCfg = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 342);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Device IP Address";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(1, 411);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "User Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(2, 440);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Password";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(0, 376);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Device Port Number";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(250, 339);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(88, 28);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = " Login";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(250, 377);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(88, 30);
            this.btnPreview.TabIndex = 2;
            this.btnPreview.Text = "Live view";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(133, 374);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(103, 22);
            this.textBoxPort.TabIndex = 3;
            this.textBoxPort.Text = "8000";
            this.textBoxPort.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(133, 341);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(103, 22);
            this.textBoxIP.TabIndex = 3;
            this.textBoxIP.Text = "10.9.4.110";
            this.textBoxIP.TextChanged += new System.EventHandler(this.textBoxIP_TextChanged);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(133, 438);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(103, 22);
            this.textBoxPassword.TabIndex = 3;
            this.textBoxPassword.Text = "12345";
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(133, 409);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(103, 22);
            this.textBoxUserName.TabIndex = 3;
            this.textBoxUserName.Text = "admin";
            // 
            // RealPlayWnd
            // 
            this.RealPlayWnd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("RealPlayWnd.BackgroundImage")));
            this.RealPlayWnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RealPlayWnd.Location = new System.Drawing.Point(3, 1);
            this.RealPlayWnd.Name = "RealPlayWnd";
            this.RealPlayWnd.Size = new System.Drawing.Size(423, 322);
            this.RealPlayWnd.TabIndex = 4;
            this.RealPlayWnd.TabStop = false;
            // 
            // btnJpegCapture
            // 
            this.btnJpegCapture.Location = new System.Drawing.Point(458, 377);
            this.btnJpegCapture.Name = "btnJpegCapture";
            this.btnJpegCapture.Size = new System.Drawing.Size(101, 30);
            this.btnJpegCapture.TabIndex = 5;
            this.btnJpegCapture.Text = "Continous Snap";
            this.btnJpegCapture.UseVisualStyleBackColor = true;
            this.btnJpegCapture.Click += new System.EventHandler(this.btnJpegCapture_Click);
            // 
            // btnFortify
            // 
            this.btnFortify.Location = new System.Drawing.Point(456, 339);
            this.btnFortify.Name = "btnFortify";
            this.btnFortify.Size = new System.Drawing.Size(93, 29);
            this.btnFortify.TabIndex = 7;
            this.btnFortify.Text = "Arm or Disarm";
            this.btnFortify.UseVisualStyleBackColor = true;
            this.btnFortify.Click += new System.EventHandler(this.btnFortify_Click);
            // 
            // TextBoxInfo
            // 
            this.TextBoxInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TextBoxInfo.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxInfo.Location = new System.Drawing.Point(432, 1);
            this.TextBoxInfo.Name = "TextBoxInfo";
            this.TextBoxInfo.Size = new System.Drawing.Size(270, 322);
            this.TextBoxInfo.TabIndex = 8;
            this.TextBoxInfo.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(588, 308);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "Debug Info Display";
            // 
            // btnOpenSound
            // 
            this.btnOpenSound.Location = new System.Drawing.Point(250, 419);
            this.btnOpenSound.Name = "btnOpenSound";
            this.btnOpenSound.Size = new System.Drawing.Size(90, 26);
            this.btnOpenSound.TabIndex = 13;
            this.btnOpenSound.Text = "Open Sound";
            this.btnOpenSound.UseVisualStyleBackColor = true;
            this.btnOpenSound.Click += new System.EventHandler(this.btnOpenSound_Click);
            // 
            // Logout
            // 
            this.Logout.Location = new System.Drawing.Point(346, 339);
            this.Logout.Name = "Logout";
            this.Logout.Size = new System.Drawing.Size(98, 29);
            this.Logout.TabIndex = 16;
            this.Logout.Text = "Logout";
            this.Logout.UseVisualStyleBackColor = true;
            this.Logout.Click += new System.EventHandler(this.Logout_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(2, 308);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 17);
            this.label6.TabIndex = 17;
            this.label6.Text = "Play Window";
            // 
            // btnStopPre
            // 
            this.btnStopPre.Location = new System.Drawing.Point(346, 377);
            this.btnStopPre.Name = "btnStopPre";
            this.btnStopPre.Size = new System.Drawing.Size(98, 30);
            this.btnStopPre.TabIndex = 18;
            this.btnStopPre.Text = "Stop Live View";
            this.btnStopPre.UseVisualStyleBackColor = true;
            this.btnStopPre.Click += new System.EventHandler(this.btnStopPre_Click);
            // 
            // Btn_CANCEL
            // 
            this.Btn_CANCEL.Location = new System.Drawing.Point(573, 419);
            this.Btn_CANCEL.Name = "Btn_CANCEL";
            this.Btn_CANCEL.Size = new System.Drawing.Size(97, 26);
            this.Btn_CANCEL.TabIndex = 20;
            this.Btn_CANCEL.Text = "Exit";
            this.Btn_CANCEL.UseVisualStyleBackColor = true;
            this.Btn_CANCEL.Click += new System.EventHandler(this.Btn_CANCEL_Click);
            // 
            // SetOverlayConfig
            // 
            this.SetOverlayConfig.Location = new System.Drawing.Point(346, 419);
            this.SetOverlayConfig.Name = "SetOverlayConfig";
            this.SetOverlayConfig.Size = new System.Drawing.Size(93, 26);
            this.SetOverlayConfig.TabIndex = 25;
            this.SetOverlayConfig.Text = "Overlay Set";
            this.SetOverlayConfig.UseVisualStyleBackColor = true;
            this.SetOverlayConfig.Click += new System.EventHandler(this.SetConfig_Click);
            // 
            // btnControl
            // 
            this.btnControl.Location = new System.Drawing.Point(458, 418);
            this.btnControl.Name = "btnControl";
            this.btnControl.Size = new System.Drawing.Size(92, 26);
            this.btnControl.TabIndex = 26;
            this.btnControl.Text = "Control";
            this.btnControl.UseVisualStyleBackColor = true;
            this.btnControl.Click += new System.EventHandler(this.btnControl_Click);
            // 
            // btnEntranceCfg
            // 
            this.btnEntranceCfg.Location = new System.Drawing.Point(573, 339);
            this.btnEntranceCfg.Name = "btnEntranceCfg";
            this.btnEntranceCfg.Size = new System.Drawing.Size(84, 28);
            this.btnEntranceCfg.TabIndex = 27;
            this.btnEntranceCfg.Text = "Entrance Cfg";
            this.btnEntranceCfg.UseVisualStyleBackColor = true;
            this.btnEntranceCfg.Click += new System.EventHandler(this.btnEntranceCfg_Click);
            // 
            // btnManual
            // 
            this.btnManual.Location = new System.Drawing.Point(573, 377);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(97, 30);
            this.btnManual.TabIndex = 28;
            this.btnManual.Text = "Manual Snap";
            this.btnManual.UseVisualStyleBackColor = true;
            this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
            // 
            // btnTriggerCfg
            // 
            this.btnTriggerCfg.Location = new System.Drawing.Point(250, 462);
            this.btnTriggerCfg.Name = "btnTriggerCfg";
            this.btnTriggerCfg.Size = new System.Drawing.Size(88, 23);
            this.btnTriggerCfg.TabIndex = 29;
            this.btnTriggerCfg.Text = "TriggerCfg";
            this.btnTriggerCfg.UseVisualStyleBackColor = true;
            this.btnTriggerCfg.Click += new System.EventHandler(this.btnTriggerCfg_Click);
            // 
            // MainFrame
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 15);
            this.ClientSize = new System.Drawing.Size(705, 502);
            this.Controls.Add(this.btnTriggerCfg);
            this.Controls.Add(this.btnManual);
            this.Controls.Add(this.btnEntranceCfg);
            this.Controls.Add(this.btnControl);
            this.Controls.Add(this.SetOverlayConfig);
            this.Controls.Add(this.Btn_CANCEL);
            this.Controls.Add(this.btnStopPre);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Logout);
            this.Controls.Add(this.btnOpenSound);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TextBoxInfo);
            this.Controls.Add(this.btnFortify);
            this.Controls.Add(this.btnJpegCapture);
            this.Controls.Add(this.RealPlayWnd);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxUserName);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Location = new System.Drawing.Point(50, 0);
            this.Name = "MainFrame";
            this.Text = "Hikvison Client SDK Demo";
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new MainFrame());
        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, System.EventArgs e)
        {
            if (textBoxIP.Text == "" || textBoxPort.Text == "" ||
                textBoxUserName.Text == "" || textBoxPassword.Text == "")
            {
                MessageBox.Show("Please input prarameters: ");
                return;
            }
            string DVRIPAddress = textBoxIP.Text;
            Int16 DVRPortNumber = Int16.Parse(textBoxPort.Text);
            string DVRUserName = textBoxUserName.Text;
            string DVRPassword = textBoxPassword.Text;
            m_lUserID = CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, ref m_struDeviceInfo);
            if (m_lUserID == -1)
            {
                MessageBox.Show("login error!");
                return;
            }
            else
            {
                logoutRtn = false;
                MessageBox.Show("Login Success!");
            }

        }

        private void btnPreview_Click(object sender, System.EventArgs e)
        {
            CHCNetSDK.NET_DVR_CLIENTINFO lpClientInfo = new CHCNetSDK.NET_DVR_CLIENTINFO();

            lpClientInfo.lChannel = 1;
            lpClientInfo.lLinkMode = 0x0000;
            lpClientInfo.sMultiCastIP = "";
            
            lpClientInfo.hPlayWnd = RealPlayWnd.Handle;
            m_ptrRealHandle = RealPlayWnd.Handle;
            m_fRealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);
            IntPtr pUser = new IntPtr();
            m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V30(m_lUserID, ref lpClientInfo, null/*m_fRealData*/, pUser, 1);

           
            if (m_lRealHandle == -1)
            {
                uint nError = CHCNetSDK.NET_DVR_GetLastError();
                DebugInfo("NET_DVR_RealPlay fail %d!");
                return;
            }
        }

        public void RemoteDisplayCBFun(int nPort, IntPtr pBuf, int nSize, int nWidth, int nHeight, int nStamp, int nType, int nReserved)
        {
            MyDebugInfo AlarmInfo = new MyDebugInfo(DebugInfo);
            if (!m_bJpegCapture)
            {
                return;
            }
            else
            {
                uint nLastErr = 100;
                /*capture JPEG image and save into local disk*/
                if (!PlayCtrl.PlayM4_ConvertToJpegFile(pBuf, nSize, nWidth, nHeight, nType, "S:/Capture.jpg"))
                {
                    //Debug.WriteLine("PlayM4_ConvertToJpegFile fail");
                    nLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                    this.BeginInvoke(AlarmInfo, "Jpeg Capture failed");
                }
                else
                {
                    this.BeginInvoke(AlarmInfo, "Jpeg Capture Succeed");
                    //Debug.WriteLine("PlayM4_ConvertToJpegFile Succ");
                }

            }
            
            m_bJpegCapture = false;
        }

        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, ref byte pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {
        //    MyDebugInfo AlarmInfo = new MyDebugInfo(DebugInfo);
        //    switch (dwDataType)
        //    {
        //        case CHCNetSDK.NET_DVR_SYSHEAD:     // sys head
        //            if (!PlayCtrl.PlayM4_GetPort(ref m_lPort))
        //            {
        //                MessageBox.Show("Get Port Fail");
        //            }

        //            if (dwBufSize > 0)
        //              {
        //                //set as stream mode, real-time stream under preview
        //                if (!PlayCtrl.PlayM4_SetStreamOpenMode(m_lPort, PlayCtrl.STREAME_REALTIME))
        //                {
        //                    this.BeginInvoke(AlarmInfo, "PlayM4_SetStreamOpenMode fail");
        //                }
        //                //start player
        //                if (!PlayCtrl.PlayM4_OpenStream(m_lPort, ref pBuffer, dwBufSize, 1024 * 1024))
        //                {
        //                    m_lPort = -1;
        //                    this.BeginInvoke(AlarmInfo, "PlayM4_OpenStream fail");
        //                    break;
        //                }
                        //set soft decode display callback function to capture
        //                m_fDisplayFun = new PlayCtrl.DISPLAYCBFUN(RemoteDisplayCBFun);
        //                if (!PlayCtrl.PlayM4_SetDisplayCallBack(m_lPort, m_fDisplayFun))
        //                {
        //                    this.BeginInvoke(AlarmInfo, "PlayM4_SetDisplayCallBack fail");
        //                }

                        //start play, set play window
        //                this.BeginInvoke(AlarmInfo, "About to call PlayM4_Play");

        //                if (!PlayCtrl.PlayM4_Play(m_lPort, m_ptrRealHandle))
        //                {
        //                    m_lPort = -1;
        //                    this.BeginInvoke(AlarmInfo, "PlayM4_Play fail");
        //                    break;
        //                }

                        //set frame buffer number

        //                if (!PlayCtrl.PlayM4_SetDisplayBuf(m_lPort, 15))
        //                {
        //                    this.BeginInvoke(AlarmInfo, "PlayM4_SetDisplayBuf fail");
        //                }

                        //set display mode
        //                if (!PlayCtrl.PlayM4_SetOverlayMode(m_lPort, 0, 0/* COLORREF(0)*/))//play off screen // todo!!!
        //                {
        //                    this.BeginInvoke(AlarmInfo, " PlayM4_SetOverlayMode fail");
        //                }
        //            }

        //            break;
        //        case CHCNetSDK.NET_DVR_STREAMDATA:     // video stream data
        //            if (dwBufSize > 0 && m_lPort != -1)
        //            {
        //                if (!PlayCtrl.PlayM4_InputData(m_lPort, ref pBuffer, dwBufSize))
        //                {
        //                    this.BeginInvoke(AlarmInfo, " PlayM4_InputData fail");
        //                }
        //            }
        //            break;

        //        case CHCNetSDK.NET_DVR_AUDIOSTREAMDATA:     //  Audio Stream Data
        //            if (dwBufSize > 0 && m_lPort != -1)
        //            {
        //                if (!PlayCtrl.PlayM4_InputVideoData(m_lPort, ref pBuffer, dwBufSize))
        //                {
        //                   this.BeginInvoke(AlarmInfo, "PlayM4_InputVideoData Fail");
        //                }
        //            }

        //            break;
        //        default:
        //            break;
        //    } */

        }



        public void MsgCallback(int lCommand, ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            MyDebugInfo AlarmInfo = new MyDebugInfo(DebugInfo);
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
            MyDebugInfo AlarmInfo = new MyDebugInfo(DebugInfo);
            CHCNetSDK.NET_DVR_ALARMINFO struAlarmInfo = new CHCNetSDK.NET_DVR_ALARMINFO();

            struAlarmInfo = (CHCNetSDK.NET_DVR_ALARMINFO)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_DVR_ALARMINFO));

            string str;
            switch (struAlarmInfo.dwAlarmType)
            {
                case 0: 
                    this.BeginInvoke(AlarmInfo, "sensor alarm");
                    break;
                case 1:
                    this.BeginInvoke(AlarmInfo, "hard disk full");
                    break;
                case 2:
                    this.BeginInvoke(AlarmInfo, "video lost");
                    break;
                case 3:
                    str = "";
                    str += pAlarmer.sDeviceIP;
                    str += " motion detection";
                    this.BeginInvoke(AlarmInfo, str);
                    m_bJpegCapture = true;
                    break;
                case 4:
                    this.BeginInvoke(AlarmInfo, "hard disk unformatted");
                    break;
                case 5:
                    this.BeginInvoke(AlarmInfo, "hard disk error");
                    break;
                case 6:
                    this.BeginInvoke(AlarmInfo, "tampering detection");
                    break;
                case 7:
                    this.BeginInvoke(AlarmInfo, "unmatched video output standard");
                    break;
                case 8:
                    this.BeginInvoke(AlarmInfo, "illegal operation");
                    break;
                default:
                    this.BeginInvoke(AlarmInfo, "Unknow alarm");
                    break;
            }
        }

        private void ProcessCommAlarm_V30(ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            MyDebugInfo AlarmInfo = new MyDebugInfo(DebugInfo);
            CHCNetSDK.NET_DVR_ALARMINFO_V30 struAlarmInfoV30 = new CHCNetSDK.NET_DVR_ALARMINFO_V30();

            struAlarmInfoV30 = (CHCNetSDK.NET_DVR_ALARMINFO_V30)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_DVR_ALARMINFO_V30));

            string str;
            switch (struAlarmInfoV30.dwAlarmType)
            {
                case 0:
                    this.BeginInvoke(AlarmInfo, "sensor alarm");
                    break;
                case 1:
                    this.BeginInvoke(AlarmInfo, "hard disk full");
                    break;
                case 2:
                    this.BeginInvoke(AlarmInfo, "video lost");
                    break;
                case 3:
                    str = "";
                    str += pAlarmer.sDeviceIP;
                    str += " motion detection";
                    this.BeginInvoke(AlarmInfo, str);
                    break;
                case 4:
                    this.BeginInvoke(AlarmInfo, "hard disk unformatted");
                    break;
                case 5:
                    this.BeginInvoke(AlarmInfo, "hard disk error");
                    break;
                case 6:
                    this.BeginInvoke(AlarmInfo, "tampering detection");
                    break;
                case 7:
                    this.BeginInvoke(AlarmInfo, "unmatched video output standard");
                    break;
                case 8:
                    this.BeginInvoke(AlarmInfo, "illegal operation");
                    break;
                case 9:
                    this.BeginInvoke(AlarmInfo, "videl Signal abnormal");
                    break;
                case 10:
                    this.BeginInvoke(AlarmInfo, "record abnormal");
                    break;
                default:
                    this.BeginInvoke(AlarmInfo, "Unknow alarm");
                    break;
            }
  
        }

        private void ProcessCommAlarm_Plate(ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            MyDebugInfo AlarmInfo = new MyDebugInfo(DebugInfo);

            CHCNetSDK.NET_DVR_PLATE_RESULT struAlarmInfoV30 = new CHCNetSDK.NET_DVR_PLATE_RESULT();
            uint dwSize = (uint)Marshal.SizeOf(struAlarmInfoV30);

            struAlarmInfoV30 = (CHCNetSDK.NET_DVR_PLATE_RESULT)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_DVR_PLATE_RESULT));

            if (struAlarmInfoV30.byResultType == 1 && struAlarmInfoV30.dwPicLen != 0)
            {
                FileStream fs = new FileStream("D:/近景图.jpg", FileMode.Create);
                int iLen = (int)struAlarmInfoV30.dwPicLen;
                byte[] by = new byte[iLen];
                Marshal.Copy(struAlarmInfoV30.pBuffer1, by, 0, iLen);
                fs.Write(by, 0, iLen);
                fs.Close();
            }
            if (struAlarmInfoV30.dwPicPlateLen != 0)
            {
                FileStream fs = new FileStream("D:/车牌图.jpg", FileMode.Create);
                int iLen = (int)struAlarmInfoV30.dwPicPlateLen;
                byte[] by = new byte[iLen];
                Marshal.Copy(struAlarmInfoV30.pBuffer2, by, 0, iLen);
                fs.Write(by, 0, iLen);
                fs.Close();
            }
            if (struAlarmInfoV30.dwFarCarPicLen != 0)
            {
                FileStream fs = new FileStream("D:/远景图.jpg", FileMode.Create);
                int iLen = (int)struAlarmInfoV30.dwFarCarPicLen;
                byte[] by = new byte[iLen];
                Marshal.Copy(struAlarmInfoV30.pBuffer5, by, 0, iLen);
                fs.Write(by, 0, iLen);
                fs.Close();
            }

        }

        private void ProcessCommAlarm_ITSPlate(ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            MyDebugInfo AlarmInfo = new MyDebugInfo(DebugInfo);

            CHCNetSDK.NET_ITS_PLATE_RESULT struAlarmInfoV30 = new CHCNetSDK.NET_ITS_PLATE_RESULT();
            uint dwSize = (uint)Marshal.SizeOf(struAlarmInfoV30);

            struAlarmInfoV30 = (CHCNetSDK.NET_ITS_PLATE_RESULT)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_ITS_PLATE_RESULT));

            for (int i = 0; i < struAlarmInfoV30.dwPicNum;i++ )
            {
                if (struAlarmInfoV30.struPicInfo[i].dwDataLen != 0)
                {
                    string str = "D:/pic_type_" + struAlarmInfoV30.struPicInfo[i].byType +"_Num"+(i+1)+ ".jpg";
                    FileStream fs = new FileStream(str, FileMode.Create);
                    int iLen = (int)struAlarmInfoV30.struPicInfo[i].dwDataLen;
                    byte[] by = new byte[iLen];
                    Marshal.Copy(struAlarmInfoV30.struPicInfo[i].pBuffer, by, 0, iLen);
                    fs.Write(by, 0, iLen);
                    fs.Close();
                }
            }

        }

        private void btnJpegCapture_Click(object sender, EventArgs e)
        {
            CHCNetSDK.NET_DVR_SNAPCFG struSnapCfg = new CHCNetSDK.NET_DVR_SNAPCFG();
            struSnapCfg.wIntervalTime = new ushort[4];
            struSnapCfg.dwSize = (uint)Marshal.SizeOf(struSnapCfg);
            struSnapCfg.byRelatedDriveWay = 1;
            struSnapCfg.bySnapTimes = 1;
            struSnapCfg.wSnapWaitTime = 100;
            struSnapCfg.wIntervalTime[0] = 100;  

            bool bManualSnap = CHCNetSDK.NET_DVR_ContinuousShoot(m_lUserID, ref struSnapCfg);
 
            if (!bManualSnap)
            {
                uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                string str = "NET_DVR_ContinuousShoot failed, error code= " + iLastErr;
                DebugInfo(str);
                return;
            }
        }



        public delegate void MyDebugInfo(string str);
        public void DebugInfo(string str)
        {
            if (str.Length > 0)
            {
                str += "\n";
                TextBoxInfo.AppendText(str);
            }

        }

        private void btnFortify_Click(object sender, EventArgs e)
        {
            if (m_bGuard)
            {
                if (m_lFortifyHandle > -1)
                {
                    if (CHCNetSDK.NET_DVR_CloseAlarmChan_V30(m_lFortifyHandle))
                    {
                        DebugInfo("NET_DVR_CloseAlarmChan_V30 Succ");
                        btnFortify.Text = "Alarm Guard";
                        m_bGuard = !m_bGuard;
                    }
                    else
                    {
                        uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        string str = "NET_DVR_CloseAlarmChan_V30 failed, error code= " + iLastErr;
                        DebugInfo(str);
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
                    btnFortify.Text = "Unguard";
                    m_bGuard = !m_bGuard;
                    DebugInfo("NET_DVR_SetupAlarmChan_V30 Succeed");
                }
                else
                {
                    uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    string str = "NET_DVR_SetupAlarmChan_V30 failed, error code= " + iLastErr;
                    DebugInfo(str);
                }


                m_falarmData = new CHCNetSDK.MSGCallBack(MsgCallback);
                if (CHCNetSDK.NET_DVR_SetDVRMessageCallBack_V30(m_falarmData, IntPtr.Zero))
                {
                    DebugInfo("NET_DVR_SetDVRMessageCallBack_V30 Succeed");
                }
                else
                {
                    uint i = CHCNetSDK.NET_DVR_GetLastError();
                    DebugInfo("NET_DVR_SetDVRMessageCallBack_V30 Failed");
                }
            }
        }

        private void btnOpenSound_Click(object sender, EventArgs e)
        {
            //   private Int32 m_iPreviewType = 0;
            if (m_lRealHandle < 0)
            {
                MessageBox.Show("Please start preview at first");
            }
            else
            {
                if (!m_bOpenSound)
                {
                        if (CHCNetSDK.NET_DVR_OpenSound(m_lRealHandle))
                        {
                            m_bOpenSound = true;
                            btnOpenSound.Text = "Stop sound";
                        }
                        else
                        {
                            uint i = CHCNetSDK.NET_DVR_GetLastError();
                            DebugInfo("NET_DVR_OpenSound Failed");
                        }
                }
                else
                {
                        if (CHCNetSDK.NET_DVR_CloseSound())
                        {
                            m_bOpenSound = false;
                            btnOpenSound.Text = "Open sound";
                        }
                        else
                        {
                            uint i = CHCNetSDK.NET_DVR_GetLastError();
                            DebugInfo("NET_DVR_CloseSound Failed");
                        }
                }
            }
        }

       
        private void Logout_Click(object sender, EventArgs e)
        {

            logoutRtn = CHCNetSDK.NET_DVR_Logout_V30(m_lUserID);
            Console.WriteLine("The error number is:" + CHCNetSDK.NET_DVR_GetLastError());

            if (logoutRtn)
                  MessageBox.Show("Successful to logout the current device.");

        }

        private void textBoxIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnStopPre_Click(object sender, EventArgs e)
        {
            bool bStopReal = CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
            RealPlayWnd.Refresh();
            if (!bStopReal)
            {
                uint nError = CHCNetSDK.NET_DVR_GetLastError();
                DebugInfo("NET_DVR_StopRealPlay fail!");
                return;
            }
        }

        private void Btn_CANCEL_Click(object sender, EventArgs e)
        {
            if (!logoutRtn)
            {
                MessageBox.Show("Please logout the current device firstly.");
                return; 
            }

            bool cleanupRtn = CHCNetSDK.NET_DVR_Cleanup(); 
            this.Close();
        }

        private void SetConfig_Click(object sender, EventArgs e)
        {
            Int32 dwInBufferSize = Marshal.SizeOf(m_struOverCond);
            Int32 dwOutBufferSize = Marshal.SizeOf(m_struOverCfg);

            m_struOverCond.dwSize = (uint)dwInBufferSize;
            m_struOverCond.dwChannel = 1;
            m_struOverCond.dwConfigMode = 1;

            IntPtr ptrOverCond = Marshal.AllocHGlobal(dwInBufferSize);
            Marshal.StructureToPtr(m_struOverCond, ptrOverCond, false);

            IntPtr ptrOverCfg = Marshal.AllocHGlobal(dwOutBufferSize);
            Marshal.StructureToPtr(m_struOverCfg, ptrOverCfg, false);

            UInt32 dwStatusList = 0;
            IntPtr lpStatusList = Marshal.AllocHGlobal(4);
            Marshal.StructureToPtr(dwStatusList, lpStatusList, false);

            if (!CHCNetSDK.NET_DVR_GetDeviceConfig(m_lUserID, CHCNetSDK.NET_ITS_GET_OVERLAP_CFG, 1, ptrOverCond, (UInt32)dwInBufferSize, lpStatusList, ptrOverCfg, (UInt32)dwOutBufferSize))
            {
                uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                string str = "NET_ITS_GET_OVERLAP_CFG failed, error code= " + iLastErr;
                DebugInfo(str);
            }
            else
            {
                m_struOverCfg = (CHCNetSDK.NET_ITS_OVERLAP_CFG)Marshal.PtrToStructure(ptrOverCfg, typeof(CHCNetSDK.NET_ITS_OVERLAP_CFG));
                m_struOverCfg.byEnable = 1;
                m_struOverCfg.struOverLapItem.wStartPosTop = 50;
                m_struOverCfg.struOverLapItem.wStartPosLeft = 50;
                m_struOverCfg.struOverLapItem.struSingleItem[6].byItemType = 18;
                m_struOverCfg.struOverLapItem.struSingleItem[6].bySpaceNum = 4;
                m_struOverCfg.struOverLapItem.struSingleItem[6].byChangeLineNum = 3;

                byte[] byName = System.Text.Encoding.Default.GetBytes("测试叠加：监测点1信息");
                byName.CopyTo(m_struOverCfg.struOverLapInfo.byMonitoringSite1, 0);

                Marshal.StructureToPtr(m_struOverCfg, ptrOverCfg, false);

                if (!CHCNetSDK.NET_DVR_SetDeviceConfig(m_lUserID, CHCNetSDK.NET_ITS_SET_OVERLAP_CFG, 1, ptrOverCond, (UInt32)dwInBufferSize, lpStatusList, ptrOverCfg, (UInt32)dwOutBufferSize))
                {
                    uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    string str = "NET_ITS_SET_OVERLAP_CFG failed, error code= " + iLastErr;
                    DebugInfo(str);
                }
                else
                {
                    DebugInfo("NET_ITS_SET_OVERLAP_CFG succ");
                }
            }
            Marshal.FreeHGlobal(ptrOverCond);
            Marshal.FreeHGlobal(ptrOverCfg);
            Marshal.FreeHGlobal(lpStatusList);
            return;
        }

        private void btnControl_Click(object sender, EventArgs e)
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
                DebugInfo(str);                
            }
            Marshal.FreeHGlobal(ptrControlCfg);
            return;
        }

        private void btnEntranceCfg_Click(object sender, EventArgs e)
        {
            CHCNetSDK.NET_DVR_BARRIERGATE_COND m_struEntranceCond = new CHCNetSDK.NET_DVR_BARRIERGATE_COND();
            CHCNetSDK.NET_DVR_ENTRANCE_CFG m_struEntranceCfg = new CHCNetSDK.NET_DVR_ENTRANCE_CFG();

            Int32 dwInBufferSize = Marshal.SizeOf(m_struEntranceCond);
            Int32 dwOutBufferSize = Marshal.SizeOf(m_struEntranceCfg);

            m_struEntranceCond.byLaneNo = 1;

            IntPtr ptrEntranceCond = Marshal.AllocHGlobal(dwInBufferSize);
            Marshal.StructureToPtr(m_struEntranceCond, ptrEntranceCond, false);

            IntPtr ptrEntranceCfg = Marshal.AllocHGlobal(dwOutBufferSize);
            Marshal.StructureToPtr(m_struEntranceCfg, ptrEntranceCfg, false);

            UInt32 dwStatusList = 0;
            IntPtr lpStatusList = Marshal.AllocHGlobal(4);
            Marshal.StructureToPtr(dwStatusList, lpStatusList, false);

            if (!CHCNetSDK.NET_DVR_GetDeviceConfig(m_lUserID, CHCNetSDK.NET_DVR_GET_ENTRANCE_PARAMCFG, 1, ptrEntranceCond, (UInt32)dwInBufferSize, lpStatusList, ptrEntranceCfg, (UInt32)dwOutBufferSize))
            {
                uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                string str = "NET_DVR_GET_ENTRANCE_PARAMCFG failed, error code= " + iLastErr;
                DebugInfo(str);
            }
            else
            {
                m_struEntranceCfg = (CHCNetSDK.NET_DVR_ENTRANCE_CFG)Marshal.PtrToStructure(ptrEntranceCfg, typeof(CHCNetSDK.NET_DVR_ENTRANCE_CFG));

                m_struEntranceCfg.byBarrierGateCtrlMode = 1;
                Marshal.StructureToPtr(m_struEntranceCfg, ptrEntranceCfg, false);

                if (!CHCNetSDK.NET_DVR_SetDeviceConfig(m_lUserID, CHCNetSDK.NET_DVR_SET_ENTRANCE_PARAMCFG, 1, ptrEntranceCond, (UInt32)dwInBufferSize, lpStatusList, ptrEntranceCfg, (UInt32)dwOutBufferSize))
                {
                    uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    string str = "NET_DVR_SET_ENTRANCE_PARAMCFG failed, error code= " + iLastErr;
                    DebugInfo(str);
                }

                DebugInfo("NET_DVR_SET_ENTRANCE_PARAMCFG succ");
            }
            Marshal.FreeHGlobal(ptrEntranceCond);
            Marshal.FreeHGlobal(ptrEntranceCfg);
            Marshal.FreeHGlobal(lpStatusList);
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            CHCNetSDK.NET_DVR_PLATE_RESULT struPlateResultInfo = new CHCNetSDK.NET_DVR_PLATE_RESULT();
            struPlateResultInfo.pBuffer1 = Marshal.AllocHGlobal(2 * 1024 * 1024);
            struPlateResultInfo.pBuffer2 = Marshal.AllocHGlobal(1024 * 1024);

            CHCNetSDK.NET_DVR_MANUALSNAP struInter = new CHCNetSDK.NET_DVR_MANUALSNAP();

            if (!CHCNetSDK.NET_DVR_ManualSnap(m_lUserID, ref struInter, ref struPlateResultInfo))
            {
                uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                string str = "NET_DVR_ManualSnap failed, error code= " + iLastErr;
                DebugInfo(str);
            }
            else
            {
                DebugInfo("NET_DVR_ManualSnap succ");
                int dwPicLen = (int)struPlateResultInfo.dwPicLen;
                int dwPicPlateLen = (int)struPlateResultInfo.dwPicPlateLen;

                if (dwPicLen > 0)
                {
                    FileStream fs = new FileStream("近景图.jpg", FileMode.Create);
                    byte[] by = new byte[dwPicLen];
                    Marshal.Copy(struPlateResultInfo.pBuffer1, by, 0, dwPicLen);
                    fs.Write(by, 0, dwPicLen);
                    fs.Close();
                }
                if (dwPicPlateLen > 0)
                {
                    FileStream fs = new FileStream("车牌图.jpg", FileMode.Create);
                    byte[] by = new byte[dwPicPlateLen];
                    Marshal.Copy(struPlateResultInfo.pBuffer2, by, 0, dwPicPlateLen);
                    fs.Write(by, 0, dwPicPlateLen);
                    fs.Close();
                }
            }
        }

        private void btnTriggerCfg_Click(object sender, EventArgs e)
        {
            Int32 dwOutBufferSize = 0;
            UInt32 dwBytesReturned = 0;
            uint dwUnionSize = (uint)Marshal.SizeOf(struTriggerCfg.struTriggerParam.uTriggerParam);;

            dwOutBufferSize = Marshal.SizeOf(struCurrentMode);
            IntPtr ptrCurrentMode = Marshal.AllocHGlobal(dwOutBufferSize);
            Marshal.StructureToPtr(struCurrentMode, ptrCurrentMode, false);
            if (!CHCNetSDK.NET_DVR_GetDVRConfig(m_lUserID, CHCNetSDK.NET_DVR_GET_CURTRIGGERMODE, -1, ptrCurrentMode, (UInt32)dwOutBufferSize, ref dwBytesReturned))
            {
                uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                string str = "NET_DVR_GET_CURTRIGGERMODE failed, error code= " + iLastErr;
                DebugInfo(str);
                return;
            }
            else 
            {
               
                DebugInfo("NET_DVR_GET_CURTRIGGERMODE succ");
                struCurrentMode = (CHCNetSDK.NET_DVR_CURTRIGGERMODE)Marshal.PtrToStructure(ptrCurrentMode, typeof(CHCNetSDK.NET_DVR_CURTRIGGERMODE));
            }

            int lChannel = (int)struCurrentMode.dwTriggerType; //当前启用的触发模式
            DebugInfo("Current trigger type = " + lChannel);


            dwOutBufferSize = Marshal.SizeOf(struTriggerCfg);
            IntPtr ptrTriggerCfg = Marshal.AllocHGlobal(dwOutBufferSize);
            Marshal.StructureToPtr(struTriggerCfg, ptrTriggerCfg, false);           
            
            if (!CHCNetSDK.NET_DVR_GetDVRConfig(m_lUserID, CHCNetSDK.NET_ITC_GET_TRIGGERCFG, lChannel, ptrTriggerCfg, (UInt32)dwOutBufferSize, ref dwBytesReturned))
            {
                uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                string str = "NET_ITC_GET_TRIGGERCFG failed, error code= " + iLastErr;
                DebugInfo(str);
                return;
            }
            else
            {
                struTriggerCfg = (CHCNetSDK.NET_ITC_TRIGGERCFG)Marshal.PtrToStructure(ptrTriggerCfg, typeof(CHCNetSDK.NET_ITC_TRIGGERCFG));

                IntPtr ptrTriggerUnion = Marshal.AllocHGlobal((Int32)dwUnionSize);
                Marshal.StructureToPtr(struTriggerCfg.struTriggerParam.uTriggerParam, ptrTriggerUnion, false);

                if (struTriggerCfg.struTriggerParam.dwTriggerType == 0x8) //ITC_POST_RS485_RADAR_TYPE： RS485雷达触发（卡口）
                { 
                    stru485RadarParam = (CHCNetSDK.NET_ITC_POST_RS485_RADAR_PARAM)Marshal.PtrToStructure(ptrTriggerUnion, typeof(CHCNetSDK.NET_ITC_POST_RS485_RADAR_PARAM));

                    string str = "省份简称, byDefaultCHN:" + stru485RadarParam.struPlateRecog.byDefaultCHN + ", byProvince:" + stru485RadarParam.struPlateRecog.byProvince; //省份简称
                    DebugInfo(str);
                    
                }

                if (struTriggerCfg.struTriggerParam.dwTriggerType == 0x10000) //ITC_PE_RS485_TYPE： RS485车检器卡式电警触发（卡式电警）
                {
                    struPERs485 = (CHCNetSDK.NET_ITC_EPOLICE_RS485_PARAM)Marshal.PtrToStructure(ptrTriggerUnion, typeof(CHCNetSDK.NET_ITC_EPOLICE_RS485_PARAM));

                    string str = "省份简称, byDefaultCHN:" + struPERs485.struPlateRecog.byDefaultCHN + ", byProvince:" + struPERs485.struPlateRecog.byProvince; //省份简称
                    DebugInfo(str);                 
                }

                if (struTriggerCfg.struTriggerParam.dwTriggerType == 0x2) //ITC_POST_SINGLEIO_TYPE： 单IO触发（卡口）
                {
                    struSingleIO = (CHCNetSDK.NET_ITC_POST_SINGLEIO_PARAM)Marshal.PtrToStructure(ptrTriggerUnion, typeof(CHCNetSDK.NET_ITC_POST_SINGLEIO_PARAM));

                    string str = "省份简称, byDefaultCHN:" + struSingleIO.struPlateRecog.byDefaultCHN + ", byProvince:" + struSingleIO.struPlateRecog.byProvince; //省份简称
                    DebugInfo(str);
                }

                if (struTriggerCfg.struTriggerParam.dwTriggerType == 0x20) //ITC_POST_HVT_TYPE_V50： 混行卡口视频触发V50（卡口） 
                {
                    struHvtParam = (CHCNetSDK.NET_ITC_POST_HVT_PARAM_V50)Marshal.PtrToStructure(ptrTriggerUnion, typeof(CHCNetSDK.NET_ITC_POST_HVT_PARAM_V50));

                    string str = "省份简称, byDefaultCHN:" + struHvtParam.struPlateRecog.byDefaultCHN + ", byProvince:" + struHvtParam.struPlateRecog.byProvince; //省份简称
                    DebugInfo(str);

                    str = "车道1, 车道类型：" + struHvtParam.struLaneParam[0].byLaneType + ", 小车标志限高速：" + struHvtParam.struLaneParam[0].bySpeedLimit
                        + ", 小车限高速值：" + struHvtParam.struLaneParam[0].bySignLowSpeed + ", 小车标志限低速：" + struHvtParam.struLaneParam[0].byLowSpeedLimit
                        + ", 小车限低速值：" + struHvtParam.struLaneParam[0].bySignSpeed + ", 大车标志限高速：" + struHvtParam.struLaneParam[0].byBigCarSignSpeed
                        + ", 大车限高速值：" + struHvtParam.struLaneParam[0].byBigCarSpeedLimit + ", 大车标志限低速：" + struHvtParam.struLaneParam[0].byBigCarSignLowSpeed
                        + ", 大车限低速值：" + struHvtParam.struLaneParam[0].byBigCarLowSpeedLimit;
                    DebugInfo(str);
                }

                Marshal.FreeHGlobal(ptrTriggerUnion);
            }

            //设置参数
            struTriggerCfg.struTriggerParam.byEnable = 1;

            IntPtr ptrTriggerCfgUnion = Marshal.AllocHGlobal((Int32)dwUnionSize);
            Marshal.StructureToPtr(struTriggerCfg.struTriggerParam.uTriggerParam, ptrTriggerCfgUnion, false);

            if (lChannel == 0x8)
            {
                struTriggerCfg.struTriggerParam.dwTriggerType = 0x8;

                //卡口RS485雷达触发参数赋值
                //stru485RadarParam.struLane[0].byCartSignSpeed = 80;  //大车标志限速限速值
                //......

                //stru485RadarParam.struPlateRecog.byDefaultCHN = "浙"; //省份简称
                //stru485RadarParam.struPlateRecog.byProvince = 33;

                Marshal.StructureToPtr(stru485RadarParam, ptrTriggerCfgUnion, false);
            }
            if (lChannel == 0x10000)
            {
                //struPERs485.struLane[0].byBigCarSignSpeed = 60; //大车标志限速值
                //......

                //struPERs485.struPlateRecog.byDefaultCHN = "浙"; //省份简称
                //struPERs485.struPlateRecog.byProvince = 33;

                Marshal.StructureToPtr(struPERs485, ptrTriggerCfgUnion, false);
            }

            if (lChannel == 0x2)
            {
                //struSingleIO.struSingleIO[0].byDefaultStatus = 0; //IO触发默认状态：0- 低电平，1- 高电平 
                //......

                //struSingleIO.struPlateRecog.byDefaultCHN = "浙"; //省份简称
                //struSingleIO.struPlateRecog.byProvince = 33;

                Marshal.StructureToPtr(struSingleIO, ptrTriggerCfgUnion, false);
            }

            if (lChannel == 0x20)
            {
                //struSingleIO.struSingleIO[0].byDefaultStatus = 0; //IO触发默认状态：0- 低电平，1- 高电平 
                //......

                //struSingleIO.struPlateRecog.byDefaultCHN = "浙"; //省份简称
                //struSingleIO.struPlateRecog.byProvince = 33;
                struHvtParam.struLaneParam[0].byBigCarSignSpeed = 70;//大车标志限高速，设置的参数需要设备支持
                //违章检测类型中超速或者低速检测类型开启之后才能配置限速值

                Marshal.StructureToPtr(struHvtParam, ptrTriggerCfgUnion, false);
            }

            struTriggerCfg.struTriggerParam.uTriggerParam = (CHCNetSDK.NET_ITC_TRIGGER_PARAM_UNION)Marshal.PtrToStructure(ptrTriggerCfgUnion, typeof(CHCNetSDK.NET_ITC_TRIGGER_PARAM_UNION));         
            Marshal.StructureToPtr(struTriggerCfg, ptrTriggerCfg, false);   

            if (!CHCNetSDK.NET_DVR_SetDVRConfig(m_lUserID, CHCNetSDK.NET_ITC_SET_TRIGGERCFG, lChannel, ptrTriggerCfg, (UInt32)dwOutBufferSize))
            {
                uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                string str = "NET_ITC_SET_TRIGGERCFG failed, error code= " + iLastErr;
                DebugInfo(str);
            }
            else
            {
                DebugInfo("NET_ITC_SET_TRIGGERCFG succ");
            }

            Marshal.FreeHGlobal(ptrTriggerCfgUnion);
            Marshal.FreeHGlobal(ptrTriggerCfg);
            return;
        }
    }
}

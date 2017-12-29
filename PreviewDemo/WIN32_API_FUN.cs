using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace Flash_Packing_Client
{
  class WIN32_API_FUN
    {
        private static string peizhi_wenjian_lujing="";

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(
                                   string section, string key, string def,
                                   StringBuilder retVal, int size, string filePath);
       
       public static string Get_DeviceCfg(string ziduan1, string ziduan2)
        {
            string str = System.Environment.CurrentDirectory;
           // str = str.Replace("\\","\\\\");
            peizhi_wenjian_lujing = str + "\\ConfigCfg.txt";
           // MessageBox.Show(peizhi_wenjian_lujing);
            StringBuilder buf = new StringBuilder();
            GetPrivateProfileString(ziduan1,ziduan2, "none", buf, 64, str + "\\ConfigCfg.txt");
            string return_str;
            return_str = buf.ToString();
            if (return_str == "none")
                return "none";
            else
                return  return_str.Replace(";", "");
         }

        public static long Set_DeviceCfg(string ziduan1,string ziduan2,string value_str)
        {
            string str;
            str = peizhi_wenjian_lujing;//.Replace("\\", "\\\\");
            if (File.Exists(str))
            {
            }
            else
            {
                //MessageBox.Show(peizhi_wenjian_lujing);
                FileStream fs = new FileStream(str, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.WriteLine("[xiangji_xinxi_1]");
                sw.WriteLine("damen_mingcheng =none");
                sw.WriteLine("zhaji_mingcheng =none");
                sw.WriteLine("zhaji_xingzhi =none");
                sw.WriteLine("xiangji_IP =none");
                sw.WriteLine("xiangji_pinpai =none");
                sw.WriteLine("\n");

                sw.WriteLine("[xiangji_xinxi_2]");
                sw.WriteLine("damen_mingcheng =none");
                sw.WriteLine("zhaji_mingcheng =none");
                sw.WriteLine("zhaji_xingzhi =none");
                sw.WriteLine("xiangji_IP =none");
                sw.WriteLine("xiangji_pinpai =none");
                sw.WriteLine("\n");

                sw.WriteLine("[xiangji_xinxi_3]");
                sw.WriteLine("damen_mingcheng =none");
                sw.WriteLine("zhaji_mingcheng =none");
                sw.WriteLine("zhaji_xingzhi =none");
                sw.WriteLine("xiangji_IP =none");
                sw.WriteLine("xiangji_pinpai =none");
                sw.WriteLine("\n");

                sw.WriteLine("[xiangji_xinxi_4]");
                sw.WriteLine("damen_mingcheng =none");
                sw.WriteLine("zhaji_mingcheng =none");
                sw.WriteLine("zhaji_xingzhi =none");
                sw.WriteLine("xiangji_IP =none");
                sw.WriteLine("xiangji_pinpai =none");
                sw.WriteLine("\n");

                sw.WriteLine("[shujuku_xinxi]");
                sw.WriteLine("server =none");
                sw.WriteLine(" database =none");
                sw.WriteLine("port =none");
                sw.WriteLine("uid =none");
                sw.WriteLine("password =none");
                sw.WriteLine("\n");

                sw.WriteLine("[banben_xinxi]");
                sw.WriteLine("shifou_wanzhengban =none");
                sw.WriteLine("\n");
                sw.Close();
                fs.Close();
            }
            value_str = value_str + ";";
            return  WritePrivateProfileString(ziduan1, ziduan2, value_str, str);
        }

        internal static void SendMessage(IntPtr handle, int tVM_SETITEM, IntPtr zero, ref TVITEM tvi)
        {
            throw new NotImplementedException();
        }

        public static void write_logs_file(string logs_str)
        {
            //string str = Environment.CurrentDirectory;
            string str = Application.StartupPath + "\\logs";
            if (!Directory.Exists(str))
            {
                Directory.CreateDirectory(str);
            }
            str = str + "\\"+ DateTime.Now.ToShortDateString().Replace("/","")+ "_log.txt";
            //if (File.Exists(str))
            //{
            //}
            //else
            //{
                //MessageBox.Show(peizhi_wenjian_lujing);
                FileStream fs = new FileStream(str, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs,Encoding.UTF8);
                sw.WriteLine(logs_str);
                sw.WriteLine(" ");          
                sw.Close();
                fs.Close();
           // }
        }
        public static string Get_CONN_STR()
        {
            string database;
            string port;
            string server;
            string uid;
            string password;

            database = Get_DeviceCfg("shujuku_xinxi", "database");
            port = Get_DeviceCfg("shujuku_xinxi", "port");
            server = Get_DeviceCfg("shujuku_xinxi", "server");
            uid = Get_DeviceCfg("shujuku_xinxi", "uid");
            password = Get_DeviceCfg("shujuku_xinxi", "password");
            return
                "database =" + database
                + ";port =" + port 
                + ";server = " + server
                + "; uid = " + uid
                + "; password = " + password;
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(
                                     string section, string key,
                                     string val, string filePath);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

        /// 自定义的结构
        /// </summary>
        public struct My_lParam
        {
            public int i;
            public string s;
        }


        /// <summary>
        /// 使用COPYDATASTRUCT来传递字符串
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }
        //treeviewlist  && checkbox 应用实例
        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        public struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)]
            public string lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage; public int cChildren; public IntPtr lParam;
        }
       // [DllImport("user32.dll", CharSet = CharSet.Auto)]
        //private static extern IntPtr SendMessage(
        //                    IntPtr hWnd, 
        //                    int Msg, 
        //                    IntPtr wParam, 
        //                    ref TVITEM lParam
        //    );
        //treeviewlist  && checkbox 应用实例

        //消息发送API
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
            int wParam,         // 参数1
            int lParam          //参数2
        );


        //消息发送API
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
            int wParam,         // 参数1
            ref My_lParam lParam //参数2
        );

        //消息发送API
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
            int wParam,         // 参数1
            ref COPYDATASTRUCT lParam  //参数2
        );

        //消息发送API
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
            int wParam,         // 参数1
            int lParam            // 参数2
        );



        //消息发送API
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
           int Msg,            // 消息ID
            int wParam,         // 参数1
            ref My_lParam lParam //参数2
        );

        //异步消息发送API
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
           int Msg,            // 消息ID
            int wParam,         // 参数1
            ref COPYDATASTRUCT lParam  // 参数2
        );
    
        public static string jisuan_shichang(DateTime xian_time,DateTime hou_time)
        {
            TimeSpan span = new TimeSpan();
            span = hou_time - xian_time;
            Double shi_chang = span.TotalMinutes;
            int nian;
                   nian =Convert.ToInt32(shi_chang / 518400);//每年有518400分钟
            int yue;
                   yue = Convert.ToInt32(shi_chang - nian * 518400) / 43200;//每月有43200分钟
            int ri;
                   ri = Convert.ToInt32(shi_chang - nian * 518400 - yue * 43200) / 1400;//每天有1440分钟
            int shi;
                   shi = Convert.ToInt32(shi_chang - nian * 518400 - yue * 43200 - ri * 1440) / 60;
            int fen;
                   fen = Convert.ToInt32(shi_chang - nian * 518400 - yue * 43200 - ri * 1440 - shi * 60);

            string shichang_str="";

            if ((nian > 0) & (yue > 0) & (ri > 0) & (shi > 0) & (fen > 0))
                shichang_str = nian.ToString() + "年" + yue.ToString() + "月" + ri.ToString() + "日" + shi.ToString() + "时" + fen.ToString() + "分";
            else if ((nian > 0) & (yue > 0) & (ri > 0) & (shi > 0) & (fen == 0))
                shichang_str = nian.ToString() + "年" + yue.ToString() + "月" + ri.ToString() + "日" + shi.ToString() + "时";
            else if ((nian > 0) & (yue > 0) & (ri > 0) & (shi == 0) & (fen > 0))
                shichang_str = nian.ToString() + "年" + yue.ToString() + "月" + ri.ToString() + "日" + " 零 " + fen.ToString() + "分";
            else if ((nian > 0) & (yue > 0) & (ri > 0) & (shi == 0) & (fen == 0))
                shichang_str = nian.ToString() + "年" + yue.ToString() + "月" + ri.ToString() + "日";
            else if ((nian > 0) & (yue > 0) & (ri == 0) & (shi > 0) & (fen > 0))
                shichang_str = nian.ToString() + "年" + yue.ToString() + "月" + " 零 " + shi.ToString() + "时" + fen.ToString() + "分";
            else if ((nian > 0) & (yue > 0) & (ri == 0) & (shi > 0) & (fen == 0))
                shichang_str = nian.ToString() + "年" + yue.ToString() + "月" + " 零 " + shi.ToString() + "时";
            else if ((nian > 0) & (yue > 0) & (ri == 0) & (shi == 0) & (fen > 0))
                shichang_str = nian.ToString() + "年" + yue.ToString() + "月" + " 零 " + fen.ToString() + "分";
            else if ((nian > 0) & (yue > 0) & (ri == 0) & (shi == 0) & (fen == 0))
                shichang_str = nian.ToString() + "年" + yue.ToString() + "月";
            else if ((nian > 0) & (yue == 0) & (ri > 0) & (shi > 0) & (fen > 0))
                shichang_str = nian.ToString() + "年" + " 零 " + ri.ToString() + "日" + shi.ToString() + "时" + fen.ToString() + "分";
            else if ((nian > 0) & (yue == 0) & (ri > 0) & (shi > 0) & (fen == 0))
                shichang_str = nian.ToString() + "年" + " 零 " + ri.ToString() + "日" + shi.ToString() + "时";
            else if ((nian > 0) & (yue == 0) & (ri > 0) & (shi == 0) & (fen > 0))
                shichang_str = nian.ToString() + "年" + " 零 " + ri.ToString() + "日" + " 零 " + fen.ToString() + "分";
            else if ((nian > 0) & (yue == 0) & (ri > 0) & (shi == 0) & (fen == 0))
                shichang_str = nian.ToString() + "年" + " 零 " + ri.ToString() + "日";
            else if ((nian > 0) & (yue == 0) & (ri == 0) & (shi > 0) & (fen > 0))
                shichang_str = nian.ToString() + "年" + " 零 " + shi.ToString() + "时" + fen.ToString() + "分";
            else if ((nian > 0) & (yue == 0) & (ri == 0) & (shi > 0) & (fen == 0))
                shichang_str = nian.ToString() + "年" + " 零 " + shi.ToString() + "时";
            else if ((nian > 0) & (yue == 0) & (ri == 0) & (shi == 0) & (fen > 0))
                shichang_str = nian.ToString() + "年" + " 零 " + fen.ToString() + "分";
            else if ((nian > 0) & (yue == 0) & (ri == 0) & (shi == 0) & (fen == 0))
                shichang_str = nian.ToString() + "年";

            else if ((nian == 0) & (yue > 0) & (ri > 0) & (shi > 0) & (fen > 0))
                shichang_str =  yue.ToString() + "月" + ri.ToString() + "日" + shi.ToString() + "时" + fen.ToString() + "分";
            else if ((nian== 0) & (yue > 0) & (ri > 0) & (shi > 0) & (fen == 0))
                shichang_str = yue.ToString() + "月" + ri.ToString() + "日" + shi.ToString() + "时";
            else if ((nian== 0) & (yue > 0) & (ri > 0) & (shi == 0) & (fen > 0))
                shichang_str = yue.ToString() + "月" + ri.ToString() + "日" + " 零 " + fen.ToString() + "分";
            else if ((nian== 0) & (yue > 0) & (ri > 0) & (shi == 0) & (fen == 0))
                shichang_str = yue.ToString() + "月" + ri.ToString() + "日";
            else if ((nian == 0) & (yue > 0) & (ri == 0) & (shi > 0) & (fen > 0))
                shichang_str = yue.ToString() + "月" + " 零 " + shi.ToString() + "时" + fen.ToString() + "分";
            else if ((nian == 0) & (yue > 0) & (ri == 0) & (shi > 0) & (fen == 0))
                shichang_str =  yue.ToString() + "月" + " 零 " + shi.ToString() + "时";
            else if ((nian== 0) & (yue > 0) & (ri == 0) & (shi == 0) & (fen > 0))
                shichang_str =  yue.ToString() + "月" + " 零 " + fen.ToString() + "分";
            else if ((nian == 0) & (yue > 0) & (ri == 0) & (shi == 0) & (fen == 0))
                shichang_str = yue.ToString() + "月";
            else if ((nian == 0) & (yue == 0) & (ri > 0) & (shi > 0) & (fen > 0))
                shichang_str =ri.ToString() + "日" + shi.ToString() + "时" + fen.ToString() + "分";
            else if ((nian == 0) & (yue == 0) & (ri > 0) & (shi > 0) & (fen == 0))
                shichang_str =ri.ToString() + "日" + shi.ToString() + "时";
            else if ((nian == 0) & (yue == 0) & (ri > 0) & (shi == 0) & (fen > 0))
                shichang_str = ri.ToString() + "日" + " 零 " + fen.ToString() + "分";
            else if ((nian == 0) & (yue == 0) & (ri > 0) & (shi == 0) & (fen == 0))
                shichang_str = ri.ToString() + "日";
            else if ((nian == 0) & (yue == 0) & (ri == 0) & (shi > 0) & (fen > 0))
                shichang_str =  shi.ToString() + "时" + fen.ToString() + "分";
            else if ((nian ==0) & (yue == 0) & (ri == 0) & (shi > 0) & (fen == 0))
                shichang_str =  shi.ToString() + "时";
            else if ((nian == 0) & (yue == 0) & (ri == 0) & (shi == 0) & (fen > 0))
                shichang_str = fen.ToString() + "分";
            else //if ((nian ==0) & (yue == 0) & (ri == 0) & (shi == 0) & (fen == 0))
                shichang_str ="0分";

            return shichang_str;
        }

       [DllImport("user32.dll")]
        public static extern int MessageBeep(uint uType);//蜂鸣器
       

   public static string IntToIp(uint ipInt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((ipInt >> 24) & 0xFF).Append(".");
            sb.Append((ipInt >> 16) & 0xFF).Append(".");
            sb.Append((ipInt >> 8) & 0xFF).Append(".");
            sb.Append(ipInt & 0xFF);
            return sb.ToString();
        }
    public static uint Reverse_uint(uint uiNum)
        {
            return ((uiNum & 0x000000FF) << 24) |
                   ((uiNum & 0x0000FF00) << 8) |
                   ((uiNum & 0x00FF0000) >> 8) |
                   ((uiNum & 0xFF000000) >> 24);
        }

        public string Unicode_to_utf8(string old_string)
        {

            Encoding utf8 = Encoding.UTF8;
            Encoding defaultCode = Encoding.Unicode;

            // Convert the string into a byte[].
            byte[] utf8Bytes = utf8.GetBytes(old_string);
            // Perform the conversion from one encoding to the other.
            byte[] defaultBytes = Encoding.Convert(utf8, defaultCode, utf8Bytes);
            char[] defaultChars = new char[defaultCode.GetCharCount(defaultBytes, 0, defaultBytes.Length)];
            defaultCode.GetChars(defaultBytes, 0, defaultBytes.Length, defaultChars, 0);
            string defaultString = new string(defaultChars);

            return defaultString;
        }

        private static string new_excel()
        {
            SaveFileDialog excel_name = new SaveFileDialog();
            excel_name.Title = "保存的excel文件";
            excel_name.InitialDirectory = "c:\\";
            excel_name.Filter = "Excel 97-2003 (*.xls)|*.xls|All Files (*.*)|*.*";
            excel_name.ShowDialog();
            if (excel_name.FileName == "" || excel_name.FileName == null)
            {
                MessageBox.Show("文件名不能为空!");
                return"";
            }
           return excel_name.FileName;
        }

        private static void xieru_datagridview_shuju(DataGridView dv,string path)
        {
            StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding("gb2312"));
            StringBuilder sb = new StringBuilder();
            try
            {
                long totalCount = dv.Rows.Count;              
                for (int k = 0; k < dv.Columns.Count; k++)
                {
                    sb.Append(dv.Columns[k].HeaderText + "\t");
                }
                sb.Append(Environment.NewLine);

                for (int i = 0; i < dv.Rows.Count; i++)
                {
                    for (int j = 0; j < dv.Columns.Count; j++)
                    {
                        sb.Append(Convert.ToString(dv.Rows[i].Cells[j].Value)=="" ? "" + "\t":dv.Rows[i].Cells[j].Value.ToString() + "\t");   
                    }
                    sb.Append(Environment.NewLine);
                }
                sw.Write(sb.ToString());
                sw.Flush();
                sw.Close();
                MessageBox.Show("已经生成指定Excel文件!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                sw.Close();
            }
        }

    private static void xieru_chaxun_tiaojian(string path,string chaxun_tiaojian,DataGridView dv)
    {
            StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding("gb2312"));
            StringBuilder sb = new StringBuilder();
            try
            {
                    string zhongzhuan = "";
                    int index = 0;
                    index = chaxun_tiaojian.IndexOf("where");
                    zhongzhuan = chaxun_tiaojian.Substring(0, index) + "where ";
                    chaxun_tiaojian = chaxun_tiaojian.Replace(zhongzhuan, "");

                    index = chaxun_tiaojian.IndexOf("order");
                    zhongzhuan = chaxun_tiaojian.Substring(index, chaxun_tiaojian.Length - index);
                    chaxun_tiaojian = chaxun_tiaojian.Replace(zhongzhuan, "");
                    chaxun_tiaojian = chaxun_tiaojian.Replace("'", "");
                    chaxun_tiaojian = chaxun_tiaojian + "and";

                    chaxun_tiaojian = chaxun_tiaojian.Replace("支付宝离场", "支付宝&");
                    chaxun_tiaojian = chaxun_tiaojian.Replace("微信离场", "微信&");
                    chaxun_tiaojian = chaxun_tiaojian.Replace("无记录离场", "无记录&");
                    chaxun_tiaojian = chaxun_tiaojian.Replace("离场", "现金&");

                    chaxun_tiaojian = chaxun_tiaojian.Replace("LiChangShiJian", "离场时间&");
                    chaxun_tiaojian = chaxun_tiaojian.Replace(">=", "大于等于&");
                    chaxun_tiaojian = chaxun_tiaojian.Replace("=", "等于&");
                    chaxun_tiaojian = chaxun_tiaojian.Replace("<=", "小于等于&");
                    chaxun_tiaojian = chaxun_tiaojian.Replace("<", "小于&");
                    chaxun_tiaojian = chaxun_tiaojian.Replace(">", "大于&");
                    chaxun_tiaojian = chaxun_tiaojian.Replace("like", "类似于  &");
                    chaxun_tiaojian = chaxun_tiaojian.Replace("LiChangMenWei", "值班员  &");
                    chaxun_tiaojian = chaxun_tiaojian.Replace("ShouFeiBiaoJi", "支付方式  &");
                    chaxun_tiaojian = chaxun_tiaojian.Replace("ChePaiHaoMa", "车牌号码  &");

                    chaxun_tiaojian = chaxun_tiaojian.Replace("qishi_riqi", "起始日期  &");
                    chaxun_tiaojian = chaxun_tiaojian.Replace("jieshu_riqi", "截止日期  &");

                    chaxun_tiaojian = chaxun_tiaojian.Replace("tianshu", "统计天数  &");


                do
                {
                    index = chaxun_tiaojian.IndexOf("and");
                    zhongzhuan = index == -1 ? chaxun_tiaojian:chaxun_tiaojian.Substring(0,index+3);
                    chaxun_tiaojian = chaxun_tiaojian.Replace(zhongzhuan, "");                 
                    zhongzhuan = zhongzhuan.Replace("and", "");

                    zhongzhuan = zhongzhuan + "&";
                    index = zhongzhuan.IndexOf("&");
                    sb.Append(zhongzhuan.Substring(0,index).Trim() + "\t");
                    zhongzhuan = zhongzhuan.Remove(0, index+1 );

                    index = zhongzhuan.IndexOf("&");
                    sb.Append(zhongzhuan.Substring(0, index).Trim() + "\t");
                    zhongzhuan = zhongzhuan.Remove(0, index+1 );
 
                    index = zhongzhuan.IndexOf("&");
                    sb.Append(zhongzhuan.Substring(0, index).Trim() + "\t");
                    zhongzhuan = zhongzhuan.Remove(0, index+1);

                    sb.Append(Environment.NewLine);
                }
                while (chaxun_tiaojian.Length > 0) ;

                sb.Append(Environment.NewLine);
                for (int k = 0; k < dv.Columns.Count; k++)
                {
                    sb.Append(dv.Columns[k].HeaderText + "\t");
                }
                sb.Append(Environment.NewLine);

                for (int i = 0; i < dv.Rows.Count; i++)
                {
                    for (int j = 0; j < dv.Columns.Count; j++)
                    {
                        sb.Append(Convert.ToString(dv.Rows[i].Cells[j].Value) == "" ? "" + "\t" : dv.Rows[i].Cells[j].Value.ToString() + "\t");
                    }
                    sb.Append(Environment.NewLine);
                }
                sw.Write(sb.ToString());
                sw.Flush();
                sw.Close();
                MessageBox.Show("已经生成指定Excel文件!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                sw.Close();
            } 
        }

        public static void WritetoExcel(DataGridView dv)
        {
            string path;
            path = new_excel();
            if (path == "") return;
               xieru_datagridview_shuju(dv,path);
        }

        public static void WritetoExcel_han_chaxun_tiaojian(DataGridView dv,string chaxun_tiaojian)
        {
            string path;
            path = new_excel();
            if (path == "") return;
            xieru_chaxun_tiaojian(path, chaxun_tiaojian,dv); 
        }


        public static  DataSet ExceltoDataGridView(DataGridView dgv, string filename)
        {
            string strConn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=NO;IMEX=1;'", filename);
            OleDbConnection conn = new OleDbConnection(strConn);

            string strExcel = "select  *  from [Sheet1$]";
            OleDbDataAdapter comm = new OleDbDataAdapter(strExcel, conn); ;
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                comm.Fill(ds, strExcel);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "错误");
                comm.Dispose();
                conn.Close();
                conn.Dispose();
                return null;
            }
            comm.Fill(ds, strExcel);
            comm.Dispose();
            conn.Close();
            conn.Dispose();

            int rowCount = (ds.Tables[0].Rows.Count) / 2;
            for (int i = 0; i < rowCount; i++)
            {
                ds.Tables[0].Rows.RemoveAt(0);
            }

            return ds;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
namespace Flash_Packing_Client
{
    class DB_Class:DB
    {
      //  private string con_string;//数据库的连接字符串
        public Const_Struct.sj_xz_cx_rk_ys chepai_info_5;//时间，车牌性质，车型，入口
        public Const_Struct.get_sj_xz sj_xz_2;//时间，车牌性质
       // public Const_Struct.ccsj_tcfy sj_fy;
     //返回入场时间,在自动插入入场记录是使用
        public Const_Struct.get_sj_xz insert_biao_tingchejilu(string chepaihaoma,
                                                              string rukou,
                                                              string qicheleixing,
                                                             // string chepaixingzhi_,
                                                              string chepai_yanse,
                                                              string ruchangmenwei)
        {
            Const_Struct.get_sj_xz sj_xz;
            // MySqlConnection conn = getMySqlCon();
            MySqlCommand comm = getSqlCommand("proc_insert_biao_tingchejilu");
            //comm.Connection = conn;
            comm.CommandType = CommandType.StoredProcedure;

            MySqlParameter sp = comm.Parameters.Add("@chepaihaoma_", MySqlDbType.String);
            sp.Value = chepaihaoma;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@ruchangshijian_", MySqlDbType.DateTime);
            sp.Direction = ParameterDirection.Output;

            sp = comm.Parameters.Add("@rukou_", MySqlDbType.VarString);
            sp.Value = rukou;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@qicheleixing_", MySqlDbType.String);
            sp.Value = qicheleixing;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@chepaixingzhi_", MySqlDbType.VarString);
          //  sp.Value = chepaixingzhi_;  //参数值
            sp.Direction = ParameterDirection.Output;

            sp = comm.Parameters.Add("@ruchangmenwei_", MySqlDbType.VarString);
            sp.Value = ruchangmenwei;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@chepai_yanse", MySqlDbType.String);
            sp.Value = chepai_yanse;  //参数值
            sp.Direction = ParameterDirection.Input;

            try
            {
                comm.Connection.Open();
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                String message = ex.Message;
                System.Windows.Forms.MessageBox.Show("插入停车记录失败了！" + message);
            }
            sj_xz.ruchangshijian = comm.Parameters["@ruchangshijian_"].Value.ToString();
            sj_xz.chepaixingzhi = comm.Parameters["@chepaixingzhi_"].Value.ToString();
            comm.Connection.Close();
            comm.Connection.Dispose();
            comm.Dispose();
            return sj_xz;
        }
        //传入入场时间，在手动插入停车记录使用
        public /*Const_Struct.get_sj_xz*/void insert_biao_tingchejilu_shoudong(
                                               string chepaihaoma,
                                               string ruchang_shijian,
                                               string rukou,
                                               string qicheleixing,
                                              // string chepaixingzhi_,
                                               string chepai_yanse,
                                               string ruchangmenwei)
        {
           // Const_Struct.get_sj_xz sj_xz;
            // MySqlConnection conn = getMySqlCon();
            MySqlCommand comm = getSqlCommand("proc_insert_biao_tingchejilu_2");
            //comm.Connection = conn;
            comm.CommandType = CommandType.StoredProcedure;

            MySqlParameter sp = comm.Parameters.Add("@chepaihaoma_", MySqlDbType.VarString);
            sp.Value = chepaihaoma;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@ruchangshijian_", MySqlDbType.DateTime);
            sp.Value = Convert.ToDateTime(ruchang_shijian);  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@rukou_", MySqlDbType.VarString);
            sp.Value = rukou;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@qicheleixing_", MySqlDbType.VarString);
            sp.Value = qicheleixing;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@chepai_yanse", MySqlDbType.String);
            sp.Value = chepai_yanse;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@chepaixingzhi_", MySqlDbType.VarString);
          //  sp.Value = chepaixingzhi_;  //参数值
            sp.Direction = ParameterDirection.Output;

            sp = comm.Parameters.Add("@ruchangmenwei_", MySqlDbType.VarString);
            sp.Value = ruchangmenwei;  //参数值
            sp.Direction = ParameterDirection.Input;

            try
            {
                comm.Connection.Open();
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
               string message = ex.Message;
               MessageBox.Show("插入停车记录失败了_2！" + message);
            }
          //  sj_xz.ruchangshijian = ruchang_shijian;//comm.Parameters["@ruchangshijian_"].Value.ToString();
         //   sj_xz.chepaixingzhi = comm.Parameters["@chepaixingzhi_"].Value.ToString();
            comm.Connection.Close();
            comm.Connection.Dispose();
            comm.Dispose();
         //   return sj_xz;
        }

        //public Const_Struct.get_sj_xz update_biao_tingchejilu(
        //                                        string chepaihaoma,
        //                                        string rukou,
        //                                        string qicheleixing,
        //                                        string ruchangmenwei)
        //{
        //    Const_Struct.get_sj_xz sj_xz;

        //    //  MySqlConnection conn = getMySqlCon(con_string);
        //    MySqlCommand comm = getSqlCommand("proc_insert_biao_tingchejilu");
        //    // comm.Connection = conn;
        //    comm.CommandType = CommandType.StoredProcedure;

        //    MySqlParameter sp = comm.Parameters.Add("@chepaihaoma", MySqlDbType.VarString);
        //    sp.Value = chepaihaoma;  //参数值
        //    sp.Direction = ParameterDirection.Input;

        //    sp = comm.Parameters.Add("@ruchangshijian", MySqlDbType.DateTime);
        //    sp.Direction = ParameterDirection.Output;


        //    sp = comm.Parameters.Add("@rukou", MySqlDbType.VarString);
        //    sp.Value = rukou;  //参数值
        //    sp.Direction = ParameterDirection.Input;

        //    sp = comm.Parameters.Add("@qicheleixing", MySqlDbType.VarString);
        //    sp.Value = qicheleixing;  //参数值
        //    sp.Direction = ParameterDirection.Input;

        //    sp = comm.Parameters.Add("@chepaixingzhi", MySqlDbType.VarString);
        //    sp.Direction = ParameterDirection.Output;

        //    sp = comm.Parameters.Add("@ruchangmenwei", MySqlDbType.VarString);
        //    sp.Value = ruchangmenwei;  //参数值
        //    sp.Direction = ParameterDirection.Input;

        //    try
        //    {
        //        comm.Connection.Open();
        //        comm.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        String message = ex.Message;
        //        System.Windows.Forms.MessageBox.Show("插入停车记录失败了！" + message);
        //    }
        //    sj_xz.ruchangshijian = comm.Parameters["@ruchangshijian"].Value.ToString();
        //    sj_xz.chepaixingzhi = comm.Parameters["@chepaixingzhi"].Value.ToString();
        //    comm.Connection.Close();
        //    comm.Connection.Dispose();
        //    comm.Dispose();
        //    return sj_xz;
        //}
        public Const_Struct.ccsj_tcfy get_tingchefei(string chepaihaoma,
                                       string ruchangshijian,
                                       string chepaixingzhi,
                                       string qichexinghao
                                                         )
        {
            Const_Struct.ccsj_tcfy sj_fy;
            // MySqlConnection conn = getMySqlCon(con_string);
            MySqlCommand comm = getSqlCommand("proc_return_tingchefei");
            //  comm.Connection = conn;
            comm.CommandType = CommandType.StoredProcedure;

            MySqlParameter sp = comm.Parameters.Add("@chepai_haoma", MySqlDbType.String);
            sp.Value = chepaihaoma;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@chepai_xingzhi", MySqlDbType.String);
            sp.Value = chepaixingzhi;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@qiche_xinghao", MySqlDbType.String);
            sp.Value = qichexinghao;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@ruchang_shijian", MySqlDbType.DateTime);
            sp.Value = Convert.ToDateTime(ruchangshijian);  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@tingche_fei", MySqlDbType.Float);
            sp.Direction = ParameterDirection.Output;

            sp = comm.Parameters.Add("@chuchang_shijian", MySqlDbType.DateTime);
            sp.Direction = ParameterDirection.Output;

            try
            {
                comm.Connection.Open();
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                String message = ex.Message;
                System.Windows.Forms.MessageBox.Show("proc_return_tingchefei！" + message);
            }
            sj_fy.tingchefei = comm.Parameters["@tingche_fei"].Value.ToString();
            sj_fy.chuchang_shijian = comm.Parameters["@chuchang_shijian"].Value.ToString();
            comm.Connection.Close();
            comm.Connection.Dispose();
            comm.Dispose();
            return sj_fy;
        }

        public void update_cheppaishenfenleibie(string chepaihaoma, Decimal jinE)
        {
            //  MySqlConnection conn = getMySqlCon(con_string);
            MySqlCommand comm = getSqlCommand("proc_update_biao_chepaishenfenleibie_shengyu_jine");
            //  comm.Connection = conn;
            comm.CommandType = CommandType.StoredProcedure;

            MySqlParameter sp = comm.Parameters.Add("@chepai_haoma", MySqlDbType.String);
            sp.Value = chepaihaoma;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@tingche_jinE", MySqlDbType.NewDecimal);
            sp.Value = jinE;  //参数值
            sp.Direction = ParameterDirection.Input;
            try
            {
                comm.Connection.Open();
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                String message = ex.Message;
                System.Windows.Forms.MessageBox.Show("proc_update_biao_chepaishenfenleibie_shengyu_jine！" + message);
            }
            comm.Connection.Close();
            comm.Connection.Dispose();
            comm.Dispose();
        }

        public void update_tingchejilu(string chepaihaoma,
                                        DateTime ruchang_shijian,
                                        DateTime lichang_shijian,
                                        string chu_kou,
                                        Decimal jiaofei_jinE,
                                        string shoufei_biaoji,
                                        string lichang_menwei)
        {
            // MySqlConnection conn = getMySqlCon(con_string);
            MySqlCommand comm = getSqlCommand("proc_update_biao_tingchejilu");
            // comm.Connection = conn;
            comm.CommandType = CommandType.StoredProcedure;

            MySqlParameter sp = comm.Parameters.Add("@chepai_haoma", MySqlDbType.String);
            sp.Value = chepaihaoma;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@ruchang_shijian", MySqlDbType.DateTime);
            sp.Value = ruchang_shijian;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@lichang_shijian", MySqlDbType.DateTime);
            sp.Value = lichang_shijian;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@chu_kou", MySqlDbType.String);
            sp.Value = chu_kou;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@jiaofei_jinE", MySqlDbType.Float);
            sp.Value = jiaofei_jinE;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@shoufei_biaoji", MySqlDbType.String);
            sp.Value = shoufei_biaoji;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@lichang_menwei", MySqlDbType.String);
            sp.Value = lichang_menwei;  //参数值
            sp.Direction = ParameterDirection.Input;

            try
            {
                comm.Connection.Open();
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                String message = ex.Message;
                System.Windows.Forms.MessageBox.Show("proc_update_biao_tingchejilu！" + message);
            }
            comm.Connection.Close();
            comm.Connection.Dispose();
            comm.Dispose();
        }


        public Const_Struct.sj_xz_cx_rk_ys get_tingchejilu(string chepaihaoma)
        {
            Const_Struct.sj_xz_cx_rk_ys sj_xz;
          
            MySqlCommand comm = getSqlCommand("proc_get_tingchejilu");
          
            comm.CommandType = CommandType.StoredProcedure;

            MySqlParameter sp = comm.Parameters.Add("@chepai_haoma", MySqlDbType.String);
            sp.Value = chepaihaoma;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@chepai_xingzhi", MySqlDbType.String);
            sp.Direction = ParameterDirection.Output;

            sp = comm.Parameters.Add("@qiche_leixing", MySqlDbType.String);
            sp.Direction = ParameterDirection.Output;

            sp = comm.Parameters.Add("@ruchang_shijian", MySqlDbType.DateTime);
            sp.Direction = ParameterDirection.Output;

            sp = comm.Parameters.Add("@ru_kou", MySqlDbType.String);
            sp.Direction = ParameterDirection.Output;

            sp = comm.Parameters.Add("@chepai_yanse", MySqlDbType.String);
            sp.Direction = ParameterDirection.Output;

            try
            {
                comm.Connection.Open();
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                MessageBox.Show("proc_get_tingchejilu！" + message);
            }
            sj_xz.ruchangshijian = comm.Parameters["@ruchang_shijian"].Value.ToString();
            sj_xz.chepaixingzhi = comm.Parameters["@chepai_xingzhi"].Value.ToString();
            sj_xz.qiche_leixing = comm.Parameters["@qiche_leixing"].Value.ToString();
            sj_xz.ru_kou = comm.Parameters["@ru_kou"].Value.ToString();
            sj_xz.chepai_yanse = comm.Parameters["@chepai_yanse"].Value.Equals(DBNull.Value)?"BLUE": comm.Parameters["@chepai_yanse"].Value.ToString();
            comm.Connection.Close();
            comm.Connection.Dispose();
            comm.Dispose();

            return sj_xz;
        }

        public int insert_biao_jifeicelue(Int16 cunhuo_biaoji,
                                                 string qiche_leixing,
                                                 int shichang_dayu,
                                                 int shichang_xiaoyu,
                                                 int jifei_danwei,
                                                 Decimal jifei_danjia)
        {
            int xuliehao = 0;
            MySqlCommand comm = getSqlCommand("proc_insert_biao_jifeicelue");
            comm.CommandType = CommandType.StoredProcedure;

            MySqlParameter sp = comm.Parameters.Add("@qiche_leixing", MySqlDbType.String);
            sp.Value = qiche_leixing;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@shichang_dayu", MySqlDbType.Int16);
            sp.Value = shichang_dayu;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@shichang_xiaoyu", MySqlDbType.Int24);
            sp.Value = shichang_xiaoyu;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@jifei_danwei", MySqlDbType.Int32);
            sp.Value = jifei_danwei;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@jifei_danjia", MySqlDbType.Float);
            sp.Value = jifei_danjia;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@xuliehao", MySqlDbType.Int32);
            sp.Direction = ParameterDirection.Output;

            try
            {
                comm.Connection.Open();
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                String message = ex.Message;
                System.Windows.Forms.MessageBox.Show("proc_insert_biao_jifeicelue失败！" + message);
            }
            xuliehao = Convert.ToInt32(comm.Parameters["@xuliehao"].Value);
            comm.Connection.Close();
            comm.Connection.Dispose();
            comm.Dispose();
            return xuliehao;
        }

        public void update_biao_jifeicelue(int xuliehao,
                                               string qiche_leixing,
                                               int shichang_dayu,
                                               int shichang_xiaoyu,
                                               int jifei_danwei,
                                               Decimal jifei_danjia,
                                               Byte cunhuo_biaoji)
        {
            MySqlCommand comm = getSqlCommand("proc_update_biao_jifeicelue");
            comm.CommandType = CommandType.StoredProcedure;

            MySqlParameter sp = comm.Parameters.Add("@qiche_leixing", MySqlDbType.String);
            sp.Value = qiche_leixing;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@shichang_dayu", MySqlDbType.Int16);
            sp.Value = shichang_dayu;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@shichang_xiaoyu", MySqlDbType.Int24);
            sp.Value = shichang_xiaoyu;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@jifei_danwei", MySqlDbType.Int32);
            sp.Value = jifei_danwei;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@jifei_danjia", MySqlDbType.Float);
            sp.Value = jifei_danjia;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@xuliehao", MySqlDbType.Int24);
            sp.Value = xuliehao;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@cunhuo_biaoji", MySqlDbType.Int16);
            sp.Value = cunhuo_biaoji;  //参数值
            sp.Direction = ParameterDirection.Input;

            try
            {
                comm.Connection.Open();
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                String message = ex.Message;
                System.Windows.Forms.MessageBox.Show("proc_update_biao_jifeicelue失败！" + message);
            }
            comm.Connection.Close();
            comm.Connection.Dispose();
            comm.Dispose();
        }

        public void delete_biao_jifeicelue(int xuliehao)
        {
            MySqlCommand comm = getSqlCommand("proc_delete_biao_jifeicelue");
            comm.CommandType = CommandType.StoredProcedure;

            MySqlParameter sp = comm.Parameters.Add("@xuliehao", MySqlDbType.Int24);
            sp.Value = xuliehao;  //参数值
            sp.Direction = ParameterDirection.Input;

            try
            {
                comm.Connection.Open();
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                String message = ex.Message;
                System.Windows.Forms.MessageBox.Show("proc_delete_biao_jifeicelue失败！" + message);
            }
            comm.Connection.Close();
            comm.Connection.Dispose();
            comm.Dispose();
        }

        public DataSet select_biao_jifeicelue()
        {
            MySqlCommand mysqlcom = getSqlCommand("proc_select_biao_jifeicelue");
            mysqlcom.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            try
            {
                mysqlcom.Connection.Open();
                mysqlcom.ExecuteNonQuery();
                adapter.SelectCommand = mysqlcom;
                adapter.Fill(ds, "proc_select_biao_jifeicelue");

                mysqlcom.Connection.Close();
                mysqlcom.Connection.Dispose();
                mysqlcom.Dispose();
                return ds;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                MessageBox.Show("proc_select_biao_jifeicelue失败！" + message);
            }
            mysqlcom.Connection.Close();
            mysqlcom.Connection.Dispose();
            mysqlcom.Dispose();
            return null;
        }

        public DataTable select_all_zaichang_chepai()
        {
            //select ChePaiHaoMa, RuChangShiJian, RuKou
            //from biao_tingchejilu
            //where ShouFeiBiaoJi = '在场';

            MySqlCommand mysqlcom = getSqlCommand("proc_select_all_tingchejilu");
            mysqlcom.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            try
            {
                mysqlcom.Connection.Open();
                mysqlcom.ExecuteNonQuery();
                adapter.SelectCommand = mysqlcom;
                adapter.Fill(ds, "proc_select_all_tingchejilu");

                mysqlcom.Connection.Close();
                mysqlcom.Connection.Dispose();
                mysqlcom.Dispose();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                String message = ex.Message;
                System.Windows.Forms.MessageBox.Show("proc_select_all_tingchejilu！" + message);
            }
            mysqlcom.Connection.Close();
            mysqlcom.Connection.Dispose();
            mysqlcom.Dispose();
            return null;
        }
        
        public DataTable select_all_zaichang_chepai_ip(string chuchang_shijian)
        {
            //-- 获取所有入场时间小于该出场时间的在场车辆的车牌等信息
            //select ChePaiHaoMa,RuChangShiJian,RuKou
            //from biao_tingchejilu
            //where ShouFeiBiaoJi = '在场' and RuChangShiJian<chuchang_shijian;
            MySqlCommand mysqlcom = getSqlCommand("proc_select_all_tingchejilu_ip");
            mysqlcom.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

           MySqlParameter sp = mysqlcom.Parameters.Add("@chuchang_shijian", MySqlDbType.DateTime);
            sp.Value =Convert.ToDateTime(chuchang_shijian);  //参数值
            sp.Direction = ParameterDirection.Input;
           
            try
            {
                mysqlcom.Connection.Open();
                mysqlcom.ExecuteNonQuery();
                adapter.SelectCommand = mysqlcom;
                adapter.Fill(ds, "proc_select_all_tingchejilu_ip");

                mysqlcom.Connection.Close();
                mysqlcom.Connection.Dispose();
                mysqlcom.Dispose();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                String message = ex.Message;
                MessageBox.Show("proc_select_all_tingchejilu_ip！" + message);
            }
            mysqlcom.Connection.Close();
            mysqlcom.Connection.Dispose();
            mysqlcom.Dispose();
            return null;
        }
        public DataView chepai_pipei(string chepai)
        {
            DataTable dt = new DataTable();
            dt = select_all_zaichang_chepai();
            DataColumn dc = new DataColumn("quanzhi", typeof(int));
            dt.Columns.Add(dc);
            int chepai_length = 0;

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int quanzhi = 0;
                    if (chepai.Length > dt.Rows[i][0].ToString().Length)
                        chepai_length = dt.Rows[i][0].ToString().Length;
                    else
                        chepai_length = chepai.Length;
                    for (int chepai_index = chepai_length - 1; chepai_index >= 0; chepai_index--)
                    {
                        if (dt.Rows[i][0].ToString().Substring(chepai_index, 1) == chepai.Substring(chepai_index, 1))
                        {
                            dt.Rows[i]["quanzhi"] = ++quanzhi;
                        }
                    }

                }
            }
            DataView dv = new DataView(dt, "quanzhi > 4", "quanzhi DESC", DataViewRowState.CurrentRows);
            if (dv.Count > 0)
                return dv;
            else
                return null;
        }

        public DataView chepai_pipei_ip(string chepai,string chuchang_shijian)
        {
            DataTable dt = new DataTable();
           
            dt = select_all_zaichang_chepai_ip(chuchang_shijian);
            DataColumn dc = new DataColumn("quanzhi", typeof(int));
            dt.Columns.Add(dc);
            int chepai_length = 0;
           
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int quanzhi = 0;
                    if (chepai.Length > dt.Rows[i][0].ToString().Length)
                        chepai_length = dt.Rows[i][0].ToString().Length;
                    else
                        chepai_length = chepai.Length;
                    for (int chepai_index = chepai_length - 1; chepai_index >= 0; chepai_index--)
                    {
                        if (dt.Rows[i][0].ToString().Substring(chepai_index, 1) == chepai.Substring(chepai_index, 1))
                        {
                            dt.Rows[i]["quanzhi"] = ++quanzhi;
                        }
                    }

                }
            }
          
            DataView dv = new DataView(dt, "quanzhi > 4", "quanzhi DESC", DataViewRowState.CurrentRows);
            if (dv.Count > 0)
                return dv;
            else
                return null;
        }

        public DataView chepai_mohu_pipei(string chepai)
        {
            if (chepai == "") return null;
            DataTable dt = new DataTable();
            dt = select_all_zaichang_chepai();
            if (dt != null)
            {
                DataView dv = new DataView(dt, "ChePaiHaoMa like '%" + chepai + "%'", "ChePaiHaoMa DESC", DataViewRowState.CurrentRows);
                if (dv.Count > 0)
                    return dv;
                else
                    return null;
            }
            return null;
        }

        //public DataView chepai_mohu_pipei_wupaiche(string chepai)
        //{
        //    if (chepai == "") return null;
        //    DataTable dt = new DataTable();
        //    dt = select_all_zaichang_chepai();
        //    if (dt != null)
        //    {
        //        DataView dv = new DataView(dt, "ChePaiHaoMa like '%" + chepai + "%'", "ChePaiHaoMa DESC", DataViewRowState.CurrentRows);
        //        if (dv.Count > 0)
        //            return dv;
        //        else
        //            return null;
        //    }
        //    return null;
        //}
        public void insert_biao_tupian(string tupan_lujing_mingcheng,
                                       DateTime cunru_shijian,
                                       string chepai_haoma,
                                       string xiangji_ip)
        {
            //FileStream文件流；此部分需要引用：System.IO;
            FileStream fs = new FileStream(tupan_lujing_mingcheng, FileMode.Open, FileAccess.Read);

            //获得图片字节数组
            byte[] byImage = new byte[fs.Length];
            fs.Read(byImage, 0, byImage.Length);
            fs.Close();

            MySqlCommand comm = getSqlCommand("proc_insert_biao_tupian");
            comm.CommandType = CommandType.StoredProcedure;

            MySqlParameter sp = comm.Parameters.Add("@tupian_neirong", MySqlDbType.MediumBlob);
            sp.Value = byImage;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@cunru_shijian", MySqlDbType.DateTime);
            sp.Value = cunru_shijian;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@chepai_haoma", MySqlDbType.String);
            sp.Value = chepai_haoma;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@xiangji_ip", MySqlDbType.String);
            sp.Value = xiangji_ip;  //参数值
            sp.Direction = ParameterDirection.Input;

            try
            {
                comm.Connection.Open();
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                comm.Connection.Close();
                comm.Connection.Dispose();
                comm.Dispose();
                String message = ex.Message;
                System.Windows.Forms.MessageBox.Show("proc_insert_biao_tupian！" + message);
            }
            comm.Connection.Close();
            comm.Connection.Dispose();
            comm.Dispose();
        }

        public Bitmap db_select_tupian_rukou(string cunru_shijian,
                                      string chepai_haoma)
        {
            Bitmap tupian;
            var com_str = "SELECT tupianneirong from biao_tupian ";
             com_str = com_str + " where chepaihaoma = '" + chepai_haoma + "' and cunrushijian = '" + cunru_shijian + "';";    
            var str = Encoding.UTF8.GetBytes(com_str);
       
            MySqlCommand comm = getSqlCommand(Encoding.UTF8.GetString(str));
            MySqlDataReader dataReader = null;
            try
            {
                comm.Connection.Open();
                dataReader = comm.ExecuteReader();
                dataReader.Read();
            }
            catch
            {
                dataReader.Dispose();
                comm.Connection.Close();
                comm.Connection.Dispose();
                comm.Dispose();
                throw new ArgumentException("图片加载超时啦！");
            }

            bool no_null = false;
            byte[] images;
            try
            {
                images = (byte[])dataReader[0];
                no_null = true;
            }
            catch
            {
                images = null;
                no_null = false;
            }
            if (no_null)
            {
                if (images.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(images);
                    Bitmap bmp = new Bitmap(ms);
                    tupian = bmp;
                }
                else
                {
                    tupian = null;
                }
            }
            else
                tupian = null;
            dataReader.Dispose();
            comm.Connection.Close();
            comm.Connection.Dispose();
            comm.Dispose();
            return tupian;
        }
   
        public Const_Struct.bitmap_chepai db_select_tupian_from_ip(string xiangji_ip)
        {
            Const_Struct.bitmap_chepai bp_cp = new Const_Struct.bitmap_chepai();
            string com_str = "SELECT tupianneirong,chepaihaoma,cunrushijian  from biao_tupian ";
            com_str = com_str + " where xiangjiIP = '"
                    + xiangji_ip
                    + "' order by cunrushijian DESC limit 1 ";

            // System.Windows.Forms.MessageBox.Show(com_str);
           
            MySqlCommand comm = getSqlCommand(com_str);
            MySqlDataReader dataReader = null;
            try
            {
                comm.Connection.Open();
                dataReader = comm.ExecuteReader();
                dataReader.Read();
            }
            catch
            {
                dataReader.Dispose();
                comm.Connection.Close();
                comm.Connection.Dispose();
                comm.Dispose();
                throw new ArgumentException("db_select_tupian_from_ip图片加载超时啦！");
            }
          
            bool no_null = false;
            byte[] images;
            try
            {
                images = (byte[])dataReader[0];
                bp_cp.chepai_haoma = dataReader[1].ToString();
                bp_cp.cunru_shijian = dataReader[2].ToString();           
                no_null = true;
            }
            catch
            {
                images = null;
                bp_cp.chepai_haoma = "";
                bp_cp.cunru_shijian = "";
               no_null = false;
            }
            if (no_null)
            {
                if (images.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(images);
                    Bitmap bmp = new Bitmap(ms);
                    bp_cp.tupian = bmp;              
                }
                else
                {
                    bp_cp.tupian = null;
                    bp_cp.chepai_haoma = "";
                    bp_cp.cunru_shijian = "";
                }
            }
            else
            {
                bp_cp.tupian = null;
                bp_cp.chepai_haoma = "";
                bp_cp.cunru_shijian = "";
            }
               
            dataReader.Dispose();
            comm.Connection.Close();
            comm.Connection.Dispose();
            comm.Dispose();
           
            return bp_cp;
          }

        public void update_xiangji_zaixiang_biaoji(string xiangji_ip,bool zaixian_biaoji)
        {
            MySqlCommand comm = getSqlCommand("proc_update_xiangji_zaixian_biao");
            comm.CommandType = CommandType.StoredProcedure;

            MySqlParameter sp = comm.Parameters.Add("@xiangji_ip", MySqlDbType.String);
            sp.Value = xiangji_ip;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@zaixian_biaoji", MySqlDbType.Bit);
            sp.Value = zaixian_biaoji;  //参数值
            sp.Direction = ParameterDirection.Input;
            try
            {
                comm.Connection.Open();
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                comm.Connection.Close();
                comm.Connection.Dispose();
                comm.Dispose();
                String message = ex.Message;
                MessageBox.Show("proc_update_xiangji_zaixian_biao！" + message);
            }
            comm.Connection.Close();
            comm.Connection.Dispose();
            comm.Dispose();
        }

        public bool xiangji_shifou_zaixian(string xiangji_ip)
        {
            bool zaixian_biaoji=false;
            MySqlCommand comm = getSqlCommand("proc_select_xiangji_shifou_zaixian");
            comm.CommandType = CommandType.StoredProcedure;

            MySqlParameter sp = comm.Parameters.Add("@xiangji_ip", MySqlDbType.String);
            sp.Value = xiangji_ip;  //参数值
            sp.Direction = ParameterDirection.Input;

            sp = comm.Parameters.Add("@zaixian_biaoji", MySqlDbType.Bit);
            sp.Value = zaixian_biaoji;  //参数值
            sp.Direction = ParameterDirection.Output;
            try
            {
                comm.Connection.Open();
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                comm.Connection.Close();
                comm.Connection.Dispose();
                comm.Dispose();
                String message = ex.Message;
                MessageBox.Show("proc_select_xiangji_shifou_zaixian！" + message);
                return false;   
            }
            if (comm.Parameters[1].Value.ToString() == "0")
                zaixian_biaoji = false;
            else
                zaixian_biaoji = true;

            comm.Connection.Close();
            comm.Connection.Dispose();
            comm.Dispose();
            return zaixian_biaoji;
        }

           public void update_chepai_xingzhi(string chepai_haoma,
                                             string ruchang_shijian,
                                             string chepai_xingzhi
                                             )
        {
            string str="";
            str = "update biao_tingchejilu  set ChePaiXingZhi='" + chepai_xingzhi + "'"
                + " where ChePaiHaoMa='" + chepai_haoma + "' and RuChangShiJian ='" + ruchang_shijian + "'";

            MySqlCommand comm = getSqlCommand(str);
            try
            {
                comm.Connection.Open();
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                MessageBox.Show("update_chepai_xingzhi失败！" + message);
            }
            comm.Connection.Close();
            comm.Connection.Dispose();
            comm.Dispose();
        }
    }
 }




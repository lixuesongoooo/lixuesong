using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace Flash_Packing_Client
{
    class DB
    {
        private string con_string;
        public DB()
        {
            con_string = WIN32_API_FUN.Get_CONN_STR();
        }
        private MySqlConnection getMySqlCon()
        {
            MySqlConnection sql_conn = new MySqlConnection(con_string);
            return sql_conn;
        }
        public MySqlCommand getSqlCommand(string sql)
        {
            MySqlConnection sql_conn = getMySqlCon();
            MySqlCommand mySqlCommand = new MySqlCommand(sql, sql_conn);
            mySqlCommand.Connection = sql_conn;
            return mySqlCommand;
        }
       


    }
}

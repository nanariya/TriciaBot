using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace TriciaBot
{
    class UserDB
    {
        private String FileName { get; set; }

        public UserDB(String fileName)
        {
            this.FileName = fileName;
        }

        public void CreateUserDB()
        {
            NTLIB.SQLite3 sql = new NTLIB.SQLite3(this.FileName);
            sql.ExecuteNonQuery(Properties.Settings.Default.DBCreateUserTable);
        }

        public void CreateWhiteListDB()
        {
            NTLIB.SQLite3 sql = new NTLIB.SQLite3(this.FileName);
            sql.ExecuteNonQuery(Properties.Settings.Default.DBCreateWhiteListTable);
        }

        public String SelectUserNickname(Int64 id)
        {
            NTLIB.SQLite3 sql = new NTLIB.SQLite3(this.FileName);
            DataTable res = sql.SelectQuery(Properties.Settings.Default.DBSelectUserTable);
            DataRow row = res.Rows.Find(id);
            if (row == null) return "";
            return row["nick_name"].ToString();
        }
        public void ChangeUserNickname(Int64 id, String nickName)
        {
            DataTable dt = SelectUserTable();
            DataRow row = dt.Rows.Find(id);
            if(row != null)
            {
                row["id"] = id;
                row["nick_name"] = nickName;
            }
            UpdateUserTable(dt);
        }
        public void AddUserData(Int64 id, String name, String screenName)
        {
            DataTable dt = SelectUserTable();
            DataRow row = dt.Rows.Find(id);
            if (row != null)
            {
                row["name"] = name;
                row["screen_name"] = screenName;
            }
            else 
            {
                DataRow ad = dt.NewRow();
                ad["id"] = id;
                ad["name"] = name;
                ad["screen_name"] = screenName;
                dt.Rows.Add(ad);
            }
            UpdateUserTable(dt);
        }

        public List<Int64> SelectWhiteList()
        {
            NTLIB.SQLite3 sql = new NTLIB.SQLite3(this.FileName);
            DataTable res = sql.SelectQuery(Properties.Settings.Default.DBSelectWhiteListTable);
            List<Int64> idList = res.Columns.Cast<Int64>().ToList();
            return idList;
        }
        public Boolean AddWhiteList(Int64 id)
        {
            DataTable dt = SelectWhiteListTable();
            DataRow row = dt.Rows.Find(id);
            if (row != null)
            {
                row["name"] = name;
                row["screen_name"] = screenName;
            }
            else
            {
                DataRow ad = dt.NewRow();
                ad["id"] = id;
                ad["name"] = name;
                ad["screen_name"] = screenName;
                dt.Rows.Add(ad);
            }
            UpdateUserTable(dt);
        }
        public Boolean DelWhiteList(Int64 id)
        {

        }

        private DataTable SelectUserTable()
        {
            NTLIB.SQLite3 sql = new NTLIB.SQLite3(this.FileName);
            return sql.SelectQuery(Properties.Settings.Default.DBSelectUserTable);
        }
        private void UpdateUserTable(DataTable dt)
        {
            NTLIB.SQLite3 sql = new NTLIB.SQLite3(this.FileName);
            sql.UpdateDatatableQuery(dt, Properties.Settings.Default.DBSelectUserTable);
        }

        private DataTable SelectWhiteListTable()
        {
            NTLIB.SQLite3 sql = new NTLIB.SQLite3(this.FileName);
            return sql.SelectQuery(Properties.Settings.Default.DBSelectWhiteListTable);
        }
        private void UpdateWhiteListTable(DataTable dt)
        {
            NTLIB.SQLite3 sql = new NTLIB.SQLite3(this.FileName);
            sql.UpdateDatatableQuery(dt, Properties.Settings.Default.DBSelectWhiteListTable);
        }

    }
}

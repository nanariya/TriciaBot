using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace NTLIB
{
    public class SQLite3
    {
        private SQLiteConnection Conn { get; set; }
        public String DatabaseFile { get; set; }

        public SQLite3(String databaseFile)
        {
            this.DatabaseFile = databaseFile;
            this.Connect();
        }
        private void  Connect()
        {
             Conn = new SQLiteConnection("Data Source=" + this.DatabaseFile);
             Conn.Open();
        }
        public void Disconnect()
        {
            Conn.Close();
        }

        public void ExecuteNonQuery(String query)
        {
                using (SQLiteCommand command = Conn.CreateCommand())
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery();     
                }
        }
        public void UpdateQuery(String query)
        {
                using (SQLiteCommand command = Conn.CreateCommand())
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
        }
        public DataTable SelectQuery(String query)
        {
            DataTable dt = new DataTable();

                using (SQLiteCommand command = Conn.CreateCommand())
                {
                    command.CommandText = query;
                    SQLiteDataAdapter adp = new SQLiteDataAdapter(command);
                    adp.FillSchema(dt, SchemaType.Source);
                    adp.Fill(dt);
                }
            return dt;
        }
       public void UpdateDatatableQuery(DataTable dt, String query)
        {
            DataTable sdt = new DataTable();
            using (SQLiteCommand command = Conn.CreateCommand())
            {
                command.CommandText = query;
                SQLiteDataAdapter adp = new SQLiteDataAdapter(command);
                adp.Fill(sdt);
                SQLiteCommandBuilder cb = new SQLiteCommandBuilder(adp);
                //SQLiteCommand update = cb.GetUpdateCommand();
                //adp.UpdateCommand = update;
                adp.Update(dt);
            }
        }
    }
}

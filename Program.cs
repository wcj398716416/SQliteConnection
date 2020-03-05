using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite.Linq;

namespace SQliteHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"d:\123.db";
            CreateSQliteTable(path);
            ReaderGivenSQliteTable(path, "t1");
            //Console.WriteLine();
            //Console.WriteLine();
            //   ReaderAllSQliteTable(path);          
          
            SQLiteConnection scn = new SQLiteConnection("data source=" + path);
            for (int i = 0; i < 1; i++)
            {
                SQliteData sdata = new SQliteData("34", i, "78",DateTime.Now.Millisecond.ToString());
            //    InsertData(sdata, scn);
                SelectData(sdata, scn);
            }
           

            Console.WriteLine("结束");
            Console.Read();

        }

        private static void InsertData(SQliteData sdata, SQLiteConnection scn)
        {
            if (scn.State != ConnectionState.Open)
            {
                scn.Open();
                SQLiteCommand cmd = scn.CreateCommand();
                cmd.Connection = scn;
                 cmd.CommandText = string.Format("insert into t1 values({0},{1},{2},{3})", sdata.Id, sdata.Indata, sdata.Strdata, sdata.Iamgeinfo);
                cmd.ExecuteNonQuery();
                scn.Close();
            }
        }

        private static void SelectData(SQliteData sdata, SQLiteConnection scn)
        {
            if (scn.State != ConnectionState.Open)
            {
                scn.Open();
                SQLiteCommand cmd = scn.CreateCommand();
                cmd.Connection = scn;
                cmd.CommandText= "select* from t1 where imageinfo=770";
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    Console.WriteLine( "Indata:"+sr.GetValue(1));
                }
                sr.Close();
                cmd.ExecuteNonQuery();
                scn.Close();
            }
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="path"></param>
        private static void CreateSQliteTable(string path)
        {
            SQLiteConnection scn = new SQLiteConnection("data source=" + path);
            if (scn.State != ConnectionState.Open)
            {
                scn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = scn;
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS t1(id varchar(4),score int,data string,imageinfo string)";
                cmd.ExecuteNonQuery();
            }
            scn.Close();
        }

        /// <summary>
        /// 查找指定表的内容
        /// </summary>
        /// <param name="path"></param>
        private static void ReaderGivenSQliteTable(string path, string t1)
        {
            SQLiteConnection scn = new SQLiteConnection("data source=" + path);
            if (scn.State != ConnectionState.Open)
            {
                scn.Open();
                SQLiteCommand cmd = scn.CreateCommand();
                cmd.CommandText = string.Format("PRAGMA table_info({0})", t1);
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.WriteLine(reader[i]);
                    }
                }
                reader.Close();
                cmd.ExecuteNonQuery();
            }
            scn.Close();
        }

        /// <summary>
        /// 查找所有表的内容
        /// </summary>
        /// <param name="path"></param>
        private static void ReaderAllSQliteTable(string path)
        {
            SQLiteConnection scn = new SQLiteConnection("data source=" + path);
            if (scn.State != ConnectionState.Open)
            {
                scn.Open();
                SQLiteCommand cmd = scn.CreateCommand();
                cmd.Connection = scn;
                cmd.CommandText = "SELECT name " +
                                  "FROM sqlite_master " +
                                  "WHERE TYPE='table' ";
                SQLiteDataReader reader = cmd.ExecuteReader();
                List<string> tables = new List<string>();
                while (reader.Read())
                {
                    tables.Add(reader.GetString(0));
                }
                reader.Close();
                foreach (var q in tables)
                {
                    cmd.CommandText = $"PRAGMA TABLE_INFO({q})";
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write(reader[i] + ",");
                        }
                        Console.WriteLine("+++++");
                    }
                    reader.Close();
                    Console.WriteLine();
                }
                cmd.ExecuteNonQuery();
            }
            scn.Close();
        }

    }

    public class SQliteData
    {
        private string id;
        private int indata;
        private string strdata;
        private string iamgeinfo;

        public string Id { get => id; set => id = value; }
        public int Indata { get => indata; set => indata = value; }
        public string Strdata { get => strdata; set => strdata = value; }
        public string Iamgeinfo { get => iamgeinfo; set => iamgeinfo = value; }
      public   SQliteData() { }
        public SQliteData(string str1,int int1,string str3,string str4)
        {
            this.id = str1;
            this.indata = int1;
            this.strdata = str3;
            this.iamgeinfo = str4;
        }
    }
}
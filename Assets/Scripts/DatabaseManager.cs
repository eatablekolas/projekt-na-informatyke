using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Mono.Data.Sqlite;

public class DatabaseManager : MonoBehaviour
{
    void ReadData(string sqlQuery = "SELECT * FROM meals")
    {
        string conn = "URI=file:" + Application.dataPath + "/dziennik.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection) new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            int meal_time = reader.GetInt32(2);
            int day = reader.GetInt32(3);
            int month = reader.GetInt32(4);
            int year = reader.GetInt32(5);
            DateTime date = new DateTime(year, month, day);

            print(id + name + meal_time + day + month + year + (int)date.DayOfWeek);
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}

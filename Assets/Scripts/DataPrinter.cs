using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Mono.Data.Sqlite;

public class DataPrinter : MonoBehaviour
{
    void Start()
    {
        string conn = "URI=file:" + Application.dataPath + "/dziennik.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection) new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = 
        "SELECT * FROM day";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            int day_of_month = reader.GetInt32(1);
            int month = reader.GetInt32(2);
            int year = reader.GetInt32(3);

            print("id = " + id + "; day of month = " + day_of_month + "; month = " +  month + "; year = " +  year);
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}

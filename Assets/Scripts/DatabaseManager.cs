using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using Mono.Data.Sqlite;

public class DatabaseManager : MonoBehaviour
{
    string GetConnectionString()
    {
        string conn;
#if UNITY_EDITOR
        conn = "URI=file:" + Application.streamingAssetsPath + "/dziennik.db";
#elif UNITY_ANDROID
        string filePath = Application.persistentDataPath + "/dziennik.db";

        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"File '{filePath}' does not exist. Attempting to create from '{Application.streamingAssetsPath}'...");

            WWW loadDb = new WWW(Application.streamingAssetsPath + "/dziennik.db");
            while (loadDb.isDone) { }
            File.WriteAllBytes(filePath, loadDb.bytes);
        }

        conn = "URI=file:" + filePath;
#endif
        return conn;
    }

    public Dictionary<int, List<string>> ReadMealNames(DateTime date)
    {
        string conn = GetConnectionString();
        Debug.Log("Establishing connection to " + conn);
        SqliteConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();
        SqliteCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery =   $"SELECT meal_time, name FROM meals " +
                            $"WHERE day = {date.Day} AND month = {date.Month} AND year = {date.Year}";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        Dictionary<int, List<string>> MealData = new Dictionary<int, List<string>>();
        for (int mealTime = 0; mealTime < 5; mealTime++)
        {
            MealData[mealTime] = new List<string>();
        }

        int i = 0;
        while (reader.Read())
        {
            int meal_time = reader.GetInt32(0);
            string name = reader.GetString(1);

            MealData[meal_time].Add(name);

            i++;
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

        return MealData;
    }

    public void AddMeal(string name, int meal_time, DateTime date)
    {
        string conn = GetConnectionString();
        IDbConnection dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery =   $"INSERT INTO meals (name, meal_time, day, month, year) " +
                            $"VALUES ('{name}', '{meal_time}', '{date.Day}', '{date.Month}', '{date.Year}')";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    public void RemoveMeal(string name, int meal_time, DateTime date)
    {
        string conn = GetConnectionString();
        IDbConnection dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery =   $"DELETE FROM meals " +
                            $"WHERE name = '{name}' " +
                            $"AND meal_time = {meal_time} " +
                            $"AND day = {date.Day} " +
                            $"AND month = {date.Month} " +
                            $"AND year = {date.Year}";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}

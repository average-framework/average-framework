using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Server.Core.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Core.Data
{
    public static class MySQL
    {
        private static MySqlConnection connection;

        public static bool IsOpen { get; private set; }

        public static void Connect(string host = "localhost", int port = 3306, string database = "default", string username = "root", string password = "")
        {
            try
            {
                connection = new MySqlConnection($"SERVER={host};PORT={port};DATABASE={database};UID={username};PASSWORD={password};");
                connection.Open();

                IsOpen = true;

                Log.WriteLog((string)Lang.Current["Server.MySQL.ConnectionEtablished"]);
            }
            catch (MySqlException ex)
            {
                IsOpen = false;
                Log.WriteLog($"{Lang.Current["Server.MySQL.ConnectionError"]}: ^{(int)ConsoleColor.Red} {ex.Message}");
            }
        }

        public static T Select<T>(string cmd)
        {
            if (!IsOpen) return (T)Activator.CreateInstance(typeof(T));

            var command = connection.CreateCommand();
            command.CommandText = cmd;
            var reader = command.ExecuteReader();
            reader.Read();

            return MapDeserialize<T>(reader);
        }

        public static List<T> SelectMultiples<T>(string cmd)
        {
            if (!IsOpen) return new List<T>();

            var command = connection.CreateCommand();
            command.CommandText = cmd;
            var reader = command.ExecuteReader();

            return MapDeserializeMultiples<T>(reader);
        }

        public static bool Exists(string cmd)
        {
            if (!IsOpen) return false;

            var command = connection.CreateCommand();
            command.CommandText = cmd;
            return command.ExecuteScalar() == null ? false : true;
        }

        public static void Insert(string table, object value)
        {
            if (!IsOpen) return;

            var dict = MapSerialize(value);
            var command = connection.CreateCommand();

            for (var i = 0; i < dict.Count; i++)
            {
                var v = dict.ElementAt(i);

                if (v.Value.GetType() == typeof(double) || v.Value.GetType() == typeof(float) || v.Value.GetType() == typeof(decimal) || v.Value.GetType() == typeof(int))
                    dict[v.Key] = $"{v.Value}";
                else
                    dict[v.Key] = $"'{v.Value}'";
            }

            var argsKey = string.Join(",", dict.Keys);
            var argsValue = string.Join(",", dict.Values);
            var stringResult = $"INSERT INTO {table}({argsKey}) VALUES({argsValue})";

            command.CommandText = stringResult;
            command.ExecuteNonQuery();
        }

        public static void InsertOrUpdate(string table, object value)
        {
            if (!IsOpen) return;

            var dict = MapSerialize(value);
            var tempDict = new Dictionary<string, object>();

            for (var i = 0; i < dict.Count; i++)
            {
                var v = dict.ElementAt(i);

                if (v.Value.GetType() == typeof(double) || v.Value.GetType() == typeof(float) || v.Value.GetType() == typeof(decimal) || v.Value.GetType() == typeof(int))
                {
                    dict[v.Key] = $"{v.Value}";
                    tempDict[v.Key] = $"{v.Key}={v.Value.ToString().Replace(",", ".")}";
                }
                else
                {
                    dict[v.Key] = $"'{v.Value}'";
                    tempDict[v.Key] = $"{v.Key}='{v.Value}'";
                }
            }

            var argsKey = string.Join(",", dict.Keys);
            var argsValue = string.Join(",", dict.Values);
            var argsDuplicate = string.Join(", ", tempDict.Values);
            var stringResult = $"INSERT INTO {table}({argsKey}) VALUES({argsValue}) ON DUPLICATE KEY UPDATE {argsDuplicate}";

            var command = connection.CreateCommand();
            command.CommandText = stringResult;
            command.ExecuteNonQuery();
        }

        public static void Update(string table, string where, object value)
        {
            if (!IsOpen) return;

            var dict = MapSerialize(value);

            for (var i = 0; i < dict.Count; i++)
            {
                var v = dict.ElementAt(i);

                if (v.Value.GetType() == typeof(double) || v.Value.GetType() == typeof(float) || v.Value.GetType() == typeof(decimal) || v.Value.GetType() == typeof(int))
                    dict[v.Key] = $"{v.Key}={v.Value.ToString().Replace(",", ".")}";
                else
                    dict[v.Key] = $"{v.Key}='{v.Value}'";
            }

            var argsValue = string.Join(",", dict.Values);
            var stringResult = $"UPDATE {table} SET {argsValue} WHERE {where}";

            var command = connection.CreateCommand();
            command.CommandText = stringResult;
            command.ExecuteNonQuery();
        }

        private static T MapDeserialize<T>(MySqlDataReader reader)
        {
            if (!IsOpen) return (T)Activator.CreateInstance(typeof(T));

            var dict = new Dictionary<string, object>();

            for (var i = 0; i < reader.FieldCount; i++)
                if (reader.GetValue(i).ToString().StartsWith("{") && reader.GetValue(i).ToString().EndsWith("}"))
                {
                    var dyn = JsonConvert.DeserializeObject(reader.GetValue(i).ToString());
                    dict.Add(reader.GetName(i), dyn);
                }
                else
                {
                    dict.Add(reader.GetName(i), reader.GetValue(i));
                }

            reader.Close();

            var json = JsonConvert.SerializeObject(dict);

            return JsonConvert.DeserializeObject<T>(json);
        }

        private static List<T> MapDeserializeMultiples<T>(MySqlDataReader reader)
        {
            if (!IsOpen) return new List<T>();

            var list = new List<Dictionary<string, object>>();

            while (reader.Read())
            {
                var dict = new Dictionary<string, object>();

                for (var i = 0; i < reader.FieldCount; i++)
                    if (reader.GetValue(i).ToString().StartsWith("{") && reader.GetValue(i).ToString().EndsWith("}"))
                    {
                        var dyn = JsonConvert.DeserializeObject(reader.GetValue(i).ToString());
                        dict.Add(reader.GetName(i), dyn);
                    }
                    else
                    {
                        dict.Add(reader.GetName(i), reader.GetValue(i));
                    }

                list.Add(dict);
            }

            reader.Close();

            var json = JsonConvert.SerializeObject(list);

            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        private static Dictionary<string, object> MapSerialize(object value)
        {
            if (!IsOpen) return new Dictionary<string, object>();

            var json = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }
    }
}

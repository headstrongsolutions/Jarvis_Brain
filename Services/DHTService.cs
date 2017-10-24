using Jarvis_Brain.Models;
using Jarvis_Brain.Code;
using System.Data;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;

namespace Jarvis_Brain.Services
{
    public class DHTService : IDHTService
    {
        ErrorFileLogger errorLog = new ErrorFileLogger("DHTService");
        
        string mysqlConnection = ConfigurationManager.ConnectionStrings["MySQL"].ToString();

        // TODO - CGMORSE - Need to refactor this to return a DHTCollection
        public DHTPackage GetLatestDHTPackage()
        {
            var dhtPackage = new DHTPackage();
            MySqlConnection connection = new MySqlConnection(mysqlConnection);
            try
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                // TODO - CGMORSE - The DB table name needs to be renamed to support DHT rather than DHT11
                cmd.CommandText = "SELECT * FROM DHT11 ORDER BY Received DESC LIMIT 1";
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adap.Fill(ds);
                
                dhtPackage = ds.Tables[0].AsEnumerable().Select(r => new DHTPackage
                {
                    Temperature = r.Field<float>("Temperature"),
                    Humidity = r.Field<float>("Humidity"),
                    BatteryLevel = r.Field<float>("BatteryLevel"),
                    Location = r.Field<string>("Location"),
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                // TODO - this needs to return out as a json object as get7days below
                errorLog.WriteToErrorLog(ex.Message);

                //errorLog.WriteToErrorLog(ex.Message);

                // returnableCollection.InternalComms = new InternalComms(){
                //     Name = "GetLast7DaysDHTPackage",
                //     Received = DateTime.Now,
                //     State = StateEnum.Error,
                //     Message = string.Format(ex.Message),
                //     ErrorType = ErrorType.Unknown
                // };

                // return returnableCollection;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return dhtPackage;
        }
        public DHTCollection GetLowestHighestTemperatureIn24Hours(string locationName)
        {
            var returnableCollection = new DHTCollection(){
                MaxMinTemps = new Dictionary<string, float>(),
                InternalComms = new InternalComms(){
                    Name = "GetLowestHighestTemperatureIn24Hours",
                    Received = DateTime.Now,
                    State = StateEnum.Sent,
                    Message = "Lowest and highest temperatures in the last 24 hours from the " + locationName
                }
            };
            MySqlConnection connection = new MySqlConnection(mysqlConnection);
            connection.Open();
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT MIN(Temperature) as minTemp, MAX(Temperature) as maxTemp FROM DHT11 WHERE Location = '" + locationName + "' AND Received >= now() - INTERVAL 1 DAY";
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnableCollection.MaxMinTemps.Add("min", float.Parse(reader["minTemp"].ToString()));
                    returnableCollection.MaxMinTemps.Add("max", float.Parse(reader["maxTemp"].ToString()));
                }

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                return returnableCollection;
            }
            catch (Exception ex)
            {
                errorLog.WriteToErrorLog(ex.Message);
                returnableCollection.InternalComms = new InternalComms(){
                    Name = "GetLowestHighestTemperatureIn24Hours",
                    Received = DateTime.Now,
                    State = StateEnum.Error,
                    Message = string.Format(ex.Message),
                    ErrorType = ErrorType.Unknown
                };
                return returnableCollection;
            }
        }

        public DHTCollection GetLast7DaysDHTPackage(string locationName)
        {
            var returnableCollection = new DHTCollection(){
                DHTPackages = new List<DHTPackage>(),
                InternalComms = new InternalComms(){
                    Name = "GetLast7DaysDHTPackage",
                    Received = DateTime.Now,
                    State = StateEnum.Sent,
                    Message = "Last 7 Days from the " + locationName
                }
            };

            try
            {
                MySqlConnection connection = new MySqlConnection(mysqlConnection);
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                // TODO - CGMORSE - The DB table name needs to be renamed to support DHT rather than DHT11
                cmd.CommandText = "SELECT * FROM DHT11 WHERE location = '" + locationName + "' ORDER BY Received DESC LIMIT 672";
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adap.Fill(ds);

                foreach (DataTable table in ds.Tables)
                {
                    foreach(DataRow row in table.Rows)
                    {
                        DHTPackage dhtPackage = new DHTPackage();
                        dhtPackage.Temperature = float.Parse(row["Temperature"].ToString());
                        dhtPackage.Humidity = float.Parse(row["Humidity"].ToString());
                        dhtPackage.BatteryLevel = float.Parse(row["BatteryLevel"].ToString());
                        dhtPackage.Location = row["Location"].ToString();
                        dhtPackage.Received = DateTime.Parse(row["Received"].ToString());

                        returnableCollection.DHTPackages.Add(dhtPackage);
                    }
                }

                
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                return returnableCollection;
            }
            catch (Exception ex)
            {
                errorLog.WriteToErrorLog(ex.Message);

                returnableCollection.InternalComms = new InternalComms(){
                    Name = "GetLast7DaysDHTPackage",
                    Received = DateTime.Now,
                    State = StateEnum.Error,
                    Message = string.Format(ex.Message),
                    ErrorType = ErrorType.Unknown
                };

                return returnableCollection;
            }
        }

        public int SaveDHTRecord(DHTPackage dhtPackage)
        {
            var result = 0;

            MySqlConnection connection = new MySqlConnection(mysqlConnection);
            MySqlCommand cmd;
            connection.Open();

            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO DHT11(Temperature,Humidity,BatteryLevel,Location,Received) VALUES(@Temperature,@Humidity,@BatteryLevel,@Location,@Received)";
                cmd.Parameters.AddWithValue("@Temperature", dhtPackage.Temperature);
                cmd.Parameters.AddWithValue("@Humidity", dhtPackage.Humidity);
                cmd.Parameters.AddWithValue("@BatteryLevel", dhtPackage.BatteryLevel);
                cmd.Parameters.AddWithValue("@Location", dhtPackage.Location);
                cmd.Parameters.AddWithValue("@Received", DateTime.Now);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errorLog.WriteToErrorLog(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    //LoadData();

                    result = 1;
                }
            }

            return result;
        }
    }
}

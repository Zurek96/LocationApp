using LocationApp.Interfaces;
using LocationApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace LocationApp.Services
{
    public class LocationService : ILocationService
    {
        public async Task<LocationModel> GetStart(string ApiKey)
        {
            var client = new HttpClient();
            var response = await client.GetAsync("http://api.ipstack.com/check?access_key="+ApiKey);
            var responseString = await response.Content.ReadAsStringAsync();
            var outputModel = new LocationModel();
            outputModel = JsonConvert.DeserializeObject<LocationModel>(responseString);
            return outputModel;
        }

        public string Add(LocationModel input, string ConnectionString)
        {
            var connection = new SqlConnection(ConnectionString);
            var adapter = new SqlDataAdapter();
            connection.Open();
            var command = "Insert into LocationData values ('" + input.ip + "','','" + input.country_name + "','" + input.city + "')";
            var cmd = new SqlCommand(command, connection);
            adapter.InsertCommand = new SqlCommand(command, connection);
            adapter.InsertCommand.ExecuteNonQuery();
            cmd.Dispose();
            connection.Close();
            return "Data added";
        }

        public List<LocationModel> GetList(string ConnectionString)
        {
            List<LocationModel> outputList = new List<LocationModel>();
            SqlDataReader reader;
            var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var command = "Select Id, Ip, CountryName, City from LocationData";
            var cdm = new SqlCommand(command, connection);
            reader = cdm.ExecuteReader();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    LocationModel model = new LocationModel()
                    {
                        id = reader.GetInt32(reader.GetOrdinal("Id")),
                        ip = reader.GetString(reader.GetOrdinal("Ip")),
                        country_name = reader.GetString(reader.GetOrdinal("CountryName")),
                        city = reader.GetString(reader.GetOrdinal("City"))
                    };
                    outputList.Add(model);
                }
            }
            connection.Close();
            return outputList;
        }

        public string Delete(string ConnectionString, int id)
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM LocationData WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connection.Close();
            return "Record deleted!";
        }

        public bool CheckDb(string ConnectionString)
        {
            try
            {
                var connection = new SqlConnection(ConnectionString);
                connection.Open();
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CheckApi(string ApiKey)
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync("http://api.ipstack.com/check?access_key=" + ApiKey);
                var responseString = await response.Content.ReadAsStringAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

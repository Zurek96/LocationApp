using LocationApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationApp.Interfaces
{
    public interface ILocationService
    {
        Task<LocationModel> GetStart(string ApiKey);
        string Add(LocationModel input, string ConnectionString);
        List<LocationModel> GetList(string ConnectionString);

        string Delete(string ConnectionString, int id);
        bool CheckDb(string ConnectionString);
        Task<bool> CheckApi(string ApiKey);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocationApp.Interfaces;
using LocationApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LocationApp.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;
        private IConfiguration _config;
        public LocationController(ILocationService locationService, IConfiguration configuration)
        {
            _config = configuration;
            _locationService = locationService;
        }
        public async Task<IActionResult> Index()
        {
            string apiKey = _config.GetSection("ApiKeys").GetSection("LocationApiKey").Value.ToString();
            string connString = _config.GetSection("DbConnection").GetSection("ConnectionString").Value.ToString();
            bool DbState = _locationService.CheckDb(connString);
            bool ApiState = await _locationService.CheckApi(apiKey);
            if (ApiState == true)
            {
                var viewModel = await _locationService.GetStart(apiKey);
                viewModel.ApiOk = ApiState;
                viewModel.DbOk = DbState;
                if (viewModel.DbOk == true)
                {
                    viewModel.list = _locationService.GetList(connString);
                }
                return View(viewModel);
            }
            else
            {
                return null;
            }
            
        }

        public RedirectToActionResult Post(string Ip, string Country, string City)
        {
            var input = new LocationModel()
            {
                ip = Ip,
                country_name = Country,
                city = City
            };
            string connString = _config.GetSection("DbConnection").GetSection("ConnectionString").Value.ToString();
            var response = _locationService.Add(input, connString);
            return RedirectToAction("Index");
        }

        public RedirectToActionResult Delete(int id)
        {
            string connString = _config.GetSection("DbConnection").GetSection("ConnectionString").Value.ToString();
            _locationService.Delete(connString, id);
            return RedirectToAction("Index");
        }
    }
}
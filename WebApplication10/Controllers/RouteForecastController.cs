using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApplication10.Models;

namespace WebApplication10.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RouteForecastController : ControllerBase
    {


        private readonly ILogger<RouteForecastController> _logger;

        public RouteForecastController(ILogger<RouteForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ResponseModel> Post(PostModel postmodel)
        {
            var robotsData = await GetRobotAPIData();
            var selectedIndex = 0;
            var seleactedDistnaceGoal = 0.0;
            for (int i = 0; i < robotsData.Count; i++)
            {
                var distanceEquation = Math.Round(Math.Sqrt(Math.Pow((postmodel.x - robotsData[i].x), 2) + Math.Pow((postmodel.y - robotsData[i].y), 2)), 2);
                if (i == 0)
                {
                    seleactedDistnaceGoal = distanceEquation;
                }
                else
                {
                    if (seleactedDistnaceGoal > distanceEquation)
                    {
                        selectedIndex = i;
                        seleactedDistnaceGoal = distanceEquation;
                    }
                    else if (seleactedDistnaceGoal == distanceEquation) //IF DISTANCES MATCHES CHECK BATTERY LEVEL 
                    {
                        if (robotsData[selectedIndex].batteryLevel < robotsData[i].batteryLevel)
                        {
                            selectedIndex = i;
                            seleactedDistnaceGoal = distanceEquation;
                        }
                    }
                }
            }

            return new ResponseModel() { batteryLevel = robotsData[selectedIndex].batteryLevel, DistanceToGoal = (decimal)seleactedDistnaceGoal, RobotId = robotsData[selectedIndex].RobotId };
        }


        private async Task<List<RestAPI>> GetRobotAPIData()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://60c8ed887dafc90017ffbd56.mockapi.io/robots");

            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


            HttpResponseMessage response = client.GetAsync("").Result;
            if (response.IsSuccessStatusCode)
            {

                var dataObjectsString = await response.Content.ReadAsStringAsync();
                // Parse the response body.
                var result = JsonConvert.DeserializeObject<List<RestAPI>>(dataObjectsString);
                return result;
            }

            return (new List<RestAPI>());
        }
    }
}

using AzureServices.Common.Models;
using AzureServices.CosmosDB;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServices.Driver.Controllers
{
    public class CosmosDBController : ControllerBase
    {
        private readonly DatabaseService databaseService;

        public CosmosDBController(DatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }


        [HttpGet]
        [Route("api/cosmosdb" + "/test")]
        public IActionResult Test()
        {
            return new OkResult();
        }

        [HttpGet]
        [Route("api/cosmosdb" + "/CreateFamilyDetails")]
        public async Task<IActionResult> CreateFamilyDetails()
        {
            Family singhFamily = new Family()
            {
                Id = "1",
                FamilyName = "Singh",
                Caste = "Hindu",
                Category = "General",
                Father = JsonConvert.SerializeObject(new Person() { FirstName = "R K", LastName = "Singh", AgeInYears = 59, Sex = "M", Education = "MA", City = "Siwan", Country = "India"}),
                Mother = JsonConvert.SerializeObject(new Person() { FirstName = "Gyanti", LastName = "Devi", AgeInYears = 54, Sex = "F", Education = "BA", City = "Siwan", Country = "India" }),
            };

            //await this.databaseService.CreateItem(singhFamily);
            //await this.databaseService.GetItemQueryIterator(singhFamily);

            //this.databaseService.GetItemLinqQueryable(singhFamily);

            //await this.databaseService.PatchItem(singhFamily);

            await databaseService.GetData();

            return new OkResult();
        }
    }
}
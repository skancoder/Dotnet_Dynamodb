﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieRank.Services;

namespace MovieRank.Controllers
{
    [Route("setup")]
    public class SetupController : Controller
    {
        private readonly ISetupService _setupService;

        public SetupController(ISetupService setupService)
        {
            _setupService = setupService;
        }

        [HttpPost]
        [Route("createTable/{dynamoDbTableName}")]
        public async Task<IActionResult> CreateDynamoDbTable(string dynamoDbTableName)
        {
            await _setupService.CreateDynamoDbTable(dynamoDbTableName);

            return Ok();
        }

        [HttpDelete]
        [Route("deleteTable/{dynamoDbTableName}")]
        public async Task<IActionResult> DeleteTable(string dynamoDbTableName)
        {
            await _setupService.DeleteDynamoDbTable(dynamoDbTableName);

            return Ok();
        }
    }
}
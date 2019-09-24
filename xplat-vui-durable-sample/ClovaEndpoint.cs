﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using XPlat.VUI.Models;

namespace XPlat.VUI.DurableSample
{
    public class ClovaEndpoint
    {
        private IDurableAssistant Assistant { get; }

        public ClovaEndpoint(IDurableAssistant assistant)
        {
            Assistant = assistant;
        }

        [FunctionName(nameof(ClovaEndpoint))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")]HttpRequest req,
            [DurableClient]IDurableOrchestrationClient starter,
            ILogger log)
        {
            try
            {
                Assistant.DurableOrchestrationClient = starter;
                Assistant.Logger = log;

                var response = await Assistant.RespondAsync(req, Platform.Clova);
                return new OkObjectResult(response.ToClovaResponse());
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                log.LogError(ex.StackTrace);
                return new NotFoundObjectResult("error");
            }
        }
    }
}

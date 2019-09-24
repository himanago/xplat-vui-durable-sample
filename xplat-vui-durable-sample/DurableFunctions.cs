using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XPlat.VUI.DurableSample
{
    public static class DurableFunctions
    {
        [FunctionName("XPlatDurableFunction_RunOrchestrator")]

        public async static Task<string> RunOrchestrator(
            [OrchestrationTrigger]IDurableOrchestrationContext context)
        {
            var input = context.GetInput<int>();
            var output = await context.CallActivityAsync<string>("XPlatDurableFunction_LongTimeActivity", input);

            return output;
        }

        [FunctionName("XPlatDurableFunction_LongTimeActivity")]
        public async static Task<string> LongTimeActivity([ActivityTrigger]int count, ILogger log)
        {
            log.LogInformation($"カウント開始: {count}");

            for (var i = 0; i < count; i++)
            {
                log.LogInformation($"count: {i + 1}");
                await Task.Delay(1000);
            }

            return $"カウント終了: {count}";
        }

    }
}

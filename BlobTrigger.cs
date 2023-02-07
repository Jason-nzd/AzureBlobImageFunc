using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Jason.ImageFunc
{
    public class BlobTrigger
    {
        [FunctionName("BlobTrigger")]
        public void Run([BlobTrigger("countdownimages/{name}", Connection = "supermarketpricewatch_STORAGE")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}

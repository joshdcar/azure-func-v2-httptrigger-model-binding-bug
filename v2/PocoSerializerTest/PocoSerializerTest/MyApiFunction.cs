using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PocoSerializerTest
{

    public class MyParentClass
    {
        [JsonProperty("myProperty")]
        public string MyProperty { get; set; }

        [JsonProperty("singleProperty")]
        public MyCollectionClass SingleProperty { get;set; }

        // Same results with  [] amd IEnumerable and with newing up the collection 
        // in a constructor
        [JsonProperty("myCollectionProperty")]
        public List<MyCollectionClass> MyCollectionProperty { get; set; }
    }

    public class MyCollectionClass
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public static class MyApiFunction
    {

        // execute CURL to test
        // curl --header "Content-Type: application/json" \
        //  --request POST \
        //  --data '{"myProperty": "Test1","singleProperty": {"id": 2,"name": "Test2"},"myCollectionProperty": [{"id": 1,"name": "Test1"},{"id": 2,"name": "Test2"}]}' \
        //  http://localhost:7071/api/MyApiFunction


        [FunctionName("MyApiFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]MyParentClass poco,
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //Manually Deserialize
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            MyParentClass data = JsonConvert.DeserializeObject<MyParentClass>(requestBody);

            log.LogInformation($"Manually Deserialize Collection Count: {data.MyCollectionProperty.Count}");

            //Function Deserialize
            if(poco.MyCollectionProperty == null)
            {
                log.LogInformation($"Collection is null!");
            }
            else
            {
                log.LogInformation($"Function Deserialize Collection Count: {poco.MyCollectionProperty.Count}");
            }
            

            return new OkObjectResult(data);

        }
    }
}

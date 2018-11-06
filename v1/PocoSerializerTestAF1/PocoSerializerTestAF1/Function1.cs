using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace PocoSerializerTestAF1
{
    public static class Function1
    {
        public class MyParentClass
        {

            [JsonProperty("myProperty")]
            public string MyProperty { get; set; }

            [JsonProperty("singleProperty")]
            public MyCollectionClass SingleProperty { get; set; }

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

        [FunctionName("MyApiFunctionV1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]MyParentClass poco, 
            TraceWriter log, HttpRequestMessage req)
        {
            log.Info("C# HTTP trigger function processed a request.");

            //Manually Deserialize
            MyParentClass data = await req.Content.ReadAsAsync<MyParentClass>();
            log.Info($"Manually Deserialize Collection Count: {data.MyCollectionProperty.Count}");
           
            //Function Deserialize
            if(poco.MyCollectionProperty == null)
            {
                log.Info($"My Collection is null!");
            }
            else
            {
                log.Info($"Function Deserialize Collection Count: {poco.MyCollectionProperty.Count}");
            }
            

            return req.CreateResponse(HttpStatusCode.OK);


        }
    }
}

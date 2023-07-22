using System;
using Microsoft.Azure.Cosmos;
using System.Security.Policy;

namespace tinyurl_2.Controllers
{
	public class CosmosDBClient: IDBClient
	{
        private Container container;

        private string connString = Secrets.COSMOS_CONN_STRING;
        private string dbId = "tinyurlstore";
        private string containerId = "urltocode";

		public CosmosDBClient()
		{
            CosmosClient cosmosClient = new CosmosClient(connString);
            this.container = cosmosClient.GetContainer(dbId, containerId);
        }

		async public Task<KeyValue> put(string url, string code)
		{
            ItemResponse<KeyValue> response;
            var urlCodePair = new KeyValue()
            {
                url = url,
                code = code
            };
            response = await this.container.CreateItemAsync<KeyValue>(urlCodePair, new PartitionKey(code));
            return response;
        }

        async public Task<KeyValue> get(string code)
        {
            try
            {
                ItemResponse<KeyValue> response;
                response = await this.container.ReadItemAsync<KeyValue>(code, new PartitionKey(code));
                return response;
            } catch(CosmosException c)
            {
                if(c.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new KeyValue
                    {
                        url = null,
                        code = code
                    };
                }
                throw c;
            }
        }
    }
}


using System;
namespace tinyurl_2.Controllers
{
	public interface IDBClient
	{
        public Task<KeyValue> put(string url, string code);
        public Task<KeyValue> get(string url);
    }
}


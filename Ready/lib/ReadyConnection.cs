using System;
using System.Net.Http;

namespace Ready
{
    public class ReadyConnection
    {
        public int ApplicationId {get;}
        public string BaseUrl {get;}
        public HttpClientHandler clientHandler {get;}


        public ReadyConnection(int appId, string baseUrl)
        {
            this.ApplicationId = appId;
            this.BaseUrl = baseUrl;
            clientHandler = new HttpClientHandler();
            clientHandler.UseDefaultCredentials = false;
        }

    }
}

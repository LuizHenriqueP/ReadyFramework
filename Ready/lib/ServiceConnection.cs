using System;
using Ready.Models;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.Http.Extensions;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using System.Text;


namespace Ready
{
    public abstract class ServiceConnection
    {
        private int ApplicationId;
        protected HttpClient client;

        public ServiceConnection()
        {
        }

        protected void init(ReadyConnection conn){
            this.ApplicationId = conn.ApplicationId;
            this.client = new HttpClient(conn.clientHandler);
            client.BaseAddress = new Uri(conn.BaseUrl);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        }


        protected ReadyDataPackage assembleDataPackage<T>(T data){
            var dataPackage = new ReadyDataPackage(ApplicationId);
            dataPackage.serializeData<T>(data);
            return dataPackage;
        }

        protected async Task<T> sendPackage<T>(string url, ReadyDataPackage package){
                var content = new StringContent(JsonConvert.SerializeObject(package), Encoding.UTF8,"application/json");
                HttpResponseMessage res = await client.PostAsync(url,content);
                if(res.IsSuccessStatusCode){
                       try{  

                           return await res.Content.ReadAsAsync<T>();
                       } catch(Exception ex){
                           
                        Console.WriteLine(ex);
                        
                        throw ex;
                       }

                }
                else{
                    Console.WriteLine(res.StatusCode);
                    return await res.Content.ReadAsAsync<T>();

                }

        }

    }
}

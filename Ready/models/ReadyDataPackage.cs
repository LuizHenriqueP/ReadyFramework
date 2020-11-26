using System;
using Newtonsoft.Json;

namespace Ready.Models{

    public class ReadyDataPackage{
        public int AppId {get;set;}
        public string Data {get;set;}

        public T getDataAsType<T> (){
            return (T) Convert.ChangeType(Data,typeof(T));
        }

        public T getDataAsObject<T>(){
          return  JsonConvert.DeserializeObject<T>(Data);
        }

        public void serializeData<T>(T Data){

            if(typeof(T) == typeof(string)){
                this.Data = Data.ToString();
            }
            else{

                this.Data = JsonConvert.SerializeObject(Data);

            }
        }

 
        public ReadyDataPackage(int id){
            AppId = id;
        }
    }
}
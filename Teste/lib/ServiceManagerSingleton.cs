using System;
using Whitebox.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Whitebox{
    public class ServiceManagerSingleton{
       private static ServiceManagerSingleton instance;

        private List<Service> serviceList = new List<Service>();
 
        private ServiceManagerSingleton() {
            
         }
 
        public static ServiceManagerSingleton Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(ServiceManagerSingleton))
                        if (instance == null) instance = new ServiceManagerSingleton();
 
                return instance;
            }
        }

        public bool logIn(Service service){
            if(!serviceList.Exists(x => x.GetName().ToLower().Trim() == service.GetName().ToLower().Trim())){
                serviceList.Add(service);
                return true;
            }
            else{
                return false;      
            }
            
        }

        public void logOut(Service service){
            serviceList.Remove(service);
        }


    }

}
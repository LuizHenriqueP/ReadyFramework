using System;
using Whitebox.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Whitebox{
    public class DaemonManagerSingleton{
       private static DaemonManagerSingleton instance;

        private List<Daemon> daemonList = new List<Daemon>();
 
        private DaemonManagerSingleton() {
            
         }
 
        public static DaemonManagerSingleton Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(DaemonManagerSingleton))
                        if (instance == null) instance = new DaemonManagerSingleton();
 
                return instance;
            }
        }

        public bool logIn(Daemon daemon){
            if(!daemonList.Exists(x => x.Name.ToLower().Trim() == daemon.Name.ToLower().Trim())){
                daemonList.Add(daemon);
                return true;
            }
            else{
                return false;      
            }
            
        }

        public void logOut(Daemon daemon){
            daemonList.Remove(daemon);
        }

        public void getDaemonReference(string daemonName, out Daemon daemon){
            daemon = daemonList.Find(x => x.Name == daemonName);
        }


    }

}
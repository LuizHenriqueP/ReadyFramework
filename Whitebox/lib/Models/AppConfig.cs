using System;

namespace Whitebox.Models{

    public class AppConfig{
        public string AppName {get;set;}
        public string AppType {get;set;}
        public string [] InvokeCommands {get;set;}
        public string [] InternalCommands {get;set;}        

        public AppConfig(){}
    }
}
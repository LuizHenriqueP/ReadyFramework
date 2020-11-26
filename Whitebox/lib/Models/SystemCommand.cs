using System;
using Newtonsoft.Json;

namespace Whitebox.Models{

    public class SystemCommand{
        
        public int Id {get;set;}
        public string Command {get;set;}
        public string Action {get;set;}

        public SystemCommand(){


        }
        public SystemCommand(int id, string command, string action){
            this.Id =id ;
            this.Command = command;
            this.Action = action;

        }

    }
}
using System;
using Newtonsoft.Json;

namespace Whitebox.Models{

    public class Application{
        
        public enum FocusStatus {Unset,Pending,Executing,Finished,Locked}
        public int Id {get;set;}

        public string Name {get;set;}
        public DateTime StartTime {get;set;}

        public DateTime LastStatusChange {get;set;}
        
        public FocusStatus focusStatus =  FocusStatus.Unset;
        public Application(){}

        public Application(string name, int id){
            this.Name = name;
            this.Id = id;
            StartTime = DateTime.UtcNow;

        }
        
    }
}
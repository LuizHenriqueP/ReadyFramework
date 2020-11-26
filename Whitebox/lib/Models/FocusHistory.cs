using System;
using Newtonsoft.Json;

namespace Whitebox.Models{

    public class FocusHistory{
        
        public int Id {get;set;}

        public int ApplicationId {get;set;}
        public DateTime Timestamp {get;set;}

        public FocusHistory(){}

        public FocusHistory(int id, int applicationId){

            this.Id = id;
            this.ApplicationId = applicationId;
            Timestamp = DateTime.UtcNow;

        }
        
    }
}
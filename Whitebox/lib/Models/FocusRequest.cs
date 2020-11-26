using System;
using Newtonsoft.Json;

namespace Whitebox.Models{

    public class FocusRequest{
        
        public int Id {get;set;}

        public int ApplicationId {get;set;}
        public DateTime Timestamp {get;set;}

        public FocusRequest(){}

        public FocusRequest(int id, int applicationId){

            this.Id = id;
            this.ApplicationId = applicationId;
            Timestamp = DateTime.UtcNow;

        }
        
    }
}
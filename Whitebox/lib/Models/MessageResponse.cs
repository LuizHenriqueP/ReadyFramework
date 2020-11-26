using System;

namespace Whitebox.Models{

    public class MessageResponse{
        public UInt64 MessageId {get;set;}
        public string Response  {get;set;} = null;

        public MessageResponse(UInt64 messageId){
            this.MessageId = messageId;
        }

        public MessageResponse(UInt64 messageId, string response){
            this.MessageId = messageId;
            this.Response = response;
        }
    }
}
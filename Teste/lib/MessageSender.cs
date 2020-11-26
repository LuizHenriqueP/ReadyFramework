using System;
using Whitebox.Models;

namespace Whitebox{
    public class MessageSender{

        public event Action<Message> send;
    
        public MessageSender(){
            
        }

        public void sendMessage(Message message){
            send(message);
        }

     
    }

}
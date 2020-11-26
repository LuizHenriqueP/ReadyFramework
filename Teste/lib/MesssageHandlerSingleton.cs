using System;
using Whitebox.Models;

namespace Whitebox{
    public class MesssageHandlerSingleton{
       private static MesssageHandlerSingleton instance;

       private MessageSender messageSender;
 
        private MesssageHandlerSingleton() {
            this.messageSender = new MessageSender();
         }
 
        public static MesssageHandlerSingleton Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(MesssageHandlerSingleton))
                        if (instance == null) instance = new MesssageHandlerSingleton();
 
                return instance;
            }
        }

        public ref MessageSender getMessageSender(){

            return ref this.messageSender;
        }

        public void sendMessage(ref Message message){
            if(message != null){
                messageSender.sendMessage(message);
                message.Dispose();
            }
            
        }

    }

}
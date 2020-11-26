using System;
using System.Collections.Generic;
using System.Threading;

namespace Whitebox.Models{
    public abstract class Service:IDisposable{

        private const int QUEUE_SIZE = 256; 

        private static UInt32 ServiceCount;
        private static string CurrentServiceName = "";
        protected Queue<Message> messageQueue = new Queue<Message>(QUEUE_SIZE); 

        protected bool _disposed = false;
        private UInt32 Id {get;}
        private bool online = false;
        private bool whitelistFlag = false;
        private List<string> whitelist = new List<string>();
        private string name;
        private string currentApp;

        public string GetName()
        {
            return name;
        }

        private MesssageHandlerSingleton messsageHandler = MesssageHandlerSingleton.Instance;
        private ServiceManagerSingleton serviceManager = ServiceManagerSingleton.Instance;
        public Service(){
            
            ServiceCount++;
            
            Id = ServiceCount; 
            messsageHandler.getMessageSender().send += receiveMessage;     
        }

        protected void init(string name){
            Console.WriteLine(name+ ": Nasci");
            this.name = name;
            online = serviceManager.logIn(this);
            if(!online){
                //criar exception
                Console.WriteLine(name+ ": Morri");

                this.Dispose();
            }
            else{
                Console.WriteLine(name+ ": To online");
                Thread serviceThread = new Thread(new ThreadStart(Run));
                serviceThread.Start();
            }
                        
        }

        ~Service(){
            Dispose(false);                     
        }
        //this function receives messages and checks if it belongs to the service
        //also checks if the service has a whitelist and if the source is in it
        protected void receiveMessage(Message message){
            if(message.Destination == this.GetName()){
                if(whitelistFlag){                    
                    if(whitelist.Exists(x => x == message.Source)){
                        if(messageQueue.Count == QUEUE_SIZE){
                          messageQueue.Dequeue().Dispose();                   
                        }
                        messageQueue.Enqueue(message);
                    }
                }
                else { 
                    if(messageQueue.Count == QUEUE_SIZE){
                        messageQueue.Dequeue().Dispose();                   
                    }
                    messageQueue.Enqueue(message);
                }
                                                
            }
            else if(message.Destination == "Broadcast" && message.Source == "AppManager") {
                switch(message.Command){
                    case "setCurrentApp":
                        currentApp = message.Parameters[0];
                        break;
                }
            }
        }

        protected void setCurrentApp(string appName){
            if(this.name == "AppManager"){
                this.currentApp = appName;
            }
        }

        protected void clearCurrentApp(){
            if(this.name == "AppManager"){
                this.currentApp = "";
            }
            
        }

        protected void enableWhitelist(){
            this.whitelistFlag = true;
        }
        protected void disableWhitelist(){
            this.whitelistFlag = false;
        }

        protected bool isWaitlistActive(){
            return this.whitelistFlag;
        }

        protected string getCurrentApp(){
            return currentApp;
        }

        protected void removeFromWhitelist(string serviceName){
            var index = whitelist.IndexOf(serviceName);
            if(index > -1){
                whitelist.RemoveAt(index);
            }
        }

        protected void addToWhitelist(string serviceName){
            var exists = whitelist.Exists(x => x == serviceName);
            if(!exists){
                whitelist.Add(serviceName);
            }
        }

        protected void sendMessage(string destination, string command, string [] parameters = null){
            var message = new Message(this.GetName(), destination,command,parameters);
            messsageHandler.sendMessage(ref message);

        }
        public void Dispose(){  
            Dispose(true);
            // any other managed resource cleanups you can do here
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
            if (disposing){
                if(online){
                    serviceManager.logOut(this);
                }  
            }

            _disposed = true;
            }

        }

        protected void clearMessageBuffer(){
            messageQueue.Clear();
        }

        protected abstract void Run();


    }

}
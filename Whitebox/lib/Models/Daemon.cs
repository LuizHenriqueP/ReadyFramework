using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Newtonsoft.Json;

namespace Whitebox.Models{
    public abstract class Daemon:IDisposable{

        private const int QUEUE_SIZE = 256; 

        private static UInt32 DaemonCount;
        private static string CurrentDaemonName = "";
        protected Queue<Message> messageQueue = new Queue<Message>(QUEUE_SIZE);
        protected List<MessageResponse> responseBuffer = new List<MessageResponse>();
        protected List<Thread> threads = new List<Thread>();

        protected bool _disposed = false;
        private UInt32 Id {get;}
        private bool online = false;
        private bool whitelistFlag = false;
        private List<string> whitelist = new List<string>();
        private string name;
        private Application currentApp;
        private static object mutex = new object();


        public string Name{get{
            return this.name;

        }}

        private MesssageHandlerSingleton messsageHandler = MesssageHandlerSingleton.Instance;
        private DaemonManagerSingleton daemonManager = DaemonManagerSingleton.Instance;
        public Daemon(){
            lock(mutex){
              DaemonCount++;
            }
            
            Id = DaemonCount; 
            messsageHandler.getMessageSender().send += receiveMessage;     
        }

        protected void init(string name){
            Console.WriteLine(name+ ": Nasci");
            this.name = name;
            online = daemonManager.logIn(this);
            if(!online){
                //criar exception
                Console.WriteLine(name+ ": Morri");

                this.Dispose();
            }
            else{
                Console.WriteLine(name+ ": To online");
                Thread daemonThread = new Thread(new ThreadStart(Run));
                daemonThread.Start();
            }
                        
        }

        ~Daemon(){
            Dispose(false);                     
        }
        //this function receives messages and checks if it belongs to the service
        //also checks if the service has a whitelist and if the source is in it
        protected void receiveMessage(Message message){
            if(message.Destination == this.Name){
                if(whitelistFlag){                    
                    if(whitelist.Exists(x => x == message.Source)){
                        if (message.Command == "Response"){
                            var response = new MessageResponse(message.Response,message.Parameters[0]);
                            responseBuffer.Add(response);
                        }
                        else {
                            if(messageQueue.Count == QUEUE_SIZE){
                                messageQueue.Dequeue().Dispose();                   
                            }
                            messageQueue.Enqueue(message);
                        }
                       
                    }
                }             
                else {
                    if (message.Command == "Response"){
                        var response = new MessageResponse(message.Response,message.Parameters[0]);
                        responseBuffer.Add(response);
                    }
                    else{
                       if(messageQueue.Count == QUEUE_SIZE){
                             messageQueue.Dequeue().Dispose();                   
                         }
                        messageQueue.Enqueue(message);     
                    }
                   
                }
                                                
            }
            else if(message.Destination == "Broadcast" && message.Source == "AppManager" && this.Name != "AppManager") {
                switch(message.Command){
                    case "setCurrentApp":
                        currentApp = JsonConvert.DeserializeObject<Application>(message.Parameters[0]);
                        break;
                    case "clearCurrentApp":
                        currentApp = null;
                        break;
                    case "updateCurrentAppStatus":
                        currentApp.focusStatus = JsonConvert.DeserializeObject<Application.FocusStatus>(message.Parameters[0]);
                        break;    
                }
            }
            
        }

        protected void setCurrentApp(Application app){
            if(this.name == "AppManager"){
                app.focusStatus = Application.FocusStatus.Pending;
                app.LastStatusChange = DateTime.UtcNow;
                this.currentApp = app;
                sendMessage("Broadcast","setCurrentApp",new string []{
                JsonConvert.SerializeObject(app)
                });
            }
        }
        protected void updateCurrentAppStatus(Application.FocusStatus status){
             if(this.name == "AppManager"){
                currentApp.focusStatus = status;
                sendMessage("Broadcast","updateCurrentAppStatus", new string []{
                    JsonConvert.SerializeObject(status)
                });
             }
        }

        protected void clearCurrentApp(){
            if(this.name == "AppManager"){
                this.currentApp = null;             
                sendMessage("Broadcast","clearCurrentApp",new string []{});
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

        protected Application getCurrentApp(){
            return currentApp;
        }

        protected void removeFromWhitelist(string daemonName){
            var index = whitelist.IndexOf(daemonName);
            if(index > -1){
                whitelist.RemoveAt(index);
            }
        }

        protected void addToWhitelist(string daemonName){
            var exists = whitelist.Exists(x => x == daemonName);
            if(!exists){
                whitelist.Add(daemonName);
            }
        }

        protected UInt64 sendMessage(string destination, string command, string [] parameters = null){
            var message = new Message(this.Name, destination,command,parameters);
            messsageHandler.sendMessage(ref message);
            return message.Id;

        }

        protected string sendMessageAndWait(string destination, string command, string [] parameters = null){
            var message = new Message(this.Name, destination,command,parameters);
            messsageHandler.sendMessage(ref message);
            while(!this.responseBuffer.Any(x => x.MessageId == message.Id && x.Response != null)){
                Thread.Sleep(1);
            }
            var response = responseBuffer.Where(x => x.MessageId == message.Id).FirstOrDefault().Response;
            responseBuffer.Remove(responseBuffer.Find(x => x.MessageId == message.Id));
            return response;
        }

        protected void sendResponse(Message oldMessage,string [] parameters = null){
            var message = new Message(oldMessage,"Response",parameters);
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
                    daemonManager.logOut(this);
                }  
            }

            _disposed = true;
            }

        }

        protected void clearMessageBuffer(){
            messageQueue.Clear();
        }

        protected void abortAllThreads(){

            foreach (var item in threads)
            {
                item.Abort();
                threads.Remove(item);
            }
        }

        protected void abortThread(string name){
            var thread = threads.Find(t => t.Name.ToLower() == name.ToLower());
            if(thread != null){
                thread.Abort();
                threads.Remove(thread);
            }
        }

        public bool isCurrentApplication(int applicationId){
            if(currentApp?.Id == applicationId){
                return true;
            }
            return false;
        }

        protected abstract void Run();



    }

}
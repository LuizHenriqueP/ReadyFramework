using System;

namespace Whitebox.Models{
    public class Message: IDisposable{

        public string Source {get;}
        public string Destination{get;}
        public string Command{get;}
        public string [] Parameters {get;}
        public UInt64 Response {get;}
        private bool _disposed = false;
        private static UInt64 count = 0;
        public UInt64 Id {get;}
        private static object mutex = new object();


        
        
        public Message(string source,string destination, string command, string [] parameters = null){
            lock(mutex){
                count++;
            }
            this.Id = count;
            this.Source = source;
            this.Destination = destination;
            this.Command = command;
            this.Parameters = parameters;
        }
        public Message(Message oldMessage, string command, string [] parameters = null){
            lock(mutex){
                count++;
            }
            this.Id = count;
            this.Source = oldMessage.Destination;
            this.Destination = oldMessage.Source;
            this.Command = command;
            this.Parameters = parameters;
            this.Response = oldMessage.Id;
        }
        public void Dispose(){  
            Dispose(true);
            // any other managed resource cleanups you can do here
            GC.SuppressFinalize(this);
        }
        ~Message()      // finalizer
        {
                Dispose(false);
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
            if (disposing){
            }

            _disposed = true;
            }

        }


    }

}
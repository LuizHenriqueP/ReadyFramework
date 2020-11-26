using System;

namespace Whitebox.Models{
    public class Message: IDisposable{

        public string Source {get;}
        public string Destination{get;}
        public string Command{get;}
        public string [] Parameters {get;}
        private bool _disposed = false;

        
        
        public Message(string source,string destination, string command, string [] parameters = null){
            this.Source = source;
            this.Destination = destination;
            this.Command = command;
            this.Parameters = parameters;
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
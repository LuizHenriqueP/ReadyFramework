using System;
using System.Collections.Generic;
using System.Threading;

namespace Whitebox.Models{
    public class Communication :Service{

        

        public Communication(string name){
            base.init(name);
        }

        ~Communication(){
            Dispose(false);                     
        }
        protected override void Run(){
            while(true){
                if(messageQueue.Count > 0){
                    var message = messageQueue.Dequeue();

                    Console.WriteLine("Recebi de "+ message.Source+": "+message.Command);
                }

                Thread.Sleep(1100);
            }

        }


    }

}
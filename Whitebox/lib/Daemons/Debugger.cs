using System;
using System.Collections.Generic;
using System.Threading;
using Whitebox.Models;

namespace Whitebox.Daemons{
    public class Debugger:Daemon{
     
        public Debugger(string name){
            base.init(name);          
        }

        ~Debugger(){
            Dispose(false);                     
        }
     
         protected override void Run(){
             var i = 0;
            while(true){
                Console.WriteLine("To Esperando");
                Thread.Sleep(2000);
                if(i < 1){
                    sendMessage("AppManager","openApp",new string []{"armario"});
                    Console.WriteLine("Enviei");
                }


                //Console.WriteLine("Enviei :"+i);
                i++;

            }

        }



    }

}
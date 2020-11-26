using System;
using System.Threading;
using Whitebox.Models;

namespace Whitebox
{
    class Program
    {
        static void Main(string[] args)
        {
            var messsageHandler = MesssageHandlerSingleton.Instance;
            var serviceManager = ServiceManagerSingleton.Instance;
            Console.WriteLine("Hello World!");

            AppManager manager = new AppManager("AppManager");

           // Communication communication = new Communication("Communication");
            
            Debugger debugger = new Debugger("Debugger");
           // Debugger bugado = new Debugger("bugado");

            

           
            while(true){

            }

           

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Whitebox.Models;

namespace Whitebox.Controllers
{
    public class ReadyController : ControllerBase
    {

    private DaemonManagerSingleton daemonManager;
    protected Daemon currentDaemon;

    public ReadyController(){
       daemonManager = DaemonManagerSingleton.Instance;
    }

    [NonAction]
    protected void connectToDaemon(string daemonName){
        daemonManager.getDaemonReference(daemonName, out currentDaemon);
        Console.WriteLine(currentDaemon.Name);
    }    

    [NonAction]
    protected ref Daemon summonDaemon(){
        return ref currentDaemon;
    }

    }
}

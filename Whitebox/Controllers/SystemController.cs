using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Whitebox.Models;
using Microsoft.Extensions.Configuration;
using Whitebox.Daemons;
using Newtonsoft.Json;

namespace Whitebox.Controllers
{
    public class SystemController : ReadyController
    {

    public SystemController(IConfiguration config)
      {
          base.connectToDaemon("AppManager");
      }


    [HttpPost]
    [Produces("application/json")]
    [Route("api/System/RequestFocus")]
      public ActionResult RequestFocus([FromBody] ReadyDataPackage package ){
        (currentDaemon as AppManager).newFocusRequest(package.AppId);
         return Ok("Focus Request Added");
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/System/SetAsBackground")]
      public ActionResult SetAsBackground([FromBody] ReadyDataPackage package ){
        var response = (currentDaemon as AppManager).setAsBackground(package.AppId);
         return Ok(response);
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/System/LockFocus")]
      public ActionResult LockFocus([FromBody] ReadyDataPackage package ){
        (currentDaemon as AppManager).setFocusLock(package.AppId);
         return Ok("Focus Locked");
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/System/UnlockFocus")]
      public ActionResult UnlockFocus([FromBody] ReadyDataPackage package ){
        (currentDaemon as AppManager).disableFocusLock(package.AppId);
         return Ok("Focus unlocked");
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/System/IsCurrentApplication")]
      public  ActionResult IsCurrentApplication([FromBody] ReadyDataPackage package ){
        var response = currentDaemon.isCurrentApplication(package.AppId);
        return Ok(JsonConvert.SerializeObject(response));
     }

     [HttpPost]
    [Produces("application/json")]
    [Route("api/System/CloseApplication")]
      public  ActionResult CloseApplication([FromBody] ReadyDataPackage package ){
        var response = (currentDaemon as AppManager).closeApplication(package.AppId);
        return Ok(JsonConvert.SerializeObject(response));
     }


    
     

    }
}

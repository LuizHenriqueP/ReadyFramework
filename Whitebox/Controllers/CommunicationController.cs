using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Whitebox.Models;
using Whitebox.Daemons;
using Microsoft.Extensions.Configuration;

namespace Whitebox.Controllers
{
    public class CommunicationController : ReadyController
    {
      public CommunicationController(IConfiguration config)
      {
          base.connectToDaemon("Communication");
      }
    [HttpPost]
    [Produces("application/json")]
    [Route("api/Communication/WaitForWord")]
     public ActionResult WaitForWord( [FromBody] ReadyDataPackage package,[FromQuery] int seconds,  [FromQuery] bool isBoolean ){
        var word = package.getDataAsType<string>();
        if(isBoolean){
          var result = (currentDaemon as Communication).waitForWordBoolean(word, seconds);
        return Ok(result);
        }
        else{
          var result = (currentDaemon as Communication).waitForWord(word, seconds, package.AppId);
        return Ok(result);

        }
        
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/Communication/WordWasSaid")]
      public ActionResult WordWasSaid([FromBody] ReadyDataPackage package){
         var word = package.getDataAsType<string>();
         var result = (currentDaemon as Communication).wordWasSaid(word);
         return Ok(result);
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/Communication/GetLatestWords")]
      public ActionResult GetLatestWords([FromBody] ReadyDataPackage package){
         //pegar valor int com o numero de palavras
         var numberOfWords = package.getDataAsType<int>();
         var result = (currentDaemon as Communication).getLatestWords(numberOfWords);
         return Ok(result);
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/Communication/ListenWordsForSeconds")]
      public ActionResult ListenWordsForSeconds([FromBody] ReadyDataPackage package){
         //pegar valor int com o tempo em segundos
         var result = (currentDaemon as Communication).listenWordsForSeconds(package.getDataAsType<int>());
         return Ok(result);
     }
     [HttpPost]
     [Produces("application/json")]
    [Route("api/Communication/WaitForCommand")]
     public ActionResult WaitForCommand([FromBody] ReadyDataPackage package, [FromQuery] int seconds, [FromQuery] bool isBoolean ){
       if(isBoolean){
        bool result = (currentDaemon as Communication).waitForCommandBoolean(package.getDataAsType<string>(), seconds, package.AppId);
         return Ok(result);

       }
       else{
          List<VoiceWord> result = (currentDaemon as Communication).waitForCommand(package.getDataAsType<string>(), seconds, package.AppId);
          return Ok(result);
       }
      
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/Communication/CommandWasSaid")]
      public ActionResult CommandWasSaid([FromBody] ReadyDataPackage package){
        var result = (currentDaemon as Communication).commandWasSaid(package.getDataAsType<string>(),package.AppId);
         return Ok(result);
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/Communication/GetLatestCommands")]
      public ActionResult GetLatestCommands([FromBody] ReadyDataPackage package){
         //pegar valor int com o numero de palavras
         var result = (currentDaemon as Communication).getLatestCommands(package.getDataAsType<int>(),package.AppId);
         return Ok(result);
     }

     [HttpPost]
     [Produces("application/json")]
    [Route("api/Communication/ListenCommandsForSeconds")]
      public ActionResult ListenCommandsForSeconds([FromBody] ReadyDataPackage package){
          //pegar valor int com o tempo em segundos
         var result = (currentDaemon as Communication).listenCommandsForSeconds(package.getDataAsType<int>(),package.AppId);
         return Ok(result);
     }

     [HttpPost]
     [Produces("application/json")]
    [Route("api/Communication/Speak")]
      public ActionResult Speak( [FromBody]  ReadyDataPackage package){
          var result = (currentDaemon as Communication).speak(package.getDataAsType<string>());
         return Ok(result);
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/Communication/SetEmotion")]
      public ActionResult SetEmotion([FromBody] ReadyDataPackage package){
          var result = (currentDaemon as Communication).setEmotion(package.Data);
         return Ok(result);
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/Communication/GetEmotion")]
      public ActionResult GetEmotion([FromBody] ReadyDataPackage package){
           var result = (currentDaemon as Communication).getEmotion();
         return Ok(result);
     }
    }
}

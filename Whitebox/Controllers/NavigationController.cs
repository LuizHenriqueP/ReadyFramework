using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Whitebox.Models;
using Microsoft.Extensions.Configuration;
using Whitebox.Daemons;

namespace Whitebox.Controllers
{
    public class NavigationController : ReadyController
    {
    //Type1: Parametros -> Angulo e velocidade; Faz o robo andar em linha reta no angulo referente a ele
    //Type2: Parametros -> Angulo, velocidade e distancia; Faz o robo andar em linha reta no angulo referente a ele por uma distancia
    //Type3: Parametros ->  Angulo, velocidade e Angulo de apontamento;  Faz o robo andar em linha reta no angulo referente a ele apontando para um certo angulo
    //Type4: Parametros ->  Angulo, velocidade e Ponto de apontamento;  Faz o robo andar em linha reta no angulo referente a ele apontando para um ponto no espaço
    public enum WalkStraightType {Type1,Type2,Type3,Type4}
    
    
    //Type1: Parametros -> Vel. Lin.  e Vel. Ang. ; Faz o robo andar somand movimento angular e linear
    //Type2: Parametros -> Vel. Lin. e Raio; Faz o robo andar em torno de um ponto baseado no raio
    //Type3: Parametros ->  Vel. Lin. , Raio e  Ponto de apontamento;  
    public enum WalkCurvedType {Type1,Type2,Type3}

    //Type1: Parametros -> Angulo ;
    //Type2: Parametros -> Angulo e altura;
    //Type3: Parametros ->  Waypoint ;  
    public enum DistanceType {Type1,Type2,Type3}

    public NavigationController(IConfiguration config)
      {
          base.connectToDaemon("Navigation");
      }


    [HttpPost]
    [Produces("application/json")]
    [Route("api/Navigation/WalkStraight")]
      public async Task<ActionResult> WalkStraight([FromBody] ReadyDataPackage package, [FromQuery] WalkStraightType type){
          var response ="";
          var walkStraight = new WalkStraightModel();
         switch (type)
         {
            case WalkStraightType.Type1:
                walkStraight = package.getDataAsObject<WalkStraightModel>();
                response = await (currentDaemon as Navigation).walkStraightType1(walkStraight.Speed,walkStraight.Angle,walkStraight.ManeuverTime);
                break;
            case WalkStraightType.Type2:
                walkStraight = package.getDataAsObject<WalkStraightModel>();
                response = await (currentDaemon as Navigation).walkStraightType2(walkStraight.Speed,walkStraight.Distance, walkStraight.Angle,walkStraight.ManeuverTime);
                break;
            case WalkStraightType.Type3:
                break;
            case WalkStraightType.Type4:
                break;             
         }
         return Ok(response);
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/Navigation/WalkCurved")]
      public ActionResult WalkCurved( [FromBody] ReadyDataPackage package, [FromQuery] WalkCurvedType type){
        var response ="";
        var walkCurved = new WalkCurvedModel();
         switch (type)
         {
            case WalkCurvedType.Type1:
                walkCurved = package.getDataAsObject<WalkCurvedModel>();
                response = (currentDaemon as Navigation).walkCurvedType1(walkCurved.LinearSpeed,walkCurved.AngularSpeed);
                break;
            case WalkCurvedType.Type2:
                walkCurved = package.getDataAsObject<WalkCurvedModel>();
                response = (currentDaemon as Navigation).walkCurvedType2(walkCurved.LinearSpeed,walkCurved.Radius, walkCurved.Angle);
                break;
            case WalkCurvedType.Type3:
                break;         
         }
         return Ok(response);
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/Navigation/GetDistance")]
      public ActionResult GetDistance( [FromBody] ReadyDataPackage package){
        var angle = package.getDataAsType<double>();
        var response = (currentDaemon as Navigation).getDistanceFromSensor(angle);
        return Ok(response);
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/Navigation/SaveWaypoint")]
    public ActionResult SaveWaypoint( [FromBody] ReadyDataPackage package){

        var name = package.getDataAsType<string>();
        var response = (currentDaemon as Navigation).saveWaypoint(name, package.AppId);
        
          return Ok(response);
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/Navigation/DeleteWaypoint")]
    public ActionResult DeleteWaypoint( [FromBody] ReadyDataPackage package){
        var name = package.getDataAsType<string>();
        var response = (currentDaemon as Navigation).deleteWaypoint(name, package.AppId);
        return Ok(response);
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/Navigation/GetWaypoint")]
    public ActionResult GetWaypoint([FromBody] ReadyDataPackage package){
        var name = package.getDataAsType<string>();
        var response = (currentDaemon as Navigation).getWaypoint(name, package.AppId);
        return Ok(response);
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/Navigation/GetDistanceFromWaypoint")]
    public ActionResult GetDistanceFromWaypoint([FromBody] ReadyDataPackage package){

        var name = package.getDataAsType<string>();
        var response = (currentDaemon as Navigation).getDistanceFromWaypoint(name, package.AppId);
        return Ok(response);
     }

     [HttpPost]
    [Produces("application/json")]
    [Route("api/Navigation/GetDistanceFromCoordinate")]
    public ActionResult GetDistanceFromCoordinate([FromBody] ReadyDataPackage package){

        var waypoint = package.getDataAsObject<Position>();
        var response = (currentDaemon as Navigation).getDistanceFromCoordinate(waypoint, package.AppId);
        return Ok(response);
     }

    [HttpPost]
    [Produces("application/json")]
    [Route("api/Navigation/GetNearestWaypoints")]
    public ActionResult GetNearestWaypoints( [FromBody] ReadyDataPackage package){
         var listSize = package.getDataAsType<int>();
        var response = (currentDaemon as Navigation).getNearestWaypoints(listSize, package.AppId);
        return Ok(response);
     }

     [HttpPost]
    [Produces("application/json")]
    [Route("api/Navigation/GoToWaypoint")]
    public ActionResult GoToWaypoint( [FromBody] ReadyDataPackage package){
        var name = package.getDataAsType<string>();
        var waypoint = (currentDaemon as Navigation).getWaypoint(name, package.AppId);
        var response = (currentDaemon as Navigation).goToWaypoint(waypoint);
        return Ok(response);
     }

     [HttpPost]
    [Produces("application/json")]
    [Route("api/Navigation/GoToCoordinate")]
    public ActionResult GoToCoordinate( [FromBody] ReadyDataPackage package){
        var coordinate = package.getDataAsObject<Coordinate>();
        var response = (currentDaemon as Navigation).goToCoordinate(coordinate.Position,coordinate.Orientation);
        return Ok(response);
     }

     [HttpPost]
    [Produces("application/json")]
    [Route("api/Navigation/GetCurrentCoordinate")]
    public ActionResult GetCurrentCoordinate( [FromBody] ReadyDataPackage package){
        var response = (currentDaemon as Navigation).getCurrentCoordinate();
        return Ok(response);
     }
     

    }
}

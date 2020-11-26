using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ready;
using Ready.Models;
using System.Threading;

namespace ReadyAppWaiterDotNetCore{
    class Program{
        static void Main(string[] args){
            int id = 3;
            var connection = new ReadyConnection(id, "http://192.168.100.142:5123/");
            var commHandler = new ReadyCommunication(connection);
            var navHandler = new ReadyNavigation(connection);
            var sysHandler = new ReadySystem(connection);
            var currentCoordinate = new Coordinate();
            var targetCoordinate = new Coordinate();

            var responseFocus = sysHandler.requestFocus();
            Console.WriteLine(responseFocus);
            while (!sysHandler.isCurrentApplication()) {
                Console.WriteLine("Nao sou a aplicacao em Foco");
            };

            sysHandler.lockFocus();
            Console.WriteLine("Sou a aplicacao em Foco");

            Random rnd = new Random();
            bool called = false;
            bool go = false;
            double distance = 0f;
            float angle = 0f;
            currentCoordinate = navHandler.getCurrentCoordinate();
            targetCoordinate = currentCoordinate;

            while(true){
                currentCoordinate = navHandler.getCurrentCoordinate();
                if(GetDistance(currentCoordinate,targetCoordinate) < 0.3f){
                    angle = rnd.Next(360);
                    distance = navHandler.getDistance(angle);
                    targetCoordinate.Position.x = Math.Cos(AngleToRadians(angle)) * (distance -distance*0.2f) + currentCoordinate.Position.x;
                    targetCoordinate.Position.y = Math.Sin(AngleToRadians(angle)) * (distance -distance*0.2f) + currentCoordinate.Position.y;    
                    targetCoordinate.Position.z = currentCoordinate.Position.z;
                    targetCoordinate.Orientation = angle;
                    navHandler.goToCoordinate(targetCoordinate.Position, targetCoordinate.Orientation );  
                }
                if(go == true) go = false;
				              
                called = commHandler.wordWasSaid("waiter");                
                if(called == true){
                     navHandler.goToCoordinate(currentCoordinate.Position, currentCoordinate.Orientation );  

                    DateTime time = DateTime.Now.AddSeconds(10);
                    while(time>DateTime.Now || go!=true){
                        go = commHandler.wordWasSaid("go"); 
                    }
                    called=false;

                }                
            }
        }

        private static double GetDistance(Coordinate current,Coordinate target)
        {
            return Math.Sqrt(Math.Pow((target.Position.x - current.Position.x ), 2) + Math.Pow((target.Position.y - current.Position.y ), 2));
        }

         private static double AngleToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
    }
}

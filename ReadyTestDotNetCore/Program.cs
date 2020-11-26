﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ready;
using Ready.Models;
using System.Threading;

namespace ReadyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int id = 1;
            var connection = new ReadyConnection(id, "http://192.168.100.142:5123/");
            var commHandler = new ReadyCommunication(connection);
            var navHandler = new ReadyNavigation(connection);
            var sysHandler = new ReadySystem(connection);



             var orbit = new Orbit();

             var waypoint = navHandler.getWaypoint("embaixo da mesa");

             orbit.setOrbitAroundWaypoint(1.0, Orbit.Orientation.Clockwise, waypoint);

            var responseTest = sysHandler.requestFocus();

             while (!sysHandler.isCurrentApplication()) {
                Console.WriteLine("nao sou");
            };
            Console.WriteLine("sou");

            var lockAnswer = sysHandler.lockFocus();
             


             var coordinate = orbit.getCurrentCoordinate();
             navHandler.goToCoordinate(coordinate.Position, coordinate.Orientation);

             while (true)
             {
                 var distace = navHandler.getDistanceFromCoordinate(orbit.getCurrentCoordinate().Position);
                 if (distace <= 0.3)
                 {
                     coordinate = orbit.setCurrentCoordinate();
                     navHandler.goToCoordinate(coordinate.Position, coordinate.Orientation);
                     var speakResponse = commHandler.speak("going to the next position!");

                     Console.WriteLine(speakResponse);

                 }



                 Thread.Sleep(100);


             }


            Console.ReadKey();
        }
    }
}

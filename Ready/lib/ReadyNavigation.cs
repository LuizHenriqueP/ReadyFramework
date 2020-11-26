using System;
using Ready.Models;
using System.Collections.Generic;

namespace Ready
{
    public class ReadyNavigation : ServiceConnection
    {
        public ReadyNavigation(ReadyConnection conn)
        {
            base.init(conn);
        }

        public string saveWaypoint(string waypointName){
            var package = assembleDataPackage<string>(waypointName);
            var url = "api/Navigation/SaveWaypoint";
            var result = sendPackage<string>(url,package).Result;
            return result;

        }
        public string deleteWaypoint(string waypointName){
            var package = assembleDataPackage<string>(waypointName);
            var url = "api/Navigation/DeleteWaypoint";
            var result = sendPackage<string>(url,package).Result;
            return result;

        }

        public Waypoint getWaypoint(string waypointName){
            var package = assembleDataPackage<string>(waypointName);
            var url = "api/Navigation/GetWaypoint";
            var result = sendPackage<Waypoint>(url,package).Result;

            return result;

        }

        public Coordinate getCurrentCoordinate(){
            var package = assembleDataPackage<string>("getCurrentCoordinate");
            var url = "api/Navigation/GetCurrentCoordinate";
            var result = sendPackage<Coordinate>(url,package).Result;

            return result;

        }

        public string goToWaypoint(string waypointName){
            var package = assembleDataPackage<string>(waypointName);
            var url = "api/Navigation/GoToWaypoint";
            var result = sendPackage<string>(url,package).Result;

            return result;

        }

         public double getDistanceFromWaypoint(string waypointName){
            var package = assembleDataPackage<string>(waypointName);
            var url = "api/Navigation/GetDistanceFromWaypoint";
            var result = sendPackage<double>(url,package).Result;
            return result;

        }
        public double getDistanceFromCoordinate(Position position){
            var package = assembleDataPackage<Position>(position);
            var url = "api/Navigation/GetDistanceFromCoordinate";
            var result = sendPackage<double>(url,package).Result;
            return result;

        }

        public string goToCoordinate(Position position, double angle  = 0.0f){
            var coordinate = new Coordinate(position,angle);
            var package = assembleDataPackage<Coordinate>(coordinate);
            var url = "api/Navigation/GoToCoordinate";
            var result = sendPackage<string>(url,package).Result;
            return result;

        }


        public List<WaypointDistance> getNearestWaypoints(int quantity){
            var package = assembleDataPackage<int>(quantity);
            var url = "api/Navigation/GetNearestWaypoints";
            var result = sendPackage<List<WaypointDistance>>(url,package).Result;
            return result;

        }

        public double getDistance(double angle){
            var package = assembleDataPackage<double>(angle);
            var url = "api/Navigation/GetDistance";
            var result = sendPackage<double>(url,package).Result;
            return result;

        }


        public string walkStraight(double speed, double angle = 0.0f, double maneuverTime = 2.0f){
            var data = new WalkStraightModel();
            data.Speed = speed;
            data.Angle = angle;
            data.ManeuverTime = maneuverTime;
            var package = assembleDataPackage<WalkStraightModel>(data);
            var url = "api/Navigation/WalkStraight?type=0";
            var result = sendPackage<string>(url,package).Result;
            return result;

        }

        public string walkStraight(double speed, double distance, double angle = 0.0f, double maneuverTime = 2.0f){
            var data = new WalkStraightModel();
            data.Speed = speed;
            data.Distance = distance;
            data.Angle = angle;
            data.ManeuverTime = maneuverTime;
            var package = assembleDataPackage<WalkStraightModel>(data);
            var url = "api/Navigation/WalkStraight?type=1";
            var result = sendPackage<string>(url,package).Result;
            return result;

        }

        public string walkCurved(double linearSpeed, double angularSpeed){
            var data = new WalkCurvedModel();
            data.LinearSpeed = linearSpeed;
            data.AngularSpeed = angularSpeed;
            var package = assembleDataPackage<WalkCurvedModel>(data);
            var url = "api/Navigation/WalkCurved?type=0";
            var result = sendPackage<string>(url,package).Result;
            return result;

        }

        public string walkCurved(double linearSpeed, double radius, WalkCurvedModel.Orientation orientation){
            var data = new WalkCurvedModel();
            data.LinearSpeed = linearSpeed;
            data.Radius = radius;
            data.Angle = (int)orientation*90;
            var package = assembleDataPackage<WalkCurvedModel>(data);
            var url = "api/Navigation/WalkCurved?type=1";
            var result = sendPackage<string>(url,package).Result;
            return result;

        }

      


    }
}

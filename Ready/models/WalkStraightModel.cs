using System;
using Newtonsoft.Json;

namespace Ready.Models{

    public class WalkStraightModel{

        public double Speed {get;set;}
        
        public double Distance {get;set;}
        public double Angle {get;set;}
        public double ManeuverTime {get;set;}


        public WalkStraightModel(){


        }
         public WalkStraightModel(double speed, double angle, double maneuverTime){
             Speed  = speed;
             Angle = angle;
             ManeuverTime = maneuverTime;

        }

    }
}


using System;
using Newtonsoft.Json;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;

namespace Whitebox.Models{

    public class WalkStraightModel{

        public double Speed {get;set;}
        public double Distance {get;set;}
        public double Angle {get;set;}
        public double ManeuverTime {get;set;}


        public WalkStraightModel(){


        }

    }
}


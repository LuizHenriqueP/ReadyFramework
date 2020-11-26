using System;
using Newtonsoft.Json;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;

namespace Whitebox.Models{

    public class WaypointDistance{

        public string Name {get;set;}

        public double Distance {get;set;}

        public WaypointDistance(){


        }

    }
}


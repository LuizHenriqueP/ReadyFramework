using System;
using Newtonsoft.Json;

namespace Ready.Models{

    public class Waypoint{

        public string Name {get;set;}
        public Position Position {get;set;}
        public Quaternion Quaternion {get;set;}


        public Waypoint(){


        }

    }
}


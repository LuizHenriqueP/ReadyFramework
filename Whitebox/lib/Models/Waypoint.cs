using System;
using Newtonsoft.Json;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;

namespace Whitebox.Models{

    public class Waypoint{

        public string Name {get;set;}
        public Position Position {get;set;} = new Position();
        public Quaternion Quaternion {get;set;} = new Quaternion();


        public Waypoint(){


        }

        public Waypoint(string [] data){
            Action<string>[] PropertyMappings =
            {
                x=>this.Name=x,
                x=>this.Position.x=Convert.ToDouble(x),
                x=>this.Position.y=Convert.ToDouble(x),
                x=>this.Position.z=Convert.ToDouble(x),
                x=>this.Quaternion.x=Convert.ToDouble(x),
                x=>this.Quaternion.y=Convert.ToDouble(x),
                x=>this.Quaternion.z=Convert.ToDouble(x),
                x=>this.Quaternion.w=Convert.ToDouble(x)
            };
            for(int i=0;i<data.Length;i++)
            {
                PropertyMappings[i](data[i]);
            }   
            
        }

        public Waypoint (string appName, Pose pose){
            this.Name =appName;
            this.Position.x = pose.position.x;
            this.Position.y = pose.position.y;
            this.Position.z = pose.position.z;

            this.Quaternion.x = pose.orientation.x;
            this.Quaternion.y = pose.orientation.y;
            this.Quaternion.z = pose.orientation.z;
            this.Quaternion.w = pose.orientation.w;
            



        }
    }
}


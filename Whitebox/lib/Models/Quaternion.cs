using System;
using Newtonsoft.Json;

namespace Whitebox.Models{

    public class Quaternion{
        
        public double w {get;set;}
        public double x {get;set;}
        public double y {get;set;}
        public double z {get;set;}

        public Quaternion(){}

        public Quaternion(double x, double y, double z, double w){
            this.x =x ;
            this.y = y;
            this.z = z;
            this.w = w;

        }
        
    }
}
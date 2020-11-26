using System;
using Newtonsoft.Json;

namespace Ready.Models{

    public class Position{
        
        public double x {get;set;}
        public double y {get;set;}
        public double z {get;set;}

        public Position(){


        }
        public Position(double x, double y, double z){
            this.x =x ;
            this.y = y;
            this.z = z;

        }


    }
}
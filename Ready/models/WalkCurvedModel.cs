using System;
using Newtonsoft.Json;

namespace Ready.Models{

    public class WalkCurvedModel{

        public enum Orientation{Left = -1,Right = 1}
        public double LinearSpeed {get;set;}
        public double AngularSpeed {get;set;}
        public double Radius {get;set;}
        public double Angle {get;set;}



        public WalkCurvedModel(){


        }

    }
}


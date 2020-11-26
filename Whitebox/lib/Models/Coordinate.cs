using System;
using Newtonsoft.Json;

namespace Whitebox.Models{

    public class Coordinate{


        public Position Position {get;set;} = new Position();

        public double Orientation {get;set;}



        public Coordinate(){


        }

        public Coordinate(double x, double y, double z, double orientation){
            Position.x = x;
            Position.y = y;
            Position.z = z;

            Orientation = orientation;

        }

        public Coordinate(Position position,  double orientation){
            this.Position = position;
            this.Orientation = orientation;

        }



    }
}


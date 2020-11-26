using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Ready.Models;

namespace ReadyTest
{
    class Orbit
    {
        private Waypoint waypoint;

        private Queue<Coordinate> coordinates = new Queue<Coordinate>();

        private Coordinate currentCoordinate;

        public enum Orientation {Clockwise,CounterClockwise};

        private Orientation currentOrientation;

        private  double [] anglesCounter = new double[] { 0.0, 45.0,90.0,135.0,180.0,225.0,270.0,315.0};

        private double[] angles = new double[] { 0.0, 315.0, 270.0, 225.0, 180.0,135.0, 90.0, 45.0};

        public Orbit() {

        }

        public Orbit(Waypoint waypoint, Queue<Coordinate> coordinates) {
            this.waypoint = waypoint;
            this.coordinates = coordinates;
        }

        public void setWaypoint(Waypoint waypoint) {
            this.waypoint = waypoint;
        }
        private void setCoordinates(Queue<Coordinate> coordinates)
        {
            this.coordinates = coordinates;
        }

        public void setOrbitAroundWaypoint( double radius, Orientation orientation, Waypoint waypoint = null) {
            if (waypoint != null) {
                setWaypoint(waypoint);
            }
            var coordinates = getOrbitAroundCurrentWaypoint(radius, orientation);
            setCoordinates(coordinates);
            setCurrentCoordinate();
        }

        public void setCurrentCoordinate(Coordinate coordinate) {
            this.currentCoordinate = coordinate;
        }
        public Coordinate setCurrentCoordinate()
        {
            var coordinate = coordinates.Dequeue();

            this.currentCoordinate = coordinate;
            coordinates.Enqueue(coordinate);
            return coordinate;
        }

        public Coordinate getCurrentCoordinate() {
            return currentCoordinate;
        }

        private Queue<Coordinate> getOrbitAroundCurrentWaypoint(double radius, Orientation orientation) {
            var coordinates = new Queue<Coordinate>();
            double rotation = 0;
            if (orientation == Orientation.CounterClockwise)
            {
                rotation = rotation + 90;
                foreach (double angle in anglesCounter)
                {
                    var coordinate = new Coordinate();
                    coordinate.Position.x = waypoint.Position.x + radius * Math.Cos(AngleToRadians(angle));
                    coordinate.Position.y = waypoint.Position.y + radius * Math.Sin(AngleToRadians(angle));
                    coordinate.Position.z = 0.0;
                    coordinate.Orientation = rotation;
                    rotation += 45;
                    coordinates.Enqueue(coordinate);
                }

            }
            else {

                rotation = rotation - 90;
                foreach (double angle in angles)
                {
                    var coordinate = new Coordinate();
                    coordinate.Position.x = waypoint.Position.x + radius * Math.Cos(AngleToRadians(angle));
                    coordinate.Position.y = waypoint.Position.y + radius * Math.Sin(AngleToRadians(angle));
                    coordinate.Position.z = 0.0;
                    coordinate.Orientation = rotation;
                    rotation -= 45;
                    coordinates.Enqueue(coordinate);
                }

            }
            return coordinates;

        }

        private double AngleToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }




    

    }
}

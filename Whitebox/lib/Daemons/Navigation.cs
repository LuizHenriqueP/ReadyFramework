using System;
using System.Collections.Generic;
using System.Threading;
using Whitebox.Models;
using RosSharp.RosBridgeClient;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Whitebox.Daemons{
    public class Navigation :Daemon{

        private TwistPublisher velocityPublisher;
        private OdomSubscriber odomSubscriber;
        private PoseStampedPublisher navMapPublisher;
        private LaserScanSubscriber scanSubscriber;

        public Navigation(string name){
            base.init(name);
            velocityPublisher = new TwistPublisher("/cmd_vel");
            odomSubscriber = new OdomSubscriber("/odom",50);
            navMapPublisher = new PoseStampedPublisher("/move_base_simple/goal");
            scanSubscriber = new LaserScanSubscriber("/scan",500);
        }

        ~Navigation(){
            Dispose(false);                     
        }
        protected override void Run(){
            while(true){
                if(messageQueue.Count > 0){
                    var message = messageQueue.Dequeue();
                    Console.WriteLine("Recebi de "+ message.Source+": "+message.Command);
                }

                Thread.Sleep(1100);
            }

        }

        public string saveWaypoint(string name, int appId){
            //comando ros pra pegar a posição 
            var pose = odomSubscriber.Odometry.pose.pose;


            Waypoint waypoint = new Waypoint(name, pose);
            var response = sendMessageAndWait("Database",
            "saveWaypoint",
            new string []{
                JsonConvert.SerializeObject(waypoint),
                appId.ToString()
            });

            return response;
        }

        public Waypoint getWaypoint(string name, int appId){

            var response = sendMessageAndWait("Database","getWaypoint",new string[]{name,appId.ToString()});
            return JsonConvert.DeserializeObject<Waypoint>(response);
        }

        public string deleteWaypoint(string name, int appId){
            var response = sendMessageAndWait("Database","deleteWaypoint",new string[]{name,appId.ToString()});
            return response;
        }

        public double getDistanceFromWaypoint(string name, int appId){
            var pose = odomSubscriber.Odometry.pose.pose;
            Waypoint waypoint = new Waypoint("current", pose);
            var response = sendMessageAndWait("Database","getDistanceFromWaypoint",new string[]{JsonConvert.SerializeObject(waypoint),appId.ToString()});
            return Convert.ToDouble(response);
        }

        public double getDistanceFromCoordinate(Position waypoint, int appId){
            var pose = odomSubscriber.Odometry.pose.pose.position;
            var currentPosition = new Position(pose.x,pose.y,pose.z);
   
            var distance = getDistance(currentPosition, waypoint);
            
            return distance;
        }

        public List<WaypointDistance> getNearestWaypoints(int listSize, int appId){
            var pose = odomSubscriber.Odometry.pose.pose;
            Waypoint waypoint = new Waypoint("current", pose);
            var response = sendMessageAndWait("Database","getNearestWaypoints",new string[]{JsonConvert.SerializeObject(waypoint),listSize.ToString(),appId.ToString()});
            return JsonConvert.DeserializeObject<List<WaypointDistance>>(response);
        }

        public Coordinate getCurrentCoordinate(){
            var pose = odomSubscriber.Odometry.pose.pose;
            var rotation = QuaternionToAngle(pose.orientation.z,pose.orientation.w);
            var coordinate = new Coordinate(pose.position.x,pose.position.y,pose.position.z,rotation);
            return coordinate;
        }

        public double getDistanceFromSensor(double angle){

            bool signal = angle>=0;

            angle = Math.Abs(angle);
            while(angle >=360){
                angle -=360;
            }

            if(!signal){
                angle = 360 - angle;
            }

            var response = scanSubscriber.getAngleDistance((int)angle);
            return response != null ? Convert.ToDouble(response) : -1.0f;
        }

        public double getDistance(Position current, Position input){

            var distance = Math.Sqrt(Math.Pow(current.x-input.x,2) + Math.Pow(current.y-input.y,2) + Math.Pow(current.z-input.z,2));

            return distance;
        }

        public string goToWaypoint (Waypoint waypoint){
            try {
             navMapPublisher.goToWaypoint(waypoint);


            }
            catch(Exception ex){
                return "error setting up waypoint";

            }
            return "success";
        }

        public string goToCoordinate (Position position, double orientation){
            try {
            var quaternion = ToQ(orientation);
            navMapPublisher.goToCoordinate(position, quaternion);


            }
            catch(Exception ex){
                return "error setting up coordinate";

            }
            return "success";
        }

        private double angleOrientation(double angle){
                bool signal = angle>=0;

                angle = Math.Abs(angle);
                while(angle >=360){
                    angle -=360;

                }

                if(angle > 180)
                    angle = (360 - angle)*(-1);

                if(!signal)
                    angle *=-1;
                return angle;
        }

        public async Task<string> walkStraightType1(double speed, double angle, double maneuverTime){
            try
            {
                 
                if(angle != 0.0f){
                     
                    angle = angleOrientation(angle);
                    var radian = Math.PI * angle / 180;
                    var angularSpeed =  radian / maneuverTime  ;

                    var angular = new RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3(0.0,0.0,angularSpeed);

                    velocityPublisher.UpdateAngularVelocity(angular);
                    await Task.Delay((int)(maneuverTime*(1200+(angularSpeed*120))));
                    angular.z =0.0;
                    velocityPublisher.UpdateAngularVelocity(angular);
                }
                var velocity = new RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3(speed,0.0,0.0);
                velocityPublisher.UpdateLinearVelocity(velocity);
            }
            catch (System.Exception ex)
            {
                
                Console.WriteLine(ex.ToString());
                return "error updating velocity";
            }

            return "success";
            
        }

         public async Task<string> walkStraightType2(double speed, double distance, double angle, double maneuverTime){
            try
            {
                 
                if(angle != 0.0f){
                     
                    angle = angleOrientation(angle);
                    var radian = Math.PI * angle / 180;
                    var angularSpeed =  radian / maneuverTime  ;

                    var angular = new RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3(0.0,0.0,angularSpeed);

                    velocityPublisher.UpdateAngularVelocity(angular);
                    await Task.Delay((int)(maneuverTime*(1200+(angularSpeed*120))));
                    angular.z =0.0;
                    velocityPublisher.UpdateAngularVelocity(angular);
                }
                var distanceTime = distance / speed;
                var velocity = new RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3(speed,0.0,0.0);
                velocityPublisher.UpdateLinearVelocity(velocity);
                await Task.Delay((int)(distanceTime*(1200+(speed*120))));
                velocity.x =0.0;
                velocityPublisher.UpdateLinearVelocity(velocity);

            }
            catch (System.Exception ex)
            {
                
                Console.WriteLine(ex.ToString());
                return "error updating velocity";
            }

            return "success";
            
        }

        public string walkCurvedType1(double linearSpeed, double angularSpeed){
            try
            {
                 
                var angular = new RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3(0.0,0.0,angularSpeed);

                var linear = new RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3(linearSpeed,0.0,0.0);
                velocityPublisher.UpdateVelocity(linear,angular);
            }
            catch (System.Exception ex)
            {
                
                Console.WriteLine(ex.ToString());
                return "error updating velocity";
            }

            return "success";
            
        }

        public string walkCurvedType2(double linearSpeed, double radius, double angle){
            try
            {
                var radians = (angle*180)/(Math.PI);
                var angularSpeed = (linearSpeed*Math.Sin(radians))/radius;
                 
                var angular = new RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3(0.0,0.0,angularSpeed);

                var linear = new RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3(linearSpeed,0.0,0.0);
                velocityPublisher.UpdateVelocity(linear,angular);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "error updating velocity";
            }

            return "success";
            
        }

        public Quaternion ToQ(double angle)
        {
            return ToQ(0.0,0.0f,angle);
        }

        public double QuaternionToAngle(double Z,double W){
            var az = Z / Math.Sin(Math.Acos(W));

            return Math.Sin(az);
        }

        public Quaternion ToQ(double yaw, double pitch, double roll)
        {
            yaw =  Math.PI * yaw / 180;
            pitch = Math.PI * pitch / 180;
            roll = Math.PI * roll / 180;
            double rollOver2 = roll * 0.5f;
            double sinRollOver2 = (double)Math.Sin((double)rollOver2);
            double cosRollOver2 = (double)Math.Cos((double)rollOver2);
            double pitchOver2 = pitch * 0.5f;
            double sinPitchOver2 = (double)Math.Sin((double)pitchOver2);
            double cosPitchOver2 = (double)Math.Cos((double)pitchOver2);
            double yawOver2 = yaw * 0.5f;
            double sinYawOver2 = (double)Math.Sin((double)yawOver2);
            double cosYawOver2 = (double)Math.Cos((double)yawOver2);
            Quaternion result = new Quaternion();
            result.w = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
            result.x = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
            result.y = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;
            result.z = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;

            return result;
        } 


    }

}
/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using Newtonsoft.Json;

namespace RosSharp.RosBridgeClient.MessageTypes.Geometry
{
    public class TwistWithCovariance : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "geometry_msgs/TwistWithCovariance";

        //  This expresses velocity in free space with uncertainty.
        public Twist twist;
        //  Row-major representation of the 6x6 covariance matrix
        //  The orientation parameters use a fixed-axis representation.
        //  In order, the parameters are:
        //  (x, y, z, rotation about X axis, rotation about Y axis, rotation about Z axis)
        public double[] covariance;

        public TwistWithCovariance()
        {
            this.twist = new Twist();
            this.covariance = new double[36];
        }

        public TwistWithCovariance(Twist twist, double[] covariance)
        {
            this.twist = twist;
            this.covariance = covariance;
        }
    }
}

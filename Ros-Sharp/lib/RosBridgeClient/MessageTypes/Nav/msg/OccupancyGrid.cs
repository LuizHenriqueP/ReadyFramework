/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using Newtonsoft.Json;

using RosSharp.RosBridgeClient.MessageTypes.Std;

namespace RosSharp.RosBridgeClient.MessageTypes.Nav
{
    public class OccupancyGrid : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "nav_msgs/OccupancyGrid";

        //  This represents a 2-D grid map, in which each cell represents the probability of
        //  occupancy.
        public Header header;
        // MetaData for the map
        public MapMetaData info;
        //  The map data, in row-major order, starting with (0,0).  Occupancy
        //  probabilities are in the range [0,100].  Unknown is -1.
        public sbyte[] data;

        public OccupancyGrid()
        {
            this.header = new Header();
            this.info = new MapMetaData();
            this.data = new sbyte[0];
        }

        public OccupancyGrid(Header header, MapMetaData info, sbyte[] data)
        {
            this.header = header;
            this.info = info;
            this.data = data;
        }
    }
}

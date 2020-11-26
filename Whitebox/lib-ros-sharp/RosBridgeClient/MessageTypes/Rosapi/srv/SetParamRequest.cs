/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using Newtonsoft.Json;

namespace RosSharp.RosBridgeClient.MessageTypes.Rosapi
{
    public class SetParamRequest : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "rosapi/SetParam";

        public string name;
        public string value;

        public SetParamRequest()
        {
            this.name = "";
            this.value = "";
        }

        public SetParamRequest(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}

/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System.Threading;
using Whitebox.Models;


namespace RosSharp.RosBridgeClient
{
    public class PoseStampedPublisher : ReadyPublisher<MessageTypes.Geometry.PoseStamped>
    {
        private MessageTypes.Geometry.PoseStamped message;
        private MessageTypes.Geometry.Pose pose;
        private MessageTypes.Std.Header header;


        public PoseStampedPublisher(string topic)
        {
            base.Start(topic);
            InitializeMessage();
        }



        private void InitializeMessage()
        {
            header = new MessageTypes.Std.Header(0,new MessageTypes.Std.Time(),"map");
            pose = new MessageTypes.Geometry.Pose();
            message = new MessageTypes.Geometry.PoseStamped(header,pose);
          
        }

        public void goToWaypoint(Waypoint waypoint)
        {
            message.pose.position.x = waypoint.Position.x;
            message.pose.position.y = waypoint.Position.y;
            message.pose.position.z = waypoint.Position.z;

            message.pose.orientation.w = waypoint.Quaternion.w;
            message.pose.orientation.x = waypoint.Quaternion.x;
            message.pose.orientation.y = waypoint.Quaternion.y;
            message.pose.orientation.z = waypoint.Quaternion.z;


            Publish(message);
        }
        public void goToCoordinate(Position position, Quaternion quaternion)
        {
            message.pose.position.x = position.x;
            message.pose.position.y = position.y;
            message.pose.position.z = position.z;

            message.pose.orientation.w = quaternion.w;
            message.pose.orientation.x = quaternion.x;
            message.pose.orientation.y = quaternion.y;
            message.pose.orientation.z = quaternion.z;

            Publish(message);
        }


    }
}

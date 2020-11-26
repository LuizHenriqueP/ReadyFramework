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
using UnityTypes;

namespace RosSharp.RosBridgeClient
{
    public class PosePublisher : MyPublisher<MessageTypes.Geometry.Pose>
    {
        public string FrameId = "Unity";

        private MessageTypes.Geometry.Pose message;

        public PosePublisher(string topic, ref RosConnector connector)
        {
            base.Start(topic, ref connector);
            InitializeMessage();
            new Thread(FixedUpdate).Start();
        }

        private void FixedUpdate()
        {
            while(true){
            UpdateMessage();      
            Thread.Sleep(1000);         
            }

        }

        private void InitializeMessage()
        {
            message = new MessageTypes.Geometry.Pose
            {
            };
        }

        private void UpdateMessage()
        {
            message.position = GetGeometryPoint( new Vector3(0.0f,0.0f,0.0f));
            message.orientation = GetGeometryQuaternion(new Quaternion(0.0f,0.0f,0.0f,0.0f));

            Publish(message);
        }

        private MessageTypes.Geometry.Point GetGeometryPoint(Vector3 position)
        {
            MessageTypes.Geometry.Point geometryPoint = new MessageTypes.Geometry.Point();
            geometryPoint.x = position.x;
            geometryPoint.y = position.y;
            geometryPoint.z = position.z;
            return geometryPoint;
        }

        private MessageTypes.Geometry.Quaternion GetGeometryQuaternion(Quaternion quaternion)
        {
            MessageTypes.Geometry.Quaternion geometryQuaternion = new MessageTypes.Geometry.Quaternion();
            geometryQuaternion.x = quaternion.x;
            geometryQuaternion.y = quaternion.y; 
            geometryQuaternion.z = quaternion.z;
            geometryQuaternion.w = quaternion.w;
            return geometryQuaternion;
        }

    }
}

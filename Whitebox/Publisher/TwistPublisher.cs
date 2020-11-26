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
using RosSharp.RosBridgeClient.MessageTypes.Geometry;

namespace RosSharp.RosBridgeClient
{
    public class TwistPublisher : ReadyPublisher<MessageTypes.Geometry.Twist>
    {
        private MessageTypes.Geometry.Twist message;


        public TwistPublisher(string topic)
        {
            base.Start(topic);
            InitializeMessage();
        }


        private void InitializeMessage()
        {

            message = new MessageTypes.Geometry.Twist();
          
        }

        public void UpdateVelocity(Vector3 linear, Vector3 angular)
        {
            
            message.linear = linear;
            message.angular = angular;

            Publish(message);
        }
        public void UpdateLinearVelocity(Vector3 velocity)
        {
            message.linear = velocity;

            Publish(message);
        }

         public void UpdateAngularVelocity(Vector3 velocity)
        {
            message.angular = velocity;

            Publish(message);
        }

    }
}

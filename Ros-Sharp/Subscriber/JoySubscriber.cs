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
using System;

namespace RosSharp.RosBridgeClient
{
    public class JoySubscriber : MySubscriber<MessageTypes.Sensor.Joy>
    {
        public JoySubscriber(){

//this.Start();
        }

        public JoySubscriber(string topic,float timeStep, ref RosConnector connector){

            base.Start(topic,timeStep,ref connector);
        }

		
        protected override void ReceiveMessage(MessageTypes.Sensor.Joy joy)
        {
            Console.WriteLine("Buttons");
            int I = joy.buttons.Length;
            for (int i = 0; i < I; i++)              
                    Console.WriteLine(joy.buttons[i]);

             Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Axes");
            I = joy.axes.Length;
            for (int i = 0; i < I; i++)
                    Console.WriteLine(joy.axes[i]);

            Console.WriteLine("");
            Console.WriteLine("");
        }
    }
}

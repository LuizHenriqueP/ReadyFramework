using System;
using RosSharp.RosBridgeClient;

namespace Ros_Sharp
{
    class Program
    {
        static void Main(string[] args)
        {
            RosConnector connector = new RosConnector();
            if(connector.IsConnected.WaitOne()){
                JoySubscriber subs = new JoySubscriber("/joy",0, ref connector);
                PosePublisher pub  = new PosePublisher("/poses",ref connector);
                while(true);
            }
        }
    }
}

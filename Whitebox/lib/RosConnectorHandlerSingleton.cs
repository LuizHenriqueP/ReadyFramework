using System;
using Whitebox.Models;
using RosSharp.RosBridgeClient;
using System.Collections.Generic;
using System.Linq;

namespace Whitebox{
    public class RosConnectorHandlerSingleton{
       private static RosConnectorHandlerSingleton instance;

       private RosConnector connector;

       private List<ReadyTopic> topicList = new List<ReadyTopic>();
       private List<Daemon> daemonList = new List<Daemon>();
 
        private RosConnectorHandlerSingleton() {
            this.connector = new RosConnector();
         }
 
        public static RosConnectorHandlerSingleton Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(RosConnectorHandlerSingleton))
                        if (instance == null) instance = new RosConnectorHandlerSingleton();
 
                return instance;
            }
        }

        public ref RosConnector getRosConnector(){

            return ref this.connector;
        }


        public bool logInTopic(ReadyTopic topic){
            if(!topicList.Exists(x => x.Topic.ToLower().Trim() == topic.Topic.ToLower().Trim() && x.Type == topic.Type)){
                topicList.Add(topic);
                return true;
            }
            else{
                return false;      
            }
            
        }


        public void logOutTopic(ReadyTopic topic){
            topicList.Remove(topic);
        }

        public List<ReadyTopic> getTopicList(){
            return topicList;

        }

        public List<ReadyTopic> getTopicList(string type){
            return topicList.Where(x => x.Type == type).ToList();

        }



        
    }

}
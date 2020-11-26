using System;
using System.Collections.Generic;
using System.Threading;
using Whitebox.Models;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using Newtonsoft.Json;

namespace Whitebox.Daemons{
    public class Database:Daemon{
        
        private  MySqlConnection conn;
        public Database(string name){
            base.init(name); 
            initializeDatabase();

        }

        ~Database(){
            Dispose(false);                     
        }

        private void initializeDatabase(){
        string connStr = "server=localhost;user=root;database=ReadyDB;port=3306;password=10";
        conn = new MySqlConnection(connStr);
        }
     
         protected override void Run(){
             while(true){
                if(messageQueue.Count > 0){
                    var message = messageQueue.Dequeue();
                    var parameters = new List<string>();

                    switch(message.Command){
                        case "saveWaypoint":
                            saveWaypointCommand(message);
                            break;
                        case "getApplicationId":
                            getApplicationIdCommand(message);                          
                            break;
                        case "getWaypoint":
                            getWaypointCommand(message);
                            break;
                        case "deleteWaypoint":
                            deleteWaypointCommand(message);
                            break;
                        case "getDistanceFromWaypoint":
                            getDistanceFromWaypointCommand(message);
                            //list all installed apps
                            break;
                        case "getNearestWaypoints":
                            getNearestWaypointsCommand(message);
                            //install app here
                            break;
                        case "analyzeWord":
                            analyzeWordCommand(message);
                            //uninstall app here
                            break;
                        case "getSystemCommand":
                            getSystemCommandCommand(message);
                            //return current app name or id
                            break;
                        case "getAppStatus":
                            //get specific app status
                            break;
                    }
                }

                Thread.Sleep(1100);
             }
           

        }

        #region saveWaypoint
        private void saveWaypointCommand(Message message){
            var parameters =message.Parameters.ToList();
            var appId = Convert.ToInt32(parameters.Last());
            parameters.RemoveAt(parameters.Count - 1 );
            var waypoint = JsonConvert.DeserializeObject<Waypoint>(parameters[0]);
            var response = saveWaypoint(waypoint,appId);
            sendResponse(message,new string []{response}); 

        }
        private string saveWaypoint(Waypoint waypoint, int applicationId){
            /*string query = ""
            conn.Open(var)*/

            var query = @"INSERT INTO Waypoint(name,axisX,axisY,axisZ,quaternionX,quaternionY ,quaternionZ ,quaternionW,applicationId)
            VALUES (@name,@axisX,@axisY,@axisZ,@quaternionX,@quaternionY ,@quaternionZ ,@quaternionW,@applicationId)";
            conn.Open();
            try
            {
                using(var cmd = new MySqlCommand(query,conn)){
                cmd.Parameters.AddWithValue("name",waypoint.Name);

                cmd.Parameters.AddWithValue("axisX",waypoint.Position.x);
                cmd.Parameters.AddWithValue("axisY",waypoint.Position.y);

                cmd.Parameters.AddWithValue("axisZ",waypoint.Position.z);
                cmd.Parameters.AddWithValue("quaternionX",waypoint.Quaternion.x);
                cmd.Parameters.AddWithValue("quaternionY",waypoint.Quaternion.y);
                cmd.Parameters.AddWithValue("quaternionZ",waypoint.Quaternion.z);
                cmd.Parameters.AddWithValue("quaternionW",waypoint.Quaternion.w);
                cmd.Parameters.AddWithValue("applicationId",applicationId);

                cmd.ExecuteScalar();

            }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                conn.Close();
                return "fail";
            }
       
            conn.Close();
            return "success";



        }
        #endregion

        #region getApplicationId
        private void getApplicationIdCommand(Message message){

            var id = getApplicationId(message.Parameters[0]);
            sendResponse(message, new string []{id.ToString()} );
        }

        private int getApplicationId(string appName){
            var query = "SELECT id FROM Application WHERE name = @appName";

            conn.Open();
            int id = 0;
            using(var cmd = new MySqlCommand(query,conn)){
                cmd.Parameters.AddWithValue("appName",appName);
                var rdr = cmd.ExecuteReader();



                while(rdr.Read()){
                    id = rdr["id"] != DBNull.Value ? Convert.ToInt32(rdr["id"]) : 0;
                }

            }
            conn.Close();
            return id;

        }
        #endregion

        #region getWaypoint
        private void getWaypointCommand(Message message){

            var waypoint  = getWaypoint(message.Parameters[0],Convert.ToInt32(message.Parameters[1]));
            sendResponse(message,new string[] {JsonConvert.SerializeObject(waypoint)});
         }
        private Waypoint getWaypoint(string name, int appId){
            var query = "SELECT * FROM Waypoint WHERE name = @name AND applicationId = @appId";

            conn.Open();
            var waypoint = new Waypoint();
            try
            {
                using(var cmd = new MySqlCommand(query,conn)){
                cmd.Parameters.AddWithValue("name",name);
                cmd.Parameters.AddWithValue("appId",appId);

                var rdr = cmd.ExecuteReader();



                while(rdr.Read()){
                    waypoint.Name = rdr["name"] != DBNull.Value ? rdr["name"].ToString() : null;
                    waypoint.Position.x = rdr["axisX"] != DBNull.Value ? Convert.ToDouble( rdr["axisX"]): 0.00;
                    waypoint.Position.y = rdr["axisY"] != DBNull.Value ? Convert.ToDouble( rdr["axisY"]): 0.00;
                    waypoint.Position.z = rdr["axisZ"] != DBNull.Value ? Convert.ToDouble( rdr["axisZ"]): 0.00;
                    waypoint.Quaternion.x = rdr["quaternionX"] != DBNull.Value ? Convert.ToDouble( rdr["quaternionX"]): 0.00;
                    waypoint.Quaternion.y = rdr["quaternionY"] != DBNull.Value ? Convert.ToDouble( rdr["quaternionY"]): 0.00;
                    waypoint.Quaternion.z = rdr["quaternionZ"] != DBNull.Value ? Convert.ToDouble( rdr["quaternionZ"]): 0.00;
                    waypoint.Quaternion.w = rdr["quaternionW"] != DBNull.Value ? Convert.ToDouble( rdr["quaternionW"]): 0.00;

                }

            }  
            }
            catch (System.Exception ex)
            {
                conn.Close();
                throw;
            }

            conn.Close();
            return waypoint;

        }
        #endregion

        #region deleteWaypoint
        private void deleteWaypointCommand(Message message){
            var response  = deleteWaypoint(message.Parameters[0],Convert.ToInt32(message.Parameters[1]));
            sendResponse(message,new string[] {response});
        }

        private string deleteWaypoint(string name, int appId){
            var query = @"DELETE FROM WayPoint WHERE name = @name AND applicationId = @appId";
            conn.Open();
            try
            {
                using(var cmd = new MySqlCommand(query,conn)){
                cmd.Parameters.AddWithValue("name",name);
                cmd.Parameters.AddWithValue("appId",appId);
                cmd.ExecuteScalar();

                }
            }
            catch (System.Exception)
            {
                conn.Close();
                return "fail";
            }
       
            conn.Close();
            return "success";


        }
        
        #endregion

        #region getDistanceFromWaypoint
        private void getDistanceFromWaypointCommand(Message message){
             var parameters =message.Parameters.ToList();
            var appId = Convert.ToInt32(parameters.Last());
            parameters.RemoveAt(parameters.Count - 1 );
            var waypoint = JsonConvert.DeserializeObject<Waypoint>(parameters[0]);
            var response = getDistanceFromWaypoint(waypoint,appId);

        }

        private double getDistanceFromWaypoint(Waypoint waypoint, int appId){

                var query = @"SELECT name, SQRT(POW(axisX - @currentX,2) + POW(axisY - @currentY,2) + POW(axisZ - @currentZ,2)) as module
                                FROM WayPoint
                                WHERE name = @name AND applicationId = @appId";

            conn.Open();
            double distance = -1.00;
            try
            {
                using(var cmd = new MySqlCommand(query,conn)){
                cmd.Parameters.AddWithValue("name",waypoint.Name);
                cmd.Parameters.AddWithValue("currentX",waypoint.Position.x);
                cmd.Parameters.AddWithValue("currentY",waypoint.Position.y);
                cmd.Parameters.AddWithValue("currentZ",waypoint.Position.z);
                cmd.Parameters.AddWithValue("appId",appId);


                var rdr = cmd.ExecuteReader();



                while(rdr.Read()){
                    distance = rdr["module"] != DBNull.Value ? Convert.ToDouble( rdr["module"]): -1.00;

                }

            }  
            }
            catch (System.Exception)
            {
                conn.Close();
                throw;
            }

            conn.Close();
            return distance;

        }
        #endregion

        #region  getNearestWaypoints
        private void getNearestWaypointsCommand(Message message){
            var parameters =message.Parameters.ToList();
            var appId = Convert.ToInt32(parameters.Last());
            var waypoint = JsonConvert.DeserializeObject<Waypoint>(parameters[0]);
            var listSize = Convert.ToInt32(parameters[1]);
            var response = getNearestWaypoints(waypoint, listSize, appId);
            sendResponse(message,new string [] {JsonConvert.SerializeObject(response)});

        }

        private List<WaypointDistance> getNearestWaypoints(Waypoint waypoint, int listSize, int appId){
                var query = @"SELECT name, SQRT(POW(axisX - @currentX,2) + POW(axisY - @currentY,2) + POW(axisZ - @currentZ,2)) as module
                                FROM WayPoint
                                ORDER BY modulo
                                WHERE applicationId = @appId
                                LIMIT @listSize";

            conn.Open();
            List<WaypointDistance> distances = new List<WaypointDistance>();
            try
            {
                using(var cmd = new MySqlCommand(query,conn)){
                cmd.Parameters.AddWithValue("listSize",listSize);
                cmd.Parameters.AddWithValue("currentX",waypoint.Position.x);
                cmd.Parameters.AddWithValue("currentY",waypoint.Position.y);
                cmd.Parameters.AddWithValue("currentZ",waypoint.Position.z);
                cmd.Parameters.AddWithValue("appId",appId);


                var rdr = cmd.ExecuteReader();



                while(rdr.Read()){
                    var waypointDistance = new WaypointDistance();
                    waypointDistance.Name = rdr["name"] != DBNull.Value ? rdr["name"].ToString(): null;
                    waypointDistance.Distance = rdr["module"] != DBNull.Value ? Convert.ToDouble( rdr["module"]): -1.00;

                    distances.Add(waypointDistance);
                }

            }  
            }
            catch (System.Exception)
            {
                conn.Close();
                throw;
            }

            conn.Close();
            return distances;

        }

        #endregion

         #region  analyzeWord
        private void analyzeWordCommand(Message message){
            var parameters =message.Parameters.ToList();
            var word = JsonConvert.DeserializeObject<VoiceWord>(parameters[0]);
            var response = analyzeWord(word);
            sendResponse(message,new string [] {JsonConvert.SerializeObject(response)});

        }

        private List<VoiceWord> analyzeWord(VoiceWord word){
            var query = @"SELECT * FROM RobotAlias
                                WHERE alias SOUNDS LIKE @word";
;

            conn.Open();
            List<VoiceWord> words = new List<VoiceWord>();
            try
            {
                using(var cmd = new MySqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("word",word.Word);
       
                    var rdr = cmd.ExecuteReader();


                    if(rdr.HasRows)
                    {
                        while(rdr.Read())
                        {
                            var aliasWord = new VoiceWord(rdr["alias"].ToString(),word.TimeStamp,word.WordId,word.MessageId,true, VoiceWord.wordType.Alias);
                            words.Add(aliasWord);
                        }


                    }
                    
                }

                query = @"SELECT * FROM SystemCommand
                                WHERE command SOUNDS LIKE @word";
                using(var cmd = new MySqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("word",word.Word);
       
                    var rdr = cmd.ExecuteReader();


                    if(rdr.HasRows)
                    {
                        while(rdr.Read())
                        {
                            var systemWord = new VoiceWord(rdr["command"].ToString(),word.TimeStamp,word.WordId,word.MessageId,true, VoiceWord.wordType.SystemCommand);
                            words.Add(systemWord);
                        }


                    }
                    
                } 
                query = @"SELECT * FROM ApplicationCommand
                                WHERE command SOUNDS LIKE @word";
                using(var cmd = new MySqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("word",word.Word);

                    var rdr = cmd.ExecuteReader();


                    if(rdr.HasRows)
                    {
                        while(rdr.Read())
                        {
                            var applicationWord = new VoiceWord(rdr["command"].ToString(),word.TimeStamp,word.WordId,word.MessageId,true, VoiceWord.wordType.ApplicationCommand, Convert.ToInt32(rdr["applicationId"].ToString()));
                            words.Add(applicationWord);
                        }


                    }
                    
                } 

                 query = @"SELECT * FROM CallWord
                                WHERE callWord SOUNDS LIKE @word";
                using(var cmd = new MySqlCommand(query,conn))
                {
       
                    cmd.Parameters.AddWithValue("word",word.Word);

                    var rdr = cmd.ExecuteReader();


                    if(rdr.HasRows)
                    {
                        while(rdr.Read())
                        {
                            var callWord = new VoiceWord(rdr["callWord"].ToString(),word.TimeStamp,word.WordId,word.MessageId,true, VoiceWord.wordType.CallWord);
                            words.Add(callWord);
                        }


                    }
                    
                }     
            }
            catch (System.Exception ex)
            {
                conn.Close();
                throw;
            }

            conn.Close();




            return words;

        }

        #endregion

        #region  getSystemCommand
        private void getSystemCommandCommand(Message message){
            var parameters =message.Parameters.ToList();
            var commandName = JsonConvert.DeserializeObject<string>(parameters[0]);
            var response = getSystemCommand(commandName);
            sendResponse(message,new string [] {JsonConvert.SerializeObject(response)});

        }

        private SystemCommand getSystemCommand(string commandName){
            var query = @"SELECT id, command, action
                                FROM SystemCommand
                                WHERE command = @command";
            var systemCommand = new SystemCommand();

            conn.Open();
            List<WaypointDistance> distances = new List<WaypointDistance>();
            try
            {
                using(var cmd = new MySqlCommand(query,conn)){
                cmd.Parameters.AddWithValue("command",commandName);

                var rdr = cmd.ExecuteReader();



                while(rdr.Read()){
                    systemCommand.Id = rdr["id"] != DBNull.Value ? Convert.ToInt32( rdr["id"]): 0;
                    systemCommand.Command = rdr["command"] != DBNull.Value ? rdr["command"].ToString():null;
                    systemCommand.Action = rdr["action"] != DBNull.Value ? rdr["action"].ToString():null;

                }

            }  
            }
            catch (System.Exception)
            {
                conn.Close();
                throw;
            }

            conn.Close();
            return systemCommand;

        }

        #endregion
    }

}
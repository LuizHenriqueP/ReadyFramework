using System;
using System.Collections.Generic;
using System.Threading;
using Whitebox.Models;
using RosSharp.RosBridgeClient;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Whitebox.Daemons{
    public class Communication :Daemon{

        private StringSubscriber voiceInput;
        private StringPublisher voiceInputClear;
        private StringPublisher voiceOutput;


        private long messageCount = 0;

        private List<VoiceWord> rawWords = new List<VoiceWord>();

        private List<VoiceWord> processedWords = new List<VoiceWord>();

        private List<VoiceWord> possibleCommands = new List<VoiceWord>();



        private Thread queueThread;

        private Thread messageAnalysisThread;

        private Thread commandAnalysisThread;


        public Communication(string name){
            voiceInput = new StringSubscriber("/voiceInput",10);

            rawWords = new List<VoiceWord>();
            processedWords = new List<VoiceWord>();
            base.init(name);

            voiceInputClear = new StringPublisher("/voiceInput");
            voiceOutput = new StringPublisher("/voiceOutput");


            queueThread =  new Thread(new ThreadStart(queueMessage));
            queueThread.Start();

            messageAnalysisThread = new Thread(new ThreadStart(analyzeMessages));
            messageAnalysisThread.Start();

            commandAnalysisThread = new Thread(new ThreadStart(analyzeCommands));
            commandAnalysisThread.Start();

        }

        ~Communication(){
            Dispose(false);                     
        }
        protected override void Run(){
            while(true){
                if(messageQueue.Count > 0){
                    var message = messageQueue.Dequeue();

                    Console.WriteLine("Recebi de "+ message.Source+": "+message.Command);
                }

                Thread.Sleep(1100);
            }

        }

        public bool waitForWordBoolean(string word, int seconds){

        var task = Task.Run(() => checkForWord(word));
            if (task.Wait(TimeSpan.FromSeconds(seconds)))
                return true;
            else
                return false;
        }

        private bool checkForWord(string word){
            while(true){
                if(rawWords.Any(x => Soundex.Generate(word) == Soundex.Generate(x.Word))){
                    return true;
                }
            }

        }

        public List<VoiceWord> waitForWord(string word, int seconds, int applicationId){

        var task = Task.Run(() => checkForWord(word));
            if (task.Wait(TimeSpan.FromSeconds(seconds)))
                return getWordMessage(word,applicationId);
            else
                return null;
        }

        private List<VoiceWord> getWordMessage(string word, int applicationId){
            var requestedWord =  rawWords.Where(x => Soundex.Generate(word) == Soundex.Generate(x.Word)).FirstOrDefault();
    
            if(requestedWord != null){
                var message = rawWords.Where(x => x.MessageId == requestedWord.MessageId).ToList();
                return message;
            }
            else{
                return null;
            }

        }


        public bool wordWasSaid(string word){

            if(rawWords.Any(x => Soundex.Generate(word) == Soundex.Generate(x.Word))){
                return true;
            }
            else{
                return false;
            }
        }

        public string [] getLatestWords(int wordCount){
            var words = rawWords.OrderByDescending(x => x.TimeStamp).Take(wordCount).Select(y => y.Word);
            return words.ToArray();

        }

        public string [] listenWordsForSeconds (int timeCount){
            //usar timestamp pra pegar as palavras
            var currentTime = DateTime.UtcNow;

            Thread.Sleep( timeCount*1000);
            var endTime = DateTime.UtcNow;

            var words = rawWords.Where(x => x.TimeStamp > currentTime && x.TimeStamp < endTime).Select(y => y.Word);


            return words.ToArray();
        }


        public bool waitForCommandBoolean(string command, int seconds, int applicationId){

            var task = Task.Run(() => checkForCommand(command, applicationId));
            if (task.Wait(TimeSpan.FromSeconds(seconds)))
                return true;
            else
                return false;
        }

        public List<VoiceWord> waitForCommand(string command, int seconds, int applicationId){
            var task = Task.Run(() => checkForCommand(command, applicationId));
            if (task.Wait(TimeSpan.FromSeconds(seconds)))
                return getCommandMessage( command,  applicationId);
            else
                return null;
        }

        private bool checkForCommand(string word, int applicationId){
            while(true){
                if(processedWords.Any(x => Soundex.Generate(word) == Soundex.Generate(x.Word) && x.WordType == VoiceWord.wordType.ApplicationCommand && x.ApplicationIdCommand == applicationId)){
                    return true;
                }
            }

        }

        private List<VoiceWord> getCommandMessage(string word, int applicationId){
            var command =  processedWords.Where(x => Soundex.Generate(word) == Soundex.Generate(x.Word) && x.WordType == VoiceWord.wordType.ApplicationCommand && x.ApplicationIdCommand == applicationId).FirstOrDefault();
    
            if(command != null){
                var message = rawWords.Where(x => x.MessageId == command.MessageId).ToList();
                return message;
            }
            else{
                return null;
            }

        }

        public bool commandWasSaid(string command, int applicationId){

                if(processedWords.Any(x => Soundex.Generate(command) == Soundex.Generate(x.Word) && x.WordType == VoiceWord.wordType.ApplicationCommand && x.ApplicationIdCommand == applicationId)){
                return true;
            }
            else{
                return false;
            }
        }

        public string [] getLatestCommands(int commandCount, int applicationId){
            var words = processedWords.Where(x=> x.WordType == VoiceWord.wordType.ApplicationCommand && x.ApplicationIdCommand == applicationId).OrderByDescending(x => x.TimeStamp).Take(commandCount).Select(y => y.Word);
            return words.ToArray();
        }

        public string [] listenCommandsForSeconds (int timeCount, int applicationId){
            //usar timestamp pra pegar as palavras
            
            var currentTime = DateTime.UtcNow;

            Thread.Sleep( timeCount*1000);
            var endTime = DateTime.UtcNow;

            var words = processedWords.Where(x => x.WordType == VoiceWord.wordType.ApplicationCommand && x.TimeStamp > currentTime && x.TimeStamp < endTime && x.ApplicationIdCommand == applicationId).Select(y => y.Word);


            return words.ToArray();
        }

        public string speak(string sentence){
            voiceOutput.UpdateMessage(sentence);
            return "success";
        }

        public string setEmotion(string sentence){

            return "deu boa";
        }

        public string getEmotion(){

            return "deu boa";
        }

        private void queueMessage(){
            string currentMessage = "";
            long wordCount ;
            while(true){
                if(voiceInput.Message != "" && voiceInput.Message != null){
                    currentMessage = voiceInput.Message;
                     voiceInputClear.UpdateMessage("");
                    messageCount++;
                    var wordArray =   currentMessage.Split(" ");
                    var timeStamp = DateTime.UtcNow;
                    wordCount =0;
                    foreach (var item in wordArray)
                    {
                        wordCount++;
                        var voiceWord = new VoiceWord(item,timeStamp,wordCount,messageCount);
                        rawWords.Add(voiceWord);
                    }
                  

                }
                rawWords.RemoveAll(x => x.TimeStamp.AddMinutes(1) < DateTime.UtcNow);

                Thread.Sleep(200);


            }

        }

        private void analyzeMessages(){
            
            while(true){
                var toAnalyze = rawWords.FindAll(x => !x.IsProcessed);
                if(toAnalyze.Count > 0){
                    foreach (VoiceWord item in toAnalyze)
                    {
                        var result = sendMessageAndWait("Database","analyzeWord",new  string[]{JsonConvert.SerializeObject(item)});
                        var list = JsonConvert.DeserializeObject<List<VoiceWord>>(result);
                        foreach(var word in list){
                            processedWords.Add(word);
                        }
                        rawWords.Find(x => x.WordId == item.WordId && x.MessageId == item.MessageId).IsProcessed = true;
                    }
                    var alias = processedWords.Where(x => x.WordType == VoiceWord.wordType.Alias).FirstOrDefault();
                    if(alias != null){
                        var robotCommandMessage = processedWords.Where( x=> x.MessageId == alias.MessageId).ToList();

                        //send command to processing thread

                    }



                }
                
                
                Thread.Sleep(100);
            }

        }

        private void analyzeCommands(){

            while(true){
                var alias = possibleCommands.Where(x=> x.WordType == VoiceWord.wordType.Alias).FirstOrDefault();
                if(alias != null){
                    var message = possibleCommands.Where(x => x.MessageId == alias.MessageId).ToList();
                    possibleCommands.RemoveAll(x => x.MessageId == alias.MessageId);
                    var systemCommand = message.Where(x=> x.WordType == VoiceWord.wordType.SystemCommand).FirstOrDefault();
                    if(systemCommand != null){
                        var result = sendMessageAndWait("Database","getSystemCommand",new  string[]{JsonConvert.SerializeObject(systemCommand)});
                        var command = JsonConvert.DeserializeObject<SystemCommand>(result);
                        switch(command.Action){
                            case "open" :
                                var callWord = message.Where(x => x.WordType == VoiceWord.wordType.CallWord).FirstOrDefault();
                                if(callWord != null){
                                    sendMessage("AppManager","openApp",new  string[]{callWord.Word});
                                }
                                break;  
                        }

                    }

                }   
                Thread.Sleep(100);
            }
        }
    




    }

}
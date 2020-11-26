using System;
using Ready.Models;
using System.Collections.Generic;

namespace Ready
{
    public class ReadyCommunication : ServiceConnection
    {
        public ReadyCommunication(ReadyConnection conn)
        {
            base.init(conn);
        }

        public bool waitForWordBoolean(string word, int secondsToWait = 8){
            var package = assembleDataPackage<string>(word);
            var url = "api/Communication/WaitForWord?isBoolean=true&seconds="+secondsToWait;
            var result = sendPackage<bool>(url,package).Result  ;
            return result;

        }
        public List<VoiceWord> waitForWord(string word, int secondsToWait = 8){
            var package = assembleDataPackage<string>(word);
            var url = "api/Communication/WaitForWord?isBoolean=false&seconds="+secondsToWait;
            var result = sendPackage<List<VoiceWord>>(url,package).Result  ;
            return result;

        }

        public bool wordWasSaid(string word){
            var package = assembleDataPackage<string>(word);
            var url = "api/Communication/WordWasSaid";
            var result = sendPackage<bool>(url,package).Result  ;
            return result;
        }
        public string [] getLatestWords(int numberOfWords){
            var package = assembleDataPackage<int>(numberOfWords);
            var url = "api/Communication/GetLatestWords";
            var result = sendPackage<string[]>(url,package).Result  ;
            return result;
        }
        public string [] listenWordsForSeconds(int seconds){
            var package = assembleDataPackage<int>(seconds);
            var url = "api/Communication/ListenWordsForSeconds";
            var result = sendPackage<string[]>(url,package).Result  ;
            return result;
        }

        public List<VoiceWord> waitForCommand(string command, int secondsToWait){
            var package = assembleDataPackage<string>(command);
            var url = "api/Communication/WaitForCommand?isBoolean=false&seconds="+secondsToWait;
            var result = sendPackage<List<VoiceWord>>(url,package).Result  ;
            return result;
        }

        public bool waitForCommandBoolean(string command, int secondsToWait){
            var package = assembleDataPackage<string>(command);
            var url = "api/Communication/WaitForCommand?isBoolean=true&seconds="+secondsToWait;
            var result = sendPackage<bool>(url,package).Result  ;
            return result;
        }

        public bool commandWasSaid(string command){
            var package = assembleDataPackage<string>(command);
            var url = "api/Communication/CommandWasSaid";
            var result = sendPackage<bool>(url,package).Result  ;
            return result;

        }
        public string [] getLatestCommands(int numberOfCommands){
            var package = assembleDataPackage<int>(numberOfCommands);
            var url = "api/Communication/GetLatestCommands";
            var result = sendPackage<string[]>(url,package).Result  ;
            return result;
        }
        public string [] listenCommandsForSeconds(int seconds){
            var package = assembleDataPackage<int>(seconds);
            var url = "api/Communication/ListenCommandsForSeconds";
            var result = sendPackage<string[]>(url,package).Result  ;
            return result;
        }

        public string speak(string word){
            var package = assembleDataPackage<string>(word);
            var url = "api/Communication/Speak";
            var result = sendPackage<string>(url,package).Result  ;
            return result;

        }
    }
}

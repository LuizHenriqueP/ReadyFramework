using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Whitebox.Models{
    public class VoiceWord{
        public enum wordType {Raw,Alias,SystemCommand,ApplicationCommand,CallWord}
        public string Word {get;set;}
        public DateTime TimeStamp{get;set;}
        public long WordId {get;set;}
        public long MessageId{get;set;}
        public bool IsProcessed {get;set;} = false;
        public int ApplicationIdCommand {get;set;}
        
        public wordType WordType {get;set;}

        public VoiceWord(string word, DateTime timeStamp, long wordId, long messageId, bool isProcessed = false, wordType wordType = wordType.Raw, int applicationIdCommand = -1 ){
            Word = word;
            TimeStamp = timeStamp;
            WordId = wordId;
            MessageId = messageId;
            IsProcessed = isProcessed;
            WordType = wordType;
            ApplicationIdCommand = applicationIdCommand;
        }


    }

}
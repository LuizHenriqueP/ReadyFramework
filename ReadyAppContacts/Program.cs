using System;
using Ready;
using Ready.Models;
using ReadyAppContacts.Models;
using System.Collections.Generic;
using System.Linq;

namespace ReadyAppContacts{
 

    class Program{

        public const int TimeForListen = 15;
        static void Main(string[] args){
            int id = 2;
            var connection = new ReadyConnection(id, "http://192.168.100.142:5123/");
            var commHandler = new ReadyCommunication(connection);
            var sysHandler = new ReadySystem(connection);

            var contactsDatabase = new ContactsDatabase();
            // var closeApp = false;
            var closeAppList = new List<string>();

            do{    
                commHandler.speak("What do you wish? Remove, Include or Search?");       
                var words = commHandler.listenCommandsForSeconds(TimeForListen);
                var wordsList  = new List<string>(words);

                if(wordsList.Exists(x => x == "remove")){
                    commHandler.speak("Who do you wanna remove?");
                    var nameArray = commHandler.listenWordsForSeconds(TimeForListen);
                    var nameList = new List<string>(nameArray);
                    foreach(string name in nameList){
                        var resultNames = contactsDatabase.getContactsName(name);
                        //display phones
                        if(resultNames != null){
                            commHandler.speak("Just for confirm, Do you wanna remove anyone this list?");
                            foreach(string eachName in resultNames){
                                commHandler.speak(eachName+", ");
                            }
                            commHandler.speak(" Please, say me who");
                        }
                        else{
                            commHandler.speak("We don't find nothing in your contacts");
                            break;
                        }
                        var answer  =commHandler.listenWordsForSeconds(TimeForListen);
                        var answerList = new List<string>(answer);  

                        foreach (string eachName in resultNames){
                            var check = answerList.Where(x => x == eachName ).FirstOrDefault();
                            if(check != null){
                                bool removed = contactsDatabase.deleteContact(eachName);
                                if(removed)
                                    commHandler.speak(eachName +" was removed with success");
                                else
                                    commHandler.speak("Failed to remove "+eachName+", Please, try again.");
                                break;
                            }
                        } 
                    }                    
                }

                if(wordsList.Exists(x => x == "include")){
                    commHandler.speak("Who do you wanna include?");                    
                    var nameArray = commHandler.listenWordsForSeconds(TimeForListen);
                    var nameList  = new List<string>(nameArray);                    
                    commHandler.speak("Please, tell me the phone number of everyone, in the same order");
                    var numberArray = commHandler.listenWordsForSeconds(TimeForListen);
                    var numberList  = new List<string>(numberArray);                                        
                    commHandler.speak("Please, tell me the emailof everyone, in the same order");
                    var emailArray = commHandler.listenWordsForSeconds(TimeForListen);
                    var emailList  = new List<string>(emailArray);                                       
                    var i=0;
                    var tamNumber= numberList.Count;
                    var tamEmail= emailList.Count;
                    bool add = false;
                    foreach(string name in nameList){
                        if(i<tamNumber && i<tamEmail)
                            add = contactsDatabase.saveNewContact(name, numberList[i], emailList[i]);
                        else if(tamNumber<i)
                            add = contactsDatabase.saveNewContact(name, "", emailList[i]);
                        else if(tamEmail<i)
                            add = contactsDatabase.saveNewContact(name, numberList[i], "");
                        i++;
                        if(add)
                            commHandler.speak(name +" was included with success");
                        else
                            commHandler.speak("Failed to try include "+name+", Please, try again.");
                    }
                }
                
                if(wordsList.Exists(x => x == "search")){
                    commHandler.speak("Who do you wanna find?");
                    var nameArray = commHandler.listenWordsForSeconds(TimeForListen);
                    var nameList  = new List<string>(nameArray);                    
                    commHandler.speak("Do you wanna the number, email or both?");
                    var whatSearch = commHandler.listenWordsForSeconds(TimeForListen);
                    var whatSearchList  = new List<string>(whatSearch);
                    
                    if(whatSearchList.Exists(x => x == "number") || whatSearchList.Exists(x=>x=="both")){                        
                        foreach(string name in nameList){
                            var number = contactsDatabase.getNumber(name);
                            var numberString = String.Join(" ",number[0].ToArray());
                            commHandler.speak("The "+name+"'s Number is "+numberString);
                        }                                                                                                                    
                        if(whatSearchList.Exists(x=>x=="both"))
                            commHandler.speak(" and ");
                    }
                    if(whatSearchList.Exists(x => x == "email") || whatSearchList.Exists(x=>x=="both")){
                        foreach(string name in nameList){
                            var email = contactsDatabase.getEmail(name);                                                                                        
                            commHandler.speak("The "+name+"'s email is "+email[0]);
                        }
                    }
                }               
                commHandler.speak("Do you wanna do something more with the contacts? Please, answer with Yes or No");
                // closeApp = commHandler.waitForWordBoolean("No");   
                var closeApp = commHandler.listenWordsForSeconds(TimeForListen);
                closeAppList  = new List<string>(closeApp);                                  
            }while(closeAppList.Exists(x => x != "no"));
            // }while (closeApp != false);
            sysHandler.closeApplication();
        }
    }
}

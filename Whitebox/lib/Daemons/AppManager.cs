using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Whitebox.Models;

namespace Whitebox.Daemons{
    public class AppManager :Daemon{

        private List<Application> openApps = new List<Application>();
        private static int appIdCounter = 0;
        private List<string> installedApps = new List<string>();
        private string homePath;

        private List<AppProcess> appProcessList = new List<AppProcess>();

        private Queue<FocusRequest> focusRequests = new Queue<FocusRequest>();
        private int focusRequestCount = 0;
        private Stack<FocusHistory> focusHistory = new Stack<FocusHistory>();

        private readonly object focusLock = new object();
 
        private bool singleFocus = false; 
        Thread processFocusRequestThread; 
        
        private ProcessStartInfo createProcessStartInfo(string appName, string AppType, string appId){
            var info = new ProcessStartInfo();
            info.UseShellExecute = false;
            //
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            info.CreateNoWindow = true;
            appIdCounter++;
            info.Arguments = appId.ToString();
            switch(AppType){           
                case "unix-binary":
                    info.FileName = @homePath+appName+"/"+appName;                    
                    break;
                case "exe":
                    info.FileName = @homePath+appName+"/"+appName+".exe";
                    break;
                    // adicionar outras inicializações               
            }
            return info;

        }



        private string getProgramExecutionType(string appName){
            var path =  @homePath+appName+"/";
            var config = new AppConfig();
            using (StreamReader r = new StreamReader(path+"config.json"))
            {
                string json = r.ReadToEnd();
                config = JsonConvert.DeserializeObject<AppConfig>(json);
            }
            return config.AppType;
        }

        private void openApp(string appName){

            var executionType = getProgramExecutionType(appName);
            var id = sendMessageAndWait("Database","getApplicationId",new string [] {appName});
            var app = new Application(appName,Convert.ToInt32(id));
            ProcessStartInfo info = createProcessStartInfo(appName, executionType, id);

            Process p = new Process();
            p.StartInfo = info;

            var newProcess = new AppProcess(appName,  p, appIdCounter);
            appProcessList.Add(newProcess);


            Thread serviceThread = new Thread(() => processThread(ref newProcess));
          
            openApps.Add(app);
            //setCurrentApp(app); 
            serviceThread.Start();
            Thread.Sleep(500);
            Console.WriteLine(appProcessList.Find(x => x.Name == appName).Process.Id);
        
        }

        private string [] getInstalledApps(){
            homePath = Environment.GetEnvironmentVariable("HOME")+"/.ready/installedApps/";
            Directory.CreateDirectory(homePath);
            var dirs = Directory.GetDirectories(homePath);
            foreach (var item in dirs)
            {
                Console.WriteLine(item);
            }
            return dirs;
        }

        public AppManager(string name){
            base.init(name);
            installedApps = new List<string>(getInstalledApps());

            processFocusRequestThread = new Thread(new ThreadStart(processFocusRequest));
            processFocusRequestThread.Start();

            var testApp = new Application("Teste",1);
            openApps.Add(testApp);

            testApp = new Application("Contacts",2);
            openApps.Add(testApp);

        }

        private void processThread(ref AppProcess process){
            Console.WriteLine("startei process");
            process.Process.Start();
            process.Process.WaitForExit();
            clearCurrentApp();
            Console.WriteLine("Sai vazado");

        }


        ~AppManager(){
            Dispose(false);                     
        }
        protected override void Run(){
            while(true){
                if(messageQueue.Count > 0){
                    var message = messageQueue.Dequeue();

                    switch(message.Command){
                        case "openApp":
                           
                            openApp(message.Parameters[0]);
                            //open app here
                            break;
                        case "closeApp":
                            //close app here
                            break;
                        case "listOpenedApps":
                            //list all open apps
                            break;
                        case "listInstalledApps":
                            //list all installed apps
                            break;
                        case "installApp":
                            //install app here
                            break;
                        case "uninstallApp":
                            //uninstall app here
                            break;
                        case "getCurrentApp":
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

        public void newFocusRequest(int applicationId){
            focusRequestCount++;
            var request = new FocusRequest(focusRequestCount,applicationId);
            focusRequests.Enqueue(request);
        }

        public string setAsBackground(int applicationId){
            var response = "This application is not the current";
            if(isCurrentApplication(applicationId)){
                clearCurrentApp();
                response = "Running on background";
            }
            return response;
        }

        public void setFocusLock(int applicationId){
            if(isCurrentApplication(applicationId)){
                if(!singleFocus){
                    setSingleFocus();
                    updateCurrentAppStatus(Application.FocusStatus.Locked);
                }
            }
            
        }

        public void disableFocusLock(int applicationId){
            if(isCurrentApplication(applicationId)){
                if(singleFocus){
                    disableSingleFocus();
                    updateCurrentAppStatus(Application.FocusStatus.Pending);
                }
            }
            
        }

        

        private void processFocusRequest(){


            while(true){
                if(!singleFocus){
                    var currentApp = getCurrentApp();
                    if(currentApp == null || (currentApp?.focusStatus == Application.FocusStatus.Finished) || (currentApp?.focusStatus == Application.FocusStatus.Pending && (currentApp?.LastStatusChange - DateTime.UtcNow)?.TotalSeconds > 5)){
                        if(focusRequests.Count > 0){
                            var request = focusRequests.Dequeue();
                            var app = openApps.Find(x => x.Id == request.ApplicationId);
                            if(app != null){
                                setCurrentApp(app);
                            }
                        }
                        else{
                            clearCurrentApp();
                        }
                    }   
                }
                Thread.Sleep(100);
               
            }
        }

        private void setSingleFocus(){

            lock(focusLock){
                singleFocus = true;
            }
        }

        private void disableSingleFocus(){

            lock(focusLock){
                singleFocus = false;
            }
        }

        public string closeApplication(int applicationId){
            var process = appProcessList.Where(x => x.Id == applicationId).FirstOrDefault();

            if(process != null){
                process.Process.Kill();
                openApps.RemoveAll(x => x.Id == applicationId);
                return "Application Closed";
            }
            return "Application Not Found";

        }


    }

}
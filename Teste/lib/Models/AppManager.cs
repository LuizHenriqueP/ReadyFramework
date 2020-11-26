using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Whitebox.Models{
    public class AppManager :Service{

        private List<string> openApps = new List<string>();
        private List<string> installedApps = new List<string>();
        private string homePath;

        private List<AppProcess> appProcessList = new List<AppProcess>();
        
        private ProcessStartInfo createProcessStartInfo(string appName, string AppType){
            var info = new ProcessStartInfo();
            info.UseShellExecute = false;
            //
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            info.CreateNoWindow = true;
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
            ProcessStartInfo info = createProcessStartInfo(appName, executionType);

            Process p = new Process();
            p.StartInfo = info;

            var newProcess = new AppProcess(appName,  p);
            appProcessList.Add(newProcess);


            Thread serviceThread = new Thread(() => processThread(ref newProcess));
            setCurrentApp(appName); 
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


    }

}
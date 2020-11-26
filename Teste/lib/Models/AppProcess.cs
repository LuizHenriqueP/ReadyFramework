using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Whitebox.Models{
    public class AppProcess{
        public string Name {get;set;}
        public Process Process{get;set;}

        public AppProcess(string name, Process process){
            Name = name;
            Process = process;
        }


    }

}
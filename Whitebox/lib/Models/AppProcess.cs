using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Whitebox.Models{
    public class AppProcess{
        public string Name {get;set;}
        public Process Process{get;set;}
        public int Id {get;}

        public AppProcess(string name, Process process, int id){
            Name = name;
            Process = process;
            Id = id;
        }


    }

}
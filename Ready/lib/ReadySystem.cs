using System;
using Ready.Models;

namespace Ready
{
    public class ReadySystem : ServiceConnection
    {
        public ReadySystem(ReadyConnection conn)
        {
            base.init(conn);
        }

        public string requestFocus(){
            var package = assembleDataPackage<string>("focus request");
            var url = "api/System/RequestFocus";
            var result = sendPackage<string>(url,package).Result  ;
            return result;

        }

        public string setAsBackground(){
            var package = assembleDataPackage<string>("set as background");
            var url = "api/System/SetAsBackground";
            var result = sendPackage<string>(url,package).Result  ;
            return result;

        }

        public string lockFocus(){
            var package = assembleDataPackage<string>("lock focus");
            var url = "api/System/LockFocus";
            var result = sendPackage<string>(url,package).Result  ;
            return result;

        }

        public string unlockFocus(){
            var package = assembleDataPackage<string>("unlock focus");
            var url = "api/System/UnlockFocus";
            var result = sendPackage<string>(url,package).Result  ;
            return result;

        }

        public bool isCurrentApplication(){
            var package = assembleDataPackage<string>("is current application");
            var url = "api/System/IsCurrentApplication";
            var result = sendPackage<bool>(url,package).Result  ;
            return result;

        }

        public string closeApplication(){
            var package = assembleDataPackage<string>("close application");
            var url = "api/System/CloseApplication";
            var result = sendPackage<string>(url,package).Result  ;
            return result;

        }


    }
}

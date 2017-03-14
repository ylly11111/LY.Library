using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.Logging
{
    public class Log
    {

        static log4net.ILog log = log4net.LogManager.GetLogger("logerror");
        static log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");

        public static void SetConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void Write(string msg)
        {
            log.Error(msg);
        }

        public static void WriteLog(string msg)
        {
            loginfo.Info(msg);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WitBird.XiaoChangHe
{
    public enum LoggingLevel
    {
        /// <summary>
        /// 一般Log，写入log目录下
        /// </summary>
        Normal,
        /// <summary>
        /// 支付log, 写入log/wxpay目录下
        /// </summary>
        WxPay
    }

    public static class Logger
    {
        /// <summary>
        /// 一般log
        /// </summary>
        public static string LogPath = HttpContext.Current.Request.PhysicalApplicationPath + "log";

        /// <summary>
        /// 支付log
        /// </summary>
        public static string WxPayLogPath = HttpContext.Current.Request.PhysicalApplicationPath + "log/wxpay";

        public static void Log(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Log(LoggingLevel.Normal, message);
            }
        }

        public static void Log(Exception exception)
        {
            if (exception != null)
            {
                Log(LoggingLevel.Normal, exception);
            }
        }

        public static void Log(LoggingLevel level, Exception exception)
        {
            if (exception != null)
            {
                Log(level, exception.ToString());
            }
        }

        public static void Log(LoggingLevel level, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            string path = LogPath;

            if (level == LoggingLevel.WxPay)
            {
                path = WxPayLogPath;
            }

            if (!Directory.Exists(path))//如果日志目录不存在就创建
            {
                Directory.CreateDirectory(path);
            }

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
            string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名

            //创建或打开日志文件，向日志文件末尾追加记录
            StreamWriter mySw = File.AppendText(filename);

            //向日志文件写入内容
            string write_content = time + "\r\n" + message;
            mySw.WriteLine(write_content);

            //关闭日志文件
            mySw.Close();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TestDBFirstCient
{
    public static class TGZZZLog
    {
        // 改行コード
        public const string NEW_LINE_CODE = "\n";
        // 復帰コード
        public const string CARRIAGE_RETURN_CODE = "\r";
        // タブコード
        public const string TAB_CODE = "\t";
        // ログレベル（情報）
        public const string LOG_LEVEL_INFO = "INFO";
        // ログレベル（警告）
        public const string LOG_LEVEL_WARN = "WARN";
        // ログレベル（エラー）
        public const string LOG_LEVEL_ERROR = "ERROR";
        // ログレベル（デバッグ）
        public const string LOG_LEVEL_DEBUG = "DEBUG";
        // ログレベル（致命的）
        public const string LOG_LEVEL_FATAL = "FATAL";
        // ログレベル（イベントログ - 情報）
        public const string EVENT_LOG_LEVEL_INFO = "INF";
        // ログレベル（イベントログ - 警告）
        public const string EVENT_LOG_LEVEL_WARN = "WAR";
        // ログレベル（イベントログ - エラー）
        public const string EVENT_LOG_LEVEL_ERROR = "ERR";

        // 汎用ログ例外文言
        public const string DEFAULT_ERR_MESSAGE = "のログファイルの出力に失敗しました。";

        public static void StartLog()
        {
            Console.WriteLine("Start Log");

        }

        public static void WriteLogFile_ERR(string message, string sessionID, string memberID, Exception e)
        {
            Console.WriteLine("WriteLogFile_ERR message: {0}, sessionID : {1}, memberID: {2}, ", message, sessionID, memberID, e.Message);
        }

        public static void WriteEventLog_ERR(string message)
        {
            Console.WriteLine("WriteEventLog_ERR message: {0}", message);

        }

        public static void EndLog()
        {
            Console.WriteLine("End Log");

        }
        public static void WriteEventLog(String logLevel, String logMessage)
        {
            Console.WriteLine("WriteEventLog" + logLevel + ", " + logMessage);

        }
        public static void WriteLogFile_INFO(string message, string sessionID, string memberID)
        {
            Console.WriteLine("WriteLogFile_INFO: " + message + ", " + sessionID + ", " + memberID);

        }
        public static void ZacLog(string message)
        {
            Console.WriteLine(message);
        }

        internal static void WriteLogFile_ERR(string v1, string v2, string v3)
        {
            throw new NotImplementedException();
        }
    }
}

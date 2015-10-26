using System;
using log4net;

namespace LoveBank.Common
{
    public static class Log
    {
        private static ILog _log;
        private static readonly object SyncLock = new object();

        public static ILog Current
        {
            get
            {
                if (_log == null) throw new ArgumentNullException("_log", "Default LogService don't be setted.");
                return _log;
            }
            private set
            {
                lock (SyncLock)
                {
                    _log = value;
                }
            }
        }

        public static void SetLog(ILog log)
        {
            Current = log;
        }

        public static void Error(object message, Exception exception)
        {
            _log.Error(message, exception);
        }

        public static void Error(object message)
        {
            _log.Error(message);
        }

        public static void Info(object message, Exception exception)
        {
            _log.Info(message, exception);
        }

        public static void Info(object message)
        {
            _log.Info(message);
        }

        public static void Debug(object message, Exception exception)
        {
            _log.Debug(message, exception);
        }

        public static void Debug(object message)
        {
            _log.Debug(message);
        }

        public static void Fatal(object message, Exception exception)
        {
            _log.Fatal(message, exception);
        }

        public static void Fatal(object message)
        {
            _log.Fatal(message);
        }
    }
}
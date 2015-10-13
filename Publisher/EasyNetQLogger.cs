using System;
using EasyNetQ;
using log4net;

namespace Publisher
{
    internal class EasyNetQLogger : IEasyNetQLogger
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(EasyNetQLogger));

        public void DebugWrite(string format, params object[] args)
        {
            if(args != null && args.Length > 0)
            {
                Log.DebugFormat(format, args);

            }
            else
            {
                Log.Debug(format);
            }
        }

        public void ErrorWrite(Exception exception)
        {
            Log.Error(exception);
        }

        public void ErrorWrite(string format, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                Log.ErrorFormat(format, args);

            }
            else
            {
                Log.Error(format);
            }
        }

        public void InfoWrite(string format, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                Log.InfoFormat(format, args);

            }
            else
            {
                Log.Info(format);
            }
        }
    }
}
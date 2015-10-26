using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Common
{


    public class IdWorker
    {
        //static string localIP = Utils.GetIP();
        //static string[] ips = localIP.Split('.');
        //public IdWorker()
        //    : this(long.Parse(ips[2] + ips[3]), long.Parse(ips[0] + ips[1]))
        //{


        //}
        public IdWorker()
            : this(0, 1)
        {


        }

        private static Object lockObj = new object();
        private long workerId;
        private long datacenterId;
        private long sequence = 0L;

        private long twepoch = 1288834974657L;

        private const int workerIdBits = 5;
        private const int datacenterIdBits = 5;
        private long maxWorkerId = int.MaxValue;
        private long maxDatacenterId = int.MaxValue;
        private const int sequenceBits = 12;

        private int workerIdShift = sequenceBits;
        private int datacenterIdShift = sequenceBits + workerIdBits;
        private int timestampLeftShift = sequenceBits + workerIdBits + datacenterIdBits;
        private long sequenceMask = -1L ^ (-1L << sequenceBits);
        private long lastTimestamp = -1L;

        public IdWorker(long workerId, long datacenterId)
        {
            // sanity check for workerId
            if (workerId > maxWorkerId || workerId < 0)
            {
                throw new Exception(String.Format("worker Id can't be greater than %d or less than 0", maxWorkerId));
            }
            if (datacenterId > maxDatacenterId || datacenterId < 0)
            {
                throw new Exception(String.Format("datacenter Id can't be greater than %d or less than 0", maxDatacenterId));
            }
            this.workerId = workerId;
            this.datacenterId = datacenterId;
            //LOG.info(String.format("worker starting. timestamp left shift %d, datacenter id bits %d, worker id bits %d, sequence bits %d, workerid %d", timestampLeftShift, datacenterIdBits, workerIdBits, sequenceBits, workerId));
        }

        public long nextId()
        {
            lock (lockObj)
            {
                long timestamp = timeGen();

                if (timestamp < lastTimestamp)
                {
                    //LOG.error(String.format("clock is moving backwards.  Rejecting requests until %d.", lastTimestamp));
                    throw new Exception(String.Format("Clock moved backwards.  Refusing to generate id for %d milliseconds", lastTimestamp - timestamp));
                }

                if (lastTimestamp == timestamp)
                {
                    sequence = (sequence + 1) & sequenceMask;
                    if (sequence == 0)
                    {
                        timestamp = tilNextMillis(lastTimestamp);
                    }
                }
                else
                {
                    sequence = 0L;
                }

                lastTimestamp = timestamp;

                return ((timestamp - twepoch) << timestampLeftShift) | (datacenterId << datacenterIdShift) | (workerId << workerIdShift) | sequence;
            }
        }

        protected long tilNextMillis(long lastTimestamp)
        {
            long timestamp = timeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = timeGen();
            }
            return timestamp;
        }

        protected long timeGen()
        {

            long currenttimemillis = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

            return currenttimemillis;
        }


        //private static IdWorker worker = null;
        //public static IdWorker GetInstance()
        //{

        //    if (worker == null)
        //    {
        //        lock (lockObj)
        //        {
        //            string localIP = Utils.GetIP();
        //            string[] ips = localIP.Split('.');

        //            long datacenterId = long.Parse(ips[0] + ips[1]);
        //            long workerId = long.Parse(ips[2] + ips[3]);

        //            worker = new IdWorker(workerId, datacenterId);
        //        }

        //    }
        //    return worker;
        //}

   
        ///// <summary>
        ///// 自定义全局long类型唯一Id比GUID字符串更高效
        ///// </summary>
        ///// <returns></returns>
        //public long GetId()
        //{


        //    long datacenterId = long.Parse(ips[0] + ips[1]);
        //    long workerId = long.Parse(ips[2] + ips[3]);


        //    return new IdWorker(0, 1).nextId();
        //}

   
    }
}

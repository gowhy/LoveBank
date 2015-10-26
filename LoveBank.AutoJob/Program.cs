using System;
using Microsoft.Owin.Hosting;
using Owin;
using Quartz;
using Quartz.Impl;
using LoveBank.Common;
using LoveBank.Common.Data;
using LoveBank.Common.Config;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Threading;


namespace LoveBank.AutoJob
{
    class Program {
        static void Start(IAppBuilder app) {

          //var ids=  IdWorker.GetId();
            
            IdWorker workId = new IdWorker();
            long id = workId.nextId();
          
            IList<long> list = new List<long>();
            
            list.Add(id);
          
            //for (int i = 0; i < 1000; i++)
            //{
            //    Task t1 = new Task(() => { list.Add(workId.nextId()); Console.WriteLine(workId.nextId()); });
            //    t1.Start();
            //    Task t2 = new Task(() => { list.Add(workId.nextId()); Console.WriteLine(workId.nextId()); });
            //    t2.Start();
            //    Task t3 = new Task(() => { list.Add(workId.nextId()); Console.WriteLine(workId.nextId()); });
            //    t3.Start();

            //    Task t4 = new Task(() => { list.Add(workId.nextId()); Console.WriteLine(workId.nextId()); });
            //    t4.Start();
            //    Task t5 = new Task(() => { list.Add(workId.nextId()); Console.WriteLine(workId.nextId()); });
            //    t5.Start();
            //    Task t6 = new Task(() => { list.Add(workId.nextId()); Console.WriteLine(workId.nextId()); });
            //    t6.Start();

            //    Task t7 = new Task(() => { list.Add(workId.nextId()); Console.WriteLine(workId.nextId()); });
            //    t7.Start();
            //    Task t8 = new Task(() => { list.Add(workId.nextId()); Console.WriteLine(workId.nextId()); });
            //    t8.Start();
            //    Task t9 = new Task(() => { list.Add(workId.nextId()); Console.WriteLine(workId.nextId()); });
            //    t9.Start();

            //    Task t10 = new Task(() => { list.Add(workId.nextId()); Console.WriteLine(workId.nextId()); });
            //    t10.Start();
            //    //Task t11 = new Task(() => { list.Add(workId.nextId()); Console.WriteLine(workId.nextId()); });
            //    //t11.Start();
            //    //Task t12 = new Task(() => { list.Add(workId.nextId()); Console.WriteLine(workId.nextId()); });
            //    //t12.Start();


            //}
            Hashtable tb = new Hashtable();
            Task[] tas = new Task[10];
            //tb.Add(1, 1);
            //tb.Add(1, 1);
            for (int i = 0; i < tas.Length; i++)
            {
                //tas[i] = new Task(() => { list.Add(workId.nextId()); Console.WriteLine(workId.nextId()); });
                tas[i] = new Task(() => {
                    for (int j= 0; j < 10; j++)
                    {
                        //var idsb =(new IdWorker(0,1)).GetId();
                        var idsb = workId.nextId();
                        tb.Add(idsb, idsb); Console.WriteLine(idsb);
                    }
                   
                });
                tas[i].Start();
            }
            Task.WaitAll(tas);
 //           List<RawWeeklyAccounts> list;
 
 //var q=from r in list.GroupBy(x=>x.MsuName)
 //      order r by r.weeknumber 
 //      select new WeeklyAccounts
 //      {
 //         Weeknumber=r.weeknumber,
 //         TotalAccounts=r.TotalAccounts 
 //      };

            //var s = from c in list.GroupBy(x=>x) orderby c select c ; 

            return;
            // First, initialize Quartz.NET as usual. In this sample app I'll configure Quartz.NET by code.
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler();
            scheduler.Start();

            // I'll add some global listeners
            //scheduler.ListenerManager.AddJobListener(new GlobalJobListener());
            //scheduler.ListenerManager.AddTriggerListener(new GlobalTriggerListener());

            // A sample trigger and job
            var trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger")
                .WithSchedule(DailyTimeIntervalScheduleBuilder.Create()
                    .WithIntervalInSeconds(60*10))//10分钟执行一次
                .StartNow()
                .Build();
            var job = new JobDetailImpl("crawlInfoJob", null, typeof(CrawlInfoJob));
            scheduler.ScheduleJob(job, trigger);

         

        
       
        }
 

        private static void Main(string[] args) {

         
            using (WebApp.Start("http://localhost:12345", Start))
                Console.ReadLine();
        }
    }
}
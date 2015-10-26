using System;
using System.Threading;
using Quartz;
using LoveBank.Core;
using LoveBank.Common;
using LoveBank.Core.Domain;
using LoveBank.Common.Data;
using System.Linq;
using LoveBank.AutoJob.DataLayer;
using Ivony.Html.Parser;
using Ivony.Html;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LoveBank.AutoJob
{
    /// <summary>
    /// A sample dummy job
    /// </summary>
    //[DisallowConcurrentExecution]
    public class CrawlInfoJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            System.Console.WriteLine(" RunCrawlJob()执行开始" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            RunCrawlJob();
            System.Console.WriteLine(" RunCrawlJob()执行完成" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public static void RunCrawlJob()
        {

            List<Crawl_Data_Item_Selector> listItemSelector = null;
            using (CrawlDBContext db = new CrawlDBContext())
            {
                listItemSelector = db.DBSet_Crawl_Data_Item_Selector.Where(x => x.State == 0).OrderBy(x => x.ID).ToList();
                Crawl_Data_Item modelTmp = null;
                foreach (var item in listItemSelector)
                {
                    string Url = item.Url;//原Url

                    Uri uri = new Uri(Url);
                    IHtmlDocument doc = new JumonyParser().LoadDocument(Url);

                    if (!string.IsNullOrEmpty(item.Encoding))
                    {
                        doc = new JumonyParser().LoadDocument(Url, Encoding.GetEncoding(item.Encoding));
                    }
                    //var doc = new JumonyParser().LoadDocument(Url, Encoding.UTF8);
                    for (int i = 0; i < doc.Find(item.TitleSelector).ToList().Count; i++)
                    {
                        var title = doc.Find(item.TitleSelector).ToList()[i].InnerText();//标题：标题内容

                        if (db.DBSet_Crawl_Data_Item.Count(x => x.Title == title) > 0)
                        {
                            continue;
                        }
                        string link = "" + doc.Find(item.GOUrlSelector).ToList()[i].Attribute("href").Value();//链接

                        string publicDate = doc.Find(item.PublicDateSelector).ToList()[i].InnerText();//日期

                        string docurl2 = string.Empty;
                        if (!link.ToLower().Contains("http://") && !link.ToLower().Contains("https://"))
                        {
                            link = new Uri(uri, link).ToString();
                        }

                        modelTmp = new Crawl_Data_Item();
                        modelTmp.AddTime = DateTime.Now;
                        modelTmp.Crawl_Data_Item_Selector_Id = item.ID;
                        modelTmp.Url = link;
                        modelTmp.Title = title;
                        modelTmp.SourceUrl = Url;
                        modelTmp.Source = item.Source;
                        if (!string.IsNullOrEmpty(item.PublicDateFormat) && item.PublicDateFormat == "{yyyy-}MM-dd")
                        {
                            modelTmp.PublicDate = DateTime.Parse(DateTime.Now.Year+"-"+publicDate); //{yyyy-}MM-dd
                        }
                        else
                        {
                            modelTmp.PublicDate = DateTime.Parse(publicDate);
                        }

                        db.AddAsync<Crawl_Data_Item>(modelTmp);
                    }

                    Thread.Sleep(1000 * 60);
                }
            }
        }
    }
}
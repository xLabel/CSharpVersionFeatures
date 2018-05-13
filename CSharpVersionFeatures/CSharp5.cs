using System;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;

namespace CSharpVersionFeatures
{
    /// <summary>
    /// C#5.0中主要增加了两个特性：
    /// 1、异步编程TAP & async/await（Async Programming）
    /// 2、调用方信息（Caller Information）
    /// </summary>
    public sealed class CSharp5
    {
        public CSharp5()
        {
            Console.WriteLine("===================C#5.0===================");

            //1、
            this.DownloadWithAPM();
            this.DownloadWithEAP();
            this.DownloadWithTAP();

            //2、
            this.DoProcessing();

            Console.WriteLine("===================C#5.0===================");
        }

        private void DownloadWithAPM()
        {
            //APM模式（C#1.0），典型特征BeginXxx和EndXxx必须成对调用。BeginXxx用于触发任务的执行，EndXxx则用于获得任务执行的返回
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri("http://www.bearky-faucet.com/robots.txt"));
            //BeginXxx
            httpWebRequest.BeginGetResponse((asyncResult) =>
            {
                HttpWebRequest preHttpWebRequest = (HttpWebRequest)asyncResult.AsyncState;
                var webResponse = preHttpWebRequest.EndGetResponse(asyncResult);
                //EndXxx
                var stream = webResponse.GetResponseStream();
                using (StreamReader reader = new StreamReader(stream))
                {
                    Console.WriteLine("===APM C#1.0===");
                    Console.WriteLine(reader.ReadToEnd());
                }
            }, httpWebRequest);
        }

        private void DownloadWithEAP()
        {
            //EAP模式（C#2.0），典型特征是一个Async结尾的方法和Completed结尾的事件
            var webClient = new WebClient();
            webClient.DownloadStringCompleted += (s, e) =>
            {
                Console.WriteLine("===EAP C#2.0===");
                Console.WriteLine(e.Result);
            };
            webClient.DownloadStringAsync(new Uri("http://www.bearky-faucet.com/robots.txt"));
        }

        private async void DownloadWithTAP()
        {
            var webClient = new WebClient();
            var result = await webClient.DownloadStringTaskAsync(new Uri("http://www.bearky-faucet.com/robots.txt"));
            Console.WriteLine("===TAP C#5.0===");
            Console.WriteLine(result);
        }

        private void DoProcessing()
        {
            this.TraceMessage("DoProcessing Start...");
        }

        private void TraceMessage(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            //CallerMemberNameAttribute 调用方法的名称
            //CallerFilePathAttribute 调用者方法所在的源文件地址
            //CallerLineNumberAttribute 方法被调用的行号

            Console.WriteLine("message: " + message);
            Console.WriteLine("member name: " + memberName);
            Console.WriteLine("source file path: " + sourceFilePath);
            Console.WriteLine("source line number: " + sourceLineNumber);
        }
    }
}

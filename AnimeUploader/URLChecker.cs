using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeUploader
{
    class UrlChecker
    {
        private const int ThreadCount = 2;
        private CountdownEvent _countdownEvent;
        private SemaphoreSlim _throttler;

        public void Check(IList<string> urls)
        {
            _countdownEvent = new CountdownEvent(urls.Count);
            _throttler = new SemaphoreSlim(ThreadCount);

            Task.Run( // prevent UI thread lock
                async () => {
                                foreach (var url in urls)
                                {
                                    // do an async wait until we can schedule again
                                    await _throttler.WaitAsync();
                                    ProccessUrl(url); // NOT await
                                }
                                //instead of await Task.WhenAll(allTasks);
                                _countdownEvent.Wait();
                });
        }

        private async void ProccessUrl(string url)
        {
            try
            {
                var page = await new WebClient()
                           .DownloadStringTaskAsync(new Uri(url));
                ProccessResult(page);
            }
            finally
            {
                _throttler.Release();
                _countdownEvent.Signal();
            }
        }

        private void ProccessResult(string page) {/*....*/}
    }
}

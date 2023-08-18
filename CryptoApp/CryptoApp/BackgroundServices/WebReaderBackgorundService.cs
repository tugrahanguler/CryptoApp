using CryptoApp.DataModel;
using CryptoApp.Entities;
using CryptoApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoApp.Services;
using System.CodeDom;
using Microsoft.AspNetCore.Mvc;

namespace CryptoApp.BackgroundServices
{
    public sealed class WebReaderBackgorundService : BackgroundService

    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _servicescopefactory;

        public WebReaderBackgorundService(ILogger<WebReaderBackgorundService> logger,IServiceScopeFactory servicescopefactory)
        {

            _logger = logger;
            _servicescopefactory = servicescopefactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                var a = _servicescopefactory.CreateScope();
                var tickerService = a.ServiceProvider.GetRequiredService<TickerService>();

                var db = _servicescopefactory.CreateScope();
                var _dbcontext = db.ServiceProvider.GetRequiredService<CryptoContext>();
                List<Crypto> MyCryptoData = await _dbcontext.Cryptos.ToListAsync();

                //foreach(TickerProperties datas in b.GetData())
                //{
                  //  Console.WriteLine(datas.pair +" " + datas.last);
                //}


                foreach (TickerProperties data_pair in tickerService.GetData())
                {
                    var isCryptoThere = MyCryptoData.FirstOrDefault(q => q.Pair == data_pair.pair);
                  
                    if (isCryptoThere != null)
                    {

                        var AddDetails = new CryptoDetail
                        {

                            Pair = isCryptoThere.Pair,
                            CurrentPrice = data_pair.last,
                            UpdateDate = DateTime.Now
                        };
                         

                        _dbcontext.UpdatedInformations.Add(AddDetails);

                        var context_Count = _dbcontext.UpdatedInformations.Count(a => a.Pair == data_pair.pair);

                        if (context_Count == 100)
                        {
                           var firstIndex = _dbcontext.UpdatedInformations
                                .Where(q => q.Pair == data_pair.pair)
                                .OrderBy(b => b.UpdateDate)
                                .First();

                            _dbcontext.UpdatedInformations.Remove(firstIndex);
                        }

                    }
                }
                _dbcontext.SaveChanges();
                await Task.Delay(10000);

            }


        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                $"{nameof(WebReaderBackgorundService)} is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }

}

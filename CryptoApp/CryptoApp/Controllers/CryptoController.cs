using CryptoApp.DataModel;
using CryptoApp.Entities;
using CryptoApp.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using System.Xml.Linq;

namespace CryptoApp.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class CryptoController : ControllerBase
    {
        private readonly CryptoContext _dbcontext;



        public CryptoController(CryptoContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
         

        [HttpGet]
        [Route("CryptoList")]
        public async Task<IActionResult> GetCryptoList()
        {
            var PricesPerHour = _dbcontext.Cryptos.Join(_dbcontext.UpdatedInformations,
                cryptodata => cryptodata.Pair,
                CryptoDetailData => CryptoDetailData.Pair,
                (cryptodata, CryptoDetailData) => new
                {
                    cryptodata.Pair,
                    CryptoDetailData.Id,
                    CryptoDetailData.CurrentPrice,
                    CryptoDetailData.UpdateDate


                }).ToList();

            return Ok(PricesPerHour);
        }

        [HttpGet]
        [Route("CryptoByName")]

        public IActionResult GetCryptoByName(string crypto_Name)
        {
            var PricesPerHourByName = _dbcontext.UpdatedInformations.Where(q => q.Pair == crypto_Name.ToUpper());
            
            if (PricesPerHourByName == null)
            {
                return NotFound();
            }

            return Ok(PricesPerHourByName);

        }

        [HttpPost]
        [Route("Crypto")]
        public async Task<IActionResult> PostCrypto([FromBody]CryptoRequestModel obj)
        {

            if(obj== null)
            {
                return BadRequest();
            }

            if(_dbcontext.Cryptos.Count() >= 10)
            {
                return BadRequest("You can't add more than 10 items.");
            }

        
            Crypto crypto = new Crypto();
            crypto.Pair = obj.Pair.ToUpper();


            _dbcontext.Cryptos.Add(crypto);
            _dbcontext.SaveChanges();

            return Ok(crypto);             
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace OlimpiadaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<HomeController> _logger;
        private readonly IDistributedCache cache;

        public HomeController(ILogger<HomeController> logger, IDistributedCache cache)
        {
            _logger = logger;
            this.cache = cache;
        }

        [HttpGet]
        public IEnumerable<Artigo> Get(string pais = "brasil")
        {
            string cacheKey = pais;
            var rng = new Random();

            var cacheValue = cache.Get(cacheKey);
            Artigo[] ret;

            if ((cacheValue?.Length ?? 0) == 0)
            {
                var qtd = cache.Get("qtd_req_base") == null ? "0" : Encoding.UTF8.GetString(cache.Get("qtd_req_base"));

                cache.Set("qtd_req_base", Encoding.UTF8.GetBytes((int.Parse(qtd) + 1).ToString()));

                ret = new Faker<Artigo>("pt_BR")
                    .RuleFor(r => r.Autor, f => f.Name.FullName())
                    .RuleFor(r => r.Categoria, f => f.PickRandom(new[] { "Esporte", "Geral", "País" }))
                    .RuleFor(r => r.Esporte, f => f.PickRandom(new[] { "Futebol", "Natação", "Volei", "Hoquei" }))
                    .RuleFor(r => r.Modalidade, f => f.PickRandom(new[] { "Feminino", "Masculino", "Feminino 100m", "Masculino Borboleta" }))
                    .RuleFor(r => r.Pais, f => f.Address.Country())
                    .RuleFor(r => r.PublicadoEm, f => f.Date.Between(new DateTime(2021, 07, 01), new DateTime(2021, 07, 31)))
                    .RuleFor(r => r.PK, (f, a) => a.PublicadoEm.ToString("yyyy-MM-dd"))
                    .RuleFor(r => r.SK, f => f.Lorem.Sentence(f.Random.Number(5, 8)))
                    .RuleFor(r => r.Url, f => f.Internet.Url())
                    .Generate(150)
                    .ToArray();

                var values = JsonSerializer.Serialize(ret);

                cache.Set(cacheKey, Encoding.UTF8.GetBytes(values), new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
                });
                Thread.Sleep(500);
            }
            else
            {
                var json = Encoding.UTF8.GetString(cacheValue);
                ret = JsonSerializer.Deserialize<Artigo[]>(json);
            }




            return ret;
        }
    }
}

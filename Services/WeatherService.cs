using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceDemo.Services
{
    public class WeatherService : Weather.WeatherBase
    {
        private static readonly string[] Summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

        private readonly ILogger<WeatherService> _logger;

        public WeatherService(ILogger<WeatherService> logger)
        {
            _logger = logger;
        }

        public override Task<WeatherForecastInfo> GetWeather(WeatherRequest request, ServerCallContext context)
        {
            var rng = new Random();
            var result = new WeatherForecastInfo
            {
                //时间转换
                Date = Timestamp.FromDateTimeOffset(new DateTimeOffset(DateTime.Now.AddDays(1))),
                TemperatureC = rng.Next(-20, 55),
                Summary = request.Name
            };
            result.TemperatureF = 32 + (int)(result.TemperatureC / 0.5556);
            return Task.FromResult(result);
        }


        public override Task<WeatherList> GetWeatherList(Empty request, ServerCallContext context)
        {
            var rng = new Random();
            //获取版本
            var data = Enumerable.Range(1, 5).Select(index =>
            {
                var info = new WeatherForecastInfo
                {
                    //时间转换
                    Date = Timestamp.FromDateTimeOffset(new DateTimeOffset(DateTime.Now.AddDays(1))),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = $"{ Summaries[rng.Next(Summaries.Length)]}"
                };
                info.TemperatureF = 32 + (int)(info.TemperatureC / 0.5556);
                return info;
            });
            WeatherList weatherList = new WeatherList();
            weatherList.ListData.Add(data);
            return Task.FromResult(weatherList);
        }
    }
}

﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApi.Data;
using WeatherApi.Models;
using System.Diagnostics;


namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : Controller
    {

        private readonly WeatherApiContext _context;

        public ApiController(WeatherApiContext context)
        {
            _context = context;
        }


        // Requst dac, możliwe, ze GUID i tworzenie nowych ID, mapowanie requesta na metode post
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StationWeatherRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors to the client
            }

            string nr_tel = request.nr_tel;
            string kod = request.kod;
            double? battery = request.bateria;
            double temp = request.temp;
            double press = request.press;
            double humi = request.humi;
            double lat = request.lat;
            double longi = request.longi;

            if (string.IsNullOrEmpty(nr_tel) || string.IsNullOrEmpty(kod))
            {
                return BadRequest("Invalid data: nr_tel and kod must be provided.");
            }
            
            // Jezeli w bazie nie ma stacji
            Guid stationId = Guid.NewGuid();
            if (!(_context.Station.Any(s => s.NrTel == nr_tel && s.Kod == kod)))
            {
                Station station = new Station(stationId, nr_tel, kod, battery, lat, longi);
                _context.Add(station);
                await _context.SaveChangesAsync();
            }
            else 
            {
                var updateData = _context.Station.FirstOrDefault(s => s.Kod == kod && s.Kod == kod);
                if (updateData == null)
                {
                    // error
                    return NotFound("updateData was not found");
                }
                updateData.Bateria = battery;
                updateData.SzerokoscGeo = lat;
                updateData.DlugoscGeo = longi;
                await _context.SaveChangesAsync();
            }
           

            var record = _context.Station.FirstOrDefault(s => s.Kod == kod && s.Kod == kod);
            if (record == null)
            {
                // error
                return NotFound("record was not found");
            }
            stationId = record.StationId; 
            DateTime czas = DateTime.Now;
            Weather weather = new Weather(Guid.NewGuid(),temp, press, humi, czas, stationId);
            _context.Add(weather);
            await _context.SaveChangesAsync();
             

            return Ok();
        }

        [HttpGet("{nr_tel}/{kod}")]
        public IActionResult GetLastWeather(string nr_tel, string kod)
        {
            // sprawdzenie czy taka stacja istnieje

            //Debug.WriteLine("0");
            if (!(_context.Station.Any(s => s.NrTel == nr_tel && s.Kod == kod)))
            {
                return NotFound();
            }

            var record = _context.Station.FirstOrDefault(s => s.NrTel == nr_tel && s.Kod == kod);
            if (record == null)
            {
                // error
                return NotFound("record was not found");
            }
            // znalezc ID tej stacji i zmapowac bateria i polozenie
            Guid stationId = record.StationId;
            double? bateria = record.Bateria;
            double lat = record.SzerokoscGeo;
            double longi = record.DlugoscGeo;
            //Debug.WriteLine($"id: {stationId}");
            //Debug.WriteLine($"bateria: {bateria}");
            //Debug.WriteLine($"lat: {lat}");
            //Debug.WriteLine($"longi: {longi}");
            // znalezc ostatnio pogode  tej stacji i zmapowac dane

            //Debug.WriteLine("1");
            var lastWeather = _context.Weather
                .Where(w => w.StationId == stationId)
                .OrderByDescending(w => w.Czas)
                .FirstOrDefault();
            //Debug.WriteLine("Last Weather:");
            //Debug.WriteLine($"Temperature: {lastWeather?.Temperatura}");
            //Debug.WriteLine($"Pressure: {lastWeather?.Cisnienie}");
            //Debug.WriteLine($"Humidity: {lastWeather?.Wilgotnosc}");
            //Debug.WriteLine($"Time: {lastWeather?.Czas}");
            if (lastWeather == null)
            {
                // No weather record found
                return NotFound();
            }
            // mapowanie danych z pogody
            double temp = lastWeather.Temperatura;
            double press = lastWeather.Cisnienie;
            double humi = lastWeather.Wilgotnosc;
            DateTime time = lastWeather.Czas;
            //Debug.WriteLine("After maping:");
            WeatherResponse weatherResponse = new WeatherResponse(bateria, temp, press, humi, time, lat, longi);
            // zwrocic jako json
            //Debug.WriteLine("Before returning:");
            return Ok(weatherResponse);
        }


        [HttpGet("station/{nr_tel}/{kod}")]
        public IActionResult GetStation(string nr_tel, string kod)
        {
            // sprawdzicz czy taka stacja istnieje

            //Debug.WriteLine("0");
            if (!(_context.Station.Any(s => s.NrTel == nr_tel && s.Kod == kod)))
            {
                return NotFound();
            }

            var record = _context.Station.FirstOrDefault(s => s.NrTel == nr_tel && s.Kod == kod);
            if (record == null)
            {
                // error
                return NotFound("record was not found");
            }
            // znalezc ID tej stacji i zmapowac bateria i polozenie
            double? bateria = record.Bateria;
            double lat = record.SzerokoscGeo;
            double longi = record.DlugoscGeo;
            //Debug.WriteLine($"bateria: {bateria}");
            //Debug.WriteLine($"lat: {lat}");
            //Debug.WriteLine($"longi: {longi}");    
            //Debug.WriteLine("After maping:");
            StationResponse StationResponse = new StationResponse(bateria, lat, longi);
            // zwrocic jako json
            //Debug.WriteLine("Before returning:");
            return Ok(StationResponse);
        }
    }
}
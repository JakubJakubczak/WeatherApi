using Microsoft.AspNetCore.Mvc;
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
                var updateData = _context.Station.FirstOrDefault(s => s.NrTel == nr_tel && s.Kod == kod);
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
           

            var record = _context.Station.FirstOrDefault(s => s.NrTel == nr_tel && s.Kod == kod);
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
 

            var lastWeather = _context.Weather
                .Where(w => w.StationId == stationId)
                .OrderByDescending(w => w.Czas)
                .FirstOrDefault();
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
            WeatherResponse weatherResponse = new WeatherResponse(bateria, temp, press, humi, time, lat, longi);
            // zwrocic jako json
            return Ok(weatherResponse);
        }

        [HttpGet("50/{nr_tel}/{kod}")]
        public IActionResult GetLast50Weather(string nr_tel, string kod)
        {
            // Check if the station exists
            if (!_context.Station.Any(s => s.NrTel == nr_tel && s.Kod == kod))
            {
                return NotFound();
            }

            var record = _context.Station.FirstOrDefault(s => s.NrTel == nr_tel && s.Kod == kod);
            if (record == null)
            {
                return NotFound("record was not found");
            }

            // Retrieve station details
            Guid stationId = record.StationId;
            double? bateria = record.Bateria;
            double lat = record.SzerokoscGeo;
            double longi = record.DlugoscGeo;

            // Retrieve the last 50 weather records for the station
            var lastWeatherRecords = _context.Weather
                .Where(w => w.StationId == stationId)
                .OrderByDescending(w => w.Czas)
                .Take(50)
                .ToList();

            if (!lastWeatherRecords.Any())
            {
                return NotFound();
            }

            // Map weather data
            var weatherResponses = lastWeatherRecords.Select(lastWeather => new WeatherResponse(
                bateria,
                lastWeather.Temperatura,
                lastWeather.Cisnienie,
                lastWeather.Wilgotnosc,
                lastWeather.Czas,
                lat,
                longi
            )).ToList();

            // Return as JSON
            return Ok(weatherResponses);
        }

        [HttpGet("station/{nr_tel}/{kod}")]
        public IActionResult GetStation(string nr_tel, string kod)
        {
            // sprawdzicz czy taka stacja istnieje

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
            StationResponse StationResponse = new StationResponse(bateria, lat, longi);
            // zwrocic jako json
            return Ok(StationResponse);
        }
    }
}
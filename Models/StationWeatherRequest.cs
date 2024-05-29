namespace WeatherApi.Models
{
    public class StationWeatherRequest
    {
        public string nr_tel { get; set; }
        public string kod { get; set; }
        public double? bateria { get; set; }
        public double temp { get; set; }
        public double press { get; set; }
        public double humi { get; set; }
        public double lat { get; set; }
        public double longi { get; set; }
    }
}

namespace WeatherApi.Models
{
    public class StationResponse
    {
        public double? battery { get; set; }
        public double lat { get; set; }
        public double longi { get; set; }


        public StationResponse(double? bateria, double szerokosc, double dlugosc)
        {
            battery = bateria;
            lat = szerokosc;
            longi = dlugosc;
        }
    }
}

namespace WeatherApi.Models
{
    public class WeatherResponse
    {
        public double? battery {  get; set; }
        public double temp {get; set;}
        public double press { get; set;}
        public double humi {  get; set;}
        public DateTime time { get; set;}
        public double lat {  get; set;}
        public double longi { get; set;}


        public WeatherResponse(double? bateria, double temperatura, double cisnienie, double wilgotnosc, DateTime czas, double szerokosc, double dlugosc)
        {
            battery = bateria;
            temp = temperatura;
            press = cisnienie;
            humi = wilgotnosc;
            time = czas;
            lat = szerokosc;
            longi = dlugosc;
        }
    }
}

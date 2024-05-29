using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApi.Models
{    public class Weather
    {
        [Key]
        public Guid WeatherId { get; private set; }
        public double Temperatura {  get; set; }
        public double Cisnienie { get; set; }
        public double Wilgotnosc { get; set; }
        public DateTime Czas {  get; set; }

        [ForeignKey(nameof(Station))]
        public Guid StationId {get; set;}
        public Station Station { get; set; }

        public Weather(Guid weatherId, double temperatura, double cisnienie, double wilgotnosc, DateTime czas, Guid stationId)
        {
            WeatherId = weatherId;
            Temperatura = temperatura;
            Cisnienie = cisnienie;
            Wilgotnosc = wilgotnosc;
            Czas = czas;
            StationId = stationId;
        }
    }
}

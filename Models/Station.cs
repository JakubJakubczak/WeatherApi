using System.ComponentModel.DataAnnotations;

namespace WeatherApi.Models
{
    public class Station
    {
        [Key]
        public Guid StationId { get; private set; }
        public string NrTel { get; set; }
        public string Kod { get; set; }
        public double? Bateria { get; set; }
        public double SzerokoscGeo { get; set; }
        public double DlugoscGeo { get; set; }

        public Station() {
            StationId = Guid.NewGuid(); 
        }
        public Station(Guid stationId, string nrTel, string kod, double? bateria, double szerokogscGeo, double dlugoscGeo)
        {
            StationId = stationId;
            NrTel = nrTel;
            Kod = kod;
            Bateria = bateria;
            SzerokoscGeo = szerokogscGeo;
            DlugoscGeo = dlugoscGeo;
        }

    }

}
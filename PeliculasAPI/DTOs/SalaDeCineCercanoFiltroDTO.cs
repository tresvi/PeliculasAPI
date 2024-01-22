using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class SalaDeCineCercanoFiltroDTO
    {
        [Range(-90, 90)]
        public double Latitud { get; set; }
        [Range(-180, 180)]
        public double Longitud { get; set; }
        private const int DISTANCIA_MAX_KMS = 50;

        private int _distanciaEnMKms = 10;
        public int DistanciaEnKms
        {
            get
            {
                return _distanciaEnMKms;
            }
            set
            {
                _distanciaEnMKms = (value > DISTANCIA_MAX_KMS) ? DISTANCIA_MAX_KMS : value;
            }
        }
    }
}

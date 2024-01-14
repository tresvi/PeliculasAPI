using PeliculasAPI.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class PeliculaCreacionDTO: PeliculaPatchDTO
    {
        [PesoArchivoValidacion(PesoMaximoEnMegabytes: 4)]
        [TipoArchivoValidacion(GrupoTipoArchivo.Imagen)]
        public IFormFile Poster { get; set; }

    }
}

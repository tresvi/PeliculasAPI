using PeliculasAPI.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class ActorCreacionDTO: ActorPatchDTO
    {
        //En el DTO quiero recibir la foto como tal por eso uso IFormFile. En los demas modelos de Actor
        //solo me interesa poner el string de la url de la foto
        [PesoArchivoValidacion(PesoMaximoEnMegabytes: 2)]
        [TipoArchivoValidacion(grupoTipoArchivo: GrupoTipoArchivo.Imagen)]
        public IFormFile Foto { get; set; }
    }
}

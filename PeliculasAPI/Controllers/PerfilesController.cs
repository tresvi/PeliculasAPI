using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    //[Route("api/perfiles")]
    //[Route("api/Actores/{idActor}/Perfiles/{idPerfil}")]
    [Route("api/Actores/{idActor}/Perfiles")]
    public class PerfilesController : ControllerBase
    {

        [HttpGet(Name = "Listar todos los perfiles")]
        public async Task<ActionResult<List<string>>> Get()
        {
            List<string> listaPerfiles = new List<string>();
            listaPerfiles.Add("PErfil 1");
            listaPerfiles.Add("PErfil 2");
            listaPerfiles.Add("PErfil 2");
            return listaPerfiles;
        }

        //[HttpGet]
        [HttpGet("{idPerfil}", Name = "En peril")]
        public async Task<ActionResult<string>> Get(string idActor, string idPerfil)
        {
            return @$"Actor: {idActor}, Perfil {idPerfil}";
        }
        /*
                [HttpGet]
                public async Task<ActionResult<string>> Get(string idActor, string idPerfil, string sobreNombre)
                {
                    return @$"Actor: {idActor}, Perfil: {idPerfil}, Sobrenombre: {sobreNombre}";
                }
        */



    }
}

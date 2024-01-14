using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using PeliculasAPI.Servicios;
using Microsoft.AspNetCore.JsonPatch;
using PeliculasAPI.Helpers;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        private object actorDb;
        private const string CONTENEDOR_FOTOS_NAME = "actores";

        public ActoresController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            _context = context;
            _mapper = mapper;
            _almacenadorArchivos = almacenadorArchivos;
        }


        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            IQueryable<Actor> queryable = _context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);

            var entidades = await queryable.Paginar(paginacionDTO).ToListAsync();
            List<ActorDTO> dtos = _mapper.Map<List<ActorDTO>>(entidades);

            return Ok(dtos);
        }


        [HttpGet("{id:int}", Name = "obtenerActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var entidad = await _context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null) return NotFound();

            var dto = _mapper.Map<ActorDTO>(entidad);

            return Ok(dto);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var entidad = _mapper.Map<Actor>(actorCreacionDTO);

            if (actorCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                    entidad.Foto = await _almacenadorArchivos.GuardarArchivo(contenido, extension, CONTENEDOR_FOTOS_NAME
                        , actorCreacionDTO.Foto.ContentType);
                }
            }

            _context.Add(entidad);
            await _context.SaveChangesAsync();
            var actorDTO = _mapper.Map<ActorDTO>(entidad);
            return new CreatedAtRouteResult("obtenerActor", new { id = actorDTO.Id }, actorDTO);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actorDB = await _context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (actorDB == null) { return NotFound(); }

            actorDB = _mapper.Map(actorCreacionDTO, actorDB);

            if (actorCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                    actorDB.Foto = await _almacenadorArchivos.EditarArchivo(contenido, extension, CONTENEDOR_FOTOS_NAME
                        , actorDB.Foto
                        , actorCreacionDTO.Foto.ContentType);
                }
            }

            //Solamente los campos que difieran seran actualizados por EF
            await _context.SaveChangesAsync();
            return NoContent();
        }
        

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument)
        {
            if (patchDocument == null) return BadRequest();

            var entidadDB = await _context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (entidadDB == null) return NotFound();

            var entidadDTO = _mapper.Map<ActorPatchDTO>(entidadDB);

            patchDocument.ApplyTo(entidadDTO, ModelState);

            var esValido = TryValidateModel(entidadDTO);

            if (!esValido) return BadRequest(ModelState);

            _mapper.Map(entidadDTO, entidadDB);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _context.Actores.AnyAsync(x => x.Id == id);
            if (!existe) return NotFound();

            _context.Remove(new Actor() { Id = id });
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}

using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using PeliculasAPI.Servicios;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController : ControllerBase
    {
        private const string CONTENEDOR_FOTOS_NAME = "peliculas";

        private readonly ApplicationDbContext _context;
        public IMapper _mapper;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;


        public PeliculasController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            _context = context;
            _mapper = mapper;
            _almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<List<PeliculaDTO>>> Get()
        {
            var peliculas = await _context.Peliculas.ToListAsync();
            return _mapper.Map<List<PeliculaDTO>>(peliculas);
        }

        [HttpGet("{id}", Name = "obtenerPelicula")]
        public async Task<ActionResult<PeliculaDTO>> Get(int id)
        {
            var pelicula = await _context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

            if (pelicula == null) return NotFound();

            return _mapper.Map<PeliculaDTO>(pelicula);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = _mapper.Map<Pelicula>(peliculaCreacionDTO);

            if (peliculaCreacionDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    pelicula.Poster = await _almacenadorArchivos.GuardarArchivo(contenido, extension, CONTENEDOR_FOTOS_NAME
                        , peliculaCreacionDTO.Poster.ContentType);
                }
            }

            _context.Add(pelicula);
            await _context.SaveChangesAsync();
            var peliculaDTO = _mapper.Map<PeliculaDTO>(pelicula);

            return new CreatedAtRouteResult("obtenerPelicula", new { id = pelicula.Id }, peliculaDTO);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var peliculaDB = await _context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

            if (peliculaDB == null) { return NotFound(); }

            peliculaDB = _mapper.Map(peliculaCreacionDTO, peliculaDB);

            if (peliculaCreacionDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    peliculaDB.Poster = await _almacenadorArchivos.EditarArchivo(contenido, extension, CONTENEDOR_FOTOS_NAME
                        , peliculaDB.Poster
                        , peliculaCreacionDTO.Poster.ContentType);
                }
            }

            //Solamente los campos que difieran seran actualizados por EF
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<PeliculaPatchDTO> patchDocument)
        {
            if (patchDocument == null) return BadRequest();

            var entidadDB = await _context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

            if (entidadDB == null) return NotFound();

            var entidadDTO = _mapper.Map<PeliculaPatchDTO>(entidadDB);

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
            var existe = await _context.Peliculas.AnyAsync(x => x.Id == id);
            if (!existe) return NotFound();

            _context.Remove(new Pelicula() { Id = id });
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

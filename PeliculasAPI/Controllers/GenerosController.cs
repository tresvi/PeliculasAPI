using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GenerosController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GenerosController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get()
        {
            var entidades = await _context.Generos.ToListAsync();
            List<GeneroDTO> dtos = _mapper.Map<List<GeneroDTO>>(entidades);
            return Ok(dtos);
        }


        [HttpGet("{id:int}", Name = "obtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            var entidad = await _context.Generos.FirstOrDefaultAsync(x => x.Id == id);
            
            if (entidad == null) return NotFound();

            var dto = _mapper.Map<GeneroDTO>(entidad);

            return Ok(dto);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            Genero entidad = _mapper.Map<Genero>(generoCreacionDTO);
            _context.Add(entidad);
            await _context.SaveChangesAsync();
            GeneroDTO generoDTO = _mapper.Map<GeneroDTO>(entidad);

            return new CreatedAtRouteResult("obtenerGenero", new { id = generoDTO.Id}, generoDTO);
        }


        [HttpPut("{id}")]
        //Normalmente los DTO de creacion y actualizacion son diferentes. En este caso se deja asi por simpleza
        //El caso contrario es por ej. las paginas que piden determinados datos para crear el usuario, pero luego 
        //piden actualizar el perfil con muchos datos mas.
        public async Task<ActionResult> Put(int Id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var entidad = _mapper.Map<Genero>(generoCreacionDTO);
            entidad.Id = Id;
            _context.Entry(entidad).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _context.Generos.AnyAsync(x => x.Id == id);
            if (!existe) return NotFound();

            _context.Remove(new Genero() { Id = id });
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}

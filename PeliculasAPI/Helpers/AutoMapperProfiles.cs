using AutoMapper;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
            
            CreateMap<Actor, ActorDTO>();
            //Desestimo la conversion del campo foto, ya que en un modelo es
            //de tipo IFormFile, y en el otro, es de tipo String
            CreateMap<ActorCreacionDTO, Actor>()
                .ForMember(x => x.Foto, options => options.Ignore());
            CreateMap<ActorPatchDTO, Actor>().ReverseMap();

            CreateMap<Pelicula, PeliculaDTO>();
            //Desestimo la conversion del campo foto, ya que en un modelo es
            //de tipo IFormFile, y en el otro, es de tipo String
            CreateMap<PeliculaCreacionDTO, Pelicula>()
                .ForMember(x => x.Poster, options => options.Ignore());
            CreateMap<PeliculaPatchDTO, Pelicula>().ReverseMap();
        }

    }
}

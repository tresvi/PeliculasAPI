using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using System.Runtime.CompilerServices;

namespace PeliculasAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory) 
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();

            CreateMap<IdentityUser, UsuarioDTO>();

            CreateMap<SalaDeCine, SalaDeCineDTO>()
                .ForMember(x => x.Latitud, x => x.MapFrom(y => y.Ubicacion.Y))
                .ForMember(x => x.Longitud, x => x.MapFrom(y => y.Ubicacion.X));

            CreateMap<SalaDeCineDTO, SalaDeCine>()
                .ForMember(x => x.Ubicacion, x => x.MapFrom(y =>
                geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));
            CreateMap<SalaDeCineCreacionDTO, SalaDeCine>()
                .ForMember(x => x.Ubicacion, x => x.MapFrom(y =>
                geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));

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
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.PeliculasGeneros, options => options.MapFrom(MapPeliculasGeneros))
                .ForMember(x => x.PeliculasActores, options => options.MapFrom(MapPeliculasActores));

            CreateMap<Pelicula, PeliculaDetallesDTO>()
                .ForMember(x => x.Generos, options => options.MapFrom(MapPeliculasGeneros))
                .ForMember(x => x.Actores, options => options.MapFrom(MapPeliculasActores));

            CreateMap<PeliculaPatchDTO, Pelicula>().ReverseMap();
        }


        private List<ActorPeliculaDetalleDTO> MapPeliculasActores(Pelicula pelicula, PeliculaDetallesDTO peliculaDetalle)
        {
            var resultado = new List<ActorPeliculaDetalleDTO>();
            if (pelicula.PeliculasActores == null) return resultado;

            foreach (var actorPelicula in pelicula.PeliculasActores)
            {
                resultado.Add(new ActorPeliculaDetalleDTO
                {
                    ActorId = actorPelicula.ActorId,
                    Personaje = actorPelicula.Personaje,
                    NombrePersona = actorPelicula.Actor.Nombre
                });
            }

            return resultado;
        }

        private List<GeneroDTO> MapPeliculasGeneros(Pelicula pelicula, PeliculaDetallesDTO peliculasDetallesDTO )
        {
            var resultado = new List<GeneroDTO>();
            if (pelicula.PeliculasGeneros == null) return resultado;

            foreach (var generoPelicula in pelicula.PeliculasGeneros)
            {
                resultado.Add(new GeneroDTO() { Id = generoPelicula.GeneroId, Nombre = generoPelicula.Genero.Nombre });
            }

            return resultado;
        }

        //Los parametros que recibe son fijos esos. Enviadospor MapFrom y tomados de los types que mapea
        private List<PeliculasGeneros> MapPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        { 
            List<PeliculasGeneros> resultado = new List<PeliculasGeneros>();
            if (peliculaCreacionDTO.GenerosIDs == null) { return resultado; }

            foreach (int id in peliculaCreacionDTO.GenerosIDs)
            {
                resultado.Add(new PeliculasGeneros() { GeneroId = id });
            }

            return resultado;
        }


        private List<PeliculasActores> MapPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            List<PeliculasActores> resultado = new List<PeliculasActores>();
            if (peliculaCreacionDTO.Actores == null) { return resultado; }

            foreach (var actor in peliculaCreacionDTO.Actores)
            {
                resultado.Add(new PeliculasActores() { ActorId = actor.ActorId, Personaje = actor.Personaje });
            }

            return resultado;
        }


    }
}

using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace PeliculasAPI.Entidades
{
    public class SalaDeCine: IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public Point Ubicacion { get; set; }
        public List<PeliculasSalasDeCine> PeliculasSalasDeCines { get; set; }
    }

    /*
    Declare @MiUbicacion GEOGRAPHY = 'POINT(-69.938988 18.481208)'

    Select TOP (1000) [Id]
	    ,[Nombre]
	    ,[Ubicacion].ToString() as Ubicacion
	    ,Ubicacion.STDistance(@MiUbicacion) as Distancia
    From [PeliculasAPI].[dbo].[SalasDeCine]
    Where Ubicacion.STDistance(@MiUbicacion) <= 3000
    Order by Ubicacion.STDistance(@MiUbicacion)
    */
}

namespace PeliculasAPI.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;

        private int _cantidadRegistrosPorPagina = 10;
        private readonly int CANTIDAD_MAX_REGISTROS_X_PAGINA = 50;
        
        public int CantidadRegistrosPorPagina
        { 
            get => _cantidadRegistrosPorPagina;
            set
            { 
                _cantidadRegistrosPorPagina = (value > CANTIDAD_MAX_REGISTROS_X_PAGINA) ? CANTIDAD_MAX_REGISTROS_X_PAGINA : value;
            }
        }


    }
}

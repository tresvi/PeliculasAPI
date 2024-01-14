namespace PeliculasAPI.Servicios
{
    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos
    {
        public readonly IWebHostEnvironment _environment;           //Para obtener donde se encuentra el wwwroot
        public readonly IHttpContextAccessor _httpContextAccessor;   //Para obtener el dominio donde esta publicado esta api, y asi consturir la URL desde la cual se va aacceder a las imagenes

        public AlmacenadorArchivosLocal(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task BorrarArchivo(string ruta, string contenedor)
        {
            if (ruta != null)
            { 
                var nombreArchivo = Path.GetFileName(ruta);
                string path = Path.Combine(_environment.WebRootPath, contenedor, nombreArchivo);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            return Task.FromResult(0);
        }


        public async Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, string ruta, string contentType)
        {
            await BorrarArchivo(ruta, contenedor);
            return await GuardarArchivo(contenido, extension, contenedor, contentType);
        }


        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string contenedor, string contentType)
        {
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(_environment.WebRootPath, contenedor);

            if (!Directory.Exists(folder))
            { 
                Directory.CreateDirectory(folder);
            }

            string ruta = Path.Combine(folder, nombreArchivo);
            await File.WriteAllBytesAsync(ruta, contenido);

            //http o https + la URL
            var urlActual = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var urlParaDB = Path.Combine(urlActual, contenedor, nombreArchivo).Replace("\\", "/");
            return urlParaDB;
        }
    }
}

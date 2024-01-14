using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace PeliculasAPI.Validaciones
{
    public class TipoArchivoValidacion: ValidationAttribute
    {
        private readonly string[] _tiposValidos;

        public TipoArchivoValidacion(string[] tiposValidos)
        {
            _tiposValidos = tiposValidos;
        }

        public TipoArchivoValidacion(GrupoTipoArchivo grupoTipoArchivo)
        {
            if (grupoTipoArchivo == GrupoTipoArchivo.Imagen)
            {
                _tiposValidos = new string[] { "image/jpeg", "image/gif" };
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (!_tiposValidos.Contains(formFile.ContentType))
            {
                return new ValidationResult($"El tipo de archivo debe ser alguno de los siguientes: {string.Join(", ", _tiposValidos)}");
            }

            return ValidationResult.Success;
        }


    }
}

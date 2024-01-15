using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace PeliculasAPI.Helpers
{
    public class TypeBinder<T>: IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            string nombrePropiedad = bindingContext.ModelName;
            var proveedoresDeValores = bindingContext.ValueProvider.GetValue(nombrePropiedad);

            if (proveedoresDeValores == ValueProviderResult.None) return Task.CompletedTask;

            try
            {
                T valorDeserializado = JsonConvert.DeserializeObject<T>(proveedoresDeValores.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(valorDeserializado);
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(nombrePropiedad, "Valor invalido para tipo List<int>");
            }

            return Task.CompletedTask;
        }
    }
}

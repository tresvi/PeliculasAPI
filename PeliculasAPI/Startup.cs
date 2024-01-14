using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Servicios;

//Performance rules https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1828

namespace PeliculasAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosAzure>();
            services.AddHttpContextAccessor();

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers()
                .AddNewtonsoftJson();
           
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {Title = "WebAPIPeliculas", Version = "v1" });
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            { 
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIPeliculas v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();   //Para que sirva contenido como fotos.
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}

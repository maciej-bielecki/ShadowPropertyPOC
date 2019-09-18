using DaceloRex.DataAccess;
using DaceloRex.WebApplication.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<ITenantContextService, TenantContextService>();
            services.AddDbContext<DaceloRexContext>(options => options.UseSqlServer(@"Server=.\SQLEXPRESS;Database=DaceloRex;Trusted_Connection=True;"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "DaceloRex", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger().UseSwaggerUI(c => c.SwaggerEndpoint("./v1/swagger.json", "DaceloRex Api v1"));
            app.UseMvc();
        }
    }
}

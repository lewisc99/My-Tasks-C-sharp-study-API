using _3___MinhasTarefasAPI.Database;
using _3___MinhasTarefasAPI.V1.Models;
using _3___MinhasTarefasAPI.V1.Repositories;
using _3___MinhasTarefasAPI.V1.Repositories.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _3___MinhasTarefasAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddDbContext<MinhasTarefasContext>(
                c =>
                c.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));



            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<MinhasTarefasContext>()
                    .AddDefaultTokenProviders();


            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ITarefaRepository, TarefaRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();



            // like json igonre of java, to not repeat code and refactor code.
            services.AddMvc(
                
                config =>
                {
                    config.ReturnHttpNotAcceptable = true; //return 406
                    config.InputFormatters.Add(new XmlSerializerInputFormatter(config)); // to activate the user to choose which format want
                    config.OutputFormatters.Add(new XmlSerializerOutputFormatter()); //to activate the response - result another format beyond JSON

                }
                
                
                ).AddNewtonsoftJson(
                opt => opt.SerializerSettings.ReferenceLoopHandling =
              Newtonsoft.Json.ReferenceLoopHandling.Ignore);



            services.AddAuthentication(options =>
            {

                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(
                //indicate which elements of token must be validated
                options => options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {

                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("chave-api-jwt-minhas-tarefas"))
        });


            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                                         .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                                         .RequireAuthenticatedUser().Build());
            });

            //to change the response when the user it's not authorized.
            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });



            services.AddApiVersioning(
                cfg =>
                {
                    cfg.ReportApiVersions = true;

                    cfg.ApiVersionReader = new HeaderApiVersionReader("api-version");

                    cfg.AssumeDefaultVersionWhenUnspecified = true;

                    cfg.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                });


            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2.0", new OpenApiInfo
                {
                    Version = "v2.0",
                    Title = "Minhas Tarefas - API V2"
                });

                c.SwaggerDoc("v1.1", new OpenApiInfo
                {
                    Version = "v1.1",
                    Title = "Minhas Tarefas - API v1.1"
                });

                c.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "Minhas Tarefas - API v1.0"
                });


                var caminhoProjeto = PlatformServices.Default.Application.ApplicationBasePath;
                var NomeProjeto = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var caminhoArquivoXMLComentario = Path.Combine(caminhoProjeto, NomeProjeto);


                c.IncludeXmlComments(caminhoArquivoXMLComentario);


                c.DocInclusionPredicate((version, desc) =>
                {
                    if (!desc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
                    var versions = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<ApiVersionAttribute>().SelectMany(attr => attr.Versions);
                    var maps = methodInfo.GetCustomAttributes(true).OfType<MapToApiVersionAttribute>().SelectMany(attr => attr.Versions).ToArray();
                    version = version.Replace("v", "");
                    return versions.Any(v => v.ToString() == version && maps.Any(v => v.ToString() == version));
                });




                c.ResolveConflictingActions(apiDescription => apiDescription.First());
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            app.UseStatusCodePages();
            app.UseDeveloperExceptionPage();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.UseSwagger();

            app.UseSwaggerUI(cfg =>
            {
                cfg.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Minhas Tarefas v1.0");
                cfg.SwaggerEndpoint("/swagger/v1.1/swagger.json", "Minhas Tarefas v1.1");
                cfg.SwaggerEndpoint("/swagger/v2.0/swagger.json", "Minhas Tarefas v2.0");


                cfg.RoutePrefix = string.Empty;

            });
        }
    }
}

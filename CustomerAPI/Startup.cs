using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CustomerRepository.Models;
using Microsoft.AspNetCore.Http;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CustomerRepository.Interfaces;
using CustomerRepository;
using AutoMapper;
using CustomerAPI.MappingProfiles;
namespace CustomerAPI
{
    public class Startup
    {
        public IWebHostEnvironment CurrentEnvironment { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc(options => options.EnableEndpointRouting = false)
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
            string dncDatasourceUsername = Configuration["dnc.datasource.username"];
            string dncDatasourcepassword = Configuration["dnc.datasource.password"];
            string dncDatasourceServer = Configuration["dnc.datasource.server"];
            string dncDatasourceDatabase = Configuration["dnc.datasource.database"];
            string dncDatasourcePort = Configuration["dnc.datasource.port"];
            string dncDatasourceAdminusername = Configuration["dnc.datasource.adminusername"];
            string dncDatasourceAdminPassword = Configuration["dnc.datasource.adminpassword"];
            string postGresConnectionString = $"Server={dncDatasourceServer};Port={dncDatasourcePort};Userid={dncDatasourceUsername};Password={dncDatasourcepassword};Database={dncDatasourceDatabase};Integrated Security=true;Pooling=true";
            string postGresconnectionStringForAdmin = $"Server={dncDatasourceServer};Port={dncDatasourcePort};Userid={dncDatasourceAdminusername};Password={dncDatasourceAdminPassword};Database={dncDatasourceDatabase};Integrated Security=true;Pooling=true";

            services.AddDbContext<AdminDbContext>(options => options.UseNpgsql(postGresconnectionStringForAdmin));
            services.AddDbContext<UserDbContext>(options => options.UseNpgsql(postGresConnectionString));
            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSingleton(this.Configuration);
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HTTP API",
                    Version = "v1",
                    Description = "The Service HTTP API"
                });
            });

            var mapperConfiguration = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CustomerMappingProfile());
                mc.AddProfile<MappingProfile>();
            });
            services.AddSingleton(mapperConfiguration.CreateMapper());
            services.AddTransient<ICustomerRepository, CustomerRepo>();
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddOptions();
            services.AddInfrastructure();
            var container = new ContainerBuilder();
            container.Populate(services);
            //container.RegisterType<CustomerRepository>().As<ICustomerRepository>().InstancePerRequest();
            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseMetricServer();

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{ string.Empty }/swagger/v1/swagger.json", "HTTP API V1");
                });

            app.UseRouting();
            app.UseCors("CorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

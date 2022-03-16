using Microsoft.OpenApi.Models;
using FlightPlanner.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using FlightPlanner.Data;
using FlightPlanner.Core.Services;
using FlightPlanner.Services;
using FlightPlanner.Models;
using FlightPlanner.Services.Validators;
using FlightPlanner.Services.Mappers;
using AutoMapper;

namespace FlightPlanner
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FlightPlanner", Version = "v1" });
            });

            // services.AddDbContext<FlightPlannerDbContext>(ServiceLifetime.Scoped);

            services.AddDbContext<FlightPlannerDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("flight-planner"));
            });

            services.AddTransient<IFlightPlannerDbContext, FlightPlannerDbContext>();
            services.AddTransient<IDbService, DbService>();
            services.AddTransient<IDbExtendedService, DbExtendedService>();
            services.AddTransient<IEntityService<Flight>, EntityService<Flight>>();
            services.AddTransient<IEntityService<Airport>, EntityService<Airport>>();
            services.AddTransient<IFlightService ,FlightService>();
            services.AddTransient<IValidator ,AddFlightRequestValidator>();
            services.AddTransient<IValidator ,AirportNameEqualityValidator>();
            services.AddTransient<IValidator ,ArrivalTimeValidator>();
            services.AddTransient<IValidator ,CarrierValidator>();
            services.AddTransient<IValidator ,DepartureTimeValidator>();
            services.AddTransient<IValidator ,FromAirportCityValidator>();
            services.AddTransient<IValidator ,FromAirportCountryValidator>();
            services.AddTransient<IValidator ,FromAirportNameValidator>();
            services.AddTransient<IValidator ,FromAirportValidator>();
            services.AddTransient<IValidator ,ToAirportCityValidator>();
            services.AddTransient<IValidator ,ToAirportCountryValidator>();
            services.AddTransient<IValidator ,ToAirportNameValidator>();
            services.AddTransient<IValidator ,ToAirportValidator>();
            services.AddTransient<IValidator ,TimeFrameValidator>();
            services.AddTransient<ISearchValidator ,SearchFlightValidator>();
            var mapper = AutoMapperConfig.CreateMapper();
            services.AddSingleton<IMapper>(mapper);





            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                   .AllowCredentials()
                    .AllowAnyMethod();

            }));
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlightPlanner v1"));
            }

            app.UseRouting();
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                                       .AllowAnyHeader()
                                          .AllowCredentials()
                                           .AllowAnyMethod();
            });
            app.UseAuthentication();
            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

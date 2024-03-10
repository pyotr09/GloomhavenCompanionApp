using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Gloom.Services;
using Gloom.Services.Implementation;
using Microsoft.OpenApi.Models;

namespace Gloom.WebApi;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("GloomApi", new OpenApiInfo
            {
                Version = "v1.0",
                Title = "Gloom App API",
                Description = "API used by the gloom app dashboard"
            });
        });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
                builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .AllowAnyHeader());
        });

        services.AddHttpContextAccessor();

        services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
        services.AddAWSService<IAmazonDynamoDB>();
        services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
        services.AddScoped<IScenarioService, ScenarioService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors();
        app.UseRouting();
        app.UseSwagger(c =>
        {
            c.SerializeAsV2 = true;
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
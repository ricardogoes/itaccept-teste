using ItAccept.Teste.Application.Middleware;
using ItAccept.Teste.Domain.Interfaces.AppSettings;
using ItAccept.Teste.Domain.Models.AppSettings;
using ItAccept.Teste.Infrastructure.Crosscutting.Automapper;
using ItAccept.Teste.Infrastructure.Crosscutting.IoC;
using ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Models;
using ItAccept.Teste.Infrastructure.Data.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO.Compression;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DictionaryKeyPolicy = null;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

var appSettings = builder.Configuration.Get<AppSettings>();
builder.Services.AddSingleton(appSettings);

var connectionConfig = builder.Configuration.GetSection(nameof(ConnectionSettings)).Get<ConnectionSettings>();
builder.Services.AddSingleton<IConnectionSettings>(connectionConfig);
builder.Services.AddSingleton<IDapperWrapper, DapperWrapper>();

//IOC
ServiceBase.GetInstance<ServiceRepositories>().Add(builder.Services);
ServiceBase.GetInstance<ServiceServices>().Add(builder.Services);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
MappingConfiguration.Configure(builder.Services);

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
        .RequireAuthenticatedUser().Build());
});

builder.Services.AddAuthentication().AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = TokenAuthOptions.Key,
        ValidAudience = TokenAuthOptions.Audience,
        ValidIssuer = TokenAuthOptions.Issuer,

        // When receiving a token, check that we've signed it.
        ValidateIssuerSigningKey = true,

        // When receiving a token, check that it is still valid.
        ValidateLifetime = true,

        // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time
        // when validating the lifetime. As we're creating the tokens locally and validating them on the same
        // machines which should have synchronised time, this can be set to zero. Where external tokens are
        // used, some leeway here could be useful.
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(origin => true)
                .WithExposedHeaders("Location");
        });
});

builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.AddSwaggerGen(options =>
{

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = $"{appSettings.SwaggerSettings.Version}",
        Title = $"{appSettings.SwaggerSettings.Title}",
        Description = $"{appSettings.SwaggerSettings.Description}",
        Contact = new OpenApiContact
        {
            Name = $"{appSettings.SwaggerSettings.ContactName}",
            Email = $"{appSettings.SwaggerSettings.ContactEmail}"
        }

    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Entre com JWT Token Bearer",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
});

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/profiler";
});

var app = builder.Build();

app.UseMiddleware(typeof(ErrorHandlingMiddleware));
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.UseAuthentication();
app.UseMiniProfiler();
app.UseResponseCompression();
app.UseApiVersioning();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "IT Accept Teste API");
});

app.UseSwagger(options =>
{
    options.SerializeAsV2 = false;
});

app.MapControllers();

builder.WebHost.UseUrls($"http://*:5001");

app.Run();

public partial class Program { }
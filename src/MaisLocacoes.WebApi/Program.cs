using AutoMapper;
using Configuration;
using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.Exceptions.Middleware;
using MaisLocacoes.WebApi.IoC;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Postgres:
var connectionString = builder.Configuration["MyPostgreSqlConnection:MyPostgreSqlConnectionString"];
builder.Services.AddDbContext<PostgreSqlContext>(options => options.UseNpgsql(connectionString), ServiceLifetime.Scoped);

//AutoMapper:
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Injeção de dependência:
Bootstrapper.Configure(builder.Services);

//Add Cors - permite requisições de servidor web para servidor web
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

//Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    //Deserialização das responses HTTP com conversão de datas em utc
    //options.JsonSerializerOptions.Converters.Add(new DateTimeConverterUsingUtc());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

//Add Authentication
var key = Encoding.ASCII.GetBytes(JwtManager.secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

//Authorization
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

//Configurando Logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders(); // Limpar provedores de log existentes
    logging.AddConsole(options =>
    {
        options.FormatterName = ConsoleFormatterNames.Systemd; // Opcional: usar formato de log do Systemd
    });
    logging.AddFilter<ConsoleLoggerProvider>("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning); // Filtrar logs específicos
});

//Adicionando swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MaisLocacoes.WebApi", Version = "v1" });
    c.EnableAnnotations();
    // Configuração de autenticação Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
{
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
});
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MaisLocacoes.WebApi v1");
        c.OAuthClientId("Swagger");
        c.OAuthAppName("Swagger UI");
    });
    app.UseSwagger();
}

app.UseExceptionHandler("/error");

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
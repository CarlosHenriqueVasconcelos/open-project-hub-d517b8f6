using PlataformaGestaoIA.DataContext;
using PlataformaGestaoIA.DataContext.Mappings;
using MySql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using PlataformaGestaoIA;
using PlataformaGestaoIA.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SecureIdentity.Password;
using Microsoft.AspNetCore.HttpLogging;


var builder = WebApplication.CreateBuilder(args);

ConfigureMvc(builder);
ConfigureAuthentication(builder);
ConfigureServices(builder);

builder.Logging.AddConsole();
builder.Services.AddHttpLogging(logging => {
    logging.LoggingFields = HttpLoggingFields.All;
});

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Aplicação iniciada com sucesso. Versão: {version}", "1.4.1");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PrincipalDataContext>();
    var runMigrations = Environment.GetEnvironmentVariable("RUN_MIGRATIONS");
    if (string.Equals(runMigrations, "true", StringComparison.OrdinalIgnoreCase))
    {
        context.Database.Migrate();
    }
}

app.UseHttpLogging();
app.UseRouting();
app.UseCors("AllowAllOrigins");

LoadConfiguration(app);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseResponseCaching();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

builder.Logging.AddConsole();

app.MapControllers();

app.Run();

void ConfigureMvc(WebApplicationBuilder builder)
{
    builder
        .Services
        .AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;

        });
}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(
        options => options.TokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.Key)),
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateIssuer = false
        });
}

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(option => {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                new string[] { }
            }
        });
    });

        builder.Services.AddDbContext<PrincipalDataContext>(options =>
        {
            var connectionString = builder.Configuration["ConnectionStrings:Default"];

            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("Connection string 'Default' is missing");

            options.UseMySQL(connectionString);
        });
    builder.Services.AddTransient<TokenService>();
    //builder.Services.AddTransient<EmailService>();
    builder.Services.AddMemoryCache();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
    });
}

void LoadConfiguration(WebApplication app)
{
    Configuration.ApiKeyValue = app.Configuration.GetValue<string>("ApiKeyValue");
    Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName");
    Configuration.Key = app.Configuration.GetValue<string>("Key");

    var smtp = new Configuration.SmtpConfiguration();
    app.Configuration.GetSection("Smtp").Bind(smtp);
    Configuration.Smtp = smtp;

    app.UseSwagger(c =>
    {
        c.RouteTemplate = "api/developers/swagger/{documentname}/swagger.json"; // Define o caminho do arquivo JSON
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/api/developers/swagger/v1/swagger.json", "Demo API V1");
        c.RoutePrefix = "api/developers";
    });
    
}

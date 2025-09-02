using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyAPI.Interfaces;
using MyAPI.Models;
using MyAPI.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();


builder.Services.AddDbContext<WorkshopAPI>(options =>
    options.UseSqlServer
    (builder.Configuration.GetConnectionString
    ("WorkshopAPI")));

builder.Services.AddScoped<ICourseLibraryRepository, CourseLibraryRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Course Library API",
        Version = "v1",
        Description = "A simple example ASP.NET Core Web API",
        Contact = new()
        {
            Name = "Pattarapon ",
            Email = "Pattarapon.thongsri@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/pattarapon-thongsri-927b62136/"),
        },
    });

    options.SwaggerDoc("v2", new()
    {
        Title = "Course Library API",
        Version = "v2",
        Description = "A simple example ASP.NET Core Web API",
        Contact = new()
        {
            Name = "Pattarapon Thongsri ",
            Email = "Pattarapon.thongsri@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/pattarapon-thongsri-927b62136/"),
        },
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearrer scheme.\r\n\r\n Enter 'Bearer' [space] and your token in the text input below. \r\n\r\n Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                },
               new List<string>()
            }
    });

    var xmlCommentFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var cmlCommentFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
    options.IncludeXmlComments(cmlCommentFullPath);

});

    builder.Services.AddApiVersioning(options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
        options.ApiVersionReader = new HeaderApiVersionReader("api-version");
    }).AddMvc();

    var appSettingsSection = builder.Configuration.GetSection("AppSettings");
    builder.Services.Configure<AppSettings>(appSettingsSection);
    var appSettings = appSettingsSection.Get<AppSettings>(); 
    var key = Encoding.ASCII.GetBytes(appSettings.Secret);

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

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Course Library API v1");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "Course Library API v2");
    options.RoutePrefix = string.Empty;
});


app.UseCors(
    x => x.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
    );

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

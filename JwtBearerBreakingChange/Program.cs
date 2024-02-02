using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using JwtBearerBreakingChange.Jwt;

namespace JwtBearerBreakingChange;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        // My awesome custom implementation
        builder.Services.AddAuthentication()
            .AddScheme<JwtBearerOptions, CustomJwtBearerHandler>(CustomJwtBearerDefaults.SCHEME,
            o => o.TokenValidationParameters = CustomJwtBearerDefaults.TokenValidationParameters);
        builder.Services.AddSingleton<CustomJwtTokenHandler>();
        builder.Services.AddSingleton<
            IPostConfigureOptions<JwtBearerOptions>,
            CustomJwtBearerPostConfigureOptions>();
        builder.Services.AddSingleton<CustomTokenIssuer>();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "My API",
                Version = "v1"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
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

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

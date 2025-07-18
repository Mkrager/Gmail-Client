﻿using Microsoft.OpenApi.Models;
using GmailClient.Persistence;
using GmailClient.Application;
using GmailClient.Infrastructure;
using GmailClient.Identity;
using GmailClient.Api.Services;
using GmailClient.Application.Contracts;
using GmailClient.Api.Middlewares;


namespace GmailClient.Api
{
    public static class StartupExtensions
    {
        public static WebApplication ConfigureService(
            this WebApplicationBuilder builder)
        {
            AddSwagger(builder.Services);

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddMemoryCache();

            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Open", policy =>
                {
                    policy.WithOrigins("https://localhost:7167")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseCustomExceptionHandler();

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GmailClient API");
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("Open");


            app.MapControllers();

            return app;
        }


        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
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
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });


                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "GmailClient Management API",
                });
            });
        }
    }
}

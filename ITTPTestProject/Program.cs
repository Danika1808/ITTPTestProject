using Application.Extensions;
using Application.Services.JWTServices;
using Application.Services.UserServices;
using Domain;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Repository;
using ITTPTestProject.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMediators();
builder.Services.AddValidators();
builder.Services.AddMapper();
builder.Services.AddMvc().AddFluentValidation();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
builder.Services.AddScoped<IPasswordValidator<ApplicationUser>, PasswordValidator<ApplicationUser>>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

builder.Services.AddControllers(options => options.InputFormatters.Insert(0, GetJsonPatchInputFormatter()))
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbConnection"));
}, ServiceLifetime.Scoped);
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 7;
        options.Password.RequireUppercase = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = Constants.Issuer,
    ValidateAudience = true,
    ValidAudience = Constants.Audience,
    ValidateLifetime = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
    ValidateIssuerSigningKey = true,
};
builder.Services.AddSingleton(tokenValidationParameters);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = tokenValidationParameters;
});

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .Build();
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
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


static IInputFormatter GetJsonPatchInputFormatter()
{
    var builder = new ServiceCollection()
                    .AddLogging()
                    .AddMvc()
                    .AddNewtonsoftJson()
                    .Services.BuildServiceProvider();

    return builder
        .GetRequiredService<IOptions<MvcOptions>>()
        .Value
        .InputFormatters
        .OfType<NewtonsoftJsonPatchInputFormatter>()
        .First();
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await RoleInitializer.InitializeAsync(scope.ServiceProvider);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();


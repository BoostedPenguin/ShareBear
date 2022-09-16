using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShareBear.BackgroundServices;
using ShareBear.Data;
using ShareBear.Helpers;
using ShareBear.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var provider = builder.Configuration.GetValue("Provider", "InMemory");
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


// HTTP Context DI accessor
builder.Services.AddHttpContextAccessor();

// HTTP Client DI
builder.Services.AddHttpClient();

// Automapper config
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// Database source config
if (builder.Environment.IsProduction())
{
    Console.WriteLine($"--> Using production database with provider: {provider}");


    builder.Services.AddDbContextFactory<DefaultContext>(
        options => _ = provider switch
        {
            "Npgsql" => options.UseNpgsql(builder.Configuration.GetConnectionString("ShareBearProdNpgsql"),
        x => x.MigrationsAssembly("GameService.NpgsqlMigrations")),

            "InMemory" => options.UseInMemoryDatabase("InMemoryProduction"),
    

                _ => throw new Exception($"Unsupported provider: {provider}")
        });
}
else
{
    Console.WriteLine($"--> Using development database with provider: {provider}");

    builder.Services.AddDbContextFactory<DefaultContext>(
        options => _ = provider switch
        {
            "Npgsql" => options.UseNpgsql(builder.Configuration.GetConnectionString("ShareBearDevNpgsql"),
        x => x.MigrationsAssembly("GameService.NpgsqlMigrations")),

            "InMemory" => options.UseInMemoryDatabase("InMemoryDevelopment"),

            _ => throw new Exception($"Unsupported provider: {provider}")
        });
}


// CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:19006/, http://192.168.0.100:19006/, exp://192.168.0.100:19000, https://192.168.0.100.nip.io:19006/")
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
    });
});


// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    //options.Authority = /* TODO: Insert Authority URL here */;

    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration.GetSection("AppSettings").GetValue<string>("Issuer"),
        ValidAudience = builder.Configuration.GetSection("AppSettings").GetValue<string>("Issuer"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings").GetValue<string>("Secret"))),
    };
});


// Add services here

builder.Services.AddSingleton<IAccountService, AccountService>();
builder.Services.AddSingleton<ScheduledFileDeletion>();
builder.Services.AddSingleton<IFileService, FileService>();
//builder.Services.AddHostedService<ContainerDeletionService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

await PrepService.PrepMigration(app);

app.Run();


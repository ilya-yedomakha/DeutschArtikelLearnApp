using DeutschArtikelLearnApp.Data;
using DeutschArtikelLearnApp.Help;
using DeutschArtikelLearnApp.Repositories;
using DeutschArtikelLearnApp.Repositories.Base;
using DeutschArtikelLearnApp.Services;
using DeutschArtikelLearnApp.Services.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddScoped(typeof(BaseRepository<>));
builder.Services.AddScoped<RightFormService>();
builder.Services.AddScoped<RightFormRepository>();
builder.Services.AddScoped<LessonRepository>();
//builder.Services.AddScoped(typeof(BaseService<,>)); // ??
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfiles>());
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(b => b.UseSqlServer(connectionString));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

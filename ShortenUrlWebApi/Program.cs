using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShortenUrlWebApi.MappingProfiles;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();
builder.Services.AddMvc();
builder.Services.AddAutoMapper(typeof(AppMappingProfileForUrl), typeof(AppMappingProfileForUrlList), typeof(AppMappingProfileForLinkForMyLinks));
builder.Services.AddScoped<IShortenService, ShortenService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy
                          /*policy.WithOrigins("http://example.com",
                                              "http://www.contoso.com",
                                              "https://localhost:7138",
                                              "https://localhost:3000")*/
                          .AllowAnyMethod()
                          .AllowAnyOrigin()
                          .AllowAnyHeader();
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
app.UseAuthentication();
app.MapControllers();
app.UseCors(MyAllowSpecificOrigins);

app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Links}/{action=ChangeLinkPrivacy}/{id}&{state}");

app.Run();

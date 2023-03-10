using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
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
builder.Services.AddScoped<IRedirectService, RedirectService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy
                          .WithOrigins("http://localhost:3000")
                          .WithOrigins("http://shorturl.com:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();

                      });
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
  {
      options.Cookie.Name = "UserLoginCookie";
      options.SlidingExpiration = true;
      options.ExpireTimeSpan = new TimeSpan(1, 0, 0); // Expires in 1 hour
      options.Events.OnRedirectToLogin = (context) =>
      {
          context.Response.StatusCode = StatusCodes.Status401Unauthorized;
          return Task.CompletedTask;
      };

      options.Cookie.HttpOnly = true;
      // Only use this when the sites are on different domains
      //options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
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
/*app.UseCookiePolicy(
    new CookiePolicyOptions
    {
        Secure = CookieSecurePolicy.Always //But what does Secure mean? It will check if the cookie is transmitted through HTTPS and only accept cookies from HTTPS. This is only forced when you want to set SameSite to None. That’s why, in local environments, I usually comment out both the SameSite and Secure policy.
    });*/

app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Links}/{action=ChangeLinkPrivacy}/{id}&{state}");

app.Run();

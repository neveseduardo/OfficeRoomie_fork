using Microsoft.EntityFrameworkCore;
using OfficeRoomie.Database;
using OfficeRoomie.Repositories;
using OfficeRoomie.Middleware;
using OfficeRoomie.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomAuthentication();
builder.Services.AddCustomSwaggerConfig();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IAdministradorRepository, AdministradorRepository>();

var app = builder.Build();



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Server");
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();


}

app.MapControllers();
app.MapRazorPages();
app.UseMiddleware<ErrorStatusCodeMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.RegisterRoutes();

app.Run();

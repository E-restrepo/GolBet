using GolBet.Repositories.Data;
using GolBet.Repositories.Implementations;
using GolBet.Repositories.Interfaces;
using GolBet.Services.Implementations;
using GolBet.Services.Interfaces;
using GolBet.Services.Mapping;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Este es el nuevo código  

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Open generic registration: one line, a repository for every entity 
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Specific repositories 
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IBetRepository, BetRepository>();

// AutoMapper: scans the assembly containing MappingProfile for all profiles 
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Business services 
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IBetService, BetService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();
// Seed the database on startup 

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

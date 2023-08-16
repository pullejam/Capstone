using CapstoneDebtSolver.Data;
using DebtDAL.Data;
using DebtDAL.Implementation;
using DebtDAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// 1 - Setup Dependency for our BulldogContext for injection
// Don't forget to setup DefaultConnection string to point to your SQL
builder.Services.AddDbContext<DebtContext>(
    options => options
    .UseLazyLoadingProxies() // Must add this line to allow lazy lading of relationships
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 2 - Setup Dependency for our IBulldogDAL for injection
builder.Services.AddScoped<IDebtDAL, EFDebtDAL>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // 3 - Setup our inital Debt data
        var context = services.GetRequiredService<DebtContext>();
        DbInitializer.Initialize(context);

        // 4 - Setup our initial users and roles
        var identiyContext = services.GetRequiredService<ApplicationDbContext>();
        identiyContext.Database.Migrate();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        IdentityDataInitializer.Initialize(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "ResultWithPayReq",
    pattern: "{controller=Home}/{action=Results}/{SalaryId?}/{HourlyId?}/{KeywordId?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
   
app.MapRazorPages();



app.Run();

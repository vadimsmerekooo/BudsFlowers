using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BudsFlowers.Areas.Identity.Data;
using BudsFlowers.Services;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("BudsContextConnection") ?? throw new InvalidOperationException("Connection string 'BudsContextConnection' not found.");
builder.Services.AddDbContext<BudsContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();



builder.Services.AddDefaultIdentity<User>(options =>
{
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZабвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ0123456789-._@+ ";
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.SignIn.RequireConfirmedAccount = true;
})
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<BudsContext>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/access-denied";
    options.Cookie.Name = "budsflowers";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(4320);
    options.LoginPath = "/account/signin";    
    options.SlidingExpiration = true;
});




builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddRazorPages();
BasketService.Deserialize();
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await DbInitializer.InitializeAsync(userManager, rolesManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});


app.Use(async (context, next) =>
{
    if (context.Request.Cookies.ContainsKey("basket"))
    {
        string idBasket = context.Request.Cookies["basket"];
    }
    else
    {
        long code = BasketService.SetId();
        if(code == 0)
        {
            context.Response.Cookies.Append("basket", "error");
        }
        else
        {
            context.Response.Cookies.Append("basket", code.ToString());
        }
    }
    await next(context);
});
app.Run();

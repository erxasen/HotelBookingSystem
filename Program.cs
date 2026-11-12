using HotelBookingSystem.Components;
using HotelBookingSystem.Data;
using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

const string connectionString = "Data Source=hotelbooking.db";

builder.Services.AddDbContextFactory<HotelDbContext>(options =>
    options.UseSqlite(connectionString));

// Identity работает с обычным (scoped) DbContext — берём его из той же фабрики.
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IDbContextFactory<HotelDbContext>>().CreateDbContext());

// ---- Аутентификация и роли ----
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorization();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<HotelDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
    options.AccessDeniedPath = "/account/access-denied";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Выход из аккаунта (POST + antiforgery-токен из формы в NavMenu)
app.MapPost("/account/logout", async (
    SignInManager<ApplicationUser> signInManager,
    [FromForm] string? returnUrl) =>
{
    await signInManager.SignOutAsync();
    return Results.LocalRedirect(LocalUrl.Sanitize(returnUrl));
});

using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider
        .GetRequiredService<IDbContextFactory<HotelDbContext>>();

    using (var db = factory.CreateDbContext())
    {
        db.Database.Migrate();
    }

    await IdentitySeeder.SeedAsync(scope.ServiceProvider, app.Configuration);
}

app.Run();

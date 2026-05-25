using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<_225DAPM32.Services.ApiClient>();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Configure Razor view engine to find views in feature folders
// builder.Services.Configure<RazorViewEngineOptions>(options =>
// {
//     // Add area view locations
//     options.AreaViewLocationFormats.Clear();
//     options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
//     options.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
//     options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
// });

var apiBaseUrl = builder.Configuration["Api:BaseUrl"] ?? "http://localhost:8000/api/";
var apiBaseUri = new Uri(apiBaseUrl.EndsWith('/') ? apiBaseUrl : $"{apiBaseUrl}/");

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = apiBaseUri;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStatusCodePagesWithReExecute("/Home/NotFoundPage");
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{controller=Admin}/{action=Index}/{id?}",
    defaults: new { area = "Admin" });

app.MapControllerRoute(
    name: "restaurant",
    pattern: "restaurant/{controller=Restaurant}/{action=Index}/{id?}",
    defaults: new { area = "Restaurant" });

app.MapControllerRoute(
    name: "shipper",
    pattern: "shipper/{controller=Shipper}/{action=Index}/{id?}",
    defaults: new { area = "Shipper" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

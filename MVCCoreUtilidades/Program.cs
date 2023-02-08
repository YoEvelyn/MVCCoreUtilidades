using MVCCoreUtilidades.Helpers;
using MVCCoreUtilidades.Services;

var builder = WebApplication.CreateBuilder(args);

string azureKeys = builder.Configuration.GetConnectionString("azurestoragekeys");
builder.Services.AddTransient<ServiceStorageFiles> (x => new ServiceStorageFiles(azureKeys));
builder.Services.AddTransient<ServiceStorageBlobs> (z => new ServiceStorageBlobs(azureKeys));
builder.Services.AddTransient<ServiceStorageTables>(y => new ServiceStorageTables(azureKeys));

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddTransient<HelperPathProvider>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

//INDICAMOS QUE VAMOS A UTILIZAR STATICFILES
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

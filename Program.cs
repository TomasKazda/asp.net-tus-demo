using tusdotnet;
using WebAppTusDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddTransient<FileManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapTus("/upload", async httpContext => new()
{
    // This method is called on each request so different configurations can be returned per user, domain, path etc.
    // Return null to disable tusdotnet for the current request.

    // Where to store data?

    Store = new tusdotnet.Stores.TusDiskStore(Path.Combine(app.Environment.ContentRootPath, "upload")),
    MaxAllowedUploadSizeInBytes = 10000000,
    Events = new()
    {
        // What to do when file is completely uploaded?
        OnFileCompleteAsync = async eventContext =>
        {
            var fsm = httpContext.RequestServices.GetService<FileManager>();

            tusdotnet.Interfaces.ITusFile file = await eventContext.GetFileAsync();

            await fsm.StoreTus(file, eventContext.CancellationToken);
        }
    }
});

app.MapRazorPages();

app.Run();

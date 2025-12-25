var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(); // این خط برای فعال کردن Razor Pages ضروریه

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // این خط برای سرویس‌دهی فایل‌های CSS، JS و تصاویر از پوشه wwwroot ضروریه

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages(); // این خط برای مسیریابی به Razor Pages ضروریه

app.Run();
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Services.SQL;
using Gadevang_Tennis_Klub.Services.SQL.Booking;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<IEventDB, EventDB_SQL>();
builder.Services.AddTransient<IActivityDB, ActivityDB_SQL>();
builder.Services.AddTransient<IEventBookingDB, EventBookingDB_SQL>();

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

app.MapRazorPages();

app.Run();

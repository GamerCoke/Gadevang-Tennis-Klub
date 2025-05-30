using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Services;
using Gadevang_Tennis_Klub.Services.SQL;
using Gadevang_Tennis_Klub.Services.SQL.Booking;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<IEventDB, EventDB_SQL>();
builder.Services.AddTransient<IActivityDB, ActivityDB_SQL>();
builder.Services.AddTransient<IEventBookingDB, EventBookingDB_SQL>();
builder.Services.AddTransient<ITrainerDB, TrainerDB_SQL>();
builder.Services.AddTransient<ITeamDB, TeamDB_SQL>();
builder.Services.AddTransient<IMemberDB, MemberDB_SQL>();
builder.Services.AddTransient<ICourtBookingDB, CourtBookingDB_SQL>();
builder.Services.AddTransient<ICourtDB, CourtDB_SQL>();
builder.Services.AddTransient<IEventBookingDB, EventBookingDB_SQL>();
builder.Services.AddTransient<ITeamBookingDB, TeamBookingDB_SQL>();
builder.Services.AddTransient<IAnnouncementDB, AnnouncementDB_SQL>();

builder.Services.AddTransient<OwnerData>();//OwnerData

builder.Services.AddSession();    //cookie
builder.Services.AddHttpContextAccessor();//cookie

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();  //cookie

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

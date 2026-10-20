using Microsoft.EntityFrameworkCore;
using SleepDiary1131708.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. 註冊 EF Core DbContext，使用 SQL Server
builder.Services.AddDbContext<SleepDiaryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. 註冊 Session 服務，用於登入驗證
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session 快取保留 30 分鐘
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// 3. 啟用預設靜態檔案 (優先支援讀取 wwwroot/index.html 作為首頁)
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

// 4. 啟用 Session Middleware (必須放在 UseRouting 與 UseAuthorization 之間)
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


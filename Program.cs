using DIHA.Data;
using DIHA.Menu;
using DIHA.Models;
using DIHA.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Cấu hình Serilog Logging
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug() // Ghi log từ cấp độ Debug trở lên
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.File("Logs/app-.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        builder.Host.UseSerilog();

        // Kết nối cơ sở dữ liệu SQL Server
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("AppMvcConnectionString")));

        // Cấu hình MVC
        builder.Services.AddControllersWithViews();
        // cấu hình react
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowReact", policy =>
            {
                policy.WithOrigins("http://localhost:3000") // Cổng React
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });
        // Cấu hình Identity
        builder.Services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        // Cấu hình tùy chỉnh cho Identity
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;  // Độ dài mật khẩu tối thiểu là 6 ký tự
            options.Password.RequiredUniqueChars = 1;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.AllowedForNewUsers = true;

            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.SignIn.RequireConfirmedAccount = true;
        });

        var Configuration = builder.Configuration;

        // Cấu hình đăng nhập với Google
        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                var gconfig = Configuration.GetSection("Authentication:Google");
                options.ClientId = gconfig["ClientId"] ?? throw new InvalidOperationException("Google ClientId is not configured.");
                options.ClientSecret = gconfig["ClientSecret"] ?? throw new InvalidOperationException("Google ClientSecret is not configured.");
                options.CallbackPath = "/dang-nhap-tu-google";
            })
            .AddFacebook(options =>
            {
                var fconfig = Configuration.GetSection("Authentication:Facebook");
                options.AppId = fconfig["AppId"] ?? throw new InvalidOperationException("Facebook AppId is not configured.");
                options.AppSecret = fconfig["AppSecret"] ?? throw new InvalidOperationException("Facebook AppSecret is not configured.");
                options.CallbackPath = "/dang-nhap-tu-facebook";
            });

        // Cấu hình dịch vụ Email
        builder.Services.AddOptions();
        var mailsetting = builder.Configuration.GetSection("MailSettings");
        builder.Services.Configure<MailSettings>(mailsetting);
        builder.Services.AddSingleton<IEmailSender, SendMailService>();

        // Cấu hình lỗi tiếng Việt cho Identity
        builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();

        // Cấu hình Authorization Policy
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("ViewManageMenu", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole(RoleName.Administrator);
            });

        // Đăng ký các dịch vụ hỗ trợ
        builder.Services.AddScoped<AdminSidebarService>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        builder.Services.AddSession();
        var app = builder.Build();

        
        app.UseSession();
        // Xử lý lỗi
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        // Middleware xử lý HTTPS và Static Files
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseCors("AllowReact");
        app.UseAuthorization();

        // Cấu hình định tuyến (Routing)
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}

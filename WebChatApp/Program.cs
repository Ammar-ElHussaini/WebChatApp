
using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using Data_Access_Layer.ProjectRoot.Infrastructure.Repositories;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebChatApp.Models;
using WebChatApp.Service.Implementations.AccountsService;
using WebChatApp.Service.Implementations.Chats.ChatUsersService.Helper;
using WebChatApp.Service.Implementations.Chats.GetAllChats;
using WebChatApp.Service.Implementations.Chats.GroupsServices.Helper;
using WebChatApp.Service.Implementations.CryptographyService;
using WebChatApp.Service.Implementations.LoggingService;
using WebChatApp.Service.Interfaces;
using WebChatApp.Service.Interfaces.AccountsService;
using WebChatApp.Service.Interfaces.IChats.IChatService.Helper;
using WebChatApp.Service.Interfaces.IChats.IGetAllChats;
using WebChatApp.Service.Interfaces.IChats.IGroupsServices.Helper;
using WebChatApp.Service.Interfaces.iLoggingService;
using WebChatApp.Service.Interfaces.NewFolder;

namespace WebChatApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();

            

            var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
            builder.Services.Configure<AppSettings>(builder.Configuration);


            builder.Services.AddDbContext<WebchatAppContext>(options =>
    options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection));

            builder.Services.AddScoped<WebchatAppContext>();


            // Identity Configuration
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<WebchatAppContext>()
                .AddDefaultTokenProviders();



            // Service Registration
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAccountsService, AccountsService>();
            builder.Services.AddScoped<IUserValidator, UserValidator>();

            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IUserContextService, UserContextService>();
            builder.Services.AddScoped<IUserStatusService, UserStatusService>();


            builder.Services.AddScoped<IGetAllChats, GetAllChats>();


            

            builder.Services.AddScoped<IGroupMemberService, GroupMemberService>(); 
            builder.Services.AddScoped<IGroupsMessagesService, GroupsMessagesService>(); 
            builder.Services.AddScoped<IGroupsUserManagement, GroupsUserManagement>(); 

            builder.Services.AddScoped<ILoggingService, LoggingService>(); 
            builder.Services.AddScoped<ICryptographyService, CryptographyService>(); 


            

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=index}/{id?}");

            app.MapHub<ChatHub>("/chatHub");


            app.Run();
        }
    }
}

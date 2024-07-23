using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.Data;
using Westwind.AspNetCore.Markdown;

namespace MyPortfolio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //Dbcontext 종속성 주입
            builder.Services.AddDbContext<AppDbContext>(Option => Option.UseSqlServer(
                builder.Configuration.GetConnectionString("Myconnection")));

            // MarkDown 관련 설정
            builder.Services.AddMarkdown();
            builder.Services.AddMvc().AddApplicationPart(typeof(MarkdownPageProcessorMiddleware).Assembly);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // 마크다운 사용설정
            app.UseMarkdown();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
                // URL패턴 : https://localhost:port/controller이름/action이름/{id}(옵션)
            app.Run();
        }
    }
}

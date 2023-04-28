using Microsoft.EntityFrameworkCore;
using Stud.DAL.Data;
using Stud.DAL.Repository.IRepository;
using Stud.DAL.Repository;
using Stud.Model.ForEmail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var con_string = builder.Configuration.GetConnectionString("con");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(con_string, ServerVersion.AutoDetect(con_string))
    );

//adding scopped
builder.Services.AddScoped<IEmailSender, EmailSender>();

//email configuration
var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailCon>();
builder.Services.AddSingleton(emailConfig);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=StudM}/{action=Login}/{id?}");

app.Run();

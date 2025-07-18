using Student_Management_System.DB_CONNECT;
using Student_Management_System.DB_CONNECT.Interfaces;
using Student_Management_System.Models;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register database services for Dependency Injection
//builder.Services.AddSingleton<StudentRepository>();
//builder.Services.AddSingleton<CourseRepository>();
//builder.Services.AddSingleton<TeacherRepository>();
//builder.Services.AddSingleton<DepartmentRepository>();

builder.Services.AddScoped<IStudent, StudentRepository>();
builder.Services.AddScoped<ICourse, CourseRepository>();
builder.Services.AddScoped<IDepartment, DepartmentRepository>();
builder.Services.AddScoped<ITeacher, TeacherRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // ✅ THIS LINE ENABLES DETAILED ERRORS
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=Index}/{id?}");

app.Run();

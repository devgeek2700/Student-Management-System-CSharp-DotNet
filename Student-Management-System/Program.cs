var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register database services for Dependency Injection
builder.Services.AddSingleton<Student_Management_System.DB_CONNECT.DbConnection>();
builder.Services.AddSingleton<Student_Management_System.DB_CONNECT.Courses>();
builder.Services.AddSingleton<Student_Management_System.DB_CONNECT.Teachers>();
builder.Services.AddSingleton<Student_Management_System.DB_CONNECT.Departments>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

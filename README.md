
---

## 🧠 How It Works

### 🔁 Data Flow

1. **Controller** → Handles user input and UI logic  
2. **Repository (via Interface)** → Performs DB operations  
3. **Models / ViewModels** → Transport and shape data  
4. **Views (.cshtml)** → Render the UI

---

## 🔁 CRUD Features

### 👨‍🎓 Students
- Add/Edit/Delete student records
- Assign multiple courses using `StudentCourse`
- Assign to departments
- View list with filtering and sorting

### 👩‍🏫 Teachers
- Manage teacher info
- Link to multiple courses via `TeacherCourse`
- Assign to departments

### 📘 Courses & 🏢 Departments
- Create, update, delete courses & departments
- Link courses with students and teachers
- Categorize and organize data

---

## 💡 Technical Details

| Feature              | Used                                 |
|----------------------|--------------------------------------|
| Architecture         | MVC + Repository Pattern             |
| UI Framework         | ASP.NET MVC + Razor Views            |
| Language             | C#                                   |
| ORM                  | Entity Framework / ADO.NET (choose)  |
| Database             | SQL Server                           |
| DI/IoC               | Interface-based Repository Injection |

---

## 💻 Screenshots

> Place images in the `wwwroot/screenshots/` folder and reference here.

### 🧍 Student List  
![Student List](wwwroot/screenshots/student-list.png)

### 👩‍🏫 Teacher List  
![Teacher List](wwwroot/screenshots/teacher-list.png)

### 📘 Course List  
![Course List](wwwroot/screenshots/course-list.png)

---

## 🚀 Getting Started

### 📋 Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Visual Studio](https://visualstudio.microsoft.com/)
- SQL Server

### 🔧 Installation

```bash
git clone https://github.com/devgeek2700/Student-Management-System-CSharp-DotNet.git

Open the project in Visual Studio

Update your SQL connection string in appsettings.json or web.config

Run the project (Ctrl + F5)

Create database tables via code-first migration or SQL scripts

🙌 Contributing
Want to add features or fix bugs?

bash
Copy
Edit
# Fork it
# Create your feature branch
git checkout -b feature/your-feature-name

# Commit your changes
git commit -m "Add feature"

# Push to the branch
git push origin feature/your-feature-name
Then open a Pull Request ✅

📄 License
MIT License. Feel free to use and modify.

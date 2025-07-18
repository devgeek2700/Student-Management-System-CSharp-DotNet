# Student Management System (C# .NET)

A robust, modular web application for managing students, teachers, courses, and departments using ASP.NET MVC, C#, and SQL Server.

---

## 🚀 Features

- **Students:** Add, edit, delete, filter, and sort student records. Assign students to multiple courses and departments.
- **Teachers:** Manage teacher details, link to multiple courses, and assign to departments.
- **Courses & Departments:** Organize, create, update, and delete courses and departments. Link courses to students and teachers for structured management.

---

## 🏛️ Architecture

- **MVC Pattern**: Clean separation of concerns for scalability and maintainability.
- **Repository Pattern**: Decoupled data access via interfaces for flexibility.
- **UI**: ASP.NET MVC with Razor Views for dynamic, server-rendered UI.
- **ORM**: Entity Framework and/or ADO.NET for efficient database communication.
- **Dependency Injection**: Interface-based repository injection.

---

## 🖥️ Screenshots

> Place images in the `wwwroot/screenshots/` folder and reference here.

| Student List | Teacher List | Course List |
|:---:|:---:|:---:|
| ![Student List](wwwroot/screenshots/student-list.png) | ![Teacher List](wwwroot/screenshots/teacher-list.png) | ![Course List](wwwroot/screenshots/course-list.png) |

---

## ⚙️ Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Visual Studio](https://visualstudio.microsoft.com/)
- SQL Server

### Installation

1. **Clone the Repository**
   ```bash
   git clone https://github.com/devgeek2700/Student-Management-System-CSharp-DotNet.git
   ```

2. **Open in Visual Studio**

3. **Configure Database**
   - Update the SQL connection string in `appsettings.json` or `web.config`.

4. **Run the Application**
   - Press `Ctrl + F5` in Visual Studio.

5. **Set Up Database**
   - Use code-first migrations or run provided SQL scripts to create database tables.

---

## 🤝 Contributing

Contributions are welcome! To propose a change:

1. **Fork** this repository
2. **Create a branch** for your feature or fix:
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. **Commit** your changes:
   ```bash
   git commit -m "Add feature"
   ```
4. **Push** to your branch:
   ```bash
   git push origin feature/your-feature-name
   ```
5. **Open a Pull Request** with a description of your changes.

---

## 📄 License

This project is licensed under the MIT License — use it freely and modify for your needs.

---

# 🍔 HumHum Food Delivery App

A scalable and modular food delivery platform built with **ASP.NET Core MVC** using **Onion Architecture**, designed to simulate real-world delivery apps with clean code, performance, and role-based access in mind.

---

## 📸 Demo

🎥 [Watch Demo on YouTube](#) https://youtu.be/RvarMUOWmig

---

## 🚀 Features

- 👥 **User Authentication & Role Management**
- 🏪 **Restaurant & Product Browsing**
- 🛒 **Real-time Cart Management**
- 📦 **Smart Checkout with Delivery Options**
- 💳 **Order & Payment Tracking**
- ☁️ **Cloudinary Integration** for Image Uploads
- 🧠 **Redis Caching** for Performance Boost
- 🧼 Clean Code Principles (SOLID, DI, Separation of Concerns)

---

## 🧱 Architecture

We applied **Onion Architecture** for maintainability and testability, separating concerns into distinct layers:

### 🔹 Core Layer

- `ITI.HumHum.Domain`: Core entities like `Order`, `Product`, `User`, etc.
- `ITI.HumHum.Service.Abstractions`: Service contracts (e.g., `ICartService`)
- `ITI.HumHum.Services`: Business logic implementations

### 🔹 Infrastructure Layer

- `ITI.HumHum.Persistence`:
  - EF Core `HumHumContext`
  - Repositories + Unit of Work pattern
  - Specification pattern for querying
  - Cloudinary integration

### 🔹 Presentation Layer

- ASP.NET Core MVC Controllers
- Razor Views with Bootstrap
- FluentValidation for clean form handling

---

## 🛠️ Tech Stack

| Layer      | Technology                              |
| ---------- | --------------------------------------- |
| Backend    | ASP.NET Core MVC, Entity Framework Core |
| Frontend   | Razor Views + Bootstrap                 |
| Database   | SQL Server                              |
| Caching    | Redis                                   |
| Validation | FluentValidation                        |
| Mapping    | AutoMapper                              |
| Media      | Cloudinary API                          |
| Patterns   | Onion Architecture, UoW, DI, SOLID      |

---

## 📁 Folder Structure

```
HumHum/
├── ITI.HumHum.Domain              # Entities
├── ITI.HumHum.Service.Abstractions  # Interfaces
├── ITI.HumHum.Services            # Business Logic
├── ITI.HumHum.Persistence         # EF Core, Repos
├── ITI.HumHum.Web                 # MVC UI
```

---

## ⚙️ How to Run

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/HumHum.git
```

### 2. Set Up Database

- Use SQL Server
- Update connection string in `appsettings.json`
- Apply migrations or import provided `.bak` file

### 3. Configure Environment Variables

```env
# Add your Cloudinary credentials
CLOUDINARY_CLOUD_NAME=...
CLOUDINARY_API_KEY=...
CLOUDINARY_API_SECRET=...

# Redis & other configs as needed
```

### 4. Run the Application

```bash
# Using Visual Studio or dotnet CLI
dotnet run --project ITI.HumHum.Web
```

---

## 📚 UML Diagrams Included

- ✅ Class Diagram
- ✅ Activity Diagram
- ✅ Sequence Diagram

---

## 👨‍💻 Team Members

This project was proudly developed by the MVC track team at **Information Technology Institute (ITI)**:

- Youssef Abdeltwab
- Matthew Maged
- Mohamed Khafaga
- Abdellateef Eid
- Ahmed Abosalem
- Mahmoud Salah

---

## 🏷️ Tags

`#ASP.NETCore` `#MVC` `#EFCore` `#Redis` `#SQLServer` `#FoodDeliveryApp` `#CleanArchitecture` `#Cloudinary` `#OnionArchitecture` `#Teamwork` `#ITIGraduationProject`

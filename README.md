# Barangay Telemedicine System
## ASP.NET Core MVC Web Application — IT Elective 2 Final Project
By Angel M. Baldonado, Francheska Jean S. Precioso, Gabriel D. Ragos
---

## Project Overview
A health information system addressing **SDG 3: Good Health and Well-being** by providing telemedicine and health record management for remote barangay communities.

---

## How to Run

1. Open the solution in **Visual Studio 2022**
2. Update `appsettings.json` with your SQL Server connection string
3. Open **Package Manager Console** and run:
   ```
   Add-Migration InitialCreate
   Update-Database
   ```
4. Press **F5** to run

---

## Available Accounts

### 🔴 Admin (seeded automatically on first run)

| Field    | Value                      |
|----------|----------------------------|
| Email    | `admin@barangay.gov.ph`    |
| Password | `Admin@123`                |
| Role     | Admin                      |
| Access   | Full access to all modules — Patients, Appointments, Consultations, Health Workers, Medicines, and user management |

### 🟡 Health Worker (register via /Account/Register)

| Field    | Value                                  |
|----------|----------------------------------------|
| Email    | Any valid email                        |
| Password | Min. 6 characters, must include a digit |
| Role     | HealthWorker                           |
| Access   | Can view Dashboard, manage Patients, Appointments, Consultations, and Medicines. Cannot manage Health Workers or delete records |

### 🟢 Patient (register via /Account/Register)

| Field    | Value                                  |
|----------|----------------------------------------|
| Email    | Any valid email                        |
| Password | Min. 6 characters, must include a digit |
| Role     | Patient                                |
| Access   | Can view Dashboard and their own records. Read-only access to most modules |

---

### Role Permissions Summary

| Module            | Admin | Health Worker | Patient |
|-------------------|:-----:|:-------------:|:-------:|
| Dashboard         | ✅    | ✅            | ✅      |
| Patients (CRUD)   | ✅    | ✅ (no delete)| 👁 view |
| Appointments (CRUD)| ✅   | ✅ (no delete)| 👁 view |
| Consultations (CRUD)| ✅  | ✅ (no delete)| 👁 view |
| Health Workers    | ✅    | ❌            | ❌      |
| Medicines         | ✅    | ✅            | ❌      |
| Delete any record | ✅    | ❌            | ❌      |

---

## Project Structure

```
BarangayTelemedicine/
│
├── Program.cs
├── BarangayTelemedicine.csproj
├── appsettings.json
│
├── Data/
│   ├── ApplicationDbContext.cs       
│   └── DbSeeder.cs                    
│
├── Models/
│   ├── ApplicationUser.cs             
│   ├── Patient.cs                     
│   ├── Appointment.cs                 
│   ├── Consultation.cs                
│   ├── HealthWorker.cs                
│   └── Medicine.cs                   
│
├── ViewModels/
│   └── ViewModels.cs                  
│
├── Controllers/
│   ├── HomeController.cs              
│   ├── AccountController.cs           
│   ├── DashboardController.cs        
│   ├── PatientsController.cs          
│   ├── AppointmentsController.cs      
│   ├── ConsultationsController.cs     
│   ├── HealthWorkersController.cs    
│   └── MedicinesController.cs        
│
└── Views/
    ├── _ViewImports.cshtml
    ├── _ViewStart.cshtml
    │
    ├── Shared/
    │   └── _Layout.cshtml             
    │
    ├── Account/
    │   ├── Login.cshtml
    │   ├── Register.cshtml
    │   └── AccessDenied.cshtml
    │
    ├── Dashboard/
    │   └── Index.cshtml             
    │
    ├── Patients/
    │   ├── Index.cshtml               
    │   ├── Create.cshtml
    │   ├── Edit.cshtml
    │   ├── Details.cshtml             
    │   └── Delete.cshtml
    │
    ├── Appointments/
    │   ├── Index.cshtml              
    │   ├── Create.cshtml
    │   ├── Edit.cshtml
    │   ├── Details.cshtml
    │   └── Delete.cshtml
    │
    ├── Consultations/
    │   ├── Index.cshtml              
    │   ├── Create.cshtml              
    │   ├── Edit.cshtml
    │   ├── Details.cshtml
    │   └── Delete.cshtml
    │
    ├── HealthWorkers/
    │   ├── Index.cshtml
    │   ├── Create.cshtml
    │   ├── Edit.cshtml
    │   ├── Details.cshtml
    │   └── Delete.cshtml
    │
    └── Medicines/
        ├── Index.cshtml             
        ├── Create.cshtml
        ├── Edit.cshtml
        ├── Details.cshtml
        └── Delete.cshtml
```

---

## System Features

| Feature | Details |
|---|---|
| User Roles | Admin, Health Worker, Patient |
| Dashboard | Summary cards: patients, appointments, consultations |
| Patients | Full CRUD, search by name/barangay |
| Appointments | Schedule, update status, filter by date/status |
| Consultations | Medical records, diagnoses, prescriptions |
| Health Workers | Manage barangay health staff |
| Medicines | Inventory tracking |
| Validation | Required fields, string length, date, error messages |
| SDG 3 | Health & well-being focus throughout |

---

## Database Tables (5+)
1. `Patients`
2. `Appointments`
3. `Consultations`
4. `HealthWorkers`
5. `Medicines`
6. `Users` (via ASP.NET Identity)

---

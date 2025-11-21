# PROG6212_POE_ST10451904
# Youtube Link: https://youtu.be/E8UgsmfllS0
# Contract Monthly Claim System (CMCS)

A comprehensive ASP.NET Core web application for managing monthly contract claims in an academic environment. This system facilitates the submission, review, and approval process for contract lecturers' monthly claims through a role-based workflow.

## 🚀 Features

### Role-Based Access Control
- **HR Administrators**: Full system access, user management, and comprehensive reporting
- **Academic Managers**: Final approval of claims and oversight
- **Programme Coordinators**: Initial claim review and approval
- **Lecturers**: Claim submission with document upload capabilities

### Core Functionality
- **Multi-step Claim Workflow**: Draft → Submitted → Coordinator Approved → Manager Approved
- **Document Management**: Upload and download supporting documents with ZIP archive support
- **Real-time Dashboard**: Role-specific overviews with key metrics
- **Comprehensive Reporting**: Export capabilities to CSV/Excel formats
- **User Management**: Create, edit, and manage users with role assignments

## 🛠️ Technology Stack

### Backend
- **Framework**: ASP.NET Core 9.0
- **Authentication**: ASP.NET Core Identity
- **Database**: Oracle Database
- **ORM**: Entity Framework Core

### Frontend
- **UI Framework**: Bootstrap 5
- **Icons**: Font Awesome 6
- **Client-side**: jQuery

## 📋 Prerequisites

- .NET 9.0 SDK
- Oracle Database (XE or higher)
- Oracle Managed Data Access

## 🗄️ Database Setup

1. **Run the Database Script**:
   ```sql
   -- Execute the complete setup script
   sqlplus SYSTEM/Password123@localhost:1521/XE @DBSetUp_PROG6212_POE.sql
   ```

2. **Default Users Created**:
   - **HR Admin**: `admin@university.edu` / `Password123!`
   - **Academic Manager**: `john.manager@university.edu` / `Password123!`
   - **Programme Coordinator**: `sarah.coordinator@university.edu` / `Password123!`
   - **Lecturer**: `david.lecturer@university.edu` / `Password123!`

## ⚙️ Configuration

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User Id=SYSTEM;Password=@Password1!;Data Source=localhost:1521/XE;"
  }
}
```

## 🚀 Running the Application

1. **Restore Dependencies**:
   ```bash
   dotnet restore
   ```

2. **Run the Application**:
   ```bash
   dotnet run
   ```

3. **Access the Application**:
   - Navigate to: `https://localhost:7000` (or the port shown in console)
   - Use one of the default accounts to log in

## 🔐 Security Features

- Role-based authorization
- Anti-forgery token validation
- Secure password policies
- Session management
- Input validation

## 📊 Key Workflows

### Claim Submission Process
1. **Lecturer** creates a draft claim
2. **Lecturer** submits claim with supporting documents
3. **Programme Coordinator** reviews and approves/rejects
4. **Academic Manager** provides final approval
5. **HR** can monitor and export all claims

### User Management
- HR can create users with automatic role assignment
- Password reset functionality
- User activation/deactivation
- Comprehensive user editing capabilities

## 🎯 Role Permissions

| Role | Permissions |
|------|-------------|
| HR Administrator | Full system access, user management, reports |
| Academic Manager | Final claim approval, dashboard analytics |
| Programme Coordinator | Initial claim review, document verification |
| Lecturer | Claim submission, draft management, document upload |

## 📈 Reporting & Analytics

- Monthly claim statistics
- Lecturer performance metrics
- Coordinator processing times
- Export functionality (CSV/Excel)
- Filterable claim listings

## 🐛 Troubleshooting

### Common Issues

1. **Database Connection**:
   - Verify Oracle service is running
   - Check connection string in appsettings.json
   - Ensure proper TNS configuration

2. **Authentication Issues**:
   - Verify default users exist in database
   - Check password requirements
   - Confirm role assignments

3. **File Upload Issues**:
   - Check database BLOB storage limits
   - Verify file size restrictions
   - Confirm content type validation

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 📄 License

This project is licensed for academic use as part of PROG6212 POE requirements.

**Note**: This system is designed for academic purposes as part of the PROG6212 Portfolio of Evidence requirements.

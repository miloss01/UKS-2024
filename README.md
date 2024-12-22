# DockerHubBackend Setup Instructions

Follow these steps to prepare and run the project:

## Prerequisites

### 1. Install PostgreSQL Database

You need a running PostgreSQL database. You can either:

- Install PostgreSQL directly on your system ([Download here](https://www.postgresql.org/download/))
- **OR** use a Docker container to run PostgreSQL:

#### Pull PostgreSQL Docker Image
```bash
docker pull postgres
```

#### Docker Command to Run PostgreSQL
```bash
docker run --name postgres-container -e POSTGRES_USER=<username> -e POSTGRES_PASSWORD=<password> -e POSTGRES_DB=<database_name> -p 5432:5432 -d postgres
```
Replace `<username>`, `<password>`, and `<database_name>` with the credentials defined in `appsettings.json`.

### 2. Configure `appsettings.json`
Ensure that your `appsettings.json` file is configured with the correct database credentials. Example:

```json
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=<database_name>;Username=<username>;Password=<password>"
}
```

### 3. Configure `super_admin_cred.json`
Create `super_admin_cred.json` file inside `Startup` folder. The file should have the structure as shown below:
```json
{
  "generatePassword": false,
  "password": "",
  "email": "admin@email.com"
}
```
`generatePassword` - Should the application generate super admin account on the first run  
`password` - This attribute will be updated with super admin account password, after application is run, if the `generatePassword` is set to `true`  
`email` - Email for super admin account  

If `generatePassword` is true, after the first successful application run its value will be automatically changed to `false`.
## Running the Project

### 1. Apply Migrations
Open the **NuGet Package Manager Console** in Visual Studio and run the following commands:

#### Step 1: Add Initial Migration
```powershell
Add-Migration InitialCreate -Project DockerHubBackend -StartupProject DockerHubBackend
```

#### Step 2: Update the Database
```powershell
Update-Database -Project DockerHubBackend -StartupProject DockerHubBackend
```

### 2. Start the Project
Finally, run the project in Visual Studio to start the application.

---

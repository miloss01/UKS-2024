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

### 2. Setup Docker Registry Auth Server
1. Open Git Bash
2. Create certificate with the following command(certificate will be saved in the folder where terminal is located):
```bash
openssl req -x509 -newkey rsa:4096 -keyout key.pem -out cert.pem -days 365 -nodes -subj '//CN=uks-registry'
```
3.  Copy `cert.pem` and `key.pem` in the same location as `app.py`
3.  Copy the `cert.pem` file to `Registry/certs` folder
4.  In `config.py` edit `DATABASE_URI` to match URI of the main database
5.  Run the server

### 3. Setup Docker Registry Webhooks Server
1. Open `RegistryWebhookServer/config.py` and edit `DATABASE_URI` to match URI of the main database
2. Run the server

### 4. Setup Docker Registry Server
1.  Navigate to the `Registry` folder
2.  Copy the certificate from before (`cert.pem`) to the `certs` subfolder
3.  Edit ip address of auth server in `docker-compose.yml`
4.  Run the following command from the `Registry` folder to start the docker registry server:
```bash
docker compose up -d
```
4.  Check the logs for any error messages

### 5. Configure `appsettings.json`
Ensure that your `appsettings.json` file is configured with the correct database credentials. Example:

```json
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=<database_name>;Username=<username>;Password=<password>"
}
```

### 6. Configure `super_admin_cred.json`
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

## Example usage of local docker registry
1. Login with username and password:
```
docker login
```

2. Tag local docker image:
```
docker tag my-app:latest localhost:5000/my-app:v1.0
```

3. Push the image to registry:
```
docker push localhost:5000/my-app:v1.0
```

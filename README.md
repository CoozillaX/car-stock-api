# Car Stock API

A RESTful API built with **C#** for managing dealer car stock. JWT
authentication ensures each dealer can only access their own data.\
The API uses **FastEndpoints**, **Dapper**, and a local **SQLite**
database, and includes **Swagger** for easy testing.

------------------------------------------------------------------------

## Tech Stack

- .NET / ASP.NET Core
- FastEndpoints
- Dapper
- SQLite
- JWT Authentication

------------------------------------------------------------------------

## Requirements

- .NET SDK 10.0 (or compatible preview version)
- No external database required (uses SQLite locally)

------------------------------------------------------------------------

## How to Run

### 1. Start the API

``` bash
dotnet run
```

The SQLite database will be created automatically.

------------------------------------------------------------------------

### 2. (Optional) Seed Demo Users

Uncomment in `Program.cs`:

``` csharp
// .AddSingleton<DbSeeder>()
```

and

``` csharp
// var seeder = scope.ServiceProvider 
//     .GetRequiredService<DbSeeder>();
// await seeder.SeedAsync();
```

This creates:

| Username | Password |
|----------|----------|
| user1    | password |
| user2    | password |
| user3    | password |

------------------------------------------------------------------------

### 3. Open Swagger

    http://localhost:5037/swagger

Use Swagger to test all endpoints.

------------------------------------------------------------------------

### 4. Login

    POST /auth/login

Example:

``` json
{
  "username": "user1",
  "password": "password"
}
```

Copy the returned token.

------------------------------------------------------------------------

### 5. Authorize

Click **Authorize** in Swagger and enter:

    Bearer {token}

------------------------------------------------------------------------

## Endpoints

- `GET /api/cars` --- List or search cars\
- `POST /api/cars` --- Add car\
- `PATCH /api/cars/{id}` --- Update car\
- `DELETE /api/cars/{id}` --- Remove car

### Search Example

    GET /api/cars?make=Toyota&model=Camry

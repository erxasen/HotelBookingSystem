# 🏨 Hotel Booking System

## Project Overview

**Hotel Booking System** is a web application developed using ASP.NET Core Blazor, C#, Entity Framework Core, and SQLite.

The application allows users to search for hotels, check room availability, make reservations, manage their bookings, and cancel reservations. It also provides an administrator panel for managing hotels and rooms.

The project demonstrates full-stack web development, database integration, authentication, role-based authorization, business logic separation, automated testing, and collaborative development using GitHub.

## Technologies Used

| Technology | Purpose |
|---|---|
| C# / .NET 10 | Application development |
| ASP.NET Core Blazor | Interactive web interface |
| Entity Framework Core | Database access and object-relational mapping |
| SQLite | Relational database |
| ASP.NET Core Identity | Authentication and role-based authorization |
| Bootstrap | Responsive user interface |
| xUnit | Automated testing |
| Microsoft.Data.Sqlite | SQLite integration testing |
| Git and GitHub | Version control and collaboration |
| Visual Studio Code | Development environment |

## Main Features

### User Features

- User registration, login, and logout
- Secure authentication
- Search hotels by name or city
- Browse available hotels
- Select check-in and check-out dates
- Search for available rooms
- Filter rooms by guest capacity
- View room details and nightly prices
- Calculate total reservation price
- Create hotel reservations
- View personal bookings
- Cancel existing reservations
- Prevent overlapping bookings through availability checks

### Administrator Features

- Role-based administrator access
- Administrator dashboard
- Create, edit, and delete hotels
- Create, edit, and delete rooms
- Manage room prices and capacity
- Activate or deactivate rooms
- View hotel and booking statistics

## Application Architecture

The application uses a **layered architecture** with Blazor components, business logic services, Entity Framework Core, and SQLite.

### Presentation Layer

Blazor components provide the user interface.

Main pages include:

- `Home.razor` — Hotel browsing and search
- `Booking.razor` — Room availability and reservation creation
- `MyBookings.razor` — Personal reservation management
- `Account/Login.razor` — User authentication
- `Account/Register.razor` — Account registration
- `Admin/` — Hotel and room administration

### Business Logic Layer

**BookingService.cs**

Provides reusable room availability logic. It checks:

- Selected hotel
- Room activation status
- Guest capacity
- Check-in and check-out dates
- Existing overlapping reservations

**BookingRules.cs**

Contains reusable date validation and overlap-checking rules.

### Data Access Layer

**HotelDbContext.cs** uses Entity Framework Core to manage database operations and relationships.

The application also uses ASP.NET Core Identity for user accounts and roles.

Some page-level operations access the database context directly.

### Database Layer

SQLite stores application data, including hotels, rooms, bookings, users, and roles.

## Database Design

| Entity | Description |
|---|---|
| Hotel | Hotel name, city, price, rating, and image |
| Room | Hotel reference, room number, type, capacity, price, and activation status |
| Booking | Hotel, room, user, dates, guest details, and total price |
| ApplicationUser | Registered user information |
| Identity Roles | User and administrator permissions |

### Main Relationships

- One hotel can have multiple rooms.
- One hotel can have multiple bookings.
- One room can have multiple bookings on different dates.
- One registered user can have multiple bookings.

Entity Framework Core migrations manage database schema changes.

## Room Availability and Booking Validation

Room availability is calculated using the selected hotel, dates, number of guests, room capacity, room status, and existing reservations.

Two reservations overlap when:

`existing.CheckIn < requested.CheckOut && requested.CheckIn < existing.CheckOut`

This allows adjacent reservations where one guest checks out on the same date another guest checks in.

The application checks room availability when searching and checks again before saving a reservation.

Booking creation also uses a database transaction.

These mechanisms reduce the risk of double booking. Additional concurrency testing and database-level safeguards would be valuable future improvements.

## Installation and Setup

### Requirements

- .NET 10 SDK
- Visual Studio Code or Visual Studio 2022
- Git

### 1. Clone the Repository

```bash
git clone https://github.com/erxasen/HotelBookingSystem.git
cd HotelBookingSystem
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Apply Database Migrations

```bash
dotnet ef database update
```

If the Entity Framework Core CLI is not installed:

```bash
dotnet tool install --global dotnet-ef --version 10.0.12
```

The application also applies migrations during startup in the current development configuration.

### 4. Build the Application

```bash
dotnet build
```

### 5. Run the Application

```bash
dotnet run
```

Open the local address displayed in the terminal.

For the current development configuration, the application has been run at:

`http://localhost:5142`

## How to Use

### Regular User

1. Register a new account.
2. Log in.
3. Search for a hotel.
4. Open the booking page.
5. Select the hotel, dates, and number of guests.
6. Click **Find Available Rooms**.
7. Select an available room.
8. Enter the required guest information.
9. Click **Confirm Booking**.
10. Open **My Bookings** to view or cancel reservations.

### Administrator

1. Log in with an account assigned the Admin role.
2. Open the Admin Panel.
3. Manage hotels and their information.
4. Manage rooms, prices, and capacity.
5. Activate or deactivate rooms.

Administrator functionality

# 🏨 Hotel Booking System

## Project Overview

Hotel Booking System is a web application developed using **ASP.NET Core Blazor, C#, Entity Framework Core, and SQLite**.

The system allows users to search for hotels, check room availability, make reservations, manage their bookings, and cancel reservations.

It also provides an administrator panel for managing hotels and rooms.

## Technologies Used

- **C# / .NET 10** — Backend development
- **ASP.NET Core Blazor Server** — Interactive web interface
- **Entity Framework Core** — Database operations
- **SQLite** — Database storage
- **ASP.NET Core Identity** — Authentication and authorization
- **Bootstrap** — Responsive user interface
- **Git and GitHub** — Version control and collaboration
- **Visual Studio Code** — Development environment

## Main Features

### User Features

- User registration and login
- Secure authentication and logout
- Search hotels by name or city
- View available hotels
- Select check-in and check-out dates
- Search for available rooms
- Filter rooms by guest capacity
- View room prices
- Calculate the total booking price
- Create hotel reservations
- View personal bookings
- Cancel existing reservations
- Prevent booking an occupied room for overlapping dates

### Administrator Features

- Administrator authentication
- Role-based access control
- Admin dashboard
- Add, edit, and delete hotels
- Add, edit, and delete rooms
- Manage room capacity and prices
- Activate or hide rooms
- View hotel and booking statistics

## Database

The application uses SQLite with Entity Framework Core.

Main entities include:

| Entity | Description |
|---|---|
| Hotel | Stores hotel information |
| Room | Stores room details, capacity, price, and status |
| Booking | Stores reservation details and dates |
| ApplicationUser | Stores registered user information |
| Identity Roles | Manages user and administrator permissions |

Entity Framework Core migrations are used to manage database schema changes.

## Room Availability

The application checks room availability based on:

1. Selected hotel
2. Check-in and check-out dates
3. Number of guests
4. Room capacity
5. Room activation status
6. Existing reservations

A room is unavailable when an existing reservation overlaps the requested dates.

The system checks availability again before saving a booking to reduce the risk of double booking.

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

If the EF Core CLI tool is not installed:

```bash
dotnet tool install --global dotnet-ef --version 10.0.12
```

### 4. Run the Application

```bash
dotnet run
```

Open the local address displayed in the terminal, typically:

http://localhost:5142

## How to Use

### Regular User

1. Register a new account.
2. Log in to the application.
3. Search for a hotel.
4. Open the booking page.
5. Select the hotel, dates, and number of guests.
6. Click **Find Available Rooms**.
7. Select an available room.
8. Click **Confirm Booking**.
9. Open **My Bookings** to view or cancel reservations.

### Administrator

1. Log in using an account assigned the Admin role.
2. Open the Admin Panel.
3. Manage hotels and their information.
4. Open the Rooms section for a hotel.
5. Add or edit rooms, prices, and capacity.
6. Activate or hide rooms when necessary.

Administrator access requires the appropriate role configuration.

## Testing

The following workflows were manually tested:

- User authentication
- Administrator access restrictions
- Hotel and room management
- Room availability search
- Booking creation
- Prevention of overlapping bookings in normal usage
- Personal booking visibility
- Booking cancellation
- Room availability after cancellation

Build and test commands:

```bash
dotnet build
dotnet test
```

## Version Control

The project was developed collaboratively using Git and GitHub.

Feature branches and pull requests were used to integrate functionality into the main branch.

## Project Structure

```text
HotelBookingSystem/
├── Components/
│   ├── Layout/
│   └── Pages/
│       ├── Account/
│       ├── Admin/
│       ├── Home.razor
│       ├── Booking.razor
│       └── MyBookings.razor
├── Data/
├── Models/
├── Migrations/
├── HotelBookingSystem.Tests/
├── Program.cs
├── HotelBookingSystem.csproj
└── README.md
```

## Team Responsibilities

**Developer 1 — Booking System**

- Home page and hotel search
- Booking interface
- Room availability
- Reservation creation
- Personal booking management
- Booking cancellation
- Database integration for bookings

**Developer 2 — Authentication and Administration**

- User registration and login
- Authentication and authorization
- User and administrator roles
- Administrator dashboard
- Hotel management
- Room management

## Future Improvements

- Online payment integration
- Email booking confirmations
- Customer reviews and ratings
- Advanced hotel filters
- Booking history and reporting
- Additional automated and concurrent booking tests

## Conclusion

Hotel Booking System demonstrates the development of a database-driven web application using modern .NET technologies.

The project combines authentication, role-based authorization, hotel management, room availability checking, and reservation management in a single application.

It also demonstrates collaborative software development using GitHub.

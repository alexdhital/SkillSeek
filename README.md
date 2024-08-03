# SkillSeek

SkillSeek is a professional hiring application built using ASP.NET Core 6, Razor Pages, and PostgreSQL database.

## Features

- **Streamlined Booking**: Book services easily and keep track of your bookings.
- **Purchase Products**: Purchase products directly from site.
- **Checkout**: Streamlined checkout process.
- **Hire Professionals**: Browse and find the right professional for your needs.

## Roles
- **Admin**
- **Customer**
- **Professionals**

## Technologies Used

- **Backend**: ASP.NET Core 6
- **Frontend**: Razor Pages
- **Database**: PostgreSQL

## Installation

1. Clone the repository:

    ```sh
    git clone https://github.com/yourusername/SkillSeek.git
    cd SkillSeek
    ```

2. Set up the database:

    Ensure you have PostgreSQL installed and running. Create a database for the application.

3. Update the connection string:

    Update the `appsettings.json` file with your PostgreSQL connection string.

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Host=your_host;Database=your_database;Username=your_username;Password=your_password"
      }
    }
    ```

4. Apply migrations:

    ```sh
    dotnet ef database update
    ```

5. Run the application:

    ```sh
    dotnet run
    ```

## Screenshots

### Booking

![Booking](https://i.postimg.cc/5NjpxH38/Bookings.png)

### Purchasing Products

![Purchasing Products](https://i.postimg.cc/522J9Srn/Products.png)

### Checkout

![Checkout](https://i.postimg.cc/c4K7ppxm/checkout.png)

### Hire Professionals 

![Professionals List](https://i.postimg.cc/6qZ30fKd/Professionals.png)

### Professional Services

![Professional Services](https://i.postimg.cc/fWCgWQhQ/Services.png)

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

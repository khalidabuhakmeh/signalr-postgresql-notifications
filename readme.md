# PostgreSQL, SignalR, and Realtime Notifications

This is a working sample built on the ideas expressed in Calvin A. Allen's original [blog post](https://www.codingwithcalvin.net/real-time-ui-updates-with-postgres-and-signalr/).

The sample shows a `BackgroundService` listening for notifications from a PostgreSQL database instance. When a notification occurs, the application communicates through **SignalR** to all connected clients.

## Requirements

- .NET 5.0+
- PostgreSQL database instance

## Getting Started

1. Update the connection string in `appsettings.json`
2. Connect to your **PostgreSQL** database instance.
3. Start the project using `dotnet run`.
4. Run the following `SQL` command from a database console window.

```postgresql
NOTIFY total_count, '1';
```

You should see the index page at `https://localhost:5001` update with the value 1. 

Try it with `9001`.

```postgresql
NOTIFY total_count, '9001';
```

Cheers!
#!/bin/bash

# exit if a command fails
set -e

DB_FILE="bin/Debug/net10.0/TvTracker.db"

# delete db file if exists
# faster than dotnet cli "dotnet ef database drop"
if [ -f "$DB_FILE" ]; then
    echo "Deleting existing database: $DB_FILE"
    rm "$DB_FILE"
else
    echo "No existing database found."
fi

# apply migrations to generate new db
echo "Applying migrations..."
dotnet ef database update

# run app
echo "Starting the app..."
dotnet run

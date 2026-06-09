#!/bin/bash

echo "Starting all MyThings APIs..."

# The '&' at the end tells the Mac to run these in the background
dotnet run --project MyThings.API.Customer &
dotnet run --project MyThings.API.Admin &
dotnet run --project MyThings.API.Partner &
dotnet run --project MyThings.API.Driver &

echo "All APIs are launching. Check your ports (5001-5004)."
# This keeps the script 'alive' so the APIs don't close immediately
wait
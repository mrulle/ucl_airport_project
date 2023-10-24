#!/bin/bash
docker build -f ./src/BookingApi/Dockerfile -t bookingapi ./src/bookingapi/
docker build -f ./src/ContentEnricher/Dockerfile -t contentenricher  ./src/contentenricher/

read -p "What Environment would you like ('Production(p, P)') else development: " VAR1

if [ "$VAR1" = "Production" ] || [ "$VAR1" = "p" ] || [ "$VAR1" = "P" ]; then
    echo "Production"
    docker compose --env-file ./.env.prod build
    docker compose --env-file ./.env.prod up -d
else
    echo "Development"
    docker compose --env-file ./.env build
    docker compose --env-file ./.env up -d
fi

read -p "Press any key to continue" HELLO
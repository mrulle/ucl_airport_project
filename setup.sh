#!/bin/bash 
docker compose down
docker build -f ./src/BookingApi/Dockerfile -t bookingapi ./src/BookingApi/

read -p "What Environment would you like ('Production(p, P)') else development: " VAR1

if [ "$VAR1" = "Production" ] || [ "$VAR1" = "p" ] || [ "$VAR1" = "P" ]; then
    echo "<----------------    Production    ---------------->"
    docker volume rm postgres_prod
    docker compose --env-file ./.env.prod build
    docker compose --env-file ./.env.prod up -d
else
    echo "<----------------    Development    ---------------->"
    docker volume rm postgres_dev
    docker compose --env-file ./.env build
    docker compose --env-file ./.env up -d
fi

read -p "Press any key to continue" HELLO

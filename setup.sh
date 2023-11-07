#!/bin/bash 
docker compose down
docker build -f ./src/BookingApi/Dockerfile -t bookingapi ./src/BookingApi/

read -p "What Environment would you like ('Production(p, P)') else development: " VAR1
read -p "Should the Volume be deleted? (y/Y)? " VAR2

if [ "$VAR1" = "Production" ] || [ "$VAR1" = "p" ] || [ "$VAR1" = "P" ]; then
    echo "<----------------    Production    ---------------->"
#    docker volume rm postgres_prod
    docker compose --env-file ./.env.prod build
    docker compose --env-file ./.env.prod up -d
else
    if ["$VAR2" = "y" ] || [ "$VAR2" = "Y" ]; then
        docker volume rm postgres_dev
    fi
    echo "<----------------    Development    ---------------->"
    docker compose --env-file ./.env build
    docker compose --env-file ./.env up -d
fi

read -p "Press any key to continue" HELLO

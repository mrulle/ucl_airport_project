#!/bin/bash

# Exits immediately if any errors occur
set -o errexit

#Array containing enviornment variables
# variable expansion ${REQUIRED_ENV_VARS[@]}
readonly REQUIRED_ENV_VARS=(
    "APP_DB_USER",
    "APP_DB_PASS",
    "APP_DB_NAME",
    "POSTGRES_USER",
    "POSTGRES_PASSWORD"
)

#MAIN
# - verifies all env vars set
# - runs SQL code to create user and DB
main() {
    echo "running main"
    #check_env_vars_set
    init_user_and_db
    initalize_database_tables
    initalize_database_views

    /bin/bash ../x-db_seeder.sh
}

#check all required env vars set
#echos texting explaining which arent set
#and the name of the ones that need to be
check_env_vars_set() {
    for required_env_var in ${REQUIRED_ENV_VARS[@]}; do
        if [[ -z "${!required_env_var}" ]]; then
            echo "ERROR: Enviornment variable '$required_env_var' not set.
            Make sure you have the following enviornment varaibles set:

            ${REQUIRED_ENV_VARS[@]}

            Aborting."

            exit 1
        fi
    done
}

# Initializes already started PostgreSQL
# uses preconfigured POSTGRES_USER user
init_user_and_db() {
    echo "Running init user"
    psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" <<-EOSQL
        CREATE USER $APP_DB_USER WITH PASSWORD '$APP_DB_PASS';
        CREATE DATABASE $APP_DB_NAME;
        GRANT ALL PRIVILEGES ON DATABASE $APP_DB_NAME TO $APP_DB_USER;
EOSQL
}
#\connect $APP_DB_NAME $APP_DB_USER

initalize_database_tables(){
    echo "Running database tables"
    psql -v ON_ERROR_STOP=1 --username "$APP_DB_USER" --dbname "$APP_DB_NAME" <<-EOSQL
    BEGIN;
        CREATE TABLE passengers (
            id uuid PRIMARY KEY,
            name VARCHAR(255),
            email VARCHAR(255),
            photo BYTEA,
            passport_number VARCHAR(255)
        );

        CREATE TABLE planes (
            id uuid PRIMARY KEY,
            max_passengers INT,
            max_baggage_total INT,
            max_baggage_weight INT,
            max_baggage_dimension INT
        );

        CREATE TABLE flights (
            id uuid PRIMARY KEY,
            arrival_time TIMESTAMP,
            departure TIMESTAMP,
            origin VARCHAR(255),
            destination VARCHAR(255),
            plane_id uuid,
            CONSTRAINT fk_flights_planes
                FOREIGN KEY(plane_id)
                    REFERENCES planes(id)
        );

        CREATE TABLE checkins (
            id uuid PRIMARY KEY,
            is_checked_in BOOLEAN
        );

        CREATE TABLE bookings (
            id uuid PRIMARY KEY,
            created_date TIMESTAMP,
            number uuid,
            checkin_id uuid,
            passenger_id uuid,
            flight_id uuid,
            CONSTRAINT fk_bookings_checkins
                FOREIGN KEY(checkin_id)
                    REFERENCES checkins(id),
            CONSTRAINT fk_bookings_passengers
                FOREIGN KEY(passenger_id)
                    REFERENCES passengers(id),
            CONSTRAINT fk_bookings_flight
                FOREIGN KEY(flight_id)
                    REFERENCES flights(id)
        );

        CREATE TABLE baggage (
            id uuid PRIMARY KEY,
            weight INT,
            booking_id uuid,
            CONSTRAINT fk_baggage_bookings
                FOREIGN KEY(booking_id)
                    REFERENCES bookings(id)
        );

    COMMIT;
EOSQL
}

initalize_database_views(){
    echo "Creating db views"
    initalize_boarding_pass_view
    initalize_booking_view
    initalize_checkin_view
    initalize_flight_info_view
}

initalize_boarding_pass_view(){
    echo "Creating boarding_pass_view"
    psql -v ON_ERROR_STOP=1 --username "$APP_DB_USER" --dbname "$APP_DB_NAME" <<-EOSQL
    BEGIN;

    create or replace view vw_boarding_pass
    AS SELECT checkin_id, passenger_id, flight_id
    FROM bookings

    COMMIT;
EOSQL
}

initalize_booking_view(){
    echo "Creating booking_view"
    psql -v ON_ERROR_STOP=1 --username "$APP_DB_USER" --dbname "$APP_DB_NAME" <<-EOSQL
    BEGIN;

    create or replace view vw_booking
    AS SELECT
        book.id,
        p.email,
        p.passport_number,
        bag.weight

    FROM bookings book
        inner join passengers p
            on p.id = book.id
        inner join baggage bag
            on bag.id = book.id;

    COMMIT;
EOSQL
}

initalize_checkin_view(){
    echo "Creating checkin_view"
    psql -v ON_ERROR_STOP=1 --username "$APP_DB_USER" --dbname "$APP_DB_NAME" <<-EOSQL
    BEGIN;

    create or replace view vw_checkin
    AS SELECT
        b.id,
        p.email

    FROM bookings b
        inner join passengers p
            on p.id = b.id;

    COMMIT;
EOSQL
}

initalize_flight_info_view(){
    echo "Creating flight_info_view"
    psql -v ON_ERROR_STOP=1 --username "$APP_DB_USER" --dbname "$APP_DB_NAME" <<-EOSQL
    BEGIN;

    create or replace view vw_flight_info
    AS SELECT
        p.id as plane_id,
        p.max_passengers,
        p.max_baggage_weight,
        f.id as flight_id,
        f.arrival_time,
        f.departure,
        f.origin,
        f.destination

    FROM flights f
        inner join planes p
            on p.id = f.plane_id;

    COMMIT;
EOSQL
}

# Runs main routing with env vars passed through command line
main"$@"

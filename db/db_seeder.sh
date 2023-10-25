#!/bin/bash

random_string() {
    local len=28
    local str=""
    local chars="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"

    for (( i=0; i<$len; i++ )); do
        str+="${chars:$RANDOM % ${#chars}:1}"
    done

    echo $str
}

seed_database_tables(){
    passengers_ids=()
    checkin_ids=()
    flight_ids=()
    booking_ids=()
    plane_ids=()
    idx=0   #initialize a counter to zero

    for i in {1..10} #some random number range
    do
        passengers_ids[idx]=$(random_string)
        checkin_ids[idx]=$(random_string)
        flight_ids[idx]=$(random_string)
        booking_ids[idx]=$(random_string)
        plane_ids[idx]=$(random_string)
        idx=$((idx+1)) #increment the counter
    done
    read -p "Press any key to continue" HELLO
    echo "Seeding database tables"
    #Seed planes table
    for id in "${passenger_ids[@]}"; do
        random_max_passengers=$((RANDOM % 1000 + 1))  # Random number between 1 and 1000
        random_max_baggageTotal=$((RANDOM % 2000 + 1))  # Random number between 1 and 2000
        random_max_baggageWeight=$((RANDOM % 50 + 1))  # Random number between 1 and 50
        random_max_baggageDimension=$((RANDOM % 50 + 1))  # Random number between 1 and 50

        seed_planes "$id" "$random_max_passengers" "$random_max_baggageTotal" "$random_max_baggageWeight" "$random_max_baggageDimension"
    done
    # Seed passengers
    for id in "${passenger_ids[@]}"; do
        random_name=$(random_string 10)  # Random name of length 10
        random_email=$(random_string 10)  # Random email of length 10
        random_photo=$(random_string 10)  # Random photo of length 10
        random_passportNumber=$(random_string 10)  # Random passport number of length 10

        seed_passengers "$id" "$random_name" "$random_email" "$random_photo" "$random_passportNumber"
    done
    # Seed flights
    for i in {1..10}; do
        random_flight_id=$(random_string 28)  # Random flight id of length 28
        random_arrival_time=$(date -u -d "2023-10-01 00:00:00 + $RANDOM seconds")  # Random arrival time
        random_departure=$(date -u -d "2023-10-01 00:00:00 + $RANDOM seconds")  # Random departure time
        random_origin=$(random_string 10)  # Random origin of length 10
        random_destination=$(random_string 10)  # Random destination of length 10
        raom_plane_ndid=${plane_ids[$RANDOM % ${#plane_ids[@]}]}  # Random plane id

        seed_flights "$random_flight_id" "$random_arrival_time" "$random_departure" "$random_origin" "$random_destination" "$random_plane_id"
    done

    # Seed checkins
    for i in {1..10}; do
        random_checkin_id=$(random_string 28)  # Random checkin id of length 28
        random_is_checked_in=$((RANDOM % 2))  # Random boolean value

        seed_checkins "$random_checkin_id" "$random_is_checked_in"
    done

    # seed_bookings
    for i in {1..10}; do
        random_booking_id=$(random_string 28)  # Random booking id of length 28
        random_created_date=$(date -u -d "2023-10-01 00:00:00 + $RANDOM seconds")  # Random created date
        random_number=$(random_string 28)  # Random number of length 28
        random_checkin_id=${checkin_ids[$RANDOM % ${#checkin_ids[@]}]}  # Random checkin id
        random_passenger_id=${passenger_ids[$RANDOM % ${#passenger_ids[@]}]}  # Random passenger id
        random_flight_id=${flight_ids[$RANDOM % ${#flight_ids[@]}]}  # Random flight id
    
        # Randomly decide whether to insert a checkin id or NULL
        if ((RANDOM % 2)); then
            random_checkin_id=""
        fi
    
        seed_bookings "$random_booking_id" "$random_created_date" "$random_number" "$random_checkin_id" "$random_passenger_id" "$random_flight_id"
    done


    # seed_bagage
    for booking_id in "${booking_ids[@]}"; do
    # Randomly decide how many baggage items to create for this booking
    num_baggage_items=$((RANDOM % 5))  # Random number between 0 and 4

        for i in $(seq 1 $num_baggage_items); do
            random_baggage_id=$(random_string 28)  # Random baggage id of length 28
            random_weight=$((RANDOM % 50 + 1))  # Random weight between 1 and 50
    
            seed_baggage "$random_baggage_id" "$random_weight" "$booking_id"
        done
    done
}



seed_planes(){
    echo "Seeding Planes"
    psql -v ON_ERROR_STOP=1 --username "$APP_DB_USER" --dbname "$APP_DB_NAME" <<-EOSQL
    BEGIN;
    INSERT INTO planes (id, max_passengers, max_baggageTotal, max_baggageWeight, max_baggageDimension)
    VALUES ('\$1', '\$2', \$3, \$4, \$5);	

    COMMIT;
EOSQL
 
}

seed_passengers(){
    echo "Seeding Passengers"
    psql -v ON_ERROR_STOP=1 --username "$APP_DB_USER" --dbname "$APP_DB_NAME" <<-EOSQL
    BEGIN;
    INSERT INTO passengers (id, name, email, photo, passportNumber)
    VALUES ('\$1', '\$2', '\$3', '\$4', '\$5');	

    COMMIT;
EOSQL
}

seed_checkins(){
    echo "Seeding Checkins"
    psql -v ON_ERROR_STOP=1 --username "$APP_DB_USER" --dbname "$APP_DB_NAME" <<-EOSQL
    BEGIN;
    INSERT INTO checkins (id, is_checked_in)
    VALUES ('\$1', \$2);	

    COMMIT;
EOSQL
}


seed_flights(){
    echo "Seeding Flights"
    psql -v ON_ERROR_STOP=1 --username "$APP_DB_USER" --dbname "$APP_DB_NAME" <<-EOSQL
    BEGIN;
    INSERT INTO flights (id, arrival_time, departure, origin, destination, plane_id)
    VALUES ('\$1', '\$2', '\$3', '\$4', '\$5', '\$6');	

    COMMIT;
EOSQL
}

seed_bookings(){
    echo "Seeding Bookings"
    psql -v ON_ERROR_STOP=1 --username "$APP_DB_USER" --dbname "$APP_DB_NAME" <<-EOSQL
    BEGIN;
    INSERT INTO bookings (id, created_date, number, checkin_id, passenger_id, flight_id)
    VALUES ('\$1', '\$2', '\$3', '\$4', '\$5', '\$6');	
    COMMIT;

EOSQL
}

seed_baggage(){
    echo "Seeding Baggage"
    psql -v ON_ERROR_STOP=1 --username "$APP_DB_USER" --dbname "$APP_DB_NAME" <<-EOSQL
    BEGIN;
    INSERT INTO baggage (id, weight, booking_id)
    VALUES ('\$1', '\$2', '\$3');	

    COMMIT;
EOSQL
}

seed_database_tables
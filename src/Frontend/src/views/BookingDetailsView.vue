<template>
    <div class="details">
        <h1>Booking Details!</h1>
        <RouterLink to="/">Go Back</RouterLink>
        <div class="flightDetails">
            <h3>Currently booking for flight:</h3>
            <p>Origin: {{ flightInfo.origin }}</p>
            <p>Destination: {{ flightInfo.destination }}</p>
            <p>Departure: {{ flightInfo.departure }}</p>
            <p>Arrival: {{ flightInfo.arrival }}</p>
        </div>
        <div class="bookingDetails">
            <form @submit.prevent="handleSubmit">
                <label>Email:</label>
                <input type="email" v-model="email">
                <label>Passport Number</label>
                <input type="number" v-model="passportNumber">
                <label>Extra luggage (in kg)</label>
                <input type="number" step="5" v-model="addedLuggage">
                <p>The luggage's largest dimension may not exceed 150 centimeters.</p>
                <div class="submit">
                    <button>Book now!</button>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
export default {
    data () {
        return {
            flightInfo: {
                planeId: '',
                flightId: '',
                departure: '',
                arrival: '',
                origin: '',
                destination: '',
                passengersAvailableTotal: 0,
                baggageWeightAvailableTotal: 0
            },
            email: '',
            passportNumber: 0,
            addedLuggage: 0,
        }
    },
    created() {
        let flightId = this.$route.params.flightId;
        fetch(`http://127.0.0.1:40080/FlightInfo/${flightId}`)
            .then((response => response.json()))
            .then((response => {
                this.flightInfo = response;
            }))
            .catch(error => alert(error));
    }
    ,
    methods: {
        handleSubmit() {
            fetch("http://127.0.0.1:40080/Booking", {
                method: "post",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    email: this.email,
                    bookingId: '',
                    passportNumber: this.passportNumber,
                    addedLuggage: this.addedLuggage
                })
            })
            .then((response) => response.json())
            .then((response => {
                alert('Booking success! Your booking id is: ' + response.bookingId);
            }));
        }
    }
}
</script>

<style scoped>
.details {
    min-height: 50vh;
    display: flex;
    align-items: center;
}
.bookingDetails {
    display:grid
}
h1 {
    color: deeppink;
    font-weight: bold;
    font-size: xx-large;
}
</style>
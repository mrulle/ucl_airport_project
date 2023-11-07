<template>
    <div class="details">
        <div class="bookingHeader">
            <h1 style="color: hsla(160, 100%, 37%, 1);">Booking Details!</h1>
            <div class="routerLink">
                <RouterLink to="/">Go Back</RouterLink>
            </div>
            <div class="flightDetails">
                <h3>Currently booking for flight:</h3>
                <p>Origin: {{ flightInfo.flightOrigin }}</p>
                <p>Destination: {{ flightInfo.flightDestination }}</p>
                <p>Departure: {{ flightInfo.departure }}</p>
                <p>Arrival: {{ flightInfo.arrival }}</p>
            </div>
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
                flightOrigin: '',
                flightDestination: '',
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
    },
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
                    passportNumber: this.passportNumber,
                    addedLuggage: this.addedLuggage,
                    bagageId: '',
                    flightId: this.$route.params.flightId,
                    passengerId: '',
                    inputBookingId: ''
                })
            })
            .then((response) => response.json())
            .then((response => {
                alert('Booking success! Your booking id is: ' + response.inputBookingId);
            }))
            .catch((error) => alert(error));
        }
    }
}
</script>

<style>
form {
    max-width: 420px;
    margin: 30px auto;
    background-color: white;
    text-align: left;
    padding: 40px;
    border-radius: 10px;
}
label {
    color: #aaa;
    display: inline-block;
    margin: 25px 0 15px;
    font-size: 1em;
    text-transform: uppercase;
    letter-spacing: 1px;
    font-weight: bold;
}
input, select {
    display: block;
    padding: 10px, 6px;
    box-sizing: border-box;
    border: none;
    border-bottom: 1px solid #ddd;
    color: #555;
}
button {
    background: #0b6dff;
    border: 0;
    padding: 10px 20px;
    margin-top: 20px;
    color: white;
    border-radius: 20px;
}
.submit {
    text-align: center;
}
</style>
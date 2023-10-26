<template>
    <div class="details">
        <h1>Booking Details!</h1>
        <RouterLink to="/">Go Back</RouterLink>
        <div class="flightDetails">
            <h3>Currently booking for flight:</h3>
            <p>Origin: {{ flightInfo.origin }}</p>
            <p>Destination: {{ flightInfo.destination }}</p>
            <p>Departure: {{ flightInfo.departure }}</p>
            <p>Duration: {{ flightInfo.duration }}</p>
        </div>
        <div class="bookingDetails">
            <form @submit.prevent="handleSubmit">
                <label>Email:</label>
                <input type="email">
                <label>Passport Number</label>
                <input type="number">
                <label>Extra luggage (in kg)</label>
                <input type="number" step="5" value="0">
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
                origin: 'copenhagen',
                destination: 'krakow',
                departure: '06:30',
                duration: '1:30',
                id: ''
            }
        }
    },
    created() {
        fetch('http://127.0.0.1:40080/FlightInfo')
            .then((response => response.json()))
            .then((response => {
                this.flightInfo.value = response;
            }))
            .catch(error => alert(error));
    }
    ,
    methods: {
        handleSubmit() {
            // TODO:
            // fetch-api with post request to booking api
            // containing information from the form
            alert('FORM SUBMITTED!');
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
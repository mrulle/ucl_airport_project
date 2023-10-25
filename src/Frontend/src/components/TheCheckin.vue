<template>
    <div class="checkin">
        <h1>Checkin</h1>
        <RouterLink to="/">Go Back</RouterLink>
        <div class="checkinDetails">
            <form @submit.prevent="handleSubmit">
                <label>Email:</label>
                <input type="email" required v-model="email">
                <label>Booking Id</label>
                <input type="text" required v-model="bookingId">
                <div class="submit">
                    <button>Check-In now!</button>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
export default {
    data () {
        return {
            email: '',
            bookingId: ''
        }
    },
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
.checkinDetails {
    display:grid;
}
.checkin {
    min-height: 50vh;
    display: flex;
    align-items: center;
}
h1 {
  color: deeppink;
  font-weight: bold;
  font-size: xx-large;
}
</style>

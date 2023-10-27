<template>
    <div class="checkin">
        <h1 style="color: hsla(160, 100%, 37%, 1);">Check-In</h1>
        <div class="routerLink">
            <RouterLink to="/">Go Back</RouterLink>
        </div>
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
            fetch("http://127.0.0.1:40080/Checkin", {
                method: "post",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    checkinId: '',
                    email: this.email,
                    bookingId: this.bookingId,
                })
            })
            .then((response) => response.json())
            .then((response => {
                alert('Check-In Success! Your boarding pass id: ' + response.checkinId + ' Your passenger id: ' + response.passengerId + ' Your flightId: ' + response.flightId);
            }))
            .catch((error) => {alert(error)});
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
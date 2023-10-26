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
                    email: this.email,
                    bookingId: this.bookingId,
                })
            })
            .then( (response) => {
                console.log(response);
                alert('Check-In success!');
            });
        }
    }
}
</script>

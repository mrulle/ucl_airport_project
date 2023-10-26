<template>
<div class="booking">
  <h1>Hello from TheBooking.vue</h1>
  <li v-for="(flight, index) in flights" v-bind:key="index">
    <p>{{ flight.origin }}</p>
    <p>{{ flight.destination }}</p>
    <p>{{ flight.departure }}</p>
    <p>{{ flight.arrival }}</p>
    <RouterLink :to="{ name: 'details', params: { flightId: flight.flightId }}">{{ flight.flightId }}</RouterLink>
  </li>
</div> 
</template>

<script>
export default {
  data() {
    return {
      flights: []
    }
  },
  created() {
    fetch('http://127.0.0.1:40080/FlightInfo')
      .then((response) => response.json())
      .then((response) => {
        this.flights = response;
      })
      .catch((error) => alert(error));
  }
}
</script>

<style scoped>
.booking {
    min-height: 50vh;
    display: flex;
    align-items: center;
  }
h1 {
  color: deeppink;
  font-weight: bold;
  font-size: xx-large;
}
li {
  padding: 20px;
}
</style>

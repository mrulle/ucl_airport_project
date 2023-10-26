<template>
<div class="booking">
  <div class="flightHeader">
    <h1 style="color: hsla(160, 100%, 37%, 1);">Available Flights</h1>
  </div>
  <li v-for="(flight, index) in flights" v-bind:key="index">
    <div class="bookingItem">
      <div class="flightHeader">
        <h3>Flight</h3>
      </div>
      <div class="flightLogo">
        <img src="../assets/monkey-plane.png" width="128" height="128">
      </div>
      <p><span class="flightSpan">Origin:</span> {{ flight.origin }}</p>
      <p><span class="flightSpan">Destination:</span> {{ flight.destination }}</p>
      <p><span class="flightSpan">Departure:</span> {{ flight.departure }}</p>
      <p><span class="flightSpan">Arrival:</span> {{ flight.arrival }}</p>
      <div class="routerLink">
        <RouterLink :to="{ name: 'details', params: { flightId: flight.flightId }}">Book now!</RouterLink>
      </div>
    </div>
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
  max-width: 420px;
  margin: 30px auto;
}
.bookingItem {
  background-color: white;
  padding: 40px;
  text-align: left;
  border-radius: 10px;
}
h3 {
  color: #666;
  display: inline-block;
  margin: 20px 0 30px;
  font-size: large;
  text-transform: uppercase;
  letter-spacing: 2px;
  font-weight: bold;
}
p {
  font-weight: bold;
  letter-spacing: 2px;
  text-transform: uppercase;
  color: #555;
}
.flightHeader {
  text-align: center;
}
.flightLogo {
  text-align: center;
  padding-bottom: 20px;
}
.flightSpan {
  color: hsla(160, 100%, 37%, 1);
}
</style>

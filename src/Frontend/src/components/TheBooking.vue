<template>
<div class="booking">
  <h1>Hello from TheBooking.vue</h1>
  <li v-for="(flight, index) in flights" v-bind:key="index">
    <p>{{ flight.origin }}</p>
    <p>{{ flight.destination }}</p>
    <p>{{ flight.departure }}</p>
    <p>{{ flight.duration }}</p>
    <RouterLink :to="{ name: 'details', params: { flightId: flight.id }}">{{ flight.id }}</RouterLink>
  </li>
</div> 
</template>

<script>
export default {
  data() {
    return {
      flights: 
      [{
        origin: 'copenhagen', 
        destination: 'london', 
        departure: '06:30', 
        duration: '1:30',
        id: '123'
      },
      {
        origin: 'bangkok', 
        destination: 'krakow', 
        departure: '18:30', 
        duration: '6:00',
        id: '456'
      }]
    }
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

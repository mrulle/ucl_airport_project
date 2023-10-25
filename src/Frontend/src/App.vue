<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header>
    <div>
      <h1>Flight Frontend Application</h1>

      <nav>
        <li>
          <RouterLink to="/checkin">Check-In</RouterLink>
        </li>
        <li>
          <RouterLink to="/">Booking</RouterLink>
        </li>
      </nav>
    </div>
  </header>

  <RouterView />
</template>

<style scoped>
h1 {
  color: deeppink;
  font-weight: bold;
  font-size: xx-large;
}
li {
  display: inline;
  margin-right: 10px;
}
</style>

<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header>
    <div class="headline">
      <h1 style="color: hsla(160, 100%, 37%, 1);">Monkey Airlines</h1>
      <img src="./assets/monkey-airlines.jpg" width="256" height="256">

      <nav>
        <li>
          <div class="routerLink main-router">
            <RouterLink to="/checkin">Check-In</RouterLink>
          </div>
        </li>
        <li>
          <div class="routerLink main-router">
            <RouterLink to="/">Booking</RouterLink>
          </div>
        </li>
        <li>
          <div class="routerLink main-router">
            <RouterLink to="/cancel">Cancel Booking</RouterLink>
          </div>
        </li>
      </nav>
    </div>
  </header>

  <RouterView />
</template>

<style>
li {
  display: inline;
  margin-right: 10px;
}
#app {
  font-family: Arial, Helvetica, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
  margin-top: 60px;
}
body {
  margin: 0;
  background: rgb(225, 223, 223);
}
.routerLink {
  font-weight: bold;
  letter-spacing: 2px;
  text-transform: uppercase;
  font-size: large;
  padding-top: 0px;
}
.headline {
  padding: 30px;
  padding-right: 50px;
  text-align: center;
  border-radius: 10px;
}
.main-router {
  font-size: x-large;
}
</style>

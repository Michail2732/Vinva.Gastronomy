<script setup>
import recipeCard from './components/recipeCard.vue';
import recipes from './assets/testDatas'
</script>

<template>
  <
  <recipeCard v-bind:recipe="recipes[0]"/>
  <recipeCard v-bind:recipe="recipes[1]"/>
  <recipeCard v-bind:recipe="recipes[2]"/>
</template>

<style scoped></style>

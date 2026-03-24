<script setup lang="ts">
import {client} from '@/api/client.gen'
import {useAuthStore} from '@/modules/auth/stores/authStore'



client.setConfig({
  baseUrl: import.meta.env.VITE_API_URL,
  auth: () => {    
    const store = useAuthStore();
    const token = store.token;     
    return token ? `Bearer ${token}` : undefined;
  }
});
</script>

<template>
   <router-view />
</template>

<style scoped></style>

<template>
     <div class="auth-layout">
    <!-- Фоновое изображение с градиентом -->
    <div class="auth-background"></div>
    
    <!-- Контент -->
    <div class="auth-container">
      <!-- Логотип -->
      <div class="auth-logo">
        <router-link to="/" class="logo-link">
          <span class="logo-icon">🍳</span>
          <span class="logo-text">Кулинарный клуб</span>
        </router-link>
      </div>
      
      <!-- Карточка с формой -->
      <div class="auth-card">
        <router-view class="card-content"/>
      </div>
      
      <!-- Ссылка на главную -->
      <div class="auth-footer">
        <router-link to="/" class="back-link">
          <i class="pi pi-home"></i>
          Вернуться на главную
        </router-link>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
export default {
    
}
</script>
<style lang="scss">
    .auth-layout {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  background: linear-gradient(135deg, #f97316 0%, #fbbf24 100%);
}

.auth-background {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-image: url('data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100"><circle cx="20" cy="20" r="15" fill="rgba(255,255,255,0.1)"/><circle cx="80" cy="30" r="25" fill="rgba(255,255,255,0.05)"/><circle cx="50" cy="80" r="30" fill="rgba(255,255,255,0.08)"/><circle cx="90" cy="85" r="12" fill="rgba(255,255,255,0.1)"/></svg>');
  background-repeat: repeat;
  opacity: 0.3;
}

.auth-container {
  width: 100%;
  max-width: 450px;
  padding: 1.5rem;
  position: relative;
  z-index: 1;
}

.auth-logo {
  text-align: center;
  margin-bottom: 2rem;
  
  .logo-link {
    display: inline-flex;
    align-items: center;
    gap: 0.5rem;
    text-decoration: none;
    color: white;
    font-size: 1.5rem;
    font-weight: bold;
    
    .logo-icon {
      font-size: 2rem;
    }
    
    .logo-text {
      letter-spacing: -0.5px;
    }
  }
}

.auth-card {
  background: white;
  border-radius: 1rem;
  padding: 2rem;
  box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04);
  .card-content
  {
    margin: 0 auto;
  }
}

.auth-footer {
  text-align: center;
  margin-top: 1.5rem;
  
  .back-link {
    display: inline-flex;
    align-items: center;
    gap: 0.5rem;
    color: white;
    text-decoration: none;
    font-size: 0.875rem;
    transition: opacity 0.2s;
    
    &:hover {
      opacity: 0.8;
    }
    
    i {
      font-size: 0.875rem;
    }
  }
}

@media (max-width: 640px) {
  .auth-container {
    padding: 1rem;
  }
  
  .auth-card {
    padding: 1.5rem;
  }
  
  .auth-logo .logo-link {
    font-size: 1.25rem;
    
    .logo-icon {
      font-size: 1.5rem;
    }
  }
}
</style>
<script setup lang="ts">
import { RecipeDto } from '@/api/generated'; '@/api/generated/models/RecipeDto'

defineProps<RecipeDto>()
defineEmits(['icon-click'])
</script>

<template >
    <div class="recipe-card">    
        <div class="recipe-image">
            <img  :src="photoId" 
                  :alt="name"
                  class="img-fluid">
        </div>        
        <div class="recipe-footer">
            <span class="recipe-title">{{ name }}</span>
            <button class="recipe-icon-btn" @click="$emit('icon-click', name)">
                <svg width="20" height="20" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M12 5V19M5 12H19" stroke="currentColor" stroke-width="2" stroke-linecap="round"/>
                </svg>
            </button>
        </div>
    </div>
</template>

<style lang="scss" scoped>

   .recipe-card {
  background: white;
  border-radius: 12px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  transition: transform 0.2s;
  
  &:hover {
    transform: translateY(-4px);
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  }
}

.recipe-image {
  width: 100%;
  aspect-ratio: 1 / 1;
  overflow: hidden;
  
  img {
    width: 100%;
    height: 100%;
    object-fit: cover;
    transition: transform 0.3s;
    
    .recipe-card:hover & {
      transform: scale(1.05);
    }
  }
}

.recipe-footer {
  padding: 12px 16px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  background: white;
}

.recipe-title {
  font-weight: 500;
  color: #333;
  font-size: 16px;
  line-height: 1.4;
}

.recipe-icon-btn {
  width: 32px;
  height: 32px;
  border: none;
  background: #f5f5f5;
  border-radius: 8px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #666;
  transition: all 0.2s;
  
  &:hover {
    background: #e0e0e0;
    color: #333;
  }
  
  svg {
    width: 18px;
    height: 18px;
  }
}
</style>
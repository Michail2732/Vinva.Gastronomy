<template>
  <div class="toggle-btn-container">
    <ToggleButton
    :model-value="modelValue"
    :on-icon="onIcon"
    :off-icon="offIcon"
    :aria-label="ariaLabel || 'Toggle'"
    :disabled="disabled"
    :size="size"    
    :pt="ptOptions"
    @update:model-value="handleChange"
    v-bind="$attrs"
  />
  </div>  
</template>

<script setup lang="ts">
import { computed } from 'vue';
import ToggleButton from 'primevue/togglebutton';

// ==================== Props ====================
interface Props {  
  modelValue?: boolean;  
  onIcon: string;  
  offIcon: string;  
  ariaLabel?: string;  
  disabled?: boolean;  
  size?: 'small' | 'large';  
  buttonSize?: string;
}

const props = withDefaults(defineProps<Props>(), {
  modelValue: false,
  ariaLabel: 'Toggle',
  disabled: false,
  buttonSize: '2.5rem',
});

// ==================== Emits ====================
interface Emits {  
  (e: 'update:modelValue', value: boolean): void;  
  (e: 'change', value: boolean): void;
}

const emit = defineEmits<Emits>();

function handleChange(value: boolean) {
  emit('update:modelValue', value);
  emit('change', value);
}


const ptOptions = computed(() => ({
  root: {
    style: {
      width: props.buttonSize,
      height: props.buttonSize,
      padding: '0',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
    },
  },
  label: {
    style: {
      display: 'none',
    },
  },
}));
</script>


<style scoped lang="scss">
.toggle-btn-container
{
  &:deep(.p-togglebutton-checked)
  {
      background-color: var(--p-surface-400);
      border-color: var(--p-surface-400);                
      .p-togglebutton-content
      {                                        
          background: none;                    
      }
  }
}
</style>
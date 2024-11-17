<template>
  <span
    class=" w-full outline-none relative"
    :class="className"
  >
    <InputText
      class="pr-7  p-3 border-round-lg search-field"
      :model-value="modelValue"
      @update:model-value="emit('update:model-value', $event)"
      @change="emit('change')"
      style="border-radius: 12px !important;"
    />
    <span class="search-field__inner-icon">
      <svg xmlns="http://www.w3.org/2000/svg" width="18" height="19" viewBox="0 0 15 16" fill="none">
        <path d="M14 14.375L11.5209 11.8958M13.2917 7.64583C13.2917 10.971 10.596 13.6667 7.27083 13.6667C3.94562 13.6667 1.25 10.971 1.25 7.64583C1.25 4.32062 3.94562 1.625 7.27083 1.625C10.596 1.625 13.2917 4.32062 13.2917 7.64583Z" stroke="white" stroke-width="1.41667" stroke-linecap="round" stroke-linejoin="round"/>
      </svg>
    </span>
  </span>
</template>

<script lang="ts" setup>
const {} = withDefaults( defineProps<{modelValue:string, className?:string}>(),{
  className:""
});
const emit = defineEmits(["update:model-value","change"]);
</script>

<script lang="ts">
import { defineComponent } from "vue";
export default defineComponent({
  name: "search-field",
  inheritAttrs: false,
  customOptions: {},
});
</script>

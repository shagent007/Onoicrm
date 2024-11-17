<script setup lang="ts">
const {arg} = defineProps<{arg:any}>();
</script>
<template>
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width="12"
    height="13"
    viewBox="0 0 15 16"
    fill="none"
    class="mr-1"
  >
    <path
      :style="`stroke:${arg.event.extendedProps?.serviceGroup?.textColor};`"
      d="M7.5 4.25V8L10 9.25M13.75 8C13.75 11.4518 10.9518 14.25 7.5 14.25C4.04822 14.25 1.25 11.4518 1.25 8C1.25 4.54822 4.04822 1.75 7.5 1.75C10.9518 1.75 13.75 4.54822 13.75 8Z"
      stroke="#FE8084"
      stroke-width="1.25"
      stroke-linecap="round"
      stroke-linejoin="round"
    />
  </svg>
</template>


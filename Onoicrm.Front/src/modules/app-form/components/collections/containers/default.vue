<template>
  <div class="flex" v-bind='attrs'>
    <slot></slot>
  </div>
</template>

<script lang='ts' setup>
const {} = defineProps<{attrs?:any}>();
</script>

<script lang="ts">
import { defineComponent } from "vue";
export default defineComponent({
  name: "collection-default-container",
  inheritAttrs: false,
  customOptions: {},
});
</script>

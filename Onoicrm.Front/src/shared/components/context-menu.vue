<template>
  <slot :toggle="toggle"/>
  <TiredMenu :popup="true" ref="menu" :model="items" />
</template>

<script lang="ts" setup>
import TiredMenu from "primevue/tieredmenu";
import { ref } from 'vue';
const menu = ref();
const toggle = (event:Event) => {
  menu.value.show(event);
};

defineProps<{items:any[]}>()
</script>

<script lang="ts">
import { defineComponent } from "vue";
export default defineComponent({
  name: "context-menu",
  inheritAttrs: false,
  customOptions: {},
});
</script>

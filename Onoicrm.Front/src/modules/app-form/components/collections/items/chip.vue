<template>
  <chip
    :label='config.getCaption(data)'
    @click='emit("remove")'
    :icon='config?.getIcon?.(data) ?? "pi pi-trash"'
    v-bind='item.attrs'
  />
</template>

<script lang='ts' setup>
import Chip from 'primevue/chip';
import { CollectionChipItem } from './models/CollectionChipItem';
const {config}=  defineProps<{data:any, config:Record<string, (data:any)=>any>, item:CollectionChipItem}>();
const emit = defineEmits<{(e:"remove"):void}>()
</script>

<script lang="ts">
import { defineComponent } from "vue";
export default defineComponent({
  name: "collection-item-chip",
  inheritAttrs: false,
  customOptions: {},
});
</script>

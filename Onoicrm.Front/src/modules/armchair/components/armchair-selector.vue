<template>
  <div class="flex ">
    <div @click="update(item)" v-for="item in armchairStore.items" :key="item.id" class="text-center  mr-4">
      <div :class="{'card--active':item.id==modelValue}" class="card card--shadow mb-3" style="height:125px; width:90px;"></div>
      <div>{{item.caption}}</div>
    </div>
  </div>
</template>

<script setup lang="ts">
import {useArmchairStore} from "~/modules/armchair";

defineProps<{modelValue:number|null}>();

const emit = defineEmits(["update:modelValue"])
const update = (item:any) => {
  emit("update:modelValue", item.id)
}

const armchairStore = useArmchairStore();
</script>

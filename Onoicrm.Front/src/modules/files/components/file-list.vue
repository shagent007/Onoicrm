<template>
  <div v-for="item in items" :key="item.id" :class="cssClass">
    <slot :item="item">
      <div class="flex align-items-center">
        <div class="w-7rem h-5rem mr-2 border-round-sm">
          <img
            class="w-full h-full border-round-sm"
            style="object-fit:cover; object-position:center;"
            :src="getSrc?.(item)"
            :alt="item.alt"
          />
        </div>
        <div>
          <h4 class="text-xs md:text-base">{{ item.name }}</h4>
          <p class="text-600 text-xs md:text-base">{{item.size}}</p>
        </div>
      </div>
      <div class="flex align-items-center">
        <i
          class="pi pi-pencil cursor-pointer text-primary pr-3"
          style="font-size: 16px"
        />
        <i
          class="pi pi-trash cursor-pointer"
          style="color: red;
          font-size: 18px"
          @click="emit('remove',item)"
        />
      </div>
    </slot>
  </div>
</template>

<script setup lang="ts">
withDefaults(defineProps<{
  items:any[],
  getSrc?:(item:any)=>string,
  cssClass?:string
}>(),{
  getSrc:(item:any) => `/api/v1/file/${item.id}`,
});
const emit = defineEmits(["remove","edit"]);
</script>


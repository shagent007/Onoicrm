<template>
  <Galleria
    v-model:activeIndex="activeIndex"
    v-model:visible="displayCustom"
    :value="items"
    :numVisible="7"
    containerStyle="max-width: 850px"
    :circular="true"
    :fullScreen="true"
    :showItemNavigators="true"
    :showThumbnails="false"
  >
    <template #item="slotProps">
      <slot name="image" :image="slotProps">
        <img
          :src="slotProps.item.url"
          :alt="slotProps.item.name"
          style="width: 100%; display: block"
        />
      </slot>
    </template>
    <template #thumbnail="slotProps">
      <img
        :src="slotProps.item.url"
        :alt="slotProps.item.name"
        style="display: block"
      />
    </template>
  </Galleria>
</template>

<script setup lang="ts">
import { IObjectDataSource } from "~/modules/data-sources/interfaces/IObjectDataSource";
import { computed, ref } from "vue";

interface PropTypes {
  dataSource?: IObjectDataSource;
  albumName?: string;
  customItems?: any[];
}

const activeIndex = ref(0);
const displayCustom = ref(false);
const select = (index: number) => {
  activeIndex.value = index;
  displayCustom.value = true;
};

const { dataSource, albumName, customItems } = withDefaults(
  defineProps<PropTypes>(),
  {
    customItems: () => [],
  },
);

defineExpose({ select });
const items = computed(() => {
  if (customItems.length > 0) return customItems;
  if (!dataSource) return [];
  return dataSource.model.value.imageAlbums
    .find((ia: any) => ia.name == albumName)
    ?.images?.sort?.((p: any, n: any) => p.priority - n.priority)
    ?.map?.((i: any) => ({
      ...i,
      url: `/api/v1/objectimage/p${i.id}.jpg`,
    }));
});
</script>

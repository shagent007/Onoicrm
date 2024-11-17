<template>
  <Dialog v-model:visible="visible" modal :style="{ width: '50vw' }" class="dialog reminder-tooth">
    <template #header>
      <div class="flex justify-content-end"></div>
    </template>
    <div class="grid min-height__dialog">
      <div class="col-12 md:col-6">
        <div class="flex flex-column justify-content-evenly h-full">
          <p
              class="card--default block  mb-3 w-full outline-none border-none p-3 dialog__title"
          >{{dataSource.model.value.caption}}</p>
          <p
              class="card--default w-full outline-none border-none p-3 h-full dialog__subtitle">
            {{dataSource.model.value.description}}
          </p>
        </div>
      </div>
      <div class="col-12 md:col-6">
        <div class="card--default p-3 h-full">
          <div class="grid">
            <file-list
                :items="dataSource.model.value.files"
                v-slot="{item}"
                css-class="col-6"
            >
              <img style="object-fit:cover" :src="`/api/v1/file/${item.id}`" class="w-full h-5rem border-round-xl" alt="">
            </file-list>
          </div>
        </div>
      </div>
    </div>
  </Dialog>
</template>

<script setup lang="ts">
import {computed, ref} from "vue";
import {useToothStore} from "~/modules/tooth";
import {ObjectDataSourceConfig, useObjectDataSource} from "~/modules/data-sources";
import FileList from "~/modules/files/components/file-list.vue";

const resolve = ref<Function>();
const toothId = ref<number>();
const visible = ref<boolean>();
const toothStore = useToothStore();
const tooth = computed(() => {
  if(!toothId.value) return;
  return  toothStore.findItem(toothId.value)
})
const dataSource = useObjectDataSource(
  new ObjectDataSourceConfig({
    className:"BookingTooth"
  })
)


const open = async (bookingToothId:number) => {
  dataSource.config.value.id = bookingToothId;
  await dataSource.get();
  visible.value = true;
  return new Promise(r => resolve.value = r);
}


defineExpose({open})
</script>

<template>
  <Dialog
    :visible="visible"
    @update:visible="close()"
    modal
    :style="{ width: '80vw' }"
    class="dialog"
  >
    <template #header>
      <div class="flex justify-content-end"></div>
    </template>
    <div class="grid align-items-stretch">
      <div class="col-12 md:col-6 xl:col-3">
        <div
          class="card--default p-3 flex flex-column align-items-center h-full"
        >
          <h3 class="align-self-start mb-4 dialog__title">Изменения</h3>
          <div class="mb-4">
            <upper-jaw  :states="states" />
          </div>

          <div>
            <lower-jaw :states="states" />
          </div>
        </div>
      </div>

      <div class="col-12 md:col-6 xl:col-3">
        <div class="flex flex-column justify-content-evenly h-full">
          <p
            class="card--default block mb-3 w-full outline-none border-none p-3 dialog__title"
          >
            {{ dataSource.model.value.caption }}
          </p>
          <p
            class="card--default w-full outline-none border-none p-3 overflow-auto min-h-17rem h-14rem md:h-full dialog__subtitle"
          >
            {{ dataSource.model.value.description }}
          </p>
        </div>
      </div>
      <div class="col-12 md:col-6 xl:col-3">
        <div class="card--default p-3 h-full">
          <div class="grid">
            <file-list
              :items="dataSource.model.value.files"
              v-slot="{ item }"
              css-class="col-6"
            >
              <img
                style="object-fit: cover"
                :src="`/api/v1/file/${item?.id}`"
                class="w-full h-5rem border-round-xl"
                alt=""
              />
            </file-list>
          </div>
        </div>
      </div>
      <div class="col-12 md:col-6 xl:col-3">
        <div class="flex flex-column justify-content-evenly h-full">
          <div class="card--default p-3 h-full mb-3">
            <h3 class="mb-3 dialog__title">Услуги и цены</h3>
            <div
              class="flex justify-content-between mb-2 border-bottom-1 pb-1"
              v-for="item in dataSource.model.value.prices"
            >
              <h4 class="dialog__title text-xs">{{ item.caption }}</h4>
              <p class="dialog__subtitle">{{ item.value }}</p>
            </div>
          </div>
          <div
            class="card--default h-3rem px-2 flex justify-content-between align-items-center"
          >
            <p class="dialog__title">Итого:</p>
            <h3 class="dialog__title">
              {{
                dataSource?.model.value.prices.reduce(
                  (a, b: any) => a + Number(b.value),
                  0,
                )
              }}
              с
            </h3>
          </div>
        </div>
      </div>
    </div>
  </Dialog>

  <Administrator-tooth-dialog
    v-if="dataSource.model.value"
    ref="AdministratorToothDialog"
    :patient="dataSource.model.value.patient"
  />
</template>
<script setup lang="ts">
import { computed, ref, inject } from "vue";
import { useToothStore } from "~/modules/tooth";
import { defaultMessage, IMessage } from "~/shared";
import {
  ObjectDataSourceConfig,
  useObjectDataSource,
} from "~/modules/data-sources";
import FileList from "~/modules/files/components/file-list.vue";
import UpperJaw from "~/modules/tooth/components/upper-jaw.vue";
import LowerJaw from "~/modules/tooth/components/lower-jaw.vue";
import {
  ToothStateModel,
} from "~/modules/tooth/models/ToothStateModel";

const visible = ref(false);
const resolve = ref();
const message = inject<IMessage>("message", defaultMessage);
const dataSource = useObjectDataSource(
  new ObjectDataSourceConfig({
    className: "Booking",
  }),
);
const toothStore = useToothStore();
const states = computed(() =>
  dataSource.model.value.userProfileTeeth.map(
    (upt: any) =>
      new ToothStateModel({
        position: toothStore.findItem(upt.toothId).position,
        color: "blue",
      }),
  ),
);


const show = async (booking?: any) => {
  dataSource.config.value.id = booking.id;
  await dataSource.get();
  visible.value = true;
  return new Promise((r) => (resolve.value = r));
};

defineExpose({ show });

const close = () => {
  visible.value = false;
  resolve.value();
};
</script>

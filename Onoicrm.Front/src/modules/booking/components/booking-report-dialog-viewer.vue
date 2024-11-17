<template>
  <Dialog
    :visible="visible"
    @update:visible="(value) => (!value ? reset() : {})"
    modal
    class="booking-report-dialog"
    :style="{ width: '50vw', background: '#F5F6F8' }"
  >
    <template #header>
      <div class="flex justify-content-between align-items-center flex-1">
        <div>
          <div class="text-xl font-semibold">Проделанные работы</div>
          <p>{{ moment().format("DD.MM.YYYY") }}</p>
        </div>

        <div class="tooth-buttons">
          <Button
            class="mr-2"
            :outlined="selectedBookingTooth !== null"
            @click="selectedBookingTooth = null"
            label="Все"
          />
          <Button
            v-for="bt in bookingTeeth"
            :key="bt.id"
            class="mr-2"
            @click="selectedBookingTooth = bt"
            :outlined="selectedBookingTooth?.toothId != bt.toothId"
            :label="toothStore.findItem(bt.toothId)?.position?.toString()"
          />
        </div>
      </div>
    </template>
    <div class="grid">
      <div class="col-12">
        <div style="height: 200px" class="card block p-3">
          <div class="scrollable--200">
            <DataTable
              scrollable
              scrollHeight="250px"
              class="p-datatable-sm p-datatable-hoverable-rows"
              :value="implementedServices"
            >
              <Column style="width: 60px" field="code" header="Код">
                <template #body="{ data }">{{ data?.code }}</template>
              </Column>
              <Column field="caption" header="Наименование">
                <template #body="{ data }">
                  <div class="short">{{ data?.caption }}</div>
                </template>
              </Column>
              <Column field="price" header="Цена">
                <template #body="{ data }">
                  <div>{{ data?.price }}</div>
                </template>
              </Column>
              <Column field="count" header="Количество" />

              <Column header="Cумма">
                <template #body="{ data }">
                  <div>{{ data?.price * data?.count }}</div>
                </template>
              </Column>


              <Column header="Cумма">
                <template #body="{ data }">
                  <div>{{ calculateTotal(data) }}</div>
                </template>
              </Column>
            </DataTable>
          </div>
        </div>
      </div>
      <div class="col-12">
        <div class="flex align-items-center justify-content-end">
          <div class="flex mr-5">
            <div class="mr-1">Сумма:</div>
            <div>{{ serviceSum }} c</div>
          </div>
          <div class="flex mr-5">
            <div class="mr-1">Скидка:</div>
            <div>{{ discountSum }} c</div>
          </div>
          <div class="flex">
            <div class="mr-1">Итого:</div>
            <div style="font-weight: 500">{{ resultSum }} c</div>
          </div>
        </div>
      </div>
      <div v-if="selectedBookingTooth" :class="files.length > 0 ? 'col-5' : 'col-12'" class=" pb-0 h-full">
        <div style="height: 250px" class="card block p-3">
          <h4 class="mb-0">Каналы</h4>
          <divider style="margin: 12px 0" />
          <div v-if="selectedBookingTooth" class="scrollable--200">
            <div
              v-for="channel in selectedBookingTooth.channels"
              :key="channel.channelId"
              class="channel mb-4"
            >
              <div class="channel__caption">
                {{ getChannelCaption(channel) }}
              </div>
              <div class="channel__card">
                <div class="channel-item">
                  <div class="channel-item__caption">МК:</div>
                  <InputNumber
                    disabled
                    v-model="channel.masterCon"
                    suffix="мм"
                  />
                </div>
                <div class="channel-item">
                  <div class="channel-item__caption">МФ:</div>
                  <InputNumber
                    disabled
                    v-model="channel.masterFile"
                    suffix="мм"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div v-if="files.length > 0" :class="selectedBookingTooth ? 'col-7' : 'col-12'"  class="pb-0 h-full">
        <div style="height: 250px; overflow: auto" class="card block p-3">
          <div class="flex align-items-center justify-content-between">
            <h4 class="mb-0">Фотографии</h4>
          </div>
          <divider style="margin-top: 0.75rem" />
          <div
            style="height: 160px"
            :class="{ 'scrollable--200': files.length > 0 }"
          >
            <div class="grid">
              <file-list css-class="col-6" v-slot="{ item }" :items="files">
                <img
                  class="w-full border-round-sm cursor-pointer"
                  style="
                    object-fit: cover;
                    object-position: center;
                    aspect-ratio: 16/9;
                  "
                  :src="getImageSrc(item)"
                  :alt="item.alt"
                />
              </file-list>
            </div>
          </div>
        </div>
      </div>
    </div>
    <template #footer>
      <div></div>
    </template>
  </Dialog>
</template>

<script setup lang="ts">
import { inject, ref } from "vue";
import { useClinicStore } from "~/modules/clinic/stores/useClinicStore";
import { computed } from "vue";
import { useToothStore } from "~/modules/tooth";
import moment from "moment";
import { Booking } from "~/modules/booking";
import { DiscountType } from "~/modules/booking/entities/Booking";
import FileList from "~/modules/files/components/file-list.vue";
import { defaultConfirm, defaultMessage, IMessage, TConfirm } from "~/shared";
import { FilterMatchMode } from "primevue/api";
import _ from "lodash";

const toothStore = useToothStore();
const clinicStore = useClinicStore();
const message = inject<IMessage>("message", defaultMessage);
const confirm = inject<TConfirm>("confirm", defaultConfirm);
const booking = ref<Booking | any>();
const selectedBookingTooth = ref<any>(null);
const useLabaratory = ref(false)

const loaded = ref<boolean>(false);
const visible = ref<boolean>(false);
const resolve = ref<(model: any) => any>();
const filters = ref({
  global: { value: null, matchMode: FilterMatchMode.CONTAINS },
});

const bookingTeeth = computed(() => booking.value?.bookingTeeth);

const calculateTotal = (data: any) => {
  let labaratoryContribution = 0;
  if (useLabaratory.value) {
    labaratoryContribution = data?.labaratoryPrice * data?.count || 0;
  }

  const actualItemPrice = (data?.price * data?.count) - labaratoryContribution;
  const salaryDiscount = (100 - data?.salary) / 100;
  return actualItemPrice * salaryDiscount;
};


const implementedServices = computed(() => {
  if (selectedBookingTooth.value !== null)
    return selectedBookingTooth?.value?.implementedServices ?? [];

  const services = bookingTeeth.value
      .map((bt: any) => bt.implementedServices)
      .flat();
  const commonServices =
      booking.value.implementedServices.filter(
          (is: any) => is.bookingToothId == null,
      ) ?? [];

  return services.concat(commonServices);
});

const files = computed(() => {
  if (selectedBookingTooth.value !== null) {
    return selectedBookingTooth?.value?.files;
  }

  return bookingTeeth.value.map((bt: any) => bt.files).flat();
});
const serviceSum = computed(() => {
  const impServices = bookingTeeth.value
      .map((bt: any) => bt.implementedServices)
      .flat();

  return booking.value.implementedServices
      .filter((ims:any) => !impServices.some((is:any) => is.id == ims.id))
      .concat(impServices)
      .map((is: any) => is.price * is.count)
      .reduce((a: number, b: number) => a + b, 0);
});
const discountSum = computed(() => {
  const discount = booking.value?.discount;
  switch (booking.value.discountType) {
    case DiscountType.Money:
      return discount;
    case DiscountType.Percent:
      return discount ? (serviceSum.value * discount) / 100 : 0;
  }
});
const resultSum = computed(() => serviceSum.value - discountSum.value);

const getChannelCaption = (bc: any) => {
  const tooth = toothStore.findItem(selectedBookingTooth.value.toothId);
  if (!tooth) return "";
  const channel = tooth.channels.find((c: any) => c.id == bc.channelId);
  if (!channel) return "";

  return channel.caption;
};
const getImageSrc = (item: any) => {
  if (!(typeof item.id == "number"))
    return `data:image/png;base64,${item.base64}`;
  return `/api/v1/file/${item.id}`;
};

const open = async (model: Booking) => {
  booking.value = _.cloneDeep(model);
  visible.value = true;
  await toothStore.getData()
  return new Promise((r) => (resolve.value = r));
};
const reset = () => {
  visible.value = false;
  booking.value = null;
  selectedBookingTooth.value = null;
  resolve.value!(null);
};
const submit = async () => {
  visible.value = false;

  resolve.value!(null);

  booking.value = null;
  selectedBookingTooth.value = null;
  visible.value = false;
};

defineExpose({ open });
</script>

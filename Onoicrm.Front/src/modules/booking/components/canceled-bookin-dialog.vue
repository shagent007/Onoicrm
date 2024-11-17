<template>
  <Dialog v-model:visible="visible" modal header="Отмененные записи" scrol :style="{ width: '50rem' }" :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">
    <DataTable
      :value="canceledBookingDataSource.items.value"
      paginator
      :rows="5"
      :rowsPerPageOptions="[5, 10, 20, 50]"
      scrollable
      scroll-height="400px"
    >
      <Column field="date" header="Дата">
        <template #body="{data}">
          {{moment(data?.createDate).format("DD.MM.YYYY")}}
        </template>
      </Column>
      <Column field="fullName" header="Пациент">
        <template #body="{data}">
          {{data?.patient.fullName}}
        </template>
      </Column>


      <Column field="informationSource" header="Причина">

        <template #body="{data}">
          <div class="flex justify-content-end w-full">
            {{getReason(data)}}
          </div>
        </template>
      </Column>
    </DataTable>
  </Dialog>
</template>
<script setup lang="ts">
import {IListDataSource} from "~/modules/data-sources";
import moment from "moment"
import {ref} from "vue";
import {IBookingListDataSource} from "~/modules/booking/interfaces/IBookingListDataSource";
import {useCancellationReasonStore} from "~/modules/cancellation-reason/stores/useCancellationReasonStore";
const open = async () => {
  visible.value = true
}
const paginate = async (event: any) => {
  canceledBookingDataSource.pageIndex.value = event.page + 1;
  canceledBookingDataSource.pageSize.value = event.rows;
  await canceledBookingDataSource.get();
};
const visible = ref(false);
const { canceledBookingDataSource, bookingCancellationReasonDataSource } = defineProps<{
  canceledBookingDataSource: IBookingListDataSource,
  bookingCancellationReasonDataSource:IListDataSource<any>
}>();

const cancellationReasonStore = useCancellationReasonStore();
const getReason = (booking:any) => cancellationReasonStore.items
    .find((cr: any) =>
      bookingCancellationReasonDataSource.items.value.some((bcr:any) =>
        bcr.bookingId == booking.id && cr.id == bcr.cancellationReasonId
      )
    )
    ?.caption
defineExpose({ open });
</script>

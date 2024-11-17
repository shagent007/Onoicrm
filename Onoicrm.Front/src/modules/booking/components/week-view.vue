<template>
  <table  class="booking-spreadsheet">
    <tr v-for="row in table.rows" :key="row.id">
      <td
        :style="getStyles(cell)"
        v-for="cell in row.cells as any[]"
        class="booking-spreadsheet__cell"
        :class="getClasses(cell)"
        :key="cell.id"
        v-html="cell.value"
        @mousedown="startSelection($event, cell)"
        @mouseenter="updateSelection($event, cell)"
        @mouseup="endSelection($event);"
        @contextmenu.prevent="showMenu($event, cell)"
      />
    </tr>
  </table>

  <OverlayPanel  ref="updateBookingTooltip">
    <div class="flex flex-column">
      <Button
        size="small"
        severity="primary"
        style="width:300px;"
        class="mb-3"
        icon="pi pi-times"
        @click="reportBooking()"
      >
        Написать отчёт
      </Button>

      <Button
        @click="remove()"
        size="small"
        severity="danger"
        style="width:300px;"

        icon="pi pi-times"
      >
        Отменить
      </Button>
    </div>

  </OverlayPanel>

  <OverlayPanel ref="createBookingTooltip">
    <Button
      @click="add()"
      style="width:300px;"
      icon="pi pi-plus"
    >
      Добавить запись
    </Button>
  </OverlayPanel>


  <Dialog v-model:visible="visible" modal header="Выберте причины отмены" :style="{ width: '50vw' }">
    <div class="mb-5"/>
    <div v-for="item in cancellcationReasonStore.items" :key="item.id" class="flex align-items-center mb-3">
      <Checkbox
        :binary="true"
        :model-value="hasValue(item)"
        :inputId="`input-${item.id}`"
        :value="item.id"
        @update:model-value="updateReasons($event, item)"
      />
      <label :for="`input-${item.id}`" class="ml-2"> {{ item.caption }} </label>
    </div>
    <template #footer>
      <Button @click="cancel()" >Отменить запись</Button>
    </template>
  </Dialog>

  <booking-report-dialog
    :serviceGroupDatasource="serviceGroupDatasource"
    ref="reportDialog"
  />
</template>

<script setup lang="ts">
import {DataCell, Table} from "~/modules/booking/entities/DataCell";
import {inject, ref} from "vue";
import {IListDataSource, ITreeDataSource} from "~/modules/data-sources";
import {useCancellationReasonStore} from "~/modules/cancellation-reason/stores/useCancellationReasonStore";
import {useClinicStore} from "~/modules/clinic/stores/useClinicStore";
import {defaultMessage, IMessage} from "~/shared";
import BookingReportDialog from "~/modules/booking/components/booking-report-dialog.vue";
import {useToothStore} from "~/modules/tooth";

const {table, dataSource, bookingCancellationReasonDataSource, serviceGroupDatasource} = defineProps<{
  table:Table
  dataSource:IListDataSource<any>,
  bookingCancellationReasonDataSource:IListDataSource<any>,
  serviceGroupDatasource:ITreeDataSource
}>();

const visible = ref(false);
const createBookingTooltip = ref();
const updateBookingTooltip = ref();

const emit = defineEmits(["select", "refresh"]);
const selecting = ref<boolean>(false);
const start = ref<DataCell| null>(null)
const end = ref<DataCell | null>(null)
const selectedBookingId = ref<number|null>(0)
const cancellationReasons = ref<any[]>([])
const clinicStore = useClinicStore();
const cancellcationReasonStore = useCancellationReasonStore();
const message = inject<IMessage>("message", defaultMessage)
const reportDialog = ref<BookingReportDialog>();

const hasValue = (item:any) => cancellationReasons.value.some((i:any) => i == item.id)
const getStyles =(cell:any) => {
  if(cell.constructor.name == DataCell.name){
    return `background-color:${cell.color};`;
  }
  return ""
}

const getClasses = (cell:any) => {
  const selected = isSelected(cell);
  return [cell.className, {"booking-spreadsheet__cell--selected":selected}]
}

const startSelection = (event:MouseEvent,cell:DataCell) => {
  closeAllContextMenu();
  if(event.button == 2) return;
  if(cell.constructor.name !== DataCell.name) {
    start.value = null
    end.value =null;
    selecting.value = false;
    return
  }
  selecting.value = true;
  start.value = cell
  end.value =cell;
};

const updateReasons = (event:any, item:any) => {
  if(event){
    return cancellationReasons.value.push(item.id);
  }
  cancellationReasons.value = cancellationReasons.value.filter((i:any) => i != item.id)
}

const reportBooking = async () => {
  const booking = dataSource.findItem(selectedBookingId.value as number);
  await reportDialog.value.open(booking);
}

const updateSelection = (event:MouseEvent,cell:DataCell) => {
  if(cell.constructor.name !== DataCell.name || event.button == 2) return;
  if (selecting.value) {
    end.value =cell;
  }
};
const endSelection = (event:MouseEvent) => {
  if(event.button == 2) return;
  selecting.value = false;
};

const add = () => {
  const selectedArea = extractUniqueDatesAndTimes(getSelectedArea());
  emit("select", selectedArea)
}

const closeAllContextMenu = () => {
  updateBookingTooltip.value.hide();
  createBookingTooltip.value.hide();
}

const remove = () => {
  visible.value = true;
}

const cancel = async () => {
  const reasons = cancellationReasons.value.map((i:any) => ({
    bookingId: selectedBookingId.value,
    cancellationReasonId: i,
    clinicId: clinicStore.clinic.value?.id
  }))

  const result = await bookingCancellationReasonDataSource.addRange(reasons);

  bookingCancellationReasonDataSource.items.value = [
    ...bookingCancellationReasonDataSource.items.value,
    ...result
  ]

  emit("refresh");

  message.success("Запись успешно отменён")
}

const extractUniqueDatesAndTimes = (dataArray:DataCell[]) => ({
  dates: [...new Set(dataArray.map(item => item.date))],
  times: [...new Set(dataArray.map(item => item.time))],
});

const showMenu = (event:any,cell:DataCell) => {
  closeAllContextMenu();
  if(cell.constructor.name !== DataCell.name || event.button == 1)return;
  if(!!cell.bookingId){
    selectedBookingId.value = cell.bookingId;
    updateBookingTooltip.value.show(event);
    return;
  }

  const selected = isSelected(cell);
  if(!selected) return;
  createBookingTooltip.value.show(event);
}

const getSelectedArea = ():DataCell[] => {
  if(!start.value || !end.value) return [];
  const minRow = Math.min(start.value.rowIndex, end.value.rowIndex);
  const maxRow = Math.max(start.value.rowIndex, end.value.rowIndex);
  const minCell = Math.min(start.value.collIndex, end.value.collIndex);
  const maxCell = Math.max(start.value.collIndex, end.value.collIndex);
  const result:DataCell[] = [];
  for (const row of table.rows) {
    for (const cell of row.cells as DataCell[]) {
      if(
        cell.rowIndex >= minRow &&
        cell.rowIndex <= maxRow &&
        cell.collIndex >= minCell &&
        cell.collIndex <= maxCell
      ){
        result.push(cell)
      }
    }
  }

  return result;

}

const isSelected = (cell:DataCell) => {
  if(cell.constructor.name !== DataCell.name){
    return false;
  }
  if(!start.value || !end.value) return false;

  const minRow = Math.min(start.value.rowIndex, end.value.rowIndex);
  const maxRow = Math.max(start.value.rowIndex, end.value.rowIndex);
  const minCell = Math.min(start.value.collIndex, end.value.collIndex);
  const maxCell = Math.max(start.value.collIndex, end.value.collIndex);
  return (
    cell.rowIndex >= minRow &&
    cell.rowIndex <= maxRow &&
    cell.collIndex >= minCell &&
    cell.collIndex <= maxCell
  );
}
</script>

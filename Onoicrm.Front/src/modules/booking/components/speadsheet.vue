<template>
  <div  class="relative">
    <div class="flex align-items-center justify-content-between">
      <TabView
        v-model:activeIndex="month"
        class="tab--moth-view"
        :scrollable="true"
        @update:active-index="refresh(); updateFilter()"
      >
        <TabPanel
          v-for="monthId in 12"
          :key="monthId"
          :header="getMothName(monthId)"
        />
      </TabView>
      <div class="flex align-items-center">
        <Button @click="year--; updateFilter();" rounded text icon="pi pi-chevron-left" />
        <b class="block mx-3">{{year}}</b>
        <Button @click="year++; updateFilter();" rounded text icon="pi pi-chevron-right" />
      </div>
    </div>

    <week-view
      v-for="table in tables"
      :key="table.id"
      :service-group-datasource="serviceGroupDataSource"
      :table="table"
      :data-source="dataSource"
      :booking-cancellation-reason-data-source="bookingCancellationReasonDataSource"
      @select="$emit('select', $event)"
      @refresh="refresh()"
    />


    <div class="fixed bottom-0" >
      <slot name="tabs"></slot>
    </div>
  </div>
</template>

<script setup lang="ts">
import {computed, onMounted, ref} from 'vue';
import {Week} from "~/modules/booking/entities/Week";
import {Day} from "~/modules/booking/entities/Day";
import {getMonthFirstMonday} from "~/modules/booking/services/getMonthFirstMonday";
import {getMonthLastSunday} from "~/modules/booking/services/getMonthLastSunday";
import {getMonth} from "~/modules/booking/services/getMonth";
import {getYear} from "~/modules/booking/services/getYear";
import {Filter, IListDataSource, ITreeDataSource, useTreeDataSource} from "~/modules/data-sources";
import moment, {Moment} from "moment";
import {TimeSlot} from "~/modules/booking/entities/TimeSlot";
import {Booking} from "~/modules/booking";
import {useServiceStore} from "~/modules/service/stores/useSymptomStore";
import {DisplayService} from "~/modules/booking/services/DisplayerService";
import {useArmchairStore} from "~/modules/armchair";
import {DataCell, DateCell, EmptyCell, Row, Table, TimeCell, ValueType} from "~/modules/booking/entities/DataCell";
import WeekView from "~/modules/booking/components/week-view.vue";
import {Cell} from "~/modules/booking/entities/Cell";
import {TreeDataSourceConfig} from "~/modules/data-sources/hooks/useTreeDataSource";
import {findElementInTree} from "~/shared/lib/helpers";

const {
  timeInterval,
  timeRange,
  dataSource,
  bookingCancellationReasonDataSource,
  serviceGroupDataSource
} = defineProps<{
  timeInterval:number,
  timeRange:string,
  dataSource:IListDataSource<any>,
  bookingCancellationReasonDataSource:IListDataSource<any>,
  serviceGroupDataSource:ITreeDataSource
}>()
const dateFormat = 'DD.MM.YYYY';
const timeFormat = 'HH:mm';
const emit = defineEmits(["select"]);
const month = ref<number>(+getMonth().format("M")-1);
const year = ref<number>(+getYear().format("YYYY"))
const getMothName = (monthId:number) => getMonth(monthId-1, year.value).format('MMMM');
const serviceStore = useServiceStore();
const armchairStore = useArmchairStore();
const tables = ref<Table[]>([]);

const bookings = computed(() => dataSource.items.value.filter(b =>
  !bookingCancellationReasonDataSource.items.value.some(bcr => bcr.bookingId == b.id)
))


const getTimeSlot = (date:Moment) => {
  const [startTime, endTime] = timeRange
    .split('-').map(time => time.trim())
    .map(t => moment(t, timeFormat));
  const count = endTime.diff(startTime,"minutes")/timeInterval;
  return Array
    .from({length:count})
    .map(()=>{
      const timeSlot = new TimeSlot({
        value:startTime.format(timeFormat),
        booking: dataSource.items.value.find((b:Booking) =>
          date.format(dateFormat) == moment(b.dateTime).format(dateFormat)
          &&
          b.timeSlots.some(
            (ts:string) => ts == startTime.format(timeFormat)
          )
        )
      });
      timeSlot.setColor(serviceStore.items)
      startTime.add(timeInterval, 'minutes');
      return timeSlot;
    });
}

const weeks = computed(()=>{
  const startDate = getMonthFirstMonday(month.value, year.value);
  const endDate = getMonthLastSunday(month.value, year.value);
  const weekCount = Math.ceil(endDate.diff(startDate, 'weeks')) + 1;
  return Array
    .from({length: weekCount})
    .map(() => new Week({
      days: Array
        .from({length: 13}) // с учётом ячеек для времени 8:00, 8:15 ..
        .map((_, index) => {
          const day = new Day({
            caption: startDate.format('dddd'),
            date: startDate.format('D-MMMM'),
            value: startDate.format('DD.MM.YYYY'),
            timeSlots:getTimeSlot(startDate),
            displayService:new DisplayService(armchairStore.items)
          });
          if (index % 2) return new Day(); // Добавляем пустые ячейки для времени  8:00, 8:15 ..
          startDate.add(1, 'day')
          return day
        })
    }));
})

const getInitials = (fullName: string): string => {
  const parts = fullName.trim().split(' ');

  if (parts.length === 0) {
    return '';
  }

  const initials = parts.map((part, index) => {
    if (index === 0) {
      // Фамилия
      return part;
    } else {
      // Имя или отчество
      if (part.endsWith('овна') || part.endsWith('кызы')) {
        // Если это женское отчество, то оставляем его полностью
        return part;
      } else {
        // Иначе, сокращаем до первой буквы
        return `${part.charAt(0)}.`;
      }
    }
  });

  return initials.join(' ');
};

const generateTables = () => {
  let startDate = getMonthFirstMonday(month.value, year.value);
  const endDate = getMonthLastSunday(month.value, year.value);
  const tableLength = Math.ceil(endDate.diff(startDate, 'weeks')) + 1;
  let counter = 0;
  const tables:Table[] = []

  for(let tableIndex=0; tableIndex < tableLength; tableIndex++) {
    const rows:Row[] = [];
    const [startTime, endTime] = timeRange
      .split('-').map(time => time.trim())
      .map(t => moment(t, timeFormat));
    const rowLength = endTime.diff(startTime,"minutes")/timeInterval + 1;
    for (let rowIndex=0; rowIndex<rowLength; rowIndex++) {
      const cells:any[] = []
      const startDateCopy = startDate.clone();
      for (let collIndex=0; collIndex<13; collIndex++) {
        counter++;
        let cell
        if(rowIndex==0) {
          cell = collIndex % 2
            ? new EmptyCell({id: counter})
            : new DateCell({
                id: counter,
                caption: startDateCopy.format('dddd'),
                date: startDateCopy.format('D-MMMM'),
                data: startDateCopy.format(dateFormat),
              })
        } else {
          if(collIndex % 2){
            cell = new TimeCell({
              id: counter,
              value: startTime.format(timeFormat),
            })
          } else {
            const booking = bookings.value.find(b =>
              moment(b.dateTime).format(dateFormat) === startDateCopy.format(dateFormat)
              &&
              b.timeSlots.includes(startTime.format(timeFormat))
            )
            const serviceGroup = serviceGroupDataSource.root.value[0]?.children.find((s:any) =>
              booking?.serviceGroups?.some?.((bs:any) =>
                bs?.serviceGroupId == s?.id
              )
            )

            cell = new DataCell({
              id: counter,
              bookingId:booking?.id,
              value:"",
              rowIndex,
              collIndex,
              time:startTime.format(timeFormat),
              date: startDateCopy.format(dateFormat),
              color: serviceGroup?.color
            })

            const prevCell = rows[rowIndex-1].cells[collIndex] as DataCell;

            if(cell.bookingId != prevCell.bookingId){
              cell.valueType = ValueType.Patient;
              if(booking?.patient?.fullName){
                cell.value = getInitials(booking?.patient?.fullName);
              }
            }

            if(cell.bookingId == prevCell.bookingId && prevCell.valueType == ValueType.Patient){
              cell.valueType = ValueType.Armchair
              cell.value = armchairStore.items.find(a =>
                booking?.armchairId == a.id
              )?.caption;
            }
          }
        }

        if(rowIndex==rowLength-1 && collIndex === 12){
          startDate = startDateCopy.clone().add(1, 'day');
        } else if( collIndex%2){
          startDateCopy.add(1, 'day')
        }

        cells.push(cell);
      }

      if(rowIndex > 0){
        startTime.add(timeInterval, "minutes")
      }

      rows.push(new Row({cells}))
    }
    tables.push(new Table({rows}))
  }


  return tables;
}

const updateFilter = async () => {
  let firstMonday = getMonthFirstMonday(month.value, year.value).format(dateFormat);
  const lastSunday = getMonthLastSunday(month.value, year.value).format(dateFormat);
  dataSource.filter.value = dataSource.filter.value.filter(f => f.name != "dateTimeRange");
  dataSource.filter.value.push(
    new Filter("dateTimeRange", `${firstMonday},${lastSunday}`)
  )
  await dataSource.get();
  refresh();
}


const scrollToCurrentWeek = () => {
  const element = document.getElementById("booking-spreadsheet--current");
  if(!element) return;
  window.scroll({
    top:element.offsetTop,
    behavior:"smooth"
  })
}

const refresh = () => {
  tables.value = generateTables();
}

defineExpose({refresh})

onMounted( async ()=>{
  scrollToCurrentWeek();
  refresh();
})
</script>

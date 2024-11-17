<template>
  <div class="times px-2">
    <div class="grid ">
      <div v-for="part in workTimeLine" :key="part.id" class="col">
        <div
          @click="addOrDeleteTimeSlot(timeSlot.timePeriod)"
          v-for="timeSlot in part.items" :key="timeSlot.id"
          class="time-slot"
          :class="timeSlot.className"
        >
          {{timeSlot.timePeriod}}
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import {computed, toRefs} from "vue";
import {IListDataSource} from "~/modules/data-sources/interfaces/IListDataSource";
import {useClinicStore} from "~/modules/clinic/stores/useClinicStore";
import {Booking} from "../../booking/entities/Booking";

import {getGUID} from "~/shared/lib/getGUID";
import {IBookingListDataSource} from "~/modules/booking/interfaces/IBookingListDataSource";

interface PropType {
  armchair:any,
  bookingDataSource: IBookingListDataSource,
  colCount?:number,
  model:Booking
}

const clinicStore = useClinicStore();


const props =  withDefaults(defineProps<PropType>(),{colCount:5});
const { armchair,bookingDataSource,colCount,model} = toRefs(props)


const addOrDeleteTimeSlot = (timeSlot:string) => {
  const index = model.value.timeSlots.findIndex((ts:any) => ts == timeSlot);
  if(index > -1){
    model.value.timeSlots.splice(index, 1);
    return;
  }
  model.value.timeSlots.push(timeSlot);
}

const isActive = (timeSlot:string) => bookingDataSource.value.items.value.some((b:any) =>
  b.doctorId == model.value.doctorId
  &&
  b.armchairId == model.value.armchairId
  &&
  model.value.timeSlots.includes(timeSlot)
);

const isExist = (timeSlot:string) => model.value.timeSlots.includes(timeSlot)
const isChairOccupied = (timeSlot:string) => {
  return bookingDataSource.value.items.value.some((b:any) =>
    model.value.id !== b.id
    &&
    b.armchairId == model.value.armchairId
    &&
    b.timeSlots.includes(timeSlot)
  )
}

const isDoctorOccupied = (timeSlot:string) => {
  return bookingDataSource.value.items.value.some((b:any) =>
    model.value.id !== b.id
    &&
    b.doctorId == model.value.doctorId
    &&
    b.timeSlots.includes(timeSlot)
  )
}

const getTimeLineWithInterval = (timeFrom?: string, timeTo?: string, interval?: number): string[] => {
  const timeSlots: any[] = [];
  const [fromHour, fromMinute] = timeFrom!.split(":").map(Number);
  const [toHour, toMinute] = timeTo!.split(":").map(Number);

  let hour = fromHour;
  let minute = fromMinute;

  while (hour < toHour || (hour === toHour && minute < toMinute)) {
    const startTime = `${padZero(hour)}:${padZero(minute)}`;
    minute += interval!;
    hour += Math.floor(minute / 60);
    minute %= 60;
    const endTime = `${padZero(hour)}:${padZero(minute)}`;
    const timePeriod = `${startTime}-${endTime}`
    const className = getTimeSlotClassName(timePeriod);
    timeSlots.push({id:getGUID(),timePeriod, className});
  }
  return timeSlots;
};

const getTimeSlotClassName = (timePeriod:string) => {
  const isActiveTimeSlot:boolean = isActive(timePeriod);
  if(isActiveTimeSlot) return "time-slot--active"
  const isDoctorOccupiedTimeSlot:boolean = isDoctorOccupied(timePeriod);
  if(isDoctorOccupiedTimeSlot) return "time-slot--doctor-occupied"
  const isArmchairOccupiedTimeSlot:boolean = isChairOccupied(timePeriod);
  if(isArmchairOccupiedTimeSlot) return "time-slot--chair-occupied"
  const isExistTimeSlot = isExist(timePeriod);
  if(isExistTimeSlot) return "time-slot--active"
}


const padZero = (num: number): string => num.toString().padStart(2, "0");


const workTimeLine = computed(()=>{
  if(!armchair) return[];
  const timeSlots = getTimeLineWithInterval(armchair.value.workTimeFrom, armchair.value.workTimeTo, 30);
  const subarrayLength = Math.ceil(timeSlots.length / colCount.value);
  const items = [];

  for (let i = 0; i < timeSlots.length; i += subarrayLength) {
    const subarray = timeSlots.slice(i, i + subarrayLength);
    items.push({id:i,items:subarray});
  }
  return items;
})


</script>

<template>
<!-- Отмененные записи -->
  <div class="calendar-date flex md:hidden">
    <h4 class="calendar-date__caption text-lg" style="color: #092C4C">{{ getTitleDate() }}</h4>
    <div class="calendar-mode flex align-items-center">
      <p @click="goToday()" class="calendar-mode__today text-gray-500 font-semibold">Сегодня</p>
    </div>
  </div>
  <div class="overflow-x-hidden flex md:hidden">
    <div class="flex align-items-center w-full justify-content-between column-gap-4 mb-3">
      <div v-for="day in weekDays" :key="day.day" class="day cursor-pointer" @click="linkDay(day.moment)">
        <h4 class="day__week-name" :class="{'text-blue-500': isSameDay(day.moment)}">{{ day.day }}</h4>
        <h4 class="day__month-day" :class="{'text-blue-500': isSameDay(day.moment)}">{{ day.date }}</h4>
      </div>
    </div>
  </div>
  <div class="calendar-items mb-2">
    <slot name="items-mobile"></slot>
  </div>

  <div ref="calendar"  class="relative w-full" >
    <FullCalendar
        v-touch:swipe.left="swipeLeft"
        v-touch:swipe.right="swipeRight"
        ref="fullcalendar"
        id="Calendar"
        :options="calendarOptions"
    >
      <template v-slot:eventContent="arg">

        <div v-if="arg.event.extendedProps?.id" class="w-full  flex align-items-center" :class="{'justify-content-between h-full':isCompact(arg,clinicStore.clinic?.bookingTimeDuration), 'pt-1':!isCompact(arg,clinicStore.clinic?.bookingTimeDuration)}">
          <h4
            :style="`color:${arg.event.borderColor} !important;line-height: 1;`"
            v-if="arg.event.extendedProps?.id"
            :class="{'mb-1':!isCompact(arg,clinicStore.clinic?.bookingTimeDuration ),'mb-0':isCompact(arg,0)}"
            class="text-xs"
          >
            {{arg.event.title}}
          </h4>
          <state v-if="isCompact(arg,clinicStore.clinic?.bookingTimeDuration)" :arg="arg"/>
        </div>
        <p
          v-if="!isCompact(arg, 30) || !arg.event.extendedProps?.id"
          :style="`color:${arg.event.borderColor} !important;`"
          class="flex align-items-center"
          :class="{'mt-1': !arg.event.extendedProps?.id}"
        >
          <clock class="mr-1" :arg="arg"/>
          <span class="text-xs">{{ arg.timeText }}</span>
        </p>
        <p
          :style="`color:${arg.event.borderColor} !important;`"
          v-if="arg.event.extendedProps?.id && !isCompact(arg, clinicStore.clinic?.bookingTimeDuration)"
          class="flex align-items-center"
        >
          <state class="mr-1" style="margin-top: 0.6px;" :arg="arg"/>
          <span class="text-xs block" >{{ getState(arg.event.extendedProps.stateId) }}</span>
        </p>
      </template>
    </FullCalendar>
    <div class="hidden md:flex fixed z-5 tabs-container" >
      <slot name="tabs"></slot>
    </div>
  </div>
  <OverlayPanel ref="createBookingTooltip" class="short-dialog">
    <div class="flex flex-column align-items-end">
      <Button
        style="width:300px;"
        @click="addEvent(currentSelectInfo)"
        icon="pi pi-plus"
        class="btn-info mb-2"
        size="small"
        severity="info" outlined
      >
        Добавить запись
      </Button>

      <Button
        style="width:300px;"
        @click="reserveBooking(currentSelectInfo)"
        icon="pi pi-plus"
        class="btn-info"
        size="small"
        severity="secondary"
        outlined
      >
        Забронировать
      </Button>
    </div>
  </OverlayPanel>

  <OverlayPanel ref="updateBookingTooltip" class="short-dialog">
    <div class="flex flex-column align-items-end p-2">
      <Button
        size="small"
        style="width:300px;"
        class="btn-info"
        :class="{'mb-3':isCreated || hasPermissions}"
        icon="pi pi-times"
        v-if="currentSelectInfo?.event?.extendedProps?.stateId != 7"
        @click="reportBooking()"
        severity="info" outlined
      >
        {{ isCreated ? "Написать отчёт" : "Редактрировать" }}
      </Button>
      <Button
        v-if="isCreated && currentSelectInfo?.event?.extendedProps?.stateId != 7"
        @click="cancelBooking()"
        size="small"
        severity="danger"
        :class="{'mb-3': hasPermissions}"
        style="width:300px;"
        class="btn-danger"
        icon="pi pi-times"
        outlined
      >
        Отменить
      </Button>
      <Button
        v-if="hasPermissions || (isCurrent && currentSelectInfo?.event?.extendedProps?.stateId == 7)"
        size="small"
        severity="danger"
        @click="remove()"
        style="width:300px;"
        class="btn-danger"
        icon="pi pi-trash"
        outlined
      >
        Удалить
      </Button>


      <div v-if="isCreated && currentSelectInfo?.event?.extendedProps?.stateId != 7" class="flex w-full mt-3 justify-content-between">
        <div class="flex flex-column align-items-start">
          <h4>Пациент</h4>
          <div class="flex align-items-center">
            <p>10 мин</p>
            <Button @click="notifyPatient" class="btn btn-primary btn-bell p-1">
              <i class="pi pi-bell"></i>
            </Button>
          </div>
        </div>
        <div class="flex flex-column ">
          <h4>Врач</h4>
          <div class="flex align-items-center">
            <p>10 мин</p>
            <Button @click="notifyDoctor" class="btn btn-primary btn-bell p-1">
              <i class="pi pi-bell"></i>
            </Button>
          </div>
        </div>
      </div>
    </div>
  </OverlayPanel>

  <Dialog v-model:visible="visible" modal header="Выберте причины отмены" :style="{ width: '50vw' }">
    <div class="mb-5"/>
    <div v-for="item in cancellationReasonStore.items" :key="item.id" class="flex align-items-center mb-3">
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
    :serviceGroupDatasource="serviceGroupDataSource"
    ref="reportDialog"
  />
  <canceled-bookin-dialog
    :bookingCancellationReasonDataSource="bookingCancellationReasonDataSource"
    :canceled-booking-data-source="canceledBookingDataSource"
    ref="canceledDialog"
  />
  <booking-report-dialog-viewer  ref="bookingViewer" />
</template>

<script setup lang="ts">
import {computed, inject, nextTick, onMounted, reactive, ref} from 'vue'
import FullCalendar from '@fullcalendar/vue3'
import dayGridPlugin from '@fullcalendar/daygrid'
import timeGridPlugin from '@fullcalendar/timegrid'
import interactionPlugin from '@fullcalendar/interaction'
import ruLocale from '@fullcalendar/core/locales/ru'
import {onClickOutside, useWindowSize} from '@vueuse/core'
import {CalendarOptions} from "@fullcalendar/core";
import {
  Filter,
  IListDataSource,
  ITreeDataSource, ObjectDataSourceConfig,
  UpdateFieldModel,
  useObjectDataSource,
} from "~/modules/data-sources";
import moment from "moment";
import BookingReportDialog from "~/modules/booking/components/booking-report-dialog.vue";
import CanceledBookingDialog from "~/modules/booking/components/canceled-bookin-dialog.vue";
import {useCancellationReasonStore} from "~/modules/cancellation-reason/stores/useCancellationReasonStore";
import {useClinicStore} from "~/modules/clinic/stores/useClinicStore";
import {defaultConfirm, defaultMessage, IMessage, TConfirm} from "~/shared";
import {IBookingListDataSource} from "~/modules/booking/interfaces/IBookingListDataSource";
import CanceledBookinDialog from "~/modules/booking/components/canceled-bookin-dialog.vue";
import {useUserStore} from "~/modules/user/stores/useUserStore";
import State from "~/modules/booking/ui/state.vue";
import Clock from "~/modules/booking/ui/clock.vue";
import {Booking} from "~/modules/booking/entities/Booking";
import BookingReportDialogViewer from "~/modules/booking/components/booking-report-dialog-viewer.vue";

const props = defineProps<{
  dataSource:IBookingListDataSource,
  serviceGroupDataSource:ITreeDataSource,
  bookingCancellationReasonDataSource:IListDataSource<any>,
  add:Function,
  reserve:Function,
  canceledBookingDataSource: IBookingListDataSource
}>();
const emit = defineEmits(["select", "refresh"]);
const userStore = useUserStore();
const clinicStore = useClinicStore();
const cancellationReasonStore = useCancellationReasonStore();
const reportDialog = ref();
const canceledDialog = ref();
const bookingViewer = ref();
const confirm = inject<TConfirm>("confirm", defaultConfirm);
const message = inject<IMessage>("message", defaultMessage)
const weekDays = ref();
const today = ref<moment.Moment>(moment());
const currentSelectInfo=ref<any>(null)
const currentEvents = ref<any[]>([])
const cancellationReasons = ref<any[]>([])
const visible = ref(false);
const createBookingTooltip = ref();
const updateBookingTooltip = ref();
const calendar = ref();
const fullcalendar = ref();
const screenWidth = ref(0);
const isCreated = computed(() => currentSelectInfo.value?.event?.extendedProps?.stateId !== 6)
const getTitleDate = ()=>{
  const month = moment.months(moment().month())
  return `${month.charAt(0).toUpperCase()+month.slice(1)},${moment().get('year')}`
}
const linkDay = (dayOfMonth: moment.Moment) => {
  const api = fullcalendar.value?.getApi?.();
  if(!api) return;
  today.value = dayOfMonth
  console.log(today.value)
  api.gotoDate(today.value.toISOString())
}

const swipeLeft = () => {
  const api = fullcalendar.value?.getApi?.();
  if(!api) return;
  today.value = today.value.add(1,"day");
  api.gotoDate(today.value.toISOString())
  weekDays.value = getWeekDays(today.value);

}

const swipeRight = () => {
  const api = fullcalendar.value?.getApi?.();
  if(!api) return;
  today.value = today.value.subtract(1,"day");
  api.gotoDate(today.value.toISOString());

  weekDays.value = getWeekDays(today.value);
}

const goToday = () => {
  const api = fullcalendar.value?.getApi?.();
  if(!api) return;
  today.value = moment();
  api.gotoDate(today.value.toISOString())
  weekDays.value = getWeekDays(today.value);
}

const isSameDay = (day:moment.Moment) => {
  return day.format("DD.MM.YYYY") == today.value.format("DD.MM.YYYY")
}

const hasValue = (item:any) => cancellationReasons.value.some((i:any) => i == item.id)
const getState = (stateId:number) => {
  switch (stateId) {
    case 1: return "В ожидании";
    case 6: return "Завершён"
    default: return  ""
  }
}

const isCurrent = computed(()=> currentSelectInfo.value?.event?.extendedProps?.doctorId == userStore.profile?.id)

const isCompact = ({event}:any, duration:number=30) => {
  const start = moment(event.start);
  const end = moment(event.end);
  const diff = end.diff(start, "minutes")
  return diff <= duration;
}

const updateReasons = (event:any, item:any) => {
  if(event){
    return cancellationReasons.value.push(item.id);
  }
  cancellationReasons.value = cancellationReasons.value.filter((i:any) => i != item.id)
}

const remove = async () => {
  const _confirm = await confirm({
    message: 'Восстановить нельзя будет',
    header: 'Уверены что хотите удалить данный запись'
  })
  if(!_confirm) return;
  await props.dataSource.remove(currentSelectInfo.value.event?.id as number);
  refresh();
  message.success("Успешно удалён")
}

const objectDataSource = useObjectDataSource(
  new ObjectDataSourceConfig({
    className:"Booking"
  })
)

const cancelBooking  = async () => {
  await cancellationReasonStore.getData();
  visible.value = true;
}

const reportBooking = async () => {
  objectDataSource.config.value.id = currentSelectInfo.value.event.id as number;
  const booking = await objectDataSource.get();
  const result = await reportDialog.value.open(booking);
  if(!result) return;
  try {
    await props.dataSource.updateField(booking.id,[
      new UpdateFieldModel({
        fieldName:"stateId",
        fieldValue: 6
      }),
      new UpdateFieldModel({
        fieldName:"discount",
        fieldValue: result.booking.discount
      }),
      new UpdateFieldModel({
        fieldName:"discountType",
        fieldValue: result.booking.discountType
      })
    ])
    await props.dataSource.updateReport(booking.id,result.updateModel)
    message.success("Изменения успешно сохранены")
  }catch (e) {
    message.error("Произошла неизвестная ошибка пожалуйста сообшите об этом в службу поддержки")
  }
  refresh()
}
const cancel = async () => {
  const reasons = cancellationReasons.value.map((i:any) => ({
    bookingId:currentSelectInfo.value.event.id as number,
    cancellationReasonId: i,
    clinicId: clinicStore.clinic?.id
  }))

  const result = await props.bookingCancellationReasonDataSource.addRange(reasons);

  props.bookingCancellationReasonDataSource.items.value = [
    ...props.bookingCancellationReasonDataSource.items.value,
    ...result
  ]

  emit("refresh");

  message.success("Запись успешно отменён")
  await props.dataSource.get();
  refresh()
  visible.value = false;
}
const hasPermissions = computed(()=> userStore.hasOptionalRoles([
  "System Administrator",
  "Administrator",
  "Director"
]));


const reserveModel = reactive<Booking>(
  new Booking({
    clinicId: clinicStore.clinic?.id,
    patient: null,
    stateId:7
  })
)

const addEvent =  async (selectInfo:any) => {
  let calendarApi = selectInfo.view.calendar
  if (['multiMonthYear', 'dayGridMonth'].includes(selectInfo.view.type)) {
    calendarApi.gotoDate(selectInfo.start)
    calendarApi.changeView('timeGridDay')
    return;
  }
  await props.add(selectInfo);
  await props.dataSource.getSchedule();
  calendarApi.unselect()
  refresh()
}

const reserveBooking = async (selectInfo:any) => {
  let calendarApi = selectInfo.view.calendar
  if (['multiMonthYear', 'dayGridMonth'].includes(selectInfo.view.type)) {
    calendarApi.gotoDate(selectInfo.start)
    calendarApi.changeView('timeGridDay')
    return;
  }
  reserveModel.dateTimeStart = selectInfo.start;
  reserveModel.dateTimeEnd = selectInfo.end;

  await props.reserve(reserveModel);
  await props.dataSource.get();
  calendarApi.unselect()
  refresh()
}

const getWeekDays =(date:moment.Moment)=> {
  const startOfWeek = date.clone().startOf('week');
  const endOfWeek = date.clone().endOf('week');
  const daysOfWeek = [];

  let day = startOfWeek;

  while (day <= endOfWeek) {
    daysOfWeek.push({
      day: day.format('dd'), // Форматируем день недели (например, "Mo", "Tu")
      date: day.date().toString().padStart(2, '0'),
      moment:day.clone()// Получаем число месяца
    });

    day = day.clone().add(1, 'day'); // Переходим к следующему дню
  }

  return daysOfWeek;
}

const handleEvents = (events:any) =>  {
  currentEvents.value = events
}
const updateTime = async (e:any) => {
  await props.dataSource.updateField(e.event.id as unknown as number, [
    new UpdateFieldModel({
      fieldName:"dateTimeStart",
      fieldValue:e.event.start
    }),
    new UpdateFieldModel({
      fieldName:"dateTimeEnd",
      fieldValue:e.event.end
    })
  ])
  message.success("Успешно обновлено")
}

const events = computed(()=> props.dataSource.items.value.map((b:any) => {
  return {
    id: b.id,
    title:  b.stateId != 7 ?  b.patient?.fullName : b.caption,
    start: moment(b.start).toDate(),
    end: moment(b.end).toDate(),
    backgroundColor: b.backgroundColor,
    borderColor:b.borderColor,
    extendedProps: {...b}
  };
}))
if(!clinicStore.clinic) throw new Error();
const {bookingTimeDuration, workEndTime,workStartTime} = clinicStore.clinic;
const calendarOptions:CalendarOptions = reactive({
  plugins: [
    dayGridPlugin,
    timeGridPlugin,
    interactionPlugin,
  ],
  headerToolbar: {
    left: 'prev,next today',
    center: 'title',
    right: 'timeGridWeek,timeGridDay',
  },
  views: {
    dayGridMonth: {
      titleFormat: { year: 'numeric', month: '2-digit', day: '2-digit' },
    },
  },
  slotDuration: {minutes:bookingTimeDuration},
  slotLabelInterval: bookingTimeDuration,
  defaultTimedEventDuration:`00:${bookingTimeDuration}:00`,
  slotLabelFormat: {
    hour: '2-digit',
    minute: '2-digit',
    omitZeroMinute: false,
    meridiem: 'numeric',
  } as any,
  allDaySlot: false,
  height:"auto",
  nowIndicator:true,
  slotMinTime: `${workStartTime}:00`,
  slotMaxTime: `${workEndTime}:00`,
  locale: ruLocale,
  initialView: 'timeGridWeek',
  initialEvents: events.value,
  editable: true,
  selectable: true,
  selectMirror: true,
  dayMaxEvents: 0,
  handleWindowResize:true,
  weekends: true,
  select: async (selectInfo:any) => {
    currentSelectInfo.value = selectInfo;
    createBookingTooltip.value.hide();
    await nextTick();
    createBookingTooltip.value.show(selectInfo.jsEvent, selectInfo.jsEvent.target);
  },
  async eventClick(e){
    currentSelectInfo.value = e;
    const stateId = e.event?.extendedProps?.stateId;
    if(stateId == 7 && !hasPermissions.value && !isCurrent.value){
      return;
    }

    if(stateId == 6 && stateId !== 7 && !hasPermissions.value){
      const booking = await props.dataSource.getById(e.event.id as unknown as number)
      bookingViewer.value.open(booking);
      return;
    }

    if(hasPermissions.value ){
      updateBookingTooltip.value.show(e.jsEvent, e.jsEvent.target)
      return;
    }

    if(e.event?.extendedProps?.stateId !== 6){
      updateBookingTooltip.value.show(e.jsEvent, e.jsEvent.target)
    }
  },
  eventAllow: function(dropInfo, draggedEvent) {
    return ![6,7].includes(draggedEvent?.extendedProps.stateId)
  },
  eventsSet: handleEvents,
  eventResize:updateTime,
  eventDrop:updateTime
})

const notifyPatient = async ()=>{
  try {
    await props.dataSource.notifyPatient(currentSelectInfo.value.event.id as number, `Уважаеммый {Patient.FullName} напоминаем что у вас в {DateTime} запись в клинике {Clinic.Caption} у стоматолога {Doctor.FullName}`)
    message.success('Напоминание пациента успешно отправлено!')
  }catch (e){
    message.error('При напоминание пациента возникла ошибка!')
    throw e
  }
}
const notifyDoctor = async ()=>{
  try {
    await props.dataSource.notifyDoctor(currentSelectInfo.value.event.id as number, `Уважаеммый {Doctor.FullName} напоминаем что у вас в {DateTime} запись на пациента {Patient.FullName} .`)
    message.success('Напоминание доктора успешно отправлено!')
  }catch (e){
    message.error('При напоминание доктора возникла ошибка!')
    throw e
  }
}

const updateFilter = async () => {
  const api = fullcalendar.value?.getApi();
  const data = api?.currentData?.dateProfile?.currentRange;
  if(!data) return;
  const dateTimeRange = `${moment(data.start).format("DD.MM.YYYY")},${moment(data.end).format("DD.MM.YYYY")}`
  await props.dataSource.updateFilter([
    new Filter("dateTimeRange", dateTimeRange),
  ], false)
  await props.dataSource.getSchedule();
  refresh();
}

onMounted(()=>{
  weekDays.value = getWeekDays(today.value);

  const prev = document.querySelector(".fc-prev-button.fc-button.fc-button-primary")!;
  const next = document.querySelector(".fc-next-button.fc-button.fc-button-primary")!;

  prev.addEventListener("click", () => updateFilter());
  next.addEventListener("click", () => updateFilter());

  onClickOutside(fullcalendar.value, () => {
    createBookingTooltip.value.hide();
    updateBookingTooltip.value.hide();
  })

  const {width} = useWindowSize()
  changeScreen(width.value);

  window.addEventListener("resize",(e:any)=>{
    changeScreen(e.target.innerWidth)
  })

})



const changeScreen = (screenSize:number) => {
  if(screenWidth.value == screenSize) return;
  screenWidth.value = screenSize;
  if(screenSize <= 768){
    const api = fullcalendar.value?.getApi?.();
    api?.changeView?.('timeGridDay');
    return;
  }

  if(screenSize >= 768){
    const api = fullcalendar.value?.getApi?.();
    api?.changeView?.('timeGridWeek');
    return;
  }

}
const refresh = () => {
  calendarOptions.events = events.value
}
defineExpose({refresh, goToday})

</script>
<style>
.fc-v-event .fc-event-main{
  color: inherit !important;
}
</style>
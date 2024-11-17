<template>
  <Dialog :visible="visible" @update:visible="close()" modal :style="{ width:'auto','max-width': '80dvw' }" class="dialog">
    <template #header>
      <div class="flex justify-content-end">
      </div>
    </template>
    <div v-if="active == 0" class="grid flex-wrap align-items-stretch">
      <div class="col-12 md:col">
        <div
          class="mb-3 card card--grey card--no-hover flex align-items-center justify-content-center py-7"
        >
          <img src="/img/tooth.png" alt="" class="w-3"/>
        </div>
      <div class="flex flex-nowrap justify-content-between mb-3">
      <div class=" max-h-7rem  card--default-blue flex flex-column justify-content-between  py-2 px-3 mr-3 flex-1">
        <h3 class="mb-2 dialog__title dialog__titles--gray">{{tooth.caption}}</h3>
        <p class="dialog__subtitle dialog__subtitle--gray">{{quarterCaption}}</p>
      </div>
      <div class=" max-h-7rem  card--default-blue flex flex-column justify-content-between py-2 px-3 w-6rem" >
        <h3 class="mb-2 dialog__subtitle dialog__titles--gray">Номер</h3>
        <p class="dialog__title ">{{ tooth.position }}</p>
      </div>
    </div>
      <div class="flex flex-nowrap justify-content-between mb-3">
        <div class=" max-h-7rem  flex flex-column justify-content-between card--default-blue py-2 px-3 mr-3 flex-1">
          <p class="mb-2 dialog__subtitle  dialog__titles--gray">Дата последней изменении</p>
          <h3 class="dialog__title">{{ lastUpdate }}</h3>
        </div>

        <div class=" max-h-7rem  card--default-blue flex flex-column py-2 pt- px-3 w-6rem" >
          <h3 class="mb-2 dialog__subtitle dialog__titles--gray">Четверть</h3>
          <p class="dialog__title ">{{ tooth.quarter }}</p>
        </div>

      </div>
        <Dropdown
          v-if="!hasBookingTooth"
            :options="toothStateStore.items"
            option-label="caption"
            v-model="model.toothStateId"
            option-value="id"
            class="color-select card-aqua w-full"
        >
          <template #value="{placeholder}">
            <div class="max-h-7rem  card--default-blue py-2 px-3 flex justify-content-between">
              <div class="flex flex-column justify-content-between">
                <p class="dialog__subtitle dialog__titles--gray mb-2 ">Текущее состояние</p>
                <h3 class=" dialog__title">{{toothStateCaption}}</h3>
              </div>
              <div :style="selectedToothStateColor" class="border-round-xl w-6rem card--memo flex align-items-center justify-content-center ">
                <p class="dialog__subtitle" v-if="!selectedToothStateColor">Выберите</p>
              </div>
            </div>
          </template>
          <template #option="{option}">
            <div class="flex justify-content-between">
              <h4>{{option?.caption}}</h4>
              <div
                :style="`background-color:${option?.color}`"
                class="border-round-xl  w-3rem card--memo"
              />
            </div>
          </template>
        </Dropdown>
        <div v-else-if="toothState" class="max-h-7rem  card--default-blue py-2 px-3 flex justify-content-between">
          <div class="flex flex-column justify-content-between">
            <p class="dialog__subtitle dialog__titles--gray mb-2 ">Текущее состояние</p>
            <h3 class=" dialog__title">{{toothState}}</h3>
          </div>
          <div :style="toothStateColor" class="border-round-xl  w-3rem sm:w-6rem card--memo flex align-items-center justify-content-center "></div>
        </div>
      </div>
      <div v-if="!hasBookingTooth" class="col-12 md:col ">
        <div  class="flex flex-column justify-content-evenly h-full">
          <input
            v-model="model.caption"
            placeholder="Заголовка"
            class="card--default block  mb-3 w-full outline-none border-none p-3 dialog__title"
          />
          <textarea
            v-model="model.description"
            placeholder="Описание"
            class="card--default w-full outline-none border-none p-3 overflow-auto min-h-17rem h-14rem md:h-full dialog__subtitle"
          />
        </div>
      </div>
      <div v-if="!hasBookingTooth" class="col-12 md:col ">
        <div class="flex flex-column justify-content-evenly overflow-auto min-h-20rem  md:h-full ">
          <booking-tooth-files
              :data-source="dataSource"
              :model="model"
              class="max-h-full h-full"
          />
        </div>

      </div>
      <div class="col-12 md:col">
        <div class="card--memo  border-round-xl overflow-auto  p-3 " :style="`height:${historyBlockHeight};`">
          <h3 class="my-3">История</h3>

          <div
              v-for="item in dataSource.items.value as any[]"
              @click="report?.open(item?.id)"
              class="flex px-3 py-2 card--default-blue justify-content-between mb-3 cursor-pointer">
            <div>
              <h3 class="dialog__title  mb-2">{{item?.caption}}</h3>
              <p class="dialog__subtitle text-blue-500">{{moment(item?.booking?.dateTime).format("DD.MM.YYYY")}}</p>
            </div>
            <div class="flex align-items-center" >
              <div class="w-2rem h-2rem border-round-xl" :style="`background-color:${toothStateStore.findItem(item?.toothStateId)?.color}`"/>
              <img src="/img/arrow-next.svg" alt="" class="ml-2" style="width: 10px;">
            </div>
          </div>


        </div>
        <div v-if="!hasBookingTooth" class="flex justify-content-between pt-3">
          <Button severity="secondary" label="Отмена" class="w-4"></Button>
          <Button @click="add" label="Сохранить" class="w-7"></Button>
        </div>
      </div>
    </div>
    <tooth-report ref="report"/>
  </Dialog>
  <div>
  </div>
</template>

<script setup lang="ts">
import {computed, reactive, ref,inject} from "vue";
import moment from "moment";
import {getQuarterCaption} from "~/modules/tooth"
import {defaultMessage, IMessage} from "~/shared";
import {BookingTooth} from "~/modules/tooth";
import {
  Filter, IListDataSource,
  ListDataSourceConfig, useListDataSource
} from "~/modules/data-sources";
import {useToothStateStore} from "~/modules/tooth-state";
import BookingToothFiles from "~/modules/booking-tooth/components/booking-tooth-files.vue";
import ToothReport from "~/modules/tooth/components/tooth-report.vue";
import FileList from "~/modules/files/components/file-list.vue";
import {BookingFiles} from "~/modules/booking";

const visible = ref(false);
const resolve = ref();
const tooth = ref();
const message = inject<IMessage>("message", defaultMessage);
const report = ref<ToothReport>();
const {patient} = withDefaults(defineProps<{ patient:any, editMode?:boolean }>(),{
  editMode:true
})
const active = ref(0);
const toothStateStore = useToothStateStore();
const historyBlockHeight = computed(() => !hasBookingTooth.value ? "calc(100% - 3.25rem)" : "100%");

const selectedToothStateColor = computed(()=>{
  const color = toothStateStore.findItem(model.toothStateId)?.color;
  if(!color) return "";
  return `background-color:${color};`
})

const toothStateColor = computed(()=>{
  const bookingTooth:any =  dataSource.items.value?.find((e:any) => e.toothStateId != null);
  if(!bookingTooth) return null;
  const color = toothStateStore.findItem(bookingTooth.toothStateId)?.color;
  if(!color) return "";
  return `background-color:${color};`
})

const toothState = computed(()=> {
  const bookingTooth:any =  dataSource.items.value?.find((e:any) => e.toothStateId != null);
  if(!bookingTooth) return null;
  return toothStateStore.findItem(bookingTooth.toothStateId)?.caption;
})
const lastUpdate = computed(()=> {
  const date =  dataSource.items.value?.[0]?.createDate;
  const format = moment(date);
  if(!format.isValid()) return null;
  return format.format("DD.MM.YYYY")
})


const model = reactive<BookingTooth>(
  new BookingTooth({
    files:[]
  })
)

const toothStateCaption = computed(()=> toothStateStore.findItem(model.toothStateId)?.caption)
const resetModel = ()=>{
  Object.assign(model, new BookingTooth())
}

const hasBookingTooth = computed(()=>dataSource.items.value.some((bt:any) => bt.bookingId == model.bookingId))

const dataSource = useListDataSource(
  new ListDataSourceConfig({
    className:"BookingTooth",
    orderFieldName: "CreateDate",
    orderFieldDirection: "DESC",
    filter:[
      new Filter("patientId", patient.id),
    ]
  })
)

const returnPromise = () => {
  visible.value = true;
  return new Promise(r => resolve.value = r);
}

const show = async (t:any,booking?:any, patientDataSource?:IListDataSource<any>) => {
  tooth.value = t;
  model.toothId = t.id;
  model.bookingId = booking?.id
  if(patientDataSource){
    dataSource.items.value = patientDataSource.items.value;
    return returnPromise();
  }

  dataSource.filter.value = [
    new Filter("patientId", patient.id),
    new Filter("toothId", t.id)
  ]
  await dataSource.get();
  return returnPromise();
}


defineExpose({ show })

const quarterCaption = computed(()=>getQuarterCaption(tooth.value.quarter))

const close = () => {
  visible.value = false;
  resolve.value();
}

const add = async () => {
  await dataSource.add(model);
  await dataSource.get();
  Object.assign(model, new BookingTooth({bookingId:model.bookingId}))
  message.success("Успешно сохранено")
}

</script>

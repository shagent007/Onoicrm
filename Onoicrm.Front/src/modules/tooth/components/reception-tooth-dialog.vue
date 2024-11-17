<template>
  <Dialog :visible="visible" @update:visible="close()" modal :style="{ 'max-width': '80vw', 'min-width':'30vw' }" class="dialog">
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
        <div class=" max-h-7rem  card--default-blue w-6rem py-2 px-3 flex align-items-center justify-content-center">
          <p class="dialog__subtitle dialog__titles--gray">Четверть</p>
        </div>
      </div>
      <div v-if="toothState" class="max-h-7rem  card--default-blue py-2 px-3 flex justify-content-between">
        <div class="flex flex-column justify-content-between">
          <p class="dialog__subtitle dialog__titles--gray mb-2 ">Текущее состояние</p>
          <h3 class=" dialog__title">{{toothState}}</h3>
        </div>
        <div :style="toothStateColor" class="border-round-xl  w-3rem sm:w-6rem card--memo flex align-items-center justify-content-center "></div>
      </div>
      </div>
      <div v-if="dataSource.items.value.length" class="col-12 md:col">
        <div class="card--memo border-round-xl overflow-auto max-h-20rem sm:max-h-full h-full  p-3 ">
          <h3 class="my-3">История</h3>

          <div
              v-for="item in dataSource.items.value as any[]"
              @click="selectHistory(item.id)"
              class="flex px-3 py-2 card--default-blue justify-content-between mb-3 cursor-pointer">
            <div>
              <h3 class="dialog__title  mb-2">{{item?.caption}}</h3>
              <p class="dialog__subtitle text-blue-500">{{moment(item?.booking?.dateTime).format("DD.MM.YYYY")}}</p>
            </div>
            <div class="flex align-items-center" >
              <div class="w-2rem h-2rem border-round-xl" :style="`background-color:${toothStateStore.findItem(item?.toothStateId)?.color}`"></div>
              <img src="/img/arrow-next.svg" alt="" class="ml-2" style="width: 10px;">
            </div>
          </div>
        </div>
      </div>

      <template v-if="objectDataSource.model.value">
        <div class="col-12 md:col ">
          <div class="flex flex-column justify-content-evenly h-full">
            <input
              v-model="objectDataSource.model.value.caption"
              placeholder="Заголовка"
              class="card--default block  mb-3 w-full outline-none border-none p-3 dialog__title"
            />
            <textarea
              v-model="objectDataSource.model.value.description"
              placeholder="Описание"
              class="card--default w-full outline-none border-none p-3 overflow-auto min-h-17rem h-14rem md:h-full dialog__subtitle"
            />
          </div>
        </div>
        <div class="col-12 md:col ">
          <div class="card--default p-3 h-full">
            <div class="grid">
              <file-list
                :items="objectDataSource.model.value.files"
                v-slot="{item}"
                css-class="col-6"
              >
                <img style="object-fit:cover" :src="`/api/v1/file/${item.id}`" class="w-full h-5rem border-round-xl" alt="">
              </file-list>
            </div>
          </div>
        </div>
      </template>

    </div>
  </Dialog>
</template>

<script setup lang="ts">
import {computed,  ref,inject} from "vue";
import moment from "moment";
import {getQuarterCaption} from "~/modules/tooth"
import {defaultMessage, IMessage} from "~/shared";
import {BookingTooth} from "~/modules/tooth";
import {
  Filter, IListDataSource,
  ListDataSourceConfig, ObjectDataSourceConfig, useListDataSource, useObjectDataSource
} from "~/modules/data-sources";
import {useToothStateStore} from "~/modules/tooth-state";
import FileList from "~/modules/files/components/file-list.vue";

const visible = ref(false);
const resolve = ref();
const tooth = ref();
const message = inject<IMessage>("message", defaultMessage);
const {patient} = withDefaults(defineProps<{ patient:any, editMode?:boolean }>(),{
  editMode:true
})
const active = ref(0);

const toothStateColor = computed(()=>{
  const bookingTooth:any =  dataSource.items.value?.find((e:any) => e.toothStateId != null);
  if(!bookingTooth) return null;
  const color = toothStateStore.findItem(bookingTooth.toothStateId)?.color;
  if(!color) return "";
  return `background-color:${color};`
})
const toothState = computed(()=> {
  const bookingTooth =   dataSource.items.value?.find((e:any) => e.toothStateId != null);
  if(!bookingTooth) return null;
  return toothStateStore.findItem(bookingTooth.toothStateId)?.caption;
})
const lastUpdate = computed(()=> {
  const date =  dataSource.items.value?.[0]?.createDate;
  const format = moment(date);
  if(!format.isValid()) return null;
  return format.format("DD.MM.YYYY")
})

const toothStateStore = useToothStateStore();

const selectHistory = async (id:number) => {
  objectDataSource.config.value.id = id;
  await objectDataSource.get();
}

const objectDataSource = useObjectDataSource<BookingTooth>(
  new ObjectDataSourceConfig({
    className:"BookingTooth"
  })
)

const dataSource = useListDataSource<BookingTooth>(
  new ListDataSourceConfig({
    className:"BookingTooth",
    orderFieldName:"CreateDate",
    orderFieldDirection:"DESC",
    filter:[
      new Filter("patientId", patient?.id),
    ]
  })
)
const returnPromise = () => {
  visible.value = true;
  return new Promise(r => resolve.value = r);
}

const show = async (t:any,booking?:any, patientDataSource?:IListDataSource<any>) => {
  tooth.value = t;
  if(patientDataSource){
    dataSource.items.value = patientDataSource.items.value;
    return returnPromise();
  }
  dataSource.items.value = [];
  objectDataSource.model.value = null;
  dataSource.filter.value = [
    new Filter("patientId", patient.id),
    new Filter("toothId", t.id)
  ]
  await dataSource.get();
  const firstId = dataSource.items?.value?.[0]?.id;
  if(firstId){
    await selectHistory(firstId);
  }


  return returnPromise();
}


defineExpose({ show })

const quarterCaption = computed(()=>getQuarterCaption(tooth.value.quarter))

const close = () => {
  visible.value = false;
  resolve.value();
}


</script>

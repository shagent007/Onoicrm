<template>
  <Dialog
    v-model:visible="visible"
    modal
    :maximizable="maximizable"
    header="Добавить новую запись"
    :class="{'p-dialog-maximized':!maximizable}"
    class="booking-dialog w-12 sm:w-10 md:w-11 lg:w-10 xl:w-7"
  >
    <div class="grid">
      <div class="col-12 md:col-5">
        <app-form v-if="props.clinic" :key="props.clinic.bookingType" :form-service="formService"/>
      </div>
      <div class="col-12 md:col-7">
        <DataTable scrollable scroll-height="300px" class="p-datatable-sm  p-datatable-hoverable-rows" :value="patientDataSource.items.value" @row-click="selectUserProfile">
          <template #empty>
            <div class="text-center">Пациент не найден</div>
          </template>
          <Column field="fullName" header="ФИО" />
          <Column field="whatsAppNumber" header="Номер телефона"/>
        </DataTable>
      </div>
    </div>


    <template #footer>
      <Button @click="reset()" class="p-button-secondary" label="Отмена"/>
      <Button @click="submit()" class="m-0" label="Сохранить"/>
    </template>
  </Dialog>
</template>

<script lang="ts" setup>
import _ from "lodash";
import {
  AppForm,
  FormService,
  FormServiceConfig,
  InputFieldAttribute,
  SelectField,
  SelectFieldConfig,
  StringField,
  Watcher
} from "~/modules/app-form";
import {reactive, ref,onMounted} from "vue";
import {useArmchairStore} from "~/modules/armchair";
import {Booking, BookingModel} from "~/modules/booking/entities/Booking";
import {
  Filter, IListDataSource,
  ITreeDataSource,
  ListDataSourceConfig,
  useListDataSource,
} from "~/modules/data-sources";
import {hasStringValue} from "~/shared/lib/hasStringValue";
import {PhoneField} from "~/modules/app-form/components/editors/phone/models/PhoneField";
import {Clinic} from "~/modules/clinic/Clinic";

const props = defineProps<{ serviceGroupDataSource:ITreeDataSource,doctorDataSource:IListDataSource<any>, clinic:Clinic }>();
const armchairStore = useArmchairStore();
const visible = ref<boolean>(false);
const resolve = ref<(model: any) => any>();

const selectUserProfile = ({data}:any)=>{
  model.booking.patient = data;
  model.booking.patientId = data.id;
}

const model = reactive<BookingModel>(
  new BookingModel({
    booking: new Booking({
      clinicId: props.clinic?.id,
      patient: reactive({
        clinicId: props.clinic.id,
        fullName:"",
        whatsAppNumber:"",
      }),
    })
  })
)

const patientDataSource = useListDataSource(
  new ListDataSourceConfig({
    className: "patient",
    fields:"new(FullName, Id, WhatsAppNumber)",
    filter: [
      new Filter("clinicId", props.clinic?.id),
    ],
  }),
);


const formService = new FormService(
    new FormServiceConfig({
      model,
      fields: [
        new StringField("booking.patient.fullName", {
          attrs: new InputFieldAttribute({
            placeholder: "Введите ФИО",
            caption:"Пациент"
          }),
          watchers:[
            new Watcher({
              changeModel:"searchPatient",
              fields:["booking.patient.fullName"]
            })
          ],
        }),
        new PhoneField("booking.patient.whatsAppNumber", {
          attrs: new InputFieldAttribute({
            caption: "Номер телефона",
            placeholder:"Напишите номер телефона"
          }),
        }),
        new SelectField("booking.serviceGroupId", {
          attrs: new InputFieldAttribute({
            caption:"Услуга",
            placeholder: "Выберите услугу",
          }),
          config: new SelectFieldConfig({
            getItems: "getServices",
            labelKeyName: "caption",
            valueKeyName: "id",
          }),
        }),
        new SelectField("booking.doctorId", {
          canShow:"canShowDoctor",
          attrs: new InputFieldAttribute({
            caption:"Врач",
            placeholder: "Выберите Врача",
          }),
          config: new SelectFieldConfig({
            getItems: "getDoctors",
            labelKeyName: "fullName",
            valueKeyName: "id",
          }),
        }),
        new SelectField("booking.armchairId", {
          canShow:"canShowArmchair",
          attrs: new InputFieldAttribute({
            caption:"Кресло",
            placeholder: "Выберите кресло",
          }),
          config: new SelectFieldConfig({
            getItems: "getArmchairs",
            labelKeyName: "caption",
            valueKeyName: "id",
          }),
        }),
      ],
      actions: {
        getArmchairs:()=> armchairStore.items,
        searchPatient:async (model: BookingModel) =>{
          const fullName = model.booking.patient?.fullName;

          if(!hasStringValue(fullName)) {
            patientDataSource.items.value = []
            return
          }
          patientDataSource.filter.value = [
            new Filter("clinicId", props.clinic?.id),
            new Filter("searchText", fullName)
          ]
          await patientDataSource.get();
        },
        getGenders:()=> [
          {caption:"Женский", value:0},
          {caption:"Мужской", value:1}
        ],
        canShowDoctor:()=> {
          return props.clinic?.bookingType == 1
        },
        canShowArmchair:() =>props.clinic?.bookingType == 0 && props.clinic?.useArmchairForBooking,
        getDoctors:()=> props.doctorDataSource.items.value,
        getServices:async ()=> {
          await props.serviceGroupDataSource.getRoot(props.clinic?.id!);
          return  props.serviceGroupDataSource.root.value[0].children
        },
      },
    }),
);

const reset = () => {
  model.serviceGroupId = null;
  model.booking = new Booking({
    clinicId: props.clinic?.id,
    patient: reactive({
      clinicId: props.clinic.id,
      fullName:"",
      whatsAppNumber:"",
    }),
  });
  visible.value = false;
  resolve.value!(null);
};

const open = async () => {
  visible.value = true;


  return new Promise((r) => (resolve.value = r));
};

const submit = async () => {
  if (!resolve.value) return;
  visible.value = false;
  let clone = _.cloneDeep({...model});
  delete clone.booking.id;
  resolve.value(clone.booking);
  reset();
  await patientDataSource.get();
};

defineExpose({ open });

const maximizable = ref(false);

const handleResize = () => {
  maximizable.value = window.innerWidth > 768;
};
onMounted(async () => {
  handleResize();
  window.addEventListener('resize', handleResize);
})
</script>

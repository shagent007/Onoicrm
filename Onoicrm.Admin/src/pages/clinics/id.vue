<template>
  <data-item :title="caption" :data-source="dataSource" :breadcrumbs="breadcrumbs" :editor-config="editorConfig" >
    <div class='grid'>
      <div class='col-3'>
        <div class='card'>
          <AppMenu :model="nav" />
        </div>
      </div>
      <div class='col-9'>
        <div class="card">
          <div v-if="route.name == 'clinic-id'">
            <h1>Информация о клинике</h1>
          </div>

          <router-view/>

        </div>
      </div>
    </div>
  </data-item>
</template>
<script setup lang="ts">
import { useRoute } from 'vue-router';
import DataItem from '../../components/data/item.vue';
import { StringField } from '@/app-form/editors/string/models/StringField';
import { InputFieldAttribute } from '@/app-form/models/InputFieldAttribute';
import { Clinic } from '@/entities/Clinic';
import { EditorDialogConfig } from '@/models/EditorDialogConfig';
import { required } from '@/services/consts';
import { useObjectDataSource } from '@/hooks/useObjectDataSource';
import { ObjectDataSourceConfig } from '@/models/ObjectDataSourceConfig';
import { computed, onMounted } from 'vue';
import AppMenu from '../../layouts/components/AppMenu.vue';
import { DateField } from '@/app-form/editors/date/models/DateField';
import { DateFieldAttribute } from '@/app-form/editors/date/models/DateFieldAttribute';
import moment from 'moment';
import { MaskField } from '@/app-form/editors/mask/models/MaskField';
import { MaskFieldConfig } from '@/app-form/editors/mask/models/MaskFieldConfig';
import { SelectField } from '@/app-form/editors/select/models/SelectField';
import { SelectFieldConfig } from '@/app-form/editors/select/models/SelectFieldConfig';
import { SelectMode } from '@/app-form/editors/select/models/SelectMode';
import { BoolField } from '@/app-form/editors/bool/models/BoolField';
const route = useRoute();
const dataSource = useObjectDataSource<Clinic>(new ObjectDataSourceConfig({ className: 'clinic', id: +route.params.id }));
const breadcrumbs = [
  {
    label: 'Клиники',
    to: '/clinics',
  },
  {
    label: dataSource.model.value?.caption,
  },
];
const nav = [
  {
    "items": [
      {
        "label": "Клиника",
        "icon": "pi pi-fw pi-home",
        "to": `/clinics/${route.params.id}`
      },
      {
        "label": "Сотрудники",
        "icon": "fas fa-user-doctor pl-1 pr-1",
        "to": `/clinics/${route.params.id}/doctors`
      },
      {
        "label": "Пациенты",
        "icon": "pi pi-fw pi-users",
        "to": `/clinics/${route.params.id}/patients`
      },
      {
        "label": "Кресла",
        "icon": "fas fa-chair pl-1 pr-1",
        "to": `/clinics/${route.params.id}/armchairs`
      },
      {
        "label": "Услуги",
        "icon": "fa-solid fa-wrench pl-1 pr-1",
        "to": `/clinics/${route.params.id}/services`
      }
    ]
  }
]

const editorConfig = new EditorDialogConfig({
  title: 'Обновить данные',
  model: new Clinic(),
  fields: [
    new StringField('caption', {
      attrs: new InputFieldAttribute({
        caption: 'Название',
      }),
      validations: [required()],
    }),
    new StringField('wappiProfileId', {
      attrs: new InputFieldAttribute({
        caption: 'Wappi Профиль',
      }),
      grid:"col-6"
    }),
    new StringField('wappiToken', {
      attrs: new InputFieldAttribute({
        caption: 'Wappi Токен',
      }),
      grid:"col-6"
    }),
    new MaskField('workStartTime', {
      attrs: new InputFieldAttribute({
        caption: 'Время начало работы',
      }),
      config: new MaskFieldConfig({
        mask: "##:##"
      }),
      grid:"col-6"
    }),
    new MaskField('workEndTime', {
      attrs: new InputFieldAttribute({
        caption: 'Время конца работы',
      }),
      config: new MaskFieldConfig({
        mask: "##:##"
      }),
      grid:"col-6"
    }),
    new MaskField('bookingTimeDuration', {
      attrs: new InputFieldAttribute({
        caption: 'Интервал между записями',
      }),
      config: new MaskFieldConfig({
        mask: "##"
      }),
      grid:"col-6"
    }),
    new DateField("expiredDate", {
      attrs: new DateFieldAttribute({
        caption: "Дата доступа",
      }),
      grid:"col-6",
      get: "getDate",
    }),
    new SelectField("bookingType",{
      attrs: new InputFieldAttribute({
        caption:"Тип записи"
      }),
      config: new SelectFieldConfig({
        getItems:"getBookingTypes",
        labelKeyName:"caption",
        valueKeyName:"value",
        mode:SelectMode.BtnGroup
      }),
      grid:"col-6",
    }),
    new BoolField("useArmchairForBooking",{
      attrs: new InputFieldAttribute({
        caption:"Использовать кресла для записи?"
      }),
      grid:"col-6",
    })
  ],
  actions :{
    getDate:(model:Clinic)=>{
      return moment(model.expiredDate).format("DD.MM.YYYY")
    },
    getBookingTypes:()=>[
      {
        caption: "По врачам",
        value:0
      },
      {
        caption: "По креслам",
        value:1
      }
    ],
  },
  updateFieldService: dataSource,
});

const caption = computed(() => dataSource.model.value?.caption ?? '');

onMounted(async () => {
  await dataSource.get();
  editorConfig.model = dataSource.model.value;
  dataSource.loading.value = false;
});
</script>

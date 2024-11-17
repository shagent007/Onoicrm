<template>
  <data-list v-if='loaded' :data-source="dataSource" :meta-info="config" :breadcrumbs="breadcrumbs" title="Клиники">
    <template #id-body="{ item }: any">
      <b>{{ item.id }}</b>
    </template>
    <template #caption-body="{ item }">
      <router-link :to="`/clinics/${item.id}`">
        {{ item.caption }}
      </router-link>
    </template>
    <template #expiredDate-body='{item}'>
      {{moment(item.expiredDate).format("DD.MM.YYYY")}}
    </template>
  </data-list>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { RouterLink } from 'vue-router';
import { StringField } from '@/app-form/editors/string/models/StringField';
import { InputFieldAttribute } from '@/app-form/models/InputFieldAttribute';
import DataList from '../../components/data/list.vue';
import { Clinic } from '@/entities/Clinic';
import { useListDataSource } from '@/hooks/useListDataSource';
import { EditorDialogConfig } from '@/models/EditorDialogConfig';
import { EntityMetaInfo } from '@/models/EntityMetaInfo/EntityMetaInfo';
import { ListConfig } from '@/models/ListConfig';
import { required } from '@/services/consts';
import { DateFieldAttribute } from '@/app-form/editors/date/models/DateFieldAttribute';
import { DateField } from '@/app-form/editors/date/models/DateField';
import moment from "moment"
import { TreeDataSourceConfig, useTreeDataSource } from '@/hooks/useTreeDataSource';
import { TreeSelectField } from '@/app-form/editors/tree-select/models/TreeSelectField';
import { MaskField } from '@/app-form/editors/mask/models/MaskField';
import { MaskFieldConfig } from '@/app-form/editors/mask/models/MaskFieldConfig';
const dataSource = useListDataSource(new ListConfig({ className: 'clinic' }));
const groupDataSource = useTreeDataSource( new TreeDataSourceConfig({ className:"groupUserProfile" }));
const loaded = ref<boolean>(false);
const breadcrumbs = [
  {
    label: 'Клиники',
  },
];
const actions = {
  getDate:(model:Clinic)=>{
    return moment(model.expiredDate).format("DD.MM.YYYY")
  },
  getGroupUserProfileConfig:() => {
    return {
      dataSource: groupDataSource
    }
  }
}

const config = new EntityMetaInfo({
  clasName: 'clinic',
  columns: [
    {
      field: 'id',
      header: '#',
      sortable: true,
    },
    {
      field: 'caption',
      header: 'Название клиники',
      sortable: true,
    },
    {
      field: 'expiredDate',
      header: 'Дата доступа',
      sortable: true,
    },
  ],
  createItemInfo: new EditorDialogConfig({
    title: 'Добавить новую клинику',
    model: new Clinic(),
    fields: [
      new StringField('caption', {
        attrs: new InputFieldAttribute({
          caption: 'Название',
          placeholder: 'Введите название клиники',
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
      })
    ],
    actions
  }),
  updateItemInfo: new EditorDialogConfig({
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
    ],
    actions,
    updateFieldService: dataSource,
  }),
});

onMounted(async () => {
  await Promise.all([dataSource.get()]);
  loaded.value = true;
});
</script>

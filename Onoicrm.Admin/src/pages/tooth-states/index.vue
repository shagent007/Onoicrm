<template>
  <data-list v-if='loaded' :data-source="dataSource" :meta-info="config" :breadcrumbs="breadcrumbs" title="Зубы">
    <template #name-body="{ item }: any">
      <router-link :to="`/clinics/${item?.id}`">
        {{ item?.name }}
      </router-link>
    </template>
    <template #color-body="{ item }: any">
      <div
        class='w-2rem h-2rem border-round'
        :style='`background-color:${item?.color};`'
      />
    </template>
  </data-list>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { RouterLink } from 'vue-router';
import { StringField } from '@/app-form/editors/string/models/StringField';
import { InputFieldAttribute } from '@/app-form/models/InputFieldAttribute';
import DataList from '@/components/data/list.vue';
import { useListDataSource } from '@/hooks/useListDataSource';
import { EditorDialogConfig } from '@/models/EditorDialogConfig';
import { EntityMetaInfo } from '@/models/EntityMetaInfo/EntityMetaInfo';
import { ListConfig } from '@/models/ListConfig';
import { required } from '@/services/consts';
import { Tooth } from '@/entities/Tooth';
import { MemoField } from '@/app-form/editors/memo/models/MemoField';
import { SelectField } from '@/app-form/editors/select/models/SelectField';
import { SelectFieldConfig } from '@/app-form/editors/select/models/SelectFieldConfig';
import { SelectMode } from '@/app-form/editors/select/models/SelectMode';

const dataSource = useListDataSource(new ListConfig({ className: 'toothState' }));
const loaded = ref<boolean>(false);
const breadcrumbs = [
  {
    label: 'Зубы',
  },
];
const fields = [
  new StringField('caption', {
    attrs: new InputFieldAttribute({
      caption: 'Название',
      placeholder: 'Введите состояние зуба',
    }),
    validations: [required()],
  }),
  new SelectField("color",{
    attrs: new InputFieldAttribute({
      caption:"Выберите условный цвет"
    }),
    config: new SelectFieldConfig({
      getItems:"getColors",
      mode:SelectMode.Color
    })
  }),
  new MemoField('description', {
    attrs: new InputFieldAttribute({
      caption: 'Описание',
      placeholder: 'Опишите состояние',
    }),
  }),
]


const actions:any ={
  getColors:()=>['#F6648B', '#F493A7','#94d49b']
}



const config = new EntityMetaInfo({
  clasName: 'tooth',
  columns: [
    {
      field: 'id',
      header: '#',
      sortable: true,
    },
    {
      field: 'caption',
      header: 'Название',
      sortable: true,
    },
    {
      field: 'color',
      header: 'Условный цвет',
    },
  ],
  createItemInfo: new EditorDialogConfig({
    title: 'Добавить новое состояние',
    model: new Tooth(),
    fields,
    actions
  }),
  updateItemInfo: new EditorDialogConfig({
    title: 'Редактировать состояние',
    model: new Tooth(),
    fields,
    updateFieldService: dataSource,
    actions
  }),
});

onMounted(async () => {
  await Promise.all([dataSource.get()]);
  loaded.value = true;
});
</script>

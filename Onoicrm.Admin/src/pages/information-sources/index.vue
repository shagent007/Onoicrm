<template>
  <data-list v-if='loaded' :data-source="dataSource" :meta-info="config" :breadcrumbs="breadcrumbs" title="Источники информации"/>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { StringField } from '@/app-form/editors/string/models/StringField';
import { InputFieldAttribute } from '@/app-form/models/InputFieldAttribute';
import DataList from '../../components/data/list.vue';
import { useListDataSource } from '@/hooks/useListDataSource';
import { EditorDialogConfig } from '@/models/EditorDialogConfig';
import { EntityMetaInfo } from '@/models/EntityMetaInfo/EntityMetaInfo';
import { ListConfig } from '@/models/ListConfig';
import { required } from '@/services/consts';
import { MemoField } from '@/app-form/editors/memo/models/MemoField';
import { Symptom } from '@/entities/Symptom';
const dataSource = useListDataSource(new ListConfig({ className: 'InformationSource' }));
const loaded = ref<boolean>(false);
const breadcrumbs = [
  {
    label: 'Источники информации',
  },
];

const config = new EntityMetaInfo({
  clasName: 'informationSource',
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
      field: 'description',
      header: 'Описание',
      sortable: true,
    },
  ],
  createItemInfo: new EditorDialogConfig({
    title: 'Добавить новый источник',
    model: new Symptom(),
    fields: [
      new StringField('caption', {
        attrs: new InputFieldAttribute({
          caption: 'Название',
          placeholder: 'Введите название',
        }),
        validations: [required()],
      }),
      new MemoField('description', {
        attrs: new InputFieldAttribute({
          caption: 'Описание',
        }),
      }),
    ],
  }),
  updateItemInfo: new EditorDialogConfig({
    title: 'Обновить данные',
    model: new Symptom(),
    fields: [
      new StringField('caption', {
        attrs: new InputFieldAttribute({
          caption: 'Название',
          placeholder: 'Введите название',
        }),
        validations: [required()],
      }),
      new MemoField('description', {
        attrs: new InputFieldAttribute({
          caption: 'Описание',
        }),
      }),
    ],
    updateFieldService: dataSource,
  }),
});

onMounted(async () => {
  await Promise.all([dataSource.get()]);
  loaded.value = true;
});
</script>

<template>
  <data-list
    v-if='loaded'
    :data-source="dataSource"
    :meta-info="config"
    :breadcrumbs="breadcrumbs"
    title="Причины отмены"
  />
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { StringField } from '@/app-form/editors/string/models/StringField';
import { InputFieldAttribute } from '../../app-form/models/InputFieldAttribute';
import DataList from '../../components/data/list.vue';
import { useListDataSource } from '../../hooks/useListDataSource';
import { EditorDialogConfig } from '../../models/EditorDialogConfig';
import { EntityMetaInfo } from '../../models/EntityMetaInfo/EntityMetaInfo';
import { ListConfig } from '../../models/ListConfig';
import { required } from '../../services/consts';
import { MemoField } from '../../app-form/editors/memo/models/MemoField';
import { CancellationReason } from '@/entities/CancellationReason';
const dataSource = useListDataSource(new ListConfig({ className: 'cancellationReason' }));
const loaded = ref<boolean>(false);
const breadcrumbs = [
  {
    label: 'Причины отмены',
  },
];
const fields = [
  new StringField('caption', {
    attrs: new InputFieldAttribute({
      caption: 'Заголовок',
      placeholder: 'Введите  заголовок',
    }),
    validations: [required()],
  }),
  new MemoField('description', {
    attrs: new InputFieldAttribute({
      caption: 'Описание',
      placeholder: 'Опишите',
    }),
  }),
]




const config = new EntityMetaInfo({
  clasName: 'CancellationReason',
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
    }
  ],
  createItemInfo: new EditorDialogConfig({
    title: 'Добавить причину',
    model: new CancellationReason(),
    fields,
  }),
  updateItemInfo: new EditorDialogConfig({
    title: 'Редактировать причину',
    model: new CancellationReason(),
    fields,
    updateFieldService: dataSource,
  }),
});

onMounted(async () => {
  await Promise.all([dataSource.get()]);
  loaded.value = true;
});
</script>

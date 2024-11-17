<template>
  <data-list v-if='loaded' :data-source="dataSource" :meta-info="config" :breadcrumbs="breadcrumbs" title="Зубы">
    <template #caption-body="{ item }: any">
      <router-link :to="`/teeth/${item.id}`">
        {{ item.caption }}
      </router-link>
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
import { NumberField } from '@/app-form/editors/Number/models/NumberField';
const dataSource = useListDataSource(new ListConfig({ className: 'tooth' }));
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
      placeholder: 'Введите название Зуба',
    }),
    validations: [required()],
  }),
  new NumberField('position', {
    attrs: new InputFieldAttribute({
      caption: 'Номер',
      placeholder: 'Введите номер зуба',
    }),
    validations: [required()],
    grid:"col-6"
  }),
  new NumberField('quarter', {
    attrs: new InputFieldAttribute({
      caption: 'Четверть',
      placeholder: 'Введите четверть зуба',
    }),
    validations: [required()],
    grid:"col-6"
  }),
  new MemoField('description', {
    attrs: new InputFieldAttribute({
      caption: 'Описание',
      placeholder: 'Опишите зуб',
    }),
  }),
]




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
      field: 'position',
      header: 'Номер',
      sortable: true,
    },
    {
      field: 'quarter',
      header: 'Четверть',
      sortable: true,
    },
  ],


  createItemInfo: new EditorDialogConfig({
    title: 'Добавить новый зуб',
    model: new Tooth(),
    fields,
  }),
  updateItemInfo: new EditorDialogConfig({
    title: 'Редактировать зуб',
    model: new Tooth(),
    fields,
    updateFieldService: dataSource,
  }),
});

onMounted(async () => {
  await Promise.all([dataSource.get()]);
  loaded.value = true;
});
</script>

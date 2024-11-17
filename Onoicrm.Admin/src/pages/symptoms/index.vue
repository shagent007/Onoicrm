<template>
  <data-list v-if='loaded' :data-source="dataSource" :meta-info="config" :breadcrumbs="breadcrumbs" title="Симптомы">
    <template #name-body="{ item }: {item:Symptom}">
      <router-link :to="`/symptom/${item.id}`">
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
import { MemoField } from '@/app-form/editors/memo/models/MemoField';
import { Symptom } from '@/entities/Symptom';
const dataSource = useListDataSource(new ListConfig({ className: 'symptom' }));
const loaded = ref<boolean>(false);
const breadcrumbs = [
  {
    label: 'Симптомы',
  },
];

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
      field: 'description',
      header: 'Описание',
      sortable: true,
    },
  ],
  createItemInfo: new EditorDialogConfig({
    title: 'Добавить новый симптом',
    model: new Symptom(),
    fields: [
      new StringField('caption', {
        attrs: new InputFieldAttribute({
          caption: 'Название',
          placeholder: 'Введите название клиники',
        }),
        validations: [required()],
      }),
      new MemoField('description', {
        attrs: new InputFieldAttribute({
          caption: 'Описание',
          placeholder: 'Опишите симптом',
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
          placeholder: 'Введите название клиники',
        }),
        validations: [required()],
      }),
      new MemoField('description', {
        attrs: new InputFieldAttribute({
          caption: 'Описание',
          placeholder: 'Опишите симптом',
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

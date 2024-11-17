<template>
  <data-item :title="caption" :data-source="dataSource" :breadcrumbs="breadcrumbs" :editor-config="editorConfig" />
</template>
<script setup lang="ts">
import { useRoute } from 'vue-router';
import DataItem from '../../components/data/item.vue';
import { StringField } from '../../app-form/editors/string/models/StringField';
import { InputFieldAttribute } from '../../app-form/models/InputFieldAttribute';
import { Clinic } from '../../entities/Clinic';
import { EditorDialogConfig } from '../../models/EditorDialogConfig';
import { required } from '../../services/consts';
import { useObjectDataSource } from '../../hooks/useObjectDataSource';
import { ObjectDataSourceConfig } from '../../models/ObjectDataSourceConfig';
import { computed, onMounted } from 'vue';
const route = useRoute();

const dataSource = useObjectDataSource<Clinic>(new ObjectDataSourceConfig({ className: 'clinic', id: +route.params.id }));
const breadcrumbs = [
  {
    label: 'Клиники',
    to: '/clinics',
  },
  {
    label: dataSource.model.value?.name,
  },
];
const editorConfig = new EditorDialogConfig({
  title: 'Обновить данные',
  model: new Clinic(),
  fields: [
    new StringField('name', {
      attrs: new InputFieldAttribute({
        caption: 'Название',
      }),
      validations: [required()],
    }),
  ],
  updateFieldService: dataSource,
});

const caption = computed(() => dataSource.model.value?.name ?? '');

onMounted(async () => {
  await dataSource.get();
  editorConfig.model = dataSource.model.value;
  dataSource.loading.value = false;
});
</script>

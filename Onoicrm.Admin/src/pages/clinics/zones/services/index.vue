<template>
  <data-list
    v-if="loaded"
    :custom-sort="sort"
    :data-source="dataSource"
    :meta-info="config"
    title="Услуги"
  />
</template>

<script setup lang="ts">
import DataList from '@/components/data/list.vue';
import { DataTableSortEvent } from 'primevue/datatable';
import { Ref, ref, onMounted } from 'vue';
import { StringField } from '@/app-form/editors/string/models/StringField';
import { InputFieldAttribute } from '@/app-form/models/InputFieldAttribute';
import { Clinic } from '@/entities/Clinic';
import { useListDataSource } from '@/hooks/useListDataSource';
import { EditorDialogConfig } from '@/models/EditorDialogConfig';
import { EntityMetaInfo } from '@/models/EntityMetaInfo/EntityMetaInfo';
import { ListConfig } from '@/models/ListConfig';
import { required } from '@/services/consts';
import { toServerCase } from '@/services/helpers';
import { Filter } from '@/models/Filter';
import { useRoute } from 'vue-router';
import { Service } from '@/entities/Service';
import { NumberField } from '@/app-form/editors/Number/models/NumberField';


const route = useRoute();
const dataSource = useListDataSource(new ListConfig({ className: 'service',filter:[
  new Filter("clinicId", +route.params.id),
]}));


const loaded: Ref<boolean> = ref(false);
const config = new EntityMetaInfo({
  clasName: 'service',
  columns: [
    {
      field: 'id',
      header: '#',
      sortable: true,
    },
    {
      field: 'caption',
      header: 'Наименование',
      filterField: 'caption',
      sortable: true,
    },
    {
      field: 'price',
      header: 'Цена',
      filterField: 'price',
      sortable: true,
    },
    {
      field: 'stateId',
      header: 'Статус',
    },
  ],
  filters: {
    caption: '',
    price:"",
  },
  createItemInfo: new EditorDialogConfig({
    title: 'Добавить новую услугу',
    model: new Service({
      clinicId: +route.params.id
    }),
    fields: [
      new StringField('caption', {
        attrs: new InputFieldAttribute({
          caption: 'Наименование',
          placeholder: 'Придумайте логин',
        }),
        validations: [required()],
      }),
      new StringField('price', {
        attrs: new InputFieldAttribute({
          caption: 'Цена',
        }),
        grid:"col-4",
      }),
      new StringField('priceFrom', {
        attrs: new InputFieldAttribute({
          caption: 'Цена от',
        }),
        grid:"col-4",
      }),
      new StringField('priceTo', {
        attrs: new InputFieldAttribute({
          caption: 'Цена до',
        }),
        grid:"col-4",
      }),
    ],
  }),
  updateItemInfo: new EditorDialogConfig({
    title: 'Обновить данные',
    model: new Clinic(),
    fields: [
      new StringField('caption', {
        attrs: new InputFieldAttribute({
          caption: 'Наименование',
        }),
        validations: [required()],
      }),
      new NumberField('price', {
        attrs: new InputFieldAttribute({
          caption: 'Цена от',
        }),
        grid:"col-4",
      }),
      new NumberField('priceFrom', {
        attrs: new InputFieldAttribute({
          caption: 'Цена от',
        }),
        grid:"col-4",
      }),
      new NumberField('priceTo', {
        attrs: new InputFieldAttribute({
          caption: 'Цена до',
        }),
        grid:"col-4",
      }),
    ],
    updateFieldService: dataSource,
  }),
});

onMounted(async () => {
  await Promise.all([
    dataSource.get(),
  ]);
  loaded.value = true;
});

const sort = async (event: DataTableSortEvent) => {
  let sortField: string = event.sortField == 'fullName' ? 'firstName' : (event.sortField as string);
  dataSource.orderFieldName.value = toServerCase(sortField);
  dataSource.orderFieldDirection.value = event.sortOrder == 1 ? 'ASC' : 'DESC';
  await dataSource.get();
};
</script>

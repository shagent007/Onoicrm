<template>
  <data-list v-if='loaded' :data-source="dataSource" :meta-info="config" title="Креслы"/>

</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import { StringField } from '../../../../app-form/editors/string/models/StringField';
import { InputFieldAttribute } from '../../../../app-form/models/InputFieldAttribute';
import DataList from '@/components/data/list.vue';
import { useListDataSource } from '../../../../hooks/useListDataSource';
import { EditorDialogConfig } from '../../../../models/EditorDialogConfig';
import { EntityMetaInfo } from '../../../../models/EntityMetaInfo/EntityMetaInfo';
import { ListConfig } from '../../../../models/ListConfig';
import { required } from '../../../../services/consts';
import { DateFieldAttribute } from '../../../../app-form/editors/date/models/DateFieldAttribute';
import { Armchair } from '../../../../entities/Armchair';
import { MemoField } from '../../../../app-form/editors/memo/models/MemoField';
import { Filter } from '../../../../models/Filter';
const loaded = ref<boolean>(false);

const route = useRoute();
const dataSource = useListDataSource(new ListConfig({
  className: 'armchair',
  filter: [new Filter("clinicId", +route.params.id)]
}));

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
    title: 'Добавить новое кресло',
    model: new Armchair({
      caption: `Кресло-${dataSource.items.value.length}`,
      clinicId: +route.params.id,
    }),
    fields: [
      new StringField('caption', {
        attrs: new InputFieldAttribute({
          caption: 'Название',
          placeholder: 'Введите название стола',
        }),
        validations: [required()],
      }),
      new MemoField("description", {
        attrs: new DateFieldAttribute({
          caption: "Описание",
        }),
      }),
    ],
  }),
  updateItemInfo: new EditorDialogConfig({
    title: 'Редактировать кресло',
    model: new Armchair(),
    fields: [
      new StringField('caption', {
        attrs: new InputFieldAttribute({
          caption: 'Название',
          placeholder: 'Введите название стола',
        }),
        validations: [required()],
      }),
      new MemoField("description", {
        attrs: new DateFieldAttribute({
          caption: "Описание",
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

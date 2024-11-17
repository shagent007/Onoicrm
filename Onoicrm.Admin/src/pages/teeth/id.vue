<template>
  <data-list
    v-if='loaded'
    :data-source="dataSource"
    :meta-info="config"
    :breadcrumbs="breadcrumbs"
    :title="title"
  />
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { StringField } from '@/app-form/editors/string/models/StringField';
import { InputFieldAttribute } from '@/app-form/models/InputFieldAttribute';
import DataList from '@/components/data/list.vue';
import { useListDataSource } from '@/hooks/useListDataSource';
import { EditorDialogConfig } from '@/models/EditorDialogConfig';
import { EntityMetaInfo } from '@/models/EntityMetaInfo/EntityMetaInfo';
import { ListConfig } from '@/models/ListConfig';
import { required } from '@/services/consts';
import { MemoField } from '@/app-form/editors/memo/models/MemoField';
import { Channel } from '@/entities/Channel';
import { useRoute } from 'vue-router';
import { useObjectDataSource } from '@/hooks/useObjectDataSource';
import { ObjectDataSourceConfig } from '@/models/ObjectDataSourceConfig';
import { Filter } from '@/models/Filter';


const loaded = ref<boolean>(false);
const route = useRoute();
const dataSource = useListDataSource(new ListConfig({
  className: 'channel',
  filter:[
    new Filter("toothId", +route.params.id)
  ]
}));

const breadcrumbs = [
  {
    label: 'Зубы',
    to:"/teeth"
  },
  {
    label: "Каналы"
  }
];
const objectDataSource = useObjectDataSource(
  new ObjectDataSourceConfig({
    id: +route.params.id,
    className: "tooth"
  })
)

const title = computed(()=>{
  const tooth:any = objectDataSource.model.value;
  if(!tooth) return "";
  let toothSide = "";
  switch (tooth.quarter) {
    case 1:
      toothSide = "верхней правой челюсти";
      break;
    case 2:
      toothSide = "верхней левой челюсти";
      break;
    case 3:
      toothSide = "нижней правой челюсти";
      break;
    case 4:
      toothSide = "нижней левой челюсти";
      break;
    default:
      toothSide = "неизвестной челюсти";
  }

  return `${tooth.caption} ${toothSide} №${tooth.position}`;
})

const fields = [
  new StringField('caption', {
    attrs: new InputFieldAttribute({
      caption: 'Название',
      placeholder: 'Введите название канала',
    }),
    validations: [required()],
  }),
  new MemoField('description', {
    attrs: new InputFieldAttribute({
      caption: 'Описание',
      placeholder: 'Опишите канал',
    }),
  }),
]



const config = new EntityMetaInfo({
  clasName: 'channel',
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
    }
  ],
  createItemInfo: new EditorDialogConfig({
    title: 'Добавить канал',
    model: new Channel({
      toothId: +route.params.id
    }),
    fields,
  }),
  updateItemInfo: new EditorDialogConfig({
    title: 'Редактировать канал',
    model: new Channel(),
    fields,
    updateFieldService: dataSource,
  }),
});

onMounted(async () => {
  await Promise.all([
    dataSource.get(),
    objectDataSource.get()
  ]);
  loaded.value = true;
});
</script>

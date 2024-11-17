<template>
  <data-list
    v-if="loaded"
    :custom-sort="sort"
    :data-source="dataSource"
    :meta-info="config"
    title="Пациенты"
  />
</template>

<script setup lang="ts">
import DataList from '../../../../components/data/list.vue';
import { DataTableSortEvent } from 'primevue/datatable';
import { Ref, ref, onMounted } from 'vue';
import { PasswordField } from '@/app-form/editors/password/models/PasswordField';
import { StringField } from '@/app-form/editors/string/models/StringField';
import { InputFieldAttribute } from '@/app-form/models/InputFieldAttribute';
import { Clinic } from '@/entities/Clinic';
import { UserProfile } from '@/entities/UserProfile';
import { useListDataSource } from '@/hooks/useListDataSource';
import { EditorDialogConfig } from '@/models/EditorDialogConfig';
import { EntityMetaInfo } from '@/models/EntityMetaInfo/EntityMetaInfo';
import { ListConfig } from '@/models/ListConfig';
import { UserRegisterModel } from '@/models/UserResisterModel';
import { required,  min, requireUpperCaseLatin, requireLowerCaseLatin, requireSymbol, like } from '@/services/consts';
import { toServerCase } from '@/services/helpers';
import { MaskField } from '@/app-form/editors/mask/models/MaskField';
import { MaskFieldConfig } from '@/app-form/editors/mask/models/MaskFieldConfig';
import { MemoField } from '@/app-form/editors/memo/models/MemoField';
import { Filter } from '@/models/Filter';
import { useRoute } from 'vue-router';


const route = useRoute();
const dataSource = useListDataSource(new ListConfig({ className: 'userProfile',filter:[
  new Filter("clinicId", +route.params.id),
  new Filter("roleNames","Patient")
]}));
const loaded: Ref<boolean> = ref(false);
const config = new EntityMetaInfo({
  clasName: 'userProfile',
  columns: [
    {
      field: 'id',
      header: '#',
      sortable: true,
    },
    {
      field: 'fullName',
      header: 'ФИО',
      filterField: 'fullName',
      sortable: true,
    },
    {
      field: 'whatsAppNumber',
      header: 'Номер WhatsApp',
      filterField: 'whatsAppNumber',
      sortable: true,
    },
    {
      field: 'stateId',
      header:"Статус"
    },
  ],
  filters: {
    fullName: '',
    whatsAppNumber:""
  },
  createItemInfo: new EditorDialogConfig({
    title: 'Жаны адамды кошуу',
    model: new UserRegisterModel({
      userProfile: new UserProfile({}),
    }),
    fields: [
      new StringField('email', {
        attrs: new InputFieldAttribute({
          caption: 'Логин',
          placeholder: 'Придумайте логин',
        }),
        validations: [required()],
      }),
      new PasswordField('password', {
        attrs: new InputFieldAttribute({
          caption: 'Пароль',
          placeholder: 'Введите пароль',
        }),
        grid:"col-6",
        validations: [required(), min(8), requireUpperCaseLatin(), requireLowerCaseLatin(), requireSymbol()],
      }),
      new StringField('confirmPassword', {
        attrs: new InputFieldAttribute({
          caption: 'Подтверждение пароля',
          placeholder: 'Подтвердите пароль',
        }),
        grid:"col-6",
        validations: [required(), like('password', 'Не совподает с ведённым паролем')],
      }),
      new StringField('userProfile.firstName', {
        attrs: new InputFieldAttribute({
          caption: 'Имя',
        }),
        grid:"col-6",
        validations: [required()],
      }),
      new StringField('userProfile.lastName', {
        attrs: new InputFieldAttribute({
          caption: 'Фамилия',
        }),
        grid:"col-6",
        validations: [required()],
      }),
      new StringField('userProfile.patronymic', {
        attrs: new InputFieldAttribute({
          caption: 'Отчество',
        }),
      }),
      new MaskField('userProfile.phoneNumber', {
        attrs: new InputFieldAttribute({
          caption: 'Номер телефона',
        }),
        config: new MaskFieldConfig({
          mask: "+996 (###) ##-##-##",
        }),
        grid:"col-6",
      }),
      new MaskField('userProfile.whatsAppNumber', {
        attrs: new InputFieldAttribute({
          caption: 'WhatsApp номер',
        }),
        config: new MaskFieldConfig({
          mask: "+996 (###) ##-##-##",
        }),
        grid:"col-6",
      }),
      new StringField('userProfile.address', {
        attrs: new InputFieldAttribute({
          caption: 'Адрес',
        }),
      }),
      new MemoField('userProfile.description', {
        attrs: new InputFieldAttribute({
          caption: 'Описание',
        }),
      })
    ],
  }),
  updateItemInfo: new EditorDialogConfig({
    title: 'Обновить данные',
    model: new Clinic(),
    fields: [
      new StringField('firstName', {
        attrs: new InputFieldAttribute({
          caption: 'Имя',
        }),
        grid:"col-6",
        validations: [required()],
      }),
      new StringField('lastName', {
        attrs: new InputFieldAttribute({
          caption: 'Фамилия',
        }),
        grid:"col-6",
        validations: [required()],
      }),
      new StringField('patronymic', {
        attrs: new InputFieldAttribute({
          caption: 'Отчество',
        }),
      }),
      new MaskField('phoneNumber', {
        attrs: new InputFieldAttribute({
          caption: 'Номер телефона',
        }),
        config: new MaskFieldConfig({
          mask: "+996 (###) ##-##-##",
        }),
        grid:"col-6",
      }),
      new MaskField('whatsAppNumber', {
        attrs: new InputFieldAttribute({
          caption: 'WhatsApp номер',
        }),
        config: new MaskFieldConfig({
          mask: "+996 (###) ##-##-##",
        }),
        grid:"col-6",
      }),
      new StringField('address', {
        attrs: new InputFieldAttribute({
          caption: 'Адрес',
        }),
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

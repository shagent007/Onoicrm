<template>
  <data-list
    v-if="loaded"
    :custom-sort="sort"
    :data-source="dataSource"
    :meta-info="config"
    title="Сотрудники"
  >
    <template #roles-body="{ item }">
      <Tag
        v-for="role in item.roles"
        class='mr-1 mb-1'
        severity="primary"
        :key='role'
        rounded
      >
        {{ role }}
      </Tag>
    </template>
  </data-list>
</template>

<script setup lang="ts">
import DataList from '../../../../components/data/list.vue';
import { DataTableSortEvent } from 'primevue/datatable';
import { Ref, ref, onMounted } from 'vue';
import { PasswordField } from '@/app-form/editors/password/models/PasswordField';
import { StringField } from '@/app-form/editors/string/models/StringField';
import { InputFieldAttribute } from '@/app-form/models/InputFieldAttribute';
import { UserProfile } from '@/entities/UserProfile';
import { SetRoleModel, useListDataSource, useUserRoleDataSource } from '@/hooks/useListDataSource';
import { EditorDialogConfig } from '@/models/EditorDialogConfig';
import { EntityMetaInfo } from '@/models/EntityMetaInfo/EntityMetaInfo';
import { ListConfig } from '@/models/ListConfig';
import { UserRegisterModel } from '@/models/UserResisterModel';
import { required,  min, requireUpperCaseLatin, requireLowerCaseLatin, requireSymbol, like } from '@/services/consts';
import { remove, toServerCase } from '@/services/helpers';
import { MaskField } from '@/app-form/editors/mask/models/MaskField';
import { MaskFieldConfig } from '@/app-form/editors/mask/models/MaskFieldConfig';
import { Filter } from '@/models/Filter';
import { useRoute } from 'vue-router';
import { CollectionField } from '@/app-form/collections/models/CollectionField';
import { FieldAttribute } from '@/app-form/models/FieldAttribute';
import { CollectionChipItem } from '@/app-form/collections/items/models/CollectionChipItem';
const userRoleDataSource = useUserRoleDataSource();
const roleDataSource = useListDataSource(
  new ListConfig({
    className:'role',
    pageSize:100
  })
)
const route = useRoute();
const dataSource = useListDataSource(
  new ListConfig({
    className: 'userProfile',
    filter:[
      new Filter("clinicId", +route.params.id),
      new Filter("","Patient")
    ]
  })
);
const loaded: Ref<boolean> = ref(false);
const actions = {
  getRolesConfig:()=>{
    const metaInfo =  new EntityMetaInfo({
      columns:[
        {
          field:  "name",
          header: 'Название роли',
          sortable:true
        }
      ],
    })
    return {dataSource:roleDataSource, metaInfo, customSort:null}
  },
  getRoleItemConfig:()=>({ getCaption:(item:any)=>item.name }),
  getUpdateRoleItemConfig:()=>({getCaption:(item:any)=>item}),
  getUpdateRoleExcludeCallBack:(item:any)=> (i:any) => i == item.name,
  addRole: async (role:any, model:any)=>{
    await userRoleDataSource.setRole(new SetRoleModel({
      roleName: role.name,
      userId: model.userId
    }))
    model.roles.push(role.name);
  },
  deleteRole: async (role:any, model:any)=>{
    await userRoleDataSource.deleteRole(new SetRoleModel({
      roleName: role,
      userId: model.userId
    }))
    remove(model.roles, (r:any) => r == role)
  },
}
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
      field: 'roles',
      header: 'Роли',
    },
    {
      field: 'stateId',
      header:"Статус"
    },
  ],
  filters: {
    fullName: ''
  },
  createItemInfo: new EditorDialogConfig({
    title: 'Жаны адамды кошуу',
    model: new UserRegisterModel({
      userProfile: new UserProfile({
        clinicId:+route.params.id,
        whatsAppNumber:"996"
      })
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
        validations: [
          required(),
          min(8),
          requireUpperCaseLatin(),
          requireLowerCaseLatin(),
          requireSymbol()
        ],
      }),
      new PasswordField('confirmPassword', {
        attrs: new InputFieldAttribute({
          caption: 'Подтверждение пароля',
          placeholder: 'Подтвердите пароль',
        }),
        grid:"col-6",
        validations: [
          required(),
          like('password', 'Не совподает с ведённым паролем')
        ],
      }),
      new StringField('userProfile.fullName', {
        attrs: new InputFieldAttribute({
          caption: 'ФИО',
        }),
        grid:"col-6",
        validations: [required()],
      }),
      new MaskField('userProfile.whatsAppNumber', {
        attrs: new InputFieldAttribute({
          caption: 'WhatsApp номер',
        }),
        config: new MaskFieldConfig({
          mask: "+996 (###) ##-##-##",
        }),
        initialValue:"996",
        grid:"col-6",
      }),
      new CollectionField("roles",{
        attrs: new FieldAttribute({
          caption:"Роли"
        }),
        getConfig:"getRolesConfig",
        getItemConfig:"getRoleItemConfig",
        item: new CollectionChipItem({
          attrs: {
            class:"mr-2 mb-2"
          }
        })
      }),
    ],
    actions,
  }),
  updateItemInfo: new EditorDialogConfig({
    title: 'Обновить данные',
    model: new UserProfile(),
    fields: [
      new StringField('fullName', {
        attrs: new InputFieldAttribute({
          caption: 'ФИО',
        }),
        grid:"col-6",
        validations: [required()],
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
      new CollectionField("roles",{
        attrs: new FieldAttribute({
          caption:"Роли"
        }),
        getConfig:"getRolesConfig",
        customAdd:"addRole",
        customDelete:"deleteRole",
        getItemConfig:"getUpdateRoleItemConfig",
        getCustomExcludeCallBack:"getUpdateRoleExcludeCallBack",
        item: new CollectionChipItem({
          attrs: {
            class:"mr-2 mb-2"
          }
        })
      }),
    ],
    actions,
    updateFieldService: dataSource,

  }),
});

onMounted(async () => {
  await Promise.all([
    dataSource.get(),
    roleDataSource.get(),
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

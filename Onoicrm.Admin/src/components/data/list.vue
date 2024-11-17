<template>
  <div class="page-head mb-3">
    <h1>{{ title }}</h1>
    <Breadcrumb v-if='breadcrumbs.length' :home="home" :model="breadcrumbs" />
  </div>
  <div class='grid'>
    <div v-if='groupDataSource' class='col-3 flex'>
      <data-tree @un-select='clearFilter()' @select='updateFilter($event)' v-slot='{branch}' :props='{metaKeySelection:false, selectionMode:"single"}' :data-source='groupDataSource'>
        <span @contextmenu.prevent='branch.toggle($event); branch.selectNode(branch.node)'>{{branch.node.label}}</span>
      </data-tree>
    </div>
    <div :class='groupDataSource ? "col-9" : "col-12"'>
      <div class="card mb-0">
        <DataTable
          :value="dataSource.items.value"
          :paginator="true"
          :lazy="true"
          :rows="dataSource.pageSize.value"
          :total-records="dataSource.total.value"
          @sort="sort"
          filterDisplay="row"
          @filter="filter"
          v-model:filters="filters"
          :globalFilterFields="['fullName']"
          @page="paginate"
          class="p-datatable-sm"
          paginatorTemplate="CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
          :rowsPerPageOptions="[5, 10, 20, 50]"
          currentPageReportTemplate="{first} - {last} из {totalRecords}"
        >
          <template #header>
            <div class="flex justify-content-end">
              <Button @click="add" label="Добавить" icon="pi pi-plus" />
            </div>
          </template>
          <template #empty> No customers found. </template>
          <template #loading> Loading customers data. Please wait. </template>
          <template v-for="column in metaInfo.columns"  :key="column.field">
            <Column v-if="column.field=='stateId'" v-bind="column">
              <template #body="{ data }">
                <slot :item="data" :name="`${column.field}-body`">
                  <Tag
                    :severity='getStateColor(getValue(data, column?.field))'
                    :value="getStateCaption(getValue(data, column?.field))"
                  />
                </slot>
              </template>
            </Column>
            <Column v-else  v-bind="column">
              <template #body="{ data }">
                <slot :item="data" :name="`${column.field}-body`">
                  {{ getValue(data, column?.field) }}
                </slot>
              </template>
              <template v-if="column.filterField" #filter="{ filterModel, filterCallback }">
                <slot :name="`${column.filterField}-filter`" :item="{ callBack: filterCallback, model: filterModel }">
                  <InputText @change="filterCallback" v-model="filterModel.value" type="text" />
                </slot>
              </template>
            </Column>
          </template>

          <Column :exportable="false" style="width: 3rem">
            <template #body="{ data }">
              <div class="flex align-items-center justify-content-end">
                <Button
                  @click="openMenu($event, data)"
                  icon="pi pi-ellipsis-v"
                  aria-haspopup="true"
                  aria-controls="overlay_tmenu"
                  class="p-button-rounded p-button-info p-button-outlined p-button-sm"
                />
              </div>
            </template>
          </Column>
        </DataTable>
        <Menu id="overlay_tmenu" ref="menu" :model="menuItems" :popup="true">
          <template #item="{ item }">
            <a @click="menuHandle(item, $event)" class="p-menuitem-link" aria-haspopup="false" aria-expanded="true" role="menuitem" tabindex="0">
              <span class="p-menuitem-icon" :class="item.icon"></span>
              <span class="p-menuitem-text">{{ item.label }}</span>
            </a>
          </template>
        </Menu>
      </div>
    </div>
  </div>


</template>

<script setup lang="ts">
import DataTree from "./tree.vue"
import { inject,  Ref, ref } from 'vue';
import { IListDataSource } from '@/models/IListDataSource';
import { EntityMetaInfo } from '@/models/EntityMetaInfo/EntityMetaInfo';
import { useEditorStore } from '@/store/useEditorDialog';
import { Confirm, defaultConfirm } from '@/models/Confirm';
import { defaultMessage, IMessage } from '@/models/IMessage';
import { getValue, toFilterArray, toPascalCase } from '@/services/helpers';
import { DataTableSortEvent, DataTablePageEvent, DataTableFilterMeta } from 'primevue/datatable';
import { ITreeDataSource } from '@/models/ITreeDataSource';
import { Filter } from '@/models/Filter';
import { UpdateFieldModel } from '@/models/UpdateFieldModel';
interface PropTypes {
  title: string;
  breadcrumbs?: any[];
  groupDataSource?:ITreeDataSource
  dataSource: IListDataSource<any>;
  metaInfo: EntityMetaInfo;
  customSort?: Function;
}

const { dataSource, metaInfo, customSort } = withDefaults(defineProps<PropTypes>(), {
  breadcrumbs: () => [],
});


const filters: Ref<DataTableFilterMeta> = ref(metaInfo.filters);
const filter = async () => {
  dataSource.filter.value = toFilterArray(filters.value);
  await dataSource.get();
};
const confirm = inject<Confirm>('confirm', defaultConfirm);
const message = inject<IMessage>('message', defaultMessage);

const getStateCaption = (stateId:number) => {
  switch (stateId){
    case 1:return"Создан";
    case 2:return"Активен";
    case 3:return"Удалён";
    case 4:return"Принят";
    case 5:return"В процессе";
    case 6:return"Сделано";
  }
}

const getStateColor = (stateId:number) => {
  switch (stateId){
    case 1:return"warning";
    case 2:return"success";
    case 3:return"danger";
    case 4:return"info";
    case 5:return"warning";
    case 6:return"success";
  }
}

const editor = useEditorStore();
const menu = ref();
const item = ref();
const menuItems: Ref<any[]> = ref([
  {
    label: 'Редактировать',
    icon: 'pi pi-fw pi-pencil text-indigo-500',
    key: 'edit',
  },
  {
    label: item.value?.stateId == 2 ? "Деактивировать" : "Активировать",
    icon: 'pi pi-fw pi-sync text-indigo-500',
    key: 'activate',
  },
  {
    label: 'Удалить',
    icon: 'pi pi-fw pi-trash text-red-500',
    key: 'delete',
  },
]);

const updateFilter = async (data:any) => {
  const index = dataSource.filter.value.findIndex(f => f.name =="groupId");
  dataSource.filter.value.splice(index, 1);
  dataSource.filter.value = [...dataSource.filter.value, new Filter("groupId", data.key)];
  await dataSource.get();
}

const clearFilter = async () => {
  const index = dataSource.filter.value.findIndex(f => f.name =="groupId");
  dataSource.filter.value.splice(index, 1);
  await dataSource.get();
}

const sort = async (event: DataTableSortEvent) => {
  if (!!customSort) {
    await customSort(event);
    return;
  }

  dataSource.orderFieldName.value = toPascalCase(event.sortField as string);
  dataSource.orderFieldDirection.value = event.sortOrder == 1 ? 'ASC' : 'DESC';
  await dataSource.get();
};

const paginate = async (event: DataTablePageEvent) => {
  dataSource.pageIndex.value = event.page + 1;
  dataSource.pageSize.value = event.rows;
  await dataSource.get();
};

const menuHandle = async ({ key }: any, event) => {
  menu.value.toggle(event);
  switch (key) {
    case 'edit':
      await update();
      break;

    case 'delete':
      await remove();
      break;

    case 'activate':
      await dataSource.updateField(item.value.id,[
        new UpdateFieldModel({
          fieldName:"stateId",
          fieldValue:item.value.stateId == 2 ? 1 : 2
        })
      ])
      break;
  }
};
const update = async () => {
  metaInfo.updateItemInfo.model= item.value;
  await editor.update(metaInfo.updateItemInfo);
  message.success('Успешно обовлено');
};

const remove = async () => {
  const _confirm = await confirm({
    message: 'Вы действительно хотите удалить данный обьект?',
    header: 'Подтвердите свое действие',
    icon: 'pi pi-info-circle',
    acceptClass: 'p-button-danger',
  });
  if (!_confirm) return;

  await dataSource.remove(item.value.id);
  message.success('Успешно удалено');
};

const openMenu = (event, i) => {
  item.value = i;
  menu.value.toggle(event);
};

const add = async () => {
  const model = await editor.create<any>(metaInfo.createItemInfo);
  if (!model) return;
  await dataSource.add(model);
  await dataSource.get();
  message.success('Успешно добавлено');
};


const home = {
  label: 'Главная',
  to: '/',
};
</script>

<script lang="ts">
import { defineComponent } from "vue";
export default defineComponent({
  name: "data-list",
  inheritAttrs: false,
  customOptions: {},
});
</script>

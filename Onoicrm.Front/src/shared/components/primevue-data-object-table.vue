<template>
  <DataTable
      :value="items"
      :paginator="true"
      :lazy="true"
      :rows="dataSource.pageSize.value ?? 30"
      :total-records="dataSource.total.value"
      @sort="sort"
      filterDisplay="row"
      @filter="filter"
      v-model:filters="filters"
      @page="paginate"
      class="p-datatable-sm"
      paginatorTemplate="CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
      :rowsPerPageOptions="[5, 10, 20, 50]"
      currentPageReportTemplate="{first} - {last} из {totalRecords}"
  >
    <template  #header>
      <slot name="header">
        <div class="flex justify-content-end">
          <Button @click="add" label="Добавить" icon="pi pi-plus" />
        </div>
      </slot>
    </template>
    <template #empty> Нет данных. </template>
    <template #loading> Идёт загрузка данных. </template>
    <Column v-for="column in metaInfo.columns as any[]" :key="column.field" v-bind="column">
      <template #body="{ data }">
        <slot :item="data" :name="`${column.field}-body`">
          {{ getValue(data, column.field) }}
        </slot>
      </template>
      <template v-if="column.filterField" #filter="{ filterModel, filterCallback }">
        <slot :name="`${column.filterField}-filter`" :item="{ callBack: filterCallback, model: filterModel }">
          <InputText @change="filterCallback" v-model="filterModel.value" type="text" />
        </slot>
      </template>
    </Column>
    <Column :exportable="false" style="width: 3rem">
      <template #body="{ data }">
        <div class="flex align-items-center justify-content-end">
          <slot name="actions" :data="data">
            <Button
                @click="openMenu($event, data)"
                icon="pi pi-ellipsis-v"
                aria-haspopup="true"
                aria-controls="overlay_tmenu"
                class="p-button-rounded p-button-info p-button-outlined p-button-sm"
            />
          </slot>

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
</template>

<script setup lang="ts">
import {ComputedRef, inject, Ref, ref,computed} from 'vue';
import DataTable, { DataTableSortEvent, DataTablePageEvent, DataTableFilterMeta } from 'primevue/datatable';
import Column from "primevue/column"
import InputText from "primevue/inputtext"
import {IListDataSource} from "~/modules/data-sources";
import {defaultConfirm, defaultMessage, EntityMetaInfo, IMessage, TConfirm} from "~/shared";
import {getValue, toFilterArray, toPascalCase} from "~/shared/lib/helpers";
import {useEditorStore} from "~/modules/editor/stores/useEditorDialog";
interface ObjectDataTableProps {
  dataSource: IListDataSource<any>;
  metaInfo: EntityMetaInfo;
  customSort?: Function;
  excludeCallBack:Function
}

const { dataSource, metaInfo, customSort, excludeCallBack } = withDefaults(defineProps<ObjectDataTableProps>(), {});
const filters: Ref<DataTableFilterMeta> = ref(metaInfo.filters);
const filter = async () => {
  dataSource.filter.value = toFilterArray(filters.value);
  await dataSource.get();
};
const items:ComputedRef<any[]> = computed(()=> (dataSource.items.value as any[]).filter((i:any) => {
  return  excludeCallBack ? excludeCallBack(i) : true
}))


const confirm = inject<TConfirm>('confirm', defaultConfirm);
const message = inject<IMessage>('message', defaultMessage);
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
    label: 'Удалить',
    icon: 'pi pi-fw pi-trash text-red-500',
    key: 'delete',
  },
]);

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

const menuHandle = async ({ key }: any, event:any) => {
  menu.value.toggle(event);
  switch (key) {
    case 'edit': return await update();
    case 'delete': return await remove();
  }
};
const update = async () => {
  Object.assign(metaInfo.updateItemInfo.model, item.value);
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

const openMenu = (event:any, i:any) => {
  item.value = i;
  menu.value.toggle(event);
};

const add = async () => {
  const model = await editor.create(metaInfo.createItemInfo);
  if (!model) return;
  await dataSource.add(model);
  await dataSource.get();
  message.success('Успешно добавлено');
};
</script>


<script lang="ts">
import { defineComponent } from "vue";
export default defineComponent({
  name: "data-object-table",
  inheritAttrs: false,
  customOptions: {},
});
</script>

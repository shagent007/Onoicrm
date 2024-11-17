<template>
  <div class="data-list">
    <div class="data-list__header flex align-items-stretch">
    </div>

    <slot name="under-header">

    </slot>

    <div class='grid'>
      <div v-if='props.groupDataSource' class='col-3 flex' style="padding-top:27px;">
        <div class="h-full w-full" style="padding-bottom:72px;">
          <div class="data-list__group-title">Группы</div>
          <div class="h-full w-full">
            <data-tree
              v-if="props.groupDataSource.root.value.length"
              :props='{metaKeySelection:false, selectionMode:"single"}'
              @select='updateFilter($event)'
              :custom-tree-actions="props.customTreeActions"
              :data-source='props.groupDataSource'
              :custom-tree-items="props.customTreeItems"
              :custom-tree-fields="customTreeFields"
              :refresh="refresh"
              @un-select='clearFilter()'
              v-slot='{branch}'
            >
              <slot name="tree-branch" :item="branch">
                <span
                  @contextmenu.prevent='branch.toggle($event);
                  branch.selectNode(branch.node)'
                >
                  {{branch.node.label}}
                </span>
              </slot>
            </data-tree>
            <Button
              v-else
              @click="addClinicRootGroupElement()"
              label="Добавить корневой элемент"
            />

          </div>
        </div>
      </div>

      <div :class='props.groupDataSource ? "col-9" : "col-12"'>
        <base-table
          :columns="props.metaInfo.columns as any[]"
          :items="items"
        >
          <template v-for="column in props.metaInfo.columns as any[]" v-slot:[`${column.name}`]="{item}">
            <slot v-if="column.name=='actions'" :name="column.name" :item="item">
              <context-menu v-slot="{ toggle }" :items="menuItems">
                <Button
                  @click="currentItem = item;toggle($event);"
                  icon="pi pi-ellipsis-v"
                  class="p-button-rounded p-button-info p-button-outlined p-button-sm"
                />
              </context-menu>
            </slot>
            <slot v-else :name="column.name" :item="item">{{getValue(item, column.name)}}</slot>
          </template>
        </base-table>
      </div>
    </div>


    <Paginator
      :template="{
        '640px': 'PrevPageLink CurrentPageReport NextPageLink',
        '960px': 'FirstPageLink PrevPageLink CurrentPageReport NextPageLink LastPageLink',
        '1300px': 'FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink',
        default: 'FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink'
      }"
      :rows="normalizedDataSource.pageSize.value"
      :model-value="normalizedDataSource.pageIndex.value"
      :totalRecords="normalizedDataSource.total.value"
      @page="paginate"
    >
    </Paginator>
  </div>

</template>

<script setup lang="ts">
import DataTree from "./tree.vue"
import {computed, inject, Ref, ref} from 'vue';
import { IListDataSource } from '~/modules/data-sources';
import { EntityMetaInfo } from '~/shared';
import { useEditorStore } from '~/modules/editor/stores/useEditorDialog';
import { TConfirm, defaultConfirm } from '~/shared';
import { defaultMessage, IMessage } from '~/shared';
import { getValue, toPascalCase } from '../lib/helpers';
import { DataTableSortEvent, DataTablePageEvent } from 'primevue/datatable';
import { Filter } from '~/modules/data-sources';
import BaseTable from "../ui/base-table.vue";
import SearchField from "../ui/search-field.vue";
import {ITreeDataSource} from "~/modules/data-sources";
import ContextMenu from "~/shared/components/context-menu.vue";
import {useClinicStore} from "~/modules/clinic/stores/useClinicStore";
import {groupBy} from "lodash";
import {unwrap} from "~/modules/data-sources/hooks/unwrap";
interface PropTypes {
  groupDataSource?:ITreeDataSource
  dataSource: IListDataSource<any> | any;
  metaInfo: EntityMetaInfo;
  customSort?: Function;
  showByStateId?:number;
  refresh?:Function,
  customTreeItems?:Ref<any[]>,
  customTreeFields?:any[],
  customTreeActions?:any
}

const props = defineProps<PropTypes>()

const normalizedDataSource = unwrap(props.dataSource);

const items = computed(()=>{
  if(!props.showByStateId) return normalizedDataSource.items.value;
  return  normalizedDataSource.items.value.filter((i:any) => i.stateId == props.showByStateId);
})
const clinicStore = useClinicStore()
const confirm = inject<TConfirm>('confirm', defaultConfirm);
const message = inject<IMessage>('message', defaultMessage);
const searchText = ref<string>("");
const editor = useEditorStore();
const menu = ref();
const currentItem = ref();
const menuItems: Ref<any[]> = ref([
  {
    label: 'Редактировать',
    icon: 'pi pi-fw pi-pencil text-indigo-500',
    key: 'edit',
    command:async ()=> await update()
  },
  {
    label: 'Удалить',
    icon: 'pi pi-fw pi-trash text-red-500',
    key: 'delete',
    command:async ()=> await remove()
  },
]);

const addClinicRootGroupElement = async () => {
  await props.groupDataSource?.add({
    caption:"Основной",
    parentId: null,
    clinicId: clinicStore.clinic.value?.id
  })
}

const updateFilter = async (data:any) => {
  const index = normalizedDataSource.filter.value.findIndex(f => f.name =="groupId");
  if(index != -1){
    normalizedDataSource.filter.value.splice(index, 1);
  }
  normalizedDataSource.filter.value = [...normalizedDataSource.filter.value, new Filter("groupId", data.key)];
  await normalizedDataSource.get();
}

const clearFilter = async () => {
  const index = normalizedDataSource.filter.value.findIndex(f => f.name =="groupId");
  normalizedDataSource.filter.value.splice(index, 1);
  await normalizedDataSource.get();
}

const searchByText = async () => {
  normalizedDataSource.filter.value = normalizedDataSource.filter.value.filter(f => f.name !== "searchText");
  normalizedDataSource.filter.value.push(new Filter("searchText", searchText.value));
  await normalizedDataSource.get();
}



const paginate = async (event: DataTablePageEvent) => {
  normalizedDataSource.pageIndex.value = event.page + 1;
  normalizedDataSource.pageSize.value = event.rows;
  await normalizedDataSource.get();
};

const update = async () => {
  Object.assign(props.metaInfo.updateItemInfo.model, currentItem.value);
  await editor.update(props.metaInfo.updateItemInfo);
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

  await normalizedDataSource.remove(currentItem.value.id);
  message.success('Успешно удалено');
};

const openMenu = (event:Event, i:any) => {
  currentItem.value = i;
  menu.value.toggle(event);
};

const add = async () => {
  const model = await editor.create<any>(props.metaInfo.createItemInfo);
  if (!model) return;

  await normalizedDataSource.add(model);
  await normalizedDataSource.get();
  message.success('Успешно добавлено');
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

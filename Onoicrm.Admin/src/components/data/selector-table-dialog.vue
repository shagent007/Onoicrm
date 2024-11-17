<template>
  <Dialog
      :header="title"
      v-model:visible="visible"
      :breakpoints="{ '960px': '75vw', '640px': '90vw' }"
      :style="{ width: '50vw' }"
      :modal="true"
  >
    <data-object-table
      :data-source="dataSource"
      :meta-info="metaInfo"
      :exclude-call-back="excludeCallBack"
      :custom-sort="customSort"
    >
      <template #header>
        <div></div>
      </template>
      <template #actions="{data}">
        <Button class="p-button-rounded p-button-outlined" icon="pi pi-plus" @click="select(data)" />
      </template>
    </data-object-table>
    <template #footer>
      <Button
          label="Закрыть"
          icon="pi pi-times"
          @click="close"
          class="p-button-text"
      />
    </template>
  </Dialog>

</template>

<script lang="ts" setup>
import {ref, Ref} from "vue";
import Dialog from "primevue/dialog";
import { IListDataSource } from '../../models/IListDataSource';
import { EntityMetaInfo } from '../../models/EntityMetaInfo/EntityMetaInfo';
import DataObjectTable from './object-table.vue';
const visible: Ref<boolean> = ref(false);
interface PropTypes {
  dataSource: IListDataSource<any>;
  metaInfo: EntityMetaInfo;
  customSort?: Function;
  excludeCallBack:Function
  title:string
}


const {title} = defineProps<PropTypes>();
let resolve: Function;
const emit = defineEmits(["select"]);
const open = () => {
  visible.value = true;
  return new Promise<any>((r) => (resolve = r));
};

const select = (item:any) => {
  emit("select", item)
}

defineExpose({open})

const close = () => {
  visible.value = false;
  resolve(null);
};
</script>

<script lang="ts">
import { defineComponent } from "vue";
export default defineComponent({
  name: "data-selector-table-dialog",
  inheritAttrs: false,
  customOptions: {},
});
</script>
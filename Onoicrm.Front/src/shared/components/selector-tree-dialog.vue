<template>
  <Dialog
    :header="title"
    v-model:visible="visible"
    :breakpoints="{ '960px': '75vw', '640px': '90vw' }"
    :style="{ width: '50vw' }"
    :modal="true"
  >
    <data-tree v-slot='{branch}'   :data-source='dataSource'>
      <div class='flex w-full align-items-center justify-content-between'>
        <span>{{branch.node.label}}</span>
        <Button
          rounded
          outlined
          v-if='excludeCallBack(branch.node.key)'
          icon="pi pi-plus"
          @click='emit("select", branch.node.key)'
        />
        <Button @click='branch.selectNode(branch.node); emit("un-select",  branch.getBranch())' class="p-button-rounded p-button-outlined" v-else icon="pi pi-minus"  />
      </div>
    </data-tree>
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
import DataTree from "./tree.vue"
import { ITreeDataSource } from '~/modules/data-sources/interfaces/ITreeDataSource';
const visible: Ref<boolean> = ref(false);
interface PropTypes {
  dataSource: ITreeDataSource;
  excludeCallBack:Function
  title:string
}


const {title} = defineProps<PropTypes>();
let resolve: Function;
const emit = defineEmits(["select", "un-select"]);
const open = () => {
  visible.value = true;
  return new Promise<any>((r) => (resolve = r));
};


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

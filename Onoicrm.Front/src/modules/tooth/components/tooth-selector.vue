<template>
  <Dialog
    :visible="visible"
    @update:visible="close()"
    modal
    header="Новая запись"
    :style="{ 'max-width': '995px'}"
    class="tooth-dialog"
  >
      <div class="grid">
          <div class="col-12">
          <upper-jaw :states="states" @show-tooth="close($event)"/>
          </div>
          <div class="col-12">
            <lower-jaw :states="states" @show-tooth="close($event)"/>
          </div>
      </div>
  </Dialog>
</template>

<script setup lang="ts">
import {  ref,inject} from "vue";
import {defaultMessage, IMessage} from "~/shared";
import UpperJaw from "~/modules/tooth/components/upper-jaw.vue";
import LowerJaw from "~/modules/tooth/components/lower-jaw.vue";
import {useToothStore} from "~/modules/tooth";
import {ToothStateModel} from "~/modules/tooth/models/ToothStateModel";

const visible = ref(false);
const resolve = ref();
const message = inject<IMessage>("message", defaultMessage);
const toothStore = useToothStore();
defineProps<{
  states:ToothStateModel[]
}>();
const select = async () => {
  visible.value = true;
  return new Promise(r => resolve.value = r);
}


defineExpose({ select })


const close = (data:any = null) => {
  if(data !== null){
    data = toothStore.items.find(t => t.position == data);
  }
  visible.value = false;
  resolve.value(data);
}


</script>

<template>
  <slot :edit="edit" v-if="!editMode"></slot>
  <input
    v-else
    ref="input"
    type="text"
    @blur="editMode=false"
    @change="update"
    :value="modelValue"
    @input="setValue($event)"
  />
</template>

<script setup lang="ts">
import {ref, nextTick} from "vue";
const {modelValue} = defineProps<{modelValue:string|number|null}>();
const editMode = ref<boolean>(false);
const input = ref<HTMLInputElement>();
const timeOut = ref<any>(null);
const emit = defineEmits(["update:modelValue", "update"]);

const setValue = ({target}:any) => {
  emit("update:modelValue", target.value)
}
const update = () => {
  editMode.value = false;
  emit("update")
}



const edit = async () => {
  editMode.value = true;
  await nextTick();
  input.value?.focus();
}
</script>

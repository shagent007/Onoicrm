<template>
  <div class="mb-2">
    <ProgressBar :value="state"/>
  </div>
</template>

<script setup lang="ts">
import {onMounted, ref} from "vue";
import {getFileFormData} from "~/shared";

import {IUploadService} from "~/modules/data-sources/interfaces/IUploadService";
const state = ref<number>(0);
const {dataSource,file} = defineProps<{dataSource:IUploadService, file:File}>();
const onLoad =(e: any) => {
  state.value = Math.round((e.loaded / e.total) * 100);
}
const emit = defineEmits(["finish"])

onMounted(async () => {
  const formData = getFileFormData(file);
  const document = await dataSource.upload(formData, onLoad);
  emit("finish", {document,file})
})
</script>


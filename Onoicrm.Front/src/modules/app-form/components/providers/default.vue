<template>
    <slot v-if="canShow" :editor="{ fieldService }"></slot>
</template>

<script setup lang="ts">
import { FieldService } from '../../services/FieldService';
import { ref, Ref, watch } from 'vue';

const {fieldService} =  defineProps<{fieldService: FieldService}>();
const canShow:Ref<boolean> = ref(fieldService.canShow());
watch(
  () => ({...fieldService.model}),
  async () => canShow.value = await fieldService.canShow()
)

</script>

<template>
  <div
    v-for="symptom in existSymptoms as any[]"
    :key="symptom.id"
    class="flex mb-2 align-items-center"
  >
    <i
      class="pi pi-exclamation-triangle mr-3"
      style="color: #7E92A2"

    />
    <h4 class="font-medium">{{ symptom.caption }}</h4>
  </div>
</template>

<script setup lang="ts">
import {computed} from "vue";
import {useSymptomStore} from "~/modules/symptom";

const {patientSymptoms} = defineProps<{patientSymptoms:any[]}>();
const symptomStore = useSymptomStore();
const existSymptoms = computed(() => symptomStore.items.filter((s: any) =>
  patientSymptoms.some((ups: any) => ups.symptomId === s.id && ups.value)
));
</script>


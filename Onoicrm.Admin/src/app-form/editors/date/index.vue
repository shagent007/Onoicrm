<template>
  <div class="calendar">
    <label class="mb-1 block" :for="fieldService.id" v-if="attrs.caption">
      {{ attrs?.caption }}
    </label>
    <Calendar
      :placeholder="attrs.placeholder"
      :class="{ 'p-invalid': fieldService.showErrors }"
      class="w-full"
      :dateFormat="attrs.format"
      :id="fieldService.id"
      :manualInput="false"
      @date-select="dateSelect"
      inputId="basic"
      :modelValue="fieldService.value"
      autocomplete="off"
    />
    <small v-if="attrs?.hint">
      {{ attrs?.hint }}
    </small>

    <div v-if="fieldService.showErrors" v-for="error in fieldService.errorMessages.value" :key="error">
      <small class="p-error">{{ error }}</small>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useField } from '../../hooks/useField';
import { DateField } from './models/DateField';
import { FieldService } from '../../services/FieldService';
import { FormService } from '../../services/FormService';
import Calendar from 'primevue/calendar';
import { computed } from 'vue';
import { DateFieldAttribute } from './models/DateFieldAttribute';

const { fieldService, formService } = defineProps<{ fieldService: FieldService<DateField>; formService: FormService }>();
const attrs = computed(() => fieldService.attrs);
const { update } = useField(fieldService, formService);

const dateSelect = async (event: any) => {
  fieldService.value = event;
  await update();
};
</script>

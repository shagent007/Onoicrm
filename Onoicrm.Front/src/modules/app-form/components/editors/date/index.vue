<template>
  <div :class="attrs.cssClass" class="calendar">
    <label class="mb-1 block" :for="fieldService.id" v-if="attrs.caption">
      {{ attrs?.caption }}
    </label>
    <div class="w-full" :class="attrs.iconPosition">
      <i v-if="attrs.icon" :class="attrs.icon" />

      <Calendar
        :placeholder="attrs.placeholder"
        :class="{ 'p-invalid': fieldService.showErrors }"
        class="w-full field"
        :dateFormat="attrs.format"
        :id="fieldService.id"
        :manualInput="false"
        @date-select="dateSelect"
        inputId="basic"
        :modelValue="new Date(fieldService.value) "
        autocomplete="off"
      />
    </div>
    <small v-if="attrs?.hint">
      {{ attrs?.hint }}
    </small>

    <div
      v-if="fieldService.showErrors"
      v-for="error in fieldService.errorMessages.value"
      :key="error"
    >
      <small class="p-error">{{ error }}</small>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useField } from "../../../hooks/useField";
import { DateField } from "~/modules/app-form";
import { FieldService } from "~/modules/app-form";
import { FormService } from "~/modules/app-form";
import Calendar from "primevue/calendar";
import { computed } from "vue";
import { DateFieldAttribute } from "~/modules/app-form";
import {isRef} from "vue";

const { fieldService, formService } = defineProps<{fieldService: FieldService<DateField>,formService: FormService}>();
const attrs = computed(() => {
  const attrs = fieldService.attrs;
  if(isRef(attrs)) return attrs.value as DateFieldAttribute;
  return attrs as DateFieldAttribute;
});

const { update } = useField(fieldService, formService);

const dateSelect = async (event: any) => {
  fieldService.value = event;
  await update();
};
</script>

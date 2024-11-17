<template>
  <div :class="attrs.cssClass">
    <label class="mb-1 block" :for="fieldService.id" v-if="attrs.caption">
      {{ attrs?.caption }}
    </label>


    <small v-if="attrs?.hint">
      {{ attrs?.hint }}
    </small>

    <div
      v-if="fieldService?.showErrors"
      v-for="error in fieldService?.errorMessages.value"
      :key="error"
    >
      <small class="p-error">{{ error }}</small>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { useField } from "../../../hooks/useField";
import {CustomField, DateFieldAttribute} from "~/modules/app-form";
import { FieldService } from "~/modules/app-form";
import { FormService } from "~/modules/app-form";
import {isRef} from "vue";

const { fieldService, formService } = defineProps<{fieldService: FieldService<CustomField>,formService: FormService}>();
const attrs = computed(() => {
  const attrs = fieldService.attrs;
  if(isRef(attrs)) return attrs.value as DateFieldAttribute;
  return attrs as DateFieldAttribute;
});const { update } = useField(fieldService, formService);

</script>

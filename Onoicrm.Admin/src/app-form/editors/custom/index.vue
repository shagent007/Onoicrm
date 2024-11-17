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
import { useField } from "../../hooks/useField";
import { CustomField } from "./models/CustomField";
import { FieldService } from "../../services/FieldService";
import { FormService } from "../../services/FormService";
import { computed } from "vue";
import {FieldAttribute} from "~/app-form/models/FieldAttribute";

const { fieldService, formService } = defineProps<{fieldService: FieldService<CustomField>,formService: FormService}>();
const attrs = computed(() => fieldService.field.attrs as FieldAttribute);
const { update } = useField(fieldService, formService);

</script>

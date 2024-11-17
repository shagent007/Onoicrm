<template>
  <div v-if="loaded">
    <label class="mb-1 block" :for="fieldService.id" v-if="attrs?.caption">
      {{ attrs?.caption }}
    </label>
    <MultiSelect
      display="chip"
      v-model="fieldService.value"
      :options="items"
      @change="fieldService.validate, update"
      :filter="true"
      :disabled="attrs?.disabled"
      :showClear="true"
      :optionLabel="config?.labelKeyName"
      :optionValue="config?.valueKeyName"
      :placeholder="attrs?.placeholder"
      class="w-full field"
    />

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
  <ProgressBar v-else mode="indeterminate" style="height: 0.5em" />
</template>

<script setup lang="ts">
import { useField } from "../../../hooks/useField";
import { MultiSelectField } from "~/modules/app-form";
import { InputFieldAttribute } from "~/modules/app-form";
import { FieldService } from "~/modules/app-form";
import { SelectFieldConfig } from "~/modules/app-form";
import { onMounted, computed } from "vue";
import { FormService } from "~/modules/app-form";
import MultiSelect from "primevue/multiselect";
import ProgressBar from "primevue/progressbar";
import {isRef} from "vue";
interface PropTypes {
  fieldService: FieldService<
      MultiSelectField,
    InputFieldAttribute,
    SelectFieldConfig
  >;
  formService: FormService;
}
const { fieldService, formService } = defineProps<PropTypes>();
const { update, items, loaded } = useField(fieldService, formService);

const config = computed(() => fieldService.config as SelectFieldConfig);
const attrs = computed(() => {
  const attrs = fieldService.attrs;
  if(isRef(attrs)) return attrs.value as InputFieldAttribute;
  return attrs as InputFieldAttribute;
});

onMounted(async () => {
  loaded.value = false;
  const config = fieldService?.field?.config;
  items.value = await formService.actions[config.getItems]();
  loaded.value = true;
});
</script>

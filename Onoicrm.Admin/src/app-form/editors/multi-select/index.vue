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
      class="w-full"
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
import { useField } from "../../hooks/useField";
import { MultiSelectField } from "./models/MultiSelectField";
import { InputFieldAttribute } from "../../models/InputFieldAttribute";
import { FieldService } from "../../services/FieldService";
import { SelectFieldConfig } from "./models/SelectFieldConfig";
import { onMounted, computed } from "vue";
import { FormService } from "../../services/FormService";
import MultiSelect from "primevue/multiselect";
import ProgressBar from "primevue/progressbar";
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
const attrs = computed(() => fieldService.attrs);

onMounted(async () => {
  loaded.value = false;
  const config = fieldService?.field?.config;
  items.value = await formService.actions[config.getItems]();
  loaded.value = true;
});
</script>

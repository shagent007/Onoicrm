<template>
  <div v-if="loaded">
    <label class="mb-1 block" :for="fieldService.id" v-if="attrs?.caption">
      {{ attrs?.caption }}
    </label>
    <Dropdown
      v-model="fieldService.value"
      :options="items"
      v-if="config.mode == SelectMode.Select"
      @change="fieldService.validate, update"
      :filter="true"
      :disabled="attrs?.disabled"
      :showClear="true"
      :optionLabel="config?.labelKeyName"
      :optionValue="config?.valueKeyName"
      :placeholder="attrs?.placeholder"
      class="w-full"
    />

    <SelectButton
      v-if="config.mode == SelectMode.BtnGroup"
      :optionLabel="config?.labelKeyName"
      :optionValue="config?.valueKeyName"
      v-model="fieldService.value"
      @change="update"
      :options="items"
    />

    <v-swatches
      v-if="config.mode == SelectMode.Color"
      v-model="fieldService.value"
      inline
      @change="update"
      :swatches="items"
    />

    <small v-if="attrs?.hint">
      {{ attrs?.hint }}
    </small>
    <div v-if="fieldService.showErrors" v-for="error in fieldService.errorMessages.value" :key="error">
      <small class="p-error">{{ error }}</small>
    </div>
  </div>
  <ProgressBar v-else mode="indeterminate" style="height: 0.5em" />
</template>

<script setup lang="ts">
import { useField } from '../../hooks/useField';
import { SelectField } from './models/SelectField';
import { InputFieldAttribute } from '../../models/InputFieldAttribute';
import { FieldService } from '../../services/FieldService';
import { SelectFieldConfig } from './models/SelectFieldConfig';
import { onMounted, computed } from 'vue';
import { FormService } from '../../services/FormService';
import SelectButton from "primevue/selectbutton"
import { VSwatches } from 'vue3-swatches'
import 'vue3-swatches/dist/style.css'

import Dropdown from 'primevue/dropdown';
import ProgressBar from 'primevue/progressbar';
import { SelectMode } from './models/SelectMode';
interface PropTypes {
  fieldService: FieldService<SelectField, InputFieldAttribute, SelectFieldConfig>;
  formService: FormService;
}
const { fieldService, formService } = defineProps<PropTypes>();
const { update, items, loaded } = useField(fieldService, formService);

const config = computed(() => fieldService.config as SelectFieldConfig);
const attrs = computed(() => fieldService.attrs as InputFieldAttribute);

onMounted(async () => {
  loaded.value = false;
  const config = fieldService?.field?.config;
  items.value = await formService.actions[config.getItems]();
  loaded.value = true;
});
</script>

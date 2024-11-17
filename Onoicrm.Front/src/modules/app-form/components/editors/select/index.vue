<template>
  <div :class="attrs.cssClass" v-if="loaded">
    <label class="mb-1 block" :for="fieldService.id" v-if="attrs?.caption">
      {{ attrs?.caption }}
    </label>
    <div class="w-full" :class="attrs.iconPosition">
      <i v-if="attrs.icon" :class="attrs.icon" />
      <Dropdown
        v-model="fieldService.value"
        :options="items"
        v-if="config.mode == SelectMode.Select"
        @change="fieldService.validate('change'); update()"
        :filter="true"
        :disabled="attrs?.disabled"
        :showClear="true"
        :optionLabel="config?.labelKeyName"
        :optionValue="config?.valueKeyName"
        :placeholder="attrs.placeholder"
        class="w-full field"
      />
    </div>

    <SelectButton
      v-if="config.mode == SelectMode.BtnGroup"
      :optionLabel="config?.labelKeyName"
      @change="fieldService.validate('change'); update()"
      :optionValue="config?.valueKeyName"
      v-model="fieldService.value"
      :options="items"
    />

    <small v-if="attrs?.hint">{{ attrs?.hint }}</small>

    <v-swatches
      v-if="config.mode == SelectMode.Color"
      v-model="fieldService.value"
      inline
      @update:modelValue="update()"
      :swatches="items"
    />

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
import { SelectField } from "~/modules/app-form";
import { InputFieldAttribute } from "../../../models";
import { SelectFieldConfig } from "~/modules/app-form";
import { onMounted, computed } from "vue";
import {FieldService, FormService} from "../../../services";
import Dropdown from "primevue/dropdown";
import SelectButton from  "primevue/selectbutton"
import ProgressBar from "primevue/progressbar";
import 'vue3-swatches/dist/style.css'

import { SelectMode } from "./models/SelectMode";
import {isRef} from "vue";
import {VSwatches} from "vue3-swatches";
interface PropTypes {
  fieldService: FieldService<
    SelectField,
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

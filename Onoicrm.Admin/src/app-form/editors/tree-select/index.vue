<template>
  <div>
    <label class="mb-1 block" :for="fieldService.id" v-if="attrs?.caption">
      {{ attrs?.caption }}
    </label>

    <TreeSelect :model-value="getValue()" @update:model-value='updateValue($event)' :options="dataSource.transformedRoot.value" :placeholder="attrs.placeholder" class="w-full" />

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
import { useField } from "../../hooks/useField";
import TreeSelect from 'primevue/treeselect';
import { TreeSelectField } from "./models/TreeSelectField";
import { InputFieldAttribute } from "../../models/InputFieldAttribute";
import { FieldService } from "../../services/FieldService";
import { SelectFieldConfig } from "./models/SelectFieldConfig";
import {  computed } from "vue";
import { FormService } from "../../services/FormService";
import { ITreeDataSource } from '../../../models/ITreeDataSource';
interface PropTypes {
  fieldService: FieldService<
    TreeSelectField,
    InputFieldAttribute,
    SelectFieldConfig
  >;
  formService: FormService;
}
const { fieldService, formService } = defineProps<PropTypes>();
const { update, items } = useField(fieldService, formService);
const updateValue = async (event:any) => {
  fieldService.value = Object.keys(event)[0];
  await update();
}

const getValue = () => fieldService.value ? ({[fieldService.value]:true}) : null
const config = computed(() => fieldService.config as SelectFieldConfig);
const attrs = computed(() =>fieldService.attrs );
const {dataSource}:{dataSource:ITreeDataSource} = fieldService.actions[fieldService.field.getConfig]();

</script>

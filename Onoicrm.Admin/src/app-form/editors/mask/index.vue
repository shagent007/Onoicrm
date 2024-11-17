<template>
  <div class="">
    <label class="mb-1 block" :for="fieldService.id" v-if="attrs?.caption">
      {{ attrs.caption }}
    </label>
    <InputText
      v-model="fieldService.value"
      :id="fieldService.id"
      type="text"
      @change="update();"
      v-maska
      :data-maska="config.mask"
      class="w-full"
      :class="{ 'p-invalid': fieldService.showErrors }"
      :placeholder="attrs?.placeholder"
    />
    <small v-if="attrs.hint">
      {{ attrs.hint }}
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
import { vMaska } from "maska"
import { useField } from "../../hooks/useField";
import { MaskField } from "./models/MaskField";
import { InputFieldAttribute } from "../../models/InputFieldAttribute";
import { FieldService } from "../../services/FieldService";
import { FormService } from "../../services/FormService";
import InputMask from "primevue/inputmask";
import { MaskFieldConfig } from "./models/MaskFieldConfig";
import { computed, isRef, onMounted } from 'vue';
interface PropTypes {
  fieldService: FieldService<MaskField, InputFieldAttribute, MaskFieldConfig>;
  formService: FormService;
}

const { fieldService, formService } = defineProps<PropTypes>();
const { update } = useField(fieldService, formService);
const config = computed(() => fieldService.config as MaskFieldConfig);
const attrs = computed(() => {
  const attrs = fieldService.attrs;
  if (isRef(attrs)) return attrs.value as InputFieldAttribute;
  return attrs as InputFieldAttribute;
});

onMounted(()=>{
  if(fieldService.field.initialValue){
    fieldService.value = fieldService.field.initialValue;
  }
})

</script>

<template>
  <div :class="attrs.cssClass">
    <label class="mb-1 block" :for="fieldService.id" v-if="attrs?.caption">
      {{ attrs.caption }}
    </label>
    <div class="w-full" :class="attrs?.iconPosition">
      <i v-if="attrs?.icon" :class="attrs?.icon" />
      <InputMask
        :model-value="fieldService.value"
        :id="fieldService.id"
        type="text"
        @change="fieldService.value = $event.target.value; update(); "
        :mask="config.mask"
        class="w-full field"
        :class="{ 'p-invalid': fieldService.showErrors }"
        :placeholder="attrs.placeholder"
      />
    </div>
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
import { useField } from "../../../hooks/useField";
import { MaskField } from "./models/MaskField";
import { InputFieldAttribute } from "../../../models/InputFieldAttribute";
import { FieldService } from "../../../services/FieldService";
import { FormService } from "../../../services/FormService";
import InputMask from "primevue/inputmask";
import { MaskFieldConfig } from "./models/MaskFieldConfig";
import { getValue } from "~/shared/lib/helpers";
import { computed, isRef, watch } from "vue";
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
</script>

<template>
  <div  class="flex items-center pt-4" :class="attrs.cssClass">
    <input
      v-model="fieldService.value"
      @input="update()"
      @change="fieldService.validate()"
      :id="fieldService.id"
      type="checkbox"
    />
    <label class="ml-2" :for="fieldService.id" v-if="attrs.caption">
      {{ attrs?.caption }}
    </label>
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
import { BoolField } from "./models/BoolField";
import { InputFieldAttribute } from "../../models/InputFieldAttribute";
import { FieldService } from "../../services/FieldService";
import { FormService } from "../../services/FormService";
import Checkbox from 'primevue/checkbox';
import { computed, isRef, ref } from 'vue';
interface PropTypes {
  fieldService: FieldService<BoolField, InputFieldAttribute>;
  formService: FormService;
}

const test = ref(false)

const { fieldService, formService } = defineProps<PropTypes>();
const attrs = computed(() => isRef(fieldService.attrs) ?  fieldService.attrs.value : fieldService.attrs);
const { update } = useField(fieldService, formService);
</script>

<template>
  <div class="">
    <label class="mb-1 block" :for="fieldService.id" v-if="attrs?.caption">
      {{ attrs?.caption }}
    </label>

    <Password
      :id="fieldService.id"
      :placeholder="attrs.placeholder"
      :toggleMask="true"
      @input="fieldService.validate()"
      v-model="fieldService.value"
      :feedback="false"
      class="w-full"
      inputClass="w-full"
      @change="update"
      :class="{ 'p-invalid': fieldService.showErrors }"
    ></Password>
    <small v-if="attrs?.hint">{{ attrs?.hint }}</small>

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
import { computed } from "vue";
import { useField } from "../../hooks/useField";
import { FieldConfig } from "../../models/FieldConfig";
import { StringField } from "../string/models/StringField";
import { InputFieldAttribute } from "../../models/InputFieldAttribute";
import { FieldService } from "../../services/FieldService";
import { FormService } from "../../services/FormService";
import Password from "primevue/password";
interface PropTypes {
  fieldService: FieldService<StringField, InputFieldAttribute, FieldConfig>;
  formService: FormService;
}
const { fieldService, formService } = defineProps<PropTypes>();
const attrs = computed(() => fieldService.attrs);
const { update } = useField(fieldService, formService);
</script>

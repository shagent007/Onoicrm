<template>
  <div :class="attrs.cssClass">
    <label class="mb-1 block" :for="fieldService.id" v-if="attrs.caption">
      {{ attrs?.caption }}
    </label>
    <InputText
      @input="fieldService.validate()"
      v-model="fieldService.value"
      :id="fieldService.id"
      type="text"
      @change="update"
      class="w-full"
      :class="{ 'p-invalid': fieldService.showErrors }"
      :placeholder="attrs.placeholder"
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
</template>

<script setup lang="ts">
import { useField } from "../../hooks/useField";
import { StringField } from "./models/StringField";
import { InputFieldAttribute } from "../../models/InputFieldAttribute";
import { FieldService } from "../../services/FieldService";
import { FormService } from "../../services/FormService";
import InputText from "primevue/inputtext";
import { computed } from "vue";
interface PropTypes {
  fieldService: FieldService<StringField, InputFieldAttribute>;
  formService: FormService;
}

const { fieldService, formService } = defineProps<PropTypes>();
const attrs = computed(() => fieldService.attrs);
const { update } = useField(fieldService, formService);
</script>

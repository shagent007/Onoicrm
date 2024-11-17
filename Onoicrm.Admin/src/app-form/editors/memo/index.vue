<template>
  <div class="">
    <label class="mb-1 block" :for="fieldService.id" v-if="attrs?.caption">
      {{ attrs?.caption }}
    </label>
    <Textarea
      :rows="  10"
      @input="fieldService.validate()"
      v-model="fieldService.value"
      :id="fieldService.id"
      type="text"
      @change="update"
      class="w-full"
      :class="{ 'p-invalid': fieldService.showErrors }"
      :placeholder="attrs?.placeholder"
    />
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
import { useField } from "../../hooks/useField";
import { FieldConfig } from "../../models/FieldConfig";
import { MemoField } from "./models/MemoField";
import { InputFieldAttribute } from "../../models/InputFieldAttribute";
import { FieldService } from "../../services/FieldService";
import { FormService } from "../../services/FormService";
import Textarea from "primevue/textarea";
import { computed } from 'vue';
interface PropTypes {
  fieldService: FieldService<MemoField, InputFieldAttribute, FieldConfig>;
  formService: FormService;
}
const attrs = computed(() => fieldService.attrs);
const { fieldService, formService } = defineProps<PropTypes>();
const { update } = useField(fieldService, formService);
</script>

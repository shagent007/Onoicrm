<template>
  <div :class="attrs.cssClass">
    <label class="mb-1 block" :for="fieldService.id" v-if="attrs.caption">
      {{ attrs.caption }}
    </label>
    <div class="w-full" :class="attrs?.iconPosition">
      <i v-if="attrs?.icon" :class="attrs?.icon" />
      <InputText
        @input="fieldService.validate('input')"
        v-model="fieldService.value"
        :id="fieldService.id"
        type="text"
        @change="update();fieldService.validate('change')"
        @blur="fieldService.validate('blur')"
        @focus="fieldService.validate('focus')"
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
import { StringField } from "~/modules/app-form";
import { InputFieldAttribute } from "~/modules/app-form";
import { FieldService } from "~/modules/app-form";
import { FormService } from "~/modules/app-form";
import InputText from "primevue/inputtext";
import { computed, isRef } from "vue";
interface PropTypes {
  fieldService: FieldService<StringField, InputFieldAttribute>;
  formService: FormService;
}

const { fieldService, formService } = defineProps<PropTypes>();
const attrs = computed(() => {
  const attrs = fieldService.attrs;
  if (isRef(attrs)) return attrs.value as InputFieldAttribute;
  return attrs as InputFieldAttribute;
});
const { update } = useField(fieldService, formService);
</script>

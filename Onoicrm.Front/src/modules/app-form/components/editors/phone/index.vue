<template>
  <div :class="attrs.cssClass">
    <label class="mb-1 block" :for="fieldService.id" v-if="attrs?.caption">
      {{ attrs.caption }}
    </label>
    <div class="w-full" :class="attrs?.iconPosition">
      <i v-if="attrs?.icon" :class="attrs?.icon" />
      <InputText
        v-model="fieldService.value"
        :id="fieldService.id"
        type="text"
        @change="update()"
        v-maska
        :data-maska="phoneMask"
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
import { PhoneField } from "./models/PhoneField";
import { InputFieldAttribute } from "~/modules/app-form";
import { FieldService, FormService } from "~/modules/app-form";
import {computed, isRef, ref, watch} from "vue";
import { vMaska } from "maska"

interface PropTypes {
  fieldService: FieldService<PhoneField, InputFieldAttribute, FieldService>;
  formService: FormService;
}

const { fieldService, formService } = defineProps<PropTypes>();
const { update } = useField(fieldService, formService);

const masks = [
  { country: 'russia', mask: '+7 (###) ###-##-##' },
  { country: 'turkey', mask: '+90 (###) ###-####' },
  { country: 'kyrgyzstan', mask: '+996 (###) ###-###' },
  { country: 'uzbekistan', mask: '+998 (##) ###-##-##' },
]

const phoneMask = ref<string>('+996 (###) ###-###')


watch(()=>fieldService.value,(value) => {
  for (const mask of masks) {
    const charactersBeforeParenthesis = mask.mask.split(' ')[0];
    phoneMask.value = "+###";
    if (value?.startsWith(charactersBeforeParenthesis)) {
      phoneMask.value = mask.mask;
      break;
    }
  }
})

const attrs = computed(() => {
  const attrs = fieldService.attrs;
  if (isRef(attrs)) return attrs.value as InputFieldAttribute;
  return attrs as InputFieldAttribute;
});
</script>

<template>
  <div :class="attrs?.cssClass">
    <label class="mb-1 block" :for="fieldService?.id" v-if="attrs.caption">
      {{ attrs?.caption }}
    </label>
    <div class="rating">
      <Rating
        onIcon="pi pi-star-fill"
        offIcon="pi pi-star-fill"
        :cancel="false"
        v-model="fieldService.value"
      />
    </div>
    <small v-if="attrs?.hint">
      {{ attrs.hint }}
    </small>

    <div
      v-if="fieldService?.showErrors"
      v-for="error in fieldService?.errorMessages.value"
      :key="error"
    >
      <small class="p-error">{{ error }}</small>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useField } from "../../../hooks/useField";
import { computed,isRef } from "vue";
import {InputFieldAttribute,RatingField,
  FieldService,
  FormService} from "~/modules/app-form";
interface PropTypes {
  fieldService: FieldService<RatingField>;
  formService: FormService;
}

const { fieldService, formService } = defineProps<PropTypes>();
const attrs = computed(() => {
  const attrs = fieldService.attrs;
  if(isRef(attrs)) return attrs.value as InputFieldAttribute;
  return attrs as InputFieldAttribute;
});const { update } = useField(fieldService, formService);
</script>

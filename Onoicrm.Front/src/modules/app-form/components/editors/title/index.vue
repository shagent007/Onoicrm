<template>
  <h2 :class="attrs.cssClass">{{ attrs.caption }}</h2>
</template>

<script setup lang="ts">
import { useField } from "../../../hooks/useField";
import { InputFieldAttribute } from "~/modules/app-form";
import { FieldService } from "~/modules/app-form";
import { FormService } from "~/modules/app-form";
import { TitleField } from "~/modules/app-form";
import {computed} from "vue";
import {isRef} from "vue";
interface PropTypes {
  fieldService: FieldService<TitleField>;
  formService: FormService;
}
const { fieldService, formService } = defineProps<PropTypes>();
const { update, items, loaded } = useField(fieldService, formService);
const attrs = computed(() => {
  const attrs = fieldService.attrs;
  if (isRef(attrs)) return attrs.value as InputFieldAttribute;
  return attrs as InputFieldAttribute;
});
</script>

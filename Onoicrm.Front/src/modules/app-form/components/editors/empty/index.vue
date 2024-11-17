<template>
  <div></div>
</template>

<script setup lang="ts">
import {FieldService} from "../../../services/FieldService";
import {computed} from "vue";
import {FormService} from "../../../services/FormService";
import {useField} from "../../../hooks/useField";
import {isRef} from "vue";
import {InputFieldAttribute} from "~/modules/app-form";

interface PropTypes {
  fieldService: FieldService;
  formService: FormService;
}

const { fieldService, formService } = defineProps<PropTypes>();
const attrs = computed(() => {
  const attrs = fieldService.attrs;
  if(isRef(attrs)) return attrs.value as InputFieldAttribute;
  return attrs as InputFieldAttribute;
});const field = useField(fieldService, formService);
</script>


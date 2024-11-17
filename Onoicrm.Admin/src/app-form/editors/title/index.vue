<template>
  <h2 :class="attrs.cssClass">{{ attrs.caption }}</h2>
</template>

<script setup lang="ts">
import { useField } from "../../hooks/useField";
import { InputFieldAttribute } from "../../models/InputFieldAttribute";
import { FieldService } from "../../services/FieldService";
import { FormService } from "../../services/FormService";
import { TitleField } from "./models/TitleField";
interface PropTypes {
  fieldService: FieldService<TitleField>;
  formService: FormService;
}
const { fieldService, formService } = defineProps<PropTypes>();
const { update, items, loaded } = useField(fieldService, formService);

const attrs = computed(() => fieldService.attrs?.value as InputFieldAttribute);
</script>

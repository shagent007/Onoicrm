<template>
  <div :class="attrs.cssClass">
    <label class="mb-1 block" :for="fieldService.id" v-if="attrs.caption">
      {{ attrs.caption }}
    </label>
    <div class="w-full" :class="attrs?.iconPosition">
      <i v-if="attrs?.icon" :class="attrs?.icon" />
      <InputNumber
        v-model.number="fieldService.value"
        :id="fieldService.id"
        type="text"
        @update:modelValue="update(); fieldService.validate();"
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
import {StringField, Watcher} from "~/modules/app-form";
import { InputFieldAttribute } from "~/modules/app-form";
import { FieldService } from "~/modules/app-form";
import { FormService } from "~/modules/app-form";
import InputText from "primevue/inputtext";
import {computed, isRef, onMounted, watch} from "vue";
import {getValue} from "~/shared/lib/helpers";
interface PropTypes {
  fieldService: FieldService<StringField, InputFieldAttribute>;
  formService: FormService;
}

const { fieldService, formService } = defineProps<PropTypes>();

const watchCallBack = async (watcher: Watcher) => {
  const { changeAttrs, changeModel } = watcher;
  if (!!changeAttrs && !!fieldService.actions[changeAttrs]) {
    fieldService.attrs = await fieldService.actions[changeAttrs](fieldService.field.attrs);
  }

  if (!!changeModel && !!fieldService.actions[changeModel]) {
    await fieldService.actions[changeModel](fieldService.model);
  }
};

for (const watcher of fieldService.field.watchers) {
  for (const fieldName of watcher.fields) {
    watch(
        () => getValue(fieldService.model, fieldName),
        async () => await watchCallBack(watcher),
    );
  }
}




onMounted(async () => {
  for (const watcher of fieldService.field.watchers) {
    if (!watcher.callOnMounted) continue;
    await watchCallBack(watcher);
  }
});


const attrs = computed(() => {
  const attrs = fieldService.attrs;
  if (isRef(attrs)) return attrs.value as InputFieldAttribute;
  return attrs as InputFieldAttribute;
});
const { update } = useField(fieldService, formService);
</script>

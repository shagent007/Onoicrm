<template>
  <component :is="container.value" v-slot="{ fieldService }">
    <component
      :is="getProvider(fieldService.field.provider).value"
      :field-service="fieldService"
    >
      <slot :field-service="fieldService" :name="fieldService.field.name">
        <component
          :is="getEditor(fieldService.field).value"
          :field-service="fieldService"
          :form-service="formService"
        />
      </slot>
    </component>
  </component>
</template>

<script setup lang="ts">
import { provide } from "vue";
import { useContainerComponents } from "./hooks/useContainerComponents";
import { useProviderComponents } from "./hooks/useProviderComponents";
import { useEditorComponents } from "./hooks/useEditorComponents";
import { FormService } from "./services/FormService";

const {formService} = defineProps<{formService: FormService}>();
const { container } = useContainerComponents(formService);
const { getProvider } = useProviderComponents();
const { getEditor } = useEditorComponents();

provide("formService", formService);
</script>


<script lang="ts">
import { defineComponent } from "vue";
export default defineComponent({
  name: "app-form",
  inheritAttrs: false,
  customOptions: {},
});
</script>
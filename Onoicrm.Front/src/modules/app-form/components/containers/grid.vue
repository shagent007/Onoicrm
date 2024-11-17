<template>
  <div>
    <div class="grid" :key="row.id" v-for="row in formContainer.rows">
      <div :class="col.size" v-for="col in row.cols" :key="col.id">
        <div class="grid">
          <template v-for="fieldService in col.fields">
            <div
                class="field"
              :class="fieldService.grid"
              v-if="fieldService.canShow()"
              :key="fieldService.id"
            >
              <slot :field-service="fieldService" />
            </div>
          </template>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { inject } from "vue";
import { GridFormContainer } from "../../models/FormContainer";
import { FormService } from "../../services/FormService";
const formService = inject<FormService>("formService");

const formContainer: GridFormContainer =
  formService?.container as GridFormContainer;

if (formService != null) {
  formContainer.initFields(formService);
}
</script>

<script lang="ts">
import { defineComponent } from "vue";
export default defineComponent({
  name: "grid-container",
  inheritAttrs: false,
  customOptions: {},
});
</script>

<style lang="scss">
.field{
  margin-bottom: 16px;
}

</style>
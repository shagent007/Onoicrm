<template>
  <div class="formgrid grid">
    <template v-for="fieldService in formService.fieldServices">

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
</template>

<script setup lang="ts">
import { inject } from "vue";
import { FormService } from "../services/FormService";
import {getValue} from "~/services/helpers";
const formService = inject<FormService>("formService") as FormService;
</script>

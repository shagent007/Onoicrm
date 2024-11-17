<template>
    <template v-if="formService">
        <template v-for="fieldService in formService.fieldServices">
            <template v-if="fieldService.canShow()">
                <slot :field-service="fieldService" />
            </template>
        </template>
    </template>
</template>

<script setup lang="ts">
import { inject } from 'vue';
import { FormService } from '../../services/FormService';
const formService = inject<FormService>('formService');
</script>

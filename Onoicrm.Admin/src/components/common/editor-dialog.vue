<template>
    <Dialog
        :header="editor.title"
        v-model:visible="editor.visible"
        :breakpoints="{ '960px': '75vw', '640px': '90vw' }"
        :style="{ width: '50vw' }"
        :modal="true"
    >
        <app-form v-if="editor.formService !== null" :form-service="(editor.formService as unknown as FormService)" />
        <template #footer>
            <Button label="Отмена" icon="pi pi-times" @click="editor.cancel()" class="p-button-text" />
            <Button label="Сохранить" icon="pi pi-check" @click="editor.submit()" autofocus />
        </template>
    </Dialog>
    <Toast />
</template>

<script setup lang="ts">
import { useEditorStore } from '../../store/useEditorDialog';
import AppForm from '../../app-form/app-form.vue';
import { FormService } from '../../app-form/services/FormService';
const editor = useEditorStore();
</script>

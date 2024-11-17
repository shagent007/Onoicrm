<template>
    <Dialog
        :header="editor.title"
        v-model:visible="editor.visible"
        :breakpoints="{ '960px': '75%', '640px': '100%' }"
        :style="{ width: '50vw' }"
        class="editor-dialog"
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
import { useEditorStore } from '../stores/useEditorDialog';
import AppForm from '~/modules/app-form/components/app-form.vue';
import { FormService } from '~/modules/app-form/services/FormService';
const editor = useEditorStore();
</script>

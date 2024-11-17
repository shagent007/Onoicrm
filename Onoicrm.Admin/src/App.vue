<template>
  <component :is="layout.value">
    <router-view />
  </component>
  <editor-dialog />
  <confirm-dialog />
  <toast />
</template>

<script setup lang="ts">
import { computed, ComputedRef, provide, ShallowRef, shallowRef } from 'vue';
import { useRoute } from 'vue-router';
import AppLayout from './layouts/app.vue';
import EditorDialog from './components/common/editor-dialog.vue';
import LoginLayout from './layouts/login.vue';
import EmptyLayout from './layouts/empty.vue';
import { useConfirm } from 'primevue/useconfirm';
import { useToast } from 'primevue/usetoast';
import { ToastSeverity } from 'primevue/api';

import { IMessage } from './models/IMessage';

const route = useRoute();
const confirm = useConfirm();
//asd
const confirmPlugin = (options: any): Promise<boolean> => {
  let resolve!: Function;
  confirm.require({
    ...options,
    acceptLabel: 'Подтвердить',
    rejectLabel: 'Отмена',
    accept: () => resolve(true),
    reject: () => resolve(false),
    onHide: () => resolve(false),
  });
  return new Promise<boolean>((r) => (resolve = r));
};

const toast = useToast();

const message: IMessage = {
  success: function (message: string, text?: string | undefined): void {
    toast.add({ severity: 'success', summary: message, detail: text, life: 3000 });
  },
  info: function (message: string, text?: string | undefined): void {
    toast.add({ severity: 'info', summary: message, detail: text, life: 3000 });
  },
  error: function (message: string, text?: string | undefined): void {
    toast.add({ severity: 'error', summary: message, detail: text, life: 3000 });
  },
};

provide('confirm', confirmPlugin);
provide('message', message);

const layout: ComputedRef<ShallowRef> = computed(() => {
  let layout;

  switch (route.meta.layout) {
    case 'login':
      layout = shallowRef(LoginLayout);
      break;
    default:
      layout = shallowRef(AppLayout);
      break;
    case 'empty':
      return shallowRef(EmptyLayout);
  }

  return layout;
});
</script>

<style lang="scss">
@import './App.scss';
</style>

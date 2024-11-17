<template>
  <metainfo>
    <template #title="{ content }">
      {{ content ? `${content} | onoicrm` : `onoicrm` }}
    </template>
  </metainfo>
  <component :is="layout.value">
    <router-view />
  </component>
  <editor-dialog />
  <confirm-dialog />
</template>

<script setup lang="ts">
import { computed, ComputedRef, provide, ShallowRef, shallowRef } from "vue";
import { useRoute } from "vue-router";
import AppLayout from "~/shared/layout/app.vue";
import LoginLayout from "~/shared/layout/login.vue";
import EmptyLayout from "~/shared/layout/empty.vue";
import ConfirmDialog from "primevue/confirmdialog";
import { useToast } from "primevue/usetoast";
import { useConfirm } from "primevue/useconfirm";
import { IMessage } from "~/shared";
import EditorDialog from "~/modules/editor/components/editor-dialog.vue";
const route = useRoute();
const confirm = useConfirm();

const confirmPlugin = (options: any): Promise<boolean> => {
  let resolve!: Function;
  confirm.require({
    ...options,
    acceptLabel: "Подтвердить",
    rejectLabel: "Отмена",
    accept: () => resolve(true),
    reject: () => resolve(false),
    onHide: () => resolve(false),
  });
  return new Promise<boolean>((r) => (resolve = r));
};

const t = useToast();

const message: IMessage = {
  success: function (message: string, text?: string | undefined): void {
    t.add({
      severity: "success",
      summary: message,
      life: 3000,
    });
  },
  info: function (message: string, text?: string | undefined): void {
    t.add({
      severity: "info",
      summary: message,
      life: 3000,
    });
  },
  error: function (message: string, text?: string | undefined): void {
    t.add({
      severity: "error",
      summary: message,
      life: 3000,
    });
  },
};
//sds
provide("confirm", confirmPlugin);
provide("message", message);

const layout: ComputedRef<ShallowRef> = computed(() => {
  switch (route.meta?.layout) {
    case "login":
      return shallowRef(LoginLayout);
    case "empty":
      return shallowRef(EmptyLayout);
    default:
      return shallowRef(AppLayout);
  }
});
</script>

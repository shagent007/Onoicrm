<template>
  <div class="page-heade mb-3">
    <div class="flex justify-content-between mb-3">
      <h1 class='mb-0'>{{ title }}</h1>
      <Button @click="editor.update<any>(editorConfig)" icon="pi pi-pencil" class="p-button-rounded p-button-primary p-button-sm" />
    </div>

    <Breadcrumb :home="home" :model="breadcrumbs" />
  </div>

  <slot></slot>

</template>

<script setup lang="ts">
import { useEditorStore } from '../../store/useEditorDialog';
import { EditorDialogConfig } from '../../models/EditorDialogConfig';
import { IObjectDataSource } from '../../models/IObjectDataSource';

interface PropTypes {
  title: string;
  breadcrumbs?: any[];
  editorConfig: EditorDialogConfig;
  dataSource: IObjectDataSource<any>;
}
defineProps<PropTypes>();
const editor = useEditorStore();

const home = {
  label: 'Главная',
  to: '/',
};
</script>


<script lang="ts">
import { defineComponent } from "vue";
export default defineComponent({
  name: "data-item",
  inheritAttrs: false,
  customOptions: {},
});
</script>
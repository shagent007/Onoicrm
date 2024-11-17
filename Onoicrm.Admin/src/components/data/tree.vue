<template>
  <Tree
    v-bind='props'
    v-model:selectionKeys="selectedKey"
    :value="dataSource.transformedRoot.value"
    :meta-key-selection="false"
    class="w-full py-2 px-1"
    @node-select='emit("select", $event)'
    @node-unselect='emit("un-select", $event)'
    v-slot='{node}'
  >
    <context-menu v-slot="{toggle}" :items="treeItems">
      <slot :branch='{node, toggle, selectNode, getBranch, selectedKey }'>
        <b @contextmenu.prevent="toggle($event); selectNode(node)">{{ node.label }}</b>
      </slot>
    </context-menu>
  </Tree>
</template>

<script lang='ts' setup>
import Tree, { TreeNode, TreeProps } from 'primevue/tree';
import ContextMenu from '../common/context-menu.vue';
import { ITreeDataSource } from '../../models/ITreeDataSource';
import {  inject, reactive, Ref, ref } from 'vue';
import { useEditorStore } from '../../store/useEditorDialog';
import {  findElementInTree, removeElementInTree, updateElementInTree } from '../../services/helpers';
import { EditorDialogConfig } from '../../models/EditorDialogConfig';
import { StringField } from '../../app-form/editors/string/models/StringField';
import { InputFieldAttribute } from '../../app-form/models/InputFieldAttribute';
import { MemoField } from '../../app-form/editors/memo/models/MemoField';
import { defaultMessage, IMessage } from '../../models/IMessage';
import { Confirm, defaultConfirm } from '../../models/Confirm';
import { Group } from '../../entities/Group';

const {dataSource} = defineProps<{ dataSource:ITreeDataSource, props?:TreeProps }>();
const editor = useEditorStore();
const selectedKey:Ref = ref(null);
const currentNode:Ref<TreeNode|null> = ref(null);
const message = inject<IMessage>("message",defaultMessage);
const confirm = inject<Confirm>("confirm", defaultConfirm);
const emit = defineEmits<{
  (e:"select", branch:any):void,
  (e:"un-select", branch:any):void
}>();
const selectNode = (node:TreeNode)=>{
  currentNode.value = node;
  selectedKey.value = {[currentNode.value.key]:true}
}

const select = (node:TreeNode) => {
  emit("select", {selectedKey:selectedKey.value, node})
}

const unSelect = (node:TreeNode) => {
  emit("un-select", {selectedKey:selectedKey.value, node})
}

const getBranch = () => findElementInTree(currentNode.value.key, dataSource.root.value);

const fields = [
  new StringField("caption",{
    attrs:new InputFieldAttribute({
      caption:"Заголовок"
    }),
  }),
  new StringField("name",{
    attrs:new InputFieldAttribute({
      caption:"Системная имя"
    }),
  }),
  new MemoField("description",{
    attrs:new InputFieldAttribute({
      caption:"Описание"
    }),
  }),
]

const treeItems:Ref<any[]> = ref([
  {
    label: 'Добавить',
    command:async ()=>{
      const model = reactive<Group>(new Group());
      model.parentId = +currentNode.value.key;
      const result = await editor.create(new EditorDialogConfig({title:"Добавить группу",fields, model}))
      if (!result) return;
      await dataSource.add(result);
      await dataSource.get("all");
      message.success("Успешно добавлено");
    }
  },
  {
    label: 'Редактировать',
    command:async ()=>{
      const model = findElementInTree(currentNode.value.key, dataSource.root.value);
      const result = await editor.create(new EditorDialogConfig({fields, model}))
      if (!result) return;
      await dataSource.update(result);
      updateElementInTree(result,dataSource.root.value,(branch:any)=>Object.assign(branch, result))
      message.success("Успешно обновлено");
    }
  },
  {
    label: 'Статус',
    items:[
      {
        label: 'Создан',
      },
      {
        label: 'Активен',
      },
      {
        label: 'Удалён',
      },
    ]
  },
  {
    label: 'Удалить',
    command: async ()=>{
      const confirmed = await confirm({
        header:"Подвердите своё действие",
        message:"Вы действительно хотите удалить данну группу?"
      });
      if(!confirmed) return;
      await dataSource.remove(+currentNode.value.key);
      removeElementInTree(currentNode.value.key,dataSource.root.value);
      message.success("Успешно удалено");
    }
  },
]);

</script>

<script lang="ts">
import { defineComponent } from "vue";
export default defineComponent({
  name: "data-tree",
  inheritAttrs: false,
  customOptions: {},
});
</script>

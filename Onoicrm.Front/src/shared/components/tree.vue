<template>
  <Tree
    v-model:selectionKeys="selectedKey"
    :value="props.dataSource.transformedRoot.value"
    :meta-key-selection="false"
    selection-mode="single"
    class="w-full py-2 px-1"
    @node-select='emit("select", $event)'
    @node-unselect='emit("un-select", $event)'
    v-slot='{node}'
  >
    <context-menu v-slot="{toggle}" :items="treeMenuItems">

      <slot :branch='{node, toggle, selectNode, getBranch, selectedKey }'>
        <div @contextmenu.stop.prevent="handleClickBranch(toggle,$event,node)" v-if="!slots.default">{{node.label}}</div>

      </slot>
    </context-menu>
  </Tree>
</template>

<script lang='ts' setup>
import Tree, {TreeNode} from 'primevue/tree';
import ContextMenu from './context-menu.vue';
import {ITreeDataSource} from '~/modules/data-sources';
import {inject, onMounted, Ref, ref, useSlots} from 'vue';
import {useEditorStore} from '~/modules/editor/stores/useEditorDialog';
import {findElementInTree, removeElementInTree, updateElementInTree} from '../lib/helpers';
import {EditorDialogConfig} from '~/modules/editor/models/EditorDialogConfig';
import {StringField} from '~/modules/app-form';
import {InputFieldAttribute} from '~/modules/app-form';
import {defaultMessage, IMessage, TConfirm, defaultConfirm} from '~/shared';
import {Group} from '../entities/Group';
import {useClinicStore} from "~/modules/clinic/stores/useClinicStore";

interface PropTypes {
  dataSource: ITreeDataSource,
  refresh?: Function,
  menuItems?: Ref<any[]>,
  createItemForm?: EditorDialogConfig,
  updateItemForm?: EditorDialogConfig,
  permissions?: boolean
}

const slots = useSlots();
const props = withDefaults(defineProps<PropTypes>(), {
  createItemForm: () => new EditorDialogConfig({
    model: new Group(),
    title: "Добавить группу", fields: [
      new StringField("caption", {
        attrs: new InputFieldAttribute({
          caption: "Заголовок"
        }),
      })
    ],
  }),
  updateItemForm: () => new EditorDialogConfig({
    title: "Редактировать группу", fields: [
      new StringField("caption", {
        attrs: new InputFieldAttribute({
          caption: "Заголовок"
        }),
      })
    ]
  }),
  permissions:false
});
const editor = useEditorStore();
const clinicStore = useClinicStore();

const selectedKey: Ref = ref(null);
const currentNode: Ref<TreeNode | any> = ref(null);
const message = inject<IMessage>("message", defaultMessage);
const confirm = inject<TConfirm>("confirm", defaultConfirm);
const expandedKeys = ref<any>({});
const emit = defineEmits<{
  (e: "select", branch: any): void,
  (e: "un-select", branch: any): void
}>();
const selectNode = (node: TreeNode) => {
  currentNode.value = node;
  selectedKey.value = {[currentNode.value.key]: true}
}

const getBranch = () => findElementInTree(currentNode.value.key, props.dataSource.root.value);

const handleClickBranch = (toggle: any, event: any, node: any) => {
  if (props.permissions) {
    toggle(event);
    selectNode(node);
  }
}

const expandNode = (node: any) => {
  if (node.children && node.children.length) {
    expandedKeys.value[node.key] = true;

    for (let child of node.children) {
      expandNode(child);
    }
  }
};

const treeMenuItems: Ref<any[]> = props.menuItems ?? ref([
  {
    label: 'Добавить',
    command: async () => {
      props.createItemForm.model.parentId = +currentNode.value.key;
      const result = await editor.create(props.createItemForm)
      if (!result) return;
      await props.dataSource.add(result);
      if (props.refresh) {
        await props.refresh();
      }
      message.success("Успешно добавлено");
    }
  },
  {
    label: 'Редактировать',
    command: async () => {
      props.updateItemForm.model = findElementInTree(currentNode.value.key, props.dataSource.root.value);
      const result = await editor.create(props.updateItemForm)
      if (!result) return;
      await props.dataSource.update(result);
      updateElementInTree(result, props.dataSource.root.value, (branch: any) => Object.assign(branch, result))
      message.success("Успешно обновлено");
    }
  },
  {
    label: 'Удалить',
    command: async () => {
      const confirmed = await confirm({
        header: "Подвердите своё действие",
        message: "Вы действительно хотите удалить данну группу?"
      });
      if (!confirmed) return;
      await props.dataSource.remove(+currentNode.value.key);
      removeElementInTree(currentNode.value.key, props.dataSource.root.value);
      message.success("Успешно удалено");
    }
  },
]);

onMounted(() => {
  for (let node of props.dataSource.transformedRoot.value) {
    expandNode(node);
  }
})
</script>

<script lang="ts">
import {defineComponent} from "vue";

export default defineComponent({
  name: "data-tree",
  inheritAttrs: false,
  customOptions: {},
});
</script>

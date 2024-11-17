<template>
  <div :class="attrs.cssClass">
    <div class="flex align-items-center justify-content-between">
      <label class="mb-1 block" v-if="attrs?.caption">
        {{ attrs.caption }}
      </label>

      <Button @click="select" label="Добавить элемент"></Button>
    </div>
    <component :is='collectionContainer.editor.value' >
      <component
        v-for="item in fieldService.value"
        :is='collectionItem.editor.value'
        :key="item.id"
        :data='item'
        :config='itemConfig'
        :item='fieldService.field.item'
        @remove='removeItem(item)'
      />
    </component>


    <small v-if="attrs?.hint">
      {{ attrs.hint }}
    </small>
    <div
      v-if="fieldService?.showErrors"
      v-for="error in fieldService?.errorMessages.value"
      :key="error"
    >
      <small class="p-error">{{ error }}</small>
    </div>
  </div>

  <component
    :is='selectorComponent'
    ref="selector"
    title="Выберите докторов"
    :meta-info="metaInfo"
    :custom-sort="customSort"
    :data-source="dataSource"
    :exclude-call-back="excludeCallBack"
    @select="add"
    @unSelect='removeItem($event)'
  />
</template>


<script lang="ts" setup>
import { useField } from "../hooks/useField";
import { FieldService } from "../services/FieldService";
import { FormService } from "../services/FormService";
import { computed, inject, onMounted, ref } from 'vue';
import {FieldAttribute} from "../models/FieldAttribute";
import {remove} from "../../services/helpers"
import { CollectionField } from './models/CollectionField';
import { CollectionFieldConfig } from './models/CollectionFieldConfig';
import { defaultMessage, IMessage } from '../../models/IMessage';
import { useCollectionContainerComponents } from './hooks/useCollectionContainerComponents';
import { useCollectionItemComponents } from './hooks/useCollectionItemComponents';
import { useSelectorComponents } from './hooks/useSelectorComponents';

const { fieldService, formService } = defineProps<{
  fieldService: FieldService<CollectionField>;
  formService: FormService;
}>();

const collectionContainer = useCollectionContainerComponents(fieldService.field.container.name);
const collectionItem = useCollectionItemComponents(fieldService.field.item.name);
const selectorComponent = useSelectorComponents(fieldService.field.selector)
const selector = ref();
const message = inject<IMessage>("message", defaultMessage)
const attrs = computed(() => fieldService.attrs as FieldAttribute);
const { update,loaded, items } = useField(fieldService as FieldService, formService);
const {dataSource, metaInfo, customSort} = fieldService.actions[fieldService.field.getConfig]();
const customAdd = fieldService.actions?.[fieldService.field?.customAdd];
const customDelete = fieldService.actions?.[fieldService.field.customDelete];
const itemConfig = fieldService.actions?.[fieldService.field?.getItemConfig]();
const customExcludeCallBack = fieldService.actions?.[fieldService.field.getCustomExcludeCallBack]
const getCallBack = (item:any) => customExcludeCallBack
  ? customExcludeCallBack(item)
  : ((i:any) => i.id == item.id)
const excludeCallBack = (item:any)=> !fieldService.value.some(getCallBack(item));
const select = async () => await selector.value.open();
const add = async (data:any) => {
  if (customAdd) return await customAdd(data, fieldService.model);
  fieldService.value.push(data);
}

const removeItem = async (data:any) => {
   if (customDelete) return await customDelete(data, fieldService.model);
  remove(fieldService.value,(i:any)=>i.id==data.id);
}


</script>


<script lang="ts">
import { defineComponent } from "vue";
export default defineComponent({
  name: "collection-editor",
  inheritAttrs: false,
  customOptions: {},
});
</script>
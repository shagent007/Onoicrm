<template>
  <Dialog v-if="toothStateStore.loaded" v-model:visible="visible" modal :style="{ width: '50vw' }" class="tooth-state__mailing-dialog">
    <template #header>
      <div class="flex justify-content-end">Сделать рассылку по категориям</div>
    </template>
    <app-form   :form-service="formService" class="h-full w-full"/>
    <template #footer>
      <div class="pt-4">
        <Button @click="reset()" class="p-button-secondary" label="Отмена" />
        <Button @click="submit()" label="Отправить" />
      </div>
    </template>
  </Dialog>
</template>

<script lang="ts" setup>
import _ from "lodash";
import moment from "moment";
import { reactive, ref, inject } from "vue";
import {usePatientListDataSource} from "~/modules/user-profile/hooks/usePatientListDataSource";
import {CategoryMailingModel} from "~/modules/user-profile/models/CategoryMailingModel";
import {useClinicStore} from "~/modules/clinic/stores/useClinicStore";
import {useToothStateStore} from "~/modules/tooth-state";
import AppForm from "~/modules/app-form/components/app-form.vue";
import {
  FieldAttribute,
  FormService,
  FormServiceConfig,
  InputFieldAttribute,
  MemoField,
  SelectField,
  SelectFieldConfig
} from "~/modules/app-form";
import {CollectionField} from "~/modules/app-form/components/collections/models/CollectionField";
import {CollectionChipItem} from "~/modules/app-form/components/collections/items/models/CollectionChipItem";
import {defaultMessage, EntityMetaInfo, IMessage} from "~/shared";
import {remove} from "~/shared/lib/helpers";
import {required} from "~/shared/consts/consts";
const clinicStore = useClinicStore();

const toothStateStore = useToothStateStore();
const resolve = ref<(model: any) => any>();
const model = reactive<CategoryMailingModel>(
  new CategoryMailingModel(
    clinicStore.clinic?.id as number
  )
);
const message = inject<IMessage>("message", defaultMessage);
const visible = ref<boolean>();
const dataSource = usePatientListDataSource();
const formService = new FormService(
    new FormServiceConfig({
      model: model,
      fields: [
        new CollectionField("categories",{
          attrs: new FieldAttribute({
            caption:"Категории"
          }),
          getConfig:"getConfig",
          getItemConfig:"getItemConfig",
          customAdd:"addCategory",
          customDelete:"deleteCategory",
          getCustomExcludeCallBack:"categoryExcludeCallBack",
          item: new CollectionChipItem({
            attrs: {
              class:"mr-2 mb-2"
            }
          }),
          validations:[required()]
        }),
        new MemoField("text", {
          rows: 7,
          attrs: new InputFieldAttribute({
            placeholder: "Жалоба",
          }),
          validations:[required()]
        }),
      ],
      actions: {
        getConfig:()=>{
          const metaInfo =  new EntityMetaInfo({
            columns:[
              {
                field: 'caption',
                header: 'Название категории',
                sortable:true
              }
            ],
          })
          return {dataSource: toothStateStore, metaInfo, customSort:null}
        },
        addCategory:(data:any, model:CategoryMailingModel)=>{
          model.categories.push(data.id)
        },
        deleteCategory: (data:any, model:CategoryMailingModel)=>{
          remove(model.categories,(i:any)=>i ==data);
        },
        getItemConfig:()=>({ getCaption:(id:number)=>toothStateStore.findItem(id)?.caption }),
        categoryExcludeCallBack:(item:any)=>(i:any) => i == item.id,
      },
    }),
);
const reset = () => {
  model.categories =[];
  model.text = "";
  visible.value = false;
  resolve.value!(null);
};

const open = async () => {
  visible.value = true;
  return new Promise((r) => (resolve.value = r));
};

const submit = async () => {
  const valid = await formService.validate();
  if (!valid){
    message.error('Заполните форму правильно')
    return
  }
  await dataSource.mailingByCategory({...model})
  message.success('Успешно отправлено!')
  visible.value = false;
  resolve.value?.(null);
  reset();
};

defineExpose({ open });
</script>

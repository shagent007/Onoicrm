<template>
  <spreadsheet :time-interval="15" time-range="08:00-21:00">
    <template #tabs>
      <TabView class="tab--filter-view" :scrollable="true">
        <TabPanel v-for="item in armchairStore.items" :key="item.id">
          <template #header="data">
            <context-menu :items="items" v-slot="{toggle}">
              <Button
                style="white-space:nowrap;"
                size="small"
                rounded
                :label="item.caption"
                @click.prevent="updateFilter(item)"
                @contextmenu.prevent="
                  toggle($event);
                  currentItem=item
                "
              />
            </context-menu>
          </template>
        </TabPanel>
      </TabView>
    </template>
  </spreadsheet>
</template>

<script setup lang="ts">
import spreadsheet from "~/modules/booking/components/speadsheet.vue"
import {ContextMenu} from "~/shared"
import {IListDataSource, UpdateFieldModel} from "~/modules/data-sources";
import {computed, inject, reactive, Ref, ref} from "vue";
import {defaultConfirm, defaultMessage, IMessage, TConfirm} from "~/shared";
import {getGUID} from "~/shared/lib/getGUID";
import {UserRegisterModel} from "~/modules/user/models/UserResisterModel";
import {useClinicStore} from "~/modules/clinic/stores/useClinicStore";
import {useEditorStore} from "~/modules/editor/stores/useEditorDialog";
import {EditorDialogConfig} from "~/modules/editor/models/EditorDialogConfig";
import {EmptyField} from "~/modules/app-form/components/editors/empty/models/StringField";
import {InputFieldAttribute, MaskField, MaskFieldConfig, MemoField, StringField, Watcher} from "~/modules/app-form";
import {required} from "~/shared/consts/consts";
import {translit} from "~/shared/lib/helpers";
import {useArmchairStore} from "~/modules/armchair";
const {} = defineProps<{
  bookingDataSource:IListDataSource<any>,
}>();

const message = inject<IMessage>("message", defaultMessage);
const confirm = inject<TConfirm>("confirm", defaultConfirm);
const clinicStore = useClinicStore();
const armchairStore = useArmchairStore();
const currentItem: Ref = ref();

const items = ref([
]);
const updateFilter = async (doctor:any) => {
  console.log(doctor.id)
}


</script>

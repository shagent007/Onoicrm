<template>
  <div v-if="loaded" class="grid">
    <div class="col-12 flex md:hidden">
      <div class="gap-3 flex">
        <Button
          v-if="appStore.buttons.length > 0"
          v-for="button in appStore.buttons"
          @click="button.action()"
          class="text-sm mr-2"
        >
          <div class="mr-2">
            <i class="pi pi-user-plus"></i>
          </div>
          Добавить
        </Button>
        <Button class="text-sm flex md:hidden">
          <div class="mr-2">
            <i class="pi pi-comments"></i>
          </div>
          Рассылка
        </Button>
      </div>
    </div>
    <div class="hidden md:block col-12  lg:mb-0">
      <div class="card block indent-table">
        <DataTable
          scrollable
          scroll-height="calc(100vh - 238px)"
          lazy
          @sort="dataSource.sort($event)"
          :rows="dataSource.pageSize.value"
          :value="dataSource.items.value"
          @rowClick="$router.push(`/patients/${$event.data.id}`)"
        >
          <Column sortable field="fullName" header="ФИО" />
          <Column field="birthDate" header="Д/рождение">
            <template #body="{ data }">
              <p v-if="data.birthDate">
                {{ moment(data?.birthDate).format("DD.MM.YYYY") }}
              </p>
              <p v-else>не указано!</p>
            </template>
          </Column>
          <Column field="genderCaption" header="Пол">
            <template #body="{ data }">
              {{ genderCaption(data?.genderId) }}
            </template>
          </Column>
          <Column field="whatsAppNumber" header="Номер Whatsapp" />
          <Column v-if="hasPermissions">
            <template #body="{ data }">
              <context-menu v-slot="{ toggle }" :items="items">
                <Button
                  @click="
                    currentItem = data;
                    toggle($event);
                  "
                  icon="pi pi-ellipsis-v text-sm"
                  class="p-button-outlined p-button-sm btn-table"
                />
              </context-menu>
            </template>
          </Column>
        </DataTable>
      </div>
    </div>
    <div class="hidden md:block lg:hidden col-12 lg:col-9">
      <div class="block tablet-patient-table indent-table px-0">
        <DataTable
          scrollable
          scroll-height="calc(100vh - 238px)"
          lazy
          @sort="dataSource.sort($event)"
          :rows="dataSource.pageSize.value"
          :value="dataSource.items.value"
          @rowClick="$router.push(`/patients/${$event.data.id}`)"
        >
          <Column sortable field="fullName" header="ФИО" />
          <Column field="birthDate" header="Д/рождение">
            <template #body="{ data }">
              <p v-if="data.birthDate">
                {{ moment(data?.birthDate).format("DD.MM.YYYY") }}
              </p>
              <p v-else>не указано!</p>
            </template>
          </Column>
          <Column field="genderCaption" header="Пол">
            <template #body="{ data }">
              {{
                genderCaption(data?.genderId) == "Женский"
                  ? "Жен"
                  : "" || genderCaption(data?.genderId) == "Мужской"
                  ? "Муж"
                  : "" || genderCaption(data?.genderId) == "Не определено"
                  ? "Не опр."
                  : ""
              }}
            </template>
          </Column>
          <Column field="whatsAppNumber" header="Номер Whatsapp" />
          <Column v-if="hasPermissions">
            <template #body="{ data }">
              <context-menu v-slot="{ toggle }" :items="items">
                <Button
                  @click="
                    currentItem = data;
                    toggle($event);
                  "
                  icon="pi pi-ellipsis-v text-sm"
                  class="p-button-outlined p-button-sm btn-table"
                />
              </context-menu>
            </template>
          </Column>
        </DataTable>
      </div>
    </div>

    <div
      v-touch:swipe.left="swipeLeft"
      v-touch:swipe.right="swipeRight"
      class="block md:hidden col-12 mb-3"
    >
      <div
        v-for="item in dataSource.items.value"
        :key="item.id"
        class="card flex-column p-3 mb-3 indent-table"
      >
        <div
          class="flex align-items-center justify-content-between w-full mb-3"
        >
          <h6
            @click="router.push(`/patients/${item.id}`)"
            class="mobile-card-caption"
          >
            {{ item.fullName }}
          </h6>
          <context-menu v-slot="{ toggle }" :items="items">
            <Button
              @click="
                currentItem = item;
                toggle($event);
              "
              icon="pi pi-ellipsis-v text-xs md:text-sm"
              class="p-button-outlined p-button-sm btn-table"
            />
          </context-menu>
        </div>
        <div class="flex items-center justify-content-between w-full">
          <p class="mobile-card-description">
            {{ genderCaption(item?.genderId) }}
          </p>
          <p v-if="item.birthDate" class="mobile-card-description">
            {{ moment(item?.birthDate).format("DD.MM.YYYY") }}
          </p>
          <p class="mobile-card-description">{{ item.whatsAppNumber }}</p>
        </div>
      </div>
    </div>

    <div class="col-12">
      <Paginator
        class="datatable-paginator"
        :template="{
          '640px': 'PrevPageLink CurrentPageReport NextPageLink',
          '960px':
            'FirstPageLink PrevPageLink CurrentPageReport NextPageLink LastPageLink',
          '1300px':
            'FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink',
          default:
            'FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink',
        }"
        :rows="dataSource.pageSize.value"
        :model-value="dataSource.pageIndex.value"
        :totalRecords="dataSource.total.value"
        @page="dataSource.paginate"
      />
    </div>
  </div>
<!--  <mailing-dialog ref="mailingDialog" />-->

</template>

<script setup lang="ts">
import { useAppStore } from "~/modules/app/stores/useAppStore";
import moment from "moment";
import {
  Filter,
  ListDataSourceConfig,
  useListDataSource,
} from "~/modules/data-sources";
import { computed, inject, onMounted, ref, Ref, watch } from "vue";
import ContextMenu from "~/shared/components/context-menu.vue";
import { useClinicStore } from "~/modules/clinic/stores/useClinicStore";
import {
  formatWhatsappNumber,


} from "~/shared/lib/helpers";
import { defaultMessage, IMessage } from "~/shared";
import { TConfirm, defaultConfirm } from "~/shared";
import { useEditorStore } from "~/modules/editor/stores/useEditorDialog";
import { usePatientListDataSource } from "~/modules/user-profile/hooks/usePatientListDataSource";
import { useRouter } from "vue-router";
import { ButtonModel } from "~/modules/app/models/ButtonModel";
import DataTable from "primevue/datatable";
import { useUserStore } from "~/modules/user/stores/useUserStore";
import { getPatientUpdateItemForm } from "~/modules/patient/forms/getPatientUpdateItemForm";
import { getPatientCreateItemForm } from "~/modules/patient/forms/getPatientCreateItemForm";
import { genderCaption } from "~/modules/patient/services/getGenderCaption";
import {Patient} from "~/modules/user-profile/Patient";

const message = inject<IMessage>("message", defaultMessage);
const confirm = inject<TConfirm>("confirm", defaultConfirm);

const router = useRouter();
const clinicStore = useClinicStore();
const appStore = useAppStore();
const userStore = useUserStore();
const editorStore = useEditorStore();
const dataSource = usePatientListDataSource();

const informationSourceDataSource = useListDataSource(
  new ListDataSourceConfig({
    className: "InformationSource",
  })
);

const currentItem: Ref = ref();
const timeOut = ref();
const hasPermissions = computed(() =>
  userStore.hasOptionalRoles([
    "System Administrator",
    "Administrator",
    "Director",
  ])
);
const loaded: Ref<boolean> = ref(false);
const test = ref<any>();
const swipeLeft = async () => {
  if (
    dataSource.pageIndex.value ===
    Math.ceil(dataSource.total.value / dataSource.pageSize.value)
  )
    return;
  dataSource.pageIndex.value++;
  test.value = dataSource.pageIndex.value;
  await dataSource.get();
};

const swipeRight = async () => {
  if (dataSource.pageIndex.value == 1) return;
  dataSource.pageIndex.value--;
  await dataSource.get();
};


//AS
const items = ref([
  {
    label: "Редактировать",
    icon: "pi pi-fw pi-pencil text-indigo-500",
    command: async () => {
      const config = getPatientUpdateItemForm(
        currentItem.value,
        dataSource,
        informationSourceDataSource
      );

      const result = await editorStore.create(config);
      
    },
  },
  {
    label: "Написать сообшение",
    icon: "pi pi-fw pi-comment text-indigo-500",
    command: () => {
      const formattedNumber = formatWhatsappNumber(
        currentItem.value.whatsAppNumber
      );
      window.open(`https://wa.me/${formattedNumber}`, "_blank");
    },
  },
]);


watch(
  () => appStore.searchText,
  async (newValue: string) => {
    clearTimeout(timeOut.value);
    timeOut.value = setTimeout(async () => {
      await dataSource.updateFilter([new Filter("searchText", newValue)]);
    }, 800);
  }
);

if (hasPermissions.value) {
  appStore.buttons = [
    new ButtonModel({
      attrs: {
        icon: "pi pi-user-plus text-xl",
      },
      action: async () => {
        const model = new Patient({
          clinicId: clinicStore.clinic?.id,
        });
        const config = getPatientCreateItemForm(model, informationSourceDataSource);
        const data = await editorStore.create(config);
        const result = await dataSource.add(data);
        dataSource.items.value.unshift(result);
        message.success("Пациент успешно добавлен");
      },
    }),
  ];
}

onMounted(async () => {
  await dataSource.get()
  loaded.value = true;
});
</script>
<style>

</style>
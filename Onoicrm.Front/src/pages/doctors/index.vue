<template>
  <div v-if="loaded">
    <div class="flex md:hidden gap-3 mb-3">
      <Button
        v-if="appStore.buttons.length > 0"
        v-for="button in appStore.buttons"
        @click="button.action()"
        class="text-sm"
      >
        <div class="mr-2">
          <i class="pi pi-user-plus text-sm"></i>
        </div>
        Добавить
      </Button>
    </div>
    <div
      style="height: calc(100vh - 205px); border-radius: 10px"
      class="hidden md:block card indent-table indent-table--height mb-7 lg:mb-0"
    >
      <DataTable
        scrollable
        scroll-height="calc(100vh - 238px)"
        lazy
        @sort="sort"
        :rows="dataSource.pageSize.value"
        :value="dataSource.activeItems.value"
        @rowClick="selectDoctor($event)"
      >
        <Column sortable field="fullName" header="ФИО" />
        <Column field="birthDate" header="Д/Р">
          <template #body="{ data }">
            {{ moment(data?.birthDate).format("DD.MM.YYYY") }}
          </template>
        </Column>
        <Column field="roles" header="Доступы">
          <template #body="{ data }">
            <Tag
              rounded
              class="mr-2 mb-2 font-normal"
              v-for="role in data?.roles"
              :value="role"
            />
          </template>
        </Column>
        <Column field="jobPosition" header="Должность" />
        <Column field="whatsAppNumber" header="Номер Whatsapp" />
        <Column v-if="hasPermissions">
          <template #body="{ data }">
            <context-menu v-slot="{ toggle }" :items="menuItems">
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
    <div class="mb-7 lg:mb-4">
      <div
        v-touch:swipe.left="swipeLeft"
        v-touch:swipe.right="swipeRight"
        v-for="item in dataSource.activeItems.value"
        class="block md:hidden card flex-column p-3 mb-3 indent-table"
      >
        <div
          class="flex align-items-center justify-content-between w-full mb-3"
        >
          <h6
            @click="router.push(`/doctors/${item.id}`)"
            class="mobile-card-caption"
          >
            {{ item.fullName }}
          </h6>
          <context-menu v-slot="{ toggle }" :items="menuItems">
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
        <div class="flex items-center justify-content-between w-full mb-3">
          <p v-if="item.birthDate" class="mobile-card-description">
            {{ moment(item?.birthDate).format("DD.MM.YYYY") }}
          </p>
          <p class="mobile-card-description">{{ item.jobPosition }}</p>
          <p class="mobile-card-description">{{ item.whatsAppNumber }}</p>
        </div>
        <div>
          <Tag
            rounded
            class="mr-2 mb-2 font-normal"
            v-for="role in item?.roles"
            :value="role"
          />
        </div>
      </div>
    </div>
    <Paginator
      class="datatable-paginator"
      :rows="dataSource.pageSize.value"
      :model-value="dataSource.pageIndex.value"
      :totalRecords="dataSource.total.value"
      @page="paginate"
      :template="{
        '640px': 'PrevPageLink CurrentPageReport NextPageLink',
        '960px':
          'FirstPageLink PrevPageLink CurrentPageReport NextPageLink LastPageLink',
        '1300px':
          'FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink',
        default:
          'FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink',
      }"
    />
  </div>
</template>

<script setup lang="ts">
import { useAppStore } from "~/modules/app/stores/useAppStore";
import { useMeta } from "vue-meta";
import moment from "moment";
import {
  Filter,
  ListDataSourceConfig,
  UpdateFieldModel,
  useListDataSource,
} from "~/modules/data-sources";
import { inject, onMounted, reactive, ref, watch } from "vue";
import ContextMenu from "~/shared/components/context-menu.vue";
import { useClinicStore } from "~/modules/clinic/stores/useClinicStore";
import { UserRegisterModel } from "~/modules/user/models/UserResisterModel";
import { EditorDialogConfig } from "~/modules/editor/models/EditorDialogConfig";
import { checkUserNameAsync, required } from "~/shared/consts/consts";
import {
  DateFieldAttribute,
  FieldAttribute,
  PasswordField,
  SelectField,
  SelectFieldConfig,
  SelectMode,
  StringField,
} from "~/modules/app-form";
import { InputFieldAttribute } from "~/modules/app-form";
import { getValue, remove, toPascalCase } from "~/shared/lib/helpers";
import { MaskField } from "~/modules/app-form";
import { MaskFieldConfig } from "~/modules/app-form";
import { defaultMessage, EntityMetaInfo, IMessage } from "~/shared";
import { TConfirm, defaultConfirm } from "~/shared";
import { useEditorStore } from "~/modules/editor/stores/useEditorDialog";
import MailingDialog from "~/modules/user-profile/components/tooth-state-mailing-dialog.vue";
import { useRouter } from "vue-router";
import { ButtonModel } from "~/modules/app/models/ButtonModel";
import DataTable, { DataTableSortEvent } from "primevue/datatable";
import { computed } from "vue";
import { useUserStore } from "~/modules/user/stores/useUserStore";
import { useUserRoleDataSource } from "~/modules/user-role/data-sources/useUserRoleDataSources";
import { SetRoleModel } from "~/modules/data-sources/hooks/setRoleModel";
import { CollectionField } from "~/modules/app-form/components/collections/models/CollectionField";
import { CollectionChipItem } from "~/modules/app-form/components/collections/items/models/CollectionChipItem";
import { UserProfile } from "~/modules/user-profile/UserProfile";
import { PhoneField } from "~/modules/app-form/components/editors/phone/models/PhoneField";
import { genderCaption } from "~/modules/patient/services/getGenderCaption";
import { useWindowSize } from "@vueuse/core";

const message = inject<IMessage>("message", defaultMessage);
const confirm = inject<TConfirm>("confirm", defaultConfirm);
const userStore = useUserStore();
const router = useRouter();
const clinicStore = useClinicStore();
const appStore = useAppStore();
const editorStore = useEditorStore();

const userRoleDataSource = useUserRoleDataSource();
const roleDataSource = useListDataSource(
  new ListDataSourceConfig({
    className: "role",
    pageSize: 100,
    filter: [new Filter("names", "Dentist,Administrator")],
  })
);
const dataSource = useListDataSource(
  new ListDataSourceConfig({
    className: "userProfile",
    filter: [
      new Filter("clinicId", clinicStore.clinic?.id),
    ],
  })
);

const loaded = ref<boolean>(false);
const currentItem = ref();
const timeOut = ref();

const hasPermissions = computed(() =>
  userStore.hasOptionalRoles([
    "System Administrator",
    "Administrator",
    "Director",
  ])
);
const menuItems = ref([
  {
    label: "Редактировать",
    icon: "pi pi-fw pi-pencil text-indigo-500",
    command: async () => {
      const config = new EditorDialogConfig({
        title: "Обновить данные",
        model: currentItem.value,
        fields: [
          new StringField("fullName", {
            attrs: new InputFieldAttribute({
              caption: "ФИО",
              placeholder: "Введите ФИО Сотрудника",
            }),
            validations: [required()],
            grid: "col-6",
          }),
          new MaskField("birthDate", {
            attrs: new DateFieldAttribute({
              placeholder: "Введите дату рождения",
              caption: "Дата рождения",
            }),
            get: "getDate",
            set: "setDate",
            config: new MaskFieldConfig({
              mask: "99.99.9999",
            }),
            grid: "col-6",
          }),
          new PhoneField("whatsAppNumber", {
            attrs: new InputFieldAttribute({
              caption: "WhatsApp номер",
              placeholder: "Введите WhatsApp номер",
            }),
            grid: "col-6",
          }),
          new SelectField("genderId", {
            attrs: new InputFieldAttribute({
              caption: "Пол",
              cssClass: "mb-3",
            }),
            config: new SelectFieldConfig({
              getItems: "getGenders",
              labelKeyName: "caption",
              valueKeyName: "value",
              mode: SelectMode.BtnGroup,
            }),
            grid: "col-6",
          }),
          new StringField("jobPosition", {
            attrs: new InputFieldAttribute({
              caption: "Должность",
              placeholder: "Введите должность",
            }),
            grid: "col-6",
          }),
          new StringField("address", {
            attrs: new InputFieldAttribute({
              caption: "Адрес",
              placeholder: "Введите адрес",
            }),
            grid: "col-6",
          }),
          new CollectionField("roles", {
            attrs: new FieldAttribute({
              caption: "Доступы",
            }),
            getConfig: "getRolesConfig",
            customAdd: "addRole",
            customDelete: "deleteRole",
            getItemConfig: "getUpdateRoleItemConfig",
            getCustomExcludeCallBack: "getUpdateRoleExcludeCallBack",
            item: new CollectionChipItem({
              attrs: {
                class: "mr-2 mb-2",
              },
            }),
          }),
        ],
        actions,
        updateFieldService: dataSource,
      });

      await editorStore.update(config);
    },
  },
  {
    label: "Написать сообшение",
    icon: "pi pi-fw pi-comment text-indigo-500",
    command: () => {
      let phoneNumber = currentItem.value.whatsAppNumber;
      let formattedNumber = phoneNumber.replace(/\D/g, "");
      if (phoneNumber.startsWith("+")) {
        formattedNumber = "+" + formattedNumber;
      }
      window.open(`https://wa.me/${formattedNumber}`, "_blank");
    },
  },
  {
    label: "Удалить",
    icon: "pi pi-fw pi-trash text-red-500",
    command: async () => {
      const _confirm = await confirm({
        message: "Вы действительно хотите удалить данный обьект?",
        header: "Подтвердите свое действие",
        icon: "pi pi-info-circle",
        acceptClass: "p-button-danger",
      });
      if (!_confirm) return;
      await dataSource.updateField(currentItem.value.id, [
        new UpdateFieldModel({
          fieldName: "stateId",
          fieldValue: 3,
        }),
      ]);
      message.success("Успешно удалено");
    },
  },
]);
const newDoctorModel: UserRegisterModel = reactive<UserRegisterModel>(
  new UserRegisterModel({
    userProfile: new UserProfile({
      clinicId:clinicStore.clinic?.id,
      stateId: 2,
    }),
  })
);

const selectDoctor = (e: any) => {
  if (hasPermissions.value) {
    router.push(`/doctors/${e.data.id}`);
  }
};
const actions = {
  getRolesConfig: () => {
    const metaInfo = new EntityMetaInfo({
      columns: [
        {
          field: "name",
          header: "Название роли",
          sortable: true,
        },
      ],
    });
    return { dataSource: roleDataSource, metaInfo, customSort: null };
  },
  getDate: (model: UserProfile, name: string) => {
    const value = moment(getValue(model, name));
    if (!value.isValid()) return "";
    return value.format("DD.MM.YYYY");
  },
  setDate: (model: UserProfile, name: string, newValue: any) => {
    const value = moment(newValue, "DD.MM.YYYY");
    if (value.isSame(moment(model.birthDate))) return;
    if (!value.isValid()) return "";
    model.birthDate = value.utc().format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");
  },
  setRegisterDate: (model: UserRegisterModel, name: string, newValue: any) => {
    const value = moment(newValue, "DD.MM.YYYY");
    if (!value.isValid()) return "";
    model.userProfile.birthDate = value
      .utc()
      .format("YYYY-MM-DDTHH:mm:ss.SSS[Z]");
  },
  getRoleItemConfig: () => ({
    getCaption: (item: any) => item.name,
  }),
  getUpdateRoleItemConfig: () => ({
    getCaption: (item: any) => item,
    getIcon: (item: any) => {
      const isInclude = ["Dentist", "Administrator"].includes(item);
      console.log(isInclude, item);
      return isInclude ? "pi pi-trash" : "true";
    },
  }),
  getUpdateRoleExcludeCallBack: (item: any) => (i: any) => i == item.name,
  addRole: async (role: any, model: any) => {
    await userRoleDataSource.setRole(
      new SetRoleModel({
        roleName: role.name,
        userId: model.userId,
      })
    );
    model.roles.push(role.name);
  },
  deleteRole: async (role: any, model: any) => {
    if (!["Dentist", "Administrator"].includes(role)) {
      return;
    }
    await userRoleDataSource.deleteRole(
      new SetRoleModel({
        roleName: role,
        userId: model.userId,
      })
    );
    remove(model.roles, (r: any) => r == role);
  },
  getGenders: () => [
    { caption: "Женский", value: 0 },
    { caption: "Мужской", value: 1 },
  ],
};
const doctorConfig = new EditorDialogConfig({
  title: "Добавить нового сотрудника",
  model: newDoctorModel,
  fields: [
    new StringField("userProfile.fullName", {
      attrs: new InputFieldAttribute({
        caption: "ФИО",
        placeholder: "Введите ФИО Сотрудника",
      }),
      validations: [required()],
      grid: "col-6",
    }),
    new MaskField("userProfile.birthDate", {
      attrs: new DateFieldAttribute({
        placeholder: "Введите дату рождения",
        caption: "Дата рождения",
      }),
      get: "getDate",
      set: "setRegisterDate",
      config: new MaskFieldConfig({
        mask: "99.99.9999",
      }),
      grid: "col-6",
    }),
    new PhoneField("userProfile.whatsAppNumber", {
      attrs: new InputFieldAttribute({
        caption: "WhatsApp номер",
        placeholder: "Введите WhatsApp номер",
      }),
      grid: "col-6",
    }),
    new SelectField("userProfile.genderId", {
      attrs: new InputFieldAttribute({
        caption: "Пол",
        cssClass: "mb-3",
      }),
      config: new SelectFieldConfig({
        getItems: "getGenders",
        labelKeyName: "caption",
        valueKeyName: "value",
        mode: SelectMode.BtnGroup,
      }),
      grid: "col-6",
    }),
    new StringField("userProfile.jobPosition", {
      attrs: new InputFieldAttribute({
        caption: "Должность",
        placeholder: "Введите должность",
      }),
      validations: [required()],
      grid: "col-6",
    }),
    new StringField("email", {
      attrs: new InputFieldAttribute({
        caption: "Логин",
        placeholder: "Придумайте логин",
      }),
      grid: "col-6",
      validations: [required(), checkUserNameAsync()],
    }),
    new PasswordField("password", {
      attrs: new InputFieldAttribute({
        caption: "Пароль",
        placeholder: "Придумайте пароль",
      }),
      grid: "col-6",
    }),
    new PasswordField("confirmPassword", {
      attrs: new InputFieldAttribute({
        caption: "Подтверждение пароля",
        placeholder: "Подтверите пароль",
      }),
      grid: "col-6",
    }),
    new StringField("userProfile.address", {
      attrs: new InputFieldAttribute({
        caption: "Адрес",
        placeholder: "Введите адрес",
      }),
    }),
    new CollectionField("roles", {
      attrs: new FieldAttribute({
        caption: "Доступы",
      }),
      getConfig: "getRolesConfig",
      getItemConfig: "getRoleItemConfig",
      item: new CollectionChipItem({
        attrs: {
          class: "mr-2 mb-2",
        },
      }),
    }),
  ],
  actions,
});

const paginate = async (event: any) => {
  dataSource.pageIndex.value = event.page + 1;
  dataSource.pageSize.value = event.rows;
  await dataSource.get();
};

const swipeLeft = async () => {
  if (
    dataSource.pageIndex.value ===
    Math.ceil(dataSource.total.value / dataSource.pageSize.value)
  )
    return;
  dataSource.pageIndex.value++;
  await dataSource.get();
};

const swipeRight = async () => {
  if (dataSource.pageIndex.value == 1) return;
  dataSource.pageIndex.value--;
  await dataSource.get();
};

watch(
  () => appStore.searchText,
  async (newValue: string) => {
    clearTimeout(timeOut.value);
    timeOut.value = setTimeout(async () => {
      await dataSource.updateFilter([new Filter("searchText", newValue)]);
    }, 1300);
  }
);
const sort = async (event: DataTableSortEvent) => {
  dataSource.orderFieldName.value = toPascalCase(event.sortField as string);
  dataSource.orderFieldDirection.value = event.sortOrder == 1 ? "ASC" : "DESC";
  await dataSource.get();
};

useMeta({
  title: "Врачи",
  htmlAttrs: { lang: "en", amp: true },
});

if (hasPermissions.value) {
  appStore.buttons = [
    new ButtonModel({
      attrs: {
        icon: "pi pi-user-plus text-xl",
        class: " ml-5 px-4 border-round-xl",
      },
      action: async () => {
        const data = await editorStore.create(doctorConfig);
        if (!data) return;
        const result = await dataSource.add(data);
        dataSource.items.value.push(result);
        message.success("Сотрудник успешно добавлен");
      },
    }),
  ];
}

onMounted(async () => {
  await Promise.all([dataSource.get()]);
  loaded.value = true;
  roleDataSource.get()
});
</script>

<template>
  <div class="grid">
    <div class="col-12 md:col-8">
      <div class="grid">
        <div class="hidden md:block col-12 md:col-7">
          <div
            style="height: calc(100vh - 225px); border-radius: 10px !important"
            class="card block indent-table overflow-x-hidden"
          >
            <DataTable
              scrollable
              scroll-height="400px"
              class="no-scroll-table"
              @rowClick="showReport($event)"
              :value="doctorDataSource.invoices.value"
            >
              <Column field="date" header="Дата">
                <template #body="{ data }">
                  {{ moment(data?.date).format("DD.MM.YYYY") }}
                </template>
              </Column>
              <Column  header="Пациент">
                <template #body="{ data }">
                  {{ data.patient }}
                </template>
              </Column>
              <Column field="sum" header="Сумма">
                <template #body="{ data }">
                  {{ data.sum }}
                </template>
              </Column>
              <Column header="Итого">
                <template #body="{ data }">
                  {{ data.percent }}
                </template>
              </Column>
            </DataTable>
          </div>
        </div>
        <div class="col-12 md:col-5">
          <div
            style="height: calc(100vh - 225px)"
            class="hidden md:block card indent-table"
          >
            <DataTable
              scrollable
              scroll-height="400px"
              :value="paymentDataSource.items.value"
            >
              <Column field="birthDate" header="Дата">
                <template #body="{ data }">
                  {{ moment(data?.createDate).format("DD.MM.YYYY") }}
                </template>
              </Column>
              <Column field="sum" header="Сумма" />
              <Column header="Способ">
                <template #body="{ data }">
                  <div class="flex align-items-center">
                    <img
                      class="mr-2"
                      style="width: 24px"
                      :src="getImg(data?.method).img"
                      alt=""
                    />
                    <p>{{ data?.method }}</p>
                  </div>
                </template>
              </Column>
            </DataTable>
          </div>
          <div
            class="flex md:hidden grid border-bottom-1 mb-3 items-center justify-content-between px-2 py-2 m-0"
          >
            <div class="col-4">
              <p class="mobile-card-description text-sm font-medium">Дата</p>
            </div>
            <div class="col-4 pl-0">
              <p class="mobile-card-description text-sm font-medium">Сумма</p>
            </div>
            <div class="col-4 pl-0">
              <p class="mobile-card-description text-sm font-medium">
                Способ
              </p>
            </div>
          </div>
          <div
            v-for="item in paymentDataSource.items.value"
            class="block md:hidden card flex-column p-3 mb-3 indent-table"
          >
            <div
              class="grid flex align-items-center justify-content-between w-full mb-3"
            >
              <div class="col-4">
                <h6 class="mobile-card-caption">
                  {{ moment(item?.createDate).format("DD.MM.YYYY") }}
                </h6>
              </div>
              <div class="col-4">
                <h6 class="mobile-card-caption">
                  {{ item.sum }}
                </h6>
              </div>
              <div class="col-4 pl-3">
                <div class="flex align-items-center">
                  <img
                    class="mr-2"
                    style="width: 24px"
                    :src="getImg(item?.method).img"
                    alt=""
                  />
                  <p class="mobile-card-caption">{{ item?.method }}</p>
                </div>
              </div>
            </div>
            <div class="flex items-center justify-content-between w-full">
              <p v-if="item.birthDate" class="mobile-card-description">
                {{ moment(item?.birthDate).format("DD.MM.YYYY") }}
              </p>
              <p class="mobile-card-description">{{ item.whatsAppNumber }}</p>
            </div>
          </div>
        </div>
        <div class="col-12 total pb-0">
          <div class="fixed md:static w-full bottom-0 left-0 card p-3">
            <div
              class="flex flex-wrap justify-content-between align-items-center w-full"
            >
              <p class="total-sum__caption mb-2">
                Общая сумма: <span class="total-sum">{{ total }}</span>
              </p>

              <div class="flex align-items-center">
                <p class="total-paid__caption mr-8">
                  Оплачено: <span class="total-paid">{{ totalPaid }}</span>
                </p>
                <p class="total-debt__caption">
                  Долг: <span class="total-debt">{{ total - totalPaid }}</span>
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="col-12 md:col-4">
      <div
        class="card w-full flex flex-column align-items-start pb-3 overflow-y-auto"
        style="height: calc(100vh - 164px); overflow: hidden"
      >
        <div class="tab-patient-id w-full tab-patient-id--profile">
          <div class="tab-panel-body">
            <div
              class="flex align-items-center w-full mb-1 justify-content-between"
            >
              <h4>Обшяя информация</h4>
              <Button class="p-0" @click="updateProfile" text>
                <svg
                  width="16"
                  height="16"
                  viewBox="0 0 16 16"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path
                    d="M7.375 2.9999H4.75C3.6999 2.9999 3.17485 2.9999 2.77377 3.20426C2.42096 3.38402 2.13413 3.67086 1.95436 4.02367C1.75 4.42475 1.75 4.9498 1.75 5.9999V11.2499C1.75 12.3 1.75 12.825 1.95436 13.2261C2.13413 13.5789 2.42096 13.8658 2.77377 14.0455C3.17485 14.2499 3.6999 14.2499 4.75 14.2499H10C11.0501 14.2499 11.5751 14.2499 11.9762 14.0455C12.329 13.8658 12.6159 13.5789 12.7956 13.2261C13 12.825 13 12.3 13 11.2499V8.6249M5.49998 10.4999H6.54657C6.85231 10.4999 7.00518 10.4999 7.14904 10.4654C7.27659 10.4347 7.39852 10.3842 7.51036 10.3157C7.6365 10.2384 7.7446 10.1303 7.96079 9.91411L13.9375 3.9374C14.4553 3.41963 14.4553 2.58016 13.9375 2.0624C13.4197 1.54463 12.5803 1.54463 12.0625 2.0624L6.08577 8.03911C5.86958 8.2553 5.76149 8.3634 5.68418 8.48954C5.61565 8.60138 5.56514 8.72331 5.53452 8.85086C5.49998 8.99472 5.49998 9.14759 5.49998 9.45333V10.4999Z"
                    stroke="#1479FF"
                    stroke-width="1.3"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                  />
                </svg>
              </Button>
            </div>
            <Divider class="mb-2 mt-2" />
            <table class="clean-table">
              <tr>
                <td>
                  <b>ФИО:</b>
                </td>
                <td class="pl-2">
                  {{ doctorDataSource.model.value?.fullName }}
                </td>
              </tr>
              <tr>
                <td>
                  <b>Дата рождения:</b>
                </td>
                <td class="pl-2">
                  {{
                    moment(doctorDataSource.model.value?.birthDate).format(
                      "DD.MM.YYYY"
                    )
                  }}
                </td>
              </tr>
              <tr>
                <td>
                  <b>Адрес:</b>
                </td>
                <td class="pl-2">
                  {{ doctorDataSource.model.value?.address }}
                </td>
              </tr>
              <tr>
                <td>
                  <b>Телефон:</b>
                </td>
                <td class="pl-2">
                  {{ doctorDataSource.model.value?.whatsAppNumber }}
                </td>
              </tr>
              <tr v-if=" doctorDataSource.model.value?.salaryType==1">
                <td>
                  <b>Процентная ставка:</b>
                </td>
                <td class="pl-2">
                  {{ doctorDataSource.model.value?.salary }}
                </td>
              </tr>
              <tr v-else>
                <td class="pb-0">
                  <b>Процентная ставка:</b>
                </td>
              </tr>
            </table>
            <Tree
                v-if="doctorDataSource.model.value?.salaryType==2"
                v-model:expandedKeys="expandedKeys"
                :value="serviceGroupDataSource.transformedRoot.value"
                v-slot="{ node }"
            >
              <div class="flex align-items-center">
                <div class="mr-1">{{ node.label }}</div>
                <div v-if="getSalaryValue(node.key)" class="flex align-items-center">
                  <strong>{{getSalaryValue(node.key).salary}}%</strong>
                </div>
              </div>
            </Tree>
          </div>
        </div>
      </div>
    </div>
  </div>
  <Dialog
    v-model:visible="visible"
    modal
    header="Редактировать карточку доктора"
    :style="{ width: '50vw' }"
    :breakpoints="{ '1199px': '60vw', '575px': '75vw' }"
  >
    <div class="grid pt-1">
      <div class="mb-4">
        <app-form :form-service="formService" />
        <div  v-if="doctorDataSource.model.value.salaryType == 2">
          <h5 class="mt-5">Процент от услуги</h5>
          <Tree
              v-model:expandedKeys="expandedKeys"
              :value="serviceGroupDataSource.transformedRoot.value"
              v-slot="{ node }"
          >
            <div class="flex align-items-center">
              <div class="mr-5">{{ node.label }}</div>
              <div v-if="getSalaryValue(node.key)" class="flex align-items-center">
                <InputText
                    @change="updateSalary(node.key, $event)"
                    :value="getSalaryValue(node.key).salary"
                    class="mr-3"
                />
                <Button
                    @click="deleteSalary(node.key)"
                    text
                    severity="secondary"
                    icon="pi pi-times"
                />
              </div>
              <Button v-else @click="addSalary(node.key)" size="small" >
                Добавить значение
              </Button>
            </div>
          </Tree>
        </div>

      </div>
    </div>
    <template #footer>
      <Button class="close" label="Отмена" @click="visible = false" text />
      <Button class="save" label="Сохранить" @click="saveProfile" autofocus />
    </template>
  </Dialog>
  <patient-payment-dialog
    ref="paymentDialog"
    :payment-data-source="paymentDataSource"
  />
  <booking-invoice-dialog ref="bookingViewer" />
</template>

<script lang="ts" setup>
import {computed, inject, onMounted, ref, Ref} from "vue";
import {useRoute} from "vue-router";
import {useMeta} from "vue-meta";
import {
  Filter,
  ListDataSourceConfig,
  ObjectDataSourceConfig,
  useBookingObjectDataSource,
  useListDataSource,
  useTreeDataSource,
} from "~/modules/data-sources";
import {useAppStore} from "~/modules/app/stores/useAppStore";
import {useClinicStore} from "~/modules/clinic/stores/useClinicStore";
import {defaultMessage, EntityMetaInfo, IMessage} from "~/shared";
import moment from "moment";
import {
  DateFieldAttribute,
  FieldAttribute,
  FormService,
  FormServiceConfig,
  InputFieldAttribute,
  MaskField,
  MaskFieldConfig,
  NumberField,
  SelectField,
  SelectFieldConfig,
  SelectMode,
  StringField,
  Watcher,
} from "~/modules/app-form";
import {required} from "~/shared/consts/consts";
import {useUserStore} from "~/modules/user/stores/useUserStore";
import {ButtonModel} from "~/modules/app/models/ButtonModel";
import PatientPaymentDialog from "~/modules/patient/components/patient-payment-dialog.vue";
import AppForm from "~/modules/app-form/components/app-form.vue";
import {useDoctorObjectDataSource} from "~/modules/doctor/data-sources/useDoctorObjectDataSource";
import {PhoneField} from "~/modules/app-form/components/editors/phone/models/PhoneField";
import _ from "lodash";
import {UserProfile} from "~/modules/user-profile/UserProfile";
import {getValue, remove} from "~/shared/lib/helpers";
import {CollectionField} from "~/modules/app-form/components/collections/models/CollectionField";
import {CollectionChipItem} from "~/modules/app-form/components/collections/items/models/CollectionChipItem";
import {SetRoleModel} from "~/modules/data-sources/hooks/setRoleModel";
import {useUserRoleDataSource} from "~/modules/user-role/data-sources/useUserRoleDataSources";
import DataTable from "primevue/datatable";
import BookingInvoiceDialog from "~/modules/booking/components/booking-invoice-dialog.vue";
import Tree from "primevue/tree";
import {TreeDataSourceConfig} from "~/modules/data-sources/hooks/useTreeDataSource";
import {isRouterActive} from "~/app/router";

const message = inject<IMessage>("message", defaultMessage);

const userStore = useUserStore();
const route = useRoute();
const appStore = useAppStore();
const clinicStore = useClinicStore();
const userRoleDataSource = useUserRoleDataSource();
const objectDataSource = useBookingObjectDataSource();
const expandedKeys = ref<any>({});
const formService = ref<any>();
const paymentDialog = ref();
const visible = ref(false);
const loaded: Ref<boolean> = ref(false);
const bookingViewer = ref();

const serviceGroupDataSource = useTreeDataSource(
  new TreeDataSourceConfig({
    className: "ServiceGroup",
  })
);

const doctorDataSource = useDoctorObjectDataSource(
  new ObjectDataSourceConfig({
    id: +route.params.id,
  })
);

const paymentDataSource = useListDataSource(
  new ListDataSourceConfig({
    className: "payment",
    filter: [
      new Filter("clinicId", clinicStore.clinic?.id),
      new Filter("profileId", +route.params.id),
    ],
  })
);

const roleDataSource = useListDataSource(
  new ListDataSourceConfig({
    className: "role",
    pageSize: 100,
    filter: [new Filter("names", "Dentist,Administrator")],
  })
);

const doctorServiceGroupSalaryDataSource = useListDataSource(
  new ListDataSourceConfig({
    className: "DoctorServiceGroupSalary",
    pageSize: 100,
    filter: [new Filter("doctorId", +route.params.id), new Filter("clinicId", clinicStore.clinic?.id)],
  })
)

const getSalaryValue =  (serviceGroupId:any) => {
  return doctorServiceGroupSalaryDataSource.items.value.find(x => x.serviceGroupId === serviceGroupId);
}

const updateSalary = async (serviceGroupId:any, {target}:any) => {
  const salary = getSalaryValue(serviceGroupId);
  salary.salary = +target.value;
  await doctorServiceGroupSalaryDataSource.update(salary);
}

const caption = computed(() => doctorDataSource.model.value?.fullName ?? "");
const hasPermissions = computed(() =>
  userStore.hasOptionalRoles([
    "System Administrator",
    "Administrator",
    "Director",
  ])
);
const total = computed(() =>
  doctorDataSource.invoices.value
    .map((i) => i.percent)
    .reduce((total, i) => total + i, 0)
);


const totalPaid = computed(() =>
  paymentDataSource.items.value.reduce((total, i) => total + i.sum, 0)
);

const getResult = (data: any) => {
  const sum = getSum(data);
  return (sum * data.salary) / 100;
};

const updateProfile = async () => {
  formService.value = new FormService(
    new FormServiceConfig({
      model: doctorDataSource.model.value,
      fields: [
        new StringField("fullName", {
          attrs: new InputFieldAttribute({
            caption: "ФИО",
            placeholder: "Введите ФИО Сотрудника",
          }),
          validations: [required()],
          grid: "col-6",
        }),
        new PhoneField("whatsAppNumber", {
          attrs: new InputFieldAttribute({
            caption: "WhatsApp номер",
            placeholder: "Введите WhatsApp номер",
          }),
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
        new StringField("jobPosition", {
          attrs: new InputFieldAttribute({
            caption: "Должность",
            placeholder: "Введите должность",
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
        new StringField("address", {
          attrs: new InputFieldAttribute({
            caption: "Адрес",
            placeholder: "Введите адрес",
          }),
          grid: "col-6",
        }),
        new SelectField("salaryType", {
          attrs: new InputFieldAttribute({
            caption: "Тип зарплаты",
          }),
          config: new SelectFieldConfig({
            getItems: "getSalaryTypes",
            labelKeyName: "caption",
            valueKeyName: "value",
            mode: SelectMode.Select,
          }),
          grid: "col-7",
        }),
        new NumberField("salary", {
          attrs: new InputFieldAttribute({
            caption: "Процентная ставка",
            placeholder: "Введите ставку",
          }),
          canShow: "canShowSalary",
          grid: "col-5",
          watchers: [
            new Watcher({
              changeAttrs: "changeSalaryAttrs",
              fields: ["salaryType"],
            }),
          ],
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
      actions: {
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
        getGenders: () => [
          { caption: "Женский", value: 0 },
          { caption: "Мужской", value: 1 },
        ],
        getSalaryTypes: () => [
          { caption: "Оклад", value: 0 },
          { caption: "Процент", value: 1 },
          { caption: "Процент от услуги", value: 2 },
        ],
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
        canShowSalary: (model: any) => {
          return model.salary == 0 || model.salary == 1;
        },
        changeSalaryAttrs: () => {
          if (doctorDataSource.model.value.salaryType == 0) {
            return new InputFieldAttribute({
              caption: "Оклад",
              placeholder: "Введите ставку",
            });
          } else if (doctorDataSource.model.value.salaryType == 1) {
            return new InputFieldAttribute({
              caption: "Процент",
              placeholder: "Введите ставку",
            });
          } else if (doctorDataSource.model.value.salaryType == 2) {
            return new InputFieldAttribute({
              caption: "Процент от услуги",
              placeholder: "Введите ставку",
            });
          }
        },
      },
    })
  );

  visible.value = true;
  if (!roleDataSource.loaded.value) {
    await roleDataSource.get();
  }
};

const showReport = async ({ data }: any) => {
  objectDataSource.config.value.id = data.bookingId;
  const booking = await objectDataSource.get();
  const result = await bookingViewer.value.open(booking);
  if (!result) return;
  data.salary = result.salary;
  message.success("Изменения успешно сохранены");
};

const addSalary = async (serviceGroupId:any) => {
  await doctorServiceGroupSalaryDataSource.add({
    clinicId:clinicStore.clinic?.id,
    doctorId: doctorDataSource.model.value.id,
    serviceGroupId: serviceGroupId,
    value: 0
  })
  await doctorServiceGroupSalaryDataSource.get();
}

const deleteSalary = async (serviceGroupId:any) => {
  const salary = getSalaryValue(serviceGroupId);
  await doctorServiceGroupSalaryDataSource.remove(salary.id);
}

const getImg = (caption: string) => {
  const index = paymentMethods.value.findIndex((i) => i.caption == caption);
  return paymentMethods.value[index];
};
const paymentMethods = ref([
  {
    img: "/img/money.svg",
    caption: "Наличным",
    active: false,
  },
  {
    img: "/img/MBank.svg",
    caption: "MBank",
    active: false,
  },
  {
    img: "/img/elCard.svg",
    caption: "Элкарт",
    active: false,
  },
  {
    img: "/img/o.svg",
    caption: "O! деньги",
    active: false,
  },
  {
    img: "/img/balans.svg",
    caption: "Balans.kg",
    active: false,
  },
  {
    img: "/img/other.svg",
    caption: "Другое",
    active: false,
  },
]);

const getSum = (data: any) => {
  return _.uniqBy(data.implementedServices, "id").reduce(
    (acc: any, c: any) => acc + c.price * c.count,
    0
  );
};

const saveProfile = async () => {
  await doctorDataSource.update(doctorDataSource.model.value);
  visible.value = false;
  message.success("Данные успешно сохранены");
};

useMeta({
  title: caption.value,
  htmlAttrs: { lang: "en", amp: true },
});

if (hasPermissions.value) {
  appStore.buttons = [
    new ButtonModel({
      attrs: {
        icon: "pi pi-plus ",
        class: "px-4 border-round-xl",
      },
      action: () => {
        paymentDialog.value.openDoctor(doctorDataSource.model.value.id);
      },
    }),
  ];
}

const expandNode = (node: any) => {
  if (node.children && node.children.length) {
    expandedKeys.value[node.key] = true;
    for (let child of node.children) {
      expandNode(child);
    }
  }
};

onMounted(async () => {
  isRouterActive("doctor-id");
  await Promise.all([
    doctorDataSource.get(),
    doctorServiceGroupSalaryDataSource.get(),
    doctorDataSource.getInvoices(clinicStore.clinic?.id as number),
    paymentDataSource.get(),
    serviceGroupDataSource.getRoot(clinicStore.clinic?.id as number),
  ]);
  for (let node of serviceGroupDataSource.transformedRoot.value) {
    expandNode(node);
  }
  appStore.pageTitle = caption.value;
  loaded.value = true;
});
</script>
<style lang="scss">
.new-patient .p-input-icon-left > i,
.p-input-icon-left > svg,
.p-input-icon-right > i,
.p-input-icon-right > svg {
  transform: translateY(-50%);
}

.new-patient .textarea.p-input-icon-left > i,
.p-input-icon-right > i {
  transform: translateY(-10%);
}

.clean-table td {
  padding-bottom: 0.25rem;
}
</style>

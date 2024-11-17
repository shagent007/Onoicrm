<template>
  <patient-card
    :patient-symptoms-data-source="patientSymptomDataSource"
    :payment-data-source="paymentDataSource"
    :patient-data-source="patientDataSource"
    :is-fixed-card="false"
    v-if="loaded"
  />
  <patient-payment-dialog
    ref="paymentDialog"
    :payment-data-source="paymentDataSource"
  />
</template>

<script lang="ts" setup>
import { computed, inject, onMounted, ref, Ref } from "vue";
import { useRoute } from "vue-router";
import { useMeta } from "vue-meta";
import {
  ObjectDataSourceConfig,
  useListDataSource,
  ListDataSourceConfig,
  UpdateFieldModel,
  Filter,
  useTreeDataSource,
} from "~/modules/data-sources";
import { useAppStore } from "~/modules/app/stores/useAppStore";
import { useClinicStore } from "~/modules/clinic/stores/useClinicStore";
import {
  BaseTableColumn,
  defaultMessage,
  EntityMetaInfo,
  IMessage,
} from "~/shared";
import { useToothStore } from "~/modules/tooth";
import moment from "moment";
import { usePatientObjectDataSource } from "~/modules/user-profile/hooks/usePatientObjectDataSource";
import {
  InputFieldAttribute,
  MemoField,
  SelectField,
  SelectFieldConfig,
  StringField,
} from "~/modules/app-form";
import { required } from "~/shared/consts/consts";
import { EditorDialogConfig } from "~/modules/editor/models/EditorDialogConfig";
import { TreatmentPlan } from "~/modules/treatment-plan/entities/TreatmentPlan";
import { useUserStore } from "~/modules/user/stores/useUserStore";
import PatientCard from "~/modules/patient/components/patient-card.vue";
import { useBookingListDataSource } from "~/modules/booking/data-sources/useBookingListDataSource";
import { ButtonModel } from "~/modules/app/models/ButtonModel";
import { Service } from "~/modules/service/entities/Service";
import PatientPaymentDialog from "~/modules/patient/components/patient-payment-dialog.vue";
import { useUserProfileGroupDataSource } from "~/modules/user-profile-group/useUserProfileGroupDataSource";
import { TreeDataSourceConfig } from "~/modules/data-sources/hooks/useTreeDataSource";

import { isRouterActive } from "~/app/router";
const message = inject<IMessage>("message", defaultMessage);
const loaded: Ref<boolean> = ref(false);
const userStore = useUserStore();
const route = useRoute();
const appStore = useAppStore();
const clinicStore = useClinicStore();
const paymentDialog = ref();

const patientDataSource = usePatientObjectDataSource(
  new ObjectDataSourceConfig({
    id: +route.params.id,
  })
);

const patientSymptomDataSource = useListDataSource(
  new ListDataSourceConfig({
    className: "patientSymptom",
    filter: [new Filter("patientId", +route.params.id)],
  })
);

const paymentDataSource = useListDataSource(
  new ListDataSourceConfig({
    className: "payment",
    filter: [
      new Filter("clinicId", clinicStore.clinic?.id),
      new Filter("patientId", +route.params.id),
    ],
  })
);

const hasPermissions = computed(() =>
  userStore.hasOptionalRoles([
    "System Administrator",
    "Administrator",
    "Director",
    "Administrator",
  ])
);

const caption = computed(() => patientDataSource.model.value?.fullName ?? "");

useMeta({
  title: caption.value,
});

// if(hasPermissions.value){
//   appStore.buttons = [
//     new ButtonModel({
//       attrs: {
//         icon: "pi pi-plus",
//         class: "ml-4 px-4 border-round-xl h-full",
//       },
//       action:  () => {
//         paymentDialog.value.open(+route.params.id)
//       },
//     }),
//   ]
// }

onMounted(async () => {
  isRouterActive("patient-id");
  await Promise.all([
    patientDataSource.get(),
    patientSymptomDataSource.get(),
    paymentDataSource.get(),
  ]);
  appStore.pageTitle = caption.value;
  appStore.breadCrumbs = [
    {
      label: "Пациенты",
      icon: "pi pi-user",
      to: "/patients",
    },
    {
      label: caption.value,
      icon: "pi pi-user",
      disabled: true,
    },
  ];
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

.clean-table {
  td:first-child {
    padding-bottom: 0.25rem;
    padding-right: 120px;
  }
}

@media screen and (max-width: 660px) {
  .clean-table {
    td:first-child {
      padding-right: 30px;
    }
  }
}
</style>

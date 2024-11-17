<template>
  <div v-if="loaded" class="booking-schedule">
    <schedule
      ref="schedule"
      :booking-cancellation-reason-data-source="bookingCancellationReasonDataSource"
      :reserve="reserveBooking"
      :service-group-data-source="serviceGroupDataSource"
      :data-source="dataSource"
      :add="addBooking"
      :canceled-booking-data-source="canceledBookingDataSource"
    >
      <template #items-mobile>
        <div
          @click="clinicStore.switchBookingType()"
          class="tag tag--active mr-3"
        >
          <img
            v-if="clinicStore.clinic?.bookingType == 1"
            src="/img/armchair-white.svg"
            alt=""
          />
          <img v-else src="/img/user-white.svg" alt="" />
        </div>
        <div
          v-if="clinicStore.clinic?.bookingType == 0"
          class="calendar-items__doctors"
        >
          <p
            @click.prevent="
              updateFilter(item.id);
              selectedDoctor = item;
            "
            v-for="item in doctorDataSource.activeItems.value as any[]"
            :key="item.id"
            class="tag"
            :class="{ 'tag--active': selectedDoctor.id === item.id }"
          >
            {{ item.fullName }}
          </p>
        </div>
        <div v-else class="calendar-items__doctors">
          <p
            @click.prevent="
              updateFilter(item.id);
              selectedArmchair = item;
            "
            v-for="item in armchairStore.items as any[]"
            :key="item.id"
            class="tag"
            :class="{ 'tag--active': selectedArmchair.id === item.id }"
          >
            {{ item.caption }}
          </p>
        </div>
      </template>
      <template #tabs>
        <div class="flex w-full align-items-center justify-content-between">
          <TabView
            v-if="clinicStore.clinic?.bookingType == 0"
            class="tab--filter-view"
            :scrollable="true"
          >
            <TabPanel
              v-for="item in doctorDataSource.activeItems.value as any[]"
              :key="item.id"
            >
              <template #header>
                <Button
                  class="text-white white-space-nowrap"
                  size="small"
                  rounded
                  :severity="getDoctorSeverity(item)"
                  :label="item.fullName"
                  @click.prevent="updateFilter(item.id);selectedDoctor = item;"
                />
              </template>
            </TabPanel>
          </TabView>
          <TabView v-else class="tab--filter-view" :scrollable="true">
            <TabPanel v-for="item in armchairStore.items as any[]" :key="item.id">
              <template #header>
                <Button
                  class="text-white white-space-nowrap"
                  size="small"
                  rounded
                  :severity="getArmchairSeverity(item)"
                  :label="item.caption"
                  @click.prevent="
                    updateFilter(item.id);
                    selectedArmchair = item;
                  "
                />
              </template>
            </TabPanel>
          </TabView>
          <Button @click="clinicStore.switchBookingType()" class="flex-grow-1 ml-2 flex align-items-center justify-content-center tab-change">
           <span v-if="clinicStore.clinic?.bookingType == 1">Креслa</span>
           <span v-else>Врачи</span>
          </Button>
        </div>

      </template>
    </schedule>
  </div>
  <booking-dialog
    v-if="clinicStore.clinic"
    :clinic="clinicStore.clinic"
    :doctor-data-source="doctorDataSource"
    :service-group-data-source="serviceGroupDataSource"
    ref="bookingDialog"
  />
</template>

<script setup lang="ts">
import {
  Filter,
  ListDataSourceConfig,
  useListDataSource,
  useTreeDataSource,
} from "~/modules/data-sources";
import { computed, nextTick, ref } from "vue";
import { useClinicStore } from "~/modules/clinic/stores/useClinicStore";
import { getMonthFirstMonday } from "~/modules/booking/services/getMonthFirstMonday";
import { getMonthLastSunday } from "~/modules/booking/services/getMonthLastSunday";
import { onMounted } from "vue";
import { useAppStore } from "~/modules/app/stores/useAppStore";
import { useMeta } from "vue-meta";
import { useUserStore } from "~/modules/user/stores/useUserStore";
import BookingDialog from "~/modules/booking/components/booking-dialog.vue";
import { Booking } from "~/modules/booking/entities/Booking";
import { useBookingListDataSource } from "~/modules/booking/data-sources/useBookingListDataSource";
import { TreeDataSourceConfig } from "~/modules/data-sources/hooks/useTreeDataSource";
import Schedule from "~/modules/booking/components/schedule.vue";
import { useArmchairStore } from "~/modules/armchair";

useMeta({
  title: "Главная страница",
  htmlAttrs: { lang: "en", amp: true },
});

const appStore = useAppStore();
const userStore = useUserStore();
const clinicStore = useClinicStore();
const schedule = ref();
const bookingDialog = ref();
const armchairStore = useArmchairStore();
const loaded = ref<boolean>(false);
const selectedDoctor = ref<any>();
const selectedArmchair = ref<any>();
const firstMonday = getMonthFirstMonday().format("DD.MM.YYYY");
const lastSunday = getMonthLastSunday().format("DD.MM.YYYY");

const serviceGroupDataSource = useTreeDataSource(
  new TreeDataSourceConfig({
    className: "ServiceGroup",
  })
);

const bookingCancellationReasonDataSource = useListDataSource(
  new ListDataSourceConfig({
    className: "BookingCancellationReason",
    pageSize: 1000,
    filter: [new Filter("clinicId", clinicStore.clinic?.id)],
  })
);

const doctorDataSource = useListDataSource(
  new ListDataSourceConfig({
    className: "userProfile",
    fields:"new(FullName, Id,StateId)",
    filter: [
      new Filter("clinicId", clinicStore.clinic?.id),
    ],
  })
);

const canceledBookingDataSource = useBookingListDataSource(
  new ListDataSourceConfig({
    className: "Booking",
    filter: [
      new Filter("clinicId", clinicStore?.clinic?.id),
      new Filter("canceled", true),
    ],
  })
);

const dataSource = useBookingListDataSource(
  new ListDataSourceConfig({
    filter: [
      new Filter("clinicId", clinicStore.clinic?.id),
      new Filter("dateTimeRange", `${firstMonday},${lastSunday}`),
      new Filter("notCancelled", true),
    ],
    pageSize: 100,
  })
);

const getDoctorSeverity = (item: any): "primary" | "secondary" => {
  if (!selectedDoctor.value) return "secondary";
  return item.id == selectedDoctor.value.id ? "primary" : "secondary";
};

const getArmchairSeverity = (item: any): "primary" | "secondary" => {
  if (!selectedDoctor.value) return "secondary";
  return item.id == selectedArmchair.value.id ? "primary" : "secondary";
};

const hasPermission = computed(() =>
  userStore.hasOptionalRoles([
    "Director",
    "Administrator",
    "System Administrator",
  ])
);

const updateFilter = async (itemId: any) => {
  if (clinicStore.clinic?.bookingType == 0) {
    dataSource.removeFilter(["armchairId"], false);
    await dataSource.updateFilter([new Filter("doctorId", itemId)],false);
  } else {
    dataSource.removeFilter(["doctorId"], false);
    await dataSource.updateFilter([new Filter("armchairId", itemId)],false);
  }

  await dataSource.getSchedule();
  await nextTick();
  if (schedule.value) {
    schedule.value.refresh();
  }
};

const addBooking = async (eventObject: any) => {
  await nextTick();
  const result: Booking = await bookingDialog.value.open();
  if (!result) return;
  const doctor = hasPermission.value ? selectedDoctor.value : userStore.profile;

  result.dateTimeStart = eventObject.start;
  result.dateTimeEnd = eventObject.end;
  result.doctorId = doctor.id;
  result.salary = doctor.salary;
  result.salaryType = doctor.salaryType;

  return await dataSource.add(result);
};

const reserveBooking = async (eventObject: any) => {
  await nextTick();
  eventObject.doctorId = hasPermission.value
    ? selectedDoctor.value.id
    : userStore.profile.id;
  eventObject.caption = "Не работает";
  return await dataSource.add(eventObject);
};

appStore.buttons = [];
onMounted(async () => {
  await  doctorDataSource.get();

  selectedArmchair.value = armchairStore.items[0];

  selectedDoctor.value = hasPermission.value
    ? doctorDataSource.items.value[0]
    : userStore.profile;

  await updateFilter(selectedDoctor.value?.id);
  loaded.value = true;
});
</script>

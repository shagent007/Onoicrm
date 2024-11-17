<template>
  <Dialog
      :visible="visible"
      @update:visible="(value) => (!value ? reset() : {})"
      modal
      :maximizable="maximizable"
      class="booking-report-dialog"
      :class="{'p-dialog-maximized':!maximizable}"
      :style="{ width: '95vw',height: '95%', background: '#F5F6F8' }"
  >
    <template #header>
      <div class="justify-content-between align-items-center flex-1 hidden md:flex">
        <div class="text-xl font-semibold">{{ booking?.patient.fullName }}</div>
        <div class=" align-items-center tab-buttons mr-4">
          <Button
              label="Отчеты"
              @click="showTab = false; initTreeSearchField();"
              :class="{ active: !showTab }"
              class="mr-2"
          />
          <Button
              :class="{ active: showTab }"
              label="Карточка пациента"
              @click=" initTreeSearchField(); initializePatientData(booking?.patient.id!)"
          />
        </div>
      </div>
      <div class="flex md:hidden flex-grow-1 w-full justify-content-between mobile-header mobile-header--bg-gray">
        <Button @click="reset()" icon="pi pi-times" severity="secondary" text rounded aria-label="Cancel"/>
        <div class="mobile-header-nav" :class="tabMenu ? 'mobile-header-nav--top-border' : 'mobile-header-nav--border'">
          <div class="flex align-items-center justify-content-center">
            <h1 class="header__title px-2 py-1 white-space-nowrap" @click="tabMenu = true">{{ tabMenuActive }}</h1>
            <i v-if="!tabMenu" class="pi pi-angle-down h-fit"></i>
          </div>
          <div class="mobile-header-nav__content pt-0 px-3" :class="tabMenu ? 'mobile-header-nav__content-border' : ''">
            <div v-if="tabMenu" v-for="item in tabItems">
              <p v-if="tabMenuActive != item.label"
                 style="padding-bottom: 0.7rem;"
                 class="header__title px-4 white-space-nowrap"
                 @click="item.command(item.label)">{{item.label}}</p>
            </div>
          </div>
        </div>
        <Button @click="submit()" icon="pi pi-check" aria-label="Filter"/>
      </div>
    </template>
    <template v-if="!showTab">
      <div class="hidden  md:flex grid">
        <div class="md:col-5 lg:col-4">
          <div class="card block h-full p-3">
            <div>
              <div class="tree-filter compact-input mb-2"></div>
              <h4 style="color: #000; font-weight: 500" class="mb-1">
                Категории
              </h4>
              <Divider style="background-color: #000" class="mt-0"/>
              <div class="compact-tree no-filter scrollable--300">
                <Tree
                  :filter="true"
                  selectionMode="single"
                  :value="serviceGroupDatasource.transformedRoot.value"
                  v-model:selectionKeys="selectedKey"
                  @node-select="updateFilter($event)"
                  @node-unselect="clearFilter()"
                  v-model:expandedKeys="expandedKeys"
                />
              </div>
            </div>
          </div>
        </div>
        <div class="md:col-7 lg:col-8">
          <div class="card block h-full p-3">
            <DataTable
              v-model:filters="filters"
              scrollable
              scrollHeight="250px"
              :globalFilterFields="['code', 'caption', 'price']"
              class="p-datatable-sm p-datatable-hoverable-rows"
              :value="serviceDataSource.items.value"
            >
              <template #header>
                <div class="compact-input">
                  <InputText
                    v-model="filters['global'].value"
                    class="w-full"
                    placeholder="Поиск"
                  />
                </div>
              </template>
              <Column style="width: 60px" field="code" header="Код"/>
              <Column field="caption" header="Наименование"/>
              <Column field="price" header="Цена"/>
              <Column field="actions">
                <template #body="{ data }">
                  <Checkbox
                    :binary="true"
                    :model-value="isSelected(data)"
                    @input="addOrDeleteImplementedService(data)"
                  />
                </template>
              </Column>
            </DataTable>
          </div>
        </div>
        <div class="col-7">
          <div style="height: 350px" class="card block p-3">
            <div v-if="clinicStore.clinic?.type !== CompanyType.Company"
                 class="flex align-items-center justify-content-between mb-1">
              <h4 class="mb-0">Выполнено</h4>
              <div class="tooth-buttons">
                <Button
                  class="mr-2"
                  :outlined="selectedBookingTooth !== null"
                  @click="selectedBookingTooth = null"
                  label="Все"
                />
                <template v-for="bt in bookingTeeth" :key="bt.id">
                  <Button
                    class="mr-2"
                    @click="selectedBookingTooth = bt"
                    v-if="toothStore.findItem(bt?.toothId)?.position"
                    :outlined="selectedBookingTooth?.toothId != bt.toothId"
                    :label="toothStore.findItem(bt.toothId)?.position?.toString()"
                  />
                </template>

                <Button
                  class="mr-2"
                  @click="addTooth"
                  outlined
                  icon="pi pi-plus"
                />
                <Button
                  v-if="selectedBookingTooth !== null"
                  v-tooltip="'Удалить отчёты о выбронном зубе'"
                  outlined
                  severity="danger"
                  icon="pi pi-trash"
                  @click="deleteBookingTooth()"
                />
              </div>

              <p>{{ moment().format("DD.MM.YYYY") }}</p>
            </div>

            <DataTable
              scrollable
              scrollHeight="250px"
              class="p-datatable-sm p-datatable-hoverable-rows"
              :value="implementedServices"
            >
              <Column style="width: 60px" field="code" header="Код">
                <template #body="{ data }">{{ data?.code }}</template>
              </Column>
              <Column header="Наименование">
                <template #body="{ data }">
                  <div>{{ data?.caption }}</div>
                </template>
              </Column>
              <Column style="width: 80px" header="Цена">
                <template #body="{ data }">
                  <div>{{ data?.price }}</div>
                </template>
              </Column>
              <Column style="width: 120px" header="Количество">
                <template #body="{ data }: any">
                  <div class="flex align-items-center">
                    <Button
                      @click="decrementCount(data)"
                      size="small"
                      icon="pi pi-minus"
                      outlined
                      severity="secondary"
                      class="w-auto"
                      style="padding: 3px 4px"
                    />
                    <div class="mx-3">{{ data?.count }}</div>
                    <Button
                      @click="data!.count++"
                      size="small"
                      icon="pi pi-plus"
                      class="w-auto"
                      style="padding: 3px 4px"
                    />
                  </div>
                </template>
              </Column>

              <Column  style="width: 80px"  header="Cумма">
                <template #body="{ data }">
                  <div>{{ data?.price * data?.count }}</div>
                </template>
              </Column>

              <Column  style="width: 60px"  v-if="useLabaratory" header="Лаб.">
                <template #body="{ data }">
                  <div>{{ data?.labaratoryPrice * data.count }}</div>
                </template>
              </Column>

              <Column  style="width: 80px"  header="Ставка">
                <template #body="{ data }">
                  <b>{{ data?.salary }}%</b>
                </template>
              </Column>

              <Column  style="width: 60px"  header="Итого">
                <template #body="{ data }">
                  <div>{{ calculateTotal(data) }}</div>
                </template>
              </Column>
              <Column  style="width: 30px">
                <template #body="{ data }">
                  <Button
                    @click="removeImplementedService(data)"
                    size="small"
                    rounded
                    icon="pi pi-trash"
                    text
                  />
                </template>
              </Column>
            </DataTable>
          </div>
        </div>
        <div class="col-5">
          <div class="grid h-full">
            <div v-if="bookingTeeth.length > 0" class="col-5 pb-0 h-full">
              <div style="height: 350px" class="card block p-3">
                <h4 class="mb-0">Каналы</h4>
                <divider style="margin: 12px 0"/>
                <div
                  v-if="selectedBookingTooth"
                  style="height: 270px"
                  class="scrollable--200"
                >
                  <div
                    v-for="channel in selectedBookingTooth.channels"
                    :key="channel.channelId"
                    class="channel mb-4"
                  >
                    <div class="channel__caption">{{ getChannelCaption(channel) }}</div>
                    <div class="channel__card">
                      <div class="channel-item">
                        <div class="channel-item__caption">МК:</div>
                        <InputNumber v-model="channel.masterCon" suffix="мм"/>
                      </div>
                      <div class="channel-item">
                        <div class="channel-item__caption">МФ:</div>
                        <InputNumber v-model="channel.masterFile" suffix="мм"/>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="text-center h-full pt-4" v-else>
                  Чтобы увидеть каналы выберите зуб
                </div>
              </div>
            </div>
            <div :class="bookingTeeth.length > 0 ? 'col-7' : 'col-12'" class=" pb-0 h-full">
              <div style="height: 350px; overflow: auto" class="card block p-3">
                <div class="flex align-items-center justify-content-between">
                  <h4 class="mb-0">Фотографии</h4>
                  <file-selector
                    v-if="files.length > 0"
                    @select="addFile($event)"
                    class="text-center"
                  >
                    <span
                      style="border-radius: 6px !important"
                      class="p-button p-1 w-auto p-component p-button-icon-only p-button-outlined"
                    >
                      <span
                        style="font-size: 14px"
                        class="p-button-icon pi pi-plus"
                      />
                    </span>
                  </file-selector>
                </div>
                <divider style="margin-top: 0.75rem"/>
                <file-selector
                  v-if="files.length == 0"
                  @select="addFile($event)"
                  class="text-center"
                >
                  Выберите файлы
                </file-selector>
                <div
                  style="height: 250px"
                  :class="{ 'scrollable--200': files.length > 0 }"
                >
                  <div class="grid">
                    <file-list css-class="col-6" v-slot="{ item }" :items="files">
                      <img
                        class="w-full border-round-sm cursor-pointer"
                        style="
                      object-fit: cover;
                      object-position: center;
                      aspect-ratio: 16/9;
                    "
                        @click="deleteFile(item)"
                        :src="getImageSrc(item)"
                        :alt="item?.alt"
                      />
                    </file-list>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="block  md:hidden h-full">
        <div class="overflow-x-hidden relative h-full pr-1">
          <div
            v-if="clinicStore.clinic?.type !== CompanyType.Company"
            class="flex align-items-center justify-content-between mb-3"
          >
            <div class="tooth-buttons">
              <Button
                class="mr-2"
                :outlined="selectedBookingTooth !== null"
                @click="selectedBookingTooth = null"
                label="Все"
              />
              <template v-for="bt in bookingTeeth" :key="bt.id">
                <Button
                  class="mr-2"
                  @click="selectedBookingTooth = bt"
                  v-if="toothStore.findItem(bt?.toothId)?.position"
                  :outlined="selectedBookingTooth?.toothId != bt.toothId"
                  :label="toothStore.findItem(bt.toothId)?.position?.toString()"
                />
              </template>
              <Button
                class="mr-2"
                @click="addTooth"
                outlined
                icon="pi pi-plus"
              />
              <Button
                v-if="selectedBookingTooth !== null"
                v-tooltip="'Удалить отчёты о выбронном зубе'"
                outlined
                severity="danger"
                icon="pi pi-trash"
                @click="deleteBookingTooth()"
              />
            </div>
          </div>
          <div class="no-scroll-bar overflow-auto flex ">
            <div class="no-scroll-bar p-1 flex gap-2 align-items-center mb-3 dialog-tabs">
              <Button
                class="white-space-nowrap"
                label="Услуги"
                v-bind="getTabState('services')"
                @click="activeTabItem='services'"
              />
              <Button
                class="white-space-nowrap"
                label="Выполнено"
                v-bind="getTabState('is')"
                @click="activeTabItem='is'"
                :badge="`${implementedServices.length}`"
                badgeClass="tab-badge"
              />
              <Button
                v-bind="getTabState('channels')"
                label="Каналы"
                v-if="bookingTeeth.length > 0"
                @click="activeTabItem='channels'"
              />
              <Button
                v-bind="getTabState('images')"
                label="Фотографии"
                @click="activeTabItem='images'"
                :badge="`${files.length}`"
              />
            </div>
          </div>
          <div v-if="activeTabItem=='services'">
            <div class="text-gray-500 column-gap-2 border-bottom-2 pb-2 mb-3 flex justify-content-between align-items-center">
              <div class="compact-tree w-full">
                <TreeSelect @node-select="updateFilter($event)"
                  @node-unselect="clearFilter()"
                  class="w-full"
                  v-model="selectedKey"
                  placeholder="Выбирайте услуг"
                  :options="serviceGroupDatasource.transformedRoot.value"
                />
              </div>
              <p class="font-semibold">{{ serviceDataSource.items.value.length }}</p>
            </div>
            <div v-for="service in serviceDataSource.items.value" class="card mb-2 p-2 flex flex-column ">
              <h3 class="w-full mb-3 text-base">{{ service?.caption }}</h3>
              <div class="flex justify-content-between align-items-center w-full ">
                <div class="flex align-items-center">
                  <p class="mr-2">Код:</p>
                  <h4 class=" font-semibold text-sm">{{ service?.code }}</h4>
                </div>
                <div class="flex align-items-center">
                  <p class="mr-2">Цена:</p>
                  <h4 class="font-semibold text-sm">{{ service?.price }}</h4>
                </div>
                <div class="flex align-items-center">
                  <Checkbox
                    :binary="true"
                    :model-value="isSelected(service)"
                    @input="addOrDeleteImplementedService(service)"
                  />
                </div>
              </div>
            </div>
          </div>

          <div v-if="activeTabItem=='is'">
            <div class="text-gray-500  border-bottom-2 pb-2 mb-3 flex justify-content-between">
              <h3 class="font-semibold">Проделанные работы</h3>
              <p class="font-semibold">{{ implementedServices.length }}</p>
            </div>

            <div v-for="implementedService in implementedServices" :key="implementedService?.id" class="card mb-2 p-2 flex flex-column ">
              <h3 class="w-full mb-3 text-base">{{ implementedService?.caption }}</h3>
              <div class="flex justify-content-between align-items-center w-full mb-2">
                <div class="flex align-items-center">
                  <p class="mr-2">Код:</p>
                  <h4 class=" font-semibold text-sm">{{ implementedService?.code }}</h4>
                </div>
                <div class="flex align-items-center">
                  <p class="mr-2">Цена:</p>
                  <h4 class="font-semibold text-sm">{{ implementedService?.price }}</h4>
                </div>
              </div>
              <div class="flex justify-content-between align-items-center w-full mb-2">
                <div class="flex align-items-center">
                  <p class="mr-2">Количество:</p>
                  <div class="flex align-items-center">
                    <Button
                      @click="decrementCount(implementedService)"
                      size="small"
                      icon="pi pi-minus"
                      outlined
                      severity="secondary"
                      class="w-auto"
                      style="padding: 3px 4px"
                    />
                    <h3 class="mx-2 font-semibold text-sm">{{ implementedService?.count }}</h3>
                    <Button
                      @click="implementedService!.count++"
                      size="small"
                      icon="pi pi-plus"
                      class="w-auto"
                      style="padding: 3px 4px"
                    />
                  </div>
                </div>
                <div class="flex align-items-center">
                  <p class="mr-2">Итог:</p>
                  <h4 class="font-semibold text-sm">{{ implementedService?.price * implementedService?.count }}</h4>
                </div>
              </div>
            </div>
          </div>
          <div v-if="activeTabItem=='channels' && bookingTeeth.length > 0">
            <div class="text-gray-500  border-bottom-2 pb-2 mb-3 flex justify-content-between">
              <h3 class="font-semibold">Каналы</h3>
            </div>
            <div v-if="selectedBookingTooth">
              <div class="grid">
                <div class="col-12" v-for="channel in selectedBookingTooth.channels" :key="channel.channelId">
                  <div class="channel mb-2">
                    <div class="channel__caption">
                      {{ getChannelCaption(channel) }}
                    </div>
                    <div class="channel__card w-full">
                      <div class="channel-item">
                        <div class="channel-item__caption">МК:</div>
                        <InputNumber v-model="channel.masterCon" suffix="мм" class="w-full"/>
                      </div>
                      <div class="channel-item">
                        <div class="channel-item__caption">МФ:</div>
                        <InputNumber v-model="channel.masterFile" suffix="мм" class="w-full"/>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div v-if="activeTabItem=='images'">
            <div class="text-gray-500  border-bottom-2 pb-2 mb-3 flex align-items-center justify-content-between">
              <h4 class="mb-0">Фотографии</h4>
              <file-selector v-if="files.length > 0" @select="addFile($event)" class="text-center"
              >
                  <span
                    style="border-radius: 6px !important"
                    class="p-button p-1 w-auto p-component p-button-icon-only p-button-outlined"
                  >
                    <span
                        style="font-size: 14px"
                        class="p-button-icon pi pi-plus"
                    />
                  </span>
              </file-selector>
            </div>
            <file-selector
              v-if="files.length == 0"
              @select="addFile($event)"
              class="text-center"
            >
              Выберите файлы
            </file-selector>
            <div class="h-full">

              <div class="grid ">
                <file-list css-class="col-12" v-slot="{ item }" :items="files">
                  <img
                    class="w-full border-round-sm cursor-pointer"
                    style="
                      object-fit: cover;
                      object-position: center;
                      aspect-ratio: 16/9;
                    "
                    @click="deleteFile(item)"
                    :src="getImageSrc(item)"
                    :alt="item?.alt"
                  />
                </file-list>
              </div>
            </div>
          </div>
        </div>
      </div>
    </template>

    <div v-else>
      <PatientCard
        :patient-data-source="patientObjectDataSource"
        :booking-data-source="bookingListDataSource"
        :patient-symptoms-data-source="patientSymptomDataSource"
        :payment-data-source="paymentDataSource"
        :is-fixed-card="false"
      />
    </div>

    <template #footer>
      <div class="hidden md:flex w-full grid">
        <div class="col-8 py-0 flex align-items-center justify-content-between">
          <template v-if="!showTab">
            <div class="flex justify-content-end">
              <span class="mr-1 align-self-center">Скидка:</span>
              <div class="compact-input">
                <InputNumber
                  class="mr-2"
                  :input-style="{ width: '20px' }"
                  v-model="booking!.discount"
                />
              </div>

              <Button
                size="small"
                @click="booking.discountType = DiscountType.Percent"
                :outlined="booking.discountType !== DiscountType.Percent"
                icon="pi pi-percentage"
                style="border-radius: 3px"
                class="px-2 py-0"
              />
              <Button
                size="small"
                @click="booking.discountType = DiscountType.Money"
                :outlined="booking.discountType !== DiscountType.Money"
                icon="pi pi-dollar"
                style="border-radius: 3px"
                class="px-2 py-0"
              />

              <InputSwitch
                v-tooltip="'Учитывать стоиммость лабаторной работы'"
                v-model="useLabaratory"
              />

            </div>
            <div class="flex align-items-center justify-content-end">
              <div class="flex mr-5">
                <div class="mr-1">Сумма:</div>
                <div>{{ serviceSum }} c</div>
              </div>
              <div class="flex mr-5">
                <div class="mr-1">Скидка:</div>
                <div>{{ discountSum }} c</div>
              </div>
              <div class="flex">
                <div class="mr-1">Итого:</div>
                <div style="font-weight: 500">{{ resultSum }} c</div>
              </div>
            </div>
          </template>
        </div>
        <div class="col-4 py-0 pr-0 flex justify-content-end">
          <Button @click="reset()" class="p-button-secondary" label="Отмена"/>
          <Button @click="submit()" class="mr-0" label="Сохранить"/>
        </div>
      </div>
      <div class="block md:hidden">
        <div class="w-full mb-3">
          <template v-if="!showTab">
            <div class="flex align-items-center justify-content-between">

              <div class="flex">
                <span class="mr-1 align-self-center">Скидка:</span>
                <div class="compact-input">
                  <InputNumber
                    class="mr-2"
                    :input-style="{ width: '100px' }"
                    v-model="booking!.discount"
                  />
                </div>

                <Button
                  size="small"
                  @click="booking.discountType = DiscountType.Percent"
                  :outlined="booking.discountType !== DiscountType.Percent"
                  icon="pi pi-percentage"
                  style="border-radius: 3px"
                  class="px-2 py-0"
                />
                <Button
                  size="small"
                  @click="booking.discountType = DiscountType.Money"
                  :outlined="booking.discountType !== DiscountType.Money"
                  icon="pi pi-dollar"
                  style="border-radius: 3px"
                  class="px-2 py-0"
                />
              </div>
              <div class="flex">
                <div class="mr-1">Итого:</div>
                <div style="font-weight: 500" class="white-space-nowrap">{{ resultSum }} c</div>
              </div>
            </div>
          </template>
        </div>
      </div>
    </template>
  </Dialog>

  <tooth-selector :states="selectedTeeth" ref="toothSelector"/>
</template>

<script setup lang="ts">
import {computed, inject, nextTick, onBeforeUnmount, onMounted, onUnmounted, ref, Ref} from "vue";
import {Filter, ITreeDataSource, ListDataSourceConfig, useListDataSource,} from "~/modules/data-sources";
import {useClinicStore} from "~/modules/clinic/stores/useClinicStore";
import {useToothStore} from "~/modules/tooth";
import moment from "moment";
import {Booking} from "~/modules/booking";
import {DiscountType} from "~/modules/booking/entities/Booking";
import Tree from "primevue/tree";
import ToothSelector from "~/modules/tooth/components/tooth-selector.vue";
import FileSelector from "~/modules/files/components/file-selector.vue";
import FileList from "~/modules/files/components/file-list.vue";
import {defaultConfirm, defaultMessage, IMessage, TConfirm} from "~/shared";
import {FilterMatchMode} from "primevue/api";
import _, {isNumber} from "lodash";
import {getGUID} from "~/shared/lib/getGUID";
import PatientCard from "~/modules/patient/components/patient-card.vue";
import {useBookingListDataSource} from "~/modules/booking/data-sources/useBookingListDataSource";
import {usePatientObjectDataSource} from "~/modules/user-profile/hooks/usePatientObjectDataSource";
import {CompanyType} from "~/modules/clinic/Clinic";
import TreeSelect from "primevue/treeselect"
import {collectTreeBranchParents, findElementInTree, getLastLevelBranches} from "~/shared/lib/helpers";
import {useRoute} from "vue-router";

const {serviceGroupDatasource} = defineProps<{ serviceGroupDatasource: ITreeDataSource; }>();

const route = useRoute();
const toothStore = useToothStore();
const clinicStore = useClinicStore();
const message = inject<IMessage>("message", defaultMessage);
const confirm = inject<TConfirm>("confirm", defaultConfirm);
const toothSelector = ref();
const booking = ref<Booking | any>();
const selectedBookingTooth = ref<any>(null);
const forAddBookingTeeth = ref<any[]>([]);
const forDeleteBookingTeeth = ref<any[]>([]);
const forDeleteImplementedServices = ref<any[]>([]);
const forAddImplementedServices = ref<any[]>([]);
const forDeleteFiles = ref<any[]>([]);
const forAddBookingFiles = ref<any[]>([]);
const forDeleteBookingFiles = ref<any[]>([]);
const activeTabItem = ref<"services" | "is" | "channels" | "images">("is");
const patientDataInitialized = ref(false);
const serviceInitialized = ref(false)
const showTab = ref<boolean>(false);
const expandedKeys = ref<any>({});
const selectedKey: Ref = ref();
const visible = ref<boolean>(false);
const resolve = ref<(model: any) => any>();
const tabMenu = ref(false)
const tabMenuActive = ref('Отчеты')
const maximizable = ref(false);
const tabItems = ref([
  {
    label: 'Отчеты',
    command: (text: any)=>{
      tabMenuActive.value = text
      tabMenu.value = false
      console.log(tabMenuActive.value, tabMenu.value)
      console.log('cdc')
      showTab.value = false;
      initTreeSearchField()
    }
  },
  {
    label: `Карточка пациента`,
    command: (text: any)=>{
      tabMenuActive.value = text
      tabMenu.value = false
      console.log(tabMenuActive.value, tabMenu.value)
      showTab.value = true;
      initTreeSearchField();
    }
  }
])
const useLabaratory = ref(false)

const filters = ref({
  global: {value: null, matchMode: FilterMatchMode.CONTAINS},
});

const patientObjectDataSource = usePatientObjectDataSource();

const doctorServiceGroupSalaryDataSource = useListDataSource(
  new ListDataSourceConfig({
    className: "DoctorServiceGroupSalary",
    pageSize: 100,
    filter: [
      new Filter("clinicId", clinicStore.clinic?.id)
    ],
  })
)

const serviceDataSource = useListDataSource(
    new ListDataSourceConfig({
      className: "service",
    }),
);

const paymentDataSource = useListDataSource(
    new ListDataSourceConfig({
      className: "payment",
    }),
);

const bookingListDataSource = useBookingListDataSource(
    new ListDataSourceConfig({
      filter: [
        new Filter("clinicId", +clinicStore?.clinic?.id!),
        new Filter("stateId", 6),
        new Filter("notCancelled", true),
      ],
    }),
);

const patientSymptomDataSource = useListDataSource(
    new ListDataSourceConfig({
      className: "patientSymptom",
    }),
);

const files = computed(() => {
  if (selectedBookingTooth.value !== null) {
    return (
        selectedBookingTooth?.value?.files.filter(
            (f: any) => !forDeleteFiles.value.some((fdf: any) => f.id == fdf.id),
        ) ?? []
    );
  }

  const bookingTeethFiles = bookingTeeth.value
      .map((bt: any) => bt.files)
      .flat()
      .concat(booking.value.files ?? [])
      .concat(forAddBookingFiles.value ?? [])
      .filter((f: any) => !forDeleteFiles.value.some((fdf: any) => f.id == fdf.id))
      .filter((f: any) => !forDeleteBookingFiles.value.some((fdbf: any) => f.id == fdbf.id));

  return bookingTeethFiles ?? []
});

const selectedTeeth = computed(() => {
  return (
      bookingTeeth.value.map((bt: any) => ({
        position: toothStore.findItem(bt.toothId)?.position,
        color: "#9f7272",
      })) ?? []
  );
});

const serviceSum = computed(() => {
  const impServices = bookingTeeth.value
      .map((bt: any) => bt.implementedServices)
      .flat();

  return booking.value.implementedServices
      .filter((ims: any) => !impServices.some((s: any) => s.id == ims.id))
      .concat(impServices)
      .map((s: any) => calculateTotal(s))
      .reduce((a: number, b: number) => a + b, 0);
});

const discountSum = computed(() => {
  const discount = booking.value?.discount;
  switch (booking.value.discountType) {
    case DiscountType.Money:
      return discount;
    case DiscountType.Percent:
      return discount ? (serviceSum.value * discount) / 100 : 0;
  }
});

const resultSum = computed(() => serviceSum.value - discountSum.value);

const bookingTeeth = computed(() => {
  const filteredBookingTeeth =
      booking.value?.bookingTeeth?.filter((bt: any) => {
        return !forDeleteBookingTeeth.value.some(
            (fdbt: any) => fdbt.toothId === bt.toothId,
        );
      }) || [];
  return filteredBookingTeeth.concat(forAddBookingTeeth.value ?? []);
});

const implementedServices = computed(() => {
  if (selectedBookingTooth.value !== null) {
    return selectedBookingTooth?.value?.implementedServices ?? [];
  }

  const services = bookingTeeth.value
      .map((bt: any) => bt.implementedServices)
      .flat();
  const commonServices =
      booking.value.implementedServices.filter(
          (is: any) => is.bookingToothId == null,
      ) ?? [];

  return services.concat(commonServices);
});

const calculateTotal = (data: any) => {
  let labaratoryContribution = 0;
  if (useLabaratory.value) {
    labaratoryContribution = data?.labaratoryPrice * data?.count || 0;
  }

  const actualItemPrice = (data?.price * data?.count) - labaratoryContribution;
  const salaryDiscount = (100 - data?.salary) / 100;
  return actualItemPrice * salaryDiscount;
};

const getTabState = (tabName: string) => {
  if (activeTabItem.value === tabName) return {}
  return {
    severity: "secondary",
    text: true
  }
}

const initializePatientData = async (patientId:number) => {
  if(patientDataInitialized.value) {
    showTab.value = true;
    return
  }
  patientObjectDataSource.config.value.id = patientId;

  await Promise.all([
    patientObjectDataSource.get(),
    patientObjectDataSource.getInvoices(),
    bookingListDataSource.updateFilter([
      new Filter("patientId", patientId),
    ]),
    patientSymptomDataSource.updateFilter([
      new Filter("patientId", patientId),
    ]),
    paymentDataSource.updateFilter([new Filter("profileId", patientId)]),
  ]);

  patientDataInitialized.value = true;
  showTab.value = true;
}

const decrementCount = (data: any) => {
  if (data.count > 1) {
    data.count--;
    return;
  }

  if (data.count == 1) {
    removeImplementedService(data);
    return;
  }

}

const addTooth = async () => {
  const tooth = await toothSelector.value.select();
  if (!tooth) return;
  const isExist = booking.value.bookingTeeth.some(
      (bt: any) => bt.toothId == tooth.id,
  );
  const isExistOnAdd = forAddBookingTeeth.value.some(
      (bt: any) => bt.toothId == tooth.id,
  );

  if (isExist || isExistOnAdd) {
    message.info("Этот зуб уже выбран");
    return;
  }
  const bookingTooth = {
    id: getGUID(),
    bookingId: booking.value.id,
    toothId: tooth.id,
    implementedServices: [],
    files: [],
    channels: tooth.channels.map((c: any) => ({
      channelId: c.id,
      masterCon: 0,
      masterFile: 0,
    })),
  };

  forAddBookingTeeth.value.push(bookingTooth);
  selectedBookingTooth.value = bookingTooth;
};

const deleteFile = (file: any) => {
  if (file.className == "BookingFile" || file.bookingId != null) {
    if (typeof file?.id == "number") {
      forDeleteBookingFiles.value.push(file);
    }

    if (booking.value.files.some((bf: any) => bf.id == file.id)) {
      booking.value.files = booking.value.files.filter((f: any) => f.id != file.id);
    }

    if (forAddBookingFiles.value.some((fabf: any) => fabf.id == file.id)) {
      forAddBookingFiles.value = forAddBookingFiles.value.filter((f: any) => f.id != file.id);
    }
    return;
  }

  if (typeof file?.id == "number") {
    forDeleteFiles.value.push(file);
  }

  if (selectedBookingTooth.value != null) {
    const isExistInSelected = selectedBookingTooth.value.files.some(
        (f: any) => f.id == file.id,
    );
    if (isExistInSelected) {
      selectedBookingTooth.value.files =
          selectedBookingTooth.value.files.filter((f: any) => f.id !== file.id);
      return;
    }
  }

  const bookingTooth = bookingTeeth.value.find((bt: any) => bt.id == file.bookingToothId);
  if (bookingTooth) {
    bookingTooth.files = bookingTooth.files.filter((f: any) => f.id !== file.id);
    return;
  }
};

const deleteBookingTooth = async () => {
  const _confirm = await confirm({
    message: "Вы действительно хотите удалить данный обьект?",
    header: "Подтвердите свое действие",
    icon: "pi pi-info-circle",
    acceptClass: "p-button-danger",
  });
  if (!_confirm) return;
  if (typeof selectedBookingTooth.value.id == "number") {
    forDeleteBookingTeeth.value.push({...selectedBookingTooth.value});
  }
  const isExistInBooking = booking.value.bookingTeeth.some(
      (bt: any) => bt.id == selectedBookingTooth.value.id,
  );
  if (isExistInBooking) {
    booking.value.bookingTeeth = booking.value.bookingTeeth.filter(
        (bt: any) => bt.id !== selectedBookingTooth.value.id,
    );
  } else {
    forAddBookingTeeth.value = forAddBookingTeeth.value.filter(
        (bt: any) => bt.id !== selectedBookingTooth.value.id,
    );
  }

  selectedBookingTooth.value = null;
  message.success("Успешно удалено");
};

const addFile = async (file: any) => {
  const src = await getFileSrc(file);
  const re: any = /(?:\.([^.]+))?$/;
  if (selectedBookingTooth.value != null) {
    selectedBookingTooth.value.files.push({
      id: getGUID(),
      base64: src,
      name: file.name,
      fileSize: file.size,
      bookingToothId: selectedBookingTooth.value.id,
      extension: re.exec(file.name)[1],
    });
    return;
  }
  const fileModel = {
    id: getGUID(),
    base64: src,
    name: file.name,
    fileSize: file.size,
    bookingId: booking.value.id,
    extension: re.exec(file.name)[1],
  };
  forAddBookingFiles.value.push(fileModel);
};

const getParents = (branches:any[]) => {
  const parents = [];
  for (const branch of branches) {
    const parent = findElementInTree(branch.parentId,serviceGroupDatasource.root.value);
    if (parent) {
      parents.push(parent);
    }
  }

  return parents;
}

const getSalary = (links:any[]) => {
  const branches = getLastLevelBranches(serviceGroupDatasource.root.value);
  let salary = null;

  if(booking.value.doctor?.salaryType == 0){
    return 0;
  }

  if(booking.value.doctor?.salaryType == 1){
    return booking.value.doctor.salary;
  }

  (function findSalary(items:any[], groupLinks:any[]){
    const defined = items.find(sg => groupLinks.some((l:any) => sg.id == l.serviceGroupId));

    if(!defined){
      findSalary(getParents(items), groupLinks);
    } else {
      const groups = collectTreeBranchParents(defined, serviceGroupDatasource.root.value)
      for (const group of groups) {
        salary = doctorServiceGroupSalaryDataSource.items.value.find(s => s.serviceGroupId == group.id)?.salary;
        if(salary){
          break;
        }
      }

      if(!salary){
        salary = booking.value.doctor?.salary;
      }
    }
  })(branches, links)

  return salary;
}

const addOrDeleteImplementedService = (data: any) => {
  const selected = isSelected(data);
  let defined = forDeleteImplementedServices.value.find((b: any) => b.serviceId == data.id);

  //add
  if (!selected) {

    const salary = getSalary(data.links);

    const implementedService = {
      id: getGUID(),
      bookingId: booking.value.id,
      serviceId: data.id,
      caption: data.caption,
      salary,
      labaratoryPrice: data.labaratoryPrice,
      salaryType: booking.value.doctor?.salaryType,
      doctorId: booking.value.doctorId,
      price: data.price,
      code: data.code,
      service: data,
      count: 1,
    };

    if (defined) {
      forDeleteImplementedServices.value = forDeleteImplementedServices.value
          .filter((b: any) => b.serviceId !== data.id);
    } else {
      forAddImplementedServices.value.push(implementedService);
    }


    if (selectedBookingTooth.value != null) {
      selectedBookingTooth.value.implementedServices.push({
        ...implementedService,
        bookingToothId: selectedBookingTooth.value.id,
      });
      return;
    }

    booking.value.implementedServices.push(implementedService);
    return;
  }

  //remove
  if (selectedBookingTooth.value !== null) {
    defined = selectedBookingTooth.value.implementedServices
        .find((b: any) => b.serviceId == data.id);

    if (defined?.id) {
      forDeleteImplementedServices.value.push(defined);
    }

    forAddImplementedServices.value = forAddImplementedServices.value
        .filter((s:any) => s.serviceId != defined.serviceId);


    selectedBookingTooth.value.implementedServices = selectedBookingTooth.value.implementedServices
        .filter((b: any) => b.serviceId !== data.id);

    const defineInCommon = booking.value.implementedServices.some((is: any) => is.id == defined?.id);

    if (defineInCommon) {
      booking.value.implementedServices = booking.value.implementedServices
          .filter((b: any) => b.serviceId !== data.id);
    }

    return;
  }

  //remove
  defined = booking.value.implementedServices.find((b: any) => b.serviceId == data.id);

  forAddImplementedServices.value = forAddImplementedServices.value
      .filter((s:any) => s.serviceId != defined.serviceId);


  if (defined?.id) {
    forDeleteImplementedServices.value.push(defined);
  }

  booking.value.implementedServices = booking.value.implementedServices
      .filter((b: any) => b.serviceId !== data.id);

  forAddImplementedServices.value = forAddImplementedServices.value
      .filter((is: any) => is.id !== data.id);
};

const removeImplementedService = (data: any) => {
  if (isNumber(data.id)) {
    forDeleteImplementedServices.value.push(data);
  }

  forAddImplementedServices.value = forAddImplementedServices.value
      .filter((s:any) => s.serviceId != data.serviceId);

  if (selectedBookingTooth.value !== null) {
    selectedBookingTooth.value.implementedServices = selectedBookingTooth.value.implementedServices
        .filter((b: any) => b.serviceId !== data.serviceId);
    return;
  }

  if (data.bookingToothId) {
    const bookingTooth = bookingTeeth.value.find((bt: any) => bt.id == data.bookingToothId);

    if (!bookingTooth) throw new Error("Ошибка при удалении проделанной работы");

    bookingTooth.implementedServices = bookingTooth.implementedServices.filter(
        (b: any) => b.serviceId !== data.serviceId,
    );

    const defineInCommon = booking.value.implementedServices.some(
        (is: any) => is.id == data?.id,
    );

    if (defineInCommon) {
      booking.value.implementedServices = booking.value.implementedServices.filter(
          (is: any) => is.id !== data.id,
      );
    }

    return;
  }

  booking.value.implementedServices = booking.value.implementedServices.filter(
      (b: any) => b.serviceId !== data.serviceId,
  );
};

const updateFilter = async (data: any) => {
  await serviceDataSource.updateFilter([new Filter("groupId", data.key)]);
};

const clearFilter = async () => {
  await serviceDataSource.removeFilter(["groupId"]);
};

const isSelected = (data: any) => implementedServices.value.some((is: any) => is.serviceId == data.id);

const getChannelCaption = (bc: any) => {
  const tooth = toothStore.findItem(selectedBookingTooth.value.toothId);
  if (!tooth) return "";
  const channel = tooth.channels.find((c: any) => c.id == bc.channelId);
  if (!channel) return "";

  return channel.caption;
};

const getFileSrc = async (file: File): Promise<string> => {
  return new Promise<string>((resolve, reject) => {
    const reader = new FileReader();

    reader.onload = (event) => {
      const result = event.target?.result as string;
      resolve(result.substr(result.indexOf(",") + 1));
    };

    reader.onerror = (error) => {
      reject(error);
    };

    reader.readAsDataURL(file);
  });
};

const getImageSrc = (item: any) => {
  if (!(typeof item.id == "number"))
    return `data:image/png;base64,${item.base64}`;
  return `/api/v1/file/${item.id}`;
};

const expandNode = (node: any) => {
  if (node.children && node.children.length) {
    expandedKeys.value[node.key] = true;
    for (let child of node.children) {
      expandNode(child);
    }
  }
};

const open = async (model: any) => {
  booking.value = _.cloneDeep(model);
  showTab.value = false;
  visible.value = true;
  await initTreeSearchField();

  await doctorServiceGroupSalaryDataSource.updateFilter([ new Filter("doctorId", model.doctorId)])
  await serviceGroupDatasource.getRoot(clinicStore.clinic?.id!);
  const rootId = serviceGroupDatasource.root.value[0]?.id;
  expandedKeys.value = {...expandedKeys.value};
  selectedKey.value = {[rootId]: true};

  if(!serviceInitialized.value){
    await updateFilter({key: rootId});
    serviceInitialized.value = true;
  }

  await toothStore.getData();



  return new Promise((r) => (resolve.value = r));
};

const initTreeSearchField = async () => {
  await nextTick();
  const compactInput = document.querySelector(".tree-filter");
  const treeFilterContainer = document.querySelector(
      ".p-tree-filter-container",
  );
  if (compactInput && treeFilterContainer) {
    const svgElement = treeFilterContainer.querySelector("svg");
    if (svgElement) {
      treeFilterContainer.removeChild(svgElement);
    }
    const inputElement = treeFilterContainer.querySelector("input");
    if (inputElement) {
      inputElement.setAttribute("placeholder", "Поиск");
    }
    compactInput.appendChild(treeFilterContainer);
  }
};

const reset = () => {
  visible.value = false;
  booking.value = null;
  forDeleteBookingTeeth.value = [];
  forAddBookingTeeth.value = [];
  forDeleteFiles.value = [];
  forDeleteImplementedServices.value = [];
  selectedBookingTooth.value = null;
  resolve.value!(null);
};

const submit = async () => {
  if (!resolve.value) return;
  const _confirm = await confirm({
    message: "Внимательно всё проверьте изменить нельхя будет",
    header: "Подтвердите свое действие",
    icon: "pi pi-info-circle",
    acceptClass: "p-button-danger",
  });
  if (!_confirm) return;

  visible.value = false;
  for (const bookingTooth of forAddBookingTeeth.value) {
    delete bookingTooth.id;
    for (const implementedService of bookingTooth.implementedServices) {
      delete implementedService.id;
      delete implementedService.service;
      delete implementedService.bookingToothId;
    }
    for (const file of bookingTooth.files) {
      delete file.id;
      delete file.bookingToothId;
    }
  }

  for (const file of forAddBookingFiles.value) {
    if (typeof file?.id == "string") {
      delete file.id;
    }
  }

  for (const bookingTooth of forDeleteBookingTeeth.value) {
    for (const implementedService of bookingTooth.implementedServices) {
      delete implementedService.bookingToothId;
    }
  }
  const updateBookingTeeth = booking.value.bookingTeeth.filter?.(
      (bt: any) =>
          !forDeleteBookingTeeth.value.some(
              (fdbt: any) => fdbt.toothId == bt.toothId,
          ),
  );

  for (const bookingTooth of updateBookingTeeth) {
    if (typeof bookingTooth?.id == "string") {
      delete bookingTooth.id;
    }

    delete bookingTooth.implementedServices;

    bookingTooth.files = bookingTooth.files.filter(
        (f: any) => f.base64 != null,
    );
    for (const file of bookingTooth.files) {
      if (typeof file?.id == "string") {
        delete file.id;
      }
    }
  }
  for (const implementedService of forAddImplementedServices.value) {
    delete implementedService.id;
    delete implementedService.service;
  }

  resolve.value!(_.cloneDeep({
    booking: booking.value,
    updateModel: {
      forAddBookingFiles: forAddBookingFiles.value,
      forDeleteBookingFiles: forDeleteBookingFiles.value,
      forAddBookingTeeth: forAddBookingTeeth.value,
      forDeleteBookingTeeth: forDeleteBookingTeeth.value,
      forUpdateBookingTeeth: updateBookingTeeth,
      forDeleteImplementedServices: forDeleteImplementedServices.value,
      forAddImplementedServices: forAddImplementedServices.value,
      forDeleteBookingToothFiles: forDeleteFiles.value,
    },
  }));

  booking.value = null;
  forDeleteBookingTeeth.value = [];
  forAddBookingFiles.value = [];
  forDeleteBookingTeeth.value = [];
  forAddBookingTeeth.value = [];
  forDeleteFiles.value = [];
  forDeleteImplementedServices.value = [];
  selectedBookingTooth.value = null;
  visible.value = false;
};

const handleResize = () => {
  maximizable.value = window.innerWidth > 768;
};

defineExpose({open});

onMounted(async () => {
  handleResize();
  window.addEventListener('resize', handleResize);
  for (let node of serviceGroupDatasource.transformedRoot.value) {
    expandNode(node);
  }
  const rootId = serviceGroupDatasource.root.value[0]?.id;
  expandedKeys.value = {...expandedKeys.value};
  selectedKey.value = {[rootId]: true};
});
</script>


<style>
.p-tabview .p-tabview-nav li .p-tabview-nav-link {
  background-color: transparent;
}

.p-tabview .p-tabview-nav li.p-highlight .p-tabview-nav-link {
  background-color: transparent;
}

.booking-report-dialog .p-dialog-footer {
  margin-top: auto !important;
}

.booking-report-dialog .p-dialog-content {
  height: 100%;
  position: relative !important;
}

@media screen and (max-width: 768px) {
  .booking-report-dialog .p-dialog-header-close {
    display: none;
  }
}
</style>



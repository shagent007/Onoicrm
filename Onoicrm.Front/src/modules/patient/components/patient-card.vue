<template>
  <!-- desktop -->
  <TabView
    v-if="width > 992"
    class="tab-patient-id tab-patient-id--payment patient-tab relative"
  >
    <TabPanel header="Анкета">
      <div class="grid h-full">
        <div class="col-7">
          <div
            class="card flex flex-column align-items-start"
            style="border-radius: 10px"
          >
            <div class="tab-patient-id tab-patient-id--profile w-full">
              <div class="tab-panel-header py-0 px-3">
                <span class="block sm:hidden p-buttonset">
                  <Button size="small" label="Первичный" />
                  <Button size="small" label="Повторный" />
                </span>
              </div>

              <div class="tab-panel-body">
                <div
                  class="flex align-items-center justify-content-between w-full"
                >
                  <h4 class="mr-3">Дополнительная информация!</h4>
                  <Button class="edit-modal" @click="updateProfile">
                    <edit></edit>
                  </Button>
                </div>
                <Divider class="my-3" />
                <table class="clean-table">
                  <tr>
                    <td>
                      <b>ФИО:</b>
                    </td>
                    <td class="pl-2">
                      {{ patientDataSource.model.value?.fullName }}
                    </td>
                  </tr>
                  <tr>
                    <td>
                      <b>Дата рождения:</b>
                    </td>
                    <td class="pl-2">
                      {{
                        patientDataSource.model.value?.birthDate
                          ? moment(
                              patientDataSource.model.value?.birthDate
                            ).format("DD.MM.YYYY")
                          : "---"
                      }}
                    </td>
                  </tr>
                  <tr>
                    <td>
                      <b>Адрес:</b>
                    </td>
                    <td class="pl-2">
                      {{
                        patientDataSource.model.value?.address
                          ? patientDataSource.model.value?.address
                          : "---"
                      }}
                    </td>
                  </tr>
                  <tr>
                    <td>
                      <b>Телефон:</b>
                    </td>
                    <td class="pl-2">
                      {{ patientDataSource.model.value?.whatsAppNumber }}
                    </td>
                  </tr>
                  <tr>
                    <td>
                      <h4 class="mt-2">От куда узнал:</h4>
                    </td>
                    <td class="pl-2">
                      <span>
                        {{ informationSource }}
                      </span>
                    </td>
                  </tr>
                  <tr>
                    <td>
                      <b>
                        <h4 class="mr-3">Статус:</h4>
                      </b>
                    </td>
                    <td class="pl-2">
                      <span class="p-buttonset">
                        <Button size="small" label="Первичный" />
                        <Button size="small" label="Повторный" />
                      </span>
                    </td>
                  </tr>
                </table>
              </div>
            </div>
          </div>
        </div>
        <div class="col-5">
          <div class="card h-full">
            <div class="tab-patient-id tab-patient-id--profile w-full">
              <div class="tab-panel-body">
                <div
                  class="flex align-items-center justify-content-between w-full"
                >
                  <h4 class="mr-3">Обратите внимание!</h4>
                  <Button class="edit-modal" @click="updateSymptom">
                    <edit></edit>
                  </Button>
                </div>
                <Divider class="my-3" />
                <PatientSymptoms
                  :patient-symptoms="patientSymptomsDataSource.items.value"
                />
              </div>
            </div>
          </div>
        </div>
      </div>
    </TabPanel>
    <TabPanel header="Визиты и оплаты">
      <Button
        class="hidden lg:flex absolute top-0 right-0 border-round-md text-sm"
        label="Завершить"
        @click="finishBg"
      />
      <div class="flex align-items-center justify-content-between w-full mb-3">
        <div>
          <ul
            class="flex align-center gap-2 list-none w-full overflow-x-hidden"
          >
            <li v-for="bg in bookingGroupDataSource.items.value" :key="bg.id">
              <p @click="updateFilter(bg)" class="tab-patient-date" :class="{'tab-patient-date--active':bg.id == bookingGroup?.id}">
                {{ moment(bg.startDate).format("DD.MM.YYYY") }}
              </p>
            </li>

          </ul>
        </div>
        <div>
          <Button
            @click="showPayment"
            v-if="!isFixedCard"
            class="border-round-md text-sm"
            icon="pi pi-plus"
            label="Добавить оплату"
          />
        </div>
      </div>
      <div class="grid">
        <div class="col-8">
          <div
            style="height: calc(100vh - 257px); border-radius: 10px !important"
            class="card block indent-table overflow-x-hidden"
          >
            <DataTable
              scrollable
              @row-click="showReport($event)"
              scroll-height="400px"
              class="no-scroll-table"
              :value="patientDataSource.invoices.value"
            >
              <Column field="date" header="Дата">
                <template #body="{ data }">
                  {{ moment(data?.date).format("DD.MM.YYYY") }}
                </template>
              </Column>
              <Column
                header="Врач"
                field="doctor.fullName"
              />
              <Column header="Зубы">
                <template #body="{ data }: any">
                  <Tag
                    class="mr-2 bg-blue-50 text-blue-500 border-1 border-blue-500"
                    v-for="bt in data?.bookingTeeth"
                    :key="bt.id"
                    :value="toothStore.findItem(bt?.toothId)?.position"
                  />
                </template>
              </Column>
              <Column header="Сумма">
                <template #body="{ data }">{{ getSum(data) }}</template>
              </Column>
              <Column header="Скидка">
                <template #body="{ data }">
                  {{ getDiscount(data) }}
                </template>
              </Column>
              <Column header="Итого" field="total" />
            </DataTable>
          </div>
        </div>
        <div class="col-4">
          <div
            style="height: calc(100vh - 257px)"
            class="card block indent-table overflow-y-hidden"
          >
            <DataTable
              scrollable
              scroll-height="360px"
              :value="paymentDataSource.items.value"
            >
              <Column field="birthDate" header="Дата">
                <template #body="{ data }">
                  {{ moment(data?.createDate).format("DD.MM.YYYY") }}
                </template>
              </Column>
              <Column field="sum" header="Сумма" />
              <Column header="Способ оплаты">
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
        </div>
        <div class="col-12 total pb-0">
          <div class="card p-3">
            <div class="flex justify-content-between align-items-center w-full">
              <p class="total-sum__caption">
                Общая сумма: <span class="total-sum">{{ total }}</span>
              </p>

              <div class="flex align-items-center">
                <p class="total-paid__caption mr-8">
                  Оплатил: <span class="total-paid">{{ totalPaid }}</span>
                </p>
                <p class="total-debt__caption">
                  Долг: <span class="total-debt">{{ total - totalPaid }}</span>
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
      <Button
        @click="showPayment"
        v-if="isFixedCard"
        class="bottom-0 absolute right-0 border-round-md"
        icon="pi pi-plus"
      />
    </TabPanel>
  </TabView>
  <!--  tablet  -->
  <TabView
    v-if="width > 576 && width < 992"
    class="tab-patient-id tab-patient-id--payment patient-tab relative"
  >
    <TabPanel header="Анкета">
      <div class="grid h-full">
        <div class="col-12">
          <div
            class="card flex flex-column align-items-start"
            style="border-radius: 10px"
          >
            <div class="tab-patient-id tab-patient-id--profile w-full">
              <div class="tab-panel-body">
                <div
                  class="flex align-items-center justify-content-between w-full"
                >
                  <h4 class="mr-3">Дополнительная информация!</h4>
                  <Button class="edit-modal" @click="updateProfile">
                    <edit></edit>
                  </Button>
                </div>
                <Divider class="my-3" />
                <table class="clean-table">
                  <tr>
                    <td>
                      <b>ФИО:</b>
                    </td>
                    <td class="pl-2">
                      {{ patientDataSource.model.value?.fullName }}
                    </td>
                  </tr>
                  <tr>
                    <td>
                      <b>Дата рождения:</b>
                    </td>
                    <td class="pl-2">
                      {{
                        moment(patientDataSource.model.value?.birthDate).format(
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
                      {{ patientDataSource.model.value?.address }}
                    </td>
                  </tr>
                  <tr>
                    <td>
                      <b>Телефон:</b>
                    </td>
                    <td class="pl-2">
                      {{ patientDataSource.model.value?.whatsAppNumber }}
                    </td>
                  </tr>
                  <tr>
                    <td>
                      <h4 class="mt-2">От куда узнал:</h4>
                    </td>
                    <td class="pl-2">
                      <span>
                        {{ informationSource }}
                      </span>
                    </td>
                  </tr>
                  <tr>
                    <td>
                      <b>
                        <h4 class="mr-3">Статус:</h4>
                      </b>
                    </td>
                    <td class="pl-2">
                      <span class="p-buttonset">
                        <Button size="small" label="Первичный" />
                        <Button size="small" label="Повторный" />
                      </span>
                    </td>
                  </tr>
                </table>
              </div>
            </div>
          </div>
        </div>
        <div class="col-12">
          <div class="card h-full">
            <div class="tab-patient-id tab-patient-id--profile w-full">
              <div class="tab-panel-body">
                <div
                  class="flex align-items-center justify-content-between w-full"
                >
                  <h4 class="mr-3">Обратите внимание!</h4>
                  <Button class="edit-modal" @click="updateSymptom">
                    <edit></edit>
                  </Button>
                </div>
                <Divider class="my-3" />
                <PatientSymptoms
                  :patient-symptoms="patientSymptomsDataSource.items.value"
                />
              </div>
            </div>
          </div>
        </div>
      </div>
    </TabPanel>
    <TabPanel header="Визиты">
      <Button
        class="absolute top-0 right-0 border-round-md text-sm"
        label="Завершить"
      />
      <div class="flex align-items-center justify-content-between w-full mb-3">
        <div>
          <ul
            class="flex align-center gap-2 list-none w-full overflow-x-hidden"
          >
            <li>
              <p class="tab-patient-date tab-patient-date--active">
                {{ patientDataSource.model.value?.firstName }}
              </p>
            </li>
            <li>
              <p class="tab-patient-date">
                {{ patientDataSource.model.value?.firstName }}
              </p>
            </li>
          </ul>
        </div>
      </div>
      <div class="grid">
        <div class="col-12 mb-8">
          <div
            style="height: calc(100vh - 257px); border-radius: 10px !important"
            class="block overflow-x-hidden"
          >
            <DataTable
              scrollable
              @row-click="showReport($event)"
              scroll-height="700px"
              class="no-scroll-table"
              :value="patientDataSource.invoices.value"
            >
              <Column field="date" header="Дата">
                <template #body="{ data }">
                  {{ moment(data?.date).format("DD.MM.YYYY") }}
                </template>
              </Column>
              <Column
                header="Врач"
                field="doctor.fullName"
              />
              <Column header="Зубы">
                <template #body="{ data }: any">
                  <Tag
                    class="mr-2 mb-1 bg-blue-50 text-blue-500 border-1 border-blue-500 px-2"
                    v-for="bt in data?.bookingTeeth"
                    :key="bt.id"
                    :value="toothStore.findItem(bt?.toothId)?.position"
                  />
                </template>
              </Column>
              <Column header="Сумма">
                <template #body="{ data }">{{ getSum(data) }}</template>
              </Column>
              <Column header="Скидка">
                <template #body="{ data }">
                  {{ getDiscount(data) }}
                </template>
              </Column>
              <Column header="Итого" field="total" />
            </DataTable>
          </div>
        </div>

        <div class="fixed bottom-0 left-0 total w-full pb-0">
          <div class="card flex-column p-3 w-full">
            <div
              class="flex justify-content-between align-items-center w-full mb-4"
            >
              <p class="total-paid__caption mr-8">
                Оплатил: <span class="total-paid">{{ totalPaid }}</span>
              </p>
              <p class="total-sum__caption">
                Общая сумма: <span class="total-sum">{{ total }}</span>
              </p>
            </div>
            <div class="flex justify-content-between align-items-center w-full">
              <p class="total-debt__caption">
                Долг: <span class="total-debt">{{ total - totalPaid }}</span>
              </p>
              <p class="total-sum__caption">
                Итог: <span class="total-sum">{{ total - totalPaid }}</span>
              </p>
            </div>
          </div>
        </div>
      </div>
      <Button
        @click="showPayment"
        v-if="isFixedCard"
        class="bottom-0 absolute right-0 border-round-md"
        icon="pi pi-plus"
      />
    </TabPanel>
    <TabPanel header="Оплаты">
      <div class="flex align-items-center justify-content-between w-full mb-3">
        <div>
          <ul class="flex align-items-center list-none">
            <li>
              <p class="patient-tab-date">
                {{ patientDataSource.model.value?.fullName }}
              </p>
            </li>
          </ul>
        </div>
        <div>
          <Button
            @click="showPayment"
            v-if="!isFixedCard"
            class="border-round-md text-sm"
            icon="pi pi-plus"
            label="Добавить оплату"
          />
        </div>
      </div>
      <div class="grid">
        <div class="col-12 mb-8">
          <div
            style="height: calc(100vh - 257px)"
            class="card block indent-table overflow-y-hidden"
          >
            <DataTable
              scrollable
              scroll-height="360px"
              :value="paymentDataSource.items.value"
            >
              <Column field="birthDate" header="Дата">
                <template #body="{ data }">
                  {{ moment(data?.createDate).format("DD.MM.YYYY") }}
                </template>
              </Column>
              <Column field="sum" header="Сумма" />
              <Column header="Способ оплаты">
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
        </div>
        <div class="fixed bottom-0 left-0 total w-full pb-0">
          <div class="card flex-column p-3 w-full">
            <div
              class="flex justify-content-between align-items-center w-full mb-4"
            >
              <p class="total-paid__caption mr-8">
                Оплатил: <span class="total-paid">{{ totalPaid }}</span>
              </p>
              <p class="total-sum__caption">
                Общая сумма: <span class="total-sum">{{ total }}</span>
              </p>
            </div>
            <div class="flex justify-content-between align-items-center w-full">
              <p class="total-debt__caption">
                Долг: <span class="total-debt">{{ total - totalPaid }}</span>
              </p>
              <p class="total-sum__caption">
                Итог: <span class="total-sum">{{ total - totalPaid }}</span>
              </p>
            </div>
          </div>
        </div>
      </div>
    </TabPanel>
  </TabView>
  <!--  mobile  -->
  <TabView
    v-if="width < 576"
    class="tab-patient-id tab-patient-id--payment patient-tab relative"
  >
    <TabPanel header="Анкета">
      <div
        class="flex flex-column align-items-start mb-3"
        style="border-radius: 10px"
      >
        <div class="tab-patient-id tab-patient-id--profile w-full">
          <Button
            class="absolute right-0 edit-modal mb-1"
            @click="updateProfile"
            style="top: 6px"
          >
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
      </div>
      <div class="card p-3 mb-3">
        <div>
          <p class="mobile-card-description text-base mb-2">ФИО</p>
          <h6 class="mobile-card-caption">
            {{ patientDataSource.model.value?.fullName }}
          </h6>
        </div>
      </div>
      <div class="card p-3 mb-3">
        <div>
          <p class="mobile-card-description text-base mb-2">Дата рождение</p>
          <h6
            v-if="patientDataSource.model.value?.birthDate"
            class="mobile-card-caption"
          >
            {{
              moment(patientDataSource.model.value?.birthDate).format(
                "DD.MM.YYYY"
              )
            }}
          </h6>
          <h6 v-else class="mobile-card-caption">Неверная дата!</h6>
        </div>
      </div>
      <div class="card p-3 mb-3">
        <div>
          <p class="mobile-card-description text-base mb-2">Адрес</p>
          <h6 class="mobile-card-caption">
            {{ patientDataSource.model.value?.address }}
          </h6>
        </div>
      </div>
      <div class="card p-3 mb-3">
        <div>
          <p class="mobile-card-description text-base mb-2">Телефон</p>
          <h6 class="mobile-card-caption">
            {{ patientDataSource.model.value?.whatsAppNumber }}
          </h6>
        </div>
      </div>
      <div class="card p-3 mb-3">
        <div>
          <p class="mobile-card-description text-base mb-2">
            От куда узанал(а):
          </p>
          <h6 class="mobile-card-caption">
            {{ informationSource }}
          </h6>
        </div>
      </div>
      <div class="card p-3 mb-3">
        <div class="flex flex-column">
          <h4 class="mr-3 mb-2">Статус:</h4>
          <span class="p-buttonset">
            <Button
              class="text-base md:text-lg mr-3"
              size="small"
              label="Первичный"
            />
            <Button
              class="text-base md:text-lg"
              size="small"
              label="Повторный"
            />
          </span>
        </div>
      </div>
      <div>
        <h4
          class="mobile-card-description text-base border-bottom-1 border-gray-700 pb-2 mb-2"
        >
          Обратите внимание
        </h4>
        <PatientSymptoms
          :patient-symptoms="patientSymptomsDataSource.items.value"
        />
      </div>
    </TabPanel>
    <TabPanel header="Визиты">
      <div class="grid">
        <div class="col-12 mb-8">
          <div
            v-for="item in patientDataSource.invoices.value"
            :key="item.id"
            style="border-radius: 5px"
            class="card flex flex-column align-items-start mb-3 p-2"
          >
            <div
              class="flex align-items-center justify-content-between w-full mb-2"
            >
              <div>
                <p class="mobile-card-description">Дата</p>
                <p class="mobile-card-description text-base">
                  {{ moment(item?.date).format("DD.MM.YYYY") }}
                </p>
              </div>
              <div>
                <Tag
                  class="mr-2 mb-1 bg-blue-50 text-blue-500 border-1 border-blue-500 px-2"
                  v-for="bt in item?.bookingTeeth"
                  :key="bt.id"
                  :value="toothStore.findItem(bt?.toothId)?.position"
                />
              </div>
            </div>
            <div class="w-full">
              <div class="border-bottom-1 py-2">
                <p class="mobile-card-description">Врач</p>
                <p class="mobile-card-caption">{{ item.doctor.fullName }}</p>
              </div>
              <div class="grid">
                <div class="col-4">
                  <p class="mobile-card-description">Сумма</p>
                  <p class="mobile-card-caption">{{ getSum(item) }}</p>
                </div>
                <div class="col-4">
                  <p class="mobile-card-description">Скидка</p>
                  <p class="mobile-card-caption">{{}}</p>
                </div>
                <div class="col-4">
                  <p class="mobile-card-description">Итог</p>
                  <p class="mobile-card-caption">{{}}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="fixed bottom-0 left-0 total w-full pb-0">
          <div class="card flex-column p-3 w-full">
            <div
              class="flex justify-content-between align-items-center w-full mb-3"
            >
              <p class="total-paid__caption mr-8">
                Оплатил:
                <span class="total-paid block mt-2 ml-0">{{ totalPaid }}</span>
              </p>
              <p class="total-sum__caption flex flex-column">
                Общая сумма:
                <span class="total-sum mt-2 flex align-self-end">{{
                  total
                }}</span>
              </p>
            </div>
            <div class="flex justify-content-between align-items-center w-full">
              <p class="total-debt__caption">
                Долг:
                <span class="total-debt block mt-2 ml-0">{{
                  total - totalPaid
                }}</span>
              </p>
              <p class="total-sum__caption flex flex-column align-items-end">
                Итог:
                <span class="total-sum flex align-self-end mt-2">{{
                  total - totalPaid
                }}</span>
              </p>
            </div>
          </div>
        </div>
      </div>
      <Button
        @click="showPayment"
        v-if="isFixedCard"
        class="top-0 absolute right-0 border-round-md"
        icon="pi pi-plus"
      />
    </TabPanel>
    <TabPanel header="Оплаты">
      <div
        class="flex flex-column align-items-start mb-3"
        style="border-radius: 10px"
      >
        <div class="tab-patient-id tab-patient-id--profile w-full">
          <Button
            v-if="appStore.buttons.length > 0"
            v-for="button in appStore.buttons"
            class="edit-modal edit-modal-mobile bg-blue-500 border-none p-2 mb-1"
            style="border-radius: 8px !important"
            @click="button.action()"
            icon="pi pi-plus text-xs"
          />
        </div>
      </div>
      <div class="grid">
        <div class="col-12">
          <div class="grid border-bottom-1 m-0">
            <div class="col-4">
              <p class="mobile-card-description text-sm">Дата</p>
            </div>
            <div class="col-3 pl-0">
              <p class="mobile-card-description text-sm">Сумма</p>
            </div>
            <div class="col-5">
              <p class="mobile-card-description text-sm">Способ оплаты</p>
            </div>
          </div>
        </div>
        <div class="col-12 mb-8">
          <div
            v-for="item in paymentDataSource.items.value"
            class="card mb-3 p-2"
          >
            <div class="grid w-full">
              <div class="col-4 flex align-items-center">
                <p class="mobile-card-caption">
                  {{ moment(item?.date).format("DD.MM.YYYY") }}
                </p>
              </div>
              <div class="col-3 flex align-items-center">
                <p class="mobile-card-caption">{{ item.sum }}</p>
              </div>
              <div class="col-5">
                <p class="mobile-card-caption flex align-items-center">
                  <img
                    class="mr-2"
                    style="width: 24px"
                    :src="getImg(item?.method).img"
                    alt=""
                  />
                  <span>{{ item?.method }}</span>
                </p>
              </div>
            </div>
          </div>
        </div>
        <div class="fixed bottom-0 left-0 total w-full pb-0">
          <div class="card flex-column p-3 w-full">
            <div
              class="flex justify-content-between align-items-center w-full mb-3"
            >
              <p class="total-paid__caption mr-8">
                Оплатил:
                <span class="total-paid block mt-2 ml-0">{{ totalPaid }}</span>
              </p>
              <p class="total-sum__caption flex flex-column">
                Общая сумма:
                <span class="total-sum mt-2 flex align-self-end">
                  {{total }}
                </span>
              </p>
            </div>
            <div class="flex justify-content-between align-items-center w-full">
              <p class="total-debt__caption">
                Долг:
                <span class="total-debt block mt-2 ml-0">
                  {{total - totalPaid}}
                </span>
              </p>
              <p class="total-sum__caption flex flex-column align-items-end">
                Итог:
                <span class="total-sum flex align-self-end mt-2">
                  {{total - totalPaid }}
                </span>
              </p>
            </div>
          </div>
        </div>
      </div>
      <Button
        @click="showPayment"
        v-if="isFixedCard"
        class="top-0 absolute right-0 border-round-md"
        icon="pi pi-plus"
      />
    </TabPanel>
  </TabView>
  <booking-report-dialog-viewer ref="bookingViewer" />
  <patient-payment-dialog
    ref="paymentDialog"
    :payment-data-source="paymentDataSource"
  />
  <Dialog
      class="w-12 sm:w-9 xl:w-7"
    header="Обновить данные"
    v-model:visible="visible"
  >
    <app-form :form-service="formService" />

    <template #footer>
      <Button
        label="Отмена"
        @click="visible = false"
        icon="pi pi-times"
        class="p-button-text"
      />
      <Button label="Сохранить" @click="save" icon="pi pi-check" autofocus />
    </template>
  </Dialog>
  <Dialog
      class="w-12 sm:w-9 xl:w-7"
    header="Обновить данные"
    v-model:visible="visibleSymptom"
  >
  <symptom-editor
      :patient-id="patientDataSource.model.value.id"
      :data-source="patientSymptomsDataSource"
  />

    <template #footer>
      <Button
        label="Отмена"
        @click="visibleSymptom = false"
        icon="pi pi-times"
        class="p-button-text"
      />
      <Button label="Сохранить" @click="save" icon="pi pi-check" autofocus />
    </template>
  </Dialog>
</template>

<script setup lang="ts">
import moment from "moment/moment";
import edit from "../icons/edit.svg";
import {
  Filter,
  IListDataSource,
  ListDataSourceConfig, UpdateFieldModel,
  useBookingObjectDataSource,
  useListDataSource,
  useTreeDataSource,
} from "~/modules/data-sources";
import { ref, computed, inject, onMounted } from "vue";
import PatientSymptoms from "~/modules/symptom/components/patient-symptoms.vue";
import { IPatientObjectDataSource } from "~/modules/user-profile/interfaces/IPatientObjectDataSource";
import { DiscountType } from "~/modules/booking/entities/Booking";
import BookingReportDialogViewer from "~/modules/booking/components/booking-report-dialog-viewer.vue";
import { AppForm, FormService, FormServiceConfig } from "~/modules/app-form";
import { ContextMenu, defaultMessage, IMessage } from "~/shared";
import PatientPaymentDialog from "~/modules/patient/components/patient-payment-dialog.vue";
import { useToothStore } from "~/modules/tooth";
import { getPatientUpdateItemForm } from "~/modules/patient/forms/getPatientUpdateItemForm";
import { useUserProfileGroupDataSource } from "~/modules/user-profile-group/useUserProfileGroupDataSource";
import { TreeDataSourceConfig } from "~/modules/data-sources/hooks/useTreeDataSource";
import { useClinicStore } from "~/modules/clinic/stores/useClinicStore";
import Edit from "../icons/edit.svg";
import SymptomEditor from "~/modules/symptom/components/symptom-editor.vue";
import _ from "lodash";

import { useEditorStore } from "~/modules/editor/stores/useEditorDialog";
import { useWindowSize } from "@vueuse/core";
import { useAppStore } from "~/modules/app/stores/useAppStore";
const visible = ref(false);
const visibleSymptom = ref(false);
const {
  patientDataSource,
  patientSymptomsDataSource,
  isFixedCard,
  paymentDataSource,
} = defineProps<{
  paymentDataSource: IListDataSource<any>;
  patientDataSource: IPatientObjectDataSource;
  patientSymptomsDataSource: IListDataSource<any>;
  isFixedCard: boolean;
}>();
const { width } = useWindowSize();
const bookingGroup = ref();
const appStore = useAppStore();
const clinicStore = useClinicStore();
const toothStore = useToothStore();
const bookingViewer = ref();
const message = inject<IMessage>("message", defaultMessage);
const paymentDialog = ref();
const objectDataSource = useBookingObjectDataSource();
const informationSourceDataSource = useListDataSource(
  new ListDataSourceConfig({
    className: "InformationSource",
  })
);

const bookingGroupDataSource = useListDataSource(
  new ListDataSourceConfig({
    className: "BookingGroup",
    filter: [
      new Filter("clinicId", clinicStore.clinic?.id),
      new Filter("patientId", patientDataSource.model.value.id),
    ],
  })
);


const getDiscount = (data: any) => {
  const discount = data.discount;
  switch (data.discountType) {
    case DiscountType.Money:
      return discount;
    case DiscountType.Percent:
      return (data.sum * discount) / 100;
  }
};
const informationSource = computed(() => {
  return informationSourceDataSource.findItem(
    patientDataSource.model.value.informationSourceId
  )?.caption;
});

const save = async () => {
  await patientDataSource.update(formService.model);
  visible.value = false;
  Object.assign(patientDataSource.model.value, formService.model);
  message.success("Изменения успешно сохранено");
};

const total = computed(() => {
  return patientDataSource.invoices.value
    .map((i) => getSum(i))
    .reduce((total, i) => total + i, 0);
});
const totalPaid = computed(() => {
  return paymentDataSource.items.value.reduce((total, i) => total + i.sum, 0);
});
const updateProfile = () => {
  visible.value = true;
};
const updateSymptom = () => {
  visibleSymptom.value = true;
};

const updateFilter = async (bg:any) => {
  bookingGroup.value = bg;
  await patientDataSource.updateInvoices(bg.id)
}

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
  const discount = getDiscount(data);
  return data.sum - discount;
};
const getImg = (caption: string) => {
  const index = paymentMethods.value.findIndex((i) => i.caption == caption);
  return paymentMethods.value[index];
};
const showReport = async ({ data }: any) => {
  objectDataSource.config.value.id = data.id;
  const booking = await objectDataSource.get();
  await bookingViewer.value.open(booking);
};
const showPayment = async () => {
  await paymentDialog.value.open(patientDataSource.model.value.id);
};
const dialogConfig = getPatientUpdateItemForm(
  patientDataSource.model.value,
  patientDataSource,
  informationSourceDataSource
);

const formService = new FormService(
  new FormServiceConfig({
    model: _.cloneDeep(dialogConfig.model),
    fields: dialogConfig.fields,
    actions: dialogConfig.actions,
  })
);

const finishBg = async () => {
  if(!bookingGroup?.value) return;

  await bookingGroupDataSource.updateField(bookingGroup.value.id, [
    new UpdateFieldModel({
      fieldName: "stateId",
      fieldValue: 6
    })
  ])
}

onMounted(async () => {
  await Promise.all([
    bookingGroupDataSource.get(),
    informationSourceDataSource.get(),
  ]);

  bookingGroup.value = bookingGroupDataSource.items.value.find(bg => bg.stateId == 2);
  if(bookingGroup?.value) {
    await patientDataSource.updateInvoices(bookingGroup.value.id);
  }
});
</script>

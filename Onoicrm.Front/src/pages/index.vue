<template>
  <div class="grid">
      <div class="categories col-12 flex justify-content-between">
        <div class="flex flex-wrap">
          <div class="card w-full sm:w-auto p-2 mb-3 md:p-3 mr-0 sm:mr-3">
            <img class="mr-2" src="/img/pasient.svg" alt="" />
            <div>
              <p class="text-sm sm:text-base text-600">Нужно лечить</p>
              <h3>450</h3>
            </div>
          </div>
          <div class="card w-full sm:w-auto p-2 mb-3 md:p-3">
            <img class="mr-2" src="/img/pasient.svg" alt="" />
            <div>
              <p class="text-sm sm:text-base text-600">Общее количество</p>
              <h3>20056</h3>
            </div>
          </div>
        </div>
        <div class="card w-full sm:w-auto p-2 mb-3 md:p-3" @click="mailingDialog?.open()">
          <img class="mr-2" src="/img/link.svg" alt="" />
          <div>
            <h3 class="text-sm sm:text-base text-600">Рассылка</h3>
          </div>
        </div>
      </div>
  </div>
  <div class="main-page-navigation mb-6">
    <div class="grid p-2 sm:p-3 w-full">
      <div class="col-12 sm:col-6 xl:col-4">
        <div class="card--pink p-3 h-7rem  relative cursor-pointer">
          <div class="flex justify-content-between flex-column h-full">
            <h2 class="text-blue-900 text-md sm:text-xl">Брекеты</h2>
            <div>
              <p class="text-xs  text-600 font-medium">
                Записаны:
              </p>
              <h4 class="text-blue-900 text-sm sm:text-base">0 клиента</h4>
              <img
                  src="/img/breket.svg"
                  alt=""
                  class="absolute symptom-service__img"
              />
            </div>
          </div>
        </div>
      </div>
      <div class="col-12 sm:col-6 xl:col-4">
        <div class="card--default-blue p-3 h-7rem  relative cursor-pointer">
          <div class="flex justify-content-between flex-column h-full">
            <h2 class="text-blue-900 text-md sm:text-xl">Ортопедия</h2>
            <div>
              <p class="text-xs  text-600 font-medium">
                Записаны:
              </p>
              <h4 class="text-blue-900 text-sm sm:text-base">0 клиента</h4>
              <img
                  src="/img/ortodantiya.svg"
                  alt=""
                  class="absolute symptom-service__img"
                  style="transform: translateY(-17px)"
              />
            </div>
          </div>
        </div>
      </div>
      <div class="col-12 sm:col-6 xl:col-4">
        <div class="card--purple p-3 h-7rem  relative cursor-pointer">
          <div class="flex justify-content-between flex-column h-full">
            <h2 class="text-blue-900 text-md sm:text-xl">Терапия</h2>
            <div>
              <p class="text-xs  text-600 font-medium">
                Записаны:
              </p>
              <h4 class="text-blue-900 text-sm sm:text-base">0 клиента</h4>
              <img
                  src="/img/terapiya.svg"
                  alt=""
                  class="absolute symptom-service__img"
              />
            </div>
          </div>
        </div>
      </div>
      <div class="col-12 sm:col-6 xl:col-4">
        <div class="card--orange p-3 h-7rem  relative cursor-pointer">
          <div class="flex justify-content-between flex-column h-full">
            <h2 class="text-blue-900 text-md sm:text-xl">Хирургия</h2>
            <div>
              <p class="text-xs  text-600 font-medium">
                Записаны:
              </p>
              <h4 class="text-blue-900 text-sm sm:text-base">0 клиента</h4>
              <img
                  src="/img/hirurgiya.svg"
                  alt=""
                  class="absolute symptom-service__img"
              />
            </div>
          </div>
        </div>
      </div>
      <div class="col-12 sm:col-6 xl:col-4">
        <div class="card--aqua p-3 h-7rem  relative cursor-pointer">
          <div class="flex justify-content-between flex-column h-full">
            <h2 class="text-blue-900 text-md sm:text-xl">
              Имплантология
            </h2>
            <div>
              <p class="text-xs  text-600 font-medium">
                Записаны:
              </p>
              <h4 class="text-blue-900 text-sm sm:text-base">0 клиента</h4>
              <img
                  src="/img/implantologiya.svg"
                  alt=""
                  class="absolute symptom-service__img"
              />
            </div>
          </div>
        </div>
      </div>
      <div class="col-12 sm:col-6 xl:col-4">
        <div class="card--green p-3 h-7rem  relative cursor-pointer">
          <div class="flex justify-content-between flex-column h-full">
            <h2 class="text-blue-900 text-md sm:text-xl">Ортодантия</h2>
            <div>
              <p class="text-xs  text-600 font-medium">
                Записаны:
              </p>
              <h4 class="text-blue-900 text-sm sm:text-base">0 клиента</h4>
              <img
                  src="/img/ortodantiya.svg"
                  alt=""
                  class="absolute symptom-service__img"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>

  <mailing-dialog ref="mailingDialog" />
</template>
<script setup lang="ts">
import {inject, onMounted, ref} from "vue";
import { useAppStore } from "~/modules/app/stores/useAppStore";
import { useUserStore } from "~/modules/user/stores/useUserStore";
import { useMeta } from "vue-meta";
import {ListDataSourceConfig} from "~/modules/data-sources/config/ListDataSourceConfig";
import {Filter} from "~/modules/data-sources/models/Filter";
import {BaseTableColumn} from "~/shared/models/base-table/BaseTableColumn";
import {useRouter} from "vue-router";
import {useBookingListDataSource} from "~/modules/booking/data-sources/useBookingListDataSource";
import {defaultMessage, IMessage} from "~/shared";
import MailingDialog from "~/modules/user-profile/components/tooth-state-mailing-dialog.vue";


const userStore = useUserStore();
const appStore = useAppStore();
const router = useRouter();
useMeta({
  title: "Главная страница",
  htmlAttrs: { lang: "en", amp: true },
});
appStore.pageTitle = `Привет ${userStore.profile.lastName}`;
appStore.breadCrumbs = [];
const message = inject<IMessage>("message", defaultMessage)
const loaded = ref<boolean>(false);
const mailingDialog = ref<MailingDialog>();
const bookingDatasource = useBookingListDataSource(
  new ListDataSourceConfig({
    className:"booking",
    orderFieldName:"DateTime",
    pageSize:10,
    filter:[
      new Filter("coming","")
    ]
  }),
)
const notifyPatient = async (id:number)=>{
    try {
      bookingDatasource.notifyPatient(id, `Уважаеммый {Patient.FullName} напоминаем что у вас сегодня в {MergedTimeSlots} запись в клинике {Clinic.Caption} у стоматолога {Doctor.FullName} по жалобу {Complaint} в кресле {Armchair.Caption}`)
      message.success('Напоминание пациента успешно отправлено!')
    }catch (e){
      message.error('При напоминание пациента возникла ошибка!')
      throw e
    }
}

const notifyDoctor = async (id:number)=>{
  try {
    bookingDatasource.notifyDoctor(id, `Уважаеммый {Doctor.FullName} напоминаем что у вас сегодня в {MergedTimeSlots} запись на пациента {Patient.FullName} по жалобу {Complaint} в кресле {Armchair.Caption}.`)
    message.success('Напоминание доктора успешно отправлено!')
  }catch (e){
    message.error('При напоминание доктора возникла ошибка!')
    throw e
  }
}
const columns = ref( [
  new BaseTableColumn({
    name: "doctor",
    caption:'Врач'
  }),
  new BaseTableColumn({
    name: "client",
    caption:'Клиент'
  }),
  new BaseTableColumn({
    name: "reminder",
    caption:'Жалоба'
  }),
  new BaseTableColumn({
    name: "numberClient",
    caption:'Номер клиента'
  }),
  new BaseTableColumn({
    name: "time",
    caption:'Время'
  }),
])

onMounted(async () => {
  await bookingDatasource.get();
  loaded.value = true;
})

</script>
<style  lang="scss">
.main-page-navigation {
  background-color: white;
  border-radius: 11px;
  &__card {
    background: #f3f8ff;
    border-radius: 10px;
    padding: 20px;
    cursor: pointer;
    text-decoration: none;
    color: inherit;
    transition: 0.3s;
  }
  &__card:hover {
    background-color: darken(#d5e2f4, $amount: 0);
  }
  &__title {
    font-weight: 600;
    font-size: 24px;
    line-height: 29px;
    /* identical to box height */

    letter-spacing: -0.01em;

    color: #7b88b5;
  }
  &__subtitle {
    font-weight: 600;
    font-size: 17px;
    line-height: 21px;
    /* identical to box height */

    letter-spacing: -0.01em;

    color: #acb5d4;
  }
}

.reminders {
  &__card {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
  }
  &__title {
    font-weight: 600;
    font-size: 17px;
    line-height: 21px;
    letter-spacing: -0.01em;
    color: #101e44;
  }
  &__name {
    font-weight: 600;
    font-size: 17px;
    line-height: 21px;
    letter-spacing: -0.01em;
    color: #101e44;
  }
  &__wand {
    color: #a6cbff;
  }

  &__number {
    font-weight: 600;
    font-size: 17px;
    line-height: 21px;
    letter-spacing: -0.01em;
    color: #101e44;
  }

  &__date {
    font-weight: 600;
    font-size: 17px;
    line-height: 21px;
    letter-spacing: -0.01em;
    color: #1479ff;
  }

  .base-table th:nth-child(1){
    font-size: 27px
  }
  .base-table th{
    padding: 0px 20px;
  }
  .base-table tbody td{
    position: relative;
  }

  .base-table thead{
    display: none;
  }
  .border-left__seperator{
    position: absolute;
    top: 50%;
    right: 0;
    transform: translateY(-50%);
    border-left: 1px #9a9afc solid;
    height: 24px
  }
}
</style>
<style scoped lang="scss">
.symptom-service__img {
  width:140px;
  bottom: -50%;
  right: 0%;
  transform: translateY(-20px);
  //   opacity: 0.95;

  @media screen and (max-width: 1200px) {
    bottom: -60%;
    right: 0%;
  }
  @media screen and (max-width: 992px) {
    bottom: -60%;
    right: -5%;
  }
  @media screen and (max-width: 768px) {
    bottom: -50%;
    right: -3%;
  }
  @media screen and (max-width: 576px) {
    width: 100px;
    bottom: -40%;
    right: -3%;
  }
}
</style>

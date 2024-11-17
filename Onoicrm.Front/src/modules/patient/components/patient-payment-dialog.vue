<template>
  <Dialog v-model:visible="visible" modal class="payment-dialog" header="Оплатить услугу" :style="{ width: '30rem' }" :breakpoints="{'575px':'30rem', '0px': '90vw' }">
    <div class="flex flex-column gap-2">
      <label class="">Введите сумму платежа</label>
      <InputText
        type="number"
        v-model="model.sum"
        class="mb-3"
        :class="{'p-invalid': validations.sum.error}"
        @input="validations.sum.validate()"
      />
    </div>
    <div class="grid mt-1">
      <div class="col-6 sm:col-4" v-for="paymentMethod in paymentMethods">
        <div class="card p-3 flex flex-column align-items-start" :class="{'active':paymentMethod.active}" @click="selectMethod(paymentMethod)">
          <img :src="paymentMethod.img" alt="">
          <h4 class="">{{paymentMethod.caption}}</h4>
        </div>
      </div>
    </div>
    <template #footer>
      <Button @click="reset" class="p-button-secondary" label="Отмена"/>
      <Button @click="submit" label="Сохранить"/>
    </template>
  </Dialog>
</template>
<script setup lang="ts">
import {ref, reactive, inject, computed, watch} from "vue";
import {Booking, BookingModel} from "~/modules/booking/entities/Booking";
import {UserRegisterModel} from "~/modules/user/models/UserResisterModel";
import {UserProfile} from "~/modules/user-profile/UserProfile";
import _ from "lodash";
import {defaultMessage, IMessage} from "~/shared";
import {Filter, IListDataSource} from "~/modules/data-sources";
import {useClinicStore} from "~/modules/clinic/stores/useClinicStore";
import axios from "axios";
import {useRoute} from "vue-router";

const message = inject<IMessage>("message", defaultMessage);

const clinicStore = useClinicStore()
const open = (patientId:number) => {
  model.patientId = patientId;
  visible.value = true
}

const openDoctor = (doctorId:number) => {
  model.profileId = doctorId;
  visible.value = true
}
const model = reactive({
  sum: null,
  method: '',
  clinicId: clinicStore.clinic?.id,
  patientId: null as null | number,
  profileId: null as null | number,
})


const selectMethod = (item:any)=>{
  paymentMethods.value.forEach(i=>{
    if (i.caption==item.caption){
      i.active=true
    }else{
      i.active = false
    }
  })
  model.method = item.caption
}
const validations = reactive({
  sum: {
    error: false,
    validate: ()=>{
      validations.sum.error = model.sum == 0;
      return !validations.sum.error
    }
  },
})
const paymentMethods = ref([
  {
    img: '/img/money.svg',
    caption: 'Наличным',
    active: false
  },{
    img: '/img/MBank.svg',
    caption: 'MBank',
    active: false
  },{
    img: '/img/elCard.svg',
    caption: 'Элкарт',
    active: false
  },{
    img: '/img/o.svg',
    caption: 'O! деньги',
    active: false
  },{
    img: '/img/balans.svg',
    caption: 'Balans.kg',
    active: false
  },{
    img: '/img/other.svg',
    caption: 'Другое',
    active: false
  },
])
const validate = () => {
  for (const validationItem of Object.values(validations)) {
    const valid = validationItem.validate()
    if (!valid)
      return false
  }

  return true
}
const submit = async () => {
  const valid = validate()
  if (!valid)
    return

  try {
    const result = await paymentDataSource.add(model);
    paymentDataSource.items.value.push(result);
    reset()
    message.success("Платёж успешно проведён")
  }catch (e :any){
    message.error(`Проверьте свои данные или Ошибка:${e.message}`)
  }
};
const reset = () => {
  visible.value = false;
  paymentMethods.value.forEach(i=>i.active=false)
  model.sum = null;
};
const visible = ref(false);
const { paymentDataSource } = defineProps<{
  paymentDataSource: IListDataSource<any>;
}>();
defineExpose({ open, openDoctor });
</script>
<style scoped lang="scss">

</style>

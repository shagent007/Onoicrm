<template>
  <div class="surface-0 flex align-items-center justify-content-center min-h-screen min-w-screen overflow-hidden">
    <div class="grid justify-content-center p-2 lg:p-0" style="min-width: 80%">
      <div class="col-12 mt-5 xl:mt-0 text-center">
        <img :src="'/images/logo-' + 'dark' + '.svg'" alt="Sakai logo" class="mb-5" style="width: 81px; height: 60px" />
      </div>
      <div
        class="col-12 xl:col-6"
        style="border-radius: 56px; padding: 0.3rem; background: linear-gradient(180deg, var(--primary-color), rgba(33, 150, 243, 0) 30%)"
      >
        <div class="h-full w-full m-0 py-7 px-4" style="border-radius: 53px; background: linear-gradient(180deg, var(--surface-50) 38.9%, var(--surface-0))">
          <div class="text-center mb-5">
            <div class="text-900 text-3xl font-medium mb-3">Добро пожаловать в систему DentTish</div>
            <span class="text-600 font-medium">Войти в систему чтобы продолжить</span>
          </div>

          <div class="w-full md:w-10 mx-auto">
            <app-form :form-service="formService">
              <template #email="{ fieldService }: any">
                <div class="mb-3">
                  <label for="email1" class="block text-900 text-xl font-medium mb-2">Логин</label>
                  <InputText
                    @input="fieldService.validate()"
                    v-model="fieldService.value"
                    id="email1"
                    type="text"
                    class="w-full"
                    :class="{ 'p-invalid': fieldService.showErrors }"
                    placeholder="Логин"
                    style="padding: 1rem"
                  />

                  <div v-if="fieldService.showErrors">
                    <div v-for="error in fieldService.errorMessages.value" :key="error">
                      <small class="p-error">{{ error }}</small>
                    </div>
                  </div>
                </div>
              </template>

              <template #password="{ fieldService }: any">
                <div>
                  <label for="password1" class="block text-900 font-medium text-xl mb-2">Пароль</label>
                  <Password
                    id="password1"
                    placeholder="Пароль"
                    :toggleMask="true"
                    :feedback="false"
                    @input="fieldService.validate()"
                    v-model="fieldService.value"
                    class="w-full"
                    :class="{ 'p-invalid': fieldService.showErrors }"
                    inputClass="w-full"
                    :inputStyle="{ padding: '1rem' }"
                  ></Password>
                  <template v-if="fieldService.showErrors">
                    <div v-for="error in fieldService.errorMessages.value" :key="error">
                      <small class="p-error">{{ error }}</small>
                    </div>
                  </template>
                </div>
              </template>
            </app-form>

            <Button :disabled="loading" :loading="loading" @click.prevent="submit" label="Войти в систему" class="w-full mt-4 p-3 text-xl"></Button>
          </div>
        </div>
      </div>
    </div>
    <Toast />
  </div>
</template>

<script lang="ts" setup>
import { reactive, ref } from 'vue';
import { useRouter } from 'vue-router';
import { StringField } from '../app-form/editors/string/models/StringField';
import { required, emailAddres, min } from '../services/consts';
import { LoginModel } from '../models/LoginModel';
import { useAuthStore } from '../store/useAuthStore';
import { LoginResponse } from '../models/LoginResponse';
import { parseErrorResponse } from '../services/helpers';
import { useToast } from 'primevue/usetoast';
import Toast from 'primevue/toast';
import AppForm from '../app-form/app-form.vue';
import { FormService } from '../app-form/services/FormService';
import { InputFieldAttribute } from '../app-form/models/InputFieldAttribute';
import { FieldConfig } from '../app-form/models/FieldConfig';
import { FormServiceConfig } from '../app-form/services/FormServiceConfig';
import { EmptyFormContainer } from '../app-form/models/FormContainer';

const authStore = useAuthStore();
const model = reactive(new LoginModel());
const router = useRouter();
const toast = useToast();
const loading = ref(false);
const formService = new FormService(
  new FormServiceConfig({
    model,
    fields: [
      new StringField('email', {
        validations: [required()],
      }),
      new StringField('password', {
        validations: [required(), min(8)],
      }),
    ],
    container: new EmptyFormContainer(),
  })
);

const submit = async () => {
  const valid = await formService.validate();
  if (!valid) return;

  try {
    loading.value = true;
    await authStore.login(model);
    loading.value = false;
    await router.push('/');
  } catch (error) {
    const response = parseErrorResponse<LoginResponse>(error);
    if (response == null) return;
    toast.add({ severity: 'error', summary: response.message, life: 3000 });
  } finally {
    loading.value = false;
  }
};
</script>

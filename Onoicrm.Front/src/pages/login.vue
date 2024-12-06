<template>
  <div class="grid">
    <div class="col-12 lg:col-7">
      <div class="login-main-box">
        <div>
          <h1 class="login-title">
            Управляйте <br />
            улыбками с легкостью
          </h1>
          <p class="login-description">
            <router-link to="">
              <span>onoicrm</span>
            </router-link>
            предоставляет возможность эффективного управления расписанием,
            моментальной связи с врачами, персонализированного обслуживания, и
            безопасного хранения медицинской информации.
          </p>
          <div class="login-home">
            <app-form :form-service="loginFormService" class="">
              <template #button>
                <Button @click="submit" class="w-full" :disabled="loading">
                  <i
                    class="pi pi-spin pi-spinner text-blue-100"
                    style="color: white"
                    v-if="loading"
                  />
                  <span class="w-full">Вход</span>
                  <svg
                    width="26"
                    height="14"
                    viewBox="0 0 26 14"
                    fill="none"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M1.5 7H25M25 7L19 1M25 7L19 13"
                      stroke="white"
                      stroke-width="2"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                    />
                  </svg>
                </Button>
              </template>
            </app-form>
          </div>
        </div>
        <div class="login-social">
          <p class="login-social__subtitle">Мы в соц. сетях</p>
          <div>
            <a target="_blank" href="https://www.instagram.com/onoicrm.dev">Instagram</a>
            <a href="#">Tik Tok</a>
            <a href="#" tooltip="не доступно">YouTube</a>
          </div>
        </div>
      </div>
    </div>
    <div class="col-12 lg:col-5 hidden lg:flex">
      <div class="login-aside-box">
        <img class="login-aside-image" src="/img/auth-back-image.png" alt="" />
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { inject, onMounted, reactive, ref } from "vue";
import { useMeta } from "vue-meta";
import { useRouter } from "vue-router";
import {
  FormService,
  InputFieldAttribute,
  StringField,
  AppForm,
  FormServiceConfig,
  PasswordField,
  FormContainer,
  CustomField,
} from "../modules/app-form";
import { min, required } from "~/shared/consts/consts";
import { LoginModel } from "~/modules/auth/models/LoginModel";
import { useAuthStore } from "~/modules/auth/stores/useAuthStore";
import { LoginResponse } from "~/modules/auth/models/LoginResponse";
import { parseErrorResponse } from "~/shared/lib/helpers";
import {
  useListDataSource,
  ListDataSourceConfig,
} from "~/modules/data-sources";
import {
  defaultMessage,
  IMessage,
} from "~/shared/models/notification/IMessage";
import { useClinicStore } from "~/modules/clinic/stores/useClinicStore";
import { Clinic } from "~/modules/clinic/Clinic";
import {useUserStore} from "~/modules/user/stores/useUserStore";


useMeta({
  title: "Вход в систему",
  htmlAttrs: { lang: "en", amp: true },
});
const useClinic = ref<boolean>(false);
const router = useRouter();
const model = reactive(
  new LoginModel({
    email: "",
    password: "",
  }),
);
const loading = ref(false);
const clinicDataSource = useListDataSource(
  new ListDataSourceConfig({
    className: "clinic",
    pageSize: 1000,
  }),
);

const clinicStore = useClinicStore();
const userStore = useUserStore();
const message = inject<IMessage>("message", defaultMessage);
const authStore = useAuthStore();
const loginFormService = new FormService(
  new FormServiceConfig({
    model,
    fields: [
      new StringField("email", {
        attrs: new InputFieldAttribute({
          cssClass: "mb-3 sm:mb-5",
          placeholder: "логин",
        }),
        validations: [required()],
      }),
      new PasswordField("password", {
        attrs: new InputFieldAttribute({
          cssClass: "mb-3",
          placeholder: "пароль",
        }),
        validations: [required(), min(6)],
        grid: "col-12 sm:col-9 mb-3",
      }),
      new CustomField("button", {
        grid: "col-12 sm:col-3 w-fit sm:w-3 mb-8",
      }),
    ],
  }),
);


const submit = async () => {
  const valid = await loginFormService.validate();
  if (!valid) return;
  try {
    loading.value = true;
    await authStore.login(model);
    await clinicStore.getClinic(userStore.profile.clinicId)
    message.success("Добро пожаловать");
    await router.push("/");
  } catch (error) {
    const response = parseErrorResponse<LoginResponse>(error);
    if (response == null) return;
    message.error(response.message);
  } finally {
    loading.value = false;
  }
};


</script>

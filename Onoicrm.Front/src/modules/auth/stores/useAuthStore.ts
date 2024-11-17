import { defineStore } from "pinia";
import { computed, ComputedRef, ref, Ref } from "vue";
import { LoginModel } from "../models/LoginModel";
import axios from "axios";
import Cookies from "js-cookie";
import { isJwtValid, tokenName } from "~/shared/lib/token";
import { useUserStore } from "~/modules/user/stores/useUserStore";
import { useClinicStore } from "~/modules/clinic/stores/useClinicStore";
import {useRouter} from "vue-router";

export const useAuthStore = defineStore("auth", () => {
  const token: Ref<string | null> = ref(null);
  const router = useRouter();
  const isAuthenticate: ComputedRef = computed(() => Boolean(token.value));
  const userStore = useUserStore();
  const clinicStore = useClinicStore();

  const login = async (loginModel: LoginModel) => {
    try {
      const { data } = await axios.post(`/api/v1/auth/login`, loginModel);
      token.value = data.token as string;
      userStore.profile = data.profile;
      userStore.profile.roles = data.roles;

      axios.defaults.headers.common["Authorization"] = `Bearer ${token.value}`;
      Cookies.set(tokenName, token.value, { expires: 7 });
      return data;
    } catch (error) {
      throw error;
    }
  };

  const logout = () => {
    token.value = null;
    Cookies.remove(tokenName);
    localStorage.clear();
    router.push("/login/auth")
  };

  const tryLogin = () => {
    const _jwtToken: string = Cookies.get(tokenName) as string;
    if (!isJwtValid(_jwtToken)) {
      token.value = null;
      Cookies.remove(tokenName);
      localStorage.clear();
      return false;
    }

    token.value = _jwtToken;
    axios.defaults.headers.common["Authorization"] = `Bearer ${token.value}`;
    Cookies.set(tokenName, token.value, { expires: 7 });
    return true;
  };

  return { isAuthenticate, login, tryLogin, logout };
});

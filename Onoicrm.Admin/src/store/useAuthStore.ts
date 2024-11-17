import { defineStore } from 'pinia';
import { computed, ComputedRef, ref, Ref } from 'vue';
import { LoginModel } from '../models/LoginModel';
import axios from 'axios';
import Cookies from 'js-cookie';
import { isJwtValid, tokenName } from '../services/token';
import { useRouter } from 'vue-router';

export const useAuthStore = defineStore('auth', () => {
  const router = useRouter();

  const token: Ref<string | null> = ref(null);
  const isAuthenticate: ComputedRef = computed(() => token !== null);

  const login = async (loginModel: LoginModel) => {
    try {
      const { data } = await axios.post(`/api/v1/auth/login`, loginModel);
      token.value = data.token as string;
      axios.defaults.headers.common['Authorization'] = `Bearer ${token.value}`;
      Cookies.set(tokenName, token.value, { expires: 7 });
    } catch (error) {
      throw error;
    }
  };

  const tryLogin = () => {
    const _jwtToken: string  = Cookies.get(tokenName) as string;
    if (!isJwtValid(_jwtToken)) {
      token.value = null;
      return false;
    }

    token.value = _jwtToken;
    axios.defaults.headers.common['Authorization'] = `Bearer ${token.value}`;
    Cookies.set(tokenName, token.value, { expires: 7 });
    return true;
  };

  const logout = () => {
    token.value = null;
    Cookies.remove(tokenName);
    router.push("/auth/login")
  };

  return { isAuthenticate, login, tryLogin,logout };
});

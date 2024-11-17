import {computed, ref, Ref} from "vue";
import { defineStore } from "pinia";
import _ from "lodash"
import axios from "axios";
import {checkDuplicateAuthError} from "~/modules/data-sources/checkDuplicateAuthError";
export const useUserStore = defineStore("user", () => {
  const profile: Ref = ref(null);
  const get = async () => {
    try {
      if(profile.value != null) return profile.value;
      const { data } = await axios.get("/api/v1/security/account");
      profile.value = data;
      return data;
    } catch (e) {
      await checkDuplicateAuthError(e);

    }

  };

  const roleCaption = computed(() => {
    const roles = profile.value?.roles
    if(!roles) return ""
    if (roles.includes('System Administrator')){
      return 'Системный администратор'
    }

    if(roles.includes('Director')){
      return 'Директор'
    }

    if(roles.includes('Administrator')){
      return 'Администратор'
    }

    if(roles.includes('Dentist')){
      return 'Врач'
    }
  })

  const hasOptionalRoles = (roleNames:string[]) => {
    const roles = profile.value?.roles ?? [];
    return _.intersection(roles, roleNames).length > 0;
  }

  const hasRequiredRoles = (roleNames:string[]) => {
    const roles = profile.value?.roles ?? [];
    return _.intersection(roles, roleNames).length === roleNames.length;
  }

  return { get, profile,hasOptionalRoles, hasRequiredRoles,roleCaption };
});

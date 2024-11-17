import {computed, ComputedRef} from "vue";
import {SetRoleModel} from "~/modules/data-sources/hooks/setRoleModel";
import axios from "axios";

export const useUserRoleDataSource = () => {
  const baseUrl: ComputedRef<string> = computed(() => `/api/v1/security/userRole`);
  const setRole = async (model:SetRoleModel) => {
    const { data } = await axios.post(`${baseUrl.value}/set-role`, model);
    return data;
  }

  const deleteRole = async (model:SetRoleModel) => {
    const { data } = await axios.post(`${baseUrl.value}/delete-role`, model);
    return data;
  }

  return {
    setRole,
    deleteRole
  };
};

import {useAuthStore} from "~/modules/auth/stores/useAuthStore";
export const checkDuplicateAuthError =async (responseError:any) => {
  const authStore = useAuthStore();
  const response = (responseError as any).response
  if(!response) return;
  if(response.status == 403 && response.data == "AuthDuplicateError"){
    authStore.logout();
  }
}

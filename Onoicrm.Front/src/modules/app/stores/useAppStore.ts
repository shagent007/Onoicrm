import { ref, Ref } from "vue";
import { defineStore } from "pinia";
import { MenuItem } from "primevue/menuitem";
import { ButtonModel } from "~/modules/app/models/ButtonModel";
export const useAppStore = defineStore("app", () => {
  const pageTitle: Ref<string> = ref("");
  const pageTitleCssClass: Ref<string> = ref<string>("text-2xl sm:text-6xl");
  const pageSubtitle: Ref<string> = ref("");
  const breadCrumbs: Ref<MenuItem[]> = ref([]);
  const buttons: Ref<ButtonModel[]> = ref([]);
  const searchText:Ref<string> = ref('')
  const drawer = ref<boolean>(false)
  return { pageTitle, pageSubtitle, breadCrumbs, pageTitleCssClass, buttons, searchText, drawer };
});

import { computed, shallowRef, ShallowRef } from "vue";
import EmptyContainer from "../containers/empty.vue";
import DefaultContainer from "../containers/default.vue";
import GridContainer from "../containers/grid.vue";
import { FormService } from "../services/FormService";

export const useContainerComponents = (form: FormService) => {
  const container = computed((): ShallowRef => {
    switch (form.container.type) {
      case "default-container":
        return shallowRef(DefaultContainer);
      case "empty-container":
        return shallowRef(EmptyContainer);
      case "grid-container":
        return shallowRef(GridContainer);
      default:
        throw new Error(`Контейнер с названием ${form.container} не найдено`);
    }
  });

  return { container };
};
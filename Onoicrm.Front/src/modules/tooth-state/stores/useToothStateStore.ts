import { onMounted, ref } from "vue";
import { defineStore } from "pinia";
import {
  IListDataSource,
  ListDataSourceConfig,
  useListDataSource,
} from "~/modules/data-sources";
import { IToothStateStore } from "~/modules/tooth-state/interfaces/IToothStateStore";

export const useToothStateStore = defineStore(
  "toothState",
  (): IToothStateStore<any> => {
    const loaded = ref<boolean>(false);
    const dataSource = useListDataSource(
      new ListDataSourceConfig({ className: "ToothState" }),
    );
    if (!loaded.value) {
      onMounted(async () => {
        await dataSource.get();
        loaded.value = true;
      });
    }

    return { ...dataSource, loaded };
  },
);

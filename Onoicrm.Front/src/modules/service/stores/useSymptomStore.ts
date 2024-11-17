import {Filter, IListDataSource, ListDataSourceConfig, useListDataSource} from "~/modules/data-sources";
import {defineStore} from "pinia";
import {onMounted, ref} from "vue";
import {useClinicStore} from "~/modules/clinic/stores/useClinicStore";
import {IListDataSourceStore} from "~/modules/data-sources/interfaces/IListDataSourceStore";

export const useServiceStore = (includeChildren:boolean = false) => defineStore("service",():IListDataSourceStore => {
  const loaded = ref<boolean>(false);
  const clinicStore = useClinicStore();

  const dataSource = useListDataSource(
    new ListDataSourceConfig({
      className:"service",
      pageSize:500,
    })
  )

  if(includeChildren){
    onMounted(async ()=>{
      dataSource.filter.value = [
        new Filter("clinicId",clinicStore.clinic.value?.id),
        new Filter("withChildren", true)
      ]
      await dataSource.get();
      loaded.value = true;
    })
  } else if(!loaded.value){
    onMounted(async ()=>{
      await dataSource.get();
      loaded.value = true;
    })
  }

  return {...dataSource, loaded}
})();

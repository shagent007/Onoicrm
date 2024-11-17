import {Filter, IListDataSource, ListDataSourceConfig, useListDataSource} from "../../data-sources";
import {defineStore} from "pinia";
import {useClinicStore} from "~/modules/clinic/stores/useClinicStore";
import {onMounted, ref} from "vue";
import {IListDataSourceStore} from "~/modules/data-sources/interfaces/IListDataSourceStore";
export const useArmchairStore = defineStore("armchair",():IListDataSourceStore => {
  const loaded = ref<boolean>(false);
  const clinicStore = useClinicStore();

  const dataSource = useListDataSource(
    new ListDataSourceConfig({
      className:"armchair",
      filter:[
        new Filter("clinicId",clinicStore.clinic?.id)
      ]
    })
  )


  if(!loaded.value){
    onMounted(async ()=>{
    //  await dataSource.get();
      loaded.value = true;
    })
  }

  return {...dataSource,loaded}
})

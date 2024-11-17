import {Filter, IListDataSource, ListDataSourceConfig, useListDataSource} from "../../data-sources";
import {defineStore} from "pinia";
import {onMounted, ref} from "vue";
import {useClinicStore} from "~/modules/clinic/stores/useClinicStore";

export const useSymptomStore = defineStore("symptom",():IListDataSource<any> => {
  const loaded = ref<boolean>();
  const clinicStore = useClinicStore();

  const dataSource = useListDataSource(
    new ListDataSourceConfig({
      className:"symptom",
      pageSize:500,
      filter:[
        new Filter("clinicId",clinicStore.clinic?.id)
      ]
    })
  )

  if(!loaded.value){
    onMounted(async ()=>{
      await dataSource.get();
      loaded.value = true;
    })
  }

  return {...dataSource}
})

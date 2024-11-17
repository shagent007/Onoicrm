import {defineStore} from "pinia";
import {IListDataSourceStore} from "~/modules/data-sources/interfaces/IListDataSourceStore";
import {ListDataSourceConfig, useListDataSource} from "~/modules/data-sources";
import {onMounted, ref} from "vue";

export const useCancellationReasonStore = defineStore("cancellationReason",()=>{
  const loaded = ref<boolean>(false);
  const dataSource = useListDataSource(
    new ListDataSourceConfig({
      className:"CancellationReason",
      pageSize:100
    })
  )

  const getData = async () => {
    if(!loaded.value){
      await dataSource.get();
      loaded.value = true;
    }
  }



  return {...dataSource, getData,loaded}
})

import {IListDataSource, ListDataSourceConfig, useListDataSource} from "../../data-sources";
import {defineStore} from "pinia";
import {onMounted, ref} from "vue";
import {IListDataSourceStore} from "~/modules/data-sources/interfaces/IListDataSourceStore";

export const useToothStore = defineStore("tooth",() => {
  const loaded = ref<boolean>(false);
  const dataSource = useListDataSource(
    new ListDataSourceConfig({
      className:"tooth",
      pageSize:100
    })
  )

  const getData = async () => {
    if(!loaded.value){
      await dataSource.get();
      loaded.value = true;
    }
  }


  return {...dataSource,getData,loaded}
})

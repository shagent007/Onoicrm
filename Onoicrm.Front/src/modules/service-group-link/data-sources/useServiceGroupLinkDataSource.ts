import {IServiceGroupLinkDataSource} from "~/modules/service-group-link/interfaces/IServiceGroupLinkDataSource";
import {ListDataSourceConfig, useListDataSource} from "~/modules/data-sources";
import axios from "axios";

export const useServiceGroupLinkDataSource = (config:Partial<ListDataSourceConfig>=new ListDataSourceConfig()):IServiceGroupLinkDataSource => {
  const dataSource = useListDataSource(
    new ListDataSourceConfig({
      ...config,
      className:"ServiceGroupLink"
    })
  );

  const cascadeAdd = async (items:any[]) => {
    try {
      const {data} = await axios.post(`/api/v1/public/ServiceGroupLink/range`, items);
      return data
    } catch (e) {
      throw e;
    }
  }

  const cascadeRemove = async (items:any[]) => {
    try {
      await axios.delete(`/api/v1/public/ServiceGroupLink/range`, { data: items });
    } catch (e) {
      throw e;
    }
  }


  return {
    ...dataSource,
    cascadeAdd,
    cascadeRemove
  }
}

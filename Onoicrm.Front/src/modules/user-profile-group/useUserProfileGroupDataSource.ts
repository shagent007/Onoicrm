import { useListDataSource } from '~/modules/data-sources';
import { ListDataSourceConfig } from '~/modules/data-sources';
import axios from 'axios';

export const useUserProfileGroupDataSource = () =>{
  const dataSource = useListDataSource(new ListDataSourceConfig({className:"userProfileGroup"}));

  const cascadeAdd = async (items:any[]) => {
    try {
      const {data} = await axios.post(`/api/v1/public/userProfileGroup/range`, items);
      return data
    } catch (e) {
      throw e;
    }
  }

  const cascadeRemove = async (items:any[]) => {
    try {
      await axios.delete(`/api/v1/public/userProfileGroup/range`, { data: items });
    } catch (e) {
      throw e;
    }
  }

  return {...dataSource, cascadeAdd, cascadeRemove}
}

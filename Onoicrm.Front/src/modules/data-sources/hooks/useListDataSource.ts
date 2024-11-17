import axios from 'axios';
import {computed, ComputedRef, ref, Ref} from 'vue';
import {Filter} from '../models/Filter';
import {toPascalCase, toQueryString} from '~/shared/lib/helpers';
import {ListDataSourceConfig, ListDataSourceResponse, UpdateFieldModel} from '~/modules/data-sources';
import {IListDataSource} from '~/modules/data-sources';
import {checkDuplicateAuthError} from "~/modules/data-sources/checkDuplicateAuthError";
import {DataTableSortEvent} from "primevue/datatable";

export const useListDataSource = (config: ListDataSourceConfig): IListDataSource<any> => {
  const items: Ref<Array<any>> = ref([]);
  const filter: Ref<Array<Filter>> = ref(config.filter);
  const pageIndex: Ref<number> = ref(config.pageIndex);
  const pageSize: Ref<number> = ref(config.pageSize);
  const orderFieldName: Ref<string> = ref(config.orderFieldName);
  const orderFieldDirection: Ref<string> = ref(config.orderFieldDirection);
  const loading: Ref<boolean> = ref(false);
  const total: Ref<number> = ref(0);
  const fields = ref(config.fields);
  const loaded = ref(false);

  const baseUrl: ComputedRef<string> = computed(() => `/api/v1/public/${config.className}`);
  const queryParams: ComputedRef<string> = computed((): string => {
    let queryParams = '/';
    const queryObject = {
      pageIndex: pageIndex.value,
      pageSize: pageSize.value,
      orderFieldName: orderFieldName.value,
      orderFieldDirection: orderFieldDirection.value,
      fields:fields.value
    };

    queryParams = toQueryString(queryObject);
    if (filter.value.length > 0) {
      queryParams += `&filter=${JSON.stringify(filter.value)}`;
    }

    return queryParams;
  });
  const activeItems = computed(()=>items.value.filter(i => i.stateId == 2))

  const findItem = (id:number) => {
    return items.value.find((i:any) => i.id == id);
  }

  const updateFilter = async (newFilters:Filter[], refreshData:boolean=true)=> {
    filter.value = filter.value.map(filter => {
      const defined = newFilters.find(newFilter => newFilter.name === filter.name);
      return defined ? defined : filter;
    });

    newFilters.forEach(newFilter => {
      if (!filter.value.some(f=> f.name === newFilter.name)) {
        filter.value.push(newFilter);
      }
    });
    if(refreshData){
      await get();
    }
  }
  const removeFilter = async (filters:string[], refreshData:boolean=true) => {
    filter.value = filter.value.filter(f => !filters.includes(f.name))
    if(refreshData){
      await get();
    }
  }
  const paginate = async (event: any) => {
    pageIndex.value = event.page + 1;
    pageSize.value = event.rows;
    await get();
  };
  const sort = async (event: DataTableSortEvent) => {
    orderFieldName.value = toPascalCase(event.sortField as string);
    orderFieldDirection.value = event.sortOrder == 1 ? 'ASC' : 'DESC';
    await get();
  };
  const get = async (): Promise<ListDataSourceResponse<any>> => {
    try {
      loading.value = true;

      const { data } = await axios.get(baseUrl.value + queryParams.value);
      items.value = data.items;
      total.value = data.total;
      loaded.value= true;
      return data;
    } catch (error) {
     await checkDuplicateAuthError(error);
      throw error;
    } finally {
      loading.value = false;
    }
  };
  const updatePriorities = async (priorityModels:any[] = []):Promise<any> => {
    try {
      loading.value = true;
      const updateItems = priorityModels.length == 0
        ? items.value.map((v,i) => ({id:v.id, priority:i}))
        : priorityModels;

      const { data } = await axios.patch(`${baseUrl.value}/priority/`, updateItems);
      return data;
    } catch (error) {
      await checkDuplicateAuthError(error);
      throw error;
    } finally {
      loading.value = false;
    }
  }
  const getById = async (id:number): Promise<any> => {
    try {
      loading.value = true;
      const { data } = await axios.get(`${baseUrl.value}/${id}`);
      return data;
    } catch (error) {
      await checkDuplicateAuthError(error);
      throw error;
    } finally {
      loading.value = false;
    }
  };
  const add = async (model: any): Promise<any> => {
    try {
      loading.value = true;
      delete model.id;
      const { data }: { data: any } = await axios.post(baseUrl.value, model);
      return data;
    } catch (error) {
      await checkDuplicateAuthError(error);
      throw error;
    } finally {
      loading.value = false;
    }
  };
  const addRange = async (items:any[]): Promise<any> => {
    try {
      loading.value = true;
      const {data} = await axios.post(`${baseUrl.value}/range/`, items);
      return  data
    } catch (error) {
      await checkDuplicateAuthError(error);
      throw error;
    } finally {
      loading.value = false;
    }
  };
  const remove = async (id: number): Promise<void> => {
    try {
      loading.value = true;
      await axios.delete(`${baseUrl.value}/${id}`);
      const index = items.value.findIndex((i: any) => i.id == id);
      if (index !== -1) {
        items.value.splice(index, 1);
      }
    } catch (error) {
      await checkDuplicateAuthError(error);
      throw error;
    } finally {
      loading.value = false;
    }
  };
  const removeRange = async (items:any[]) => {
    try {
      await axios.delete(`${baseUrl.value}/range/`, { data: items });
    } catch (e) {
      throw e;
    }
  }
  const update = async (model: any): Promise<void> => {
    const item = items.value.find((i: any) => i.id == model.id);
    if (item == undefined) throw new Error('Элемент не найден');
    try {
      loading.value = true;
      const { data } = await axios.put(`${baseUrl.value}/${model.id}/`, model);
      Object.assign(item, data);
    } catch (error) {
      await checkDuplicateAuthError(error);

      throw error;
    } finally {
      loading.value = false;
    }
  };
  const updateField = async (id: number, updateFields: UpdateFieldModel[]): Promise<any> => {
    const item = items.value.find((i: any) => i.id == id);
    if (item == undefined) throw new Error('Элемент не найден');
    try {
      loading.value = true;
      const { data } = await axios.patch(`${baseUrl.value}/${id}`, updateFields);
      Object.assign(item, data);
      return data;
    } catch (error) {
      await checkDuplicateAuthError(error);

      throw error;
    } finally {
      loading.value = false;
    }
  };

  return {
    items,
    pageIndex,
    pageSize,
    orderFieldName,
    orderFieldDirection,
    loading,
    total,
    loaded,
    filter,
    activeItems,
    paginate,
    sort,
    updatePriorities,
    getById,
    updateFilter,
    get,
    findItem: findItem,
    add,
    remove,
    update,
    updateField,
    addRange,
    removeFilter,
    removeRange
  };
};



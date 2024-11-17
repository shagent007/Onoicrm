import axios from 'axios';
import { computed, ComputedRef, onMounted, ref, Ref } from 'vue';
import { Entity } from '../entities/Entity';
import { Filter } from '../models/Filter';
import { toQueryString } from '../services/helpers';
import { ListResponse } from '../models/ListResponse';
import { ListConfig } from '../models/ListConfig';
import { IListDataSource } from '../models/IListDataSource';
import { UpdateFieldModel } from '../models/UpdateFieldModel';

export const useListDataSource = <TEntity extends Entity>(config: ListConfig): IListDataSource<TEntity> => {
  const items: Ref<Array<TEntity>> = ref([]);
  const filter: Ref<Array<Filter>> = ref(config.filter);
  const pageIndex: Ref<number> = ref(config.pageIndex);
  const pageSize: Ref<number> = ref(config.pageSize);
  const orderFieldName: Ref<string> = ref(config.orderFieldName);
  const orderFieldDirection: Ref<string> = ref(config.orderFieldDirection);
  const loading: Ref<boolean> = ref(false);
  const total: Ref<number> = ref(0);
  const baseUrl: ComputedRef<string> = computed(() => `/api/v1/admin/${config.className}`);

  const queryParams: ComputedRef<string> = computed((): string => {
    let queryParams = '/';
    const queryObject = {
      pageIndex: pageIndex.value,
      pageSize: pageSize.value,
      orderFieldName: orderFieldName.value,
      orderFieldDirection: orderFieldDirection.value,
    };

    queryParams = toQueryString(queryObject);
    if (filter.value.length > 0) {
      queryParams += `&filter=${JSON.stringify(filter.value)}`;
    }

    return queryParams;
  });

  const get = async (): Promise<ListResponse<TEntity>> => {
    try {
      loading.value = true;
      const { data }: { data: ListResponse<TEntity> } = await axios.get(baseUrl.value + queryParams.value);
      items.value = data.items;
      total.value = data.total;

      return data;
    } catch (error) {
      throw error;
    } finally {
      loading.value = false;
    }
  };

  const add = async (model: TEntity): Promise<TEntity> => {
    try {
      loading.value = true;
      delete model.id;
      const { data }: { data: TEntity } = await axios.post(baseUrl.value, model);
      return data;
    } catch (error) {
      throw error;
    } finally {
      loading.value = false;
    }
  };

  const remove = async (id: number): Promise<void> => {
    try {
      loading.value = true;
      await axios.delete(`${baseUrl.value}/${id}`);
      const index = items.value.findIndex((i: TEntity) => i.id == id);
      if (index !== -1) {
        items.value.splice(index, 1);
      }
    } catch (error) {
      throw error;
    } finally {
      loading.value = false;
    }
  };

  const update = async (model: TEntity): Promise<void> => {
    const item = items.value.find((i: TEntity) => i.id == model.id);
    if (item == undefined) throw new Error('Элемент не найден');
    try {
      loading.value = true;
      const { data } = await axios.put(baseUrl.value, model);
      Object.assign(item, data);
    } catch (error) {
      throw error;
    } finally {
      loading.value = false;
    }
  };

  const updateField = async (id: number, updateFields: UpdateFieldModel[]): Promise<TEntity> => {
    const item:any = items.value.find((i: TEntity) => i.id == id);
    if (item == undefined) throw new Error('Элемент не найден');
    try {
      loading.value = true;
      const { data } = await axios.patch(`${baseUrl.value}/${id}`, updateFields);
      for (const updateField of updateFields) {
        item[updateField.fieldName] = updateField.fieldValue
      }
      return data;
    } catch (error) {
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
    filter,
    get,
    add,
    remove,
    update,
    updateField,
  };
};


export class SetRoleModel {
  public roleName!:string;
  public userId!:string;

  constructor(init?:Partial<SetRoleModel>) {
    Object.assign(this, init)
  }
}

export const useUserRoleDataSource = () => {
  const baseUrl: ComputedRef<string> = computed(() => `/api/v1/admin/userRole`);
  const setRole = async (model:SetRoleModel) => {
    const { data } = await axios.post(`${baseUrl.value}/set-role`, model);
    return data;
  }

  const deleteRole = async (model:SetRoleModel) => {
    const { data } = await axios.post(`${baseUrl.value}/delete-role`, model);
    return data;
  }

  return {
    setRole,
    deleteRole
  };
};

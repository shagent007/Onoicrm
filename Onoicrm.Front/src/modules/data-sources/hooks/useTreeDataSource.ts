import axios from 'axios';
import { ITreeDataSource } from '~/modules/data-sources';
import { computed, ComputedRef, ref, Ref } from 'vue';
import { Filter } from '../models/Filter';
import { UpdateFieldModel } from '~/modules/data-sources';
import {deepTransformTree, sortTree} from '~/shared/lib/helpers';
import { TransformInfo } from '~/shared';
import { checkDuplicateAuthError } from "~/modules/data-sources/checkDuplicateAuthError";

export class TreeDataSourceConfig {
    public className!:string;
    public root!:string;
    public filter:Filter[] = []

    constructor(init?:Partial<TreeDataSourceConfig>) {
        Object.assign(this,init)
    }
}

export const useTreeDataSource = (config:TreeDataSourceConfig): ITreeDataSource => {
  const filter: Ref<Array<Filter>> = ref([]);
  const loading: Ref<boolean> = ref(false);
  const root: Ref = ref([]);
  const loaded = ref(false);
  const transformedRoot:Ref = computed(()=>{
    const sortedRoot = sortTree(root.value)
    return deepTransformTree(sortedRoot,[
      new TransformInfo("id","key"),
      new TransformInfo("caption","label"),
    ],new TransformInfo("children","children"));
  });

  const total: Ref<number> = ref(0);
  const baseUrl: ComputedRef<string> = computed(() => `/api/v1/public/${config.className}`);

  const get = async (rootElName: string): Promise<any> => {
    try {
      const { data } = await axios.get(`${baseUrl.value}/${rootElName}/tree`);
      root.value= [data]
      return data;
    } catch (e) {
      await checkDuplicateAuthError(e);// this is the main part. Use the response property from the error object

      return e;
    }
  }

  const getRoot = async (clinicId: number): Promise<any> => {
    if(loaded.value) return root.value;
    try {
      const { data } = await axios.get(`${baseUrl.value}/${clinicId}/root`);
      if(!data) return;
      root.value= [data]
      loaded.value = true;
      return data;
    } catch (e) {
      await checkDuplicateAuthError(e);
      throw e;
    }
  }

  const add = async (model: any): Promise<any> => {
    try {
      loading.value = true;
      delete model.id;
      const { data } = await axios.post(baseUrl.value, model);
      return data;
    } catch (error) {
      await checkDuplicateAuthError(error);
      throw error;
    } finally {
      loading.value = false;
    }
  };

  const mailing = async (id:number,model:any) :Promise<any> => {
    try {
      loading.value = true;
      const { data } = await axios.post(`${baseUrl.value}/${id}/mailing`, model);
      return data;
    } catch (error) {
      await checkDuplicateAuthError(error);
      throw error;
    } finally {
      loading.value = false;
    }
  }

  const remove = async (id: number): Promise<void> => {
    try {
      loading.value = true;
      await axios.delete(`${baseUrl.value}/${id}`);
    } catch (error) {
      await checkDuplicateAuthError(error);
      throw error;
    } finally {
      loading.value = false;
    }
  };

  const update = async (model: any): Promise<void> => {
    try {
      loading.value = true;
      delete model.children
      const { data } = await axios.put(`${baseUrl.value}/${model.id}`, model);
    } catch (error) {
      await checkDuplicateAuthError(error);
      throw error;
    } finally {
      loading.value = false;
    }
  };

  const updateField = async (id: number, updateFields: UpdateFieldModel[]): Promise<any> => {
    try {
      loading.value = true;
      const { data } = await axios.patch(`${baseUrl.value}/${id}`, updateFields);
      return data;
    } catch (error) {
      await checkDuplicateAuthError(error);
      throw error;
    } finally {
      loading.value = false;
    }
  };

  return {
    filter,
    loading,
    root,
    transformedRoot,
    total,
    add,
    mailing,
    get,
    getRoot,
    remove,
    update,
    updateField
  }
}

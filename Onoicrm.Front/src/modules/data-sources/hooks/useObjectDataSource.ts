import axios from "axios";
import { computed, ComputedRef, ref, Ref } from "vue";
import { Entity } from "~/shared/entities/Entity";
import { ObjectDataSourceConfig } from "~/modules/data-sources";
import { UpdateFieldModel } from "~/modules/data-sources";
import { IObjectDataSource } from "~/modules/data-sources";
import {checkDuplicateAuthError} from "~/modules/data-sources/checkDuplicateAuthError";

export const useObjectDataSource = <TEntity extends Entity = any>(
  conf: ObjectDataSourceConfig,
): IObjectDataSource => {
  const loading: Ref<boolean> = ref(false);
  const config = ref<ObjectDataSourceConfig>(conf);
  const baseUrl: ComputedRef<string> = computed(
    () => `/api/v1/public/${config.value.className}/${config.value?.id}`,
  );
  const model: Ref<TEntity | null> = ref(null);

  const get = async (): Promise<TEntity> => {
    try {
      loading.value = true;
      const { data }: { data: TEntity } = await axios.get(baseUrl.value);
      model.value = data;
      return data;
    } catch (error) {
      await checkDuplicateAuthError(error);
      throw error;
    } finally {
      loading.value = false;
    }
  };

  const update = async (newModel: TEntity): Promise<void> => {
    try {
      loading.value = true;
      const { data }: { data: TEntity } = await axios.put(
        baseUrl.value,
        newModel,
      );
      Object.assign(model, data);
    } catch (error) {
      await checkDuplicateAuthError(error);

      throw error;
    } finally {

      loading.value = false;
    }
  };

  const updateField = async (
    id: number,
    updateFields: UpdateFieldModel[],
  ): Promise<TEntity> => {
    try {
      loading.value = true;
      const { data } = await axios.patch(baseUrl.value, updateFields);
      Object.assign(model, data);

      return data;
    } catch (error) {
      await checkDuplicateAuthError(error);

      throw error;
    } finally {
      loading.value = false;
    }
  };

  return {
    loading,
    model,
    config,
    get,
    update,
    updateField,
  };
};

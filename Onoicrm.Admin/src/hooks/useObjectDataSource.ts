import axios from 'axios';
import { computed, ComputedRef, ref, Ref } from 'vue';
import { Entity } from '../entities/Entity';
import { ObjectDataSourceConfig } from '../models/ObjectDataSourceConfig';
import { UpdateFieldModel } from '../models/UpdateFieldModel';
import { IObjectDataSource } from '../models/IObjectDataSource';

export const useObjectDataSource = <TEntity extends Entity>(config: ObjectDataSourceConfig): IObjectDataSource<TEntity> => {
    const loading: Ref<boolean> = ref(false);
    const baseUrl: ComputedRef<string> = computed(() => `/api/v1/admin/${config.className}/${config.id}`);
    const model: Ref<TEntity | null> = ref(null);

    const get = async (): Promise<TEntity> => {
        try {
            loading.value = true;
            const { data }: { data: TEntity } = await axios.get(baseUrl.value);
            model.value = data;
            return data;
        } catch (error) {
            throw error;
        } finally {
            loading.value = false;
        }
    };

    const update = async (newModel: TEntity): Promise<void> => {
        try {
            loading.value = true;
            const { data }: { data: TEntity } = await axios.put(baseUrl.value, newModel);
            Object.assign(model, data);
        } catch (error) {
            throw error;
        } finally {
            loading.value = false;
        }
    };

    const updateField = async (id: number, updateFields: UpdateFieldModel[]): Promise<TEntity> => {
        try {
            loading.value = true;
            const { data } = await axios.patch(baseUrl.value, updateFields);
            Object.assign(model, data);

            return data;
        } catch (error) {
            throw error;
        } finally {
            loading.value = false;
        }
    };

    return {
        loading,
        model,
        get,
        update,
        updateField,
    };
};

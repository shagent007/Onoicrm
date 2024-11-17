import { Ref } from 'vue';
import { IUpdateFieldService } from '../app-form/services/IUpdateFieldService';
import { Entity } from '../entities/Entity';

export interface IObjectDataSource<TEntity extends Entity> extends IUpdateFieldService {
    model: Ref<TEntity | null>;
    loading: Ref<boolean>;
    get(): Promise<TEntity>;
    update(model: TEntity): Promise<void>;
}

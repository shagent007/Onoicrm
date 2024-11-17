import { Ref } from 'vue';
import { IUpdateFieldService } from '../app-form/services/IUpdateFieldService';
import { Entity } from '../entities/Entity';
import { Filter } from './Filter';
import { ListResponse } from './ListResponse';

export interface IListDataSource<TEntity extends Entity> extends IUpdateFieldService {
    items: Ref<TEntity[]>;
    pageIndex: Ref<number>;
    pageSize: Ref<number>;
    orderFieldName: Ref<string>;
    orderFieldDirection: Ref<string>;
    loading: Ref<boolean>;
    total: Ref<number>;
    filter: Ref<Array<Filter>>;
    get(): Promise<ListResponse<TEntity>>;
    add(model: TEntity): Promise<TEntity>;
    remove(id: number): Promise<void>;
    update(model: TEntity): Promise<void>;
}



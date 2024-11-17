import { IUpdateFieldService } from '../app-form/services/IUpdateFieldService';
import { Ref } from 'vue';
import { Filter } from './Filter';
import { ListResponse } from './ListResponse';

export interface ITreeDataSource extends IUpdateFieldService {
    root: Ref;
    transformedRoot:Ref;
    loading: Ref<boolean>;
    total: Ref<number>;
    filter: Ref<Array<Filter>>;
    get(rootElName: string): Promise<any>;
    add(model: any): Promise<any>;
    remove(id: number): Promise<void>;
    update(model: any): Promise<void>;
}
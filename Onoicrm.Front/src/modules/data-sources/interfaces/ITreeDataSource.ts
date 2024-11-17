import { IUpdateFieldService } from '../../app-form/services/IUpdateFieldService';
import { Ref } from 'vue';
import { Filter } from '../models/Filter';

export interface ITreeDataSource extends IUpdateFieldService {
    root: Ref;
    transformedRoot:Ref;
    loading: Ref<boolean>;
    total: Ref<number>;
    filter: Ref<Array<Filter>>;
    get(rootElName: string): Promise<any>;
    getRoot(clinicId:number):Promise<any>
    add(model: any): Promise<any>;
    remove(id: number): Promise<void>;
    update(model: any): Promise<void>;
    mailing(id:number,model:any): Promise<any>
}

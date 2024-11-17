import {ComputedRef, Ref} from 'vue';
import {IUpdateFieldService} from '../../app-form/services/IUpdateFieldService';
import {Entity} from '~/shared/entities/Entity';
import {Filter} from '../models/Filter';
import {ListDataSourceResponse} from '~/modules/data-sources';
import {DataTableSortEvent} from "primevue/datatable";

export interface IListDataSource<TEntity extends Entity> extends IUpdateFieldService {
    items: Ref<any[]>;
    pageIndex: Ref<number>;
    pageSize: Ref<number>;
    orderFieldName: Ref<string>;
    orderFieldDirection: Ref<string>;
    loading: Ref<boolean> | boolean;
    loaded: Ref<boolean>;
    total: Ref<number>;
    filter: Ref<Array<Filter>>;
    activeItems:ComputedRef<any[]>;
    paginate(event: any): Promise<void>;
    sort(event: DataTableSortEvent):Promise<void>;
    get(): Promise<any>;
    findItem(id:number):any;
    getById(id:number):any;
    add(model: any): Promise<TEntity>;
    remove(id: number): Promise<void>;
    update(model: TEntity): Promise<void>;
    updatePriorities(items?:any[] ):Promise<any>;
    addRange(items:any[]):Promise<any>;
    removeRange(items:any[]): Promise<void>;
    updateFilter(filter:Filter[],refreshData?:boolean):any;
    removeFilter(filters:string[],refreshData?:boolean):any;
}


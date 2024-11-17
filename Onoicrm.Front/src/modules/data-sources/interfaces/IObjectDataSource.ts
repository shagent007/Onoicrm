import { Ref } from 'vue';
import { IUpdateFieldService } from '../../app-form/services/IUpdateFieldService';
import {ObjectDataSourceConfig} from "~/modules/data-sources";

declare  interface IObjectDataSource extends IUpdateFieldService {
    model: Ref<any | null>;
    loading: Ref<boolean>;
    config:Ref<ObjectDataSourceConfig>;
    get(): Promise<any>;
    update(model: any): Promise<void>;
}

export {IObjectDataSource}

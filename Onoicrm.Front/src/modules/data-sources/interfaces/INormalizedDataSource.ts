import {IUpdateFieldService} from "~/modules/app-form/services/IUpdateFieldService";
import {WritableComputedRef} from "vue";
import {Filter, ListDataSourceResponse} from "~/modules/data-sources";

export interface INormalizedDataSource extends IUpdateFieldService {
  items: WritableComputedRef<any[]>;
  pageIndex: WritableComputedRef<number>;
  pageSize: WritableComputedRef<number>;
  orderFieldName: WritableComputedRef<string>;
  orderFieldDirection: WritableComputedRef<string>;
  loading: WritableComputedRef<boolean>;
  total: WritableComputedRef<number>;
  filter: WritableComputedRef<Filter[]>;

  get(): Promise<ListDataSourceResponse<any>>;

  getItem(id: number): any;

  add(model: any): Promise<any>;

  remove(id: number): Promise<void>;

  update(model: any): Promise<void>;
}

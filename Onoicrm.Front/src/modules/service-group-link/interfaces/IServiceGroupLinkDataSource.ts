import {IListDataSource} from "~/modules/data-sources";

export interface IServiceGroupLinkDataSource extends IListDataSource<any>{
  cascadeAdd(items:any[]):Promise<any>
  cascadeRemove(items:any[]):Promise<any>
}

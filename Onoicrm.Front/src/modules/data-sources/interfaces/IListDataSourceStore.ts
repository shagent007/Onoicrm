import {IListDataSource} from "~/modules/data-sources";
import {Ref} from "vue";

export interface IListDataSourceStore extends IListDataSource<any>{
  loaded:Ref<boolean>;
}

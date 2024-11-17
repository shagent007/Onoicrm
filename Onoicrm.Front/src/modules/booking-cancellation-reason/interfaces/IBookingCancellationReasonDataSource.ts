import {IListDataSource} from "~/modules/data-sources";
import {Ref} from "vue";

export interface IBookingCancellationReasonDataSource extends IListDataSource<any> {
  getStatistics():Promise<any>;
  statistics:Ref<any[]>
}

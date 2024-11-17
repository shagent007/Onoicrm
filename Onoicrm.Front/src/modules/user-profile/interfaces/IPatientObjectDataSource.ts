import { IListDataSource, IObjectDataSource } from "~/modules/data-sources";
import { Ref } from "vue";
import { ToothStateModel } from "~/modules/tooth/models/ToothStateModel";

export interface IPatientObjectDataSource extends IObjectDataSource {
  getToothStates: (clinicId: number) => {};
  toothStates: Ref<ToothStateModel[]>;
  getInvoices: () => Promise<any>;
  updateInvoices: (groupId: number) => Promise<any>;
  invoices:Ref<any[]>
}

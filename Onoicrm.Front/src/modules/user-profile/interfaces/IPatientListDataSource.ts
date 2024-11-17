import {IListDataSource} from "~/modules/data-sources";
import {CategoryMailingModel} from "~/modules/user-profile/models/CategoryMailingModel";

export interface IPatientListDataSource extends IListDataSource<any>{
  mailingByCategory(model:CategoryMailingModel):Promise<any>;

}

// toothStates:Ref<ToothState[]>
// getToothStates(clinicId:number) // ?clinicId=

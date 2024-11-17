import {IObjectDataSource} from "~/modules/data-sources";
import {Ref} from "vue";

export interface IDoctorObjectDataSource extends IObjectDataSource{
  getBookingCount(dateTime:string):Promise<any>
  getInvoices(clinicId:number):Promise<any>
  invoices:Ref<any[]>;
}

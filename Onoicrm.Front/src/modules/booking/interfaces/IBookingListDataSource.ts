import {IListDataSource} from "~/modules/data-sources";

export interface IBookingListDataSource extends IListDataSource<any>{
    notifyPatient: (id: number, text: string)=>Promise<any>,
    notifyDoctor: (id: number, text: string)=>Promise<any>,
    getSchedule:()=>any,
    updateReport: (id: number,model:any)=>Promise<any>
}




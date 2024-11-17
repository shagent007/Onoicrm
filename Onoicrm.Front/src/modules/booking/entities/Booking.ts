import {Entity} from "~/shared/entities/Entity";
import {UserRegisterModel} from "~/modules/user/models/UserResisterModel";
export enum DiscountType {Percent, Money}
export enum SalaryType {Salary, Percent, ServicePercent }
export class Booking extends Entity {
  public doctorId: number| null = null;
  public patientId: number| null = null;
  public doctor:any;
  public serviceGroupId:any
  public armchairId: number | null = null;
  public clinicId: number| null = null;
  public description: string = "";
  public сomplaint: string = "";
  public dateTimeStart:string = "";
  public dateTimeEnd:string = "";
  public patient?: any;
  public bookingServices:any[] = []
  public serviceGroups:any[] = []
  public bookingTeeth:any[] = []
  public discount:number = 0;
  public discountType:DiscountType = DiscountType.Percent;
  public implementedServices:any[] = []
  public salary:number = 0;
  public salaryType:SalaryType = SalaryType.Percent;
  constructor(instance?: Partial<Booking>) {
    super();
    Object.assign(this, instance);
  }
}


export class BookingModel {
  public booking!:Booking;
  public serviceGroupId?:number|null = null
  constructor(instance?: Partial<BookingModel>) {
    Object.assign(this, instance);
  }
}


export enum PatientTypeId {
  Exist,
  New
}

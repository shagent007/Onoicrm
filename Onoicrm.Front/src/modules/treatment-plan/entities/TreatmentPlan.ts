import {Entity} from "~/shared/entities/Entity";

export class TreatmentPlan extends Entity{
  public patientId!:number
  public doctorId!:number
  public toothId!:number
  public clinicId!:number
  public bookingId!:number
  public caption!:string
  public description:string = ""

  constructor(init?:Partial<TreatmentPlan>) {
    super();
    Object.assign(this, init)
  }
}

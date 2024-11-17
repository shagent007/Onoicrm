import {Day} from "~/modules/booking/entities/Day";

export class Cell {
  public day!:Day
  public timeSlot!:string

  constructor(init?:Partial<Cell>) {
    if (init){
      Object.assign(this,init)
    }
  }
}

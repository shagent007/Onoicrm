import Html from "~/shared/ui/file-types/html.vue";
import {getGUID} from "~/shared/lib/getGUID";

export type Cell = DataCell | DateCell | TimeCell | EmptyCell;
export enum ValueType {
  Patient,Armchair,Doctor, Null
}
export class Table {
  public id = getGUID();
  public rows:Row[] = []
  constructor(init?:Partial<Table>) {
    Object.assign(this,init)
  }
}

export class Row {
  public id = getGUID();
  public cells:Cell[] = []
  constructor(init?:Partial<Row>) {
    Object.assign(this,init)
  }
}

export class DataCell{
  public id!:number
  public bookingId!:number;
  public color!:string;
  public value!:string;
  public date!:string;
  public time!:string;
  public rowIndex!:number;
  public collIndex!:number;
  public valueType:ValueType = ValueType.Null;
  public className:string = 'booking-spreadsheet__cell--selectable'
  constructor(init?:Partial<DataCell>) {
    Object.assign(this, init)
  }
}

export class DateCell{
  public id!:number
  public date!:string;
  public caption!:string;
  public data!:string
  public className:string = 'booking-spreadsheet__header'
  public get value(){
    return `<span class="capitalize">${this.caption}</span> <br> ${this.date}`
  }
  constructor(init?:Partial<DateCell>) {
    Object.assign(this, init)
  }
}


export class TimeCell{
  public id!:number
  public value!:string;
  public date!:string;
  constructor(init?:Partial<TimeCell>) {
    Object.assign(this, init)
  }
}


export class EmptyCell {
  public id!:number
  public value:string = ""
  constructor(init?:Partial<TimeCell>) {
    Object.assign(this, init)
  }
}

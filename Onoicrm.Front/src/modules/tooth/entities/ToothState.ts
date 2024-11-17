import {getGUID} from "~/shared/lib/getGUID";

export class ToothState {
  public id:string = getGUID();
  public cssClass!: string;
  public caption!: string;
  public description!: string;
  public color!:string;
  constructor(init?: Partial<ToothState>) {
    Object.assign(this, init)
  }
}



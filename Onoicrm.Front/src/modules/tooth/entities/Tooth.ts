import {ToothState} from "./ToothState";

import {getGUID} from "~/shared/lib/getGUID";

export class Tooth {
  public id:string = getGUID();
  public name: string = "";
  public position!: number
  public description: string = "";
  public quarter!:number
  public state!: ToothState

  constructor(init?: Partial<Tooth>) {
    Object.assign(this, init)
  }
}

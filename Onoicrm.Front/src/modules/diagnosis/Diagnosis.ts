import {getGUID} from "~/shared/lib/getGUID";

export class Diagnosis {
  public id:string = getGUID();
  public caption!: string;
  public description!: string;
  public value!: boolean;

  constructor(init?: Partial<Diagnosis>) {
    Object.assign(this, init)
  }
}

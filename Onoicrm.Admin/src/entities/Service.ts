import { Entity } from './Entity';

export class Service extends Entity {
  public caption!: string;
  public price!: number;
  public priceFrom!: number;
  public priceTo!: number;
  public clinicId!:number;
  constructor(init?:Partial<Service>) {
      super();
      Object.assign(this,init)
  }
}

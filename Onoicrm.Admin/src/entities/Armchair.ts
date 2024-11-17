import { Entity } from './Entity';

export class Armchair extends Entity {
  public caption!: string;
  public description!: string;
  public clinicId!:number
  constructor(instance?: Partial<Armchair>) {
    super();
    Object.assign(this, instance);
  }
}
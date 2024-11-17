import { Entity } from './Entity';
import { Clinic } from './Clinic';

export class Tooth extends Entity {
  public caption!: string;
  public description!: string;
  public position!: number;

  constructor(instance?: Partial<Clinic>) {
    super();
    Object.assign(this, instance);
  }
}
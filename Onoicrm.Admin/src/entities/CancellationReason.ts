import { Entity } from './Entity';
import { Clinic } from './Clinic';

export class CancellationReason extends Entity {
  public caption!: string;
  public description!: string;

  constructor(instance?: Partial<CancellationReason>) {
    super();
    Object.assign(this, instance);
  }
}

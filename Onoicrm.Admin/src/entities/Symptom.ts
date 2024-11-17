import { Entity } from './Entity';

export class Symptom extends Entity {
  public caption!: string;
  public description!: string;

  constructor(instance?: Partial<Symptom>) {
    super();
    Object.assign(this, instance);
  }
}
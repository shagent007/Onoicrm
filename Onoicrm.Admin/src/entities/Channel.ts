import { Entity } from './Entity';

export class Channel extends Entity {
  public caption?: string;
  public description?: string;
  public toothId?: number;

  constructor(instance?: Partial<Channel>) {
    super();
    Object.assign(this, instance);
  }
}

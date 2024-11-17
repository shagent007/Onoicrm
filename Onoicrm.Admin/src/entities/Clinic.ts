import { Entity } from './Entity';

export class Clinic extends Entity {
    public caption!: string;
    public expiredDate!:string;
    public groupUserProfile:any
    constructor(instance?: Partial<Clinic>) {
        super();
        Object.assign(this, instance);
    }
}


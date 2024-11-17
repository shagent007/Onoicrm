import {getGUID} from "../../lib/getGUID";

export class EntityPropertyGroup {
    public id: string = getGUID();
    public name!: string;
    public caption!: string;
    public priority!: number;

    constructor(instance?: Partial<EntityPropertyGroup>) {
        Object.assign(this, instance);
    }
}

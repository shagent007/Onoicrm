import { EntityPropertyType } from './EntityPropertyType';
import { EntityPropertyEditor } from './EntityPropertyEditor';
import {getGUID} from "../../lib/getGUID";

export class EntityPropertyInfo {
    public id: string = getGUID();
    public name!: string;
    public caption!: string;
    public type!: EntityPropertyType;
    public editor!: EntityPropertyEditor;
    public priority?: number;
    public groupName?: string;

    constructor(instance?: Partial<EntityPropertyInfo>) {
        Object.assign(this, instance);
    }
}

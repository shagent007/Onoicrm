import { getGUID } from '../../services/helpers';
import { EntitityPropertyType } from './EntitityPropertyType';
import { EntitityPropertyEditor } from './EntitityPropertyEditor';

export class EntityPropertyInfo {
    public id: string = getGUID();
    public name!: string;
    public caption!: string;
    public type!: EntitityPropertyType;
    public editor!: EntitityPropertyEditor;
    public priority?: number;
    public groupName?: string;

    constructor(instance?: Partial<EntityPropertyInfo>) {
        Object.assign(this, instance);
    }
}

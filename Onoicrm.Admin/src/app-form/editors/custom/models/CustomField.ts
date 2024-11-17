import { Field } from '../../../models/Field';
import {ShallowRef, VNode} from "vue";
import { FieldAttribute } from '../../../models/FieldAttribute';
import { FieldConfig } from '../../../models/FieldConfig';
export class CustomField extends Field {
    public readonly editor: string = 'custom';
    public component!:VNode;
    public attrs!: FieldAttribute;
    public config!: FieldConfig;
    constructor(name: string, init?: Partial<CustomField>) {
        super(name, init);
        this.name = name;
        Object.assign(this, init);
    }


}

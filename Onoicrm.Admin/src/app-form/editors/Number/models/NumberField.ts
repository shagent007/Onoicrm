import { Field } from '../../../models/Field';
import { InputFieldAttribute } from '../../../models/InputFieldAttribute';
import { FieldConfig } from '../../../models/FieldConfig';
export class NumberField extends Field {
    public readonly editor: string = 'number';
    public attrs!: InputFieldAttribute;
    public config!: FieldConfig;
    constructor(name: string, init?: Partial<NumberField>) {
        super(name, init);
        this.name = name;
        Object.assign(this, init);
    }
}

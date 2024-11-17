import { Field } from '../../../models/Field';

import { InputFieldAttribute } from '../../../models/InputFieldAttribute';
import { FieldConfig } from '../../../models/FieldConfig';
export class BoolField extends Field {
    public readonly editor: string = 'bool';
    public attrs!: InputFieldAttribute;
    public config!: FieldConfig;
    constructor(name: string, init?: Partial<BoolField>) {
        super(name, init);
        this.name = name;
        Object.assign(this, init);
    }
}

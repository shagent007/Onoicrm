import { Field } from '../../../models/Field';

import { InputFieldAttribute } from '../../../models/InputFieldAttribute';
import { FieldAttribute } from '../../../models/FieldAttribute';
import { FieldConfig } from '../../../models/FieldConfig';
export class StringField extends Field {
    public readonly editor: string = 'string';
    public attrs!: InputFieldAttribute;
    public config!: FieldConfig;
    constructor(name: string, init?: Partial<StringField>) {
        super(name, init);
        this.name = name;
        Object.assign(this, init);
    }
}

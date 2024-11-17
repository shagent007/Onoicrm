import { Field } from '../../../../models/Field';
import { InputFieldAttribute } from '../../../../models/InputFieldAttribute';
import { FieldConfig } from '../../../../models/FieldConfig';

export class MemoField extends Field {
    public readonly editor: string = 'memo';
    public attrs!: InputFieldAttribute;
    public config!: FieldConfig;
    public rows!:number
    constructor(name: string, init?: Partial<MemoField>) {
        super(name, init);
        this.name = name;
        Object.assign(this, init);
    }
}

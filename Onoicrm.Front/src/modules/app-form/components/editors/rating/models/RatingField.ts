import { Field } from '../../../../models/Field';
import { FieldAttribute } from '../../../../models/FieldAttribute';
import { FieldConfig } from '../../../../models/FieldConfig';

export class RatingField extends Field {
    public readonly editor: string = 'rating';
    public attrs!: FieldAttribute;
    public config!: FieldConfig;
    constructor(name: string, init?: Partial<RatingField>) {
        super(name, init);
        this.name = name;
        Object.assign(this, init);
    }
}

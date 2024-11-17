import { FieldConfig } from '../../models/FieldConfig';

export class CollectionFieldConfig extends FieldConfig {
    public getItems!: string;
    constructor(init?: Partial<CollectionFieldConfig>) {
        super();
        Object.assign(this, init);
    }
}
import { Field } from '../../../models/Field';
import { SelectField } from '../../editors/select/models/SelectField';
import { FieldAttribute } from '../../../models/FieldAttribute';
import { CollectionFieldConfig } from './CollectionFieldConfig';
import { CollectionContainer } from '../containers/models/CollectionContainer';
import { CollectionDefaultContainer } from '../containers/models/CollectionDefaultContainer';
import { CollectionItem } from '../items/models/CollectionItem';
import { CollectionChipItem } from '../items/models/CollectionChipItem';
export type DataSelectorType = "table" | "tree";

export class CollectionField extends Field {
    public readonly editor: string = "collection";
    public config!:CollectionFieldConfig;
    public attrs!: FieldAttribute;
    public getConfig!:string;
    public customAdd!:string;
    public customDelete!:string;
    public container:CollectionContainer = new CollectionDefaultContainer();
    public item:CollectionItem = new CollectionChipItem();
    public getItemConfig!:string;
    public getCustomExcludeCallBack!:string
    public selector:DataSelectorType = "table";
    constructor(name: string, init?: Partial<CollectionField>) {
        super(name, init);
        this.name = name;
        Object.assign(this, init);
    }
}

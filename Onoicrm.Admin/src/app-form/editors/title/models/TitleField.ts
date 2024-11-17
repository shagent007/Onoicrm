import { Field } from "../../../models/Field";
import { FieldAttribute } from '../../../models/FieldAttribute';
import { FieldConfig } from '../../../models/FieldConfig';

export class TitleField extends Field {
  public readonly editor: string = "title";
  public attrs!: FieldAttribute;
  public config!: FieldConfig;
  constructor(name: string, init?: Partial<TitleField>) {
    super(name, init);
    this.name = name;
    Object.assign(this, init);
  }
}

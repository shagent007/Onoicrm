import { Field } from "../../../models/Field";
import { FieldAttribute } from '../../../models/FieldAttribute';
import { FieldConfig } from '../../../models/FieldConfig';
export class EmptyField extends Field {
  public readonly editor: string = "empty";
  public attrs!: FieldAttribute;
  public config!: FieldConfig;
  constructor(name: string, init?: Partial<EmptyField>) {
    super(name, init);
    this.name = name;
    Object.assign(this, init);
  }
}

import { Field } from "../../../models/Field";
import { InputFieldAttribute } from "../../../models/InputFieldAttribute";
import { SelectFieldConfig } from "./SelectFieldConfig";

export class SelectField extends Field {
  public readonly editor: string = "select";
  public attrs!: InputFieldAttribute;
  public config!: SelectFieldConfig;
  constructor(name: string, init?: Partial<SelectField>) {
    super(name, init);
    this.name = name;
    Object.assign(this, init);
  }
}

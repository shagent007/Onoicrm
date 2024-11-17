import { Field } from "../../../../models/Field";
import { InputFieldAttribute } from "../../../../models/InputFieldAttribute";
import { SelectFieldConfig } from "./SelectFieldConfig";

export class MultiSelectField extends Field {
  public readonly editor: string = "multiSelect";
  public attrs!: InputFieldAttribute;
  public config!: SelectFieldConfig;
  constructor(name: string, init?: Partial<MultiSelectField>) {
    super(name, init);
    this.name = name;
    Object.assign(this, init);
  }
}

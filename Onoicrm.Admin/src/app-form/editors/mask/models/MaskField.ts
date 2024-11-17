import { Field } from "../../../models/Field";

import { InputFieldAttribute } from "../../../models/InputFieldAttribute";
import { MaskFieldConfig } from "./MaskFieldConfig";
export class MaskField extends Field {
  public readonly editor: string = "mask";
  public attrs!: InputFieldAttribute;
  public config!: MaskFieldConfig;

  constructor(name: string, init?: Partial<MaskField>) {
    super(name, init);
    this.name = name;
    Object.assign(this, init);
  }
}

import { Field } from "../../../models/Field";
import { InputFieldAttribute } from "../../../models/InputFieldAttribute";
import { SelectFieldConfig } from "./SelectFieldConfig";

export class TreeSelectField extends Field {
  public readonly editor: string = "treeSelect";
  public attrs!: InputFieldAttribute;
  public config!: SelectFieldConfig;
  public getConfig!:string
  constructor(name: string, init?: Partial<TreeSelectField>) {
    super(name, init);
    this.name = name;
    Object.assign(this, init);
  }
}

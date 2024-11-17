import { FieldConfig } from "../../../../models/FieldConfig";
import { SelectMode } from "./SelectMode";

export class SelectFieldConfig extends FieldConfig {
  public labelKeyName!: string;
  public valueKeyName!: string;
  public multiple!: boolean;
  public getItems!: string;
  public mode: SelectMode = SelectMode.Select;

  constructor(init?: Partial<SelectFieldConfig>) {
    super();
    Object.assign(this, init);
  }
}

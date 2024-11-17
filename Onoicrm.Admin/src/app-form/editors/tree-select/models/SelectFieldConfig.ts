import { FieldConfig } from "../../../models/FieldConfig";

export class SelectFieldConfig extends FieldConfig {
  public labelKeyName!: string;
  public valueKeyName!: string;
  public multiple!: boolean;
  public getItems!: string;


  constructor(init?: Partial<SelectFieldConfig>) {
    super();
    Object.assign(this, init);
  }
}

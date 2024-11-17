import { FieldConfig } from "../../../../app-form/models/FieldConfig";

export class MaskFieldConfig extends FieldConfig {
  public mask!: string;
  constructor(init?: Partial<MaskFieldConfig>) {
    super();
    Object.assign(this, init);
  }
}

import {Field, FieldConfig, InputFieldAttribute} from "~/modules/app-form";

export class PhoneField extends Field {
  public readonly editor: string = "phone";
  public attrs!: InputFieldAttribute;
  public config!: FieldConfig;

  constructor(name: string, init?: Partial<PhoneField>) {
    super(name, init);
    this.name = name;
    Object.assign(this, init);
  }
}

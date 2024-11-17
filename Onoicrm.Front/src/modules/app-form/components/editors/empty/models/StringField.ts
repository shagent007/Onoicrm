import { Field,FieldAttribute,FieldConfig } from "~/modules/app-form";
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

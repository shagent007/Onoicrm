import { FieldAttribute } from "./FieldAttribute";

export class InputFieldAttribute extends FieldAttribute {
  public placeholder!: string;
  public disabled!: boolean;
  public constructor(init?: Partial<InputFieldAttribute>) {
    super(init);
    Object.assign(this, init);
  }
}

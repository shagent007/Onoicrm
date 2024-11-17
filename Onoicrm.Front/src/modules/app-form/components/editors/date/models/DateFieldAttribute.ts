import { InputFieldAttribute } from "../../../../models/InputFieldAttribute";

export class DateFieldAttribute extends InputFieldAttribute {
  public format: string = "dd-mm-yy";
  constructor(init?: Partial<DateFieldAttribute>) {
    super(init);
    Object.assign(this, init);
  }
}

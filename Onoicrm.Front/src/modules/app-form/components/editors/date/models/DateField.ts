import { Field } from "../../../../models/Field";

import { DateFieldAttribute } from "./DateFieldAttribute";
import { FieldAttribute } from '../../../../models/FieldAttribute';
import { FieldConfig } from '../../../../models/FieldConfig';
export class DateField extends Field {
  public readonly editor: string = "date";
  public attrs!: DateFieldAttribute;
  public config!: FieldConfig;
  constructor(name: string, init?: Partial<DateField>) {
    super(name, init);
    this.name = name;
    Object.assign(this, init);
  }
}

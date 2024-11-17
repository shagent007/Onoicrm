import { containers } from "../consts/consts";
import { FieldService } from "../services/FieldService";
import { FormService } from "../services/FormService";
import {getGUID} from "~/shared/lib/getGUID";

export class FormContainer {
  public id: string = getGUID();
  public type: string = containers.default;
  constructor(init?: Partial<FormContainer>) {
    Object.assign(this, init);
  }
}

export class DefaultFormContainer extends FormContainer {
  constructor(init?: Partial<DefaultFormContainer>) {
    super(init);
    Object.assign(this, init);
  }
}

export class EmptyFormContainer extends FormContainer {
  public type: string = containers.empty;
  constructor(init?: Partial<DefaultFormContainer>) {
    super(init);
    Object.assign(this, init);
  }
}


export class GridFormContainer extends FormContainer {
  public rows: Row[] = [];
  public initFields(formService: FormService) {
    for (const row of this.rows) {
      for (const col of row.cols) {
        col.initFields(formService.fieldServices);
      }
    }
  }

  constructor(init?: Partial<GridFormContainer>) {
    super(init);
    Object.assign(this, init);
  }
}

export class Row {
  public id: string = getGUID();
  public cols: Col[] = [];
  constructor(init?: Partial<Row>) {
    Object.assign(this, init);
  }
}

export class Col {
  public id: string = getGUID();
  public size!: string;
  public fieldNames: string[] = [];
  public fields: FieldService[] = [];

  public initFields(fields: FieldService[]) {
    for (const fieldName of this.fieldNames) {
      const field = fields.find((f: FieldService) => f.field.name == fieldName);
      if (!field) continue;
      this.fields.push(field);
    }
  }
  constructor(init?: Partial<Col>) {
    Object.assign(this, init);
  }
}

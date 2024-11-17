import { Field } from "../models/Field";
import { DefaultFormContainer, FormContainer } from "../models/FormContainer";
import { FieldService } from "./FieldService";
import { FormServiceConfig } from "./FormServiceConfig";
import { IUpdateFieldService } from "./IUpdateFieldService";

export class FormService {
  public fieldServices: FieldService[] = [];
  public actions: any = {};
  public container: FormContainer = new DefaultFormContainer();
  public updateFieldService?: IUpdateFieldService;

  constructor(config: FormServiceConfig) {
    this.fieldServices = config.fields.map(
      (f: Field) =>
        new FieldService(
          config.model,
          f,
          config.actions,
          config.updateFieldService,
        ),
    );
    this.actions = config.actions ?? {};

    this.updateFieldService = config.updateFieldService;
    this.container = config.container ?? new DefaultFormContainer();
  }

  public async validate(): Promise<boolean> {
    let isValid = true;
    for (const fieldService of this.fieldServices) {
      const valid = await fieldService.validate();
      if (!valid) isValid = valid;
    }
    return isValid;
  }

  public reset() {
    for (const fieldService of this.fieldServices) {
      fieldService.reset();
    }
  }

  public clear() {
    for (const fieldService of this.fieldServices) {
      fieldService.clear();
    }
  }
}

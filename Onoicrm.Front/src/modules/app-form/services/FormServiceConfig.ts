import { Ref } from "vue";
import { containers } from "../consts/consts";
import { Field } from "../models/Field";
import { DefaultFormContainer, FormContainer } from "../models/FormContainer";
import { IUpdateFieldService } from "./IUpdateFieldService";

export class FormServiceConfig {
  model!: any;
  fields: Field[] = [];
  actions: any = {};
  container: FormContainer = new DefaultFormContainer();
  updateFieldService?: IUpdateFieldService;

  constructor(instance: Partial<FormServiceConfig>) {
    Object.assign(this, instance);
  }
}

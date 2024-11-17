import { getValue, setValue } from "~/shared/lib/helpers";
import { isRef, ref, Ref } from "vue";
import { validate } from "~/shared/lib/validators";
import { Field } from "../models/Field";
import { FieldAttribute } from "../models/FieldAttribute";
import { FieldConfig } from "../models/FieldConfig";
import { IUpdateFieldService } from "./IUpdateFieldService";
import {getGUID} from "~/shared/lib/getGUID";

export class FieldService<
  TField extends Field = Field,
  TFieldAttribute extends FieldAttribute = FieldAttribute,
  TFieldConfig extends FieldConfig = FieldConfig,
> {
  public id: string = getGUID();
  public errorMessages: Ref<string[]> = ref([]);
  public successMessages: Ref<string[]> = ref([]);
  public model: any;
  public readonly field: TField;
  public readonly actions: any;
  public updateFieldService?: IUpdateFieldService;

  public get value(): any {
    if (this.field.isEmpty) return "";
    const customGetter = this.actions?.[this.field.get ?? ""];
    if (customGetter) return customGetter(this.model, this.field.name);
    return getValue(this.model, this.field.name);
  }

  public set value(value: any) {
    const customSetter = this.actions?.[this.field.set ?? ""];
    if (customSetter) customSetter(this.model, this.field.name, value);
    else setValue(this.model, this.field.name, value);
  }

  public attrs!: Ref<TFieldAttribute | undefined>;

  public get config() {
    return this.field.config;
  }

  public get showErrors() {
    return this.errorMessages.value?.length > 0;
  }

  public get grid() {
    return this.field.grid;
  }

  public async update() {
    if (!this.updateFieldService)
      throw new Error("updateFieldService не передан");
    await this.updateFieldService.updateField(this.model.id, [
      {
        fieldName: this.field.name,
        fieldValue: getValue(this.model, this.field.name),
      },
    ]);
  }

  public canShow() {
    if (!this.actions?.[this?.field?.canShow as string]) return true;
    const canShow = this.actions[this.field.canShow](this.model);
    return canShow;
  }

  public reset() {
    this.successMessages.value = [];
    this.errorMessages.value = [];
  }

  public async validate(trigger:string|null=null,withoutTrigger:boolean=false) {
    const res = await validate<TField, TFieldAttribute, TFieldConfig>(this, trigger,withoutTrigger);
    this.errorMessages.value = res.errorMessages;
    this.successMessages.value = res.successMessages;
    return res.isValid;
  }

  public clear() {
    this.value = this.field.initialValue;
  }

  public constructor(
    model: any,
    field: TField,
    actions: any,
    updateFieldService?: IUpdateFieldService,
  ) {
    this.field = field;
    this.actions = actions;
    this.model = model;
    if(field.initialValue) {
      this.value = field.initialValue;
    }

    this.attrs = ref(field.attrs) as Ref<TFieldAttribute>;
    this.errorMessages.value = [];
    this.successMessages.value = [];
    this.updateFieldService = updateFieldService;
  }
}

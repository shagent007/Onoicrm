import { Field } from '~/modules/app-form/models/Field';
import { Validation } from '~/modules/app-form/models/Validation';
import { regexList } from '../consts/consts';
import { ValidationResult } from '~/shared';
import { FieldService } from '~/modules/app-form/services/FieldService';
import { FieldAttribute } from '~/modules/app-form/models/FieldAttribute';
import { FieldConfig } from '~/modules/app-form/models/FieldConfig';
import { isRef } from 'vue';
import { getValue } from './helpers';
import axios from "axios";

const required = (value: any, info: Validation, model?: any): Promise<string> =>
    new Promise((resolve: any, reject: any) => (!!value ? resolve(info.successMessage) : reject(info.errorMessage)));

const regular = (value: any, info: Validation, model?: any): Promise<string> =>
    new Promise<string>((resolve, reject) => {
        regexList[info.value].test(value) ? resolve(info.successMessage) : reject(info.errorMessage);
    });

const min = (value: any, info: Validation, model?: any): Promise<string> =>
    new Promise<string>((resolve, reject) => {
        const _value = typeof value === 'number' ? value : value?.length ?? 0;
        _value > info.value ? resolve(info.successMessage) : reject(info.errorMessage);
    });

const max = (value: any, info: Validation, model?: any): Promise<string> =>
    new Promise<string>((resolve, reject) => {
        const _value = typeof value === 'number' ? value : value?.length ?? 0;
        _value < info.value ? resolve(info.successMessage) : reject(info.errorMessage);
    });

const like = (value: any, info: Validation, model?: any): Promise<string> => {
    const source = isRef(model) ? model.value : model;
    const targetValue = getValue(source, info.value);
    return new Promise<string>((resolve, reject) => {
        value === targetValue ? resolve(info?.successMessage) : reject(info.errorMessage);
    });
};

const userIsExist = async (value: any, info: Validation, model?: any): Promise<string> => {
  return await new Promise<string>( async (resolve, reject) => {
    try {
      const source = isRef(model) ? model.value : model;
      const targetValue = getValue(source, info.value);
      await axios.get(`/api/v1/public/userprofile/check/${targetValue}/`);
      resolve(info?.successMessage)
    } catch (e) {
      reject(info.errorMessage);
    }
  });
};

export const validators: any = { required, regular, min, max, like ,userIsExist};

export const validate = async <TField extends Field, TFieldAttribute extends FieldAttribute, TFieldConfig extends FieldConfig>(
    fieldService: FieldService<TField, TFieldAttribute, TFieldConfig>,
    trigger:string|null=null,
    withoutTrigger:boolean=false
): Promise<ValidationResult> => {
    const errorMessages: Array<string> = [];
    const successMessages: Array<string> = [];
    for (const validation of fieldService.field.validations) {
        try {
          if (!withoutTrigger && validation.trigger !== null && trigger !== validation.trigger) {
            continue;
          }

          const successMessage = await validators[validation.name](fieldService.value, validation, fieldService.model);
          successMessages.push(successMessage ?? '');
        } catch (errorMessage: any) {
            errorMessages.push(errorMessage);
        }
    }

    return new ValidationResult({ errorMessages, successMessages });
};

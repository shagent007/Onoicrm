import { Field } from '../app-form/models/Field';
import { Validation } from '../app-form/models/Validation';
import { regexList } from './consts';
import { ValidationResult } from '../models/ValidationResult';
import { FieldService } from '../app-form/services/FieldService';
import { FieldAttribute } from '../app-form/models/FieldAttribute';
import { FieldConfig } from '../app-form/models/FieldConfig';
import { isRef } from 'vue';
import { getValue } from './helpers';

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
    const sorce = isRef(model) ? model.value : model;
    const targetValue = getValue(sorce, info.value);
    return new Promise<string>((resolve, reject) => {
        value === targetValue ? resolve(info?.successMessage) : reject(info.errorMessage);
    });
};

export const validators: any = { required, regular, min, max, like };

export const validate = async <TField extends Field, TFieldAttribute extends FieldAttribute, TFieldConfig extends FieldConfig>(
    fieldService: FieldService<TField, TFieldAttribute, TFieldConfig>
): Promise<ValidationResult> => {
    const errorMessages: Array<string> = [];
    const successMessages: Array<string> = [];
    for (const validation of fieldService.field.validations) {
        try {
            const successMessage = await validators[validation.name](fieldService.value, validation, fieldService.model);
            successMessages.push(successMessage ?? '');
        } catch (errorMessage: any) {
            errorMessages.push(errorMessage);
        }
    }

    return new ValidationResult({ errorMessages, successMessages });
};

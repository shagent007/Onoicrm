import { Ref } from 'vue';
import { Field } from '../app-form/models/Field';

export interface EditorModel {
    visible: Ref<boolean>;
    create(instance: Record<string, any>, fields: Field[], actions?: Record<string, any>): Promise<any>;
    update(instance: Record<string, any>, fields: Field[], actions?: Record<string, any>): Promise<any>;
    cancel(): void;
    submit(): Promise<boolean>;
    form: Ref;
}

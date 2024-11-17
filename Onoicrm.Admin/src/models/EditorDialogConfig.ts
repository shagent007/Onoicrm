import { Field } from '../app-form/models/Field';
import { IUpdateFieldService } from '../app-form/services/IUpdateFieldService';
import { DefaultFormContainer, FormContainer } from '../app-form/models/FormContainer';

export class EditorDialogConfig {
    public title: string = 'Обновить данные';
    public model: any;
    public fields: Field[] = [];
    public actions!: Record<string, Function>;
    public container: FormContainer = new DefaultFormContainer();
    public btnInstructions: any[] = [];
    public updateFieldService?: IUpdateFieldService;

    constructor(instance: Partial<EditorDialogConfig>) {
        Object.assign(this, instance);
    }
}

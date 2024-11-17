import { Field } from '~/modules/app-form';
import {DefaultFormContainer, FormContainer} from "~/modules/app-form/models/FormContainer";
import {IUpdateFieldService} from "~/modules/app-form/services/IUpdateFieldService";

export class EditorDialogConfig {
    public title!: string;
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

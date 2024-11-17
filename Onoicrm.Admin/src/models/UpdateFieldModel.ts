export class UpdateFieldModel {
    public fieldName!: string;
    public fieldValue!: any;

    constructor(instance?: Partial<UpdateFieldModel>) {
        Object.assign(this, instance);
    }
}

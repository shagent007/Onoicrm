import { UpdateFieldModel } from '../../models/UpdateFieldModel';

export interface IUpdateFieldService {
    updateField(id: number, updateFields: UpdateFieldModel[]): Promise<any>;
}

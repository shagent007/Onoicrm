import { UpdateFieldModel } from '~/modules/data-sources';

export interface IUpdateFieldService {
    updateField(id: number, updateFields: UpdateFieldModel[]): Promise<any>;
}

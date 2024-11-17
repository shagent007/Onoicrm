import { EntityPropertyGroup } from './EntityPropertyGroup';
import { EditorDialogConfig } from '~/modules/editor/models/EditorDialogConfig';
import {BaseTableColumn} from "../base-table/BaseTableColumn";
import {ColumnProps} from "primevue/column";
export class EntityMetaInfo {
    public clasName!: string;
    public columns!: BaseTableColumn[] | ColumnProps[];
    public groups: EntityPropertyGroup[] = [];
    public createItemInfo!: EditorDialogConfig;
    public updateItemInfo!: EditorDialogConfig;
    public filters!:any
    constructor(instance?: Partial<EntityMetaInfo>) {
        Object.assign(this, instance);
    }
}

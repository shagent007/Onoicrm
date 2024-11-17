import { EntityPropertyGroup } from './EntityPropertyGroup';
import { EntityPropertyInfo } from './EntityPropertyInfo';
import { EditorDialogConfig } from '../EditorDialogConfig';
import { ColumnProps } from 'primevue/column';
import { DataTableFilterMeta } from 'primevue/datatable';
export class EntityMetaInfo {
    public clasName!: string;
    public columns!: ColumnProps[];
    public properties: EntityPropertyInfo[] = [];
    public filters!: DataTableFilterMeta;
    public groups: EntityPropertyGroup[] = [];
    public createItemInfo!: EditorDialogConfig;
    public updateItemInfo!: EditorDialogConfig;
    constructor(instance?: Partial<EntityMetaInfo>) {
        Object.assign(this, instance);
    }
}

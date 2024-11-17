import {Entity} from "~/shared/entities/Entity";
import {Ref} from "vue";
import {IListDataSource} from "~/modules/data-sources/interfaces/IListDataSource";

export interface IToothStateStore<TEntity extends Entity> extends IListDataSource<any> {
    loaded: Ref<boolean> | boolean;
}


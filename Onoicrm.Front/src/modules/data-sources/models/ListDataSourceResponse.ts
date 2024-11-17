import { Entity } from '~/shared/entities/Entity';

export class ListDataSourceResponse<TEntity extends Entity> {
    public items: Array<TEntity> = [];
    public total!: number;
}

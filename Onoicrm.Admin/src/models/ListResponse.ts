import { Entity } from '../entities/Entity';

export class ListResponse<TEntity extends Entity> {
    public items: Array<TEntity>;
    public total: number;
}

export class ObjectDataSourceConfig {
    public className!: string;
    public id!: number;

    public constructor(init?: Partial<ObjectDataSourceConfig>) {
        Object.assign(this, init);
    }
}

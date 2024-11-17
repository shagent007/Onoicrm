export abstract class CollectionContainer {
    public abstract name: string;

    protected constructor(init?: Partial<CollectionContainer>) {
        Object.assign(this, init);
    }
}
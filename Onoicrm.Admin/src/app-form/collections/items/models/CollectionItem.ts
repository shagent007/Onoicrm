export abstract class CollectionItem {
    public abstract name: string;
    protected constructor(init?: Partial<CollectionItem>) {
        Object.assign(this, init);
    }
}


export class BaseTableConfig {
    public keyName: string = "id";

    constructor(init?: Partial<BaseTableConfig>) {
        Object.assign(this, init)
    }
}
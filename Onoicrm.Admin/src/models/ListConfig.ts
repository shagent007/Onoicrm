import { Filter } from './Filter';

export class ListConfig {
    public className!: string;
    public pageIndex: number = 1;
    public pageSize: number = 20;
    public orderFieldName: string = 'Id';
    public orderFieldDirection: string = 'ASC';
    public filter:Filter[] = []
    constructor(instance?: Partial<ListConfig>) {
        Object.assign(this, instance);
    }
}

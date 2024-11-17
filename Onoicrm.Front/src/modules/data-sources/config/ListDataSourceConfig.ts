import {Filter} from "../models/Filter";

export class ListDataSourceConfig {
    public className!: string;
    public pageIndex: number = 1;
    public pageSize: number = 20;
    public orderFieldName: string = 'Priority';
    public orderFieldDirection: string = 'ASC';
    public filter:Filter[] = [];
    public fields:string = "";

    constructor(instance?: Partial<ListDataSourceConfig>) {
        Object.assign(this, instance);
    }
}

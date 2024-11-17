import { CollectionContainer } from './CollectionContainer';

export class CollectionDefaultContainer extends CollectionContainer{
    public className!:string
    public name: string = "default";
    constructor(init?:Partial<CollectionDefaultContainer>) {
        super(init);
        Object.assign(this, init)
    }
}
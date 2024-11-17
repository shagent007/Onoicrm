import { ChipProps } from 'primevue/chip';
import { CollectionItem } from './CollectionItem';

export class CollectionChipItem extends CollectionItem {
    public name: string = 'chip';
    public attrs: ChipProps | any = {};

    constructor(init?: Partial<CollectionChipItem>) {
        super(init);
        Object.assign(this, init);
    }
}
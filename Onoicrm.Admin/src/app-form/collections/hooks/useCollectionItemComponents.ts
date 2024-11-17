import { CollectionField } from '../models/CollectionField';
import { ShallowRef, shallowRef } from 'vue';
import CollectionDefaultItem from "../items/chip.vue"
export type CollectionItemHook = {editor:ShallowRef} ;
export const useCollectionItemComponents = (name:string):CollectionItemHook => {
    const components:Record<string, ShallowRef> ={
        chip: shallowRef(CollectionDefaultItem)
    }

    if (!components[name])
        throw new Error(`Item коллекции с названием ${name} не найдено`);


    return {editor: components[name]}
}
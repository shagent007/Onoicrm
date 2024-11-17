import { CollectionField } from '../models/CollectionField';
import { ShallowRef, shallowRef } from 'vue';
import CollectionDefaultContainer from "../containers/default.vue"
export type CollectionContainerHook = {editor:ShallowRef} ;
export const useCollectionContainerComponents = (name:string):CollectionContainerHook => {
    const components:Record<string, ShallowRef> ={
        default: shallowRef(CollectionDefaultContainer)
    }

    if (!components[name])
        throw new Error(`Контейнер коллекции с названием ${name} не найдено`);


    return {editor: components[name]}
}
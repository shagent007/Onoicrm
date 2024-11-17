import { ShallowRef, shallowRef } from 'vue';
import DataTableSelectorDialog from "../../../components/data/selector-table-dialog.vue"
import DataTreeSelectorDialog from "../../../components/data/selector-tree-dialog.vue"
import { DataSelectorType } from '../models/CollectionField';
export const useSelectorComponents = (name:DataSelectorType):ShallowRef => {
    const components:Record<string, ShallowRef> ={
        table: shallowRef(DataTableSelectorDialog),
        tree: shallowRef(DataTreeSelectorDialog)
    }

    if (!components[name])
        throw new Error(`Selector c названием ${name} не найдено`);

    return  components[name]
}
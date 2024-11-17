import { ShallowRef, shallowRef } from "vue";
import { Field } from "../models/Field";
import StringEditor from "../editors/string/index.vue";
import PasswordEditor from "../editors/password/index.vue";
import SelectEditor from "../editors/select/index.vue";
import MemoEditor from "../editors/memo/index.vue";
import MaskEditor from "../editors/mask/index.vue";
import EmptyEditor from "../editors/empty/index.vue";
import TitleEditor from "../editors/title/index.vue";
import DateEditor from "../editors/date/index.vue";
import MultiSelectEditor from "../editors/multi-select/index.vue"
import CustomEditor from "../editors/custom/index.vue"
import RatingEditor from "../editors/rating/index.vue"
import CollectionEditor from "../collections/index.vue"
import TreeSelectEditor from "../editors/tree-select/index.vue"
import NumberEditor from "../editors/Number/index.vue"
import BoolEditor from "../editors/bool/index.vue"
export const useEditorComponents = () => {
  const editors: Record<string, ShallowRef> = {
    string: shallowRef(StringEditor),
    password: shallowRef(PasswordEditor),
    select: shallowRef(SelectEditor),
    memo: shallowRef(MemoEditor),
    mask: shallowRef(MaskEditor),
    empty: shallowRef(EmptyEditor),
    title: shallowRef(TitleEditor),
    date: shallowRef(DateEditor),
    multiSelect:shallowRef(MultiSelectEditor),
    custom:shallowRef(CustomEditor),
    rating:shallowRef(RatingEditor),
    collection:shallowRef(CollectionEditor),
    treeSelect:shallowRef(TreeSelectEditor),
    number:shallowRef(NumberEditor),
    bool:shallowRef(BoolEditor)
  };

  const getEditor = (field: Field): ShallowRef => {
    if (!editors[field.editor])
      throw new Error(`Редактор с названием ${field.editor} не найдено`);
    return editors[field.editor];
  };

  return { getEditor };
};

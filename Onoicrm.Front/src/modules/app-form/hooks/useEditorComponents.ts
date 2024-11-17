import { ShallowRef, shallowRef } from "vue";
import { Field } from "../models/Field";
import StringEditor from "../components/editors/string/index.vue";
import PasswordEditor from "../components/editors/password/index.vue";
import SelectEditor from "../components/editors/select/index.vue";
import MemoEditor from "../components/editors/memo/index.vue";
import MaskEditor from "../components/editors/mask/index.vue";
import EmptyEditor from "../components/editors/empty/index.vue";
import TitleEditor from "../components/editors/title/index.vue";
import DateEditor from "../components/editors/date/index.vue";
import MultiSelectEditor from "../components/editors/multi-select/index.vue"
import CustomEditor from "../components/editors/custom/index.vue"
import RatingEditor from "../components/editors/rating/index.vue"
import CollectionEditor from "../components/collections/index.vue"
import NumberField from "../components/editors/number/index.vue";
import PhoneField from "../components/editors/phone/index.vue"
export const useEditorComponents = () => {
  const editors: Record<string, ShallowRef> = {
    string: shallowRef(StringEditor),
    number: shallowRef(NumberField),
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
    phone:shallowRef(PhoneField)
  };

  const getEditor = (field: Field): ShallowRef => {
    if (!editors[field.editor])
      throw new Error(`Редактор с названием ${field.editor} не найдено`);
    return editors[field.editor];
  };

  return { getEditor };
};

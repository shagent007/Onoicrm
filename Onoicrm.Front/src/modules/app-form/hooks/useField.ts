import { inject, onMounted, ref, Ref, watch } from 'vue';
import { getValue } from "~/shared/lib/helpers";
import { IMessage, defaultMessage } from "~/shared/models/notification/IMessage";
import { Field } from "../models/Field";
import { FieldAttribute } from "../models/FieldAttribute";
import { FieldConfig } from "../models/FieldConfig";
import { Watcher } from "../models/Watcher";
import { FieldService } from "../services/FieldService";
import { FormService } from "../services/FormService";

export const useField = <
  TField extends Field = Field,
  TFieldAttribute extends FieldAttribute = FieldAttribute,
  TFieldConfig extends FieldConfig = FieldConfig,
>(
  fieldService: FieldService<TField, TFieldAttribute, TFieldConfig>,
  formService: FormService,
) => {
  const message = inject<IMessage>("message", defaultMessage);
  const items: Ref<any[]> = ref([]);
  const loaded: Ref<boolean> = ref(true);
  const loading: Ref<boolean> = ref(false);
  const update = async () => {
    if (!fieldService.updateFieldService || !formService.updateFieldService)
      return;
    await fieldService.update();
    message.success(`Поле успешно обновлено`);
  };
  const { field } = fieldService;
  const { actions } = formService;

  const watchCallBack = async (watcher: Watcher) => {
    loading.value = true;
    const { changeAttrs, changeItems, changeModel } = watcher;
    if (!!changeAttrs && !!actions[changeAttrs]) {
      fieldService.attrs.value = await actions[changeAttrs](field.attrs);
    }

    if (!!changeItems && !!actions[changeItems]) {
      items.value = await actions[changeItems]();
    }

    if (!!changeModel && !!actions[changeModel]) {
      await actions[changeModel](fieldService.model);
    }
    loading.value = false;
  };

  for (const watcher of fieldService.field.watchers) {
    for (const fieldName of watcher.fields) {
      watch(
        () => getValue(fieldService.model, fieldName),
        async () => await watchCallBack(watcher),
      );
    }
  }




  onMounted(async () => {
    for (const watcher of fieldService.field.watchers) {
      if (!watcher.callOnMounted) continue;
      await watchCallBack(watcher);
    }
  });

  return { message, update, items, loaded, loading };
};

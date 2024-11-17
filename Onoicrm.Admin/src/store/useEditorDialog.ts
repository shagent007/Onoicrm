import { defineStore } from 'pinia';
import { Ref, ref } from 'vue';
import { FormService } from '../app-form/services/FormService';
import { FormServiceConfig } from '../app-form/services/FormServiceConfig';
import { EditorDialogConfig } from '../models/EditorDialogConfig';
import { cloneDeep } from "lodash"

export const useEditorStore = defineStore('editor', () => {
    const formService: Ref<FormService | null> = ref(null);
    const visible: Ref<boolean> = ref(false);
    const title: Ref<string> = ref('');
    const model: Ref = ref({});
    let resolve: Function;

    const mount = (config: EditorDialogConfig) => {
        if (!!formService.value) {
            formService.value.reset();
        }


        formService.value = new FormService(
          new FormServiceConfig({
              model,
              fields: config.fields,
              actions: config.actions,
              container: config.container,
              updateFieldService: config.updateFieldService,
          })
        );
        title.value = config.title;
        visible.value = true;
    }

    const create = <TType = void>(config: EditorDialogConfig) => {
        model.value = cloneDeep(config.model);
        mount(config);
        return new Promise<TType>((r) => (resolve = r));
    };

    const update = <TType = void>(config: EditorDialogConfig) => {
        model.value = config.model;
        mount(config);
        return new Promise<TType>((r) => (resolve = r));
    };

    const submit = async () => {
        if (!formService.value) return;
        const valid = await formService.value.validate();
        if (!valid) return;
        visible.value = false;
        resolve({ ...model.value });
        model.value = null;
    };

    const cancel = () => {
        visible.value = false;
        resolve(null);
    };

    return { formService, update, submit, cancel, create, visible, title };
});

import { defineStore } from 'pinia';
import { Ref, ref } from 'vue';
import { FormService } from '~/modules/app-form/services/FormService';
import { FormServiceConfig } from '~/modules/app-form/services/FormServiceConfig';
import { EditorDialogConfig } from '../models/EditorDialogConfig';
import _ from 'lodash';

export const useEditorStore = defineStore('counter', () => {
    const formService: Ref<FormService | null> = ref(null);
    const visible: Ref<boolean> = ref(false);
    const title: Ref<string> = ref('');
    const model: Ref<any> = ref({});
    let resolve: Function;

    const create = <TType = any>(config: EditorDialogConfig) => {
        if (!!formService.value) {
            formService.value.reset();
        }
        model.value = _.cloneDeep(config.model);

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
        return new Promise<TType>((r) => (resolve = r));
    };

    const update = (config: EditorDialogConfig) => {
        if (!!formService.value) {
            formService.value.reset();
        }
        model.value = config.model;
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
        return new Promise<any>((r) => (resolve = r));
    };

    const submit = async () => {
        if (!formService.value) return;
        const valid = await formService.value.validate();
        if (!valid) return;
        visible.value = false;
        resolve({ ...model.value });
    };

    const cancel = () => {
        visible.value = false;
        resolve(null);
    };

    return { formService, update, submit, cancel, create, visible, title };
});

import { ShallowRef, shallowRef } from 'vue';
import { Provider } from '../models/Provider';
import DefaultProvider from '../components/providers/default.vue';

export const useProviderComponents = () => {
    const getProvider = (provider: Provider): ShallowRef => {
        switch (provider.name) {
            case 'default':
                return shallowRef(DefaultProvider);
            default:
                throw new Error(`Провайдер с названием ${provider.name} не найдено`);
        }
    };

    return { getProvider };
};

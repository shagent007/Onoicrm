import { createApp, reactive } from 'vue';
import { createPinia } from 'pinia';
import router from './router';
import App from './App.vue';
import CodeHighlight from './AppCodeHighlight';
import primevue from './plugins/primevue';
import { dom } from '@fortawesome/fontawesome-svg-core';
import { library } from '@fortawesome/fontawesome-svg-core';
import { faPhone, faTooth, faUserDoctor, faChair } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
dom.watch(); // This will kick of the initial replacement of i to svg tags and configure a MutationObserver
library.add(faPhone);
library.add(faTooth);
library.add(faUserDoctor);
library.add(faChair);

const app = createApp(App);
app.config.globalProperties.$appState = reactive({ theme: 'lara-light-indigo', darkTheme: false });
const pinia = createPinia();
let data = {
  navigation: [
    {
      label: 'Система управления',
      items: [
        {
          label: 'Пользователи',
          icon: 'pi pi-fw pi-users',
          to: '/user-profiles',
        },
        {
          label: 'Клиники',
          icon: 'pi pi-fw pi-building',
          to: '/clinics',
        },
        {
          label: 'Симптомы',
          icon: 'pi pi-fw pi-check-square',
          to: '/symptoms',
        },
        {
          label: 'Зубы',
          icon: 'fas fa-tooth pl-1 pr-1',
          to: '/teeth',
        },
        {
          label: 'Причины отмены',
          icon: 'fas fa-tooth pl-1 pr-1',
          to: '/cancellation-reason',
        },
        {
          label: 'Источники информации',
          icon: 'pi pi-book',
          to: '/information-sources',
        },
      ],
    },
  ],
};

app.provide('config', data);
app.use(primevue);
app.use(router);
app.use(pinia);
app.component('font-awesome-icon', FontAwesomeIcon);
app.directive('code', CodeHighlight);

app.mount('#app');

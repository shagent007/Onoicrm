import { createApp } from "vue";
import { createMetaManager } from "vue-meta";
import App from "./App.vue";
import primevue from "./plugins/primevue";
import router from "./router";
import { createPinia } from "pinia";
import "./assets/scss/main.scss";
import "primevue/resources/primevue.min.css"
import "primeflex/primeflex.min.css"
import Vue3TouchEvents from "vue3-touch-events";


const app = createApp(App);
const pinia = createPinia();
app.use(pinia);
app.use(router);
app.use(primevue);
app.use(createMetaManager());
app.use(Vue3TouchEvents)

app.mount("#app");

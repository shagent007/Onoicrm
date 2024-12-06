import { createRouter, createWebHistory, RouteLocationNormalized } from 'vue-router';
import { useAuthStore } from './store/useAuthStore';

const routes = [
  {
    path: "/hello",
    name: "hello",
    component: () => import("./pages/hello.vue"),
    meta: {
      layout: "empty",
      allowAnonymus:true
    },
  },
  {
    path: '/',
    name: 'dashboard',
    component: () => import('./pages/index.vue'),
  },
  {
    path: '/auth/login',
    name: 'login',
    component: () => import('./pages/login.vue'),
    meta: {
      layout: 'login',
    },
  },

  {
    path: '/clinics',
    name: 'clinic',
    component: () => import('./pages/clinics/index.vue'),

  },
  {
    path: '/clinics/:id',
    name: 'clinic-id',
    component: () => import('./pages/clinics/id.vue'),
    children: [
      {
        path: 'doctors',
        name: 'clinic-id-doctor',
        component: () => import('./pages/clinics/zones/doctors/index.vue')
      },
      {
        path: 'armchairs',
        name: 'clinic-id-armchair',
        component: () => import('./pages/clinics/zones/armchairs/index.vue')
      },
      {
        path: 'patients',
        name: 'clinic-id-patient',
        component: () => import('./pages/clinics/zones/patients/index.vue')
      },
      {
        path: 'services',
        name: 'clinic-id-service',
        component: () => import('./pages/clinics/zones/services/index.vue')
      }
    ]
  },
  {
    path: '/symptoms',
    name: 'symptom',
    component: () => import('./pages/symptoms/index.vue'),
  },
  {
    path: '/symptoms/:id',
    name: 'symptom-id',
    component: () => import('./pages/symptoms/id.vue'),
  },
  {
    path: '/service-groups',
    name: 'service-groups',
    component: () => import('./pages/service-groups/index.vue'),
  },
  {
    path: '/teeth',
    name: 'tooth',
    component: () => import('./pages/teeth/index.vue'),
  },
  {
    path: '/teeth/:id',
    name: 'tooth-id',
    component: () => import('./pages/teeth/id.vue'),
  },
  {
    path: '/tooth-states',
    name: 'tooth-state',
    component: () => import('./pages/tooth-states/index.vue'),
  },
  {
    path: '/tooth-states/:id',
    name: 'tooth-state-id',
    component: () => import('./pages/tooth-states/id.vue'),
  },
  {
    path: '/cancellation-reason',
    name: 'cancellation-reason',
    component: () => import('./pages/cancellation-reason/index.vue'),
  },
  {
    path: '/information-sources',
    name: 'information-sources',
    component: () => import('./pages/information-sources/index.vue'),
  },
];
const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to: RouteLocationNormalized, {}, next: Function) => {
  const authStore = useAuthStore();
  const successAuth = authStore.tryLogin();
  const route = getNextRoute(to, successAuth);
  next(route);
});

const getNextRoute = (to: RouteLocationNormalized, successAuthenticate: boolean): string | boolean =>
  to.name === 'login' || (to.meta?.allowAnonymus as boolean) || successAuthenticate || '/auth/login';

export default router;

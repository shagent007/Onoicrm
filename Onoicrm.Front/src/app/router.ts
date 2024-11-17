import {createRouter, createWebHistory, RouteLocationNormalized,} from "vue-router";
import {useAuthStore} from "~/modules/auth/stores/useAuthStore";
import {useUserStore} from "~/modules/user/stores/useUserStore";
import {useAppStore} from "~/modules/app/stores/useAppStore";
import {useClinicStore} from "~/modules/clinic/stores/useClinicStore";
import {CompanyType} from "~/modules/clinic/Clinic";

const beforeEnter = async (to: RouteLocationNormalized, {}, next: Function) => {
  const authStore = useAuthStore();
  const clinicStore = useClinicStore();
  const appStore = useAppStore();
  if (to.meta.title) {
      document.title = to.meta.title as string;
      appStore.pageTitle = to.meta.title as string;
  }
  isRouterActive(to.name as string)
  const userStore = useUserStore();
  const successAuth = authStore.tryLogin();
  if(successAuth){
    if (!userStore.profile) await userStore.get();
    if(!clinicStore.clinic) await clinicStore.getClinic(userStore.profile.clinicId);
  }

  const route = await getNextRoute(to, successAuth);
  next(route);
};
export const isRouterActive = (to:string)=>{
  const routers: any = document.querySelectorAll('.navbar__router');
  for (const item of routers) {
    item.classList.remove('navbar__router--active');
  }
  if(to == 'patient-id'){
    routers[1].classList.add('navbar__router--active');
  }if(to == 'doctor-id'){
    routers[2].classList.add('navbar__router--active');
  }
}
const getNextRoute = async (to: RouteLocationNormalized, successAuthenticate: boolean): Promise<string | any> => {
  if (to.name === "login") {
    return true;
  }

  if (successAuthenticate) {
    return await getAccess(to);
  }

  return { name: "login" };
};
const getAccess = async (to: RouteLocationNormalized): Promise<boolean> => {
  const userStore = useUserStore();
  if (!userStore.profile) await userStore.get();
  const roles = [
    ...Array.from(to.meta?.roles as string[]),
    "System Administrator",
    "Director",
  ];
  return userStore.hasOptionalRoles(roles);
};

const routes = [
  {
    path: "/",
    name: "index",
    component: () => import("~/pages/bookings/index.vue"),
    beforeEnter,
    meta: {
      roles: ["Administrator", "Dentist"],
      title: "Главная",
      title2:"Главная",
    },
  },
  {
    path: "/login/auth",
    name: "login",
    component: () => import("../pages/login.vue"),
    meta: {
      layout: "login",
      title: "Вход в систему",
    },
  },
  {
    path: "/patients",
    name: "user",
    component: () => import("~/pages/patients/index.vue"),
    beforeEnter,
    meta: {
      roles: ["Administrator", "Dentist"],
      title: "Пациенты",
      title2: "Клиенты",
    },
  },
  {
    path: "/patients/:id",
    name: "patient-id",
    component: () => import("~/pages/patients/id.vue"),
    beforeEnter,
    meta: {
      roles: ["Administrator", "Dentist"],
    },
  },
  {
    path: "/doctors",
    name: "doctor",
    component: () => import("~/pages/doctors/index.vue"),
    beforeEnter,
    meta: {
      roles: ["Administrator", "Dentist"],
      title: "Сотрудники",
    },
  },
  {
    path: "/doctors/:id",
    name: "doctor-id",
    component: () => import("~/pages/doctors/id.vue"),
    beforeEnter,
    meta: {
      roles: ["Administrator"],
    },
  },
  {
    path: "/services",
    name: "service",
    component: () => import("~/pages/services/index.vue"),
    beforeEnter,
    meta: {
      roles: ["Administrator", "Dentist"],
      title: "Услуги",
    },
  },
];
const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;

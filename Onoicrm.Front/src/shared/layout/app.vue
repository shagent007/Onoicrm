<template>
  <div class="app">
      <aside class="navbar">
      <div class="flex flex-column justify-content-between h-screen">
        <router-link to="/">
          <logo/>
        </router-link>

        <div class="mt-5">
          <router-link class="navbar__router my-5" to="/">
            <home></home>
          </router-link>
          <router-link class="navbar__router my-5" to="/patients">
            <patient></patient>
          </router-link>
          <router-link class="navbar__router my-5" to="/doctors">
            <doctor></doctor>
          </router-link>
          <router-link class="navbar__router my-5" to="/services">
            <service></service>
          </router-link>
        </div>
        <div>
          <a
              class="navbar__router"
              href="https://wa.me/+996553871595"
              target="_blank"
          >
            <info></info>
          </a>
          <router-link
              class="navbar__router"
              to="/login/auth"
              @click="authStore.logout()"
          >
            <logout></logout>
          </router-link>
        </div>
      </div>
    </aside>
      <main class="main h-full">
        <div class="mobile-header column-gap-2">
          <img src="/img/logo.svg" alt="" class="mobile-header__logo">
          <div
              class="mobile-header-nav"
              :class="navMenu ? 'mobile-header-nav--top-border' : 'mobile-header-nav--border'"
              @click="navMenu = !navMenu"
              @mouseleave="navMenu = false"
              @focus="navMenu = false"
          >
            <div class="flex align-items-center justify-content-center px-3 py-0">
              <h1 class="header__title px-0 mr-1">{{appStore?.pageTitle}}</h1>
              <i v-if="!navMenu" class="pi pi-angle-down h-fit text-white"></i>
            </div>
            <div
                v-if="navMenu"
                class="mobile-header-nav__content pt-0 px-3"
                :class="navMenu ? 'mobile-header-nav__content-border' : ''"
            >
              <div  v-for="item in pages">
                <p
                    v-if="item.label != appStore.pageTitle"
                    class="header__title pb-3"
                    @click="
                      navMenu = true;
                      router.push(item.to)
                    "
                >
                  {{item.label}}
                </p>
              </div>
            </div>
          </div>
          <img src="/img/burger.svg" alt="" class="mobile-header__burger cursor-pointer" @click="appStore.drawer = true">
        </div>

        <div class="header">
          <div class="header__content">
            <h1 v-if="width > 992" class="header__title">{{ appStore?.pageTitle }}</h1>
            <p v-if="appStore?.pageSubtitle" class="header__subtitle text-xs sm:text-base text-blue-200">{{ appStore?.pageSubtitle }}</p>
          </div>

          <div class="header__items">
            <SearchField
                class-name="mr-4 block"
                v-if="width > 992"
                :model-value="appStore.searchText"
                @update:model-value="newValue => appStore.searchText = newValue"
            />
            <Button
                v-if="appStore.buttons.length > 0"
                v-for="button in appStore.buttons"
                v-bind="button.attrs"
                class="px-4 border-round-xl header__button hidden lg:flex"
                @click="button.action()"
            />
            <ProfileField class-name="header__profile"/>
          </div>
        </div>
        <SearchField
            class-name="mb-4 block mobile-search"
            v-if="width < 992"
            :model-value="appStore.searchText"
            @update:model-value="newValue => appStore.searchText = newValue"
        />
        <slot></slot>
        <div class="card flex justify-content-center">
          <Sidebar v-model:visible="appStore.drawer" position="left" class="w-20rem pl-3">
            <div class="flex flex-column justify-content-between h-full mobile-menu">
              <div class="flex flex-column">
                <ProfileField/>
                <div class="flex align-items-center mobile-menu__item mt-3">
                  <router-link class="navbar__router" to="/info">
                    <info></info>
                  </router-link>
                  <router-link to="/services" class="navbar__router-caption">Поддержка</router-link>
                </div>
                <div  @click="authStore.logout()" class="flex align-items-center mobile-menu__item mt-3">
                  <div class="navbar__router" @click="authStore.logout()">
                    <logout></logout>
                  </div>
                  <div  class="navbar__router-caption">Выйти</div>
                </div>
              </div>
            </div>
          </Sidebar>
        </div>
      </main>
  </div>
</template>
<script setup lang="ts">
import logo from "../images/logo.svg"
import home from "../images/home.svg"
import info from "../images/info.svg"
import logout from "../images/logout.svg"
import patient from "../images/patient.svg"
import doctor from "../images/doctor.svg"
import service from "../images/service.svg"
import {computed, onMounted, onUnmounted, ref, watch} from "vue";
import {useAppStore} from "~/modules/app/stores/useAppStore";
import SearchField from "~/shared/ui/search-field.vue";
import ProfileField from "~/modules/booking/components/profile.vue";
import {useRoute, useRouter} from "vue-router";
import {useAuthStore} from "~/modules/auth/stores/useAuthStore";
import {useWindowSize} from "@vueuse/core";
const { width } = useWindowSize()

const router = useRouter();
const appStore = useAppStore();
const authStore = useAuthStore();
const navMenu = ref(false)
const pages = ref([
  {
    label: 'Главная',
    to: '/',
  },
  {
    label: 'Пациенты',
    to: '/patients'
  },
  {
    label: 'Сотрудники',
    to: '/doctors'
  },
  {
    label: 'Услуги',
    to: '/services'
  },
])


</script>

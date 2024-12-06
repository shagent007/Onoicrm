<template>
  <div :class="containerClass" @click="onWrapperClick">
    <AppTopBar @menu-toggle="onMenuToggle" />
    <div class="layout-sidebar" @click="onSidebarClick">
      <AppMenu :model="config.navigation" @menuitem-click="onMenuItemClick" />
    </div>

    <div class="layout-main-container">
      <div class="layout-main">
        <slot></slot>
      </div>
      <AppFooter />
    </div>

    <transition name="layout-mask">
      <div class="layout-mask p-component-overlay" v-if="mobileMenuActive"></div>
    </transition>
  </div>
</template>

<script setup>
const config = {
  navigation: [
    {
      label: 'Система управления',
      items: [
        {
          label: 'Клиники',
          icon: 'pi pi-fw pi-building',
          to: '/clinics',
        },
        {
          label: 'Категории услуг',
          icon: 'pi pi-fw pi-folder',
          to: '/service-groups',
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
          label: 'Состоянии зуба',
          icon: 'pi pi-sync pl-1 pr-1',
          to: '/tooth-states',
        },
        {
          "label": "Причины отмены",
          "icon": "pi pi-times-circle pl-1 pr-1",
          "to": "/cancellation-reason"
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
</script>

<script>
import AppTopBar from './components/AppTopbar.vue';
import AppMenu from './components/AppMenu.vue';
import AppConfig from './components/AppConfig.vue';
import AppFooter from './components/AppFooter.vue';

export default {
  data() {
    return {
      layoutMode: 'static',
      staticMenuInactive: false,
      overlayMenuActive: false,
      mobileMenuActive: false,
    };
  },
  watch: {
    $route() {
      this.menuActive = false;
      this.$toast.removeAllGroups();
    },
  },
  methods: {
    onWrapperClick() {
      if (!this.menuClick) {
        this.overlayMenuActive = false;
        this.mobileMenuActive = false;
      }

      this.menuClick = false;
    },
    onMenuToggle() {
      this.menuClick = true;

      if (this.isDesktop()) {
        if (this.layoutMode === 'overlay') {
          if (this.mobileMenuActive === true) {
            this.overlayMenuActive = true;
          }

          this.overlayMenuActive = !this.overlayMenuActive;
          this.mobileMenuActive = false;
        } else if (this.layoutMode === 'static') {
          this.staticMenuInactive = !this.staticMenuInactive;
        }
      } else {
        this.mobileMenuActive = !this.mobileMenuActive;
      }

      event.preventDefault();
    },
    onSidebarClick() {
      this.menuClick = true;
    },
    onMenuItemClick(event) {
      if (event.item && !event.item.items) {
        this.overlayMenuActive = false;
        this.mobileMenuActive = false;
      }
    },
    onLayoutChange(layoutMode) {
      this.layoutMode = layoutMode;
    },
    addClass(element, className) {
      if (element.classList) element.classList.add(className);
      else element.className += ' ' + className;
    },
    removeClass(element, className) {
      if (element.classList) element.classList.remove(className);
      else element.className = element.className.replace(new RegExp('(^|\\b)' + className.split(' ').join('|') + '(\\b|$)', 'gi'), ' ');
    },
    isDesktop() {
      return window.innerWidth >= 992;
    },
    isSidebarVisible() {
      if (this.isDesktop()) {
        if (this.layoutMode === 'static') return !this.staticMenuInactive;
        else if (this.layoutMode === 'overlay') return this.overlayMenuActive;
      }

      return true;
    },
  },
  computed: {
    containerClass() {
      return [
        'layout-wrapper',
        {
          'layout-overlay': this.layoutMode === 'overlay',
          'layout-static': this.layoutMode === 'static',
          'layout-static-sidebar-inactive': this.staticMenuInactive && this.layoutMode === 'static',
          'layout-overlay-sidebar-active': this.overlayMenuActive && this.layoutMode === 'overlay',
          'layout-mobile-sidebar-active': this.mobileMenuActive,
          'p-input-filled': this.$primevue.config.inputStyle === 'filled',
          'p-ripple-disabled': this.$primevue.config.ripple === false,
          'layout-theme-light': this.$appState.theme.startsWith('saga'),
        },
      ];
    },
    logo() {
      return this.$appState.darkTheme ? 'images/logo-white.svg' : 'images/logo.svg';
    },
  },
  beforeUpdate() {
    if (this.mobileMenuActive) this.addClass(document.body, 'body-overflow-hidden');
    else this.removeClass(document.body, 'body-overflow-hidden');
  },
  components: {
    AppTopBar: AppTopBar,
    AppMenu: AppMenu,
    AppConfig: AppConfig,
    AppFooter: AppFooter,
  },
};
</script>

<style lang="scss">
@import '../App.scss';
</style>
